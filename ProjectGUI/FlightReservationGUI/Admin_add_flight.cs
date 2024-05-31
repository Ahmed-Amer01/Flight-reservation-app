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
using System.Reflection;

namespace FlightReservationGUI
{
    public partial class Admin_add_flight : Form
    {
        private bool isDepartureDateModified = false;
        private bool isArrivalDateModified = false;

        public Admin_add_flight()
        {
            InitializeComponent();
            try
            {
                string query = "SELECT * FROM AIRPLANE WHERE Serial NOT IN (SELECT Serial FROM FLIGHT)";
                SqlConnection con = Session.CurrentSession.GetConnection();
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, con))
                {
                    DataTable airplaneTable = new DataTable();
                    adapter.Fill(airplaneTable);
                    dataGridView1.DataSource = airplaneTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/dd/yyyy HH:mm";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "MM/dd/yyyy HH:mm";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminForm admin = new AdminForm();
            admin.ShowDialog();
            this.Close();
        }

        private void Admin_add_flight_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int serial;
            bool checkSerial = int.TryParse(textBox1.Text, out serial);
            int flightNum;
            bool checkFlightNum = int.TryParse(textBox2.Text, out flightNum);
            string source = textBox3.Text;
            string destination = textBox4.Text;
            int reqNumSeats;
            bool checkReqNum = int.TryParse(textBox5.Text, out reqNumSeats);
            int classAPrice;
            bool checkClassA = int.TryParse(textBox6.Text, out classAPrice);
            int classBPrice;
            bool checkClassB = int.TryParse(textBox7.Text, out classBPrice);
            DateTime arrival = dateTimePicker1.Value;
            DateTime departure = dateTimePicker2.Value;
            if (!checkSerial)
            {
                MessageBox.Show("Invalid Serial");
                return;
            }
            else if (!checkFlightNum)
            {
                MessageBox.Show("Invalid FlightNumber");
                return;
            }
            else if (string.IsNullOrEmpty(source))
            {
                MessageBox.Show("Invalid Source");
                return;
            }
            else if (string.IsNullOrEmpty(destination))
            {
                MessageBox.Show("Invalid Destination");
                return;
            }
            else if (!checkReqNum)
            {
                MessageBox.Show("Invalid ReqNumOfSeats");
                return;
            }
            else if (!checkClassA)
            {
                MessageBox.Show("Invalid Price");
                return;
            }
            else if (!checkClassB)
            {
                MessageBox.Show("Invalid Price");
                return;
            }
            else if (!isDepartureDateModified || !isArrivalDateModified)
            {
                MessageBox.Show("Please select both Departure and Arrival dates.");
                return;
            }
            else if (arrival <= departure)
            {
                MessageBox.Show("Arrival date must be after departure date.");
                return;
            }
            else if (DoesFlightNumExist(flightNum, serial))
            {
                return;
            }
            if (DoesSerialExist(serial))
            {
                try
                {
                    int capacity = GetCapacity(serial);
                    if (reqNumSeats > capacity)
                    {
                        MessageBox.Show("Required number of seats exceeds the airplane's capacity.");
                        return;
                    }

                    string query = "INSERT INTO FLIGHT (Flight_number, Serial, Departure_date, Arrival_date, Source, Destination, Required_num_of_seats, Class_a_price, Class_b_price) VALUES(@Flight_number, @Serial, @Departure_date, @Arrival_date, @Source, @Destination, @Required_num_of_seats, @Class_a_price, @Class_b_price);";
                    SqlConnection con = Session.CurrentSession.GetConnection();
                    using (SqlCommand command = new SqlCommand(query, con))
                    {
                        command.Parameters.AddWithValue("@Serial", serial);
                        command.Parameters.AddWithValue("@Flight_number", flightNum);
                        command.Parameters.AddWithValue("@Departure_date", departure);
                        command.Parameters.AddWithValue("@Arrival_date", arrival);
                        command.Parameters.AddWithValue("@Source", source);
                        command.Parameters.AddWithValue("@Destination", destination);
                        command.Parameters.AddWithValue("@Required_num_of_seats", reqNumSeats);
                        command.Parameters.AddWithValue("@Class_a_price", classAPrice);
                        command.Parameters.AddWithValue("@Class_b_price", classBPrice);

                        command.ExecuteNonQuery();
                    }
                    MessageBox.Show("Adding successful!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("An error occurred: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Airplane Does Not Exist");
            }
        }

        private bool DoesSerialExist(int serial)
        {
            SqlConnection con = Session.CurrentSession.GetConnection();
            string query = "SELECT COUNT(*) FROM AIRPLANE WHERE Serial = @Serial";
            using (SqlCommand command = new SqlCommand(query, con))
            {
                command.Parameters.AddWithValue("@Serial", serial);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        private bool DoesFlightNumExist(int flightNum, int serial)
        {
            SqlConnection con = Session.CurrentSession.GetConnection();
            string queryFlight = "SELECT COUNT(*) FROM FLIGHT WHERE Flight_number = @Flight_number OR Serial = @Serial";
            using (SqlCommand command = new SqlCommand(queryFlight, con))
            {
                command.Parameters.AddWithValue("@Flight_number", flightNum);
                command.Parameters.AddWithValue("@Serial", serial);
                int count = (int)command.ExecuteScalar();
                if(count > 0)
                {
                    MessageBox.Show("The Flight Number is not avaliable or the airplane is not available");
                }
                return count > 0;
            }
        }

        private int GetCapacity(int serial)
        {
            SqlConnection con = Session.CurrentSession.GetConnection();
            string query = "SELECT Capacity FROM AIRPLANE WHERE Serial = @Serial";
            using (SqlCommand command = new SqlCommand(query, con))
            {
                command.Parameters.AddWithValue("@Serial", serial);
                object result = command.ExecuteScalar();
                if (result != null)
                {
                    return (int)result;
                }
                else
                {
                    throw new Exception("Airplane capacity not found.");
                }
            }
        }

        private void DateTimePicker2_ValueChanged(object sender, EventArgs e)
        {
            isDepartureDateModified = true;
        }

        private void DateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            isArrivalDateModified = true;
        }
    }
}
