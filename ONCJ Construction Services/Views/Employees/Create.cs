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
    public partial class Create : Form
    {
        public Create()
        {
            InitializeComponent();
        }

        private static Create form;
        private static bool result = false;

        public static bool ShowCustomDialog()
        {
            form = new Create();
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

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            Employee employee = new Employee();
            employee.first_name = TxtFirstName.Text;
            employee.last_name = TxtLastName.Text;
            employee.position = TxtPosition.Text;
            employee.picture_url = DateTime.Now.Ticks + Path.GetExtension(ProfilePicture.ImageLocation);

            if (employee.Create())
            {
                if (!Directory.Exists(Directory.GetCurrentDirectory() + "\\Uploads"))
                {
                    Directory.CreateDirectory(Directory.GetCurrentDirectory() + "\\Uploads");
                }

                File.Copy(ProfilePicture.ImageLocation, Directory.GetCurrentDirectory() + "\\Uploads\\" + employee.picture_url);

                AppController.ShowMessage("Successfully created an employee.", "Notification");
                result = true;
                this.Close();
            }
        }
    }
}
