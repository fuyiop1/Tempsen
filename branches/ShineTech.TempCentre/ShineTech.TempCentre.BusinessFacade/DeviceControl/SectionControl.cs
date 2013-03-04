using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.Platform;

namespace ShineTech.TempCentre.BusinessFacade
{
    public partial class SectionControl : Control
    {
        private readonly static Font DefaultTitleFont = new Font("Arial", 13, FontStyle.Bold, GraphicsUnit.Pixel);
        private readonly static Font DefaultHeaderFont = new Font("Arial", 11, FontStyle.Bold, GraphicsUnit.Pixel);
        private readonly static Font DefaultSmallHeaderFont = new Font("Arial", 13, FontStyle.Bold, GraphicsUnit.Pixel);
        private readonly static Font DefaultContentFont = new Font("Arial", 11, GraphicsUnit.Pixel);
        private readonly static Font DefaultSmallContentFont = new Font("Arial", 9, GraphicsUnit.Pixel);
        private readonly static Font MarkedDataItemFont = new Font("Arial", 9, FontStyle.Bold, GraphicsUnit.Pixel);
        private readonly static Brush DefaultBrush = new SolidBrush(Color.Black);
        private readonly static Brush RedBrush = new SolidBrush(Color.Red);
        private readonly static Pen DefaultPen = new Pen(Color.Black, 1);
        private float defaultRowHeight = 20;
        private int defaultTitleHeight = 30;
        private float[] columnsWidths;
        private readonly static int ContentFramePadding = 3;
        private bool isContentFrameWithPadding;
        private bool isTotalPageAdd;

        private bool isHeightReCaculated;

        public ContentAlignment HorizonalTextAlignment
        {
            get;
            set;
        }

        private ContentAlignment[] horizonalTextAlignments;
        
        public bool IsFootSection
        {
            get;
            set;
        }

        public bool IsBorderShown
        {
            get;
            set;
        }

        public bool IsCellBorderShown
        {
            get;
            set;
        }

        public bool IsContentWithSmallFont { get; set; }

        public bool IsAllowNewLine { get; set; }

        public bool IsTripInfoSection { get; set; }

        public bool IsDataListContentSection { get; set; }

        public int DefaultTitleHeight
        {
            get
            {
                return this.defaultTitleHeight;
            }
            set
            {
                this.defaultTitleHeight = value;
            }
        }

        public float DefaultRowHeight
        {
            get
            {
                return this.defaultRowHeight;
            }
            set
            {
                this.defaultRowHeight = value;
            }
        }

        public int PageCount;

        private List<string[]> data = new List<string[]>();

        private string[] header;

        private string title;

        public void InitializeLayout(float[] columnsWidths)
        {
            this.InitializeLayout(columnsWidths, null);
        }

        public void InitializeLayout(float[] columnsWidths, ContentAlignment[] horizonalTextAlignments)
        {
            if (horizonalTextAlignments != null)
            {
                this.horizonalTextAlignments = horizonalTextAlignments;
            }
            this.columnsWidths = columnsWidths;
            //InitializeWithlabel(columnsWidths);
            int totalHeight = 0;
            if (!string.IsNullOrEmpty(title))
            {
                totalHeight += defaultTitleHeight;
            }
            totalHeight += (int)(this.data.Count * DefaultRowHeight);
            if (this.IsBorderShown)
            {
                totalHeight += 2;
            }
            if (this.IsCellBorderShown)
            {
                totalHeight += (this.data.Count + 1);
            }
            if (this.header != null)
            {
                totalHeight += (int)DefaultRowHeight;
                if (this.IsCellBorderShown)
                {
                    totalHeight += 1;
                }
            }
            if (this.isContentFrameWithPadding)
            {
                totalHeight += ContentFramePadding * 2;
            }
            this.Height = totalHeight;
            this.Margin = new System.Windows.Forms.Padding(0);
            Refresh();
        }

        public SectionControl() : this(true)
        {
        }

        public SectionControl(bool isContentFrameWithPadding)
        {
            InitializeComponent();
            this.IsBorderShown = true;
            this.isContentFrameWithPadding = isContentFrameWithPadding;
        }

        public void PaintByUser()
        {
            PaintEventArgs pe = new PaintEventArgs(this.CreateGraphics(), this.DisplayRectangle);
            OnPaint(pe);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Font cellContentFont = this.IsContentWithSmallFont ? DefaultSmallContentFont : DefaultContentFont;
            Graphics g = pe.Graphics;
            float[] longestStringLengthOfEachColumn = null;
            if (this.data != null && this.data.Count > 0)
            {
                longestStringLengthOfEachColumn = new float[data[0].Length];
            }
            if (longestStringLengthOfEachColumn != null)
            {
                for (int i = 0; i < longestStringLengthOfEachColumn.Length; i++)
                {
                    longestStringLengthOfEachColumn[i] = 0f;
                }
            }
            
            foreach (var item in data)
            {
                for (int i = 0; i < item.Length; i++)
                {
                    if (longestStringLengthOfEachColumn[i] < g.MeasureString(item[i], cellContentFont).Width)
                    {
                        longestStringLengthOfEachColumn[i] = g.MeasureString(item[i], cellContentFont).Width;
                    }
                }
            }
            base.OnPaint(pe);
            this.BackColor = Color.White;
            
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            int rowCount = data.Count;
            int columnCount = this.columnsWidths.Length;
            int initPoint = this.isContentFrameWithPadding ? ContentFramePadding : 0;
            // Draw Section Title
            PointF location = new PointF(initPoint, initPoint);
            if (!string.IsNullOrEmpty(title))
            {
                string[] titleParts = title.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries);
                if (titleParts.Length == 2)
                {
                    g.DrawString(titleParts[0], DefaultTitleFont, DefaultBrush, new RectangleF(new PointF(location.X, location.Y), new SizeF(this.Width * 0.4f, this.DefaultTitleHeight)));
                    g.DrawString(titleParts[1], DefaultContentFont, DefaultBrush, new RectangleF(new PointF(location.X + this.Width / 2 + this.Width / 2 - g.MeasureString(titleParts[1], DefaultContentFont).Width, location.Y), new SizeF(this.Width / 0.6f, this.DefaultTitleHeight)));
                }
                else
                {
                    g.DrawString(this.title, DefaultTitleFont, DefaultBrush, new RectangleF(new PointF(location.X, location.Y), new SizeF(this.Width, this.DefaultTitleHeight)));
                }
                location.Y += this.DefaultTitleHeight;
            }

            float[] absoluteColumnWidths = new float[columnCount];
            for (int i = 0; i < columnCount; i++)
            {
                int totalWidth = this.Width;
                if (this.IsCellBorderShown)
                {
                    totalWidth -= (columnCount + 1);
                }
                absoluteColumnWidths[i] = this.columnsWidths[i] * totalWidth;
            }

            if (header != null)
            {
                Font font = this.IsContentWithSmallFont ? DefaultSmallHeaderFont : DefaultHeaderFont;
                for (int i = 0; i < columnCount; i++)
                {
                    SizeF textSize = g.MeasureString(header[i], font);
                    PointF textPoint = new PointF();
                    textPoint.Y = location.Y + (this.DefaultRowHeight - textSize.Height) / 2;
                    if (this.HorizonalTextAlignment == ContentAlignment.MiddleCenter)
                    {
                        textPoint.X = location.X + (absoluteColumnWidths[i] - textSize.Width) / 2;
                    }
                    else
                    {
                        textPoint.X = location.X;
                    }
                    if (this.horizonalTextAlignments != null)
                    {
                        try
                        {
                            if (horizonalTextAlignments[i] == ContentAlignment.MiddleCenter)
                            {
                                textPoint.X = location.X + (absoluteColumnWidths[i] - textSize.Width) / 2;
                            }
                            else if (horizonalTextAlignments[i] == ContentAlignment.MiddleRight)
                            {
                                textPoint.X = location.X + absoluteColumnWidths[i] - textSize.Width;
                            }
                            else
                            {
                                textPoint.X = location.X;
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    g.DrawString(header[i], font, DefaultBrush, new RectangleF(textPoint, new SizeF(textSize.Width, this.DefaultRowHeight)));
                    location.X += (int)absoluteColumnWidths[i];
                }
                location.X = initPoint;
                location.Y += this.DefaultRowHeight;
            }

            float totalCellHeight = 0;

            for (int i = 0; i < rowCount; i++)
            {
                string[] rowData = data[i];
                if (this.IsCellBorderShown)
                {
                    g.DrawLine(DefaultPen, location, new PointF(location.X + this.Width, location.Y));
                    location.Y++;
                }
             
                for (int j = 0; j < columnCount; j++)
                {
                    if (this.IsCellBorderShown)
                    {
                        location.X++;
                    }
                    SizeF textSize = g.MeasureString(rowData[j], cellContentFont);
                    PointF textPoint = new PointF();
                    if (this.HorizonalTextAlignment == ContentAlignment.MiddleCenter)
                    {
                        textPoint.X = location.X + (absoluteColumnWidths[j] - textSize.Width) / 2;
                    }
                    else
                    {
                        textPoint.X = location.X;
                    }
                    if (this.horizonalTextAlignments != null)
                    {
                        try
                        {
                            if (horizonalTextAlignments[j] == ContentAlignment.MiddleCenter)
                            {
                                textPoint.X = location.X + (absoluteColumnWidths[j] - textSize.Width) / 2;
                            }
                            else if (horizonalTextAlignments[j] == ContentAlignment.MiddleRight)
                            {
                                textPoint.X = location.X + absoluteColumnWidths[j] - textSize.Width - ContentFramePadding;
                            }
                            else
                            {
                                textPoint.X = location.X;
                            }
                        }
                        catch (Exception)
                        {
                        }
                    }
                    if (this.IsFootSection)
                    {
                        if (j == columnCount - 1)
                        {
                            textPoint.X = location.X + absoluteColumnWidths[j] - textSize.Width;
                            if (i == 0)
                            {
                                textPoint.X = textPoint.X + 2;
                            }
                        }
                        
                    }
                    if (this.IsAllowNewLine)
                    {
                        if (textSize.Width > absoluteColumnWidths[j] - 2)
                        {
                            float textHeight = g.MeasureString(rowData[j], cellContentFont, (int)(absoluteColumnWidths[j] - 2)).Height;
                            totalCellHeight = textHeight + ContentFramePadding * 2;
                            textPoint.Y = location.Y + ContentFramePadding;
                            g.DrawString(rowData[j], cellContentFont, DefaultBrush, new RectangleF(textPoint, new SizeF(absoluteColumnWidths[j] - 2, totalCellHeight)));
                            location.X += (int)absoluteColumnWidths[j];
                            if (!this.isHeightReCaculated && !this.IsTripInfoSection)
                            {
                                this.Height += (int) Math.Ceiling(totalCellHeight - this.DefaultRowHeight);
                            }
                            continue;
                        }
                    }
                    textPoint.Y = location.Y + (this.DefaultRowHeight - textSize.Height) / 2;
                    if (this.IsDataListContentSection && rowData.Length == columnCount + 1 && true.ToString().Equals(rowData[columnCount], StringComparison.InvariantCultureIgnoreCase))
                    {
                        textSize = g.MeasureString(rowData[j], MarkedDataItemFont);
                        g.DrawString(rowData[j], MarkedDataItemFont, RedBrush, new RectangleF(textPoint, new SizeF(textSize.Width, this.DefaultRowHeight)));
                    }
                    else
                    {
                        g.DrawString(rowData[j], cellContentFont, DefaultBrush, new RectangleF(textPoint, new SizeF(textSize.Width, this.DefaultRowHeight)));
                    }
                    location.X += (int)absoluteColumnWidths[j];
                }
                location.X = initPoint;
                if (totalCellHeight != 0)
                {
                    location.Y += totalCellHeight;
                }
                else
                {
                    location.Y += this.DefaultRowHeight;
                }
            }

            // Draw border
            if (this.IsCellBorderShown)
            {
                // Draw the last horizonal line
                g.DrawLine(DefaultPen, location, new PointF(location.X + this.Width, location.Y));

                // Draw vertical border
                PointF beginPoint = new PointF(0, 0);
                PointF endPoint = new PointF(0, 0);
                if (this.title != null)
                {
                    beginPoint.Y = defaultTitleHeight;
                    endPoint.Y = DefaultTitleHeight;
                }
                beginPoint.X = 0;
                endPoint.X = 0;
                endPoint.Y = location.Y;
                for (int i = 0; i < columnCount + 1; i++)
                {
                    if (i == columnCount)
                    {
                        beginPoint.X = this.Width - 1;
                        endPoint.X = this.Width - 1;
                    }
                    g.DrawLine(DefaultPen, beginPoint, endPoint);
                    if (i < columnCount - 1)
                    {
                        beginPoint.X += (int)absoluteColumnWidths[i];
                        endPoint.X += (int)absoluteColumnWidths[i];
                    }
                }
            }

            // Draw the section border if IsBorderShown is set as true
            if (this.IsBorderShown)
            {
                int beginY = 0;
                if (this.title != null)
                {
                    beginY += defaultTitleHeight;
                }
                g.DrawLine(DefaultPen, new Point(0, beginY), new Point(this.Width - 1, beginY));
                g.DrawLine(DefaultPen, new Point(0, beginY), new Point(0, this.Height - 1));
                g.DrawLine(DefaultPen, new Point(0, this.Height - 1), new Point(this.Width - 1, this.Height - 1));
                g.DrawLine(DefaultPen, new Point(this.Width - 1, beginY), new Point(this.Width - 1, this.Height - 1));
            }
            this.isHeightReCaculated = true;
        }

        public void ClearRows()
        {
            data.Clear();
        }

        public void AddRow(string[] row)
        {
            data.Add(row);
        }

        public void SetHeader(string[] header)
        {
            this.header = header;
        }

        public void SetSectionTitle(string title)
        {
            this.title = title;
        }


        
    }
}
