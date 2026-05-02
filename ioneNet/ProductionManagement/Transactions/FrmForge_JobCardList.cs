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
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Syncfusion.WinForms.DataGridConverter;
using Syncfusion.WinForms.DataGrid.Enums;
namespace ioneNet.ProductionManagement.Transactions
{
    public partial class FrmForge_JobCardList : Form
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
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {


                    //string cellValue;
                    for (int i = 1; i < sfDataGrid1.RowCount; i++)
                    {
                        foreach (var item in sfDataGrid1.SelectedItems)
                        {

                            
                            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                            var mappingName = sfDataGrid1.Columns[0].MappingName;
                            
                            if (rowData == item)
                            {
                                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                               
                                string myString = cellVaue.ToString();
                                if ((from u in db.Forging_ForgingReport_Masters where u.Plan_Ref_No == myString && u.Company_ID == logIn.company select u).Count() > 0)
                                {
                                    MessageBox.Show("Job Card Already Processed, Cannot Be Deleted");
                                    return;
                                }
                                else
                                {
                                    db.sp_JobCard_Delete(myString, logIn.company);
                                }
                            }
                        }
                    }
                    MessageBox.Show("Selected Job Cards(s) Are Deleted Successfully");
                    BindJobCardslist();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            
        }

        private void preCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                //string cellValue;
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
                        //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                        //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                        if (rowData == item)
                        {
                            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                            SqlCommand cmd = new SqlCommand();

                            SO_No = cellVaue.ToString();
                            cmd.CommandText = "Update Forging_JobCard set status = '26' where Job_CardNo=@param1 and Company_ID =@compName";
                            cmd.Parameters.AddWithValue("@param1", SO_No);
                            cmd.Parameters.AddWithValue("@CompName", logIn.company);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            cmd.Parameters.Clear();
                            string strT = logIn.username + "-" + DateTime.Now;
                            cmd.CommandText = "Update Forging_JobCard set Modified_By = @strT where Job_CardNo=@param1 and Company_ID =@compName";
                            cmd.Parameters.AddWithValue("@strT", strT);
                            cmd.Parameters.AddWithValue("@param1", SO_No);
                            cmd.Parameters.AddWithValue("@CompName", logIn.company);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                MessageBox.Show("Selected Job Card is Pre-Closed Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //crys viewer1 = new CrstalReportViewer1();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "JobCard.pdf");
                //string path = Path.Combine(Directory.GetCurrentDirectory(), @"\Purchase_Order.pdf");
                System.IO.FileInfo fi = new System.IO.FileInfo(path);
                Cursor.Current = Cursors.WaitCursor;
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                //     rep = new MaterialManagement.Transactions.PurchaseOrder();
                
                    rep = new ProductionManagement.Transactions.JobCard_RFPL();

                
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["Job_CardNo"].MappingName;
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


                rep.SetParameterValue("reportNo", SO_No);               
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

        private void btnImport_Click(object sender, EventArgs e)
        {
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 5;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            workBook.Worksheets[0].Range["A2"].Value = "Job Card List";
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\Job_Card_List.xlsx");
            string doc = Fname + "\\Job_Card_List.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sfButton1_Click(object sender, EventArgs e)
        {
            BindJobCardslist();
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                //if (i >= 0)
                //{
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var currentCellValue = (rowData.GetType().GetProperty("Job_CardNo").GetValue(rowData, null).ToString());

                //var mappingName = sfDataGrid1.Columns[9].MappingName;
                //var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                //if (cellVaue.ToString() != "Closed" || cellVaue.ToString() != "Pre Closed")
                //{
                //    if (cellVaue.ToString() == "Approved" || cellVaue.ToString() == "Despatches Started")
                //    {
                        var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Job Card" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                        if (uRole.Count > 0)
                        {
                    if (uRole[0].Modify_Role == true)
                    {

                        SO_No = currentCellValue.ToString();
                        var = "0";
                        editMode = true;
                        if (logIn.company == 18)
                        {
                            ProductionManagement.Transactions.frmForge_JobCard frm = new frmForge_JobCard();
                            //OrderManagement.Transactions.
                            frm.MdiParent = this.MdiParent;
                            frm.Show();
                        }
                        else
                        {
                            ProductionManagement.Transactions.frmForge_RFPL_JobCard frm = new frmForge_RFPL_JobCard();
                            //OrderManagement.Transactions.
                            frm.MdiParent = this.MdiParent;
                            frm.Show();

                        }
                    }
                    else
                    {
                        MessageBox.Show("You Have No Permissions to Modify The Job Card");
                        return;
                    }
                        }
                        else
                        {
                            SO_No = currentCellValue.ToString();
                            var = "0";
                             editMode = true;
                            if (logIn.company == 18)
                            {
                                ProductionManagement.Transactions.frmForge_JobCard frm = new frmForge_JobCard();
                                //OrderManagement.Transactions.
                                frm.MdiParent = this.MdiParent;
                                frm.Show();
                            }
                            else
                            {
                                ProductionManagement.Transactions.frmForge_RFPL_JobCard frm = new frmForge_RFPL_JobCard();
                                //OrderManagement.Transactions.
                                frm.MdiParent = this.MdiParent;
                                frm.Show();

                            }
                }
                    //}
                    //else
                    //{
                    //    MessageBox.Show("The Order Cannot Be Modified Either Closed or Despatches Started");
                    //    //i1 = 0;
                    //}
               // }
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

        
        public FrmForge_JobCardList()
        {
            InitializeComponent();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            editMode = false;
            if (logIn.company == 18)
            {
                ProductionManagement.Transactions.frmForge_JobCard frm = new ProductionManagement.Transactions.frmForge_JobCard();
                //frm.MdiParent = this.MdiParent;
                //var = "1";
                frm.Show();
            }
            else
            {
                ProductionManagement.Transactions.frmForge_RFPL_JobCard frm = new ProductionManagement.Transactions.frmForge_RFPL_JobCard();
                frm.MdiParent = this.MdiParent;
                //var = "1";
                frm.Show();
            }
        }

        private void FrmForge_JobCardList_Load(object sender, EventArgs e)
        {
            BindJobCardslist();
        }
        public void BindJobCardslist()
        {
            try
            {
                if (logIn.company == 18)
                {
                    var d = (from data in db.ShowJobCardList(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date) select data).ToList();

                    if (d.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        sfDataGrid1.DataSource = d;
                    }
                }
                else
                {
                    var d = (from data in db.ShowJobCardList_RFPL(logIn.company,logIn.fy_Start_Date,logIn.fy_End_Date) select data).ToList();

                    if (d.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        sfDataGrid1.DataSource = d;
                        this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                        this.sfDataGrid1.Columns["Job_CardNo"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["Job_CardNo"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["Job_CardNo"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["Job_CardNo"].FilterRowCondition = FilterRowCondition.Contains;
                        
                    }
                }
                this.sfDataGrid1.QueryCellStyle += sfDataGrid_QueryCellStyle;
                //string cellValue;
                //for (int i = 1; i < sfDataGrid1.RowCount; i++)
                //{
                //    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                //    var mappingName = sfDataGrid1.Columns[9].MappingName;
                //    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                //    if (cellVaue.ToString() == "Approved")
                //    {
                //        SetCellBackgroundColor(new RowColumnIndex(i, 9), Color.Green);
                //    }
                //    if (cellVaue.ToString() == "Despatches Started")
                //    {
                //        SetCellBackgroundColor(new RowColumnIndex(i, 9), Color.LightSkyBlue);
                //    }
                //    if (cellVaue.ToString() == "Closed")
                //    {
                //        SetCellBackgroundColor(new RowColumnIndex(i, 9), Color.SaddleBrown);
                //    }
                //    if (cellVaue.ToString() == "Pre-Closed")
                //    {
                //        SetCellBackgroundColor(new RowColumnIndex(i, 9), Color.Red);
                //    }
                //}
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
