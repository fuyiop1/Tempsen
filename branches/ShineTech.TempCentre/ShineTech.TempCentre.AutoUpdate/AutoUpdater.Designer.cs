namespace ShineTech.TempCentre.AutoUpdate
{
    partial class AutoUpdater
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoUpdater));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pnList = new System.Windows.Forms.Panel();
            this.lbStatus = new System.Windows.Forms.Label();
            this.progressDownload = new System.Windows.Forms.ProgressBar();
            this.lvUpdateList = new System.Windows.Forms.ListView();
            this.clmFileName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmVersion = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.clmProgress = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label1 = new System.Windows.Forms.Label();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnFinish = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.pnList.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::ShineTech.TempCentre.AutoUpdate.Properties.Resources.update;
            this.pictureBox1.Location = new System.Drawing.Point(12, 11);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(96, 240);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // pnList
            // 
            this.pnList.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnList.Controls.Add(this.lbStatus);
            this.pnList.Controls.Add(this.progressDownload);
            this.pnList.Controls.Add(this.lvUpdateList);
            this.pnList.Controls.Add(this.label1);
            this.pnList.Location = new System.Drawing.Point(123, 11);
            this.pnList.Name = "pnList";
            this.pnList.Size = new System.Drawing.Size(289, 240);
            this.pnList.TabIndex = 3;
            // 
            // lbStatus
            // 
            this.lbStatus.AutoSize = true;
            this.lbStatus.ForeColor = System.Drawing.Color.Blue;
            this.lbStatus.Location = new System.Drawing.Point(6, 191);
            this.lbStatus.Name = "lbStatus";
            this.lbStatus.Size = new System.Drawing.Size(145, 15);
            this.lbStatus.TabIndex = 3;
            this.lbStatus.Text = "Click \'Next\' to update files";
            // 
            // progressDownload
            // 
            this.progressDownload.Location = new System.Drawing.Point(9, 211);
            this.progressDownload.Name = "progressDownload";
            this.progressDownload.Size = new System.Drawing.Size(271, 16);
            this.progressDownload.TabIndex = 2;
            // 
            // lvUpdateList
            // 
            this.lvUpdateList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.clmFileName,
            this.clmVersion,
            this.clmProgress});
            this.lvUpdateList.Location = new System.Drawing.Point(9, 39);
            this.lvUpdateList.Name = "lvUpdateList";
            this.lvUpdateList.Size = new System.Drawing.Size(271, 149);
            this.lvUpdateList.TabIndex = 1;
            this.lvUpdateList.UseCompatibleStateImageBehavior = false;
            this.lvUpdateList.View = System.Windows.Forms.View.Details;
            // 
            // clmFileName
            // 
            this.clmFileName.Text = "Component";
            this.clmFileName.Width = 100;
            // 
            // clmVersion
            // 
            this.clmVersion.Text = "Version";
            this.clmVersion.Width = 75;
            // 
            // clmProgress
            // 
            this.clmProgress.Text = "Progress";
            this.clmProgress.Width = 70;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(191, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "The files to be updated in list view";
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(233, 270);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 4;
            this.btnNext.Text = "&Next";
            this.btnNext.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(314, 270);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "&Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnFinish
            // 
            this.btnFinish.Location = new System.Drawing.Point(152, 270);
            this.btnFinish.Name = "btnFinish";
            this.btnFinish.Size = new System.Drawing.Size(75, 23);
            this.btnFinish.TabIndex = 4;
            this.btnFinish.Text = "&Finish";
            this.btnFinish.UseVisualStyleBackColor = true;
            // 
            // AutoUpdater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(424, 302);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnFinish);
            this.Controls.Add(this.btnNext);
            this.Controls.Add(this.pnList);
            this.Controls.Add(this.pictureBox1);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AutoUpdater";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AutoUpdate";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.pnList.ResumeLayout(false);
            this.pnList.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel pnList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ListView lvUpdateList;
        private System.Windows.Forms.ColumnHeader clmFileName;
        private System.Windows.Forms.ColumnHeader clmVersion;
        private System.Windows.Forms.ColumnHeader clmProgress;
        private System.Windows.Forms.Label lbStatus;
        private System.Windows.Forms.ProgressBar progressDownload;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnFinish;
    }
}

