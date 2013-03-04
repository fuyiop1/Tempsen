using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Drawing.Text;
using Microsoft.Win32;
using System.IO;
using System.Globalization;

namespace ShineTech.TempCentre.BusinessFacade
{
    public sealed class PdfElementGenerator
    {
       
        public static Document AddEmptyParagraphToDocument(Document document, float height)
        {
            PdfPTable table = createTable(new float[] { 1f }, false, false);
            PdfPCell[] cells = AddEmptyRowToTable(table);
            cells[0].FixedHeight = height;
            document.Add(table);
            return document;
        }

        public static PdfPCell[] AddEmptyRowToTable(PdfPTable table)
        {
            return AddEmptyRowToTable(table, false);
        }

        public static PdfPCell[] AddEmptyRowToTable(PdfPTable table, bool isBorderShown)
        {
            return AddEmptyRowToTable(table, isBorderShown, true);
        }

        public static PdfPCell[] AddEmptyRowToTable(PdfPTable table, bool isBorderShown, bool isWithPadding)
        {
            return AddEmptyRowToTable(table, isBorderShown, true, false);
        }

        public static PdfPCell[] AddEmptyRowToTable(PdfPTable table, bool isBorderShown, bool isWithPadding, bool isAlighCenter)
        {
            PdfPCell[] rowCells = new PdfPCell[table.NumberOfColumns];
            for (int i = 0; i < rowCells.Length; i++)
            {
                rowCells[i] = new PdfPCell();
                if (!isBorderShown)
                {
                    rowCells[i].Border = 0;
                }
                if (!isWithPadding)
                {
                    rowCells[i].Padding = 0f;
                }
                if (isAlighCenter)
                {
                    rowCells[i].HorizontalAlignment = Element.ALIGN_CENTER;
                }
            }
            PdfPRow addedRow = new PdfPRow(rowCells);
            table.Rows.Add(addedRow);
            return addedRow.GetCells();
        }

        public static PdfPTable createTable(float[] columnsWidths)
        {
            return createTable(columnsWidths, false);
        }

        public static PdfPTable createTable(float[] columnsWidths, bool isBorderShown)
        {
            return createTable(columnsWidths, isBorderShown, true);
        }

        public static PdfPTable createTable(float[] columnsWidths, bool isBorderShown, bool isWithDefaultRow)
        {
            PdfPTable result = new PdfPTable(columnsWidths);
            if (isWithDefaultRow)
            {
                PdfPCell[] firstRowCells = new PdfPCell[columnsWidths.Length];
                for (int i = 0; i < firstRowCells.Length; i++)
                {
                    firstRowCells[i] = new PdfPCell();
                    if (!isBorderShown)
                    {
                        firstRowCells[i].Border = 0;
                    }
                }
                result.Rows.Add(new PdfPRow(firstRowCells));
            }
            return result;
        }

        public static PdfPTable createSectionFrameTable(PdfPTable includedTable)
        {
            return createSectionFrameTable(includedTable, true);
        }

        public static PdfPTable createSectionFrameTable(PdfPTable includedTable, bool isBorderShown)
        {
            return createSectionFrameTable(includedTable, isBorderShown, true);
        }

        public static PdfPTable createSectionFrameTable(PdfPTable includedTable, bool isBorderShown, bool isWithAdditionalEmptyRows)
        {
            int rowIndex = 0;
            PdfPTable result = new PdfPTable(new float[1] { 1f });
            if (isWithAdditionalEmptyRows)
            {
                AddEmptyRowToTable(result);
                AddEmptyRowToTable(result);
                rowIndex = 2;
            }
            PdfPCell[] firstRowCells = new PdfPCell[1] { new PdfPCell(includedTable) };
            result.Rows.Add(new PdfPRow(firstRowCells));
            if (!isBorderShown)
            {
                result.Rows[rowIndex].GetCells()[0].Border = 0;
            }
            result.Rows[rowIndex].GetCells()[0].Padding = 3f;
            return result;
        }

        private static BaseFont _baseFontEn;
        private static Font _fontEn;
        private static Font _boldFontEn;
        private static Font _markedPointFontEn;

        private static BaseFont _baseFontZh;
        private static Font _fontZh;
        private static Font _boldFontZh;
        private static Font _markedPointFontZh;


        private static float fontAdjustParameter = 1f;

        public static BaseFont GetNormalBaseFont()
        {
            initFonts();
            return _baseFontEn;
        }

        public static float FontAdjustParameter
        {
            get
            {
                return fontAdjustParameter;
            }
        }

        public static Phrase createPhrase(string content, float fontSize, bool isBold)
        {
            return createPhrase(content, fontSize, isBold, false);
        }

        public static Phrase createPhrase(string content, float fontSize, bool isBold, bool isMarked)
        {
            Phrase result;
            initFonts();
            result = new Phrase();
            var chunkList = new List<Chunk>();
            if (content != null && content.Length > 0)
            {
                content = content.Replace(Environment.NewLine, "\n");
                int startIndex = 0;
                int length = 0;
                bool isEnglishChar = IsEnglishChar(content.First());
                Font font = GetFontForChar(content.First(), fontSize, isBold, isMarked);
                for (int i = 0; i < content.Length; i++)
                {
                    char item = content[i];
                    if (isEnglishChar == IsEnglishChar(item))
                    {
                        length++;
                    }
                    else
                    {
                        if (length > 0)
                        {
                            chunkList.Add(new Chunk(content.Substring(startIndex, length), font));
                        }
                        startIndex = i;
                        length = 1;
                        font = GetFontForChar(item, fontSize, isBold, isMarked);
                    }
                }
                chunkList.Add(new Chunk(content.Substring(startIndex, length), font));
            }
            result.AddAll<Chunk>(chunkList);
            return result;
        }

        private static bool IsEnglishChar(char ch)
        {
            if (ch < 128 || ch == '°')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static Font GetFontForChar(char ch, float fontSize, bool isBold, bool isMarked)
        {
            Font result = null;
            if (IsEnglishChar(ch))
            {
                if (isBold)
                {
                    result = _boldFontEn;
                }
                else
                {
                    result = _fontEn;
                }
                if (isMarked)
                {
                    result = _markedPointFontEn;
                }
            }
            else
            {
                if (isBold)
                {
                    result = _boldFontZh;
                }
                else
                {
                    result = _fontZh;
                }
                if (isMarked)
                {
                    result = _markedPointFontZh;
                }
            }
            result.Size = fontSize;
            return result;
        }

        private static void initFonts()
        {
            if (_baseFontEn == null || _fontEn == null || _boldFontEn == null || _markedPointFontEn == null)
            {
                try
                {
                    _baseFontEn = BaseFont.CreateFont(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "ARIAL.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    fontAdjustParameter = 1f;
                }
                catch
                {
                    _baseFontEn = BaseFont.CreateFont();
                    fontAdjustParameter = 1f;
                }
                _fontEn = new Font(_baseFontEn);
                _boldFontEn = new Font(_baseFontEn);
                _markedPointFontEn = new Font(_baseFontEn);
                _fontEn.SetStyle(Font.NORMAL);
                _boldFontEn.SetStyle(Font.BOLD);
                _markedPointFontEn.SetStyle(Font.BOLD);
                _markedPointFontEn.Color = BaseColor.RED;
            }
            if (_baseFontZh == null || _fontZh == null || _boldFontZh == null || _markedPointFontZh == null)
            {
                try
                {
                    _baseFontZh = BaseFont.CreateFont(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "simsun.ttc,0"),BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
                    fontAdjustParameter = 1f;
                }
                catch
                {
                    _baseFontZh = BaseFont.CreateFont();
                    fontAdjustParameter = 1f;
                }
                _fontZh = new Font(_baseFontZh);
                _boldFontZh = new Font(_baseFontZh);
                _markedPointFontZh = new Font(_baseFontZh);
                _fontZh.SetStyle(Font.NORMAL);
                _boldFontZh.SetStyle(Font.BOLD);
                _markedPointFontZh.SetStyle(Font.BOLD);
                _markedPointFontZh.Color = BaseColor.RED;
            }
        }

        public static Phrase createTableCellContentPhrase(string content)
        {
            return createPhrase(content, 7.1f, false);
        }

        public static Phrase createTableHeaderPhrase(string content)
        {
            return createPhrase(content, 7.2f, true);
        }

        public static Phrase createSectionTitlePhrase(string content)
        {
            return createPhrase(content, 8.3f, true);
        }

        public static Phrase createDataListContentPhrase(string content, bool isMarked)
        {
            return createPhrase(content, 6.8f, isMarked, isMarked);
        }

        public static Phrase createFooterPhrase(string content)
        {
            return createPhrase(content, 7f, false);
        }

        public static Phrase createDocumentTitlePhrase(string documentTitle)
        {
            return createPhrase(documentTitle, 13f, true);
        }

        public static Paragraph createSectionTitleParagraph(string content)
        {
            Paragraph result;
            result = new Paragraph(createSectionTitlePhrase(content));
            result.IndentationLeft = 60f;
            return result;
        }

        public static void SetTableCellContentValue(PdfPCell [] cells, string[] values)
        {
            SetTableCellContentValue(cells, values, Element.ALIGN_LEFT);
        }

        public static void SetTableCellContentValue(PdfPCell[] cells, string[] values, int alignment)
        {
            if (cells != null && values != null)
            {
                int length = Math.Min(cells.Length, values.Length);
                for (int i = 0; i < length; i++)
                {
                    cells[i].Phrase = createTableCellContentPhrase(values[i]);
                    if (i ==  length - 1)
                    {
                        cells[i].HorizontalAlignment = Element.ALIGN_RIGHT;
                    }
                    else
                    {
                        cells[i].HorizontalAlignment = alignment;
                    }
                }

            }
        }

    }
}
