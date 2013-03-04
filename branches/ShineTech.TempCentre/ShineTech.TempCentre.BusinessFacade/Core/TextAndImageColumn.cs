using System; 
using System.Collections.Generic;  
using System.Text; 
using System.Windows.Forms; 
using System.Drawing; 
using System.Drawing.Drawing2D;
using ShineTech.TempCentre.Platform;


namespace ShineTech.TempCentre.BusinessFacade
{ 
    public class TextAndImageColumn : DataGridViewTextBoxColumn 
    { 
        private CheckBox imageValue; 
        
        public TextAndImageColumn() 
        { 
            this.CellTemplate = new TextAndImageCell(); 
        }
 
        public override object Clone() 
        { 
            TextAndImageColumn c = base.Clone() as TextAndImageColumn; 
            c.imageValue = this.imageValue;
            return c; 
        }

        public CheckBox CheckText 
        { 
            get { return this.imageValue; }

            set 
            { 
            	 
                if (this.CheckText != value) 
                { 
                    this.imageValue = value;
                } 
            } 
        }
 
        private TextAndImageCell TextAndImageCellTemplate 
        { 
            get { return this.CellTemplate as TextAndImageCell; } 
        }
        public override DataGridViewCell CellTemplate
        {
            get
            {
                return base.CellTemplate;
            }
            set
            {
                if (value != null && !value.GetType().IsAssignableFrom(typeof(TextAndImageCell)))
                {
                    throw new InvalidCastException("����DataGridViewTreeViewCell");
                }
                base.CellTemplate = value;
            }
        }
    }

    public class TextAndImageCell : DataGridViewTextBoxCell 
    { 
        private CheckBox imageValue; 
        
 
        public override object Clone() 
        { 
            TextAndImageCell c = base.Clone() as TextAndImageCell; 
            c.imageValue = this.imageValue; 
            return c; 
        } 
        
        public CheckBox CheckTextbox 
        { 
            get 
            { 
                if (this.OwningColumn == null ||  this.OwningTextAndImageColumn == null) 
                { 
                    return imageValue; 
                }

                else if (this.imageValue != null) 
                { 
                    return this.imageValue; 
                } 
                else 
                { 
                    return this.OwningTextAndImageColumn.CheckText; 
                } 
            } 
            set 
            { 
                if (this.imageValue != value) 
                { 
                    try 
                    { 
                        this.imageValue = value; 
                       
                    }
                    catch (Exception exp)
                    {
                        Utils.ShowMessageBox(exp.Message, Messages.TitleNotification);
                    }
                }
            }
        }

        

        private TextAndImageColumn OwningTextAndImageColumn 
        { 
            get { return this.OwningColumn as TextAndImageColumn; } 
        }

}
} 



/*
System.Drawing.Pen myPen = new System.Drawing.Pen(System.Drawing.Color.Red);//���� System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Red);//��ˢ System.Drawing.Graphics formGraphics = this.CreateGraphics(); 

formGraphics.FillEllipse(myBrush, new Rectangle(0,0,100,200));//��ʵ����Բ 

formGraphics.DrawEllipse(myPen, new Rectangle(0,0,100,200));//����Բ 

formGraphics.FillRectangle(myBrush, new Rectangle(0,0,100,200));//��ʵ�ķ� 

formGraphics.DrawRectangle(myPen, new Rectangle(0,0,100,200));//���ľ��� 

formGraphics.DrawLine(myPen, 0, 0, 200, 200);//���� 

formGraphics.DrawPie(myPen,90,80,140,40,120,100); //���ڱ�ͼ�� //������� 

formGraphics.DrawPolygon(myPen,new Point[]{ new Point(30,140), new Point(270,250), new Point(110,240), new Point (200,170), new Point(70,350), new Point(50,200)}); //����ʹ�õ���Դ myPen.Dispose(); myBrush.Dispose(); formGraphics.Dispose(); 

*/