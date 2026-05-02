namespace ioneNet
{
    partial class MaterialRequirementPlanning
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MaterialRequirementPlanning));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.TxtQty = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnSearch = new System.Windows.Forms.Button();
            this.dpIndentDate = new System.Windows.Forms.DateTimePicker();
            this.Label3 = new System.Windows.Forms.Label();
            this.txtIndentNo = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.txtPordNo = new System.Windows.Forms.TextBox();
            this.BtnGetOrders = new System.Windows.Forms.Button();
            this.Label1 = new System.Windows.Forms.Label();
            this.dgIndent = new System.Windows.Forms.DataGridView();
            this.ProdCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProductName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Product_Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Unit = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RequiredQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StockInQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ProcureQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MfgQty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Min_Stock = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnPO = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.txttotalQty = new System.Windows.Forms.TextBox();
            this.btnPR = new System.Windows.Forms.Button();
            this.PictureBox3 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.dgPO = new System.Windows.Forms.DataGridView();
            this.dgMFg = new System.Windows.Forms.DataGridView();
            this.btnGenetePOItems = new System.Windows.Forms.Button();
            this.GenerateMfgItems = new System.Windows.Forms.Button();
            this.GroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgIndent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgPO)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgMFg)).BeginInit();
            this.SuspendLayout();
            // 
            // GroupBox1
            // 
            this.GroupBox1.AutoSize = true;
            this.GroupBox1.Controls.Add(this.TxtQty);
            this.GroupBox1.Controls.Add(this.label5);
            this.GroupBox1.Controls.Add(this.txtProductName);
            this.GroupBox1.Controls.Add(this.label4);
            this.GroupBox1.Controls.Add(this.btnGenerate);
            this.GroupBox1.Controls.Add(this.btnSearch);
            this.GroupBox1.Controls.Add(this.dpIndentDate);
            this.GroupBox1.Controls.Add(this.Label3);
            this.GroupBox1.Controls.Add(this.txtIndentNo);
            this.GroupBox1.Controls.Add(this.Label2);
            this.GroupBox1.Location = new System.Drawing.Point(5, 47);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(1230, 74);
            this.GroupBox1.TabIndex = 0;
            this.GroupBox1.TabStop = false;
            // 
            // TxtQty
            // 
            this.TxtQty.Location = new System.Drawing.Point(1009, 21);
            this.TxtQty.Name = "TxtQty";
            this.TxtQty.Size = new System.Drawing.Size(100, 26);
            this.TxtQty.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.Color.Blue;
            this.label5.Location = new System.Drawing.Point(944, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 18);
            this.label5.TabIndex = 72;
            this.label5.Text = "Quantity";
            // 
            // txtProductName
            // 
            this.txtProductName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.txtProductName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.txtProductName.Location = new System.Drawing.Point(576, 21);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(346, 26);
            this.txtProductName.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Blue;
            this.label4.Location = new System.Drawing.Point(421, 21);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(152, 18);
            this.label4.TabIndex = 70;
            this.label4.Text = "Finished Product Name";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Location = new System.Drawing.Point(1115, 21);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(74, 26);
            this.btnGenerate.TabIndex = 4;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // btnSearch
            // 
            this.btnSearch.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSearch.ForeColor = System.Drawing.Color.Black;
            this.btnSearch.Image = ((System.Drawing.Image)(resources.GetObject("btnSearch.Image")));
            this.btnSearch.Location = new System.Drawing.Point(193, 21);
            this.btnSearch.Name = "btnSearch";
            this.btnSearch.Size = new System.Drawing.Size(28, 28);
            this.btnSearch.TabIndex = 69;
            this.btnSearch.UseVisualStyleBackColor = true;
            this.btnSearch.Click += new System.EventHandler(this.btnSearch_Click);
            // 
            // dpIndentDate
            // 
            this.dpIndentDate.Checked = false;
            this.dpIndentDate.CustomFormat = "dd/MM/yyyy";
            this.dpIndentDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dpIndentDate.Location = new System.Drawing.Point(288, 21);
            this.dpIndentDate.Name = "dpIndentDate";
            this.dpIndentDate.Size = new System.Drawing.Size(100, 26);
            this.dpIndentDate.TabIndex = 1;
            this.dpIndentDate.Validating += new System.ComponentModel.CancelEventHandler(this.dpIndentDate_Validating);
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.ForeColor = System.Drawing.Color.Blue;
            this.Label3.Location = new System.Drawing.Point(243, 21);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(40, 18);
            this.Label3.TabIndex = 2;
            this.Label3.Text = " Date";
            // 
            // txtIndentNo
            // 
            this.txtIndentNo.Enabled = false;
            this.txtIndentNo.Location = new System.Drawing.Point(87, 21);
            this.txtIndentNo.Name = "txtIndentNo";
            this.txtIndentNo.Size = new System.Drawing.Size(100, 26);
            this.txtIndentNo.TabIndex = 0;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.ForeColor = System.Drawing.Color.Blue;
            this.Label2.Location = new System.Drawing.Point(36, 21);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(50, 18);
            this.Label2.TabIndex = 0;
            this.Label2.Text = "Ref No";
            // 
            // txtPordNo
            // 
            this.txtPordNo.Enabled = false;
            this.txtPordNo.Location = new System.Drawing.Point(720, 23);
            this.txtPordNo.Name = "txtPordNo";
            this.txtPordNo.Size = new System.Drawing.Size(160, 26);
            this.txtPordNo.TabIndex = 2;
            this.txtPordNo.Visible = false;
            // 
            // BtnGetOrders
            // 
            this.BtnGetOrders.Location = new System.Drawing.Point(886, 11);
            this.BtnGetOrders.Name = "BtnGetOrders";
            this.BtnGetOrders.Size = new System.Drawing.Size(178, 30);
            this.BtnGetOrders.TabIndex = 1;
            this.BtnGetOrders.Text = "Get Production OrderNos";
            this.BtnGetOrders.UseVisualStyleBackColor = true;
            this.BtnGetOrders.Visible = false;
            this.BtnGetOrders.Click += new System.EventHandler(this.BtnGetOrders_Click);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.ForeColor = System.Drawing.Color.Red;
            this.Label1.Location = new System.Drawing.Point(476, 1);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(259, 19);
            this.Label1.TabIndex = 2;
            this.Label1.Text = "MATERIAL REQUIREMENT PLANNING";
            // 
            // dgIndent
            // 
            this.dgIndent.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgIndent.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ProdCode,
            this.ProductName,
            this.Product_Type,
            this.Unit,
            this.RequiredQty,
            this.StockInQty,
            this.ProcureQty,
            this.MfgQty,
            this.Min_Stock,
            this.Column1,
            this.Column2,
            this.Column3,
            this.Column4,
            this.Column5,
            this.Column6});
            this.dgIndent.Location = new System.Drawing.Point(5, 111);
            this.dgIndent.Name = "dgIndent";
            this.dgIndent.RowHeadersVisible = false;
            this.dgIndent.Size = new System.Drawing.Size(1230, 258);
            this.dgIndent.TabIndex = 0;
            this.dgIndent.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgIndent_CellEndEdit);
            this.dgIndent.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgIndent_EditingControlShowing);
            // 
            // ProdCode
            // 
            this.ProdCode.DataPropertyName = "ProdCode";
            this.ProdCode.HeaderText = "ProdCode";
            this.ProdCode.Name = "ProdCode";
            this.ProdCode.ReadOnly = true;
            this.ProdCode.Width = 80;
            // 
            // ProductName
            // 
            this.ProductName.DataPropertyName = "ProductName";
            this.ProductName.HeaderText = "Product Name";
            this.ProductName.Name = "ProductName";
            this.ProductName.ReadOnly = true;
            this.ProductName.Width = 250;
            // 
            // Product_Type
            // 
            this.Product_Type.DataPropertyName = "Product_Type";
            this.Product_Type.HeaderText = "Product Type";
            this.Product_Type.Name = "Product_Type";
            // 
            // Unit
            // 
            this.Unit.DataPropertyName = "Unit";
            this.Unit.HeaderText = "UOM";
            this.Unit.Name = "Unit";
            this.Unit.ReadOnly = true;
            this.Unit.Width = 70;
            // 
            // RequiredQty
            // 
            this.RequiredQty.DataPropertyName = "RequiredQty";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.RequiredQty.DefaultCellStyle = dataGridViewCellStyle4;
            this.RequiredQty.HeaderText = "Qty Required";
            this.RequiredQty.Name = "RequiredQty";
            this.RequiredQty.ReadOnly = true;
            // 
            // StockInQty
            // 
            this.StockInQty.DataPropertyName = "StockInQty";
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.StockInQty.DefaultCellStyle = dataGridViewCellStyle5;
            this.StockInQty.HeaderText = "Qty in Stock";
            this.StockInQty.Name = "StockInQty";
            this.StockInQty.ReadOnly = true;
            // 
            // ProcureQty
            // 
            this.ProcureQty.DataPropertyName = "ProcureQty";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.ProcureQty.DefaultCellStyle = dataGridViewCellStyle6;
            this.ProcureQty.HeaderText = "Qty to procure";
            this.ProcureQty.Name = "ProcureQty";
            // 
            // MfgQty
            // 
            this.MfgQty.DataPropertyName = "MfgQty";
            this.MfgQty.HeaderText = "Qty to Produce";
            this.MfgQty.Name = "MfgQty";
            // 
            // Min_Stock
            // 
            this.Min_Stock.DataPropertyName = "Min_Stock";
            this.Min_Stock.HeaderText = "Minimum Stock";
            this.Min_Stock.Name = "Min_Stock";
            // 
            // Column1
            // 
            this.Column1.HeaderText = "Item Class";
            this.Column1.Name = "Column1";
            // 
            // Column2
            // 
            this.Column2.HeaderText = "Qty Reserve";
            this.Column2.Name = "Column2";
            // 
            // Column3
            // 
            this.Column3.HeaderText = "Lead Time";
            this.Column3.Name = "Column3";
            // 
            // Column4
            // 
            this.Column4.HeaderText = "PO Qty TO Receive";
            this.Column4.Name = "Column4";
            // 
            // Column5
            // 
            this.Column5.HeaderText = "Qty In WIP";
            this.Column5.Name = "Column5";
            // 
            // Column6
            // 
            this.Column6.HeaderText = "Remarks";
            this.Column6.Name = "Column6";
            // 
            // btnPO
            // 
            this.btnPO.BackColor = System.Drawing.Color.LightBlue;
            this.btnPO.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPO.Location = new System.Drawing.Point(1004, 571);
            this.btnPO.Name = "btnPO";
            this.btnPO.Size = new System.Drawing.Size(77, 40);
            this.btnPO.TabIndex = 8;
            this.btnPO.Text = "Generate \r\nProd Ord";
            this.btnPO.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPO.UseVisualStyleBackColor = false;
            this.btnPO.Click += new System.EventHandler(this.btnIssue_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(492, 580);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 18);
            this.label8.TabIndex = 258;
            this.label8.Text = "Total Qty";
            this.label8.Visible = false;
            // 
            // txttotalQty
            // 
            this.txttotalQty.Location = new System.Drawing.Point(561, 580);
            this.txttotalQty.Name = "txttotalQty";
            this.txttotalQty.Size = new System.Drawing.Size(146, 26);
            this.txttotalQty.TabIndex = 257;
            this.txttotalQty.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txttotalQty.Visible = false;
            // 
            // btnPR
            // 
            this.btnPR.BackColor = System.Drawing.Color.LightBlue;
            this.btnPR.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnPR.Location = new System.Drawing.Point(927, 571);
            this.btnPR.Name = "btnPR";
            this.btnPR.Size = new System.Drawing.Size(77, 40);
            this.btnPR.TabIndex = 7;
            this.btnPR.Text = "Generate\r\nPR";
            this.btnPR.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnPR.UseVisualStyleBackColor = false;
            this.btnPR.Click += new System.EventHandler(this.btnPR_Click);
            // 
            // PictureBox3
            // 
            //this.PictureBox3.Image = global::Laksana_ERPW.Properties.Resources.Logo_Updated;
            this.PictureBox3.Location = new System.Drawing.Point(1147, 2);
            this.PictureBox3.Name = "PictureBox3";
            this.PictureBox3.Size = new System.Drawing.Size(90, 49);
            this.PictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.PictureBox3.TabIndex = 259;
            this.PictureBox3.TabStop = false;
            // 
            // pictureBox1
            // 
            //this.pictureBox1.Image = global::Laksana_ERPW.Properties.Resources.SSP_Logo;
            this.pictureBox1.InitialImage = null;
            this.pictureBox1.Location = new System.Drawing.Point(1, 1);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(76, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 73;
            this.pictureBox1.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.LightBlue;
            this.btnClose.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Image = ((System.Drawing.Image)(resources.GetObject("btnClose.Image")));
            this.btnClose.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnClose.Location = new System.Drawing.Point(1158, 571);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(77, 40);
            this.btnClose.TabIndex = 10;
            this.btnClose.Text = "Close";
            this.btnClose.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.LightBlue;
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnDelete.Location = new System.Drawing.Point(1081, 571);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(77, 40);
            this.btnDelete.TabIndex = 9;
            this.btnDelete.Text = "Delete";
            this.btnDelete.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.LightBlue;
            this.btnSave.ForeColor = System.Drawing.Color.Black;
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnSave.Location = new System.Drawing.Point(850, 571);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(77, 40);
            this.btnSave.TabIndex = 6;
            this.btnSave.Text = "Save";
            this.btnSave.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.LightBlue;
            this.btnClear.ForeColor = System.Drawing.Color.Black;
            this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
            this.btnClear.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.btnClear.Location = new System.Drawing.Point(773, 571);
            this.btnClear.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(77, 40);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "Clear";
            this.btnClear.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // dgPO
            // 
            this.dgPO.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgPO.Location = new System.Drawing.Point(3, 398);
            this.dgPO.Name = "dgPO";
            this.dgPO.Size = new System.Drawing.Size(615, 170);
            this.dgPO.TabIndex = 2;
            // 
            // dgMFg
            // 
            this.dgMFg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgMFg.Location = new System.Drawing.Point(621, 398);
            this.dgMFg.Name = "dgMFg";
            this.dgMFg.Size = new System.Drawing.Size(614, 170);
            this.dgMFg.TabIndex = 4;
            // 
            // btnGenetePOItems
            // 
            this.btnGenetePOItems.Location = new System.Drawing.Point(3, 371);
            this.btnGenetePOItems.Name = "btnGenetePOItems";
            this.btnGenetePOItems.Size = new System.Drawing.Size(217, 26);
            this.btnGenetePOItems.TabIndex = 1;
            this.btnGenetePOItems.Text = "Generate Summery To Procure Qty";
            this.btnGenetePOItems.UseVisualStyleBackColor = true;
            this.btnGenetePOItems.Click += new System.EventHandler(this.btnGenetePOItems_Click);
            // 
            // GenerateMfgItems
            // 
            this.GenerateMfgItems.Location = new System.Drawing.Point(619, 371);
            this.GenerateMfgItems.Name = "GenerateMfgItems";
            this.GenerateMfgItems.Size = new System.Drawing.Size(222, 26);
            this.GenerateMfgItems.TabIndex = 3;
            this.GenerateMfgItems.Text = "Generate Summery To Produce";
            this.GenerateMfgItems.UseVisualStyleBackColor = true;
            this.GenerateMfgItems.Click += new System.EventHandler(this.GenerateMfgItems_Click);
            // 
            // MaterialRequirementPlanning
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.MintCream;
            this.ClientSize = new System.Drawing.Size(1240, 612);
            this.Controls.Add(this.GenerateMfgItems);
            this.Controls.Add(this.btnGenetePOItems);
            this.Controls.Add(this.dgMFg);
            this.Controls.Add(this.dgPO);
            this.Controls.Add(this.btnPR);
            this.Controls.Add(this.txtPordNo);
            this.Controls.Add(this.PictureBox3);
            this.Controls.Add(this.BtnGetOrders);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txttotalQty);
            this.Controls.Add(this.btnPO);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.dgIndent);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.Label1);
            this.Controls.Add(this.GroupBox1);
            this.Font = new System.Drawing.Font("Calibri", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MaterialRequirementPlanning";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.ProductionReport_Load);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgIndent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PictureBox3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgPO)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgMFg)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.GroupBox GroupBox1;
        private System.Windows.Forms.Button btnSearch;
        internal System.Windows.Forms.DateTimePicker dpIndentDate;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.TextBox txtIndentNo;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Button btnClose;
        internal System.Windows.Forms.Button btnDelete;
        internal System.Windows.Forms.Button btnSave;
        internal System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.DataGridView dgIndent;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button BtnGetOrders;
        private System.Windows.Forms.TextBox txtPordNo;
        private System.Windows.Forms.Button btnPO;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txttotalQty;
        internal System.Windows.Forms.PictureBox PictureBox3;
        private System.Windows.Forms.TextBox TxtQty;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnPR;
        private System.Windows.Forms.DataGridView dgPO;
        private System.Windows.Forms.DataGridView dgMFg;
        private System.Windows.Forms.Button btnGenetePOItems;
        private System.Windows.Forms.Button GenerateMfgItems;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProdCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProductName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Product_Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Unit;
        private System.Windows.Forms.DataGridViewTextBoxColumn RequiredQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn StockInQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn ProcureQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn MfgQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Min_Stock;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
    }
}