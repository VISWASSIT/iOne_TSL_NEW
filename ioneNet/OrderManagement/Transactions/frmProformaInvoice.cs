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
using System.Data.Linq.SqlClient;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Ione_DAL;
namespace ioneNet.OrderManagement.Transactions
{
    public partial class frmProformaInvoice : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();
        public static string SONo,ItemCode,OrdQty;
        decimal taxRate = 0;
        decimal cgstPer,sgstPer,igstPer;

        public frmProformaInvoice()
        {
            InitializeComponent();
        }

        private void frmNewOrder_Load(object sender, EventArgs e)
        {
            dpInvDate.MinDate = logIn.fy_Start_Date;
            dpInvDate.MaxDate = logIn.fy_End_Date;
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

            bindCustomer();
            bindConsignee();
            bindDroupDown_Lookup();           

            //AutoincrementId();
            if (ListOfProformaInvoices.editMode == true || frmCRMDashBoard.editMode ==true)
            {
                bindedit();
            }
            else
            {
                AutoincrementId();

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
                else if (txtCustPoNo.Text == string.Empty)
                {
                    MessageBox.Show("Customer PO No Should Not Be Empty");
                    txtCustPoNo.Focus();
                    return;
                }
               
                else if (cmbStatus.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Status");
                    cmbStatus.Focus();
                    return;
                }
              
                else if (cmbTaxClass.Text == string.Empty)
                {
                    MessageBox.Show("Tax Class Cannot be Empty,");
                    cmbTaxClass.Focus();
                    return;
                }
                else 
                {
                    //Check Wether Any products Entered or not incl Qty and No Of Packs
                    if (dgProducts.Rows[0].Cells["Item_Code"].Value == null)
                    {
                        MessageBox.Show("Atleast One Record To Be Entered To Proceed", "Order Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        Save();
                    }
                    //else
                    //{


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void bindCustomer()
        {
            try
            {
                var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    CmbBuyerName.DataSource = Buyerblind;
                    CmbBuyerName.ValueMember = "ID";
                    CmbBuyerName.DisplayMember = "Supplier_Name";

                    //CmbConsigneeName.DataSource = Buyerblind;
                    //CmbConsigneeName.ValueMember = "ID";
                    //CmbConsigneeName.DisplayMember = "Customer_Alias_Name";

                }
                //if (CmbBuyerName.Items.Count > 0)
                //    CmbBuyerName.SelectedIndex = -1;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void bindConsignee()
        {
            try
            {
                var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company  select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    //CmbBuyerName.DataSource = Buyerblind;
                    //CmbBuyerName.ValueMember = "ID";
                    //CmbBuyerName.DisplayMember = "Customer_Alias_Name";

                    CmbConsigneeName.DataSource = Buyerblind;
                    CmbConsigneeName.ValueMember = "ID";
                    CmbConsigneeName.DisplayMember = "Supplier_Name";

                }
                //if (CmbBuyerName.Items.Count > 0)
                //    CmbBuyerName.SelectedIndex = -1;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void bindDeliveryAddress()
        {
            try
            {
                var Buyerblind = (from m in db.SaleOrder_Consignee_Datas where m.SO_NO == SONo select new { m.ConsigneeCode, m.ConsigneeName }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    //CmbBuyerName.DataSource = Buyerblind;
                    //CmbBuyerName.ValueMember = "ConsigneeCode";
                    //CmbBuyerName.DisplayMember = "ConsigneeName";

                    CmbConsigneeName.DataSource = Buyerblind;
                    CmbConsigneeName.ValueMember = "ConsigneeCode";
                    CmbConsigneeName.DisplayMember = "ConsigneeName";

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
                

                //Status
                var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Trans" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbStatus.DataSource = pStatus;
                    cmbStatus.ValueMember = "ID";
                    cmbStatus.DisplayMember = "Descr";
                }



                //Bind Tax Class
                var bindLoc = (from m in db.Tax_Class_Masters
                               where m.Company_ID == logIn.company
                               select new
                               {
                                   m.Tax_Class_Name,
                                   m.ID,
                               }).ToList();

                if (bindLoc.Count > 0)
                {
                    cmbTaxClass.DisplayMember = "Tax_Class_Name";
                    cmbTaxClass.ValueMember = "ID";
                    cmbTaxClass.DataSource = bindLoc;

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
                
                var result = db.Sp_autoincrement_Proforma_Invoice(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date,logIn.BU_ID);
                txtInvNo.Text = result.FirstOrDefault().Inv_No;
                  
               
                
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
                tb3.AutoCompleteCustomSource = null;
                if (tb3 != null && columnName == "UOM")
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
                else
                {
                    if (columnName == "Item Description")
                    {
                        var Prodname = (from d in db.Get_ProductsList(logIn.company, 0, null) select new { d.Prod_Name }).ToList();
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


            }
            catch (Exception ex)
            {
            }
        }

        private void CmbBuyerName_Leave(object sender, EventArgs e)
        {
            //try
            //{
            //    string t = CmbBuyerName.Text;

            //    if (t != null)
            //    {
            //    //    if (CmbBuyerName.SelectedValue != null)
            //    //{
            //        var State = (from c in db.Supplier_informations
            //                     where c.Supplier_Name == t  && c.Company_ID ==logIn.company //Convert.ToInt32(CmbBuyerName.SelectedValue.ToString())
            //                     select new { c.GSTIN_NO }).ToList();
            //        if (State.Count > 0)
            //        {
            //            txtCustGSTNo.Text = State[0].GSTIN_NO;
            //        }
            //    }
            //    else
            //    {
            //        MessageBox.Show("Customer Name Not Selected");
            //    }

                

            //}
            //catch (Exception ex)
            //{

            //    MessageBox.Show(ex.Message);
            //}
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
                if (e.KeyCode == Keys.F6) //Delivery Information / Locations
                {
                    if (dgProducts.Rows.Count > 0)
                    {
                        
                        foreach (DataGridViewCell oneCell in dgProducts.SelectedCells)
                        {
                            if (oneCell.Selected)
                                dgProducts.Rows.RemoveAt(oneCell.RowIndex);
                        }
                        //}
                        decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
                        for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                        {

                            x += (dgProducts.Rows[i].Cells["Inv_Qty"].Value == "" || dgProducts.Rows[i].Cells["Inv_Qty"].Value == null || dgProducts.Rows[i].Cells["Inv_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Inv_Qty"].Value);
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
                        decimal fAmt = (txtFrieght.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtFrieght.Text);
                        decimal OthAmt = (txtOtherCharges.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtOtherCharges.Text);

                        txtTot_TaxableValue.Text = (v).ToString(".00");

                        txtTot_CGST.Text = cg.ToString(".00");
                        txtTot_SGST.Text = sg.ToString(".00");
                        txtTot_IGST.Text = ig.ToString(".00");
                        decimal AmtForTCs = (fAmt + OthAmt + v + cg + sg + ig);
                        decimal tcsPer = (txtTCSPer.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtTCSPer.Text);

                        decimal TcsAmt = AmtForTCs * tcsPer / 100;
                        txtTCSAmt.Text = TcsAmt.ToString(".00");
                        decimal rndAmt = (txtRounding.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtRounding.Text);
                        txtTot_InvValue.Text = (totA + TcsAmt + fAmt + OthAmt + rndAmt).ToString(".00");


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
                decimal taxRate = 0;
                DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].Name;
               

                if (columnName == "Item_Description" && R1.Cells["Item_Description"].Value != null)
                {

                    var getProductName = (from s in db.Get_ProductsList(logIn.company, 1, R1.Cells["Item_Description"].Value.ToString())
                                          select new { s.prod_ID, s.Uom_Descr, s.Prod_Group_Name, s.Prod_HSN_Code, s.Gst_Rate, s.Prod_Customer_Code, s.Prod_Description }).FirstOrDefault();

                    if (getProductName != null)
                    {
                        R1.Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                        R1.Cells["Item_code"].Value = getProductName.prod_ID.ToString();
                        if (getProductName.Prod_Customer_Code != null)
                        {
                            R1.Cells["Remarks"].Value = getProductName.Prod_Customer_Code.ToString();
                        }
                        taxRate = Convert.ToDecimal(getProductName.Gst_Rate);
                        //R1.Cells["Prod_Tole_Qty"].Value = "0";
                        //R1.Cells["Delivery_Date"].Value = DateTime.Today.ToString("MM/dd/yyyy");
                        R1.Cells["Item_Grade"].Value = getProductName.Prod_Description;
                    }

                    else
                    {
                        R1.Cells["Item_Code"].Value = dgProducts.CurrentCell.RowIndex + 1;
                        //R1.Cells["Prod_Tole_Qty"].Value = "0";
                        //R1.Cells["Delivery_Date"].Value = DateTime.Today.ToString("dd/MM/yyyy");
                    }
                    decimal d;

                    if (taxRate != 0)
                    {

                    }
                    else
                    {
                        taxRate = 18;
                    }
                    var d1 = (from a in db.Company_Infos where a.Id == logIn.company select new { a.GST_No, a.State_Code }).ToList();
                    if (d1.Count > 0)
                    {
                        comnpstatecode = d1[0].State_Code;
                        suppStateCode = txtCustStateCode.Text;                        
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

                }
                if (columnName == "Inv_Qty")
                {
                    decimal d;
                    Boolean sezorder = false;
                    var getProductName = (from s in db.Get_ProductsList(logIn.company, 1, R1.Cells["Item_Description"].Value.ToString())
                                          select new { s.prod_ID, s.Uom_Descr, s.Prod_Group_Name, s.Prod_HSN_Code, s.Gst_Rate }).FirstOrDefault();


                    if (getProductName != null)
                    {
                        R1.Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                        R1.Cells["Item_code"].Value = getProductName.prod_ID.ToString();
                        taxRate = Convert.ToDecimal(getProductName.Gst_Rate);
                    }
                    if (taxRate != 0)
                    {

                    }
                    else
                    {
                        taxRate = 18;
                    }
                    var d1 = (from a in db.Company_Infos where a.Id == logIn.company select new { a.GST_No, a.State_Code }).ToList();
                    if (d1.Count > 0)
                    {
                        var d2 = (from a in db.Sale_Order_Masters where a.SO_NO == R1.Cells["SO_Ref_No"].Value.ToString() && a.Company_ID == logIn.company && a.BU_ID == logIn.BU_ID select new { a.SEZ_Order }).ToList();
                        if (d2.Count > 0)
                        {
                            if(d2[0].SEZ_Order == true)
                            {
                                sezorder = true;
                            }
                            else
                            {
                                sezorder = false;
                            }
                            
                        }
                            comnpstatecode = d1[0].State_Code;
                        suppStateCode = txtCustStateCode.Text;
                        if (sezorder == true)
                        {
                            R1.Cells["CGST_Per"].Value = "0.00";
                            R1.Cells["SGST_Per"].Value = "0.00";
                            R1.Cells["IGST_Per"].Value = "0.00";
                            //cgstPer = Convert.ToDecimal(R1.Cells["CGST_Per"].Value);
                            //sgstPer = Convert.ToDecimal(R1.Cells["SGST_Per"].Value);
                            //igstPer = Convert.ToDecimal(R1.Cells["IGST_Per"].Value);
                        }
                        else
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
                    }

                }
                if (R1.Cells["Item_Description"].Value != null)
                {
                    decimal ReceivedQty = (R1.Cells["Inv_Qty"].Value == "" || R1.Cells["Inv_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Inv_Qty"].Value);
                    //decimal RejectedQty = (R1.Cells["Qty_Rejected"].Value == "" || R1.Cells["Qty_Rejected"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty_Rejected"].Value);



                    if (columnName == "Basic_Price" || columnName == "Disc_Per" || columnName == "Inv_Qty")
                    {
                        if (ReceivedQty > 0)
                        {
                            decimal AcceptedQty = (R1.Cells["Inv_Qty"].Value == "" || R1.Cells["Inv_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Inv_Qty"].Value);
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
                            decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0;
                            for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                            {

                                x += (dgProducts.Rows[i].Cells["Inv_Qty"].Value == "" || dgProducts.Rows[i].Cells["Inv_Qty"].Value == null || dgProducts.Rows[i].Cells["Inv_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Inv_Qty"].Value);
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

                            txtTot_InvValue.Text = (totA).ToString(".00");
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

        private void tableLayoutPanel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtCustPoNo_Leave(object sender, EventArgs e)
        {
            try
            {
                if(txtCustPoNo.Text!="")
                {
                    if (txtCustPoNo.Text != "NA")
                    {
                        GetOrderInfo();
                    }
                }
            }
            catch
            {

            }
        }
        public void GetTot()
        {
            double totQty = 0;
            decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
            for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
            {
                totQty += (dgProducts.Rows[i].Cells["Inv_Qty"].Value.ToString() == "" || dgProducts.Rows[i].Cells["Inv_Qty"].Value == null || dgProducts.Rows[i].Cells["Inv_Qty"].Value == DBNull.Value) ? Convert.ToDouble(0) : Convert.ToDouble(dgProducts.Rows[i].Cells["Inv_Qty"].Value);
                //x += (dgProducts.Rows[i].Cells["Inv_Qty"].Value == "" || dgProducts.Rows[i].Cells["Inv_Qty"].Value == null || dgProducts.Rows[i].Cells["Inv_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Inv_Qty"].Value);
                y += (dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value.ToString() == "" || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == null || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value);
                q += (dgProducts.Rows[i].Cells["Disc_Amt"].Value.ToString() == "" || dgProducts.Rows[i].Cells["Disc_Amt"].Value == null || dgProducts.Rows[i].Cells["Disc_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Amt"].Value);
                v += (dgProducts.Rows[i].Cells["Taxable_Value"].Value.ToString() == "" || dgProducts.Rows[i].Cells["Taxable_Value"].Value == null || dgProducts.Rows[i].Cells["Taxable_Value"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Taxable_Value"].Value);
                cg += (dgProducts.Rows[i].Cells["CGST_Amt"].Value.ToString() == "" || dgProducts.Rows[i].Cells["CGST_Amt"].Value == null || dgProducts.Rows[i].Cells["CGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Amt"].Value);
                sg += (dgProducts.Rows[i].Cells["SGST_Amt"].Value.ToString() == "" || dgProducts.Rows[i].Cells["SGST_Amt"].Value == null || dgProducts.Rows[i].Cells["SGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Amt"].Value);
                ig += (dgProducts.Rows[i].Cells["IGST_Amt"].Value.ToString() == "" || dgProducts.Rows[i].Cells["IGST_Amt"].Value == null || dgProducts.Rows[i].Cells["IGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Amt"].Value);
                totA += (dgProducts.Rows[i].Cells["Total_Amount"].Value.ToString() == "" || dgProducts.Rows[i].Cells["Total_Amount"].Value == null || dgProducts.Rows[i].Cells["Total_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Total_Amount"].Value);
                cgstPer = (dgProducts.Rows[i].Cells["CGST_Per"].Value.ToString() == "" || dgProducts.Rows[i].Cells["CGST_Per"].Value == null || dgProducts.Rows[i].Cells["CGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Per"].Value);
                //cgstPer = Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Per"].Value);
                sgstPer = (dgProducts.Rows[i].Cells["SGST_Per"].Value.ToString() == "" || dgProducts.Rows[i].Cells["SGST_Per"].Value == null || dgProducts.Rows[i].Cells["SGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Per"].Value);
                igstPer = (dgProducts.Rows[i].Cells["IGST_Per"].Value.ToString() == "" || dgProducts.Rows[i].Cells["IGST_Per"].Value == null || dgProducts.Rows[i].Cells["IGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Per"].Value);

            }

            txtTotalQty.Text = totQty.ToString(".00000");
            txtSubTotal.Text = y.ToString("0.00");
            txtTotDiscount.Text = q.ToString(".00");
            decimal fAmt = (txtFrieght.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtFrieght.Text);
            decimal OthAmt = (txtOtherCharges.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtOtherCharges.Text);

            txtTot_TaxableValue.Text = (v+fAmt+OthAmt).ToString(".00");
            decimal taxvalue = Convert.ToDecimal(txtTot_TaxableValue.Text);
            decimal cgst = (taxvalue * cgstPer) / 100;
            decimal sgst = (taxvalue * sgstPer) / 100;
            decimal igst = (taxvalue * igstPer) / 100;
            txtTot_CGST.Text = cgst.ToString(".00");
            txtTot_SGST.Text = sgst.ToString(".00");
            txtTot_IGST.Text = igst.ToString(".00");
            //decimal taxvalue = Convert.ToDecimal(txtTot_TaxableValue.Text);
            //decimal cgst = (taxvalue * cgstPer) / 100;
            //decimal sgst = (taxvalue * sgstPer) / 100;
            //decimal igst = (taxvalue * igstPer) / 100;

            //txtTot_CGST.Text = cgst.ToString(".00");
            //txtTot_SGST.Text = sgst.ToString(".00");
            //txtTot_IGST.Text = igst.ToString(".00");
            //txtTot_InvValue.Text = (totA).ToString(".00");
            decimal AmtForTCs = (fAmt + OthAmt + v + cgst + sgst + igst);

            //decimal AmtForTCs = (fAmt + OthAmt + v + cgst + sgst + igst);
            decimal tcsPer = (txtTCSPer.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtTCSPer.Text);

            decimal TcsAmt = AmtForTCs * tcsPer / 100;
            txtTCSAmt.Text = TcsAmt.ToString(".00");
            decimal rndAmt = (txtRounding.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtRounding.Text);
            txtTot_InvValue.Text = (taxvalue + cgst+ sgst+ igst + TcsAmt + rndAmt).ToString(".00");

        }
        public void Save()
        {
            try
            {
                String myString = "";
                //AutoincrementId();
                myString = txtInvNo.Text;
                if ((from u in db.Proforma_Invoice_Masters where u.Inv_No == myString && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    myString = txtInvNo.Text;
                    db.sp_ProformaInv_Delete(myString, logIn.company,logIn.BU_ID);
                }
                else
                {
                    AutoincrementId();
                    myString = txtInvNo.Text;

                }
                    //if (frmGate.Modify.Contains(this.Text))
                    //{

               // dgProducts.Enabled = false;
                //var S = db.Sale_Order_Masters.Where(w => w.SO_NO == myString && w.Company_ID == logIn.company).FirstOrDefault();
                Proforma_Invoice_Master S = new Proforma_Invoice_Master();
                {
                    S.Inv_No = myString;
                    S.InvDate = dpInvDate.Value;                   
                    S.Tax_Class = Convert.ToInt32(cmbTaxClass.SelectedValue.ToString());
                    S.BuyerName = Convert.ToInt32(CmbBuyerName.SelectedValue.ToString());
                    S.ConsigneeName = Convert.ToInt32(CmbConsigneeName.SelectedValue.ToString());
                    S.ConsigneeAddress = txtConAddress.Text;
                    S.Con_GST_No = txtConGSTNo.Text;
                    S.Status = Convert.ToInt32(cmbStatus.SelectedValue.ToString());
                    double amt = Convert.ToDouble(txtTotalQty.Text);
                    //decimal qty = decimal.Round(Convert.ToDecimal(amt),5);
                    S.TotalQty = amt;
                    //S.TotalQty = (txtTotalQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotalQty.Text);
                    S.SubTotal = (txtSubTotal.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtSubTotal.Text);
                    S.Tot_Discount = (txtTotDiscount.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotDiscount.Text);
                    S.Tot_TaxableValue = (txtTot_TaxableValue.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTot_TaxableValue.Text);
                    S.CustomerPONo = (txtCustPoNo.Text == "") ? "" : txtCustPoNo.Text;                            
                    S.PODate = dpPODate.Value;
                    S.Cust_GST_No = (txtCustGSTNo.Text == "") ? "" : txtCustGSTNo.Text;
                    S.Frieght_amnt = (txtFrieght.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtFrieght.Text);
                    S.Other_Charges = (txtOtherCharges.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtOtherCharges.Text);
                    S.Rounding = (txtRounding.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtRounding.Text);
                    S.Tot_CGST_Amnt = (txtTot_CGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_CGST.Text);
                    S.Tot_SGST_Amnt = (txtTot_SGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_SGST.Text);
                    S.Tot_IGST_Amnt = (txtTot_IGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_IGST.Text);
                    S.TCS_Per = (txtTCSPer.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTCSPer.Text);
                    S.TCS_Amnt = (txtTCSAmt.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTCSAmt.Text);
                    S.Tot_Inv_Value = (txtTot_InvValue.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_InvValue.Text);
                   
                   
                    S.Spl_Instructions = (txtSplInstructions.Text == "") ? "" : txtSplInstructions.Text;
                    S.SO_Ref_No = txtSoNo.Text;                   
                    S.BU_ID = logIn.BU_ID;
                    S.Company_ID = logIn.company;
                    S.Created_By = lblCreatedBy.Text;                    
                    S.Modified_By = logIn.username + "-" + DateTime.Now;                   
                    db.Proforma_Invoice_Masters.InsertOnSubmit(S);
                    db.SubmitChanges();
                }
                        //db.Transaction = transaction;
                for (int i = 0; i < dgProducts.RowCount - 1; i++)
                {
                    Proforma_Invoice_Child SC = new Proforma_Invoice_Child();
                    var d1 = (from a in db.Proforma_Invoice_Masters where a.Inv_No == myString && a.Company_ID == logIn.company select new { a.Id }).ToList();
                    SC.Inv_Master_ID = d1[0].Id;
                    SC.Inv_No = myString;
                    SC.Prod_Code = (dgProducts.Rows[i].Cells["Item_Code"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Code"].Value).ToString();
                    SC.Product_Description = (dgProducts.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value).ToString();
                    SC.Prod_Grade = (dgProducts.Rows[i].Cells["Item_Grade"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value).ToString();
                    SC.Uom = (dgProducts.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["uom"].Value.ToString();
                    SC.PO_Qty = (dgProducts.Rows[i].Cells["PO_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["PO_Qty"].Value);
                                      
                    double amt = Convert.ToDouble(dgProducts.Rows[i].Cells["Inv_Qty"].Value);
                    //decimal qty = decimal.Round(Convert.ToDecimal(amt),5);
                    SC.Qty = amt;
                    //SC.Qty = (dgProducts.Rows[i].Cells["Inv_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("0") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Inv_Qty"].Value);
                    
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
                    SC.SO_Ref_No = (dgProducts.Rows[i].Cells["SO_Ref_No"].Value == null) ? "" : (dgProducts.Rows[i].Cells["SO_Ref_No"].Value).ToString();
                    SC.Item_No = i+1;

                    SC.Company_ID = logIn.company;
                    db.Proforma_Invoice_Childs.InsertOnSubmit(SC);
                }
                db.SubmitChanges();
                //transaction.Commit();               
                MessageBox.Show("Invoice Saved / Updated Successfully With Transaction Ref No : " + txtInvNo.Text);
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

        private void CmbConsigneeName_Leave(object sender, EventArgs e)
        {
            try
            {
              
                    if (Convert.ToInt32(CmbConsigneeName.SelectedValue) != 0)
                    {
                        var State = (from c in db.Supplier_informations
                                     where c.ID == Convert.ToInt32(CmbConsigneeName.SelectedValue.ToString())
                                     select new { c.Address_1, c.Address_2, c.City, c.State, c.GSTIN_NO }).ToList();
                        if (State.Count > 0)
                        {
                            txtConGSTNo.Text = State[0].GSTIN_NO;
                            txtConAddress.Text = State[0].Address_1 + ", " + State[0].Address_2 + ", " + State[0].City + ", " + State[0].State;
                        }
                    if (txtCustPoNo.Text != "Multi")
                    {
                        //Bind Products
                        int CustID = Convert.ToInt32(CmbConsigneeName.SelectedValue.ToString());
                        DataTable dt = new DataTable();
                        //String conStr = "Data Source=172.16.6.173;Initial Catalog=servion_hari;User  ID=sa;Password=Servion@123";
                        //SqlConnection con = new SqlConnection(con);
                        SqlCommand com = new SqlCommand("PendingOrderInfo", con);
                        com.Parameters.AddWithValue("@compname", logIn.company);
                        com.Parameters.AddWithValue("@CustPo", txtSoNo.Text);
                        com.Parameters.AddWithValue("@CustName", CustID);
                        com.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(com);

                        con.Open();
                        da.Fill(dt);

                        if (dt.Rows.Count > 0)
                        {
                            dgProducts.DataSource = dt;
                            for (int i = 0; i < dgProducts.Rows.Count; i++)
                            {

                                //Get Stock Report
                                DateTime t = dpInvDate.Value;
                                string dt1 = t.ToString("yyyy/MM/dd");
                                var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(Item_Code.ToString()), Convert.ToDateTime(dt1), logIn.BU_ID) select data).ToList();

                                if (stock.Count > 0)
                                {
                                    //dgProductsList.DataSource = d;
                                    dgProducts.Rows[i].Cells["Stock_Qty"].Value = stock[0].ClosingQty;
                                }

                            }

                        }
                        else
                        {
                            MessageBox.Show("No Pending / Approved Orders On Selected Customer / PO No");
                            dgProducts.DataSource = dt;
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
                 if (con.State == ConnectionState.Open)
                      con.Close();
            }
        }

        private void txtFrieght_Leave(object sender, EventArgs e)
        {
            try
            {
                GetTot();
                
                //decimal v=0, cg=0, sg=0, ig=0,totA=0;
                //decimal fAmt = (txtFrieght.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtFrieght.Text);
                //decimal OthAmt = (txtOtherCharges.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtOtherCharges.Text);
                //v = Convert.ToDecimal(txtTot_TaxableValue.Text);
                //cg = Convert.ToDecimal(txtTot_CGST.Text);
                //sg = Convert.ToDecimal(txtTot_SGST.Text);
                //ig = Convert.ToDecimal(txtTot_IGST.Text);
                //totA = v + cg + sg + ig;


                //decimal AmtForTCs = (fAmt + OthAmt + v + cg + sg + ig);
                //decimal tcsPer = (txtTCSPer.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtTCSPer.Text);

                //decimal TcsAmt = AmtForTCs * tcsPer / 100;
                //txtTCSAmt.Text = TcsAmt.ToString(".00");
                //decimal rndAmt = Convert.ToDecimal(txtRounding.Text);
                //txtTot_InvValue.Text = (totA + TcsAmt + fAmt + OthAmt + rndAmt).ToString(".00");


            }
            catch
            {

            }
        }

        private void txtOtherCharges_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtOtherCharges_Leave(object sender, EventArgs e)
        {
            try
            {
                GetTot();
            }
            catch
            {

            }
        }

        private void txtTCSPer_Leave(object sender, EventArgs e)
        {
            try
            {
                GetTot();

            }
            catch
            {

            }
        }

        private void dgProducts_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                if (CmbBuyerName.Text == "")
                {
                    MessageBox.Show("Select Customer Name To Proceed");
                }
                else
                   if (txtCustGSTNo.Text == "")
                {
                    MessageBox.Show("Customer GST No Required To Proceed");
                }
                else
                      if (cmbTaxClass.Text == "")
                {
                    MessageBox.Show("Select Tax Class To Proceed");
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {

              
                var p = (from obj in db.Sale_Order_Masters
                         join C in db.Customer_informations on obj.BuyerName equals C.ID
                          where obj.Status == 6 && obj.Company_ID == logIn.company && SqlMethods.Like(C.Customer_Name, "%" + txtSearch.Text + "%")
                          
                             select new
                         {
                             C.Customer_Name,
                             obj.CustomerPONo,
                             obj.SO_NO


                         }
                        );
                SqlCommand cmd1 = (SqlCommand)db.GetCommand(p);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);

                if (dt1.Rows.Count > 0)
                {
                    sfDataGrid1.DataSource = dt1;
                }
                else
                {
                    MessageBox.Show("No Orders Found on Entered Customer");
                    sfDataGrid1.DataSource = dt1;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmdPrevOrder_Click(object sender, EventArgs e)
        {
            var d = (from data in db.ShowSOList_Invoicing(logIn.company, Convert.ToInt32(CmbBuyerName.SelectedValue),logIn.BU_ID) select data).ToList();

            if (d.Count > 0)
            {
                //dgProductsList.DataSource = d;
                sfDataGrid1.DataSource = d;
            }

            groupBox2.Visible = true;
            txtSearch.Focus();
        }
        System.Data.DataRow drgetproducts;
        DataTable dtexisting = new DataTable();
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                
                dtgetproducts.Columns.Clear();
                dtgetproducts.Rows.Clear();
                dtgetproducts.Columns.Add("Item_Code", typeof(string));
                dtgetproducts.Columns.Add("Item_Description", typeof(string));
                dtgetproducts.Columns.Add("Item_Grade", typeof(string));
                dtgetproducts.Columns.Add("UOM", typeof(string));
                dtgetproducts.Columns.Add("PO_Qty", typeof(decimal));
                dtgetproducts.Columns.Add("Stock_Qty", typeof(decimal));
                dtgetproducts.Columns.Add("Inv_Qty", typeof(decimal));
                dtgetproducts.Columns.Add("Basic_Price", typeof(decimal));
                dtgetproducts.Columns.Add("Amt_Before_Disc", typeof(decimal));
                dtgetproducts.Columns.Add("Disc_Per", typeof(decimal));
                dtgetproducts.Columns.Add("Disc_Amt", typeof(decimal));
                dtgetproducts.Columns.Add("Taxable_Value", typeof(decimal));
                //dtgetproducts.Columns.Add("CGST_Per", typeof(decimal));
                //dtgetproducts.Columns.Add("CGST_Amt", typeof(decimal));
                //dtgetproducts.Columns.Add("SGST_Per", typeof(decimal));
                //dtgetproducts.Columns.Add("SGST_Amt", typeof(decimal));
                //dtgetproducts.Columns.Add("IGST_Per", typeof(decimal));
                //dtgetproducts.Columns.Add("IGST_Amt", typeof(decimal));
                dtgetproducts.Columns.Add("Total_Amount", typeof(decimal));
                dtgetproducts.Columns.Add("Remarks", typeof(string));
                dtgetproducts.Columns.Add("SO_Ref_No", typeof(string));
                dtgetproducts.Columns.Add("Item_No", typeof(string));
                dtgetfinalprducts.Rows.Clear();
                //listBox.Items.Clear();
                // Get the selected items of SfDataGrid
                //var reflector = this.sfDataGrid1.View.GetPropertyAccessProvider();
                //var row = this.sfDataGrid1.SelectedItem;

                //string ProdCode;
                //string SoNo;
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
                        var ProdCodeCol = sfDataGrid1.Columns[1].MappingName;
                        var SONoCol = sfDataGrid1.Columns[0].MappingName;
                        //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                        //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                        if (rowData == item)
                        {
                            var Item_Code = (rowData.GetType().GetProperty("Prod_Code").GetValue(rowData, null).ToString());
                            var Item_Description = (rowData.GetType().GetProperty("Product_Description").GetValue(rowData, null).ToString());
                            var UOM = (rowData.GetType().GetProperty("Uom").GetValue(rowData, null).ToString());
                            var PO_Qty = (rowData.GetType().GetProperty("BalQty").GetValue(rowData, null).ToString());
                            var Basic_Price = (rowData.GetType().GetProperty("Price").GetValue(rowData, null).ToString());
                            var SO_Ref_No = (rowData.GetType().GetProperty("SO_NO").GetValue(rowData, null).ToString());
                            drgetproducts = dtgetproducts.NewRow();
                            drgetproducts["Item_Code"] = Item_Code.ToString();
                            drgetproducts["Item_Description"] = Item_Description.ToString();
                            var PGrade = (from data in db.Sale_Order_Childs                          
                                         where data.SO_NO == SO_Ref_No && data.Prod_Code == Convert.ToInt32(Item_Code) && data.Company_ID ==logIn.company  select data).ToList();

                            if (PGrade.Count > 0)
                            {
                                //dgProductsList.DataSource = d;
                                drgetproducts["Item_Grade"] = PGrade[0].Prod_Grade;
                                //drgetproducts["CGST_Per"] = PGrade[0].CGST_Per;
                                //drgetproducts["CGST_Amt"] = 0;
                                //drgetproducts["SGST_Per"] = PGrade[0].SGST_Per; 
                                //drgetproducts["SGST_Amt"] = 0;
                                //drgetproducts["IGST_Per"] = PGrade[0].IGST_Per;
                                //drgetproducts["IGST_Amt"] = 0;
                            }                           
                            drgetproducts["UOM"] = UOM.ToString();
                            drgetproducts["PO_Qty"] = PO_Qty.ToString();
                            DateTime t = dpInvDate.Value;
                            string dt1 = t.ToString("yyyy/MM/dd");
                            var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(Item_Code.ToString()), Convert.ToDateTime(dt1),  logIn.BU_ID) select data).ToList();

                            if (stock.Count > 0)
                            {
                                //dgProductsList.DataSource = d;
                                drgetproducts["Stock_Qty"] = stock[0].ClosingQty;
                            }
                            //drgetproducts["Stock_Qty"] =0;
                            drgetproducts["Inv_Qty"] = 0;
                            drgetproducts["Basic_Price"] = Basic_Price.ToString();
                            drgetproducts["Disc_Per"] = 0;
                            drgetproducts["Disc_Amt"] = 0;
                            drgetproducts["Taxable_Value"] = 0;
                            drgetproducts["Total_Amount"] = 0;
                            drgetproducts["Remarks"] = "";
                            drgetproducts["SO_Ref_No"] = SO_Ref_No.ToString();
                            drgetproducts["Item_No"] = "";
                            dtgetproducts.Rows.Add(drgetproducts);
                            dtgetfinalprducts = dtgetfinalprducts.AsEnumerable().Union(dtgetproducts.AsEnumerable()).CopyToDataTable();
                            //}
                            //}
                            dtgetproducts.Rows.Clear();
                        }
                    }
                }
                //dtexisting = dtexisting.AsEnumerable().Union(dtgetfinalprducts.AsEnumerable()).CopyToDataTable();
                //dgProducts.DataSource = dtexisting;
                txtCustPoNo.Text = "Multi";
                txtSoNo.Text = "Multi";
                
                dgProducts.DataSource = dtgetfinalprducts;
                CmbConsigneeName.Text = CmbBuyerName.Text;
                CmbConsigneeName.Focus();
                groupBox2.Visible = false;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgPOSearch_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgPOSearch_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //int i = dgPOSearch.CurrentRow.Index;
            //txtCustPoNo.Text = dgPOSearch.Rows[i].Cells["CustomerPONo"].Value.ToString();
            txtCustPoNo.Focus();
            groupBox2.Visible = false;        }

        private void dgProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgProducts.Rows[dgProducts.CurrentRow.Index].Cells[dgProducts.CurrentCell.ColumnIndex].Value == "Remove")
                {
                    if (dgProducts.Rows.Count > 0)
                    {
                        //if ((from u in db.Invoice_Childs
                        //     where u.Inv_No == txtInvNo.Text && u.Prod_Code == dgProducts.Rows[dgProducts.CurrentRow.Index].Cells["Item_Code"].Value && u.Company_ID == logIn.company
                        //     select u).Count() > 0)
                        //{


                        //    //db.de(txtInvoiceno.Text, dgvInvoice.Rows[dgvInvoice.CurrentRow.Index].Cells["Item_Code"].Value.ToString(), Creation_Company);

                        //}
                        //else
                        //{
                            foreach (DataGridViewCell oneCell in dgProducts.SelectedCells)
                            {
                                if (oneCell.Selected)
                                dgProducts.Rows.RemoveAt(oneCell.RowIndex);
                            }
                        //}
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
                        decimal fAmt = (txtFrieght.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtFrieght.Text);
                        decimal OthAmt = (txtOtherCharges.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtOtherCharges.Text);

                        txtTot_TaxableValue.Text = (v).ToString(".00");

                        txtTot_CGST.Text = cg.ToString(".00");
                        txtTot_SGST.Text = sg.ToString(".00");
                        txtTot_IGST.Text = ig.ToString(".00");
                        decimal AmtForTCs = (fAmt + OthAmt + v + cg + sg + ig);
                        decimal tcsPer = (txtTCSPer.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtTCSPer.Text);

                        decimal TcsAmt = AmtForTCs * tcsPer / 100;
                        txtTCSAmt.Text = TcsAmt.ToString(".00");
                        decimal rndAmt = (txtRounding.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtRounding.Text);
                        txtTot_InvValue.Text = (totA + TcsAmt + fAmt + OthAmt + rndAmt).ToString(".00");


                    }
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtFrieght_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtRounding_Leave(object sender, EventArgs e)
        {
            GetTot();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void txtTCSPer_Layout(object sender, LayoutEventArgs e)
        {

        }

        private void dpInvDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void CmbBuyerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string t = CmbBuyerName.Text;

                if (t != null)
                {
                    //    if (CmbBuyerName.SelectedValue != null)
                    //{
                    var State = (from c in db.Supplier_informations
                                 where c.Supplier_Name == t && c.Company_ID == logIn.company //Convert.ToInt32(CmbBuyerName.SelectedValue.ToString())
                                 select new { c.GSTIN_NO,c.StateCode }).ToList();
                    if (State.Count > 0)
                    {
                        txtCustGSTNo.Text = State[0].GSTIN_NO;
                        txtCustStateCode.Text = State[0].StateCode;
                    }
                }
                else
                {
                    MessageBox.Show("Customer Name Not Selected");
                }



            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        public void bindedit()
        {
            try
            {
                if (OrderManagement.Transactions.ListOfProformaInvoices.SO_No != null)
                {
                    txtInvNo.Text = OrderManagement.Transactions.ListOfProformaInvoices.SO_No;
                }
                else
                {
                    txtInvNo.Text = frmCRMDashBoard.SO_No;
                }
                String myString = "";
                myString = txtInvNo.Text;
                var da = (from obj in db.Proforma_Invoice_Masters
                          where obj.Inv_No == txtInvNo.Text && obj.Company_ID == logIn.company && obj.BU_ID == logIn.BU_ID
                          select obj).ToList();

                if (da.Count > 0)
                {
                    txtInvNo.Text = da[0].Inv_No.ToString();
                    dpInvDate.Text = da[0].InvDate.ToString();                    

                    cmbTaxClass.SelectedValue = da[0].Tax_Class;
                    //bindCustomer();
                    CmbBuyerName.SelectedValue = da[0].BuyerName;
                    bindConsignee();
                    CmbConsigneeName.SelectedValue = da[0].ConsigneeName;
                    txtCustGSTNo.Text = da[0].Cust_GST_No;
                    txtConAddress.Text  = da[0].ConsigneeAddress;
                    txtConGSTNo.Text = da[0].Con_GST_No;
                    txtCustPoNo.Text = da[0].CustomerPONo;
                    dpPODate.Text = da[0].PODate.ToString();
                                   
                    //cmbCustomer.Enabled = false;
                    txtTotalQty.Text = da[0].TotalQty.ToString();
                    txtSubTotal.Text = da[0].SubTotal.ToString();
                    txtTotDiscount.Text = da[0].Tot_Discount.ToString();
                    txtTot_TaxableValue.Text = da[0].Tot_TaxableValue.ToString();
                    txtTot_CGST.Text = da[0].Tot_CGST_Amnt.ToString();
                    txtTot_SGST.Text = da[0].Tot_SGST_Amnt.ToString();
                    txtTot_IGST.Text = da[0].Tot_IGST_Amnt.ToString();
                    txtTot_InvValue.Text = da[0].Tot_Inv_Value.ToString();
                    txtFrieght.Text = da[0].Frieght_amnt.ToString();
                    txtOtherCharges.Text = da[0].Other_Charges.ToString();
                    txtTCSPer.Text = da[0].TCS_Per.ToString();
                    txtTCSAmt.Text = da[0].TCS_Amnt.ToString();
                    txtRounding.Text = da[0].Rounding.ToString();                    
                    txtSplInstructions.Text = da[0].Spl_Instructions;                    
                    cmbStatus.SelectedValue = da[0].Status;      
                    lblCreatedBy.Text = da[0].Created_By;
                    lblModified.Text = da[0].Modified_By;
                    txtSoNo.Text = da[0].SO_Ref_No;
                                        

                }


                var dm1 = (from s in db.Proforma_Invoice_Childs
                           where s.Inv_No == myString && s.Company_ID == logIn.company orderby s.Item_No


                           select new

                           {
                               Item_Code =s.Prod_Code,
                               Item_Description =s.Product_Description,
                               Item_Grade =s.Prod_Grade,
                               UOM=s.Uom,
                               s.PO_Qty,                             
                               Inv_Qty = s.Qty,
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
                               s.Remarks,
                               SO_Ref_No= s.SO_Ref_No,
                               Item_No = s.Item_No



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
        public void GetOrderInfo()
        {
            try
            {
                //txtSoNo.Text = OrderManagement.Transactions.ListOfOrders.SO_No;
                String myString = "";
                
                var da = (from obj in db.Sale_Order_Masters
                          where obj.CustomerPONo == txtCustPoNo.Text && obj.Company_ID == logIn.company && obj.Status==6
                          select obj).ToList();

                if (da.Count > 0)
                {
                    SONo =  da[0].SO_NO.ToString();
                    txtSoNo.Text = da[0].CustomerPONo;
                    cmbTaxClass.SelectedValue = da[0].Tax_Class;
                    //dpSODate.Text = da[0].SODate.ToString();
                    if (da[0].Multi_Loc_Delivery == true)
                    {
                        bindDeliveryAddress();
                    }
                    else
                    {
                        //bindCustomer();
                        CmbBuyerName.SelectedValue = da[0].BuyerName;
                        bindConsignee();
                        CmbConsigneeName.SelectedValue = da[0].ConsigneeName;
                        txtCustGSTNo.Text = da[0].Cust_GST_No;
                        
                    }
                    //txtAmendNo.Text = da[0].So_Amend_No;
                    //dtAmendDate.Text = da[0].So_Amend_Date.ToString();
                    

                    //txtTotalQty.Text = da[0].TotalQty.ToString();
                    //txtSubTotal.Text = da[0].SubTotal.ToString();
                    //txtTotDiscount.Text = da[0].Tot_Discount.ToString();
                    //txtTot_TaxableValue.Text = da[0].Tot_TaxableValue.ToString();
                    //txtTot_CGST.Text = da[0].Tot_CGST_Amnt.ToString();
                    //txtTot_SGST.Text = da[0].Tot_SGST_Amnt.ToString();
                    //txtTot_IGST.Text = da[0].Tot_IGST_Amnt.ToString();
                    //txtTot_InvValue.Text = da[0].Tot_Ord_Value.ToString();
                    dpPODate.Text = da[0].PODate.ToString();
                    
                    
                }
                else
                {
                    MessageBox.Show("Invalid SO / PO No Enterered, Cannot Proceed");
                    txtCustPoNo.Focus();
                    return;

                }


                
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
