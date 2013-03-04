namespace ShineTech.TempCentre.DeviceManage
{
    partial class Administrator
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvUser = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.clbMeaning = new System.Windows.Forms.CheckedListBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnPolicy = new System.Windows.Forms.Button();
            this.btnAddMean = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnEditMean = new System.Windows.Forms.Button();
            this.btnDelMean = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUser)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dgvUser);
            this.groupBox1.Location = new System.Drawing.Point(0, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(470, 196);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Users";
            // 
            // dgvUser
            // 
            this.dgvUser.AllowUserToAddRows = false;
            this.dgvUser.AllowUserToDeleteRows = false;
            this.dgvUser.BackgroundColor = System.Drawing.Color.White;
            this.dgvUser.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUser.GridColor = System.Drawing.Color.White;
            this.dgvUser.Location = new System.Drawing.Point(3, 19);
            this.dgvUser.Name = "dgvUser";
            this.dgvUser.ReadOnly = true;
            this.dgvUser.RowTemplate.Height = 23;
            this.dgvUser.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvUser.Size = new System.Drawing.Size(464, 174);
            this.dgvUser.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.clbMeaning);
            this.groupBox2.Location = new System.Drawing.Point(0, 205);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(470, 158);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Meanings";
            // 
            // clbMeaning
            // 
            this.clbMeaning.CheckOnClick = true;
            this.clbMeaning.Dock = System.Windows.Forms.DockStyle.Fill;
            this.clbMeaning.FormattingEnabled = true;
            this.clbMeaning.Location = new System.Drawing.Point(3, 19);
            this.clbMeaning.Name = "clbMeaning";
            this.clbMeaning.Size = new System.Drawing.Size(464, 136);
            this.clbMeaning.TabIndex = 0;
            // 
            // btnAdd
            // 
            this.btnAdd.BackColor = System.Drawing.Color.White;
            this.btnAdd.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAdd.ForeColor = System.Drawing.Color.Black;
            this.btnAdd.Location = new System.Drawing.Point(476, 18);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(100, 27);
            this.btnAdd.TabIndex = 1;
            this.btnAdd.Text = "&Add User...";
            this.btnAdd.UseVisualStyleBackColor = false;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnEdit
            // 
            this.btnEdit.BackColor = System.Drawing.Color.White;
            this.btnEdit.Enabled = false;
            this.btnEdit.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnEdit.ForeColor = System.Drawing.Color.Black;
            this.btnEdit.Location = new System.Drawing.Point(476, 51);
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(100, 27);
            this.btnEdit.TabIndex = 1;
            this.btnEdit.Text = "&Edit User...";
            this.btnEdit.UseVisualStyleBackColor = false;
            this.btnEdit.Click += new System.EventHandler(this.btnEdit_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.White;
            this.btnDelete.Enabled = false;
            this.btnDelete.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Location = new System.Drawing.Point(476, 84);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(100, 27);
            this.btnDelete.TabIndex = 1;
            this.btnDelete.Text = "&Delete User";
            this.btnDelete.UseVisualStyleBackColor = false;
            // 
            // btnPolicy
            // 
            this.btnPolicy.BackColor = System.Drawing.Color.White;
            this.btnPolicy.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnPolicy.ForeColor = System.Drawing.Color.Black;
            this.btnPolicy.Location = new System.Drawing.Point(476, 117);
            this.btnPolicy.Name = "btnPolicy";
            this.btnPolicy.Size = new System.Drawing.Size(100, 27);
            this.btnPolicy.TabIndex = 1;
            this.btnPolicy.Text = "&Policy...";
            this.btnPolicy.UseVisualStyleBackColor = false;
            this.btnPolicy.Click += new System.EventHandler(this.btnPolicy_Click);
            // 
            // btnAddMean
            // 
            this.btnAddMean.BackColor = System.Drawing.Color.White;
            this.btnAddMean.ForeColor = System.Drawing.Color.Black;
            this.btnAddMean.Location = new System.Drawing.Point(476, 213);
            this.btnAddMean.Name = "btnAddMean";
            this.btnAddMean.Size = new System.Drawing.Size(100, 27);
            this.btnAddMean.TabIndex = 1;
            this.btnAddMean.Text = "Add &meaning";
            this.btnAddMean.UseVisualStyleBackColor = false;
            this.btnAddMean.Click += new System.EventHandler(this.btnAddMean_Click);
            // 
            // btnExit
            // 
            this.btnExit.BackColor = System.Drawing.Color.White;
            this.btnExit.ForeColor = System.Drawing.Color.Black;
            this.btnExit.Location = new System.Drawing.Point(476, 322);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(100, 27);
            this.btnExit.TabIndex = 1;
            this.btnExit.Text = "E&xit";
            this.btnExit.UseVisualStyleBackColor = false;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnEditMean
            // 
            this.btnEditMean.BackColor = System.Drawing.Color.White;
            this.btnEditMean.ForeColor = System.Drawing.Color.Black;
            this.btnEditMean.Location = new System.Drawing.Point(476, 246);
            this.btnEditMean.Name = "btnEditMean";
            this.btnEditMean.Size = new System.Drawing.Size(100, 27);
            this.btnEditMean.TabIndex = 1;
            this.btnEditMean.Text = "Edit mea&ning";
            this.btnEditMean.UseVisualStyleBackColor = false;
            this.btnEditMean.Click += new System.EventHandler(this.btnEditMean_Click);
            // 
            // btnDelMean
            // 
            this.btnDelMean.BackColor = System.Drawing.Color.White;
            this.btnDelMean.ForeColor = System.Drawing.Color.Black;
            this.btnDelMean.Location = new System.Drawing.Point(476, 279);
            this.btnDelMean.Name = "btnDelMean";
            this.btnDelMean.Size = new System.Drawing.Size(100, 27);
            this.btnDelMean.TabIndex = 1;
            this.btnDelMean.Text = "Del mean&ing";
            this.btnDelMean.UseVisualStyleBackColor = false;
            // 
            // Administrator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(594, 374);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnDelMean);
            this.Controls.Add(this.btnEditMean);
            this.Controls.Add(this.btnAddMean);
            this.Controls.Add(this.btnPolicy);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnEdit);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "Administrator";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Administrator";
            this.Load += new System.EventHandler(this.Administrator_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvUser)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dgvUser;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnPolicy;
        private System.Windows.Forms.Button btnAddMean;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnEditMean;
        private System.Windows.Forms.Button btnDelMean;
        private System.Windows.Forms.CheckedListBox clbMeaning;
    }
}