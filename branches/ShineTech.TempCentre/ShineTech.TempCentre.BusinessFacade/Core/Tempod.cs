using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TempsenLibHid.PDF;
using TempSen;
using ShineTech.TempCentre.DAL;
using System.Xml.Serialization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
///<summary>
///CLR Ver : 4.0.30319.225
///CreateBy: wangfei
///CreateOn:9/9/2011 5:38:57 PM
///FileName:Tempod
///</summary>
namespace ShineTech.TempCentre.BusinessFacade
{
    [Serializable]
    public class Tempod:SuperDevice
    {
        #region override properity
        private string _ProductName = "Tempod";
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
        private string _DeviceName = "Tempod";
        public override string DeviceName
        {
            get { return _DeviceName; }
            set { _DeviceName = value; }
        }
        private int _DeviceID = (int)DeviceType.Tempod;

        public override int DeviceID
        {
            get { return _DeviceID; }
            set { _DeviceID = value; }
        }
       
        #endregion
        [NonSerialized]
        private DeviceBLL _bll;
        public Tempod()
        {
            _bll = new DeviceBLL();
        }
        public override void Summary()
        {
            //throw new NotImplementedException();
        }
        public override SuperDevice Clone(string datetimeFormat)
        {
            SuperDevice device = new Tempod();
            using (MemoryStream ms = new MemoryStream())
            {
                BinaryFormatter formator = new BinaryFormatter(null, new StreamingContext(StreamingContextStates.Clone));
                formator.Serialize(ms, this);
                ms.Seek(0, SeekOrigin.Begin);
                device = formator.Deserialize(ms) as Tempod;
                ms.Close();
            }
            if (AlarmMode == 1)
            {
                if (!string.IsNullOrEmpty(this.AlarmHighLimit))
                    device.AlarmHighLimit = Common.TransferTemp(this.TempUnit, this.AlarmHighLimit);
                if (!string.IsNullOrEmpty(this.AlarmLowLimit))
                    device.AlarmLowLimit = Common.TransferTemp(this.TempUnit, this.AlarmLowLimit);
            }
            else if (AlarmMode == 2)
            {
                if (!string.IsNullOrEmpty(this.A1))
                    device.A1 = Common.TransferTemp(this.TempUnit, this.A1);
                if (!string.IsNullOrEmpty(this.A2))
                    device.A2 = Common.TransferTemp(this.TempUnit, this.A2);
                if (!string.IsNullOrEmpty(this.A3))
                    device.A3 = Common.TransferTemp(this.TempUnit, this.A3);
                if (!string.IsNullOrEmpty(this.A4))
                    device.A4 = Common.TransferTemp(this.TempUnit, this.A4);
                if (!string.IsNullOrEmpty(this.A5))
                    device.A5 = Common.TransferTemp(this.TempUnit, this.A5);
            }
            if (device.tempList.Count > 0)
            {
                device.tempList.ForEach(p =>
                {
                    p.PointTemp = Convert.ToDouble(Common.TransferTemp(this.TempUnit, p.PointTemp.ToString()));
                });
                device.AverageC = Common.TransferTemp(this.TempUnit, device.AverageC);
                device.MKT = Common.TransferTemp(this.TempUnit,device.MKT);
                List<string> high = device.HighestC.Split(new string[] { "°", "@" }, StringSplitOptions.RemoveEmptyEntries).ToList();
                if(high!=null&&high.Count>0)
                    device.HighestC = Common.TransferTemp(this.TempUnit, high.First()) +"°" + (this.TempUnit == "C" ? "F" : "C") + "@" +high.Last();
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
    }
}
