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
    public partial class StockLedger_SubContracto : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();

        public StockLedger_SubContracto()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            try
            {
                binddata();

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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


                SqlCommand cmd2 = new SqlCommand("StockLedger_SubContractor", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);               
                cmd2.Parameters.AddWithValue("@Fromdate", Convert.ToDateTime(dt1));
                cmd2.Parameters.AddWithValue("@Todate", Convert.ToDateTime(dt2));
                cmd2.Parameters.AddWithValue("@SubContractor", Convert.ToInt32(cmbSubContractor.SelectedValue.ToString()));
                cmd2.Parameters.AddWithValue("@prodname", Convert.ToInt32(comboBox1.SelectedValue.ToString()));
                cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataSet ds2 = new DataSet();
                DataTable ds2 = new DataTable();
                // da2.Fill(ds2, "x");
                da2.Fill(ds2);
                dataGridView1.DataSource = ds2;
                double OPQty = 0, RecQty = 0, IssQty = 0;
                decimal rec, iss, op, closing, recv, issv, opv, closingv = 0;
               for (int i = 1; i < dataGridView1.Rows.Count - 1; i++)
                {
                    
                    op = ((dataGridView1.Rows[i - 1].Cells["ClosingQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i - 1].Cells["ClosingQty"].Value));
                    rec = (dataGridView1.Rows[i].Cells["ReceiptQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i].Cells["ReceiptQty"].Value);
                    iss = (dataGridView1.Rows[i].Cells["IssuedQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i].Cells["IssuedQty"].Value);

                    opv = ((dataGridView1.Rows[i - 1].Cells["ClosingValue"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i - 1].Cells["ClosingValue"].Value));
                    recv = (dataGridView1.Rows[i].Cells["ReceiptValue"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i].Cells["ReceiptValue"].Value);
                    issv = (dataGridView1.Rows[i].Cells["IssueValue"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dataGridView1.Rows[i].Cells["IssueValue"].Value);


                    closing = op + rec - iss;
                    closingv = opv + recv - issv;

                    dataGridView1.Rows[i].Cells["ClosingQty"].Value = closing;
                    dataGridView1.Rows[i].Cells["ClosingValue"].Value = closingv;
                }

                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void sfDataGrid_QueryCellStyle(object sender, Syncfusion.WinForms.DataGrid.Events.QueryCellStyleEventArgs e)
        {
            //var rowColumnIndex = new RowColumnIndex(e.RowIndex, e.ColumnIndex);
            //if (colorDict.ContainsKey(rowColumnIndex))
            //    e.Style.BackColor = colorDict[rowColumnIndex];
        }


        void SetCellBackgroundColor(RowColumnIndex rowColumnIndex, Color color)
        {
            //if (!colorDict.ContainsKey(rowColumnIndex))
            //    colorDict.Add(rowColumnIndex, color);
            //else
            //    colorDict[rowColumnIndex] = color;
            //sfDataGrid1.TableControl.Invalidate(this.sfDataGrid1.TableControl.GetCellRectangle(rowColumnIndex.RowIndex, rowColumnIndex.ColumnIndex, false));
        }

        private void StockLedger_Load(object sender, EventArgs e)
        {
            dtpFrmDate.MinDate = logIn.fy_Start_Date;
            dtpToDate.MaxDate = logIn.fy_End_Date;
            var d = (from po in db.Products
                    
                     where po.Company_ID == logIn.company //&& A.GroupType == "Expenses"
                     select new { po.prod_ID, po.Prod_Name }).Distinct().ToList();
            if (d.Count > 0)
            {
                comboBox1.DataSource = d;
                comboBox1.ValueMember = "prod_ID";
                comboBox1.DisplayMember = "Prod_Name";
            }

            var d1 = (from po in db.Supplier_informations

                     where po.Company_ID == logIn.company && po.Supplier_Category ==32
                      select new { po.ID, po.Supplier_Name }).Distinct().ToList();
            if (d1.Count > 0)
            {
                cmbSubContractor.DataSource = d1;
                cmbSubContractor.ValueMember = "ID";
                cmbSubContractor.DisplayMember = "Supplier_Name";
            }

            if (MaterialManagement.Reports.StockStatement_SubContractor.prodcode > 0)
            {

                dtpFrmDate.Value = logIn.fy_Start_Date;
                dtpToDate.Value = MaterialManagement.Reports.StockStatement_SubContractor.toDate;
                cmbSubContractor.SelectedValue = MaterialManagement.Reports.StockStatement_SubContractor.subcontractor;
                comboBox1.SelectedValue = MaterialManagement.Reports.StockStatement_SubContractor.prodcode;
                btnView_Click(sender, e);
            }
        }
    }
}
