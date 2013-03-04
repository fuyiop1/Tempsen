using System;


namespace ShineTech.TempCentre.DAL
{
    [AttributeUsage(AttributeTargets.Class,Inherited=false,AllowMultiple=false)]
    public class TableAttribute : System.Attribute
    {
        private string _TableName;
        /// <summary>
        /// 映射的表名
        /// </summary>
        public string Name
        {
            get { return _TableName; }
            set { _TableName = value; }
        }
        public TableAttribute() { }

        public TableAttribute(string name)
        {
            this.Name = name;
        }
    }
}
