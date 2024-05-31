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
    public partial class Update_user : Form
    {
        public Update_user()
        {
            InitializeComponent();
            SalaryTextBox.Enabled = false;
            SalaryLabel.Enabled = false;
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            passwordTextBox.PasswordChar = '*';
            try
            {
                string connectionString = "Data Source=DESKTOP-DFCJR49;Initial Catalog=FlightReservation;Integrated Security=True";
                string queryAdmin = "SELECT * FROM ADMIN";
                string queryCustomer = "SELECT * FROM CUSTOMER";
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(queryAdmin, con))
                    {
                        DataTable adminTable = new DataTable();
                        adapter.Fill(adminTable);
                        dataGridView1.DataSource = adminTable;
                    }
                    con.Close();
                    con.Open();
                    using (SqlDataAdapter adapter = new SqlDataAdapter(queryCustomer, con))
                    {
                        DataTable customerTable = new DataTable();
                        adapter.Fill(customerTable);
                        dataGridView2.DataSource = customerTable;
                    }
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void Update_user_Load(object sender, EventArgs e)
        {

        }

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 reg = new Form1();
            reg.ShowDialog();
            this.Close();
        }

        private void UpdateButton_Click(object sender, EventArgs e)
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
            if (DoesSsnExistInAdmin(ssn, userType))
            {
               string query = "UPDATE ADMIN SET Fname = @Fname, Lname = @Lname, Password = @Password, Bdate = @Bdate," +
                              "Address = @Address, Gender = @Gender, Nationality = @Nationality, Salary = @Salary WHERE Ssn = @Ssn;";
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
                    command.Parameters.AddWithValue("@Salary", salary);

                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Update successful!");
                con.Close();
            }
            else if(DoesSsnExistInCustomer(ssn, userType))
            {
                string query = "UPDATE CUSTOMER SET Fname = @Fname, Lname = @Lname, Password = @Password, Bdate = @Bdate," +
                               "Address = @Address, Gender = @Gender, Nationality = @Nationality WHERE Ssn = @Ssn;";
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

                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Update successful!");
                con.Close();
            }
            else
            {
                MessageBox.Show("ID Does Not Exist");
            }
        }

        private bool DoesSsnExistInAdmin(int ssn, string userType)
        {
            if(userType == "Admin")
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
            else
            {
                return false;
            }
        }

        private bool DoesSsnExistInCustomer(int ssn, string userType)
        {
            if(userType == "Customer")
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
            else
            {
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
    }
}
