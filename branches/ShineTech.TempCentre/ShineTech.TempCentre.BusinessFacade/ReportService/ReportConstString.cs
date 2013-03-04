using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShineTech.TempCentre.Platform;

namespace ShineTech.TempCentre.BusinessFacade
{
    public static class ReportConstString
    {
        public const string TitleAlarmString = "Alarm";
        public const string TitleOkString = "OK";
        public const string TitleDefaultString = "Please enter report title here";
        public const string CommentDefaultString = "Please enter comments here";

        public static string CreatedTimeString
        {
            get
            {
                return string.Format("Created at: {0}", TempsenFormatHelper.GetFormattedDateTime(DateTime.Now));
            }
        }
        public static string PoweredBy = string.Format("Powered by {0}", Messages.Caption);
        public const string Site = "www.tempsen.com";

        public static string TotalPageOfCurrentReport = "0";
    }
}
