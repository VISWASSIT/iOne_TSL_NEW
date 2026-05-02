namespace ioneNet.MaterialManagement.Masters
{
    partial class frmOpeningStock_FG
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOpeningStock_FG));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle14 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle15 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle16 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label12 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbStorageLocation = new System.Windows.Forms.ComboBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnGetAccounts = new System.Windows.Forms.Button();
            this.brnSearch = new System.Windows.Forms.Button();
            this.AsAtdate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.txtVoucherNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label30 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.lblModified = new System.Windows.Forms.LinkLabel();
            this.lblCreatedBy = new System.Windows.Forms.LinkLabel();
            this.dgvJournalVouchar = new System.Windows.Forms.DataGridView();
            this.Prod_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Prod_Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Prod_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Uom_Descr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Prod_Unit_Wt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Prod_Grade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Prod_length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OB_Stock_qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OB_Stock_Wt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OB_Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OB_Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbProdGroup = new System.Windows.Forms.ComboBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.cmdPrint = new System.Windows.Forms.Button();
            this.cmdDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmbCustomer = new System.Windows.Forms.ComboBox();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJournalVouchar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.DarkBlue;
            this.label12.Location = new System.Drawing.Point(62, 21);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(215, 25);
            this.label12.TabIndex = 186;
            this.label12.Text = "OPENING STOCKS - FG";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(483, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 36);
            this.label1.TabIndex = 139;
            this.label1.Text = "Stage";
            // 
            // cmbStorageLocation
            // 
            this.cmbStorageLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbStorageLocation.BackColor = System.Drawing.SystemColors.Info;
            this.cmbStorageLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStorageLocation.FormattingEnabled = true;
            this.cmbStorageLocation.Items.AddRange(new object[] {
            "Cutting",
            "Straightening",
            "Finished"});
            this.cmbStorageLocation.Location = new System.Drawing.Point(579, 3);
            this.cmbStorageLocation.Name = "cmbStorageLocation";
            this.cmbStorageLocation.Size = new System.Drawing.Size(90, 26);
            this.cmbStorageLocation.TabIndex = 138;
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImport.ForeColor = System.Drawing.Color.Black;
            this.btnImport.Image = global::ioneNet.Properties.Resources.Import_Picture_Document_icon;
            this.btnImport.Location = new System.Drawing.Point(963, 3);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(92, 30);
            this.btnImport.TabIndex = 136;
            this.btnImport.Text = "Import";
            this.btnImport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnGetAccounts
            // 
            this.btnGetAccounts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGetAccounts.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetAccounts.ForeColor = System.Drawing.Color.Black;
            this.btnGetAccounts.Image = global::ioneNet.Properties.Resources.Search_16x16;
            this.btnGetAccounts.Location = new System.Drawing.Point(867, 3);
            this.btnGetAccounts.Name = "btnGetAccounts";
            this.btnGetAccounts.Size = new System.Drawing.Size(90, 30);
            this.btnGetAccounts.TabIndex = 135;
            this.btnGetAccounts.Text = "Get Products";
            this.btnGetAccounts.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnGetAccounts.UseVisualStyleBackColor = true;
            this.btnGetAccounts.Click += new System.EventHandler(this.btnGetAccounts_Click);
            // 
            // brnSearch
            // 
            this.brnSearch.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.brnSearch.BackColor = System.Drawing.Color.MintCream;
            this.brnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.brnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.brnSearch.ForeColor = System.Drawing.Color.Black;
            this.brnSearch.Image = ((System.Drawing.Image)(resources.GetObject("brnSearch.Image")));
            this.brnSearch.Location = new System.Drawing.Point(195, 3);
            this.brnSearch.Name = "brnSearch";
            this.brnSearch.Size = new System.Drawing.Size(90, 30);
            this.brnSearch.TabIndex = 132;
            this.brnSearch.TabStop = false;
            this.brnSearch.UseVisualStyleBackColor = false;
            this.brnSearch.Click += new System.EventHandler(this.brnSearch_Click);
            // 
            // AsAtdate
            // 
            this.AsAtdate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AsAtdate.CustomFormat = "dd/MM/yyyy";
            this.AsAtdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AsAtdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.AsAtdate.Location = new System.Drawing.Point(387, 3);
            this.AsAtdate.Name = "AsAtdate";
            this.AsAtdate.Size = new System.Drawing.Size(90, 24);
            this.AsAtdate.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(291, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 36);
            this.label6.TabIndex = 101;
            this.label6.Text = "As On Date";
            // 
            // txtVoucherNo
            // 
            this.txtVoucherNo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtVoucherNo.BackColor = System.Drawing.SystemColors.Info;
            this.txtVoucherNo.Enabled = false;
            this.txtVoucherNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVoucherNo.Location = new System.Drawing.Point(98, 2);
            this.txtVoucherNo.Margin = new System.Windows.Forms.Padding(2);
            this.txtVoucherNo.Name = "txtVoucherNo";
            this.txtVoucherNo.Size = new System.Drawing.Size(92, 24);
            this.txtVoucherNo.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 36);
            this.label3.TabIndex = 53;
            this.label3.Text = "Ref No";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 11;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090908F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090908F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090908F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090908F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090908F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090908F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090908F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090908F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090908F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090908F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.090908F));
            this.tableLayoutPanel1.Controls.Add(this.label30, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.label33, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.label31, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnImport, 10, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblModified, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.lblCreatedBy, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.dgvJournalVouchar, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtVoucherNo, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.brnSearch, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.AsAtdate, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.label6, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.label1, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbStorageLocation, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbProdGroup, 8, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnClose, 10, 4);
            this.tableLayoutPanel1.Controls.Add(this.cmdPrint, 9, 4);
            this.tableLayoutPanel1.Controls.Add(this.cmdDelete, 8, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnSave, 7, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnClear, 6, 4);
            this.tableLayoutPanel1.Controls.Add(this.btnGetAccounts, 9, 0);
            this.tableLayoutPanel1.Controls.Add(this.checkBox1, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.label4, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.cmbCustomer, 6, 1);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 69);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.91133F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.926108F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 75.36946F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 4.926108F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.702791F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1058, 609);
            this.tableLayoutPanel1.TabIndex = 192;
            // 
            // label30
            // 
            this.label30.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(195, 555);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(85, 54);
            this.label30.TabIndex = 5;
            this.label30.Text = "Modified By";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label33, 4);
            this.label33.ForeColor = System.Drawing.Color.DarkBlue;
            this.label33.Location = new System.Drawing.Point(3, 525);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(300, 18);
            this.label33.TabIndex = 194;
            this.label33.Text = "F3 - Batch/Lot Wise Details ;F6 - Delete Item";
            // 
            // label31
            // 
            this.label31.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(3, 555);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(81, 54);
            this.label31.TabIndex = 3;
            this.label31.Text = "Created By";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblModified
            // 
            this.lblModified.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblModified.AutoSize = true;
            this.lblModified.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModified.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblModified.Location = new System.Drawing.Point(291, 555);
            this.lblModified.Name = "lblModified";
            this.lblModified.Size = new System.Drawing.Size(90, 54);
            this.lblModified.TabIndex = 6;
            this.lblModified.TabStop = true;
            this.lblModified.Text = "linkLabel8";
            this.lblModified.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCreatedBy
            // 
            this.lblCreatedBy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCreatedBy.AutoSize = true;
            this.lblCreatedBy.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreatedBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblCreatedBy.Location = new System.Drawing.Point(99, 555);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(90, 54);
            this.lblCreatedBy.TabIndex = 4;
            this.lblCreatedBy.TabStop = true;
            this.lblCreatedBy.Text = "linkLabel8";
            this.lblCreatedBy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // dgvJournalVouchar
            // 
            this.dgvJournalVouchar.AllowUserToOrderColumns = true;
            this.dgvJournalVouchar.AllowUserToResizeRows = false;
            dataGridViewCellStyle13.BackColor = System.Drawing.SystemColors.Info;
            this.dgvJournalVouchar.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle13;
            this.dgvJournalVouchar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvJournalVouchar.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = System.Drawing.Color.LightSkyBlue;
            dataGridViewCellStyle14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle14.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle14.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvJournalVouchar.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle14;
            this.dgvJournalVouchar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvJournalVouchar.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Prod_ID,
            this.Prod_Code,
            this.Prod_Name,
            this.Uom_Descr,
            this.Prod_Unit_Wt,
            this.Prod_Grade,
            this.Prod_length,
            this.OB_Stock_qty,
            this.OB_Stock_Wt,
            this.OB_Price,
            this.OB_Value});
            this.tableLayoutPanel1.SetColumnSpan(this.dgvJournalVouchar, 11);
            this.dgvJournalVouchar.EnableHeadersVisualStyles = false;
            this.dgvJournalVouchar.Location = new System.Drawing.Point(3, 69);
            this.dgvJournalVouchar.Name = "dgvJournalVouchar";
            this.dgvJournalVouchar.RowHeadersWidth = 51;
            this.dgvJournalVouchar.Size = new System.Drawing.Size(1052, 453);
            this.dgvJournalVouchar.TabIndex = 1;
            this.dgvJournalVouchar.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvJournalVouchar_CellClick);
            this.dgvJournalVouchar.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvJournalVouchar_CellEndEdit);
            this.dgvJournalVouchar.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvJournalVouchar_EditingControlShowing);
            this.dgvJournalVouchar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvJournalVouchar_KeyDown);
            // 
            // Prod_ID
            // 
            this.Prod_ID.DataPropertyName = "Prod_ID";
            this.Prod_ID.HeaderText = "Product ID";
            this.Prod_ID.MinimumWidth = 6;
            this.Prod_ID.Name = "Prod_ID";
            this.Prod_ID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Prod_Code
            // 
            this.Prod_Code.DataPropertyName = "Prod_Code";
            this.Prod_Code.HeaderText = "Prod_Code";
            this.Prod_Code.MinimumWidth = 6;
            this.Prod_Code.Name = "Prod_Code";
            // 
            // Prod_Name
            // 
            this.Prod_Name.DataPropertyName = "Prod_Name";
            this.Prod_Name.HeaderText = "Item Description";
            this.Prod_Name.MinimumWidth = 6;
            this.Prod_Name.Name = "Prod_Name";
            // 
            // Uom_Descr
            // 
            this.Uom_Descr.DataPropertyName = "Uom_Descr";
            this.Uom_Descr.HeaderText = "UOM";
            this.Uom_Descr.MinimumWidth = 6;
            this.Uom_Descr.Name = "Uom_Descr";
            // 
            // Prod_Unit_Wt
            // 
            this.Prod_Unit_Wt.DataPropertyName = "Prod_Unit_Wt";
            dataGridViewCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Prod_Unit_Wt.DefaultCellStyle = dataGridViewCellStyle15;
            this.Prod_Unit_Wt.HeaderText = "Unit Wt";
            this.Prod_Unit_Wt.MinimumWidth = 6;
            this.Prod_Unit_Wt.Name = "Prod_Unit_Wt";
            // 
            // Prod_Grade
            // 
            this.Prod_Grade.DataPropertyName = "Prod_Grade";
            dataGridViewCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Prod_Grade.DefaultCellStyle = dataGridViewCellStyle16;
            this.Prod_Grade.HeaderText = "Item GRADE";
            this.Prod_Grade.MinimumWidth = 6;
            this.Prod_Grade.Name = "Prod_Grade";
            // 
            // Prod_length
            // 
            this.Prod_length.DataPropertyName = "Prod_length";
            this.Prod_length.HeaderText = "Length";
            this.Prod_length.MinimumWidth = 6;
            this.Prod_length.Name = "Prod_length";
            // 
            // OB_Stock_qty
            // 
            this.OB_Stock_qty.DataPropertyName = "OB_Stock_qty";
            this.OB_Stock_qty.HeaderText = "Qty in Nos";
            this.OB_Stock_qty.MinimumWidth = 6;
            this.OB_Stock_qty.Name = "OB_Stock_qty";
            // 
            // OB_Stock_Wt
            // 
            this.OB_Stock_Wt.DataPropertyName = "OB_Stock_Wt";
            this.OB_Stock_Wt.HeaderText = "Qty in Wt";
            this.OB_Stock_Wt.MinimumWidth = 6;
            this.OB_Stock_Wt.Name = "OB_Stock_Wt";
            // 
            // OB_Price
            // 
            this.OB_Price.DataPropertyName = "OB_Price";
            this.OB_Price.HeaderText = "Price";
            this.OB_Price.MinimumWidth = 6;
            this.OB_Price.Name = "OB_Price";
            // 
            // OB_Value
            // 
            this.OB_Value.DataPropertyName = "OB_Value";
            this.OB_Value.HeaderText = "Ob Value";
            this.OB_Value.MinimumWidth = 6;
            this.OB_Value.Name = "OB_Value";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(675, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 36);
            this.label2.TabIndex = 195;
            this.label2.Text = "Product Group";
            // 
            // cmbProdGroup
            // 
            this.cmbProdGroup.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbProdGroup.FormattingEnabled = true;
            this.cmbProdGroup.Location = new System.Drawing.Point(771, 3);
            this.cmbProdGroup.Name = "cmbProdGroup";
            this.cmbProdGroup.Size = new System.Drawing.Size(90, 26);
            this.cmbProdGroup.TabIndex = 196;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Image = global::ioneNet.Properties.Resources.Exit;
            this.btnClose.Location = new System.Drawing.Point(963, 560);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(92, 44);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // cmdPrint
            // 
            this.cmdPrint.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdPrint.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.cmdPrint.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdPrint.ForeColor = System.Drawing.Color.Black;
            this.cmdPrint.Location = new System.Drawing.Point(867, 558);
            this.cmdPrint.Name = "cmdPrint";
            this.cmdPrint.Size = new System.Drawing.Size(90, 48);
            this.cmdPrint.TabIndex = 130;
            this.cmdPrint.Text = "To Excel";
            this.cmdPrint.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cmdPrint.UseVisualStyleBackColor = false;
            this.cmdPrint.Click += new System.EventHandler(this.cmdPrint_Click);
            // 
            // cmdDelete
            // 
            this.cmdDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDelete.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.cmdDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdDelete.ForeColor = System.Drawing.Color.Black;
            this.cmdDelete.Image = global::ioneNet.Properties.Resources.Delete;
            this.cmdDelete.Location = new System.Drawing.Point(771, 558);
            this.cmdDelete.Name = "cmdDelete";
            this.cmdDelete.Size = new System.Drawing.Size(90, 48);
            this.cmdDelete.TabIndex = 131;
            this.cmdDelete.Text = "Delete";
            this.cmdDelete.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.cmdDelete.UseVisualStyleBackColor = false;
            this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Image = global::ioneNet.Properties.Resources.Save_16x16;
            this.btnSave.Location = new System.Drawing.Point(675, 560);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(90, 44);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Submit";
            this.btnSave.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Image = global::ioneNet.Properties.Resources.Refresh_16x16;
            this.btnClear.Location = new System.Drawing.Point(579, 560);
            this.btnClear.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(90, 44);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear";
            this.btnClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ioneNet.Properties.Resources.logoface_Ione;
            this.pictureBox2.Location = new System.Drawing.Point(3, -1);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(53, 42);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 185;
            this.pictureBox2.TabStop = false;
            // 
            // checkBox1
            // 
            this.checkBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkBox1.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.checkBox1, 2);
            this.checkBox1.Location = new System.Drawing.Point(291, 39);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(186, 24);
            this.checkBox1.TabIndex = 197;
            this.checkBox1.Text = "Conversion Mtrl?";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(483, 36);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(78, 30);
            this.label4.TabIndex = 198;
            this.label4.Text = "Customer Name";
            // 
            // cmbCustomer
            // 
            this.cmbCustomer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.cmbCustomer, 4);
            this.cmbCustomer.FormattingEnabled = true;
            this.cmbCustomer.Location = new System.Drawing.Point(579, 39);
            this.cmbCustomer.Name = "cmbCustomer";
            this.cmbCustomer.Size = new System.Drawing.Size(378, 26);
            this.cmbCustomer.TabIndex = 199;
            // 
            // frmOpeningStock_FG
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1082, 690);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.pictureBox2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmOpeningStock_FG";
            this.Text = "frmOpeningStock";
            this.Load += new System.EventHandler(this.frmOpeningStock_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJournalVouchar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnGetAccounts;
        private System.Windows.Forms.Button brnSearch;
        private System.Windows.Forms.DateTimePicker AsAtdate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtVoucherNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dgvJournalVouchar;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label31;
        internal System.Windows.Forms.Button cmdPrint;
        internal System.Windows.Forms.Button cmdDelete;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.LinkLabel lblCreatedBy;
        private System.Windows.Forms.LinkLabel lblModified;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbStorageLocation;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbProdGroup;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prod_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prod_Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prod_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Uom_Descr;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prod_Unit_Wt;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prod_Grade;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prod_length;
        private System.Windows.Forms.DataGridViewTextBoxColumn OB_Stock_qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn OB_Stock_Wt;
        private System.Windows.Forms.DataGridViewTextBoxColumn OB_Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn OB_Value;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cmbCustomer;
    }
}