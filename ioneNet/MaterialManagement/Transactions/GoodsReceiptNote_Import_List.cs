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
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGridConverter;
using System.Diagnostics;
using Syncfusion.WinForms.DataGrid;
using Ione_DAL;
using System.IO;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

namespace ioneNet.MaterialManagement.Transactions
{
    public partial class GoodsReceiptNote_Import_List : Form
    {
        public static string InvoiceNoList, Order_NoList, SO_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;
      
        DataClasses1DataContext db = new DataClasses1DataContext();
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static Boolean editMode;
        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void sfDataGrid1_Click(object sender, EventArgs e)
        {

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {

                    string cellValue;
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
                            var mappingName = sfDataGrid1.Columns["GRN_No"].MappingName;
                            var mappingName1 = sfDataGrid1.Columns["status"].MappingName;
                            //var mappingName = sfDataGrid1.Columns[0].MappingName;
                            //var mappingName1 = sfDataGrid1.Columns[8].MappingName;
                            //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                            //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                            if (rowData == item)
                            {
                                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                                var cellStatus = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                                //if (cellStatus.ToString() != "Despatches Started")
                                //{
                                    SqlCommand cmd = new SqlCommand();

                                    SO_No = cellVaue.ToString();
                                    cmd.CommandText = "Update GoodsReceiptNote_Import_Master set isdeleted = '1' where Grn_NO=@param1 and Company_ID =@compName";
                                    cmd.Parameters.AddWithValue("@param1", SO_No);
                                    cmd.Parameters.AddWithValue("@CompName", logIn.company);
                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                    cmd.Parameters.Clear();
                                    string strT = logIn.username + "-" + DateTime.Now;
                                    cmd.CommandText = "Update GoodsReceiptNote_Import_Master set Modified_By = @strT where Grn_NO=@param1 and Company_ID =@compName";
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
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void viewSupplyDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MaterialManagement.Reports.MRNReport frm = new MaterialManagement.Reports.MRNReport();
            frm.MdiParent = this.MdiParent;
            frm.Show();
        }

        private void approveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 2; i < sfDataGrid1.RowCount; i++)
                {

                    //foreach (var item in sfDataGrid1.SelectedItems)
                    //{
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["GRN_No"].MappingName;
                    var mappingName1 = sfDataGrid1.Columns["status"].MappingName;
                    var mappingName2 = sfDataGrid1.Columns["Sel"].MappingName;
                    //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                    var cellVaue3 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
                    if (cellVaue3 == "True")
                    {
                        var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                        var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());

                        string Status = cellVaue1.ToString();
                        string OrdNo = cellVaue.ToString();
                        if (Status == "Created" || Status == "Reviewed")
                        {

                            var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Good Receipt Note" && m.Role_ID == logIn.UserRoleID select new { m.Approve_Role }).Distinct().ToList();
                            if (uRole.Count > 0)
                            {
                                if (uRole[0].Approve_Role == true)
                                {
                                    var ci = db.GoodsReceiptNote_Import_Masters.Where(w => w.Grn_NO == OrdNo && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                                    {
                                        ci.Status = 6;
                                        ci.Modified_By = logIn.username + "-" + DateTime.Now;
                                        db.SubmitChanges();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("You Have No Permissions to Approve The GRN");
                                    return;
                                }
                            }

                        }
                        else
                        {
                            MessageBox.Show("GRN No " + OrdNo + " Is Already Approved or Further Processed.. No Work Done");

                        }
                    }

                    //}
                }
                BindOrderslist();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sfButton4_Click(object sender, EventArgs e)
        {

            var options = new ExcelExportingOptions();
            options.StartRowIndex = 5;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname + "-" + logIn.BU_ID;
            workBook.Worksheets[0].Range["A2"].Value = "GRN List";
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\GRN_List.xlsx");
            string doc = Fname + "\\GRN_List.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //crys viewer1 = new CrstalReportViewer1();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "GRN_import.pdf");
                //string path = Path.Combine(Directory.GetCurrentDirectory(), @"\Purchase_Order.pdf");
                System.IO.FileInfo fi = new System.IO.FileInfo(path);
                Cursor.Current = Cursors.WaitCursor;
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                //     rep = new MaterialManagement.Transactions.PurchaseOrder();
                var gstno = (from c in db.Company_Report_Formats
                             where c.Company_ID == logIn.company
                             select new { c.GRN_Format }).ToList();
                if (gstno.Count > 0)
                {
                    if (gstno[0].GRN_Format == "GRN_RF")
                    {
                        rep = new MaterialManagement.Transactions.GRN_import();
                    }
                    else
                    {
                        rep = new MaterialManagement.Transactions.GRN_import();
                    }
                }
                else
                {
                    rep = new MaterialManagement.Transactions.GRN_import();

                }
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["GRN_No"].MappingName;
                //var mappingName1 = sfDataGrid1.Columns[4].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                SO_No = cellVaue.ToString();


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


                rep.SetParameterValue("GRN_No", SO_No);
                rep.SetParameterValue("BU_ID", logIn.BU_ID);
                ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                viewer.crystalReportViewer1.ReportSource = rep;
                viewer.crystalReportViewer1.Refresh();
                rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                Process.Start(path);

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public GoodsReceiptNote_Import_List()
        {
            InitializeComponent();
        }

        private void GoodsReceiptNoteList_Load(object sender, EventArgs e)
        {
            BindOrderslist();
        }
        public void BindOrderslist()
        {
            try
            {
                var d = (from data in db.SP_ShowGRN_import_list(logIn.company,logIn.BU_ID, logIn.fy_Start_Date, logIn.fy_End_Date, null) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                    (sfDataGrid1.Columns["Sel"] as GridCheckBoxColumn).AllowThreeState = false;
                    (sfDataGrid1.Columns["Sel"] as GridCheckBoxColumn).AllowCheckBoxOnHeader = true;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["GRN_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["GRN_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["GRN_No"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["GRN_No"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Supplier"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Supplier"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Supplier"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Supplier"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Supp_Inv_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Supp_Inv_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Supp_Inv_No"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Supp_Inv_No"].FilterRowCondition = FilterRowCondition.Equals;
                    this.sfDataGrid1.QueryCellStyle += sfDataGrid_QueryCellStyle;
                string cellValue;
                for (int i = 2; i < sfDataGrid1.RowCount; i++)
                {
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["status"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    if (cellVaue.ToString() == "Approved")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.Green);
                    }
                    if (cellVaue.ToString() == "Despatches Started")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.LightSkyBlue);
                    }
                    if (cellVaue.ToString() == "Closed")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.SaddleBrown);
                    }
                    if (cellVaue.ToString() == "Pre-Closed")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.Red);
                    }
                }                    
                }
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

        private void sfButton1_Click(object sender, EventArgs e)
        {
            editMode = false;
            var = "1";           
            MaterialManagement.Transactions.GoodsReceiptNote_Import frm = new MaterialManagement.Transactions.GoodsReceiptNote_Import();
            frm.MdiParent = this.MdiParent;
            frm.Show();
            BindOrderslist();
          
                
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            BindOrderslist();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["GRN_No"].MappingName;
                var mappingName1 = sfDataGrid1.Columns["status"].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                if (cellVaue.ToString() != "")
                {
                    SO_No = cellVaue;
                    var = "0";
                    editMode = true;
                    MaterialManagement.Transactions.GoodsReceiptNote_Import frm = new MaterialManagement.Transactions.GoodsReceiptNote_Import();
                    //OrderManagement.Transactions.
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                    //FrmInv.ShowDialog();
                    //i1 = 0;
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
    }
}
