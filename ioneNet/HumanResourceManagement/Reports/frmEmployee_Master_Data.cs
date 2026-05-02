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
    public partial class frmEmployee_Master_Data : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static int DivID;
        public frmEmployee_Master_Data()
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
                DivID = logIn.BU_ID;
                BindEmployeeslist();
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void BindEmployeeslist()
        {
            try
            {
                //sfDataGrid1.DataSource=Refresh();


                SqlCommand cmd2 = new SqlCommand("SP_HR_Get_Employee_MasterData ", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);
                cmd2.Parameters.AddWithValue("@buid", DivID);
                if (checkBox1.Checked)
                {
                    cmd2.Parameters.AddWithValue("@para", 1);
                }
                else
                {
                    cmd2.Parameters.AddWithValue("@para", 0);
                }

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

                this.sfDataGrid1.Columns["AadharNo"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["AadharNo"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["AadharNo"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["AadharNo"].FilterRowCondition = FilterRowCondition.Contains;

                this.sfDataGrid1.TableSummaryRows.Clear();
                GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                tableSummaryRow1.Name = "TableSummary";
                tableSummaryRow1.ShowSummaryInRow = false;
                tableSummaryRow1.Position = VerticalPosition.Bottom;

                GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                summaryColumn1.Name = "Total Employees";
                summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn1.Format = "Total Employees: {Count}";
                summaryColumn1.MappingName = "EMP_ID";

                tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

                this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);


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
            workBook.Worksheets[0].Range["A2"].Value = "Employee Master Data";
            //workBook.Worksheets[0].Range["D2"].Value = "for the Month of :" + cmbMonth.Text + '-' + cmbYear.Text;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\Employee_Data.xlsx");
            string doc = Fname + "\\Employee_Data.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void frmEmployee_Master_Data_Load(object sender, EventArgs e)
        {
            if(HRMDashboard.Div_ID!= null || HRMDashboard.Div_ID==0)
            {
                DivID = HRMDashboard.Div_ID;
                BindEmployeeslist();
            }
        }
    }
}
