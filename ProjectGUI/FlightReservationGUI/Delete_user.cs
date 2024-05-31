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
    public partial class Delete_user : Form
    {
        public Delete_user()
        {
            InitializeComponent();
        }

        private void Delete_user_Load(object sender, EventArgs e)
        {

        }

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 reg = new Form1();
            reg.ShowDialog();
            this.Close();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            int ssn;
            bool checkSsn = int.TryParse(textBox4.Text, out ssn);
            if (!checkSsn)
            {
                MessageBox.Show("Invalid ID");
                return;
            }
            if (DoesSsnExistInAdmin(ssn))
            {
                string ConnectionString = "Data Source=DESKTOP-DFCJR49;Initial Catalog=FlightReservation;Integrated Security=True";
                SqlConnection con = new SqlConnection(ConnectionString);
                string query = "DELETE FROM ADMIN WHERE Ssn = @Ssn";
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@Ssn", ssn);

                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Delete successful!");
                con.Close();
            }
            else if (DoesSsnExistInCustomer(ssn))
            {
                string ConnectionString = "Data Source=DESKTOP-DFCJR49;Initial Catalog=FlightReservation;Integrated Security=True";
                SqlConnection con = new SqlConnection(ConnectionString);
                string query = "DELETE FROM CUSTOMER WHERE Ssn = @Ssn";
                con.Open();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@Ssn", ssn);

                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Delete successful!");
                con.Close();
            }
            else
            {
                MessageBox.Show("ID Does Not Exist");
            }
        }

        private bool DoesSsnExistInAdmin(int ssn)
        {
            string connectionString = "Data Source=DESKTOP-DFCJR49;Initial Catalog=FlightReservation;Integrated Security=True";
            string queryAdmin = "SELECT COUNT(Ssn) as Num FROM ADMIN WHERE Ssn = @Ssn";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                // Check in ADMIN table
                using (SqlCommand command = new SqlCommand(queryAdmin, con))
                {
                    command.Parameters.AddWithValue("@Ssn", ssn);
                    int countAdmin = (int)command.ExecuteScalar();

                    if (countAdmin > 0)
                    {
                        con.Close();
                        return true;
                    }
                }
                con.Close();
                return false;
            }
        }

        private bool DoesSsnExistInCustomer(int ssn)
        {
            string connectionString = "Data Source=DESKTOP-DFCJR49;Initial Catalog=FlightReservation;Integrated Security=True";
            string queryCustomer = "SELECT COUNT(Ssn) as Num FROM CUSTOMER WHERE Ssn = @Ssn";
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                // Check in CUSTOMER table
                using (SqlCommand command = new SqlCommand(queryCustomer, con))
                {
                    command.Parameters.AddWithValue("@Ssn", ssn);
                    int countCustomer = (int)command.ExecuteScalar();

                    if (countCustomer > 0)
                    {
                        con.Close();
                        return true;
                    }
                }
                con.Close();
                return false;
            }
        }
    }
}
