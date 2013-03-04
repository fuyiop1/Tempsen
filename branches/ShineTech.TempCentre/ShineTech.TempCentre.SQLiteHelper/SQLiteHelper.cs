using System.Data.SQLite;
using System.Configuration;
using System.Data;
using System;
using System.IO;
using System.Data.Common;
using ShineTech.TempCentre.Platform;
///<summary>
///createby wangfei 
///createon 2011.07.12
///desc:SQLiteHelper for operate the sqlite database
///</summary>
namespace ShineTech.TempCentre.SQLiteHelper
{
    public sealed class SQLiteHelper 
    {
        /// <summary>
        /// 轻量级数据库sqlite连接字符串data source=d:\\test.db;
        /// 加密后的写法data source=d:\\test.db;vesion=3;password=123
        /// </summary>
        public static string connString = "data source=" + Path.Combine(System.Windows.Forms.Application.StartupPath, ConfigurationManager.AppSettings["FileName"].ToString());
        public SQLiteHelper()
        {
        }
        #region ExecuteNonQuery
        /// <summary>
        /// 执行T-SQL语句，并返回影响的行数
        /// </summary>
        /// <param name="cmdText">text or procedure</param>
        /// <param name="isProcedure">是否存储过程</param>
        /// <param name="paras">参数列表</param>
        /// <returns>影响的行数</returns>
        public static int ExecuteNonQuery(string cmdText,bool isProcedure,params DbParameter []paras)
        {
            SQLiteConnection conn = CreateConn();
            SQLiteCommand cmd = new SQLiteCommand(cmdText, conn);
            if (isProcedure)
                cmd.CommandType = CommandType.StoredProcedure;
            else
                cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            if (paras != null && paras.Length > 0)
            {
                foreach (DbParameter para in paras)
                {
                    cmd.Parameters.Add(para);
                }
            }
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                return cmd.ExecuteNonQuery();
            }
            catch(Exception exp)
            {
                conn.Close();
                return 0;
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 执行T-SQL语句，并返回影响的行数
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="paras">参数列表</param>
        /// <returns>影响的行数</returns>
        public static int ExecuteNonQuery(string cmdText, params DbParameter[] paras)
        {
            return ExecuteNonQuery(cmdText, false, paras);
        }
        /// <summary>
        /// 执行带事物T-SQL语句并返回影响的行数
        /// </summary>
        /// <param name="trans">事物</param>
        /// <param name="cmdText">text or procedure</param>
        /// <param name="isProcedure">是否存储过程</param>
        /// <param name="paras">参数列表</param>
        /// <returns>影响的行数</returns>
        public static int ExecuteNonQuery(DbTransaction trans, string cmdText, bool isProcedure, params DbParameter[] paras)
        {
            SQLiteConnection conn = (SQLiteConnection)trans.Connection;
            SQLiteCommand cmd = new SQLiteCommand(cmdText, conn);
            if (isProcedure)
                cmd.CommandType = CommandType.StoredProcedure;
            else
                cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            if (paras != null && paras.Length > 0)
            {
                foreach (DbParameter para in paras)
                {
                    cmd.Parameters.Add(para);
                }
            }
            if (trans != null)
                cmd.Transaction = (SQLiteTransaction)trans;
            try
            {
                if(conn.State!=ConnectionState.Open)
                    conn.Open();
                return cmd.ExecuteNonQuery();
            }
            finally
            {
                if(trans==null)
                    conn.Close();
            }
        }

        /// <summary>
        /// 执行T-SQL语句，并返回影响的行数
        /// </summary>
        /// <param name="trans">传入的事物对象</param>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="paras">参数列表</param>
        /// <returns></returns>
        public static int ExecuteNonQuery(DbTransaction trans, string cmdText, params DbParameter[] paras)
        {
            if (trans != null)
                return ExecuteNonQuery(trans, cmdText, false, paras);
            else
                return ExecuteNonQuery(cmdText, paras);
        }
        #endregion
        #region ExecuteQueryScalar
        /// <summary>
        /// 执行T-SQL语句，并返回第一行第一列
        /// </summary>
        /// <param name="cmdText">text or procedure</param>
        /// <param name="isProcedure">是否存储过程</param>
        /// <param name="paras">参数列表</param>
        /// <returns>影响的行数</returns>
        public static object ExecuteQueryScalar(string cmdText, bool isProcedure, params DbParameter[] paras)
        {
            SQLiteConnection conn = CreateConn();
            SQLiteCommand cmd = new SQLiteCommand(cmdText, conn);
            if (isProcedure)
                cmd.CommandType = CommandType.StoredProcedure;
            else
                cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            if (paras != null && paras.Length > 0)
            {
                foreach (DbParameter para in paras)
                {
                    cmd.Parameters.Add(para);
                }
            }
            try
            {
                conn.Open();
                return cmd.ExecuteScalar();
            }
            catch
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
                return 0;
            }
            finally
            {
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
        }
        /// <summary>
        /// 执行T-SQL语句，并返回第一行第一列
        /// </summary>
        /// <param name="cmdText">text or procedure</param>
        /// <param name="paras">参数列表</param>
        /// <returns>影响的行数</returns>
        public static object ExecuteQueryScalar(string cmdText, params DbParameter[] paras)
        {
            return ExecuteQueryScalar(cmdText, false, paras);
        }
        /// <summary>
        /// 执行带事物T-SQL语句并返回第一行第一列
        /// </summary>
        /// <param name="trans">事物</param>
        /// <param name="cmdText">text or procedure</param>
        /// <param name="isProcedure">是否存储过程</param>
        /// <param name="paras">参数列表</param>
        /// <returns>影响的行数</returns>
        public static object ExecuteQueryScalar(DbTransaction trans, string cmdText, bool isProcedure, params DbParameter[] paras)
        {
            SQLiteConnection conn = (SQLiteConnection)trans.Connection;
            SQLiteCommand cmd = new SQLiteCommand(cmdText, conn);
            if (isProcedure)
                cmd.CommandType = CommandType.StoredProcedure;
            else
                cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            if (paras != null && paras.Length > 0)
            {
                foreach (DbParameter para in paras)
                {
                    cmd.Parameters.Add(para);
                }
            }
            if (trans != null)
                cmd.Transaction = (SQLiteTransaction)trans;
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                return cmd.ExecuteScalar();
            }
            finally
            {
                if (trans == null)
                    conn.Close();
            }
        }

        /// <summary>
        /// 执行T-SQL语句，并返回返回第一行第一列
        /// </summary>
        /// <param name="trans">传入的事物对象</param>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="paras">参数列表</param>
        /// <returns></returns>
        public static object ExecuteQueryScalar(DbTransaction trans, string cmdText, params DbParameter[] paras)
        {
            if (trans != null)
                return ExecuteQueryScalar(trans, cmdText, false, paras);
            else
                return ExecuteQueryScalar(cmdText, paras);
        }
        #endregion
        #region ExecuteReader
        /// <summary>
        /// 执行T-SQL语句，并返回reader
        /// </summary>
        /// <param name="cmdText">text or procedure</param>
        /// <param name="isProcedure">是否存储过程</param>
        /// <param name="paras">参数列表</param>
        /// <returns>影响的行数</returns>
        public static SQLiteDataReader ExecuteDataReader(string cmdText, bool isProcedure, params DbParameter[] paras)
        {
            SQLiteConnection conn = CreateConn();
            SQLiteCommand cmd = new SQLiteCommand(cmdText, conn);
            if (isProcedure)
                cmd.CommandType = CommandType.StoredProcedure;
            else
                cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            if (paras != null && paras.Length > 0)
            {
                foreach (DbParameter para in paras)
                {
                    cmd.Parameters.Add(para);
                }
            }
            try
            {
                if (conn.State!=ConnectionState.Open)
                    conn.Open();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                conn.Close();
                throw;            
            }
        }
        /// <summary>
        /// 执行SQL，并返回结果集的只前进数据读取器
        /// </summary>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="paras">DbParameter参数列表，0个或多个参数</param>
        /// <returns></returns>
        public static SQLiteDataReader ExecuteDataReader(string cmdText, params DbParameter[] paras)
        {
            return ExecuteDataReader(cmdText, false, paras);
        }
        /// <summary>
        /// 执行SQL，并返回结果集的只前进数据读取器
        /// </summary>
        /// <param name="trans">传递事务对象</param>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <param name="isProcedure">第二个参数是否为存储过程名,true为是,false为否</param>
        /// <param name="paras">DbParameter参数列表，0个或多个参数</param>
        /// <returns></returns>
        public static SQLiteDataReader ExecuteDataReader(DbTransaction trans, string cmdText, bool isProcedure, params DbParameter[] paras)
        {
            SQLiteConnection conn = (SQLiteConnection)trans.Connection;
            SQLiteCommand cmd = new SQLiteCommand(cmdText, conn);

            if (isProcedure)
            {
                cmd.CommandType = CommandType.StoredProcedure;
            }
            else
            {
                cmd.CommandType = CommandType.Text;
            }

            cmd.Parameters.Clear();
            if (paras != null && paras.Length > 0)
            {
                foreach (DbParameter para in paras)
                {
                    cmd.Parameters.Add(para);
                }
            }

            if (trans != null)
            {
                cmd.Transaction = (SQLiteTransaction)trans;
            }

            try
            {
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch
            {
                if (trans == null)
                {
                    conn.Close();
                }
                throw;
            }

        }

        /// <summary>
        /// 执行SQL，并返回结果集的只前进数据读取器
        /// </summary>
        /// <param name="trans">传递事务对象</param>
        /// <param name="cmdText">SQL语句</param>
        /// <param name="paras">DbParameter参数列表，0个或多个参数</param>
        /// <returns></returns>
        public static SQLiteDataReader ExecuteDataReader(DbTransaction trans, string cmdText, params DbParameter[] paras)
        {
            if (trans != null)
                return ExecuteDataReader(trans, cmdText, false, paras);
            else
                return ExecuteDataReader(cmdText, paras);
        }

        #endregion 
        #region ExecuteDataSet
        /// <summary>
        /// 执行T-SQL语句，并返回影响结果集合
        /// </summary>
        /// <param name="cmdText">text or procedure</param>
        /// <param name="isProcedure">是否存储过程</param>
        /// <param name="paras">参数列表</param>
        /// <returns>DataSet集合</returns>
        public static DataSet ExecuteDataSet(string cmdText,bool isProcedure,params DbParameter[] paras)
        {
            SQLiteConnection conn = CreateConn();
            SQLiteCommand cmd = new SQLiteCommand(cmdText, conn);
            if (isProcedure)
                cmd.CommandType = CommandType.StoredProcedure;
            else
                cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            if (paras != null && paras.Length > 0)
            {
                foreach (DbParameter para in paras)
                {
                    cmd.Parameters.Add(para);
                }
            }
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
            finally
            {
                conn.Close();
            }
        }
        /// <summary>
        /// 执行T-SQL语句，并返回影响结果集合
        /// </summary>
        /// <param name="cmdText">T-SQL语句</param>
        /// <param name="paras">参数列表</param>
        /// <returns>DataSet集合</returns>
        public static DataSet ExecuteDataSet(string cmdText, params DbParameter[] paras)
        {
            return ExecuteDataSet(cmdText, false, paras);
        }
        public static DataSet ExecuteDataSet(DbTransaction trans, string cmdText, bool isProcedure, params DbParameter[] paras)
        {
            SQLiteConnection conn = (SQLiteConnection)trans.Connection;
            SQLiteCommand cmd = new SQLiteCommand(cmdText, conn);
            if (isProcedure)
                cmd.CommandType = CommandType.StoredProcedure;
            else
                cmd.CommandType = CommandType.Text;
            cmd.Parameters.Clear();
            if (paras != null && paras.Length > 0)
            {
                foreach (DbParameter para in paras)
                {
                    cmd.Parameters.Add(para);
                }
            }
            if (trans != null)
                cmd.Transaction = (SQLiteTransaction)trans;
            try
            {
                if (conn.State != ConnectionState.Open)
                    conn.Open();
                SQLiteDataAdapter adapter = new SQLiteDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
            finally
            {
                if (trans == null)
                {
                    conn.Close();
                }
            }
        }
        public static DataSet ExecuteDataSet(DbTransaction trans, string cmdText, params DbParameter[] paras)
        {
            if (trans != null)
                return ExecuteDataSet(trans, cmdText, false, paras);
            else
                return ExecuteDataSet(cmdText, paras);
        }
        #endregion
        #region createparamter
        /// <summary>
        /// Builds a value parameter name for the current database by ensuring there is an '@' at the
        /// start of the name.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>A correctly formated parameter name, which starts with an '@'.</returns>
        public static string BuildParameterName(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (name.Trim().Length == 0)
            {
                throw new ArgumentException("The string cannot be empty.", "name");
            }

            if (name[0] != '@')
            {
                return "@" + name;
            }
            else
            {
                return name;
            }
        }
        /// <summary>
        /// Creates a new parameter and sets the name of the parameter.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <param name="value">
        /// The value you want assigned to thsi parameter. A null value will be converted to
        /// a <see cref="DBNull"/> value in the parameter.
        /// </param>
        /// <returns>
        /// A new <see cref="DbParameter"/> instance of the correct type for this database.</returns>
        /// <remarks>
        /// The database will automatically add the correct prefix, like "@" for SQLite, to the
        /// parameter name. In other words, you can just supply the name without a prefix.
        /// </remarks>
        public static DbParameter CreateParameter(string name, object value)
        {

            DbParameter param = CreateParameter(name);
            param.Value = (value == null) ? DBNull.Value : value;

            return param;
        }

        public static DbParameter CreateParameter(string name)
        {
            DbParameter parameter = new SQLiteParameter();
            parameter.ParameterName = BuildParameterName(name);

            return parameter;
        }
        public static DbParameter CreateParameter(string name, DbType type, int size, object value)
        {
            DbParameter param = CreateParameter(name);
            param.DbType = type;
            param.Size = size;
            param.Value = (value == null) ? DBNull.Value : value;

            return param;
        }
        #endregion
        #region create database
        /// <summary>
        /// 创建一个database
        /// </summary>
        public static bool OnCreated()
        {
            try
            {
                string fileName = ConfigurationManager.AppSettings["FileName"].ToString();
                if (!File.Exists(fileName))
                {
                    SQLiteConnection.CreateFile(fileName);
                    bool encrypt = Convert.ToBoolean(ConfigurationManager.AppSettings["Encrypt"]);
                    
                    string constr ="data source="+Path.Combine(System.Windows.Forms.Application.StartupPath, fileName);
                    SQLiteConnection conn = new SQLiteConnection(constr);
                    conn.Open();
                    if (encrypt)
                    {
                        if(Utils.WriteToXML())
                            conn.ChangePassword(Utils.Decode(Utils.ReadPwdFromXML(), Utils.ReadKeyFromXML(), Utils.ReadIVFromXML()));
                     }
                    string initPath = ConfigurationManager.AppSettings["InitSqlFile"] == null ? "" : ConfigurationManager.AppSettings["InitSqlFile"].ToString();
                    if (initPath != "")
                    {
                        string initSql = Utils.Read(initPath);
                        SQLiteCommand cmd = new SQLiteCommand(initSql, conn);
                        cmd.ExecuteNonQuery();
                    }
                    conn.Close();
                    return true;
                }
                else
                    return false;
            }
            catch 
            {
                
                return false; 
            }
        }
        #endregion
        /// <summary>
        /// 创建连接对象
        /// </summary>
        /// <returns></returns>
        public static SQLiteConnection CreateConn()
        {
            SQLiteConnection conn = new SQLiteConnection(connString);
            bool encrypt = Convert.ToBoolean(ConfigurationManager.AppSettings["Encrypt"]);
            if (encrypt)
                conn.SetPassword(Platform.Utils.Decode(Utils.ReadPwdFromXML(), Utils.ReadKeyFromXML(), Utils.ReadIVFromXML()));
            return conn;
        }
    }
}
