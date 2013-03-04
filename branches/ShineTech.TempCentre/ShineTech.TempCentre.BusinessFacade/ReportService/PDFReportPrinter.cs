using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Drawing.Printing;

using iTextSharp.text.pdf;
using System.Windows.Forms;
using ShineTech.TempCentre.Platform;

namespace ShineTech.TempCentre.BusinessFacade
{
    public class PDFReportPrinter : IPrintService
    {
        private PrintDocument printDocument;
        private PrintDialog printDialog;

        private bool isPrintDialogShown;
        private int pageCount;
        private int currentPageIndex;
        private int endPageIndex;
        private string fileNameWithFullPath;

        public PrintDocument PrintDocument
        {
            get
            {
                return this.printDocument;
            }
        }

        public PDFReportPrinter() { }

        public PDFReportPrinter(string fileNameWithFullPath)
        {
            this.printDocument = new PrintDocument();
            this.printDialog = new PrintDialog();
            this.printDialog.Document = this.printDocument;
            this.printDialog.AllowSomePages = true;
            this.isPrintDialogShown = false;
            this.printDocument.DocumentName = "Report";
            this.fileNameWithFullPath = fileNameWithFullPath;
            this.PrintDocument.BeginPrint += new PrintEventHandler(printDocument_BeginPrint);
            this.printDocument.PrintPage += new PrintPageEventHandler(printDocument_PrintPage);
            this.printDocument.EndPrint += new PrintEventHandler(printDocument_EndPrint);
            PdfReader pdfReader = null;
            try
            {
                pdfReader = new PdfReader(fileNameWithFullPath);
                this.pageCount = pdfReader.NumberOfPages;
            }
            catch (Exception)
            {
            }
            finally
            {
                if (pdfReader != null)
                {
                    pdfReader.Close();
                }
            }
            this.printDialog.PrinterSettings.FromPage = 1;
            this.printDialog.PrinterSettings.ToPage = this.pageCount;
            this.currentPageIndex = 0;
            this.endPageIndex = this.pageCount;
        }

        public void PrintReport()
        {
            if (this.printDialog !=  null && this.printDocument != null)
            { 
                DialogResult dialogResult = this.printDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    this.printDocument.Print();
                }
            }   
        }

        private void printDocument_PrintPage(object sender, PrintPageEventArgs e)
        {
            if (this.pageCount < 1)
            {
                e.HasMorePages = false;
            }
            else
            {
                this.currentPageIndex++;
                Graphics g = e.Graphics;
                g.DrawImage(this.GetImage(this.fileNameWithFullPath, currentPageIndex), new Point(0, 0));
                if (this.currentPageIndex >= this.endPageIndex)
                {
                    e.HasMorePages = false;
                }
                else
                {
                    e.HasMorePages = true;
                }
            }
        }

        private void printDocument_BeginPrint(object sender, PrintEventArgs e)
        {
            if (this.isPrintDialogShown)
            {
                if (this.printDialog.ShowDialog() == DialogResult.OK)
                {
                    e.Cancel = true;
                    if (!(this.printDialog.PrinterSettings.FromPage > 0 && this.printDialog.PrinterSettings.ToPage >= this.printDialog.PrinterSettings.FromPage && this.printDialog.PrinterSettings.ToPage <= this.pageCount))
                    {
                        Utils.ShowMessageBox(Messages.IncorrectPrintSetting, Messages.TitleError);
                        return;
                    }
                    else
                    {
                        this.isPrintDialogShown = false;
                        this.currentPageIndex = this.printDialog.PrinterSettings.FromPage - 1;
                        this.endPageIndex = this.printDialog.PrinterSettings.ToPage;
                        this.printDocument.Print();
                    }
                }
                else
                {
                    e.Cancel = true;
                }
            }
            
        }

        private void printDocument_EndPrint(object sender, PrintEventArgs e)
        {
            this.isPrintDialogShown = true;
            this.currentPageIndex = 0;
            this.endPageIndex = this.pageCount;
        }

        /// <summary>
        /// 将PDF 相应的页转换为图片
        /// </summary>
        /// <param name="strPDFpath">PDF 路径</param>
        /// <param name="Page">需要转换的页页码</param>
        private Image GetImage(string strPDFpath, int Page)
        {
            Image result = null;
            FileInfo file = new FileInfo(strPDFpath);
            string strSavePath = file.Directory.FullName;
            byte[] ImgData = GetImgData(strPDFpath, Page);

            MemoryStream ms = new MemoryStream(ImgData, 0, ImgData.Length);
            Bitmap returnImage = (Bitmap)Bitmap.FromStream(ms);
            result = returnImage;
            return result;
        }

        /// <summary>
        /// 从PDF中获取首页的图片
        /// </summary>
        /// <param name="PDFPath">PDF 文件路径</param>
        /// <param name="Page">需要获取的第几页</param>
        /// <returns></returns>
        private byte[] GetImgData(string PDFPath, int Page)
        {
            System.Drawing.Image img = PDFView.ConvertPDF.PDFConvert.GetPageFromPDF(PDFPath, Page, 300, "", true);
            return GetDataByImg(img);//读取img的数据并返回
        }

        /// <summary>
        /// 将单页的PDF转换为图片
        /// </summary>
        /// <param name="_image"></param>
        /// <returns></returns>
        private byte[] GetDataByImg(System.Drawing.Image _image)
        {
            System.IO.MemoryStream Ms = new MemoryStream();
            _image.Save(Ms, System.Drawing.Imaging.ImageFormat.Jpeg);
            byte[] imgdata = new byte[Ms.Length];
            Ms.Position = 0;
            Ms.Read(imgdata, 0, Convert.ToInt32(Ms.Length));
            Ms.Close();
            return imgdata;
        }
    }
}
