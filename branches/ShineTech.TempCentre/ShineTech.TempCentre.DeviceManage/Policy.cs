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
    public partial class Policy : Form
    {
        PolicyUI ui;
        public Policy()
        {
            InitializeComponent();
            ui = new PolicyUI(this);
        }
    }
}
