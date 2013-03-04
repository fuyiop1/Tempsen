using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ShineTech.TempCentre.DAL;
using System.Windows.Forms;

namespace ShineTech.TempCentre.BusinessFacade
{
    public class MeaningsUI
    {
        private System.Windows.Forms.TextBox tbMean;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;

        private IDataProcessor processor;
        private Form form;
        private Dictionary<int,string> meaning = null;
        public MeaningsUI(Form form)
        {
            this.form = form;
            this.ConstructForms(form);
            this.InitEvents();
        }
        public MeaningsUI(Form form, Dictionary<int, string> mean)
        {
            this.form = form;
            this.meaning = mean;
            this.ConstructForms(form);
            this.InitEvents();
            this.tbMean.Text = mean==null ? "" : mean.First().Value;
        }
        private void ConstructForms(Form form)
        {
            btnOK = form.Controls.Find("btnOK", true)[0] as Button;
            btnCancel = form.Controls.Find("btnCancel", true)[0] as Button;
            tbMean = form.Controls.Find("tbMean", true)[0] as TextBox;
        }
        private void InitEvents()
        {
            this.btnCancel.Click+=new EventHandler(delegate( object sender,EventArgs args ){
                form.Close();
            });
            //ok event
            this.btnOK.Click += new EventHandler(OK);
            this.tbMean.KeyPress+=new KeyPressEventHandler(delegate( object sender,KeyPressEventArgs args ){
                if (args.KeyChar == 13)
                    OK(sender, args);
            });
        }
        private void OK(object sender, EventArgs args)
        {
            if (processor == null)
                processor = new DeviceProcessor();
            if (this.tbMean.Text == string.Empty)
                MessageBox.Show("Please input the meaning");
            else
            {
                if (meaning == null)
                {
                    Meanings mean = new Meanings();
                    object o = processor.QueryScalar("SELECT MAX(ID) FROM Meanings", null);
                    mean.Id = o != null && o.ToString() != string.Empty ? Convert.ToInt32(o) + 1 : 1;
                    mean.Desc = this.tbMean.Text.TrimEnd();
                    mean.Remark = DateTime.Now.ToString();
                    if (processor.Insert<Meanings>(mean, null))
                        form.DialogResult = DialogResult.OK;
                    else
                    {
                        MessageBox.Show("Saved Failure");
                        form.DialogResult = DialogResult.No;
                    }
                }
                else
                {
                    Meanings mean = new Meanings();
                    mean.Id = meaning.First().Key;
                    mean.Desc = this.tbMean.Text.TrimEnd();
                    mean.Remark = DateTime.Now.ToString();
                    if (processor.Update<Meanings>(mean, null))
                        form.DialogResult = MessageBox.Show("Saved Successfully");
                    else
                    {
                        MessageBox.Show("Saved Failure");
                        form.DialogResult = DialogResult.No;
                    }
                }
            }
        }
    }
}
