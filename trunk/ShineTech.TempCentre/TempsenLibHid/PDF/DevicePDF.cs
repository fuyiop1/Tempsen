using System;
using System.Collections.Generic;
using System.Text;
using Services.Common;
using TempSenLib;
using UsbLibrary;
using System.Threading;
using TempsenLibHid.PDF;

namespace TempSen
{
    public class DevicePDF
    {
        //private static ITracing _tracing = TracingManager.GetTracing(typeof(int));

        //public DevicePDF() : this(0x04d8, 0x0054) { }
        //public DevicePDF(int vid, int pid)
        //{
        //    VID = vid;
        //    PID = pid;
        //}
        public DeviceData Data { get; set; }
        //public int VID { get; set; }
        //public int PID { get; set; }
        public SpecifiedDevice spt;
        private DeviceInfo devInfo { get; set; }
        private DeviceSetting devSetting { get; set; }
        private DeviceTempList TempList { get; set; }
        public SortedList<string, string> AllData = new SortedList<string, string>();
        public object _lock = new object();




        private bool ReadDone = false;
        public bool connectDevice()
        {
            try
            {
                AllData = new SortedList<string, string>();
                ReadDone = false;
                while (spt == null)
                {
                    spt = SpecifiedDevice.FindSpecifiedDevice();
                    if (spt == null)
                    {
                        Thread.Sleep(3000);
                    }
                    else
                    {
                        break;
                    }
                }
                spt.DataRecieved += new DataRecievedEventHandler(spt_DataRecieved);
                Data = new DeviceData(this);

                //try {
                //    if (Data.RunStatus == 2)
                //        StopRecord();
                //}
                //catch { 
                //}

                return true;

            }
            catch
            {
                //_tracing.Error("can not found any device connected");
                return false;
            }
        }

        private void spt_DataRecieved(object sender, DataRecievedEventArgs args)
        {

            string value = Utils.ToHexString(args.data);
            string head = value.Substring(0, 4);
            string key = "";
            string data = "";
            switch (head)
            {
                case "68AB"://read reponse
                    key = value.Substring(0, 12);
                    data = value.Substring(12, 64);
                    lock (_lock)
                        AllData[key] = data;
                    break;
                case "6856"://write response
                    key = value.Substring(0, 12);
                    data = value.Substring(12, 64);
                    lock (_lock)
                        AllData[key] = data;

                    //key = PDFCmd.ReadResponse + value.Substring(6, 6);
                    //data = value.Substring(12, 64);
                    //lock (_lock)
                    //    AllData[key] = data;
                    break;
                case "68C3":// cmd exec
                    key = value.Substring(0, 10);
                    data = value.Substring(10, 68);
                    lock (_lock)
                        AllData[key] = data;
                    break;
                default:
                    key = value.Substring(0, 4);
                    data = value.Substring(4, 72);
                    lock (_lock)
                        AllData[key] = data;
                    break;

            }
        }
        public bool softDisconnectDevice()
        {
            AllData = new SortedList<string, string>();
            return true;
        }

        public bool disconnectDevice()
        {

            ReadDone = false;
            try
            {
                if (spt != null)
                {
                    spt.DataRecieved -= new DataRecievedEventHandler(spt_DataRecieved);
                    spt.Dispose();
                    GC.Collect(2);
                }
                spt = null;

                GC.Collect(2);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public bool Reset()
        {
            try
            {
                if (spt != null)
                {
                    spt.DataRecieved -= new DataRecievedEventHandler(spt_DataRecieved);

                    spt.Dispose();
                }
                spt = null;
                spt = SpecifiedDevice.FindSpecifiedDevice();
                spt.DataRecieved += new DataRecievedEventHandler(spt_DataRecieved);
                if (spt == null)
                    return false;
                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// 存值
        /// </summary>
        /// <param name="page"></param>
        /// <param name="row"></param>
        /// <param name="index">字节数索引</param>
        /// <param name="length">字节数长度</param>
        /// <param name="value">对应hex string . 长度必须为字节数长度二倍。</param>
        /// <returns></returns>
        public bool SetValue(int page, int row, int index, int length, string value)
        {
            try
            {
                string key = PDFCmd.ReadResponse + page.ToString("X4") + row.ToString("X2");
                string rowvalue = GetValue(page, row);
                if (string.IsNullOrEmpty(rowvalue))
                    throw new Exception("Can not get value");
                if (value.Length != length * 2)
                    throw new Exception("400 invalid value and length");
                string result = rowvalue.Substring(0, index * 2) + value + rowvalue.Substring(index * 2 + length * 2);

                //if (AllData.ContainsKey(key))
                {
                    lock (_lock)
                        AllData[key] = result;
                }



                return true;
            }
            catch
            {
                return false;
            }

        }

        /// <summary>
        /// 写入时，一定要按顺序从PAGE1的第一行开始写。连续写入
        /// </summary>
        /// <returns></returns>
        public bool DoWrite()
        {
            Data.WriteTime = DateTime.UtcNow;
            //if (Data.StartConditionTime < Data.WriteTime)
            //    Data.StartConditionTime = Data.WriteTime;
            return DoWrite(3);
        }
        private bool DoWrite(int trytimes)
        {
            try
            {
                string writePages = "0";
                try
                {
                    writePages = System.Configuration.ConfigurationManager.AppSettings["WritePages"];
                    if (string.IsNullOrEmpty(writePages))
                        writePages = "0";
                }
                catch
                {
                    writePages = "0";
                }
                string[] pages = writePages.Split(',');

                foreach (string page in pages)
                {
                    int i = 0;
                    try
                    {
                        i = int.Parse(page);
                    }
                    catch {
                        i = 0;
                    }
                    for (int j = 0; j < 8; j++)
                    {
                        if (i == 0 && j == 0)
                        {
                            string value = Utils.FillString("", 44, 'F') + GetValue(i, j).SubStringEx(44,20);
                            spt.WriteRow(i, j, value);
                        }
                        else
                            spt.WriteRow(i, j, GetValue(i, j));

                        string key = PDFCmd.WriteResponse + i.ToString("X4") + j.ToString("X2");
                        int t = 500;
                        while (--t > 0)
                        {
                            if (!AllData.ContainsKey(key))
                                Thread.Sleep(10);
                            else
                            {
                                Console.WriteLine("write reponse:" + key + AllData[key]);
                                break;
                            }
                        }
                    }
                }
                AllData = new SortedList<string, string>();
                return true;
            }
            catch (Exception ex)
            {
                if (trytimes > 0)
                    return DoWrite(--trytimes);
                else
                    return false;
            }
        }
        public string GetValue(int page, int row)
        {
            return GetValue(page, row, 3);
        }
        private string GetValue(int page, int row, int trytimes)
        {
            string key = PDFCmd.ReadResponse + page.ToString("X4") + row.ToString("X2");
            if (!AllData.ContainsKey(key))
            {
                spt.ReadRow(page, row);
                int tryt = 100;
                while (tryt-- > 0)
                {
                    lock (_lock)
                        if (AllData.ContainsKey(key))
                            break;
                    Thread.Sleep(1);
                }

            }
            if (!AllData.ContainsKey(key))
            {
                if (trytimes > 0)
                {
                    //disconnectDevice();
                    //connectDevice();
                    return GetValue(page, row, --trytimes);
                }
                else
                {
                    Reset();
                    return "";
                }

            }
            else
            {

                lock (_lock)
                    return AllData[key];
            }

        }
        public void DoRead()
        {
            DoRead(false);
        }

        private void DoRead(bool fouceRead)
        {
            if (!ReadDone || fouceRead)
            {
                try
                {
                    if (spt == null)
                    {
                        connectDevice();
                    }
                    int points = Data.LogCount;
                    int maxpage = 10 + points / 128;
                    spt.ReadAll(maxpage);
                    ReadDone = true;
                }
                catch (Exception ex)
                {
                    ReadDone = false;
                    Console.WriteLine(ex.Message + ex.StackTrace);
                    Reset();
                    //throw new Exception("do read failed.");
                }
            }

            if (devInfo != null && devSetting != null && TempList != null)
                ReadDone = true;


        }

        public string QueryStatus()
        {
            return QueryStatus(3);
        }
        private string QueryStatus(int trytimes)
        {
            string key = PDFCmd.QueryResponse;
            if (!AllData.ContainsKey(key))
            {
                spt.QueryStatus();
                int tryt = 100;
                while (tryt-- > 0)
                {
                    lock (_lock)
                        if (AllData.ContainsKey(key))
                            break;
                    Thread.Sleep(1);
                }

            }
            if (!AllData.ContainsKey(key))
            {
                if (trytimes > 0)
                {
                    return QueryStatus(--trytimes);
                }
                else
                {
                    return "";
                }

            }
            else
            {

                lock (_lock)
                    return AllData[key];
            }

        }
        public string StopRecord()
        {
            return StopRecord(3);
        }
        private string StopRecord(int trytimes)
        {
            string key = PDFCmd.StopRecordResponse;
            if (!AllData.ContainsKey(key))
            {
                spt.StopRecord();
                int tryt = 100;
                while (tryt-- > 0)
                {
                    lock (_lock)
                        if (AllData.ContainsKey(key))
                            break;
                    Thread.Sleep(100);
                }

            }
            if (!AllData.ContainsKey(key))
            {
                if (trytimes > 0)
                {
                    return StopRecord(--trytimes);
                }
                else
                {
                    return "";
                }

            }
            else
            {

                lock (_lock)
                    return AllData[key];
            }

        }

    }
    public class StatusPDF
    {
        public static readonly string OK = "OK";
        public static readonly string NoDev = "Error:Cann't Found ITAG-SingleUse Temperature Label!";
        public static readonly string NoSet = "Error: Can't get alarm settings!";
        public static readonly string NoConfig = "Error: Can't get alarm settings!";
        public static readonly string NoData = "Error: No valid data!";
    }

}
