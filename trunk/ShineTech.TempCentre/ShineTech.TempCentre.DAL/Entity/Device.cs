using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ShineTech.TempCentre.DAL
{
    public class Device :IEntity
    {
        public Device()
		{
			//
			// TODO: 在此处添加构造函数逻辑
			//
		}
		public const string F_ID="ID";
		private Int32 mID;
		/// <summary>
		/// [int]
		/// </summary>
        [Column(Name="ID",DbType=DbType.Int32,PK=true)]
		public Int32 ID
		{
			get
			{
				return mID;
			}
			set
			{
				mID = value;
			}
		}
		public const string F_PID="PID";
		private Int32 mPID;
		/// <summary>
		/// [int]
		/// </summary>
		public Int32 PID
		{
			get
			{
				return mPID;
			}
			set
			{
				mPID = value;
			}
		}
		public const string F_TypeID="TypeID";
		private Int32 mTypeID;
		/// <summary>
		/// [int]
		/// </summary>
		public Int32 TypeID
		{
			get
			{
				return mTypeID;
			}
			set
			{
				mTypeID = value;
			}
		}
		public const string F_ProductName="ProductName";
		private String mProductName;
		/// <summary>
		/// [nvarchar]
		/// </summary>
		public String ProductName
		{
			get
			{
				return mProductName;
			}
			set
			{
				mProductName = value;
			}
		}
		public const string F_SerialNum="SerialNum";
		private String mSerialNum;
		/// <summary>
		/// [nvarchar]
		/// </summary>
		public String SerialNum
		{
			get
			{
				return mSerialNum;
			}
			set
			{
				mSerialNum = value;
			}
		}
		public const string F_TripNum="TripNum";
		private String mTripNum;
		/// <summary>
		/// [nvarchar]
		/// </summary>
		public String TripNum
		{
			get
			{
				return mTripNum;
			}
			set
			{
				mTripNum = value;
			}
		}
		public const string F_Model="Model";
		private String mModel;
		/// <summary>
		/// [nvarchar]
		/// </summary>
		public String Model
		{
			get
			{
				return mModel;
			}
			set
			{
				mModel = value;
			}
		}
		public const string F_Battery="Battery";
		private object mBattery;
		/// <summary>
		/// [numeric]
		/// </summary>
		public object Battery
		{
			get
			{
				return mBattery;
			}
			set
			{
				mBattery = value;
			}
		}
		public const string F_DESCS="DESCS";
		private String mDESCS;
		/// <summary>
		/// [nvarchar]
		/// </summary>
		public String DESCS
		{
			get
			{
				return mDESCS;
			}
			set
			{
				mDESCS = value;
			}
		}
		public const string F_Remark="Remark";
		private String mRemark;
		/// <summary>
		/// [nvarchar]
		/// </summary>
		public String Remark
		{
			get
			{
				return mRemark;
			}
			set
			{
				mRemark = value;
			}
		}
    }
}
