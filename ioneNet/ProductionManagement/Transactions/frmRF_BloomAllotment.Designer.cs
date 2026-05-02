namespace ioneNet.ProductionManagement.Transactions
{
    partial class frmRF_BloomAllotment
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmRF_BloomAllotment));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label12 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.multiSelectionComboBox1 = new Syncfusion.Windows.Forms.Tools.MultiSelectionComboBox();
            this.grdBloomData = new System.Windows.Forms.DataGridView();
            this.RM_Sec = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.RollNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Bal_Length = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CutLength = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Cut_Qty = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Remarks = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.button1 = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.txtQty = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtItemCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMONo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmdBrowseDrawing = new System.Windows.Forms.Button();
            this.btnSubmit = new Syncfusion.WinForms.Controls.SfButton();
            this.btnClose = new Syncfusion.WinForms.Controls.SfButton();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.multiSelectionComboBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBloomData)).BeginInit();
            this.SuspendLayout();
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.DarkBlue;
            this.label12.Location = new System.Drawing.Point(71, 32);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(130, 23);
            this.label12.TabIndex = 192;
            this.label12.Text = "ALLOT BLOOMS";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ioneNet.Properties.Resources.logoface_Ione;
            this.pictureBox2.Location = new System.Drawing.Point(3, 4);
            this.pictureBox2.Margin = new System.Windows.Forms.Padding(4);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(60, 49);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 193;
            this.pictureBox2.TabStop = false;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkBox1);
            this.groupBox1.Controls.Add(this.multiSelectionComboBox1);
            this.groupBox1.Controls.Add(this.grdBloomData);
            this.groupBox1.Controls.Add(this.button1);
            this.groupBox1.Controls.Add(this.linkLabel1);
            this.groupBox1.Controls.Add(this.txtQty);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtItemCode);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtMONo);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(16, 60);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(888, 353);
            this.groupBox1.TabIndex = 194;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Basic Details";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(605, 63);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(126, 24);
            this.checkBox1.TabIndex = 199;
            this.checkBox1.Text = "Round Shape?";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // multiSelectionComboBox1
            // 
            this.multiSelectionComboBox1.BeforeTouchSize = new System.Drawing.Size(278, 30);
            this.multiSelectionComboBox1.ButtonStyle = Syncfusion.Windows.Forms.ButtonAppearance.Metro;
            this.multiSelectionComboBox1.DataSource = ((object)(resources.GetObject("multiSelectionComboBox1.DataSource")));
            this.multiSelectionComboBox1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.multiSelectionComboBox1.Location = new System.Drawing.Point(192, 60);
            this.multiSelectionComboBox1.Margin = new System.Windows.Forms.Padding(4);
            this.multiSelectionComboBox1.Name = "multiSelectionComboBox1";
            this.multiSelectionComboBox1.Size = new System.Drawing.Size(278, 30);
            this.multiSelectionComboBox1.TabIndex = 198;
            this.multiSelectionComboBox1.ThemeName = "Metro";
            this.multiSelectionComboBox1.UseVisualStyle = true;
            // 
            // grdBloomData
            // 
            this.grdBloomData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSkyBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdBloomData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grdBloomData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdBloomData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.RM_Sec,
            this.RollNo,
            this.Bal_Length,
            this.CutLength,
            this.Cut_Qty,
            this.Remarks});
            this.grdBloomData.EnableHeadersVisualStyles = false;
            this.grdBloomData.Location = new System.Drawing.Point(13, 98);
            this.grdBloomData.Margin = new System.Windows.Forms.Padding(4);
            this.grdBloomData.Name = "grdBloomData";
            this.grdBloomData.RowHeadersWidth = 62;
            this.grdBloomData.Size = new System.Drawing.Size(855, 247);
            this.grdBloomData.TabIndex = 197;
            this.grdBloomData.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdBloomData_CellEndEdit);
            // 
            // RM_Sec
            // 
            this.RM_Sec.DataPropertyName = "RM_Sec";
            this.RM_Sec.HeaderText = "Section";
            this.RM_Sec.MinimumWidth = 8;
            this.RM_Sec.Name = "RM_Sec";
            // 
            // RollNo
            // 
            this.RollNo.DataPropertyName = "RollNo";
            this.RollNo.HeaderText = "RF No";
            this.RollNo.MinimumWidth = 8;
            this.RollNo.Name = "RollNo";
            // 
            // Bal_Length
            // 
            this.Bal_Length.DataPropertyName = "Bal_Length";
            this.Bal_Length.HeaderText = "Stock Length";
            this.Bal_Length.MinimumWidth = 8;
            this.Bal_Length.Name = "Bal_Length";
            // 
            // CutLength
            // 
            this.CutLength.DataPropertyName = "CutLength";
            this.CutLength.HeaderText = "Cut Length";
            this.CutLength.MinimumWidth = 8;
            this.CutLength.Name = "CutLength";
            // 
            // Cut_Qty
            // 
            this.Cut_Qty.DataPropertyName = "Cut_Qty";
            this.Cut_Qty.HeaderText = "Cut Qty";
            this.Cut_Qty.MinimumWidth = 8;
            this.Cut_Qty.Name = "Cut_Qty";
            // 
            // Remarks
            // 
            this.Remarks.DataPropertyName = "Remarks";
            this.Remarks.HeaderText = "Remarks";
            this.Remarks.MinimumWidth = 8;
            this.Remarks.Name = "Remarks";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(755, 60);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 28);
            this.button1.TabIndex = 196;
            this.button1.Text = "Get RF Data";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(60, 60);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(84, 20);
            this.linkLabel1.TabIndex = 29;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "RM Section";
            // 
            // txtQty
            // 
            this.txtQty.Enabled = false;
            this.txtQty.Location = new System.Drawing.Point(723, 25);
            this.txtQty.Margin = new System.Windows.Forms.Padding(4);
            this.txtQty.Name = "txtQty";
            this.txtQty.Size = new System.Drawing.Size(135, 27);
            this.txtQty.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(559, 25);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(89, 20);
            this.label4.TabIndex = 6;
            this.label4.Text = "Planned Qty";
            // 
            // txtItemCode
            // 
            this.txtItemCode.Enabled = false;
            this.txtItemCode.Location = new System.Drawing.Point(423, 25);
            this.txtItemCode.Margin = new System.Windows.Forms.Padding(4);
            this.txtItemCode.Name = "txtItemCode";
            this.txtItemCode.Size = new System.Drawing.Size(129, 27);
            this.txtItemCode.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(336, 25);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 20);
            this.label2.TabIndex = 2;
            this.label2.Text = "Item Code";
            // 
            // txtMONo
            // 
            this.txtMONo.Enabled = false;
            this.txtMONo.Location = new System.Drawing.Point(192, 25);
            this.txtMONo.Margin = new System.Windows.Forms.Padding(4);
            this.txtMONo.Name = "txtMONo";
            this.txtMONo.Size = new System.Drawing.Size(135, 27);
            this.txtMONo.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 25);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(114, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "MO No / SO No";
            // 
            // cmdBrowseDrawing
            // 
            this.cmdBrowseDrawing.Location = new System.Drawing.Point(799, 144);
            this.cmdBrowseDrawing.Margin = new System.Windows.Forms.Padding(4);
            this.cmdBrowseDrawing.Name = "cmdBrowseDrawing";
            this.cmdBrowseDrawing.Size = new System.Drawing.Size(176, 28);
            this.cmdBrowseDrawing.TabIndex = 27;
            this.cmdBrowseDrawing.Text = "Add /Modify ";
            this.cmdBrowseDrawing.UseVisualStyleBackColor = true;
            this.cmdBrowseDrawing.Visible = false;
            // 
            // btnSubmit
            // 
            this.btnSubmit.AccessibleName = "Button";
            this.btnSubmit.BackColor = System.Drawing.Color.Turquoise;
            this.btnSubmit.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnSubmit.Location = new System.Drawing.Point(683, 421);
            this.btnSubmit.Margin = new System.Windows.Forms.Padding(4);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(92, 34);
            this.btnSubmit.Style.BackColor = System.Drawing.Color.Turquoise;
            this.btnSubmit.TabIndex = 195;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnClose
            // 
            this.btnClose.AccessibleName = "Button";
            this.btnClose.BackColor = System.Drawing.Color.Turquoise;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnClose.Location = new System.Drawing.Point(783, 421);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(101, 34);
            this.btnClose.Style.BackColor = System.Drawing.Color.Turquoise;
            this.btnClose.TabIndex = 196;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmRF_BloomAllotment
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(916, 460);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmdBrowseDrawing);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.label12);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmRF_BloomAllotment";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmRF_BloomAllotment";
            this.Load += new System.EventHandler(this.frmRF_BloomAllotment_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.multiSelectionComboBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdBloomData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView grdBloomData;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Button cmdBrowseDrawing;
        private System.Windows.Forms.TextBox txtQty;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtItemCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMONo;
        private System.Windows.Forms.Label label1;
        private Syncfusion.WinForms.Controls.SfButton btnSubmit;
        private Syncfusion.WinForms.Controls.SfButton btnClose;
        private Syncfusion.Windows.Forms.Tools.MultiSelectionComboBox multiSelectionComboBox1;
        private System.Windows.Forms.DataGridViewTextBoxColumn RM_Sec;
        private System.Windows.Forms.DataGridViewTextBoxColumn RollNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn Bal_Length;
        private System.Windows.Forms.DataGridViewTextBoxColumn CutLength;
        private System.Windows.Forms.DataGridViewTextBoxColumn Cut_Qty;
        private System.Windows.Forms.DataGridViewTextBoxColumn Remarks;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}