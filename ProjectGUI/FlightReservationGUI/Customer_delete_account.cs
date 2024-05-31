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
    public partial class Customer_delete_account : Form
    {
        public Customer_delete_account()
        {
            InitializeComponent();
        }

        private void Customer_delete_account_Load(object sender, EventArgs e)
        {

        }

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            CustomerForm customerForm = new CustomerForm();
            customerForm.ShowDialog();
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
                string query = "DELETE FROM CUSTOMER WHERE Ssn = @Ssn";
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

        private bool DataIsTrue(int ssn, string password)
        {
            int recordedSsn;
            bool stringSsn = int.TryParse(Session.CurrentSession.Ssn, out recordedSsn);
            if (ssn == recordedSsn)
            {
                SqlConnection con = Session.CurrentSession.GetConnection();
                string queryCustomer = "SELECT COUNT(*) FROM CUSTOMER WHERE Ssn = @Ssn AND Password = @Password";
                using (SqlCommand command = new SqlCommand(queryCustomer, con))
                {
                    command.Parameters.AddWithValue("@Ssn", ssn);
                    command.Parameters.AddWithValue("@Password", password);
                    int countCustomer = (int)command.ExecuteScalar();
                    if (countCustomer > 0)
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
