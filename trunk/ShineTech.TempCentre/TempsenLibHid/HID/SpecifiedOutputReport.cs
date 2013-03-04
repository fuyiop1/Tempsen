using System;
using System.Collections.Generic;
using System.Text;

namespace UsbLibrary
{
    public class SpecifiedOutputReport : OutputReport
    {
        public SpecifiedOutputReport(HIDDevice oDev) : base(oDev) {

        }

        public bool SendData(byte[] data)
        {
            byte[] arrBuff = Buffer; //new byte[Buffer.Length];
            for (int i = 1; i < arrBuff.Length; i++)
            {
                if (i <= data.Length)
                    arrBuff[i] = data[i-1];
                else
                    arrBuff[i] = 0;
            }
            arrBuff[0] = 0;

            //Buffer = arrBuff;

            //returns false if the data does not fit in the buffer. else true
            if (arrBuff.Length < data.Length)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
