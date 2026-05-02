namespace ioneNet.MaterialManagement
{
    partial class ListOfSupplier
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
            this.Label2 = new System.Windows.Forms.Label();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.modifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deactivateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.updateOpeningStockToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
            this.purchaseOrderDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.purchaseDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.orderDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.salesDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stockLedgerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.btnClose = new Syncfusion.WinForms.Controls.SfButton();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.dgvCustomers = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            this.label12 = new System.Windows.Forms.Label();
            this.btnReset = new Syncfusion.WinForms.Controls.SfButton();
            this.sfButton1 = new Syncfusion.WinForms.Controls.SfButton();
            this.btnAddNew = new Syncfusion.WinForms.Controls.SfButton();
            this.btnImport = new Syncfusion.WinForms.Controls.SfButton();
            this.sfButton2 = new Syncfusion.WinForms.Controls.SfButton();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GroupBox1.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).BeginInit();
            this.SuspendLayout();
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(492, -115);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(201, 20);
            this.Label2.TabIndex = 7;
            this.Label2.Text = "MANAGE COMPANIES";
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.comboBox1);
            this.GroupBox1.Controls.Add(this.Label1);
            this.GroupBox1.Location = new System.Drawing.Point(-391, -92);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(1114, 51);
            this.GroupBox1.TabIndex = 5;
            this.GroupBox1.TabStop = false;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            " Company ID",
            "Company Name",
            "Group Company",
            "City",
            "TIN No",
            "Status"});
            this.comboBox1.Location = new System.Drawing.Point(97, 13);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(140, 28);
            this.comboBox1.TabIndex = 9;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(5, 18);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(104, 20);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Filter / Search";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem,
            this.modifyToolStripMenuItem,
            this.deactivateToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.toolStripMenuItem1,
            this.updateOpeningStockToolStripMenuItem,
            this.toolStripMenuItem3,
            this.purchaseOrderDataToolStripMenuItem,
            this.purchaseDataToolStripMenuItem,
            this.orderDataToolStripMenuItem,
            this.salesDataToolStripMenuItem,
            this.stockLedgerToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(245, 284);
            // 
            // modifyToolStripMenuItem
            // 
            this.modifyToolStripMenuItem.Name = "modifyToolStripMenuItem";
            this.modifyToolStripMenuItem.Size = new System.Drawing.Size(244, 24);
            this.modifyToolStripMenuItem.Text = "Modify";
            this.modifyToolStripMenuItem.Click += new System.EventHandler(this.modifyToolStripMenuItem_Click);
            // 
            // deactivateToolStripMenuItem
            // 
            this.deactivateToolStripMenuItem.Name = "deactivateToolStripMenuItem";
            this.deactivateToolStripMenuItem.Size = new System.Drawing.Size(244, 24);
            this.deactivateToolStripMenuItem.Text = "Deactivate";
            this.deactivateToolStripMenuItem.Click += new System.EventHandler(this.deactivateToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(244, 24);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(241, 6);
            // 
            // updateOpeningStockToolStripMenuItem
            // 
            this.updateOpeningStockToolStripMenuItem.Name = "updateOpeningStockToolStripMenuItem";
            this.updateOpeningStockToolStripMenuItem.Size = new System.Drawing.Size(244, 24);
            this.updateOpeningStockToolStripMenuItem.Text = "Update Opening Balance";
            // 
            // toolStripMenuItem3
            // 
            this.toolStripMenuItem3.Name = "toolStripMenuItem3";
            this.toolStripMenuItem3.Size = new System.Drawing.Size(241, 6);
            // 
            // purchaseOrderDataToolStripMenuItem
            // 
            this.purchaseOrderDataToolStripMenuItem.Name = "purchaseOrderDataToolStripMenuItem";
            this.purchaseOrderDataToolStripMenuItem.Size = new System.Drawing.Size(244, 24);
            this.purchaseOrderDataToolStripMenuItem.Text = "Purchase Order Data";
            // 
            // purchaseDataToolStripMenuItem
            // 
            this.purchaseDataToolStripMenuItem.Name = "purchaseDataToolStripMenuItem";
            this.purchaseDataToolStripMenuItem.Size = new System.Drawing.Size(244, 24);
            this.purchaseDataToolStripMenuItem.Text = "Purchase Data";
            // 
            // orderDataToolStripMenuItem
            // 
            this.orderDataToolStripMenuItem.Name = "orderDataToolStripMenuItem";
            this.orderDataToolStripMenuItem.Size = new System.Drawing.Size(244, 24);
            this.orderDataToolStripMenuItem.Text = "Sale Order Data";
            // 
            // salesDataToolStripMenuItem
            // 
            this.salesDataToolStripMenuItem.Name = "salesDataToolStripMenuItem";
            this.salesDataToolStripMenuItem.Size = new System.Drawing.Size(244, 24);
            this.salesDataToolStripMenuItem.Text = "Sales Data";
            // 
            // stockLedgerToolStripMenuItem
            // 
            this.stockLedgerToolStripMenuItem.Name = "stockLedgerToolStripMenuItem";
            this.stockLedgerToolStripMenuItem.Size = new System.Drawing.Size(244, 24);
            this.stockLedgerToolStripMenuItem.Text = "Account Ledger";
            // 
            // btnClose
            // 
            this.btnClose.AccessibleName = "Button";
            this.btnClose.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnClose.Location = new System.Drawing.Point(1074, 29);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(85, 28);
            this.btnClose.Style.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnClose.TabIndex = 196;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
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
            this.tableLayoutPanel2.Controls.Add(this.dgvCustomers, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.label12, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnReset, 6, 0);
            this.tableLayoutPanel2.Controls.Add(this.sfButton1, 7, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnAddNew, 3, 0);
            this.tableLayoutPanel2.Controls.Add(this.btnImport, 4, 0);
            this.tableLayoutPanel2.Controls.Add(this.sfButton2, 5, 0);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(12, 12);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 8;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.543624F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 2.684564F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 22.98658F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.37907F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 13.03602F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 14.40823F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 15.43739F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.80618F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(999, 596);
            this.tableLayoutPanel2.TabIndex = 198;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ioneNet.Properties.Resources.logoface_Ione;
            this.pictureBox2.Location = new System.Drawing.Point(3, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(57, 33);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 191;
            this.pictureBox2.TabStop = false;
            // 
            // dgvCustomers
            // 
            this.dgvCustomers.AccessibleName = "Table";
            this.dgvCustomers.AllowResizingColumns = true;
            this.dgvCustomers.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvCustomers.AutoSizeColumnsMode = Syncfusion.WinForms.DataGrid.Enums.AutoSizeColumnsMode.AllCells;
            this.tableLayoutPanel2.SetColumnSpan(this.dgvCustomers, 8);
            this.dgvCustomers.ContextMenuStrip = this.contextMenuStrip1;
            this.dgvCustomers.Location = new System.Drawing.Point(3, 58);
            this.dgvCustomers.Name = "dgvCustomers";
            this.tableLayoutPanel2.SetRowSpan(this.dgvCustomers, 6);
            this.dgvCustomers.Size = new System.Drawing.Size(993, 535);
            this.dgvCustomers.Style.CellStyle.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dgvCustomers.Style.CellStyle.Font.Facename = "Segoe UI";
            this.dgvCustomers.Style.CellStyle.Font.Size = 10F;
            this.dgvCustomers.Style.HeaderStyle.BackColor = System.Drawing.Color.LightBlue;
            this.dgvCustomers.TabIndex = 195;
            this.dgvCustomers.Text = "sfDataGrid1";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tableLayoutPanel2.SetColumnSpan(this.label12, 2);
            this.label12.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.DarkBlue;
            this.label12.Location = new System.Drawing.Point(66, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(291, 39);
            this.label12.TabIndex = 190;
            this.label12.Text = "MANAGE SUPPLIERS";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnReset
            // 
            this.btnReset.AccessibleName = "Button";
            this.btnReset.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReset.BackColor = System.Drawing.Color.White;
            this.btnReset.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnReset.Location = new System.Drawing.Point(723, 3);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(114, 33);
            this.btnReset.Style.BackColor = System.Drawing.Color.White;
            this.btnReset.Style.Image = global::ioneNet.Properties.Resources.Refresh;
            this.btnReset.TabIndex = 1;
            this.btnReset.Text = "Refresh";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.sfButton1_Click);
            // 
            // sfButton1
            // 
            this.sfButton1.AccessibleName = "Button";
            this.sfButton1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sfButton1.BackColor = System.Drawing.Color.White;
            this.sfButton1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sfButton1.Location = new System.Drawing.Point(843, 3);
            this.sfButton1.Name = "sfButton1";
            this.sfButton1.Size = new System.Drawing.Size(153, 33);
            this.sfButton1.Style.BackColor = System.Drawing.Color.White;
            this.sfButton1.Style.Image = global::ioneNet.Properties.Resources.Exit;
            this.sfButton1.TabIndex = 2;
            this.sfButton1.Text = "Close";
            this.sfButton1.UseVisualStyleBackColor = false;
            this.sfButton1.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnAddNew
            // 
            this.btnAddNew.AccessibleName = "Button";
            this.btnAddNew.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddNew.BackColor = System.Drawing.Color.White;
            this.btnAddNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddNew.Location = new System.Drawing.Point(363, 3);
            this.btnAddNew.Name = "btnAddNew";
            this.btnAddNew.Size = new System.Drawing.Size(114, 33);
            this.btnAddNew.Style.BackColor = System.Drawing.Color.White;
            this.btnAddNew.Style.Image = global::ioneNet.Properties.Resources.Add;
            this.btnAddNew.TabIndex = 0;
            this.btnAddNew.Text = "Add New";
            this.btnAddNew.UseVisualStyleBackColor = false;
            this.btnAddNew.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnImport
            // 
            this.btnImport.AccessibleName = "Button";
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.BackColor = System.Drawing.Color.White;
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnImport.Location = new System.Drawing.Point(483, 3);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(114, 33);
            this.btnImport.Style.BackColor = System.Drawing.Color.White;
            this.btnImport.Style.Image = global::ioneNet.Properties.Resources.Import_Picture_Document_icon;
            this.btnImport.TabIndex = 193;
            this.btnImport.Text = "Export";
            this.btnImport.UseVisualStyleBackColor = false;
            this.btnImport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // sfButton2
            // 
            this.sfButton2.AccessibleName = "Button";
            this.sfButton2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sfButton2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sfButton2.Location = new System.Drawing.Point(603, 3);
            this.sfButton2.Name = "sfButton2";
            this.sfButton2.Size = new System.Drawing.Size(114, 33);
            this.sfButton2.Style.BackColor = System.Drawing.Color.White;
            this.sfButton2.TabIndex = 196;
            this.sfButton2.Text = "Import";
            this.sfButton2.Click += new System.EventHandler(this.sfButton2_Click);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(244, 24);
            this.viewToolStripMenuItem.Text = "View";
            this.viewToolStripMenuItem.Click += new System.EventHandler(this.viewToolStripMenuItem_Click);
            // 
            // ListOfSupplier
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1023, 620);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.GroupBox1);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.KeyPreview = true;
            this.Name = "ListOfSupplier";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Party List";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmCustomers_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmCustomers_KeyDown);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.contextMenuStrip1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCustomers)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.GroupBox GroupBox1;
        private System.Windows.Forms.ComboBox comboBox1;

        internal System.Windows.Forms.Label Label1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem modifyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deactivateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem updateOpeningStockToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
        private System.Windows.Forms.ToolStripMenuItem orderDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem salesDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stockLedgerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem purchaseOrderDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem purchaseDataToolStripMenuItem;
        private Syncfusion.WinForms.Controls.SfButton btnClose;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private Syncfusion.WinForms.DataGrid.SfDataGrid dgvCustomers;
        private System.Windows.Forms.Label label12;
        private Syncfusion.WinForms.Controls.SfButton btnAddNew;
        private Syncfusion.WinForms.Controls.SfButton btnImport;
        private Syncfusion.WinForms.Controls.SfButton btnReset;
        private Syncfusion.WinForms.Controls.SfButton sfButton1;
        private Syncfusion.WinForms.Controls.SfButton sfButton2;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
    }
}