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
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

namespace ONCJ_Construction_Services.Views.Projects
{
    public partial class Edit : Form
    {
        private DataTable dtMembers;

        public Edit()
        {
            InitializeComponent();

            LoadEmployeesTable();
        }

        private static Project project;
        private static Edit form;
        private static bool result = false;

        public static bool ShowCustomDialog(Project project)
        {
            Edit.project = project;
            form = new Edit();
            form.TxtSubject.Text = project.subject;
            form.DateStart.Value = project.date_start;
            form.DateEnd.Value = project.date_end;
            form.InitMembersTable(project.members);
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

        public void InitMembersTable(List<ProjectMember> members)
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

            if (members == null)
            {
                return;
            }

            foreach (ProjectMember member in members)
            {
                Employee employee = new Employee();
                employee.ID = member.employee_id;
                employee = employee.Find();

                dtMembers.Rows.Add(new object[]
                {
                    member.ID,
                    employee.last_name + ", " + employee.first_name,
                    employee.first_name,
                    employee.last_name,
                    employee.position,
                    member.rate
                });
            }
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void BtnSave_Click(object sender, EventArgs e)
        {
            Project project = new Project();
            project.ID = Edit.project.ID;
            project.subject = TxtSubject.Text;
            project.date_start = DateStart.Value;
            project.date_end = DateEnd.Value;

            //AppController.ShowMessage(DateStart.Value.ToString("yyyy-MM-dd HH:mm:ss") + "\n" + DateEnd.Value.ToString("yyyy-MM-dd HH:mm:ss"), "");
            //return;

            if (dataGridView2.Rows.Count < 0)
            {
                return;
            }

            bool success = project.Update();

            // Delete the previous members of this project
            ProjectMember x = new ProjectMember();
            x.project_id = project.ID;
            x.Delete();

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                ProjectMember member = new ProjectMember();
                member.project_id = project.ID;
                member.employee_id = Convert.ToInt32(row.Cells["ID"].Value);
                member.rate = Convert.ToDouble(row.Cells["Daily Rate"].Value);
                member.Create();

                project.members.Add(member);
            }

            if (success)
            {
                AppController.ShowMessage("Successfully updated a project.", "Notification");
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

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this project?", "Alert", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
            {
                return;
            }

            if (Edit.project.Delete())
            {
                AppController.ShowMessage("Successfully deleted a project.", "Notification");
                result = true;
                this.Close();
            }
        }

        private void BtnExport_Click(object sender, EventArgs evt)
        {
            Excel.Application app;
            Excel._Workbook book;
            Excel._Worksheet sheet;
            Excel.Range range;

            try
            {
                app = new Excel.Application();
                app.Visible = true;

                book = (Excel._Workbook) (app.Workbooks.Add(Missing.Value));
                sheet = (Excel._Worksheet) book.ActiveSheet;

                sheet.Cells[1, 1] = "PAYROLL";

                sheet.Cells[2, 1] = "Project: ";
                sheet.Cells[2, 3] = TxtSubject.Text;
                sheet.Cells[3, 1] = "SubCon: ";
                sheet.Cells[3, 3] = "ONCJ";

                sheet.Cells[2, 7] = "Date Covered: ";
                sheet.Cells[2, 9] = DateStart.Value.ToString("MMM dd") + " - " + DateEnd.Value.ToString("MMM dd, yyyy");

                range = sheet.get_Range("A1", "K1");
                range.Font.Bold = true;
                range.Font.Size = 24;
                range.HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                sheet.get_Range("A1", "K1").Merge();
                sheet.get_Range("A2", "B2").Merge();
                sheet.get_Range("A3", "B3").Merge();
                sheet.get_Range("C2", "C3").Font.Bold = true;
                sheet.get_Range("I2", "K2").Font.Bold = true;
                sheet.get_Range("I2", "K2").HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
                sheet.get_Range("I2", "K2").Merge();

                List<string> columns = new List<string>()
                {
                    "NO.", "NAME", "Days", "Rate", "Total", "O.T.", "O.T. Total", "Balance", "Advances", "Net Pay", "Signature"
                };

                for (int i = 0; i < columns.Count; i++)
                {
                    sheet.Cells[4, i + 1] = columns[i];
                }

                sheet.get_Range("A4", "K4").Font.Bold = true;
                sheet.get_Range("A4", "K4").VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;

                // Excel Data
                TimeSpan diff = DateEnd.Value.Subtract(DateStart.Value);

                for (int i = 0, rowIndex = 5; i < dataGridView2.Rows.Count; i++)
                {
                    DataGridViewRow row = dataGridView2.Rows[i];
                    sheet.Cells[rowIndex + i, 1] = i + 1;
                    sheet.Cells[rowIndex + i, 2] = row.Cells["Name"].Value.ToString();
                    sheet.Cells[rowIndex + i, 3] = diff.Days + 1;
                    sheet.Cells[rowIndex + i, 4] = Convert.ToDouble(row.Cells["Daily Rate"].Value);
                    sheet.Cells[rowIndex + i, 6] = 10.0;
                }

                sheet.get_Range("C5", "C" + (5 + dataGridView2.Rows.Count - 1).ToString()).NumberFormat = "0.00";
                sheet.get_Range("D5", "D" + (5 + dataGridView2.Rows.Count - 1).ToString()).NumberFormat = "$#,##0.00";

                range = sheet.get_Range("E5", "E" + (5 + dataGridView2.Rows.Count - 1).ToString());
                range.Formula = "=C5*D5";
                range.NumberFormat = "$#,##0.00";

                range = sheet.get_Range("G5", "G" + (5 + dataGridView2.Rows.Count - 1).ToString());
                range.Formula = "=(D5/8)*F5";
                range.NumberFormat = "$#,##0.00";

                range = sheet.get_Range("J5", "J" + (5 + dataGridView2.Rows.Count - 1).ToString());
                range.Formula = "=E5+G5";
                range.NumberFormat = "$#,##0.00";

                // Autofit every columns
                sheet.get_Range("A1", "K1").EntireColumn.AutoFit();

                //Make sure Excel is visible and give the user control
                //of Microsoft Excel's lifetime.
                app.Visible = true;
                app.UserControl = true;
            }
            catch (Exception e)
            {
                AppController.ShowMessage("Error: " + e.Message, "Error");
            }
        }
    }
}
