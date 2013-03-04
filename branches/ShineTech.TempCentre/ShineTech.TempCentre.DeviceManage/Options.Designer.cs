namespace ShineTech.TempCentre.DeviceManage
{
    partial class Options
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Options));
            this.tabOption = new System.Windows.Forms.TabControl();
            this.tpGraph = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbFillRange = new System.Windows.Forms.CheckBox();
            this.cbShowMark = new System.Windows.Forms.CheckBox();
            this.cbShowLimit = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tbIdealRange = new System.Windows.Forms.TextBox();
            this.tbAlarmLimit = new System.Windows.Forms.TextBox();
            this.lbRangeRgb = new System.Windows.Forms.Label();
            this.lbLimitRgb = new System.Windows.Forms.Label();
            this.lbTempRgb = new System.Windows.Forms.Label();
            this.tbTempCurve = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.tpDate = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cmbDateDelimiter = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.cmbTimeFormat = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbDateFormat = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabOption.SuspendLayout();
            this.tpGraph.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tpDate.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabOption
            // 
            this.tabOption.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabOption.Controls.Add(this.tpGraph);
            this.tabOption.Controls.Add(this.tpDate);
            this.tabOption.Location = new System.Drawing.Point(12, 12);
            this.tabOption.Name = "tabOption";
            this.tabOption.SelectedIndex = 0;
            this.tabOption.Size = new System.Drawing.Size(560, 296);
            this.tabOption.TabIndex = 0;
            // 
            // tpGraph
            // 
            this.tpGraph.Controls.Add(this.groupBox2);
            this.tpGraph.Controls.Add(this.groupBox1);
            this.tpGraph.Font = new System.Drawing.Font("Arial", 9F);
            this.tpGraph.Location = new System.Drawing.Point(4, 24);
            this.tpGraph.Margin = new System.Windows.Forms.Padding(0);
            this.tpGraph.Name = "tpGraph";
            this.tpGraph.Size = new System.Drawing.Size(552, 268);
            this.tpGraph.TabIndex = 0;
            this.tpGraph.Text = "Graph Options";
            this.tpGraph.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbFillRange);
            this.groupBox2.Controls.Add(this.cbShowMark);
            this.groupBox2.Controls.Add(this.cbShowLimit);
            this.groupBox2.Location = new System.Drawing.Point(352, 74);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(187, 120);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            // 
            // cbFillRange
            // 
            this.cbFillRange.AutoSize = true;
            this.cbFillRange.Checked = true;
            this.cbFillRange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbFillRange.Location = new System.Drawing.Point(31, 76);
            this.cbFillRange.Name = "cbFillRange";
            this.cbFillRange.Size = new System.Drawing.Size(107, 19);
            this.cbFillRange.TabIndex = 0;
            this.cbFillRange.Text = "Fill ideal range";
            this.cbFillRange.UseVisualStyleBackColor = true;
            // 
            // cbShowMark
            // 
            this.cbShowMark.AutoSize = true;
            this.cbShowMark.Checked = true;
            this.cbShowMark.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowMark.Location = new System.Drawing.Point(31, 51);
            this.cbShowMark.Name = "cbShowMark";
            this.cbShowMark.Size = new System.Drawing.Size(95, 19);
            this.cbShowMark.TabIndex = 0;
            this.cbShowMark.Text = "Show marks";
            this.cbShowMark.UseVisualStyleBackColor = true;
            // 
            // cbShowLimit
            // 
            this.cbShowLimit.AutoSize = true;
            this.cbShowLimit.Checked = true;
            this.cbShowLimit.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbShowLimit.Location = new System.Drawing.Point(31, 26);
            this.cbShowLimit.Name = "cbShowLimit";
            this.cbShowLimit.Size = new System.Drawing.Size(125, 19);
            this.cbShowLimit.TabIndex = 0;
            this.cbShowLimit.Text = "Show alarm limits";
            this.cbShowLimit.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tbIdealRange);
            this.groupBox1.Controls.Add(this.tbAlarmLimit);
            this.groupBox1.Controls.Add(this.lbRangeRgb);
            this.groupBox1.Controls.Add(this.lbLimitRgb);
            this.groupBox1.Controls.Add(this.lbTempRgb);
            this.groupBox1.Controls.Add(this.tbTempCurve);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(14, 74);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(332, 120);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            // 
            // tbIdealRange
            // 
            this.tbIdealRange.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbIdealRange.Location = new System.Drawing.Point(141, 76);
            this.tbIdealRange.Name = "tbIdealRange";
            this.tbIdealRange.Size = new System.Drawing.Size(125, 21);
            this.tbIdealRange.TabIndex = 1;
            // 
            // tbAlarmLimit
            // 
            this.tbAlarmLimit.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbAlarmLimit.Location = new System.Drawing.Point(141, 50);
            this.tbAlarmLimit.Name = "tbAlarmLimit";
            this.tbAlarmLimit.Size = new System.Drawing.Size(125, 21);
            this.tbAlarmLimit.TabIndex = 1;
            // 
            // lbRangeRgb
            // 
            this.lbRangeRgb.BackColor = System.Drawing.Color.Aqua;
            this.lbRangeRgb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbRangeRgb.Location = new System.Drawing.Point(290, 77);
            this.lbRangeRgb.Name = "lbRangeRgb";
            this.lbRangeRgb.Size = new System.Drawing.Size(20, 20);
            this.lbRangeRgb.TabIndex = 0;
            // 
            // lbLimitRgb
            // 
            this.lbLimitRgb.BackColor = System.Drawing.SystemColors.Highlight;
            this.lbLimitRgb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbLimitRgb.Location = new System.Drawing.Point(290, 51);
            this.lbLimitRgb.Name = "lbLimitRgb";
            this.lbLimitRgb.Size = new System.Drawing.Size(20, 20);
            this.lbLimitRgb.TabIndex = 0;
            // 
            // lbTempRgb
            // 
            this.lbTempRgb.BackColor = System.Drawing.Color.Red;
            this.lbTempRgb.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lbTempRgb.Location = new System.Drawing.Point(290, 25);
            this.lbTempRgb.Name = "lbTempRgb";
            this.lbTempRgb.Size = new System.Drawing.Size(20, 20);
            this.lbTempRgb.TabIndex = 0;
            // 
            // tbTempCurve
            // 
            this.tbTempCurve.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbTempCurve.Location = new System.Drawing.Point(141, 24);
            this.tbTempCurve.Name = "tbTempCurve";
            this.tbTempCurve.Size = new System.Drawing.Size(125, 21);
            this.tbTempCurve.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 15);
            this.label3.TabIndex = 0;
            this.label3.Text = "Ideal range";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(22, 52);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Alarm limit lines";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(109, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Temperature curve";
            // 
            // tpDate
            // 
            this.tpDate.Controls.Add(this.groupBox3);
            this.tpDate.Location = new System.Drawing.Point(4, 24);
            this.tpDate.Name = "tpDate";
            this.tpDate.Padding = new System.Windows.Forms.Padding(3);
            this.tpDate.Size = new System.Drawing.Size(552, 268);
            this.tpDate.TabIndex = 1;
            this.tpDate.Text = "Date/Time Settings";
            this.tpDate.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cmbDateDelimiter);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.cmbTimeFormat);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.cmbDateFormat);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Location = new System.Drawing.Point(20, 18);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(513, 232);
            this.groupBox3.TabIndex = 1;
            this.groupBox3.TabStop = false;
            // 
            // cmbDateDelimiter
            // 
            this.cmbDateDelimiter.DropDownHeight = 100;
            this.cmbDateDelimiter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDateDelimiter.DropDownWidth = 120;
            this.cmbDateDelimiter.FormattingEnabled = true;
            this.cmbDateDelimiter.IntegralHeight = false;
            this.cmbDateDelimiter.Items.AddRange(new object[] {
            "/",
            "-",
            "."});
            this.cmbDateDelimiter.Location = new System.Drawing.Point(403, 67);
            this.cmbDateDelimiter.Name = "cmbDateDelimiter";
            this.cmbDateDelimiter.Size = new System.Drawing.Size(46, 23);
            this.cmbDateDelimiter.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(311, 70);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 15);
            this.label6.TabIndex = 1;
            this.label6.Text = "Date delimiter";
            // 
            // cmbTimeFormat
            // 
            this.cmbTimeFormat.DropDownHeight = 100;
            this.cmbTimeFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTimeFormat.DropDownWidth = 120;
            this.cmbTimeFormat.FormattingEnabled = true;
            this.cmbTimeFormat.IntegralHeight = false;
            this.cmbTimeFormat.Items.AddRange(new object[] {
            "hh:mm:ss tt",
            "HH:mm:ss",
            "H:m:s",
            "h:m:s tt"});
            this.cmbTimeFormat.Location = new System.Drawing.Point(146, 143);
            this.cmbTimeFormat.Name = "cmbTimeFormat";
            this.cmbTimeFormat.Size = new System.Drawing.Size(121, 23);
            this.cmbTimeFormat.TabIndex = 2;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(63, 146);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 15);
            this.label5.TabIndex = 1;
            this.label5.Text = "Time format";
            // 
            // cmbDateFormat
            // 
            this.cmbDateFormat.DropDownHeight = 107;
            this.cmbDateFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDateFormat.DropDownWidth = 120;
            this.cmbDateFormat.FormattingEnabled = true;
            this.cmbDateFormat.IntegralHeight = false;
            this.cmbDateFormat.Location = new System.Drawing.Point(146, 67);
            this.cmbDateFormat.Name = "cmbDateFormat";
            this.cmbDateFormat.Size = new System.Drawing.Size(121, 23);
            this.cmbDateFormat.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(65, 70);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(71, 15);
            this.label4.TabIndex = 1;
            this.label4.Text = "Date format";
            // 
            // btnApply
            // 
            this.btnApply.Location = new System.Drawing.Point(402, 327);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(75, 23);
            this.btnApply.TabIndex = 1;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(493, 327);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // Options
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.ClientSize = new System.Drawing.Size(584, 362);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnApply);
            this.Controls.Add(this.tabOption);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Options";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Options";
            this.tabOption.ResumeLayout(false);
            this.tpGraph.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tpDate.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabOption;
        private System.Windows.Forms.TabPage tpGraph;
        private System.Windows.Forms.TabPage tpDate;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbIdealRange;
        private System.Windows.Forms.TextBox tbAlarmLimit;
        private System.Windows.Forms.TextBox tbTempCurve;
        private System.Windows.Forms.CheckBox cbFillRange;
        private System.Windows.Forms.CheckBox cbShowMark;
        private System.Windows.Forms.CheckBox cbShowLimit;
        private System.Windows.Forms.Label lbTempRgb;
        private System.Windows.Forms.Label lbLimitRgb;
        private System.Windows.Forms.Label lbRangeRgb;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbDateFormat;
        private System.Windows.Forms.ComboBox cmbTimeFormat;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbDateDelimiter;
        private System.Windows.Forms.Label label6;
    }
}