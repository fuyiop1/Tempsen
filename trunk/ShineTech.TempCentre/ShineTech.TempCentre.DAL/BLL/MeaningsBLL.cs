using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
namespace ShineTech.TempCentre.DAL
{
    public class MeaningsBLL
    {
        private IDataProcessor processor;
        public MeaningsBLL()
        {
            processor = new DeviceProcessor();
        }
        public List<UserMeanRelation> GetMeaningByUser(string username)
        {
            if (!string.IsNullOrEmpty(username))
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("username", username);
                return processor.Query<UserMeanRelation>("SELECT * FROM UserMeanRelation WHERE  username=@username", dic);
            }
            else
                return null;
        }
        public Meanings GetMeaningByName(string meanName)
        {
            if (!string.IsNullOrEmpty(meanName))
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("Desc", meanName);
                return processor.QueryOne<Meanings>("select * from Meanings where desc=@Desc", dic);
            }
            else
                return null;
        }
        public bool InsertMeaning(Meanings m)
        {
            return InsertMeaning(m, null);
        }
        public bool InsertMeaning(Meanings m,DbTransaction tran)
        {
            return processor.Insert<Meanings>(m, tran);
        }
        public bool InsertMeaning(Dictionary<string, object> dic, DbTransaction tran)
        {
            Meanings m = new Meanings();
            if (dic.Keys.Contains("ID"))
            {
                m.Id = (int)dic.First(p => { return p.Key == "ID"; }).Value;
            }
            else
            {
                int id = this.GetMeaningPKValue();
                m.Id = id + 1;
            }
            m.Desc = (string)dic.First(p => { return p.Key == "Desc"; }).Value;
            m.Remark = (string)dic.First(p => { return p.Key == "Remark"; }).Value;
            return processor.Insert<Meanings>(m,tran);
        }
        public bool InsertMeaning(Dictionary<string, object> dic)
        {
            return InsertMeaning(dic, null);
        }
        public bool InsertMeaning(Func<Dictionary<string, object>> func)
        {
            Dictionary<string, object> dic = func();
            return InsertMeaning(dic);
        }
        public bool InsertOrUpdateMeaning(Dictionary<string, object> dic, DbTransaction tran)
        {
            string desc = (string)dic["Desc"];
            Meanings m = GetMeaningByName(desc);
            if (m == null||m.Id==0)
                return this.InsertMeaning(dic, tran);
            return true;
        }
        public bool InsertOrUpdateMeaning(Dictionary<string, object> dic)
        {
            return InsertOrUpdateMeaning(dic, null);
        }
        /// <summary>
        /// 插入relation
        /// </summary>
        /// <param name="username"></param>
        /// <param name="dic"></param>
        /// <returns></returns>
        public bool InsertMeanRel(string username, Dictionary<string, object> dic, DbTransaction tran)
        {
            /*先查询是否存在username->meaning的关系*/
            Dictionary<string, object> condition = new Dictionary<string, object>();
            condition.Add("username",username);
            condition.Add("desc",dic["Desc"]);
            object o = processor.QueryScalar("select 1 from UserMeanRelation where username=@username and MeaningDesc=@desc",condition);
            if (o == null || o.ToString() == "")
            {
                UserMeanRelation rel = new UserMeanRelation();
                //if (dic.Keys.Contains("mID"))
                //    rel.MeaningsID = (int)dic.First(p => p.Key == "mID").Value;
                //else
                //{
                    //查询meaning id
               // Meanings m = this.GetMeaningByName(dic["Desc"].ToString());
                //if(m!=null&&m.Id!=0)
                //{
                if (dic.Keys.Contains("ID"))
                {
                    rel.ID = (int)dic.First(p => { return p.Key == "ID"; }).Value;
                }
                else
                {
                    int id = this.GetRelationPKValue();
                    rel.ID = id + 1;
                }
                rel.MeaningDesc = dic["Desc"].ToString();
                rel.Username = username;
                rel.Remark = DateTime.Now.ToString();
                return processor.Insert<UserMeanRelation>(rel, tran);
                //}
            }
            return false;
        }
        //插入relation
        public bool InsertMeanRel(string username, Dictionary<string, object> dic)
        {
            return InsertMeanRel(username, dic, null);
        }
        /// <summary>
        /// 获取当前主键的最大id值
        /// </summary>
        /// <returns></returns>
        public int GetMeaningPKValue()
        {
            object u = processor.QueryScalar("select max(id) from Meanings", null);
            if (u != null && u.ToString() != string.Empty)
                return Convert.ToInt32(u);
            else
                return 0;
        }
        public int GetRelationPKValue()
        {
            object u = processor.QueryScalar("select max(id) from UserMeanRelation", null);
            if (u != null && u.ToString() != string.Empty)
                return Convert.ToInt32(u);
            else
                return 0;
        }
    }
}
