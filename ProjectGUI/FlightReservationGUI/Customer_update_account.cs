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
    public partial class Customer_update_account : Form
    {
        public Customer_update_account()
        {
            InitializeComponent();
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            passwordTextBox.PasswordChar = '*';
            try
            {
                int ssn;
                bool stringSsn = int.TryParse(Session.CurrentSession.Ssn, out ssn);
                SqlConnection con = Session.CurrentSession.GetConnection();
                string queryCustomer = "SELECT * FROM CUSTOMER WHERE Ssn = @Ssn";
                using (SqlCommand command = new SqlCommand(queryCustomer, con))
                {
                    command.Parameters.AddWithValue("@Ssn", ssn);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable customerTable = new DataTable();
                        adapter.Fill(customerTable);
                        dataGridView1.DataSource = customerTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void Customer_update_account_Load(object sender, EventArgs e)
        {

        }

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            CustomerForm customerForm = new CustomerForm();
            customerForm.ShowDialog();
            this.Close();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
        {
            string Fname = textBox1.Text;
            string Lname = textBox2.Text;
            string password = passwordTextBox.Text;
            string address = textBox5.Text;
            DateTime bdate = dateTimePicker1.Value;
            string nationality = textBox3.Text;
            bool gender = comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() == "Male";
            string userType = "Customer";
            if (string.IsNullOrEmpty(Fname) || string.IsNullOrEmpty(Lname))
            {
                MessageBox.Show("Invalid Username");
                return;
            }
            else if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Invalid Password");
                return;
            }
            else if (string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Invalid address.");
                return;
            }
            else if (string.IsNullOrEmpty(nationality))
            {
                MessageBox.Show("Invalid nationality.");
                return;
            }
            else if (string.IsNullOrEmpty(comboBox1.SelectedItem?.ToString()))
            {
                MessageBox.Show("Please select a gender.");
                return;
            }
            int ssn;
            bool stringSsn = int.TryParse(Session.CurrentSession.Ssn, out ssn);
            if (DoesSsnExistInCustomer(ssn))
            {
                string query = "UPDATE CUSTOMER SET Fname = @Fname, Lname = @Lname, Password = @Password, Bdate = @Bdate," +
                               "Address = @Address, Gender = @Gender, Nationality = @Nationality WHERE Ssn = @Ssn;";
                SqlConnection con = Session.CurrentSession.GetConnection();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@Ssn", ssn);
                    command.Parameters.AddWithValue("@Fname", Fname);
                    command.Parameters.AddWithValue("@Lname", Lname);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@Bdate", bdate);
                    command.Parameters.AddWithValue("@Address", address);
                    command.Parameters.AddWithValue("@Gender", gender ? 1 : 0);
                    command.Parameters.AddWithValue("@Nationality", nationality);

                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Update successful!");
            }
            else
            {
                MessageBox.Show("ID Does Not Exist");
            }
        }

        private bool DoesSsnExistInCustomer(int ssn)
        {
            SqlConnection con = Session.CurrentSession.GetConnection();
            string queryCustomer = "SELECT COUNT(Ssn) as Num FROM CUSTOMER WHERE Ssn = @Ssn";
            using (SqlCommand command = new SqlCommand(queryCustomer, con))
            {
                command.Parameters.AddWithValue("@Ssn", ssn);
                int countCustomer = (int)command.ExecuteScalar();

                if (countCustomer > 0)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
