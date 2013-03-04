namespace ShineTech.TempCentre.DeviceManage
{
    partial class Administration
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tp1 = new System.Windows.Forms.TabPage();
            this.btnDisUser = new System.Windows.Forms.Button();
            this.btnEditUser = new System.Windows.Forms.Button();
            this.btnAddUser = new System.Windows.Forms.Button();
            this.dgvUser = new System.Windows.Forms.DataGridView();
            this.tp2 = new System.Windows.Forms.TabPage();
            this.tp3 = new System.Windows.Forms.TabPage();
            this.tp4 = new System.Windows.Forms.TabPage();
            this.tp5 = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tp1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUser)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tp1);
            this.tabControl1.Controls.Add(this.tp2);
            this.tabControl1.Controls.Add(this.tp3);
            this.tabControl1.Controls.Add(this.tp4);
            this.tabControl1.Controls.Add(this.tp5);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Font = new System.Drawing.Font("Microsoft YaHei", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(594, 472);
            this.tabControl1.TabIndex = 0;
            // 
            // tp1
            // 
            this.tp1.Controls.Add(this.btnDisUser);
            this.tp1.Controls.Add(this.btnEditUser);
            this.tp1.Controls.Add(this.btnAddUser);
            this.tp1.Controls.Add(this.dgvUser);
            this.tp1.Location = new System.Drawing.Point(4, 29);
            this.tp1.Name = "tp1";
            this.tp1.Padding = new System.Windows.Forms.Padding(3);
            this.tp1.Size = new System.Drawing.Size(586, 439);
            this.tp1.TabIndex = 0;
            this.tp1.Text = "User Accounts";
            this.tp1.UseVisualStyleBackColor = true;
            // 
            // btnDisUser
            // 
            this.btnDisUser.BackColor = System.Drawing.Color.White;
            this.btnDisUser.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDisUser.Location = new System.Drawing.Point(220, 390);
            this.btnDisUser.Name = "btnDisUser";
            this.btnDisUser.Size = new System.Drawing.Size(100, 25);
            this.btnDisUser.TabIndex = 2;
            this.btnDisUser.Text = "&Disable User";
            this.btnDisUser.UseVisualStyleBackColor = false;
            // 
            // btnEditUser
            // 
            this.btnEditUser.BackColor = System.Drawing.Color.White;
            this.btnEditUser.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnEditUser.Location = new System.Drawing.Point(114, 390);
            this.btnEditUser.Name = "btnEditUser";
            this.btnEditUser.Size = new System.Drawing.Size(100, 25);
            this.btnEditUser.TabIndex = 2;
            this.btnEditUser.Text = "&Edit User";
            this.btnEditUser.UseVisualStyleBackColor = false;
            this.btnEditUser.Click += new System.EventHandler(this.btnEditUser_Click);
            // 
            // btnAddUser
            // 
            this.btnAddUser.BackColor = System.Drawing.Color.White;
            this.btnAddUser.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAddUser.Location = new System.Drawing.Point(8, 390);
            this.btnAddUser.Name = "btnAddUser";
            this.btnAddUser.Size = new System.Drawing.Size(100, 25);
            this.btnAddUser.TabIndex = 2;
            this.btnAddUser.Text = "&Add User";
            this.btnAddUser.UseVisualStyleBackColor = false;
            // 
            // dgvUser
            // 
            this.dgvUser.AllowUserToAddRows = false;
            this.dgvUser.AllowUserToDeleteRows = false;
            this.dgvUser.BackgroundColor = System.Drawing.Color.White;
            this.dgvUser.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.dgvUser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvUser.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvUser.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvUser.GridColor = System.Drawing.Color.White;
            this.dgvUser.Location = new System.Drawing.Point(3, 3);
            this.dgvUser.MultiSelect = false;
            this.dgvUser.Name = "dgvUser";
            this.dgvUser.ReadOnly = true;
            this.dgvUser.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUser.Size = new System.Drawing.Size(580, 365);
            this.dgvUser.TabIndex = 1;
            // 
            // tp2
            // 
            this.tp2.Location = new System.Drawing.Point(4, 29);
            this.tp2.Name = "tp2";
            this.tp2.Padding = new System.Windows.Forms.Padding(3);
            this.tp2.Size = new System.Drawing.Size(586, 439);
            this.tp2.TabIndex = 1;
            this.tp2.Text = "User Policies";
            this.tp2.UseVisualStyleBackColor = true;
            // 
            // tp3
            // 
            this.tp3.Location = new System.Drawing.Point(4, 29);
            this.tp3.Name = "tp3";
            this.tp3.Padding = new System.Windows.Forms.Padding(3);
            this.tp3.Size = new System.Drawing.Size(586, 439);
            this.tp3.TabIndex = 2;
            this.tp3.Text = "User Rights";
            this.tp3.UseVisualStyleBackColor = true;
            // 
            // tp4
            // 
            this.tp4.Location = new System.Drawing.Point(4, 29);
            this.tp4.Name = "tp4";
            this.tp4.Padding = new System.Windows.Forms.Padding(3);
            this.tp4.Size = new System.Drawing.Size(586, 439);
            this.tp4.TabIndex = 3;
            this.tp4.Text = "User Profile";
            this.tp4.UseVisualStyleBackColor = true;
            // 
            // tp5
            // 
            this.tp5.Location = new System.Drawing.Point(4, 29);
            this.tp5.Name = "tp5";
            this.tp5.Padding = new System.Windows.Forms.Padding(3);
            this.tp5.Size = new System.Drawing.Size(586, 439);
            this.tp5.TabIndex = 4;
            this.tp5.Text = "Meanings of Signatures";
            this.tp5.UseVisualStyleBackColor = true;
            // 
            // Administration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(594, 472);
            this.Controls.Add(this.tabControl1);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.MaximizeBox = false;
            this.Name = "Administration";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Administration";
            this.tabControl1.ResumeLayout(false);
            this.tp1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUser)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tp1;
        private System.Windows.Forms.TabPage tp2;
        private System.Windows.Forms.TabPage tp3;
        private System.Windows.Forms.TabPage tp4;
        private System.Windows.Forms.TabPage tp5;
        private System.Windows.Forms.DataGridView dgvUser;
        private System.Windows.Forms.Button btnDisUser;
        private System.Windows.Forms.Button btnEditUser;
        private System.Windows.Forms.Button btnAddUser;
    }
}