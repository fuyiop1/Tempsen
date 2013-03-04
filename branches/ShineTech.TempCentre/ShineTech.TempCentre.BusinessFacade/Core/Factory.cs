using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShineTech.TempCentre.BusinessFacade
{
    public abstract class Factory
    {
        public abstract SuperDevice Creator();
    }
}
