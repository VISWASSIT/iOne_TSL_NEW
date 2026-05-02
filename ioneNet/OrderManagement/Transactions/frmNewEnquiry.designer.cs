using System;

namespace ioneNet.OrderManagement.Transactions
{
    partial class frmNewEnquiry
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
            this.label1 = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.txtEnqNo = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dpEnqDate = new System.Windows.Forms.DateTimePicker();
            this.label6 = new System.Windows.Forms.Label();
            this.CmbBuyerName = new System.Windows.Forms.ComboBox();
            this.label38 = new System.Windows.Forms.Label();
            this.dpCustRefDate = new System.Windows.Forms.DateTimePicker();
            this.chkSEZSupply = new System.Windows.Forms.CheckBox();
            this.chkExport = new System.Windows.Forms.CheckBox();
            this.btnaddnewcustomer = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.txtCustRefNo = new System.Windows.Forms.TextBox();
            this.linkLabel2 = new System.Windows.Forms.LinkLabel();
            this.chkNew = new System.Windows.Forms.CheckBox();
            this.txtEnqSource = new System.Windows.Forms.TextBox();
            this.cmbEnqSource = new System.Windows.Forms.TextBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.txtOtherRemarks = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.label39 = new System.Windows.Forms.Label();
            this.dgProducts = new System.Windows.Forms.DataGridView();
            this.Item_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Item_Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Item_Description = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Item_Grade = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Prod_Length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.UOM = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Stock_Avialble = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Pending_Order_Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label33 = new System.Windows.Forms.Label();
            this.dtpQuoteToSubmit = new System.Windows.Forms.DateTimePicker();
            this.label31 = new System.Windows.Forms.Label();
            this.lblCreatedBy = new System.Windows.Forms.LinkLabel();
            this.label30 = new System.Windows.Forms.Label();
            this.lblModified = new System.Windows.Forms.LinkLabel();
            this.label34 = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.label21 = new System.Windows.Forms.Label();
            this.dtDeliveryDate = new System.Windows.Forms.DateTimePicker();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.cmbSaleExecutive = new System.Windows.Forms.ComboBox();
            this.chkTPI = new System.Windows.Forms.CheckBox();
            this.chkIBR = new System.Windows.Forms.CheckBox();
            this.cmbPacking = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.txtSalesregion = new System.Windows.Forms.TextBox();
            this.txtPayment = new System.Windows.Forms.TextBox();
            this.txtPricebasis = new System.Windows.Forms.TextBox();
            this.cmbSaleOffice = new System.Windows.Forms.TextBox();
            this.cmbPaymentterms = new System.Windows.Forms.TextBox();
            this.cmbPricebasis = new System.Windows.Forms.TextBox();
            this.cmbCutting = new System.Windows.Forms.TextBox();
            this.linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.linkLabel4 = new System.Windows.Forms.LinkLabel();
            this.linkLabel5 = new System.Windows.Forms.LinkLabel();
            this.linkLabel6 = new System.Windows.Forms.LinkLabel();
            this.linkLabel7 = new System.Windows.Forms.LinkLabel();
            this.linkLabel8 = new System.Windows.Forms.LinkLabel();
            this.linkLabel9 = new System.Windows.Forms.LinkLabel();
            this.cmbUT = new System.Windows.Forms.TextBox();
            this.chkMacro = new System.Windows.Forms.CheckBox();
            this.txtPackCode = new System.Windows.Forms.TextBox();
            this.txtUT = new System.Windows.Forms.TextBox();
            this.txtCutting = new System.Windows.Forms.TextBox();
            this.txtCity = new System.Windows.Forms.TextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
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
            this.label1.Location = new System.Drawing.Point(54, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(193, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "NEW ENQUIRY REGISTRATION";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 12;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.419518F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.992318F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 3.707224F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.64639F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.60076F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.410646F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 5.703422F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 7.889734F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.186312F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 8.079848F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 10.26616F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.01901F));
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtEnqNo, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.label3, 3, 0);
            this.tableLayoutPanel1.Controls.Add(this.dpEnqDate, 4, 0);
            this.tableLayoutPanel1.Controls.Add(this.label6, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.CmbBuyerName, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.label38, 10, 1);
            this.tableLayoutPanel1.Controls.Add(this.dpCustRefDate, 11, 1);
            this.tableLayoutPanel1.Controls.Add(this.chkSEZSupply, 10, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkExport, 11, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnaddnewcustomer, 5, 1);
            this.tableLayoutPanel1.Controls.Add(this.label5, 6, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtCustRefNo, 8, 1);
            this.tableLayoutPanel1.Controls.Add(this.linkLabel2, 5, 0);
            this.tableLayoutPanel1.Controls.Add(this.chkNew, 9, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtEnqSource, 8, 0);
            this.tableLayoutPanel1.Controls.Add(this.cmbEnqSource, 6, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(12, 47);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1052, 68);
            this.tableLayoutPanel1.TabIndex = 0;
            this.tableLayoutPanel1.Paint += new System.Windows.Forms.PaintEventHandler(this.tableLayoutPanel1_Paint);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Black;
            this.label2.Location = new System.Drawing.Point(3, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 34);
            this.label2.TabIndex = 0;
            this.label2.Text = "Enq No";
            // 
            // txtEnqNo
            // 
            this.txtEnqNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtEnqNo, 2);
            this.txtEnqNo.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEnqNo.Location = new System.Drawing.Point(102, 3);
            this.txtEnqNo.Name = "txtEnqNo";
            this.txtEnqNo.Size = new System.Drawing.Size(117, 23);
            this.txtEnqNo.TabIndex = 1;
            this.txtEnqNo.Layout += new System.Windows.Forms.LayoutEventHandler(this.txtSoNo_Layout);
            this.txtEnqNo.Leave += new System.EventHandler(this.txtSoNo_Leave);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Black;
            this.label3.Location = new System.Drawing.Point(225, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(106, 34);
            this.label3.TabIndex = 2;
            this.label3.Text = "Date";
            // 
            // dpEnqDate
            // 
            this.dpEnqDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dpEnqDate.CustomFormat = "dd/MM/yyyy";
            this.dpEnqDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dpEnqDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dpEnqDate.Location = new System.Drawing.Point(337, 3);
            this.dpEnqDate.Name = "dpEnqDate";
            this.dpEnqDate.ShowUpDown = true;
            this.dpEnqDate.Size = new System.Drawing.Size(95, 21);
            this.dpEnqDate.TabIndex = 2;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.Color.Blue;
            this.label6.Location = new System.Drawing.Point(3, 34);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 34);
            this.label6.TabIndex = 10;
            this.label6.Text = "Customer Name";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // CmbBuyerName
            // 
            this.CmbBuyerName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CmbBuyerName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.CmbBuyerName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.CmbBuyerName.BackColor = System.Drawing.SystemColors.Info;
            this.tableLayoutPanel1.SetColumnSpan(this.CmbBuyerName, 4);
            this.CmbBuyerName.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CmbBuyerName.FormattingEnabled = true;
            this.CmbBuyerName.Location = new System.Drawing.Point(102, 37);
            this.CmbBuyerName.Name = "CmbBuyerName";
            this.CmbBuyerName.Size = new System.Drawing.Size(330, 23);
            this.CmbBuyerName.TabIndex = 7;
            this.CmbBuyerName.Leave += new System.EventHandler(this.CmbBuyerName_Leave);
            // 
            // label38
            // 
            this.label38.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label38.AutoSize = true;
            this.label38.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label38.Location = new System.Drawing.Point(788, 34);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(102, 34);
            this.label38.TabIndex = 16;
            this.label38.Text = "Date";
            // 
            // dpCustRefDate
            // 
            this.dpCustRefDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dpCustRefDate.CustomFormat = "dd/MM/yyyy";
            this.dpCustRefDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dpCustRefDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dpCustRefDate.Location = new System.Drawing.Point(896, 37);
            this.dpCustRefDate.Name = "dpCustRefDate";
            this.dpCustRefDate.ShowUpDown = true;
            this.dpCustRefDate.Size = new System.Drawing.Size(153, 21);
            this.dpCustRefDate.TabIndex = 10;
            // 
            // chkSEZSupply
            // 
            this.chkSEZSupply.AutoSize = true;
            this.chkSEZSupply.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSEZSupply.ForeColor = System.Drawing.Color.Red;
            this.chkSEZSupply.Location = new System.Drawing.Point(788, 3);
            this.chkSEZSupply.Name = "chkSEZSupply";
            this.chkSEZSupply.Size = new System.Drawing.Size(98, 21);
            this.chkSEZSupply.TabIndex = 5;
            this.chkSEZSupply.Text = "SEZ Supply?";
            this.chkSEZSupply.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkSEZSupply.UseVisualStyleBackColor = true;
            // 
            // chkExport
            // 
            this.chkExport.AutoSize = true;
            this.chkExport.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkExport.ForeColor = System.Drawing.Color.Red;
            this.chkExport.Location = new System.Drawing.Point(896, 3);
            this.chkExport.Name = "chkExport";
            this.chkExport.Size = new System.Drawing.Size(121, 21);
            this.chkExport.TabIndex = 6;
            this.chkExport.Text = "Export Enquiry?";
            this.chkExport.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkExport.UseVisualStyleBackColor = true;
            // 
            // btnaddnewcustomer
            // 
            this.btnaddnewcustomer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnaddnewcustomer.Location = new System.Drawing.Point(438, 37);
            this.btnaddnewcustomer.Name = "btnaddnewcustomer";
            this.btnaddnewcustomer.Size = new System.Drawing.Size(93, 28);
            this.btnaddnewcustomer.TabIndex = 8;
            this.btnaddnewcustomer.Text = "Add";
            this.btnaddnewcustomer.UseVisualStyleBackColor = true;
            this.btnaddnewcustomer.Click += new System.EventHandler(this.btnaddnewcustomer_Click);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.tableLayoutPanel1.SetColumnSpan(this.label5, 2);
            this.label5.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(537, 34);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(137, 34);
            this.label5.TabIndex = 14;
            this.label5.Text = "Cust Ref Doc No";
            // 
            // txtCustRefNo
            // 
            this.txtCustRefNo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.SetColumnSpan(this.txtCustRefNo, 2);
            this.txtCustRefNo.Location = new System.Drawing.Point(680, 37);
            this.txtCustRefNo.Name = "txtCustRefNo";
            this.txtCustRefNo.Size = new System.Drawing.Size(102, 23);
            this.txtCustRefNo.TabIndex = 9;
            // 
            // linkLabel2
            // 
            this.linkLabel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel2.AutoSize = true;
            this.linkLabel2.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold);
            this.linkLabel2.Location = new System.Drawing.Point(438, 0);
            this.linkLabel2.Name = "linkLabel2";
            this.linkLabel2.Size = new System.Drawing.Size(93, 34);
            this.linkLabel2.TabIndex = 17;
            this.linkLabel2.TabStop = true;
            this.linkLabel2.Text = "Enq Source";
            this.linkLabel2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel2_LinkClicked);
            this.linkLabel2.Click += new System.EventHandler(this.linkLabel2_Click);
            // 
            // chkNew
            // 
            this.chkNew.AutoSize = true;
            this.chkNew.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkNew.ForeColor = System.Drawing.Color.Red;
            this.chkNew.Location = new System.Drawing.Point(703, 3);
            this.chkNew.Name = "chkNew";
            this.chkNew.Size = new System.Drawing.Size(79, 21);
            this.chkNew.TabIndex = 4;
            this.chkNew.Text = "New Customer?";
            this.chkNew.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.chkNew.UseVisualStyleBackColor = true;
            // 
            // txtEnqSource
            // 
            this.txtEnqSource.Enabled = false;
            this.txtEnqSource.Location = new System.Drawing.Point(680, 3);
            this.txtEnqSource.Name = "txtEnqSource";
            this.txtEnqSource.Size = new System.Drawing.Size(17, 23);
            this.txtEnqSource.TabIndex = 18;
            // 
            // cmbEnqSource
            // 
            this.cmbEnqSource.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbEnqSource.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tableLayoutPanel1.SetColumnSpan(this.cmbEnqSource, 2);
            this.cmbEnqSource.Location = new System.Drawing.Point(537, 3);
            this.cmbEnqSource.Name = "cmbEnqSource";
            this.cmbEnqSource.Size = new System.Drawing.Size(137, 23);
            this.cmbEnqSource.TabIndex = 3;
            this.cmbEnqSource.Enter += new System.EventHandler(this.cmbEnqSource_Enter);
            this.cmbEnqSource.Leave += new System.EventHandler(this.cmbEnqSource_Leave);
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.ColumnCount = 8;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 9.790874F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.8289F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 11.97719F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 17.96578F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 2.281369F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 14.35361F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 13.87833F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.87453F));
            this.tableLayoutPanel2.Controls.Add(this.txtOtherRemarks, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.label13, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.label39, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.dgProducts, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.label33, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.dtpQuoteToSubmit, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label31, 0, 7);
            this.tableLayoutPanel2.Controls.Add(this.lblCreatedBy, 1, 7);
            this.tableLayoutPanel2.Controls.Add(this.label30, 2, 7);
            this.tableLayoutPanel2.Controls.Add(this.lblModified, 3, 7);
            this.tableLayoutPanel2.Controls.Add(this.label34, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.cmbStatus, 1, 6);
            this.tableLayoutPanel2.Controls.Add(this.label21, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.dtDeliveryDate, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.linkLabel1, 2, 2);
            this.tableLayoutPanel2.Controls.Add(this.cmbSaleExecutive, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.chkTPI, 7, 2);
            this.tableLayoutPanel2.Controls.Add(this.chkIBR, 7, 3);
            this.tableLayoutPanel2.Controls.Add(this.cmbPacking, 6, 3);
            this.tableLayoutPanel2.Controls.Add(this.btnClose, 7, 7);
            this.tableLayoutPanel2.Controls.Add(this.btnSave, 6, 7);
            this.tableLayoutPanel2.Controls.Add(this.btnClear, 5, 7);
            this.tableLayoutPanel2.Controls.Add(this.txtSalesregion, 4, 2);
            this.tableLayoutPanel2.Controls.Add(this.txtPayment, 4, 3);
            this.tableLayoutPanel2.Controls.Add(this.txtPricebasis, 4, 4);
            this.tableLayoutPanel2.Controls.Add(this.cmbSaleOffice, 3, 2);
            this.tableLayoutPanel2.Controls.Add(this.cmbPaymentterms, 3, 3);
            this.tableLayoutPanel2.Controls.Add(this.cmbPricebasis, 3, 4);
            this.tableLayoutPanel2.Controls.Add(this.cmbCutting, 6, 4);
            this.tableLayoutPanel2.Controls.Add(this.linkLabel3, 2, 3);
            this.tableLayoutPanel2.Controls.Add(this.linkLabel4, 2, 4);
            this.tableLayoutPanel2.Controls.Add(this.linkLabel5, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.linkLabel6, 5, 2);
            this.tableLayoutPanel2.Controls.Add(this.linkLabel7, 5, 3);
            this.tableLayoutPanel2.Controls.Add(this.linkLabel8, 5, 4);
            this.tableLayoutPanel2.Controls.Add(this.linkLabel9, 5, 5);
            this.tableLayoutPanel2.Controls.Add(this.cmbUT, 6, 5);
            this.tableLayoutPanel2.Controls.Add(this.chkMacro, 7, 4);
            this.tableLayoutPanel2.Controls.Add(this.txtPackCode, 4, 1);
            this.tableLayoutPanel2.Controls.Add(this.txtUT, 4, 6);
            this.tableLayoutPanel2.Controls.Add(this.txtCutting, 4, 5);
            this.tableLayoutPanel2.Controls.Add(this.txtCity, 6, 2);
            this.tableLayoutPanel2.Location = new System.Drawing.Point(13, 121);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 8;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 46.56374F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.304729F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 6.483559F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.070987F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.070987F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.43063F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 7.662387F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 10.41298F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(1052, 509);
            this.tableLayoutPanel2.TabIndex = 1;
            // 
            // txtOtherRemarks
            // 
            this.txtOtherRemarks.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel2.SetColumnSpan(this.txtOtherRemarks, 3);
            this.txtOtherRemarks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOtherRemarks.Location = new System.Drawing.Point(105, 370);
            this.txtOtherRemarks.Multiline = true;
            this.txtOtherRemarks.Name = "txtOtherRemarks";
            this.txtOtherRemarks.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtOtherRemarks.Size = new System.Drawing.Size(459, 42);
            this.txtOtherRemarks.TabIndex = 20;
            // 
            // label13
            // 
            this.label13.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(3, 367);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(96, 48);
            this.label13.TabIndex = 11;
            this.label13.Text = "Special Instructions if Any";
            // 
            // label39
            // 
            this.label39.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label39.AutoSize = true;
            this.label39.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label39.Location = new System.Drawing.Point(3, 264);
            this.label39.Name = "label39";
            this.label39.Size = new System.Drawing.Size(96, 33);
            this.label39.TabIndex = 0;
            this.label39.Text = "Quot To Submit On";
            // 
            // dgProducts
            // 
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.AliceBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgProducts.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgProducts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.LightSkyBlue;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgProducts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgProducts.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Item_ID,
            this.Item_Code,
            this.Item_Description,
            this.Item_Grade,
            this.Prod_Length,
            this.UOM,
            this.Qty,
            this.Stock_Avialble,
            this.Pending_Order_Qty,
            this.Remarks});
            this.tableLayoutPanel2.SetColumnSpan(this.dgProducts, 8);
            this.dgProducts.EnableHeadersVisualStyles = false;
            this.dgProducts.Location = new System.Drawing.Point(3, 3);
            this.dgProducts.Name = "dgProducts";
            this.dgProducts.RowHeadersWidth = 62;
            this.dgProducts.Size = new System.Drawing.Size(1046, 231);
            this.dgProducts.TabIndex = 11;
            this.dgProducts.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.dgProducts_CellBeginEdit);
            this.dgProducts.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgProducts_CellContentClick);
            this.dgProducts.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgProducts_CellEndEdit);
            this.dgProducts.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgProducts_EditingControlShowing);
            this.dgProducts.KeyDown += new System.Windows.Forms.KeyEventHandler(this.dgProducts_KeyDown);
            // 
            // Item_ID
            // 
            this.Item_ID.DataPropertyName = "Item_ID";
            this.Item_ID.HeaderText = "Item ID";
            this.Item_ID.MinimumWidth = 6;
            this.Item_ID.Name = "Item_ID";
            this.Item_ID.Visible = false;
            // 
            // Item_Code
            // 
            this.Item_Code.DataPropertyName = "Item_Code";
            this.Item_Code.HeaderText = "Item Code";
            this.Item_Code.MinimumWidth = 8;
            this.Item_Code.Name = "Item_Code";
            this.Item_Code.ReadOnly = true;
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
            this.Item_Grade.HeaderText = "Item Grade";
            this.Item_Grade.MinimumWidth = 8;
            this.Item_Grade.Name = "Item_Grade";
            // 
            // Prod_Length
            // 
            this.Prod_Length.DataPropertyName = "Prod_Length";
            this.Prod_Length.HeaderText = "Length";
            this.Prod_Length.MinimumWidth = 8;
            this.Prod_Length.Name = "Prod_Length";
            // 
            // UOM
            // 
            this.UOM.DataPropertyName = "UOM";
            this.UOM.HeaderText = "UOM";
            this.UOM.MinimumWidth = 8;
            this.UOM.Name = "UOM";
            this.UOM.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // Qty
            // 
            this.Qty.DataPropertyName = "Qty";
            this.Qty.HeaderText = "Qty";
            this.Qty.MinimumWidth = 8;
            this.Qty.Name = "Qty";
            // 
            // Stock_Avialble
            // 
            this.Stock_Avialble.DataPropertyName = "Stock_Avialble";
            this.Stock_Avialble.HeaderText = "Stock_Available";
            this.Stock_Avialble.MinimumWidth = 6;
            this.Stock_Avialble.Name = "Stock_Avialble";
            // 
            // Pending_Order_Qty
            // 
            this.Pending_Order_Qty.DataPropertyName = "Pending_Order_Qty";
            this.Pending_Order_Qty.HeaderText = "Pending_Orders";
            this.Pending_Order_Qty.MinimumWidth = 6;
            this.Pending_Order_Qty.Name = "Pending_Order_Qty";
            // 
            // Remarks
            // 
            this.Remarks.DataPropertyName = "Remarks";
            this.Remarks.HeaderText = "Remarks";
            this.Remarks.MinimumWidth = 8;
            this.Remarks.Name = "Remarks";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.label33, 2);
            this.label33.Font = new System.Drawing.Font("Calibri", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label33.ForeColor = System.Drawing.Color.Red;
            this.label33.Location = new System.Drawing.Point(3, 237);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(197, 14);
            this.label33.TabIndex = 7;
            this.label33.Text = "F5: Add New Item ; F6 - Delete Item";
            this.label33.Click += new System.EventHandler(this.label33_Click);
            // 
            // dtpQuoteToSubmit
            // 
            this.dtpQuoteToSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtpQuoteToSubmit.CustomFormat = "dd/MM/yyyy";
            this.dtpQuoteToSubmit.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtpQuoteToSubmit.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpQuoteToSubmit.Location = new System.Drawing.Point(105, 267);
            this.dtpQuoteToSubmit.Name = "dtpQuoteToSubmit";
            this.dtpQuoteToSubmit.ShowUpDown = true;
            this.dtpQuoteToSubmit.Size = new System.Drawing.Size(148, 21);
            this.dtpQuoteToSubmit.TabIndex = 11;
            // 
            // label31
            // 
            this.label31.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label31.AutoSize = true;
            this.label31.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label31.Location = new System.Drawing.Point(3, 454);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(72, 55);
            this.label31.TabIndex = 3;
            this.label31.Text = "Created By";
            this.label31.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblCreatedBy
            // 
            this.lblCreatedBy.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblCreatedBy.AutoSize = true;
            this.lblCreatedBy.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCreatedBy.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblCreatedBy.Location = new System.Drawing.Point(105, 454);
            this.lblCreatedBy.Name = "lblCreatedBy";
            this.lblCreatedBy.Size = new System.Drawing.Size(148, 55);
            this.lblCreatedBy.TabIndex = 4;
            this.lblCreatedBy.TabStop = true;
            this.lblCreatedBy.Text = "linkLabel8";
            this.lblCreatedBy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label30
            // 
            this.label30.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label30.AutoSize = true;
            this.label30.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label30.Location = new System.Drawing.Point(259, 454);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(80, 55);
            this.label30.TabIndex = 5;
            this.label30.Text = "Modified By";
            this.label30.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblModified
            // 
            this.lblModified.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblModified.AutoSize = true;
            this.lblModified.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblModified.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.lblModified.Location = new System.Drawing.Point(383, 454);
            this.lblModified.Name = "lblModified";
            this.lblModified.Size = new System.Drawing.Size(181, 55);
            this.lblModified.TabIndex = 6;
            this.lblModified.TabStop = true;
            this.lblModified.Text = "linkLabel8";
            this.lblModified.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label34
            // 
            this.label34.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label34.AutoSize = true;
            this.label34.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label34.ForeColor = System.Drawing.Color.Red;
            this.label34.Location = new System.Drawing.Point(3, 415);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(96, 39);
            this.label34.TabIndex = 41;
            this.label34.Text = "Status";
            this.label34.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbStatus
            // 
            this.cmbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbStatus.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbStatus.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbStatus.BackColor = System.Drawing.SystemColors.Info;
            this.cmbStatus.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(105, 418);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(148, 25);
            this.cmbStatus.TabIndex = 0;
            // 
            // label21
            // 
            this.label21.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label21.AutoSize = true;
            this.label21.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label21.ForeColor = System.Drawing.Color.Black;
            this.label21.Location = new System.Drawing.Point(3, 297);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(96, 35);
            this.label21.TabIndex = 2;
            this.label21.Text = "Tentative Delivery Date";
            // 
            // dtDeliveryDate
            // 
            this.dtDeliveryDate.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dtDeliveryDate.CustomFormat = "dd/MM/yyyy";
            this.dtDeliveryDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtDeliveryDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtDeliveryDate.Location = new System.Drawing.Point(105, 300);
            this.dtDeliveryDate.Name = "dtDeliveryDate";
            this.dtDeliveryDate.ShowUpDown = true;
            this.dtDeliveryDate.Size = new System.Drawing.Size(148, 21);
            this.dtDeliveryDate.TabIndex = 14;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold);
            this.linkLabel1.Location = new System.Drawing.Point(259, 264);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(118, 33);
            this.linkLabel1.TabIndex = 50;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Sales Region";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // cmbSaleExecutive
            // 
            this.cmbSaleExecutive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSaleExecutive.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbSaleExecutive.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbSaleExecutive.BackColor = System.Drawing.SystemColors.Info;
            this.cmbSaleExecutive.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSaleExecutive.FormattingEnabled = true;
            this.cmbSaleExecutive.Location = new System.Drawing.Point(105, 335);
            this.cmbSaleExecutive.Name = "cmbSaleExecutive";
            this.cmbSaleExecutive.Size = new System.Drawing.Size(148, 23);
            this.cmbSaleExecutive.TabIndex = 17;
            // 
            // chkTPI
            // 
            this.chkTPI.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkTPI.AutoSize = true;
            this.chkTPI.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkTPI.ForeColor = System.Drawing.Color.Red;
            this.chkTPI.Location = new System.Drawing.Point(886, 267);
            this.chkTPI.Name = "chkTPI";
            this.chkTPI.Size = new System.Drawing.Size(163, 27);
            this.chkTPI.TabIndex = 21;
            this.chkTPI.Text = "TPI Involved?";
            this.chkTPI.UseVisualStyleBackColor = true;
            // 
            // chkIBR
            // 
            this.chkIBR.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkIBR.AutoSize = true;
            this.chkIBR.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkIBR.ForeColor = System.Drawing.Color.Red;
            this.chkIBR.Location = new System.Drawing.Point(886, 300);
            this.chkIBR.Name = "chkIBR";
            this.chkIBR.Size = new System.Drawing.Size(163, 29);
            this.chkIBR.TabIndex = 22;
            this.chkIBR.Text = "IBR Certificate Required?";
            this.chkIBR.UseVisualStyleBackColor = true;
            // 
            // cmbPacking
            // 
            this.cmbPacking.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPacking.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbPacking.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cmbPacking.BackColor = System.Drawing.SystemColors.Info;
            this.cmbPacking.Location = new System.Drawing.Point(742, 300);
            this.cmbPacking.Name = "cmbPacking";
            this.cmbPacking.Size = new System.Drawing.Size(138, 23);
            this.cmbPacking.TabIndex = 16;
            this.cmbPacking.Enter += new System.EventHandler(this.cmbPacking_Enter);
            this.cmbPacking.Leave += new System.EventHandler(this.cmbPacking_Leave);
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnClose.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Image = global::ioneNet.Properties.Resources.Close;
            this.btnClose.Location = new System.Drawing.Point(886, 459);
            this.btnClose.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(163, 45);
            this.btnClose.TabIndex = 3;
            this.btnClose.Text = "Close";
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnSave.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSave.Image = global::ioneNet.Properties.Resources.Save;
            this.btnSave.Location = new System.Drawing.Point(742, 459);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(138, 45);
            this.btnSave.TabIndex = 22;
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
            this.btnClear.BackColor = System.Drawing.SystemColors.MenuBar;
            this.btnClear.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Image = global::ioneNet.Properties.Resources.Refresh;
            this.btnClear.Location = new System.Drawing.Point(593, 459);
            this.btnClear.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(143, 45);
            this.btnClear.TabIndex = 4;
            this.btnClear.Text = "Reset";
            this.btnClear.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClear.UseVisualStyleBackColor = false;
            // 
            // txtSalesregion
            // 
            this.txtSalesregion.Enabled = false;
            this.txtSalesregion.Location = new System.Drawing.Point(570, 267);
            this.txtSalesregion.Name = "txtSalesregion";
            this.txtSalesregion.Size = new System.Drawing.Size(17, 23);
            this.txtSalesregion.TabIndex = 53;
            this.txtSalesregion.Visible = false;
            // 
            // txtPayment
            // 
            this.txtPayment.Enabled = false;
            this.txtPayment.Location = new System.Drawing.Point(570, 300);
            this.txtPayment.Name = "txtPayment";
            this.txtPayment.Size = new System.Drawing.Size(17, 23);
            this.txtPayment.TabIndex = 54;
            this.txtPayment.Visible = false;
            // 
            // txtPricebasis
            // 
            this.txtPricebasis.Enabled = false;
            this.txtPricebasis.Location = new System.Drawing.Point(570, 335);
            this.txtPricebasis.Name = "txtPricebasis";
            this.txtPricebasis.Size = new System.Drawing.Size(17, 23);
            this.txtPricebasis.TabIndex = 55;
            this.txtPricebasis.Visible = false;
            // 
            // cmbSaleOffice
            // 
            this.cmbSaleOffice.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbSaleOffice.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbSaleOffice.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cmbSaleOffice.Enabled = false;
            this.cmbSaleOffice.Location = new System.Drawing.Point(383, 267);
            this.cmbSaleOffice.Name = "cmbSaleOffice";
            this.cmbSaleOffice.Size = new System.Drawing.Size(181, 23);
            this.cmbSaleOffice.TabIndex = 12;
            this.cmbSaleOffice.Enter += new System.EventHandler(this.cmbSaleOffice_Enter);
            this.cmbSaleOffice.Leave += new System.EventHandler(this.cmbSaleOffice_Leave);
            // 
            // cmbPaymentterms
            // 
            this.cmbPaymentterms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPaymentterms.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbPaymentterms.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cmbPaymentterms.BackColor = System.Drawing.SystemColors.Info;
            this.cmbPaymentterms.Location = new System.Drawing.Point(383, 300);
            this.cmbPaymentterms.Name = "cmbPaymentterms";
            this.cmbPaymentterms.Size = new System.Drawing.Size(181, 23);
            this.cmbPaymentterms.TabIndex = 15;
            this.cmbPaymentterms.Enter += new System.EventHandler(this.cmbPaymentterms_Enter);
            this.cmbPaymentterms.Leave += new System.EventHandler(this.cmbPaymentterms_Leave);
            // 
            // cmbPricebasis
            // 
            this.cmbPricebasis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbPricebasis.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbPricebasis.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cmbPricebasis.BackColor = System.Drawing.SystemColors.Info;
            this.cmbPricebasis.Location = new System.Drawing.Point(383, 335);
            this.cmbPricebasis.Name = "cmbPricebasis";
            this.cmbPricebasis.Size = new System.Drawing.Size(181, 23);
            this.cmbPricebasis.TabIndex = 18;
            this.cmbPricebasis.Enter += new System.EventHandler(this.cmbPricebasis_Enter);
            this.cmbPricebasis.Leave += new System.EventHandler(this.cmbPricebasis_Leave);
            // 
            // cmbCutting
            // 
            this.cmbCutting.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCutting.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbCutting.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cmbCutting.BackColor = System.Drawing.SystemColors.Info;
            this.cmbCutting.Location = new System.Drawing.Point(742, 335);
            this.cmbCutting.Name = "cmbCutting";
            this.cmbCutting.Size = new System.Drawing.Size(138, 23);
            this.cmbCutting.TabIndex = 19;
            this.cmbCutting.Enter += new System.EventHandler(this.cmbCutting_Enter);
            this.cmbCutting.Leave += new System.EventHandler(this.cmbCutting_Leave);
            // 
            // linkLabel3
            // 
            this.linkLabel3.AutoSize = true;
            this.linkLabel3.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold);
            this.linkLabel3.Location = new System.Drawing.Point(259, 297);
            this.linkLabel3.Name = "linkLabel3";
            this.linkLabel3.Size = new System.Drawing.Size(98, 17);
            this.linkLabel3.TabIndex = 61;
            this.linkLabel3.TabStop = true;
            this.linkLabel3.Text = "Payment Terms";
            this.linkLabel3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel3_LinkClicked);
            this.linkLabel3.Click += new System.EventHandler(this.linkLabel3_Click_1);
            // 
            // linkLabel4
            // 
            this.linkLabel4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel4.AutoSize = true;
            this.linkLabel4.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold);
            this.linkLabel4.Location = new System.Drawing.Point(259, 332);
            this.linkLabel4.Name = "linkLabel4";
            this.linkLabel4.Size = new System.Drawing.Size(118, 35);
            this.linkLabel4.TabIndex = 62;
            this.linkLabel4.TabStop = true;
            this.linkLabel4.Text = "Price Basis";
            this.linkLabel4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel4_LinkClicked);
            this.linkLabel4.Click += new System.EventHandler(this.linkLabel4_Click);
            // 
            // linkLabel5
            // 
            this.linkLabel5.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel5.AutoSize = true;
            this.linkLabel5.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold);
            this.linkLabel5.Location = new System.Drawing.Point(3, 332);
            this.linkLabel5.Name = "linkLabel5";
            this.linkLabel5.Size = new System.Drawing.Size(96, 35);
            this.linkLabel5.TabIndex = 63;
            this.linkLabel5.TabStop = true;
            this.linkLabel5.Text = "Sales Executive";
            this.linkLabel5.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel5_LinkClicked);
            // 
            // linkLabel6
            // 
            this.linkLabel6.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel6.AutoSize = true;
            this.linkLabel6.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold);
            this.linkLabel6.Location = new System.Drawing.Point(593, 264);
            this.linkLabel6.Name = "linkLabel6";
            this.linkLabel6.Size = new System.Drawing.Size(143, 33);
            this.linkLabel6.TabIndex = 64;
            this.linkLabel6.TabStop = true;
            this.linkLabel6.Text = "Delivery Location";
            this.linkLabel6.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel6_LinkClicked);
            // 
            // linkLabel7
            // 
            this.linkLabel7.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel7.AutoSize = true;
            this.linkLabel7.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold);
            this.linkLabel7.Location = new System.Drawing.Point(593, 297);
            this.linkLabel7.Name = "linkLabel7";
            this.linkLabel7.Size = new System.Drawing.Size(143, 35);
            this.linkLabel7.TabIndex = 65;
            this.linkLabel7.TabStop = true;
            this.linkLabel7.Text = "Packing";
            this.linkLabel7.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel7_LinkClicked);
            // 
            // linkLabel8
            // 
            this.linkLabel8.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel8.AutoSize = true;
            this.linkLabel8.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold);
            this.linkLabel8.Location = new System.Drawing.Point(593, 332);
            this.linkLabel8.Name = "linkLabel8";
            this.linkLabel8.Size = new System.Drawing.Size(143, 35);
            this.linkLabel8.TabIndex = 66;
            this.linkLabel8.TabStop = true;
            this.linkLabel8.Text = "Cutting";
            this.linkLabel8.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel8_LinkClicked);
            // 
            // linkLabel9
            // 
            this.linkLabel9.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel9.AutoSize = true;
            this.linkLabel9.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold);
            this.linkLabel9.Location = new System.Drawing.Point(593, 367);
            this.linkLabel9.Name = "linkLabel9";
            this.linkLabel9.Size = new System.Drawing.Size(143, 48);
            this.linkLabel9.TabIndex = 67;
            this.linkLabel9.TabStop = true;
            this.linkLabel9.Text = "UT Required";
            this.linkLabel9.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel9_LinkClicked);
            // 
            // cmbUT
            // 
            this.cmbUT.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbUT.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbUT.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.cmbUT.BackColor = System.Drawing.SystemColors.Info;
            this.cmbUT.Location = new System.Drawing.Point(742, 370);
            this.cmbUT.Name = "cmbUT";
            this.cmbUT.Size = new System.Drawing.Size(138, 23);
            this.cmbUT.TabIndex = 21;
            this.cmbUT.Enter += new System.EventHandler(this.cmbUT_Enter);
            this.cmbUT.Leave += new System.EventHandler(this.cmbUT_Leave);
            // 
            // chkMacro
            // 
            this.chkMacro.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkMacro.AutoSize = true;
            this.chkMacro.Font = new System.Drawing.Font("Calibri", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkMacro.ForeColor = System.Drawing.Color.Red;
            this.chkMacro.Location = new System.Drawing.Point(886, 335);
            this.chkMacro.Name = "chkMacro";
            this.chkMacro.Size = new System.Drawing.Size(163, 29);
            this.chkMacro.TabIndex = 24;
            this.chkMacro.Text = "Macro /Micro Test?";
            this.chkMacro.UseVisualStyleBackColor = true;
            // 
            // txtPackCode
            // 
            this.txtPackCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPackCode.Enabled = false;
            this.txtPackCode.Location = new System.Drawing.Point(570, 240);
            this.txtPackCode.Name = "txtPackCode";
            this.txtPackCode.Size = new System.Drawing.Size(17, 23);
            this.txtPackCode.TabIndex = 52;
            this.txtPackCode.Visible = false;
            // 
            // txtUT
            // 
            this.txtUT.Enabled = false;
            this.txtUT.Location = new System.Drawing.Point(570, 418);
            this.txtUT.Name = "txtUT";
            this.txtUT.Size = new System.Drawing.Size(17, 23);
            this.txtUT.TabIndex = 69;
            this.txtUT.Visible = false;
            // 
            // txtCutting
            // 
            this.txtCutting.Enabled = false;
            this.txtCutting.Location = new System.Drawing.Point(570, 370);
            this.txtCutting.Name = "txtCutting";
            this.txtCutting.Size = new System.Drawing.Size(17, 23);
            this.txtCutting.TabIndex = 56;
            this.txtCutting.Visible = false;
            // 
            // txtCity
            // 
            this.txtCity.BackColor = System.Drawing.SystemColors.Info;
            this.txtCity.Location = new System.Drawing.Point(742, 267);
            this.txtCity.Name = "txtCity";
            this.txtCity.Size = new System.Drawing.Size(138, 23);
            this.txtCity.TabIndex = 70;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ioneNet.Properties.Resources.logoface_Ione;
            this.pictureBox2.Location = new System.Drawing.Point(3, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(45, 40);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 13;
            this.pictureBox2.TabStop = false;
            // 
            // frmNewEnquiry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(1068, 642);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.tableLayoutPanel2);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "frmNewEnquiry";
            this.Text = "frmNewEnqury";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmNewEnquiry_FormClosed);
            this.Load += new System.EventHandler(this.frmNewOrder_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgProducts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtEnqNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox CmbBuyerName;
        private System.Windows.Forms.DateTimePicker dpEnqDate;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.DataGridView dgProducts;
        private System.Windows.Forms.LinkLabel lblCreatedBy;
        private System.Windows.Forms.LinkLabel lblModified;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Label label31;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.Label label34;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.CheckBox chkNew;
        private System.Windows.Forms.TextBox txtCustRefNo;
        private System.Windows.Forms.Label label38;
        private System.Windows.Forms.DateTimePicker dpCustRefDate;
        private System.Windows.Forms.CheckBox chkSEZSupply;
        private System.Windows.Forms.TextBox txtOtherRemarks;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.DateTimePicker dtDeliveryDate;
        private System.Windows.Forms.Label label39;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.DateTimePicker dtpQuoteToSubmit;
        private System.Windows.Forms.ComboBox cmbSaleExecutive;
        private System.Windows.Forms.CheckBox chkExport;
        private System.Windows.Forms.Button btnaddnewcustomer;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private EventHandler button2_Click;
        private System.Windows.Forms.LinkLabel linkLabel2;
        private System.Windows.Forms.TextBox cmbPacking;
        private System.Windows.Forms.TextBox txtPackCode;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.CheckBox chkTPI;
        private System.Windows.Forms.CheckBox chkIBR;
        private System.Windows.Forms.CheckBox chkMacro;
        private System.Windows.Forms.TextBox txtSalesregion;
        private System.Windows.Forms.TextBox txtPayment;
        private System.Windows.Forms.TextBox txtPricebasis;
        private System.Windows.Forms.TextBox txtCutting;
        private System.Windows.Forms.TextBox cmbSaleOffice;
        private System.Windows.Forms.TextBox cmbPaymentterms;
        private System.Windows.Forms.TextBox cmbPricebasis;
        private System.Windows.Forms.TextBox cmbCutting;
        private System.Windows.Forms.LinkLabel linkLabel3;
        private System.Windows.Forms.LinkLabel linkLabel4;
        private System.Windows.Forms.LinkLabel linkLabel5;
        private System.Windows.Forms.LinkLabel linkLabel6;
        private System.Windows.Forms.LinkLabel linkLabel7;
        private System.Windows.Forms.LinkLabel linkLabel8;
        private System.Windows.Forms.LinkLabel linkLabel9;
        private System.Windows.Forms.TextBox cmbUT;
        private System.Windows.Forms.TextBox txtUT;
        private System.Windows.Forms.TextBox txtEnqSource;
        private System.Windows.Forms.TextBox cmbEnqSource;
        private System.Windows.Forms.TextBox txtCity;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item_Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item_Description;
        private System.Windows.Forms.DataGridViewTextBoxColumn Item_Grade;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prod_Length;
        private System.Windows.Forms.DataGridViewTextBoxColumn UOM;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Stock_Avialble;
        private System.Windows.Forms.DataGridViewTextBoxColumn Pending_Order_Qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
    }
}