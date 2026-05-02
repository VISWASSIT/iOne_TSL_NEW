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
using System.Configuration;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.IO;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.WinForms.DataGridConverter;
using Ione_DAL;
namespace ioneNet.FinanceManagement.Reports
{
    public partial class frmGSTR_PurchaseData : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmGSTR_PurchaseData()
        {
            InitializeComponent();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            binddata();
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


                SqlCommand cmd2 = new SqlCommand("GSTRPurchaseData", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);                
                cmd2.Parameters.AddWithValue("@Fromdate", Convert.ToDateTime(dt1));
                cmd2.Parameters.AddWithValue("@Todate", Convert.ToDateTime(dt2));
                cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataSet ds2 = new DataSet();
                DataTable ds2 = new DataTable();
                // da2.Fill(ds2, "x");
                da2.Fill(ds2);
                sfDataGrid1.DataSource = ds2;

                this.sfDataGrid1.TableSummaryRows.Clear();

                GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                tableSummaryRow1.Name = "TableSummary";
                tableSummaryRow1.ShowSummaryInRow = false;
                tableSummaryRow1.Position = VerticalPosition.Bottom;

                GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                summaryColumn1.Name = "Total_Taxable_Value";
                summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn1.Format = "{Sum}";
                summaryColumn1.MappingName = "Taxable_Value";

                tableSummaryRow1.SummaryColumns.Add(summaryColumn1);
                GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                summaryColumn2.Name = "Total_CGST_Amnt";
                summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn2.Format = "{Sum}";
                summaryColumn2.MappingName = "CGST_Amnt";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn2);

                GridSummaryColumn summaryColumn3 = new GridSummaryColumn();
                summaryColumn3.Name = "Total_SGST_Amnt";
                summaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn3.Format = "{Sum}";
                summaryColumn3.MappingName = "SGST_Amnt";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn3);

                GridSummaryColumn summaryColumn4 = new GridSummaryColumn();
                summaryColumn4.Name = "Total_IGST_Amnt";
                summaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn4.Format = "{Sum}";
                summaryColumn4.MappingName = "IGST_Amnt";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn4);

                this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);




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

        private void btnExcell_Click(object sender, EventArgs e)
        {
            DateTime dt = dpFromDate.Value;
            string dt1 = dt.ToString("dd/MM/yyyy");

            DateTime dtt = dpTodate.Value;
            string dt2 = dtt.ToString("dd/MM/yyyy");
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 5;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            workBook.Worksheets[0].Range["A2"].Value = "Purchase Data - GSTR";
            workBook.Worksheets[0].Range["D2"].Value = "Period :" + dt1 + "-" + dt2;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\GST_Purchase.xlsx");
            string doc = Fname + "\\GST_Purchase.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
            //MessageBox.Show("Excel File Saved in " + Fname + " Folder");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmGSTR_PurchaseData_Load(object sender, EventArgs e)
        {
            dpFromDate.MinDate = logIn.fy_Start_Date;
            dpTodate.MaxDate = logIn.fy_End_Date;
        }
    }
}
