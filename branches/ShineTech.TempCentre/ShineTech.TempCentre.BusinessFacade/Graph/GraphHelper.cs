using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZedGraph;
using System.Drawing;
using ShineTech.TempCentre.DAL;
using System.Configuration;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Globalization;

namespace ShineTech.TempCentre.BusinessFacade
{
    public class GraphHelper
    {
        public static int _YAxisLength;
        public static int YAxisLength
        {
            get
            {
                if (_YAxisLength == 0)
                {
                    object o = ConfigurationManager.AppSettings["YAxisLength"];
                    _YAxisLength = o == null ? 10 : Convert.ToInt32(o);
                }
                return _YAxisLength;
            }
        }
        public static List<Color> _LineColor;
        private static RectangleF rec;
        private static PointF _ChartPos;
        public static List<Color> LineColor
        {
            get 
            {
                if (_LineColor == null)
                    _LineColor = new List<Color>();
                if (_LineColor.Count == 0)
                {
                    _LineColor.Add(Color.Red);
                    _LineColor.Add(Color.RoyalBlue);
                    _LineColor.Add(Color.Brown);
                    _LineColor.Add(Color.Green);
                    _LineColor.Add(Color.Purple);
                    _LineColor.Add(Color.Orange);
                    _LineColor.Add(Color.Sienna);
                    _LineColor.Add(Color.Violet);
                    _LineColor.Add(Color.LightSalmon);
                    _LineColor.Add(Color.Coral);
                    _LineColor.Add(Color.Black);
                }
                return GraphHelper._LineColor; 
            }
            set { GraphHelper._LineColor = value; }
        }
        private static string _minLimit;

        public static string MinLimit
        {
            get { return _minLimit; }
            set { _minLimit = value; }
        }
        private static string _maxLimit;

        public static string MaxLimit
        {
            get { return _maxLimit; }
            set { _maxLimit = value; }
        }
        public GraphHelper()
        {
        }
        public static GraphHelper CreateIntance()
        {
            return new GraphHelper();
        }
        public static Color GetColorFromProfile(string rgb)
        {
            List<int> list = rgb.Split(new char[] { ',' }).ToList().Select(p => Convert.ToInt32(p)).ToList();
            return Color.FromArgb(list[0], list[1], list[2]);
        }
        public static void SetGraphAsDefaultProperity(ZedGraphControl zgc, XAxisVisibility selection)
        {
            if (zgc != null)
            {
                GraphPane pane = zgc.GraphPane;
                pane.CurveList.Clear();
                pane.GraphObjList.Clear();
                LineBase.Default.Width = 1.5f;
                pane.Title.Text = string.Empty;
                switch (selection)
                {
                    case XAxisVisibility.DateAndTime:
                        pane.XAxis.Title.Text = "Date/Time";
                        pane.XAxis.Type = AxisType.Text;
                        break;
                    case XAxisVisibility.DataPoints:
                        pane.XAxis.Title.Text = "Data Points";
                        pane.XAxis.Type = AxisType.Text;
                        break;
                    default:
                        pane.XAxis.Title.Text = "Elapsed Time";
                        pane.XAxis.Type = AxisType.Text;
                        break;
                }
                pane.YAxis.Title.Text = "Temperature(°C)";
                pane.XAxis.Scale.IsVisible = true;
                pane.YAxis.Scale.IsVisible = true;
                pane.IsFontsScaled = false;
                pane.XAxis.MajorGrid.IsVisible = true;
                pane.XAxis.MajorGrid.DashOff = 2.5f;
                pane.XAxis.MajorGrid.DashOn = 5f;
                pane.XAxis.MajorGrid.Color = Color.Gray;
                pane.YAxis.MajorGrid.IsVisible = true;
                pane.YAxis.MajorGrid.Color = Color.Gray;
                pane.YAxis.MajorGrid.DashOff = 2.5f;
                pane.YAxis.MajorGrid.DashOn = 5f;
                pane.YAxis.MajorGrid.IsZeroLine = false;
                pane.XAxis.MajorGrid.IsZeroLine = false;
                pane.XAxis.MajorTic.IsInside = true;
                pane.XAxis.MajorTic.IsOutside = false;
                pane.YAxis.MinorTic.IsInside = true;
                pane.YAxis.MinorTic.IsOutside = false;
                pane.YAxis.MajorTic.IsInside = true;
                pane.YAxis.MajorTic.IsOutside = false;
                pane.XAxis.Scale.FontSpec.Size = 11.0f;
                //pane.XAxis.Scale.FontSpec.IsAntiAlias = true;
                pane.XAxis.Scale.FontSpec.StringAlignment = StringAlignment.Near;
                pane.XAxis.Scale.FontSpec.Family = "Arial";
                pane.XAxis.Scale.FontSpec.Size = 11.0f;
                //pane.XAxis.Scale.FontSpec.IsAntiAlias = true;
                pane.YAxis.Scale.FontSpec.StringAlignment = StringAlignment.Near;
                pane.YAxis.Scale.FontSpec.Family = "Arial";
                pane.YAxis.Scale.FontSpec.Size = 11.0f;
                pane.Legend.Position = LegendPos.Bottom;
                pane.Legend.Border.IsVisible = false;
                
                zgc.IsShowPointValues = true;//显示悬停值
                zgc.IsShowContextMenu = false;//去掉右键信息
                zgc.IsEnableWheelZoom = false;
                zgc.IsEnableHPan = false;
                zgc.IsEnableVPan = false;//禁止上下左右拖拽
                zgc.IsEnableHZoom = true;//允许水平放大
                zgc.IsEnableVZoom = false;//禁止垂直放大
                zgc.GraphPane.Border = new Border(false, Color.White, .0f);//去掉pane的border
                zgc.IsPointsZoom = true;
                //zgc.MasterPane.BaseDimension = 0.0f;
                //pane.XAxis.Scale.MajorUnit = DateUnit.Second;
                pane.XAxis.Scale.FontSpec.Fill.IsScaled = false;
                pane.YAxis.Scale.FontSpec.Fill.IsScaled = false;
                pane.YAxis.Scale.Format = "F1";
                YAxis.Default.IsZeroLine = false;
                //pane.XAxis.CrossAuto = false;
                SetRecOfChart(zgc,pane);
            }
        }
        public static void SetInitProperity(ZedGraphControl zgc)
        {
            if (zgc != null)
            {
                GraphPane pane = zgc.GraphPane;
                pane.CurveList.Clear();
                pane.GraphObjList.Clear();

                pane.YAxis.Title.Text = "Temperature(°C)";
                LineBase.Default.Width = 1.5f;
               
                pane.IsFontsScaled = false;
                pane.XAxis.MajorGrid.IsVisible = true;
                pane.XAxis.MajorGrid.DashOff = 2.5f;
                pane.XAxis.MajorGrid.DashOn = 5f;
                pane.XAxis.MajorGrid.Color = Color.Gray;
                pane.YAxis.MajorGrid.IsVisible = true;
                pane.YAxis.MajorGrid.Color = Color.Gray;
                pane.YAxis.MajorGrid.DashOff = 2.5f;
                pane.YAxis.MajorGrid.DashOn = 5f;
                pane.YAxis.MajorGrid.IsZeroLine = false;
                pane.XAxis.MajorGrid.IsZeroLine = false;
                pane.XAxis.MajorTic.IsInside = true;
                pane.XAxis.MajorTic.IsOutside = false;
                pane.XAxis.MinorTic.IsInside = true;
                pane.XAxis.MinorTic.IsOutside = false;
                pane.YAxis.MinorTic.IsInside = true;
                pane.YAxis.MinorTic.IsOutside = false;
                pane.YAxis.MajorTic.IsInside = true;
                pane.YAxis.MajorTic.IsOutside = false;
                pane.XAxis.Scale.FontSpec.Size = 11.0f;
                //pane.XAxis.Scale.FontSpec.IsAntiAlias = true;
                pane.XAxis.Scale.FontSpec.StringAlignment = StringAlignment.Near;
                pane.XAxis.Scale.FontSpec.Family = "Arial";
                pane.XAxis.Scale.FontSpec.Size = 11.0f;
                pane.YAxis.Scale.FontSpec.StringAlignment = StringAlignment.Near;
                pane.YAxis.Scale.FontSpec.Family = "Arial";
                pane.YAxis.Scale.FontSpec.Size = 11.0f;
                pane.Legend.Position = LegendPos.Bottom;
                pane.Legend.Border.IsVisible = false;
                pane.Legend.FontSpec.Size = 9.5f;
                zgc.IsShowPointValues = true;//显示悬停值
                zgc.IsShowContextMenu = false;//去掉右键信息
                zgc.IsEnableWheelZoom = false;
                
                zgc.IsEnableHPan = false;
                zgc.IsEnableVPan = false;//禁止上下左右拖拽
                zgc.IsEnableHZoom = true;//允许水平放大
                zgc.IsEnableVZoom = false;//禁止垂直放大
                zgc.GraphPane.Border = new Border(false, Color.White, .0f);//去掉pane的border
                zgc.IsPointsZoom = true;
                pane.XAxis.Scale.FontSpec.Fill.IsScaled = false;
                pane.YAxis.Scale.FontSpec.Fill.IsScaled = false;
                pane.YAxis.Scale.Format = "F1";
                YAxis.Default.IsZeroLine = false;
                pane.Title.Text = string.Empty;
                SetRecOfChart(zgc, pane);
                pane.XAxis.Title.Text = string.Empty;
                pane.YAxis.Title.Text = string.Empty;
                pane.XAxis.Scale.IsVisible = false;
                pane.YAxis.Scale.IsVisible = false;
                pane.XAxis.Scale.IsUseTenPower = false;
                pane.XAxis.Title.IsOmitMag = true;
            }
        }
        public static  void SetRecOfChart(ZedGraphControl zgc,GraphPane pane)
        {
            rec = pane.CalcChartRect(zgc.CreateGraphics());
            if (rec != RectangleF.Empty)
            {
                if (_ChartPos == PointF.Empty)
                    _ChartPos = new PointF(rec.X, rec.Y + 20);
                rec = new RectangleF(new PointF(_ChartPos.X, _ChartPos.Y), new SizeF(zgc.Width - 110, zgc.Height - 135));
                pane.Chart.Rect = rec;
            }
        }
        /// <summary>
        /// 整个系统中图设置的source
        /// </summary>
        /// <param name="zgc">画布</param>
        /// <param name="list">温度点</param>
        /// <param name="label">曲线名</param>
        /// <param name="selection">横轴类型</param>
        /// <param name="tempUnit">温度单位</param>
        /// <param name="i">第几条曲线</param>
        /// <param name="isCompare">compare状态</param>
        public static void SetGraphDataSource(ZedGraphControl zgc,SuperDevice tag,XAxisVisibility selection,bool isMarked)
        {
            if (tag != null)
            {
                List<PointKeyValue> list = Common.GetTempPointsLocalTime(tag.tempList);
                if (list != null && list.Count > 0)
                {
                    string tempUnit=tag.TempUnit;
                    string label=string.Format("{0}_{1}",tag.SerialNumber,tag.TripNumber);
                    double interval = Convert.ToDouble(tag.LogInterval);
                    switch (selection)
                    {
                        case XAxisVisibility.DateAndTime:
                            SetXAxisAsDateTime(zgc,interval, list, label, XAxisVisibility.DateAndTime, isMarked);
                            break;
                        case XAxisVisibility.DataPoints:
                            SetXAxisAsDataPoints(zgc, interval, list, label, XAxisVisibility.DataPoints, isMarked);
                            break;
                        default:
                            SetXAxisAsElapsedTime(zgc, interval, list, label, XAxisVisibility.ElapsedTime, isMarked);
                            break;
                    }
                    zgc.GraphPane.YAxis.Title.Text = string.Format("Temperature(°{0})", tempUnit);
                    zgc.AxisChange();
                    zgc.Refresh();
                }
            }
        }
        public static void SetGraphDataSource(ZedGraphControl zgc, List<SuperDevice> TagList, XAxisVisibility selection, string tempUnit,bool isMarked)
        {
            if (TagList != null && TagList.Count > 0)
            {
                switch (selection)
                {
                    case XAxisVisibility.DateAndTime:
                        SetXAxisAsDateTime(zgc.GraphPane, TagList, isMarked);
                        break;
                    case XAxisVisibility.DataPoints:
                        SetXAxisAsDataPoints(zgc.GraphPane, TagList, isMarked);
                        break;
                    default:
                        SetXAxisAsElapsedTime(zgc.GraphPane, TagList, isMarked);
                        break;
                }
                zgc.GraphPane.YAxis.Title.Text = "Temperature(°" + tempUnit + ")";
                zgc.AxisChange();
                zgc.Refresh();
            }
            else
            {
                SetInitProperity(zgc);
            }
        }
        public static void SetXAxisAsDateTime(ZedGraphControl zgc,double interval, List<PointKeyValue> list, string label, XAxisVisibility selection,bool isMarked)
        {
            GraphPane pane = zgc.GraphPane;
            PointPairList pair = new PointPairList();
            int j = 1;
            list.ToList().ForEach(v =>
            {
                pair.Add(j, Convert.ToDouble(v.PointTemp));
                j++;
            });
            pane.CurveList.Clear();
            AddSingleLineToCanvas(pane, pair, list, label, selection, 1, false,isMarked);
            pane.XAxis.Scale.TextLabels = SetTextLabels(list,interval, selection, 1, list.Count, 10);
        }
        private static List<string> GetDateTimeTextFormat(List<DateTime> list)
        {
            List<string> s = list.Select(v =>
            {
                string dt = v.ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture);
                string[] split = dt.Split(new char[] { ' ' });
                dt = split[0] + Environment.NewLine;
                for (int i = 1; i < split.Length; i++)
                {
                    dt = dt + split[i]+" ";
                }
                return dt.TrimEnd();
            }).ToList();
            return s;
        }
        public static void SetXAxisAsDataPoints(ZedGraphControl zgc, double interval, List<PointKeyValue> list, string label, XAxisVisibility selection, bool isMarked)
        {
            GraphPane pane = zgc.GraphPane;
            List<string> xAxis = new List<string>();
            PointPairList pair = new PointPairList();
            int j = 1;
            list.ToList().ForEach(v =>
            {
                xAxis.Add(j.ToString());
                pair.Add(j, Convert.ToDouble(v.PointTemp));
                j++;
            });
            pane.CurveList.Clear();
            AddSingleLineToCanvas(pane, pair, list, label, selection, 1, false ,isMarked);
            pane.XAxis.Scale.TextLabels = SetTextLabels(list,interval, selection, 1, list.Count, 10);
            pane.XAxis.Scale.FontSpec.Size = 11.0f;
            if (GraphHelper.rec != RectangleF.Empty)
            {
                pane.Chart.Rect = rec;
            }
        }
        public static void SetXAxisAsElapsedTime(ZedGraphControl zgc, double interval, List<PointKeyValue> list, string label, XAxisVisibility selection, bool isMarked)
        {
            GraphPane pane = zgc.GraphPane;
            List<string> xAxis = new List<string>();
            DateTime start=DateTime.Now;
            PointPairList pair = new PointPairList();
            for (int i = 0; i < list.Count; i++)
            {
                pair.Add(i+1, list[i].PointTemp);
            }
            pane.CurveList.Clear();
            AddSingleLineToCanvas(pane, pair, list, label, selection, 1, false,isMarked);
            pane.XAxis.Scale.TextLabels = SetTextLabels(list,interval, selection, 1, list.Count, 10);
            pane.XAxis.Scale.FontSpec.Size = 11.0f;
            if (GraphHelper.rec != RectangleF.Empty)
            {
                pane.Chart.Rect = rec;
            }
        }
        public static void SetXAxisTextLabelByComparing(ZedGraphControl zgc,XAxisVisibility selection,List<SuperDevice> taglist)
        {
            if (taglist.Count == 0)
                return;
            GraphPane pane = zgc.GraphPane;
             List<SuperDevice> list= taglist.OrderBy(p => p.tempList.Count).ToList();
             if (list.Count > 0)
             {
                 List<CurveItem> items = pane.CurveList.OrderBy(p => p.Points.Count).ToList();
                 if (items.Count > 0)
                 {
                     int xMax = items.Last().Points.Count;
                     SuperDevice Tag = list.Last();
                     List<PointKeyValue> points = new List<PointKeyValue>();
                     string firstlabel, lastlabel;
                     firstlabel = pane.XAxis.Scale.TextLabels.First();
                     lastlabel = pane.XAxis.Scale.TextLabels.Last();
                     //select the point pos
                     try
                     {
                         switch (selection)
                         {
                             case XAxisVisibility.DataPoints:
                                 DateTime start, end;
                                 int begin, last;
                                 if (DateTime.TryParse(firstlabel, out start) && DateTime.TryParse(lastlabel, out end))
                                 {
                                     points = Tag.tempList.Where(p => p.PointTime >= start && p.PointTime <= end).ToList();
                                 }
                                 else
                                 {
                                     begin = Convert.ToInt32(TempsenFormatHelper.GetSecondsFromFormatString(firstlabel)) / Convert.ToInt32(Tag.LogInterval);
                                     last = Convert.ToInt32(TempsenFormatHelper.GetSecondsFromFormatString(lastlabel)) / Convert.ToInt32(Tag.LogInterval);
                                     for (int i = begin; i <= last; i++)
                                     {
                                         if (i < Tag.tempList.Count)
                                             points.Add(Tag.tempList[i]);
                                     }
                                 }
                                 break;
                             case XAxisVisibility.DateAndTime:
                                 if (int.TryParse(firstlabel, out begin) && int.TryParse(lastlabel, out last))
                                 {
                                     for (int i = begin-1; i < last; i++)
                                     {
                                         if (i < Tag.tempList.Count)
                                             points.Add(Tag.tempList[i]);
                                     }
                                 }
                                 else
                                 {
                                     begin = Convert.ToInt32(TempsenFormatHelper.GetSecondsFromFormatString(firstlabel)) / Convert.ToInt32(Tag.LogInterval);
                                     last = Convert.ToInt32(TempsenFormatHelper.GetSecondsFromFormatString(lastlabel)) / Convert.ToInt32(Tag.LogInterval);
                                     for (int i = begin; i <= last; i++)
                                     {
                                         if (i < Tag.tempList.Count)
                                             points.Add(Tag.tempList[i]);
                                         else
                                         {
                                             PointKeyValue pkv = new PointKeyValue();
                                             pkv.PointTemp = 0;
                                             pkv.PointTime = Tag.tempList.Last().PointTime.AddSeconds(Convert.ToDouble(Tag.LogInterval));
                                             Tag.tempList.Add(pkv);
                                         }
                                     }
                                 }
                                 break;
                             default:
                                 //DateTime start,end;
                                 if (DateTime.TryParse(firstlabel, out start) && DateTime.TryParse(lastlabel, out end))
                                 {
                                     points = Tag.tempList.Where(p => p.PointTime >= start && p.PointTime <= end).ToList();
                                 }
                                 else
                                 {
                                     begin = Convert.ToInt32(firstlabel);
                                     last = Convert.ToInt32(lastlabel);
                                     for (int i = begin-1; i < last; i++)
                                     {
                                         if (i < Tag.tempList.Count)
                                             points.Add(Tag.tempList[i]);
                                     }
                                 }
                                 break;
                         }
                         pane.XAxis.Scale.TextLabels = SetTextLabels(Tag,points, selection, 1, points.Count, 10);
                     }
                     catch
                     {
                     }
                 }
             }
        }
        private static void AddLineToCanvas(GraphPane pane, PointPairList pair, List<PointKeyValue> list, string label, XAxisVisibility selection, int index, bool isCompare)
        {
            double yMin = list.Select(v => Convert.ToDouble(v.PointTemp)).Min();
            double yMax = list.Select(v => Convert.ToDouble(v.PointTemp)).Max();
            LineItem myCurve;
            double xMax = list.Count;
            Color globalColor = GetColorFromProfile(Common.GlobalProfile.TempCurveRGB);

            myCurve = new LineItem(label, pair, LineColor[index - 1], SymbolType.UserDefined);
            CurveList curves = pane.CurveList;
            curves.Add(myCurve);
            List<CurveItem> items = curves.OrderBy(p => p.Points.Count).ToList();//根据记录点排序
            pane.CurveList.Clear();
            xMax = items.Last().Points.Count;
            index = 1;
            
            items.ForEach(p =>
            {
                if (myCurve == p && !isCompare)
                {
                    myCurve = new LineItem(p.Label.Text, p.Points, globalColor, SymbolType.UserDefined);
                    myCurve.Symbol = new Symbol(SymbolType.UserDefined, globalColor);
                    //myCurve.Symbol.Fill = new Fill(globalColor);
                    myCurve.Symbol.Fill=new Fill (GetImageFromColor(globalColor,((char)(64 + index)).ToString()),System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                }
                else
                {
                    myCurve = new LineItem(p.Label.Text, p.Points, LineColor[index - 1], SymbolType.UserDefined);
                    myCurve.Symbol = new Symbol(SymbolType.UserDefined, LineColor[index - 1]);
                    //myCurve.Symbol.Fill = new Fill(LineColor[index - 1]);
                    myCurve.Symbol.Fill = new Fill(GetImageFromColor(LineColor[index - 1], ((char)(64 + index)).ToString()), System.Drawing.Drawing2D.WrapMode.TileFlipXY);
                }
                pane.CurveList.Add(myCurve);
                myCurve.Symbol.IsAntiAlias = true;
                myCurve.Symbol.IsDrawSingleSymbol = true;//设置只画一个点
                
                //myCurve.Symbol.DrawSymbolIndex = (pane.CurveList.Count + 1) * 100 <= p.Points.Count - 1 ? (pane.CurveList.Count + 1) * 100 : p.Points.Count /10;
                int iSymbol = (int)CalcXAxisStep(1, xMax, YAxisLength) * index;
                myCurve.Symbol.DrawSymbolIndex = iSymbol >= p.Points.Count ? p.Points.Count - 1 : iSymbol;
                myCurve.Symbol.UserSymbol = new System.Drawing.Drawing2D.GraphicsPath();
                RectangleF recf = new RectangleF(-.75F, -.9F, 2F, 1.8F);
                //myCurve.Symbol.UserSymbol.AddString(((char)(64 + index)).ToString(), new FontFamily("Arial"), (int)FontStyle.Bold, 2, new PointF(-.8F, -1.1F), StringFormat.GenericDefault);
                myCurve.Symbol.UserSymbol.AddRectangle(recf);
                //myCurve.Symbol.UserSymbol.AddString(((char)(64 + index)).ToString(), new FontFamily("Arial"), (int)FontStyle.Bold, 2, recf, StringFormat.GenericDefault);
                myCurve.Symbol.MarkedSymbolList = GetMarkedIndexList(list);
                myCurve.Symbol.MarkedSymbolType = SymbolType.Triangle;
                myCurve.Symbol.MarkedSymbolColor = Color.Green;
                myCurve.Symbol.Border.IsVisible = false;
                
                myCurve.Line.Width = 1.5F;
                myCurve.Line.IsAntiAlias = true;
                double min,max;
                min=FindMaxYValue(myCurve.Points, false);
                max=FindMaxYValue(myCurve.Points, true);
                yMin = yMin>min?min:yMin;
                yMax = yMax>max?yMax:max;
                index++;
            });
            if (!isCompare)
            {
                if (!string.IsNullOrEmpty(_maxLimit))
                    yMax = yMax >= Convert.ToDouble(_maxLimit) ? yMax : Convert.ToDouble(_maxLimit);
                if (!string.IsNullOrEmpty(_minLimit))
                    yMin = yMin <= Convert.ToDouble(_minLimit) ? yMin : Convert.ToDouble(_minLimit);
            }
            GraphHelper.SetYAxisStep(pane, yMin, yMax, YAxisLength, 0.5);
            GraphHelper.SetXAxisStep(pane, 1, xMax, 10, selection);
        }
        private static void AddSingleLineToCanvas(GraphPane pane, PointPairList pair, List<PointKeyValue> list, string label, XAxisVisibility selection, int index, bool isCompare,bool isMarked)
        {
            double yMin = list.Select(v => Convert.ToDouble(v.PointTemp)).Min();
            double yMax = list.Select(v => Convert.ToDouble(v.PointTemp)).Max();
            double xMax = list.Count;
            LineItem myCurve;
            Color globalColor = GetColorFromProfile(Common.GlobalProfile.TempCurveRGB);
            if (isCompare)
            {
                globalColor = LineColor[index - 1];
            }
            myCurve = new LineItem(label, pair, globalColor, SymbolType.UserDefined);
            myCurve.Symbol = new Symbol(SymbolType.UserDefined, globalColor);
            myCurve.Symbol.Fill = new Fill(GetImageFromColor(globalColor, ((char)(64 + index)).ToString()), System.Drawing.Drawing2D.WrapMode.TileFlipXY);
            myCurve.Symbol.IsAntiAlias = true;
            myCurve.Symbol.IsDrawSingleSymbol = true;//设置只画一个点
            int iSymbol = (int)CalcXAxisStep(1, xMax, YAxisLength) * index;
            myCurve.Symbol.DrawSymbolIndex = iSymbol >= list.Count ? list.Count - 1 : iSymbol;
            myCurve.Symbol.UserSymbol = new System.Drawing.Drawing2D.GraphicsPath();
            RectangleF recf = new RectangleF(-.75F, -.9F, 2F, 1.8F);
            myCurve.Symbol.UserSymbol.AddRectangle(recf);
            if (isMarked)
            {
                myCurve.Symbol.MarkedSymbolList = GetMarkedIndexList(list);
                myCurve.Symbol.MarkedSymbolType = SymbolType.Triangle;
                myCurve.Symbol.MarkedSymbolColor = globalColor;
            }
            else
            {
                myCurve.Symbol.MarkedSymbolList = null;
            }
            myCurve.Symbol.Border.IsVisible = false;
            myCurve.Line.Width = 1.5F;
            myCurve.Line.IsAntiAlias = true;
            pane.CurveList.Add(myCurve);
            if (!isCompare)
            {
                GetMaxMinTempValue(ref yMax, ref yMin, _maxLimit, _minLimit);
                GraphHelper.SetYAxisStep(pane, yMin, yMax, YAxisLength, 0.5);
                GraphHelper.SetXAxisStep(pane, 1, xMax, 10, selection);
            }
        }
        private static Image GetImageFromColor(Color color,string s)
        {
            Image image = new Bitmap(15,15);
            using (Graphics g = Graphics.FromImage(image))
            {
                using (Brush b = new SolidBrush(color))
                {
                    g.FillRectangle(b,-1,0,15,15);
                    g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

                    //Arial字体，大小为9，白色。
                    FontFamily fm = new FontFamily("Arial");
                    Font font = new Font(fm, 11, FontStyle.Bold, GraphicsUnit.Point);
                    SolidBrush sb = new SolidBrush(Color.White);

                    g.DrawString(s, font, sb, new PointF(-.5F, .5F));
                    g.Dispose();
                    sb.Dispose();
                    //image=Platform.Utils.DrawTextOnImage(image, s, 1, 1);
                }
            }
            return image;
        }
        public static List<int> ForEachPointsToCalc(ZedGraphControl zgc,Rectangle rec,SuperDevice tag,XAxisVisibility selection)
        {
            int x = rec.X < rec.Right ? rec.X : rec.Right;
            int y = rec.Y < rec.Bottom ? rec.Y : rec.Bottom;
            PointF mousePt = new PointF(x, y);
            GraphPane pane = zgc.GraphPane;
            List<int> result = new List<int>();
            if (pane != null && rec.Width > 0)
            {
                double a, b, c, d;
                pane.ReverseTransform(mousePt, out a, out b);
                x = rec.X < rec.Right ? rec.Right : rec.X;
                y = rec.Y < rec.Bottom ? rec.Bottom : rec.Y;//第二条线
                pane.ReverseTransform(new PointF(x, y), out c, out d);
                a = a < 0 ? 0 : a;
                List<PointKeyValue> tempList = tag.tempList;
                tempList = GetTempList(zgc.GraphPane, tag, selection, (int)(a), (int)c);
                if (tempList != null)
                {
                    double rightPersent = c - (int)c;
                    double leftPersent = a - (int)a;
                    string first = pane.XAxis.Scale.TextLabels[0];
                    string second = pane.XAxis.Scale.TextLabels[1];
                    double interval = Convert.ToDouble(tag.LogInterval);
                    int rightCount = 0;
                    int leftCount = 0;
                    double gap = 0;
                    double seconds = 0;
                    DateTime start = DateTime.MinValue;
                    DateTime end = DateTime.MaxValue;
                    if (tempList.Count > 0)
                    {
                        start = tempList.First().PointTime;
                        end = tempList.Last().PointTime;
                    }
                    switch (selection)
                    {
                        case XAxisVisibility.DateAndTime:
                            DateTime bigDT= DateTime.ParseExact(second.Replace("\r\n"," "), Common.GlobalProfile.DateTimeFormator, CultureInfo.CurrentCulture);
                            DateTime smallDT = DateTime.ParseExact(first.Replace("\r\n", " "), Common.GlobalProfile.DateTimeFormator, CultureInfo.CurrentCulture);
                            gap = (bigDT-smallDT).TotalSeconds;
                            seconds = gap * rightPersent;
                            rightCount = (int)(seconds / interval);

                            seconds = gap * leftPersent;
                            leftCount = (int)(seconds / interval);

                            start = start.ToUniversalTime();
                            end = end.ToUniversalTime();
                            break;
                        case XAxisVisibility.ElapsedTime:
                            gap = Convert.ToDouble(TempsenFormatHelper.GetSecondsFromFormatString(second)) - Convert.ToDouble(TempsenFormatHelper.GetSecondsFromFormatString(first));
                            seconds = gap * rightPersent;
                            rightCount = (int)(seconds / interval);

                            seconds = gap * leftPersent;
                            leftCount = (int)(seconds / interval);
                            break;
                        case XAxisVisibility.DataPoints:
                            gap = Convert.ToInt32(second) - Convert.ToInt32(first);
                            rightCount = (int)(gap * rightPersent);

                            seconds = gap * leftPersent;
                            leftCount = (int)(seconds / interval);
                            break;
                        default:
                            break;
                    }
                    if (rightCount > 0)
                    {
                        int index = tag.tempList.FindIndex(p => p.PointTime > end);
                        for (int i = index; i < index + rightCount && index > -1 && i >= 0; i++)
                        {
                            tempList.Add(tag.tempList[i]);
                        }
                    }
                    int listCount = tempList.Count;
                    if (leftCount > 0 && leftCount <= listCount)
                    {
                        tempList.RemoveRange(0, leftCount);
                    }
                    for (int i = 0; i < tempList.Count; i++)
                    {
                        result.Add((int)(tempList[i].PointTemp * 100));
                    }
                }
            }
            return result;
        }
        public static void SetYAxisStep(GraphPane pane, double min,double max,int StepCount,double factor)
        {
            double tmpMax = Math.Truncate(max + 1);
            double tmpMin = Math.Truncate(min - 1);
            double floating = ((StepCount * factor) - (tmpMax - tmpMin) % (StepCount * factor)) * factor;
            pane.YAxis.Scale.Min = tmpMin - floating;
            pane.YAxis.Scale.Max = tmpMax + floating;
            pane.YAxis.Scale.MajorStep = (floating / factor + (tmpMax - tmpMin)) / StepCount;
            pane.YAxis.Scale.MinorStep = pane.YAxis.Scale.MajorStep ;
            pane.YAxis.Scale.BaseTic = tmpMin - floating;
        }
        public static void SetXAxisStep(GraphPane pane, double min, double max, int StepCount, XAxisVisibility selection)
        {
            double xMax = .0;
            double floating = (StepCount - (max - min) % StepCount);
            xMax = max + (floating == StepCount && max > 10 ? 0 : floating);
            //xMax = max;
            switch (selection)
            {
                default:
                    pane.XAxis.Scale.Min = min;
                    pane.XAxis.Scale.Max = xMax;
                    pane.XAxis.Scale.MajorStep = (xMax-min ) / StepCount;
                    pane.XAxis.Scale.MinorStep = pane.XAxis.Scale.MajorStep;
                    pane.XAxis.Scale.BaseTic = min;
                    //pane.XAxis.Scale.Min = min;
                    //pane.XAxis.Scale.Max = max;
                    //pane.XAxis.Scale.MajorStep = (max+1 - min) / StepCount;
                    break;
            }
        }
        private static double CalcXAxisStep(double min, double max, int StepCount)
        {
            double xMax = .0;
            double floating = (StepCount - (max - min) % StepCount);
            xMax = max + (floating == StepCount ? 0 : floating);
            return (xMax - min) / StepCount;
        }
        public static void DrawLimitLintAndFillIdeal(GraphPane pane, SuperDevice Tag, bool isShowIdeal, bool isShowLimit)
        {

            if (Tag == null || Tag.tempList.Count == 0 || Tag.AlarmMode == 0)
                return;
            pane.GraphObjList.Clear();
            double yMax = Tag.tempList.Select(p => p.PointTemp).Max();
            double yMin = Tag.tempList.Select(p => p.PointTemp).Min();
            GetMaxMinTempValue(ref yMax, ref yMin, _maxLimit, _minLimit);
            if (!isShowIdeal && !isShowLimit)
            {
                return;
            }
            string hightext, lowtext, a1Text, a2Text, a5Text;//limitline show text
            double highTemp = double.MinValue, lowTemp = double.MinValue, a1Temp = double.MinValue, a2Temp = double.MinValue, a5Temp = double.MinValue;//limitline tempurature;
            BoxObj box,box1,box2,box5;
            TextObj text3, text4,text1,text2,text5;
            hightext = lowtext = a1Text = a2Text = a5Text=string.Empty;
            if (isShowLimit)
            {
                if (Tag.AlarmMode == 1)
                {
                    if (!string.IsNullOrEmpty(Tag.AlarmHighLimit))
                    {
                        highTemp = Convert.ToDouble(Tag.AlarmHighLimit);
                        hightext = highTemp.ToString() + "(HL)";
                    }
                    if (!string.IsNullOrEmpty(Tag.AlarmLowLimit))
                    {
                        lowTemp = Convert.ToDouble(Tag.AlarmLowLimit);
                        lowtext = lowTemp.ToString() + "(LL)";
                    }
                }
                else if (Tag.AlarmMode == 2)
                {
                    if (!string.IsNullOrEmpty(Tag.A3))
                    {
                        highTemp = Convert.ToDouble(Tag.A3);
                        hightext = highTemp.ToString();
                    }
                    if (!string.IsNullOrEmpty(Tag.A4))
                    {
                        lowTemp = Convert.ToDouble(Tag.A4);
                        lowtext = lowTemp.ToString();
                    }
                    if (!string.IsNullOrEmpty(Tag.A1))
                    {
                        a1Temp = Convert.ToDouble(Tag.A1);
                        a1Text = a1Temp.ToString();
                    }
                    if (!string.IsNullOrEmpty(Tag.A2))
                    {
                        a2Temp = Convert.ToDouble(Tag.A2);
                        a2Text = a2Temp.ToString();
                    }
                    if (!string.IsNullOrEmpty(Tag.A2))
                    {
                        a5Temp = Convert.ToDouble(Tag.A5);
                        a5Text = a5Temp.ToString();
                    }
                }
                text3 = new TextObj(hightext, hightext.Contains("(") ? 25 : 22, highTemp, CoordType.XChartScaleYScale);
                text4 = new TextObj(lowtext, lowtext.Contains("(") ? 25 : 22, lowTemp, CoordType.XChartScaleYScale);
                text4.FontSpec.Fill.IsVisible = text3.FontSpec.Fill.IsVisible = false;
                text4.FontSpec.Border.IsVisible = text3.FontSpec.Border.IsVisible = false;
                text4.FontSpec.FontColor = text3.FontSpec.FontColor = GetColorFromProfile(Common.GlobalProfile.AlarmLineRGB);
                text4.FontSpec.IsBold = text3.FontSpec.IsBold = false;
                text4.ZOrder = text3.ZOrder = ZOrder.A_InFront;
                if (highTemp != double.MinValue && lowTemp != double.MinValue)
                {
                    box = new BoxObj(1, highTemp, pane.XAxis.Scale.Max, highTemp - lowTemp, GetColorFromProfile(Common.GlobalProfile.AlarmLineRGB), Color.Empty);
                    pane.GraphObjList.Add(text3);
                    pane.GraphObjList.Add(text4);
                }
                else if (highTemp != double.MinValue)
                {
                    box = new BoxObj(1, highTemp, pane.XAxis.Scale.Max, double.MinValue, GetColorFromProfile(Common.GlobalProfile.AlarmLineRGB), Color.Empty);
                    pane.GraphObjList.Add(text3);
                }
                else
                {
                    box = new BoxObj(1, lowTemp, pane.XAxis.Scale.Max, double.MinValue, GetColorFromProfile(Common.GlobalProfile.AlarmLineRGB), Color.Empty);
                    pane.GraphObjList.Add(text4);
                }
                box.IsClippedToChartRect = true;
                box.ZOrder = ZOrder.E_BehindCurves;
                pane.GraphObjList.Add(box);
                if (!string.IsNullOrEmpty(a1Text))
                {
                    text1 = new TextObj(a1Text, 22, a1Temp, CoordType.XChartScaleYScale);
                    text1.FontSpec.Fill.IsVisible = false;
                    text1.FontSpec.Border.IsVisible = false;
                    text1.FontSpec.FontColor = GetColorFromProfile(Common.GlobalProfile.AlarmLineRGB);
                    text1.FontSpec.IsBold = false;
                    text1.ZOrder = ZOrder.A_InFront;
                    box1 = new BoxObj(1, a1Temp, pane.XAxis.Scale.Max, double.MinValue, GetColorFromProfile(Common.GlobalProfile.AlarmLineRGB), Color.Empty);
                    box1.IsClippedToChartRect = true;
                    box1.ZOrder = ZOrder.E_BehindCurves;
                    pane.GraphObjList.Add(text1);
                    pane.GraphObjList.Add(box1);
                }
                if (!string.IsNullOrEmpty(a2Text))
                {
                    text2 = new TextObj(a2Text, 22, a2Temp, CoordType.XChartScaleYScale);
                    text2.FontSpec.Fill.IsVisible = false;
                    text2.FontSpec.Border.IsVisible = false;
                    text2.FontSpec.FontColor = GetColorFromProfile(Common.GlobalProfile.AlarmLineRGB);
                    text2.FontSpec.IsBold = false;
                    text2.ZOrder = ZOrder.A_InFront;
                    box2 = new BoxObj(1, a2Temp, pane.XAxis.Scale.Max,double.MinValue, GetColorFromProfile(Common.GlobalProfile.AlarmLineRGB), Color.Empty);
                    box2.IsClippedToChartRect = true;
                    box2.ZOrder = ZOrder.E_BehindCurves;
                    pane.GraphObjList.Add(box2);
                    pane.GraphObjList.Add(text2);
                }
                if (!string.IsNullOrEmpty(a5Text))
                {
                    text5 = new TextObj(a5Text, 22, a5Temp, CoordType.XChartScaleYScale);
                    text5.FontSpec.Fill.IsVisible = false;
                    text5.FontSpec.Border.IsVisible = false;
                    text5.FontSpec.FontColor = GetColorFromProfile(Common.GlobalProfile.AlarmLineRGB);
                    text5.FontSpec.IsBold = false;
                    text5.ZOrder = ZOrder.A_InFront;
                    box5 = new BoxObj(1, a5Temp, pane.XAxis.Scale.Max, double.MinValue, GetColorFromProfile(Common.GlobalProfile.AlarmLineRGB), Color.Empty);
                    box5.IsClippedToChartRect = true;
                    box5.ZOrder = ZOrder.E_BehindCurves;
                    pane.GraphObjList.Add(box5);
                    pane.GraphObjList.Add(text5);
                    
                }
                //box.ZOrder = ZOrder.E_BehindCurves;
            }
            //SetYAxisStep(pane, yMin, yMax, YAxisLength, 0.5);
            if (isShowIdeal)//fill ideal
            {
                if (Tag.AlarmMode == 1)
                {
                    //yMax = Convert.ToDouble(Tag.AlarmHighLimit) > tagMax ? Convert.ToDouble(Tag.AlarmHighLimit) : tagMax;
                    //yMin = Convert.ToDouble(Tag.AlarmLowLimit) < tagMin ? Convert.ToDouble(Tag.AlarmLowLimit) : tagMin;
                    if (!string.IsNullOrEmpty(Tag.AlarmHighLimit))
                    {
                        highTemp = Convert.ToDouble(Tag.AlarmHighLimit);
                        hightext = highTemp.ToString() + "(HL)";
                    }
                    if (!string.IsNullOrEmpty(Tag.AlarmLowLimit))
                    {
                        lowTemp = Convert.ToDouble(Tag.AlarmLowLimit);
                        lowtext = lowTemp.ToString() + "(LL)";
                    }
                }
                else if (Tag.AlarmMode == 2)
                {
                    if (!string.IsNullOrEmpty(Tag.A3))
                    {
                        highTemp = Convert.ToDouble(Tag.A3);
                        hightext = highTemp.ToString();
                    }
                    if (!string.IsNullOrEmpty(Tag.A4))
                    {
                        lowTemp = Convert.ToDouble(Tag.A4);
                        lowtext = lowTemp.ToString();
                    }
                }
                if (highTemp != double.MinValue && lowTemp != double.MinValue)
                {
                    box = new BoxObj(1, highTemp, pane.XAxis.Scale.Max, highTemp - lowTemp, GetColorFromProfile(Common.GlobalProfile.AlarmLineRGB), Color.Empty);
                    box.Fill = new Fill(Color.FromArgb(255, GetColorFromProfile(Common.GlobalProfile.IdealRangeRGB)));
                }
                else if (highTemp != double.MinValue)
                    box = new BoxObj(1, highTemp, pane.XAxis.Scale.Max, double.MinValue, GetColorFromProfile(Common.GlobalProfile.AlarmLineRGB), Color.Empty);
                else
                    box = new BoxObj(1, lowTemp, pane.XAxis.Scale.Max, double.MinValue, GetColorFromProfile(Common.GlobalProfile.AlarmLineRGB), Color.Empty);
                box.IsClippedToChartRect = true;
                if (!isShowLimit)
                    box.Border.Color = Color.Empty;
                else
                    box.Border.Color = GetColorFromProfile(Common.GlobalProfile.AlarmLineRGB);
                box.ZOrder = ZOrder.F_BehindGrid;
                pane.GraphObjList.Add(box);
            }
        }
        public static void RemoveLimitLine(GraphPane pane,SuperDevice Tag)
        {
            pane.GraphObjList.Clear();
            double yMin = Tag.tempList.Select(v => Convert.ToDouble(v.PointTemp)).Min();
            double yMax = Tag.tempList.Select(v => Convert.ToDouble(v.PointTemp)).Max();
            double xMax = Tag.tempList.Count;
            GraphHelper.SetYAxisStep(pane, yMin, yMax, YAxisLength, 0.5);
        }
        public static void SetLimitLineYAxis(GraphPane pane, LineItem line, List<PointKeyValue> list, bool IsMax)
        {
            double yMin, yMax;
            if (IsMax)
            {
                //yMin = Tag.points.Select(v => Convert.ToDouble(v.PointTemp)).Min();
                if (line != null && line.Points.Count > 0)
                {
                    yMin = line.Points[0].Y >= list.Select(v => v.PointTemp).Min() ? list.Select(v => v.PointTemp).Min() : line.Points[0].Y;
                }
                else
                    yMin = list.Select(v => v.PointTemp).Min();
                //yMin = pane.YAxis.Scale.Min;
                yMax = list.Select(v => v.PointTemp).Max();
            }
            else
            {
                yMin = list.Select(v => v.PointTemp).Min();
                if (line != null && line.Points.Count > 0)
                {
                    yMax = line.Points[0].Y >= list.Select(v => v.PointTemp).Max() ? line.Points[0].Y : list.Select(v => v.PointTemp).Max();
                }
                else
                    yMax = list.Select(v => v.PointTemp).Max();
            }
            SetYAxisStep(pane, yMin, yMax, YAxisLength, 0.5);
        }
        public static void SetToolTip(ZedGraphControl zedGraphControl1,Rectangle MouseRec,SuperDevice Tag,XAxisVisibility selection, ToolTip toolTip)
        {
            if (Tag != null)
            {
                List<int> temp = GraphHelper.ForEachPointsToCalc(zedGraphControl1, MouseRec, Tag, selection);
                toolTip.SetToolTip(zedGraphControl1, GetTips(temp, Tag));
                toolTip.ShowAlways = true;
                toolTip.AutoPopDelay = 30000;
                toolTip.Active = true;
            }
        }
        public static void SetStatisticsTips(ZedGraphControl zgc, Rectangle MouseRec, ToolTip toolTip,Size size,Point mouseX)
        {
            StringBuilder builder = new StringBuilder();
            if ( MouseRec.Width > 0 && size != Size.Empty)
            {
                toolTip.AutoPopDelay = 30000;
                toolTip.Active = true;
                if (mouseX.X + size.Width > zgc.Width)
                {
                    //toolTip.Show("0", zgc, zgc.Width - size.Width, mouseX.Y);
                    //toolTip.ShowAlways = false;
                    toolTip.SetToolTip(zgc, "0");
                }
                else
                {
                    toolTip.SetToolTip(zgc, "0");
                }
            }
        }
        public static List<string[]> GetRightAnalysisTips(ZedGraphControl zedGraphControl1,Rectangle MouseRec, List<SuperDevice> tags, XAxisVisibility selection)
        {
            List<string[]> result = new List<string[]>();
            if (tags != null && tags.Count > 0 && MouseRec.Width > 0)
            {
                for (int i = 0; i < tags.Count; i++)
                {
                    string[] tip = new[] { string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty,string.Empty };
                    List<int> temp = GraphHelper.ForEachPointsToCalc(zedGraphControl1, MouseRec, tags[i], selection);
                    for (int j = 0; j < 7; j++)
                    {
                        tip[j] = GetTips(temp, tags[i], j);
                    }
                    if (tip.Count(p => !string.IsNullOrEmpty(p)) > 0)
                    {
                        result.Add(tip);
                    }
                }
                
            }
            return result;
        }
        #region 右键
        public static void DrawRectangle(ZedGraphControl zedGraphControl1, Rectangle MouseRec)
        {
            Rectangle rec = zedGraphControl1.RectangleToScreen(MouseRec);
            ControlPaint.DrawReversibleFrame(rec, Color.Black, FrameStyle.Dashed);
        }
        private static string GetTips(List<int> temp, SuperDevice Tag)
        {
            string tips = string.Empty;
            if (temp != null && temp.Count > 0)
            {
                string serialNo = string.Format("Serial: {0}",Tag.SerialNumber);
                string trip = string.Format("Trip: {0}", Tag.TripNumber);
                string pointCount = string.Format("Points: {0}", temp.Count);
                int elapsed = (temp.Count-1) * Convert.ToInt32(Tag.LogInterval);
                string elapsedTime = string.Format("Elapsed Time: {0}", TempsenFormatHelper.ConvertSencondToFormmatedTime(elapsed.ToString()));
                string maxTemp = string.Format("Max Temp: {0}°{1}", temp.Max() * 1.0 / 100,Tag.TempUnit);
                string minTemp = string.Format("Min Temp: {0}°{1}", temp.Min() * 1.0 / 100, Tag.TempUnit);
                string mkt = string.Format("MKT: {0}°{1}", Common.CalcMKT(temp),Tag.TempUnit);
                tips = string.Concat(serialNo,Environment.NewLine ,trip, Environment.NewLine, pointCount, Environment.NewLine
                                                                  , elapsedTime, Environment.NewLine, maxTemp, Environment.NewLine, minTemp
                                                                  , Environment.NewLine, mkt);
            }
            return tips;
        }
        private static string GetTips(List<int> temp, SuperDevice Tag,int handle)
        {
            string tips = string.Empty;
            if (temp != null && temp.Count > 0)
            {
                switch (handle)
                {
                    case 0:
                        tips = string.Format("Serial: {0}", Tag.SerialNumber);
                        break;
                    case 1:
                        tips = string.Format("Trip: {0}", Tag.TripNumber);
                        break;
                    case 2:
                        string elapsed = TempsenFormatHelper.ConvertSencondToFormmatedTime(((temp.Count-1) * Convert.ToInt32(Tag.LogInterval)).ToString());
                        tips = string.Format("Elapsed Time: {0}", elapsed);
                        break;
                    case 3:
                        tips = string.Format("Points: {0}", temp.Count);
                        break;
                    case 4:
                        string max = (temp.Max() * 1.0 / 100).ToString();
                        tips = string.Format("Max Temp: {0}°{1}", max, Tag.TempUnit);
                        break;
                    case 5:
                        string min = (temp.Min() * 1.0 / 100).ToString();
                        tips = string.Format("Min Temp: {0}°{1}", min, Tag.TempUnit);
                        break;
                    case 6:
                        tips = string.Format("MKT: {0}°{1}", Common.CalcMKT(temp), Tag.TempUnit);
                        break;
                    default:
                        break;
                }
            }
            return tips;
        }
        public static void ResizeRectangle(Point p, ZedGraphControl zedGraphControl1, ref Rectangle MouseRec)
        {
            DrawRectangle(zedGraphControl1,MouseRec);
            MouseRec.Width = p.X - MouseRec.Left;
            MouseRec.Height = (int)zedGraphControl1.GraphPane.Chart.Rect.Height;
            DrawRectangle(zedGraphControl1, MouseRec);
        }
        public static void DrawOriginal(Point p, ZedGraphControl zedGraphControl1, ref Rectangle MouseRec)
        {
            //zedGraphControl1.GraphPane.XAxis.Scale.
            Cursor.Clip = zedGraphControl1.RectangleToScreen(new Rectangle((int)zedGraphControl1.GraphPane.Chart.Rect.Location.X
                                                                            , (int)zedGraphControl1.GraphPane.Chart.Rect.Location.Y
                                                                            , (int)zedGraphControl1.GraphPane.Chart.Rect.Width
                                                                            , (int)zedGraphControl1.GraphPane.Chart.Rect.Height));
            MouseRec = new Rectangle(p.X, (int)zedGraphControl1.GraphPane.Chart.Rect.Location.Y//起始点
                                     , 0, (int)zedGraphControl1.GraphPane.Chart.Rect.Height);
        }
        public static void ReDraw(bool all, ZedGraphControl zedGraphControl1, ref Rectangle MouseRec)
        {
            if (all)
            {
                System.Drawing.Pen myPen = new System.Drawing.Pen(System.Drawing.Color.Gray, 2f);
                using (Graphics g = zedGraphControl1.CreateGraphics())
                {
                    //确定顶点及长宽
                    MouseRec = new Rectangle(MouseRec.X < MouseRec.Right ? MouseRec.X : MouseRec.Right
                                         , MouseRec.Y < MouseRec.Bottom ? MouseRec.Y : MouseRec.Bottom
                                         , Math.Abs(MouseRec.Width), MouseRec.Height);
                    g.DrawRectangle(myPen, MouseRec);
                    SolidBrush sb = new SolidBrush(Color.FromArgb(0x5A, 211, 244, 253));//创建半透明画刷
                    g.FillRectangle(sb, MouseRec);
                    myPen.Dispose();
                }
            }
        }
        //鼠标是否移动到矩形区域上改变鼠标样式
        public static RedimStatus IsOverRectangle(int x, int y, ref bool IsLeftSide, ref Rectangle MouseRec)
        {
            int x1 = MouseRec.X < MouseRec.Right ? MouseRec.X : MouseRec.Right;
            int x2 = MouseRec.X < MouseRec.Right ? MouseRec.Right : MouseRec.X;
            if (x > x1 && x < x2)
                return RedimStatus.Center;
            else if (x == x1)
            {
                IsLeftSide = true;
                return RedimStatus.West;
            }
            else if (x == x2)
            {
                IsLeftSide = false;
                return RedimStatus.East;
            }
            else
                return RedimStatus.None;
        }
        //移动矩形
        public static void MoveRectangle(int x, int y, int MouseDownX, Rec rec, ref Rectangle MouseRec, ZedGraphControl zedGraphControl1)
        {
            int xcross = rec.X + x - MouseDownX;
            if ((rec.X + Math.Abs(rec.Width) + x - MouseDownX) > zedGraphControl1.GraphPane.Chart.Rect.Right||
                rec.X + x - MouseDownX < zedGraphControl1.GraphPane.Chart.Rect.Left)
            {
                int fl=(rec.X + Math.Abs(rec.Width) + x - MouseDownX)-(int)zedGraphControl1.GraphPane.Chart.Rect.Right;
                xcross =xcross - fl;
                if (fl < 0)
                    xcross = (int)zedGraphControl1.GraphPane.Chart.Rect.Left;
                MouseRec = new Rectangle(xcross, (int)zedGraphControl1.GraphPane.Chart.Rect.Location.Y
            , Math.Abs(MouseRec.Width), MouseRec.Height);
            }
            else
                MouseRec = new Rectangle(rec.X + x - MouseDownX, (int)zedGraphControl1.GraphPane.Chart.Rect.Location.Y
            , Math.Abs(MouseRec.Width), MouseRec.Height);
        }
        /// <summary>
        /// 改变矩形大小
        /// </summary>
        public static void RedimRectangle(int x, int y, int MouseDownX, bool IsLeftSide, Rec rec, ref Rectangle MouseRec, ZedGraphControl zedGraphControl1)
        {
            if (IsLeftSide)
            {
                MouseRec = new Rectangle(x, (int)zedGraphControl1.GraphPane.Chart.Rect.Location.Y
                                        , Math.Abs(rec.Width) - x + rec.X, MouseRec.Height);
            }
            else
                MouseRec = new Rectangle(rec.X, (int)zedGraphControl1.GraphPane.Chart.Rect.Location.Y
                                        , Math.Abs(rec.Width) + x - MouseDownX, MouseRec.Height);
        }
        public static List<int> ForEachRightPoints(ZedGraphControl zed, Rectangle rec, SuperDevice Tag)
        {
            int x = rec.X < rec.Right ? rec.X : rec.Right;
            int y = rec.Y < rec.Bottom ? rec.Y : rec.Bottom;
            PointF mousePt = new PointF(x, y);
            GraphPane pane = zed.MasterPane.FindChartRect(mousePt);
            //pane.fin
            if (pane != null)
            {
                double a, b;
                pane.ReverseTransform(mousePt, out a, out b);
                XDate x1 = new XDate(a);
                x = rec.X < rec.Right ? rec.Right : rec.X;
                y = rec.Y < rec.Bottom ? rec.Bottom : rec.Y;//第二条线
                pane.ReverseTransform(new PointF(x, y), out a, out b);
                XDate x2 = new XDate(a);
                //List<int> temperature = new List<int>();
                var v = from p in Tag.tempList
                        where p.PointTime >= x1.DateTime && p.PointTime <= x2.DateTime
                        select (int)(Convert.ToDouble(p.PointTemp) * 100);
                return v.ToList();
            }
            else
                return null;
        }
        public static void SetTempTimeFormator(ref List<PointKeyValue> list)
        {
            list.ForEach(p =>
            {
                p.PointTime = Convert.ToDateTime(p.PointTime.ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture));
            });
        }
        private static double FindMaxYValue(IPointList points,bool isMax)
        {
            List<double> list = new List<double>();
            for (int i = 0; i < points.Count; i++)
            {
                list.Add(points[i].Y);
            }
            if (isMax)
                return list.Max();
            else
                return list.Min();
        }
        #endregion
        #region 左键放大
        /// <summary>
        /// 处理放大信息
        /// </summary>
        /// <param name="zgc"></param>
        /// <param name="e"></param>
        public static void HandleZoomFinish(ZedGraphControl zgc, Point dragEnd, SuperDevice Tag, XAxisVisibility selection, int iIndex, bool isCompare, bool isMarked)
        {
            GraphPane pane = zgc.GraphPane;
            Point mousePt = dragEnd;
            double x1, y1, x2, y2;
            int start, end;
            pane.ReverseTransform(zgc.DragStartPt, out x1, out y1);
            pane.ReverseTransform(mousePt, out x2, out y2);
            int _pointsCount = (int)Math.Abs((Math.Truncate(x2) - Math.Ceiling(x1)));
            if (_pointsCount < 11 || zgc.DragStartPt.X <= pane.Chart.Rect.Left || zgc.DragStartPt.X >= pane.Chart.Rect.Right || zgc.DragStartPt.Y <= pane.Chart.Rect.Top || zgc.DragStartPt.Y >= pane.Chart.Rect.Bottom)
            {
                return;
            }
            else
            {
                start = (int)Math.Ceiling(x1) < (int)Math.Truncate(x2) ? (int)Math.Ceiling(x1) : (int)Math.Truncate(x2);
                end = (int)Math.Ceiling(x1) > (int)Math.Truncate(x2) ? (int)Math.Ceiling(x1) : (int)Math.Truncate(x2);
                start = start < 1 ? 1 : start;
                end = end < 1 ? 1 : end;
                List<PointKeyValue> list = new List<PointKeyValue>();
                list = GetTempList(zgc.GraphPane, Tag, selection, start, end);
                if (list.Count == 0)
                    return;
                string[] labels = SetTextLabels(Tag,list, selection, 1, list.Count, YAxisLength);
                pane.CurveList.Clear();
                SetGraphAsDefaultProperity(zgc, selection);
                PointPairList pair = new PointPairList();
                for (int i = 0; i < list.Count; i++)
                {
                    pair.Add(i + 1, list[i].PointTemp);
                }
                //AddLineToCanvas(pane, pair, list, Tag.SerialNumber + "_" + Tag.TripNumber, selection, iIndex, isCompare);
                AddSingleLineToCanvas(pane, pair, list, Tag.SerialNumber + "_" + Tag.TripNumber, selection, iIndex, isCompare, isMarked);
                /*重新设置text label*/
                pane.XAxis.Scale.TextLabels = labels;
                pane.XAxis.Scale.FontSpec.Size = 11.0f;
                pane.YAxis.Title.Text = string.Format("Temperature(°{0})",Tag.TempUnit);
                if (GraphHelper.rec != RectangleF.Empty)
                {
                    pane.Chart.Rect = rec;
                }
                zgc.AxisChange();
                zgc.Refresh();
            }
        }
        public static void HandleZoomFinish(ZedGraphControl zgc, Point dragEnd, List<SuperDevice> tags, XAxisVisibility selection,bool isMarked)
        {
            GraphPane pane = zgc.GraphPane;
            Point mousePt = dragEnd;
            double x1, y1, x2, y2;
            int start, end;
            pane.ReverseTransform(zgc.DragStartPt, out x1, out y1);
            pane.ReverseTransform(mousePt, out x2, out y2);
            int _pointsCount = (int)Math.Abs((Math.Truncate(x2) - Math.Ceiling(x1)));
            if (_pointsCount < 11 || zgc.DragStartPt.X <= pane.Chart.Rect.Left || zgc.DragStartPt.X >= pane.Chart.Rect.Right || zgc.DragStartPt.Y <= pane.Chart.Rect.Top || zgc.DragStartPt.Y >= pane.Chart.Rect.Bottom)
            {
                return;
            }
            else
            {
                start = (int)Math.Ceiling(x1) < (int)Math.Truncate(x2) ? (int)Math.Ceiling(x1) : (int)Math.Truncate(x2);
                end = (int)Math.Ceiling(x1) > (int)Math.Truncate(x2) ? (int)Math.Ceiling(x1) : (int)Math.Truncate(x2);
                start = start < 1 ? 1 : start;
                end = end < 1 ? 1 : end;
               
                switch (selection)
                {
                    case XAxisVisibility.DateAndTime:
                        HandleDateTimeZoom(pane, tags, start, end, isMarked);
                        break;
                    case XAxisVisibility.ElapsedTime:
                        HandleElapseTimeZoom(pane, tags, start, end, isMarked);
                        break;
                    case XAxisVisibility.DataPoints:
                        HandleDataPointZoom(pane, tags, start, end, isMarked);
                        break;
                    default:
                        break;
                }
                //Dictionary<string, int> indexLine = new Dictionary<string, int>();
                //for (int i = 0; i < tags.Count; i++)
                //{
                //    indexLine.Add(tags[i].SerialNumber + "_" + tags[i].TripNumber, i + 1);
                //}
                //pane.CurveList.Clear();
                //SetGraphAsDefaultProperity(zgc, selection);
                //tags=tags.OrderBy(p => p.tempList.Count).ToList();
                //string[] labels=new string[end-start];
                //double yMin, yMax, xMax;
                //yMin = yMax = xMax = 0;
                //for (int i = 0; i < tags.Count; i++)
                //{
                //    List<PointKeyValue> list = GetTempList(zgc.GraphPane, tags[i], selection, start, end);
                //    if (list.Count == 0)
                //        continue;
                //    labels = SetTextLabels(tags[i], list, selection, 1, list.Count, 10);
                //    PointPairList pair = new PointPairList();
                //    for (int j = 0; j < list.Count; j++)
                //    {
                //        pair.Add(j + 1, list[j].PointTemp);
                //    }
                //    string labelText=tags[i].SerialNumber + "_" + tags[i].TripNumber;
                //    AddSingleLineToCanvas(pane, pair, list,labelText , selection, indexLine[labelText], true,isMarked);
                //    if (yMin == 0 && yMax == 0 && xMax == 0)
                //    {
                //        yMin = list.Min(p => p.PointTemp);
                //        yMax = list.Max(p => p.PointTemp);
                //        xMax = list.Count;
                //    }
                //    else
                //    {
                //        double min = list.Min(p => p.PointTemp);
                //        double max = list.Max(p => p.PointTemp);
                //        yMin = yMin > min ? min : yMin;
                //        yMax = yMax > max ? yMax : max;
                //        xMax = xMax > list.Count ? xMax : list.Count;
                //    }
                //}
                //GraphHelper.SetYAxisStep(pane, yMin, yMax, YAxisLength, .5);
                //GraphHelper.SetXAxisStep(pane, 1, xMax, YAxisLength, selection);
                //pane.XAxis.Scale.TextLabels = labels;
                //pane.XAxis.Scale.FontSpec.Size = 11.0f;
                //pane.YAxis.Title.Text = string.Format("Temperature(°{0})", tags.FirstOrDefault().TempUnit);
                //if (GraphHelper.rec != RectangleF.Empty)
                //{
                //    pane.Chart.Rect = rec;
                //}
            }
        }
        private static bool IsZoomZoneValid(int start,int end,GraphPane pane)
        {
            bool flag = false;
            for (int i = 0; i < pane.CurveList.Count; i++)
            {
                CurveItem item = pane.CurveList[i];
                if (item.Points[item.Points.Count - 1].X < start || item.Points[item.Points.Count - 1].X > end)
                    flag = false;
                else
                    flag = true;
            }
            return flag;
        }
        public static List<PointKeyValue> GetTempList(GraphPane pane, SuperDevice Tag, XAxisVisibility selection, int start, int end)
        {
            List<PointKeyValue> points = new List<PointKeyValue>();
            List<string> _TextLabels = GetTextLabels(pane, start, end);
            if (_TextLabels.Count == 0||Tag==null)
                return points;
            //select the point pos
            switch (selection)
            {
                case XAxisVisibility.DataPoints:
                    List<int> l= _TextLabels.Select(p => Convert.ToInt32(p)-1).ToList();
                    if (l.Count > 0)
                        l.ForEach(p =>
                        {
                            if(p<Tag.tempList.Count)
                                points.Add(Tag.tempList[p]);
                        });
                    break;
                case XAxisVisibility.DateAndTime:
                    DateTimeFormatInfo dtfi=new DateTimeFormatInfo ();
                    dtfi.FullDateTimePattern=Common.GlobalProfile.DateTimeFormator;
                    List<PointKeyValue> temp = Tag.tempList.Where(v => v.PointTime.ToLocalTime() >= DateTime.ParseExact(_TextLabels.First().Replace(Environment.NewLine, " "), Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture)
                                                                      && v.PointTime.ToLocalTime() <= DateTime.ParseExact(_TextLabels.Last().Replace(Environment.NewLine, " "), Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture)).ToList();
                    if (temp != null && temp.Count > 0)
                        temp.ForEach(p => points.Add(new PointKeyValue { IsMark=p.IsMark, PointTime=p.PointTime.ToLocalTime(), PointTemp=p.PointTemp}));
                    break;
                default:
                    DateTime ori = Tag.tempList.First().PointTime;
                    List<string> xAxis = new List<string>();
                    double first = Convert.ToDouble(TempsenFormatHelper.GetSecondsFromFormatString(_TextLabels.First()));
                    double last = Convert.ToDouble(TempsenFormatHelper.GetSecondsFromFormatString(_TextLabels.Last()));
                    //int sIndex=Tag.tempList.FindIndex(p => (p.PointTime - ori).TotalSeconds >= first);
                    //int eIndex = Tag.tempList.FindIndex(p => (p.PointTime - ori).TotalSeconds >= last);
                    IEnumerable<PointKeyValue> result = Tag.tempList.Where(p => (p.PointTime - ori).TotalSeconds >= first && (p.PointTime - ori).TotalSeconds <= last);
                    //if (sIndex != -1)
                    //{
                    //    for (int j = sIndex; j <= eIndex && j < Tag.tempList.Count; j++)
                    //    {
                    //        points.Add(Tag.tempList[j]);
                    //    }
                    //}
                    points.AddRange(result);
                    break;
            }
            return points;
        }
        public static List<PointKeyValue> GetTempPointList(GraphPane pane, SuperDevice Tag, XAxisVisibility selection, int start, int end)
        {
            List<PointKeyValue> points = new List<PointKeyValue>();
            List<string> _TextLabels = GetTextLabels(pane, start, end);
            if (_TextLabels.Count == 0 || Tag == null)
                return points;
            //select the point pos
            switch (selection)
            {
                case XAxisVisibility.DataPoints:
                    List<int> l = _TextLabels.Select(p => Convert.ToInt32(p) - 1).ToList();
                    if (l.Count > 0)
                        l.ForEach(p =>
                        {
                            if (p < Tag.tempList.Count)
                                points.Add(Tag.tempList[p]);
                        });
                    break;
                case XAxisVisibility.DateAndTime:
                    DateTimeFormatInfo dtfi = new DateTimeFormatInfo();
                    dtfi.FullDateTimePattern = Common.GlobalProfile.DateTimeFormator;
                    List<PointKeyValue> temp = Tag.tempList.Where(v => v.PointTime.ToLocalTime() >= DateTime.ParseExact(_TextLabels.First().Replace(Environment.NewLine, " "), Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture)
                                                                      && v.PointTime.ToLocalTime() <= DateTime.ParseExact(_TextLabels.Last().Replace(Environment.NewLine, " "), Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture)).ToList();
                    if (temp != null && temp.Count > 0)
                        temp.ForEach(p => points.Add(new PointKeyValue { IsMark = p.IsMark, PointTime = p.PointTime.ToLocalTime(), PointTemp = p.PointTemp }));
                    break;
                default:
                    DateTime ori = Tag.tempList.First().PointTime;
                    string first = TempsenFormatHelper.GetSecondsFromFormatString(_TextLabels.First());
                    string last = TempsenFormatHelper.GetSecondsFromFormatString(_TextLabels.Last());
                    int sIndex=Tag.tempList.FindIndex(p => (p.PointTime - ori).TotalSeconds >= Convert.ToDouble(first));
                    int eIndex = Tag.tempList.FindLastIndex(p => (p.PointTime - ori).TotalSeconds <= Convert.ToDouble(last));
                    if (sIndex >= 0)
                    {
                        for (int j = sIndex; j <= eIndex && j < Tag.tempList.Count; j++)
                        {
                            points.Add(Tag.tempList[j]);
                        }
                    }
                    break;
            }
            return points;
        }
        private static List<string> GetTextLabels(GraphPane pane, int start, int end)
        {
            List<string> _TextLabels = new List<string>();
            if (pane.XAxis.Scale.TextLabels != null)
            {
                for (int i = start - 1; i < end && i < pane.XAxis.Scale.TextLabels.Length; i++)
                {
                    if (i < 0)
                        break;
                    _TextLabels.Add(pane.XAxis.Scale.TextLabels[i]);
                }
            }
            return _TextLabels;
        }
        private static string [] SetTextLabels(SuperDevice Tag, List<PointKeyValue> list, XAxisVisibility selection, double min, double max, int StepCount)
        {
            double xMax = .0;
            double floating = (StepCount - (max - min) % StepCount);
            xMax = max + (floating == StepCount ? 0 : floating);
            List<string> result = new List<string>();
            double interval = Convert.ToDouble(Tag.LogInterval);
            switch (selection)
            {
                case XAxisVisibility.DateAndTime:
                    //int last = list.Count;
                    PointKeyValue lastPoint = list.Last();
                    //lastPoint.PointTime = lastPoint.PointTime.ToUniversalTime();
                    int last = Tag.tempList.FindIndex(p => p.PointTime.ToLocalTime() == lastPoint.PointTime && p.PointTemp == lastPoint.PointTemp);
                    List<PointKeyValue> outList = new List<PointKeyValue>(list);
                    for (int i = last + 1; i <= last + floating && floating != StepCount; i = i + 1)
                    {
                        if (i >= Tag.tempList.Count)
                        {
                            PointKeyValue pkv = new PointKeyValue() { PointTemp = 0, PointTime = Tag.tempList.Last().PointTime.ToLocalTime().AddSeconds((i-last) * interval), IsMark = false };
                            outList.Add(pkv);
                        }
                        else
                        {
                            PointKeyValue pkv = new PointKeyValue() { PointTemp = Tag.tempList[i].PointTemp, PointTime = Tag.tempList[i].PointTime.ToLocalTime(), IsMark = Tag.tempList[i].IsMark };
                            list.Add(pkv);
                            outList.Add(pkv);
                        }
                    }
                    result = GetDateTimeTextFormat(outList.Select(p => p.PointTime).ToList());
                    break;
                case XAxisVisibility.DataPoints:
                    int j = Tag.tempList.IndexOf(list.First()) + 1;
                    last = list.Count;
                    list.ToList().ForEach(v =>
                    {
                        result.Add(j.ToString());
                        j++;
                    });
                    for (int i = last + 1; i <= last + floating && floating != StepCount; i = i + 1)
                    {
                        if (i < Tag.tempList.Count)
                            list.Add(Tag.tempList[i]);
                        result.Add(j.ToString());
                        j++;
                    }
                    break;
                default:
                    if (list != null && list.Count > 0)
                    {
                        DateTime start = Tag.tempList.First().PointTime;
                        last = list.Count;
                        //找到list在这个points中位置
                        int pointIndex = Tag.tempList.FindIndex(p => p.PointTime == list.First().PointTime);
                        for (int i = 0; i < list.Count; i++)
                        {
                            result.Add(TempsenFormatHelper.ConvertSencondToFormmatedTime((list[i].PointTime - start).TotalSeconds.ToString()));
                        }
                        double elaspedTime = Convert.ToDouble(TempsenFormatHelper.GetSecondsFromFormatString(result.Last()));
                        for (int i = last; i < last + floating && floating != StepCount; i++)
                        {
                            if (i + pointIndex < Tag.tempList.Count)
                                list.Add(Tag.tempList[i + pointIndex]);
                            result.Add(TempsenFormatHelper.ConvertSencondToFormmatedTime((elaspedTime+interval*(i-last+1)).ToString()));
                        }
                    }
                    break;
            }
            return result.ToArray();
        }
        public static string[] SetTextLabels(List<PointKeyValue> tempList,double interval,  XAxisVisibility selection, double min, double max, int StepCount)
        {
            double xMax = .0;
            double floating = (StepCount - (max - min) % StepCount);
            xMax = max + (floating == StepCount && max > 10 ? 0 : floating);
            List<string> result = new List<string>();
            List<PointKeyValue> list = new List<PointKeyValue>(tempList);
            List<PointKeyValue> unMark = list.Where(p => !p.IsMark).ToList();
            switch (selection)
            {
                case XAxisVisibility.DateAndTime:

                    for (int i = 1; i <= floating; i++)
                    {
                        PointKeyValue pkv = new PointKeyValue() { PointTemp = 0, PointTime = tempList.Last().PointTime.AddSeconds(i * interval), IsMark = false };
                        list.Add(pkv);
                    }
                    result = GetDateTimeTextFormat(list.Select(p => p.PointTime).ToList());
                    break;
                case XAxisVisibility.DataPoints:
                    for (int i = 1; i <= list.Count + floating; i++)
                    {
                        result.Add(i.ToString());
                    }
                    break;
                default:
                    for (int i = 0,j=0; i < list.Count + floating; i++)
                    {
                        if (i < list.Count)
                        {
                            if (!list[i].IsMark)
                            {
                                result.Add(TempsenFormatHelper.ConvertSencondToFormmatedTime((interval * j).ToString()));
                                j++;
                            }
                            else
                            {
                                result.Add(TempsenFormatHelper.ConvertSencondToFormmatedTime((list[i].PointTime-list.First().PointTime).TotalSeconds.ToString()));
                            }
                        }
                        else
                        {
                            result.Add(TempsenFormatHelper.ConvertSencondToFormmatedTime((interval * i).ToString()));
                        }
                    }
                    break;
            }
            return result.ToArray();
        }
        private static void HandleElapseTimeZoom(GraphPane pane, List<SuperDevice> tags,int start,int end,bool isMark)
        {
            SuperDevice sd = DoSearchLongestElapsedTimeList(tags);
            List<PointKeyValue> longestCurve = DoSearchElapsedTimeList(pane, tags,start,end);
            pane.CurveList.Clear();
            double yMin = .0, min = .0;
            double yMax = .0, max = .0;
            LineItem myItem;
            pane.XAxis.Scale.TextLabels = SetTextLabels(sd, longestCurve, XAxisVisibility.ElapsedTime, 1, longestCurve.Count, YAxisLength);
            start = 1; end = pane.XAxis.Scale.TextLabels.Length;
            //longestCurve = GetTempList(pane, sd, XAxisVisibility.ElapsedTime, start, end);
            for (int i = 0; i < tags.Count && sd != null; i++)
            {
                string sntn = tags[i].SerialNumber + "_" + tags[i].TripNumber;
                List<PointKeyValue> list = GetTempList(pane, tags[i], XAxisVisibility.ElapsedTime, start, end);
                if (list.Count == 0)
                    continue;
                else
                {
                    if (sd != tags[i])
                    {
                        int endX = FindIndexOfLongestCurveItem(list, longestCurve, Convert.ToDouble(tags[i].LogInterval));
                        myItem = AddElapsedTimeLine(list, sntn, i + 1, endX);
                    }
                    else
                    {
                        PointPairList pair = new PointPairList();
                        for (int j = 0; j < list.Count; j++)
                        {
                            pair.Add(j + 1, list[j].PointTemp);
                        }
                        myItem = GenerateCurveItem(sntn, pair, i + 1, 1, list.Count);
                        longestCurve = list;
                    }
                    if (isMark)
                    {
                        myItem.Symbol.MarkedSymbolList = GetMarkedIndexList(list);
                        myItem.Symbol.MarkedSymbolType = SymbolType.Triangle;
                        myItem.Symbol.MarkedSymbolColor = LineColor[i];
                    }
                    pane.CurveList.Add(myItem);
                    min = FindMaxYValue(myItem.Points, false);
                    max = FindMaxYValue(myItem.Points, true);
                    if (yMax == .0 && yMin == .0)
                    {
                        yMin = min;
                        yMax = max;
                    }
                    else
                    {
                        yMin = yMin > min ? min : yMin;
                        yMax = yMax > max ? yMax : max;
                    }
                }
            }
            //pane.XAxis.Scale.TextLabels = SetTextLabels(sd,longestCurve, XAxisVisibility.ElapsedTime, 1, longestCurve.Count, YAxisLength);
            GraphHelper.SetYAxisStep(pane, yMin, yMax, YAxisLength, 0.5);
            GraphHelper.SetXAxisStep(pane, 1, longestCurve.Count, YAxisLength, XAxisVisibility.ElapsedTime);
        }
        private static void HandleDataPointZoom(GraphPane pane, List<SuperDevice> tags, int start, int end, bool isMark)
        {
            if (tags != null && tags.Count > 0)
            {
                SuperDevice sd = DoSearchMostDataPointList(tags);
                string[] labels = new string[end - start];
                double yMin, yMax, xMax;
                yMin = yMax = xMax = 0;
                pane.CurveList.Clear();
                List<PointKeyValue> list = GetTempList(pane, sd, XAxisVisibility.DataPoints, start, end);
                labels = SetTextLabels(sd, list, XAxisVisibility.DataPoints, 1, list.Count, YAxisLength);
                pane.XAxis.Scale.TextLabels = labels;
                start = 1;
                end = labels.Length;
                for (int i = 0; i < tags.Count; i++)
                {
                    string sntn = tags[i].SerialNumber + "_" + tags[i].TripNumber;
                    list = GetTempList(pane, tags[i], XAxisVisibility.DataPoints, start, end);
                    if (list.Count == 0)
                        continue;
                    else
                    {
                        //if (sd == tags[i])
                        //{
                        //    labels = SetTextLabels(tags[i], list, XAxisVisibility.DataPoints, 1, list.Count, 10);
                        //}
                        PointPairList pair = new PointPairList();
                        for (int j = 0; j < list.Count; j++)
                        {
                            pair.Add(j + 1, list[j].PointTemp);
                        }
                        string labelText = tags[i].SerialNumber + "_" + tags[i].TripNumber;
                        AddSingleLineToCanvas(pane, pair, list, labelText, XAxisVisibility.DataPoints, i+1, true, isMark);
                        if (yMin == 0 && yMax == 0 && xMax == 0)
                        {
                            yMin = list.Min(p => p.PointTemp);
                            yMax = list.Max(p => p.PointTemp);
                            xMax = list.Count;
                        }
                        else
                        {
                            double min = list.Min(p => p.PointTemp);
                            double max = list.Max(p => p.PointTemp);
                            yMin = yMin > min ? min : yMin;
                            yMax = yMax > max ? yMax : max;
                            xMax = xMax > list.Count ? xMax : list.Count;
                        }
                    }
                }
                
                GraphHelper.SetYAxisStep(pane, yMin, yMax, YAxisLength, 0.5);
                GraphHelper.SetXAxisStep(pane, 1, xMax, YAxisLength, XAxisVisibility.DataPoints);
            }
        }
        private static void HandleDateTimeZoom(GraphPane pane, List<SuperDevice> tags, int start, int end, bool isMark)
        {
            if (tags != null && tags.Count > 0)
            {
                double yMin = .0, min = .0;
                double yMax = .0, max = .0;
                LineItem myItem;
                //DateTime start, end;
                List<DateTime> textLabels = GenerateDateTimeXAxis(pane, tags, start, end);
                pane.XAxis.Scale.TextLabels = GetDateTimeTextFormat(textLabels).ToArray();
                start = 1; end = pane.XAxis.Scale.TextLabels.Length;
                pane.CurveList.Clear();
                for (int i = 0; i < tags.Count; i++)
                {
                    string sntn = tags[i].SerialNumber + "_" + tags[i].TripNumber;
                    List<PointKeyValue> list = GetTempList(pane, tags[i], XAxisVisibility.DateAndTime, start, end);
                    if (list.Count == 0)
                        continue;
                    else
                    {
                        int first = FindThePosInTheXAxis(list, textLabels, true);
                        int last = FindThePosInTheXAxis(list, textLabels, false);
                        myItem = AddDateTimeLine(list, sntn, i + 1, first, last);
                        if (isMark)
                        {
                            myItem.Symbol.MarkedSymbolList = GetMarkedIndexList(list);
                            myItem.Symbol.MarkedSymbolType = SymbolType.Triangle;
                            myItem.Symbol.MarkedSymbolColor = LineColor[i];
                        }
                        pane.CurveList.Add(myItem);
                        min = FindMaxYValue(myItem.Points, false);
                        max = FindMaxYValue(myItem.Points, true);
                        if (yMax == .0 && yMin == .0)
                        {
                            yMin = min;
                            yMax = max;
                        }
                        else
                        {
                            yMin = yMin > min ? min : yMin;
                            yMax = yMax > max ? yMax : max;
                        }
                    }
                }
                //pane.XAxis.Scale.TextLabels = GetDateTimeTextFormat(textLabels).ToArray();
                GraphHelper.SetYAxisStep(pane, yMin, yMax, YAxisLength, 0.5);
                GraphHelper.SetXAxisStep(pane, 1, textLabels.Count, YAxisLength, XAxisVisibility.DateAndTime);
            }
        }
        private static List<DateTime> GenerateDateTimeXAxis(GraphPane pane,List<SuperDevice> tags,int start,int end)
        {
            List<DateTime> textLabels = new List<DateTime>();
            if (tags != null && tags.Count > 0)
            {
                DateTime min=DateTime.MaxValue;
                DateTime max=DateTime.MinValue;
                double interval = tags.Max(p => Convert.ToDouble(p.LogInterval));
                List<PointKeyValue> maxList=new List<PointKeyValue> ();
                pane.CurveList.Clear();
                for (int i = 0; i < tags.Count; i++)
                {
                    string sntn = tags[i].SerialNumber + "_" + tags[i].TripNumber;
                    List<PointKeyValue> list = GetTempList(pane, tags[i], XAxisVisibility.DateAndTime, start, end);
                    if (list.Count == 0)
                        continue;
                    else
                    {
                        min = min >= list.Min(p => p.PointTime) ? list.Min(p => p.PointTime) : min;
                        max = max >= list.Max(p => p.PointTime) ? max : list.Max(p => p.PointTime);
                        if (interval == Convert.ToDouble(tags[i].LogInterval))
                            maxList = list;
                    }
                }
                textLabels = GetDataTimeLabels(min, max, interval, maxList.Where(p => p.IsMark).ToList());
            }
            return textLabels;
        }
        private static List<PointKeyValue> GenerateTempPoint(List<PointKeyValue> raw, List<PointKeyValue> full,double interval, int stepCount, XAxisVisibility selection)
        {
            double floating = (stepCount - (raw.Count - 1) % stepCount);
            List<PointKeyValue> result = new List<PointKeyValue>(raw);

            int pointIndex = full.FindIndex(p => p.PointTime > raw.Last().PointTime);
            switch (selection)
            {
                case XAxisVisibility.DateAndTime:

                    for (int i = pointIndex; i <= pointIndex + floating && floating != stepCount; i = i + 1)
                    {
                        if (i >= full.Count)
                        {
                            PointKeyValue pkv = new PointKeyValue() { PointTemp = 0, PointTime = full.Last().PointTime.AddSeconds((i - pointIndex) * interval), IsMark = false };
                            result.Add(pkv);
                        }
                        else
                        {
                            PointKeyValue pkv = new PointKeyValue() { PointTemp = full[i].PointTemp, PointTime = full[i].PointTime, IsMark = full[i].IsMark };
                            result.Add(pkv);
                        }
                    }
                    break;
                case XAxisVisibility.ElapsedTime:
                    DateTime start = full.First().PointTime;
                    //找到list在这个points中位置
                    if (pointIndex >= 0)
                    {
                        for (int i = pointIndex; i < pointIndex + floating && floating != stepCount; i++)
                        {
                            if (i < full.Count)
                            {
                                result.Add(full[i]);
                            }
                            else
                            {

                            }
                        }
                    }
                    break;
                case XAxisVisibility.DataPoints:
                    break;
                default:
                    break;
            }
            return result;
        }
        #endregion
        public static void SetMinMaxLimits(SuperDevice tag)
        {
            if (tag != null)
            {
                _maxLimit = _minLimit = string.Empty;
                if (tag.AlarmMode == 1)
                {
                    if (!string.IsNullOrEmpty(tag.AlarmHighLimit))
                        _maxLimit = tag.AlarmHighLimit;
                    if (!string.IsNullOrEmpty(tag.AlarmLowLimit))
                        _minLimit = tag.AlarmLowLimit;
                }
                else if (tag.AlarmMode == 2)
                {
                    if (!string.IsNullOrEmpty(tag.A1))
                        _maxLimit = tag.A1;
                    else if (!string.IsNullOrEmpty(tag.A2))
                        _maxLimit = tag.A2;
                    else if (!string.IsNullOrEmpty(tag.A3))
                        _maxLimit = tag.A3;

                    if (!string.IsNullOrEmpty(tag.A5))
                        _minLimit = tag.A5;
                    else if (!string.IsNullOrEmpty(tag.A4))
                        _minLimit = tag.A4;
                }
            }
        }
        /// <summary>
        /// 多条曲线横坐标切换
        /// </summary>
        /// <param name="zgc"></param>
        /// <param name="selection"></param>
        /// <param name="TagList"></param>
        public static void SetMultiLineGraph(ZedGraphControl zgc,XAxisVisibility selection,List<SuperDevice> TagList)
        {
            if (TagList == null || TagList.Count == 0)
                return;
            GraphPane pane = zgc.GraphPane;

        }
        public static void SetXAxisAsElapsedTime(GraphPane pane, List<SuperDevice> TagList, bool isMarked)
        {
            SuperDevice sd = DoSearchLongestElapsedTimeList(TagList);
            if (sd != null&&TagList!=null&&TagList.Count>0)
            {
                pane.CurveList.Clear();
                pane.GraphObjList.Clear();
                pane.XAxis.Type = AxisType.Linear;
                double yMin = .0,min=.0;
                double yMax = .0,max=.0;
                LineItem myItem;
                for (int i = 0; i < TagList.Count; i++)
                {
                    string sntn=TagList[i].SerialNumber + "_" + TagList[i].TripNumber;
                    if (TagList[i] != sd)
                    {
                        int endX = FindIndexOfLongestCurveItem(TagList[i], sd);
                        myItem = AddElapsedTimeLine(TagList[i].tempList,sntn, i + 1, endX);
                    }
                    else
                    {
                        PointPairList pair = new PointPairList();
                        for (int j = 0; j < TagList[i].tempList.Count; j++)
                        {
                            pair.Add(j + 1, TagList[i].tempList[j].PointTemp);
                        }
                        myItem = GenerateCurveItem(sntn, pair, i + 1, 1, TagList[i].tempList.Count);
                        
                    }
                    if (isMarked)
                    {
                        myItem.Symbol.MarkedSymbolList = GetMarkedIndexList(TagList[i].tempList);
                        myItem.Symbol.MarkedSymbolType = SymbolType.Triangle;
                        myItem.Symbol.MarkedSymbolColor = LineColor[i];
                    }
                    pane.CurveList.Add(myItem);
                    min = FindMaxYValue(myItem.Points, false);
                    max = FindMaxYValue(myItem.Points, true);
                    if (yMax == .0 && yMin == .0)
                    {
                        yMin = min;
                        yMax = max;
                    }
                    else
                    {
                        yMin = yMin > min ? min : yMin;
                        yMax = yMax > max ? yMax : max;
                    }
                }
                pane.XAxis.Scale.TextLabels = SetTextLabels(sd.tempList,Convert.ToDouble(sd.LogInterval), XAxisVisibility.ElapsedTime, 1, sd.tempList.Count, YAxisLength);
                GraphHelper.SetYAxisStep(pane, yMin, yMax, YAxisLength, 0.5);
                GraphHelper.SetXAxisStep(pane, 1, sd.tempList.Count, 10, XAxisVisibility.ElapsedTime);
                pane.XAxis.Scale.FontSpec.Size = 11.0f;
                if (GraphHelper.rec != RectangleF.Empty)
                {
                    pane.Chart.Rect = rec;
                }
            }
            else
            {
            }
        }
        public static void SetXAxisAsDateTime(GraphPane pane, List<SuperDevice> TagList,bool isMarked)
        {
            if (TagList != null && TagList.Count > 0)
            {
                pane.CurveList.Clear();
                pane.GraphObjList.Clear();
                pane.XAxis.Type = AxisType.Linear;
                double yMin = .0, min = .0;
                double yMax = .0, max = .0;
                LineItem myItem;
                DateTime start, end;
                start = DoSearchDatetimeList(TagList, false);
                end = DoSearchDatetimeList(TagList, true);
                double interval=TagList.Max(p => Convert.ToDouble(p.LogInterval));
                SuperDevice tag=TagList.FindLast(p => Convert.ToDouble(p.LogInterval) == interval);
                List<DateTime> textLabels = GetDataTimeLabels(start, end, interval, tag == null ? null : tag.tempList.Where(p => p.IsMark).ToList());
                for (int i = 0; i < TagList.Count; i++)
                {
                    int first=FindThePosInTheXAxis(TagList[i].tempList,textLabels,true);
                    int last=FindThePosInTheXAxis(TagList[i].tempList,textLabels,false);
                    string sntn=TagList[i].SerialNumber+"_"+TagList[i].TripNumber;
                    myItem = AddDateTimeLine(TagList[i].tempList, sntn, i + 1, first, last);
                    if (isMarked)
                    {
                        myItem.Symbol.MarkedSymbolList = GetMarkedIndexList(TagList[i].tempList);
                        myItem.Symbol.MarkedSymbolType = SymbolType.Triangle;
                        myItem.Symbol.MarkedSymbolColor = LineColor[i];
                    }
                    pane.CurveList.Add(myItem);
                    min = FindMaxYValue(myItem.Points, false);
                    max = FindMaxYValue(myItem.Points, true);
                    if (yMax == .0 && yMin == .0)
                    {
                        yMin = min;
                        yMax = max;
                    }
                    else
                    {
                        yMin = yMin > min ? min : yMin;
                        yMax = yMax > max ? yMax : max;
                    }
                }
                pane.XAxis.Scale.TextLabels = GetDateTimeTextFormat(textLabels).ToArray();
                GraphHelper.SetYAxisStep(pane, yMin, yMax, YAxisLength, 0.5);
                GraphHelper.SetXAxisStep(pane, 1, textLabels.Count, YAxisLength, XAxisVisibility.DateAndTime);
                pane.XAxis.Scale.FontSpec.Size = 11.0f;
                if (GraphHelper.rec != RectangleF.Empty)
                {
                    pane.Chart.Rect = rec;
                }
            }
        }
        public static void SetXAxisAsDataPoints(GraphPane pane, List<SuperDevice> TagList, bool isMarked)
        {
            if (TagList != null && TagList.Count > 0)
            {
                SuperDevice sd = DoSearchMostDataPointList(TagList);
                pane.CurveList.Clear();
                pane.GraphObjList.Clear();
                pane.XAxis.Type = AxisType.Linear;
                double yMin = .0, min = .0;
                double yMax = .0, max = .0;
                LineItem myItem;
                for (int i = 0; i < TagList.Count; i++)
                {
                    string sntn = TagList[i].SerialNumber + "_" + TagList[i].TripNumber;
                    myItem = AddDataPointLine(TagList[i].tempList, sntn, i + 1, 1, TagList[i].tempList.Count);
                    if (isMarked)
                    {
                        myItem.Symbol.MarkedSymbolList = GetMarkedIndexList(TagList[i].tempList);
                        myItem.Symbol.MarkedSymbolType = SymbolType.Triangle;
                        myItem.Symbol.MarkedSymbolColor = LineColor[i];
                    }
                    pane.CurveList.Add(myItem);
                    min = FindMaxYValue(myItem.Points, false);
                    max = FindMaxYValue(myItem.Points, true);
                    if (yMax ==.0&&yMin == .0)
                    {
                        yMin = min ;
                        yMax = max;
                    }
                    else
                    {
                        yMin = yMin > min ? min : yMin;
                        yMax = yMax > max ? yMax : max;
                    }
                }
                pane.XAxis.Scale.TextLabels = SetTextLabels(sd.tempList,Convert.ToDouble(sd.LogInterval), XAxisVisibility.DataPoints, 1, sd.tempList.Count, YAxisLength);
                GraphHelper.SetYAxisStep(pane, yMin, yMax, YAxisLength, 0.5);
                GraphHelper.SetXAxisStep(pane, 1, sd.tempList.Count, YAxisLength, XAxisVisibility.DataPoints);
                pane.XAxis.Scale.FontSpec.Size = 11.0f;
                if (GraphHelper.rec != RectangleF.Empty)
                {
                    pane.Chart.Rect = rec;
                }
            }
        }
        /// <summary>
        /// 查找当前所绘制的曲线中elapse time最长的list
        /// </summary>
        /// <param name="pane"></param>
        /// <param name="TagList"></param>
        /// <returns></returns>
        private static List<PointKeyValue> DoSearchElapsedTimeList(GraphPane pane,List<SuperDevice> TagList,int start,int end)
        {
            List<PointKeyValue> result = new List<PointKeyValue>();
            //查找最长的tag
            double LongestElapsedTime = TagList.Max(a => Convert.ToDouble(a.LogInterval) * a.tempList.Count);
            var v = (from p in TagList
                     where Convert.ToDouble(p.LogInterval) * p.tempList.Count == LongestElapsedTime
                     select p).ToList();
            string labelText = string.Empty;
            if (v!=null&&v.Count>0)
            {
                labelText = v.First().SerialNumber + "_" + v.First().TripNumber;
            }
            for (int i = 0; i < TagList.Count; i++)
            {
                if (TagList[i].SerialNumber + "_" + TagList[i].TripNumber == labelText)
                {
                    //CurveItem item = pane.CurveList.Find(p => p.Label.Text == labelText);
                    //if (item == null)
                    //    continue;
                    //else
                    //{
                        int startX = 1;
                        int endX = TagList[i].tempList.Count;
                        result = GetTempPointList(pane, TagList[i], XAxisVisibility.ElapsedTime, start, end);
                        break;
                    //}
                }
            }
            return result;
        }
        /// <summary>
        /// 查找elapsed time 最长的设备记录
        /// </summary>
        /// <param name="TagList"></param>
        /// <returns></returns>
        private static SuperDevice DoSearchLongestElapsedTimeList(List<SuperDevice> TagList)
        {
            double LongestElapsedTime=TagList.Max(a => Convert.ToDouble(a.LogInterval) * a.tempList.Count);
            var v = (from p in TagList
                            where Convert.ToDouble(p.LogInterval) * p.tempList.Count == LongestElapsedTime
                            select p).ToList();
            if (v != null && v.Count > 0)
                return v.First();
            else
                return null;
        }
        private static DateTime DoSearchDatetimeList(List<SuperDevice> TagList, bool isMax)
        {
            DateTime dt = DateTime.MinValue;
            if (TagList == null || TagList.Count < 0)
                return dt;
            else
            {
                for (int i = 0; i < TagList.Count; i++)
                {
                    if (!isMax)
                    {
                        DateTime now = TagList[i].tempList.Select(p => p.PointTime.ToLocalTime()).Min();
                        if (dt == DateTime.MinValue)
                            dt = now;
                        else if (dt > now)
                            dt = now;
                    }
                    else
                    {
                        DateTime now = TagList[i].tempList.Select(p => p.PointTime.ToLocalTime()).Max();
                        if (dt == DateTime.MinValue)
                            dt = now;
                        else if (dt < now)
                            dt = now;
                    }
                }
                return dt;
            }
        }
        private static SuperDevice DoSearchMostDataPointList(List<SuperDevice> TagList)
        {
            return TagList.OrderBy(p => p.tempList.Count).Last();
        }
        /// <summary>
        /// 查找devic逝去总时间在longest中第几个点
        /// </summary>
        private static int FindIndexOfLongestCurveItem(SuperDevice device,SuperDevice longest)
        {
            //get the elapsed time
            double elapsed = .0;
            elapsed = (device.tempList.Count(p=>!p.IsMark) - 1) * Convert.ToDouble(device.LogInterval);
            DateTime FirstLogTime = longest.tempList.First().PointTime;
            List<PointKeyValue> points= longest.tempList.Where(p => (p.PointTime - FirstLogTime).TotalSeconds >=elapsed &&!p.IsMark).ToList();
            if (points.Count > 0)
                return longest.tempList.IndexOf(points.First()) + 1;
            else
                return -1;
        }
        private static int FindIndexOfLongestCurveItem(List<PointKeyValue> current, List<PointKeyValue> target,double logInterval)
        {
            //get the elapsed time
            double elapsed = .0;
            elapsed = (current.Count(p => !p.IsMark) - 1) * logInterval;
            DateTime FirstLogTime = target.First().PointTime;
            List<PointKeyValue> points = target.Where(p => (p.PointTime - FirstLogTime).TotalSeconds >= elapsed && !p.IsMark).ToList();
            if (points.Count > 0)
                return target.IndexOf(points.First()) + 1;
            else
                return target.Count+1;
        }
        private static LineItem AddElapsedTimeLine(List<PointKeyValue> tempList, string sntn, int index, int endX)
        {
            PointPairList pair = new PointPairList();
            double step = (endX) / (double)tempList.Count;
            for (int i = 0; i < tempList.Count; i++)
            {
                pair.Add(i * step + 1, tempList[i].PointTemp);
            }
            return GenerateCurveItem(sntn, pair, index, 1, endX);
        }
        private static LineItem AddDateTimeLine(List<PointKeyValue> tempList,string sntn, int index, int startX, int endX)
        {
            PointPairList pair = new PointPairList();
            double step = (endX+1 - startX) / (double)tempList.Count;
            for (int i = 0; i < tempList.Count; i++)
            {
                pair.Add(i * step + startX, tempList[i].PointTemp);
            }
            return GenerateCurveItem(sntn, pair,index, startX, endX);
        }
        private static LineItem AddDataPointLine(List<PointKeyValue> tempList, string sntn, int index, int startX, int endX)
        {
            PointPairList pair = new PointPairList();
            double step = (endX+1 - startX) / (double)tempList.Count;
            for (int i = 0; i < tempList.Count; i++)
            {
                pair.Add(i * step + startX, tempList[i].PointTemp);
            }
            return GenerateCurveItem(sntn, pair, index, startX, endX);
        }
        private static LineItem GenerateCurveItem(string label, IPointList pair, int index,int xMin,int xMax)
        {
            LineItem myCurve = new LineItem(label, pair, LineColor[index - 1], SymbolType.UserDefined);
            myCurve.Symbol = new Symbol(SymbolType.UserDefined, LineColor[index - 1]);
            myCurve.Symbol.Fill = new Fill(GetImageFromColor(LineColor[index - 1], ((char)(64 + index)).ToString()), System.Drawing.Drawing2D.WrapMode.TileFlipXY);
            myCurve.Symbol.IsAntiAlias = true;
            myCurve.Symbol.IsDrawSingleSymbol = true;//设置只画一个点
            int iSymbol = (int)CalcXAxisStep(xMin, xMax, YAxisLength) * index;
            myCurve.Symbol.DrawSymbolIndex = iSymbol >= xMax ? xMax - 1 : iSymbol;
            myCurve.Symbol.UserSymbol = new System.Drawing.Drawing2D.GraphicsPath();
            RectangleF recf = new RectangleF(-.75F, -.9F, 2F, 1.8F);
            myCurve.Symbol.UserSymbol.AddRectangle(recf);
            myCurve.Symbol.Border.IsVisible = false;
            myCurve.Line.IsAntiAlias = true;
            myCurve.Line.Width = 1.5F;
            return myCurve;
        }
        private static List<DateTime> GetDataTimeLabels(DateTime min,DateTime max,double interval,List<PointKeyValue> markPoint)
        {
            List<DateTime> list = new List<DateTime>();
            int markCount = 0;
            if (markPoint != null && markPoint.Count > 0)
            {
                list.AddRange(markPoint.Select(p => p.PointTime.ToLocalTime()).ToList());
                markCount = markPoint.Count;
            }
            DateTime start = min;
            while (start <= max)
            {
                list.Add(start);
                start = start.AddSeconds(interval);
            }
            if (list.Count > 0)
            {
                if (list.Last() < max)
                {
                    list.Add(start);
                }
                else if (list.Last() > max)
                {
                    start = start.AddSeconds(-interval);
                }
                double xMax = .0;
                double floating = (YAxisLength - (list.Count - 1) % YAxisLength);
                xMax = list.Count + (floating == YAxisLength ? 0 : floating);
                for (int i = 1; i <= floating && floating != YAxisLength; i++)
                {
                    list.Add(start.AddSeconds(i * interval));
                }
            }
            return list.OrderBy(p => p).ToList();
        }
        /// <summary>
        /// 当坐标轴是datetime类型时获取不同曲线在最大最小值中的起点和
        /// 终点坐标值
        /// </summary>
        /// <param name="device"></param>
        /// <param name="list">textlabel值</param>
        /// <param name="isStart"></param>
        /// <returns></returns>
        private static int FindThePosInTheXAxis(List<PointKeyValue> tempList,List<DateTime> list,bool isStart)
        {
            DateTime keyTime = DateTime.MinValue;
            List<DateTime> results;
            if (isStart)
            {
                keyTime=tempList.Select(p => p.PointTime).First().ToLocalTime();
                results= list.Where(p => p <= keyTime).ToList();
                if (results != null && results.Count > 0)
                {
                    return list.IndexOf(results.Last())+1;
                }
            }
            else
            {
                keyTime = tempList.Select(p => p.PointTime).Last().ToLocalTime();
                results = list.Where(p => p >= keyTime).ToList();
                if (results != null && results.Count > 0)
                {
                    return list.IndexOf(results.First()) + 1;
                }
            }
            return -1;
        }
        public static List<int> GetMarkedIndexList(List<PointKeyValue> tempPoints)
        {
            List<int> indexList = new List<int>();
            if (tempPoints != null && tempPoints.Count > 0)
            {
                List<PointKeyValue> markdedList=tempPoints.Where(p => p.IsMark).ToList();
                if (markdedList != null && markdedList.Count > 0)
                {
                    markdedList.ForEach(p =>
                    {
                        int index=tempPoints.IndexOf(p);
                        if (index > -1)
                        {
                            indexList.Add(index);
                        }
                    });
                }
            }
            return indexList;
        }
        #region mark重新绘图
        /// <summary>
        /// 根据图上的线条获取应该重新绘制的记录点信息compare时
        /// </summary>
        public static void ReDrawCompareCurveItem(GraphPane pane, List<SuperDevice> tags, XAxisVisibility selection, bool isMarked)
        {
            switch (selection)
            {
                case XAxisVisibility.DateAndTime:
                    ShowDateTimeMark(pane, tags, isMarked);
                    break;
                case XAxisVisibility.ElapsedTime:
                    ShowElapsedTimeMark(pane, tags, isMarked);
                    break;
                case XAxisVisibility.DataPoints:
                    ShowDataPointMark(pane, tags, isMarked);
                    break;
                default:
                    break;
            }
        }
        public static void ReDrawUnCompareCurveItem(GraphPane pane, SuperDevice tag, XAxisVisibility selection, bool isMarked)
        {
            if (tag != null && pane.CurveList.Count > 0 && tag.tempList.Count(p => p.IsMark) > 0)
            {
                string labelText = tag.SerialNumber + "_" + tag.TripNumber;
                CurveItem item = pane.CurveList.Find(p => p.Label.Text == labelText);
                
                if (item == null)
                {
                    return;
                }
                else
                {
                    List<PointKeyValue> list = GetTempList(pane, tag, selection, 1, item.Points.Count);
                    if (list == null || list.Count <= 0)
                        return;
                    else
                    {
                        pane.CurveList.Remove(item);
                        Color globalColor = GetColorFromProfile(Common.GlobalProfile.TempCurveRGB);
                        LineItem myItem = GenerateCurveItem(labelText, item.Points, 1, 1, item.NPts);
                        myItem.Line.Color = globalColor;
                        if (isMarked)
                        {
                            myItem.Symbol.MarkedSymbolList = GetMarkedIndexList(list);
                            myItem.Symbol.MarkedSymbolType = SymbolType.Triangle;
                            myItem.Symbol.MarkedSymbolColor = globalColor;
                        }
                        pane.CurveList.Add(myItem);
                    }
                }
            }
        }
        public static void ShowDateTimeMark(GraphPane pane, List<SuperDevice> tags, bool isMarked)
        {
            if (tags != null && tags.Count > 0)
            {
                //pane.CurveList.Clear();
                pane.GraphObjList.Clear();
                List<DateTime> textLabels = pane.XAxis.Scale.TextLabels.Select(p => Convert.ToDateTime(p)).ToList();
                LineItem myItem;
                for (int i = 0; i < tags.Count; i++)
                {
                    string labelText = tags[i].SerialNumber + "_" + tags[i].TripNumber;
                    CurveItem item= pane.CurveList.Find(p => p.Label.Text == labelText);
                    if (item == null)
                        continue;
                    int startX=(int)item.Points[0].X;
                    int endX = (int)item.Points[item.NPts - 1].X;
                    List<PointKeyValue> list = GetTempPointList(pane, tags[i], XAxisVisibility.DateAndTime, startX, endX);
                    if (list.Count == 0)
                        continue;
                    else
                    {
                        pane.CurveList.Remove(item);
                        int first = FindThePosInTheXAxis(list, textLabels, true);
                        int last = FindThePosInTheXAxis(list, textLabels, false);
                        //myItem = AddDateTimeLine(list, labelText, i + 1, first, last);
                        myItem = GenerateCurveItem(labelText, item.Points, i + 1, first, last);
                        if (isMarked)
                        {
                            myItem.Symbol.MarkedSymbolList = GetMarkedIndexList(list);
                            myItem.Symbol.MarkedSymbolType = SymbolType.Triangle;
                            myItem.Symbol.MarkedSymbolColor = LineColor[i];
                        }
                        pane.CurveList.Add(myItem);
                    }
                }
            }
        }
        public static void ShowDataPointMark(GraphPane pane, List<SuperDevice> tags, bool isMarked)
        {
            if (tags != null && tags.Count > 0)
            {
                //SuperDevice sd = DoSearchMostDataPointList(TagList);
                pane.GraphObjList.Clear();
                LineItem myItem;
                for (int i = 0; i < tags.Count; i++)
                {
                    string labelText = tags[i].SerialNumber + "_" + tags[i].TripNumber;
                    CurveItem item = pane.CurveList.Find(p => p.Label.Text == labelText);
                    if (item == null)
                        continue;
                    int startX = (int)item.Points[0].X;
                    int endX = (int)Math.Ceiling(item.Points[item.NPts - 1].X);
                    List<PointKeyValue> list = GetTempPointList(pane, tags[i], XAxisVisibility.DataPoints, startX, endX);
                    if (list.Count == 0)
                        continue;
                    else
                    {
                        pane.CurveList.Remove(item);
                        //myItem = AddDataPointLine(list, labelText, i + 1, 1, list.Count);
                        myItem = GenerateCurveItem(labelText, item.Points, i + 1, 1, list.Count);
                        if (isMarked)
                        {
                            myItem.Symbol.MarkedSymbolList = GetMarkedIndexList(list);
                            myItem.Symbol.MarkedSymbolType = SymbolType.Triangle;
                            myItem.Symbol.MarkedSymbolColor = LineColor[i];
                        }
                        pane.CurveList.Add(myItem);
                    }
                }
            }
        }
        public static void ShowElapsedTimeMark(GraphPane pane, List<SuperDevice> tags, bool isMarked)
        {
            
            if (tags != null && tags.Count > 0)
            {
                pane.GraphObjList.Clear();
                LineItem myItem;
                for (int i = 0; i < tags.Count; i++)
                {
                    string labelText = tags[i].SerialNumber + "_" + tags[i].TripNumber;
                    CurveItem item = pane.CurveList.Find(p => p.Label.Text == labelText);
                    if (item == null)
                        continue;
                    int startX = (int)item.Points[0].X;
                    int endX = (int)Math.Ceiling(item.Points[item.NPts - 1].X);
                    List<PointKeyValue> list = GetTempPointList(pane, tags[i], XAxisVisibility.ElapsedTime, startX, endX);
                    if (list.Count == 0)
                        continue;
                    else
                    {
                        pane.CurveList.Remove(item);
                        myItem = GenerateCurveItem(labelText, item.Points, i + 1, 1, list.Count);
                        if (isMarked)
                        {
                            myItem.Symbol.MarkedSymbolList = GetMarkedIndexList(list);
                            myItem.Symbol.MarkedSymbolType = SymbolType.Triangle;
                            myItem.Symbol.MarkedSymbolColor = LineColor[i];
                        }
                        pane.CurveList.Add(myItem);
                    }
                }
            }
        }
        private static void GetMaxMinTempValue(ref double yMax, ref double yMin, string maxLimit,string minLimit)
        {
            if (!string.IsNullOrEmpty(maxLimit))
                yMax = yMax >= Convert.ToDouble(maxLimit) ? yMax : Convert.ToDouble(maxLimit);
            else
            {
                if (!string.IsNullOrEmpty(minLimit))
                {
                    yMax = yMax >= Convert.ToDouble(minLimit) ? yMax : Convert.ToDouble(minLimit);
                }
            }
            if (!string.IsNullOrEmpty(minLimit))
                yMin = yMin <= Convert.ToDouble(minLimit) ? yMin : Convert.ToDouble(minLimit);
            else
            {
                if (!string.IsNullOrEmpty(maxLimit))
                {
                    yMin = yMin <= Convert.ToDouble(maxLimit) ? yMin : Convert.ToDouble(maxLimit);
                }
            }
        }
        #endregion
    }
    public enum XAxisVisibility{DateAndTime,ElapsedTime,DataPoints}
}
