using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.DAL;
namespace ShineTech.TempCentre.BusinessFacade
{
    public partial class UserMeaning : UserControl
    {
        private MeaningsBLL _bll = new MeaningsBLL();
        public UserMeaning()
        {
            InitializeComponent();
            this.InitEvents();
        }
        private List<Meaning> list=new List<Meaning>();//用户控件


        public List<Dictionary<string, object>> mEntity;
        //public List<Dictionary<string, object>> rEntity;
        #region method
        public void Init()
        {
            List<UserMeanRelation>  relation = _bll.GetMeaningByUser(null);
            layOutPn.Controls.Clear();
            if (relation != null)
            {
                foreach( UserMeanRelation ur in relation )
                {
                    Meaning m = new Meaning();
                    m.DelEvent+=new EventHandler(delegate(object sender,EventArgs args)
                        {
                            this.Remove((Meaning)sender);
                        });
                    //list.Add(m);
                    layOutPn.Controls.Add(m);
                }
            }
        }
        private void AddMeaing()
        {
            Meaning m = new Meaning();
            m.TextBoxEnable = true;
            m.DelEvent += new EventHandler(delegate(object sender, EventArgs args)
            {
                this.Remove(m);
            });
            list.Add(m);
            this.layOutPn.Controls.Add(m);
        }

        /// <summary>
        /// 移除控件
        /// </summary>
        /// <param name="m"></param>
        private void Remove(Meaning m)
        {
            //this.list.Remove(m);
            this.layOutPn.Controls.Remove(m);
        }

        public void SetValue()
        {
            if (mEntity == null)
                mEntity = new List<Dictionary<string, object>>();
            
            foreach (Meaning m in layOutPn.Controls)
            {
                m.SetValue();//自动赋值
                if (m.Mean != string.Empty)//空值不要
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("Desc", m.Mean);
                    dic.Add("Remark", DateTime.Now.ToString());
                    if(mEntity.Contains(dic))
                        continue;
                    this.mEntity.Add(dic);
                }
            }
        }
        public void InitEvents()
        {
            #region add meaning
            this.btnAdd.Click += new EventHandler(delegate(object sender,EventArgs args)
            {
                foreach(Meaning m in list){
                    m.CheckBoxVisible = true;
                }
                AddMeaing();
            });
            #endregion.

        }
        #endregion 
    }
}
