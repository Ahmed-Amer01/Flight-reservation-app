using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlightReservationGUI
{
    public partial class CustomerForm : Form
    {
        public CustomerForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Session.CurrentSession.Logout();
            this.Hide();
            Form1 form1 = new Form1();
            form1.ShowDialog();
            this.Close();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            this.Hide();
            Customer_update_account update_account = new Customer_update_account();
            update_account.ShowDialog();
            this.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.Hide();
            Customer_delete_account delete_account = new Customer_delete_account();
            delete_account.ShowDialog();
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Customer_add_dependent add_dependent = new Customer_add_dependent();
            add_dependent.ShowDialog();
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Customer_delete_dependent delete_dependent = new Customer_delete_dependent();
            delete_dependent.ShowDialog();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Customer_update_dependent update_dependent = new Customer_update_dependent();
            update_dependent.ShowDialog();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Customer_reserve_flight reserve_flight = new Customer_reserve_flight();
            reserve_flight.ShowDialog();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            Customer_update_reservation update_reservation = new Customer_update_reservation();
            update_reservation.ShowDialog();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            Customer_cancel_reservation cancel_reservation = new Customer_cancel_reservation();
            cancel_reservation.ShowDialog();
            this.Close();
        }
    }
}
