using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Office.Interop.Excel;

using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using ShineTech.TempCentre.Platform;
using ShineTech.TempCentre.DAL;
using ShineTech.TempCentre.Versions;
using Microsoft.Office.Core;

namespace ShineTech.TempCentre.BusinessFacade
{
    public class ExcelReportExporter : ReportExporter, IReportExportService
    {
        private Application excelApplication;
        private Workbook workBook;
        private Worksheet summary;
        private Worksheet dataGraph;
        private Worksheet dataList;
        private int currentRowIndexOfSummary;
        private string tempReportTitleIconPath = Path.Combine(System.Windows.Forms.Application.StartupPath, "temp", Guid.NewGuid().ToString());
        private float _officeVersion;
        private string tempHeaderFullPath;
        private List<double> _alarmLimits = new List<double>();

        public ExcelReportExporter(DeviceDataFrom deviceDataFrom, SuperDevice device, IList<DigitalSignature> signatureList, string fileNameWithFullPath)
            : base(deviceDataFrom, device, signatureList, fileNameWithFullPath)
        {
            
        }

        private Range getSectionRangeFromSummary(int supposedRowCountOfTheSection)
        {
            return getSectionRangeFromSummary(supposedRowCountOfTheSection, true);
        }

        private Range getSectionRangeFromSummary(int supposedRowCountOfTheSection, bool isWithPaddingTopEmptyRow)
        {
            return getSectionRangeFromSummary(supposedRowCountOfTheSection, isWithPaddingTopEmptyRow, false);
        }

        private Range getSectionRangeFromSummary(int supposedRowCountOfTheSection, bool isWithPaddingTopEmptyRow, bool isWrapText)
        {
            Range result = null;
            int startRowIndex = 0;
            if (isWithPaddingTopEmptyRow)
            {
                startRowIndex = this.currentRowIndexOfSummary + 1;
            }
            else
            {
                startRowIndex = this.currentRowIndexOfSummary;
            }
            int endRowIndex = startRowIndex + supposedRowCountOfTheSection - 1;
            result = this.summary.get_Range("A" + startRowIndex, "F" + endRowIndex);
            result.Cells.ColumnWidth = 14.2;
            result.Font.Name = "Arial";
            if (isWrapText)
            {
                result.Cells.WrapText = true;
            }
            this.currentRowIndexOfSummary = endRowIndex + 1;
            result.NumberFormat = "@";
            return result;
        }

        private void setSectionTitleStyleForRange(Range range, int rowIndex)
        {
            Range row = range.Rows[rowIndex];
            row.Font.Bold = true;
            row.Font.Size = 10;
        }

        private void setTableHeaderStyleForRange(Range range, int rowIndex)
        {
            Range row = range.Rows[rowIndex];
            row.Font.Bold = true;
            row.Font.Size = 8;
            row.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
        }

        private void setTableCellContentStyleForRange(Range range, int rowIndex)
        {
            setTableCellContentStyleForRange(range, rowIndex, false);
        }

        private void setTableCellContentStyleForRange(Range range, int rowIndex, bool isMarked)
        {
            Range row = range.Rows[rowIndex];
            row.Font.Size = 8;
            if (isMarked)
            {
                row.Font.Color = Color.Red;
                row.Font.Bold = true;
            }
            row.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
        }

        private void setTableCellContentStyleForRange(Range range, int[] rowIndexs)
        {
            foreach (var item in rowIndexs)
            {
                Range row = range.Rows[item];
                row.Font.Size = 8;
                row.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignLeft;
            }
        }

        private void setLongTextStyleForRange(Range range)
        {
            range.Font.Name = "Arial";
            range.Font.Size = 8;
        }

        private void setLongTextStyleForRange(Range range, int rowIndex)
        {
            Range row = range.Rows[rowIndex];
            row.Font.Size = 8;
        }

        protected override void GenerateReportHeader()
        {
            byte[] logoByte = Common.GlobalProfile.Logo;
            string contactInfo = Common.GlobalProfile.ContactInfo;
            if (logoByte != null || !string.IsNullOrWhiteSpace(contactInfo))
            {
                Range headerRange = this.getSectionRangeFromSummary(4);
                if (logoByte != null)
                {
                    this.tempHeaderFullPath = Path.Combine(System.Windows.Forms.Application.StartupPath, "temp", Guid.NewGuid().ToString());
                    Image tempImage = new Bitmap(new MemoryStream(logoByte));
                    //tempImage.Save(this.tempHeaderFullPath);
                    Utils.SaveTheFile(logoByte, this.tempHeaderFullPath);
                    int finalWidth = tempImage.Width;
                    int finalHeight = tempImage.Height;
                    Size originalSize = new Size((int)headerRange.Width / 2, (int)headerRange.Height);
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
                    this.summary.Shapes.AddPicture(this.tempHeaderFullPath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoTrue, headerRange.Left, headerRange.Top, finalWidth, finalHeight);
                }
                headerRange.get_Range("D1", "F4").Merge(headerRange.get_Range("D1", "F4").MergeCells);
                headerRange.get_Range("A1", "C4").Merge(headerRange.get_Range("A1", "C4").MergeCells);
                headerRange[1, 4] = contactInfo;
                headerRange[1, 4].WrapText = true;
                headerRange.Font.Size = 8;
            }
        }

        protected override void GenerateReportTitle()
        {
            Range titleRange = this.getSectionRangeFromSummary(1);
            titleRange.Cells[1, 1] = this.Title.Trim() == ReportConstString.TitleDefaultString ? "" : this.Title;
            if (this.reportTitleIconStatus == ReportTitleIconStatus.Alarm)
            {
                Utils.SaveTheFile(Utils.CopyToBinary(reportCrossSmall), this.tempReportTitleIconPath);
                this.summary.Shapes.AddPicture(this.tempReportTitleIconPath, MsoTriState.msoFalse, MsoTriState.msoTrue, titleRange.Cells[1, 6].Left + 1, titleRange.Cells[1, 6].Top + 1, titleRange.Cells.Height - 1, titleRange.Cells.Height - 1);
                titleRange.Cells[1, 6] = string.Format("     {0}", ReportConstString.TitleAlarmString);
            }
            else if (this.reportTitleIconStatus == ReportTitleIconStatus.OK)
            {
                Utils.SaveTheFile(Utils.CopyToBinary(reportOkSmall), this.tempReportTitleIconPath);
                this.summary.Shapes.AddPicture(this.tempReportTitleIconPath, MsoTriState.msoFalse, MsoTriState.msoTrue, titleRange.Cells[1, 6].Left + 1, titleRange.Cells[1, 6].Top + 1, titleRange.Cells.Height - 1, titleRange.Cells.Height - 1);
                titleRange.Cells[1, 6] = string.Format("     {0}", ReportConstString.TitleOkString);
            }
            else
            {
                //nothing to do
            }
            titleRange.Font.Size = 12;
            titleRange.Font.Bold = true;
        }

        private string GetKeyFromStringSplitByColon(string str)
        {
            return str.Substring(0, str.IndexOf(':') + 1);
        }

        private string GetValueFromStringSplitByColon(string str)
        {
            string result = string.Empty;
            if (str.LastIndexOf(':') + 2 < str.Length)
            {
                result = str.Substring(str.IndexOf(':') + 2);
            }
            return result;
        }

        protected override void GenerateDeviceConfigurationAndTripInfomation()
        {
            Range deviceConfigurationTitleRange = this.getSectionRangeFromSummary(1);
            deviceConfigurationTitleRange.Cells[1, 1] = "Device Configuration";
            this.setSectionTitleStyleForRange(deviceConfigurationTitleRange, 1);

            IDictionary<string, string[]> deviceConfigurationContents = this.reportdataGenerator.GetDeviceConfigurationTripInfoRowsContents(this.device);
            string[] row1Contents = deviceConfigurationContents["row1Contents"];
            string[] row2Contents = deviceConfigurationContents["row2Contents"];
            string[] row3Contents = deviceConfigurationContents["row3Contents"];
            string[] tripInfoContents = deviceConfigurationContents["tripInfoContents"];

            Range deviceConfigurationContentRange = this.getSectionRangeFromSummary(3, false);
            Range leftDeviceConfigurationContentRange1 = deviceConfigurationContentRange.Columns[1];
            this.SetEdgeBorderForRange(leftDeviceConfigurationContentRange1, true, false, true, true);
            Range leftDeviceConfigurationContentRange2 = deviceConfigurationContentRange.Columns[2];
            this.SetEdgeBorderForRange(leftDeviceConfigurationContentRange2, true, true, true, false);
            Range rightDeviceConfigurationContentRange1 = deviceConfigurationContentRange.Columns[4];
            this.SetEdgeBorderForRange(rightDeviceConfigurationContentRange1, true, false, true, true);
            Range rightDeviceConfigurationContentRange2 = deviceConfigurationContentRange.Columns[5];
            this.SetEdgeBorderForRange(rightDeviceConfigurationContentRange2, true, true, true, false);

            deviceConfigurationContentRange.Cells[1, 1] = this.GetKeyFromStringSplitByColon(row1Contents[0]);
            deviceConfigurationContentRange.Cells[1, 2] = this.GetValueFromStringSplitByColon(row1Contents[0]);

            deviceConfigurationContentRange.Cells[1, 4] = this.GetKeyFromStringSplitByColon(row1Contents[1]);
            deviceConfigurationContentRange.Cells[1, 5] = this.GetValueFromStringSplitByColon(row1Contents[1]);

            deviceConfigurationContentRange.Cells[2, 1] = this.GetKeyFromStringSplitByColon(row2Contents[0]);
            deviceConfigurationContentRange.Cells[2, 2] = this.GetValueFromStringSplitByColon(row2Contents[0]);

            deviceConfigurationContentRange.Cells[2, 4] = this.GetKeyFromStringSplitByColon(row2Contents[1]);
            deviceConfigurationContentRange.Cells[2, 5] = this.GetValueFromStringSplitByColon(row2Contents[1]);

            deviceConfigurationContentRange.Cells[3, 1] = this.GetKeyFromStringSplitByColon(row3Contents[0]);
            deviceConfigurationContentRange.Cells[3, 2] = this.GetValueFromStringSplitByColon(row3Contents[0]);

            deviceConfigurationContentRange.Cells[3, 4] = this.GetKeyFromStringSplitByColon(row3Contents[1]);
            deviceConfigurationContentRange.Cells[3, 5] = this.GetValueFromStringSplitByColon(row3Contents[1]);

            this.setTableCellContentStyleForRange(deviceConfigurationContentRange, new int[] { 1, 2, 3 });

            Range tripInformationTitleRange = this.getSectionRangeFromSummary(1);

            tripInformationTitleRange.Cells[1, 1] = "Trip Information";
            this.setSectionTitleStyleForRange(tripInformationTitleRange, 1);
            tripInformationTitleRange.Cells[1, 6] = reportdataGenerator.GetLocalTimeZoneString();
            tripInformationTitleRange.Cells[1, 6].Font.Size = 8;
            tripInformationTitleRange.Cells[1, 6].Font.Bold = false;
            tripInformationTitleRange.Cells[1, 6].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignRight;

            Range tripNumberRange = this.getSectionRangeFromSummary(1, false);
            tripNumberRange.Merge();
            tripNumberRange.Borders.Color = Color.Black;
            tripNumberRange.Cells[1, 1] = string.Format("Trip Number: {0}", tripInfoContents[0]);
            this.setTableCellContentStyleForRange(tripNumberRange, 1);

            Range tripDescriptionRange = this.getSectionRangeFromSummary(1, false);
            tripDescriptionRange.Merge();
            tripDescriptionRange.Borders.Color = Color.Black;
            tripDescriptionRange.Cells[1, 1] = string.Format("{0}{1}", tripInfoContents[1], tripInfoContents[2]);
            this.setTableCellContentStyleForRange(tripDescriptionRange, 1);
        }

        protected override void GenerateLoggingSummary()
        {
            Range range = this.getSectionRangeFromSummary(1);
            range.Cells[1, 1] = "Logging Summary";
            this.setSectionTitleStyleForRange(range, 1);

            IDictionary<string, string[]> loggingSummaryContents = this.reportdataGenerator.GetLoggingSummaryColumsContents(this.device, this.deviceDataFrom);
            string[] column1Contents = loggingSummaryContents["column1Contents"];
            string[] column2Contents = loggingSummaryContents["column2Contents"];

            Range contentRange = this.getSectionRangeFromSummary(4, false);
            Range leftRange = contentRange.Columns[3];
            leftRange.Borders.get_Item(XlBordersIndex.xlEdgeRight).Color = Color.Black;
            this.SetEdgeBorderForRange(contentRange);

            contentRange.Cells[1, 1] = this.GetKeyFromStringSplitByColon(column1Contents[0]);
            contentRange.Cells[1, 2] = this.GetValueFromStringSplitByColon(column1Contents[0]);

            contentRange.Cells[2, 1] = this.GetKeyFromStringSplitByColon(column1Contents[1]);
            contentRange.Cells[2, 2] = this.GetValueFromStringSplitByColon(column1Contents[1]);

            contentRange.Cells[3, 1] = this.GetKeyFromStringSplitByColon(column1Contents[2]);
            contentRange.Cells[3, 2] = this.GetValueFromStringSplitByColon(column1Contents[2]);

            contentRange.Cells[4, 1] = "MKT: ";
            contentRange.Cells[4, 2] = this.GetValueFromStringSplitByColon(column1Contents[3]);

            contentRange.Cells[5, 1] = this.GetKeyFromStringSplitByColon(column1Contents[4]);
            contentRange.Cells[5, 2] = this.GetValueFromStringSplitByColon(column1Contents[4]);

            contentRange.Cells[1, 4] = this.GetKeyFromStringSplitByColon(column2Contents[0]);
            contentRange.Cells[1, 5] = this.GetValueFromStringSplitByColon(column2Contents[0]);

            contentRange.Cells[2, 4] = this.GetKeyFromStringSplitByColon(column2Contents[1]);
            contentRange.Cells[2, 5] = this.GetValueFromStringSplitByColon(column2Contents[1]);

            contentRange.Cells[3, 4] = this.GetKeyFromStringSplitByColon(column2Contents[2]);
            contentRange.Cells[3, 5] = this.GetValueFromStringSplitByColon(column2Contents[2]);

            contentRange.Cells[4, 4] = this.GetKeyFromStringSplitByColon(column2Contents[3]);
            contentRange.Cells[4, 5] = this.GetValueFromStringSplitByColon(column2Contents[3]);

            this.setTableCellContentStyleForRange(contentRange, new int[] { 1, 2, 3, 4 });

            Range loggerReaderRange = this.getSectionRangeFromSummary(1, false);
            this.SetEdgeBorderForRange(loggerReaderRange);
            loggerReaderRange[1, 1] = this.GetKeyFromStringSplitByColon(column1Contents[4]);
            loggerReaderRange[1, 2] = this.GetValueFromStringSplitByColon(column1Contents[4]);
            this.setTableCellContentStyleForRange(loggerReaderRange, 1);
        }

        protected override void GenerateAlarms()
        {
            Range alarmTitleRange = this.getSectionRangeFromSummary(1);
            alarmTitleRange.Cells[1, 1] = this.reportdataGenerator.GetAlarmSectionTitle(this.device);
            this.setSectionTitleStyleForRange(alarmTitleRange, 1);

            IDictionary<string, string[]> alarmDataContents = this.reportdataGenerator.GetAlarmRowContents(this.device);
            string[] highRowContents = alarmDataContents["highRowContents"];
            string[] lowRowContents = alarmDataContents["lowRowContents"];
            string[] a1RowContents = alarmDataContents["a1RowContents"];
            string[] a2RowContents = alarmDataContents["a2RowContents"];
            string[] a3RowContents = alarmDataContents["a3RowContents"];
            string[] a4RowContents = alarmDataContents["a4RowContents"];
            string[] a5RowContents = alarmDataContents["a5RowContents"];
            string[] a6RowContents = alarmDataContents["a6RowContents"];
            if (this.device.AlarmMode == 0)
            {
                Range alarmRange = this.getSectionRangeFromSummary(3, false, true);
                alarmRange.Merge();
                alarmRange.Borders.Color = Color.Black;
            }
            else if (this.device.AlarmMode == 1)
            {
                Range alarmRange = this.getSectionRangeFromSummary(3, false, true);
                alarmRange.Borders.Color = Color.Black;
                alarmRange.Cells[1, 1] = "Alarm Zones";
                alarmRange.Cells[1, 2] = "Alarm Delay";
                alarmRange.Cells[1, 3] = "Total Time";
                alarmRange.Cells[1, 4] = "Events";
                alarmRange.Cells[1, 5] = "First Triggered";
                alarmRange.Cells[1, 6] = "Alarm Status";
                this.setTableHeaderStyleForRange(alarmRange, 1);

                this.setCellValueForRange(alarmRange, 2, highRowContents);

                this.setCellValueForRange(alarmRange, 3, lowRowContents);
                this.setTableCellContentStyleForRange(alarmRange, new int[] { 2, 3 });
            }
            else if (this.device.AlarmMode == 2)
            {
                Range alarmRange = this.getSectionRangeFromSummary(7 ,false, true);
                alarmRange.Borders.Color = Color.Black;
                alarmRange.Cells[1, 1] = "Alarm Zones";
                alarmRange.Cells[1, 2] = "Alarm Delay";
                alarmRange.Cells[1, 3] = "Total Time";
                alarmRange.Cells[1, 4] = "Events";
                alarmRange.Cells[1, 5] = "First Triggered";
                alarmRange.Cells[1, 6] = "Alarm Status";
                this.setTableHeaderStyleForRange(alarmRange, 1);

                this.setCellValueForRange(alarmRange, 2, a1RowContents);

                this.setCellValueForRange(alarmRange, 3, a2RowContents);

                this.setCellValueForRange(alarmRange, 4, a3RowContents);

                this.setCellValueForRange(alarmRange, 5, a4RowContents);

                this.setCellValueForRange(alarmRange, 6, a5RowContents);

                this.setCellValueForRange(alarmRange, 7, a6RowContents);

                this.setTableCellContentStyleForRange(alarmRange, new int[] { 2, 3, 4, 5, 6, 7 });
            }
        }

        private void setCellValueForRange(Range range, int rowIndex, string[] values)
        {
            if (values != null)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    range[rowIndex, i + 1] = values[i];
                }
                this.setTableCellContentStyleForRange(range, rowIndex);
            }
        }

        private void SetEdgeBorderForRange(Range range)
        {
            SetEdgeBorderForRange(range, true, true, true, true);
        }

        private void SetEdgeBorderForRange(Range range, bool isTopBorderShown, bool isRightBorderShown, bool isBottomBorderShown, bool isLeftBorderShown)
        {
            if (range != null)
            {
                if (isTopBorderShown)
                {
                    range.Borders.get_Item(XlBordersIndex.xlEdgeTop).Color = Color.Black;
                }
                if (isRightBorderShown)
                {
                    range.Borders.get_Item(XlBordersIndex.xlEdgeRight).Color = Color.Black;
                }
                if (isBottomBorderShown)
                {
                    range.Borders.get_Item(XlBordersIndex.xlEdgeBottom).Color = Color.Black;
                }
                if (isLeftBorderShown)
                {
                    range.Borders.get_Item(XlBordersIndex.xlEdgeLeft).Color = Color.Black;
                }
            }
        }

        protected override void GenerateComments()
        {
            string finalCommentString = string.Empty;
            if (this.CurrentComment != ReportConstString.CommentDefaultString)
            {
                finalCommentString = this.CurrentComment;
            }

            Range range = this.getSectionRangeFromSummary(1);
            range[1, 1] = "Comments";
            this.setSectionTitleStyleForRange(range, 1);

            Range commentContentRange = this.getSectionRangeFromSummary(2, false);
            commentContentRange.Merge();
            commentContentRange[1, 1].VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignTop;
            commentContentRange[1, 1] = finalCommentString;
            commentContentRange[1, 1].WrapText = true;
            commentContentRange.Cells.RowHeight = 17;
            commentContentRange.Borders.Color = Color.Black;
            this.setTableCellContentStyleForRange(commentContentRange, 1);
            
        }

        protected override void GenerateDataGraph()
        {
            string yAxisName = string.Format("{0}", unit);
            string xAxisName = "Data Points";
            ChartObjects chartObjects = this.dataGraph.ChartObjects(Type.Missing);
            ChartObject chartObject = chartObjects.Add(0, 0, 850, 500);
            Chart chart = chartObject.Chart;
            Range dataRange = null;
            string dataStartColumnIndex = "B";

            if (_officeVersion > 11.0f)
            {
                yAxisName = string.Format("Temperature({0})", unit);
                xAxisName = "Date Time";
                dataStartColumnIndex = "A";
                int xAxisPointCount = 10;
                int tickSpacing = this.device.tempList.Count / xAxisPointCount;
                if (tickSpacing == 0)
                {
                    tickSpacing = 1;
                }
                if (tickSpacing == 1 && this.device.tempList.Count != xAxisPointCount)
                {
                    tickSpacing = 2;
                }
                try
                {
                    chart.PlotArea.Width = chart.PlotArea.Width - 10;
                    chart.PlotArea.Height = chart.PlotArea.Height - 30;
                    chart.PlotArea.Top = 30;
                    Microsoft.Office.Interop.Excel.Axis xAxis = (Microsoft.Office.Interop.Excel.Axis)chart.Axes(Microsoft.Office.Interop.Excel.XlAxisType.xlCategory, Microsoft.Office.Interop.Excel.XlAxisGroup.xlPrimary);
                    xAxis.TickMarkSpacing = tickSpacing;
                    xAxis.TickLabelSpacing = tickSpacing;
                    xAxis.TickLabelPosition = Microsoft.Office.Interop.Excel.XlTickLabelPosition.xlTickLabelPositionLow;
                    xAxis.MajorTickMark = Microsoft.Office.Interop.Excel.XlTickMark.xlTickMarkNone;
                }
                catch (Exception)
                {
                }
            }
            if (_alarmLimits.Count > 0)
            {
                dataRange = this.dataList.get_Range(dataStartColumnIndex + "3", string.Format("{0}{1}", ConvertAlarmLimitsCountToColumnIndex(_alarmLimits.Count), this.device.tempList.Count + 2));
            }
            else
            {
                dataRange = this.dataList.get_Range(dataStartColumnIndex + "3", string.Format("B{0}", this.device.tempList.Count + 2));
            }
            chart.ChartWizard(dataRange, Microsoft.Office.Interop.Excel.XlChartType.xlLine, Type.Missing, Type.Missing, Type.Missing, Type.Missing, false, "Data Graph", xAxisName, yAxisName, Type.Missing);
            for (int i = 0; i < _alarmLimits.Count + 1; i++)
            {
                Series series = (Series)chart.SeriesCollection(i + 1);
                series.MarkerStyle = Microsoft.Office.Interop.Excel.XlMarkerStyle.xlMarkerStyleNone;
                series.Smooth = true;
                if (i > 0)
                {
                    series.Border.Color = Color.Red;
                }
            }

        }

        protected override void GenerateSignatures()
        {
            if (this.isSignatureShown)
            {
                Range signatureTitleRange = this.getSectionRangeFromSummary(1);
                signatureTitleRange[1, 1] = "Electronic Signatures";
                this.setSectionTitleStyleForRange(signatureTitleRange, 1);

                Range signatureHeaderRange = this.getSectionRangeFromSummary(1, false);
                signatureHeaderRange.Borders.Color = Color.Black;
                signatureHeaderRange[1, 1] = "Index";
                signatureHeaderRange[1, 2] = "User Name";
                signatureHeaderRange[1, 3] = "Full Name";
                signatureHeaderRange[1, 4] = "Meaning";
                signatureHeaderRange[1, 5] = "Date";
                signatureHeaderRange[1, 6] = "Time";
                this.setTableHeaderStyleForRange(signatureHeaderRange, 1);

                Range range = this.getSectionRangeFromSummary(this.signatureList.Count, false, true);
                range.Borders.Color = Color.Black;
                for (int i = 0; i < this.signatureList.Count; i++)
                {
                    string index = (i + 1).ToString();
                    string userName = this.signatureList[i].UserName;
                    string fullName = this.signatureList[i].FullName;
                    string meaning = this.signatureList[i].MeaningDesc;
                    string date = TempsenFormatHelper.GetFormattedDate(this.signatureList[i].SignTime.ToLocalTime());
                    string time = TempsenFormatHelper.GetFormattedTime(this.signatureList[i].SignTime.ToLocalTime());

                    range[i + 1, 1] = index;
                    range[i + 1, 2] = userName;
                    range[i + 1, 3] = fullName;
                    range[i + 1, 4] = meaning;
                    range[i + 1, 5] = date;
                    range[i + 1, 6] = time;
                    this.setTableCellContentStyleForRange(range, i + 1);
                }
            }
            Range testRange = this.getSectionRangeFromSummary(3);
        }

        protected override void GenerateDataList()
        {
            int recordsCount = this.device.tempList.Count;
            int totalRowCount = recordsCount + 2;
            Range dataListRange = this.dataList.get_Range("A1", "B" + totalRowCount);
            dataListRange.Font.Name = "Arial";
            dataListRange.Columns[1].ColumnWidth = 18;
            dataListRange.Columns[2].ColumnWidth = 10;

            dataListRange.Cells[1, 1] = " Data List";
            this.setSectionTitleStyleForRange(dataListRange, 1);

            dataListRange.Cells[2, 1] = " Date Time";
            dataListRange.Cells[2, 2] = unit;
            this.setTableHeaderStyleForRange(dataListRange, 2);
            dataListRange.get_Range("A2", "B2").Borders.Color = Color.Black;
            this.SetEdgeBorderForRange(dataListRange.get_Range("A3", string.Format("B{0}", totalRowCount)));

            ProcessAlarmLimits(totalRowCount);

            for (int i = 0; i < this.device.tempList.Count; i++)
            {
                string dateTime = TempsenFormatHelper.GetFormattedDateTime(this.device.tempList[i].PointTime.ToLocalTime());
                dataListRange.Cells[i + 3, 1] = " " + dateTime;
                dataListRange.Cells[i + 3, 2] = TempsenFormatHelper.GetFormattedTemperature(this.device.tempList[i].PointTemp);
                this.setTableCellContentStyleForRange(dataListRange, i + 3, this.device.tempList[i].IsMark);
            }
        }

        private void ProcessAlarmLimits(int totalRowCount)
        {
            //获得报警线设置信息
            string[] rawAlarmLimits = new string[]
            {
                this.device.AlarmHighLimit,
                this.device.AlarmLowLimit,
                this.device.A1,
                this.device.A2,
                this.device.A3,
                this.device.A4,
                this.device.A5
            };
            foreach (var item in rawAlarmLimits)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    double tmp = 0;
                    if (!double.TryParse(item, out tmp))
                    {
                        tmp = double.MinValue;
                    }
                    if (tmp != double.MinValue)
                    {
                        _alarmLimits.Add(tmp);
                    }
                }
            }
            if (_alarmLimits.Count > 0)
            {
                Range alarmLimitRange = this.dataList.get_Range("C3", ConvertAlarmLimitsCountToColumnIndex(_alarmLimits.Count) + totalRowCount);
                alarmLimitRange.Font.Color = Color.White;
                for (int i = 0; i < _alarmLimits.Count; i++)
                {
                    alarmLimitRange.Columns[i + 1] = _alarmLimits[i];
                }
            }
        }

        private string ConvertAlarmLimitsCountToColumnIndex(int count)
        {
            string result = string.Empty;
            char initColumIndex = 'C';
            result = ((char) (initColumIndex + (count - 1))).ToString();
            return result;
        }

        protected override void GenerateReportFooter()
        {
        }

        public override bool GenerateReport()
        {
            bool result = true;
            try
            {
                this.excelApplication = new Application();
                float.TryParse(this.excelApplication.Version, out _officeVersion);
                if (_officeVersion <= 11.0f && !string.IsNullOrWhiteSpace(this.fileNameWithFullPath))
                {
                    string fileNameWithoutExtension = this.fileNameWithFullPath.Substring(0, this.fileNameWithFullPath.LastIndexOf("."));
                    this.fileNameWithFullPath = string.Format("{0}.xls", fileNameWithoutExtension);
                }
                this.excelApplication.AlertBeforeOverwriting = false;
                this.excelApplication.DisplayAlerts = false;
                this.excelApplication.Visible = false;
                this.workBook = excelApplication.Workbooks.Add();
                this.summary = workBook.Worksheets[1];
                this.dataGraph = workBook.Worksheets[2];
                this.dataList = workBook.Worksheets[3];
                this.summary.Name = "Summary";
                this.dataGraph.Name = "Data Graph";
                this.dataList.Name = "Data List";

                try
                {
                    this.summary.PageSetup.TopMargin = 43;
                    this.summary.PageSetup.BottomMargin = 43;
                    this.summary.PageSetup.LeftMargin = 28;
                    this.summary.PageSetup.RightMargin = 28;
                }
                catch (Exception)
                {
                }

                this.workBook.ConflictResolution = XlSaveConflictResolution.xlLocalSessionChanges;
                if (SoftwareVersions.Pro == Common.Versions && Common.GlobalProfile != null && Common.GlobalProfile.IsShowHeader)
                {
                    this.GenerateReportHeader();
                }
                this.GenerateReportTitle();
                this.GenerateDeviceConfigurationAndTripInfomation();
                this.GenerateLoggingSummary();
                this.GenerateAlarms();
                this.GenerateComments();
                this.GenerateDataList();
                this.GenerateDataGraph();
                if (SoftwareVersions.Pro == Common.Versions)
                {
                    this.GenerateSignatures();
                }
                this.summary.Protect(this.summary.get_Range("A1"));
                this.dataGraph.Protect(this.dataGraph.get_Range("A1"));
                this.dataList.Protect(this.dataList.get_Range("A1"));
                this.workBook.SaveAs(this.fileNameWithFullPath, Type.Missing, Type.Missing, Type.Missing, true, Type.Missing, XlSaveAsAccessMode.xlShared);
            }
            catch (IOException ioe)
            {
                result = false;
                throw ioe;
            }
            catch (Exception e)
            {
                result = false;
                throw e;
            }
            finally
            {
                if (this.excelApplication != null)
                {
                    this.excelApplication.Quit();
                    if (this.summary != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(this.summary);
                        this.summary = null;
                    }
                    if (this.dataGraph != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(this.dataGraph);
                        this.dataGraph = null;
                    }
                    if (this.dataList != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(this.dataList);
                        this.dataList = null;
                    }
                    if (this.workBook != null)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(this.workBook);
                        this.workBook = null;
                    }
                    GC.Collect(0);
                    IntPtr handler = new IntPtr(this.excelApplication.Hwnd);
                    int processId = 0;
                    GetWindowThreadProcessId(handler, out processId);
                    System.Diagnostics.Process.GetProcessById(processId).Kill();
                }
                if (!string.IsNullOrEmpty(this.tempHeaderFullPath))
                {
                    FileInfo file = new FileInfo(this.tempHeaderFullPath);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
                if (!string.IsNullOrEmpty(this.tempReportTitleIconPath))
                {
                    FileInfo file = new FileInfo(this.tempReportTitleIconPath);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
            }
            return result;
        }

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);

        
    }
}
