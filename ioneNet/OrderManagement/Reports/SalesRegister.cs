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
using Syncfusion.WinForms.DataGridConverter;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.Data;
using Ione_DAL;
namespace ioneNet.OrderManagement.Reports
{
    public partial class SalesRegister : Form
    {
        public static string InvoiceNoList, Order_NoList, SO_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);


        private void dgvRecordList_FilterStringChanged(object sender, EventArgs e)
        {
            //this.salesReportBindingSource.Filter = dgvRecordList.FilterString;
            //decimal x = 0;
            //for (int i = 0; i < dgvRecordList.Rows.Count - 1; i++)
            //{

            //    x += (dgvRecordList.Rows[i].Cells["taxableValueDataGridViewTextBoxColumn"].Value == "" || dgvRecordList.Rows[i].Cells["taxableValueDataGridViewTextBoxColumn"].Value == null || dgvRecordList.Rows[i].Cells["taxableValueDataGridViewTextBoxColumn"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgvRecordList.Rows[i].Cells["taxableValueDataGridViewTextBoxColumn"].Value);

            //}

            //txtTotalValue.Text = x.ToString(".00");

        }

        private void dgvRecordList_SortStringChanged(object sender, EventArgs e)
        {
            //this.salesReportBindingSource.Sort = dgvRecordList.SortString;
        }

        private void approveToolStripMenuItem_Click(object sender, EventArgs e)
        {
           try
           {
                //foreach (DataGridViewRow row in dgvRecordList.Rows)
                //{
                //    bool isSelected = Convert.ToBoolean(row.Cells["SelOrd"].Value);
                //    if (isSelected)
                //    {
                //        SqlCommand cmd = new SqlCommand();
                //        SO_No = row.Cells["sONODataGridViewTextBoxColumn"].Value.ToString();
                //        cmd.CommandText = "Update Sale_Order_Master_New set status = 'Approved' where So_No=@param1 and Company_ID =@compName";
                //        cmd.Parameters.AddWithValue("@param1", SO_No);
                //        cmd.Parameters.AddWithValue("@CompName", logIn.company);
                //        cmd.Connection = con;
                //        con.Open();
                //        cmd.ExecuteNonQuery();
                //        con.Close();
                //        string strT = logIn.username + "-" + DateTime.Now;
                //        cmd.CommandText = "Update Sale_Order_Master_New set Modified_By = @strT where So_No=@param1 and Company_ID =@compName";
                //        cmd.Parameters.AddWithValue("@strT", strT);
                //        cmd.Parameters.AddWithValue("@param1", SO_No);
                //        cmd.Parameters.AddWithValue("@CompName", logIn.company);
                //        cmd.Connection = con;
                //        con.Open();
                //        cmd.ExecuteNonQuery();
                //        con.Close();
                //    }
                    //else
                    //{
                    //    MessageBox.Show("Atlease One So No to be Selected to Approve");
                    //}
                //}
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
                //foreach (DataGridViewRow row in dgvRecordList.Rows)
                //{
                //    bool isSelected = Convert.ToBoolean(row.Cells["SelOrd"].Value);
                //    if (isSelected)
                //    {
                //        SqlCommand cmd = new SqlCommand();
                //        SO_No = row.Cells["sONODataGridViewTextBoxColumn"].Value.ToString();
                //        cmd.CommandText = "Update Sale_Order_Master_New set status = 'Pre_Closed' where So_No=@param1 and Company_ID =@compName";
                //        cmd.Parameters.AddWithValue("@param1", SO_No);
                //        cmd.Parameters.AddWithValue("@CompName", logIn.company);                       
                //        cmd.Connection = con;
                //        con.Open();
                //        cmd.ExecuteNonQuery();
                //        con.Close();
                //        string strT = logIn.username + "-" + DateTime.Now;
                //        cmd.CommandText = "Update Sale_Order_Master_New set Modified_By = @strT where So_No=@param1 and Company_ID =@compName";
                //        cmd.Parameters.AddWithValue("@strT", strT);
                //        cmd.Parameters.AddWithValue("@param1", SO_No);
                //        cmd.Parameters.AddWithValue("@CompName", logIn.company);
                //        cmd.Connection = con;
                //        con.Open();
                //        cmd.ExecuteNonQuery();
                //        con.Close();
                //    }
                //    else
                //    {
                //        MessageBox.Show("Atlease One So No to be Selected to Approve");
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DateTime dt = dtpFrmDate.Value;
            string dt1 = dt.ToString("dd/MM/yyyy");

            DateTime dtt = dtpToDate.Value;
            string dt2 = dtt.ToString("dd/MM/yyyy");
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 3;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:X500"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            workBook.Worksheets[0].Range["A2"].Value = "Sales Register";
            workBook.Worksheets[0].Range["D2"].Value = "Period :" + dt1 + "-" + dt2;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\Sales_Report.xlsx");
            string doc = Fname + "\\Sales_Report.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //foreach (DataGridViewRow row in dgvRecordList.Rows)
                //{
                //    bool isSelected = Convert.ToBoolean(row.Cells["SelOrd"].Value);
                //    if (isSelected)
                //    {
                //        SqlCommand cmd = new SqlCommand();
                //        SO_No = row.Cells["sONODataGridViewTextBoxColumn"].Value.ToString();
                //        cmd.CommandText = "Update Sale_Order_Master_New set status = 'Deleted' where So_No=@param1 and Company_ID =@compName";
                //        cmd.Parameters.AddWithValue("@param1", SO_No);
                //        cmd.Parameters.AddWithValue("@CompName", logIn.company);
                //        cmd.Connection = con;
                //        con.Open();
                //        cmd.ExecuteNonQuery();
                //        con.Close();
                //        string strT = logIn.username + "-" + DateTime.Now;
                //        cmd.CommandText = "Update Sale_Order_Master_New set Modified_By = @strT where So_No=@param1 and Company_ID =@compName";
                //        cmd.Parameters.AddWithValue("@strT", strT);
                //        cmd.Parameters.AddWithValue("@param1", SO_No);
                //        cmd.Parameters.AddWithValue("@CompName", logIn.company);
                //        cmd.Connection = con;
                //        con.Open();
                //        cmd.ExecuteNonQuery();
                //        con.Close();
                //    }
                //    else
                //    {
                //        MessageBox.Show("Atlease One So No to be Selected to Approve");
                //    }
                //}
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
            //salesReportTableAdapter.Fill(ioneDataSet.SalesReport,logIn.company, dtpFrmDate.Value, dtpToDate.Value, null);
            //sfDataGrid1.DataSource = salesReportBindingSource;
            //decimal x = 0;
            //for (int i = 0; i < dgvRecordList.Rows.Count - 1; i++)
            //{

            //    x += (dgvRecordList.Rows[i].Cells["taxableValueDataGridViewTextBoxColumn"].Value == "" || dgvRecordList.Rows[i].Cells["taxableValueDataGridViewTextBoxColumn"].Value == null || dgvRecordList.Rows[i].Cells["taxableValueDataGridViewTextBoxColumn"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgvRecordList.Rows[i].Cells["taxableValueDataGridViewTextBoxColumn"].Value);

            //}

            //txtTotalValue.Text = x.ToString(".00");
        }


        public static Boolean editMode;
        public SalesRegister()
        {
            InitializeComponent();
        }



        public void binddata()
        {
            try
            {
                DateTime dt = dtpFrmDate.Value;
                string dt1 = dt.ToString("yyyy/MM/dd");

                DateTime dtt = dtpToDate.Value;
                string dt2 = dtt.ToString("yyyy/MM/dd");

                //var data = db.Sp_ProductionTunnageReport(AppCode.GlobalAccess.companyName, Convert.ToDateTime(dt1), Convert.ToDateTime(dt2)).ToString();


                SqlCommand cmd2 = new SqlCommand("LoadingSlip_Report", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);
                cmd2.Parameters.AddWithValue("@frmDate", Convert.ToDateTime(dt1));
                cmd2.Parameters.AddWithValue("@Todate", Convert.ToDateTime(dt2));
                cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataSet ds2 = new DataSet();
                DataTable ds2 = new DataTable();
                // da2.Fill(ds2, "x");
                da2.Fill(ds2);
                sfDataGrid1.DataSource = ds2;

                this.sfDataGrid1.TableSummaryRows.Clear();
                //this.sfDataGrid1.GroupSummaryRows.Clear();
                GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                tableSummaryRow1.Name = "TableSummary";
                tableSummaryRow1.ShowSummaryInRow = false;
                tableSummaryRow1.Position = VerticalPosition.Bottom;


                GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                summaryColumn1.Name = "Qty_Loaded";
                summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn1.Format = "{Sum}";
                summaryColumn1.MappingName = "Qty_Loaded";

               

                this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);

                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
            //if (checkBox1.Checked == true)
            //{
                DateTime t = dtpToDate.Value;
                DateTime f = dtpFrmDate.Value;
                string f1 = f.ToString("dd/MMM/yyyy");
                string t1 = t.ToString("dd/MMM/yyyy");
            binddata();
            //    salesReportTableAdapter.Fill(ioneDataSet.SalesReport, logIn.company, dtpFrmDate.Value, dtpToDate.Value, null);
            //    sfDataGrid1.DataSource = salesReportBindingSource;
            ////}
            //else
            //{
            //    DateTime t = dtpToDate.Value;
            //    DateTime f = dtpFrmDate.Value;
            //    string f1 = f.ToString("dd/MMM/yyyy");
            //    string t1 = t.ToString("dd/MMM/yyyy");
            //    orderRegisterTableAdapter.Fill(ioneDataSet.OrderRegister, logIn.company, dtpFrmDate.Value, dtpToDate.Value, null, 1);
            //    dgvRecordList.DataSource = orderRegisterBindingSource;
            //}
            //decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
            //for (int i = 0; i < dgvRecordList.Rows.Count - 1; i++)
            //{

            //    x += (dgvRecordList.Rows[i].Cells["taxableValueDataGridViewTextBoxColumn"].Value == "" || dgvRecordList.Rows[i].Cells["taxableValueDataGridViewTextBoxColumn"].Value == null || dgvRecordList.Rows[i].Cells["taxableValueDataGridViewTextBoxColumn"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgvRecordList.Rows[i].Cells["taxableValueDataGridViewTextBoxColumn"].Value);
              
            //}

            //txtTotalValue.Text = x.ToString(".00");

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            DateTime t = dtpToDate.Value;
            DateTime f = dtpFrmDate.Value;
            string f1 = f.ToString("dd/MMM/yyyy");
            string t1 = t.ToString("dd/MMM/yyyy");
            salesReportTableAdapter.Fill(ioneDataSet.SalesReport, logIn.company, dtpFrmDate.Value, dtpToDate.Value, null);
            sfDataGrid1.DataSource = salesReportBindingSource;

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
                //int i = dgvRecordList.CurrentRow.Index;
                //if (dgvRecordList.Rows[i].Cells["sONODataGridViewTextBoxColumn"].Value.ToString() != "")
                //{
                //    //SO_No = dgvRecordList.Rows[i].Cells["sONODataGridViewTextBoxColumn"].Value.ToString();
                //    //var = "0";
                //    //editMode = true;
                //    //OrderManagement.Transactions.frmNewOrder frm = new frmNewOrder();
                //    ////OrderManagement.Transactions.
                //    //frm.MdiParent = this.MdiParent;
                //    //frm.Show();
                //    //FrmInv.ShowDialog();
                //    //i1 = 0;
                //}
                //else
                //{
                //    MessageBox.Show("Please Select Any One Record");
                //    //i1 = 0;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
