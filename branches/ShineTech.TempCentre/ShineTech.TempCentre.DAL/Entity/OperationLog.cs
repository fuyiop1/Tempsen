using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ShineTech.TempCentre.DAL 
{
    [Table(Name = "OperationLog")]
    public class OperationLog :IEntity
    {
        private int _id;

        [Column(Name = "ID", DbType = DbType.Int32,PK=true)]
        public int ID
        {
            get { return _id; }
            set { _id = value; }
        }

        private DateTime _operatetime;
        [Column(Name = "Operatetime", DbType = DbType.DateTime)]
        public DateTime Operatetime
        {
            get { return _operatetime; }
            set { _operatetime = value; }
        }
        private string _action;

        [Column(Name = "Action", DbType = DbType.String)]
        public string Action
        {
            get { return _action; }
            set { _action = value; }
        }
        private string _username;

        [Column(Name = "Username", DbType = DbType.String)]
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        private string _fullname;

        [Column(Name = "Fullname", DbType = DbType.String)]
        public string Fullname
        {
            get { return _fullname; }
            set { _fullname = value; }
        }
        private string _detail;

        [Column(Name = "Detail", DbType = DbType.String)]
        public string Detail
        {
            get { return _detail; }
            set { _detail = value; }
        }
        private int _logtype;

        [Column(Name = "Logtype", DbType = DbType.Int32)]
        public int LogType
        {
            get { return _logtype; }
            set { _logtype = value; }
        }

    }
}
