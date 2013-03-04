using System;
using System.Data;

namespace ShineTech.TempCentre.DAL
{
    [Table(Name = "ProductType")]
    public class ProductType:IEntity
    {
        private int _id;

        private string _name;

        private string _remark;    
        public ProductType()
        {
        }

        [Column(Name = "ID", DbType = DbType.Int32,PK=true)]
        
        public int ID
        {
            get
            {
                return this._id;
            }
            set
            {
                if ((_id != value))
                {
                    this._id = value;
                }
            }
        }

        [Column(Name = "Name", DbType =DbType.String)]
        
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                if (((_name == value)
                            == false))
                {
                    this._name = value;
                }
            }
        }

        [Column(Name = "Remark", DbType =DbType.String)]
        
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
    }
}
