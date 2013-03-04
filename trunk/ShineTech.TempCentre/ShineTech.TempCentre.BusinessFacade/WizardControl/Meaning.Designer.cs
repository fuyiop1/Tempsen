namespace ShineTech.TempCentre.BusinessFacade
{
    partial class Meaning
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Meaning));
            this.cbEdit = new System.Windows.Forms.CheckBox();
            this.tbMean = new System.Windows.Forms.TextBox();
            this.pbDel = new System.Windows.Forms.PictureBox();
            this.pbEdit = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pbDel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEdit)).BeginInit();
            this.SuspendLayout();
            // 
            // cbEdit
            // 
            this.cbEdit.AutoSize = true;
            this.cbEdit.Font = new System.Drawing.Font("Microsoft YaHei", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cbEdit.Location = new System.Drawing.Point(4, 6);
            this.cbEdit.Name = "cbEdit";
            this.cbEdit.Size = new System.Drawing.Size(15, 14);
            this.cbEdit.TabIndex = 1;
            this.cbEdit.UseVisualStyleBackColor = true;
            // 
            // tbMean
            // 
            this.tbMean.Font = new System.Drawing.Font("Microsoft YaHei", 7.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbMean.Location = new System.Drawing.Point(25, 1);
            this.tbMean.Name = "tbMean";
            this.tbMean.Size = new System.Drawing.Size(170, 21);
            this.tbMean.TabIndex = 0;
            // 
            // pbDel
            // 
            this.pbDel.Image = ((System.Drawing.Image)(resources.GetObject("pbDel.Image")));
            this.pbDel.Location = new System.Drawing.Point(202, 2);
            this.pbDel.Name = "pbDel";
            this.pbDel.Size = new System.Drawing.Size(19, 19);
            this.pbDel.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbDel.TabIndex = 2;
            this.pbDel.TabStop = false;
            this.pbDel.Visible = false;
            // 
            // pbEdit
            // 
            this.pbEdit.Image = ((System.Drawing.Image)(resources.GetObject("pbEdit.Image")));
            this.pbEdit.Location = new System.Drawing.Point(228, 2);
            this.pbEdit.Name = "pbEdit";
            this.pbEdit.Size = new System.Drawing.Size(19, 19);
            this.pbEdit.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbEdit.TabIndex = 2;
            this.pbEdit.TabStop = false;
            this.pbEdit.Visible = false;
            // 
            // Meaning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pbEdit);
            this.Controls.Add(this.pbDel);
            this.Controls.Add(this.tbMean);
            this.Controls.Add(this.cbEdit);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Meaning";
            this.Size = new System.Drawing.Size(250, 24);
            ((System.ComponentModel.ISupportInitialize)(this.pbDel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbEdit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox cbEdit;
        private System.Windows.Forms.TextBox tbMean;
        private System.Windows.Forms.PictureBox pbDel;
        private System.Windows.Forms.PictureBox pbEdit;
    }
}
