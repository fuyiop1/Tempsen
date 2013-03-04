using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.BusinessFacade;
namespace ShineTech.TempCentre.DeviceManage
{
    public partial class Meanings : Form
    {
        private MeaningsUI _ui;
        public Meanings()
        {
            InitializeComponent();
            _ui = new MeaningsUI(this);
        }
        public Meanings(Dictionary<int,string> mean)
        {
            InitializeComponent();
            _ui = new MeaningsUI(this,mean);
        }
    }
}
