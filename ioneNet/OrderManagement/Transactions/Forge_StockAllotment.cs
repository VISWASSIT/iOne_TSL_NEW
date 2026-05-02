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
using System.Globalization;
using Ione_DAL;
using ioneNet.OrderManagement.Reports;
namespace ioneNet.OrderManagement.Transactions
{
    public partial class Forge_StockAllotment : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public Forge_StockAllotment()
        {
            InitializeComponent();
        }

        private void Forge_StockAllotment_Load(object sender, EventArgs e)
        {
            try
            {
                var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company && m.Status == 1 select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    CmbBuyerName.DataSource = Buyerblind;
                    CmbBuyerName.ValueMember = "ID";
                    CmbBuyerName.DisplayMember = "Supplier_Name";

                }
                //if (CmbBuyerName.Items.Count > 0)
                CmbBuyerName.SelectedIndex = -1;

                txtSoNo.Text = OrderRegister.SO_No;
                txtSoNo_Leave(sender, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtSoNo_Leave(object sender, EventArgs e)
        {
            try
            {
                txtSoNo.Text = txtSoNo.Text;
               
                    String myString = "";
                    int Ord_id = 0;
                    myString = txtSoNo.Text;
                    var da = (from obj in db.Sale_Order_Masters
                              where obj.SO_NO == txtSoNo.Text && obj.Company_ID == logIn.company
                              select obj).ToList();

                    if (da.Count > 0)
                    {
                        Ord_id = da[0].Id;
                        CmbBuyerName.SelectedValue = da[0].BuyerName;

                    }
                    if ((from u in db.Forging_Sale_Order_Stock_Allotements where u.SO_NO == myString && u.Company_ID == logIn.company select u).Count() > 0)
                    {
                        var dm1 = (from s in db.Forging_Sale_Order_Stock_Allotements
                                   join q in db.Sale_Order_Childs on new { X1 = s.SO_NO, X2 = Convert.ToInt32(s.Prod_Code) } equals new { X1 = q.SO_NO, X2 = Convert.ToInt32(q.Prod_Code) }
                                   join p in db.Products on Convert.ToInt32(s.Prod_Code) equals p.prod_ID
                                   join u in db.UoM_Masters on p.Prod_Primary_UOM_Id equals u.UOM_ID
                                   where s.SO_NO == myString && s.Company_ID == logIn.company


                                   select new

                                   {
                                       Item_Code = s.Prod_Code,
                                       Item_Description = p.Prod_Name,
                                       UOM = u.Uom_Descr,
                                       q.Prod_Grade,                                       
                                       s.Qty,
                                       Qty_Avbl = s.Stock_Qty,                                       
                                       Alloted_Qty= s.Qty_Alloted,
                                       s.Remarks
                                   });




                        SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        DataTable dtr = new DataTable();
                        da2.Fill(dtr);
                        if (dtr.Rows.Count >= 0)
                            dgProducts.DataSource = dtr;
                    }
                    else
                    {

                    DateTime dt = DateTime.Now;
                    string dt1 = dt.ToString("yyyy/MM/dd");

                    //DateTime dtt = dtpToDate.Value;
                    //string dt2 = dtt.ToString("yyyy/MM/dd");

                    SqlCommand cmd2 = new SqlCommand("ShowStockForAllotment", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@compname", logIn.company);
                    cmd2.Parameters.AddWithValue("@fromDate", Convert.ToDateTime(dt1));
                    cmd2.Parameters.AddWithValue("@toDate", Convert.ToDateTime(dt1));
                    cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
                    cmd2.Parameters.AddWithValue("@so_id", Convert.ToInt32(Ord_id));

                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    //DataSet ds2 = new DataSet();
                    DataTable ds2 = new DataTable();
                    // da2.Fill(ds2, "x");
                    da2.Fill(ds2);
                    dgProducts.DataSource = ds2;



                    //var dm1 = (from s in db.Sale_Order_Childs
                    //           join p in db.Products on Convert.ToInt32(s.Prod_Code) equals p.prod_ID
                    //           join u in db.UoM_Masters on p.Prod_Primary_UOM_Id equals u.UOM_ID
                    //           where s.So_Master_ID == Ord_id && s.Company_ID == logIn.company


                    //           select new

                    //               {
                    //                   SO_Item_No= s.enq_item_no,
                    //                   Item_Code = s.Prod_Code,
                    //                   Item_Description = p.Prod_Name,
                    //                   UOM = u.Uom_Descr,
                    //                   s.Prod_Grade,
                    //                   s.Prod_Length,
                    //                   s.Qty,
                    //                   s.Remarks
                    //               });




                    //    SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                    //    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    //    DataTable dtr = new DataTable();
                    //    da2.Fill(dtr);
                    //    if (dtr.Rows.Count >= 0)
                    //        dgProducts.DataSource = dtr;
                        


                    }                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgProducts_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].HeaderText;
                TextBox tb3 = e.Control as TextBox;
                tb3.AutoCompleteMode = AutoCompleteMode.None;
                if (tb3 != null && columnName == "Grade")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }

                if (tb3 != null && columnName == "Item Description")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void addItems(AutoCompleteStringCollection coll)
        {
            try
            {
                DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];

                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].HeaderText;               
               

                if (columnName == "Item Description")
                {
                    var Prodname = (from d in db.Sale_Order_Childs where d.SO_NO == txtSoNo.Text && d.Company_ID == logIn.company select new { d.Product_Description }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Product_Description");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Product_Description);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
                    }
                }
                if (columnName == "Grade")
                {
                    var Prodname = (from d in db.Sale_Order_Childs where d.SO_NO == txtSoNo.Text && d.Company_ID == logIn.company select new { d.Product_Description }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Grade");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Product_Description);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgProducts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                            
                DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].Name;
                string prodcode = R1.Cells["Remarks"].Value.ToString();
                string prodname = R1.Cells["Item_Description"].Value.ToString();
                if (columnName == "Item_Description")
                {
                    var getProductName = (from s in db.Sale_Order_Childs where s.Product_Description == prodname && s.SO_NO == txtSoNo.Text
                                          select new { s.Prod_Code, s.Remarks, s.Uom, s.Qty }).FirstOrDefault();

                    if (getProductName != null)
                    {
                        R1.Cells["UOM"].Value = getProductName.Uom.ToString();
                        R1.Cells["Item_Code"].Value = getProductName.Prod_Code.ToString();
                        R1.Cells["Remarks"].Value = getProductName.Remarks.ToString();
                        R1.Cells["Qty"].Value = getProductName.Qty.ToString();

                    }
                }
                
                if (columnName == "Alloted_Qty")
                {
                    decimal JCQty = Convert.ToDecimal(R1.Cells["Qty"].Value);
                    decimal AvblQty = Convert.ToDecimal(R1.Cells["Qty_Avbl"].Value);
                    decimal AllotQty = Convert.ToDecimal(R1.Cells["Alloted_Qty"].Value);
                    if (AllotQty > AvblQty || AllotQty > JCQty)
                    {
                        MessageBox.Show("Qty Alloted Cannot be Greterthan Job Card Qty or Available Qty");
                        R1.Cells["Alloted_Qty"].Value = "0.00";


                    }
                    else
                    {

                    }
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

        private void btnSave_Click(object sender, EventArgs e)
        {


            try
            {
                if (CmbBuyerName.Text == string.Empty)
                {
                    MessageBox.Show("Customer Name Should Not Be Empty", "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CmbBuyerName.Focus();
                    return;
                }
                else if (txtSoNo.Text == string.Empty)
                {
                    MessageBox.Show("Enter SO No", "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtSoNo.Focus();
                    return;
                }                
                else
                {
                    Save();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void Save()
        {
            try
            {
                String myString = "";
                myString = txtSoNo.Text;
                if ((from u in db.Forging_Sale_Order_Stock_Allotements where u.SO_NO == myString && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    var SC = db.Forging_Sale_Order_Stock_Allotements.Where(w => w.SO_NO == myString && w.Company_ID == logIn.company).FirstOrDefault();
                    for (int i = 0; i < dgProducts.RowCount - 1; i++)
                    {
                        SC.SO_NO = myString;
                        //SC.Buyer_Name = CmbBuyerName.Text;
                        SC.Prod_Code = (dgProducts.Rows[i].Cells["Item_Code"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Code"].Value).ToString();
                        //SC.SO_Item_No = Convert.ToInt32((dgProducts.Rows[i].Cells["SO_Item_No"].Value).ToString());
                        var S = (from a in db.QA_Mtrl_Grade_Masters
                                 where a.Company_ID == logIn.company && a.Material_Grade == (dgProducts.Rows[i].Cells["Prod_Grade"].Value).ToString()
                                 select new { a.id }).ToList();


                        SC.Prod_Grade = S[0].id;
                        double amt = Convert.ToDouble(dgProducts.Rows[i].Cells["Qty"].Value);
                        SC.Qty = amt;
                        SC.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value).ToString();
                      
                        SC.Stock_Qty = (dgProducts.Rows[i].Cells["Qty_Avbl"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty_Avbl"].Value);
                        SC.Qty_Alloted = (dgProducts.Rows[i].Cells["Alloted_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Alloted_Qty"].Value);
                        
                        SC.Company_ID = logIn.company;
                        db.SubmitChanges();
                    }
                    MessageBox.Show("Record Updated Successfully");
                    //myString = txtSoNo.Text;
                    //db.sp_SO_Delete(myString, logIn.company);
                }
                else
                {
                    for (int i = 0; i < dgProducts.RowCount - 1; i++)
                    {
                        Forging_Sale_Order_Stock_Allotement SC = new Forging_Sale_Order_Stock_Allotement();
                        SC.SO_NO = myString;
                        //SC.Buyer_Name = CmbBuyerName.Text;
                        SC.Prod_Code = (dgProducts.Rows[i].Cells["Item_Code"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Code"].Value).ToString();
                        //SC.SO_Item_No = Convert.ToInt32((dgProducts.Rows[i].Cells["SO_Item_No"].Value).ToString());
                        var S = (from a in db.QA_Mtrl_Grade_Masters
                                 where a.Company_ID == logIn.company && a.Material_Grade == (dgProducts.Rows[i].Cells["Prod_Grade"].Value).ToString()
                                 select new { a.id }).ToList();


                        SC.Prod_Grade = S[0].id;

                        double amt = Convert.ToDouble(dgProducts.Rows[i].Cells["Qty"].Value);
                        SC.Qty = amt;
                        SC.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value).ToString();

                        SC.Stock_Qty = (dgProducts.Rows[i].Cells["Qty_Avbl"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty_Avbl"].Value);
                        SC.Qty_Alloted = (dgProducts.Rows[i].Cells["Alloted_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Alloted_Qty"].Value);

                        SC.Company_ID = logIn.company;
                        db.Forging_Sale_Order_Stock_Allotements.InsertOnSubmit(SC);
                    }
                    db.SubmitChanges();               
                     MessageBox.Show("Record Saved / Updated Successfully");
                   
                }
                //if (frmGate.Modify.Contains(this.Text))
                //{
                this.Close();


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        private void txtSoNo_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
