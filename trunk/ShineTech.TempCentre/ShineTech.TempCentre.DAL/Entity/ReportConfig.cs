using System;

using System.Data;

namespace ShineTech.TempCentre.DAL
{
    [Table(Name = "ReportConfig")]
    public class ReportConfig : IEntity
    {
        public ReportConfig() { }
        private int _id;

        [Column(Name = "Id", DbType = DbType.Int32, PK = true)]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _ReportTitle;
        [Column(Name = "ReportTitle", DbType = DbType.String)]
        public string ReportTitle
        {
            get { return _ReportTitle; }
            set { _ReportTitle = value; }
        }
        private string _CompanyName;
        [Column(Name = "CompanyName", DbType = DbType.String)]
        public string CompanyName
        {
            get { return _CompanyName; }
            set { _CompanyName = value; }
        }
        private string _Adress;

        [Column(Name = "Adress", DbType = DbType.String)]
        public string Adress
        {
            get { return _Adress; }
            set { _Adress = value; }
        }
        private string _ContactPhone;

        [Column(Name = "ContactPhone", DbType = DbType.String)]
        public string ContactPhone
        {
            get { return _ContactPhone; }
            set { _ContactPhone = value; }
        }
        private string _Fax;

        [Column(Name = "Fax", DbType = DbType.String)]
        public string Fax
        {
            get { return _Fax; }
            set { _Fax = value; }
        }
        private string _Email;

        [Column(Name = "Email", DbType = DbType.String)]
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        private string _WebSite;
        [Column(Name = "WebSite", DbType = DbType.String)]
        public string WebSite
        {
            get { return _WebSite; }
            set { _WebSite = value; }
        }
        private byte[] _Logo;
        [Column(Name = "Logo", DbType = DbType.Binary)]
        public byte[] Logo
        {
            get { return _Logo; }
            set { _Logo = value; }
        }

        private string _Remark;

        [Column(Name = "Remark", DbType = DbType.String)]
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
        }
    }
}
