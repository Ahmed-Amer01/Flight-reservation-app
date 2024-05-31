using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Reflection;

namespace FlightReservationGUI
{
    public partial class Admin_delete_airplane : Form
    {
        public Admin_delete_airplane()
        {
            InitializeComponent();
        }

        private void Admin_delete_airplane_Load(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            AdminForm admin = new AdminForm();
            admin.ShowDialog();
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int serial;
            bool checkSerial = int.TryParse(textBox1.Text, out serial);
            if (!checkSerial)
            {
                MessageBox.Show("Invalid Serial");
                return;
            }
            if (DoesSerialExist(serial))
            {

                string query = "DELETE FROM AIRPLANE WHERE Serial = @Serial";
                SqlConnection con = Session.CurrentSession.GetConnection();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@Serial", serial);

                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Delete successful!");
            }
            else
            {
                MessageBox.Show("Airplane Does Not Exist");
            }
        }

        private bool DoesSerialExist(int serial)
        {
            SqlConnection con = Session.CurrentSession.GetConnection();
            string query = "SELECT COUNT(*) FROM AIRPLANE WHERE Serial = @Serial";
            using (SqlCommand command = new SqlCommand(query, con))
            {
                command.Parameters.AddWithValue("@Serial", serial);
                int count = (int)command.ExecuteScalar();
                return count > 0;
            }
        }
    }
}
