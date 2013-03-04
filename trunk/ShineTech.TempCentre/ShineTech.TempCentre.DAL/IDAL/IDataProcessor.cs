using System;
using System.Collections.Generic;
using System.Data;
using System.Collections;
using System.Data.Common;
namespace ShineTech.TempCentre.DAL
{
    public interface IDataProcessor
    {
        /// <summary>
        /// 根据输入的T-SQL和参数返回集合
        /// </summary>
        /// <param name="cmdText">T-SQL(形如：select * from A WHERE ID=@ID)</param>
        /// <param name="list">参数值列表</param>
        /// <returns></returns>
        DataSet Query(string cmdText,Dictionary<string,object> o);
        List<T> Query<T>(string cmdText, Dictionary<string,object> o) where T : IEntity,new();
        T QueryOne<T>(string cmdText, Dictionary<string,object> o) where T : IEntity,new();
        T QueryOne<T>(string cmdText, Func<Dictionary<string,object>> func) where T : IEntity, new();
        void ExecuteNonQuery(string cmdText, Dictionary<string,object> o);
        object QueryScalar(string cmdText, Dictionary<string, object> o);

        bool Insert<T>(T entity,DbTransaction trans) where T : IEntity;
        void Insert<T>(List<T> entity) where T : IEntity;
        bool InsertOrUpdate<T>(T entity, DbTransaction trans, bool flag) where T : IEntity;

        bool Update<T>(T entity, DbTransaction trans) where T : IEntity;

        bool OnCreated();
        //bool Update<T>(List<T> entity) where T : IEntity;

        //bool Delete<T>(T entity) where T : IEntity;
        //bool Delete<T>(List<T> entity) where T : IEntity;        
        
    }
}
