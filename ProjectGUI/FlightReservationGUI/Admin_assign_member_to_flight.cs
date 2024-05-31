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
    public partial class Admin_assign_member_to_flight : Form
    {
        public Admin_assign_member_to_flight()
        {
            InitializeComponent();
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
                query = "SELECT * FROM CREW_MEMBER";
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, con))
                {
                    DataTable memberTable = new DataTable();
                    adapter.Fill(memberTable);
                    dataGridView2.DataSource = memberTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void Admin_assign_member_to_flight_Load(object sender, EventArgs e)
        {

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
            int ssn;
            bool checkSsn = int.TryParse(textBox1.Text, out ssn);
            int flightNum;
            bool checkFlightNum = int.TryParse(textBox2.Text, out flightNum);
            if (!checkSsn)
            {
                MessageBox.Show("Invalid Ssn");
                return;
            }
            else if (!checkFlightNum)
            {
                MessageBox.Show("Invalid Flight Number");
                return;
            }
            if(IsExist(ssn, flightNum))
            {
                DateTime departureDate;
                DateTime arrivalDate;
                if(GetDate(flightNum, out departureDate, out arrivalDate))
                {
                    if (CheckFree(ssn, departureDate, arrivalDate))
                    {
                        string query = "INSERT INTO MANAGE_A (Ssn, Flight_number) VALUES (@Ssn, @Flight_number)";
                        SqlConnection con = Session.CurrentSession.GetConnection();
                        using (SqlCommand command = new SqlCommand(query, con))
                        {
                            command.Parameters.AddWithValue("@Ssn", ssn);
                            command.Parameters.AddWithValue("@Flight_number", flightNum);

                            command.ExecuteNonQuery();
                        }

                        MessageBox.Show("Crew member assigned to the flight successfully!");
                    }
                    else
                    {
                        MessageBox.Show("Crew member is not available in that time");
                    }
                }
            }
            else
            {
                MessageBox.Show("Crew member or Flight Is Not Exist");
            }
        }

        private bool IsExist(int ssn, int flightNumber)
        {
            bool checkFlight = false;
            bool checkSsn = false;
            string query1 = "SELECT COUNT(*) FROM FLIGHT WHERE Flight_number = @Flight_number";
            SqlConnection con = Session.CurrentSession.GetConnection();
            using (SqlCommand command = new SqlCommand(query1, con))
            {
                command.Parameters.AddWithValue("@Flight_number", flightNumber);

                int count = (int)command.ExecuteScalar();
                if(count > 0)
                {
                    checkFlight = true;
                }
                else
                {
                    checkFlight = false;
                }
            }
            string query2 = "SELECT COUNT(*) FROM CREW_MEMBER WHERE Ssn = @Ssn";
            using (SqlCommand command = new SqlCommand(query2, con))
            {
                command.Parameters.AddWithValue("@Ssn", ssn);

                int count = (int)command.ExecuteScalar();
                if (count > 0)
                {
                    checkSsn = true;
                }
                else
                {
                    checkSsn = false;
                }
            }
            if(checkFlight == true && checkSsn == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool GetDate(int flightNumber, out DateTime departureDate, out DateTime arrivalDate)
        {
            string getFlightData = "SELECT Departure_date, Arrival_date FROM FLIGHT WHERE Flight_number = @Flight_number";
            SqlConnection con = Session.CurrentSession.GetConnection();
            using (SqlCommand getCommand = new SqlCommand(getFlightData, con))
            {
                getCommand.Parameters.AddWithValue("@Flight_number", flightNumber);
                using (SqlDataReader reader = getCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        departureDate = reader.GetDateTime(0);
                        arrivalDate = reader.GetDateTime(1);
                        return true;
                    }
                }
            }
            departureDate = DateTime.MinValue;
            arrivalDate = DateTime.MinValue;
            return false;
        }

        private bool CheckFree(int ssn, DateTime departureDate, DateTime arrivalDate)
        {
            SqlConnection con = Session.CurrentSession.GetConnection();
            string query = @"
                    SELECT COUNT(*) 
                    FROM FLIGHT f JOIN MANAGE_A m 
                    ON f.Flight_number = m.Flight_number
                    WHERE m.Ssn = @Ssn
                    AND (
                        (@Departure_date BETWEEN f.Departure_date AND f.Arrival_date)
                        OR
                        (@Arrival_date BETWEEN f.Departure_date AND f.Arrival_date)
                        OR
                        (f.Departure_date BETWEEN @Departure_date AND @Arrival_date)
                        OR
                        (f.Arrival_date BETWEEN @Departure_date AND @Arrival_date)
                      )";
            using (SqlCommand command = new SqlCommand(query, con))
            {
                command.Parameters.AddWithValue("@Ssn", ssn);
                command.Parameters.AddWithValue("@Departure_date", departureDate);
                command.Parameters.AddWithValue("@Arrival_date", arrivalDate);

                int count = (int)command.ExecuteScalar();
                if(count > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    }
}
