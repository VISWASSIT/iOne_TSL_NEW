namespace ioneNet.OrderManagement.Transactions
{
    partial class ListOfInvoices
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
            this.components = new System.ComponentModel.Container();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.modifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printInvoiceWithDigitalSignToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printInvoiceOnPrePrintedFormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printChallanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printLabelsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateEInvoiceJSONToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eMailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accountPostingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.viewSuppliesMadeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.showInvListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ioneDataSet = new ioneNet.ioneDataSet();
            this.showInvListTableAdapter = new ioneNet.ioneDataSetTableAdapters.ShowInvListTableAdapter();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtQRCode = new System.Windows.Forms.TextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.sfDataGrid1 = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            this.label12 = new System.Windows.Forms.Label();
            this.btnAddNew = new Syncfusion.WinForms.Controls.SfButton();
            this.btnImport = new Syncfusion.WinForms.Controls.SfButton();
            this.btnReset = new Syncfusion.WinForms.Controls.SfButton();
            this.btnClose = new Syncfusion.WinForms.Controls.SfButton();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.showInvListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ioneDataSet)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sfDataGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.modifyToolStripMenuItem,
            this.printToolStripMenuItem,
            this.printInvoiceWithDigitalSignToolStripMenuItem,
            this.printInvoiceOnPrePrintedFormToolStripMenuItem,
            this.printChallanToolStripMenuItem,
            this.printLabelsToolStripMenuItem,
            this.generateEInvoiceJSONToolStripMenuItem,
            this.eMailToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.accountPostingToolStripMenuItem,
            this.toolStripMenuItem1,
            this.viewSuppliesMadeToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(305, 340);
            // 
            // modifyToolStripMenuItem
            // 
            this.modifyToolStripMenuItem.Image = global::ioneNet.Properties.Resources.Edit_16x16;
            this.modifyToolStripMenuItem.Name = "modifyToolStripMenuItem";
            this.modifyToolStripMenuItem.Size = new System.Drawing.Size(304, 30);
            this.modifyToolStripMenuItem.Text = "Modify";
            this.modifyToolStripMenuItem.Click += new System.EventHandler(this.modifyToolStripMenuItem_Click);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Image = global::ioneNet.Properties.Resources.Print;
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size(304, 30);
            this.printToolStripMenuItem.Text = "Print Invoice";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.printToolStripMenuItem_Click);
            // 
            // printInvoiceWithDigitalSignToolStripMenuItem
            // 
            this.printInvoiceWithDigitalSignToolStripMenuItem.Image = global::ioneNet.Properties.Resources.Print_preview;
            this.printInvoiceWithDigitalSignToolStripMenuItem.Name = "printInvoiceWithDigitalSignToolStripMenuItem";
            this.printInvoiceWithDigitalSignToolStripMenuItem.Size = new System.Drawing.Size(304, 30);
            this.printInvoiceWithDigitalSignToolStripMenuItem.Text = "Print Invoice With Digital Sign";
            this.printInvoiceWithDigitalSignToolStripMenuItem.Click += new System.EventHandler(this.printInvoiceWithDigitalSignToolStripMenuItem_Click);
            // 
            // printInvoiceOnPrePrintedFormToolStripMenuItem
            // 
            this.printInvoiceOnPrePrintedFormToolStripMenuItem.Name = "printInvoiceOnPrePrintedFormToolStripMenuItem";
            this.printInvoiceOnPrePrintedFormToolStripMenuItem.Size = new System.Drawing.Size(304, 30);
            this.printInvoiceOnPrePrintedFormToolStripMenuItem.Text = "Print Invoice on Pre-Printed Form";
            this.printInvoiceOnPrePrintedFormToolStripMenuItem.Click += new System.EventHandler(this.printInvoiceOnPrePrintedFormToolStripMenuItem_Click);
            // 
            // printChallanToolStripMenuItem
            // 
            this.printChallanToolStripMenuItem.Name = "printChallanToolStripMenuItem";
            this.printChallanToolStripMenuItem.Size = new System.Drawing.Size(304, 30);
            this.printChallanToolStripMenuItem.Text = "Print Challan";
            this.printChallanToolStripMenuItem.Click += new System.EventHandler(this.printChallanToolStripMenuItem_Click);
            // 
            // printLabelsToolStripMenuItem
            // 
            this.printLabelsToolStripMenuItem.Name = "printLabelsToolStripMenuItem";
            this.printLabelsToolStripMenuItem.Size = new System.Drawing.Size(304, 30);
            this.printLabelsToolStripMenuItem.Text = "Print Labels";
            this.printLabelsToolStripMenuItem.Click += new System.EventHandler(this.printLabelsToolStripMenuItem_Click);
            // 
            // generateEInvoiceJSONToolStripMenuItem
            // 
            this.generateEInvoiceJSONToolStripMenuItem.Name = "generateEInvoiceJSONToolStripMenuItem";
            this.generateEInvoiceJSONToolStripMenuItem.Size = new System.Drawing.Size(304, 30);
            this.generateEInvoiceJSONToolStripMenuItem.Text = "Generate E Invoice";
            this.generateEInvoiceJSONToolStripMenuItem.Click += new System.EventHandler(this.generateEInvoiceJSONToolStripMenuItem_Click);
            // 
            // eMailToolStripMenuItem
            // 
            this.eMailToolStripMenuItem.Image = global::ioneNet.Properties.Resources.Mail;
            this.eMailToolStripMenuItem.Name = "eMailToolStripMenuItem";
            this.eMailToolStripMenuItem.Size = new System.Drawing.Size(304, 30);
            this.eMailToolStripMenuItem.Text = "E Mail";
            this.eMailToolStripMenuItem.Click += new System.EventHandler(this.eMailToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Image = global::ioneNet.Properties.Resources.Erase;
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(304, 30);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // accountPostingToolStripMenuItem
            // 
            this.accountPostingToolStripMenuItem.Image = global::ioneNet.Properties.Resources.Undo;
            this.accountPostingToolStripMenuItem.Name = "accountPostingToolStripMenuItem";
            this.accountPostingToolStripMenuItem.Size = new System.Drawing.Size(304, 30);
            this.accountPostingToolStripMenuItem.Text = "Cancel";
            this.accountPostingToolStripMenuItem.Click += new System.EventHandler(this.accountPostingToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(301, 6);
            // 
            // viewSuppliesMadeToolStripMenuItem
            // 
            this.viewSuppliesMadeToolStripMenuItem.Name = "viewSuppliesMadeToolStripMenuItem";
            this.viewSuppliesMadeToolStripMenuItem.Size = new System.Drawing.Size(304, 30);
            this.viewSuppliesMadeToolStripMenuItem.Text = "View Deailed Report";
            // 
            // showInvListBindingSource
            // 
            this.showInvListBindingSource.DataMember = "ShowInvList";
            this.showInvListBindingSource.DataSource = this.ioneDataSet;
            // 
            // ioneDataSet
            // 
            this.ioneDataSet.DataSetName = "ioneDataSet";
            this.ioneDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // showInvListTableAdapter
            // 
            this.showInvListTableAdapter.ClearBeforeFill = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtQRCode);
            this.groupBox1.Controls.Add(this.pictureBox1);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.textBox1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.comboBox1);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(336, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(326, 158);
            this.groupBox1.TabIndex = 24;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Print Inv With Digital Sign";
            this.groupBox1.Visible = false;
            // 
            // txtQRCode
            // 
            this.txtQRCode.Location = new System.Drawing.Point(77, 125);
            this.txtQRCode.Name = "txtQRCode";
            this.txtQRCode.Size = new System.Drawing.Size(100, 27);
            this.txtQRCode.TabIndex = 40;
            this.txtQRCode.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(24, 114);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(47, 38);
            this.pictureBox1.TabIndex = 39;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Visible = false;
            // 
            // button2
            // 
            this.button2.BackColor = System.Drawing.Color.White;
            this.button2.Image = global::ioneNet.Properties.Resources.Close;
            this.button2.Location = new System.Drawing.Point(227, 108);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 31);
            this.button2.TabIndex = 5;
            this.button2.Text = "Close";
            this.button2.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button2.UseVisualStyleBackColor = false;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.Color.White;
            this.button1.Image = global::ioneNet.Properties.Resources.Print;
            this.button1.Location = new System.Drawing.Point(146, 108);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 31);
            this.button1.TabIndex = 4;
            this.button1.Text = "Print";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(87, 79);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(45, 27);
            this.textBox1.TabIndex = 3;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 82);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Copy No";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Original for Receipent",
            "Duplicate for Transporter / Supplier",
            "Triplicate for Supplier"});
            this.comboBox1.Location = new System.Drawing.Point(87, 50);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(197, 28);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            this.comboBox1.Leave += new System.EventHandler(this.comboBox1_Leave);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Copy ";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 8;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.805806F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.11612F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.00103F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.00103F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.00103F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.00103F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.00103F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.93919F));
            this.tableLayoutPanel2.Controls.Add(this.pictureBox2, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.sfDataGrid1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label12, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnAddNew, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnImport, 5, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnReset, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnClose, 7, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 8;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 8.061749F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 3.773585F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.37907F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.37907F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.03602F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.40823F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.43739F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.80618F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1009, 606);
            this.tableLayoutPanel2.TabIndex = 199;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ioneNet.Properties.Resources.logoface_Ione;
            this.pictureBox2.Location = new System.Drawing.Point(3, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(52, 40);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 191;
            this.pictureBox2.TabStop = false;
            // 
            // sfDataGrid1
            // 
            this.sfDataGrid1.AccessibleName = "Table";
            this.sfDataGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sfDataGrid1.AutoSizeColumnsMode = Syncfusion.WinForms.DataGrid.Enums.AutoSizeColumnsMode.AllCells;
            this.tableLayoutPanel2.SetColumnSpan(this.sfDataGrid1, 8);
            this.sfDataGrid1.ContextMenuStrip = this.contextMenuStrip1;
            this.sfDataGrid1.Location = new System.Drawing.Point(3, 79);
            this.sfDataGrid1.Name = "sfDataGrid1";
            this.sfDataGrid1.PreviewRowHeight = 35;
            this.tableLayoutPanel2.SetRowSpan(this.sfDataGrid1, 6);
            this.sfDataGrid1.Size = new System.Drawing.Size(1003, 524);
            this.sfDataGrid1.Style.CellStyle.BackColor = System.Drawing.Color.WhiteSmoke;
            this.sfDataGrid1.Style.CellStyle.Font.Facename = "Segoe UI";
            this.sfDataGrid1.Style.CellStyle.Font.Size = 10F;
            this.sfDataGrid1.Style.HeaderStyle.BackColor = System.Drawing.Color.LightBlue;
            this.sfDataGrid1.Style.HeaderStyle.Font.Facename = "Segoe UI";
            this.sfDataGrid1.Style.HeaderStyle.Font.Size = 10F;
            this.sfDataGrid1.TabIndex = 195;
            this.sfDataGrid1.Text = "sfDataGrid1";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tableLayoutPanel2.SetColumnSpan(this.label12, 3);
            this.label12.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.DarkBlue;
            this.label12.Location = new System.Drawing.Point(67, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(416, 52);
            this.label12.TabIndex = 190;
            this.label12.Text = "MANAGE INVOICES";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnAddNew
            // 
            this.btnAddNew.AccessibleName = "Button";
            this.btnAddNew.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddNew.BackColor = System.Drawing.Color.White;
            this.btnAddNew.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddNew.Location = new System.Drawing.Point(489, 3);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(116, 46);
            this.btnAddNew.Style.BackColor = System.Drawing.Color.White;
            this.btnAddNew.Style.Image = global::ioneNet.Properties.Resources.Add;
            this.btnAddNew.TabIndex = 0;
            this.btnAddNew.Text = "Add New";
            this.btnAddNew.UseVisualStyleBackColor = false;
            this.btnAddNew.Click += new System.EventHandler(this.btnAddNew_Click);
            // 
            // btnImport
            // 
            this.btnImport.AccessibleName = "Button";
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.BackColor = System.Drawing.Color.White;
            this.btnImport.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImport.Location = new System.Drawing.Point(611, 3);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(116, 46);
            this.btnImport.Style.BackColor = System.Drawing.Color.White;
            this.btnImport.Style.Image = global::ioneNet.Properties.Resources.Import_Picture_Document_icon;
            this.btnImport.TabIndex = 193;
            this.btnImport.Text = "Export";
            this.btnImport.UseVisualStyleBackColor = false;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnReset
            // 
            this.btnReset.AccessibleName = "Button";
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.BackColor = System.Drawing.Color.White;
            this.btnReset.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.Location = new System.Drawing.Point(733, 3);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(116, 46);
            this.btnReset.Style.BackColor = System.Drawing.Color.White;
            this.btnReset.Style.Image = global::ioneNet.Properties.Resources.Refresh;
            this.btnReset.TabIndex = 1;
            this.btnReset.Text = "Refresh";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnClose
            // 
            this.btnClose.AccessibleName = "Button";
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(855, 3);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(151, 46);
            this.btnClose.Style.BackColor = System.Drawing.Color.White;
            this.btnClose.Style.Image = global::ioneNet.Properties.Resources.Exit;
            this.btnClose.TabIndex = 2;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // ListOfInvoices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(1023, 620);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ListOfInvoices";
            this.Text = "Invoices List";
            this.Load += new System.EventHandler(this.ListOfOrders_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.showInvListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ioneDataSet)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sfDataGrid1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem modifyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accountPostingToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem viewSuppliesMadeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eMailToolStripMenuItem;
        private System.Windows.Forms.BindingSource showInvListBindingSource;
        private ioneDataSet ioneDataSet;
        private ioneDataSetTableAdapters.ShowInvListTableAdapter showInvListTableAdapter;
        private System.Windows.Forms.ToolStripMenuItem printChallanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printLabelsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printInvoiceOnPrePrintedFormToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printInvoiceWithDigitalSignToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolStripMenuItem generateEInvoiceJSONToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private Syncfusion.WinForms.DataGrid.SfDataGrid sfDataGrid1;
        private System.Windows.Forms.Label label12;
        private Syncfusion.WinForms.Controls.SfButton btnAddNew;
        private Syncfusion.WinForms.Controls.SfButton btnImport;
        private Syncfusion.WinForms.Controls.SfButton btnReset;
        private Syncfusion.WinForms.Controls.SfButton btnClose;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.TextBox txtQRCode;
    }
}