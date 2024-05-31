using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace FlightReservationGUI
{
    public partial class Customer_add_dependent : Form
    {
        public Customer_add_dependent()
        {
            InitializeComponent();
        }

        private void Customer_add_dependent_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            CustomerForm customer = new CustomerForm();
            customer.ShowDialog();
            this.Close();
        }

        private void SignUpButton_Click(object sender, EventArgs e)
        {
            int ssn;
            bool stringSsn = int.TryParse(Session.CurrentSession.Ssn, out ssn);
            string name = textBox1.Text;
            string relationship = textBox2.Text;
            DateTime bdate = dateTimePicker1.Value;
            bool gender = comboBox1.SelectedItem != null && comboBox1.SelectedItem.ToString() == "Male";
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Invalid Name");
                return;
            }
            else if (string.IsNullOrEmpty(relationship))
            {
                MessageBox.Show("Invalid relationship.");
                return;
            }
            else if (string.IsNullOrEmpty(comboBox1.SelectedItem?.ToString()))
            {
                MessageBox.Show("Please select a gender.");
                return;
            }
            else if (IsExist(ssn,name))
            {
                MessageBox.Show("Dependent already exists in the system.");
                return;
            }
            else
            {
                string query = "INSERT INTO DEPENDENT (Ssn, Name, Gender, Bdate, Relationship) VALUES (@Ssn, @Name, @Gender, @Bdate, @Relationship)";
                SqlConnection con = Session.CurrentSession.GetConnection();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@Ssn", ssn);
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@Gender", gender);
                    command.Parameters.AddWithValue("@Bdate", bdate);
                    command.Parameters.AddWithValue("@Relationship", relationship);

                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Adding successful!");
            }
        }
        private bool IsExist(int ssn, string name)
        {
            SqlConnection con = Session.CurrentSession.GetConnection();
            string query = "SELECT COUNT(*) FROM DEPENDENT WHERE Ssn = @Ssn AND Name = @Name";
            using (SqlCommand command = new SqlCommand(query, con))
            {
                command.Parameters.AddWithValue("@Ssn", ssn);
                command.Parameters.AddWithValue("@Name", name);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
    }
}
