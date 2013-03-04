using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TempSen;
using TempSenLib;
using System.Globalization;

namespace TempsenLibHid.PDF
{
    [Serializable]
    public class DeviceData 
    {
        public DeviceData(DevicePDF pdf)
        {
            dev = pdf;
        }
        public DevicePDF dev { get; set; }

        //0,1
        /// <summary>
        /// 设备型号(Model)  hex string .
        /// </summary>
        public string Model { get { return dev.GetValue(0, 0).SubStringEx(0, 4); } }
        /// <summary>
        /// 串号（Serial No.）	10		10byte，可以是字母（大写A-Z）或数字（0-9）
        /// </summary>
        public string SerialNo { get { return Utils.HexToValueString(dev.GetValue(0,0).SubStringEx(4, 20)); } }
        /// <summary>
        /// 运输号 Trip Number 运输号,可以是字母（大写A-Z）或数字（0-9）
        /// </summary>
        public string TripNumber
        {
            get { return Utils.HexToValueString(dev.GetValue(0, 0).SubStringEx(44, 20)); }
            set {
                string fvalue = Utils.FillString(value, 10, ' ');
                dev.SetValue(0, 0, 22, 10, Utils.ValueStringToHex(fvalue));
            }
        }
        //0,2
        /// <summary>
        /// 记录间隔(Log Interval)	2	s	以秒为单位,最大18小时
        /// </summary>
        public int LogInterval { get { return ListHelper.IntPause(""+dev.GetValue(0, 1).SubStringEx(0, 4),System.Globalization.NumberStyles.HexNumber); }
            set { dev.SetValue(0, 1, 0, 2, value.ToString("X4")); }
        }
        /// <summary>
        /// 启动方式（Start Mode）	1		0xF8 = 按键启动； 0x8F = 定时启动； 0x67 = 到温启动
        /// </summary>
        public string StartMode { get { return dev.GetValue(0, 1).SubStringEx(10, 2); }
            set { dev.SetValue(0, 1, 5, 1, value); }
        }
        /// <summary>
        /// 启动延时时间 (Start Delay)	2	m	以分钟为单位，最大45天
        /// </summary>
        public int StartDelay { get { return ListHelper.IntPause(dev.GetValue(0, 1).SubStringEx(12, 4),NumberStyles.HexNumber) ; }
            set { dev.SetValue(0, 1, 6, 2, value.ToString("X4")); }
        }
        
        /// <summary>
        /// 报警阀值 (Alarm Limits)*	10		5段报警阀值（每个阈值2字节）
        /// </summary>
        string AlarmLimits
        {
            get { return dev.GetValue(0, 1).SubStringEx(32, 20); }
            set { dev.SetValue(0, 1, 16, 10, value); }
        }
        #region AlarmLimits
        /// <summary>
        /// 报警阀值 (Alarm Limits)*	10		5段报警阀值（每个阈值2字节）
        /// 0xC0   上限单次 0x80   下限单次 0x81   下限累计  0xC1   上限累计  0x00   失效
        /// </summary>
        public string AlarmLimits0
        {
            get { return HexToTempValue(AlarmLimits.SubStringEx(0,4)); }
            set {
                string v = TempValueToHex(value);
                string all=AlarmLimits;
                AlarmLimits = v + all.Substring(4);
            }
        }
        /// <summary>
        /// 报警阀值 (Alarm Limits)*	10		5段报警阀值（每个阈值2字节）
        /// 0xC0   上限单次 0x80   下限单次 0x81   下限累计  0xC1   上限累计  0x00   失效
        /// </summary>
        public string AlarmLimits1
        {
            get { return HexToTempValue(AlarmLimits.SubStringEx(4, 4)); }
            set
            {
                string v = TempValueToHex(value);
                string all = AlarmLimits;
                AlarmLimits = all.Substring(0, 4) + v + all.Substring(8);
            }
        }
        
        #endregion
        /// <summary>
        /// 报警方式 (Alarm Type)*	5		5段报警方式，第1位代表本报警使能，第2位代表这个上限报报警，否则是下限报警。第8位代表累计报警(Cumulative Alarm)
        /// 或是单次报警(Single Event)（每个方式1字节）	"0b11000001
        /// value&0x80>0  则，开启了报警  否则未开启
        ///value&0x40>0 则, 开启了上限报警  否则未开启
        ///value&0x01>0  则，开启了累计报警   否则，单次报警
        /// </summary>
        string AlarmType { get { return dev.GetValue(0, 1).SubStringEx(52, 10); }
            //set { dev.SetValue(0, 1, 26, 5, value); }
        }

        #region AlarmType
        /// <summary>
        /// 报警方式 (Alarm Type)*	5		5段报警方式，第1位代表本报警使能，第2位代表这个上限报报警，否则是下限报警。第8位代表累计报警(Cumulative Alarm)
        /// 或是单次报警(Single Event)（每个方式1字节）	"0b11000001
        /// value&0x80>0  则，开启了报警  否则未开启
        ///value&0x40>0 则, 开启了上限报警  否则未开启
        ///value&0x01>0  则，开启了累计报警   否则，单次报警
        /// </summary>
        public byte AlarmType0
        {
            get { return byte.Parse(dev.GetValue(0, 1).SubStringEx(52, 2),NumberStyles.HexNumber); }
            set { dev.SetValue(0, 1, 26, 1, value.ToString("X2")); }
        }
        /// <summary>
        /// 报警方式 (Alarm Type)*	5		5段报警方式，第1位代表本报警使能，第2位代表这个上限报报警，否则是下限报警。第8位代表累计报警(Cumulative Alarm)
        /// 或是单次报警(Single Event)（每个方式1字节）	"0b11000001
        /// value&0x80>0  则，开启了报警  否则未开启
        ///value&0x40>0 则, 开启了上限报警  否则未开启
        ///value&0x01>0  则，开启了累计报警   否则，单次报警
        /// </summary>
        public byte AlarmType1
        {
            get { return byte.Parse(dev.GetValue(0, 1).SubStringEx(54, 2), NumberStyles.HexNumber); }
            set { dev.SetValue(0, 1, 27, 1, value.ToString("X2")); }
        }
        
        #endregion
        //0,4
        /// <summary>
        /// 15	报警延时 (Alarm Delay)*	15		以秒为单位，5段报警延时时间（每个延时3字节）	
        /// </summary>
        string AlarmDelay { get { return dev.GetValue(0, 2).SubStringEx(0, 30); }
            //set { dev.SetValue(0, 2, 0, 15, value); }
        }
        #region AlarmDelay
        /// <summary>
        /// 15	报警延时 (Alarm Delay)*	15		以秒为单位，5段报警延时时间（每个延时3字节）	
        /// </summary>
        public int AlarmDelay0
        {
            get { return ListHelper.IntPause(dev.GetValue(0, 2).SubStringEx(0, 6),NumberStyles.HexNumber); }
            set { dev.SetValue(0, 2, 0, 3, value.ToString("X6")); }
        }
        /// <summary>
        /// 15	报警延时 (Alarm Delay)*	15		以秒为单位，5段报警延时时间（每个延时3字节）	
        /// </summary>
        public int AlarmDelay1
        {
            get { return ListHelper.IntPause(dev.GetValue(0, 2).SubStringEx(6, 6), NumberStyles.HexNumber); }
            set { dev.SetValue(0, 2, 3, 3, value.ToString("X6")); }
        }
#endregion

        /// <summary>
        /// 报警模式 (Alarm Mode)0为不报警	1		1为单次报警(Single Alarm)，2为多次报警( Multi Alarm) 
        /// </summary>
        public int AlarmMode
        {
            get { return ListHelper.IntPause(dev.GetValue(0, 2).SubStringEx(32, 2), NumberStyles.HexNumber); }
            set { dev.SetValue(0, 2, 16, 1, value.ToString("X2")); }
        }
        /// <summary>
        /// 18	设置时间 (Write Time)	6		年/月/日/时/分/秒
        /// 该时间输入输出都为utc时间！！切切
        /// </summary>
        public DateTime WriteTime { get {
            return HexToDatetime(dev.GetValue(0, 2).SubStringEx(52, 12));
        }
            set {
                
                dev.SetValue(0, 2, 26, 6, DateTimeToHex(value));
            }
        }
        
        /// <summary>
        /// 20	温度格式（Temperature Unit）	1		"0: C
        ///                                              1: F"
        /// </summary>
        public int TemperatureUnit
        {
            get { return ListHelper.IntPause(dev.GetValue(0, 3).SubStringEx(6, 2), NumberStyles.HexNumber); }
            set { dev.SetValue(0, 3, 3, 1, value.ToString("X2")); }
        }
        
        /// <summary>
        /// 固件版本	1		1-256
        /// </summary>
        public int FirmwareVersion
        {
            get { return ListHelper.IntPause(dev.GetValue(0, 3).SubStringEx(18, 2),NumberStyles.HexNumber); }
            set { dev.SetValue(0, 3, 9, 1, value.ToString("X2")); }
        }
        
        /// <summary>
        /// 运行状态(RunStatus)	1		0:未设定，1：已设定， 2：记录中。3：已停止
        /// </summary>
        public int RunStatus { get { return ListHelper.IntPause(dev.GetValue(2, 0).SubStringEx(2, 2),NumberStyles.HexNumber); } }
        
        /// <summary>
        /// 记录点数Data Points	3		
        /// </summary>
        public int DataPoints { get {
            int value= ListHelper.IntPause("" + dev.GetValue(2, 0).SubStringEx(6, 6), System.Globalization.NumberStyles.HexNumber);
            return value > 64000 ? 0 : value;
        }
        }
        /// <summary>
        /// 同DataPoints 为了兼容其他代码
        /// </summary>
        public int LogCount { get {
            int value= ListHelper.IntPause("" + dev.GetValue(2, 0).SubStringEx(6, 6), System.Globalization.NumberStyles.HexNumber);
            return value > 64000 ? 0 : value;
        }
        }
        
        /// <summary>
        /// 报警标志	1		0:未报警，1：已报警
        /// </summary>
        public int AlarmStatus { get { return ListHelper.IntPause(dev.GetValue(2, 0).SubStringEx(4, 2)); } }
        /// <summary>
        /// 开始时间(第一个记录点)	6		
        /// </summary>
        public DateTime StartTime
        {
            get
            {
                return HexToDatetime(dev.GetValue(2, 0).SubStringEx(12, 12));
            }
        }
        /// <summary>
        /// 结束时间	6		
        /// </summary>
        public DateTime StopTime
        {
            get
            {
                return  HexToDatetime(dev.GetValue(2, 0).SubStringEx(32, 12));
            }
        }
        
        /// <summary>
        /// 最大温度点及其时间MaxTemperature
        /// </summary>
        public string MaxTemperature { get { return HexToTempValue(dev.GetValue(2, 1).SubStringEx(0, 4)); } }
        /// <summary>
        /// 最大温度点及其时间MaxTimeStamp
        /// </summary>
        public DateTime MaxTimeStamp { get { return HexToDatetime(dev.GetValue(2, 1).SubStringEx(4, 12)); } }
        /// <summary>
        /// 最小温度点及其时间
        /// </summary>
        public string MinTemperature { get { return HexToTempValue(dev.GetValue(2, 1).SubStringEx(32, 4)); } }
        /// <summary>
        /// 最小温度点及其时间
        /// </summary>
        public DateTime MinTimeStamp { get { return HexToDatetime(dev.GetValue(2, 1).SubStringEx(36, 12)); } }

        //2,4-5
        /// <summary>
        /// 平均温度
        /// </summary>
        public string AverageTemperature { get { return HexToTempValue(dev.GetValue(2, 2).SubStringEx(0, 4)); } }
        /// <summary>
        /// MKT温度值 （83.14472）
        /// </summary>
        public string MKT { get { return HexToTempValue(dev.GetValue(2, 2).SubStringEx(4, 4)); } }


        /// <summary>
        /// 各区域超温时间统计
        /// 时间以秒为单位
        /// </summary>
        string Totaltimeofviolations { get { return dev.GetValue(2, 2).SubStringEx(32, 20); } }
        /// <summary>
        /// 各区域超温时间统计 
        /// 时间以秒为单位
        /// </summary>
        public int[] Totaltimeofviolation{
            get {
                int[] rst=new int[2];
                string value= dev.GetValue(2, 2).SubStringEx(32, 20);
                rst[0] = ListHelper.IntPause(value.SubStringEx(0, 4), NumberStyles.HexNumber) *LogInterval;
                rst[1] = ListHelper.IntPause(value.SubStringEx(4, 4), NumberStyles.HexNumber) * LogInterval;
                //rst[2] = ListHelper.IntPause(value.SubStringEx(8, 4), NumberStyles.HexNumber) * LogInterval;
                //rst[3] = ListHelper.IntPause(value.SubStringEx(12, 4), NumberStyles.HexNumber) * LogInterval;
                //rst[4] = ListHelper.IntPause(value.SubStringEx(16, 4), NumberStyles.HexNumber) * LogInterval;
                return rst;
            }
        }


        //2,6-9
        /// <summary>
        /// 各区域超温次数统计	10		
        /// </summary>
        string Totalnumberofviolations { get { return dev.GetValue(2, 3).SubStringEx(0, 20); } }
        /// <summary>
        /// 各区域超温次数统计	10		
        /// </summary>
        public int[] Totalnumberofviolation
        {
            get
            {
                int[] rst = new int[2];
                string value = dev.GetValue(2, 3).SubStringEx(0, 20);
                rst[0] = ListHelper.IntPause(value.SubStringEx(0, 4), NumberStyles.HexNumber) ;
                rst[1] = ListHelper.IntPause(value.SubStringEx(4, 4), NumberStyles.HexNumber) ;
                //rst[2] = ListHelper.IntPause(value.SubStringEx(8, 4), NumberStyles.HexNumber) ;
                //rst[3] = ListHelper.IntPause(value.SubStringEx(12, 4), NumberStyles.HexNumber) ;
                //rst[4] = ListHelper.IntPause(value.SubStringEx(16, 4), NumberStyles.HexNumber) ;
                return rst;
            }
        }
        /// <summary>
        /// 各区域报警状态	5		0代表没有报警，1代表有报警
        /// </summary>
        string AlarmStatusforeachzones { get { return dev.GetValue(2, 3).SubStringEx(20, 10); } }

        /// <summary>
        /// 各区域报警状态	5		0代表没有报警，1代表有报警
        /// </summary>
        public int[] AlarmStatusforeachzone
        {
            get
            {
                int[] rst = new int[2];
                string value = dev.GetValue(2, 3).SubStringEx(20, 10);
                rst[0] = ListHelper.IntPause(value.SubStringEx(0, 2), NumberStyles.HexNumber) == 0 ? 0 : 1;
                rst[1] = ListHelper.IntPause(value.SubStringEx(2, 2), NumberStyles.HexNumber) == 0 ? 0 : 1;
                //rst[2] = ListHelper.IntPause(value.SubStringEx(4, 2), NumberStyles.HexNumber) == 0 ? 0 : 1;
                //rst[3] = ListHelper.IntPause(value.SubStringEx(6, 2), NumberStyles.HexNumber) == 0 ? 0 : 1;
                //rst[4] = ListHelper.IntPause(value.SubStringEx(8, 2), NumberStyles.HexNumber) == 0 ? 0 : 1;
                return rst;
            }
        }

        /// <summary>
        /// DataPointsList
        /// </summary>
        public List<string> DataPointsList { get {
            List<string> result = new List<string>();
            if (RunStatus == 0)
                return result;
            int count = DataPoints;
            string tempvalue="";
            for(int i=0;i<count;i++)
            {
                tempvalue=GetTempValue(i);
                result.Add(tempvalue);
            }
            return result;
        } }
        /// <summary>
        /// DataPointsList
        /// </summary>
        public List<string> DataPointsListWithTime
        {
            get
            {
            List<string> result = new List<string>();
            if (RunStatus == 0)
                return result;
            int count = DataPoints;
            DateTime start=StartTime;
            string tempvalue = "";
            for (int i = 0; i < count; i++)
            {
                tempvalue = GetTempValue(i);
                result.Add(start.ToString("yyyy-MM-dd HH:mm:ss")+","+tempvalue);
                start= start.AddSeconds(LogInterval);
            }
            return result;
        } }
        string GetTempValue(int index)
        {
            string rst = "";
            int page = index / 128+10;
            int row = index % 128 / 16;
            int pos = index % 16;
            string rststr = dev.GetValue(page, row).SubStringEx(pos * 4, 4);
            rst = HexToTempValue(rststr);
            return rst;
        }

        static string HexToTempValue(string str2ByteHex)
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
        private static DateTime HexToDatetime(string datetimehex)
        {
            byte[] timeb = Utils.HexToByte(datetimehex);
            try
            {
                if (timeb[1] > 12)
                {
                    return DateTime.MinValue;
                }
                DateTime start = new DateTime(timeb[0] + 2000, timeb[1], timeb[2], timeb[3], timeb[4], timeb[5]);
                return start;
            }
            catch {
                return DateTime.MinValue;
            }
        }
        private static string DateTimeToHex(DateTime value)
        {
            byte[] timeb = new byte[6] { (byte)(value.Year % 100), (byte)value.Month, (byte)value.Day, (byte)value.Hour, (byte)value.Minute, (byte)value.Second };
            return Utils.ToHexString(timeb); 
        
        }
    }
}
