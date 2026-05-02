namespace ioneNet.FinanceManagement.Transactions
{
    partial class frmImport_GSTR2
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label3 = new System.Windows.Forms.Label();
            this.txtExcellSheet = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.txtChooseFile = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.dgProductData = new System.Windows.Forms.DataGridView();
            this.GSTIN = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AccName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Inv_No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Inv_type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Inv_Date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Inv_Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RCM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Tax_Rate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Taxable_Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.IGST = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CGST = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SGST = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cess = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Return_Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Our_Entry_Ref_No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgProductData)).BeginInit();
            this.SuspendLayout();
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DarkBlue;
            this.label3.Location = new System.Drawing.Point(54, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(224, 28);
            this.label3.TabIndex = 86;
            this.label3.Text = "GSTR 2 COMPARISION";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtExcellSheet
            // 
            this.txtExcellSheet.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtExcellSheet.Location = new System.Drawing.Point(665, 64);
            this.txtExcellSheet.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtExcellSheet.Name = "txtExcellSheet";
            this.txtExcellSheet.Size = new System.Drawing.Size(275, 28);
            this.txtExcellSheet.TabIndex = 85;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(554, 64);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(162, 22);
            this.label2.TabIndex = 84;
            this.label2.Text = "Excell Sheet Name";
            // 
            // btnImport
            // 
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImport.ForeColor = System.Drawing.Color.Black;
            this.btnImport.Location = new System.Drawing.Point(946, 62);
            this.btnImport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(97, 32);
            this.btnImport.TabIndex = 83;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnBrowse
            // 
            this.btnBrowse.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnBrowse.ForeColor = System.Drawing.Color.Black;
            this.btnBrowse.Location = new System.Drawing.Point(470, 66);
            this.btnBrowse.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(78, 28);
            this.btnBrowse.TabIndex = 82;
            this.btnBrowse.Text = "Browse";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // txtChooseFile
            // 
            this.txtChooseFile.Enabled = false;
            this.txtChooseFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChooseFile.Location = new System.Drawing.Point(133, 66);
            this.txtChooseFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.txtChooseFile.Name = "txtChooseFile";
            this.txtChooseFile.Size = new System.Drawing.Size(331, 28);
            this.txtChooseFile.TabIndex = 81;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(60, 66);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(106, 22);
            this.label12.TabIndex = 80;
            this.label12.Text = "Choose File";
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ioneNet.Properties.Resources.logoface_Ione;
            this.pictureBox2.Location = new System.Drawing.Point(3, 1);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(45, 40);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 87;
            this.pictureBox2.TabStop = false;
            // 
            // dgProductData
            // 
            this.dgProductData.AllowUserToResizeColumns = false;
            this.dgProductData.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Info;
            this.dgProductData.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgProductData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.LightSkyBlue;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgProductData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgProductData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgProductData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GSTIN,
            this.AccName,
            this.Inv_No,
            this.Inv_type,
            this.Inv_Date,
            this.Inv_Value,
            this.RCM,
            this.Tax_Rate,
            this.Taxable_Value,
            this.IGST,
            this.CGST,
            this.SGST,
            this.Cess,
            this.Return_Status,
            this.Our_Entry_Ref_No});
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgProductData.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgProductData.EnableHeadersVisualStyles = false;
            this.dgProductData.Location = new System.Drawing.Point(26, 124);
            this.dgProductData.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dgProductData.Name = "dgProductData";
            this.dgProductData.RowHeadersWidth = 62;
            this.dgProductData.Size = new System.Drawing.Size(1017, 507);
            this.dgProductData.TabIndex = 88;
            // 
            // GSTIN
            // 
            this.GSTIN.DataPropertyName = "GSTIN";
            this.GSTIN.HeaderText = "GSTIN NO";
            this.GSTIN.MinimumWidth = 8;
            this.GSTIN.Name = "GSTIN";
            this.GSTIN.Width = 70;
            // 
            // AccName
            // 
            this.AccName.DataPropertyName = "AccName";
            this.AccName.HeaderText = "Supplier Name";
            this.AccName.MinimumWidth = 8;
            this.AccName.Name = "AccName";
            this.AccName.Width = 150;
            // 
            // Inv_No
            // 
            this.Inv_No.DataPropertyName = "Inv_No";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Inv_No.DefaultCellStyle = dataGridViewCellStyle3;
            this.Inv_No.HeaderText = "Inv No";
            this.Inv_No.MaxInputLength = 50;
            this.Inv_No.MinimumWidth = 8;
            this.Inv_No.Name = "Inv_No";
            this.Inv_No.Width = 65;
            // 
            // Inv_type
            // 
            this.Inv_type.DataPropertyName = "Inv_Type";
            this.Inv_type.HeaderText = "Inv Type";
            this.Inv_type.MinimumWidth = 8;
            this.Inv_type.Name = "Inv_type";
            this.Inv_type.Width = 65;
            // 
            // Inv_Date
            // 
            this.Inv_Date.DataPropertyName = "Inv_Date";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Inv_Date.DefaultCellStyle = dataGridViewCellStyle4;
            this.Inv_Date.HeaderText = "Inv Date";
            this.Inv_Date.MinimumWidth = 8;
            this.Inv_Date.Name = "Inv_Date";
            this.Inv_Date.Width = 60;
            // 
            // Inv_Value
            // 
            this.Inv_Value.DataPropertyName = "Inv_Value";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Inv_Value.DefaultCellStyle = dataGridViewCellStyle5;
            this.Inv_Value.HeaderText = "Inv Value";
            this.Inv_Value.MinimumWidth = 8;
            this.Inv_Value.Name = "Inv_Value";
            this.Inv_Value.Width = 60;
            // 
            // RCM
            // 
            this.RCM.DataPropertyName = "RCM";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.RCM.DefaultCellStyle = dataGridViewCellStyle6;
            this.RCM.HeaderText = "Apply RC";
            this.RCM.MinimumWidth = 8;
            this.RCM.Name = "RCM";
            this.RCM.Width = 70;
            // 
            // Tax_Rate
            // 
            this.Tax_Rate.DataPropertyName = "Tax_Rate";
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Tax_Rate.DefaultCellStyle = dataGridViewCellStyle7;
            this.Tax_Rate.HeaderText = "Tax Rate";
            this.Tax_Rate.MinimumWidth = 8;
            this.Tax_Rate.Name = "Tax_Rate";
            this.Tax_Rate.Width = 150;
            // 
            // Taxable_Value
            // 
            this.Taxable_Value.DataPropertyName = "Taxable_Value";
            this.Taxable_Value.HeaderText = "Taxable Value";
            this.Taxable_Value.MinimumWidth = 8;
            this.Taxable_Value.Name = "Taxable_Value";
            this.Taxable_Value.Width = 150;
            // 
            // IGST
            // 
            this.IGST.DataPropertyName = "IGST";
            this.IGST.HeaderText = "Integrated Tax";
            this.IGST.MinimumWidth = 8;
            this.IGST.Name = "IGST";
            this.IGST.Width = 150;
            // 
            // CGST
            // 
            this.CGST.DataPropertyName = "CGST";
            this.CGST.HeaderText = "Central Tax";
            this.CGST.MinimumWidth = 8;
            this.CGST.Name = "CGST";
            this.CGST.Width = 150;
            // 
            // SGST
            // 
            this.SGST.DataPropertyName = "SGST";
            this.SGST.HeaderText = "State Tax";
            this.SGST.MinimumWidth = 8;
            this.SGST.Name = "SGST";
            this.SGST.Width = 150;
            // 
            // Cess
            // 
            this.Cess.DataPropertyName = "Cess";
            this.Cess.HeaderText = "Cess";
            this.Cess.MinimumWidth = 8;
            this.Cess.Name = "Cess";
            this.Cess.Width = 150;
            // 
            // Return_Status
            // 
            this.Return_Status.DataPropertyName = "Return_Status";
            this.Return_Status.HeaderText = "Return Status";
            this.Return_Status.MinimumWidth = 8;
            this.Return_Status.Name = "Return_Status";
            this.Return_Status.Width = 150;
            // 
            // Our_Entry_Ref_No
            // 
            this.Our_Entry_Ref_No.DataPropertyName = "Our_Entry_Ref_No";
            this.Our_Entry_Ref_No.HeaderText = "Our_Entry_Ref_No";
            this.Our_Entry_Ref_No.MinimumWidth = 8;
            this.Our_Entry_Ref_No.Name = "Our_Entry_Ref_No";
            this.Our_Entry_Ref_No.Width = 150;
            // 
            // frmImport_GSTR2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1068, 642);
            this.Controls.Add(this.dgProductData);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtExcellSheet);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtChooseFile);
            this.Controls.Add(this.label12);
            this.Name = "frmImport_GSTR2";
            this.Text = "frmImport_GSTR2";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgProductData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtExcellSheet;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.TextBox txtChooseFile;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DataGridView dgProductData;
        private System.Windows.Forms.DataGridViewTextBoxColumn GSTIN;
        private System.Windows.Forms.DataGridViewTextBoxColumn AccName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Inv_No;
        private System.Windows.Forms.DataGridViewTextBoxColumn Inv_type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Inv_Date;
        private System.Windows.Forms.DataGridViewTextBoxColumn Inv_Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn RCM;
        private System.Windows.Forms.DataGridViewTextBoxColumn Tax_Rate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Taxable_Value;
        private System.Windows.Forms.DataGridViewTextBoxColumn IGST;
        private System.Windows.Forms.DataGridViewTextBoxColumn CGST;
        private System.Windows.Forms.DataGridViewTextBoxColumn SGST;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cess;
        private System.Windows.Forms.DataGridViewTextBoxColumn Return_Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn Our_Entry_Ref_No;
    }
}