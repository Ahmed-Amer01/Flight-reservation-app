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
    public partial class AdminForm : Form
    {
        public AdminForm()
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

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin_add_airplane add_plane = new Admin_add_airplane();
            add_plane.ShowDialog();
            this.Close();
        }

        private void AdminForm_Load(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin_delete_airplane delete_plane = new Admin_delete_airplane();
            delete_plane.ShowDialog();
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin_update_airplane update_plane = new Admin_update_airplane();
            update_plane.ShowDialog();
            this.Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin_add_flight add_flight = new Admin_add_flight();
            add_flight.ShowDialog();
            this.Close();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin_delete_flight delete_flight = new Admin_delete_flight();
            delete_flight.ShowDialog();
            this.Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin_update_flight update_flight = new Admin_update_flight();
            update_flight.ShowDialog();
            this.Close();
        }

        private void button10_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin_add_member add_member = new Admin_add_member();
            add_member.ShowDialog();
            this.Close();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin_delete_member delete_member = new Admin_delete_member();
            delete_member.ShowDialog();
            this.Close();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin_update_member update_member = new Admin_update_member();
            update_member.ShowDialog();
            this.Close();
        }

        private void button12_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin_update_account update_account = new Admin_update_account();
            update_account.ShowDialog();
            this.Close();
        }

        private void button11_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin_delete_account delete_account = new Admin_delete_account();
            delete_account.ShowDialog();
            this.Close();
        }

        private void button15_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin_assign_member_to_flight assign_member_to_flight = new Admin_assign_member_to_flight();
            assign_member_to_flight.ShowDialog();
            this.Close();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin_remove_member_from_flight remove_member_from_flight = new Admin_remove_member_from_flight();
            remove_member_from_flight.ShowDialog();
            this.Close();
        }

        private void button13_Click(object sender, EventArgs e)
        {
            this.Hide();
            Admin_display_flight_info display_flight_info = new Admin_display_flight_info();
            display_flight_info.ShowDialog();
            this.Close();
        }

        private void button16_Click(object sender, EventArgs e)
        {
            this.Hide();
            Meaningful_report report = new Meaningful_report();
            report.ShowDialog();
            this.Close();
        }
    }
}
