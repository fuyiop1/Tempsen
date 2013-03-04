using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
///<summary>
///CLR Ver : 4.0.30319.225
///CreateBy: wangfei
///CreateOn:9/9/2011 3:55:22 PM
///FileName:ITAGPDFFactory
///</summary>
namespace ShineTech.TempCentre.BusinessFacade
{
    public class ITAGPDFFactory:Factory
    {
         public override SuperDevice Creator()
        {
            return new ITAGPDF();
        }
         public ITAGPDFFactory() { }
    }
}
