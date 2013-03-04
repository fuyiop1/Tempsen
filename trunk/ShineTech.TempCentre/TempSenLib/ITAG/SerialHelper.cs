using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.IO.Ports;
using Services.Common;

namespace TempSenLib
{
    public class SerialHelper
    {
        private static ITracing _tracing = TracingManager.GetTracing(typeof(SerialHelper));
        #region temp list 

        public static string FirstCom = "";
        public static string GetOneCom()
        {

            if (SerialPortTran.IsTheDevice(FirstCom))
                return FirstCom;

            {
                string[] sValues = SerialPort.GetPortNames(); // keyCom.GetValueNames();
                foreach (string sValue in sValues)
                {
                    if(sValue!=FirstCom)
                    {
                    try
                    {
                        bool b = SerialPortTran.IsTheDevice(sValue);
                        if (b)
                        {
                            FirstCom = sValue;
                            return sValue;
                        }
                    }
                    catch(Exception ex) {
                        _tracing.Error(ex, "failed to try connect and send test ."+sValue);
                    }
                    }
                }
            }
            return "";
        }

        public static Dictionary<string, string> GetComList()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            RegistryKey keyCom = Registry.LocalMachine.OpenSubKey("Hardware\\DeviceMap\\SerialComm");
            if (keyCom != null)
            {
                string[] sSubKeys = keyCom.GetValueNames();
                foreach (string sName in sSubKeys)
                {
                    string sValue = (string)keyCom.GetValue(sName);
                    try {
                        bool b = SerialPortTran.IsTheDevice(sValue);
                        if (b)
                            result.Add(sValue, sName);
                    }
                    catch (Exception ex)
                    {
                        _tracing.Error(ex, "failed to GetComList ." + sValue);
                    }
                }
            } 
            return result;
        }

        private static  Dictionary<string, string> test2()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();

            string[] spname = SerialPort.GetPortNames();

            

            if (spname.Length > 0)
            {
                foreach (string spName in spname)
                {
                    result.Add(spName, spName);//得到所有串口号
                    try
                    {
                        bool b = SerialPortTran.IsTheDevice(spName);

                    }
                    catch { }
                }
            }
            return result;
        }
        #endregion

        #region infos 
        /*
         产品/SN   (4byte) --- 高位在前            =>{B3、B4、B5、B6}
软件版本  (1byte) ---                     =>{B7}
工作状态  (1byte) --- Bit0 校准位 1：校准 0：未校准
Bit1 设定位 1：设定 0：未设定
Bit2 起动位 1：起动 0：未起动
Bit3 停止位 1：停止 0：未停止
Bit4 校准中位 1：校准中
Bit5 快采集位 1：快速 0：普通
Bit6…Bit7 备用     =>{B8}
电池电量  (1byte)                         =>{B9}
已用空间  (2byte) ---  存储结束地址       =>{B10、B11}
总容量    (2byte) ---  EEPROM  总容量     =>{B12、B13}
超温字节  (1byte) ---  超标点数EEPROM的起始地址为 0x0840
=>{B14} 
设定日期  (3byte) ---  年：月：日         =>{B15、B16、B17}
设定时间  (3byte) ---  时：分：秒         =>{B18、B19、B20} 
记录日期  (3byte) ---  年：月：日         =>{B21、B22、B23}
记录时间  (3byte) ---  时：分：秒         =>{B24、B25、B26} 
结束日期  (3byte) ---  年：月：日         =>{B27、B28、B29}
结束时间  (3byte) ---  时：分：秒         =>{B30、B31、B32}

         */
        #endregion
        #region setting 
        /*
         应答<-0x68 0x15 LEN B3…B29 FCS 0x0A
产品/SN   (4byte) --- 高位在前            =>{B3、B4、B5、B6}
采样方式  (1byte) --- 采样方式（10s、60s）;采样总时间;采样间隔
                                          =>{B7、B8、B9}
报警设置  (1byte) ---  0x00 禁止[上下限无效]
		               0x8C 上限[摄氏]   0x1C 下限[摄氏]
		               0x8F 上限[华氏]   0x1F 下限[华氏]
=>{B10}
温度上限  (2byte) --- bit7-bit0数值      =>{B11,B12}
温度下限  (2byte) --- bit7-bit0数值      =>{B13,B14}
延时时间  (3byte) --- 时：分：秒          =>{B15、B16、B17}
启动延时  (3byte) --- 时：分：秒          =>{B18、B19、B20} 
当前日期  (3byte) --- 年：月：日          =>{B21、B22、B23}
当前时间  (3byte) --- 时：分：秒          =>{B24、B25、B26}
工作状态  (1byte) --- Bit0 校准位 1：校准 0：未校准
Bit1 设定位 1：设定 0：未设定
Bit2 起动位 1：起动 0：未起动
Bit3 停止位 1：停止 0：未停止
Bit4 校准中位 1：校准中
Bit4…Bit7 备用     =>{B27}
校准参数 (2byte) ---  校准后的修正值      =>{B28、B29}
当前温度 (2byte) ---  12bit 标准格式      =>{B30、B31}

         */

        #endregion
    }
}
