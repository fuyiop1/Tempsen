using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShineTech.TempCentre.DAL;
using System.Windows.Forms;
using System.Data;
namespace ShineTech.TempCentre.BusinessFacade
{
    public class AdministrationUI
    {
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tp1;
        private System.Windows.Forms.TabPage tp2;
        private System.Windows.Forms.TabPage tp3;
        private System.Windows.Forms.TabPage tp4;
        private System.Windows.Forms.TabPage tp5;
        private System.Windows.Forms.DataGridView dgvUser;
        private System.Windows.Forms.Button btnDisUser;
        private System.Windows.Forms.Button btnEditUser;
        private System.Windows.Forms.Button btnAddUser;

        private Form form;
        private UserInfoBLL _userBll;
        private void ConstructForms(Form form)
        {
            this.form = form;
            dgvUser = form.Controls.Find("dgvUser", true)[0] as DataGridView;
            btnDisUser = form.Controls.Find("btnDisUser", true)[0] as Button;
            btnEditUser = form.Controls.Find("btnEditUser", true)[0] as Button;
            btnAddUser = form.Controls.Find("btnAddUser", true)[0] as Button;
            tabControl1 = form.Controls.Find("tabControl1", true)[0] as TabControl;
            tp1 = form.Controls.Find("tp1", true)[0] as TabPage;
            tp2 = form.Controls.Find("tp2", true)[0] as TabPage;
            tp3 = form.Controls.Find("tp3", true)[0] as TabPage;
            tp4 = form.Controls.Find("tp4", true)[0] as TabPage;
            tp5 = form.Controls.Find("tp5", true)[0] as TabPage;
        }
        public AdministrationUI(Form form)
        {
            this.ConstructForms(form);
            this.InitUsers();
        }

        public void InitUsers()
        {
            if (_userBll == null)
                _userBll = new UserInfoBLL();
            DataTable dt = _userBll.GetUserInfoByInit();
            if (dt != null)
                this.dgvUser.DataSource = dt;
            for (int i = 0; i < dgvUser.Columns.Count; i++)
            {
                this.dgvUser.Columns[i].Width = 120;
            }

        }
        
    }
}
