using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Xml.Linq;
using ShineTech.TempCentre.Versions;
namespace ShineTech.TempCentre.DeviceManage
{
    public partial class AboutTempCentre : Form
    {
        public AboutTempCentre()
        {
            InitializeComponent();
            GetSoftVersionAndType();
        }
        private void GetSoftVersionAndType()
        {
            lbType.Text = SoftwareVersion.Version == SoftwareVersions.Pro ? "TempCentre" : "TempCentre Lite";
            this.Text = string.Format("About {0}", lbType.Text);
            //FileVersionInfo myFileVersion = FileVersionInfo.GetVersionInfo("TempCentre.exe");
            //lbVersion.Text = string.Format("Version: {0}.{1}.{2}.{3}", myFileVersion.FileMajorPart, myFileVersion.FileMinorPart, myFileVersion.FileBuildPart, myFileVersion.FilePrivatePart);
            GetFileVersion();
        }
        private void GetFileVersion()
        {
            string localXmlFile = string.Concat(Application.StartupPath, "\\UpdateList.xml");
            var q = from c in XElement.Load(localXmlFile).Elements("Application")
                    select c.Element("Version").Value;
            if (q != null && q.Count() > 0)
            {
                string version = q.First();
                lbVersion.Text = string.Format("Version: {0}", version);
            }
        }
    }
}
