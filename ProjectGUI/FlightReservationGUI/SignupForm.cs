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
using System.Collections;
using Microsoft.IdentityModel.Tokens;

namespace FlightReservationGUI
{
    public partial class SignupForm : Form
    {
        public SignupForm()
        {
            InitializeComponent();
            SalaryTextBox.Enabled = false;
            SalaryLabel.Enabled = false;
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            passwordTextBox.PasswordChar = '*';
        }
        private void SignUpButton_Click(object sender, EventArgs e)
        {
            try
            {
                string Fname = textBox1.Text;
                string Lname = textBox2.Text;
                string password = passwordTextBox.Text;
                int ssn;
                bool checkSsn = int.TryParse(textBox4.Text.Trim(), out ssn);
                string address = textBox5.Text;
                DateTime bdate = dateTimePicker1.Value;
                string nationality = textBox3.Text;
                bool gender = comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() == "Male";
                string userType = comboBox2.SelectedItem?.ToString();
                double salary = 0;
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
                else if (!checkSsn)
                {
                    MessageBox.Show("Invalid ID");
                    return;
                }
                else if(string.IsNullOrEmpty(address))
                {
                    MessageBox.Show("Invalid address.");
                    return;
                }
                else if (string.IsNullOrEmpty(nationality))
                {
                    MessageBox.Show("Invalid nationality.");
                    return;
                }
                else if (DoesSsnExist(ssn))
                {
                    MessageBox.Show("SSN already exists in the system.");
                    return;
                }
                
                else if (string.IsNullOrEmpty(comboBox1.SelectedItem?.ToString()))
                {
                    MessageBox.Show("Please select a gender.");
                    return;
                }
                else if (string.IsNullOrEmpty(userType))
                {
                    MessageBox.Show("Please select a type.");
                    return;
                }
                else if (comboBox2.SelectedItem.ToString() == "Admin")
                {
                    if (!double.TryParse(SalaryTextBox.Text.Trim(), out salary))
                    {
                        MessageBox.Show("Invalid salary.");
                        return;
                    }
                }
                
                string query;
                if(userType == "Admin")
                {
                    query = "INSERT INTO ADMIN (Ssn, Fname, Lname, Password, Bdate, Address, Gender, Nationality, Salary) VALUES (@Ssn, @Fname, @Lname, @Password, @Bdate, @Address, @Gender, @Nationality, @Salary)";
                }
                else
                {
                    query = "INSERT INTO CUSTOMER (Ssn, Fname, Lname, Password, Bdate, Address, Gender, Nationality) VALUES (@Ssn, @Fname, @Lname, @Password, @Bdate, @Address, @Gender, @Nationality)";
                }
                string ConnectionString = "Data Source=DESKTOP-DFCJR49;Initial Catalog=FlightReservation;Integrated Security=True";
                SqlConnection con = new SqlConnection(ConnectionString);
                con.Open();
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

                    if (userType == "Admin")
                    {
                        command.Parameters.AddWithValue("@Salary", salary);
                    }
                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Sign-up successful!");
                con.Close();
            }
            catch(Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }
        // function to check if the ssn is exist in customer or admin table
        private bool DoesSsnExist(int ssn)
        {
            string connectionString = "Data Source=DESKTOP-DFCJR49;Initial Catalog=FlightReservation;Integrated Security=True";
            string queryAdmin = "SELECT COUNT(Ssn) as Num FROM ADMIN WHERE Ssn = @Ssn";
            string queryCustomer = "SELECT COUNT(Ssn) as Num FROM CUSTOMER WHERE Ssn = @Ssn";
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

        private void User_Type_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedItem.ToString() == "Admin")
            {
                SalaryTextBox.Enabled = true;
                SalaryLabel.Enabled = true;
            }
            else
            {
                SalaryTextBox.Enabled = false;
                SalaryLabel.Enabled = false;
            }
        }

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 reg = new Form1();
            reg.ShowDialog();
            this.Close();
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = "";
                textBox2.Text = "";
                passwordTextBox.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox3.Text = "";
                //comboBox1.SelectedItem = "";
                //comboBox2.SelectedItem = "";
                SalaryTextBox.Text = "";
            }
            catch(Exception exception)
            {
                MessageBox.Show(exception.ToString());
            }
        }

        private void SignupForm_Load(object sender, EventArgs e)
        {

        }
    }
}
