using System;
using System.Collections.Generic;
using System.Text;

namespace TempSenLib
{
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
    public class DeviceInfo
    {
        public DeviceInfo(string result)
        {
            byte[] bytes = Utils.HexToByte(result);
            sn = Utils.BytesToIntp(bytes[3], bytes[4], bytes[5], bytes[6]).ToString();
            version = Utils.BytesToIntp(bytes[7]).ToString();
            status = bytes[8];
            battery = Utils.BytesToIntp(bytes[9]).ToString();
            usedSpace = Utils.BytesToIntp(bytes[10], bytes[11]);
            totalSpace = Utils.BytesToIntp(bytes[12], bytes[13]);
            overLimitByte = bytes[14];
            SettingDateTime = new DateTime(2000+(int)bytes[15], (int)bytes[16], (int)bytes[17], (int)bytes[18], (int)bytes[19], (int)bytes[20]);
            RecordDateTime = new DateTime(2000+(int)bytes[21], (int)bytes[22], (int)bytes[23], (int)bytes[24], (int)bytes[25], (int)bytes[26]);
            FinishDateTime = new DateTime(2000+(int)bytes[27], (int)bytes[28], (int)bytes[29], (int)bytes[30], (int)bytes[31], (int)bytes[32]);

        
        }
        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("产品/SN :" + sn);
            sb.AppendLine("软件版本:" + version);
            sb.AppendLine("工作状态:" + status);
            sb.AppendLine("电池电量:" + battery);
            sb.AppendLine("已用空间:" + usedSpace);
            sb.AppendLine("总容量:" + totalSpace);
            sb.AppendLine("超温字节:" + overLimitByte);
            sb.AppendLine("设定时间:" + SettingDateTime);
            sb.AppendLine("记录时间:" + RecordDateTime);
            sb.AppendLine("结束时间:" + FinishDateTime);
            return sb.ToString();
        }
        public string sn { get; set; }
        public string version { get; set; }
        public byte status { get; set; }
        public string battery { get; set; }
        public int usedSpace { get; set; }
        public int totalSpace { get; set; }
        public byte overLimitByte { get; set; }
        public DateTime SettingDateTime { get; set; }
        public DateTime RecordDateTime { get; set; }
        public DateTime FinishDateTime { get; set; }

    }
}
