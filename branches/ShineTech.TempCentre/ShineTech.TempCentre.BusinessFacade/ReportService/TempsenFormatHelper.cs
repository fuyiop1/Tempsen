using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace ShineTech.TempCentre.BusinessFacade
{
    public class TempsenFormatHelper
    {
        public static string GetFormattedDate(DateTime dateTime)
        {
            string result = "";
            if (dateTime != null)
            {
                result = dateTime.ToString(GetDateFormat());
            }
            return result.ToString();
        }

        private static string GetDateFormat()
        {
            string result = "";
            string rawFormat = Common.GlobalProfile.DateTimeFormator;
            if (Common.GlobalProfile != null)
            {
                result = rawFormat.Substring(0, rawFormat.IndexOf(" ") + 1);
            }
            return result;
        }

        private static string GetTimeFormat()
        {
            string result = "";
            string rawFormat = Common.GlobalProfile.DateTimeFormator;
            if (Common.GlobalProfile != null)
            {
                result = rawFormat.Substring(rawFormat.IndexOf(" ") + 1);
            }
            return result;
        }

        public static string GetFormattedTime(DateTime dateTime)
        {
            string result = "";
            if (dateTime != null)
            {
                result = dateTime.ToString(GetTimeFormat(), CultureInfo.InvariantCulture);
            }
            return result;
        }

        public static string GetFormattedTemperature(double temperature)
        {
            string result = null;
            result = string.Format("{0:00.0}", temperature);
            return result;
        }

        public static string GetFormattedDateTime(string dateTime)
        {
            string result = "";
            DateTime dateTimeObject = new DateTime();
            if (DateTime.TryParse(dateTime, out dateTimeObject))
            {
                result = GetFormattedDateTime(dateTimeObject);
            }
            return " " + result;
        }

        public static string ReplaceNonEnglishCharactersInDateTime(string originalString)
        {
            string result = string.Empty;
            if (!string.IsNullOrWhiteSpace(originalString))
            {
                string[] splitStrings = originalString.Split('@');
                result = splitStrings[0] + "@" + GetFormattedDateTime(splitStrings[1]);
            }
            return result;
        }

        public static string GetFormattedDateTime(DateTime dateTime)
        {
            string result = "";
            string rawFormat = "";
            if (Common.GlobalProfile != null)
            {
                rawFormat = Common.GlobalProfile.DateTimeFormator;
            }
            if (dateTime != null)
            {
                result = dateTime.ToLocalTime().ToString(rawFormat, CultureInfo.InvariantCulture);
            }
            return result;
        }

        public static string ConvertSencondToFormmatedTime(string secondString)
        {
            if (string.IsNullOrEmpty(secondString))
                return "";
            //StringBuilder result = new StringBuilder();
            StringBuilder result = new StringBuilder();
            int second = 0;
            int.TryParse(secondString, out second);
            TimeSpan span = new TimeSpan(0,0,second);
            
            if (span.Days > 0)
            {
                result.Append(string.Format("{0}d", span.Days));
            }
            if (span.Hours > 0)
            {
                result.Append(string.Format("{0}h", span.Hours));
            }
            if (span.Minutes > 0)
            {
                result.Append(string.Format("{0}m", span.Minutes));
            }
            if (span.Seconds > 0)
            {
                result.Append(string.Format("{0}s", span.Seconds));
            }
            if (result.Length == 0)
            {
                result.Append("0s");
            }
            return result.ToString();
        }

        public static string GetRefinedFormatOfDelayTime(string originalString)
        {
            return "";
        }
        public static string GetSecondsFromFormatString(string originalString)
        {
            if (string.IsNullOrEmpty(originalString))
                return "0";
            int s=0;
            //day
            int dIndex= originalString.IndexOf("d");
            if (dIndex > -1)
            {
                s += Convert.ToInt32(originalString.Substring(0, dIndex)) * 86400;
                originalString = originalString.Substring(dIndex + 1, originalString.Length - dIndex - 1);
            }
            //hour
            dIndex = originalString.IndexOf("h");
            if (dIndex > -1)
            {
                s += Convert.ToInt32(originalString.Substring(0, dIndex)) * 3600;
                originalString = originalString.Substring(dIndex + 1, originalString.Length - dIndex - 1);
            }
            //minute
            dIndex = originalString.IndexOf("m");
            if (dIndex > -1)
            {
                s += Convert.ToInt32(originalString.Substring(0, dIndex)) * 60;
                originalString = originalString.Substring(dIndex + 1, originalString.Length - dIndex - 1);
            }
            //second
            dIndex = originalString.IndexOf("s");
            if (dIndex > -1)
            {
                s += Convert.ToInt32(originalString.Substring(0, dIndex));
                originalString = originalString.Substring(dIndex + 1, originalString.Length - dIndex - 1);
            }
            return s.ToString();
        }

    }

    
}
