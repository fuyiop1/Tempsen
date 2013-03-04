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
    public partial class UserWizard : Form
    {
        private UserWizardUI _ui;
        public UserWizard()
        {
            InitializeComponent();
            //_ui = new UserWizardUI(this);
        }
        public UserWizard(bool flag):this()
        {
            _ui = new UserWizardUI(this,flag);
        }
        /*自定义属性*/
        
    }
}
