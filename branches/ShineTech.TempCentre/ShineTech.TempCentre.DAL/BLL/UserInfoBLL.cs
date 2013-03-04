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
        public bool DisableUser(string Username,bool effective)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("disable",effective==true?0:1);
            dic.Add("username", Username);
            return processor.ExecuteNonQuery("update userinfo set disabled=@disable where username=@username COLLATE NOCASE", dic) == 0 ? false : true;
        }
        public bool UdateUser(UserInfo user)
        {
            return processor.Update<UserInfo>(user, null);
        }
        public void UpdateAllUserPwdExpire(DateTime newDate)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("newdate", newDate);
            string text = "update userinfo set LastPwdChangedTime=@newdate";
            processor.ExecuteNonQuery(text, dic);
        }
        /// <summary>
        /// 用户向导部分插入四个表
        /// </summary>
        /// <param name="user"></param>
        /// <param name="list"></param>
        /// <returns></returns>
        public bool InsertUserWizard(UserInfo user,Policy policy,List<string> right, List<Dictionary<string,object>> list)
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
                    /*修改策略*/
                    processor.Update<Policy>(policy, tran);
                    /*修改权限*/
                    if (right != null)
                    {
                        Dictionary<string, List<string>> r = new Dictionary<string, List<string>>();
                        r.Add(user.UserName, right);
                        new UserRightBLL().SummitUserRight(r, tran);
                    }
                   /*修改meaning*/
                    if(list!=null&&list.Count>0)
                        bll.InsertMeanRel(user.UserName, list, tran);
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

        /// <summary>
        /// 获得当前系统中管理员的个数
        /// </summary>
        /// <returns>
        /// </returns>
        public int GetAdminCount() {
            int result = 0;
            DataSet adminCount = processor.Query("SELECT COUNT(*) AS AdminCount FROM UserInfo WHERE (RoleId = 1)", null);
            int.TryParse(adminCount.Tables[0].Columns[0].ToString(),out result);
            return result;
        }

        /// <summary>
        /// 根据用户名获得并封装当前对象
        /// </summary>
        /// <returns></returns>
        public UserInfo GetUserInfoByUsername(string username) {
            UserInfo result = null;
            result = processor.QueryOne<UserInfo>("SELECT * FROM UserInfo WHERE username=@username COLLATE NOCASE", delegate()
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("@username", username.Trim());
                return dic;
            });
            return result;
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

        public IList<UserInfo> GetUserList()
        {
            return processor.Query<UserInfo>("Select * from userInfo", null);
            
        }
    }
}
