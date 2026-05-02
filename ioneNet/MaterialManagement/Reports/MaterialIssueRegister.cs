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
   
    public partial class MaterialIssueRegister : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public MaterialIssueRegister()
        {
            InitializeComponent();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            sfDataGrid1.DataSource = null;
            if (checkBox1.Checked)
            {
                var d = (from data in db.MaterialIssues_ARN_WISE(logIn.company, dtpFrmDate.Value, dtpToDate.Value, logIn.BU_ID) select data).ToList();
                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;

                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;                    
                    this.sfDataGrid1.Columns["Slip_NO"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Slip_NO"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Prod_Code"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Prod_Code"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Prod_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Prod_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["ARN_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["ARN_No"].ShowFilterRowOptions = false;

                    this.sfDataGrid1.Columns["Prod_Name"].ImmediateUpdateColumnFilter = true;
                    //this.sfDataGrid1.Columns["Supplier_InvNo"].ShowFilterRowOptions = false;


                    this.sfDataGrid1.TableSummaryRows.Clear();
                    GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                    tableSummaryRow1.Name = "TableSummary";
                    tableSummaryRow1.ShowSummaryInRow = false;
                    tableSummaryRow1.Position = VerticalPosition.Bottom;

                    GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                    summaryColumn1.Name = "Total Qty";
                    summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn1.Format = "{Sum}";
                    summaryColumn1.MappingName = "Issued_Qty";
                    tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

                    GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                    summaryColumn2.Name = "Total ARN_Qty";
                    summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn2.Format = "{Sum}";
                    summaryColumn2.MappingName = "ARN_Qty";

                    tableSummaryRow1.SummaryColumns.Add(summaryColumn2);

                    this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);
                }
            }
            else
            {
                var d = (from data in db.MaterialIssueRegister(logIn.company, dtpFrmDate.Value, dtpToDate.Value, logIn.BU_ID) select data).ToList();
                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;

                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["Issued_To"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Issued_To"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Issued_To"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Slip_NO"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Slip_NO"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Ref_Doc_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Ref_Doc_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Prod_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Prod_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Prod_Name"].ImmediateUpdateColumnFilter = true;
                    //this.sfDataGrid1.Columns["Supplier_InvNo"].ShowFilterRowOptions = false;


                    this.sfDataGrid1.TableSummaryRows.Clear();
                    GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                    tableSummaryRow1.Name = "TableSummary";
                    tableSummaryRow1.ShowSummaryInRow = false;
                    tableSummaryRow1.Position = VerticalPosition.Bottom;

                    GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                    summaryColumn1.Name = "Total Qty";
                    summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn1.Format = "{Sum}";
                    summaryColumn1.MappingName = "Issued_Qty";
                    tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

                    GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                    summaryColumn2.Name = "Total Value";
                    summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn2.Format = "{Sum}";
                    summaryColumn2.MappingName = "Issue_Value";

                    tableSummaryRow1.SummaryColumns.Add(summaryColumn2);

                    this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MaterialIssueRegister_Load(object sender, EventArgs e)
        {
            if(logIn.company ==1044)
            {
                checkBox1.Visible = true;
            }
            else
            {
                checkBox1.Visible = false;
            }
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
            workBook.Worksheets[0].Range["A2"].Value = "Material Issue Register";
            workBook.Worksheets[0].Range["D2"].Value = "Period :" + dt1 + "-" + dt2;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\Issues_Report.xlsx");
            string doc = Fname + "\\Issues_Report.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }
    }
}
