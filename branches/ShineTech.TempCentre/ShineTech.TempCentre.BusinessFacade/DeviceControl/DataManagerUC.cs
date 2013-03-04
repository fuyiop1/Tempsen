using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.DAL;
using ZedGraph;
using System.Configuration;
using System.IO;
using ShineTech.TempCentre.Platform;
using System.Drawing.Printing;
using ShineTech.TempCentre.Versions;
using System.Globalization;
using System.Diagnostics;
using ShineTech.TempCentre.BusinessFacade.ViewModel;
namespace ShineTech.TempCentre.BusinessFacade
{
    public partial class DataManagerUC : UserControl
    {
        #region properity
        private DeviceBLL _deviceBll = new DeviceBLL();
        private PointTempBLL _pointTempBll = new PointTempBLL();
        private LogConfigBLL _logConfigBll = new LogConfigBLL();
        private AlarmConfigBLL _alarmConfigBll = new AlarmConfigBLL();
        private ReportEditorBLL _reportEditorBll = new ReportEditorBLL();
        private DigitalSignatureBLL _digitalSignBll = new DigitalSignatureBLL();
        private Device _device;
        private LogConfig _logConfig;
        private PointInfo _pointInfo;
        private List<AlarmConfig> _alarmConfig = new List<AlarmConfig>();
        private List<AlarmConfig> alarmConfigList = new List<AlarmConfig>();
        private List<Device> deviceList = new List<Device>();
        private List<LogConfig> logConfigList = new List<LogConfig>();
        private List<PointInfo> pointInfoList = new List<PointInfo>();
        private List<PointKeyValue> pointKeyValueList = new List<PointKeyValue>();
        private List<DigitalSignature> digitalSignList = new List<DigitalSignature>();
        private ReportEditor editor = new ReportEditor();
        public event EventHandler ShowAdminForm;
        public bool IsCompare;
        private int _YAxisLength;
        private RowMergeView dgvSummary;
        private SuperDevice Tag;
        private List<SuperDevice> TagsList = new List<SuperDevice>();
        private char[] _DescSplit = new char[] { '_', '\n' };
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
        public Panel PnSignHis
        {
            get { return pnSignHis; }
        }
        public Size ViewSize
        {
            get { return pnView.Size; }
            set { pnView.Size = value; }
        }
        private PrintPreviewDialog printPreviewDialog = Utils.GetPrintPreviewDialogue();

        public int YAxisLength
        {
            get
            {
                if (_YAxisLength == 0)
                {
                    object o = ConfigurationManager.AppSettings["YAxisLength"];
                    _YAxisLength = o == null ? 10 : Convert.ToInt32(o);
                }
                return _YAxisLength;
            }
        }
        public void SetPanelVisible(bool visible)
        {
            pnHis.Visible = visible;
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
        public System.Windows.Forms.TextBox TbCmt
        {
            get { return tbCmt; }
            set { tbCmt = value; }
        }
        //private LineItem highLimit;
        //private LineItem lowLimit;
        //private LineItem idealHigh;
        private RedimStatus redimStatus;//鼠标在矩形区域内的位置
        private bool IsRectanglePainted = false;//矩形是否完成
        private bool IsLeftSide = false;//是否拖动左边框
        private int MouseDownX, MouseDownY;
        private Rec rec;//画好后的rectangle保存坐标及长宽
        private bool IsMouseDown;
        private Rectangle MouseRec;
        private ToolTip toolTip = new ToolTip();
        private XAxisVisibility selection = XAxisVisibility.DataPoints;
        #endregion
        private OperationLogBLL logBll = new OperationLogBLL();
        private bool IsSaved = true;
        private TextBox tbReportTitle = new TextBox();
        private TextBox tbReportComment = new TextBox();
        private string tbReportCommentString;
        private ReportDataGenerator reportdataGenerator = new ReportDataGenerator();
        private bool isRefeshing;
        private bool m_IsRbCompareCheckedChanging;
        private bool m_IsHoverDetailedHeader = false;
        public DataManagerUC()
        {
            Application.CurrentCulture = CultureInfo.InvariantCulture;
            InitializeComponent();
            InitCtor();
        }
        private void InitCtor()
        {
            initializeTbReportComment();
            InitEvents();//事件
            ClearLabelContents();//清空label
            InitHistoryData();//左边历史
            Intelligence();//智能搜索
            InitContextMenu();//左键删除菜单
            InitViewTool();//绘图小工具
            if (Common.Versions == SoftwareVersions.S)
            {
                lblReportEditorTip.Visible = false;
            }
            if (this.dgvList.MergeColIsShow == null)
            {
                dgvList.MergeColIsShow = new Dictionary<string, bool>();
            }
            pnViewClick.BackgroundImage = Utils.DrawTextOnImage(Properties.Resources.wk_vm, "View Manager", 50, 9);
            Common.SetToolTip(toolTip, pbBackToList, "Normal List");
        }
        private void initializeTbReportComment()
        {
            this.tbReportComment.Width = 730;
            this.tbReportComment.Font = new Font("Arial", 9, GraphicsUnit.Pixel);
        }

        private void InitViewTool()
        {
            cbLimitLine.Checked = Common.GlobalProfile.IsShowAlarmLimit;
            cbIdealRange.Checked = Common.GlobalProfile.IsFillIdealRange;
            cbShowMark.Checked = Common.GlobalProfile.IsShowMark;
            rbDateTime.Click += new EventHandler((sender, args) =>
            {
                SetAxis(sender);
                if (!IsCompare)
                {
                    this.DrawLimitLine(zedGraphControl1.GraphPane, cbLimitLine.Checked, cbIdealRange.Checked);
                    DrawIdealRange(zedGraphControl1.GraphPane, cbIdealRange.Checked, cbLimitLine.Checked);
                }
                zedGraphControl1.Refresh();
            });
            rbDtaPoints.Click += new EventHandler((sender, args) =>
            {
                SetAxis(sender);
                if (!IsCompare)
                {
                    this.DrawLimitLine(zedGraphControl1.GraphPane, cbLimitLine.Checked, cbIdealRange.Checked);
                    DrawIdealRange(zedGraphControl1.GraphPane, cbIdealRange.Checked, cbLimitLine.Checked);
                }
                zedGraphControl1.Refresh();
            });
            rbElapsedTime.Click += new EventHandler((sender, args) =>
            {
                SetAxis(sender);
                if (!IsCompare)
                {
                    this.DrawLimitLine(zedGraphControl1.GraphPane, cbLimitLine.Checked, cbIdealRange.Checked);
                    DrawIdealRange(zedGraphControl1.GraphPane, cbIdealRange.Checked, cbLimitLine.Checked);
                }
                zedGraphControl1.Refresh();
            });
            pbUndo.Click += new EventHandler((sender, args) =>
            {
                zedGraphControl1.ZoomOutAll(zedGraphControl1.GraphPane);
                InitDataManager();
                MouseRec = Rectangle.Empty;
                GraphHelper.ReDraw(true, zedGraphControl1, ref MouseRec);
            });
            cbLimitLine.CheckedChanged += new EventHandler((sender, args) =>
            {
                this.DrawLimitLine(zedGraphControl1.GraphPane, cbLimitLine.Checked, cbIdealRange.Checked);
                zedGraphControl1.Refresh();
                GraphHelper.ReDraw(true, zedGraphControl1, ref MouseRec);
            });
            cbIdealRange.CheckedChanged += new EventHandler((sender, args) =>
            {
                DrawIdealRange(zedGraphControl1.GraphPane, cbIdealRange.Checked, cbLimitLine.Checked);
                zedGraphControl1.Refresh();
            });
            cbShowMark.CheckedChanged+=new EventHandler(DrawMark);
        }
        public void InitHistoryData()
        {
            this.TagsList.Clear();
            this.Tag = null;
            this.pointInfoList.Clear();
            alarmConfigList.Clear();
            GetHistoryDataSource();
            deviceList = _deviceBll.GetDeviceList().OrderByDescending(p => Convert.ToDateTime(p.Remark)).ToList();

            CtorDataTimeFilter();
            Dictionary<string, bool> dic = GetHistoryDataCheckState();
            CtorRecordList();
            SetHistoryDataCheckState(dic);
            this.InitDataManager();
        }
        public void ResetHistoryData()
        {
            deviceList = _deviceBll.GetDeviceList().OrderByDescending(p => Convert.ToDateTime(p.Remark)).ToList();
            if (deviceList != null && deviceList.Count > 0)
            {
                //InitFilter(deviceList);
                m_HistoryDataViewModel.Clear();
                CtorDataTimeFilter();
                CtorRecordList();
            }
            else
            {
                dgvHistory.Rows.Clear();
            }
        }
        private void InitFilter(List<Device> deviceList)
        {
            dtpHistoryFrom.Format = DateTimePickerFormat.Custom;
            dtpHistoryFrom.CustomFormat = Common.GlobalProfile.DateTimeFormator.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
            dtpHistoryTo.Format = DateTimePickerFormat.Custom;
            dtpHistoryTo.CustomFormat = Common.GlobalProfile.DateTimeFormator.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
            dtpHistoryFrom.Value = deviceList.Select(p => Convert.ToDateTime(p.Remark).ToLocalTime()).Min();
            dtpHistoryTo.Value = deviceList.Select(p => Convert.ToDateTime(p.Remark).ToLocalTime()).Max();
        }

        private void initFilterAfterOptionsChanged()
        {
            dtpHistoryFrom.Format = DateTimePickerFormat.Custom;
            dtpHistoryFrom.CustomFormat = Common.GlobalProfile.DateTimeFormator.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
            dtpHistoryTo.Format = DateTimePickerFormat.Custom;
            dtpHistoryTo.CustomFormat = Common.GlobalProfile.DateTimeFormator.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
        }

        private void InitGraph()
        {
            if (rbDateTime.Checked)
            {
                selection = XAxisVisibility.DateAndTime;
            }
            else if (rbDtaPoints.Checked)
                selection = XAxisVisibility.DataPoints;
            else
                selection = XAxisVisibility.ElapsedTime;
            GraphHelper.SetGraphAsDefaultProperity(zedGraphControl1, selection);
        }
        public void UnCompareDataList(string sn, string tn, bool IsShow)
        {
            //构造column
            if (dgvList.Columns.Count == 0)
            {
                GenerateFirstDataListColumn(sn, tn);
                BindSingleDataList(sn, tn);
            }
            else
            {
                if (IsShow)
                {
                    if (!dgvList.MergeColumnNames.Contains(sn + "_" + tn))
                    {
                        AddColumnToDataList(sn, tn);
                        BindSingleDataList(sn, tn);
                    }
                    else if (dgvList.MergeColIsShow[sn + "_" + tn] == true)
                    {
                        BindSingleDataList(sn, tn);
                    }
                }
                else
                {
                    if (dgvList.Columns.Count == 4)
                    {
                        dgvList.Rows.Clear();
                        dgvList.Columns.Clear();
                    }
                    else
                        dgvList.Columns[sn + "_" + tn].Visible = IsShow;
                }
            }
        }
        public void InitEvents()
        {

            this.pbUndo.MouseHover += new EventHandler((sender, args) => pbUndo.Image = Properties.Resources.graph_back_h);
            this.pbUndo.MouseLeave += new EventHandler((sender, args) => pbUndo.Image = Properties.Resources.graph_back);
            this.pbUnfold.MouseHover += new EventHandler((sender, args) => pbUnfold.Image = tableViewTool.Visible == false ? Properties.Resources.graph_more_h : Properties.Resources.graph_collapse_h);
            this.pbUnfold.MouseLeave += new EventHandler((sender, args) => pbUnfold.Image = tableViewTool.Visible == false ? Properties.Resources.graph_more : Properties.Resources.graph_collapse);
            this.dgvList.ColumnWidthChanged += new DataGridViewColumnEventHandler((sender, args) => dgvList.Refresh());
            this.btnSign.Click += new EventHandler(Sign);
            this.btnSave.Click += new EventHandler((sender, args) => Save());
            this.Load += new EventHandler((sender, args) =>
            {
                this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                this.SetStyle(ControlStyles.DoubleBuffer, true);
                this.SetStyle(ControlStyles.UserPaint, true);
                this.SetStyle(ControlStyles.ResizeRedraw, true);
            });

            this.tbReportTitle.TextChanged += new EventHandler(ButtonStateOnChange);
            this.tbCmt.TextChanged += new EventHandler(ButtonStateOnChange);
            this.dtpHistoryFrom.ValueChanged += new EventHandler(DataTimeFromChanged);
            this.dtpHistoryTo.ValueChanged += new EventHandler(DataTimeToChanged);
            this.cbElapsed.CheckedChanged += new EventHandler((a, b) => SetElapsedTimeShow(cbElapsed.Checked));
            this.cbDate.CheckedChanged += new EventHandler((a, b) =>
            {
                SetDateTimeShow(cbDate.Checked);
            });
            this.rbCompare.CheckedChanged += new EventHandler((sender, e) => ClickCompare());
            this.tbSearch.MouseUp += new MouseEventHandler((sender, e) =>
            {
                if (e.Button == MouseButtons.Left && (bool)tbSearch.Tag)
                    tbSearch.SelectAll();
                tbSearch.Tag = false;
            });
            this.tbSearch.MouseEnter += new EventHandler((sender, e) =>
                {
                    tbSearch.Tag = true;
                    this.tbSearch.SelectAll();
                });
            this.btnClear.Click += new EventHandler(OnClickClear);
            dgvHistory.CellContentClick += new DataGridViewCellEventHandler(DataGridCellContentClick);
            dgvFloatingHistory.CellContentClick += new DataGridViewCellEventHandler(DataGridCellContentClick);
            this.rbTempUnitC.CheckedChanged += new EventHandler(TempUnitChange);
            #region 电子签名显示记录
            this.pbArrowUp.Click += new EventHandler(ShowSignHistoryList);
            this.pbArrowDown.Click += new EventHandler((sender, args) => this.pnSignature.Visible = false);
            pbArrowUp.MouseHover += new EventHandler((sender, args) => pbArrowUp.Image = Properties.Resources.arrow_up_hover);
            pbArrowUp.MouseLeave += new EventHandler((sender, args) => pbArrowUp.Image = Properties.Resources.arrow_up);
            pbArrowDown.MouseHover += new EventHandler((sender, args) => pbArrowDown.Image = Properties.Resources.arrow_down_hover);
            pbArrowDown.MouseLeave += new EventHandler((sender, args) => pbArrowDown.Image = Properties.Resources.arrow_down);
            pbMax.MouseHover += new EventHandler(OnHoverFullScreen);
            pbMax.MouseLeave += new EventHandler(OnLeaveFullScreen);
            #endregion

            // 注册ReportEditor的事件
            reportCanvasPanel.MouseDown += new MouseEventHandler((o, e) => { reportCanvasPanel.Focus(); });
            this.zedGraphControl1.PointValueEvent += new ZedGraphControl.PointValueHandler(MyPointValueHandler);
            this.zedGraphControl1.MouseDownEvent += new ZedGraphControl.ZedMouseEventHandler(ZedOnMouseDown);
            this.zedGraphControl1.MouseMoveEvent += new ZedGraphControl.ZedMouseEventHandler(ZedOnMouseMove);
            this.zedGraphControl1.MouseUpEvent += new ZedGraphControl.ZedMouseEventHandler(ZedOnMouseUp);
            this.zedGraphControl1.GraphPane.XAxis.ScaleFormatEvent += new Axis.ScaleFormatHandler(XAxisScaleLabels);
            this.tpGraph.SizeChanged += new EventHandler((sender, args) => ChangeSizeOfGraph());
            //注册report title 和 comment文本框的事件
            ReportPublicControlEventInitializer.InitEventForTitleTextBox(this.tbReportTitle);
            ReportPublicControlEventInitializer.InitEventForCoupleTextBox(this.tbReportComment, this.tbCmt);
            ReportPublicControlEventInitializer.InitEventForCommentTextBox(this.tbCmt);
            ReportPublicControlEventInitializer.InitEventForCommentTextBox(this.tbReportComment);
            this.tbReportComment.TextChanged += new EventHandler(tbReportComment_TextChanged);
            this.lbSign.SizeChanged += new EventHandler((sender, args) => SetSignLabel(lbSign.Tag as string));
            //this.lDesc.SizeChanged += new EventHandler(SetDynamicLabelText);
            this.lbLoggerReader.SizeChanged += new EventHandler(GetLoggerReaderText);
            this.dgvHistory.SizeChanged += new EventHandler(InitDgvHistorySize);
            if (m_RightMouseAnalyse != null)
            {
                m_RightMouseAnalyse.Draw += new DrawToolTipEventHandler(RightMouseAnalyseDraw);
                m_RightMouseAnalyse.Popup += new PopupEventHandler(RightMouseAnalysePopUp);
            }

            this.pbBackToList.Click+=new EventHandler(BackToRecordListClick);
            this.dgvHistory.ColumnHeaderMouseClick += new DataGridViewCellMouseEventHandler(DetailedListClick);

            pbBackToList.MouseHover += new EventHandler((sender, args) => pbBackToList.Image=Properties.Resources.arrow_left_hover);
            pbBackToList.MouseLeave += new EventHandler((sender, args) => pbBackToList.Image = Properties.Resources.arrow_left);
            dgvHistory.CellMouseMove += new DataGridViewCellMouseEventHandler(OnHoverDetailedList);
            dgvHistory.CellMouseLeave += new DataGridViewCellEventHandler(OnLeaveDetailedList);
        }
        private void OnHoverFullScreen(object sender, EventArgs args)
        {
            if (pnHis.Visible)
            {
                pbMax.Image = Properties.Resources.fullscreen_hover;
            }
            else
            {
                pbMax.Image = Properties.Resources.fullscreen_exit_hover;
            }
        }
        private void OnLeaveFullScreen(object sender, EventArgs args)
        {
            if (pnHis.Visible)
            {
                pbMax.Image = Properties.Resources.fullscreen;
            }
            else
            {
                pbMax.Image = Properties.Resources.fullscreen_exit;
            }
        }
        private void OnHoverDetailedList(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.RowIndex==-1&&e.ColumnIndex==0)
            {
                m_IsHoverDetailedHeader = true;
                dgvHistory.Refresh();
            }
        }
        private void OnLeaveDetailedList(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1 && e.ColumnIndex == 0)
            {
                m_IsHoverDetailedHeader = false;
                dgvHistory.Refresh();
            }
        }
        private void ShowSignHistoryList(object sender, EventArgs args)
        {
            pnSignature.Visible = true;
            pnSignature.Parent = this;
            int x, y;
            x = pnView.Location.X + 12;
            y =  (pnView.Height + pnView.Location.Y)-pnSignature.Height-10;
            pnSignature.Location = new Point(x, y);
            pnSignature.BringToFront();
        }
        private void InitDgvHistorySize(object sender, EventArgs args)
        {
            if (this.dgvHistory != null && this.pnHis != null)
            {
                int expectedDgvHistoryHeight = 0;
                int marginBottom = 49;
                if (this.pnHis.Height != 0)
                {
                    expectedDgvHistoryHeight = this.pnHis.Height - this.dgvHistory.Location.Y - marginBottom;
                }
                if (this.dgvHistory.Height != 0 && this.dgvHistory.Height != expectedDgvHistoryHeight)
                {
                    this.dgvHistory.Height = expectedDgvHistoryHeight;
                }
            }
        }

        private void OnClickClear(object sender, EventArgs args)
        {
            ClearDataRecordsPanel();
        }

        private void ClearDataRecordsPanel()
        {
            ResetHistoryData();
            this.tbSearch.Text = Infrastructure.SearchRecordConst;
            dgvList.Rows.Clear();
            dgvList.Columns.Clear();
            dgvList.MergeColumnNames.Clear();
            dgvList.MergeColIsShow.Clear();
            if (!IsCompare)
            {
                tpSummary.Controls.Clear();
                tpSummary.Controls.Add(pnSummary);
            }
            else
            {
                this.tpSummary.Controls.Clear();
                tpSummary.Controls.Add(dgvSummary);
            }
            pnSignHis.Visible = Common.Versions == SoftwareVersions.Pro;
            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl1.GraphPane.GraphObjList.Clear();
            reportCanvasPanel.Controls.Clear();
            TagsList.Clear();
            Tag = null;
            ClearLabelContents();
            lvSignature.Rows.Clear();
            digitalSignList.Clear();
            lbSign.Text = "Unsigned";
            tbCmt.Text = Platform.ControlText.CommentText;
            GraphHelper.SetInitProperity(zedGraphControl1);
            zedGraphControl1.Refresh();
            clearToolTips();
        }

        private void clearToolTips()
        {
            if (this.tips != null)
            {
                this.tips.RemoveAll();
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

        public void InitTpReportEditor(bool isCommentAndTitleRefresh)
        {
            if (editor != null && isCommentAndTitleRefresh)
            {
                if (!string.IsNullOrWhiteSpace(editor.Comments) && ReportConstString.CommentDefaultString != editor.Comments)
                {
                    tbReportComment.Text = editor.Comments;
                }
                else
                {
                    tbReportComment.Text = ReportConstString.CommentDefaultString;
                }
                if (!string.IsNullOrWhiteSpace(editor.ReportTitle) && ReportConstString.TitleDefaultString != editor.ReportTitle)
                {
                    tbReportTitle.Text = editor.ReportTitle;
                }
                else
                {
                    tbReportTitle.Text = ReportConstString.TitleDefaultString;
                }
            }
            if (Tag != null && !string.IsNullOrWhiteSpace(Tag.SerialNumber) && Tag.tempList.Count > 0)
            {
                DigitalSignatureBLL _digitalBll = new DigitalSignatureBLL();
                ReportEditorExporter exporter = new ReportEditorExporter(DeviceDataFrom.DataManager, this.Tag, this.digitalSignList, this.reportCanvasPanel, this.tbReportTitle, this.tbReportComment, this.lblReportEditorTip);
                exporter.SignatureList = _digitalBll.GetDigitalSignatureBySnTn(Tag.SerialNumber, Tag.TripNumber);
                exporter.GenerateReport();
            }

        }

        public void InitTpReportEditor()
        {
            InitTpReportEditor(true);
        }
        public void SetSignSaveButtonState(SuperDevice Tag)
        {
            if (Tag == null)
                return;
            ConnectionController.SetSignButtonState(btnSign, Tag.tempList.Count == 0 ? false : true);
            ConnectionController.SetSaveButtonState(btnSave, Common.IsDeviceModification(Tag, tbCmt, tbReportTitle));
        }
        private void ButtonStateOnChange(object sender, EventArgs args)
        {
            if (Tag == null)
                return;
            ConnectionController.SetSaveButtonState(btnSave, Common.IsDeviceModification(Tag, tbCmt, tbReportTitle, editor));
        }
        private void UncompareDataHistoryShow(int SelectedIndex)
        {
            //history列表单选
            dgvHistory.Rows.Cast<DataGridViewRow>().ToList().ForEach(p =>
            {
                if (p != dgvHistory.Rows[SelectedIndex])
                {
                    p.Cells["CheckCol"].Value = false;
                    p.ReadOnly = false;
                }
                else
                    p.ReadOnly = true;
            });
            dgvFloatingHistory.Rows.Cast<DataGridViewRow>().ToList().ForEach(p =>
            {
                if (p != dgvFloatingHistory.Rows[SelectedIndex])
                {
                    p.Cells["CheckCol"].Value = false;
                    p.ReadOnly = false;
                }
                else
                    p.ReadOnly = true;
            });
        }
        public void RefreshDataManagerWithAllConditions()
        {
            if (deviceList == null)
                return;
            var v = from p in deviceList
                    where Convert.ToDateTime(p.Remark).ToLocalTime().Date >= dtpHistoryFrom.Value.Date &&
                          Convert.ToDateTime(p.Remark).ToLocalTime().Date <= dtpHistoryTo.Value.Date &&
                          string.Format("{0}_{1}", p.SerialNum, p.TripNum).Contains((string.IsNullOrWhiteSpace(this.tbSearch.Text) || Infrastructure.SearchRecordConst == this.tbSearch.Text) ? string.Empty : this.tbSearch.Text.Trim())
                    select p;
            if (v != null)
            {
                Dictionary<string, bool> dic = GetHistoryDataCheckState();
                CtorRecordList();
                SetHistoryDataCheckState(dic);
                ResetInitState();
                InitDataManager();
            }
        }

        private void TempUnitChange(object sender, EventArgs args)
        {
            InitDataManager(false);
        }
        public void InitDataManager(bool isCommentAndTitleRefresh)
        {
            if (isRefeshing)
            {
                return;
            }
            //zedGraphControl1.GraphPane.CurveList.Clear();
            GraphHelper.SetInitProperity(zedGraphControl1);
            zedGraphControl1.Refresh();
            dgvList.Rows.Clear();
            dgvList.Columns.Clear();
            dgvList.MergeColumnNames.Clear();
            if (dgvList.MergeColIsShow != null)
                dgvList.MergeColIsShow.Clear();
            //this.CtorRecordList();
            if ( !_IsFromTps && dgvHistory != null && dgvHistory.RowCount > 0)
            {
                //ForEachTempByUnit();
                for (int i = 0; i < dgvHistory.RowCount; i++)
                {
                    string sn_tn = dgvHistory.Rows[i].Cells["record"].Value.ToString();
                    if (sn_tn.LastIndexOf("\n") != -1)
                    {
                        sn_tn = sn_tn.Substring(0, sn_tn.LastIndexOf("\n"));
                    }
                    string[] sntn = new string[2] { string.Empty, string.Empty };
                    if (sn_tn.IndexOf('_') != -1)
                    {
                        sntn[0] = sn_tn.Substring(0, sn_tn.IndexOf('_'));
                        sntn[1] = sn_tn.Substring(sn_tn.IndexOf('_') + 1);
                    }
                    bool isShow = Convert.ToBoolean(dgvHistory.Rows[i].Cells["CheckCol"].EditedFormattedValue);
                    GetDeviceObjects(sntn[0], sntn[1]);
                    InitDeviceType(_device, _logConfig
                                     , _pointInfo, _alarmConfig, isShow);
                    if (isShow)
                    {
                        if (!IsCompare)
                        {
                            //UncompareDataHistoryShow(e.RowIndex);
                            UncompareDataListShow(sntn[0], sntn[1], isShow);
                            UnCompareDataList(sntn[0], sntn[1], isShow);
                            UnCompareGraph(sntn[0], sntn[1], isShow);
                            InitTpSingleSummary(Tag);
                            SetSignedRecordLabel(digitalSignList);
                            InitTpReportEditor(isCommentAndTitleRefresh);
                            SetSignSaveButtonState(Tag);
                            return;
                        }
                        else //compare状态下
                        {
                            CompareDataListShow(sntn[0], sntn[1], isShow);
                            CompareDataList(sntn[0], sntn[1], isShow);
                            CompareGraph(sntn[0], sntn[1], isShow);
                            CompareSummary(sntn[0], sntn[1], isShow);
                        }
                    }
                }
            }
            if (this._IsFromTps && Tag != null)
            {
                GetDeviceObjects(Tag.SerialNumber, Tag.TripNumber);
                InitDeviceType(_device, _logConfig
                                 , _pointInfo, _alarmConfig, true);
                UncompareDataListShow(Tag.SerialNumber, Tag.TripNumber, true);
                UnCompareDataList(Tag.SerialNumber, Tag.TripNumber, true);
                UnCompareGraph(Tag.SerialNumber, Tag.TripNumber, true);
                InitTpSingleSummary(Tag);
                SetSignedRecordLabel(digitalSignList);
                InitTpReportEditor(isCommentAndTitleRefresh);
            }
            this.initFilterAfterOptionsChanged();
        }

        public void InitDataManager()
        {
            InitDataManager(true);
        }

        public void ChangeSizeOfGraph()
        {
            if (zedGraphControl1.GraphPane.CurveList.Count <= 0)
                GraphHelper.SetInitProperity(zedGraphControl1);
            else
                GraphHelper.SetRecOfChart(zedGraphControl1, zedGraphControl1.GraphPane);
        }
        #region data list操作
        /// <summary>
        /// 单选时 dgvlist显示隐藏问题
        /// </summary>
        private void UncompareDataListShow(string sn, string tn, bool isShow)
        {
            //datalist 清空
            dgvList.Columns.Cast<DataGridViewColumn>().ToList().ForEach(p =>
            {
                if (p.Name != "PointTime" + sn + "_" + tn
                    && p.Name != "Temperature" + sn + "_" + tn
                    && p.Name != "interval" + sn + "_" + tn
                    && p.Name != "ID" && p.Visible == true)
                {
                    p.Visible = false;
                }
                else if (p.Name == "PointTime" + sn + "_" + tn
                    || p.Name == "Temperature" + sn + "_" + tn
                    || p.Name == "interval" + sn + "_" + tn
                    && p.Visible == false)
                {
                    p.Visible = true;
                }
            });
            if (dgvList.MergeColIsShow != null)
            {
                dgvList.MergeColIsShow.ToList().ForEach(p =>
                {
                    if (p.Key == sn + "_" + tn)
                        dgvList.MergeColIsShow[p.Key] = true;
                    else
                        dgvList.MergeColIsShow[p.Key] = false;
                });
            }
            //dgvList.MergeColIsShow[sn + "_" + tn] = isShow;
            dgvList.Rows.Clear();
        }


        private void CompareDataListShow(string sn, string tn, bool isShow)
        {
            if (dgvList.Columns.Count > 0)
            {
                dgvList.Columns.Cast<DataGridViewColumn>().ToList().ForEach(p =>
                {
                    if (p.Name == "PointTime" + sn + "_" + tn

                       || p.Name == "Temperature" + sn + "_" + tn
                        )
                    {
                        p.Visible = isShow;
                    }
                    else if (p.Name == "interval" + sn + "_" + tn && cbElapsed.Checked)
                        p.Visible = isShow;
                });
                int count = dgvList.Columns.Cast<DataGridViewColumn>().ToList().Where(p => p.Visible == true).Count();
                if (count == 1)
                    dgvList.Columns["ID"].Visible = isShow;
                else
                    dgvList.Columns["ID"].Visible = true;
                dgvList.MergeColIsShow.ToList().ForEach(p =>
                {
                    if (p.Key == sn + "_" + tn)
                        dgvList.MergeColIsShow[p.Key] = isShow;
                });
            }
        }
        /// <summary>
        /// 重新构造datalist列表
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="tn"></param>
        private void CompareDataList(string sn, string tn, bool isShow)
        {
            if (dgvList.Columns.Count == 0)
            {
                GenerateFirstDataListColumn(sn, tn);
                BindSingleDataList(sn, tn);
            }
            else
            {
                if (!dgvList.MergeColumnNames.Contains(sn + "_" + tn) || dgvList.MergeColIsShow[sn + "_" + tn] == false)
                {
                    AddColumnToDataList(sn, tn);//添加columns
                    AddRowToDataList(sn, tn);//添加rows
                }
            }
        }
        private void SetElapsedTimeShow(bool IsShow)
        {
            dgvList.MergeColIsShow.ToList().ForEach(p =>
            {
                dgvList.Columns.Cast<DataGridViewColumn>().ToList().ForEach(v =>
                {
                    if (p.Value == true)
                        if (v.Name == "interval" + p.Key)
                        {
                            v.Visible = IsShow;
                            dgvList.Refresh();
                        }
                });
            });
        }
        private void SetDateTimeShow(bool IsShow)
        {
            dgvList.MergeColIsShow.ToList().ForEach(p =>
            {
                dgvList.Columns.Cast<DataGridViewColumn>().ToList().ForEach(v =>
                {
                    if (p.Value == true)
                        if (v.Name == "PointTime" + p.Key)
                            v.Visible = IsShow;
                });
            });
        }
        /// <summary>
        /// 第一次选中时
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="tn"></param>
        private void GenerateFirstDataListColumn(string sn, string tn)
        {
            dgvList.Columns.Add(new DataGridViewColumn()
            {
                CellTemplate = new DataGridViewTextBoxCell(),
                HeaderText = "Points",
                Name = "ID",
                Width = 80,
            });
            dgvList.Columns.Add(new DataGridViewColumn()
            {
                CellTemplate = new DataGridViewTextBoxCell(),
                HeaderText = "Elapsed Time",
                Name = "interval" + sn + "_" + tn,
                Width = 100
            });
            dgvList.Columns.Add(new DataGridViewColumn()
            {
                CellTemplate = new DataGridViewTextBoxCell(),
                HeaderText = "Date/Time",
                Name = "PointTime" + sn + "_" + tn,
                Width = 130
            });
            dgvList.Columns.Add(new DataGridViewColumn()
            {
                CellTemplate = new DataGridViewTextBoxCell(),
                HeaderText = "Temperature(°" + Tag.TempUnit + ")",
                Name = "Temperature" + sn + "_" + tn,
                Width = 100
            });
            dgvList.ColumnHeadersHeight = 40;
            dgvList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvList.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 9, FontStyle.Regular);
            dgvList.MergeColumnNames.Add(sn + "_" + tn);
            if (!dgvList.MergeColIsShow.Keys.Contains(sn + "_" + tn))
                dgvList.MergeColIsShow.Add(sn + "_" + tn, true);
            dgvList.AddSpanHeader(1, 3, sn + "_" + tn);
        }
        private void BindSingleDataList(string sn, string tn)
        {
            List<Tuple<int?, DateTime, double, string>> list = GetList(sn, tn);
            int i = 0,j=1;
            if (list != null && list.Count > 0)
            {
                dgvList.Rows.Clear();
                dgvList.Rows.Add(list.Count);
                list.ForEach(p =>
                {
                    dgvList.Rows[i].Cells["ID"].Value = p.Item1;
                    dgvList.Rows[i].Cells["Temperature" + sn + "_" + tn].Value = p.Item4;
                    if (p.Item1.HasValue)
                    {
                        dgvList.Rows[i].Cells["interval" + sn + "_" + tn].Value = TempsenFormatHelper.ConvertSencondToFormmatedTime(p.Item3.ToString());
                    }
                    else
                    {
                        if (!IsCompare)
                        {
                            dgvList.Rows[i].Cells["ID"].Value = string.Format("Mark{0}", j++);
                            dgvList.Rows[i].Cells["ID"].Style.ForeColor = Color.Red;
                            dgvList.Rows[i].Cells["ID"].Style.Font = new Font("Arial",9f,FontStyle.Bold);
                        }
                        dgvList.Rows[i].Cells["interval" + sn + "_" + tn].Value = null;
                        dgvList.Rows[i].Cells["Temperature" + sn + "_" + tn].Style.ForeColor = Color.Red;
                        dgvList.Rows[i].Cells["PointTime" + sn + "_" + tn].Style.ForeColor = Color.Red;
                        dgvList.Rows[i].Cells["Temperature" + sn + "_" + tn].Style.Font = new Font("Arial", 9f, FontStyle.Bold);
                        dgvList.Rows[i].Cells["PointTime" + sn + "_" + tn].Style.Font = new Font("Arial", 9f, FontStyle.Bold);
                    }
                    dgvList.Rows[i].Cells["PointTime" + sn + "_" + tn].Value = p.Item2.ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture);

                    i++;
                });
                dgvList.Columns["interval" + sn + "_" + tn].Visible = cbElapsed.Checked;
                dgvList.Columns["PointTime" + sn + "_" + tn].Visible = cbDate.Checked;
            }
        }
        /// <summary>
        /// 添加合并表头column
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="tn"></param>
        private void AddColumnToDataList(string sn, string tn)
        {
            if (!dgvList.MergeColumnNames.Contains(sn + "_" + tn))
            {
                dgvList.Columns.Add(new DataGridViewColumn()
                {
                    CellTemplate = new DataGridViewTextBoxCell(),
                    HeaderText = "Elapsed Time",
                    Name = "interval" + sn + "_" + tn,
                    Width = 100
                });
                dgvList.Columns.Add(new DataGridViewColumn()
                {
                    CellTemplate = new DataGridViewTextBoxCell(),
                    HeaderText = "Date/Time",
                    Name = "PointTime" + sn + "_" + tn,
                    Width = 130
                    //AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                });
                dgvList.Columns.Add(new DataGridViewColumn()
                {
                    CellTemplate = new DataGridViewTextBoxCell(),
                    HeaderText = "Temperature(°" + Tag.TempUnit + ")",
                    Name = "Temperature" + sn + "_" + tn,
                    Width = 100
                });
                dgvList.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
                dgvList.MergeColumnNames.Add(sn + "_" + tn);
                if (!dgvList.MergeColIsShow.Keys.Contains(sn + "_" + tn))
                    dgvList.MergeColIsShow.Add(sn + "_" + tn, true);
                dgvList.AddSpanHeader(dgvList.Columns.Count - 3, 3, sn + "_" + tn);
            }
        }
        private void AddRowToDataList(string sn, string tn)
        {
            PointInfo point = _pointTempBll.GetPointsListByTNSN(sn, tn);
            List<Tuple<int?, DateTime, double, string>> list = GetList(sn, tn);
            int listCount = list.Count;
            int dgvCount = dgvList.Rows.Count;
            for (int i = 0,j=1; i < (dgvCount >= listCount ? dgvCount : listCount); i++)
            {
                if (listCount <= dgvCount)
                {
                    if (i == listCount)
                        break;
                }
                else
                {
                    if (i >= dgvCount)
                    {
                        dgvList.Rows.Add(1);
                        dgvList.Rows[i].Cells["ID"].Value = list[i].Item1;
                    }
                }
                dgvList.Rows[i].Cells["Temperature" + sn + "_" + tn].Value = list[i].Item4;
                if (list[i].Item1.HasValue)
                    dgvList.Rows[i].Cells["interval" + sn + "_" + tn].Value = TempsenFormatHelper.ConvertSencondToFormmatedTime(list[i].Item3.ToString());
                else
                {
                    dgvList.Rows[i].Cells["interval" + sn + "_" + tn].Value = null;
                    dgvList.Rows[i].Cells["Temperature" + sn + "_" + tn].Style.ForeColor = Color.Red;
                    dgvList.Rows[i].Cells["PointTime" + sn + "_" + tn].Style.ForeColor = Color.Red;
                    dgvList.Rows[i].Cells["Temperature" + sn + "_" + tn].Style.Font = new Font("Arial", 9f, FontStyle.Bold);
                    dgvList.Rows[i].Cells["PointTime" + sn + "_" + tn].Style.Font = new Font("Arial", 9f, FontStyle.Bold);
                }
                dgvList.Rows[i].Cells["PointTime" + sn + "_" + tn].Value = list[i].Item2.ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture);
                dgvList.Columns["interval" + sn + "_" + tn].Visible = cbElapsed.Checked;
                dgvList.Columns["PointTime" + sn + "_" + tn].Visible = cbDate.Checked;
            }
        }
        /// <summary>
        /// 返回待绑定的数据参数
        /// </summary>
        /// <param name="sn"></param>
        /// <param name="tn"></param>
        /// <returns></returns>
        private List<Tuple<int?, DateTime, double, string>> GetList(string sn, string tn)
        {
            if (_pointInfo != null && _pointInfo.ID != 0)
            {
                if (!pointInfoList.Contains(_pointInfo))
                    pointInfoList.Add(_pointInfo);
                pointKeyValueList = ObjectManage.DeserializePointKeyValue<PointKeyValue>(_pointInfo);
                bool isC = _pointInfo.TempUnit == "C" ? true : false;
                if (rbTempUnitC.Checked != isC)
                    pointKeyValueList = ObjectManage.GetTempListByUnit(pointKeyValueList, _pointInfo.TempUnit);
                int i = 0;
                List<Tuple<int?, DateTime, double, string>> list = new List<Tuple<int?, DateTime, double, string>>();
                Tuple<int?, DateTime, double, string> tuple;
                pointKeyValueList.ToList().ForEach(p =>
                {
                    if (!p.IsMark)
                    {
                        tuple = new Tuple<int?, DateTime, double, string>(i + 1
                                                                                                             , p.PointTime.ToLocalTime()
                                                                                                             , i * Convert.ToDouble(_logConfig.LogInterval)
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
            else
                return null;
        }
        private void CtorRecordList()
        {
            GenerateNormalDataView();
            GenerateFloatingDataView();
            BindDataToHistoryDataView();
        }

        private void ClickCompare()
        {
            if (IsCompare)
            {
                IsCompare = false;
                dgvList.Rows.Clear();
                dgvList.Columns.Clear();
                dgvList.MergeColumnNames.Clear();
                dgvList.MergeColIsShow.Clear();
                dgvHistory.Rows.Cast<DataGridViewRow>().ToList().ForEach(p =>
                {
                    p.ReadOnly = false;
                    p.Cells["CheckCol"].Value = false;
                });
                dgvFloatingHistory.Rows.Cast<DataGridViewRow>().ToList().ForEach(p =>
                {
                    p.ReadOnly = false;
                    p.Cells["CheckCol"].Value = false;
                });
                tpSummary.Controls.Clear();
                tpSummary.Controls.Add(pnSummary);
                zedGraphControl1.GraphPane.CurveList.Clear();
                zedGraphControl1.GraphPane.GraphObjList.Clear();
                TagsList.Clear();
                Tag = null;
                lbLoggerReader.Tag = null;
                lbSign.Tag = null;
                ClearLabelContents();
                pnSignHis.Visible = Common.Versions == SoftwareVersions.Pro;
                GraphHelper.SetInitProperity(zedGraphControl1);
                zedGraphControl1.Refresh();
                pbUnfold.Visible = tableViewTool.Visible = true;
                tabCtl.TabPages.Add(tpReportEdit);
                btnSave.Visible = true;
                btnSign.Visible = Common.Versions == SoftwareVersions.Pro;
            }
            else
            {
                if (m_IsRbCompareCheckedChanging)
                {
                    return;
                }
                else
                {
                    try
                    {
                        m_IsRbCompareCheckedChanging = true;
                        DialogResult dialogResult = DeviceManagerExitDialog(MessageBoxButtons.OKCancel);
                        if (DialogResult.Cancel == dialogResult)
                        {

                            this.rbUnCompare.Checked = true;
                            return;
                        }
                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e.Message);
                    }
                    finally
                    {
                        m_IsRbCompareCheckedChanging = false;
                    }
                }
                IsCompare = true;
                pnSignHis.Visible = false;
                pnSignature.Visible = false;
                lvSignature.Rows.Clear();
                lbSign.Text = string.Empty;
                this.tpSummary.Controls.Clear();
                if (dgvSummary == null)
                {
                    dgvSummary = new RowMergeView();
                    dgvSummary.Dock = DockStyle.Fill;
                    dgvSummary.ReadOnly = true;
                    this.dgvSummary.ColumnHeadersDefaultCellStyle = dgvList.ColumnHeadersDefaultCellStyle;
                    this.dgvSummary.AllowUserToAddRows = false;
                    this.dgvSummary.AllowUserToDeleteRows = false;
                    this.dgvSummary.AllowUserToResizeRows = false;
                    this.dgvSummary.BackgroundColor = System.Drawing.Color.White;
                    this.dgvSummary.RowHeadersVisible = false;
                    this.dgvSummary.RowTemplate.Height = 23;
                    this.dgvSummary.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.RowHeaderSelect;
                    this.dgvSummary.ColumnWidthChanged += new DataGridViewColumnEventHandler((sender, args) => dgvSummary.Refresh());
                }
                else
                {
                    dgvSummary.Rows.Clear();
                    dgvSummary.Columns.Clear();
                }
                tpSummary.Controls.Add(dgvSummary);
                var v = dgvList.MergeColIsShow.Where(p => p.Value == true).Select(p => p.Key).ToList();
                if (v != null && v.Count > 0)
                {
                    string sn_tn = v[0];
                    if (sn_tn.LastIndexOf("\n") != -1)
                    {
                        sn_tn = sn_tn.Substring(0, sn_tn.LastIndexOf("\n"));
                    }
                    string[] sntn = new string[2] { string.Empty, string.Empty };
                    if (sn_tn.IndexOf('_') != -1)
                    {
                        sntn[0] = sn_tn.Substring(0, sn_tn.IndexOf('_'));
                        sntn[1] = sn_tn.Substring(sn_tn.IndexOf('_') + 1);
                    }
                    CompareSummary(sntn[0], sntn[1], true);
                }
                ForEachTempByUnit();
                //pbUnfold.Visible = tableViewTool.Visible = false;
                tabCtl.TabPages.Remove(tpReportEdit);
                btnSign.Visible = btnSave.Visible = false;
                dgvHistory.Rows.Cast<DataGridViewRow>().ToList().ForEach(p => p.ReadOnly = false);
                dgvFloatingHistory.Rows.Cast<DataGridViewRow>().ToList().ForEach(p => p.ReadOnly = false);
            }
            SetViewToolState();
            if (this.OnComapreStatusChange != null)
            {
                this.OnComapreStatusChange(this, new EventArgs());
            }
        }
        private void ResetInitState()
        {
            dgvList.Rows.Clear();
            dgvList.Columns.Clear();
            dgvList.MergeColumnNames.Clear();
            if (dgvList.MergeColIsShow != null)
            {
                dgvList.MergeColIsShow.Clear();
            }
            zedGraphControl1.GraphPane.CurveList.Clear();
            zedGraphControl1.GraphPane.GraphObjList.Clear();
            Tag = null;
            reportCanvasPanel.Controls.Clear();
            ClearLabelContents();
            lvSignature.Rows.Clear();
            digitalSignList.Clear();
            lbSign.Text = "Unsigned";
            lbLoggerReader.Tag = null;
            lbSign.Tag = null;
            ConnectionController.SetSignButtonState(btnSign, false);
            ConnectionController.SetSaveButtonState(btnSave, false);
            GraphHelper.SetInitProperity(zedGraphControl1);
            zedGraphControl1.Refresh();
            this.pnHis.Enabled = true;
        }
        private void ForEachTempByUnit()
        {
            //遍历taglist和pointinfolist将摄氏和华氏转换
            //TagsList.ForEach(p =>
            //{
            //    bool isC = p.TempUnit == "C" ? true : false;
            //    if (rbTempUnitC.Checked != isC)
            //    {
            //        p = Common.CopyTo(p);
            //    }
            //});
            for (int i = 0; i < TagsList.Count; i++)
            {
                bool isC = TagsList[i].TempUnit == "C" ? true : false;
                if (rbTempUnitC.Checked != isC)
                {
                    TagsList[i] = Common.CopyTo(TagsList[i]);
                }
            }
        }
        #endregion
        #region summary
        public void InitTpSingleSummary(SuperDevice tag)
        {
            ClearLabelContents();
            InitDeviceInfo(tag);
            InitTripInformation(tag);
            InitLoggingSummary(tag);
            InitAlarms(tag);
        }
        private void InitDeviceInfo(SuperDevice tag)
        {
            #region product
            lProductName.Text = string.Format("Device: {0}", tag.DeviceName);
            lSerialNum.Text = string.Format("Serial Number: {0}", tag.SerialNumber);
            lModel.Text = string.Format("Model: {0}", tag.Model);
            lLogCycle.Text = string.Format("Log Interval/Cycle: {0}{2}{1}", TempsenFormatHelper.ConvertSencondToFormmatedTime(tag.LogInterval), tag.LogCycle, "/");
            lStartMode.Text = string.Format("Start Mode: {0}", tag.StartModel);
            if (tag.StartModel == "Manual Start")
                lStartDelay.Text = string.Format("Start Delay: {0}", tag.LogStartDelay);
            else
                //lStartDelay.Text = string.Format("Start Time: {0}", tag.StartConditionTime.ToString(Common.GlobalProfile.DateTimeFormator));
                lStartDelay.Text = string.Empty;
            #endregion
        }
        private void InitTripInformation(SuperDevice tag)
        {
            lTripNum.Text = string.Format("Trip Number: {0}", tag.TripNumber.IndexOf('_') == -1 ? tag.TripNumber : tag.TripNumber.Substring(0, tag.TripNumber.IndexOf('_')));
            if (tag.DeviceID < 200)
                lbDescText.Visible = lDesc.Visible = false;
            else
            {
                lbDescText.Visible = lDesc.Visible = true;
                lbDescText.Text = "Description:";
                //SetDynamicLabelText(lDesc, null);
                this.tips.SetToolTip(lDesc, tag.Description);
                lDesc.Text = tag.Description;
            }
        }
        private void InitLoggingSummary(SuperDevice tag)
        {
            lLowest.Text = string.Format("Lowest Temperature: {0}", Common.SetTempTimeFormat(tag.LowestC));
            lHighest.Text = string.Format("Highest Temperature: {0}", Common.SetTempTimeFormat(tag.HighestC));
            if (tag.TripLength != null)
            {
                lTripLen.Text = string.Format("Trip Length: {0}", tag.TripLength);
            }
            //lFirstPoint.Text = string.Format("First Point: {0}", tag.LoggingStart.ToString(Common.GlobalProfile.DateTimeFormator));
            lLogStop.Text = string.Format("Stop Time: {0}", tag.LoggingEnd.ToLocalTime().ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture));
            lLogStart.Text = string.Format("Start Time/First Point: {0}", tag.LoggingStart.ToLocalTime().ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture));
            lMKT.Text = string.Format("Mean Kinetic Temperature: {0}°{1}", tag.MKT, tag.TempUnit);
            lAveTemp.Text = string.Format("Average Temperature: {0}°{1}", tag.AverageC, tag.TempUnit);
            lDataPoint.Text = string.Format("Data Points: {0}", tag.DataPoints.ToString());
            string loggerReader = Tag.LoggerRead;
            string tagString = string.Empty;
            if (string.IsNullOrEmpty(loggerReader))
            {
                if (_device != null && !string.IsNullOrEmpty(_device.LoggerReader))
                {
                    string reader = string.Empty;
                    if (Common.Versions == SoftwareVersions.Pro && _device.LoggerReader.Split(new char[] { '@' }).Length > 1 && !string.IsNullOrWhiteSpace(_device.LoggerReader.Split(new char[] { '@' })[0]))
                    {
                        reader = "Logger Read: By {0}@{1}";
                    }
                    else
                    {
                        reader = "Logger Read: @{1}";
                    }
                    if (_device.LoggerReader.Split(new char[] { '@' }).Length > 1)
                    {
                        tag.LoggerRead = _device.LoggerReader;
                        tagString = string.Format(reader, _device.LoggerReader.Split(new char[] { '@' })[0], Convert.ToDateTime(_device.LoggerReader.Split(new char[] { '@' })[1]).ToLocalTime().ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture));
                    }
                }
            }
            else
            {
                if (loggerReader == null)
                {
                    loggerReader = string.Empty;
                }
                string[] loggerReadereString = loggerReader.Split(new char[] { '@' });
                if (loggerReadereString.Length >= 2)
                {
                    if (Common.Versions == SoftwareVersions.S || string.IsNullOrWhiteSpace(loggerReadereString[0]))
                    {
                        tagString = string.Format("Logger Read: @{0}", TempsenFormatHelper.GetFormattedDateTime(Convert.ToDateTime(loggerReadereString[1])));
                    }
                    else if (Common.Versions == SoftwareVersions.Pro)
                    {
                        tagString = string.Format("Logger Read: By {0}@{1}", loggerReadereString[0], TempsenFormatHelper.GetFormattedDateTime(Convert.ToDateTime(loggerReadereString[1])));
                    }
                }
                else
                {
                    tagString = "Logger Read:";
                }
            }
            lbLoggerReader.Tag = tagString;
            GetLoggerReaderText(lbLoggerReader, null);
            //SetDynamicLabelText(lbLoggerReader, null);
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
        private void CompareSummary(string sn, string tn, bool isShow)
        {
            AddColumnToSummaryList(sn, tn, isShow);//添加column
            AddRowsToSummaryList(sn, tn);//添加rows
        }
        private void AddColumnToSummaryList(string sn, string tn, bool isShow)
        {
            if (!dgvSummary.Columns.Contains("category"))
            {
                dgvSummary.Columns.Add(new DataGridViewColumn()
                {
                    CellTemplate = new DataGridViewTextBoxCell(),
                    HeaderText = "",
                    Name = "category",
                    Resizable = DataGridViewTriState.False,
                    Width = 150
                });
                this.dgvSummary.MergeColumnNames.Add("category");
            }
            if (!dgvSummary.Columns.Contains("property"))
            {
                dgvSummary.Columns.Add(new DataGridViewColumn()
                {
                    CellTemplate = new DataGridViewTextBoxCell(),
                    HeaderText = "Items",
                    Name = "property",
                    Width = 150
                });
            }
            if (!dgvSummary.Columns.Contains(sn + "_" + tn) && isShow)
            {
                dgvSummary.Columns.Add(new DataGridViewColumn()
                {
                    CellTemplate = new DataGridViewTextBoxCell(),
                    HeaderText = sn + "_" + tn,
                    Name = sn + "_" + tn,
                    Width = 200
                });
            }
            else if (dgvSummary.Columns.Contains(sn + "_" + tn))
                dgvSummary.Columns[sn + "_" + tn].Visible = isShow;
        }
        private void AddRowsToSummaryList(string sn, string tn)
        {
            /*添加property列即（trip num ，serial num etc.）*/
            if (dgvSummary.Rows.Count == 0)
            {
                dgvSummary.Rows.Add(42);
                //deviceconfiguration
                for (int i = 0; i < 6; i++)
                    dgvSummary.Rows[i].Cells["category"].Value = "Device Configuration";
                for (int i = 6; i < 8; i++)//trip information
                    dgvSummary.Rows[i].Cells["category"].Value = "Trip Information";
                for (int i = 8; i < 17; i++)//log summary
                    dgvSummary.Rows[i].Cells["category"].Value = "Logging Summary";
                for (int i = 17; i < 42; i++)//alarms
                    dgvSummary.Rows[i].Cells["category"].Value = "Alarms";
            }
            //if (dgvSummary.Rows[0].Cells[sn + "_" + tn].Value == null)
            //{
            dgvSummary.Rows[0].Cells["property"].Value = "Device";
            dgvSummary.Rows[0].Cells[sn + "_" + tn].Value = Tag.DeviceName;
            dgvSummary.Rows[1].Cells["property"].Value = "Model";
            dgvSummary.Rows[1].Cells[sn + "_" + tn].Value = Tag.Model;
            dgvSummary.Rows[2].Cells["property"].Value = "Serial Number";
            dgvSummary.Rows[2].Cells[sn + "_" + tn].Value = Tag.SerialNumber;
            dgvSummary.Rows[3].Cells["property"].Value = "Log Interval/Cycle";
            dgvSummary.Rows[3].Cells[sn + "_" + tn].Value = Tag.LogInterval + "s/" + Tag.LogCycle;
            dgvSummary.Rows[4].Cells["property"].Value = "Start Mode";
            dgvSummary.Rows[4].Cells[sn + "_" + tn].Value = Tag.StartModel;
            dgvSummary.Rows[5].Cells["property"].Value = "Start Delay";
            dgvSummary.Rows[5].Cells[sn + "_" + tn].Value = Tag.LogStartDelay;

            dgvSummary.Rows[6].Cells["property"].Value = "Trip Number";
            dgvSummary.Rows[6].Cells[sn + "_" + tn].Value = Tag.TripNumber.IndexOf('_') == -1 ? Tag.TripNumber : Tag.TripNumber.Substring(0, Tag.TripNumber.IndexOf('_'));
            dgvSummary.Rows[7].Cells["property"].Value = "Description";
            dgvSummary.Rows[7].Cells[sn + "_" + tn].Value = Tag.Description;

            dgvSummary.Rows[8].Cells["property"].Value = "Lowest Temperature";
            dgvSummary.Rows[8].Cells[sn + "_" + tn].Value = Common.SetTempTimeFormat(Tag.LowestC);
            dgvSummary.Rows[9].Cells["property"].Value = "Highest Temperature";
            dgvSummary.Rows[9].Cells[sn + "_" + tn].Value = Common.SetTempTimeFormat(Tag.HighestC);
            dgvSummary.Rows[10].Cells["property"].Value = "Trip Length";
            dgvSummary.Rows[10].Cells[sn + "_" + tn].Value = Tag.TripLength;
            dgvSummary.Rows[11].Cells["property"].Value = "First Point";
            dgvSummary.Rows[11].Cells[sn + "_" + tn].Value = Tag.LoggingStart == DateTime.MinValue ? string.Empty : Tag.LoggingStart.ToLocalTime().ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture);
            dgvSummary.Rows[12].Cells["property"].Value = "Stop Time";
            dgvSummary.Rows[12].Cells[sn + "_" + tn].Value = Tag.LoggingEnd == DateTime.MinValue ? string.Empty : Tag.LoggingEnd.ToLocalTime().ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture);
            dgvSummary.Rows[13].Cells["property"].Value = "Start Time";
            dgvSummary.Rows[13].Cells[sn + "_" + tn].Value = Tag.LoggingStart == DateTime.MinValue ? string.Empty : Tag.LoggingStart.ToLocalTime().ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture);
            dgvSummary.Rows[14].Cells["property"].Value = "Mean Kinetic Temperature";
            dgvSummary.Rows[14].Cells[sn + "_" + tn].Value = Tag.MKT;
            dgvSummary.Rows[15].Cells["property"].Value = "Average Temperature";
            dgvSummary.Rows[15].Cells[sn + "_" + tn].Value = Tag.AverageC;
            dgvSummary.Rows[16].Cells["property"].Value = "Data Point";
            dgvSummary.Rows[16].Cells[sn + "_" + tn].Value = Tag.DataPoints;

            //ALARM PROPERITY
            for (int i = 17; i < 42; i++)
            {
                int j = i - 17;
                if (j % 5 == 0)
                {
                    if (j / 5 == 2 )
                    {
                        dgvSummary.Rows[i].Cells["property"].Value = "A3";
                    }
                    else if(j / 5 == 3)
                    {
                        dgvSummary.Rows[i].Cells["property"].Value = "A5";
                    }
                    else 
                    {
                        dgvSummary.Rows[i].Cells["property"].Value = "A1";
                    }
                    for (int c = 1; c < dgvSummary.ColumnCount; c++)
                    {
                        dgvSummary.Rows[i].Cells[c].Style.BackColor = Color.FromArgb(216,216,216);
                    }
                }
                else if (j % 5 == 1)
                    dgvSummary.Rows[i].Cells["property"].Value = "Total Time";
                else if (j % 5 == 2)
                    dgvSummary.Rows[i].Cells["property"].Value = "Events";
                else if (j % 5 == 3)
                    dgvSummary.Rows[i].Cells["property"].Value = "First Triggered";
                else if (j % 5 == 4)
                    dgvSummary.Rows[i].Cells["property"].Value = "Alarm Status";
                else
                    continue;
            }
            //A1
            dgvSummary.Rows[17].Cells["property"].Value = "A1";
            if (!string.IsNullOrEmpty(Tag.A1))
            {
               dgvSummary.Rows[17].Cells[sn + "_" + tn].Value  = string.Format("over {0}°{1}", Tag.A1, Tag.TempUnit);
               dgvSummary.Rows[18].Cells[sn + "_" + tn].Value = string.Format("{0}", Tag.tempList.Count == 0 ? string.Empty : Tag.AlarmTotalTimeA1);
               dgvSummary.Rows[19].Cells[sn + "_" + tn].Value = string.Format("{0}", Tag.tempList.Count == 0 ? string.Empty : Tag.AlarmNumA1.ToString());
               dgvSummary.Rows[20].Cells[sn + "_" + tn].Value = string.Format("{0}", Convert.ToDateTime(Tag.AlarmA1First) == DateTime.MinValue ? "" : Convert.ToDateTime(Tag.AlarmA1First).ToLocalTime().ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture));
               dgvSummary.Rows[21].Cells[sn + "_" + tn].Value = string.Format("{0}", Tag.tempList.Count == 0 ? string.Empty : Tag.AlarmA1Status);
            }
            //A2
            dgvSummary.Rows[22].Cells["property"].Value = "A2";
            if (!string.IsNullOrEmpty(Tag.A2))
            {
                dgvSummary.Rows[22].Cells[sn + "_" + tn].Value = string.Format("over {0}°{1}", Tag.A2, Tag.TempUnit);
                dgvSummary.Rows[23].Cells[sn + "_" + tn].Value = string.Format("{0}", Tag.tempList.Count == 0 ? string.Empty : Tag.AlarmTotalTimeA2);
                dgvSummary.Rows[24].Cells[sn + "_" + tn].Value = string.Format("{0}", Tag.tempList.Count == 0 ? string.Empty : Tag.AlarmNumA2.ToString());
                dgvSummary.Rows[25].Cells[sn + "_" + tn].Value = string.Format("{0}", Convert.ToDateTime(Tag.AlarmA2First) == DateTime.MinValue ? "" : Convert.ToDateTime(Tag.AlarmA2First).ToLocalTime().ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture));
                dgvSummary.Rows[26].Cells[sn + "_" + tn].Value = string.Format("{0}", Tag.tempList.Count == 0 ? string.Empty : Tag.AlarmA2Status);
            }
            if (TagsList.Count(p => p.AlarmMode == 1) > 0 && TagsList.Count(p => p.AlarmMode == 2) > 0)
            { //A3
                dgvSummary.Rows[27].Cells["property"].Value = "A3/High";
                //A5
                dgvSummary.Rows[32].Cells["property"].Value = "A5/Low";
            }
            else if (TagsList.Count(p => p.AlarmMode == 2) > 0)
            {
                //A3
                dgvSummary.Rows[27].Cells["property"].Value = "A3";
                //A5
                dgvSummary.Rows[32].Cells["property"].Value = "A5";
            }
            else if (TagsList.Count(p => p.AlarmMode == 1) > 0)
            {
                //A3
                dgvSummary.Rows[27].Cells["property"].Value = "High Limit";
                //A5
                dgvSummary.Rows[32].Cells["property"].Value = "Low Limit";
            }
            if (Tag.AlarmMode == 2)
            {
                if (!string.IsNullOrEmpty(Tag.A3))
                {
                    dgvSummary.Rows[27].Cells[sn + "_" + tn].Value = string.Format("over {0}°{1}", Tag.A3, Tag.TempUnit);
                    dgvSummary.Rows[28].Cells[sn + "_" + tn].Value = string.Format("{0}", Tag.tempList.Count == 0 ? string.Empty : Tag.AlarmTotalTimeA3);
                    dgvSummary.Rows[29].Cells[sn + "_" + tn].Value = string.Format("{0}", Tag.tempList.Count == 0 ? string.Empty : Tag.AlarmNumA3.ToString());
                    dgvSummary.Rows[30].Cells[sn + "_" + tn].Value = string.Format("{0}", Convert.ToDateTime(Tag.AlarmA3First) == DateTime.MinValue ? "" : Convert.ToDateTime(Tag.AlarmA3First).ToLocalTime().ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture));
                    dgvSummary.Rows[31].Cells[sn + "_" + tn].Value = string.Format("{0}", Tag.tempList.Count == 0 ? string.Empty : Tag.AlarmA3Status);
                }
                if (!string.IsNullOrEmpty(Tag.A4))
                {
                    dgvSummary.Rows[32].Cells[sn + "_" + tn].Value = string.Format("under {0}°{1}", Tag.A4, Tag.TempUnit);
                    dgvSummary.Rows[33].Cells[sn + "_" + tn].Value = string.Format("{0}", Tag.tempList.Count == 0 ? string.Empty : Tag.AlarmTotalTimeA4);
                    dgvSummary.Rows[34].Cells[sn + "_" + tn].Value = string.Format("{0}", Tag.tempList.Count == 0 ? string.Empty : Tag.AlarmNumA4.ToString());
                    dgvSummary.Rows[35].Cells[sn + "_" + tn].Value = string.Format("{0}", Convert.ToDateTime(Tag.AlarmA4First) == DateTime.MinValue ? "" : Convert.ToDateTime(Tag.AlarmA4First).ToLocalTime().ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture));
                    dgvSummary.Rows[36].Cells[sn + "_" + tn].Value = string.Format("{0}", Tag.tempList.Count == 0 ? string.Empty : Tag.AlarmA4Status);
                }
            }
            else if (Tag.AlarmMode == 1)
            {
                if (!string.IsNullOrEmpty(Tag.AlarmHighLimit))
                {
                    dgvSummary.Rows[27].Cells[sn + "_" + tn].Value = string.Format("over {0}°{1}", Tag.AlarmHighLimit, Tag.TempUnit);
                    dgvSummary.Rows[28].Cells[sn + "_" + tn].Value = string.Format("{0}", Tag.tempList.Count == 0 ? string.Empty : Tag.HighAlarmTotalTimeAbove);
                    dgvSummary.Rows[29].Cells[sn + "_" + tn].Value = string.Format("{0}", Tag.tempList.Count == 0 ? string.Empty : Tag.HighAlarmEvents.ToString());
                    dgvSummary.Rows[30].Cells[sn + "_" + tn].Value = string.Format("{0}", Convert.ToDateTime(Tag.HighAlarmFirstTrigged) == DateTime.MinValue ? "" : Convert.ToDateTime(Tag.HighAlarmFirstTrigged).ToLocalTime().ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture));
                    dgvSummary.Rows[31].Cells[sn + "_" + tn].Value = string.Format("{0}", Tag.tempList.Count == 0 ? string.Empty : Tag.AlarmHighStatus);
                }
                //A5
                if (!string.IsNullOrEmpty(Tag.AlarmLowLimit))
                {
                    dgvSummary.Rows[32].Cells[sn + "_" + tn].Value = string.Format("under {0}°{1}", Tag.AlarmLowLimit, Tag.TempUnit);
                    dgvSummary.Rows[33].Cells[sn + "_" + tn].Value = string.Format("{0}", Tag.tempList.Count == 0 ? string.Empty : Tag.LowAlarmTotalTimeBelow);
                    dgvSummary.Rows[34].Cells[sn + "_" + tn].Value = string.Format("{0}", Tag.tempList.Count == 0 ? string.Empty : Tag.LowAlarmEvents.ToString());
                    dgvSummary.Rows[35].Cells[sn + "_" + tn].Value = string.Format("{0}", Convert.ToDateTime(Tag.LowAlarmFirstTrigged) == DateTime.MinValue ? "" : Convert.ToDateTime(Tag.LowAlarmFirstTrigged).ToLocalTime().ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture));
                    dgvSummary.Rows[36].Cells[sn + "_" + tn].Value = string.Format("{0}", Tag.tempList.Count == 0 ? string.Empty : Tag.AlarmLowStatus);
                }
            }

            //A6
            dgvSummary.Rows[37].Cells["property"].Value = "A6";
            if (!string.IsNullOrEmpty(Tag.A5))
            {
                dgvSummary.Rows[37].Cells[sn + "_" + tn].Value = string.Format("over {0}°{1}", Tag.A5, Tag.TempUnit);
                dgvSummary.Rows[38].Cells[sn + "_" + tn].Value = string.Format("{0}", Tag.tempList.Count == 0 ? string.Empty : Tag.AlarmTotalTimeA5);
                dgvSummary.Rows[39].Cells[sn + "_" + tn].Value = string.Format("{0}", Tag.tempList.Count == 0 ? string.Empty : Tag.AlarmNumA5.ToString());
                dgvSummary.Rows[40].Cells[sn + "_" + tn].Value = string.Format("{0}", Convert.ToDateTime(Tag.AlarmA5First) == DateTime.MinValue ? "" : Convert.ToDateTime(Tag.AlarmA3First).ToLocalTime().ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture));
                dgvSummary.Rows[41].Cells[sn + "_" + tn].Value = string.Format("{0}", Tag.tempList.Count == 0 ? string.Empty : Tag.AlarmA5Status);
            }
        }
        private Device GetDevice(string sn, string tn)
        {
            var device = from p in deviceList
                         where p.SerialNum == sn && p.TripNum == tn
                         select p;
            if (device != null && device.Count() > 0)
            {
                return device.ToList()[0];
            }
            else
            {
                Device itag = _deviceBll.GetDeviceBySnTn(sn, tn);
                deviceList.Add(itag);
                return itag;
            }
        }
        private PointInfo GetPointInfo(string sn, string tn)
        {
            var pointinfo = from p in pointInfoList
                            where p.SN == sn && p.TN == tn
                            select p;
            if (pointinfo != null && pointinfo.Count() > 0)
                return pointinfo.ToList()[0];
            else
            {
                PointInfo point = _pointTempBll.GetPointsListByTNSN(sn, tn);
                pointInfoList.Add(point);
                return point;
            }
        }
        private LogConfig GetLogConfig(string sn, string tn)
        {
            var log = from p in logConfigList
                      where p.SN == sn && p.TN == tn
                      select p;
            if (log != null && log.Count() > 0)
                return log.ToList()[0];
            else
            {
                LogConfig logconfig = _logConfigBll.GetLogConfigBySNTN(sn, tn);
                logConfigList.Add(logconfig);
                return logconfig;
            }
        }
        private void GetDigitalSignature(string sn, string tn)
        {
            digitalSignList = _digitalSignBll.GetDigitalSignatureBySnTn(sn, tn);
        }
        private List<AlarmConfig> GetAlarmConfig(string sn, string tn)
        {
            var alarmConfig = alarmConfigList.Where(p => p.SN == sn && p.TN == tn);
            if (alarmConfig.Count() <= 0)
            {
                alarmConfig = _alarmConfigBll.GetAlarmConfigBySnTn(sn, tn);
                alarmConfigList.AddRange(alarmConfig);
            }
            return alarmConfig.ToList();
        }
        private SuperDevice GetCurrentTagFromList(string serialNo, string tripNo)
        {
            SuperDevice currentTag = null;
            var var = from p in TagsList
                      where p.SerialNumber == serialNo && p.TripNumber == tripNo
                      select p;
            int index = TagsList.FindIndex(p => p.SerialNumber == serialNo && p.TripNumber == tripNo);
            if (var != null && var.Count() > 0)
            {
                currentTag = var.ToList()[0];
            }
            return currentTag;
        }
        private void InitDeviceType(Device device, LogConfig logconfig, PointInfo point, List<AlarmConfig> alarmConfig, bool isShow)
        {
            var var = from p in TagsList
                      where p.SerialNumber == device.SerialNum && p.TripNumber == device.TripNum
                      select p;
            int index = TagsList.FindIndex(p => p.SerialNumber == device.SerialNum && p.TripNumber == device.TripNum);
            if (var != null && var.Count() > 0)
            {
                Tag = var.ToList()[0];
                if (isShow)
                {
                    bool isC = Tag.TempUnit == "C" ? true : false;
                    if (rbTempUnitC.Checked != isC)
                    {
                        Tag = Common.CopyTo(var.ToList()[0]);
                    }
                }
                else
                {
                    if (index != -1)
                        TagsList.RemoveAt(index);
                }
            }
            else if (isShow && index == -1)
            {
                Tag = ObjectManage.GetDeviceInstance((DeviceType)device.DeviceID);
                List<PointKeyValue> list = ObjectManage.DeserializePointKeyValue<PointKeyValue>(point);
                Tag.SerialNumber = device.SerialNum;
                Tag.TripNumber = device.TripNum;
                Tag.Description = device.DESCS;
                Tag.Model = device.Model;
                Tag.DeviceName = Tag.ProductName = device.ProductName;
                Tag.DeviceID = device.DeviceID;
                Tag.LogCycle = logconfig.LogCycle;
                Tag.LogInterval = logconfig.LogInterval;
                Tag.LogStartDelay = logconfig.StartDelay;
                Tag.StartModel = logconfig.StartMode;
                Tag.LoggingStart = point.StartTime;
                Tag.LoggingEnd = point.EndTime;
                Tag.TripLength = point.TripLength;
                Tag.DataPoints = list.Count(p => !p.IsMark);
                Tag.TempUnit = point.TempUnit;
                Tag.points = point;
                Tag.AlarmMode = device.AlarmMode;
                Tag.tempList = list;
                if (Tag.AlarmMode == 1)
                {
                    List<AlarmConfig> highs = alarmConfig.Where(p => p.AlarmLevel == "A6").ToList();
                    List<AlarmConfig> lows = alarmConfig.Where(p => p.AlarmLevel == "A1").ToList();
                    if (highs.Count > 0)
                    {

                        Tag.AlarmHighLimit = highs.First().AlarmTemp;
                        Tag.AlarmHighDelay = highs.First().AlarmDelay;
                        Tag.HighAlarmEvents = highs.First().AlarmNumbers;
                        if (highs.First().IsAlarm == "Alarm")
                            Tag.HighAlarmFirstTrigged = highs.First().AlarmFirstTriggered;
                        Tag.HighAlarmTotalTimeAbove = highs.First().AlarmTotalTime;
                        Tag.AlarmHighStatus = highs.First().IsAlarm;
                        Tag.HighAlarmType = highs.First().AlarmType;
                    }
                    if (lows.Count > 0)
                    {
                        Tag.AlarmLowLimit = lows.First().AlarmTemp;
                        Tag.AlarmLowDelay = lows.First().AlarmDelay;
                        Tag.LowAlarmEvents = lows.First().AlarmNumbers;
                        if (lows.First().IsAlarm == "Alarm")
                            Tag.LowAlarmFirstTrigged = lows.First().AlarmFirstTriggered;
                        Tag.LowAlarmTotalTimeBelow = lows.First().AlarmTotalTime;
                        Tag.AlarmLowStatus = lows.First().IsAlarm;
                        Tag.LowAlarmType = lows.First().AlarmType;
                    }
                }
                else if (Tag.AlarmMode == 2)
                {
                    try
                    {
                        List<AlarmConfig> a1 = alarmConfig.Where(p => p.AlarmLevel == "A1").ToList();
                        List<AlarmConfig> a2 = alarmConfig.Where(p => p.AlarmLevel == "A2").ToList();
                        List<AlarmConfig> a3 = alarmConfig.Where(p => p.AlarmLevel == "A3").ToList();
                        List<AlarmConfig> a4 = alarmConfig.Where(p => p.AlarmLevel == "A4").ToList();
                        List<AlarmConfig> a5 = alarmConfig.Where(p => p.AlarmLevel == "A5").ToList();
                        if (a1.Count > 0)
                        {
                            Tag.AlarmDelayA1 = Convert.ToInt32(a1.First().AlarmDelay);
                            Tag.A1 = a1.First().AlarmTemp;
                            Tag.AlarmTypeA1 = a1.First().AlarmType;
                            if (a1.First().IsAlarm == "Alarm")
                                Tag.AlarmA1First = a1.First().AlarmFirstTriggered;
                            Tag.AlarmA1Status = a1.First().IsAlarm;
                            Tag.AlarmNumA1 = a1.First().AlarmNumbers;
                            Tag.AlarmTotalTimeA1 = a1.First().AlarmTotalTime;
                        }
                        if (a2.Count > 0)
                        {
                            Tag.AlarmDelayA2 = Convert.ToInt32(a2.First().AlarmDelay);
                            Tag.A2 = a2.First().AlarmTemp;
                            Tag.AlarmTypeA2 = a2.First().AlarmType;
                            if (a2.First().IsAlarm == "Alarm")
                                Tag.AlarmA2First = a2.First().AlarmFirstTriggered;
                            Tag.AlarmA2Status = a2.First().IsAlarm;
                            Tag.AlarmNumA2 = a2.First().AlarmNumbers;
                            Tag.AlarmTotalTimeA2 = a2.First().AlarmTotalTime;
                        }
                        if (a3.Count > 0)
                        {
                            Tag.AlarmDelayA3 = Convert.ToInt32(a3.First().AlarmDelay);
                            Tag.A3 = a3.First().AlarmTemp;
                            Tag.AlarmTypeA3 = a3.First().AlarmType;
                            if (a3.First().IsAlarm == "Alarm")
                                Tag.AlarmA3First = a3.First().AlarmFirstTriggered;
                            Tag.AlarmA3Status = a3.First().IsAlarm;
                            Tag.AlarmNumA3 = a3.First().AlarmNumbers;
                            Tag.AlarmTotalTimeA3 = a3.First().AlarmTotalTime;
                        }
                        if (a4.Count > 0)
                        {
                            Tag.AlarmDelayA4 = Convert.ToInt32(a4.First().AlarmDelay);
                            Tag.A4 = a4.First().AlarmTemp;
                            Tag.AlarmTypeA4 = a4.First().AlarmType;
                            if (a4.First().IsAlarm == "Alarm")
                                Tag.AlarmA4First = a4.First().AlarmFirstTriggered;
                            Tag.AlarmA4Status = a4.First().IsAlarm;
                            Tag.AlarmNumA4 = a4.First().AlarmNumbers;
                            Tag.AlarmTotalTimeA4 = a4.First().AlarmTotalTime;
                        }
                        if (a5.Count > 0)
                        {
                            Tag.AlarmDelayA5 = Convert.ToInt32(a5.First().AlarmDelay);
                            Tag.A5 = a5.First().AlarmTemp;
                            Tag.AlarmTypeA5 = a5.First().AlarmType;
                            if (a5.First().IsAlarm == "Alarm")
                                Tag.AlarmA5First = a5.First().AlarmFirstTriggered;
                            Tag.AlarmA5Status = a5.First().IsAlarm;
                            Tag.AlarmNumA5 = a5.First().AlarmNumbers;
                            Tag.AlarmTotalTimeA5 = a5.First().AlarmTotalTime;
                        }

                        Tag.AlarmTotalTimeIdeal = TempsenFormatHelper.ConvertSencondToFormmatedTime(Tag.CalcMultiAlarmTotalTime("Ideal").ToString());
                    }
                    catch
                    {
                    }
                }
                Tag.HighestC = point.HighestC == null ? "" : point.HighestC;
                Tag.LowestC = point.LowestC == null ? "" : point.LowestC;
                Tag.AverageC = point.AVGTemp == null ? "" : point.AVGTemp;
                Tag.MKT = point.MKT;
                //System.Diagnostics.Debug.WriteLine(Common.CalcMKT(list.Select(p => (int)(p.PointTemp * 100)).ToList()), "MKT");
                if (!rbTempUnitC.Checked && !rbTempUnitF.Checked)
                {
                    rbTempUnitC.Checked = Tag.TempUnit == "C" ? true : false;
                    rbTempUnitF.Checked = Tag.TempUnit == "F" ? true : false;
                }
                //bool isC = Tag.TempUnit == "C" ? true : false;
                //if (rbTempUnitC.Checked != isC)
                //{
                //    Tag = Common.CopyTo(Tag);
                //}
                if (IsCompare)
                    TagsList.Add(Tag);
                else
                {
                    TagsList.Clear();
                    TagsList.Add(Tag);
                }
                //}
            }
        }
        public double AlarmTimeCalc(List<PointKeyValue> point, bool IsHighest)
        {
            List<double> seconds = new List<double>();//散列点联系的时间
            PointKeyValue info;
            double continuation;
            double interal;
            interal = continuation = int.Parse(Tag.LogInterval);
            var v = new List<PointKeyValue>();
            if (IsHighest)
            {
                v = point.Where(p => p.PointTemp >= Convert.ToDouble(Tag.AlarmHighLimit)).ToList();
                var varible = (from p in v
                               where p.PointTime.ToString() == Tag.HighAlarmFirstTrigged
                               select p);
                if (varible.ToList().Count == 0)
                    return 0;
                info = varible.First();
            }
            else
            {
                v = point.Where(p => p.PointTemp <= Convert.ToDouble(Tag.AlarmLowLimit)).ToList();
                var varible = (from p in v
                               where p.PointTime.ToString() == Tag.LowAlarmFirstTrigged
                               select p);
                if (varible.ToList().Count == 0)
                    return 0;
                info = varible.First();
            }
            v.ForEach(p =>
            {
                if ((p.PointTime - info.PointTime).TotalSeconds == continuation)
                {
                    continuation = continuation + interal;
                }
                else if ((p.PointTime - info.PointTime).TotalSeconds != 0)
                {
                    seconds.Add(continuation);
                    continuation = int.Parse(Tag.LogInterval);
                    info = p;
                }
            });
            if (seconds.Count == 0)
                seconds.Add(continuation);
            return seconds.Max();
        }
        #endregion
        #region 上下文菜单选项
        private ContextMenu contextMenu = new ContextMenu();
        private void InitContextMenu()
        {
            MenuItem item = new MenuItem("Show Description", new EventHandler((seneder, e) => ClickShowDescription((MenuItem)seneder)));
            contextMenu.MenuItems.Add(item);
            item = new MenuItem("Delete Selected Record", new EventHandler((seneder, e) => ClickDeleteSelectedRecords((MenuItem)seneder, false)));
            if (!IsAllowDeleteAll() && !IsAllowDeleteUnsigned())
                item.Enabled = false;
            contextMenu.MenuItems.Add(item);
            item = new MenuItem("Delete All Records", new EventHandler((seneder, e) => ClickDeleteSelectedRecords((MenuItem)seneder, true)));
            if (!IsAllowDeleteAll() && !IsAllowDeleteUnsigned())
                item.Enabled = false;
            contextMenu.MenuItems.Add(item);
            pbOtherInfo.MouseClick += new MouseEventHandler((a, b) =>
            {
                if (b.Button == MouseButtons.Left)
                {
                    if (pbOtherInfo.ContextMenu == null)
                        pbOtherInfo.ContextMenu = contextMenu;
                    contextMenu.Show(pbOtherInfo, new Point(b.X, b.Y));
                }
            });
        }
        private void ClickShowDescription(MenuItem item)
        {
            item.Checked = !item.Checked;
            if (dgvHistory != null && dgvHistory.Rows.Count > 0)
            {
                int i = 0;
                dgvHistory.Rows.Cast<DataGridViewRow>().ToList().ForEach(p =>
                {
                    string sn_tn = p.Cells["record"].Value.ToString();
                    if (sn_tn.LastIndexOf("\n") != -1)
                    {
                        sn_tn = sn_tn.Substring(0, sn_tn.LastIndexOf("\n"));
                    }
                    string[] sntn = new string[2] { string.Empty, string.Empty };
                    if (sn_tn.IndexOf('_') != -1)
                    {
                        sntn[0] = sn_tn.Substring(0, sn_tn.IndexOf('_'));
                        sntn[1] = sn_tn.Substring(sn_tn.IndexOf('_') + 1);
                    }
                   
                    string desc = deviceList.Where(v => v.SerialNum == sntn[0] && v.TripNum == sntn[1]).Select(v => v.DESCS).First().Trim();
                    if (desc != string.Empty && item.Checked)
                    {
                        p.Cells["record"].Value = p.Cells["record"].Value.ToString() + "\n" + GetDynamicText(desc,label18);
                        
                    }
                    else
                    {
                        p.Cells["record"].Value = sntn[0] + "_" + sntn[1];
                    }
                    dgvHistory.AutoResizeRow(i);
                    i++;
                });

            }
        }
        private void ClickDeleteSelectedRecords(MenuItem item, bool isAll)
        {
            //item.Checked = !item.Checked;
            if (IsAllowDeleteAll() || IsAllowDeleteUnsigned())
            {
                DeleteRecords(isAll);
            }
        }

        private void ClearTpReportEditorAfterDelete()
        {
            this.Tag = null;
            if (this.reportCanvasPanel != null)
            {
                this.reportCanvasPanel.Controls.Clear();
            }
        }
        private void DeleteRecords(bool isAll)
        {
            bool allowAll = IsAllowDeleteAll();
            bool allowUnsign = IsAllowDeleteUnsigned();
            DigitalSignatureBLL _digital = new DigitalSignatureBLL();
            if (isAll)
            {
                if (DialogResult.Yes == Utils.ShowMessageBox(Messages.DeleteAllRecord, Messages.TitleWarning, MessageBoxButtons.YesNo))
                {
                    if (deviceList.Count > 0)
                    {
                        if (allowAll)
                        {
                            GetAllList();
                            DeleteAllRecords(deviceList, pointInfoList, logConfigList, alarmConfigList, digitalSignList);
                            this.ClearTpReportEditorAfterDelete();
                            //return;
                        }
                        else if (allowUnsign)
                        {
                            //List<string> di = digitalSignList.Where(p => !_digital.IsExist(p.SN, p.TN)).Select(v => v.SN + "_" + v.TN).ToList();
                            List<string> di = GetUnsignedDevice(deviceList.Select(p => p.SerialNum + "_" + p.TripNum).ToList(), _digital);
                            if (di == null || di.Count == 0)
                            {
                                Utils.ShowMessageBox(Messages.OnlyCanDeleteUnsignedRecords, Messages.TitleNotification);
                            }
                            if (di != null && di.Count > 0)
                            {
                                DeleteAllRecords(deviceList.Where(p => di.Contains(p.SerialNum + "_" + p.TripNum)).ToList()
                                                        , pointInfoList.Where(p => di.Contains(p.SN + "_" + p.TN)).ToList()
                                                        , logConfigList.Where(p => di.Contains(p.SN + "_" + p.TN)).ToList()
                                                        , _alarmConfig.Where(p => di.Contains(p.SN + "_" + p.TN)).ToList()
                                                        , digitalSignList.Where(p => di.Contains(p.SN + "_" + p.TN)).ToList()
                                                        );
                                this.ClearTpReportEditorAfterDelete();
                            }
                            else
                                return;

                        }
                        else
                        {
                            Utils.ShowMessageBox(Messages.NoPermission, Messages.TitleError);
                            return;
                        }
                        ResetInitState();
                    }
                }
            }
            else
            {
                string sn, tn;
                sn = tn = string.Empty;
                List<string> v = dgvHistory.Rows.Cast<DataGridViewRow>().ToList().Where(p =>
                {
                    return Convert.ToBoolean(p.Cells["CheckCol"].EditedFormattedValue);
                }).Select(p =>
                {
                    string sn_tn = p.Cells["record"].Value.ToString();
                    if (sn_tn.LastIndexOf("\n") != -1)
                    {
                        sn_tn = sn_tn.Substring(0, sn_tn.LastIndexOf("\n"));
                    }
                    string[] sntn = new string[2] { string.Empty, string.Empty };
                    if (sn_tn.IndexOf('_') != -1)
                    {
                        sntn[0] = sn_tn.Substring(0, sn_tn.IndexOf('_'));
                        sntn[1] = sn_tn.Substring(sn_tn.IndexOf('_') + 1);
                    }
                    //string[] sntn = p.Cells["record"].Value.ToString().Split(_DescSplit);
                    sn = sntn[0];
                    tn = sntn[1];
                    return sn + "_" + tn;
                }).ToList();
                if (v.Count > 0 && DialogResult.Yes == Utils.ShowMessageBox(Messages.DeleteSelectedRecord, Messages.TitleWarning, MessageBoxButtons.YesNo))
                {
                    if (allowAll)
                        DeleteOneRecord(deviceList.Where(p => v.Contains(p.SerialNum + "_" + p.TripNum)).ToList()
                            , pointInfoList.Where(p => v.Contains(p.SN + "_" + p.TN)).ToList()
                            , logConfigList.Where(p => v.Contains(p.SN + "_" + p.TN)).ToList()
                            , alarmConfigList.Where(p => v.Contains(p.SN + "_" + p.TN)).ToList()
                            , digitalSignList.Where(p => v.Contains(p.SN + "_" + p.TN)).ToList());
                    else if (allowUnsign)
                    {
                        List<string> di = GetUnsignedDevice(v, _digital);
                        if (di == null || di.Count == 0)
                        {
                            Utils.ShowMessageBox(Messages.OnlyCanDeleteUnsignedRecords, Messages.TitleNotification);
                        }
                        if (di != null && di.Count > 0)
                        {
                            DeleteOneRecord(deviceList.Where(p => di.Contains(p.SerialNum + "_" + p.TripNum)).ToList()
                                                , pointInfoList.Where(p => di.Contains(p.SN + "_" + p.TN)).ToList()
                                                , logConfigList.Where(p => di.Contains(p.SN + "_" + p.TN)).ToList()
                                                , alarmConfigList.Where(p => di.Contains(p.SN + "_" + p.TN)).ToList()
                                                , digitalSignList.Where(p => di.Contains(p.SN + "_" + p.TN)).ToList()
                                                );
                            this.ClearTpReportEditorAfterDelete();
                        }
                        else
                            return;
                    }
                    else
                    {
                        Utils.ShowMessageBox(Messages.NoPermission, Messages.TitleError);
                        return;
                    }
                    ResetInitState();
                }
            }
        }
        private List<string> GetUnsignedDevice(List<string> selected, DigitalSignatureBLL _bll)
        {
            return selected.Where(p =>
            {
                string sn_tn = p;
                if (sn_tn.LastIndexOf("\n") != -1)
                {
                    sn_tn = sn_tn.Substring(0, sn_tn.LastIndexOf("\n"));
                }
                string[] sntn = new string[2] { string.Empty, string.Empty };
                if (sn_tn.IndexOf('_') != -1)
                {
                    sntn[0] = sn_tn.Substring(0, sn_tn.IndexOf('_'));
                    sntn[1] = sn_tn.Substring(sn_tn.IndexOf('_') + 1);
                }
                //string[] sntn = p.Split(_DescSplit);
                return !_bll.IsExist(sntn[0], sntn[1]);
            }).ToList();
        }
        private void DeleteAllRecords(List<Device> device, List<PointInfo> points, List<LogConfig> log, List<AlarmConfig> alarm, List<DigitalSignature> digital)
        {
            if (_deviceBll.DeleteDeviceInformation(device, points, log, alarm, digital))
            {
                //记录日志
                dgvHistory.Rows.Cast<DataGridViewRow>().ToList().ForEach(p =>
                {
                    SaveOperationLog(p.Cells["record"].Value.ToString());
                });
                InitHistoryData();
                if (dgvList != null)
                {
                    this.dgvList.Rows.Clear();
                    this.dgvList.Columns.Clear();
                    this.dgvList.MergeColIsShow.Clear();
                    this.dgvList.MergeColumnNames.Clear();
                }
                if (dgvSummary != null)
                {
                    this.dgvSummary.Rows.Clear();
                    this.dgvSummary.Columns.Clear();
                    this.dgvSummary.MergeColumnNames.Clear();
                }
                //this.zedGraphControl1.GraphPane
                if (device != null)
                {
                    device.ForEach(p =>
                    {
                        var v = this.zedGraphControl1.GraphPane.CurveList.Where(c => c.Label.Text == p.SerialNum + "_" + p.TripNum).ToList();
                        if (v.Count > 0)
                        {
                            this.zedGraphControl1.GraphPane.CurveList.Remove(v.First());
                        }
                    });
                    zedGraphControl1.Refresh();
                }
                ClearLabelContents();
            }
        }
        private void DeleteOneRecord(List<Device> device, List<PointInfo> points, List<LogConfig> log, List<AlarmConfig> alarm, List<DigitalSignature> digital)
        {
            if (_deviceBll.DeleteDeviceInformation(device, points, log, alarm, digital))
            {
                device.ForEach(p =>
                {
                    SaveOperationLog(p.SerialNum + "_" + p.TripNum);
                });
                InitHistoryData();
                if (dgvList != null)
                {
                    this.dgvList.Rows.Clear();
                    this.dgvList.Columns.Clear();
                    this.dgvList.MergeColIsShow.Clear();
                    this.dgvList.MergeColumnNames.Clear();
                }
                if (dgvSummary != null)
                {
                    this.dgvSummary.Rows.Clear();
                    this.dgvSummary.Columns.Clear();
                    this.dgvSummary.MergeColumnNames.Clear();
                }
                //this.zedGraphControl1.GraphPane
                if (device != null)
                {
                    device.ForEach(p =>
                    {
                        var v = this.zedGraphControl1.GraphPane.CurveList.Where(c => c.Label.Text == p.SerialNum + "_" + p.TripNum).ToList();
                        if (v.Count > 0)
                        {
                            this.zedGraphControl1.GraphPane.CurveList.Remove(v.First());
                        }
                    });
                    zedGraphControl1.Refresh();
                }
                ClearLabelContents();
                this.ClearTpReportEditorAfterDelete();
            }
        }
        private bool IsAllowDeleteAll()
        {
            return Common.IsAuthorized(RightsText.DeleteRecords);
        }
        private bool IsAllowDeleteUnsigned()
        {
            return Common.IsAuthorized(RightsText.DeleteUnsignedRecords);
        }
        #endregion
        private void GetDeviceObjects(string sn, string tn)
        {

            _device = GetDevice(sn, tn);
            _logConfig = GetLogConfig(sn, tn);
            _pointInfo = GetPointInfo(sn, tn);
            _alarmConfig = GetAlarmConfig(sn, tn);
            editor = GetReportEditor(sn, tn);
            GetDigitalSignature(sn, tn);
        }
        private void GetAllList()
        {
            dgvHistory.Rows.Cast<DataGridViewRow>().ToList().ForEach(p =>
            {
                string sn_tn = p.Cells["record"].Value.ToString();
                if (sn_tn.LastIndexOf("\n") != -1)
                {
                    sn_tn = sn_tn.Substring(0, sn_tn.LastIndexOf("\n"));
                }
                string[] sntn = new string[2] { string.Empty, string.Empty };
                if (sn_tn.IndexOf('_') != -1)
                {
                    sntn[0] = sn_tn.Substring(0, sn_tn.IndexOf('_'));
                    sntn[1] = sn_tn.Substring(sn_tn.IndexOf('_') + 1);
                }
                //string[] sntn = p.Cells["record"].Value.ToString().Split(_DescSplit);
                GetDeviceObjects(sntn[0], sntn[1]);
            });

        }
        private void ClearLabelContents()
        {
            if (string.IsNullOrWhiteSpace(tbCmt.Text))
            {
                tbCmt.Text = ReportConstString.CommentDefaultString;
            }
            tableAlarms.Controls.Cast<System.Windows.Forms.Label>().ToList().ForEach(p =>
            {
                System.Windows.Forms.Label labels = p as System.Windows.Forms.Label;
                if (labels != null)
                {
                    if (labels != null && labels.Text != "Alarm Zones" && labels.Text != "Alarm Delay" && labels.Text != "Total Time"
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
            pnTrip.Controls.Cast<System.Windows.Forms.Label>().ToList().ForEach(p =>
            {
                System.Windows.Forms.Label labels = p as System.Windows.Forms.Label;
                if (labels != null)
                {
                    if (labels != null && labels.Text != "Alarm Zones" && labels.Text != "Alarm Delay" && labels.Text != "Total Time"
                        && labels.Text != "Events" && labels.Text != "First Triggered" && labels.Text != "Alarm Status")
                        labels.Text = string.Empty;
                }
            });
            if (dgvSummary != null)
            {
                dgvSummary.Rows.Clear();
                dgvSummary.Columns.Clear();
            }
            lbDescText.Text = lbLimits.Text = lTripNum.Text = lDesc.Text = string.Empty;
            if (Tag == null || Tag.tempList.Count == 0 || Tag.AlarmMode == 0)
                lbAlarm.Visible = lbDelayTime.Visible = lbTotalTime.Visible = lbNum.Visible = lbfirst.Visible = lbAlarmStatus.Visible = false;
            else
                lbAlarm.Visible = lbDelayTime.Visible = lbTotalTime.Visible = lbNum.Visible = lbfirst.Visible = lbAlarmStatus.Visible = true;
        }
        private void SaveOperationLog(string sntn)
        {
            //tpGraph.Visible = false;
            //记录成功的日志
            if (Common.User.UserName != Common.SUPERUSER)
            {
                logBll.InsertLog(() =>
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("OperateTime", DateTime.UtcNow);
                    dic.Add("Action", LogAction.Deleterecord);
                    dic.Add("UserName", Common.User.UserName);
                    dic.Add("FullName", Common.User.FullName);
                    dic.Add("Detail", sntn);
                    dic.Add("LogType", LogAction.AnalysisAuditTrail);
                    return dic;
                });
            }
        }
        #region graph
        private void CompareGraph(string sn, string tn, bool isShow)
        {
            GraphPane pane = zedGraphControl1.GraphPane;
            GraphHelper.SetRecOfChart(zedGraphControl1, zedGraphControl1.GraphPane);
            if (pane.CurveList.Count >= 10 && isShow)
                return;
            if (pane.CurveList.Count == 0 && isShow)
                AddLineItemToGraph(sn, tn, 1);
            else if (isShow)
            {
                var v = pane.CurveList.Where(p => p.Label.Text == sn + "_" + tn).ToList();
                if (v.Count == 0)
                    AddLineItemToGraph(sn, tn, pane.CurveList.Count + 1);
            }
            else
            {
                AddLineItemToGraph(sn, tn, pane.CurveList.Count + 1);
            }
            zedGraphControl1.Refresh();
        }
        private void UnCompareGraph(string sn, string tn, bool isShow)
        {
            GraphPane pane = zedGraphControl1.GraphPane;
            pane.CurveList.Clear();
            pane.GraphObjList.Clear();
            AddLineItemToGraph(sn, tn, 1);
        }
        private void AddLineItemToGraph(string sn, string tn, int i)
        {
            if (zedGraphControl1.GraphPane.CurveList.Count <= 0)
                InitGraph();
            if (!IsCompare)
            {
                List<PointKeyValue> p = pointKeyValueList;
                GraphHelper.SetMinMaxLimits(Tag);
                GraphHelper.SetGraphDataSource(zedGraphControl1, Tag, selection, true);
                this.DrawLimitLine(zedGraphControl1.GraphPane, true, false);
                if (zedGraphControl1.GraphPane.Rect != RectangleF.Empty)
                {
                    try
                    {
                        int width = 730;
                        int height = (int)(730 * 0.63);
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
                GraphHelper.SetGraphDataSource(zedGraphControl1, Tag, selection, cbShowMark.Checked);
                //根据viewtool设置是否show limit及ideal range
                this.DrawLimitLine(zedGraphControl1.GraphPane, cbLimitLine.Checked, cbIdealRange.Checked);
                this.DrawIdealRange(zedGraphControl1.GraphPane, cbIdealRange.Checked, cbLimitLine.Checked);
            }
            else
            {
                ForEachTempByUnit();
                GraphHelper.SetGraphDataSource(zedGraphControl1, TagsList, selection, Tag.TempUnit,cbShowMark.Checked);
            }
            zedGraphControl1.Refresh();
        }
        private void SetAxis(object sender)
        {
            if (Tag == null)
                return;
            RadioButton rb = (RadioButton)sender;
            if (rb.Checked)
            {
                switch (rb.Text)
                {
                    case "Date/Time":
                        SetXAxisAsDataTime(1);
                        break;
                    case "Elapsed Time":
                        SetXAxisAsElapsedTime(1);
                        break;
                    default:
                        SetXAxisAsDataPoints(1);
                        break;
                }
                zedGraphControl1.GraphPane.XAxis.Title.Text = rb.Text;
                zedGraphControl1.Refresh();
                GraphHelper.ReDraw(true, zedGraphControl1, ref MouseRec);
            }
        }
        private void SetXAxisAsDataTime(int i)
        {
            selection = XAxisVisibility.DateAndTime;

            if (!IsCompare)
            {
                GraphHelper.SetGraphAsDefaultProperity(zedGraphControl1, selection);
                string sntn = Tag.SerialNumber + "_" + Tag.TripNumber;
                GraphHelper.SetGraphDataSource(zedGraphControl1, Tag, selection, cbShowMark.Checked);
                //根据viewtool设置是否show limit及ideal range
                this.DrawLimitLine(zedGraphControl1.GraphPane, cbLimitLine.Checked, cbIdealRange.Checked);
                DrawIdealRange(zedGraphControl1.GraphPane, cbIdealRange.Checked, cbLimitLine.Checked);
            }
            else
            {
                GraphHelper.SetXAxisAsDateTime(zedGraphControl1.GraphPane, TagsList, cbShowMark.Checked);
            }
            zedGraphControl1.Refresh();
        }
        private void SetXAxisAsDataPoints(int i)
        {
            selection = XAxisVisibility.DataPoints;

            if (!IsCompare)
            {
                GraphHelper.SetGraphAsDefaultProperity(zedGraphControl1, XAxisVisibility.DataPoints);
                string sntn = Tag.SerialNumber + "_" + Tag.TripNumber;
                GraphHelper.SetGraphDataSource(zedGraphControl1, Tag, XAxisVisibility.DataPoints,  cbShowMark.Checked);
                //根据viewtool设置是否show limit及ideal range
                this.DrawLimitLine(zedGraphControl1.GraphPane, cbLimitLine.Checked, cbIdealRange.Checked);
                DrawIdealRange(zedGraphControl1.GraphPane, cbIdealRange.Checked, cbLimitLine.Checked);
            }
            else
            {
                //GraphHelper.SetXAxisTextLabelByComparing(zedGraphControl1, selection, TagsList);
                GraphHelper.SetXAxisAsDataPoints(zedGraphControl1.GraphPane, TagsList, cbShowMark.Checked);
            }
            zedGraphControl1.Refresh();
        }
        private void SetXAxisAsElapsedTime(int i)
        {
            selection = XAxisVisibility.ElapsedTime;

            if (!IsCompare)
            {
                GraphHelper.SetGraphAsDefaultProperity(zedGraphControl1, XAxisVisibility.ElapsedTime);
                string sntn = Tag.SerialNumber + "_" + Tag.TripNumber;
                GraphHelper.SetGraphDataSource(zedGraphControl1, Tag, XAxisVisibility.ElapsedTime,cbShowMark.Checked);
                //根据viewtool设置是否show limit及ideal range
                this.DrawLimitLine(zedGraphControl1.GraphPane, cbLimitLine.Checked, cbIdealRange.Checked);
                DrawIdealRange(zedGraphControl1.GraphPane, cbIdealRange.Checked, cbLimitLine.Checked);
            }
            else
            {
                //GraphHelper.SetXAxisTextLabelByComparing(zedGraphControl1, selection, TagsList);
                GraphHelper.SetXAxisAsElapsedTime(zedGraphControl1.GraphPane, TagsList, cbShowMark.Checked);
            }
            zedGraphControl1.Refresh();
        }

        private void DrawLimitLine(GraphPane pane, bool isShowLimit, bool isShowIdeal)//画最高最低线
        {
            GraphHelper.DrawLimitLintAndFillIdeal(pane, Tag, isShowIdeal, isShowLimit);
        }
        private void DrawIdealRange(GraphPane pane, bool isShowIdeal, bool isShowLimit)
        {

            GraphHelper.DrawLimitLintAndFillIdeal(pane, Tag, isShowIdeal, isShowLimit);
        }
        private void DrawMark(object sender, EventArgs e)
        {
            if (!IsCompare)
            {
                GraphHelper.ReDrawUnCompareCurveItem(zedGraphControl1.GraphPane, Tag, selection, cbShowMark.Checked);
                DrawLimitLine(zedGraphControl1.GraphPane, cbLimitLine.Checked, cbIdealRange.Checked);
                DrawIdealRange(zedGraphControl1.GraphPane, cbIdealRange.Checked, cbLimitLine.Checked);
            }
            else
            {
                GraphHelper.ReDrawCompareCurveItem(zedGraphControl1.GraphPane, TagsList, selection, cbShowMark.Checked);
            }
            GraphHelper.ReDraw(true, zedGraphControl1, ref MouseRec);
            zedGraphControl1.Refresh();
        }
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
                return false;
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
                    return false;
            }
            else
            {
                if (IsRectanglePainted && MouseRec.Width != 0)
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
            Size tipSize = GetToolTipSize();
            rec = new Rec(MouseRec);
            if (e.Button == MouseButtons.Right)
            {
                if (IsMouseDown)
                {
                    zedGraphControl1.Capture = false;
                    Cursor.Clip = Rectangle.Empty;
                    IsMouseDown = false;
                    GraphHelper.DrawRectangle(zedGraphControl1, MouseRec);
                    if (MouseRec != null)
                    {
                        //GraphHelper.SetToolTip(zedGraphControl1, MouseRec, Tag, selection, toolTip);
                        GraphHelper.SetStatisticsTips(sender, MouseRec, m_RightMouseAnalyse, tipSize,e.Location);
                        GraphHelper.ReDraw(true, zedGraphControl1, ref MouseRec);
                        IsRectanglePainted = true;
                    }
                }
                return true;
            }
            else if (e.Button == MouseButtons.Left)
            {
                if (IsRectanglePainted && redimStatus != RedimStatus.None)
                {
                    GraphHelper.SetStatisticsTips(sender, MouseRec, m_RightMouseAnalyse, tipSize, e.Location);
                    GraphHelper.ReDraw(true, zedGraphControl1, ref MouseRec);
                    this.tableViewTool.Refresh();
                    return true;
                }
                else
                {
                    if (IsCompare)
                    {
                        GraphHelper.HandleZoomFinish(sender, new Point(e.X, e.Y), TagsList, selection,cbShowMark.Checked);
                    }
                    else
                    {
                        GraphHelper.HandleZoomFinish(sender, e.Location, Tag, selection, 1, false,cbShowMark.Checked);
                        DrawIdealRange(sender.GraphPane, cbIdealRange.Checked, cbLimitLine.Checked);
                        this.DrawLimitLine(sender.GraphPane, cbLimitLine.Checked, cbIdealRange.Checked);
                    }
                    MouseRec = Rectangle.Empty;
                    m_RightMouseAnalyse.Hide(zedGraphControl1);
                    sender.Refresh();
                    this.tableViewTool.Refresh();
                    return false;
                }
            }
            else
                return false;
        }
        private void RightMouseAnalyseDraw(object sender, DrawToolTipEventArgs e)
        {
            Graphics g = e.Graphics;
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            Font font = this.Font;
            Brush defaultBrush = new SolidBrush(Color.Black);
            List<string[]> result = GraphHelper.GetRightAnalysisTips(zedGraphControl1, MouseRec, TagsList, selection);
            if (result != null && result.Count > 0)
            {
                e.DrawBackground();
                float rowHeight = 18f;
                float maxColumnWidth = 0;
                float padding = 6;
                float fontHeight = g.MeasureString("Trip", font).Height;
                int dataLength = 7;
                int columnCount = 5;
                foreach (var column in result)
                {
                    for (int j = 0; j < column.Length; j++)
                    {
                        if (g.MeasureString(column[j], font).Width > maxColumnWidth)
                        {
                            maxColumnWidth = g.MeasureString(column[j], font).Width;
                        }
                    }
                }
                maxColumnWidth += padding;
                PointF location = new PointF(padding, padding + (rowHeight - fontHeight) / 2);
                for (int i = 0; i < result.Count; i++)
                {
                    if (i == columnCount)
                    {
                        location.X = padding;
                    }
                    string[] column = result[i];
                    for (int j = 0; j < column.Length; j++)
                    {
                        if (string.IsNullOrEmpty(column[j]))
                        {
                            continue;
                        }
                        g.DrawString(column[j], font, defaultBrush, location);
                        location.Y += rowHeight;
                    }
                    location.X += maxColumnWidth;
                    location.Y = padding + (rowHeight - fontHeight) / 2 + (rowHeight * (dataLength + 1) * ((i+1) / columnCount));
                }
                g.DrawLines(SystemPens.ControlDarkDark, new Point[] 
                {
	                new Point (0, e.Bounds.Height - 1),
	                new Point (0, 0),
	                new Point (e.Bounds.Width - 1, 0)
	            });
                g.DrawLines(SystemPens.ControlDarkDark, new Point[] 
                {	                   
                    new Point (0, e.Bounds.Height - 1),
	                new Point (e.Bounds.Width - 1, e.Bounds.Height - 1),	            
                    new Point (e.Bounds.Width - 1, 0)
	            });
            }
        }

        private void RightMouseAnalysePopUp(object sender, PopupEventArgs e)
        {

            e.ToolTipSize = GetToolTipSize();
        }
        private Size GetToolTipSize()
        {
            Size tipSize = new Size();

            using (Graphics g = this.CreateGraphics())
            {
                Font font = this.Font;
                using (Brush defaultBrush = new SolidBrush(Color.Black))
                {
                    List<string[]> result = GraphHelper.GetRightAnalysisTips(zedGraphControl1, MouseRec, TagsList, selection);
                    float totalWidth = 0;
                    float totalHeight = 0;
                    float padding = 6;
                    int columnCount = 5;
                    int dataLength = 7;
                    if (result != null && result.Count > 0)
                    {
                        float rowHeight = 18f;
                        float maxColumnWidth = 0;
                        foreach (var column in result)
                        {
                            for (int j = 0; j < column.Length; j++)
                            {
                                if (g.MeasureString(column[j], font).Width > maxColumnWidth)
                                {
                                    maxColumnWidth = g.MeasureString(column[j], font).Width;
                                }
                            }
                        }
                        maxColumnWidth += padding;
                        totalWidth = maxColumnWidth * (result.Count > columnCount ? columnCount : result.Count);
                        int rowCount = (result.Count > columnCount ? 2 : 1);
                        totalHeight = rowHeight * dataLength * rowCount;
                        if (rowCount > 1)
                        {
                            totalHeight += rowHeight;
                        }
                        totalHeight += padding * 2;
                        totalWidth += padding;
                        tipSize.Width = (int)Math.Ceiling(totalWidth);
                        tipSize.Height = (int)Math.Ceiling(totalHeight);
                    }
                }
            }
            return tipSize;
        }
        /// <summary>
        ///  重绘鼠标悬停事件
        /// </summary>
        /// <param name="control"></param>
        /// <param name="pane">绘图板</param>
        /// <param name="curve">曲线</param>
        /// <param name="iPt">坐标位置</param>
        /// <returns>label的显示值</returns>
        private string MyPointValueHandler(ZedGraphControl control, GraphPane pane,
                        CurveItem curve, int iPt)
        {
            Axis x = curve.GetXAxis(pane);
            PointPair pt = curve[iPt];
            SuperDevice tag = this.Tag;
            if (IsCompare)
            {
                var var = from p in TagsList
                          where p.SerialNumber + "_" + p.TripNumber == curve.Label.Text
                          select p;
                if (var.ToList() != null && var.ToList().Count > 0)
                {
                    tag = var.ToList().First();
                }
            }
            // Get the PointPair that is under the mouse
            List<PointKeyValue> list = GraphHelper.GetTempList(pane, tag, selection, (int)curve.Points[0].X, (int)Math.Ceiling(curve.Points[curve.Points.Count - 1].X));
            if (list != null && list.Count > iPt)
            {

                
                StringBuilder stringBuilder = new StringBuilder();
                string serial = string.Empty;
                string label = string.Empty;
                string pointIndex = string.Empty;
                string date = string.Empty;
                string time = string.Empty;
                if (tag != null)
                {
                    serial = string.Format("Serial Number: {0}", tag.SerialNumber);
                    label = string.Format("Trip Number: {0}", tag.TripNumber.IndexOf('_') == -1 ? tag.TripNumber : tag.TripNumber.Substring(0, tag.TripNumber.IndexOf('_')));
                }
                string value = string.Format("Vaule: {0:f1}°{1}", pt.Y, tag.TempUnit);

                date = string.Format("Date: {0}", list[iPt].PointTime.ToLocalTime().ToString(Common.GetDateOrTimeFormat(true, Common.GlobalProfile.DateTimeFormator), CultureInfo.InvariantCulture));
                time = string.Format("Time: {0}", list[iPt].PointTime.ToLocalTime().ToString(Common.GetDateOrTimeFormat(false, Common.GlobalProfile.DateTimeFormator), CultureInfo.InvariantCulture));
                PointKeyValue pkv = list.FirstOrDefault();
                if (selection == XAxisVisibility.DateAndTime)
                {
                    pkv.PointTime = pkv.PointTime.ToUniversalTime();
                }
                List<PointKeyValue> pointList = tag.tempList;
                int index = pointList.FindIndex(p => p.PointTemp == pkv.PointTemp && p.PointTime == pkv.PointTime && pkv.IsMark == p.IsMark) + iPt;
                pkv = tag.tempList[index];
                int markCount = tag.tempList.Where(p => p.PointTime <= pkv.PointTime && p.IsMark).Count();
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
        private string XAxisScaleLabels(GraphPane pane, Axis axis, double val, int index)
        {
            //string label=string.Empty;
            index *= (int)pane.XAxis.Scale.MajorStep;
            if (pane.XAxis.Scale.TextLabels == null || index < 0 || index >= pane.XAxis.Scale.TextLabels.Length)
                return string.Empty;
            else
                return pane.XAxis.Scale.TextLabels[index];
            //return label;
        }
        #endregion
        #region report editor

        private ReportEditor GetReportEditor(string sn, string tn)
        {
            ReportEditor editor = _reportEditorBll.GetReportEditorBySnTn(sn, tn);
            //if (editor != null)
            //{
            //    if (!string.IsNullOrWhiteSpace(editor.Comments) && ReportConstString.CommentDefaultString != editor.Comments)
            //    {
            //        tbReportComment.Text = editor.Comments;
            //    }
            //    if (!string.IsNullOrWhiteSpace(editor.ReportTitle) && ReportConstString.TitleDefaultString != editor.ReportTitle)
            //    {
            //        tbReportTitle.Text = editor.ReportTitle;
            //    }
            //}
            return editor;

        }
        #endregion
        #region export
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
                Common.SetDefaultPathForSaveFileDialog(file, SavingFileType.Report);
                file.FileName = Tag.SerialNumber + "_" + Tag.TripNumber;

                DigitalSignatureBLL _digitalBll = new DigitalSignatureBLL();
                this.digitalSignList = _digitalBll.GetDigitalSignatureBySnTn(Tag.SerialNumber, Tag.TripNumber);
                if (file.ShowDialog() == DialogResult.OK)
                {
                    string src = file.FileName.ToString();
                    //重新获取TAG对象
                    SuperDevice exportTag = Tag;
                    if (exportTag == null)
                        return;
                    switch (reportType)
                    {
                        case "tps":
                            var q = TagsList.Where(p => p.SerialNumber == exportTag.SerialNumber && p.TripNumber == exportTag.TripNumber).FirstOrDefault();
                            if (q != null)
                            {
                                exportTag = q;
                            }
                            ReportEditor editor = Common.GetReportEditorSelection(_reportEditorBll, Tag.SerialNumber, Tag.TripNumber, tbCmt.Text, tbReportTitle.Text);
                            SaveToTps(exportTag, digitalSignList, editor, src); ;
                            break;
                        case "xlsx":
                            exporter = new ExcelReportExporter(DeviceDataFrom.DataManager, exportTag, this.digitalSignList, src);
                            break;
                        case "pdf":
                            exporter = new PDFReportExporter(DeviceDataFrom.DataManager, exportTag, this.digitalSignList, src);
                            break;
                        default:
                            break;
                    }
                    if (exporter != null)
                    {
                        this.setReportPropertyAndSaveReport(exporter);
                    }
                }
            }
        }
        /// <summary>
        ///将对象保存到文件中
        /// </summary>
        private void SaveToTps(SuperDevice device, List<DigitalSignature> sign, ReportEditor editor, string path)
        {
            byte[] plaintext = Platform.Utils.SerializeToXML<SuperDevice>(device);
            DataSignature signature = SignatureHelper.CreateSignature(plaintext);
            signature.List = sign;
            signature.Editor = editor;
            byte[] tps = Platform.Utils.SerializeToXML<DataSignature>(signature);
            Platform.Utils.SaveTheFile(tps, path);
        }
        #endregion
        #region 设置搜索框
        private BackgroundWorker bw;
        private AutoCompleteStringCollection ac;
        /// <summary>
        /// 设置textbox智能提示输入框
        /// </summary>
        private void Intelligence()
        {
            this.tbSearch.TextChanged += new EventHandler((sender, args) =>
            {
                if (this.tbSearch.Text != Infrastructure.SearchRecordConst)
                {
                    this.tbSearch.ForeColor = Color.Black;
                }
                else
                {
                    this.tbSearch.ForeColor = Color.Silver;
                }
                GetHistoryByInput(this.tbSearch.Text);
            });
        }
        private void GetHistoryByInput(string input)
        {
            if (input == Infrastructure.SearchRecordConst)
                return;
            var v = from p in deviceList
                    where Convert.ToDateTime(p.Remark).Date >= dtpHistoryFrom.Value.Date &&
                          Convert.ToDateTime(p.Remark).Date <= dtpHistoryTo.Value.Date && (p.SerialNum + "_" + p.TripNum).IndexOf(input) == 0
                    select p;
            if (v != null)
            {
                List<Device> result = v.ToList();
                m_HistoryDataViewModel.Condition.RecordName = input;
                Dictionary<string, bool> dic = GetHistoryDataCheckState();
                CtorRecordList();
                if (result.Count == 0)
                    ResetInitState();
                else
                {
                    SetHistoryDataCheckState(dic);
                    ResetInitState();
                    InitDataManager();
                }
            }
        }
        private Dictionary<string, bool> GetHistoryDataCheckState()
        {
            Dictionary<string, bool> dic = new Dictionary<string, bool>();//临时存储check state
            for (int i = 0; i < dgvHistory.RowCount; i++)
            {
                string sn_tn = dgvHistory.Rows[i].Cells["record"].Value.ToString();
                if (sn_tn.LastIndexOf("\n") != -1)
                {
                    sn_tn = sn_tn.Substring(0, sn_tn.LastIndexOf("\n"));
                }
                bool isShow = Convert.ToBoolean(dgvHistory.Rows[i].Cells["CheckCol"].EditedFormattedValue);
                if (!dic.Keys.Contains(sn_tn))
                {
                    dic.Add(sn_tn, isShow);
                }
            }
            return dic;
        }
        private void SetHistoryDataCheckState(Dictionary<string, bool> dic)
        {
            for (int i = 0; i < dgvHistory.RowCount; i++)
            {
                string sn_tn = dgvHistory.Rows[i].Cells["record"].Value.ToString();
                if (sn_tn.LastIndexOf("\n") != -1)
                {
                    sn_tn = sn_tn.Substring(0, sn_tn.LastIndexOf("\n"));
                }
                string[] sntn = new string[2] { string.Empty, string.Empty };
                if (sn_tn.IndexOf('_') != -1)
                {
                    sntn[0] = sn_tn.Substring(0, sn_tn.IndexOf('_'));
                    sntn[1] = sn_tn.Substring(sn_tn.IndexOf('_') + 1);
                }
                if (dic.ContainsKey(sntn[0] + "_" + sntn[1]))
                {
                    dgvHistory.Rows[i].Cells["CheckCol"].Value = dic[sntn[0] + "_" + sntn[1]];
                    dgvFloatingHistory.Rows[i].Cells["CheckCol"].Value = dic[sntn[0] + "_" + sntn[1]];
                }
            }
        }
        //private bool searchdown;
        #endregion
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

        public void Print(bool isPreview)
        {
            if (this.Tag != null && !string.IsNullOrWhiteSpace(Tag.SerialNumber))
            {
                FileHelper.DeleteTempFiles("print");
                string tempFileName = Path.Combine(System.Windows.Forms.Application.StartupPath, "temp", string.Format("{0}_{1}.print", Tag.SerialNumber, Tag.TripNumber));
                IReportExportService exporter = new PDFReportExporter(DeviceDataFrom.DataManager, this.Tag, this.digitalSignList, tempFileName, true);
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

        public void Email()
        {
            if (this.Tag != null && !string.IsNullOrWhiteSpace(Tag.SerialNumber))
            {

                string fileNameWithFullPath = Path.Combine(System.Windows.Forms.Application.StartupPath, "temp", string.Format("{0}.tps", Tag.SerialNumber + "_" + Tag.TripNumber));
                DigitalSignatureBLL _digitalBll = new DigitalSignatureBLL();
                this.digitalSignList = _digitalBll.GetDigitalSignatureBySnTn(Tag.SerialNumber, Tag.TripNumber);
                ReportEditor editor = Common.GetReportEditorSelection(_reportEditorBll, Tag.SerialNumber, Tag.TripNumber, tbCmt.Text, tbReportTitle.Text);
                SuperDevice exportTag = Tag;
                var q = TagsList.Where(p => p.SerialNumber == exportTag.SerialNumber && p.TripNumber == exportTag.TripNumber).FirstOrDefault();
                if (q != null)
                {
                    exportTag = q;
                }
                SaveToTps(exportTag, digitalSignList, editor, fileNameWithFullPath);
                new ReportOutlookEmailer().CreateEmailAndAddAttachments(fileNameWithFullPath);
                FileHelper.DeleteTempFiles(fileNameWithFullPath);
            }
        }
        #region signature
        private void SetSignedRecordLabel(List<DigitalSignature> list)
        {
            if (list != null && list.Count > 0)
            {
                list = list.OrderBy(p => p.SignTime).ToList();
                string signtext = string.Format("Signature[{0}]: {1} ", list.Count, list.Last().ToString(Common.GlobalProfile.DateTimeFormator));
                SetSignLabel(signtext);
                //设置电子签名list
                GenerateSignatureColumns();
                BindSignature(list);
                digitalSignList = list;
            }
            else
            {
                lbSign.Text = "Unsigned";
                lvSignature.Rows.Clear();
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
                    MinimumWidth=150,
                    AutoSizeMode=DataGridViewAutoSizeColumnMode.Fill
                });
            }
        }
        private void BindSignature(List<DigitalSignature> list)
        {
            lvSignature.Rows.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                lvSignature.Rows.Add();
                lvSignature.Rows[i].Cells["ID"].Value = i+1;
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
                this.tips.SetToolTip(lbSign, text);
            }
            catch (Exception exp)
            {
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
                        string[] reader = text.Split(new[] { '@' },StringSplitOptions.RemoveEmptyEntries);
                        if (reader.Length > 0)
                        {
                            for (int i = reader[0].Length - 1; i >= 0; i--)
                            {
                                s = reader[0].Substring(0, i) + "...@" + reader[1];
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
                
            }
        }
        private void SetDynamicLabelText(object sender, EventArgs args)
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
                this.tips.SetToolTip(lb, text);
            }
            catch (Exception exp)
            {
            }
        }
        private string GetDynamicText(string text,System.Windows.Forms.Label lb)
        {
            try
            {
                if (string.IsNullOrEmpty(text))
                    return string.Empty;
                int width = TextRenderer.MeasureText(text, lbSign.Font).Width;
                if (width > 221)
                {
                    string s = string.Empty;
                    int index = 0;
                    for (int i = text.Length - 1; i >= 0; i--)
                    {
                        s = text.Substring(0, i) + "...";
                        index = i;
                        width = TextRenderer.MeasureText(s, lbSign.Font).Width;
                        if (width > 221)
                            continue;
                        else
                            break;
                    }
                    return s;
                }
                else
                    return text;
            }
            catch (Exception exp)
            {
                return string.Empty;
            }
        }
        public void Save()
        {
            if (_IsFromTps)
            {
                Common.SaveTps(ref IsSaved, digitalSignList, _reportEditorBll, _deviceBll, _alarmConfigBll, _logConfigBll, Tag, tbCmt.Text, tbReportTitle.Text);
                if (IsSaved)
                {
                    if (Tag != null)
                    {
                        Common.AddSaveRecordLog(logBll, Tag);
                        Common.AddCommentsLog(logBll, Tag, tbCmt);
                    }
                    ConnectionController.SetSaveButtonState(btnSave, false);
                    SetTpsControlState(true);
                    bool isShow = true;
                    string sn = Tag.SerialNumber, tn = Tag.TripNumber;
                    ReSetHistoryRecords(sn, tn);
                    ConnectionController.SetSignButtonState(btnSign, true);
                    GetDeviceObjects(sn, tn);
                    InitDeviceType(_device, _logConfig
                                     , _pointInfo, _alarmConfig, isShow);
                    UncompareDataListShow(sn, tn, isShow);
                    UnCompareDataList(sn, tn, isShow);
                    UnCompareGraph(sn, tn, isShow);
                    InitTpSingleSummary(Tag);
                    SetSignedRecordLabel(digitalSignList);
                    InitTpReportEditor();
                    _IsFromTps = false;
                    this.lblImportedTpsFileName.Text = string.Empty;
                }
            }
            else
            {
                if ((editor.Comments != tbCmt.Text.Trim())
                || (editor.ReportTitle != tbReportTitle.Text.Trim() && tbReportTitle.Text != ReportConstString.TitleDefaultString))
                {
                    Common.Save(ref IsSaved, digitalSignList, _reportEditorBll, _deviceBll, Tag, tbCmt.Text, tbReportTitle.Text, editor);
                    if (IsSaved)
                    {
                        if (Tag != null)
                        {
                            Common.AddSaveRecordLog(logBll, Tag);
                            Common.AddCommentsLog(logBll, Tag, tbCmt);
                        }
                        ConnectionController.SetSaveButtonState(btnSave, false);
                    }
                }
            }
            
        }
        public void Save(SuperDevice Tag)
        {
            if ((editor.Comments != tbCmt.Text.Trim())
                || (editor.ReportTitle != tbReportTitle.Text.Trim() && tbReportTitle.Text != ReportConstString.TitleDefaultString))
            {
                Common.Save(ref IsSaved, digitalSignList, _reportEditorBll, _deviceBll, Tag, tbCmt.Text, tbReportTitle.Text, editor);
                if (IsSaved)
                {
                    if (Tag != null)
                    {
                        Common.AddSaveRecordLog(logBll, Tag);
                        Common.AddCommentsLog(logBll, Tag, tbCmt);
                    }
                }
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

        private void lblReportEditorTip_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowAdminForm(sender, e);
        }
        private void SetViewToolState()
        {

            zedGraphControl1.Refresh();
            if (IsCompare)
            {
                zedGraphControl1.GraphPane.GraphObjList.Clear();
                zedGraphControl1.Refresh();
                cbLimitLine.Enabled = cbIdealRange.Enabled = false;
                cbLimitLine.Checked = cbIdealRange.Checked = false;
                rbDtaPoints.Checked = true;
                selection = XAxisVisibility.DataPoints;
                SetAxis(rbDtaPoints);
                zedGraphControl1.Refresh();
            }
            else
            {
                cbLimitLine.Enabled = cbIdealRange.Enabled = true;
                rbDateTime.Checked = true;
                selection = XAxisVisibility.DateAndTime;
                SetAxis(rbDateTime);
                this.DrawLimitLine(zedGraphControl1.GraphPane, cbLimitLine.Checked, cbIdealRange.Checked);
                DrawIdealRange(zedGraphControl1.GraphPane, cbIdealRange.Checked, cbLimitLine.Checked);
            }
        }

        public DialogResult DeviceManagerExitDialog()
        {
            return DeviceManagerExitDialog(MessageBoxButtons.YesNoCancel);
        }

        public DialogResult DeviceManagerExitDialog(MessageBoxButtons buttons)
        {
            if (IsCompare)
            {
                return DialogResult.OK;
            }
            if (_IsFromTps)
            {
                DialogResult dialogResult = Utils.ShowMessageBox(Messages.B19, Messages.TitleNotification, buttons);
                if (DialogResult.Yes == dialogResult || DialogResult.OK == dialogResult)
                {
                    this.Save();
                }
                else if (DialogResult.No == dialogResult)
                {
                    this.ResetInitState();
                    this.TagsList.Clear();
                    _IsFromTps = false;
                    this.lblImportedTpsFileName.Text = string.Empty;
                }
                return dialogResult;
            }
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
            return DialogResult.OK;
            
        }

        public void DeviceManagerExitDialog(SuperDevice Tag)
        {
            bool result = Common.IsDeviceModification(Tag, tbCmt, tbReportTitle);
            if (result == true)
            {
                if (DialogResult.Yes == Utils.ShowMessageBox(Messages.B20, Messages.TitleNotification, MessageBoxButtons.YesNo))
                {
                    this.Save(Tag);
                }
            }
        }
        private void Sign(object sender, EventArgs args)
        {
            int result = Common.DeviceModificationType(Tag, tbCmt, tbReportTitle);
            if (result != -1)
            {
                string m = result == 0 ? Messages.B19 : Messages.B20;
                if (DialogResult.OK == Utils.ShowMessageBox(m, Messages.TitleNotification, MessageBoxButtons.OKCancel))
                {
                    Common.Save(ref IsSaved, digitalSignList, _reportEditorBll, _deviceBll, Tag, tbCmt.Text, tbReportTitle.Text, editor);
                    if (IsSaved)
                    {
                        if (Tag != null)
                        {
                            Common.AddSaveRecordLog(logBll, Tag);
                            Common.AddCommentsLog(logBll, Tag, tbCmt);
                        }
                        Common.Sign(IsSaved, Tag, ref digitalSignList);
                        editor = _reportEditorBll.GetReportEditorBySnTn(Tag.SerialNumber, Tag.TripNumber);
                        this.InitTpReportEditor();
                        SetSignedRecordLabel(digitalSignList);
                        GetHistoryDataSource();
                        CtorDataTimeFilter();
                        Dictionary<string, bool> dic = GetHistoryDataCheckState();
                        CtorRecordList();
                        SetHistoryDataCheckState(dic);
                    }
                }
            }
            else
            {
                Common.Sign(IsSaved, Tag, ref digitalSignList);
                this.InitTpReportEditor();
                SetSignedRecordLabel(digitalSignList);

                GetHistoryDataSource();
                CtorDataTimeFilter();
                Dictionary<string, bool> dic = GetHistoryDataCheckState();
                CtorRecordList();
                SetHistoryDataCheckState(dic);
            }
        }

        public event EventHandler OnComapreStatusChange;

        public bool IsDeviceNullOrNoValidData
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
        public bool LoadTps()
        {
            OpenFileDialog file = new OpenFileDialog();
            Common.SetDefaultPathForOpenFileDialog(file, OpenFileType.OpenTPS);
            file.Filter = "Tps Files(.tps)|*.tps";
            bool result = false;
            if (file.ShowDialog() == DialogResult.OK)
            {
                string src = file.FileName.ToString();
                if (ReadFromTps(src))
                {
                    //左侧列表不能使用在保存前
                    SetTpsControlState(false);
                    InitViewManage();
                    this.btnSave.Enabled = true;
                    this.btnSign.Enabled = false;
                    _IsFromTps=result = true;
                    if (src.Length > src.LastIndexOf("\\") + 1)
                    {
                        this.lblImportedTpsFileName.Text = string.Format("File imported: {0}", src.Substring(src.LastIndexOf("\\") + 1));
                    }
                }
                else
                    result = false;
            }
            return result;
        }
        private bool ReadFromTps(string path)
        {
            byte[] tps = Platform.Utils.ReadFromFile(path);
            DataSignature ds = new DataSignature();
            try
            {
                ds = Platform.Utils.DeserializeFromXML<DataSignature>(tps, typeof(DataSignature));//反序列化
            }
            catch
            {
                Utils.ShowMessageBox(Platform.Messages.FileDamage, Platform.Messages.TitleWarning);
                return false;
            }
            if (SignatureHelper.VerifySignature(ds))
            {
                this.rbUnCompare.Checked = true;
                ClearDataRecordsPanel();
                //if (Tag == null)
                //    Tag = ObjectManage.GetDeviceInstance(DeviceType.ITAGSingleUse);
                //Tag = Platform.Utils.DeserializeFromXML<SuperDevice>(ds.Data, Tag.GetType());
                Tag = Platform.Utils.DeserializeFromXML<SuperDevice>(ds.Data, typeof(SuperDevice));
                var s = (from p in Tag.tempList
                         where p.PointTemp == Tag.tempList.Select(t => t.PointTemp).Max()
                         select p.PointTemp.ToString() + "°" + Tag.TempUnit + "@" + p.PointTime.ToString(Common.GetDefaultDateTimeFormat(), CultureInfo.InvariantCulture));
                Tag.HighestC = s.ToList().Count == 0 ? "" : s.ToList().First();
                s = (from p in Tag.tempList
                     where p.PointTemp == Tag.tempList.Select(t => t.PointTemp).Min()
                     select p.PointTemp + "°" + Tag.TempUnit + "@" + p.PointTime.ToString(Common.GetDefaultDateTimeFormat(), CultureInfo.InvariantCulture));
                Tag.LowestC = s.ToList().Count == 0 ? "" : s.First();
                //ObjectManage.SetDevice(Tag);
                SetReportEditorFromTps(ds.Editor);
                SetSignedRecordLabel(ds.List);
                SetTemperatureUnit();
                return true;
            }
            else
            {
                Utils.ShowMessageBox(Messages.UnauthorizedTps, Messages.TitleError);
                return false;
            }
        }

        private void SetTemperatureUnit()
        {
            if (Tag != null)
            {
                if ("C" == Tag.TempUnit)
                {
                    this.rbTempUnitC.Checked = true;
                }
                else if ("F" == Tag.TempUnit)
                {
                    this.rbTempUnitF.Checked = true;
                }
                else
                {
                    // nothing to do
                }
            }
        }

        private void SetReportEditorFromTps(ReportEditor editor)
        {
            this.editor = editor;
        }
        private void SetTpsControlState(bool isEnable)
        {
            pnHis.Enabled = isEnable;
            dgvHistory.ClearSelection();
        }
        public void InitViewManage()
        {
            if (Tag != null)
            {
                SuperDevice currentTag;
                string currentTagUnit = "";
                if (rbTempUnitC.Checked)
                    currentTagUnit = "C";
                else if (rbTempUnitF.Checked)
                    currentTagUnit = "F";
                else
                {
                    currentTagUnit = Tag.TempUnit;
                    rbTempUnitC.Checked = Tag.TempUnit == "C" ? true : false;
                    rbTempUnitF.Checked = Tag.TempUnit == "F" ? true : false;
                }
                if (Tag.TempUnit != currentTagUnit)
                    currentTag = Common.CopyTo(Tag);
                else
                    currentTag = Tag;
                TagsList.Clear();
                TagsList.Add(Tag);
                InitTpSingleSummary(currentTag);
                InitGraph(currentTag);
                InitTpDataList(currentTag);
                InitTpReportEditor();
            }
        }
        /// <summary>
        /// 对图进行初始化
        /// </summary>
        public void InitGraph(SuperDevice Tag)
        {
            if (Tag == null || Tag.tempList.Count == 0)
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
            if (zedGraphControl1.GraphPane.Rect != RectangleF.Empty)
            {
                try
                {
                    int width = 730;
                    int height = (int)(730 * 0.63);
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
            GraphHelper.SetGraphDataSource(zedGraphControl1, Tag, selection, cbShowMark.Checked);
            //根据viewtool设置是否show limit及ideal range
            this.DrawLimitLine(zedGraphControl1.GraphPane, cbLimitLine.Checked, cbIdealRange.Checked);
            DrawIdealRange(zedGraphControl1.GraphPane, cbIdealRange.Checked, cbLimitLine.Checked);
            zedGraphControl1.Refresh();
            GraphPane pane = zedGraphControl1.GraphPane;
        }
        public void InitTpDataList(SuperDevice Tag)
        {
            if ( Tag != null)
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
                List<Tuple<int, DateTime, double, string>> list = this.GetPointTempValue(Tag);
                int i = 0;
                if (list != null && list.Count > 0)
                {
                    list.ForEach(p =>
                    {
                        dgvList.Rows.Add(1);
                        dgvList.Rows[i].Cells["ID"].Value = p.Item1;
                        dgvList.Rows[i].Cells["PointTemp"].Value = p.Item4;
                        //if (cbElapsed.Checked)
                        dgvList.Rows[i].Cells["interval"].Value = TempsenFormatHelper.ConvertSencondToFormmatedTime(p.Item3.ToString());
                        //if (cbDate.Checked)
                        dgvList.Rows[i].Cells["PointTime"].Value = p.Item2.ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture);
                        i++;
                    });
                    dgvList.Columns["interval"].Visible = cbElapsed.Checked;
                    dgvList.Columns["PointTime"].Visible = cbDate.Checked;
                }
            }
        }
        private List<Tuple<int, DateTime, double, string>> GetPointTempValue(SuperDevice Tag)
        {
            List<PointKeyValue> pointList = Tag.tempList;
            int i = 0;
            List<Tuple<int, DateTime, double, string>> list = new List<Tuple<int, DateTime, double, string>>();
            pointList.ToList().ForEach(p =>
            {
                Tuple<int, DateTime, double, string> tuple = new Tuple<int, DateTime, double, string>(i + 1
                                                                                                     , p.PointTime.ToLocalTime()
                                                                                                     , i * Convert.ToDouble(Tag.LogInterval)
                                                                                                     , p.PointTemp.ToString("F1"));
                list.Add(tuple);
                i++;
            });
            return list;
        }
        private void SetSaveAndSignState(SuperDevice Tag)
        {
            if (Tag == null)
                return;
            ConnectionController.SetSignButtonState(btnSign, Tag.tempList.Count == 0 ? false : true);
            ConnectionController.SetSaveButtonState(btnSave, Common.IsDeviceModification(Tag, tbCmt, tbReportTitle));
        }
        private void ReSetHistoryRecords(string sn,string tn)
        {
            deviceList = _deviceBll.GetDeviceList().OrderByDescending(p => Convert.ToDateTime(p.Remark)).ToList();
            if (deviceList != null && deviceList.Count > 0)
            {
                GetHistoryDataSource();
                CtorDataTimeFilter();
                CtorRecordList();
                for (int i = 0; i < dgvHistory.Rows.Count; i++)
                {
                    if (dgvHistory.Rows[i].Cells["record"].Value.ToString() == sn + "_" + tn)
                    {
                        dgvHistory.Rows[i].Cells["CheckCol"].Value = true;
                        dgvFloatingHistory.Rows[i].Cells["CheckCol"].Value = true;
                        break;
                    }
                }
            }
            else
            {
                dgvHistory.Rows.Clear();
            }
        }
        public bool _IsFromTps = false;
        #region 浮动框 和正常框
        private HistoryRecordDataViewModel m_HistoryDataViewModel;
        private void GetHistoryDataSource()
        {
            if (m_HistoryDataViewModel == null)
            {
                m_HistoryDataViewModel = new HistoryRecordDataViewModel(_deviceBll);
            }
            else
            {
                m_HistoryDataViewModel.Clear();
            }
            //IList<HistoryRecordData> list = m_HistoryDataViewModel.HistoryData;
        }
        private HistoryRecordDataCondition GetCurrentRecordCondition(DateTime start,DateTime end,string recordName)
        {
            HistoryRecordDataCondition condition = new HistoryRecordDataCondition();
            condition.Start = start;
            condition.End = end;
            condition.RecordName = recordName;
            return condition;
        }
        private void GenerateFloatingDataView()
        {
            dgvFloatingHistory.Columns.Clear();
            dgvFloatingHistory.Rows.Clear();
            dgvFloatingHistory.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFloatingHistory.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvFloatingHistory.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 9, FontStyle.Regular);
            dgvFloatingHistory.Columns.Add(new DataGridViewCheckBoxColumn()
            {
                CellTemplate = new DataGridViewCheckBoxCell(),
                HeaderText = "",
                Name = "CheckCol",
                Width = 17
            });
            dgvFloatingHistory.Columns.Add(new DataGridViewColumn()
            {
                CellTemplate = new DataGridViewTextBoxCell(),
                HeaderText = "Record",
                Name = "record",
                Width = 215,
                Resizable = DataGridViewTriState.False,
                ReadOnly = true
            });
            dgvFloatingHistory.Columns.Add(new DataGridViewColumn()
            {
                CellTemplate = new DataGridViewTextBoxCell(),
                HeaderText = "Description",
                Name = "desc",
                Width = 140,
                ReadOnly = true
            });
            dgvFloatingHistory.Columns.Add(new DataGridViewColumn()
            {
                CellTemplate = new DataGridViewTextBoxCell(),
                HeaderText = "Start Time",
                Name = "LogStartTime",
                SortMode = DataGridViewColumnSortMode.Automatic,
                Width = 140,
                ReadOnly = true
            });
            dgvFloatingHistory.Columns.Add(new DataGridViewColumn()
            {
                CellTemplate = new DataGridViewTextBoxCell(),
                HeaderText = "Created Time",
                Name = "CreateTime",
                SortMode = DataGridViewColumnSortMode.Automatic,
                Width = 140,
                ReadOnly = true
            });
            dgvFloatingHistory.Columns.Add(new DataGridViewColumn()
            {
                CellTemplate = new DataGridViewTextBoxCell(),
                HeaderText = "Signature",
                Name = "SignatureTimes",
                Width = 60,
                ReadOnly = true
            });
            dgvFloatingHistory.Columns.Add(new DataGridViewColumn()
            {
                CellTemplate = new DataGridViewTextBoxCell(),
                HeaderText = "Alarm Status",
                Name = "AlarmStatus",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                Resizable = DataGridViewTriState.False,
                MinimumWidth = 60,
                ReadOnly = true
            });
        }
        private void GenerateNormalDataView()
        {
            dgvHistory.Columns.Clear();
            dgvHistory.Rows.Clear();
            dgvHistory.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHistory.RowsDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvHistory.ColumnHeadersDefaultCellStyle.Font = new System.Drawing.Font("Arial", 9, FontStyle.Regular);
            dgvHistory.Columns.Add(new DataGridViewCheckBoxColumn()
            {
                CellTemplate = new DataGridViewCheckBoxCell(),
                Name = "CheckCol",
                Resizable = DataGridViewTriState.False,
                Width = 18
            });
            dgvHistory.Columns.Add(new DataGridViewColumn()
            {
                CellTemplate = new DataGridViewTextBoxCell(),
                HeaderText = "Record",
                Name = "record",
                Width = 185,
                Resizable = DataGridViewTriState.False,
                AutoSizeMode=DataGridViewAutoSizeColumnMode.Fill,
                ReadOnly = true
            });
            dgvHistory.Columns["CheckCol"].HeaderCell.Value = Properties.Resources.detailed_list;
            dgvHistory.Columns["CheckCol"].HeaderCell.ToolTipText = "Detailed List";
        }
        private void BindDataToHistoryDataView()
        {
            MenuItem item = null;
            if (contextMenu != null && contextMenu.MenuItems.Count > 0)
                item = contextMenu.MenuItems[0];

            for (int i = 0; i < m_HistoryDataViewModel.HistoryData.Count; i++)
            {
                dgvHistory.Rows.Add();
                dgvFloatingHistory.Rows.Add();
                bool isShowDescription = false;
                if (item == null)
                {
                    isShowDescription = false;
                }
                else
                {
                    isShowDescription = item.Checked;
                }
                CtorNormalDataGridViewRow(dgvHistory.Rows[i], m_HistoryDataViewModel.HistoryData[i], isShowDescription);
                CtorFloatingDataViewRow(dgvFloatingHistory.Rows[i], m_HistoryDataViewModel.HistoryData[i]);
            }
        }
        private void CtorFloatingDataViewRow(DataGridViewRow row, HistoryRecordData recordData)
        {
            row.Cells["CheckCol"].Value = false;
            row.Cells["record"].Value = recordData.ToString();
            row.Cells["desc"].Value = recordData.DESCS;
            row.Cells["LogStartTime"].Value = recordData.LogStartTime.ToLocalTime().ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture);
            row.Cells["CreateTime"].Value = Convert.ToDateTime(recordData.CreateTime).ToLocalTime().ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture);
            row.Cells["SignatureTimes"].Value = recordData.SignatureTimes;
            row.Cells["AlarmStatus"].Value = recordData.AlarmStatus;
        }
        private void CtorNormalDataGridViewRow(DataGridViewRow row,HistoryRecordData recordData,bool isShowDescription)
        {
            row.Cells["CheckCol"].Value = false;
            if (!isShowDescription)
            {
                row.Cells["record"].Value = recordData.ToString();
            }
            else if (!string.IsNullOrEmpty(recordData.DESCS))
            {
                row.Cells["record"].Value = string.Format("{0}\n{1}", recordData.ToString(), GetDynamicText(recordData.DESCS, label18));
            }
            else
            {
                row.Cells["record"].Value = recordData.ToString();
            }
        }
        private void CtorDataTimeFilter()
        {
            dtpHistoryFrom.Format = DateTimePickerFormat.Custom;
            dtpHistoryFrom.CustomFormat = Common.GlobalProfile.DateTimeFormator.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
            dtpHistoryTo.Format = DateTimePickerFormat.Custom;
            dtpHistoryTo.CustomFormat = Common.GlobalProfile.DateTimeFormator.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[0];
            if (m_HistoryDataViewModel.HistoryData.Count > 0)
            {
                dtpHistoryFrom.ValueChanged -= new EventHandler(DataTimeFromChanged);
                dtpHistoryTo.ValueChanged -= new EventHandler(DataTimeToChanged);
                dtpHistoryFrom.Value = m_HistoryDataViewModel.Condition.Start.ToLocalTime();
                dtpHistoryTo.Value = m_HistoryDataViewModel.Condition.End.ToLocalTime();
                dtpHistoryFrom.ValueChanged += new EventHandler(DataTimeFromChanged);
                dtpHistoryTo.ValueChanged += new EventHandler(DataTimeToChanged);
            }
        }

        private void ShowRecordList(bool isShow)
        {
            pnFullHistoryRecord.Visible = isShow;
            pnFullHistoryRecord.BringToFront();
        }
        private void PresentData(int rowIndex,string[] sntn,bool isShow,bool isReadOnly)
        {
            _IsFromTps = false;
            this.lblImportedTpsFileName.Text = string.Empty;
            if (sntn != null && sntn.Length >= 2)
            {
                SuperDevice tmp = Tag;
                GetDeviceObjects(sntn[0], sntn[1]);
                InitDeviceType(_device, _logConfig
                                 , _pointInfo, _alarmConfig, isShow);
                if (!IsCompare && !isShow)
                {
                    //dgvHistory.Rows[e.RowIndex].Cells["CheckCol"].Value = !isShow;
                    return;
                }
                else if (!IsCompare && isShow && isReadOnly == false)
                {

                    UncompareDataHistoryShow(rowIndex);
                    DeviceManagerExitDialog(tmp);
                    if (_pointInfo != null)
                    {
                        if (_pointInfo.TempUnit == "C" && !rbTempUnitC.Checked)
                        {
                            rbTempUnitC.Checked = true;
                        }
                        else if (_pointInfo.TempUnit == "F" && !rbTempUnitF.Checked)
                        {
                            rbTempUnitF.Checked = true;
                        }
                    }

                }
                else if (IsCompare)//compare状态下
                {
                    CompareSummary(sntn[0], sntn[1], isShow);
                }
            }
        }

        public void RefreshHistoryRecordData()
        {
            m_HistoryDataViewModel.Condition = GetCurrentRecordCondition(dtpHistoryFrom.Value, dtpHistoryTo.Value, tbSearch.Text.Trim());
            if (m_HistoryDataViewModel.HistoryData != null)
            {
                Dictionary<string, bool> dic = GetHistoryDataCheckState();
                CtorRecordList();
                SetHistoryDataCheckState(dic);
                ResetInitState();
                InitDataManager();
            }
        }
        //事件
        private void DataTimeFromChanged(object sender, EventArgs e)
        {
            RefreshHistoryRecordData();
        }
        private void DataTimeToChanged(object sender, EventArgs e)
        {
            RefreshHistoryRecordData();
        }
        private void DataGridCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                isRefeshing = true;
                this.rbTempUnitC.CheckedChanged -= new EventHandler(TempUnitChange);
                if (e.ColumnIndex == 0 && e.RowIndex != -1)
                {
                    int checkedRecordsLimit = 10;
                    int actualCheckedRecords = 0;
                    DataGridView dgv = sender as DataGridView;
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        bool isRecordChecked = Convert.ToBoolean(dgv.Rows[i].Cells["CheckCol"].EditedFormattedValue);
                        if (isRecordChecked)
                        {
                            actualCheckedRecords++;
                        }
                    }
                    if (actualCheckedRecords > checkedRecordsLimit)
                    {
                        var checkBox = dgv.Rows[e.RowIndex].Cells["CheckCol"] as DataGridViewCheckBoxCell;
                        if (checkBox != null)
                        {
                            checkBox.EditingCellFormattedValue = false;
                        }
                        Utils.ShowMessageBox(Messages.CheckedRecordsOverLimit, Messages.TitleNotification);
                        return;
                    }
                    string sn_tn = dgv.Rows[e.RowIndex].Cells["record"].Value.ToString();
                    if (sn_tn.LastIndexOf("\n") != -1)
                    {
                        sn_tn = sn_tn.Substring(0, sn_tn.LastIndexOf("\n"));
                    }
                    string[] sntn = new string[2] { string.Empty, string.Empty };
                    if (sn_tn.IndexOf('_') != -1)
                    {
                        sntn[0] = sn_tn.Substring(0, sn_tn.IndexOf('_'));
                        sntn[1] = sn_tn.Substring(sn_tn.IndexOf('_') + 1);
                    }
                    
                    bool isShow = Convert.ToBoolean(dgv.Rows[e.RowIndex].Cells["CheckCol"].EditedFormattedValue);
                    bool isReadOnly = dgv.Rows[e.RowIndex].Cells["CheckCol"].ReadOnly;
                    dgvHistory.Rows[e.RowIndex].Cells["CheckCol"].Value = isShow;
                    dgvFloatingHistory.Rows[e.RowIndex].Cells["CheckCol"].Value = isShow;
                    PresentData(e.RowIndex, sntn, isShow, isReadOnly);
                }
                isRefeshing = false;
                
                InitDataManager();
                rbTempUnitC.CheckedChanged += new EventHandler(TempUnitChange);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
            finally
            {
                isRefeshing = false;
            }
        }
        private void FloatingDataGridCellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }
        private void DetailedListClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex == 0)
            {
                m_IsHoverDetailedHeader = false;
                dgvHistory.Refresh();
                ShowRecordList(true);
            }
        }
        private void BackToRecordListClick(object sender, EventArgs e)
        {
            ShowRecordList(false);
        }
        private void RePaintHeader(object sender, DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                if (e.RowIndex == -1 && e.ColumnIndex == 0)
                {
                    Bitmap headerIcon = Properties.Resources.detailed_list;
                    if (m_IsHoverDetailedHeader)
                    {
                        headerIcon = Properties.Resources.detailed_list_hover;
                    }
                    e.PaintBackground(e.CellBounds, true);
                    e.Graphics.DrawImage(headerIcon, e.CellBounds.Left+4, e.CellBounds.Top+5, 10, 10);//绘制图标
                    e.PaintContent(e.CellBounds);
                    e.Handled = true;
                }
            }
            catch
            {
            }
        }
        #endregion
        private void DrawPanelBorderPaint(object sender, PaintEventArgs e)
        {
            Panel panel = sender as Panel;
            Brush brush = new SolidBrush(Color.FromArgb(102, 153, 255));
            using (Pen pen = new Pen(brush, 1F))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, panel.Width - 1, panel.Height - 1);
            }
        }
       
    }
}
