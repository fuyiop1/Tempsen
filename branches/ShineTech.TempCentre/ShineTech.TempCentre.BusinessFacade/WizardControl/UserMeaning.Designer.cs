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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.layOutPn = new System.Windows.Forms.FlowLayoutPanel();
            this.label3 = new System.Windows.Forms.Label();
            this.btnDeleteMean = new System.Windows.Forms.Button();
            this.btnEditMean = new System.Windows.Forms.Button();
            this.btnAddMean = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.pnMean.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnMean
            // 
            this.pnMean.Controls.Add(this.tableLayoutPanel1);
            this.pnMean.Controls.Add(this.btnDeleteMean);
            this.pnMean.Controls.Add(this.btnEditMean);
            this.pnMean.Controls.Add(this.btnAddMean);
            this.pnMean.Controls.Add(this.label2);
            this.pnMean.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnMean.Font = new System.Drawing.Font("Arial", 9F);
            this.pnMean.Location = new System.Drawing.Point(0, 0);
            this.pnMean.Name = "pnMean";
            this.pnMean.Size = new System.Drawing.Size(594, 270);
            this.pnMean.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.layOutPn, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(15, 75);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 132F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(479, 160);
            this.tableLayoutPanel1.TabIndex = 7;
            // 
            // layOutPn
            // 
            this.layOutPn.Dock = System.Windows.Forms.DockStyle.Fill;
            this.layOutPn.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.layOutPn.Location = new System.Drawing.Point(0, 28);
            this.layOutPn.Margin = new System.Windows.Forms.Padding(0);
            this.layOutPn.Name = "layOutPn";
            this.layOutPn.Size = new System.Drawing.Size(479, 132);
            this.layOutPn.TabIndex = 0;
            this.layOutPn.TabStop = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Arial", 9F);
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(479, 28);
            this.label3.TabIndex = 2;
            this.label3.Text = "Available Meanings";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnDeleteMean
            // 
            this.btnDeleteMean.Font = new System.Drawing.Font("Arial", 9F);
            this.btnDeleteMean.Location = new System.Drawing.Point(509, 209);
            this.btnDeleteMean.Name = "btnDeleteMean";
            this.btnDeleteMean.Size = new System.Drawing.Size(70, 25);
            this.btnDeleteMean.TabIndex = 6;
            this.btnDeleteMean.Text = "Delete";
            this.btnDeleteMean.UseVisualStyleBackColor = true;
            // 
            // btnEditMean
            // 
            this.btnEditMean.Font = new System.Drawing.Font("Arial", 9F);
            this.btnEditMean.Location = new System.Drawing.Point(509, 156);
            this.btnEditMean.Name = "btnEditMean";
            this.btnEditMean.Size = new System.Drawing.Size(70, 25);
            this.btnEditMean.TabIndex = 5;
            this.btnEditMean.Text = "Edit";
            this.btnEditMean.UseVisualStyleBackColor = true;
            // 
            // btnAddMean
            // 
            this.btnAddMean.BackColor = System.Drawing.Color.Transparent;
            this.btnAddMean.Font = new System.Drawing.Font("Arial", 9F);
            this.btnAddMean.Location = new System.Drawing.Point(509, 103);
            this.btnAddMean.Name = "btnAddMean";
            this.btnAddMean.Size = new System.Drawing.Size(70, 25);
            this.btnAddMean.TabIndex = 1;
            this.btnAddMean.Text = "Add";
            this.btnAddMean.UseVisualStyleBackColor = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F);
            this.label2.Location = new System.Drawing.Point(15, 30);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(433, 15);
            this.label2.TabIndex = 2;
            this.label2.Text = "Please assign appropriate meanings to user for electronic signature purpose.";
            // 
            // UserMeaning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnMean);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UserMeaning";
            this.Size = new System.Drawing.Size(594, 270);
            this.pnMean.ResumeLayout(false);
            this.pnMean.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnMean;
        private System.Windows.Forms.FlowLayoutPanel layOutPn;
        private System.Windows.Forms.Button btnAddMean;
        private System.Windows.Forms.Button btnDeleteMean;
        private System.Windows.Forms.Button btnEditMean;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label3;
    }
}
