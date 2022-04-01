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

namespace ONCJ_Construction_Services.Views.Employees
{
    public partial class Edit : Form
    {
        public Edit()
        {
            InitializeComponent();
        }

        private static Edit form;
        private static bool result = false;
        private static Employee employee;

        public static bool ShowCustomDialog(Employee employee)
        {
            Edit.employee = employee;

            form = new Edit();
            form.TxtFirstName.Text = employee.first_name;
            form.TxtLastName.Text = employee.last_name;
            form.TxtPosition.Text = employee.position;
            form.ProfilePicture.ImageLocation = Directory.GetCurrentDirectory() + "\\Uploads\\" + employee.picture_url;
            form.ShowDialog();

            return result;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnBrowse_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Select picture for the employee";
            openFileDialog.Filter = "All Files|*.png; *.jpg; *.jpeg";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                ProfilePicture.ImageLocation = openFileDialog.FileName;
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            Employee employee = new Employee();
            employee.ID = Edit.employee.ID;
            employee.first_name = TxtFirstName.Text;
            employee.last_name = TxtLastName.Text;
            employee.position = TxtPosition.Text;
            employee.picture_url = DateTime.Now.Ticks + Path.GetExtension(ProfilePicture.ImageLocation);

            if (employee.Update())
            {
                if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\Uploads"))
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Uploads");
                }

                File.Copy(ProfilePicture.ImageLocation, Directory.GetCurrentDirectory() + "\\Uploads\\" + employee.picture_url);

                AppController.ShowMessage("Successfully updated an employee.", "Notification");
                result = true;
                this.Close();
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this record?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }

            if (Edit.employee.Delete())
            {
                AppController.ShowMessage("Successfully deleted an employee.", "Notification");
                result = true;
                this.Close();
            }
        }
    }
}
