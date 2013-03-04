using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.DAL;
using ShineTech.TempCentre.Platform;

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
        //private Meanings mEntity;
        //private UserMeanRelation rel;
        private readonly bool flag=false;//是否第一次创建；
        private UserInfoBLL _userbll;
        private event EventHandler UserEvent;//user next事件
        private event EventHandler PolicyEvent;//policy next事件
        private event EventHandler RightEvent;//right next事件
        private event EventHandler MeanEvent;//mean 保存事件
        public UserWizardUI(Form form)
        {
            this.ConstructForms(form);
        }
        public UserWizardUI(Form form,bool flag):this(form)
        {
            this.flag = flag;
            this.Init();
            this.InitEvent();
            this.form.Text = Common.FormTitle;
        }

        public string FormTitleText
        {
            get
            {
                if (this.form != null)
                {
                    return this.form.Text;
                }
                else
                {
                    return string.Empty;
                }
            }
            set
            {
                if (this.form != null)
                {
                    this.form.Text = value;
                }
            }
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
            dic.Add(WizardName.CreateUser, new UserCreate(this) { FirstCreate = this.flag });
            dic.Add(WizardName.CreatePolicy, new UserPolicy());
            dic.Add(WizardName.CreateRight, new UserRight());
            dic.Add(WizardName.CreateMean, new UserMeaning());
            /*显示第一款面板*/
            this.pnShow.Controls.Clear();
            this.pnShow.Controls.Add(dic[WizardName.CreateUser]);
            this.btnSave.Enabled = false;
            this.UserEvent += new EventHandler(delegate(object sender, EventArgs args) { ((UserCreate)dic[WizardName.CreateUser]).SetValue(); });
            this.PolicyEvent += new EventHandler(delegate(object sender, EventArgs args) { ((UserPolicy)dic[WizardName.CreatePolicy]).SetValue(); });
            this.MeanEvent += new EventHandler(delegate(object sender, EventArgs args) { ((UserMeaning)dic[WizardName.CreateMean]).SetValue(); });
            this.RightEvent += new EventHandler((a, b) => { ((UserRight)dic[WizardName.CreateRight]).SetValue(); });
            ((UserRight)dic[WizardName.CreateRight]).SignRightOnChange += new EventHandler(SetNextEnableOnThird);
            if (this._userbll ==  null)
            {
                this._userbll = new UserInfoBLL();
            }
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
                        if (!flag)//如果不是第一次创建
                        {
                            UserRight r = dic[WizardName.CreateRight] as UserRight;
                            bool isAdmin=(dic[WizardName.CreateUser] as UserCreate).Group == 1 ? true : false;
                            r.InitRight("", isAdmin);
                            if (isAdmin)
                            {
                                RightEvent(form, null);
                                this.pnShow.Controls.Add(dic[WizardName.CreateMean]);
                                STEP = Step.fourth;
                            }
                            else
                            {
                                this.pnShow.Controls.Add(dic[WizardName.CreateRight]);
                                STEP = Step.third;
                            }
                        }
                        else
                        {
                            this.pnShow.Controls.Add(dic[WizardName.CreatePolicy]);
                            STEP = Step.second;
                        }
                    }
                    else
                        return;
                    break;
                case Step.second:
                    PolicyEvent(form, new EventArgs());
                    if (this.SetUserPolicy())
                    {
                        UserRight r= dic[WizardName.CreateRight] as UserRight;
                        bool isAdmin = (dic[WizardName.CreateUser] as UserCreate).Group == 1 ? true : false;
                        r.InitRight("", isAdmin);
                        this.pnShow.Controls.Clear();//先清空显示panel中的各种控件
                        if (!isAdmin)
                        {
                            this.pnShow.Controls.Add(dic[WizardName.CreateRight]);
                            STEP = Step.third;
                        }
                        else
                        {
                            RightEvent(form, null);
                            this.pnShow.Controls.Add(dic[WizardName.CreateMean]);
                            STEP = Step.fourth;
                        }
                    }
                    else
                        return;
                    break;
                case Step.third:
                    RightEvent(form, null);
                    if ((dic[WizardName.CreateRight] as UserRight).Right.Contains(RightsText.SignRecords))
                    {
                        this.pnShow.Controls.Clear();//先清空显示panel中的各种控件
                        this.pnShow.Controls.Add(dic[WizardName.CreateMean]);
                        STEP = Step.fourth;
                    }
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
                    if (flag)
                    {
                        this.pnShow.Controls.Add(dic[WizardName.CreatePolicy]);
                        STEP = Step.second;
                    }
                    else
                    {
                        this.pnShow.Controls.Add(dic[WizardName.CreateUser]);
                        STEP = Step.first;
                    }
                    break;
                case Step.fourth:
                    bool isAdmin = (dic[WizardName.CreateUser] as UserCreate).Group == 1 ? true : false;
                    if (!isAdmin)
                    {
                        this.pnShow.Controls.Add(dic[WizardName.CreateRight]);
                        STEP = Step.third;
                    }
                    else
                    {
                        if (flag)
                        {
                            this.pnShow.Controls.Add(dic[WizardName.CreatePolicy]);
                            STEP = Step.second;
                        }
                        else
                        {
                            this.pnShow.Controls.Add(dic[WizardName.CreateUser]);
                            STEP = Step.first;
                        }
                    }
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
            if (_userbll.InsertUserWizard(user,policy,(dic[WizardName.CreateRight] as UserRight).Right, um.mEntity))
            {
                UserProfileBLL userProfileBll = new UserProfileBLL();
                UserProfile userProfile = new UserProfile();
                userProfile.ID = userProfileBll.GetProfilePKValue() + 1;
                userProfile.UserName = user.UserName;
                userProfile.TempCurveRGB = Common.GlobalProfile.TempCurveRGB;
                userProfile.AlarmLineRGB = Common.GlobalProfile.AlarmLineRGB;
                userProfile.IdealRangeRGB = Common.GlobalProfile.IdealRangeRGB;
                userProfile.IsShowAlarmLimit = Common.GlobalProfile.IsShowAlarmLimit;
                userProfile.IsShowMark = Common.GlobalProfile.IsShowMark;
                userProfile.IsFillIdealRange = Common.GlobalProfile.IsFillIdealRange;
                userProfile.DateTimeFormator = Common.GlobalProfile.DateTimeFormator;
                userProfile.Remark = DateTime.Now.ToString();

                userProfile.ContactInfo = "";
                userProfile.Logo = ShineTech.TempCentre.Platform.Utils.CopyToBinary(Properties.Resources.tempsen);
                userProfile.DefaultPath = "";
                userProfile.ReportTitle = "";

                userProfile.IsGlobal = (int)GlobalType.None;
                userProfile.IsShowHeader = false;
                userProfile.TempUnit = "C";
                userProfileBll.InsertProfile(userProfile);

                //insert sys log
                InsertCreateUserLog();
                InsertAssignRightsLog();
                // new implementation of meanings
                if (flag)
                {
                    DialogResult result = Utils.ShowMessageBox(Messages.FirstCreate,Messages.TitleNotification,MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                        form.DialogResult = DialogResult.OK;
                    else
                        form.DialogResult = DialogResult.No;
                }
                else
                {
                    form.DialogResult = DialogResult.OK;
                }
                
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
                    this.btnSave.Enabled = false;
                    this.btnBack.Focus();
                    this.form.AcceptButton = this.btnNext;
                    if (this.dic != null && this.dic[WizardName.CreateUser] != null)
                    {
                        var control = this.dic[WizardName.CreateUser] as UserCreate;
                        if (control != null)
                        {
                            if (control.Group == 1)
                            {
                                this.FormTitleText = InputBoxTitle.WizardCreateAdmin;
                            }
                            else
                            {
                                this.FormTitleText = InputBoxTitle.WizardCreateUser;
                            }
                        }
                    }
                    break;
                case Step.second:
                    Common.SetControlEnable(this.btnNext, true);
                    Common.SetControlEnable(this.btnBack, true);
                    this.btnSave.Enabled = false;
                    this.form.AcceptButton = this.btnNext;
                    this.btnBack.Focus();
                    //this.FormTitleText = InputBoxTitle.WizarPolicy;
                    break;
                case Step.third:
                    if ((dic[WizardName.CreateRight] as UserRight).Right.Contains(RightsText.SignRecords))
                    {
                        Common.SetControlEnable(this.btnNext, true);
                        Common.SetControlEnable(this.btnBack, true);
                        this.btnSave.Enabled = false;
                        this.form.AcceptButton = this.btnNext;
                        this.btnBack.Focus();
                    }
                    else
                    {
                        Common.SetControlEnable(this.btnNext, false);
                        Common.SetControlEnable(this.btnBack, true);
                        this.btnSave.Enabled = true;
                        this.btnSave.TabIndex = 0;
                        this.form.AcceptButton = this.btnSave;
                        this.btnSave.Focus();
                    }
                    //this.FormTitleText = InputBoxTitle.WizardRight;
                    break;
                case Step.fourth:
                    Common.SetControlEnable(this.btnNext, false);
                    Common.SetControlEnable(this.btnBack, true);
                    this.btnSave.Enabled = true;
                    this.btnSave.TabIndex = 0;
                    this.form.AcceptButton = this.btnSave;
                    this.btnSave.Focus();
                    //this.FormTitleText = InputBoxTitle.WizardMeaning;
                    break;
            }
        }
        /// <summary>
        /// 点击next时设置userinfo属性
        /// </summary>
        private bool SetUserInfo()
        {
            bool result = false;
            UserCreate createUser = dic[WizardName.CreateUser] as UserCreate;
            string message = string.Empty;
            if (createUser != null)
            {
                message = this.checkAllUserInfoFields(createUser);
            
                if (string.IsNullOrEmpty(message))
                {
                    int userid = new UserInfoBLL().GetCurrentUserID();
                    if (user == null)
                        user = new UserInfo();
                    user.Userid = userid + 1;
                    user.UserName = createUser.UserName.Trim();
                    user.Account = createUser.UserName.Trim();
                    user.FullName = createUser.FullName;
                    user.Description = createUser.Role;
                    user.Pwd = createUser.Password;
                    user.Disabled = 0;//false
                    user.Locked = 0;//false
                    user.RoleId = createUser.Group;
                    user.LastPwdChangedTime = DateTime.Now;
                    user.Remark = DateTime.Now.ToString();
                    result = true;
                }
                else
                {
                    Utils.ShowMessageBox(message, Messages.TitleError);
                }

            }
            
                
            
            return result;
        }

       
        private string checkAllUserInfoFields(UserCreate createUser)
        {
            StringBuilder message = new StringBuilder();
            string userName = createUser.UserName;
            string fullName = createUser.FullName;
            string password = createUser.Password;
            string passwordConfirm = createUser.ConfirmPwd;
            string role = createUser.Role;

            if (string.IsNullOrEmpty(userName))
            {
                message.Append(string.Format("User Name: {0}{1}", Messages.EmptyContentError, Environment.NewLine));
                createUser.SetFieldErrorToolTip(UserInfoFields.UserName, Messages.EmptyContentError);
            }
            if (_userbll.GetUserInfoByUsername(createUser.UserName).Userid != 0)
            {
                message.Append(string.Format("User Name: {0}{1}", Messages.UserNameOccupied, Environment.NewLine));
                createUser.SetFieldErrorToolTip(UserInfoFields.UserName, Messages.UserNameOccupied);
            }

            if (string.IsNullOrEmpty(fullName))
            {
                message.Append(string.Format("Full Name: {0}{1}", Messages.EmptyContentError, Environment.NewLine));
                createUser.SetFieldErrorToolTip(UserInfoFields.FullName, Messages.EmptyContentError);
            }

            if (string.IsNullOrEmpty(password))
            {
                message.Append(string.Format("Password: {0}{1}", Messages.EmptyContentError, Environment.NewLine));
                createUser.SetFieldErrorToolTip(UserInfoFields.Password, Messages.EmptyContentError);
            }
            else
            {
                if (policy == null || policy.MinPwdSize > password.Length)
                {
                    message.Append(string.Format("Password: {0}{1}", string.Format(Messages.PasswordShortThanDefined, Common.Policy.MinPwdSize), Environment.NewLine));
                    createUser.SetFieldErrorToolTip(UserInfoFields.Password, string.Format(Messages.PasswordShortThanDefined, Common.Policy.MinPwdSize));
                }
            }
            if (string.IsNullOrEmpty(passwordConfirm))
            {
                message.Append(string.Format("Confirm Password: {0}{1}", Messages.EmptyContentError, Environment.NewLine));
                createUser.SetFieldErrorToolTip(UserInfoFields.ConfirmPassword, Messages.EmptyContentError);
            }
            if (password != passwordConfirm)
            {
                message.Append(string.Format("Confirm Password: {0}{1}", Messages.MismatchPassword, Environment.NewLine));
                createUser.SetFieldErrorToolTip(UserInfoFields.ConfirmPassword, Messages.MismatchPassword);
            }
            if (string.IsNullOrEmpty(role))
            {
                message.Append(string.Format("Role: {0}{1}", Messages.EmptyContentError, Environment.NewLine));
                createUser.SetFieldErrorToolTip(UserInfoFields.Role, Messages.EmptyContentError);
            }

            return message.ToString();
        }

        private bool SetUserPolicy()
        {
            UserPolicy createPolicy = dic[WizardName.CreatePolicy] as UserPolicy;
            if (createPolicy.MinPwdSize == 0||!string.IsNullOrEmpty(createPolicy.Error))
                return false;
            policy.InactivityTime = createPolicy.InactivityTime;
            policy.LockedTimes = createPolicy.LockedTimes;
            policy.PwdExpiredDay = createPolicy.PwdExpiredDay;
            policy.MinPwdSize = createPolicy.MinPwdSize;
            policy.ProfileFolder = "";
            policy.Remark = DateTime.Now.ToString();
            return true;
        }
        private void InsertCreateUserLog()
        {
            if (Common.User.UserName != Common.SUPERUSER)
            {
                new OperationLogBLL().InsertLog(() =>
                {
                    Dictionary<string, object> d = new Dictionary<string, object>();
                    d.Add("OperateTime", DateTime.UtcNow);
                    d.Add("Action", LogAction.AddUser);
                    d.Add("UserName", flag == true ? user.UserName : Common.User.UserName);
                    d.Add("FullName", flag == true ? user.FullName : Common.User.FullName);
                    d.Add("Detail", user.UserName + " : " + (user.RoleId == 1 ? "Admin" : "User"));
                    d.Add("LogType", LogAction.SystemAuditTrail);
                    return d;
                });
            }
        }
        private void InsertAssignRightsLog()
        {
             List<string> right=(dic[WizardName.CreateRight] as UserRight).Right;
             if (right == null || right.Count <= 0)
                 return;
             string detail = user.UserName + " : ";
             right.ForEach(p => {
                 detail = detail + p+",";
             });
             detail = detail.Substring(0, detail.Length - 1);
             if (Common.User.UserName != Common.SUPERUSER)
             {
                 new OperationLogBLL().InsertLog(() =>
                 {
                     Dictionary<string, object> d = new Dictionary<string, object>();
                     d.Add("OperateTime", DateTime.UtcNow);
                     d.Add("Action", LogAction.AssignRights);
                     d.Add("UserName", flag == true ? user.UserName : Common.User.UserName);
                     d.Add("FullName", flag == true ? user.FullName : Common.User.FullName);
                     d.Add("Detail", detail);
                     d.Add("LogType", LogAction.SystemAuditTrail);
                     return d;
                 });
             }
        }
        private void SetNextEnableOnThird(object sender,EventArgs args)
        {
            ListBox lb = sender as ListBox;
            if (lb.Items.Contains(RightsText.SignRecords))
            {
                Common.SetControlEnable(this.btnNext, true);
                Common.SetControlEnable(this.btnBack, true);
                this.btnSave.Enabled = false;
                this.form.AcceptButton = this.btnNext;
            }
            else
            {
                Common.SetControlEnable(this.btnNext, false);
                Common.SetControlEnable(this.btnBack, true);
                this.btnSave.Enabled = true;
                this.btnSave.TabIndex = 0;
                this.form.AcceptButton = this.btnSave;
            }
            UserRight r = dic[WizardName.CreateRight] as UserRight;
            r.SetValue();
        }
    }
    enum WizardName { CreateUser,CreatePolicy,CreateRight,CreateMean }//panelname
    enum Step { first,second,third,fourth }//step by step

}
