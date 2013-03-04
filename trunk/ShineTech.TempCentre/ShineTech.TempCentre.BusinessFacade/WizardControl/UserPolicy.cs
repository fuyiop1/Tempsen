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
    public partial class UserPolicy : UserControl
    {
        public UserPolicy()
        {
            InitializeComponent();
            InitPolicy();
            this.Load += new EventHandler(delegate(object sender, EventArgs args) { this.mtbPwdSize.Focus(); });
            
        }
        #region/*自定义属性*/
        private int _MinPwdSize;

        /// <summary>
        /// 最小密码长度
        /// </summary>
        public int MinPwdSize
        {
            get { return _MinPwdSize; }
            
        }
        private int _LockedTimes;

        /// <summary>
        /// 锁定次数
        /// </summary>
        public int LockedTimes
        {
            get { return _LockedTimes;} 
        }
        private int _PwdExpiredDay;

        /// <summary>
        /// 密码过去天数
        /// </summary>
        public int PwdExpiredDay
        {
            get { return _PwdExpiredDay; }
        }
        private int _InactivityTime;

        /// <summary>
        /// 失效分钟数
        /// </summary>
        public int InactivityTime
        {
            get { return _InactivityTime; }
        }
        #endregion
        #region methods
        private void InitPolicy()
        {
            if (Common.Policy != null)
            {
                this.mtbInactivity.Text = Common.Policy.InactivityTime.ToString();
                this.mtbPwdSize.Text = Common.Policy.MinPwdSize.ToString();
                this.mtbPwdExpired.Text = Common.Policy.PwdExpiredDay.ToString();
                this.mtbLocked.Text = Common.Policy.LockedTimes.ToString();
            }
        }
        public void SetValue()
        {
            _MinPwdSize = mtbPwdSize.Text==string.Empty ? 0 : Convert.ToInt32(this.mtbPwdSize.Text);
            _LockedTimes = mtbLocked.Text == string.Empty ? 0 : Convert.ToInt32(mtbLocked.Text);
            _PwdExpiredDay = mtbPwdExpired.Text == string.Empty ? 0 : Convert.ToInt32(this.mtbPwdExpired.Text);
            _InactivityTime = mtbInactivity.Text == string.Empty ? 0 : Convert.ToInt32(this.mtbInactivity.Text); 
        }
        #endregion
    }
}
