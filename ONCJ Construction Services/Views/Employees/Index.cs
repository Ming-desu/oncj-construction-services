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

namespace ONCJ_Construction_Services.Views.Employees
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
            Employee employee = new Employee();
            List<Employee> employees = employee.Read(query);

            DataTable dt = new DataTable();
            dt.Columns.Add("Employee Picture", typeof(byte[]));
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
                Image image = (Image)(new Bitmap(Image.FromFile(Directory.GetCurrentDirectory() + "\\Uploads\\" + emp.picture_url), 50, 50));
                dt.Rows.Add(imageToByteArray(image), emp.ID, emp.first_name, emp.last_name, emp.position);
            }
        }

        private byte[] imageToByteArray(System.Drawing.Image imageIn)
        {
            MemoryStream ms = new MemoryStream();
            imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
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

                Employee employee = new Employee();
                employee.ID = id;
                employee = employee.Find();

                if (employee == null)
                {
                    AppController.ShowMessage("Employee does not exists.", "Notification");
                    return;
                }

                if (Employees.Edit.ShowCustomDialog(employee.Find()))
                {
                    LoadTable();
                }
            }
        }
    }
}
