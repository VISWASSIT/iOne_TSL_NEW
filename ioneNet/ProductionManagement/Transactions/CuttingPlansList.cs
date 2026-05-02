using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using System.Data.OleDb;
using Syncfusion.WinForms.GridCommon.ScrollAxis;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Ione_DAL;
using Syncfusion.WinForms.DataGrid.Enums;

namespace ioneNet.ProductionManagement
{
    public partial class CuttingPlansList : Form
    {
        public static string InvoiceNoList, Order_NoList, SO_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;
        public static Boolean editMode;
        private void sfButton1_Click(object sender, EventArgs e)
        {
            BindProjects();
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
                    var mappingName = sfDataGrid1.Columns["PlanningRef"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    //if (cellVaue.ToString() == "Approved")
                    //{
                    SO_No = cellVaue.ToString();
                        var = "0";
                        editMode = true;
                        ProductionManagement.Transactions.frmForge_CuttingPlan frm = new ProductionManagement.Transactions.frmForge_CuttingPlan();
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
                    MessageBox.Show("No Plan No is Selected to Modify");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void releaseForProductionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                if (i >= 0)
                {
                    //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["CP_No"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    //if (cellVaue.ToString() == "Approved")
                    //{
                    SO_No = cellVaue.ToString();
                    var = "0";
                    editMode = true;
                    ProductionManagement.Transactions.ReleaseCuttingPlan frm = new ProductionManagement.Transactions.ReleaseCuttingPlan();
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
                    MessageBox.Show("No Plan No is Selected to Release");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            BindProjects();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        DataClasses1DataContext db = new DataClasses1DataContext();
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public CuttingPlansList()
        {
            InitializeComponent();
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            ProductionManagement.Transactions.frmForge_CuttingPlan frm = new ProductionManagement.Transactions.frmForge_CuttingPlan();
            frm.MdiParent = this.MdiParent;

            //var = "1";
            frm.Show();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            ProductionManagement.Transactions.frmForge_CuttingPlan frm = new ProductionManagement.Transactions.frmForge_CuttingPlan();
            frm.MdiParent = this.MdiParent;

            //var = "1";
            frm.Show();
        }

        private void ProjectCodes_Load(object sender, EventArgs e)
        {
            BindProjects();
        }
        public void BindProjects()
        {
            try
            {
                var d = (from data in db.SP_Forge_Get_CuttingPlanList(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["PlanningRef"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["PlanningRef"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["PlanningRef"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["PlanningRef"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["Forging_Plan_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Forging_Plan_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Forging_Plan_No"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Forging_Plan_No"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["Material_Type"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Material_Type"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Material_Type"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Material_Type"].FilterRowCondition = FilterRowCondition.Contains;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
