namespace ShineTech.TempCentre.BusinessFacade
{
    partial class ViewTool
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
            this.rbDtaPoints = new System.Windows.Forms.RadioButton();
            this.rbElapsedTime = new System.Windows.Forms.RadioButton();
            this.rbDateTime = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.cbLowLimit = new System.Windows.Forms.CheckBox();
            this.cbHighLimit = new System.Windows.Forms.CheckBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // rbDtaPoints
            // 
            this.rbDtaPoints.AutoSize = true;
            this.rbDtaPoints.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbDtaPoints.Location = new System.Drawing.Point(16, 91);
            this.rbDtaPoints.Margin = new System.Windows.Forms.Padding(4);
            this.rbDtaPoints.Name = "rbDtaPoints";
            this.rbDtaPoints.Size = new System.Drawing.Size(83, 20);
            this.rbDtaPoints.TabIndex = 2;
            this.rbDtaPoints.TabStop = true;
            this.rbDtaPoints.Text = "Data Points";
            this.rbDtaPoints.UseVisualStyleBackColor = true;
            // 
            // rbElapsedTime
            // 
            this.rbElapsedTime.AutoSize = true;
            this.rbElapsedTime.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbElapsedTime.Location = new System.Drawing.Point(16, 65);
            this.rbElapsedTime.Margin = new System.Windows.Forms.Padding(4);
            this.rbElapsedTime.Name = "rbElapsedTime";
            this.rbElapsedTime.Size = new System.Drawing.Size(91, 20);
            this.rbElapsedTime.TabIndex = 2;
            this.rbElapsedTime.TabStop = true;
            this.rbElapsedTime.Text = "Elapsed Time";
            this.rbElapsedTime.UseVisualStyleBackColor = true;
            // 
            // rbDateTime
            // 
            this.rbDateTime.AutoSize = true;
            this.rbDateTime.Checked = true;
            this.rbDateTime.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rbDateTime.Location = new System.Drawing.Point(16, 37);
            this.rbDateTime.Margin = new System.Windows.Forms.Padding(4);
            this.rbDateTime.Name = "rbDateTime";
            this.rbDateTime.Size = new System.Drawing.Size(78, 20);
            this.rbDateTime.TabIndex = 2;
            this.rbDateTime.TabStop = true;
            this.rbDateTime.Text = "Date/Time";
            this.rbDateTime.UseVisualStyleBackColor = true;
            this.rbDateTime.CheckedChanged += new System.EventHandler(this.AxisTitle);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 7.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(6, 11);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(121, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "X Axis synchronization";
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Font = new System.Drawing.Font("微软雅黑", 7F);
            this.checkBox3.Location = new System.Drawing.Point(16, 63);
            this.checkBox3.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(81, 20);
            this.checkBox3.TabIndex = 0;
            this.checkBox3.Text = "Show Mark";
            this.checkBox3.UseVisualStyleBackColor = true;
            // 
            // cbLowLimit
            // 
            this.cbLowLimit.AutoSize = true;
            this.cbLowLimit.Font = new System.Drawing.Font("微软雅黑", 7F);
            this.cbLowLimit.Location = new System.Drawing.Point(16, 35);
            this.cbLowLimit.Margin = new System.Windows.Forms.Padding(4);
            this.cbLowLimit.Name = "cbLowLimit";
            this.cbLowLimit.Size = new System.Drawing.Size(102, 20);
            this.cbLowLimit.TabIndex = 0;
            this.cbLowLimit.Text = "Show Low Limit";
            this.cbLowLimit.UseVisualStyleBackColor = true;
            this.cbLowLimit.CheckedChanged += new System.EventHandler(this.AxisTitle);
            // 
            // cbHighLimit
            // 
            this.cbHighLimit.AutoSize = true;
            this.cbHighLimit.Font = new System.Drawing.Font("微软雅黑", 7F);
            this.cbHighLimit.Location = new System.Drawing.Point(16, 7);
            this.cbHighLimit.Margin = new System.Windows.Forms.Padding(4);
            this.cbHighLimit.Name = "cbHighLimit";
            this.cbHighLimit.Size = new System.Drawing.Size(106, 20);
            this.cbHighLimit.TabIndex = 0;
            this.cbHighLimit.Text = "Show High Limit";
            this.cbHighLimit.UseVisualStyleBackColor = true;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Image = global::ShineTech.TempCentre.BusinessFacade.Properties.Resources.Graph_more;
            this.pictureBox1.Location = new System.Drawing.Point(139, 3);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(20, 20);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox2.Image = global::ShineTech.TempCentre.BusinessFacade.Properties.Resources.Graph_back;
            this.pictureBox2.Location = new System.Drawing.Point(96, 3);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(20, 20);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 4;
            this.pictureBox2.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel1.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 34);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45.68966F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 54.31034F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(168, 233);
            this.tableLayoutPanel1.TabIndex = 5;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.cbHighLimit);
            this.panel1.Controls.Add(this.cbLowLimit);
            this.panel1.Controls.Add(this.checkBox3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(5, 5);
            this.panel1.Margin = new System.Windows.Forms.Padding(4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(158, 97);
            this.panel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.rbDtaPoints);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.rbElapsedTime);
            this.panel2.Controls.Add(this.rbDateTime);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(5, 111);
            this.panel2.Margin = new System.Windows.Forms.Padding(4);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(158, 117);
            this.panel2.TabIndex = 0;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.pictureBox1);
            this.panel3.Controls.Add(this.pictureBox2);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Margin = new System.Windows.Forms.Padding(4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(168, 26);
            this.panel3.TabIndex = 6;
            // 
            // ViewTool
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "ViewTool";
            this.Size = new System.Drawing.Size(168, 267);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.CheckBox cbLowLimit;
        private System.Windows.Forms.CheckBox cbHighLimit;
        private System.Windows.Forms.RadioButton rbDtaPoints;
        private System.Windows.Forms.RadioButton rbElapsedTime;
        private System.Windows.Forms.RadioButton rbDateTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel3;

    }
}
