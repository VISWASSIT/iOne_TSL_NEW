using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.WinForms.GridCommon.ScrollAxis;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Ione_DAL;
using CrystalDecisions.Shared;
using System.Diagnostics;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;

namespace ioneNet.MaterialManagement.Transactions
{
    public partial class GRNJWList : Form
    {
        public static string InvoiceNoList, Order_NoList, SO_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;

        DataClasses1DataContext db = new DataClasses1DataContext();
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        private Database crDatabase;
        private Tables crTables;
        private Table crTable;



        private TableLogOnInfo crTableLogOnInfo;


        public static Boolean editMode;
        public GRNJWList()
        {
            InitializeComponent();
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        private void viewSupplyDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //crys viewer1 = new CrstalReportViewer1();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "GRN_JW.pdf");
                //string path = Path.Combine(Directory.GetCurrentDirectory(), @"\Purchase_Order.pdf");
                System.IO.FileInfo fi = new System.IO.FileInfo(path);
                Cursor.Current = Cursors.WaitCursor;
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                rep = new MaterialManagement.Transactions.GRN_JW();


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
                var mappingName = sfDataGrid1.Columns["GRN_No"].MappingName;
                var mappingName1 = sfDataGrid1.Columns[4].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                //int i = dgvRecordList.CurrentRow.Index;
                //string a = dgvRecordList.Rows[i].Cells["PoNoDateDataGridViewTextBoxColumn"].Value.ToString();
                //rep.RecordSelectionFormula = "{ Purchase_Req_Master.PR_NO} = " + cellVaue + " and { Purchase_Req_Master.Company_ID} =" + logIn.company;

                //string b = AppCode.GlobalAccess.companyName;
                // rep.RecordSelectionFormula = "{Purchase_Order_Master.PO_NO} = " + a + "  and {Purchase_Order_Master.Company_ID} = " + logIn.company;
                rep.SetParameterValue("GRN_No", cellVaue);
                rep.SetParameterValue("BU_ID", logIn.BU_ID);

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

        private void GRNJWList_Load(object sender, EventArgs e)
        {
            try
            {
                var bindLoc = (from m in db.User_Roles
                               where m.Company_ID == logIn.company && m.Role_ID == logIn.UserRoleID && m.Form_Name== "Material Receipts- After Job Work"
                               select new
                               {
                                   m.Approve_Role,
                                   m.View_Role,
                                   m.Modify_Role,
                                   m.Create_Role,
                                   m.Delete_Role
                               }).ToList();
                this.modifyToolStripMenuItem.Enabled = false;
                if (bindLoc[0].Modify_Role == true)
                {
                    this.modifyToolStripMenuItem.Enabled = true;
                }
               
                this.deleteToolStripMenuItem.Enabled = false;
                if (bindLoc[0].Delete_Role == true)
                {

                    this.deleteToolStripMenuItem.Enabled = true;
                }
                btnAddnew.Enabled = false;
                if (bindLoc[0].Create_Role == true)
                {
                    btnAddnew.Enabled = true;
                }
                BindOrderslist();
            }
            catch (Exception ex)
            {

            }
      
        }
        private void btnAddnew_Click(object sender, EventArgs e)
        {
            editMode = false;
            var = "1";
            MaterialManagement.Transactions.GoodsReceiptNoteJW frm = new MaterialManagement.Transactions.GoodsReceiptNoteJW();
            frm.MdiParent = this.MdiParent;
            frm.Show();
        }
        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns[0].MappingName;
                var mappingName1 = sfDataGrid1.Columns[4].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                if (cellVaue.ToString() != "")
                {
                    var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Material Receipts- After Job Work" && m.Role_ID == logIn.UserRoleID select new { m.View_Role }).Distinct().ToList();
                    if (uRole.Count > 0)
                    {
                        if (uRole[0].View_Role == true)
                        {

                            SO_No = cellVaue;
                            var = "0";
                            editMode = true;
                            MaterialManagement.Transactions.GoodsReceiptNoteJW frm = new MaterialManagement.Transactions.GoodsReceiptNoteJW();
                            //OrderManagement.Transactions.
                            frm.MdiParent = this.MdiParent;
                            frm.Show();
                            //FrmInv.ShowDialog();
                            //i1 = 0;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Please Select Any One Record");
                    //i1 = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
                var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Material Receipts- After Job Work" && m.Role_ID == logIn.UserRoleID select new { m.Delete_Role }).Distinct().ToList();
                if (uRole.Count > 0)
                {
                    DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {


                        for (int i = 1; i < sfDataGrid1.RowCount; i++)
                        {
                            foreach (var item in sfDataGrid1.SelectedItems)
                            {

                                //foreach (var col in sfDataGrid1.Columns)
                                //{
                                //if (col.MappingName == "Alternative_Code")
                                //{
                                //var mappingName = sfDataGrid1.Columns[columnIndex].MappingName;
                                //var recordIndex = sfDataGrid1.TableControl.ResolveToRecordIndex(rowIndex);
                                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                                var mappingName = sfDataGrid1.Columns[0].MappingName;
                                // var mappingName1 = sfDataGrid1.Columns[8].MappingName;
                                //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                                //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                                if (rowData == item)
                                {
                                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                                    // var cellStatus = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                                    //if (cellStatus.ToString() != "Despatches Started")
                                    //{
                                    SqlCommand cmd = new SqlCommand();

                                    SO_No = cellVaue.ToString();
                                    cmd.CommandText = "Update GRN_JW_Master set isdeleted = '1' where Grn_NO=@param1 and Company_ID =@compName";
                                    cmd.Parameters.AddWithValue("@param1", SO_No);
                                    cmd.Parameters.AddWithValue("@CompName", logIn.company);
                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                    cmd.Parameters.Clear();
                                    string strT = logIn.username + "-" + DateTime.Now;
                                    cmd.CommandText = "Update GRN_JW_Master set Modified_By = @strT where Grn_NO=@param1 and Company_ID =@compName";
                                    cmd.Parameters.AddWithValue("@strT", strT);
                                    cmd.Parameters.AddWithValue("@param1", SO_No);
                                    cmd.Parameters.AddWithValue("@CompName", logIn.company);
                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                    //}
                                    //else
                                    //{
                                    //    MessageBox.Show("Selected GRN Cannot Be Deleted As Already Despatches Started, Pre-Close the order insted");
                                    //}
                                }
                            }
                        }
                        MessageBox.Show("Selected GRN(s) Are Deleted Successfully");
                        BindOrderslist();
                    }
                }
               
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            BindOrderslist();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void BindOrderslist()
        {
            try
            {
                var d = (from data in db.SP_ShowGRNJWlist (logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, null) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                }
                this.sfDataGrid1.QueryCellStyle += sfDataGrid_QueryCellStyle;
                
                for (int i = 1; i < sfDataGrid1.RowCount; i++)
                {
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["status"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    //if (cellVaue.ToString() == "Approved")
                    //{
                    //    SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.Green);
                    //}
                    //if (cellVaue.ToString() == "Despatches Started")
                    //{
                    //    SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.LightSkyBlue);
                    //}
                    //if (cellVaue.ToString() == "Closed")
                    //{
                    //    SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.SaddleBrown);
                    //}
                    //if (cellVaue.ToString() == "Pre-Closed")
                    //{
                    //    SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.Red);
                    //}
                }
                //else
                //{
                //    MessageBox.Show("Record Not Found");
                //    //txtSearch.Text = "";
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void sfDataGrid_QueryCellStyle(object sender, Syncfusion.WinForms.DataGrid.Events.QueryCellStyleEventArgs e)
        {
            var rowColumnIndex = new RowColumnIndex(e.RowIndex, e.ColumnIndex);
            if (colorDict.ContainsKey(rowColumnIndex))
                e.Style.BackColor = colorDict[rowColumnIndex];
        }


        void SetCellBackgroundColor(RowColumnIndex rowColumnIndex, Color color)
        {
            if (!colorDict.ContainsKey(rowColumnIndex))
                colorDict.Add(rowColumnIndex, color);
            else
                colorDict[rowColumnIndex] = color;
            sfDataGrid1.TableControl.Invalidate(this.sfDataGrid1.TableControl.GetCellRectangle(rowColumnIndex.RowIndex, rowColumnIndex.ColumnIndex, false));
        }
    }

}
