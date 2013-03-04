using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShineTech.TempCentre.BusinessFacade
{
    class CustomComboBox : ComboBox
    {
        protected override void WndProc(ref Message m)
        {
            if (m.Msg == 0x020A)
            {
            }
            else
            {
                base.WndProc(ref m);
            }

        }
    }
}
