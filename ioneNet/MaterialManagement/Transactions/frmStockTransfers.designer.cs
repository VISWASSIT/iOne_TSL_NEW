namespace ioneNet.MaterialManagement.Transactions
{
    partial class frmStockTransfers
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStockTransfers));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.brnSearch = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSlipNo = new System.Windows.Forms.TextBox();
            this.cmbTransferTo = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbTransferFrom = new System.Windows.Forms.ComboBox();
            this.label38 = new System.Windows.Forms.Label();
            this.dpSODate = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.lblCreatedBy = new System.Windows.Forms.LinkLabel();
            this.label33 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.lblModified = new System.Windows.Forms.LinkLabel();
            this.label30 = new System.Windows.Forms.Label();
            this.dgProducts = new System.Windows.Forms.DataGridView();
            this.S_No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Item_Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Prod_Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Item_Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Item_Spec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Item_Grade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UOM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Transfer_Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Heat_No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.TCNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Int_Batch_No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgProducts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.DarkBlue;
            this.label1.Location = new System.Drawing.Point(71, 30);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 23);
            this.label1.TabIndex = 14;
            this.label1.Text = "STOCK TRANSFERS";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // brnSearch
            // 
            this.brnSearch.BackColor = System.Drawing.Color.MintCream;
            this.brnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.brnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.brnSearch.ForeColor = System.Drawing.Color.Black;
            this.brnSearch.Image = ((System.Drawing.Image)(resources.GetObject("brnSearch.Image")));
            this.brnSearch.Location = new System.Drawing.Point(256, 4);
            this.brnSearch.Margin = new System.Windows.Forms.Padding(4);
            this.brnSearch.Name = "brnSearch";
            this.brnSearch.Size = new System.Drawing.Size(33, 30);
            this.brnSearch.TabIndex = 133;
            this.brnSearch.TabStop = false;
            this.brnSearch.UseVisualStyleBackColor = false;
            this.brnSearch.Click += new System.EventHandler(this.brnSearch_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(4, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(118, 38);
            this.label2.TabIndex = 0;
            this.label2.Text = "Slip No";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtSlipNo
            // 
            this.txtSlipNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSlipNo.Enabled = false;
            this.txtSlipNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSlipNo.Location = new System.Drawing.Point(130, 4);
            this.txtSlipNo.Margin = new System.Windows.Forms.Padding(4);
            this.txtSlipNo.Name = "txtSlipNo";
            this.txtSlipNo.Size = new System.Drawing.Size(118, 24);
            this.txtSlipNo.TabIndex = 1;
            // 
            // cmbTransferTo
            // 
            this.cmbTransferTo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTransferTo.BackColor = System.Drawing.SystemColors.Info;
            this.cmbTransferTo.FormattingEnabled = true;
            this.cmbTransferTo.Location = new System.Drawing.Point(1012, 4);
            this.cmbTransferTo.Margin = new System.Windows.Forms.Padding(4);
            this.cmbTransferTo.Name = "cmbTransferTo";
            this.cmbTransferTo.Size = new System.Drawing.Size(118, 24);
            this.cmbTransferTo.TabIndex = 13;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.Color.Black;
            this.label4.Location = new System.Drawing.Point(886, 0);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(118, 38);
            this.label4.TabIndex = 10;
            this.label4.Text = "To";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbTransferFrom
            // 
            this.cmbTransferFrom.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbTransferFrom.FormattingEnabled = true;
            this.cmbTransferFrom.Items.AddRange(new object[] {
            "PR",
            "NA"});
            this.cmbTransferFrom.Location = new System.Drawing.Point(760, 4);
            this.cmbTransferFrom.Margin = new System.Windows.Forms.Padding(4);
            this.cmbTransferFrom.Name = "cmbTransferFrom";
            this.cmbTransferFrom.Size = new System.Drawing.Size(118, 24);
            this.cmbTransferFrom.TabIndex = 14;
            // 
            // label38
            // 
            this.label38.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label38.AutoSize = true;
            this.label38.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label38.Location = new System.Drawing.Point(634, 0);
            this.label38.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(118, 38);
            this.label38.TabIndex = 4;
            this.label38.Text = "From";
            this.label38.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dpSODate
            // 
            this.dpSODate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dpSODate.CustomFormat = "dd/MM/yyyy";
            this.dpSODate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dpSODate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dpSODate.Location = new System.Drawing.Point(508, 4);
            this.dpSODate.Margin = new System.Windows.Forms.Padding(4);
            this.dpSODate.Name = "dpSODate";
            this.dpSODate.ShowUpDown = true;
            this.dpSODate.Size = new System.Drawing.Size(118, 24);
            this.dpSODate.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(382, 0);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(118, 38);
            this.label3.TabIndex = 2;
            this.label3.Text = "Date";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 10;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel2.Controls.Add(this.txtRemarks, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.lblCreatedBy, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.label33, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label8, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label31, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.txtSlipNo, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.brnSearch, 2, 0);
            this.tableLayoutPanel2.Controls.Add(this.label3, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.dpSODate, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.label38, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmbTransferFrom, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.label4, 7, 0);
            this.tableLayoutPanel2.Controls.Add(this.cmbTransferTo, 8, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnClose, 9, 4);
            this.tableLayoutPanel2.Controls.Add(this.btnDelete, 8, 4);
            this.tableLayoutPanel2.Controls.Add(this.btnSave, 7, 4);
            this.tableLayoutPanel2.Controls.Add(this.btnClear, 6, 4);
            this.tableLayoutPanel2.Controls.Add(this.lblModified, 4, 4);
            this.tableLayoutPanel2.Controls.Add(this.label30, 3, 4);
            this.tableLayoutPanel2.Controls.Add(this.dgProducts, 0, 1);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(5, 69);
            this.tableLayoutPanel2.Margin = new System.Windows.Forms.Padding(4);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 5;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.436337F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 76F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.571429F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.142857F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.714286F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1262, 700);
            this.tableLayoutPanel2.TabIndex = 17;
            // 
            // txtRemarks
            // 
            this.txtRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.txtRemarks, 4);
            this.txtRemarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRemarks.Location = new System.Drawing.Point(130, 606);
            this.txtRemarks.Margin = new System.Windows.Forms.Padding(4);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRemarks.Size = new System.Drawing.Size(496, 35);
            this.txtRemarks.TabIndex = 11;
            // 
            // lblCreatedBy
            // 
            this.lblCreatedBy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCreatedBy.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.lblCreatedBy, 2);
            this.lblCreatedBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreatedBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblCreatedBy.Location = new System.Drawing.Point(130, 645);
            this.lblCreatedBy.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(244, 55);
            this.lblCreatedBy.TabIndex = 4;
            this.lblCreatedBy.TabStop = true;
            this.lblCreatedBy.Text = "linkLabel8";
            this.lblCreatedBy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.ForeColor = System.Drawing.Color.DarkBlue;
            this.label33.Location = new System.Drawing.Point(4, 570);
            this.label33.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(100, 16);
            this.label33.TabIndex = 206;
            this.label33.Text = "F6 - Delete Item";
            this.label33.Click += new System.EventHandler(this.label33_Click);
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(4, 602);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(118, 43);
            this.label8.TabIndex = 10;
            this.label8.Text = "Remarks";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label31
            // 
            this.label31.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.Location = new System.Drawing.Point(4, 645);
            this.label31.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(78, 55);
            this.label31.TabIndex = 3;
            this.label31.Text = "Created By";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(1138, 651);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(120, 43);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(1012, 649);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(118, 47);
            this.btnDelete.TabIndex = 7;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(886, 651);
            this.btnSave.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(118, 43);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Submit";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(760, 651);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(118, 43);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            // 
            // lblModified
            // 
            this.lblModified.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblModified.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.lblModified, 2);
            this.lblModified.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModified.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblModified.Location = new System.Drawing.Point(508, 645);
            this.lblModified.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblModified.Name = "lblModified";
            this.lblModified.Size = new System.Drawing.Size(244, 55);
            this.lblModified.TabIndex = 6;
            this.lblModified.TabStop = true;
            this.lblModified.Text = "linkLabel8";
            this.lblModified.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label30
            // 
            this.label30.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.Location = new System.Drawing.Point(382, 645);
            this.label30.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(81, 55);
            this.label30.TabIndex = 5;
            this.label30.Text = "Modified By";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgProducts
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.Beige;
            this.dgProducts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgProducts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.LightSkyBlue;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgProducts.ColumnHeadersHeight = 34;
            this.dgProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.S_No,
            this.Item_Code,
            this.Prod_Code,
            this.Item_Description,
            this.Item_Spec,
            this.Item_Grade,
            this.UOM,
            this.Transfer_Qty,
            this.Heat_No,
            this.TCNo,
            this.Int_Batch_No,
            this.Remarks});
            this.tableLayoutPanel2.SetColumnSpan(this.dgProducts, 10);
            this.dgProducts.EnableHeadersVisualStyles = false;
            this.dgProducts.Location = new System.Drawing.Point(3, 41);
            this.dgProducts.Name = "dgProducts";
            this.dgProducts.RowHeadersWidth = 62;
            this.dgProducts.Size = new System.Drawing.Size(1256, 526);
            this.dgProducts.TabIndex = 18;
            // 
            // S_No
            // 
            this.S_No.DataPropertyName = "S_No";
            this.S_No.HeaderText = "S_No";
            this.S_No.MinimumWidth = 6;
            this.S_No.Name = "S_No";
            this.S_No.Width = 125;
            // 
            // Item_Code
            // 
            this.Item_Code.DataPropertyName = "Item_Code";
            this.Item_Code.FillWeight = 256.917F;
            this.Item_Code.HeaderText = "Item Code";
            this.Item_Code.MinimumWidth = 8;
            this.Item_Code.Name = "Item_Code";
            this.Item_Code.Visible = false;
            this.Item_Code.Width = 134;
            // 
            // Prod_Code
            // 
            this.Prod_Code.DataPropertyName = "Prod_Code";
            this.Prod_Code.HeaderText = "Prod Code";
            this.Prod_Code.MinimumWidth = 8;
            this.Prod_Code.Name = "Prod_Code";
            this.Prod_Code.Width = 150;
            // 
            // Item_Description
            // 
            this.Item_Description.DataPropertyName = "Item_Description";
            this.Item_Description.FillWeight = 465.773F;
            this.Item_Description.HeaderText = "Item Description";
            this.Item_Description.MinimumWidth = 8;
            this.Item_Description.Name = "Item_Description";
            this.Item_Description.Width = 187;
            // 
            // Item_Spec
            // 
            this.Item_Spec.DataPropertyName = "Item_Spec";
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.Item_Spec.DefaultCellStyle = dataGridViewCellStyle3;
            this.Item_Spec.HeaderText = "Spec/ Length";
            this.Item_Spec.MinimumWidth = 8;
            this.Item_Spec.Name = "Item_Spec";
            this.Item_Spec.Width = 157;
            // 
            // Item_Grade
            // 
            this.Item_Grade.DataPropertyName = "Item_Grade";
            this.Item_Grade.FillWeight = 282.3272F;
            this.Item_Grade.HeaderText = "Make /Model / Grade";
            this.Item_Grade.MinimumWidth = 8;
            this.Item_Grade.Name = "Item_Grade";
            this.Item_Grade.Width = 226;
            // 
            // UOM
            // 
            this.UOM.DataPropertyName = "UOM";
            this.UOM.FillWeight = 80.83842F;
            this.UOM.HeaderText = "UOM";
            this.UOM.MinimumWidth = 8;
            this.UOM.Name = "UOM";
            this.UOM.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.UOM.Width = 92;
            // 
            // Transfer_Qty
            // 
            this.Transfer_Qty.DataPropertyName = "Transfer_Qty";
            this.Transfer_Qty.FillWeight = 95.13086F;
            this.Transfer_Qty.HeaderText = "Transfer Qty";
            this.Transfer_Qty.MinimumWidth = 8;
            this.Transfer_Qty.Name = "Transfer_Qty";
            this.Transfer_Qty.ReadOnly = true;
            this.Transfer_Qty.Width = 122;
            // 
            // Heat_No
            // 
            this.Heat_No.DataPropertyName = "Heat_No";
            this.Heat_No.FillWeight = 46.3914F;
            this.Heat_No.HeaderText = "Supp Heat No";
            this.Heat_No.MinimumWidth = 8;
            this.Heat_No.Name = "Heat_No";
            this.Heat_No.Width = 165;
            // 
            // TCNo
            // 
            this.TCNo.DataPropertyName = "TCNo";
            this.TCNo.FillWeight = 43.16387F;
            this.TCNo.HeaderText = "TC No";
            this.TCNo.MinimumWidth = 8;
            this.TCNo.Name = "TCNo";
            this.TCNo.Width = 98;
            // 
            // Int_Batch_No
            // 
            this.Int_Batch_No.DataPropertyName = "Int_Batch_No";
            this.Int_Batch_No.HeaderText = "Int Batch/Lot No";
            this.Int_Batch_No.MinimumWidth = 8;
            this.Int_Batch_No.Name = "Int_Batch_No";
            this.Int_Batch_No.Width = 185;
            // 
            // Remarks
            // 
            this.Remarks.DataPropertyName = "Remarks";
            this.Remarks.FillWeight = 40.25526F;
            this.Remarks.HeaderText = "Remarks";
            this.Remarks.MinimumWidth = 8;
            this.Remarks.Name = "Remarks";
            this.Remarks.Width = 118;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ioneNet.Properties.Resources.logoface_Ione;
            this.pictureBox2.Location = new System.Drawing.Point(3, -1);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(60, 49);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 15;
            this.pictureBox2.TabStop = false;
            // 
            // frmStockTransfers
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1271, 808);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmStockTransfers";
            this.Text = "frmStockTransfers";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMaterialIssues_FormClosed);
            this.Load += new System.EventHandler(this.frmMaterialIssues_Load);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgProducts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSlipNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker dpSODate;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.LinkLabel lblCreatedBy;
        private System.Windows.Forms.LinkLabel lblModified;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.ComboBox cmbTransferTo;
        private System.Windows.Forms.Button brnSearch;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.ComboBox cmbTransferFrom;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.DataGridView dgProducts;
        private System.Windows.Forms.DataGridViewTextBoxColumn S_No;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item_Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prod_Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item_Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item_Spec;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item_Grade;
        private System.Windows.Forms.DataGridViewTextBoxColumn UOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn Transfer_Qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Heat_No;
        private System.Windows.Forms.DataGridViewTextBoxColumn TCNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Int_Batch_No;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
    }
}