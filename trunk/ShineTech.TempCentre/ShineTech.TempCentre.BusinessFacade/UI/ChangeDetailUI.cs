using System;
using System.Collections.Generic;
using System.Linq;
using ShineTech.TempCentre.DAL;
using System.Windows.Forms;

namespace ShineTech.TempCentre.BusinessFacade
{
    public class ChangeDetailUI
    {
        #region controls
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.TextBox tbPwd;
        private System.Windows.Forms.TextBox tbNewPwd;
        private System.Windows.Forms.TextBox tbConfirm;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lbAlarmUn;
        private System.Windows.Forms.Label lbAlarmPwd;
        private System.Windows.Forms.Label lbAlarmNewPwd;
        private System.Windows.Forms.Label lbAlarmConfirm;
        #endregion
        private Form form;
        private IDataProcessor processor;
        private void ConstructForms(Form form)
        {
            btnOK = form.Controls.Find("btnOK", true)[0] as Button;
            btnCancel = form.Controls.Find("btnCancel", true)[0] as Button;
            tbUserName = form.Controls.Find("tbUserName", true)[0] as TextBox;
            tbPwd = form.Controls.Find("tbPwd", true)[0] as TextBox;
            tbNewPwd = form.Controls.Find("tbNewPwd", true)[0] as TextBox;
            tbConfirm = form.Controls.Find("tbConfirm", true)[0] as TextBox;
            lbAlarmUn = form.Controls.Find("lbAlarmUn", true)[0] as Label;
            lbAlarmPwd = form.Controls.Find("lbAlarmPwd", true)[0] as Label;
            lbAlarmNewPwd = form.Controls.Find("lbAlarmNewPwd", true)[0] as Label;
            lbAlarmConfirm = form.Controls.Find("lbAlarmConfirm", true)[0] as Label;
            this.form = form;
        }
        public ChangeDetailUI(Form form)
        {
            this.ConstructForms(form);
            SetUserName();
            InitEvents();
        }
        private void SetUserName()
        {
            this.tbUserName.Text = Common.User.UserName;
        }
        private void InitEvents()
        {
            /*关闭窗口*/
            this.btnCancel.Click += new EventHandler(delegate(object sender, EventArgs args)
                {
                    this.form.Close();
                });
            this.tbPwd.Leave += new EventHandler(delegate(object sender, EventArgs args)
            {
                if (Common.TextBoxChecked(tbPwd))
                {
                    if (this.tbPwd.Text.TrimEnd() == Common.User.Pwd)
                        this.lbAlarmPwd.Text = "√ correct password.";
                    else
                    {
                        this.tbPwd.Focus();
                        this.lbAlarmPwd.Text = "x wrong password.";
                    }
                }
                else
                {
                    this.tbPwd.Focus();
                    this.lbAlarmPwd.Text = "x none content.";
                }
            });
            /*新密码校验*/
            this.tbNewPwd.Leave += new EventHandler(delegate(object sender, EventArgs args)
            {
                if (Common.TextBoxChecked(tbNewPwd))
                {
                    //判断密钥长度
                    if (Common.Policy == null)
                        this.lbAlarmNewPwd.Text = "√";
                    else
                    {
                        if (tbNewPwd.Text.TrimEnd().Length >= Common.Policy.MinPwdSize)
                        {
                            if (tbNewPwd.Text.TrimEnd() != Common.User.Pwd)
                                this.lbAlarmNewPwd.Text = "√";
                            else
                            {
                                this.lbAlarmNewPwd.Text = "x same as current.";
                                this.tbNewPwd.Focus();
                            }
                        }
                        else
                        {
                            this.lbAlarmNewPwd.Text = "x min pwd size is " + Common.Policy.MinPwdSize.ToString();
                            this.tbNewPwd.Focus();
                        }
                    }
                }
                else if (!Common.TextBoxChecked(tbNewPwd))
                {
                    this.tbNewPwd.Focus();
                    this.lbAlarmNewPwd.Text = "x none content";
                }

                if (Common.PasswordConfirmed(tbNewPwd, tbConfirm))
                    this.lbAlarmConfirm.Text = "√";
            });
            /*密码确认*/
            this.tbConfirm.Leave += new EventHandler(delegate(object sender, EventArgs args)
                {
                    if (Common.TextBoxChecked(tbConfirm))
                    {
                        if (Common.PasswordConfirmed(tbNewPwd, tbConfirm))
                            this.lbAlarmConfirm.Text = "√";
                        else
                        {
                            this.lbAlarmConfirm.Text = "x confirmed failure";
                        }
                    }
                    else
                    {
                        this.tbConfirm.Focus();
                        this.lbAlarmConfirm.Text = "x none content";
                    }
                });
            /*保存修改后的密码*/
            this.btnOK.Click += new EventHandler(delegate(object sender, EventArgs args)
                {
                    OK();
                });
            /*确认密码回车*/
            this.tbConfirm.KeyPress += new KeyPressEventHandler(delegate(object sender, KeyPressEventArgs args)
            {
                if (args.KeyChar == 13)
                    OK();
            });
        }
        private void OK()
        {
            if (Common.TextBoxChecked(this.tbPwd) && Common.TextBoxChecked(this.tbNewPwd) &&
                   Common.TextBoxChecked(this.tbConfirm) && Common.PasswordConfirmed(tbNewPwd, tbConfirm))
            {
                Common.User.Pwd = this.tbNewPwd.Text.TrimEnd();
                if (processor == null)
                    processor = new DeviceProcessor();
                if (processor.Update<UserInfo>(Common.User, null))
                    MessageBox.Show("Change Successfully.");
                else
                    MessageBox.Show("Change Failure.");
                form.Close();
            }
        }
    }
}
/*
╭━━灬╮╭━━∞╮ .︵ 
┃⌒　⌒┃┃⌒　⌒┃ (の) 
┃┃　┃┃┃▂　▂┃╱︶ 
╰━━━〇〇━━━〇 
 */
