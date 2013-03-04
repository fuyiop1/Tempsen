using System;
using System.Data;

namespace ShineTech.TempCentre.DAL
{
    public class AlarmConfig :IEntity
    {
        private string _alarmDelay;
        private string _alarmLevel;
        private string _alarmMode;
        private string _alarmTemp;
        private string _alarmType;
        private int _id;
        private string _isAlarm;
        private string _sn;
        private string _tn;
        private string _productName;
        private string _remark;
        private string _AlarmTotalTime;

        [Column(Name = "AlarmTotalTime", DbType = DbType.String)]
        public string AlarmTotalTime
        {
            get { return _AlarmTotalTime; }
            set { _AlarmTotalTime = value; }
        }
        private int _AlarmNumbers;

        [Column(Name = "AlarmNumbers", DbType = DbType.Int32)]
        public int AlarmNumbers
        {
            get { return _AlarmNumbers; }
            set { _AlarmNumbers = value; }
        }
        private string _AlarmFirstTriggered;

        [Column(Name = "AlarmFirstTriggered", DbType = DbType.String)]
        public string AlarmFirstTriggered
        {
            get { return _AlarmFirstTriggered; }
            set { _AlarmFirstTriggered = value; }
        }
        private bool _AlarmEnable;

        [Column(Name = "AlarmEnable", DbType = DbType.Boolean)]
        public bool AlarmEnable
        {
            get { return _AlarmEnable; }
            set { _AlarmEnable = value; }
        }
        public AlarmConfig()
        {
        }

        [Column(Name = "AlarmDelay", DbType = DbType.String)]
        public string AlarmDelay
        {
            get
            {
                return this._alarmDelay;
            }
            set
            {
                if (((_alarmDelay == value)
                            == false))
                {
                    this._alarmDelay = value;
                }
            }
        }

        [Column(Name = "AlarmLevel", DbType = DbType.String)]
        
        public string AlarmLevel
        {
            get
            {
                return this._alarmLevel;
            }
            set
            {
                if (((_alarmLevel == value)
                            == false))
                {
                    this._alarmLevel = value;
                }
            }
        }

        [Column(Name = "AlarmMode", DbType = DbType.String)]
        
        public string AlarmMode
        {
            get
            {
                return this._alarmMode;
            }
            set
            {
                if (((_alarmMode == value)
                            == false))
                {
                    this._alarmMode = value;
                }
            }
        }

        [Column(Name = "AlarmTemp", DbType = DbType.String)]

        public string AlarmTemp
        {
            get
            {
                return this._alarmTemp;
            }
            set
            {
                if ((_alarmTemp != value))
                {
                    this._alarmTemp = value;
                }
            }
        }

        [Column(Name = "AlarmType", DbType = DbType.String)]
        
        public string AlarmType
        {
            get
            {
                return this._alarmType;
            }
            set
            {
                if (((_alarmType == value)
                            == false))
                {
                    this._alarmType = value;
                }
            }
        }

        [Column(Name = "ID", DbType = DbType.Int32,PK=true)]
        
        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((_id != value))
                {
                    this._id = value;
                }
            }
        }

        [Column(Name = "IsAlarm", DbType =DbType.Int32)]
        
        public string IsAlarm
        {
            get
            {
                return this._isAlarm;
            }
            set
            {
                if ((_isAlarm != value))
                {
                    this._isAlarm = value;
                }
            }
        }

        [Column(Name = "SN", DbType =DbType.String)]
        
        public string SN
        {
            get
            {
                return this._sn;
            }
            set
            {
                if ((_sn != value))
                {
                    this._sn = value;
                }
            }
        }
        [Column(Name = "TN", DbType =DbType.String)]
        
        public string TN
        {
            get
            {
                return this._tn;
            }
            set
            {
                if ((_tn != value))
                {
                    this._tn = value;
                }
            }
        }
        [Column(Name = "ProductName", DbType = DbType.String)]
        
        public string ProductName
        {
            get
            {
                return this._productName;
            }
            set
            {
                if (((_productName == value)
                            == false))
                {
                    this._productName = value;
                }
            }
        }

        [Column(Name = "Remark", DbType = DbType.String)]
        
        public string Remark
        {
            get
            {
                return this._remark;
            }
            set
            {
                if (((_remark == value)
                            == false))
                {
                    this._remark = value;
                }
            }
        }
    }
}
