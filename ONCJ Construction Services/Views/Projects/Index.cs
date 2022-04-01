using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using ONCJ_Construction_Services.Common;
using ONCJ_Construction_Services.Models;

namespace ONCJ_Construction_Services.Views.Projects
{
    public partial class Index : Form
    {
        public Index()
        {
            InitializeComponent();

            LoadTable();
        }

        public void LoadTable(string query = "")
        {
            Project project = new Project();
            List<Project> projects = project.Read(query);

            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Subject");
            dt.Columns.Add("Date Covered");

            dataGridView1.DataSource = dt;
            dataGridView1.Columns["ID"].Width = 30;

            if (projects == null)
            {
                return;
            }

            foreach (Project proj in projects)
            {
                dt.Rows.Add(new object[]
                {
                    proj.ID,
                    proj.subject,
                    proj.date_start.ToString("MMM dd") + " - " + proj.date_end.ToString("MMM dd, yyyy")
                });
            }
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
        {
            LoadTable();
        }

        private void BtnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TxtSearch.Text))
            {
                LoadTable(TxtSearch.Text);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
                int id = Convert.ToInt32(row.Cells["ID"].Value);

                Project project = new Project();
                project.ID = id;
                project = project.Find();

                if (project == null)
                {
                    AppController.ShowMessage("Project does not exists.", "Notification");
                    return;
                }

                ProjectMember member = new ProjectMember();
                member.project_id = project.ID;

                project.members = member.Read();

                if (Projects.Edit.ShowCustomDialog(project))
                {
                    LoadTable();
                }
            }
        }
    }
}
