using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShineTech.TempCentre.DAL;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
using ShineTech.TempCentre.BusinessFacade.DeviceControl;
using ShineTech.TempCentre.Platform;
using System.IO;
using ShineTech.TempCentre.BusinessFacade.ViewModel;
namespace ShineTech.TempCentre.BusinessFacade
{
    public class AdministrationUI
    {
        #region controls
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tp1;
        private System.Windows.Forms.TabPage tp2;
        private System.Windows.Forms.TabPage tp3;
        private System.Windows.Forms.TabPage tp4;
        private System.Windows.Forms.TabPage tp5;
        private System.Windows.Forms.DataGridView dgvUser;
        private System.Windows.Forms.Button btnDisUser;
        private System.Windows.Forms.Button btnEditUser;
        private System.Windows.Forms.Button btnAddUser;
        private System.Windows.Forms.Panel pnPolicy;
        private DataGridView dgvUser1;
        private System.Windows.Forms.FlowLayoutPanel layOutPn;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.ListBox lbAssigned;
        private System.Windows.Forms.ListBox lbAvailbale;
        private System.Windows.Forms.DataGridView dgvUser2;
        private Button btnAddMean;
        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.TextBox tbContactInfo;
        private System.Windows.Forms.TextBox tbReportTitle;
        private System.Windows.Forms.TextBox tbDefaultPath;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnBrowse;

        private System.Windows.Forms.CheckBox cbIsShowTitle;
        private System.Windows.Forms.CheckBox cbIsGlobal;
        private System.Windows.Forms.RadioButton rbF;
        private System.Windows.Forms.RadioButton rbC;

        private Button btnUnlockUser;

        private MeanCheckListBox clbMeans;
        private Button btnEditMean;
        private Button btnDeleteMean;
        #endregion
        private Form form;
        private UserInfoBLL _userBll;
        private PolicyBLL _policyBll;
        private MeaningsBLL _meanBll;
        private UserRightBLL _rightBll;
        private UserProfileBLL _profileBll;
        private UserPolicy _createPolicy;
        //private UserMeaning _createMean;

        UserProfile profile;
        private GlobalEntity globalProfile;
        private GlobalType shareType;
        private IList<UserInfo> allUsers;
        private IList<Meanings> allMeanings;
        private IDictionary<string, IList<int>> currentRelationOfUserAndMeaning = new Dictionary<string, IList<int>>();
        private Dictionary<string, List<string>> currentRelationOfUserRights;
        private Dictionary<string, bool> UserRightsIsChange;
        private event EventHandler PolicyEvent;//policy next事件
        private event EventHandler MeanEvent;

        private string originalTitleString;

        private void ConstructForms(Form form)
        {
            this.form = form;
            dgvUser = form.Controls.Find("dgvUser", true)[0] as DataGridView;
            btnDisUser = form.Controls.Find("btnDisUser", true)[0] as Button;
            btnEditUser = form.Controls.Find("btnEditUser", true)[0] as Button;
            btnAddUser = form.Controls.Find("btnAddUser", true)[0] as Button;
            tabControl1 = form.Controls.Find("tabControl1", true)[0] as TabControl;
            tp1 = form.Controls.Find("tp1", true)[0] as TabPage;
            tp2 = form.Controls.Find("tp2", true)[0] as TabPage;
            tp3 = form.Controls.Find("tp3", true)[0] as TabPage;
            tp4 = form.Controls.Find("tp4", true)[0] as TabPage;
            tp5 = form.Controls.Find("tp5", true)[0] as TabPage;
            pnPolicy = form.Controls.Find("pnPolicy", true)[0] as Panel;
            dgvUser1 = form.Controls.Find("dgvUser1", true)[0] as DataGridView;
            layOutPn = form.Controls.Find("layOutPn", true)[0] as FlowLayoutPanel;
            btnRight = form.Controls.Find("btnRight", true)[0] as Button;
            btnLeft = form.Controls.Find("btnLeft", true)[0] as Button;
            lbAssigned = form.Controls.Find("lbAssigned", true)[0] as ListBox;
            lbAvailbale = form.Controls.Find("lbAvailbale", true)[0] as ListBox;
            dgvUser2 = form.Controls.Find("dgvUser2", true)[0] as DataGridView;
            btnAddMean = form.Controls.Find("btnAddMean", true)[0] as Button;
            btnOpen = form.Controls.Find("btnOpen", true)[0] as Button;
            pbLogo = form.Controls.Find("pbLogo", true)[0] as PictureBox;
            tbContactInfo = form.Controls.Find("tbContactInfo", true)[0] as TextBox;
            tbReportTitle = form.Controls.Find("tbReportTitle", true)[0] as TextBox;
            tbDefaultPath = form.Controls.Find("tbDefaultPath", true)[0] as TextBox;
            btnBrowse = form.Controls.Find("btnBrowse", true)[0] as Button;
            btnApply = form.Controls.Find("btnApply", true)[0] as Button;
            btnCancel = form.Controls.Find("btnCancel", true)[0] as Button;
            cbIsShowTitle = form.Controls.Find("cbIsShowTitle", true)[0] as CheckBox;
            cbIsGlobal = form.Controls.Find("cbIsGlobal", true)[0] as CheckBox;
            rbF = form.Controls.Find("rbF", true)[0] as RadioButton;
            rbC = form.Controls.Find("rbC", true)[0] as RadioButton;
            btnEditMean = form.Controls.Find("btnEditMean", true)[0] as Button;
            btnDeleteMean = form.Controls.Find("btnDeleteMean", true)[0] as Button;
            btnUnlockUser = form.Controls.Find("btnUnlockUser", true)[0] as Button;
        }

        public AdministrationUI(Form form)
        {
            _userBll = new UserInfoBLL();
            _policyBll = new PolicyBLL();
            _meanBll = new MeaningsBLL();
            _rightBll = new UserRightBLL();
            _profileBll = new UserProfileBLL();
            this.ConstructForms(form);
            InitAdminGroup();
            this.form.Text = Common.FormTitle;
        }

        private void InitAdminGroup()
        {
            this.InitUsers();
            this.InitProfile();
            if (Common.User.RoleId != 1)
            {
                this.tabControl1.Controls.Remove(tp2);
                this.tabControl1.Controls.Remove(tp3);
                this.tabControl1.Controls.Remove(tp5);
                this.btnDisUser.Visible = false;
                this.btnAddUser.Visible = false;
                this.btnEditUser.Location = this.btnAddUser.Location;
                this.btnUnlockUser.Visible = false;
                this.cbIsGlobal.Visible = false;
                allUsers = _userBll.GetUserList();
            }
            else
            {
                this.InitPolicy();
                this.InitMeaning();
                this.InitRight();
            }
            this.InitEvents();
        }
        public void InitUsers()
        {
            dgvUser.CellFormatting += new DataGridViewCellFormattingEventHandler(dgvUser_CellFormatting);
            dgvUser.SelectionChanged += new EventHandler(dgvUser_SelectionChanged);

            IList<UserInfo> userList = _userBll.GetUserList();
            GenerateColumns(dgvUser);
            dgvUser.DataSource = userList;
            userList = ResortUserListWithCurrentUser(userList);
            DataTable sortedTable = new DataTable();
            if (userList != null)
            {
                this.dgvUser.DataSource = userList;
                if (Common.User.RoleId == 1)
                {
                    GenerateColumns(dgvUser1);
                    GenerateColumns(dgvUser2);
                    dgvUser1.DataSource = userList;
                    dgvUser2.DataSource = userList;
                }
            }
            for (int i = 0; i < dgvUser.Columns.Count; i++)
            {
                this.dgvUser.Columns[i].Width = this.dgvUser.Width / 5;
                if (Common.User.RoleId == 1)
                {
                    this.dgvUser1.Columns[i].Width = this.dgvUser1.Width / 5;
                    this.dgvUser2.Columns[i].Width = this.dgvUser2.Width / 5;
                }
            }

        }

        private static void GenerateColumns(DataGridView dgv)
        {
            if (dgv.Columns.Count <= 0)
            {
                dgv.AutoGenerateColumns = false;
                dgv.Columns.Clear();
                dgv.Columns.Add(new DataGridViewColumn() { Name = "User Name", DataPropertyName = "UserName", CellTemplate = new DataGridViewTextBoxCell() });
                dgv.Columns.Add(new DataGridViewColumn() { Name = "Full Name", DataPropertyName = "FullName", CellTemplate = new DataGridViewTextBoxCell() });
                dgv.Columns.Add(new DataGridViewColumn() { Name = "Role", DataPropertyName = "Description", CellTemplate = new DataGridViewTextBoxCell() });
                dgv.Columns.Add(new DataGridViewColumn() { Name = "Group", DataPropertyName = "Group", CellTemplate = new DataGridViewTextBoxCell() });
                dgv.Columns.Add(new DataGridViewColumn() { Name = "Status", DataPropertyName = "Status", CellTemplate = new DataGridViewTextBoxCell() });
            }
        }

        private IList<UserInfo> ResortUserListWithCurrentUser(IList<UserInfo> userList)
        {
            IList<UserInfo> userSortedList = new List<UserInfo>();
            if (userList != null)
            {
                foreach (var userInfo in userList)
                {
                    if (userInfo.UserName == Common.User.UserName)
                    {
                        userSortedList.Add(userInfo);
                        break;
                    }
                }
                foreach (var userInfo in userList)
                {
                    if (userInfo.UserName != Common.User.UserName)
                    {
                        userSortedList.Add(userInfo);
                    }
                }
            }
            return userSortedList;
        }

        private void tp_Enter(object sender, EventArgs e)
        {
            if (sender == this.tp1)
            {
                if (Common.User.RoleId == 1)
                {
                    this.btnAddUser.Visible = true;
                    this.btnDisUser.Visible = false;
                    this.btnUnlockUser.Visible = true;
                }
                this.btnEditUser.Visible = true;
                this.btnApply.Visible = false;
                this.btnCancel.Visible = true;
            }
            else
            {
                this.btnAddUser.Visible = false;
                this.btnEditUser.Visible = false;
                this.btnDisUser.Visible = false;
                this.btnUnlockUser.Visible = false;
                this.btnApply.Visible = true;
                this.btnCancel.Visible = true;
            }
        }

        private void dgvUser_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvUser.Rows.Count > 0)
            {
                if (Common.User.RoleId != 1)
                {
                    var userInfo = dgvUser.Rows[e.RowIndex].DataBoundItem as UserInfo;
                    if (userInfo != null && !Common.User.UserName.Equals(userInfo.UserName, StringComparison.Ordinal))
                    {
                        e.CellStyle.ForeColor = Color.Gray;
                        e.CellStyle.BackColor = Color.White;
                        e.CellStyle.SelectionBackColor = Color.White;
                        e.CellStyle.SelectionForeColor = Color.Gray;
                    }
                }

            }
        }

        private void dgvUser_SelectionChanged(object sender, EventArgs e)
        {
            if (Common.User.RoleId != 1 && dgvUser.CurrentRow != null)
            {
                int currentRowIndex = dgvUser.CurrentRow.Index;
                int targetRowIndex = 0;
                foreach (DataGridViewRow row in dgvUser.Rows)
                {
                    var userInfo = row.DataBoundItem as UserInfo;
                    if (userInfo != null && userInfo.UserName == Common.User.UserName)
                    {
                        targetRowIndex = row.Index;
                        break;
                    }
                }

                if (currentRowIndex != targetRowIndex) // some condition meaning not editable
                {
                    dgvUser.CurrentCell = dgvUser.Rows[targetRowIndex].Cells[0];
                }
            }
            else
            {
                if (this.dgvUser.SelectedRows.Count > 0)
                {
                    string username = this.dgvUser.SelectedRows[0].Cells["User Name"].Value.ToString();
                    UserInfo userInfo = _userBll.GetUserInfoByUsername(username);
                    if (userInfo != null)
                    {
                        if (userInfo.Locked == 1)
                        {
                            btnUnlockUser.Text = "Unlock User";
                        }
                        else
                        {
                            btnUnlockUser.Text = "Lock User";
                        }
                        if (username == Common.User.UserName)
                        {
                            btnUnlockUser.Enabled = false;
                        }
                        else
                        {
                            btnUnlockUser.Enabled = true;
                        }
                        if (userInfo.UserName == Common.User.UserName || userInfo.Disabled == 1)
                        {
                            this.btnDisUser.Enabled = false;
                        }
                        else
                        {
                            this.btnDisUser.Enabled = true;
                        }
                    }
                }
            }

        }

        private void dgvUser1_SelectionChanged(object sender, EventArgs e)
        {
            var userInfo = dgvUser1.CurrentRow.DataBoundItem as UserInfo;
            this.UpdateStatusForCheckBoxAfterSelectionChanged(userInfo);
        }

        private void dgvUsers_SelectedUserChanged(DataGridView dgv)
        {
            if (dgv == dgvUser && dgvUser.CurrentRow != null)
            {
                Common.CurrentSelectedUserOfDgv = dgvUser.CurrentRow.DataBoundItem as UserInfo;
            }
            else if (dgv == dgvUser1 && dgvUser1.CurrentRow != null)
            {
                Common.CurrentSelectedUserOfDgv1 = dgvUser1.CurrentRow.DataBoundItem as UserInfo;
            }
            else if (dgv == dgvUser2 && dgvUser2.CurrentRow != null)
            {
                Common.CurrentSelectedUserOfDgv2 = dgvUser2.CurrentRow.DataBoundItem as UserInfo;
            }
            else
            {
                // nothing to do
            }
        }

        private void UpdateStatusForCheckBoxAfterSelectionChanged(UserInfo userInfo)
        {
            if (this.clbMeans != null)
            {
                for (int i = 0; i < this.clbMeans.Items.Count; i++)
                {
                    var meanItem = this.clbMeans.Items[i] as MeaningViewModel;
                    if (meanItem != null)
                    {
                        this.clbMeans.SetItemChecked(i, currentRelationOfUserAndMeaning[userInfo.UserName].Contains(meanItem.Id));
                    }
                }
            }
        }



        private void SavePolicy()
        {
            bool isExpired = false;
            if (Common.Policy.PwdExpiredDay != _createPolicy.PwdExpiredDay)
            {
                isExpired = true;
            }
            else
            {
                isExpired = false;
            }
            Common.Policy.InactivityTime = _createPolicy.InactivityTime;
            Common.Policy.LockedTimes = _createPolicy.LockedTimes;
            Common.Policy.PwdExpiredDay = _createPolicy.PwdExpiredDay;
            Common.Policy.MinPwdSize = _createPolicy.MinPwdSize;
            Common.Policy.ProfileFolder = "";
            Common.Policy.Remark = DateTime.Now.ToString();
            if (_policyBll.InsertOrUpdate(Common.Policy, false))
            {
                _userBll.UpdateAllUserPwdExpire(DateTime.Now);
            }
        }
        public void InitPolicy()
        {

            if (_createPolicy == null)
            {
                _createPolicy = new UserPolicy();
                _createPolicy.Width = 635;
            }
            if (PolicyEvent == null)
                PolicyEvent += new EventHandler((sender, args) => _createPolicy.SetValue());
            this.pnPolicy.Controls.Add(_createPolicy);
        }
        public void InitMeaning()
        {
            allUsers = _userBll.GetUserList();
            allMeanings = _meanBll.GetAllMeans();
            UserInfo currentSelectedUser = allUsers.Where(u => u.UserName == Common.User.UserName).FirstOrDefault();
            Common.CurrentSelectedUserOfDgv = currentSelectedUser;
            Common.CurrentSelectedUserOfDgv1 = currentSelectedUser;
            Common.CurrentSelectedUserOfDgv2 = currentSelectedUser;
            if (allUsers != null && currentRelationOfUserAndMeaning != null)
            {
                foreach (var userItem in allUsers)
                {
                    if (!currentRelationOfUserAndMeaning.Keys.Contains(userItem.UserName))
                    {
                        currentRelationOfUserAndMeaning[userItem.UserName] = new List<int>();
                    }
                }
                IList<UserMeanRelation> relationFromDb = _meanBll.GetAllRelations();
                if (relationFromDb != null)
                {
                    foreach (var relationItem in relationFromDb)
                    {
                        UserInfo user = allUsers.Where(u => u.UserName == relationItem.Username).FirstOrDefault();
                        Meanings meaning = allMeanings.Where(m => m.Id == relationItem.MeaningId).FirstOrDefault();
                        if (meaning != null && user != null && !currentRelationOfUserAndMeaning[user.UserName].Contains(meaning.Id))
                        {
                            currentRelationOfUserAndMeaning[user.UserName].Add(meaning.Id);
                        }
                    }
                }
            }
            
            if (allMeanings != null)
            {
                this.layOutPn.Controls.Clear();
                this.clbMeans = new MeanCheckListBox(allMeanings, currentRelationOfUserAndMeaning, this.layOutPn.Width);
                this.clbMeans.Width = this.layOutPn.Width;
                this.clbMeans.Height = this.layOutPn.Height;
                this.clbMeans.Margin = new Padding(0);
                this.layOutPn.Controls.Add(this.clbMeans);
            }

        }




        public void InitRight()
        {
            /*查找所有用户的userright*/
            if (currentRelationOfUserRights == null)
            {
                currentRelationOfUserRights = new Dictionary<string, List<string>>();
                UserRightsIsChange = new Dictionary<string, bool>();
                allUsers.ToList().ForEach(p =>
                {
                    List<DAL.UserRight> ls = _rightBll.GetRightByUserName(p.UserName);
                    currentRelationOfUserRights.Add(p.UserName, ls.Select(v => v.Right).ToList());
                    UserRightsIsChange.Add(p.UserName, false);
                });
            }
            else
            {
                allUsers.ToList().ForEach(p =>
                {
                    if (!currentRelationOfUserRights.Keys.Contains(p.UserName))
                    {
                       List<DAL.UserRight> ls = _rightBll.GetRightByUserName(p.UserName);
                       currentRelationOfUserRights.Add(p.UserName, ls.Select(v => v.Right).ToList());
                       UserRightsIsChange.Add(p.UserName, false);

                    }
                });
            }
            //left
            this.lbAvailbale.Items.Clear();
            Rights r = Common.GetRightsList();
            if (r == null)
                r = Common.SetRightsList();
            r.right.ToList().ForEach(p => this.lbAvailbale.Items.Add(p));
            //right
            if (this.dgvUser2.SelectedRows.Count > 0)
            {
                string username = Convert.ToString(this.dgvUser2.SelectedRows[0].Cells["User Name"].Value);
                string group = Convert.ToString(this.dgvUser2.SelectedRows[0].Cells["Group"].Value);
                List<string> list = currentRelationOfUserRights[username];
                if (group == "User")
                    this.btnLeft.Enabled = this.btnRight.Enabled = true;
                else
                    this.btnLeft.Enabled = this.btnRight.Enabled = false;
                lbAssigned.Items.Clear();
                if (list != null && list.Count > 0)
                {
                    list.ToList().ForEach(p => lbAssigned.Items.Add(p));
                }
                else
                    this.btnLeft.Enabled = false;
            }
        }
        public void InitProfile()
        {
            string username = Common.User.UserName;
            if (!string.IsNullOrEmpty(username))
            {
                //profile = _profileBll.GetProfileByUserName(username);
                profile = Common.GetCurrentUserProfile();//20110825
                globalProfile = Common.GetGlobalSetting();
                if (profile != null)
                {
                    this.tbContactInfo.Text = profile.ContactInfo;
                    this.tbDefaultPath.Text = string.IsNullOrEmpty(profile.DefaultPath) ? Path.Combine(Application.StartupPath, "TempCentre Data") : profile.DefaultPath;
                    this.tbReportTitle.Text = profile.ReportTitle;
                    if (profile.Logo != null && profile.Logo.Length > 0)
                    {
                        this.pbLogo.Image = ShineTech.TempCentre.Platform.Utils.ReadSource(profile.Logo);

                        try
                        {
                            int finalWidth = this.pbLogo.Image.Width;
                            int finalHeight = this.pbLogo.Image.Height;
                            Size originalSize = new Size(313, 47);
                            if (finalWidth > originalSize.Width)
                            {
                                finalHeight = (int)(finalHeight * (originalSize.Width * 1.0 / finalWidth));
                                finalWidth = originalSize.Width;
                            }
                            if (finalHeight > originalSize.Height)
                            {
                                finalWidth = (int)(finalWidth * (originalSize.Height * 1.0 / finalHeight));
                                finalHeight = originalSize.Height;
                            }
                            pbLogo.Width = finalWidth;
                        }
                        catch (Exception)
                        {
                        }
                    }
                    if (globalProfile != null)
                    {
                        cbIsGlobal.Checked = Common.User.RoleId == 1 ? true : false;
                        pbLogo.Image = ShineTech.TempCentre.Platform.Utils.ReadSource(globalProfile.Logo);
                        tbContactInfo.Text = globalProfile.ContactInfo;
                        shareType = GlobalType.LogoAndConctact;
                    }
                    else
                    {
                        cbIsGlobal.Checked = false;
                        shareType = GlobalType.None;
                    }
                    this.rbF.Checked = profile.TempUnit == "F" ? true : false;
                    this.cbIsShowTitle.Checked = profile.IsShowHeader;
                    SetProfileEnable(shareType, Common.User.RoleId == 1 ? true : false);
                }
                this.tbContactInfoText = this.tbContactInfo.Text;
            }
        }
        private void InitEvents()
        {
            this.tp1.Enter += new EventHandler(tp_Enter);
            this.tp2.Enter += new EventHandler(tp_Enter);
            this.tp3.Enter += new EventHandler(tp_Enter);
            this.tp4.Enter += new EventHandler(tp_Enter);
            this.tp5.Enter += new EventHandler(tp_Enter);
            #region mean apply
            this.btnAddMean.Click += new EventHandler((a, b) =>
            {

                InputBoxDialog newMeanDialog = new InputBoxDialog(InputBoxTitle.AddMeaning, InputBoxTipMessage.AddMeaning, false);
                if (newMeanDialog.ShowDialog(this.form) == DialogResult.OK)
                {
                    Meanings meaning = new Meanings() { Id = this._meanBll.GetMeaningPKValue() + 1, Desc = newMeanDialog.InputBoxText.TrimEnd(), Remark = DateTime.Now.ToString() };
                    this._meanBll.InsertOrUpdateMeaning(meaning);
                    this.allMeanings.Add(meaning);
                    if (this.clbMeans != null)
                    {
                        this.clbMeans.AddMean(meaning);
                    }
                }

            });

            this.btnEditMean.Click += new EventHandler((send, args) =>
            {
                InputBoxDialog updateMeanDialog = new InputBoxDialog(InputBoxTitle.EditMeaning, InputBoxTipMessage.EditMeaning, false);
                int selectedMeanId = this.clbMeans.GetCurrentSelectedMean();
                Meanings selectedMean = null;
                if (allMeanings != null)
                {
                    selectedMean = allMeanings.Where<Meanings>(m => m.Id == selectedMeanId).FirstOrDefault();
                }
                int selectedMeanIndex = this.clbMeans.SelectedIndex;
                if (selectedMean != null)
                {
                    updateMeanDialog.InputBoxText = selectedMean.Desc;
                    if (updateMeanDialog.ShowDialog(this.form) == DialogResult.OK)
                    {
                        if (!updateMeanDialog.InputBoxText.TrimEnd().Equals(selectedMean.Desc, StringComparison.Ordinal))
                        {
                            selectedMean.Desc = updateMeanDialog.InputBoxText.TrimEnd();
                            selectedMean.Remark = DateTime.Now.ToString();
                            this._meanBll.InsertOrUpdateMeaning(selectedMean);
                            this.clbMeans.SetSelected(selectedMeanIndex, true);
                            this.clbMeans.Refresh();
                        }
                    }
                }

            });

            this.btnDeleteMean.Click += new EventHandler((send, args) =>
            {
                if (DialogResult.Yes == Utils.ShowMessageBox(Messages.DeleteMeaning, Messages.TitleWarning, MessageBoxButtons.YesNo))
                {
                    int selectedMeanId = this.clbMeans.GetCurrentSelectedMean();
                    Meanings selectedMean = null;
                    if (allMeanings != null)
                    {
                        selectedMean = allMeanings.Where<Meanings>(m => m.Id == selectedMeanId).FirstOrDefault();
                    }
                    if (selectedMean != null)
                    {
                        this._meanBll.DeleteMeaningAndRelation(selectedMean);
                        allMeanings.Remove(allMeanings.Where<Meanings>(m => m.Id == selectedMeanId).FirstOrDefault());
                        this.clbMeans.Items.Remove(selectedMean);
                        foreach (var item in currentRelationOfUserAndMeaning.Values)
                        {
                            item.Remove(selectedMeanId);
                        }
                        if (this.clbMeans.Items.Count > 0)
                        {
                            this.clbMeans.SetSelected(0, true);
                        }
                        this.clbMeans.Refresh();
                    }
                }
            });

            this.btnApply.Click += new EventHandler((a, b) =>
            {
                try
                {
                    if (Common.User.RoleId == 1)
                    {
                        PolicyEvent(form, null);
                        if (!string.IsNullOrEmpty(_createPolicy.Error))
                        {
                            Utils.ShowMessageBox(_createPolicy.Error, Messages.TitleError, MessageBoxButtons.OK);
                            return;
                        }
                        SavePolicy();
                        if (this.dgvUser2.SelectedRows.Count > 0)
                        {
                            //string currentRowUsername = Convert.ToString(this.dgvUser2.SelectedRows[0].Cells["User Name"].Value);
                            //this.UserRightApply(currentRowUsername);
                            UserRightApply();
                        }
                        this.meanApply();
                    }
                    this.UserProfileApply(Common.User.UserName);
                    Utils.ShowMessageBox(Messages.ApplySuccessfully, Messages.TitleNotification);
                }
                catch 
                {
                    Utils.ShowMessageBox(Messages.ApplyFailed, Messages.TitleError);
                }
            });

            this.btnCancel.Click += new EventHandler((sender, args) =>
            {
                this.form.Dispose();
            });

            #endregion
            //this.form.Load += new EventHandler((sender, args) => this.InitMeaning());
            //this.dgvUser1.SelectionChanged += new EventHandler((sender, args) => this.InitMeaning());
            this.dgvUser2.SelectionChanged += new EventHandler((sender, args) => this.InitRight());
            #region 用户失效
            this.btnDisUser.Click += new EventHandler(btnDisUser_Click);

            this.btnUnlockUser.Click += new EventHandler(btnUnlockUser_Click);
            #endregion
            #region 左右移动
            this.btnLeft.Click += new EventHandler(RemoveRights);
            this.btnRight.Click += new EventHandler(AssignRights);
            #endregion

            #region profile
            this.btnOpen.Click += new EventHandler((a, b) =>
            {
                OpenFileDialog file = new OpenFileDialog();
                file.Filter = "All Possible Files(.jpg .png .gif .bmp)|*.jpg;*.png;*.gif;*.bmp|JPEG Files(.jpg)|*.jpg|PNG Files(.png)|*.png|GIF Files(.gif)|*.gif|BMP Files(.bmp)|*.bmp";
                if (file.ShowDialog() == DialogResult.OK)
                {
                    string src = file.FileName.ToString();
                    FileInfo fileInfo = new FileInfo(src);
                    if (fileInfo != null)
                    {
                        if (fileInfo.Length > (2 * 1024 * 1024))
                        {
                            Utils.ShowMessageBox(Messages.TooLargeFile, Messages.TitleError);
                            return;
                        }
                    }
                    Image originalImage = null;
                    try
                    {
                        originalImage = Image.FromFile(src);
                        pbLogo.Image = originalImage;
                        int finalWidth = originalImage.Width;
                        int finalHeight = originalImage.Height;
                        Size originalSize = new Size(313, 47);
                        if (finalWidth > originalSize.Width)
                        {
                            finalHeight = (int)(finalHeight * (originalSize.Width * 1.0 / finalWidth));
                            finalWidth = originalSize.Width;
                        }
                        if (finalHeight > originalSize.Height)
                        {
                            finalWidth = (int)(finalWidth * (originalSize.Height * 1.0 / finalHeight));
                            finalHeight = originalSize.Height;
                        }
                        pbLogo.Width = finalWidth;
                    }
                    catch (Exception)
                    {
                    }
                }

            });
            this.btnBrowse.Click += new EventHandler(delegate(object sender, EventArgs args)
            {
                FolderBrowserDialog folder = new FolderBrowserDialog();
                folder.SelectedPath = this.tbDefaultPath.Text; 
                if (folder.ShowDialog() == DialogResult.OK)
                {
                    this.tbDefaultPath.Text = folder.SelectedPath;
                }
            });

            this.tbContactInfo.TextChanged += new EventHandler(tbContactInfo_TextChanged);
            this.tbReportTitle.TextChanged += new EventHandler(tbReportTitle_TextChanged);

            #endregion

            this.dgvUser.SelectionChanged += new EventHandler((sender, args) => this.dgvUsers_SelectedUserChanged(dgvUser));
            this.dgvUser1.SelectionChanged += new EventHandler((sender, args) => this.dgvUsers_SelectedUserChanged(dgvUser1));
            this.dgvUser2.SelectionChanged += new EventHandler((sender, args) => this.dgvUsers_SelectedUserChanged(dgvUser2));
            this.dgvUser1.SelectionChanged += new EventHandler(dgvUser1_SelectionChanged);


            this.initRightListBoxEvent();
        }

        private string tbContactInfoText;

        private void tbContactInfo_TextChanged(object sender, EventArgs e)
        {
            var tbContactInfo = sender as TextBox;
            if (tbContactInfo != null)
            {
                string textTrimEnd = tbContactInfo.Text.TrimEnd();
                string[] lines = textTrimEnd.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                int actualLineCount = 0;
                int maxRowCount = 5;
                foreach (var item in lines)
                {
                    int textBoxPadding = 9;
                    int actualLineHeight = (int)Math.Ceiling(TextRenderer.MeasureText(item, tbContactInfo.Font).Width * 1.0 / (tbContactInfo.ClientSize.Width - textBoxPadding));
                    actualLineCount += actualLineHeight == 0 ? 1 : actualLineHeight;
                }

                if (actualLineCount > maxRowCount)
                {
                    tbContactInfo.Text = this.tbContactInfoText;
                    tbContactInfo.SelectionStart = tbContactInfo.Text.Length;
                }
                else
                {
                    this.tbContactInfoText = textTrimEnd;
                    //tbContactInfo.SelectionStart = tbContactInfo.Text.Length;
                }
            }
        }

        private void tbReportTitle_TextChanged(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null)
            {
                string textTrimEnd = tb.Text.TrimEnd();
                using (Graphics g = tb.CreateGraphics())
                {
                    int actualWidth = (int)Math.Ceiling(g.MeasureString(textTrimEnd, tb.Font).Width);

                    if (actualWidth > tb.ClientSize.Width / 1.5)
                    {
                        tb.Text = this.originalTitleString;
                        tb.SelectionStart = tb.Text.Length;
                    }
                    else
                    {
                        this.originalTitleString = textTrimEnd;
                        //tb.SelectionStart = tb.Text.Length;
                    }
                }
            }
        }

        private void initRightListBoxEvent()
        {
            if (this.lbAvailbale != null)
            {
                this.lbAvailbale.DrawItem += new DrawItemEventHandler(listBox_DrawItem);
            }

            if (this.lbAssigned != null)
            {
                this.lbAssigned.DrawItem += new DrawItemEventHandler(listBox_DrawItem);
            }
        }

        private void listBox_DrawItem(object s, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            var sender = s as ListBox;
            if (sender != null)
            {
                e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
                e.Graphics.DrawString(sender.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds);
            }
        }


        #region Events of lock, disable User
        private void btnUnlockUser_Click(object sender, EventArgs e)
        {
            string username = this.dgvUser.SelectedRows[0].Cells["User Name"].Value.ToString();
            UserInfo userInfo = _userBll.GetUserInfoByUsername(username);
            if (userInfo != null)
            {
                if (btnUnlockUser.Text == "Unlock User")
                {
                    userInfo.Locked = 0;
                    new ActivityLogManager().AddUnlockUserLogEntry(Common.User.UserName, Common.User.FullName, username);
                }
                else
                {
                    if (Utils.ShowMessageBox(string.Format(Messages.B32, username), Messages.TitleNotification, MessageBoxButtons.YesNo) != DialogResult.Yes)
                    {
                        return;
                    }
                    userInfo.Locked = 1;
                    new ActivityLogManager().AddLockUserLogEntry(Common.User.UserName, Common.User.FullName, username);
                }
                _userBll.UdateUser(userInfo);
                this.InitUsers();
            }
        }

        private void btnDisUser_Click(object sender, EventArgs e)
        {
            if (this.dgvUser.SelectedRows.Count > 0)
            {
                string username = this.dgvUser.SelectedRows[0].Cells["User Name"].Value.ToString();

                if (!string.IsNullOrEmpty(username) && username.Equals(Common.User.UserName, StringComparison.Ordinal))
                {
                    Utils.ShowMessageBox(Messages.DisableYourself, Messages.TitleWarning, MessageBoxButtons.OK);
                }
                else
                {
                    UserInfo userInfo = _userBll.GetUserInfoByUsername(username);
                    if (userInfo != null)
                    {
                        bool targetStatus = Math.Abs(userInfo.Disabled - 1) == 0;
                        if (!targetStatus)
                        {
                            if (DialogResult.Yes == Utils.ShowMessageBox(string.Format(Messages.DisableUser, username)
                                                        , Messages.TitleWarning, MessageBoxButtons.YesNo))
                            {
                                _userBll.DisableUser(username, targetStatus);
                                if (Common.User.UserName != Common.SUPERUSER)
                                {
                                    new OperationLogBLL().InsertLog(() =>
                                    {
                                        Dictionary<string, object> d = new Dictionary<string, object>();
                                        d.Add("OperateTime", DateTime.UtcNow);
                                        d.Add("Action", LogAction.DisableUser);
                                        d.Add("UserName", Common.User.UserName);
                                        d.Add("FullName", Common.User.FullName);
                                        d.Add("Detail", username);
                                        d.Add("LogType", LogAction.SystemAuditTrail);
                                        return d;
                                    });
                                }
                            }
                        }
                        //else
                        //{
                        //    _userBll.DisableUser(username, targetStatus);
                        //}
                        this.InitUsers();
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// 移除控件
        /// </summary>
        /// <param name="m"></param>
    
        private bool UserRightApply(string username)
        {
            Dictionary<string, List<string>> dic = new Dictionary<string, List<string>>();
            List<string> list = new List<string>();
            if (lbAssigned.Items.Count > 0)
            {
                if (list == null)
                    list = new List<string>();
                lbAssigned.Items.Cast<string>().ToList().ForEach(p => list.Add(p));
            }
            dic.Add(username, list);
            AddAssignRightsLog(username, list);
            return _rightBll.SummitUserRight(dic, null);
        }
        private void AddAssignRightsLog(string username, List<string> list)
        {
            if (list == null || list.Count <= 0)
                return;
            string detail = username + ": ";
            list.ForEach(p =>
            {
                detail = detail + p + ",";
            });
            detail = detail.Substring(0, detail.Length - 1);
            if (Common.User.UserName != Common.SUPERUSER)
            {
                new OperationLogBLL().InsertLog(() =>
                {
                    Dictionary<string, object> d = new Dictionary<string, object>();
                    d.Add("OperateTime", DateTime.UtcNow);
                    d.Add("Action", LogAction.AssignRights);
                    d.Add("UserName", Common.User.UserName);
                    d.Add("FullName", Common.User.FullName);
                    d.Add("Detail", detail);
                    d.Add("LogType", LogAction.SystemAuditTrail);
                    return d;
                });
            }
        }
        private bool meanApply()
        {
            bool result = false;
            //try
            //{
            _meanBll.DeleteAllRelation();
            foreach (var item in currentRelationOfUserAndMeaning)
            {
                foreach (var meaning in item.Value)
                {
                    UserMeanRelation relation = new UserMeanRelation() { ID = _meanBll.GetRelationPKValue() + 1, Username = item.Key, MeaningId = meaning, Remark = DateTime.Now.ToString() };
                    _meanBll.InsertRelation(relation);
                }
            }
            result = true;
            //}
            //catch (Exception)
            //{
            //    result = false;
            //}
            return result;
        }
        private void UserRightApply()
        {
            foreach (var v in UserRightsIsChange)
            {
                if (v.Value == true)
                {
                    _rightBll.DeleteUserRightByUserName(v.Key);
                    currentRelationOfUserRights[v.Key].ForEach(p =>
                    {
                        DAL.UserRight right = new DAL.UserRight() { ID = _rightBll.GetUserRightPKValue() + 1, UserName = v.Key, Right = p, Remark = DateTime.Now.ToString() };
                        _rightBll.InsertUserRight(right);

                    });
                    //记录日志
                    AddAssignRightsLog(v.Key, currentRelationOfUserRights[v.Key].ToList());
                }
            }
        }
        private bool UserProfileApply(string username)
        {
            SaveCurrentProfile(username, shareType);
            SaveGlobalProfile(shareType);
            Common.GlobalProfile = null;
            Common.GlobalProfile = Common.GetGlobalUserProfile();
            return true;
        }
        private void SaveCurrentProfile(string username, GlobalType shareType)
        {
            UserProfile profileInDB = _profileBll.GetProfileByUserName(username);
            GetProfileFromScreen(profile, shareType);
            if (profileInDB.ID == 0)
            {
                profile.ID = _profileBll.GetProfilePKValue() + 1;
                profile.UserName = Common.User.UserName;
                _profileBll.InsertProfile(profile);
            }
            else
            {
                _profileBll.UpdateProfile(profile);
            }
        }
        private void GetProfileFromScreen(UserProfile profile, GlobalType shareType)
        {
            profile.Remark = DateTime.Now.ToString();
            profile.ContactInfo = this.tbContactInfo.Text;
            //float fixedHeight = 50;
            if (pbLogo.Image != null)
            {
                Image originalImage = pbLogo.Image;
                //int justifiedWidth = originalImage.Width;
                //int justifiedHeight = originalImage.Height;
                //if (originalImage.Height > fixedHeight)
                //{
                //    justifiedHeight = (int)fixedHeight;
                //    justifiedWidth = (int)(originalImage.Width * (fixedHeight / originalImage.Height));
                //}
                //Image fixHeightImage = new Bitmap(originalImage, justifiedWidth, justifiedHeight);
                profile.Logo = ShineTech.TempCentre.Platform.Utils.CopyToBinary(originalImage);
            }
            else
                profile.Logo = null;
            profile.DefaultPath = this.tbDefaultPath.Text;
            profile.ReportTitle = this.tbReportTitle.Text;
            if (cbIsGlobal.Checked)
                profile.IsGlobal = (int)GlobalType.LogoAndConctact;
            else
                profile.IsGlobal = (int)GlobalType.None;
            profile.IsShowHeader = cbIsShowTitle.Checked;
            profile.TempUnit = rbC.Checked ? "C" : "F";
        }
        private void SaveGlobalProfile(GlobalType shareType)
        {
            UserProfile profile;
            switch (shareType)
            {
                case GlobalType.LogoAndConctact:
                    if (globalProfile != null && Common.User.IsAdmin)
                    {
                        profile = _profileBll.GetProfileByPK(globalProfile.Id);
                        profile.Logo = ShineTech.TempCentre.Platform.Utils.CopyToBinary(pbLogo.Image);
                        profile.Remark = DateTime.Now.ToString();
                        profile.ContactInfo = tbContactInfo.Text;
                        if (cbIsGlobal.Checked)
                            profile.IsGlobal = (int)GlobalType.LogoAndConctact;
                        else
                            profile.IsGlobal = (int)GlobalType.None;
                        _profileBll.UpdateProfile(profile);
                    }
                    break;
            }
        }
        private void SetProfileEnable(GlobalType isGlobal, bool isAdmin)
        {
            if (!isAdmin)
            {
                switch (isGlobal)
                {
                    case GlobalType.None:
                        this.tbContactInfo.ReadOnly = false;
                        this.btnOpen.Enabled = true;
                        break;
                    default:
                        this.tbContactInfo.ReadOnly = true;
                        this.btnOpen.Enabled = false;
                        break;
                }
            }
        }

        private void RemoveRights(object sender,EventArgs args)
        {
            if (this.lbAssigned.MoveSelectedItem(lbAvailbale, true, () => Utils.ShowMessageBox(Messages.NoRightItemSelected, Messages.TitleError)))
            {
                if (lbAssigned.Items.Count == 0)
                    this.btnLeft.Enabled = false;
                string username = Convert.ToString(this.dgvUser2.SelectedRows[0].Cells["User Name"].Value);
                List<string> list = new List<string>();
                //if (lbAssigned.Items.Count > 0)
                //{
                    List<string> ur = new List<string>();
                    list=lbAssigned.Items.Cast<string>().ToList();
                    //lbAssigned.Items.Clear();
                    bool ischange = false;
                    currentRelationOfUserRights[username].ForEach(p =>
                    {
                        if (!list.Contains(p))
                        {
                            ischange = true;
                        }
                        //if (list.Contains(p) && !ur.Contains(p))
                        //{
                        //    ur.Add(p);
                        //    lbAssigned.Items.Add(p);
                        //}
                        //else
                        //    ischange = true;
                    });
                    currentRelationOfUserRights[username] = list;
                    if(UserRightsIsChange[username]!=true)
                        UserRightsIsChange[username] = ischange;
                //}
                
            }
        }
        private void AssignRights(object sender, EventArgs args)
        {
            if (this.lbAvailbale.MoveSelectedItem(lbAssigned, false, () => Utils.ShowMessageBox(Messages.NoRightItemSelected, Messages.TitleError)))
            {
                this.btnLeft.Enabled = true;
                string username = Convert.ToString(this.dgvUser2.SelectedRows[0].Cells["User Name"].Value);
                List<string> list = new List<string>();
                bool ischange = false;
                if (lbAssigned.Items.Count > 0)
                {
                    List<string> ur = new List<string>(currentRelationOfUserRights[username]);
                    list = lbAssigned.Items.Cast<string>().ToList();
                    list.ForEach(p =>
                        {
                            if (!ur.Contains(p))
                            {
                                ur.Add(p);
                                ischange = true;
                            }
                        });
                    currentRelationOfUserRights[username] = ur;
                    UserRightsIsChange[username] = ischange;
                }
            }
        }

    }
}
