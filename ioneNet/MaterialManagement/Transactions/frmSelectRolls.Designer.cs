namespace ioneNet.MaterialManagement.Transactions
{
    partial class frmSelectRolls
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
            this.txtItem_Code = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtGRN_No = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.txtTotalQty = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.Button();
            this.dgProductsList = new System.Windows.Forms.DataGridView();
            this.LOT_No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Roll_Width = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty_Kgs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Qty_Issued = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgProductsList)).BeginInit();
            this.SuspendLayout();
            // 
            // txtItem_Code
            // 
            this.txtItem_Code.Location = new System.Drawing.Point(445, 63);
            this.txtItem_Code.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtItem_Code.Name = "txtItem_Code";
            this.txtItem_Code.Size = new System.Drawing.Size(124, 22);
            this.txtItem_Code.TabIndex = 203;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(356, 65);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 16);
            this.label2.TabIndex = 202;
            this.label2.Text = "Item Code";
            // 
            // txtGRN_No
            // 
            this.txtGRN_No.Location = new System.Drawing.Point(123, 62);
            this.txtGRN_No.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtGRN_No.Name = "txtGRN_No";
            this.txtGRN_No.Size = new System.Drawing.Size(204, 22);
            this.txtGRN_No.TabIndex = 200;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(57, 65);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(49, 16);
            this.label1.TabIndex = 199;
            this.label1.Text = "Ref No";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.DarkBlue;
            this.label12.Location = new System.Drawing.Point(72, 31);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(313, 18);
            this.label12.TabIndex = 198;
            this.label12.Text = "SELECT ROLLS TO ISSUE / CONSUME";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ioneNet.Properties.Resources.ione;
            this.pictureBox2.Location = new System.Drawing.Point(4, 0);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(60, 49);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 201;
            this.pictureBox2.TabStop = false;
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btnSubmit.Location = new System.Drawing.Point(539, 343);
            this.btnSubmit.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(100, 28);
            this.btnSubmit.TabIndex = 208;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // txtTotalQty
            // 
            this.txtTotalQty.Enabled = false;
            this.txtTotalQty.Location = new System.Drawing.Point(363, 353);
            this.txtTotalQty.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtTotalQty.Name = "txtTotalQty";
            this.txtTotalQty.Size = new System.Drawing.Size(124, 22);
            this.txtTotalQty.TabIndex = 207;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(280, 356);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 16);
            this.label4.TabIndex = 206;
            this.label4.Text = "Total Qty";
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.PaleTurquoise;
            this.btnClose.Location = new System.Drawing.Point(647, 346);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 28);
            this.btnClose.TabIndex = 205;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
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
            this.Roll_Width,
            this.Qty_Kgs,
            this.Qty_Issued});
            this.dgProductsList.EnableHeadersVisualStyles = false;
            this.dgProductsList.Location = new System.Drawing.Point(16, 95);
            this.dgProductsList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.dgProductsList.Name = "dgProductsList";
            this.dgProductsList.RowHeadersWidth = 51;
            this.dgProductsList.Size = new System.Drawing.Size(731, 241);
            this.dgProductsList.TabIndex = 204;
            this.dgProductsList.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgProductsList_CellEndEdit);
            // 
            // LOT_No
            // 
            this.LOT_No.DataPropertyName = "LOT_No";
            this.LOT_No.HeaderText = "LOT / Roll No";
            this.LOT_No.MinimumWidth = 6;
            this.LOT_No.Name = "LOT_No";
            // 
            // Roll_Width
            // 
            this.Roll_Width.DataPropertyName = "Roll_Width";
            this.Roll_Width.HeaderText = "Roll Width";
            this.Roll_Width.MinimumWidth = 6;
            this.Roll_Width.Name = "Roll_Width";
            // 
            // Qty_Kgs
            // 
            this.Qty_Kgs.DataPropertyName = "Qty_Kgs";
            this.Qty_Kgs.HeaderText = "Qty In Stock (Kgs)";
            this.Qty_Kgs.MinimumWidth = 6;
            this.Qty_Kgs.Name = "Qty_Kgs";
            this.Qty_Kgs.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Qty_Kgs.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Qty_Issued
            // 
            this.Qty_Issued.DataPropertyName = "Qty_Issued";
            this.Qty_Issued.HeaderText = "Qty Issued (Kgs)";
            this.Qty_Issued.MinimumWidth = 6;
            this.Qty_Issued.Name = "Qty_Issued";
            // 
            // frmSelectRolls
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(763, 404);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.txtTotalQty);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.dgProductsList);
            this.Controls.Add(this.txtItem_Code);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.txtGRN_No);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label12);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmSelectRolls";
            this.Text = "frmSelectRolls";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSelectRolls_FormClosed);
            this.Load += new System.EventHandler(this.frmSelectRolls_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgProductsList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtItem_Code;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TextBox txtGRN_No;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.TextBox txtTotalQty;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.DataGridView dgProductsList;
        private System.Windows.Forms.DataGridViewTextBoxColumn LOT_No;
        private System.Windows.Forms.DataGridViewTextBoxColumn Roll_Width;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty_Kgs;
        private System.Windows.Forms.DataGridViewTextBoxColumn Qty_Issued;
    }
}