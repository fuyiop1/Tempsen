using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ShineTech.TempCentre.DAL
{
    [Serializable]
    [Table(Name = "PointInfo")]
    public  class PointInfo : IEntity
    {
         private int _id;

        private string _sn;
        private string  _tn;
        private byte[] _points;
        private string _productName;

        private string _remark;

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

        [Column(Name = "Points", DbType = DbType.Binary)]
        
        public byte[] Points
        {
            get
            {
                return this._points;
            }
            set
            {
                if (((_points == value)
                            == false))
                {
                    this._points = value;
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
        private string _TempUnit;
        [Column(Name = "TempUnit", DbType = DbType.String)]
        public string TempUnit
        {
            get { return _TempUnit; }
            set { _TempUnit = value; }
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
        private string _HighestC;
        [Column(Name = "HighestC", DbType = DbType.String)]
        public string HighestC
        {
            get { return _HighestC; }
            set { _HighestC = value; }
        }
        private string _LowestC;
        [Column(Name = "LowestC", DbType = DbType.String)]
        public string LowestC
        {
            get { return _LowestC; }
            set { _LowestC = value; }
        }
        private DateTime _StartTime;
        [Column(Name = "StartTime", DbType = DbType.DateTime)]
        public DateTime StartTime
        {
            get { return _StartTime; }
            set { _StartTime = value; }
        }
        private DateTime _EndTime;
        [Column(Name = "EndTime", DbType = DbType.DateTime)]
        public DateTime EndTime
        {
            get { return _EndTime; }
            set { _EndTime = value; }
        }
        private string _AVGTemp;
        [Column(Name = "AVGTemp", DbType = DbType.String)]
        public string AVGTemp
        {
            get { return _AVGTemp; }
            set { _AVGTemp = value; }
        }
        private DateTime _FirstPoint;
        [Column(Name = "FirstPoint", DbType = DbType.DateTime)]
        public DateTime FirstPoint
        {
            get { return _FirstPoint; }
            set { _FirstPoint = value; }
        }
        private string _MKT;
        [Column(Name = "MKT", DbType = DbType.String)]
        public string MKT
        {
            get { return _MKT; }
            set { _MKT = value; }
        }
        private string _TripLength;
        [Column(Name = "TripLength", DbType = DbType.String)]
        public string TripLength
        {
            get { return _TripLength; }
            set { _TripLength = value; }
        }
    }

    [Serializable]
    public class PointKeyValue
    {
        private DateTime _pointTime;

        public DateTime PointTime
        {
            get { return _pointTime; }
            set { _pointTime = value; }
        }
        private double _pointTemp;


        public double PointTemp
        {
            get { return _pointTemp; }
            set { _pointTemp = value; }
        }
        private bool _IsMark;

        public bool IsMark
        {
            get { return _IsMark; }
            set { _IsMark = value; }
        }
        public PointKeyValue() { }
    }
}
