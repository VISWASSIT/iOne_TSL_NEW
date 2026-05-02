using Ione_DAL;
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
using Ione_DAL;

namespace ioneNet.MaterialManagement.Transactions
{
    public partial class frmARNWiseData : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static decimal TotQty;
        public frmARNWiseData()
        {
            InitializeComponent();
        }

        private void frmARNWiseData_Load(object sender, EventArgs e)
        {
            try
            {
                if (GlobalVariables.FormName == "GRN")

                {
                    if (MaterialManagement.Transactions.GoodsReceiptNote.SONo != null && MaterialManagement.Transactions.GoodsReceiptNote.SONo != "OB")
                    {
                        txtGRN_No.Text = MaterialManagement.Transactions.GoodsReceiptNote.SONo;
                        txtItem_Code.Text = MaterialManagement.Transactions.GoodsReceiptNote.ItemCode;
                        textBox1.Text = MaterialManagement.Transactions.GoodsReceiptNote.RecQty;                        
                    }
                }
                if (GlobalVariables.FormName == "OpeningStock")
                {
                    txtGRN_No.Text = MaterialManagement.Masters.frmOpeningStock.SONo;
                    txtItem_Code.Text = MaterialManagement.Masters.frmOpeningStock.ItemCode;
                    textBox1.Text = MaterialManagement.Masters.frmOpeningStock.RecQty;
                }
                if (GlobalVariables.FormName == "ProdReport")
                {
                    txtGRN_No.Text = ProductionManagement.ProductionReport.DocNo;
                    txtItem_Code.Text = ProductionManagement.ProductionReport.ItemCode;
                    textBox1.Text = ProductionManagement.ProductionReport.RecQty;
                }
                if (logIn.company == 1044)
                {
                    var dm1 = (from s in db.Bloom_Roll_Wise_Receipts
                               where s.Grn_ID == txtGRN_No.Text && s.Prod_ID == Convert.ToInt32(txtItem_Code.Text) && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID
                               select new
                               {
                                   BatchNo = s.SupplierName,
                                   LOT_No = s.RollNo,
                                   Qty_Kgs = s.RollWt,
                                   Re_Test_Date = s.Re_Test_Date

                               }); ;
                    SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    DataTable dtr = new DataTable();
                    da2.Fill(dtr);
                    if (dtr.Rows.Count >= 0)
                    {
                        dgProductsList.DataSource = dtr;
                    }
                }
                else
                {
                    var dm1 = (from s in db.Bloom_Roll_Wise_Receipts
                               where s.Grn_ID == txtGRN_No.Text && s.Prod_ID == Convert.ToInt32(txtItem_Code.Text) && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID
                               select new
                               {
                                   LOT_No = s.RollNo,
                                   Roll_Width = s.RollWidth,
                                   Qty_Kgs = s.RollWt,
                                   Qty_Mtrs = s.QtyinSqMtrs,

                               });
                    SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    DataTable dtr = new DataTable();
                    da2.Fill(dtr);
                    if (dtr.Rows.Count >= 0)
                    {
                        dgProductsList.DataSource = dtr;
                    }
                }
                decimal x = 0, n = 0;
                for (int i = 0; i < dgProductsList.Rows.Count - 1; i++)
                {

                    x += (dgProductsList.Rows[i].Cells["Qty_Kgs"].Value == "" || dgProductsList.Rows[i].Cells["Qty_Kgs"].Value == null || dgProductsList.Rows[i].Cells["Qty_Kgs"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Qty_Kgs"].Value);
                    n += 1;

                }
                txtTotalQty.Text = x.ToString(".00");
                //textBox2.Text = n.ToString();
                //txtQty.Text = MaterialManagement.Transactions.GoodsReceiptNote.RecQty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgProductsList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow R1 = dgProductsList.Rows[dgProductsList.CurrentRow.Index];
            int columnIndex = dgProductsList.CurrentCell.ColumnIndex;
            string columnName = dgProductsList.Columns[columnIndex].Name;
            if (columnName == "Qty_Kgs")
            {
                decimal x = 0;
                decimal r = 0;
                for (int i = 0; i < dgProductsList.Rows.Count - 1; i++)
                {

                    x += (dgProductsList.Rows[i].Cells["Qty_Kgs"].Value == "" || dgProductsList.Rows[i].Cells["Qty_Kgs"].Value == null || dgProductsList.Rows[i].Cells["Qty_Kgs"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Qty_Kgs"].Value);
                    r = r + 1;

                }
                txtTotalQty.Text = x.ToString("0.00");                
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                String myString = "";
                int Icode = Convert.ToInt32(txtItem_Code.Text);
                myString = txtGRN_No.Text;
                //if (Convert.ToDecimal(textBox1.Text) != Convert.ToDecimal(textBox2.Text))
                //{
                //    MessageBox.Show("Data for All the Rolls Not Entered, Cannot Be Saved");
                //    return;
                //}
                //else
                //{
                    if ((from u in db.Bloom_Roll_Wise_Receipts where u.Grn_ID == myString && u.Prod_ID == Icode && u.Company_ID == logIn.company select u).Count() > 0)
                    {
                        myString = txtGRN_No.Text;

                        db.sp_GRN_RollData_Delete(myString, Icode, logIn.company);
                    }
                    else
                    {

                        // myString = txtSoNo.Text;

                    }
                    decimal y, x;
                    y = Convert.ToDecimal(txtTotalQty.Text);



                    dgProductsList.Enabled = false;
                    Boolean NewRec = true;
                    DateTime retestdate;
                    var gstno = (from c in db.GoodsReceiptNote_Masters
                                 where c.Grn_NO == myString
                                 select new { c.Id }).ToList();
                    if (gstno.Count > 0)
                    {
                        NewRec = false;
                    }
                    //db.Transaction = transaction;
                    for (int i = 0; i < dgProductsList.RowCount - 1; i++)
                    {
                        Bloom_Roll_Wise_Receipt SC = new Bloom_Roll_Wise_Receipt();

                        SC.Grn_ID = myString;
                        SC.Prod_ID = Convert.ToInt32(txtItem_Code.Text);
                        SC.TotalWt = Convert.ToDecimal(txtTotalQty.Text);
                        SC.RollNo = (dgProductsList.Rows[i].Cells["LOT_No"].Value == null) ? "" : (dgProductsList.Rows[i].Cells["LOT_No"].Value).ToString();
                        //SC.RollWidth = (dgProductsList.Rows[i].Cells["BatchNo"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Roll_Width"].Value);
                        //SC.Con_GSTINNo = (dgProductsList.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProductsList.Rows[i].Cells["Remarks"].Value).ToString();
                        SC.RollWt = (dgProductsList.Rows[i].Cells["Qty_Kgs"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Qty_Kgs"].Value);
                        //SC.QtyinSqMtrs = (dgProductsList.Rows[i].Cells["Qty_Mtrs"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Qty_Mtrs"].Value);
                        SC.SupplierName = (dgProductsList.Rows[i].Cells["BatchNo"].Value == null) ? "" : (dgProductsList.Rows[i].Cells["BatchNo"].Value).ToString();
                         if (dgProductsList.Rows[i].Cells["Re_Test_Date"].Value == DBNull.Value)
                        {
                        SC.Re_Test_Date = null;
                        }
                         else
                        {
                        retestdate = Convert.ToDateTime(dgProductsList.Rows[i].Cells["Re_Test_Date"].Value);
                        SC.Re_Test_Date = retestdate;
                        }
                    
                    if (NewRec == false)
                        {
                            SC.Status = "Closed";
                        }
                        else
                        {
                            SC.Status = "Open";
                        }

                        SC.Company_ID = logIn.company;

                        SC.BU_ID = logIn.BU_ID;

                        db.Bloom_Roll_Wise_Receipts.InsertOnSubmit(SC);
                    }
                    db.SubmitChanges();
                    //transaction.Commit();               
                    MessageBox.Show("ARN Wise Details Updated Sucessfully");
                    TotQty = Convert.ToDecimal(txtTotalQty.Text);
                    this.Close();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            //TotQty = Convert.ToDecimal(txtTotalQty.Text);
            this.Close();
        }
    }
}
