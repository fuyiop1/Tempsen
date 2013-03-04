using System;
using System.Collections.Generic;
using System.Text;
using TempSenLib;
using Services.Common;

namespace TempSen
{
    public class device
    {
        private static ITracing _tracing = TracingManager.GetTracing(typeof(int));
        public device(int code)
        {
            Code = code;
        }
        private int Code { get; set; }
        public SerialPortTran spt;
        private DeviceInfo devInfo { get; set; }
        private DeviceSetting devSetting { get; set; }
        private DeviceTempList TempList { get; set; }
        private bool ReadDone = false;
        public bool connectDevice()
        {
            if (Code == 11)
                return false;
            try
            {
                string comname = SerialHelper.GetOneCom();
                ReadDone = false;
                if (comname.Length > 0)
                {
                    SerialPortFixer.Execute(comname);
                    spt = new SerialPortTran(comname, false);
                    return true;
                }
                else
                {
                    //throw new Exception("can not found any device connected");
                    _tracing.Error("can not found any device connected");
                    return false;
                }

            }
            catch {

                return false;
            }
        }
        public bool disconnectDevice() {

            ReadDone = false;
            try
            {
                if (spt != null)
                {
                    spt.Dispose();
                }
                spt = null;

                return true;
            }
            catch
            {
                return false;
            }
        }
        /// <summary>
        /// 1 – request temperature list(temperature in Celsius). 2 – request temperature(temperature in Celsius), date and time list.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public string[] getRecord(int parameter) {
            try
            {
                DoRead();
                List<string> result = new List<string>();
                switch(parameter)
                {
                    case 1:
                        result = TempList.TempStrList;
                        return result.ToArray();
                    case 2:
                        result = TempList.TempDateStrList;
                        return result.ToArray();
                    default:
//                        throw new Exception("400 parameter error");
                        return new string[1];
                }
            }
            catch {
                //throw new Exception("500 getrecord process error");
                return new string[1];
            }
        }
        public string[] getAlarmSet() {

            
            string[] result= new string[5];
            result[0] = Status.OK;

            try
            {
                DoRead();
                string alermtype = devSetting.alerttype;
                string alermunit = "";
                if (alermtype.EndsWith("F"))
                    alermunit = "F";
                else
                    alermunit = "C";
                int limittype = Utils.BytesToInt(Utils.HexToByte(alermtype)) / 16;

                result[1] = alermunit;
                if (alermunit == "C")
                {
                    if ((8 & limittype) > 0)
                        result[2] = TempSenHelper.TempIntToStringDropDigi(devSetting.tempup);
                    else
                        result[2] = "--";
                    if ((1 & limittype) > 0)
                        result[3] = TempSenHelper.TempIntToStringDropDigi(devSetting.tempdown);
                    else
                        result[3] = "--";
                }else
                {
                    if ((8 & limittype) > 0)
                        result[2] = TempSenHelper.TempIntToStringDropDigi(devSetting.C2F(devSetting.tempup));
                    else
                        result[2] = "--";
                    if ((1 & limittype) > 0)
                        result[3] = TempSenHelper.TempIntToStringDropDigi(devSetting.C2F(devSetting.tempdown));
                    else
                        result[3] = "--";
                }
                
                result[4] = string.Format("{0:D2}h{1:D2}m{2:D2}s", devSetting.delaytime.Hour, devSetting.delaytime.Minute, devSetting.delaytime.Second);

            }
            catch (Exception ex){
                Console.WriteLine(ex.Message + ex.StackTrace);
                result[0] = Status.NoSet;
            }
            return result;
        
        }
        public string[] getOtherInfo()
        {
            string[] result = new string[3];
            result[0] = Status.OK;
            try
            {
                DoRead();
                result[1] = devInfo.battery;
                int pt=devInfo.usedSpace*100/devInfo.totalSpace;
                if(pt<0) pt=0;
                if(pt>100) pt=100;
                result[2] = pt.ToString();
            }
            catch
            {
                result[0] = Status.NoConfig;
            }

            return result;
        }


        public string[] getConfig()
        {
            string[] result = new string[7];
            result[0] = Status.OK;
            try
            {
                DoRead();
                result[1] = devInfo.sn;
                if (devSetting.recordmethods==10)
                {
                    result[2] = devSetting.recordInterval.ToString()+"s";  // m or s
                    result[3] = devSetting.recordCycle.ToString()+"h";         //d or h.
                }else
                {
                    result[2] = devSetting.recordInterval.ToString()+"m";  // m or s
                    result[3] = devSetting.recordCycle.ToString()+"d";         //d or h.
                }
                result[4] = string.Format("{0:D2}h{1:D2}m{2:D2}s", devSetting.delaystart.Hour, devSetting.delaystart.Minute, devSetting.delaystart.Second);
                result[5] = spt.ItemCount.ToString();
                result[6] = devInfo.RecordDateTime.ToString("yyyy-MM-dd HH:mm:ss");


            }
            catch
            {
                result[0] = Status.NoConfig;
            }

            return result;
        }
        public string[] getAnalysis()
        {
            string[] result = new string[5];
            result[0] = Status.OK;
            try
            {
                DoRead();
                result[1] = TempSenHelper.TempIntToString(TempList.TempAvg);
                result[2] = TempSenHelper.TempIntToString(TempList.TempHigh);
                result[3] = TempSenHelper.TempIntToString(TempList.TempLow);
                //result[4] = "MKT: do you have the code to calc MKT?";
#region calc the mkt .

                /////////////////////////////////////////////////
                double ndot, sum, ln, mkt;

                sum = 0;
                ndot = 0;
                try
                {
                    foreach (int iStr in TempList.TempIntList)
                    {
                        sum += Math.Exp(-10 / TempSenHelper.TempIntTodouble(iStr+27315));
                        ndot++;
                    }

                    ln = Math.Log(sum / ndot);

                    mkt = 10 / (-ln) - 273.15;

                    result[4] = mkt.ToString("F01");// Convert.ToString(mkt);             // MKT 平均动能温度
                    //F02
                }
                catch
                {
                    result[4] = "0.0";
                }
#endregion

            }
            catch
            {
                result[0] = Status.NoData;
            }
            return result;
        }

        private void DoRead()
        {
            DoRead(false);
        }

        private void DoRead(bool fouceRead)
        {
            if (!ReadDone || fouceRead)
            {
                try{
                if (spt == null || !spt.connected)
                {
                    connectDevice();
                }
                devInfo = new DeviceInfo(spt.SendGetInfo());
                devSetting = new DeviceSetting(spt.SentGetSetting());

                string tempListstr = spt.SentGetRecords();
                TempList = new DeviceTempList(tempListstr, devInfo.usedSpace, devInfo.RecordDateTime, devSetting.recordIntervalInSecond);

                //Console.WriteLine(temps);
                //string s = TempSenHelper.GetTempListCString(tempList, spt.ItemCount, devInfo.RecordDateTime, devSetting.recordInterval);
                }catch(Exception ex){
                    ReadDone=false;
                    Console.WriteLine(ex.Message + ex.StackTrace);
                    throw new Exception("do read failed.");
                }
            }

            if (devInfo != null && devSetting != null && TempList!=null)
                ReadDone = true;


        }


        public bool SendSetConfigTime(DateTime dtNow, DateTime correctTime, double TempNow)
        {
            return spt.SendSetConfigTime(dtNow, correctTime, TempNow);
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
        /// b7 10或60
        /// b8 当b7为10，则b8单位为小时  b7为60,b8单位为天
        /// b9 当b7为10，则b8单位为秒  b7为60,b8单位为分钟
        ///</param>
        /// <param name="alerttype">1byte hex string
        /// 报警设置  (1byte) --- 0x00 禁止[上下限无效]
        ///	0x8C 上限[摄氏]   0x1C 下限[摄氏]  9C 上限加下限
        /// 0x8F 上限[华氏]   0x1F 下限[华氏]  9F 上限加下限
        ///</param>
        /// <param name="upTemp"></param>
        /// <param name="downTemp"></param>
        /// <param name="alertDealy"></param>
        /// <param name="startDelay"></param>
        /// <param name="dtnow"></param>
        /// <param name="isFastRecord"></param>
        /// <returns></returns>
        public bool SendSetConfigInfo(int sn, string recordtype, string alerttype, double upTemp, double downTemp, DateTime alertDelay, DateTime startDelay, DateTime dtnow, bool isFastRecord)
        {
            return spt.SendSetConfigInfo(sn, recordtype, alerttype, upTemp, downTemp, alertDelay, startDelay, dtnow, isFastRecord);
        }

    }

    public class Status
    { 
        public static readonly string OK="OK";
        public static readonly string NoDev="Error:Cann't Found ITAG-SingleUse Temperature Label!";
        public static readonly string NoSet = "Error: Can't get alarm settings!";
        public static readonly string NoConfig = "Error: Can't get alarm settings!";
        public static readonly string NoData = "Error: No valid data!";
    }
}
