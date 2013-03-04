using System;
using System.Data;

namespace ShineTech.TempCentre.DAL
{
    [Table(Name="StatusCode")]
    public class StatusCode :IEntity
    {
        private int _id;
        private string _remark;
        private int _statusID;
        private string _statusName;

        public StatusCode()
        {
        }

        [Column(Name = "ID", DbType = DbType.Int32, PK = true)]
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

        [Column(Name = "StatusID", DbType = DbType.Int32)]
        
        public int StatusID
        {
            get
            {
                return this._statusID;
            }
            set
            {
                if ((_statusID != value))
                {
                    this._statusID = value;
                }
            }
        }

        [Column(Name = "StatusName", DbType = DbType.String)]
        
        public string StatusName
        {
            get
            {
                return this._statusName;
            }
            set
            {
                if (((_statusName == value)
                            == false))
                {
                    this._statusName = value;
                }
            }
        }
    }
}
