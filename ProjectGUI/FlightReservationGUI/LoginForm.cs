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
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            passwordTextBox.PasswordChar = '*';
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 reg = new Form1();
            reg.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int idNum;
            bool checkID = int.TryParse(idTextBox.Text.Trim(), out idNum);
            string password = passwordTextBox.Text;
            string id = idNum.ToString();
            if (!checkID)
            {
                MessageBox.Show("Invalid ID");
                return;
            }
            else if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Invalid Password");
                return;
            }
            
            if (DataExist(id, password, out string userType))
            {
                Session.CurrentSession.SetUserSession(id, userType);
                this.Hide();
                if (userType == "Admin")
                {
                    // Redirect to Admin Form
                    AdminForm adminForm = new AdminForm();
                    adminForm.ShowDialog();
                }
                else if (userType == "Customer")
                {
                    // Redirect to Customer Form
                    CustomerForm customerForm = new CustomerForm();
                    customerForm.ShowDialog();
                }
            }
            else
            {
                MessageBox.Show("Invalid SSN or password.");
            }
        }

        // check if id and password match in database
        private bool DataExist(string ssn, string password, out string userType)
        {
            string connectionString = "Data Source=DESKTOP-DFCJR49;Initial Catalog=FlightReservation;Integrated Security=True";
            Session.CurrentSession.CreateConnection(connectionString);
            SqlConnection con = Session.CurrentSession.GetConnection();
            // Check in ADMIN table
            using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM ADMIN WHERE Ssn = @Ssn AND Password = @Password", con))
            {
                command.Parameters.AddWithValue("@Ssn", ssn);
                command.Parameters.AddWithValue("@Password", password);
                int count = (int)command.ExecuteScalar();
                if (count > 0)
                {
                    userType = "Admin";
                    return true;
                }
            }

            // Check in CUSTOMER table
            using (SqlCommand command = new SqlCommand("SELECT COUNT(*) FROM CUSTOMER WHERE Ssn = @Ssn AND Password = @Password", con))
            {
                command.Parameters.AddWithValue("@Ssn", ssn);
                command.Parameters.AddWithValue("@Password", password);
                int count = (int)command.ExecuteScalar();
                if (count > 0)
                {
                    userType = "Customer";
                    return true;
                }
            }

            Session.CurrentSession.Logout();

            userType = string.Empty;
            return false;
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

        }
    }
}
