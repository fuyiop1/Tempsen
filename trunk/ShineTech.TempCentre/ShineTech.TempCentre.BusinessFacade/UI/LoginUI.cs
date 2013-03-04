using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.DAL;
using System.Xml.Serialization;
using System.ComponentModel;
using System.IO;
namespace ShineTech.TempCentre.BusinessFacade
{
    public class LoginUI
    {
        private System.Windows.Forms.TextBox tbAccount;
        private System.Windows.Forms.TextBox tbPwd;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lbAccount;
        private System.Windows.Forms.Label lbPwd;

        private Form form;
        private IDataProcessor processor;
        private int loginTimes=1;
        private System.ComponentModel.BackgroundWorker bw;
        private AutoCompleteStringCollection ac;
        private readonly string UserXMLPath = "users.xml";
        private OperationLogBLL logBll=new OperationLogBLL ();
        /// <summary>
        /// 登录次数
        /// </summary>
        public int LoginTimes
        {
            get { return loginTimes; }
            set { loginTimes = value; }
        }
        public LoginUI(Form form)
        {
            this.form = form;
            this.ConstructForms(form);
            this.Init();
            this.InitEvent();
            this.Intelligence();
        }
        private void ConstructForms(Form form)
        {
            tbAccount = form.Controls.Find("tbAccount", true)[0] as TextBox;
            tbPwd = form.Controls.Find("tbPwd", true)[0] as TextBox;
            btnLogin = form.Controls.Find("btnLogin", true)[0] as Button;
            lbAccount = form.Controls.Find("lbAccount", true)[0] as Label;
            lbPwd = form.Controls.Find("lbPwd", true)[0] as Label;
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
            catch(Exception exc){}
        }
        /// <summary>
        /// 初始化事件
        /// </summary>
        private void InitEvent()
        {
            this.tbAccount.Leave += new EventHandler(delegate(object sender, EventArgs args)
            {
                if (Common.TextBoxChecked(tbAccount))
                    this.lbAccount.Text = "√";
                else
                {
                    this.lbAccount.Text = "x none content";
                    //this.tbAccount.Focus();
                }
            });
            this.tbPwd.Leave += new EventHandler(delegate(object sender,EventArgs args)
            {
                if (Common.TextBoxChecked(tbPwd))
                    this.lbPwd.Text = "√";
                else
                {
                    this.lbPwd.Text = "x none content";
                    //this.tbPwd.Focus();
                }
            });
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

                    UserInfo user = processor.QueryOne<UserInfo>("SELECT * FROM UserInfo WHERE username=@username", delegate()
                    {
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic.Add("username", tbAccount.Text.TrimEnd());
                        //dic.Add("pwd", tbPwd.Text.TrimEnd());
                        return dic;
                    });
                    if (user.Locked == 0)
                    {
                        if (LoginTimes <= Common.Policy.LockedTimes)
                        {
                            if (user.Userid != 0 && user.Pwd.Equals(this.tbPwd.Text))
                            {
                                Common.User = user;
                                LoginTimes = 0;
                                this.SaveUserList();//保存列表
                                //记录成功的日志
                                logBll.InsertLog(() => 
                                {
                                    Dictionary<string, object> dic = new Dictionary<string, object>();
                                    dic.Add("OperateTime",DateTime.Now);
                                    dic.Add("Action","Log on");
                                    dic.Add("UserName", user.UserName);
                                    dic.Add("FullName", user.FullName);
                                    dic.Add("Detail", "Success");
                                    dic.Add("LogType", 0);
                                    return dic;
                                });
                                return true;
                            }
                            else if (user.Userid == 0)
                            {
                                this.lbAccount.Text = "× user does not exist.";
                                //this.lbAccount.ForeColor = System.Drawing.Color.Red;
                                return false;
                            }
                            else
                            {
                                this.lbPwd.Text = "× password invalid.";
                                //this.lbPwd.ForeColor = System.Drawing.Color.Red;
                                LoginTimes++;
                                //记录日志
                                logBll.InsertLog(() =>
                                {
                                    Dictionary<string, object> dic = new Dictionary<string, object>();
                                    dic.Add("OperateTime", DateTime.Now);
                                    dic.Add("Action", "Log on");
                                    dic.Add("UserName", user.UserName);
                                    dic.Add("FullName", user.FullName);
                                    dic.Add("Detail", "Failure");
                                    dic.Add("LogType", 0);
                                    return dic;
                                });
                                return false;
                            }
                        }
                        else
                        {
                            Dictionary<string, object> dic = new Dictionary<string, object>();
                            dic.Add("locked", 1);
                            dic.Add("username", this.tbAccount.Text.TrimEnd());
                            processor.ExecuteNonQuery("UPDATE userinfo set locked=@locked where username=@username", dic);
                            this.lbPwd.Text = "× over " + Common.Policy.LockedTimes.ToString() + " times";
                        }
                    }
                    else
                    {
                        this.lbPwd.Text = "× account's locked.";
                        //记录账号锁定日志
                        logBll.InsertLog(() =>
                        {
                            Dictionary<string, object> dic = new Dictionary<string, object>();
                            dic.Add("OperateTime", DateTime.Now);
                            dic.Add("Action", "Log on");
                            dic.Add("UserName", user.UserName);
                            dic.Add("FullName", user.FullName);
                            dic.Add("Detail", "Failure");
                            dic.Add("LogType", 0);
                            return dic;
                        });
                    }
                }
                return false;
            }
            catch (Exception exc) { return false; }
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
        //private
    }

    ///定义一个users列表可供智能提示用
    public class Users
    {
        //
        [XmlElement]
        public List<string> UserName;
    }
}
