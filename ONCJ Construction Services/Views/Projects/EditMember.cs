using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ONCJ_Construction_Services.Models;
using ONCJ_Construction_Services.Common;
using System.IO;

namespace ONCJ_Construction_Services.Views.Projects
{
    public partial class EditMember : Form
    {
        public EditMember()
        {
            InitializeComponent();
        }

        public static bool toDelete = false;
        public static double rate = 0;
        private static Employee employee;
        private static EditMember form;
        private static bool result = false;

        public static bool ShowCustomDialog(Employee employee, double rate)
        {
            EditMember.toDelete = false;
            EditMember.employee = employee;
            EditMember.rate = rate;
            form = new EditMember();
            form.TxtName.Text = employee.first_name + " " + employee.last_name + " (" + employee.position + ")";
            form.TxtRate.Text = rate.ToString();
            form.ShowDialog();

            return result;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            double rate;
            double.TryParse(TxtRate.Text.Trim('.'), out rate);

            EditMember.rate = rate;
            EditMember.result = true;
            this.Close();
        }

        private void TxtRate_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.' && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this member from this project?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }

            EditMember.toDelete = true;
            EditMember.result = true;
            this.Close();
        }
    }
}
