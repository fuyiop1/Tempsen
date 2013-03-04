using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.DAL;

namespace ShineTech.TempCentre.BusinessFacade
{
    public class UserProfileUI
    {
        #region controls
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.TextBox tbFullName;
        private System.Windows.Forms.TextBox tbDescription;
        private System.Windows.Forms.TextBox tbPwd;
        private System.Windows.Forms.TextBox tbConfirm;
        private System.Windows.Forms.CheckBox cbLocked;
        private System.Windows.Forms.CheckBox cbChangePwd;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lbAlarmUn;
        private System.Windows.Forms.Label lbAlarmFN;
        private System.Windows.Forms.Label lbAlarmDesc;
        private System.Windows.Forms.Label lbAlarmPwd;
        private System.Windows.Forms.Label lbAlarmConfirm;
        private System.Windows.Forms.ComboBox cbxRole;
        #endregion
        private Form form;
        private int userid;
        private IDataProcessor processor=new DeviceProcessor();
        private string username;
        private UserInfo user=new UserInfo();
        public UserProfileUI(Form form)
        {
            this.ConstructForms(form);
            this.form = form;
            this.InitEvents();
            this.Init();
        }
        public UserProfileUI(Form form,string username)
        {
            this.ConstructForms(form);
            this.form = form;
            this.username = username;
            this.InitEvents();
            this.Init();
        }
        private void ConstructForms(Form form)
        {
            tbUserName = form.Controls.Find("tbUserName", true)[0] as TextBox;
            tbFullName = form.Controls.Find("tbFullName", true)[0] as TextBox;
            tbDescription = form.Controls.Find("tbDescription", true)[0] as TextBox;
            cbLocked = form.Controls.Find("cbLocked", true)[0] as CheckBox;
            cbChangePwd = form.Controls.Find("cbChangePwd", true)[0] as CheckBox;
            btnOK = form.Controls.Find("btnOK", true)[0] as Button;
            btnCancel = form.Controls.Find("btnCancel", true)[0] as Button;
            tbPwd = form.Controls.Find("tbPwd", true)[0] as TextBox;
            tbConfirm = form.Controls.Find("tbConfirm", true)[0] as TextBox;
            lbAlarmUn = form.Controls.Find("lbAlarmUn", true)[0] as Label;
            lbAlarmFN = form.Controls.Find("lbAlarmFN", true)[0] as Label;
            lbAlarmDesc = form.Controls.Find("lbAlarmDesc", true)[0] as Label;
            lbAlarmPwd = form.Controls.Find("lbAlarmPwd", true)[0] as Label;
            lbAlarmConfirm = form.Controls.Find("lbAlarmConfirm", true)[0] as Label;
            cbxRole = form.Controls.Find("cbxRole", true)[0] as ComboBox;
        }
        private void InitEvents()
        {
            this.btnOK.Click+=new EventHandler(delegate(object sender,EventArgs args){
                if (Common.TextBoxChecked(this.tbUserName) /*&& Common.TextBoxChecked(this.tbFullName) &&
                    Common.TextBoxChecked(this.tbDescription)*/ && Common.TextBoxChecked(this.tbPwd) &&
                    Common.TextBoxChecked(this.tbConfirm) && Common.PasswordConfirmed(tbPwd, tbConfirm))
                { 
                    /*密钥长度*/
                    if (Common.Policy == null || Common.Policy.MinPwdSize > this.tbPwd.Text.Length)
                        return;
                    if(username==string.Empty)
                        user.Userid = ++userid;
                    user.Account = this.tbUserName.Text.TrimEnd();
                    user.FullName = this.tbFullName.Text.TrimEnd();
                    user.Description = this.tbDescription.Text.TrimEnd();
                    user.LastPwdChangedTime = this.tbPwd.Text == user.Pwd ? user.LastPwdChangedTime : DateTime.Now;
                    user.Pwd = this.tbPwd.Text.TrimEnd();
                    user.Locked = this.cbLocked.Checked == true ? 1 : 0;
                    user.ChangePwd = this.cbChangePwd.Checked == true ? 1 : 0;
                    user.Remark = DateTime.Now.ToString();
                    user.RoleId = this.cbxRole.SelectedValue==null ? 1: Convert.ToInt32(this.cbxRole.SelectedValue);
                    if (processor.InsertOrUpdate<UserInfo>(user, null, username == string.Empty ? true : false))
                    {
                        MessageBox.Show("Saved Successfully");
                        form.DialogResult = DialogResult.OK;
                    }
                    else
                    {
                        MessageBox.Show("Saved Failure");
                        form.DialogResult = DialogResult.No;
                    }
                    
                }
            });
            this.btnCancel.Click+=new EventHandler(delegate(object sender,EventArgs args){
                form.Close();
            });
            this.tbUserName.Leave += new EventHandler(delegate(object sender, EventArgs args)
            {
                if (Common.TextBoxChecked((TextBox)sender))
                {
                    //判断用户名是否使用过
                    UserInfo user = processor.QueryOne<UserInfo>("select * from userinfo where username=@username", delegate() { 
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic.Add("username",tbUserName.Text);
                        return dic;
                    });
                    if (user.Userid == 0)
                        lbAlarmUn.Text = "√";
                    else
                    {
                        this.lbAlarmUn.Text = "x " + tbUserName.Text + " has been used";
                        this.tbUserName.Focus();
                    }
                }
                else
                {
                    this.tbUserName.Focus();
                    this.lbAlarmUn.Text = "x none content";                    
                }
            });
            this.tbFullName.Leave += new EventHandler(delegate(object sender, EventArgs args)
            {
                if (Common.TextBoxChecked((TextBox)sender))
                {
                    this.lbAlarmFN.Text = "√";
                }
                else
                {
                    //this.tbFullName.Focus();
                    this.lbAlarmFN.Text = "x none content";
                }
            });
            this.tbDescription.Leave += new EventHandler(delegate(object sender, EventArgs args)
            {
                if (Common.TextBoxChecked(tbDescription))
                {
                    this.lbAlarmDesc.Text = "√";
                }
                else
                {
                    //this.tbDescription.Focus();
                    this.lbAlarmDesc.Text = "x none content";
                }
            });
            this.tbPwd.Leave += new EventHandler(delegate(object sender, EventArgs args)
            {
                if (Common.TextBoxChecked(tbPwd))
                {   
                    //判断密钥长度
                    if (Common.Policy == null)
                        this.lbAlarmPwd.Text = "√";
                    else
                    {
                        if (tbPwd.Text.TrimEnd().Length >= Common.Policy.MinPwdSize)
                            this.lbAlarmPwd.Text = "√";
                        else
                        {
                            this.lbAlarmPwd.Text = "x min pwd size is "+Common.Policy.MinPwdSize.ToString();
                            //this.tbPwd.Focus();
                        }
                    }
                }
                else if (!Common.TextBoxChecked(tbPwd))
                {
                    //this.tbPwd.Focus();
                    this.lbAlarmPwd.Text = "x none content";
                }
                
                if(Common.PasswordConfirmed(tbPwd,tbConfirm))
                    this.lbAlarmConfirm.Text = "√";
            });
            this.tbConfirm.Leave += new EventHandler(delegate(object sender, EventArgs args)
            {
                if (Common.TextBoxChecked(tbConfirm))
                {
                    if (Common.PasswordConfirmed(tbPwd, tbConfirm))
                        this.lbAlarmConfirm.Text = "√";
                    else
                    {
                        this.lbAlarmConfirm.Text = "x confirmed failure";
                    }
                }
                else
                {
                    //this.tbConfirm.Focus();
                    this.lbAlarmConfirm.Text = "x none content";
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
                role.Add(new RoleInfo() { ID=1,Rolename="Administrators",Remark=DateTime.Now.ToString() });
                role.Add(new RoleInfo() { ID = 2, Rolename = "Users", Remark = DateTime.Now.ToString() });
                processor.Insert<RoleInfo>(role);
            }
            this.cbxRole.DataSource = role;
            this.cbxRole.DisplayMember = "Rolename";
            this.cbxRole.ValueMember = "ID";
            userid = this.GetCurrentUserId();
            if (username!=null&&username != string.Empty)
            {
                user = processor.QueryOne<UserInfo>("SELECT * FROM USERINFO WHERE username=@username", delegate()
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("username", username);
                    return dic;
                });
                this.tbUserName.Text = user.UserName;
                this.tbFullName.Text = user.FullName;
                this.tbPwd.Text = user.Pwd;
                this.tbDescription.Text = user.Description;
                this.tbConfirm.Text = user.Pwd;
                this.cbChangePwd.Checked = Convert.ToBoolean(user.ChangePwd);
                this.cbLocked.Checked = Convert.ToBoolean(user.Locked);
                Common.SetControlEnable(this.cbLocked, true);
                Common.SetControlEnable(this.tbUserName, false);
            }
            else
            {
                Common.SetControlEnable(this.cbLocked, false);
                Common.SetControlEnable(this.tbUserName, true);
            }
        }
    }
}
