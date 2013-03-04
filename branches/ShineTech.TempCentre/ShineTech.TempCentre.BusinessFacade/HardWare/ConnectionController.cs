using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.Windows.Forms;
using ShineTech.TempCentre.Platform;
using System.IO;

using Babu.Windows;
using System.ComponentModel;
///<summary>
///CLR Ver : 4.0.30319.225
///CreateBy: wangfei
///CreateOn:10/10/2011 9:21:55 AM
///FileName:ConnectionController
///</summary>
namespace ShineTech.TempCentre.BusinessFacade
{
    public class ConnectionController
    {
        public static bool IsAbortConnection { get; set; }
        private static List<string> _deviceList=new List<string> ();

        public static List<string> DeviceList
        {
            get 
            {
                return _deviceList;
            }
        }
        private static System.Threading.Thread _ConnectThread;

        public static System.Threading.Thread ConnectThread
        {
            get 
            { 
                return ConnectionController._ConnectThread;
            }
            set { _ConnectThread = value; }
        }
        private static BackgroundWorker m_ManualConnectWorker;
        private static BackgroundWorker m_AutoConnectWorker;
        private static BackgroundWorker m_ProgressWorker;

        public static BackgroundWorker ProgressWorker
        {
            get
            {
                if (m_ProgressWorker == null)
                {
                    m_ProgressWorker = new BackgroundWorker();
                    m_ProgressWorker.WorkerSupportsCancellation = true;
                    m_ProgressWorker.WorkerReportsProgress = true;
                }
                return ConnectionController.m_ProgressWorker;
            }
            set { m_ProgressWorker = value; }
        }
        public static BackgroundWorker AutoConnectWorker
        {
            get
            {
                if (m_AutoConnectWorker == null)
                {
                    m_AutoConnectWorker = new BackgroundWorker();
                    m_AutoConnectWorker.WorkerSupportsCancellation = true;
                }
                return ConnectionController.m_AutoConnectWorker;
            }
            set { m_AutoConnectWorker = value; }
        }
        public static BackgroundWorker ManualConnectWorker
        {
            get
            {
                if (m_ManualConnectWorker == null)
                {
                    m_ManualConnectWorker = new BackgroundWorker();
                    m_ManualConnectWorker.WorkerSupportsCancellation = true;
                }
                return ConnectionController.m_ManualConnectWorker;
            }
            set { m_ManualConnectWorker = value; }
        }
        private static Dictionary<DeviceType, CheckBox> _cbList=new Dictionary<DeviceType,CheckBox> ();
        private static string _A1;
        private static string _A3;
        private static string _A4;
        private static string _A5;
        private static string _A6;
        private static string _High;
        private static string _Low;
        public static Dictionary<DeviceType, CheckBox> CbList
        {
            get { return ConnectionController._cbList; }
            set { ConnectionController._cbList = value; }
        }
        public static bool GetConfigVisibleProperity(SuperDevice tag)
        {
            bool isVisible = false;
            if (tag != null)
            {
                if (tag is ITAGSingleUse || tag is ITAGPDF)
                    isVisible = false;
                else
                {
                    if ((tag.DeviceID == 201 || tag.DeviceID == 203) && (tag.RunStatus == 2 || tag.RunStatus == 3))
                        isVisible = false;
                    else
                        isVisible = true;
                }
            }
            return isVisible;
        }

        public static void GenerateDeviceList(Panel container)
        {
            XDocument xmlFile = XDocument.Load(Path.Combine(Application.StartupPath,"HardWare\\DeviceRecorders.xml"));
            var deviceList = from c in xmlFile.Element("DeviceList").Elements()
                             select c;
            int i = 0;
            _cbList.Clear();
            foreach (XElement name in deviceList)
            {
                CheckBox cb = new CheckBox();
                cb.AutoSize = true;
                cb.BackColor = System.Drawing.Color.White;
                cb.Font = new System.Drawing.Font("Arial", 9F);
                cb.Location = new System.Drawing.Point(16, 52+25*i);
                cb.Name = "cb"+name.Value.Split(new char[]{'-'}).Last();
                cb.Size = new System.Drawing.Size(113, 19);
                cb.TabIndex = i;
                cb.Text = name.Value;
                cb.UseVisualStyleBackColor = false;
                cb.CheckedChanged += new EventHandler(SetCheckedOfCheckBox);
                int code = Convert.ToInt32(name.Attributes().First().Value);
                _cbList.Add((DeviceType)code, cb);
                //_deviceList.Add(name.Value);
                //container.Controls.Add(cb);
                i++;
            }
            container.Controls.AddRange(_cbList.Values.ToArray());
        }
        private static void SetCheckedOfCheckBox(object sender, EventArgs e)
        {
            CheckBox cb = sender as CheckBox;
            if (cb != null&&cb.Checked)
            {
                _cbList.Values.ToList().ForEach(p =>
                {
                    if (p.Text != cb.Text)
                        p.Checked = false;
                });
            }
        }
        public static ResultForCheckTempWhetherExceedDefinedRange IsOverTemp(int deviceid,string temp,string tempUnit)
        {
            ResultForCheckTempWhetherExceedDefinedRange result = ResultForCheckTempWhetherExceedDefinedRange.Correct;
            double defaultHighLimit = 70;
            double defaultLowLimit = -30;
            double elogHighLimit = 105;
            double elogLowLimit = -40;
            if ("F" == tempUnit)
            {
                defaultHighLimit = Convert.ToDouble(Common.TransferTemp("C", defaultHighLimit.ToString()));
                defaultLowLimit = Convert.ToDouble(Common.TransferTemp("C", defaultLowLimit.ToString()));
                elogHighLimit = Convert.ToDouble(Common.TransferTemp("C", elogHighLimit.ToString()));
                elogLowLimit = Convert.ToDouble(Common.TransferTemp("C", elogLowLimit.ToString()));
            }
            try
            {
                double d = Convert.ToDouble(temp); ;
                switch (deviceid)
                {
                    case 300:
                        if (d < elogLowLimit)
                        {
                            result = ResultForCheckTempWhetherExceedDefinedRange.TooLow;
                        }
                        if(d > elogHighLimit)
                        {
                            result = ResultForCheckTempWhetherExceedDefinedRange.TooHigh;
                        }
                        break;
                    default:
                        if (d < defaultLowLimit)
                        {
                            result = ResultForCheckTempWhetherExceedDefinedRange.TooLow;
                        }
                        if (d > defaultHighLimit)
                        {
                            result = ResultForCheckTempWhetherExceedDefinedRange.TooHigh;
                        }
                        break;
                }
            }
            catch (Exception)
            {
                result = ResultForCheckTempWhetherExceedDefinedRange.Error;
            }
            
            return result;
        }


        private static void checkAllSingleAlarmTempFields(TextBox tbHigh, TextBox tbLow, PictureBox pbHigh, PictureBox pbLow, ToolTip wrongTip, SuperDevice Tag, string configUnit, CheckBox cbHigh, CheckBox cbLow)
        {
            string pattern = "^[-+]?[0-9]+[/.]?[0-9]?$";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
            if (cbHigh.Checked)
            {
                if (!regex.IsMatch(tbHigh.Text))
                {
                    pbHigh.Visible = true;
                    Common.SetToolTip(wrongTip, pbHigh, Messages.TemperatureValueInvalid);
                }
                else
                {
                    pbHigh.Visible = false;
                }
            }

            if (cbLow.Checked)
            {
                if (!regex.IsMatch(tbLow.Text))
                {
                    pbLow.Visible = true;
                    Common.SetToolTip(wrongTip, pbLow, Messages.TemperatureValueInvalid);
                }
                else
                {
                    pbLow.Visible = false;
                }
            }

            if (cbHigh.Checked && !pbHigh.Visible)
            {
                ResultForCheckTempWhetherExceedDefinedRange result = ConnectionController.IsOverTemp(Tag.DeviceID, tbHigh.Text, configUnit);
                if (result == ResultForCheckTempWhetherExceedDefinedRange.TooHigh)
                {
                    pbHigh.Visible = true;
                    Common.SetToolTip(wrongTip, pbHigh, Messages.AlarmHighLimitInvalid);
                }
                else if (result == ResultForCheckTempWhetherExceedDefinedRange.TooLow)
                {
                    pbHigh.Visible = true;
                    Common.SetToolTip(wrongTip, pbHigh, Messages.AlarmLowLimitInvalid);
                }
                else
                {
                    // nothing
                }
            }

            if (cbLow.Checked && !pbLow.Visible)
            {
                ResultForCheckTempWhetherExceedDefinedRange result = ConnectionController.IsOverTemp(Tag.DeviceID, tbLow.Text, configUnit);
                if (result == ResultForCheckTempWhetherExceedDefinedRange.TooHigh)
                {
                    pbLow.Visible = true;
                    Common.SetToolTip(wrongTip, pbLow, Messages.AlarmHighLimitInvalid);
                }
                else if (result == ResultForCheckTempWhetherExceedDefinedRange.TooLow)
                {
                    pbLow.Visible = true;
                    Common.SetToolTip(wrongTip, pbLow, Messages.AlarmLowLimitInvalid);
                }
                else
                {
                    // nothing
                }
            }

            if (cbHigh.Checked && cbLow.Checked && !pbHigh.Visible && !pbLow.Visible)
            {
                if (!string.IsNullOrEmpty(tbHigh.Text) && Convert.ToDouble(tbLow.Text) >= Convert.ToDouble(tbHigh.Text))
                {
                    pbHigh.Visible = true;
                    pbLow.Visible = true;
                    Common.SetToolTip(wrongTip, pbLow, Messages.HighLimitMoreThanLowLimit);
                    Common.SetToolTip(wrongTip, pbHigh, Messages.HighLimitMoreThanLowLimit);
                }
            }

        }
        

        private static void checkAllMultiAlarmTempFields(TextBox tbA1Temp, TextBox tbA3Temp, TextBox tbA4Temp, TextBox tbA5Temp, TextBox tbA6Temp, PictureBox pbA1, PictureBox pbA2, PictureBox pbA4, PictureBox pbA5, PictureBox pbA6
            , ToolTip wrongTip, SuperDevice Tag, string configUnit, CheckBox cbA6, CheckBox cbA5, CheckBox cbA1)
        {
            string pattern = "^[-+]?[0-9]+[/.]?[0-9]?$";
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);

            if (cbA1.Checked)
            {
                if (!regex.IsMatch(tbA1Temp.Text))
                {
                    pbA1.Visible = true;
                    Common.SetToolTip(wrongTip, pbA1, Messages.TemperatureValueInvalid);
                }
                else
                {
                    pbA1.Visible = false;
                }
            }

            if (!regex.IsMatch(tbA3Temp.Text))
            {
                pbA2.Visible = true;
                Common.SetToolTip(wrongTip, pbA2, Messages.TemperatureValueInvalid);
            }
            else
            {
                pbA2.Visible = false;
            }

            if (!regex.IsMatch(tbA4Temp.Text))
            {
                pbA4.Visible = true;
                Common.SetToolTip(wrongTip, pbA4, Messages.TemperatureValueInvalid);
            }
            else
            {
                pbA4.Visible = false;
            }


            if (cbA5.Checked)
            {
                if (!regex.IsMatch(tbA5Temp.Text))
                {
                    pbA5.Visible = true;
                    Common.SetToolTip(wrongTip, pbA5, Messages.TemperatureValueInvalid);
                }
                else
                {
                    pbA5.Visible = false;
                }
            }


            if (cbA6.Checked)
            {
                if (!regex.IsMatch(tbA6Temp.Text))
                {
                    pbA6.Visible = true;
                    Common.SetToolTip(wrongTip, pbA6, Messages.TemperatureValueInvalid);
                }
                else
                {
                    pbA6.Visible = false;
                }
            }

            if (cbA6.Checked)
            {
                if (!regex.IsMatch(tbA6Temp.Text))
                {
                    pbA6.Visible = true;
                    Common.SetToolTip(wrongTip, pbA6, Messages.TemperatureValueInvalid);
                }
                else
                {
                    pbA6.Visible = false;
                }
            }


            if (cbA1.Checked && !pbA1.Visible)
            {
                ResultForCheckTempWhetherExceedDefinedRange result = ConnectionController.IsOverTemp(Tag.DeviceID, tbA1Temp.Text, configUnit);
                if (result == ResultForCheckTempWhetherExceedDefinedRange.TooHigh)
                {
                    pbA1.Visible = true;
                    Common.SetToolTip(wrongTip, pbA1, Messages.AlarmHighLimitInvalid);
                }
                else if (result == ResultForCheckTempWhetherExceedDefinedRange.TooLow)
                {
                    pbA1.Visible = true;
                    Common.SetToolTip(wrongTip, pbA1, Messages.AlarmLowLimitInvalid);
                }
                else
                {
                    // nothing
                }
            }

            if (!pbA2.Visible)
            {
                ResultForCheckTempWhetherExceedDefinedRange result = ConnectionController.IsOverTemp(Tag.DeviceID, tbA3Temp.Text, configUnit);
                if (result == ResultForCheckTempWhetherExceedDefinedRange.TooHigh)
                {
                    pbA2.Visible = true;
                    Common.SetToolTip(wrongTip, pbA2, Messages.AlarmHighLimitInvalid);
                }
                else if (result == ResultForCheckTempWhetherExceedDefinedRange.TooLow)
                {
                    pbA2.Visible = true;
                    Common.SetToolTip(wrongTip, pbA2, Messages.AlarmLowLimitInvalid);
                }
                else
                {
                    // nothing
                }
            }


            if (!pbA4.Visible)
            {
                ResultForCheckTempWhetherExceedDefinedRange result = ConnectionController.IsOverTemp(Tag.DeviceID, tbA4Temp.Text, configUnit);
                if (result == ResultForCheckTempWhetherExceedDefinedRange.TooHigh)
                {
                    pbA4.Visible = true;
                    Common.SetToolTip(wrongTip, pbA4, Messages.AlarmHighLimitInvalid);
                }
                else if (result == ResultForCheckTempWhetherExceedDefinedRange.TooLow)
                {
                    pbA4.Visible = true;
                    Common.SetToolTip(wrongTip, pbA4, Messages.AlarmLowLimitInvalid);
                }
                else
                {
                    // nothing
                }
            }


            if (cbA5.Checked && !pbA5.Visible)
            {
                ResultForCheckTempWhetherExceedDefinedRange result = ConnectionController.IsOverTemp(Tag.DeviceID, tbA5Temp.Text, configUnit);
                if (result == ResultForCheckTempWhetherExceedDefinedRange.TooHigh)
                {
                    pbA5.Visible = true;
                    Common.SetToolTip(wrongTip, pbA5, Messages.AlarmHighLimitInvalid);
                }
                else if (result == ResultForCheckTempWhetherExceedDefinedRange.TooLow)
                {
                    pbA5.Visible = true;
                    Common.SetToolTip(wrongTip, pbA5, Messages.AlarmLowLimitInvalid);
                }
                else
                {
                    // nothing
                }
            }

            if (cbA6.Checked && !pbA6.Visible)
            {
                ResultForCheckTempWhetherExceedDefinedRange result = ConnectionController.IsOverTemp(Tag.DeviceID, tbA6Temp.Text, configUnit);
                if (result == ResultForCheckTempWhetherExceedDefinedRange.TooHigh)
                {
                    pbA6.Visible = true;
                    Common.SetToolTip(wrongTip, pbA6, Messages.AlarmHighLimitInvalid);
                }
                else if (result == ResultForCheckTempWhetherExceedDefinedRange.TooLow)
                {
                    pbA6.Visible = true;
                    Common.SetToolTip(wrongTip, pbA6, Messages.AlarmLowLimitInvalid);
                }
                else
                {
                    // nothing
                }
            }
            
            if (cbA1.Checked && !pbA1.Visible)
            {
                if (!pbA2.Visible && !string.IsNullOrEmpty(tbA1Temp.Text) && !string.IsNullOrEmpty(tbA3Temp.Text)
                            && Convert.ToDouble(tbA1Temp.Text) >= Convert.ToDouble(tbA3Temp.Text))
                {
                    pbA1.Visible = true;
                    pbA2.Visible = true;
                    Common.SetToolTip(wrongTip, pbA1, string.Format("A{0} >= A{1}: {2}", "5", "4", Messages.HighLimitMoreThanLowLimit));
                    Common.SetToolTip(wrongTip, pbA2, string.Format("A{0} >= A{1}: {2}", "5", "4", Messages.HighLimitMoreThanLowLimit));
                }
                else if (!pbA4.Visible && !string.IsNullOrEmpty(tbA4Temp.Text) && Convert.ToDouble(tbA1Temp.Text) >= Convert.ToDouble(tbA4Temp.Text))
                {
                    pbA1.Visible = true;
                    pbA4.Visible = true;
                    Common.SetToolTip(wrongTip, pbA1, "Alarm limit over A3");
                    Common.SetToolTip(wrongTip, pbA1, string.Format("A{0} >= A{1}: {2}", "5", "3", Messages.HighLimitMoreThanLowLimit));
                    Common.SetToolTip(wrongTip, pbA4, string.Format("A{0} >= A{1}: {2}", "5", "3", Messages.HighLimitMoreThanLowLimit));
                }
                else if (!pbA5.Visible && cbA5.Checked && !string.IsNullOrEmpty(tbA5Temp.Text) && Convert.ToDouble(tbA1Temp.Text) >= Convert.ToDouble(tbA5Temp.Text))
                {
                    pbA1.Visible = true;
                    pbA5.Visible = true;
                    Common.SetToolTip(wrongTip, pbA1, "Alarm limit over A2");
                    Common.SetToolTip(wrongTip, pbA1, string.Format("A{0} >= A{1}: {2}", "5", "2", Messages.HighLimitMoreThanLowLimit));
                    Common.SetToolTip(wrongTip, pbA5, string.Format("A{0} >= A{1}: {2}", "5", "2", Messages.HighLimitMoreThanLowLimit));
                }
                else if (!pbA6.Visible && cbA6.Checked && !string.IsNullOrEmpty(tbA6Temp.Text) && Convert.ToDouble(tbA1Temp.Text) >= Convert.ToDouble(tbA6Temp.Text))
                {
                    pbA1.Visible = true;
                    pbA6.Visible = true;
                    Common.SetToolTip(wrongTip, pbA1, "Alarm limit over A1");
                    Common.SetToolTip(wrongTip, pbA1, string.Format("A{0} >= A{1}: {2}", "5", "1", Messages.HighLimitMoreThanLowLimit));
                    Common.SetToolTip(wrongTip, pbA6, string.Format("A{0} >= A{1}: {2}", "5", "1", Messages.HighLimitMoreThanLowLimit));
                }
            }

            if (!pbA2.Visible)
            {
                if (!pbA4.Visible && !string.IsNullOrEmpty(tbA5Temp.Text) && Convert.ToDouble(tbA3Temp.Text) >= Convert.ToDouble(tbA4Temp.Text))
                {
                    pbA2.Visible = true;
                    pbA4.Visible = true;
                    Common.SetToolTip(wrongTip, pbA2, "Alarm limit over A3");
                    Common.SetToolTip(wrongTip, pbA2, string.Format("A{0} >= A{1}: {2}", "4", "3", Messages.HighLimitMoreThanLowLimit));
                    Common.SetToolTip(wrongTip, pbA4, string.Format("A{0} >= A{1}: {2}", "4", "3", Messages.HighLimitMoreThanLowLimit));
                }
                else if (!pbA5.Visible && cbA5.Checked && !string.IsNullOrEmpty(tbA5Temp.Text) && Convert.ToDouble(tbA3Temp.Text) >= Convert.ToDouble(tbA5Temp.Text))
                {
                    pbA2.Visible = true;
                    pbA5.Visible = true;
                    Common.SetToolTip(wrongTip, pbA2, "Alarm limit over A2");
                    Common.SetToolTip(wrongTip, pbA2, string.Format("A{0} >= A{1}: {2}", "4", "2", Messages.HighLimitMoreThanLowLimit));
                    Common.SetToolTip(wrongTip, pbA5, string.Format("A{0} >= A{1}: {2}", "4", "2", Messages.HighLimitMoreThanLowLimit));
                }
                else if (!pbA6.Visible && cbA6.Checked && !string.IsNullOrEmpty(tbA6Temp.Text) && Convert.ToDouble(tbA3Temp.Text) >= Convert.ToDouble(tbA6Temp.Text))
                {
                    pbA2.Visible = true;
                    pbA6.Visible = true;
                    Common.SetToolTip(wrongTip, pbA2, "Alarm limit over A1");
                    Common.SetToolTip(wrongTip, pbA2, string.Format("A{0} >= A{1}: {2}", "4", "1", Messages.HighLimitMoreThanLowLimit));
                    Common.SetToolTip(wrongTip, pbA6, string.Format("A{0} >= A{1}: {2}", "4", "1", Messages.HighLimitMoreThanLowLimit));
                }
            }

            if (!pbA4.Visible)
            {
                if (!pbA5.Visible && cbA5.Checked && !string.IsNullOrEmpty(tbA5Temp.Text) && Convert.ToDouble(tbA4Temp.Text) >= Convert.ToDouble(tbA5Temp.Text))
                {
                    pbA4.Visible = true;
                    pbA5.Visible = true;
                    Common.SetToolTip(wrongTip, pbA4, "Alarm limit over A2");
                    Common.SetToolTip(wrongTip, pbA4, string.Format("A{0} >= A{1}: {2}", "3", "2", Messages.HighLimitMoreThanLowLimit));
                    Common.SetToolTip(wrongTip, pbA5, string.Format("A{0} >= A{1}: {2}", "3", "2", Messages.HighLimitMoreThanLowLimit));
                }
                else if (!pbA6.Visible && cbA6.Checked && !string.IsNullOrEmpty(tbA6Temp.Text) && Convert.ToDouble(tbA4Temp.Text) >= Convert.ToDouble(tbA6Temp.Text))
                {
                    pbA4.Visible = true;
                    pbA6.Visible = true;
                    Common.SetToolTip(wrongTip, pbA4, "Alarm limit over A1");
                    Common.SetToolTip(wrongTip, pbA4, string.Format("A{0} >= A{1}: {2}", "3", "1", Messages.HighLimitMoreThanLowLimit));
                    Common.SetToolTip(wrongTip, pbA6, string.Format("A{0} >= A{1}: {2}", "3", "1", Messages.HighLimitMoreThanLowLimit));
                }
            }

            if (cbA5.Checked && !pbA5.Visible)
            {
                if (!pbA6.Visible && cbA6.Checked && !string.IsNullOrEmpty(tbA6Temp.Text) && Convert.ToDouble(tbA5Temp.Text) >= Convert.ToDouble(tbA6Temp.Text))
                {
                    pbA5.Visible = true;
                    pbA6.Visible = true;
                    Common.SetToolTip(wrongTip, pbA5, "Alarm limit below A1");
                    Common.SetToolTip(wrongTip, pbA5, string.Format("A{0} >= A{1}: {2}", "2", "1", Messages.HighLimitMoreThanLowLimit));
                    Common.SetToolTip(wrongTip, pbA6, string.Format("A{0} >= A{1}: {2}", "2", "1", Messages.HighLimitMoreThanLowLimit));
                }
            }

            
        }

        public static void VerifyTheTempCfg(TextBox tb, bool isRight, TextBox tbA1Temp, TextBox tbA3Temp
            , TextBox tbA4Temp, TextBox tbA5Temp, TextBox tbA6Temp, TextBox tbHigh, TextBox tbLow
            , PictureBox pbA1, PictureBox pbA2, PictureBox pbA4, PictureBox pbA5, PictureBox pbA6
            , PictureBox pbHigh, PictureBox pbLow, ToolTip wrongTip, SuperDevice Tag, string configUnit, CheckBox cbHigh, CheckBox cbLow, CheckBox cbA6, CheckBox cbA5, CheckBox cbA1, RadioButton rbSingleAlarm, RadioButton rbMultiAlarm)
        {
            if (rbSingleAlarm.Checked)
            {
                checkAllSingleAlarmTempFields(tbHigh, tbLow, pbHigh, pbLow, wrongTip, Tag, configUnit, cbHigh, cbLow);
            }
            if (rbMultiAlarm.Checked)
            {
                checkAllMultiAlarmTempFields(tbA1Temp, tbA3Temp, tbA4Temp, tbA5Temp, tbA6Temp, pbA1, pbA2, pbA4, pbA5, pbA6, wrongTip, Tag, configUnit, cbA6, cbA5, cbA1);    
            }
        }

        public static void GetTextChange(object sender,EventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb != null)
            {
                string pattern = "^[-+]?[0-9]*[.]?[0-9]?$";
                System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(pattern);
                string text = string.Empty;
                switch (tb.Name)
                {
                    case "tbA1Temp":
                        text = _A1;
                        break;
                    case "tbA3Temp":
                        text = _A3;
                        break;
                    case "tbA4Temp":
                        text = _A4;
                        break;
                    case "tbA5Temp":
                        text = _A5;
                        break;
                    case "tbA6Temp":
                        text = _A6;
                        break;
                    case "tbHigh":
                        text = _High;
                        break;
                    case "tbLow":
                        text = _Low;
                        break;
                }
                if (regex.IsMatch(tb.Text))
                {
                    text = tb.Text;
                    switch (tb.Name)
                    {
                        case "tbA1Temp":
                            _A1=text;
                            break;
                        case "tbA3Temp":
                            _A3= text ;
                            break;
                        case "tbA4Temp":
                            _A4=text ;
                            break;
                        case "tbA5Temp":
                             _A5=text ;
                            break;
                        case "tbA6Temp":
                              _A6=text;
                            break;
                        case "tbHigh":
                             _High=text ;
                            break;
                        case "tbLow":
                              _Low=text;
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
        public static void ShowStopDevice()
        {
            Babu.Windows.Forms.MessageBox mb = new Babu.Windows.Forms.MessageBox();
            string path = Path.Combine(Application.StartupPath, "tipsparams.xml");
            mb.Buttons = MessageBoxButtons.OK;
            mb.Caption = Messages.TitleNotification;
            mb.CheckBoxEnabled = true;
            if (!File.Exists(path))
            {

                mb.Checked = false;
            }
            else
            {
                XElement xdoc = XElement.Load(path);
                var v = from c in xdoc.Elements()
                        where c.Name == "ask"
                        select c;
                if (v != null && v.ToList().Count > 0)
                    mb.Checked = Convert.ToBoolean(v.First().Value);
                if (mb.Checked)
                    return;
            }
            mb.IsTimedOut = false;
            mb.Message = Messages.ConfigTips;
            mb.Icon = MessageBoxIcon.Information;
            mb.CheckBoxText = "Do not show this tip any more";
            if (!mb.Checked)
            {
                if (DialogResult.OK == mb.ShowDialog())
                {
                    if (!File.Exists(path))
                    {
                        XElement config = new XElement("params", new XElement("ask", mb.Checked));
                        config.Save(path);

                    }
                    else
                    {
                        XElement xdoc = XElement.Load(path);
                        var v = from c in xdoc.Elements()
                                where c.Name == "ask"
                                select c;
                        v.First().SetValue(mb.Checked);
                        xdoc.Save(path);
                    }
                }
            }
        }
        public static void SetSignButtonState(Button sign,bool isEnable)
        {
            sign.Enabled = isEnable;
        }
        public static void SetSaveButtonState(Button save, bool isEnable)
        {
            save.Enabled = isEnable;
        }
        public static void SetCommentTextState(TextBox tb, bool isEnable)
        {
            tb.Enabled = isEnable;
        }
        public static void SetSignPanelState(Panel panel, bool isEnable)
        {
            panel.Enabled = isEnable;
        }
        public static void DeviceManagerExitDialog()
        {

        }
    }

    public enum ResultForCheckTempWhetherExceedDefinedRange { TooHigh, TooLow, Error, Correct }
}
