using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShineTech.TempCentre.BusinessFacade
{
    public partial class ViewTool : UserControl
    {
        public ViewTool()
        {
            InitializeComponent();
            InitEvent();
        }
        private Point m_MousePoint;
        private Point m_LastPoint;
        //private ZedGraph.GraphPane pane;
        public event EventHandler AxisVisibilityEvent;
        public event EventHandler TooHideEvent;
        public event EventHandler GraphRestoreEvent;
        public event EventHandler LimitLineEvent;
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            this.m_LastPoint = this.Location;
            this.m_MousePoint = this.PointToScreen(e.Location);
        }
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            if (e.Button == MouseButtons.Left)
            {
                Point t = this.PointToScreen(e.Location);
                Point l = this.m_LastPoint;

                l.Offset(t.X - this.m_MousePoint.X, t.Y - this.m_MousePoint.Y);
                this.Location = l;
            }
        }
        private void InitEvent()
        {
            this.rbDateTime.CheckedChanged += new EventHandler(AxisTitle);
            this.rbElapsedTime.CheckedChanged += new EventHandler(AxisTitle);
            this.rbDtaPoints.CheckedChanged += new EventHandler(AxisTitle);
            this.pictureBox1.Click += new EventHandler((a, b) => {
                //this.Hide();
                if (this.tableLayoutPanel1.Visible == true)
                {
                    this.tableLayoutPanel1.Visible = false;
                    this.Height = this.Height - tableLayoutPanel1.Height;
                }
                else
                {
                    this.Height = this.Height + tableLayoutPanel1.Height;
                    this.tableLayoutPanel1.Visible = true;
                }
                this.Refresh();
                //TooHideEvent(a, b);
            });
            this.pictureBox2.Click += new EventHandler((a, b) =>
            {
                GraphRestoreEvent(a, b);
            });
            this.cbHighLimit.CheckedChanged+=new EventHandler((a,b)=>LimitLineEvent(a,b));
            this.cbLowLimit.CheckedChanged += new EventHandler((a, b) => LimitLineEvent(a, b));
        }
        private void AxisTitle(object sender, EventArgs args)
        {
            //if(AxisVisibilityEvent!=null)
            AxisVisibilityEvent(sender, args);
        }
    }

}
