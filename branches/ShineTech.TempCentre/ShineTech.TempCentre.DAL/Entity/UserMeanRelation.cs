using System;
using System.Data;

namespace ShineTech.TempCentre.DAL
{
    [Table(Name = "UserMeanRelation")]
    public class UserMeanRelation : IEntity
    {
        public UserMeanRelation() { }
        private int _ID;

        [Column(Name = "ID",DbType=DbType.Int32,PK=true)]
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        private string _username;
        [Column(Name = "Username", DbType = DbType.String)]
        public string Username
        {
            get { return _username; }
            set { _username = value; }
        }
        //private int _MeaningsID;
        //[Column(Name = "MeaningsID", DbType = DbType.Int32)]
        //public int MeaningsID
        //{
        //    get { return _MeaningsID; }
        //    set { _MeaningsID = value; }
        //}
        private int _MeaningId;
        [Column(Name = "MeaningId", DbType = DbType.Int32)]
        public int MeaningId
        {
            get { return _MeaningId; }
            set { _MeaningId = value; }
        }
        private string _remark;
        [Column(Name = "Remark", DbType = DbType.String)]
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
        
    }
}
