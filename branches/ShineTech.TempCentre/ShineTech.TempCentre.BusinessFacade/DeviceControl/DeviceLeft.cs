using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.Platform;
namespace ShineTech.TempCentre.BusinessFacade
{
    public partial class DeviceLeft : UserControl
    {
        private SuperDevice tag;

        public SuperDevice Tag
        {
            get { return tag; }
        }
        private bool _IsConnected;

        public bool IsConnected
        {
            get { return _IsConnected; }
            set { _IsConnected = value; }
        }
        public DeviceLeft()
        {
            InitializeComponent();
            tag = ObjectManage.GetDeviceInstance(DeviceType.ITAGSingleUse);
            this.InitEvents();
        }
        #region method
        public event EventHandler ConnectEvent;
        public event EventHandler ProgressEvent;
        public bool ConnectDevice()
        {
            try
            {
                if (cbSingleUse.Checked)
                {
                    if (tag == null)
                        tag = ObjectManage.GetDeviceInstance(DeviceType.ITAGSingleUse);
                    //ProgressEvent(null, null);
                    return _IsConnected=tag.Connect((int)DeviceType.ITAGSingleUse);
                }
                else
                    Utils.ShowMessageBox(Messages.ConnectWithNoDeviceSelected, Messages.TitleError);
            }
            catch
            {
                return false;
            }
            return false;
        }
        public bool AutoConnect()
        {
            try
            {
                if (tag == null)
                    tag = ObjectManage.GetDeviceInstance(DeviceType.ITAGSingleUse);
                //ProgressEvent(null, null);
                _IsConnected=this.cbSingleUse.Checked = tag.Auto((int)DeviceType.ITAGSingleUse);
            }
            catch { return false; }
            return this.cbSingleUse.Checked;
        }
        public void InitEvents()
        {
            this.btnConnect.Click += new EventHandler((a, b) =>
            {
                if (this.ConnectDevice())
                {
                    //初始化值
                    if (tag is ITAGSingleUse)
                    {
                        ((ITAGSingleUse)tag).Summary();
                    }
                    this.InitStatus();
                    ConnectEvent(tag, null);
                }
            });
            this.btnAuto.Click += new EventHandler((a, b) =>
            {
                if (this.AutoConnect())
                {
                    //初始化值
                    if (tag is ITAGSingleUse)
                    {
                        ((ITAGSingleUse)tag).Summary();
                    }
                    this.InitStatus();
                    ConnectEvent(tag, null);
                }
                else
                    Utils.ShowMessageBox(Messages.ConnectDeviceFailed, Messages.TitleError);
            });
        }
        private void InitStatus()
        {
            this.lbMemory.Text = string.Format("Memory: {0}%",tag.OtherInfo[2]);
            this.lBattery.Text = string.Format("Battery: {0}%", tag.OtherInfo[1]);
            this.lStatus.Text = string.Format("Current Status: Stop");
        }
        
        #endregion
    }
}
