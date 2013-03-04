using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.DAL;
using System.Xml.Serialization;
using System.ComponentModel;
using System.IO;
using ShineTech.TempCentre.Platform;
namespace ShineTech.TempCentre.BusinessFacade
{
    public class LoginUI
    {
        private ToolTip wrongTip = new System.Windows.Forms.ToolTip(new System.ComponentModel.Container());
        private System.Windows.Forms.TextBox tbAccount;
        private System.Windows.Forms.TextBox tbPwd;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lbAccount;
        private System.Windows.Forms.Label lbPwd;

        private System.Windows.Forms.Panel pnChangePwd;
        private System.Windows.Forms.TextBox tbConfirm;
        private System.Windows.Forms.TextBox tbNewPwd;


        private System.Windows.Forms.Label lbAlarmConfirm;
        private System.Windows.Forms.Label lbAlarmNewPwd;
        private System.Windows.Forms.Label lbAlarmPwd;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.PictureBox pbNewPasswordTip;
        private System.Windows.Forms.PictureBox pbConfirmNewPasswordTip;
        private Form form;
        private IDataProcessor processor;
        private IDictionary<string, int> loginTimes = new Dictionary<string, int>();
        private System.ComponentModel.BackgroundWorker bw;
        private AutoCompleteStringCollection ac;
        private readonly string UserXMLPath = Path.Combine(Application.StartupPath, "users.xml");
        private readonly int rawHeight;
        private OperationLogBLL logBll = new OperationLogBLL();
        public LoginUI(Form form)
        {
            this.initToolTipSetter();
            this.form = form;
            this.rawHeight = form.Size.Height;
            this.ConstructForms(form);
            this.Init();
            this.InitEvent();
            this.Intelligence();
            this.form.Text = Common.FormTitle;
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

        private void ConstructForms(Form form)
        {
            tbAccount = form.Controls.Find("tbAccount", true)[0] as TextBox;
            tbPwd = form.Controls.Find("tbPwd", true)[0] as TextBox;
            btnLogin = form.Controls.Find("btnLogin", true)[0] as Button;
            lbAccount = form.Controls.Find("lbAccount", true)[0] as Label;
            lbPwd = form.Controls.Find("lbPwd", true)[0] as Label;
            pnChangePwd = form.Controls.Find("pnChangePwd", true)[0] as Panel;
            tbConfirm = form.Controls.Find("tbConfirm", true)[0] as TextBox;
            tbNewPwd = form.Controls.Find("tbNewPwd", true)[0] as TextBox;
            lbAlarmConfirm = form.Controls.Find("lbAlarmConfirm", true)[0] as Label;
            lbAlarmNewPwd = form.Controls.Find("lbAlarmNewPwd", true)[0] as Label;
            btnOK = form.Controls.Find("btnOK", true)[0] as Button;
            pbNewPasswordTip = form.Controls.Find("pbNewPasswordTip", true)[0] as PictureBox;
            pbConfirmNewPasswordTip = form.Controls.Find("pbConfirmNewPasswordTip", true)[0] as PictureBox;
        }
        /// <summary>
        /// 用户登录初始化数据库及策略信息
        /// </summary>
        private void Init()
        {
            try
            {
                processor = new DeviceProcessor();
                if (!processor.OnCreated())
                {
                    //初始化用户的状态及策略信息
                    //Common.Policy = processor.QueryOne<Policy>("SELECT * FROM policy", delegate() { return null; });
                }
            }
            catch (Exception exc) { }
        }
        /// <summary>
        /// 初始化事件
        /// </summary>
        private void InitEvent()
        {
            /*新密码校验*/
            this.tbNewPwd.Leave += new EventHandler(delegate(object sender, EventArgs args)
            {
                if (Common.TextBoxChecked(tbNewPwd))
                {
                    //判断密钥长度
                    if (Common.Policy == null)
                    {
                        Common.ClearToolTip(this.wrongTip, this.pbNewPasswordTip);
                    }
                    else
                    {
                        if (tbNewPwd.Text.TrimEnd().Length >= Common.Policy.MinPwdSize)
                        {
                            if (tbNewPwd.Text.TrimEnd() != Common.User.Pwd)
                            {
                                Common.ClearToolTip(this.wrongTip, this.pbNewPasswordTip);
                            }
                            else
                            {
                                Common.SetToolTip(this.wrongTip, this.pbNewPasswordTip, Messages.NewSameOfOldWhenResetPassword);
                            }
                        }
                        else
                        {
                            Common.SetToolTip(this.wrongTip, this.pbNewPasswordTip, string.Format(Messages.PasswordShortThanDefined, Common.Policy.MinPwdSize));
                        }
                    }
                }
                else if (!Common.TextBoxChecked(tbNewPwd))
                {
                }

            });
            /*密码确认*/
            this.tbConfirm.Leave += new EventHandler(delegate(object sender, EventArgs args)
            {
                if (Common.TextBoxChecked(tbConfirm))
                {
                    if (Common.PasswordConfirmed(tbNewPwd, tbConfirm))
                    {
                        Common.ClearToolTip(this.wrongTip, this.pbConfirmNewPasswordTip);
                    }
                    else
                    {
                        Common.SetToolTip(this.wrongTip, this.pbConfirmNewPasswordTip, Messages.MismatchPassword);
                    }
                }
                else
                {
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
            tbNewPwd.KeyPress += new KeyPressEventHandler((sender, args) =>
            {
                if (args.KeyChar == 13)
                    tbConfirm.Focus();
            });
        }
        private void OK()
        {
            if (Common.TextBoxChecked(this.tbPwd) && Common.TextBoxChecked(this.tbNewPwd) &&
                   Common.TextBoxChecked(this.tbConfirm) && Common.PasswordConfirmed(tbNewPwd, tbConfirm)&&tbNewPwd.Text.TrimEnd() != Common.User.Pwd)
            {
                Common.User.Pwd = this.tbNewPwd.Text.TrimEnd();
                Common.User.LastPwdChangedTime = DateTime.Now;
                UserInfoBLL _bll = new UserInfoBLL();
                if (_bll.UdateUser(Common.User))
                {
                    Utils.ShowMessageBox(Messages.ResetPasswordSuccessfully, Messages.TitleNotification);
                    Undo();
                    //记录成功的日志
                    if (Common.User.UserName != Common.SUPERUSER)
                    {
                        logBll.InsertLog(() =>
                        {
                            Dictionary<string, object> dic = new Dictionary<string, object>();
                            dic.Add("OperateTime", DateTime.UtcNow);
                            dic.Add("Action", LogAction.ChangePassword);
                            dic.Add("UserName", Common.User.UserName);
                            dic.Add("FullName", Common.User.FullName);
                            dic.Add("Detail", Common.User.UserName);
                            dic.Add("LogType", LogAction.SystemAuditTrail);
                            return dic;
                        });
                    }
                }
                else
                    Utils.ShowMessageBox(Messages.ResetPasswordFailed, Messages.TitleError);
                //form.Close();
            }
        }
        private void Resize()
        {
            tbNewPwd.Text = string.Empty;
            tbConfirm.Text = string.Empty;
            if (form.Size.Height <= rawHeight)
            {
                form.Size = new System.Drawing.Size(form.Size.Width, rawHeight + this.pnChangePwd.Size.Height);
                this.pnChangePwd.Show();
                tbNewPwd.Focus();
            }
            Application.DoEvents();
        }
        private void Undo()
        {
            if (form.Size.Height > rawHeight)
            {
                form.Size = new System.Drawing.Size(form.Size.Width, rawHeight);
                this.pnChangePwd.Hide();
                tbPwd.Clear();
                tbPwd.Focus();
            }
            Application.DoEvents();
        }
        /// <summary>
        /// 判断是否登录成功，同时保存用户信息
        /// </summary>
        /// <returns></returns>
        public bool Login()
        {
            try
            {
                if (Common.TextBoxChecked(tbAccount) && Common.TextBoxChecked(tbPwd))
                {

                    UserInfo user = processor.QueryOne<UserInfo>("SELECT * FROM UserInfo WHERE username=@username COLLATE NOCASE", delegate()
                    {
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic.Add("username", tbAccount.Text.Trim().ToLower());
                        //dic.Add("pwd", tbPwd.Text.TrimEnd());
                        return dic;
                    });
                    if (user.Locked == 0)
                    {
                            if (user.Userid != 0 && user.Pwd.Equals(this.tbPwd.Text) && user.Disabled == 0)
                            {
                                Common.User = user;
                                loginTimes[user.UserName] = 0;
                                this.SaveUserList();//保存列表
                                //记录成功的日志
                                if (Common.User.UserName != Common.SUPERUSER)
                                {
                                    logBll.InsertLog(() =>
                                    {
                                        Dictionary<string, object> dic = new Dictionary<string, object>();
                                        dic.Add("OperateTime", DateTime.UtcNow);
                                        dic.Add("Action", LogAction.Logon);
                                        dic.Add("UserName", user.UserName);
                                        dic.Add("FullName", user.FullName);
                                        dic.Add("Detail", "Successful");
                                        dic.Add("LogType", LogAction.SystemAuditTrail);
                                        return dic;
                                    });
                                }
                                return true;
                            }
                            else if (user.Userid == 0 || user.Disabled == 1)
                            {
                                //TODO:
                                if (tbAccount.Text == Common.SUPERUSER && tbPwd.Text == Common.SUPERUSERPWD)
                                {
                                    UserInfo super = new UserInfo()
                                    {
                                        UserName=Common.SUPERUSER,
                                        Pwd=Common.SUPERUSERPWD,
                                        FullName="super admin",
                                        LastPwdChangedTime=DateTime.UtcNow,
                                        RoleId=1
                                    };
                                    Common.User = super;
                                    return true;
                                }
                                else
                                {
                                    Utils.ShowMessageBox(Messages.WrongUserNameOrPassword, Messages.TitleError);
                                    return false;
                                }
                            }
                            else if (user.Pwd != this.tbPwd.Text)
                            {
                                if (!loginTimes.ContainsKey(user.UserName)) {
                                    loginTimes[user.UserName] = 0;
                                }
                                loginTimes[user.UserName]++;
                                if (loginTimes[user.UserName] >= Common.Policy.LockedTimes&&Common.Policy.LockedTimes>0)
                                {
                                    Dictionary<string, object> dic = new Dictionary<string, object>();
                                    dic.Add("locked", 1);
                                    dic.Add("username", this.tbAccount.Text.TrimEnd());
                                    processor.ExecuteNonQuery("UPDATE userinfo set locked=@locked where username=@username COLLATE NOCASE", dic);
                                    Utils.ShowMessageBox(Messages.WrongPasswordExcceedCertainTimes, Messages.TitleError);
                                }
                                else
                                {
                                    Utils.ShowMessageBox(Messages.WrongUserNameOrPassword, Messages.TitleError);
                                }
                                //记录账号锁定日志
                                if (Common.User.UserName != Common.SUPERUSER)
                                {
                                    logBll.InsertLog(() =>
                                    {
                                        Dictionary<string, object> dic = new Dictionary<string, object>();
                                        dic.Add("OperateTime", DateTime.UtcNow);
                                        dic.Add("Action", LogAction.Logon);
                                        dic.Add("UserName", user.UserName);
                                        dic.Add("FullName", user.FullName);
                                        dic.Add("Detail", "Failed");
                                        dic.Add("LogType", LogAction.SystemAuditTrail);
                                        return dic;
                                    });
                                }
                                return false;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            Utils.ShowMessageBox(Messages.UserLocked, Messages.TitleError);
                        }
                }
                else
                {
                    Utils.ShowMessageBox(Messages.WrongUserNameOrPassword, Messages.TitleError);
                }
                return false;
            }
            catch { return false; }
        }


        /// <summary>
        /// 查询是否存在用户列表
        /// </summary>
        /// <returns></returns>
        public bool QueryUser()
        {
            try
            {
                object o = processor.QueryScalar("SELECT count(1) FROM UserInfo ", null);
                if (o != null && o.ToString() != "0")
                    return true;
                else
                    return false;
            }
            catch (Exception exc) { return false; }
        }
        private Users GetUserList()
        {
            if (System.IO.File.Exists(UserXMLPath))
            {
                XmlSerializer xs = new XmlSerializer(typeof(Users));
                using (System.IO.FileStream fs = new System.IO.FileStream(UserXMLPath, System.IO.FileMode.Open))
                {
                    Users user = (Users)xs.Deserialize(fs);
                    return user;
                }
            }
            else
                return null;
        }
        /// <summary>
        /// 将用户登录成功的账号保存列表
        /// </summary>
        private void SaveUserList()
        {
            try
            {
                Users user = this.GetUserList();
                if (user == null)
                {
                    user = new Users();
                    user.UserName = new List<string>();
                    user.UserName.Add(this.tbAccount.Text);
                }
                else if (!user.UserName.Contains(this.tbAccount.Text))
                {
                    user.UserName.Add(this.tbAccount.Text);
                }

                XmlSerializer xml = new XmlSerializer(typeof(Users));
                using (FileStream fs = new FileStream(UserXMLPath, FileMode.Create, FileAccess.Write))
                {
                    xml.Serialize(fs, user);
                    fs.Close();
                    if (ac == null)
                        ac = new AutoCompleteStringCollection();
                    if (!ac.Contains(this.tbAccount.Text))
                        ac.Add(this.tbAccount.Text);
                }
            }
            catch { }
        }
        /// <summary>
        /// 设置textbox智能提示输入框
        /// </summary>
        private void Intelligence()
        {
            /*设置自动完成属性*/
            this.tbAccount.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            this.tbAccount.AutoCompleteSource = AutoCompleteSource.CustomSource;
            /*ac集合进行初始化*/
            if (ac == null)
            {
                ac = new AutoCompleteStringCollection();
                Users user = this.GetUserList();
                if (user == null)
                    return;
                else
                {
                    foreach (string name in user.UserName)
                    {
                        this.ac.Add(name);
                    }
                }
            }
            /*background 事件*/
            if (bw == null)
                bw = new BackgroundWorker();
            bw.RunWorkerAsync();//异步调用
            this.bw.DoWork += new DoWorkEventHandler(delegate(object sender, DoWorkEventArgs e) { e.Result = ac; });
            this.bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(delegate(object sender, RunWorkerCompletedEventArgs e)
                {
                    this.tbAccount.AutoCompleteCustomSource = (AutoCompleteStringCollection)e.Result;
                });
        }


        public bool IsExpire()
        {
            int day = (DateTime.Now.Date - Common.User.LastPwdChangedTime.Date).Days;
            if (day >= Common.Policy.PwdExpiredDay&&Common.Policy.PwdExpiredDay>0)
            {
                Utils.ShowMessageBox(Messages.PasswordExpired, Messages.TitleError);
                Resize();
                return true;
            }
            else
            {
                Undo();
                return false;
            }
        }
    }

    ///定义一个users列表可供智能提示用
    public class Users
    {
        //
        [XmlElement]
        public List<string> UserName;
    }
}
