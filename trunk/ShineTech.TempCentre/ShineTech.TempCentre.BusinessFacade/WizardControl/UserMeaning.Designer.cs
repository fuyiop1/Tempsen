namespace ShineTech.TempCentre.BusinessFacade
{
    partial class UserMeaning
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
            this.pnMean = new System.Windows.Forms.Panel();
            this.btnAdd = new System.Windows.Forms.Button();
            this.layOutPn = new System.Windows.Forms.FlowLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.pnMean.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnMean
            // 
            this.pnMean.Controls.Add(this.btnAdd);
            this.pnMean.Controls.Add(this.layOutPn);
            this.pnMean.Controls.Add(this.label1);
            this.pnMean.Controls.Add(this.panel2);
            this.pnMean.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnMean.Location = new System.Drawing.Point(0, 0);
            this.pnMean.Name = "pnMean";
            this.pnMean.Size = new System.Drawing.Size(594, 310);
            this.pnMean.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.White;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdd.Location = new System.Drawing.Point(484, 284);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(107, 23);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "&Add Meaning";
            this.btnAdd.UseVisualStyleBackColor = false;
            // 
            // layOutPn
            // 
            this.layOutPn.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.layOutPn.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.layOutPn.Location = new System.Drawing.Point(3, 50);
            this.layOutPn.Name = "layOutPn";
            this.layOutPn.Size = new System.Drawing.Size(475, 257);
            this.layOutPn.TabIndex = 0;
            this.layOutPn.TabStop = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 21);
            this.label1.TabIndex = 2;
            this.label1.Text = "Meanings";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Location = new System.Drawing.Point(0, 46);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(591, 1);
            this.panel2.TabIndex = 2;
            // 
            // UserMeaning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnMean);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UserMeaning";
            this.Size = new System.Drawing.Size(594, 310);
            this.pnMean.ResumeLayout(false);
            this.pnMean.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnMean;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.FlowLayoutPanel layOutPn;
        private System.Windows.Forms.Button btnAdd;
    }
}
