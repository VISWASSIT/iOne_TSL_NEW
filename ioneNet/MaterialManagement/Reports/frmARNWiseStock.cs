using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid;
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
using Ione_DAL;
using System.Configuration;
using System.Diagnostics;
using Syncfusion.Data;
using Syncfusion.WinForms.DataGridConverter;


namespace ioneNet.MaterialManagement.Reports
{
    public partial class frmARNWiseStock : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public frmARNWiseStock()
        {
            InitializeComponent();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            var dm1 = (from s in db.Bloom_Roll_Wise_Stocks
                       join p in db.Products  on s.Prod_ID equals p.prod_ID
                       where s.QtyStock > 0 && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID
                       select new
                       {
                           p.Prod_Alternative_Code,
                           p.Prod_Name,
                           s.Ref_Doc,
                           ARN_No = s.RollNo,
                           Qty_Stock = s.RollWt,
                           Qty_Issued = s.QtyIssued,
                           Qty_Kgs = s.QtyStock,
                           s.Stock_Value,
                           s.Re_Test_Date
                       }); ;
            SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            DataTable dtr = new DataTable();
            da2.Fill(dtr);
            if (dtr.Rows.Count >= 0)
            {
                sfDataGrid1.DataSource = dtr;
                this.sfDataGrid1.Columns["ARN_No"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["ARN_No"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["ARN_No"].ImmediateUpdateColumnFilter = true;

            }

        }

        private void btnExcell_Click(object sender, EventArgs e)
        {
            DateTime dt = DateTime.Now;
            string dt1 = dt.ToString("dd/MM/yyyy");

            
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 5;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            workBook.Worksheets[0].Range["A2"].Value = "ARN Wise Stock Register";
            workBook.Worksheets[0].Range["D2"].Value = "As On :" + dt1; 
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\ARN_Stock_Report.xlsx");
            string doc = Fname + "\\ARN_Stock_Report.xlsx";
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
