namespace TempCentreProductKeyGen
{
    partial class KeyGen
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KeyGen));
            this.label1 = new System.Windows.Forms.Label();
            this.tbItemCount = new System.Windows.Forms.TextBox();
            this.dgvKeyList = new System.Windows.Forms.DataGridView();
            this.btnExtract = new System.Windows.Forms.Button();
            this.btnGen = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvKeyList)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "Key Item Count";
            // 
            // tbItemCount
            // 
            this.tbItemCount.Location = new System.Drawing.Point(108, 14);
            this.tbItemCount.Name = "tbItemCount";
            this.tbItemCount.Size = new System.Drawing.Size(64, 21);
            this.tbItemCount.TabIndex = 1;
            this.tbItemCount.Text = "100";
            // 
            // dgvKeyList
            // 
            this.dgvKeyList.AllowUserToAddRows = false;
            this.dgvKeyList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvKeyList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvKeyList.Location = new System.Drawing.Point(2, 54);
            this.dgvKeyList.Name = "dgvKeyList";
            this.dgvKeyList.RowHeadersVisible = false;
            this.dgvKeyList.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            this.dgvKeyList.RowTemplate.Height = 23;
            this.dgvKeyList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvKeyList.Size = new System.Drawing.Size(337, 309);
            this.dgvKeyList.TabIndex = 2;
            // 
            // btnExtract
            // 
            this.btnExtract.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExtract.Location = new System.Drawing.Point(275, 14);
            this.btnExtract.Name = "btnExtract";
            this.btnExtract.Size = new System.Drawing.Size(53, 23);
            this.btnExtract.TabIndex = 3;
            this.btnExtract.Text = "&Extract";
            this.btnExtract.UseVisualStyleBackColor = true;
            this.btnExtract.Click += new System.EventHandler(this.btnExtract_Click);
            // 
            // btnGen
            // 
            this.btnGen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGen.Location = new System.Drawing.Point(201, 14);
            this.btnGen.Name = "btnGen";
            this.btnGen.Size = new System.Drawing.Size(53, 23);
            this.btnGen.TabIndex = 3;
            this.btnGen.Text = "&Gen";
            this.btnGen.UseVisualStyleBackColor = true;
            this.btnGen.Click += new System.EventHandler(this.btnGen_Click);
            // 
            // KeyGen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(340, 363);
            this.Controls.Add(this.btnGen);
            this.Controls.Add(this.btnExtract);
            this.Controls.Add(this.dgvKeyList);
            this.Controls.Add(this.tbItemCount);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MinimumSize = new System.Drawing.Size(356, 401);
            this.Name = "KeyGen";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Key Generator";
            ((System.ComponentModel.ISupportInitialize)(this.dgvKeyList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbItemCount;
        private System.Windows.Forms.DataGridView dgvKeyList;
        private System.Windows.Forms.Button btnExtract;
        private System.Windows.Forms.Button btnGen;
    }
}