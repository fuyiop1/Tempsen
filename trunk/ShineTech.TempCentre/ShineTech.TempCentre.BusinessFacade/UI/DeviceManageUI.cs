using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.DAL;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;

namespace ShineTech.TempCentre.BusinessFacade
{
    public partial class DeviceManageUI
    {
        #region private variable
        private System.Windows.Forms.Label lBattery;
        private System.Windows.Forms.Label lStatus;
        private System.Windows.Forms.Label lSerialNum;
        private System.Windows.Forms.Label lDesc;
        private System.Windows.Forms.Label lTripNum;
        private System.Windows.Forms.Label lModel;
        private System.Windows.Forms.Label lProductName;

        private System.Windows.Forms.Label lLogInterval;
        private System.Windows.Forms.Label lStartDelay;
        private System.Windows.Forms.Label lStartMode;
        private System.Windows.Forms.Label lLogCycle;

        private System.Windows.Forms.Label lLowAlarm;
        private System.Windows.Forms.Label lHighLimit;
        private System.Windows.Forms.Label lDelayTime;
        private System.Windows.Forms.Label lAlarmType;
        private System.Windows.Forms.Label lAlarmEvent;

        private System.Windows.Forms.Label lLowest;
        private System.Windows.Forms.Label lHighest;
        private System.Windows.Forms.Label lTripLen;
        private System.Windows.Forms.Label lFirstPoint;
        private System.Windows.Forms.Label lLogStop;
        private System.Windows.Forms.Label lLogStart;
        private System.Windows.Forms.Label lMKT;
        private System.Windows.Forms.Label lDataPoint;
        private System.Windows.Forms.Label lAveTemp;

        private System.Windows.Forms.Label lEventLowTrig;
        private System.Windows.Forms.Label lEventHighTrig;
        private System.Windows.Forms.Label lEventLongestBelow;
        private System.Windows.Forms.Label lEventLongestAbove;
        private System.Windows.Forms.Label lEventBelow;
        private System.Windows.Forms.Label lEventHighestAbove;
        private System.Windows.Forms.Label lEventLowNum;
        private System.Windows.Forms.Label lEventLow;
        private System.Windows.Forms.Label lEventNum;
        private System.Windows.Forms.Label lEventHigh;
        private Label lbUserinfo;
        private System.Windows.Forms.Button btnSave;
        private ToolStripMenuItem reportConfigToolStripMenuItem;
        private ToolStripMenuItem UserManageMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeDetailToolStripMenuItem;
        private Button btnAuditTrial;
        private System.Windows.Forms.DataGridView dgvLog;
        private System.Windows.Forms.Panel pnAuditTrial;
        private System.Windows.Forms.Panel pnAuditCheck;
        private System.Windows.Forms.CheckBox cbAnaAudit;
        private System.Windows.Forms.CheckBox cbSysAudit;
        private System.Windows.Forms.TabControl tabCtl;
        private System.Windows.Forms.Panel pnAuditFilter;
        private System.Windows.Forms.DateTimePicker dtpAuditTo;
        private System.Windows.Forms.DateTimePicker dtpAuditFrom;
        private Panel pnDevice;
        private Panel pnSource;
        private System.Windows.Forms.ComboBox cmbUserName;
        private System.Windows.Forms.ComboBox cmbAciton;
        #endregion
        IDataProcessor processor;
        private Form form;
        private OperationLogBLL logbll = new OperationLogBLL();
        #region .ctor
        public DeviceManageUI()
        {
        }
        public DeviceManageUI(Form form)
        {
            this.ConstructForms(form);
            processor = new DeviceProcessor();
            this.InitProduct();
            this.InitLoggerSetting();
            this.InitAlarmSetting();
            this.InitLogSummary();
            this.InitAlarmEvent();
            this.InitUserinfo();
            this.InitEvents();
            //SetPanel();
        }
        private void ConstructForms(Form  form) 
        {
            this.form = form;
            lProductName = form.Controls.Find("lProductName", true)[0] as Label;
            lStatus = form.Controls.Find("lStatus", true)[0] as Label;
            lSerialNum = form.Controls.Find("lSerialNum", true)[0] as Label;
            lDesc = form.Controls.Find("lDesc", true)[0] as Label;
            lTripNum = form.Controls.Find("lTripNum", true)[0] as Label;
            lModel = form.Controls.Find("lModel", true)[0] as Label;
            lProductName = form.Controls.Find("lProductName", true)[0] as Label;
            lBattery = form.Controls.Find("lBattery", true)[0] as Label;

            lLogInterval = form.Controls.Find("lLogInterval", true)[0] as Label;
            lStartDelay = form.Controls.Find("lStartDelay", true)[0] as Label;
            lStartMode = form.Controls.Find("lStartMode", true)[0] as Label;
            lLogCycle = form.Controls.Find("lLogCycle", true)[0] as Label;

            lLowAlarm = form.Controls.Find("lLowAlarm", true)[0] as Label;
            lHighLimit = form.Controls.Find("lHighLimit", true)[0] as Label;
            lDelayTime = form.Controls.Find("lDelayTime", true)[0] as Label;
            lAlarmType = form.Controls.Find("lAlarmType", true)[0] as Label;
            lAlarmEvent = form.Controls.Find("lAlarmEvent", true)[0] as Label;

            lLowest = form.Controls.Find("lLowest", true)[0] as Label;
            lHighest = form.Controls.Find("lHighest", true)[0] as Label;
            lTripLen = form.Controls.Find("lTripLen", true)[0] as Label;
            lFirstPoint = form.Controls.Find("lFirstPoint", true)[0] as Label;
            lLogStop = form.Controls.Find("lLogStop", true)[0] as Label;
            lLogStart = form.Controls.Find("lLogStart", true)[0] as Label;
            lMKT = form.Controls.Find("lMKT", true)[0] as Label;
            lDataPoint = form.Controls.Find("lDataPoint", true)[0] as Label;
            lAveTemp = form.Controls.Find("lAveTemp", true)[0] as Label;

            lEventLowTrig= form.Controls.Find("lEventLowTrig", true)[0] as Label;
            lEventHighTrig= form.Controls.Find("lEventHighTrig", true)[0] as Label;
            lEventLongestBelow= form.Controls.Find("lEventLongestBelow", true)[0] as Label;
            lEventLongestAbove= form.Controls.Find("lEventLongestAbove", true)[0] as Label;
            lEventBelow = form.Controls.Find("lEventBelow", true)[0] as Label;
            lEventHighestAbove = form.Controls.Find("lEventHighestAbove", true)[0] as Label;
            lEventLowNum = form.Controls.Find("lEventLowNum", true)[0] as Label;
            lEventLow = form.Controls.Find("lEventLow", true)[0] as Label;
            lEventNum = form.Controls.Find("lEventNum", true)[0] as Label;
            lEventHigh = form.Controls.Find("lEventHigh", true)[0] as Label;

            btnSave = form.Controls.Find("btnSave", true)[0] as Button;

            reportConfigToolStripMenuItem = (ToolStripMenuItem)((MenuStrip)form.Controls.Find("menuStrip1",true)[0]).Items.Find("reportConfigToolStripMenuItem",true)[0];
            UserManageMenuItem = (ToolStripMenuItem)((MenuStrip)form.Controls.Find("menuStrip1", true)[0]).Items.Find("UserManageMenuItem", true)[0];
            changeDetailToolStripMenuItem = (ToolStripMenuItem)((MenuStrip)form.Controls.Find("menuStrip1", true)[0]).Items.Find("changeDetailToolStripMenuItem", true)[0];
            lbUserinfo = form.Controls.Find("lbUserinfo", true)[0] as Label;

            btnAuditTrial = form.Controls.Find("btnAuditTrial", true)[0] as Button;
            dgvLog = form.Controls.Find("dgvLog", true)[0] as DataGridView;
            pnAuditTrial = form.Controls.Find("pnAuditTrial", true)[0] as Panel;
            pnAuditCheck = form.Controls.Find("pnAuditCheck", true)[0] as Panel;
            cbAnaAudit = form.Controls.Find("cbAnaAudit", true)[0] as CheckBox;
            cbSysAudit = form.Controls.Find("cbSysAudit", true)[0] as CheckBox;
            tabCtl = form.Controls.Find("tabCtl", true)[0] as TabControl;
            pnAuditFilter = form.Controls.Find("pnAuditFilter", true)[0] as Panel;
            dtpAuditTo = form.Controls.Find("dtpAuditTo", true)[0] as DateTimePicker;
            dtpAuditFrom = form.Controls.Find("dtpAuditFrom", true)[0] as DateTimePicker;
            pnDevice = form.Controls.Find("pnDevice", true)[0] as Panel;
            pnSource = form.Controls.Find("pnSource", true)[0] as Panel;
            cmbUserName = form.Controls.Find("cmbUserName", true)[0] as ComboBox;
            cmbAciton = form.Controls.Find("cmbAciton", true)[0] as ComboBox;
        }
        #endregion
        /// <summary>
        /// summary product信息列表
        /// </summary>
        private void InitProduct()
        {
            
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("PID", 10001);
            Device d = processor.QueryOne<Device>("select * from device where pid=@PID", dic);
             lProductName.Text= string.Format("Product Name: {0}",d.ProductName);
             lSerialNum.Text=string.Format("Serial Number: {0}",d.SerialNum);
             lModel.Text = string.Format("Model: {0}", d.Model);
             lTripNum.Text = string.Format("Trip Number: {0}", d.TripNum);
             lDesc.Text = string.Format("Description: {0}", d.DESCS);
             lBattery.Text = string.Format("Battery: {0:#%}", d.Battery);
             lStatus.Text = string.Format("Current Status: {0}", "Connected-Logging");
        }
        public void InitLoggerSetting()
        {
            lLogCycle.Text = string.Format("Log Cycle: {0}", "7d15h35m");
            lLogInterval.Text =string.Format("Log Interval: {0}", "30m15s");
            lStartDelay.Text = string.Format("Start Delay: {0}", "30m");
            lStartMode.Text = string.Format("Start Mode: {0}", "Manual Start");
        }
        public void InitAlarmSetting()
        {
            lLowAlarm.Text = string.Format("Low Alarm: {0}", "2°C");
            lAlarmType.Text = string.Format("Alarm Type: {0}", "Cumulative Alarm");
            lAlarmEvent.Text = string.Format("Alarm Event: {0}", "Single Event");
            lHighLimit.Text = string.Format("High Limit: {0}", "8°C");
            lDelayTime.Text = string.Format("Delay Time: {0}", "30m");
        }
        public void InitLogSummary()
        {
            lLowest.Text = string.Format("Lowest Temperature: {0}", "18.2°C @ Jun29,2011 08:29:22AM");
            lHighest.Text = string.Format("Highest Temperature: {0}", "33.2°C @ Jun28,2011 09:29:22AM");
            lTripLen.Text = string.Format("Trip Length: {0}", "1033Km");
            lFirstPoint.Text = string.Format("First Point: {0}", "Jun24,2011 09:29:22AM");
            lLogStop.Text = string.Format("Stop Time: {0}", "Jun29,2011 19:29:22PM");
            lLogStart.Text = string.Format("Start Time: {0}", "Jun24,2011 00:29:22AM");
            lMKT.Text = string.Format("MKT: {0}", "");
            lAveTemp.Text = string.Format("Average Temperature: {0}", "20.2°C");
            lDataPoint.Text = string.Format("Data Point: {0}", "158");
        }
        public void InitAlarmEvent()
        {
            lEventLowTrig.Text = string.Format("Fist Trigged: {0}", "Jun29,2011 08:29:22AM");
            lEventHighTrig.Text = string.Format("Fist Trigged: {0}", "Jun28,2011 09:29:22AM");
            lEventLongestBelow.Text = string.Format("Longest Time Below HL: {0}", "1h2m");
            lEventLongestAbove.Text = string.Format("Longest Time Above HL: {0}", "3h10m");
            lEventBelow.Text = string.Format("Total Time Below HL: {0}", "4h21m");
            lEventHighestAbove.Text = string.Format("Total Time Above HL: {0}", " 5h49m");
            lEventLowNum.Text = string.Format("Number of Events: {0}", "27");
            lEventLow.Text = string.Format("Low Alarm: {0}", "2°C");
            lEventNum.Text = string.Format("Number of Events: {0}", "15");
            lEventHigh.Text = string.Format("High Alarm: {0}", "8°C");
        }
        private void InitUserinfo()
        {
            this.lbUserinfo.Text = Common.User.UserName + " log on " + DateTime.Now.ToString()+". ";
            int day = (DateTime.Now.Date - Common.User.LastPwdChangedTime.Date).Days;
            if (day >= Common.Policy.PwdExpiredDay)
                this.lbUserinfo.Text += ". Please change the password,it's expired " + (day - Common.Policy.PwdExpiredDay).ToString()+" days.";
            else
                this.lbUserinfo.Text +=  (-day + Common.Policy.PwdExpiredDay).ToString() + " days left to change the password.";
            /*设置用户能否修改密码*/
            Boolean change = Convert.ToBoolean(Common.User.ChangePwd);
            this.changeDetailToolStripMenuItem.Enabled = change;
            this.UserManageMenuItem.Enabled= Convert.ToInt32(Common.User.RoleId)>1 ? false : true;
        }
        private void Save()
        {
            if (!File.Exists("c:\\my.bin"))
            {
                ArrayList al = new ArrayList();
                al.Add(Common.User);
                al.Add(Common.User);
                IFormatter formatter = new BinaryFormatter();
                Stream s = new FileStream("c:\\my.bin", FileMode.Create, FileAccess.Write, FileShare.None);
                formatter.Serialize(s, al);
                s.Close();
            }
            else
            {
                IFormatter formatter = new BinaryFormatter();
                Stream s = new FileStream("c:\\my.bin", FileMode.Open, FileAccess.Read, FileShare.Read);
                ArrayList u = formatter.Deserialize(s) as ArrayList;
                s.Close();
            }
        }
        private void InitEvents()
        {
            this.btnSave.Click+=new EventHandler(delegate(object sender,EventArgs args){
                this.Save();
            });
            this.btnAuditTrial.Click += new EventHandler(delegate(object sender, EventArgs args)
            {
                this.AuditTrial();
                //MessageBox.Show(this.pnAuditList.Parent.ToString());
            });
            this.cmbUserName.SelectedIndexChanged += new EventHandler(delegate(object sender, EventArgs args)
            {
                SearchLog();
            });
            
        }
        //审计日志
        private void AuditTrial()
        {
            this.form.Refresh();
            this.SetControlState(CtlState.audit);
            List<OperationLog> list=logbll.GetLog(null);
            this.dgvLog.DataSource = list;
            //设置过滤条件 action,usename
            List<string> ls = list.Select(p => { return p.Action; }).Distinct().ToList();
            ls.Add("");
            ls.Reverse();
            this.cmbAciton.DataSource = ls;
            this.cmbAciton.DisplayMember = "Action";
            //this.cmbAciton.ValueMember = "Action";
            List<string> useList=list.Select(p => { return p.Username; }).Distinct().ToList();
            useList.Add("");
            useList.Reverse();
            this.cmbUserName.DataSource = useList;
            this.cmbUserName.DisplayMember = "UserName";
            DateTime end = list.Max(p => { return p.Operatetime;});
            DateTime start = list.Min(p => { return p.Operatetime; });
            this.dtpAuditFrom.Value = start;
            this.dtpAuditTo.Value = end;
            //this.cmbUserName.ValueMember = "UserName";
            //this.dtpAuditFrom.Value =
            //this.dgvLog.
        }
        private void SearchLog()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (!cbSysAudit.Checked && !cbAnaAudit.Checked)
                dic.Add("LogType", -1);
            else if (cbAnaAudit.Checked && !cbSysAudit.Checked)
                dic.Add("LogType", 1);
            else if (cbSysAudit.Checked && !cbAnaAudit.Checked)
                dic.Add("LogType", 0);
            if(cmbAciton.SelectedValue!=string.Empty)
                dic.Add("Action",cmbAciton.SelectedValue);
            if(cmbUserName.SelectedValue!=string.Empty)
                dic.Add("UserName", cmbUserName.SelectedValue);
            dic.Add("OperateTime1",dtpAuditFrom.Value.ToString("yyyyMMdd"));
            dic.Add("OperateTime2", dtpAuditTo.Value.ToString("yyyyMMdd"));
            this.dgvLog.DataSource= logbll.GetLog(dic);
        }
        /*设置空间状态*/
        private void SetControlState(CtlState state)
        {
            switch (state)
            {
                case CtlState.audit:
                    //pnAuditCheck.Visible = true;
                    //pnAuditFilter.Visible = true;
                    pnAuditTrial.Show();
                    pnDevice.Hide();
                    pnSource.Hide();
                    break;
                default:
                    pnAuditCheck.Visible = false;
                    pnAuditFilter.Visible = false;

                    pnDevice.Visible = true;
                    pnSource.Visible = true;
                    break;
            }
        }

        private void SetPanel()
        {
            Panel panel = new Panel();
            panel.Parent = form;
            panel.Location = new System.Drawing.Point(210, 111);
            panel.Visible = true;
            panel.Size = new System.Drawing.Size(824, 602);
            panel.Controls.Add(this.dgvLog);
        }
    }
    enum CtlState { device,data,audit }
}
