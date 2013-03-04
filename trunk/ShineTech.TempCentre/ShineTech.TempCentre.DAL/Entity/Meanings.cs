using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ShineTech.TempCentre.DAL
{
    [Table(Name="Meanings")]
    public class Meanings:IEntity
    {
        public Meanings() { }
        private int _id;
        [Column(Name = "Id", DbType = DbType.Int32,PK=true)]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }
        private string _desc;
        [Column(Name = "Desc", DbType = DbType.String)]
        public string Desc
        {
            get { return _desc; }
            set { _desc = value; }
        }
        private string _remark = string.Empty;
        [Column(Name = "Remark", DbType = DbType.String)]
        public string Remark
        {
            get { return _remark; }
            set { _remark = value; }
        }
    }
}
