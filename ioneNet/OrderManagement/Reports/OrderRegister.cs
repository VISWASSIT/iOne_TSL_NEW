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
//using Syncfusion.WinForms.DataGrid.DataGridConverter;
using Ione_DAL;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Syncfusion.WinForms.DataGrid.Events;
using ioneNet.OrderManagement.Transactions;

namespace ioneNet.OrderManagement.Reports
{
    public partial class OrderRegister : Form
    {
        public static string InvoiceNoList, Order_NoList, SO_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);


        private void dgvRecordList_FilterStringChanged(object sender, EventArgs e)
        {
           
            decimal x = 0;
           

        }

        private void dgvRecordList_SortStringChanged(object sender, EventArgs e)
        {
           
        }

        private void approveToolStripMenuItem_Click(object sender, EventArgs e)
        {
           try
           {
                
          }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }            
        }

        private void accountPostingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sfDataGrid1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
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
            workBook.Worksheets[0].Range["A2"].Value = "Order Register";
            workBook.Worksheets[0].Range["D2"].Value = "Period :" + dt1 + "-" + dt2;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\Order_Register.xlsx");
            string doc = Fname + "\\Order_Register.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void preCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                string cellValue;
                for (int i = 1; i < sfDataGrid1.RowCount; i++)
                {
                    foreach (var item in sfDataGrid1.SelectedItems)
                    {

                        //foreach (var col in sfDataGrid1.Columns)
                        //{
                        //if (col.MappingName == "Alternative_Code")
                        //{
                        //var mappingName = sfDataGrid1.Columns[columnIndex].MappingName;
                        //var recordIndex = sfDataGrid1.TableControl.ResolveToRecordIndex(rowIndex);
                        var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                        var mappingName = sfDataGrid1.Columns[0].MappingName;
                        //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                        //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                        if (rowData == item)
                        {
                            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                            SqlCommand cmd = new SqlCommand();

                            SO_No = cellVaue.ToString();
                            cmd.CommandText = "Update Sale_Order_Master set status = '26' where So_No=@param1 and Company_ID =@compName";
                            cmd.Parameters.AddWithValue("@param1", SO_No);
                            cmd.Parameters.AddWithValue("@CompName", logIn.company);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            cmd.Parameters.Clear();
                            string strT = logIn.username + "-" + DateTime.Now;
                            cmd.CommandText = "Update Sale_Order_Master set Modified_By = @strT where So_No=@param1 and Company_ID =@compName";
                            cmd.Parameters.AddWithValue("@strT", strT);
                            cmd.Parameters.AddWithValue("@param1", SO_No);
                            cmd.Parameters.AddWithValue("@CompName", logIn.company);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                //MessageBox.Show("Selected Order(s) Are Pre-Closed Successfully");
                //BindOrderslist();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void allotStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = sfDataGrid1.CurrentCell.RowIndex;
            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
            //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
            var currentCellValue = (rowData.GetType().GetProperty("SO_NO").GetValue(rowData, null).ToString());

            SO_No = currentCellValue.ToString();
            OrderManagement.Transactions.Forge_StockAllotment frm = new Forge_StockAllotment();
            //frm.MdiParent = this.MdiParent;
            frm.ShowDialog();
        }

        private void reOpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                string cellValue;
                for (int i = 1; i < sfDataGrid1.RowCount; i++)
                {
                    foreach (var item in sfDataGrid1.SelectedItems)
                    {

                        //foreach (var col in sfDataGrid1.Columns)
                        //{
                        //if (col.MappingName == "Alternative_Code")
                        //{
                        //var mappingName = sfDataGrid1.Columns[columnIndex].MappingName;
                        //var recordIndex = sfDataGrid1.TableControl.ResolveToRecordIndex(rowIndex);
                        var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                        var mappingName = sfDataGrid1.Columns[0].MappingName;
                        //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                        //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                        if (rowData == item)
                        {
                            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                            SqlCommand cmd = new SqlCommand();

                            SO_No = cellVaue.ToString();
                            cmd.CommandText = "Update Sale_Order_Master set status = '6' where So_No=@param1 and Company_ID =@compName";
                            cmd.Parameters.AddWithValue("@param1", SO_No);
                            cmd.Parameters.AddWithValue("@CompName", logIn.company);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            cmd.Parameters.Clear();
                            string strT = logIn.username + "-" + DateTime.Now;
                            cmd.CommandText = "Update Sale_Order_Master set Modified_By = @strT where So_No=@param1 and Company_ID =@compName";
                            cmd.Parameters.AddWithValue("@strT", strT);
                            cmd.Parameters.AddWithValue("@param1", SO_No);
                            cmd.Parameters.AddWithValue("@CompName", logIn.company);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                //MessageBox.Show("Selected Order(s) Are Pre-Closed Successfully");
                //BindOrderslist();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ListOfOrders_Load(object sender, EventArgs e)
        {
            DateTime t = dtpToDate.Value;
            DateTime f = dtpFrmDate.Value;
            string f1 = f.ToString("dd/MMM/yyyy");
            string t1 = t.ToString("dd/MMM/yyyy");

            List<string> OrderType = new List<string>();
            OrderType.Add("All");
            OrderType.Add("Only Pending");
            OrderType.Add("Pre-Closed");           
            cmbViewOrders.DataSource = OrderType;

        }


        public static Boolean editMode;
        public OrderRegister()
        {
            InitializeComponent();
        }

        
        

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            editMode = false;
            //OrderManagement.Transactions.frmNewOrder frm = new frmNewOrder();
           //frm.MdiParent = this.MdiParent;
           //frm.Show();
        }

        private void btnSearch_Click(object sender, EventArgs e)

        {
            try
            {
                this.sfDataGrid1.TableSummaryRows.Clear();
                if(checkBox1.Checked)
                {

                    var d = (from data in db.OrderSummaryReport(logIn.company, dtpFrmDate.Value, dtpToDate.Value, null, 2, logIn.BU_ID) select data).ToList();
                    if (d.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        sfDataGrid1.DataSource = d;
                        this.sfDataGrid1.QueryCellStyle += SfDataGrid1_QueryCellStyle;


                        this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                        this.sfDataGrid1.Columns["Item_Description"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["Item_Description"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["Item_Description"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["Item_Description"].FilterRowCondition = FilterRowCondition.Contains;

                        GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                        tableSummaryRow1.Name = "TableSummary";
                        tableSummaryRow1.ShowSummaryInRow = false;
                        tableSummaryRow1.Position = VerticalPosition.Bottom;

                        GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                        summaryColumn1.Name = "Order_Bal_Qty";
                        summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn1.Format = "Total : {Sum}";
                        summaryColumn1.MappingName = "Order_Bal_Qty";

                        tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

                        GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                        summaryColumn2.Name = "Stock_Available";
                        summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn2.Format = "Total : {Sum}";
                        summaryColumn2.MappingName = "Stock_Available";

                        tableSummaryRow1.SummaryColumns.Add(summaryColumn2);
                        this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);


                        GridSummaryRow groupSummaryRow1 = new GridSummaryRow();
                        groupSummaryRow1.Name = "GroupSummary";
                        groupSummaryRow1.ShowSummaryInRow = false;

                        // Creates the GridSummaryColumn.
                        GridSummaryColumn GsummaryColumn1 = new GridSummaryColumn();
                        GsummaryColumn1.Name = "Order_Bal_Qty";
                        GsummaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                        GsummaryColumn1.Format = "Total Ord Qty : {Sum:c}";
                        GsummaryColumn1.MappingName = "Order_Bal_Qty";

                        // Adds the GridSummaryColumn in SummaryColumns collection.
                        groupSummaryRow1.SummaryColumns.Add(GsummaryColumn1);

                        GridSummaryColumn GsummaryColumn2 = new GridSummaryColumn();
                        GsummaryColumn2.Name = "Stock_Available";
                        GsummaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                        GsummaryColumn2.Format = "Total Stock_Avbl : {Sum:c}";
                        GsummaryColumn2.MappingName = "Stock_Available";

                        // Adds the GridSummaryColumn in SummaryColumns collection.
                        groupSummaryRow1.SummaryColumns.Add(GsummaryColumn2);



                        // Adds the summary row in the GroupSummaryRows collection.
                        this.sfDataGrid1.GroupSummaryRows.Add(groupSummaryRow1);
                    }
                }
                else
                {
                    if (cmbViewOrders.Text != "")
                    {
                        if (cmbViewOrders.Text == "Only Pending")
                        {
                            var d = (from data in db.OrderRegister_TSL(logIn.company, dtpFrmDate.Value, dtpToDate.Value, null, 2, logIn.BU_ID) select data).ToList();
                            if (d.Count > 0)
                            {
                                //dgProductsList.DataSource = d;
                                sfDataGrid1.DataSource = d;
                            }
                        }
                        else
                        {
                            if (cmbViewOrders.Text == "All")
                            {
                                var d1 = (from data in db.OrderRegister_TSL(logIn.company, dtpFrmDate.Value, dtpToDate.Value, null, 1, logIn.BU_ID) select data).ToList();
                                if (d1.Count > 0)
                                {
                                    //dgProductsList.DataSource = d;
                                    sfDataGrid1.DataSource = d1;

                                }
                            }
                            else
                            {
                                if (cmbViewOrders.Text == "Pre-Closed")
                                {
                                    var d1 = (from data in db.OrderRegister_TSL(logIn.company, dtpFrmDate.Value, dtpToDate.Value, null, 3, logIn.BU_ID) select data).ToList();
                                    if (d1.Count > 0)
                                    {
                                        //dgProductsList.DataSource = d;
                                        sfDataGrid1.DataSource = d1;
                                    }
                                }
                            }
                        }
                        this.sfDataGrid1.QueryCellStyle += SfDataGrid1_QueryCellStyle;


                        this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                        this.sfDataGrid1.Columns["SO_NO"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["SO_NO"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["SO_NO"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["SO_NO"].FilterRowCondition = FilterRowCondition.Contains;
                        this.sfDataGrid1.Columns["Customer_Name"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["Customer_Name"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["Customer_Name"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["Customer_Name"].FilterRowCondition = FilterRowCondition.Contains;
                        this.sfDataGrid1.Columns["CustomerPoNo"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["CustomerPoNo"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["CustomerPoNo"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["CustomerPoNo"].FilterRowCondition = FilterRowCondition.Contains;
                        this.sfDataGrid1.Columns["Sale_Exe"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["Sale_Exe"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["Sale_Exe"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["Sale_Exe"].FilterRowCondition = FilterRowCondition.Contains;
                        this.sfDataGrid1.Columns["Item_Description"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["Item_Description"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["Item_Description"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["Item_Description"].FilterRowCondition = FilterRowCondition.Contains;

                        GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                        tableSummaryRow1.Name = "TableSummary";
                        tableSummaryRow1.ShowSummaryInRow = false;
                        tableSummaryRow1.Position = VerticalPosition.Bottom;

                        GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                        summaryColumn1.Name = "Total Order Value";
                        summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn1.Format = "Total : {Sum}";
                        summaryColumn1.MappingName = "Taxable_Value";

                        tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

                        GridSummaryColumn summaryColumn2 = new GridSummaryColumn();
                        summaryColumn2.Name = "Total Bal Qty";
                        summaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn2.Format = "Total : {Sum}";
                        summaryColumn2.MappingName = "Bal_Qty";

                        tableSummaryRow1.SummaryColumns.Add(summaryColumn2);

                        GridSummaryColumn summaryColumn3 = new GridSummaryColumn();
                        summaryColumn3.Name = "Total Ord Qty";
                        summaryColumn3.SummaryType = SummaryType.DoubleAggregate;
                        summaryColumn3.Format = "Total : {Sum}";
                        summaryColumn3.MappingName = "Ord_qty";

                        tableSummaryRow1.SummaryColumns.Add(summaryColumn3);
                        this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);


                        // Creates the GridSummaryRow.
                        GridSummaryRow groupSummaryRow1 = new GridSummaryRow();
                        groupSummaryRow1.Name = "GroupSummary";
                        groupSummaryRow1.ShowSummaryInRow = false;

                        // Creates the GridSummaryColumn.
                        GridSummaryColumn GsummaryColumn1 = new GridSummaryColumn();
                        GsummaryColumn1.Name = "Ord_qty";
                        GsummaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                        GsummaryColumn1.Format = "Total Ord Qty : {Sum:c}";
                        GsummaryColumn1.MappingName = "Ord_qty";

                        // Adds the GridSummaryColumn in SummaryColumns collection.
                        groupSummaryRow1.SummaryColumns.Add(GsummaryColumn1);

                        GridSummaryColumn GsummaryColumn2 = new GridSummaryColumn();
                        GsummaryColumn2.Name = "Bal_Qty";
                        GsummaryColumn2.SummaryType = SummaryType.DoubleAggregate;
                        GsummaryColumn2.Format = "Total Bal Qty : {Sum:c}";
                        GsummaryColumn2.MappingName = "Bal_Qty";

                        // Adds the GridSummaryColumn in SummaryColumns collection.
                        groupSummaryRow1.SummaryColumns.Add(GsummaryColumn2);



                        // Adds the summary row in the GroupSummaryRows collection.
                        this.sfDataGrid1.GroupSummaryRows.Add(groupSummaryRow1);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SfDataGrid1_QueryCellStyle(object sender, QueryCellStyleEventArgs e)
        {
            if (e.Column.MappingName == "Bal_Qty")
            {
                if (e.DisplayText != "0.00")
                {
                    e.Style.BackColor = Color.Orange;
                    e.Style.TextColor = Color.Black;
                }
                else if (e.DisplayText == "0.00")
                {
                    e.Style.BackColor = Color.LightGreen;
                    e.Style.TextColor = Color.DarkSlateBlue;
                }
            }

            if (e.Column.MappingName == "Ord_Type")
            {
                if (e.DisplayText == "Conversion")
                {
                    e.Style.BackColor = Color.LightGreen;
                    e.Style.TextColor = Color.Black;
                }
                else if (e.DisplayText == "Direct Sale")
                {
                    e.Style.BackColor = Color.SkyBlue;
                    e.Style.TextColor = Color.Black;
                }
            }
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            DateTime t = dtpToDate.Value;
            DateTime f = dtpFrmDate.Value;
            string f1 = f.ToString("dd/MMM/yyyy");
            string t1 = t.ToString("dd/MMM/yyyy");
            

            //txtSearch.Text = "";
            //BindPurInvoicelist();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
             
                //var options = new ExcelExportingOptions();
                //var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
                //var workBook = excelEngine.Excel.Workbooks[0];
                //workBook.SaveAs("Sample.xlsx");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
