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
namespace ioneNet.MaterialManagement.Reports
{
    public partial class RawMaterial_Requirement_Plan : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public RawMaterial_Requirement_Plan()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                this.sfDataGrid1.TableSummaryRows.Clear();
                string fdate = "2024-04-01";
                var d1 = (from data in db.SP_Get_RMRequirement(logIn.company, Convert.ToDateTime(fdate), dtpFrmDate.Value,  logIn.BU_ID) select data).ToList();
                if (d1.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d1;

                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    

                    this.sfDataGrid1.Columns["prod_name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["prod_name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["prod_name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["prod_name"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Item_Grade"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Item_Grade"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Item_Grade"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Item_Grade"].FilterRowCondition = FilterRowCondition.Contains;



                  

                    //this.sfDataGrid1.QueryCellStyle += SfDataGrid1_QueryCellStyle;
                    GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                    tableSummaryRow1.Name = "TableSummary";
                    tableSummaryRow1.ShowSummaryInRow = false;
                    tableSummaryRow1.Position = VerticalPosition.Bottom;

                    GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                    summaryColumn1.Name = "Total Req Qty";
                    summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn1.Format = "Total : {Sum}";
                    summaryColumn1.MappingName = "Qty_Required";

                    tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

                    GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                    summaryColumn2.Name = "Total Stock Qty";
                    summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn2.Format = "Total : {Sum}";
                    summaryColumn2.MappingName = "Stock_P";

                    tableSummaryRow1.SummaryColumns.Add(summaryColumn2);

                    GridSummaryColumn summaryColumn3 = new GridSummaryColumn();
                    summaryColumn3.Name = "Total PO Qty";
                    summaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn3.Format = "Total : {Sum}";
                    summaryColumn3.MappingName = "PO_Pending_Qty";

                    tableSummaryRow1.SummaryColumns.Add(summaryColumn3);


                    this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            DateTime dt = dtpFrmDate.Value;
            string dt1 = dt.ToString("dd/MM/yyyy");

            
            
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 3;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
           
                workBook.Worksheets[0].Range["A2"].Value = "Raw Material Requirement Plan";
           
            workBook.Worksheets[0].Range["D2"].Value = "As on :" + dt1;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string doc = "";
            
            workBook.SaveAs(Fname + "\\RM_Requirement.xlsx");
            doc = Fname + "\\RM_Requirement.xlsx";
            

            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sfDataGrid1_CurrentCellActivated(object sender, CurrentCellActivatedEventArgs e)
        {
            int i = sfDataGrid1.CurrentCell.RowIndex;
            //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
            var c = sfDataGrid1.CurrentCell.Column.MappingName;
            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
            var mappingName = sfDataGrid1.Columns["RM_ID"].MappingName;
            var mappingName1 = sfDataGrid1.Columns["Item_Grade"].MappingName;
            //var currentAmendValue = (rowData.GetType().GetProperty("Po_Amend_No").GetValue(rowData, null).ToString());
            //var mappingName1 = sfDataGrid1.Columns["Status"].MappingName;
            //var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            //var cellvalue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
            //if (c == "Item_Name")
            //{
            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());

            //CustomerPo_No = "";
            //Customer_Name = "";
            var d = (from data in db.SP_Get_PendingPOs_Item_GradeWise(logIn.company, Convert.ToInt32(cellVaue), cellVaue1, logIn.BU_ID) select data).ToList();
            sfDataGrid3.DataSource = null;
            this.sfDataGrid3.TableSummaryRows.Clear();

            if (d.Count > 0)
            {
                //dgProductsList.DataSource = d;
                sfDataGrid3.DataSource = d;

                GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                tableSummaryRow1.Name = "TableSummary";
                tableSummaryRow1.ShowSummaryInRow = false;
                tableSummaryRow1.Position = VerticalPosition.Bottom;

                GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                summaryColumn1.Name = "Total PO Bal Qty";
                summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn1.Format = "Total : {Sum}";
                summaryColumn1.MappingName = "Bal_Qty";

                tableSummaryRow1.SummaryColumns.Add(summaryColumn1);
                this.sfDataGrid3.TableSummaryRows.Add(tableSummaryRow1);
                //}
            }


            SqlCommand cmd2 = new SqlCommand("SP_Get_OrderData_For_RMRequirement", con);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@compname", logIn.company);
            cmd2.Parameters.AddWithValue("@RM_ID", Convert.ToInt32(cellVaue));
            cmd2.Parameters.AddWithValue("@RM_Grade", cellVaue1);
            cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
            //cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //DataSet ds2 = new DataSet();
            DataTable ds2 = new DataTable();
            // da2.Fill(ds2, "x");
            da2.Fill(ds2);
            this.sfDataGrid2.TableSummaryRows.Clear();
            //    var d = (from data in db.ShowStockReport_New(logIn.company, dt1, dt2, logIn.BU_ID) select data).ToList();
            if (ds2.Rows.Count > 0)
            {
                sfDataGrid2.DataSource = ds2;

                GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                tableSummaryRow1.Name = "TableSummary";
                tableSummaryRow1.ShowSummaryInRow = false;
                tableSummaryRow1.Position = VerticalPosition.Bottom;

                GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                summaryColumn1.Name = "Total Required Qty";
                summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn1.Format = "Total : {Sum}";
                summaryColumn1.MappingName = "Qty_Required";

                tableSummaryRow1.SummaryColumns.Add(summaryColumn1);
                this.sfDataGrid2.TableSummaryRows.Add(tableSummaryRow1);
            }
        }

        private void sfButton1_Click(object sender, EventArgs e)
        {
            DateTime dt = dtpFrmDate.Value;
            string dt1 = dt.ToString("dd/MM/yyyy");



            var options = new ExcelExportingOptions();
            options.StartRowIndex = 3;
            var excelEngine = sfDataGrid2.ExportToExcel(sfDataGrid2.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;

            workBook.Worksheets[0].Range["A2"].Value = "Pending Orders - For RM";

            workBook.Worksheets[0].Range["D2"].Value = "As on :" + dt1;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string doc = "";

            workBook.SaveAs(Fname + "\\Pending_Orders_For_RM.xlsx");
            doc = Fname + "\\Pending_Orders_For_RM.xlsx";


            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void sfButton2_Click(object sender, EventArgs e)
        {
            DateTime dt = dtpFrmDate.Value;
            string dt1 = dt.ToString("dd/MM/yyyy");



            var options = new ExcelExportingOptions();
            options.StartRowIndex = 3;
            var excelEngine = sfDataGrid3.ExportToExcel(sfDataGrid3.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;

            workBook.Worksheets[0].Range["A2"].Value = "Pending Purchase Orders - RM";

            workBook.Worksheets[0].Range["D2"].Value = "As on :" + dt1;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string doc = "";

            workBook.SaveAs(Fname + "\\Pending_PO_RM.xlsx");
            doc = Fname + "\\Pending_PO_RM.xlsx";


            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }
    }
}
