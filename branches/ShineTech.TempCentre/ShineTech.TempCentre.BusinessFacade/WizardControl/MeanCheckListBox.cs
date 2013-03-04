using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.DAL;
using ShineTech.TempCentre.BusinessFacade.ViewModel;

namespace ShineTech.TempCentre.BusinessFacade
{
    public partial class MeanCheckListBox : CheckedListBox
    {
        private static readonly int CheckBoxWidth = 13;

        private int originalIndex = 0;
        private int _supposedWidth;

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        private MeaningsBLL meaningBLL = new MeaningsBLL();

        private IList<Meanings> allMeanings = new List<Meanings>();
        private IDictionary<string, IList<int>> currentRelationOfUserAndMeaning;

        private ToolTip meaningTip = new ToolTip();
        private int _currentShowingToolTipItemIndex = -1;

        public MeanCheckListBox(IList<Meanings> allMeanings, IDictionary<string, IList<int>> currentRelationOfUserAndMeaning, int supposedWidth)
        {
            this.allMeanings = allMeanings;
            this.currentRelationOfUserAndMeaning = currentRelationOfUserAndMeaning;
            _supposedWidth = supposedWidth - 15;
            InitializeComponent();
            DisplayMember = "DisplayDesc";
            ValueMember = "Id";
            this.InitEvent();
            this.InitPresentation();
            this.InitMeans();
        }

        private void InitMeans()
        {
            this.Items.Clear();
            if (this.allMeanings != null && this.allMeanings.Count > 0)
            {
                foreach (Meanings meaningItem in this.allMeanings)
                {
                    var meaningViewModel = new MeaningViewModel(meaningItem, this.Font, _supposedWidth);
                    this.Items.Add(meaningViewModel);
                }
                this.SetSelected(0, true);
            }
        }


        public void AddMean(Meanings mean)
        {
            if (mean != null)
            {
                var meaningViewModel = new MeaningViewModel(mean, this.Font, _supposedWidth);
                this.Items.Add(meaningViewModel);
                this.SetItemChecked(this.Items.Count - 1, true);
                this.SetSelected(this.Items.Count - 1, true);
            }
        }

        public int GetCurrentSelectedMean()
        {
            int result = 0;
            var selectedMean = this.SelectedItem as MeaningViewModel;
            if (selectedMean != null)
            {
                result = selectedMean.Id;
            }
            return result;
        }


        private void InitPresentation()
        {
            this.CheckOnClick = false;
        }

        private void InitEvent()
        {
            this.ItemCheck += this_ItemCheck;
            this.MouseMove += new MouseEventHandler(ShowMeaningTips);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            var mouseEventArgs = e as MouseEventArgs;
            if (mouseEventArgs != null)
            {
                if (originalIndex != SelectedIndex && mouseEventArgs.X < CheckBoxWidth)
                {
                    this.SetItemChecked(SelectedIndex, !GetItemChecked(SelectedIndex));
                }
            }
        }

        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);
            this.originalIndex = SelectedIndex;
        }

        public override void Refresh()
        {
            for (int i = this.Items.Count - 1; i >= 0; i--)
            {
                var viewModel = this.Items[i] as MeaningViewModel;
                if (viewModel != null && allMeanings.Where(m => m.Id == viewModel.Id).Count() == 0)
                {
                    this.Items.RemoveAt(i);
                }
            }
            base.Refresh();
        }

        private void this_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (currentRelationOfUserAndMeaning.Count == 0)
            {
                return;
            }
            var checkStateChangedMeaning = this.Items[e.Index] as MeaningViewModel;
            if (checkStateChangedMeaning != null)
            {
                if (e.NewValue == CheckState.Checked)
                {
                    if (!currentRelationOfUserAndMeaning[Common.CurrentSelectedUserOfDgv1.UserName].Contains(checkStateChangedMeaning.Id))
                    {
                        currentRelationOfUserAndMeaning[Common.CurrentSelectedUserOfDgv1.UserName].Add(checkStateChangedMeaning.Id);
                    }
                }
                else
                {
                    if (currentRelationOfUserAndMeaning[Common.CurrentSelectedUserOfDgv1.UserName].Contains(checkStateChangedMeaning.Id))
                    {
                        currentRelationOfUserAndMeaning[Common.CurrentSelectedUserOfDgv1.UserName].Remove(checkStateChangedMeaning.Id);
                    }
                }
            }
        }

        private void ShowMeaningTips(object sender, MouseEventArgs e)
        {
            int index = IndexFromPoint(e.X, e.Y);
            if (index == -1)
            {
                _currentShowingToolTipItemIndex = -1;
                this.meaningTip.SetToolTip(this, string.Empty);
            }
            else
            {
                if (index != _currentShowingToolTipItemIndex)
                {
                    _currentShowingToolTipItemIndex = index;
                    string text = Items[index].GetType().GetProperty("Desc").GetValue(Items[index], null).ToString();
                    this.meaningTip.SetToolTip(this, text);
                }
            }
        }
    }
}
