using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.DAL;
using ShineTech.TempCentre.BusinessFacade.DeviceControl;
using ShineTech.TempCentre.Platform;

namespace ShineTech.TempCentre.BusinessFacade
{
    public partial class Meaning : UserControl
    {
        private MeaningsBLL meaningBLL = new MeaningsBLL();

        public bool IsNewMeaning { get; set; }

        private IList<Meanings> allMeanings = new List<Meanings>();
        private IDictionary<string, IList<Meanings>> currentRelationOfUserAndMeaning;

        public Meaning()
            : this(null, null, null)
        {
        }

        public Meanings MeaningObject
        {
            get;
            set;
        }
        public UserMeanRelation UserMeanRelationObject
        {
            get;
            set;
        }
        public Meaning(Meanings mean, IList<Meanings> allMeanings, IDictionary<string, IList<Meanings>> currentRelationOfUserAndMeaning)
        {
            InitializeComponent();
            this.InitEvent();
            this.SetVisible(false);
            if (mean != null)
            {
                this.MeaningObject = mean;
                this.tbMean.Text = mean.Desc;
            }
            this.allMeanings = allMeanings;
            this.currentRelationOfUserAndMeaning = currentRelationOfUserAndMeaning;
        }

        public Button AddMeaningButton;

        public void SetVisible(bool flag)
        {
            //this.cbEdit.Visible = flag;
            this.pbDel.Visible = flag;
            this.pbEdit.Visible = flag;
            this.tbMean.Enabled = flag;
        }
        private string _mean;
        public string Mean
        {
            get { return _mean; }
            set { this._mean = value; }
        }

        public bool TextBoxEnable
        {
            get { return this.tbMean.Enabled; }
            set { this.tbMean.Enabled = value; }
        }

        public bool CheckBoxVisible
        {
            set { this.cbEdit.Visible = value; }
        }

        public bool IsCheckBoxChecked
        {
            get { return this.cbEdit.Checked; }
            set { this.cbEdit.Checked = value; }
        }

        public void TextBoxFocus()
        {
            this.tbMean.Focus();
        }
        public event EventHandler DelEvent;
        private Meanings _meanging;
        private UserMeanRelation relation;
        public Meaning control;

        public void SetValue()
        {
            //_mean = this.tbMean.Text.TrimEnd();
        }
        private void InitEvent()
        {
            //this.cbEdit.CheckedChanged+=new EventHandler(delegate(object sender,EventArgs args){
            //    this.tbMean.Enabled=this.pbEdit.Visible = this.pbDel.Visible = cbEdit.Checked;
            //});

            //this.pbEdit.Click += new EventHandler(delegate(object sender, EventArgs args)
            //{
            //    this.tbMean.Enabled = true;
            //    this.tbMean.Focus();
            //});

            //this.pbDel.Click += new EventHandler(delegate(object sender, EventArgs args)
            //{
            //    control = this;
            //    DelEvent(this, args);
            //});

            //this.tbMean.Leave += new EventHandler(delegate(object sender, EventArgs args)
            //{
            //    this._mean = this.tbMean.Text;
            //});
            //this.Load += new EventHandler((sender, args) => { this.tbMean.Focus(); });
            this.cbEdit.Click += new EventHandler(cbEdit_Click);
            this.MouseEnter += new EventHandler(wholeControl_MouseEnter);
            this.MouseLeave += new EventHandler(wholeControl_MouseLeave);
            //this.tbMean.LostFocus += new EventHandler((sender, args) => this.changeContentOfMeaning());
            foreach (Control control in Controls)
            {
                control.MouseLeave += new EventHandler(wholeControl_MouseLeave);
            }
            //this.tbMean.KeyDown += new KeyEventHandler(tbMean_KeyDown);
            this.pbEdit.Click += new EventHandler(pbEdit_Click);
            this.pbDel.Click += new EventHandler(pbDel_Click);
        }

        private void cbEdit_Click(object sender, EventArgs e)
        {
            if (currentRelationOfUserAndMeaning.Count == 0 )
            {
                return;
            }
            if (this.IsCheckBoxChecked)
            {
                if (!currentRelationOfUserAndMeaning[Common.CurrentSelectedUserOfDgv1.UserName].Contains(this.MeaningObject))
                {
                    currentRelationOfUserAndMeaning[Common.CurrentSelectedUserOfDgv1.UserName].Add(this.MeaningObject);
                }
            }
            else
            {
                if (currentRelationOfUserAndMeaning[Common.CurrentSelectedUserOfDgv1.UserName].Contains(this.MeaningObject))
                {
                    currentRelationOfUserAndMeaning[Common.CurrentSelectedUserOfDgv1.UserName].Remove(this.MeaningObject);
                }
            }
        }

        private void wholeControl_MouseEnter(object sender, EventArgs e)
        {
            if (this.ClientRectangle.Contains(PointToClient(Cursor.Position)))
            {
                this.pbDel.Visible = true;
                this.pbEdit.Visible = true;
            }
        }

        private void wholeControl_MouseLeave(object sender, EventArgs e)
        {
            if (!this.ClientRectangle.Contains(PointToClient(Cursor.Position)))
            {
                this.pbDel.Visible = false;
                this.pbEdit.Visible = false;
            }
        }

        private bool isChangingProcess = false;
        private void changeContentOfMeaning()
        {
            //if (isChangingProcess)
            //{
            //    return;
            //}
            //isChangingProcess = true;

            if (!this.IsNewMeaning)
            {
                if (string.IsNullOrEmpty(this.tbMean.Text.TrimEnd()))
                {
                    this.tbMean.Text = this.MeaningObject.Desc;
                    this.tbMean.Focus();
                }
                else
                {
                    
                }
            }
            //isChangingProcess = false;
        }

        public void UpdateStatusWhenAddNewMean()
        {
            Meanings meaning = new Meanings() { Id = meaningBLL.GetMeaningPKValue() + 1, Desc = this.Mean.TrimEnd(), Remark = DateTime.Now.ToString() };
            meaningBLL.InsertOrUpdateMeaning(meaning);
            this.allMeanings.Add(meaning);
            this.MeaningObject = meaning;
            this.tbMean.Text = this.MeaningObject.Desc;
            //this.IsNewMeaning = false;
            //this.AddMeaningButton.Enabled = true;
            this.tbMean.Enabled = false;
            this.CheckBoxVisible = true;
        }

        private void pbEdit_Click(object sender, EventArgs e)
        {
            InputBoxDialog updateMeanDialog = new InputBoxDialog(InputBoxTitle.EditMeaning, InputBoxTipMessage.EditMeaning, false);
            updateMeanDialog.InputBoxText = this.MeaningObject.Desc;
            if (updateMeanDialog.ShowDialog(this) == DialogResult.OK)
            {
                if (!updateMeanDialog.InputBoxText.TrimEnd().Equals(this.MeaningObject.Desc, StringComparison.Ordinal))
                {
                    this.MeaningObject.Desc = updateMeanDialog.InputBoxText.TrimEnd();
                    this.MeaningObject.Remark = DateTime.Now.ToString();
                    meaningBLL.InsertOrUpdateMeaning(this.MeaningObject);
                    this.tbMean.Text = this.MeaningObject.Desc;
                    this.tbMean.Enabled = false;
                }
            }
        }


        private void pbDel_Click(object sender, EventArgs e)
        {
            if (DialogResult.Yes == MessageBox.Show("This operation could influence all the other users that are related with this meaning. Are you sure to delete this Meaning?", "warning", MessageBoxButtons.YesNo))
            {
                meaningBLL.DeleteMeaningAndRelation(this.MeaningObject);
                allMeanings.Remove(this.MeaningObject);
                foreach (var item in currentRelationOfUserAndMeaning.Values)
                {
                    item.Remove(this.MeaningObject);
                }
                this.Dispose();
            }
        }

    }
}
