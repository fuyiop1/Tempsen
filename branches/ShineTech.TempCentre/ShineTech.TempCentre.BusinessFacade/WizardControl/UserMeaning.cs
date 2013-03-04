using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.DAL;
using ShineTech.TempCentre.BusinessFacade.DeviceControl;
using ShineTech.TempCentre.Platform;
using ShineTech.TempCentre.BusinessFacade.ViewModel;
namespace ShineTech.TempCentre.BusinessFacade
{
    public partial class UserMeaning : UserControl
    {
        private MeaningsBLL _meanBll = new MeaningsBLL();
        private MeanCheckListBox clbMeans;
        public UserMeaning()
        {
            InitializeComponent();
            this.InitEvents();
            this.Init();
        }

        private IDictionary<string, IList<int>> newUserRelation = new Dictionary<string, IList<int>>();
        private IList<Meanings> allMeanings;


        public List<Dictionary<string, object>> mEntity;
        //public List<Dictionary<string, object>> rEntity;
        #region method
        public void Init()
        {
            allMeanings = _meanBll.GetAllMeans();
            //List<UserMeanRelation>  relation = _bll.GetMeaningByUser(null);
            if (allMeanings != null)
            {
                this.layOutPn.Controls.Clear();
                this.clbMeans = new MeanCheckListBox(allMeanings, this.newUserRelation, this.layOutPn.Width);
                this.clbMeans.Width = this.layOutPn.Width;
                this.clbMeans.Height = this.layOutPn.Height;
                this.clbMeans.Margin = new Padding(0);
                this.layOutPn.Controls.Add(this.clbMeans);
            }
        }

        

        

        public void SetValue()
        {
            if (mEntity == null)
                mEntity = new List<Dictionary<string, object>>();

            System.Windows.Forms.CheckedListBox.CheckedItemCollection checkedMeans = this.clbMeans.CheckedItems;
            foreach (var item in checkedMeans)
            {
                var meanViewModel = item as MeaningViewModel;
                if (meanViewModel != null)
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    Meanings mean = allMeanings.Where(m => m.Id == meanViewModel.Id).First();
                    dic.Add("Mean", mean);
                    dic.Add("Remark", DateTime.Now.ToString());
                    if(mEntity.Contains(dic))
                        continue;
                    this.mEntity.Add(dic);
                }
            }
            
        }
        public void InitEvents()
        {
            this.btnAddMean.Click += new EventHandler((a, b) =>
            {

                InputBoxDialog newMeanDialog = new InputBoxDialog(InputBoxTitle.AddMeaning, InputBoxTipMessage.AddMeaning, false);
                if (newMeanDialog.ShowDialog(this) == DialogResult.OK)
                {
                    Meanings meaning = new Meanings() { Id = this._meanBll.GetMeaningPKValue() + 1, Desc = newMeanDialog.InputBoxText.TrimEnd(), Remark = DateTime.Now.ToString() };
                    this._meanBll.InsertOrUpdateMeaning(meaning);
                    this.allMeanings.Add(meaning);
                    if (this.clbMeans != null)
                    {
                        this.clbMeans.AddMean(meaning);
                    }
                }

            });

            this.btnEditMean.Click += new EventHandler((send, args) =>
            {
                InputBoxDialog updateMeanDialog = new InputBoxDialog(InputBoxTitle.EditMeaning, InputBoxTipMessage.EditMeaning, false);
                int selectedMeanId = this.clbMeans.GetCurrentSelectedMean();
                Meanings selectedMean = null;
                if (allMeanings != null)
                {
                    selectedMean = allMeanings.Where<Meanings>(m => m.Id == selectedMeanId).FirstOrDefault();
                }
                if (selectedMean != null)
                {
                    updateMeanDialog.InputBoxText = selectedMean.Desc;
                    if (updateMeanDialog.ShowDialog(this) == DialogResult.OK)
                    {
                        if (!updateMeanDialog.InputBoxText.TrimEnd().Equals(selectedMean.Desc, StringComparison.Ordinal))
                        {
                            selectedMean.Desc = updateMeanDialog.InputBoxText.TrimEnd();
                            selectedMean.Remark = DateTime.Now.ToString();
                            this._meanBll.InsertOrUpdateMeaning(selectedMean);
                            this.clbMeans.Refresh();
                        }
                    }
                }

            });

            this.btnDeleteMean.Click += new EventHandler((send, args) =>
            {
                if (DialogResult.Yes == Utils.ShowMessageBox(Messages.DeleteMeaning, Messages.TitleWarning, MessageBoxButtons.YesNo))
                {
                    int selectedMeanId = this.clbMeans.GetCurrentSelectedMean();
                    Meanings selectedMean = null;
                    if (allMeanings != null)
                    {
                        selectedMean = allMeanings.Where<Meanings>(m => m.Id == selectedMeanId).FirstOrDefault();
                    }
                    if (selectedMean != null)
                    {
                        this._meanBll.DeleteMeaningAndRelation(selectedMean);
                        allMeanings.Remove(selectedMean);
                        this.clbMeans.Items.Remove(selectedMean);
                        foreach (var item in this.newUserRelation.Values)
                        {
                            item.Remove(selectedMeanId);
                        }
                        this.clbMeans.Refresh();
                    }
                }
            });

        }
        #endregion 

    }
}
