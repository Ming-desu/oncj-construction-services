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
    public partial class AddMember : Form
    {
        public AddMember()
        {
            InitializeComponent();
        }

        public static double rate = 0;
        private static Employee employee;
        private static AddMember form;
        private static bool result = false;

        public static bool ShowCustomDialog(Employee employee)
        {
            AddMember.employee = employee;
            form = new AddMember();
            form.TxtName.Text = employee.first_name + " " + employee.last_name + " (" + employee.position + ")";
            form.ShowDialog();

            return result;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            double rate;
            double.TryParse(TxtRate.Text.Trim('.'), out rate);

            AddMember.rate = rate;
            AddMember.result = true;
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
    }
}
