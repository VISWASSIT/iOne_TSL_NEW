namespace ioneNet.ProductionManagement.Transactions
{
    partial class frmProdSectionWiseLengths
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtTotalQtyLengthwise = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtItemName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtReportNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.dgProductsList = new System.Windows.Forms.DataGridView();
            this.Planned_Length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Planned_Qty_Nos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Length_In_Mtrs = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Prod_Length_To_Claculate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Produced_Qty_Nos = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Produced_Qty_MT = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label12 = new System.Windows.Forms.Label();
            this.txtItemCode = new System.Windows.Forms.TextBox();
            this.txtTotQtyFinished = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtSecWt = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.txtgrade = new System.Windows.Forms.TextBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            ((System.ComponentModel.ISupportInitialize)(this.dgProductsList)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // txtTotalQtyLengthwise
            // 
            this.txtTotalQtyLengthwise.Enabled = false;
            this.txtTotalQtyLengthwise.Location = new System.Drawing.Point(496, 462);
            this.txtTotalQtyLengthwise.Name = "txtTotalQtyLengthwise";
            this.txtTotalQtyLengthwise.Size = new System.Drawing.Size(94, 22);
            this.txtTotalQtyLengthwise.TabIndex = 217;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(42, 462);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(131, 20);
            this.label4.TabIndex = 216;
            this.label4.Text = "Total Finished Qty";
            // 
            // txtItemName
            // 
            this.txtItemName.Enabled = false;
            this.txtItemName.Location = new System.Drawing.Point(155, 91);
            this.txtItemName.Name = "txtItemName";
            this.txtItemName.Size = new System.Drawing.Size(255, 22);
            this.txtItemName.TabIndex = 215;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(28, 93);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 20);
            this.label2.TabIndex = 214;
            this.label2.Text = "Item Description";
            // 
            // txtReportNo
            // 
            this.txtReportNo.Enabled = false;
            this.txtReportNo.Location = new System.Drawing.Point(155, 63);
            this.txtReportNo.Name = "txtReportNo";
            this.txtReportNo.Size = new System.Drawing.Size(130, 22);
            this.txtReportNo.TabIndex = 211;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(32, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 20);
            this.label1.TabIndex = 210;
            this.label1.Text = "Prod Reprt Ref.";
            // 
            // dgProductsList
            // 
            this.dgProductsList.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.LightSkyBlue;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgProductsList.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgProductsList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgProductsList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Planned_Length,
            this.Planned_Qty_Nos,
            this.Length_In_Mtrs,
            this.Prod_Length_To_Claculate,
            this.Produced_Qty_Nos,
            this.Produced_Qty_MT});
            this.dgProductsList.EnableHeadersVisualStyles = false;
            this.dgProductsList.Location = new System.Drawing.Point(12, 133);
            this.dgProductsList.Name = "dgProductsList";
            this.dgProductsList.RowHeadersWidth = 62;
            this.dgProductsList.Size = new System.Drawing.Size(909, 292);
            this.dgProductsList.TabIndex = 209;
            this.dgProductsList.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgProductsList_CellEndEdit);
            this.dgProductsList.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgProductsList_EditingControlShowing);
            // 
            // Planned_Length
            // 
            this.Planned_Length.DataPropertyName = "Planned_Length";
            this.Planned_Length.HeaderText = "Planned_Length";
            this.Planned_Length.MinimumWidth = 6;
            this.Planned_Length.Name = "Planned_Length";
            // 
            // Planned_Qty_Nos
            // 
            this.Planned_Qty_Nos.DataPropertyName = "Planned_Qty_Nos";
            this.Planned_Qty_Nos.HeaderText = "Planned_Nos";
            this.Planned_Qty_Nos.MinimumWidth = 6;
            this.Planned_Qty_Nos.Name = "Planned_Qty_Nos";
            // 
            // Length_In_Mtrs
            // 
            this.Length_In_Mtrs.DataPropertyName = "Length_In_Mtrs";
            this.Length_In_Mtrs.HeaderText = "Produced_Length";
            this.Length_In_Mtrs.MinimumWidth = 8;
            this.Length_In_Mtrs.Name = "Length_In_Mtrs";
            // 
            // Prod_Length_To_Claculate
            // 
            this.Prod_Length_To_Claculate.DataPropertyName = "Prod_Length_To_Claculate";
            this.Prod_Length_To_Claculate.HeaderText = "Length_For_Wt";
            this.Prod_Length_To_Claculate.MinimumWidth = 6;
            this.Prod_Length_To_Claculate.Name = "Prod_Length_To_Claculate";
            // 
            // Produced_Qty_Nos
            // 
            this.Produced_Qty_Nos.DataPropertyName = "Produced_Qty_Nos";
            this.Produced_Qty_Nos.HeaderText = "Produced_Nos";
            this.Produced_Qty_Nos.MinimumWidth = 6;
            this.Produced_Qty_Nos.Name = "Produced_Qty_Nos";
            // 
            // Produced_Qty_MT
            // 
            this.Produced_Qty_MT.DataPropertyName = "Produced_Qty_MT";
            this.Produced_Qty_MT.HeaderText = "Produced_MT";
            this.Produced_Qty_MT.MinimumWidth = 8;
            this.Produced_Qty_MT.Name = "Produced_Qty_MT";
            this.Produced_Qty_MT.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Produced_Qty_MT.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.DarkBlue;
            this.label12.Location = new System.Drawing.Point(52, 21);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(441, 22);
            this.label12.TabIndex = 208;
            this.label12.Text = "ENTER SECTION WISE LENGTHS PRODUCED";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtItemCode
            // 
            this.txtItemCode.Enabled = false;
            this.txtItemCode.Location = new System.Drawing.Point(416, 91);
            this.txtItemCode.Name = "txtItemCode";
            this.txtItemCode.Size = new System.Drawing.Size(96, 22);
            this.txtItemCode.TabIndex = 223;
            // 
            // txtTotQtyFinished
            // 
            this.txtTotQtyFinished.Enabled = false;
            this.txtTotQtyFinished.Location = new System.Drawing.Point(205, 460);
            this.txtTotQtyFinished.Name = "txtTotQtyFinished";
            this.txtTotQtyFinished.Size = new System.Drawing.Size(94, 22);
            this.txtTotQtyFinished.TabIndex = 224;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(333, 462);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(157, 20);
            this.label3.TabIndex = 225;
            this.label3.Text = "Total Length Wise Qty";
            // 
            // txtSecWt
            // 
            this.txtSecWt.Enabled = false;
            this.txtSecWt.Location = new System.Drawing.Point(804, 91);
            this.txtSecWt.Name = "txtSecWt";
            this.txtSecWt.Size = new System.Drawing.Size(96, 22);
            this.txtSecWt.TabIndex = 226;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(706, 91);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(82, 20);
            this.label5.TabIndex = 227;
            this.label5.Text = "Section Wt";
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.White;
            this.btnSubmit.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmit.Image = global::ioneNet.Properties.Resources.Apply;
            this.btnSubmit.Location = new System.Drawing.Point(656, 460);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(132, 45);
            this.btnSubmit.TabIndex = 218;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ioneNet.Properties.Resources.logoface_Ione;
            this.pictureBox2.Location = new System.Drawing.Point(1, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(45, 40);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 213;
            this.pictureBox2.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.White;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Image = global::ioneNet.Properties.Resources.Close;
            this.btnClose.Location = new System.Drawing.Point(794, 460);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(127, 45);
            this.btnClose.TabIndex = 212;
            this.btnClose.Text = "Close";
            this.btnClose.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnClose.UseVisualStyleBackColor = false;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(518, 91);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(50, 20);
            this.label6.TabIndex = 229;
            this.label6.Text = "Grade";
            // 
            // txtgrade
            // 
            this.txtgrade.Enabled = false;
            this.txtgrade.Location = new System.Drawing.Point(574, 91);
            this.txtgrade.Name = "txtgrade";
            this.txtgrade.Size = new System.Drawing.Size(126, 22);
            this.txtgrade.TabIndex = 228;
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dateTimePicker1.Location = new System.Drawing.Point(321, 62);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(125, 22);
            this.dateTimePicker1.TabIndex = 230;
            this.dateTimePicker1.Visible = false;
            // 
            // frmProdSectionWiseLengths
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(933, 517);
            this.Controls.Add(this.dateTimePicker1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.txtgrade);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtSecWt);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTotQtyFinished);
            this.Controls.Add(this.txtItemCode);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.txtTotalQtyLengthwise);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.txtItemName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.txtReportNo);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgProductsList);
            this.Controls.Add(this.label12);
            this.Name = "frmProdSectionWiseLengths";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmProdSectionWiseLengths";
            this.Load += new System.EventHandler(this.frmProdSectionWiseLengths_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgProductsList)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.TextBox txtTotalQtyLengthwise;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtItemName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.TextBox txtReportNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView dgProductsList;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtItemCode;
        private System.Windows.Forms.TextBox txtTotQtyFinished;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtSecWt;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Planned_Length;
        private System.Windows.Forms.DataGridViewTextBoxColumn Planned_Qty_Nos;
        private System.Windows.Forms.DataGridViewTextBoxColumn Length_In_Mtrs;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prod_Length_To_Claculate;
        private System.Windows.Forms.DataGridViewTextBoxColumn Produced_Qty_Nos;
        private System.Windows.Forms.DataGridViewTextBoxColumn Produced_Qty_MT;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtgrade;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
    }
}