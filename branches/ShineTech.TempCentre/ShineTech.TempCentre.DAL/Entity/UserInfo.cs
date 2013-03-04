using System;
using System.Data;
using System.Text;

namespace ShineTech.TempCentre.DAL
{
    [Serializable]
    [Table(Name="UserInfo")]
    public class UserInfo :IEntity
    {
        private string _account;
        private string _pwd;
        private string _remark;
        private int _userid;
        private string _UserName;
        private string _fullName;
        private string _Description;
        private int _Disabled;
        private int _Locked;
        private int _RoleId;
        private DateTime _LastPwdChangedTime;

        [Column(Name = "LastPwdChangedTime", DbType = DbType.DateTime)]
        public DateTime LastPwdChangedTime
        {
            get { return _LastPwdChangedTime; }
            set { _LastPwdChangedTime = value; }
        }

        
        public UserInfo()
        {
        }

        [Column( Name = "Account", DbType = DbType.String)]
        
        public string Account
        {
            get
            {
                return this._account;
            }
            set
            {
                if (((_account == value)
                            == false))
                {                    
                    this._account = value;
                }
            }
        }
        [Column(Name = "UserName", DbType = DbType.String)]
        public string UserName
        {
            get
            {
                return this._UserName;
            }
            set
            {
                if (((_UserName == value)
                            == false))
                {
                    this._UserName = value;
                }
            }
        }

        [Column(Name = "Pwd", DbType = DbType.String)]
        
        public string Pwd
        {
            get
            {
                return this._pwd;
            }
            set
            {
                if (((_pwd == value)
                            == false))
                {
                    this._pwd = value;
                }
            }
        }

        [Column( Name = "Remark", DbType = DbType.String)]
        
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

        [Column(Name = "Userid", DbType = DbType.Int32, PK = true)]
        
        public int Userid
        {
            get
            {
                return this._userid;
            }
            set
            {
                if ((_userid != value))
                {
                    this._userid = value;
                }
            }
        }

        public string Group
        {
            get
            {
                return RoleId == 1 ? "Admin" : "User";
            }
        }
        public bool IsAdmin
        {
            get
            {
                return RoleId == 1 ? true : false;
            }
        }
        [Column(Name = "RoleId", DbType = DbType.Int32)]
        public int RoleId
        {
            get { return _RoleId; }
            set { _RoleId = value; }
        }

        [Column(Name = "Locked", DbType = DbType.Int32)]
        public int Locked
        {
            get { return _Locked; }
            set { _Locked = value; }
        }

        [Column(Name = "Disabled", DbType = DbType.Int32)]
        public int Disabled
        {
            get { return _Disabled; }
            set { _Disabled = value; }
        }
        [Column(Name = "Description", DbType = DbType.String)]
        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        [Column(Name = "FullName", DbType = DbType.String)]
        public string FullName
        {
            get { return _fullName; }
            set { _fullName = value; }
        }

        public string Status
        {
            get
            {
                //StringBuilder result = new StringBuilder();
                //if (this.Disabled == 1)
                //{
                //    result.Append("Disabled");
                //}
                //if (this.Locked == 1)
                //{
                //    if (result.Length > 0)
                //    {
                //        result.Append(", ");
                //    }
                //    result.Append("Locked");
                //}
                //if (result.Length == 0)
                //{
                //    result.Append("Enabled");
                //}
                //return result.ToString();
                if (this.Locked == 1)
                    return "Locked";
                else
                    return string.Empty;
            }
        }
    }
}
