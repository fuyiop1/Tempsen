using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
///<summary>
///CLR Ver : 4.0.30319.225
///CreateBy: wangfei
///CreateOn:9/9/2011 5:48:59 PM
///FileName:ELogTIFactory
///</summary>
namespace ShineTech.TempCentre.BusinessFacade
{
    public class ELogTIFactory:Factory
    {
         public override SuperDevice Creator()
        {
            return new ELog();
        }
         public ELogTIFactory() { }
    }
}
