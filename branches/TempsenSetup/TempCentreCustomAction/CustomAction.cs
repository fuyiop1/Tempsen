using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Deployment.WindowsInstaller;
using System.IO;
using System.Windows.Forms;
using Microsoft.Win32;

namespace TempCentreCustomAction
{
    public class CustomActions
    {
        [CustomAction]
        public static ActionResult DoSearchLastSetUpDir(Session session)
        {
            //System.Diagnostics.Debugger.Break();
            try
            {
                string path = "SOFTWARE\\" + session["Manufacturer"] + "\\TempCentre";
                string version = DetectLatestInstallInformation(path, "SoftType");
                string installdir = DetectLatestInstallInformation(path, "FileFolder");
                if (!string.IsNullOrEmpty(version) && !string.IsNullOrEmpty(installdir))
                {
                    string filename0 = Path.Combine(installdir, "TempCentre.exe");
                    if (File.Exists(filename0))
                    {
                        if (Convert.ToInt32(version) > Convert.ToInt32(session["SoftType"]))
                        {
                            MessageBox.Show("You have already set a pro version in "+installdir,"Information",MessageBoxButtons.OK);
                            return ActionResult.Failure;
                        }
                    }
                }
            }
            catch
            {
                return ActionResult.Failure;
            }
            return ActionResult.Success;
        }
        [CustomAction]
        public static ActionResult DoSearchDataBase(Session session)
        {
            //System.Diagnostics.Debugger.Break();
            string path = session["INSTALLLOCATION"];
            if (SearchLastInfo(session) > -1)
            {
                if (!string.IsNullOrEmpty(path))
                {
                    IntectDatabase(path,session);
                }
                SetRegistry(session["Manufacturer"], "TempCentre", "FileFolder", path, "SoftType", session["SoftType"]);
                return ActionResult.Success;
            }
            else
                return ActionResult.Failure;
        }
        private static void IntectDatabase(string path, Session session)
        {
            string filename0 = Path.Combine(path, "tempsen.db");
            string filename1 = Path.Combine(path, "srcsafe.xml");
            if (File.Exists(filename0) || File.Exists(filename1))
            {
                string title = session["SoftType"] == "1" ? "TempCentre" : "TempCentre Lite";
                DialogResult result = MessageBox.Show("There already exists a data base in current directory, would you like to remove it and install a new data base? Select \"Yes\" to delete existing data base and install a new one, select \"No\" to continue to use the existing data base.",title, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (File.Exists(filename0))
                        File.Delete(filename0);
                    if (File.Exists(filename1))
                        File.Delete(filename1);
                }
            }
        }
        private static void SetRegistry(string Manufacturer, string ProductName,string name,string value,string name1,string value1)
        {

            RegistryKey rk = Registry.LocalMachine;
            string path = "SOFTWARE\\" + Manufacturer + "\\" + ProductName;
            if (!IsExist(path))
            {
                rk.CreateSubKey(path);
            }
            SetRegistryVale(path, name, value);
            SetRegistryVale(path, name1, value1);
        }
        private static void SetRegistryVale(string path,string name,string value)
        {
            Registry.LocalMachine.OpenSubKey(path, true).SetValue(name, value);
        }
        private static bool IsExist(string Manufacturer, string ProductName)
        {
            bool result = false;
            RegistryKey rk = Registry.LocalMachine;
            string path = "SOFTWARE\\" + Manufacturer + "\\" + ProductName;
            if (rk.OpenSubKey(path) == null)
            {
                result = false;
            }
            else
                result = true;
            return result;
        }
        private static bool IsExist(string path)
        {
            bool result = false;
            RegistryKey rk = Registry.LocalMachine;
            if (rk.OpenSubKey(path) == null)
            {
                result = false;
            }
            else
                result = true;
            return result;
        }
        private static string DetectLatestInstallInformation(string path, string key)
        {
            string softversion = string.Empty;
            if (IsExist(path))
            {
                softversion = Registry.LocalMachine.OpenSubKey(path).GetValue(key) == null ? "" : Registry.LocalMachine.OpenSubKey(path).GetValue(key).ToString();
            }
            return softversion;
        }
        private static int  SearchLastInfo(Session session)
        {
            try
            {
                string path = "SOFTWARE\\" + session["Manufacturer"] + "\\TempCentre" ;
                string version = DetectLatestInstallInformation(path, "SoftType");
                string installdir = DetectLatestInstallInformation(path, "FileFolder");
                if (!string.IsNullOrEmpty(version) && !string.IsNullOrEmpty(installdir))
                {
                    string filename0 = Path.Combine(installdir, "TempCentre.exe");
                    if (File.Exists(filename0))
                    {
                        if (Convert.ToInt32(version) > Convert.ToInt32(session["SoftType"]))
                        {
                            MessageBox.Show("You have already set a pro version in " + installdir, "Information", MessageBoxButtons.OK);
                            
                            return -1;
                        }
                    }
                }
                return 0;
            }
            catch
            {
                return -1;
            }
        }
        [CustomAction]
        public static ActionResult RemoveOldVersion(Session session)
        {
            //System.Diagnostics.Debugger.Break();
            string title = session["SoftType"] == "1" ? "TempCentre" : "TempCentre Lite";
            DialogResult r = MessageBox.Show("The Standard Version has already been Setuped.Are you sure you want to set up Pro Version?",title,MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (r==DialogResult.Yes)
            {
                RegistryKey rk = Registry.LocalMachine;
                string path = "SOFTWARE\\" + session["Manufacturer"] + "\\TempCentre";
                if (!IsExist(path))
                {
                    rk.CreateSubKey(path);
                }
                SetRegistryVale(path, "PREVERSION", session["PREVERSION"]);
                return ActionResult.Success;
            }
            else
                return ActionResult.Failure;
        }
        [CustomAction]
        public static ActionResult FindNewestVersion(Session session)
        {
            //System.Diagnostics.Debugger.Break();
            string title = session["SoftType"] == "1" ? "TempCentre" : "TempCentre Lite";
            DialogResult r = MessageBox.Show("TempCentre has been installed in current computer, installation of standard version is already cancelled.", title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return ActionResult.Failure;
        }
        [CustomAction]
        public static ActionResult UninstallSoft(Session session)
        {
            //System.Diagnostics.Debugger.Break();
            string path = "SOFTWARE\\" + session["Manufacturer"] + "\\TempCentre";
            string PREVERSION = DetectLatestInstallInformation(path, "PREVERSION");
            if (PREVERSION == "0" || string.IsNullOrEmpty(PREVERSION))
            {
                string title = session["SoftType"] == "1" ? "TempCentre" : "TempCentre Lite";
                MessageBox.Show("Uninstallation of TempCentre software will not remove database covering data records and user information.", title, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                RegistryKey rk = Registry.LocalMachine;
                if (!IsExist(path))
                {
                    rk.CreateSubKey(path);
                }
                SetRegistryVale(path, "PREVERSION", "0");
            }
            return ActionResult.Success;
        }
        /// <summary>
        /// 记录未验证日期
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        [CustomAction]
        public static ActionResult SkipValidation(Session session)
        {
            //System.Diagnostics.Debugger.Break();
            string message = "You choose to skip entering CD-KEY, then you will have a 30-day trial period, if you want to continue to use TempCentre software after trial period expired, please purchase CD-KEY from your resellers and click Help in menu bar, navigate to Active TempCentre, enter the CD-KEY in the message box popped out.";
            if (MessageBox.Show(message, "Notification", MessageBoxButtons.OK) == DialogResult.OK)
            {
                string path = "SOFTWARE\\" + session["Manufacturer"] + "\\TempCentre";
                SetRegistryVale(path, "UnValdationDate", DateTime.UtcNow.ToString("yyyy-MM-dd"));
                return ActionResult.Success;
            }
            else
            {
                return ActionResult.Failure;
            }
            
        }
    }
}
