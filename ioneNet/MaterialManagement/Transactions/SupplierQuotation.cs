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

namespace ioneNet.MaterialManagement.Transactions
{
    public partial class SupplierQuotation : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();
        decimal taxRate = 0;
        
        public SupplierQuotation()
        {
            InitializeComponent();
        }

        private void SupplierQuotation_Load(object sender, EventArgs e)
        {
            bind();
            bindDroupDown_Lookup();
            AutoincrementId();

        }
        public void bind()
        {
            try
            {
                var Buyerblind = (from m in db.Req_Quation_Masters where m.Company_ID == logIn.company select new { m.ID, m.RFQ_No }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {

                    cmbrfq.DataSource = Buyerblind;
                    cmbrfq.ValueMember = "ID";
                    cmbrfq.DisplayMember = "RFQ_No";

                }
                cmbrfq.SelectedIndex = -1;


                var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Trans" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbStatus.DataSource = pStatus;
                    cmbStatus.ValueMember = "ID";
                    cmbStatus.DisplayMember = "Descr";
                }
                cmbStatus.SelectedIndex = -1;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        //                  }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                
               

                if (cmbStatus.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Status");
                    cmbStatus.Focus();
                    return;
                }
               
                else if (cmbpdt.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Product Name");
                    cmbpdt.Focus();
                    return;
                }
                else if (cmbrfq.Text == string.Empty)
                {
                    MessageBox.Show("Please Select RFQ NO");
                    cmbrfq.Focus();
                    return;
                }
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

        public void AutoincrementId()
        {
            try
            {
                var getSufix = (from m in db.Financial_Year_Masters where m.Company_ID == logIn.company && m.Start_Date == logIn.fy_Start_Date select new { m.Uses_AsSufix }).Distinct().ToList();
                if (getSufix.Count > 0)
                {

                    var result = db.Sp_autoincrement_Supplier_Quotation(logIn.company, logIn.BU_ID, logIn.fy_Start_Date, logIn.fy_End_Date);
                    txtvchNo.Text = result.FirstOrDefault().RFQ_No;

                }
                txtvchNo.Enabled = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void SaveNew_Sql_proc()
        {
            try
            {
             
                if (txtvchNo.Text != "")
                {


                    SqlCommand cmd = new SqlCommand("Save_SupplierQuotation", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@VchNo", txtvchNo.Text);
                    cmd.Parameters.AddWithValue("@VDate", dpSODate.Value);
                    cmd.Parameters.AddWithValue("@Product_Name", Convert.ToInt32(cmbpdt.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@Prod_spec", (txtprd.Text == "") ? "" : txtprd.Text);
                    //cmd.Parameters.AddWithValue("@UOM", (txtUOM.Text == "") ? "" : txtUOM.Text);
                  //  cmd.Parameters.AddWithValue("@Qty", (txtqty.Text == "") ? "" : txtqty.Text);
                    cmd.Parameters.AddWithValue("@Q_Date", dtquote.Value);
                    cmd.Parameters.AddWithValue("@REQ_No", (cmbrfq.Text == "") ? "" : cmbrfq.Text);
                    //cmd.Parameters.AddWithValue("@Supplier_State", (cmbsupplier.Text == "") ? "" : cmbsupplier.Text);
                    // cmd.Parameters.AddWithValue("@Sale_Account", (cmbSaleAccount.Text == "NA" || cmbSaleAccount.Text == "") ? 7210 : Convert.ToInt32(cmbSaleAccount.SelectedValue.ToString()));
                    //cmd.Parameters.AddWithValue("@Remarks", (txtrmks.Text == "") ? "" : txtrmks.Text);

                    cmd.Parameters.AddWithValue("@Status", Convert.ToInt32(cmbStatus.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@BU_ID", logIn.BU_ID);
                    cmd.Parameters.AddWithValue("@Company_ID", logIn.company);
                    cmd.Parameters.AddWithValue("@Created_By", lblCreatedBy.Text);
                    cmd.Parameters.AddWithValue("@Modified_By", logIn.username + "-" + DateTime.Now);

                   

                    string supp_Code = "";
                    string supp_Q_No = "";
                    string Price = "";
 
                    string Disc_Per = "";
                    string Disc_Amount = "";
                    string Taxable_Value = "";
                    string CGST_Per = "";
                    string SGST_Per = "";
                    string IGST_Per = "";
                    string CGST_Amnt = "";
                    string SGST_Amnt = "";
                    string IGST_Amnt = "";

                    string Total_Amount = "";
                    string Price_Basic = "";
                    string Insurance = "";
                    string Warrenty = "";
                    string Payment_Terms = "";
                    string Delivery_Terms = "";
                    string Other_Terms = "";
                    string Company_ID = "";
                    int rowcount = 0;

                    for (int i = 0; i < dgProducts.RowCount - 1; i++)
                    {
                        var State = (from c in db.Supplier_informations where c.Supplier_Name == Convert.ToString(dgProducts.Rows[i].Cells["Supplier_Name"].Value) && c.Company_ID == logIn.company select new { c.Supplier_Id }).ToList();
                        supp_Code = supp_Code+ Convert.ToString(State[0].Supplier_Id).Trim().PadRight(24); 
                        supp_Q_No = supp_Q_No + Convert.ToString(dgProducts.Rows[i].Cells["S_Qut_No"].Value).Trim().PadRight(25);
                        Price = Price + Convert.ToString(dgProducts.Rows[i].Cells["Price"].Value).Trim().PadRight(14);
                        Disc_Per = Disc_Per + Convert.ToString(dgProducts.Rows[i].Cells["Disc_Per"].Value).PadRight(14);
                        Disc_Amount = Disc_Amount + Convert.ToString(dgProducts.Rows[i].Cells["Disc_Amt"].Value).PadRight(14);
                        Taxable_Value = Taxable_Value + Convert.ToString(dgProducts.Rows[i].Cells["Taxable_Value"].Value).PadRight(14);
                        CGST_Per = CGST_Per + Convert.ToString(dgProducts.Rows[i].Cells["CGST_Per"].Value).PadRight(14);
                        SGST_Per = SGST_Per + Convert.ToString(dgProducts.Rows[i].Cells["SGST_Per"].Value).PadRight(14);
                        IGST_Per = IGST_Per + Convert.ToString(dgProducts.Rows[i].Cells["IGST_Per"].Value).PadRight(14);
                        CGST_Amnt = CGST_Amnt + Convert.ToString(dgProducts.Rows[i].Cells["CGST_Amt"].Value).PadRight(14);
                        SGST_Amnt = SGST_Amnt + Convert.ToString(dgProducts.Rows[i].Cells["SGST_Amount"].Value).PadRight(14);
                        IGST_Amnt = IGST_Amnt + Convert.ToString(dgProducts.Rows[i].Cells["IGST_Amount"].Value).PadRight(14);
                        Total_Amount = Total_Amount + Convert.ToString(dgProducts.Rows[i].Cells["Total_Amount"].Value).PadRight(14);
                        Price_Basic = Price_Basic + Convert.ToString(dgProducts.Rows[i].Cells["Price_Basic"].Value).Trim().PadRight(24);
                        Insurance = Insurance + Convert.ToString(dgProducts.Rows[i].Cells["Insurance"].Value).Trim().PadRight(24);
                        Warrenty = Warrenty + Convert.ToString(dgProducts.Rows[i].Cells["Warrenty"].Value).PadRight(24);
                        Payment_Terms = Payment_Terms + Convert.ToString(dgProducts.Rows[i].Cells["Payment_Terms"].Value).Trim().PadRight(14);
                        Delivery_Terms = Delivery_Terms + Convert.ToString(dgProducts.Rows[i].Cells["Delivery_Terms"].Value).Trim().PadRight(14);
                        Other_Terms = Other_Terms + Convert.ToString(dgProducts.Rows[i].Cells["Other_Terms"].Value).Trim().PadRight(14);

                        Company_ID = logIn.company.ToString();
                        rowcount += 1;
                    }

                    cmd.Parameters.AddWithValue("@txt_Supplier_Name", supp_Code);
                    cmd.Parameters.AddWithValue("@txt_Supp_Q_No", supp_Q_No);
                    cmd.Parameters.AddWithValue("@txt_Price", Price);
                    cmd.Parameters.AddWithValue("@txt_Disc_Per", Disc_Per);
                    cmd.Parameters.AddWithValue("@txt_disc_Price", Disc_Amount);
                    cmd.Parameters.AddWithValue("@txt_Taxable_Value", Taxable_Value);
                    cmd.Parameters.AddWithValue("@txt_CGST_per", CGST_Per);
                    cmd.Parameters.AddWithValue("@txt_CGST_Price", CGST_Amnt);
                    cmd.Parameters.AddWithValue("@txt_SGST_per", SGST_Per);
                    cmd.Parameters.AddWithValue("@txt_SGST_Price", SGST_Amnt);
                    cmd.Parameters.AddWithValue("@txt_IGST_per", IGST_Per);
                    cmd.Parameters.AddWithValue("@txt_IGST_Price", IGST_Amnt);
                    cmd.Parameters.AddWithValue("@txt_Total_Amount", Total_Amount);
                    cmd.Parameters.AddWithValue("@txt_Price_Basic", Price_Basic);
                    cmd.Parameters.AddWithValue("@txt_Insurance", Insurance);
                    cmd.Parameters.AddWithValue("@txt_Warrenty", Warrenty);
                    cmd.Parameters.AddWithValue("@txt_Payment_Terms", Payment_Terms);
                    cmd.Parameters.AddWithValue("@txt_Delivery_Terms", Delivery_Terms);
                    cmd.Parameters.AddWithValue("@txt_Other_Terms", Other_Terms);
                    cmd.Parameters.AddWithValue("@txt_Company_ID", Company_ID);
                    
                    cmd.Parameters.AddWithValue("@gridcount", rowcount);

                    try
                    {
                        con.Close();
                        con.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (Convert.ToBoolean(result))
                        {
                            MessageBox.Show("Record has been successfully Saved/Updated with Voucher No :" + txtvchNo.Text);
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

        private void comboBox3_Leave(object sender, EventArgs e)
            {
            try
            {
                var Buyerblind = (from m in db.Req_Quation_Masters where m.Company_ID == logIn.company && m.RFQ_No == cmbrfq.Text select new { m.Q_Date }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {

                    dtquote.Value = Buyerblind[0].Q_Date.Value;
                }

                var dm1 = (from s in db.Get_suppliers_RFQ(logIn.company,logIn.BU_ID,cmbrfq.Text)                          
                           
                           select new
                           {
                               s.Supplier_Name,
                               s.supplierID,

                           }).Distinct().ToList();
                if (dm1.Count > 0)
                {

                    cmbpdt.DataSource = dm1;
                    cmbpdt.ValueMember = "supplierID";
                    cmbpdt.DisplayMember = "Supplier_Name";

                }
                cmbpdt.SelectedIndex = -1;

                var dm2 = (from s in db.Req_Qutation_Childs
                           join a in db.Req_Quation_Masters on s.M_ID equals a.ID
                           join pr in db.Products on s.Itemcode equals pr.prod_ID
                           join u in db.UoM_Masters on pr.Prod_Primary_UOM_Id equals u.UOM_ID
                           where s.RFQ_No == cmbrfq.Text && s.Company_ID == logIn.company && a.BU_ID == logIn.BU_ID
                           select new
                           {
                               //S_No = s.,
                               Item_Code = s.Itemcode,
                               Prod_code = (logIn.company == 1044 ? pr.Prod_Alternative_Code : pr.Prod_Code),
                               Item_Description = pr.Prod_Name,
                               Prod_Spec = "",
                               Item_Grade = pr.Prod_Field2,
                               UOM = u.Uom_Descr,
                               Qty = s.Qty,
                               PR_No = s.PR_NO.Trim(),
                               Remarks = s.Remarks.Trim()
                           });


                SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm2);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataTable dtr = new DataTable();
                da2.Fill(dtr);
                if (dtr.Rows.Count >= 0)
                    dgProducts.DataSource = dtr;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        System.Data.DataRow drgetproducts;
        System.Data.DataTable dtexisting = new DataTable();
        private void comboBox2_Leave(object sender, EventArgs e)
        {
            try
            {


         
                //var Buyerblind = (from m in db.Req_Qutation_Childs where m.Company_ID == logIn.company && m.RFQ_No == cmbrfq.Text && m.Itemcode == Convert.ToInt32(cmbpdt.SelectedValue) select new { m.Qty, m.UOM, m.length_size, m.model_grade }).Distinct().ToList();
                //if (Buyerblind.Count > 0)
                //{

                //    //txtprd.Text = Buyerblind[0].model_grade.Trim() + "," + Buyerblind[0].length_size.Trim();
                //    //txtqty.Text = Buyerblind[0].Qty.ToString();
                   
                //}


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
                if (columnName == "Price")
                {
                    R1.Cells["Taxable_Value"].Value = (Convert.ToDecimal(R1.Cells["Price"].Value) * Convert.ToDecimal(R1.Cells["Qty"].Value));
                }

                decimal AcceptedQty = Convert.ToDecimal(R1.Cells["Qty"].Value);
                decimal price = (R1.Cells["Price"].Value == "" || R1.Cells["Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Price"].Value);
                decimal DiscPer = (R1.Cells["Disc_Per"].Value == "" || R1.Cells["Disc_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Disc_Per"].Value);


                decimal Amt, DiscAmt, netAmt, gst, igst, totamt;

                Amt = AcceptedQty * price;

                //R1.Cells["Amt_Before_Disc"].Value = Amt.ToString("0.00");
                DiscAmt = (Amt * DiscPer) / 100;
                R1.Cells["Disc_Amt"].Value = DiscAmt.ToString("0.00");
                netAmt = Amt - DiscAmt;
                R1.Cells["Taxable_Value"].Value = netAmt.ToString("0.00");
                gst = (Convert.ToDecimal(R1.Cells["Taxable_Value"].Value) * Convert.ToDecimal(R1.Cells["CGST_Per"].Value)) / 100;

                R1.Cells["CGST_Amt"].Value = gst;
                R1.Cells["SGST_Amount"].Value = gst;
                igst = (Convert.ToDecimal(R1.Cells["Taxable_Value"].Value) * Convert.ToDecimal(R1.Cells["IGST_Per"].Value)) / 100;
                R1.Cells["IGST_Amount"].Value = igst;

                totamt = Math.Round(netAmt + gst + gst + igst);
                R1.Cells["Total_Amount"].Value = totamt;
                ////var taxPer = (from s in db.Tax_Class_Masters
                ////              where s.ID == Convert.ToInt32(comboBox2.SelectedValue) && s.Company_ID==logIn.company
                ////              select new { s.Gst_Rate }).FirstOrDefault();

                ////taxRate = Convert.ToDecimal(taxPer.Gst_Rate);



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }



        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtvchNo.Text = "";
            dpSODate.Value = DateTime.Now;
            cmbrfq.Text = "";
            cmbpdt.Text = "";
            txtprd.Text = "";            
            //txtqty.Text = "";          
           
            cmbStatus.Text = "";
            dtgetfinalprducts.Rows.Clear();
            dtgetfinalprducts.Columns.Clear();
            dgProducts.DataSource = dtgetfinalprducts;
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            AutoincrementId();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtSoNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgProducts_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].HeaderText;
                TextBox tb3 = e.Control as TextBox;
                tb3.AutoCompleteMode = AutoCompleteMode.None;
                if (tb3 != null && columnName == "Supplier_Name")
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

                if (columnName == "Supplier_Name")
                {
                    var Prodname = (from d in db.Get_suppliers_RFQ(logIn.company,logIn.BU_ID,cmbrfq.Text)
                                    select new
                                    {
                                        d.Supplier_Name
                                    }
                                    ).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Supplier_Name");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Supplier_Name);
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
        public void bindDroupDown_Lookup()
        {
            try
            {
              
                //Price Basis
                var PBasis = (from m in db.Attributes_Datas where m.Head_Name == "Price Basis" && (m.Company_ID == logIn.company || m.Company_ID == 0) select new { m.ID, m.Descr }).Distinct().ToList();
                if (PBasis.Count > 0)
                {
                    cmbPriceBasis.DataSource = PBasis;
                    cmbPriceBasis.ValueMember = "ID";
                    cmbPriceBasis.DisplayMember = "Descr";
                }
                //Payment Terms
                var PTerms = (from m in db.Attributes_Datas where m.Head_Name == "Payment Terms" && (m.Company_ID == logIn.company || m.Company_ID == 0) select new { m.ID, m.Descr }).Distinct().ToList();
                if (PTerms.Count > 0)
                {
                    cmbPaymentTerms.DataSource = PTerms;
                    cmbPaymentTerms.ValueMember = "ID";
                    cmbPaymentTerms.DisplayMember = "Descr";
                }

                //Status
                var pStatus = (from m in db.Attributes_Datas
                               join r in db.view_Trans_Auth_Levels
                               on m.ID equals r.Status_Code
                               where r.Menu_Item == this.Text && (r.Company_ID == logIn.company) && r.Role_ID == logIn.UserRoleID
                               select new { m.ID, m.Descr }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbStatus.DataSource = pStatus;
                    cmbStatus.ValueMember = "ID";
                    cmbStatus.DisplayMember = "Descr";
                }


                //Insurance
                var pIns = (from m in db.Attributes_Datas where m.Head_Name == "Insurance" && (m.Company_ID == logIn.company || m.Company_ID == 0) select new { m.ID, m.Descr }).Distinct().ToList();
                if (pIns.Count > 0)
                {
                    cmbInsurance.DataSource = pIns;
                    cmbInsurance.ValueMember = "ID";
                    cmbInsurance.DisplayMember = "Descr";
                }
               
             
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgProducts_CellEndEdit_1(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].Name;
                string pcode = "";
                

                if (columnName == "Qty" || columnName == "Disc_Per" || columnName == "Basic_Price" || columnName == "Frieght_Unit" || columnName == "Other_Charges_Unit")
                {
                    int Itemcode = Convert.ToInt32(R1.Cells["Item_Code"].Value.ToString());
                    decimal taxRate;
                    if (Itemcode != null)
                    {
                        decimal b, c, tr;
                        decimal AcceptedQty = (R1.Cells["Qty"].Value == "" || R1.Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty"].Value);
                        decimal price = (R1.Cells["Basic_Price"].Value == "" || R1.Cells["Basic_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Basic_Price"].Value);
                        decimal DiscPer = (R1.Cells["Disc_Per"].Value == "" || R1.Cells["Disc_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Disc_Per"].Value);
                        decimal famt = (R1.Cells["Frieght_Unit"].Value == "" || R1.Cells["Frieght_Unit"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Frieght_Unit"].Value);
                        decimal oamt = (R1.Cells["Other_Charges_Unit"].Value == "" || R1.Cells["Other_Charges_Unit"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Other_Charges_Unit"].Value);




                        //c = dgProducts.Rows[dgProducts.CurrentRow.Index];
                        var taxratelist = (from prd in db.Products join tax in db.Tax_Class_Masters on prd.Prod_Tax_Class equals tax.ID where prd.prod_ID == Itemcode select new { tax.Gst_Rate }).ToList();
                        if (taxratelist.Count > 0)
                        {
                            taxRate = Convert.ToDecimal(taxratelist[0].Gst_Rate); //Convert.ToInt32(getProduct_Name.GSTRate);
                            R1.Cells["CGST_Per"].Value = taxRate;
                        }
                        else
                        {
                            MessageBox.Show("Tax Clas Not Defined For the Selected Product");
                            return;
                        }
                        

                        decimal Amt, DiscAmt, netAmt, gst, igst, totamt, netrate;

                        Amt = AcceptedQty * price;
                       

                        R1.Cells["Amt_Before_Disc"].Value = Amt.ToString("0.00");
                        DiscAmt = (Amt * DiscPer) / 100;
                        R1.Cells["Disc_Amt"].Value = DiscAmt.ToString("0.00");
                        netAmt = Amt - DiscAmt;
                        R1.Cells["Taxable_Value"].Value = netAmt.ToString("0.00");
                        gst = (Convert.ToDecimal(R1.Cells["Taxable_Value"].Value) * Convert.ToDecimal(R1.Cells["CGST_Per"].Value)) / 100;

                        R1.Cells["CGST_Amt"].Value = gst;

                        netrate = (netAmt / AcceptedQty) + famt + oamt;
                        R1.Cells["Net_Unit_Rate"].Value = netrate.ToString("0.00");
                        totamt = Math.Round(netAmt + gst);
                        R1.Cells["Total_Amount"].Value = totamt;
                        

                    }
                    else
                    {

                    }


                }
                if (columnName == "Item_Description" && R1.Cells["Item_Description"].Value != null)
                {
                    decimal ReceivedQty = (R1.Cells["Qty"].Value == "" || R1.Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty"].Value);
                    //decimal RejectedQty = (R1.Cells["Qty_Rejected"].Value == "" || R1.Cells["Qty_Rejected"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty_Rejected"].Value);

                    string Custstatetcode;

                    if (columnName == "Basic_Price" || columnName == "Disc_Per" || columnName == "Qty" || columnName == "Frieght_Unit" || columnName == "Other_Charges_Unit")
                    {
                        if (ReceivedQty > 0)
                        {
                            decimal AcceptedQty = (R1.Cells["Qty"].Value == "" || R1.Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty"].Value);
                            decimal price = (R1.Cells["Basic_Price"].Value == "" || R1.Cells["Basic_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Basic_Price"].Value);
                            decimal DiscPer = (R1.Cells["Disc_Per"].Value == "" || R1.Cells["Disc_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Disc_Per"].Value);
                            decimal famt = (R1.Cells["Frieght_Unit"].Value == "" || R1.Cells["Frieght_Unit"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Frieght_Unit"].Value);
                            decimal oamt = (R1.Cells["Other_Charges_Unit"].Value == "" || R1.Cells["Other_Charges_Unit"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Other_Charges_Unit"].Value);


                            decimal Amt, DiscAmt, netAmt, gst, igst, totamt,netrate;

                            Amt = AcceptedQty * price;

                            R1.Cells["Amt_Before_Disc"].Value = Amt.ToString("0.00");
                            DiscAmt = (Amt * DiscPer) / 100;
                            R1.Cells["Disc_Amt"].Value = DiscAmt.ToString("0.00");
                            netAmt = Amt - DiscAmt;
                            R1.Cells["Taxable_Value"].Value = netAmt.ToString("0.00");
                            gst = (Convert.ToDecimal(R1.Cells["Taxable_Value"].Value) * Convert.ToDecimal(R1.Cells["CGST_Per"].Value)) / 100;

                            R1.Cells["CGST_Amt"].Value = gst;
                            netrate = (netAmt / AcceptedQty) + famt + oamt;
                            R1.Cells["Net_Unit_Rate"].Value = netrate.ToString("0.00");
                            totamt = Math.Round(netAmt + gst);
                            R1.Cells["Total_Amount"].Value = totamt;
                           
                        }

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

        private void cmbPriceBasis_Leave(object sender, EventArgs e)
        {
            int i = (cmbPriceBasis.FindString(cmbPriceBasis.Text));
            if (i < 0)
            {
                MessageBox.Show("Invalid Selection");
                cmbPriceBasis.Focus();

            }

        }

        private void cmbInsurance_Leave(object sender, EventArgs e)
        {
            int i = (cmbInsurance.FindString(cmbInsurance.Text));
            if (i < 0)
            {
                MessageBox.Show("Invalid Selection");
                cmbInsurance.Focus();

            }
        }

        private void cmbPaymentTerms_Leave(object sender, EventArgs e)
        {
            int i = (cmbPaymentTerms.FindString(cmbPaymentTerms.Text));
            if (i < 0)
            {
                MessageBox.Show("Invalid Selection");
                cmbPaymentTerms.Focus();

            }
        }
    }
}
