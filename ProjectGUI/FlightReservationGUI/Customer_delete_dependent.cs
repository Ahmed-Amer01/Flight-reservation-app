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
    public partial class Customer_delete_dependent : Form
    {
        public Customer_delete_dependent()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            CustomerForm customer = new CustomerForm();
            customer.ShowDialog();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int ssn;
            bool stringSsn = int.TryParse(Session.CurrentSession.Ssn, out ssn);
            string name = textBox1.Text;
            if (string.IsNullOrEmpty(name))
            {
                MessageBox.Show("Invalid Name");
                return;
            }
            if (IsExist(ssn, name))
            {
                string query = "DELETE FROM DEPENDENT WHERE Ssn = @Ssn AND Name = @Name";
                SqlConnection con = Session.CurrentSession.GetConnection();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@Ssn", ssn);
                    command.Parameters.AddWithValue("@Name", name);

                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Delete successful!");
            }
            else
            {
                MessageBox.Show("Dependent Does Not Exist");
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
