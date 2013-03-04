using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TempSen;
using UsbLibrary;
using System.Reflection;

namespace temptest
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
            dev = new DevicePDF();
            groupBox2.Enabled = false;
        }
        #region demo for device plugin and remove
        
        /// <summary>
        /// This event will be triggered when a device is pluged into your usb port on
        /// the computer. And it is completly enumerated by windows and ready for use.
        /// </summary>
        
        public event EventHandler OnDeviceChange;


        protected override void OnHandleCreated(EventArgs e)
        {
            Win32Usb.RegisterForUsbEvents(this.Handle, Win32Usb.HIDGuid);
        }

        protected override void WndProc(ref Message m)
        {
            ParseMessages(ref m);
            base.WndProc(ref m);
        }
        /// <summary>
        /// This method will filter the messages that are passed for usb device change messages only. 
        /// And parse them and take the appropriate action 
        /// </summary>
        /// <param name="m">a ref to Messages, The messages that are thrown by windows to the application.</param>
        /// <example> This sample shows how to implement this method in your form.
        /// <code> 
        ///</code>
        ///</example>
        public void ParseMessages(ref Message m)
        {
            if (m.Msg == Win32Usb.WM_DEVICECHANGE)	// we got a device change message! A USB device was inserted or removed
            {
                OnDeviceChange(this, new EventArgs());
                
            }
        }

        #endregion


        public DevicePDF dev;


        private void button2_Click(object sender, EventArgs e)
        {
            if (button2.Text == "connectDevice")
            {
                if (dev.connectDevice() == true)
                {
                    groupBox2.Enabled = true;
                    label1.ForeColor = Color.Blue;
                    this.label1.Text = "connected to " + HIDDevice.DevicePath;
                    button2.Text = "disconnectDevice";
                }
                else
                {
                    groupBox2.Enabled = false;
                    label1.ForeColor = Color.Red;
                    this.label1.Text = "can not find any device.";
                }
            }
            else { dev.disconnectDevice(); button2.Text = "connectDevice"; }


                
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dev.DoRead();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            dev.DoWrite();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var data = dev.Data;
            Type t = data.GetType();
            PropertyInfo[] infos = t.GetProperties();
            List<string> result = new List<string>();
            foreach (PropertyInfo info in infos)
            {
                var value = t.GetProperty(info.Name).GetValue(dev.Data, null);
               
                if (value.GetType() == typeof(string[]))
                {
                    string[] vs = (string[])value;
                    for (int i = 0; i < vs.Length; i++)
                    {
                        try
                        {
                        
                        Console.WriteLine(info.Name + i.ToString() + ":" + vs[i]);
                        result.Add(info.Name + i.ToString() + ":" + vs[i]);
                        }
                        catch (System.Exception ex)
                        {
                            result.Add(ex.Message + ex.StackTrace);
                        }
                    }
                    continue;
                }
                if (value.GetType() == typeof(int[]))
                {
                    
                    int[] vs = (int[])value;
                    for (int i = 0; i < vs.Length; i++)
                    {
                        try{
                        Console.WriteLine(info.Name + i.ToString() + ":" + vs[i]);
                        result.Add(info.Name + i.ToString() + ":" + vs[i]);
                        }
                        catch (System.Exception ex)
                        {
                            result.Add(ex.Message + ex.StackTrace);
                        }
                    }

                    continue;
                }
                if (value.GetType() == typeof(DateTime[]))
                {
                    DateTime[] vs = (DateTime[])value;
                    for (int i = 0; i < vs.Length; i++)
                    {
                        try{
                        Console.WriteLine(info.Name + i.ToString() + ":" + vs[i]);
                        result.Add(info.Name + i.ToString() + ":" + vs[i]);
                        }
                        catch (System.Exception ex)
                        {
                            result.Add(ex.Message + ex.StackTrace);
                        }
                    }
                    continue;
                }

                if (value.GetType() == typeof(List<string>))
                {
                    //Console.WriteLine("press enter to show List:");
                    //Console.ReadLine();
                    //List<string> vs = (List<string>)value;
                    //for (int i = 0; i < vs.Count; i++)
                    //    Console.WriteLine(info.Name + i.ToString() + ":" + vs[i]);
                    continue;
                }
                try{
                Console.WriteLine(info.Name + ":" + value);
                result.Add(info.Name + ":" + value);
                }
                catch (System.Exception ex)
                {
                    result.Add(ex.Message + ex.StackTrace);

                }
            }
            this.textBox1.Lines = result.ToArray();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            List<string> result = new List<string>();

            List<string> vs = dev.Data.TempList;
            //for (int i = 0; i < vs.Count; i++)
            //    Console.WriteLine(info.Name + i.ToString() + ":" + vs[i]);


            this.textBox1.Lines = vs.ToArray();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            List<string> result = new List<string>();

            List<string> vs = dev.Data.TempListWithTime;
            //for (int i = 0; i < vs.Count; i++)
            //    Console.WriteLine(info.Name + i.ToString() + ":" + vs[i]);


            this.textBox1.Lines = vs.ToArray();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> result = new List<string>();

            result.Add("AlarmMode:" + dev.Data.AlarmMode+"     [注：0为单次报警(Single Alarm)，1为多次报警( Multi Alarm)]");
            result.Add("Alarm0: ");

            if ((dev.Data.AlarmType0 & 0x80) > 0)
            {
                result.Add(((dev.Data.AlarmType0 & 0x80) > 0 ? "启用" : "未启用") + "-" + ((dev.Data.AlarmType0 & 0x40) > 0 ? "上限报警" : "下限报警") + "-" + ((dev.Data.AlarmType0 & 0x01) > 0 ? "累计报警" : "单次报警"));
                result.Add("阀值:" + dev.Data.AlarmLimits0.ToString() + "--延时:" + dev.Data.AlarmDelay0);
            }
            else
            {
                result.Add("未启用");
                result.Add("--");
            }
            result.Add("");
            result.Add("Alarm1:");

            if ((dev.Data.AlarmType1 & 0x80) > 0)
            {
                result.Add(((dev.Data.AlarmType1 & 0x80) > 0 ? "启用" : "未启用") + "-" + ((dev.Data.AlarmType1 & 0x40) > 0 ? "上限报警" : "下限报警") + "-" + ((dev.Data.AlarmType1 & 0x01) > 0 ? "累计报警" : "单次报警"));
                result.Add("阀值:" + dev.Data.AlarmLimits1.ToString() + "--延时:" + dev.Data.AlarmDelay1);
            }
            else
            {
                result.Add("未启用");
                result.Add("--");
            }
            result.Add("");
            result.Add("Alarm2:");

            if ((dev.Data.AlarmType2 & 0x80) > 0)
            {
                result.Add(((dev.Data.AlarmType2 & 0x80) > 0 ? "启用" : "未启用") + "-" + ((dev.Data.AlarmType2 & 0x40) > 0 ? "上限报警" : "下限报警") + "-" + ((dev.Data.AlarmType2 & 0x01) > 0 ? "累计报警" : "单次报警"));
                result.Add("阀值:" + dev.Data.AlarmLimits2.ToString() + "--延时:" + dev.Data.AlarmDelay2);
            }
            else
            {
                result.Add("未启用");
                result.Add("--");
            }
            result.Add("");
            result.Add("Alarm3:");

            if ((dev.Data.AlarmType3 & 0x80) > 0)
            {
                result.Add(((dev.Data.AlarmType3 & 0x80) > 0 ? "启用" : "未启用") + "-" + ((dev.Data.AlarmType3 & 0x40) > 0 ? "上限报警" : "下限报警") + "-" + ((dev.Data.AlarmType3 & 0x01) > 0 ? "累计报警" : "单次报警"));
                result.Add("阀值:" + dev.Data.AlarmLimits3.ToString() + "--延时:" + dev.Data.AlarmDelay3);
            }
            else
            {
                result.Add("未启用");
                result.Add("--");
            }
            result.Add("");
            result.Add("Alarm4:");

            if ((dev.Data.AlarmType4 & 0x80) > 0)
            {
                result.Add(((dev.Data.AlarmType4 & 0x80) > 0 ? "启用" : "未启用") + "-" + ((dev.Data.AlarmType4 & 0x40) > 0 ? "上限报警" : "下限报警") + "-" + ((dev.Data.AlarmType4 & 0x01) > 0 ? "累计报警" : "单次报警"));
                result.Add("阀值:" + dev.Data.AlarmLimits4.ToString() + "--延时:" + dev.Data.AlarmDelay4);
            }
            else
            {
                result.Add("未启用");
                result.Add("--");
            }
            result.Add("");

            this.textBox1.Lines = result.ToArray();
        }
    }
}
