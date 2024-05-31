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
    public partial class Admin_update_airplane : Form
    {
        public Admin_update_airplane()
        {
            InitializeComponent();
            try
            {
                string query = "SELECT * FROM AIRPLANE";
                SqlConnection con = Session.CurrentSession.GetConnection();
                using (SqlDataAdapter adapter = new SqlDataAdapter(query, con))
                {
                    DataTable airplaneTable = new DataTable();
                    adapter.Fill(airplaneTable);
                    dataGridView1.DataSource = airplaneTable;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
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
            string model = textBox2.Text;
            int capacity;
            bool checkCapacity = int.TryParse(textBox3.Text, out capacity);

            if (!checkSerial)
            {
                MessageBox.Show("Invalid Serial");
                return;
            }
            else if (string.IsNullOrEmpty(model))
            {
                MessageBox.Show("Invalid Model");
                return;
            }
            else if (!checkCapacity)
            {
                MessageBox.Show("Invalid Capacity");
                return;
            }
            if (DoesSerialExist(serial))
            {
                string query = "UPDATE AIRPLANE SET Capacity = @Capacity, Model = @Model WHERE Serial = @Serial;";
                SqlConnection con = Session.CurrentSession.GetConnection();
                using (SqlCommand command = new SqlCommand(query, con))
                {
                    command.Parameters.AddWithValue("@Serial", serial);
                    command.Parameters.AddWithValue("@Model", model);
                    command.Parameters.AddWithValue("@Capacity", capacity);

                    command.ExecuteNonQuery();
                }
                MessageBox.Show("Updating successful!");
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

        private void Admin_update_airplane_Load(object sender, EventArgs e)
        {

        }
    }
}
