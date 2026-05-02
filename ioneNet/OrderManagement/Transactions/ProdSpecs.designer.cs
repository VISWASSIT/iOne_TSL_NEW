namespace ioneNet.OrderManagement.Transactions
{
    partial class ProdSpecs
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
            this.btnClose = new System.Windows.Forms.Button();
            this.txtGRN_No = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgProductsList = new System.Windows.Forms.DataGridView();
            this.label12 = new System.Windows.Forms.Label();
            this.txtItem_Code = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtTotalQty = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.LOT_No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty_Kgs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty_Mtrs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgProductsList)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ioneNet.Properties.Resources.logoface_Ione;
            this.pictureBox2.Location = new System.Drawing.Point(-1, 1);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(45, 40);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 195;
            this.pictureBox2.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btnClose.Location = new System.Drawing.Point(485, 287);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 194;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // txtGRN_No
            // 
            this.txtGRN_No.Enabled = false;
            this.txtGRN_No.Location = new System.Drawing.Point(254, 23);
            this.txtGRN_No.Name = "txtGRN_No";
            this.txtGRN_No.Size = new System.Drawing.Size(130, 27);
            this.txtGRN_No.TabIndex = 193;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(198, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 20);
            this.label1.TabIndex = 192;
            this.label1.Text = "Doc No";
            // 
            // dgProductsList
            // 
            this.dgProductsList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.PaleTurquoise;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgProductsList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgProductsList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgProductsList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LOT_No,
            this.Qty_Kgs,
            this.Qty_Mtrs});
            this.dgProductsList.EnableHeadersVisualStyles = false;
            this.dgProductsList.Location = new System.Drawing.Point(12, 83);
            this.dgProductsList.Name = "dgProductsList";
            this.dgProductsList.RowHeadersWidth = 62;
            this.dgProductsList.Size = new System.Drawing.Size(548, 196);
            this.dgProductsList.TabIndex = 191;
            this.dgProductsList.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgProductsList_CellContentClick);
            this.dgProductsList.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgProductsList_CellEndEdit);
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.DarkBlue;
            this.label12.Location = new System.Drawing.Point(50, 26);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(189, 20);
            this.label12.TabIndex = 190;
            this.label12.Text = "ENTER BLOOM WISE DATA";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtItem_Code
            // 
            this.txtItem_Code.Enabled = false;
            this.txtItem_Code.Location = new System.Drawing.Point(254, 52);
            this.txtItem_Code.Name = "txtItem_Code";
            this.txtItem_Code.Size = new System.Drawing.Size(94, 27);
            this.txtItem_Code.TabIndex = 197;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(187, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 20);
            this.label2.TabIndex = 196;
            this.label2.Text = "Item Code";
            // 
            // txtTotalQty
            // 
            this.txtTotalQty.Enabled = false;
            this.txtTotalQty.Location = new System.Drawing.Point(272, 293);
            this.txtTotalQty.Name = "txtTotalQty";
            this.txtTotalQty.Size = new System.Drawing.Size(94, 27);
            this.txtTotalQty.TabIndex = 201;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(210, 295);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 20);
            this.label4.TabIndex = 200;
            this.label4.Text = "Total Qty";
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btnSubmit.Location = new System.Drawing.Point(404, 285);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 202;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(432, 51);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(60, 27);
            this.textBox1.TabIndex = 204;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(365, 53);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 20);
            this.label3.TabIndex = 203;
            this.label3.Text = "No Of Blooms";
            // 
            // textBox2
            // 
            this.textBox2.Enabled = false;
            this.textBox2.Location = new System.Drawing.Point(144, 293);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(60, 27);
            this.textBox2.TabIndex = 206;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(77, 295);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(104, 20);
            this.label5.TabIndex = 205;
            this.label5.Text = "No Of Blooms";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(432, 23);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(100, 27);
            this.textBox3.TabIndex = 207;
            // 
            // LOT_No
            // 
            this.LOT_No.DataPropertyName = "LOT_No";
            this.LOT_No.HeaderText = "Bloom_No";
            this.LOT_No.MinimumWidth = 8;
            this.LOT_No.Name = "LOT_No";
            // 
            // Qty_Kgs
            // 
            this.Qty_Kgs.DataPropertyName = "Qty_Kgs";
            this.Qty_Kgs.HeaderText = "Qty in Kgs";
            this.Qty_Kgs.MinimumWidth = 8;
            this.Qty_Kgs.Name = "Qty_Kgs";
            this.Qty_Kgs.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Qty_Kgs.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Qty_Mtrs
            // 
            this.Qty_Mtrs.DataPropertyName = "Qty_Mtrs";
            this.Qty_Mtrs.HeaderText = "Length in Mtrs";
            this.Qty_Mtrs.MinimumWidth = 8;
            this.Qty_Mtrs.Name = "Qty_Mtrs";
            // 
            // ProdSpecs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(572, 328);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.txtTotalQty);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtItem_Code);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtGRN_No);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgProductsList);
            this.Controls.Add(this.label12);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "ProdSpecs";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Bloom Wise Data";
            this.Load += new System.EventHandler(this.ProdSpecs_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgProductsList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtGRN_No;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgProductsList;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtItem_Code;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtTotalQty;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.DataGridViewTextBoxColumn LOT_No;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty_Kgs;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty_Mtrs;
    }
}