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
using Syncfusion.WinForms.DataGrid.Interactivity;
using Syncfusion.WinForms.GridCommon.ScrollAxis;
using Ione_DAL;

namespace ioneNet.MaterialManagement.Reports
{
    public partial class StockReport_Rawmaterial : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();
        public static string InvoiceNoList, Order_NoList, SO_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                sfDataGrid1.DataSource = null;
                DateTime dt = dtpFrmDate.Value;
                string dt1 = dt.ToString("yyyy/MM/dd");

                DateTime dtt = dateTimePicker1.Value;
                string dt2 = dtt.ToString("yyyy/MM/dd");

                
                if (checkBox2.Checked)
                {
                    sfDataGrid1.DataSource = null;

                    SqlCommand cmd2 = new SqlCommand("ShowStockReport_AgeWise", con);
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

                    //    var d = (from data in db.ShowStockReport_New(logIn.company, dt1, dt2, logIn.BU_ID) select data).ToList();
                    if (ds2.Rows.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        sfDataGrid1.DataSource = ds2;

                        this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                        this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                        this.sfDataGrid1.Columns["Item_Name"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["Item_Name"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["Item_Name"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["Item_Name"].FilterRowCondition = FilterRowCondition.Contains;

                        this.sfDataGrid1.Columns["Prod_Code"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["Prod_Code"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["Prod_Code"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["Prod_Code"].FilterRowCondition = FilterRowCondition.Contains;



                        this.sfDataGrid1.TableSummaryRows.Clear();
                        //this.sfDataGrid1.GroupSummaryRows.Clear();
                        GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                        tableSummaryRow1.Name = "TableSummary";
                        tableSummaryRow1.ShowSummaryInRow = false;
                        tableSummaryRow1.Position = VerticalPosition.Bottom;

                        GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                        summaryColumn1.Name = "Stock_Value";
                        summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn1.Format = "{Sum}";
                        summaryColumn1.MappingName = "Stock_Value";
                        tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

                        GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                        summaryColumn2.Name = "Less_Than_3_Months";
                        summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn2.Format = "{Sum}";
                        summaryColumn2.MappingName = "Less_Than_3_Months";
                        tableSummaryRow1.SummaryColumns.Add(summaryColumn2);

                        GridSummaryColumn summaryColumn3 = new GridSummaryColumn();
                        summaryColumn3.Name = "3_to_6_Months";
                        summaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn3.Format = "{Sum}";
                        summaryColumn3.MappingName = "3_to_6_Months";
                        tableSummaryRow1.SummaryColumns.Add(summaryColumn3);

                        GridSummaryColumn summaryColumn4 = new GridSummaryColumn();
                        summaryColumn4.Name = "6_to_12_Months";
                        summaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn4.Format = "{Sum}";
                        summaryColumn4.MappingName = "6_to_12_Months";
                        tableSummaryRow1.SummaryColumns.Add(summaryColumn4);

                        GridSummaryColumn summaryColumn5 = new GridSummaryColumn();
                        summaryColumn5.Name = "Above_12_Months";
                        summaryColumn5.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn5.Format = "{Sum}";
                        summaryColumn5.MappingName = "Above_12_Months";
                        tableSummaryRow1.SummaryColumns.Add(summaryColumn5);



                        this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);


                    }

                }

                else
                {
                        
                       
                    sfDataGrid1.DataSource = null;

                    SqlCommand cmd2 = new SqlCommand("ShowStockReport_RM", con);
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


                    //var d = (from data in db.ShowStockReport_New(logIn.company, dtpFrmDate.Value, dateTimePicker1.Value, logIn.BU_ID) select data).ToList();
                    if (ds2.Rows.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        sfDataGrid1.DataSource = ds2;

                        this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                        this.sfDataGrid1.Columns["Item_Name"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["Item_Name"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["Item_Name"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["Item_Name"].FilterRowCondition = FilterRowCondition.Contains;

                        this.sfDataGrid1.Columns["Prod_Code"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["Prod_Code"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["Prod_Code"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["Prod_Code"].FilterRowCondition = FilterRowCondition.Contains;

                        this.sfDataGrid1.Columns["RM_Grade"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["RM_Grade"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["RM_Grade"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["RM_Grade"].FilterRowCondition = FilterRowCondition.Contains;


                        


                        this.sfDataGrid1.TableSummaryRows.Clear();
                        //this.sfDataGrid1.GroupSummaryRows.Clear();
                        GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                        tableSummaryRow1.Name = "TableSummary";
                        tableSummaryRow1.ShowSummaryInRow = false;
                        tableSummaryRow1.Position = VerticalPosition.Bottom;

                        //GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                        //summaryColumn1.Name = "obValue";
                        //summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                        //summaryColumn1.Format = "{Sum}";
                        //summaryColumn1.MappingName = "obValue";
                        //tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

                        GridSummaryColumn summaryColumnOBQty1 = new GridSummaryColumn();
                        summaryColumnOBQty1.Name = "obQty";
                        summaryColumnOBQty1.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumnOBQty1.Format = "{Sum}";
                        summaryColumnOBQty1.MappingName = "obQty";
                        tableSummaryRow1.SummaryColumns.Add(summaryColumnOBQty1);


                        //GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                        //summaryColumn2.Name = "ReceiptValue";
                        //summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                        //summaryColumn2.Format = "{Sum}";
                        //summaryColumn2.MappingName = "ReceiptValue";
                        //tableSummaryRow1.SummaryColumns.Add(summaryColumn2);

                        GridSummaryColumn summaryColumnReceipt2 = new GridSummaryColumn();
                        summaryColumnReceipt2.Name = "ReceiptQty";
                        summaryColumnReceipt2.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumnReceipt2.Format = "{Sum}";
                        summaryColumnReceipt2.MappingName = "ReceiptQty";
                        tableSummaryRow1.SummaryColumns.Add(summaryColumnReceipt2);

                        //GridSummaryColumn summaryColumn3 = new GridSummaryColumn();
                        //summaryColumn3.Name = "IssueValue";
                        //summaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                        //summaryColumn3.Format = "{Sum}";
                        //summaryColumn3.MappingName = "IssueValue";
                        //tableSummaryRow1.SummaryColumns.Add(summaryColumn3);

                        GridSummaryColumn summaryColumnIss3 = new GridSummaryColumn();
                        summaryColumnIss3.Name = "IssueQty";
                        summaryColumnIss3.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumnIss3.Format = "{Sum}";
                        summaryColumnIss3.MappingName = "IssuedQty";
                        tableSummaryRow1.SummaryColumns.Add(summaryColumnIss3);



                        GridSummaryColumn summaryColumn4 = new GridSummaryColumn();
                        summaryColumn4.Name = "ClosingQty";
                        summaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn4.Format = "{Sum}";
                        summaryColumn4.MappingName = "ClosingQty";
                        tableSummaryRow1.SummaryColumns.Add(summaryColumn4);


                        this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);



                        //// Creates the GridSummaryRow.
                        GridSummaryRow groupSummaryRow1 = new GridSummaryRow();
                        groupSummaryRow1.Name = "GroupSummary";
                        groupSummaryRow1.ShowSummaryInRow = false;
                        GridSummaryColumn GsummaryColumn1 = new GridSummaryColumn();
                        GsummaryColumn1.Name = "obQty";
                        GsummaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                        GsummaryColumn1.Format = "{Sum}";
                        GsummaryColumn1.MappingName = "obQty";

                        //// Adds the GridSummaryColumn in SummaryColumns collection.
                        groupSummaryRow1.SummaryColumns.Add(GsummaryColumn1);


                        //// Adds the summary row in the GroupSummaryRows collection.

                        GridSummaryColumn GsummaryColumn2 = new GridSummaryColumn();
                        GsummaryColumn2.Name = "ReceiptQty";
                        GsummaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                        GsummaryColumn2.Format = "{Sum}";
                        GsummaryColumn2.MappingName = "ReceiptQty";
                        // Adds the GridSummaryColumn in SummaryColumns collection.
                        groupSummaryRow1.SummaryColumns.Add(GsummaryColumn2);



                        GridSummaryColumn GsummaryColumn3 = new GridSummaryColumn();
                        GsummaryColumn3.Name = "IssuedQty";
                        GsummaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                        GsummaryColumn3.Format = "{Sum}";
                        GsummaryColumn3.MappingName = "IssuedQty";
                        // Adds the GridSummaryColumn in SummaryColumns collection.
                        groupSummaryRow1.SummaryColumns.Add(GsummaryColumn3);


                        GridSummaryColumn GsummaryColumn5 = new GridSummaryColumn();
                        GsummaryColumn5.Name = "ClosingQty";
                        GsummaryColumn5.SummaryType = SummaryType.DoubleAggregate;
                        GsummaryColumn5.Format = "{Sum}";
                        GsummaryColumn5.MappingName = "ClosingQty";
                        // Adds the GridSummaryColumn in SummaryColumns collection.
                        groupSummaryRow1.SummaryColumns.Add(GsummaryColumn5);



                        //// Adds the summary row in the GroupSummaryRows collection.
                        this.sfDataGrid1.GroupSummaryRows.Add(groupSummaryRow1);
                    }
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

            DateTime dtt = dateTimePicker1.Value;
            string dt2 = dtt.ToString("dd/MM/yyyy");
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 3;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            if (checkBox2.Checked)
            {

                workBook.Worksheets[0].Range["A2"].Value = "Stock Statement - Age Wise";

            }
            else
            {
                workBook.Worksheets[0].Range["A2"].Value = "Stock Statement - Raw Material";
            }
            workBook.Worksheets[0].Range["D2"].Value = "Period :" + dt1 + "-" + dt2;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string doc = "";
            if (checkBox2.Checked)
            {
                workBook.SaveAs(Fname + "\\StockStatement_Ageing.xlsx");
                doc = Fname + "\\StockStatement_Ageing.xlsx";
            }
            else
            {
                workBook.SaveAs(Fname + "\\StockStatement_RM.xlsx");
                doc = Fname + "\\StockStatement_RM.xlsx";
            }

            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        public StockReport_Rawmaterial()
        {
            InitializeComponent();
        }

        private void StockReport_Rawmaterial_Load(object sender, EventArgs e)
        {

        }
    }
}
