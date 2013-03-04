/* ***********************************************
 * Description: AppUpdater
 * CreateBy wangfei 
 * CreateOn 8/30/2011 9:53:08 AM 
 * Email: wangf@shinetechchina.com
 * ***********************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Collections;

namespace ShineTech.TempCentre.AutoUpdate
{
    public class AppUpdater
    {
        #region 成员与字段属性
        private string _updaterUrl;
        private bool disposed = false;
        private IntPtr handle;
        private Component component = new Component();
        [System.Runtime.InteropServices.DllImport("Kernel32")]
        private extern static Boolean CloseHandle(IntPtr handle);


        public string UpdaterUrl
        {
            set { _updaterUrl = value; }
            get { return this._updaterUrl; }
        }
        #endregion

        /// <summary>
        /// AppUpdater构造函数
        /// </summary>
        private AppUpdater()
        {
            this.handle = handle;
        }
        public static AppUpdater GetInstance()
        {
            return new AppUpdater();
        }
        /// <summary>
        /// 检查更新文件
        /// </summary>
        /// <param name="serverXmlFile"></param>
        /// <param name="localXmlFile"></param>
        /// <param name="updateFileList"></param>
        /// <returns></returns>
        public int CheckForUpdate(string serverXmlFile, string localXmlFile, out Hashtable updateFileList)
        {
            updateFileList = new Hashtable();
            if (!File.Exists(localXmlFile) || !File.Exists(serverXmlFile))
            {
                return -1;
            }

            XmlFiles serverXmlFiles = new XmlFiles(serverXmlFile);
            XmlFiles localXmlFiles = new XmlFiles(localXmlFile);

            XmlNodeList newNodeList = serverXmlFiles.GetNodeList("AutoUpdater/Files");
            XmlNodeList oldNodeList = localXmlFiles.GetNodeList("AutoUpdater/Files");

            var newAppVersionNode = serverXmlFiles.GetElementsByTagName("Version");
            string newAppVersion = string.Empty;
            var oldAppVersionNode = localXmlFiles.GetElementsByTagName("Version");
            string oldAppVersion = string.Empty;
            if (newAppVersionNode != null && newAppVersionNode.Count > 0)
            {
                newAppVersion = newAppVersionNode[0].InnerText;
            }
            if (oldAppVersionNode != null && oldAppVersionNode.Count > 0)
            {
                oldAppVersion = oldAppVersionNode[0].InnerText;
            }


            int k = 0;
            for (int i = 0; i < newNodeList.Count; i++)
            {
                string[] fileList = new string[3];

                string newFileName = newNodeList.Item(i).Attributes["Name"].Value.Trim();
                string newVer = newNodeList.Item(i).Attributes["Ver"].Value.Trim();

                List<string> oldFileAl = new List<string>();
                for (int j = 0; j < oldNodeList.Count; j++)
                {
                    string oldFileName = oldNodeList.Item(j).Attributes["Name"].Value.Trim();
                    string oldVer = oldNodeList.Item(j).Attributes["Ver"].Value.Trim();

                    oldFileAl.Add(oldFileName);
                    oldFileAl.Add(oldVer);

                }
                int pos = oldFileAl.IndexOf(newFileName);
                if (pos == -1 && newAppVersion.CompareTo(oldAppVersion) > 0)
                {
                    fileList[0] = newFileName;
                    fileList[1] = newVer;
                    updateFileList.Add(k, fileList);
                    k++;
                }
                else if (pos > -1 && newVer.CompareTo(oldFileAl[pos + 1].ToString()) > 0)
                {
                    fileList[0] = newFileName;
                    fileList[1] = newVer;
                    updateFileList.Add(k, fileList);
                    k++;
                }

            }
            return k;
        }

        /// <summary>
        /// 检查更新文件
        /// </summary>
        /// <param name="serverXmlFile"></param>
        /// <param name="localXmlFile"></param>
        /// <param name="updateFileList"></param>
        /// <returns></returns>
        public int CheckForUpdate()
        {
            string localXmlFile = Application.StartupPath + "\\UpdateList.xml";
            if (!File.Exists(localXmlFile))
            {
                return -1;
            }

            XmlFiles updaterXmlFiles = new XmlFiles(localXmlFile);


            string tempUpdatePath = Environment.GetEnvironmentVariable("Temp") + "\\" + "_" + updaterXmlFiles.FindNode("//Application").Attributes["applicationId"].Value + "_" + "y" + "_" + "x" + "_" + "m" + "_" + "\\";
            this.UpdaterUrl = updaterXmlFiles.GetNodeValue("//Url") + "/UpdateList.xml";
            this.DownAutoUpdateFile(tempUpdatePath);

            string serverXmlFile = tempUpdatePath + "\\UpdateList.xml";
            if (!File.Exists(serverXmlFile))
            {
                return -1;
            }

            XmlFiles serverXmlFiles = new XmlFiles(serverXmlFile);
            XmlFiles localXmlFiles = new XmlFiles(localXmlFile);

            XmlNodeList newNodeList = serverXmlFiles.GetNodeList("AutoUpdater/Files");
            XmlNodeList oldNodeList = localXmlFiles.GetNodeList("AutoUpdater/Files");

            int k = 0;
            for (int i = 0; i < newNodeList.Count; i++)
            {
                string[] fileList = new string[3];

                string newFileName = newNodeList.Item(i).Attributes["Name"].Value.Trim();
                string newVer = newNodeList.Item(i).Attributes["Ver"].Value.Trim();

                List<string> oldFileAl = new List<string>();
                for (int j = 0; j < oldNodeList.Count; j++)
                {
                    string oldFileName = oldNodeList.Item(j).Attributes["Name"].Value.Trim();
                    string oldVer = oldNodeList.Item(j).Attributes["Ver"].Value.Trim();

                    oldFileAl.Add(oldFileName);
                    oldFileAl.Add(oldVer);

                }
                int pos = oldFileAl.IndexOf(newFileName);
                if (pos == -1)
                {
                    fileList[0] = newFileName;
                    fileList[1] = newVer;
                    k++;
                }
                else if (pos > -1 && newVer.CompareTo(oldFileAl[pos + 1].ToString()) > 0)
                {
                    fileList[0] = newFileName;
                    fileList[1] = newVer;
                    k++;
                }

            }
            return k;
        }
        /// <summary>
        /// 返回下载更新文件的临时目录
        /// </summary>
        /// <returns></returns>
        public void DownAutoUpdateFile(string downpath)
        {
            if (!System.IO.Directory.Exists(downpath))
                System.IO.Directory.CreateDirectory(downpath);
            string serverXmlFile = downpath + @"/UpdateList.xml";

            try
            {
                WebRequest req = WebRequest.Create(this.UpdaterUrl);
                WebResponse res = req.GetResponse();
                if (res.ContentLength > 0)
                {
                    try
                    {
                        WebClient wClient = new WebClient();
                        wClient.DownloadFile(this.UpdaterUrl, serverXmlFile);
                    }
                    catch
                    {
                        return;
                    }
                }
            }
            catch
            {
                return;
            }
            //return tempPath;
        }
    }
}
/**********************************
╭━━灬╮╭━━∞╮ .︵ 
┃⌒　⌒┃┃⌒　⌒┃ (の) 
┃┃　┃┃┃▂　▂┃╱︶ 
╰━━━〇〇━━━〇 
**********************************/