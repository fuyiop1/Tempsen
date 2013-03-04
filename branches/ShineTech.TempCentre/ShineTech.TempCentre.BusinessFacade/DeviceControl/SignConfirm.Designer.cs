namespace ShineTech.TempCentre.BusinessFacade
{
    partial class SignConfirm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SignConfirm));
            this.lbPwd = new System.Windows.Forms.Label();
            this.lbAccount = new System.Windows.Forms.Label();
            this.tbPwd = new System.Windows.Forms.TextBox();
            this.tbAccount = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnConfirm = new System.Windows.Forms.Button();
            this.gbSign = new System.Windows.Forms.GroupBox();
            this.clbMeanings = new System.Windows.Forms.CheckedListBox();
            this.btnSign = new System.Windows.Forms.Button();
            this.meaningTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbSign.SuspendLayout();
            this.SuspendLayout();
            // 
            // lbPwd
            // 
            this.lbPwd.AutoSize = true;
            this.lbPwd.ForeColor = System.Drawing.Color.Green;
            this.lbPwd.Location = new System.Drawing.Point(307, 65);
            this.lbPwd.Name = "lbPwd";
            this.lbPwd.Size = new System.Drawing.Size(0, 15);
            this.lbPwd.TabIndex = 9;
            // 
            // lbAccount
            // 
            this.lbAccount.AutoSize = true;
            this.lbAccount.ForeColor = System.Drawing.Color.Green;
            this.lbAccount.Location = new System.Drawing.Point(307, 34);
            this.lbAccount.Name = "lbAccount";
            this.lbAccount.Size = new System.Drawing.Size(0, 15);
            this.lbAccount.TabIndex = 10;
            // 
            // tbPwd
            // 
            this.tbPwd.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbPwd.Location = new System.Drawing.Point(88, 52);
            this.tbPwd.Name = "tbPwd";
            this.tbPwd.Size = new System.Drawing.Size(296, 21);
            this.tbPwd.TabIndex = 1;
            this.tbPwd.UseSystemPasswordChar = true;
            // 
            // tbAccount
            // 
            this.tbAccount.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.tbAccount.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbAccount.Location = new System.Drawing.Point(88, 20);
            this.tbAccount.Name = "tbAccount";
            this.tbAccount.Size = new System.Drawing.Size(296, 21);
            this.tbAccount.TabIndex = 0;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(10, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(10, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "User Name";
            // 
            // btnConfirm
            // 
            this.btnConfirm.BackColor = System.Drawing.Color.Transparent;
            this.btnConfirm.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnConfirm.ForeColor = System.Drawing.Color.Black;
            this.btnConfirm.Location = new System.Drawing.Point(314, 89);
            this.btnConfirm.Name = "btnConfirm";
            this.btnConfirm.Size = new System.Drawing.Size(70, 23);
            this.btnConfirm.TabIndex = 2;
            this.btnConfirm.Text = "&Confirm";
            this.btnConfirm.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnConfirm.UseVisualStyleBackColor = false;
            // 
            // gbSign
            // 
            this.gbSign.Controls.Add(this.clbMeanings);
            this.gbSign.Controls.Add(this.btnSign);
            this.gbSign.Location = new System.Drawing.Point(10, 0);
            this.gbSign.Name = "gbSign";
            this.gbSign.Size = new System.Drawing.Size(380, 124);
            this.gbSign.TabIndex = 14;
            this.gbSign.TabStop = false;
            this.gbSign.Text = "Available Meanings";
            this.gbSign.Visible = false;
            // 
            // clbMeanings
            // 
            this.clbMeanings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.clbMeanings.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.clbMeanings.CheckOnClick = true;
            this.clbMeanings.ColumnWidth = 100;
            this.clbMeanings.FormattingEnabled = true;
            this.clbMeanings.Location = new System.Drawing.Point(6, 20);
            this.clbMeanings.Name = "clbMeanings";
            this.clbMeanings.Size = new System.Drawing.Size(368, 64);
            this.clbMeanings.TabIndex = 0;
            // 
            // btnSign
            // 
            this.btnSign.BackColor = System.Drawing.Color.Transparent;
            this.btnSign.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSign.ForeColor = System.Drawing.Color.Black;
            this.btnSign.Location = new System.Drawing.Point(304, 93);
            this.btnSign.Name = "btnSign";
            this.btnSign.Size = new System.Drawing.Size(70, 23);
            this.btnSign.TabIndex = 13;
            this.btnSign.Text = "&Sign";
            this.btnSign.UseVisualStyleBackColor = false;
            // 
            // SignConfirm
            // 
            this.AcceptButton = this.btnConfirm;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.ClientSize = new System.Drawing.Size(399, 122);
            this.Controls.Add(this.gbSign);
            this.Controls.Add(this.btnConfirm);
            this.Controls.Add(this.lbPwd);
            this.Controls.Add(this.lbAccount);
            this.Controls.Add(this.tbPwd);
            this.Controls.Add(this.tbAccount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Arial", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SignConfirm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Electronic Signature";
            this.gbSign.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbPwd;
        private System.Windows.Forms.Label lbAccount;
        private System.Windows.Forms.TextBox tbPwd;
        private System.Windows.Forms.TextBox tbAccount;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnConfirm;
        private System.Windows.Forms.GroupBox gbSign;
        private System.Windows.Forms.CheckedListBox clbMeanings;
        private System.Windows.Forms.Button btnSign;
        private System.Windows.Forms.ToolTip meaningTip;
    }
}