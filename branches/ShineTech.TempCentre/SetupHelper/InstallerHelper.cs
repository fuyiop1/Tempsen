using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.IO;
using System.Windows.Forms;
namespace SetupHelper
{
    [RunInstaller(true)]
    public partial class InstallerHelper : System.Configuration.Install.Installer
    {
        public InstallerHelper()
        {
            InitializeComponent();
        }
        protected override void OnBeforeInstall(IDictionary savedState)
        {
            try
            {
                string filefolder = this.Context.Parameters["targetdir"];
                IntectDatabase(filefolder);
                throw new InstallException("rollback");
                base.OnBeforeInstall(savedState);

            }
            catch
            {
            }
        }
        public override void Install(IDictionary stateSaver)
        {
            base.Install(stateSaver);
            base.Rollback(stateSaver);
        }
        private void IntectDatabase(string path)
        {
            string filename0 = Path.Combine(path,"tempsen.db");
            string filename1= Path.Combine(path,"srcsafe.xml");
            if (File.Exists(filename0)||File.Exists(filename1))
            {
                DialogResult result = MessageBox.Show("There already exists a data base in current directory, would you like to remove it and install a new data base?", "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    if (File.Exists(filename0))
                        File.Delete(filename0);
                    if (File.Exists(filename1))
                        File.Delete(filename1);
                }
                
            }
        }
        protected override void OnBeforeUninstall(IDictionary savedState)
        {
            MessageBox.Show("Uninstallation of TempCentre software will not remove database covering data records and user information.", "Information", MessageBoxButtons.OK);
            base.OnBeforeUninstall(savedState);
        }
    }
}
