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
using Ione_DAL;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.Data;
using Syncfusion.WinForms.DataGridConverter;
using System.Diagnostics;

namespace ioneNet.HumanResourceManagement.Reports
{
    public partial class frmMonthAttendanceReport : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public frmMonthAttendanceReport()
        {
            InitializeComponent();
        }

        private void frmMonthAttendanceReport_Load(object sender, EventArgs e)
        {

        }

        private void btnGetAccounts_Click(object sender, EventArgs e)
        {
            try
            {
               
                
                SqlCommand cmd2 = new SqlCommand("SP_HR_Get_Leave_LOP_Report", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);                
                cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
                cmd2.Parameters.AddWithValue("@month", cmbMonth.Text);
                cmd2.Parameters.AddWithValue("@year", cmbYear.Text);

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
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 5;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.BU_Name;
            workBook.Worksheets[0].Range["A2"].Value = "LEAVE / LOP  Report";
            workBook.Worksheets[0].Range["D2"].Value = "Month :" + cmbMonth.Text +  "-" + cmbYear.Text;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\Leave_LOP_Report.xlsx");
            string doc = Fname + "\\Leave_LOP_Report.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
