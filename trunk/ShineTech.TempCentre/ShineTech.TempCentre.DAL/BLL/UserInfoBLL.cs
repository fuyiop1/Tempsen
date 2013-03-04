using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
//using ShineTech
namespace ShineTech.TempCentre.DAL
{
    public class UserInfoBLL
    {
        private IDataProcessor processor;
        public UserInfoBLL()
        {
            processor = new DeviceProcessor();
        }
        public int GetCurrentUserID()
        {
            object u = processor.QueryScalar("select max(userid) from userinfo", null);
            if (u != null && u.ToString() != string.Empty)
                return Convert.ToInt32(u);
            else
                return 0;
        }
        public bool InsertUser(UserInfo user,System.Data.Common.DbTransaction tran)
        {
            return processor.Insert<UserInfo>(user, tran);
        }
        public bool InsertUser(UserInfo user)
        {
            return InsertUser(user, null);
        }
        /// <summary>
        /// 用户向导部分插入四个表
        /// </summary>
        /// <param name="user"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool InsertUserWizard(UserInfo user,List<Dictionary<string,object>> list)
        {
            MeaningsBLL bll = new MeaningsBLL();
            using(System.Data.SQLite.SQLiteConnection conn=SQLiteHelper.SQLiteHelper.CreateConn())
            {
                if(conn.State!=System.Data.ConnectionState.Open)
                    conn.Open();
                System.Data.Common.DbTransaction tran = conn.BeginTransaction();
                try
                {
                    /*插入userinfo*/
                    this.InsertUser(user, tran);
                    //先获取当前meaning及relation的最大id
                    int mId = bll.GetMeaningPKValue();
                    int rId = bll.GetRelationPKValue();
                    Dictionary<string, object> rDic, mDic;
                    foreach (Dictionary<string, object> dic in list)
                    {
                        rDic = new Dictionary<string,object>(dic);
                        mDic= new Dictionary<string,object>(dic);
                        mDic.Add("ID",++mId);
                        rDic.Add("ID",++rId);
                        bll.InsertOrUpdateMeaning(mDic, tran);
                        bll.InsertMeanRel(user.UserName, rDic, tran);
                    }
                    tran.Commit();
                }
                catch
                {
                    tran.Rollback();
                    conn.Close();
                    return false;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
            }
            
            return true;
        }

        public DataTable GetUserInfoByInit()
        {
            DataSet ds=processor.Query(@"SELECT username as 'User Name',fullname as 'Full Name',description as 'Role' 
                                                 ,CASE RoleID WHEN 1 THEN 'Admin'  else 'User' end as 'Group'
                                           FROM userinfo", null);
            if (ds != null && ds.Tables.Count > 0)
                return ds.Tables[0];
            else
                return null;
        }
    }
}
