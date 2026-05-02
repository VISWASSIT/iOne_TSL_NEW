namespace ioneNet.OrderManagement.Masters
{
    partial class frmattributesdata
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.txtattrID = new System.Windows.Forms.Label();
            this.txtattrdatadesc = new System.Windows.Forms.Label();
            this.txtheadname = new System.Windows.Forms.Label();
            this.txtID = new System.Windows.Forms.TextBox();
            this.txtattrheadname = new System.Windows.Forms.TextBox();
            this.txtattrdesc = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.dgattrdata = new System.Windows.Forms.DataGridView();
            this.ID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Descr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Head_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.linkModifiedBy = new System.Windows.Forms.LinkLabel();
            this.linkCreatedBy = new System.Windows.Forms.LinkLabel();
            this.label8 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgattrdata)).BeginInit();
            this.SuspendLayout();
            // 
            // txtattrID
            // 
            this.txtattrID.AutoSize = true;
            this.txtattrID.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtattrID.Location = new System.Drawing.Point(523, 109);
            this.txtattrID.Name = "txtattrID";
            this.txtattrID.Size = new System.Drawing.Size(22, 18);
            this.txtattrID.TabIndex = 0;
            this.txtattrID.Text = "ID";
            // 
            // txtattrdatadesc
            // 
            this.txtattrdatadesc.AutoSize = true;
            this.txtattrdatadesc.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtattrdatadesc.Location = new System.Drawing.Point(462, 137);
            this.txtattrdatadesc.Name = "txtattrdatadesc";
            this.txtattrdatadesc.Size = new System.Drawing.Size(83, 18);
            this.txtattrdatadesc.TabIndex = 1;
            this.txtattrdatadesc.Text = "Description";
            // 
            // txtheadname
            // 
            this.txtheadname.AutoSize = true;
            this.txtheadname.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.txtheadname.Location = new System.Drawing.Point(458, 170);
            this.txtheadname.Name = "txtheadname";
            this.txtheadname.Size = new System.Drawing.Size(87, 18);
            this.txtheadname.TabIndex = 2;
            this.txtheadname.Text = "Head Name";
            // 
            // txtID
            // 
            this.txtID.Location = new System.Drawing.Point(551, 109);
            this.txtID.Name = "txtID";
            this.txtID.ReadOnly = true;
            this.txtID.Size = new System.Drawing.Size(100, 22);
            this.txtID.TabIndex = 3;
            // 
            // txtattrheadname
            // 
            this.txtattrheadname.Location = new System.Drawing.Point(551, 170);
            this.txtattrheadname.Name = "txtattrheadname";
            this.txtattrheadname.ReadOnly = true;
            this.txtattrheadname.Size = new System.Drawing.Size(194, 22);
            this.txtattrheadname.TabIndex = 4;
            // 
            // txtattrdesc
            // 
            this.txtattrdesc.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.txtattrdesc.Location = new System.Drawing.Point(551, 137);
            this.txtattrdesc.Name = "txtattrdesc";
            this.txtattrdesc.Size = new System.Drawing.Size(194, 22);
            this.txtattrdesc.TabIndex = 5;
            this.txtattrdesc.Leave += new System.EventHandler(this.txtattrdesc_Leave);
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold);
            this.label12.ForeColor = System.Drawing.Color.DarkBlue;
            this.label12.Location = new System.Drawing.Point(59, 33);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(132, 20);
            this.label12.TabIndex = 182;
            this.label12.Text = "ATTRIBUTES DATA";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.label12.Click += new System.EventHandler(this.label12_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ioneNet.Properties.Resources.logoface_Ione;
            this.pictureBox2.Location = new System.Drawing.Point(1, 2);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(52, 46);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 181;
            this.pictureBox2.TabStop = false;
            this.pictureBox2.Click += new System.EventHandler(this.pictureBox2_Click);
            // 
            // dgattrdata
            // 
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.White;
            this.dgattrdata.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgattrdata.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.LightSkyBlue;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgattrdata.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle5;
            this.dgattrdata.ColumnHeadersHeight = 25;
            this.dgattrdata.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.dgattrdata.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ID,
            this.Descr,
            this.Head_Name});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.Silver;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgattrdata.DefaultCellStyle = dataGridViewCellStyle6;
            this.dgattrdata.EnableHeadersVisualStyles = false;
            this.dgattrdata.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(142)))), ((int)(((byte)(215)))));
            this.dgattrdata.Location = new System.Drawing.Point(38, 83);
            this.dgattrdata.Name = "dgattrdata";
            this.dgattrdata.RowHeadersVisible = false;
            this.dgattrdata.RowHeadersWidth = 51;
            this.dgattrdata.Size = new System.Drawing.Size(402, 235);
            this.dgattrdata.TabIndex = 183;
            this.dgattrdata.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgattrdata_CellDoubleClick);
            // 
            // ID
            // 
            this.ID.DataPropertyName = "ID";
            this.ID.FillWeight = 103.5533F;
            this.ID.Frozen = true;
            this.ID.HeaderText = "ID";
            this.ID.MinimumWidth = 6;
            this.ID.Name = "ID";
            this.ID.Width = 53;
            // 
            // Descr
            // 
            this.Descr.DataPropertyName = "Descr";
            this.Descr.FillWeight = 96.44669F;
            this.Descr.Frozen = true;
            this.Descr.HeaderText = "Description";
            this.Descr.MinimumWidth = 6;
            this.Descr.Name = "Descr";
            this.Descr.Width = 116;
            // 
            // Head_Name
            // 
            this.Head_Name.DataPropertyName = "Head_Name";
            this.Head_Name.HeaderText = "Head Name";
            this.Head_Name.MinimumWidth = 6;
            this.Head_Name.Name = "Head_Name";
            this.Head_Name.Width = 119;
            // 
            // btnDelete
            // 
            this.btnDelete.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnDelete.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnDelete.ForeColor = System.Drawing.Color.Black;
            this.btnDelete.Location = new System.Drawing.Point(445, 352);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(87, 29);
            this.btnDelete.TabIndex = 185;
            this.btnDelete.Text = "Delete";
            this.btnDelete.UseVisualStyleBackColor = false;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnClose.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.ForeColor = System.Drawing.Color.Black;
            this.btnClose.Location = new System.Drawing.Point(539, 352);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 29);
            this.btnClose.TabIndex = 186;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnSubmit
            // 
            this.btnSubmit.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnSubmit.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnSubmit.ForeColor = System.Drawing.Color.Black;
            this.btnSubmit.Location = new System.Drawing.Point(353, 352);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(87, 29);
            this.btnSubmit.TabIndex = 184;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = false;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // linkModifiedBy
            // 
            this.linkModifiedBy.AutoSize = true;
            this.linkModifiedBy.ForeColor = System.Drawing.Color.Gray;
            this.linkModifiedBy.Location = new System.Drawing.Point(446, 425);
            this.linkModifiedBy.Name = "linkModifiedBy";
            this.linkModifiedBy.Size = new System.Drawing.Size(76, 16);
            this.linkModifiedBy.TabIndex = 190;
            this.linkModifiedBy.TabStop = true;
            this.linkModifiedBy.Text = "User Name";
            // 
            // linkCreatedBy
            // 
            this.linkCreatedBy.AutoSize = true;
            this.linkCreatedBy.ForeColor = System.Drawing.Color.Gray;
            this.linkCreatedBy.Location = new System.Drawing.Point(115, 425);
            this.linkCreatedBy.Name = "linkCreatedBy";
            this.linkCreatedBy.Size = new System.Drawing.Size(76, 16);
            this.linkCreatedBy.TabIndex = 189;
            this.linkCreatedBy.TabStop = true;
            this.linkCreatedBy.Text = "User Name";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.Color.Gray;
            this.label8.Location = new System.Drawing.Point(367, 425);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 16);
            this.label8.TabIndex = 188;
            this.label8.Text = "Modified By";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.Color.Gray;
            this.label10.Location = new System.Drawing.Point(43, 425);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 16);
            this.label10.TabIndex = 187;
            this.label10.Text = "Created By";
            // 
            // frmattributesdata
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(786, 450);
            this.Controls.Add(this.linkModifiedBy);
            this.Controls.Add(this.linkCreatedBy);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.btnDelete);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.dgattrdata);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.txtattrdesc);
            this.Controls.Add(this.txtattrheadname);
            this.Controls.Add(this.txtID);
            this.Controls.Add(this.txtheadname);
            this.Controls.Add(this.txtattrdatadesc);
            this.Controls.Add(this.txtattrID);
            this.Name = "frmattributesdata";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmattributesdata";
            this.Load += new System.EventHandler(this.frmattributesdata_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmattributesdata_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgattrdata)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label txtattrID;
        private System.Windows.Forms.Label txtattrdatadesc;
        private System.Windows.Forms.Label txtheadname;
        private System.Windows.Forms.TextBox txtID;
        private System.Windows.Forms.TextBox txtattrheadname;
        private System.Windows.Forms.TextBox txtattrdesc;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.DataGridView dgattrdata;
        private System.Windows.Forms.DataGridViewTextBoxColumn ID;
        private System.Windows.Forms.DataGridViewTextBoxColumn Descr;
        private System.Windows.Forms.DataGridViewTextBoxColumn Head_Name;
        internal System.Windows.Forms.Button btnDelete;
        internal System.Windows.Forms.Button btnClose;
        internal System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.LinkLabel linkModifiedBy;
        private System.Windows.Forms.LinkLabel linkCreatedBy;
        internal System.Windows.Forms.Label label8;
        internal System.Windows.Forms.Label label10;
    }
}