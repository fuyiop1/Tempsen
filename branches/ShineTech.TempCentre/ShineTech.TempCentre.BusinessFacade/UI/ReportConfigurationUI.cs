using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.DAL;
using ShineTech.TempCentre.Platform;

namespace ShineTech.TempCentre.BusinessFacade
{
    public class ReportConfigurationUI
    {
        #region form controls
        private System.Windows.Forms.CheckBox cbCompanyName;
        private System.Windows.Forms.CheckBox cbReportTitle;
        private System.Windows.Forms.CheckBox cbLogo;
        private System.Windows.Forms.CheckBox cbWebSite;
        private System.Windows.Forms.CheckBox cbEMail;
        private System.Windows.Forms.CheckBox cbFax;
        private System.Windows.Forms.CheckBox cbContactPhone;
        private System.Windows.Forms.CheckBox cbAdress;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.TextBox tbWebSite;
        private System.Windows.Forms.TextBox tbEMail;
        private System.Windows.Forms.TextBox tbFax;
        private System.Windows.Forms.TextBox tbContactPhone;
        private System.Windows.Forms.TextBox tbAdress;
        private System.Windows.Forms.TextBox tbCompanyName;
        private System.Windows.Forms.TextBox tbReportTitle;
        private System.Windows.Forms.Button btnSave;
        #endregion
        private Form _Form;
        private IDataProcessor processor;
        private int? ID;//报表配置记录ID
        private ReportConfig rc;
        public ReportConfigurationUI(Form form)
        {
            this.ConstructForms(form);
            _Form = form;
            processor = new DeviceProcessor();
            InitEvents();
            _Form.Load += new EventHandler(LoadReportConfiguration);

        }
        private void ConstructForms(Form form)
        {
            cbCompanyName = form.Controls.Find("cbCompanyName", true)[0] as CheckBox;
            cbReportTitle = form.Controls.Find("cbReportTitle", true)[0] as CheckBox;
            cbLogo = form.Controls.Find("cbLogo", true)[0] as CheckBox;
            cbWebSite = form.Controls.Find("cbWebSite", true)[0] as CheckBox;
            cbEMail = form.Controls.Find("cbEMail", true)[0] as CheckBox;
            cbFax = form.Controls.Find("cbFax", true)[0] as CheckBox;
            cbContactPhone = form.Controls.Find("cbContactPhone", true)[0] as CheckBox;
            cbAdress = form.Controls.Find("cbAdress", true)[0] as CheckBox;
            cbEMail = form.Controls.Find("cbEMail", true)[0] as CheckBox;
            btnOpen = form.Controls.Find("btnOpen", true)[0] as Button;
            btnSave = form.Controls.Find("btnSave", true)[0] as Button;
            pbLogo = form.Controls.Find("pbLogo", true)[0] as PictureBox;
            tbWebSite = form.Controls.Find("tbWebSite", true)[0] as TextBox;
            tbEMail = form.Controls.Find("tbEMail", true)[0] as TextBox;
            tbFax = form.Controls.Find("tbFax", true)[0] as TextBox;
            tbContactPhone = form.Controls.Find("tbContactPhone", true)[0] as TextBox;
            tbAdress = form.Controls.Find("tbAdress", true)[0] as TextBox;
            tbCompanyName = form.Controls.Find("tbCompanyName", true)[0] as TextBox;
            tbReportTitle = form.Controls.Find("tbReportTitle", true)[0] as TextBox;
        }
        public void LoadReportConfiguration(object sender,EventArgs args)
        {
            rc = processor.QueryOne<ReportConfig>("SELECT * FROM ReportConfig", delegate() { return null; });
            if (rc != null)
            {
                ID = rc.Id;
                this.tbReportTitle.Text = rc.ReportTitle;
                this.tbAdress.Text = rc.Adress;
                this.tbCompanyName.Text = rc.CompanyName;
                this.tbEMail.Text = rc.Email;
                this.tbFax.Text = rc.Fax;
                this.tbWebSite.Text = rc.WebSite;
                this.tbContactPhone.Text = rc.ContactPhone;
                if (rc.Logo != null&&rc.Logo.Length>0)
                {
                    this.pbLogo.Image = Utils.ReadSource(rc.Logo);
                }
            }
        }
        private void InitEvents()
        {
            #region checkbox
            cbCompanyName.CheckedChanged += new EventHandler(delegate(object sender, EventArgs args)
            {
                this.tbCompanyName.Enabled = cbCompanyName.Checked;
                this.tbCompanyName.Focus();
            });
            cbAdress.CheckedChanged += new EventHandler(delegate(object sender, EventArgs args)
            {
                this.tbAdress.Enabled = this.cbAdress.Checked; tbAdress.Focus();
            });
            this.cbContactPhone.CheckedChanged += new EventHandler(delegate(object sender, EventArgs args)
            {
                this.tbContactPhone.Enabled = this.cbContactPhone.Checked; tbContactPhone.Focus();
            });
            this.cbEMail.CheckedChanged += new EventHandler(delegate(object sender, EventArgs args)
            {
                this.tbEMail.Enabled = this.cbEMail.Checked; tbEMail.Focus();
            });
            this.cbFax.CheckedChanged += new EventHandler(delegate(object sender, EventArgs args)
            {
                this.tbFax.Enabled = this.cbFax.Checked; tbFax.Focus();
            });
            this.cbReportTitle.CheckedChanged += new EventHandler(delegate(object sender, EventArgs args)
            {
                this.tbReportTitle.Enabled = this.cbReportTitle.Checked; tbReportTitle.Focus();
            });
            this.cbWebSite.CheckedChanged += new EventHandler(delegate(object sender, EventArgs args)
            {
                this.tbWebSite.Enabled = this.cbWebSite.Checked; tbWebSite.Focus();
            });
            this.cbLogo.CheckedChanged += new EventHandler(delegate(object sender, EventArgs args)
            {
                this.btnOpen.Enabled = this.cbLogo.Checked; btnOpen.Focus();
            });
            #endregion
            #region button
            this.btnOpen.Click+=new EventHandler(delegate(object sender,EventArgs args){
                OpenFileDialog file = new OpenFileDialog();
                if (file.ShowDialog() == DialogResult.OK)
                {
                    string src = file.FileName.ToString();
                    pbLogo.Image = System.Drawing.Image.FromFile(src);
                }
                
            });
            this.btnSave.Click += new EventHandler(delegate(object sender, EventArgs args) {
                ReportConfig config;
                bool flag = false;
                if (ID == 0)
                {
                    config = new ReportConfig();
                    config.Id = (int)(ID + 1);
                    flag = true;
                }
                else
                {
                    config = rc;
                    flag = false;
                }                 
                config.ReportTitle = this.tbReportTitle.Text;
                config.CompanyName = this.tbCompanyName.Text;
                config.ContactPhone = this.tbCompanyName.Text;
                config.Email = this.tbEMail.Text;
                config.Fax = this.tbFax.Text;
                config.WebSite = this.tbWebSite.Text;
                config.Adress = this.tbAdress.Text;
                if(pbLogo.Image!=null)
                    config.Logo = Utils.CopyToBinary(pbLogo.Image);
                if (processor.InsertOrUpdate<ReportConfig>(config,null,flag))
                    MessageBox.Show("Saved Corrected","OK",MessageBoxButtons.OK);
                else
                    MessageBox.Show("Saved Failure","Error", MessageBoxButtons.OK);

            });
            #endregion
        }
    }
}
