namespace ShineTech.TempCentre.BusinessFacade
{
    partial class ProgressStatus
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
            this.bar = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // bar
            // 
            this.bar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.bar.Location = new System.Drawing.Point(0, 0);
            this.bar.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.bar.Name = "bar";
            this.bar.Size = new System.Drawing.Size(316, 22);
            this.bar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.bar.TabIndex = 0;
            // 
            // ProgressStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(316, 22);
            this.Controls.Add(this.bar);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "ProgressStatus";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "ProgressStatus";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar bar;
    }
}