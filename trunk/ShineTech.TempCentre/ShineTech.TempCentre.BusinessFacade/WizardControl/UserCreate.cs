using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.DAL;

namespace ShineTech.TempCentre.BusinessFacade
{
    public partial class UserCreate : UserControl
    {
        private IDataProcessor processor = new DeviceProcessor();
        public UserCreate()
        {
            InitializeComponent();
            this.InitEvent();
            this.rbUser.Enabled = FirstCreate;
            
        }
        #region/*自定义属性*/
        private bool _FirstCreate=false;

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
            this.Load += new EventHandler(delegate(object sender, EventArgs args) { this.tbUserName.Focus(); });
            #region username
            this.tbUserName.Leave += new EventHandler(delegate(object sender, EventArgs args)
            {
                if (Common.TextBoxChecked((TextBox)sender))
                {
                    //判断用户名是否使用过
                    UserInfo user = processor.QueryOne<UserInfo>("select * from userinfo where username=@username", delegate()
                    {
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic.Add("username", tbUserName.Text);
                        return dic;
                    });
                    if (user.Userid == 0)
                        lbAlarmUn.Text = "√";
                    else
                    {
                        this.lbAlarmUn.Text = "x " + tbUserName.Text + " has been used";
                        this.tbUserName.Text = "";
                        //this.tbUserName.Focus();
                    }
                }
                else
                {
                    //this.tbUserName.Focus();
                    this.lbAlarmUn.Text = "x none content";
                }
            });
            #endregion
            #region fullname
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
            #endregion
            #region desc
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
            #endregion
            #region pwd
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
                            this.lbAlarmPwd.Text = "x min pwd size is " + Common.Policy.MinPwdSize.ToString();
                            //this.tbPwd.Focus();
                        }
                    }
                }
                else if (!Common.TextBoxChecked(tbPwd))
                {
                    //this.tbPwd.Focus();
                    this.lbAlarmPwd.Text = "x none content";
                }

                if (Common.PasswordConfirmed(tbPwd, tbConfirm))
                    this.lbAlarmConfirm.Text = "√";
            });
            #endregion
            #region confirm
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
            #endregion
        }
        public void SetFocus()
        {
            this.tbUserName.Focus();
        }
        #endregion
        public void SetValue()
        {
            _UserName = this.tbUserName.Text == string.Empty ? "" : this.tbUserName.Text;
            _FullName = this.tbFullName.Text; 
            _Password = this.tbPwd.Text; 
            _Role = this.tbDescription.Text; 
            _Group =this.rbAdmin.Checked==true ? 1 : 2;
        }
    }
}
