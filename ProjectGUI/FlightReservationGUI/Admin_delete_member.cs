using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace FlightReservationGUI
{
    public partial class Admin_delete_member : Form
    {
        public Admin_delete_member()
        {
            InitializeComponent();
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
            if (!checkSsn)
            {
                MessageBox.Show("Invalid Ssn");
                return;
            }
            if (DoesSsnExist(ssn))
            {
                string query = "DELETE FROM CREW_MEMBER WHERE Ssn = @Ssn";
                SqlConnection con = Session.CurrentSession.GetConnection();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@Ssn", ssn);

                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Delete successful!");
            }
            else
            {
                MessageBox.Show("The Crew member is not exist");
            }
        }

        private bool DoesSsnExist(int ssn)
        {
            SqlConnection con = Session.CurrentSession.GetConnection();
            string query = "SELECT COUNT(*) FROM CREW_MEMBER WHERE Ssn = @Ssn";
            using (SqlCommand command = new SqlCommand(query, con))
            {
                command.Parameters.AddWithValue("@Ssn", ssn);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
    }
}
