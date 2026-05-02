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
namespace ioneNet.MaterialManagement.Reports
{
   
    public partial class GoodsInwardReport : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public GoodsInwardReport()
        {
            InitializeComponent();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            sfDataGrid1.DataSource = null;
            var d = (from data in db.GoodsInwardRegister(logIn.company,dtpFrmDate.Value,dtpToDate.Value,logIn.BU_ID) select data).ToList();
            if (d.Count > 0)
            {
                //dgProductsList.DataSource = d;
                sfDataGrid1.DataSource = d;
           
            this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
            this.sfDataGrid1.Columns["Supplier_Name"].FilterRowEditorType = "TextBox";
            this.sfDataGrid1.Columns["Supplier_Name"].ShowFilterRowOptions = false;
            this.sfDataGrid1.Columns["Supplier_Name"].ImmediateUpdateColumnFilter = true;

            this.sfDataGrid1.Columns["Grn_NO"].FilterRowEditorType = "TextBox";
            this.sfDataGrid1.Columns["Grn_NO"].ShowFilterRowOptions = false;
            this.sfDataGrid1.Columns["PO_No"].FilterRowEditorType = "TextBox";

            this.sfDataGrid1.Columns["PO_No"].ShowFilterRowOptions = false;
            this.sfDataGrid1.Columns["Prod_Name"].FilterRowEditorType = "TextBox";
            this.sfDataGrid1.Columns["Prod_Name"].ShowFilterRowOptions = false;
            this.sfDataGrid1.Columns["Prod_Name"].ImmediateUpdateColumnFilter = true;
            this.sfDataGrid1.Columns["Supplier_InvNo"].ShowFilterRowOptions = false;


            this.sfDataGrid1.TableSummaryRows.Clear();
            GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
            tableSummaryRow1.Name = "TableSummary";
            tableSummaryRow1.ShowSummaryInRow = false;
            tableSummaryRow1.Position = VerticalPosition.Bottom;

            GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
            summaryColumn1.Name = "Total Amount";
            summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
            summaryColumn1.Format = "{Sum}";
            summaryColumn1.MappingName = "Amount";
            tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

            GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
            summaryColumn2.Name = "Total Disc Amount";
            summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
            summaryColumn2.Format = "{Sum}";
            summaryColumn2.MappingName = "Disc_Amount";

            tableSummaryRow1.SummaryColumns.Add(summaryColumn2);

            GridSummaryColumn summaryColumn3 = new GridSummaryColumn();
            summaryColumn3.Name = "Total Taxable Value";
            summaryColumn3.SummaryType = SummaryType.DoubleAggregate;
            summaryColumn3.Format = "{Sum}";
            summaryColumn3.MappingName = "Taxable_Value";

            tableSummaryRow1.SummaryColumns.Add(summaryColumn3);

           // tableSummaryRow1.SummaryColumns.Add(summaryColumn2);

            GridSummaryColumn summaryColumn4 = new GridSummaryColumn();
            summaryColumn4.Name = "Total CGST Amount";
            summaryColumn4.SummaryType = SummaryType.DoubleAggregate;
            summaryColumn4.Format = "{Sum}";
            summaryColumn4.MappingName = "CGST_Amnt";

            tableSummaryRow1.SummaryColumns.Add(summaryColumn4);

            GridSummaryColumn summaryColumn5 = new GridSummaryColumn();
            summaryColumn5.Name = "Total SGST Amount";
            summaryColumn5.SummaryType = SummaryType.DoubleAggregate;
            summaryColumn5.Format = "{Sum}";
            summaryColumn5.MappingName = "SGST_Amnt";

            tableSummaryRow1.SummaryColumns.Add(summaryColumn5);

            GridSummaryColumn summaryColumn6 = new GridSummaryColumn();
            summaryColumn6.Name = "Total IGST Amount";
            summaryColumn6.SummaryType = SummaryType.DoubleAggregate;
            summaryColumn6.Format = "{Sum}";
            summaryColumn6.MappingName = "IGST_Amnt";

            tableSummaryRow1.SummaryColumns.Add(summaryColumn6);

            GridSummaryColumn summaryColumn7= new GridSummaryColumn();
            summaryColumn7.Name = "Total  Net Amount";
            summaryColumn7.SummaryType = SummaryType.DoubleAggregate;
            summaryColumn7.Format = "{Sum}";
            summaryColumn7.MappingName = "Net_Amount";

            tableSummaryRow1.SummaryColumns.Add(summaryColumn7);
            GridSummaryColumn summaryColumn8 = new GridSummaryColumn();
            summaryColumn8.Name = "Total  Qty";
            summaryColumn8.SummaryType = SummaryType.DoubleAggregate;
            summaryColumn8.Format = "{Sum}";
            summaryColumn8.MappingName = "AcceptedQty";

            tableSummaryRow1.SummaryColumns.Add(summaryColumn8);

            this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExcell_Click(object sender, EventArgs e)
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
            workBook.Worksheets[0].Range["A2"].Value = "Goods Inward Register";
            workBook.Worksheets[0].Range["D2"].Value = "Period :" + dt1 + "-" + dt2;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\GRN_Report.xlsx");
            string doc = Fname + "\\GRN_Report.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }
    }
}
