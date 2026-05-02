namespace ioneNet.Masters
{
    partial class ProdSearch
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
            this.label12 = new System.Windows.Forms.Label();
            this.showProductsListBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ioneDataSet = new ioneNet.ioneDataSet();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.showProductsListTableAdapter = new ioneNet.ioneDataSetTableAdapters.ShowProductsListTableAdapter();
            this.showProductsListBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.showProductsListBindingSource2 = new System.Windows.Forms.BindingSource(this.components);
            this.sfButton1 = new Syncfusion.WinForms.Controls.SfButton();
            this.sfButton2 = new Syncfusion.WinForms.Controls.SfButton();
            this.sfButton3 = new Syncfusion.WinForms.Controls.SfButton();
            this.sfDataGrid1 = new Syncfusion.WinForms.DataGrid.SfDataGrid();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.showProductsListBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ioneDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.showProductsListBindingSource1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.showProductsListBindingSource2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.sfDataGrid1)).BeginInit();
            this.SuspendLayout();
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.DarkBlue;
            this.label12.Location = new System.Drawing.Point(55, 26);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(166, 23);
            this.label12.TabIndex = 182;
            this.label12.Text = "SEARCH PRODUCTS";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // showProductsListBindingSource
            // 
            this.showProductsListBindingSource.DataMember = "ShowProductsList";
            this.showProductsListBindingSource.DataSource = this.ioneDataSet;
            // 
            // ioneDataSet
            // 
            this.ioneDataSet.DataSetName = "ioneDataSet";
            this.ioneDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(210, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Search";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(259, 23);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(215, 27);
            this.txtSearch.TabIndex = 1;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::ioneNet.Properties.Resources.logoface_Ione;
            this.pictureBox2.Location = new System.Drawing.Point(1, 1);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(45, 40);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 189;
            this.pictureBox2.TabStop = false;
            // 
            // showProductsListTableAdapter
            // 
            this.showProductsListTableAdapter.ClearBeforeFill = true;
            // 
            // showProductsListBindingSource1
            // 
            this.showProductsListBindingSource1.DataMember = "ShowProductsList";
            this.showProductsListBindingSource1.DataSource = this.ioneDataSet;
            // 
            // showProductsListBindingSource2
            // 
            this.showProductsListBindingSource2.DataMember = "ShowProductsList";
            this.showProductsListBindingSource2.DataSource = this.ioneDataSet;
            // 
            // sfButton1
            // 
            this.sfButton1.AccessibleName = "Button";
            this.sfButton1.BackColor = System.Drawing.Color.Turquoise;
            this.sfButton1.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.sfButton1.Location = new System.Drawing.Point(578, 344);
            this.sfButton1.Name = "sfButton1";
            this.sfButton1.Size = new System.Drawing.Size(127, 28);
            this.sfButton1.Style.BackColor = System.Drawing.Color.Turquoise;
            this.sfButton1.TabIndex = 5;
            this.sfButton1.Text = "Complete Selection";
            this.sfButton1.UseVisualStyleBackColor = false;
            this.sfButton1.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // sfButton2
            // 
            this.sfButton2.AccessibleName = "Button";
            this.sfButton2.BackColor = System.Drawing.Color.Turquoise;
            this.sfButton2.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.sfButton2.Location = new System.Drawing.Point(480, 23);
            this.sfButton2.Name = "sfButton2";
            this.sfButton2.Size = new System.Drawing.Size(71, 23);
            this.sfButton2.Style.BackColor = System.Drawing.Color.Turquoise;
            this.sfButton2.TabIndex = 2;
            this.sfButton2.Text = "Find";
            this.sfButton2.UseVisualStyleBackColor = false;
            this.sfButton2.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // sfButton3
            // 
            this.sfButton3.AccessibleName = "Button";
            this.sfButton3.BackColor = System.Drawing.Color.Turquoise;
            this.sfButton3.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.sfButton3.Location = new System.Drawing.Point(453, 344);
            this.sfButton3.Name = "sfButton3";
            this.sfButton3.Size = new System.Drawing.Size(119, 28);
            this.sfButton3.Style.BackColor = System.Drawing.Color.Turquoise;
            this.sfButton3.TabIndex = 4;
            this.sfButton3.Text = "Select Products";
            this.sfButton3.UseVisualStyleBackColor = false;
            this.sfButton3.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // sfDataGrid1
            // 
            this.sfDataGrid1.AccessibleName = "Table";
            this.sfDataGrid1.AllowResizingColumns = true;
            this.sfDataGrid1.AutoSizeColumnsMode = Syncfusion.WinForms.DataGrid.Enums.AutoSizeColumnsMode.AllCells;
            this.sfDataGrid1.Location = new System.Drawing.Point(12, 70);
            this.sfDataGrid1.Name = "sfDataGrid1";
            this.sfDataGrid1.SelectionMode = Syncfusion.WinForms.DataGrid.Enums.GridSelectionMode.Multiple;
            this.sfDataGrid1.Size = new System.Drawing.Size(705, 265);
            this.sfDataGrid1.TabIndex = 3;
            this.sfDataGrid1.Text = "sfDataGrid1";
            this.sfDataGrid1.Click += new System.EventHandler(this.sfDataGrid1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 344);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(148, 20);
            this.label2.TabIndex = 197;
            this.label2.Text = "Products Selected(0)";
            // 
            // ProdSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(730, 384);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.sfDataGrid1);
            this.Controls.Add(this.sfButton3);
            this.Controls.Add(this.sfButton2);
            this.Controls.Add(this.sfButton1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label12);
            this.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ProdSearch";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ProdSearch";
            this.Load += new System.EventHandler(this.ProdSearch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.showProductsListBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ioneDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.showProductsListBindingSource1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.showProductsListBindingSource2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.sfDataGrid1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.BindingSource showProductsListBindingSource;
        private ioneDataSet ioneDataSet;
        private ioneDataSetTableAdapters.ShowProductsListTableAdapter showProductsListTableAdapter;
        private System.Windows.Forms.BindingSource showProductsListBindingSource1;
        private System.Windows.Forms.BindingSource showProductsListBindingSource2;
        private Syncfusion.WinForms.Controls.SfButton sfButton1;
        private Syncfusion.WinForms.Controls.SfButton sfButton2;
        private Syncfusion.WinForms.Controls.SfButton sfButton3;
        private Syncfusion.WinForms.DataGrid.SfDataGrid sfDataGrid1;
        private System.Windows.Forms.Label label2;
    }
}