using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.DAL;
using System.Text.RegularExpressions;

namespace ShineTech.TempCentre.BusinessFacade
{
    public partial class UserPolicy : UserControl
    {
        public UserPolicy()
        {
            InitializeComponent();
            InitPolicy();
            this.Load += new EventHandler(InitLoad);
            this.mtbPwdSize.Leave += new EventHandler(CheckContent);
            this.mtbPwdSize.TextChanged += new EventHandler(GetTextChange);
            this.mtbInactivity.TextChanged += new EventHandler(GetTextChange);
            this.mtbLocked.TextChanged += new EventHandler(GetTextChange);
            this.mtbPwdExpired.TextChanged += new EventHandler(GetTextChange);
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
        private string error=string.Empty;

        public string Error
        {
            get { return error; }
            set { error = value; }
        }
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
            string pwdsize = tips.GetToolTip(pbPwdSize);
            string pwdexpire = tips.GetToolTip(pbPwdExpired);
            string inactive = tips.GetToolTip(pbInactivity);
            string locks=tips.GetToolTip(pbLocked);
            if (pwdsize != string.Empty || pwdexpire != string.Empty || inactive != string.Empty || locks != string.Empty)
            {
                if(pwdsize!=string.Empty)
                    error = pwdsize;
            }
            else
                error = string.Empty;
            _MinPwdSize = mtbPwdSize.Text==string.Empty ? 0 : Convert.ToInt32(this.mtbPwdSize.Text);
            _LockedTimes = mtbLocked.Text == string.Empty ? 0 : Convert.ToInt32(mtbLocked.Text);
            _PwdExpiredDay = mtbPwdExpired.Text == string.Empty ? 0 : Convert.ToInt32(this.mtbPwdExpired.Text);
            _InactivityTime = mtbInactivity.Text == string.Empty ? 0 : Convert.ToInt32(this.mtbInactivity.Text); 
        }
        private void CheckContent(object sender, EventArgs args)
        {
            MaskedTextBox mtb = sender as MaskedTextBox;
            string t = mtb.Text.Trim();
            switch (mtb.Name)
            {
                case "mtbPwdSize":
                    int result;
                    if (int.TryParse(t, out result))
                    {
                        if (result < 3 || result > 12)
                        {
                            pbPwdSize.Visible = true;
                            this.tips.SetToolTip(pbPwdSize, Platform.Messages.PasswordLength);
                        }
                        else
                        {
                            pbPwdSize.Visible = false;
                            this.tips.SetToolTip(pbPwdSize, "");
                        }
                    }
                    else
                    {
                        pbPwdSize.Visible = true;
                        this.tips.SetToolTip(pbPwdSize, Platform.Messages.Characters);
                    }
                    break;
                case "mtbPwdExpired":
                    if (int.TryParse(t, out result))
                    {
                        pbPwdExpired.Visible = false;
                        this.tips.SetToolTip(pbPwdExpired, "");
                    }
                    else
                    {
                        pbPwdExpired.Visible = true;
                        this.tips.SetToolTip(pbPwdExpired, Platform.Messages.Characters);
                    }
                    break;
                case "mtbInactivity":
                    if (int.TryParse(t, out result))
                    {
                        pbInactivity.Visible = false;
                        this.tips.SetToolTip(pbInactivity, "");
                    }
                    else
                    {
                        pbPwdExpired.Visible = true;
                        this.tips.SetToolTip(pbInactivity, Platform.Messages.Characters);
                    }
                    break;
                case "mtbLocked":
                    if (int.TryParse(t, out result))
                    {
                        pbLocked.Visible = false;
                        this.tips.SetToolTip(pbLocked, "");
                    }
                    else
                    {
                        pbLocked.Visible = true;
                        this.tips.SetToolTip(pbLocked, Platform.Messages.Characters);
                    }
                    break;
            }
            
        }
        private void TextChange(object sender,EventArgs args)
        {
            MaskedTextBox mtb = sender as MaskedTextBox;
            mtb.Mask = "";
            for (int i = 0; i < mtb.Text.Length; i++)
                mtb.Mask += "9";
        }
        private void InitLoad(object sender, EventArgs args)
        {
            this.mtbPwdSize.Focus();
        }

        public void GetTextChange(object sender, EventArgs e)
        {
            MaskedTextBox tb = sender as MaskedTextBox;
            if (tb != null)
            {
                string pattern = "^[0-9]?[0-9]?$";
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
                string text = string.Empty;
                switch (tb.Name)
                {
                    case "mtbPwdSize":
                        text = _MinPwdSize.ToString();
                        break;
                    case "mtbPwdExpired":
                        text = _PwdExpiredDay.ToString();
                        break;
                    case "mtbInactivity":
                        text = _InactivityTime.ToString();
                        break;
                    case "mtbLocked":
                        text = _LockedTimes.ToString();
                        break;
                }
                if (regex.IsMatch(tb.Text))
                {
                    text = tb.Text;

                    if (string.IsNullOrEmpty(text))
                    {
                        tb.Text = text = "0";
                    }
                    tb.Text = Convert.ToInt32(text).ToString();
                    switch (tb.Name)
                    {
                        case "mtbPwdSize":
                            _MinPwdSize = Convert.ToInt32(text);
                            break;
                        case "mtbPwdExpired":
                            _PwdExpiredDay = Convert.ToInt32(text);
                            break;
                        case "mtbInactivity":
                            _InactivityTime = Convert.ToInt32(text);
                            break;
                        case "mtbLocked":
                            _LockedTimes = Convert.ToInt32(text);
                            break;
                    }
                }
                else
                {
                    tb.Text = text;
                    tb.SelectionStart = tb.TextLength;
                }
            }
        }
        #endregion
    }
}
