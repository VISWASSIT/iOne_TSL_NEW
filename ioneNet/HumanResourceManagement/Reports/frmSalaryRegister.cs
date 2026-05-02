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
    public partial class frmSalaryRegister : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();

        public frmSalaryRegister()
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
                    cmd2.Parameters.AddWithValue("@empGroup", Convert.ToInt32(cmbEmpGroup.SelectedValue));
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
                    SqlCommand cmd2 = new SqlCommand("SP_HR_GetSalaryReg ", con);
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
                summaryColumnOBQty1.Name = "BasicSal";
                summaryColumnOBQty1.SummaryType = SummaryType.DoubleAggregate;
                summaryColumnOBQty1.Format = "{Sum}";
                summaryColumnOBQty1.MappingName = "BasicSal";
                tableSummaryRow1.SummaryColumns.Add(summaryColumnOBQty1);


                GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                summaryColumn2.Name = "HRA";
                summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn2.Format = "{Sum}";
                summaryColumn2.MappingName = "HRA";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn2);

                GridSummaryColumn summaryColumnConv = new GridSummaryColumn();
                summaryColumnConv.Name = "CONV";
                summaryColumnConv.SummaryType = SummaryType.DoubleAggregate;
                summaryColumnConv.Format = "{Sum}";
                summaryColumnConv.MappingName = "CONV";
                tableSummaryRow1.SummaryColumns.Add(summaryColumnConv);


                GridSummaryColumn summaryColumnReceipt2 = new GridSummaryColumn();
                summaryColumnReceipt2.Name = "LTA";
                summaryColumnReceipt2.SummaryType = SummaryType.DoubleAggregate;
                summaryColumnReceipt2.Format = "{Sum}";
                summaryColumnReceipt2.MappingName = "LTA";
                tableSummaryRow1.SummaryColumns.Add(summaryColumnReceipt2);

                GridSummaryColumn summaryColumnExp3 = new GridSummaryColumn();
                summaryColumnExp3.Name = "EXP";
                summaryColumnExp3.SummaryType = SummaryType.DoubleAggregate;
                summaryColumnExp3.Format = "{Sum}";
                summaryColumnExp3.MappingName = "EXP";
                tableSummaryRow1.SummaryColumns.Add(summaryColumnExp3);

                GridSummaryColumn summaryColumnARRS4 = new GridSummaryColumn();
                summaryColumnARRS4.Name = "ARS";
                summaryColumnARRS4.SummaryType = SummaryType.DoubleAggregate;
                summaryColumnARRS4.Format = "{Sum}";
                summaryColumnARRS4.MappingName = "ARS";
                tableSummaryRow1.SummaryColumns.Add(summaryColumnARRS4);

                GridSummaryColumn summaryColumnOT = new GridSummaryColumn();
                summaryColumnOT.Name = "OtAmt";
                summaryColumnOT.SummaryType = SummaryType.DoubleAggregate;
                summaryColumnOT.Format = "{Sum}";
                summaryColumnOT.MappingName = "OtAmt";
                tableSummaryRow1.SummaryColumns.Add(summaryColumnOT);

                GridSummaryColumn summaryColumnReceipt4 = new GridSummaryColumn();
                summaryColumnReceipt4.Name = "OtherEarnings";
                summaryColumnReceipt4.SummaryType = SummaryType.DoubleAggregate;
                summaryColumnReceipt4.Format = "{Sum}";
                summaryColumnReceipt4.MappingName = "OtherEarnings";
                tableSummaryRow1.SummaryColumns.Add(summaryColumnReceipt4);

                GridSummaryColumn summaryColumn3 = new GridSummaryColumn();
                summaryColumn3.Name = "TotEarnings";
                summaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn3.Format = "{Sum}";
                summaryColumn3.MappingName = "TotEarnings";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn3);

                GridSummaryColumn summaryColumnIss3 = new GridSummaryColumn();
                summaryColumnIss3.Name = "PFAmt";
                summaryColumnIss3.SummaryType = SummaryType.DoubleAggregate;
                summaryColumnIss3.Format = "{Sum}";
                summaryColumnIss3.MappingName = "PFAmt";
                tableSummaryRow1.SummaryColumns.Add(summaryColumnIss3);

                GridSummaryColumn summaryColumn4 = new GridSummaryColumn();
                summaryColumn4.Name = "ESIAmt";
                summaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn4.Format = "{Sum}";
                summaryColumn4.MappingName = "ESIAmt";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn4);

                GridSummaryColumn summaryColumnCBQty = new GridSummaryColumn();
                summaryColumnCBQty.Name = "PTAmount";
                summaryColumnCBQty.SummaryType = SummaryType.DoubleAggregate;
                summaryColumnCBQty.Format = "{Sum}";
                summaryColumnCBQty.MappingName = "PTAmount";
                tableSummaryRow1.SummaryColumns.Add(summaryColumnCBQty);

                GridSummaryColumn summaryColumnTDS = new GridSummaryColumn();
                summaryColumnTDS.Name = "TDS";
                summaryColumnTDS.SummaryType = SummaryType.DoubleAggregate;
                summaryColumnTDS.Format = "{Sum}";
                summaryColumnTDS.MappingName = "TDS";
                tableSummaryRow1.SummaryColumns.Add(summaryColumnTDS);

                GridSummaryColumn summaryColumnAdvDed = new GridSummaryColumn();
                summaryColumnAdvDed.Name = "AdvDed";
                summaryColumnAdvDed.SummaryType = SummaryType.DoubleAggregate;
                summaryColumnAdvDed.Format = "{Sum}";
                summaryColumnAdvDed.MappingName = "AdvDed";
                tableSummaryRow1.SummaryColumns.Add(summaryColumnAdvDed);

                GridSummaryColumn summaryColumnOthDed = new GridSummaryColumn();
                summaryColumnOthDed.Name = "OtherDeductions";
                summaryColumnOthDed.SummaryType = SummaryType.DoubleAggregate;
                summaryColumnOthDed.Format = "{Sum}";
                summaryColumnOthDed.MappingName = "OtherDeductions";
                tableSummaryRow1.SummaryColumns.Add(summaryColumnOthDed);

                GridSummaryColumn summaryColumnTotDed = new GridSummaryColumn();
                summaryColumnTotDed.Name = "Tot_Deductions";
                summaryColumnTotDed.SummaryType = SummaryType.DoubleAggregate;
                summaryColumnTotDed.Format = "{Sum}";
                summaryColumnTotDed.MappingName = "Tot_Deductions";
                tableSummaryRow1.SummaryColumns.Add(summaryColumnTotDed);

                GridSummaryColumn summaryColumnNetPay = new GridSummaryColumn();
                summaryColumnNetPay.Name = "Net_Pay";
                summaryColumnNetPay.SummaryType = SummaryType.DoubleAggregate;
                summaryColumnNetPay.Format = "{Sum}";
                summaryColumnNetPay.MappingName = "Net_Pay";
                tableSummaryRow1.SummaryColumns.Add(summaryColumnNetPay);

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
                                 join emp in db.HR_Employee_Master_Datas on m.ID equals  emp.Emp_Group
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
            workBook.Worksheets[0].Range["A1"].Value = logIn.BU_Name;
            workBook.Worksheets[0].Range["A2"].Value = "Salary Register";
            workBook.Worksheets[0].Range["D2"].Value = "for the Month of :" + cmbMonth.Text +'-'+ cmbYear.Text;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\Salary_Register.xlsx");
            string doc = Fname + "\\Salary_Register.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void sfButton1_Click(object sender, EventArgs e)
        {
            try
            {
                //crys viewer1 = new CrstalReportViewer1();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "PaySlip.pdf");
                //string path = Path.Combine(Directory.GetCurrentDirectory(), @"\Purchase_Order.pdf");
                System.IO.FileInfo fi = new System.IO.FileInfo(path);
                Cursor.Current = Cursors.WaitCursor;
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                //     rep = new MaterialManagement.Transactions.PurchaseOrder();



                //int i = sfDataGrid1.CurrentCell.RowIndex;
                //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                //var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                //var mappingName = sfDataGrid1.Columns["Rec_ID"].MappingName;
                ////var mappingName1 = sfDataGrid1.Columns[4].MappingName;
                //var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                if (logIn.company == 21 || logIn.company == 24)
                {
                    rep = new HumanResourceManagement.Reports.PaySlip_RVPR();
                    SqlCommand cmd = new SqlCommand("SP_HR_GetPaySlip_RVPR", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@compname", logIn.company);
                    cmd.Parameters.AddWithValue("@strmonth", cmbMonth.Text);
                    cmd.Parameters.AddWithValue("@strYear", cmbYear.Text);
                    cmd.Parameters.AddWithValue("@empGroup", Convert.ToInt32(cmbEmpGroup.SelectedValue));
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
                        viewer.crystalReportViewer1.ReportSource = rep;
                        viewer.crystalReportViewer1.Refresh();
                        rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                        cmd.Parameters.Clear();
                        Process.Start(path);
                    }
                }
                else
                {
                    if (logIn.company == 20)
                    {
                        rep = new HumanResourceManagement.Reports.PaySlip_V_Pack();
                    }
                    else if (logIn.company ==1039)
                    {
                        rep = new HumanResourceManagement.Reports.WageSlip();
                    }
                    else
                    { 
                    rep = new HumanResourceManagement.Reports.PaySlip_OTH();
                    }
                    SqlCommand cmd = new SqlCommand("SP_HR_GetPaySlip", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@compname", logIn.company);
                    cmd.Parameters.AddWithValue("@strmonth", cmbMonth.Text);
                    cmd.Parameters.AddWithValue("@strYear", cmbYear.Text);
                    cmd.Parameters.AddWithValue("@empGroup", Convert.ToInt32(cmbEmpGroup.SelectedValue));
                    cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                    cmd.Parameters.AddWithValue("@empid", 0);
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
                        viewer.crystalReportViewer1.ReportSource = rep;
                        viewer.crystalReportViewer1.Refresh();                    
                        rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                        cmd.Parameters.Clear();
                        Process.Start(path);
                    }
                }
                con.Close();
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
                string path = Path.Combine(Directory.GetCurrentDirectory(), "PaySlip.pdf");
                //string path = Path.Combine(Directory.GetCurrentDirectory(), @"\Purchase_Order.pdf");
                System.IO.FileInfo fi = new System.IO.FileInfo(path);
                Cursor.Current = Cursors.WaitCursor;
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                //     rep = new MaterialManagement.Transactions.PurchaseOrder();
                if (logIn.company == 20)
                {
                    rep = new HumanResourceManagement.Reports.PaySlip_V_Pack();
                }
                else if (logIn.company == 1039)
                {
                    rep = new HumanResourceManagement.Reports.WageSlip();
                }
                else
                {
                    rep = new HumanResourceManagement.Reports.PaySlip_OTH();
                }

                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["EMP_ID"].MappingName;
                ////var mappingName1 = sfDataGrid1.Columns[4].MappingName;
                //var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

               
                SqlCommand cmd = new SqlCommand("SP_HR_GetPaySlip", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@compname", logIn.company);
                cmd.Parameters.AddWithValue("@strmonth", cmbMonth.Text);
                cmd.Parameters.AddWithValue("@strYear", cmbYear.Text);
                cmd.Parameters.AddWithValue("@empGroup", Convert.ToInt32(cmbEmpGroup.SelectedValue));
                cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                cmd.Parameters.AddWithValue("@empid", Convert.ToInt32(currentCellValue));
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
                    viewer.crystalReportViewer1.ReportSource = rep;
                    viewer.crystalReportViewer1.Refresh();
                    rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                    cmd.Parameters.Clear();
                    Process.Start(path);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }
    }
}
