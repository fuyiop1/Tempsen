namespace ShineTech.TempCentre.BusinessFacade
{
    partial class UserPolicy
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
            this.pnPolicy = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.mtbLocked = new System.Windows.Forms.MaskedTextBox();
            this.mtbInactivity = new System.Windows.Forms.MaskedTextBox();
            this.mtbPwdExpired = new System.Windows.Forms.MaskedTextBox();
            this.mtbPwdSize = new System.Windows.Forms.MaskedTextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.pnPolicy.SuspendLayout();
            this.SuspendLayout();
            // 
            // pnPolicy
            // 
            this.pnPolicy.Controls.Add(this.label9);
            this.pnPolicy.Controls.Add(this.label10);
            this.pnPolicy.Controls.Add(this.label16);
            this.pnPolicy.Controls.Add(this.label11);
            this.pnPolicy.Controls.Add(this.mtbLocked);
            this.pnPolicy.Controls.Add(this.mtbInactivity);
            this.pnPolicy.Controls.Add(this.mtbPwdExpired);
            this.pnPolicy.Controls.Add(this.mtbPwdSize);
            this.pnPolicy.Controls.Add(this.label12);
            this.pnPolicy.Controls.Add(this.label13);
            this.pnPolicy.Controls.Add(this.label14);
            this.pnPolicy.Controls.Add(this.label15);
            this.pnPolicy.Controls.Add(this.label8);
            this.pnPolicy.Controls.Add(this.panel4);
            this.pnPolicy.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnPolicy.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.pnPolicy.Location = new System.Drawing.Point(0, 0);
            this.pnPolicy.Name = "pnPolicy";
            this.pnPolicy.Size = new System.Drawing.Size(594, 310);
            this.pnPolicy.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label9.Location = new System.Drawing.Point(357, 206);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(115, 20);
            this.label9.TabIndex = 11;
            this.label9.Text = "logon attempts.";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label10.Location = new System.Drawing.Point(357, 174);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(66, 20);
            this.label10.TabIndex = 10;
            this.label10.Text = "minutes.";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label16.Location = new System.Drawing.Point(357, 109);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(80, 20);
            this.label16.TabIndex = 12;
            this.label16.Text = "characters.";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label11.Location = new System.Drawing.Point(358, 141);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(42, 20);
            this.label11.TabIndex = 12;
            this.label11.Text = "days.";
            // 
            // mtbLocked
            // 
            this.mtbLocked.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mtbLocked.Location = new System.Drawing.Point(307, 203);
            this.mtbLocked.Mask = "9";
            this.mtbLocked.Name = "mtbLocked";
            this.mtbLocked.PromptChar = ' ';
            this.mtbLocked.Size = new System.Drawing.Size(39, 26);
            this.mtbLocked.TabIndex = 3;
            // 
            // mtbInactivity
            // 
            this.mtbInactivity.Location = new System.Drawing.Point(308, 171);
            this.mtbInactivity.Mask = "999";
            this.mtbInactivity.Name = "mtbInactivity";
            this.mtbInactivity.PromptChar = ' ';
            this.mtbInactivity.Size = new System.Drawing.Size(39, 26);
            this.mtbInactivity.TabIndex = 2;
            // 
            // mtbPwdExpired
            // 
            this.mtbPwdExpired.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mtbPwdExpired.Location = new System.Drawing.Point(308, 136);
            this.mtbPwdExpired.Mask = "999";
            this.mtbPwdExpired.Name = "mtbPwdExpired";
            this.mtbPwdExpired.PromptChar = ' ';
            this.mtbPwdExpired.Size = new System.Drawing.Size(39, 26);
            this.mtbPwdExpired.TabIndex = 1;
            // 
            // mtbPwdSize
            // 
            this.mtbPwdSize.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.mtbPwdSize.Location = new System.Drawing.Point(308, 103);
            this.mtbPwdSize.Mask = "99";
            this.mtbPwdSize.Name = "mtbPwdSize";
            this.mtbPwdSize.PromptChar = ' ';
            this.mtbPwdSize.Size = new System.Drawing.Size(39, 26);
            this.mtbPwdSize.TabIndex = 0;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label12.Location = new System.Drawing.Point(165, 206);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(129, 20);
            this.label12.TabIndex = 4;
            this.label12.Text = "Disable user after:";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label13.Location = new System.Drawing.Point(137, 136);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(161, 20);
            this.label13.TabIndex = 6;
            this.label13.Text = "Password expires after:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label14.Location = new System.Drawing.Point(132, 171);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(162, 20);
            this.label14.TabIndex = 8;
            this.label14.Text = "Lock user session after:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label15.Location = new System.Drawing.Point(101, 106);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(200, 20);
            this.label15.TabIndex = 7;
            this.label15.Text = "Password must have at least:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(12, 9);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(105, 21);
            this.label8.TabIndex = 11;
            this.label8.Text = "User Policies";
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Location = new System.Drawing.Point(3, 46);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(591, 1);
            this.panel4.TabIndex = 12;
            // 
            // UserPolicy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.pnPolicy);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "UserPolicy";
            this.Size = new System.Drawing.Size(594, 310);
            this.pnPolicy.ResumeLayout(false);
            this.pnPolicy.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel pnPolicy;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.MaskedTextBox mtbLocked;
        private System.Windows.Forms.MaskedTextBox mtbInactivity;
        private System.Windows.Forms.MaskedTextBox mtbPwdExpired;
        private System.Windows.Forms.MaskedTextBox mtbPwdSize;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Panel panel4;
    }
}
