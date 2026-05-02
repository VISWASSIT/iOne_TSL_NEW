using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ione_DAL;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGridConverter;
using System.Diagnostics;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Configuration;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.Data;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;


namespace ioneNet.HumanResourceManagement.Reports
{
    public partial class SalaryRevisionReport : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        public SalaryRevisionReport()
        {
            InitializeComponent();
        }

        private void SalaryRevisionReport_Load(object sender, EventArgs e)
        {
            
        }

        private void btnAddNew_Click(object sender, EventArgs e)
        {

            try
            {
                //crys viewer1 = new CrstalReportViewer1();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "SalaryRevisionReport.pdf");
                //string path = Path.Combine(Directory.GetCurrentDirectory(), @"\Purchase_Order.pdf");
                System.IO.FileInfo fi = new System.IO.FileInfo(path);
                Cursor.Current = Cursors.WaitCursor;
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                //     rep = new MaterialManagement.Transactions.PurchaseOrder();


                rep = new HumanResourceManagement.Reports.SalaryRevisionReprot();
                SqlCommand cmd = new SqlCommand("Sp_HR_Salary_IncrementReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@compname", logIn.company);
                cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
              


                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable Dt = new DataTable();

                da.SelectCommand = cmd;
                da.Fill(Dt);

                if (Dt.Rows.Count > 0)
                {


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
                    rep.SetDataSource(Dt);


                    //rep.SetParameterValue("Invoice_No", SO_No);
                    //rep.SetParameterValue("Creation_Company", logIn.company);
                    ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                    // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                    this.crystalReportViewer1.ReportSource = rep;
                    this.crystalReportViewer1.Refresh();
                    rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                    cmd.Parameters.Clear();
                    //Process.Start(path);
                }

                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


            //try
            //{

            //    SqlCommand cmd2 = new SqlCommand("Sp_HR_Salary_IncrementReport ", con);
            //    cmd2.CommandType = CommandType.StoredProcedure;
            //    cmd2.Parameters.AddWithValue("@compname", logIn.company);
            //    cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);

            //    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //    //DataSet ds2 = new DataSet();
            //    DataTable ds2 = new DataTable();
            //    // da2.Fill(ds2, "x");
            //    da2.Fill(ds2);
            //    sfDataGrid1.DataSource = ds2;
            //    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
            //    this.sfDataGrid1.Columns["Emp_Master_ID"].FilterRowEditorType = "TextBox";
            //    this.sfDataGrid1.Columns["Emp_Master_ID"].ShowFilterRowOptions = false;
            //    this.sfDataGrid1.Columns["Emp_Master_ID"].ImmediateUpdateColumnFilter = true;
            //    this.sfDataGrid1.Columns["Emp_Master_ID"].FilterRowCondition = FilterRowCondition.Contains;
            //    this.sfDataGrid1.Columns["emp_code"].FilterRowEditorType = "TextBox";
            //    this.sfDataGrid1.Columns["emp_code"].ShowFilterRowOptions = false;
            //    this.sfDataGrid1.Columns["emp_code"].ImmediateUpdateColumnFilter = true;
            //    this.sfDataGrid1.Columns["emp_code"].FilterRowCondition = FilterRowCondition.Contains;
            //    this.sfDataGrid1.Columns["Emp_Name"].FilterRowEditorType = "TextBox";
            //    this.sfDataGrid1.Columns["Emp_Name"].ShowFilterRowOptions = false;
            //    this.sfDataGrid1.Columns["Emp_Name"].ImmediateUpdateColumnFilter = true;
            //    this.sfDataGrid1.Columns["Emp_Name"].FilterRowCondition = FilterRowCondition.Contains;
            //    this.sfDataGrid1.Columns["Dept_Name"].FilterRowEditorType = "TextBox";
            //    this.sfDataGrid1.Columns["Dept_Name"].ShowFilterRowOptions = false;
            //    this.sfDataGrid1.Columns["Dept_Name"].ImmediateUpdateColumnFilter = true;
            //    this.sfDataGrid1.Columns["Dept_Name"].FilterRowCondition = FilterRowCondition.Contains;


            //    //this.sfDataGrid1.TableSummaryRows.Clear();
            //    ////this.sfDataGrid1.GroupSummaryRows.Clear();
            //    //GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
            //    //tableSummaryRow1.Name = "TableSummary";
            //    //tableSummaryRow1.ShowSummaryInRow = false;
            //    //tableSummaryRow1.Position = VerticalPosition.Bottom;
            //    //string ColNum = "0";

            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void sfButton1_Click(object sender, EventArgs e)
        {
            //DateTime dt = dtpFrmDate.Value;
            //string dt1 = dt.ToString("dd/MM/yyyy");



            //var options = new ExcelExportingOptions();
            //options.StartRowIndex = 5;
            //var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            //var workBook = excelEngine.Excel.Workbooks[0];
            //var ws = excelEngine.Excel.Worksheets[1];
            //workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            //workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            //workBook.Worksheets[0].Range["A2"].Value = "Salary Revision Report";
            
            //workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            //workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            //workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            //workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            //workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            //workBook.Worksheets[0].PageSetup.Zoom = 85;
            //workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            //string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            //workBook.SaveAs(Fname + "\\Salary_Revision_Report.xlsx");
            //string doc = Fname + "\\Salary_Revision_Report.xlsx";
            //Process prc = new Process();
            //prc.StartInfo.FileName = doc;
            //prc.Start();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
