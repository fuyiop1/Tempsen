using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShineTech.TempCentre.DAL;
///<summary>
///CLR Ver : 4.0.30319.225
///CreateBy: wangfei
///CreateOn:12/26/2011 2:16:51 PM
///FileName:HistoryRecordData
///</summary>
namespace ShineTech.TempCentre.BusinessFacade.ViewModel
{
    public class HistoryRecordData:IEntity
    {
        public string SerialNum { get; set; }
        public string TripNum { get; set; }
        public string DESCS { get; set; }
        public DateTime CreateTime { get; set; }
        public int SignatureTimes { get; set; }
        public string AlarmStatus { get; set; }
    }
}
