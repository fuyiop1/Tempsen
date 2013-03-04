using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShineTech.TempCentre.BusinessFacade
{
    public partial class ProgressStatus : Form
    {
        public int ProgressValue 
         {
             get { return this.bar.Value; }
             set {
                 if (value > 100)
                     bar.Value = 100;
                 else
                     bar.Value = value;
             }
         }
        public ProgressStatus()
        {
            InitializeComponent();
        }
        public bool Increase(int nValue)
        {

            if (nValue > 0)
            {

                if (bar.Value + nValue < bar.Maximum)
                {

                    bar.Value += nValue;
                    return true;
                }

                else
                {

                    bar.Value = bar.Maximum;
                    this.Close();
                    return false;

                }

            }

            return false;

        }
        public void Send()
        {
            for (int i = 0; i < 100; i++)
            {
                Increase(i);
            }
        }
    }
}
