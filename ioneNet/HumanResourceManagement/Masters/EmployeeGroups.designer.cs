namespace ioneNet.HumanResourceManagement.Masters
{
    partial class EmployeeGroups
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EmployeeGroups));
            this.txtGroupName = new System.Windows.Forms.TextBox();
            this.txtGroupID = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.sfDataGrid1 = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            this.chkPF = new System.Windows.Forms.CheckBox();
            this.chkESI = new System.Windows.Forms.CheckBox();
            this.multiSelectionComboBox2 = new Syncfusion.Windows.Forms.Tools.MultiSelectionComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.multiSelectionComboBox1 = new Syncfusion.Windows.Forms.Tools.MultiSelectionComboBox();
            this.label24 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtPrefix = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtGroupShortName = new System.Windows.Forms.TextBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.linkModifiedBy = new System.Windows.Forms.LinkLabel();
            this.linkCreatedBy = new System.Windows.Forms.LinkLabel();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label12 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sfDataGrid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.multiSelectionComboBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.multiSelectionComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // txtGroupName
            // 
            this.txtGroupName.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtGroupName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGroupName.Location = new System.Drawing.Point(455, 47);
            this.txtGroupName.MaxLength = 50;
            this.txtGroupName.Name = "txtGroupName";
            this.txtGroupName.Size = new System.Drawing.Size(151, 21);
            this.txtGroupName.TabIndex = 3;
            this.txtGroupName.Leave += new System.EventHandler(this.txtGroupname_Leave);
            // 
            // txtGroupID
            // 
            this.txtGroupID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGroupID.Location = new System.Drawing.Point(455, 20);
            this.txtGroupID.Name = "txtGroupID";
            this.txtGroupID.ReadOnly = true;
            this.txtGroupID.Size = new System.Drawing.Size(94, 21);
            this.txtGroupID.TabIndex = 1;
            this.txtGroupID.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(325, 48);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(78, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Group Name";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(325, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 15);
            this.label2.TabIndex = 0;
            this.label2.Text = "Group ID";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.sfDataGrid1);
            this.panel1.Controls.Add(this.chkPF);
            this.panel1.Controls.Add(this.chkESI);
            this.panel1.Controls.Add(this.multiSelectionComboBox2);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.multiSelectionComboBox1);
            this.panel1.Controls.Add(this.label24);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.txtPrefix);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.txtGroupShortName);
            this.panel1.Controls.Add(this.btnDelete);
            this.panel1.Controls.Add(this.btnClose);
            this.panel1.Controls.Add(this.btnSubmit);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.txtGroupID);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.txtGroupName);
            this.panel1.Location = new System.Drawing.Point(10, 60);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(692, 338);
            this.panel1.TabIndex = 0;
            this.panel1.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // sfDataGrid1
            // 
            this.sfDataGrid1.AccessibleName = "Table";
            this.sfDataGrid1.AllowGrouping = false;
            this.sfDataGrid1.AllowResizingColumns = true;
            this.sfDataGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sfDataGrid1.AutoSizeColumnsMode = Syncfusion.WinForms.DataGrid.Enums.AutoSizeColumnsMode.Fill;
            this.sfDataGrid1.Location = new System.Drawing.Point(15, 20);
            this.sfDataGrid1.Name = "sfDataGrid1";
            this.sfDataGrid1.Size = new System.Drawing.Size(284, 290);
            this.sfDataGrid1.TabIndex = 17;
            this.sfDataGrid1.Text = "sfDataGrid1";
            this.sfDataGrid1.CellDoubleClick += new Syncfusion.WinForms.DataGrid.Events.CellClickEventHandler(this.sfDataGrid1_CellDoubleClick);
            this.sfDataGrid1.Click += new System.EventHandler(this.sfDataGrid1_Click);
            // 
            // chkPF
            // 
            this.chkPF.AutoSize = true;
            this.chkPF.Location = new System.Drawing.Point(461, 217);
            this.chkPF.Name = "chkPF";
            this.chkPF.Size = new System.Drawing.Size(103, 19);
            this.chkPF.TabIndex = 13;
            this.chkPF.Text = "PF Applicable?";
            this.chkPF.UseVisualStyleBackColor = true;
            // 
            // chkESI
            // 
            this.chkESI.AutoSize = true;
            this.chkESI.Location = new System.Drawing.Point(461, 196);
            this.chkESI.Name = "chkESI";
            this.chkESI.Size = new System.Drawing.Size(107, 19);
            this.chkESI.TabIndex = 12;
            this.chkESI.Text = "ESI Applicable?";
            this.chkESI.UseVisualStyleBackColor = true;
            // 
            // multiSelectionComboBox2
            // 
            this.multiSelectionComboBox2.BeforeTouchSize = new System.Drawing.Size(170, 26);
            this.multiSelectionComboBox2.ButtonStyle = Syncfusion.Windows.Forms.ButtonAppearance.Metro;
            this.multiSelectionComboBox2.DataSource = ((object)(resources.GetObject("multiSelectionComboBox2.DataSource")));
            this.multiSelectionComboBox2.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.multiSelectionComboBox2.Location = new System.Drawing.Point(455, 160);
            this.multiSelectionComboBox2.Name = "multiSelectionComboBox2";
            this.multiSelectionComboBox2.Size = new System.Drawing.Size(170, 26);
            this.multiSelectionComboBox2.TabIndex = 11;
            this.multiSelectionComboBox2.ThemeName = "Metro";
            this.multiSelectionComboBox2.UseVisualStyle = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(325, 160);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(129, 15);
            this.label6.TabIndex = 10;
            this.label6.Text = "Deductions Applicable";
            // 
            // multiSelectionComboBox1
            // 
            this.multiSelectionComboBox1.BeforeTouchSize = new System.Drawing.Size(170, 26);
            this.multiSelectionComboBox1.ButtonStyle = Syncfusion.Windows.Forms.ButtonAppearance.Metro;
            this.multiSelectionComboBox1.DataSource = ((object)(resources.GetObject("multiSelectionComboBox1.DataSource")));
            this.multiSelectionComboBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.multiSelectionComboBox1.Location = new System.Drawing.Point(455, 128);
            this.multiSelectionComboBox1.Name = "multiSelectionComboBox1";
            this.multiSelectionComboBox1.Size = new System.Drawing.Size(170, 26);
            this.multiSelectionComboBox1.TabIndex = 9;
            this.multiSelectionComboBox1.ThemeName = "Metro";
            this.multiSelectionComboBox1.UseVisualStyle = true;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label24.ForeColor = System.Drawing.Color.Black;
            this.label24.Location = new System.Drawing.Point(325, 128);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(111, 15);
            this.label24.TabIndex = 8;
            this.label24.Text = "Head(s) Applicable";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(325, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 15);
            this.label4.TabIndex = 6;
            this.label4.Text = "Prefix";
            // 
            // txtPrefix
            // 
            this.txtPrefix.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPrefix.Location = new System.Drawing.Point(455, 101);
            this.txtPrefix.Name = "txtPrefix";
            this.txtPrefix.Size = new System.Drawing.Size(94, 21);
            this.txtPrefix.TabIndex = 7;
            this.txtPrefix.TabStop = false;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(325, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 15);
            this.label3.TabIndex = 4;
            this.label3.Text = "Short Name";
            // 
            // txtGroupShortName
            // 
            this.txtGroupShortName.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGroupShortName.Location = new System.Drawing.Point(455, 74);
            this.txtGroupShortName.Name = "txtGroupShortName";
            this.txtGroupShortName.Size = new System.Drawing.Size(94, 21);
            this.txtGroupShortName.TabIndex = 5;
            this.txtGroupShortName.TabStop = false;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Location = new System.Drawing.Point(497, 296);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(87, 29);
            this.btnDelete.TabIndex = 15;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Location = new System.Drawing.Point(591, 296);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 29);
            this.btnClose.TabIndex = 16;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSubmit.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmit.ForeColor = System.Drawing.Color.Black;
            this.btnSubmit.Location = new System.Drawing.Point(404, 296);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(87, 29);
            this.btnSubmit.TabIndex = 14;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // linkModifiedBy
            // 
            this.linkModifiedBy.AutoSize = true;
            this.linkModifiedBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkModifiedBy.ForeColor = System.Drawing.Color.Gray;
            this.linkModifiedBy.Location = new System.Drawing.Point(413, 401);
            this.linkModifiedBy.Name = "linkModifiedBy";
            this.linkModifiedBy.Size = new System.Drawing.Size(60, 13);
            this.linkModifiedBy.TabIndex = 176;
            this.linkModifiedBy.TabStop = true;
            this.linkModifiedBy.Text = "User Name";
            // 
            // linkCreatedBy
            // 
            this.linkCreatedBy.AutoSize = true;
            this.linkCreatedBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkCreatedBy.ForeColor = System.Drawing.Color.Gray;
            this.linkCreatedBy.Location = new System.Drawing.Point(83, 401);
            this.linkCreatedBy.Name = "linkCreatedBy";
            this.linkCreatedBy.Size = new System.Drawing.Size(60, 13);
            this.linkCreatedBy.TabIndex = 175;
            this.linkCreatedBy.TabStop = true;
            this.linkCreatedBy.Text = "User Name";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Gray;
            this.label8.Location = new System.Drawing.Point(335, 401);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(62, 13);
            this.label8.TabIndex = 174;
            this.label8.Text = "Modified By";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Gray;
            this.label10.Location = new System.Drawing.Point(11, 401);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(59, 13);
            this.label10.TabIndex = 173;
            this.label10.Text = "Created By";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ioneNet.Properties.Resources.logoface_Ione;
            this.pictureBox2.Location = new System.Drawing.Point(-1, 1);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(52, 46);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 179;
            this.pictureBox2.TabStop = false;
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.DarkBlue;
            this.label12.Location = new System.Drawing.Point(57, 32);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(157, 16);
            this.label12.TabIndex = 180;
            this.label12.Text = "EMPLOYEE GROUPS";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // EmployeeGroups
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(714, 432);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.linkModifiedBy);
            this.Controls.Add(this.linkCreatedBy);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.panel1);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.KeyPreview = true;
            this.Name = "EmployeeGroups";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EmployeeGroups";
            this.Load += new System.EventHandler(this.frmCityMaster_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCityMaster_KeyDown);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sfDataGrid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.multiSelectionComboBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.multiSelectionComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox txtGroupName;
        private System.Windows.Forms.TextBox txtGroupID;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel panel1;
        internal System.Windows.Forms.Button btnDelete;
        internal System.Windows.Forms.Button btnClose;
        internal System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.LinkLabel linkModifiedBy;
        private System.Windows.Forms.LinkLabel linkCreatedBy;
        internal System.Windows.Forms.Label label8;
        internal System.Windows.Forms.Label label10;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtGroupShortName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtPrefix;
        private System.Windows.Forms.CheckBox chkPF;
        private System.Windows.Forms.CheckBox chkESI;
        private Syncfusion.Windows.Forms.Tools.MultiSelectionComboBox multiSelectionComboBox2;
        private System.Windows.Forms.Label label6;
        private Syncfusion.Windows.Forms.Tools.MultiSelectionComboBox multiSelectionComboBox1;
        private System.Windows.Forms.Label label24;
        private Syncfusion.WinForms.DataGrid.SfDataGrid sfDataGrid1;
    }
}