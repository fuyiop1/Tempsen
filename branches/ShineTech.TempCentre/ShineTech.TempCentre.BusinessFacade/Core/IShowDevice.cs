using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace ShineTech.TempCentre.BusinessFacade
{
    public interface IShowDevice
    {
       bool Connect(int code);
       bool Auto(int code);
       DataTable GetDataList();
    }
}
