using System;
using System.Data;

namespace ShineTech.TempCentre.DAL
{
    [AttributeUsage(AttributeTargets.Property, Inherited = false, AllowMultiple = false)]
    public class ColumnAttribute : System.Attribute
    {
        private string _ColumnName;
        private DbType _dbType;
        private bool _PK = false;

        public bool PK
        {
            get { return _PK; }
            set { _PK = value; }
        }
        public DbType DbType
        {
            get { return _dbType; }
            set { _dbType = value; }
        }
        public string Name
        {
            get { return _ColumnName; }
            set { _ColumnName = value; }
        }
        public ColumnAttribute() { }
        public ColumnAttribute(string name) {
            this.Name = name;
        }
        public ColumnAttribute(string name ,DbType dbtype) :this(name)
        {
            this.DbType = dbtype;
        }
    }
}
