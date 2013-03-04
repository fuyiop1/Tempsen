using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShineTech.TempCentre.DAL;
using System.Data.Common;
namespace ShineTech.TempCentre.DAL
{
    public class UserRightBLL
    {
        private IDataProcessor processor;
        public UserRightBLL()
        {
            processor = new DeviceProcessor();
        }

        public List<UserRight> GetRightByUserName(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Username", username);
                return processor.Query<UserRight>("select * from UserRight where Username=@Username COLLATE NOCASE", dic);
            }
            else
                return null;
        }
        public bool SummitUserRight(Dictionary<string,List<string>> dic,DbTransaction tran)
        {
            if (dic == null || dic.Count == 0)
                return false;
            foreach (KeyValuePair<string,List<string>> kp in dic)
            {
                string username = kp.Key;
                List<UserRight> list = this.GetUserRightByUserName(username);
                //List<UserRight> insert = new List<UserRight>();
                //List<UserRight> remove = new List<UserRight>();
                int i=this.GetUserRightPKValue();
                var del = from right in list
                        where !kp.Value.Contains(right.Right)
                        select right;
                var add = from p in kp.Value
                          where !(from r in list select r.Right).Contains(p)
                          select p;
                foreach (UserRight u in del)
                {
                    this.RemoceUserRight(u,tran);

                }
                foreach (string s in add)
                {
                    UserRight right = new UserRight();
                    right.ID = ++i;
                    right.UserName = username;
                    right.Right = s;
                    right.Remark = DateTime.Now.ToString();
                    this.InsertUserRight(right,tran);
                }               
                
            }
            return true;
        }
        public bool IsExist(string username,string right)
        {
            Dictionary<string,object> dic=new Dictionary<string,object> ();
            dic.Add("username",username);
            dic.Add("right",right);
            object o = processor.QueryScalar("select 1 from userright where username=@username COLLATE NOCASE and right=@right", dic);
            if (o == null || o.ToString() == "")
                return false;
            return true;
        }
        public bool InsertUserRight(UserRight right,DbTransaction tran)
        {
            return processor.Insert<UserRight>(right, tran);
        }
        public bool InsertUserRight(UserRight right)
        {
            return this.InsertUserRight(right, null);
        }
        public bool RemoceUserRight(UserRight right,DbTransaction tran)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ID", right.ID);
            return processor.ExecuteNonQuery("delete from userright where id=@ID",tran, dic)==0 ? false : true;
        }
        public int GetUserRightPKValue()
        {
            object u = processor.QueryScalar("select max(id) from userright", null);
            if (u != null && u.ToString() != string.Empty)
                return Convert.ToInt32(u);
            else
                return 0;
        }
        public List<UserRight> GetUserRightByUserName(string username)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("username", username);
            return processor.Query<UserRight>("select * from userright where username=@username COLLATE NOCASE", dic);
        }
        public void DeleteAllUserRight()
        {
            processor.ExecuteNonQuery("DELETE FROM userright", null);
        }
        public void DeleteUserRightByUserName(string username)
        {
            Dictionary<string,object> dic = new Dictionary<string,object>();
            dic.Add("username", username);
            processor.ExecuteNonQuery("DELETE FROM userright where username=@username COLLATE NOCASE", dic);
        }
    }
}
