using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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
namespace ioneNet.MaterialManagement.Reports
{
    public partial class PurchaseSummary : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public PurchaseSummary()
        {
            InitializeComponent();
        }

        private void PurchaseSummary_Load(object sender, EventArgs e)
        {


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


                SqlCommand cmd2 = new SqlCommand("PurchaseSummary", con);
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
                //this.sfDataGrid1.GroupSummaryRows.Clear();
                GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                tableSummaryRow1.Name = "TableSummary";
                tableSummaryRow1.ShowSummaryInRow = false;
                tableSummaryRow1.Position = VerticalPosition.Bottom;

                GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                summaryColumn1.Name = "Total_Taxable_Value";
                summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn1.Format = "{Sum}";
                summaryColumn1.MappingName = "Tot_TaxableValue";

                tableSummaryRow1.SummaryColumns.Add(summaryColumn1);
                GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                summaryColumn2.Name = "Total_CGST_Amnt";
                summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn2.Format = "{Sum}";
                summaryColumn2.MappingName = "Tot_CGST_Amnt";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn2);

                GridSummaryColumn summaryColumn3 = new GridSummaryColumn();
                summaryColumn3.Name = "Total_SGST_Amnt";
                summaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn3.Format = "{Sum}";
                summaryColumn3.MappingName = "Tot_SGST_Amnt";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn3);

                GridSummaryColumn summaryColumn4 = new GridSummaryColumn();
                summaryColumn4.Name = "Total_IGST_Amnt";
                summaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn4.Format = "{Sum}";
                summaryColumn4.MappingName = "Tot_IGST_Amnt";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn4);

                GridSummaryColumn summaryColumn5 = new GridSummaryColumn();
                summaryColumn5.Name = "Total_INV_Amnt";
                summaryColumn5.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn5.Format = "{Sum}";
                summaryColumn5.MappingName = "Tot_Inv_Value";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn5);

                this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);

                // Creates the GridSummaryRow.
                //GridSummaryRow groupSummaryRow1 = new GridSummaryRow();
                //groupSummaryRow1.Name = "GroupSummary";
                //groupSummaryRow1.ShowSummaryInRow = false;
                //GridSummaryColumn GsummaryColumn1 = new GridSummaryColumn();
                //GsummaryColumn1.Name = "Total_Taxable_Value";
                //GsummaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                //GsummaryColumn1.Format = "{Sum}";
                //GsummaryColumn1.MappingName = "Taxable_Value";

                //// Adds the GridSummaryColumn in SummaryColumns collection.
                //groupSummaryRow1.SummaryColumns.Add(GsummaryColumn1);
                //// Adds the summary row in the GroupSummaryRows collection.

                //GridSummaryColumn GsummaryColumn2 = new GridSummaryColumn();
                //GsummaryColumn2.Name = "Total_CGST_Amnt";
                //GsummaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                //GsummaryColumn2.Format = "{Sum}";
                //GsummaryColumn2.MappingName = "CGST_Amnt";
                //// Adds the GridSummaryColumn in SummaryColumns collection.
                //groupSummaryRow1.SummaryColumns.Add(GsummaryColumn2);

                //GridSummaryColumn GsummaryColumn3 = new GridSummaryColumn();
                //GsummaryColumn3.Name = "Total_SGST_Amnt";
                //GsummaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                //GsummaryColumn3.Format = "{Sum}";
                //GsummaryColumn3.MappingName = "SGST_Amnt";
                //// Adds the GridSummaryColumn in SummaryColumns collection.
                //groupSummaryRow1.SummaryColumns.Add(GsummaryColumn3);

                //GridSummaryColumn GsummaryColumn4 = new GridSummaryColumn();
                //GsummaryColumn4.Name = "Total_IGST_Amnt";
                //GsummaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                //GsummaryColumn4.Format = "{Sum}";
                //GsummaryColumn4.MappingName = "IGST_Amnt";
                //// Adds the GridSummaryColumn in SummaryColumns collection.
                //groupSummaryRow1.SummaryColumns.Add(GsummaryColumn4);

                //// Adds the summary row in the GroupSummaryRows collection.
                //this.sfDataGrid1.GroupSummaryRows.Add(groupSummaryRow1);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            binddata();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExcell_Click(object sender, EventArgs e)
        {
            DateTime dt = dpFromDate.Value;
            string dt1 = dt.ToString("dd/MM/yyyy");

            DateTime dtt = dpTodate.Value;
            string dt2 = dtt.ToString("dd/MM/yyyy");
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 3;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:L100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            workBook.Worksheets[0].Range["A2"].Value = "Purchase Summary Report";
            workBook.Worksheets[0].Range["D2"].Value = "Period :" + dt1 + "-" + dt2;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\Purchase_Summary.xlsx");
            string doc = Fname + "\\Purchase_Summary.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();

        }
    }
}
