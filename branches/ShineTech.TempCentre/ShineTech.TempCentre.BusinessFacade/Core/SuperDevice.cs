using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ShineTech.TempCentre.DAL;
using System.Drawing;
using System.Xml.Serialization;
using TempSen;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
namespace ShineTech.TempCentre.BusinessFacade
{
    [Serializable]
    [XmlInclude(typeof(ITAGSingleUse)), XmlInclude(typeof(Tempod)), XmlInclude(typeof(ELog)), XmlInclude(typeof(ITAGPDF))]
    public abstract class SuperDevice : IShowDevice
    {
        #region abstract property
        public abstract string ProductName { get; set; }
        public abstract string Model { get; set; }
        public abstract string Description { get; set; }
        public abstract string TripNumber { get; set; }
        public abstract string StartModel { get; set; }
        public abstract string HighAlarmType { get; set; }
        public abstract string LowAlarmType { get; set; }
        public abstract string CurrentStatus { get; set; }
        public abstract string DeviceName { get; set; }
        public abstract int DeviceID { get; set; }
        #endregion
        #region fileds
        private string _SerialNumber;

        public string SerialNumber
        {
            get { return _SerialNumber; }
            set { _SerialNumber = value; }
        }
        private int _Memory;

        public int Memory
        {
            get { return _Memory; }
            set { _Memory = value; }
        }
        private string _Battery;

        public string Battery
        {
            get { return _Battery; }
            set { _Battery = value; }
        }
        private string _LogCycle;

        public string LogCycle
        {
            get { return _LogCycle; }
            set { _LogCycle = value; }
        }
        private string _LogInterval;

        public string LogInterval
        {
            get { return _LogInterval; }
            set { _LogInterval = value; }
        }
        private string _LogStartDelay;

        public string LogStartDelay
        {
            get { return _LogStartDelay; }
            set { _LogStartDelay = value; }
        }
        private string _AlarmHighLimit;

        public string AlarmHighLimit
        {
            get { return _AlarmHighLimit; }
            set { _AlarmHighLimit = value; }
        }
        private string _AlarmLowLimit;

        public string AlarmLowLimit
        {
            get { return _AlarmLowLimit; }
            set { _AlarmLowLimit = value; }
        }
        private string _AlarmHighDelay;

        public string AlarmHighDelay
        {
            get { return _AlarmHighDelay; }
            set { _AlarmHighDelay = value; }
        }
        private string _AlarmLowDelay;

        public string AlarmLowDelay
        {
            get { return _AlarmLowDelay; }
            set { _AlarmLowDelay = value; }
        }
        private DateTime _LoggingStart;

        public DateTime LoggingStart
        {
            get { return _LoggingStart; }
            set { _LoggingStart = value; }
        }
        private DateTime _LoggingEnd;

        public DateTime LoggingEnd
        {
            get { return _LoggingEnd; }
            set { _LoggingEnd = value; }
        }
        private DateTime _StartConditionTime;

        public DateTime StartConditionTime
        {
            get { return _StartConditionTime; }
            set { _StartConditionTime = value; }
        }
        private string _TripLength;

        public string TripLength
        {
            get { return _TripLength; }
            set { _TripLength = value; }
        }
        private int _DataPoints;

        public int DataPoints
        {
            get { return _DataPoints; }
            set { _DataPoints = value; }
        }
        private string _HighestC;

        public string HighestC
        {
            get { return _HighestC; }
            set { _HighestC = value; }
        }
        private string _LowestC;

        public string LowestC
        {
            get { return _LowestC; }
            set { _LowestC = value; }
        }
        private string _AverageC;

        public string AverageC
        {
            get { return _AverageC; }
            set { _AverageC = value; }
        }
        private string _MKT;

        public string MKT
        {
            get { return _MKT; }
            set { _MKT = value; }
        }
        private int _HighAlarmEvents;

        public int HighAlarmEvents
        {
            get { return _HighAlarmEvents; }
            set { _HighAlarmEvents = value; }
        }
        private int _LowAlarmEvents;

        public int LowAlarmEvents
        {
            get { return _LowAlarmEvents; }
            set { _LowAlarmEvents = value; }
        }
        private string _HighAlarmTotalTimeAbove;

        public string HighAlarmTotalTimeAbove
        {
            get { return _HighAlarmTotalTimeAbove; }
            set { _HighAlarmTotalTimeAbove = value; }
        }
        private string _LowAlarmTotalTimeBelow;

        public string LowAlarmTotalTimeBelow
        {
            get { return _LowAlarmTotalTimeBelow; }
            set { _LowAlarmTotalTimeBelow = value; }
        }
        private string _HighAlarmLongestAbove;

        public string HighAlarmLongestAbove
        {
            get { return _HighAlarmLongestAbove; }
            set { _HighAlarmLongestAbove = value; }
        }
        private string _LowAlarmLongestBelow;

        public string LowAlarmLongestBelow
        {
            get { return _LowAlarmLongestBelow; }
            set { _LowAlarmLongestBelow = value; }
        }
        private string _HighAlarmFirstTrigged;

        public string HighAlarmFirstTrigged
        {
            get { return _HighAlarmFirstTrigged; }
            set { _HighAlarmFirstTrigged = value; }
        }
        private string _LowAlarmFirstTrigged;

        public string LowAlarmFirstTrigged
        {
            get { return _LowAlarmFirstTrigged; }
            set { _LowAlarmFirstTrigged = value; }
        }
        [XmlIgnore]
        public string[] Config;
        [XmlIgnore]
        public string[] AlarmSet;
        [XmlIgnore]
        public string[] Analysis;
        [XmlIgnore]
        public string[] Record;
        [XmlIgnore]
        public string[] OtherInfo;
        private PointInfo _points = new PointInfo();

        public PointInfo points
        {
            get { return _points; }
            set { _points = value; }
        }
        private List<PointKeyValue> _tempList = new List<PointKeyValue>();
        public List<PointKeyValue> tempList
        {
            get { return _tempList; }
            set { _tempList = value; }
        }

        private string _TempUnit;

        public string TempUnit
        {
            get { return _TempUnit; }
            set { _TempUnit = value; }
        }
        private int _AlarmMode;

        public int AlarmMode
        {
            get { return _AlarmMode; }
            set { _AlarmMode = value; }
        }
        private string _a1;
        private string _a2;

        public string A2
        {
            get { return _a2; }
            set { _a2 = value; }
        }
        private string _a3;

        public string A3
        {
            get { return _a3; }
            set { _a3 = value; }
        }
        private string _a4;

        public string A4
        {
            get { return _a4; }
            set { _a4 = value; }
        }
        private string _a5;

        public string A5
        {
            get { return _a5; }
            set { _a5 = value; }
        }

        public string A1
        {
            get { return _a1; }
            set { _a1 = value; }
        }
        private int _alarmDelayA1;

        public int AlarmDelayA1
        {
            get { return _alarmDelayA1; }
            set { _alarmDelayA1 = value; }
        }
        private int _alarmDelayA2;

        public int AlarmDelayA2
        {
            get { return _alarmDelayA2; }
            set { _alarmDelayA2 = value; }
        }
        private int _alarmDelayA3;

        public int AlarmDelayA3
        {
            get { return _alarmDelayA3; }
            set { _alarmDelayA3 = value; }
        }
        private int _alarmDelayA4;

        public int AlarmDelayA4
        {
            get { return _alarmDelayA4; }
            set { _alarmDelayA4 = value; }
        }
        private int _alarmDelayA5;

        public int AlarmDelayA5
        {
            get { return _alarmDelayA5; }
            set { _alarmDelayA5 = value; }
        }
        private string _alarmTypeA1;

        public string AlarmTypeA1
        {
            get { return _alarmTypeA1; }
            set { _alarmTypeA1 = value; }
        }
        private string _alarmTypeA2;

        public string AlarmTypeA2
        {
            get { return _alarmTypeA2; }
            set { _alarmTypeA2 = value; }
        }
        private string _alarmTypeA3;

        public string AlarmTypeA3
        {
            get { return _alarmTypeA3; }
            set { _alarmTypeA3 = value; }
        }
        private string _alarmTypeA4;

        public string AlarmTypeA4
        {
            get { return _alarmTypeA4; }
            set { _alarmTypeA4 = value; }
        }
        private string _alarmTypeA5;

        public string AlarmTypeA5
        {
            get { return _alarmTypeA5; }
            set { _alarmTypeA5 = value; }
        }
        private string _alarmTotalTimeA1;

        public string AlarmTotalTimeA1
        {
            get { return _alarmTotalTimeA1; }
            set { _alarmTotalTimeA1 = value; }
        }
        private string _alarmTotalTimeA2;

        public string AlarmTotalTimeA2
        {
            get { return _alarmTotalTimeA2; }
            set { _alarmTotalTimeA2 = value; }
        }
        private string _alarmTotalTimeA3;

        public string AlarmTotalTimeA3
        {
            get { return _alarmTotalTimeA3; }
            set { _alarmTotalTimeA3 = value; }
        }
        private string _alarmTotalTimeA4;

        public string AlarmTotalTimeA4
        {
            get { return _alarmTotalTimeA4; }
            set { _alarmTotalTimeA4 = value; }
        }
        private string _alarmTotalTimeA5;

        public string AlarmTotalTimeA5
        {
            get { return _alarmTotalTimeA5; }
            set { _alarmTotalTimeA5 = value; }
        }
        private string _alarmTotalTimeIdeal;

        public string AlarmTotalTimeIdeal
        {
            get { return _alarmTotalTimeIdeal; }
            set { _alarmTotalTimeIdeal = value; }
        }
        private int _alarmNumA1;
        private int _alarmNumA2;

        public int AlarmNumA2
        {
            get { return _alarmNumA2; }
            set { _alarmNumA2 = value; }
        }
        private int _alarmNumA3;

        public int AlarmNumA3
        {
            get { return _alarmNumA3; }
            set { _alarmNumA3 = value; }
        }
        private int _alarmNumA4;

        public int AlarmNumA4
        {
            get { return _alarmNumA4; }
            set { _alarmNumA4 = value; }
        }
        private int _alarmNumA5;

        public int AlarmNumA5
        {
            get { return _alarmNumA5; }
            set { _alarmNumA5 = value; }
        }

        public int AlarmNumA1
        {
            get { return _alarmNumA1; }
            set { _alarmNumA1 = value; }
        }
        private string _alarmA1First;

        public string AlarmA1First
        {
            get { return _alarmA1First; }
            set { _alarmA1First = value; }
        }
        private string _alarmA2First;

        public string AlarmA2First
        {
            get { return _alarmA2First; }
            set { _alarmA2First = value; }
        }
        private string _alarmA3First;

        public string AlarmA3First
        {
            get { return _alarmA3First; }
            set { _alarmA3First = value; }
        }
        private string _alarmA4First;

        public string AlarmA4First
        {
            get { return _alarmA4First; }
            set { _alarmA4First = value; }
        }
        private string _alarmA5First;

        public string AlarmA5First
        {
            get { return _alarmA5First; }
            set { _alarmA5First = value; }
        }
        private string _alarmA1Status;
        private string _alarmA2Status;

        public string AlarmA2Status
        {
            get { return _alarmA2Status; }
            set { _alarmA2Status = value; }
        }
        private string _alarmA3Status;

        public string AlarmA3Status
        {
            get { return _alarmA3Status; }
            set { _alarmA3Status = value; }
        }
        private string _alarmA4Status;

        public string AlarmA4Status
        {
            get { return _alarmA4Status; }
            set { _alarmA4Status = value; }
        }
        private string _alarmA5Status;

        public string AlarmA5Status
        {
            get { return _alarmA5Status; }
            set { _alarmA5Status = value; }
        }

        public string AlarmA1Status
        {
            get { return _alarmA1Status; }
            set { _alarmA1Status = value; }
        }
        private string _alarmHighStatus = "";

        public string AlarmHighStatus
        {
            get { return _alarmHighStatus; }
            set { _alarmHighStatus = value; }
        }
        private string _alarmLowStatus = "";

        public string AlarmLowStatus
        {
            get { return _alarmLowStatus; }
            set { _alarmLowStatus = value; }
        }
        private int _RunStatus = 3;

        public int RunStatus
        {
            get { return _RunStatus; }
            set { _RunStatus = value; }
        }

        public string LoggerRead { get; set; }

        private byte[] _ReportGraph;
        [XmlIgnoreAttribute]
        public byte[] ReportGraph
        {
            get { return _ReportGraph; }
            set { _ReportGraph = value; }
        }
        #endregion
        #region method
        public SuperDevice() {
            Application.CurrentCulture = CultureInfo.InvariantCulture;
        }
        /// <summary>
        /// 默认连接tempsen single-tag
        /// </summary>
        /// <returns></returns>
        public virtual bool Connect(int code)
        {
            if ((DeviceType)code != DeviceType.ITAGSingleUse)
            {
                return ConnectInit(code);
            }
            else
                return false;
        }
        public virtual bool Auto(int code)
        {
            return Connect(code);
        }
        public virtual DeviceType Auto()
        {
            return GetModelFromDevice();
        }
        public static DeviceType GetModelFromDevice()
        {
            DeviceType dt = DeviceType.ITAGPDF;
            try
            {
                if (ObjectManage.DeviceSingleUse == null)
                    ObjectManage.DeviceSingleUse = new TempSen.device((int)DeviceType.ITAGSingleUse);
                if (ObjectManage.DeviceSingleUse.connectDevice())
                {
                    dt = DeviceType.ITAGSingleUse;
                    //ObjectManage.DeviceNew.disconnectDevice();
                }
                else
                {
                    bool result = true,isconnect=false;
                    if (ObjectManage.DeviceNew == null || ObjectManage.DeviceNew.Data == null)
                    {
                        ObjectManage.DeviceNew = new DevicePDF();
                        isconnect = result = ObjectManage.DeviceNew.connectDevice();
                    }
                    if (result)
                    {
                        switch (ObjectManage.DeviceNew.Data.DevModel)
                        {
                            case "0100":
                                dt = DeviceType.ITAGPDF;
                                break;
                            case "0101":
                            case "0102":
                                dt = DeviceType.ITAG3;
                                break;
                            //case "0102":
                            //    dt = DeviceType.ITAG3Pro;
                            //    break;
                            case "0200":
                            case "0201":
                            case "0202":
                            case "0203":
                                dt = DeviceType.Tempod;
                                break;
                            //case "0201":
                            //    dt = DeviceType.TempodS;
                            //    break;
                            //case "0202":
                            //    dt = DeviceType.TempodPro;
                            //    break;
                            //case "0203":
                            //    dt = DeviceType.TempodProS;
                            //    break;
                            //case "0300":
                            //    dt = DeviceType.ELogTE;
                            //    break;
                            case "0301":
                            case "0300":
                                dt = DeviceType.ELogTI;
                                break;
                            default:
                                dt = DeviceType.ITAGPDF;
                                break;
                        }
                        //pdf.disconnectDevice();
                    }
                }
                return dt;
            }
            catch
            {
                return dt;
            }
        }
        public virtual DataTable GetDataList()
        {
            DataTable dt = new DataTable();
            return dt;
        }
        public abstract void Summary();
        public abstract SuperDevice Clone(string datetimeFormat);
        public virtual bool WriteConfiguration(ConfigurationProfile cfg)
        {
            //DevicePDF device = new DevicePDF();
            try
            {
                //device = this.Pdf;
                if (ObjectManage.DeviceNew != null)
                {
                    ObjectManage.DeviceNew.Data.TripNo = cfg.Tn;
                    ObjectManage.DeviceNew.Data.Description = cfg.Desc;
                    //ObjectManage.DeviceNew.Data.AlarmType = cfg.
                    ObjectManage.DeviceNew.Data.TemperatureUnit = cfg.TempUnit == "C" ? 1 : 2;
                    ObjectManage.DeviceNew.Data.LogInterval = Convert.ToInt32(cfg.LogIntervalH) * 3600 + Convert.ToInt32(cfg.LogIntervalM) * 60 + Convert.ToInt32(cfg.LogIntervalS);
                    string cycle = TempsenFormatHelper.GetSecondsFromFormatString(cfg.LogCycle);
                    ObjectManage.DeviceNew.Data.LogCycle = Convert.ToInt32(cycle)/3600;//log cycle
                    //if (cycle.Count > 1)
                    //    ObjectManage.DeviceNew.Data.LogCycle += cycle[1];
                    if (cfg.StartMode == "Manual Start")
                    {
                        ObjectManage.DeviceNew.Data.StartDelay = (Convert.ToInt32(cfg.StartDelayD) * 1440 + Convert.ToInt32(cfg.StartDelayH) * 60 + Convert.ToInt32(cfg.StartDelayM));
                        ObjectManage.DeviceNew.Data.StartMode = "F8";
                    }
                    else
                    {
                        ObjectManage.DeviceNew.Data.StartConditionTime = Convert.ToDateTime(cfg.StartDate).ToUniversalTime();//UTC时间写入
                        //ObjectManage.DeviceNew.Data.StartConditionTime = Convert.ToDateTime(cfg.StartDate);
                        ObjectManage.DeviceNew.Data.StartMode = "8F";
                    }
                    if (cfg.IsSingleAlarm)
                    {
                        ObjectManage.DeviceNew.Data.AlarmMode = 1;
                        if (cfg.IsHighLimit)
                        {
                            ObjectManage.DeviceNew.Data.AlarmLimits2 = cfg.HighTemp;
                            ObjectManage.DeviceNew.Data.AlarmType2 = cfg.HighAlarmType == "Single" ? Convert.ToByte(192) : Convert.ToByte(193);
                            ObjectManage.DeviceNew.Data.AlarmDelay2 = cfg.HighDay * 24 * 3600 + cfg.HighH * 3600 + cfg.HighM * 60;
                        }
                        else
                            ObjectManage.DeviceNew.Data.AlarmType2 = Convert.ToByte(0);
                        if (cfg.IsLowLimit)
                        {
                            ObjectManage.DeviceNew.Data.AlarmLimits3 = cfg.LowTemp;
                            ObjectManage.DeviceNew.Data.AlarmType3 = cfg.LowAlarmType == "Single" ? Convert.ToByte(128) : Convert.ToByte(129);
                            ObjectManage.DeviceNew.Data.AlarmDelay3 = cfg.LowDay * 24 * 3600 + cfg.LowH * 3600 + cfg.LowM * 60;
                        }
                        else
                            ObjectManage.DeviceNew.Data.AlarmType3 = Convert.ToByte(0);
                        ObjectManage.DeviceNew.Data.AlarmType0 = Convert.ToByte(0);
                        ObjectManage.DeviceNew.Data.AlarmType1 = Convert.ToByte(0);
                        ObjectManage.DeviceNew.Data.AlarmType4 = Convert.ToByte(0);
                    }
                    else if (cfg.IsMultiAlarm)
                    {
                        ObjectManage.DeviceNew.Data.AlarmMode = 2;
                        if (cfg.IsA6)
                        {
                            ObjectManage.DeviceNew.Data.AlarmLimits0 = cfg.A6Temp;
                            ObjectManage.DeviceNew.Data.AlarmType0 = cfg.A6AlarmType == "Single" ? Convert.ToByte(192) : Convert.ToByte(193);
                            ObjectManage.DeviceNew.Data.AlarmDelay0 = cfg.A6Day * 24 * 3600 + cfg.A6H * 3600 + cfg.A6M * 60;
                        }
                        else
                            ObjectManage.DeviceNew.Data.AlarmType0 = Convert.ToByte(0);
                        if (cfg.IsA5)
                        {
                            ObjectManage.DeviceNew.Data.AlarmLimits1 = cfg.A5Temp;
                            ObjectManage.DeviceNew.Data.AlarmType1 = cfg.A5AlarmType == "Single" ? Convert.ToByte(192) : Convert.ToByte(193);
                            ObjectManage.DeviceNew.Data.AlarmDelay1 = cfg.A5Day * 24 * 3600 + cfg.A5H * 3600 + cfg.A5M * 60;
                        }
                        else
                            ObjectManage.DeviceNew.Data.AlarmType1 =Convert.ToByte(0);
                        ObjectManage.DeviceNew.Data.AlarmLimits2 = cfg.A4Temp;
                        ObjectManage.DeviceNew.Data.AlarmLimits3 = cfg.A3Temp;
                        if (cfg.IsA1)
                        {
                            ObjectManage.DeviceNew.Data.AlarmLimits4 = cfg.A1Temp;
                            ObjectManage.DeviceNew.Data.AlarmType4 = cfg.A1AlarmType == "Single" ? Convert.ToByte(128) : Convert.ToByte(129);
                            ObjectManage.DeviceNew.Data.AlarmDelay4 = cfg.A1Day * 24 * 3600 + cfg.A1H * 3600 + cfg.A1M * 60;
                        }
                        else
                            ObjectManage.DeviceNew.Data.AlarmType4 = Convert.ToByte(0);
                        ObjectManage.DeviceNew.Data.AlarmType2 = cfg.A4AlarmType == "Single" ? Convert.ToByte(192) : Convert.ToByte(193);
                        ObjectManage.DeviceNew.Data.AlarmType3 = cfg.A2AlarmType == "Single" ? Convert.ToByte(128) : Convert.ToByte(129);
                        ObjectManage.DeviceNew.Data.AlarmDelay2 = cfg.A4Day * 24 * 3600 + cfg.A4H * 3600 + cfg.A4M * 60;
                        ObjectManage.DeviceNew.Data.AlarmDelay3 = cfg.A2Day * 24 * 3600 + cfg.A2H * 3600 + cfg.A2M * 60;
                    }
                    else
                        ObjectManage.DeviceNew.Data.AlarmMode = 0;
                    return (ObjectManage.DeviceNew.DoWrite());
                }
                return false;
            }
            catch
            {
                //ObjectManage.DeviceNew.disconnectDevice();
                return false;
            }
        }
        public int CalcMultiAlarmTotalTime(string level)
        {
            int num = CalcAlarmLevelTotalNum(level) * Convert.ToInt32(LogInterval);
            return num;
        }
        public int CalcAlarmLevelTotalNum(string level)
        {
            int num;

            switch (level)
            {
                case "A1":
                    var v = tempList.Where(p => p.PointTemp >= Convert.ToDouble(this.A1)).ToList();
                    num = v.Count;
                    break;
                case "A2":
                    v = tempList.Where(p => p.PointTemp < Convert.ToDouble(this.A1) && p.PointTemp >= Convert.ToDouble(A2)).ToList();
                    num = v.Count;
                    break;
                case "A3":
                    v = tempList.Where(p => p.PointTemp < Convert.ToDouble(this.A2) && p.PointTemp >= Convert.ToDouble(A3)).ToList();
                    num = v.Count;
                    break;
                case "A4":
                    v = tempList.Where(p => p.PointTemp < Convert.ToDouble(this.A4) && p.PointTemp >= Convert.ToDouble(A5)).ToList();
                    num = v.Count;
                    break;
                case "A5":
                    v = tempList.Where(p => p.PointTemp < Convert.ToDouble(this.A5)).ToList();
                    num = v.Count;
                    break;
                default:
                    v = tempList.Where(p => p.PointTemp < Convert.ToDouble(this.A3) && p.PointTemp >= Convert.ToDouble(A4)).ToList();
                    num = v.Count;
                    break;
            }
            return num;
        }
        public string CalcAlarmLevelFirstAlarm(string level)
        {
            string t = "";

            switch (level)
            {
                case "A1":
                    var v = tempList.Where(p => p.PointTemp >= Convert.ToDouble(this.A1)).ToList();
                    t = v.Count == 0 ? "" : v.First().PointTime.ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture);
                    break;
                case "A2":
                    v = tempList.Where(p => p.PointTemp < Convert.ToDouble(this.A1) && p.PointTemp >= Convert.ToDouble(A2)).ToList();
                    t = v.Count == 0 ? "" : v.First().PointTime.ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture);
                    break;
                case "A3":
                    v = tempList.Where(p => p.PointTemp < Convert.ToDouble(this.A2) && p.PointTemp >= Convert.ToDouble(A3)).ToList();
                    t = v.Count == 0 ? "" : v.First().PointTime.ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture);
                    break;
                case "A4":
                    v = tempList.Where(p => p.PointTemp < Convert.ToDouble(this.A4) && p.PointTemp >= Convert.ToDouble(A5)).ToList();
                    t = v.Count == 0 ? "" : v.First().PointTime.ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture);
                    break;
                case "A5":
                    v = tempList.Where(p => p.PointTemp < Convert.ToDouble(this.A5)).ToList();
                    t = v.Count == 0 ? "" : v.First().PointTime.ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture);
                    break;
                default:
                    v = tempList.Where(p => p.PointTemp < Convert.ToDouble(this.A3) && p.PointTemp >= Convert.ToDouble(A4)).ToList();
                    t = v.Count == 0 ? "" : v.First().PointTime.ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture);
                    break;
            }
            return t;
        }
        private bool ConnectInit(int code)
        {
            bool result = false;
            if (ObjectManage.DeviceNew == null||ObjectManage.DeviceNew.Data==null)
            {
                ObjectManage.DeviceNew = new DevicePDF();
                result = ObjectManage.DeviceNew.connectDevice();
            }
            try
            {
                if (ObjectManage.DeviceNew != null)
                {
                    if (Convert.ToInt32(ObjectManage.DeviceNew.Data.DevModel.Substring(0, 3)) != code / 10)
                    {
                        Common.IsConnectCompleted = true;
                        return false;
                    }
                    switch (ObjectManage.DeviceNew.Data.DevModel)
                    {
                        case "0100":
                            ProductName = DeviceName = "ITAG-PDF";
                            Model = "TAGP";
                            DeviceID = 100;
                            Memory = 3600;
                            break;
                        case "0101":
                            ProductName = DeviceName = "ITAG3";
                            Model = "TAGT";
                            DeviceID = 101;
                            Memory = 3600;
                            break;
                        case "0102":
                            ProductName = DeviceName = Model = "ITAG3 Pro";
                            Model = "TAGH";
                            DeviceID = 102;
                            Memory = 3600;
                            break;
                        case "0200":
                            ProductName = DeviceName = Model = "Tempod";
                            Model = "TP15";
                            DeviceID = 200;
                            Memory = 3600;
                            break;
                        case "0201":
                            ProductName = DeviceName = Model = "Tempod";
                            Model = "TP15S";
                            DeviceID = 201;
                            Memory = 3600;
                            break;
                        case "0202":
                            ProductName = DeviceName = Model = "Tempod";
                            Model = "TP25";
                            DeviceID = 202;
                            Memory = 7200;
                            break;
                        case "0203":
                            ProductName = DeviceName = Model = "Tempod";
                            Model = "TP25S";
                            DeviceID = 203;
                            Memory = 7200;
                            break;
                        case "0300":
                            ProductName = DeviceName = Model = "Elog TE";
                            Model = "EL-TE";
                            DeviceID = 300;
                            Memory = 7200;
                            break;
                        case "0301":
                            ProductName = DeviceName = Model = "Elog TI";
                            Model = "EL-TI";
                            DeviceID = 301;
                            Memory = 7200;
                            break;
                        default:
                            ProductName = DeviceName = Model = "TAGS";
                            DeviceID = 100;
                            Memory = 3600;
                            break;
                    }
                    this.Battery = ObjectManage.DeviceNew.Data.Battery >= 255 ? "100" : ObjectManage.DeviceNew.Data.Battery.ToString();
                    this.RunStatus = ObjectManage.DeviceNew.Data.RunStatus;
                    switch (ObjectManage.DeviceNew.Data.RunStatus)
                    {
                        case 0:
                            this.CurrentStatus = "Unconfigured";
                            break;
                        case 1:
                            this.CurrentStatus = "Standby";
                            break;
                        case 2:
                            this.CurrentStatus = "Recording";
                            break;
                        default:
                            this.CurrentStatus = "Stopped";
                            break;
                    }
                    this.SerialNumber = ObjectManage.DeviceNew.Data.DevNo.Trim();
                    if (ObjectManage.DeviceNew.Data.RunStatus != 0)
                    {
                        #region summary
                        this.TripNumber = ObjectManage.DeviceNew.Data.TripNo.Trim() == "" ? SerialNumber : ObjectManage.DeviceNew.Data.TripNo.Trim();
                        this.LogCycle = string.Format("{0}", TempsenFormatHelper.ConvertSencondToFormmatedTime((ObjectManage.DeviceNew.Data.LogCycle * 3600).ToString()));
                        this.LogInterval = ObjectManage.DeviceNew.Data.LogInterval.ToString();
                        if (ObjectManage.DeviceNew.Data.RunStatus != 1&&ObjectManage.DeviceNew.Data.TempListWithTime.Count>0)
                        {
                            this.LoggingStart = ObjectManage.DeviceNew.Data.LogStartTime;
                            this.LoggingEnd = ObjectManage.DeviceNew.Data.LogEndTime;
                            this.TripLength = TempsenFormatHelper.ConvertSencondToFormmatedTime((LoggingEnd-LoggingStart).TotalSeconds.ToString());
                        }
                        this.Description = ObjectManage.DeviceNew.Data.Description;
                        this.TempUnit = ObjectManage.DeviceNew.Data.TemperatureUnit == 1 ? "C" : "F";
                        this.StartModel = ObjectManage.DeviceNew.Data.StartMode == "F8" ? "Manual Start" : "Auto Start";
                        if (ObjectManage.DeviceNew.Data.StartMode == "F8")
                            this.LogStartDelay = TempsenFormatHelper.ConvertSencondToFormmatedTime((ObjectManage.DeviceNew.Data.StartDelay * 60).ToString());
                        else
                            this.StartConditionTime = ObjectManage.DeviceNew.Data.StartConditionTime;
                        this.AlarmMode = ObjectManage.DeviceNew.Data.AlarmMode;
                        DataPoints = ObjectManage.DeviceNew.Data.LogCount;
                        if (ObjectManage.DeviceNew.Data.TempListWithTime.Count>0&&SerializePointInfo(ObjectManage.DeviceNew))
                        {
                            _points.FirstPoint = _points.StartTime = this.LoggingStart;
                            _points.EndTime = this.LoggingEnd;
                            _points.TripLength = this.TripLength;
                            if (tempList != null && tempList.Count > 0)
                            {
                                AverageC = ObjectManage.DeviceNew.Data.AvgTemp;
                                switch (ObjectManage.DeviceNew.Data.DevModel)
                                {
                                    case "0101":
                                    case "0200":
                                    case "0201":
                                        AverageC = Math.Round(tempList.Select(p => p.PointTemp).Average(), 1).ToString();
                                        MKT = Common.CalcMKT(tempList.Select(p => (int)(p.PointTemp * 100)).ToList());
                                        break;
                                    default:
                                        MKT = ObjectManage.DeviceNew.Data.MKT;
                                        break;
                                }
                                //test mkt
                                //Debug.WriteLine(Common.CalcMKT(tempList.Select(p => (int)p.PointTemp * 100).ToList()), "MKT");
                                HighestC = ObjectManage.DeviceNew.Data.MaxTemp + "°" + (this.TempUnit) + "@" + ObjectManage.DeviceNew.Data.MaxTempTime.ToString(Common.GetDefaultDateTimeFormat(), CultureInfo.InvariantCulture);
                                LowestC = ObjectManage.DeviceNew.Data.MinTemp + "°" + (this.TempUnit) + "@" + ObjectManage.DeviceNew.Data.MinTempTime.ToString(Common.GetDefaultDateTimeFormat(), CultureInfo.InvariantCulture);
                                _points.HighestC = this.HighestC;
                                _points.LowestC = this.LowestC;
                                _points.AVGTemp = AverageC;
                                _points.MKT = this.MKT;
                            }
                        }
                        if (ObjectManage.DeviceNew.Data.AlarmMode == 1)
                        {
                            if ((ObjectManage.DeviceNew.Data.AlarmType2 & 0x80) > 0)
                            {
                                AlarmHighLimit = ObjectManage.DeviceNew.Data.AlarmLimits2;
                                AlarmHighDelay = TempsenFormatHelper.ConvertSencondToFormmatedTime(ObjectManage.DeviceNew.Data.AlarmDelay2.ToString());
                                HighAlarmType = Convert.ToInt32(ObjectManage.DeviceNew.Data.AlarmType2 & 0x01) > 0 ? "Cumulative" : "Single";
                                if (ObjectManage.DeviceNew.Data.RunStatus != 1)
                                {
                                    AlarmHighStatus = ObjectManage.DeviceNew.Data.AllZoneAlartStatusArray[2] > 0 ? "Alarm" : "OK";
                                    HighAlarmTotalTimeAbove = TempsenFormatHelper.ConvertSencondToFormmatedTime(ObjectManage.DeviceNew.Data.AllZoneOverTempTimeStatArray[2].ToString());
                                    if(ObjectManage.DeviceNew.Data.AllZoneAlartStatusArray[2] > 0)
                                        HighAlarmFirstTrigged = ObjectManage.DeviceNew.Data.FirstAlarmTimeArray[2].ToString(Common.GetDefaultDateTimeFormat(), CultureInfo.InvariantCulture);
                                    HighAlarmEvents = ObjectManage.DeviceNew.Data.AllZoneOverTempTimesStatArray[2];
                                }
                            }
                            if ((ObjectManage.DeviceNew.Data.AlarmType3 & 0x80) > 0)
                            {
                                AlarmLowLimit = ObjectManage.DeviceNew.Data.AlarmLimits3;
                                AlarmLowDelay = TempsenFormatHelper.ConvertSencondToFormmatedTime(ObjectManage.DeviceNew.Data.AlarmDelay3.ToString());
                                LowAlarmType = Convert.ToInt32(ObjectManage.DeviceNew.Data.AlarmType3 & 0x01) > 0 ? "Cumulative" : "Single";
                                if (ObjectManage.DeviceNew.Data.RunStatus != 1)
                                {
                                    AlarmLowStatus = ObjectManage.DeviceNew.Data.AllZoneAlartStatusArray[3] > 0 ? "Alarm" : "OK";
                                    LowAlarmTotalTimeBelow = TempsenFormatHelper.ConvertSencondToFormmatedTime(ObjectManage.DeviceNew.Data.AllZoneOverTempTimeStatArray[3].ToString());
                                    if (ObjectManage.DeviceNew.Data.AllZoneAlartStatusArray[3] > 0)
                                        LowAlarmFirstTrigged = ObjectManage.DeviceNew.Data.FirstAlarmTimeArray[3].ToString(Common.GetDefaultDateTimeFormat(), CultureInfo.InvariantCulture);
                                    LowAlarmEvents = ObjectManage.DeviceNew.Data.AllZoneOverTempTimesStatArray[3];
                                }
                            }
                        }
                        else if (ObjectManage.DeviceNew.Data.AlarmMode == 2)
                        {
                            if ((ObjectManage.DeviceNew.Data.AlarmType0 & 0x80) > 0)
                            {
                                AlarmDelayA1 = ObjectManage.DeviceNew.Data.AlarmDelay0;
                                A1 = ObjectManage.DeviceNew.Data.AlarmLimits0;
                                AlarmTypeA1 = Convert.ToInt32(ObjectManage.DeviceNew.Data.AlarmType0 & 0x01) > 0 ? "Cumulative" : "Single";
                                if (ObjectManage.DeviceNew.Data.RunStatus != 1)
                                {
                                    if (ObjectManage.DeviceNew.Data.AllZoneAlartStatusArray[0] > 0)
                                        AlarmA1First = ObjectManage.DeviceNew.Data.FirstAlarmTimeArray[0].ToString(Common.GetDefaultDateTimeFormat(), CultureInfo.InvariantCulture);
                                    AlarmNumA1 = ObjectManage.DeviceNew.Data.AllZoneOverTempTimesStatArray[0];
                                    AlarmA1Status = ObjectManage.DeviceNew.Data.AllZoneAlartStatusArray[0] == 0 ? "OK" : "Alarm";
                                    AlarmTotalTimeA1 = TempsenFormatHelper.ConvertSencondToFormmatedTime(ObjectManage.DeviceNew.Data.AllZoneOverTempTimeStatArray[0].ToString());
                                }
                            }
                            if ((ObjectManage.DeviceNew.Data.AlarmType1 & 0x80) > 0)
                            {
                                AlarmDelayA2 = ObjectManage.DeviceNew.Data.AlarmDelay1;
                                A2 = ObjectManage.DeviceNew.Data.AlarmLimits1;
                                AlarmTypeA2 = Convert.ToInt32(ObjectManage.DeviceNew.Data.AlarmType1 & 0x01) > 0 ? "Cumulative" : "Single";
                                if (ObjectManage.DeviceNew.Data.RunStatus != 1)
                                {
                                    if (ObjectManage.DeviceNew.Data.AllZoneAlartStatusArray[1] > 0)
                                        AlarmA2First = ObjectManage.DeviceNew.Data.FirstAlarmTimeArray[1].ToString(Common.GetDefaultDateTimeFormat(), CultureInfo.InvariantCulture);
                                    AlarmA2Status = ObjectManage.DeviceNew.Data.AllZoneAlartStatusArray[1] == 0 ? "OK" : "Alarm";
                                    AlarmTotalTimeA2 = TempsenFormatHelper.ConvertSencondToFormmatedTime(ObjectManage.DeviceNew.Data.AllZoneOverTempTimeStatArray[1].ToString());
                                }
                            }
                            if ((ObjectManage.DeviceNew.Data.AlarmType4 & 0x80) > 0)
                            {
                                AlarmDelayA5 = ObjectManage.DeviceNew.Data.AlarmDelay4;
                                A5 = ObjectManage.DeviceNew.Data.AlarmLimits4;
                                AlarmTypeA5 = Convert.ToInt32(ObjectManage.DeviceNew.Data.AlarmType4 & 0x01) > 0 ? "Cumulative" : "Single";
                                if (ObjectManage.DeviceNew.Data.RunStatus != 1)
                                {
                                    if(ObjectManage.DeviceNew.Data.AllZoneAlartStatusArray[4]>0)
                                        AlarmA5First = ObjectManage.DeviceNew.Data.FirstAlarmTimeArray[4].ToString(Common.GetDefaultDateTimeFormat(), CultureInfo.InvariantCulture);
                                    AlarmNumA5 = ObjectManage.DeviceNew.Data.AllZoneOverTempTimesStatArray[4];
                                    AlarmTotalTimeA5 = TempsenFormatHelper.ConvertSencondToFormmatedTime(ObjectManage.DeviceNew.Data.AllZoneOverTempTimeStatArray[4].ToString());
                                }
                            }
                            AlarmDelayA3 = ObjectManage.DeviceNew.Data.AlarmDelay2;
                            AlarmDelayA4 = ObjectManage.DeviceNew.Data.AlarmDelay3;
                            A3 = ObjectManage.DeviceNew.Data.AlarmLimits2;
                            A4 = ObjectManage.DeviceNew.Data.AlarmLimits3;
                            AlarmTypeA3 = Convert.ToInt32(ObjectManage.DeviceNew.Data.AlarmType2 & 0x01) > 0 ? "Cumulative" : "Single";
                            AlarmTypeA4 = Convert.ToInt32(ObjectManage.DeviceNew.Data.AlarmType3 & 0x01) > 0 ? "Cumulative" : "Single";
                            if (ObjectManage.DeviceNew.Data.RunStatus != 1)
                            {
                                AlarmA3Status = ObjectManage.DeviceNew.Data.AllZoneAlartStatusArray[2] == 0 ? "OK" : "Alarm";
                                AlarmA4Status = ObjectManage.DeviceNew.Data.AllZoneAlartStatusArray[3] == 0 ? "OK" : "Alarm";
                                AlarmA5Status = ObjectManage.DeviceNew.Data.AllZoneAlartStatusArray[4] == 0 ? "OK" : "Alarm";
                                if (ObjectManage.DeviceNew.Data.AllZoneAlartStatusArray[2] > 0)
                                    AlarmA3First = ObjectManage.DeviceNew.Data.FirstAlarmTimeArray[2].ToString(Common.GetDefaultDateTimeFormat(), CultureInfo.InvariantCulture);
                                if (ObjectManage.DeviceNew.Data.AllZoneAlartStatusArray[3] > 0)
                                    AlarmA4First = ObjectManage.DeviceNew.Data.FirstAlarmTimeArray[3].ToString(Common.GetDefaultDateTimeFormat(), CultureInfo.InvariantCulture);

                                AlarmNumA2 = ObjectManage.DeviceNew.Data.AllZoneOverTempTimesStatArray[1];
                                AlarmNumA3 = ObjectManage.DeviceNew.Data.AllZoneOverTempTimesStatArray[2];
                                AlarmNumA4 = ObjectManage.DeviceNew.Data.AllZoneOverTempTimesStatArray[3];
                                AlarmTotalTimeA3 = TempsenFormatHelper.ConvertSencondToFormmatedTime(ObjectManage.DeviceNew.Data.AllZoneOverTempTimeStatArray[2].ToString());
                                AlarmTotalTimeA4 = TempsenFormatHelper.ConvertSencondToFormmatedTime(ObjectManage.DeviceNew.Data.AllZoneOverTempTimeStatArray[3].ToString());
                                AlarmTotalTimeIdeal = TempsenFormatHelper.ConvertSencondToFormmatedTime(CalcMultiAlarmTotalTime("Ideal").ToString());
                            }
                        }
                        else
                        {
                            //<ToDo>
                        }
                        //Common.IsConnectCompleted = true;
                        //result= ObjectManage.DeviceNew.disconnectDevice();
                        result = true;
                        //return true;
                        #endregion
                    }
                    else
                        result = true;
                }
                Common.IsConnectCompleted = true;
                return result;
            }
            catch
            {
                Common.IsConnectCompleted = true;
                return false;
            }
        }
        public bool SerializePointInfo(DevicePDF device)
        {
            try
            {
                tempList.Clear();

                Dictionary<DateTime, double> dic = new Dictionary<DateTime, double>();
                ObjectManage.DeviceNew.Data.TempListWithTime.ToList().ForEach(p =>
                {
                    string[] s = p.Split(new char[1] { ',' });
                    tempList.Add(new PointKeyValue() { PointTime = Convert.ToDateTime(s[0]), PointTemp = Convert.ToDouble(s[1]) });

                });
                //此处增加mark属性值得读取及添加排序
                int markCount=ObjectManage.DeviceNew.Data.MarkTimeCount;
                if ( markCount> 0)
                {
                    for (int i = 0; i < markCount && i < ObjectManage.DeviceNew.Data.MarkTimeArray.Length; i++)
                    {
                        string[] markPoint = ObjectManage.DeviceNew.Data.MarkTimeArray[i].Split(new[] { ',' });
                        if (markPoint.Length < 2)
                            continue;
                        else if (Convert.ToDateTime(markPoint[1]) != DateTime.MinValue)
                        {
                            tempList.Add(new PointKeyValue(){
                                                                PointTemp=Convert.ToDouble(markPoint[0]),
                                                                PointTime = Convert.ToDateTime(markPoint[1]),
                                                                IsMark=true
                                                            });
                        }
                    }
                    tempList=tempList.OrderBy(p=>p.PointTime).ToList();
                }
                int id = new DeviceBLL().GetPointPKValue();
                PointInfo point = new PointInfo();
                point.ID = id + 1;
                point.ProductName = ProductName;
                point.SN = SerialNumber;
                point.TN = TripNumber;
                point.TempUnit = TempUnit;
                point.Remark = DateTime.Now.ToString();
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    if (tempList.Count > 0)
                    {
                        /*序列化*/
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<PointKeyValue>));
                        xmlSerializer.Serialize(ms, tempList);
                        point.Points = ms.ToArray();
                    }
                }
                points = point;
                //tempList.Clear();
                //ObjectManage.DeviceNew.Data.TempListWithTime.ToList().ForEach(p =>
                //{
                //    string[] s = p.Split(new char[1] { ',' });
                //    tempList.Add(new PointKeyValue() { PointTime = Convert.ToDateTime(s[0]).ToLocalTime(), PointTemp = Convert.ToDouble(s[1]) });

                //});
                return true;
            }
            catch { return false; }
        }
        #endregion
    }
}
