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
    public partial class Customer_update_reservation : Form
    {
        public Customer_update_reservation()
        {
            InitializeComponent();
            int ssn;
            bool stringSsn = int.TryParse(Session.CurrentSession.Ssn, out ssn);
            SqlConnection con = Session.CurrentSession.GetConnection();
            string query = @"
                        SELECT f.Flight_number, f.Departure_date, f.Arrival_date,
                               f.Class_a_num, f.Class_b_num, f.Class_a_price, f.Class_b_price,
                               f.Source, f.Destination, r.Class, r.Num_of_tickets
                        FROM FLIGHT f INNER JOIN RESERVE r 
                        ON f.Flight_number = r.Flight_number
                        WHERE r.Ssn = @Ssn";
            SqlCommand command = new SqlCommand(query, con);
            command.Parameters.AddWithValue("@Ssn", ssn);

            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                DataTable reservedFlightsTable = new DataTable();
                adapter.Fill(reservedFlightsTable);
                dataGridView1.DataSource = reservedFlightsTable;

                if (reservedFlightsTable.Rows.Count == 0)
                {
                    MessageBox.Show("No reserved flights found for this customer.");
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            CustomerForm customer = new CustomerForm();
            customer.ShowDialog();
            this.Close();
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
            SqlConnection con = Session.CurrentSession.GetConnection();
            try
            {
                string queryCheckReservation = @"
                       SELECT Num_of_tickets 
                       FROM RESERVE 
                       WHERE Ssn = @Ssn AND Flight_number = @FlightNumber";

                SqlCommand commandCheckReservation = new SqlCommand(queryCheckReservation, con);
                commandCheckReservation.Parameters.AddWithValue("@Ssn", ssn);
                commandCheckReservation.Parameters.AddWithValue("@FlightNumber", flightNum);

                int numTickets = 0;
                using (SqlDataReader reader = commandCheckReservation.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        numTickets = (int)reader["Num_of_tickets"];
                    }
                }

                if (numTickets == 0)
                {
                    MessageBox.Show("No reservation found for the that flight.");
                    return;
                }

                string queryGetOldClass = @"
                       SELECT Class 
                       FROM RESERVE 
                       WHERE Ssn = @Ssn AND Flight_number = @FlightNumber";

                SqlCommand commandGetOldClass = new SqlCommand(queryGetOldClass, con);
                commandGetOldClass.Parameters.AddWithValue("@Ssn", ssn);
                commandGetOldClass.Parameters.AddWithValue("@FlightNumber", flightNum);

                string oldClassValue = commandGetOldClass.ExecuteScalar()?.ToString();
                if (string.IsNullOrEmpty(oldClassValue))
                {
                    MessageBox.Show("No reservation found for the that flight.");
                    return;
                }
                string queryUpdateReservation = @"
                       UPDATE RESERVE 
                       SET Class = @Class 
                       WHERE Ssn = @Ssn AND Flight_number = @FlightNumber";

                SqlCommand commandUpdateReservation = new SqlCommand(queryUpdateReservation, con);
                commandUpdateReservation.Parameters.AddWithValue("@Class", selectedClass);
                commandUpdateReservation.Parameters.AddWithValue("@Ssn", ssn);
                commandUpdateReservation.Parameters.AddWithValue("@FlightNumber", flightNum);

                commandUpdateReservation.ExecuteNonQuery();

                string oldClass = oldClassValue == "A" ? "Class_a_num" : "Class_b_num";
                string newClass = selectedClass == "A" ? "Class_a_num" : "Class_b_num";

                string queryUpdateOldClassCount = $@"
                       UPDATE FLIGHT 
                       SET {oldClass} = {oldClass} - @NumTickets
                       WHERE Flight_number = @FlightNumber";

                SqlCommand commandUpdateOldClassCount = new SqlCommand(queryUpdateOldClassCount, con);
                commandUpdateOldClassCount.Parameters.AddWithValue("@NumTickets", numTickets);
                commandUpdateOldClassCount.Parameters.AddWithValue("@FlightNumber", flightNum);

                commandUpdateOldClassCount.ExecuteNonQuery();

                string queryUpdateNewClassCount = $@"
                       UPDATE FLIGHT 
                       SET {newClass} = {newClass} + @NumTickets
                       WHERE Flight_number = @FlightNumber";

                SqlCommand commandUpdateNewClassCount = new SqlCommand(queryUpdateNewClassCount, con);
                commandUpdateNewClassCount.Parameters.AddWithValue("@NumTickets", numTickets);
                commandUpdateNewClassCount.Parameters.AddWithValue("@FlightNumber", flightNum);
                
                commandUpdateNewClassCount.ExecuteNonQuery();

                MessageBox.Show("Reservation updated successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void Customer_update_reservation_Load(object sender, EventArgs e)
        {

        }
    }
}
