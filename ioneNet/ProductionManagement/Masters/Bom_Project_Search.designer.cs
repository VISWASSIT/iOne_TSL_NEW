namespace ioneNet.ProductionManagement.Masters
{
    partial class Bom_Project_Search
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
            this.dgvBOMData = new System.Windows.Forms.DataGridView();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.btnFind = new System.Windows.Forms.Button();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.MO_No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Project_Id = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Project_Code = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Prod_Group_Name = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Module_No = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBOMData)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvBOMData
            // 
            this.dgvBOMData.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.LightSkyBlue;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBOMData.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvBOMData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBOMData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.MO_No,
            this.Project_Id,
            this.Project_Code,
            this.Prod_Group_Name,
            this.Module_No});
            this.dgvBOMData.EnableHeadersVisualStyles = false;
            this.dgvBOMData.Location = new System.Drawing.Point(2, 69);
            this.dgvBOMData.Name = "dgvBOMData";
            this.dgvBOMData.RowHeadersWidth = 51;
            this.dgvBOMData.Size = new System.Drawing.Size(973, 473);
            this.dgvBOMData.TabIndex = 1;
            this.dgvBOMData.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBOMData_CellContentClick);
            this.dgvBOMData.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellDoubleClick);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ioneNet.Properties.Resources.logoface_Ione;
            this.pictureBox2.Location = new System.Drawing.Point(2, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(52, 46);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 195;
            this.pictureBox2.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnClose.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(585, 25);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(87, 27);
            this.btnClose.TabIndex = 194;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // btnFind
            // 
            this.btnFind.BackColor = System.Drawing.Color.LightSkyBlue;
            this.btnFind.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFind.Location = new System.Drawing.Point(490, 26);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(87, 27);
            this.btnFind.TabIndex = 193;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = false;
            // 
            // txtSearch
            // 
            this.txtSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSearch.Location = new System.Drawing.Point(234, 26);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(250, 24);
            this.txtSearch.TabIndex = 192;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(177, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 18);
            this.label1.TabIndex = 191;
            this.label1.Text = "Search";
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.ForeColor = System.Drawing.Color.DarkBlue;
            this.label12.Location = new System.Drawing.Point(60, 29);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(102, 20);
            this.label12.TabIndex = 190;
            this.label12.Text = "BOM DETAILS";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // MO_No
            // 
            this.MO_No.DataPropertyName = "MO_No";
            this.MO_No.HeaderText = "MO No";
            this.MO_No.MinimumWidth = 6;
            this.MO_No.Name = "MO_No";
            // 
            // Project_Id
            // 
            this.Project_Id.DataPropertyName = "Project_Id";
            this.Project_Id.HeaderText = "Project_ID";
            this.Project_Id.MinimumWidth = 6;
            this.Project_Id.Name = "Project_Id";
            // 
            // Project_Code
            // 
            this.Project_Code.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.Project_Code.DataPropertyName = "Project_Code";
            this.Project_Code.HeaderText = "Project";
            this.Project_Code.MinimumWidth = 6;
            this.Project_Code.Name = "Project_Code";
            this.Project_Code.Width = 200;
            // 
            // Prod_Group_Name
            // 
            this.Prod_Group_Name.DataPropertyName = "Prod_Group_Name";
            this.Prod_Group_Name.HeaderText = "Category";
            this.Prod_Group_Name.MinimumWidth = 6;
            this.Prod_Group_Name.Name = "Prod_Group_Name";
            // 
            // Module_No
            // 
            this.Module_No.DataPropertyName = "Module_No";
            this.Module_No.HeaderText = "Module";
            this.Module_No.MinimumWidth = 6;
            this.Module_No.Name = "Module_No";
            // 
            // Bom_Project_Search
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(987, 554);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.dgvBOMData);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name = "Bom_Project_Search";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "BomSearch";
            this.Load += new System.EventHandler(this.BomSearch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBOMData)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvBOMData;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.DataGridViewTextBoxColumn MO_No;
        private System.Windows.Forms.DataGridViewTextBoxColumn Project_Id;
        private System.Windows.Forms.DataGridViewTextBoxColumn Project_Code;
        private System.Windows.Forms.DataGridViewTextBoxColumn Prod_Group_Name;
        private System.Windows.Forms.DataGridViewTextBoxColumn Module_No;
    }
}