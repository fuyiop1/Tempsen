using System;
using System.Collections.Generic;
using System.Text;

namespace TempSen
{
    public class Demo
    {
        public static void Main()
        {
            TempSen.device ITAG = new TempSen.device(10); // or 11
            if (ITAG.connectDevice() == true) // search for an usable device and  connect to it .
            {
                string[] AlarmSet = ITAG.getAlarmSet();
                string[] Config = ITAG.getConfig();
                /// 1 – request temperature list(temperature in Celsius). 2 – request temperature(temperature in Celsius), date and time list.
                string[] Record1 = ITAG.getRecord(1);
                string[] Record2 = ITAG.getRecord(2);
                string[] Analysis = ITAG.getAnalysis();
                string[] OtherInfo = ITAG.getOtherInfo();
            }

            ITAG.disconnectDevice();
        }
    }
}
