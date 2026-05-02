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

namespace ioneNet.OrderManagement.Transactions
{
    public partial class frmNewOrder : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public static string SONo,ItemCode,OrdQty;
        public frmNewOrder()
        {
            InitializeComponent();
        }

        private void frmNewOrder_Load(object sender, EventArgs e)
        {
            //CmbBuyerName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //CmbConsigneeName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cmbQuotNo.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cmbSaleOffice.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cmbSaleExecutive.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            //cmbStatus.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            bindCustomer();
            bindDroupDown_Lookup();
            AutoincrementId();
            if (ListOfInvoices.editMode == true)
            {
                bindedit();
            }
        }

       

       
       
        

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            Save();
        }

        public void bindCustomer()
        {
            try
            {
                var Buyerblind = (from m in db.Customer_informations select new { m.ID, m.Customer_Name }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    CmbBuyerName.DataSource = Buyerblind;
                    CmbBuyerName.ValueMember = "ID";
                    CmbBuyerName.DisplayMember = "Customer_Name";

                    CmbConsigneeName.DataSource = Buyerblind;
                    CmbConsigneeName.ValueMember = "ID";
                    CmbConsigneeName.DisplayMember = "Customer_Name";

                }
                //if (CmbBuyerName.Items.Count > 0)
                //    CmbBuyerName.SelectedIndex = -1;

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
                var PBasis = (from m in db.Attributes_Datas where m.Head_Name == "Price Basis" select new { m.ID, m.Descr }).Distinct().ToList();
                if (PBasis.Count > 0)
                {
                    cmbPriceBasis.DataSource = PBasis;
                    cmbPriceBasis.ValueMember = "ID";
                    cmbPriceBasis.DisplayMember = "Descr";                    
                }
                //Payment Terms
                var PTerms = (from m in db.Attributes_Datas where m.Head_Name == "Payment Terms" select new { m.ID, m.Descr }).Distinct().ToList();
                if (PTerms.Count > 0)
                {
                    cmbPaymentTerms.DataSource = PTerms;
                    cmbPaymentTerms.ValueMember = "ID";
                    cmbPaymentTerms.DisplayMember = "Descr";
                }

                //Status
                var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Trans" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbStatus.DataSource = pStatus;
                    cmbStatus.ValueMember = "ID";
                    cmbStatus.DisplayMember = "Descr";
                }
                //Insurance
                var pIns = (from m in db.Attributes_Datas where m.Head_Name == "Insurance" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pIns.Count > 0)
                {
                    cmbInsurance.DataSource = pIns;
                    cmbInsurance.ValueMember = "ID";
                    cmbInsurance.DisplayMember = "Descr";
                }

                //Transportation
                var pTrans = (from m in db.Attributes_Datas where m.Head_Name == "Transportation" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pTrans.Count > 0)
                {
                    cmbTransport_Scope.DataSource = pTrans;
                    cmbTransport_Scope.ValueMember = "ID";
                    cmbTransport_Scope.DisplayMember = "Descr";
                }

                
               
                //Sales Office
                var SO = (from m in db.Attributes_Datas where m.Head_Name == "Sales Office" select new { m.ID, m.Descr }).Distinct().ToList();
                if (SO.Count > 0)
                {
                    cmbSaleOffice.DataSource = SO;
                    cmbSaleOffice.ValueMember = "ID";
                    cmbSaleOffice.DisplayMember = "Descr";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void AutoincrementId()
        {
            try
            {

                var result = db.Sp_autoincrement_SaleOrder (logIn.company);
                txtSoNo.Text = result.FirstOrDefault().So_no;
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
                if (tb3 != null && columnName == "UOM")
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
        public static string Mid(string s, int a, int b)
        {
            string temp = s.Substring(a - 1, b);
            return temp;
        }
        public void addItems(AutoCompleteStringCollection coll)
        {
            try
            {
                DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];

                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].HeaderText;

                if (columnName == "UOM")
                {
                    var Prodname = (from d in db.UoM_Masters select new { d.Uom_Descr }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Uom_Descr");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Uom_Descr);
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

        private void CmbBuyerName_Leave(object sender, EventArgs e)
        {
            try
            {
                var State = (from c in db.Customer_informations
                             where c.Customer_Name == CmbBuyerName.Text
                             select new { c.GSTIN_NO}).ToList();
                if (State.Count > 0)
                {
                    txtCustGSTNo.Text = State[0].GSTIN_NO;                   
                }                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dgProducts_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F2)
                {
                    ioneNet.Masters.ProdSearch form = new ioneNet.Masters.ProdSearch();
                    ioneNet.Masters.ProdSearch.frmName = "SOrder";                                   
                    form.ShowDialog();
                }
                if (e.KeyCode == Keys.F3) //Delivery Information / Locations
                {
                    if (chkMultiLocation.Checked == true)
                    {
                        ioneNet.OrderManagement.Transactions.SO_MultiDeliveryAddress form = new OrderManagement.Transactions.SO_MultiDeliveryAddress();
                        //ioneNet.Masters.ProdSearch.frmName = "SOrder";
                        int i = dgProducts.CurrentCell.RowIndex;
                        SONo = txtSoNo.Text;
                        ItemCode = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                        OrdQty = dgProducts.Rows[i].Cells["Qty"].Value.ToString();
                        form.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Check Multi Location Delivery? To Add The Details");

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
                string comnpstatecode, suppStateCode;
                DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].Name;
                if (columnName == "Item_Description")
                {

                    decimal b, c, d;
                    //c = dgProducts.Rows[dgProducts.CurrentRow.Index];
                    R1.Cells["Item_Code"].Value = dgProducts.CurrentCell.RowIndex+1;
                    int taxRate = 18; //Convert.ToInt32(getProduct_Name.GSTRate);
                    
                    var d1 = (from a in db.Company_Infos where a.Id == logIn.company select new { a.GST_No }).ToList();
                    if (d1.Count > 0)
                    {
                        comnpstatecode = Mid(d1[0].GST_No, 1, 2);
                        suppStateCode = Mid(txtCustGSTNo.Text, 1, 2);
                        if (chkSEZOrder.Checked==false)
                        {
                            if (suppStateCode == comnpstatecode)
                            {
                                d = Convert.ToDecimal(taxRate) / 2;
                                R1.Cells["CGST_Per"].Value = d.ToString("0.00");
                                R1.Cells["SGST_Per"].Value = d.ToString("0.00");
                                R1.Cells["IGST_Per"].Value = "0.00";
                            }
                            else
                            {
                                d = taxRate;
                                R1.Cells["CGST_Per"].Value = "0.00";
                                R1.Cells["SGST_Per"].Value = "0.00";
                                R1.Cells["IGST_Per"].Value = d.ToString("0.00");
                            }
                        }
                        else
                        {
                            R1.Cells["CGST_Per"].Value = "0.00";
                            R1.Cells["SGST_Per"].Value = "0.00";
                            R1.Cells["IGST_Per"].Value = "0.00";
                        }
                    }

                }
                if (R1.Cells["Item_Description"].Value != null)
                {
                    decimal ReceivedQty = (R1.Cells["Qty"].Value == "" || R1.Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty"].Value);
                    //decimal RejectedQty = (R1.Cells["Qty_Rejected"].Value == "" || R1.Cells["Qty_Rejected"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty_Rejected"].Value);

                    string Custstatetcode;                   

                    if (columnName == "Basic_Price" || columnName == "Disc_Per")
                    {
                        if (ReceivedQty > 0)
                        {
                            decimal AcceptedQty = (R1.Cells["Qty"].Value == "" || R1.Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty"].Value);
                            decimal price = (R1.Cells["Basic_Price"].Value == "" || R1.Cells["Basic_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Basic_Price"].Value);
                            decimal DiscPer = (R1.Cells["Disc_Per"].Value == "" || R1.Cells["Disc_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Disc_Per"].Value);


                            decimal Amt, DiscAmt, netAmt, gst, igst, totamt;

                            Amt = AcceptedQty * price;

                            R1.Cells["Amt_Before_Disc"].Value = Amt.ToString("0.00");
                            DiscAmt = (Amt * DiscPer) / 100;
                            R1.Cells["Disc_Amt"].Value = DiscAmt.ToString("0.00");
                            netAmt = Amt - DiscAmt;
                            R1.Cells["Taxable_Value"].Value = netAmt.ToString("0.00");
                            gst = (Convert.ToDecimal(R1.Cells["Taxable_Value"].Value) * Convert.ToDecimal(R1.Cells["CGST_Per"].Value)) / 100;

                            R1.Cells["CGST_Amt"].Value = gst;
                            R1.Cells["SGST_Amt"].Value = gst;
                            igst = (Convert.ToDecimal(R1.Cells["Taxable_Value"].Value) * Convert.ToDecimal(R1.Cells["IGST_Per"].Value)) / 100;
                            R1.Cells["IGST_Amt"].Value = igst;

                            totamt = Math.Round(netAmt + gst + gst + igst);
                            R1.Cells["Total_Amount"].Value = totamt;
                            decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
                            for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                            {

                                x += (dgProducts.Rows[i].Cells["Qty"].Value == "" || dgProducts.Rows[i].Cells["Qty"].Value == null || dgProducts.Rows[i].Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty"].Value);
                                y += (dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == "" || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == null || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value);
                                q += (dgProducts.Rows[i].Cells["Disc_Amt"].Value == "" || dgProducts.Rows[i].Cells["Disc_Amt"].Value == null || dgProducts.Rows[i].Cells["Disc_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Amt"].Value);
                                v += (dgProducts.Rows[i].Cells["Taxable_Value"].Value == "" || dgProducts.Rows[i].Cells["Taxable_Value"].Value == null || dgProducts.Rows[i].Cells["Taxable_Value"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Taxable_Value"].Value);
                                cg += (dgProducts.Rows[i].Cells["CGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["CGST_Amt"].Value == null || dgProducts.Rows[i].Cells["CGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Amt"].Value);
                                sg += (dgProducts.Rows[i].Cells["SGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["SGST_Amt"].Value == null || dgProducts.Rows[i].Cells["SGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Amt"].Value);
                                ig += (dgProducts.Rows[i].Cells["IGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["IGST_Amt"].Value == null || dgProducts.Rows[i].Cells["IGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Amt"].Value);
                                totA += (dgProducts.Rows[i].Cells["Total_Amount"].Value == "" || dgProducts.Rows[i].Cells["Total_Amount"].Value == null || dgProducts.Rows[i].Cells["Total_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Total_Amount"].Value);

                            }

                            txtTotalQty.Text = x.ToString(".00");
                            txtSubTotal.Text = y.ToString("0.00");
                            txtTotDiscount.Text = q.ToString(".00");
                            txtTot_TaxableValue.Text = v.ToString(".00");
                            txtTot_CGST.Text = cg.ToString(".00");
                            txtTot_SGST.Text = sg.ToString(".00");
                            txtTot_IGST.Text = ig.ToString(".00");
                            //decimal fAmt = (txtFrieght.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtFrieght.Text);

                            txtTot_OrderValue.Text = (totA).ToString(".00");
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
        public void Save()
        {
            try
            {
                String myString = "";
                myString = txtSoNo.Text;
                if ((from u in db.Sale_Order_Masters where u.SO_NO == myString && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    myString = txtSoNo.Text;
                    db.sp_SO_Delete(myString, logIn.company);
                }
                else
                {
                    AutoincrementId();
                    myString = txtSoNo.Text;

                }
                    //if (frmGate.Modify.Contains(this.Text))
                    //{

                dgProducts.Enabled = false;
                //var S = db.Sale_Order_Masters.Where(w => w.SO_NO == myString && w.Company_ID == logIn.company).FirstOrDefault();
                Sale_Order_Master S = new Sale_Order_Master();
                {
                    S.SO_NO = myString;
                    S.SODate = dpSODate.Value;
                    S.QuotNo = cmbQuotNo.Text;
                    S.QuotDate = dpQuotDate.Value;
                    S.BuyerName = Convert.ToInt32(CmbBuyerName.SelectedValue.ToString());
                    S.ConsigneeName = Convert.ToInt32(CmbConsigneeName.SelectedValue.ToString());
                    S.Status = Convert.ToInt32(cmbStatus.SelectedValue.ToString()); 
                    S.TotalQty = (txtTotalQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotalQty.Text);
                    S.SubTotal = (txtSubTotal.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtSubTotal.Text);
                    S.Tot_Discount = (txtTotDiscount.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotDiscount.Text);
                    S.Tot_TaxableValue = (txtTot_TaxableValue.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTot_TaxableValue.Text);
                    S.CustomerPONo = (txtCustPoNo.Text == "") ? "" : txtCustPoNo.Text;                            
                    S.PODate = dpPODate.Value;
                    S.Cust_GST_No = (txtCustGSTNo.Text == "") ? "" : txtCustGSTNo.Text;
                    S.Tot_CGST_Amnt = (txtTot_CGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_CGST.Text);
                    S.Tot_SGST_Amnt = (txtTot_SGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_SGST.Text);
                    S.Tot_IGST_Amnt = (txtTot_IGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_IGST.Text);
                    S.Tot_Ord_Value = (txtTot_OrderValue.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_OrderValue.Text);
                    S.Delivery_Date = dtDeliveryDate.Value;
                    S.So_Amend_No = (txtAmendNo.Text == "") ? "" : txtAmendNo.Text;
                    S.So_Amend_Date = dtAmendDate.Value;
                    S.Sale_office  = Convert.ToInt32(cmbSaleOffice.SelectedValue.ToString());
                    //S.SaleExecutive = Convert.ToInt32(cmbSaleExecutive.SelectedValue.ToString());
                    S.Price_Basis = Convert.ToInt32(cmbPriceBasis.SelectedValue.ToString());
                    S.Insurance_Scope = Convert.ToInt32(cmbInsurance.SelectedValue.ToString());
                    S.PaymentTerms = Convert.ToInt32(cmbPaymentTerms.SelectedValue.ToString());
                    S.Trasnport_Scope = Convert.ToInt32(cmbTransport_Scope.SelectedValue.ToString());
                    S.Transporter_Name = (cmbTransporter.Text == "") ? "" : cmbTransporter.Text;
                    S.Multi_Loc_Delivery = (chkMultiLocation.Checked == true) ? true : false;                            
                    S.Repeat_Order = (chkRepeatOrder.Checked == true) ? true : false;
                    //S.Old_Ord_Ref = (txt.Text == "") ? "" : txtOtherTerms.Text;
                    S.SEZ_Order = (chkSEZOrder.Checked == true) ? true : false;
                    S.Customer_Contact = (txtCustomeContact.Text == "") ? "" : txtCustomeContact.Text;
                    S.Other_Terms =   (txtOtherTerms.Text == "") ? "" : txtOtherTerms.Text;
                    S.Spl_Instructions = (txtSplInstructions.Text == "") ? "" : txtSplInstructions.Text;
                    S.Company_ID = logIn.company;
                    S.Created_By = lblCreatedBy.Text;                    
                    S.Modified_By = logIn.username + "-" + DateTime.Now;
                    db.Sale_Order_Masters.InsertOnSubmit(S);
                    db.SubmitChanges();
                }
                        //db.Transaction = transaction;
                for (int i = 0; i < dgProducts.RowCount - 1; i++)
                {
                    Sale_Order_Child SC = new Sale_Order_Child();
                            
                    SC.SO_NO = myString;
                    SC.Prod_Code = (dgProducts.Rows[i].Cells["Item_Code"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Code"].Value).ToString();
                    SC.Product_Description = (dgProducts.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value).ToString();
                    SC.Prod_Grade = (dgProducts.Rows[i].Cells["Item_Grade"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value).ToString();
                    SC.Uom = (dgProducts.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["uom"].Value.ToString();
                    SC.Qty = (dgProducts.Rows[i].Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty"].Value);
                    SC.Price = (dgProducts.Rows[i].Cells["Basic_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Basic_Price"].Value);

                    SC.Amount = (dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value);
                    SC.Disc_Per = (dgProducts.Rows[i].Cells["Disc_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Per"].Value);
                    SC.Disc_Amount = (dgProducts.Rows[i].Cells["Disc_Amt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Amt"].Value);
                    SC.Taxable_Value = (dgProducts.Rows[i].Cells["Taxable_Value"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Taxable_Value"].Value);
                    SC.CGST_Per = (dgProducts.Rows[i].Cells["CGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Per"].Value);
                    SC.SGST_Per = (dgProducts.Rows[i].Cells["SGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Per"].Value);
                    SC.IGST_Per = (dgProducts.Rows[i].Cells["IGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Per"].Value);
                    SC.CGST_Amnt = (dgProducts.Rows[i].Cells["CGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Amt"].Value);
                    SC.SGST_Amnt = (dgProducts.Rows[i].Cells["SGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Amt"].Value);
                    SC.IGST_Amnt = (dgProducts.Rows[i].Cells["IGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Amt"].Value);
                    SC.Net_Amount = (dgProducts.Rows[i].Cells["Total_Amount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Total_Amount"].Value);
                    SC.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value).ToString();

                    SC.Company_ID = logIn.company;
                    db.Sale_Order_Childs.InsertOnSubmit(SC);
                }
                db.SubmitChanges();
                //transaction.Commit();               
                MessageBox.Show("Record Saved / Updated Successfully With Transaction Ref No : " + txtSoNo.Text);                   

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        public void bindedit()
        {
            try
            {
                txtSoNo.Text = OrderManagement.Transactions.ListOfInvoices.SO_No;
                String myString = "";
                myString = txtSoNo.Text;
                var da = (from obj in db.Sale_Order_Masters
                          where obj.SO_NO == txtSoNo.Text && obj.Company_ID == logIn.company
                          select obj).ToList();

                if (da.Count > 0)
                {
                    txtSoNo.Text = da[0].SO_NO.ToString();
                    dpSODate.Text = da[0].SODate.ToString();
                    //bindCustomer();
                    CmbBuyerName.SelectedValue = da[0].BuyerName;
                    CmbConsigneeName.SelectedValue = da[0].ConsigneeName;
                    txtCustGSTNo.Text = da[0].Cust_GST_No;
                    txtAmendNo.Text = da[0].So_Amend_No;
                    dtAmendDate.Text = da[0].So_Amend_Date.ToString();
                    if(da[0].Repeat_Order == true)
                    {
                        chkRepeatOrder.Checked = true;                    
                    }
                    else
                    {
                      chkRepeatOrder.Checked = false;                        
                    }
                    if (da[0].SEZ_Order == true)
                    {
                        chkSEZOrder.Checked = true;
                    }
                    else
                    {
                        chkSEZOrder.Checked = false;
                    }
                    if (da[0].Multi_Loc_Delivery == true)
                    {
                        chkMultiLocation.Checked = true;
                    }
                    else
                    {
                        chkMultiLocation.Checked = false;
                    }

                    //cmbCustomer.Enabled = false;
                    cmbQuotNo.Text = da[0].QuotNo;
                    dpQuotDate.Text = da[0].QuotDate.ToString();

                    txtTotalQty.Text = da[0].TotalQty.ToString();
                    txtSubTotal.Text = da[0].SubTotal.ToString();
                    txtTotDiscount.Text = da[0].Tot_Discount.ToString();
                    txtTot_TaxableValue.Text = da[0].Tot_TaxableValue.ToString();
                    txtTot_CGST.Text = da[0].Tot_CGST_Amnt.ToString();
                    txtTot_SGST.Text = da[0].Tot_SGST_Amnt.ToString();
                    txtTot_IGST.Text = da[0].Tot_IGST_Amnt.ToString();
                    txtTot_OrderValue.Text = da[0].Tot_Ord_Value.ToString();
                    txtCustPoNo.Text = da[0].CustomerPONo;
                    dpPODate.Text = da[0].PODate.ToString();
                    dtDeliveryDate.Text = da[0].Delivery_Date.ToString();
                    cmbSaleOffice.SelectedValue = da[0].Sale_office;
                    cmbPriceBasis.SelectedValue = da[0].Price_Basis;
                    cmbInsurance.SelectedValue = da[0].Insurance_Scope;
                    cmbPaymentTerms.SelectedValue = da[0].PaymentTerms;
                    cmbTransport_Scope.SelectedValue = da[0].Trasnport_Scope;
                    cmbTransporter.Text = da[0].Transporter_Name;
                    txtCustomeContact.Text = da[0].Customer_Contact;
                    txtOtherTerms.Text = da[0].Other_Terms;
                    txtSplInstructions.Text = da[0].Spl_Instructions;                    
                    cmbStatus.SelectedValue = da[0].Status;      
                    lblCreatedBy.Text = da[0].Created_By;
                    lblModified.Text = da[0].Modified_By;                    
                }


                var dm1 = (from s in db.Sale_Order_Childs
                           where s.SO_NO == myString && s.Company_ID == logIn.company


                           select new

                           {
                               Item_Code =s.Prod_Code,
                               Item_Description =s.Product_Description,
                               Item_Grade =s.Prod_Grade,
                               UOM=s.Uom,
                               s.Qty,
                               Basic_Price = s.Price,
                               Amt_Before_Disc = s.Amount,
                               s.Disc_Per,
                               Disc_Amt = s.Disc_Amount,
                               s.Taxable_Value,
                               s.CGST_Per,
                               CGST_Amt = s.CGST_Amnt,
                               s.SGST_Per,
                               SGST_Amt = s.SGST_Amnt,
                               s.IGST_Per,
                               IGST_Amt = s.IGST_Amnt,
                               Total_Amount = s.Net_Amount,
                               s.Remarks



                           });




                SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
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
    }
}
