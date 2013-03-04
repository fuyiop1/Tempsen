using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ShineTech.TempCentre.Platform.DialogForm
{
    public partial class MessageBox : Form
    {
        public MessageBox()
        {
            InitializeComponent();
            #if PRO
            this.Text = "TempsenCentre Pro";
            #endif
        }
        public void Show(string text, string caption, MessageBoxButtons buttons, MessageBoxIcon icon)
        {

        }
    }
}
