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
    public partial class Admin_display_flight_info : Form
    {
        public Admin_display_flight_info()
        {
            InitializeComponent();
            try
            {
                string query = "SELECT * FROM MANAGE_A";
                SqlConnection con = Session.CurrentSession.GetConnection();
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, con))
                {
                    DataTable airplaneTable = new DataTable();
                    adapter.Fill(airplaneTable);
                    dataGridView2.DataSource = airplaneTable;
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
            bool checkFlightNum = int.TryParse(textBox1.Text, out flightNum);
            if (!checkFlightNum)
            {
                MessageBox.Show("Invalid Ssn");
                return;
            }
            if (IsExist(flightNum))
            {

                string query = @"
                    SELECT FLIGHT.*, CREW_MEMBER.Fname, CREW_MEMBER.Lname, CREW_MEMBER.Role
                    FROM FLIGHT, MANAGE_A, CREW_MEMBER
                    WHERE FLIGHT.Flight_number = MANAGE_A.Flight_number 
                          AND MANAGE_A.Ssn = CREW_MEMBER.Ssn
                          AND FLIGHT.Flight_number = @FlightNumber";

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
                            MessageBox.Show("Here is the data.");
                        }
                        else
                        {
                            MessageBox.Show("No data found for the specified flight number.");
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
                return count > 0 ;
            }
        }

        private void Admin_display_flight_info_Load(object sender, EventArgs e)
        {

        }
    }
}
