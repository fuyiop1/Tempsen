using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using iTextSharp.text;
using iTextSharp.text.pdf;
using ShineTech.TempCentre.Platform;
using System.Drawing;
using System.Windows.Forms;
using ShineTech.TempCentre.DAL;
using ShineTech.TempCentre.Versions;
namespace ShineTech.TempCentre.BusinessFacade
{
    public class PDFReportExporter : ReportExporter, IReportExportService
    {
        

        private Document document;
        private PdfWriter writer;

        private float dataListContentCellHeight = 7.8f;
        private int rowsInfectedBySignature = 0;
        private float sectionMargin;


        public PDFReportExporter(DeviceDataFrom deviceDataFrom, SuperDevice device, IList<DigitalSignature> signatureList, string fileNameWithFullPath)
            : base(deviceDataFrom, device, signatureList, fileNameWithFullPath)
        {
            this.calculateSectionMargin();
        }

        public PDFReportExporter(DeviceDataFrom deviceDataFrom, SuperDevice device, IList<DigitalSignature> signatureList, string fileNameWithFullPath, bool isTempFile)
            : base(deviceDataFrom, device, signatureList, fileNameWithFullPath, isTempFile)
        {
            this.calculateSectionMargin();
        }

        private iTextSharp.text.Image getImage(byte[] bytes)
        {
            iTextSharp.text.Image result = null;
            if (bytes != null)
            {
                result = iTextSharp.text.Image.GetInstance(bytes);
            }
            return result;
        }

        protected override void GenerateReportHeader()
        {
            byte[] logoByte = Common.GlobalProfile.Logo;
            string contactInfo = Common.GlobalProfile.ContactInfo;

            if (logoByte != null || !string.IsNullOrWhiteSpace(contactInfo))
            {
                PdfPTable headerTable = PdfElementGenerator.createTable(new float[] { 0.57f, 0.05f, 0.38f });
                PdfPCell[] headRowCells = headerTable.Rows[0].GetCells();
                if (logoByte != null)
                {
                    headRowCells[0].Image = this.getImage(logoByte);
                }
                if (!string.IsNullOrWhiteSpace(contactInfo))
                {
                    headRowCells[2].Phrase = PdfElementGenerator.createPhrase(contactInfo, 6.1f, false);
                    headRowCells[2].Border = 15;
                    //headRowCells[2].HorizontalAlignment = Element.ALIGN_RIGHT;
                }
                foreach (var item in headRowCells)
                {
                    item.FixedHeight = 35f;
                    if (item.Phrase != null)
                    {
                        item.VerticalAlignment = Element.ALIGN_MIDDLE;
                    }
                }

                document.Add(headerTable);
                PdfElementGenerator.AddEmptyParagraphToDocument(this.document, this.sectionMargin);
            }
        }

        protected override void calculateSectionMargin()
        {
            int totalSectionCount = 7;
            int headerHeight = 33;
            int titleHeight = 16;
            int deviceInfoHeight = 65;
            int loggingSummaryHeight = 92;
            int alarmHeight = 0;
            int commentHeight = 46;
            int graphHeight = 300;
            if (this.device != null)
            {
                if (this.device.AlarmMode == 1)
                {
                    alarmHeight = 65;
                }
                else if (this.device.AlarmMode == 2)
                {
                    alarmHeight = 118;
                }
                int totalBlankHeight = 842 - 33 - 50 - headerHeight - titleHeight - deviceInfoHeight - loggingSummaryHeight - alarmHeight - commentHeight - graphHeight;
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
                //this.sectionMargin = totalBlankHeight / (totalSectionCount + 1) - 5;
                this.sectionMargin = 10f;
            }
        }

        protected override void GenerateReportTitle()
        {
            PdfPTable titleTable = null;
            if (this.reportTitleIconStatus == ReportTitleIconStatus.Alarm)
            {
                titleTable = PdfElementGenerator.createTable(new float[] { 0.84f, 0.04f, 0.12f });
            }
            else if (this.reportTitleIconStatus == ReportTitleIconStatus.OK)
            {
                titleTable = PdfElementGenerator.createTable(new float[] { 0.89f, 0.04f, 0.07f });
            }
            else
            {
                titleTable = PdfElementGenerator.createTable(new float[] { 0.89f, 0.04f, 0.07f });
            }
            PdfPCell[] rowCells = titleTable.Rows[0].GetCells();
            if (!string.IsNullOrWhiteSpace(this.Title))
            {
                rowCells[0].Phrase = PdfElementGenerator.createDocumentTitlePhrase(this.Title.Trim() == ReportConstString.TitleDefaultString ? "" : this.Title);
            }
            if (this.reportTitleIconStatus == ReportTitleIconStatus.Alarm && reportCrossSmall != null)
            {
                rowCells[1].Image = this.getImage(Utils.CopyToBinary(reportCrossSmall));
                rowCells[1].FixedHeight = 18;
                rowCells[2].Phrase = PdfElementGenerator.createDocumentTitlePhrase(ReportConstString.TitleAlarmString);
            }
            else if (this.reportTitleIconStatus == ReportTitleIconStatus.OK && reportOkSmall != null)
            {
                rowCells[1].Image = this.getImage(Utils.CopyToBinary(reportOkSmall));
                rowCells[1].FixedHeight = 18;
                rowCells[2].Phrase = PdfElementGenerator.createDocumentTitlePhrase(ReportConstString.TitleOkString);
            }
            else
            {
                // nothing to do
            }
            
            rowCells[2].PaddingBottom = 0;
            rowCells[2].VerticalAlignment = Element.ALIGN_MIDDLE;
            document.Add(titleTable);
            PdfElementGenerator.AddEmptyParagraphToDocument(this.document, this.sectionMargin);
        }

        protected override void GenerateDeviceConfigurationAndTripInfomation()
        {
            PdfPTable generalTable = PdfElementGenerator.createTable(new float[] { 0.49f, 0.02f, 0.49f });
            PdfPCell[] firstRowCells = generalTable.Rows[0].GetCells();

            // 生成两个版块的标题
            firstRowCells[0].Phrase = PdfElementGenerator.createSectionTitlePhrase("Device Configuration");

            PdfPTable tripInfoTitleTable = PdfElementGenerator.createTable(new float[2] { 0.4f, 0.6f }, false, false);
            PdfPCell[] tripInfoTitleRowCells = PdfElementGenerator.AddEmptyRowToTable(tripInfoTitleTable, false, false);
            tripInfoTitleRowCells[0].Phrase = PdfElementGenerator.createSectionTitlePhrase("Trip Information");
            tripInfoTitleRowCells[1].Phrase = PdfElementGenerator.createTableCellContentPhrase(reportdataGenerator.GetLocalTimeZoneString());
            tripInfoTitleRowCells[1].HorizontalAlignment = Element.ALIGN_RIGHT;

            firstRowCells[2] = new PdfPCell(tripInfoTitleTable);
            firstRowCells[2].Border = 0;

            firstRowCells[0].PaddingBottom = 4f;
            firstRowCells[2].PaddingBottom = 4f;
            //PdfElementGenerator.AddEmptyRowToTable(generalTable);
            
            // 填充Device Configuration的内容
            IDictionary<string, string[]> deviceConfigurationContents = this.reportdataGenerator.GetDeviceConfigurationTripInfoRowsContents(this.device);
            string[] row1Contents = deviceConfigurationContents["row1Contents"];
            string[] row2Contents = deviceConfigurationContents["row2Contents"];
            string[] row3Contents = deviceConfigurationContents["row3Contents"];
            string[] tripInfoContents = deviceConfigurationContents["tripInfoContents"];
            PdfPTable deviceConfigTable = PdfElementGenerator.createTable(new float[] { 0.42f, 0.58f });
            PdfPCell[] deviceRowCells = deviceConfigTable.Rows[0].GetCells();
            deviceRowCells[0].Phrase = PdfElementGenerator.createTableCellContentPhrase(row1Contents[0] + "\n\r" +
                                                                        row2Contents[0] + "\n\r" +
                                                                        row3Contents[0]);
            deviceRowCells[1].Phrase = PdfElementGenerator.createTableCellContentPhrase(row1Contents[1] + "\n\r" +
                                                                        row2Contents[1] + "\n\r" +
                                                                        row3Contents[1]);

            // 填充Trip Information的内容
            PdfPTable tripInfoTable = PdfElementGenerator.createTable(new float[] { 0.185f, 0.815f });
            IList<PdfPCell> tripInfoCells = tripInfoTable.Rows[0].GetCells();
            tripInfoCells[0].Colspan = 2;
            tripInfoCells[0].Phrase = PdfElementGenerator.createTableCellContentPhrase("Trip Number: " + tripInfoContents[0]);
            PdfPCell[] descriptionRowCells = PdfElementGenerator.AddEmptyRowToTable(tripInfoTable, false, true, false);
            descriptionRowCells[0].Phrase = PdfElementGenerator.createTableCellContentPhrase(tripInfoContents[1].Trim());
            descriptionRowCells[1].Phrase = PdfElementGenerator.createTableCellContentPhrase(tripInfoContents[2]);

            descriptionRowCells[0].PaddingTop = 6f;
            descriptionRowCells[1].PaddingTop = 6f;

            PdfPCell[] sencondRowCells = PdfElementGenerator.AddEmptyRowToTable(generalTable, false);
            sencondRowCells[0] = new PdfPCell(PdfElementGenerator.createSectionFrameTable(deviceConfigTable, false, false));
            sencondRowCells[2] = new PdfPCell(PdfElementGenerator.createSectionFrameTable(tripInfoTable, false, false));

            document.Add(generalTable);
            PdfElementGenerator.AddEmptyParagraphToDocument(this.document, this.sectionMargin);

        }

        protected override void GenerateLoggingSummary()
        {
            this.document.Add(PdfElementGenerator.createSectionTitleParagraph("Logging Summary"));

            IDictionary<string, string[]> loggingSummaryContents = this.reportdataGenerator.GetLoggingSummaryColumsContents(this.device, this.deviceDataFrom);
            string[] column1Contents = loggingSummaryContents["column1Contents"];
            string[] column2Contents = loggingSummaryContents["column2Contents"];

            PdfPTable loggingSummaryTable = PdfElementGenerator.createTable(new float[2] { 0.5f, 0.5f });
            PdfPCell[] firstRowCells = loggingSummaryTable.Rows[0].GetCells();
            firstRowCells[0].Phrase = PdfElementGenerator.createTableCellContentPhrase(column1Contents[0] + "\n\r" +
                                                                                       column1Contents[1] + "\n\r" +
                                                                                       column1Contents[2] + "\n\r" +
                                                                                       column1Contents[3] + "\n\r" +
                                                                                       column1Contents[4]);
            firstRowCells[1].Phrase = PdfElementGenerator.createTableCellContentPhrase(column2Contents[0] + "\n\r" +
                                                                                       column2Contents[1] + "\n\r" +
                                                                                       column2Contents[2] + "\n\r" +
                                                                                       column2Contents[3] + "\n\r" +
                                                                                       column2Contents[4]);
            this.document.Add(PdfElementGenerator.createSectionFrameTable(loggingSummaryTable));
            PdfElementGenerator.AddEmptyParagraphToDocument(this.document, this.sectionMargin);
        }

        protected override void GenerateAlarms()
        {
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

            this.document.Add(PdfElementGenerator.createSectionTitleParagraph(this.reportdataGenerator.GetAlarmSectionTitle(this.device)));
            PdfPTable alarmTable = PdfElementGenerator.createTable(new float[] { 0.18f, 0.19f, 0.17f, 0.10f, 0.23f, 0.13f }, true, false);
            if (this.device.AlarmMode == 0)
            {
                PdfElementGenerator.AddEmptyRowToTable(alarmTable, false, true, false);
                alarmTable.Rows[0].GetCells()[0].FixedHeight = 40f;
            }
            else if (this.device.AlarmMode == 1)
            {
                PdfPCell[] tableHeaderRowCells = PdfElementGenerator.AddEmptyRowToTable(alarmTable, false, true, false);
                tableHeaderRowCells[0].Phrase = PdfElementGenerator.createTableHeaderPhrase("Alarm Zones");
                tableHeaderRowCells[1].Phrase = PdfElementGenerator.createTableHeaderPhrase("Alarm Delay");
                tableHeaderRowCells[2].Phrase = PdfElementGenerator.createTableHeaderPhrase("Total Time");
                tableHeaderRowCells[3].Phrase = PdfElementGenerator.createTableHeaderPhrase("Events");
                tableHeaderRowCells[4].Phrase = PdfElementGenerator.createTableHeaderPhrase("First Triggered");
                tableHeaderRowCells[5].Phrase = PdfElementGenerator.createTableHeaderPhrase("Alarm Status");
                tableHeaderRowCells[5].HorizontalAlignment = Element.ALIGN_RIGHT;
                if (reportdataGenerator.IsStringArrayNotEmpty(highRowContents))
                {
                    PdfPCell[] firstRowCells = PdfElementGenerator.AddEmptyRowToTable(alarmTable, false, true, false);
                    PdfElementGenerator.SetTableCellContentValue(firstRowCells, highRowContents);
                    totalRows--;
                }

                if (reportdataGenerator.IsStringArrayNotEmpty(lowRowContents))
                {
                    PdfPCell[] secondRowCells = PdfElementGenerator.AddEmptyRowToTable(alarmTable, false, true, false);
                    PdfElementGenerator.SetTableCellContentValue(secondRowCells, lowRowContents);
                    totalRows--;
                }
            }
            else if (this.device.AlarmMode == 2)
            {
                PdfPCell[] tableHeaderRowCells = PdfElementGenerator.AddEmptyRowToTable(alarmTable, false, true, false);
                tableHeaderRowCells[0].Phrase = PdfElementGenerator.createTableHeaderPhrase("Alarm Zones");
                tableHeaderRowCells[1].Phrase = PdfElementGenerator.createTableHeaderPhrase("Alarm Delay");
                tableHeaderRowCells[2].Phrase = PdfElementGenerator.createTableHeaderPhrase("Total Time");
                tableHeaderRowCells[3].Phrase = PdfElementGenerator.createTableHeaderPhrase("Events");
                tableHeaderRowCells[4].Phrase = PdfElementGenerator.createTableHeaderPhrase("First Triggered");
                tableHeaderRowCells[5].Phrase = PdfElementGenerator.createTableHeaderPhrase("Alarm Status");
                tableHeaderRowCells[5].HorizontalAlignment = Element.ALIGN_RIGHT;

                if (reportdataGenerator.IsStringArrayNotEmpty(a1RowContents))
                {
                    PdfPCell[] firstRowCells = PdfElementGenerator.AddEmptyRowToTable(alarmTable, false, true, true);
                    PdfElementGenerator.SetTableCellContentValue(firstRowCells, a1RowContents);
                    totalRows--;
                }

                if (reportdataGenerator.IsStringArrayNotEmpty(a2RowContents))
                {
                    PdfPCell[] secondRowCells = PdfElementGenerator.AddEmptyRowToTable(alarmTable, false, true, true);
                    PdfElementGenerator.SetTableCellContentValue(secondRowCells, a2RowContents);
                    totalRows--;
                }

                if (reportdataGenerator.IsStringArrayNotEmpty(a3RowContents))
                {
                    PdfPCell[] thirdRowCells = PdfElementGenerator.AddEmptyRowToTable(alarmTable, false, true, true);
                    PdfElementGenerator.SetTableCellContentValue(thirdRowCells, a3RowContents);
                    totalRows--;
                }

                if (reportdataGenerator.IsStringArrayNotEmpty(a4RowContents))
                {
                    PdfPCell[] fourthRowCells = PdfElementGenerator.AddEmptyRowToTable(alarmTable, false, true, true);
                    PdfElementGenerator.SetTableCellContentValue(fourthRowCells, a4RowContents);
                    totalRows--;
                }

                if (reportdataGenerator.IsStringArrayNotEmpty(a5RowContents))
                {
                    PdfPCell[] fifthRowCells = PdfElementGenerator.AddEmptyRowToTable(alarmTable, false, true, true);
                    PdfElementGenerator.SetTableCellContentValue(fifthRowCells, a5RowContents);
                    totalRows--;
                }

                if (reportdataGenerator.IsStringArrayNotEmpty(a6RowContents))
                {
                    PdfPCell[] sixthRowCells = PdfElementGenerator.AddEmptyRowToTable(alarmTable, false, true, true);
                    PdfElementGenerator.SetTableCellContentValue(sixthRowCells, a6RowContents);
                    totalRows--;
                }
            }
            for (int i = 0; i < totalRows; i++)
            {
                string[] emptyRow = new string[6] { " ", " ", " ", " ", " ", " " };
                PdfPCell[] emptyRowCells = PdfElementGenerator.AddEmptyRowToTable(alarmTable, false, true, true);
                PdfElementGenerator.SetTableCellContentValue(emptyRowCells, emptyRow);
            }

            this.document.Add(PdfElementGenerator.createSectionFrameTable(alarmTable, true));
            PdfElementGenerator.AddEmptyParagraphToDocument(this.document, this.sectionMargin);
        }

        protected override void GenerateComments()
        {
            string finalCommentString = string.Empty;
            if (this.CurrentComment != ReportConstString.CommentDefaultString)
            {
                finalCommentString = this.CurrentComment;
            }
            this.document.Add(PdfElementGenerator.createSectionTitleParagraph("Comments"));
            PdfPTable commentTable = PdfElementGenerator.createTable(new float[] { 1f });
            PdfPCell[] firstRowCells = commentTable.Rows[0].GetCells();
            firstRowCells[0].Phrase = PdfElementGenerator.createTableCellContentPhrase(finalCommentString);
            firstRowCells[0].FixedHeight = 25f;
            this.document.Add(PdfElementGenerator.createSectionFrameTable(commentTable));
        }

        protected override void GenerateDataGraph()
        {
            PdfPTable dataGraphTable = PdfElementGenerator.createTable(new float[] { 1f });
            PdfPCell cell = dataGraphTable.Rows[0].GetCells()[0];
            cell.Padding = 0;
            cell.FixedHeight = 310f;
            cell.Image = this.getImage(this.device.ReportGraph);
            dataGraphTable.SetTotalWidth(new float[] { 475f });
            dataGraphTable.WriteSelectedRows(0, -1, 60f, 360, this.writer.DirectContent);
            //this.document.Add(PdfElementGenerator.createSectionFrameTable(dataGraphTable, false));
        }

        protected override void GenerateSignatures()
        {
            if (this.isSignatureShown)
            {
                document.NewPage();
                float sectionMargin = 10f;
                float cellVerticalPadding = 4f;
                this.document.Add(PdfElementGenerator.createSectionTitleParagraph("Electronic Signatures"));
                PdfPTable signatureTable = PdfElementGenerator.createTable(new float[] { 0.05f, 0.95f }, false, false);
                for (int i = 0; i < this.signatureList.Count; i++)
                {
                    PdfPCell[] newRowCells = PdfElementGenerator.AddEmptyRowToTable(signatureTable, true);
                    newRowCells[0].Phrase = PdfElementGenerator.createTableCellContentPhrase((i + 1).ToString());
                    newRowCells[1].Phrase = PdfElementGenerator.createTableCellContentPhrase(this.signatureList[i].ToString(Common.GlobalProfile.DateTimeFormator));
                    newRowCells[0].HorizontalAlignment = Element.ALIGN_CENTER;
                    foreach (var item in newRowCells)
                    {
                        item.PaddingTop = cellVerticalPadding;
                        item.PaddingBottom = cellVerticalPadding;
                    }
                }
                this.document.Add(PdfElementGenerator.createSectionFrameTable(signatureTable, false, true));
                this.rowsInfectedBySignature = (int)((30 + signatureTable.TotalHeight + sectionMargin * 3) / this.dataListContentCellHeight);
                PdfElementGenerator.AddEmptyParagraphToDocument(this.document, this.sectionMargin);
            }
        }

        protected override void GenerateDataList()
        {
            if (!this.isSignatureShown)
            {
                this.document.NewPage();
            }
            PdfPTable dataListTable = null;
            PdfPCell[] dataListCells = null;
            int columnIndex = 0;
            float[] listCellLayout = new float[] { 0.37f, 0.43f, 0.20f };
            PdfPTable cellContentTable = null;
            int cellDataCount = 100;
            int columnPerPage = 4;
            int dataPerPage = cellDataCount * columnPerPage;
            
            cellContentTable = PdfElementGenerator.createTable(listCellLayout, false, true);
            for (int i = 0; i < Math.Min((cellDataCount - this.rowsInfectedBySignature) * columnPerPage, this.device.tempList.Count); i++)
            {
                if (i != 0 && i % (cellDataCount - this.rowsInfectedBySignature) == 0)
                {
                    if (cellContentTable != null)
                    {
                        dataListCells[columnIndex] = new PdfPCell(cellContentTable);
                    }
                    cellContentTable = PdfElementGenerator.createTable(listCellLayout, false, true);
                    columnIndex = (i / (cellDataCount - this.rowsInfectedBySignature)) % columnPerPage;
                }
                if (i % dataPerPage == 0)
                {
                    dataListTable = PdfElementGenerator.createTable(new float[] { 0.25f, 0.25f, 0.25f, 0.25f }, true);
                    PdfPCell[] headerRowCells = dataListTable.Rows[0].GetCells();
                    for (int j = 0; j < headerRowCells.Length; j++)
                    {
                        PdfPTable headerTable = PdfElementGenerator.createTable(listCellLayout, false, true);
                        PdfPCell[] headerCells = headerTable.Rows[0].GetCells();
                        headerCells[0].Phrase = PdfElementGenerator.createTableHeaderPhrase("Date");
                        headerCells[1].Phrase = PdfElementGenerator.createTableHeaderPhrase("Time");
                        headerCells[2].Phrase = PdfElementGenerator.createTableHeaderPhrase(unit);
                        foreach (var item in headerCells)
                        {
                            item.HorizontalAlignment = Element.ALIGN_CENTER;
                            //item.PaddingTop = 1f;
                        }
                        headerRowCells[j] = new PdfPCell(headerTable);
                        headerRowCells[j].PaddingTop = 2f;
                    }
                    dataListCells = PdfElementGenerator.AddEmptyRowToTable(dataListTable, true, true);
                }
                PdfPCell[] rowCells = PdfElementGenerator.AddEmptyRowToTable(cellContentTable, false, false);
                rowCells[0].Phrase = PdfElementGenerator.createDataListContentPhrase(TempsenFormatHelper.GetFormattedDate(this.device.tempList[i].PointTime.ToLocalTime()), this.device.tempList[i].IsMark);
                rowCells[1].Phrase = PdfElementGenerator.createDataListContentPhrase(TempsenFormatHelper.GetFormattedTime(this.device.tempList[i].PointTime.ToLocalTime()), this.device.tempList[i].IsMark);
                rowCells[2].Phrase = PdfElementGenerator.createDataListContentPhrase(TempsenFormatHelper.GetFormattedTemperature(this.device.tempList[i].PointTemp), this.device.tempList[i].IsMark);
                foreach (var item in rowCells)
                {
                    item.HorizontalAlignment = Element.ALIGN_CENTER;
                    item.PaddingLeft = 0;
                    item.PaddingRight = 0;
                    item.PaddingTop = 0.0f;
                    item.PaddingBottom = 0.8f;
                }
                if (i == Math.Min((cellDataCount - this.rowsInfectedBySignature) * columnPerPage, this.device.tempList.Count) - 1)
                {
                    dataListCells[columnIndex] = new PdfPCell(cellContentTable);
                    for (int j = columnPerPage - 1; j > columnIndex; j--)
                    {
                        dataListTable.Rows[0].GetCells()[j].Phrase = PdfElementGenerator.createTableHeaderPhrase(string.Empty);
                    }
                    this.document.Add(PdfElementGenerator.createSectionFrameTable(dataListTable, false));
                    cellContentTable = null;
                    dataListTable = null;
                    columnIndex = 0;
                    break;
                }
            }
            for (int i = (cellDataCount - this.rowsInfectedBySignature) * columnPerPage; i < device.tempList.Count; i++)
            {
                int k = i - ((cellDataCount - this.rowsInfectedBySignature) * columnPerPage);
                if (k == 0)
                {
                    cellContentTable = PdfElementGenerator.createTable(listCellLayout, false, true);
                }
                if (k != 0 && k % cellDataCount == 0)
                {
                    if (cellContentTable != null)
                    {
                        dataListCells[columnIndex] = new PdfPCell(cellContentTable);
                    }
                    cellContentTable = PdfElementGenerator.createTable(listCellLayout, false, true);
                    columnIndex = (k / cellDataCount) % columnPerPage;
                }
                if (k % dataPerPage == 0)
                {
                    if (dataListTable != null)
                    {
                        this.document.Add(PdfElementGenerator.createSectionFrameTable(dataListTable, false));
                    }
                    dataListTable = PdfElementGenerator.createTable(new float[] { 0.25f, 0.25f, 0.25f, 0.25f }, true);
                    PdfPCell[] headerRowCells = dataListTable.Rows[0].GetCells();
                    for (int j = 0; j < headerRowCells.Length; j++)
                    {
                        PdfPTable headerTable = PdfElementGenerator.createTable(listCellLayout, false, true);
                        PdfPCell[] headerCells = headerTable.Rows[0].GetCells();
                        headerCells[0].Phrase = PdfElementGenerator.createTableHeaderPhrase("Date");
                        headerCells[1].Phrase = PdfElementGenerator.createTableHeaderPhrase("Time");
                        headerCells[2].Phrase = PdfElementGenerator.createTableHeaderPhrase(unit);
                        foreach (var item in headerCells)
                        {
                            item.HorizontalAlignment = Element.ALIGN_CENTER;
                        }
                        headerRowCells[j] = new PdfPCell(headerTable);
                        headerRowCells[j].PaddingTop = 2f;
                    }
                    dataListCells = PdfElementGenerator.AddEmptyRowToTable(dataListTable, true, true);
                }
                PdfPCell[] rowCells = PdfElementGenerator.AddEmptyRowToTable(cellContentTable, false, false);
                rowCells[0].Phrase = PdfElementGenerator.createDataListContentPhrase(TempsenFormatHelper.GetFormattedDate(this.device.tempList[i].PointTime.ToLocalTime()), this.device.tempList[i].IsMark);
                rowCells[1].Phrase = PdfElementGenerator.createDataListContentPhrase(TempsenFormatHelper.GetFormattedTime(this.device.tempList[i].PointTime.ToLocalTime()), this.device.tempList[i].IsMark);
                rowCells[2].Phrase = PdfElementGenerator.createDataListContentPhrase(TempsenFormatHelper.GetFormattedTemperature(this.device.tempList[i].PointTemp), this.device.tempList[i].IsMark);
                foreach (var item in rowCells)
                {
                    item.HorizontalAlignment = Element.ALIGN_CENTER;
                    item.PaddingLeft = 0;
                    item.PaddingRight = 0;
                    item.PaddingTop = 0.0f;
                    item.PaddingBottom = 0.8f;
                }
                if (i == device.tempList.Count - 1)
                {
                    dataListCells[columnIndex] = new PdfPCell(cellContentTable);
                    for (int j = columnPerPage - 1; j > columnIndex; j--)
                    {
                        dataListTable.Rows[0].GetCells()[j].Phrase = PdfElementGenerator.createTableHeaderPhrase(string.Empty);
                    }
                    this.document.Add(PdfElementGenerator.createSectionFrameTable(dataListTable, false));
                    break;
                }
            }
        }

        protected override void GenerateReportFooter()
        {
        }


        public override bool GenerateReport()
        {
            bool result = true;
            PdfWriter writer = null;
            try
            {
                PdfPageEventHelperForFooter footerEventHelper = new PdfPageEventHelperForFooter(this.fileNameWithFullPath);
                document = new Document(PageSize.A4, 0f, 0f, 30f, 50f);
                DirectoryInfo directoryInfo = new DirectoryInfo(this.fileNameWithFullPath.Substring(0, this.fileNameWithFullPath.LastIndexOf("\\") + 1));
                if (!directoryInfo.Exists)
                {
                    directoryInfo.Create();
                }
                writer = PdfWriter.GetInstance(document, new FileStream(fileNameWithFullPath, FileMode.Create));
                if (!this.IsTempFile)
                {
                    writer.SetEncryption(PdfEncryption.STANDARD_ENCRYPTION_128, string.Empty, string.Empty, PdfWriter.ALLOW_PRINTING);
                }
                writer.PageEvent = footerEventHelper;
                this.writer = writer;
                document.Open();
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

                this.document.Close();
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
                if (this.document.IsOpen())
                {
                    this.document.Close();
                }
                if (writer != null)
                {
                    writer.Close();
                }
            }
            return result;
        }

        
    }
}
