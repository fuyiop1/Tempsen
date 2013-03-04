using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace ShineTech.TempCentre.DAL
{
    public class PolicyBLL
    {
         private IDataProcessor processor;
         public PolicyBLL()
        {
            processor = new DeviceProcessor();
        }
         public bool InsertPolicy(Policy entity)
         {
             return processor.Insert<Policy>(entity, null);
         }
         public bool InsertOrUpdate(Policy entity,bool flag)
         {
             if (flag)
                 return processor.Insert<Policy>(entity,null);
             else
                 return processor.Update<Policy>(entity,null);
         }
    }
}
