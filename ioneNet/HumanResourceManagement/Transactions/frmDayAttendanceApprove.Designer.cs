namespace ioneNet.HumanResourceManagement.Transactions
{
    partial class frmDayAttendanceApprove
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label12 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.sfButton1 = new Syncfusion.WinForms.Controls.SfButton();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.btnExcell = new Syncfusion.WinForms.Controls.SfButton();
            this.btnClose = new Syncfusion.WinForms.Controls.SfButton();
            this.btnGenerate = new Syncfusion.WinForms.Controls.SfButton();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvAttnData = new System.Windows.Forms.DataGridView();
            this.BioMetric_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Emp_Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Emp_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.InTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.outtime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Hrs_Worked = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Attn_Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Miss_Punch_Reason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Final_Attn_Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttnData)).BeginInit();
            this.SuspendLayout();
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.DarkBlue;
            this.label12.Location = new System.Drawing.Point(83, 26);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(183, 25);
            this.label12.TabIndex = 188;
            this.label12.Text = "DAY ATTENDANCE ";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ioneNet.Properties.Resources.logoface_Ione;
            this.pictureBox2.Location = new System.Drawing.Point(4, -1);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 2, 4, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(71, 52);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 187;
            this.pictureBox2.TabStop = false;
            // 
            // groupBox3
            // 
            this.groupBox3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox3.Controls.Add(this.sfButton1);
            this.groupBox3.Controls.Add(this.dateTimePicker1);
            this.groupBox3.Controls.Add(this.btnExcell);
            this.groupBox3.Controls.Add(this.btnClose);
            this.groupBox3.Controls.Add(this.btnGenerate);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Font = new System.Drawing.Font("Calibri", 10F);
            this.groupBox3.ForeColor = System.Drawing.Color.RoyalBlue;
            this.groupBox3.Location = new System.Drawing.Point(4, 69);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox3.Size = new System.Drawing.Size(1360, 60);
            this.groupBox3.TabIndex = 193;
            this.groupBox3.TabStop = false;
            // 
            // sfButton1
            // 
            this.sfButton1.AccessibleName = "Button";
            this.sfButton1.BackColor = System.Drawing.Color.LightSkyBlue;
            this.sfButton1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.sfButton1.Location = new System.Drawing.Point(933, 17);
            this.sfButton1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.sfButton1.Name = "sfButton1";
            this.sfButton1.Size = new System.Drawing.Size(132, 34);
            this.sfButton1.Style.BackColor = System.Drawing.Color.LightSkyBlue;
            this.sfButton1.TabIndex = 148;
            this.sfButton1.Text = "Update Data";
            this.sfButton1.UseVisualStyleBackColor = false;
            this.sfButton1.Click += new System.EventHandler(this.sfButton1_Click);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.CustomFormat = "dd/MM/yyyy";
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(95, 17);
            this.dateTimePicker1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(120, 28);
            this.dateTimePicker1.TabIndex = 147;
            // 
            // btnExcell
            // 
            this.btnExcell.AccessibleName = "Button";
            this.btnExcell.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnExcell.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnExcell.Location = new System.Drawing.Point(1073, 17);
            this.btnExcell.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnExcell.Name = "btnExcell";
            this.btnExcell.Size = new System.Drawing.Size(133, 34);
            this.btnExcell.Style.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnExcell.TabIndex = 146;
            this.btnExcell.Text = "Export - Excel";
            this.btnExcell.UseVisualStyleBackColor = false;
            // 
            // btnClose
            // 
            this.btnClose.AccessibleName = "Button";
            this.btnClose.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnClose.Location = new System.Drawing.Point(1215, 17);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(113, 34);
            this.btnClose.Style.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnClose.TabIndex = 145;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnGenerate
            // 
            this.btnGenerate.AccessibleName = "Button";
            this.btnGenerate.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnGenerate.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnGenerate.Location = new System.Drawing.Point(821, 17);
            this.btnGenerate.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(104, 34);
            this.btnGenerate.Style.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnGenerate.TabIndex = 144;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = false;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(29, 18);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 18);
            this.label3.TabIndex = 141;
            this.label3.Text = "Date";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.dgvAttnData, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(4, 138);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1388, 633);
            this.tableLayoutPanel1.TabIndex = 194;
            // 
            // dgvAttnData
            // 
            this.dgvAttnData.AllowUserToOrderColumns = true;
            this.dgvAttnData.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvAttnData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAttnData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvAttnData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.LightSkyBlue;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAttnData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvAttnData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAttnData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.BioMetric_Id,
            this.Emp_Code,
            this.Emp_Name,
            this.InTime,
            this.outtime,
            this.Hrs_Worked,
            this.Attn_Status,
            this.Miss_Punch_Reason,
            this.Final_Attn_Status});
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAttnData.DefaultCellStyle = dataGridViewCellStyle4;
            this.dgvAttnData.EnableHeadersVisualStyles = false;
            this.dgvAttnData.Location = new System.Drawing.Point(4, 4);
            this.dgvAttnData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgvAttnData.Name = "dgvAttnData";
            this.dgvAttnData.RowHeadersWidth = 62;
            this.dgvAttnData.Size = new System.Drawing.Size(1380, 625);
            this.dgvAttnData.TabIndex = 1;
            this.dgvAttnData.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvAttnData_EditingControlShowing);
            // 
            // BioMetric_Id
            // 
            this.BioMetric_Id.DataPropertyName = "BioMetric_Id";
            this.BioMetric_Id.HeaderText = "Bio Metric ID";
            this.BioMetric_Id.MinimumWidth = 6;
            this.BioMetric_Id.Name = "BioMetric_Id";
            // 
            // Emp_Code
            // 
            this.Emp_Code.DataPropertyName = "Emp_Code";
            this.Emp_Code.HeaderText = "Emp Code";
            this.Emp_Code.MinimumWidth = 8;
            this.Emp_Code.Name = "Emp_Code";
            // 
            // Emp_Name
            // 
            this.Emp_Name.DataPropertyName = "Emp_Name";
            this.Emp_Name.HeaderText = "Employee Name";
            this.Emp_Name.MinimumWidth = 8;
            this.Emp_Name.Name = "Emp_Name";
            // 
            // InTime
            // 
            this.InTime.DataPropertyName = "InTime";
            this.InTime.HeaderText = "In Time";
            this.InTime.MinimumWidth = 8;
            this.InTime.Name = "InTime";
            this.InTime.ReadOnly = true;
            // 
            // outtime
            // 
            this.outtime.DataPropertyName = "outtime";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.outtime.DefaultCellStyle = dataGridViewCellStyle3;
            this.outtime.HeaderText = "Out Time";
            this.outtime.MinimumWidth = 8;
            this.outtime.Name = "outtime";
            this.outtime.ReadOnly = true;
            // 
            // Hrs_Worked
            // 
            this.Hrs_Worked.DataPropertyName = "Hrs_Worked";
            this.Hrs_Worked.HeaderText = "Hrs_Worked";
            this.Hrs_Worked.MinimumWidth = 8;
            this.Hrs_Worked.Name = "Hrs_Worked";
            this.Hrs_Worked.ReadOnly = true;
            // 
            // Attn_Status
            // 
            this.Attn_Status.DataPropertyName = "Attn_Status";
            this.Attn_Status.HeaderText = "Attn_Status";
            this.Attn_Status.MinimumWidth = 8;
            this.Attn_Status.Name = "Attn_Status";
            this.Attn_Status.ReadOnly = true;
            // 
            // Miss_Punch_Reason
            // 
            this.Miss_Punch_Reason.DataPropertyName = "Miss_Punch_Reason";
            this.Miss_Punch_Reason.HeaderText = "Reason for Miss Punch";
            this.Miss_Punch_Reason.MinimumWidth = 8;
            this.Miss_Punch_Reason.Name = "Miss_Punch_Reason";
            // 
            // Final_Attn_Status
            // 
            this.Final_Attn_Status.DataPropertyName = "Final_Attn_Status";
            this.Final_Attn_Status.HeaderText = "Final Attn Status";
            this.Final_Attn_Status.MinimumWidth = 8;
            this.Final_Attn_Status.Name = "Final_Attn_Status";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(457, 31);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 195;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmDayAttendanceApprove
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1443, 849);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.pictureBox2);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmDayAttendanceApprove";
            this.Text = "frmDayAttendanceApprove";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvAttnData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private Syncfusion.WinForms.Controls.SfButton btnExcell;
        private Syncfusion.WinForms.Controls.SfButton btnClose;
        private Syncfusion.WinForms.Controls.SfButton btnGenerate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dgvAttnData;
        private System.Windows.Forms.DataGridViewTextBoxColumn BioMetric_Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Emp_Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Emp_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn InTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn outtime;
        private System.Windows.Forms.DataGridViewTextBoxColumn Hrs_Worked;
        private System.Windows.Forms.DataGridViewTextBoxColumn Attn_Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Miss_Punch_Reason;
        private System.Windows.Forms.DataGridViewTextBoxColumn Final_Attn_Status;
        private Syncfusion.WinForms.Controls.SfButton sfButton1;
        private System.Windows.Forms.Button button1;
    }
}