using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
///<summary>
///CLR Ver : 4.0.30319.225
///CreateBy: wangfei
///CreateOn:9/9/2011 5:43:39 PM
///FileName:TempodFactory
///</summary>
namespace ShineTech.TempCentre.BusinessFacade
{
    public class TempodFactory:Factory
    {
        public override SuperDevice Creator()
        {
            return new Tempod();
        }
        public TempodFactory() { }
    }
}
