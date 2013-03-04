using System;
using System.Collections.Generic;
using System.Text;

namespace TempSenLib
{
    public class DeviceSetting
    {
      /*           应答<-0x68 0x15 LEN B3…B29 FCS 0x0A
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

        public DeviceSetting(string result)
        {
            byte[] bytes = Utils.HexToByte(result);
            sn = Utils.BytesToIntp(bytes[3], bytes[4], bytes[5], bytes[6]).ToString();
            recordtype = Utils.ToHexStringp(bytes[7], bytes[8], bytes[9]);
            recordmethods = bytes[7];
            recordCycle = bytes[8];
            recordInterval = bytes[9];



            alerttype = Utils.ToHexStringp(bytes[10]);
            tempup = bytesToTemp(bytes[11], bytes[12]);
            tempdown = bytesToTemp(bytes[13], bytes[14]);
            delaytime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, bytes[15], bytes[16], bytes[17]);
            delaystart = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, bytes[18], bytes[19], bytes[20]);
            int month=bytes[22]%12;
            if(month==0) month=12;
            datetimenow = new DateTime(2000 + (bytes[21] % 100), month, bytes[23] % 32, bytes[24] % 24, bytes[25] % 60, bytes[26] % 60);
            workstatus = Utils.ToHexStringp(bytes[27]);
            checkvalue = Utils.ToHexStringp(bytes[28], bytes[29]);
            tempnow = bytesToTemp(bytes[28], bytes[29]);

        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("产品/SN :" + sn);
            sb.AppendLine("采样方式:" + recordtype);
            sb.AppendLine("recordmethods:" + recordmethods+" s");
            sb.AppendLine("recordCycle:" + recordCycle+" h");
            sb.AppendLine("recordInterval:" + recordInterval+" s");
            sb.AppendLine("报警设置:" + alerttype);
            sb.AppendLine("温度上限:" + tempup);
            sb.AppendLine("温度下限:" + tempdown);
            sb.AppendLine("延时时间:" + delaytime);
            sb.AppendLine("启动延时:" + delaystart);
            sb.AppendLine("当前时间:" + datetimenow);
            sb.AppendLine("工作状态:" + workstatus);
            sb.AppendLine("校准参数:" + checkvalue);
            sb.AppendLine("当前温度:" + tempnow);
            return sb.ToString();
        }

        public int bytesToTemp(byte b0,byte b2)
        {
            return TempSenHelper.bytesToTemp(b0, b2);
        }
        
        /// <summary>
        /// °C=(°F-32)*5/9
        /// F=c*9/5+32 
        /// </summary>
        /// <param name="c">value * 100</param>
        /// <returns>value *100 </returns>
        public int C2F(int c)
        {
            int result=c * 9 /5 + 3200;
            result = ((int)Math.Round(1.0 * result / 100, 0)) *100;
            return result;
        }

        public string sn { get; set; }
        public string recordtype { get; set; }
        public int recordmethods { get; set; }
        public int recordCycle { get; set; }
        public int recordInterval { get; set; }

        public int recordIntervalInSecond
        {
            get
            {
                if (recordmethods == 10)
                    return recordInterval;
                else
                    return recordInterval * 60;
            }
        }

        public string alerttype { get; set; }
        public int tempup { get; set; }
        public int tempdown { get; set; }
        public DateTime delaytime { get; set; }
        public DateTime delaystart { get; set; }
        public DateTime datetimenow { get; set; }
        public string workstatus { get; set; }
        public string checkvalue { get; set; }
        public int tempnow { get; set; }

    }
}
