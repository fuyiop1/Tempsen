using System;
using System.Collections.Generic;
using System.Text;
using System.IO.Ports;
using System.Threading;
using TempSenLib;
using Services.Common;

namespace TempSenLib
{
    public class SerialPortTran
    {
        private static ITracing _tracing = Services.Common.TracingManager.GetTracing(typeof(SerialPortTran));
        public static bool IsTheDevice(string port)
        {
            SerialPortTran spt;
            try
            {
                SerialPortFixer.Execute(port);
                spt = new SerialPortTran(port, false);
                spt.SPWrite(Utils.HexToByte("681600000A"));
                string result="";
                Thread.Sleep(100);
                result = spt.spReadOnce();
                spt.sp.Close();
                
                if (result.StartsWith("6817"))
                    return true;
            }
            catch
            {
                return false;
            }
            finally
            { 
                
            }

            return false;
        }
        public SerialPortTran(string PortName) : this(PortName, true) { }
        public SerialPortTran(string PortName ,bool needReceive)
        {
            this.PortName = PortName;
            sp = new SerialPort(PortName, 19200, Parity.None, 8, StopBits.One);
            sp.ReceivedBytesThreshold = 5;
            sp.Handshake = Handshake.None;
            sp.DtrEnable = true;
            sp.ReadTimeout = 2000;
            sp.WriteTimeout = 2000;
            autoreceive = needReceive;
            SerialPortFixer.Execute(PortName);
            if (sp.IsOpen)
            {
                sp.Close();
                sp.Open();
            }
            else
            {
                sp.Open();
            }

            if (autoreceive)
                sp.DataReceived += new SerialDataReceivedEventHandler(ReceiveMessage);
            connected = true;

        }

        public void ReSet()
        {

            try
            {
                connected = false;
                sp.Close();
            //---------------------------------------------

            sp = new SerialPort(PortName, 19200, Parity.None, 8, StopBits.One);
            sp.ReceivedBytesThreshold = 5;
            sp.Handshake = Handshake.None;
            sp.DtrEnable = true;
            sp.ReadTimeout = 2000;
            sp.WriteTimeout = 2000;
            if (sp.IsOpen)
            {
                sp.Close();
                sp.Open();
            }
            else
            {
                sp.Open();
            }

            if (autoreceive)
                sp.DataReceived += new SerialDataReceivedEventHandler(ReceiveMessage);
            connected = true;
            }
            catch { }


        }

        public void Dispose() {
            connected = false;
            if (sp != null)
            {
                try
                {
                    sp.Close();
                    
                }
                catch { }
                sp = null;
            }
        
        }

        ~SerialPortTran()
        {
            Dispose();
        }
        public bool connected = true;
        public bool autoreceive = false;
        public SerialPort sp;
        public string PortName = "";
        public void SPWrite(byte[] bytes)
        {
            try
            {
                if (sp != null && sp.IsOpen)
                {
                    sp.Write(bytes, 0, bytes.Length);
                    Console.WriteLine("write:" + Utils.ToHexString(bytes));
                }
            }
            catch(Exception exp)
            {
                Console.WriteLine("write Exception:" + exp.Message);
            }
        }

        public string spReadOnce()
        {
            try
            {
                if (sp != null && sp.IsOpen)
                {
                    byte[] buffer = new byte[sp.BytesToRead];
                    sp.Read(buffer, 0, buffer.Length);
                    string str = Utils.ToHexString(buffer);
                    return str;
                }
                else
                    return string.Empty;
            }
            catch
            {
                return string.Empty;
            }
        }


        private string _spRead()
        {
            try
            {
                if (sp != null && sp.IsOpen)
                {
                    byte[] buffer = new byte[sp.BytesToRead];
                    sp.Read(buffer, 0, buffer.Length);
                    string str = Utils.ToHexString(buffer);
                    return str;
                }
                else
                    return string.Empty;
            }
            catch (Exception exp)
            {
                Console.WriteLine("read Exception:" + exp.Message);
                return string.Empty;
            }
        }

        public string spRead()
        {
            int timeout = 500;
            string result = "";
            while (timeout > 0)
            {
                result += _spRead();
                if (result.Length > 0)
                {
                    if(result.EndsWith("0A"))
                        break;
                }
                Thread.Sleep(3);
                timeout -= 3;
            }
            return result;
        
        }


        private void ReceiveMessage(object sender, SerialDataReceivedEventArgs e)
        {
            ReceiveMessage();
        }
        public void ReceiveMessage()
        {
            try
            {
                if (sp != null && sp.IsOpen)
                {
                    byte[] buffer = new byte[sp.BytesToRead];
                    sp.Read(buffer, 0, buffer.Length);
                    string str = Utils.ToHexString(buffer);
                    Console.WriteLine("Read:" + str);
                    str = str.Trim();
                    if (str.Length > 6)
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("ReceiveMessage" + ex.Message);
            }

        }

        #region for tempsen
        public string SendGetInfo()
        {
            try{
            return _SendGetInfo(3);
            }
            catch{
                ReSet();
                try
                {
                    return _SendGetInfo(3);
                }
                catch
                {
                    throw new Exception("read error ,maybe you usb device is broken .");
                }
            }
        
        }

        private string _SendGetInfo(int trytimes)
        {
            SPWrite(Utils.HexToByte("681600000A"));
            if(autoreceive)
                return "";
            string result = spRead();

            if (!CheckFCS(result) || !result.StartsWith("6817"))
            {
                if(trytimes>0)
                    return _SendGetInfo(--trytimes);
                else
                    throw new Exception("retry n times all faild .");
            }

            byte[] bytes = Utils.HexToByte(result);
            ItemCount = bytes[10] * 256 + bytes[11];
            if(ItemCount%2==0)
                DataEndAddr = (ItemCount * 3) / 2;
            else
                DataEndAddr = (ItemCount * 3 + 1) / 2;

           return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="recordtype">3byte hex string 
        /// 采样方式  (1byte) --- 采样方式（10s、60s）;采样总时间;采样间隔
        /// =>{B7、B8、B9}
        /// 10s  h/ s 
        /// 60s  d /m
        ///</param>
        /// <param name="alerttype">1byte hex string
        /// 报警设置  (1byte) --- 0x00 禁止[上下限无效]
        ///	0x8C 上限[摄氏]   0x1C 下限[摄氏]
		/// 0x8F 上限[华氏]   0x1F 下限[华氏]
        ///</param>
        /// <param name="upTemp"></param>
        /// <param name="downTemp"></param>
        /// <param name="alertDealy"></param>
        /// <param name="startDelay"></param>
        /// <param name="dtnow"></param>
        /// <param name="isFastRecord"></param>
        /// <returns></returns>
        public bool SendSetConfigInfo(int sn, string recordtype, string alerttype,double upTemp,double downTemp,DateTime alertDelay,DateTime startDelay,DateTime dtnow,bool isFastRecord)
        { return _SendSetConfigInfo(sn, recordtype, alerttype, upTemp, downTemp, alertDelay, startDelay, dtnow, isFastRecord,3); }
        private bool _SendSetConfigInfo(int sn, string recordtype, string alerttype, double upTemp, double downTemp, DateTime alertDelay,
            DateTime startDelay, DateTime dtNow, bool isFastRecord, int trytimes)
        {

            string head = "681222";
            StringBuilder msgsb = new StringBuilder();
            msgsb.Append(Utils.IntToHexString(sn,4));
            msgsb.Append(recordtype);
            msgsb.Append(alerttype);
            msgsb.Append(Utils.ToHexString(TempSenHelper.TempDouble2Byte(upTemp)));
            msgsb.Append(Utils.ToHexString(TempSenHelper.TempDouble2Byte(downTemp)));

            msgsb.Append(Utils.IntToHexString(alertDelay.Hour % 100, 1));
            msgsb.Append(Utils.IntToHexString(alertDelay.Minute % 100, 1));
            msgsb.Append(Utils.IntToHexString(alertDelay.Second % 100, 1));
            msgsb.Append(Utils.IntToHexString(startDelay.Hour % 100, 1));
            msgsb.Append(Utils.IntToHexString(startDelay.Minute % 100, 1));
            msgsb.Append(Utils.IntToHexString(startDelay.Second % 100, 1));


            msgsb.Append(Utils.IntToHexString(dtNow.Year % 100, 1));
            msgsb.Append(Utils.IntToHexString(dtNow.Month % 100, 1));
            msgsb.Append(Utils.IntToHexString(dtNow.Day % 100, 1));
            msgsb.Append(Utils.IntToHexString(dtNow.Hour % 100, 1));
            msgsb.Append(Utils.IntToHexString(dtNow.Minute % 100, 1));
            msgsb.Append(Utils.IntToHexString(dtNow.Second % 100, 1));
            if (isFastRecord)
                msgsb.Append("FF");
            else
                msgsb.Append("00");

            string msg = msgsb.ToString();
            msg = AddFCS(msg);
            string end = "0A";


            SPWrite(Utils.HexToByte(head + msg + end));
            if (autoreceive)
                return true;
            string result = spRead();

            if (!result.StartsWith("68130000"))
            {
                if (trytimes > 0)
                    return _SendSetConfigInfo(sn, recordtype, alerttype, upTemp, downTemp, alertDelay, alertDelay, dtNow, isFastRecord, --trytimes);
                else
                    throw new Exception("retry n times all faild .");
            }
            return true;
        }
        public bool SendSetConfigTime(DateTime dtNow, DateTime correctTime, double TempNow)
        {
            return _SendSetConfigTime(dtNow, correctTime, TempNow, 3);
        }
        private bool _SendSetConfigTime(DateTime dtNow, DateTime correctTime, double TempNow, int trytimes)
        {
            string head = "681013";
            StringBuilder msgsb = new StringBuilder();
            msgsb.Append(Utils.IntToHexString(dtNow.Year % 100, 1));
            msgsb.Append(Utils.IntToHexString(dtNow.Month % 100, 1));
            msgsb.Append(Utils.IntToHexString(dtNow.Day % 100, 1));
            msgsb.Append(Utils.IntToHexString(dtNow.Hour % 100, 1));
            msgsb.Append(Utils.IntToHexString(dtNow.Minute % 100, 1));
            msgsb.Append(Utils.IntToHexString(dtNow.Second % 100, 1));
            msgsb.Append(Utils.IntToHexString(correctTime.Year % 100, 1));
            msgsb.Append(Utils.IntToHexString(correctTime.Month % 100, 1));
            msgsb.Append(Utils.IntToHexString(correctTime.Day % 100, 1));
            msgsb.Append(Utils.IntToHexString(correctTime.Hour % 100, 1));
            msgsb.Append(Utils.IntToHexString(correctTime.Minute % 100, 1));
            msgsb.Append(Utils.ToHexString(TempSenHelper.TempDouble2Byte(TempNow)));
            string msg = msgsb.ToString();
            msg = AddFCS(msg);
            string end = "0A";


            SPWrite(Utils.HexToByte(head+msg+end));
            if (autoreceive)
                return true;
            string result = spRead();

            if (!result.StartsWith("68110000"))
            {
                if (trytimes > 0)
                    return _SendSetConfigTime(dtNow, correctTime, TempNow, --trytimes);
                else
                    throw new Exception("retry n times all faild .");
            }
            return true;
        }
        public int ItemCount = 0;
        public int DataEndAddr = 0;

        public string SentGetSetting()
        {
            try
            {
                return _SentGetSetting(3);
            }
            catch
            {
                ReSet();
                try
                {
                    return _SentGetSetting(3);
                }
                catch
                {
                    throw new Exception("read error ,maybe you usb device is broken .");
                }
            }

        }


        private string _SentGetSetting(int trytimes)
        {
            SPWrite(Utils.HexToByte("681400000A"));
            if (autoreceive)
                return "";
            string result = spRead();
            if (!CheckFCS(result) || !result.StartsWith("6815"))
            {
                if (trytimes > 0)
                    return _SendGetInfo(--trytimes);
                else
                    throw new Exception("retry n times all faild .");
            }

                return result;
        }
        public string SentGetRecords()
        {
            return SentGetRecords(3);
        }
        public string SentGetRecords(int trytimes)
        {
            Dictionary<string, string> rst = new Dictionary<string, string>();
            int start = 0;
            string startAddr = Utils.IntToHexString(start, 2);
            string length = "20";
            try
            {
                while (start < DataEndAddr)
                {
                    string msg = "0000" + startAddr + length;
                    msg = AddFCS(msg);
                    string tresult = GetOneRecord(msg);
                    rst.Add(startAddr, tresult);
                    ////next
                    start += 32;
                    startAddr = Utils.IntToHexString(start, 2);
                }
            }
            catch (Exception ex){
                if (trytimes > 0)
                {
                    Console.WriteLine("ReSet and  retry all"+ex.Message+ex.StackTrace);
                    ReSet();
                    SentGetRecords(--trytimes);
                }
            }
            string result = "";
            foreach (string k in rst.Keys)
                result += rst[k].Substring(6, 64); ;
            return result;
        }

        private string GetOneRecord(string msg)
        { return GetOneRecord(msg, 3); }
        private string GetOneRecord(string msg,int trytime)
        {
            //spRead();
            SPWrite(Utils.HexToByte("682405" + msg + "0A"));
            string tresult = spRead();
            if(CheckonResult(tresult))
                return tresult;
            else if (trytime > 0)
            {
                Console.WriteLine("onrecord retry ");
                return GetOneRecord(msg, --trytime);
            }
            else
                throw new Exception("Get record faild ,usb error .");
        }


        
        private bool CheckonResult(string result)
        {
            if (!result.EndsWith("0A"))
                return false;
            if (!result.StartsWith("682520"))
                return false;
            if (result.Length != 74)
                return false;
            if (!CheckFCS(result))
                return false;
            
            return true;
        }
        public bool CheckFCS(string result)
        {
            byte[] bytes = Utils.HexToByte(result);
            byte b = 0;
            for (int i = 3; i < bytes.Length - 2; i++)
                b = (byte)(b ^ bytes[i]);

            if (b != bytes[bytes.Length - 2])
                return false;
            return true;
        
        }


        private string AddFCS(string str)
        {
            byte[] bytes = Utils.HexToByte(str);
            byte b = 0;
            foreach(byte bt in bytes)
                b=(byte)(b^bt);
        
            return str+b.ToString("X2");
        }

        #endregion


        
    }

}
