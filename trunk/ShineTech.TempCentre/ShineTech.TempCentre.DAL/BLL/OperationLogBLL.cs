using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShineTech.TempCentre.DAL
{
    public class OperationLogBLL
    {
        private IDataProcessor processor;
        public OperationLogBLL()
        {
            processor = new DeviceProcessor();
        }
        public bool InsertLog(OperationLog entity)
        {
            return processor.Insert<OperationLog>(entity, null);
        }
        /// <summary>
        /// 插入用户操作日志
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public bool InsertLog(Dictionary<string, object> dic)
        {
            OperationLog log = new OperationLog();
            if (dic.Keys.Contains("ID"))
            {
                log.ID = (int)dic.First(p => { return p.Key == "ID"; }).Value;
            }
            else
            {
                object o = processor.QueryScalar("select max(id) from OperationLog", null);
                int id = 0;
                if (o != null&&!string.IsNullOrEmpty(o.ToString()))
                {
                    id=Convert.ToInt32(o);
                }
                log.ID = id + 1;
            }
            log.Operatetime = (DateTime)dic.First(p => { return p.Key == "OperateTime"; }).Value;
            log.Action = (string)dic.First(p => { return p.Key == "Action"; }).Value;
            log.Username = (string)dic.First(p => { return p.Key == "UserName"; }).Value;
            log.Fullname = (string)dic.First(p => { return p.Key == "FullName"; }).Value;
            log.Detail = (string)dic.First(p => { return p.Key == "Detail"; }).Value;
            log.LogType = (int)dic.First(p => { return p.Key == "LogType"; }).Value;//0系统，1分析
            return this.InsertLog(log);
        }
        public bool InsertLog(Func<Dictionary<string,object>> func)
        {
            Dictionary<string, object> dic = func();
            return InsertLog(dic);
        }
        //根据条件读取所有日志
        public List<OperationLog> GetLog(Dictionary<string,object> dic)
        {
            string cmdtext = "select * from OperationLog where 1=1";
            if (dic != null)
            {
                foreach (string s in dic.Keys)
                {
                    if (s == "OperateTime1")
                        cmdtext = string.Concat(cmdtext, " and strftime('%Y%m%d',date(operatetime)) >=@", s);
                    else if (s == "OperateTime2")
                        cmdtext = string.Concat(cmdtext, " and strftime('%Y%m%d',date(operatetime)) <=@", s);
                    else
                        cmdtext = string.Concat(cmdtext, " and ", s, "=@", s);
                }
            }
            return processor.Query<OperationLog>(cmdtext,dic);
        }
    }
}
