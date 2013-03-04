using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ZedGraph;
using ShineTech.TempCentre.DAL;
using System.Configuration;
using Services.Common;
using System.Drawing.Printing;
using System.IO;
using ShineTech.TempCentre.Platform;
using Babu.Windows;
using System.Text.RegularExpressions;
using System.Diagnostics;
using ShineTech.TempCentre.Versions;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;

namespace ShineTech.TempCentre.BusinessFacade
{
    public partial class ViewManager : UserControl
    {
        SuperDevice Tag;
        private bool IsMouseDown;
        public bool IsSaved;
        private bool isAuto = false;
        private Rectangle MouseRec;
        private ToolTip toolTip =new ToolTip ();
        private RedimStatus redimStatus;//鼠标在矩形区域内的位置
        private bool IsRectanglePainted = false;//矩形是否完成
        private bool IsLeftSide = false;//是否拖动左边框
        private int MouseDownX, MouseDownY;
        private Rec rec;//画好后的rectangle保存坐标及长宽
        private TextBox tbReportTitle = new TextBox();
        private TextBox tbReportComment = new TextBox();
        private SuperDevice currentTag;
        // TabControl是否刷新标志位
        private bool isTpSummaryRefreshed = false;
        private bool isTpGraphRefreshed = false;
        private bool isTpListRefreshed = false;
        private bool isTpReportEdiorRefreshed = false;
        private bool isSetViewTool = false;
        private List<SuperDevice> TagsList = new List<SuperDevice>();
        private ReportDataGenerator reportdataGenerator = new ReportDataGenerator();

        private string tbReportCommentString;
        

        public PictureBox PbMax
        {
            get
            {
                return pbMax;
            }
            set
            {
                pbMax = value;
            }
        }
        public Size ViewSize
        {
            get { return pnView.Size; }
            set { pnView.Size = value; }
        }
        private DeviceBLL _deviceBll = new DeviceBLL();
        private LogConfigBLL _logConfigBll = new LogConfigBLL();
        private AlarmConfigBLL _alarmConfigBll = new AlarmConfigBLL();
        private OperationLogBLL _logBll = new OperationLogBLL();
        private ReportEditorBLL _reportEditorBll = new ReportEditorBLL();
        private List<DigitalSignature> signatureList = new List<DigitalSignature>();
        public event EventHandler SaveRecordEvent;
        public event EventHandler ShowAdminForm;
        private XAxisVisibility selection = XAxisVisibility.DateAndTime;
        private static ITracing _tracing = TracingManager.GetTracing(typeof(ViewManager));
        private PrintPreviewDialog printPreviewDialog = Utils.GetPrintPreviewDialogue();
        private  int _YAxisLength;
        public  int YAxisLength
        {
            get
            { 
                if(_YAxisLength==0)
                {
                    object o=ConfigurationManager.AppSettings["YAxisLength"];
                    _YAxisLength = o == null ? 10 : Convert.ToInt32(o);
                }
                return _YAxisLength; 
            }
        }

       
        public System.Windows.Forms.TextBox TbCmt
        {
            get { return tbCmt; }
            set { tbCmt = value; }
        }
        public System.Windows.Forms.Label LbSign
        {
            get { return lbSign; }
            set { lbSign = value; }
        }
        public System.Windows.Forms.Button BtnSign
        {
            get { return btnSign; }
            set { btnSign = value; }
        }
        public PictureBox PbDeviceConfig
        {
            get { return pbDeviceConfig; }
            set { pbDeviceConfig = value; }
        }
        public Panel PnSignHis
        {
            get { return pnSignHis; }
        }
        private bool _IsConnected=false;
        //UserProfile profile;
        public ViewManager()
        {
            //Utils.ShowMessageBox("New Version", Messages.TitleNotification);
            Application.CurrentCulture = CultureInfo.InvariantCulture;
            this.initToolTipSetter();
            InitializeComponent();
            this.initializeTbReportComment();
            ClearLabelContents();
            this.InitEvents();
            this.InitLeftEvents();
            InitBackGroundWorker();
            this.tabCtl.TabPages.Remove(tabPage1);
            InitReportEdit();
            InitConfiguration();
            pnViewClick.BackgroundImage = Utils.DrawTextOnImage(Properties.Resources.wk_vm_h, "View Manager", 50, 9);
            pbDeviceConfig.Image = Utils.DrawTextOnImage(Properties.Resources.wk_cd, "Configure Device", 50, 9);
            ConnectionController.GenerateDeviceList(pnDevice);//构造checkbox
        }

        private void initializeTbReportComment()
        {
            this.tbReportComment.Width = 730;
            this.tbReportComment.Font = new Font("Arial", 9, GraphicsUnit.Pixel);
        }

        private System.Windows.Forms.ToolTip wrongTip = new System.Windows.Forms.ToolTip(new System.ComponentModel.Container());
        private void initToolTipSetter()
        {
            if (this.wrongTip != null)
            {
                this.wrongTip.ShowAlways = true;
                //this.wrongTip.IsBalloon = true;
                this.wrongTip.UseAnimation = false;
                this.wrongTip.UseFading = false;
                this.wrongTip.AutoPopDelay = 0;
                this.wrongTip.AutomaticDelay = 0;
                this.wrongTip.InitialDelay = 0;
                this.wrongTip.ReshowDelay = 0;
            }
        }
        public ViewManager(SuperDevice tag)
            : this()
        {
            this.Tag = tag;
        }
        #region

        [MethodImpl(MethodImplOptions.Synchronized)]

        public void InitViewManage(bool isCommentTitleRefresh)
        {
            if (Tag != null && Tag.RunStatus != 0 && Tag.TempUnit != null)
            {
                if (currentTag == null || string.Format("{0}{1}", currentTag.SerialNumber, currentTag.TripNumber) != string.Format("{0}{1}", Tag.SerialNumber, Tag.TripNumber))
                {
                    currentTag = Tag;
                    rbTempUnitC.Checked = Tag.TempUnit == "C" ? true : false;
                    rbTempUnitF.Checked = Tag.TempUnit == "F" ? true : false;
                }
                if (currentTag != null)
                {
                    string currentTagUnit = "";
                    if (rbTempUnitC.Checked)
                    {
                        currentTagUnit = "C";
                    }
                    if (rbTempUnitF.Checked)
                    {
                        currentTagUnit = "F";
                    }
                    if (Tag.TempUnit != currentTagUnit)
                    {
                        currentTag = Common.CopyTo(Tag);
                    }
                    else
                        currentTag = Tag;
                }
                this.isTpSummaryRefreshed = false;
                this.isTpGraphRefreshed = false;
                this.isTpListRefreshed = false;
                this.isTpReportEdiorRefreshed = false;
                TagsList.Clear();
                TagsList.Add(currentTag);
                GetDigitalSignature(currentTag);
                InitCommentsTitle(currentTag, isCommentTitleRefresh);
                if (this.tpSummary.Visible)
                {
                    InitTpSingleSummary(currentTag);
                }
                else if (this.tpGraph.Visible)
                {
                    InitTpGraph();
                }
                else if (this.tpList.Visible)
                {
                    InitTpDataList(currentTag);
                }
                else if (this.tpReportEdit.Visible)
                {
                    InitTpReportEditor();
                }
            }
        }

        public void InitViewManage()
        {
            InitViewManage(true);
        }

        private void InitTpGraph()
        {
            if (!this.isTpGraphRefreshed && currentTag != null)
            {
                InitViewTool();
                InitGraph(currentTag);
                this.isTpGraphRefreshed = true;
            }
            else if (currentTag != null)
            {
                InitGraph(currentTag);
            }
            else if(currentTag==null)
                GraphHelper.SetInitProperity(zedGraphControl1);
        }
        
        public  void InitViewManage(SuperDevice tag)
        {
            if (Tag == null)
                Tag = tag;
            InitViewManage();
        }
        private void InitViewTool()
        {
            if (!isSetViewTool)
            {
                cbLimitLine.Checked = Common.GlobalProfile.IsShowAlarmLimit;
                cbIdealRange.Checked = Common.GlobalProfile.IsFillIdealRange;
                cbShowMark.Checked = Common.GlobalProfile.IsShowMark;
                rbDateTime.Click += new EventHandler((sender, args) =>
                {
                    SetAxis(sender);
                    this.DrawLimitLine(zedGraphControl1.GraphPane, cbLimitLine.Checked, cbIdealRange.Checked);
                    DrawIdealRange(zedGraphControl1.GraphPane, cbIdealRange.Checked, cbLimitLine.Checked);
                    zedGraphControl1.Refresh();
                });
                rbDtaPoints.Click += new EventHandler((sender, args) =>
                {
                    SetAxis(sender);
                    this.DrawLimitLine(zedGraphControl1.GraphPane, cbLimitLine.Checked, cbIdealRange.Checked);
                    DrawIdealRange(zedGraphControl1.GraphPane, cbIdealRange.Checked, cbLimitLine.Checked);
                    zedGraphControl1.Refresh();
                });
                rbElapsedTime.Click += new EventHandler((sender, args) =>
                {
                    SetAxis(sender);
                    this.DrawLimitLine(zedGraphControl1.GraphPane, cbLimitLine.Checked, cbIdealRange.Checked);
                    DrawIdealRange(zedGraphControl1.GraphPane, cbIdealRange.Checked, cbLimitLine.Checked);
                    zedGraphControl1.Refresh();
                });
                pbUndo.Click += new EventHandler((sender, args) =>
                {
                    //zedGraphControl1.ZoomOutAll(zedGraphControl1.GraphPane);
                    InitGraph(currentTag);
                    MouseRec = Rectangle.Empty;
                    //ReDraw(true);
                    GraphHelper.ReDraw(true, zedGraphControl1, ref MouseRec);
                });
                cbLimitLine.CheckedChanged += new EventHandler((sender, args) =>
                {
                    this.DrawLimitLine(zedGraphControl1.GraphPane, cbLimitLine.Checked, cbIdealRange.Checked);
                    zedGraphControl1.Refresh();
                    //ReDraw(true);
                    GraphHelper.ReDraw(true, zedGraphControl1, ref MouseRec);
                });
                cbIdealRange.CheckedChanged += new EventHandler((sender, args) =>
                {
                    DrawIdealRange(zedGraphControl1.GraphPane, cbIdealRange.Checked, cbLimitLine.Checked);
                    zedGraphControl1.Refresh();
                });
                cbShowMark.CheckedChanged += new EventHandler(DrawMark);
                isSetViewTool = true;
            }
        }
        private void SetAxis(object sender)
        {
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked && (currentTag != null && currentTag.tempList.Count > 0))
            {
                switch (rb.Text)
                {
                    case "Date/Time":
                        InitGraph(currentTag);
                        break;
                    case "Elapsed Time":
                        SetXAxisAsElapsedTime();
                        break;
                    default:
                        SetXAxisAsDataPoints();
                        break;
                }
                zedGraphControl1.GraphPane.XAxis.Title.Text = rb.Text;
                zedGraphControl1.Refresh();
                GraphHelper.ReDraw(true, zedGraphControl1, ref MouseRec);
            }
        }
        private void InitReportEdit()
        {
            if (Common.Versions == SoftwareVersions.S)
            {
                //lblReportEditorTip.LinkArea = new LinkArea(0, 0);
                this.lblReportEditorTip.Visible = false;
            }
        }
        /// <summary>
        /// 对图进行初始化
        /// </summary>
        public void InitGraph(SuperDevice Tag)
        {
            if (Tag == null||Tag.tempList.Count==0)
                return;
            if (rbDateTime.Checked)
            {
                selection = XAxisVisibility.DateAndTime;
            }
            else if (rbDtaPoints.Checked)
                selection = XAxisVisibility.DataPoints;
            else
                selection = XAxisVisibility.ElapsedTime;
            zedGraphControl1.GraphPane.CurveList.Clear();
            GraphHelper.SetGraphAsDefaultProperity(zedGraphControl1, selection);
            GraphHelper.SetMinMaxLimits(Tag);
            GraphHelper.SetGraphDataSource(zedGraphControl1, Tag, selection, true);
            this.DrawLimitLine(zedGraphControl1.GraphPane, true, false);
            if (zedGraphControl1.GraphPane.Rect != RectangleF.Empty&&Tag.ReportGraph==null)
            {
                try
                {
                    int width = 730;
                    int height = (int) (730 * 0.63);
                    int rightMargin = 108;
                    int bottomMargin = 100;
                    Tag.ReportGraph = Platform.Utils.CopyToBinary(zedGraphControl1.GraphPane.GetImage(width, height, true, rightMargin, bottomMargin));
                    GraphHelper.SetRecOfChart(zedGraphControl1, zedGraphControl1.GraphPane);
                }
                catch
                {
                    Tag.ReportGraph = null;
                }
            }
            //根据viewtool设置是否show limit及ideal range
            GraphHelper.SetGraphDataSource(zedGraphControl1, Tag, selection, cbShowMark.Checked);
            this.DrawLimitLine(zedGraphControl1.GraphPane, cbLimitLine.Checked, cbIdealRange.Checked);
            DrawIdealRange(zedGraphControl1.GraphPane, cbIdealRange.Checked, cbLimitLine.Checked);
            zedGraphControl1.Refresh();
            GraphPane pane = zedGraphControl1.GraphPane;
        }
        public void InitTpDataList(SuperDevice Tag)
        {
            if (!this.isTpListRefreshed&&Tag!=null)
            {
                dgvList.Columns.Clear();
                dgvList.Rows.Clear();
                if (dgvList.Columns.Count == 0)
                {
                    dgvList.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    dgvList.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                    /*根据pointinfo来判断*/
                    DataGridViewColumn column = new DataGridViewColumn();
                    column.HeaderText = "Point";
                    column.Name = "ID";
                    column.ReadOnly = true;
                    column.CellTemplate = new DataGridViewTextBoxCell();
                    dgvList.Columns.Add(new DataGridViewColumn()
                    {
                        CellTemplate = new DataGridViewTextBoxCell()
                        ,
                        HeaderText = "Point"
                        ,
                        Name = "ID",
                        Width = 100
                    });
                    dgvList.Columns.Add(new DataGridViewColumn()
                        {
                            CellTemplate = new DataGridViewTextBoxCell()
                            ,
                            HeaderText = "Date/Time"
                            ,
                            Name = "PointTime",
                            Width = 200
                        });
                        dgvList.Columns.Add(new DataGridViewColumn()
                        {
                            CellTemplate = new DataGridViewTextBoxCell()
                            ,
                            HeaderText = "Elapsed Time"
                            ,
                            Name = "interval",
                            Width = 200
                        });
                    dgvList.Columns.Add(new DataGridViewColumn()
                    {
                        CellTemplate = new DataGridViewTextBoxCell()
                        ,
                        HeaderText = Tag.SerialNumber + "_" + Tag.TripNumber + "(°" + Tag.TempUnit + ")"
                        ,
                        Name = "PointTemp",
                        Width = 200
                    });
                }
                List<Tuple<int?, DateTime, double, string>> list = this.GetPointTempValue(Tag);
                int i = 0, j = 1;
                if (list != null && list.Count > 0)
                {
                    list.ForEach(p =>
                    {
                        dgvList.Rows.Add(1);
                        dgvList.Rows[i].Cells["ID"].Value = p.Item1;
                        dgvList.Rows[i].Cells["PointTemp"].Value = p.Item4;
                        if (p.Item1 != null)
                            dgvList.Rows[i].Cells["interval"].Value = TempsenFormatHelper.ConvertSencondToFormmatedTime(p.Item3.ToString());
                        else
                        {
                            dgvList.Rows[i].Cells["ID"].Value = string.Format("Mark{0}",j++);
                            dgvList.Rows[i].Cells["interval"].Value = null;
                            dgvList.Rows[i].DefaultCellStyle.ForeColor = Color.Red;
                            dgvList.Rows[i].DefaultCellStyle.Font = new Font("Arial",9f,FontStyle.Bold);
                        }
                        dgvList.Rows[i].Cells["PointTime"].Value = p.Item2.ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture);
                        i++;
                    });
                    dgvList.Columns["interval"].Visible = cbElapsed.Checked;
                    dgvList.Columns["PointTime"].Visible = cbDate.Checked;
                }
                this.isTpListRefreshed = true;
            }
        }
        private void InitEvents()
        {
            this.pbUndo.MouseHover += new EventHandler((sender, args) => pbUndo.Image=Properties.Resources.graph_back_h);
            this.pbUndo.MouseLeave += new EventHandler((sender, args) => pbUndo.Image = Properties.Resources.graph_back);
            this.pbUnfold.MouseHover += new EventHandler((sender, args) => pbUnfold.Image = tableViewTool.Visible == false ? Properties.Resources.graph_more_h : Properties.Resources.graph_collapse_h);
            this.pbUnfold.MouseLeave += new EventHandler((sender, args) => pbUnfold.Image = tableViewTool.Visible == false ? Properties.Resources.graph_more : Properties.Resources.graph_collapse);
            this.Load += new EventHandler((sender, args) =>
            {
                this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                this.SetStyle(ControlStyles.DoubleBuffer, true);
                this.SetStyle(ControlStyles.UserPaint, true);
                this.SetStyle(ControlStyles.ResizeRedraw, true);
                this.UpdateStyles();
            });
            this.tbCmt.TextChanged+=new EventHandler(ButtonStateOnChange);
            this.tbReportTitle.TextChanged += new EventHandler(ButtonStateOnChange);
            this.cbDate.CheckedChanged += new EventHandler((a, b) =>
            {
                if (dgvList.Columns["PointTime"] != null)
                    this.dgvList.Columns["PointTime"].Visible = cbDate.Checked;
            });
            this.cbElapsed.CheckedChanged += new EventHandler((a, b) =>
            {
                if (dgvList.Columns["interval"] != null)
                {
                    this.dgvList.Columns["interval"].Visible = cbElapsed.Checked;
                }
            });
            this.reportCanvasPanel.MouseDown += new MouseEventHandler((o, e) => reportCanvasPanel.Focus());
            this.rbTempUnitC.CheckedChanged += new EventHandler((sender, args) =>InitViewManage(false));
           
            this.zedGraphControl1.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);
            this.zedGraphControl1.MouseDownEvent += new ZedGraphControl.ZedMouseEventHandler(ZedOnMouseDown);
            this.zedGraphControl1.MouseMoveEvent += new ZedGraphControl.ZedMouseEventHandler(ZedOnMouseMove);
            this.zedGraphControl1.MouseUpEvent += new ZedGraphControl.ZedMouseEventHandler(ZedOnMouseUp);
            this.zedGraphControl1.Paint += new PaintEventHandler((a, b) => GraphHelper.ReDraw(false, zedGraphControl1, ref MouseRec));//on paint
            this.btnSave.Click+=new EventHandler((sender,args)=>Save());
            this.btnSign.Click += new EventHandler(Sign);//digital signature
            this.dgvList.CellPainting+=new DataGridViewCellPaintingEventHandler((sender,e)=>
            {
                Brush gridBrush = new SolidBrush(dgvList.GridColor);
                Pen gridLinePen = new Pen(gridBrush);
                if (e.RowIndex == dgvList.Rows.Count - 1)
                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom, e.CellBounds.Right-1, e.CellBounds.Bottom);
                gridLinePen.Dispose();
            });
            #region 图片
            this.pnViewClick.MouseHover += new EventHandler((a, b) =>
            {
                //pbViewManager.Image =  Utils.DrawTextOnImage(Properties.Resources.wk_vm_h, "View Manager", 50, 9);
                pnViewClick.BackgroundImage = Utils.DrawTextOnImage(Properties.Resources.wk_vm_h, "View Manager", 50, 9);
            });
            this.pbDeviceConfig.MouseHover += new EventHandler((a, b) =>
            {
                pbDeviceConfig.Image = Utils.DrawTextOnImage(Properties.Resources.wk_cd_h, "Configure Device", 50, 9);
            });
            this.pnViewClick.MouseLeave += new EventHandler((a, b) =>
            {
                if (pnView.Controls.Contains(this.tabCtl))
                    return;
                //pbViewManager.Image = Utils.DrawTextOnImage(Properties.Resources.wk_vm, "View Manager", 50, 9);
                pnViewClick.BackgroundImage = Utils.DrawTextOnImage(Properties.Resources.wk_vm, "View Manager", 50, 9);
            });
            this.pbDeviceConfig.MouseLeave += new EventHandler((a, b) =>
            {
                if (pnView.Controls.Contains(pnDeviceConfig))
                    return;
                pbDeviceConfig.Image = Utils.DrawTextOnImage(Properties.Resources.wk_cd, "Configure Device", 50, 9);
            });
            #endregion
            #region 电子签名显示记录
            this.pbArrowUp.Click += new EventHandler((sender, args) =>
            {
                this.pnSignature.Visible = true;
            });
            this.pbArrowDown.Click += new EventHandler((sender, args) =>
            {
                this.pnSignature.Visible = false;
            });
            pbArrowUp.MouseHover += new EventHandler((sender, args) => pbArrowUp.Image=Properties.Resources.arrow_up_hover);
            pbArrowUp.MouseLeave += new EventHandler((sender, args) => pbArrowUp.Image = Properties.Resources.arrow_up);
            pbArrowDown.MouseHover += new EventHandler((sender, args) => pbArrowDown.Image = Properties.Resources.arrow_down_hover);
            pbArrowDown.MouseLeave += new EventHandler((sender, args) => pbArrowDown.Image = Properties.Resources.arrow_down);
            pbMax.MouseHover += new EventHandler(OnHoverFullScreen);
            pbMax.MouseLeave += new EventHandler(OnLeaveFullScreen);
            #endregion
            #region 显示viewmanager和devicemanager
            this.pnViewClick.Click += new EventHandler(OnClickViewManager);
            this.pbDeviceConfig.Click += new EventHandler(OnClickDeviceConfig);
            #endregion

            // 注册几个tab页的Enter事件
            InitEventsForTabControls();

            //注册report title 和 comment文本框的事件
            ReportPublicControlEventInitializer.InitEventForTitleTextBox(this.tbReportTitle);
            ReportPublicControlEventInitializer.InitEventForCoupleTextBox(this.tbReportComment, this.tbCmt);
            ReportPublicControlEventInitializer.InitEventForCommentTextBox(this.tbReportComment);
            this.tbReportComment.TextChanged += new EventHandler(tbReportComment_TextChanged);
            ReportPublicControlEventInitializer.InitEventForCommentTextBox(this.tbCmt);
            //注册log interval相关的事件
            initDpLogIntervalStatusEvents();
            this.tbDesc.TextChanged += new EventHandler(tbDesc_Change);
            this.lbSign.SizeChanged += new EventHandler((sender, args) => SetSignLabel(lbSign.Tag as string));
            this.lbLoggerReader.SizeChanged += new EventHandler(GetLoggerReaderText);
        }
        private void OnClickDeviceConfig(object sender, EventArgs args)
        {
            if (!pnView.Controls.Contains(this.pnDeviceConfig))
            {
                
                pbDeviceConfig.Image = Utils.DrawTextOnImage(Properties.Resources.wk_cd_h, "Configure Device", 50, 9);
                pnViewClick.BackgroundImage = Utils.DrawTextOnImage(Properties.Resources.wk_vm, "View Manager", 50, 9);
                this.pnDeviceConfig.Location = tabCtl.Location;
                pnDeviceConfig.Size = new Size(tabCtl.Width, pnView.Height - 47);
                pnDeviceConfig.BackColor = Color.Transparent;
                this.pnView.Controls.Add(pnDeviceConfig);
                this.pnView.Controls.Remove(tabCtl);
                pnDeviceConfig.BringToFront();
                SetAlarmEnable();
                SetLogCycle();
                if (Tag != null && Tag.RunStatus == 2)
                {
                    if (DialogResult.Yes == Utils.ShowMessageBox(Messages.StopTheLogger, Messages.TitleNotification, MessageBoxButtons.YesNo))
                    {
                        //<to stop the device>
                        try
                        {
                            if (ObjectManage.DeviceNew != null)
                            {
                                string result=ObjectManage.DeviceNew.StopRecord();
                                if (!string.IsNullOrEmpty(result))
                                {
                                    Tag.RunStatus = 3;
                                    Tag.CurrentStatus = "Stopped";
                                    this.lbDeviceStatus.Text = string.Format("Current Status: {0}", Tag.CurrentStatus);
                                    Utils.ShowMessageBox(Messages.StopSuccessfully, Messages.TitleNotification);
                                }
                                else
                                {
                                    Utils.ShowMessageBox(Messages.StopFailed, Messages.TitleNotification);
                                }
                            }
                        }
                        catch { }
                    }
                    else
                        OnClickViewManager(sender,args);
                }
                if (Tag != null && Tag.RunStatus == 3 && this.btnDisconn != null && this.btnDisconn.Visible)
                {
                    ConnectionController.ShowStopDevice();
                }
            }
        }
        private void OnHoverFullScreen(object sender, EventArgs args)
        {
            if (pnDevice.Visible)
            {
                pbMax.Image =Properties.Resources.fullscreen_hover;
            }
            else
            {
                pbMax.Image = Properties.Resources.fullscreen_exit_hover;
            }
        }
        private void OnLeaveFullScreen(object sender, EventArgs args)
        {
            if (pnDevice.Visible)
            {
                pbMax.Image = Properties.Resources.fullscreen;
            }
            else
            {
                pbMax.Image = Properties.Resources.fullscreen_exit;
            }
        }
        private void OnClickViewManager(object sender, EventArgs args)
        {
            if (!pnView.Controls.Contains(this.tabCtl))
            {
                pnViewClick.BackgroundImage = Utils.DrawTextOnImage(Properties.Resources.wk_vm_h, "View Manager", 50, 9);
                pbDeviceConfig.Image = Utils.DrawTextOnImage(Properties.Resources.wk_cd, "Configure Device", 50, 9);
                this.tabCtl.Visible = true;
                this.tabCtl.Size = new Size(pnDeviceConfig.Width, pnView.Height - 90);
                this.pnView.Controls.Add(tabCtl);
                this.pnView.Controls.Remove(pnDeviceConfig);
            }
        }
        private void tbReportComment_TextChanged(object sender, EventArgs e)
        {
            var tbReportComment = sender as TextBox;
            if (tbReportComment != null)
            {
                using (Graphics g = tbReportComment.CreateGraphics())
                {
                    string[] lines = tbReportComment.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);
                    int actualLineCount = 0;
                    int maxRowCount = 3;
                    foreach (var item in lines)
                    {
                        int actualLineHeight = (int)Math.Ceiling(g.MeasureString(item, tbReportComment.Font).Width / (tbReportComment.Width - 25.5));
                        actualLineCount += actualLineHeight == 0 ? 1 : actualLineHeight;
                    }

                    if (actualLineCount > maxRowCount)
                    {
                        tbReportComment.Text = this.tbReportCommentString;
                        tbReportComment.SelectionStart = tbReportComment.Text.Length;
                        this.tbCmt.SelectionStart = this.tbCmt.Text.Length;
                    }
                    else
                    {
                        this.tbReportCommentString = tbReportComment.Text;
                    }
                }
            }
        }

        private void InitEventsForTabControls()
        {
            this.tpSummary.Enter += new EventHandler((sender, e) => {
                this.InitTpSingleSummary(currentTag);
            });
            tpGraph.SizeChanged += new EventHandler((a, b) =>
            {
                this.InitTpGraph();
                //GraphHelper.SetGraphAsDefaultProperity(zedGraphControl1, selection);
            });
            this.tpGraph.Enter += new EventHandler((sender, e) =>
            {
                this.InitTpGraph();
            });
            this.tpList.Enter += new EventHandler((sender, e) =>
            {
                this.InitTpDataList(currentTag);
            });
            this.tpReportEdit.Enter += new EventHandler((sender, e) =>
            {
                this.InitTpReportEditor();
            });
        }

        private void InitTpReportEditor()
        {
            this.InitTpGraph();
            if (Tag != null && !string.IsNullOrWhiteSpace(Tag.SerialNumber) && Tag.tempList.Count > 0)
            {
                if (!this.isTpReportEdiorRefreshed)
                {
                    DigitalSignatureBLL _digitalBll = new DigitalSignatureBLL();
                    SuperDevice currentTag = this.GetExportedTagAccordingToUnit();
                    if (currentTag != null)
                    {
                        InitTpSingleSummary(currentTag);
                        ReportEditorExporter exporter = new ReportEditorExporter(DeviceDataFrom.ViewManager, currentTag, this.signatureList, this.reportCanvasPanel, this.tbReportTitle, this.tbReportComment, this.lblReportEditorTip);
                        exporter.SignatureList = _digitalBll.GetDigitalSignatureBySnTn(Tag.SerialNumber, Tag.TripNumber);
                        exporter.GenerateReport();
                    }
                    this.isTpReportEdiorRefreshed = true;
                }
                
            }

        }
        /// <summary>
        ///  重绘鼠标悬停事件
        /// </summary>
        /// <param name="control"></param>
        /// <param name="pane">绘图板</param>
        /// <param name="curve">曲线</param>
        /// <param name="iPt">坐标位置</param>
        /// <returns>label的显示值</returns>
        private string MyPointValueHandler(ZedGraphControl control, GraphPane pane, CurveItem curve, int iPt)
        {
            // Get the PointPair that is under the mouse
            List<PointKeyValue> list = GraphHelper.GetTempList(pane, currentTag, selection, (int)Math.Ceiling(curve.Points[0].X), (int)(curve.Points[curve.Points.Count - 1].X));
            if (list != null && list.Count > iPt)
            {
                Axis x = curve.GetXAxis(pane);
                PointPair pt = curve[iPt];
                StringBuilder stringBuilder = new StringBuilder();
                string serial = string.Format("Serial Number: {0}", currentTag.SerialNumber);
                string label = string.Format("Trip Number: {0}", currentTag.TripNumber);
                string value = string.Format("Vaule: {0:f1}°{1}", pt.Y, currentTag.TempUnit);
                string date = string.Empty;
                string time = string.Empty;
                string pointIndex = string.Empty;
                date = string.Format("Date: {0}", list[iPt].PointTime.ToLocalTime().ToString(Common.GetDateOrTimeFormat(true, Common.GlobalProfile.DateTimeFormator), CultureInfo.InvariantCulture));
                time = string.Format("Time: {0}", list[iPt].PointTime.ToLocalTime().ToString(Common.GetDateOrTimeFormat(false, Common.GlobalProfile.DateTimeFormator), CultureInfo.InvariantCulture));
                PointKeyValue pkv = list.FirstOrDefault();
                if (selection == XAxisVisibility.DateAndTime)
                    pkv.PointTime = pkv.PointTime.ToUniversalTime();
                List<PointKeyValue> pointList = currentTag.tempList;
                int index = pointList.FindIndex(p => p.PointTemp == pkv.PointTemp && p.PointTime == pkv.PointTime && pkv.IsMark == p.IsMark) + iPt;
                pkv = currentTag.tempList[index];
                int markCount = currentTag.tempList.Where(p => p.PointTime <= pkv.PointTime && p.IsMark).Count();
                if (!pkv.IsMark)
                {
                    pointIndex = string.Format("Point: {0}", index - markCount + 1);
                }
                else
                {

                    pointIndex = string.Format("Mark: {0}", markCount);
                }
                return stringBuilder.AppendLine(serial).AppendLine(label).AppendLine(pointIndex).AppendLine(date).AppendLine(time).Append(value).ToString();
            }
            else
                return string.Empty;
            
        }
        /// <summary>
        /// 构造datalist中的结构
        /// </summary>
        /// <returns></returns>
        private List<Tuple<int?, DateTime, double, string>> GetPointTempValue(SuperDevice Tag)
        {
            List<PointKeyValue> pointList = Tag.tempList;
            int i = 0;
            List<Tuple<int?, DateTime, double, string>> list = new List<Tuple<int?, DateTime, double, string>>();
            pointList.ToList().ForEach(p=>
            {
                Tuple<int?, DateTime, double, string> tuple;
                if (!p.IsMark)
                {
                    tuple = new Tuple<int?, DateTime, double, string>(i + 1
                                                                                                         , p.PointTime.ToLocalTime()
                                                                                                         , i * Convert.ToDouble(Tag.LogInterval)
                                                                                                         , p.PointTemp.ToString("F1"));
                    
                    i++;
                }
                else
                {
                    tuple = new Tuple<int?, DateTime, double, string>(null
                                                                                                         , p.PointTime.ToLocalTime()
                                                                                                         , i * Convert.ToDouble(0)
                                                                                                         , p.PointTemp.ToString("F1"));
                }
                list.Add(tuple);
            });
            return list;
        }

        private void DrawLimitLine(GraphPane pane, bool isShowLimit, bool isShowIdeal)//画最高最低线
        {
            GraphHelper.DrawLimitLintAndFillIdeal(pane, currentTag, isShowIdeal, isShowLimit);
        }
        private void DrawIdealRange(GraphPane pane, bool isShowIdeal, bool isShowLimit)
        {
         
            GraphHelper.DrawLimitLintAndFillIdeal(pane, currentTag, isShowIdeal, isShowLimit);
        }
        private void DrawMark(object sender,EventArgs e)
        {
            GraphHelper.ReDrawUnCompareCurveItem(zedGraphControl1.GraphPane, currentTag, selection, cbShowMark.Checked);
            this.DrawLimitLine(zedGraphControl1.GraphPane, cbLimitLine.Checked, cbIdealRange.Checked);
            DrawIdealRange(zedGraphControl1.GraphPane, cbIdealRange.Checked, cbLimitLine.Checked);
            GraphHelper.ReDraw(true, zedGraphControl1, ref MouseRec);
            zedGraphControl1.Refresh();
        }
        #endregion
        #region 保存连接设备信息中的内容
        /// <summary>
        /// 保存设备中的信息
        /// </summary>
        public void Save()
        {
            Common.Save(ref IsSaved, signatureList, _reportEditorBll, _deviceBll, _alarmConfigBll, _logConfigBll, Tag,tbCmt.Text,tbReportTitle.Text);
            if (IsSaved)
            {
                Common.AddSaveRecordLog(_logBll, Tag);
                Common.AddCommentsLog(_logBll, Tag, tbCmt);
                //ConnectionController.SetSaveButtonState(btnSave, false);
                SaveRecordEvent(null, null);
            }
        }
        #endregion
        #region 定义鼠标有选择框计算明细值：1.绘制矩形；2.遍历所有的point
        private bool ZedOnMouseDown(ZedGraphControl sender, MouseEventArgs e)
        {
            MouseDownX = e.Location.X;
            MouseDownY = e.Location.Y;
            zedGraphControl1.DragStartPt = e.Location;
            if (e.Button == MouseButtons.Right)
            {
                if (e.X <= zedGraphControl1.GraphPane.Chart.Rect.Left || e.X >= zedGraphControl1.GraphPane.Chart.Rect.Right)
                {
                    return false;
                }
                zedGraphControl1.Refresh();
                MouseRec = Rectangle.Empty;
                IsMouseDown = true;
                IsRectanglePainted = false;
                //DrawOriginal(e.Location);
                GraphHelper.DrawOriginal(e.Location, zedGraphControl1, ref MouseRec);
                return true;
            }
            else
            {
                return false;
            }
        }
        private bool ZedOnMouseMove(ZedGraphControl sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                if (IsMouseDown)
                {
                    //ResizeRectangle(e.Location);
                    GraphHelper.ResizeRectangle(e.Location, zedGraphControl1, ref MouseRec);
                    //
                }
                return true;
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (IsRectanglePainted && RedimStatus.None != redimStatus)
                {
                    zedGraphControl1.Refresh();
                    switch (redimStatus)
                    {
                        case RedimStatus.Center:
                            zedGraphControl1.Cursor = Cursors.Hand;
                            //MoveRectangle(e.X, e.Y);
                            GraphHelper.MoveRectangle(e.X, e.Y, MouseDownX, rec, ref MouseRec, zedGraphControl1);
                            break;
                        default:
                            zedGraphControl1.Cursor = Cursors.SizeWE;
                            //RedimRectangle(e.X, e.Y);
                            GraphHelper.RedimRectangle(e.X, e.Y, MouseDownX, IsLeftSide, rec, ref MouseRec, zedGraphControl1);
                            break;
                    }
                    //ReDraw(true);
                    GraphHelper.ReDraw(true, zedGraphControl1, ref MouseRec);
                    return true;
                }
                else
                {
                    //zedGraphControl1.HandleZoomDrag(e.Location);
                    return false;
                }
            }
            else
            {
                if (IsRectanglePainted&&MouseRec.Width!=0)
                {
                    //redimStatus = IsOverRectangle(e.X, e.Y);
                    redimStatus = GraphHelper.IsOverRectangle(e.X, e.Y, ref IsLeftSide, ref MouseRec);
                    switch (redimStatus)
                    {
                        case RedimStatus.Center:
                            zedGraphControl1.Cursor = Cursors.Hand;
                            break;
                        case RedimStatus.None:
                            zedGraphControl1.Cursor = Cursors.Cross;
                            break;
                        default:
                            zedGraphControl1.Cursor = Cursors.SizeWE;
                            break;
                    }
                    return true;
                }
                return false;
            }
        }
        private bool ZedOnMouseUp(ZedGraphControl sender, MouseEventArgs e)
        {

            rec = new Rec(MouseRec);
            if (e.Button == MouseButtons.Right)
            {
                if (IsMouseDown)
                {
                    zedGraphControl1.Capture = false;
                    Cursor.Clip = Rectangle.Empty;
                    IsMouseDown = false;
                    //DrawRectangle();
                    GraphHelper.DrawRectangle(zedGraphControl1, MouseRec);
                    if (MouseRec != null)
                    {
                        //this.SetToolTip();
                        GraphHelper.SetToolTip(zedGraphControl1, MouseRec, currentTag, selection, toolTip);
                        //ReDraw(true);
                        GraphHelper.ReDraw(true, zedGraphControl1, ref MouseRec);
                        IsRectanglePainted = true;
                    }
                    
                    //MouseRec = Rectangle.Empty;
                }
                return true;
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (IsRectanglePainted && redimStatus != RedimStatus.None)
                {
                    GraphHelper.SetToolTip(zedGraphControl1, MouseRec, currentTag, selection, toolTip);
                    GraphHelper.ReDraw(true, zedGraphControl1, ref MouseRec);
                    this.tableViewTool.Refresh();
                    return true;
                }
                else
                {
                    MouseRec = Rectangle.Empty;
                    GraphHelper.HandleZoomFinish(sender, e.Location, currentTag, selection, 1, false,cbShowMark.Checked);
                    this.DrawLimitLine(sender.GraphPane, cbLimitLine.Checked, cbIdealRange.Checked);
                    DrawIdealRange(sender.GraphPane, cbIdealRange.Checked, cbLimitLine.Checked);
                    sender.Refresh();
                    this.tableViewTool.Refresh();
                    return false;
                }
            }
            else
                return false;
        }
        /// <summary>
        /// 计算MKT值
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private string CalcMKT(List<int> list)
        {
            #region calc the mkt .
            double ndot, sum, ln, mkt;
            sum = 0;
            ndot = 0;
            try
            {
                foreach (int iStr in list)
                {
                    sum += Math.Exp(-10 / Math.Round((1.0 * (iStr + 27315) / 100), 1, MidpointRounding.AwayFromZero));
                    ndot++;
                }

                ln = Math.Log(sum / ndot);

                mkt = 10 / (-ln) - 273.15;

                return mkt.ToString("F01");// Convert.ToString(mkt);             // MKT 平均动能温度
                //F02
            }
            catch
            {
                return "0.0";
            }
            #endregion
        }
        #endregion

        private void SetXAxisAsDataPoints()
        {
            if (currentTag != null)
            {
                selection = XAxisVisibility.DataPoints;
                GraphHelper.SetGraphAsDefaultProperity(zedGraphControl1, XAxisVisibility.DataPoints);
                string sntn = currentTag.SerialNumber + "_" + currentTag.TripNumber;
                GraphHelper.SetGraphDataSource(zedGraphControl1, currentTag, XAxisVisibility.DataPoints,  cbShowMark.Checked);

            }
        }
        private void SetXAxisAsElapsedTime()
        {
            if (currentTag != null)
            {
                selection = XAxisVisibility.ElapsedTime;
                GraphHelper.SetGraphAsDefaultProperity(zedGraphControl1, XAxisVisibility.ElapsedTime);
                string sntn = currentTag.SerialNumber + "_" + currentTag.TripNumber;
                GraphHelper.SetGraphDataSource(zedGraphControl1, currentTag, XAxisVisibility.ElapsedTime,cbShowMark.Checked);
            }
        }
        #region signature
        private void Sign(object sender,EventArgs args)
        {
            int result = Common.DeviceModificationType(Tag, tbCmt, tbReportTitle);
            if (result != -1)
            {
                string m = result == 0 ? Messages.B19 : Messages.B20;
                if (DialogResult.OK == Utils.ShowMessageBox(m, Messages.TitleNotification, MessageBoxButtons.OKCancel))
                {
                    Common.Save(ref IsSaved, signatureList, _reportEditorBll, _deviceBll, _alarmConfigBll, _logConfigBll, Tag, tbCmt.Text, tbReportTitle.Text);
                    if (IsSaved)
                    {
                        Common.AddSaveRecordLog(_logBll, Tag);
                        Common.AddCommentsLog(_logBll, Tag, tbCmt);
                        //ConnectionController.SetSaveButtonState(btnSave, false);
                        SaveRecordEvent(null, null);
                        Common.Sign(IsSaved, Tag, ref signatureList);
                        this.InitViewManage();
                    }
                }
            }
            else
            {
                Common.Sign(IsSaved, Tag, ref signatureList);
                this.InitViewManage();
            }
        }
        /// <summary>
        ///将对象保存到文件中
        /// </summary>
        private void SaveToTps(SuperDevice device,List<DigitalSignature> sign,ReportEditor editor, string path)
        {
            byte[] plaintext = Platform.Utils.SerializeToXML<SuperDevice>(device);
            DataSignature signature = SignatureHelper.CreateSignature(plaintext);
            signature.List = sign;
            signature.Editor = editor;
            byte[] tps = Platform.Utils.SerializeToXML<DataSignature>(signature);
            Platform.Utils.SaveTheFile(tps, path);
        }

        private bool ReadFromTps(string path)
        {
            byte []tps=Platform.Utils.ReadFromFile(path);
            DataSignature ds=new DataSignature ();
            try
            {
                ds = Platform.Utils.DeserializeFromXML<DataSignature>(tps, typeof(DataSignature));//反序列化
            }
            catch
            {
                Utils.ShowMessageBox(Platform.Messages.FileDamage,Platform.Messages.TitleWarning);
                return false;
            }
            if (SignatureHelper.VerifySignature(ds))
            {
                //if (Tag == null)
                //    Tag = ObjectManage.GetDeviceInstance(DeviceType.ITAGSingleUse);
                //Tag = Platform.Utils.DeserializeFromXML<SuperDevice>(ds.Data, Tag.GetType());
                Tag = Platform.Utils.DeserializeFromXML<SuperDevice>(ds.Data, typeof(SuperDevice));
                var s = (from p in Tag.tempList
                         where p.PointTemp == Tag.tempList.Select(t => t.PointTemp).Max()
                         select p.PointTemp.ToString() + "°" + Tag.TempUnit + "@" + p.PointTime.ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture));
                Tag.HighestC = s.ToList().Count == 0 ? "" : s.ToList().First();
                s = (from p in Tag.tempList
                     where p.PointTemp == Tag.tempList.Select(t => t.PointTemp).Min()
                     select p.PointTemp + "°" + Tag.TempUnit + "@" + p.PointTime.ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture));
                Tag.LowestC = s.ToList().Count == 0 ? "" : s.First();
                ObjectManage.SetDevice(Tag);
                SetReportEditorFromTps(ds.Editor);
                SetSignedRecordLabel(ds.List);
                return true;
            }
            else
            {
                Utils.ShowMessageBox(Messages.UnauthorizedTps, Messages.TitleError);
                return false;
            }
        }
        public void ExportTps(string reportType)
        {
            if (Tag != null && !string.IsNullOrWhiteSpace(Tag.SerialNumber))
            {
                SaveFileDialog file = new SaveFileDialog();
                IReportExportService exporter = null;
                switch (reportType)
                {
                    case "tps":
                        file.Filter = "Tps Files(.tps)|*.tps";
                        break;
                    case "xlsx":
                        file.Filter = "Excel Files(.xlsx)|*.xlsx";
                        break;
                    case "pdf":
                        file.Filter = "PDF Files(.pdf)|*.pdf";
                        break;
                    default:
                        break;
                }
                //file.Filter = "Tps Files(.tps)|*.tps|Excel Files(.xlsx)|*.xlsx|PDF Files(.pdf)|*.pdf";
                Common.SetDefaultPathForSaveFileDialog(file, SavingFileType.Report);
                file.FileName = Tag.SerialNumber + "_" + Tag.TripNumber;
                
                DigitalSignatureBLL _digitalBll = new DigitalSignatureBLL();
                signatureList = _digitalBll.GetDigitalSignatureBySnTn(Tag.SerialNumber, Tag.TripNumber);
                if (file.ShowDialog() == DialogResult.OK)
                {
                    string src = file.FileName.ToString();
                    //string reportType = src.Substring(src.LastIndexOf('.') + 1);
                    SuperDevice currentTag = GetExportedTagAccordingToUnit();
                    if (currentTag != null)
                    {
                        switch (reportType)
                        {
                            case "tps":
                                //ReportEditor editor = GetReportEditorSelection(Tag.SerialNumber, Tag.TripNumber);
                                ReportEditor editor = Common.GetReportEditorSelection(_reportEditorBll, Tag.SerialNumber, Tag.TripNumber, tbCmt.Text, tbReportTitle.Text);
                                SaveToTps(Tag, signatureList, editor, src);
                                break;
                            case "xlsx":
                                exporter = new ExcelReportExporter(DeviceDataFrom.ViewManager, currentTag, this.signatureList, src);
                                break;
                            case "pdf":
                                exporter = new PDFReportExporter(DeviceDataFrom.ViewManager, currentTag, this.signatureList, src);
                                break;
                            default:
                                break;
                        }
                    }
                    if (exporter != null)
                    {
                        this.setReportPropertyAndSaveReport(exporter);
                    }
                }
            }
        }

        private SuperDevice GetExportedTagAccordingToUnit()
        {
            return currentTag;
        }

        private void setReportPropertyAndSaveReport(IReportExportService exporter)
        {
            if (!string.IsNullOrWhiteSpace(this.tbReportTitle.Text))
            {
                exporter.Title = this.tbReportTitle.Text;
            }
            else
            {
                if (Common.GlobalProfile != null)
                {
                    exporter.Title = Common.GlobalProfile.ReportTitle;
                }
            }
            
            exporter.CurrentComment = this.tbReportComment.Text;
            this.InitTpGraph();
            try
            {
                exporter.GenerateReport();
            }
            catch (IOException)
            {
                Utils.ShowMessageBox(Messages.SameNameFileOpened, Messages.TitleError);
            }
            catch (Exception e)
            {
                Utils.ShowMessageBox(e.Message, Messages.TitleError);
            }
        }
        #endregion
        #region report edit
        private ReportEditor GetReportEditorSelection(string sn,string tn)
        {
            ReportEditor editor = _reportEditorBll.GetReportEditorBySnTn(sn,tn);
            if (editor != null && editor.ID == 0)
            {
                editor.ID = _reportEditorBll.GetReportEditorPKValue() + 1;
            }
            editor.SN = sn;
            editor.TN = tn;
            editor.Comments = tbCmt.Text.Trim();
            editor.ReportTitle = tbReportTitle.Text.Trim();
            editor.Remark = DateTime.Now.ToString();
            return editor;
        }
        private void SetReportEditorFromTps(ReportEditor editor)
        {
            if (editor != null)
            {
                tbCmt.Text = editor.Comments;
                tbReportTitle.Text = editor.ReportTitle;
            }
        }
        private void SetSignedRecordLabel(List<DigitalSignature> list)
        {
            if (list != null && list.Count > 0)
            {
                signatureList = list.OrderBy(p => p.SignTime).ToList();
                string signtext = string.Format("Signature[{0}]: {1} ", signatureList.Count, signatureList.Last().ToString(Common.GlobalProfile.DateTimeFormator));
                SetSignLabel(signtext);
                //lbSign.Text = signtext;
                //设置电子签名list
                GenerateSignatureColumns();
                BindSignature(list);
            }
        }
        private void GenerateSignatureColumns()
        {
            if (lvSignature.Columns.Count <= 0)
            {
                int meaningWidth = lvSignature.Width - 390;
                lvSignature.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 9, FontStyle.Regular);
                lvSignature.Columns.Add(new DataGridViewColumn()
                {
                    CellTemplate = new DataGridViewTextBoxCell(),
                    HeaderText = "Index",
                    Name = "ID",
                    Width = 40
                });
                lvSignature.Columns.Add(new DataGridViewColumn()
                {
                    CellTemplate = new DataGridViewTextBoxCell(),
                    HeaderText = "User Name",
                    Name = "UserName",
                    Width = 80
                });
                lvSignature.Columns.Add(new DataGridViewColumn()
                {
                    CellTemplate = new DataGridViewTextBoxCell(),
                    HeaderText = "Full Name",
                    Name = "FullName",
                    Width = 120
                });
                lvSignature.Columns.Add(new DataGridViewColumn()
                {
                    CellTemplate = new DataGridViewTextBoxCell(),
                    HeaderText = "Meaning",
                    Name = "Meaning",
                    Width = meaningWidth
                });
                lvSignature.Columns.Add(new DataGridViewColumn()
                {
                    CellTemplate = new DataGridViewTextBoxCell(),
                    HeaderText = "Date Time",
                    Name = "DateTime",
                    MinimumWidth = 150,
                    AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
                });
            }
        }
        private void BindSignature(List<DigitalSignature> list)
        {
            lvSignature.Rows.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                lvSignature.Rows.Add();
                lvSignature.Rows[i].Cells["ID"].Value = i + 1;
                lvSignature.Rows[i].Cells["UserName"].Value = list[i].UserName;
                lvSignature.Rows[i].Cells["FullName"].Value = list[i].FullName;
                lvSignature.Rows[i].Cells["Meaning"].Value = list[i].MeaningDesc;
                lvSignature.Rows[i].Cells["DateTime"].Value = list[i].SignTime.ToLocalTime().ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture);
            }
        }
        private void SetSignLabel(string text)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                    return;
                int width = TextRenderer.MeasureText(text, lbSign.Font).Width;
                if (width > lbSign.Width)
                {
                    string s = string.Empty;
                    int index = 0;
                    for (int i = text.Length - 1; i >= 0; i--)
                    {
                        s = text.Substring(0, i) + "...";
                        index = i;
                        width = TextRenderer.MeasureText(s, lbSign.Font).Width;
                        if (width > lbSign.Width)
                            continue;
                        else
                            break;
                    }
                    //lbSign.Text = (index - 3) <= 0 ? "" : text.Substring(0, index - 3) + "...";
                    lbSign.Text = s;
                }
                else
                    lbSign.Text = text;
                if (lbSign.Tag == null)
                {
                    lbSign.Tag = text;
                }
                this.toolTip.SetToolTip(lbSign, text);
            }
            catch (Exception exp)
            {
                _tracing.Error(exp, "sign label error");
            }
        }
        #endregion

        public void Print(bool isPreview)
        {
            if (this.Tag != null && !string.IsNullOrWhiteSpace(Tag.SerialNumber) && this.Tag.tempList != null && this.Tag.tempList.Count > 0)
            {
                FileHelper.DeleteTempFiles("print");
                string tempFileName = Path.Combine(System.Windows.Forms.Application.StartupPath, "temp", string.Format("{0}_{1}.print", Tag.SerialNumber, Tag.TripNumber));
                SuperDevice currentTag = GetExportedTagAccordingToUnit();
                if (true)
                {
                    IReportExportService exporter = new PDFReportExporter(DeviceDataFrom.ViewManager, currentTag, this.signatureList, tempFileName, true);
                    this.setReportPropertyAndSaveReport(exporter);
                    PDFReportPrinter printer = new PDFReportPrinter(tempFileName);
                    
                    try
                    {
                        if (isPreview)
                        {
                            this.printPreviewDialog.Document = printer.PrintDocument;
                            this.printPreviewDialog.Height = 500;
                            this.printPreviewDialog.Width = 800;
                            this.printPreviewDialog.ShowDialog();
                        }
                        else
                        {
                            printer.PrintReport();
                        }
                    }
                    catch (InvalidPrinterException ipe)
                    {
                        Utils.ShowMessageBox(Messages.NoPrinterInstalled, Messages.TitleError);
                    }
                    catch (Exception e)
                    {
                        Utils.ShowMessageBox(e.Message, Messages.TitleError);
                    }
                    
                    
                }
            }
        }
        public void EMail()
        {
            if (this.Tag != null && !string.IsNullOrWhiteSpace(Tag.SerialNumber))
            {
                string fileNameWithFullPath = Path.Combine(System.Windows.Forms.Application.StartupPath, "temp", string.Format("{0}.tps", Tag.SerialNumber + "_" + Tag.TripNumber));
                DigitalSignatureBLL _digitalBll = new DigitalSignatureBLL();
                signatureList = _digitalBll.GetDigitalSignatureBySnTn(Tag.SerialNumber, Tag.TripNumber);
                ReportEditor editor = GetReportEditorSelection(Tag.SerialNumber, Tag.TripNumber);
                SaveToTps(Tag, signatureList, editor, fileNameWithFullPath);
                new ReportOutlookEmailer().CreateEmailAndAddAttachments(fileNameWithFullPath);
                FileHelper.DeleteTempFiles(fileNameWithFullPath);
            }
        }
        #region device left
        
        public void OnClickConnectDevice(object sender ,EventArgs e)
        {
            try
            {
                DeviceType dt = DeviceType.ITAGSingleUse;
                var v = ConnectionController.CbList.Where(p => p.Value.Checked).ToList();
                if (v != null && v.Count > 0)
                {
                    cb = v.First().Value;
                    dt = v.First().Key;
                    Tag = ObjectManage.GetDeviceInstance(dt);
                }
                else
                {
                    Utils.ShowMessageBox(Messages.ConnectWithNoDeviceSelected, Messages.TitleError);
                    return;
                }
                ConnectionController.ManualConnectWorker.RunWorkerAsync();
                StartBackGroudWorker();
                this.IsSaved = false;
            }
            catch
            {
                this.IsSaved = false;
                return;
            }
        }
        public void OnClickAutoConnectDevice(object sender,EventArgs e)
        {
            try
            {
                AutoSelectedDevice();
                StartBackGroudWorker();
                this.IsSaved = false;
            }
            catch { this.IsSaved = false; return; }
        }
        private void AutoSelectedDevice()
        {
            isAuto = true;
            ConnectionController.AutoConnectWorker.RunWorkerAsync();
        }
        private void ManualConnectDevice(object sender, DoWorkEventArgs e)
        {
            DeviceType dt = DeviceType.ITAGSingleUse;
            var v = ConnectionController.CbList.Where(p => p.Value.Checked).ToList();
            if (v != null && v.Count > 0)
            {
                cb = v.First().Value;
                dt = v.First().Key;
                Tag = ObjectManage.GetDeviceInstance(dt);
            }
            DateTime start = DateTime.Now, end = DateTime.Now;
            while (!_IsConnected && ((end - start).TotalSeconds <= Common.TimeOut) && !ConnectionController.ManualConnectWorker.CancellationPending)
            {
                //DisconnetInForce();
                Common.IsConnectCompleted=_IsConnected = Tag.Connect((int)dt);
                if (!_IsConnected)
                {
                    Thread.Sleep(2000);
                }
                end = DateTime.Now;
            }
            Common.IsConnectCompleted = true;
        }
        private void AutoConnectDevice(object sender, DoWorkEventArgs e)
        {
            DateTime start = DateTime.Now, end = DateTime.Now;
            while (!_IsConnected && ((end - start).TotalSeconds <= Common.TimeOut)&&!ConnectionController.AutoConnectWorker.CancellationPending)
            {
                //DisconnetInForce();
                DeviceType dt = SuperDevice.GetModelFromDevice();
                cb = ConnectionController.CbList[dt];
                Tag = ObjectManage.GetDeviceInstance(dt);
                Common.IsConnectCompleted = _IsConnected = Tag.Connect((int)dt);
                if (!_IsConnected)
                {
                    Thread.Sleep(2000);
                }
                end = DateTime.Now;
            }
            Common.IsConnectCompleted = true;
        }
        private void OnClickCancelConnect(object sender, EventArgs e)
        {
            if (isAuto)
                ConnectionController.AutoConnectWorker.CancelAsync();
            else
                ConnectionController.ManualConnectWorker.CancelAsync();
            ConnectionController.ProgressWorker.CancelAsync();
            //DisconnetInForce();
            ResetConnection();
            InitConnectBackGroundWorker();
            btnCancelConnect.Visible = pbProgress.Visible = false;
            this.btnAuto.Visible = this.btnConnect.Visible = true;
            ConnectionController.IsAbortConnection = true;
        }
        public void InitLeftEvents()
        {
            this.btnConnect.Click += new EventHandler(OnClickConnectDevice);
            this.btnAuto.Click += new EventHandler(OnClickAutoConnectDevice);
            this.btnCancelConnect.Click += new EventHandler(OnClickCancelConnect);
        }
        private void InitStatus()
        {
            this.lbProductName.Text = string.Format("Product Name: {0}", Tag.DeviceName);
            this.lBattery.Text = string.Format("Battery: {0}%", Tag.Battery);
            this.lStatus.Text = string.Format("Device Connected");
            this.lbDeviceStatus.Text = string.Format("Current Status: {0}",Tag.CurrentStatus);
            this.lbModel.Text = string.Format("Model: {0}",Tag.Model);
            this.lbSerialNum.Text = string.Format("Serial Number: {0}", Tag.SerialNumber);
            if (Tag.DeviceID < 200)
                lBattery.Visible = false;
            else
                lBattery.Visible = true; 
            SetConnectButtonStatus();
        }
        private bool SetConfigEnable(SuperDevice tag)
        {
            if (tag == null)
                return false;
            bool isVisible=ConnectionController.GetConfigVisibleProperity(tag);
            if(Common.IsAuthorized(RightsText.ConfigurateDevices))
                pbDeviceConfig.Visible = isVisible;
            pnDeviceConfig.Enabled = isVisible;
            if (pnView.Controls.Contains(this.pnDeviceConfig)&&!isVisible)
            {
                pnViewClick.BackgroundImage = Utils.DrawTextOnImage(Properties.Resources.wk_vm_h, "View Manager", 50, 9);
                pbDeviceConfig.Image = Utils.DrawTextOnImage(Properties.Resources.wk_cd, "Configure Device", 50, 9);
                this.tabCtl.Visible = true;
                this.pnView.Controls.Add(tabCtl);
                this.pnView.Controls.Remove(pnDeviceConfig);
            }
            if (tag.DeviceID == 200 || tag.DeviceID == 201)
            {
                rbMultiAlarm.Visible = false;
                rbSingleAlarm.Checked = true;
            }
            else
            {
                rbMultiAlarm.Visible = true;
            }
            //SetAlarmPanel(null, null);
            switch (tag.RunStatus)
                {
                    case 0:
                        if (isVisible && !pnDeviceConfig.Visible)
                        {
                            DialogResult result = Utils.ShowMessageBox(Messages.ConfirmContinueToConfigureDevice, Messages.TitleNotification, MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                            {
                                if (!pnView.Controls.Contains(this.pnDeviceConfig) && Common.IsAuthorized(RightsText.ConfigurateDevices))
                                {
                                    pbDeviceConfig.Image = Utils.DrawTextOnImage(Properties.Resources.wk_cd_h, "Configure Device", 50, 9);
                                    pnViewClick.BackgroundImage = Utils.DrawTextOnImage(Properties.Resources.wk_vm, "View Manager", 50, 9);
                                    this.pnDeviceConfig.Location = tabCtl.Location;
                                    pnDeviceConfig.Size = new Size(tabCtl.Width, pnView.Height - 47);
                                    pnDeviceConfig.BackColor = Color.Transparent;
                                    this.pnView.Controls.Add(pnDeviceConfig);
                                    this.pnView.Controls.Remove(tabCtl);
                                    pnDeviceConfig.BringToFront();
                                }
                            }
                        }
                        break;
                    case 3:
                     if (pnView.Controls.Contains(this.pnDeviceConfig))
                         ConnectionController.ShowStopDevice();
                        break;
                    default:
                        if((tag.tempList==null ||tag.tempList.Count==0)&&tag.RunStatus!=1)
                            Utils.ShowMessageBox(Messages.NoTempPoint, Messages.TitleNotification, MessageBoxButtons.OK);
                        if (tag.RunStatus == 1)
                            ConnectionController.SetSignPanelState(pnSignHis, false);
                        break;
                }
            return true;
        }
        /// <summary>
        /// 线程间的调用
        /// </summary>
        private void SetText(string text)
        {
            if (lStatus.InvokeRequired)
            {
                lStatus.Invoke(new Action<string>(SetText), text);
            }
            else
            {
                lStatus.Text = text;
                Application.DoEvents();
            }
        }
        private void SetCheckStatus(Control ctl, bool status)
        {
            if (ctl.InvokeRequired)
            {
                ctl.Invoke(new Action<Control,bool>(SetCheckStatus),ctl, status);
            }
            else
            {
                ((CheckBox)ctl).Checked = status;
            }
        }
        public void SetPanelVisible(bool visible)
        {
            pnDeviceStatus.Visible=pnDevice.Visible = visible;
        }
        public Panel PnDeviceCfg
        {
            get { return pnDeviceConfig; }
            set { pnDeviceConfig = value; }
        }
        
        public void SetPanelDock(DockStyle dock)
        {
            this.pnView.Dock = dock;
        }
        public void SetPanelAnchor()
        {
            this.pnView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
        }
        #endregion
        #region progress bar
        //private BackgroundWorker worker;
        private int count;
        private CheckBox cb;
        private Button btnDisconn;
        private void StartBackGroudWorker()
        {
            ConnectionController.ProgressWorker.RunWorkerAsync();
            pbProgress.Visible = true;
            this.btnCancelConnect.Visible = true;
            this.btnAuto.Visible = this.btnConnect.Visible = false;
            this.Cursor = Cursors.WaitCursor;
        }
        private void DoWork(object sender, DoWorkEventArgs e)
        {
            while (!_IsConnected && !Common.IsConnectCompleted && !ConnectionController.ProgressWorker.CancellationPending)
            {
                System.Threading.Thread.Sleep(600);
                count = count + 1;
                if (count > 100)
                {
                    count = 0;
                    ConnectionController.ProgressWorker.ReportProgress(100);
                    SetText(string.Format("Connecting {0}%.", 100));
                    break;
                }
                else
                {
                    ConnectionController.ProgressWorker.ReportProgress(count);
                    SetText(string.Format("Connecting {0}%.", count));
                }
                
            }
        }
        private void CompleteWork(object sender, RunWorkerCompletedEventArgs e)
        {
            ResetManager();
           
            if (_IsConnected)
            {
                SetText(string.Format("Connecting {0}%.", 100));
                SetSaveAndSignState(Tag);
                if (SetConfigEnable(Tag))
                {
                    InitStatus();
                    InitViewManage(Tag);
                }
                SetCheckStatus(cb, _IsConnected);
            }
            else
            {
                //连接对象置空
                ResetConnection();
                Tag = null;
                btnCancelConnect.Visible = false;
                btnAuto.Visible = btnConnect.Visible = true;
                if (!ConnectionController.IsAbortConnection)
                {
                    SetText(string.Format("Connecting {0}.", "failed"));
                    if (!isAuto)
                        Utils.ShowMessageBox(Messages.ConnectDeviceFailed, Messages.TitleError);
                    else
                        Utils.ShowMessageBox(Messages.AutoConnectDeviceFailed, Messages.TitleError);
                }
                else
                {
                    SetText(string.Format("Connecting {0}.", "canceled"));
                }
            }
            isAuto = ConnectionController.IsAbortConnection = Common.IsRemoveDevice = Common.IsConnectCompleted = _IsConnected = false;
            count = 0;
            btnCancelConnect.Visible = false;
            pbProgress.Visible = false;
            this.Cursor = Cursors.Default;
            Common.LoggerReadTime = Common.User.FullName + "@" + DateTime.UtcNow.ToString(Common.GetDefaultDateTimeFormat(), CultureInfo.InvariantCulture);
        }
        private void SetSaveAndSignState(SuperDevice Tag)
        {
            if (Tag == null)
                return;
            ConnectionController.SetSignButtonState(btnSign, Tag.tempList.Count == 0 ? false : true);
            ConnectionController.SetSaveButtonState(btnSave, true);
            //ConnectionController.SetSaveButtonState(btnSave, Common.IsDeviceModification(Tag, tbCmt, tbReportTitle));
            ConnectionController.SetCommentTextState(tbCmt,(Tag.tempList.Count==0|| !Common.IsAuthorized(RightsText.CommentRecords))?false:true);
        }
        private void ButtonStateOnChange(object sender, EventArgs args)
        {
            if (Tag == null)
                return;
            ConnectionController.SetSaveButtonState(btnSave, true);
            //ConnectionController.SetSaveButtonState(btnSave, Common.IsDeviceModification(Tag, tbCmt, tbReportTitle));
        }
        private void ResetManager()
        {
            ClearLabelContents();
            ResetLeftStatus();
            lvSignature.Rows.Clear();
            signatureList.Clear();
            lbSign.Text = "Unsigned";
            lbLoggerReader.Tag = null;
            lbSign.Tag = null;
            tbCmt.Enabled = Common.IsAuthorized(RightsText.CommentRecords);
            GraphHelper.SetInitProperity(zedGraphControl1);
            zedGraphControl1.Refresh();
            dgvList.Rows.Clear();
            dgvList.Columns.Clear();
            reportCanvasPanel.Controls.Clear();
            pbDeviceConfig.Visible =Common.IsAuthorized(RightsText.ConfigurateDevices);
            pnDeviceConfig.Enabled = false;
            ConnectionController.SetSignButtonState(btnSign, false);
            ConnectionController.SetSaveButtonState(btnSave, false);
            ConnectionController.SetSignPanelState(pnSignHis, true);
            clearToolTips();
        }

        private void clearToolTips()
        {
            this.toolTip.RemoveAll();
        }
        private void ResetLeftStatus()
        {
            this.lbProductName.Text = string.Format("{0}", string.Empty);
            this.lBattery.Text = string.Format("{0}", string.Empty);
            this.lStatus.Text = string.Format("No Device Connected");
            this.lbDeviceStatus.Text = string.Format("");
            this.lbModel.Text = string.Format("{0}", string.Empty);
            this.lbSerialNum.Text = string.Format("{0}", string.Empty);
        }
        private void InitBackGroundWorker()
        {
            ConnectionController.ProgressWorker = null;
            ConnectionController.ProgressWorker.DoWork += new DoWorkEventHandler(DoWork);
            ConnectionController.ProgressWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(CompleteWork);
            InitConnectBackGroundWorker();
        }
        private void InitConnectBackGroundWorker()
        {
            if (ConnectionController.AutoConnectWorker != null)
            {
                ConnectionController.AutoConnectWorker.Dispose();
                ConnectionController.AutoConnectWorker = null;
            }
            if (ConnectionController.ManualConnectWorker != null)
            {
                ConnectionController.ManualConnectWorker.Dispose();
                ConnectionController.ManualConnectWorker = null;
            }
            ConnectionController.ManualConnectWorker.DoWork+=new DoWorkEventHandler(ManualConnectDevice);
            ConnectionController.AutoConnectWorker.DoWork += new DoWorkEventHandler(AutoConnectDevice);
        }
        private void SetConnectButtonStatus()
        {
            if (btnDisconn == null)
            {
                btnDisconn = new Button();
                btnDisconn.Click += new EventHandler(DisconnectDevice);
                pnDevice.Controls.Add(btnDisconn);
            }
            btnDisconn.Visible = true;
            btnDisconn.Text = "Disconnect";
            btnDisconn.Font = btnAuto.Font;
            btnDisconn.Size = btnAuto.Size;
            btnDisconn.BackColor = btnAuto.BackColor;
            btnDisconn.Location = new Point((pnDevice.Width-btnDisconn.Width)/2,btnAuto.Location.Y);
            btnDisconn.Anchor = btnAuto.Anchor;
            btnAuto.Visible = btnConnect.Visible = false;
        }
        public void DisconnectDevice(object sender, EventArgs args)
        {
            btnAuto.Visible = btnConnect.Visible = true;
            if(btnDisconn!=null)
                btnDisconn.Visible = false;
            try
            {
                Tag = currentTag = null;
                ResetConnection();
                //ResetLeftStatus();
                ResetManager();
            }
            catch(Exception exp)
            {
                _tracing.Error(exp, "disconnect the device error!");
            }
        }
        public void DisconnectDeviceOnRemove(object sender, EventArgs args)
        {
            btnCancelConnect.Visible = false;
            btnAuto.Visible = btnConnect.Visible = true;
            if (btnDisconn != null)
                btnDisconn.Visible = false;
            try
            {
                if (ConnectionController.AutoConnectWorker.IsBusy)
                    ConnectionController.AutoConnectWorker.CancelAsync();
                if (ConnectionController.ManualConnectWorker.IsBusy)
                    ConnectionController.ManualConnectWorker.CancelAsync();
                //Common.IsConnectCompleted = false;
                Tag = currentTag = null;
                DisconnetInForce();
                ResetManager();
            }
            catch (Exception exp)
            {
                _tracing.Error(exp, "disconnect the device error!");
            }
        }
        private void DisconnetInForce()
        {
            if (ObjectManage.DeviceNew != null)
            {
                ObjectManage.DeviceNew.disconnectDevice();
                ObjectManage.DeviceNew = null;
            }
            if (ObjectManage.DeviceSingleUse != null)
            {
                ObjectManage.DeviceSingleUse.disconnectDevice();
                ObjectManage.DeviceSingleUse = null;
            }
        }
        private void ResetConnection()
        {
            if (ObjectManage.DeviceNew != null)
            {
                if ((Environment.OSVersion.Version.Major == 5) && (Environment.OSVersion.Version.Minor >= 1))
                {
                    ObjectManage.DeviceNew.softDisconnectDevice();
                }
                else
                {
                    ObjectManage.DeviceNew.disconnectDevice();
                    ObjectManage.DeviceNew = null;
                }
            }
            if (ObjectManage.DeviceSingleUse != null)
            {
                ObjectManage.DeviceSingleUse.disconnectDevice();
                ObjectManage.DeviceSingleUse = null;
            }
        }
        #endregion

        #region <summary>
        private void GetDigitalSignature(SuperDevice tag)
        {
            if (tag != null)
            {
                signatureList = new DigitalSignatureBLL().GetDigitalSignatureBySnTn(tag.SerialNumber, tag.TripNumber);
                SetSignedRecordLabel(signatureList);
            }
        }
        private void InitCommentsTitle(SuperDevice tag)
        {
            InitCommentsTitle(tag, true);
        }
        private void InitCommentsTitle(SuperDevice tag, bool isCommentTitleRefresh)
        {
            if (tag != null && isCommentTitleRefresh)
            {
                ReportEditor editor = _reportEditorBll.GetReportEditorBySnTn(tag.SerialNumber, tag.TripNumber);
                if (editor != null)
                {
                    tbCmt.Text = ReportConstString.CommentDefaultString;
                    tbReportTitle.Text = ReportConstString.TitleDefaultString;
                    if (!string.IsNullOrWhiteSpace(editor.Comments) && ReportConstString.CommentDefaultString != editor.Comments)
                        tbCmt.Text = editor.Comments;
                    if (!string.IsNullOrWhiteSpace(editor.ReportTitle) && ReportConstString.TitleDefaultString != editor.ReportTitle)
                        tbReportTitle.Text = editor.ReportTitle;
                }
            }
        }
        public void InitTpSingleSummary(SuperDevice tag)
        {
            if (!this.isTpSummaryRefreshed&&tag!=null)
            {
                try
                {
                    InitDeviceInfo(tag);
                    InitTripInformation(tag);
                    InitLoggingSummary(tag);
                    InitAlarms(tag);
                }
                catch (Exception exp)
                {
                    _tracing.Error(exp,"Summary init failed.");
                }
                this.isTpSummaryRefreshed = true;
            }
            
        }
        private void InitDeviceInfo(SuperDevice tag)
        {
            #region product
            lProductName.Text = string.Format("Device: {0}", tag.DeviceName);
            lSerialNum.Text = string.Format("Serial Number: {0}", tag.SerialNumber);
            lModel.Text = string.Format("Model: {0}", tag.Model);
            lLogCycle.Text = string.Format("Log Interval/Cycle: {0}{2}{1}", TempsenFormatHelper.ConvertSencondToFormmatedTime(tag.LogInterval), tag.LogCycle,"/");
            lStartMode.Text = string.Format("Start Mode: {0}", tag.StartModel);
            if (tag.StartModel == "Manual Start")
                lStartDelay.Text = string.Format("Start Delay: {0}", tag.LogStartDelay);
            else
                lStartDelay.Text = string.Empty;
            #endregion
        }
        private void InitTripInformation(SuperDevice tag)
        {
            lTripNum.Text = string.Format("Trip Number: {0}", tag.TripNumber);
            if (tag.DeviceID < 200)
                lbDescText.Visible = lDesc.Visible = false;
            else
            {
                lbDescText.Visible=lDesc.Visible = true;
                lDesc.Tag = string.Format("{0}", tag.Description);
                lbDescText.Text = "Description:";
                this.toolTip.SetToolTip(lDesc, tag.Description);
                lDesc.Text = tag.Description;
            }
        }
        private void InitLoggingSummary(SuperDevice tag)
        {

            lLowest.Text = string.Format("Lowest Temperature: {0}", Common.SetTempTimeFormat(tag.LowestC));
            lHighest.Text = string.Format("Highest Temperature: {0}", Common.SetTempTimeFormat(tag.HighestC));
            lTripLen.Text = string.Format("Trip Length: {0}", tag.TripLength);
            lLogStop.Text = string.Format("Stop Time: {0}", tag.LoggingEnd == DateTime.MinValue ? string.Empty : tag.LoggingEnd.ToLocalTime().ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture));
            lLogStart.Text = string.Format("Start Time/First Point: {0}", tag.LoggingStart == DateTime.MinValue ? string.Empty : tag.LoggingStart.ToLocalTime().ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture));
            lMKT.Text = string.Format("Mean Kinetic Temperature: {0}{1}", tag.MKT, string.IsNullOrEmpty(tag.MKT) ? string.Empty : "°"+tag.TempUnit);
            lAveTemp.Text = string.Format("Average Temperature: {0}{1}", tag.AverageC, string.IsNullOrEmpty(tag.AverageC) ? string.Empty : "°" + tag.TempUnit);
            lDataPoint.Text = string.Format("Data Points: {0}", tag.DataPoints == 0 ? string.Empty : tag.DataPoints.ToString());
            lbLoggerReader.Tag=SetLoggerReader(tag);
            //SetDynamicLabelText(lbLoggerReader, null);
            GetLoggerReaderText(lbLoggerReader, null);
        }

        private void InitAlarms(SuperDevice tag)
        {
            IDictionary<string, string[]> alarmDataContents = this.reportdataGenerator.GetAlarmRowContents(tag);
            string[] highRowContents = alarmDataContents["highRowContents"];
            string[] lowRowContents = alarmDataContents["lowRowContents"];
            string[] a1RowContents = alarmDataContents["a1RowContents"];
            string[] a2RowContents = alarmDataContents["a2RowContents"];
            string[] a3RowContents = alarmDataContents["a3RowContents"];
            string[] a4RowContents = alarmDataContents["a4RowContents"];
            string[] a5RowContents = alarmDataContents["a5RowContents"];
            string[] a6RowContents = alarmDataContents["a6RowContents"];

            if (tag.AlarmMode == 1)
            {
                lbAlarmType.Text = string.Format("Alarms");
                lbLimits.Text = string.Format("[{0}]", "High && Low Alarm");
                lbAlarm.Text = "Alarm Zones";
                if (!string.IsNullOrEmpty(tag.AlarmLowLimit))
                {
                    lLowLimit.Text = string.Format("Low limit: {0}°{1}", tag.AlarmLowLimit, tag.TempUnit);
                    this.lLowAlarmType.Text = string.Format("{0}", tag.tempList.Count == 0 ? string.Empty : tag.AlarmLowStatus);
                    lbLowDelay.Text = string.Format("{0}({1})", tag.AlarmLowDelay, tag.LowAlarmType);
                    //lEventLowTrig.Text = string.Format("{0}", Convert.ToDateTime(tag.LowAlarmFirstTrigged) == DateTime.MinValue ? "" : Convert.ToDateTime(tag.LowAlarmFirstTrigged).ToString(Common.GlobalProfile.DateTimeFormator));
                    lEventLowTrig.Text = lowRowContents[4];
                    this.lLowAlarmEventNum.Text = string.Format("{0}", tag.tempList.Count == 0 ? string.Empty : tag.LowAlarmEvents.ToString());
                    this.lEventTotalTimeBelow.Text = string.Format("{0}", tag.tempList.Count == 0 ? string.Empty : tag.LowAlarmTotalTimeBelow);
                }
                else
                    lLowLimit.Text = string.Format("Low limit:");
                if (!string.IsNullOrEmpty(tag.AlarmHighLimit))
                {
                    lHighLimit.Text = string.Format("High limit: {0}°{1}", tag.AlarmHighLimit, tag.TempUnit);
                    //lEventHighTrig.Text = string.Format("{0}", Convert.ToDateTime(tag.HighAlarmFirstTrigged) == DateTime.MinValue ? "" : Convert.ToDateTime(tag.HighAlarmFirstTrigged).ToString(Common.GlobalProfile.DateTimeFormator));
                    lEventHighTrig.Text = highRowContents[4];
                    this.lEventTotalTimetAbove.Text = string.Format("{0}", tag.tempList.Count == 0 ? string.Empty : tag.HighAlarmTotalTimeAbove);
                    lHighAlarmEventNum.Text = string.Format("{0}", tag.tempList.Count == 0 ? string.Empty : tag.HighAlarmEvents.ToString());
                    this.lHighAlarmType.Text = string.Format("{0}", tag.tempList.Count == 0 ? string.Empty : tag.AlarmHighStatus);
                    lbHighDelay.Text = string.Format("{0}({1})", tag.AlarmHighDelay, tag.HighAlarmType);
                }
                else
                    lHighLimit.Text = string.Format("High limit:");
            }
            else if (tag.AlarmMode == 2)
            {
                lbAlarmType.Text = string.Format("Alarms");
                lbLimits.Text = string.Format("[{0}]", "Multiple Alarms");
                lbAlarm.Text = "Alarm Zones";
                //a6
                if (!string.IsNullOrEmpty(tag.A1))
                {
                    lHighLimit.Text = string.Format("A1: over {0}°{1}", tag.A1, tag.TempUnit);
                    lbHighDelay.Text = string.Format("{0}({1})", TempsenFormatHelper.ConvertSencondToFormmatedTime(tag.AlarmDelayA1.ToString()), tag.AlarmTypeA1);
                    this.lEventTotalTimetAbove.Text = string.Format("{0}", tag.tempList.Count == 0 ? string.Empty : tag.AlarmTotalTimeA1);
                    lHighAlarmEventNum.Text = string.Format("{0}", tag.tempList.Count == 0 ? string.Empty : tag.AlarmNumA1.ToString());
                    //lEventHighTrig.Text = string.Format("{0}", Convert.ToDateTime(tag.AlarmA1First) == DateTime.MinValue ? "" : Convert.ToDateTime(tag.AlarmA1First).ToString(Common.GlobalProfile.DateTimeFormator));
                    lEventHighTrig.Text = a1RowContents[4];
                    lHighAlarmType.Text = string.Format("{0}", tag.tempList.Count == 0 ? string.Empty : tag.AlarmA1Status);
                }
                else
                {
                    lHighLimit.Text = string.Format("A1:");
                }
                //a5
                if (!string.IsNullOrEmpty(tag.A2))
                {
                    lLowLimit.Text = string.Format("A2: over {0}°{1}", tag.A2, tag.TempUnit);
                    lbLowDelay.Text = string.Format("{0}({1})", TempsenFormatHelper.ConvertSencondToFormmatedTime(tag.AlarmDelayA2.ToString()), tag.AlarmTypeA2);
                    this.lEventTotalTimeBelow.Text = string.Format("{0}", tag.tempList.Count == 0 ? string.Empty : tag.AlarmTotalTimeA2);
                    lLowAlarmEventNum.Text = string.Format("{0}", tag.tempList.Count == 0 ? string.Empty : tag.AlarmNumA2.ToString());
                    //lEventLowTrig.Text = string.Format("{0}", Convert.ToDateTime(tag.AlarmA2First) == DateTime.MinValue ? "" : Convert.ToDateTime(tag.AlarmA2First).ToString(Common.GlobalProfile.DateTimeFormator));
                    lEventLowTrig.Text = a2RowContents[4];
                    lLowAlarmType.Text = string.Format("{0}", tag.tempList.Count == 0 ? string.Empty : tag.AlarmA2Status);
                }
                else
                    lLowLimit.Text = string.Format("A2:");
                //a4
                if (!string.IsNullOrEmpty(tag.A3))
                {
                    lbA3Temp.Text = string.Format("A3: over {0}°{1}", tag.A3, tag.TempUnit);
                    lbA3Delay.Text = string.Format("{0}({1})", TempsenFormatHelper.ConvertSencondToFormmatedTime(tag.AlarmDelayA3.ToString()), tag.AlarmTypeA3);
                    lbA3TotalTime.Text = string.Format("{0}", tag.tempList.Count == 0 ? string.Empty : tag.AlarmTotalTimeA3);
                    lbA3Num.Text = string.Format("{0}", tag.tempList.Count == 0 ? string.Empty : tag.AlarmNumA3.ToString());
                    //lbA3First.Text = string.Format("{0}", Convert.ToDateTime(tag.AlarmA3First) == DateTime.MinValue ? "" : Convert.ToDateTime(tag.AlarmA3First).ToString(Common.GlobalProfile.DateTimeFormator));
                    lbA3First.Text = a3RowContents[4];
                    lbA3Status.Text = string.Format("{0}", tag.tempList.Count == 0 ? string.Empty : tag.AlarmA3Status);
                }
                else
                    lbA3Temp.Text = string.Format("A3:");
                //a3
                lbA4Temp.Text = string.Format("A4: {0} to {1}°{2}", tag.A4, tag.A3, tag.TempUnit);
                lbA4Delay.Text = string.Format("{0}", "Unlimited");
                lbA4TotalTime.Text = string.Format("{0}", tag.tempList.Count == 0 ? string.Empty : tag.AlarmTotalTimeIdeal);
                //a2
                if (!string.IsNullOrEmpty(tag.A4))
                {
                    lbA5Temp.Text = string.Format("A5: under {0}°{1}", tag.A4, tag.TempUnit);
                    lbA5Delay.Text = string.Format("{0}({1})", TempsenFormatHelper.ConvertSencondToFormmatedTime(tag.AlarmDelayA4.ToString()), tag.AlarmTypeA4);
                    lbA5TotalTime.Text = string.Format("{0}", tag.tempList.Count == 0 ? string.Empty : tag.AlarmTotalTimeA4);
                    lbA5Num.Text = string.Format("{0}", tag.tempList.Count == 0 ? string.Empty : tag.AlarmNumA4.ToString());
                    //lbA5First.Text = string.Format("{0}", Convert.ToDateTime(tag.AlarmA4First) == DateTime.MinValue ? "" : Convert.ToDateTime(tag.AlarmA4First).ToString(Common.GlobalProfile.DateTimeFormator));
                    lbA5First.Text = a5RowContents[4];
                    lbA5Status.Text = string.Format("{0}", tag.tempList.Count == 0 ? string.Empty : tag.AlarmA4Status);
                }
                else
                    lbA5Temp.Text = string.Format("A5:");
                //a1
                if (!string.IsNullOrEmpty(tag.A5))
                {
                    lbA6Temp.Text = string.Format("A6: under {0}°{1}", tag.A5, tag.TempUnit);
                    lbA6Delay.Text = string.Format("{0}({1})", TempsenFormatHelper.ConvertSencondToFormmatedTime(tag.AlarmDelayA5.ToString()), tag.AlarmTypeA5);
                    lbA6TotalTime.Text = string.Format("{0}", tag.tempList.Count == 0 ? string.Empty : tag.AlarmTotalTimeA5);
                    lbA6Num.Text = string.Format("{0}", tag.tempList.Count == 0 ? string.Empty : tag.AlarmNumA5.ToString());
                    //lbA6First.Text = string.Format("{0}", Convert.ToDateTime(tag.AlarmA5First) == DateTime.MinValue ? "" : Convert.ToDateTime(tag.AlarmA5First).ToString(Common.GlobalProfile.DateTimeFormator));
                    lbA6First.Text = a6RowContents[4];
                    lbA6Status.Text = string.Format("{0}", tag.tempList.Count == 0 ? string.Empty : tag.AlarmA5Status);
                }
                else
                    lbA6Temp.Text = string.Format("A6:");
            }
            else
                lbLimits.Text = string.Format("[{0}]", "No Alarm Setting");
        }
        private void ClearLabelContents()
        {
            tbCmt.Text = Platform.ControlText.CommentText;
            tableAlarms.Controls.Cast<System.Windows.Forms.Label>().ToList().ForEach(p =>
            {
                System.Windows.Forms.Label labels = p as System.Windows.Forms.Label;
                if (labels != null)
                {
                    if (labels != null && labels.Text != "Alarm Zones" && labels.Text != "Alarm Zones" && labels.Text != "Alarm Delay" && labels.Text != "Total Time"
                        && labels.Text != "Events" && labels.Text != "First Triggered" && labels.Text != "Alarm Status")
                        labels.Text = string.Empty;
                }
            });
            tableDevice.Controls.Cast<System.Windows.Forms.Label>().ToList().ForEach(p =>
            {
                System.Windows.Forms.Label labels = p as System.Windows.Forms.Label;
                if (labels != null)
                {
                    if (labels != null && labels.Text != "Alarm Zones" && labels.Text != "Alarm Zones" && labels.Text != "Alarm Delay" && labels.Text != "Total Time"
                        && labels.Text != "Events" && labels.Text != "First Triggered" && labels.Text != "Alarm Status")
                        labels.Text = string.Empty;
                }
            });
            tableLog.Controls.Cast<System.Windows.Forms.Label>().ToList().ForEach(p =>
            {
                System.Windows.Forms.Label labels = p as System.Windows.Forms.Label;
                if (labels != null)
                {
                    if (labels != null && labels.Text != "Alarm Zones" && labels.Text != "Alarm Delay" && labels.Text != "Total Time"
                        && labels.Text != "Events" && labels.Text != "First Triggered" && labels.Text != "Alarm Status")
                        labels.Text = string.Empty;
                }
            });
            lbDescText.Text = lbLimits.Text = lTripNum.Text = lDesc.Text = string.Empty;
            if (Tag == null ||Tag.RunStatus==0|| Tag.AlarmMode==0 )
                lbAlarm.Visible = lbDelayTime.Visible = lbTotalTime.Visible = lbNum.Visible = lbfirst.Visible = lbAlarmStatus.Visible = false;
            else
                lbAlarm.Visible = lbDelayTime.Visible = lbTotalTime.Visible = lbNum.Visible = lbfirst.Visible = lbAlarmStatus.Visible = true;
        }
        private string SetLoggerReader(SuperDevice Tag)
        {
            string result = string.Empty;
            string defaulTimeFormatString = string.Empty;
            if (string.IsNullOrEmpty(Tag.LoggerRead))
            {
                if (Tag.RunStatus != 0 && Tag.RunStatus != 1 && Tag.tempList.Count != 0)
                {
                    string reader = string.Empty;
                    if (Common.Versions == SoftwareVersions.Pro)
                    {
                        reader = "Logger Read: By {0}@{1}";
                    }
                    else
                    {
                        reader = "Logger Read: @{1}";
                    }
                    var now = DateTime.UtcNow;
                    result = string.Format(reader, Common.User.FullName, now.ToLocalTime().ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture));
                    defaulTimeFormatString = string.Format("{0}@{1}", Common.User.FullName, now.ToString(Common.GetDefaultDateTimeFormat(), CultureInfo.InvariantCulture));
                }
                else if (Tag.RunStatus == 1 || Tag.tempList.Count == 0)
                {
                    result = string.Format("Logger Read:");
                }
                Tag.LoggerRead = defaulTimeFormatString;
            }
            else
            {
                string loggerReader = Tag.LoggerRead;
                if (loggerReader == null)
                {
                    loggerReader = string.Empty;
                }
                string[] loggerReadereString = loggerReader.Split(new char[] { '@' });
                if (loggerReadereString.Length >= 2)
                {
                    if (Common.Versions == SoftwareVersions.S || string.IsNullOrWhiteSpace(loggerReadereString[0]))
                    {
                        result = string.Format("Logger Read: @{0}", TempsenFormatHelper.GetFormattedDateTime(Convert.ToDateTime(loggerReadereString[1])));
                    }
                    else if (Common.Versions == SoftwareVersions.Pro)
                    {
                        result = string.Format("Logger Read: By {0}@{1}", loggerReadereString[0], TempsenFormatHelper.GetFormattedDateTime(Convert.ToDateTime(loggerReadereString[1])));
                    }
                }
                else
                {
                    result = "Logger Read:";
                }
            }
            return result;
        }
        private void SetDynamicLabelText(object sender,EventArgs args)
        {
            System.Windows.Forms.Label lb = sender as System.Windows.Forms.Label;
            string text = lb.Tag as string;
            try
            {
                if (string.IsNullOrEmpty(text))
                    return;
                using (Graphics g = lb.CreateGraphics())
                {
                    int width = Common.GetTextPixel(text, g, lb.Font);
                    if (width > lb.Width)
                    {
                        string s = string.Empty;
                        int index = 0;
                        //text = text + "...";
                        for (int i = text.Length - 1; i >= 0; i--)
                        {
                            s = text.Substring(0, i) + "...";
                            index = i;
                            width = Common.GetTextPixel(s, g, lb.Font);
                            if (width > lb.Width)
                                continue;
                            else
                                break;
                        }
                        //lb.Text = (index - 3) <= 0 ? "" : text.Substring(0, index - 3) + "...";
                        lb.Text = s;
                    }
                    else
                        lb.Text = text;
                }
                if (lb.Tag == null)
                {
                    lb.Tag = text;
                }
                this.toolTip.SetToolTip(lb, text);
            }
            catch (Exception exp)
            {
                _tracing.Error(exp, "label set error");
            }
        }
        private void GetLoggerReaderText(object sender, EventArgs args)
        {
            System.Windows.Forms.Label lb = sender as System.Windows.Forms.Label;
            string text = lb.Tag as string;
            try
            {
                if (string.IsNullOrEmpty(text))
                    return;
                using (Graphics g = lb.CreateGraphics())
                {
                    int width = Common.GetTextPixel(text, g, lb.Font);
                    if (width > lb.Width)
                    {
                        string s = string.Empty;
                        int index = 0;
                        string[] reader = text.Split(new []{'@'});
                        if (reader.Length > 0)
                        {
                            for (int i = reader[0].Length - 1; i >= 0; i--)
                            {
                                s = reader[0].Substring(0, i) + "...@"+reader[1];
                                index = i;
                                width = Common.GetTextPixel(s, g, lb.Font);
                                if (width > lb.Width)
                                    continue;
                                else
                                    break;
                            }
                            lb.Text = s;
                        }
                        else
                            lb.Text = text;
                    }
                    else
                        lb.Text = text;
                }
                if (lb.Tag == null)
                {
                    lb.Tag = text;
                }
                this.toolTip.SetToolTip(lb, text);
            }
            catch (Exception exp)
            {
                _tracing.Error(exp, "label set error");
            }
        }
        #endregion

        private void pbUnfold_Click(object sender, EventArgs e)
        {
            tableViewTool.Visible = !tableViewTool.Visible;
            if (tableViewTool.Visible)
                pbUnfold.Image = Properties.Resources.graph_collapse_h;
            else
                pbUnfold.Image = Properties.Resources.graph_more_h;
        }
        #region 设备配置写入
        private void InitConfiguration()
        {
            /*判断显示那块panel*/
            if (rbSingleAlarm.Checked)
            {
                pnSingleAlarm.Visible = true;
            }
            else if (rbMultiAlarm.Checked)
            {
                pnMultiAlarm.Visible = true;
            }
            else
                pnMultiAlarm.Visible = pnMultiAlarm.Visible = false;
            rbC.Checked = Common.GlobalProfile.TempUnit == "C" ? true : false;
            rbF.Checked = Common.GlobalProfile.TempUnit == "F" ? true : false;
            InitDropDownList();
            InitConfigurateEvents();
            InitTextBox();
            InitDateTime();
            InitLableUnit();
            InitIdealRange();
        }
        private void InitConfigurateEvents()
        {
            rbNoAlarm.CheckedChanged += new EventHandler(SetAlarmPanel);
            rbMultiAlarm.CheckedChanged += new EventHandler(SetAlarmPanel);
            rbC.CheckedChanged += new EventHandler((sender, args) =>
            {
                InitLableUnit();
                SetTempTextBox(sender, args);
            });
            cbAutoGenerate.CheckedChanged += new EventHandler(SetTextBox);
            dpStartMode.SelectedValueChanged += new EventHandler(SetStartDelayPanel);
            cbA1.CheckedChanged += new EventHandler(SetAlarmEnable);
            cbA5.CheckedChanged += new EventHandler(SetAlarmEnable);
            cbA6.CheckedChanged += new EventHandler(SetAlarmEnable);
            cbLow.CheckedChanged += new EventHandler(SetAlarmEnable);
            cbHigh.CheckedChanged += new EventHandler(SetAlarmEnable);
            tbA1Temp.Leave += new EventHandler(CheckRegular);
            tbA3Temp.Leave += new EventHandler(CheckRegular);
            tbA4Temp.Leave += new EventHandler(CheckRegular);
            tbA5Temp.Leave += new EventHandler(CheckRegular);
            tbA6Temp.Leave += new EventHandler(CheckRegular);
            tbHigh.Leave += new EventHandler(CheckRegular);
            tbLow.Leave += new EventHandler(CheckRegular);
            tbTripNum.Leave += new EventHandler(SetTripNumTip);
            dpStartDelayD.SelectedValueChanged += new EventHandler(CheckDayOrHour);
            dpLogIntervalH.SelectedValueChanged += new EventHandler(CheckDayOrHour);
            dpLogIntervalH.SelectedValueChanged += new EventHandler((sender, args) => SetLogCycle());
            dpLogIntervalM.SelectedIndexChanged+=new EventHandler((sender,args)=>SetLogCycle());
            dpLogIntervalS.SelectedIndexChanged += new EventHandler((sender, args) => SetLogCycle());
            dpA1Day.SelectedValueChanged += new EventHandler(CheckDayOrHour);
            dpA2Day.SelectedValueChanged += new EventHandler(CheckDayOrHour);
            dpA4Day.SelectedValueChanged += new EventHandler(CheckDayOrHour);
            dpA5Day.SelectedValueChanged += new EventHandler(CheckDayOrHour);
            dpA6Day.SelectedValueChanged += new EventHandler(CheckDayOrHour);
            dpHighDay.SelectedValueChanged += new EventHandler(CheckDayOrHour);
            dpLowDay.SelectedValueChanged += new EventHandler(CheckDayOrHour);
            btnSaveConfigProfile.Click += new EventHandler(SaveTheConfigurationProfile);
            btnLoadConfigProfile.Click += new EventHandler(LoadTheConfigurationProfile);
            btnWriteConfig.Click += new EventHandler(WriteCfgToDevice);
            pnAlarmCfg.SizeChanged+=new EventHandler((sender,args)=>SetPosOfAlarm());
            tbDesc.TextChanged += new EventHandler(GetDescLength);
            this.tbHigh.TextChanged += new EventHandler( ConnectionController.GetTextChange);
            this.tbLow.TextChanged += new EventHandler(ConnectionController.GetTextChange);
            this.tbA1Temp.TextChanged += new EventHandler(ConnectionController.GetTextChange);
            this.tbA3Temp.TextChanged += new EventHandler(ConnectionController.GetTextChange);
            this.tbA4Temp.TextChanged += new EventHandler(ConnectionController.GetTextChange);
            this.tbA5Temp.TextChanged += new EventHandler(ConnectionController.GetTextChange);
            this.tbA6Temp.TextChanged += new EventHandler(ConnectionController.GetTextChange);
        }
        private void SetAlarmPanel(object sender,EventArgs args)
        {

            if (rbNoAlarm.Checked)
            {
                tableCfgPic.Visible = tableLayOutAlarmHeader.Visible = pnSingleAlarm.Visible = pnMultiAlarm.Visible = false;
                pnAlarmCfg.BackColor = Color.FromArgb(240, 240, 240);
               
            }
            if (rbSingleAlarm.Checked)
            {
                tableLayOutAlarmHeader.Visible = pnSingleAlarm.Visible = true;
                tableCfgPic.Visible = pnMultiAlarm.Visible = false;
                pnAlarmCfg.BackColor = Color.Transparent;
                 lbAlarmCfgLimits.Text = "Alarm Zones";
            }
            if (rbMultiAlarm.Checked)
            {
                pnSingleAlarm.Visible = false;
                tableLayOutAlarmHeader.Visible = tableCfgPic.Visible = pnMultiAlarm.Visible = true;
                pnAlarmCfg.BackColor = Color.Transparent;
                lbAlarmCfgLimits.Text = "Alarm Zones";
            }
        }
        private void SetTextBox(object sender, EventArgs args)
        {
            if (cbAutoGenerate.Checked)
            {
                pbTripNum.Visible = false;
                DateTime now = DateTime.Now;
                tbTripNum.Text = ((char)(now.Year - 1946)).ToString() + ((char)(now.Month + 64)).ToString() + now.ToString("ddHHmmss");
            }
            tbTripNum.Enabled = !cbAutoGenerate.Checked;
            tbTripNum.Focus();
        }
        private void SetDateTime()
        {
            DateTime now = DateTime.Now.AddMinutes(15);
            now=now.AddSeconds(now.Second * -1);
            dtpDate.Value = now;
            dtpTime.Value = now;
        }
        private void SetAlarmEnable(object sender,EventArgs args)
        {
            CheckBox cb = (CheckBox)sender;
            bool isCheck=cb.Checked;
            switch (cb.Name)
            {
                case "cbHigh":
                    if (!isCheck && !cbLow.Checked)
                    {
                        cb.Checked = !isCheck;
                        Utils.ShowMessageBox(Messages.AtLeastChooseOneLimit, Messages.TitleError);
                        break;
                    }
                    if (!isCheck)
                        pbHigh.Visible = false;
                    tbHigh.Enabled = cmbHighSingleType.Enabled = dpHighDay.Enabled = dpHighHour.Enabled = dpHighMinitue.Enabled = isCheck;
                    break;
                case "cbLow":
                    if (!isCheck && !cbHigh.Checked)
                    {
                        cb.Checked = !isCheck;
                        Utils.ShowMessageBox(Messages.AtLeastChooseOneLimit, Messages.TitleError);
                        break;
                    }
                    if (!isCheck)
                        pbLow.Visible = false;
                    tbLow.Enabled = cmbLowSingleType.Enabled = dpLowDay.Enabled = dpLowHour.Enabled = dpLowMinitue.Enabled = isCheck;
                    break;
                case "cbA6":
                    if (!isCheck)
                        pbA6.Visible = false;
                    tbA6Temp.Enabled = dpAlarmTypeA6.Enabled = dpA6Day.Enabled = dpA6H.Enabled = dpA6M.Enabled = isCheck;
                    break;
                case "cbA5":
                    if (!isCheck)
                        pbA5.Visible = false;
                    tbA5Temp.Enabled = dpAlarmTypeA5.Enabled = dpA5Day.Enabled = dpA5H.Enabled = dpA5M.Enabled = isCheck;
                    break;
                default:
                    if (!isCheck)
                        pbA1.Visible = false;
                    tbA1Temp.Enabled = dpAlarmTypeA1.Enabled = dpA1Day.Enabled = dpA1H.Enabled = dpA1M.Enabled = isCheck;
                    break;
            }
        }
        private void SetAlarmEnable()
        {
            tbHigh.Enabled = cmbHighSingleType.Enabled = dpHighDay.Enabled = dpHighHour.Enabled = dpHighMinitue.Enabled = cbHigh.Checked;
            tbLow.Enabled = cmbLowSingleType.Enabled = dpLowDay.Enabled = dpLowHour.Enabled = dpLowMinitue.Enabled = cbLow.Checked;
            tbA6Temp.Enabled = dpAlarmTypeA6.Enabled = dpA6Day.Enabled = dpA6H.Enabled = dpA6M.Enabled = cbA6.Checked;
            tbA5Temp.Enabled = dpAlarmTypeA5.Enabled = dpA5Day.Enabled = dpA5H.Enabled = dpA5M.Enabled = cbA5.Checked;
            tbA1Temp.Enabled = dpAlarmTypeA1.Enabled = dpA1Day.Enabled = dpA1H.Enabled = dpA1M.Enabled = cbA1.Checked;
        }
        private void SetStartDelayPanel(object sender,EventArgs args)
        {
            if (dpStartMode.SelectedValue.ToString() == "Auto Start")
            {
                pnAutoStart.Visible = true;
                pnAutoStart.BringToFront();
                SetDateTime();
            }
            else
            {
                pnAutoStart.Visible = false;
                pnManualStart.BringToFront();
            }
        }
        private void SetTempTextBox(object sender, EventArgs e)
        {

            string pattern = "^[-+]?[0-9]+[/.]?[0-9]?$";
            string text = tbA1Temp.Text.Trim();
            if (!string.IsNullOrEmpty(text) && IsMatch(pattern, text))
            {
                tbA1Temp.Text = Common.TransferTemp(!rbC.Checked ? "C" : "F", text);
            }
            text = tbA3Temp.Text.Trim();
            if (!string.IsNullOrEmpty(text) && IsMatch(pattern, text))
            {
                tbA3Temp.Text = Common.TransferTemp(!rbC.Checked ? "C" : "F", text);
            }
            text = tbA4Temp.Text.Trim();
            if (!string.IsNullOrEmpty(text) && IsMatch(pattern, text))
            {
                tbA4Temp.Text = Common.TransferTemp(!rbC.Checked ? "C" : "F", text);
            }
            text = tbA5Temp.Text.Trim();
            if (!string.IsNullOrEmpty(text) && IsMatch(pattern, text))
            {
                tbA5Temp.Text = Common.TransferTemp(!rbC.Checked ? "C" : "F", text);
            }
            text = tbA6Temp.Text.Trim();
            if (!string.IsNullOrEmpty(text) && IsMatch(pattern, text))
            {
                tbA6Temp.Text = Common.TransferTemp(!rbC.Checked ? "C" : "F", text);
                text = tbA3Temp.Text.Trim();
            }
            text = tbHigh.Text.Trim();
            if (!string.IsNullOrEmpty(text) && IsMatch(pattern, text))
            {
                tbHigh.Text = Common.TransferTemp(!rbC.Checked ? "C" : "F", text);
            }
            text = tbLow.Text.Trim();
            if (!string.IsNullOrEmpty(text) && IsMatch(pattern, text))
            {
                tbLow.Text = Common.TransferTemp(!rbC.Checked ? "C" : "F", text);
            }
            
        }
        private void CheckRegular(object sender,EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            this.checkTextBoxWithRegular(tb);
            this.Validate();
            this.ValidateChildren();
        }
        private void checkTextBoxWithRegular(TextBox tb)
        {
            checkTextBoxWithRegular(new TextBox[] { tb });
        }

        private void checkTextBoxWithRegular(TextBox[] tbs)
        {
            foreach (var tb in tbs)
            {
                string pattern = "^[-+]?[0-9]+[/.]?[0-9]?$";
                this.SetPbTip(tb, IsMatch(pattern, tb.Text));
            }
        }

        private void CheckDayOrHour(object sender, EventArgs args)
        {
            ComboBox cb = (ComboBox)sender;
            if (cb == dpLogIntervalH)
            {
                if ((int)dpLogIntervalH.SelectedValue == 2)
                {
                    this.dpLogIntervalM.SelectedItem = 0;
                    //if (!dpLogIntervalS.Items.Contains(0))
                    //    dpLogIntervalS.Items.Add(0);
                    this.dpLogIntervalS.SelectedItem = 0;
                    dpLogIntervalS.Enabled = dpLogIntervalM.Enabled = false;
                }
                else
                {
                    dpLogIntervalS.Enabled = dpLogIntervalM.Enabled = true;
                    //dpLogIntervalS.Items.Remove(0);
                }
            }
            else if (cb == dpStartDelayD)
            {
                if ((int)dpStartDelayD.SelectedItem == 45)
                {
                    this.dpStartDelayH.SelectedItem = 0;
                    this.dpStartDelayM.SelectedItem = 0;
                    dpStartDelayM.Enabled = dpStartDelayH.Enabled = false;
                }
                else
                    dpStartDelayM.Enabled = dpStartDelayH.Enabled = true;
            }
            else if (cb == dpA1Day)
            {
                if ((int)cb.SelectedItem == 5)
                {
                    dpA1H.SelectedItem = dpA1M.SelectedItem = 0;
                    dpA1H.Enabled = dpA1M.Enabled = false;


                }
                else
                    dpA1H.Enabled = dpA1M.Enabled = true;
            }
            else if (cb == dpA2Day)
            {
                if ((int)cb.SelectedItem == 5)
                {
                    dpA2H.SelectedItem = dpA2M.SelectedItem = 0;
                    dpA2H.Enabled = dpA2M.Enabled = false;


                }
                else
                    dpA2H.Enabled = dpA2M.Enabled = true;
            }
            else if(cb == dpA4Day)
            {
                if ((int)cb.SelectedItem == 5)
                {
                    dpA4H.SelectedItem = dpA1M.SelectedItem = 0;
                    dpA4H.Enabled = dpA4M.Enabled = false;


                }
                else
                    dpA4H.Enabled = dpA4M.Enabled = true;
            }
            else if (cb == dpA5Day)
            {
                if ((int)cb.SelectedItem == 5)
                {
                    dpA5H.SelectedItem = dpA5M.SelectedItem = 0;
                    dpA5H.Enabled = dpA5M.Enabled = false;


                }
                else
                    dpA5H.Enabled = dpA5M.Enabled = true;
            }
            else if (cb == dpA6Day)
            {
                if ((int)cb.SelectedItem == 5)
                {
                    dpA6H.SelectedItem = dpA6M.SelectedItem = 0;
                    dpA6H.Enabled = dpA6M.Enabled = false;


                }
                else
                    dpA6H.Enabled = dpA6M.Enabled = true;
            }
            else if (cb == dpHighDay)
            {
                if ((int)cb.SelectedItem == 5)
                {
                    dpHighHour.SelectedItem = dpHighMinitue.SelectedItem = 0;
                    dpHighHour.Enabled = dpHighMinitue.Enabled = false;


                }
                else
                    dpHighHour.Enabled = dpHighMinitue.Enabled = true;
            }
            else if (cb == dpLowDay)
            {
                if ((int)cb.SelectedItem == 5)
                {
                    dpLowHour.SelectedItem = dpLowMinitue.SelectedItem = 0;
                    dpLowHour.Enabled = dpLowMinitue.Enabled = false;


                }
                else
                    dpLowHour.Enabled = dpLowMinitue.Enabled = true;
            }
        }
        private void SetPbTip(TextBox tb,bool isRight)
        {
            string configUnit = "C";
            if (this.rbF.Checked)
            {
                configUnit = "F";
            }
            ConnectionController.VerifyTheTempCfg(tb, isRight, tbA1Temp, tbA3Temp, tbA4Temp, tbA5Temp, tbA6Temp, tbHigh, tbLow
                , pbA1, pbA2, pbA4, pbA5, pbA6, pbHigh, pbLow, this.wrongTip, Tag, configUnit, this.cbHigh, this.cbLow, this.cbA6, this.cbA5, this.cbA1, this.rbSingleAlarm, this.rbMultiAlarm);
        }
        private void SetTripNumTip(object sender, EventArgs e)
        {
            string pattern = "^[a-zA-Z0-9]{1,10}$";
            bool isTripNumberValid = IsMatch(pattern,tbTripNum.Text.Trim());
            if (!isTripNumberValid)
            {
                pbTripNum.Visible = true;
                Common.SetToolTip(this.wrongTip, pbTripNum, Messages.TripNumberInvalid);
            }
            else
            {
                Common.ClearToolTip(this.wrongTip, this.pbTripNum);
                pbTripNum.Visible = false;
            }
        }
        private bool IsMatch(string patern,string text)
        {
            System.Text.RegularExpressions.Regex regex = new System.Text.RegularExpressions.Regex(patern);
            return regex.IsMatch(text);
        }
        private void GetDescLength(object sender, EventArgs args)
        {
            lbDescTip.Text = string.Format("{0}/{1} Characters",tbDesc.TextLength,Common.DescLength);
        }


        private string CheckStatusOfAllFields()
        {
            StringBuilder message = new StringBuilder();
            if (this.rbMultiAlarm.Checked)
            {
                this.checkTextBoxWithRegular(new TextBox[] { this.tbA6Temp, this.tbA5Temp, this.tbA4Temp, this.tbA3Temp, this.tbA1Temp });
                if (this.pbA6.Visible)
                {
                    message.Append(string.Format("A1: {0}{1}", Common.GetToolTip(this.wrongTip, this.pbA6), Environment.NewLine));
                }
                if (this.pbA5.Visible)
                {
                    message.Append(string.Format("A2: {0}{1}", Common.GetToolTip(this.wrongTip, this.pbA5), Environment.NewLine));
                }
                if (this.pbA4.Visible)
                {
                    message.Append(string.Format("A3: {0}{1}", Common.GetToolTip(this.wrongTip, this.pbA4), Environment.NewLine));
                }
                if (this.pbA2.Visible)
                {
                    message.Append(string.Format("A5: {0}{1}", Common.GetToolTip(this.wrongTip, this.pbA2), Environment.NewLine));
                }
                if (this.pbA1.Visible)
                {
                    message.Append(string.Format("A6: {0}{1}", Common.GetToolTip(this.wrongTip, this.pbA1), Environment.NewLine));
                }
            }
            if (this.rbSingleAlarm.Checked)
            {
                this.checkTextBoxWithRegular(new TextBox[] { this.tbHigh, this.tbLow });
                if (this.pbHigh.Visible)
                {
                    message.Append(string.Format("High limit: {0}{1}", Common.GetToolTip(this.wrongTip, this.pbHigh), Environment.NewLine));
                }
                if (this.pbLow.Visible)
                {
                    message.Append(string.Format("Low limit: {0}{1}", Common.GetToolTip(this.wrongTip, this.pbLow), Environment.NewLine));
                }
            }
            
            if (this.pbTripNum.Visible)
            {
                message.Append(string.Format("Trip number: {0}{1}", Common.GetToolTip(this.wrongTip, this.pbTripNum), Environment.NewLine));
            }
            if (dpStartMode.SelectedValue.ToString() == "Auto Start"&&dtpTime.Value<=DateTime.Now)
            {
                message.Append(string.Format("{0}{1}",Platform.Messages.SaveCfgTimeLess,Environment.NewLine));
            }
            message.Append(this.checkAllDelayTimeWithLogInterval());
            return message.ToString();
        }

        private void allDelayTimeSelectedValueChanged(object sender, EventArgs e)
        {
            this.checkAllDelayTimeWithLogInterval();
        }

        private string checkAllDelayTimeWithLogInterval()
        {
            StringBuilder result = new StringBuilder();
            try
            {
                int logInterval = (int)this.dpLogIntervalH.SelectedValue * 3600 + (int)this.dpLogIntervalM.SelectedValue * 60 + (int)this.dpLogIntervalS.SelectedValue;
                if (this.rbSingleAlarm.Checked)
                {
                    if (this.cbHigh.Checked)
                    {
                        int delayHigh = ((int)this.dpHighDay.SelectedValue * 24 * 60 + (int)this.dpHighHour.SelectedValue * 60 + (int)this.dpHighMinitue.SelectedValue) * 60;
                        if (((string)this.cmbHighSingleType.SelectedValue) == AlarmType.Single.ToString() &&  delayHigh == 0)
                        {
                            this.pbHighDelay.Visible = false;
                        }
                        else if (delayHigh < logInterval)
                        {
                            this.pbHighDelay.Visible = true;
                            Common.SetToolTip(this.wrongTip, this.pbHighDelay, Messages.AlarmDelayInvalid);
                            result.Append(string.Format("High limit delay: {0}{1}", Messages.AlarmDelayInvalid, Environment.NewLine));
                        }
                        else
                        {
                            this.pbHighDelay.Visible = false;
                        }
                    }
                    else
                    {
                        this.pbHighDelay.Visible = false;
                    }
                    if (this.cbLow.Checked)
                    {
                        int delayLow = ((int)this.dpLowDay.SelectedValue * 24 * 60 + (int)this.dpLowHour.SelectedValue * 60 + (int)this.dpLowMinitue.SelectedValue) * 60;
                        if (((string)this.cmbLowSingleType.SelectedValue) == AlarmType.Single.ToString() && delayLow == 0)
                        {
                            this.pbLowDelay.Visible = false;
                        }
                        else if (delayLow < logInterval)
                        {
                            this.pbLowDelay.Visible = true;
                            Common.SetToolTip(this.wrongTip, this.pbLowDelay, Messages.AlarmDelayInvalid);
                            result.Append(string.Format("Low limit delay: {0}{1}", Messages.AlarmDelayInvalid, Environment.NewLine));
                        }
                        else
                        {
                            this.pbLowDelay.Visible = false;
                        }
                    }
                    else
                    {
                        this.pbLowDelay.Visible = false;
                    }
                }
                if (this.rbMultiAlarm.Checked)
                {
                    if (this.cbA6.Checked)
                    {
                        int delayA6 = ((int)this.dpA6Day.SelectedValue * 24 * 60 + (int)this.dpA6H.SelectedValue * 60 + (int)this.dpA6M.SelectedValue) * 60;
                        if (((string)this.dpAlarmTypeA6.SelectedValue) == AlarmType.Single.ToString() && delayA6 == 0)
                        {
                            this.pbA6Delay.Visible = false;
                        }
                        else if (delayA6 < logInterval)
                        {
                            this.pbA6Delay.Visible = true;
                            Common.SetToolTip(this.wrongTip, this.pbA6Delay, Messages.AlarmDelayInvalid);
                            result.Append(string.Format("A1 limit delay: {0}{1}", Messages.AlarmDelayInvalid, Environment.NewLine));
                        }
                        else
                        {
                            this.pbA6Delay.Visible = false;
                        }
                    }
                    else
                    {
                        this.pbA6Delay.Visible = false;
                    }
                    if (this.cbA5.Checked)
                    {
                        int delayA5 = ((int)this.dpA5Day.SelectedValue * 24 * 60 + (int)this.dpA5H.SelectedValue * 60 + (int)this.dpA5M.SelectedValue) * 60;
                        if (((string)this.dpAlarmTypeA5.SelectedValue) == AlarmType.Single.ToString() && delayA5 == 0)
                        {
                            this.pbA5Delay.Visible = false;
                        }
                        else if (delayA5 < logInterval)
                        {
                            this.pbA5Delay.Visible = true;
                            Common.SetToolTip(this.wrongTip, this.pbA5Delay, Messages.AlarmDelayInvalid);
                            result.Append(string.Format("A2 limit delay: {0}{1}", Messages.AlarmDelayInvalid, Environment.NewLine));
                        }
                        else
                        {
                            this.pbA5Delay.Visible = false;
                        }
                    }
                    else
                    {
                        this.pbA5Delay.Visible = false;
                    }   
                    int delayA4 = ((int)this.dpA4Day.SelectedValue * 24 * 60 + (int)this.dpA4H.SelectedValue * 60 + (int)this.dpA4M.SelectedValue) * 60;
                    if (((string)this.dpAlarmTypeA4.SelectedValue) == AlarmType.Single.ToString() && delayA4 == 0)
                    {
                        this.pbA4Delay.Visible = false;
                    }
                    else if (delayA4 < logInterval)
                    {
                        this.pbA4Delay.Visible = true;
                        Common.SetToolTip(this.wrongTip, this.pbA4Delay, Messages.AlarmDelayInvalid);
                        result.Append(string.Format("A3 limit delay: {0}{1}", Messages.AlarmDelayInvalid, Environment.NewLine));
                    }
                    else
                    {
                        this.pbA4Delay.Visible = false;
                    }
                    int delayA2 = ((int)this.dpA2Day.SelectedValue * 24 * 60 + (int)this.dpA2H.SelectedValue * 60 + (int)this.dpA2M.SelectedValue) * 60;
                    if (((string)this.dpAlarmTypeA2.SelectedValue) == AlarmType.Single.ToString() && delayA2 == 0)
                    {
                        this.pbA2Delay.Visible = false;
                    }
                    else if (delayA2 < logInterval)
                    {
                        this.pbA2Delay.Visible = true;
                        Common.SetToolTip(this.wrongTip, this.pbA2Delay, Messages.AlarmDelayInvalid);
                        result.Append(string.Format("A5 limit delay: {0}{1}", Messages.AlarmDelayInvalid, Environment.NewLine));
                    }
                    else
                    {
                        this.pbA2Delay.Visible = false;
                    }
                    if (this.cbA1.Checked)
                    {
                        int delayA1 = ((int)this.dpA1Day.SelectedValue * 24 * 60 + (int)this.dpA1H.SelectedValue * 60 + (int)this.dpA1M.SelectedValue) * 60;
                        if (((string)this.dpAlarmTypeA1.SelectedValue) == AlarmType.Single.ToString() && delayA1 == 0)
                        {
                            this.pbA1Delay.Visible = false;
                        }
                        else if (delayA1 < logInterval)
                        {
                            this.pbA1Delay.Visible = true;
                            Common.SetToolTip(this.wrongTip, this.pbA1Delay, Messages.AlarmDelayInvalid);
                            result.Append(string.Format("A6 limit delay: {0}{1}", Messages.AlarmDelayInvalid, Environment.NewLine));
                        }
                        else
                        {
                            this.pbA1Delay.Visible = false;
                        }
                    }
                    else
                    {
                        this.pbA1Delay.Visible = false;
                    }
                }
            }
            catch (Exception)
            {
                
            }
            
            return result.ToString();
        }

        private void SaveTheConfigurationProfile(object sender,EventArgs args)
        {
            string message = this.CheckStatusOfAllFields();
            if (!string.IsNullOrEmpty(message))
            {
                Utils.ShowMessageBox(message, Messages.TitleError);
                return;
            }
            else
            {
                SetLogCycle();
                ConfigurationProfile configuration = GetCfg();
                SaveFileDialog file = new SaveFileDialog();
                //file.FileName = tbTripNum.Text.Trim();
                file.Filter = "Configuration Profile(.cfg)|*.cfg";
                Common.SetDefaultPathForSaveFileDialog(file, SavingFileType.DeviceConfig);
                if (file.ShowDialog() == DialogResult.OK)
                {
                    byte[] cfg=Platform.Utils.SerializeToXML<ConfigurationProfile>(configuration);
                    if(cfg!=null)
                        Platform.Utils.SaveTheFile(cfg, file.FileName);
                }
            }
        }
        private void SetLogCycle()
        {
            if (Tag == null)
                return;
            int inverval=Convert.ToInt32(dpLogIntervalH.SelectedValue)*3600+Convert.ToInt32(dpLogIntervalM.SelectedValue)*60+Convert.ToInt32(dpLogIntervalS.SelectedValue);
            int cycle=Tag.Memory*inverval;
            lbLogCycle.Text = TempsenFormatHelper.ConvertSencondToFormmatedTime(cycle.ToString());
        }
        private void SetPosOfAlarm()
        {
            //int height = pnAlarmCfg.Size.Height;
            //int y = (height - pnMultiAlarm.Height) / 2;
            //pnMultiAlarm.Location = new Point(pnMultiAlarm.Location.X, y);
            //pnSingleAlarm.Location = new Point(pnSingleAlarm.Location.X,y);
            //tableCfgPic.Location = new Point(tableCfgPic.Location.X,y);
        }
        private ConfigurationProfile GetCfg()
        {
            ConfigurationProfile configuration = new ConfigurationProfile();
            configuration.Tn = tbTripNum.Text.Trim();
            configuration.Desc = tbDesc.Text.Trim();
            configuration.TempUnit = rbC.Checked ? "C" : "F";
            configuration.Owner = Common.User.UserName;
            configuration.WriteTime = DateTime.Now;
            configuration.LogCycle = lbLogCycle.Text.Trim();
            configuration.LogIntervalH = (int)dpLogIntervalH.SelectedValue;
            configuration.LogIntervalM = (int)dpLogIntervalM.SelectedValue;
            configuration.LogIntervalS = (int)dpLogIntervalS.SelectedValue;
            configuration.StartMode = (string)dpStartMode.SelectedValue;
            configuration.StartDelayD = (int)dpStartDelayD.SelectedValue;
            configuration.StartDelayH = (int)dpStartDelayH.SelectedValue;
            configuration.StartDelayM = (int)dpStartDelayM.SelectedValue;
            configuration.StartDate = ((DateTime)dtpDate.Value).ToShortDateString() + " " + ((DateTime)dtpTime.Value).ToLongTimeString();
            configuration.IsNoAlarm = rbNoAlarm.Checked;
            configuration.IsSingleAlarm = rbSingleAlarm.Checked;
            configuration.IsMultiAlarm = rbMultiAlarm.Checked;
            configuration.IsA1 = cbA1.Checked;
            configuration.IsA5 = cbA5.Checked;
            configuration.IsA6 = cbA6.Checked;
            configuration.IsHighLimit = cbHigh.Checked;
            configuration.IsLowLimit = cbLow.Checked;
            configuration.A1Temp = tbA1Temp.Text.Trim();
            configuration.A3Temp = tbA3Temp.Text;
            configuration.A4Temp = tbA4Temp.Text;
            configuration.A5Temp = tbA5Temp.Text;
            configuration.A6Temp = tbA6Temp.Text;
            configuration.HighTemp = tbHigh.Text;
            configuration.LowTemp = tbLow.Text;
            configuration.A1AlarmType = (string)dpAlarmTypeA1.SelectedValue;
            configuration.A2AlarmType = (string)dpAlarmTypeA2.SelectedValue;
            configuration.A4AlarmType = (string)dpAlarmTypeA4.SelectedValue;
            configuration.A5AlarmType = (string)dpAlarmTypeA5.SelectedValue;
            configuration.A6AlarmType = (string)dpAlarmTypeA6.SelectedValue;
            configuration.HighAlarmType = (string)cmbHighSingleType.SelectedValue;
            configuration.LowAlarmType = (string)cmbLowSingleType.SelectedValue;
            configuration.A1Day = (int)dpA1Day.SelectedValue;
            configuration.A2Day = (int)dpA2Day.SelectedValue;
            configuration.A4Day = (int)dpA4Day.SelectedValue;
            configuration.A5Day = (int)dpA5Day.SelectedValue;
            configuration.A6Day = (int)dpA6Day.SelectedValue;
            configuration.HighDay = (int)dpHighDay.SelectedValue;
            configuration.LowDay = (int)dpLowDay.SelectedValue;

            configuration.A1H = (int)dpA1H.SelectedValue;
            configuration.A2H = (int)dpA2H.SelectedValue;
            configuration.A4H = (int)dpA4H.SelectedValue;
            configuration.A5H = (int)dpA5H.SelectedValue;
            configuration.A6H = (int)dpA6H.SelectedValue;
            configuration.HighH = (int)dpHighHour.SelectedValue;
            configuration.LowH = (int)dpLowHour.SelectedValue;

            configuration.A1M = (int)dpA1M.SelectedValue;
            configuration.A2M = (int)dpA2M.SelectedValue;
            configuration.A4M = (int)dpA4M.SelectedValue;
            configuration.A5M = (int)dpA5M.SelectedValue;
            configuration.A6M = (int)dpA6M.SelectedValue;
            configuration.HighM = (int)dpHighMinitue.SelectedValue;
            configuration.LowM = (int)dpLowMinitue.SelectedValue;
            return configuration;
        }
        private void LoadTheConfigurationProfile(object sender,EventArgs args)
        {
            OpenFileDialog file = new OpenFileDialog();
            Common.SetDefaultPathForOpenFileDialog(file, OpenFileType.OpenDeviceConfig);
            file.Filter = "Configuration Profile(.cfg)|*.cfg";
            if (DialogResult.OK == file.ShowDialog())
            {
                try
                {
                    byte[] source = Platform.Utils.ReadFromFile(file.FileName);
                    if (source != null)
                    {
                        ConfigurationProfile cfg = Platform.Utils.DeserializeFromXML<ConfigurationProfile>(source, typeof(ConfigurationProfile));
                        if (cfg != null)
                        {
                            /*根据当前页面上的单位来显示配置*/
                            if (cfg.TempUnit == "C")
                                rbTempUnitC.Checked = true;
                            else
                                rbTempUnitF.Checked = true;
                            /*设置checkbox*/
                            cbA1.Checked = cfg.IsA1;
                            cbA5.Checked = cfg.IsA5;
                            cbA6.Checked = cfg.IsA6;
                            cbHigh.Checked = cfg.IsHighLimit;
                            cbLow.Checked = cfg.IsLowLimit;
                            /*设置是否手动或者自动*/
                            dpStartMode.SelectedItem = cfg.StartMode;
                            /*设置log interval*/
                            dpLogIntervalH.SelectedItem = cfg.LogIntervalH;
                            dpLogIntervalM.SelectedItem = cfg.LogIntervalM;
                            dpLogIntervalS.SelectedItem = cfg.LogIntervalS;
                            lbLogCycle.Text = cfg.LogCycle;
                            /*start delay*/
                            dpStartDelayD.SelectedItem = cfg.StartDelayD;
                            dpStartDelayH.SelectedItem = cfg.StartDelayH;
                            dpStartDelayM.SelectedItem = cfg.StartDelayM;
                            /*设置alarm setting*/
                            rbNoAlarm.Checked = cfg.IsNoAlarm;
                            rbSingleAlarm.Checked = cfg.IsSingleAlarm;
                            rbMultiAlarm.Checked = cfg.IsMultiAlarm;
                            /*设置温度*/
                            tbA1Temp.Text = cfg.A1Temp;
                            tbA5Temp.Text = cfg.A5Temp;
                            tbA6Temp.Text = cfg.A6Temp;
                            tbHigh.Text = cfg.HighTemp;
                            tbLow.Text = cfg.LowTemp;
                            /*设置alarm type*/
                            dpAlarmTypeA1.SelectedItem = cfg.A1AlarmType;
                            dpAlarmTypeA2.SelectedItem = cfg.A2AlarmType;
                            dpAlarmTypeA4.SelectedItem = cfg.A4AlarmType;
                            dpAlarmTypeA5.SelectedItem = cfg.A5AlarmType;
                            dpAlarmTypeA6.SelectedItem = cfg.A6AlarmType;
                            cmbHighSingleType.SelectedItem = cfg.HighAlarmType;
                            cmbLowSingleType.SelectedItem = cfg.LowAlarmType;
                            /*设置alarm delay day*/
                            dpA1Day.SelectedItem = cfg.A1Day;
                            dpA2Day.SelectedItem = cfg.A2Day;
                            dpA4Day.SelectedItem = cfg.A4Day;
                            dpA5Day.SelectedItem = cfg.A5Day;
                            dpA6Day.SelectedItem = cfg.A6Day;
                            dpHighDay.SelectedItem = cfg.HighDay;
                            dpLowDay.SelectedItem = cfg.LowDay;
                            /*设置alarm delay hour*/
                            dpA1H.SelectedItem = cfg.A1H;
                            dpA2H.SelectedItem = cfg.A2H;
                            dpA4H.SelectedItem = cfg.A4H;
                            dpA5H.SelectedItem = cfg.A5H;
                            dpA6H.SelectedItem = cfg.A6H;
                            dpHighHour.SelectedItem = cfg.HighH;
                            dpLowHour.SelectedItem = cfg.LowH;
                            /*设置alarm delay minitue*/
                            dpA1M.SelectedItem = cfg.A1M;
                            dpA2M.SelectedItem = cfg.A2M;
                            dpA4M.SelectedItem = cfg.A4M;
                            dpA5M.SelectedItem = cfg.A5M;
                            dpA6M.SelectedItem = cfg.A6M;
                            dpHighMinitue.SelectedItem = cfg.HighM;
                            dpLowMinitue.SelectedItem = cfg.LowM;
                        }
                    }
                }
                catch { }
            }
        }
        private void WriteCfgToDevice(object sender,EventArgs args)
        {
            string message = this.CheckStatusOfAllFields();
            if (!string.IsNullOrEmpty(message))
            {
                Utils.ShowMessageBox(message, Messages.TitleError);
                return;
            }
            else if (rbSingleAlarm.Checked&&(pbHigh.Visible || pbLow.Visible
                || (cbHigh.Checked&&string.IsNullOrEmpty(tbHigh.Text))|| (cbLow.Checked&&string.IsNullOrEmpty(tbLow.Text))))
            {
                Utils.ShowMessageBox(Messages.HasIllegalConfiguration, Messages.TitleError);
                return;
            }
            else
            {
                if (Tag.WriteConfiguration(GetCfg()))
                {
                    Utils.ShowMessageBox(Messages.WriteConfigOk, Messages.TitleNotification);
                    SetTextBox(sender, args);
                }
                else
                    Utils.ShowMessageBox(Messages.WriteConfigFailed, Messages.TitleError);
            }
        }
        private void InitIdealRange()
        {
            this.tbA3Temp.Text = rbC.Checked ? "2" : Common.TransferTemp("C", "2");
            this.tbA4Temp.Text = rbC.Checked ? "8" : Common.TransferTemp("C", "8");
        }
        public void InitDateTime()
        {
            List<string> datetime = Common.GlobalProfile.DateTimeFormator.Split(new char[] { ' ' }).ToList();
            dtpDate.CustomFormat = datetime[0];
            string timeformat = "";
            datetime.ForEach(p =>
            {
                if (p != datetime.First())
                {
                    timeformat = timeformat + p + " ";
                }
            });
            dtpTime.CustomFormat = timeformat;
            dtpTime.ShowUpDown = true;
            this.dtpDate.MinDate = DateTime.Now;
        }
        private void InitTextBox()
        {
            lbDescTip.Text =  string.Format("{0}/{1} Characters",0,Common.DescLength);
            tbTripNum.MaxLength = 10;
            DateTime now = DateTime.Now;
            tbTripNum.Text = ((char)(now.Year - 1946)).ToString() + ((char)(now.Month + 64)).ToString() + now.ToString("ddHHmmss");
        }
        private void InitLableUnit()
        {
            this.lbA1Unit.Text = rbC.Checked ? "°C" : "°F";
            this.lbA2Unit.Text = rbC.Checked ? "°C" : "°F";
            this.lbA4Unit.Text = rbC.Checked ? "°C" : "°F";
            this.lbA5Unit.Text = rbC.Checked ? "°C" : "°F";
            this.lbA6Unit.Text = rbC.Checked ? "°C" : "°F";
            this.lbHighUnit.Text = rbC.Checked ? "°C" : "°F";
            this.lbLowUnit.Text = rbC.Checked ? "°C" : "°F";
        }
        private void InitDropDownList()
        {
            /*对所有的控件有初始值进行初始化*/
            List<int> hour = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23 };
            List<int> LogHour = new List<int>() { 0, 1, 2 };
            List<int> LogMinitue = new List<int>();
            this.LogSecond = new List<int>() { 10, 20, 30, 40, 50,0 };
            List<int> startDelayDay = new List<int>();
            List<int> alarmDelayDay = new List<int>() { 0, 1, 2, 3, 4, 5 };
            List<int> alarmDelayMinitue = new List<int>();
            for (int i = 0; i < 60; i++)
                LogMinitue.Add(i);
            for (int i = 0; i <= 45; i++)
                startDelayDay.Add(i);
            for (int i = 1; i < 60; i++)
                alarmDelayMinitue.Add(i);
            alarmDelayMinitue.Add(0);
            dpLogIntervalH.DataSource = new List<int>(LogHour);
            dpLogIntervalM.DataSource = new List<int>(LogMinitue);
            dpLogIntervalS.DataSource = new List<int>( LogSecond);
            dpStartDelayD.DataSource = new List<int>(startDelayDay);
            dpStartDelayH.DataSource = new List<int>(hour);
            dpStartDelayM.DataSource = new List<int>(LogMinitue);
            dpHighDay.DataSource = new List<int>(alarmDelayDay);
            dpLowDay.DataSource = new List<int>(alarmDelayDay);
            dpHighHour.DataSource = new List<int>(hour);
            dpLowHour.DataSource = new List<int>(hour);
            dpHighMinitue.DataSource = new List<int>(alarmDelayMinitue);
            dpLowMinitue.DataSource = new List<int>(alarmDelayMinitue);
            dpA6Day.DataSource = new List<int>(alarmDelayDay);
            dpA5Day.DataSource = new List<int>(alarmDelayDay);
            dpA4Day.DataSource = new List<int>(alarmDelayDay);
            dpA2Day.DataSource = new List<int>(alarmDelayDay);
            dpA1Day.DataSource = new List<int>(alarmDelayDay);

            dpA6H.DataSource = new List<int>(hour);
            dpA5H.DataSource = new List<int>(hour);
            dpA4H.DataSource = new List<int>(hour);
            dpA2H.DataSource = new List<int>(hour);
            dpA1H.DataSource = new List<int>(hour);

            dpA6M.DataSource = new List<int>(alarmDelayMinitue);
            dpA5M.DataSource = new List<int>(alarmDelayMinitue);
            dpA4M.DataSource = new List<int>(alarmDelayMinitue);
            dpA2M.DataSource = new List<int>(alarmDelayMinitue);
            dpA1M.DataSource = new List<int>(alarmDelayMinitue);

            List<string> startMode = new List<string>() { "Manual Start", "Auto Start" };
            List<string> alarmType = new List<string>() { "Single", "Cumulative" };
            dpAlarmTypeA6.DataSource = new List<string>(alarmType);
            dpAlarmTypeA5.DataSource = new List<string>(alarmType);
            dpAlarmTypeA4.DataSource = new List<string>(alarmType);
            dpAlarmTypeA2.DataSource = new List<string>(alarmType);
            dpAlarmTypeA1.DataSource = new List<string>(alarmType);
            cmbHighSingleType.DataSource = new List<string>(alarmType);
            cmbLowSingleType.DataSource = new List<string>(alarmType);
            dpStartMode.DataSource = new List<string>(startMode);
        }
        
        #endregion

        private void lblReportEditorTip_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowAdminForm(sender, e);
        }

        #region 检查log interval下拉框之间的关系
        private List<int> LogSecond;

        private void checkDpLogIntervalStatus()
        {
            int logIntervalH = 0;
            int logIntervalM = 0;
            int logIntervalS = 0;
            var intervalSList = this.dpLogIntervalS.DataSource as IList<int>;
            try
            {
                logIntervalH = (int)this.dpLogIntervalH.SelectedItem;
                logIntervalM = (int)this.dpLogIntervalM.SelectedItem;
                logIntervalS = (int)this.dpLogIntervalS.SelectedItem;
            }
            catch
            {
            }
            if (logIntervalH == 0 && logIntervalM == 0)
            {
                if (logIntervalS == 0)
                {
                    this.dpLogIntervalS.SelectedItem = this.LogSecond.Where<int>(p => p == 10).FirstOrDefault();
                }
                if (intervalSList != null)
                {
                    intervalSList.Remove(0);
                }
            }
            else
            {
                if (intervalSList != null && !intervalSList.Contains(0))
                {
                    intervalSList.Add(0);
                }
            }
            this.dpLogIntervalS.DataSource = null;
            this.dpLogIntervalS.DataSource = intervalSList;
        }

        private void initDpLogIntervalStatusEvents()
        {
            this.dpLogIntervalH.SelectedValueChanged +=new EventHandler((sender, args) => {
                this.checkDpLogIntervalStatus();
            });
            this.dpLogIntervalM.SelectedValueChanged += new EventHandler((sender, args) =>
            {
                this.checkDpLogIntervalStatus();
            });
        }

        #endregion

        private string oldConfigDescription;
        private void tbDesc_Change(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null)
            {
                if (!Utils.IsInputTextValid(tb, this.oldConfigDescription))
                {
                    return;
                }
                string textTrimEnd = tb.Text.TrimEnd();
                using (Graphics g = tb.CreateGraphics())
                {
                    int actualWidth = (int)Math.Ceiling(g.MeasureString(textTrimEnd, tb.Font).Width);

                    if (actualWidth > tb.ClientSize.Width - 4)
                    {
                        tb.Text = this.oldConfigDescription;
                        tb.SelectionStart = tb.Text.Length;
                    }
                    else
                    {
                        this.oldConfigDescription = textTrimEnd;
                        //tb.SelectionStart = tb.Text.Length;
                    }
                }
            }
            
            
        }

        public DialogResult DeviceManagerExitDialog()
        {
            return DeviceManagerExitDialog(MessageBoxButtons.YesNoCancel);
        }

        public DialogResult DeviceManagerExitDialog(MessageBoxButtons buttons)
        {
            int result = Common.DeviceModificationType(Tag, tbCmt, tbReportTitle);
            if (result != -1)
            {
                string m = result == 0 ? Messages.B19 : Messages.B20;
                DialogResult dialogResult = Utils.ShowMessageBox(m, Messages.TitleNotification, buttons);
                if (DialogResult.Yes == dialogResult || DialogResult.OK == dialogResult)
                {
                    this.Save();
                }
                return dialogResult;
            }
            else
                return DialogResult.OK;
        }

        public bool IsDeviceNull
        {
            get
            {
                bool result = false;
                if (this.Tag == null)
                {
                    result = true;
                }
                else if (this.Tag.tempList.Count == 0)
                {
                    result = true;
                }
                return result;
            }
        }

        public bool IsDeviceModified()
        {
            bool result = false;
            if (this.Tag != null && this.tbCmt != null && this.tbReportTitle != null)
            {
                result = Common.IsDeviceModification(this.Tag, this.tbCmt, this.tbReportTitle);
            }
            return result;
        }
        private void DrawPanelBorderPaint(object sender, PaintEventArgs e)
        {
            Panel panel=sender as Panel;
            Brush brush = new SolidBrush(Color.FromArgb(102, 153, 255));
            using (Pen pen = new Pen(brush, 1F))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
            }
        }
    }
    public enum RedimStatus { Center=0,West,East,None }
    public enum AlarmType { Single, Cumulative }
    public struct Rec {
        private int x;
        public int X
        {
            get { return x; }
        }
        private int y;
        public int Y
        {
            get { return y; }
        }
        private int width;
        public int Width
        {
            get { return width; }
        }
        private int height;
        public int Height
        {
            get { return height; }
        }
        public Rec(Rectangle MouseRec)
        {
            int x1 = MouseRec.X < MouseRec.Right ? MouseRec.X : MouseRec.Right;//左边的坐标
            x = x1;
            y = MouseRec.Y;
            width = Math.Abs(MouseRec.Width);
            height = MouseRec.Height;
        }
    }
}
