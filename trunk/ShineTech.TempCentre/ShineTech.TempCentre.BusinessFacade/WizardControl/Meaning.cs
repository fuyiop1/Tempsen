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
    public partial class Meaning : UserControl
    {
        public Meaning()
        {
            InitializeComponent();
            this.InitEvent();
            this.SetVisible(false);
            
        }

        public Meaning(Meanings mn,UserMeanRelation ur):this()
        {
            this._meanging = mn;
            this.relation=ur;
        }
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


        public event EventHandler DelEvent;
        private Meanings _meanging;
        private UserMeanRelation relation;
        public Meaning control;
        
        public void SetValue()
        {
            _mean = this.tbMean.Text.TrimEnd();
        }
        private void InitEvent()
        {
            this.cbEdit.CheckedChanged+=new EventHandler(delegate(object sender,EventArgs args){
                this.pbEdit.Visible = this.pbDel.Visible = cbEdit.Checked;
            });

            this.pbEdit.Click += new EventHandler(delegate(object sender, EventArgs args)
            {
                this.tbMean.Enabled = true;
            });

            this.pbDel.Click += new EventHandler(delegate(object sender, EventArgs args)
            {
                control = this;
                DelEvent(this, args);
            });

            this.tbMean.Leave += new EventHandler(delegate(object sender, EventArgs args)
            {
                this._mean = this.tbMean.Text;
            });
            this.Load += new EventHandler((sender, args) => { this.tbMean.Focus(); });
        }
    }
}
