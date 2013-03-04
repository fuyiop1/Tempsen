using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Globalization;
namespace ShineTech.TempCentre.DAL
{
    [Serializable]
    [Table(Name = "DigitalSignature")]
    public class DigitalSignature:IEntity
    {
        public DigitalSignature() { }
        private int _ID;
        [Column(Name = "ID", DbType = DbType.Int32, PK = true)]
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _UserName;
        [Column(Name = "UserName", DbType = DbType.String)]
        public string UserName
        {
            get { return _UserName; }
            set { _UserName = value; }
        }
        private string _FullName;
        [Column(Name = "FullName", DbType = DbType.String)]
        public string FullName
        {
            get { return _FullName; }
            set { _FullName = value; }
        }
        private string _MeaningDesc;
        [Column(Name = "MeaningDesc", DbType = DbType.String)]
        public string MeaningDesc
        {
            get { return _MeaningDesc; }
            set { _MeaningDesc = value; }
        }
        private DateTime _SignTime;
        [Column(Name = "SignTime", DbType = DbType.DateTime)]
        public DateTime SignTime
        {
            get { return _SignTime; }
            set { _SignTime = value; }
        }
        private string _TN;
        [Column(Name = "TN", DbType = DbType.String)]
        public string TN
        {
            get { return _TN; }
            set { _TN = value; }
        }
        private string _SN;
        [Column(Name = "SN", DbType = DbType.String)]
        public string SN
        {
            get { return _SN; }
            set { _SN = value; }
        }
        private string _Remark;
        [Column(Name = "Remark", DbType = DbType.String)]
        public string Remark
        {
            get { return _Remark; }
            set { _Remark = value; }
        }

        public override string ToString()
        {
            object[] args = new object[4];
            args[0] = this.UserName;
            args[1] = this.FullName;
            args[2] = this.SignTime.ToLocalTime().ToString();
            args[3] = this.MeaningDesc;
            return string.Format("{0}({1})_{3}_{2}", args);
        }
        public string ToString(string format)
        {
            object[] args = new object[4];
            args[0] = this.UserName;
            args[1] = this.FullName;
            args[2] = this.SignTime.ToLocalTime().ToString(format, CultureInfo.InvariantCulture);
            args[3] = this.MeaningDesc;
            return string.Format("{0}({1})_{3}_{2}", args);
        }
        public override bool Equals(object obj)
        {
            DigitalSignature obj1 = obj as DigitalSignature;
            if (this.SN == obj1.SN && this.TN == obj1.TN && this.UserName == obj1.UserName && this.SignTime == obj1.SignTime)
                return true;
            else
                return false;
        }
    }
}
