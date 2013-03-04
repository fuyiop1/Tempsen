using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.BusinessFacade;
using ShineTech.TempCentre.Platform;

namespace ShineTech.TempCentre.DeviceManage
{
    public partial class Options : Form
    {
        private OptionsUI _ui;
        private bool isConfigTakeEffectNextTime;
        public event EventHandler OptionChangeEvent;
        public Options()
        {
            InitializeComponent();
            _ui = OptionsUI.CreateInstance();
            InitEvents();
            InitOptions();
            this.Text = Common.FormTitle;
        }
        public void InitEvents()
        {
            this.btnCancel.Click += new EventHandler((sender, args) =>
            {
                this.Close();
            });
            this.Load += new EventHandler((sender, args) =>
            {
                this.tbTempCurve.Text =_ui.GetTextFromRGB(lbTempRgb.BackColor.R,lbTempRgb.BackColor.G,lbTempRgb.BackColor.B);
                this.tbIdealRange.Text = _ui.GetTextFromRGB(lbRangeRgb.BackColor.R, lbRangeRgb.BackColor.G, lbRangeRgb.BackColor.B);
                this.tbAlarmLimit.Text = _ui.GetTextFromRGB(lbLimitRgb.BackColor.R, lbLimitRgb.BackColor.G, lbLimitRgb.BackColor.B);
            });
            this.lbTempRgb.MouseClick += new MouseEventHandler((sender, args) =>
            {
                lbTempRgb.BackColor = _ui.GetColorFromPallete(lbTempRgb.BackColor);
                this.tbTempCurve.Text = _ui.GetTextFromRGB(lbTempRgb.BackColor.R, lbTempRgb.BackColor.G, lbTempRgb.BackColor.B);
            });
            this.lbLimitRgb.MouseClick += new MouseEventHandler((sender, args) =>
            {
                lbLimitRgb.BackColor = _ui.GetColorFromPallete(lbLimitRgb.BackColor);
                this.tbAlarmLimit.Text = _ui.GetTextFromRGB(lbLimitRgb.BackColor.R, lbLimitRgb.BackColor.G, lbLimitRgb.BackColor.B);
            });
            this.lbRangeRgb.MouseClick += new MouseEventHandler((sender, args) =>
            {
                lbRangeRgb.BackColor = _ui.GetColorFromPallete(lbRangeRgb.BackColor);
                this.tbIdealRange.Text = _ui.GetTextFromRGB(lbRangeRgb.BackColor.R, lbRangeRgb.BackColor.G, lbRangeRgb.BackColor.B);
            });
            this.tbTempCurve.KeyDown += new KeyEventHandler((sender, args) =>
            {
                if(args.KeyCode==Keys.Enter)
                    SetBackColor(tbTempCurve, lbTempRgb);
            });
            this.tbIdealRange.KeyDown += new KeyEventHandler((sender, args) =>
            {
                if (args.KeyCode == Keys.Enter)
                    SetBackColor(tbIdealRange, lbRangeRgb);
            });
            this.tbAlarmLimit.KeyDown += new KeyEventHandler((sender, args) =>
            {
                if (args.KeyCode == Keys.Enter)
                    SetBackColor(tbAlarmLimit, lbLimitRgb);
            });
            this.btnApply.Click += new EventHandler((sender, arg) =>
            {
                string format = cmbDateFormat.SelectedItem.ToString() + " " + cmbTimeFormat.SelectedItem.ToString();
                _ui.SaveTheGraphOption(tbTempCurve.Text, tbAlarmLimit.Text, tbIdealRange.Text
                                     ,cbShowLimit.Checked, cbShowMark.Checked, cbFillRange.Checked,format, this.isConfigTakeEffectNextTime);
                this.Close();
                if (OptionChangeEvent != null)
                    OptionChangeEvent(this, null);
               
            });
            this.cmbDateDelimiter.SelectedValueChanged += new EventHandler((sender, args) =>
            {
                this.SetSeparator(cmbDateDelimiter.SelectedItem.ToString(), "");
            });
        }
        private void InitOptions()
        {
            string curveRgb, limitRgb, rangeRgb, format;
            bool isShowLimit, isShowMark, isFillRange;
            _ui.GetCurrentUserOption(out  curveRgb, out  limitRgb, out  rangeRgb
                                        , out  isShowLimit, out  isShowMark, out  isFillRange,out format);
            this.tbAlarmLimit.Text = limitRgb;
            this.tbTempCurve.Text = curveRgb;
            this.tbIdealRange.Text = rangeRgb;

            SetBackColor(tbTempCurve, lbTempRgb);
            SetBackColor(tbIdealRange, lbRangeRgb);
            SetBackColor(tbAlarmLimit, lbLimitRgb);

            this.cbFillRange.Checked = isFillRange;
            this.cbShowLimit.Checked = isShowLimit;
            this.cbShowMark.Checked = isShowMark;

            this.initEventForConfigTakeEffectNextTimeChanged();

            List<string> datetime = format.Split(new char[]{' '}).ToList();
            this.SetSeparator(GetSepatator(datetime.First()), datetime.First());
            string timeformat = "";
            datetime.ForEach(p =>
            {
                if (p != datetime.First())
                {
                    timeformat = timeformat + p + " ";
                }
            });            
            cmbTimeFormat.Text = timeformat.Trim();

        }
        private void SetBackColor(TextBox box, Label label)
        {
            if (_ui.ValidateRgb(box.Text))
            {
                string text = box.Text;
                List<int> list = text.Split(new char[] { ',' }).ToList().Select(p => Convert.ToInt32(p)).ToList();
                label.BackColor = _ui.GetColorFromHex(list[0], list[1], list[2]);
            }
            else
            {
                Utils.ShowMessageBox(Messages.RgbInvalid, Messages.TitleError);
            }
        }
        private void SetSeparator(string split,string text)
        {
            if (cmbDateFormat.Items.Count <= 0)
            {
                cmbDateFormat.Items.Clear();
                cmbDateFormat.Items.Add(string.Format("yyyy{0}MM{0}dd", split));
                cmbDateFormat.Items.Add(string.Format("yy{0}MM{0}dd", split));
                cmbDateFormat.Items.Add(string.Format("yy{0}M{0}d", split));
                cmbDateFormat.Items.Add(string.Format("MM{0}dd{0}yyyy", split));
                cmbDateFormat.Items.Add(string.Format("MM{0}dd{0}yy", split));
                cmbDateFormat.Items.Add(string.Format("M{0}d{0}yy", split));
                cmbDateFormat.Items.Add(string.Format("dd{0}MM{0}yyyy", split));
                cmbDateFormat.Items.Add(string.Format("dd{0}MM{0}yy", split));
                cmbDateFormat.Items.Add(string.Format("d{0}M{0}yy", split));
                cmbDateFormat.Text = text;
            }
            else
            {
                for (int i = 0; i < cmbDateFormat.Items.Count; i++)
                {
                    string s=(string)cmbDateFormat.Items[i];
                    cmbDateFormat.Items[i] = s.Replace(GetSepatator(s), split);
                }
            }
            cmbDateDelimiter.Text = split;
        }
        private string GetSepatator(string date)
        {
            List<string> list=date.Split(new char[]{'/','.','-'}).ToList();
            return date.Substring(list.First().Length, 1);

        }
        private void initEventForConfigTakeEffectNextTimeChanged()
        {
            this.cbShowMark.CheckedChanged +=new EventHandler((sender, e) => {
                this.isConfigTakeEffectNextTime = true;
            });
            this.cbShowLimit.CheckedChanged += new EventHandler((sender, e) =>
            {
                this.isConfigTakeEffectNextTime = true;
            });
            this.cbFillRange.CheckedChanged += new EventHandler((sender, e) =>
            {
                this.isConfigTakeEffectNextTime = true;
            });

        }
    }
}
