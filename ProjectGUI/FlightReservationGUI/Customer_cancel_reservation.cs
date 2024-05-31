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
    public partial class Customer_cancel_reservation : Form
    {
        public Customer_cancel_reservation()
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

        private void button3_Click(object sender, EventArgs e)
        {
            int ssn;
            bool stringSsn = int.TryParse(Session.CurrentSession.Ssn, out ssn);
            int flightNum;
            if (!int.TryParse(textBox3.Text, out flightNum))
            {
                MessageBox.Show("Invalid Flight Number");
                return;
            }
            SqlConnection con = Session.CurrentSession.GetConnection();
            try
            {
                string queryCheckReservation = @"
                       SELECT Num_of_tickets 
                       FROM RESERVE 
                       WHERE Ssn = @Ssn AND Flight_number = @FlightNumber";

                SqlCommand commandCheckReservation = new SqlCommand(queryCheckReservation, con);
                commandCheckReservation.Parameters.AddWithValue("@Ssn", ssn);
                commandCheckReservation.Parameters.AddWithValue("@FlightNumber", flightNum);

                int numTickets = 0;
                using (SqlDataReader reader = commandCheckReservation.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        numTickets = (int)reader["Num_of_tickets"];
                    }
                }

                if (numTickets == 0)
                {
                    MessageBox.Show("No reservation found for the that flight.");
                    return;
                }

                string queryGetClass = @"
                       SELECT Class 
                       FROM RESERVE 
                       WHERE Ssn = @Ssn AND Flight_number = @FlightNumber";

                SqlCommand commandGetClass = new SqlCommand(queryGetClass, con);
                commandGetClass.Parameters.AddWithValue("@Ssn", ssn);
                commandGetClass.Parameters.AddWithValue("@FlightNumber", flightNum);

                string classValue = commandGetClass.ExecuteScalar()?.ToString();
                if (string.IsNullOrEmpty(classValue))
                {
                    MessageBox.Show("No reservation found for the that flight.");
                    return;
                }

                string cancelFlightReservation = "DELETE FROM RESERVE WHERE Ssn = @Ssn AND Flight_number = @Flight_number;";
                SqlCommand commandUpdateReservation = new SqlCommand(cancelFlightReservation, con);
                commandUpdateReservation.Parameters.AddWithValue("@Ssn", ssn);
                commandUpdateReservation.Parameters.AddWithValue("@Flight_number", flightNum);

                commandUpdateReservation.ExecuteNonQuery();

                string Class = classValue == "A" ? "Class_a_num" : "Class_b_num";
                string queryUpdateOldClassCount = $@"
                       UPDATE FLIGHT 
                       SET {Class} = {Class} - @NumTickets
                       WHERE Flight_number = @Flight_number";

                SqlCommand commandUpdateOldClassCount = new SqlCommand(queryUpdateOldClassCount, con);
                commandUpdateOldClassCount.Parameters.AddWithValue("@NumTickets", numTickets);
                commandUpdateOldClassCount.Parameters.AddWithValue("@Flight_number", flightNum);

                commandUpdateOldClassCount.ExecuteNonQuery();

                MessageBox.Show("Reservation cancelled successfully.");
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message);
            }
        }

        private void Customer_cancel_reservation_Load(object sender, EventArgs e)
        {

        }
    }
}
