using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;
using System.Security.Permissions;

namespace ShineTech.TempCentre.Platform
{
    public class TrialValidation
    {
        [RegistryPermissionAttribute(SecurityAction.Demand, Read = @"HKEY_LOCAL_MACHINE\SOFTWARE\TempSen Electronics Company\TempCentre")]
        public static bool IsValidated()
        {
            bool result = true;
            string path = "SOFTWARE\\TempSen Electronics Company\\TempCentre";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(path, true);
            if (key != null)
            {
                var date =key.GetValue("UnValdationDate");
                if (date!=null)
                {
                    var installedDate = Convert.ToDateTime(date);
                    if ((DateTime.UtcNow - installedDate).TotalDays >= 30)
                    {
                        result = false;
                    }
                }
            }
            return result;
        }
        public static bool IsProductKeyExist()
        {
            bool result = false;
            string path = "SOFTWARE\\TempSen Electronics Company\\TempCentre";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(path, true);
            if (key != null)
            {
                var date = key.GetValue("UnValdationDate");
                if (date != null)
                {
                    result = true;
                }
            }
            return result;
        }
        public static bool RemoveTrialKey()
        {
            bool result = false;
            string path = "SOFTWARE\\TempSen Electronics Company\\TempCentre";
            RegistryKey key = Registry.LocalMachine.OpenSubKey(path, true);
            if (key != null)
            {
                var date = key.GetValue("UnValdationDate");
                if (date != null)
                {
                    key.DeleteValue("UnValdationDate");
                    result = true;
                }
            }
            return result;
        }
    }
}
