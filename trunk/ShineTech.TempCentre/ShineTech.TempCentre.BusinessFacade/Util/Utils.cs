using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Xml.Linq;
using System.Security.Cryptography;

namespace ShineTech.TempCentre.BusinessFacade
{
    public class Utils
    {
        /// <summary>
        /// 将二进制数据转换成图片
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static System.Drawing.Image ReadSource(byte []data)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(data, 0, data.Length, true, true);
            return System.Drawing.Image.FromStream(ms);
        }
        /// <summary>
        /// 将图片转换成二进制
        /// </summary>
        /// <param name="image">图片</param>
        /// <returns></returns>
        public static  byte[] CopyToBinary(Image image)
        {
            System.IO.MemoryStream ms=new System.IO.MemoryStream();
            image.Save(ms,System.Drawing.Imaging.ImageFormat.Png);
            byte [] data=new byte[ms.Length];
            ms.Position=0;
            ms.Read(data,0,data.Length);
            return data;
        }
        public static string ReadKeyFromXML()
        {
            string path = "srcsafe.xml";
            var v = from c in  XElement.Load(path).Descendants("system").Elements("dbconfig")
                        where c.Name=="key"
                        select c;
            string key = "";
            foreach (string i in v)
                key = i;
            return key;
        }
        public static string ReadPwdFromXML()
        {
            string path = "srcsafe.xml";
            var v = from c in XElement.Load(path).Descendants("system").Elements("dbconfig")
                    where c.Name == "pwd"
                    select c;
            string key = "";
            foreach (string i in v)
                key = i;
            return key;
        }
        public static string ReadIVFromXML()
        {
            string path = "srcsafe.xml";
            var v = from c in XElement.Load(path).Descendants("system").Elements("dbconfig")
                    where c.Name == "iv"
                    select c;
            string key = "";
            foreach (string i in v)
                key = i;
            return key;
        }
        
        public static string Encode(string plaintext, byte[] key, byte[] iv)
        {
            byte[] plain=Encoding.UTF8.GetBytes(plaintext);
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                using (CryptoStream crypto = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateEncryptor(key, iv), CryptoStreamMode.Write))
                {
                    crypto.Write(plain, 0, plain.Length);
                    return Convert.ToBase64String(ms.ToArray());
                }
            }
          
        }
        public static string Decode(string encypttext,string key,string iv)
        {
            byte[] plain = Encoding.UTF8.GetBytes(encypttext);
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(plain))
            {
                using(CryptoStream crypto = new CryptoStream(ms, new TripleDESCryptoServiceProvider().CreateDecryptor(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(iv)), CryptoStreamMode.Read))        
                {
                    byte[] decode = new byte[plain.Length];
                    crypto.Read(decode, 0, decode.Length);
                    return Convert.ToBase64String(decode);
                }
            }
        }
        public static string GenerateGuid()
        {
            return System.Guid.NewGuid().ToString();
        }
        /// <summary>
        /// 写入XML配置文件
        /// </summary>
        /// <returns></returns>
        public static bool WriteToXML()
        {
            TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
            string key = Convert.ToBase64String(des.Key);
            string iv = Convert.ToBase64String(des.IV);
            string pwd = Encode(GenerateGuid().Substring(0, 8) + "ts", des.Key, des.IV);
            XElement config = new XElement("system",new XElement("dbconfig"
                                                                             ,new XElement("key",key)
                                                                             ,new XElement("iv",iv)
                                                                             ,new XElement("pwd",pwd)
                                                                             ));
            config.Save("srcsafe.xml");
            return true;
        }
    }
}
