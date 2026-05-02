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
using Syncfusion.WinForms.DataGridConverter;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Configuration;
using Syncfusion.Data;
using Ione_DAL;
using Syncfusion.WinForms.DataGrid.Interactivity;

namespace ioneNet.OrderManagement.Reports
{
    public partial class EnquiryRegister : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public EnquiryRegister()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                this.sfDataGrid1.TableSummaryRows.Clear();

                var d1 = (from data in db.Enquiry_Status_Report(logIn.company, dtpFrmDate.Value, dtpToDate.Value, logIn.BU_ID) select data).ToList();
                if (d1.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d1;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["Enq_NO"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Enq_NO"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Enq_NO"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Enq_NO"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Customer_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Customer_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Customer_Name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Customer_Name"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["prod_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["prod_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["prod_Name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["prod_Name"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Quot_NO"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Quot_NO"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Quot_NO"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Quot_NO"].FilterRowCondition = FilterRowCondition.Equals;

                    this.sfDataGrid1.Columns["Executive"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Executive"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Executive"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Executive"].FilterRowCondition = FilterRowCondition.Contains;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
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
            workBook.Worksheets[0].Range["A2"].Value = "Enquiry Report";
            workBook.Worksheets[0].Range["D2"].Value = "Period :" + dt1 + "-" + dt2;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\Enquiry_Report.xlsx");
            string doc = Fname + "\\Enquiry_Report.xlsx";
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
