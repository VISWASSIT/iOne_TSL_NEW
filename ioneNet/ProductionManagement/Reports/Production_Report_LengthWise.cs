using Ione_DAL;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.WinForms.DataGridConverter;
using System.Diagnostics;

namespace ioneNet.ProductionManagement.Reports
{
    public partial class Production_Report_LengthWise : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public Production_Report_LengthWise()
        {
            InitializeComponent();
        }
        
        private void Production_Report_LengthWise_Load(object sender, EventArgs e)
        {

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                this.sfDataGrid1.TableSummaryRows.Clear();

                var d = (from data in db.SP_Production_Report_Section_LengthWise(logIn.company, dtpFrmDate.Value, dtpToDate.Value) select data).ToList();
                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                               

                this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                this.sfDataGrid1.Columns["Section_Rolled"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["Section_Rolled"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Section_Rolled"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["Section_Rolled"].FilterRowCondition = FilterRowCondition.Contains;

                this.sfDataGrid1.Columns["Material_Grade"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["Material_Grade"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Material_Grade"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["Material_Grade"].FilterRowCondition = FilterRowCondition.Contains;
                }
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
            workBook.Worksheets[0].Range["A2"].Value = "Production Report - Length Wise";
            workBook.Worksheets[0].Range["D2"].Value = "Period :" + dt1 + "-" + dt2;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\Production_Report - Length Wise.xlsx");
            string doc = Fname + "\\Production_Report - Length Wise.xlsx";
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
