namespace ioneNet.OrderManagement.Transactions
{
    partial class SO_MultiDeliveryAddress
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
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.txtSONo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.txtItemNo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dgDetails = new System.Windows.Forms.DataGridView();
            this.Consignee_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cust_ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Con_GSTINNO = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Ord_Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtOrdQty = new System.Windows.Forms.TextBox();
            this.txtPlannedQty = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetails)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ioneNet.Properties.Resources.logoface_Ione;
            this.pictureBox2.Location = new System.Drawing.Point(1, 1);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(45, 40);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 193;
            this.pictureBox2.TabStop = false;
            // 
            // txtSONo
            // 
            this.txtSONo.Enabled = false;
            this.txtSONo.Location = new System.Drawing.Point(107, 62);
            this.txtSONo.Name = "txtSONo";
            this.txtSONo.Size = new System.Drawing.Size(103, 23);
            this.txtSONo.TabIndex = 192;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(58, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(42, 15);
            this.label1.TabIndex = 191;
            this.label1.Text = "SO No";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.DarkBlue;
            this.label12.Location = new System.Drawing.Point(52, 26);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(184, 15);
            this.label12.TabIndex = 190;
            this.label12.Text = "MULTIPLE DELIVERY LOCATIONS";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtItemNo
            // 
            this.txtItemNo.Enabled = false;
            this.txtItemNo.Location = new System.Drawing.Point(320, 62);
            this.txtItemNo.Name = "txtItemNo";
            this.txtItemNo.Size = new System.Drawing.Size(86, 23);
            this.txtItemNo.TabIndex = 195;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(216, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(98, 15);
            this.label2.TabIndex = 194;
            this.label2.Text = "Item Code / S No";
            // 
            // dgDetails
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.PaleTurquoise;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgDetails.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDetails.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Consignee_Name,
            this.Cust_ID,
            this.Con_GSTINNO,
            this.Ord_Qty});
            this.dgDetails.EnableHeadersVisualStyles = false;
            this.dgDetails.Location = new System.Drawing.Point(11, 91);
            this.dgDetails.Name = "dgDetails";
            this.dgDetails.Size = new System.Drawing.Size(549, 168);
            this.dgDetails.TabIndex = 196;
            this.dgDetails.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgDetails_CellEndEdit);
            this.dgDetails.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgDetails_EditingControlShowing);
            // 
            // Consignee_Name
            // 
            this.Consignee_Name.DataPropertyName = "Consignee_Name";
            this.Consignee_Name.HeaderText = "Customer Name";
            this.Consignee_Name.Name = "Consignee_Name";
            this.Consignee_Name.Width = 200;
            // 
            // Cust_ID
            // 
            this.Cust_ID.DataPropertyName = "Cust_ID";
            this.Cust_ID.HeaderText = "Cust ID";
            this.Cust_ID.Name = "Cust_ID";
            this.Cust_ID.Width = 75;
            // 
            // Con_GSTINNO
            // 
            this.Con_GSTINNO.DataPropertyName = "Con_GSTINNO";
            this.Con_GSTINNO.HeaderText = "GSTIN No";
            this.Con_GSTINNO.Name = "Con_GSTINNO";
            this.Con_GSTINNO.Width = 150;
            // 
            // Ord_Qty
            // 
            this.Ord_Qty.DataPropertyName = "Ord_Qty";
            this.Ord_Qty.HeaderText = "Qty";
            this.Ord_Qty.Name = "Ord_Qty";
            this.Ord_Qty.Width = 75;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btnClose.Location = new System.Drawing.Point(485, 293);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 198;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btnSubmit.Location = new System.Drawing.Point(404, 294);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 197;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(412, 62);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(26, 15);
            this.label3.TabIndex = 199;
            this.label3.Text = "Qty";
            // 
            // txtOrdQty
            // 
            this.txtOrdQty.Enabled = false;
            this.txtOrdQty.Location = new System.Drawing.Point(444, 62);
            this.txtOrdQty.Name = "txtOrdQty";
            this.txtOrdQty.Size = new System.Drawing.Size(86, 23);
            this.txtOrdQty.TabIndex = 200;
            // 
            // txtPlannedQty
            // 
            this.txtPlannedQty.Enabled = false;
            this.txtPlannedQty.Location = new System.Drawing.Point(444, 264);
            this.txtPlannedQty.Name = "txtPlannedQty";
            this.txtPlannedQty.Size = new System.Drawing.Size(86, 23);
            this.txtPlannedQty.TabIndex = 202;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(412, 264);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(34, 15);
            this.label4.TabIndex = 201;
            this.label4.Text = "Total";
            // 
            // SO_MultiDeliveryAddress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(572, 328);
            this.Controls.Add(this.txtPlannedQty);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtOrdQty);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.dgDetails);
            this.Controls.Add(this.txtItemNo);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.txtSONo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label12);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "SO_MultiDeliveryAddress";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SO_MultiDeliveryAddress";
            this.Load += new System.EventHandler(this.SO_MultiDeliveryAddress_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgDetails)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TextBox txtSONo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtItemNo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DataGridView dgDetails;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtOrdQty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Consignee_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cust_ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Con_GSTINNO;
        private System.Windows.Forms.DataGridViewTextBoxColumn Ord_Qty;
        private System.Windows.Forms.TextBox txtPlannedQty;
        private System.Windows.Forms.Label label4;
    }
}