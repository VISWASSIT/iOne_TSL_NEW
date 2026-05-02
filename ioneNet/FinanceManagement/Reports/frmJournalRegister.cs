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
    public partial class frmJournalRegister : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmJournalRegister()
        {
            InitializeComponent();
        }

        private void frmJournalRegister_Load(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime t = dtpFrmDate.Value;
                DateTime t1 = dtpFrmDate.Value;
                string t2 = t.ToString("dd/MMM/yyyy");
                string t3 = t1.ToString("dd/MMM/yyyy");
               
                var d = (from data in db.Account_Vouchers where data.Company_ID == logIn.company && data.Voucher_Type == comboBox1.Text 
                         && data.Voucher_Date >= dtpFrmDate.Value && data.Voucher_Date<= dateTimePicker1.Value 
                         orderby data.Voucher_Date, data.Voucher_No 
                         select new
                         {
                             Voucher_Date = data.Voucher_Date,
                             data.Voucher_No,data.AccName,
                             data.Debit_Amount,data.Credit_Amount,
                             data.Narration,data.Doc_Ref_No,
                             data.Transaction_Ref_No,data.Transaction_Ref_Date }).ToList();
                if (d.Count > 0)
                {

                    sfDataGrid1.DataSource = d;

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
                summaryColumn1.MappingName = "Debit_Amount";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn1);
                GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                summaryColumn2.Name = "Tot_Credit";
                summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn2.Format = "{Sum}";
                summaryColumn2.MappingName = "Credit_Amount";

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

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime dt = dtpFrmDate.Value;
            System.Data.OleDb.OleDbConnection MyConnection;
            System.Data.DataTable DtSet;
            System.Data.OleDb.OleDbDataAdapter MyCommand;
            string dt1 = dt.ToString("dd/MM/yyyy");

            DateTime dtt = dateTimePicker1.Value;
            string dt2 = dtt.ToString("dd/MM/yyyy");
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 3;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            workBook.Worksheets[0].Range["A2"].Value = comboBox1.Text + " Report";
            workBook.Worksheets[0].Range["D2"].Value = "Period :" + dt1 + "-"+dt2 ;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\" + comboBox1.Text + "_Report.xls");
            string doc = Fname + "\\" + comboBox1.Text + "_Report.xls";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();

            //MessageBox.Show("Excel File Generated and Saved in " + Fname + " Folder");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
