namespace ShineTech.TempCentre.DeviceManage
{
    partial class TrialValidation
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TrialValidation));
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lbTrialText = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.key1 = new System.Windows.Forms.MaskedTextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.key2 = new System.Windows.Forms.MaskedTextBox();
            this.key3 = new System.Windows.Forms.MaskedTextBox();
            this.key4 = new System.Windows.Forms.MaskedTextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 12F);
            this.label1.Location = new System.Drawing.Point(46, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(134, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Enter Product Key";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lbTrialText);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.groupBox1.Font = new System.Drawing.Font("Arial", 9F);
            this.groupBox1.Location = new System.Drawing.Point(49, 39);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(327, 94);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            // 
            // lbTrialText
            // 
            this.lbTrialText.Font = new System.Drawing.Font("Arial", 9F);
            this.lbTrialText.Location = new System.Drawing.Point(7, 41);
            this.lbTrialText.Name = "lbTrialText";
            this.lbTrialText.Size = new System.Drawing.Size(314, 31);
            this.lbTrialText.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 9F);
            this.label2.Location = new System.Drawing.Point(6, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(156, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Product Activation Required";
            // 
            // key1
            // 
            this.key1.AsciiOnly = true;
            this.key1.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.key1.HidePromptOnLeave = true;
            this.key1.Location = new System.Drawing.Point(49, 149);
            this.key1.Mask = "9999";
            this.key1.Name = "key1";
            this.key1.PromptChar = ' ';
            this.key1.Size = new System.Drawing.Size(43, 23);
            this.key1.TabIndex = 0;
            this.key1.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.key1.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Arial", 9F);
            this.button1.Location = new System.Drawing.Point(301, 149);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Continue";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // key2
            // 
            this.key2.AsciiOnly = true;
            this.key2.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.key2.HidePromptOnLeave = true;
            this.key2.Location = new System.Drawing.Point(112, 149);
            this.key2.Mask = "9999";
            this.key2.Name = "key2";
            this.key2.PromptChar = ' ';
            this.key2.Size = new System.Drawing.Size(43, 23);
            this.key2.TabIndex = 1;
            this.key2.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.key2.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // key3
            // 
            this.key3.AsciiOnly = true;
            this.key3.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.key3.HidePromptOnLeave = true;
            this.key3.Location = new System.Drawing.Point(175, 149);
            this.key3.Mask = "9999";
            this.key3.Name = "key3";
            this.key3.PromptChar = ' ';
            this.key3.Size = new System.Drawing.Size(43, 23);
            this.key3.TabIndex = 2;
            this.key3.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.key3.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // key4
            // 
            this.key4.AsciiOnly = true;
            this.key4.Font = new System.Drawing.Font("Arial", 10F, System.Drawing.FontStyle.Bold);
            this.key4.HidePromptOnLeave = true;
            this.key4.Location = new System.Drawing.Point(238, 149);
            this.key4.Mask = "9999";
            this.key4.Name = "key4";
            this.key4.PromptChar = ' ';
            this.key4.Size = new System.Drawing.Size(43, 23);
            this.key4.TabIndex = 3;
            this.key4.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.key4.TextChanged += new System.EventHandler(this.OnTextChanged);
            // 
            // TrialValidation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.ClientSize = new System.Drawing.Size(424, 201);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.key4);
            this.Controls.Add(this.key3);
            this.Controls.Add(this.key2);
            this.Controls.Add(this.key1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial Narrow", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "TrialValidation";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "TrialValidation";
            this.Load += new System.EventHandler(this.TrialValidation_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.MaskedTextBox key1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbTrialText;
        private System.Windows.Forms.MaskedTextBox key2;
        private System.Windows.Forms.MaskedTextBox key3;
        private System.Windows.Forms.MaskedTextBox key4;
    }
}