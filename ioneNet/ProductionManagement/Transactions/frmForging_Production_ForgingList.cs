using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using Ione_DAL;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.Windows.Forms.Tools.Win32API;
using Syncfusion.WinForms.DataGrid.Interactivity;

namespace ioneNet.ProductionManagement
{
    public partial class frmForging_Production_ForgingList : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string InvoiceNoList, Order_NoList, SO_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;
        public static Boolean editMode;

        private void btnImport_Click(object sender, EventArgs e)
        {
            BindForgingReportList();
        }

        public static string Voucherno;
        public frmForging_Production_ForgingList()
        {
            InitializeComponent();
        }

        private void Productionvouchersearch_Load(object sender, EventArgs e)
        {
            try
            {
                this.WindowState = FormWindowState.Maximized;
                BindForgingReportList();


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void BindForgingReportList()
        {
            try
            {
                var d = (from data in db.SP_ShowProdReportForgingView(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date,"") select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["Voucher_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Voucher_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Voucher_No"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Voucher_No"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["Job_CardNo"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Job_CardNo"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Job_CardNo"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Job_CardNo"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["Plan_Ref_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Plan_Ref_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Plan_Ref_No"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Plan_Ref_No"].FilterRowCondition = FilterRowCondition.Contains;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
                      
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            ProductionManagement.Transactions.frmForge_Production_Forging_RFPL frm = new ProductionManagement.Transactions.frmForge_Production_Forging_RFPL();
            frm.MdiParent = this.MdiParent;

            //var = "1";
            frm.Show();
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                if (i >= 0)
                {
                    //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["Voucher_No"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    //if (cellVaue.ToString() == "Approved")
                    //{
                    SO_No = cellVaue.ToString();
                    var = "0";
                    editMode = true;
                    ProductionManagement.Transactions.frmForge_Production_Forging_RFPL frm = new ProductionManagement.Transactions.frmForge_Production_Forging_RFPL();
                    //OrderManagement.Transactions.
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                    //FrmInv.ShowDialog();
                    //i1 = 0;
                    //}
                    //else
                    //{
                    //    MessageBox.Show("The Order Cannot Be Modified Either Closed or Despatches Started");
                    //    //i1 = 0;
                    //}
                }
                else
                {
                    MessageBox.Show("No Report  No is Selected to Modify");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
