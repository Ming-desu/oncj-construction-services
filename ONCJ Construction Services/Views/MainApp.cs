using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ONCJ_Construction_Services.Common;

namespace ONCJ_Construction_Services.Views
{
    public partial class MainApp : Form
    {
        public MainApp()
        {
            InitializeComponent();

            ShowForm(new Employees.Index());
        }

        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppController.ShowForm(new Login());
        }

        private void exitApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void ShowForm(Form form)
        {
            foreach (var c in MainContainer.Controls)
            {
                if (c is Form)
                {
                    (c as Form).Close();
                }
            }

            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            form.Parent = MainContainer;
            form.Show();
        }

        private void newEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Employees.Create.ShowCustomDialog())
            {
                LoadTable();
            }
        }

        private void showAllEmployeesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm(new Employees.Index());
        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (Projects.Create.ShowCustomDialog())
            {
                LoadTable();
            }
        }

        private void showAllProjectsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowForm(new Projects.Index());
        }

        private void LoadTable()
        {
            foreach (var c in MainContainer.Controls)
            {
                if (c is Projects.Index)
                {
                    (c as Projects.Index).LoadTable();
                }
                else if (c is Employees.Index)
                {
                    (c as Employees.Index).LoadTable();
                }
            }
        }
    }
}
