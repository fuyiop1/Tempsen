namespace ShineTech.TempCentre.BusinessFacade
{
    partial class UserRight
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
            this.pnRight = new System.Windows.Forms.Panel();
            this.btnRight = new System.Windows.Forms.Button();
            this.btnLeft = new System.Windows.Forms.Button();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbAvailbale = new System.Windows.Forms.ListBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbAssigned = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnRight.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnRight
            // 
            this.pnRight.Controls.Add(this.btnRight);
            this.pnRight.Controls.Add(this.btnLeft);
            this.pnRight.Controls.Add(this.tableLayoutPanel1);
            this.pnRight.Controls.Add(this.label1);
            this.pnRight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnRight.Font = new System.Drawing.Font("Arial", 9F);
            this.pnRight.Location = new System.Drawing.Point(0, 0);
            this.pnRight.Name = "pnRight";
            this.pnRight.Size = new System.Drawing.Size(594, 270);
            this.pnRight.TabIndex = 16;
            this.pnRight.Paint += new System.Windows.Forms.PaintEventHandler(this.pnRight_Paint);
            // 
            // btnRight
            // 
            this.btnRight.BackColor = System.Drawing.Color.Transparent;
            this.btnRight.ForeColor = System.Drawing.Color.Black;
            this.btnRight.Location = new System.Drawing.Point(286, 176);
            this.btnRight.Name = "btnRight";
            this.btnRight.Size = new System.Drawing.Size(22, 25);
            this.btnRight.TabIndex = 8;
            this.btnRight.Text = ">";
            this.btnRight.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnRight.UseVisualStyleBackColor = false;
            // 
            // btnLeft
            // 
            this.btnLeft.BackColor = System.Drawing.Color.Transparent;
            this.btnLeft.ForeColor = System.Drawing.Color.Black;
            this.btnLeft.Location = new System.Drawing.Point(286, 145);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Size = new System.Drawing.Size(22, 25);
            this.btnLeft.TabIndex = 8;
            this.btnLeft.Text = "<";
            this.btnLeft.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.btnLeft.UseVisualStyleBackColor = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.Single;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(15, 75);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 132F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(565, 160);
            this.tableLayoutPanel1.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label2.Font = new System.Drawing.Font("Arial", 9F);
            this.label2.Location = new System.Drawing.Point(4, 1);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(275, 28);
            this.label2.TabIndex = 5;
            this.label2.Text = "Available Rights";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Font = new System.Drawing.Font("Arial", 9F);
            this.label3.Location = new System.Drawing.Point(286, 1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(275, 28);
            this.label3.TabIndex = 5;
            this.label3.Text = "Assigned Rights";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lbAvailbale);
            this.panel1.Location = new System.Drawing.Point(1, 30);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(281, 128);
            this.panel1.TabIndex = 6;
            // 
            // lbAvailbale
            // 
            this.lbAvailbale.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbAvailbale.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lbAvailbale.FormattingEnabled = true;
            this.lbAvailbale.ItemHeight = 20;
            this.lbAvailbale.Location = new System.Drawing.Point(32, 5);
            this.lbAvailbale.Name = "lbAvailbale";
            this.lbAvailbale.Size = new System.Drawing.Size(230, 120);
            this.lbAvailbale.TabIndex = 7;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.lbAssigned);
            this.panel2.Location = new System.Drawing.Point(283, 30);
            this.panel2.Margin = new System.Windows.Forms.Padding(0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(281, 128);
            this.panel2.TabIndex = 7;
            // 
            // lbAssigned
            // 
            this.lbAssigned.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbAssigned.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.lbAssigned.FormattingEnabled = true;
            this.lbAssigned.ItemHeight = 20;
            this.lbAssigned.Location = new System.Drawing.Point(32, 5);
            this.lbAssigned.Name = "lbAssigned";
            this.lbAssigned.Size = new System.Drawing.Size(230, 120);
            this.lbAssigned.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 9F);
            this.label1.Location = new System.Drawing.Point(15, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(324, 15);
            this.label1.TabIndex = 5;
            this.label1.Text = "Please assign system rights for current user to be created";
            // 
            // UserRight
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnRight);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UserRight";
            this.Size = new System.Drawing.Size(594, 270);
            this.pnRight.ResumeLayout(false);
            this.pnRight.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnRight;
        private System.Windows.Forms.Button btnRight;
        private System.Windows.Forms.Button btnLeft;
        private System.Windows.Forms.ListBox lbAssigned;
        private System.Windows.Forms.ListBox lbAvailbale;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
    }
}
