using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.BusinessFacade;
namespace ShineTech.TempCentre.DeviceManage
{
    public partial class UserAccount : Form
    {
        private UserAccountUI ui;
        public UserAccount()
        {
            InitializeComponent();
            ui = new UserAccountUI(this);
        }
        public UserAccount(string username)
        {
            InitializeComponent();
            ui = new UserAccountUI(this,username);
        }

        private void UserAccount_Load(object sender, EventArgs e)
        {

        }
    }
}
