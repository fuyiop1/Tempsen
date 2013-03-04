using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.DAL;
using System.Data;

namespace ShineTech.TempCentre.BusinessFacade
{
    public class AdministratorUI
    {
        #region control
        private System.Windows.Forms.DataGridView dgvUser;
        private System.Windows.Forms.DataGridViewTextBoxColumn UserName;
        private System.Windows.Forms.DataGridViewTextBoxColumn FullName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Description;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnPolicy;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnAddMean;
        private System.Windows.Forms.Button btnEditMean;
        private System.Windows.Forms.Button btnDelMean;
        private System.Windows.Forms.CheckedListBox clbMeaning;
        #endregion
        private IDataProcessor processor= new DeviceProcessor();
        private Boolean flag;//联动事件symbol
        private void ConstructForms(Form form)
        {
            dgvUser = form.Controls.Find("dgvUser", true)[0] as DataGridView;
            btnAdd = form.Controls.Find("btnAdd", true)[0] as Button;
            btnEdit = form.Controls.Find("btnEdit", true)[0] as Button;
            btnDelete = form.Controls.Find("btnDelete", true)[0] as Button;
            btnPolicy = form.Controls.Find("btnPolicy", true)[0] as Button;
            btnAddMean = form.Controls.Find("btnAddMean", true)[0] as Button;
            btnEditMean = form.Controls.Find("btnEditMean", true)[0] as Button;
            btnDelMean = form.Controls.Find("btnDelMean", true)[0] as Button;
            clbMeaning = form.Controls.Find("clbMeaning", true)[0] as CheckedListBox;
            btnExit = form.Controls.Find("btnExit", true)[0] as Button;
        }
        public AdministratorUI(Form form)
        {
            this.ConstructForms(form); 
            this.InitUsers(); 
            this.InitMeaning();
            this.InitEvents();


            
        }
        /// <summary>
        /// init meanings 列表
        /// </summary>
        public void InitMeaning()
        {
            // DataSet  list = processor.Query("SELECT * FROM Meanings", null);
            List<Meanings> list = processor.Query<Meanings>("SELECT * FROM Meanings", null);
            if (list != null && list.Count > 0)
            {

                this.clbMeaning.DataSource = list;
                this.clbMeaning.ValueMember = "ID";
                this.clbMeaning.DisplayMember = "Desc";
                Common.SetControlEnable(this.btnEditMean, true);
                Common.SetControlEnable(this.btnDelMean, true);
            }
            else
            {
                Common.SetControlEnable(this.btnEditMean, false);
                Common.SetControlEnable(this.btnDelMean, false);
            }
        }
        public void InitUsers()
        {
            DataSet ds = processor.Query(@"SELECT username as 'User Name',fullname as 'Full Name'
                                                                                       ,description as 'Description' ,remark 'Remark' 
                                                                         FROM userinfo", null);
            if (ds != null && ds.Tables.Count > 0)
            {
                dgvUser.DataSource = ds.Tables[0];
                if (ds.Tables[0].Rows.Count > 0)
                {
                    this.dgvUser.ClearSelection();
                    //this.dgvUser.Rows[ds.Tables[0].Rows.Count-1].Selected = true;
                    //this.dgvUser.Rows
                    Common.SetControlEnable(this.btnEdit, true);
                    Common.SetControlEnable(this.btnDelete, true);
                }
                else
                { 
                    Common.SetControlEnable(this.btnEdit, false);
                    Common.SetControlEnable(this.btnDelete, false);
                }
            }
            else 
            {
                Common.SetControlEnable(this.btnEdit, false);
                Common.SetControlEnable(this.btnDelete, false);
            }
        }
        
        private void InitEvents()
        {
            /*删除用户*/
            this.btnDelete.Click += new EventHandler(delegate(object sender,EventArgs args)
            {
                DataTable dt = dgvUser.DataSource as DataTable;
                if (null != dt)
                {
                    Dictionary<string, object> dic;
                    foreach (DataGridViewRow row in this.dgvUser.SelectedRows)
                    {
                        dic = new Dictionary<string, object>();
                        dic.Add("username", row.Cells["User Name"].Value.ToString());
                        processor.ExecuteNonQuery("DELETE FROM USERINFO  WHERE username=@username", dic);
                        dt.Rows.RemoveAt(row.Index);                        
                    }
                    //this.InitUsers();
                    this.dgvUser.DataSource = dt;
                }
            });
            /*删除meanings*/
            this.btnDelMean.Click += new EventHandler(delegate(object sender, EventArgs args)
            {
                object o = this.clbMeaning.SelectedValue;
                if (o == null && o.ToString() == string.Empty)
                {
                }
                else
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    dic.Add("id", o.ToString());
                    processor.ExecuteNonQuery("DELETE FROM Meanings  WHERE id=@id", dic);
                    processor.ExecuteNonQuery("DELETE FROM UserMeanRelation  WHERE MeaningsID=@id", dic);
                    //this.InitMeaning();
                    /* 重新绑定数据源 取消从数据库中读取*/
                    List<Meanings> list = this.clbMeaning.DataSource as List<Meanings>;
                    if (list != null)
                    {
                        list = new List<Meanings>(list);//it does not make sense? it is bugged?
                        list.Remove(this.clbMeaning.SelectedItem as Meanings);
                        this.clbMeaning.DataSource = list;
                        this.clbMeaning.ValueMember = "ID";
                        this.clbMeaning.DisplayMember = "Desc";
                    }
                }
            });
            #region comment itemcheck
            /*list check event*/
            this.clbMeaning.ItemCheck += new ItemCheckEventHandler(delegate(object sender, ItemCheckEventArgs args)
            {
                if (!flag) return;
                if (this.dgvUser.SelectedRows.Count <= 0)
                {
                    args.NewValue = args.CurrentValue;
                }
                else
                {
                    Dictionary<string, object> dic = new Dictionary<string, object>();
                    int meanid = ((Meanings)clbMeaning.SelectedItem).Id;
                    string username = this.dgvUser.SelectedRows[0].Cells["User Name"].Value.ToString();
                    dic.Add("MeaningsID", meanid);
                    dic.Add("username", username);
                    object obj = processor.QueryScalar("SELECT 1 FROM UserMeanRelation where MeaningsID=@MeaningsID AND username=@username", dic);
                    if (args.NewValue == CheckState.Checked && obj == null)//添加
                    {
                        obj = processor.QueryScalar("SELECT MAX(ID) FROM UserMeanRelation", null);
                        UserMeanRelation u = new UserMeanRelation();
                        u.ID = obj != null && obj.ToString() != string.Empty ? Convert.ToInt32(obj) + 1 : 1;
                        //u.MeaningsID = meanid;
                        u.Username = username;
                        //u.MeaningDesc = ((Meanings)clbMeaning.SelectedItem).Desc;
                        u.Remark = DateTime.Now.ToString();
                        if (processor.Insert<UserMeanRelation>(u, null))
                            return;//MessageBox.Show("Add the meaning to " + username + " successfully!");
                        else
                            MessageBox.Show("Add the meaning to " + username + " error!");
                    }
                    else if (args.NewValue == CheckState.Unchecked && obj != null)
                    {
                        processor.ExecuteNonQuery("DELETE FROM UserMeanRelation WHERE MeaningsID=@MeaningsID AND username=@username ", dic);
                    }
                }
            });
            #endregion
            //selection changed
            this.dgvUser.SelectionChanged += new EventHandler(delegate(object sender, EventArgs args)
            {
                this.UserSelectedChange();
            });
            #region comment
            //this.clbMeaning.SelectedIndexChanged += new EventHandler(delegate(object sender, EventArgs args)
            //{
            //    if (!flag) return;
            //    CheckState ck = this.clbMeaning.GetItemCheckState(clbMeaning.Items.IndexOf(clbMeaning.SelectedItem));
            //    if (this.dgvUser.SelectedRows.Count <= 0)
            //    {
            //        MessageBox.Show("Please select the user!");
            //        //args.NewValue = args.CurrentValue;
            //    }
            //    else
            //    {
            //        Dictionary<string, object> dic = new Dictionary<string, object>();
            //        int meanid = ((Meanings)clbMeaning.SelectedItem).Id;
            //        string username = this.dgvUser.SelectedRows[0].Cells["User Name"].Value.ToString();
            //        dic.Add("MeaningsID", meanid);
            //        dic.Add("username", username);
            //        object obj = processor.QueryScalar("SELECT 1 FROM UserMeanRelation where MeaningsID=@MeaningsID AND username=@username", dic);
            //        if (ck == CheckState.Unchecked && obj == null)//添加
            //        {
            //            obj = processor.QueryScalar("SELECT MAX(ID) FROM UserMeanRelation", null);
            //            UserMeanRelation u = new UserMeanRelation();
            //            u.ID = obj != null && obj.ToString() != string.Empty ? Convert.ToInt32(obj) + 1 : 1;
            //            u.MeaningsID = meanid;
            //            u.Username = username;
            //            u.MeaningDesc = ((Meanings)clbMeaning.SelectedItem).Desc;
            //            u.Remark = DateTime.Now.ToString();
            //            if (processor.Insert<UserMeanRelation>(u, null))
            //            {
            //                //MessageBox.Show("Add the meaning to " + username + " successfully!");
            //                this.clbMeaning.SetItemCheckState(this.clbMeaning.SelectedIndex, ck == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked);
            //            }
            //            else
            //                MessageBox.Show("Add the meaning to " + username + " error!");
            //        }
            //        else if (ck == CheckState.Checked && obj != null)
            //        {
            //            processor.ExecuteNonQuery("DELETE FROM UserMeanRelation WHERE MeaningsID=@MeaningsID AND username=@username ", dic);
            //            this.clbMeaning.SetItemCheckState(this.clbMeaning.SelectedIndex, ck == CheckState.Checked ? CheckState.Unchecked : CheckState.Checked);
            //        }
            //    }
            //});
            #endregion
        }
        public void UserSelectedChange()
        {
            if (this.dgvUser.SelectedRows.Count > 0&&clbMeaning.Items!=null)
            {
                Dictionary<string, object> dic = new Dictionary<string, object>();
                //int meanid = ((Meanings)clbMeaning.SelectedItem).Id;
                string username = this.dgvUser.SelectedRows[0].Cells["User Name"].Value.ToString();
                dic.Add("username", username);
                List<UserMeanRelation> list = processor.Query<UserMeanRelation>("SELECT * FROM UserMeanRelation WHERE  username=@username", dic);
                flag = false;
                for (int i = 0; i < this.clbMeaning.Items.Count; i++)
                {
                    //UserMeanRelation r = list.Find(p => p.MeaningsID == ((Meanings)this.clbMeaning.Items[i]).Id);
                    //clbMeaning.SetItemCheckState(i, r != null ? CheckState.Checked : CheckState.Unchecked);
                }
                flag = true;
            }
        }
    }
}
