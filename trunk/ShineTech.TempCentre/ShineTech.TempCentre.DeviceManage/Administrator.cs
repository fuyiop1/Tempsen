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
    public partial class Administrator : Form
    {
        private AdministratorUI ui;
        public Administrator()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            UserProfile user = new UserProfile("");
            if (user.ShowDialog() == DialogResult.OK)
                ui.InitUsers();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnPolicy_Click(object sender, EventArgs e)
        {
            Policy policy = new Policy();
            policy.ShowDialog(this);
            //this.Close();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (this.dgvUser.SelectedRows.Count > 0)
            {
                string username = this.dgvUser.SelectedRows[0].Cells["User Name"].Value.ToString();
                if (username != null && username != string.Empty)
                {
                    UserProfile user = new UserProfile(username);
                    if (user.ShowDialog() == DialogResult.OK)
                        ui.InitUsers();
                }

            }
            else
            {
                MessageBox.Show("Please select the row!");
            }
        }

        private void btnAddMean_Click(object sender, EventArgs e)
        {
            Meanings mean = new Meanings(null);
            if (mean.ShowDialog() == DialogResult.OK)
            {
                ui.InitMeaning();
                ui.UserSelectedChange();
            }
        }

        private void btnEditMean_Click(object sender, EventArgs e)
        {
            object obj1 = this.clbMeaning.SelectedItem ;
            if (obj1 != null )
            {
                Dictionary<int, string> dic = new Dictionary<int, string>();
                ShineTech.TempCentre.DAL.Meanings m = obj1 as ShineTech.TempCentre.DAL.Meanings;
                dic.Add(m.Id,m.Desc);
                Meanings mean = new Meanings(dic);
                if (mean.ShowDialog() == DialogResult.OK)
                {
                    ui.InitMeaning();
                    ui.UserSelectedChange();
                }
            }
            else
                MessageBox.Show("Please select the row!");
        }

        private void Administrator_Load(object sender, EventArgs e)
        {

            ui = new AdministratorUI(this);
        }
    }
}
