using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Xml;
using System.Xml.Linq;
using System.Security.Cryptography;
using System.IO;
using System.Xml.Serialization;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace ShineTech.TempCentre.Platform
{
    public class Utils
    {
        private static string path = Path.Combine(Application.StartupPath, "srcsafe.xml");
        /// <summary>
        /// 将二进制数据转换成图片
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static System.Drawing.Image ReadSource(byte []data)
        {
            if (data != null)
            {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(data, 0, data.Length, true, true))
                {
                    return System.Drawing.Image.FromStream(ms);
                }
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// 将图片转换成二进制
        /// </summary>
        /// <param name="image">图片</param>
        /// <returns></returns>
        public static  byte[] CopyToBinary(Image image)
        {
            if (image == null)
                return null;
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            {
                Bitmap newimg = new Bitmap(image);
                //image.Dispose();
                newimg.Save(ms, ImageFormat.Png);
                byte[] data = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(data, 0, data.Length);
                ms.Close();
                return data;
            }
        }

        public static string ReadKeyFromXML()
        {
            //string path = "srcsafe.xml";
            if (File.Exists(path))
            {
                var v = from c in XElement.Load(path).Descendants("dbconfig").Elements()
                        where c.Name == "key"
                        select c;
                string key = "";
                foreach (string i in v)
                    key = i;
                return key;
            }
            else
                return "";
        }
        public static string ReadPwdFromXML()
        {
            //string path = "srcsafe.xml";
            if (File.Exists(path))
            {
                var v = from c in XElement.Load(path).Descendants("dbconfig").Elements()
                        where c.Name == "pwd"
                        select c;
                string key = "";
                foreach (string i in v)
                    key = i;
                return key;
            }
            return "";
        }
        public static string ReadIVFromXML()
        {
            //string path = "srcsafe.xml";
            if (File.Exists(path))
            {
                var v = from c in XElement.Load(path).Descendants("dbconfig").Elements()
                        where c.Name == "iv"
                        select c;
                string key = "";
                foreach (string i in v)
                    key = i;
                return key;
            }
            else
                return "";
        }
        
        public static string Encode(string plaintext, byte[] key, byte[] iv)
        {
            byte[] plain=Encoding.Default.GetBytes(plaintext);
            //using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
            //{
            //    using (TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider())
            //    {
            //        using (CryptoStream crypto = new CryptoStream(ms, provider.CreateEncryptor(key, iv), CryptoStreamMode.Write))
            //        {
            //            crypto.Write(plain, 0, plain.Length);
            //            return ms.ToArray();
            //        }
            //    }
            //}
            return plaintext;
          
        }
        public static string Decode(string encypttext,string key,string iv)
        {
            //string a = Encoding.Default.GetString(Convert.FromBase64String(encypttext));
            //byte[] plain = Encoding.Default.GetBytes(Encoding.Default.GetString(Convert.FromBase64String(encypttext)));
            //byte[] plain = Convert.FromBase64String(encypttext);            
            //using (System.IO.MemoryStream ms = new System.IO.MemoryStream(plain))
            //{
            //    using (TripleDESCryptoServiceProvider provider = new TripleDESCryptoServiceProvider())
            //    {
            //        using (CryptoStream crypto = new CryptoStream(ms, provider.CreateDecryptor(Convert.FromBase64String(key), Convert.FromBase64String(iv)), CryptoStreamMode.Read))
            //        {
            //            byte[] decode = new byte[plain.Length];
            //            crypto.Read(decode, 0, decode.Length);
            //            return Convert.ToBase64String(decode);
            //        }
            //    }
            //}
            return encypttext;
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
            if (!File.Exists(path))
            {
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
                string key = Convert.ToBase64String(des.Key);
                string iv = Convert.ToBase64String(des.IV);
                //string plaintext = Convert.ToBase64String(Encoding.Default.GetBytes(GenerateGuid().Substring(0, 8)));
                string plaintext = GenerateGuid();
                string pwd = Encode(plaintext, des.Key, des.IV);
                XElement config = new XElement("system", new XElement("dbconfig"
                                                                                 , new XElement("key", key)
                                                                                 , new XElement("iv", iv)
                                                                                 , new XElement("pwd", pwd)
                                                                                 ));
                config.Save(path);
                return true;
            }
            else
                return false;
           
        }

        /// <summary>
        /// 读取文件到内存中
        /// </summary>
        /// <param name="path">文件</param>
        /// <returns>content</returns>
        public static string Read(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("{0} does not exist.", path);
                return "";
            }
            using (StreamReader sr = File.OpenText(path))
            {
                string input;
                StringBuilder sb = new StringBuilder();
                while ((input = sr.ReadLine()) != null)
                {
                    sb.AppendLine(input);
                }
                sr.Close();
                return sb.ToString();
            }
        }
        public static byte[] SerializeToXML<T>(T t)
        {
            try
            {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    /*序列化*/
                    //XmlSerializer xmlSerializer = new XmlSerializer(t.GetType());
                    XmlSerializer xmlSerializer = new XmlSerializer(typeof(T));
                    xmlSerializer.Serialize(ms, t);
                    return ms.ToArray();
                }
            }
            catch { return null;}
        }
        /// <summary>
        /// 反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="CipherText"></param>
        /// <returns></returns>
        public static TResult DeserializeFromXML<TResult>(byte[] CipherText,Type type) where TResult : class
        {
            if (CipherText != null && CipherText.Length > 0)
            {
                TResult t;
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(CipherText))
                {
                    XmlSerializer serializer = new XmlSerializer(type);
                    t = serializer.Deserialize(ms) as TResult;
                }
                return t;
            }
            else
                return null;
        }
        public static void SaveTheFile(byte[] text, string path)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(path.Substring(0, path.LastIndexOf("\\") + 1));
            if (!directoryInfo.Exists)
            {
                directoryInfo.Create();
            }
            using (FileStream stream = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                stream.SetLength(text.Length);
                stream.Write(text, 0, text.Length);
                stream.Close();
            }
        }
        public static byte[] ReadFromFile(string path)
        {
            using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                //stream.
                byte [] text=new byte[stream.Length];
                stream.Read(text, 0, text.Length);
                stream.Close();
                return text;
            }
        }

        public static bool IsFileExist(string filePath)
        {
            bool result = false;
            FileInfo file = new FileInfo(filePath);
            if (file.Exists)
            {
                result = true;
            }
            return result;
        }

        public static Image DrawTextOnImage(Image b, string txt, int x, int y)
        {
            if (b == null)
            {
                return null;
            }

            Graphics g = Graphics.FromImage(b);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            //Arial字体，大小为9，白色。
            FontFamily fm = new FontFamily("Arial");
            Font font = new Font(fm, 9, FontStyle.Bold, GraphicsUnit.Point);
            SolidBrush sb = new SolidBrush(Color.White);

            g.DrawString(txt, font, sb, new PointF(x, y));
            g.Dispose();
            sb.Dispose();
            return b;
        }
        public static Image DrawTextOnImage(Image b, string txt, int x, int y,int size)
        {
            if (b == null)
            {
                return null;
            }
            Bitmap tmp = new Bitmap(b);
            Graphics g = Graphics.FromImage(tmp);
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

            //Arial字体，大小为9，白色。
            FontFamily fm = new FontFamily("Arial");
            Font font = new Font(fm, size, FontStyle.Bold, GraphicsUnit.Point);
            SolidBrush sb = new SolidBrush(Color.White);
            
            g.DrawString(txt, font, sb, new PointF(x, y));
            g.Dispose();
            sb.Dispose();
            return tmp;
        }

        public static DialogResult ShowMessageBox(string message, string type)
        {
            return ShowMessageBox(message, Messages.Caption, MessageBoxButtons.OK);
        }

        public static DialogResult ShowMessageBox(string message, string type, MessageBoxButtons buttons)
        {
            MessageBoxIcon iconType = MessageBoxIcon.Information;
            switch (type)
            {
                case Messages.TitleError:
                    iconType = MessageBoxIcon.Error;
                    break;
                case Messages.TitleWarning:
                    iconType = MessageBoxIcon.Warning;
                    break;
                case Messages.TitleNotification:
                    iconType = MessageBoxIcon.Information;
                    break;
                default:
                    iconType = MessageBoxIcon.Information;
                    break;
            }
            return MessageBox.Show(message, Messages.Caption, buttons, iconType);
        }

        public static PrintPreviewDialog GetPrintPreviewDialogue()
        {
            if (printPreviewDialog == null)
            {
                printPreviewDialog = new PrintPreviewDialog();
                printPreviewDialog.ShowIcon = false;
            }
            return printPreviewDialog;
        }

        private static PrintPreviewDialog printPreviewDialog;

        public static string GetDisplayString(string content, Font font, int displayWidth)
        {
            string result = string.Empty;
            if(font != null && !string.IsNullOrEmpty(content)){
                int actualWidth = TextRenderer.MeasureText(content, font).Width;
                if (actualWidth <= displayWidth)
                {
                    result = content;
                }
                else
                {
                    string tail = "...";
                    int tailWidth = TextRenderer.MeasureText(tail, font).Width;
                    int expectedWidth = displayWidth - tailWidth;
                    for (int i = 0; i < content.Length; i++)
                    {
                        string subString = content.Substring(0, content.Length - i);
                        if (TextRenderer.MeasureText(subString, font).Width <= expectedWidth)
                        {
                            result = subString + tail;
                            break;
                        }
                    }
                }
            }
            return result;
        }

        public static bool IsInputTextValid(TextBox tb, string oldValue)
        {
            bool result = true;
            if (tb == null)
            {
                return result;
            }
            string textTrimEnd = tb.Text.TrimEnd();
            if (textTrimEnd.Length > 0)
            {
                var lastCharacter = textTrimEnd[textTrimEnd.Length - 1];
                Regex regex = new Regex("[_]");
                if (regex.IsMatch(lastCharacter.ToString()))
                {
                    if (string.IsNullOrEmpty(oldValue))
                    {
                        if (textTrimEnd.Length > 1)
                        {
                            tb.Text = textTrimEnd.Substring(0, textTrimEnd.Length - 1);
                        }
                        else
                        {
                            tb.Text = string.Empty;
                        }
                    }
                    else
                    {
                        tb.Text = oldValue;
                    }
                    tb.SelectionStart = tb.Text.Length;
                    result = false;
                    Utils.ShowMessageBox(Messages.InvalidCharacter, Messages.TitleError);
                }
            }
            return result;
        }

        public static bool IsInputTextValid(TextBox tb)
        {
            return IsInputTextValid(tb, null);
        }
    }

    

}
