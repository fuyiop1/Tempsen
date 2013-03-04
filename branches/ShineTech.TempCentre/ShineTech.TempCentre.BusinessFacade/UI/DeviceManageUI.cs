using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.DAL;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Collections;
using System.Data;
using System.Threading;
using System.ComponentModel;
using System.Drawing;
using ShineTech.TempCentre.Platform;

namespace ShineTech.TempCentre.BusinessFacade
{
    public partial class DeviceManageUI
    {
        #region private variable
        private Label lbUserinfo;
        private System.Windows.Forms.Panel pnContainer;
        private System.Windows.Forms.Panel pnTool;
        private System.Windows.Forms.Panel pnMenu;
        
        private System.Windows.Forms.PictureBox pbDeviceManager;
        private System.Windows.Forms.PictureBox pbDataBase;
        private System.Windows.Forms.PictureBox pbAuditTrail;
        private System.Windows.Forms.PictureBox pbPrint;
        private System.Windows.Forms.PictureBox pbMail;
        private System.Windows.Forms.PictureBox pbExport;
        private System.Windows.Forms.PictureBox pbUpdate;
        private System.Windows.Forms.PictureBox pbAdministration;
        private Panel pnUserStatus;
        private  ToolStripMenuItem saveToolStripMenuItem;
        private ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem emailFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printPreviewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tpsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pDFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem excelToolStripMenuItem;
        private MenuStrip menuStrip1;
        #endregion
        private Form form;
        private ViewManager viewManager;
        private AuditTrail auditTrail;
        private DataManagerUC dataManager;

        public DataManagerUC DataManager
        {
            get { return dataManager; }
            set { dataManager = value; }
        }
        public ViewManager ViewManager
        {
            get { return viewManager; }
            set { viewManager = value; }
        }
        public AuditTrail AuditTrail
        {
            get { return auditTrail; }
            set { auditTrail = value; }
        }
        private FullScreen _FullScreen;
        private Size _PnContainerSize;
        private Size _ViewSize;
        private bool isMinimum = false;
        private bool isFullScreen = false;
        #region .ctor
        public DeviceManageUI()
        {
        }
        public DeviceManageUI(Form form)
        {
            this.ConstructForms(form);
            this.InitPanel();
            this.InitEvents();
            this.InitUserRights();
            this.InitContextMenu();
            this.SetOrigionPic();
            _PnContainerSize=pnContainer.Size;
            this.form.Text = Common.FormTitle;
        }
        private void ConstructForms(Form  form) 
        {
            this.form = form;
            _FullScreen = new FullScreen(form);
            lbUserinfo = form.Controls.Find("lbUserinfo", true)[0] as Label;
            pnContainer = form.Controls.Find("pnContainer", true)[0] as Panel;
            pnTool = form.Controls.Find("pnTool", true)[0] as Panel;
            pnMenu = form.Controls.Find("pnMenu", true)[0] as Panel;
            saveToolStripMenuItem = (ToolStripMenuItem)((MenuStrip)form.Controls.Find("menuStrip1", true)[0]).Items.Find("saveToolStripMenuItem", true)[0];
            openToolStripMenuItem = (ToolStripMenuItem)((MenuStrip)form.Controls.Find("menuStrip1", true)[0]).Items.Find("openToolStripMenuItem", true)[0];
            menuStrip1 = form.Controls.Find("menuStrip1", true)[0] as  MenuStrip ;
            pnUserStatus = form.Controls.Find("pnUserStatus", true)[0] as Panel;
            pbDeviceManager = form.Controls.Find("pbDeviceManager", true)[0] as PictureBox;
            pbDataBase = form.Controls.Find("pbDataBase", true)[0] as PictureBox;
            pbAuditTrail = form.Controls.Find("pbAuditTrail", true)[0] as PictureBox;
            pbPrint = form.Controls.Find("pbPrint", true)[0] as PictureBox;
            pbMail = form.Controls.Find("pbMail", true)[0] as PictureBox;
            pbExport = form.Controls.Find("pbExport", true)[0] as PictureBox;
            pbUpdate = form.Controls.Find("pbUpdate", true)[0] as PictureBox;
            pbAdministration = form.Controls.Find("pbAdministration", true)[0] as PictureBox;
            exportToolStripMenuItem = (ToolStripMenuItem)((MenuStrip)form.Controls.Find("menuStrip1", true)[0]).Items.Find("exportToolStripMenuItem", true)[0];
            emailFilesToolStripMenuItem = (ToolStripMenuItem)((MenuStrip)form.Controls.Find("menuStrip1", true)[0]).Items.Find("emailFilesToolStripMenuItem", true)[0];
            printFileToolStripMenuItem = (ToolStripMenuItem)((MenuStrip)form.Controls.Find("menuStrip1", true)[0]).Items.Find("printFileToolStripMenuItem", true)[0];
            printPreviewToolStripMenuItem = (ToolStripMenuItem)((MenuStrip)form.Controls.Find("menuStrip1", true)[0]).Items.Find("printPreviewToolStripMenuItem", true)[0];
            exitToolStripMenuItem = (ToolStripMenuItem)((MenuStrip)form.Controls.Find("menuStrip1", true)[0]).Items.Find("exitToolStripMenuItem", true)[0];
            tpsToolStripMenuItem = (ToolStripMenuItem)((MenuStrip)form.Controls.Find("menuStrip1", true)[0]).Items.Find("tpsToolStripMenuItem", true)[0];
            pDFToolStripMenuItem = (ToolStripMenuItem)((MenuStrip)form.Controls.Find("menuStrip1", true)[0]).Items.Find("pDFToolStripMenuItem", true)[0];
            excelToolStripMenuItem = (ToolStripMenuItem)((MenuStrip)form.Controls.Find("menuStrip1", true)[0]).Items.Find("excelToolStripMenuItem", true)[0];
        }
        #endregion
        private void InitPanel()
        {
            if (viewManager == null)
            {

                
                viewManager = new ViewManager();
                viewManager.Size = new Size(pnContainer.Width, pnContainer.Height);
                //viewManager.Dock = DockStyle.Fill;
            }
            if (auditTrail == null)
            {
                auditTrail = new AuditTrail();
                auditTrail.Dock = DockStyle.Fill;
            }
            if (dataManager == null)
                dataManager = new DataManagerUC();
            AddDeviceManagerPanel();//默认显示device manager
            //订阅事件
            this.viewManager.SaveRecordEvent += new EventHandler((a, b) => this.dataManager.InitHistoryData());
        }

        private void InitEvents()
        {
            this.pbDeviceManager.Click += new EventHandler((a, b) =>
            {
                try
                {
                    //先判断容器中有没有device manager
                    if ( pnContainer.Controls.Contains(viewManager))
                        return;
                    if (pnContainer.Controls.Contains(dataManager))
                    {
                        if (DialogResult.Cancel == dataManager.DeviceManagerExitDialog())
                        {
                            return;
                        }
                    }
                        
                    AddDeviceManagerPanel();
                    SetDeviceClickPic((PictureBox)a);
                }
                catch { }
            });
            this.pbAuditTrail.Click += new EventHandler((a, b) =>
            {
                try
                {
                    //先判断容器中有没有audit Trail
                    if (pnContainer.Controls.Contains(auditTrail))
                        return;
                    if (pnContainer.Controls.Contains(dataManager))
                    {
                        if (DialogResult.Cancel == dataManager.DeviceManagerExitDialog())
                        {
                            return;
                        }
                    }
                    if (pnContainer.Controls.Contains(viewManager))
                    {
                        if (DialogResult.Cancel == viewManager.DeviceManagerExitDialog())
                        {
                            return;
                        }
                    }
                    auditTrail.RefreshAuditTrail();
                    AddAuditTrail();
                    SetDeviceClickPic((PictureBox)a);
                }
                catch { }
            });
            this.pbDataBase.Click += new EventHandler(ShowDataBase);
            //保存tps
            this.saveToolStripMenuItem.Click += new EventHandler((sender, args) =>
            {

                if (pnContainer.Contains(viewManager))
                    //viewManager.ExportTps("tps");
                    viewManager.Save();
                else
                    //dataManager.ExportTps();
                    dataManager.Save();
            });
            this.openToolStripMenuItem.Click += new EventHandler(OpenTPSFile);
            //关闭窗口
            this.exitToolStripMenuItem.Click += new EventHandler((sender, args) => this.form.Close());
            //导出files
            this.tpsToolStripMenuItem.Click+=new EventHandler((sender,args)=>ExportTPS());
            this.pDFToolStripMenuItem.Click += new EventHandler((sender, args) => ExportPDF());
            this.excelToolStripMenuItem.Click += new EventHandler((sender, args) => ExportExcel());
            this.emailFilesToolStripMenuItem.Click += new EventHandler((sender, args) => Email());
            this.printFileToolStripMenuItem.Click += new EventHandler((sender, args) => Print(false));
            this.printPreviewToolStripMenuItem.Click += new EventHandler((sender, args) => Print(true));
            this.pbMail.Click += new EventHandler((sender, args) =>Email());
            //this.pbPrint.Click += new EventHandler((sender, args) =>Print());
            this.viewManager.PbMax.Click += new EventHandler((a, b) =>
            {
                if (form.FormBorderStyle == FormBorderStyle.Sizable)
                {
                    ShowFullScreen(true);
                }
                else
                {
                    UndoShowFullScreen(true);
                }
            });
            this.dataManager.PbMax.Click += new EventHandler((a, b) =>
            {
                if (form.FormBorderStyle == FormBorderStyle.Sizable)
                {
                    ShowFullScreen(false);
                }
                else
                {
                    UndoShowFullScreen(false);
                }
            });
            #region 图片hover
            //pbDeviceManager.MouseHover += new EventHandler((sender, args) =>
            //{
            //    if (!pnContainer.Controls.Contains(viewManager))
            //    {
            //        pbDeviceManager.Image = Properties.Resources.tb_DM_h;
            //        pbDeviceManager.Refresh();
            //    }
            //});
            //pbDataBase.MouseHover += new EventHandler((sender, args) =>
            //{
            //    if (pnContainer.Controls.Contains(dataManager))
            //        return;
            //    pbDataBase.Image = Properties.Resources.tb_DB_h;
            //});
            //pbAuditTrail.MouseHover += new EventHandler((sender, args) =>
            //{
            //    if (pnContainer.Controls.Contains(auditTrail))
            //        return;
            //    pbAuditTrail.Image = Properties.Resources.tb_audit_h;
            //});
            pbExport.MouseHover += new EventHandler((sender, args) =>
            {
                pbExport.Image = Properties.Resources.tb_export_s;
            });
            pbMail.MouseHover += new EventHandler((sender, args) =>
            {
                pbMail.Image = Properties.Resources.tb_mail_s;
            });
            pbPrint.MouseHover += new EventHandler((sender, args) =>
            {
                pbPrint.Image = Properties.Resources.tb_print_s;
            });
            pbUpdate.MouseHover += new EventHandler((sender, args) =>
            {
                pbUpdate.Image = Properties.Resources.tb_update_s;
            });
            pbAdministration.MouseHover += new EventHandler((sender, args) =>
            {
                pbAdministration.Image = Properties.Resources.tb_admin_s;
            });
            #endregion
            #region 图片leave
            pbExport.MouseLeave += new EventHandler((sender, args) =>
            {
                pbExport.Image = Properties.Resources.tb_export;
            });
            pbMail.MouseLeave += new EventHandler((sender, args) =>
            {
                pbMail.Image = Properties.Resources.tb_mail;
            });
            pbPrint.MouseLeave += new EventHandler((sender, args) =>
            {
                pbPrint.Image = Properties.Resources.Print;
            });
            pbUpdate.MouseLeave += new EventHandler((sender, args) =>
            {
                pbUpdate.Image = Properties.Resources.tb_update;
            });
            pbAdministration.MouseLeave += new EventHandler((sender, args) =>
            {
                pbAdministration.Image = Properties.Resources.tb_admin;
            });
            //pbDeviceManager.MouseLeave += new EventHandler((sender, args) =>
            //{
            //    if (pnContainer.Controls.Contains(viewManager))
            //        return;
            //    pbDeviceManager.Image = Properties.Resources.tb_DM;
            //});
            //pbDataBase.MouseLeave += new EventHandler((sender, args) =>
            //{
            //    if (pnContainer.Controls.Contains(dataManager))
            //        return;
            //    pbDataBase.Image = Properties.Resources.tb_DB;
            //});
            //pbAuditTrail.MouseLeave += new EventHandler((sender, args) =>
            //{
            //    if (pnContainer.Controls.Contains(auditTrail))
            //        return;
            //    pbAuditTrail.Image = Properties.Resources.tb_audit;
            //});
            #endregion
            form.SizeChanged += new EventHandler((sender, args) =>
            {
                Size emptySize = new Size(1, 0);
                form.SuspendLayout();
                if (isFullScreen == false)
                {
                    if (isMinimum && (pnContainer.Size.Width > _PnContainerSize.Width || pnContainer.Size.Height > _PnContainerSize.Height))
                    {
                        pnContainer.Size = _PnContainerSize;
                        isMinimum = false;
                    }
                    else if (pnContainer.Size != emptySize)
                    {
                        _PnContainerSize = pnContainer.Size;
                        isMinimum = false;
                    }
                    else if (pnContainer.Size == emptySize)
                        isMinimum = true;
                    else
                        isMinimum = false;
                } 
                pnContainer.Refresh();
                form.ResumeLayout(false);
            });
            //form.Resize += new EventHandler(ResizeEnd);
            if (this.dataManager != null)
            {
                this.dataManager.OnComapreStatusChange += new EventHandler((sender, e) =>
                {
                    this.checkCompareStatusAndSetExportMenuItems(sender);
                });
            }
            //form.FormClosing += new FormClosingEventHandler(FormClosing);
            this.pbDeviceManager.MouseClick += new MouseEventHandler((sender, e) => {
                this.checkCompareStatusAndSetExportMenuItems(sender);
            });
            this.pbDataBase.MouseClick += new MouseEventHandler((sender, e) =>
            {
                this.checkCompareStatusAndSetExportMenuItems(sender);
            });
            this.pbAuditTrail.MouseClick += new MouseEventHandler((sender, e) =>
            {
                this.checkCompareStatusAndSetExportMenuItems(sender);
            });
        }
        public DialogResult FormClosing(object sender, EventArgs e)
        {
            if (pnContainer.Controls.Contains(dataManager))
                return dataManager.DeviceManagerExitDialog();
            else if(pnContainer.Controls.Contains(viewManager))
                return viewManager.DeviceManagerExitDialog();
            else
            {
                return DialogResult.OK;
            }
        }
        private void InitUserRights()
        {
            SetUIVisibleByUserRights();
        }
        private void SetUIVisibleByUserRights()
        {
            pbAuditTrail.Visible = Common.IsAuthorized(RightsText.ViewAuditTrail);
            dataManager.PnSignHis.Visible = dataManager.BtnSign.Visible = Common.IsAuthorized(RightsText.SignRecords);
            viewManager.PnSignHis.Visible = viewManager.BtnSign.Visible = Common.IsAuthorized(RightsText.SignRecords);
            viewManager.PbDeviceConfig.Visible = Common.IsAuthorized(RightsText.ConfigurateDevices);
            dataManager.TbCmt.Enabled = Common.IsAuthorized(RightsText.CommentRecords);
        }
        private void OpenTPSFile(object sender,EventArgs args)
        {
            if (dataManager.LoadTps())
            {
                ShowDataBase(pbDataBase, null);
            }
        }
        private void ShowDataBase(object sender, EventArgs args)
        {
            if (pnContainer.Controls.Contains(dataManager))
                return;
            if (pnContainer.Controls.Contains(viewManager))
            {
                if (DialogResult.Cancel == viewManager.DeviceManagerExitDialog())
                {
                    return;
                }
            }

            AddDataManager();
            SetDeviceClickPic((PictureBox)sender);
        }
        #region 设置容器显示那一个usercontrol
        private void AddDeviceManagerPanel()
        {
            this.pnContainer.Controls.Clear();
            //this.pnContainer.Controls.Add(deviceLeft);
            viewManager.Dock = DockStyle.Fill;
            this.pnContainer.Controls.Add(viewManager);
            pnContainer.Refresh();
        }
        private void AddAuditTrail()
        {
            this.pnContainer.Controls.Clear();
            this.pnContainer.Controls.Add(auditTrail);
            auditTrail.Dock = DockStyle.Fill;
            pnContainer.Refresh();
        }
        private void AddDataManager()
        {
            this.pnContainer.Controls.Clear();
            this.pnContainer.Controls.Add(dataManager);
            dataManager.Dock = DockStyle.Fill;
            pnContainer.Refresh();
        }
        #endregion
        #region 设置picturebox
        private void SetOrigionPic()
        {
            pbDeviceManager.Image = Properties.Resources.tb_DM_s;
            pbDataBase.Image = Properties.Resources.tb_DB;
            pbAuditTrail.Image = Properties.Resources.tb_audit;
            pbAdministration.Image = Properties.Resources.tb_admin;
            pbUpdate.Image = Properties.Resources.tb_update;
            pbExport.Image = Properties.Resources.tb_export;
            pbMail.Image = Properties.Resources.tb_mail;
            pbPrint.Image = Properties.Resources.Print;
        }
        //左边三个键pic
        private void SetDeviceClickPic(PictureBox pb)
        {
            if (pb == pbDeviceManager)
            {
                pbDeviceManager.Image = Properties.Resources.tb_DM_s;
                pbDataBase.Image = Properties.Resources.tb_DB;
                pbAuditTrail.Image = Properties.Resources.tb_audit;
            }
            else if (pb == pbDataBase)
            {
                pbDeviceManager.Image = Properties.Resources.tb_DM;
                pbDataBase.Image = Properties.Resources.tb_DB_s;
                pbAuditTrail.Image = Properties.Resources.tb_audit;
            }
            else
            {
                pbDeviceManager.Image = Properties.Resources.tb_DM;
                pbDataBase.Image = Properties.Resources.tb_DB;
                pbAuditTrail.Image = Properties.Resources.tb_audit_s;
            }
        }
        //右边功能键，发送导出打印等
        private void SetRightClicPic(PictureBox pb)
        {
        }
        #endregion
        public void AddLogOffLog()
        {
            if (Common.User.UserName != Common.SUPERUSER)
            {
                new OperationLogBLL().InsertLog(() =>
                {
                    Dictionary<string, object> d = new Dictionary<string, object>();
                    d.Add("OperateTime", DateTime.UtcNow);
                    d.Add("Action", LogAction.Logoff);
                    d.Add("UserName", Common.User.UserName);
                    d.Add("FullName", Common.User.FullName);
                    d.Add("Detail", "Successful");
                    d.Add("LogType", LogAction.SystemAuditTrail);
                    return d;
                });
            }
        }
        #region 导出
        /// <summary>
        /// 上下文菜单
        /// </summary>
        private void InitContextMenu()
        {
            ContextMenu contextMenu = new ContextMenu();
            MenuItem item = new MenuItem("Export to TPS", new EventHandler((seneder, e) => ExportTPS()));
            contextMenu.MenuItems.Add(item);
            item = new MenuItem("Export to PDF", new EventHandler((seneder, e) => ExportPDF()));
            contextMenu.MenuItems.Add(item);
            item = new MenuItem("Export to Excel", new EventHandler((seneder, e) => ExportExcel()));
            contextMenu.MenuItems.Add(item);

            this.pbExport.MouseClick += new MouseEventHandler((a, b) =>
            {
                DialogResult dialogResult = this.GetDataRecordSavingStatus();
                if (DialogResult.OK == dialogResult || DialogResult.Yes == dialogResult)
                {
                    if (b.Button == MouseButtons.Left)
                    {
                        if (this.pnContainer.Controls.Contains(this.auditTrail))
                        {
                            ExportPDF();
                        }
                        else
                        {
                            if (pbExport.ContextMenu == null)
                                pbExport.ContextMenu = contextMenu;
                            contextMenu.Show(pbExport, new System.Drawing.Point(b.X, b.Y));
                        }
                    }
                }
            });

            ContextMenu printMenu = new ContextMenu();
            MenuItem printMenuItem = null;
            printMenuItem = new MenuItem("Print Preview", new EventHandler((seneder, e) => Print(true)));
            printMenu.MenuItems.Add(printMenuItem);
            printMenuItem = new MenuItem("Print", new EventHandler((seneder, e) => Print(false)));
            printMenu.MenuItems.Add(printMenuItem);

            this.pbPrint.MouseClick += new MouseEventHandler((sender, args) => {
                DialogResult dialogResult = this.GetDataRecordSavingStatus();
                if (DialogResult.OK == dialogResult || DialogResult.Yes == dialogResult)
                {
                    if (args.Button == MouseButtons.Left)
                    {
                        if (this.pbPrint.ContextMenu == null)
                        {
                            this.pbPrint.ContextMenu = printMenu;
                        }
                        printMenu.Show(this.pbPrint, new System.Drawing.Point(args.X, args.Y));
                    }
                }
            });

        }

        private DialogResult GetDataRecordSavingStatus()
        {
            DialogResult result = DialogResult.Cancel;
            
            if (this.dataManager != null && this.pnContainer.Controls.Contains(this.dataManager) && !this.dataManager._IsFromTps)
            {
                result = this.dataManager.DeviceManagerExitDialog(MessageBoxButtons.OKCancel);
            }
            if (this.viewManager != null && this.pnContainer.Controls.Contains(this.viewManager))
            {
                result = this.viewManager.DeviceManagerExitDialog(MessageBoxButtons.OKCancel);
            }
            if (this.auditTrail != null && this.pnContainer.Controls.Contains(this.auditTrail))
            {
                result = DialogResult.OK;
            }
            return result;
        }
      
        private void ExportTPS()
        {
            DialogResult dialogResult = this.GetDataRecordSavingStatus();
            if (DialogResult.OK == dialogResult || DialogResult.Yes == dialogResult)
            {
                if (pnContainer.Contains(viewManager))
                    this.viewManager.ExportTps("tps");
                else if (pnContainer.Contains(dataManager))
                    this.dataManager.ExportTps("tps");
            }
        }
        private void ExportExcel()
        {
            DialogResult dialogResult = this.GetDataRecordSavingStatus();
            if (DialogResult.OK == dialogResult || DialogResult.Yes == dialogResult)
            {
                if (pnContainer.Contains(viewManager))
                    this.viewManager.ExportTps("xlsx");
                else if (pnContainer.Contains(dataManager))
                    this.dataManager.ExportTps("xlsx");
            }
        }
        private void ExportPDF()
        {
            DialogResult dialogResult = this.GetDataRecordSavingStatus();
            if (DialogResult.OK == dialogResult || DialogResult.Yes == dialogResult)
            {
                if (pnContainer.Contains(viewManager))
                    this.viewManager.ExportTps("pdf");
                else if (pnContainer.Contains(dataManager))
                    this.dataManager.ExportTps("pdf");
                else
                    this.auditTrail.ExportAuditTrail();
            }
        }
        private void Email()
        {
            DialogResult dialogResult = this.GetDataRecordSavingStatus();
            if (DialogResult.OK == dialogResult || DialogResult.Yes == dialogResult)
            {
                if (pnContainer.Contains(viewManager))
                    this.viewManager.EMail();
                else if (pnContainer.Contains(dataManager))
                    this.dataManager.Email();
                else
                    this.auditTrail.EMail();
            }
        }
        private void Print(bool isPreview)
        {
            if (pnContainer.Contains(viewManager))
                this.viewManager.Print(isPreview);
            else if (pnContainer.Contains(dataManager))
                this.dataManager.Print(isPreview);
            else
                this.auditTrail.Print(isPreview);
        }
        #endregion
        #region 全屏、退出全屏
        private void ShowFullScreen(bool isViewManager)
        {
            _PnContainerSize = pnContainer.Size;
            if (isViewManager)
            {
                viewManager.PnDeviceCfg.Visible = false;
                _ViewSize = viewManager.ViewSize;
                viewManager.SetPanelVisible(false);
                viewManager.SetPanelDock(DockStyle.Fill);
                this.viewManager.PbMax.Image = Properties.Resources.fullscreen_exit;
            }
            else
            {
                _ViewSize = dataManager.ViewSize;
                dataManager.SetPanelVisible(false);
                dataManager.SetPanelDock(DockStyle.Fill);
                this.dataManager.PbMax.Image = Properties.Resources.fullscreen_exit;
            }
            pnContainer.Dock = DockStyle.Fill;
            pnTool.Visible = pnMenu.Visible = false;
            isFullScreen = true;
            _FullScreen.ShowFullScreen();
            viewManager.PnDeviceCfg.Visible = true;
        }
        private void UndoShowFullScreen(bool isViewManager)
        {
            viewManager.PnDeviceCfg.Visible = false;
            pnTool.Visible = pnMenu.Visible = true;
            pnContainer.Dock = DockStyle.None;
            pnContainer.Size = _PnContainerSize;
            isFullScreen = false;
            _FullScreen.ShowFullScreen();
            if (isViewManager)
            {

                viewManager.SetPanelVisible(true);
                viewManager.SetPanelDock(DockStyle.None);
                viewManager.ViewSize = _ViewSize;
                this.viewManager.PbMax.Image = Properties.Resources.fullscreen;
                viewManager.SetPanelAnchor();
            }
            else
            {
                dataManager.SetPanelVisible(true);
                dataManager.SetPanelDock(DockStyle.None);
                dataManager.ViewSize = _ViewSize;
                this.dataManager.SetPanelAnchor();
                this.dataManager.PbMax.Image = Properties.Resources.fullscreen;
            }
            viewManager.PnDeviceCfg.Visible = true;
            pnContainer.Anchor = (AnchorStyles)(AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom);
        }
        #endregion 
        public void SetHelp()
        {
            string helpName = "Help.chm";
            if (Common.Versions == Versions.SoftwareVersions.S)
            {
                helpName = "Help Lite.chm";
            }
            Help.ShowHelp(form, Path.Combine(Application.StartupPath, helpName));
        }
        #region compare时让export, print, email按钮disable
        private void checkCompareStatusAndSetExportMenuItems(object sender)
        {
            if (sender == this.pbAuditTrail)
            {
                this.exportToolStripMenuItem.DropDownItems[0].Enabled = false;
                this.exportToolStripMenuItem.DropDownItems[2].Enabled = false;
            }
            else
            {
                this.exportToolStripMenuItem.DropDownItems[0].Enabled = true;
                this.exportToolStripMenuItem.DropDownItems[2].Enabled = true;
            }
            if (sender == this.pbDeviceManager || sender == this.pbAuditTrail)
            {
                this.pbExport.Enabled = true;
                this.pbMail.Enabled = true;
                this.pbPrint.Enabled = true;
                this.exportToolStripMenuItem.Enabled = true;
                this.emailFilesToolStripMenuItem.Enabled = true;
                this.printFileToolStripMenuItem.Enabled = true;
            }
            else
            {
                DataManagerUC dmu = this.dataManager;
                if (dmu != null)
                {
                    if (dmu.IsCompare)
                    {
                        this.pbExport.Enabled = false;
                        this.pbMail.Enabled = false;
                        this.pbPrint.Enabled = false;
                        this.exportToolStripMenuItem.Enabled = false;
                        this.emailFilesToolStripMenuItem.Enabled = false;
                        this.printFileToolStripMenuItem.Enabled = false;
                    }
                    else
                    {
                        this.pbExport.Enabled = true;
                        this.pbMail.Enabled = true;
                        this.pbPrint.Enabled = true;
                        this.exportToolStripMenuItem.Enabled = true;
                        this.emailFilesToolStripMenuItem.Enabled = true;
                        this.printFileToolStripMenuItem.Enabled = true;
                    }
                }
            }
        }
        #endregion
    }
    enum CtlState { device,data,audit }
}
