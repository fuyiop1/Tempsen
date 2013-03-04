using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ShineTech.TempCentre.BusinessFacade
{
    public interface IReportEmailService
    {
        void CreateEmailAndAddAttachments(string tpsFilePath);
    }
}
