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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FlightReservationGUI
{
    public partial class Admin_update_flight : Form
    {
        private bool isDepartureDateModified = false;
        private bool isArrivalDateModified = false;
        public Admin_update_flight()
        {
            InitializeComponent();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "MM/dd/yyyy HH:mm";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "MM/dd/yyyy HH:mm";
            try
            {
                string query = "SELECT * FROM FLIGHT";
                SqlConnection con = Session.CurrentSession.GetConnection();
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, con))
                {
                    DataTable flightTable = new DataTable();
                    adapter.Fill(flightTable);
                    dataGridView1.DataSource = flightTable;
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
            AdminForm admin = new AdminForm();
            admin.ShowDialog();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int flightNum;
            bool checkFlightNum = int.TryParse(textBox2.Text, out flightNum);
            string source = textBox3.Text;
            string destination = textBox4.Text;
            //int reqNumSeats;
            //bool checkReqNum = int.TryParse(textBox5.Text, out reqNumSeats);
            int classAPrice;
            bool checkClassA = int.TryParse(textBox6.Text, out classAPrice);
            int classBPrice;
            bool checkClassB = int.TryParse(textBox7.Text, out classBPrice);
            DateTime arrival = dateTimePicker1.Value;
            DateTime departure = dateTimePicker2.Value;
            if (!checkFlightNum)
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
            //else if (!checkReqNum)
            //{
            //    MessageBox.Show("Invalid ReqNumOfSeats");
            //    return;
            //}
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
            else if (DoesFlightNumExist(flightNum))
            {
                string query = "UPDATE FLIGHT SET Departure_date = @Departure_date, Arrival_date = @Arrival_date, Source = @Source, Destination = @Destination, Class_a_price = @Class_a_price, Class_b_price = @Class_b_price WHERE Flight_number = @Flight_number ;";
                SqlConnection con = Session.CurrentSession.GetConnection();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@Flight_number", flightNum);
                    command.Parameters.AddWithValue("@Departure_date", departure);
                    command.Parameters.AddWithValue("@Arrival_date", arrival);
                    command.Parameters.AddWithValue("@Source", source);
                    command.Parameters.AddWithValue("@Destination", destination);
                    //command.Parameters.AddWithValue("@Required_num_of_seats", reqNumSeats);
                    command.Parameters.AddWithValue("@Class_a_price", classAPrice);
                    command.Parameters.AddWithValue("@Class_b_price", classBPrice);

                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Updating successful!");
            }
            else
            {
                MessageBox.Show("Flight Does Not Exist");
            }
        }

        private bool DoesFlightNumExist(int flightNum)
        {
            SqlConnection con = Session.CurrentSession.GetConnection();
            string query = "SELECT COUNT(*) FROM FLIGHT WHERE Flight_number = @Flight_number";
            using (SqlCommand command = new SqlCommand(query, con))
            {
                command.Parameters.AddWithValue("@Flight_number", flightNum);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        private void Admin_update_flight_Load(object sender, EventArgs e)
        {

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
