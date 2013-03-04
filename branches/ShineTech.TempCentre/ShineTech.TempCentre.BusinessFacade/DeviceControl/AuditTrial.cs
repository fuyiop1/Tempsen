using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ShineTech.TempCentre.DAL;
using ShineTech.TempCentre.BusinessFacade.ReportService;
using ShineTech.TempCentre.BusinessFacade.DeviceControl;
using System.IO;
using ShineTech.TempCentre.Platform;
using System.Drawing.Printing;
using System.Globalization;

namespace ShineTech.TempCentre.BusinessFacade
{
    public partial class AuditTrail : UserControl
    {
        private OperationLogBLL logbll = new OperationLogBLL();
        private List<OperationLog> list = new List<OperationLog>();
        private int count = 0;

        private PrintPreviewDialog printPreviewDialog = Utils.GetPrintPreviewDialogue();
        public AuditTrail()
        {
            InitializeComponent();
            this.InitLog();
            this.InitEvents();
            pnAuditClick.BackgroundImage=Platform.Utils.DrawTextOnImage(Properties.Resources.wk_at,"Audit Trial" ,50, 9);
        }
        public void InitLog()
        {
            cbAnaAudit.Checked = cbSysAudit.Checked = true;
            dtpAuditFrom.Format = DateTimePickerFormat.Custom;
            dtpAuditFrom.CustomFormat = Common.GetDateOrTimeFormat(true, Common.GlobalProfile.DateTimeFormator);
            dtpAuditTo.Format = DateTimePickerFormat.Custom;
            dtpAuditTo.CustomFormat = Common.GetDateOrTimeFormat(true, Common.GlobalProfile.DateTimeFormator);
            list = logbll.GetLog(null);
            if (list != null && list.Count > 0)
            {
                count = list.Count;
                AddColumnsToGrid();
                AddRowsToGrid();
                DateTime end = list.Max(p => p.Operatetime.ToLocalTime());
                DateTime start = list.Min(p => p.Operatetime.ToLocalTime());
                this.dtpAuditFrom.Value = start;
                this.dtpAuditTo.Value = end;
                List<string> useList = list.Select(p => p.Username).Distinct().ToList();
                useList.Add("");
                useList.Reverse();
                this.cmbUserName.DataSource = useList;
                this.cmbUserName.DisplayMember = "User Name";
                InitAction();
            }
        }

        public void InitEvents()
        {
            this.cmbUserName.SelectedIndexChanged += new EventHandler((sender, args) => SearchLog());
            this.cmbAciton.SelectedIndexChanged += new EventHandler((sender, args) => SearchLog());
            this.cbAnaAudit.CheckedChanged += new EventHandler((sender, args) => {
                SearchAction(); 
                SearchLog();
            });
            this.cbSysAudit.CheckedChanged += new EventHandler((sender, args) =>
            {
                SearchAction();
                SearchLog();
            });
            this.dtpAuditFrom.ValueChanged += new EventHandler((sender, args) => SearchLog());
            this.dtpAuditTo.ValueChanged += new EventHandler((sender, args) => SearchLog());
            this.dgvLog.CellPainting += new DataGridViewCellPaintingEventHandler(CellPainting);
            this.btnClear.Click += new EventHandler((sender, args) =>InitLog());
        }
        public void RefreshAuditTrail()
        {
            string username = (string)cmbUserName.SelectedItem;
            bool sys = cbSysAudit.Checked;
            bool ana = cbAnaAudit.Checked;
            string action = (string)cmbAciton.SelectedItem;
            DateTime start = dtpAuditFrom.Value;
            DateTime end = dtpAuditTo.Value;
            InitLog();
            cbSysAudit.Checked = sys;
            cbAnaAudit.Checked = ana;
            cmbAciton.SelectedItem = action;
            cmbUserName.SelectedItem = username;
            dtpAuditFrom.Value = start;
            dtpAuditTo.Value = end;
            SearchLog();
        }
        private void SearchLog()
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            if (!cbSysAudit.Checked && !cbAnaAudit.Checked)
                dic.Add("LogType", -1);
            else if (cbAnaAudit.Checked && !cbSysAudit.Checked)
                dic.Add("LogType", 1);
            else if (cbSysAudit.Checked && !cbAnaAudit.Checked)
                dic.Add("LogType", 0);
            if (cmbAciton.SelectedValue.ToString() != string.Empty)
                dic.Add("Action", cmbAciton.SelectedValue);
            if (cmbUserName.SelectedValue.ToString() != string.Empty)
                dic.Add("UserName", cmbUserName.SelectedValue);
            dic.Add("OperateTime1", dtpAuditFrom.Value.ToUniversalTime().ToString("yyyyMMdd", CultureInfo.InvariantCulture));
            dic.Add("OperateTime2", dtpAuditTo.Value.ToUniversalTime().ToString("yyyyMMdd", CultureInfo.InvariantCulture));
            list=logbll.GetLog(dic);
            AddColumnsToGrid();
            //this.dgvLog.DataSource = logbll.GetLog(dic);
            AddRowsToGrid();
        }
        private void CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.RowIndex >= 0 && e.ColumnIndex == 0)
            {
                if (dgvLog.Rows[e.RowIndex].Cells["Action"].Value == null)
                    return;
                int logtype = list[e.RowIndex].LogType;
                Image img;
                if (logtype == 0)//系统日志
                    img = Properties.Resources.system_audit;
                else
                    img = Properties.Resources.analyze_audit;
                Rectangle newRect = new Rectangle(e.CellBounds.X + 3, e.CellBounds.Y + 8, 9,
                   9);

                using (Brush gridBrush = new SolidBrush(this.dgvLog.GridColor), backColorBrush = new SolidBrush(e.CellStyle.BackColor))
                {
                    using (Pen gridLinePen = new Pen(gridBrush,2))
                    {
                        // Erase the cell.
                        e.Graphics.FillRectangle(backColorBrush, e.CellBounds);
                        e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
                        //划线
                        if (e.RowIndex != dgvLog.Rows.Count - 1)
                        {
                            Point p1 = new Point(e.CellBounds.Left + e.CellBounds.Width, e.CellBounds.Top);
                            Point p2 = new Point(e.CellBounds.Left + e.CellBounds.Width, e.CellBounds.Top + e.CellBounds.Height);
                            Point p3 = new Point(e.CellBounds.Left, e.CellBounds.Top + e.CellBounds.Height);
                            //Point p0 = new Point(e.CellBounds.Left, e.CellBounds.Top);
                            Point[] ps = new Point[] { p1, p2, p3 };
                            e.Graphics.DrawLines(gridLinePen, ps);
                        }
                        else
                        {
                            Point p1 = new Point(e.CellBounds.Left + e.CellBounds.Width, e.CellBounds.Top);
                            Point p2 = new Point(e.CellBounds.Left + e.CellBounds.Width, e.CellBounds.Top + e.CellBounds.Height);
                            //Point p3 = new Point(e.CellBounds.Left, e.CellBounds.Top + e.CellBounds.Height);
                            //Point p0 = new Point(e.CellBounds.Left, e.CellBounds.Top);
                            Point[] ps = new Point[] { p1, p2 };
                            e.Graphics.DrawLines(gridLinePen, ps);
                            gridLinePen.Width = 1F;
                            e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom-1, e.CellBounds.Right - 1, e.CellBounds.Bottom-1);
                        }
                        //画图标
                        e.Graphics.DrawImage(img, newRect);
                        //画字符串
                        e.Graphics.DrawString(dgvLog.Rows[e.RowIndex].Cells["Action"].Value.ToString(), e.CellStyle.Font, Brushes.Black,
                            e.CellBounds.Left + 20, e.CellBounds.Top + 5, StringFormat.GenericDefault);
                        e.Handled = true;
                    }
                }
            }
        }
        private void AddColumnsToGrid()
        {
            if (dgvLog.Columns.Count == 0)
            {
                dgvLog.Columns.Add(new DataGridViewColumn()
                {
                    CellTemplate = new DataGridViewTextBoxCell(),
                    HeaderText = "Action",
                    Name = "Action",
                    Width = 140
                });
                dgvLog.Columns.Add(new DataGridViewColumn()
                {
                    CellTemplate = new DataGridViewTextBoxCell(),
                    HeaderText = "User Name",
                    Name = "UserName",
                    Width = 80
                });
                dgvLog.Columns.Add(new DataGridViewColumn()
                {
                    CellTemplate = new DataGridViewTextBoxCell(),
                    HeaderText = "Full Name",
                    Name = "fullname",
                    Width = 100
                });
                dgvLog.Columns.Add(new DataGridViewColumn()
                {
                    CellTemplate = new DataGridViewTextBoxCell(),
                    HeaderText = "Date",
                    Name = "operatetime",
                    Width = 140,
                    SortMode=DataGridViewColumnSortMode.Automatic
                });
                dgvLog.Columns.Add(new DataGridViewColumn()
                {
                    CellTemplate = new DataGridViewTextBoxCell(),
                    HeaderText = "Detail",
                    Name = "Detail",
                    AutoSizeMode=DataGridViewAutoSizeColumnMode.Fill,
                    MinimumWidth=450,
                    Resizable=DataGridViewTriState.False,
                    Width = 450
                });
            }
        }
        private void AddRowsToGrid()
        {
            dgvLog.Rows.Clear();
            if (list != null && list.Count > 0)
            {
                dgvLog.Rows.Add(list.Count);
                for (int i = 0; i < list.Count; i++)
                {
                    dgvLog.Rows[i].Cells["Action"].Value = list[i].Action;
                    dgvLog.Rows[i].Cells["UserName"].Value = list[i].Username;
                    dgvLog.Rows[i].Cells["fullname"].Value = list[i].Fullname;
                    dgvLog.Rows[i].Cells["operatetime"].Value = list[i].Operatetime.ToLocalTime().ToString(Common.GlobalProfile.DateTimeFormator, CultureInfo.InvariantCulture);
                    dgvLog.Rows[i].Cells["Detail"].Value = list[i].Detail;
                }
            }
            lbCounts.Text = string.Format("{0} of {1}", list == null ? 0 : list.Count, count < list.Count?list.Count:count);
        }
        private void InitAction()
        {
            List<string> ls=new List<string> (){"",LogAction.AddUser,LogAction.EditUser,LogAction.ChangePassword,LogAction.ChangeGroup,LogAction.Logon
                                                ,LogAction.Logoff,LogAction.AssignRights,LogAction.LockUser,LogAction.UnlockUser,LogAction.ConfigurateDevice
                                                ,LogAction.Saverecord,LogAction.Signrecord,LogAction.Commentrecord,LogAction.Deleterecord};
            this.cmbAciton.DataSource = ls;
            this.cmbAciton.DisplayMember = "Action";
        }
        private void SearchAction()
        {
            List<string> ls1 = new List<string>(){"",LogAction.AddUser,LogAction.EditUser,LogAction.ChangePassword,LogAction.ChangeGroup,LogAction.Logon
                                                ,LogAction.Logoff,LogAction.AssignRights,LogAction.LockUser,LogAction.UnlockUser};
            List<string> ls2 = new List<string>(){"",LogAction.ConfigurateDevice
                                                ,LogAction.Saverecord,LogAction.Signrecord,LogAction.Commentrecord,LogAction.Deleterecord};
            if (cbAnaAudit.Checked && cbSysAudit.Checked)
                InitAction();
            else if (cbSysAudit.Checked && !cbAnaAudit.Checked)
            {
                this.cmbAciton.DataSource = ls1;
                this.cmbAciton.DisplayMember = "Action";
            }
            else if (!cbSysAudit.Checked && cbAnaAudit.Checked)
            {
                this.cmbAciton.DataSource = ls2;
                this.cmbAciton.DisplayMember = "Action";
            }
            else
            {
                this.cmbAciton.DataSource = new List<string>() { ""};
                this.cmbAciton.DisplayMember = "Action";
            }
        }
        public void ExportAuditTrail()
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "PDF Files(.pdf)|*.pdf";
            Common.SetDefaultPathForSaveFileDialog(saveFileDialog, SavingFileType.AuditTrail);
            if (saveFileDialog.ShowDialog(this) == DialogResult.OK)
            {
                try
                {
                    new AuditTrailExporter(saveFileDialog.FileName, "", this.list).GenerateReport();
                }
                catch (IOException)
                {
                    Utils.ShowMessageBox(Messages.SameNameFileOpened, Messages.TitleError);
                }
                catch (Exception e)
                {
                    Utils.ShowMessageBox(e.Message, Messages.TitleError);
                }
            }
        }
        public void EMail()
        {
            try
            {
                string fileNameWithFullPath = Path.Combine(System.Windows.Forms.Application.StartupPath, "temp", string.Format("{0}.pdf", DateTime.Now.ToString("yyyy-MM-dd"), CultureInfo.InvariantCulture));
                if (new AuditTrailExporter(fileNameWithFullPath, "", this.list).GenerateReport())
                {
                    new ReportOutlookEmailer().CreateEmailAndAddAttachments(fileNameWithFullPath);
                    FileHelper.DeleteTempFiles(fileNameWithFullPath);
                }
            }
            catch (Exception)
            {
                // TODO
            }
        }
        public void Print(bool isPreview)
        {
            FileHelper.DeleteTempFiles("print");
            string tempFileName = Path.Combine(System.Windows.Forms.Application.StartupPath, "temp", "temp.print");
            try
            {

                IReportExportService exporter = new  AuditTrailExporter(tempFileName, "", this.list, true);
                exporter.GenerateReport();
                PDFReportPrinter printer = new PDFReportPrinter(tempFileName);
                if (isPreview)
                {
                    this.printPreviewDialog.Document = printer.PrintDocument;
                    this.printPreviewDialog.Height = 500;
                    this.printPreviewDialog.Width = 800;
                    this.printPreviewDialog.ShowDialog();
                }
                else
                {
                    printer.PrintReport();
                }
            }
            catch (InvalidPrinterException ipe)
            {
                Utils.ShowMessageBox(Messages.NoPrinterInstalled, Messages.TitleError);
            }
            catch (Exception e)
            {
                Utils.ShowMessageBox(e.Message, Messages.TitleError);
            }
        }
    }
}
