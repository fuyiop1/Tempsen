using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.DAL;
using ShineTech.TempCentre.Platform;

namespace ShineTech.TempCentre.BusinessFacade
{
    public partial class UserCreate : UserControl
    {
        private IDataProcessor processor = new DeviceProcessor();
        private UserWizardUI parentForm;
        private ToolTip wrongTip = new System.Windows.Forms.ToolTip(new System.ComponentModel.Container());
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
        public UserCreate(UserWizardUI parentForm)
        {
            this.initToolTipSetter();
            this.parentForm = parentForm;
            InitializeComponent();
            this.InitEvent();
        }
        #region/*自定义属性*/
        private bool _FirstCreate=true;

        /// <summary>
        /// 是否第一次创建用户
        /// </summary>
        public bool FirstCreate
        {
            get { return _FirstCreate; }
            set { _FirstCreate = value; }
        }
        private string _UserName;

        //登录账号
        public string UserName
        {
            get { return _UserName; }
        }
        private string _FullName;

        public string FullName
        {
            get { return _FullName; }

        }
        private string _Password;

        public string Password
        {
            get { return _Password; }

        }
        private string _confirmPwd;

        public string ConfirmPwd
        {
            get { return _confirmPwd; }
        }
        private string _Role;//desc

        public string Role
        {
            get { return _Role; }
            
        }
        private int _Group;

        /// <summary>
        /// 用户组权限
        /// </summary>
        public int Group
        {
            get { return _Group; }
            
        }
        #endregion
        #region events
        private void InitEvent()
        {
            this.Load += new EventHandler(delegate(object sender, EventArgs args) { 
                this.tbUserName.Focus();
                if (FirstCreate)
                {
                    this.parentForm.FormTitleText = InputBoxTitle.WizardCreateAdmin;
                }
                this.rbUser.Enabled = !FirstCreate;
                this.rbAdmin.Checked = FirstCreate;
                this.rbUser.Checked = !FirstCreate;
            });
            #region username
            this.tbUserName.Leave += new EventHandler(delegate(object sender, EventArgs args)
            {
                ((TextBox)sender).Text = ((TextBox)sender).Text.Trim();
                if (Common.TextBoxChecked((TextBox)sender))
                {
                    //判断用户名是否使用过
                    UserInfo user = processor.QueryOne<UserInfo>("select * from userinfo where username=@username COLLATE NOCASE", delegate()
                    {
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic.Add("username", tbUserName.Text.Trim().ToLower());
                        return dic;
                    });
                    if (user.Userid == 0)
                    {
                        Common.ClearToolTip(this.wrongTip, this.pbUserNameTip);
                    }
                    else
                    {
                        Common.SetToolTip(this.wrongTip, this.pbUserNameTip, Messages.UserNameOccupied);
                    }
                }
                else
                {
                    Common.SetToolTip(this.wrongTip, this.pbUserNameTip, Messages.EmptyContentError);
                }
            });
            #endregion
            #region fullname
            this.tbFullName.Leave += new EventHandler(delegate(object sender, EventArgs args)
            {
                if (Common.TextBoxChecked((TextBox)sender))
                {
                    Common.ClearToolTip(this.wrongTip, this.pbFullNameTip);
                }
                else
                {
                    Common.SetToolTip(this.wrongTip, this.pbFullNameTip, Messages.EmptyContentError);
                }
            });
            #endregion
            #region desc
            this.tbDescription.Leave += new EventHandler(delegate(object sender, EventArgs args)
            {
                if (Common.TextBoxChecked(tbDescription))
                {
                    Common.ClearToolTip(this.wrongTip, this.pbRoleTip);
                }
                else
                {
                    Common.SetToolTip(this.wrongTip, this.pbRoleTip, Messages.EmptyContentError);
                }
            });
            #endregion
            #region pwd
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
                            Common.SetToolTip(this.wrongTip, this.pbPasswordTip, string.Format(Messages.PasswordShortThanDefined, Common.Policy.MinPwdSize));
                        }
                    }
                }
                else
                {
                    Common.SetToolTip(this.wrongTip, this.pbPasswordTip, Messages.EmptyContentError);
                }

            });
            #endregion
            #region confirm
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
                        Common.SetToolTip(this.wrongTip, this.pbConfirmPasswordTip, Messages.MismatchPassword);
                    }
                }
                else
                {
                    Common.SetToolTip(this.wrongTip, this.pbConfirmPasswordTip, Messages.EmptyContentError);
                }
            });
            #endregion

            this.initGroupRadioCheckEvent();
            //非法字符处理事件
            this.tbUserName.TextChanged += new EventHandler((sender, e) => {
                Utils.IsInputTextValid(this.tbUserName);
            });
            this.tbFullName.TextChanged += new EventHandler((sender, e) =>
            {
                Utils.IsInputTextValid(this.tbFullName);
            });
        }



        private void initGroupRadioCheckEvent()
        {
            this.rbAdmin.CheckedChanged += new EventHandler((sender, e) => {
                var rb = sender as RadioButton;
                if (rb != null && rb.Checked)
                {
                    this.parentForm.FormTitleText = InputBoxTitle.WizardCreateAdmin;
                }
            });

            this.rbUser.CheckedChanged += new EventHandler((sender, e) =>
            {
                var rb = sender as RadioButton;
                if (rb != null && rb.Checked)
                {
                    this.parentForm.FormTitleText = InputBoxTitle.WizardCreateUser;
                }
            });
        }

        #endregion
        public void SetValue()
        {
            _UserName = this.tbUserName.Text == string.Empty ? "" : this.tbUserName.Text;
            _FullName = this.tbFullName.Text; 
            _Password = this.tbPwd.Text;
            _confirmPwd = this.tbConfirm.Text;
            _Role = this.tbDescription.Text; 
            _Group =this.rbAdmin.Checked==true ? 1 : 2;
        }
        public void SetGroupEnable(bool flag)
        {
            this.rbUser.Enabled = flag;
        }

         public void SetFieldErrorToolTip(UserInfoFields field, string message)
        {
            switch (field)
            {
                case UserInfoFields.UserName:
                    this.pbUserNameTip.Visible = true;
                    this.wrongTip.SetToolTip(this.pbUserNameTip, message);
                    break;
                case UserInfoFields.FullName:
                    this.pbFullNameTip.Visible = true;
                    this.wrongTip.SetToolTip(this.pbFullNameTip, message);
                    break;
                case UserInfoFields.Password:
                    this.pbPasswordTip.Visible = true;
                    this.wrongTip.SetToolTip(this.pbPasswordTip, message);
                    break;
                case UserInfoFields.ConfirmPassword:
                    this.pbConfirmPasswordTip.Visible = true;
                    this.wrongTip.SetToolTip(this.pbConfirmPasswordTip, message);
                    break;
                case UserInfoFields.Role:
                    this.pbRoleTip.Visible = true;
                    this.wrongTip.SetToolTip(this.pbRoleTip, message);
                    break;
                default:
                    break;
            }
        }

    }

    public enum UserInfoFields { UserName, FullName, Password, ConfirmPassword, Role }
}
