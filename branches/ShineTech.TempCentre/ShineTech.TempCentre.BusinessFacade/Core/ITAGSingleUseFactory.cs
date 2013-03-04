using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShineTech.TempCentre.BusinessFacade
{
    public class ITAGSingleUseFactory:Factory
    {
        public override SuperDevice Creator()
        {
            return new ITAGSingleUse();
        }
        public ITAGSingleUseFactory() { }
    }
}
