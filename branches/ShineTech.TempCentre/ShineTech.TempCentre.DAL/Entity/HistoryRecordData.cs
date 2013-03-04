using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
///<summary>
///CLR Ver : 4.0.30319.225
///CreateBy: wangfei
///CreateOn:12/26/2011 2:16:51 PM
///FileName:HistoryRecordData
///</summary>
namespace ShineTech.TempCentre.DAL
{
    public class HistoryRecordData:IEntity
    {
        [Column(Name = "SerialNum", DbType = DbType.String)]
        public string SerialNum { get; set; }
        [Column(Name = "TripNum", DbType = DbType.String)]
        public string TripNum { get; set; }
        [Column(Name = "DESCS", DbType = DbType.String)]

        public string DESCS { get; set; }
        [Column(Name = "LogStartTime", DbType = DbType.DateTime)]
        public DateTime LogStartTime { get; set; }
        [Column(Name = "CreateTime", DbType = DbType.DateTime)]
        public string CreateTime { get; set; }
        [Column(Name = "SignatureTimes", DbType = DbType.Int32)]
        public Int64 SignatureTimes { get; set; }
        [Column(Name = "AlarmStatus", DbType = DbType.String)]
        public string AlarmStatus { get; set; }
        public override string ToString()
        {
            return string.Format("{0}_{1}", SerialNum, TripNum);
        }
    }
}
