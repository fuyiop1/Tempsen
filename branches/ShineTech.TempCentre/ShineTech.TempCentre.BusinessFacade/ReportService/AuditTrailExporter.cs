using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using ShineTech.TempCentre.DAL;
using System.Globalization;

namespace ShineTech.TempCentre.BusinessFacade.ReportService
{
    public class AuditTrailExporter : IReportExportService
    {
        private bool IsTempFile;
        private IList<OperationLog> logListToPrint;

        public string Title
        {
            get
            {
                return "";
            }
            set
            {
            }
        }

        public string CurrentComment
        {
            get
            {
                return "";
            }
            set
            {
            }
        }

        public IList<DAL.DigitalSignature> SignatureList
        {
            get
            {
                return null;
            }
            set
            {
            }
        }

        public AuditTrailExporter(string fileNameWithFullPath, string password, IList<OperationLog> logListToPrint) : this(fileNameWithFullPath, password, logListToPrint, false)
        {
        }

        public AuditTrailExporter(string fileNameWithFullPath, string password, IList<OperationLog> logListToPrint, bool isTempFile)
        {
            this.fileNameWithFullPath = fileNameWithFullPath;
            this.password = password;
            this.logListToPrint = logListToPrint;
            this.IsTempFile = isTempFile;
            this.logBll = new OperationLogBLL();
        }

        private Document document;
        private BaseFont baseFont;
        private string fileNameWithFullPath;
        private string password;

        private OperationLogBLL logBll;

        

        public bool GenerateReport()
        {
            bool result = true;
            PdfReader reader = null;
            FileStream encryptedFile = null;
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
                document.Open();

                PdfPTable titleTable = PdfElementGenerator.createTable(new float[1] { 1f }, false, true);
                titleTable.Rows[0].GetCells()[0].Phrase = PdfElementGenerator.createDocumentTitlePhrase(DocumentTitle);
                titleTable.Rows[0].GetCells()[0].HorizontalAlignment = Element.ALIGN_CENTER;
                document.Add(titleTable);

                IList<OperationLog> logList = this.logListToPrint;
                if (logList != null && logList.Count > 0)
                {
                    PdfPTable logTable = PdfElementGenerator.createTable(new float[5] { 0.12f, 0.12f, 0.12f, 0.18f, 0.46f }, true);
                    PdfPCell[] headerRowCells = logTable.Rows[0].GetCells();
                    headerRowCells[0].Phrase = PdfElementGenerator.createTableHeaderPhrase("Action");
                    headerRowCells[1].Phrase = PdfElementGenerator.createTableHeaderPhrase("User Name");
                    headerRowCells[2].Phrase = PdfElementGenerator.createTableHeaderPhrase("Full Name");
                    headerRowCells[3].Phrase = PdfElementGenerator.createTableHeaderPhrase("Date");
                    headerRowCells[4].Phrase = PdfElementGenerator.createTableHeaderPhrase("Detail");

                    foreach (var item in logList)
                    {
                        PdfPCell[] addedRowCells = PdfElementGenerator.AddEmptyRowToTable(logTable, true);
                        addedRowCells[0].Phrase = PdfElementGenerator.createTableCellContentPhrase(item.Action);
                        addedRowCells[1].Phrase = PdfElementGenerator.createTableCellContentPhrase(item.Username);
                        addedRowCells[2].Phrase = PdfElementGenerator.createTableCellContentPhrase(item.Fullname);
                        addedRowCells[3].Phrase = PdfElementGenerator.createTableCellContentPhrase(item.Operatetime.ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture));
                        addedRowCells[4].Phrase = PdfElementGenerator.createTableCellContentPhrase(item.Detail);
                    }

                    this.document.Add(logTable);
                }


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
                if (reader != null)
                {
                    reader.Close();
                }
                if (encryptedFile != null)
                {
                    encryptedFile.Close();
                }
            }
            return result;
        }

        private readonly static string DocumentTitle = "Audit Trail";
    }
}
