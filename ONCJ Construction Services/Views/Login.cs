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
using ONCJ_Construction_Services.Models;

namespace ONCJ_Construction_Services.Views
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, EventArgs e)
        {
            User user = new User();
            user.username = TxtUsername.Text;
            user.password = TxtPassword.Text;

            if (user.Login())
            {
                AppController.ShowMessage("Logged in", "Kamote");
                AppController.ShowForm(new MainApp());
            }
        }
    }
}
