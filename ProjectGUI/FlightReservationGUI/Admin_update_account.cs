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

namespace FlightReservationGUI
{
    public partial class Admin_update_account : Form
    {
        public Admin_update_account()
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
                string queryAdmin = "SELECT * FROM ADMIN WHERE Ssn = @Ssn";
                using (SqlCommand command = new SqlCommand(queryAdmin, con))
                {
                    command.Parameters.AddWithValue("@Ssn", ssn);
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        DataTable adminTable = new DataTable();
                        adapter.Fill(adminTable);
                        dataGridView1.DataSource = adminTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void ReturnButton_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminForm adminForm = new AdminForm();
            adminForm.ShowDialog();
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
            string userType = "Admin";
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
            else if (!double.TryParse(SalaryTextBox.Text.Trim(), out salary))
            {
                MessageBox.Show("Invalid salary.");
                return;
            }
            int ssn;
            bool stringSsn = int.TryParse(Session.CurrentSession.Ssn, out ssn);
            if (DoesSsnExistInAdmin(ssn))
            {
                string query = "UPDATE ADMIN SET Fname = @Fname, Lname = @Lname, Password = @Password, Bdate = @Bdate," +
                               "Address = @Address, Gender = @Gender, Nationality = @Nationality, Salary = @Salary WHERE Ssn = @Ssn;";
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
                    command.Parameters.AddWithValue("@Salary", salary);

                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Update successful!");
            }
            else
            {
                MessageBox.Show("ID Does Not Exist");
            }
        }

        private bool DoesSsnExistInAdmin(int ssn)
        {
            SqlConnection con = Session.CurrentSession.GetConnection();
            string queryAdmin = "SELECT COUNT(Ssn) as Num FROM ADMIN WHERE Ssn = @Ssn";
            using (SqlCommand command = new SqlCommand(queryAdmin, con))
            {
                command.Parameters.AddWithValue("@Ssn", ssn);
                int countAdmin = (int)command.ExecuteScalar();

                if (countAdmin > 0)
                {
                    return true;
                }
            }
            return false;
        }

        private void Admin_update_account_Load(object sender, EventArgs e)
        {

        }
    }
}
