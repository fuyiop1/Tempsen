using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShineTech.TempCentre.DAL;

namespace ShineTech.TempCentre.BusinessFacade
{
    public class ActivityLogManager
    {
        private void AddLogEntry(string action, string username,string fullname,string detail,int logType)
        {
            if (Common.User.UserName != Common.SUPERUSER)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("OperateTime", DateTime.UtcNow);
                dic.Add("Action", action);
                dic.Add("UserName", username);
                dic.Add("FullName", fullname);
                dic.Add("Detail", detail);
                dic.Add("LogType", logType);
                new OperationLogBLL().InsertLog(dic);
            }
        }

        /// <summary>
        /// add unlock log entry
        /// </summary>
        /// <param name="usename"></param>
        /// <param name="fullname"></param>
        /// <param name="detail"></param>
        public void AddUnlockUserLogEntry(string usename, string fullname, string detail)
        {
            AddLogEntry(LogAction.UnlockUser, usename, fullname, detail, LogAction.SystemAuditTrail);
        }

        /// <summary>
        /// add lock user log entry
        /// </summary>
        /// <param name="usename"></param>
        /// <param name="fullname"></param>
        /// <param name="detail"></param>
        public void AddLockUserLogEntry(string usename, string fullname, string detail)
        {
            AddLogEntry(LogAction.LockUser, usename, fullname, detail, LogAction.SystemAuditTrail);
        }
    }
}
