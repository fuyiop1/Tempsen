using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using TempSenLib;
using temptest;

namespace ITAG_TEST
{
    public partial class mForm : Form
    {
        private TempSen.device ITAG;
        public mForm()
        {
            InitializeComponent();
            ITAG = new TempSen.device(10);
            groupBox2.Enabled = false;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            label1.Text = ""; 
            if (button1.Text == "connectDevice")
            {
                if (ITAG.connectDevice() == true)
                {
                    groupBox2.Enabled = true; 
                    label1.ForeColor = Color.Blue; 
                    label1.Text = "Found ITAG-SingleUse Temperature Label!";
                    button1.Text = "disconnectDevice";
                }
                else { 
                    groupBox2.Enabled = false; 
                    label1.ForeColor = Color.Red; 
                    label1.Text = "Please connect to ITAG-SingleUse Temperature Label!"; 
                }
            }
            else { ITAG.disconnectDevice(); button1.Text = "connectDevice"; }
        }

        private void button2_Click(object sender, EventArgs e)
        { string[] tempStr; tempStr = ITAG.getAlarmSet(); textBox1.Lines = tempStr; }
        private void button3_Click(object sender, EventArgs e)
        { string[] tempStr; tempStr = ITAG.getConfig(); textBox1.Lines = tempStr; }
        private void button4_Click(object sender, EventArgs e)
        {
            string[] tempStr; tempStr = ITAG.getRecord(1); textBox1.Lines = tempStr; //foreach (string s in tempStr) //{ // //listBox1.Items.Add(s); //}
        }

        private void button5_Click(object sender, EventArgs e)
        { string[] tempStr; tempStr = ITAG.getRecord(2); textBox1.Lines = tempStr; }


        private void button6_Click(object sender, EventArgs e)
        { string[] tempStr; 
            tempStr = ITAG.getAnalysis(); 
            textBox1.Lines = tempStr;
        
        }

        private void button8_Click(object sender, EventArgs e)
        {
            string[] tempStr;
            tempStr = ITAG.getOtherInfo();
            textBox1.Lines = tempStr;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Report();

            this.textBox1.Text = "终于完成了！谢谢等待\r\n" + this.textBox1.Text;
        }

        public string Report()
        {
            {
                SerialPortTran spt = this.ITAG.spt;
                var dinfo = new DeviceInfo(spt.SendGetInfo());
                string filename = dinfo.sn + ".txt";
                if (File.Exists(filename)) File.Delete(filename);
                StreamWriter sw = new StreamWriter(filename);

                Console.WriteLine("===============" +  DateTime.Now.ToString() + "===========================================================================");
                sw.WriteLine("===============" +  DateTime.Now.ToString() + "===========================================================================");


                string deviceinfo = dinfo.ToString();
                Console.WriteLine(deviceinfo);
                sw.WriteLine(deviceinfo);
                var dsetting = new DeviceSetting(spt.SentGetSetting());
                string devicesetting = dsetting.ToString();
                Console.WriteLine(devicesetting);
                sw.WriteLine(devicesetting);
                string temps = spt.SentGetRecords();
                //Console.WriteLine(temps);
                string s = TempSenHelper.GetTempListCString(temps, spt.ItemCount, dinfo.RecordDateTime, dsetting.recordIntervalInSecond);
                sw.WriteLine(s);
                //Console.WriteLine(s);
                Console.WriteLine("===============end" + DateTime.Now.ToString() + "===========================================================================");
                sw.WriteLine("===============end" + DateTime.Now.ToString() + "===========================================================================");

                sw.Close();
                this.textBox1.Text = File.ReadAllText(filename, Encoding.UTF8);

                Process vProcess = Process.Start(Directory.GetCurrentDirectory() + "\\" + filename);
                return "";
            }




        }

        private void button9_Click(object sender, EventArgs e)
        {
            new Form2().ShowDialog();
        }

        

    }
}
