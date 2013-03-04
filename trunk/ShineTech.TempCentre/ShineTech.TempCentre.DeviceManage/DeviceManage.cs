using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using ShineTech.TempCentre.BusinessFacade;
using ZedGraph;

namespace ShineTech.TempCentre.DeviceManage
{
    public partial class DeviceManage : Form
    {
        private DeviceManageUI dm;
        private Form form;
        public static DeviceManage manage = null;
        public DeviceManage()
        {
            InitializeComponent();
            
        }
        protected DeviceManage(Form form)
        {
            InitializeComponent();
            this.form = form;
            this.Test();
            //this.SetPanel();
        }    

        private void reportConfigToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ReportConfiguration reportConfig = new ReportConfiguration();
            reportConfig.ShowDialog(this);
        }

        private void UserManageMenuItem_Click(object sender, EventArgs e)
        {
            Administration admin = new Administration();
            admin.ShowDialog(this);
        }

        private void btnLogIn_Click(object sender, EventArgs e)
        {
            form.Show();
            manage = null;//单例资源释放
            this.Dispose();
        }

        private void DeviceManage_FormClosed(object sender, FormClosedEventArgs e)
        {
            form.Close();
        }
        /*单例*/
        public static DeviceManage GetInstance(Form form)
        {
            if (manage == null)
                manage = new DeviceManage(form);
            return manage;
        }
        /// <summary>
        /// 自动登出检查
        /// </summary>
        private void UserSessionChecker()
        {
            UserSession.BeginTimer(60000, delegate(object sender,EventArgs args) {
                if (!UserSession.SessionAlive)
                {
                    UserSession.MinutesAlive += (int)UserSession.UserTimer.Interval/60000;
                    if (UserSession.MinutesAlive >= Common.Policy.InactivityTime)
                    {
                        form.Invoke(new Action(delegate() {
                            form.Show();
                            manage = null;
                            this.Dispose();
                        }));
                    }      
                } 
                else
                    {
                        UserSession.MinutesAlive = 0;
                    }
                UserSession.ResetTimer();
            });
            //UserSession.Begin();
        }
        private void AttachEvents()
        {
            this.MouseMove+=new MouseEventHandler(delegate(object sender, MouseEventArgs args){
                UserSession.SessionAlive = true;
            });
            this.KeyDown += new KeyEventHandler(delegate(object sender, KeyEventArgs args)
            {
                UserSession.SessionAlive = true;
            });
        }

        private void DeviceManage_Load(object sender, EventArgs e)
        {
            //Control.CheckForIllegalCrossThreadCalls = false;
            dm = new DeviceManageUI(this);
            AttachEvents();
            UserSessionChecker();
        }

        private void changeDetailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ChangeDetail detail = new ChangeDetail();
            detail.ShowDialog(this);
        }
        /*图表测试*/
        private void Test()
        {
            GraphPane pane = this.zedGraphControl1.GraphPane;
            pane.Title.Text = "my graph";
            pane.XAxis.Title.Text = "Date/Time";
            pane.YAxis.Title.Text = "Temperature(C)";
            string[] times = new string[12];
            DateTime dt = DateTime.Now;
            for (int i = 0; i < 12; i++)
            {
                times[i] = dt.AddSeconds(2.0).ToLongTimeString();
                dt = dt.AddSeconds(2.0);
            }
            double[] data = new double[] { 10.0, 12.0, 9.0, 13.2, 15.3, 16.3, 11.0, 9.0, 22.5, 33.0, 31.5, 0.0 };
            PointPairList list = new PointPairList();
            pane.XAxis.Type = AxisType.Text;
            LineItem myCurve = pane.AddCurve("Alpha",
                null, data, Color.Orange, SymbolType.Diamond);
            myCurve.Line.IsSmooth = true;
            myCurve.Line.IsAntiAlias = true;
            pane.XAxis.Scale.TextLabels = times;
            pane.XAxis.Scale.FontSpec.Angle = 40;
            //pane.YAxis.Scale.TextLabels=
            pane.Chart.Fill = new Fill(Color.White, Color.White, 45.0f);
            pane.XAxis.IsVisible = true;
            this.zedGraphControl1.GraphPane = pane;
            this.zedGraphControl1.IsShowPointValues = true;
            this.zedGraphControl1.Cursor = Cursors.Hand;
            //this.zedGraphControl1.IsShowContextMenu = true;
            //this.zedGraphControl1.ContextMenuStrip = null;
            this.zedGraphControl1.AxisChange();
        }
    }
}
