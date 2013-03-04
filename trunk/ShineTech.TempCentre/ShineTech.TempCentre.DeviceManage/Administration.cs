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
    public partial class Administration : Form
    {
        AdministrationUI _admin;
        public Administration()
        {
            InitializeComponent();
            _admin = new AdministrationUI(this);
        }

        private void btnEditUser_Click(object sender, EventArgs e)
        {
            //if (this.dgvUser.SelectedRows.Count > 0)
            //{
            //    string username = this.dgvUser.SelectedRows[0].Cells["User Name"].Value.ToString();
            //    if (username != null && username != string.Empty)
            //    {
            //        UserProfile user = new UserProfile(username);
            //        if (user.ShowDialog() == DialogResult.OK)
            //            _admin.InitUsers();
            //    }

            //}
            //else
            //{
            //    MessageBox.Show("Please select the row!");
            //}
            ChangeDetail detail = new ChangeDetail();
            detail.ShowDialog(this);
        }
    }
}
