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
    public partial class Register : Form
    {
        public Register()
        {
            InitializeComponent();
        }

        private void BtnRegister_Click(object sender, EventArgs e)
        {
            User user = new User();
            user.username = TxtUsername.Text;
            user.password = TxtPassword.Text;
            user.first_name = TxtFirstName.Text;
            user.last_name = TxtLastName.Text;

            if (user.Create())
            {
                AppController.ShowMessage("Successfully created an account.", "Notification");
                AppController.ShowForm(new Login());
            }
        }
    }
}
