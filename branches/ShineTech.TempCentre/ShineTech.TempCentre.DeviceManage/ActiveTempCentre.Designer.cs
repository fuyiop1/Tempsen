namespace ShineTech.TempCentre.DeviceManage
{
    partial class ActiveTempCentre
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ActiveTempCentre));
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.key4 = new System.Windows.Forms.MaskedTextBox();
            this.key3 = new System.Windows.Forms.MaskedTextBox();
            this.key2 = new System.Windows.Forms.MaskedTextBox();
            this.key1 = new System.Windows.Forms.MaskedTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(273, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please enter CD-KEY in following fields to active TempCentre";
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Arial", 9F);
            this.button1.Location = new System.Drawing.Point(207, 137);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 9;
            this.button1.Text = "Continue";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // key4
            // 
            this.key4.AsciiOnly = true;
            this.key4.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.key4.HidePromptOnLeave = true;
            this.key4.Location = new System.Drawing.Point(220, 86);
            this.key4.Mask = "9999";
            this.key4.Name = "key4";
            this.key4.PromptChar = ' ';
            this.key4.Size = new System.Drawing.Size(43, 23);
            this.key4.TabIndex = 8;
            this.key4.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.key4.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // key3
            // 
            this.key3.AsciiOnly = true;
            this.key3.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.key3.HidePromptOnLeave = true;
            this.key3.Location = new System.Drawing.Point(157, 86);
            this.key3.Mask = "9999";
            this.key3.Name = "key3";
            this.key3.PromptChar = ' ';
            this.key3.Size = new System.Drawing.Size(43, 23);
            this.key3.TabIndex = 7;
            this.key3.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.key3.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // key2
            // 
            this.key2.AsciiOnly = true;
            this.key2.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.key2.HidePromptOnLeave = true;
            this.key2.Location = new System.Drawing.Point(94, 86);
            this.key2.Mask = "9999";
            this.key2.Name = "key2";
            this.key2.PromptChar = ' ';
            this.key2.Size = new System.Drawing.Size(43, 23);
            this.key2.TabIndex = 6;
            this.key2.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.key2.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // key1
            // 
            this.key1.AsciiOnly = true;
            this.key1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.key1.HidePromptOnLeave = true;
            this.key1.Location = new System.Drawing.Point(31, 86);
            this.key1.Mask = "9999";
            this.key1.Name = "key1";
            this.key1.PromptChar = ' ';
            this.key1.Size = new System.Drawing.Size(43, 23);
            this.key1.TabIndex = 5;
            this.key1.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.key1.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // ActiveTempCentre
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(294, 172);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.key4);
            this.Controls.Add(this.key3);
            this.Controls.Add(this.key2);
            this.Controls.Add(this.key1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "ActiveTempCentre";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Active TempCentre";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.MaskedTextBox key4;
        private System.Windows.Forms.MaskedTextBox key3;
        private System.Windows.Forms.MaskedTextBox key2;
        private System.Windows.Forms.MaskedTextBox key1;
    }
}