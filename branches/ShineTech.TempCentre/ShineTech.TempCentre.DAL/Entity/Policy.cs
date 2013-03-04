using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ShineTech.TempCentre.DAL
{
    public class Policy :IEntity
    {
        private int _id;

        private int _lockedTimes;

        private int _minPwdSize;
        private int _InactivityTime;
        private string _profileFolder;

        private int _pwdExpiredDay;

        private string _remark;
        public Policy()
        {
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

        [Column(Name = "LockedTimes", DbType = DbType.Int32)]

        public int LockedTimes
        {
            get
            {
                return this._lockedTimes;
            }
            set
            {
                if ((_lockedTimes != value))
                {
                    this._lockedTimes = value;
                }
            }
        }

        [Column(Name = "MinPwdSize", DbType = DbType.Int32)]

        public int MinPwdSize
        {
            get
            {
                return this._minPwdSize;
            }
            set
            {
                if ((_minPwdSize != value))
                {
                    this._minPwdSize = value;
                }
            }
        }
        [Column(Name = "InactivityTime", DbType = DbType.Int32)]
        public int InactivityTime
                {
                    get
                    {
                        return this._InactivityTime;
                    }
                    set
                    {
                        if ((_InactivityTime != value))
                        {
                            this._InactivityTime = value;
                        }
                    }
                }

        [Column(Name = "ProfileFolder", DbType = DbType.String)]

        public string ProfileFolder
        {
            get
            {
                return this._profileFolder;
            }
            set
            {
                if (((_profileFolder == value)
                            == false))
                {
                    this._profileFolder = value;
                }
            }
        }

        [Column(Name = "PwdExpiredDay", DbType =DbType.Int32)]

        public int PwdExpiredDay
        {
            get
            {
                return this._pwdExpiredDay;
            }
            set
            {
                if ((_pwdExpiredDay != value))
                {
                    this._pwdExpiredDay = value;
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
