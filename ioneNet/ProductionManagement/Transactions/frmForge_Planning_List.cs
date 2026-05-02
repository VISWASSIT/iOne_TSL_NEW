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
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace ioneNet.ProductionManagement.Transactions
{
    public partial class frmForge_Planning_List : Form
    {
        public static string InvoiceNoList, Order_NoList, SO_No, SO_Amend_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;
        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {

                int i = sfDataGrid1.CurrentCell.RowIndex;
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var currentCellValue = (rowData.GetType().GetProperty("PlanningRef").GetValue(rowData, null).ToString());

                //var mappingName = sfDataGrid1.Columns[4].MappingName;
                //var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                if ((from u in db.Forge_Cutting_Plans where u.Forging_Plan_No == currentCellValue && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    MessageBox.Show("Plan Already Processed, Cannot Be Deleted");
                    return;
                }
                else
                {

                    db.sp_Forge_Plan_Delete(currentCellValue, logIn.company);

                    MessageBox.Show("Record Deleted Sucessfully");
                    BindOrderslist();
                }
            }
        }

        private void sfButton3_Click(object sender, EventArgs e)
        {
            BindOrderslist();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //crys viewer1 = new CrstalReportViewer1();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "ForgingPlan.pdf");
                //string path = Path.Combine(Directory.GetCurrentDirectory(), @"\Purchase_Order.pdf");
                System.IO.FileInfo fi = new System.IO.FileInfo(path);
                Cursor.Current = Cursors.WaitCursor;
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                //     rep = new MaterialManagement.Transactions.PurchaseOrder();

                rep = new ProductionManagement.Transactions.rptForgingPlan();




                crConnectionInfo.ServerName = frmMain.ServerIP;
                crConnectionInfo.DatabaseName = frmMain.Database;
                crConnectionInfo.UserID = frmMain.DBUserID;
                crConnectionInfo.Password = frmMain.Password;


                crDatabase = rep.Database;
                crTables = crDatabase.Tables;
                //Loop through all tables in the report and apply the connection information for each table.
                for (int k = 0; k < crTables.Count; k++)
                {
                    //  crTable = crTables[i];
                    crTableLogOnInfo = crTables[k].LogOnInfo;
                    crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                    crTables[k].ApplyLogOnInfo(crTableLogOnInfo);

                }
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns[0].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                // string path1 = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.pdf");
                SqlCommand cmd1 = con.CreateCommand();
                SqlCommand cmd2 = con.CreateCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                //int i = sfDataGrid1.CurrentRow.Index;
                SO_No = cellVaue.ToString();

                rep.SetParameterValue("PlanRef", SO_No);
                //rep.SetParameterValue("BU_ID", logIn.BU_ID);
                ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                viewer.crystalReportViewer1.ReportSource = rep;
                viewer.crystalReportViewer1.Refresh();
                rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                Process.Start(path);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        DataClasses1DataContext db = new DataClasses1DataContext();
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static Boolean editMode = false;

        public frmForge_Planning_List()
        {
            InitializeComponent();
        }
        private void frmForge_Planning_List_Load(object sender, EventArgs e)
        {
            BindOrderslist();
        }

        private void sfButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sfButton1_Click(object sender, EventArgs e)
        {
            editMode = false;
            ProductionManagement.Transactions.frmForge_ProductionPlanning frm = new frmForge_ProductionPlanning();
            frm.MdiParent = this.MdiParent;
            frm.Show();
            BindOrderslist();
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var currentCellValue = (rowData.GetType().GetProperty("PlanningRef").GetValue(rowData, null).ToString());

                var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Forging Planning" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                if (uRole.Count > 0)
                {
                    if (uRole[0].Modify_Role == true)
                    {
                        SO_No = currentCellValue.ToString();
                        var = "0";
                        editMode = true;
                        ProductionManagement.Transactions.frmForge_ProductionPlanning frm = new frmForge_ProductionPlanning();
                        //OrderManagement.Transactions.
                        frm.MdiParent = this.MdiParent;
                        frm.Show();

                    }
                    else
                    {
                        MessageBox.Show("You Have No Permissions to Modify The Planning Data");
                        return;
                    }
                }
                //else
                //{
                //    SO_No = currentCellValue.ToString();
                //    var = "0";
                //    editMode = true;
                //    OrderManagement.Transactions.frmNewEnquiry frm = new frmNewEnquiry();
                //    //OrderManagement.Transactions.
                //    frm.MdiParent = this.MdiParent;
                //    frm.Show();
                //    //FrmInv.ShowDialog();
                //    //i1 = 0;
                //}                   
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void BindOrderslist()
        {
            try
            {
                var d = (from data in db.ShowForge_PlanningData(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["PlanningRef"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["PlanningRef"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["PlanningRef"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["PlanningRef"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["Forging_Press"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Forging_Press"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Forging_Press"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Forging_Press"].FilterRowCondition = FilterRowCondition.Contains;
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
