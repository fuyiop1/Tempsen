using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;

namespace ShineTech.TempCentre.DAL
{
    public class AlarmConfigBLL
    {
        private IDataProcessor processor;
        public AlarmConfigBLL()
        {
            processor = new DeviceProcessor();
        }
        public int GetAlarmConfigPKValue()
        {
            object u = processor.QueryScalar("select max(id) from AlarmConfig", null);
            if (u != null && u.ToString() != string.Empty)
                return Convert.ToInt32(u);
            else
                return 0;
        }


        public bool InsertAlarmConfig(AlarmConfig alarm)
        {
            return InsertAlarmConfig(alarm, null);
        }
        public bool InsertAlarmConfig(AlarmConfig alarm, DbTransaction tran)
        {
            return processor.Insert<AlarmConfig>(alarm, tran);
        }
        public bool InsertAlarmConfig(List<AlarmConfig> list, DbTransaction tran)
        {
           
                try
                {
                    if (list != null && list.Count > 0)
                    {
                        list.ForEach(p => InsertAlarmConfig(p, tran));
                    }
                    return true;
                }
                catch
                {
                    return false;
                }
        }
        public void DeleteAlarmConfig(List<AlarmConfig> list, DbTransaction tran)
        {

            if (list != null && list.Count > 0)
            {
                list.ForEach(p => DeleteAlarmConfig(p, tran));
            }
        }
        public void DeleteAlarmConfig(AlarmConfig alarm, DbTransaction tran)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ID", alarm.ID);
            processor.ExecuteNonQuery("delete from alarmconfig where id=@ID", tran, dic);
        }
        public List<AlarmConfig> GetAlarmConfigBySnTn(string sn, string tn)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("sn", sn);
            dic.Add("tn", tn);
            return processor.Query<AlarmConfig>("select * from AlarmConfig where SN=@sn and TN=@tn", dic);
        }
        public bool UpdateAlarmConfig(List<AlarmConfig> list, DbTransaction tran)
        {
            if (list != null && list.Count > 0)
            {
                List<AlarmConfig> p = GetAlarmConfigBySnTn(list[0].SN, list[0].TN);
                int i=0;
                list.ForEach(v =>
                {
                    v.ID = p[i].ID;
                    processor.Update<AlarmConfig>(v, tran);
                    i++;
                });
            }
            return true;
        }
    }
}