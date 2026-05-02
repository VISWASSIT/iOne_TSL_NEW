using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.WinForms.GridCommon.ScrollAxis;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace ioneNet.FinanaceManagement.Reports
{
    using System.Configuration;
    using System.Data.SqlClient;
    using Excel = Microsoft.Office.Interop.Excel;
    using System.Diagnostics;
    using System.IO;
    using Syncfusion.Windows.Forms.Grid;
    using Ione_DAL;

    public partial class frmAccountLedger : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        private Database crDatabase;
        private Tables crTables;
        //private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();


        public static Boolean vchedit;
        public static string vchno, vchtype;
        public frmAccountLedger()
        {
            InitializeComponent();
        }

        private void frmStockLedgerNew_Load(object sender, EventArgs e)
        {
            //PictureBox2.Image = AppCode.GlobalAccess.comylogo;
            //var data = db.AccountMasters.Select(c => c.AccName).Distinct().ToArray();
            //AutoCompleteStringCollection instcol = new AutoCompleteStringCollection();
            //instcol.AddRange(data);
            //txtProductName.AutoCompleteCustomSource = instcol;
            // Account
            dpFromDate.MinDate = logIn.fy_Start_Date;
            dpTodate.MaxDate = logIn.fy_End_Date;
            var d = (from po in db.AccountMasters
                     join A in db.AccountGroups on po.AccGroup_ID equals A.ID
                     where po.Company_ID == logIn.company //&& A.GroupType == "Expenses"
                     select new { po.id, po.AccName }).Distinct().ToList();
            if (d.Count > 0)
            {
                cmbAccName.DataSource = d;
                cmbAccName.ValueMember = "id";
                cmbAccName.DisplayMember = "AccName";
            }

            if (FinanceManagement.Reports.frmTrailBalance.intAccCode >0)
            {

            dpFromDate.Value =logIn.fy_Start_Date;
            dpTodate.Value = FinanceManagement.Reports.frmTrailBalance.toDate;
            cmbAccName.SelectedValue = FinanceManagement.Reports.frmTrailBalance.intAccCode;
            btnFind_Click(sender, e);
            }
            vchedit = false;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                //DateTime f = dpFromDate.Value;
                //string f1 = f.ToString("yyyy/MM/dd");

                //DateTime d2 = dpTodate.Value;
                //string f2 = d2.ToString("yyyy/MM/dd");

                // dgStockdata.DataSource = db.sp_StockSummaryReport(txtProductName.Text, AppCode.GlobalAccess.companyName.Trim(), txtBIN.Text.Trim(), Convert.ToDateTime(f1), Convert.ToDateTime(f2), comboBox1.Text.Trim());
                // dgStockdata.DataSource = db.sp_StockReport_Daily(AppCode.GlobalAccess.companyName.Trim(), Convert.ToDateTime(f1), txtProductName.Text, txtBIN.Text.Trim(), comboBox1.Text.Trim());
                binddata();
                

                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            Cursor.Current = Cursors.Default;
        }
        public void binddata()
        {
            try
            {
                DateTime dt = dpFromDate.Value;
                string dt1 = dt.ToString("yyyy/MM/dd");

                DateTime dtt = dpTodate.Value;
                string dt2 = dtt.ToString("yyyy/MM/dd");

                //var data = db.Sp_ProductionTunnageReport(AppCode.GlobalAccess.companyName, Convert.ToDateTime(dt1), Convert.ToDateTime(dt2)).ToString();


                SqlCommand cmd2 = new SqlCommand("sp_AccountLedger", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);
                cmd2.Parameters.AddWithValue("@accName", Convert.ToInt32(cmbAccName.SelectedValue.ToString()));
                cmd2.Parameters.AddWithValue("@Fromdate", Convert.ToDateTime(dt1));
                cmd2.Parameters.AddWithValue("@Todate", Convert.ToDateTime(dt2));
                cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
                cmd2.Parameters.AddWithValue("@fystartdate", logIn.fy_Start_Date);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataSet ds2 = new DataSet();
                DataTable ds2 = new DataTable();
                // da2.Fill(ds2, "x");
                da2.Fill(ds2);
                dataGridView1.DataSource = ds2;
                //double OPQty = 0, RecQty = 0, IssQty = 0;
                decimal rec, iss, op, closing = 0;
                //op = (dataGridView1.Rows[0].Cells["Balance"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i - 1].Cells["Balance"].Value);
                rec = (dataGridView1.Rows[0].Cells["Debit"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[0].Cells["Debit"].Value);
                iss = (dataGridView1.Rows[0].Cells["Credit"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[0].Cells["Credit"].Value);
                if (rec - iss > 0)
                {
                    closing = rec - iss;
                    //closing = Convert.ToDecimal(dataGridView1.Rows[i-1].Cells["CBQty"].Value) + Convert.ToDecimal(dataGridView1.Rows[i].Cells["ReceiptQty"].Value) - Convert.ToDecimal(dataGridView1.Rows[i].Cells["IssuedQty"].Value);
                    dataGridView1.Rows[0].Cells["Balance"].Value = closing;
                    dataGridView1.Rows[0].Cells["BalType"].Value = "Dr";
                }
                else
                {
                    closing = iss - rec;
                    //closing = Convert.ToDecimal(dataGridView1.Rows[i-1].Cells["CBQty"].Value) + Convert.ToDecimal(dataGridView1.Rows[i].Cells["ReceiptQty"].Value) - Convert.ToDecimal(dataGridView1.Rows[i].Cells["IssuedQty"].Value);
                    dataGridView1.Rows[0].Cells["Balance"].Value = closing;
                    dataGridView1.Rows[0].Cells["BalType"].Value = "Cr";
                }
                rec = 0;
                iss = 0;
                closing = 0;
                //this.sfDataGrid1.Columns.Add(new GridUnBoundColumn() { HeaderText = "Balance", MappingName = "Balance", Expression = "" });
                //this.sfDataGrid1.Columns.Add(new GridUnBoundColumn() { HeaderText = "Bal Type", MappingName = "Bal_Tye", Expression = "" });

                //Get The Balance

                //double OPQty = 0, RecQty = 0, IssQty = 0;
                //decimal rec, iss, op, closing = 0;
                for (int i = 1; i < dataGridView1.Rows.Count - 2; i++)
                {
                    //OP //Column8
                    //var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    //var mappingName = sfDataGrid1.Columns[0].MappingName;
                    //// decimal op, closing;
                    //var Balance = (rowData.GetType().GetProperty("Balance").GetValue(rowData, null).ToString());
                    //var Debit = (rowData.GetType().GetProperty("Debit").GetValue(rowData, null).ToString());
                    //var Credit = (rowData.GetType().GetProperty("Credit").GetValue(rowData, null).ToString());
                    if (dataGridView1.Rows[i - 1].Cells["BalType"].Value == "Cr")
                    {
                        op =0- ((dataGridView1.Rows[i - 1].Cells["Balance"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i - 1].Cells["Balance"].Value));


                    }
                    else
                    {
                        op = (dataGridView1.Rows[i - 1].Cells["Balance"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i - 1].Cells["Balance"].Value);
                    }
                    rec = (dataGridView1.Rows[i].Cells["Debit"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i].Cells["Debit"].Value);
                    iss = (dataGridView1.Rows[i].Cells["Credit"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i].Cells["Credit"].Value);
                    //op = Convert.ToDecimal(Balance);
                    //rec = Convert.ToDecimal(Debit);
                    //iss = Convert.ToDecimal(Credit);

                    if (op + rec - iss > 0)
                    {
                        closing = op + rec - iss;
                        //closing = Convert.ToDecimal(dataGridView1.Rows[i-1].Cells["CBQty"].Value) + Convert.ToDecimal(dataGridView1.Rows[i].Cells["ReceiptQty"].Value) - Convert.ToDecimal(dataGridView1.Rows[i].Cells["IssuedQty"].Value);
                        //sfDataGrid1
                        dataGridView1.Rows[i].Cells["Balance"].Value = closing;
                        dataGridView1.Rows[i].Cells["BalType"].Value = "Dr";
                    }
                    else
                    {
                        closing = op + rec - iss;
                        //closing = Convert.ToDecimal(dataGridView1.Rows[i-1].Cells["CBQty"].Value) + Convert.ToDecimal(dataGridView1.Rows[i].Cells["ReceiptQty"].Value) - Convert.ToDecimal(dataGridView1.Rows[i].Cells["IssuedQty"].Value);
                        dataGridView1.Rows[i].Cells["Balance"].Value = Math.Abs(closing);
                        dataGridView1.Rows[i].Cells["BalType"].Value = "Cr";
                    }
                    //RecQty += (dataGridView1.Rows[i].Cells["Debit"].Value == DBNull.Value) ? Convert.ToDouble("00") : Convert.ToDouble(dataGridView1.Rows[i].Cells["Debit"].Value);
                    //IssQty += (dataGridView1.Rows[i].Cells["Credit"].Value == DBNull.Value) ? Convert.ToDouble("00") : Convert.ToDouble(dataGridView1.Rows[i].Cells["Credit"].Value);
                    //ClsQty += (dgStockdata.Rows[i].Cells["ClosingQty"].Value == DBNull.Value) ? Convert.ToDouble("00") : Convert.ToDouble(dgStockdata.Rows[i].Cells["ClosingQty"].Value);
                }



                //GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                //tableSummaryRow1.Name = "TableSummary";
                //tableSummaryRow1.ShowSummaryInRow = false;
                //tableSummaryRow1.Position = VerticalPosition.Bottom;

                //GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                //summaryColumn1.Name = "Total_Debit";
                //summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                //summaryColumn1.Format = "{Sum}";
                //summaryColumn1.MappingName = "Debit";

                //tableSummaryRow1.SummaryColumns.Add(summaryColumn1);
                //GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                //summaryColumn2.Name = "Total_Credit";
                //summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                //summaryColumn2.Format = "{Sum}";
                //summaryColumn2.MappingName = "Credit";
                //tableSummaryRow1.SummaryColumns.Add(summaryColumn2);
                //this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);




                //double OPQty = 0, RecQty = 0, IssQty = 0;
                //decimal rec, iss, op, closing = 0;
                //for (int i = 1; i < sf.Rows.Count - 1; i++)
                //{
                //    //OP //Column8

                //    // decimal op, closing;
                //    op = (dataGridView1.Rows[i - 1].Cells["Balance"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i - 1].Cells["Balance"].Value);
                //    rec = (dataGridView1.Rows[i].Cells["Debit"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i].Cells["Debit"].Value);
                //    iss = (dataGridView1.Rows[i].Cells["Credit"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i].Cells["Credit"].Value);
                //    if (op + rec - iss > 0)
                //    {
                //        closing = op + rec - iss;
                //        //closing = Convert.ToDecimal(dataGridView1.Rows[i-1].Cells["CBQty"].Value) + Convert.ToDecimal(dataGridView1.Rows[i].Cells["ReceiptQty"].Value) - Convert.ToDecimal(dataGridView1.Rows[i].Cells["IssuedQty"].Value);
                //        dataGridView1.Rows[i].Cells["Balance"].Value = closing;
                //        dataGridView1.Rows[i].Cells["BalType"].Value = "Dr";
                //    }
                //    else
                //    {
                //        closing = op + iss - rec;
                //        //closing = Convert.ToDecimal(dataGridView1.Rows[i-1].Cells["CBQty"].Value) + Convert.ToDecimal(dataGridView1.Rows[i].Cells["ReceiptQty"].Value) - Convert.ToDecimal(dataGridView1.Rows[i].Cells["IssuedQty"].Value);
                //        dataGridView1.Rows[i].Cells["Balance"].Value = closing;
                //        dataGridView1.Rows[i].Cells["BalType"].Value = "Cr";
                //    }
                //    RecQty += (dataGridView1.Rows[i].Cells["Debit"].Value == DBNull.Value) ? Convert.ToDouble("00") : Convert.ToDouble(dataGridView1.Rows[i].Cells["Debit"].Value);
                //    IssQty += (dataGridView1.Rows[i].Cells["Credit"].Value == DBNull.Value) ? Convert.ToDouble("00") : Convert.ToDouble(dataGridView1.Rows[i].Cells["Credit"].Value);
                //    //ClsQty += (dgStockdata.Rows[i].Cells["ClosingQty"].Value == DBNull.Value) ? Convert.ToDouble("00") : Convert.ToDouble(dgStockdata.Rows[i].Cells["ClosingQty"].Value);
                //}
                //dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["Debit"].Value = RecQty;
                //dataGridView1.Rows[dataGridView1.Rows.Count - 1].Cells["Credit"].Value = IssQty;

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
            try
            {

                if (dataGridView1.Rows.Count > 0)
                {
                    Microsoft.Office.Interop.Excel._Application app = new Microsoft.Office.Interop.Excel.Application();
                    // creating new WorkBook within Excel application  
                    Microsoft.Office.Interop.Excel._Workbook workbook = app.Workbooks.Add(Type.Missing);
                    // creating new Excelsheet in workbook  

                    //Microsoft.Office.Interop.Excel.ApplicationClass ExcelApp = new Microsoft.Office.Interop.Excel.ApplicationClass();
                    //Workbook xlWorkbook = ExcelApp.Workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                    Microsoft.Office.Interop.Excel._Worksheet worksheet = null;
                    var data = (from s in db.Company_Infos
                                where s.Id == logIn.company
                    select new
                                {
                                    s.Company_Name,
                                    Company_address = s.Address + ',' + s.City + ',' + s.State + ',' + s.Phone_No + ',' + s.E_Mail + ',' + s.Website + '.'
                                }).ToList();
                    worksheet = workbook.Sheets["Sheet1"];
                    worksheet.Cells[1, 1] = data[0].Company_Name.ToString();
                    worksheet.Cells[2, 1] = "Account Ledger";
                    worksheet.Cells[3, 4] = "Period :" + dpFromDate.Text + " To " + dpTodate.Text;
                    worksheet.Cells[3, 1] = "Account Name : " + cmbAccName.Text;
                    worksheet.Range["A1:d1"].MergeCells = true;
                    worksheet.Range["A2:d2"].MergeCells = true;
                    worksheet.Range["A3:c3"].MergeCells = true;
                    worksheet.Range["D3:F3"].MergeCells = true;



                    for (int j = 1; j < dataGridView1.Columns.Count; j++)
                    {
                        worksheet.Cells[4, j] = dataGridView1.Columns[j - 1].HeaderText;
                    }
                    ///*a*/pp.Visible = true;
                    // Storing Each row and column value to excel sheet
                    for (int k = 0; k < dataGridView1.Rows.Count; k++)
                    {
                        for (int l = 0; l < dataGridView1.Columns.Count-1; l++)
                        {
                            //if (l == 1)
                            //{
                            //    string d = dataGridView1.Rows[k].Cells[l].Value.ToString().Substring(0, 2);
                            //    string m = dataGridView1.Rows[k].Cells[l].Value.ToString().Substring(3, 2);
                            //    string y = dataGridView1.Rows[k].Cells[l].Value.ToString().Substring(6, 4);
                            //    worksheet.Cells[k + 4, l + 1] = d + '/' + m + '/' + y;
                            //}
                            //else
                            //{
                            worksheet.Cells[k + 5, l + 1] = (dataGridView1.Rows[k].Cells[l].Value == "" || dataGridView1.Rows[k].Cells[l].Value == null ||
                                dataGridView1.Rows[k].Cells[l].Value == DBNull.Value) ? "" : dataGridView1.Rows[k].Cells[l].Value.ToString();
                            //}
                            //app.Range[worksheet.Cells[k + 4, 2], worksheet.Cells[k + 4, 2]].NumberFormat
                            //   = "dd-MM-yyyy";
                        }
                    }



                    worksheet.PageSetup.PrintGridlines = true;
                    worksheet.PageSetup.LeftMargin = 1.00;
                    worksheet.PageSetup.RightMargin = 0.50;
                    worksheet.Columns.AutoFit();
                    app.Visible = true;

                    //ExportToExcel(dataGridView1, "Invoice_Report");
                }



                if (dataGridView1.Rows.Count > 0)
                {
                    ExportToExcel(dataGridView1, "Account_Ledger");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void ExportToExcel(DataGridView gridviewID, string excelFilename)
        {
            try
            {

                string path = Path.Combine(Directory.GetCurrentDirectory(), "" + excelFilename + ".xlsx");
                Excel.Application xlAppToExport = new Excel.Application();
                xlAppToExport.Workbooks.Add("");

                // ADD A WORKSHEET.
                Excel.Worksheet xlWorkSheetToExport = default(Excel.Worksheet);
                xlWorkSheetToExport = (Excel.Worksheet)xlAppToExport.Sheets["Sheet1"];

                FileInfo file = new FileInfo(path);
                if (file.Exists)//check file exsit or not
                {
                    file.Delete();
                }

                //int iRowCnt = 7;
                var data = (from s in db.Company_Infos
                            where s.Id == logIn.company
                            select new
                            {
                                s.Company_Name,
                                Company_address = s.Address + ',' + s.City + ',' + s.State + ',' + s.Phone_No + ',' + s.E_Mail + ',' + s.Website + '.'
                            }).ToList();


                xlWorkSheetToExport.Cells[1, 1] = data[0].Company_Name.ToString();
                Excel.Range range = xlWorkSheetToExport.Cells[1, 1] as Excel.Range;
                range.EntireRow.Font.Name = "Calibri";
                range.EntireRow.Font.Bold = true;
                range.EntireRow.Font.Size = 12;
                //xlWorkSheetToExport.Range["A1:M1"].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                //xlWorkSheetToExport.Range["A1:M1"].MergeCells = true;       // MERGE CELLS OF THE HEADER.                
                xlWorkSheetToExport.Cells[2, 1] = "Account Ledger";


                xlWorkSheetToExport.Cells[4, 1] = "Account Name" + cmbAccName.Text;
                //Excel.Range range1 = xlWorkSheetToExport.Cells[2, 1] as Excel.Range;
                //range1.EntireRow.Font.Name = "Calibri";
                //range1.EntireRow.Font.Bold = false;
                //range1.EntireRow.Font.Size = 12;
                //range1.RowHeight = 20;
                //xlWorkSheetToExport.Range["A2:M2"].WrapText = true;
                //xlWorkSheetToExport.Range["A2:M2"].MergeCells = true;
                // SHOW THE HEADER File Name
                string d = "";
                // SHOW THE HEADER File Name
                if (dpFromDate.Text != "" && dpTodate.Text != "")
                {
                    d = " From Date :" + dpFromDate.Text + ",      TO Date :" + dpTodate.Text;

                }

                else if (dpFromDate.Text != "" && dpTodate.Text != "" && cmbAccName.Text != "")
                {
                    d = " From Date :" + dpFromDate.Text + ",      TO Date :" + dpTodate.Text + ",    Account Name :" + cmbAccName.Text;

                }
                else
                {
                    if (dpFromDate.Text != "" && dpTodate.Text != "" && cmbAccName.Text != "")
                    {
                        d = " From Date :" + dpFromDate.Text + ",      TO Date :" + dpTodate.Text + ",     Account Name :" + cmbAccName.Text;

                    }
                }
                // string d = " From Date :" +dpFromDate.Text +",      TO Date :"+ (dpTodate.Text) +",      Customer Name :"+ txtCust_Prod_code.Text + "  ,  Product Name :"+txtProductName.Text;
                xlWorkSheetToExport.Cells[4, 1] = d;
                Excel.Range range5 = xlWorkSheetToExport.Cells[2, 1] as Excel.Range;
                range5.EntireRow.Font.Name = "Calibri";
                //  range5.EntireRow.Font.Bold = true;
                range5.EntireRow.Font.Size = 12;
                //xlWorkSheetToExport.Range["A5:M5"].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                //xlWorkSheetToExport.Range["A5:M5"].WrapText = true;
                //xlWorkSheetToExport.Range["A5:M5"].MergeCells = true;
                // MERGE CELLS OF THE HEADER.

                for (int i = 1; i < gridviewID.Columns.Count + 1; i++)
                {
                    xlWorkSheetToExport.Cells[6, i] = gridviewID.Columns[i - 1].HeaderText;
                }

                for (int i = 0; i < gridviewID.Rows.Count; i++)
                {
                    for (int j = 0; j < gridviewID.Columns.Count; j++)
                    {
                        if (gridviewID.Rows[i].Cells[j].Value != null)
                        {
                            Excel.Range range7 = xlWorkSheetToExport.Cells[i + 7, j + 1] as Excel.Range;

                            range7.NumberFormat = "@";

                            xlWorkSheetToExport.Cells[i + 7, j + 1] = gridviewID.Rows[i].Cells[j].Value.ToString();

                        }
                    }
                }
                
                xlWorkSheetToExport.Columns.AutoFit();
                xlAppToExport.DisplayAlerts = false;
                xlWorkSheetToExport.SaveAs(path);
                // CLEAR.
                xlAppToExport.Workbooks.Close();
                xlAppToExport.Quit();
                xlAppToExport = null;
                xlWorkSheetToExport = null;
                Process.Start(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void txtProductName_Leave(object sender, EventArgs e)
        {
            //if(txtProductName.Text !="")
            //{

                //var data = (from s in db.Products
                //            where s.Item_Name == txtProductName.Text
                //            select new
                //            {
                //                s.Alternative_Code                                
                //            }).ToList();
                
                //txtBIN.Text = data[0].Alternative_Code.ToString();

            //}
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void Label8_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {


                if (checkBox1.Checked)
                {
                   
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "AccountLedger_All.pdf");
                    //string path = @"D:\Invoice.pdf";
                    FileInfo fi1 = new FileInfo(path);

                    DateTime dt = dpFromDate.Value;
                    string dt1 = dt.ToString("yyyy/MM/dd");

                    DateTime dtt = dpTodate.Value;
                    string dt2 = dtt.ToString("yyyy/MM/dd");
                    if (fi1.Exists)
                    {
                        fi1.Delete();
                    }
                    SqlCommand cmd = new SqlCommand("sp_AccountLedger_all", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@compname", logIn.company);                    
                    cmd.Parameters.AddWithValue("@Fromdate", Convert.ToDateTime(dt1));
                    cmd.Parameters.AddWithValue("@Todate", Convert.ToDateTime(dt2));
                    cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                    cmd.Parameters.AddWithValue("@fystartdate", logIn.fy_Start_Date);


                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    DataTable Dt = new DataTable();

                    da.SelectCommand = cmd;
                    da.Fill(Dt);
                    //if (Dt.Rows.Count > 0)
                    //{
                    CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                    //Get Invoice Format Mapped to the Company

                    rep = new FinanceManagement.Reports.AccountLedger_All();

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
                    // rep.RecordSelectionFormula = "{ Invoice_labels.Inv_No} = '" + txtInvNo.Text + "' and { Invoice_labels.Item_Code} = " + cmbItemCode.Text + " and { Invoice_labels.Company_ID} = " + logIn.company + "";

                    ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                    // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                    viewer.crystalReportViewer1.ReportSource = rep;
                    viewer.crystalReportViewer1.Refresh();
                    rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);
                    Process.Start(path);
                    cmd.Parameters.Clear();
                    //}
                    con.Close();
                  
                }
                else
                {
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "AccountLedger.pdf");
                    //string path = @"D:\Invoice.pdf";
                    FileInfo fi1 = new FileInfo(path);

                    DateTime dt = dpFromDate.Value;
                    string dt1 = dt.ToString("yyyy/MM/dd");

                    DateTime dtt = dpTodate.Value;
                    string dt2 = dtt.ToString("yyyy/MM/dd");
                    if (fi1.Exists)
                    {
                        fi1.Delete();
                    }
                    SqlCommand cmd = new SqlCommand("sp_AccountLedger_rpt", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@compname", logIn.company);
                    cmd.Parameters.AddWithValue("@accName", Convert.ToInt32(cmbAccName.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@Fromdate", Convert.ToDateTime(dt1));
                    cmd.Parameters.AddWithValue("@Todate", Convert.ToDateTime(dt2));
                    cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                    cmd.Parameters.AddWithValue("@fystartdate", logIn.fy_Start_Date);


                    SqlDataAdapter da = new SqlDataAdapter(cmd);

                    DataTable Dt = new DataTable();

                    da.SelectCommand = cmd;
                    da.Fill(Dt);
                    //if (Dt.Rows.Count > 0)
                    //{
                    CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                    //Get Invoice Format Mapped to the Company

                    rep = new FinanceManagement.Reports.AccountLdger();

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
                    // rep.RecordSelectionFormula = "{ Invoice_labels.Inv_No} = '" + txtInvNo.Text + "' and { Invoice_labels.Item_Code} = " + cmbItemCode.Text + " and { Invoice_labels.Company_ID} = " + logIn.company + "";

                    ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                    // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                    viewer.crystalReportViewer1.ReportSource = rep;
                    viewer.crystalReportViewer1.Refresh();
                    rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);
                    Process.Start(path);
                    cmd.Parameters.Clear();
                    con.Close();
                    //}
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (dataGridView1.Rows[0].Cells["BalType"].Value == "Cr")
            vchedit = true;
            vchno = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["Voucher_No"].Value.ToString();
            vchtype = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["TransType"].Value.ToString();
            ioneNet.FinanaceManagement.AccountVoucher frm = new ioneNet.FinanaceManagement.AccountVoucher();
            frm.MdiParent = this.MdiParent;
            frm.Show();
        }
    }
}
