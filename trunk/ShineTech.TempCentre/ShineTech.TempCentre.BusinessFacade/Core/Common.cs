using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using ShineTech.TempCentre.DAL;
namespace ShineTech.TempCentre.BusinessFacade
{
    public class Common
    {
        private static UserInfo user;
        private static Policy policy;

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
                        policy.InactivityTime = 10;
                        policy.LockedTimes = 5;
                        policy.MinPwdSize = 6;
                        policy.ProfileFolder = "";
                        policy.PwdExpiredDay = 30;
                        policy.Remark = DateTime.Now.ToString();
                        processor.Insert<Policy>(policy, null);
                    }
                }
                return Common.policy; 
            }
            set { Common.policy = value; }
        }
        
        /// <summary>
        /// 用户信息
        /// </summary>
        public static UserInfo User
        {
            get { return user; }
            set { user = value; }
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
        
    }
}
