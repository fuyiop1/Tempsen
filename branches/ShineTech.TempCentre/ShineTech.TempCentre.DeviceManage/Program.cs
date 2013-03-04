using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ShineTech.TempCentre.BusinessFacade;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using ShineTech.TempCentre.Versions;
using System.Threading;

namespace ShineTech.TempCentre.DeviceManage
{
    static class Program
    {
        private static SoftwareVersions _componentVersion = SoftwareVersion.Version;
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (_componentVersion != SoftwareVersion.VersionForAuthentication || _componentVersion != Common.Versions)
            {
                return;
            }
            Application.ThreadException += new ThreadExceptionEventHandler(Application_ThreadException);
            //Application.Run(new TrialValidation());
            Process instance = RunningInstance();
            if (instance == null)
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                if (_componentVersion != SoftwareVersions.S)
                    Application.Run(new Login());
                else
                    Application.Run(DeviceManage.GetInstance(new Login()));
                //Application.Run(new UserWizard());
            }
            else
                HandleRunningInstance(instance);
        }

        static void Application_ThreadException(object sender, ThreadExceptionEventArgs e)
        {
            MessageBox.Show(e.Exception.Message,"Exception",MessageBoxButtons.OK,MessageBoxIcon.Error);
        }
        private static Process RunningInstance()
        {
            Process current = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(current.ProcessName);
            //遍历与当前进程名称相同的进程列表  
            foreach (Process process in processes)
            {
                //如果实例已经存在则忽略当前进程  
                if (process.Id != current.Id)
                {
                    //保证要打开的进程同已经存在的进程来自同一文件路径
                    if (Assembly.GetExecutingAssembly().Location.Replace("/", "\\") == current.MainModule.FileName)
                    {
                        //返回已经存在的进程
                        return process;

                    }
                }
            }
            return null;
        }
        public static void HandleRunningInstance(Process instance)
        {
            ShowWindowAsync(instance.MainWindowHandle, 1);  //调用api函数，正常显示窗口
            SetForegroundWindow(instance.MainWindowHandle); //将窗口放置最前端
        }
        [DllImport("User32.dll")]
        private static extern bool ShowWindowAsync(System.IntPtr hWnd, int cmdShow);
        [DllImport("User32.dll")]
        private static extern bool SetForegroundWindow(System.IntPtr hWnd);

    }
}
