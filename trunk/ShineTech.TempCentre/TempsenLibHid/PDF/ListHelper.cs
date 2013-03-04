using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace TempsenLibHid.PDF
{
    public static class ListHelper
    {
        public static int IntPause(string value,NumberStyles style)
        {
            return IntPause(value,0, style);
        }
        public static int IntPause(string value)
        {
            return IntPause(value,0, NumberStyles.Any);
        }
        public static int IntPause(string value, int def, NumberStyles style)
        {
            try
            {
                return int.Parse(value,style);
            }
            catch (System.Exception ex)
            {
                return def;
            }
        }

        public static string ToFullString(this List<string> list)
        {
            StringBuilder sb = new StringBuilder();
            foreach (string line in list)
                sb.AppendLine(line);
            return sb.ToString();
        }
        public static string ToFullString(this string line)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(line);
            return sb.ToString();
        }
        public static string ToFullString(this int line)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(line.ToString());
            return sb.ToString();
        }
        public static string ToFullString(this DateTime line)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(line.ToString("yyyy-MM-dd HH:mm:ss"));
            return sb.ToString();
        }
        public static string SubStringEx(this string str, int index, int length)
        {
            if (str.Length > index + length)
            {
                return str.Substring(index, length);
            }
            else if (str.Length > index)
            {
                return str.Substring(index);
            }
            else
                return "";

        }

        public static string SubStringEx(this string str, int index)
        {
            if (str.Length > index)
            {
                return str.Substring(index);
            }
            else
                return "";

        }
    }
}
