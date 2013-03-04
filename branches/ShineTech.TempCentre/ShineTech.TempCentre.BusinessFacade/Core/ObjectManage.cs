using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShineTech.TempCentre.DAL;
using System.Xml.Serialization;
using System.Reflection;
using TempSen;
namespace ShineTech.TempCentre.BusinessFacade
{
    public class ObjectManage
    {
        private static IEntity user;
        public static IEntity GetInstance(int id) 
        {
            switch (id)
            {
                case 1:
                    if (user == null)
                        user = new UserInfo();
                    break;
                case 2:
                    if (user == null)
                        user = new Policy();
                    break;
                case 3:
                    if (user == null)
                        user = new Meanings();
                    break;
            }
            return user;
        }
        private static SuperDevice tag;
        private static DevicePDF _DeviceNew = new DevicePDF();
        private static TempSen.device _DeviceSingleUse=new device (10);

        public static TempSen.device DeviceSingleUse
        {
            get { return ObjectManage._DeviceSingleUse; }
            set { ObjectManage._DeviceSingleUse = value; }
        }
        public static DevicePDF DeviceNew
        {
            get { return ObjectManage._DeviceNew; }
            set { ObjectManage._DeviceNew = value; }
        }
        public static SuperDevice Tag
        {
            get 
            { 
                return ObjectManage.tag;
            }
        }
        public static SuperDevice GetDeviceInstance(DeviceType deviceType)
        {
            DeviceType d;
            switch (deviceType)
            {
                case DeviceType.ITAGSingleUse:
                    d = DeviceType.ITAGSingleUse;
                    break;
                case DeviceType.ITAGPDF:
                case DeviceType.ITAG3Pro:
                case DeviceType.ITAG3:
                    d = DeviceType.ITAGPDF;
                    break;
                case DeviceType.ELogTE:
                case DeviceType.ELogTI:
                    d = DeviceType.ELogTI;
                    break;
                default:
                    d = DeviceType.Tempod;
                    break;
            }
            string assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;
            string factoryName = assemblyName + "." + d.ToString() + "Factory";
            Factory obj = Assembly.Load(assemblyName).CreateInstance(factoryName) as Factory;
            tag = obj.Creator();
            return tag;
        }
        /// <summary>
        /// 反序列化pointinfo
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="point"></param>
        /// <returns></returns>
        public static List<T> DeserializePointKeyValue<T>(PointInfo point) where T : class
        {
            List<T> t = new List<T>();
            if (point != null && point.Points != null)
            {
                using (System.IO.MemoryStream ms = new System.IO.MemoryStream(point.Points))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(List<T>));
                    t = serializer.Deserialize(ms) as List<T>;
                }
            }
            return t;
        }
        public static List<PointKeyValue> GetTempListByUnit(List<PointKeyValue> list,string tempunit)
        {
            if(!string.IsNullOrEmpty(tempunit))
                list.ForEach(p =>
                {
                    p.PointTemp = Convert.ToDouble(Common.TransferTemp(tempunit, p.PointTemp.ToString()));
                });
            return list;
        }
        public static void SetDevice(SuperDevice device)
        {
            tag = device;
        }
    }
    public enum DeviceType { ITAGSingleUse = 10, ITAGPDF=100, ITAG3=101, ITAG3Pro=102, Tempod=200,TempodS=201,TempodPro=202,TempodProS=203, ELogTI=301, ELogTE=300 }
}
