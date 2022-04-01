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
using ONCJ_Construction_Services.Views;

namespace ONCJ_Construction_Services
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            AppController.MainForm = this;
            InitLogin();
            //AppController.ShowForm(new MainApp());
        }

        private void InitLogin()
        {
            if (User.HasUser())
            {
                AppController.ShowForm(new Login());
            }
            else
            {
                AppController.ShowForm(new Register());
            }
        }
    }
}
