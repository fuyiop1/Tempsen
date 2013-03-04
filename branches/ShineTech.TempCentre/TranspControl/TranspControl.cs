using System;
using System.Collections;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace TranspControl
{
    [Designer(typeof(TranspControlDesigner))]
    public class TranspControl : Panel
    {
        public bool drag = false;
        public bool enab = false;
        private Color fillColor = Color.White;
        private int opacity = 100;
        private int alpha;

        // Fields
        private System.Drawing.Color _BackColour1 = System.Drawing.SystemColors.Window;
        private System.Drawing.Color _BackColour2 = System.Drawing.SystemColors.Window;
        private LinearGradientMode _GradientMode = LinearGradientMode.None;
        private System.Windows.Forms.BorderStyle _BorderStyle = System.Windows.Forms.BorderStyle.None;
        private System.Drawing.Color _BorderColour = System.Drawing.SystemColors.WindowFrame;
        private int _BorderWidth = 1;
        private int _Curvature = 0;
        // Properties
        //   Shadow the Backcolor property so that the base class will still render with a transparent backcolor
        private CornerCurveMode _CurveMode = CornerCurveMode.All;

        [System.ComponentModel.DefaultValueAttribute(typeof(System.Drawing.Color), "Window"), System.ComponentModel.CategoryAttribute("Appearance"), System.ComponentModel.DescriptionAttribute("The primary background color used to display text and graphics in the control.")]
        public new System.Drawing.Color BackColor
        {
            get
            {
                return this._BackColour1;
            }
            set
            {
                this._BackColour1 = value;
                if (this.DesignMode == true)
                {
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.DefaultValueAttribute(typeof(System.Drawing.Color), "Window"), System.ComponentModel.CategoryAttribute("Appearance"), System.ComponentModel.DescriptionAttribute("The secondary background color used to paint the control.")]
        public System.Drawing.Color BackColor2
        {
            get
            {
                return this._BackColour2;
            }
            set
            {
                this._BackColour2 = value;
                if (this.DesignMode == true)
                {
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.DefaultValueAttribute(typeof(LinearGradientMode), "None"), System.ComponentModel.CategoryAttribute("Appearance"), System.ComponentModel.DescriptionAttribute("The gradient direction used to paint the control.")]
        public LinearGradientMode GradientMode
        {
            get
            {
                return this._GradientMode;
            }
            set
            {
                this._GradientMode = value;
                if (this.DesignMode == true)
                {
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.DefaultValueAttribute(typeof(System.Windows.Forms.BorderStyle), "None"), System.ComponentModel.CategoryAttribute("Appearance"), System.ComponentModel.DescriptionAttribute("The border style used to paint the control.")]
        public new System.Windows.Forms.BorderStyle BorderStyle
        {
            get
            {
                return this._BorderStyle;
            }
            set
            {
                this._BorderStyle = value;
                if (this.DesignMode == true)
                {
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.DefaultValueAttribute(typeof(System.Drawing.Color), "WindowFrame"), System.ComponentModel.CategoryAttribute("Appearance"), System.ComponentModel.DescriptionAttribute("The border color used to paint the control.")]
        public System.Drawing.Color BorderColor
        {
            get
            {
                return this._BorderColour;
            }
            set
            {
                this._BorderColour = value;
                if (this.DesignMode == true)
                {
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.DefaultValueAttribute(typeof(int), "1"), System.ComponentModel.CategoryAttribute("Appearance"), System.ComponentModel.DescriptionAttribute("The width of the border used to paint the control.")]
        public int BorderWidth
        {
            get
            {
                return this._BorderWidth;
            }
            set
            {
                this._BorderWidth = value;
                if (this.DesignMode == true)
                {
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.DefaultValueAttribute(typeof(int), "0"), System.ComponentModel.CategoryAttribute("Appearance"), System.ComponentModel.DescriptionAttribute("The radius of the curve used to paint the corners of the control.")]
        public int Curvature
        {
            get
            {
                return this._Curvature;
            }
            set
            {
                this._Curvature = value;
                if (this.DesignMode == true)
                {
                    this.Invalidate();
                }
            }
        }

        [System.ComponentModel.DefaultValueAttribute(typeof(CornerCurveMode), "All"), System.ComponentModel.CategoryAttribute("Appearance"), System.ComponentModel.DescriptionAttribute("The style of the curves to be drawn on the control.")]
        public CornerCurveMode CurveMode
        {
            get
            {
                return this._CurveMode;
            }
            set
            {
                this._CurveMode = value;
                if (this.DesignMode == true)
                {
                    this.Invalidate();
                }
            }
        }

        private int adjustedCurve
        {
            get
            {
                int curve = 0;
                if (!(this._CurveMode == CornerCurveMode.None))
                {
                    if (this._Curvature > (this.ClientRectangle.Width / 2))
                    {
                        curve = DoubleToInt(this.ClientRectangle.Width / 2);
                    }
                    else
                    {
                        curve = this._Curvature;
                    }
                    if (curve > (this.ClientRectangle.Height / 2))
                    {
                        curve = DoubleToInt(this.ClientRectangle.Height / 2);
                    }
                }
                return curve;
            }
        }


        public TranspControl()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            SetStyle(ControlStyles.Opaque, true);
            this.BackColor = Color.Transparent;
        }

        public Color FillColor
        {
            get
            {
                return this.fillColor;
            }
            set
            {
                this.fillColor = value;
                if (this.Parent != null) Parent.Invalidate(this.Bounds, true);
            }
        }

        public int Opacity
        {
            get
            {
                if (opacity > 100) { opacity = 100; }
                else if (opacity < 1) { opacity = 1; }
                return this.opacity;
            }
            set
            {
                this.opacity = value;
                if (this.Parent != null) Parent.Invalidate(this.Bounds, true);
            }
        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x20;
                return cp;
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle bounds = new Rectangle(0, 0, this.Width - 1, this.Height - 1);

            Color frmColor = this.Parent.BackColor;
            Brush brushColor;
            Brush bckColor;

            alpha = (opacity * 255) / 100;

            if (drag)
            {
                Color dragFillColor;
                Color dragBckColor;

                if (BackColor != Color.Transparent)
                {
                    int Rb = BackColor.R * alpha / 255 + frmColor.R * (255 - alpha) / 255;
                    int Gb = BackColor.G * alpha / 255 + frmColor.G * (255 - alpha) / 255;
                    int Bb = BackColor.B * alpha / 255 + frmColor.B * (255 - alpha) / 255;
                    dragBckColor = Color.FromArgb(Rb, Gb, Bb);
                }
                else dragBckColor = frmColor;

                if (fillColor != Color.Transparent)
                {
                    int Rf = fillColor.R * alpha / 255 + frmColor.R * (255 - alpha) / 255;
                    int Gf = fillColor.G * alpha / 255 + frmColor.G * (255 - alpha) / 255;
                    int Bf = fillColor.B * alpha / 255 + frmColor.B * (255 - alpha) / 255;
                    dragFillColor = Color.FromArgb(Rf, Gf, Bf);
                }
                else dragFillColor = dragBckColor;

                alpha = 255;
                brushColor = new SolidBrush(Color.FromArgb(alpha, dragFillColor));
                bckColor = new SolidBrush(Color.FromArgb(alpha, dragBckColor));
            }
            else
            {
                Color color = fillColor;
                brushColor = new SolidBrush(Color.FromArgb(alpha, color));
                bckColor = new SolidBrush(Color.FromArgb(alpha, this.BackColor));
            }

            Pen pen = new Pen(this.ForeColor);

            System.Drawing.Drawing2D.GraphicsPath graphPath;
            graphPath = this.GetPath();

            if (this.BackColor != Color.Transparent | drag)
            {
                g.FillPath(bckColor, graphPath);
            }

            if (FillColor != Color.Transparent | drag)
            {
                g.FillPath(brushColor, graphPath);
            }
            else
            {
                g.FillPath(new SolidBrush(Color.FromArgb(1, Color.White)), graphPath);
            }


            //g.FillPath(brushColor, graphPath);

            pen.Dispose();
            brushColor.Dispose();
            bckColor.Dispose();
            g.Dispose();
            base.OnPaint(e);
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            if (this.Parent != null) Parent.Invalidate(this.Bounds, true);
            base.OnBackColorChanged(e);
        }

        protected override void OnParentBackColorChanged(EventArgs e)
        {
            this.Invalidate();
            base.OnParentBackColorChanged(e);
        }

        private System.Drawing.Drawing2D.GraphicsPath GetPath()
        {
            System.Drawing.Drawing2D.GraphicsPath graphPath = new System.Drawing.Drawing2D.GraphicsPath();
            if (this._BorderStyle == System.Windows.Forms.BorderStyle.Fixed3D)
            {
                graphPath.AddRectangle(this.ClientRectangle);
            }
            else
            {
                try
                {
                    int curve = 0;
                    System.Drawing.Rectangle rect = this.ClientRectangle;
                    int offset = 0;
                    if (this._BorderStyle == System.Windows.Forms.BorderStyle.FixedSingle)
                    {
                        if (this._BorderWidth > 1)
                        {
                            offset = DoubleToInt(this.BorderWidth / 2);
                        }
                        curve = this.adjustedCurve;
                    }
                    else if (this._BorderStyle == System.Windows.Forms.BorderStyle.Fixed3D)
                    {
                    }
                    else if (this._BorderStyle == System.Windows.Forms.BorderStyle.None)
                    {
                        curve = this.adjustedCurve;
                    }
                    if (curve == 0)
                    {
                        graphPath.AddRectangle(System.Drawing.Rectangle.Inflate(rect, -offset, -offset));
                    }
                    else
                    {
                        int rectWidth = rect.Width - 1 - offset;
                        int rectHeight = rect.Height - 1 - offset;
                        int curveWidth = 1;
                        if ((this._CurveMode & CornerCurveMode.TopRight) != 0)
                        {
                            curveWidth = (curve * 2);
                        }
                        else
                        {
                            curveWidth = 1;
                        }
                        graphPath.AddArc(rectWidth - curveWidth, offset, curveWidth, curveWidth, 270, 90);
                        if ((this._CurveMode & CornerCurveMode.BottomRight) != 0)
                        {
                            curveWidth = (curve * 2);
                        }
                        else
                        {
                            curveWidth = 1;
                        }
                        graphPath.AddArc(rectWidth - curveWidth, rectHeight - curveWidth, curveWidth, curveWidth, 0, 90);
                        if ((this._CurveMode & CornerCurveMode.BottomLeft) != 0)
                        {
                            curveWidth = (curve * 2);
                        }
                        else
                        {
                            curveWidth = 1;
                        }
                        graphPath.AddArc(offset, rectHeight - curveWidth, curveWidth, curveWidth, 90, 90);
                        if ((this._CurveMode & CornerCurveMode.TopLeft) != 0)
                        {
                            curveWidth = (curve * 2);
                        }
                        else
                        {
                            curveWidth = 1;
                        }
                        graphPath.AddArc(offset, offset, curveWidth, curveWidth, 180, 90);
                        graphPath.CloseFigure();
                    }
                }
                catch (System.Exception)
                {
                    graphPath.AddRectangle(this.ClientRectangle);
                }
            }
            return graphPath;
        }

        public static int DoubleToInt(double value)
        {
            return System.Decimal.ToInt32(System.Decimal.Floor(System.Decimal.Parse((value).ToString())));
        }
    }

    internal class TranspControlDesigner : ControlDesigner
    {
        private TranspControl myControl;

        protected override void OnMouseDragMove(int x, int y)
        {
            myControl = (TranspControl)(this.Control);
            myControl.drag = true;
            base.OnMouseDragMove(x, y);
        }

        protected override void OnMouseLeave()
        {
            myControl = (TranspControl)(this.Control);
            myControl.drag = false;
            base.OnMouseLeave();
        }
    }

    [FlagsAttribute()]
    //圆角位置
    public enum CornerCurveMode
    {

        None = 0,
        TopLeft = 1,
        TopRight = 2,
        TopLeft_TopRight = 3,
        BottomLeft = 4,
        TopLeft_BottomLeft = 5,
        TopRight_BottomLeft = 6,
        TopLeft_TopRight_BottomLeft = 7,
        BottomRight = 8,
        BottomRight_TopLeft = 9,
        BottomRight_TopRight = 10,
        BottomRight_TopLeft_TopRight = 11,
        BottomRight_BottomLeft = 12,
        BottomRight_TopLeft_BottomLeft = 13,
        BottomRight_TopRight_BottomLeft = 14,
        All = 15
    }
    //渐变
    public enum LinearGradientMode
    {
        Horizontal = 0,
        Vertical = 1,
        ForwardDiagonal = 2,
        BackwardDiagonal = 3,
        None = 4
    }

}
