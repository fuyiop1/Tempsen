using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace ShineTech.TempCentre.DAL
{
    public class PointTempBLL
    {
        private IDataProcessor processor;
        public PointTempBLL()
        {
            processor = new DeviceProcessor();
        }

        public bool InsertPoint(PointInfo point)
        {
            return InsertPoint(point, null);
        }
        public bool InsertPoint(PointInfo point, DbTransaction tran)
        {
            object o=processor.QueryScalar("select 1 from pointinfo where id="+point.ID.ToString(),null);
            if (o != null && o.ToString() != "")
                point.ID = GetPointPKValue() + 1;
            return processor.Insert<PointInfo>(point, tran);
        }
        public bool InsertPoint(List<PointInfo> list, DbTransaction tran)
        {
            try
            {
                if (list != null && list.Count > 0)
                {
                    list.ForEach(p=>InsertPoint(p,tran));
                }
                return true;
            }
            catch
            {
                return false;
            }
        }
        public void DeletePointInfo(PointInfo point, DbTransaction tran)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ID", point.ID);
            processor.ExecuteNonQuery("delete from pointinfo where id=@ID", tran, dic);
        }
        public void DeletePointInfo(List<PointInfo> list, DbTransaction tran)
        {
            if (list != null && list.Count > 0)
            {
                list.ForEach(p => DeletePointInfo(p, tran));
            }
        }
        public List<PointInfo> GetPointsList()
        {
            return processor.Query<PointInfo>("select * from pointinfo", null);
        }
        public PointInfo GetPointsListByTNSN(string serialNum,string tripNum)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("sn", serialNum);
            dic.Add("tn", tripNum);
            return processor.QueryOne<PointInfo>("select * from pointinfo where SN=@sn and TN=@tn", dic);
        }
        public int GetPointPKValue()
        {
            object u = processor.QueryScalar("select max(id) from PointInfo", null);
            if (u != null && u.ToString() != string.Empty)
                return Convert.ToInt32(u);
            else
                return 0;
        }
        public bool UpdatePoint(PointInfo point,DbTransaction tran)
        {
            PointInfo p = GetPointsListByTNSN(point.SN, point.TN);
            point.ID = p.ID;
            return processor.Update<PointInfo>(point, tran);
        }
    }
}
