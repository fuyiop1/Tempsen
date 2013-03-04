using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShineTech.TempCentre.DAL;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Globalization;
namespace ShineTech.TempCentre.BusinessFacade
{
    [Serializable]
    [XmlInclude(typeof(System.Drawing.Bitmap))]
    public class ITAGSingleUse:SuperDevice
    {
        #region override properity
        private string _ProductName="ITAG-SingleUse";
        public override string ProductName
        {
            get { return _ProductName; }
            set { _ProductName = value; }
        }
        private string _Model = "TAGS";
        public override string Model
        {
            get { return _Model; }
            set { _Model = value; }
        }
        private string _Description = string.Empty;
        public override string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }
        private string _TripNumber = "";
        public override string TripNumber
        {
            get { return _TripNumber; }
            set { _TripNumber = value; }
        }
        private string _StartModel = "Manual Start";
        public override string StartModel
        {
            get { return _StartModel; }
            set { _StartModel = value; }
        }
        private string _HighAlarmType = "Single Event";
        public override string HighAlarmType
        {
            get { return _HighAlarmType; }
            set { _HighAlarmType = value; }
        }
        private string _LowAlarmType = "Single Event";
        public override string LowAlarmType
        {
            get { return _LowAlarmType; }
            set { _LowAlarmType = value; }
        }
        private string _CurrentStatus = "Stopped";
        public override string CurrentStatus
        {
            get { return _CurrentStatus; }
            set { _CurrentStatus = value; }
        }
        private string _DeviceName = "ITAG-SingleUse";
        public override string DeviceName
        {
            get { return _DeviceName; }
            set { _DeviceName = value; }
        }
        private int _DeviceID = 10;

        public override int DeviceID
        {
            get { return _DeviceID; }
            set { _DeviceID = value; }
        }
        #endregion

        [NonSerialized]
        private DeviceBLL _bll;
        public ITAGSingleUse()
        {
            _bll = new DeviceBLL();
        }
        public override bool Connect(int code)
        {
            try
            {
                bool result = true;
                if (ObjectManage.DeviceSingleUse == null)
                {
                    ObjectManage.DeviceSingleUse = new TempSen.device(code);
                    result=ObjectManage.DeviceSingleUse.connectDevice();
                }
                if (ObjectManage.DeviceSingleUse != null)
                {
                    Config = ObjectManage.DeviceSingleUse.getConfig();
                    this.AlarmSet = ObjectManage.DeviceSingleUse.getAlarmSet();
                    this.Analysis = ObjectManage.DeviceSingleUse.getAnalysis();
                    this.Record = FahrenheitOrCelsius(ObjectManage.DeviceSingleUse.getRecord(2));
                    OtherInfo = ObjectManage.DeviceSingleUse.getOtherInfo();
                    this.Summary();
                    this.CalculateAlarmEvents();
                    Common.IsConnectCompleted = true;
                    if (Config[0] == "OK")
                        result = true;
                    else
                        result = false;
                    return result;
                }
                Common.IsConnectCompleted = true;
                return false;
            }
            catch (Exception exc)
            {
                Common.IsConnectCompleted = true;
                return false;
            }
        }
        /// <summary>
        /// 取得摘要的值
        /// </summary>
        public override void Summary()
        {
            if (Config[0] == "OK")
            {
                SerialNumber = Config[1];
                _TripNumber = SerialNumber;
                LogCycle = TempsenFormatHelper.ConvertSencondToFormmatedTime( TempsenFormatHelper.GetSecondsFromFormatString(Config[3]));
                LogInterval = TempsenFormatHelper.GetSecondsFromFormatString(Config[2]);
                TempUnit = AlarmSet[1];
                LogStartDelay =TempsenFormatHelper.ConvertSencondToFormmatedTime( TempsenFormatHelper.GetSecondsFromFormatString(Config[4]));
                LoggingStart = Convert.ToDateTime(Config[6]);
                //LoggingEnd = Convert.ToDateTime(Config[6]).AddSeconds(int.Parse(Config[2].Substring(0, Config[2].Length - 1)) * int.Parse(Config[5]));
                DataPoints = Convert.ToInt32(Config[5]);
                AlarmHighDelay = TempsenFormatHelper.ConvertSencondToFormmatedTime( TempsenFormatHelper.GetSecondsFromFormatString(AlarmSet[4]));
                AlarmLowDelay = TempsenFormatHelper.ConvertSencondToFormmatedTime( TempsenFormatHelper.GetSecondsFromFormatString(AlarmSet[4]));
                Battery = OtherInfo[1];
                AlarmMode = 1;
                if (AlarmSet[2] == "--" && AlarmSet[3] == "--")
                {
                    AlarmMode = 0;
                }
                else if (AlarmSet[2] != "--" && AlarmSet[3] == "--")
                {
                    AlarmHighLimit = AlarmSet[2];
                }
                else if (AlarmSet[3] != "--" && AlarmSet[2] == "--")
                {
                    AlarmLowLimit = AlarmSet[3];
                }
                else
                {
                    AlarmHighLimit = AlarmSet[2];
                    AlarmLowLimit = AlarmSet[3];
                }
                if (SerializePointInfo())
                {
                    points.FirstPoint = points.StartTime = this.LoggingStart;
                    if (tempList != null && tempList.Count > 0)
                    {
                        LoggingEnd = tempList[tempList.Count - 1].PointTime;
                    }
                    TripLength = TempsenFormatHelper.ConvertSencondToFormmatedTime((LoggingEnd - LoggingStart).TotalSeconds.ToString());
                    points.EndTime = this.LoggingEnd;
                    points.TripLength = this.TripLength;
                    if (AlarmSet[2] != "--")
                    {
                        HighAlarmEvents = tempList.Count(p => p.PointTemp >= Convert.ToDouble(AlarmSet[2]));
                        var v = tempList.Where(p => p.PointTemp >= Convert.ToDouble(AlarmSet[2]));
                        HighAlarmFirstTrigged = v.ToList().Count == 0 ? DateTime.MinValue.ToString() : v.Min(p => p.PointTime).ToString();
                    }
                    if (AlarmSet[3] != "--")
                    {
                        LowAlarmEvents = tempList.Count(p => p.PointTemp <= Convert.ToDouble(AlarmSet[3]));
                        var v = tempList.Where(p => p.PointTemp <= Convert.ToDouble(AlarmSet[3]));
                        LowAlarmFirstTrigged = v.ToList().Count == 0 ? DateTime.MinValue.ToString() : v.Min(p => p.PointTime).ToString();
                    }
                    HighAlarmTotalTimeAbove = TempsenFormatHelper.ConvertSencondToFormmatedTime((Convert.ToDouble(LogInterval) * (HighAlarmEvents-1)).ToString());
                    LowAlarmTotalTimeBelow = TempsenFormatHelper.ConvertSencondToFormmatedTime((Convert.ToDouble(LogInterval) * (LowAlarmEvents-1)).ToString());
                    if (tempList != null && tempList.Count > 0)
                    {
                        AverageC = Math.Round(tempList.Select(p => p.PointTemp).Average(), 1).ToString();
                        MKT = Common.CalcMKT(tempList.Select(p => (int)(p.PointTemp * 100)).ToList());
                        var s = (from p in tempList
                                 where p.PointTemp == tempList.Select(t => t.PointTemp).Max()
                                 select p.PointTemp.ToString() + "°" + (this.TempUnit) + "@" + p.PointTime.ToString(Common.GetDefaultDateTimeFormat(), CultureInfo.InvariantCulture));
                        HighestC = s.ToList().Count == 0 ? "" : s.ToList().First();
                        s = (from p in tempList
                             where p.PointTemp == tempList.Select(t => t.PointTemp).Min()
                             select p.PointTemp + "°" + (this.TempUnit) + "@" + p.PointTime.ToString(Common.GetDefaultDateTimeFormat(), CultureInfo.InvariantCulture));
                        LowestC = s.ToList().Count == 0 ? "" : s.First();
                        points.HighestC = this.HighestC;
                        points.LowestC = this.LowestC;
                        points.AVGTemp = AverageC;
                        points.MKT = this.MKT;
                    }
                }
                
            }
            return;
        }


        private void CalculateAlarmEvents()
        {
            int tempIntForTryParse = 0;
            double tempDoubleForTryParse = 0;

            IList<PointKeyValue> points = this.tempList;

            double highLimit = double.MaxValue;
            if (double.TryParse(this.AlarmHighLimit, out tempDoubleForTryParse))
            {
                highLimit = double.Parse(this.AlarmHighLimit);
            }
            double lowLimit = double.MinValue;
            if (double.TryParse(this.AlarmLowLimit, out tempDoubleForTryParse))
            {
                lowLimit = double.Parse(this.AlarmLowLimit);
            }
            int highAlarmDelay = int.MaxValue;
            highAlarmDelay = int.Parse(TempsenFormatHelper.GetSecondsFromFormatString(this.AlarmHighDelay));
            int lowAlarmDelay = int.MaxValue;
            lowAlarmDelay = int.Parse(TempsenFormatHelper.GetSecondsFromFormatString(this.AlarmLowDelay));
            AlarmType highAlarmType = AlarmType.Single;
            if ("Cumulative Event".Equals(this.HighAlarmType, StringComparison.InvariantCultureIgnoreCase))
            {
                highAlarmType = AlarmType.Cumulative;
            }
            AlarmType lowAlarmType = AlarmType.Single;
            if ("Cumulative Event".Equals(this.LowAlarmType, StringComparison.InvariantCultureIgnoreCase))
            {
                lowAlarmType = AlarmType.Cumulative;
            }
            int logInterval = 0;
            if (int.TryParse(this.LogInterval, out tempIntForTryParse))
            {
                logInterval = int.Parse(this.LogInterval);
            }

            int overHighLimitEvents = 0;
            int belowLowLimitEvents = 0;
            DateTime highAlarmFirstTriggered = DateTime.MinValue;
            DateTime lowAlarmFirstTriggered = DateTime.MinValue;
            int overHighLimitTotalTime = 0; 
            int belowLowLimitTotalTime = 0;

            int overHighLimitCumulativePoints = 0;
            int belowLowLimitCumulativePoints = 0;

            DateTime timeOfTheLastPointWithoutMark = DateTime.MinValue;
            foreach (var item in points)
            {
                if (!item.IsMark)
                {
                    if (item.PointTemp >= highLimit)
                    {
                        overHighLimitCumulativePoints++;
                        if (belowLowLimitCumulativePoints > 0)
                        {
                            belowLowLimitEvents++;
                            int actualIntervals = belowLowLimitCumulativePoints - 1;
                            if (actualIntervals == 0)
                            {
                                actualIntervals = 1;
                            }
                            belowLowLimitTotalTime += logInterval * actualIntervals;
                            belowLowLimitCumulativePoints = 0;
                        }

                        int actualIntervalsToCheckAlarm = overHighLimitCumulativePoints - 1;
                        int actualTotalTimeToCheckAlarm = 0;
                        if (actualIntervalsToCheckAlarm == 0)
                        {
                            actualIntervalsToCheckAlarm = 1;
                        }
                        actualTotalTimeToCheckAlarm += logInterval * actualIntervalsToCheckAlarm;
                        if (AlarmType.Cumulative == highAlarmType)
                        {
                            actualTotalTimeToCheckAlarm += overHighLimitTotalTime;
                        }
                        if (highAlarmFirstTriggered == DateTime.MinValue && actualTotalTimeToCheckAlarm >= highAlarmDelay)
                        {
                            highAlarmFirstTriggered = item.PointTime;
                        }
                    }
                    else if (item.PointTemp <= lowLimit)
                    {
                        belowLowLimitCumulativePoints++;
                        if (overHighLimitCumulativePoints > 0)
                        {
                            overHighLimitEvents++;
                            int actualIntervals = overHighLimitCumulativePoints - 1;
                            if (actualIntervals == 0)
                            {
                                actualIntervals = 1;
                            }
                            overHighLimitTotalTime += logInterval * actualIntervals;
                            overHighLimitCumulativePoints = 0;
                        }

                        int actualIntervalsToCheckAlarm = belowLowLimitCumulativePoints - 1;
                        int actualTotalTimeToCheckAlarm = 0;
                        if (actualIntervalsToCheckAlarm == 0)
                        {
                            actualIntervalsToCheckAlarm = 1;
                        }
                        actualTotalTimeToCheckAlarm += logInterval * actualIntervalsToCheckAlarm;
                        if (AlarmType.Cumulative == lowAlarmType)
                        {
                            actualTotalTimeToCheckAlarm += belowLowLimitTotalTime;
                        }
                        if (lowAlarmFirstTriggered == DateTime.MinValue && actualTotalTimeToCheckAlarm >= lowAlarmDelay)
                        {
                            lowAlarmFirstTriggered = item.PointTime;
                        }
                    }
                    else
                    {
                        if (belowLowLimitCumulativePoints > 0)
                        {
                            belowLowLimitEvents++;
                            int actualIntervals = belowLowLimitCumulativePoints - 1;
                            if (actualIntervals == 0)
                            {
                                actualIntervals = 1;
                            }
                            belowLowLimitTotalTime += logInterval * actualIntervals;
                            belowLowLimitCumulativePoints = 0;

                            int actualIntervalsToCheckAlarm = belowLowLimitCumulativePoints - 1;
                            int actualTotalTimeToCheckAlarm = 0;
                            if (actualIntervalsToCheckAlarm == 0)
                            {
                                actualIntervalsToCheckAlarm = 1;
                            }
                            actualTotalTimeToCheckAlarm += logInterval * actualIntervalsToCheckAlarm;
                            if (AlarmType.Cumulative == lowAlarmType)
                            {
                                actualTotalTimeToCheckAlarm += belowLowLimitTotalTime;
                            }
                            if (lowAlarmFirstTriggered == DateTime.MinValue && actualTotalTimeToCheckAlarm >= lowAlarmDelay)
                            {
                                lowAlarmFirstTriggered = item.PointTime;
                            }
                        }
                        if (overHighLimitCumulativePoints > 0)
                        {
                            overHighLimitEvents++;
                            int actualIntervals = overHighLimitCumulativePoints - 1;
                            if (actualIntervals == 0)
                            {
                                actualIntervals = 1;
                            }
                            overHighLimitTotalTime += logInterval * actualIntervals;
                            overHighLimitCumulativePoints = 0;

                            int actualIntervalsToCheckAlarm = overHighLimitCumulativePoints - 1;
                            int actualTotalTimeToCheckAlarm = 0;
                            if (actualIntervalsToCheckAlarm == 0)
                            {
                                actualIntervalsToCheckAlarm = 1;
                            }
                            actualTotalTimeToCheckAlarm += logInterval * actualIntervalsToCheckAlarm;
                            if (AlarmType.Cumulative == highAlarmType)
                            {
                                actualTotalTimeToCheckAlarm += overHighLimitTotalTime;
                            }
                            if (highAlarmFirstTriggered == DateTime.MinValue && actualTotalTimeToCheckAlarm >= highAlarmDelay)
                            {
                                highAlarmFirstTriggered = item.PointTime;
                            }
                        }
                    }
                    timeOfTheLastPointWithoutMark = item.PointTime;
                }
            }
            if (belowLowLimitCumulativePoints > 0)
            {
                belowLowLimitEvents++;
                int actualIntervals = belowLowLimitCumulativePoints - 1;
                if (actualIntervals == 0)
                {
                    actualIntervals = 1;
                }
                belowLowLimitTotalTime += logInterval * actualIntervals;
                belowLowLimitCumulativePoints = 0;

                int actualIntervalsToCheckAlarm = belowLowLimitCumulativePoints - 1;
                int actualTotalTimeToCheckAlarm = 0;
                if (actualIntervalsToCheckAlarm == 0)
                {
                    actualIntervalsToCheckAlarm = 1;
                }
                actualTotalTimeToCheckAlarm += logInterval * actualIntervalsToCheckAlarm;
                if (AlarmType.Cumulative == lowAlarmType)
                {
                    actualTotalTimeToCheckAlarm += belowLowLimitTotalTime;
                }
                if (lowAlarmFirstTriggered == DateTime.MinValue && actualTotalTimeToCheckAlarm >= lowAlarmDelay)
                {
                    lowAlarmFirstTriggered = timeOfTheLastPointWithoutMark;
                }
            }
            if (overHighLimitCumulativePoints > 0)
            {
                overHighLimitEvents++;
                int actualIntervals = overHighLimitCumulativePoints - 1;
                if (actualIntervals == 0)
                {
                    actualIntervals = 1;
                }
                overHighLimitTotalTime += logInterval * actualIntervals;
                overHighLimitCumulativePoints = 0;

                int actualIntervalsToCheckAlarm = overHighLimitCumulativePoints - 1;
                int actualTotalTimeToCheckAlarm = 0;
                if (actualIntervalsToCheckAlarm == 0)
                {
                    actualIntervalsToCheckAlarm = 1;
                }
                actualTotalTimeToCheckAlarm += logInterval * actualIntervalsToCheckAlarm;
                if (AlarmType.Cumulative == highAlarmType)
                {
                    actualTotalTimeToCheckAlarm += overHighLimitTotalTime;
                }
                if (highAlarmFirstTriggered == DateTime.MinValue && actualTotalTimeToCheckAlarm >= highAlarmDelay)
                {
                    highAlarmFirstTriggered = timeOfTheLastPointWithoutMark;
                }
            }

            this.HighAlarmEvents = overHighLimitEvents;
            this.LowAlarmEvents = belowLowLimitEvents;
            this.HighAlarmFirstTrigged = highAlarmFirstTriggered.ToString(Common.GetDefaultDateTimeFormat(), CultureInfo.InvariantCulture);
            this.LowAlarmFirstTrigged = lowAlarmFirstTriggered.ToString(Common.GetDefaultDateTimeFormat(), CultureInfo.InvariantCulture);
            this.HighAlarmTotalTimeAbove = TempsenFormatHelper.ConvertSencondToFormmatedTime(overHighLimitTotalTime.ToString());
            this.LowAlarmTotalTimeBelow = TempsenFormatHelper.ConvertSencondToFormmatedTime(belowLowLimitTotalTime.ToString());
            AlarmHighStatus = HighAlarmEvents > 0 ? "Alarm" : "OK";
            AlarmLowStatus = LowAlarmEvents > 0 ? "Alarm" : "OK";
        }


        /// <summary>
        /// 计算最长时间、最短时间连续时间
        /// </summary>
        /// <returns></returns>
        public double AlarmTimeCalc(List<PointKeyValue> point, bool IsHighest)
        {
            List<double> seconds = new List<double>();//散列点联系的时间
            PointKeyValue info;
            double continuation;
            double interal;
            interal = continuation = Convert.ToDouble(LogInterval);
            var v = new List<PointKeyValue>();
            if (IsHighest)
            {
                v = point.Where(p => p.PointTemp >= Convert.ToDouble(AlarmSet[2])).ToList();
                 var varible = (from p in v
                                where p.PointTime.ToString() == HighAlarmFirstTrigged
                                select p);
                 if (varible.ToList().Count == 0)
                     return 0;
                 info = varible.First();
            }
            else
            {
                v = point.Where(p => p.PointTemp <= Convert.ToDouble(AlarmSet[3])).ToList();
                var varible=(from p in v
                        where p.PointTime.ToString() == LowAlarmFirstTrigged
                        select p);
                if (varible.ToList().Count == 0)
                    return 0;
                info = varible.First();
            }
            v.ForEach(p =>
                {
                    if ((p.PointTime - info.PointTime).TotalSeconds == continuation)
                    {
                        continuation = continuation + interal;
                    }
                    else if ((p.PointTime - info.PointTime).TotalSeconds != 0)
                    {
                        seconds.Add(continuation);
                        continuation = Convert.ToDouble(LogInterval);
                        info = p;
                    }
                });
            if (seconds.Count == 0)
                seconds.Add(continuation);
            return seconds.Max();
        }

        public  bool SerializePointInfo()
        {
            try
            {
                tempList.Clear();
                
                Dictionary<DateTime, double> dic = new Dictionary<DateTime, double>();
                Record.ToList().ForEach(p =>
                    {
                        string[] s = p.Split(new char[1] { ',' });
                        tempList.Add(new PointKeyValue() { PointTime = Convert.ToDateTime(s[0]), PointTemp = Convert.ToDouble(s[1]) });
                       
                    });
                int id = _bll.GetPointPKValue();
                PointInfo point = new PointInfo();
                point.ID = id + 1;
                point.ProductName = ProductName;
                point.SN = SerialNumber;
                point.TN = TripNumber;
                point.TempUnit = TempUnit;
                point.Remark = DateTime.Now.ToString();
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream())
                {
                    if (tempList.Count > 0)
                    {
                        /*序列化*/
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<PointKeyValue>));
                        xmlSerializer.Serialize(ms, tempList);
                        point.Points = ms.ToArray();
                    }
                }
                points=point;
                return true;
            }
            catch { return false; }
        }
        public sealed override SuperDevice Clone(string datetimeFormat)
        {
            SuperDevice device = new ITAGSingleUse();
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formator = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                formator.Serialize(ms, this);
                ms.Seek(0, SeekOrigin.Begin);
                device = formator.Deserialize(ms) as ITAGSingleUse;
                ms.Close();
            }
            device.AlarmHighLimit = Common.TransferTemp(this.TempUnit, this.AlarmHighLimit);
            device.AlarmLowLimit = Common.TransferTemp(this.TempUnit, this.AlarmLowLimit);
            if (device.tempList.Count > 0)
            {
                device.tempList.ForEach(p =>
                {
                    p.PointTemp = Convert.ToDouble(Common.TransferTemp(this.TempUnit, p.PointTemp.ToString()));
                });
                //device.AverageC = Math.Round(device.tempList.Select(p => p.PointTemp).Average(), 1).ToString();
                device.AverageC = Common.TransferTemp(this.TempUnit, device.AverageC);
                device.MKT = Common.CalcMKT(device.tempList.Select(p => (int)(p.PointTemp * 100)).ToList()); 
                List<string> high = device.HighestC.Split(new string[] { "°", "@" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (high != null && high.Count > 0)
                    device.HighestC = Common.TransferTemp(this.TempUnit, high.First()) + "°" + (this.TempUnit == "C" ? "F" : "C") + "@" + high.Last();
                List<string> low = device.LowestC.Split(new string[] { "°", "@" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if (low != null && low.Count > 0)
                    device.LowestC = Common.TransferTemp(this.TempUnit, low.First()) + "°" + (this.TempUnit == "C" ? "F" : "C") + "@" + low.Last();
            }
            if (this.TempUnit == "C")
            {
                device.TempUnit = "F";
            }
            else
                device.TempUnit = "C";
            
            return device;
        }
        private string[] FahrenheitOrCelsius(string [] record)
        {
            List<string> list = record.ToList();
            if (AlarmSet[1] == "F")
            {
                list.Clear();
                record.ToList().ForEach(p =>
                {
                    string[] s = p.Split(new char[1] { ',' });
                    p = s[0] +","+ Common.TransferTemp("C", s[1]);
                    list.Add(p);
                });
            }
            return list.ToArray();
        }
    }
}
