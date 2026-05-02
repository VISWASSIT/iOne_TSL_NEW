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
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
//using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid;
//using Syncfusion.Data;
using Syncfusion.WinForms.DataGridConverter;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.Data;
using Ione_DAL;
namespace ioneNet.FinanceManagement.Reports
{
    public partial class ReceivableReport_BillWise : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        public ReceivableReport_BillWise()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime t = dtpFrmDate.Value;
                string t1 = t.ToString("dd/MMM/yyyy");
                if (label4.Text == "RECEIVABLE REPORT - BILL WISE")
                {
                    dgReport.DataSource = null;
                    var d = (from data in db.ReceivableAgeing(logIn.company, dtpFrmDate.Value,logIn.BU_ID) select data).ToList();

                    if (d.Count > 0)
                    {
                        dgReport.DataSource = d;

                    }
                    con.Close();
                    (dgReport.Columns["Sel"] as GridCheckBoxColumn).AllowThreeState = false;
                    (dgReport.Columns["Sel"] as GridCheckBoxColumn).AllowCheckBoxOnHeader = true;
                    this.dgReport.FilterRowPosition = RowPosition.Top;
                    this.dgReport.Columns["AccName"].FilterRowEditorType = "TextBox";
                    this.dgReport.Columns["AccName"].ShowFilterRowOptions = false;
                    this.dgReport.Columns["AccName"].ImmediateUpdateColumnFilter = true;

                    this.dgReport.TableSummaryRows.Clear();
                    this.dgReport.GroupSummaryRows.Clear();
                    //this.sfDataGrid1.GroupSummaryRows.Clear();
                    GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                    tableSummaryRow1.Name = "TableSummary";
                    tableSummaryRow1.ShowSummaryInRow = false;
                    tableSummaryRow1.Position = VerticalPosition.Bottom;

                    GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                    summaryColumn1.Name = "Bill_Amount";
                    summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn1.Format = "{Sum}";
                    summaryColumn1.MappingName = "Bill_Amount";
                    tableSummaryRow1.SummaryColumns.Add(summaryColumn1);
                    GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                    summaryColumn2.Name = "AmtReceived";
                    summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn2.Format = "{Sum}";
                    summaryColumn2.MappingName = "AmtReceived";

                    tableSummaryRow1.SummaryColumns.Add(summaryColumn2);
                    GridSummaryColumn summaryColumn3 = new GridSummaryColumn();
                    summaryColumn3.Name = "DueAmount";
                    summaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn3.Format = "{Sum}";
                    summaryColumn3.MappingName = "DueAmount";
                    tableSummaryRow1.SummaryColumns.Add(summaryColumn3);

                    GridSummaryColumn summaryColumn4 = new GridSummaryColumn();
                    summaryColumn4.Name = "_0_30_Days";
                    summaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn4.Format = "{Sum}";
                    summaryColumn4.MappingName = "_0_30_Days";
                    tableSummaryRow1.SummaryColumns.Add(summaryColumn4);

                    GridSummaryColumn summaryColumn5 = new GridSummaryColumn();
                    summaryColumn5.Name = "_30_60_Days";
                    summaryColumn5.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn5.Format = "{Sum}";
                    summaryColumn5.MappingName = "_30_60_Days";
                    tableSummaryRow1.SummaryColumns.Add(summaryColumn5);


                    GridSummaryColumn summaryColumn6 = new GridSummaryColumn();
                    summaryColumn6.Name = "_60_90_Days";
                    summaryColumn6.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn6.Format = "{Sum}";
                    summaryColumn6.MappingName = "_60_90_Days";
                    tableSummaryRow1.SummaryColumns.Add(summaryColumn6);

                    GridSummaryColumn summaryColumn7 = new GridSummaryColumn();
                    summaryColumn7.Name = "_90_120_Days";
                    summaryColumn7.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn7.Format = "{Sum}";
                    summaryColumn7.MappingName = "_90_120_Days";
                    tableSummaryRow1.SummaryColumns.Add(summaryColumn7);


                    GridSummaryColumn summaryColumn8 = new GridSummaryColumn();
                    summaryColumn8.Name = "_120_180_Days";
                    summaryColumn8.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn8.Format = "{Sum}";
                    summaryColumn8.MappingName = "_120_180_Days";
                    tableSummaryRow1.SummaryColumns.Add(summaryColumn8);

                    GridSummaryColumn summaryColumn9 = new GridSummaryColumn();
                    summaryColumn9.Name = "_180_Days_and_Above";
                    summaryColumn9.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn9.Format = "{Sum}";
                    summaryColumn9.MappingName = "_180_Days_and_Above";
                    tableSummaryRow1.SummaryColumns.Add(summaryColumn9);




                    this.dgReport.TableSummaryRows.Add(tableSummaryRow1);


                    // Creates the GridSummaryRow.
                    GridSummaryRow groupSummaryRow1 = new GridSummaryRow();
                    groupSummaryRow1.Name = "GroupSummary";
                    groupSummaryRow1.ShowSummaryInRow = false;
                    GridSummaryColumn GsummaryColumn1 = new GridSummaryColumn();
                    GsummaryColumn1.Name = "Total_Bill_Amount";
                    GsummaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                    GsummaryColumn1.Format = "{Sum}";
                    GsummaryColumn1.MappingName = "Bill_Amount";

                    // Adds the GridSummaryColumn in SummaryColumns collection.
                    groupSummaryRow1.SummaryColumns.Add(GsummaryColumn1);
                    // Adds the summary row in the GroupSummaryRows collection.

                    GridSummaryColumn GsummaryColumn2 = new GridSummaryColumn();
                    GsummaryColumn2.Name = "Total_AmtReceived";
                    GsummaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                    GsummaryColumn2.Format = "{Sum}";
                    GsummaryColumn2.MappingName = "AmtReceived";
                    // Adds the GridSummaryColumn in SummaryColumns collection.
                    groupSummaryRow1.SummaryColumns.Add(GsummaryColumn2);

                    GridSummaryColumn GsummaryColumn3 = new GridSummaryColumn();
                    GsummaryColumn3.Name = "Total_DueAmount";
                    GsummaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                    GsummaryColumn3.Format = "{Sum}";
                    GsummaryColumn3.MappingName = "DueAmount";
                    // Adds the GridSummaryColumn in SummaryColumns collection.
                    groupSummaryRow1.SummaryColumns.Add(GsummaryColumn3);

                    GridSummaryColumn GsummaryColumn4 = new GridSummaryColumn();
                    GsummaryColumn4.Name = "_0_30_Days";
                    GsummaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                    GsummaryColumn4.Format = "{Sum}";
                    GsummaryColumn4.MappingName = "_0_30_Days";
                    groupSummaryRow1.SummaryColumns.Add(GsummaryColumn4);

                    GridSummaryColumn GsummaryColumn5 = new GridSummaryColumn();
                    GsummaryColumn5.Name = "_30_60_Days";
                    GsummaryColumn5.SummaryType = SummaryType.DoubleAggregate;
                    GsummaryColumn5.Format = "{Sum}";
                    GsummaryColumn5.MappingName = "_30_60_Days";
                    groupSummaryRow1.SummaryColumns.Add(GsummaryColumn5);


                    GridSummaryColumn GsummaryColumn6 = new GridSummaryColumn();
                    GsummaryColumn6.Name = "_60_90_Days";
                    GsummaryColumn6.SummaryType = SummaryType.DoubleAggregate;
                    GsummaryColumn6.Format = "{Sum}";
                    GsummaryColumn6.MappingName = "_60_90_Days";
                    groupSummaryRow1.SummaryColumns.Add(GsummaryColumn6);

                    GridSummaryColumn GsummaryColumn7 = new GridSummaryColumn();
                    GsummaryColumn7.Name = "_90_120_Days";
                    GsummaryColumn7.SummaryType = SummaryType.DoubleAggregate;
                    GsummaryColumn7.Format = "{Sum}";
                    GsummaryColumn7.MappingName = "_90_120_Days";
                    groupSummaryRow1.SummaryColumns.Add(GsummaryColumn7);


                    GridSummaryColumn GsummaryColumn8 = new GridSummaryColumn();
                    GsummaryColumn8.Name = "_120_180_Days";
                    GsummaryColumn8.SummaryType = SummaryType.DoubleAggregate;
                    GsummaryColumn8.Format = "{Sum}";
                    GsummaryColumn8.MappingName = "_120_180_Days";
                    groupSummaryRow1.SummaryColumns.Add(GsummaryColumn8);

                    GridSummaryColumn GsummaryColumn9 = new GridSummaryColumn();
                    GsummaryColumn9.Name = "_180_Days_and_Above";
                    GsummaryColumn9.SummaryType = SummaryType.DoubleAggregate;
                    GsummaryColumn9.Format = "{Sum}";
                    GsummaryColumn9.MappingName = "_180_Days_and_Above";
                    groupSummaryRow1.SummaryColumns.Add(GsummaryColumn9);





                    // Adds the summary row in the GroupSummaryRows collection.
                    this.dgReport.GroupSummaryRows.Add(groupSummaryRow1);
                }


                if (label4.Text == "PAYABLE REPORT - BILL WISE")
                {
                    dgReport.DataSource = null;
                    var d = (from data in db.PayableAgeing(logIn.company, dtpFrmDate.Value, logIn.BU_ID) select data).ToList();

                    if (d.Count > 0)
                    {
                        dgReport.DataSource = d;

                    }
                    con.Close();
                    (dgReport.Columns["Sel"] as GridCheckBoxColumn).AllowThreeState = false;
                    (dgReport.Columns["Sel"] as GridCheckBoxColumn).AllowCheckBoxOnHeader = true;
                    this.dgReport.FilterRowPosition = RowPosition.Top;
                    this.dgReport.Columns["AccName"].FilterRowEditorType = "TextBox";
                    this.dgReport.Columns["AccName"].ShowFilterRowOptions = false;
                    this.dgReport.Columns["AccName"].ImmediateUpdateColumnFilter = true;

                    this.dgReport.TableSummaryRows.Clear();
                    this.dgReport.GroupSummaryRows.Clear();
                    //this.sfDataGrid1.GroupSummaryRows.Clear();
                    GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                    tableSummaryRow1.Name = "TableSummary";
                    tableSummaryRow1.ShowSummaryInRow = false;
                    tableSummaryRow1.Position = VerticalPosition.Bottom;

                    GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                    summaryColumn1.Name = "Bill_Amount";
                    summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn1.Format = "{Sum}";
                    summaryColumn1.MappingName = "Bill_Amount";
                    tableSummaryRow1.SummaryColumns.Add(summaryColumn1);
                    GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                    summaryColumn2.Name = "AmtReceived";
                    summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn2.Format = "{Sum}";
                    summaryColumn2.MappingName = "AmtReceived";

                    tableSummaryRow1.SummaryColumns.Add(summaryColumn2);
                    GridSummaryColumn summaryColumn3 = new GridSummaryColumn();
                    summaryColumn3.Name = "DueAmount";
                    summaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn3.Format = "{Sum}";
                    summaryColumn3.MappingName = "DueAmount";
                    tableSummaryRow1.SummaryColumns.Add(summaryColumn3);


                    this.dgReport.TableSummaryRows.Add(tableSummaryRow1);


                    // Creates the GridSummaryRow.
                    GridSummaryRow groupSummaryRow1 = new GridSummaryRow();
                    groupSummaryRow1.Name = "GroupSummary";
                    groupSummaryRow1.ShowSummaryInRow = false;
                    GridSummaryColumn GsummaryColumn1 = new GridSummaryColumn();
                    GsummaryColumn1.Name = "Total_Bill_Amount";
                    GsummaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                    GsummaryColumn1.Format = "{Sum}";
                    GsummaryColumn1.MappingName = "Bill_Amount";

                    // Adds the GridSummaryColumn in SummaryColumns collection.
                    groupSummaryRow1.SummaryColumns.Add(GsummaryColumn1);
                    // Adds the summary row in the GroupSummaryRows collection.

                    GridSummaryColumn GsummaryColumn2 = new GridSummaryColumn();
                    GsummaryColumn2.Name = "Total_AmtReceived";
                    GsummaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                    GsummaryColumn2.Format = "{Sum}";
                    GsummaryColumn2.MappingName = "AmtPaid";
                    // Adds the GridSummaryColumn in SummaryColumns collection.
                    groupSummaryRow1.SummaryColumns.Add(GsummaryColumn2);

                    GridSummaryColumn GsummaryColumn3 = new GridSummaryColumn();
                    GsummaryColumn3.Name = "Total_DueAmount";
                    GsummaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                    GsummaryColumn3.Format = "{Sum}";
                    GsummaryColumn3.MappingName = "DueAmount";
                    // Adds the GridSummaryColumn in SummaryColumns collection.
                    groupSummaryRow1.SummaryColumns.Add(GsummaryColumn3);


                    // Adds the summary row in the GroupSummaryRows collection.
                    this.dgReport.GroupSummaryRows.Add(groupSummaryRow1);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ReceivableReport_BillWise_Load(object sender, EventArgs e)
        {
            dtpFrmDate.MinDate = logIn.fy_Start_Date;
            dtpFrmDate.MaxDate = logIn.fy_End_Date;
            string VchType = frmMain.frmname;
            string menuname = frmMain.menuName;
            switch (menuname)
            {
                case "Receivable Report- With Ageing":
                    label4.Text = "RECEIVABLE REPORT - BILL WISE";
                    break;
                case "Payable Report- With Ageing":
                    label4.Text = "PAYABLE REPORT - BILL WISE";
                    break;
            }

        }

        private void btnReminder_Click(object sender, EventArgs e)
        {
            try
            {


                SqlCommand cmd1 = new SqlCommand("delete  from temp_PaymentReminder", con);
                if (con.State != ConnectionState.Open)
                    con.Open();
                //con.Open();
                cmd1.ExecuteNonQuery();
                con.Close();

                for (int i = 2; i < dgReport.RowCount-1; i++)
                {
                    var rowData = dgReport.GetRecordAtRowIndex(i);
                    var currentCellValue = dgReport.CurrentCell.CellRenderer.GetControlValue();
                    var Acc_ID = dgReport.Columns["Acc_ID"].MappingName;
                    var Doc_Ref_No = dgReport.Columns["Doc_Ref_No"].MappingName;
                    var InvDate = dgReport.Columns["InvDate"].MappingName;
                    var Bill_Amount = dgReport.Columns["Bill_Amount"].MappingName;
                    var DueAmount = dgReport.Columns["DueAmount"].MappingName;
                    var Due_Date = dgReport.Columns["Due_Date"].MappingName;

                    var mappingName2 = dgReport.Columns["Sel"].MappingName;
                    //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                    var cellVaue3 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
                    if (cellVaue3 == "True")
                    {
                        var cellVaue = (rowData.GetType().GetProperty(Acc_ID).GetValue(rowData, null).ToString());
                        var cellVaue1 = (rowData.GetType().GetProperty(Doc_Ref_No).GetValue(rowData, null).ToString());
                        var cellVaue2 = (rowData.GetType().GetProperty(InvDate).GetValue(rowData, null).ToString());
                        var cellVaue4 = (rowData.GetType().GetProperty(Bill_Amount).GetValue(rowData, null).ToString());
                        var cellVaue5 = (rowData.GetType().GetProperty(DueAmount).GetValue(rowData, null).ToString());
                        var cellVaue6 = (rowData.GetType().GetProperty(Due_Date).GetValue(rowData, null).ToString());

                        SqlCommand cmd = new SqlCommand();
                        //inv_No1 = row.Cells["Invoice_No"].Value.ToString();
                        cmd.CommandText = "INSERT INTO temp_PaymentReminder ([Acc_ID],[Doc_Ref_No],[InvDate],[Bill_Amount],[DueAmount],[Due_Date],[Company_ID]) Values (@param1,@param2,@param3,@param4,@param5,@param6,@param7)";
                        cmd.Parameters.AddWithValue("@param1", cellVaue.ToString());
                        cmd.Parameters.AddWithValue("@param2", cellVaue1.ToString());
                        cmd.Parameters.AddWithValue("@param3", cellVaue2.ToString());
                        cmd.Parameters.AddWithValue("@param4", cellVaue4.ToString());
                        cmd.Parameters.AddWithValue("@param5", cellVaue5.ToString());
                        cmd.Parameters.AddWithValue("@param6", cellVaue6.ToString());
                        cmd.Parameters.AddWithValue("@param7", logIn.BU_ID);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                   
                Print();
                //Sales.MultiInvoiceReportViewer frm = new Sales.MultiInvoiceReportViewer();
                //frm.MdiParent = this.MdiParent;

                //frm.Show();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Print()
        {
            try
            {
                // string path1 = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.pdf");
                string path = Path.Combine(Directory.GetCurrentDirectory(), "ReminderLetter.pdf");
                //string path = @"D:\Invoice.pdf";
                FileInfo fi1 = new FileInfo(path);

                if (fi1.Exists)
                {
                    fi1.Delete();
                }
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                rep = new FinanceManagement.Reports.paymenReminder();
                crConnectionInfo.ServerName = frmMain.ServerIP;
                crConnectionInfo.DatabaseName = frmMain.Database;
                crConnectionInfo.UserID = frmMain.DBUserID;
                crConnectionInfo.Password = frmMain.Password;
                crDatabase = rep.Database;
                crTables = crDatabase.Tables;
                //Loop through all tables in the report and apply the connection information for each table.
                for (int i = 0; i < crTables.Count; i++)
                {
                    //  crTable = crTables[i];
                    crTableLogOnInfo = crTables[i].LogOnInfo;
                    crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                    crTables[i].ApplyLogOnInfo(crTableLogOnInfo);
                    //If your DatabaseName is changing at runtime, specify the table location. For example, when you are reporting off of a Northwind database on SQL server you should have the following line of code:

                }

                //rep.SetParameterValue("Creation_Company", frmLogin.Creation_Company);                 
                //rep.RecordSelectionFormula = "{ Invoice_labels.Inv_No} = '" + txtInvNo.Text + "' and { Invoice_labels.Item_Code} = " + cmbItemCode.Text + " and { Invoice_labels.Company_ID} = " + logIn.company + "";



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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime dt = dtpFrmDate.Value;
            System.Data.OleDb.OleDbConnection MyConnection;
            System.Data.DataTable DtSet;
            System.Data.OleDb.OleDbDataAdapter MyCommand;
            string dt1 = dt.ToString("dd/MM/yyyy");

            //DateTime dtt = dpTodate.Value;
            //string dt2 = dtt.ToString("dd/MM/yyyy");
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 3;
            var excelEngine = dgReport.ExportToExcel(dgReport.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            workBook.Worksheets[0].Range["A2"].Value = label4.Text;
            workBook.Worksheets[0].Range["D2"].Value = "As On :" + dt1;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\" + label4.Text + ".xls");
            string doc = Fname + "\\" + label4.Text + ".xls";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();


            //MessageBox.Show("Excel File Generated and Saved in " + Fname + " Folder");
        }

        private void viewPaymentReceiptDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {


            int i = dgReport.CurrentCell.RowIndex;
            var currentCellValue = dgReport.CurrentCell.CellRenderer.GetControlValue();
            var rowData = dgReport.GetRecordAtRowIndex(i);
            var mappingName = dgReport.Columns["Acc_ID"].MappingName;            
            var mappingName1 = dgReport.Columns["Supplier_InvNo"].MappingName;
            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            var cellvalue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());

            var uRole = (from m in db.Cust_Received_Bills where m.Company_ID == logIn.company && m.Bill_No == cellvalue1.ToString() && m.Customer_ID == Convert.ToInt32(cellVaue.ToString()) select new { m.Voucher_No,m.AmtReceived }).Distinct().ToList();
            if (uRole.Count > 0)
            {
                dataGridView1.DataSource = uRole;
            }
            groupBox1.Visible = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
        }
    }
}
