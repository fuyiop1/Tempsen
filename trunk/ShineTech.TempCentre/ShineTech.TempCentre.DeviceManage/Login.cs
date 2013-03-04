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
    public partial class Login : Form
    {
        private LoginUI loginUI;
        //public event EventHandler Shows;
        public Login()
        {
            InitializeComponent();
            loginUI = new LoginUI(this);
            
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (loginUI.Login())
            {
                //this.tbAccount.Focus();
                //this.tbAccount.Clear();
                this.tbPwd.Clear();
                this.lbAccount.Text = "";
                this.lbPwd.Text = "";
                this.Visible = false;
               
                DeviceManage dm = DeviceManage.GetInstance(this);
                if(dm!=null&&!dm.IsDisposed)
                    dm.Show();
                
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {
            if (loginUI.QueryUser())
                return;
            else
            {
                //Administrator admin = new Administrator();
                //admin.ShowDialog();
                //this.Close();
                this.Hide();
                UserWizard wizard = new UserWizard(false);
                if (DialogResult.OK == wizard.ShowDialog())
                    this.Show();
                else
                    this.Close();
            }

        }

        private void tbPwd_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                btnLogin_Click(sender, e);
            }
        }
    }
}
