using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Services.Common;
using System.IO;
using System.Collections;
using System.Diagnostics;
using System.Net;
using System.Threading;
using ShineTech.TempCentre.Platform;

namespace ShineTech.TempCentre.AutoUpdate
{
    public partial class AutoUpdater : Form
    {
        private static ITracing _tracing = TracingManager.GetTracing(typeof(AutoUpdater));
        public AutoUpdater()
        {
            InitializeComponent();
            InitEvents();
        }
        private XmlFiles _UpdaterXmlFiles;
        private string _UpdateUri;
        private string _TempUpatePath;
        private int availableUpdate;
        private string _MainExe;
        private bool isRun=false;
        private void InitEvents()
        {
            this.Load += new EventHandler((sender, args) =>
            {
                FormLoad();
            });
            this.btnCancel.Click += new EventHandler((sender, args) =>
            {
                this.Close();
                Application.ExitThread();
                Application.Exit();
            });
            this.btnNext.Click += new EventHandler((sender, args) =>
            {
                if (availableUpdate > 0)
                {
                    if (IsMainAppRun())
                    {
                        Utils.ShowMessageBox(Messages.CloseSystemWhenUpdate, Messages.TitleWarning, MessageBoxButtons.OK);
                        return;
                    }
                     Thread thread = new System.Threading.Thread(new ThreadStart(DownLoadFiles));
                     thread.IsBackground = true;
                     thread.Start();
                }
                else
                {
                    Utils.ShowMessageBox(Messages.NoAvailableUpdate, Messages.TitleNotification);
                }
            });
            this.btnFinish.Click += new EventHandler((sender, args) =>
            {
                this.Close();
                this.Dispose();
                try
                {
                    CopyFiles(_TempUpatePath, System.Windows.Forms.Application.StartupPath);
                    System.IO.Directory.Delete(_TempUpatePath, true);
                    RunMainExe();
                }
                catch (Exception ex)
                {
                    Utils.ShowMessageBox(ex.Message.ToString(), Messages.TitleNotification);
                }
                if (true == this.isRun) Process.Start(_MainExe);
            });
        }
        private void FormLoad()
        {
            this.btnFinish.Visible = false;
            string localXmlFile = string.Concat(Application.StartupPath, "\\UpdateList.xml");
            string serverXmlFile = string.Empty;
            try
            {
                _UpdaterXmlFiles = new XmlFiles(localXmlFile);
            }
            catch(Exception e)
            {
                _tracing.Error(e,"the config xml is reading failed.");
                this.Close();
                return;
            }
            //获取更新服务器地址
            _UpdateUri = _UpdaterXmlFiles.GetNodeValue("//Url");
            AppUpdater appupdater = AppUpdater.GetInstance();
            appupdater.UpdaterUrl = _UpdateUri + "/UpdateList.xml";
            //与服务器连接获取配置文件
            try
            {
                _TempUpatePath = Environment.GetEnvironmentVariable("Temp") + "\\_" + _UpdaterXmlFiles.FindNode("//Application").Attributes["applicationId"].Value + "_y_x_m_\\";
                appupdater.DownAutoUpdateFile(_TempUpatePath);
            }
            catch(Exception e)
            {
                _tracing.Error(e, "Connect to server failed");
                Utils.ShowMessageBox(Messages.ConnectServerFailed, Messages.TitleNotification);
                this.Close();
                return;
            }
            //获取文件列表
            serverXmlFile = _TempUpatePath + "\\UpdateList.xml";
            if (!File.Exists(serverXmlFile))
            {
                return;
            }
            Hashtable htUpdateFile=new Hashtable ();
            availableUpdate = appupdater.CheckForUpdate(serverXmlFile, localXmlFile, out htUpdateFile);
            if (availableUpdate > 0)
            {
                for (int i = 0; i < htUpdateFile.Count; i++)
                {
                    string[] fileArray = (string[])htUpdateFile[i];
                    //lvUpdateList.Items.Add(new ListViewItem(fileArray));
                    lvUpdateList.Items.Add(new  ListViewItem(fileArray));
                }
            }
        }
        private void DownLoadFiles()
        {
            //this.Cursor = Cursors.WaitCursor;
            SetCursor(Cursors.WaitCursor);
            _MainExe = _UpdaterXmlFiles.GetNodeValue("//EntryPoint");
            Process[] allProcess = Process.GetProcesses();
            List<Process> pros = Process.GetProcesses().Where(p => p.ProcessName.Split(new char[] { '.' })[0].ToLower() + ".exe" == this._MainExe.ToLower()).ToList();
            if (pros != null && pros.Count > 0)
                pros.ForEach(p =>
                {
                    p.Threads.Cast<ProcessThread>().ToList().ForEach(v =>
                    {
                        v.Dispose();
                    });
                    p.Kill();
                    isRun = true;
                });
            WebClient wcClient = new WebClient();
            for (int i = 0; i < this.lvUpdateList.Items.Count; i++)
            {
                string UpdateFile = GetListViewText(i);
                string updateFileUrl = _UpdateUri + GetListViewText(i);
                long fileLength = 0;
                try
                {

                    WebRequest webReq = WebRequest.Create(updateFileUrl);
                    WebResponse webRes = webReq.GetResponse();
                    fileLength = webRes.ContentLength;
                    Stream srm = webRes.GetResponseStream();
                    SetStatusText("Downloading,Please wait a moment...");
                    SetProgressBarInitValue(0);
                    SetProgressBarValue((int)fileLength, true);
                    StreamReader srmReader = new StreamReader(srm);
                    byte[] bufferbyte = new byte[fileLength];
                    int allByte = (int)bufferbyte.Length;
                    int startByte = 0;
                    while (fileLength > 0)
                    {
                        Application.DoEvents();
                        int downByte = srm.Read(bufferbyte, startByte, allByte);
                        if (downByte == 0) { break; };
                        startByte += downByte;
                        allByte -= downByte;
                        SetProgressBarValue(downByte, false);
                        //this.progressDownload.Value += downByte;

                        float part = (float)startByte / 1024;
                        float total = (float)bufferbyte.Length / 1024;
                        //int percent = Convert.ToInt32((part / total) * 100);
                        int percent = Convert.ToInt32((part / total));
                        //this.lvUpdateList.Items[i].SubItems[2].Text = percent.ToString() + "%";
                        //SetListViewText(i,percent.ToString()+"%");
                        SetListViewText(i,string.Format("{0:P}",percent));

                    }

                    string tempPath = _TempUpatePath + UpdateFile;
                    CreateDirtory(tempPath);
                    FileStream fs = new FileStream(tempPath, FileMode.OpenOrCreate, FileAccess.Write);
                    fs.Write(bufferbyte, 0, bufferbyte.Length);
                    srm.Close();
                    srmReader.Close();
                    fs.Close();
                }
                catch (WebException ex)
                {
                    Utils.ShowMessageBox(Messages.DownLoadFailed + ex.Message.ToString(), Messages.TitleError);
                    _tracing.Error(ex, "Download failed" );
                }
            }
            SetStatusText("Download completed.");
            InvalidateControl();
            SetCursor(Cursors.Default);
        }
        //创建目录
        private void CreateDirtory(string path)
        {
            if (!File.Exists(path))
            {
                string[] dirArray = path.Split('\\');
                string temp = string.Empty;
                for (int i = 0; i < dirArray.Length - 1; i++)
                {
                    temp += dirArray[i].Trim() + "\\";
                    if (!Directory.Exists(temp))
                        Directory.CreateDirectory(temp);
                }
            }
        }
        private void CopyFiles(string sourcePath, string objPath)
        {
            if (!Directory.Exists(objPath))
            {
                Directory.CreateDirectory(objPath);
            }
            string[] files = Directory.GetFiles(sourcePath);
            for (int i = 0; i < files.Length; i++)
            {
                string[] childfile = files[i].Split('\\');
                File.Copy(files[i], objPath + @"\" + childfile[childfile.Length - 1], true);
            }
            string[] dirs = Directory.GetDirectories(sourcePath);
            for (int i = 0; i < dirs.Length; i++)
            {
                string[] childdir = dirs[i].Split('\\');
                CopyFiles(dirs[i], objPath + @"\" + childdir[childdir.Length - 1]);
            }
        }
        private string  GetListViewText(int i)
        {
            if (lvUpdateList.InvokeRequired)
            {
                return (string)lvUpdateList.Invoke(new Func<int,string>(GetListViewText), i);
            }
            else
            {
                return lvUpdateList.Items[i].Text.Trim();
            }
        }
        private void SetListViewText(int i,string text)
        {
            if (lvUpdateList.InvokeRequired)
            {
                lvUpdateList.Invoke(new Action<int, string>(SetListViewText), i, text);
            }
            else
            {
                lvUpdateList.Items[i].SubItems[2].Text = text;
            }
        }
        private void SetStatusText(string text)
        {
            if (lbStatus.InvokeRequired)
            {
                lbStatus.Invoke(new Action<string>(SetStatusText), text);
            }
            else
            {
                lbStatus.Text = text;
            }
        }
        private void SetProgressBarValue(int value,bool maxValue)
        {
            if (progressDownload.InvokeRequired)
            {
                progressDownload.Invoke(new Action<int,bool>(SetProgressBarValue), value, maxValue);
            }
            else
            {
                if (maxValue)
                    progressDownload.Maximum = value;
                else
                {
                    if ((progressDownload.Value + value) > progressDownload.Maximum)
                        progressDownload.Value = progressDownload.Maximum;
                    else
                        progressDownload.Value += value;

                }
            }
        }
        private void SetProgressBarInitValue(int value)
        {
            if (progressDownload.InvokeRequired)
            {
                progressDownload.Invoke(new Action<int>(SetProgressBarInitValue), value);
            }
            else
            {
                    progressDownload.Value = value;
            }
        }
        private void SetCursor(Cursor c)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action<Cursor>(SetCursor), c);
            }
            else
            {
                this.Cursor = c;
            }
        }
        private void SetControlVisible(Control control, bool isVisible)
        {
            if (control.InvokeRequired)
                control.Invoke(new Action<Control, bool>(SetControlVisible), control, isVisible);
            else
                control.Visible = isVisible;
        }
        private void SetButtonLocation(Point p)
        {
            if (btnFinish.InvokeRequired)
                btnFinish.Invoke(new Action<Point>(SetButtonLocation), p);
            else
                btnFinish.Location = p;
        }
        private void InvalidateControl()
        {
            SetControlVisible(btnCancel, false);
            SetControlVisible(btnNext, false);
            SetControlVisible(btnFinish, true);
            //btnNext.Visible = false;
            //btnFinish.Visible = true;
            //btnCancel.Visible = false;
            SetButtonLocation(btnCancel.Location);
        }
        private bool IsMainAppRun()
        {
            string mainAppExe = this._UpdaterXmlFiles.GetNodeValue("//EntryPoint");
            bool isRun = false;
            List<Process> pros = Process.GetProcesses().Where(p => p.ProcessName.Split(new char[] { '.' })[0].ToLower() + ".exe" == mainAppExe.ToLower()).ToList();
            if (pros != null && pros.Count > 0)
                isRun = true;
            return isRun;
        }
        private void RunMainExe()
        {
            try
            {
                System.Diagnostics.Process.Start(System.Windows.Forms.Application.StartupPath + "\\" + _MainExe);
            }
            catch
            {
            }
        }
    }
}
