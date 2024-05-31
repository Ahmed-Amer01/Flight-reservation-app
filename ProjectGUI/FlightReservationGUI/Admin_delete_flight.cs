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
    public partial class Admin_delete_flight : Form
    {
        public Admin_delete_flight()
        {
            InitializeComponent();
        }

        private void Admin_delete_flight_Load(object sender, EventArgs e)
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
            int flightNum;
            bool checkFlightNum = int.TryParse(textBox1.Text, out flightNum);
            if (!checkFlightNum)
            {
                MessageBox.Show("Invalid FlightNumber");
                return;
            }
            if (DoesFlightNumExist(flightNum))
            {
                string query = "DELETE FROM FLIGHT WHERE Flight_number = @Flight_number";
                SqlConnection con = Session.CurrentSession.GetConnection();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@Flight_number", flightNum);

                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Delete successful!");
            }
            else
            {
                MessageBox.Show("The Flight is not exist");
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
    }
}
