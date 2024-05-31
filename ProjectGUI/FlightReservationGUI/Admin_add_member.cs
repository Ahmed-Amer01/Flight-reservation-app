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
using System.Reflection;

namespace FlightReservationGUI
{
    public partial class Admin_add_member : Form
    {
        public Admin_add_member()
        {
            InitializeComponent();
        }

        private void Admin_add_member_Load(object sender, EventArgs e)
        {

        }
        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminForm admin = new AdminForm();
            admin.ShowDialog();
            this.Close();
        }

        private void SignUpButton_Click(object sender, EventArgs e)
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
            string role = textBox6.Text;
            double salary;
            bool checkSalary = double.TryParse(SalaryTextBox.Text.Trim(), out salary);

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
            else if (string.IsNullOrEmpty(role))
            {
                MessageBox.Show("Invalid role.");
                return;
            }
            else if (!checkSalary)
            {
                MessageBox.Show("Invalid Salary");
                return;
            }
            else if (DoesSsnExist1(ssn))
            {
                MessageBox.Show("SSN already exists in the system.");
                return;
            }
            else if (DoesSsnExist2(ssn))
            {
                MessageBox.Show("SSN already exists in the system.");
                return;
            }
            else if (DoesSsnExist3(ssn))
            {
                MessageBox.Show("SSN already exists in the system.");
                return;
            }
            string query = "INSERT INTO CREW_MEMBER (Ssn, Fname, Lname, Password, Bdate, Address, Gender, Nationality, Salary, Role) " +
                           "VALUES (@Ssn, @Fname, @Lname, @Password, @Bdate, @Address, @Gender, @Nationality, @Salary, @Role)";
            SqlConnection con = Session.CurrentSession.GetConnection();
            using (SqlCommand command = new SqlCommand(query, con))
            {
                command.Parameters.AddWithValue("@Ssn", ssn);
                command.Parameters.AddWithValue("@Fname", Fname);
                command.Parameters.AddWithValue("@Lname", Lname);
                command.Parameters.AddWithValue("@Password", password);
                command.Parameters.AddWithValue("@Bdate", bdate);
                command.Parameters.AddWithValue("@Address", address);
                command.Parameters.AddWithValue("@Gender", gender);
                command.Parameters.AddWithValue("@Nationality", nationality);
                command.Parameters.AddWithValue("@Salary", salary);
                command.Parameters.AddWithValue("@Role", role);

                command.ExecuteNonQuery();
            }
            MessageBox.Show("Adding successful!");
        }

        private bool DoesSsnExist1(int ssn)
        {
            SqlConnection con = Session.CurrentSession.GetConnection();
            string query = "SELECT COUNT(*) FROM ADMIN WHERE Ssn = @Ssn";
            using (SqlCommand command = new SqlCommand(query, con))
            {
                command.Parameters.AddWithValue("@Ssn", ssn);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        private bool DoesSsnExist2(int ssn)
        {
            SqlConnection con = Session.CurrentSession.GetConnection();
            string query = "SELECT COUNT(*) FROM CUSTOMER WHERE Ssn = @Ssn";
            using (SqlCommand command = new SqlCommand(query, con))
            {
                command.Parameters.AddWithValue("@Ssn", ssn);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }

        private bool DoesSsnExist3(int ssn)
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
