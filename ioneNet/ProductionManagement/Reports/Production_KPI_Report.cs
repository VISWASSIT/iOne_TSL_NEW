using Ione_DAL;
using Syncfusion.Data;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Events;
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
using Syncfusion.WinForms.DataGrid.Interactivity;
using System.Diagnostics;
using Syncfusion.WinForms.DataGridConverter;

namespace ioneNet.ProductionManagement.Reports
{
    public partial class Production_KPI_Report : Form
    {
        public static string InvoiceNoList, Order_NoList, SO_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;

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
            workBook.Worksheets[0].Range["A2"].Value = "Production Report";
            workBook.Worksheets[0].Range["D2"].Value = "Period :" + dt1 + "-" + dt2;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\Production_Report.xlsx");
            string doc = Fname + "\\Production_Report.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public Production_KPI_Report()
        {
            InitializeComponent();
        }

        private void Production_Report_Rolling_Load(object sender, EventArgs e)
        {

        }

        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                sfDataGrid1.DataSource = null;
                DateTime dt = dtpFrmDate.Value;
                string dt1 = dt.ToString("yyyy/MM/dd");

                DateTime dtt = dtpToDate.Value;
                string dt2 = dtt.ToString("yyyy/MM/dd");


                SqlCommand cmd2 = new SqlCommand("SP_Production_KPI_Report", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);
                cmd2.Parameters.AddWithValue("@Fromdate", Convert.ToDateTime(dt1));
                cmd2.Parameters.AddWithValue("@Todate", Convert.ToDateTime(dt2));
                //cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataSet ds2 = new DataSet();
                DataTable ds2 = new DataTable();
                // da2.Fill(ds2, "x");
                da2.Fill(ds2);

                //    var d = (from data in db.ShowStockReport_New(logIn.company, dt1, dt2, logIn.BU_ID) select data).ToList();
                if (ds2.Rows.Count > 0)
                {
                    sfDataGrid1.DataSource = ds2;

                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;

                    this.sfDataGrid1.Columns["Section_Rolled"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Section_Rolled"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Section_Rolled"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Section_Rolled"].FilterRowCondition = FilterRowCondition.Contains;

                }
                    //var d = (from data in db.SP_Production_KPI_Report(dtpFrmDate.Value, dtpToDate.Value, logIn.company) select data).ToList();
                    //if (d.Count > 0)
                    //{
                    //    //dgProductsList.DataSource = d;
                    //    sfDataGrid1.DataSource = d;
                    //}
                    //this.sfDataGrid1.QueryCellStyle += SfDataGrid1_QueryCellStyle;

                    //this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    //this.sfDataGrid1.Columns["Section_Rolled"].FilterRowEditorType = "TextBox";
                    //this.sfDataGrid1.Columns["Section_Rolled"].ShowFilterRowOptions = false;
                    //this.sfDataGrid1.Columns["Section_Rolled"].ImmediateUpdateColumnFilter = true;
                    //this.sfDataGrid1.Columns["Section_Rolled"].FilterRowCondition = FilterRowCondition.Contains;
                   

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void SfDataGrid1_QueryCellStyle(object sender, QueryCellStyleEventArgs e)
        {
            if (e.Column.MappingName == "EC_Per")
            {
                string V = e.DisplayText;
                if (V != "")
                {

                    if (Convert.ToDecimal(V) <= 5)
                    {
                        //e.Style.BackColor = Color.Gray;
                        e.Style.TextColor = Color.Green;
                    }
                    else
                    {
                        //e.Style.BackColor = Color.Gray;
                        e.Style.TextColor = Color.Red;
                    }
                }
            }

            if (e.Column.MappingName == "Rolled_For")
            {
                if (e.DisplayText == "TATA")
                {
                    e.Style.BackColor = Color.Orange;
                    e.Style.TextColor = Color.Black;
                }
                else if (e.DisplayText == "RINL")
                {
                    e.Style.BackColor = Color.SkyBlue;
                    e.Style.TextColor = Color.Black;
                }
                else if (e.DisplayText == "Own")
                {
                    e.Style.BackColor = Color.LightGreen;
                    e.Style.TextColor = Color.Black;
                }
            }
            if (e.Column.MappingName == "Section_Rolled")
            {
                if (e.DisplayText.Contains("No Production"))
                {
                    //e.Style.BackColor = Color.Red;
                    e.Style.TextColor = Color.Red;
                }
                //else if (e.DisplayText == "RINL")
                //{
                //    e.Style.BackColor = Color.SkyBlue;
                //    e.Style.TextColor = Color.Black;
                //}
                //else if (e.DisplayText == "OWN")
                //{
                //    e.Style.BackColor = Color.LightGreen;
                //    e.Style.TextColor = Color.Black;
                //}
            }
        }
    }
}
