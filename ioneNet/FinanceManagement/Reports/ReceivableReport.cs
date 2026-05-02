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
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.IO;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.Data;
using Syncfusion.WinForms.DataGridConverter;
using Ione_DAL;

namespace ioneNet.FinanceManagement.Reports
{
    public partial class ReceivableReport : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public ReceivableReport()
        {
            InitializeComponent();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime t = dtpFrmDate.Value;
                string t1 = t.ToString("dd/MMM/yyyy");
                if (label4.Text == "RECEIVABLE REPORT")
                {
                    var d = (from data in db.GetReceivable_Payable_Report(logIn.company, dtpFrmDate.Value, 0,logIn.BU_ID) select data).ToList();
                    if (d.Count > 0)
                    {

                        sfDataGrid1.DataSource = d;

                    }
                }
                else
                {
                    var d = (from data in db.GetReceivable_Payable_Report(logIn.company, dtpFrmDate.Value, 4,logIn.BU_ID) select data).ToList();
                    if (d.Count > 0)
                    {

                        sfDataGrid1.DataSource = d;

                    }
                }
                
                this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                this.sfDataGrid1.Columns["AccName"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["AccName"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["AccName"].ImmediateUpdateColumnFilter = true;

                this.sfDataGrid1.TableSummaryRows.Clear();
                //this.sfDataGrid1.GroupSummaryRows.Clear();
                GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                tableSummaryRow1.Name = "TableSummary";
                tableSummaryRow1.ShowSummaryInRow = false;
                tableSummaryRow1.Position = VerticalPosition.Bottom;

                GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                summaryColumn1.Name = "Tot_Debit";
                summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                
                summaryColumn1.Format = "{Sum}";
                summaryColumn1.MappingName = "Debit";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn1);
                GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                summaryColumn2.Name = "Tot_Credit";
                summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn2.Format = "{Sum}";
                summaryColumn2.MappingName = "Credit";

                tableSummaryRow1.SummaryColumns.Add(summaryColumn2);

                this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);

               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ReceivableReport_Load(object sender, EventArgs e)
        {
            dtpFrmDate.MinDate = logIn.fy_Start_Date;
            dtpFrmDate.MaxDate = logIn.fy_End_Date;

            string VchType = frmMain.menuName;
            switch (VchType)
            {
                case "Receivable Report":
                    label4.Text = "RECEIVABLE REPORT";                    
                    break;
                case "Payable Report":
                    label4.Text = "PAYABLE REPORT";
                    break;
            }
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
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
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
            workBook.SaveAs(Fname + "\\"+ label4.Text+".xls");
            string doc = Fname + "\\" + label4.Text + ".xls";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();

            MessageBox.Show("Excel File Generated and Saved in " + Fname + " Folder");
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

                int iRowCnt = 7;
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
                xlWorkSheetToExport.Cells[2, 1] = "Receivable Repot as on "+ dtpFrmDate.Text ;


               // xlWorkSheetToExport.Cells[4, 1] = "Account Name" + cmbAccName.Text;
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
    }
}
