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
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.Data;
using Syncfusion.WinForms.DataGridConverter;
using Ione_DAL;
using System.IO;
using Syncfusion.WinForms.DataGrid.Interactivity;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
namespace ioneNet.HumanResourceManagement.Reports
{
    public partial class frmPF_UAN_Report : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();

        public frmPF_UAN_Report()
        {
            InitializeComponent();
        }

      
        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                if (logIn.company == 21)
                {
                    SqlCommand cmd2 = new SqlCommand("SP_HR_GetSalaryReg_RVPR ", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@compname", logIn.company);
                    cmd2.Parameters.AddWithValue("@strmonth", cmbMonth.Text);
                    cmd2.Parameters.AddWithValue("@strYear", cmbYear.Text);                   
                    cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);

                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    //DataSet ds2 = new DataSet();
                    DataTable ds2 = new DataTable();
                    // da2.Fill(ds2, "x");
                    da2.Fill(ds2);
                    sfDataGrid1.DataSource = ds2;
                }
                else
                {
                    SqlCommand cmd2 = new SqlCommand("SP_HR_GetPF_UAN_Report ", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@compname", logIn.company);
                    cmd2.Parameters.AddWithValue("@strmonth", cmbMonth.Text);
                    cmd2.Parameters.AddWithValue("@strYear", cmbYear.Text);
                    cmd2.Parameters.AddWithValue("@empGroup", Convert.ToInt32(cmbEmpGroup.SelectedValue));
                    cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);

                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    //DataSet ds2 = new DataSet();
                    DataTable ds2 = new DataTable();
                    // da2.Fill(ds2, "x");
                    da2.Fill(ds2);
                    sfDataGrid1.DataSource = ds2;
                }
                

                this.sfDataGrid1.TableSummaryRows.Clear();
                //this.sfDataGrid1.GroupSummaryRows.Clear();
                GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                tableSummaryRow1.Name = "TableSummary";
                tableSummaryRow1.ShowSummaryInRow = false;
                tableSummaryRow1.Position = VerticalPosition.Bottom;

                GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                summaryColumn1.Name = "GrossSal";
                summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn1.Format = "{Sum}";
                summaryColumn1.MappingName = "GrossSal";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

                GridSummaryColumn summaryColumnOBQty1 = new GridSummaryColumn();
                summaryColumnOBQty1.Name = "PFGross";
                summaryColumnOBQty1.SummaryType = SummaryType.DoubleAggregate;
                summaryColumnOBQty1.Format = "{Sum}";
                summaryColumnOBQty1.MappingName = "PFGross";
                tableSummaryRow1.SummaryColumns.Add(summaryColumnOBQty1);


                GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                summaryColumn2.Name = "EPSWages";
                summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn2.Format = "{Sum}";
                summaryColumn2.MappingName = "EPSWages";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn2);

                GridSummaryColumn summaryColumnReceipt2 = new GridSummaryColumn();
                summaryColumnReceipt2.Name = "EDLIWages";
                summaryColumnReceipt2.SummaryType = SummaryType.DoubleAggregate;
                summaryColumnReceipt2.Format = "{Sum}";
                summaryColumnReceipt2.MappingName = "EDLIWages";
                tableSummaryRow1.SummaryColumns.Add(summaryColumnReceipt2);

                GridSummaryColumn summaryColumnReceipt3 = new GridSummaryColumn();
                summaryColumnReceipt3.Name = "PFAmt";
                summaryColumnReceipt3.SummaryType = SummaryType.DoubleAggregate;
                summaryColumnReceipt3.Format = "{Sum}";
                summaryColumnReceipt3.MappingName = "PFAmt";
                tableSummaryRow1.SummaryColumns.Add(summaryColumnReceipt3);

                GridSummaryColumn summaryColumnReceipt4 = new GridSummaryColumn();
                summaryColumnReceipt4.Name = "EPSContr";
                summaryColumnReceipt4.SummaryType = SummaryType.DoubleAggregate;
                summaryColumnReceipt4.Format = "{Sum}";
                summaryColumnReceipt4.MappingName = "EPSContr";
                tableSummaryRow1.SummaryColumns.Add(summaryColumnReceipt4);

                GridSummaryColumn summaryColumn5 = new GridSummaryColumn();
                summaryColumn5.Name = "EPFDiff";
                summaryColumn5.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn5.Format = "{Sum}";
                summaryColumn5.MappingName = "EPFDiff";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn5);                

                this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmSalaryRegister_Load(object sender, EventArgs e)
        {
            try
            {
                //Bind EMP Groups
                var bindTypes = (from m in db.HR_Employee_Groups
                                 join emp in db.HR_Employee_Master_Datas on m.ID equals emp.Emp_Group
                                 where emp.Company_ID == logIn.company && emp.BU_ID == logIn.BU_ID

                                 select new
                                 {
                                     m.Group_Name,
                                     m.ID,
                                 }).Distinct().ToList();

                if (bindTypes.Count > 0)
                {
                    cmbEmpGroup.DataSource = bindTypes;
                    cmbEmpGroup.DisplayMember = "Group_Name";
                    cmbEmpGroup.ValueMember = "ID";
                    cmbEmpGroup.SelectedIndex = -1;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExcell_Click(object sender, EventArgs e)
        {
            //DateTime dt = dtpFrmDate.Value;
            //string dt1 = dt.ToString("dd/MM/yyyy");



            var options = new ExcelExportingOptions();
            options.StartRowIndex = 5;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            workBook.Worksheets[0].Range["A2"].Value = "PF UAN REPORT";
            workBook.Worksheets[0].Range["D2"].Value = "for the Month of :" + cmbMonth.Text +'-'+ cmbYear.Text;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\PF_UAN_Report.xlsx");
            string doc = Fname + "\\PF_UAN_Report.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void sfButton1_Click(object sender, EventArgs e)
        {
            try
            {
                ////crys viewer1 = new CrstalReportViewer1();
                //string path = Path.Combine(Directory.GetCurrentDirectory(), "PaySlip.pdf");
                ////string path = Path.Combine(Directory.GetCurrentDirectory(), @"\Purchase_Order.pdf");
                //System.IO.FileInfo fi = new System.IO.FileInfo(path);
                //Cursor.Current = Cursors.WaitCursor;
                //CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                ////     rep = new MaterialManagement.Transactions.PurchaseOrder();
                
                

                ////int i = sfDataGrid1.CurrentCell.RowIndex;
                ////var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                ////var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                ////var mappingName = sfDataGrid1.Columns["Rec_ID"].MappingName;
                //////var mappingName1 = sfDataGrid1.Columns[4].MappingName;
                ////var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                //if (logIn.company == 21 || logIn.company ==24)
                //{
                //    rep = new HumanResourceManagement.Reports.PaySlip_RVPR();
                //    SqlCommand cmd = new SqlCommand("SP_HR_GetPaySlip_RVPR", con);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.AddWithValue("@compname", logIn.company);
                //    cmd.Parameters.AddWithValue("@strmonth", cmbMonth.Text);
                //    cmd.Parameters.AddWithValue("@strYear", cmbYear.Text);
                //    cmd.Parameters.AddWithValue("@empGroup", Convert.ToInt32(cmbEmpGroup.SelectedValue));
                //    cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                //    SqlDataAdapter da = new SqlDataAdapter(cmd);

                //    DataTable Dt = new DataTable();

                //    da.SelectCommand = cmd;
                //    da.Fill(Dt);
                //    if (Dt.Rows.Count > 0)
                //    {


                //        crConnectionInfo.ServerName = frmMain.ServerIP;
                //        crConnectionInfo.DatabaseName = frmMain.Database;
                //        crConnectionInfo.UserID = frmMain.DBUserID;
                //        crConnectionInfo.Password = frmMain.Password;


                //        crDatabase = rep.Database;
                //        crTables = crDatabase.Tables;
                //        //Loop through all tables in the report and apply the connection information for each table.
                //        for (int k = 0; k < crTables.Count; k++)
                //        {
                //            //  crTable = crTables[i];
                //            crTableLogOnInfo = crTables[k].LogOnInfo;
                //            crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                //            crTables[k].ApplyLogOnInfo(crTableLogOnInfo);

                //        }
                //        rep.SetDataSource(Dt);


                //        //rep.SetParameterValue("Invoice_No", SO_No);
                //        //rep.SetParameterValue("Creation_Company", logIn.company);
                //        ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                //        // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                //        viewer.crystalReportViewer1.ReportSource = rep;
                //        viewer.crystalReportViewer1.Refresh();
                //        rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                //        cmd.Parameters.Clear();
                //        Process.Start(path);
                //    }
                //}
                //else
                //{
                //    rep = new HumanResourceManagement.Reports.PaySlip();
                //    SqlCommand cmd = new SqlCommand("SP_HR_GetPaySLip", con);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.AddWithValue("@compname", logIn.company);
                //    cmd.Parameters.AddWithValue("@strmonth", cmbMonth.Text);
                //    cmd.Parameters.AddWithValue("@strYear", cmbYear.Text);
                //    cmd.Parameters.AddWithValue("@empGroup", Convert.ToInt32(cmbEmpGroup.SelectedValue));
                //    cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                //    SqlDataAdapter da = new SqlDataAdapter(cmd);

                //    DataTable Dt = new DataTable();

                //    da.SelectCommand = cmd;
                //    da.Fill(Dt);
                
                //    if (Dt.Rows.Count > 0)
                //    {


                //        crConnectionInfo.ServerName = frmMain.ServerIP;
                //        crConnectionInfo.DatabaseName = frmMain.Database;
                //        crConnectionInfo.UserID = frmMain.DBUserID;
                //        crConnectionInfo.Password = frmMain.Password;


                //        crDatabase = rep.Database;
                //        crTables = crDatabase.Tables;
                //        //Loop through all tables in the report and apply the connection information for each table.
                //        for (int k = 0; k < crTables.Count; k++)
                //        {
                //            //  crTable = crTables[i];
                //            crTableLogOnInfo = crTables[k].LogOnInfo;
                //            crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                //            crTables[k].ApplyLogOnInfo(crTableLogOnInfo);

                //        }
                //        rep.SetDataSource(Dt);


                //        //rep.SetParameterValue("Invoice_No", SO_No);
                //        //rep.SetParameterValue("Creation_Company", logIn.company);
                //        ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                //        // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                //        viewer.crystalReportViewer1.ReportSource = rep;
                //        viewer.crystalReportViewer1.Refresh();                    
                //        rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                //        cmd.Parameters.Clear();
                //        Process.Start(path);
                //    }
                //}
                //con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void printPaySlipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //crys viewer1 = new CrstalReportViewer1();
                //string path = Path.Combine(Directory.GetCurrentDirectory(), "PaySlip.pdf");
                ////string path = Path.Combine(Directory.GetCurrentDirectory(), @"\Purchase_Order.pdf");
                //System.IO.FileInfo fi = new System.IO.FileInfo(path);
                //Cursor.Current = Cursors.WaitCursor;
                //CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                ////     rep = new MaterialManagement.Transactions.PurchaseOrder();

                //rep = new HumanResourceManagement.Reports.PaySlip();

                //int i = sfDataGrid1.CurrentCell.RowIndex;
                //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                ////var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                ////var mappingName = sfDataGrid1.Columns["Emp ID"].MappingName;
                //////var mappingName1 = sfDataGrid1.Columns[4].MappingName;
                ////var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                //SqlCommand cmd = new SqlCommand("SP_HR_GetPaySLip_Employee", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@compname", logIn.company);
                //cmd.Parameters.AddWithValue("@strmonth", cmbMonth.Text);
                //cmd.Parameters.AddWithValue("@strYear", cmbYear.Text);
                //cmd.Parameters.AddWithValue("@empCode", Convert.ToInt32(currentCellValue));
                //cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                //SqlDataAdapter da = new SqlDataAdapter(cmd);

                //DataTable Dt = new DataTable();

                //da.SelectCommand = cmd;
                //da.Fill(Dt);
                //if (Dt.Rows.Count > 0)
                //{


                //    crConnectionInfo.ServerName = frmMain.ServerIP;
                //    crConnectionInfo.DatabaseName = frmMain.Database;
                //    crConnectionInfo.UserID = frmMain.DBUserID;
                //    crConnectionInfo.Password = frmMain.Password;


                //    crDatabase = rep.Database;
                //    crTables = crDatabase.Tables;
                //    //Loop through all tables in the report and apply the connection information for each table.
                //    for (int k = 0; k < crTables.Count; k++)
                //    {
                //        //  crTable = crTables[i];
                //        crTableLogOnInfo = crTables[k].LogOnInfo;
                //        crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                //        crTables[k].ApplyLogOnInfo(crTableLogOnInfo);

                //    }
                //    rep.SetDataSource(Dt);


                //    //rep.SetParameterValue("Invoice_No", SO_No);
                //    //rep.SetParameterValue("Creation_Company", logIn.company);
                //    ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                //    // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                //    viewer.crystalReportViewer1.ReportSource = rep;
                //    viewer.crystalReportViewer1.Refresh();
                //    rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                //    cmd.Parameters.Clear();
                //    Process.Start(path);
                //}
                //con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
