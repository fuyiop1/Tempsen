using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Office.Interop.Outlook;
using ShineTech.TempCentre.Platform;

namespace ShineTech.TempCentre.BusinessFacade
{
    public class ReportOutlookEmailer : IReportEmailService
    {
        public void CreateEmailAndAddAttachments(string tpsFilePath)
        {
            try
            {
                Application application = new Application();
                MailItem item = application.CreateItem(OlItemType.olMailItem);
                item.Attachments.Add(tpsFilePath);
                item.Display();
            }
            catch (System.Exception)
            {
                Utils.ShowMessageBox(Messages.OutlookError, Messages.TitleError);
            }
            
        }
    }
}
