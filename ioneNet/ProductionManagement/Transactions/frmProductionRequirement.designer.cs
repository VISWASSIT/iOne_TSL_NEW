namespace ioneNet.ProductionManagement.Transactions
{
    partial class frmProductionRequirement
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
            this.label12 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.cmbConvPartyName = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.cmdDelete = new System.Windows.Forms.Button();
            this.dgProducts = new System.Windows.Forms.DataGridView();
            this.Item_No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Item_Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Item_Grade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Prod_Length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Order_Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Stock_Available = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.To_Be_Rolled = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.No_Of_Pieces = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SO_NO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.So_line_Item_No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.lblModified = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.txtSlipNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.dpSODate = new System.Windows.Forms.DateTimePicker();
            this.button1 = new System.Windows.Forms.Button();
            this.lblCreatedBy = new System.Windows.Forms.LinkLabel();
            this.btnClear = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button2 = new System.Windows.Forms.Button();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.sfDataGrid1 = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgProducts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sfDataGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.DarkBlue;
            this.label12.Location = new System.Drawing.Point(45, 20);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(217, 20);
            this.label12.TabIndex = 67;
            this.label12.Text = "PRODUCTION REQUIREMENT";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 11;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.8104F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3.632801F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.37641F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.33197F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.498979F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.52255F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.488372F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 0.744186F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.767442F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.490215F));
            this.tableLayoutPanel1.Controls.Add(this.cmbConvPartyName, 9, 0);
            this.tableLayoutPanel1.Controls.Add(this.label9, 8, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 10, 3);
            this.tableLayoutPanel1.Controls.Add(this.cmdDelete, 9, 3);
            this.tableLayoutPanel1.Controls.Add(this.dgProducts, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnSubmit, 8, 3);
            this.tableLayoutPanel1.Controls.Add(this.lblModified, 4, 3);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label30, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtSlipNo, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.label31, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.dpSODate, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.button1, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblCreatedBy, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.btnClear, 6, 3);
            this.tableLayoutPanel1.Controls.Add(this.checkBox1, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.button2, 10, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 46);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.221198F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 85.25346F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.75576F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(846, 470);
            this.tableLayoutPanel1.TabIndex = 69;
            // 
            // cmbConvPartyName
            // 
            this.cmbConvPartyName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbConvPartyName.FormattingEnabled = true;
            this.cmbConvPartyName.Location = new System.Drawing.Point(681, 3);
            this.cmbConvPartyName.Name = "cmbConvPartyName";
            this.cmbConvPartyName.Size = new System.Drawing.Size(76, 21);
            this.cmbConvPartyName.TabIndex = 152;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(580, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(95, 27);
            this.label9.TabIndex = 153;
            this.label9.Text = "Party Name";
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Image = global::ioneNet.Properties.Resources.Cancel_16x161;
            this.btnClose.Location = new System.Drawing.Point(762, 433);
            this.btnClose.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(82, 33);
            this.btnClose.TabIndex = 148;
            this.btnClose.Text = "&Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cmdDelete
            // 
            this.cmdDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDelete.BackColor = System.Drawing.Color.White;
            this.cmdDelete.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdDelete.ForeColor = System.Drawing.Color.Black;
            this.cmdDelete.Image = global::ioneNet.Properties.Resources.Delete;
            this.cmdDelete.Location = new System.Drawing.Point(681, 432);
            this.cmdDelete.Name = "cmdDelete";
            this.cmdDelete.Size = new System.Drawing.Size(76, 35);
            this.cmdDelete.TabIndex = 150;
            this.cmdDelete.Text = "Delete";
            this.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cmdDelete.UseVisualStyleBackColor = false;
            this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
            // 
            // dgProducts
            // 
            this.dgProducts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSkyBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgProducts.ColumnHeadersHeight = 34;
            this.dgProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Item_No,
            this.Item_Description,
            this.Item_Grade,
            this.Prod_Length,
            this.Order_Qty,
            this.Stock_Available,
            this.To_Be_Rolled,
            this.No_Of_Pieces,
            this.SO_NO,
            this.So_line_Item_No,
            this.Remarks});
            this.tableLayoutPanel1.SetColumnSpan(this.dgProducts, 11);
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgProducts.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgProducts.EnableHeadersVisualStyles = false;
            this.dgProducts.Location = new System.Drawing.Point(3, 50);
            this.dgProducts.Name = "dgProducts";
            this.dgProducts.RowHeadersWidth = 62;
            this.dgProducts.Size = new System.Drawing.Size(840, 376);
            this.dgProducts.TabIndex = 0;
            this.dgProducts.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgProducts_CellEndEdit);
            this.dgProducts.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgProducts_EditingControlShowing);
            // 
            // Item_No
            // 
            this.Item_No.DataPropertyName = "Item_No";
            this.Item_No.HeaderText = "Item Code";
            this.Item_No.MinimumWidth = 8;
            this.Item_No.Name = "Item_No";
            // 
            // Item_Description
            // 
            this.Item_Description.DataPropertyName = "Item_Description";
            this.Item_Description.HeaderText = "Item Description";
            this.Item_Description.MinimumWidth = 8;
            this.Item_Description.Name = "Item_Description";
            // 
            // Item_Grade
            // 
            this.Item_Grade.DataPropertyName = "Item_Grade";
            this.Item_Grade.HeaderText = "Prod Grade";
            this.Item_Grade.MinimumWidth = 8;
            this.Item_Grade.Name = "Item_Grade";
            // 
            // Prod_Length
            // 
            this.Prod_Length.DataPropertyName = "Prod_Length";
            this.Prod_Length.HeaderText = "Prod_Length";
            this.Prod_Length.MinimumWidth = 8;
            this.Prod_Length.Name = "Prod_Length";
            // 
            // Order_Qty
            // 
            this.Order_Qty.DataPropertyName = "Order_Qty";
            this.Order_Qty.HeaderText = "Ord Bal Qty";
            this.Order_Qty.MinimumWidth = 8;
            this.Order_Qty.Name = "Order_Qty";
            // 
            // Stock_Available
            // 
            this.Stock_Available.DataPropertyName = "Stock_Available";
            this.Stock_Available.HeaderText = "Stock/Delivered";
            this.Stock_Available.MinimumWidth = 6;
            this.Stock_Available.Name = "Stock_Available";
            // 
            // To_Be_Rolled
            // 
            this.To_Be_Rolled.DataPropertyName = "To_Be_Rolled";
            this.To_Be_Rolled.HeaderText = "To_Be_Rolled_MT";
            this.To_Be_Rolled.MinimumWidth = 6;
            this.To_Be_Rolled.Name = "To_Be_Rolled";
            // 
            // No_Of_Pieces
            // 
            this.No_Of_Pieces.DataPropertyName = "No_Of_Pieces";
            this.No_Of_Pieces.HeaderText = "To_Be_Rolled_Nos";
            this.No_Of_Pieces.MinimumWidth = 8;
            this.No_Of_Pieces.Name = "No_Of_Pieces";
            this.No_Of_Pieces.ReadOnly = true;
            // 
            // SO_NO
            // 
            this.SO_NO.DataPropertyName = "SO_NO";
            this.SO_NO.HeaderText = "SO_No";
            this.SO_NO.MinimumWidth = 8;
            this.SO_NO.Name = "SO_NO";
            this.SO_NO.ReadOnly = true;
            // 
            // So_line_Item_No
            // 
            this.So_line_Item_No.DataPropertyName = "So_line_Item_No";
            this.So_line_Item_No.HeaderText = "SO_ItemNo";
            this.So_line_Item_No.MinimumWidth = 8;
            this.So_line_Item_No.Name = "So_line_Item_No";
            // 
            // Remarks
            // 
            this.Remarks.DataPropertyName = "Remarks";
            this.Remarks.HeaderText = "Remarks";
            this.Remarks.MinimumWidth = 8;
            this.Remarks.Name = "Remarks";
            // 
            // btnSubmit
            // 
            this.btnSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSubmit.BackColor = System.Drawing.Color.White;
            this.btnSubmit.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmit.Image = global::ioneNet.Properties.Resources.Save_16x16;
            this.btnSubmit.Location = new System.Drawing.Point(579, 433);
            this.btnSubmit.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(97, 33);
            this.btnSubmit.TabIndex = 147;
            this.btnSubmit.Text = "&Submit";
            this.btnSubmit.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnSubmit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // lblModified
            // 
            this.lblModified.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblModified.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblModified, 2);
            this.lblModified.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModified.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblModified.Location = new System.Drawing.Point(333, 429);
            this.lblModified.Name = "lblModified";
            this.lblModified.Size = new System.Drawing.Size(155, 41);
            this.lblModified.TabIndex = 6;
            this.lblModified.TabStop = true;
            this.lblModified.Text = "linkLabel8";
            this.lblModified.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 27);
            this.label2.TabIndex = 0;
            this.label2.Text = "Ref No";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label30
            // 
            this.label30.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.Location = new System.Drawing.Point(237, 429);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(62, 41);
            this.label30.TabIndex = 5;
            this.label30.Text = "Modified By";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtSlipNo
            // 
            this.txtSlipNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtSlipNo, 2);
            this.txtSlipNo.Enabled = false;
            this.txtSlipNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSlipNo.Location = new System.Drawing.Point(111, 3);
            this.txtSlipNo.Name = "txtSlipNo";
            this.txtSlipNo.Size = new System.Drawing.Size(120, 21);
            this.txtSlipNo.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(333, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 27);
            this.label3.TabIndex = 2;
            this.label3.Text = "Date";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label31
            // 
            this.label31.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.Location = new System.Drawing.Point(3, 429);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(59, 41);
            this.label31.TabIndex = 3;
            this.label31.Text = "Created By";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dpSODate
            // 
            this.dpSODate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dpSODate.CustomFormat = "dd/MM/yyyy";
            this.dpSODate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dpSODate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dpSODate.Location = new System.Drawing.Point(405, 3);
            this.dpSODate.Name = "dpSODate";
            this.dpSODate.ShowUpDown = true;
            this.dpSODate.Size = new System.Drawing.Size(83, 21);
            this.dpSODate.TabIndex = 3;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(237, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(74, 20);
            this.button1.TabIndex = 15;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Visible = false;
            // 
            // lblCreatedBy
            // 
            this.lblCreatedBy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCreatedBy.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.lblCreatedBy, 2);
            this.lblCreatedBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreatedBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblCreatedBy.Location = new System.Drawing.Point(111, 429);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(120, 41);
            this.lblCreatedBy.TabIndex = 4;
            this.lblCreatedBy.TabStop = true;
            this.lblCreatedBy.Text = "linkLabel8";
            this.lblCreatedBy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.BackColor = System.Drawing.Color.White;
            this.tableLayoutPanel1.SetColumnSpan(this.btnClear, 2);
            this.btnClear.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Image = global::ioneNet.Properties.Resources.Refresh_16x16;
            this.btnClear.Location = new System.Drawing.Point(493, 433);
            this.btnClear.Margin = new System.Windows.Forms.Padding(2, 4, 2, 4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(82, 33);
            this.btnClear.TabIndex = 149;
            this.btnClear.Text = "&Reset";
            this.btnClear.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.checkBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox1.Location = new System.Drawing.Point(494, 3);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(74, 21);
            this.checkBox1.TabIndex = 17;
            this.checkBox1.Text = "Make to Stock";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(763, 3);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(80, 21);
            this.button2.TabIndex = 16;
            this.button2.Text = "Select SO List";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // checkBox2
            // 
            this.checkBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox2.AutoSize = true;
            this.checkBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBox2.ForeColor = System.Drawing.Color.Blue;
            this.checkBox2.Location = new System.Drawing.Point(613, 16);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(2);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(105, 19);
            this.checkBox2.TabIndex = 151;
            this.checkBox2.Text = "Conversion?";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ioneNet.Properties.Resources.logoface_Ione;
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(45, 40);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 68;
            this.pictureBox2.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.Azure;
            this.groupBox2.Controls.Add(this.sfDataGrid1);
            this.groupBox2.Controls.Add(this.btnCancel);
            this.groupBox2.Controls.Add(this.btnOK);
            this.groupBox2.Location = new System.Drawing.Point(42, 237);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(2);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(2);
            this.groupBox2.Size = new System.Drawing.Size(807, 246);
            this.groupBox2.TabIndex = 70;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Get Order Data";
            this.groupBox2.Visible = false;
            // 
            // sfDataGrid1
            // 
            this.sfDataGrid1.AccessibleName = "Table";
            this.sfDataGrid1.AllowResizingColumns = true;
            this.sfDataGrid1.AutoSizeColumnsMode = Syncfusion.WinForms.DataGrid.Enums.AutoSizeColumnsMode.Fill;
            this.sfDataGrid1.Location = new System.Drawing.Point(17, 29);
            this.sfDataGrid1.Margin = new System.Windows.Forms.Padding(2);
            this.sfDataGrid1.Name = "sfDataGrid1";
            this.sfDataGrid1.PreviewRowHeight = 35;
            this.sfDataGrid1.SelectionMode = Syncfusion.WinForms.DataGrid.Enums.GridSelectionMode.Multiple;
            this.sfDataGrid1.Size = new System.Drawing.Size(779, 414);
            this.sfDataGrid1.TabIndex = 200;
            this.sfDataGrid1.Text = "sfDataGrid1";
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCancel.Location = new System.Drawing.Point(721, 448);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(65, 25);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.White;
            this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnOK.Location = new System.Drawing.Point(633, 448);
            this.btnOK.Margin = new System.Windows.Forms.Padding(2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(80, 25);
            this.btnOK.TabIndex = 4;
            this.btnOK.Text = "Select";
            this.btnOK.UseVisualStyleBackColor = false;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // frmProductionRequirement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(885, 544);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.checkBox2);
            this.Name = "frmProductionRequirement";
            this.Text = "Production Requirement";
            this.Load += new System.EventHandler(this.frmForge_ProductionPlanning_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgProducts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sfDataGrid1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSlipNo;
        private System.Windows.Forms.DateTimePicker dpSODate;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dgProducts;
        private System.Windows.Forms.LinkLabel lblCreatedBy;
        private System.Windows.Forms.LinkLabel lblModified;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button btnClose;
        internal System.Windows.Forms.Button cmdDelete;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.GroupBox groupBox2;
        private Syncfusion.WinForms.DataGrid.SfDataGrid sfDataGrid1;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item_No;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item_Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item_Grade;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prod_Length;
        private System.Windows.Forms.DataGridViewTextBoxColumn Order_Qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Stock_Available;
        private System.Windows.Forms.DataGridViewTextBoxColumn To_Be_Rolled;
        private System.Windows.Forms.DataGridViewTextBoxColumn No_Of_Pieces;
        private System.Windows.Forms.DataGridViewTextBoxColumn SO_NO;
        private System.Windows.Forms.DataGridViewTextBoxColumn So_line_Item_No;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.ComboBox cmbConvPartyName;
        private System.Windows.Forms.Label label9;
    }
}