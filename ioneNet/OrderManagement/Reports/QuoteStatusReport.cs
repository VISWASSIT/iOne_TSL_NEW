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
//using Syncfusion.WinForms.DataGrid.DataGridConverter;
using Ione_DAL;
using Syncfusion.WinForms.DataGrid.Interactivity;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Syncfusion.WinForms.DataGrid.Events;

namespace ioneNet.OrderManagement.Reports
{
    public partial class QuoteStatusReport : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public QuoteStatusReport()
        {
            InitializeComponent();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            DateTime dt = dtpFrmDate.Value;
            string dt1 = dt.ToString("dd/MM/yyyy");

            DateTime dtt = dtpToDate.Value;
            string dt2 = dtt.ToString("dd/MM/yyyy");
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 5;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            workBook.Worksheets[0].Range["A2"].Value = "Quotation Status Report";
            workBook.Worksheets[0].Range["D2"].Value = "Period :" + dt1 + "-" + dt2;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\Quotation_Status_Report.xlsx");
            string doc = Fname + "\\Quotation_Status_Report.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                this.sfDataGrid1.TableSummaryRows.Clear();
                        
                var d1 = (from data in db.Quote_Status_Report(logIn.company, dtpFrmDate.Value, dtpToDate.Value, logIn.BU_ID) select data).ToList();
               
                if (d1.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d1;

                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["Quot_NO"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Quot_NO"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Quot_NO"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Quot_NO"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["Customer_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Customer_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Customer_Name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Customer_Name"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["prod_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["prod_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["prod_Name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["prod_Name"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["prod_grade"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["prod_grade"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["prod_grade"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["prod_grade"].FilterRowCondition = FilterRowCondition.Contains;



                    this.sfDataGrid1.Columns["Executive"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Executive"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Executive"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Executive"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["Quote_Status"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Quote_Status"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Quote_Status"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Quote_Status"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.QueryCellStyle += SfDataGrid1_QueryCellStyle;
                    GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                    tableSummaryRow1.Name = "TableSummary";
                    tableSummaryRow1.ShowSummaryInRow = false;
                    tableSummaryRow1.Position = VerticalPosition.Bottom;

                    GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                    summaryColumn1.Name = "Total Quot Qty";
                    summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn1.Format = "Total : {Sum}";
                    summaryColumn1.MappingName = "Quote_Qty";

                    tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

                    GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                    summaryColumn2.Name = "Total Ord Qty";
                    summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn2.Format = "Total : {Sum}";
                    summaryColumn2.MappingName = "Ord_Received_Qty";

                    tableSummaryRow1.SummaryColumns.Add(summaryColumn2);

                    GridSummaryColumn summaryColumn3 = new GridSummaryColumn();
                    summaryColumn3.Name = "Total Quot Value";
                    summaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn3.Format = "Total : {Sum}";
                    summaryColumn3.MappingName = "Quot_Value";

                    tableSummaryRow1.SummaryColumns.Add(summaryColumn3);

                    GridSummaryColumn summaryColumn4 = new GridSummaryColumn();
                    summaryColumn4.Name = "Total Ord Value";
                    summaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn4.Format = "Total : {Sum}";
                    summaryColumn4.MappingName = "Order_Value";

                    tableSummaryRow1.SummaryColumns.Add(summaryColumn4);


                    this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);

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
        public string Status = "";
        private void SfDataGrid1_QueryCellStyle(object sender, QueryCellStyleEventArgs e)
        {


            if (e.Column.MappingName == "Quote_Status")
            {
                if (e.DisplayText == "Created")
                {
                    Status = "Created";
                    e.Style.BackColor = Color.LightSkyBlue;
                    e.Style.TextColor = Color.Black;
                }
                else if (e.DisplayText == "Order Received")
                {
                    Status = "Order Received";
                    e.Style.BackColor = Color.LightGreen;
                    e.Style.TextColor = Color.DarkSlateBlue;

                }
                else if (e.DisplayText == "Lost")
                {
                    Status = "Lost";
                    e.Style.BackColor = Color.Red;
                    e.Style.TextColor = Color.Black;

                }
            }

            if (e.Column.MappingName == "Quote_Validity")
            {

                if (Status == "Created")
                {
                    DateTime dtvalid = Convert.ToDateTime(e.DisplayText);
                    if (dtvalid <= DateTime.Now)
                    {
                        e.Style.BackColor = Color.Orange;
                    }
                }
            }
        }
    }
}
