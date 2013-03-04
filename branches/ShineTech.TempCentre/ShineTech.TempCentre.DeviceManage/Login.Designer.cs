namespace ShineTech.TempCentre.DeviceManage
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.tbAccount = new System.Windows.Forms.TextBox();
            this.tbPwd = new System.Windows.Forms.TextBox();
            this.btnLogin = new System.Windows.Forms.Button();
            this.lbAccount = new System.Windows.Forms.Label();
            this.lbPwd = new System.Windows.Forms.Label();
            this.pnChangePwd = new System.Windows.Forms.Panel();
            this.pbConfirmNewPasswordTip = new System.Windows.Forms.PictureBox();
            this.pbNewPasswordTip = new System.Windows.Forms.PictureBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.tbConfirm = new System.Windows.Forms.TextBox();
            this.tbNewPwd = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbAlarmConfirm = new System.Windows.Forms.Label();
            this.lbAlarmNewPwd = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pnChangePwd.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbConfirmNewPasswordTip)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNewPasswordTip)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Arial", 9F);
            this.label1.Location = new System.Drawing.Point(45, 236);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "User Name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.Font = new System.Drawing.Font("Arial", 9F);
            this.label2.Location = new System.Drawing.Point(53, 275);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(66, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Password:";
            // 
            // tbAccount
            // 
            this.tbAccount.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Append;
            this.tbAccount.Font = new System.Drawing.Font("Arial", 9F);
            this.tbAccount.Location = new System.Drawing.Point(135, 233);
            this.tbAccount.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbAccount.Name = "tbAccount";
            this.tbAccount.Size = new System.Drawing.Size(152, 21);
            this.tbAccount.TabIndex = 0;
            // 
            // tbPwd
            // 
            this.tbPwd.Font = new System.Drawing.Font("Arial", 9F);
            this.tbPwd.Location = new System.Drawing.Point(135, 273);
            this.tbPwd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbPwd.MaxLength = 15;
            this.tbPwd.Name = "tbPwd";
            this.tbPwd.Size = new System.Drawing.Size(152, 21);
            this.tbPwd.TabIndex = 1;
            this.tbPwd.UseSystemPasswordChar = true;
            this.tbPwd.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbPwd_KeyPress);
            // 
            // btnLogin
            // 
            this.btnLogin.BackColor = System.Drawing.Color.Transparent;
            this.btnLogin.Font = new System.Drawing.Font("Arial", 9F);
            this.btnLogin.ForeColor = System.Drawing.Color.Black;
            this.btnLogin.Location = new System.Drawing.Point(374, 269);
            this.btnLogin.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnLogin.Name = "btnLogin";
            this.btnLogin.Size = new System.Drawing.Size(70, 25);
            this.btnLogin.TabIndex = 2;
            this.btnLogin.Text = "Login";
            this.btnLogin.UseVisualStyleBackColor = false;
            this.btnLogin.Click += new System.EventHandler(this.btnLogin_Click);
            // 
            // lbAccount
            // 
            this.lbAccount.AutoSize = true;
            this.lbAccount.BackColor = System.Drawing.Color.White;
            this.lbAccount.ForeColor = System.Drawing.Color.Green;
            this.lbAccount.Location = new System.Drawing.Point(298, 236);
            this.lbAccount.Name = "lbAccount";
            this.lbAccount.Size = new System.Drawing.Size(0, 15);
            this.lbAccount.TabIndex = 3;
            // 
            // lbPwd
            // 
            this.lbPwd.AutoSize = true;
            this.lbPwd.BackColor = System.Drawing.Color.White;
            this.lbPwd.ForeColor = System.Drawing.Color.Green;
            this.lbPwd.Location = new System.Drawing.Point(298, 279);
            this.lbPwd.Name = "lbPwd";
            this.lbPwd.Size = new System.Drawing.Size(0, 15);
            this.lbPwd.TabIndex = 3;
            // 
            // pnChangePwd
            // 
            this.pnChangePwd.BackColor = System.Drawing.Color.White;
            this.pnChangePwd.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnChangePwd.Controls.Add(this.pbConfirmNewPasswordTip);
            this.pnChangePwd.Controls.Add(this.pbNewPasswordTip);
            this.pnChangePwd.Controls.Add(this.btnOK);
            this.pnChangePwd.Controls.Add(this.tbConfirm);
            this.pnChangePwd.Controls.Add(this.tbNewPwd);
            this.pnChangePwd.Controls.Add(this.label4);
            this.pnChangePwd.Controls.Add(this.label3);
            this.pnChangePwd.Controls.Add(this.lbAlarmConfirm);
            this.pnChangePwd.Controls.Add(this.lbAlarmNewPwd);
            this.pnChangePwd.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.pnChangePwd.Font = new System.Drawing.Font("Arial", 9F);
            this.pnChangePwd.Location = new System.Drawing.Point(0, 220);
            this.pnChangePwd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pnChangePwd.Name = "pnChangePwd";
            this.pnChangePwd.Size = new System.Drawing.Size(550, 130);
            this.pnChangePwd.TabIndex = 4;
            this.pnChangePwd.Visible = false;
            // 
            // pbConfirmNewPasswordTip
            // 
            this.pbConfirmNewPasswordTip.BackColor = System.Drawing.Color.White;
            this.pbConfirmNewPasswordTip.Image = global::ShineTech.TempCentre.DeviceManage.Properties.Resources.wrong_cross;
            this.pbConfirmNewPasswordTip.Location = new System.Drawing.Point(298, 76);
            this.pbConfirmNewPasswordTip.Name = "pbConfirmNewPasswordTip";
            this.pbConfirmNewPasswordTip.Size = new System.Drawing.Size(12, 12);
            this.pbConfirmNewPasswordTip.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbConfirmNewPasswordTip.TabIndex = 15;
            this.pbConfirmNewPasswordTip.TabStop = false;
            this.pbConfirmNewPasswordTip.Visible = false;
            // 
            // pbNewPasswordTip
            // 
            this.pbNewPasswordTip.BackColor = System.Drawing.Color.White;
            this.pbNewPasswordTip.Image = global::ShineTech.TempCentre.DeviceManage.Properties.Resources.wrong_cross;
            this.pbNewPasswordTip.Location = new System.Drawing.Point(298, 38);
            this.pbNewPasswordTip.Name = "pbNewPasswordTip";
            this.pbNewPasswordTip.Size = new System.Drawing.Size(12, 12);
            this.pbNewPasswordTip.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbNewPasswordTip.TabIndex = 13;
            this.pbNewPasswordTip.TabStop = false;
            this.pbNewPasswordTip.Visible = false;
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.Transparent;
            this.btnOK.Font = new System.Drawing.Font("Arial", 9F);
            this.btnOK.ForeColor = System.Drawing.Color.Black;
            this.btnOK.Location = new System.Drawing.Point(373, 69);
            this.btnOK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(70, 25);
            this.btnOK.TabIndex = 3;
            this.btnOK.Text = "&OK";
            this.btnOK.UseVisualStyleBackColor = false;
            // 
            // tbConfirm
            // 
            this.tbConfirm.Font = new System.Drawing.Font("Arial", 9F);
            this.tbConfirm.Location = new System.Drawing.Point(134, 71);
            this.tbConfirm.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbConfirm.MaxLength = 15;
            this.tbConfirm.Name = "tbConfirm";
            this.tbConfirm.Size = new System.Drawing.Size(152, 21);
            this.tbConfirm.TabIndex = 2;
            this.tbConfirm.UseSystemPasswordChar = true;
            // 
            // tbNewPwd
            // 
            this.tbNewPwd.Font = new System.Drawing.Font("Arial", 9F);
            this.tbNewPwd.Location = new System.Drawing.Point(134, 33);
            this.tbNewPwd.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tbNewPwd.MaxLength = 15;
            this.tbNewPwd.Name = "tbNewPwd";
            this.tbNewPwd.Size = new System.Drawing.Size(152, 21);
            this.tbNewPwd.TabIndex = 1;
            this.tbNewPwd.UseSystemPasswordChar = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 9F);
            this.label4.Location = new System.Drawing.Point(5, 73);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(113, 15);
            this.label4.TabIndex = 9;
            this.label4.Text = "Confirm Password:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 9F);
            this.label3.Location = new System.Drawing.Point(24, 35);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 15);
            this.label3.TabIndex = 7;
            this.label3.Text = "New Password:";
            // 
            // lbAlarmConfirm
            // 
            this.lbAlarmConfirm.AutoSize = true;
            this.lbAlarmConfirm.ForeColor = System.Drawing.Color.Green;
            this.lbAlarmConfirm.Location = new System.Drawing.Point(292, 77);
            this.lbAlarmConfirm.Name = "lbAlarmConfirm";
            this.lbAlarmConfirm.Size = new System.Drawing.Size(0, 15);
            this.lbAlarmConfirm.TabIndex = 6;
            // 
            // lbAlarmNewPwd
            // 
            this.lbAlarmNewPwd.AutoSize = true;
            this.lbAlarmNewPwd.ForeColor = System.Drawing.Color.Green;
            this.lbAlarmNewPwd.Location = new System.Drawing.Point(292, 37);
            this.lbAlarmNewPwd.Name = "lbAlarmNewPwd";
            this.lbAlarmNewPwd.Size = new System.Drawing.Size(0, 15);
            this.lbAlarmNewPwd.TabIndex = 5;
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Arial", 9F);
            this.btnCancel.ForeColor = System.Drawing.Color.Black;
            this.btnCancel.Location = new System.Drawing.Point(461, 269);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(70, 25);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(229)))), ((int)(((byte)(229)))));
            this.BackgroundImage = global::ShineTech.TempCentre.DeviceManage.Properties.Resources.login_bg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(550, 350);
            this.ControlBox = false;
            this.Controls.Add(this.pnChangePwd);
            this.Controls.Add(this.lbPwd);
            this.Controls.Add(this.lbAccount);
            this.Controls.Add(this.tbPwd);
            this.Controls.Add(this.tbAccount);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnLogin);
            this.Font = new System.Drawing.Font("Arial", 9F);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            this.pnChangePwd.ResumeLayout(false);
            this.pnChangePwd.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbConfirmNewPasswordTip)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbNewPasswordTip)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbAccount;
        private System.Windows.Forms.TextBox tbPwd;
        private System.Windows.Forms.Button btnLogin;
        private System.Windows.Forms.Label lbAccount;
        private System.Windows.Forms.Label lbPwd;
        private System.Windows.Forms.Panel pnChangePwd;
        private System.Windows.Forms.TextBox tbConfirm;
        private System.Windows.Forms.TextBox tbNewPwd;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbAlarmConfirm;
        private System.Windows.Forms.Label lbAlarmNewPwd;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.PictureBox pbConfirmNewPasswordTip;
        private System.Windows.Forms.PictureBox pbNewPasswordTip;
    }
}