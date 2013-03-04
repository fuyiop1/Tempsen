using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using ShineTech.TempCentre.Platform;
using iTextSharp.text;

namespace ShineTech.TempCentre.BusinessFacade
{
    public class PdfPageEventHelperForFooter : PdfPageEventHelper
    {
        private string fileName;

        private PdfContentByte contentByte;
        private PdfTemplate template;
        private string createTime = string.Format("Created at: {0}", TempsenFormatHelper.GetFormattedDateTime(DateTime.Now));
        private float pageNumberLength;

        private BaseFont baseFooterFont = PdfElementGenerator.GetNormalBaseFont();
        private float fontSize = 7f * PdfElementGenerator.FontAdjustParameter;

        protected Font footerFont
        {
            get
            {
                Font font = FontFactory.GetFont("Arial", 8, Font.NORMAL, BaseColor.BLACK);
                return font;
            }
        }   

        public PdfPageEventHelperForFooter(string fileName)
        {
            this.fileName = string.Format("File Name: {0}", fileName.Substring(fileName.LastIndexOf("\\") + 1));
        }

        private PdfPCell getFooterCell(string content)
        {
            PdfPCell result = null;
            result = new PdfPCell(PdfElementGenerator.createFooterPhrase(content));
            result.Border = 0;
            return result;
        }

        public override void OnOpenDocument(PdfWriter writer, Document document)
        {
            base.OnOpenDocument(writer, document);
            this.contentByte = writer.DirectContent;
            this.template = contentByte.CreateTemplate(30, 10);
        }

        public override void OnEndPage(PdfWriter writer, iTextSharp.text.Document document)
        {
            base.OnEndPage(writer, document);
            Rectangle pageSize = document.PageSize;
            this.contentByte.AddTemplate(this.template, 60f, 24.5f);


            PdfPTable footer = new PdfPTable(new float[3] { 0.35f, 0.30f, 0.35f });
            float footerHorizonalPadding = 40f;
            float footerHorizonalTableCellPadding = 60f - footerHorizonalPadding;
            footer.TotalWidth = document.PageSize.Width - footerHorizonalPadding * 2;

            PdfPCell[] footerFirstRowCells = new PdfPCell[3];

            footerFirstRowCells[0] = this.getFooterCell(this.fileName);
            footerFirstRowCells[1] = this.getFooterCell(this.createTime);
            footerFirstRowCells[2] = this.getFooterCell(ReportConstString.PoweredBy);

            footerFirstRowCells[0].PaddingLeft = footerHorizonalTableCellPadding;
            footerFirstRowCells[2].PaddingRight = footerHorizonalTableCellPadding;
            footerFirstRowCells[2].HorizontalAlignment = Element.ALIGN_RIGHT;

            foreach (var item in footerFirstRowCells)
            {
                item.Border = 1;
                item.BorderColor = BaseColor.GRAY;
                item.PaddingTop = 10;
                footer.AddCell(item);
            }

            PdfPCell[] footerSecondRowCells = new PdfPCell[3];
            string pageNumber = string.Format("{0} / ", document.PageNumber.ToString());
            this.pageNumberLength = this.baseFooterFont.GetWidthPoint(pageNumber, fontSize);
            footerSecondRowCells[0] = this.getFooterCell(pageNumber);
            footerSecondRowCells[1] = this.getFooterCell(string.Empty);
            footerSecondRowCells[2] = this.getFooterCell(ReportConstString.Site);

            footerSecondRowCells[0].PaddingLeft = footerHorizonalTableCellPadding;
            footerSecondRowCells[2].PaddingRight = footerHorizonalTableCellPadding;
            footerSecondRowCells[2].HorizontalAlignment = Element.ALIGN_RIGHT;

            foreach (var item in footerSecondRowCells)
            {
                footer.AddCell(item);
            }

            footer.WriteSelectedRows(0, -1, footerHorizonalPadding, 50, writer.DirectContent);
        }

        public override void OnCloseDocument(PdfWriter writer, Document document)
        {
            base.OnCloseDocument(writer, document);
            this.template.BeginText();
            this.template.SetFontAndSize(this.baseFooterFont, fontSize);
            this.template.SetTextMatrix(pageNumberLength, 0);
            this.template.ShowText((writer.PageNumber - 1).ToString());
            this.template.EndText();
        }
    }
}
