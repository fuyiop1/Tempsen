using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.DAL;
///<summary>
///CLR Ver : 4.0.30319.225
///CreateBy: wangfei
///CreateOn:9/5/2011 3:41:21 PM
///FileName:OptionsUI
///</summary>
namespace ShineTech.TempCentre.BusinessFacade
{
    public class OptionsUI
    {
        private UserProfile _profile;
        private OptionsUI()
        {
            _profile = Common.GetCurrentUserProfile();
        }
        public static OptionsUI CreateInstance()
        {
            return new OptionsUI();
        }
        public string GetTextFromRGB(byte r, byte g, byte b)
        {
            return string.Format("{0},{1},{2}", r, g, b);
        }
        public System.Drawing.Color GetColorFromPallete(System.Drawing.Color color)
        {
            ColorDialog dialog = new ColorDialog();
            dialog.Color = color;
            dialog.FullOpen = true;
            DialogResult result = dialog.ShowDialog();
            if (result == DialogResult.OK)//确定事件响应
            {
                return dialog.Color;
            }
            else
                return color;
        }
        public System.Drawing.Color GetColorFromHex(int red,int green,int blue)
        {
            return System.Drawing.Color.FromArgb(red, green, blue>255?255:blue);
        }
        public bool ValidateRgb(string rgb)
        {
            string pattern = "[0-9]{1,3},[0-9]{1,3},[0-9]{1,3}$";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
            return regex.IsMatch(rgb);
        }
        public void GetCurrentUserOption(out string curveRgb,out string limitRgb,out string rangeRgb
                                        ,out bool isShowLimit,out bool isShowMark,out bool isFillRange,out string format)
        {
            if (_profile != null && _profile.ID != 0&&!string.IsNullOrEmpty(_profile.TempCurveRGB))
            {
                curveRgb = _profile.TempCurveRGB;
                limitRgb = _profile.AlarmLineRGB;
                rangeRgb = _profile.IdealRangeRGB;
                isShowLimit = _profile.IsShowAlarmLimit;
                isShowMark = _profile.IsShowMark;
                isFillRange = _profile.IsFillIdealRange;
                format = _profile.DateTimeFormator;
            }
            else
            {
                isShowLimit =isShowMark=isFillRange =true;
                curveRgb = "255,0,0";
                limitRgb = "51,153,255";
                rangeRgb = "0,255,255";
                format = "yyyy/MM/dd HH:mm:ss";
            }
        }
        public void SaveTheGraphOption( string curveRgb, string limitRgb, string rangeRgb
                                        , bool isShowLimit, bool isShowMark, bool isFillRange, string format, bool isConfigTakeEffectNextTime)
        {
            UserProfileBLL _bll=new UserProfileBLL ();
            IList<UserProfile> allUserProfiles = _bll.GetAllUserProfiles();
            bool result = false;
            if (allUserProfiles != null && allUserProfiles.Count > 0)
            {
                foreach (var item in allUserProfiles)
                {
                    if (item != null && item.ID != 0)
                    {
                        if (CheckRGBFormat(curveRgb))
                            item.TempCurveRGB = curveRgb;
                        if (CheckRGBFormat(limitRgb))
                            item.AlarmLineRGB = limitRgb;
                        if (CheckRGBFormat(rangeRgb))
                            item.IdealRangeRGB = rangeRgb;
                        item.IsShowAlarmLimit = isShowLimit;
                        item.IsShowMark = isShowMark;
                        item.IsFillIdealRange = isFillRange;
                        item.DateTimeFormator = format;
                        result = _bll.UpdateProfile(item);
                    }
                    if (!result)
                    {
                        break;
                    }
                }
            }
            else
            {
                UserProfile userProfile = new UserProfile();
                userProfile.ID = _bll.GetProfilePKValue() + 1;
                userProfile.UserName = Common.User.UserName == null ? "" : Common.User.UserName;
                if (CheckRGBFormat(curveRgb))
                    userProfile.TempCurveRGB = curveRgb;
                if (CheckRGBFormat(limitRgb))
                    userProfile.AlarmLineRGB = limitRgb;
                if (CheckRGBFormat(rangeRgb))
                    userProfile.IdealRangeRGB = rangeRgb;
                userProfile.IsShowAlarmLimit = isShowLimit;
                userProfile.IsShowMark = isShowMark;
                userProfile.IsFillIdealRange = isFillRange;
                userProfile.DateTimeFormator = format;
                userProfile.Remark = DateTime.Now.ToString();

                userProfile.ContactInfo = "";
                userProfile.Logo = ShineTech.TempCentre.Platform.Utils.CopyToBinary(Properties.Resources.tempsen);
                userProfile.DefaultPath = "";
                userProfile.ReportTitle = "";

                userProfile.IsGlobal = (int)GlobalType.None;
                userProfile.IsShowHeader = false;
                userProfile.TempUnit = "C";
                result = _bll.InsertProfile(userProfile);
            }
           
            
            if (result)
            {
                if (isConfigTakeEffectNextTime)
                {
                    Platform.Utils.ShowMessageBox(Platform.Messages.B48, Platform.Messages.TitleNotification, MessageBoxButtons.OK);
                }
                else
                {
                    Platform.Utils.ShowMessageBox(Platform.Messages.B47, Platform.Messages.TitleNotification, MessageBoxButtons.OK);
                }
                Common.GlobalProfile = null;
            }
            else
                Platform.Utils.ShowMessageBox(Platform.Messages.B49, Platform.Messages.TitleNotification, MessageBoxButtons.OK);
        }
        private bool CheckRGBFormat(string rgb)
        {
            bool result = false;
            List<int> list= rgb.Split(new char[] { ',' }).Select(p=>Convert.ToInt32(p)).Where(p=>Convert.ToInt32(p)>255).ToList();
            if (list.Count > 0)
                result = false;
            else
                result = true;
            return result;
        }
    }
}
