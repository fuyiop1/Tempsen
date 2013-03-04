using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace ShineTech.TempCentre.DAL
{
    public class LogConfigBLL
    {
        private IDataProcessor processor;
        public LogConfigBLL()
        {
            processor = new DeviceProcessor();
        }
        public bool InsertLogConfig(LogConfig log, DbTransaction tran)
        {
            return processor.Insert<LogConfig>(log, tran);
        }
        public bool InsertLogConfig(LogConfig log)
        {
            return InsertLogConfig(log, null);
        }
        public void DeleteLogConfig(LogConfig log, DbTransaction tran)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ID", log.ID);
            processor.ExecuteNonQuery("delete from logconfig where id=@ID", tran, dic);
        }
        public void DeleteLogConfig(List<LogConfig> list, DbTransaction tran)
        {
            if (list != null && list.Count > 0)
            {
                list.ForEach(p => DeleteLogConfig(p, tran));
            }
        }
        public LogConfig GetLogConfigBySNTN(string serialNum, string tripNum)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("sn", serialNum);
            dic.Add("tn", tripNum);
            return processor.QueryOne<LogConfig>("select * from LogConfig where SN=@sn and TN=@tn", dic);
        }
        public int GetLogConfigPKValue()
        {
            object u = processor.QueryScalar("select max(id) from LogConfig", null);
            if (u != null && u.ToString() != string.Empty)
                return Convert.ToInt32(u);
            else
                return 0;
        }
        public bool UpdateLogConfig(LogConfig config, DbTransaction tran)
        {
            LogConfig p = GetLogConfigBySNTN(config.SN, config.TN);
            config.ID = p.ID;
            return processor.Update<LogConfig>(config, tran);
        }
    }
}
