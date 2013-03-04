using System;
using System.Collections.Generic;
using System.Text;

namespace TempSenLib
{
    public class Utils
    {
        public static string ToHexStringp(params byte[] bytes)
        {

            return ToHexString(bytes);

        }
        public static string ToHexString(byte[] bytes) // 0xae00cf => "AE00CF "
        {
            string hexString = string.Empty;
            if (bytes != null)
            {
                StringBuilder strB = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    strB.Append(bytes[i].ToString("X2"));
                }
                hexString = strB.ToString();
            }
            return hexString;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="a"></param>
        /// <param name="byteLen"></param>
        /// <returns></returns>
        public static string IntToHexString(int a, int byteLen)
        {
            byte[] bytes =System.BitConverter.GetBytes(a);
            Array.Reverse(bytes);
            string str=ToHexString(bytes);
            if (byteLen * 2 <= str.Length)
                return str.Substring(str.Length - byteLen * 2, byteLen * 2);
            else
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < byteLen*2 - str.Length; i++)
                {
                    sb.Append("0");
                }
                return sb.ToString() + str;
            
            }
        }
        public static string IntToHexString(int a)
        {
            return IntToHexString(a, 4);
        }
        public static int BytesToInt(byte[] bytes)
        {
            byte[] bs = new byte[4];
            if (bytes.Length < 4)
            {

                for (int i = 0; i < bytes.Length; i++)
                    bs[i] = bytes[i];

            }
            else
                bs = bytes;

            int sh = System.BitConverter.ToInt32(bs, 0);
            System.BitConverter.ToSingle(bs, 0);
            return sh;
        }
        /// <summary>
        /// Utils.BytesToIntp(2, 0, 0, 0)/256/256/256=2;
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static int BytesToIntp(params byte[] bytes)
        {
            Array.Reverse(bytes);
            int sh = BytesToInt(bytes);
            return sh;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns></returns>
        public static byte[] HexToByte(string hexString)
        {
            if (string.IsNullOrEmpty(hexString))
            {
                hexString = "00";
            }
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        public static string HexToValueString(string hex)
        {
            byte[] bs = HexToByte(hex);
            StringBuilder sb = new StringBuilder();
            foreach (byte b in bs)
            {
                if (b >= '!' && b <= '~')
                    sb.Append((char)b);
                else
                    sb.Append(' ');
            }
            return sb.ToString();
        
        }
        public static string ValueStringToHex(string value)
        {
            byte[] bytes=Encoding.ASCII.GetBytes(value);
            return ToHexString(bytes);
        
        }

        public static string FillString(string orgiStr, int length, char FillWithStr)
        {
            string fvalue = orgiStr;
            if (orgiStr.Length > length)
                return orgiStr.Substring(0, length);
            else
            {
                for (int i=0; i < length - orgiStr.Length; i++)
                    fvalue += FillWithStr;
            }
            return fvalue;

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hexString"></param>
        /// <param name="discarded"></param>
        /// <returns></returns>
        public static byte[] GetBytes(string hexString, out int discarded)
        {
            discarded = 0;
            string newString = "";
            char c;
            // remove all none A-F, 0-9, characters
            for (int i = 0; i < hexString.Length; i++)
            {
                c = hexString[i];
                if (Uri.IsHexDigit(c))
                    newString += c;
                else
                    discarded++;
            }
            // if odd number of characters, discard last character
            if (newString.Length % 2 != 0)
            {
                discarded++;
                newString = newString.Substring(0, newString.Length - 1);
            }

            return HexToByte(newString);
        }

    }
}
