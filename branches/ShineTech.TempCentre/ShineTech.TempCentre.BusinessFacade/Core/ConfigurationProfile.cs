using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
///<summary>
///CLR Ver : 4.0.30319.225
///CreateBy: wangfei
///CreateOn:9/8/2011 5:13:55 PM
///FileName:ConfigurationProfile
///</summary>
namespace ShineTech.TempCentre.BusinessFacade
{
    [Serializable]
    public class ConfigurationProfile
    {
        private int _logIntervalH;

        public int LogIntervalH
        {
            get { return _logIntervalH; }
            set { _logIntervalH = value; }
        }
        private int _logIntervalM;

        public int LogIntervalM
        {
            get { return _logIntervalM; }
            set { _logIntervalM = value; }
        }
        private int _logIntervalS;

        public int LogIntervalS
        {
            get { return _logIntervalS; }
            set { _logIntervalS = value; }
        }
        private string _startMode;

        public string StartMode
        {
            get { return _startMode; }
            set { _startMode = value; }
        }
        private string _logCycle;

        public string LogCycle
        {
            get { return _logCycle; }
            set { _logCycle = value; }
        }
        private int _startDelayD;

        public int StartDelayD
        {
            get { return _startDelayD; }
            set { _startDelayD = value; }
        }
        private int _startDelayH;

        public int StartDelayH
        {
            get { return _startDelayH; }
            set { _startDelayH = value; }
        }
        private int _startDelayM;

        public int StartDelayM
        {
            get { return _startDelayM; }
            set { _startDelayM = value; }
        }
        private bool _isSingleAlarm;

        public bool IsSingleAlarm
        {
            get { return _isSingleAlarm; }
            set { _isSingleAlarm = value; }
        }
        private bool _isMultiAlarm;

        public bool IsMultiAlarm
        {
            get { return _isMultiAlarm; }
            set { _isMultiAlarm = value; }
        }
        private bool _isNoAlarm;

        public bool IsNoAlarm
        {
            get { return _isNoAlarm; }
            set { _isNoAlarm = value; }
        }
        private bool _isHighLimit;

        public bool IsHighLimit
        {
            get { return _isHighLimit; }
            set { _isHighLimit = value; }
        }
        private bool _isLowLimit;

        public bool IsLowLimit
        {
            get { return _isLowLimit; }
            set { _isLowLimit = value; }
        }
        private bool _isA6;

        public bool IsA6
        {
            get { return _isA6; }
            set { _isA6 = value; }
        }
        private bool _isA5;

        public bool IsA5
        {
            get { return _isA5; }
            set { _isA5 = value; }
        }
        private bool _isA1;

        public bool IsA1
        {
            get { return _isA1; }
            set { _isA1 = value; }
        }
        private string _tempUnit;

        public string TempUnit
        {
            get { return _tempUnit; }
            set { _tempUnit = value; }
        }
        private string _a6Temp;

        public string A6Temp
        {
            get { return _a6Temp; }
            set { _a6Temp = value; }
        }
        private string _a5Temp;

        public string A5Temp
        {
            get { return _a5Temp; }
            set { _a5Temp = value; }
        }
        private string _a4Temp;

        public string A4Temp
        {
            get { return _a4Temp; }
            set { _a4Temp = value; }
        }
        private string _a3Temp;

        public string A3Temp
        {
            get { return _a3Temp; }
            set { _a3Temp = value; }
        }
        private string _a2Temp;

        public string A2Temp
        {
            get { return _a2Temp; }
            set { _a2Temp = value; }
        }
        private string _a1Temp;

        public string A1Temp
        {
            get { return _a1Temp; }
            set { _a1Temp = value; }
        }
        private string _highTemp;

        public string HighTemp
        {
            get { return _highTemp; }
            set { _highTemp = value; }
        }
        private string _lowTemp;

        public string LowTemp
        {
            get { return _lowTemp; }
            set { _lowTemp = value; }
        }
        private string _a6AlarmType;

        public string A6AlarmType
        {
            get { return _a6AlarmType; }
            set { _a6AlarmType = value; }
        }
        private string _a5AlarmType;

        public string A5AlarmType
        {
            get { return _a5AlarmType; }
            set { _a5AlarmType = value; }
        }
        private string _a4AlarmType;

        public string A4AlarmType
        {
            get { return _a4AlarmType; }
            set { _a4AlarmType = value; }
        }
        private string _a2AlarmType;

        public string A2AlarmType
        {
            get { return _a2AlarmType; }
            set { _a2AlarmType = value; }
        }
        private string _a1AlarmType;

        public string A1AlarmType
        {
            get { return _a1AlarmType; }
            set { _a1AlarmType = value; }
        }
        private string _highAlarmType;

        public string HighAlarmType
        {
            get { return _highAlarmType; }
            set { _highAlarmType = value; }
        }
        private string _lowAlarmType;

        public string LowAlarmType
        {
            get { return _lowAlarmType; }
            set { _lowAlarmType = value; }
        }
        private int _A6Day;

        public int A6Day
        {
            get { return _A6Day; }
            set { _A6Day = value; }
        }
        private int _A5Day;

        public int A5Day
        {
            get { return _A5Day; }
            set { _A5Day = value; }
        }
        private int _A4Day;

        public int A4Day
        {
            get { return _A4Day; }
            set { _A4Day = value; }
        }
        private int _A2Day;

        public int A2Day
        {
            get { return _A2Day; }
            set { _A2Day = value; }
        }
        private int _A1Day;

        public int A1Day
        {
            get { return _A1Day; }
            set { _A1Day = value; }
        }
        private int _highDay;

        public int HighDay
        {
            get { return _highDay; }
            set { _highDay = value; }
        }
        private int _lowDay;

        public int LowDay
        {
            get { return _lowDay; }
            set { _lowDay = value; }
        }

        private int _A6H;

        public int A6H
        {
            get { return _A6H; }
            set { _A6H = value; }
        }
        private int _A5H;

        public int A5H
        {
            get { return _A5H; }
            set { _A5H = value; }
        }
        private int _A4H;

        public int A4H
        {
            get { return _A4H; }
            set { _A4H = value; }
        }
        private int _A2H;

        public int A2H
        {
            get { return _A2H; }
            set { _A2H = value; }
        }
        private int _A1H;

        public int A1H
        {
            get { return _A1H; }
            set { _A1H = value; }
        }
        private int _highH;

        public int HighH
        {
            get { return _highH; }
            set { _highH = value; }
        }
        private int _lowH;

        public int LowH
        {
            get { return _lowH; }
            set { _lowH = value; }
        }

        private int _A6M;

        public int A6M
        {
            get { return _A6M; }
            set { _A6M = value; }
        }
        private int _A5M;

        public int A5M
        {
            get { return _A5M; }
            set { _A5M = value; }
        }
        private int _A4M;

        public int A4M
        {
            get { return _A4M; }
            set { _A4M = value; }
        }
        private int _A2M;

        public int A2M
        {
            get { return _A2M; }
            set { _A2M = value; }
        }
        private int _A1M;

        public int A1M
        {
            get { return _A1M; }
            set { _A1M = value; }
        }
        private int _highM;

        public int HighM
        {
            get { return _highM; }
            set { _highM = value; }
        }
        private int _lowM;

        public int LowM
        {
            get { return _lowM; }
            set { _lowM = value; }
        }
        private DateTime _writeTime;
        private string _owner;

        public string Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }
        public DateTime WriteTime
        {
            get { return _writeTime; }
            set { _writeTime = value; }
        }
        private string _startDate;

        public string StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }
        private string _tn;
        private string _desc;

        public string Desc
        {
            get { return _desc; }
            set { _desc = value; }
        }

        public string Tn
        {
            get { return _tn; }
            set { _tn = value; }
        }
        public ConfigurationProfile() { }
    }
}
