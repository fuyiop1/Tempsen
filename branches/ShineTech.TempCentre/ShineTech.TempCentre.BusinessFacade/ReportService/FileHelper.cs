using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ShineTech.TempCentre.BusinessFacade
{
    public class FileHelper
    {
        public static bool DeleteTempFiles(string subfix)
        {
            bool result = true;
            string tempDirectoryPath = Path.Combine(System.Windows.Forms.Application.StartupPath, "temp");
            DirectoryInfo directoryInfo = new DirectoryInfo(tempDirectoryPath);
            if (directoryInfo.Exists)
            {
                string[] files = Directory.GetFiles(Path.Combine(System.Windows.Forms.Application.StartupPath, "temp"));
                try
                {
                    foreach (var item in files)
                    {
                        FileInfo file = new FileInfo(item);
                        if (file.Exists && item.EndsWith(subfix))
                        {
                            file.Delete();
                        }
                    }
                }
                catch (Exception)
                {
                    result = false;
                }
            }
            return result;
        }
    }
}
