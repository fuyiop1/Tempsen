using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TempSen;
using TempSenLib;
using System.Globalization;

namespace TempsenLibHid.PDF
{
    public class a
    {
        [Serializable]
        private class DDAll
        {
            public DDAll(DevicePDF pdf)
            {
                dev = pdf;
            }
            public DevicePDF dev { get; set; }

            //0,1
            /// <summary>
            /// 设备型号(Model)  hex string .
            /// </summary>
            public string DevModel { get { return dev.GetValue(0, 0).SubStringEx(0, 4); } }
            /// <summary>
            /// 串号（Serial No.）	10		10byte，可以是字母（大写A-Z）或数字（0-9）
            /// </summary>
            public string DevNo { get { return Utils.HexToValueString(dev.GetValue(0, 0).SubStringEx(4, 20)); } }
            /// <summary>
            /// 硬件地址	4		MAC地址	192.168.105.188
            /// </summary>
            public string HardWareAddr
            {
                get
                {
                    byte[] byte4 = Utils.HexToByte((dev.GetValue(0, 0).SubStringEx(24, 8)));
                    StringBuilder sb = new StringBuilder();
                    foreach (byte b in byte4)
                        sb.Append("." + b.ToString());
                    return sb.ToString().SubStringEx(1);
                }
            }
            /// <summary>
            /// 校准值(Calibration)	6		三段校准,第一位是个位，第二位是小数位	0.1;1.0;2.0
            /// 返回三组值
            /// </summary>
            public string[] Calcultion
            {
                get
                {
                    string[] rst = new string[3];
                    string value = dev.GetValue(0, 0).SubStringEx(32, 12);

                    rst[0] = HexToTempValue(value.SubStringEx(0, 4));
                    rst[1] = HexToTempValue(value.SubStringEx(4, 4));
                    rst[2] = HexToTempValue(value.SubStringEx(8, 4));
                    return rst;
                }
            }
            /// <summary>
            /// 运输号,可以是字母（大写A-Z）或数字（0-9）
            /// </summary>
            public string TripNo
            {
                get { return Utils.HexToValueString(dev.GetValue(0, 0).SubStringEx(44, 20)); }
                set
                {
                    string fvalue = Utils.FillString(value, 10, ' ');
                    dev.SetValue(0, 0, 22, 10, Utils.ValueStringToHex(fvalue));
                }
            }
            //0,2
            /// <summary>
            /// 记录间隔(Log Interval)	2	s	以秒为单位,最大18小时
            /// </summary>
            public int LogInterval
            {
                get { return ListHelper.IntPause("" + dev.GetValue(0, 1).SubStringEx(0, 4), System.Globalization.NumberStyles.HexNumber); }
                set { dev.SetValue(0, 1, 0, 2, value.ToString("X4")); }
            }
            /// <summary>
            /// 记录周期(Log Cycle)	2	h	以小时为单位,最大2730天
            /// </summary>
            public int LogCycle
            {
                get { return ListHelper.IntPause("" + dev.GetValue(0, 1).SubStringEx(4, 4), System.Globalization.NumberStyles.HexNumber); }
                set { dev.SetValue(0, 1, 2, 2, value.ToString("X4")); }
            }
            /// <summary>
            /// 运行模式（Run Mode）	1		开始/停止；循环  	"0代表开始/停止 1代表循环"
            /// </summary>
            public int RunMode
            {
                get { return ListHelper.IntPause(dev.GetValue(0, 1).SubStringEx(8, 2), System.Globalization.NumberStyles.HexNumber); }
                set { dev.SetValue(0, 1, 4, 1, value.ToString("X2")); }
            }

            /// <summary>
            /// 启动方式（Start Mode）	1		0xF8 = 按键启动； 0x8F = 定时启动； 0x67 = 到温启动
            /// </summary>
            public string StartMode
            {
                get { return dev.GetValue(0, 1).SubStringEx(10, 2); }
                set { dev.SetValue(0, 1, 5, 1, value); }
            }
            /// <summary>
            /// 启动延时时间 (Start Delay)	2	m	以分钟为单位，最大45天
            /// </summary>
            public int StartDelay
            {
                get { return ListHelper.IntPause(dev.GetValue(0, 1).SubStringEx(12, 4), NumberStyles.HexNumber); }
                set { dev.SetValue(0, 1, 6, 2, value.ToString("X4")); }
            }
            /// <summary>
            /// 到温启动条件	2		暂无更多说明
            /// </summary>
            public string StartConditionTemp
            {
                get { return HexToTempValue(dev.GetValue(0, 1).SubStringEx(16, 4)); }
                set { dev.SetValue(0, 1, 8, 2, TempValueToHex(value)); }
            }
            /// <summary>
            /// 定时启动时间	6		定时时间，一定要比设定时间晚。年/月/日/时/分/秒
            /// </summary>
            public DateTime StartConditionTime
            {
                get
                {
                    return HexToDatetime(dev.GetValue(0, 1).SubStringEx(20, 12));
                }
                set
                {

                    dev.SetValue(0, 1, 10, 6, DateTimeToHex(value));
                }
            }
            //0,3
            /// <summary>
            /// 报警阀值 (Alarm Limits)*	10		5段报警阀值（每个阈值2字节）
            /// </summary>
            public string AlarmLimits
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
                get { return HexToTempValue(AlarmLimits.SubStringEx(0, 4)); }
                set
                {
                    string v = TempValueToHex(value);
                    string all = AlarmLimits;
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
            /// <summary>
            /// 报警阀值 (Alarm Limits)*	10		5段报警阀值（每个阈值2字节）
            /// 0xC0   上限单次 0x80   下限单次 0x81   下限累计  0xC1   上限累计  0x00   失效
            /// </summary>
            public string AlarmLimits2
            {
                get { return HexToTempValue(AlarmLimits.SubStringEx(8, 4)); }
                set
                {
                    string v = TempValueToHex(value);
                    string all = AlarmLimits;
                    AlarmLimits = all.Substring(0, 8) + v + all.Substring(12);
                }
            }
            /// <summary>
            /// 报警阀值 (Alarm Limits)*	10		5段报警阀值（每个阈值2字节）
            /// 0xC0   上限单次 0x80   下限单次 0x81   下限累计  0xC1   上限累计  0x00   失效
            /// </summary>
            public string AlarmLimits3
            {
                get { return HexToTempValue(AlarmLimits.SubStringEx(12, 4)); }
                set
                {
                    string v = TempValueToHex(value);
                    string all = AlarmLimits;
                    AlarmLimits = all.Substring(0, 12) + v + all.Substring(16);
                }
            }
            /// <summary>
            /// 报警阀值 (Alarm Limits)*	10		5段报警阀值（每个阈值2字节）
            /// 0xC0   上限单次 0x80   下限单次 0x81   下限累计  0xC1   上限累计  0x00   失效
            /// </summary>
            public string AlarmLimits4
            {
                get { return HexToTempValue(AlarmLimits.SubStringEx(16, 4)); }
                set
                {
                    string v = TempValueToHex(value);
                    string all = AlarmLimits;
                    AlarmLimits = all.Substring(0, 16) + v;
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
            public string AlarmType
            {
                get { return dev.GetValue(0, 1).SubStringEx(52, 10); }
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
                get { return byte.Parse(dev.GetValue(0, 1).SubStringEx(52, 2), NumberStyles.HexNumber); }
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
            /// <summary>
            /// 报警方式 (Alarm Type)*	5		5段报警方式，第1位代表本报警使能，第2位代表这个上限报报警，否则是下限报警。第8位代表累计报警(Cumulative Alarm)
            /// 或是单次报警(Single Event)（每个方式1字节）	"0b11000001
            /// value&0x80>0  则，开启了报警  否则未开启
            ///value&0x40>0 则, 开启了上限报警  否则未开启
            ///value&0x01>0  则，开启了累计报警   否则，单次报警
            /// </summary>
            public byte AlarmType2
            {
                get { return byte.Parse(dev.GetValue(0, 1).SubStringEx(56, 2), NumberStyles.HexNumber); }
                set { dev.SetValue(0, 1, 28, 1, value.ToString("X2")); }
            }
            /// <summary>
            /// 报警方式 (Alarm Type)*	5		5段报警方式，第1位代表本报警使能，第2位代表这个上限报报警，否则是下限报警。第8位代表累计报警(Cumulative Alarm)
            /// 或是单次报警(Single Event)（每个方式1字节）	"0b11000001
            /// value&0x80>0  则，开启了报警  否则未开启
            ///value&0x40>0 则, 开启了上限报警  否则未开启
            ///value&0x01>0  则，开启了累计报警   否则，单次报警
            /// </summary>
            public byte AlarmType3
            {
                get { return byte.Parse(dev.GetValue(0, 1).SubStringEx(58, 2), NumberStyles.HexNumber); }
                set { dev.SetValue(0, 1, 29, 1, value.ToString("X2")); }
            }
            /// <summary>
            /// 报警方式 (Alarm Type)*	5		5段报警方式，第1位代表本报警使能，第2位代表这个上限报报警，否则是下限报警。第8位代表累计报警(Cumulative Alarm)
            /// 或是单次报警(Single Event)（每个方式1字节）	"0b11000001
            /// value&0x80>0  则，开启了报警  否则未开启
            ///value&0x40>0 则, 开启了上限报警  否则未开启
            ///value&0x01>0  则，开启了累计报警   否则，单次报警
            /// </summary>
            public byte AlarmType4
            {
                get { return byte.Parse(dev.GetValue(0, 1).SubStringEx(60, 2), NumberStyles.HexNumber); }
                set { dev.SetValue(0, 1, 30, 1, value.ToString("X2")); }
            }
            #endregion
            //0,4
            /// <summary>
            /// 15	报警延时 (Alarm Delay)*	15		以秒为单位，5段报警延时时间（每个延时3字节）	
            /// </summary>
            public string AlarmDelay
            {
                get { return dev.GetValue(0, 2).SubStringEx(0, 30); }
                //set { dev.SetValue(0, 2, 0, 15, value); }
            }
            #region AlarmDelay
            /// <summary>
            /// 15	报警延时 (Alarm Delay)*	15		以秒为单位，5段报警延时时间（每个延时3字节）	
            /// </summary>
            public int AlarmDelay0
            {
                get { return ListHelper.IntPause(dev.GetValue(0, 2).SubStringEx(0, 6), NumberStyles.HexNumber); }
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
            /// <summary>
            /// 15	报警延时 (Alarm Delay)*	15		以秒为单位，5段报警延时时间（每个延时3字节）	
            /// </summary>
            public int AlarmDelay2
            {
                get { return ListHelper.IntPause(dev.GetValue(0, 2).SubStringEx(12, 6), NumberStyles.HexNumber); }
                set { dev.SetValue(0, 2, 6, 3, value.ToString("X6")); }
            }
            /// <summary>
            /// 15	报警延时 (Alarm Delay)*	15		以秒为单位，5段报警延时时间（每个延时3字节）	
            /// </summary>
            public int AlarmDelay3
            {
                get { return ListHelper.IntPause(dev.GetValue(0, 2).SubStringEx(18, 6), NumberStyles.HexNumber); }
                set { dev.SetValue(0, 2, 9, 3, value.ToString("X6")); }
            }
            /// <summary>
            /// 15	报警延时 (Alarm Delay)*	15		以秒为单位，5段报警延时时间（每个延时3字节）	
            /// </summary>
            public int AlarmDelay4
            {
                get { return ListHelper.IntPause(dev.GetValue(0, 2).SubStringEx(24, 6), NumberStyles.HexNumber); }
                set { dev.SetValue(0, 2, 12, 3, value.ToString("X6")); }
            }
            #endregion

            //0,5
            /// <summary>
            /// 16	各区域超温次数限制	5		超温次数，最大250次	
            /// </summary>
            public string AlarmTimeLimit
            {
                get { return dev.GetValue(0, 2).SubStringEx(42, 10); }
                //set { dev.SetValue(0, 2, 21, 5, value); }
            }
            #region AlarmTimeLimit
            /// <summary>
            /// 16	各区域超温次数限制	5		超温次数，最大250次	
            /// </summary>
            public int AlarmTimeLimit0
            {
                get { return ListHelper.IntPause(dev.GetValue(0, 2).SubStringEx(42, 2), NumberStyles.HexNumber); }
                set { dev.SetValue(0, 2, 21, 1, value.ToString("X2")); }
            }
            /// <summary>
            /// 16	各区域超温次数限制	5		超温次数，最大250次	
            /// </summary>
            public int AlarmTimeLimit1
            {
                get { return ListHelper.IntPause(dev.GetValue(0, 2).SubStringEx(44, 2), NumberStyles.HexNumber); }
                set { dev.SetValue(0, 2, 22, 1, value.ToString("X2")); }
            }
            /// <summary>
            /// 16	各区域超温次数限制	5		超温次数，最大250次	
            /// </summary>
            public int AlarmTimeLimit2
            {
                get { return ListHelper.IntPause(dev.GetValue(0, 2).SubStringEx(46, 2), NumberStyles.HexNumber); }
                set { dev.SetValue(0, 2, 23, 1, value.ToString("X2")); }
            }
            /// <summary>
            /// 16	各区域超温次数限制	5		超温次数，最大250次	
            /// </summary>
            public int AlarmTimeLimit3
            {
                get { return ListHelper.IntPause(dev.GetValue(0, 2).SubStringEx(48, 2), NumberStyles.HexNumber); }
                set { dev.SetValue(0, 2, 24, 1, value.ToString("X2")); }
            }
            /// <summary>
            /// 16	各区域超温次数限制	5		超温次数，最大250次	
            /// </summary>
            public int AlarmTimeLimit4
            {
                get { return ListHelper.IntPause(dev.GetValue(0, 2).SubStringEx(50, 2), NumberStyles.HexNumber); }
                set { dev.SetValue(0, 2, 25, 1, value.ToString("X2")); }
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
            public DateTime WriteTime
            {
                get
                {
                    return HexToDatetime(dev.GetValue(0, 2).SubStringEx(52, 12));
                }
                set
                {

                    dev.SetValue(0, 2, 26, 6, DateTimeToHex(value));
                }
            }
            //0,6
            /// <summary>
            /// 19	时区(Time Zone)	1		设置地跟GMT的时时间，如中国（+80）
            /// </summary>
            public byte TimeZone
            {
                get { return byte.Parse(dev.GetValue(0, 3).SubStringEx(0, 2), NumberStyles.HexNumber); }
                set { dev.SetValue(0, 3, 0, 1, value.ToString("X2")); }
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
            //public string MKT { get { return dev.GetValue(0, 3).SubStringEx(8, 8); }
            //    set { dev.SetValue(0, 3, 4, 4, value); }
            //}
            /// <summary>
            /// 协议版本	1		1-256
            /// </summary>
            public int ProtocolVersion
            {
                get { return ListHelper.IntPause(dev.GetValue(0, 3).SubStringEx(16, 2), NumberStyles.HexNumber); }
                set { dev.SetValue(0, 3, 8, 1, value.ToString("X2")); }
            }
            /// <summary>
            /// 固件版本	1		1-256
            /// </summary>
            public int FirmwareVersion
            {
                get { return ListHelper.IntPause(dev.GetValue(0, 3).SubStringEx(18, 2), NumberStyles.HexNumber); }
                set { dev.SetValue(0, 3, 9, 1, value.ToString("X2")); }
            }
            //0,7
            public string PackageNumber
            {
                get { return Utils.HexToValueString(dev.GetValue(0, 3).SubStringEx(32, 16)); }
                set { dev.SetValue(0, 3, 16, 8, Utils.ValueStringToHex(Utils.FillString(value, 8, ' '))); }
            }
            public string PalletNumber
            {
                get { return Utils.HexToValueString(dev.GetValue(0, 3).SubStringEx(48, 16)); }
                set { dev.SetValue(0, 3, 24, 8, Utils.ValueStringToHex(Utils.FillString(value, 8, ' '))); }
            }
            //0,8
            public string MonitorLocation
            {
                get { return Utils.HexToValueString(dev.GetValue(0, 4).SubStringEx(0, 16)); }
                set { dev.SetValue(0, 4, 0, 8, Utils.ValueStringToHex(Utils.FillString(value, 8, ' '))); }
            }
            /// <summary>
            /// CRC	2		全部数据的CRC，保证设置的东西是正确的。
            /// </summary>
            public string CRC
            {
                get { return dev.GetValue(0, 4).SubStringEx(28, 4); }
                set { dev.SetValue(0, 4, 14, 2, value); }
            }
            //0,9
            //0,10,11,12,13,14,15
            /// <summary>
            /// 27	任务摘要(Description)	128		
            /// </summary>
            public string Description
            {
                get { return Utils.HexToValueString(dev.GetValue(0, 6) + dev.GetValue(0, 7) + dev.GetValue(1, 0) + dev.GetValue(1, 1)).Trim(); }
                set
                {
                    string fvalue = Utils.FillString(value, 128, ' ');


                    dev.SetValue(0, 6, 0, 32, Utils.ValueStringToHex(fvalue.SubStringEx(0, 32)));
                    dev.SetValue(0, 7, 0, 32, Utils.ValueStringToHex(fvalue.SubStringEx(32, 32)));
                    dev.SetValue(1, 0, 0, 32, Utils.ValueStringToHex(fvalue.SubStringEx(64, 32)));
                    dev.SetValue(1, 1, 0, 32, Utils.ValueStringToHex(fvalue.SubStringEx(96, 32)));
                }
            }
            ////page1
            ///// <summary>
            ///// 
            ///// </summary>
            //public string Operator { get { 
            //    StringBuilder sb=new StringBuilder();
            //    for(int i=0;i<8;i++)
            //        sb.Append(Utils.HexToValueString(dev.GetValue(1,i)));
            //    return sb.ToString().Trim();
            //}
            //    set {
            //        string fvalue = Utils.FillString(value, 32*8, ' ');
            //        for (int i = 0; i < 8; i++)
            //            dev.SetValue(1, i, 0, 32, Utils.ValueStringToHex(fvalue.SubStringEx(i * 32, 32)));
            //    }
            //}


            //zone2
            //2,0-1

            /// <summary>
            /// 电池电量	1		0-100
            /// </summary>
            public int Battery { get { return ListHelper.IntPause(dev.GetValue(2, 0).SubStringEx(0, 2), NumberStyles.HexNumber); } }
            /// <summary>
            /// 运行状态(RunStatus)	1		0:未设定，1：已设定， 2：记录中。3：已停止
            /// </summary>
            public int RunStatus { get { return ListHelper.IntPause(dev.GetValue(2, 0).SubStringEx(2, 2), NumberStyles.HexNumber); } }
            /// <summary>
            /// 报警标志	1		0:未报警，1：已报警
            /// </summary>
            public int AlarmMark { get { return ListHelper.IntPause(dev.GetValue(2, 0).SubStringEx(4, 2)); } }
            /// <summary>
            /// 记录点数	3		
            /// </summary>
            public int LogCount
            {
                get
                {
                    int value = ListHelper.IntPause("" + dev.GetValue(2, 0).SubStringEx(6, 6), System.Globalization.NumberStyles.HexNumber);
                    return value > 64000 ? 0 : value;
                }
            }
            /// <summary>
            /// 开始时间(第一个记录点)	6		
            /// </summary>
            public DateTime LogStartTime
            {
                get
                {
                    return HexToDatetime(dev.GetValue(2, 0).SubStringEx(12, 12));
                }
            }
            /// <summary>
            /// 结束时间	6		
            /// </summary>
            public DateTime LogEndTime
            {
                get
                {
                    return HexToDatetime(dev.GetValue(2, 0).SubStringEx(32, 12));
                }
            }
            /// <summary>
            /// 当前时间	6		
            /// </summary>
            public DateTime LogNowTime
            {
                get
                {
                    return HexToDatetime(dev.GetValue(2, 0).SubStringEx(44, 12));
                }
            }
            //2,2-3
            /// <summary>
            /// 最大温度点及其时间
            /// </summary>
            public string MaxTemp { get { return HexToTempValue(dev.GetValue(2, 1).SubStringEx(0, 4)); } }
            /// <summary>
            /// 最大温度点及其时间
            /// </summary>
            public DateTime MaxTempTime { get { return HexToDatetime(dev.GetValue(2, 1).SubStringEx(4, 12)); } }
            /// <summary>
            /// 最小温度点及其时间
            /// </summary>
            public string MinTemp { get { return HexToTempValue(dev.GetValue(2, 1).SubStringEx(32, 4)); } }
            /// <summary>
            /// 最小温度点及其时间
            /// </summary>
            public DateTime MinTempTime { get { return HexToDatetime(dev.GetValue(2, 1).SubStringEx(36, 12)); } }

            //2,4-5
            /// <summary>
            /// 平均温度
            /// </summary>
            public string AvgTemp { get { return HexToTempValue(dev.GetValue(2, 2).SubStringEx(0, 4)); } }
            /// <summary>
            /// MKT温度值 （83.14472）
            /// </summary>
            public string MKT { get { return HexToTempValue(dev.GetValue(2, 2).SubStringEx(4, 4)); } }



            public string STDEV { get { return HexToTempValue(dev.GetValue(2, 2).SubStringEx(8, 4)); } }

            /// <summary>
            /// 各区域超温时间统计
            /// 时间以秒为单位
            /// </summary>
            public string AllZoneOverTempTimeStat { get { return dev.GetValue(2, 2).SubStringEx(32, 20); } }
            /// <summary>
            /// 各区域超温时间统计 
            /// 时间以秒为单位
            /// </summary>
            public int[] AllZoneOverTempTimeStatArray
            {
                get
                {
                    int[] rst = new int[5];
                    string value = dev.GetValue(2, 2).SubStringEx(32, 20);
                    rst[0] = ListHelper.IntPause(value.SubStringEx(0, 4), NumberStyles.HexNumber) * LogInterval;
                    rst[1] = ListHelper.IntPause(value.SubStringEx(4, 4), NumberStyles.HexNumber) * LogInterval;
                    rst[2] = ListHelper.IntPause(value.SubStringEx(8, 4), NumberStyles.HexNumber) * LogInterval;
                    rst[3] = ListHelper.IntPause(value.SubStringEx(12, 4), NumberStyles.HexNumber) * LogInterval;
                    rst[4] = ListHelper.IntPause(value.SubStringEx(16, 4), NumberStyles.HexNumber) * LogInterval;
                    return rst;
                }
            }


            //2,6-9
            /// <summary>
            /// 各区域超温次数统计	10		
            /// </summary>
            public string AllZoneOverTempTimesStat { get { return dev.GetValue(2, 3).SubStringEx(0, 20); } }
            /// <summary>
            /// 各区域超温次数统计	10		
            /// </summary>
            public int[] AllZoneOverTempTimesStatArray
            {
                get
                {
                    int[] rst = new int[5];
                    string value = dev.GetValue(2, 3).SubStringEx(0, 20);
                    rst[0] = ListHelper.IntPause(value.SubStringEx(0, 4), NumberStyles.HexNumber);
                    rst[1] = ListHelper.IntPause(value.SubStringEx(4, 4), NumberStyles.HexNumber);
                    rst[2] = ListHelper.IntPause(value.SubStringEx(8, 4), NumberStyles.HexNumber);
                    rst[3] = ListHelper.IntPause(value.SubStringEx(12, 4), NumberStyles.HexNumber);
                    rst[4] = ListHelper.IntPause(value.SubStringEx(16, 4), NumberStyles.HexNumber);
                    return rst;
                }
            }
            /// <summary>
            /// 各区域报警状态	5		0代表没有报警，1代表有报警
            /// </summary>
            public string AllZoneAlartStatus { get { return dev.GetValue(2, 3).SubStringEx(20, 10); } }

            /// <summary>
            /// 各区域报警状态	5		0代表没有报警，1代表有报警
            /// </summary>
            public int[] AllZoneAlartStatusArray
            {
                get
                {
                    int[] rst = new int[5];
                    string value = dev.GetValue(2, 3).SubStringEx(20, 10);
                    rst[0] = ListHelper.IntPause(value.SubStringEx(0, 2), NumberStyles.HexNumber) == 0 ? 0 : 1;
                    rst[1] = ListHelper.IntPause(value.SubStringEx(2, 2), NumberStyles.HexNumber) == 0 ? 0 : 1;
                    rst[2] = ListHelper.IntPause(value.SubStringEx(4, 2), NumberStyles.HexNumber) == 0 ? 0 : 1;
                    rst[3] = ListHelper.IntPause(value.SubStringEx(6, 2), NumberStyles.HexNumber) == 0 ? 0 : 1;
                    rst[4] = ListHelper.IntPause(value.SubStringEx(8, 2), NumberStyles.HexNumber) == 0 ? 0 : 1;
                    return rst;
                }
            }


            public string FirstAlarmTime
            {
                get
                {
                    return
                        dev.GetValue(2, 3).SubStringEx(32, 24)
                        + dev.GetValue(2, 4).SubStringEx(0, 24)
                        + dev.GetValue(2, 4).SubStringEx(32, 12);
                }
            }


            public DateTime[] FirstAlarmTimeArray
            {
                get
                {
                    DateTime[] rst = new DateTime[5];
                    string value = dev.GetValue(2, 3).SubStringEx(32, 24)
                        + dev.GetValue(2, 4).SubStringEx(0, 24)
                        + dev.GetValue(2, 4).SubStringEx(32, 12); ;
                    rst[0] = HexToDatetime(value.SubStringEx(0, 12));
                    rst[1] = HexToDatetime(value.SubStringEx(12, 12));
                    rst[2] = HexToDatetime(value.SubStringEx(24, 12));
                    rst[3] = HexToDatetime(value.SubStringEx(36, 12));
                    rst[4] = HexToDatetime(value.SubStringEx(48, 12));
                    return rst;
                }
            }
            /// <summary>
            /// Mark次数
            /// </summary>
            public int MarkTimeCount { get { return ListHelper.IntPause(dev.GetValue(2, 4).SubStringEx(44, 2), NumberStyles.HexNumber); } }

            public string MarkTime { get { return dev.GetValue(2, 5) + dev.GetValue(2, 6) + dev.GetValue(2, 7); } }
            public string[] MarkTimeArray
            {
                get
                {
                    int marktimecount = 12;
                    //int marktimecount = MarkTimeCount;
                    string value = dev.GetValue(2, 5) + dev.GetValue(2, 6) + dev.GetValue(2, 7);
                    string[] values = new string[marktimecount];
                    for (int i = 0; i < marktimecount; i++)
                    {
                        values[i] = HexToTempValue(value.Substring(i * 16, 4)) + "," + HexToDatetime(value.Substring(i * 16 + 4, 12));
                    }
                    return values;
                }
            }

            public List<string> TempList
            {
                get
                {
                    List<string> result = new List<string>();
                    if (RunStatus == 0)
                        return result;
                    int count = LogCount;
                    string tempvalue = "";
                    for (int i = 0; i < count; i++)
                    {
                        tempvalue = GetTempValue(i);
                        result.Add(tempvalue);
                    }
                    return result;
                }
            }
            public List<string> TempListWithTime
            {
                get
                {
                    List<string> result = new List<string>();
                    if (RunStatus == 0)
                        return result;
                    int count = LogCount;
                    DateTime start = LogStartTime;
                    string tempvalue = "";
                    for (int i = 0; i < count; i++)
                    {
                        tempvalue = GetTempValue(i);
                        result.Add(start.ToString("yyyy-MM-dd HH:mm:ss") + "," + tempvalue);
                        start = start.AddSeconds(LogInterval);
                    }
                    return result;
                }
            }
            public string GetTempValue(int index)
            {
                string rst = "";
                int page = index / 128 + 10;
                int row = index % 128 / 16;
                int pos = index % 16;
                string rststr = dev.GetValue(page, row).SubStringEx(pos * 4, 4);
                rst = HexToTempValue(rststr);
                return rst;
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
            private static DateTime HexToDatetime(string datetimehex)
            {
                byte[] timeb = Utils.HexToByte(datetimehex);
                try
                {
                    DateTime start = new DateTime(timeb[0] + 2000, timeb[1], timeb[2], timeb[3], timeb[4], timeb[5]);
                    return start;
                }
                catch
                {
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
}
