using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShineTech.TempCentre.DAL;
using ShineTech.TempCentre.Versions;
using System.Globalization;

namespace ShineTech.TempCentre.BusinessFacade
{
    public class ReportDataGenerator
    {

        public string GetLocalTimeZoneString()
        {
            string result = string.Empty;
            int offset = TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).Hours;
            string offsetString = string.Empty;
            if (offset >= 0)
            {
                offsetString = string.Format("+{0}", offset.ToString("00"));
            }
            else
            {
                offsetString = offset.ToString("00");
            }
            result = string.Format("Time Zone: (GMT{0}:00)  {1}", offsetString, Common.GetDateOrTimeFormat(true, Common.GetGlobalUserProfile().DateTimeFormator));
            return result;
        }

        public IDictionary<string, string[]> GetDeviceConfigurationTripInfoRowsContents(SuperDevice device)
        {
            IDictionary<string, string[]> result = new Dictionary<string, string[]>();
            string[] row1Contents = new string[2];
            string[] row2Contents = new string[2];
            string[] row3Contents = new string[2];

            string[] tripInfoContents = new string[3];

            result.Add("row1Contents", row1Contents);
            result.Add("row2Contents", row2Contents);
            result.Add("row3Contents", row3Contents);
            result.Add("tripInfoContents", tripInfoContents);
            if (device != null)
            {
                row1Contents[0] = string.Format("Device: {0}", device.DeviceName);
                row1Contents[1] = string.Format("Model: {0}", device.Model);
                row2Contents[0] = string.Format("Serial Number: {0}", device.SerialNumber);
                row2Contents[1] = string.Format("Log Interval/Cycle: {0}{2}{1}", TempsenFormatHelper.ConvertSencondToFormmatedTime(device.LogInterval), device.LogCycle, "/");
                row3Contents[0] = string.Format("Start Mode: {0}", device.StartModel);
                if (device.StartModel == "Manual Start")
                {
                    row3Contents[1] = string.Format("Start Delay: {0}", device.LogStartDelay);
                }
                else
                {
                    row3Contents[1] = string.Empty;
                }
                string tripNumberWithoutSuffix = string.Empty;
                if (device.TripNumber != null && device.TripNumber.IndexOf('_') != -1)
                {
                    tripNumberWithoutSuffix = device.TripNumber.Substring(0, device.TripNumber.IndexOf('_'));
                }
                else
                {
                    tripNumberWithoutSuffix = device.TripNumber;
                }

                tripInfoContents[0] = tripNumberWithoutSuffix;
                tripInfoContents[1] = device.DeviceID < 200 ? string.Empty : "Description: ";
                string rawDescription = string.IsNullOrWhiteSpace(device.Description) ? string.Empty : device.Description;
                if (device.DeviceID < 200)
                {
                    tripInfoContents[2] = string.Empty;
                }
                else
                {
                    tripInfoContents[2] = rawDescription;
                }
            }
            return result;
        }



        public IDictionary<string, string[]> GetLoggingSummaryColumsContents(SuperDevice device, DeviceDataFrom deviceDatafrom)
        {
            IDictionary<string, string[]> result = new Dictionary<string, string[]>();
            string[] column1Contents = new string[5];
            string[] column2Contents = new string[5];

            result.Add("column1Contents", column1Contents);
            result.Add("column2Contents", column2Contents);

            column1Contents[0] = string.Format("Highest Temperature: {0}", Common.SetTempTimeFormat(device.HighestC));
            column1Contents[1] = string.Format("Lowest Temperature: {0}", Common.SetTempTimeFormat(device.LowestC));
            column1Contents[2] = string.Format("Average Temperature: {0}{1}", device.AverageC, string.IsNullOrEmpty(device.AverageC) ? string.Empty : "°" + device.TempUnit);
            column1Contents[3] = string.Format("Mean Kinetic Temperature: {0}{1}", device.MKT, string.IsNullOrEmpty(device.MKT) ? string.Empty : "°" + device.TempUnit);
            string finalString = string.Empty;
            int maxDisplayLength = 10;
            string loggerReader = device.LoggerRead;
            if (loggerReader == null)
            {
                loggerReader = string.Empty;
            }
            string[] loggerReadereString = loggerReader.Split(new char[] { '@' });
            if (loggerReadereString.Length >= 2)
            {
                if (Common.Versions == SoftwareVersions.S || string.IsNullOrWhiteSpace(loggerReadereString[0]))
                {
                    finalString = string.Format("Logger Read: @{0}", TempsenFormatHelper.GetFormattedDateTime(Convert.ToDateTime(loggerReadereString[1])));
                }
                else if (Common.Versions == SoftwareVersions.Pro)
                {
                    if (loggerReadereString[0].Length > maxDisplayLength)
                    {
                        finalString = string.Format("Logger Read: By {0}@{1}", loggerReadereString[0].Substring(0, maxDisplayLength) + "...", TempsenFormatHelper.GetFormattedDateTime(Convert.ToDateTime(loggerReadereString[1])));
                    }
                    else
                    {
                        finalString = string.Format("Logger Read: By {0}@{1}", loggerReadereString[0], TempsenFormatHelper.GetFormattedDateTime(Convert.ToDateTime(loggerReadereString[1])));
                    }
                }
            }
            else
            {
                finalString = "Logger Read:";
            }
           
            column1Contents[4] = finalString;

            column2Contents[0] = string.Format("Start Time/First Point: {0}", device.LoggingStart == DateTime.MinValue ? string.Empty : TempsenFormatHelper.GetFormattedDateTime(device.LoggingStart.ToLocalTime()));
            column2Contents[1] = string.Format("Stop Time: {0}", device.LoggingEnd == DateTime.MinValue ? string.Empty : TempsenFormatHelper.GetFormattedDateTime(device.LoggingEnd.ToLocalTime()));
            column2Contents[2] = string.Format("Data Points: {0}", device.DataPoints == 0 ? string.Empty : device.DataPoints.ToString());
            column2Contents[3] = string.Format("Trip Length: {0}", device.TripLength);
            column2Contents[4] = string.Empty;
            return result;
        }

        public string GetAlarmSectionTitle(SuperDevice device)
        {
            string result = string.Empty;
            if (device.AlarmMode == 1)
            {
                result = string.Format("Alarms [{0}]", "High & Low Alarm");
                
            }
            else if (device.AlarmMode == 2)
            {
                result = string.Format("Alarms [{0}]", "Multiple Alarms");
                
            }
            else
                result = string.Format("Alarms [{0}]", "No Alarm Setting");
            return result;
        }

        public IDictionary<string, string[]> GetAlarmRowContents(SuperDevice device)
        {
            IDictionary<string, string[]> result = new Dictionary<string, string[]>();
            string[] highRowContents = new string[6];
            string[] lowRowContents = new string[6];
            string[] a1RowContents = new string[6];
            string[] a2RowContents = new string[6];
            string[] a3RowContents = new string[6];
            string[] a4RowContents = new string[6];
            string[] a5RowContents = new string[6];
            string[] a6RowContents = new string[6];

            result.Add("highRowContents", highRowContents);
            result.Add("lowRowContents", lowRowContents);
            result.Add("a1RowContents", a1RowContents);
            result.Add("a2RowContents", a2RowContents);
            result.Add("a3RowContents", a3RowContents);
            result.Add("a4RowContents", a4RowContents);
            result.Add("a5RowContents", a5RowContents);
            result.Add("a6RowContents", a6RowContents);

            if (device.AlarmMode == 1)
            {
                if (!string.IsNullOrEmpty(device.AlarmLowLimit))
                {
                    lowRowContents[0] = string.Format("Low limit: {0}°{1}", device.AlarmLowLimit, device.TempUnit);
                    lowRowContents[1] = string.Format("{0}({1})", device.AlarmLowDelay, device.LowAlarmType);
                    lowRowContents[2] = string.Format("{0}", device.tempList.Count == 0 ? string.Empty : device.LowAlarmTotalTimeBelow);
                    lowRowContents[3] = string.Format("{0}", device.tempList.Count == 0 ? string.Empty : device.LowAlarmEvents.ToString());
                    if (device.LowAlarmEvents > 0)
                    {
                        lowRowContents[4] = string.Format("{0}", Convert.ToDateTime(device.LowAlarmFirstTrigged) == DateTime.MinValue ? string.Empty : TempsenFormatHelper.GetFormattedDateTime(Convert.ToDateTime(device.LowAlarmFirstTrigged).ToLocalTime()));
                    }
                    else
                    {
                        lowRowContents[4] = string.Empty;
                    }
                    lowRowContents[5] = string.Format("{0}", device.tempList.Count == 0 ? string.Empty : device.AlarmLowStatus);
                }
                else
                {
                    lowRowContents[0] = "Low limit: ";
                }
                if (!string.IsNullOrEmpty(device.AlarmHighLimit))
                {
                    highRowContents[0] = string.Format("High limit: {0}°{1}", device.AlarmHighLimit, device.TempUnit);
                    highRowContents[1] = string.Format("{0}({1})", device.AlarmHighDelay, device.HighAlarmType);
                    highRowContents[2] = string.Format("{0}", device.tempList.Count == 0 ? string.Empty : device.HighAlarmTotalTimeAbove);
                    highRowContents[3] = string.Format("{0}", device.tempList.Count == 0 ? string.Empty : device.HighAlarmEvents.ToString());
                    if (device.HighAlarmEvents > 0)
                    {
                        highRowContents[4] = string.Format("{0}", Convert.ToDateTime(device.HighAlarmFirstTrigged) == DateTime.MinValue ? string.Empty : TempsenFormatHelper.GetFormattedDateTime(Convert.ToDateTime(device.HighAlarmFirstTrigged).ToLocalTime()));
                    }
                    else
                    {
                        highRowContents[4] = string.Empty;
                    }
                    
                    highRowContents[5] = string.Format("{0}", device.tempList.Count == 0 ? string.Empty : device.AlarmHighStatus);
                }
                else
                {
                    highRowContents[0] = "High limit: ";
                }
            }
            else if (device.AlarmMode == 2)
            {
                if (!string.IsNullOrEmpty(device.A1))
                {
                    a1RowContents[0] = string.Format("A1: over {0}°{1}", device.A1, device.TempUnit);
                    a1RowContents[1] = string.Format("{0}({1})", TempsenFormatHelper.ConvertSencondToFormmatedTime(device.AlarmDelayA1.ToString()), device.AlarmTypeA1);
                    a1RowContents[2] = string.Format("{0}", device.tempList.Count == 0 ? string.Empty : device.AlarmTotalTimeA1);
                    a1RowContents[3] = string.Format("{0}", device.tempList.Count == 0 ? string.Empty : device.AlarmNumA1.ToString());
                    if (device.AlarmNumA1 > 0)
                    {
                        a1RowContents[4] = string.Format("{0}", Convert.ToDateTime(device.AlarmA1First) == DateTime.MinValue ? string.Empty : TempsenFormatHelper.GetFormattedDateTime(Convert.ToDateTime(device.AlarmA1First).ToLocalTime()));
                    }
                    else
                    {
                        a1RowContents[4] = string.Empty;
                    }
                    a1RowContents[5] = string.Format("{0}", device.tempList.Count == 0 ? string.Empty : device.AlarmA1Status);
                }
                else
                {
                    a1RowContents[0] = "A1: ";
                }
                //a5
                if (!string.IsNullOrEmpty(device.A2))
                {
                    a2RowContents[0] = string.Format("A2: over {0}°{1}", device.A2, device.TempUnit);
                    a2RowContents[1] = string.Format("{0}({1})", TempsenFormatHelper.ConvertSencondToFormmatedTime(device.AlarmDelayA2.ToString()), device.AlarmTypeA2);
                    a2RowContents[2] = string.Format("{0}", device.tempList.Count == 0 ? string.Empty : device.AlarmTotalTimeA2);
                    a2RowContents[3] = string.Format("{0}", device.tempList.Count == 0 ? string.Empty : device.AlarmNumA2.ToString());
                    if (device.AlarmNumA2 > 0)
                    {
                        a2RowContents[4] = string.Format("{0}", Convert.ToDateTime(device.AlarmA2First) == DateTime.MinValue ? string.Empty : TempsenFormatHelper.GetFormattedDateTime(Convert.ToDateTime(device.AlarmA2First).ToLocalTime()));
                    }
                    else
                    {
                        a2RowContents[4] = string.Empty;
                    }
                    a2RowContents[5] = string.Format("{0}", device.tempList.Count == 0 ? string.Empty : device.AlarmA2Status);
                }
                else
                {
                    a2RowContents[0] = "A2: ";
                }
                //a4
                if (!string.IsNullOrEmpty(device.A3))
                {
                    a3RowContents[0] = string.Format("A3: over {0}°{1}", device.A3, device.TempUnit);
                    a3RowContents[1] = string.Format("{0}({1})", TempsenFormatHelper.ConvertSencondToFormmatedTime(device.AlarmDelayA3.ToString()), device.AlarmTypeA3);
                    a3RowContents[2] = string.Format("{0}", device.tempList.Count == 0 ? string.Empty : device.AlarmTotalTimeA3);
                    a3RowContents[3] = string.Format("{0}", device.tempList.Count == 0 ? string.Empty : device.AlarmNumA3.ToString());
                    if (device.AlarmNumA3 > 0)
                    {
                        a3RowContents[4] = string.Format("{0}", Convert.ToDateTime(device.AlarmA3First) == DateTime.MinValue ? string.Empty : TempsenFormatHelper.GetFormattedDateTime(Convert.ToDateTime(device.AlarmA3First).ToLocalTime()));
                    }
                    else
                    {
                        a3RowContents[4] = string.Empty;
                    }
                    a3RowContents[5] = string.Format("{0}", device.tempList.Count == 0 ? string.Empty : device.AlarmA3Status);
                }
                else
                {
                    a3RowContents[0] = "A3: ";
                }
                //a3
                a4RowContents[0] = string.Format("A4: {0} to {1}°{2}", device.A4, device.A3, device.TempUnit);
                a4RowContents[1] = string.Format("{0}", "Unlimited");
                a4RowContents[2] = string.Format("{0}", device.tempList.Count == 0 ? string.Empty : device.AlarmTotalTimeIdeal);
                //a2
                if (!string.IsNullOrEmpty(device.A4))
                {
                    a5RowContents[0] = string.Format("A5: under {0}°{1}", device.A4, device.TempUnit);
                    a5RowContents[1] = string.Format("{0}({1})", TempsenFormatHelper.ConvertSencondToFormmatedTime(device.AlarmDelayA4.ToString()), device.AlarmTypeA4);
                    a5RowContents[2] = string.Format("{0}", device.tempList.Count == 0 ? string.Empty : device.AlarmTotalTimeA4);
                    a5RowContents[3] = string.Format("{0}", device.tempList.Count == 0 ? string.Empty : device.AlarmNumA4.ToString());
                    if (device.AlarmNumA4 > 0)
                    {
                        a5RowContents[4] = string.Format("{0}", Convert.ToDateTime(device.AlarmA4First) == DateTime.MinValue ? string.Empty : TempsenFormatHelper.GetFormattedDateTime(Convert.ToDateTime(device.AlarmA4First).ToLocalTime()));
                    }
                    else
                    {
                        a5RowContents[4] = string.Empty;
                    }
                    a5RowContents[5] = string.Format("{0}", device.tempList.Count == 0 ? string.Empty : device.AlarmA4Status);
                }
                else
                {
                    a5RowContents[0] = "A5: ";
                }
                //a1
                if (!string.IsNullOrEmpty(device.A5))
                {
                    a6RowContents[0] = string.Format("A6: under {0}°{1}", device.A5, device.TempUnit);
                    a6RowContents[1] = string.Format("{0}({1})", TempsenFormatHelper.ConvertSencondToFormmatedTime(device.AlarmDelayA5.ToString()), device.AlarmTypeA5);
                    a6RowContents[2] = string.Format("{0}", device.tempList.Count == 0 ? string.Empty : device.AlarmTotalTimeA5);
                    a6RowContents[3] = string.Format("{0}", device.tempList.Count == 0 ? string.Empty : device.AlarmNumA5.ToString());
                    if (device.AlarmNumA5 > 0)
                    {
                        a6RowContents[4] = string.Format("{0}", Convert.ToDateTime(device.AlarmA5First) == DateTime.MinValue ? string.Empty : TempsenFormatHelper.GetFormattedDateTime(Convert.ToDateTime(device.AlarmA5First).ToLocalTime()));
                    }
                    else
                    {
                        a6RowContents[4] = string.Empty;
                    }
                    a6RowContents[5] = string.Format("{0}", device.tempList.Count == 0 ? string.Empty : device.AlarmA5Status);
                }
                else
                {
                    a6RowContents[0] = "A6: ";
                }
            }
            return result;
        }

        public bool IsStringArrayNotEmpty(string[] array)
        {
            bool result = false;
            foreach (var item in array)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    result = true;
                }
            }
            return result;
        }

    }
}
