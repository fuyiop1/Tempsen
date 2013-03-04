using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

namespace temptest
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

       
        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.label1.Text = "终于完成了！谢谢等待";
        }

        /*
        public static void ALLReport()
        {
            Dictionary<string, string> comlist = SerialHelper.GetComList();


            foreach (string name in comlist.Keys)
            {
                SerialPortTran spt = new SerialPortTran(name, false);
                var dinfo = new DeviceInfo(spt.SendGetInfo());
                string filename = dinfo.sn + ".txt";
                if (File.Exists(filename)) File.Delete(filename);
                StreamWriter sw = new StreamWriter(filename);

                Console.WriteLine("===============" + name + DateTime.Now.ToString() + "===========================================================================");
                sw.WriteLine("===============" + name + DateTime.Now.ToString() + "===========================================================================");


                string deviceinfo = dinfo.ToString();
                Console.WriteLine(deviceinfo);
                sw.WriteLine(deviceinfo);
                var dsetting = new DeviceSetting(spt.SentGetSetting());
                string devicesetting = dsetting.ToString();
                Console.WriteLine(devicesetting);
                sw.WriteLine(devicesetting);
                string temps = spt.SentGetRecords();
                //Console.WriteLine(temps);
                string s = TempSenHelper.GetTempListCString(temps, spt.ItemCount, dinfo.RecordDateTime, dsetting.recordInterval);
                sw.WriteLine(s);
                //Console.WriteLine(s);
                Console.WriteLine("===============end" + DateTime.Now.ToString() + "===========================================================================");
                sw.WriteLine("===============end" + DateTime.Now.ToString() + "===========================================================================");

                sw.Close();

                Process vProcess = Process.Start(Directory.GetCurrentDirectory() + "\\" + filename);
            }




        }
        */
    }
}
