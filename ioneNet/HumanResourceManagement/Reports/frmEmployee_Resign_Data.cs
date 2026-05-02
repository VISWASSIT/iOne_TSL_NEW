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
using System.IO;
using Syncfusion.WinForms.DataGrid.Interactivity;

namespace ioneNet.HumanResourceManagement.Reports
{
    public partial class frmEmployee_Resign_Data : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public frmEmployee_Resign_Data()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
               
                    SqlCommand cmd2 = new SqlCommand("SP_HR_Get_Employee_ResignData ", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@compname", logIn.company);                   
                    cmd2.Parameters.AddWithValue("@buid",logIn.BU_ID);
                    cmd2.Parameters.AddWithValue("@frmDate", dateTimePicker1.Value);
                    cmd2.Parameters.AddWithValue("@todate", dateTimePicker2.Value);

                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    //DataSet ds2 = new DataSet();
                    DataTable ds2 = new DataTable();
                    // da2.Fill(ds2, "x");
                    da2.Fill(ds2);
                    sfDataGrid1.DataSource = ds2;
                this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                this.sfDataGrid1.Columns["Emp_Code"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["Emp_Code"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Emp_Code"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["Emp_Code"].FilterRowCondition = FilterRowCondition.Contains;

                this.sfDataGrid1.Columns["Emp_Name"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["Emp_Name"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Emp_Name"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["Emp_Name"].FilterRowCondition = FilterRowCondition.Contains;
                               

                this.sfDataGrid1.TableSummaryRows.Clear();
                GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                tableSummaryRow1.Name = "TableSummary";
                tableSummaryRow1.ShowSummaryInRow = false;
                tableSummaryRow1.Position = VerticalPosition.Bottom;

                GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                summaryColumn1.Name = "Total Employees";
                summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn1.Format = "Total Employees: {Count}";
                summaryColumn1.MappingName = "Emp_Name";

                tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

                this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);



                //this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnExcell_Click(object sender, EventArgs e)
        {
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 5;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.BU_Name;
            workBook.Worksheets[0].Range["A2"].Value = "Resigned Employees Report";
            workBook.Worksheets[0].Range["D2"].Value = "for the Period :" + dateTimePicker1.Text + '-' + dateTimePicker2.Text;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\Resigned_Employee_Data.xlsx");
            string doc = Fname + "\\Resigned_Employee_Data.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void frmEmployee_Resign_Data_Load(object sender, EventArgs e)
        {

        }
    }
}
