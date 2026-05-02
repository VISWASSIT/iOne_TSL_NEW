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
namespace ioneNet.OrderManagement.Reports
{
    public partial class Forge_OrderStatusReport : Form
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
            workBook.Worksheets[0].Range["A2"].Value = "Order Status Report";
            workBook.Worksheets[0].Range["D2"].Value = "Period :" + dt1 + "-" + dt2;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\Order_Status_Report.xlsx");
            string doc = Fname + "\\Order_Status_Report.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
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

        private void ListOfOrders_Load(object sender, EventArgs e)
        {
            DateTime t = dtpToDate.Value;
            DateTime f = dtpFrmDate.Value;
            string f1 = f.ToString("dd/MMM/yyyy");
            string t1 = t.ToString("dd/MMM/yyyy");

          
        }


        public static Boolean editMode;
        public Forge_OrderStatusReport()
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
                DateTime dt = dtpFrmDate.Value;
                string dt1 = dt.ToString("yyyy/MM/dd");

                DateTime dtt = dtpToDate.Value;
                string dt2 = dtt.ToString("yyyy/MM/dd");
                SqlCommand cmd2 = new SqlCommand("Sp_Forging_OrderStatus", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);                
                cmd2.Parameters.AddWithValue("@frmDate", Convert.ToDateTime(dt1));
                cmd2.Parameters.AddWithValue("@Todate", Convert.ToDateTime(dt2));
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataSet ds2 = new DataSet();
                DataTable ds2 = new DataTable();
                // da2.Fill(ds2, "x");
                da2.Fill(ds2);
                sfDataGrid1.DataSource = ds2;




                //var d = (from data in db.Sp_Forging_OrderStatus(logIn.company, dtpFrmDate.Value, dtpToDate.Value) select data).ToList();
                //if (d.Count > 0)
                //{
                //    //dgProductsList.DataSource = d;
                //    sfDataGrid1.DataSource = d;
                //}
                
               
                //GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                //tableSummaryRow1.Name = "TableSummary";
                //tableSummaryRow1.ShowSummaryInRow = false;
                //tableSummaryRow1.Position = VerticalPosition.Bottom;

                //GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                //summaryColumn1.Name = "Total Order Value";
                //summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                //summaryColumn1.Format = "Total : {Sum}";
                //summaryColumn1.MappingName = "Taxable_Value";

                //tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

                //this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);
           
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
