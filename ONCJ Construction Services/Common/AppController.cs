using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ONCJ_Construction_Services.Common
{
    public class AppController
    {
        public static Form MainForm;

        public static void ShowMessage(string message, string title, MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            MessageBox.Show(MainForm, message, title, MessageBoxButtons.OK, icon);
        }

        public static void ShowForm(Form form)
        {
            foreach (var c in MainForm.Controls)
            {
                if (c is Form)
                {
                    (c as Form).Close();
                }
            }

            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            form.Parent = MainForm;
            form.Show();
        }
    }
}
