using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data;
using Services.Common;

namespace ShineTech.TempCentre.DAL
{
    public class DeviceBLL
    {
        private static ITracing _tracing = TracingManager.GetTracing(typeof(DeviceBLL));
        private IDataProcessor processor;
        public DeviceBLL()
        {
            processor = new DeviceProcessor();
        }
        public int GetPointPKValue()
        {
            object u = processor.QueryScalar("select max(id) from PointInfo", null);
            if (u != null && u.ToString() != string.Empty)
                return Convert.ToInt32(u);
            else
                return 0;
        }
        public bool InsertDevice(Device device)
        {
            return InsertDevice(device, null);
        }
        public bool InsertDevice(Device device, DbTransaction tran)
        {
            return processor.Insert<Device>(device, tran);
        }

        public void DeleteDevice(Device device, DbTransaction tran)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("ID",device.ID);
            processor.ExecuteNonQuery("delete from device where id=@ID", tran, dic);
        }
        public void DeleteDevice(List<Device> list, DbTransaction tran)
        {
            if (list != null && list.Count > 0)
            {
                list.ForEach(p => DeleteDevice(p, tran));
            }
        }
        public List<Device> GetDeviceList()
        {
            return processor.Query<Device>("select * from device", null);
        }
        public Device GetDeviceBySnTn(string sn, string tn)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("sn", sn);
            dic.Add("tn", tn);
            return processor.QueryOne<Device>("select * from Device where SerialNum=@sn and TripNum=@tn", dic);
        }
        /// <summary>
        /// 保存设备信息
        /// </summary>
        /// <returns></returns>
        public bool SaveDeviceInfomation(Device device, PointInfo points, LogConfig log, List<AlarmConfig> alarm,List<DigitalSignature> ds)
        {
            using (System.Data.SQLite.SQLiteConnection conn = SQLiteHelper.SQLiteHelper.CreateConn())
            {
                PointTempBLL _point = new PointTempBLL();
                LogConfigBLL _log = new LogConfigBLL();
                AlarmConfigBLL _alarm = new AlarmConfigBLL();
                ReportEditorBLL _report=new ReportEditorBLL ();
                DigitalSignatureBLL _digital = new DigitalSignatureBLL();
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();
                System.Data.Common.DbTransaction tran = conn.BeginTransaction();
                try
                {
                    //保存设备信息
                    if (InsertDevice(device, tran) &&
                        //保存温度点信息
                    _point.InsertPoint(points, tran) &&
                        //保存log信息
                    _log.InsertLogConfig(log, tran) &&
                        //保存alarm信息
                    _alarm.InsertAlarmConfig(alarm, tran)
                    && _digital.InsertDigitalSignature(ds,tran)
                        )
                    {
                        tran.Commit();
                    }
                    else
                        tran.Rollback();
                }
                catch(Exception ex)
                {
                    tran.Rollback();
                    conn.Close();
                    _tracing.Error(ex, "save data to db failed!");
                    return false;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }

            }
            return true;
        }
        public bool UpdateDeviceInfomation(Device device, PointInfo points, LogConfig log, List<AlarmConfig> alarm)
        {
            using (System.Data.SQLite.SQLiteConnection conn = SQLiteHelper.SQLiteHelper.CreateConn())
            {
                PointTempBLL _point = new PointTempBLL();
                LogConfigBLL _log = new LogConfigBLL();
                AlarmConfigBLL _alarm = new AlarmConfigBLL();
                ReportEditorBLL _report = new ReportEditorBLL();
                DigitalSignatureBLL _digital = new DigitalSignatureBLL();
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();
                System.Data.Common.DbTransaction tran = conn.BeginTransaction();
                try
                {
                    //保存设备信息
                    if (UpdateDevice(device, tran) &&
                        //保存温度点信息
                    _point.UpdatePoint(points, tran) &&
                        //保存log信息
                    _log.UpdateLogConfig(log, tran) &&
                        //保存alarm信息
                    _alarm.UpdateAlarmConfig(alarm, tran)
                        )
                    {
                        tran.Commit();
                    }
                    else
                    {
                        tran.Rollback();
                        return false;
                    }

                }
                catch (Exception ex)
                {
                    tran.Rollback();
                    conn.Close();
                    _tracing.Error(ex, "save data to db failed!");
                    return false;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }

            }
            return true;
        }
        public int GetDevicePKValue()
        {
            object u = processor.QueryScalar("select max(id) from device", null);
            if (u != null && u.ToString() != string.Empty)
                return Convert.ToInt32(u);
            else
                return 0;
        }
        
        
        public bool DeleteDeviceInformation(List<Device> device, List<PointInfo> points, List<LogConfig> log, List<AlarmConfig> alarm,List<DigitalSignature> digital)
        {
            
            //if(_digital.IsExist()
            using (System.Data.SQLite.SQLiteConnection conn = SQLiteHelper.SQLiteHelper.CreateConn())
            {
                PointTempBLL _point = new PointTempBLL();
                LogConfigBLL _log = new LogConfigBLL();
                AlarmConfigBLL _alarm = new AlarmConfigBLL();
                DigitalSignatureBLL _digital = new DigitalSignatureBLL();
                ReportEditorBLL _reportBll = new ReportEditorBLL();
                if (conn.State != System.Data.ConnectionState.Open)
                    conn.Open();
                System.Data.Common.DbTransaction tran = conn.BeginTransaction();
                try
                {
                    //删除设备信息
                    DeleteDevice(device, tran);
                    ////删除温度点信息
                    _point.DeletePointInfo(points, tran);
                    ////删除log信息
                    _log.DeleteLogConfig(log, tran);
                    ////删除alarm信息
                    _alarm.DeleteAlarmConfig(alarm, tran);
                    device.ForEach(p=>_reportBll.DeleteReportEditorBySnTn(p.SerialNum,p.TripNum,tran));
                    
                    _digital.DeleteDigitalSignature(digital, tran);
                    tran.Commit();
                }
                catch(Exception ex)
                {
                    _tracing.Error(ex, "delete the device info failed");
                    tran.Rollback();
                    conn.Close();
                    return false;
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }

            }
            return true;
        }
        public bool IsDeviceInfoExist(Device device)
        {
            string sn = device.SerialNum;
            string tn = device.TripNum;
            string text = "select 1 from device where SerialNum=@sn and TripNum=@tn";
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("sn",sn);
            dic.Add("tn", tn);
            object o = processor.QueryScalar(text, dic);
            if (o != null && o.ToString() != string.Empty)
                return true;
            return false;
        }
        public bool UpdateDevice(Device device, DbTransaction tran)
        {
            Device d = GetDeviceBySnTn(device.SerialNum, device.TripNum);
            device.ID = d.ID;
            return processor.Update<Device>(device, tran);
        }

        public string GetNextTripNumberForDeviceTag(Device device)
        {
            string result = string.Empty;
            if (device != null)
            {
                string sn = device.SerialNum;
                int subStringLength = device.TripNum.Length;
                if (device.TripNum.IndexOf('_') != -1)
                {
                    subStringLength = device.TripNum.IndexOf('_');
                }
                string tnWithOutSubfix = device.TripNum.Substring(0, subStringLength);
                Dictionary<string, object> dic = new Dictionary<string, object>();
                dic.Add("sn", sn);
                string sql = string.Format("select * from device where SerialNum=@sn and TripNum like '{0}%' order by Id desc", tnWithOutSubfix);
                List<Device> deviceList = processor.Query<Device>(sql, dic);
                if (deviceList != null && deviceList.Count > 0)
                {
                    result = "0001";
                    Device firstItem = deviceList[0];
                    if (firstItem.TripNum.Contains('_') && firstItem.TripNum.Length - 1 > firstItem.TripNum.LastIndexOf('_') + 1)
                    {
                        string tripNumSubfix = firstItem.TripNum.Substring(firstItem.TripNum.LastIndexOf('_') + 1);
                        if (!string.IsNullOrEmpty(tripNumSubfix))
                        {
                            int actualSubfix = 0;
                            int.TryParse(tripNumSubfix, out actualSubfix);
                            if (actualSubfix != 0)
                            {
                                actualSubfix++;
                                result = actualSubfix.ToString("d4");
                            }
                        }
                    }
                }
            }
            return result;
        }
        public List<HistoryRecordData> FindHistoryRecordData()
        {
            string text = @"select a.SerialNum,a.TripNum,a.DESCS,b.StartTime LogStartTime,  a.Remark as CreateTime,ifnull(c.SignatureTimes,0) as SignatureTimes
                                  ,case when ifnull(d.nums,-1)>0 then 'Alarm' when ifnull(d.nums,-1)=0 then 'OK' when ifnull(d.nums,-1)=-1 then '' end AlarmStatus
                            from device a 
                            left join PointInfo b on a.SerialNum=b.SN AND a.TripNum=b.TN
                            left join (select SN,TN,COUNT(1) SignatureTimes from DigitalSignature group by  SN,TN) c on a.SerialNum=c.SN AND a.TripNum=c.TN
                            Left join(select SN,TN,SUM(AlarmNumbers) nums from AlarmConfig group by SN,TN) d on a.SerialNum=d.SN AND a.TripNum=d.TN";
            return processor.Query<HistoryRecordData>(text, null);
        }
    }
}
