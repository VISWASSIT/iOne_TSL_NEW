namespace ioneNet.MaterialManagement.Masters
{
    partial class frmOpeningStock
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOpeningStock));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbStorageLocation = new System.Windows.Forms.ComboBox();
            this.chkAllAccounts = new System.Windows.Forms.CheckBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnGetAccounts = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbAccountGroup = new System.Windows.Forms.ComboBox();
            this.brnSearch = new System.Windows.Forms.Button();
            this.AsAtdate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.txtVoucherNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.dgvJournalVouchar = new System.Windows.Forms.DataGridView();
            this.Prod_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Prod_Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Item_Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Prod_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Uom_Descr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Drawing_No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OB_Stock_Wt = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OB_Price = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OB_Value = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.label30 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.label31 = new System.Windows.Forms.Label();
            this.cmdPrint = new System.Windows.Forms.Button();
            this.cmdDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblCreatedBy = new System.Windows.Forms.LinkLabel();
            this.lblModified = new System.Windows.Forms.LinkLabel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label33 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvJournalVouchar)).BeginInit();
            this.tableLayoutPanel5.SuspendLayout();
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
            this.label12.Size = new System.Drawing.Size(173, 25);
            this.label12.TabIndex = 186;
            this.label12.Text = "OPENING STOCKS";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // groupBox3
            // 
            this.groupBox3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.groupBox3.Controls.Add(this.checkBox1);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.cmbStorageLocation);
            this.groupBox3.Controls.Add(this.chkAllAccounts);
            this.groupBox3.Controls.Add(this.btnImport);
            this.groupBox3.Controls.Add(this.btnGetAccounts);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.cmbAccountGroup);
            this.groupBox3.Controls.Add(this.brnSearch);
            this.groupBox3.Controls.Add(this.AsAtdate);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.txtVoucherNo);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Font = new System.Drawing.Font("Calibri", 10F);
            this.groupBox3.ForeColor = System.Drawing.Color.RoyalBlue;
            this.groupBox3.Location = new System.Drawing.Point(15, 48);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.groupBox3.Size = new System.Drawing.Size(1038, 84);
            this.groupBox3.TabIndex = 187;
            this.groupBox3.TabStop = false;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(204, 45);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(161, 25);
            this.checkBox1.TabIndex = 140;
            this.checkBox1.Text = "At Sub Contractor ";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(350, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(163, 18);
            this.label1.TabIndex = 139;
            this.label1.Text = "Storage Location / Yard";
            // 
            // cmbStorageLocation
            // 
            this.cmbStorageLocation.BackColor = System.Drawing.SystemColors.Info;
            this.cmbStorageLocation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStorageLocation.FormattingEnabled = true;
            this.cmbStorageLocation.Location = new System.Drawing.Point(492, 43);
            this.cmbStorageLocation.Name = "cmbStorageLocation";
            this.cmbStorageLocation.Size = new System.Drawing.Size(276, 26);
            this.cmbStorageLocation.TabIndex = 138;
            // 
            // chkAllAccounts
            // 
            this.chkAllAccounts.AutoSize = true;
            this.chkAllAccounts.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAllAccounts.ForeColor = System.Drawing.Color.Blue;
            this.chkAllAccounts.Location = new System.Drawing.Point(670, 16);
            this.chkAllAccounts.Name = "chkAllAccounts";
            this.chkAllAccounts.Size = new System.Drawing.Size(73, 22);
            this.chkAllAccounts.TabIndex = 137;
            this.chkAllAccounts.Text = "Get All";
            this.chkAllAccounts.UseVisualStyleBackColor = true;
            // 
            // btnImport
            // 
            this.btnImport.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnImport.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImport.ForeColor = System.Drawing.Color.Black;
            this.btnImport.Location = new System.Drawing.Point(901, 14);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(104, 28);
            this.btnImport.TabIndex = 136;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = false;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnGetAccounts
            // 
            this.btnGetAccounts.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnGetAccounts.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGetAccounts.ForeColor = System.Drawing.Color.Black;
            this.btnGetAccounts.Location = new System.Drawing.Point(783, 16);
            this.btnGetAccounts.Name = "btnGetAccounts";
            this.btnGetAccounts.Size = new System.Drawing.Size(103, 28);
            this.btnGetAccounts.TabIndex = 135;
            this.btnGetAccounts.Text = "Get Products";
            this.btnGetAccounts.UseVisualStyleBackColor = false;
            this.btnGetAccounts.Click += new System.EventHandler(this.btnGetAccounts_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(400, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(106, 18);
            this.label2.TabIndex = 134;
            this.label2.Text = "Product Group";
            // 
            // cmbAccountGroup
            // 
            this.cmbAccountGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbAccountGroup.FormattingEnabled = true;
            this.cmbAccountGroup.Location = new System.Drawing.Point(492, 14);
            this.cmbAccountGroup.Name = "cmbAccountGroup";
            this.cmbAccountGroup.Size = new System.Drawing.Size(172, 26);
            this.cmbAccountGroup.TabIndex = 133;
            // 
            // brnSearch
            // 
            this.brnSearch.BackColor = System.Drawing.Color.MintCream;
            this.brnSearch.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.brnSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.brnSearch.ForeColor = System.Drawing.Color.Black;
            this.brnSearch.Image = ((System.Drawing.Image)(resources.GetObject("brnSearch.Image")));
            this.brnSearch.Location = new System.Drawing.Point(170, 16);
            this.brnSearch.Name = "brnSearch";
            this.brnSearch.Size = new System.Drawing.Size(25, 24);
            this.brnSearch.TabIndex = 132;
            this.brnSearch.TabStop = false;
            this.brnSearch.UseVisualStyleBackColor = false;
            this.brnSearch.Click += new System.EventHandler(this.brnSearch_Click);
            // 
            // AsAtdate
            // 
            this.AsAtdate.CustomFormat = "dd/MM/yyyy";
            this.AsAtdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AsAtdate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.AsAtdate.Location = new System.Drawing.Point(284, 16);
            this.AsAtdate.Name = "AsAtdate";
            this.AsAtdate.Size = new System.Drawing.Size(104, 24);
            this.AsAtdate.TabIndex = 1;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Black;
            this.label6.Location = new System.Drawing.Point(201, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 18);
            this.label6.TabIndex = 101;
            this.label6.Text = "As On Date";
            // 
            // txtVoucherNo
            // 
            this.txtVoucherNo.BackColor = System.Drawing.SystemColors.Info;
            this.txtVoucherNo.Enabled = false;
            this.txtVoucherNo.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtVoucherNo.Location = new System.Drawing.Point(65, 16);
            this.txtVoucherNo.Margin = new System.Windows.Forms.Padding(2);
            this.txtVoucherNo.Name = "txtVoucherNo";
            this.txtVoucherNo.Size = new System.Drawing.Size(91, 24);
            this.txtVoucherNo.TabIndex = 0;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(13, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 18);
            this.label3.TabIndex = 53;
            this.label3.Text = "Ref No";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.dgvJournalVouchar, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 139);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1041, 428);
            this.tableLayoutPanel1.TabIndex = 192;
            // 
            // dgvJournalVouchar
            // 
            this.dgvJournalVouchar.AllowUserToOrderColumns = true;
            this.dgvJournalVouchar.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Info;
            this.dgvJournalVouchar.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvJournalVouchar.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvJournalVouchar.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.LightSkyBlue;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvJournalVouchar.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvJournalVouchar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvJournalVouchar.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Prod_ID,
            this.Prod_Code,
            this.Item_Code,
            this.Prod_Name,
            this.Uom_Descr,
            this.Drawing_No,
            this.OB_Stock_Wt,
            this.OB_Price,
            this.OB_Value});
            this.dgvJournalVouchar.EnableHeadersVisualStyles = false;
            this.dgvJournalVouchar.Location = new System.Drawing.Point(3, 3);
            this.dgvJournalVouchar.Name = "dgvJournalVouchar";
            this.dgvJournalVouchar.RowHeadersWidth = 51;
            this.dgvJournalVouchar.Size = new System.Drawing.Size(1035, 422);
            this.dgvJournalVouchar.TabIndex = 1;
            this.dgvJournalVouchar.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvJournalVouchar_CellClick);
            this.dgvJournalVouchar.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvJournalVouchar_CellEndEdit);
            this.dgvJournalVouchar.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvJournalVouchar_EditingControlShowing);
            this.dgvJournalVouchar.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgvJournalVouchar_KeyDown);
            // 
            // Prod_ID
            // 
            this.Prod_ID.DataPropertyName = "Prod_ID";
            this.Prod_ID.HeaderText = "ID";
            this.Prod_ID.MinimumWidth = 6;
            this.Prod_ID.Name = "Prod_ID";
            this.Prod_ID.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Prod_ID.Visible = false;
            // 
            // Prod_Code
            // 
            this.Prod_Code.DataPropertyName = "Prod_Code";
            this.Prod_Code.HeaderText = "Prod_ID";
            this.Prod_Code.MinimumWidth = 6;
            this.Prod_Code.Name = "Prod_Code";
            // 
            // Item_Code
            // 
            this.Item_Code.DataPropertyName = "Item_Code";
            this.Item_Code.HeaderText = "Prod_Code";
            this.Item_Code.MinimumWidth = 6;
            this.Item_Code.Name = "Item_Code";
            // 
            // Prod_Name
            // 
            this.Prod_Name.DataPropertyName = "Prod_Name";
            this.Prod_Name.HeaderText = "Prod_Name";
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
            // Drawing_No
            // 
            this.Drawing_No.DataPropertyName = "Drawing_No";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            this.Drawing_No.DefaultCellStyle = dataGridViewCellStyle3;
            this.Drawing_No.HeaderText = "Grade / Make /Drawing ";
            this.Drawing_No.MinimumWidth = 6;
            this.Drawing_No.Name = "Drawing_No";
            // 
            // OB_Stock_Wt
            // 
            this.OB_Stock_Wt.DataPropertyName = "OB_Stock_Wt";
            this.OB_Stock_Wt.HeaderText = "OB_Stock_qty";
            this.OB_Stock_Wt.MinimumWidth = 6;
            this.OB_Stock_Wt.Name = "OB_Stock_Wt";
            // 
            // OB_Price
            // 
            this.OB_Price.DataPropertyName = "OB_Price";
            this.OB_Price.HeaderText = "OB_Price";
            this.OB_Price.MinimumWidth = 6;
            this.OB_Price.Name = "OB_Price";
            // 
            // OB_Value
            // 
            this.OB_Value.DataPropertyName = "OB_Value";
            this.OB_Value.HeaderText = "OB_Value";
            this.OB_Value.MinimumWidth = 6;
            this.OB_Value.Name = "OB_Value";
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel5.ColumnCount = 9;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.388942F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.39848F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.60076F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 21.57795F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.333333F));
            this.tableLayoutPanel5.Controls.Add(this.label30, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnClear, 4, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnSave, 5, 0);
            this.tableLayoutPanel5.Controls.Add(this.label31, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.cmdPrint, 7, 0);
            this.tableLayoutPanel5.Controls.Add(this.cmdDelete, 6, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnClose, 8, 0);
            this.tableLayoutPanel5.Controls.Add(this.lblCreatedBy, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.lblModified, 3, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(12, 600);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(1038, 38);
            this.tableLayoutPanel5.TabIndex = 193;
            // 
            // label30
            // 
            this.label30.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(281, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(85, 38);
            this.label30.TabIndex = 5;
            this.label30.Text = "Modified By";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnClear.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(605, 5);
            this.btnClear.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(80, 28);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSave.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Location = new System.Drawing.Point(691, 5);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(80, 28);
            this.btnSave.TabIndex = 1;
            this.btnSave.Text = "Submit";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // label31
            // 
            this.label31.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(3, 0);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(81, 38);
            this.label31.TabIndex = 3;
            this.label31.Text = "Created By";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmdPrint
            // 
            this.cmdPrint.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdPrint.BackColor = System.Drawing.Color.LightSkyBlue;
            this.cmdPrint.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdPrint.ForeColor = System.Drawing.Color.Black;
            this.cmdPrint.Location = new System.Drawing.Point(863, 3);
            this.cmdPrint.Name = "cmdPrint";
            this.cmdPrint.Size = new System.Drawing.Size(80, 29);
            this.cmdPrint.TabIndex = 130;
            this.cmdPrint.Text = "To Excel";
            this.cmdPrint.UseVisualStyleBackColor = false;
            this.cmdPrint.Click += new System.EventHandler(this.cmdPrint_Click);
            // 
            // cmdDelete
            // 
            this.cmdDelete.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdDelete.BackColor = System.Drawing.Color.LightSkyBlue;
            this.cmdDelete.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdDelete.ForeColor = System.Drawing.Color.Black;
            this.cmdDelete.Location = new System.Drawing.Point(777, 3);
            this.cmdDelete.Name = "cmdDelete";
            this.cmdDelete.Size = new System.Drawing.Size(80, 29);
            this.cmdDelete.TabIndex = 131;
            this.cmdDelete.Text = "Delete";
            this.cmdDelete.UseVisualStyleBackColor = false;
            this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(949, 5);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(86, 28);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblCreatedBy
            // 
            this.lblCreatedBy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCreatedBy.AutoSize = true;
            this.lblCreatedBy.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreatedBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblCreatedBy.Location = new System.Drawing.Point(90, 0);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(185, 38);
            this.lblCreatedBy.TabIndex = 4;
            this.lblCreatedBy.TabStop = true;
            this.lblCreatedBy.Text = "linkLabel8";
            this.lblCreatedBy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblModified
            // 
            this.lblModified.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblModified.AutoSize = true;
            this.lblModified.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModified.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblModified.Location = new System.Drawing.Point(381, 0);
            this.lblModified.Name = "lblModified";
            this.lblModified.Size = new System.Drawing.Size(218, 38);
            this.lblModified.TabIndex = 6;
            this.lblModified.TabStop = true;
            this.lblModified.Text = "linkLabel8";
            this.lblModified.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
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
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.ForeColor = System.Drawing.Color.DarkBlue;
            this.label33.Location = new System.Drawing.Point(12, 570);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(305, 18);
            this.label33.TabIndex = 194;
            this.label33.Text = "F3 - Batch/Roll Wise Details ;F6 - Delete Item";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(660, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 195;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // frmOpeningStock
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1082, 690);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label33);
            this.Controls.Add(this.tableLayoutPanel5);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.pictureBox2);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmOpeningStock";
            this.Text = "frmOpeningStock";
            this.Load += new System.EventHandler(this.frmOpeningStock_Load);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvJournalVouchar)).EndInit();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkAllAccounts;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnGetAccounts;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbAccountGroup;
        private System.Windows.Forms.Button brnSearch;
        private System.Windows.Forms.DateTimePicker AsAtdate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtVoucherNo;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.DataGridView dgvJournalVouchar;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
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
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prod_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prod_Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item_Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prod_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Uom_Descr;
        private System.Windows.Forms.DataGridViewTextBoxColumn Drawing_No;
        private System.Windows.Forms.DataGridViewTextBoxColumn OB_Stock_Wt;
        private System.Windows.Forms.DataGridViewTextBoxColumn OB_Price;
        private System.Windows.Forms.DataGridViewTextBoxColumn OB_Value;
        private System.Windows.Forms.Button button1;
    }
}