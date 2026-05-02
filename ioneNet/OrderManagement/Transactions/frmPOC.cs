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

namespace ioneNet.OrderManagement.Transactions
{
    public partial class frmPOC : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();
        public static string SONo, ItemCode, OrdQty;
        decimal taxRate = 0;
        decimal cgstPer, sgstPer, igstPer;
        double p = 0.00, a = 0.00;

        private void txtrepNoa_Leave(object sender, EventArgs e)
        {
            POC_d();
        }

        private void dgrmconsumed_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                if (txtrepNoa.Text == "")
                {
                    MessageBox.Show("Enter POC NO");
                }
                else
                   if (CmbsectionrollName.Text == "")
                {
                    MessageBox.Show("Customer Name to be selected");
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

        private void dgrmconsumed_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int columnIndex1 = dgrmconsumed.CurrentCell.ColumnIndex;
                string columnName1 = dgrmconsumed.Columns[columnIndex1].HeaderText;
                TextBox tb1 = e.Control as TextBox;
                tb1.AutoCompleteCustomSource = null;
                if (tb1 != null && columnName1 == "Product Name")
                {
                    tb1.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb1.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb1.AutoCompleteCustomSource = DataColl;

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
                


                DataGridViewRow R2 = dgrmconsumed.Rows[dgrmconsumed.CurrentRow.Index];

                int columnIndex1 = dgrmconsumed.CurrentCell.ColumnIndex;
                string columnName1 = dgrmconsumed.Columns[columnIndex1].HeaderText;
                if (columnName1 == "Product Name")
                {
                    var Prodname = (from a in db.Products
                                    join u in db.UoM_Masters on a.Prod_Primary_UOM_Id equals u.UOM_ID
                                    join g in db.Product_Groups on a.Prod_Group_Id equals g.ID
                                    where a.Company_ID == logIn.company /*&& a.Prod_Type_Id == 140*/
                                    select new { a.Prod_Name, a.prod_ID }).ToList();
                    DataTable dt = new DataTable();


                    dt.Columns.Add("Prod_Name");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Prod_Name);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
                    }


                }
            }
            catch (Exception ex)
            {
            }
        }

        private void dgrmconsumed_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                
                if (e.KeyCode == Keys.F6) //Delivery Information / Locations
                {
                    if (dgrmconsumed.Rows.Count > 0)
                    {

                        foreach (DataGridViewCell oneCell in dgrmconsumed.SelectedCells)
                        {
                            //ask for permission
                            if (oneCell.Selected)
                            {
                                DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                                if (result == DialogResult.Yes)
                                {
                                    //int i = dgrmconsumed.CurrentCell.RowIndex;
                                    //SONo = txtrepNoa.Text;
                                    //ItemCode = dgrmconsumed.Rows[i].Cells["Item_Code"].Value.ToString();
                                    //SqlCommand cmd1 = new SqlCommand("delete  from [Invoice_Child] where [Prod_Code] =@ProdID and [Inv_No] = @pono", con);
                                    //cmd1.Parameters.AddWithValue("@ProdID", ItemCode);
                                    //cmd1.Parameters.AddWithValue("@pono", SONo);
                                    //if (con.State != ConnectionState.Open)
                                    //    con.Open();
                                    ////con.Open();
                                    //cmd1.ExecuteNonQuery();
                                    //con.Close();
                                    //if (oneCell.Selected)
                                    //    dgrmconsumed.Rows.RemoveAt(oneCell.RowIndex);
                                    //break;
                                }
                            }
                        }
                        //}
                        //decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
                        //for (int i = 0; i < dgrmconsumed.Rows.Count - 1; i++)
                        //{

                        //    x += (dgrmconsumed.Rows[i].Cells["Inv_Qty"].Value == "" || dgrmconsumed.Rows[i].Cells["Inv_Qty"].Value == null || dgrmconsumed.Rows[i].Cells["Inv_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["Inv_Qty"].Value);
                        //    y += (dgrmconsumed.Rows[i].Cells["Amt_Before_Disc"].Value == "" || dgrmconsumed.Rows[i].Cells["Amt_Before_Disc"].Value == null || dgrmconsumed.Rows[i].Cells["Amt_Before_Disc"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["Amt_Before_Disc"].Value);
                        //    q += (dgrmconsumed.Rows[i].Cells["Disc_Amt"].Value == "" || dgrmconsumed.Rows[i].Cells["Disc_Amt"].Value == null || dgrmconsumed.Rows[i].Cells["Disc_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["Disc_Amt"].Value);
                        //    v += (dgrmconsumed.Rows[i].Cells["Taxable_Value"].Value == "" || dgrmconsumed.Rows[i].Cells["Taxable_Value"].Value == null || dgrmconsumed.Rows[i].Cells["Taxable_Value"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["Taxable_Value"].Value);
                        //    cg += (dgrmconsumed.Rows[i].Cells["CGST_Amt"].Value == "" || dgrmconsumed.Rows[i].Cells["CGST_Amt"].Value == null || dgrmconsumed.Rows[i].Cells["CGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["CGST_Amt"].Value);
                        //    sg += (dgrmconsumed.Rows[i].Cells["SGST_Amt"].Value == "" || dgrmconsumed.Rows[i].Cells["SGST_Amt"].Value == null || dgrmconsumed.Rows[i].Cells["SGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["SGST_Amt"].Value);
                        //    ig += (dgrmconsumed.Rows[i].Cells["IGST_Amt"].Value == "" || dgrmconsumed.Rows[i].Cells["IGST_Amt"].Value == null || dgrmconsumed.Rows[i].Cells["IGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["IGST_Amt"].Value);
                        //    totA += (dgrmconsumed.Rows[i].Cells["Total_Amount"].Value == "" || dgrmconsumed.Rows[i].Cells["Total_Amount"].Value == null || dgrmconsumed.Rows[i].Cells["Total_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["Total_Amount"].Value);

                        //}

                        //txtTotalQty.Text = x.ToString(".00");
                        //txtSubTotal.Text = y.ToString("0.00");
                        //txtTotDiscount.Text = q.ToString(".00");
                       

                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                
                //Boolean recval = false;
                //for (int i = 0; i < dgrmconsumed.RowCount - 1; i++)
                //{
                //    if (dgrmconsumed.Rows[i].Cells["Item_Code"].Value != null)
                //    {
                //        double amt = Convert.ToDouble(dgrmconsumed.Rows[i].Cells["Inv_Qty"].Value);
                //        if (amt > 0)
                //        {
                //            recval = true;
                //        }
                //        else
                //        {
                //            recval = false;
                //        }

                //    }
                //}

                if (CmbsectionrollName.Text == string.Empty)
                {
                    MessageBox.Show("Customer Name Should Not Be Empty", "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CmbsectionrollName.Focus();
                    return;
                }

                else if (txtrepNoa.Text == string.Empty)
                {
                    MessageBox.Show("Customer PO No Should Not Be Empty");
                    txtrepNoa.Focus();
                    return;
                }

                else if (txtrepNoa.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Status");
                    txtrepNoa.Focus();
                    return;
                }



                else if (dgrmconsumed.Rows[0].Cells["Prod_Name"].Value == null)
                {
                    MessageBox.Show("Atleast One Record To Be Entered To Proceed", "Order Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //else if (recval == false)
                //{
                //    MessageBox.Show("Delivery Qty Should Be Greater Than 0 for all the products", "Order Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

                else
                {
                    SaveNew_Sql_proc();
                    // Save();

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void SaveNew_Sql_proc()
        {
            try
            {
                String myString = "";
                if (logIn.company == 25 || logIn.company == 1042)
                {
                }
                else
                {
                    myString = txtrepNoa.Text;
                    if ((from u in db.POC_Masters where u.POC_No == myString && u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID select u).Count() > 0)
                    {
                        myString = txtrepNoa.Text;
                    }
                    else
                    {
                        //if (cmbInvType.Text == "Bill of Supply" && logIn.company == 11) //Only for YEN Flexi
                        //{
                        if (txtrepNoa.Text != "")
                        {

                        }
                        else
                        {
                            MessageBox.Show("Please Enter POC No");
                            txtrepNoa.Focus();
                            return;
                            //     }
                        }
                        //else
                        //{
                        //    AutoincrementId();
                        //}
                    }
                }
                myString = txtrepNoa.Text;
                if (txtrepNoa.Text != "")
                {


                    SqlCommand cmd = new SqlCommand("SavePOC", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@POC_No", myString);
                    cmd.Parameters.AddWithValue("@POC_Date", Date.Value);
                    cmd.Parameters.AddWithValue("@Customer_name", CmbsectionrollName.SelectedValue);
                    cmd.Parameters.AddWithValue("@Customer_PO_Ref", (textBox2.Text == "") ? "" : textBox2.Text);
                    cmd.Parameters.AddWithValue("@Total_Qty", (TxtrolledQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(TxtrolledQty.Text));
                    //cmd.Parameters.AddWithValue("@txtMissroll", (txtMissroll.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtMissroll.Text));
                    cmd.Parameters.AddWithValue("@Qty", (txtMissroll.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtMissroll.Text));
                    cmd.Parameters.AddWithValue("@Stock", comboBox5.SelectedText);
                    cmd.Parameters.AddWithValue("@RM_Status", comboBox6.SelectedText);
                    cmd.Parameters.AddWithValue("@Booked_by", Convert.ToInt32(comboBox4.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@Price_basic", comboBox3.SelectedText);
                    cmd.Parameters.AddWithValue("@Packing_charging", (textBox1.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(textBox1.Text));
                    cmd.Parameters.AddWithValue("@Frieght_charge", (txtBurnloss.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtBurnloss.Text));
                   
                    cmd.Parameters.AddWithValue("@Technical_spec", textBox5.Text);

                    cmd.Parameters.AddWithValue("@Del_schedule", txttotal_wrk_hrs.Text);
                    cmd.Parameters.AddWithValue("@payment_terms", comboBox2.SelectedText) ;
                    cmd.Parameters.AddWithValue("@Custom", textBox3.Text);
                    
                    cmd.Parameters.AddWithValue("@Status", Convert.ToInt32(comboBox1ac.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@BU_ID", logIn.BU_ID);
                    cmd.Parameters.AddWithValue("@Company_ID", logIn.company);
                    cmd.Parameters.AddWithValue("@Created_By", lblCreatedBy.Text);
                    cmd.Parameters.AddWithValue("@Modified_By", logIn.username + "-" + DateTime.Now);

                  
                    string Prod_Code = "";
                    string Product_Description = "";
                    string Prod_Grade = "";
                    string Uom = "";
                    string PO_Qty = "";
                    string Stock_Qty = "";
                    string Qty = "";
                    string Price = "";
                    string Amount = "";
                    string Disc_Per = "";
                    string Disc_Amount = "";
                    string Taxable_Value = "";
                    string CGST_Per = "";
                    string SGST_Per = "";
                    string IGST_Per = "";
                    string CGST_Amnt = "";
                    string SGST_Amnt = "";
                    string IGST_Amnt = "";
                    string Net_Amount = "";
                    string SO_NO = "";
                    string Remarks = "";
                    string ProdSno = "";
                    string Company_ID = "";
                    int rowcount = 0;

                    for (int i = 0; i < dgrmconsumed.RowCount - 1; i++)
                    {

                       
                        Product_Description = Product_Description + Convert.ToString(dgrmconsumed.Rows[i].Cells["Prod_Name"].Value).Trim().PadRight(250);
                        Prod_Grade = Prod_Grade + Convert.ToString(dgrmconsumed.Rows[i].Cells["Order_Qty"].Value).Trim().PadRight(250);
                        Uom = Uom + Convert.ToString(dgrmconsumed.Rows[i].Cells["Sale_Price"].Value).Trim().PadRight(14);
                        PO_Qty = PO_Qty + Convert.ToString(dgrmconsumed.Rows[i].Cells["RM_Price"].Value).PadRight(14);
                        Stock_Qty = Stock_Qty + Convert.ToString(dgrmconsumed.Rows[i].Cells["Gross_Margin"].Value).PadRight(14);

                        if (dgrmconsumed.Rows[i].Cells["Item_No"].Value == null || dgrmconsumed.Rows[i].Cells["Item_No"].Value == "")
                        {
                            ProdSno = ProdSno + Convert.ToString(i + 1).PadRight(14);
                        }
                        else
                        {
                            ProdSno = ProdSno + Convert.ToString(dgrmconsumed.Rows[i].Cells["Item_No"].Value).PadRight(14);
                        }



                        //ProdSno = ProdSno + Convert.ToString((i + 1)).PadRight(14); 
                        //Company_ID = logIn.company;
                        rowcount += 1;
                    }

                    cmd.Parameters.AddWithValue("@txt_Product_Name", Product_Description);
                    cmd.Parameters.AddWithValue("@txt_order_qty", Prod_Grade);
                    cmd.Parameters.AddWithValue("@txt_sale_price", Uom);
                    cmd.Parameters.AddWithValue("@txt_Total_RM", PO_Qty);
                    cmd.Parameters.AddWithValue("@txt_Gross_Margin", Stock_Qty);
                    cmd.Parameters.AddWithValue("@txt_Prod_SNO", ProdSno);
                    cmd.Parameters.AddWithValue("@gridcount", rowcount);

                    try
                    {
                        con.Close();
                        con.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (Convert.ToBoolean(result))
                        {
                            MessageBox.Show("Record has been successfully Saved/Updated with POC No :" + myString);
                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        con.Close();
                    }


                }
                else
                {
                    //AutoincrementId();
                    //myString = txtSoNo.Text;

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

        private void dgrmconsumed_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string comnpstatecode, suppStateCode;
                DataGridViewRow R1 = dgrmconsumed.Rows[dgrmconsumed.CurrentRow.Index];
                int columnIndex = dgrmconsumed.CurrentCell.ColumnIndex;
                string columnName = dgrmconsumed.Columns[columnIndex].Name;

                if (columnName == "Product Name")
                {
                    R1.Cells["Order_Qty"].Value = 0.0;
                    R1.Cells["Sale_Price"].Value = 0.0;
                    R1.Cells["RM_Price"].Value = 0.0;
                    R1.Cells["Gross_Margin"].Value = 0.0;
                }
                if (columnName == "RM_Price" || columnName == "Sale_Price")
                {
                    decimal d = Convert.ToDecimal(R1.Cells["Sale_Price"].Value) - Convert.ToDecimal(R1.Cells["RM_Price"].Value);
                    if (d> 0)
                    {
                        R1.Cells["Gross_Margin"].Value = d;
                    }
                    else
                    {
                        MessageBox.Show("RM_Should not be greater than Sale Price");
                        R1.Cells["RM_Price"].Value = 0.0;
                    }
                }


                GetTot();



                }
            catch { }
        }
        public void GetTot()
        {
            try
            {
                decimal totQty = 0;
                decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
                for (int i = 0; i < dgrmconsumed.Rows.Count - 1; i++)
                {
                   
                    totQty += (dgrmconsumed.Rows[i].Cells["Order_Qty"].Value.ToString() == "" || dgrmconsumed.Rows[i].Cells["Order_Qty"].Value == null || dgrmconsumed.Rows[i].Cells["Order_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["Order_Qty"].Value);
                  
                }

                TxtrolledQty.Text = totQty.ToString(".00000");
               
               



               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
        public void POC_d()
        {
            if (txtrepNoa.Text != "")
            {
                var Bu = (from m in db.POC_Masters where m.Company_ID == logIn.company && m.POC_No== txtrepNoa.Text select m).Distinct().ToList();
                if (Bu.Count > 0)
                {

                    CmbsectionrollName.SelectedValue = Bu[0].Customer_name;
                    Date.Text = Bu[0].POC_Date.ToString();
                    textBox2.Text = Bu[0].Customer_PO_Ref;
                    comboBox5.SelectedValue = Bu[0].Stock;
                    comboBox5.SelectedValue = Bu[0].RM_Status;
                    TxtrolledQty.Text = Bu[0].Total_Qty.ToString();
                    txtMissroll.Text = Bu[0].Qty.ToString();
                    comboBox4.SelectedValue = Bu[0].Booked_by;
                    comboBox3.SelectedValue = Bu[0].Price_basic;
                    textBox1.Text = Bu[0].Packing_charging.ToString();
                    txtBurnloss.Text = Bu[0].Frieght_charge.ToString();
                    textBox5.Text = Bu[0].Technical_spec;
                    txttotal_wrk_hrs.Text = Bu[0].Del_schedule;
                    comboBox2.SelectedValue = Bu[0].payment_terms;
                    textBox3.Text = Bu[0].Custom;
                    comboBox1ac.SelectedValue = Bu[0].Status;
                }
                var dm1 = (from s in db.POC_Childs
                           where s.POC_No == txtrepNoa.Text && s.Company_ID == logIn.company
                           


                           select new

                           {
                               Prod_Name=s.prod_name,
                               Order_Qty=s.order_qty,
                               Sale_Price=s.sale_price,
                               RM_Price=s.Total_RM,
                               Gross_Margin=s.Gross_Margin


                           });




                SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataTable dtr = new DataTable();
                da2.Fill(dtr);
                if (dtr.Rows.Count >= 0)
                    dgrmconsumed.DataSource = dtr;


            }
        }
        public frmPOC()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmPOC_Load(object sender, EventArgs e)
        {
            bindCustomer();
        }
        public void bindCustomer()
        {
            try
            {
                var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Trans" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    comboBox1ac.DataSource = pStatus;
                    comboBox1ac.ValueMember = "ID";
                    comboBox1ac.DisplayMember = "Descr";
                }
                comboBox1ac.SelectedIndex = -1;
                var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    CmbsectionrollName.DataSource = Buyerblind;
                    CmbsectionrollName.ValueMember = "ID";
                    CmbsectionrollName.DisplayMember = "Supplier_Name";

                    //CmbConsigneeName.DataSource = Buyerblind;
                    //CmbConsigneeName.ValueMember = "ID";
                    //CmbConsigneeName.DisplayMember = "Customer_Alias_Name";

                }
                //if (CmbBuyerName.Items.Count > 0)
                CmbsectionrollName.SelectedIndex = -1;
                var Buyerblind1 = (from m in db.Sales_Men_Informations where m.Company_ID == logIn.company select new { m.Id,m.Salesmen_Code }).Distinct().ToList();
                if (Buyerblind1.Count > 0)
                {

                    comboBox4.DataSource = Buyerblind1;
                    comboBox4.ValueMember = "Id";
                    comboBox4.DisplayMember = "Salesmen_Code";

                }
                comboBox4.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
