using System;
using System.Data;


namespace ShineTech.TempCentre.DAL
{
    [Table(Name = "UserRight")]
    public class UserRight:IEntity
    {
        private int _ID;
        [Column(Name = "ID", DbType = DbType.Int32,PK=true)]
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _userName;
        [Column(Name = "UserName", DbType = DbType.String)]
        public string UserName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        private string _right;
        [Column(Name = "Right", DbType = DbType.String)]
        public string Right
        {
            get { return _right; }
            set { _right = value; }
        }
        private string _remark;
        [Column(Name = "_remark", DbType = DbType.String)]
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
    }
}
