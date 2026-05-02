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
using System.IO;
using System.Data.SqlClient;
using Syncfusion.WinForms.DataGrid.Interactivity;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Configuration;
using System.Diagnostics;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Syncfusion.WinForms.DataGridConverter;
namespace ioneNet.HumanResourceManagement.Reports
{
    public partial class frmAttendaceReport_DayWise : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        public frmAttendaceReport_DayWise()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dt = dateTimePicker1.Value;
                string dt1 = dt.ToString("yyyy/MM/dd");

                DateTime dt2 = dateTimePicker2.Value;
                string dt3 = dt2.ToString("yyyy/MM/dd");
                SqlCommand cmd2 = new SqlCommand("SP_HR_Get_Attendance_Report_Daywise_New ", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);
                cmd2.Parameters.AddWithValue("@frmDate", dt1);
                cmd2.Parameters.AddWithValue("@todate", dt3);
                cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);

                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataSet ds2 = new DataSet();
                DataTable ds2 = new DataTable();
                // da2.Fill(ds2, "x");
                da2.Fill(ds2);
                sfDataGrid1.DataSource = ds2;

                //////crys viewer1 = new CrstalReportViewer1();
                ////string path = Path.Combine(Directory.GetCurrentDirectory(), "DayWiseAttnReport.pdf");
                //////string path = Path.Combine(Directory.GetCurrentDirectory(), @"\Purchase_Order.pdf");
                ////System.IO.FileInfo fi = new System.IO.FileInfo(path);
                ////Cursor.Current = Cursors.WaitCursor;
                ////CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                ////rep = new HumanResourceManagement.Reports.DayWiseAttendanceReport();
                ////SqlCommand cmd = new SqlCommand("SP_HR_Get_Attendance_Report_Daywise", con);
                ////cmd.CommandType = CommandType.StoredProcedure;
                ////cmd.Parameters.AddWithValue("@compname", logIn.company);
                ////cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                ////cmd.Parameters.AddWithValue("@frmDate", dateTimePicker1.Value);
                ////cmd.Parameters.AddWithValue("@todate", dateTimePicker2.Value);

                ////SqlDataAdapter da = new SqlDataAdapter(cmd);

                ////DataTable Dt = new DataTable();

                ////da.SelectCommand = cmd;
                ////da.Fill(Dt);
                ////if (Dt.Rows.Count > 0)
                ////{


                ////    crConnectionInfo.ServerName = frmMain.ServerIP;
                ////    crConnectionInfo.DatabaseName = frmMain.Database;
                ////    crConnectionInfo.UserID = frmMain.DBUserID;
                ////    crConnectionInfo.Password = frmMain.Password;


                ////    crDatabase = rep.Database;
                ////    crTables = crDatabase.Tables;
                ////    //Loop through all tables in the report and apply the connection information for each table.
                ////    for (int k = 0; k < crTables.Count; k++)
                ////    {
                ////        //  crTable = crTables[i];
                ////        crTableLogOnInfo = crTables[k].LogOnInfo;
                ////        crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                ////        crTables[k].ApplyLogOnInfo(crTableLogOnInfo);

                ////    }
                ////    rep.SetDataSource(Dt);


                ////    //rep.SetParameterValue("Invoice_No", SO_No);
                ////    //rep.SetParameterValue("Creation_Company", logIn.company);
                ////    ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                ////    // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                ////    viewer.crystalReportViewer1.ReportSource = rep;
                ////    viewer.crystalReportViewer1.Refresh();
                ////    rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                ////    cmd.Parameters.Clear();
                ////    Process.Start(path);
                ////}
                ////con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sfButton1_Click(object sender, EventArgs e)

        {
            DateTime dt = dateTimePicker1.Value;
            string dt1 = dt.ToString("yyyy/MM/dd");

            DateTime dt2 = dateTimePicker2.Value;
            string dt3 = dt2.ToString("yyyy/MM/dd");
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 5;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.BU_Name;
            workBook.Worksheets[0].Range["A2"].Value = "Day Wise Attendnace Report";
            workBook.Worksheets[0].Range["D2"].Value = "for the Period of :" + dt1 + '-' + dt3;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\DayWiseAttnReport.xlsx");
            string doc = Fname + "\\DayWiseAttnReport.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }
    }
}
