using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShineTech.TempCentre.DAL;
using System.Data.Common;
namespace ShineTech.TempCentre.DAL
{
    public class DigitalSignatureBLL
    {
        private IDataProcessor processor;
        public DigitalSignatureBLL()
        {
            processor = new DeviceProcessor();
        }
        public int GetDigitalSignaturePKValue()
        {
            object u = processor.QueryScalar("select max(id) from DigitalSignature", null);
            if (u != null && u.ToString() != string.Empty)
                return Convert.ToInt32(u);
            else
                return 0;
        }
        public bool InsertDigitalSignature(DigitalSignature ds)
        {
            return InsertDigitalSignature(ds, null);
        }
        public bool InsertDigitalSignature(DigitalSignature ds, DbTransaction tran)
        {
            return processor.Insert<DigitalSignature>(ds, tran);
        }
        public List<DigitalSignature> GetDigitalSignatureList()
        {
            return processor.Query<DigitalSignature>("select * from DigitalSignature", null);
        }
        public List<DigitalSignature> GetDigitalSignatureBySnTn(string sn, string tn)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("sn", sn);
            dic.Add("tn", tn);
            return processor.Query<DigitalSignature>("select * from DigitalSignature where sn=@sn and tn=@tn", dic);
        }
        public void DeleteDigitalSignature(List<DigitalSignature> list,DbTransaction tran)
        {
            if (list != null && list.Count > 0)
            {
                list.ForEach(p => DeleteDigitalSignature(p, tran));
            }
        }
        public void DeleteDigitalSignature(DigitalSignature digital, DbTransaction tran)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ID", digital.ID);
            processor.ExecuteNonQuery("delete from DigitalSignature where id=@ID", tran, dic);
        }
        public bool IsExist(string sn, string tn)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("sn", sn);
            dic.Add("tn", tn);
            object u = processor.QueryScalar("select 1 from DigitalSignature where sn=@sn and tn=@tn", dic);
            if (u != null && u.ToString() != string.Empty)
                return true;
            return false;
        }
        public bool InsertDigitalSignature(List<DigitalSignature> list, DbTransaction tran)
        {
            if (list!= null && list.Count > 0)
            {
                List<DigitalSignature> ds = GetDigitalSignatureBySnTn(list[0].SN, list[0].TN);
                int j = 1;
                int id = GetDigitalSignaturePKValue();
                list.ForEach(p =>
                {
                    bool flag=false;
                    for (int i = 0; i < ds.Count; i++)
                    {
                        if (p.Equals(ds[i]))
                        {
                            flag = true;
                            break;
                        }
                    }
                    if (!flag)
                    {
                        p.ID = id + j;
                        InsertDigitalSignature(p, tran);
                    }
                    j++;
                });
            }
            return true;
        }
        public bool UpdateDigitalSignature(List<DigitalSignature> list, DbTransaction tran)
        {
            if (list != null && list.Count > 0)
            {
                List<DigitalSignature> p = GetDigitalSignatureBySnTn(list[0].SN, list[0].TN);
                int i = 0;
                list.ForEach(v =>
                {
                    v.ID = p[i++].ID;
                    processor.Update<DigitalSignature>(v, tran);
                });
            }
            return true;
        }
    }
}
