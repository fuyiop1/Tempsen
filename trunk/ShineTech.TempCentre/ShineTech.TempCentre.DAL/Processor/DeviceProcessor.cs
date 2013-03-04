using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ShineTech.TempCentre.SQLiteHelper;
using System.Data.SQLite;
using System.Reflection;
using System.Data.Common;
namespace ShineTech.TempCentre.DAL
{
    public class DeviceProcessor : IDataProcessor
    {
        public DeviceProcessor() { }
        public DataSet Query(string cmdText,Dictionary<string,object> o)
        {
            try
            {
                DataSet ds = new DataSet();
                if (o != null && o.Keys.Count > 0)
                {
                    List<DbParameter> list = new List<DbParameter>();
                    foreach (KeyValuePair<string, object> kvp in o)
                    {
                        //SQLiteHelper.SQLiteHelper.CreateParameter(kvp.Key, kvp.Value);
                        list.Add(SQLiteHelper.SQLiteHelper.CreateParameter(kvp.Key, kvp.Value));
                    }
                    return SQLiteHelper.SQLiteHelper.ExecuteDataSet(cmdText, list.ToArray());
                    //SQLiteHelper.SQLiteHelper.CreateParameter(
                }
                else
                    return SQLiteHelper.SQLiteHelper.ExecuteDataSet(cmdText, null);
                //return ds;
            }
            catch { return new DataSet(); }
        }
        public List<T> Query<T>(string cmdText, Dictionary<string, object> o) where T : IEntity ,new()
        {
            DataSet ds = this.Query(cmdText, o);
            if (ds != null && ds.Tables.Count > 0)
            {
                var list = new List<T>();
                var properityInfo = typeof(T).GetProperties().Where(v => v.CanWrite).ToList();
                ds.Tables[0].Rows.OfType<DataRow>().ToList().ForEach(row =>
                {
                    var obj = new T();
                    properityInfo.ForEach(p =>
                    {
                        if (ds.Tables[0].Columns.Contains(p.Name))
                            p.SetValue(obj, row[p.Name].ToString()=="" ? " " :row[p.Name], null);
                    });
                    list.Add(obj);
                });
                return list;
            }
            else
                return null;
        }
        public T QueryOne<T>(string cmdText, Dictionary<string, object> o) where T : IEntity,new()
        {
            List<T> t=this.Query<T>(cmdText, o);
            if (t != null && t.Count > 0)
                return t[0];
            else
                return new T();
        }
        public T QueryOne<T>(string cmdText, Func<Dictionary<string, object>> func) where T : IEntity, new()
        {
            Dictionary<string, object> o = func();
            List<T> t = this.Query<T>(cmdText, o);
            if (t != null && t.Count > 0)
                return t[0];
            else
                return new T();
        }
        public void ExecuteNonQuery(string cmdText, Dictionary<string, object> o)
        {
            if (o != null && o.Keys.Count > 0)
            {
                List<DbParameter> list = new List<DbParameter>();
                foreach (KeyValuePair<string, object> kvp in o)
                {
                    list.Add(SQLiteHelper.SQLiteHelper.CreateParameter(kvp.Key, kvp.Value));
                }
                 SQLiteHelper.SQLiteHelper.ExecuteNonQuery(cmdText, list.ToArray());
                //SQLiteHelper.SQLiteHelper.CreateParameter(
            }
            else
                SQLiteHelper.SQLiteHelper.ExecuteNonQuery(cmdText, null);
        }
        public object QueryScalar(string cmdText, Dictionary<string, object> o)
        {
            if (o != null && o.Keys.Count > 0)
            {
                List<DbParameter> list = new List<DbParameter>();
                foreach (KeyValuePair<string, object> kvp in o)
                {
                    //SQLiteHelper.SQLiteHelper.CreateParameter(kvp.Key, kvp.Value);
                    list.Add(SQLiteHelper.SQLiteHelper.CreateParameter(kvp.Key, kvp.Value));
                }
                return SQLiteHelper.SQLiteHelper.ExecuteQueryScalar(cmdText, list.ToArray());
                //SQLiteHelper.SQLiteHelper.CreateParameter(
            }
            else
                return SQLiteHelper.SQLiteHelper.ExecuteQueryScalar(cmdText,null);
        }

        public bool Insert<T>(T entity, DbTransaction trans) where T : IEntity
        {
            StringBuilder sql = new StringBuilder();
            StringBuilder sqllast = new StringBuilder (")VALUES(");
            Type type = entity.GetType();
            sql.Append("INSERT INTO ").Append(type.Name).Append(" ( ");            
            var properityInfo = type.GetProperties().Where(v=>v.CanWrite).ToList();
            List<DbParameter> list = new List<DbParameter>();
            properityInfo.ForEach(p => {
                sql.Append(p.Name.ToString()).Append(",");
                sqllast.Append("@" + p.Name+",");
                list.Add(SQLiteHelper.SQLiteHelper.CreateParameter(p.Name,p.GetValue(entity,p.GetIndexParameters())));
            });
            sql.Replace(',', ' ', sql.Length - 1, 1);
            sqllast.Replace(',', ')', sqllast.Length - 1, 1);            
            
            return SQLiteHelper.SQLiteHelper.ExecuteNonQuery(trans,sql.ToString()+sqllast.ToString(), list.ToArray())==1 ? true : false;
        }
        public void Insert<T>(List<T> entity) where T : IEntity
        {
            //DbTransaction tran=new 
            using (SQLiteConnection conn = SQLiteHelper.SQLiteHelper.CreateConn())
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                DbTransaction trans = conn.BeginTransaction();
                try
                {

                    foreach (T t in entity)
                    {
                        this.Insert<T>(t, trans);
                    }
                    trans.Commit();
                }
                catch (Exception e) { trans.Rollback(); conn.Close(); }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
        }

        public bool Update<T>(T entity, DbTransaction trans) where T : IEntity
        {
            StringBuilder sql = new StringBuilder();
            StringBuilder where = new StringBuilder("WHERE 1=1 ");
            Type type = entity.GetType();
            sql.Append("UPDATE ").Append(type.Name).Append(" SET ");
            var properityInfo = type.GetProperties().Where(v => v.CanWrite).ToList();
            List<DbParameter> list = new List<DbParameter>();
            int i = 0;
            properityInfo.ForEach(p =>
            {
                sql.Append(p.Name.ToString()).Append("=@"+ p.Name + ",");
                //sqllast.Append("@" );
               object []obj= p.GetCustomAttributes(typeof(ColumnAttribute), true);
               if (obj.Length > 0)
               {
                   ColumnAttribute ca = obj[0] as ColumnAttribute;
                   if(ca!=null&&ca.PK)
                       where.Append(" and " + p.Name + "=" + p.GetValue(entity, p.GetIndexParameters()));
               }
                //if(p.PropertyType==typeof(int))
                    //where.Append(" and "+p.Name + "=" + p.GetValue(entity, p.GetIndexParameters()));
                list.Add(SQLiteHelper.SQLiteHelper.CreateParameter(p.Name, p.GetValue(entity, p.GetIndexParameters())));

            });
            sql.Replace(',', ' ', sql.Length - 1, 1);
            
            return SQLiteHelper.SQLiteHelper.ExecuteNonQuery(trans, sql.ToString() + where.ToString(), list.ToArray()) == 1 ? true : false;
        }

        /// <summary>
        /// 插入或者更新
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="trans"></param>
        /// <param name="flag">true Insert，false update</param>
        /// <returns></returns>
        public bool InsertOrUpdate<T>(T entity, DbTransaction trans, bool flag) where T : IEntity
        {
            if (flag)
                return Insert<T>(entity, trans);
            else
                return Update<T>(entity, trans);
        }

        public bool OnCreated()
        {

            return SQLiteHelper.SQLiteHelper.OnCreated();
        }
    }
}
