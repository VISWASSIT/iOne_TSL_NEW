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
   
    public partial class StockStatement : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();
        public static string InvoiceNoList, Order_NoList, SO_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;

        private void sfButton1_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 2; i < sfDataGrid1.RowCount; i++)
                {
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["prod_ID"].MappingName;
                    //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);                   
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                    var mappingName1 = sfDataGrid1.Columns["ClosingValue"].MappingName;
                    //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);                   
                    var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());

                    string CVal = cellVaue1.ToString();
                    if (Convert.ToInt32(CVal) < 0)
                    {
                        string OrdNo = cellVaue.ToString();

                        string dt2 = "2020/04/01";
                        string dt3 = "2022/11/30";
                        SqlDataReader rdr = null;

                        SqlCommand cmd2 = new SqlCommand("UpdateStockAdj_Price", con);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Parameters.AddWithValue("@Company", logIn.company);
                        cmd2.Parameters.AddWithValue("@ProductID", Convert.ToInt32(OrdNo));
                        cmd2.Parameters.AddWithValue("@frmDate", Convert.ToDateTime(dt3));
                        cmd2.Parameters.AddWithValue("@todate", Convert.ToDateTime(dt3));
                        cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);

                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        if (con.State != ConnectionState.Open)
                        {
                            con.Close();
                            con.Open();
                        }
                        //  con.Open();
                        DataTable ds2 = new DataTable();
                        rdr = cmd2.ExecuteReader();
                        con.Close();
                    }
               }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public StockStatement()
        {
            InitializeComponent();
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

                if (checkBox1.Checked)
                {
                    sfDataGrid1.DataSource = null;

                    SqlCommand cmd2 = new SqlCommand("ShowStockReport_Summary", con);
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

                        this.sfDataGrid1.Columns["Prod_Group_Name"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["Prod_Group_Name"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["Prod_Group_Name"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["Prod_Group_Name"].FilterRowCondition = FilterRowCondition.Contains;



                        this.sfDataGrid1.TableSummaryRows.Clear();
                        //this.sfDataGrid1.GroupSummaryRows.Clear();
                        GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                        tableSummaryRow1.Name = "TableSummary";
                        tableSummaryRow1.ShowSummaryInRow = false;
                        tableSummaryRow1.Position = VerticalPosition.Bottom;

                        GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                        summaryColumn1.Name = "OBValue";
                        summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn1.Format = "{Sum}";
                        summaryColumn1.MappingName = "OBValue";
                        tableSummaryRow1.SummaryColumns.Add(summaryColumn1);


                        GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                        summaryColumn2.Name = "Purchase_Value";
                        summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn2.Format = "{Sum}";
                        summaryColumn2.MappingName = "Purchase_Value";
                        tableSummaryRow1.SummaryColumns.Add(summaryColumn2);



                        GridSummaryColumn summaryColumn3 = new GridSummaryColumn();
                        summaryColumn3.Name = "Import_Purchase";
                        summaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn3.Format = "{Sum}";
                        summaryColumn3.MappingName = "Import_Purchase";
                        tableSummaryRow1.SummaryColumns.Add(summaryColumn3);


                        GridSummaryColumn summaryColumn4 = new GridSummaryColumn();
                        summaryColumn4.Name = "JW_Receipts";
                        summaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn4.Format = "{Sum}";
                        summaryColumn4.MappingName = "JW_Receipts";
                        tableSummaryRow1.SummaryColumns.Add(summaryColumn4);

                        GridSummaryColumn summaryColumn5 = new GridSummaryColumn();
                        summaryColumn5.Name = "Production_Receipts";
                        summaryColumn5.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn5.Format = "{Sum}";
                        summaryColumn5.MappingName = "Production_Receipts";
                        tableSummaryRow1.SummaryColumns.Add(summaryColumn5);

                        GridSummaryColumn summaryColumn6 = new GridSummaryColumn();
                        summaryColumn6.Name = "Issue_Returns";
                        summaryColumn6.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn6.Format = "{Sum}";
                        summaryColumn6.MappingName = "Issue_Returns";
                        tableSummaryRow1.SummaryColumns.Add(summaryColumn6);

                        GridSummaryColumn summaryColumn7 = new GridSummaryColumn();
                        summaryColumn7.Name = "Sale_Returns";
                        summaryColumn7.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn7.Format = "{Sum}";
                        summaryColumn7.MappingName = "Sale_Returns";
                        tableSummaryRow1.SummaryColumns.Add(summaryColumn7);

                        GridSummaryColumn summaryColumn8 = new GridSummaryColumn();
                        summaryColumn8.Name = "Quarantine_Stock_Val";
                        summaryColumn8.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn8.Format = "{Sum}";
                        summaryColumn8.MappingName = "Quarantine_Stock_Val";
                        tableSummaryRow1.SummaryColumns.Add(summaryColumn8);

                        GridSummaryColumn summaryColumn9 = new GridSummaryColumn();
                        summaryColumn9.Name = "IssueValue";
                        summaryColumn9.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn9.Format = "{Sum}";
                        summaryColumn9.MappingName = "IssueValue";
                        tableSummaryRow1.SummaryColumns.Add(summaryColumn9);

                        GridSummaryColumn summaryColumn10 = new GridSummaryColumn();
                        summaryColumn10.Name = "GP_Value";
                        summaryColumn10.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn10.Format = "{Sum}";
                        summaryColumn10.MappingName = "GP_Value";
                        tableSummaryRow1.SummaryColumns.Add(summaryColumn10);

                        GridSummaryColumn summaryColumn11 = new GridSummaryColumn();
                        summaryColumn11.Name = "Sale_Value";
                        summaryColumn11.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn11.Format = "{Sum}";
                        summaryColumn11.MappingName = "Sale_Value";
                        tableSummaryRow1.SummaryColumns.Add(summaryColumn11);

                        GridSummaryColumn summaryColumn12 = new GridSummaryColumn();
                        summaryColumn12.Name = "issued_production";
                        summaryColumn12.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn12.Format = "{Sum}";
                        summaryColumn12.MappingName = "issued_production";
                        tableSummaryRow1.SummaryColumns.Add(summaryColumn12);

                        GridSummaryColumn summaryColumn13 = new GridSummaryColumn();
                        summaryColumn13.Name = "Purhcase_Returns";
                        summaryColumn13.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn13.Format = "{Sum}";
                        summaryColumn13.MappingName = "Purhcase_Returns";
                        tableSummaryRow1.SummaryColumns.Add(summaryColumn13);

                        GridSummaryColumn summaryColumn14 = new GridSummaryColumn();
                        summaryColumn14.Name = "Stock_Adj_Value";
                        summaryColumn14.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn14.Format = "{Sum}";
                        summaryColumn14.MappingName = "Stock_Adj_Value";
                        tableSummaryRow1.SummaryColumns.Add(summaryColumn14);

                        GridSummaryColumn summaryColumn15 = new GridSummaryColumn();
                        summaryColumn15.Name = "ClosingValue";
                        summaryColumn15.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn15.Format = "{Sum}";
                        summaryColumn15.MappingName = "ClosingValue";
                        tableSummaryRow1.SummaryColumns.Add(summaryColumn15);


                        this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);


                    }
                }
                else
                {
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
                        if (checkBox3.Checked)
                        {
                            sfDataGrid1.DataSource = null;

                            SqlCommand cmd2 = new SqlCommand("ShowStockReport_Only", con);
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
                                summaryColumn1.Name = "ClosingValue";
                                summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                                summaryColumn1.Format = "{Sum}";
                                summaryColumn1.MappingName = "ClosingValue";
                                tableSummaryRow1.SummaryColumns.Add(summaryColumn1);
                                this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);
                            }
                        }
                        else
                        {
                            sfDataGrid1.DataSource = null;

                            SqlCommand cmd2 = new SqlCommand("ShowStockReport_New", con);
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


                                this.sfDataGrid1.TableSummaryRows.Clear();
                                //this.sfDataGrid1.GroupSummaryRows.Clear();
                                GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                                tableSummaryRow1.Name = "TableSummary";
                                tableSummaryRow1.ShowSummaryInRow = false;
                                tableSummaryRow1.Position = VerticalPosition.Bottom;

                                GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                                summaryColumn1.Name = "obValue";
                                summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                                summaryColumn1.Format = "{Sum}";
                                summaryColumn1.MappingName = "obValue";
                                tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

                                //GridSummaryColumn summaryColumnOBQty1 = new GridSummaryColumn();
                                //summaryColumnOBQty1.Name = "obQty";
                                //summaryColumnOBQty1.SummaryType = SummaryType.DoubleAggregate;
                                //summaryColumnOBQty1.Format = "{Sum}";
                                //summaryColumnOBQty1.MappingName = "obQty";
                                //tableSummaryRow1.SummaryColumns.Add(summaryColumnOBQty1);


                                GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                                summaryColumn2.Name = "ReceiptValue";
                                summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                                summaryColumn2.Format = "{Sum}";
                                summaryColumn2.MappingName = "ReceiptValue";
                                tableSummaryRow1.SummaryColumns.Add(summaryColumn2);

                                //GridSummaryColumn summaryColumnReceipt2 = new GridSummaryColumn();
                                //summaryColumnReceipt2.Name = "ReceiptQty";
                                //summaryColumnReceipt2.SummaryType = SummaryType.DoubleAggregate;
                                //summaryColumnReceipt2.Format = "{Sum}";
                                //summaryColumnReceipt2.MappingName = "ReceiptQty";
                                //tableSummaryRow1.SummaryColumns.Add(summaryColumnReceipt2);

                                GridSummaryColumn summaryColumn3 = new GridSummaryColumn();
                                summaryColumn3.Name = "IssueValue";
                                summaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                                summaryColumn3.Format = "{Sum}";
                                summaryColumn3.MappingName = "IssueValue";
                                tableSummaryRow1.SummaryColumns.Add(summaryColumn3);

                                //GridSummaryColumn summaryColumnIss3 = new GridSummaryColumn();
                                //summaryColumnIss3.Name = "IssueQty";
                                //summaryColumnIss3.SummaryType = SummaryType.DoubleAggregate;
                                //summaryColumnIss3.Format = "{Sum}";
                                //summaryColumnIss3.MappingName = "IssuedQty";
                                //tableSummaryRow1.SummaryColumns.Add(summaryColumnIss3);



                                GridSummaryColumn summaryColumn4 = new GridSummaryColumn();
                                summaryColumn4.Name = "ClosingValue";
                                summaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                                summaryColumn4.Format = "{Sum}";
                                summaryColumn4.MappingName = "ClosingValue";
                                tableSummaryRow1.SummaryColumns.Add(summaryColumn4);

                                GridSummaryColumn summaryColumnCBQty = new GridSummaryColumn();
                                summaryColumnCBQty.Name = "Quarantine_Stock_Val";
                                summaryColumnCBQty.SummaryType = SummaryType.DoubleAggregate;
                                summaryColumnCBQty.Format = "{Sum}";
                                summaryColumnCBQty.MappingName = "Quarantine_Stock_Val";
                                tableSummaryRow1.SummaryColumns.Add(summaryColumnCBQty);

                                this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);



                                // Creates the GridSummaryRow.
                                GridSummaryRow groupSummaryRow1 = new GridSummaryRow();
                                groupSummaryRow1.Name = "GroupSummary";
                                groupSummaryRow1.ShowSummaryInRow = false;
                                GridSummaryColumn GsummaryColumn1 = new GridSummaryColumn();
                                GsummaryColumn1.Name = "OBValue";
                                GsummaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                                GsummaryColumn1.Format = "{Sum}";
                                GsummaryColumn1.MappingName = "obValue";

                                // Adds the GridSummaryColumn in SummaryColumns collection.
                                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn1);


                                // Adds the summary row in the GroupSummaryRows collection.

                                GridSummaryColumn GsummaryColumn2 = new GridSummaryColumn();
                                GsummaryColumn2.Name = "ReceiptValue";
                                GsummaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                                GsummaryColumn2.Format = "{Sum}";
                                GsummaryColumn2.MappingName = "ReceiptValue";
                                // Adds the GridSummaryColumn in SummaryColumns collection.
                                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn2);



                                GridSummaryColumn GsummaryColumn3 = new GridSummaryColumn();
                                GsummaryColumn3.Name = "IssueValue";
                                GsummaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                                GsummaryColumn3.Format = "{Sum}";
                                GsummaryColumn3.MappingName = "IssueValue";
                                // Adds the GridSummaryColumn in SummaryColumns collection.
                                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn3);


                                GridSummaryColumn GsummaryColumn4 = new GridSummaryColumn();
                                GsummaryColumn4.Name = "Quarantine_Stock_Val";
                                GsummaryColumn4.SummaryType = SummaryType.DoubleAggregate;
                                GsummaryColumn4.Format = "{Sum}";
                                GsummaryColumn4.MappingName = "Quarantine_Stock_Val";
                                // Adds the GridSummaryColumn in SummaryColumns collection.
                                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn4);


                                GridSummaryColumn GsummaryColumn5 = new GridSummaryColumn();
                                GsummaryColumn5.Name = "ClosingValue";
                                GsummaryColumn5.SummaryType = SummaryType.DoubleAggregate;
                                GsummaryColumn5.Format = "{Sum}";
                                GsummaryColumn5.MappingName = "ClosingValue";
                                // Adds the GridSummaryColumn in SummaryColumns collection.
                                groupSummaryRow1.SummaryColumns.Add(GsummaryColumn5);



                                // Adds the summary row in the GroupSummaryRows collection.
                                this.sfDataGrid1.GroupSummaryRows.Add(groupSummaryRow1);
                            }
                        }
                    }
                }
            
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void sfDataGrid_QueryCellStyle(object sender, Syncfusion.WinForms.DataGrid.Events.QueryCellStyleEventArgs e)
        {
            var rowColumnIndex = new RowColumnIndex(e.RowIndex, e.ColumnIndex);
            if (colorDict.ContainsKey(rowColumnIndex))
                e.Style.BackColor = colorDict[rowColumnIndex];
        }


        void SetCellBackgroundColor(RowColumnIndex rowColumnIndex, Color color)
        {
            if (!colorDict.ContainsKey(rowColumnIndex))
                colorDict.Add(rowColumnIndex, color);
            else
                colorDict[rowColumnIndex] = color;
            sfDataGrid1.TableControl.Invalidate(this.sfDataGrid1.TableControl.GetCellRectangle(rowColumnIndex.RowIndex, rowColumnIndex.ColumnIndex, false));
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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
                workBook.Worksheets[0].Range["A2"].Value = "Stock Statement";
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
                workBook.SaveAs(Fname + "\\StockStatement.xlsx");
                 doc = Fname + "\\StockStatement.xlsx";
            }                
            
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void StockStatement_Load(object sender, EventArgs e)
        {

        }

        private void viewLedgerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                //var mappingName = sfDataGrid1.Columns["Item_name"].MappingName;
                //var mappingName1 = sfDataGrid1.Columns["status"].MappingName;
                //var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                if (currentCellValue.ToString() != "")
                {
                    SO_No = currentCellValue.ToString(); ;
                    var = "0";                    
                    MaterialManagement.Reports.StockLedger frm = new MaterialManagement.Reports.StockLedger();
                    //OrderManagement.Transactions.
                    frm.MdiParent = this.MdiParent;
                    frm.Show();
                    //FrmInv.ShowDialog();
                    //i1 = 0;
                }
                else
                {
                    MessageBox.Show("Please Select Any One Record");
                    //i1 = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
