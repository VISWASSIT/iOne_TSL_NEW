
namespace ioneNet.MaterialManagement.Transactions
{
    partial class frmTransLog
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.dgProductsList = new System.Windows.Forms.DataGridView();
            this.txtRefDocNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTransName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.E_Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.e_User = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Trans_Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Trans_Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.dtEDate = new System.Windows.Forms.DateTimePicker();
            this.txtTransRemarks = new System.Windows.Forms.TextBox();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtusername = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgProductsList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btnSubmit.Location = new System.Drawing.Point(539, 346);
            this.btnSubmit.Margin = new System.Windows.Forms.Padding(4);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(100, 28);
            this.btnSubmit.TabIndex = 217;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btnClose.Location = new System.Drawing.Point(647, 349);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 28);
            this.btnClose.TabIndex = 216;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // dgProductsList
            // 
            this.dgProductsList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.PaleTurquoise;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgProductsList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgProductsList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgProductsList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.E_Date,
            this.e_User,
            this.Trans_Remarks,
            this.Trans_Status});
            this.dgProductsList.EnableHeadersVisualStyles = false;
            this.dgProductsList.Location = new System.Drawing.Point(16, 149);
            this.dgProductsList.Margin = new System.Windows.Forms.Padding(4);
            this.dgProductsList.Name = "dgProductsList";
            this.dgProductsList.RowHeadersWidth = 51;
            this.dgProductsList.Size = new System.Drawing.Size(731, 190);
            this.dgProductsList.TabIndex = 215;
            // 
            // txtRefDocNo
            // 
            this.txtRefDocNo.Enabled = false;
            this.txtRefDocNo.Location = new System.Drawing.Point(445, 66);
            this.txtRefDocNo.Margin = new System.Windows.Forms.Padding(4);
            this.txtRefDocNo.Name = "txtRefDocNo";
            this.txtRefDocNo.Size = new System.Drawing.Size(124, 22);
            this.txtRefDocNo.TabIndex = 214;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(356, 68);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 17);
            this.label2.TabIndex = 213;
            this.label2.Text = "Ref No";
            // 
            // txtTransName
            // 
            this.txtTransName.Enabled = false;
            this.txtTransName.Location = new System.Drawing.Point(123, 65);
            this.txtTransName.Margin = new System.Windows.Forms.Padding(4);
            this.txtTransName.Name = "txtTransName";
            this.txtTransName.Size = new System.Drawing.Size(204, 22);
            this.txtTransName.TabIndex = 211;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 68);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 17);
            this.label1.TabIndex = 210;
            this.label1.Text = "Transaction";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.DarkBlue;
            this.label12.Location = new System.Drawing.Point(72, 34);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(164, 18);
            this.label12.TabIndex = 209;
            this.label12.Text = "TRANSACTION LOG";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // E_Date
            // 
            this.E_Date.DataPropertyName = "e_Date";
            this.E_Date.HeaderText = "Date";
            this.E_Date.MinimumWidth = 6;
            this.E_Date.Name = "E_Date";
            // 
            // e_User
            // 
            this.e_User.DataPropertyName = "e_User";
            this.e_User.HeaderText = "User";
            this.e_User.MinimumWidth = 6;
            this.e_User.Name = "e_User";
            // 
            // Trans_Remarks
            // 
            this.Trans_Remarks.DataPropertyName = "Trans_Remarks";
            this.Trans_Remarks.HeaderText = "Transaction Remarks";
            this.Trans_Remarks.MinimumWidth = 6;
            this.Trans_Remarks.Name = "Trans_Remarks";
            this.Trans_Remarks.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Trans_Remarks.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Trans_Status
            // 
            this.Trans_Status.DataPropertyName = "Trans_Status";
            this.Trans_Status.HeaderText = "Status";
            this.Trans_Status.MinimumWidth = 6;
            this.Trans_Status.Name = "Trans_Status";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ioneNet.Properties.Resources.logoface_Ione;
            this.pictureBox2.Location = new System.Drawing.Point(4, 3);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(60, 49);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 212;
            this.pictureBox2.TabStop = false;
            // 
            // dtEDate
            // 
            this.dtEDate.CustomFormat = "dd/MM/yyyy HH:mm";
            this.dtEDate.Enabled = false;
            this.dtEDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtEDate.Location = new System.Drawing.Point(19, 120);
            this.dtEDate.Name = "dtEDate";
            this.dtEDate.Size = new System.Drawing.Size(147, 22);
            this.dtEDate.TabIndex = 218;
            // 
            // txtTransRemarks
            // 
            this.txtTransRemarks.Location = new System.Drawing.Point(289, 121);
            this.txtTransRemarks.Name = "txtTransRemarks";
            this.txtTransRemarks.Size = new System.Drawing.Size(223, 22);
            this.txtTransRemarks.TabIndex = 220;
            // 
            // cmbStatus
            // 
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(518, 120);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(163, 24);
            this.cmbStatus.TabIndex = 221;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 102);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 17);
            this.label3.TabIndex = 222;
            this.label3.Text = "Date";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(286, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 17);
            this.label4.TabIndex = 223;
            this.label4.Text = "Remarks";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(515, 100);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(48, 17);
            this.label5.TabIndex = 224;
            this.label5.Text = "Status";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(169, 101);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(38, 17);
            this.label6.TabIndex = 226;
            this.label6.Text = "User";
            // 
            // txtusername
            // 
            this.txtusername.Enabled = false;
            this.txtusername.Location = new System.Drawing.Point(172, 121);
            this.txtusername.Name = "txtusername";
            this.txtusername.Size = new System.Drawing.Size(111, 22);
            this.txtusername.TabIndex = 225;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(687, 120);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(60, 24);
            this.button1.TabIndex = 227;
            this.button1.Text = "Add";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmTransLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 386);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtusername);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.txtTransRemarks);
            this.Controls.Add(this.dtEDate);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgProductsList);
            this.Controls.Add(this.txtRefDocNo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.txtTransName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label12);
            this.Name = "frmTransLog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmTransLog";
            this.Load += new System.EventHandler(this.frmTransLog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgProductsList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgProductsList;
        private System.Windows.Forms.TextBox txtRefDocNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TextBox txtTransName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DataGridViewTextBoxColumn E_Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn e_User;
        private System.Windows.Forms.DataGridViewTextBoxColumn Trans_Remarks;
        private System.Windows.Forms.DataGridViewTextBoxColumn Trans_Status;
        private System.Windows.Forms.DateTimePicker dtEDate;
        private System.Windows.Forms.TextBox txtTransRemarks;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtusername;
        private System.Windows.Forms.Button button1;
    }
}