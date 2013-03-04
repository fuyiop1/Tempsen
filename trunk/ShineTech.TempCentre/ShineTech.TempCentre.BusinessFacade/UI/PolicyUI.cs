using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.DAL;
namespace ShineTech.TempCentre.BusinessFacade
{
    public class PolicyUI
    {
        #region controls
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox tbFolder;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.MaskedTextBox mtbPwdSize;
        private System.Windows.Forms.MaskedTextBox mtbPwdExpired;
        private System.Windows.Forms.MaskedTextBox mtbInactivity;
        private System.Windows.Forms.MaskedTextBox mtbLocked;
        #endregion
        private Form form;
        private int id;
        private IDataProcessor processor;
        private void ConstructForms(Form form)
        {
            btnBrowse = form.Controls.Find("btnBrowse", true)[0] as Button;
            tbFolder = form.Controls.Find("tbFolder", true)[0] as TextBox;
            btnOk = form.Controls.Find("btnOk", true)[0] as Button;
            btnCancel = form.Controls.Find("btnCancel", true)[0] as Button;
            mtbPwdSize = form.Controls.Find("mtbPwdSize", true)[0] as MaskedTextBox;
            mtbPwdExpired = form.Controls.Find("mtbPwdExpired", true)[0] as MaskedTextBox;
            mtbLocked = form.Controls.Find("mtbLocked", true)[0] as MaskedTextBox;
            mtbInactivity = form.Controls.Find("mtbInactivity", true)[0] as MaskedTextBox;
        }
        public PolicyUI(Form form)
        {
            this.form = form;
            this.ConstructForms(form);
            InitEvent();
            InitPolicy();
        }
        private void InitEvent()
        {
            this.btnCancel.Click+=new EventHandler(delegate(object sender,EventArgs args){
                this.form.Close();
            });
            this.btnBrowse.Click += new EventHandler(delegate(object sender, EventArgs args)
            {
                FolderBrowserDialog folder = new FolderBrowserDialog();
                if (folder.ShowDialog() == DialogResult.OK)
                {
                    this.tbFolder.Text = folder.SelectedPath;
                }
            });
            this.btnOk.Click += new EventHandler(delegate(object sender, EventArgs args)
            {
                if(id==0)
                    Common.Policy.ID = Common.Policy.ID+1;
                Common.Policy.MinPwdSize = Convert.ToInt32(this.mtbPwdSize.Text);
                Common.Policy.PwdExpiredDay = Convert.ToInt32(this.mtbPwdExpired.Text);
                Common.Policy.LockedTimes = Convert.ToInt32(this.mtbLocked.Text);
                Common.Policy.ProfileFolder = this.tbFolder.Text;
                Common.Policy.InactivityTime = Convert.ToInt32( this.mtbInactivity.Text);
                Common.Policy.Remark = DateTime.Now.ToString();
                processor = new DeviceProcessor();
                if(processor.InsertOrUpdate<Policy>(Common.Policy,null, id == 0 ? true : false))
                    MessageBox.Show("Saved Sucessfully", "OK", MessageBoxButtons.OK);
                else
                    MessageBox.Show("Saved Failure", "Error", MessageBoxButtons.OK);
            });
        }
        private void InitPolicy()
        {
            if (Common.Policy != null)
            {
                this.mtbInactivity.Text = Common.Policy.InactivityTime.ToString();
                this.mtbPwdSize.Text = Common.Policy.MinPwdSize.ToString();
                this.mtbPwdExpired.Text = Common.Policy.PwdExpiredDay.ToString();
                this.mtbLocked.Text = Common.Policy.LockedTimes.ToString();
                this.tbFolder.Text = Common.Policy.ProfileFolder;
                id = Common.Policy.ID;
            }
            else
            {

            }
        }
    }
}
