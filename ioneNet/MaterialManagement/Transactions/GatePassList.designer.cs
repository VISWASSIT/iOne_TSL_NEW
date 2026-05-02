namespace ioneNet.MaterialManagement
{
    partial class GatePassList
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
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modifyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.approveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.eMailToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accountPostingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.viewDetailedLedgerVocherWiseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ledgerPartWiseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ledgerProductWiseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.sfDataGrid1 = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            this.showGRNListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ioneDataSet = new ioneNet.ioneDataSet();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.label3 = new System.Windows.Forms.Label();
            this.showGRNListTableAdapter = new ioneNet.ioneDataSetTableAdapters.ShowGRNListTableAdapter();
            this.btnClose = new Syncfusion.WinForms.Controls.SfButton();
            this.btnReset = new Syncfusion.WinForms.Controls.SfButton();
            this.btnAddnew = new Syncfusion.WinForms.Controls.SfButton();
            this.contextMenuStrip1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sfDataGrid1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.showGRNListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ioneDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem,
            this.modifyToolStripMenuItem,
            this.deleteToolStripMenuItem,
            this.approveToolStripMenuItem,
            this.eMailToolStripMenuItem,
            this.accountPostingToolStripMenuItem,
            this.toolStripMenuItem1,
            this.viewDetailedLedgerVocherWiseToolStripMenuItem,
            this.ledgerPartWiseToolStripMenuItem,
            this.ledgerProductWiseToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(226, 226);
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(225, 24);
            this.viewToolStripMenuItem.Text = "View";
            this.viewToolStripMenuItem.Click += new System.EventHandler(this.viewToolStripMenuItem_Click);
            // 
            // modifyToolStripMenuItem
            // 
            this.modifyToolStripMenuItem.Name = "modifyToolStripMenuItem";
            this.modifyToolStripMenuItem.Size = new System.Drawing.Size(225, 24);
            this.modifyToolStripMenuItem.Text = "Modify";
            this.modifyToolStripMenuItem.Click += new System.EventHandler(this.modifyToolStripMenuItem_Click);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            this.deleteToolStripMenuItem.Size = new System.Drawing.Size(225, 24);
            this.deleteToolStripMenuItem.Text = "Delete";
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // approveToolStripMenuItem
            // 
            this.approveToolStripMenuItem.Name = "approveToolStripMenuItem";
            this.approveToolStripMenuItem.Size = new System.Drawing.Size(225, 24);
            this.approveToolStripMenuItem.Text = "Print";
            this.approveToolStripMenuItem.Click += new System.EventHandler(this.approveToolStripMenuItem_Click);
            // 
            // eMailToolStripMenuItem
            // 
            this.eMailToolStripMenuItem.Name = "eMailToolStripMenuItem";
            this.eMailToolStripMenuItem.Size = new System.Drawing.Size(225, 24);
            this.eMailToolStripMenuItem.Text = "E Mail";
            this.eMailToolStripMenuItem.Click += new System.EventHandler(this.eMailToolStripMenuItem_Click);
            // 
            // accountPostingToolStripMenuItem
            // 
            this.accountPostingToolStripMenuItem.Name = "accountPostingToolStripMenuItem";
            this.accountPostingToolStripMenuItem.Size = new System.Drawing.Size(225, 24);
            this.accountPostingToolStripMenuItem.Text = "Receive Material";
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(222, 6);
            // 
            // viewDetailedLedgerVocherWiseToolStripMenuItem
            // 
            this.viewDetailedLedgerVocherWiseToolStripMenuItem.Name = "viewDetailedLedgerVocherWiseToolStripMenuItem";
            this.viewDetailedLedgerVocherWiseToolStripMenuItem.Size = new System.Drawing.Size(225, 24);
            this.viewDetailedLedgerVocherWiseToolStripMenuItem.Text = "Ledger - Vocher Wise";
            // 
            // ledgerPartWiseToolStripMenuItem
            // 
            this.ledgerPartWiseToolStripMenuItem.Name = "ledgerPartWiseToolStripMenuItem";
            this.ledgerPartWiseToolStripMenuItem.Size = new System.Drawing.Size(225, 24);
            this.ledgerPartWiseToolStripMenuItem.Text = "Ledger - Part Wise";
            // 
            // ledgerProductWiseToolStripMenuItem
            // 
            this.ledgerProductWiseToolStripMenuItem.Name = "ledgerProductWiseToolStripMenuItem";
            this.ledgerProductWiseToolStripMenuItem.Size = new System.Drawing.Size(225, 24);
            this.ledgerProductWiseToolStripMenuItem.Text = "Ledger - Product Wise";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.sfDataGrid1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 62);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1124, 559);
            this.tableLayoutPanel1.TabIndex = 8;
            // 
            // sfDataGrid1
            // 
            this.sfDataGrid1.AccessibleName = "Table";
            this.sfDataGrid1.AllowFiltering = true;
            this.sfDataGrid1.AllowResizingColumns = true;
            this.sfDataGrid1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.sfDataGrid1.AutoSizeColumnsMode = Syncfusion.WinForms.DataGrid.Enums.AutoSizeColumnsMode.AllCells;
            this.sfDataGrid1.ContextMenuStrip = this.contextMenuStrip1;
            this.sfDataGrid1.Location = new System.Drawing.Point(3, 3);
            this.sfDataGrid1.Name = "sfDataGrid1";
            this.sfDataGrid1.PreviewRowHeight = 35;
            this.sfDataGrid1.ShowGroupDropArea = true;
            this.sfDataGrid1.Size = new System.Drawing.Size(1118, 553);
            this.sfDataGrid1.TabIndex = 0;
            this.sfDataGrid1.Text = "sfDataGrid1";
            // 
            // showGRNListBindingSource
            // 
            this.showGRNListBindingSource.DataMember = "ShowGRNList";
            this.showGRNListBindingSource.DataSource = this.ioneDataSet;
            // 
            // ioneDataSet
            // 
            this.ioneDataSet.DataSetName = "ioneDataSet";
            this.ioneDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ioneNet.Properties.Resources.logoface_Ione;
            this.pictureBox2.Location = new System.Drawing.Point(0, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(45, 40);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 15;
            this.pictureBox2.TabStop = false;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.DarkBlue;
            this.label3.Location = new System.Drawing.Point(51, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(167, 24);
            this.label3.TabIndex = 14;
            this.label3.Text = "GATEPASS LIST";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // showGRNListTableAdapter
            // 
            this.showGRNListTableAdapter.ClearBeforeFill = true;
            // 
            // btnClose
            // 
            this.btnClose.AccessibleName = "Button";
            this.btnClose.BackColor = System.Drawing.Color.Turquoise;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnClose.Location = new System.Drawing.Point(1035, 18);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(96, 28);
            this.btnClose.Style.BackColor = System.Drawing.Color.Turquoise;
            this.btnClose.TabIndex = 22;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnReset
            // 
            this.btnReset.AccessibleName = "Button";
            this.btnReset.BackColor = System.Drawing.Color.Turquoise;
            this.btnReset.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnReset.Location = new System.Drawing.Point(918, 18);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(96, 28);
            this.btnReset.Style.BackColor = System.Drawing.Color.Turquoise;
            this.btnReset.TabIndex = 21;
            this.btnReset.Text = "Refresh";
            this.btnReset.UseVisualStyleBackColor = false;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // btnAddnew
            // 
            this.btnAddnew.AccessibleName = "Button";
            this.btnAddnew.BackColor = System.Drawing.Color.Turquoise;
            this.btnAddnew.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnAddnew.Location = new System.Drawing.Point(805, 18);
            this.btnAddnew.Name = "btnAddnew";
            this.btnAddnew.Size = new System.Drawing.Size(96, 28);
            this.btnAddnew.Style.BackColor = System.Drawing.Color.Turquoise;
            this.btnAddnew.TabIndex = 20;
            this.btnAddnew.Text = "Add New";
            this.btnAddnew.UseVisualStyleBackColor = false;
            this.btnAddnew.Click += new System.EventHandler(this.btnAddNew_Click);
            // 
            // GatePassList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1148, 678);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnReset);
            this.Controls.Add(this.btnAddnew);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "GatePassList";
            this.Text = "Gate Pass List";
            this.Load += new System.EventHandler(this.ListOfOrders_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.sfDataGrid1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.showGRNListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ioneDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem approveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem modifyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accountPostingToolStripMenuItem;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.BindingSource showGRNListBindingSource;
        private ioneDataSet ioneDataSet;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.DataGridViewTextBoxColumn sONODataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn sODateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn soAmendNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn soAmendDateDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn quotNoDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerNameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn customerPONoDataGridViewTextBoxColumn;
        private ioneDataSetTableAdapters.ShowGRNListTableAdapter showGRNListTableAdapter;
        private System.Windows.Forms.DataGridViewTextBoxColumn totOrdValueDataGridViewTextBoxColumn;
        private Syncfusion.WinForms.DataGrid.SfDataGrid sfDataGrid1;
        private Syncfusion.WinForms.Controls.SfButton btnClose;
        private Syncfusion.WinForms.Controls.SfButton btnReset;
        private Syncfusion.WinForms.Controls.SfButton btnAddnew;
        private System.Windows.Forms.ToolStripMenuItem viewDetailedLedgerVocherWiseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ledgerPartWiseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ledgerProductWiseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem eMailToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
    }
}