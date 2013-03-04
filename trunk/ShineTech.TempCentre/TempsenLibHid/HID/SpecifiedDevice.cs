using System;
using System.Collections.Generic;
using System.Text;
using TempSenLib;

namespace UsbLibrary
{
    public class DataRecievedEventArgs : EventArgs
    {
        public readonly byte[] data;

        public DataRecievedEventArgs(byte[] data)
        {
            this.data = data;
        }
    }

    public class DataSendEventArgs : EventArgs
    {
        public readonly byte[] data;

        public DataSendEventArgs(byte[] data)
        {
            this.data = data;
        }
    }

    public class PDFCmd
    {
        public static string WriteRequest = "685572";
        public static string WriteResponse = "685673";
        public static string ReadRequest = "68AA31";
        public static string ReadResponse = "68AB32";
        public static string QueryRequest = "68C0";
        public static string QueryResponse = "68C1";
        public static string StopRecordRequest = "68C200000100";
        public static string StopRecordResponse = "68C3000001";


    }

    public delegate void DataRecievedEventHandler(object sender, DataRecievedEventArgs args);
    public delegate void DataSendEventHandler(object sender, DataSendEventArgs args);

    public class SpecifiedDevice : HIDDevice
    {
        public event DataRecievedEventHandler DataRecieved;
        public event DataSendEventHandler DataSend;

        public override InputReport CreateInputReport()
        {
            return new SpecifiedInputReport(this);
        }

        public static SpecifiedDevice FindSpecifiedDevice()
        {
            return (SpecifiedDevice)FindDevice(typeof(SpecifiedDevice));
        }

        protected override void HandleDataReceived(InputReport oInRep)
        {
            // Fire the event handler if assigned
            if (DataRecieved != null)
            {
                try
                {
                    SpecifiedInputReport report = (SpecifiedInputReport)oInRep;
                    string datastring = Utils.ToHexString(report.Data).Substring(2,80);
                    if (CheckonResult(datastring))
                    {
                        DataRecieved(this, new DataRecievedEventArgs(Utils.HexToByte(datastring)));
                    }
                }
                catch { }
            }
        }

        public void SendData(byte[] data)
        {
            SpecifiedOutputReport oRep = new SpecifiedOutputReport(this);	// create output report
            oRep.SendData(data);	// set the lights states
            try
            {
                Write(oRep,3); // write the output report
                if (DataSend != null)
                {
                    DataSend(this, new DataSendEventArgs(data));
                }
            }catch (HIDDeviceException ex)
            {
                // Device may have been removed!
            }catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        //protected override void Dispose(bool bDisposing)
        //{
        //    if (bDisposing)
        //    {
        //        //this.Dispose();
        //    }
        //    base.Dispose(bDisposing);
        //}


        #region for tempsen

        #region page operation .

        public string ReadAll(int maxpage)
        {
            for (int i = 0; i < 600; i++)
            {
                if(i<=maxpage)
                    for (int j = 0; j < 8; j++)
                        ReadRow(i, j);
            }
            return "";
        }
        public string QueryStatus()
        {
            string msg = Utils.IntToHexString(0, 34);
            msg = AddFCS(msg);
            SendData(Utils.HexToByte(PDFCmd.QueryRequest + msg + "AA"));
            return "";
        }
        public string StopRecord()
        {
            string msg = Utils.IntToHexString(0, 32);
            msg = AddFCS(msg);
            SendData(Utils.HexToByte(PDFCmd.StopRecordRequest + msg + "AA"));
            return "";
        }


        public string ReadRow(int pageNo, int rowNo)
        {
            return ReadRow(pageNo, rowNo,3);
        }
        private string ReadRow(int pageNo, int row, int trytime)
        {
            //spRead();
            string msg = Utils.IntToHexString(pageNo, 2) + Utils.IntToHexString(row, 1) + Utils.IntToHexString(0, 32);
            msg = AddFCS(msg);
            SendData(Utils.HexToByte(PDFCmd.ReadRequest + msg + "55"));
            string tresult = "";// spRead();
            //if (CheckonResult(tresult))
            //{
            //    tresult = tresult.Substring(6, 32);
            //    return tresult;
            //}
            //else if (trytime > 0)
            //{
            //    Console.WriteLine("onrecord retry ");
            //    return ReadPageRow(pageNo, row, trytime--);
            //}
            //else
            //    throw new Exception("Get record faild ,usb error .");
            return tresult;
        }

        public string WriteRow(int pageNo, int RowNo, string pageValue)
        {
            return WriteRow(pageNo, RowNo, pageValue, 3);
        }
        private string WriteRow(int pageNo, int RowNo, string pageValue, int trytime)
        {
            string msg = Utils.IntToHexString(pageNo, 2) + Utils.IntToHexString(RowNo, 1) + pageValue;
            msg = AddFCS(msg);
            Console.WriteLine(string.Format("write page{0} row{1} values: "+ PDFCmd.WriteRequest + msg + "AA",pageNo,RowNo));
            SendData(Utils.HexToByte(PDFCmd.WriteRequest + msg + "AA"));
            string tresult = "";
            
            return tresult;
        }

        #endregion

        private bool CheckonResult(string result)
        {
            //if ((!result.EndsWith("55")) && (!result.EndsWith("AA")))
            //    return false;
            if (!result.StartsWith("68"))
                return false;
            if (result.Length != 80)
                return false;
            //if (!CheckFCS(result))
            //    return false;

            return true;
        }
        public bool CheckFCS(string result)
        {
            byte[] bytes = Utils.HexToByte(result);
            byte b = 0;
            for (int i = 3; i < bytes.Length - 2; i++)
                b = (byte)(b ^ bytes[i]);

            if (b != bytes[bytes.Length - 2])
                return false;
            return true;

        }


        private string AddFCS(string str)
        {
            byte[] bytes = Utils.HexToByte(str);
            byte b = 0;
            foreach (byte bt in bytes)
                b = (byte)(b ^ bt);

            return str + b.ToString("X2");
        }

        #endregion


    }
}
