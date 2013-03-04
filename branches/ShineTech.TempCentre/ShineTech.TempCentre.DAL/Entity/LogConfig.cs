using System;
using System.Data;

namespace ShineTech.TempCentre.DAL
{
    [Table(Name = "LogConfig")]
    public class LogConfig:IEntity
    {
        private int _ID;
        [Column(Name = "ID", DbType = DbType.String,PK=true)]
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _ProductName;
        [Column(Name = "ProductName", DbType = DbType.String)]
        public string ProductName
        {
            get { return _ProductName; }
            set { _ProductName = value; }
        }
        private string _SerialNum;
        [Column(Name = "SN", DbType = DbType.String)]
        public string SN
        {
            get { return _SerialNum; }
            set { _SerialNum = value; }
        }
        private string _TripNum;
        [Column(Name = "TN", DbType = DbType.String)]
        public string TN
        {
            get { return _TripNum; }
            set { _TripNum = value; }
        }
        private string _startMode;
        [Column(Name = "StartMode", DbType = DbType.String)]
        public string StartMode
        {
            get { return _startMode; }
            set { _startMode = value; }
        }
        private string _logInterval;
        [Column(Name = "LogInterval", DbType = DbType.String)]
        public string LogInterval
        {
            get { return _logInterval; }
            set { _logInterval = value; }
        }
        private string _LogCycle;
        [Column(Name = "LogCycle", DbType = DbType.String)]
        public string LogCycle
        {
            get { return _LogCycle; }
            set { _LogCycle = value; }
        }
        private string _startDelay;
        [Column(Name = "StartDelay", DbType = DbType.String)]
        public string StartDelay
        {
            get { return _startDelay; }
            set { _startDelay = value; }
        }
        private string _remark;
        [Column(Name = "Remark", DbType = DbType.String)]
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
    }
}
