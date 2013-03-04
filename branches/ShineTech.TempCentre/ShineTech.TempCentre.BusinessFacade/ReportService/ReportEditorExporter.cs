using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using System.IO;
using ShineTech.TempCentre.DAL;
using ShineTech.TempCentre.Platform;
using ShineTech.TempCentre.Versions;

namespace ShineTech.TempCentre.BusinessFacade
{
    public class ReportEditorExporter : ReportExporter, IReportExportService
    {
        private readonly static Font ReportTitleFont = new Font("Arial", 18, FontStyle.Bold, GraphicsUnit.Pixel);
        private readonly static Font ReportTitleAlarmFont = new Font("Arial", 26, FontStyle.Bold, GraphicsUnit.Pixel);
        private readonly static Font DefaultFont = new Font("Arial", 11, GraphicsUnit.Pixel);
        private readonly static Font HearderContentFont = new Font("Arial", 9, GraphicsUnit.Pixel);
        private readonly static Font SectionTitleFont = new Font("Arial", 13, FontStyle.Bold, GraphicsUnit.Pixel);

        private TextBox tbReportTitle;
        private TextBox tbReportComment;
        private Label lblReportEditorTip;

        public ReportEditorExporter(DeviceDataFrom deviceDataFrom, SuperDevice device, IList<DigitalSignature> signatureList, Panel documentPanel, TextBox tbReportTitle, TextBox tbReportComment, Label lblReportEditorTip)
            : base(deviceDataFrom, device, signatureList)
        {
            if (Common.IsAuthorized(RightsText.CommentRecords))
            {
                this.isCommentShown = true;
            }
            this.tbReportTitle = tbReportTitle;
            this.tbReportComment = tbReportComment;
            this.lblReportEditorTip = lblReportEditorTip;
            this.documentPanel = documentPanel;
            this.calculateSectionMargin();
            documentPanel.SizeChanged += new EventHandler(documentPanel_SizeChanged);
            documentPanel.Controls.Clear();
            this.AddNewPage();
            if (this.tbReportTitle != null)
            {
                this.tbReportTitle.TextChanged += new EventHandler(tbReportTitle_TextChanged);
            }
        }

        private string originalTitleString = string.Empty;
        private void tbReportTitle_TextChanged(object sender, EventArgs e)
        {
            var tb = sender as TextBox;
            if (tb != null)
            {
                string textTrimEnd = tb.Text.TrimEnd();
                using (Graphics g = tb.CreateGraphics())
                {
                    int actualWidth = (int)Math.Ceiling(g.MeasureString(textTrimEnd, tb.Font).Width);

                    if (actualWidth > tb.ClientSize.Width - 4)
                    {
                        tb.Text = this.originalTitleString;
                        tb.SelectionStart = tb.Text.Length;
                    }
                    else
                    {
                        this.originalTitleString = textTrimEnd;
                        //tb.SelectionStart = tb.Text.Length;
                    }
                }
            }
        }

        protected override void calculateSectionMargin()
        {
            int totalSectionCount = 7;
            int headerHeight = 50;
            int titleHeight = 25;
            int deviceInfoHeight = 100;
            int loggingSummaryHeight = 140;
            int alarmHeight = 0;
            int commentHeight = 70;
            int graphHeight = 450;
            if (this.device != null)
            {
                if (this.device.AlarmMode == 1)
                {
                    alarmHeight = 100;
                }
                else if (this.device.AlarmMode == 2)
                {
                    alarmHeight = 180;
                }
                int totalBlankHeight = PageHeight - PageTopPadding - FooterHeight - headerHeight - titleHeight - deviceInfoHeight - loggingSummaryHeight - alarmHeight - commentHeight - graphHeight;
                if (!this.isHeaderShown)
                {
                    totalSectionCount--;
                    totalBlankHeight += headerHeight;
                }
                if (!this.isAlarmShown)
                {
                    totalSectionCount--;
                    totalBlankHeight += alarmHeight;
                }
                if (!this.isCommentShown)
                {
                    totalSectionCount--;
                    totalBlankHeight += commentHeight;
                }
                //this.sectionMargin = totalBlankHeight / (totalSectionCount + 1);
                this.sectionMargin = 13;
            }
        }

        private void documentPanel_SizeChanged(object sender, EventArgs e)
        {
            this.documentPanel.SuspendLayout();
            if (documentPanel.Width > PageWidth)
            {
                panelLeft = (documentPanel.Width - PageWidth) / 2; 
            }
            else
            {
                panelLeft = 0;
            }
            foreach (Control control in documentPanel.Controls)
            {
                if (control.Tag != null && PagePanelTag.Equals(control.Tag.ToString(), StringComparison.Ordinal))
                {
                    control.Left = panelLeft;
                }
            }
            if (this.lblReportEditorTip != null)
            {
                this.lblReportEditorTip.Left = panelLeft;
            }
            this.documentPanel.ResumeLayout();
        }

        private Panel documentPanel;
        private IList<Panel> documentPages = new List<Panel>();
        private Panel currentPagePanel;
        private int currentSectionTop = 0;
        private int pageCount = 0;

        private const int ReportTitleWidth = 500;
        private const int ReportTitleHeight = 25;
        private const int PageWidth = 910;
        private const int PageHeight = 1280;
        private int sectionMargin = 10;
        private const int PageTopPadding = 50;
        private const int PageHorizonalPadding = 90;
        private const int FooterHeight = 75;
        private const int DefaultSectionWidth = PageWidth - PageHorizonalPadding * 2;
        private const string PagePanelTag = "Page_Panel_Tag";

        private float dataListContentCellHeight = 11.2f;
        private int rowsInfectedBySignature = 0;

        private readonly static float[] dataListTableColumnWidthsLayout = new float[] { 0.37f, 0.43f, 0.20f };

        private int panelLeft = 0;

        private void AddNewPage()
        {
            Panel newPagePanel = new Panel();
            newPagePanel.Tag = PagePanelTag;
            newPagePanel.Width = PageWidth;
            newPagePanel.Height = PageHeight;
            newPagePanel.BackColor = Color.White;
            if (this.documentPages.Count > 0)
            {
                newPagePanel.Top = this.documentPages.Count * (PageHeight + 20);
            }
            newPagePanel.Left = panelLeft;
            newPagePanel.BorderStyle = BorderStyle.FixedSingle;
            this.documentPages.Add(newPagePanel);
            this.documentPanel.Controls.Add(newPagePanel);
            this.currentPagePanel = newPagePanel;
            this.currentSectionTop = PageTopPadding;
            this.pageCount++;
        }

        private void AddSection(Control control)
        {
            this.AddSection(control, false);
        }

        private void AddSection(Control control, bool isFooter)
        {
            if (control.Top == 0)
            {
                control.Top = this.currentSectionTop;
                control.Top += sectionMargin;
            }
            if (control.Bottom >= PageHeight - FooterHeight && !isFooter)
            {
                this.GenerateReportFooter();
                this.AddNewPage();
                control.Top = PageTopPadding;
            }
            if (control.Left == 0)
            {
                control.Left += PageHorizonalPadding;
            }
            this.currentPagePanel.Controls.Add(control);
            this.currentSectionTop = control.Bottom;
        }

        protected override void GenerateReportHeader()
        {
            byte[] logoByte = Common.GlobalProfile.Logo;
            string contactInfo = Common.GlobalProfile.ContactInfo;
            
            if (logoByte != null || !string.IsNullOrWhiteSpace(contactInfo))
            {
                int headerHeight = 63;
                TableLayoutPanel headerTable = new TableLayoutPanel();
                headerTable.Top = PageTopPadding;
                float cellLogoWidth = 0.59f;
                float cellAddressWidth = 0.05f;
                float cellContactInfoWidth = 0.36f;
                headerTable.Width = DefaultSectionWidth;
                headerTable.Height = headerHeight;
                headerTable.ColumnCount = 3;
                headerTable.ColumnStyles.Clear();
                headerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, cellLogoWidth));
                headerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, cellAddressWidth));
                headerTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, cellContactInfoWidth));
                headerTable.RowCount = 1;
                headerTable.RowStyles.Clear();
                headerTable.RowStyles.Add(new RowStyle(SizeType.Absolute, headerHeight));
                if (logoByte != null)
                {
                    Image logo = new Bitmap(new MemoryStream(logoByte));
                    PictureBox pbLogo = new PictureBox();
                    pbLogo.Width = logo.Width;
                    pbLogo.Height = headerHeight;
                    pbLogo.SizeMode = PictureBoxSizeMode.Zoom;
                    try
                    {
                        int finalWidth = logo.Width;
                        int finalHeight = logo.Height;
                        Size originalSize = new Size(pbLogo.Width, pbLogo.Height);
                        if (finalWidth > originalSize.Width)
                        {
                            finalHeight = (int)(finalHeight * (originalSize.Width * 1.0 / finalWidth));
                            finalWidth = originalSize.Width;
                        }
                        if (finalHeight > originalSize.Height)
                        {
                            finalWidth = (int)(finalWidth * (originalSize.Height * 1.0 / finalHeight));
                            finalHeight = originalSize.Height;
                        }
                        pbLogo.Width = finalWidth;
                    }
                    catch (Exception)
                    {
                    }
                    
                    pbLogo.Image = logo;
                    pbLogo.Margin = new Padding(0);
                    headerTable.Controls.Add(pbLogo);
                    headerTable.SetCellPosition(pbLogo, new TableLayoutPanelCellPosition(0, 0));
                }
                if (!string.IsNullOrWhiteSpace(contactInfo))
                {
                    Label lblContactInfo = new Label();
                    lblContactInfo.Width = (int)(DefaultSectionWidth * cellContactInfoWidth);
                    lblContactInfo.Height = headerHeight;
                    lblContactInfo.BorderStyle = BorderStyle.FixedSingle;
                    //lblContactInfo.TextAlign = ContentAlignment.TopRight;
                    lblContactInfo.Font = HearderContentFont;
                    lblContactInfo.Text = contactInfo;
                    headerTable.Controls.Add(lblContactInfo);
                    headerTable.SetCellPosition(lblContactInfo, new TableLayoutPanelCellPosition(2, 0));
                }
                this.AddSection(headerTable);
                this.currentSectionTop += 10;
            }
        }

        protected override void GenerateReportTitle()
        {
            if (this.tbReportTitle != null)
            {
                this.tbReportTitle.Top = 0;
                this.tbReportTitle.Left = 0;
                int widthBetweenTitleAndAlarmCount = 115;
                
                this.tbReportTitle.Width = ReportTitleWidth;
                this.tbReportTitle.Height = ReportTitleHeight;
                this.tbReportTitle.Font = ReportTitleFont;
                if (string.IsNullOrWhiteSpace(this.tbReportTitle.Text))
                {
                    this.tbReportTitle.Text = ReportConstString.TitleDefaultString;
                }
                if (ReportConstString.TitleDefaultString.Equals(this.tbReportTitle.Text.TrimEnd()) && Common.GlobalProfile != null && !string.IsNullOrWhiteSpace(Common.GlobalProfile.ReportTitle))
                {
                    this.tbReportTitle.Text = Common.GlobalProfile.ReportTitle;
                }
                this.AddSection(this.tbReportTitle);
                if (this.reportTitleIconStatus == ReportTitleIconStatus.Alarm && reportCrossSmall != null)
                {
                    PictureBox pbAlarm = new PictureBox();
                    pbAlarm.Height = ReportTitleHeight;
                    pbAlarm.Width = ReportTitleHeight;
                    pbAlarm.Top = this.tbReportTitle.Top;
                    pbAlarm.Left = this.tbReportTitle.Right + widthBetweenTitleAndAlarmCount;
                    pbAlarm.SizeMode = PictureBoxSizeMode.StretchImage;
                    pbAlarm.Image = reportCrossSmall;
                    this.AddSection(pbAlarm);

                    Label lblAlarm = new Label();
                    lblAlarm.Text = ReportConstString.TitleAlarmString;
                    lblAlarm.Font = ReportTitleAlarmFont;
                    lblAlarm.Height = ReportTitleHeight;
                    lblAlarm.TextAlign = ContentAlignment.MiddleLeft;
                    lblAlarm.Top = this.tbReportTitle.Top;
                    lblAlarm.Left = pbAlarm.Right + 5;
                    this.AddSection(lblAlarm);
                }
                else if (this.reportTitleIconStatus == ReportTitleIconStatus.OK && reportOkSmall != null)
                {
                    PictureBox pbAlarm = new PictureBox();
                    pbAlarm.Height = ReportTitleHeight;
                    pbAlarm.Width = ReportTitleHeight;
                    pbAlarm.Top = this.tbReportTitle.Top;
                    pbAlarm.Left = this.tbReportTitle.Right + widthBetweenTitleAndAlarmCount + 30;
                    pbAlarm.SizeMode = PictureBoxSizeMode.StretchImage;
                    pbAlarm.Image = reportOkSmall;
                    this.AddSection(pbAlarm);

                    Label lblAlarm = new Label();
                    lblAlarm.Text = ReportConstString.TitleOkString;
                    lblAlarm.Font = ReportTitleAlarmFont;
                    lblAlarm.Height = ReportTitleHeight;
                    lblAlarm.TextAlign = ContentAlignment.MiddleLeft;
                    lblAlarm.Top = this.tbReportTitle.Top;
                    lblAlarm.Left = pbAlarm.Right + 5;
                    this.AddSection(lblAlarm);
                }
                else
                {
                    // nothing to do
                }
                this.currentSectionTop += 20;
            }
        }

        protected override void GenerateDeviceConfigurationAndTripInfomation()
        {
            int widthBetweenTwoSection = 20;
            SectionControl sectionDeviceConfig = new SectionControl();
            sectionDeviceConfig.Width = (PageWidth - PageHorizonalPadding * 2) / 2 - widthBetweenTwoSection / 2;
            sectionDeviceConfig.SetSectionTitle("Device Configuration");

            IDictionary<string, string[]> deviceConfigurationContents = this.reportdataGenerator.GetDeviceConfigurationTripInfoRowsContents(this.device);
            string[] row1Contents = deviceConfigurationContents["row1Contents"];
            string[] row2Contents = deviceConfigurationContents["row2Contents"];
            string[] row3Contents = deviceConfigurationContents["row3Contents"];
            string[] tripInfoContents = deviceConfigurationContents["tripInfoContents"];

            sectionDeviceConfig.AddRow(row1Contents);
            sectionDeviceConfig.AddRow(row2Contents);
            sectionDeviceConfig.AddRow(row3Contents);
            sectionDeviceConfig.InitializeLayout(new float[] { 0.42f, 0.58f });
            this.AddSection(sectionDeviceConfig);

            SectionControl sectionTripInfo = new SectionControl();
            sectionTripInfo.Top = sectionDeviceConfig.Top;
            sectionTripInfo.Width = sectionDeviceConfig.Width;
            sectionTripInfo.Left = sectionDeviceConfig.Right + widthBetweenTwoSection;
            sectionTripInfo.IsAllowNewLine = true;
            sectionTripInfo.IsTripInfoSection = true;
            sectionTripInfo.SetSectionTitle(string.Format("Trip Information_{0}", reportdataGenerator.GetLocalTimeZoneString()));
            sectionTripInfo.AddRow(new string[] { "Trip Number:", tripInfoContents[0] });
            sectionTripInfo.AddRow(new string[] { tripInfoContents[1].Trim(), tripInfoContents[2] });
            sectionTripInfo.AddRow(new string[] { string.Empty, string.Empty });
            sectionTripInfo.InitializeLayout(new float[] { 0.203f, 0.797f });

            this.AddSection(sectionTripInfo);
        }

        protected override void GenerateLoggingSummary()
        {
            SectionControl sectionLoggingSummary = new SectionControl();
            sectionLoggingSummary.Width = DefaultSectionWidth;
            sectionLoggingSummary.SetSectionTitle("Logging Summary");

            IDictionary<string, string[]> loggingSummaryContents = this.reportdataGenerator.GetLoggingSummaryColumsContents(this.device, this.deviceDataFrom);
            string[] column1Contents = loggingSummaryContents["column1Contents"];
            string[] column2Contents = loggingSummaryContents["column2Contents"];

            sectionLoggingSummary.AddRow(new string[] { column1Contents[0], column2Contents[0] });
            sectionLoggingSummary.AddRow(new string[] { column1Contents[1], column2Contents[1] });
            sectionLoggingSummary.AddRow(new string[] { column1Contents[2], column2Contents[2] });
            sectionLoggingSummary.AddRow(new string[] { column1Contents[3], column2Contents[3] });
            sectionLoggingSummary.AddRow(new string[] { column1Contents[4], column2Contents[4] });
            sectionLoggingSummary.InitializeLayout(new float[] { 0.5f, 0.5f });
            this.AddSection(sectionLoggingSummary);
        }

        protected override void GenerateAlarms()
        {
            SectionControl sectionAlarm = new SectionControl();
            sectionAlarm.HorizonalTextAlignment = ContentAlignment.MiddleCenter;
            sectionAlarm.Width = DefaultSectionWidth;
            sectionAlarm.SetSectionTitle(this.reportdataGenerator.GetAlarmSectionTitle(this.device));

            IDictionary<string, string[]> alarmDataContents = this.reportdataGenerator.GetAlarmRowContents(this.device);
            string[] highRowContents = alarmDataContents["highRowContents"];
            string[] lowRowContents = alarmDataContents["lowRowContents"];
            string[] a1RowContents = alarmDataContents["a1RowContents"];
            string[] a2RowContents = alarmDataContents["a2RowContents"];
            string[] a3RowContents = alarmDataContents["a3RowContents"];
            string[] a4RowContents = alarmDataContents["a4RowContents"];
            string[] a5RowContents = alarmDataContents["a5RowContents"];
            string[] a6RowContents = alarmDataContents["a6RowContents"];

            int totalRows = 6;
            if (this.device.AlarmMode == 0)
            {
                // nothing to do
            }
            else if (device.AlarmMode == 1)
            {
                sectionAlarm.SetHeader(new string[] { "Alarm Zones", "Alarm Delay", "Total Time", "Events", "First Triggered", "Alarm Status" });
                if (reportdataGenerator.IsStringArrayNotEmpty(highRowContents))
                {
                    sectionAlarm.AddRow(highRowContents);
                    totalRows--;
                }
                if (reportdataGenerator.IsStringArrayNotEmpty(lowRowContents))
                {
                    sectionAlarm.AddRow(lowRowContents);
                    totalRows--;
                }
            }
            else if (device.AlarmMode == 2)
            {
                sectionAlarm.SetHeader(new string[] { "Alarm Zones", "Alarm Delay", "Total Time", "Events", "First Triggered", "Alarm Status" });
                if (reportdataGenerator.IsStringArrayNotEmpty(a1RowContents))
                {
                    sectionAlarm.AddRow(a1RowContents);
                    totalRows--;
                }
                if (reportdataGenerator.IsStringArrayNotEmpty(a2RowContents))
                {
                    sectionAlarm.AddRow(a2RowContents);
                    totalRows--;
                }
                if (reportdataGenerator.IsStringArrayNotEmpty(a3RowContents))
                {
                    sectionAlarm.AddRow(a3RowContents);
                    totalRows--;
                }
                if (reportdataGenerator.IsStringArrayNotEmpty(a4RowContents))
                {
                    sectionAlarm.AddRow(a4RowContents);
                    totalRows--;
                }
                if (reportdataGenerator.IsStringArrayNotEmpty(a5RowContents))
                {
                    sectionAlarm.AddRow(a5RowContents);
                    totalRows--;
                }
                if (reportdataGenerator.IsStringArrayNotEmpty(a6RowContents))
                {
                    sectionAlarm.AddRow(a6RowContents);
                    totalRows--;
                }
            }
            for (int i = 0; i < totalRows; i++)
            {
                sectionAlarm.AddRow(new string[6]);
            }
            sectionAlarm.InitializeLayout(new float[] { 0.18f, 0.19f, 0.17f, 0.10f, 0.23f, 0.13f }, new ContentAlignment[] { ContentAlignment.MiddleLeft, ContentAlignment.MiddleLeft, ContentAlignment.MiddleLeft, ContentAlignment.MiddleLeft, ContentAlignment.MiddleLeft, ContentAlignment.MiddleRight });
            this.AddSection(sectionAlarm);
        }

        protected override void GenerateComments()
        {
            if (Common.IsAuthorized(RightsText.CommentRecords) && this.tbReportComment != null)
            {
                int heightBetweenTitleAndContent = 5;
                int tbCommentContentheight = 51;
                Label lblCommentTitle = new Label();
                lblCommentTitle.Font = SectionTitleFont;
                lblCommentTitle.Text = "Comments";
                this.AddSection(lblCommentTitle);

                this.tbReportComment.Multiline = true;
                this.tbReportComment.ScrollBars = ScrollBars.None;
                this.tbReportComment.Font = new Font("Arial", 9);
                this.tbReportComment.Width = DefaultSectionWidth;
                this.tbReportComment.Height = tbCommentContentheight;
                this.tbReportComment.Top = lblCommentTitle.Bottom + heightBetweenTitleAndContent;
                //this.tbReportComment.MaxLength = 200;
                this.AddSection(this.tbReportComment);
            }
        }

        protected override void GenerateDataGraph()
        {
            //PictureBox reportGraph = new PictureBox();
            //reportGraph.Width = DefaultSectionWidth;
            //reportGraph.Height = (int)(reportGraph.Width * 0.63);
            //reportGraph.Top = PageHeight - FooterHeight - reportGraph.Height - 15;
            //reportGraph.SizeMode = PictureBoxSizeMode.StretchImage;
            ////reportGraph.Image = new Bitmap(Path.Combine(Environment.CurrentDirectory.ToString(), "Images", "DataGraph.jpg"));
            //MemoryStream graphStream = null;
            //try
            //{
            //    if (this.device.ReportGraph != null)
            //    {
            //        graphStream = new MemoryStream(this.device.ReportGraph);
            //        reportGraph.Image = new Bitmap(graphStream);
            //    }
            //}
            //catch (Exception)
            //{
            //}
            //finally
            //{
            //    if (graphStream != null)
            //    {
            //        graphStream.Close();
            //    }
            //}
            //this.AddSection(reportGraph);
            CreateReportLine();
        }

        protected override void GenerateSignatures()
        {
            if (this.isSignatureShown)
            {
                float signatureRowHeight = 25f;
                

                this.GenerateReportFooter();
                this.AddNewPage();
                SectionControl sectionSignature = new SectionControl(false);
                sectionSignature.IsCellBorderShown = true;
                sectionSignature.IsBorderShown = false;
                sectionSignature.IsAllowNewLine = true;
                sectionSignature.Width = DefaultSectionWidth;
                sectionSignature.DefaultRowHeight = signatureRowHeight;
                sectionSignature.SetSectionTitle("Electronic Signatures");
                for (int i = 0; i < this.signatureList.Count; i++)
                {
                    sectionSignature.AddRow(new string[] { (i + 1).ToString(), this.signatureList[i].ToString(Common.GlobalProfile.DateTimeFormator) });
                }
                sectionSignature.InitializeLayout(new float[] { 0.05f, 0.95f });
                sectionSignature.PaintByUser();
                this.rowsInfectedBySignature = (int)Math.Ceiling(((sectionSignature.Height + sectionMargin * 3) / this.dataListContentCellHeight));
                this.AddSection(sectionSignature);
            }
        }

        private TableLayoutPanel CreateDataListTableLayoutPanel()
        {
            return CreateDataListTableLayoutPanel(false);
        }

        private TableLayoutPanel CreateDataListTableLayoutPanel(bool isFirstTable)
        {
            int dataCountOfOneCell = 100;
            if (isFirstTable)
            {
                dataCountOfOneCell = dataCountOfOneCell - this.rowsInfectedBySignature;
            }
            float headerHeight = 16;
            TableLayoutPanel result = new TableLayoutPanel();
            result.ColumnCount = 4;
            result.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
            for (int i = 0; i < result.ColumnCount; i++)
            {
                result.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 1f / ((float)result.ColumnCount)));
            }
            result.RowCount = 2;
            result.RowStyles.Add(new RowStyle(SizeType.Absolute, headerHeight));
            result.RowStyles.Add(new RowStyle(SizeType.Absolute, dataCountOfOneCell * dataListContentCellHeight));
            result.Width = DefaultSectionWidth;
            result.Height = (int)(headerHeight + dataCountOfOneCell * dataListContentCellHeight + 3);
            return result;
        }

        protected override void GenerateDataList()
        {
            IList<PointKeyValue> dataList = this.device.tempList;
            TableLayoutPanel dataListTable = null;
            int columnIndex = 0;
            int cellContentSectionWidth = (DefaultSectionWidth - 4) / 3;
            int cellDataCount = 100;
            int columnPerPage = 4;
            int dataPerPage = cellDataCount * columnPerPage;
            SectionControl cellContentSection = null;
            SectionControl tableHearderSection = null;

            dataListTable = this.CreateDataListTableLayoutPanel(true);
            dataListTable.Top = this.currentSectionTop + sectionMargin * 2;
            for (int i = 0; i < Math.Min((cellDataCount - this.rowsInfectedBySignature) * columnPerPage, this.device.tempList.Count); i++)
            {
                if (i % (cellDataCount - this.rowsInfectedBySignature) == 0)
                {
                    if (cellContentSection != null)
                    {
                        dataListTable.SetCellPosition(tableHearderSection, new TableLayoutPanelCellPosition(columnIndex, 0));
                        dataListTable.SetCellPosition(cellContentSection, new TableLayoutPanelCellPosition(columnIndex, 1));
                        tableHearderSection.InitializeLayout(dataListTableColumnWidthsLayout);
                        cellContentSection.InitializeLayout(dataListTableColumnWidthsLayout);
                    }
                    tableHearderSection = new SectionControl(false);
                    cellContentSection = new SectionControl(false);
                    cellContentSection.IsDataListContentSection = true;
                    dataListTable.Controls.Add(cellContentSection);
                    dataListTable.Controls.Add(tableHearderSection);
                    tableHearderSection.Width = cellContentSectionWidth;
                    cellContentSection.Width = cellContentSectionWidth;
                    cellContentSection.IsContentWithSmallFont = true;
                    tableHearderSection.IsContentWithSmallFont = true;
                    tableHearderSection.HorizonalTextAlignment = ContentAlignment.MiddleCenter;
                    cellContentSection.HorizonalTextAlignment = ContentAlignment.MiddleCenter;
                    cellContentSection.IsBorderShown = false;
                    tableHearderSection.IsBorderShown = false;
                    tableHearderSection.DefaultRowHeight = 16;
                    cellContentSection.DefaultRowHeight = 11.2f;
                    
                    tableHearderSection.SetHeader(new string[] { "Date", "Time", unit });
                    columnIndex = (i / (cellDataCount - this.rowsInfectedBySignature)) % columnPerPage;
                }
                cellContentSection.AddRow(new string[] { TempsenFormatHelper.GetFormattedDate(dataList[i].PointTime.ToLocalTime()),
                                                         TempsenFormatHelper.GetFormattedTime(dataList[i].PointTime.ToLocalTime()),
                                                         TempsenFormatHelper.GetFormattedTemperature(dataList[i].PointTemp),
                                                         dataList[i].IsMark.ToString()});

                if (i == Math.Min((cellDataCount - this.rowsInfectedBySignature) * columnPerPage, this.device.tempList.Count) - 1)
                {
                    dataListTable.SetCellPosition(tableHearderSection, new TableLayoutPanelCellPosition(columnIndex, 0));
                    dataListTable.SetCellPosition(cellContentSection, new TableLayoutPanelCellPosition(columnIndex, 1));
                    tableHearderSection.InitializeLayout(dataListTableColumnWidthsLayout);
                    cellContentSection.InitializeLayout(dataListTableColumnWidthsLayout);
                    this.AddSection(dataListTable);
                    this.GenerateReportFooter();
                    cellContentSection = null;
                    dataListTable = null;
                }
            }

            for (int i = (cellDataCount - this.rowsInfectedBySignature) * columnPerPage; i < device.tempList.Count; i++)
            {
                int j = i - ((cellDataCount - this.rowsInfectedBySignature) * columnPerPage);
                if (j % dataPerPage == 0)
                {
                    if (dataListTable != null)
                    {
                        this.AddSection(dataListTable);
                    }
                    dataListTable = this.CreateDataListTableLayoutPanel();
                }
                if (j % cellDataCount == 0)
                {
                    if (cellContentSection != null)
                    {
                        dataListTable.SetCellPosition(tableHearderSection, new TableLayoutPanelCellPosition(columnIndex, 0));
                        dataListTable.SetCellPosition(cellContentSection, new TableLayoutPanelCellPosition(columnIndex, 1));
                        tableHearderSection.InitializeLayout(dataListTableColumnWidthsLayout);
                        cellContentSection.InitializeLayout(dataListTableColumnWidthsLayout);
                    }
                    tableHearderSection = new SectionControl(false);
                    cellContentSection = new SectionControl(false);
                    cellContentSection.IsDataListContentSection = true;
                    dataListTable.Controls.Add(cellContentSection);
                    dataListTable.Controls.Add(tableHearderSection);
                    tableHearderSection.Width = cellContentSectionWidth;
                    cellContentSection.Width = cellContentSectionWidth;
                    cellContentSection.IsContentWithSmallFont = true;
                    tableHearderSection.IsContentWithSmallFont = true;
                    tableHearderSection.HorizonalTextAlignment = ContentAlignment.MiddleCenter;
                    cellContentSection.HorizonalTextAlignment = ContentAlignment.MiddleCenter;
                    cellContentSection.IsBorderShown = false;
                    tableHearderSection.IsBorderShown = false;
                    tableHearderSection.DefaultRowHeight = 16;
                    cellContentSection.DefaultRowHeight = 11.2f;
                    
                    tableHearderSection.SetHeader(new string[] { "Date", "Time", unit });
                    columnIndex = (j / cellDataCount) % 4;
                }
                cellContentSection.AddRow(new string[] { TempsenFormatHelper.GetFormattedDate(dataList[i].PointTime.ToLocalTime()),
                                                         TempsenFormatHelper.GetFormattedTime(dataList[i].PointTime.ToLocalTime()),
                                                         TempsenFormatHelper.GetFormattedTemperature(dataList[i].PointTemp),
                                                         dataList[i].IsMark.ToString()});

                if (i == device.tempList.Count - 1)
                {
                    dataListTable.SetCellPosition(tableHearderSection, new TableLayoutPanelCellPosition(columnIndex, 0));
                    dataListTable.SetCellPosition(cellContentSection, new TableLayoutPanelCellPosition(columnIndex, 1));
                    tableHearderSection.InitializeLayout(dataListTableColumnWidthsLayout);
                    cellContentSection.InitializeLayout(dataListTableColumnWidthsLayout);
                    this.AddSection(dataListTable);
                    this.GenerateReportFooter();
                }
            }
        }

        protected override void GenerateReportFooter()
        {
            int extraWidth = 30;
            Panel footDivisionLine = new Panel();
            footDivisionLine.Top = PageHeight - FooterHeight;
            footDivisionLine.Left = PageHorizonalPadding - extraWidth;
            footDivisionLine.Width = DefaultSectionWidth + extraWidth * 2;
            footDivisionLine.Height = 1;
            footDivisionLine.BorderStyle = BorderStyle.None;
            footDivisionLine.BackColor = Color.Silver;
            this.AddSection(footDivisionLine, true);

            SectionControl footerSection = new SectionControl(false);
            //string defaultFileName = string.Format("File name: {0}_{1}.pdf", this.device.SerialNumber, this.device.TripNumber);
            //string pageNumber = string.Format("{0} / ", this.pageCount.ToString());

            int footerRowHeight = 12;
            int heightBetweenLineAndContent = 10;

            footerSection.Width = DefaultSectionWidth;
            footerSection.Top = footDivisionLine.Bottom + heightBetweenLineAndContent;
            footerSection.IsBorderShown = false;
            footerSection.DefaultRowHeight = footerRowHeight;
            footerSection.IsFootSection = true;
            //footerSection.AddRow(new string[] { defaultFileName, ReportConstString.CreatedTime, ReportConstString.PoweredBy });
            //footerSection.AddRow(new string[] { pageNumber, string.Empty, ReportConstString.Site });
            //footerSection.InitializeLayout(new float[] { 0.35f, 0.25f, 0.40f });
            footerSections.Add(new FooterSection() { FooterSectionControl = footerSection, PageNumber = pageCount });
            this.AddSection(footerSection, true);
        }

        string IReportExportService.Title
        {
            get;
            set;
        }

        string IReportExportService.CurrentComment
        {
            get;
            set;
        }


        private IList<FooterSection> footerSections = new List<FooterSection>();


        public override bool GenerateReport()
        {
            bool result = true;
            if (SoftwareVersions.Pro == Common.Versions && Common.GlobalProfile != null && Common.GlobalProfile.IsShowHeader)
            {
                this.GenerateReportHeader();
            }
            this.GenerateReportTitle();
            this.GenerateDeviceConfigurationAndTripInfomation();
            this.GenerateLoggingSummary();
            this.GenerateAlarms();
            this.GenerateComments();
            this.GenerateDataGraph();
            if (SoftwareVersions.Pro == Common.Versions)
            {
                this.GenerateSignatures();
            }
            this.GenerateDataList();

            OnGenerateFinished();
            return result;
        }

        protected virtual void OnGenerateFinished()
        {
            // Set footer content
            string defaultFileName = string.Format("File name: {0}_{1}.pdf", this.device.SerialNumber, this.device.TripNumber);
            string pageNumber = string.Empty;
            foreach (var footerSection in footerSections)
            {
                pageNumber = string.Format("{0} / {1}", footerSection.PageNumber.ToString(), this.pageCount.ToString());
                footerSection.FooterSectionControl.AddRow(new string[] { defaultFileName, ReportConstString.CreatedTimeString, ReportConstString.PoweredBy });
                footerSection.FooterSectionControl.AddRow(new string[] { pageNumber, string.Empty, ReportConstString.Site });
                footerSection.FooterSectionControl.InitializeLayout(new float[] { 0.35f, 0.30f, 0.35f });
            }
        }
        private void CreateReportLine()
        {
            //panel
            Panel panel = new Panel();
            panel.Size = new System.Drawing.Size(DefaultSectionWidth, (int)(DefaultSectionWidth * 0.63));
            panel.Top = PageHeight - FooterHeight - panel.Height - 15;
            //check box
            CheckBox cbLimit = new CheckBox();
            CheckBox cbIdeal = new CheckBox();
            CheckBox cbMark = new CheckBox();
            cbLimit.Text = "Show Alarm Limit";
            cbIdeal.Text = "Show Ideal Range";
            cbMark.Text = "Show Mark";
            cbLimit.Size = new System.Drawing.Size(109, 18);
            cbIdeal.Size = new Size(114, 18);
            cbMark.Size = new Size(81, 18);
            cbLimit.Font = new System.Drawing.Font("Arial", 8F);
            cbIdeal.Font = new System.Drawing.Font("Arial", 8F);
            cbMark.Font = new System.Drawing.Font("Arial", 8F);
            cbLimit.Left = DefaultSectionWidth - 351;
            cbIdeal.Left = DefaultSectionWidth - 241;
            cbMark.Left = DefaultSectionWidth - 126;
            cbLimit.Checked = Common.GlobalProfile.IsShowAlarmLimit;
            cbIdeal.Checked = Common.GlobalProfile.IsFillIdealRange;
            cbMark.Checked = Common.GlobalProfile.IsShowMark;
            cbLimit.Checked = Common.GlobalProfile.IsShowAlarmLimit;
            cbIdeal.Checked = Common.GlobalProfile.IsFillIdealRange;
            cbMark.Checked = Common.GlobalProfile.IsShowMark;
            panel.Controls.Add(cbLimit);
            panel.Controls.Add(cbIdeal);
            panel.Controls.Add(cbMark);
            //zedgraph
            ZedGraph.ZedGraphControl reportZedGraph = new ZedGraph.ZedGraphControl();
            reportZedGraph.Size = new System.Drawing.Size(DefaultSectionWidth, (int)(DefaultSectionWidth* 0.63));
            reportZedGraph.Dock = DockStyle.Fill;
            reportZedGraph.GraphPane.CurveList.Clear();
            GraphHelper.SetGraphAsDefaultProperity(reportZedGraph, XAxisVisibility.DateAndTime);
            GraphHelper.SetMinMaxLimits(base.device);
            GraphHelper.SetGraphDataSource(reportZedGraph, base.device, XAxisVisibility.DateAndTime, cbMark.Checked);
            reportZedGraph.IsShowPointValues = false;
            reportZedGraph.IsEnableSelection = false;
            reportZedGraph.IsEnableZoom = false;
            reportZedGraph.Font = new System.Drawing.Font("Arial", 8F);
            panel.Controls.Add(reportZedGraph);

            cbLimit.CheckedChanged += new EventHandler((sender, e) =>
            {
                GraphHelper.DrawLimitLintAndFillIdeal(reportZedGraph.GraphPane, base.device, cbIdeal.Checked, cbLimit.Checked);
                base.device.ReportGraph = Platform.Utils.CopyToBinary(reportZedGraph.GraphPane.GetImage(true));
                reportZedGraph.Refresh();
            });
            cbIdeal.CheckedChanged += new EventHandler((sender, e) =>
            {
                GraphHelper.DrawLimitLintAndFillIdeal(reportZedGraph.GraphPane, base.device, cbIdeal.Checked, cbLimit.Checked);
                base.device.ReportGraph = Platform.Utils.CopyToBinary(reportZedGraph.GraphPane.GetImage(true));
                reportZedGraph.Refresh();
            });
            cbMark.CheckedChanged += new EventHandler((sender, e) =>
            {
                GraphHelper.ReDrawUnCompareCurveItem(reportZedGraph.GraphPane, base.device, XAxisVisibility.DateAndTime, cbMark.Checked);
                base.device.ReportGraph = Platform.Utils.CopyToBinary(reportZedGraph.GraphPane.GetImage(true));
                reportZedGraph.Refresh();
            });
            GraphHelper.ReDrawUnCompareCurveItem(reportZedGraph.GraphPane, base.device, XAxisVisibility.DateAndTime, cbMark.Checked);
            GraphHelper.DrawLimitLintAndFillIdeal(reportZedGraph.GraphPane, base.device, cbIdeal.Checked, cbLimit.Checked);
            reportZedGraph.Refresh();
            base.device.ReportGraph = Platform.Utils.CopyToBinary(reportZedGraph.GraphPane.GetImage(true));
            this.AddSection(panel);
        }
        public class FooterSection
        {
            public int PageNumber { get; set; }

            public SectionControl FooterSectionControl { get; set; }
        }
    }
}
