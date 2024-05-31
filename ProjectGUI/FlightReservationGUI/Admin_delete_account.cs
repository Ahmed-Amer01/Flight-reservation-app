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
    public partial class Admin_delete_account : Form
    {
        public Admin_delete_account()
        {
            InitializeComponent();
        }

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminForm adminForm = new AdminForm();
            adminForm.ShowDialog();
            this.Close();
        }

        private void DeleteButton_Click(object sender, EventArgs e)
        {
            int ssn;
            bool checkSsn = int.TryParse(textBox4.Text.Trim(), out ssn);
            string password = textBox1.Text;
            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Invalid Password");
                return;
            }
            else if (!checkSsn)
            {
                MessageBox.Show("Invalid ID");
                return;
            }
            if (DataIsTrue(ssn, password))
            {
                SqlConnection con = Session.CurrentSession.GetConnection();
                string query = "DELETE FROM ADMIN WHERE Ssn = @Ssn";
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@Ssn", ssn);
                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Delete successful!");
                Session.CurrentSession.Logout();
                this.Hide();
                Form1 form1 = new Form1();
                form1.ShowDialog();
                this.Close();
            }
            else
            {
                MessageBox.Show("Wrong Data");
            }
        }

        private void Admin_delete_account_Load(object sender, EventArgs e)
        {

        }

        private bool DataIsTrue(int ssn, string password)
        {
            int recordedSsn;
            bool stringSsn = int.TryParse(Session.CurrentSession.Ssn, out recordedSsn);
            if(ssn == recordedSsn)
            {
                SqlConnection con = Session.CurrentSession.GetConnection();
                string queryAdmin = "SELECT COUNT(*) FROM ADMIN WHERE Ssn = @Ssn AND Password = @Password";
                using (SqlCommand command = new SqlCommand(queryAdmin, con))
                {
                    command.Parameters.AddWithValue("@Ssn", ssn);
                    command.Parameters.AddWithValue("@Password", password);
                    int countAdmin = (int)command.ExecuteScalar();
                    if (countAdmin > 0)
                    {
                        return true;
                    }
                }
                return false;
            }
            else
            {
                return false;
            }  
        }

    }
}
