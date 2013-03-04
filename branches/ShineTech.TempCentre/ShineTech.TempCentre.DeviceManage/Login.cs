using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.BusinessFacade;
using ShineTech.TempCentre.Platform;
using ShineTech.TempCentre.Versions;
namespace ShineTech.TempCentre.DeviceManage
{
    public partial class Login : Form
    {
        private LoginUI loginUI;
        private readonly int rawHeight;
        //public event EventHandler Shows;
        public Login()
        {
            InitializeComponent();
            loginUI = new LoginUI(this);
            if (Common.Versions == SoftwareVersions.S)
            {
                this.Load -= new EventHandler(Login_Load);
                ShowMainForm();
                return;
            }
            rawHeight = this.Size.Height;
           
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (loginUI.Login() && !loginUI.IsExpire())
            {
                this.tbPwd.Clear();
                this.lbAccount.Text = "";
                this.lbPwd.Text = "";
                //this.Visible = false;
                ShowMainForm();
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
                UserWizard wizard = new UserWizard(true);
                if (DialogResult.OK == wizard.ShowDialog(this))
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
        private void ShowMainForm()
        {
            this.Hide();
            DeviceManage dm = DeviceManage.GetInstance(this);
            if (dm != null && !dm.IsDisposed)
            {
                dm.WindowState = FormWindowState.Maximized;
                dm.Show();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
