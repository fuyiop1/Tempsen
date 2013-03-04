using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ShineTech.TempCentre.DAL
{
    [Table(Name = "PointInfo")]
    public  class PointInfo : IEntity
    {
         private int _id;

        private string _sn;
        private string  _tn;
        private string _pointTemp;

        private System.Nullable<System.DateTime> _pointTime;

        private string _productName;

        private string _remark;

        private string _tripLength;

        public PointInfo()
        {
        }

        [Column( Name = "ID", DbType = DbType.Int32,PK=true)]
        
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

        [Column( Name = "PointTemp", DbType = DbType.String)]
        
        public string PointTemp
        {
            get
            {
                return this._pointTemp;
            }
            set
            {
                if (((_pointTemp == value)
                            == false))
                {
                    this._pointTemp = value;
                }
            }
        }

        [Column( Name = "PointTime", DbType=DbType.DateTime)]
        
        public System.Nullable<System.DateTime> PointTime
        {
            get
            {
                return this._pointTime;
            }
            set
            {
                if ((_pointTime != value))
                {
                   this._pointTime = value;
                }
            }
        }

        [Column( Name = "ProductName", DbType = DbType.String)]
        
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

        [Column( Name = "Remark", DbType =DbType.String)]
        
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

        [Column( Name = "TripLength", DbType = DbType.String)]
        
        public string TripLength
        {
            get
            {
                return this._tripLength;
            }
            set
            {
                if (((_tripLength == value)
                            == false))
                {
                    this._tripLength = value;
                }
            }
        }
    }
}
