using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Syncfusion.WinForms.GridCommon.ScrollAxis;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.Data;
using System.Configuration;
using System.Data.SqlClient;
using Excel = Microsoft.Office.Interop.Excel;
using System.Diagnostics;
using System.IO;
using Syncfusion.Windows.Forms.Grid;
using Syncfusion.WinForms.DataGridConverter;
using System.Data.OleDb;
using System.Data.Common;
using Newtonsoft.Json;
using System.Net;
//using Syncfusion.XlsIO;
using Ione_DAL;
namespace ioneNet.FinanceManagement.Reports
{
    public partial class frmGSTR_SaleData : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmGSTR_SaleData()
        {
            InitializeComponent();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                binddata_Tally();
            }
            else
            {
                binddata();
            }
        }
        public void binddata()
        {
            try
            {
                DateTime dt = dpFromDate.Value;
                string dt1 = dt.ToString("yyyy/MM/dd");

                DateTime dtt = dpTodate.Value;
                string dt2 = dtt.ToString("yyyy/MM/dd");

                //var data = db.Sp_ProductionTunnageReport(AppCode.GlobalAccess.companyName, Convert.ToDateTime(dt1), Convert.ToDateTime(dt2)).ToString();


                SqlCommand cmd2 = new SqlCommand("GSTRSaleData", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);
                cmd2.Parameters.AddWithValue("@Fromdate", Convert.ToDateTime(dt1));
                cmd2.Parameters.AddWithValue("@Todate", Convert.ToDateTime(dt2));
                cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataSet ds2 = new DataSet();
                DataTable ds2 = new DataTable();
                // da2.Fill(ds2, "x");
                da2.Fill(ds2);
                sfDataGrid1.DataSource = ds2;

                //this.sfDataGrid1.TableSummaryRows.Clear();
                this.sfDataGrid1.GroupSummaryRows.Clear();
                GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                tableSummaryRow1.Name = "TableSummary";
                tableSummaryRow1.ShowSummaryInRow = false;
                tableSummaryRow1.Position = VerticalPosition.Bottom;

                GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                summaryColumn1.Name = "Total_Taxable_Value";
                summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn1.Format = "{Sum}";
                summaryColumn1.MappingName = "Taxable_Value";

                tableSummaryRow1.SummaryColumns.Add(summaryColumn1);
                GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                summaryColumn2.Name = "Total_CGST_Amnt";
                summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn2.Format = "{Sum}";
                summaryColumn2.MappingName = "CGST_Amnt";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn2);

                GridSummaryColumn summaryColumn3 = new GridSummaryColumn();
                summaryColumn3.Name = "Total_SGST_Amnt";
                summaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn3.Format = "{Sum}";
                summaryColumn3.MappingName = "SGST_Amnt";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn3);

                GridSummaryColumn summaryColumn4 = new GridSummaryColumn();
                summaryColumn4.Name = "Total_IGST_Amnt";
                summaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn4.Format = "{Sum}";
                summaryColumn4.MappingName = "IGST_Amnt";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn4);

                this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);

                // Creates the GridSummaryRow.
                GridSummaryRow groupSummaryRow1 = new GridSummaryRow();
                groupSummaryRow1.Name = "GroupSummary";
                groupSummaryRow1.ShowSummaryInRow = false;
                GridSummaryColumn GsummaryColumn1 = new GridSummaryColumn();
                GsummaryColumn1.Name = "Total_Taxable_Value";
                GsummaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumn1.Format = "{Sum}";
                GsummaryColumn1.MappingName = "Taxable_Value";

                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn1);
                // Adds the summary row in the GroupSummaryRows collection.

                GridSummaryColumn GsummaryColumn2 = new GridSummaryColumn();
                GsummaryColumn2.Name = "Total_CGST_Amnt";
                GsummaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumn2.Format = "{Sum}";
                GsummaryColumn2.MappingName = "CGST_Amnt";
                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn2);

                GridSummaryColumn GsummaryColumn3 = new GridSummaryColumn();
                GsummaryColumn3.Name = "Total_SGST_Amnt";
                GsummaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumn3.Format = "{Sum}";
                GsummaryColumn3.MappingName = "SGST_Amnt";
                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn3);

                GridSummaryColumn GsummaryColumn4 = new GridSummaryColumn();
                GsummaryColumn4.Name = "Total_IGST_Amnt";
                GsummaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumn4.Format = "{Sum}";
                GsummaryColumn4.MappingName = "IGST_Amnt";
                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn4);

                // Adds the summary row in the GroupSummaryRows collection.
                this.sfDataGrid1.GroupSummaryRows.Add(groupSummaryRow1);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void binddata_Tally()
        {
            try
            {
                DateTime dt = dpFromDate.Value;
                string dt1 = dt.ToString("yyyy/MM/dd");

                DateTime dtt = dpTodate.Value;
                string dt2 = dtt.ToString("yyyy/MM/dd");

                //var data = db.Sp_ProductionTunnageReport(AppCode.GlobalAccess.companyName, Convert.ToDateTime(dt1), Convert.ToDateTime(dt2)).ToString();


                SqlCommand cmd2 = new SqlCommand("Sp_Report_Invoice_Tally", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);
                cmd2.Parameters.AddWithValue("@Fromdate", Convert.ToDateTime(dt1));
                cmd2.Parameters.AddWithValue("@Todate", Convert.ToDateTime(dt2));
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataSet ds2 = new DataSet();
                DataTable ds2 = new DataTable();
                // da2.Fill(ds2, "x");
                da2.Fill(ds2);
                sfDataGrid1.DataSource = ds2;

                //this.sfDataGrid1.TableSummaryRows.Clear();
                this.sfDataGrid1.GroupSummaryRows.Clear();
                GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                tableSummaryRow1.Name = "TableSummary";
                tableSummaryRow1.ShowSummaryInRow = false;
                tableSummaryRow1.Position = VerticalPosition.Bottom;

                GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                summaryColumn1.Name = "Total_Taxable_Value";
                summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn1.Format = "{Sum}";
                summaryColumn1.MappingName = "Taxable_Value";

                tableSummaryRow1.SummaryColumns.Add(summaryColumn1);
                GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                summaryColumn2.Name = "Total_CGST_Amnt";
                summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn2.Format = "{Sum}";
                summaryColumn2.MappingName = "CGST_Amnt";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn2);

                GridSummaryColumn summaryColumn3 = new GridSummaryColumn();
                summaryColumn3.Name = "Total_SGST_Amnt";
                summaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn3.Format = "{Sum}";
                summaryColumn3.MappingName = "SGST_Amnt";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn3);

                GridSummaryColumn summaryColumn4 = new GridSummaryColumn();
                summaryColumn4.Name = "Total_IGST_Amnt";
                summaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn4.Format = "{Sum}";
                summaryColumn4.MappingName = "IGST_Amnt";
                tableSummaryRow1.SummaryColumns.Add(summaryColumn4);

                this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);

                // Creates the GridSummaryRow.
                GridSummaryRow groupSummaryRow1 = new GridSummaryRow();
                groupSummaryRow1.Name = "GroupSummary";
                groupSummaryRow1.ShowSummaryInRow = false;
                GridSummaryColumn GsummaryColumn1 = new GridSummaryColumn();
                GsummaryColumn1.Name = "Total_Taxable_Value";
                GsummaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumn1.Format = "{Sum}";
                GsummaryColumn1.MappingName = "Taxable_Value";

                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn1);
                // Adds the summary row in the GroupSummaryRows collection.

                GridSummaryColumn GsummaryColumn2 = new GridSummaryColumn();
                GsummaryColumn2.Name = "Total_CGST_Amnt";
                GsummaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumn2.Format = "{Sum}";
                GsummaryColumn2.MappingName = "CGST_Amnt";
                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn2);

                GridSummaryColumn GsummaryColumn3 = new GridSummaryColumn();
                GsummaryColumn3.Name = "Total_SGST_Amnt";
                GsummaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumn3.Format = "{Sum}";
                GsummaryColumn3.MappingName = "SGST_Amnt";
                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn3);

                GridSummaryColumn GsummaryColumn4 = new GridSummaryColumn();
                GsummaryColumn4.Name = "Total_IGST_Amnt";
                GsummaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                GsummaryColumn4.Format = "{Sum}";
                GsummaryColumn4.MappingName = "IGST_Amnt";
                // Adds the GridSummaryColumn in SummaryColumns collection.
                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn4);

                // Adds the summary row in the GroupSummaryRows collection.
                this.sfDataGrid1.GroupSummaryRows.Add(groupSummaryRow1);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnExcell_Click(object sender, EventArgs e)
        {
            DateTime dt = dpFromDate.Value;
            //System.Data.OleDb.OleDbConnection MyConnection;
            //System.Data.DataTable DtSet;
            //System.Data.OleDb.OleDbDataAdapter MyCommand;
            string dt1 = dt.ToString("dd/MM/yyyy");

            DateTime dtt = dpTodate.Value;
            string dt2 = dtt.ToString("dd/MM/yyyy");
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 3;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            workBook.Worksheets[0].Range["A2"].Value = "Sales Data - GSTR";
            workBook.Worksheets[0].Range["D2"].Value = "Period :" + dt1 + "-" + dt2;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\GST_Sales.xlsx");
            string doc = Fname + "\\GST_Sales.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
           // MessageBox.Show("Excel File Saved in " + Fname + " Folder");


            ////Convert to Json
            //var pathToExcel = Fname + "\\GST_Sales.xls";
            ////var sheetName = "Sheet1";

            //MyConnection = new OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + pathToExcel + ";Extended Properties='Excel 8.0;HDR=Yes'");
            //string sheetname = "Sheet1";
            //MyCommand = new System.Data.OleDb.OleDbDataAdapter("Select * From [" + sheetname + "$]", MyConnection);

            //MyCommand.TableMappings.Add("Table", pathToExcel);
            //DtSet = new System.Data.DataTable();

            //MyCommand.Fill(DtSet);
            //var file = Fname + "\\GST_Sales.xls";//client.DownloadData("https://devhow.net/wp-content/uploads/2018/11/SampleExcelData.xlsx");
            //var excelContent = ParseExcel(new MemoryStream(file));

            //string json = JsonConvert.SerializeObject(excelContent);
            //Console.Write(json);
            //Newtonsoft.Json.JsonConvert.SerializeObject(MyCommand);
            ////  return json;

            //Instantiate the spreadsheet creation engine.
            //using (ExcelEngine excelEngine1 = new ExcelEngine())
            //{
            //    IApplication application = excelEngine1.Excel;

            //    //The workbook is opened.
            //    FileStream fileStream = new FileStream(Fname + "\\convertcsv (1).xlsx", FileMode.Open);

            //    IWorkbook workbook = application.Workbooks.Open(fileStream, ExcelOpenType.Automatic);
            //    IWorksheet worksheet = workbook.Worksheets[0];

            //    ////Export worksheet data into CLR Objects
            //    IList<GSTData> GSTSales = worksheet.ExportData<GSTData>(1, 1, worksheet.UsedRange.LastRow, workbook.Worksheets[0].UsedRange.LastColumn);

            //    //open file stream

            //    using (StreamWriter file = File.CreateText(Fname + "\\GST_Sales.json"))
            //    {
            //        JsonSerializer serializer = new JsonSerializer();

            //        //serialize object directly into file stream
            //        serializer.Serialize(file, GSTSales);
            //    }
            //}
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmGSTR_PurchaseData_Load(object sender, EventArgs e)
        {
            dpFromDate.MinDate = logIn.fy_Start_Date;
            dpTodate.MaxDate = logIn.fy_End_Date;
        }

        
        public class GSTData
        {
            #region Members
            private string m_CustGST;
            private string m_customerName;
            private DateTime m_date;
            private string m_country;
            #endregion

            #region Prperties
            [DisplayNameAttribute("ctin CustomerName Date Country")]
            public string ctin
            {
                get
                {
                    return m_CustGST;
                }
                set
                {
                    m_CustGST = value;
                }
            }

            public string CustomerName
            {
                get
                {
                    return m_customerName;
                }
                set
                {
                    m_customerName = value;
                }
            }
            public DateTime Date
            {
                get
                {
                    return m_date;
                }
                set
                {
                    m_date = value;
                }

            }
            public string Country
            {
                get
                {
                    return m_country;
                }
                set
                {
                    m_country = value;
                }

            }
            #endregion

            #region Intialization
            public GSTData()
            {
            }

            #endregion
        }

    }

}
        

