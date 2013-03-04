using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using ShineTech.TempCentre.DAL;
using System.Xml.Serialization;
using ShineTech.TempCentre.Platform;
namespace ShineTech.TempCentre.BusinessFacade
{
    public partial class UserRight : UserControl
    {
        private string UserName=string.Empty;
        private UserRightBLL _bll = new UserRightBLL();
        public event EventHandler SignRightOnChange;
        public UserRight()
        {
            InitializeComponent();
            this.InitLeft();
            this.InitRight(UserName, true);
            this.InitEvents();
        }
        public UserRight(bool admin)
        {
            InitializeComponent();
            this.InitEvents();
            this.InitLeft();
            this.InitRight(UserName, admin);
        }
        #region 属性
        private string _username;

        public string User
        {
            get { return _username; }
            set { _username = value; }
        }
        private List<string> _right;

        public List<string> Right
        {
            get { return _right; }
        }
        #endregion
        public UserRight(string username)
            : this()
        {
            UserName = username;
        }
        /// <summary>
        /// 加载左边的xml值
        /// </summary>
        private void InitLeft()
        {
            Rights r = Common.GetRightsList();
            if (r == null)
                r = Common.SetRightsList();
            r.right.ToList().ForEach(p=>this.lbAvailbale.Items.Add(p));
        }
        public void InitRight(string username,Boolean admin)
        {
            lbAssigned.Items.Clear();
            if (!admin)
            {
                List<DAL.UserRight> list = _bll.GetRightByUserName(username);
                if (list != null && list.Count > 0)
                {
                    list.ToList().ForEach(p => lbAssigned.Items.Add(p.Right));
                    this.btnRight.Enabled = true;
                }
                else
                {
                    lbAssigned.Items.Add(RightsText.ConfigurateDevices);
                    lbAssigned.Items.Add(RightsText.CommentRecords);
                    //this.btnLeft.Enabled = false;
                }
            }
            else
            {
                Rights r = Common.GetRightsList();
                if (r == null)
                    r = Common.SetRightsList();
                r.right.ToList().ForEach(p => this.lbAssigned.Items.Add(p));
                this.btnLeft.Enabled = true;
            }
            SetValue();
        }


        public void SetValue()
        {
            this.User = UserName;
            /*插入list*/
            if (lbAssigned.Items.Count > 0)
            {
                if (_right == null)
                    _right = new List<string>();
                _right.Clear();
                lbAssigned.Items.Cast<string>().ToList().ForEach(p=>_right.Add(p));
            }
            
        }
        private void InitEvents()
        {
            this.btnLeft.Click += new EventHandler((sender, args) => {
                if (this.lbAssigned.MoveSelectedItem(lbAvailbale, true, () => Utils.ShowMessageBox(Messages.NoRightItemSelected, Messages.TitleError)))
                {
                    if (lbAssigned.Items.Count == 0)
                        this.btnLeft.Enabled = false;
                }
                lbAssigned_TextChanged(lbAssigned, args);
            });
            this.btnRight.Click += new EventHandler((sender, args) => {

                if (this.lbAvailbale.MoveSelectedItem(lbAssigned, false, () => Utils.ShowMessageBox(Messages.NoRightItemSelected, Messages.TitleError)))
                    this.btnLeft.Enabled = true;
                lbAssigned_TextChanged(lbAssigned, args);
            });
            this.initRightListBoxEvent();
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
                //this.lbAssigned.it+=new EventHandler(lbAssigned_TextChanged);
            }
        }

        private void listBox_DrawItem(object s, DrawItemEventArgs e)
        {
            e.DrawBackground();
            e.DrawFocusRectangle();
            var sender = s as ListBox;
            if (sender != null)
            {
                e.Graphics.DrawString(sender.Items[e.Index].ToString(), e.Font, new SolidBrush(e.ForeColor), e.Bounds);
            }
        }
        private void lbAssigned_TextChanged(object sender, EventArgs args)
        {
            if (SignRightOnChange != null)
                SignRightOnChange(sender, args);
        }
        private void pnRight_Paint(object sender, PaintEventArgs e)
        {

        }
    }
    public class Rights
    {
        [XmlElement]
        public  List<string> right;
    }
}
