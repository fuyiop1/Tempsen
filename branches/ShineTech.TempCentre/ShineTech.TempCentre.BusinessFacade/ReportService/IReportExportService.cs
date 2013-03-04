using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShineTech.TempCentre.DAL;

namespace ShineTech.TempCentre.BusinessFacade
{
    public interface IReportExportService
    {

        string Title { get; set; }
        string CurrentComment { get; set; }

        bool GenerateReport();
    }
}
