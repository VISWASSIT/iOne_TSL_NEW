namespace ioneNet.ProductionManagement.Transactions
{
    partial class frmForge_Production_Cutting
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmForge_Production_Cutting));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label12 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.btnFind = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.dpdate = new System.Windows.Forms.DateTimePicker();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbShift = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.cmbMachineID = new System.Windows.Forms.ComboBox();
            this.dgJobCardData = new System.Windows.Forms.DataGridView();
            this.RM_Sec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Machine_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Job_Card = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Job_card_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Item_Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Item_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Heat_Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Job_Card_Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pending_Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty_Produced = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty_Rejected = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty_Accepted = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cutting_Length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Blade_No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Blade_Cond = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RM_End_Piece = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Disposal = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.txtvchno = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbOperator = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbSupervisor = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtRemarks = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblCreatedBy = new System.Windows.Forms.LinkLabel();
            this.lblModified = new System.Windows.Forms.LinkLabel();
            this.label30 = new System.Windows.Forms.Label();
            this.label31 = new System.Windows.Forms.Label();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.sfDataGrid1 = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            this.btnCancelItemSelection = new System.Windows.Forms.Button();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgJobCardData)).BeginInit();
            this.tableLayoutPanel5.SuspendLayout();
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
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.DarkBlue;
            this.label12.Location = new System.Drawing.Point(63, 35);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(189, 24);
            this.label12.TabIndex = 192;
            this.label12.Text = "CUTTING REPORT";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 9;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.02011F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 12.16475F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.375479F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.13254F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.13254F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.13254F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.13254F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.910892F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.35644F));
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnFind, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.label6, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.dpdate, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.label10, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbShift, 6, 0);
            this.tableLayoutPanel1.Controls.Add(this.label15, 7, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbMachineID, 8, 0);
            this.tableLayoutPanel1.Controls.Add(this.dgJobCardData, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtvchno, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label9, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.cmbOperator, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label8, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.cmbSupervisor, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.label11, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.txtRemarks, 1, 4);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(14, 73);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.5F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 79.09091F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.590909F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.818182F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 65F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1044, 506);
            this.tableLayoutPanel1.TabIndex = 194;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(6, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(123, 33);
            this.label1.TabIndex = 0;
            this.label1.Text = "Report No";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnFind
            // 
            this.btnFind.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFind.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFind.Font = new System.Drawing.Font("Calibri", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFind.Image = ((System.Drawing.Image)(resources.GetObject("btnFind.Image")));
            this.btnFind.Location = new System.Drawing.Point(267, 7);
            this.btnFind.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(64, 19);
            this.btnFind.TabIndex = 2;
            this.btnFind.UseVisualStyleBackColor = false;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(343, 0);
            this.label6.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(103, 33);
            this.label6.TabIndex = 3;
            this.label6.Text = "Date";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // dpdate
            // 
            this.dpdate.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dpdate.CustomFormat = "dd/MM/yyyy";
            this.dpdate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dpdate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dpdate.Location = new System.Drawing.Point(458, 7);
            this.dpdate.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.dpdate.Name = "dpdate";
            this.dpdate.Size = new System.Drawing.Size(103, 26);
            this.dpdate.TabIndex = 4;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.Location = new System.Drawing.Point(573, 0);
            this.label10.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(103, 33);
            this.label10.TabIndex = 5;
            this.label10.Text = "Shift";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbShift
            // 
            this.cmbShift.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbShift.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.cmbShift.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbShift.FormattingEnabled = true;
            this.cmbShift.Items.AddRange(new object[] {
            "G",
            "A",
            "B",
            "C"});
            this.cmbShift.Location = new System.Drawing.Point(685, 4);
            this.cmbShift.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbShift.Name = "cmbShift";
            this.cmbShift.Size = new System.Drawing.Size(109, 28);
            this.cmbShift.TabIndex = 6;
            // 
            // label15
            // 
            this.label15.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label15.Location = new System.Drawing.Point(803, 0);
            this.label15.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(80, 33);
            this.label15.TabIndex = 7;
            this.label15.Text = "M/C Name";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbMachineID
            // 
            this.cmbMachineID.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbMachineID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.cmbMachineID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbMachineID.FormattingEnabled = true;
            this.cmbMachineID.Location = new System.Drawing.Point(892, 4);
            this.cmbMachineID.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbMachineID.Name = "cmbMachineID";
            this.cmbMachineID.Size = new System.Drawing.Size(149, 28);
            this.cmbMachineID.TabIndex = 8;
            // 
            // dgJobCardData
            // 
            this.dgJobCardData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.SkyBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgJobCardData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgJobCardData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgJobCardData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RM_Sec,
            this.Machine_Name,
            this.Job_Card,
            this.Job_card_date,
            this.Item_Code,
            this.Item_Name,
            this.Heat_Code,
            this.Job_Card_Qty,
            this.Pending_Qty,
            this.Qty_Produced,
            this.Qty_Rejected,
            this.Qty_Accepted,
            this.Cutting_Length,
            this.Blade_No,
            this.Blade_Cond,
            this.RM_End_Piece,
            this.Disposal,
            this.Remarks});
            this.tableLayoutPanel1.SetColumnSpan(this.dgJobCardData, 10);
            this.dgJobCardData.EnableHeadersVisualStyles = false;
            this.dgJobCardData.Location = new System.Drawing.Point(3, 37);
            this.dgJobCardData.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.dgJobCardData.Name = "dgJobCardData";
            this.dgJobCardData.RowHeadersWidth = 62;
            this.dgJobCardData.Size = new System.Drawing.Size(1038, 340);
            this.dgJobCardData.TabIndex = 196;
            this.dgJobCardData.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgJobCardData_CellContentClick);
            this.dgJobCardData.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgJobCardData_CellEndEdit);
            this.dgJobCardData.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgJobCardData_EditingControlShowing);
            this.dgJobCardData.Enter += new System.EventHandler(this.dgJobCardData_Enter);
            this.dgJobCardData.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgJobCardData_KeyDown);
            // 
            // RM_Sec
            // 
            this.RM_Sec.DataPropertyName = "RM_Sec";
            this.RM_Sec.HeaderText = "RM Section";
            this.RM_Sec.MinimumWidth = 8;
            this.RM_Sec.Name = "RM_Sec";
            this.RM_Sec.Width = 150;
            // 
            // Machine_Name
            // 
            this.Machine_Name.DataPropertyName = "Machine_Name";
            this.Machine_Name.HeaderText = "Machine_Name";
            this.Machine_Name.MinimumWidth = 8;
            this.Machine_Name.Name = "Machine_Name";
            this.Machine_Name.Width = 150;
            // 
            // Job_Card
            // 
            this.Job_Card.DataPropertyName = "Job_Card";
            this.Job_Card.HeaderText = "Job Card No";
            this.Job_Card.MinimumWidth = 8;
            this.Job_Card.Name = "Job_Card";
            this.Job_Card.Width = 150;
            // 
            // Job_card_date
            // 
            this.Job_card_date.DataPropertyName = "Job_card_date";
            this.Job_card_date.HeaderText = "JC Date";
            this.Job_card_date.MinimumWidth = 8;
            this.Job_card_date.Name = "Job_card_date";
            this.Job_card_date.ReadOnly = true;
            this.Job_card_date.Width = 150;
            // 
            // Item_Code
            // 
            this.Item_Code.DataPropertyName = "Item_Code";
            this.Item_Code.HeaderText = "Item Code";
            this.Item_Code.MinimumWidth = 8;
            this.Item_Code.Name = "Item_Code";
            this.Item_Code.ReadOnly = true;
            this.Item_Code.Width = 150;
            // 
            // Item_Name
            // 
            this.Item_Name.DataPropertyName = "Item_Name";
            this.Item_Name.HeaderText = "Item Name";
            this.Item_Name.MinimumWidth = 8;
            this.Item_Name.Name = "Item_Name";
            this.Item_Name.ReadOnly = true;
            this.Item_Name.Width = 150;
            // 
            // Heat_Code
            // 
            this.Heat_Code.DataPropertyName = "Heat_Code";
            this.Heat_Code.HeaderText = "Heat Code";
            this.Heat_Code.MinimumWidth = 8;
            this.Heat_Code.Name = "Heat_Code";
            this.Heat_Code.Width = 150;
            // 
            // Job_Card_Qty
            // 
            this.Job_Card_Qty.DataPropertyName = "Job_Card_Qty";
            this.Job_Card_Qty.HeaderText = "JC Qty";
            this.Job_Card_Qty.MinimumWidth = 8;
            this.Job_Card_Qty.Name = "Job_Card_Qty";
            this.Job_Card_Qty.ReadOnly = true;
            this.Job_Card_Qty.Width = 150;
            // 
            // Pending_Qty
            // 
            this.Pending_Qty.DataPropertyName = "Pending_Qty";
            this.Pending_Qty.HeaderText = "Pending Qty";
            this.Pending_Qty.MinimumWidth = 8;
            this.Pending_Qty.Name = "Pending_Qty";
            this.Pending_Qty.ReadOnly = true;
            this.Pending_Qty.Width = 150;
            // 
            // Qty_Produced
            // 
            this.Qty_Produced.DataPropertyName = "Qty_Produced";
            this.Qty_Produced.HeaderText = "Qty Produced";
            this.Qty_Produced.MinimumWidth = 8;
            this.Qty_Produced.Name = "Qty_Produced";
            this.Qty_Produced.Width = 150;
            // 
            // Qty_Rejected
            // 
            this.Qty_Rejected.DataPropertyName = "Qty_Rejected";
            this.Qty_Rejected.HeaderText = "Qty Rejected";
            this.Qty_Rejected.MinimumWidth = 8;
            this.Qty_Rejected.Name = "Qty_Rejected";
            this.Qty_Rejected.Width = 150;
            // 
            // Qty_Accepted
            // 
            this.Qty_Accepted.DataPropertyName = "Qty_Accepted";
            this.Qty_Accepted.HeaderText = "Qty Accepted";
            this.Qty_Accepted.MinimumWidth = 8;
            this.Qty_Accepted.Name = "Qty_Accepted";
            this.Qty_Accepted.ReadOnly = true;
            this.Qty_Accepted.Width = 150;
            // 
            // Cutting_Length
            // 
            this.Cutting_Length.DataPropertyName = "Cutting_Length";
            this.Cutting_Length.HeaderText = "Cutting Length";
            this.Cutting_Length.MinimumWidth = 8;
            this.Cutting_Length.Name = "Cutting_Length";
            this.Cutting_Length.Width = 150;
            // 
            // Blade_No
            // 
            this.Blade_No.DataPropertyName = "Blade_No";
            this.Blade_No.HeaderText = "Blade No";
            this.Blade_No.MinimumWidth = 8;
            this.Blade_No.Name = "Blade_No";
            this.Blade_No.Width = 150;
            // 
            // Blade_Cond
            // 
            this.Blade_Cond.DataPropertyName = "Blade_Cond";
            this.Blade_Cond.HeaderText = "Blade Condition";
            this.Blade_Cond.MinimumWidth = 8;
            this.Blade_Cond.Name = "Blade_Cond";
            this.Blade_Cond.Width = 150;
            // 
            // RM_End_Piece
            // 
            this.RM_End_Piece.DataPropertyName = "RM_End_Piece";
            this.RM_End_Piece.HeaderText = "RM End Piece";
            this.RM_End_Piece.MinimumWidth = 8;
            this.RM_End_Piece.Name = "RM_End_Piece";
            this.RM_End_Piece.Width = 150;
            // 
            // Disposal
            // 
            this.Disposal.DataPropertyName = "Disposal";
            this.Disposal.HeaderText = "Disposal";
            this.Disposal.MinimumWidth = 8;
            this.Disposal.Name = "Disposal";
            this.Disposal.Width = 150;
            // 
            // Remarks
            // 
            this.Remarks.DataPropertyName = "Remarks";
            this.Remarks.HeaderText = "Remarks";
            this.Remarks.MinimumWidth = 8;
            this.Remarks.Name = "Remarks";
            this.Remarks.Width = 150;
            // 
            // txtvchno
            // 
            this.txtvchno.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtvchno.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtvchno.Location = new System.Drawing.Point(141, 7);
            this.txtvchno.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.txtvchno.Name = "txtvchno";
            this.txtvchno.ReadOnly = true;
            this.txtvchno.Size = new System.Drawing.Size(114, 26);
            this.txtvchno.TabIndex = 1;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(6, 381);
            this.label9.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(123, 29);
            this.label9.TabIndex = 41;
            this.label9.Text = "Operator";
            // 
            // cmbOperator
            // 
            this.cmbOperator.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.cmbOperator, 2);
            this.cmbOperator.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbOperator.FormattingEnabled = true;
            this.cmbOperator.Location = new System.Drawing.Point(138, 385);
            this.cmbOperator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbOperator.Name = "cmbOperator";
            this.cmbOperator.Size = new System.Drawing.Size(196, 28);
            this.cmbOperator.TabIndex = 40;
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(6, 410);
            this.label8.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(123, 30);
            this.label8.TabIndex = 39;
            this.label8.Text = "Supervisor";
            // 
            // cmbSupervisor
            // 
            this.cmbSupervisor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.cmbSupervisor, 2);
            this.cmbSupervisor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSupervisor.FormattingEnabled = true;
            this.cmbSupervisor.Location = new System.Drawing.Point(138, 414);
            this.cmbSupervisor.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cmbSupervisor.Name = "cmbSupervisor";
            this.cmbSupervisor.Size = new System.Drawing.Size(196, 28);
            this.cmbSupervisor.TabIndex = 42;
            this.cmbSupervisor.SelectedIndexChanged += new System.EventHandler(this.cmbSupervisor_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.Location = new System.Drawing.Point(6, 440);
            this.label11.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(123, 66);
            this.label11.TabIndex = 43;
            this.label11.Text = "Remarks";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtRemarks
            // 
            this.txtRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtRemarks, 3);
            this.txtRemarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRemarks.Location = new System.Drawing.Point(141, 447);
            this.txtRemarks.Margin = new System.Windows.Forms.Padding(6, 7, 6, 7);
            this.txtRemarks.Multiline = true;
            this.txtRemarks.Name = "txtRemarks";
            this.txtRemarks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtRemarks.Size = new System.Drawing.Size(305, 52);
            this.txtRemarks.TabIndex = 195;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel5.ColumnCount = 8;
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.149666F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.25262F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.435653F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.68065F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10F));
            this.tableLayoutPanel5.Controls.Add(this.btnClose, 7, 0);
            this.tableLayoutPanel5.Controls.Add(this.lblCreatedBy, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.lblModified, 3, 0);
            this.tableLayoutPanel5.Controls.Add(this.label30, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.label31, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnDelete, 6, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnSave, 5, 0);
            this.tableLayoutPanel5.Controls.Add(this.btnClear, 4, 0);
            this.tableLayoutPanel5.Location = new System.Drawing.Point(17, 587);
            this.tableLayoutPanel5.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new System.Drawing.Size(1017, 50);
            this.tableLayoutPanel5.TabIndex = 208;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(897, 7);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(117, 36);
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
            this.lblCreatedBy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreatedBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblCreatedBy.Location = new System.Drawing.Point(89, 0);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(177, 50);
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
            this.lblModified.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModified.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblModified.Location = new System.Drawing.Point(361, 0);
            this.lblModified.Name = "lblModified";
            this.lblModified.Size = new System.Drawing.Size(170, 50);
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
            this.label30.Location = new System.Drawing.Point(272, 0);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(81, 50);
            this.label30.TabIndex = 5;
            this.label30.Text = "Modified By";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label31
            // 
            this.label31.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.Location = new System.Drawing.Point(3, 0);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(78, 50);
            this.label31.TabIndex = 3;
            this.label31.Text = "Created By";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnDelete
            // 
            this.btnDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDelete.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.Location = new System.Drawing.Point(777, 4);
            this.btnDelete.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(114, 42);
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
            this.btnSave.Location = new System.Drawing.Point(657, 7);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(114, 36);
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
            this.btnClear.Location = new System.Drawing.Point(537, 7);
            this.btnClear.Margin = new System.Windows.Forms.Padding(3, 7, 3, 7);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(114, 36);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ioneNet.Properties.Resources.logoface_Ione;
            this.pictureBox2.Location = new System.Drawing.Point(3, 3);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(52, 52);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 193;
            this.pictureBox2.TabStop = false;
            // 
            // groupBox2
            // 
            this.groupBox2.BackColor = System.Drawing.Color.LightGray;
            this.groupBox2.Controls.Add(this.sfDataGrid1);
            this.groupBox2.Controls.Add(this.btnCancelItemSelection);
            this.groupBox2.Location = new System.Drawing.Point(63, 112);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(845, 398);
            this.groupBox2.TabIndex = 209;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Select Job Card";
            this.groupBox2.Visible = false;
            // 
            // sfDataGrid1
            // 
            this.sfDataGrid1.AccessibleName = "Table";
            this.sfDataGrid1.AllowResizingColumns = true;
            this.sfDataGrid1.AutoSizeColumnsMode = Syncfusion.WinForms.DataGrid.Enums.AutoSizeColumnsMode.AllCells;
            this.sfDataGrid1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.sfDataGrid1.Location = new System.Drawing.Point(6, 24);
            this.sfDataGrid1.Name = "sfDataGrid1";
            this.sfDataGrid1.PreviewRowHeight = 35;
            this.sfDataGrid1.Size = new System.Drawing.Size(830, 334);
            this.sfDataGrid1.TabIndex = 6;
            this.sfDataGrid1.Text = "sfDataGrid1";
            this.sfDataGrid1.Click += new System.EventHandler(this.sfDataGrid1_Click);
            // 
            // btnCancelItemSelection
            // 
            this.btnCancelItemSelection.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnCancelItemSelection.Location = new System.Drawing.Point(740, 364);
            this.btnCancelItemSelection.Name = "btnCancelItemSelection";
            this.btnCancelItemSelection.Size = new System.Drawing.Size(85, 28);
            this.btnCancelItemSelection.TabIndex = 2;
            this.btnCancelItemSelection.Text = "Cancel";
            this.btnCancelItemSelection.UseVisualStyleBackColor = false;
            this.btnCancelItemSelection.Click += new System.EventHandler(this.btnCancelItemSelection_Click);
            // 
            // frmForge_Production_Cutting
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1072, 677);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.tableLayoutPanel5);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label12);
            this.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmForge_Production_Cutting";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cutting Report";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmForge_Production_Cutting_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgJobCardData)).EndInit();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtvchno;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DateTimePicker dpdate;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbShift;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox cmbMachineID;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbOperator;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbSupervisor;
        private System.Windows.Forms.TextBox txtRemarks;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.LinkLabel lblCreatedBy;
        private System.Windows.Forms.LinkLabel lblModified;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.DataGridView dgJobCardData;
        private System.Windows.Forms.GroupBox groupBox2;
        private Syncfusion.WinForms.DataGrid.SfDataGrid sfDataGrid1;
        private System.Windows.Forms.Button btnCancelItemSelection;
        private System.Windows.Forms.DataGridViewTextBoxColumn RM_Sec;
        private System.Windows.Forms.DataGridViewTextBoxColumn Machine_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Job_Card;
        private System.Windows.Forms.DataGridViewTextBoxColumn Job_card_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item_Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Heat_Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Job_Card_Qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pending_Qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty_Produced;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty_Rejected;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty_Accepted;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cutting_Length;
        private System.Windows.Forms.DataGridViewTextBoxColumn Blade_No;
        private System.Windows.Forms.DataGridViewTextBoxColumn Blade_Cond;
        private System.Windows.Forms.DataGridViewTextBoxColumn RM_End_Piece;
        private System.Windows.Forms.DataGridViewTextBoxColumn Disposal;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
    }
}