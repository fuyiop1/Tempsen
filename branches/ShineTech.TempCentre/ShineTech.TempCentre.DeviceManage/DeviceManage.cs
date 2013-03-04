using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using ShineTech.TempCentre.BusinessFacade;
using ZedGraph;
using System.Threading;
using ShineTech.TempCentre.AutoUpdate;
using ShineTech.TempCentre.Platform;
using System.Timers;
using Winforms.Components;
using Winforms.Components.ApplicationIdleData;
using System.Diagnostics;
using System.Runtime.InteropServices;
using ShineTech.TempCentre.Versions;

namespace ShineTech.TempCentre.DeviceManage
{
    public partial class DeviceManage : Form
    {
        private DeviceManageUI dm;
        private Form form;
        public static DeviceManage manage = null;
        public DeviceManage()
        {
            InitializeComponent();
            //UsbLibrary.Win32Usb.RegisterDeviceNotification(this.Handle,
        }
        protected DeviceManage(Form form)
        {
            InitializeComponent();
            RegisterHidNotification();
            this.UpdateStyles();
            this.form = form;
            this.helpToolStripMenuItem1.Click+=new EventHandler((sender,args)=>dm.SetHelp());
            if (Common.Versions == SoftwareVersions.S)
            {
                this.menuStrip1.Items.Remove(administrationToolStripMenuItem);
                pbAdministration.Visible = false;
                pnUserStatus.Visible = false;
                return;
            }
            SetLoggingStatus(true);
            this.pbLogin.MouseHover += new EventHandler((sender, args) =>
            {
                if (pnContainer.Controls.Count != 0)
                    //this.pbLogin.Image = Properties.Resources.menu_logoff_h;
                    this.pbLogin.Image = Utils.DrawTextOnImage(Properties.Resources.logout, "Logout", 18 , 7,9);
                else
                    //this.pbLogin.Image = Properties.Resources.logout;
                    this.pbLogin.Image = Utils.DrawTextOnImage(Properties.Resources.logout, "Login", 22 , 7,9);
            });
            this.pbLogin.MouseLeave += new EventHandler((sender, args) =>
            {
                if (pnContainer.Controls.Count != 0)
                    //this.pbLogin.Image = Properties.Resources.menu_log_off;
                    this.pbLogin.Image = Utils.DrawTextOnImage(Properties.Resources.login, "Logout", 18 , 7,9);
                else
                    //this.pbLogin.Image = Properties.Resources.menu_in;
                    this.pbLogin.Image = Utils.DrawTextOnImage(Properties.Resources.login, "Login", 22 , 7,9);
            });
           
        }
        private void administrationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Administration admin = new Administration();
            admin.ShowDialog(this);
            RefreshWorkSpaceViaAdmin();
        }
        private void DeviceManage_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
            form.Close();
            form.Dispose();
        }
        /*单例*/
        public static DeviceManage GetInstance(Form form)
        {
            if (manage == null)
                manage = new DeviceManage(form);
            return manage;
        }
        private void SetLoggingStatus(bool isLogin)
        {
            int len = pbLogin.Location.X;
            if (isLogin)
            {
                this.pbLogStatus.Image = Properties.Resources.menu_pesonnel;
                this.lbSessionStatus.Text = string.Format("{0}",Common.User.UserName);
                pbLogStatus.Location = new Point(len-lbSessionStatus.Width-pbLogStatus.Width,pbLogStatus.Location.Y);
                lbSessionStatus.Location = new Point(pbLogStatus.Location.X+pbLogStatus.Width,lbSessionStatus.Location.Y);
                //this.pbLogin.Image = Properties.Resources.menu_log_off;
                this.pbLogin.Image = Utils.DrawTextOnImage(Properties.Resources.login, "Logout", 18 , 7,9);
                this.pbLogStatus.Visible = true;
            }
            else
            {
                this.pbLogStatus.Image = Properties.Resources.menu_pesonnel;
                this.lbSessionStatus.Text = string.Empty;
                //this.pbLogin.Image = Properties.Resources.menu_in;
                this.pbLogin.Image = Utils.DrawTextOnImage(Properties.Resources.login, "Login", 22 , 7,9);
                this.pbLogStatus.Visible = false;
            }
        }
        //System.Timers.Timer _CheckTimer;
        private void DeviceManage_Load(object sender, EventArgs e)
        {
            aboutTempCentreToolStripMenuItem.Text = Common.Versions == Versions.SoftwareVersions.Pro ? "About TempCentre" : "About TempCentre Lite";
            this.Width = Screen.PrimaryScreen.WorkingArea.Width;
            this.Height = (Screen.PrimaryScreen.WorkingArea.Height);
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint, true);
            this.dm = new DeviceManageUI(this);
            dm.ViewManager.ShowAdminForm += new EventHandler(ShowAdmin);
            dm.DataManager.ShowAdminForm += new EventHandler(ShowAdmin);
            OnDeviceChange += new EventHandler(dm.ViewManager.DisconnectDeviceOnRemove);
            if (Platform.TrialValidation.IsProductKeyExist())
            {
                activeTempCentreToolStripMenuItem.Visible = true;
            }
            else
            {
                activeTempCentreToolStripMenuItem.Visible = false;
            }
            if (Common.Versions == SoftwareVersions.Pro)
            {
                if (!Platform.TrialValidation.IsValidated())
                {
                    TrialValidation trial = new TrialValidation();
                    trial.ShowDialog();
                    if (!trial.Validated)
                    {
                        this.Close();
                    }
                    else
                    {
                        trial.Close();
                        trial.Dispose();
                    }
                }
                InitIdle();
            }
            else
            {
                if (activeTempCentreToolStripMenuItem != null)
                {
                    activeTempCentreToolStripMenuItem.Visible = false;
                }
            }
        }
        private void ShowAdmin(object sender, EventArgs e)
        {

            Administration admin = new Administration();
            admin.SetTabPage();
            admin.ShowDialog(this);
            dm.ViewManager.InitViewManage();
            dm.DataManager.InitTpReportEditor();
            admin.Dispose();
            
        }
        private void Logout()
        {if (form != null)
            lock (form)
            {
                
                    form.Invoke(new Action(delegate()
                    {
                        form.Show();
                        applicationIdle.Stop();
                        applicationIdle.Dispose();
                        manage = null;
                        this.Dispose();
                    }));
            }
        }

        private void pbUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                List<Process> pros = Process.GetProcesses().Where(p => p.ProcessName.Split(new char[] { '.' })[0].ToLower() + ".exe" == "AutoUpdater.exe".ToLower()).ToList();
                if (pros.Count == 0)
                {
                    System.Diagnostics.Process.Start(System.Windows.Forms.Application.StartupPath + "\\AutoUpdater.exe");
                }
                else
                {
                    Program.HandleRunningInstance(pros.FirstOrDefault());
                }
            }
            catch
            {
                Utils.ShowMessageBox(Messages.MissingUpdateCompenents, Messages.TitleError);
            }
        }

        private void pbAdministration_Click(object sender, EventArgs e)
        {
            Administration admin = new Administration();
            admin.ShowDialog(this);
            RefreshWorkSpaceViaAdmin();
        }

        private void initTpAfterProfileChanged()
        {
            if (dm.ViewManager != null)
            {
                dm.ViewManager.InitViewManage();
                dm.ViewManager.InitDateTime();
            }
            if (dm.DataManager != null)
            {
                dm.DataManager.RefreshDataManagerWithAllConditions();
                //dm.DataManager.InitTpReportEditor();
            }
            if (dm.AuditTrail != null && Common.Versions == SoftwareVersions.Pro)
            {
                dm.AuditTrail.RefreshAuditTrail();
            }
        }
        private void RefreshWorkSpaceViaAdmin()
        {
            if (dm.ViewManager != null)
            {
                dm.ViewManager.InitViewManage();
            }
            if (dm.DataManager != null)
            {
                dm.DataManager.InitTpReportEditor();
            }
            if (dm.AuditTrail != null && Common.Versions == SoftwareVersions.Pro)
            {
                dm.AuditTrail.RefreshAuditTrail();
            }
        }

        private void pbLogin_Click(object sender, EventArgs e)
        {
            if (pnContainer.Controls.Count == 0)
            {
                form.Show();
                applicationIdle.Stop();
                applicationIdle.Dispose();
                manage = null;//单例资源释放
                this.Dispose();
            }
            else
            {
                switch(dm.FormClosing(sender, e))
                {
                    case DialogResult.OK:
                        if (DialogResult.Yes == Utils.ShowMessageBox(Messages.LogOut, Messages.TitleWarning, MessageBoxButtons.YesNo))
                        {
                            SignOut();
                        }
                        break;
                    case DialogResult.Cancel:
                        break;
                    default:
                        SignOut();
                        break;
                }
            }
        }
        private void SignOut()
        {

            this.pnContainer.Controls.Clear();
            this.pbAuditTrail.Enabled = this.pbDeviceManager.Enabled = this.pbDataBase.Enabled = false;
            menuStrip1.Enabled = this.pbExport.Enabled = pbMail.Enabled = pbPrint.Enabled = pbUpdate.Enabled = pbAdministration.Enabled = false;
            SetLoggingStatus(false);
            dm.AddLogOffLog();
            Common.GlobalProfile = null;
        }
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Options option = new Options();
            option.OptionChangeEvent += new EventHandler((a, args) =>
            {
                initTpAfterProfileChanged();
            });
            option.ShowDialog(this);
            option.Dispose();
        }
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);
            switch (dm.FormClosing(null, e))
            {
                case DialogResult.Cancel:
                    e.Cancel = true;
                    break;
                default:
                    e.Cancel = false;
                    break;
            }
        }
        #region user session
        private void InitIdle()
        {
            if (dm != null&&Common.Policy.InactivityTime!=0)
            {
                applicationIdle.IdleTime = new TimeSpan(0, Common.Policy.InactivityTime, 0);
                if (!applicationIdle.IsRunning)
                    applicationIdle.Start();
                applicationIdle.IdleAsync += new EventHandler(applicationIdle_IdleAsync);
            }
        }
        void applicationIdle_IdleAsync(object sender, EventArgs e)
        {
            BeginInvoke(new MethodInvoker(
                delegate() { applicationIdle_Idle(sender, e); })
                );
        }

        void applicationIdle_Idle(object sender, EventArgs e)
        {
            this.Logout();
        }
        #endregion
        #region 设备拔出消息

        /// <summary>
        /// This event will be triggered when a device is pluged into your usb port on
        /// the computer. And it is completly enumerated by windows and ready for use.
        /// </summary>
        private bool isCopy = false;
        public event EventHandler OnDeviceChange;
        protected override void WndProc(ref Message m)
        {
            ParseMessages(ref m);
            base.WndProc(ref m);
        }
        //protected override void DefWndProc(ref Message m)
        //{
        //    ParseMessages(ref m);
        //    base.DefWndProc(ref m);
        //}
        /// <summary>
        /// This method will filter the messages that are passed for usb device change messages only. 
        /// And parse them and take the appropriate action 
        /// </summary>
        /// <param name="m">a ref to Messages, The messages that are thrown by windows to the application.</param>
        /// <example> This sample shows how to implement this method in your form.
        /// <code> 
        ///</code>
        ///</example>
        private void ParseMessages(ref Message m)
        {
            if (m.Msg == UsbLibrary.Win32Usb.WM_DEVICECHANGE)	// we got a device change message! A USB device was inserted or removed
            {
                switch (m.WParam.ToInt32())
                {
                    case UsbLibrary.Win32Usb.DEVICE_REMOVECOMPLETE:
                        if (OnDeviceChange != null)
                        {
                            OnDeviceChange(this, new EventArgs());
                            isCopy = false;
                        }
                        break;
                    case UsbLibrary.Win32Usb.DEVICE_ARRIVAL:
                        if (!isCopy)
                            isCopy = true;
                        break;
                }
            }
        }
        private void RegisterHidNotification()
        {
            UsbLibrary.Win32Usb.DeviceBroadcastInterface dbi = new UsbLibrary.Win32Usb.DeviceBroadcastInterface();
            int size = Marshal.SizeOf(dbi);
            dbi.Size = size;
            dbi.DeviceType = 0x05;
            dbi.Reserved = 0;
            dbi.ClassGuid = UsbLibrary.Win32Usb.HIDGuid;
            dbi.Name = "tempsenCentre";
            //IntPtr buffer = Marshal.AllocHGlobal(size);
            //Marshal.StructureToPtr(dbi, buffer, true);
            IntPtr r = UsbLibrary.Win32Usb.RegisterDeviceNotification(this.Handle, dbi, 0);
        }
        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                List<Process> pros = Process.GetProcesses().Where(p => p.ProcessName.Split(new char[] { '.' })[0].ToLower() + ".exe" == "AutoUpdater.exe".ToLower()).ToList();
                if (pros.Count == 0)
                    System.Diagnostics.Process.Start(System.Windows.Forms.Application.StartupPath + "\\AutoUpdater.exe");
                else
                {
                    Program.HandleRunningInstance(pros.FirstOrDefault());
                }
            }
            catch
            {
                Utils.ShowMessageBox(Messages.MissingUpdateCompenents, Messages.TitleError);
            }
        }
        #endregion

        private void aboutTempCentreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutTempCentre about = new AboutTempCentre();
            about.ShowDialog();
            about.Dispose();
        }

        private void activeTempCentreToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ActiveTempCentre atForm = new ActiveTempCentre();
            atForm.ShowDialog();
            activeTempCentreToolStripMenuItem.Visible = !atForm.Validated;
        }

       
    }
}
