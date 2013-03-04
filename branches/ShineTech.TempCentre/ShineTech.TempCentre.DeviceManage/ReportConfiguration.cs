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
    public partial class ReportConfiguration : Form
    {
        private ReportConfigurationUI _UI;
        public ReportConfiguration()
        {
            InitializeComponent();
            _UI = new ReportConfigurationUI(this);
        }
    }
}
