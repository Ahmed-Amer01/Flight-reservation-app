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
    public partial class Admin_remove_member_from_flight : Form
    {
        public Admin_remove_member_from_flight()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminForm admin = new AdminForm();
            admin.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
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
                MessageBox.Show("Invalid FlightNumber");
                return;
            }
            if (DoesDataExist(ssn, flightNum))
            {
                string query = "DELETE FROM MANAGE_A WHERE Flight_number = @Flight_number AND Ssn = @Ssn";
                SqlConnection con = Session.CurrentSession.GetConnection();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@Flight_number", flightNum);
                    command.Parameters.AddWithValue("@Ssn", ssn);

                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Remove successful!");
            }
            else
            {
                MessageBox.Show("This Assign is not exist");
            }
        }

        private bool DoesDataExist(int ssn, int flightNum)
        {
            SqlConnection con = Session.CurrentSession.GetConnection();
            string query = "SELECT COUNT(*) FROM MANAGE_A WHERE Flight_number = @Flight_number AND Ssn = @Ssn";
            using (SqlCommand command = new SqlCommand(query, con))
            {
                command.Parameters.AddWithValue("@Flight_number", flightNum);
                command.Parameters.AddWithValue("@Ssn", ssn);

                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
    }
}
