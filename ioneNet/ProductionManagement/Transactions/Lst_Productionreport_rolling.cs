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
using Syncfusion.WinForms.DataGridConverter;
using ioneNet.Qulaity_Management.Transactions;

namespace ioneNet.ProductionManagement.Transactions
{

    

    public partial class Lst_Productionreport_rolling : Form
    {
        public static string InvoiceNoList, Order_NoList, PR_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;

        private void btnReset_Click(object sender, EventArgs e)
        {
            BindOrderslist();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {



                int i = sfDataGrid1.CurrentCell.RowIndex;
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns[1].MappingName;
                //var currentCellValue = (rowData.GetType().GetProperty("MO_NO").GetValue(rowData, null).ToString());

                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                //myString = txtSlipNo.Text;
                db.Sp_delete_Production_ReportData(cellVaue, logIn.company, logIn.BU_ID);
                //if ((from u in db.Forge_ProdPlannings where u.Mo_No == cellVaue && u.Company_ID == logIn.company select u).Count() > 0)
                //{
                //    MessageBox.Show("Planning Already Initiated Againist This MO, Cannot Be Deleted");
                //    return;
                //}
                //else
                //{

                //db.Sp_delete_Production_rollings(cellVaue, logIn.company, logIn.BU_ID);
                    BindOrderslist();
                //}
            }
            
        }

        DataClasses1DataContext db = new DataClasses1DataContext();
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);


        public Lst_Productionreport_rolling()
        {
            InitializeComponent();
        }

        private void Lst_Productionreport_rolling_Load(object sender, EventArgs e)
        {
            BindOrderslist();
        }

        public void BindOrderslist()
        {
            try
            {
                var d = (from data in db.Production_report_rollings where data.Company_Id == logIn.company select  (data)).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                    sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    sfDataGrid1.Columns["PR_date"].FilterRowEditorType = "TextBox";
                    sfDataGrid1.Columns["PR_date"].ShowFilterRowOptions = false;
                    sfDataGrid1.Columns["PR_date"].ImmediateUpdateColumnFilter = true;
                    sfDataGrid1.Columns["PR_date"].FilterRowCondition = FilterRowCondition.Contains;
                    sfDataGrid1.Columns["PR_No"].FilterRowEditorType = "TextBox";
                    sfDataGrid1.Columns["PR_No"].ShowFilterRowOptions = false;
                    sfDataGrid1.Columns["PR_No"].ImmediateUpdateColumnFilter = true;
                    sfDataGrid1.Columns["PR_No"].FilterRowCondition = FilterRowCondition.Contains;
                    //sfDataGrid1.Columns["Section_rolled"].FilterRowEditorType = "TextBox";
                    //sfDataGrid1.Columns["Section_rolled"].ShowFilterRowOptions = false;
                    //sfDataGrid1.Columns["Section_rolled"].ImmediateUpdateColumnFilter = true;
                    //sfDataGrid1.Columns["Section_rolled"].FilterRowCondition = FilterRowCondition.Contains;
                    
                    sfDataGrid1.Columns["Finished_Qty"].FilterRowEditorType = "TextBox";
                    sfDataGrid1.Columns["Finished_Qty"].ShowFilterRowOptions = false;
                    sfDataGrid1.Columns["Finished_Qty"].ImmediateUpdateColumnFilter = true;
                    sfDataGrid1.Columns["Finished_Qty"].FilterRowCondition = FilterRowCondition.Contains;

                }


              // sfDataGrid1.QueryCellStyle += sfDataGrid_QueryCellStyle;



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static Boolean editMode;
        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                //if (i >= 0)
                //{
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var currentCellValue = (rowData.GetType().GetProperty("PR_No").GetValue(rowData, null).ToString());

                var mappingName = sfDataGrid1.Columns[1].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());



                var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company  &&  m.Form_Name == "Production Report Rolling" &&  m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                if (uRole.Count > 0)
                {
                    if (uRole[0].Modify_Role == true)
                    {
                        //sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.None;
                        PR_No = currentCellValue.ToString();
                        var = "0";
                        editMode = true;
                        if (logIn.company == 25)
                        {
                            ProductionManagement.Transactions.frm_Production_Report frm = new frm_Production_Report();
                            frm.MdiParent = this.MdiParent;
                            frm.Show();
                        }
                        else
                        {
                            ProductionManagement.Transactions.frm_Production_Report frm = new frm_Production_Report();
                            frm.MdiParent = this.MdiParent;
                            frm.Show();
                        }


                        //sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
                        //OrderManagement.Transactions.frmNewOrder frm = new frmNewOrder();
                        //    //OrderManagement.Transactions.
                        //    frm.MdiParent = this.MdiParent;
                        //    frm.Show();

                    }
                    else
                    {
                        MessageBox.Show("You Have No Permissions to Modify Report");
                        return;
                    }
                }
                else
                {
                    PR_No = currentCellValue.ToString();
                    var = "0";
                    editMode = true;
                    if (logIn.company == 25)
                    {
                        ProductionManagement.Transactions.frm_Production_Report frm = new frm_Production_Report();
                        frm.MdiParent = this.MdiParent;
                        frm.Show();
                    }
                    else
                    {
                        ProductionManagement.Transactions.frm_Production_Report frm = new frm_Production_Report();
                        frm.MdiParent = this.MdiParent;
                        frm.Show();
                    }
                }



                

                //else
                //{
                //    MessageBox.Show("No Order is Selected to Modify");

                //}
            }
  

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {

            //sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.None;

            editMode = false;
            ProductionManagement.Transactions.frm_Production_Report frm = new frm_Production_Report();
            frm.MdiParent = this.MdiParent;
            frm.Show();
            //sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
            BindOrderslist();
        }
    }
}
