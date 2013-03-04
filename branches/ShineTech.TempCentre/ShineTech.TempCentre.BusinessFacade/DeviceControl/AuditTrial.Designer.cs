namespace ShineTech.TempCentre.BusinessFacade
{
    partial class AuditTrail
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.headerPanel1 = new ShineTech.TempCentre.BusinessFacade.HeaderPanel();
            this.dgvLog = new System.Windows.Forms.DataGridView();
            this.headerPanel2 = new ShineTech.TempCentre.BusinessFacade.HeaderPanel();
            this.pnAuditClick = new ShineTech.TempCentre.BusinessFacade.HeaderPanel();
            this.groupBox9 = new System.Windows.Forms.GroupBox();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.cbAnaAudit = new System.Windows.Forms.CheckBox();
            this.cbSysAudit = new System.Windows.Forms.CheckBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.dtpAuditTo = new System.Windows.Forms.DateTimePicker();
            this.dtpAuditFrom = new System.Windows.Forms.DateTimePicker();
            this.cmbUserName = new System.Windows.Forms.ComboBox();
            this.lbCounts = new System.Windows.Forms.Label();
            this.cmbAciton = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.headerPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).BeginInit();
            this.headerPanel2.SuspendLayout();
            this.groupBox9.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox8.SuspendLayout();
            this.SuspendLayout();
            // 
            // headerPanel1
            // 
            this.headerPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.headerPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.headerPanel1.Controls.Add(this.dgvLog);
            this.headerPanel1.Controls.Add(this.headerPanel2);
            this.headerPanel1.Controls.Add(this.groupBox9);
            this.headerPanel1.Controls.Add(this.groupBox8);
            this.headerPanel1.Curvature = 5;
            this.headerPanel1.Location = new System.Drawing.Point(13, 13);
            this.headerPanel1.Name = "headerPanel1";
            this.headerPanel1.Size = new System.Drawing.Size(995, 605);
            this.headerPanel1.TabIndex = 20;
            // 
            // dgvLog
            // 
            this.dgvLog.AllowUserToAddRows = false;
            this.dgvLog.AllowUserToDeleteRows = false;
            this.dgvLog.AllowUserToResizeRows = false;
            this.dgvLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvLog.BackgroundColor = System.Drawing.Color.White;
            this.dgvLog.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9F);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvLog.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLog.Location = new System.Drawing.Point(360, 56);
            this.dgvLog.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgvLog.Name = "dgvLog";
            this.dgvLog.ReadOnly = true;
            this.dgvLog.RowHeadersVisible = false;
            this.dgvLog.RowTemplate.Height = 23;
            this.dgvLog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvLog.Size = new System.Drawing.Size(601, 520);
            this.dgvLog.TabIndex = 20;
            // 
            // headerPanel2
            // 
            this.headerPanel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(138)))), ((int)(((byte)(5)))));
            this.headerPanel2.Controls.Add(this.pnAuditClick);
            this.headerPanel2.Curvature = 5;
            this.headerPanel2.CurveMode = ((ShineTech.TempCentre.BusinessFacade.CornerCurveMode)((ShineTech.TempCentre.BusinessFacade.CornerCurveMode.TopLeft | ShineTech.TempCentre.BusinessFacade.CornerCurveMode.TopRight)));
            this.headerPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.headerPanel2.Location = new System.Drawing.Point(0, 0);
            this.headerPanel2.Name = "headerPanel2";
            this.headerPanel2.Size = new System.Drawing.Size(995, 30);
            this.headerPanel2.TabIndex = 0;
            // 
            // pnAuditClick
            // 
            this.pnAuditClick.BackColor = System.Drawing.Color.Transparent;
            this.pnAuditClick.BackgroundImage = global::ShineTech.TempCentre.BusinessFacade.Properties.Resources.wk_at;
            this.pnAuditClick.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.pnAuditClick.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(214)))), ((int)(((byte)(138)))), ((int)(((byte)(5)))));
            this.pnAuditClick.Curvature = 5;
            this.pnAuditClick.CurveMode = ShineTech.TempCentre.BusinessFacade.CornerCurveMode.TopLeft;
            this.pnAuditClick.Dock = System.Windows.Forms.DockStyle.Left;
            this.pnAuditClick.Location = new System.Drawing.Point(0, 0);
            this.pnAuditClick.Name = "pnAuditClick";
            this.pnAuditClick.Size = new System.Drawing.Size(145, 30);
            this.pnAuditClick.TabIndex = 11;
            // 
            // groupBox9
            // 
            this.groupBox9.BackColor = System.Drawing.Color.White;
            this.groupBox9.Controls.Add(this.pictureBox3);
            this.groupBox9.Controls.Add(this.pictureBox2);
            this.groupBox9.Controls.Add(this.cbAnaAudit);
            this.groupBox9.Controls.Add(this.cbSysAudit);
            this.groupBox9.Location = new System.Drawing.Point(35, 48);
            this.groupBox9.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox9.Name = "groupBox9";
            this.groupBox9.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox9.Size = new System.Drawing.Size(292, 75);
            this.groupBox9.TabIndex = 19;
            this.groupBox9.TabStop = false;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Image = global::ShineTech.TempCentre.BusinessFacade.Properties.Resources.analyze_audit;
            this.pictureBox3.Location = new System.Drawing.Point(174, 54);
            this.pictureBox3.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(9, 9);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 4;
            this.pictureBox3.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ShineTech.TempCentre.BusinessFacade.Properties.Resources.system_audit;
            this.pictureBox2.Location = new System.Drawing.Point(174, 24);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(9, 9);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 5;
            this.pictureBox2.TabStop = false;
            // 
            // cbAnaAudit
            // 
            this.cbAnaAudit.AutoSize = true;
            this.cbAnaAudit.Checked = true;
            this.cbAnaAudit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbAnaAudit.Font = new System.Drawing.Font("Arial", 9F);
            this.cbAnaAudit.Location = new System.Drawing.Point(23, 47);
            this.cbAnaAudit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbAnaAudit.Name = "cbAnaAudit";
            this.cbAnaAudit.Size = new System.Drawing.Size(128, 19);
            this.cbAnaAudit.TabIndex = 2;
            this.cbAnaAudit.Text = "Analysis Audit Trail";
            this.cbAnaAudit.UseVisualStyleBackColor = true;
            // 
            // cbSysAudit
            // 
            this.cbSysAudit.AutoSize = true;
            this.cbSysAudit.Checked = true;
            this.cbSysAudit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbSysAudit.Font = new System.Drawing.Font("Arial", 9F);
            this.cbSysAudit.Location = new System.Drawing.Point(23, 18);
            this.cbSysAudit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbSysAudit.Name = "cbSysAudit";
            this.cbSysAudit.Size = new System.Drawing.Size(123, 19);
            this.cbSysAudit.TabIndex = 3;
            this.cbSysAudit.Text = "System Audit Trail";
            this.cbSysAudit.UseVisualStyleBackColor = true;
            // 
            // groupBox8
            // 
            this.groupBox8.BackColor = System.Drawing.Color.White;
            this.groupBox8.Controls.Add(this.btnClear);
            this.groupBox8.Controls.Add(this.dtpAuditTo);
            this.groupBox8.Controls.Add(this.dtpAuditFrom);
            this.groupBox8.Controls.Add(this.cmbUserName);
            this.groupBox8.Controls.Add(this.lbCounts);
            this.groupBox8.Controls.Add(this.cmbAciton);
            this.groupBox8.Controls.Add(this.label5);
            this.groupBox8.Controls.Add(this.label4);
            this.groupBox8.Controls.Add(this.label3);
            this.groupBox8.Controls.Add(this.label2);
            this.groupBox8.Location = new System.Drawing.Point(35, 147);
            this.groupBox8.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.groupBox8.Size = new System.Drawing.Size(292, 205);
            this.groupBox8.TabIndex = 16;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Filter";
            // 
            // btnClear
            // 
            this.btnClear.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnClear.Location = new System.Drawing.Point(205, 162);
            this.btnClear.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(63, 22);
            this.btnClear.TabIndex = 14;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            // 
            // dtpAuditTo
            // 
            this.dtpAuditTo.Font = new System.Drawing.Font("Arial", 9F);
            this.dtpAuditTo.Location = new System.Drawing.Point(89, 125);
            this.dtpAuditTo.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpAuditTo.Name = "dtpAuditTo";
            this.dtpAuditTo.Size = new System.Drawing.Size(179, 21);
            this.dtpAuditTo.TabIndex = 12;
            // 
            // dtpAuditFrom
            // 
            this.dtpAuditFrom.Font = new System.Drawing.Font("Arial", 9F);
            this.dtpAuditFrom.Location = new System.Drawing.Point(89, 99);
            this.dtpAuditFrom.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dtpAuditFrom.Name = "dtpAuditFrom";
            this.dtpAuditFrom.Size = new System.Drawing.Size(179, 21);
            this.dtpAuditFrom.TabIndex = 13;
            // 
            // cmbUserName
            // 
            this.cmbUserName.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUserName.Font = new System.Drawing.Font("Microsoft YaHei", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbUserName.FormattingEnabled = true;
            this.cmbUserName.Location = new System.Drawing.Point(93, 57);
            this.cmbUserName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbUserName.Name = "cmbUserName";
            this.cmbUserName.Size = new System.Drawing.Size(175, 24);
            this.cmbUserName.TabIndex = 11;
            // 
            // lbCounts
            // 
            this.lbCounts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.lbCounts.AutoSize = true;
            this.lbCounts.Font = new System.Drawing.Font("Arial", 9F);
            this.lbCounts.ForeColor = System.Drawing.Color.Blue;
            this.lbCounts.Location = new System.Drawing.Point(13, 166);
            this.lbCounts.Name = "lbCounts";
            this.lbCounts.Size = new System.Drawing.Size(0, 15);
            this.lbCounts.TabIndex = 9;
            // 
            // cmbAciton
            // 
            this.cmbAciton.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAciton.Font = new System.Drawing.Font("Microsoft YaHei", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbAciton.FormattingEnabled = true;
            this.cmbAciton.Location = new System.Drawing.Point(93, 29);
            this.cmbAciton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cmbAciton.Name = "cmbAciton";
            this.cmbAciton.Size = new System.Drawing.Size(175, 24);
            this.cmbAciton.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 9F);
            this.label5.Location = new System.Drawing.Point(13, 130);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 15);
            this.label5.TabIndex = 9;
            this.label5.Text = "Date To";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9F);
            this.label4.Location = new System.Drawing.Point(13, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 15);
            this.label4.TabIndex = 5;
            this.label4.Text = "Date From";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9F);
            this.label3.Location = new System.Drawing.Point(13, 60);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 15);
            this.label3.TabIndex = 8;
            this.label3.Text = "User Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F);
            this.label2.Location = new System.Drawing.Point(13, 32);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 15);
            this.label2.TabIndex = 7;
            this.label2.Text = "Action";
            // 
            // AuditTrail
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.Controls.Add(this.headerPanel1);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.Name = "AuditTrail";
            this.Size = new System.Drawing.Size(1021, 631);
            this.headerPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).EndInit();
            this.headerPanel2.ResumeLayout(false);
            this.groupBox9.ResumeLayout(false);
            this.groupBox9.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbCounts;
        private System.Windows.Forms.GroupBox groupBox9;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.CheckBox cbAnaAudit;
        private System.Windows.Forms.CheckBox cbSysAudit;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DateTimePicker dtpAuditTo;
        private System.Windows.Forms.DateTimePicker dtpAuditFrom;
        private System.Windows.Forms.ComboBox cmbUserName;
        private System.Windows.Forms.ComboBox cmbAciton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private HeaderPanel headerPanel1;
        private HeaderPanel headerPanel2;
        private HeaderPanel pnAuditClick;
        private System.Windows.Forms.DataGridView dgvLog;

    }
}
