using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace FlightReservationGUI
{
    public partial class Customer_reserve_flight : Form
    {
        public Customer_reserve_flight()
        {
            InitializeComponent();
            try
            {
                int ssn;
                bool stringSsn = int.TryParse(Session.CurrentSession.Ssn, out ssn);
                SqlConnection con = Session.CurrentSession.GetConnection();
                string query = @"
                       SELECT f.Flight_number, f.Serial, f.Departure_date, f.Arrival_date,
                              f.Source, f.Destination, f.Required_num_of_seats,
                              f.Class_a_price, f.Class_b_price
                       FROM FLIGHT f LEFT JOIN RESERVE r 
                       ON f.Flight_number = r.Flight_number AND r.Ssn = @Ssn
                       WHERE (f.Class_a_num + f.Class_b_num) < f.Required_num_of_seats
                             AND r.Ssn IS NULL";

                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@Ssn", ssn);

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable flightTable = new DataTable();
                    adapter.Fill(flightTable);
                    dataGridView1.DataSource = flightTable;
                    if (flightTable.Rows.Count == 0)
                    {
                        MessageBox.Show("No available flights for reservation.");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            CustomerForm customer = new CustomerForm();
            customer.ShowDialog();
            this.Close();
        }

        private void Customer_reserve_flight_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int ssn;
            bool stringSsn = int.TryParse(Session.CurrentSession.Ssn, out ssn);
            string source = textBox1.Text;
            string destination = textBox2.Text;
            if (string.IsNullOrEmpty(source))
            {
                MessageBox.Show("Invalid Source");
                return;
            }
            else if (string.IsNullOrEmpty(destination))
            {
                MessageBox.Show("Invalid Destination");
                return;
            }
            try
            {
                string query = @"
            SELECT f.Flight_number, f.Serial, f.Departure_date, f.Arrival_date, f.Source, f.Destination,
                   f.Class_a_num, f.Class_b_num, f.Class_a_price, f.Class_b_price
            FROM FLIGHT f
            LEFT JOIN RESERVE r ON f.Flight_number = r.Flight_number AND r.Ssn = @Ssn
            WHERE (f.Class_a_num + f.Class_b_num) < f.Required_num_of_seats
                AND f.Source = @Source
                AND f.Destination = @Destination
                AND r.Ssn IS NULL";

                SqlConnection con = Session.CurrentSession.GetConnection();
                SqlCommand command = new SqlCommand(query, con);
                command.Parameters.AddWithValue("@Ssn", ssn);
                command.Parameters.AddWithValue("@Source", source);
                command.Parameters.AddWithValue("@Destination", destination);

                using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                {
                    DataTable flightTable = new DataTable();
                    adapter.Fill(flightTable);

                    if (flightTable.Rows.Count == 0)
                    {
                        MessageBox.Show("No available flights for reservation like your search.");
                        return;
                    }

                    dataGridView2.DataSource = flightTable;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int ssn;
            bool stringSsn = int.TryParse(Session.CurrentSession.Ssn, out ssn);
            int flightNum;
            if (!int.TryParse(textBox3.Text, out flightNum))
            {
                MessageBox.Show("Invalid Flight Number");
                return;
            }
            string selectedClass = comboBox1.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedClass))
            {
                MessageBox.Show("Please select a class (A or B)");
                return;
            }
            int numTickets;
            if (!int.TryParse(textBox4.Text, out numTickets))
            {
                MessageBox.Show("Invalid Number Of Tickets");
                return;
            }
            else if (numTickets <= 0)
            {
                MessageBox.Show("Invalid Number Of Tickets");
                return;
            }

            // Check if the flight is available for reservation
            string queryAvailability = @"
                   SELECT (Class_a_num + Class_b_num) AS TotalReserved, Required_num_of_seats
                   FROM FLIGHT
                   WHERE Flight_number = @FlightNumber";

            SqlConnection con = Session.CurrentSession.GetConnection();
            SqlCommand commandAvailability = new SqlCommand(queryAvailability, con);
            commandAvailability.Parameters.AddWithValue("@FlightNumber", flightNum);

            using (SqlDataReader reader = commandAvailability.ExecuteReader())
            {
                if (reader.Read())
                {
                    int totalReserved = reader.GetInt32(0);
                    int requiredSeats = reader.GetInt32(1);
                    int freeSeats = requiredSeats - totalReserved;

                    if (freeSeats < numTickets)
                    {
                        MessageBox.Show("Not enough seats available for the requested number of tickets.");
                        return;
                    }
                    if (totalReserved >= requiredSeats)
                    {
                        MessageBox.Show("This flight is not available for reservation.");
                        return;
                    }
                }
                else
                {
                    MessageBox.Show("Flight not found.");
                    return;
                }
            }

            // Check if the customer has already reserved this flight
            string queryCheckReservation = @"
                   SELECT COUNT(*) 
                   FROM RESERVE 
                   WHERE Ssn = @Ssn AND Flight_number = @FlightNumber";

            SqlCommand commandCheckReservation = new SqlCommand(queryCheckReservation, con);
            commandCheckReservation.Parameters.AddWithValue("@Ssn", ssn);
            commandCheckReservation.Parameters.AddWithValue("@FlightNumber", flightNum);

            int Count = (int)commandCheckReservation.ExecuteScalar();
            if (Count > 0)
            {
                MessageBox.Show("You have already reserved this flight.");
                return;
            }

            // Check the number of dependents for the customer
            string queryDependentsCount = "SELECT COUNT(*) FROM DEPENDENT WHERE Ssn = @Ssn";
            SqlCommand commandDependentsCount = new SqlCommand(queryDependentsCount, con);
            commandDependentsCount.Parameters.AddWithValue("@Ssn", ssn);

            int dependentsCount = (int)commandDependentsCount.ExecuteScalar();
            if (numTickets > (dependentsCount + 1))
            {
                MessageBox.Show($"The number of tickets exceeds the allowed limit. You can only reserve up to {(dependentsCount + 1)} tickets.");
                return;
            }

            string queryReserveFlight = "INSERT INTO RESERVE (Ssn, Flight_number, Class, Num_of_tickets) VALUES (@Ssn, @FlightNumber, @Class, @Num_of_tickets)";
            SqlCommand commandReserveFlight = new SqlCommand(queryReserveFlight, con);
            commandReserveFlight.Parameters.AddWithValue("@Ssn", ssn);
            commandReserveFlight.Parameters.AddWithValue("@FlightNumber", flightNum);
            commandReserveFlight.Parameters.AddWithValue("@Class", selectedClass);
            commandReserveFlight.Parameters.AddWithValue("@Num_of_tickets", numTickets);
            commandReserveFlight.ExecuteNonQuery();

            // Update the class count in the flight based on the selected class
            string classColumnName = selectedClass == "A" ? "Class_a_num" : "Class_b_num";
            string queryUpdateClassCount = $"UPDATE FLIGHT SET {classColumnName} = {classColumnName} + {numTickets} WHERE Flight_number = @FlightNumber";
            SqlCommand commandUpdateClassCount = new SqlCommand(queryUpdateClassCount, con);
            commandUpdateClassCount.Parameters.AddWithValue("@FlightNumber", flightNum);
            commandUpdateClassCount.ExecuteNonQuery();

            MessageBox.Show("Flight reserved successfully!");
        }
    }
}
