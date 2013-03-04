using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace ShineTech.TempCentre.DAL
{
    public class ReportEditorBLL
    {
        private IDataProcessor processor;
        public ReportEditorBLL()
        {
            processor = new DeviceProcessor();
        }
        public int GetReportEditorPKValue()
        {
            try
            {
                //object u = processor.QueryScalar("select Max(ID) from ReportEditor", null);
                ReportEditor r = processor.QueryOne<ReportEditor>("select * from ReportEditor where id=(select max(id) from ReportEditor)", () => null);
                //if (r != null)
                    return r.ID;
                //if (u != null && u.ToString() != string.Empty)
                //    return Convert.ToInt32(u);
                //else
                //    return 0;
            }
            catch { return 0; }
        }
        public bool InsertReportEditor(ReportEditor report)
        {
            return InsertReportEditor(report, null);
        }
        public bool InsertReportEditor(ReportEditor report, DbTransaction tran)
        {
            return processor.Insert<ReportEditor>(report, tran);
        }
        public ReportEditor GetReportEditorBySnTn(string sn, string tn)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("sn", sn);
            dic.Add("tn", tn);
            return processor.QueryOne<ReportEditor>("select * from ReportEditor where SN=@sn and TN=@tn", dic);
        }
        public bool UpdateReportEditor(ReportEditor report)
        {
            return processor.Update<ReportEditor>(report,null);
        }
        public void DeleteReportEditorBySnTn(string sn,string tn,DbTransaction tran)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("sn", sn);
            dic.Add("tn", tn);
            processor.ExecuteNonQuery("delete from ReportEditor where SN=@sn and TN=@tn",tran, dic);
        }
    }
}
