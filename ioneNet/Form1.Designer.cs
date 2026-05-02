namespace ioneNet
{
    partial class frmMenuBoard
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
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.btmPlantMaint = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.btnAdministrator = new System.Windows.Forms.Button();
            this.btnQuality = new System.Windows.Forms.Button();
            this.button9 = new System.Windows.Forms.Button();
            this.btnFinanceManagement = new System.Windows.Forms.Button();
            this.btnHRMS = new System.Windows.Forms.Button();
            this.btnProduction = new System.Windows.Forms.Button();
            this.btnMM = new System.Windows.Forms.Button();
            this.OrderManagement = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLabel1.Location = new System.Drawing.Point(26, 492);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(69, 15);
            this.linkLabel1.TabIndex = 13;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "iOneAdmin";
            this.linkLabel1.Visible = false;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            this.linkLabel1.Click += new System.EventHandler(this.linkLabel1_Click);
            // 
            // btmPlantMaint
            // 
            this.btmPlantMaint.BackColor = System.Drawing.Color.White;
            this.btmPlantMaint.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btmPlantMaint.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btmPlantMaint.Image = global::ioneNet.Properties.Resources.Prod_Mgmt_Logo;
            this.btmPlantMaint.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btmPlantMaint.Location = new System.Drawing.Point(453, 321);
            this.btmPlantMaint.Name = "btmPlantMaint";
            this.btmPlantMaint.Size = new System.Drawing.Size(224, 54);
            this.btmPlantMaint.TabIndex = 10;
            this.btmPlantMaint.Text = "Plant Maintenance";
            this.btmPlantMaint.UseVisualStyleBackColor = false;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Calibri", 13.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.Red;
            this.label3.Location = new System.Drawing.Point(97, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(682, 19);
            this.label3.TabIndex = 6;
            this.label3.Text = "label3";
            this.label3.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(782, 456);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(152, 15);
            this.label1.TabIndex = 1;
            this.label1.Text = "Designed and Developed By";
            // 
            // btnAdministrator
            // 
            this.btnAdministrator.BackColor = System.Drawing.Color.White;
            this.btnAdministrator.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAdministrator.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btnAdministrator.Image = global::ioneNet.Properties.Resources.Admin_Mgmt_Logo;
            this.btnAdministrator.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdministrator.Location = new System.Drawing.Point(208, 175);
            this.btnAdministrator.Name = "btnAdministrator";
            this.btnAdministrator.Size = new System.Drawing.Size(224, 54);
            this.btnAdministrator.TabIndex = 14;
            this.btnAdministrator.Text = "Administrator";
            this.btnAdministrator.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAdministrator.UseVisualStyleBackColor = false;
            this.btnAdministrator.Click += new System.EventHandler(this.btnAdministrator_Click);
            // 
            // btnQuality
            // 
            this.btnQuality.BackColor = System.Drawing.Color.White;
            this.btnQuality.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnQuality.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btnQuality.Image = global::ioneNet.Properties.Resources.Quality_Mgmt_Logo;
            this.btnQuality.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnQuality.Location = new System.Drawing.Point(453, 249);
            this.btnQuality.Name = "btnQuality";
            this.btnQuality.Size = new System.Drawing.Size(224, 54);
            this.btnQuality.TabIndex = 9;
            this.btnQuality.Text = "Quality Management";
            this.btnQuality.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnQuality.UseVisualStyleBackColor = false;
            this.btnQuality.Click += new System.EventHandler(this.btnQuality_Click);
            // 
            // button9
            // 
            this.button9.BackColor = System.Drawing.Color.Khaki;
            this.button9.Font = new System.Drawing.Font("Century Gothic", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button9.ForeColor = System.Drawing.Color.Navy;
            this.button9.Image = global::ioneNet.Properties.Resources.Exit;
            this.button9.Location = new System.Drawing.Point(403, 482);
            this.button9.Name = "button9";
            this.button9.Size = new System.Drawing.Size(83, 33);
            this.button9.TabIndex = 8;
            this.button9.Text = "Exit";
            this.button9.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button9.UseVisualStyleBackColor = false;
            this.button9.Click += new System.EventHandler(this.button9_Click);
            // 
            // btnFinanceManagement
            // 
            this.btnFinanceManagement.BackColor = System.Drawing.Color.White;
            this.btnFinanceManagement.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFinanceManagement.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btnFinanceManagement.Image = global::ioneNet.Properties.Resources.Fin_Mgmt_Logo;
            this.btnFinanceManagement.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnFinanceManagement.Location = new System.Drawing.Point(453, 394);
            this.btnFinanceManagement.Name = "btnFinanceManagement";
            this.btnFinanceManagement.Size = new System.Drawing.Size(224, 54);
            this.btnFinanceManagement.TabIndex = 5;
            this.btnFinanceManagement.Text = "Finance Management";
            this.btnFinanceManagement.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnFinanceManagement.UseVisualStyleBackColor = false;
            this.btnFinanceManagement.Click += new System.EventHandler(this.btnFinanceManagement_Click);
            // 
            // btnHRMS
            // 
            this.btnHRMS.BackColor = System.Drawing.Color.White;
            this.btnHRMS.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHRMS.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btnHRMS.Image = global::ioneNet.Properties.Resources.HR_Mgmt_Logo;
            this.btnHRMS.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnHRMS.Location = new System.Drawing.Point(208, 394);
            this.btnHRMS.Name = "btnHRMS";
            this.btnHRMS.Size = new System.Drawing.Size(224, 54);
            this.btnHRMS.TabIndex = 4;
            this.btnHRMS.Text = "Human Resource Management";
            this.btnHRMS.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnHRMS.UseVisualStyleBackColor = false;
            this.btnHRMS.Click += new System.EventHandler(this.btnHRMS_Click);
            // 
            // btnProduction
            // 
            this.btnProduction.BackColor = System.Drawing.Color.White;
            this.btnProduction.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProduction.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btnProduction.Image = global::ioneNet.Properties.Resources.Prod_Mgmt_Logo_New;
            this.btnProduction.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnProduction.Location = new System.Drawing.Point(453, 175);
            this.btnProduction.Name = "btnProduction";
            this.btnProduction.Size = new System.Drawing.Size(224, 54);
            this.btnProduction.TabIndex = 3;
            this.btnProduction.Text = "Production Management";
            this.btnProduction.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnProduction.UseVisualStyleBackColor = false;
            this.btnProduction.Click += new System.EventHandler(this.button4_Click);
            // 
            // btnMM
            // 
            this.btnMM.BackColor = System.Drawing.Color.White;
            this.btnMM.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.btnMM.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnMM.ForeColor = System.Drawing.Color.MidnightBlue;
            this.btnMM.Image = global::ioneNet.Properties.Resources.Inventory_Mgmt_Logo1;
            this.btnMM.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnMM.Location = new System.Drawing.Point(208, 321);
            this.btnMM.Name = "btnMM";
            this.btnMM.Size = new System.Drawing.Size(224, 54);
            this.btnMM.TabIndex = 2;
            this.btnMM.Text = "Material Management";
            this.btnMM.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnMM.UseVisualStyleBackColor = false;
            this.btnMM.Click += new System.EventHandler(this.btnMtrlManagement_Click);
            // 
            // OrderManagement
            // 
            this.OrderManagement.BackColor = System.Drawing.Color.White;
            this.OrderManagement.Font = new System.Drawing.Font("Calibri", 10.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.OrderManagement.ForeColor = System.Drawing.Color.MidnightBlue;
            this.OrderManagement.Image = global::ioneNet.Properties.Resources.Order_Mgmt_Logo;
            this.OrderManagement.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.OrderManagement.Location = new System.Drawing.Point(208, 249);
            this.OrderManagement.Name = "OrderManagement";
            this.OrderManagement.Size = new System.Drawing.Size(224, 54);
            this.OrderManagement.TabIndex = 1;
            this.OrderManagement.Text = "Order Management";
            this.OrderManagement.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.OrderManagement.UseVisualStyleBackColor = false;
            this.OrderManagement.Click += new System.EventHandler(this.OrderManagement_Click);
            // 
            // frmMenuBoard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.BackgroundImage = global::ioneNet.Properties.Resources.Blue_and_White_Watercolor_Castle_Wedding_Poster__2_;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(941, 544);
            this.Controls.Add(this.button9);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.btnAdministrator);
            this.Controls.Add(this.btmPlantMaint);
            this.Controls.Add(this.btnFinanceManagement);
            this.Controls.Add(this.btnHRMS);
            this.Controls.Add(this.btnQuality);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnProduction);
            this.Controls.Add(this.btnMM);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OrderManagement);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.Name = "frmMenuBoard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmMenuBoard_FormClosed);
            this.Load += new System.EventHandler(this.frmMenuBoard_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button9;
        private System.Windows.Forms.Button btnFinanceManagement;
        private System.Windows.Forms.Button btnHRMS;
        private System.Windows.Forms.Button btnProduction;
        private System.Windows.Forms.Button btnMM;
        private System.Windows.Forms.Button OrderManagement;
        private System.Windows.Forms.Button btmPlantMaint;
        private System.Windows.Forms.Button btnQuality;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnAdministrator;
        private System.Windows.Forms.Label label3;
    }
}

