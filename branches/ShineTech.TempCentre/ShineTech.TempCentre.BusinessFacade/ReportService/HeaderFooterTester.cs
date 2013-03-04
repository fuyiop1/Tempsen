using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using System.Data;

namespace ShineTech.TempCentre.BusinessFacade
{
    class HeaderFooterTester : PdfPageEventHelper
    {
        static void Main(string[] args)        
        {
            DataTable dt = new DataTable();
            for (int i = 0; i < 7; i++)
            {                
                dt.Columns.Add(new DataColumn("Header" + i.ToString())); 
            }            
            Random random = new Random(1000); 
            for (int j = 0; j < 100; j++)        
            {             
                DataRow row = dt.NewRow();   
                row[0] = random.Next();          
                row[1] = random.Next();             
                row[2] = random.Next();          
                row[3] = random.Next();         
                row[4] = random.Next();      
                row[5] = random.Next();        
                row[6] = random.Next();          
                dt.Rows.Add(row);        
            }        
            GeneratePdf("HelloWorld", dt);
        }

        private static void GeneratePdf(string title, DataTable dt)    
        {        
            FileStream fs = new FileStream("HelloWorld.pdf", FileMode.Create);   
            Document doc = new Document(PageSize.A4, 42, 53, 70, 50);     
            PdfWriter writer = PdfWriter.GetInstance(doc, fs);        
            PdfPage pdfPage = new PdfPage();  
            writer.PageEvent = pdfPage;      
            doc.Open();    
            BaseFont baseFont = BaseFont.CreateFont();     
            Font font = new Font(baseFont);        
            //doc.NewPage();        
            Paragraph pTitle = new Paragraph(title);   
            pTitle.Alignment = Element.ALIGN_CENTER;    
            doc.Add(pTitle);        
            doc.Add(new Paragraph(" "));       
            doc.Add(GetPdfDetail(dt, font));   
            doc.Add(new Paragraph(" "));    
            doc.Add(GetPdfDetail(dt, font));     
            doc.Close();
            fs = new FileStream("HelloWorld.pdf", FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
            AddWaterMark(fs, System.Windows.Forms.Application.StartupPath + "\\HelloWorld.png");
        }

        private static PdfPTable GetPdfDetail(DataTable dt, Font font) 
        {        
            PdfPCell cell = null;    
            PdfPTable table = new PdfPTable(dt.Columns.Count); 
            //table.HeaderRows = 1;        换页是否显示标题      
            for (int i = 0; i < dt.Columns.Count; i++)      
            {          
                cell = new PdfPCell(new Phrase(dt.Columns[i].ColumnName));      
                cell.BackgroundColor = new BaseColor(66, 66, 66);         
                cell.Phrase.Font.Color = BaseColor.WHITE;          
                cell.Phrase.Font.Size = 5;          
                table.AddCell(cell);           
            }
            foreach (DataRow row in dt.Rows)       
            {              
                for (int j = 0; j < dt.Columns.Count; j++)    
                {                
                    cell = new PdfPCell(new Phrase(row.ItemArray[j].ToString(), font));     
                    cell.Phrase.Font.Size = 5;           
                    table.AddCell(cell);           
                }      
            }
            return table;    
        }

        private static void AddWaterMark(FileStream fs, string picName)    
        {      
            PdfReader pdfReader = new PdfReader(fs);     
            PdfStamper pdfStamper = new PdfStamper(pdfReader, fs);     
            PdfContentByte waterContent;        
            iTextSharp.text.Image img = iTextSharp.text.Image.GetInstance(picName);    
            img.GrayFill = 20;      
            for (int i = 1; i <= pdfReader.NumberOfPages; i++)    
            {         
                iTextSharp.text.Rectangle pSize = pdfReader.GetPageSize(i);    
                float width = pSize.Width;          
                float height = pSize.Height;  
                img.SetAbsolutePosition(width / 2 - img.Width, height / 2 - img.Height);     
                waterContent = pdfStamper.GetUnderContent(i);      
                waterContent.AddImage(img);
                //PdfPTable head = new PdfPTable(2);    
                //head.TotalWidth = pSize.Width;       
                //// add image; PdfPCell() overload sizes image to fit cell      
                //PdfPCell c = new PdfPCell(new Paragraph("This Page is:"));       
                //c.HorizontalAlignment = Element.ALIGN_RIGHT;        
                //c.Border = Rectangle.NO_BORDER;          
                //head.AddCell(c);
                //// header text       
                //c = new PdfPCell(new Phrase(          
                //  DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss") + " GMT",    
                //  new Font(Font.FontFamily.COURIER, 8)       
                //));            
                //c.Border = Rectangle.NO_BORDER;      
                //c.VerticalAlignment = Element.ALIGN_BOTTOM;   
                //head.AddCell(c);      
                ///*            
                /////* write header w/WriteSelectedRows(); requires absolute positions!    
                //*/              
                //head.WriteSelectedRows(     
                //  0, -1,       
                // first/last row; -1 flags all write all rows      
                //  0, // left offset        
                //
                // ** bottom** yPos of the table       
                //  pSize.Height - doc.TopMargin + head.TotalHeight,           
                //  writer.DirectContent     
                //);            }
                pdfStamper.Close();     
                pdfReader.Close();    
            }    
        }
    }
}


//namespace MakePdf{    class Program : PdfPageEventHelper    {        } }

//页眉页脚：

namespace ShineTech.TempCentre.BusinessFacade
{  
    public class PdfPage : iTextSharp.text.pdf.PdfPageEventHelper 
    {  
        //I create a font object to use within my footer
        protected Font footer  
        {   
            get        
            {         
                // create a basecolor to use for the footer font, if needed.    
                BaseColor grey = new BaseColor(128, 128, 128);      
                Font font = FontFactory.GetFont("Arial", 9, Font.NORMAL, grey);     
                return font;       
            }     
        }      
        //override the OnStartPage event handler to add our header    
        public override void OnStartPage(PdfWriter writer, Document doc)  
        {       
            //I use a PdfPtable with 1 column to position my header where I want it   
            PdfPTable headerTbl = new PdfPTable(1);
            //set the width of the table to be the same as the document     
            headerTbl.TotalWidth = doc.PageSize.Width;
            //I use an image logo in the header so I need to get an instance of the image to be able to insert it. I believe this is something you couldn't do with older versions of iTextSharp    
            Image logo = Image.GetInstance(System.Windows.Forms.Application.StartupPath + "\\HelloWorld.png");
            //I used a large version of the logo to maintain the quality when the size was reduced. I guess you could reduce the size manually and use a smaller version, but I used iTextSharp to reduce the scale. As you can see, I reduced it down to 7% of original size.    
            logo.ScalePercent(70);
            //create instance of a table cell to contain the logo    
            PdfPCell cell = new PdfPCell(logo);
            //align the logo to the right of the cell   
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;
            //add a bit of padding to bring it away from the right edge  
            cell.PaddingRight = 20;
            //remove the border        
            cell.Border = 0;
            //Add the cell to the table    
            headerTbl.AddCell(cell);
            //write the rows out to the PDF output stream. I use the height of the document to position the table. Positioning seems quite strange in iTextSharp and caused me the biggest headache.. It almost seems like it starts from the bottom of the page and works up to the top, so you may ned to play around with this.       
            headerTbl.WriteSelectedRows(0, -1, 0, (doc.PageSize.Height - 10), writer.DirectContent);    
        }

        //override the OnPageEnd event handler to add our footer  
        public override void OnEndPage(PdfWriter writer, Document doc) 
        {         
            //I use a PdfPtable with 2 columns to position my footer where I want it          
            PdfPTable footerTbl = new PdfPTable(2);
            //set the width of the table to be the same as the document 
            footerTbl.TotalWidth = doc.PageSize.Width;
            //Center the table on the page      
            footerTbl.HorizontalAlignment = Element.ALIGN_CENTER;
            //Create a paragraph that contains the footer text    
            Paragraph para = new Paragraph("Some footer text", footer);
            //add a carriage return       
            para.Add(Environment.NewLine);      
            para.Add("Some more footer text");
            //create a cell instance to hold the text    
            PdfPCell cell = new PdfPCell(para);
            //set cell border to 0      
            cell.Border = 0;
            //add some padding to bring away from the edge    
            cell.PaddingLeft = 10;  
            cell.PaddingTop = 10;
            //add cell to table    
            footerTbl.AddCell(cell);
            //create new instance of Paragraph for 2nd cell text  
            para = new Paragraph("Some text for the second cell", footer);
            //create new instance of cell to hold the text     
            cell = new PdfPCell(para);
            //align the text to the right of the cell      
            cell.HorizontalAlignment = Element.ALIGN_RIGHT;     
            //set border to 0       
            cell.Border = 0;
            // add some padding to take away from the edge of the page    
            cell.PaddingRight = 10;         
            cell.PaddingTop = 10;
            //add the cell to the table     
            footerTbl.AddCell(cell);
            //write the rows out to the PDF output stream.   
            footerTbl.WriteSelectedRows(0, -1, 0, (doc.BottomMargin + 10), writer.DirectContent);    
        }   
    }
}


