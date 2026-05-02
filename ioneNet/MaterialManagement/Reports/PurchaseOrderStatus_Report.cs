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
using Syncfusion.WinForms.DataGrid.Interactivity;
using Syncfusion.Windows.Forms.CellGrid.ScrollAxis;
namespace ioneNet.MaterialManagement.Reports
{
   
    public partial class PurchaseOrderStatus_Report : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();
        public PurchaseOrderStatus_Report()
        {
            InitializeComponent();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            this.sfDataGrid1.TableSummaryRows.Clear();
            if (checkBox1.Checked)
            {
                var d = (from data in db.PurchaseOrderRegister(logIn.company, logIn.BU_ID, dpFromDate.Value, dpTodate.Value)
                         select data).Where(c => c.PO_Group == "Raw Material").ToList();
                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;


                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;


                    this.sfDataGrid1.Columns["Prod_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Prod_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Prod_Name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Prod_Name"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["prod_spec"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["prod_spec"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["prod_spec"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["prod_spec"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Supplier_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Supplier_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Supplier_Name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Supplier_Name"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["PO_NO"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["PO_NO"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["PO_NO"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["PO_NO"].FilterRowCondition = FilterRowCondition.Contains;


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
                    this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);



                }
            }
            else
            {
                var d = (from data in db.PurchaseOrderRegister(logIn.company, logIn.BU_ID, dpFromDate.Value, dpTodate.Value)
                         select data).Where(c => c.PO_Group == "Store").ToList();
                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                }
            }

            this.sfDataGrid1.QueryCellStyle += sfDataGrid_QueryCellStyle;
            string cellValue;
            for (int i = 2; i < sfDataGrid1.RowCount-1; i++)
            {
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["Status"].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                if (cellVaue.ToString() == "Approved")
                {
                    SetCellBackgroundColor(new RowColumnIndex(i, 3), Color.Green);
                }
                if (cellVaue.ToString() == "Part Material Received")
                {
                    SetCellBackgroundColor(new RowColumnIndex(i, 3), Color.LightSkyBlue);
                }
                if (cellVaue.ToString() == "Closed")
                {
                    SetCellBackgroundColor(new RowColumnIndex(i, 3), Color.SaddleBrown);
                }
                if (cellVaue.ToString() == "Pre-Closed")
                {
                    SetCellBackgroundColor(new RowColumnIndex(i, 3), Color.Red);
                }
                if (cellVaue.ToString() == "Created")
                {
                    SetCellBackgroundColor(new RowColumnIndex(i, 3), Color.White);
                }
            }

        }
        void sfDataGrid_QueryCellStyle(object sender, Syncfusion.WinForms.DataGrid.Events.QueryCellStyleEventArgs e)
        {
            var rowColumnIndex = new RowColumnIndex(e.RowIndex, e.ColumnIndex);
            if (colorDict.ContainsKey(rowColumnIndex))
                e.Style.BackColor = colorDict[rowColumnIndex];
        }
        void SetCellBackgroundColor(RowColumnIndex rowColumnIndex, Color color)
        {
            if (!colorDict.ContainsKey(rowColumnIndex))
                colorDict.Add(rowColumnIndex, color);
            else
                colorDict[rowColumnIndex] = color;
            sfDataGrid1.TableControl.Invalidate(this.sfDataGrid1.TableControl.GetCellRectangle(rowColumnIndex.RowIndex, rowColumnIndex.ColumnIndex, false));
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnExcell_Click(object sender, EventArgs e)
        {
            DateTime dt = dpFromDate.Value;
            string dt1 = dt.ToString("dd/MM/yyyy");

            
           
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 5;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            workBook.Worksheets[0].Range["A2"].Value = "Purchase Order Status Report";
            workBook.Worksheets[0].Range["D2"].Value = "As on :" + dt1;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\PO_Status_Report.xlsx");
            string doc = Fname + "\\PO_Status_Report.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void PurchaseOrderStatus_Report_Load(object sender, EventArgs e)
        {

        }
    }
}
