using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ShineTech.TempCentre.DAL;
using System.Xml.Serialization;
using System.Reflection;
using ShineTech.TempCentre.Versions;
using System.Windows.Forms;
using ShineTech.TempCentre.Platform;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
namespace ShineTech.TempCentre.BusinessFacade
{
    public class Common
    {
        public  const string SUPERUSER="_superAdmin";
        public  const string SUPERUSERPWD = "1234!@#$Q";
        private static int _DescLength=50;
        private static ToolTip commonToolTip = new ToolTip();
        public static int DescLength
        {
            get { return Common._DescLength; }
            set { Common._DescLength = value; }
        }
        private static int _TimeOut;

        public static int TimeOut
        {
            get 
            {
                if (_TimeOut == 0)
                {
                    object obj = System.Configuration.ConfigurationManager.AppSettings["TimeOut"];
                    _TimeOut = obj == null ? 30 : Convert.ToInt32(obj);
                }
                return Common._TimeOut; 
            }
        }
        private static bool isRemoveDevice = false;
        public static string LoggerReadTime { get; set; }
        public static bool IsRemoveDevice
        {
            get { return Common.isRemoveDevice; }
            set { Common.isRemoveDevice = value; }
        }
        private static UserInfo user=new UserInfo ();
        private static Policy policy;
        private static readonly string RightsPath = Path.Combine(Application.StartupPath, "Rights.xml");
        public static Policy Policy
        {
            get 
            {
                if (policy == null)
                {
                    IDataProcessor processor = new DeviceProcessor();
                    Policy p = processor.QueryOne<Policy>("select * from policy", delegate() { return null; });
                    if (p != null&&p.ID!=0)
                        policy = p;
                    else
                    {
                        policy = new DAL.Policy();
                        policy.ID = 1;
                        policy.InactivityTime = 0;
                        policy.LockedTimes = 0;
                        policy.MinPwdSize = 6;
                        policy.ProfileFolder = "";
                        policy.PwdExpiredDay = 0;
                        policy.Remark = DateTime.Now.ToString();
                        processor.Insert<Policy>(policy, null);
                    }
                }
                return Common.policy; 
            }
            set { Common.policy = value; }
        }
        private static List<DAL.UserRight> userright;
        public static List<DAL.UserRight> Userright
        {
            get 
            {
              
                    UserRightBLL rightbll = new UserRightBLL();
                    return userright=rightbll.GetUserRightByUserName(User.UserName);

            }
            set { Common.userright = value; }
        }
        private static SoftwareVersions _version;

        public static SoftwareVersions Versions
        {
            get 
            {
                if (_version == SoftwareVersions.Init)
                {

                    if (SoftwareVersion.Version == SoftwareVersions.Pro)
                    {
                        _version = SoftwareVersions.Pro;
                    }
                    else
                        _version = SoftwareVersions.S;
                }
                return Common._version;
            }
        }
        private static bool isConnectCompleted;

        public static bool IsConnectCompleted
        {
            get { return Common.isConnectCompleted; }
            set { Common.isConnectCompleted = value; }
        }
        private static string _FormTitle;

        public static string FormTitle
        {
            get {
                if (string.IsNullOrEmpty(_FormTitle))
                {
                    if (_version == SoftwareVersions.Pro)
                        _FormTitle = "TempCentre";
                    else
                        _FormTitle = "TempCentre Lite";

                }
                return Common._FormTitle;
            }
            //set { Common._FormTitle = value; }
        }
        /// <summary>
        /// 用户信息
        /// </summary>
        public static UserInfo User
        {
            get { return user; }
            set { user = value; }
        }
        private static UserProfile _GlobalProfile;

        public static UserProfile GlobalProfile
        {
            get 
            {
                if (_GlobalProfile == null)
                    _GlobalProfile = GetGlobalUserProfile();
                return Common._GlobalProfile;
            }
            set { _GlobalProfile = value; }
        } 
        private static SuperDevice tag;
        private static SuperDevice _cloneTag;

        public static SuperDevice CloneTag
        {
            get { return Common._cloneTag; }
            set { Common._cloneTag = value; }
        }
        public static SuperDevice Tag
        {
            get
            {
                return tag;
            }
        }
        public static SuperDevice GetDeviceInstance(DeviceType deviceType)
        {

            string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string factoryName = assemblyName + "." + deviceType.ToString() + "Factory";
            Factory obj = Assembly.Load(assemblyName).CreateInstance(factoryName) as Factory;
            tag = obj.Creator();
            return tag;
        }
        public static bool TextBoxChecked(System.Windows.Forms.TextBox tb)
        {
            return tb.Text == "" ? false : true;
        }
        public static bool PasswordConfirmed(System.Windows.Forms.TextBox tb1, System.Windows.Forms.TextBox tb2)
        {
            return tb1.Text.Equals(tb2.Text) ;
        }
        public static void SetControlEnable(System.Windows.Forms.Control con, bool enabled) { con.Enabled = enabled; }
        public static Rights GetRightsList()
        {
            if (System.IO.File.Exists(RightsPath))
            {
                XmlSerializer xs = new XmlSerializer(typeof(Rights));
                using (System.IO.FileStream fs = new System.IO.FileStream(RightsPath, System.IO.FileMode.Open))
                {
                    Rights right = (Rights)xs.Deserialize(fs);
                    fs.Close();
                    return right;
                }
            }
            return null;
        }
        public static bool IsAuthorized(string right)
        {
            if (Versions == SoftwareVersions.S)
            {
                if (right == RightsText.ViewAuditTrail || right == RightsText.SignRecords)
                    return false;
                else
                    return true;
            }
            else
            {
                if (Userright != null && Userright.Count > 0)
                    return Userright.Select(p => p.Right).Contains(right);
            }
            return false;
        }
        public static Rights SetRightsList()
        {
            Rights right = new Rights();
            right.right = new List<string>();
            right.right.Add(RightsText.ConfigurateDevices);
            right.right.Add(RightsText.SignRecords);
            right.right.Add(RightsText.CommentRecords);
            right.right.Add(RightsText.DeleteRecords);
            right.right.Add(RightsText.DeleteUnsignedRecords);
            right.right.Add(RightsText.ViewAuditTrail);
            XmlSerializer xs = new XmlSerializer(typeof(Rights));
            using (System.IO.FileStream fs = new System.IO.FileStream(RightsPath, System.IO.FileMode.Create, FileAccess.Write))
            {
                xs.Serialize(fs, right);
                fs.Close();
            }
            return right;
        }

        public static UserInfo CurrentSelectedUserOfDgv
        {
            get;
            set;
        }

        public static UserInfo CurrentSelectedUserOfDgv1
        {
            get;
            set;
        }

        public static UserInfo CurrentSelectedUserOfDgv2
        {
            get;
            set;
        }
        /// <summary>
        /// 计算MKT值
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static string CalcMKT(List<int> list)
        {
            #region calc the mkt .
            double ndot, sum, ln, mkt;
            double factorValue = 10000;
            sum = 0;
            ndot = 0;
            try
            {
                //foreach (int iStr in list)
                //{
                //    sum += Math.Exp(-10 / Math.Round((1.0 * (iStr + 27315) / 100), 1, MidpointRounding.AwayFromZero));
                //    ndot++;
                //}

                //ln = Math.Log(sum / ndot);

                //mkt = 10 / (-ln) - 273.15;
                foreach (int iStr in list)
                {
                    
                    //sum += Math.Exp(-10 / Math.Round((1.0 * (iStr + 27315) / 100), 1, MidpointRounding.AwayFromZero));
                    sum += Math.Exp(-1 * factorValue / (1.0 * (iStr + 27315) / 100));
                    ndot++;
                }

                ln = Math.Log(sum / ndot);
                Debug.WriteLine(Math.E, "MathE");
                mkt = factorValue / (-ln) - 273.15;

                return mkt.ToString("F01");// Convert.ToString(mkt);             // MKT 平均动能温度
                //F02
            }
            catch
            {
                return "0.0";
            }
            #endregion
        }
        public static string SetTempTimeFormat(string s)
        {
            string result = s;
            try
            {
                if (!string.IsNullOrEmpty(result))
                {
                    int hIndex = result.IndexOf("@");
                    if (hIndex != -1)
                    {
                        string f1 = result.Substring(0, hIndex);
                        string f2 = result.Substring(hIndex + 1, result.Length - hIndex - 1);
                        if (!string.IsNullOrEmpty(f2))
                        {
                            result = f1 + "@" + DateTime.ParseExact(f2, Common.GetDefaultDateTimeFormat(), CultureInfo.InvariantCulture).ToLocalTime().ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture);
                        }
                    }
                }
            }
            catch { }
            return result;
        }
        public static UserProfile GetCurrentUserProfile()
        {
            UserProfileBLL _profile = new UserProfileBLL();
             //List<UserProfile> profile =  _profile.GetGlobalUserProfile();
            UserProfile currentProfile = _profile.GetProfileByUserName(Common.User.UserName == null ? "" : Common.User.UserName);
            ////判断是否有全局的设置
            
            return currentProfile;
        }
        public static GlobalEntity GetGlobalSetting()
        {
            GlobalEntity entity = new GlobalEntity();
            UserProfileBLL _profile = new UserProfileBLL();
            List<UserProfile> profile =  _profile.GetGlobalSetting();
            ////判断是否有全局的设置
            if (profile != null && profile.Count > 0)
            {
                entity.Id = profile[0].ID;
                entity.Logo = profile[0].Logo;
                entity.ContactInfo = profile[0].ContactInfo;
                return entity;
            }
            else
                return null;
        }
        public static UserProfile GetGlobalUserProfile()
        {
            //UserProfileBLL _profile = new UserProfileBLL();
            //List<UserProfile> profile =  _profile.GetGlobalUserProfile();
            UserProfile currentProfile = GetCurrentUserProfile();
            GlobalEntity global = GetGlobalSetting();
            if (global != null)
            {
                currentProfile.Logo = global.Logo;
                currentProfile.ContactInfo = global.ContactInfo;
            }
            return currentProfile;
        }
        public static SuperDevice CopyTo(SuperDevice tag)
        {
            if(tag!=null)
                return tag.Clone(Common.GetGlobalUserProfile().DateTimeFormator);
            return null;
        }
        public static string TransferTemp(string tempunit,string temp)
        {
            if (string.IsNullOrEmpty(tempunit) || string.IsNullOrEmpty(temp))
                return string.Empty;

            double t = Convert.ToDouble(temp);
            if (tempunit == "C")
            {
                return Math.Round(9 * t / 5 + 32,1).ToString();
            }
            else
            {
                return Math.Round(5 * (t - 32) / 9,1).ToString();
            }
        }

        //private static ToolTip toolTipSetter = new ToolTip();

        public static void SetToolTip(ToolTip toolTipSetter, Control control, string caption)
        {
            //toolTipSetter.ShowAlways = true;
            //toolTipSetter.AutoPopDelay = 0;
            //toolTipSetter.AutomaticDelay = 0;
            //toolTipSetter.InitialDelay = 0;
            //toolTipSetter.ReshowDelay = 0;
            if (control != null)
            {
                control.Visible = true;
                toolTipSetter.SetToolTip(control, caption);
                commonToolTip = new ToolTip();
                commonToolTip.ShowAlways = true;
                //commonToolTip.IsBalloon = true;
                commonToolTip.UseAnimation = false;
                commonToolTip.UseFading = false;
                commonToolTip.AutoPopDelay = 0;
                commonToolTip.AutomaticDelay = 0;
                commonToolTip.InitialDelay = 0;
                commonToolTip.ReshowDelay = 0;
                commonToolTip.SetToolTip(control, caption);
            }
        }

        public static String GetToolTip(ToolTip toolTipSetter, Control control)
        {
            string result = string.Empty;
            if (control != null)
            {
                result = toolTipSetter.GetToolTip(control);
            }
            return result;
        }


        public static void ClearToolTip(ToolTip toolTipSetter, Control control)
        {
            if (control != null)
            {
                toolTipSetter.SetToolTip(control, "");
                control.Visible = false;
            }
        }
        /// <summary>
        /// 签名
        /// </summary>
        public static void Sign(bool IsSaved, SuperDevice Tag, ref List<DigitalSignature> signatureList)
        {
            if (Tag != null)
            {
                SignConfirm sign = new SignConfirm(Tag.SerialNumber, Tag.TripNumber);
                DialogResult result = sign.ShowDialog();
                if (result == DialogResult.OK)
                {
                    Utils.ShowMessageBox(Messages.SignSuccessfully, Messages.TitleNotification);
                    signatureList = new DigitalSignatureBLL().GetDigitalSignatureBySnTn(Tag.SerialNumber, Tag.TripNumber);
                    //SetSignedRecordLabel(ref signatureList, ref lvSignature, ref lbSign);
                }
            }
        }
        private static void SetSignedRecordLabel( ref List<DigitalSignature> signatureList, ref ListBox lvSignature, ref Label lbSign)
        {
            if (signatureList != null && signatureList.Count > 0)
            {
                signatureList = signatureList.OrderBy(p => p.SignTime).ToList();
                lbSign.Text = string.Format("Signature[{0}]: {1} ", signatureList.Count, signatureList.Last().ToString(Common.GlobalProfile.DateTimeFormator));
                //设置电子签名list
                lvSignature.Items.Clear();
                for (int i = 0; i < signatureList.Count; i++)
                {
                    //string text = string.Format("    [{0}] {1}({2})_{3}_{4} ", i+1, list[i].UserName, list[i].FullName, list[i].MeaningDesc, list[i].SignTime);
                    string text = string.Format("[{0}] {1}", i + 1, signatureList[i].ToString(Common.GlobalProfile.DateTimeFormator));
                    lvSignature.Items.Add(text);
                }
            }
        }
        
        public static void Save(ref bool IsSaved, List<DigitalSignature> signatureList,ReportEditorBLL _reportEditorBll, DeviceBLL _deviceBll, AlarmConfigBLL _alarmConfigBll
            , LogConfigBLL _logConfigBll, SuperDevice Tag, string comments, string reporttile)
        {
            try
            {
                if (Tag.tempList == null || Tag.tempList.Count == 0)
                    return;
                Device device = GenerateDeviceObject(_deviceBll,Tag);
                ReportEditor editor = GetReportEditorSelection(_reportEditorBll,Tag.SerialNumber, Tag.TripNumber, comments,reporttile);
                

                if (!_deviceBll.IsDeviceInfoExist(device))
                {
                    if (_deviceBll.SaveDeviceInfomation(device, Tag.points
                                                        , GenerateLogConfigObject(_logConfigBll, Tag), GenerateAlarmConfig(_alarmConfigBll, Tag), signatureList))
                    {
                        //AddSaveRecordLog();
                        //AddCommentsLog();
                        _reportEditorBll.InsertReportEditor(editor);
                        Utils.ShowMessageBox(string.Format(Messages.SaveDataSuccessfully, Tag.SerialNumber, Tag.TripNumber), Messages.TitleNotification);
                        IsSaved = true;
                        //SaveRecordEvent(null, null);
                    }
                    else
                    {
                        Utils.ShowMessageBox(Messages.SaveDataFailed, Messages.TitleNotification);
                        IsSaved = false;
                    }
                }
                else
                {
                    int dataChangeStatus = Common.DeviceModificationType(Tag, new TextBox() { Text = comments }, new TextBox() { Text = reporttile });
                    string message = string.Empty;
                    if (dataChangeStatus == 0)
                    {
                        message = Messages.B93;
                    }
                    else
                    {
                        message = Messages.B94;
                    }
                    if (Utils.ShowMessageBox(string.Format(message, Tag.SerialNumber + "_" + Tag.TripNumber), Messages.TitleNotification, MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        _reportEditorBll.UpdateReportEditor(editor);
                        //AddCommentsLog();
                        _deviceBll.UpdateDeviceInfomation(device, Tag.points
                                                            , GenerateLogConfigObject(_logConfigBll, Tag), GenerateAlarmConfigForUpdate(_alarmConfigBll, Tag));
                        new DigitalSignatureBLL().InsertDigitalSignature(signatureList, null);
                        IsSaved = true;
                        Utils.ShowMessageBox(string.Format(Messages.SaveDataSuccessfully, Tag.SerialNumber, Tag.TripNumber), Messages.TitleNotification);
                    }
                }
            }

                //}
            catch (Exception ex)
            {
                //_tracing.Error(ex, "save the data to database failed.");
            }
        }

        public static void SaveTps(ref bool IsSaved, List<DigitalSignature> signatureList, ReportEditorBLL _reportEditorBll, DeviceBLL _deviceBll, AlarmConfigBLL _alarmConfigBll
            , LogConfigBLL _logConfigBll, SuperDevice Tag, string comments, string reporttile)
        {
            Device device = GenerateDeviceObject(_deviceBll,Tag);
            ReportEditor editor = GetReportEditorSelection(_reportEditorBll, Tag.SerialNumber, Tag.TripNumber, comments, reporttile);
            string tripNumberSubfix = _deviceBll.GetNextTripNumberForDeviceTag(device);
            if (string.IsNullOrEmpty(tripNumberSubfix))
            {
                editor = GetReportEditorSelection(_reportEditorBll, Tag.SerialNumber, Tag.TripNumber, comments, reporttile);
                if (_deviceBll.SaveDeviceInfomation(device, Tag.points
                                          , GenerateLogConfigObject(_logConfigBll, Tag), GenerateAlarmConfig(_alarmConfigBll, Tag), signatureList))
                {
                    _reportEditorBll.InsertReportEditor(editor);
                    Utils.ShowMessageBox(string.Format(Messages.SaveDataSuccessfully, Tag.SerialNumber, Tag.TripNumber), Messages.TitleNotification);
                    IsSaved = true;
                }
                else
                {
                    Utils.ShowMessageBox(Messages.SaveDataFailed, Messages.TitleNotification);
                    IsSaved = false;
                }
            }
            else
            {
                string finalTripNumber = string.Empty;
                string currentTripNumberWithOutSubfix = string.Empty;
                if (Tag.TripNumber.IndexOf('_') != -1)
                {
                    currentTripNumberWithOutSubfix = Tag.TripNumber.Substring(0, Tag.TripNumber.IndexOf('_'));
                }
                else
                {
                    currentTripNumberWithOutSubfix = Tag.TripNumber;
                }
                DialogResult dialogResult = Utils.ShowMessageBox(string.Format(Messages.SameRecordFoundInDatabase, string.Format("{0}_{1}", Tag.SerialNumber, Tag.TripNumber), string.Format("{0}_{1}_{2}", Tag.SerialNumber, currentTripNumberWithOutSubfix, tripNumberSubfix)), Messages.TitleNotification, MessageBoxButtons.YesNoCancel);
                switch (dialogResult)
                {
                    case DialogResult.Cancel:
                        IsSaved = false;
                        break;
                    case DialogResult.No:
                        Tag.TripNumber = string.Format("{0}_{1}", currentTripNumberWithOutSubfix, tripNumberSubfix);
                        Tag.points.TN = Tag.TripNumber;
                        device.TripNum = Tag.TripNumber;
                        foreach (var item in signatureList)
                        {
                            item.TN = Tag.TripNumber;
                        }
                        editor = GetReportEditorSelection(_reportEditorBll, Tag.SerialNumber, Tag.TripNumber, comments, reporttile);
                        if (_deviceBll.SaveDeviceInfomation(device, Tag.points
                                                  , GenerateLogConfigObject(_logConfigBll, Tag), GenerateAlarmConfig(_alarmConfigBll, Tag), signatureList))
                        {
                            _reportEditorBll.InsertReportEditor(editor);
                            Utils.ShowMessageBox(string.Format(Messages.SaveDataSuccessfully, Tag.SerialNumber, Tag.TripNumber), Messages.TitleNotification);
                            IsSaved = true;
                        }
                        else
                        {
                            Utils.ShowMessageBox(Messages.SaveDataFailed, Messages.TitleNotification);
                            IsSaved = false;
                        }
                        break;
                    case DialogResult.Yes:
                        if (_deviceBll.UpdateDeviceInfomation(device, Tag.points, GenerateLogConfigObject(_logConfigBll, Tag), GenerateAlarmConfigForUpdate(_alarmConfigBll, Tag)))
                        {
                            _reportEditorBll.UpdateReportEditor(editor);
                            new DigitalSignatureBLL().InsertDigitalSignature(signatureList, null);
                            Utils.ShowMessageBox(string.Format(Messages.SaveDataSuccessfully, Tag.SerialNumber, Tag.TripNumber), Messages.TitleNotification);
                        }
                        else
                        {
                            if (_deviceBll.SaveDeviceInfomation(device, Tag.points
                                                  , GenerateLogConfigObject(_logConfigBll, Tag), GenerateAlarmConfig(_alarmConfigBll, Tag), signatureList))
                            {
                                _reportEditorBll.InsertReportEditor(editor);
                                Utils.ShowMessageBox(string.Format(Messages.SaveDataSuccessfully, Tag.SerialNumber, Tag.TripNumber), Messages.TitleNotification);
                                IsSaved = true;
                            }
                            else
                            {
                                Utils.ShowMessageBox(Messages.SaveDataFailed, Messages.TitleNotification);
                                IsSaved = false;
                            }
                        }
                        
                        IsSaved = true;
                        break;
                    default:
                        IsSaved = false;
                        break;
                }
            }
        }

        public static void Save(ref bool IsSaved, List<DigitalSignature> signatureList, ReportEditorBLL _reportEditorBll, DeviceBLL _deviceBll, SuperDevice Tag,string comments,string reporttile, ReportEditor originalEditor)
        {
            try
            {
                Device device = GenerateDeviceObject(_deviceBll, Tag);
                ReportEditor editor = GetReportEditorSelection(_reportEditorBll, Tag.SerialNumber, Tag.TripNumber, comments,reporttile);
                _reportEditorBll.UpdateReportEditor(editor);
                originalEditor.Comments = editor.Comments;
                originalEditor.ReportTitle = editor.ReportTitle;
                new DigitalSignatureBLL().InsertDigitalSignature(signatureList, null);
                IsSaved = true;
                Utils.ShowMessageBox(string.Format(Messages.SaveDataSuccessfully, Tag.SerialNumber, Tag.TripNumber), Messages.TitleNotification);
            }
            catch (Exception ex)
            {
                //_tracing.Error(ex, "save the data to database failed.");
            }
        }
        public static bool IsDeviceModification(SuperDevice Tag,TextBox tbcmt,TextBox tbreporttitle)
        {
            bool result = false;
            if (Tag==null||Tag.tempList.Count == 0||Tag.RunStatus==0||Tag.RunStatus==1)
                result = false;
            else
            {
                PointTempBLL _point = new PointTempBLL();
                PointInfo temp= _point.GetPointsListByTNSN(Tag.SerialNumber, Tag.TripNumber);
                List<PointKeyValue> list = ObjectManage.DeserializePointKeyValue<PointKeyValue>(temp);
                if ((list != null && list.Count ==0) ||( list.Count < Tag.tempList.Count))
                {
                    result = true;
                }
                else
                {
                    ReportEditorBLL _report = new ReportEditorBLL();
                    ReportEditor editor = _report.GetReportEditorBySnTn(Tag.SerialNumber, Tag.TripNumber);
                    if (tbcmt.Text != editor.Comments && editor.ID != 0)
                        result = true;
                    else if (tbreporttitle.Text.Trim() != ReportConstString.TitleDefaultString && !string.IsNullOrEmpty(tbreporttitle.Text) && tbreporttitle.Text != Common.GlobalProfile.ReportTitle && tbreporttitle.Text != editor.ReportTitle && editor.ID != 0)
                        result = true;
                    else if (editor.ID == 0 && (tbreporttitle.Text.Trim() != ReportConstString.TitleDefaultString && !string.IsNullOrEmpty(tbreporttitle.Text)))
                        result = true;
                    else
                        result = false;
                }
            }
            return result;
        }
        public static int DeviceModificationType(SuperDevice Tag, TextBox tbcmt, TextBox tbreporttitle)
        {
            int result = 0;
            if (Tag == null || Tag.tempList.Count == 0 || Tag.RunStatus == 0 || Tag.RunStatus == 1)
                result = -1;
            else
            {
                PointTempBLL _point = new PointTempBLL();
                PointInfo temp = _point.GetPointsListByTNSN(Tag.SerialNumber, Tag.TripNumber);
                List<PointKeyValue> list = ObjectManage.DeserializePointKeyValue<PointKeyValue>(temp);
                if ((list != null && list.Count == 0) || (list.Count < Tag.tempList.Count))
                {
                    result = 0;
                }
                else
                {
                    ReportEditorBLL _report = new ReportEditorBLL();
                    ReportEditor editor = _report.GetReportEditorBySnTn(Tag.SerialNumber, Tag.TripNumber);
                    if(tbcmt.Text != editor.Comments && editor.ID != 0)
                        result = 1;
                    else if (tbreporttitle.Text.Trim() != ReportConstString.TitleDefaultString && !string.IsNullOrEmpty(tbreporttitle.Text) && tbreporttitle.Text != Common.GlobalProfile.ReportTitle && tbreporttitle.Text != editor.ReportTitle && editor.ID != 0)
                        result = 1;
                    else
                        result = -1;
                }
            }
            return result;
        }
        public static bool IsDeviceModification(SuperDevice Tag, TextBox tbcmt, TextBox tbreporttitle,ReportEditor editor)
        {
            bool result = false;
            if (Tag.tempList.Count == 0 || Tag.RunStatus == 0 || Tag.RunStatus == 1)
                result = false;
            else
            {
                PointTempBLL _point = new PointTempBLL();
                PointInfo temp = _point.GetPointsListByTNSN(Tag.SerialNumber, Tag.TripNumber);
                List<PointKeyValue> list = ObjectManage.DeserializePointKeyValue<PointKeyValue>(temp);
                if (list != null && list.Count > 0 && list.Count < Tag.tempList.Count)
                {
                    result = true;
                }
                else
                {
                    if (tbcmt.Text != editor.Comments && editor.ID != 0)
                        result = true;
                    else if (tbreporttitle.Text.Trim() != ReportConstString.TitleDefaultString && !string.IsNullOrEmpty(tbreporttitle.Text) && tbreporttitle.Text != editor.ReportTitle && editor.ID != 0)
                        result = true;
                    else
                        result = false;
                }
            }
            return result;
        }
        public static void AddSaveRecordLog(OperationLogBLL _logBll, SuperDevice Tag)
        {
            /*记录log日志*/
            //记录成功的日志
            if (Tag != null)
            {
                if (Common.User.UserName != Common.SUPERUSER)
                {
                    _logBll.InsertLog(() =>
                    {
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic.Add("OperateTime", DateTime.UtcNow);
                        dic.Add("Action", LogAction.Saverecord);
                        dic.Add("UserName", Common.User.UserName);
                        dic.Add("FullName", Common.User.FullName);
                        dic.Add("Detail", Tag.SerialNumber + "_" + Tag.TripNumber);
                        dic.Add("LogType", LogAction.AnalysisAuditTrail);// analysis audit trail
                        return dic;
                    });
                }
            }
        }
        public static void AddCommentsLog(OperationLogBLL _logBll, SuperDevice Tag,TextBox tbCmt)
        {
            if (Tag!=null&&tbCmt.Text != string.Empty && tbCmt.Text != ControlText.CommentText)
                if (Common.User.UserName != Common.SUPERUSER)
                {
                    _logBll.InsertLog(() =>
                    {
                        Dictionary<string, object> dic = new Dictionary<string, object>();
                        dic.Add("OperateTime", DateTime.UtcNow);
                        dic.Add("Action", LogAction.Commentrecord);
                        dic.Add("UserName", Common.User.UserName);
                        dic.Add("FullName", Common.User.FullName);
                        dic.Add("Detail", Tag.SerialNumber + "_" + Tag.TripNumber);
                        dic.Add("LogType", LogAction.AnalysisAuditTrail);// analysis audit trail
                        return dic;
                    });
                }
        }
        public static ReportEditor GetReportEditorSelection( ReportEditorBLL _reportEditorBll, string sn, string tn,string comments,string reportTitle)
        {
            ReportEditor editor = _reportEditorBll.GetReportEditorBySnTn(sn, tn);
            if (editor != null && editor.ID == 0)
            {
                editor.ID = _reportEditorBll.GetReportEditorPKValue() + 1;
            }
            editor.SN = sn;
            editor.TN = tn;
            editor.Comments = comments;
            editor.ReportTitle = reportTitle;
            editor.Remark = DateTime.UtcNow.ToString();
            return editor;
        }
        /// <summary>
        /// 构造device对象
        /// </summary>
        /// <returns></returns>
        private static Device GenerateDeviceObject(DeviceBLL _deviceBll,SuperDevice Tag)
        {
            Device device = new Device();
            int id = _deviceBll.GetDevicePKValue();
            device.ID = id + 1;
            device.DeviceID = Tag.DeviceID;
            device.ProductName = Tag.ProductName;
            device.SerialNum = Tag.SerialNumber;
            device.TripNum = Tag.TripNumber;
            device.Model = Tag.Model;
            device.Memory = Convert.ToDecimal(Tag.Memory);
            device.Battery = Tag.Battery;
            device.DESCS = Tag.Description;
            device.AlarmMode = Tag.AlarmMode;
            device.LoggerReader = Tag.LoggerRead;
            device.Remark = DateTime.UtcNow.ToString(GetDefaultDateTimeFormat(), CultureInfo.InvariantCulture);
            return device;
        }
        /// <summary>
        /// 构造logconfig对象
        /// </summary>
        /// <returns></returns>
        private static LogConfig GenerateLogConfigObject(LogConfigBLL _logConfigBll, SuperDevice Tag)
        {
            LogConfig log = new LogConfig();
            int id = _logConfigBll.GetLogConfigPKValue();
            log.ID = id + 1;
            log.LogInterval = Tag.LogInterval;
            log.ProductName = Tag.ProductName;
            log.SN = Tag.SerialNumber;
            log.TN = Tag.TripNumber;
            log.StartMode = Tag.StartModel;
            if (Tag.LogStartDelay != null)
            {
                log.StartDelay = Tag.LogStartDelay;
            }
            else
            {
                log.StartDelay = Tag.StartConditionTime.ToString();
            }
            log.LogCycle = Tag.LogCycle;
            log.Remark = DateTime.UtcNow.ToString();
            return log;
        }
        /// <summary>
        /// 构造alarmconfig对象
        /// </summary>
        /// <returns></returns>
        private static List<AlarmConfig> GenerateAlarmConfig(AlarmConfigBLL _alarmConfigBll, SuperDevice Tag)
        {
            List<AlarmConfig> list = new List<AlarmConfig>();
            #region 报警线
            switch (Tag.AlarmMode)
            {
                case 1:
                    AlarmConfig alarm = new AlarmConfig();
                    int id = _alarmConfigBll.GetAlarmConfigPKValue();
                    alarm.ID = ++id;
                    if (!string.IsNullOrEmpty(Tag.AlarmLowLimit))
                    {
                        alarm.SN = Tag.SerialNumber;
                        alarm.TN = Tag.TripNumber;
                        alarm.ProductName = Tag.ProductName;
                        alarm.AlarmMode = Tag.LowAlarmType;
                        alarm.AlarmLevel = "A1";
                        alarm.AlarmTemp = Tag.AlarmLowLimit;
                        alarm.AlarmType = Tag.LowAlarmType;
                        alarm.AlarmDelay = Tag.AlarmLowDelay;
                        alarm.IsAlarm = Tag.AlarmLowStatus;
                        alarm.AlarmTotalTime = Tag.LowAlarmTotalTimeBelow;
                        alarm.AlarmNumbers = Tag.LowAlarmEvents;
                        alarm.AlarmFirstTriggered = (Tag.LowAlarmFirstTrigged);
                        alarm.AlarmEnable = true;
                        alarm.Remark = DateTime.UtcNow.ToString();
                        list.Add(alarm);
                    }
                    alarm = new AlarmConfig();
                    if (!string.IsNullOrEmpty(Tag.AlarmHighLimit))
                    {
                        alarm.ID = ++id;
                        alarm.SN = Tag.SerialNumber;
                        alarm.TN = Tag.TripNumber;
                        alarm.ProductName = Tag.ProductName;
                        alarm.AlarmMode = Tag.HighAlarmType;
                        alarm.AlarmLevel = "A6";
                        alarm.AlarmTemp = Tag.AlarmHighLimit;
                        alarm.AlarmType = Tag.HighAlarmType;
                        alarm.AlarmDelay = Tag.AlarmHighDelay;
                        alarm.IsAlarm = Tag.AlarmHighStatus;
                        alarm.AlarmTotalTime = Tag.HighAlarmTotalTimeAbove;
                        alarm.AlarmNumbers = Tag.HighAlarmEvents;
                        alarm.AlarmFirstTriggered = (Tag.HighAlarmFirstTrigged);
                        alarm.AlarmEnable = true;
                        alarm.Remark = DateTime.UtcNow.ToString();
                        list.Add(alarm);
                    }
                    break;
                case 2:
                    alarm = new AlarmConfig();
                    id = _alarmConfigBll.GetAlarmConfigPKValue();
                    if (!string.IsNullOrEmpty(Tag.A5))
                    {
                        alarm.ID = ++id;
                        alarm.SN = Tag.SerialNumber;
                        alarm.TN = Tag.TripNumber;
                        alarm.ProductName = Tag.ProductName;
                        alarm.AlarmMode = Tag.AlarmTypeA5;
                        alarm.AlarmLevel = "A5";
                        alarm.AlarmTemp = Tag.A5;
                        alarm.AlarmType = Tag.AlarmTypeA5;
                        alarm.AlarmDelay = Tag.AlarmDelayA5.ToString();
                        alarm.IsAlarm = Tag.AlarmA5Status;
                        alarm.AlarmTotalTime = Tag.AlarmTotalTimeA5;
                        alarm.AlarmNumbers = Tag.AlarmNumA5;
                        alarm.AlarmFirstTriggered = (Tag.AlarmA5First);
                        alarm.AlarmEnable = true;
                        alarm.Remark = DateTime.UtcNow.ToString();
                        list.Add(alarm);
                    }
                    alarm = new AlarmConfig();
                    if (!string.IsNullOrEmpty(Tag.A4))
                    {
                        alarm.ID = ++id;
                        alarm.SN = Tag.SerialNumber;
                        alarm.TN = Tag.TripNumber;
                        alarm.ProductName = Tag.ProductName;
                        alarm.AlarmMode = Tag.AlarmTypeA4;
                        alarm.AlarmLevel = "A4";
                        alarm.AlarmTemp = Tag.A4;
                        alarm.AlarmType = Tag.AlarmTypeA4;
                        alarm.AlarmDelay = Tag.AlarmDelayA4.ToString();
                        alarm.IsAlarm = Tag.AlarmA4Status;
                        alarm.AlarmTotalTime = Tag.AlarmTotalTimeA4;
                        alarm.AlarmNumbers = Tag.AlarmNumA4;
                        alarm.AlarmFirstTriggered = (Tag.AlarmA4First);
                        alarm.AlarmEnable = true;
                        alarm.Remark = DateTime.UtcNow.ToString();
                        list.Add(alarm);
                    }
                    alarm = new AlarmConfig();
                    
                    if (!string.IsNullOrEmpty(Tag.A3))
                    {
                        alarm.ID = ++id;
                        alarm.SN = Tag.SerialNumber;
                        alarm.TN = Tag.TripNumber;
                        alarm.ProductName = Tag.ProductName;
                        alarm.AlarmMode = Tag.AlarmTypeA3;
                        alarm.AlarmLevel = "A3";
                        alarm.AlarmTemp = Tag.A3;
                        alarm.AlarmType = Tag.AlarmTypeA3;
                        alarm.AlarmDelay = Tag.AlarmDelayA3.ToString();
                        alarm.IsAlarm = Tag.AlarmA3Status ;
                        alarm.AlarmTotalTime = Tag.AlarmTotalTimeA3;
                        alarm.AlarmNumbers = Tag.AlarmNumA3;
                        alarm.AlarmFirstTriggered =(Tag.AlarmA3First);
                        alarm.AlarmEnable = true;
                        alarm.Remark = DateTime.UtcNow.ToString();
                        list.Add(alarm);
                    }
                    alarm = new AlarmConfig();
                    
                    if (!string.IsNullOrEmpty(Tag.A2))
                    {
                        alarm.ID = ++id;
                        alarm.SN = Tag.SerialNumber;
                        alarm.TN = Tag.TripNumber;
                        alarm.ProductName = Tag.ProductName;
                        alarm.AlarmMode = Tag.AlarmTypeA2;
                        alarm.AlarmLevel = "A2";
                        alarm.AlarmTemp = Tag.A2;
                        alarm.AlarmType = Tag.AlarmTypeA2;
                        alarm.AlarmDelay = Tag.AlarmDelayA2.ToString();
                        alarm.IsAlarm = Tag.AlarmA2Status;
                        alarm.AlarmTotalTime = Tag.AlarmTotalTimeA2;
                        alarm.AlarmNumbers = Tag.AlarmNumA2;
                        alarm.AlarmFirstTriggered = (Tag.AlarmA2First);
                        alarm.AlarmEnable = true;
                        alarm.Remark = DateTime.UtcNow.ToString();
                        list.Add(alarm);
                    }
                    alarm = new AlarmConfig();
                    
                    if (!string.IsNullOrEmpty(Tag.A1))
                    {
                        alarm.ID = ++id;
                        alarm.SN = Tag.SerialNumber;
                        alarm.TN = Tag.TripNumber;
                        alarm.ProductName = Tag.ProductName;
                        alarm.AlarmMode = Tag.AlarmTypeA1;
                        alarm.AlarmLevel = "A1";
                        alarm.AlarmTemp = Tag.A1;
                        alarm.AlarmType = Tag.AlarmTypeA1;
                        alarm.AlarmDelay = Tag.AlarmDelayA1.ToString();
                        alarm.IsAlarm = Tag.AlarmA1Status;
                        alarm.AlarmTotalTime = Tag.AlarmTotalTimeA1;
                        alarm.AlarmNumbers = Tag.AlarmNumA1;
                        alarm.AlarmFirstTriggered = (Tag.AlarmA1First);
                        alarm.AlarmEnable = true;
                        alarm.Remark = DateTime.UtcNow.ToString();
                        list.Add(alarm);
                    }
                    break;
            }
            #endregion
            return list;
        }
        private static List<AlarmConfig> GenerateAlarmConfigForUpdate(AlarmConfigBLL _alarmConfigBll, SuperDevice Tag)
        {
            List<AlarmConfig> list= _alarmConfigBll.GetAlarmConfigBySnTn(Tag.SerialNumber, Tag.TripNumber);
            #region 报警线
            switch (Tag.AlarmMode)
            {
                case 1:
                    AlarmConfig alarm = list.Where(p => p.AlarmLevel == "A1").LastOrDefault();
                    if (!string.IsNullOrEmpty(Tag.AlarmLowLimit) && alarm!=null)
                    {
                        alarm.SN = Tag.SerialNumber;
                        alarm.TN = Tag.TripNumber;
                        alarm.ProductName = Tag.ProductName;
                        alarm.AlarmMode = Tag.LowAlarmType;
                        alarm.AlarmLevel = "A1";
                        alarm.AlarmTemp = Tag.AlarmLowLimit;
                        alarm.AlarmType = Tag.LowAlarmType;
                        alarm.AlarmDelay = Tag.AlarmLowDelay;
                        alarm.IsAlarm = Tag.AlarmLowStatus;
                        alarm.AlarmTotalTime = Tag.LowAlarmTotalTimeBelow;
                        alarm.AlarmNumbers = Tag.LowAlarmEvents;
                        alarm.AlarmFirstTriggered = (Tag.LowAlarmFirstTrigged);
                        alarm.AlarmEnable = true;
                        alarm.Remark = DateTime.UtcNow.ToString();
                        //list.Add(alarm);
                    }
                    alarm = list.Where(p => p.AlarmLevel == "A6").LastOrDefault();
                    if (!string.IsNullOrEmpty(Tag.AlarmHighLimit) && alarm != null)
                    {
                        alarm.SN = Tag.SerialNumber;
                        alarm.TN = Tag.TripNumber;
                        alarm.ProductName = Tag.ProductName;
                        alarm.AlarmMode = Tag.HighAlarmType;
                        alarm.AlarmLevel = "A6";
                        alarm.AlarmTemp = Tag.AlarmHighLimit;
                        alarm.AlarmType = Tag.HighAlarmType;
                        alarm.AlarmDelay = Tag.AlarmHighDelay;
                        alarm.IsAlarm = Tag.AlarmHighStatus;
                        alarm.AlarmTotalTime = Tag.HighAlarmTotalTimeAbove;
                        alarm.AlarmNumbers = Tag.HighAlarmEvents;
                        alarm.AlarmFirstTriggered = (Tag.HighAlarmFirstTrigged);
                        alarm.AlarmEnable = true;
                        alarm.Remark = DateTime.UtcNow.ToString();
                        
                    }
                    break;
                case 2:
                    alarm = list.Where(p => p.AlarmLevel == "A5").LastOrDefault();
                    if (!string.IsNullOrEmpty(Tag.A5) && alarm != null)
                    {
                        alarm.SN = Tag.SerialNumber;
                        alarm.TN = Tag.TripNumber;
                        alarm.ProductName = Tag.ProductName;
                        alarm.AlarmMode = Tag.AlarmTypeA5;
                        alarm.AlarmLevel = "A5";
                        alarm.AlarmTemp = Tag.A5;
                        alarm.AlarmType = Tag.AlarmTypeA5;
                        alarm.AlarmDelay = Tag.AlarmDelayA5.ToString();
                        alarm.IsAlarm = Tag.AlarmA5Status;
                        alarm.AlarmTotalTime = Tag.AlarmTotalTimeA5;
                        alarm.AlarmNumbers = Tag.AlarmNumA5;
                        alarm.AlarmFirstTriggered = (Tag.AlarmA5First);
                        alarm.AlarmEnable = true;
                        alarm.Remark = DateTime.UtcNow.ToString();
                        
                    }

                    alarm = list.Where(p => p.AlarmLevel == "A4").LastOrDefault();
                    if (!string.IsNullOrEmpty(Tag.A4) && alarm != null)
                    {
                        alarm.SN = Tag.SerialNumber;
                        alarm.TN = Tag.TripNumber;
                        alarm.ProductName = Tag.ProductName;
                        alarm.AlarmMode = Tag.AlarmTypeA4;
                        alarm.AlarmLevel = "A4";
                        alarm.AlarmTemp = Tag.A4;
                        alarm.AlarmType = Tag.AlarmTypeA4;
                        alarm.AlarmDelay = Tag.AlarmDelayA4.ToString();
                        alarm.IsAlarm = Tag.AlarmA4Status;
                        alarm.AlarmTotalTime = Tag.AlarmTotalTimeA4;
                        alarm.AlarmNumbers = Tag.AlarmNumA4;
                        alarm.AlarmFirstTriggered = (Tag.AlarmA4First);
                        alarm.AlarmEnable = true;
                        alarm.Remark = DateTime.UtcNow.ToString();
                        
                    }

                    alarm = list.Where(p => p.AlarmLevel == "A3").LastOrDefault();
                    if (!string.IsNullOrEmpty(Tag.A3) && alarm != null)
                    {
                        alarm.SN = Tag.SerialNumber;
                        alarm.TN = Tag.TripNumber;
                        alarm.ProductName = Tag.ProductName;
                        alarm.AlarmMode = Tag.AlarmTypeA3;
                        alarm.AlarmLevel = "A3";
                        alarm.AlarmTemp = Tag.A3;
                        alarm.AlarmType = Tag.AlarmTypeA3;
                        alarm.AlarmDelay = Tag.AlarmDelayA3.ToString();
                        alarm.IsAlarm = Tag.AlarmA3Status;
                        alarm.AlarmTotalTime = Tag.AlarmTotalTimeA3;
                        alarm.AlarmNumbers = Tag.AlarmNumA3;
                        alarm.AlarmFirstTriggered = (Tag.AlarmA3First);
                        alarm.AlarmEnable = true;
                        alarm.Remark = DateTime.UtcNow.ToString();
                        
                    }
                    alarm = list.Where(p => p.AlarmLevel == "A2").LastOrDefault();
                    if (!string.IsNullOrEmpty(Tag.A2) && alarm != null)
                    {
                        alarm.SN = Tag.SerialNumber;
                        alarm.TN = Tag.TripNumber;
                        alarm.ProductName = Tag.ProductName;
                        alarm.AlarmMode = Tag.AlarmTypeA2;
                        alarm.AlarmLevel = "A2";
                        alarm.AlarmTemp = Tag.A2;
                        alarm.AlarmType = Tag.AlarmTypeA2;
                        alarm.AlarmDelay = Tag.AlarmDelayA2.ToString();
                        alarm.IsAlarm = Tag.AlarmA2Status;
                        alarm.AlarmTotalTime = Tag.AlarmTotalTimeA2;
                        alarm.AlarmNumbers = Tag.AlarmNumA2;
                        alarm.AlarmFirstTriggered = (Tag.AlarmA2First);
                        alarm.AlarmEnable = true;
                        alarm.Remark = DateTime.UtcNow.ToString();
                        
                    }
                    alarm = list.Where(p => p.AlarmLevel == "A1").LastOrDefault();
                    if (!string.IsNullOrEmpty(Tag.A1) && alarm != null)
                    {
                        alarm.SN = Tag.SerialNumber;
                        alarm.TN = Tag.TripNumber;
                        alarm.ProductName = Tag.ProductName;
                        alarm.AlarmMode = Tag.AlarmTypeA1;
                        alarm.AlarmLevel = "A1";
                        alarm.AlarmTemp = Tag.A1;
                        alarm.AlarmType = Tag.AlarmTypeA1;
                        alarm.AlarmDelay = Tag.AlarmDelayA1.ToString();
                        alarm.IsAlarm = Tag.AlarmA1Status;
                        alarm.AlarmTotalTime = Tag.AlarmTotalTimeA1;
                        alarm.AlarmNumbers = Tag.AlarmNumA1;
                        alarm.AlarmFirstTriggered = (Tag.AlarmA1First);
                        alarm.AlarmEnable = true;
                        alarm.Remark = DateTime.UtcNow.ToString();
                        
                    }
                    break;
            }
            #endregion
            return list;
        }
        public static int GetTextPixel(string text,System.Drawing.Graphics g,System.Drawing.Font f)
        {
            int i=(int)Math.Ceiling(g.MeasureString(text, f).Width);
            return i;
        }
        public static void SetDefaultPathForSaveFileDialog(SaveFileDialog saveFileDialog, SavingFileType saveFileType)
        {
            string userSetPath = Common.GlobalProfile != null ? Common.GlobalProfile.DefaultPath : string.Empty;
            if (string.IsNullOrEmpty(userSetPath))
            {
                userSetPath = Path.Combine(Application.StartupPath, "TempCentre Data");
            }
            switch (saveFileType)
            {
                case SavingFileType.AuditTrail:
                    userSetPath = Path.Combine(userSetPath, "Audit Trail");
                    saveFileDialog.FileName = DateTime.Now.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    break;
                case SavingFileType.DeviceConfig:
                    userSetPath = Path.Combine(userSetPath, "Config");
                    saveFileDialog.FileName = "New Config";
                    break;
                default:
                    break;
            }
            DirectoryInfo dirInfo = new DirectoryInfo(userSetPath);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            saveFileDialog.InitialDirectory = userSetPath;
        }

        public static void SetDefaultPathForOpenFileDialog(OpenFileDialog openFileDialog, OpenFileType openFileType)
        {
            string userSetPath = Common.GlobalProfile != null ? Common.GlobalProfile.DefaultPath : string.Empty;
            if (string.IsNullOrEmpty(userSetPath))
            {
                userSetPath = Path.Combine(Application.StartupPath, "TempCentre Data");
            }
            switch (openFileType)
            {
                case OpenFileType.OpenDeviceConfig:
                    userSetPath = Path.Combine(userSetPath, "Config");
                    break;
                default:
                    break;
            }
            DirectoryInfo dirInfo = new DirectoryInfo(userSetPath);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            openFileDialog.InitialDirectory = userSetPath;
        }

        public static string GetDefaultDateTimeFormat()
        {
            return "yyyy/MM/dd HH:mm:ss";
        }
        public static string GetDateOrTimeFormat(bool isDate,string format)
        {
            string result=format;
            if (isDate)
                result = format.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0] ?? format;
            else
            {
                string[] splits = format.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (splits != null)
                {
                    result = string.Empty;
                    for (int i = 1; i < splits.Length; i++)
                    {
                        result += splits[i] + " ";
                    }
                }
                    //result = format.Split(new char[1] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1] ?? format;
            }
            return result.TrimEnd();

        }
        public static List<PointKeyValue> GetTempPointsLocalTime(List<PointKeyValue> TempList)
        {
            if (TempList.Count == 0)
                return TempList;
            else
            {
                List<PointKeyValue> list = new List<PointKeyValue>();
                TempList.ForEach(p => list.Add(new PointKeyValue() { PointTemp = p.PointTemp, IsMark = p.IsMark, PointTime = p.PointTime.ToLocalTime() }));
                return list;
            }
        }
    }


    public enum SavingFileType
    {
        Report,
        AuditTrail,
        DeviceConfig
    }

    public enum OpenFileType
    {
        OpenTPS,
        OpenDeviceConfig,
        None
    }
}
