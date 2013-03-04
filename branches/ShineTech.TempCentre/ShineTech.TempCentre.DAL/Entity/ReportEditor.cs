using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ShineTech.TempCentre.DAL
{
    public class ReportEditor:IEntity
    {
        public ReportEditor() { }
        private int _id;
        [Column(Name = "ID", DbType = DbType.Int32, PK = true)]
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _SN;
        [Column(Name = "SN", DbType = DbType.String)]
        public string SN
        {
            get { return _SN; }
            set { _SN = value; }
        }
        private string _TN;
        [Column(Name = "TN", DbType = DbType.String)]
        public string TN
        {
            get { return _TN; }
            set { _TN = value; }
        }
        private bool _IsLogoExist;
        [Column(Name = "IsLogoExist", DbType = DbType.Boolean)]
        public bool IsLogoExist
        {
            get { return _IsLogoExist; }
            set { _IsLogoExist = value; }
        }
        private bool _IsContactInfoChecked;
        [Column(Name = "IsContactInfoChecked", DbType = DbType.Boolean)]
        public bool IsContactInfoChecked
        {
            get { return _IsContactInfoChecked; }
            set { _IsContactInfoChecked = value; }
        }
        private bool _IsDeviceInfoChecked;
        [Column(Name = "IsDeviceInfoChecked", DbType = DbType.Boolean)]
        public bool IsDeviceInfoChecked
        {
            get { return _IsDeviceInfoChecked; }
            set { _IsDeviceInfoChecked = value; }
        }
        private bool _IsSummaryChecked;
        [Column(Name = "IsSummaryChecked", DbType = DbType.Boolean)]
        public bool IsSummaryChecked
        {
            get { return _IsSummaryChecked; }
            set { _IsSummaryChecked = value; }
        }
        private string _Comments;
        [Column(Name = "Comments", DbType = DbType.String)]
        public string Comments
        {
            get { return _Comments; }
            set { _Comments = value; }
        }
        private string _Remark;
        [Column(Name = "Remark", DbType = DbType.String)]
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
        }
        private string _ReportTitle;
        [Column(Name = "ReportTitle", DbType = DbType.String)]
        public string ReportTitle
        {
            get { return _ReportTitle; }
            set { _ReportTitle = value; }
        }
    }
}
