using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.BusinessFacade;
using ShineTech.TempCentre.DAL;

namespace ShineTech.TempCentre.DeviceManage
{
    public partial class Administration : Form
    {
        AdministrationUI _admin;
        
        public Administration()
        {
            InitializeComponent();
            _admin = new AdministrationUI(this);
        }
        public void SetTabPage()
        {
            this.tabControl1.SelectedTab = tp4;
        }
        private void btnEditUser_Click(object sender, EventArgs e)
        {
            if (this.dgvUser.SelectedRows.Count > 0)
            {
                string username = this.dgvUser.SelectedRows[0].Cells["User Name"].Value.ToString();
                if (username != null && username != string.Empty)
                {
                    UserAccount user = new UserAccount(username);
                    user.BringToFront();
                    if (user.ShowDialog() == DialogResult.OK)
                        _admin.InitUsers();
                    user.Dispose();
                }

            }
        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            UserWizard wizard = new UserWizard(false);
            if (DialogResult.OK == wizard.ShowDialog())
            {
                _admin.InitMeaning();
                _admin.InitUsers();
            }
        }

    }
}
