using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TempSenLib;
using System.Threading;
using System.Collections;
using TempSen;
using UsbLibrary;
using TempsenLibHid.PDF;
using System.Reflection;

namespace ConsoleApplication1
{
    class Program
    {
       
        static void Main(string[] args)
        {
            //test device temp convert
            for (float i = -200; i < 1000; i+=0.1F)
            {
                Console.Write("float: " + i.ToString());
                var hex= TempValueToHex(i.ToString());
                Console.Write("\t  TempValueToHex" + hex);
                Console.Write("\t  HexToTempValue" + HexToTempValue(hex));
                Console.WriteLine();
            }


            //////////////////////////////////////////
            DevicePDF dev = new DevicePDF();
            dev.connectDevice();


            dev.DoRead();
            //dev.Data.Description = "tttttest";
            //dev.Data.Operator = "zhurenjie did";
            var data = dev.Data;
            Type t = data.GetType();
            PropertyInfo[] infos = t.GetProperties();
            foreach (PropertyInfo info in infos)
            {
                var value = t.GetProperty(info.Name).GetValue(dev.Data,null);

                //t.GetProperty(info.Name).SetValue(dev.Data, 111);
                Console.WriteLine(info.Name+":"+value);
                if (value.GetType() == typeof(string[]))
                {
                    string[] vs = (string[])value;
                    for (int i = 0; i < vs.Length; i++) 
                        Console.WriteLine(info.Name + i.ToString()+":" + vs[i]);
                }
                
                if (value.GetType() == typeof(List<string>))
                {
                    Console.WriteLine("press enter to show List:");
                    Console.ReadLine();
                    List<string> vs = (List<string>)value;
                    for (int i = 0; i < vs.Count; i++)
                        Console.WriteLine(info.Name + i.ToString() + ":" + vs[i]);
                }
            }
            
            

            Console.WriteLine("press enter to Write:");
            Console.ReadLine();
            //dev.Data.Description = "20110916";
            //dev.Data.LogInterval = 1;
            //dev.Data.LogCycle = 1;
            //dev.Data.RunMode = 1;
            //dev.Data.StartMode = "F8";
            //dev.Data.TripNo = "xxxxxxx";
            //dev.Data.Description = "abcdefghijklmnopqrstuvwxyz";


            dev.DoWrite();
            Console.WriteLine("press enter to exit:");
            Console.ReadLine();
            //Console.ReadLine();

            //dev = new DevicePDF();
            //dev.connectDevice();
            ////start get data ..
            //Console.WriteLine(dev.Data.AlarmDelay);

            ////the end .
            //////////////////////////////////////////////
            
            
            //string status=dev.QueryStatus();
            
            //Console.WriteLine(status + "       " + DateTime.Now.ToString());
            //Console.WriteLine(dev.AllData.Count.ToString() + "       " + DateTime.Now.ToString());
            //string row = "";
            //row=dev.GetValue(0, 1);
            //dev.DoRead();
            //row = dev.GetValue(0, 1);

            //Console.WriteLine(dev.AllData.Count.ToString() + "       " + DateTime.Now.ToString());
            //for (int i = 0; i < 600; i++)
            //{
            //    for (int j = 0; j < 8; j++)
            //        Console.WriteLine(i.ToString("X4") + j.ToString("X2") + " " + dev.GetValue(i, j));
            //}
            ////DeviceData dd = new DeviceData(dev);
            //var dd = dev.Data;

            //Console.WriteLine(dev.AllData.Count.ToString() + "       " + DateTime.Now.ToString());


            ////SpecifiedDevice dev = SpecifiedDevice.FindSpecifiedDevice(0x04d8, 0x0054);
            ////dev.DataSend += new DataSendEventHandler(dev_DataSend);
            ////dev.DataRecieved += new DataRecievedEventHandler(dev_DataRecieved);
            ////dev.SendData(HexToByte("68C000000000000000000000000000000000000000000000000000000000000000000000000000AA")); Thread.Sleep(1000);
            ////InputReport irp = dev.CreateInputReport();
            ////Console.WriteLine("IRP:" + ToHexString(irp.Buffer));
            ////dev.SendData(HexToByte("68C000000000000000000000000000000000000000000000000000000000000000000000000000AA")); Thread.Sleep(1000);
            ////irp = dev.CreateInputReport();
            ////Console.WriteLine("IRP:" + ToHexString(irp.Buffer));
            ////for (int i = 0; i < 1000; i++)
            ////{
            ////    for (int j = 0; j < 8; j++)
            ////    {
            ////        dev.SendData(HexToByte("68AA31"+i.ToString("X4") + j.ToString("X2") + "000000000000000000000000000000000000000000000000000000000000000000AA")); 
            ////        irp = dev.CreateInputReport();
            ////        Console.WriteLine("IRP:" + ToHexString(irp.Buffer));
            ////    }
            ////    Thread.Sleep(100);
            ////}
            
            

        }


        private static string HexToTempValue(string str2ByteHex)
        {
            try
            {
                byte[] bytes = Utils.HexToByte(str2ByteHex);
                float temp = bytes[0] * 16 + bytes[1] / 16 + 0.1F * (bytes[1] % 16);
                string rst = (temp - 200).ToString("F1");
                return rst;
            }
            catch
            {
                return HexToTempValue("FFFF");
            }
        }
        private static string TempValueToHex(string tempValue)
        {
            var temp = float.Parse(tempValue);
            temp += 200;
            string[] bytes = new string[2];
            try
            {
                bytes[0] = ((int)temp / 16).ToString("X2");
            }
            catch { bytes[0] = "00"; }
            try
            {
                bytes[1] = ((int)temp % 16).ToString("X1") + ((byte)(temp * 10 % 10)).ToString("X1");
            }
            catch { bytes[1] = "00"; }
            string rst = bytes[0] + bytes[1];
            return rst;
        }


        static void dev_DataRecieved(object sender, DataRecievedEventArgs args)
        {
            Console.WriteLine("Rec:     " + ToHexString(args.data));
        }

        static void dev_DataSend(object sender, DataSendEventArgs args)
        {
            Console.WriteLine("Send:        " + ToHexString(args.data));
        }

        public static byte[] HexToByte(string hexString)
        {
            if (string.IsNullOrEmpty(hexString))
            {
                hexString = "00";
            }
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }
        public static string ToHexString(byte[] bytes) // 0xae00cf => "AE00CF "
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }

    }
}
