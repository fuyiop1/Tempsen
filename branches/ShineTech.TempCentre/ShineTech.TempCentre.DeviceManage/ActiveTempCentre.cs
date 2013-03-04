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
    public partial class ActiveTempCentre : Form
    {
        public bool Validated { get; set; }
        TrialValidationUI m_Ui = new TrialValidationUI();
        public ActiveTempCentre()
        {
            InitializeComponent();
            Validated = false;
            this.Text = Common.FormTitle;
            key1.Focus();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (key1.Text.Length == 4 && key2.Text.Length == 4 && key3.Text.Length == 4 && key4.Text.Length == 4)
            //if (key1.Text.Length == 16)
            {
                string nubmer = string.Format("{0}{1}{2}{3}", key1.Text, key2.Text, key3.Text, key4.Text);
                if (m_Ui.VerifyMode7(nubmer))
                {
                    if (m_Ui.RemoveTrialVersion())
                    {
                        Validated = true;
                        this.Close();
                    }
                }
            }
        }
        private void OnTextChanged(object sender, EventArgs e)
        {
            MaskedTextBox mtb = sender as MaskedTextBox;
            if (mtb.Text.Length >= 4)
            {
                switch (mtb.Name)
                {
                    case "key1":
                        key2.Focus();
                        break;
                    case "key2":
                        key3.Focus();
                        break;
                    case "key3":
                        key4.Focus();
                        break;
                    case "key4":
                        button1.Focus();
                        break;
                    default:
                        break;
                }
            }
        }
    }
}
