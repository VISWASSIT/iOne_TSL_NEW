using Ione_DAL;
using Syncfusion.WinForms.DataGridConverter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ioneNet.HumanResourceManagement.Reports
{
    public partial class frmOTReport_Daywise : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmOTReport_Daywise()
        {
            InitializeComponent();
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
                SqlCommand cmd2 = new SqlCommand("SP_HR_OT_DayWiseReport", con);
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
                sfDataGrid1.Style.CellStyle.BackColor = Color.White;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
            workBook.Worksheets[0].Range["A2"].Value = "OT Day Wise Repoprt";
            workBook.Worksheets[0].Range["D2"].Value = "Period :" + dt1 + "-" + dt2;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\OT_DayWise_Report.xlsx");
            string doc = Fname + "\\OT_DayWise_Report.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
