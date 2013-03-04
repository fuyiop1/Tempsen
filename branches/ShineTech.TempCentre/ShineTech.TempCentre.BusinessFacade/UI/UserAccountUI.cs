using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.DAL;
using ShineTech.TempCentre.Platform;

namespace ShineTech.TempCentre.BusinessFacade
{
    public class UserAccountUI
    {
        #region controls
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.TextBox tbFullName;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.TextBox tbPwd;
        private System.Windows.Forms.TextBox tbConfirm;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ComboBox cbxRole;

        private PictureBox pbPasswordTip;
        private PictureBox pbConfirmPasswordTip;
        private PictureBox pbFullNameTip;
        private PictureBox pbRoleTip;
        
        #endregion
        private ToolTip wrongTip = new System.Windows.Forms.ToolTip(new System.ComponentModel.Container());
        private Form form;
        private int userid;
        private IDataProcessor processor=new DeviceProcessor();
        private string username;
        private UserInfo user=new UserInfo();
        public UserAccountUI(Form form)
        {
            this.ConstructForms(form);
            this.form = form;
            this.InitEvents();
            this.Init();
        }
        private void initToolTipSetter()
        {
            if (this.wrongTip != null)
            {
                this.wrongTip.ShowAlways = true;
                this.wrongTip.AutoPopDelay = 0;
                this.wrongTip.AutomaticDelay = 0;
                this.wrongTip.InitialDelay = 0;
                this.wrongTip.ReshowDelay = 0;
            }
        }
        public UserAccountUI(Form form,string username)
        {
            this.initToolTipSetter();
            this.ConstructForms(form);
            this.form = form;
            this.username = username;
            this.InitEvents();
            this.Init();
            this.form.Text = InputBoxTitle.EditUser;
        }
        private void ConstructForms(Form form)
        {
            tbUserName = form.Controls.Find("tbUserName", true)[0] as TextBox;
            tbFullName = form.Controls.Find("tbFullName", true)[0] as TextBox;
            tbDescription = form.Controls.Find("tbDescription", true)[0] as TextBox;
            btnOK = form.Controls.Find("btnOK", true)[0] as Button;
            btnCancel = form.Controls.Find("btnCancel", true)[0] as Button;
            tbPwd = form.Controls.Find("tbPwd", true)[0] as TextBox;
            tbConfirm = form.Controls.Find("tbConfirm", true)[0] as TextBox;
            cbxRole = form.Controls.Find("cbxRole", true)[0] as ComboBox;
            pbPasswordTip = form.Controls.Find("pbPasswordTip", true)[0] as PictureBox;
            pbConfirmPasswordTip = form.Controls.Find("pbConfirmPasswordTip", true)[0] as PictureBox;
            pbFullNameTip = form.Controls.Find("pbFullNameTip", true)[0] as PictureBox;
            pbRoleTip = form.Controls.Find("pbRoleTip", true)[0] as PictureBox;
        }
        private void InitEvents()
        {
            this.btnOK.Click+=new EventHandler(delegate(object sender,EventArgs args){
                OK();
            });
            this.btnCancel.Click+=new EventHandler(delegate(object sender,EventArgs args){
                form.Close();
            });
            this.InitEventsForTextBoxValidation();
            //非法字符处理事件
            this.tbFullName.TextChanged += new EventHandler((sender, e) =>
            {
                Utils.IsInputTextValid(this.tbFullName);
            });
        }

        private void InitEventsForTextBoxValidation()
        {
            this.tbPwd.Leave += new EventHandler(delegate(object sender, EventArgs args)
            {
                if (Common.TextBoxChecked(tbPwd))
                {
                    //判断密钥长度
                    if (Common.Policy == null)
                    {
                        Common.ClearToolTip(this.wrongTip, this.pbPasswordTip);
                    }
                    else
                    {
                        if (tbPwd.Text.TrimEnd().Length >= Common.Policy.MinPwdSize)
                        {
                            Common.ClearToolTip(this.wrongTip, this.pbPasswordTip);
                        }
                        else
                        {
                            this.pbPasswordTip.Visible = true;
                            this.wrongTip.SetToolTip(this.pbPasswordTip, string.Format(Messages.PasswordShortThanDefined, Common.Policy.MinPwdSize));
                        }
                    }
                }
                else
                {
                    this.pbPasswordTip.Visible = true;
                    this.wrongTip.SetToolTip(this.pbPasswordTip, Messages.EmptyContentError);
                }
            });
            /*密码确认*/
            this.tbConfirm.Leave += new EventHandler(delegate(object sender, EventArgs args)
            {
                if (Common.TextBoxChecked(tbConfirm))
                {
                    if (Common.PasswordConfirmed(tbPwd, tbConfirm))
                    {
                        Common.ClearToolTip(this.wrongTip, this.pbConfirmPasswordTip);
                    }
                    else
                    {
                        this.pbConfirmPasswordTip.Visible = true;
                        this.wrongTip.SetToolTip(this.pbConfirmPasswordTip, Messages.MismatchPassword);
                    }
                }
                else
                {
                    this.pbConfirmPasswordTip.Visible = true;
                    this.wrongTip.SetToolTip(this.pbConfirmPasswordTip, Messages.EmptyContentError);
                }
            });

            this.tbFullName.Leave += new EventHandler((sender, args) =>{
                if (Common.TextBoxChecked(this.tbFullName))
                {
                    Common.ClearToolTip(this.wrongTip, this.pbFullNameTip);
                }
                else
                {
                    this.pbFullNameTip.Visible = true;
                    this.wrongTip.SetToolTip(this.pbFullNameTip, Messages.EmptyContentError);
                }
            });

            this.tbDescription.Leave += new EventHandler((sender, args) =>
            {
                if (Common.TextBoxChecked(this.tbDescription))
                {
                    Common.ClearToolTip(this.wrongTip, this.pbRoleTip);
                }
                else
                {
                    this.pbRoleTip.Visible = true;
                    this.wrongTip.SetToolTip(this.pbRoleTip, Messages.EmptyContentError);
                }
            });
        }
        private int GetCurrentUserId()
        {
            object u=processor.QueryScalar("select max(userid) from userinfo", null);
            if (u != null&&u.ToString()!=string.Empty)
                return Convert.ToInt32(u);
            else
                return 0;
        }
        public void Init()
        {
            List<RoleInfo> role = processor.Query<RoleInfo>("SELECT * FROM RoleInfo", null);
            if (role == null || role.Count == 0)
            {
                role.Add(new RoleInfo() { ID=1,Rolename="Admin",Remark=DateTime.Now.ToString() });
                role.Add(new RoleInfo() { ID = 2, Rolename = "User", Remark = DateTime.Now.ToString() });
                processor.Insert<RoleInfo>(role);
            }
            this.cbxRole.DataSource = role;
            this.cbxRole.DisplayMember = "Rolename";
            this.cbxRole.ValueMember = "ID";
            userid = this.GetCurrentUserId();
            if (username!=null&&username != string.Empty)
            {
                user = processor.QueryOne<UserInfo>("SELECT * FROM USERINFO WHERE username=@username COLLATE NOCASE", delegate()
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("username", username.Trim().ToLower());
                    return dic;
                });
                this.tbUserName.Text = user.UserName;
                this.tbFullName.Text = user.FullName;
                this.tbPwd.Text = user.Pwd;
                this.tbDescription.Text = user.Description;
                this.tbConfirm.Text = user.Pwd;
                switch (user.RoleId)
                {  
                    case 1:
                        this.cbxRole.SelectedValue = 1;
                        break;
                    case 2:
                        this.cbxRole.SelectedValue = 2;
                        break;
                    default:
                        break;
                }
                // 如果为普通用户，则隐藏锁定、Diable、Group修改选项
                if (Common.User.RoleId != 1 || Common.User.Userid == user.Userid)
                {
                    this.cbxRole.Enabled = false;
                }
                Common.SetControlEnable(this.tbUserName, false);
            }
            else
            {
                Common.SetControlEnable(this.tbUserName, true);
            }
        }
        private void OK()
        {
            string message = this.checkAllUserInfoFields();
            if (!string.IsNullOrEmpty(message))
            {
                Utils.ShowMessageBox(message, Messages.TitleError);
                return;
            }
            if (Common.TextBoxChecked(this.tbUserName) && Common.TextBoxChecked(this.tbFullName) &&
                    Common.TextBoxChecked(this.tbDescription) && Common.TextBoxChecked(this.tbPwd) &&
                   Common.TextBoxChecked(this.tbConfirm) && Common.PasswordConfirmed(tbPwd, tbConfirm))
            {
                /*密钥长度*/
                if (Common.Policy == null || Common.Policy.MinPwdSize > this.tbPwd.Text.Length)
                    return;
                
                bool isChangeGroup = false, isChangePwd = false, isEditUser = false,isDisableUser=false;
                /*记录日志*/
                if (user.RoleId != Convert.ToInt32(this.cbxRole.SelectedValue))
                    isChangeGroup = true;
                if (user.Pwd != tbPwd.Text)
                    isChangePwd = true;
                if (user.Description != tbDescription.Text || user.FullName != tbFullName.Text)
                    isEditUser = true;
                if (isChangePwd || isChangeGroup || isEditUser || isDisableUser)
                {
                    if (username == string.Empty)
                        user.Userid = ++userid;
                    user.Account = this.tbUserName.Text.TrimEnd();
                    user.FullName = this.tbFullName.Text.TrimEnd();
                    user.Description = this.tbDescription.Text.TrimEnd();
                    user.LastPwdChangedTime = this.tbPwd.Text == user.Pwd ? user.LastPwdChangedTime : DateTime.Now;
                    user.Pwd = this.tbPwd.Text.TrimEnd();
                    user.Remark = DateTime.Now.ToString();
                    user.RoleId = this.cbxRole.SelectedValue == null ? 1 : Convert.ToInt32(this.cbxRole.SelectedValue);
                    if (processor.InsertOrUpdate<UserInfo>(user, null, username == string.Empty ? true : false))
                    {
                        Utils.ShowMessageBox(Messages.SavedSuccessfully, Messages.TitleNotification);
                        if (isChangeGroup)
                            InsertChangeLog("Change group");
                        if (isChangePwd)
                            InsertChangeLog("Change password");
                        if (isEditUser)
                            InsertChangeLog("Edit user");
                        if (isDisableUser && user.Disabled != 0)
                            InsertChangeLog("Disable user");
                        form.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        Utils.ShowMessageBox(Messages.SavedFailed, Messages.TitleError);
                        form.DialogResult = DialogResult.No;
                    }
                }
                else
                    form.DialogResult = DialogResult.OK;
            }
        }
        private void InsertChangeLog(string action)
        {
            if (Common.User.UserName != Common.SUPERUSER)
            {
                new OperationLogBLL().InsertLog(() =>
                {
                    Dictionary<string, object> d = new Dictionary<string, object>();
                    switch (action)
                    {
                        case "Change group":
                            d.Add("Detail", tbUserName.Text + " : " + ((int)cbxRole.SelectedValue == 1 ? "Admin" : "User"));
                            break;
                        default:
                            d.Add("Detail", tbUserName.Text);
                            break;
                    }
                    d.Add("Action", action);
                    d.Add("UserName", Common.User.UserName);
                    d.Add("FullName", Common.User.FullName);
                    d.Add("OperateTime", DateTime.UtcNow);
                    d.Add("LogType", 0);
                    return d;
                });
            }
        }
        private string checkAllUserInfoFields()
        {
            StringBuilder message = new StringBuilder();
            string fullName = this.tbFullName.Text;
            string password = this.tbPwd.Text;
            string passwordConfirm = this.tbConfirm.Text;
            string role = this.tbDescription.Text;

            if (string.IsNullOrEmpty(fullName))
            {
                message.Append(string.Format("Full Name: {0}{1}", Messages.EmptyContentError, Environment.NewLine));
                this.pbFullNameTip.Visible = true;
                this.wrongTip.SetToolTip(this.pbFullNameTip, Messages.EmptyContentError);
            }

            if (string.IsNullOrEmpty(password))
            {
                message.Append(string.Format("Password: {0}{1}", Messages.EmptyContentError, Environment.NewLine));
                this.pbPasswordTip.Visible = true;
                this.wrongTip.SetToolTip(this.pbPasswordTip, Messages.EmptyContentError);
            }
            else
            {
                if (Common.Policy == null || Common.Policy.MinPwdSize > password.Length)
                {
                    message.Append(string.Format("Password: {0}{1}", string.Format(Messages.PasswordShortThanDefined, Common.Policy.MinPwdSize), Environment.NewLine));
                    this.pbPasswordTip.Visible = true;
                    this.wrongTip.SetToolTip(this.pbPasswordTip, string.Format(Messages.PasswordShortThanDefined, Common.Policy.MinPwdSize));
                }
            }
            if (string.IsNullOrEmpty(passwordConfirm))
            {
                message.Append(string.Format("Confirm Password: {0}{1}", Messages.EmptyContentError, Environment.NewLine));
                this.pbConfirmPasswordTip.Visible = true;
                this.wrongTip.SetToolTip(this.pbConfirmPasswordTip, Messages.EmptyContentError);
            }
            if (password != passwordConfirm)
            {
                message.Append(string.Format("Confirm Password: {0}{1}", Messages.MismatchPassword, Environment.NewLine));
                this.pbConfirmPasswordTip.Visible = true;
                this.wrongTip.SetToolTip(this.pbConfirmPasswordTip, Messages.MismatchPassword);
            }
            if (string.IsNullOrEmpty(role))
            {
                message.Append(string.Format("Role: {0}{1}", Messages.EmptyContentError, Environment.NewLine));
                this.pbRoleTip.Visible = true;
                this.wrongTip.SetToolTip(this.pbRoleTip, Messages.EmptyContentError);
            }
            return message.ToString();
        }
    }
}
