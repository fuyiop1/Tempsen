using System;
using System.Collections.Generic;
using System.Text;

namespace TempSenLib
{
    public class DeviceTempList
    {
        public List<string> TempStrList = new List<string>();
        public List<string> TempDateStrList = new List<string>();
        public List<int> TempIntList = new List<int>();
        //public Dictionary<string, string> tempDic = new Dictionary<string, string>();



        public string orgiStr{get;set;}
        public DateTime startDateTime{get;set;}
        public int interval{get;set;}
        public int length{get;set;}
        public string TempString { get; set; }
        public string TempDateString { get; set; }
        public DeviceTempList(string str,int length,DateTime start, int interval)
        {
            this.length=length;
            this.interval=interval;
            this.startDateTime=start;
            this.orgiStr=str;
            TempIntList=TempSenHelper.GetTempListC(str,length);
            TempStrList=TempSenHelper.GetTempListCStringList(str,length);
            TempDateStrList = TempSenHelper.GetTempListCStringList(str, length, start, interval);
            //tempDic=TempSenHelper.GetTempListCStringDic(str,length,startDateTime,interval);
            TempString = TempSenHelper.GetTempListCString(str, length);
            TempDateString = TempSenHelper.GetTempListCString(str, length, start, interval);
        }
        public int TempAvg
        {
            get
            {
                double sum = 0;
                foreach (int v in TempIntList)
                {
                    double x = (Math.Round(1.0 * v / 100, 1, MidpointRounding.AwayFromZero));
                    sum +=  x;
                }
                return (int)(Math.Round(sum / TempIntList.Count,2,MidpointRounding.AwayFromZero)*100);
            }
        }
        public int TempHigh
        {
            get
            {
                int High = 0;
                foreach (int v in TempIntList)
                {
                    if (v > High)
                        High = v;
                }
                return High;
            }
        }
        public int TempLow
        {
            get
            {
                int Low = TempIntList[0];
                foreach (int v in TempIntList)
                {
                    if (v < Low)
                        Low = v;
                }
                return Low;
            }
        }

    }
}
