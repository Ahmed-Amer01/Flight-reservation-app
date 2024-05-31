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
    public partial class Meaningful_report : Form
    {
        public Meaningful_report()
        {
            InitializeComponent();
            SqlConnection con = Session.CurrentSession.GetConnection();
            string flightQuery = "SELECT COUNT(*) FROM FLIGHT";
            using (SqlCommand countCommand = new SqlCommand(flightQuery, con))
            {
                int totalFlights = (int)countCommand.ExecuteScalar();
                label9.Text = $"{totalFlights}";
            }
            string airplaneQuery = "SELECT COUNT(*) FROM AIRPLANE";
            using (SqlCommand countCommand = new SqlCommand(airplaneQuery, con))
            {
                int totalAirplanes = (int)countCommand.ExecuteScalar();
                label8.Text = $"{totalAirplanes}";
            }
            string crewQuery = "SELECT COUNT(*) FROM CREW_MEMBER";
            using (SqlCommand countCommand = new SqlCommand(crewQuery, con))
            {
                int totalCrews = (int)countCommand.ExecuteScalar();
                label10.Text = $"{totalCrews}";
            }
            string earnQuery = "SELECT SUM(Class_a_num * Class_a_price + Class_b_num * Class_b_price) AS TotalEarnings FROM FLIGHT";
            using (SqlCommand countCommand = new SqlCommand(earnQuery, con))
            {
                decimal totalEarn = 0;
                var result = countCommand.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    totalEarn = (decimal)result;
                }
                label11.Text = $"{totalEarn}";
            }
            string adminQuery = "SELECT COUNT(*) FROM ADMIN";
            using (SqlCommand countCommand = new SqlCommand(adminQuery, con))
            {
                int totalAdmins = (int)countCommand.ExecuteScalar();
                label12.Text = $"{totalAdmins}";
            }
            string customerQuery = "SELECT COUNT(*) FROM CUSTOMER";
            using (SqlCommand countCommand = new SqlCommand(customerQuery, con))
            {
                int totalCustomers = (int)countCommand.ExecuteScalar();
                label14.Text = $"{totalCustomers}";
            }
            string dependentQuery = "SELECT COUNT(*) FROM DEPENDENT";
            using (SqlCommand countCommand = new SqlCommand(dependentQuery, con))
            {
                int totalDependents = (int)countCommand.ExecuteScalar();
                label16.Text = $"{totalDependents}";
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
            bool checkFlightNum = int.TryParse(textBox1.Text, out flightNum);
            if (!checkFlightNum)
            {
                MessageBox.Show("Invalid Ssn");
                return;
            }
            if (IsExist(flightNum))
            {

                string query = @"
                    SELECT FLIGHT.Flight_number, FLIGHT.Serial, FLIGHT.Departure_date, FLIGHT.Arrival_date, 
                               FLIGHT.Source, FLIGHT.Destination, 
                               FLIGHT.Class_a_num, FLIGHT.Class_a_price, (FLIGHT.Class_a_num * FLIGHT.Class_a_price) AS Class_a_earnings, 
                               FLIGHT.Class_b_num, FLIGHT.Class_b_price, (FLIGHT.Class_b_num * FLIGHT.Class_b_price) AS Class_b_earnings,
                               ((FLIGHT.Class_a_num * FLIGHT.Class_a_price) + (FLIGHT.Class_b_num * FLIGHT.Class_b_price)) AS Total_earnings
                        FROM FLIGHT
                        WHERE FLIGHT.Flight_number = @FlightNumber";

                SqlConnection con = Session.CurrentSession.GetConnection();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@FlightNumber", flightNum);

                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable flightInfo = new DataTable();
                        adapter.Fill(flightInfo);
                        if (flightInfo.Rows.Count > 0)
                        {
                            dataGridView1.DataSource = flightInfo;
                            MessageBox.Show("Here is the data Of Flight.");
                        }
                        else
                        {
                            MessageBox.Show("No data found for the specified flight number.");
                        }
                    }
                }
                string query2 = @"
                SELECT CREW_MEMBER.Ssn, CREW_MEMBER.Fname, CREW_MEMBER.Lname, CREW_MEMBER.Gender, CREW_MEMBER.Role, CREW_MEMBER.Nationality, CREW_MEMBER.Address, CREW_MEMBER.Salary
                FROM CREW_MEMBER INNER JOIN MANAGE_A 
                ON CREW_MEMBER.Ssn = MANAGE_A.Ssn
                WHERE MANAGE_A.Flight_number = @FlightNumber";
                using (SqlCommand command = new SqlCommand(query2, con))
                {
                    command.Parameters.AddWithValue("@FlightNumber", flightNum);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable crewInfo = new DataTable();
                        adapter.Fill(crewInfo);
                        if (crewInfo.Rows.Count > 0)
                        {
                            dataGridView2.DataSource = crewInfo;
                            MessageBox.Show("Here is the data of its Crew.");
                        }
                        else
                        {
                            MessageBox.Show("There is no Crew Assigned to the flight yet");
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Flight is not exist");
            }
        }

        private bool IsExist(int flightNumber)
        {
            string query = "SELECT COUNT(*) FROM FLIGHT WHERE Flight_number = @Flight_number";
            SqlConnection con = Session.CurrentSession.GetConnection();
            using (SqlCommand command = new SqlCommand(query, con))
            {
                command.Parameters.AddWithValue("@Flight_number", flightNumber);

                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        private void Meaningful_report_Load(object sender, EventArgs e)
        {

        }
    }
}
