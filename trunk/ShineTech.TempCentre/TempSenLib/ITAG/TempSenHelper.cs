using System;
using System.Collections.Generic;
using System.Text;

namespace TempSenLib
{
    public class TempSenHelper
    {
        public static List<int> GetTempListC(string str,int length)
        {
            //length = length + 16;
            byte[] bytes = Utils.HexToByte(str);
            List<int> rst = new List<int>();
            int i=0;
            for (i = 0; i < bytes.Length / 3; i++)
            {
                int a = 0, b = 0;
                a = Utils.BytesToIntp(bytes[i * 3 + 0], (byte)(bytes[i * 3 + 1] & 0xF0), (byte)0, (byte)0) / 256 / 256 / 16 * 100 / 16;
                b = Utils.BytesToIntp(bytes[i * 3 + 2], (byte)((bytes[i * 3 + 1] & 0x0F)*16), (byte)0, (byte)0) / 256 / 256 / 16 * 100 / 16;
                rst.Add(a);
                rst.Add(b);
                if (rst.Count >= length)
                    break;
                
            }
            if (rst.Count < length)
            {
                int left = str.Length - i * 3;
                if (left == 2)
                {
                    int a = bytes[i * 3 + 0] * 100 + bytes[i * 3 + 1] / 16 * 100 / 16;
                    int b = bytes[i * 3 + 1] % 16 * 100 / 16;
                    rst.Add(a);
                    rst.Add(b);
                }
            }
            List<int> rt = new List<int>();
            foreach (int a in rst)
            {
             if(rt.Count<length)
                rt.Add(a);
            }

            return rt;
        }

        public static string GetTempListCString(string str, int length)
        {
            List<int> rst = GetTempListC(str,length);
            StringBuilder sb = new StringBuilder();
            foreach (int a in rst)
            {
                sb.AppendLine(TempIntToString(a));
            }
            return sb.ToString();
        }

        public static string GetTempListCString(string str, int length,DateTime start,int interval)
        {
            List<int> rst = GetTempListC(str, length);
            StringBuilder sb = new StringBuilder();
            foreach (int a in rst)
            {
                sb.AppendLine(start.ToString("yyyy-MM-dd HH:mm:ss")+","+ TempIntToString(a));
                start=start.AddSeconds(interval);
            }
            return sb.ToString();
        }


        public static List<string> GetTempListCStringList(string str, int length)
        {
            List<int> rst = GetTempListC(str, length);
            List<string> result = new List<string>();
            foreach (int a in rst)
            {
                result.Add(TempIntToString(a));
            }
            return result;
        }

        public static List<string> GetTempListCStringList(string str, int length, DateTime start, int interval)
        {
            List<int> rst = GetTempListC(str, length);
            List<string> result = new List<string>();
            foreach (int a in rst)
            {
                result.Add(start.ToString("yyyy-MM-dd HH:mm:ss") + "," + TempIntToString(a));
                start=start.AddSeconds(interval);
            }
            return result;
        }
        [Obsolete("never used",true)]
        public static Dictionary<string,string> GetTempListCStringDic(string str, int length, DateTime start, int interval)
        {
            List<int> rst = GetTempListC(str, length);
            Dictionary<string, string> result = new Dictionary<string, string>();
            foreach (int a in rst)
            {
                result.Add(start.ToString("yyyy-MM-dd HH:mm:ss") ,TempIntToString(a));
                start = start.AddSeconds(interval);
            }
            return result;
        }

        public static string TempIntToString(int value)
        {
            return (1.0*value/100).ToString("F1");
        }

        public static string TempIntToStringDropDigi(int value)
        {
            string rt=(1.0 * value / 100).ToString("F02");
            if (rt.Length > 0)
                return rt.Substring(0, rt.Length - 1);
            else
                return "0.0";
        }


        public static double TempIntTodouble(int value)
        {
            return Math.Round((1.0 * value / 100),1,MidpointRounding.AwayFromZero);
        }

        public static byte bx = (byte)0x80;

        /// <summary>
        /// 1.5byte to tempx100  int value .
        /// </summary>
        /// <param name="b0"></param>
        /// <param name="b2"></param>
        /// <returns></returns>
        public static int bytesToTemp(byte b0, byte b2)
        {
            return Utils.BytesToIntp(b0, (byte)(b2 & 0xF0), (byte)0, (byte)0)/256/256/16*100/16;

            //if ((b0 & bx) > 0)
            //{
            //    return -1 * (~b0 * 100 + (~(b2 / 16 - 1)) * 100 / 16);
            
            //}else
            //    return b0 * 100 + b2 / 16 * 100 / 16;
        }
        /// <summary>
        /// a double temp value to 2byte value .
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] TempDouble2Byte(double value)
        {
            byte[] rst = new byte[2];
            rst[0] = (byte)value;
            rst[1] = (byte)(value % 1 * 16 * 16);
            return rst;
        }

    }
}
