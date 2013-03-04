using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShineTech.TempCentre.SQLiteHelper;
using System.Data.SQLite;
using ShineTech.TempCentre.DAL;
using System.Reflection;
using System.Data.Common;
using ShineTech.TempCentre.Platform;
namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            //SQLiteHelper.CreateDataBase();
            //SQLiteConnection con = new SQLiteConnection("data source=test1.db");
            //con.SetPassword("pass");
            //con.Open();
            //con.Close();
            //SQLiteHelper.ExecuteDataSet("select * from device",null);
            //Device d = new Device();
            //d.ID = 1;
            //Type type = d.GetType();
            //var properityInfo = type.GetProperties().Where(p=>p.CanWrite).ToList();
            //properityInfo.ForEach(p =>
            //{
            //   object o=p.GetValue(d,p.GetIndexParameters());
            //   Console.WriteLine(o);
            //});
            IDataProcessor processor = new DeviceProcessor();
            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("PID", 10001);
            Device d = processor.QueryOne<Device>("select * from device where pid=@PID", dic);
            d.GetType().GetProperties().Where(p => p.CanRead).ToList().ForEach(p =>
            {
                Console.WriteLine("the column of {0} is {1}",p.Name,p.GetValue(d,p.GetIndexParameters()));
            });
            //修改
            //Dictionary<string, object> dic = new Dictionary<string, object>();
            //dic.Add("text","test"+DateTime.Now.ToString());
            //processor.ExecuteNonQuery("update device set remark=@text",dic);
            //d = processor.QueryOne<Device>("select * from device", null);
            //d.GetType().GetProperties().Where(p => p.CanRead).ToList().ForEach(p =>
            //{
            //    Console.WriteLine("the column of {0} is {1}", p.Name, p.GetValue(d, p.GetIndexParameters()));
            //});
            //Utils.WriteToXML();
            //string s = Utils.Decode(Utils.ReadPwdFromXML(), Utils.ReadKeyFromXML(), Utils.ReadIVFromXML());
            processor.OnCreated();
            processor.Query<Device>("select * from device",null);
            Console.ReadKey();
        }
    }
}
