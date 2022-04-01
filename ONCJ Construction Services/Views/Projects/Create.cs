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
    public partial class Create : Form
    {
        private DataTable dtMembers;

        public Create()
        {
            InitializeComponent();

            LoadEmployeesTable();
            InitMembersTable();
        }

        private static Create form;
        private static bool result = false;

        public static bool ShowCustomDialog()
        {
            form = new Create();
            form.ShowDialog();

            return result;
        }

        private void LoadEmployeesTable(string query = "")
        {
            Employee employee = new Employee();
            List<Employee> employees = employee.Read(query);

            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("First name");
            dt.Columns.Add("Last name");
            dt.Columns.Add("Position");

            dataGridView1.DataSource = dt;

            if (employees == null)
            {
                return;
            }

            foreach (Employee emp in employees)
            {
                dt.Rows.Add(new object[]
                {
                    emp.ID,
                    emp.first_name,
                    emp.last_name,
                    emp.position
                });
            }
        }

        private void InitMembersTable()
        {
            dtMembers = new DataTable();
            dtMembers.Columns.Add("ID");
            dtMembers.Columns.Add("Name");
            dtMembers.Columns.Add("First name");
            dtMembers.Columns.Add("Last name");
            dtMembers.Columns.Add("Position");
            dtMembers.Columns.Add("Daily Rate");

            dataGridView2.DataSource = dtMembers;
            dataGridView2.Columns["ID"].Width = 30;
            dataGridView2.Columns["First name"].Visible = false;
            dataGridView2.Columns["Last name"].Visible = false;
            dataGridView2.Columns["Position"].Visible = false;
            dataGridView2.Columns["Daily Rate"].Width = 100;
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void BtnCreate_Click(object sender, EventArgs e)
        {
            Project project = new Project();
            project.subject = TxtSubject.Text;
            project.date_start = DateStart.Value;
            project.date_end = DateEnd.Value;

            //AppController.ShowMessage(DateStart.Value.ToString("yyyy-MM-dd HH:mm:ss") + "\n" + DateEnd.Value.ToString("yyyy-MM-dd HH:mm:ss"), "");
            //return;

            if (dataGridView2.Rows.Count < 0)
            {
                return;
            }

            int id = project.Create();

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                ProjectMember member = new ProjectMember();
                member.project_id = id;
                member.employee_id = Convert.ToInt32(row.Cells["ID"].Value);
                member.rate = Convert.ToDouble(row.Cells["Daily Rate"].Value);
                member.Create();

                project.members.Add(member);    
            }

            if (id > 0)
            {
                AppController.ShowMessage("Successfully created a project.", "Notification");
                result = true;
                this.Close();
            }
        }

        private void TxtSearch_KeyUp(object sender, KeyEventArgs e)
        {
            LoadEmployeesTable(TxtSearch.Text);
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewRow row = dataGridView1.Rows[e.RowIndex];
            Employee employee = new Employee();
            employee.ID = Convert.ToInt32(row.Cells["ID"].Value);
            employee.first_name = row.Cells["First name"].Value.ToString();
            employee.last_name = row.Cells["Last name"].Value.ToString();
            employee.position = row.Cells["Position"].Value.ToString();

            if (Projects.AddMember.ShowCustomDialog(employee))
            {
                dtMembers.Rows.Add(new object[]
                {
                    employee.ID,
                    employee.last_name + ", " + employee.first_name,
                    employee.first_name,
                    employee.last_name,
                    employee.position,
                    AddMember.rate
                });
            }
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            DataGridViewRow row = dataGridView2.Rows[e.RowIndex];
            Employee employee = new Employee();
            employee.ID = Convert.ToInt32(row.Cells["ID"].Value);
            employee.first_name = row.Cells["First name"].Value.ToString();
            employee.last_name = row.Cells["Last name"].Value.ToString();
            employee.position = row.Cells["Position"].Value.ToString();
            double rate = Convert.ToDouble(row.Cells["Daily Rate"].Value);


            if (Projects.EditMember.ShowCustomDialog(employee, rate))
            {
                row.Cells["Daily Rate"].Value = EditMember.rate;

                if (Projects.EditMember.toDelete)
                {
                    dataGridView2.Rows.RemoveAt(e.RowIndex);
                }
            }
        }
    }
}
