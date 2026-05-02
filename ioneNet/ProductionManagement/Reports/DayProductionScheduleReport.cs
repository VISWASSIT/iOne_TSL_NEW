using Ione_DAL;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.WinForms.DataGridConverter;
using System.Diagnostics;
using Syncfusion.Data;

namespace ioneNet.ProductionManagement.Reports
{
    public partial class DayProductionScheduleReport : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public DayProductionScheduleReport()
        {
            InitializeComponent();
        }

        private void DayProductionScheduleReport_Load(object sender, EventArgs e)
        {

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                this.sfDataGrid1.TableSummaryRows.Clear();
                DateTime dt = dtpFrmDate.Value;
                string dt1 = dt.ToString("yyyy/MM/dd");

                DateTime dtt = dtpToDate.Value;
                string dt2 = dtt.ToString("yyyy/MM/dd");
                SqlCommand cmd2 = new SqlCommand("Prod_Day_Schedule_Report_Summary", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);
                cmd2.Parameters.AddWithValue("@formDate", dt);
                cmd2.Parameters.AddWithValue("@ToDate", dtt);
                //cmd2.Parameters.AddWithValue("@mtrlgrade", "");

                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataSet ds2 = new DataSet();
                DataTable ds2 = new DataTable();
                // da2.Fill(ds2, "x");
                da2.Fill(ds2);
                sfDataGrid1.DataSource = ds2;

                sfDataGrid1.Style.HeaderStyle.Font.Orientation = 45;
                sfDataGrid1.Style.HeaderStyle.Font.Bold = true;
                sfDataGrid1.HeaderRowHeight = 60;
                sfDataGrid1.Style.HeaderStyle.BackColor = Color.SkyBlue;
                sfDataGrid1.RowHeight = 25;
                sfDataGrid1.Style.CellStyle.BackColor  = Color.White;
                //var d = (from data in db.Prod_Day_Schedule_Report_Summary (logIn.company,dtpFrmDate.Value,dtpToDate.Value) select data).ToList();
                //if (d.Count > 0)
                //{
                //    //dgProductsList.DataSource = d;
                //    sfDataGrid1.DataSource = d;

                //    //  this.sfDataGrid1.QueryCellStyle += SfDataGrid1_QueryCellStyle;

                //    //this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                //    //this.sfDataGrid1.Columns["Section_Name"].FilterRowEditorType = "TextBox";
                //    //this.sfDataGrid1.Columns["Section_Name"].ShowFilterRowOptions = false;
                //    //this.sfDataGrid1.Columns["Section_Name"].ImmediateUpdateColumnFilter = true;
                //    //this.sfDataGrid1.Columns["Section_Name"].FilterRowCondition = FilterRowCondition.Contains;

                //    //this.sfDataGrid1.Columns["Material_Grade"].FilterRowEditorType = "TextBox";
                //    //this.sfDataGrid1.Columns["Material_Grade"].ShowFilterRowOptions = false;
                //    //this.sfDataGrid1.Columns["Material_Grade"].ImmediateUpdateColumnFilter = true;
                //    //this.sfDataGrid1.Columns["Material_Grade"].FilterRowCondition = FilterRowCondition.Contains;

                //    //this.sfDataGrid1.Columns["RM_Size"].FilterRowEditorType = "TextBox";
                //    //this.sfDataGrid1.Columns["RM_Size"].ShowFilterRowOptions = false;
                //    //this.sfDataGrid1.Columns["RM_Size"].ImmediateUpdateColumnFilter = true;
                //    //this.sfDataGrid1.Columns["RM_Size"].FilterRowCondition = FilterRowCondition.Contains;

                //    //this.sfDataGrid1.Columns["RM_Lot_No"].FilterRowEditorType = "TextBox";
                //    //this.sfDataGrid1.Columns["RM_Lot_No"].ShowFilterRowOptions = false;
                //    //this.sfDataGrid1.Columns["RM_Lot_No"].ImmediateUpdateColumnFilter = true;
                //    //this.sfDataGrid1.Columns["RM_Lot_No"].FilterRowCondition = FilterRowCondition.Contains;


                //    GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                //    tableSummaryRow1.Name = "TableSummary";
                //    tableSummaryRow1.ShowSummaryInRow = false;
                //    tableSummaryRow1.Position = VerticalPosition.Bottom;



                //    GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                //    summaryColumn2.Name = "Planned_Qty ";
                //    summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                //    summaryColumn2.Format = "Total : {Sum}";
                //    summaryColumn2.MappingName = "Planned_Qty";

                //    tableSummaryRow1.SummaryColumns.Add(summaryColumn2);

                //    GridSummaryColumn summaryColumn3 = new GridSummaryColumn();
                //    summaryColumn3.Name = "RM_Alloted";
                //    summaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                //    summaryColumn3.Format = "Total : {Sum}";
                //    summaryColumn3.MappingName = "RM_Alloted";

                //    tableSummaryRow1.SummaryColumns.Add(summaryColumn3);
                //    this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);


                //    // Creates the GridSummaryRow.
                //    GridSummaryRow groupSummaryRow1 = new GridSummaryRow();
                //    groupSummaryRow1.Name = "GroupSummary";
                //    groupSummaryRow1.ShowSummaryInRow = false;


                //    // Adds the GridSummaryColumn in SummaryColumns collection.
                //    //groupSummaryRow1.SummaryColumns.Add(GsummaryColumn1);

                //    GridSummaryColumn GsummaryColumn2 = new GridSummaryColumn();
                //    GsummaryColumn2.Name = "Planned_Qty";
                //    GsummaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                //    GsummaryColumn2.Format = "Total Planned Qty : {Sum:c}";
                //    GsummaryColumn2.MappingName = "Planned_Qty";

                //    // Adds the GridSummaryColumn in SummaryColumns collection.
                //    groupSummaryRow1.SummaryColumns.Add(GsummaryColumn2);

                //    GridSummaryColumn GsummaryColumn3 = new GridSummaryColumn();
                //    GsummaryColumn3.Name = "RM_Alloted";
                //    GsummaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                //    GsummaryColumn3.Format = "Total Alloted Qty : {Sum:c}";
                //    GsummaryColumn3.MappingName = "RM_Alloted";

                //    // Adds the GridSummaryColumn in SummaryColumns collection.
                //    groupSummaryRow1.SummaryColumns.Add(GsummaryColumn3);


                //    // Adds the summary row in the GroupSummaryRows collection.
                //    this.sfDataGrid1.GroupSummaryRows.Add(groupSummaryRow1);

                //}
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
            workBook.Worksheets[0].Range["A2"].Value = "Day Production Schedule Report";
            workBook.Worksheets[0].Range["D2"].Value = "Period :" + dt1 + "-" + dt2;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\Day_Production_Schedule_Report.xlsx");
            string doc = Fname + "\\Day_Production_Schedule_Report.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }
    }
}
