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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Syncfusion.WinForms.DataGrid.Events;

namespace ioneNet.MaterialManagement.Reports
{
   
    public partial class GoodsInwardReport_RM : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public GoodsInwardReport_RM()
        {
            InitializeComponent();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            sfDataGrid1.DataSource = null;
            var d = (from data in db.GoodsInwardRegister_Raw_Material(logIn.company,dtpFrmDate.Value,dtpToDate.Value,logIn.BU_ID) select data).ToList();
            if (d.Count > 0)
            {
                //dgProductsList.DataSource = d;
                sfDataGrid1.DataSource = d;
                this.sfDataGrid1.QueryCellStyle += SfDataGrid1_QueryCellStyle;
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


            this.sfDataGrid1.Columns["TCNo"].FilterRowEditorType = "TextBox";
            this.sfDataGrid1.Columns["TCNo"].ShowFilterRowOptions = false;
            this.sfDataGrid1.Columns["TCNo"].ImmediateUpdateColumnFilter = true;

            this.sfDataGrid1.Columns["Internal_LOT_No"].FilterRowEditorType = "TextBox";
            this.sfDataGrid1.Columns["Internal_LOT_No"].ShowFilterRowOptions = false;
            this.sfDataGrid1.Columns["Internal_LOT_No"].ImmediateUpdateColumnFilter = true;

            this.sfDataGrid1.Columns["Supplier_InvNo"].ShowFilterRowOptions = false;


            this.sfDataGrid1.TableSummaryRows.Clear();
            GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
            tableSummaryRow1.Name = "TableSummary";
            tableSummaryRow1.ShowSummaryInRow = false;
            tableSummaryRow1.Position = VerticalPosition.Bottom;           
            
            GridSummaryColumn summaryColumn8 = new GridSummaryColumn();
            summaryColumn8.Name = "Total  Qty";
            summaryColumn8.SummaryType = SummaryType.DoubleAggregate;
            summaryColumn8.Format = "{Sum}";
            summaryColumn8.MappingName = "AcceptedQty";

            tableSummaryRow1.SummaryColumns.Add(summaryColumn8);

            this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);

                GridSummaryRow groupSummaryRow1 = new GridSummaryRow();
                groupSummaryRow1.Name = "GroupSummary";
                groupSummaryRow1.ShowSummaryInRow = false;

                // Creates the GridSummaryColumn.
                GridSummaryColumn GsummaryColumn1 = new GridSummaryColumn();
                GsummaryColumn1.Name = "Total_Qty";
                GsummaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumn1.Format = "Total_Qty : {Sum:c}";
                GsummaryColumn1.MappingName = "AcceptedQty";

                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn1);              



                // Adds the summary row in the GroupSummaryRows collection.
                this.sfDataGrid1.GroupSummaryRows.Add(groupSummaryRow1);

            }
        }
        private void SfDataGrid1_QueryCellStyle(object sender, QueryCellStyleEventArgs e)
        {


            if (e.Column.MappingName == "TCNo")
            {
                if (e.DisplayText == "To Receive")
                {
                    
                    e.Style.BackColor = Color.White;
                    e.Style.TextColor = Color.Red;
                }
                else 
                {
                    
                    e.Style.BackColor = Color.LightGreen;
                    e.Style.TextColor = Color.DarkSlateBlue;

                }
                
            }
            if (e.Column.MappingName == "Insp_STatus")
            {
                if (e.DisplayText == "Awaiting For Insp")
                {

                    e.Style.BackColor = Color.Orange;
                    e.Style.TextColor = Color.Black;
                }
                else
                {

                    e.Style.BackColor = Color.LightGreen;
                    e.Style.TextColor = Color.DarkSlateBlue;

                }

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
            workBook.Worksheets[0].Range["A2"].Value = "Goods Inward Register - Import";
            workBook.Worksheets[0].Range["D2"].Value = "Period :" + dt1 + "-" + dt2;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\GRN_Import_Report.xlsx");
            string doc = Fname + "\\GRN_Import_Report.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void GoodsInwardReport_Import_Load(object sender, EventArgs e)
        {

        }
    }
}
