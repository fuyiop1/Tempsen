using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using ITAG_TEST;
using TempSenLib;
using TempSen;
using System.Text;
using TempsenLibHid.PDF;
using System.Reflection;

namespace temptest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            



            //TempSenLib.DeviceInfo info = new TempSenLib.DeviceInfo("68171E0153158E010E6405AB05AB0B0B08010B25190B08010B25240B08020B30248A0A");

            //TempSenLib.DeviceSetting setting = new TempSenLib.DeviceSetting("68151D0153158E3C01019C1900FB0000010000000A0B08020B330E0E00002070E90A");

            
            /*
            TempSen.device ITAG = new TempSen.device(10); // or 11
            if (ITAG.connectDevice() == true) // search for an usable device and  connect to it .
            {
                string[] AlarmSet = ITAG.getAlarmSet();
                string[] Config = ITAG.getConfig();
                /// 1 – request temperature list(temperature in Celsius). 2 – request temperature(temperature in Celsius), date and time list.
                string[] Record1 = ITAG.getRecord(1);
                string[] Record2 = ITAG.getRecord(2);
                string[] Analysis = ITAG.getAnalysis();
            }

            ITAG.disconnectDevice();
            */

            //Process vProcess = Process.Start(Directory.GetCurrentDirectory() + "\\" + "20712002.txt");
            //int a = TempSenHelper.bytesToTemp(0xFF, 0xFF);
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                //Application.Run(new Form1());

                Application.Run(new mForm());
            }
            catch
            {
            }
        }
    }
}
