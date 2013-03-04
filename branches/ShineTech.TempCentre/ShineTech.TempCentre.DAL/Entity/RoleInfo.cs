using System;
using System.Data;

namespace ShineTech.TempCentre.DAL
{
    [Table(Name = "RoleInfo")]
    public class RoleInfo :IEntity
    {
        public RoleInfo() { }
        private int _ID;
        [Column(Name = "ID", DbType = DbType.Int32, PK = true)]        
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _rolename;
        [Column(Name = "Rolename", DbType = DbType.String)]
        public string Rolename
        {
            get { return _rolename; }
            set { _rolename = value; }
        }
        private string _remark="";
        [Column(Name = "Remark", DbType = DbType.String)]
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
    }
}
