using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.Platform;

namespace ShineTech.TempCentre.BusinessFacade.DeviceControl
{
    public partial class InputBoxDialog : Form
    {
        private string inputBoxText;

        public string InputBoxText
        {
            get
            {
                return this.inputBoxText;
            }
            set
            {
                this.tbInput.Text = value;
            }
        }

        public InputBoxDialog(string title, string tipText, bool isPasswordBox)
        {
            InitializeComponent();
            if (isPasswordBox)
            {
                this.tbInput.PasswordChar = '*';
            }
            this.labelTip.Text = tipText;
            this.Text = title;
            //this.Text = Common.FormTitle;
            if (!isPasswordBox)
            {
                InitEvents();
            }
        }

        private void InitEvents()
        {
            //非法字符处理事件
            this.tbInput.TextChanged += new EventHandler((sender, e) =>
            {
                Utils.IsInputTextValid(this.tbInput);
            });
            
        }
        
        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.tbInput.Text))
            {
            }
            else
            {
                this.inputBoxText = this.tbInput.Text;
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void InputBoxDialog_Shown(object sender, EventArgs e)
        {
            this.tbInput.Focus();
        }

        
    }

    
}
