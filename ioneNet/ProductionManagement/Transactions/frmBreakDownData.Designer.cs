namespace ioneNet.ProductionManagement.Transactions
{
    partial class frmBreakDownData
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
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label12 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtReportNo = new System.Windows.Forms.TextBox();
            this.txtOperation = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dgBDData = new System.Windows.Forms.DataGridView();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.Time_From = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Time_To = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.BD_Nature = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.No_of_Hrs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgBDData)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ioneNet.Properties.Resources.logoface_Ione;
            this.pictureBox2.Location = new System.Drawing.Point(2, 5);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(52, 46);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 62;
            this.pictureBox2.TabStop = false;
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.DarkBlue;
            this.label12.Location = new System.Drawing.Point(62, 28);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(187, 25);
            this.label12.TabIndex = 61;
            this.label12.Text = "BREAKDOWN DATA";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(87, 63);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 20);
            this.label1.TabIndex = 63;
            this.label1.Text = "Report No";
            // 
            // txtReportNo
            // 
            this.txtReportNo.Location = new System.Drawing.Point(170, 60);
            this.txtReportNo.Name = "txtReportNo";
            this.txtReportNo.Size = new System.Drawing.Size(116, 27);
            this.txtReportNo.TabIndex = 64;
            // 
            // txtOperation
            // 
            this.txtOperation.Location = new System.Drawing.Point(396, 63);
            this.txtOperation.Name = "txtOperation";
            this.txtOperation.Size = new System.Drawing.Size(200, 27);
            this.txtOperation.TabIndex = 66;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(313, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 20);
            this.label2.TabIndex = 65;
            this.label2.Text = "Operation";
            // 
            // dgBDData
            // 
            this.dgBDData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgBDData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Time_From,
            this.Time_To,
            this.BD_Nature,
            this.No_of_Hrs});
            this.dgBDData.Location = new System.Drawing.Point(14, 103);
            this.dgBDData.Name = "dgBDData";
            this.dgBDData.RowHeadersWidth = 51;
            this.dgBDData.Size = new System.Drawing.Size(642, 259);
            this.dgBDData.TabIndex = 67;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(487, 368);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(83, 29);
            this.btnSubmit.TabIndex = 69;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(573, 368);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(83, 29);
            this.btnClose.TabIndex = 70;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // Time_From
            // 
            this.Time_From.DataPropertyName = "Time_From";
            this.Time_From.HeaderText = "Time From";
            this.Time_From.MinimumWidth = 6;
            this.Time_From.Name = "Time_From";
            this.Time_From.Width = 147;
            // 
            // Time_To
            // 
            this.Time_To.DataPropertyName = "Time_To";
            this.Time_To.HeaderText = "Time To";
            this.Time_To.MinimumWidth = 6;
            this.Time_To.Name = "Time_To";
            this.Time_To.Width = 148;
            // 
            // BD_Nature
            // 
            this.BD_Nature.DataPropertyName = "BD_Nature";
            this.BD_Nature.HeaderText = "Nature of Break Down";
            this.BD_Nature.MinimumWidth = 6;
            this.BD_Nature.Name = "BD_Nature";
            this.BD_Nature.Width = 147;
            // 
            // No_of_Hrs
            // 
            this.No_of_Hrs.DataPropertyName = "No_of_Hrs";
            this.No_of_Hrs.HeaderText = "No_Of_Hrs";
            this.No_of_Hrs.MinimumWidth = 6;
            this.No_of_Hrs.Name = "No_of_Hrs";
            this.No_of_Hrs.Width = 147;
            // 
            // frmBreakDownData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(668, 407);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.dgBDData);
            this.Controls.Add(this.txtOperation);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtReportNo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label12);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmBreakDownData";
            this.Text = "frmBreakDownData";
            this.Load += new System.EventHandler(this.frmBreakDownData_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgBDData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtReportNo;
        private System.Windows.Forms.TextBox txtOperation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgBDData;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time_From;
        private System.Windows.Forms.DataGridViewTextBoxColumn Time_To;
        private System.Windows.Forms.DataGridViewTextBoxColumn BD_Nature;
        private System.Windows.Forms.DataGridViewTextBoxColumn No_of_Hrs;
    }
}