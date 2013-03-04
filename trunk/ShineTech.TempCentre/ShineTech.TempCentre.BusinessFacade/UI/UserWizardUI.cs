using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.DAL;

namespace ShineTech.TempCentre.BusinessFacade
{
    public class UserWizardUI
    {
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Panel pnShow;

        private Form form;
        private Dictionary<WizardName, UserControl> dic;
        private Step STEP = Step.first;
        private UserInfo user;
        private Policy policy=Common.Policy;
        private Meanings mEntity;
        //private UserMeanRelation rel;
        private bool flag;//是否第一次创建；
        private UserInfoBLL _userbll;
        private event EventHandler UserEvent;//user next事件
        private event EventHandler PolicyEvent;//policy next事件
        private event EventHandler RightEvent;//right next事件
        private event EventHandler MeanEvent;//mean 保存事件
        public UserWizardUI(Form form)
        {
            this.ConstructForms(form);
            this.Init();
            this.InitEvent();
        }
        public UserWizardUI(Form form,bool flag):this(form)
        {
            this.flag = flag;
        }
        private void ConstructForms(Form form)
        {
            this.form = form;
            btnBack = form.Controls.Find("btnBack", true)[0] as Button;
            btnCancel = form.Controls.Find("btnCancel", true)[0] as Button;
            btnNext = form.Controls.Find("btnNext", true)[0] as Button;
            btnSave = form.Controls.Find("btnSave", true)[0] as Button;
            pnShow = form.Controls.Find("pnShow", true)[0] as Panel;
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        private void Init()
        {
            /*用户向导pannel维护列表*/
            dic = new Dictionary<WizardName, UserControl>();
            dic.Add(WizardName.CreateUser, new UserCreate() {  FirstCreate=flag});
            dic.Add(WizardName.CreatePolicy, new UserPolicy());
            dic.Add(WizardName.CreateRight, new UserRight());
            dic.Add(WizardName.CreateMean, new UserMeaning());
            /*显示第一款面板*/
            this.pnShow.Controls.Clear();
            this.pnShow.Controls.Add(dic[WizardName.CreateUser]);
            this.UserEvent += new EventHandler(delegate(object sender, EventArgs args) { ((UserCreate)dic[WizardName.CreateUser]).SetValue(); });
            this.PolicyEvent += new EventHandler(delegate(object sender, EventArgs args) { ((UserPolicy)dic[WizardName.CreatePolicy]).SetValue(); });
            this.MeanEvent += new EventHandler(delegate(object sender, EventArgs args) { ((UserMeaning)dic[WizardName.CreateMean]).SetValue(); });
        }
        private void InitEvent()
        {
            this.btnCancel.Click += new EventHandler(delegate(object sender, EventArgs args) 
                { 
                    form.Close(); 
                });
            #region 订阅next事件
            this.btnNext.Click += new EventHandler(delegate(object sender, EventArgs args)
                {
                    try
                    {
                        this.Next();
                    }
                    catch { }
                });
            #endregion
            #region 订阅Back事件
            this.btnBack.Click += new EventHandler(delegate(object sender, EventArgs args) { this.Back(); });
            #endregion
            #region 订阅Save事件
            this.btnSave.Click += new EventHandler(delegate(object sender, EventArgs args) { this.Save(); });
            #endregion
        }
        /// <summary>
        /// 前进
        /// </summary>
        private void Next()
        {
            switch (STEP)
            {
                case Step.first:
                    /*显示第二步policy*/
                    UserEvent(form, new EventArgs());
                    if (this.SetUserInfo())
                    {
                        this.pnShow.Controls.Clear();//先清空显示panel中的各种控件
                        this.pnShow.Controls.Add(dic[WizardName.CreatePolicy]);
                        STEP = Step.second;
                    }
                    else
                        return;
                    break;
                case Step.second:
                    PolicyEvent(form, new EventArgs());
                    if (this.SetUserPolicy())
                    {
                        this.pnShow.Controls.Clear();//先清空显示panel中的各种控件
                        this.pnShow.Controls.Add(dic[WizardName.CreateRight]);
                        STEP = Step.third;
                    }
                    else
                        return;
                    break;
                case Step.third:
                    this.pnShow.Controls.Clear();//先清空显示panel中的各种控件
                    this.pnShow.Controls.Add(dic[WizardName.CreateMean]);
                    STEP = Step.fourth;
                    break;
            }

            this.WizardStateControl(STEP);
        }
        /// <summary>
        /// 后退
        /// </summary>
        private void Back()
        {
            this.pnShow.Controls.Clear();//先清空显示panel中的各种控件
            
            switch (STEP)
            {
                case Step.second:
                    this.pnShow.Controls.Add(dic[WizardName.CreateUser]);
                    STEP = Step.first;
                    break;
                case Step.third:
                    this.pnShow.Controls.Add(dic[WizardName.CreatePolicy]);
                    STEP = Step.second;
                    break;
                case Step.fourth:
                    this.pnShow.Controls.Add(dic[WizardName.CreateRight]);
                    STEP = Step.third;
                    break;
            }
            this.WizardStateControl(STEP);
        }
        private void Save()
        {
            MeanEvent(form, new EventArgs());//先获取meaning的设置
            if (_userbll == null)
                _userbll = new UserInfoBLL();
            UserMeaning um = dic[WizardName.CreateMean] as UserMeaning;
            if (_userbll.InsertUserWizard(user, um.mEntity))
            {
                form.DialogResult = DialogResult.OK;
                //insert sys log
                new OperationLogBLL().InsertLog(() =>
                {
                    Dictionary<string, object> d = new Dictionary<string, object>();
                    d.Add("OperateTime", DateTime.Now);
                    d.Add("Action", "Create user account");
                    d.Add("UserName", user.UserName);
                    d.Add("FullName","");
                    d.Add("Detail", user.UserName);
                    d.Add("LogType", 0);
                    return d;
                });
            }
            else
                form.DialogResult = DialogResult.No;
        }
        /// <summary>
        /// 按键控制
        /// </summary>
        /// <param name="state"></param>
        private void WizardStateControl(Step state)
        {
            switch (state)
            {
                case Step.first:
                    Common.SetControlEnable(this.btnNext, true);
                    Common.SetControlEnable(this.btnBack, false);
                    this.btnSave.Visible = false;
                    break;
                case Step.second:
                    Common.SetControlEnable(this.btnNext, true);
                    Common.SetControlEnable(this.btnBack, true);
                    this.btnSave.Visible = false;
                    break;
                case Step.third:
                    Common.SetControlEnable(this.btnNext, true);
                    Common.SetControlEnable(this.btnBack, true);
                    this.btnSave.Visible = false;
                    break;
                case Step.fourth:
                    Common.SetControlEnable(this.btnNext, false);
                    Common.SetControlEnable(this.btnBack, true);
                    this.btnSave.Visible = true;
                    break;
            }
        }
        /// <summary>
        /// 点击next时设置userinfo属性
        /// </summary>
        private bool SetUserInfo()
        {

            UserCreate createUser = dic[WizardName.CreateUser] as UserCreate;
            if (!string.IsNullOrEmpty(createUser.UserName) && !string.IsNullOrEmpty(createUser.FullName)
                && !string.IsNullOrEmpty(createUser.Role) && !string.IsNullOrEmpty(createUser.Password))
            {
                /*密钥长度*/
                if (policy == null || policy.MinPwdSize > createUser.Password.Length)
                    return false;
                int userid = new UserInfoBLL().GetCurrentUserID();
                if (user == null)
                    user = new UserInfo();
                user.Userid = userid + 1;
                user.UserName = createUser.UserName;
                user.Account = createUser.UserName;
                user.FullName = createUser.FullName;
                user.Description = createUser.Role;
                user.Pwd = createUser.Password;
                user.ChangePwd = 1;
                user.Locked = 0;
                user.RoleId = createUser.Group;
                user.LastPwdChangedTime = DateTime.Now;
                user.Remark = DateTime.Now.ToString();
                return true;
            }
            return false;
        }
        private bool SetUserPolicy()
        {
            UserPolicy createPolicy = dic[WizardName.CreatePolicy] as UserPolicy;
            if (createPolicy.InactivityTime == 0 || createPolicy.LockedTimes == 0 || createPolicy.MinPwdSize == 0
               || createPolicy.PwdExpiredDay == 0)
                return false;
            policy.InactivityTime = createPolicy.InactivityTime;
            policy.LockedTimes = createPolicy.LockedTimes;
            policy.PwdExpiredDay = createPolicy.PwdExpiredDay;
            policy.MinPwdSize = createPolicy.MinPwdSize;
            policy.ProfileFolder = "";
            policy.Remark = DateTime.Now.ToString();
            return true;
        }
    }
    enum WizardName { CreateUser,CreatePolicy,CreateRight,CreateMean }//panelname
    enum Step { first,second,third,fourth }//step by step
}
