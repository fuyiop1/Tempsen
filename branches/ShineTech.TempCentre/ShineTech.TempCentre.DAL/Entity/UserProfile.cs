using System;
using System.Data;

namespace ShineTech.TempCentre.DAL
{
    [Table(Name = "UserProfile")]
    public class UserProfile :IEntity
    {
        private int _id;
        [Column(Name = "ID", DbType = DbType.Int32,PK=true)]
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _UserName;
        [Column(Name = "UserName", DbType = DbType.String)]
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
        private string _ContactInfo;
        [Column(Name = "ContactInfo", DbType = DbType.String)]
        public string ContactInfo
        {
            get { return _ContactInfo; }
            set { _ContactInfo = value; }
        }
        private string _ReportTitle;
        [Column(Name = "ReportTitle", DbType = DbType.String)]
        public string ReportTitle
        {
            get { return _ReportTitle; }
            set { _ReportTitle = value; }
        }
        private string _DefaultPath;
        [Column(Name = "DefaultPath", DbType = DbType.String)]
        public string DefaultPath
        {
            get { return _DefaultPath; }
            set { _DefaultPath = value; }
        }
        private string _Remark;
        [Column(Name = "Remark", DbType = DbType.String)]
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
        }
        private byte[] _logo;
        [Column(Name = "Logo", DbType = DbType.Binary)]
        public byte[] Logo
        {
            get { return _logo; }
            set { _logo = value; }
        }
        private int _IsGlobal;
        [Column(Name = "IsGlobal", DbType = DbType.Int32)]
        public int IsGlobal
        {
            get { return _IsGlobal; }
            set { _IsGlobal = value; }
        }
        private bool _IsShowHeader;
        [Column(Name = "IsShowHeader", DbType = DbType.Boolean)]
        public bool IsShowHeader
        {
            get { return _IsShowHeader; }
            set { _IsShowHeader = value; }
        }
        private string _TempUnit="C";
        [Column(Name = "TempUnit", DbType = DbType.String)]
        public string TempUnit
        {
            get { return _TempUnit; }
            set { _TempUnit = value; }
        }
        private string _TempCurveRGB = "255,0,0";
        [Column(Name = "TempCurveRGB", DbType = DbType.String)]
        public string TempCurveRGB
        {
            get { return _TempCurveRGB; }
            set { _TempCurveRGB = value; }
        }
        private string _AlarmLineRGB = "51,153,255";
        [Column(Name = "AlarmLineRGB", DbType = DbType.String)]
        public string AlarmLineRGB
        {
            get { return _AlarmLineRGB; }
            set { _AlarmLineRGB = value; }
        }
        private string _IdealRangeRGB = "0,255,255";
        [Column(Name = "IdealRangeRGB", DbType = DbType.Boolean)]
        public string IdealRangeRGB
        {
            get { return _IdealRangeRGB; }
            set { _IdealRangeRGB = value; }
        }
        private bool _IsShowAlarmLimit = true;
        [Column(Name = "IsShowAlarmLimit", DbType = DbType.Boolean)]
        public bool IsShowAlarmLimit
        {
            get { return _IsShowAlarmLimit; }
            set { _IsShowAlarmLimit = value; }
        }
        private bool _IsShowMark = true;
        [Column(Name = "IsShowMark", DbType = DbType.Boolean)]
        public bool IsShowMark
        {
            get { return _IsShowMark; }
            set { _IsShowMark = value; }
        }
        private bool _IsFillIdealRange = true;
        [Column(Name = "IsFillIdealRange", DbType = DbType.Boolean)]
        public bool IsFillIdealRange
        {
            get { return _IsFillIdealRange; }
            set { _IsFillIdealRange = value; }
        }
        private string _DateTimeFormator = "yyyy/MM/dd HH:mm:ss";
        [Column(Name = "DateTimeFormator", DbType = DbType.String)]
        public string DateTimeFormator
        {
            get { return _DateTimeFormator; }
            set { _DateTimeFormator = value; }
        }
        public GlobalType ShareType
        {
            get
            {
                switch (_IsGlobal)
                {
                    case 0 :
                    return GlobalType.None;
                    default:
                    return GlobalType.LogoAndConctact;
                }
            }
        }
        
    }
    public enum GlobalType { None=0,LogoAndConctact }
    public class GlobalEntity
    {
        public GlobalEntity() { }
        private int _id;

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private byte[] logo;
        public byte[] Logo
        {
            get { return logo; }
            set { logo = value; }
        }
        private string _ContactInfo;
        public string ContactInfo
        {
            get { return _ContactInfo; }
            set { _ContactInfo = value; }
        }
    }
}
