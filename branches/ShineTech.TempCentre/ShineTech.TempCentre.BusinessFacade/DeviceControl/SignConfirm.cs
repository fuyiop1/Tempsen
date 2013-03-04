using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.DAL;
using ShineTech.TempCentre.Platform;
using ShineTech.TempCentre.Versions;
using ShineTech.TempCentre.BusinessFacade.ViewModel;

namespace ShineTech.TempCentre.BusinessFacade
{
    public partial class SignConfirm : Form
    {
        private UserInfoBLL _userBll=new UserInfoBLL ();
        private MeaningsBLL _relation = new MeaningsBLL();
        private DigitalSignatureBLL _digital = new DigitalSignatureBLL();
        private string username = string.Empty;
        private string fullname = string.Empty;
        private OperationLogBLL logBll = new OperationLogBLL();
        private string sn, tn;

        private int _currentShowingToolTipItemIndex = -1;

        public SignConfirm()
        {
            InitializeComponent();
            InitEvents();
        }
        public SignConfirm(string sn, string tn) :this()
        {
            this.sn = sn;
            this.tn = tn;
        }
        private void InitEvents()
        {
            this.btnConfirm.Click += new EventHandler((sender, args) =>
            {

                if (Confirm())
                {
                    if (this.IsAuthorized(RightsText.SignRecords))
                    {
                        SetSignFormSize();
                        GetMeaning();
                        this.AcceptButton = this.btnSign;
                    }
                    else
                    {
                        Utils.ShowMessageBox(Messages.NoSignRight, Messages.TitleError);
                    }
                }
            });
            this.btnSign.Click += new EventHandler((sender, args) =>
            {
                this.SaveTheSignatureToDatabase();
            });
            this.clbMeanings.ItemCheck += new ItemCheckEventHandler((sender, e) =>
            {
                SelectedMeanChanged(e.Index);
            });
            this.clbMeanings.MouseMove += new MouseEventHandler(ShowMeaningTips);
        }
        public  bool IsAuthorized(string right)
        {
            if (Common.Versions == SoftwareVersions.S)
            {
                if (right == RightsText.ViewAuditTrail || right == RightsText.SignRecords)
                    return false;
                else
                    return true;
            }
            else
            {
                UserRightBLL rightbll = new UserRightBLL();
                List<DAL.UserRight> Userright= rightbll.GetUserRightByUserName(tbAccount.Text.Trim().ToLower());
                return Userright.Select(p => p.Right).Contains(right);
            }
            //return false;
        }
        private void SetSignFormSize()
        {

            if (gbSign.Visible == false)
            {
                this.Size = new Size(this.Width, this.Height + gbSign.Height + 20);
                gbSign.Location = new Point(gbSign.Location.X, this.btnConfirm.Location.Y + btnConfirm.Height+10);
                gbSign.Show();
            }
        }
        private bool Confirm()
        {
            try
            {
                if (Common.TextBoxChecked(tbAccount) && Common.TextBoxChecked(tbPwd))
                {
                    UserInfo user = _userBll.GetUserInfoByUsername(tbAccount.Text.Trim());
                    if (user != null && user.Userid != 0)
                    {
                        int day = (DateTime.Now.Date - user.LastPwdChangedTime.Date).Days;
                        if (day < Common.Policy.PwdExpiredDay || Common.Policy.PwdExpiredDay == 0)
                        {
                            if (user.Pwd == tbPwd.Text.Trim() && user.Locked == 0 && user.Disabled == 0)
                            {
                                username = user.UserName;
                                fullname = user.FullName;
                                return true;
                            }
                            if (user.Locked == 1)
                            {
                                Utils.ShowMessageBox(Messages.UserLocked, Messages.TitleError);
                                return false;
                            }
                        }
                        else
                        {
                            Utils.ShowMessageBox(Messages.PasswordExpired, Messages.TitleError);
                            return false;
                        }
                    }
                    Utils.ShowMessageBox(Messages.WrongUserNameOrPassword, Messages.TitleError);
                    return false;
                }
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        private void GetMeaning()
        {
            string username = tbAccount.Text.Trim().ToLower();
            List<UserMeanRelation> list= _relation.GetMeaningByUser(username);
            List<Meanings> allmean = _relation.GetAllMeans() as List< Meanings>;
            if (list != null && list.Count > 0)
            {
                var v = from p in list
                        join o in allmean on p.MeaningId equals o.Id
                        select new
                        {
                            p.Username,
                            o.Id,
                            o.Desc
                        };
                var dataSource = new List<MeaningViewModel>();
                foreach (var item in v)
                {
                    Meanings meaning = new Meanings() { Id = item.Id, Desc = item.Desc };
                    dataSource.Add(new MeaningViewModel(meaning, clbMeanings.Font, clbMeanings.ClientSize.Width - 10));
                }

                if (v != null && v.Count() > 0)
                {
                    clbMeanings.DataSource = dataSource;
                    clbMeanings.DisplayMember = "DisplayDesc";
                    clbMeanings.ValueMember = "Id";
                }
                else
                {
                    Utils.ShowMessageBox(Messages.AssignMeaningFirst, Messages.TitleWarning);
                }
            }
            else
            {
                Utils.ShowMessageBox(Messages.AssignMeaningFirst, Messages.TitleWarning);
            }
        }
        private void SelectedMeanChanged(int index)
        {
            if (clbMeanings.CheckedItems.Count > 0)
            {
                for (int i = 0; i < clbMeanings.Items.Count; i++)
                    if (i != index&&clbMeanings.GetItemChecked(i))
                        clbMeanings.SetItemChecked(i, false);
            }
        }
        /// <summary>
        /// 保存签名记录
        /// </summary>
        private void SaveTheSignatureToDatabase()
        {
            if (clbMeanings.CheckedItems.Count <= 0)
            {
                if(clbMeanings.Items.Count<=0)
                    Utils.ShowMessageBox(Messages.NoSignMeanings, Messages.TitleError);
                else
                    Utils.ShowMessageBox(Messages.NoSignMeanSelected, Messages.TitleError);
                return;
            }
            string selectedItem = (clbMeanings.SelectedItem).GetType().GetProperty("Desc").GetValue(clbMeanings.SelectedItem, null).ToString();
            if (!string.IsNullOrEmpty(selectedItem))
            {
                DigitalSignature signature = new DigitalSignature();
                int id = _digital.GetDigitalSignaturePKValue();
                signature.ID = id + 1;
                signature.UserName = username;
                signature.MeaningDesc = selectedItem;
                signature.Remark = DateTime.UtcNow.ToString();
                signature.SignTime = DateTime.UtcNow;
                signature.SN = ObjectManage.Tag.SerialNumber==null ? "" : this.sn;
                signature.TN = ObjectManage.Tag.TripNumber==null ? "" : this.tn;
                signature.FullName = fullname;
                //保存签名记录
                if (
                _digital.InsertDigitalSignature(signature))
                {
                    //记录成功的日志
                    if (Common.User.UserName != Common.SUPERUSER)
                    {
                        logBll.InsertLog(() =>
                        {
                            Dictionary<string, object> dic = new Dictionary<string, object>();
                            dic.Add("OperateTime", DateTime.UtcNow);
                            dic.Add("Action", LogAction.Signrecord);
                            dic.Add("UserName", username);
                            dic.Add("FullName", fullname);
                            dic.Add("Detail", selectedItem + ": " + signature.SN + "_" + signature.TN);
                            dic.Add("LogType", LogAction.AnalysisAuditTrail);
                            return dic;
                        });
                    }
                }
                this.DialogResult = DialogResult.OK;
            }

        }
        private void ShowMeaningTips(object sender, MouseEventArgs e)
        {
            int index = this.clbMeanings.IndexFromPoint(e.X, e.Y);
            if (index == -1)
            {
                _currentShowingToolTipItemIndex = -1;
                this.meaningTip.SetToolTip(clbMeanings, string.Empty);
            }
            else
            {
                if (index != _currentShowingToolTipItemIndex)
                {
                    _currentShowingToolTipItemIndex = index;
                    string text = clbMeanings.Items[index].GetType().GetProperty("Desc").GetValue(clbMeanings.Items[index], null).ToString();
                    this.meaningTip.SetToolTip(clbMeanings, text);
                }
            }
        }
    }
}
