using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShineTech.TempCentre.DAL;
using ShineTech.TempCentre.Platform;
///<summary>
///CLR Ver : 4.0.30319.225
///CreateBy: wangfei
///CreateOn:12/26/2011 2:06:56 PM
///FileName:HistoryRecordDataViewModel
///</summary>
namespace ShineTech.TempCentre.BusinessFacade.ViewModel
{
    public class HistoryRecordDataViewModel
    {
        private IList<HistoryRecordData> m_HistoryData;
        private DeviceBLL m_DeviceBll;
        private IList<HistoryRecordData> m_CurrentHistoryData;
        public HistoryRecordDataCondition Condition { get; set; }
        public HistoryRecordDataCondition CurrentCondition { get; set; }
        public IList<HistoryRecordData> HistoryData
        {
            get
            {
                if (m_HistoryData == null)
                {
                    m_CurrentHistoryData=m_HistoryData = m_DeviceBll.FindHistoryRecordData().OrderByDescending(p => Convert.ToDateTime(p.CreateTime)).ToList();
                    Condition = new HistoryRecordDataCondition();
                    if (m_HistoryData.Count > 0)
                    {
                        Condition.Start = m_HistoryData.Min(p => Convert.ToDateTime(p.CreateTime));
                        Condition.End = m_HistoryData.Max(p => Convert.ToDateTime(p.CreateTime));
                    }
                    else
                    {
                        Condition.Start = DateTime.Now;
                        Condition.End = DateTime.Now;
                    }
                    Condition.RecordName = Infrastructure.SearchRecordConst;
                    CurrentCondition = Condition.Copy();
                }
                else
                {
                    if (Condition != null && Condition != CurrentCondition)
                    {
                        var v = from p in m_HistoryData
                                where Convert.ToDateTime(p.CreateTime).ToLocalTime().Date >= Condition.Start.Date &&
                                Convert.ToDateTime(p.CreateTime).ToLocalTime().Date <= Condition.End.Date &&
                              (p.ToString().IndexOf(Condition.RecordName) == 0 || Condition.RecordName == Infrastructure.SearchRecordConst)
                                select p;
                        m_CurrentHistoryData = v.ToList();
                        CurrentCondition = Condition.Copy();
                    }
                }
                return m_CurrentHistoryData;
            }
        }
        public HistoryRecordDataViewModel(DeviceBLL bll)
        {
            m_DeviceBll = bll;
        }
        public void Clear()
        {
            m_HistoryData = null;
        }
        
    }

    ///<summary>
    ///查询条件
    ///</summary>
    public class HistoryRecordDataCondition
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string RecordName { get; set; }
        public static bool operator ==(HistoryRecordDataCondition condition1, HistoryRecordDataCondition condition2)
        {

            return Equals(condition1,condition2);
        }
        public static bool operator !=(HistoryRecordDataCondition condition1, HistoryRecordDataCondition condition2)
        {
            return !Equals(condition1, condition2);
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (GetType() != obj.GetType())
                return false;
            HistoryRecordDataCondition condition2 = (HistoryRecordDataCondition)obj;
            if (Start == condition2.Start
             && End == condition2.End
             && RecordName == condition2.RecordName)
                return true;
            else
                return false;
        }
        public HistoryRecordDataCondition Copy()
        {
            HistoryRecordDataCondition con = new HistoryRecordDataCondition();
            con.End = End;
            con.Start = Start;
            con.RecordName = RecordName;
            return con;
        }
    }
}
