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
   
    public partial class MaterialIndentStatusReport : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public MaterialIndentStatusReport()
        {
            InitializeComponent();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            sfDataGrid1.DataSource = null;
            var d = (from data in db.SP_Indent_StatusReport(logIn.company,dtpFrmDate.Value,dtpToDate.Value,logIn.BU_ID) select data).ToList();
            if (d.Count > 0)
            {
                //dgProductsList.DataSource = d;
                sfDataGrid1.DataSource = d;
                this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                this.sfDataGrid1.Columns["Indent_NO"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["Indent_NO"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Indent_NO"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["Prod_Code"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["Prod_Code"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Prod_Code"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["Product_Description"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["Product_Description"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Product_Description"].ImmediateUpdateColumnFilter = true;
            }
            
            //this.sfDataGrid1.Columns["Supplier_InvNo"].ShowFilterRowOptions = false;

           
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void MaterialIssueRegister_Load(object sender, EventArgs e)
        {

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
            workBook.Worksheets[0].Range["A2"].Value = "Material Indent Report";
            workBook.Worksheets[0].Range["D2"].Value = "Period :" + dt1 + "-" + dt2;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\Indent_Report.xlsx");
            string doc = Fname + "\\Indent_Report.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }
    }
}
