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
    public partial class ChangeDetail : Form
    {
        private ChangeDetailUI _ui;
        public ChangeDetail()
        {
            InitializeComponent();
            _ui = new ChangeDetailUI(this);
        }
    }
}
