using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using Ione_DAL;
//using Microsoft.Office.Interop.Excel;

namespace ioneNet.MaterialManagement.Transactions
{
    public partial class frmSelectRolls : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static decimal TotQty = 0;
        public Boolean ProdReport = false;
        public frmSelectRolls()
        {
            InitializeComponent();
        }

        private void frmSelectRolls_Load(object sender, EventArgs e)
        {
            try
            {
                
                if (MaterialManagement.Transactions.frmMaterialIssues.DocNo != null )
                {
                    txtGRN_No.Text = MaterialManagement.Transactions.frmMaterialIssues.DocNo;
                    txtItem_Code.Text = MaterialManagement.Transactions.frmMaterialIssues.ItemCode;
                    MaterialManagement.Transactions.frmMaterialIssues.DocNo = null;
                    var dm2 = (from s in db.Bloom_Roll_Wise_Issues
                               where s.Prod_ID == Convert.ToInt32(txtItem_Code.Text) && s.Doc_Ref == txtGRN_No.Text && s.Company_ID == logIn.company && s.BU_ID==logIn.BU_ID
                               select new
                               {
                                   LOT_No = s.RollNo,
                                   Roll_Width = s.RollWidth,
                                   //Qty_Kgs = s.RollWt,
                                   Qty_Kgs = s.RollWt_Stock,
                                   Qty_Issued = s.RollWt_Issued,

                               });

                    SqlCommand cmd3 = (SqlCommand)db.GetCommand(dm2);
                    SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                    DataTable dtr1 = new DataTable();
                    da3.Fill(dtr1);
                    if (dtr1.Rows.Count > 0)
                    {
                        dgProductsList.DataSource = dtr1;
                    }
                    else
                    {
                        if (ProdReport == false)
                        {
                            var dm1 = (from s in db.Bloom_Roll_Wise_Stocks
                                       where s.Prod_ID == Convert.ToInt32(txtItem_Code.Text) && s.QtyStock>0 && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID
                                       select new
                                       {
                                           LOT_No = s.RollNo,
                                           Roll_Width = s.RollWidth,
                                           //Qty_Kgs = s.RollWt,
                                           Qty_Kgs = s.QtyStock,

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
                    }
                }
                else
                {
                    txtGRN_No.Text = ProductionManagement.ProductionReport.DocNo;
                    txtItem_Code.Text = ProductionManagement.ProductionReport.ItemCode;
                    ProductionManagement.ProductionReport.DocNo = "";
                    ProdReport = true;
                    var dm2 = (from s in db.Bloom_Roll_Wise_Productions
                               where s.Prod_ID == Convert.ToInt32(txtItem_Code.Text) && s.Doc_Ref == txtGRN_No.Text && s.Company_ID == logIn.company
                               select new
                               {
                                   LOT_No = s.RollNo,
                                   Roll_Width = s.RollWidth,
                                   //Qty_Kgs = s.RollWt,
                                   Qty_Kgs = s.RollWt_Stock,
                                   Qty_Issued = s.RollWt_Issued,

                               });

                    SqlCommand cmd3 = (SqlCommand)db.GetCommand(dm2);
                    SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                    DataTable dtr1 = new DataTable();
                    da3.Fill(dtr1);
                    if (dtr1.Rows.Count > 0)
                    {
                        dgProductsList.DataSource = dtr1;
                    }
                    else
                    {
                            var dm1 = (from s in db.Bloom_Roll_Pending_For_Productions
                                       where s.Prod_ID == Convert.ToInt32(txtItem_Code.Text) && s.Company_ID == logIn.company
                                       select new
                                       {
                                           LOT_No = s.RollNo,
                                           Roll_Width = s.RollWidth,
                                           //Qty_Kgs = s.RollWt,
                                           Qty_Kgs = s.Roll_Qty_Balance,

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
                    
                }
                decimal x = 0, y = 0;
                for (int i = 0; i < dgProductsList.Rows.Count - 1; i++)
                {

                    x += (dgProductsList.Rows[i].Cells["Qty_Issued"].Value == "" || dgProductsList.Rows[i].Cells["Qty_Issued"].Value == null || dgProductsList.Rows[i].Cells["Qty_Issued"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Qty_Issued"].Value);
                    y = y + 1;
                }
                txtTotalQty.Text = x.ToString("0.00");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (ProdReport == false)
            {
                SaveRollIssueData();
            }
            else
            {
                SaveRollProdData();
            }
        }

        private void dgProductsList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                DataGridViewRow R1 = dgProductsList.Rows[dgProductsList.CurrentRow.Index];
                int columnIndex = dgProductsList.CurrentCell.ColumnIndex;
                string columnName = dgProductsList.Columns[columnIndex].Name;               
                if (columnName == "Qty_Issued")
                {
                    //int Itemcode = Convert.ToInt32(R1.Cells["Item_Code"].Value.ToString());
                    //if (R1.Cells["Item_Description"].Value != null)
                    //{
                        decimal IssuedQty = (R1.Cells["Qty_Issued"].Value == "" || R1.Cells["Qty_Issued"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty_Issued"].Value);
                        decimal StockQty = (R1.Cells["Qty_Kgs"].Value == "" || R1.Cells["Qty_Kgs"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty_Kgs"].Value);
                       
                        if (IssuedQty <= StockQty)
                        {
                            decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
                            for (int i = 0; i < dgProductsList.Rows.Count - 1; i++)
                            {

                                x += (dgProductsList.Rows[i].Cells["Qty_Issued"].Value == "" || dgProductsList.Rows[i].Cells["Qty_Issued"].Value == null || dgProductsList.Rows[i].Cells["Qty_Issued"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Qty_Issued"].Value);
                                y = y + 1;
                            }
                            txtTotalQty.Text = x.ToString("0.00");                        }
                        else
                        {
                            MessageBox.Show("Issued Qty Cannot Be Greater Than Stock Qty");
                            R1.Cells["Qty_Issued"].Value = 0;
                            return;
                        }                       
                    //}

                }

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
            this.Close();
        }

        private void btnSubmit_Click_1(object sender, EventArgs e)
        {
            
        }
        private void SaveRollIssueData()
        {
            try
            {
                String myString = "";
                int Icode = Convert.ToInt32(txtItem_Code.Text);
                myString = txtGRN_No.Text;
                if ((from u in db.Bloom_Roll_Wise_Issues where u.Doc_Ref == myString && u.Prod_ID == Icode && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    myString = txtGRN_No.Text;

                    db.sp_ISSUE_RollData_Delete(myString, Icode, logIn.company);
                }
                else
                {

                    // myString = txtSoNo.Text;

                }
                decimal y, x;
                y = Convert.ToDecimal(txtTotalQty.Text);



                dgProductsList.Enabled = false;
                //db.Transaction = transaction;
                for (int i = 0; i < dgProductsList.RowCount - 1; i++)
                {
                    Bloom_Roll_Wise_Issue SC = new Bloom_Roll_Wise_Issue();

                    SC.Doc_Ref = myString;
                    SC.Prod_ID = Convert.ToInt32(txtItem_Code.Text);
                    SC.TotalWt = Convert.ToDecimal(txtTotalQty.Text);
                    SC.RollNo = (dgProductsList.Rows[i].Cells["LOT_No"].Value == null) ? "" : (dgProductsList.Rows[i].Cells["LOT_No"].Value).ToString();
                    SC.RollWidth = (dgProductsList.Rows[i].Cells["Roll_Width"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Roll_Width"].Value);
                    //SC.Con_GSTINNo = (dgProductsList.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProductsList.Rows[i].Cells["Remarks"].Value).ToString();
                    SC.RollWt_Stock = (dgProductsList.Rows[i].Cells["Qty_Kgs"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Qty_Kgs"].Value);
                    SC.RollWt_Issued = (dgProductsList.Rows[i].Cells["Qty_Issued"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Qty_Issued"].Value);
                    SC.Status = "Open";

                    SC.Company_ID = logIn.company;
                    SC.BU_ID = logIn.BU_ID;
                    db.Bloom_Roll_Wise_Issues.InsertOnSubmit(SC);
                }
                db.SubmitChanges();
                //transaction.Commit();               
                MessageBox.Show("Roll Wise / Batch Issue Details Updated Sucessfully");
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
       private void SaveRollProdData()
        {
            try
            {
                String myString = "";
                int Icode = Convert.ToInt32(txtItem_Code.Text);
                myString = txtGRN_No.Text;
                if ((from u in db.Bloom_Roll_Wise_Productions where u.Doc_Ref == myString && u.Prod_ID == Icode && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    myString = txtGRN_No.Text;

                    db.sp_ISSUE_RollData_Delete(myString, Icode, logIn.company);
                }
                else
                {

                    // myString = txtSoNo.Text;

                }
                decimal y, x;
                y = Convert.ToDecimal(txtTotalQty.Text);



                dgProductsList.Enabled = false;
                //db.Transaction = transaction;
                for (int i = 0; i < dgProductsList.RowCount - 1; i++)
                {
                    Bloom_Roll_Wise_Production SC = new Bloom_Roll_Wise_Production();

                    SC.Doc_Ref = myString;
                    SC.Prod_ID = Convert.ToInt32(txtItem_Code.Text);
                    SC.TotalWt = Convert.ToDecimal(txtTotalQty.Text);
                    SC.RollNo = (dgProductsList.Rows[i].Cells["LOT_No"].Value == null) ? "" : (dgProductsList.Rows[i].Cells["LOT_No"].Value).ToString();
                    SC.RollWidth = (dgProductsList.Rows[i].Cells["Roll_Width"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Roll_Width"].Value);
                    //SC.Con_GSTINNo = (dgProductsList.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProductsList.Rows[i].Cells["Remarks"].Value).ToString();
                    SC.RollWt_Stock = (dgProductsList.Rows[i].Cells["Qty_Kgs"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Qty_Kgs"].Value);
                    SC.RollWt_Issued = (dgProductsList.Rows[i].Cells["Qty_Issued"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Qty_Issued"].Value);
                    SC.Status = "Open";
                    SC.BU_ID = logIn.BU_ID;
                    SC.Company_ID = logIn.company;
                    db.Bloom_Roll_Wise_Productions.InsertOnSubmit(SC);
                }
                db.SubmitChanges();
                //transaction.Commit();               
                MessageBox.Show("Roll Wise / Batch Production Details Updated Sucessfully");
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

        private void frmSelectRolls_FormClosed(object sender, FormClosedEventArgs e)
        {
            TotQty = Convert.ToDecimal(txtTotalQty.Text);
        }
    }
}
