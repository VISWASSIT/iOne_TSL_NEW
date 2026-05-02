using Ione_DAL;
using Syncfusion.Data;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Events;
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
using Syncfusion.WinForms.DataGrid.Interactivity;
using System.Diagnostics;
using Syncfusion.WinForms.DataGridConverter;

namespace ioneNet.ProductionManagement.Reports
{
    public partial class Production_Report_Rolling : Form
    {
        public static string InvoiceNoList, Order_NoList, SO_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;

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
            workBook.Worksheets[0].Range["A2"].Value = "Production Report";
            workBook.Worksheets[0].Range["D2"].Value = "Period :" + dt1 + "-" + dt2;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\Production_Report.xlsx");
            string doc = Fname + "\\Production_Report.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public Production_Report_Rolling()
        {
            InitializeComponent();
        }

        private void Production_Report_Rolling_Load(object sender, EventArgs e)
        {

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                this.sfDataGrid1.TableSummaryRows.Clear();
                
                    var d = (from data in db.SP_Production_Report_Rolling(logIn.company, dtpFrmDate.Value, dtpToDate.Value) select data).ToList();
                    if (d.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        sfDataGrid1.DataSource = d;
                    
                    this.sfDataGrid1.QueryCellStyle += SfDataGrid1_QueryCellStyle;

                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["Section_Rolled"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Section_Rolled"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Section_Rolled"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Section_Rolled"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["Material_Grade"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Material_Grade"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Material_Grade"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Material_Grade"].FilterRowCondition = FilterRowCondition.Contains;
                   

                    GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                    tableSummaryRow1.Name = "TableSummary";
                    tableSummaryRow1.ShowSummaryInRow = false;
                    tableSummaryRow1.Position = VerticalPosition.Bottom;

                    GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                    summaryColumn1.Name = "Rolled_Qty";
                    summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn1.Format = "Total : {Sum}";
                    summaryColumn1.MappingName = "Rolled_Qty";

                    tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

                    GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                    summaryColumn2.Name = "Qty_Finished";
                    summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn2.Format = "Total : {Sum}";
                    summaryColumn2.MappingName = "Qty_Finished";

                    tableSummaryRow1.SummaryColumns.Add(summaryColumn2);

                    GridSummaryColumn summaryColumn3 = new GridSummaryColumn();
                    summaryColumn3.Name = "Qty_Rejected";
                    summaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn3.Format = "Total : {Sum}";
                    summaryColumn3.MappingName = "Qty_Rejected";

                    tableSummaryRow1.SummaryColumns.Add(summaryColumn3);
                    this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);


                    // Creates the GridSummaryRow.
                    GridSummaryRow groupSummaryRow1 = new GridSummaryRow();
                    groupSummaryRow1.Name = "GroupSummary";
                    groupSummaryRow1.ShowSummaryInRow = false;

                    // Creates the GridSummaryColumn.
                    GridSummaryColumn GsummaryColumn1 = new GridSummaryColumn();
                    GsummaryColumn1.Name = "Rolled_Qty";
                    GsummaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                    GsummaryColumn1.Format = "Total Rolled Qty : {Sum:c}";
                    GsummaryColumn1.MappingName = "Rolled_Qty";

                    // Adds the GridSummaryColumn in SummaryColumns collection.
                    groupSummaryRow1.SummaryColumns.Add(GsummaryColumn1);

                    GridSummaryColumn GsummaryColumn2 = new GridSummaryColumn();
                    GsummaryColumn2.Name = "Qty_Finished";
                    GsummaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                    GsummaryColumn2.Format = "Total Finished Qty : {Sum:c}";
                    GsummaryColumn2.MappingName = "Qty_Finished";

                    // Adds the GridSummaryColumn in SummaryColumns collection.
                    groupSummaryRow1.SummaryColumns.Add(GsummaryColumn2);



                    // Adds the summary row in the GroupSummaryRows collection.
                    this.sfDataGrid1.GroupSummaryRows.Add(groupSummaryRow1);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SfDataGrid1_QueryCellStyle(object sender, QueryCellStyleEventArgs e)
        {
            if (e.Column.MappingName == "EC_Per")
            {
                string V = e.DisplayText;
                if (V != "")
                {

                    if (Convert.ToDecimal(V) <= 5)
                    {
                        //e.Style.BackColor = Color.Gray;
                        e.Style.TextColor = Color.Green;
                    }
                    else
                    {
                        //e.Style.BackColor = Color.Gray;
                        e.Style.TextColor = Color.Red;
                    }
                }
            }

            if (e.Column.MappingName == "Rolled_For")
            {
                if (e.DisplayText == "TATA")
                {
                    e.Style.BackColor = Color.Orange;
                    e.Style.TextColor = Color.Black;
                }
                else if (e.DisplayText == "RINL")
                {
                    e.Style.BackColor = Color.SkyBlue;
                    e.Style.TextColor = Color.Black;
                }
                else if (e.DisplayText == "Own")
                {
                    e.Style.BackColor = Color.LightGreen;
                    e.Style.TextColor = Color.Black;
                }
            }
            if (e.Column.MappingName == "Section_Rolled")
            {
                if (e.DisplayText.Contains("No Production"))
                {
                    //e.Style.BackColor = Color.Red;
                    e.Style.TextColor = Color.Red;
                }
                //else if (e.DisplayText == "RINL")
                //{
                //    e.Style.BackColor = Color.SkyBlue;
                //    e.Style.TextColor = Color.Black;
                //}
                //else if (e.DisplayText == "OWN")
                //{
                //    e.Style.BackColor = Color.LightGreen;
                //    e.Style.TextColor = Color.Black;
                //}
            }
        }
    }
}
