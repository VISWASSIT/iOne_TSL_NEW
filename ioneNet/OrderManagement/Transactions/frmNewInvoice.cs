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
using Syncfusion.WinForms.DataGrid.Enums;
using Newtonsoft.Json.Linq;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using OpenCvSharp.CPlusPlus;
using Syncfusion.XPS;

namespace ioneNet.OrderManagement.Transactions
{
    public partial class frmNewInvoice : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();
        public static string SONo,ItemCode,OrdQty;
        decimal taxRate = 0;
        decimal cgstPer,sgstPer,igstPer;
        public static string DocNo, ItemCode_Issue, RecQty, Suppname;

        public frmNewInvoice()
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
            textBox1.Text = DateTime.Now.ToString("hh:mm");
            textBox2.Text = DateTime.Now.ToString("hh:mm");
            txtInvNo.Enabled = false;


            AutoincrementId();
            if (ListOfInvoices.editMode == true || frmCRMDashBoard.editMode == true)
            {
                bindedit();
            }
            else
            {
                if (logIn.company == 1047 || logIn.company == 1042)
                {
                    txtInvNo.Enabled = true;
                }
                
                else
                { 
                    //AutoincrementId();
                }

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
                //Check Invoice Qty >0
                Boolean recval = false;
                for (int i = 0; i < dgProducts.RowCount - 1; i++)
                {
                    if (dgProducts.Rows[i].Cells["Item_Code"].Value != null)
                    {
                        //if (dgProducts.Rows[i].Cells["Prod_Code"].Value.ToString() == "S" && dgProducts.Rows[i].Cells["Prod_Code"].Value.ToString() != null)
                        //{
                        //}
                        //else
                        //{ 

                        //    double amt = Convert.ToDouble(dgProducts.Rows[i].Cells["Inv_Qty"].Value);
                        //    if (amt > 0)
                        //    {
                        //        recval = true;
                        //    }
                        //    else
                        //    {
                        //        recval = false;
                        //    }
                        //}

                    }
                }

                if (cmbInvType.Text == string.Empty)
                {
                    MessageBox.Show("Inv Type Should Not Be Empty", "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbInvType.Focus();
                    return;
                }
                if (CmbBuyerName.Text == string.Empty)
                {
                    MessageBox.Show("Customer Name Should Not Be Empty", "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CmbBuyerName.Focus();
                    return;
                }
                //else if (txtInvNo.Text == string.Empty)
                //{
                //    MessageBox.Show("Invoice No Should Cannot Be Blank");
                //    txtInvNo.Focus();
                //    return;
                //}
                else if (txtCustPoNo.Text == string.Empty)
                {
                    MessageBox.Show("Customer PO No Should Not Be Empty");
                    txtCustPoNo.Focus();
                    return;
                }
                else if (cmbInvType.Text == string.Empty)
                {
                    MessageBox.Show("Select Invoice Type,");
                    cmbInvType.Focus();
                    return;
                }
                else if (cmbStatus.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Status");
                    cmbStatus.Focus();
                    return;
                }
                else if (cmbTCS.Text == string.Empty)
                {
                    decimal TotTO = Convert.ToDecimal(linkLabel2.Text);
                    if (TotTO > 5000000)
                    {
                        MessageBox.Show("Sale Turnover to this Customer is >50.0 Lacs, Need To Deduct TCS, Select NA to Proceed without Deduction");
                        
                    }
                    else                     
                        
                    {
                        MessageBox.Show("Please Select Wether TCS Applicable or Not");
                        cmbTCS.Focus();
                        return;
                    }
                    
                    

                }
                else if (cmbTaxClass.Text == string.Empty)
                {
                    MessageBox.Show("Tax Class Cannot be Empty,");
                    cmbTaxClass.Focus();
                    return;
                }
                else if (dgProducts.Rows[0].Cells["Item_Code"].Value == null)
                {
                    MessageBox.Show("Atleast One Record To Be Entered To Proceed", "Order Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
               
                //else if (recval == false)
                //{
                //    //MessageBox.Show("Invoice Qty Should Be Greater Than 0 for all the products", "Order Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    //return;
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
                    CmbBuyerName.SelectedIndex = -1;

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
                CmbConsigneeName.SelectedIndex = -1;

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

                //Sales Account
                //var d = (from po in db.AccountMasters
                //         join A in db.AccountGroups on po.AccGroup_ID equals A.ID
                //         where po.Company_ID == logIn.company && A.GroupType =="Income" 
                //select new { po.id, po.AccName }).Distinct().ToList();
                //if (d.Count > 0)
                //{
                //    cmbSaleAccount.DataSource = d;
                //    cmbSaleAccount.ValueMember = "id";
                //    cmbSaleAccount.DisplayMember = "AccName";
                //}


                //Bind Transporter
                //Sales Account
                var t1 = (from po in db.Supplier_informations                       
                         where po.Company_ID == logIn.company && po.Supplier_Category == 29
                         select new { po.ID, po.Supplier_Name }).Distinct().ToList();
                if (t1.Count > 0)
                {
                    cmbTransporter.DataSource = t1;
                    cmbTransporter.ValueMember = "ID";
                    cmbTransporter.DisplayMember = "Supplier_Name";
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
                if (logIn.company == 1044)
                {
                    var result = db.Sp_autoincrement_Invoice(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, cmbInvType.Text, logIn.BU_ID);
                    txtInvNo.Text = result.FirstOrDefault().Inv_No;

                }
                else
                {
                    var getSufix = (from m in db.Financial_Year_Masters where m.Company_ID == logIn.company && m.Start_Date == logIn.fy_Start_Date select new { m.Uses_AsSufix }).Distinct().ToList();
                    if (getSufix.Count > 0)
                    {
                        if (getSufix[0].Uses_AsSufix == true)
                        {
                            var result = db.Sp_autoincrement_Invoice_Suffix(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date);
                            txtInvNo.Text = result.FirstOrDefault().Inv_No;
                        }
                        else
                        {
                            var result = db.Sp_autoincrement_Invoice_Prefix(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, logIn.BU_ID);
                            txtInvNo.Text = result.FirstOrDefault().Inv_No;
                        }
                    }
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
                    var Prodname = (from d in db.UoM_Masters where d.Company_ID  ==logIn.company select new { d.Uom_Descr }).ToList();
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
                if (columnName == "Item Description")
                {
                    var Prodname = (from d in db.Products where d.Company_ID == logIn.company select new { d.Prod_Name }).ToList();
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

        private void CmbBuyerName_Leave(object sender, EventArgs e)
        {
            //try
            //{
            //    string t = CmbBuyerName.Text;

            //    if (t != null)
            //    {
            //        //    if (CmbBuyerName.SelectedValue != null)
            //        //{
            //        var State = (from c in db.Supplier_informations
            //                     where c.Supplier_Name == t && c.Company_ID == logIn.company //Convert.ToInt32(CmbBuyerName.SelectedValue.ToString())
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
                            //ask for permission
                            if (oneCell.Selected)
                            {
                                DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                                if (result == DialogResult.Yes)
                                {
                                    int i = dgProducts.CurrentCell.RowIndex;
                                    SONo = txtInvNo.Text;
                                    ItemCode = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                                    SqlCommand cmd1 = new SqlCommand("delete  from [Invoice_Child] where [Prod_Code] =@ProdID and [Inv_No] = @pono", con);
                                    cmd1.Parameters.AddWithValue("@ProdID", ItemCode);
                                    cmd1.Parameters.AddWithValue("@pono", SONo);
                                    if (con.State != ConnectionState.Open)
                                        con.Open();
                                    //con.Open();
                                    cmd1.ExecuteNonQuery();
                                    con.Close();
                                    if (oneCell.Selected)
                                        dgProducts.Rows.RemoveAt(oneCell.RowIndex);
                                    break;
                                }
                            }
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
                        decimal OthAmt = (txtPacking.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtPacking.Text);

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
                if (e.KeyCode == Keys.F3)
                {
                    if (logIn.company == 1044)
                    {
                        
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
                string pcode="";
                decimal toleQty = 0;
                if (columnName == "Item_Description")
                {
                    R1.Cells["Item_Code"].Value = dgProducts.CurrentCell.RowIndex + 1;

                    decimal b, c, d;
                    //c = dgProducts.Rows[dgProducts.CurrentRow.Index];
                    if (txtCustPoNo.Text == "NA")
                    {

                        if (columnName == "Item_Description" && R1.Cells["Item_Description"].Value != null)
                        {
                            pcode = R1.Cells["Item_Description"].Value.ToString();
                        }
                        if (pcode != "")
                        {
                            var prod = (from data in db.Get_Product_into_Trans(logIn.company, logIn.BU_ID, pcode)
                                        select


                                            new
                                            {
                                                Item_Code = data.prod_id,
                                                data.Prod_Code,
                                                Item_Description = data.Prod_Name,
                                                UOM = data.Uom_Descr,
                                                data.Prod_Field2,
                                                data.Gst_Rate
                                            }).ToList();


                            R1.Cells["UOM"].Value = prod[0].UOM.ToString();
                            R1.Cells["Item_Code"].Value = prod[0].Item_Code.ToString();
                            R1.Cells["Prod_Code"].Value = prod[0].Prod_Code.ToString();
                            R1.Cells["Item_Description"].Value = prod[0].Item_Description.ToString();
                            R1.Cells["PO_Qty"].Value = "0";
                            R1.Cells["Disc_Per"].Value = "0";

                            DateTime t = dpInvDate.Value;
                            string dt1 = t.ToString("yyyy/MM/dd");
                            var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(R1.Cells["Item_Code"].Value.ToString()), Convert.ToDateTime(dt1), logIn.BU_ID) select data).ToList();
                            R1.Cells["Stock_Qty"].Value = "0";
                            if (stock.Count > 0)
                            {
                                //dgProductsList.DataSource = d;
                                R1.Cells["Stock_Qty"].Value = stock[0].ClosingQty;

                                //decimal cValue = Convert.ToDecimal(stock[0].ClosingValue);
                                //decimal cStock = Convert.ToDecimal(stock[0].ClosingQty);
                                 R1.Cells["Stock_Price"].Value = stock[0].CBPrice;
                            }


                            taxRate = Convert.ToDecimal(prod[0].Gst_Rate);
                            var cu = (from a in db.Costing_Units where a.id == logIn.BU_ID select new { a.GST_No, a.State_Code }).ToList();
                            if (cu.Count > 0)
                            {
                                comnpstatecode = cu[0].State_Code;
                                suppStateCode = txtCustStateCode.Text;

                                if (cmbInvType.Text == "Common Invoice" || cmbInvType.Text == "Service Invoice" || cmbInvType.Text == "Trading Invoice")
                                {
                                    if (suppStateCode == comnpstatecode)
                                    {
                                        d = Convert.ToDecimal(taxRate) / 2;
                                        R1.Cells["CGST_Per"].Value = d.ToString("0.00");
                                        R1.Cells["SGST_Per"].Value = d.ToString("0.00");
                                        R1.Cells["IGST_Per"].Value = "0.00";
                                        cgstPer = d;
                                        sgstPer = d;
                                    }
                                    else
                                    {
                                        d = taxRate;
                                        R1.Cells["CGST_Per"].Value = "0.00";
                                        R1.Cells["SGST_Per"].Value = "0.00";
                                        R1.Cells["IGST_Per"].Value = d.ToString("0.00");
                                        igstPer = d;
                                    }
                                }
                                else
                                {
                                    if (cmbInvType.Text == "Deemed Export")
                                    {
                                        if (suppStateCode == comnpstatecode)
                                        {
                                            // d = 0.1 / 2;
                                            R1.Cells["CGST_Per"].Value = "0.05";
                                            R1.Cells["SGST_Per"].Value = "0.05";
                                            R1.Cells["IGST_Per"].Value = "0.00";
                                            cgstPer = Convert.ToDecimal(R1.Cells["CGST_Per"].Value);
                                            sgstPer = Convert.ToDecimal(R1.Cells["SGST_Per"].Value);
                                        }
                                        else
                                        {
                                            //d =0.1;
                                            R1.Cells["CGST_Per"].Value = "0.00";
                                            R1.Cells["SGST_Per"].Value = "0.00";
                                            R1.Cells["IGST_Per"].Value = "0.10";
                                            sgstPer = Convert.ToDecimal(R1.Cells["IGST_Per"].Value);

                                        }

                                    }
                                    else
                                    {
                                        if (cmbInvType.Text == "SEZ Invoice" || cmbInvType.Text == "Bill of Supply")
                                        {
                                            R1.Cells["CGST_Per"].Value = "0.00";
                                            R1.Cells["SGST_Per"].Value = "0.00";
                                            R1.Cells["IGST_Per"].Value = "0.00";
                                            cgstPer = Convert.ToDecimal(R1.Cells["CGST_Per"].Value);
                                            sgstPer = Convert.ToDecimal(R1.Cells["SGST_Per"].Value);
                                            igstPer = Convert.ToDecimal(R1.Cells["IGST_Per"].Value);
                                        }
                                    }
                                }
                            }

                        }
                       
                    }
                     
                    var State = (from c1 in db.Tax_Class_Masters
                                 where c1.ID == Convert.ToInt32(cmbTaxClass.SelectedValue)
                                 select new { c1.Gst_Rate }).ToList();
                    if (State.Count > 0)
                    {

                        taxRate = Convert.ToDecimal(State[0].Gst_Rate);
                    }
                    else
                    {
                        string SO_Ref_No = R1.Cells["SO_Ref_No"].Value.ToString();
                        var PGrade = (from data in db.Sale_Order_Childs where data.SO_NO == SO_Ref_No && data.Prod_Code == Convert.ToInt32(R1.Cells["Item_Code"].Value) && data.Company_ID == logIn.company select data).ToList();

                        if (PGrade.Count > 0)
                        {
                            //dgProductsList.DataSource = d;
                            toleQty = Convert.ToDecimal(PGrade[0].Tole_Qty);  
                            if (PGrade[0].Prod_Grade != null)
                            {
                                drgetproducts["Item_Grade"] = PGrade[0].Prod_Grade.Trim();
                            }
                            if (cmbInvType.Text == "SEZ Invoice" || cmbInvType.Text == "Bill of Supply")
                            {
                                drgetproducts["Basic_Price"] = PGrade[0].Price;
                                drgetproducts["SGST_Per"] = "0.00";
                                drgetproducts["SGST_Per"] = "0.00";
                                drgetproducts["IGST_Per"] = "0.00";
                                cgstPer = Convert.ToDecimal(drgetproducts["CGST_Per"]);
                                sgstPer = Convert.ToDecimal(drgetproducts["SGST_Per"]);
                                igstPer = Convert.ToDecimal(drgetproducts["IGST_Per"]);
                            }
                            else
                            {
                                drgetproducts["Basic_Price"] = PGrade[0].Price;
                                drgetproducts["Disc_Per"] = PGrade[0].Disc_Per;
                                drgetproducts["CGST_Per"] = PGrade[0].CGST_Per;
                                drgetproducts["CGST_Amt"] = 0;
                                drgetproducts["SGST_Per"] = PGrade[0].SGST_Per;
                                drgetproducts["SGST_Amt"] = 0;
                                drgetproducts["IGST_Per"] = PGrade[0].IGST_Per;
                                drgetproducts["IGST_Amt"] = 0;
                            }
                        }
                        else
                        {
                            taxRate = 18;
                        }
                    }
                     //Convert.ToInt32(getProduct_Name.GSTRate);
                    
                    var d1 = (from a in db.Costing_Units where a.id == logIn.BU_ID select new { a.GST_No,a.State_Code }).ToList();
                        if (d1.Count > 0)
                        {
                            comnpstatecode = d1[0].State_Code;
                            suppStateCode = txtCustStateCode.Text;

                            if (cmbInvType.Text == "Common Invoice" || cmbInvType.Text == "Service Invoice")
                            {
                                if (suppStateCode == comnpstatecode)
                                {
                                    d = Convert.ToDecimal(taxRate) / 2;
                                    R1.Cells["CGST_Per"].Value = d.ToString("0.00");
                                    R1.Cells["SGST_Per"].Value = d.ToString("0.00");
                                    R1.Cells["IGST_Per"].Value = "0.00";
                                    cgstPer = d;
                                    sgstPer = d;
                                }
                                else
                                {
                                    d = taxRate;
                                    R1.Cells["CGST_Per"].Value = "0.00";
                                    R1.Cells["SGST_Per"].Value = "0.00";
                                    R1.Cells["IGST_Per"].Value = d.ToString("0.00");
                                    igstPer = d;
                                }
                            }
                            else
                            {
                                if (cmbInvType.Text == "Deemed Export")
                                {
                                    if (suppStateCode == comnpstatecode)
                                    {
                                        // d = 0.1 / 2;
                                        R1.Cells["CGST_Per"].Value = "0.05";
                                        R1.Cells["SGST_Per"].Value = "0.05";
                                        R1.Cells["IGST_Per"].Value = "0.00";
                                        cgstPer = Convert.ToDecimal(R1.Cells["CGST_Per"].Value);
                                        sgstPer = Convert.ToDecimal(R1.Cells["SGST_Per"].Value);
                                    }
                                    else
                                    {
                                        //d =0.1;
                                        R1.Cells["CGST_Per"].Value = "0.00";
                                        R1.Cells["SGST_Per"].Value = "0.00";
                                        R1.Cells["IGST_Per"].Value = "0.10";
                                        sgstPer = Convert.ToDecimal(R1.Cells["IGST_Per"].Value);

                                    }

                                }
                                else
                                {
                                    if (cmbInvType.Text == "SEZ Invoice" || cmbInvType.Text == "Bill of Supply")
                                    {
                                        R1.Cells["CGST_Per"].Value = "0.00";
                                        R1.Cells["SGST_Per"].Value = "0.00";
                                        R1.Cells["IGST_Per"].Value = "0.00";
                                        cgstPer = Convert.ToDecimal(R1.Cells["CGST_Per"].Value);
                                        sgstPer = Convert.ToDecimal(R1.Cells["SGST_Per"].Value);
                                        igstPer = Convert.ToDecimal(R1.Cells["IGST_Per"].Value);
                                    }
                                }
                            }
                        }

                }
                if (R1.Cells["Item_Description"].Value != null)
                {
                    decimal POQty = (R1.Cells["PO_Qty"].Value == "" || R1.Cells["PO_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["PO_Qty"].Value);
                    decimal Stock_Qty = (R1.Cells["Stock_Qty"].Value == "" || R1.Cells["Stock_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Stock_Qty"].Value);
                    decimal Amt, DiscAmt, netAmt, gst, igst, totamt;
                    string Custstatetcode;
                    if (columnName == "Amt_Before_Disc")
                    {
                        Amt = Convert.ToDecimal(R1.Cells["Amt_Before_Disc"].Value);
                        DiscAmt = 0;
                        R1.Cells["Disc_Amt"].Value = DiscAmt.ToString("0.00");
                        netAmt = Amt - DiscAmt;
                        R1.Cells["Taxable_Value"].Value = netAmt.ToString("0.00");
                        gst = (Convert.ToDecimal(R1.Cells["Taxable_Value"].Value) * Convert.ToDecimal(R1.Cells["CGST_Per"].Value)) / 100;

                        R1.Cells["CGST_Amt"].Value = gst.ToString("0.00");
                        R1.Cells["SGST_Amt"].Value = gst.ToString("0.00");
                        igst = (Convert.ToDecimal(R1.Cells["Taxable_Value"].Value) * Convert.ToDecimal(R1.Cells["IGST_Per"].Value)) / 100;
                        R1.Cells["IGST_Amt"].Value = igst.ToString("0.00");

                        totamt = netAmt + gst + gst + igst;
                        R1.Cells["Total_Amount"].Value = totamt.ToString("0.00");
                        GetTot();

                    }


                        if (columnName == "Inv_Qty" || columnName == "Basic_Price")
                    {
                        
                        decimal AcceptedQty = (R1.Cells["Inv_Qty"].Value == "" || R1.Cells["Inv_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Inv_Qty"].Value);
                        if (AcceptedQty > POQty)
                        {

                            if (txtCustPoNo.Text != "NA")
                            {
                                if (AcceptedQty > (POQty + toleQty))
                                {
                                    MessageBox.Show("Invoice Qty Cannot Be Greater Than Order Qty");
                                    R1.Cells["Inv_Qty"].Value = "0";
                                    return;
                                }
                            }
                            
                        }

                        if (AcceptedQty > Stock_Qty && logIn.company == 20 && cmbInvType.Text != "Service Invoice")
                        {
                            MessageBox.Show("Invoice Qty Cannot Be Greater Than Stock Qty Qty");
                            R1.Cells["Inv_Qty"].Value = "0";
                            return;
                        }
                        else
                        {

                            decimal price = (R1.Cells["Basic_Price"].Value == "" || R1.Cells["Basic_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Basic_Price"].Value);
                            decimal DiscPer = (R1.Cells["Disc_Per"].Value == "" || R1.Cells["Disc_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Disc_Per"].Value);

                            //Get Tax rates
                            decimal b, c, d;

                            var getProductName = (from s in db.Products
                                                  join u in db.UoM_Masters on s.Prod_Primary_UOM_Id equals u.UOM_ID
                                                  join g in db.Product_Groups on s.Prod_Group_Id equals g.ID
                                                  join t in db.Tax_Class_Masters on s.Prod_Tax_Class equals t.ID
                                                  where s.Prod_Name == R1.Cells["Item_Description"].Value.ToString() && s.Company_ID == logIn.company
                                                  select new { s.prod_ID, u.Uom_Descr, g.Prod_Group_Name, s.Prod_HSN_Code, t.Gst_Rate }).FirstOrDefault();

                            if (getProductName != null)
                            {
                                //R1.Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                                //R1.Cells["Item_code"].Value = getProductName.prod_ID.ToString();
                                taxRate = Convert.ToDecimal(getProductName.Gst_Rate);
                            }
                            if (taxRate != 0)
                            {

                            }
                            else
                            {
                                string SO_Ref_No = R1.Cells["SO_Ref_No"].Value.ToString();
                                var PGrade = (from data in db.Sale_Order_Childs where data.SO_NO == SO_Ref_No && data.Prod_Code == Convert.ToInt32(R1.Cells["Item_Code"].Value) && data.Company_ID == logIn.company select data).ToList();

                                if (PGrade.Count > 0)
                                {
                                    //dgProductsList.DataSource = d;
                                    if (PGrade[0].Prod_Grade != null)
                                    {
                                        R1.Cells["Item_Grade"].Value = PGrade[0].Prod_Grade.Trim();
                                    }
                                    R1.Cells["Basic_Price"].Value = PGrade[0].Price.ToString();
                                    R1.Cells["Disc_Per"].Value = PGrade[0].Disc_Per.ToString();
                                    R1.Cells["CGST_Per"].Value = PGrade[0].CGST_Per.ToString();
                                    R1.Cells["CGST_Amt"].Value = 0;
                                    R1.Cells["SGST_Per"].Value = PGrade[0].SGST_Per.ToString();
                                    R1.Cells["SGST_Amt"].Value = 0;
                                    R1.Cells["IGST_Per"].Value = PGrade[0].IGST_Per.ToString();
                                    R1.Cells["IGST_Amt"].Value = 0;
                                    cgstPer = Convert.ToDecimal(R1.Cells["CGST_Per"].Value);
                                    sgstPer = Convert.ToDecimal(R1.Cells["SGST_Per"].Value);
                                    igstPer = Convert.ToDecimal(R1.Cells["IGST_Per"].Value);
                                    taxRate = cgstPer+sgstPer+igstPer;
                                }
                                else
                                {
                                    taxRate = 18;
                                }
                            }

                            var d1 = (from a in db.Costing_Units where a.id == logIn.BU_ID select new { a.GST_No, a.State_Code }).ToList();
                            if (d1.Count > 0)
                            {
                                //comnpstatecode = Mid(d1[0].GST_No, 1, 2);
                                //suppStateCode = Mid(txtCustGSTNo.Text, 1, 2);
                                comnpstatecode = d1[0].State_Code;
                                suppStateCode = txtCustStateCode.Text;
                                if (cmbInvType.Text == "Common Invoice" || cmbInvType.Text == "Service Invoice")
                                {
                                    if (suppStateCode == comnpstatecode)
                                    {
                                        d = Convert.ToDecimal(taxRate) / 2;
                                        R1.Cells["CGST_Per"].Value = d.ToString("0.00");
                                        R1.Cells["SGST_Per"].Value = d.ToString("0.00");
                                        R1.Cells["IGST_Per"].Value = "0.00";
                                        cgstPer = Convert.ToDecimal(R1.Cells["CGST_Per"].Value);
                                        sgstPer = Convert.ToDecimal(R1.Cells["SGST_Per"].Value);
                                        igstPer = Convert.ToDecimal(R1.Cells["IGST_Per"].Value);
                                    }
                                    else
                                    {
                                        d = taxRate;
                                        R1.Cells["CGST_Per"].Value = "0.00";
                                        R1.Cells["SGST_Per"].Value = "0.00";
                                        R1.Cells["IGST_Per"].Value = d.ToString("0.00");
                                        cgstPer = Convert.ToDecimal(R1.Cells["CGST_Per"].Value);
                                        sgstPer = Convert.ToDecimal(R1.Cells["SGST_Per"].Value);
                                        igstPer = Convert.ToDecimal(R1.Cells["IGST_Per"].Value);
                                    }
                                }
                                else
                                {
                                    if (cmbInvType.Text == "Deemed Export")
                                    {
                                        if (suppStateCode == comnpstatecode)
                                        {
                                            //d = 0.1 / 2;
                                            R1.Cells["CGST_Per"].Value = "0.05";
                                            R1.Cells["SGST_Per"].Value = "0.05";
                                            R1.Cells["IGST_Per"].Value = "0.00";
                                            cgstPer = Convert.ToDecimal(R1.Cells["CGST_Per"].Value);
                                            sgstPer = Convert.ToDecimal(R1.Cells["SGST_Per"].Value);
                                            igstPer = Convert.ToDecimal(R1.Cells["IGST_Per"].Value);
                                        }
                                        else
                                        {
                                            //d =0.1;
                                            R1.Cells["CGST_Per"].Value = "0.00";
                                            R1.Cells["SGST_Per"].Value = "0.00";
                                            R1.Cells["IGST_Per"].Value = "0.10";
                                            cgstPer = Convert.ToDecimal(R1.Cells["CGST_Per"].Value);
                                            sgstPer = Convert.ToDecimal(R1.Cells["SGST_Per"].Value);
                                            igstPer = Convert.ToDecimal(R1.Cells["IGST_Per"].Value);
                                        }

                                    }
                                    else
                                    {
                                        if (cmbInvType.Text == "SEZ Invoice" || cmbInvType.Text == "Bill of Supply")
                                        {
                                            R1.Cells["CGST_Per"].Value = "0.00";
                                            R1.Cells["SGST_Per"].Value = "0.00";
                                            R1.Cells["IGST_Per"].Value = "0.00";
                                            cgstPer = Convert.ToDecimal(R1.Cells["CGST_Per"].Value);
                                            sgstPer = Convert.ToDecimal(R1.Cells["SGST_Per"].Value);
                                            igstPer = Convert.ToDecimal(R1.Cells["IGST_Per"].Value);
                                        }
                                    }
                                }
                            }

                            Amt = AcceptedQty * price;
                            int j1 = dgProducts.CurrentRow.Index;
                            //int j1 =  Convert.ToInt32(R1.ToString()) + 1;
                            if (R1.Cells["Item_Code"].Value != "")
                            {

                            }
                            else
                            {
                                R1.Cells["Item_Code"].Value = (j1 + 1).ToString();
                            }
                            R1.Cells["Amt_Before_Disc"].Value = Amt.ToString("0.00");
                            DiscAmt = (Amt * DiscPer) / 100;
                            R1.Cells["Disc_Amt"].Value = DiscAmt.ToString("0.00");
                            netAmt = Amt - DiscAmt;
                            R1.Cells["Taxable_Value"].Value = netAmt.ToString("0.00");
                            gst = (Convert.ToDecimal(R1.Cells["Taxable_Value"].Value) * Convert.ToDecimal(R1.Cells["CGST_Per"].Value)) / 100;

                            R1.Cells["CGST_Amt"].Value = gst.ToString("0.00");
                            R1.Cells["SGST_Amt"].Value = gst.ToString("0.00");
                            igst = (Convert.ToDecimal(R1.Cells["Taxable_Value"].Value) * Convert.ToDecimal(R1.Cells["IGST_Per"].Value)) / 100;
                            R1.Cells["IGST_Amt"].Value = igst.ToString("0.00");

                            totamt = netAmt + gst + gst + igst;
                            R1.Cells["Total_Amount"].Value = totamt.ToString("0.00");
                            GetTot();

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

                if(txtCustPoNo.Text !="")
                {
                    if (txtCustPoNo.Text != "NA")
                    {
                        if (txtCustPoNo.Text != "Multi")
                        {
                            GetOrderInfo();
                        }
                    }
                }
            }
            catch
            {

            }
        }
        public void GetTot()
        {
            try
            {
                decimal totQty = 0;
                decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
                for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                {
                    totQty += (dgProducts.Rows[i].Cells["Inv_Qty"].Value.ToString() == "" || dgProducts.Rows[i].Cells["Inv_Qty"].Value == null || dgProducts.Rows[i].Cells["Inv_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Inv_Qty"].Value);
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

                    //sgstPer = Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Per"].Value);
                    //igstPer = Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Per"].Value);
                }

                txtTotalQty.Text = totQty.ToString(".00000");
                txtSubTotal.Text = y.ToString("0.00");
                txtTotDiscount.Text = q.ToString(".00");
                decimal frieghtPerMT = (txtFrieghtPerTon.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtFrieghtPerTon.Text);
                if (frieghtPerMT > 0)
                {
                    txtFrieght.Text = (frieghtPerMT * totQty).ToString(); ;
                 }
                else
                {

                }
                decimal fAmt = (txtFrieght.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtFrieght.Text);
                decimal OthAmt = (txtOthers.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtOthers.Text);
                decimal pAmt = (txtPacking.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtPacking.Text);
                decimal InsAmt = (txtInsurance.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtInsurance.Text);
                decimal cessAmt = (txtCessAmt.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtCessAmt.Text);

                txtTot_TaxableValue.Text = (v + fAmt + OthAmt+pAmt+InsAmt).ToString(".00");
                decimal taxvalue = Convert.ToDecimal(txtTot_TaxableValue.Text);
                decimal cgst = (taxvalue * cgstPer) / 100;
                decimal sgst = (taxvalue * sgstPer) / 100;
                decimal igst = (taxvalue * igstPer) / 100;

                txtTot_CGST.Text = cgst.ToString(".00");
                txtTot_SGST.Text = sgst.ToString(".00");
                txtTot_IGST.Text = igst.ToString(".00");
                decimal AmtForTCs = 0;
                


                if (cmbTCS.Text == "TCS on Basic")
                {
                    AmtForTCs = (v);
                }
                else if (cmbTCS.Text == "TCS on Gross")

                {
                    AmtForTCs = (fAmt + OthAmt + v + cgst + sgst + igst+ cessAmt);
                    //AmtForTCs = (v);
                }
                else
                {
                    AmtForTCs = 0;
                }
                
                decimal tcsPer = (txtTCSPer.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtTCSPer.Text);

                decimal TcsAmt = AmtForTCs * tcsPer / 100;
                txtTCSAmt.Text = TcsAmt.ToString(".00");
                decimal rndAmt = (txtRounding.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtRounding.Text);
                txtTot_InvValue.Text = (taxvalue + cgst + sgst + igst + TcsAmt + rndAmt+cessAmt).ToString(".00");
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
                String myString = "";
                if (logIn.company == 1047 || logIn.company == 1042)
                {
                }
                else
                {
                    myString = txtInvNo.Text;
                    if ((from u in db.Invoice_Masters where u.Inv_No == myString && u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID select u).Count() > 0)
                    {
                        myString = txtInvNo.Text;
                    }
                    else
                    {
                        if (cmbInvType.Text == "Bill of Supply" && logIn.company == 11) //Only for YEN Flexi
                        {
                            if(txtInvNo.Text !="")
                            {

                            }
                            else
                            {
                                MessageBox.Show ("Please Enter Invoice No");
                                txtInvNo.Focus();
                                return;
                            }
                        }
                        else
                        {
                            AutoincrementId();
                        }
                    }
                }
                myString = txtInvNo.Text;
                if (txtInvNo.Text != "") {


                    SqlCommand cmd = new SqlCommand("SaveGSTInvoice", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Inv_NO", myString);
                    cmd.Parameters.AddWithValue("@Inv_Date", dpInvDate.Value);
                    cmd.Parameters.AddWithValue("@InvType", cmbInvType.Text);
                    cmd.Parameters.AddWithValue("@Tax_Class", Convert.ToInt32(cmbTaxClass.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@BuyerName", Convert.ToInt32(CmbBuyerName.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@Cust_GST_No", (txtCustGSTNo.Text == "") ? "" : txtCustGSTNo.Text);
                    cmd.Parameters.AddWithValue("@ConsigneeName", Convert.ToInt32(CmbConsigneeName.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@ConsigneeAddress", (txtConAddress.Text == "") ? "" : txtConAddress.Text);
                    cmd.Parameters.AddWithValue("@Con_GST_No", (txtConGSTNo.Text == "") ? "" : txtConGSTNo.Text);
                    cmd.Parameters.AddWithValue("@CustomerPONo", (txtCustPoNo.Text == "") ? "" : txtCustPoNo.Text);
                    cmd.Parameters.AddWithValue("@SO_Ref_No", txtSoNo.Text);
                    cmd.Parameters.AddWithValue("@Sale_Account", (cmbSaleAccount.Text == "NA" || cmbSaleAccount.Text == "") ? 7210 : Convert.ToInt32(cmbSaleAccount.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@PODate", dpPODate.Value);

                    cmd.Parameters.AddWithValue("@TotalQty", (txtTotalQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotalQty.Text));
                    cmd.Parameters.AddWithValue("@SubTotal", (txtSubTotal.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtSubTotal.Text));
                    cmd.Parameters.AddWithValue("@Tot_Discount", (txtTotDiscount.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotDiscount.Text));
                    cmd.Parameters.AddWithValue("@Frieght_amnt", (txtFrieght.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtFrieght.Text));
                    cmd.Parameters.AddWithValue("@Other_Charges", (txtOthers.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtOthers.Text));

                    cmd.Parameters.AddWithValue("@Tot_TaxableValue", (txtTot_TaxableValue.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTot_TaxableValue.Text));
                    cmd.Parameters.AddWithValue("@Tot_CGST_Amnt", (txtTot_CGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_CGST.Text));
                    cmd.Parameters.AddWithValue("@Tot_SGST_Amnt", (txtTot_SGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_SGST.Text));
                    cmd.Parameters.AddWithValue("@Tot_IGST_Amnt", (txtTot_IGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_IGST.Text));
                    cmd.Parameters.AddWithValue("@cessAmt", (txtCessAmt.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtCessAmt.Text));
                    cmd.Parameters.AddWithValue("@TCS_Per", (txtTCSPer.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTCSPer.Text));
                    cmd.Parameters.AddWithValue("@TCS_Amnt", (txtTCSAmt.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTCSAmt.Text));
                    cmd.Parameters.AddWithValue("@Rounding", (txtRounding.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtRounding.Text));

                    cmd.Parameters.AddWithValue("@Tot_Inv_Value", (txtTot_InvValue.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_InvValue.Text));
                    cmd.Parameters.AddWithValue("@Transporter_Name", (cmbTransporter.Text == "") ? "" : cmbTransporter.Text);
                    cmd.Parameters.AddWithValue("@DespatchThrough", (txtDespthrough.Text == "") ? "" : txtDespthrough.Text);
                    cmd.Parameters.AddWithValue("@VehicleNo", (txtVehicleNo.Text == "") ? "" : txtVehicleNo.Text);
                    cmd.Parameters.AddWithValue("@LR_No", (txtLRNo.Text == "") ? "" : txtLRNo.Text);
                    cmd.Parameters.AddWithValue("@LR_Date", dtLRDate.Value);
                    cmd.Parameters.AddWithValue("@WayBillNo", (txtWayBillNo.Text == "") ? "" : txtWayBillNo.Text);
                    cmd.Parameters.AddWithValue("@Spl_Instructions", (txtSplInstructions.Text == "") ? "" : txtSplInstructions.Text);
                    cmd.Parameters.AddWithValue("@Status", Convert.ToInt32(cmbStatus.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@BU_ID", logIn.BU_ID);
                    cmd.Parameters.AddWithValue("@Company_ID", logIn.company);
                    cmd.Parameters.AddWithValue("@Created_By", lblCreatedBy.Text);
                    cmd.Parameters.AddWithValue("@Modified_By", logIn.username + "-" + DateTime.Now);
                    cmd.Parameters.AddWithValue("@Inv_Issue_Time", (textBox1.Text == "") ? "" : textBox1.Text);
                    cmd.Parameters.AddWithValue("@Inv_Removal_Time", (textBox2.Text == "") ? "" : textBox2.Text);
                    cmd.Parameters.AddWithValue("@RCM_Invoice", checkBox1.Checked);
                    cmd.Parameters.AddWithValue("@Packing_Charges", (txtPacking.Text == null || txtPacking.Text == "") ? 0 : Convert.ToDecimal(txtPacking.Text));
                    cmd.Parameters.AddWithValue("@Insurance_Charges", (txtInsurance.Text == null || txtInsurance.Text == "") ? 0 : Convert.ToDecimal(txtInsurance.Text));

                    cmd.Parameters.AddWithValue("@DC_No", (txtDCNo.Text == "") ? "" : txtDCNo.Text);
                    cmd.Parameters.AddWithValue("@Destination", (txtDestination.Text == "") ? "" : txtDestination.Text);
                    cmd.Parameters.AddWithValue("@EInv_IRN_No", (txtIRNNo.Text == "") ? "" : txtIRNNo.Text);
                    cmd.Parameters.AddWithValue("@Einv_ACK_No", (txtACKNo.Text == "") ? "" : txtACKNo.Text);
                    cmd.Parameters.AddWithValue("@Einv_ACK_Date", (txtACKDate.Text == "") ? "" : txtACKDate.Text);
                    cmd.Parameters.AddWithValue("@EInv_QR_Code", (txtQRCode.Text == "") ? "" : txtQRCode.Text);



                    string Prod_Code = "";
                    string Product_Description = "";
                    string Prod_Grade = "";
                    string Uom = "";
                    string PO_Qty = "";
                    string Stock_Qty = "";
                    string Qty = "";
                    string Price = "";
                    string Amount = "";
                    //string Disc_Per = "";
                    //string Disc_Amount = "";
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
                    //string Stock_Price = "";
                    //string SO_Item_No = "";
                    int rowcount = 0;

                    for (int i = 0; i < dgProducts.RowCount - 1; i++)
                    {

                        Prod_Code = Prod_Code + Convert.ToString(dgProducts.Rows[i].Cells["Item_Code"].Value).PadRight(14);
                        int slen = Convert.ToString(dgProducts.Rows[i].Cells["Item_Description"].Value).Length;
                        Product_Description = Product_Description + Convert.ToString(dgProducts.Rows[i].Cells["Item_Description"].Value).Trim().PadRight(250);
                        Prod_Grade = Prod_Grade + Convert.ToString(dgProducts.Rows[i].Cells["Item_Grade"].Value).Trim().PadRight(250);
                        Uom = Uom + Convert.ToString(dgProducts.Rows[i].Cells["uom"].Value).Trim().PadRight(14);
                        PO_Qty = PO_Qty + Convert.ToString(dgProducts.Rows[i].Cells["PO_Qty"].Value).PadRight(14);
                        Stock_Qty = Stock_Qty + Convert.ToString(dgProducts.Rows[i].Cells["Stock_Qty"].Value).PadRight(14);
                        Qty = Qty + Convert.ToString(dgProducts.Rows[i].Cells["Inv_Qty"].Value).PadRight(14);
                        Price = Price + Convert.ToString(dgProducts.Rows[i].Cells["Basic_Price"].Value).PadRight(14);
                        Amount = Amount + Convert.ToString(dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value).PadRight(14);
                        //Disc_Per = Disc_Per + Convert.ToString(dgProducts.Rows[i].Cells["Disc_Per"].Value).PadRight(14);
                        //Disc_Amount = Disc_Amount + Convert.ToString(dgProducts.Rows[i].Cells["Disc_Amt"].Value).PadRight(14);
                        Taxable_Value = Taxable_Value + Convert.ToString(dgProducts.Rows[i].Cells["Taxable_Value"].Value).PadRight(14);
                        CGST_Per = CGST_Per + Convert.ToString(dgProducts.Rows[i].Cells["CGST_Per"].Value).PadRight(14);
                        SGST_Per = SGST_Per + Convert.ToString(dgProducts.Rows[i].Cells["SGST_Per"].Value).PadRight(14);
                        IGST_Per = IGST_Per + Convert.ToString(dgProducts.Rows[i].Cells["IGST_Per"].Value).PadRight(14);
                        CGST_Amnt = CGST_Amnt + Convert.ToString(dgProducts.Rows[i].Cells["CGST_Amt"].Value).PadRight(14);
                        SGST_Amnt = SGST_Amnt + Convert.ToString(dgProducts.Rows[i].Cells["SGST_Amt"].Value).PadRight(14);
                        IGST_Amnt = IGST_Amnt + Convert.ToString(dgProducts.Rows[i].Cells["IGST_Amt"].Value).PadRight(14);
                        Net_Amount = Net_Amount + Convert.ToString(dgProducts.Rows[i].Cells["Total_Amount"].Value).PadRight(14);
                        //Stock_Price = Stock_Price + Convert.ToString((dgProducts.Rows[i].Cells["Stock_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Stock_Price"].Value)).PadRight(14);
                        Remarks = Remarks + Convert.ToString(dgProducts.Rows[i].Cells["Remarks"].Value).Trim().PadRight(14);
                        SO_NO = SO_NO + Convert.ToString(dgProducts.Rows[i].Cells["SO_Ref_No"].Value).Trim().PadRight(14);
                        //SO_Item_No = SO_Item_No + Convert.ToString(dgProducts.Rows[i].Cells["SO_Item_No"].Value).Trim().PadRight(14);
                        if (dgProducts.Rows[i].Cells["Item_No"].Value == null || dgProducts.Rows[i].Cells["Item_No"].Value == "" || i==0)
                        {
                            ProdSno = ProdSno + Convert.ToString(i + 1).PadRight(14);
                        }
                        else
                        {
                            ProdSno = ProdSno + Convert.ToString(dgProducts.Rows[i].Cells["Item_No"].Value).PadRight(14);
                        }



                        //ProdSno = ProdSno + Convert.ToString((i + 1)).PadRight(14); 
                        //Company_ID = logIn.company;
                        rowcount += 1;
                    }

                    cmd.Parameters.AddWithValue("@txt_Prod_Code", Prod_Code);
                    cmd.Parameters.AddWithValue("@txt_Product_Description", Product_Description);
                    cmd.Parameters.AddWithValue("@txt_Prod_Grade", Prod_Grade);
                    cmd.Parameters.AddWithValue("@txt_Uom", Uom);
                    cmd.Parameters.AddWithValue("@txt_PO_Qty", PO_Qty);
                    cmd.Parameters.AddWithValue("@txt_Stock_Qty", Stock_Qty);
                    cmd.Parameters.AddWithValue("@txt_Qty", Qty);
                    cmd.Parameters.AddWithValue("@txt_Price", Price);
                    cmd.Parameters.AddWithValue("@txt_Amount", Amount);
                    //cmd.Parameters.AddWithValue("@txt_Disc_Per", Disc_Per);
                    //cmd.Parameters.AddWithValue("@txt_Disc_Amount", Disc_Amount);
                    cmd.Parameters.AddWithValue("@txt_Taxable_Value", Taxable_Value);
                    cmd.Parameters.AddWithValue("@txt_CGST_Per", CGST_Per);
                    cmd.Parameters.AddWithValue("@txt_CGST_Amnt", CGST_Amnt);
                    cmd.Parameters.AddWithValue("@txt_SGST_Amnt", SGST_Amnt);
                    cmd.Parameters.AddWithValue("@txt_SGST_Per", SGST_Per);
                    cmd.Parameters.AddWithValue("@txt_IGST_Per", IGST_Per);
                    cmd.Parameters.AddWithValue("@txt_IGST_Amnt", IGST_Amnt);
                    cmd.Parameters.AddWithValue("@txt_Net_Amount", Net_Amount);
                    cmd.Parameters.AddWithValue("@txt_Remarks", Remarks);
                    cmd.Parameters.AddWithValue("@txt_SO_NO", SO_NO);
                    cmd.Parameters.AddWithValue("@txt_Prod_SNO", ProdSno);
                    //cmd.Parameters.AddWithValue("@txt_Stock_Price", Stock_Price);
                    //cmd.Parameters.AddWithValue("@txt_SO_Item_No", SO_Item_No);
                    cmd.Parameters.AddWithValue("@gridcount", rowcount);

                    try
                    {
                        con.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (Convert.ToBoolean(result))
                        {
                            MessageBox.Show("Record has been successfully Saved/Updated with Inv No :" + myString);
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
        public void Save()
        {
            try
            {
                String myString = "";
                //AutoincrementId();
                myString = txtInvNo.Text;
                if ((from u in db.Invoice_Masters where u.Inv_No == myString && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    myString = txtInvNo.Text;
                    db.sp_GSTInv_Delete(myString, logIn.company,logIn.BU_ID);
                }
                else
                {
                    if (logIn.company == 25)
                    {
                    }
                    else
                    {
                        AutoincrementId();
                    }
                    myString = txtInvNo.Text;

                }
                    //if (frmGate.Modify.Contains(this.Text))
                    //{

               // dgProducts.Enabled = false;
                //var S = db.Sale_Order_Masters.Where(w => w.SO_NO == myString && w.Company_ID == logIn.company).FirstOrDefault();
                Invoice_Master S = new Invoice_Master();
                {
                    S.Inv_No = myString;
                    S.InvDate = dpInvDate.Value;
                    S.InvType = cmbInvType.Text;
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
                    S.Other_Charges = (txtOthers.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtOthers.Text);
                    S.Rounding = (txtRounding.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtRounding.Text);
                    S.Tot_CGST_Amnt = (txtTot_CGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_CGST.Text);
                    S.Tot_SGST_Amnt = (txtTot_SGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_SGST.Text);
                    S.Tot_IGST_Amnt = (txtTot_IGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_IGST.Text);
                    S.TCS_Per = (txtTCSPer.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTCSPer.Text);
                    S.TCS_Amnt = (txtTCSAmt.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTCSAmt.Text);
                    S.Tot_Inv_Value = (txtTot_InvValue.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_InvValue.Text);
                    if (cmbSaleAccount.Text != "" && cmbSaleAccount.Text !="NA")
                    {
                        S.Sale_Account = Convert.ToInt32(cmbSaleAccount.SelectedValue.ToString());
                    }
                    else
                    {
                        S.Sale_Account = 7210;
                    }
                    S.Transporter_Name = (cmbTransporter.Text == "") ? "" : cmbTransporter.Text;
                    S.RCM_Invoice = checkBox1.Checked;
                    S.DespatchThrough = (txtDespthrough.Text == "") ? "" : txtDespthrough.Text;
                    S.VehicleNo =   (txtVehicleNo.Text == "") ? "" : txtVehicleNo.Text;
                    S.WayBillNo = (txtWayBillNo.Text == "") ? "" : txtWayBillNo.Text;
                    S.InvValueInWords = (txtLRNo.Text == "") ? "" : txtLRNo.Text;
                    S.Spl_Instructions = (txtSplInstructions.Text == "") ? "" : txtSplInstructions.Text;
                    S.SO_Ref_No = txtSoNo.Text;
                    S.LR_No = txtLRNo.Text;
                    S.LR_Date = dtLRDate.Value;
                    S.BU_ID = logIn.BU_ID;
                    S.Company_ID = logIn.company;
                    S.Created_By = lblCreatedBy.Text;                    
                    S.Modified_By = logIn.username + "-" + DateTime.Now;
                    S.Inv_Issue_Time = (textBox1.Text == "") ? "" : textBox1.Text;
                    S.useDigitalSign = checkBox2.Checked;
                    S.Inv_Removal_Time = DateTime.Now.ToString("hh:mm");
                    S.Insurance_Charges = (txtInsurance.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtInsurance.Text);
                    S.Packing_Charges = (txtPacking.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtPacking.Text);
                    S.DC_No = (txtDCNo.Text == "") ? "" : txtDCNo.Text;
                    S.Destination = (txtDestination.Text == "") ? "" : txtDestination.Text;
                    db.Invoice_Masters.InsertOnSubmit(S);
                    db.SubmitChanges();
                }
                        //db.Transaction = transaction;
                for (int i = 0; i < dgProducts.RowCount - 1; i++)
                {
                    Invoice_Child SC = new Invoice_Child();
                    var d1 = (from a in db.Invoice_Masters where a.Inv_No == myString && a.Company_ID == logIn.company select new { a.Id }).ToList();
                    SC.Inv_Master_ID = d1[0].Id;
                    SC.Inv_No = myString;
                    SC.Prod_Code = (dgProducts.Rows[i].Cells["Item_Code"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Code"].Value).ToString();
                    SC.Product_Description = (dgProducts.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value).ToString();
                    SC.Prod_Grade = (dgProducts.Rows[i].Cells["Item_Grade"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value).ToString();
                    SC.Uom = (dgProducts.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["uom"].Value.ToString();
                    SC.PO_Qty = (dgProducts.Rows[i].Cells["PO_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["PO_Qty"].Value);

                    SC.Stock_qty = (dgProducts.Rows[i].Cells["Stock_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Stock_Qty"].Value);

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
                    db.Invoice_Childs.InsertOnSubmit(SC);
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
                                     select new { c.Address_1, c.Address_2, c.City, c.State, c.GSTIN_NO,c.Pincode,c.StateCode }).ToList();
                        if (State.Count > 0)
                        {

                        genConsigneeAddress CAddr = new genConsigneeAddress();
                        CAddr.Address1 = State[0].Address_1;

                        CAddr.Address2 = State[0].Address_2;

                        if (State[0].Pincode == "" || State[0].Pincode == null)
                        {
                            MessageBox.Show("Consignee PIN CODE should not be blank and Min 6 Digits, Update the same by click on View/Update Address");
                            CAddr.PinCode = 0;
                        }
                        else
                        {
                            CAddr.PinCode = Convert.ToInt32(State[0].Pincode);
                        }
                        CAddr.State = State[0].State;
                        CAddr.StateCode = State[0].StateCode;
                        CAddr.GSTIN = State[0].GSTIN_NO;

                        JavaScriptSerializer serializer = new JavaScriptSerializer();

                        string json = JsonConvert.SerializeObject(CAddr, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
                        {
                            NullValueHandling = NullValueHandling.Ignore
                        });





                        txtConAddress.Text = json;

                        txtConGSTNo.Text = State[0].GSTIN_NO;
                           // txtConAddress.Text = State[0].Address_1 + ", " + State[0].Address_2 + ", " + State[0].City;
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

                DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].Name;
               
                if(txtCustPoNo.Text =="NA")
                {
                    R1.Cells["Basic_Price"].ReadOnly = false;
                }
                else
                {
                    R1.Cells["Basic_Price"].ReadOnly = true;
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
                this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                this.sfDataGrid1.Columns["SO_NO"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["SO_NO"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["SO_NO"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["SO_NO"].FilterRowCondition = FilterRowCondition.Contains;

                this.sfDataGrid1.Columns["CustomerPoNo"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["CustomerPoNo"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["CustomerPoNo"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["CustomerPoNo"].FilterRowCondition = FilterRowCondition.Contains;

                this.sfDataGrid1.Columns["Product_Description"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["Product_Description"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Product_Description"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["Product_Description"].FilterRowCondition = FilterRowCondition.Contains;


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
                DataTable dtgetSelectedprducts = new DataTable();
                //Check Whether Exisitng Products Already Selected in Main Grid
                if (dgProducts.Rows.Count > 1)
                {
                    dtexisting.Rows.Clear();
                    dtexisting.Columns.Clear();
                    dtexisting.Columns.Add("Item_Code", typeof(string));
                    dtexisting.Columns.Add("Prod_Code", typeof(string));
                    dtexisting.Columns.Add("Item_Description", typeof(string));                   
                    dtexisting.Columns.Add("Item_Grade", typeof(string));
                    dtexisting.Columns.Add("UOM", typeof(string));
                    dtexisting.Columns.Add("PO_Qty", typeof(string));
                    dtexisting.Columns.Add("Stock_Qty", typeof(string));
                    dtexisting.Columns.Add("Inv_Qty", typeof(string));
                    dtexisting.Columns.Add("Basic_Price", typeof(string));
                    dtexisting.Columns.Add("Amt_Before_Disc", typeof(string));
                    dtexisting.Columns.Add("Disc_Per", typeof(string));
                    dtexisting.Columns.Add("Disc_Amt", typeof(string));
                    dtexisting.Columns.Add("Taxable_Value", typeof(string));
                    dtexisting.Columns.Add("CGST_Per", typeof(string));
                    dtexisting.Columns.Add("CGST_Amt", typeof(string));
                    dtexisting.Columns.Add("SGST_Per", typeof(string));
                    dtexisting.Columns.Add("SGST_Amt", typeof(string));
                    dtexisting.Columns.Add("IGST_Per", typeof(string));
                    dtexisting.Columns.Add("IGST_Amt", typeof(string));
                    dtexisting.Columns.Add("Total_Amount", typeof(string));
                    dtexisting.Columns.Add("Remarks", typeof(string));
                    dtexisting.Columns.Add("SO_Ref_No", typeof(string));
                    //dtexisting.Columns.Add("Item_No", typeof(string));
                    dtexisting.Columns.Add("SO_Item_No", typeof(string));
                    dtexisting.Columns.Add("Stock_Price", typeof(string));
                    
                    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                    {
                        DataRow dr;
                        dr = dtexisting.NewRow();
                        dr["Item_Code"] = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                        dr["Prod_Code"] = dgProducts.Rows[i].Cells["Prod_Code"].Value.ToString();
                        dr["Item_Description"] = dgProducts.Rows[i].Cells["Item_Description"].Value.ToString();
                        dr["Item_Grade"] = dgProducts.Rows[i].Cells["Item_Grade"].Value.ToString();                        
                        dr["UOM"] = dgProducts.Rows[i].Cells["UOM"].Value.ToString();
                        dr["PO_Qty"] = dgProducts.Rows[i].Cells["PO_Qty"].Value.ToString();
                        dr["Stock_Qty"] = dgProducts.Rows[i].Cells["Stock_Qty"].Value.ToString();
                        dr["Inv_Qty"] = dgProducts.Rows[i].Cells["Inv_Qty"].Value.ToString();                        
                        dr["Basic_Price"] = dgProducts.Rows[i].Cells["Basic_Price"].Value.ToString();
                        dr["Amt_Before_Disc"] = dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value.ToString();
                        dr["Disc_Per"] = dgProducts.Rows[i].Cells["Disc_Per"].Value.ToString();
                        dr["Disc_Amt"] = dgProducts.Rows[i].Cells["Disc_Amt"].Value.ToString();
                        dr["Taxable_Value"] = dgProducts.Rows[i].Cells["Taxable_Value"].Value.ToString();
                        dr["CGST_Per"] = dgProducts.Rows[i].Cells["CGST_Per"].Value.ToString();
                        dr["CGST_Amt"] = dgProducts.Rows[i].Cells["CGST_Amt"].Value.ToString();
                        dr["SGST_Per"] = dgProducts.Rows[i].Cells["SGST_Per"].Value.ToString();
                        dr["SGST_Amt"] = dgProducts.Rows[i].Cells["SGST_Amt"].Value.ToString();
                        dr["IGST_Per"] = dgProducts.Rows[i].Cells["IGST_Per"].Value.ToString();
                        dr["IGST_Amt"] = dgProducts.Rows[i].Cells["IGST_Amt"].Value.ToString();
                        dr["Total_Amount"] = dgProducts.Rows[i].Cells["Total_Amount"].Value.ToString();
                        dr["Remarks"] = dgProducts.Rows[i].Cells["Remarks"].Value.ToString();
                        dr["SO_Ref_No"] = dgProducts.Rows[i].Cells["SO_Ref_No"].Value.ToString();
                        //dr["SO_Item_No"] = dgProducts.Rows[i].Cells["SO_Item_No"].Value.ToString();
                        dr["SO_Item_No"] = dgProducts.Rows[i].Cells["SO_Item_No"].Value.ToString();
                        dr["Stock_Price"] = dgProducts.Rows[i].Cells["Stock_Price"].Value.ToString();

                        dtexisting.Rows.Add(dr);

                    }
                    dtexisting.AcceptChanges();               
                }

                dtgetproducts.Columns.Clear();
                dtgetproducts.Rows.Clear();
                dtgetproducts.Columns.Add("Item_Code", typeof(string));
                dtgetproducts.Columns.Add("Prod_Code", typeof(string));
                dtgetproducts.Columns.Add("Item_Description", typeof(string));
                dtgetproducts.Columns.Add("Item_Grade", typeof(string));
                dtgetproducts.Columns.Add("UOM", typeof(string));
                dtgetproducts.Columns.Add("PO_Qty", typeof(string));
                dtgetproducts.Columns.Add("Stock_Qty", typeof(string));
                dtgetproducts.Columns.Add("Inv_Qty", typeof(string));
                dtgetproducts.Columns.Add("Basic_Price", typeof(string));
                dtgetproducts.Columns.Add("Amt_Before_Disc", typeof(string));
                dtgetproducts.Columns.Add("Disc_Per", typeof(string));
                dtgetproducts.Columns.Add("Disc_Amt", typeof(string));
                dtgetproducts.Columns.Add("Taxable_Value", typeof(string));
                dtgetproducts.Columns.Add("CGST_Per", typeof(string));
                dtgetproducts.Columns.Add("CGST_Amt", typeof(string));
                dtgetproducts.Columns.Add("SGST_Per", typeof(string));
                dtgetproducts.Columns.Add("SGST_Amt", typeof(string));
                dtgetproducts.Columns.Add("IGST_Per", typeof(string));
                dtgetproducts.Columns.Add("IGST_Amt", typeof(string));
                dtgetproducts.Columns.Add("Total_Amount", typeof(string));
                dtgetproducts.Columns.Add("Remarks", typeof(string));
                dtgetproducts.Columns.Add("SO_Ref_No", typeof(string));
                //dtgetproducts.Columns.Add("Item_No", typeof(string));
                dtgetproducts.Columns.Add("SO_Item_No", typeof(string));
                dtgetproducts.Columns.Add("Stock_Price", typeof(string));
                
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

                            int ordId = 0;
                            //Get Consignee Details
                            var State = (from c in db.Sale_Order_Masters
                                         join s in db.Supplier_informations on c.ConsigneeName equals s.ID
                                         where c.Status !=24 && c.SO_NO == SO_Ref_No && c.Company_ID == logIn.company //&& c.BU_ID == logIn.BU_ID //Convert.ToInt32(CmbBuyerName.SelectedValue.ToString())
                                         select new { c.Delivery_Address, c.ConsigneeName,c.Delivery_GSTIN,c.Frieght_Unit, c.Id,s.City }).ToList();
                            if (State.Count > 0)
                            {
                                if (State[0].Delivery_Address != null)
                                {
                                    txtConAddress.Text = State[0].Delivery_Address.ToString();
                                }
                               // txtDestination.Text = CCity;
                                CmbConsigneeName.SelectedValue = State[0].ConsigneeName;
                                txtFrieghtPerTon.Text = State[0].Frieght_Unit.ToString();
                                //if (Mid(State[0].Delivery_Address, 3, 4) == "Addr")
                                //{
                                //    JObject jsoncancel = JObject.Parse(State[0].Delivery_Address);

                                //    string CAddr = (string)jsoncancel.SelectToken("Address1") + "," + (string)jsoncancel.SelectToken("Address2");
                                //    string CCity = (string)jsoncancel.SelectToken("City") + "," + (string)jsoncancel.SelectToken("PinCode");
                                //    string cState = (string)jsoncancel.SelectToken("State") + ", State Code : " + (string)jsoncancel.SelectToken("StateCode");
                                //    txtConGSTNo.Text = (string)jsoncancel.SelectToken("GSTIN");
                                //    txtConAddress.Text = CAddr + ", " + CCity + ", " + cState;
                                //    txtDestination.Text = CCity;
                                //    CmbConsigneeName.SelectedValue = State[0].ConsigneeName;
                                //    txtFrieghtPerTon.Text = State[0].Frieght_Unit.ToString();
                                    ordId = State[0].Id;

                                //}
                                //else
                                //{

                                //    txtConGSTNo.Text = State[0].Delivery_GSTIN;
                                //    txtConAddress.Text = State[0].Delivery_Address;
                                //    txtDestination.Text = State[0].City;
                                //    CmbConsigneeName.SelectedValue = State[0].ConsigneeName;
                                //    txtFrieghtPerTon.Text = State[0].Frieght_Unit.ToString();
                                //    ordId = State[0].Id;

                                //}
                            }
                            drgetproducts["Disc_Per"] = 0;
                            var PGrade = (from data in db.Sale_Order_Childs where data.So_Master_ID == ordId && data.Prod_Code == Convert.ToInt32(Item_Code) && data.Company_ID ==logIn.company  select data).ToList();

                            if (PGrade.Count > 0)
                            {
                                
                                //dgProductsList.DataSource = d;
                                if (PGrade[0].Prod_Grade != null)
                                {
                                    drgetproducts["Item_Grade"] = PGrade[0].Prod_Grade.Trim();
                                }
                                drgetproducts["Prod_Code"] = PGrade[0].Int_Prod_Code;
                                drgetproducts["Disc_Per"] = PGrade[0].Disc_Per;                                
                                drgetproducts["CGST_Per"] = PGrade[0].CGST_Per;
                                drgetproducts["CGST_Amt"] = 0;
                                drgetproducts["SGST_Per"] = PGrade[0].SGST_Per; 
                                drgetproducts["SGST_Amt"] = 0;
                                drgetproducts["IGST_Per"] = PGrade[0].IGST_Per;
                                drgetproducts["IGST_Amt"] = 0;
                            }                           
                            drgetproducts["UOM"] = UOM.ToString();
                            drgetproducts["PO_Qty"] = PO_Qty.ToString();
                            DateTime t = dpInvDate.Value;
                            string dt1 = t.ToString("yyyy/MM/dd");
                            var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(Item_Code.ToString()), Convert.ToDateTime(dt1), logIn.BU_ID) select data).ToList();
                            drgetproducts["Stock_Qty"] = "0";
                            if (stock.Count > 0)
                            {
                                //dgProductsList.DataSource = d;
                                drgetproducts["Stock_Qty"] = stock[0].ClosingQty;
                                drgetproducts["Stock_Price"] = stock[0].CBPrice;
                            }
                            //drgetproducts["Stock_Qty"] =0;
                            drgetproducts["Inv_Qty"] = 0;
                            drgetproducts["Basic_Price"] = Basic_Price.ToString();
                            drgetproducts["Amt_Before_Disc"] = 0;
                            //drgetproducts["Disc_Per"] = 0;
                            drgetproducts["Disc_Amt"] = 0;
                            drgetproducts["Taxable_Value"] = 0;
                            drgetproducts["Total_Amount"] = 0;
                            drgetproducts["Remarks"] = "";
                            drgetproducts["SO_Ref_No"] = SO_Ref_No.ToString();
                            //drgetproducts["Item_No"] = "";
                            drgetproducts["SO_Item_No"] = PGrade[0].enq_item_no;

                            dtgetproducts.Rows.Add(drgetproducts);
                            dtgetfinalprducts = dtgetfinalprducts.AsEnumerable().Union(dtgetproducts.AsEnumerable()).CopyToDataTable();
                            //}
                            //}
                            dtgetproducts.Rows.Clear();
                        }
                    }
                }
                dtexisting = dtexisting.AsEnumerable().Union(dtgetfinalprducts.AsEnumerable()).CopyToDataTable();
                //dtgetSelectedprducts = dtgetSelectedprducts.AsEnumerable().Union(dtexisting.AsEnumerable()).CopyToDataTable();
                dtgetSelectedprducts = dtgetSelectedprducts.AsEnumerable().Union(dtexisting.AsEnumerable()).CopyToDataTable();

                dgProducts.DataSource = dtgetSelectedprducts;

                txtCustPoNo.Text = "Multi";
                txtSoNo.Text = "Multi";
                
                //dgProducts.DataSource = dtexisting;
                //CmbConsigneeName.Text = CmbBuyerName.Text;
                //CmbConsigneeName.Focus();
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
                        decimal OthAmt = (txtPacking.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtPacking.Text);

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

        private void txtPacking_Leave(object sender, EventArgs e)
        {
            GetTot();
        }

        private void txtInsurance_Leave(object sender, EventArgs e)
        {
            GetTot();
        }

        private void txtOthers_Leave(object sender, EventArgs e)
        {
            GetTot();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void txtInvNo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtInvNo.Text != "")
                {

                    if (ListOfInvoices.editMode == true)
                    {
                    }
                    else
                    {

                        if ((from u in db.Invoice_Masters where u.Inv_No == txtInvNo.Text && u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID select u).Count() > 0)
                        {
                            MessageBox.Show("Invoice No Cannot Be Duplicate", "Invoice Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtInvNo.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Invoice Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtCessAmt_Leave(object sender, EventArgs e)
        {
            GetTot();
        }

        private void cmbInvType_Leave(object sender, EventArgs e)
        {

            if (txtInvNo.Text != "")
            {
            }
            else
            {
                if (cmbInvType.Text != "" && cmbInvType.Text == "Bill of Supply")
                {
                    txtInvNo.Enabled = true;
                }
                else
                {
                    txtInvNo.Enabled = false;
                    AutoincrementId();
                }
            }

           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var bindNearestCity = (from m in db.City_Masters select new { m.City_Name, m.ID }).Distinct().ToList();
            if (bindNearestCity.Count > 0)
            {
                cmbCity.DataSource = bindNearestCity;
                cmbCity.ValueMember = "ID";
                cmbCity.DisplayMember = "City_Name";
            }
            if (cmbCity.Items.Count == 1)
                cmbCity.SelectedIndex = 0;
            else
                cmbCity.SelectedIndex = -1;
            if (Mid(txtConAddress.Text, 3, 4) == "Addr")
            {
                JObject jsoncancel = JObject.Parse(txtConAddress.Text);
                txtAddress1.Text = (string)jsoncancel.SelectToken("Address1"); ;
                txtAddress2.Text = (string)jsoncancel.SelectToken("Address2");
                cmbCity.Text = (string)jsoncancel.SelectToken("City");
                txtstate.Text = (string)jsoncancel.SelectToken("State");
                txtPincode.Text = (string)jsoncancel.SelectToken("PinCode");
                txtStateCode.Text = (string)jsoncancel.SelectToken("StateCode");
                txtConGSTIN.Text = (string)jsoncancel.SelectToken("GSTIN");
                //txtConGSTNo.Text = State[0].GSTIN_NO;
            }
            else
            {
                txtAddress1.Text = txtConAddress.Text;
            }
            groupBox3.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            genConsigneeAddress CAddr = new genConsigneeAddress();
            CAddr.Address1 = txtAddress1.Text;

            CAddr.Address2 = txtAddress2.Text; ;
            CAddr.City = cmbCity.Text;
            CAddr.PinCode = Convert.ToInt32(txtPincode.Text);
            CAddr.State = txtstate.Text;
            CAddr.StateCode = txtStateCode.Text;
            CAddr.GSTIN = txtConGSTIN.Text;
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            string json = JsonConvert.SerializeObject(CAddr, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            txtConAddress.Text = json;
            groupBox3.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = false;
        }

        private void dpInvDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void txtDCNo_Leave(object sender, EventArgs e)
        {
            if (txtDCNo.Text != "")
            {
                GetLoadingSlipData();
                decimal d;
                for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                {
                    DataGridViewRow R1 = dgProducts.Rows[i];
                    taxRate = 18;
                    var cu = (from a in db.Costing_Units where a.id == logIn.BU_ID select new { a.GST_No, a.State_Code }).ToList();
                    if (cu.Count > 0)
                    {
                        string comnpstatecode = cu[0].State_Code;
                        string suppStateCode = txtCustStateCode.Text;

                        if (cmbInvType.Text == "Common Invoice" || cmbInvType.Text == "Service Invoice" || cmbInvType.Text == "Trading Invoice")
                        {
                            if (suppStateCode == comnpstatecode)
                            {
                                d = Convert.ToDecimal(taxRate) / 2;
                                R1.Cells["CGST_Per"].Value = d.ToString("0.00");
                                R1.Cells["SGST_Per"].Value = d.ToString("0.00");
                                R1.Cells["IGST_Per"].Value = "0.00";
                                cgstPer = d;
                                sgstPer = d;
                            }
                            else
                            {
                                d = taxRate;
                                R1.Cells["CGST_Per"].Value = "0.00";
                                R1.Cells["SGST_Per"].Value = "0.00";
                                R1.Cells["IGST_Per"].Value = d.ToString("0.00");
                                igstPer = d;
                            }
                        }
                        else
                        {
                            if (cmbInvType.Text == "Deemed Export")
                            {
                                if (suppStateCode == comnpstatecode)
                                {
                                    // d = 0.1 / 2;
                                    R1.Cells["CGST_Per"].Value = "0.05";
                                    R1.Cells["SGST_Per"].Value = "0.05";
                                    R1.Cells["IGST_Per"].Value = "0.00";
                                    cgstPer = Convert.ToDecimal(R1.Cells["CGST_Per"].Value);
                                    sgstPer = Convert.ToDecimal(R1.Cells["SGST_Per"].Value);
                                }
                                else
                                {
                                    //d =0.1;
                                    R1.Cells["CGST_Per"].Value = "0.00";
                                    R1.Cells["SGST_Per"].Value = "0.00";
                                    R1.Cells["IGST_Per"].Value = "0.10";
                                    sgstPer = Convert.ToDecimal(R1.Cells["IGST_Per"].Value);

                                }

                            }
                            else
                            {
                                if (cmbInvType.Text == "SEZ Invoice" || cmbInvType.Text == "Bill of Supply")
                                {
                                    R1.Cells["CGST_Per"].Value = "0.00";
                                    R1.Cells["SGST_Per"].Value = "0.00";
                                    R1.Cells["IGST_Per"].Value = "0.00";
                                    cgstPer = Convert.ToDecimal(R1.Cells["CGST_Per"].Value);
                                    sgstPer = Convert.ToDecimal(R1.Cells["SGST_Per"].Value);
                                    igstPer = Convert.ToDecimal(R1.Cells["IGST_Per"].Value);
                                }
                            }
                        }
                    }
                    decimal Amt = Convert.ToDecimal(R1.Cells["Amt_Before_Disc"].Value);
                    decimal DiscAmt = 0;
                    R1.Cells["Disc_Amt"].Value = DiscAmt.ToString("0.00");
                    decimal netAmt = Amt - DiscAmt;
                    R1.Cells["Taxable_Value"].Value = netAmt.ToString("0.00");
                    decimal gst = (Convert.ToDecimal(R1.Cells["Taxable_Value"].Value) * Convert.ToDecimal(R1.Cells["CGST_Per"].Value)) / 100;

                    R1.Cells["CGST_Amt"].Value = gst.ToString("0.00");
                    R1.Cells["SGST_Amt"].Value = gst.ToString("0.00");
                    decimal igst = (Convert.ToDecimal(R1.Cells["Taxable_Value"].Value) * Convert.ToDecimal(R1.Cells["IGST_Per"].Value)) / 100;
                    R1.Cells["IGST_Amt"].Value = igst.ToString("0.00");

                    decimal totamt = netAmt + gst + gst + igst;
                    R1.Cells["Total_Amount"].Value = totamt.ToString("0.00");
                    

                }
                GetTot();
            }
        }

        private void cmbCity_Leave(object sender, EventArgs e)
        {
            try
            {
                var State = (from c in db.City_Masters
                             where c.City_Name == cmbCity.Text
                             select new { c.State_Name, c.State_Code }).ToList();
                if (State.Count > 0)
                {
                    txtstate.Text = State[0].State_Name;
                    txtStateCode.Text = State[0].State_Code;
                    txtPincode.Focus();
                }
                //if (txtCustomerAliasName.Text != "")
                //{
                //    txtCustomerName.Text = txtCustomerAliasName.Text + "-" + cmbCity.Text;
                //}
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void txtDCNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDCNo_LocationChanged(object sender, EventArgs e)
        {

        }

        private void CmbConsigneeName_LocationChanged(object sender, EventArgs e)
        {

        }

        private void dgProducts_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
            int columnIndex = dgProducts.CurrentCell.ColumnIndex;
            string columnName = dgProducts.Columns[columnIndex].Name;
            //if (columnName == "Basic_Price")
            //{
                if (txtCustPoNo.Text == "NA")
                {
                    R1.Cells["Basic_Price"].ReadOnly = false;
                }
                else
                {
                    R1.Cells["Basic_Price"].ReadOnly = true;
                }
            //}
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

                    var ST = (from c in db.Get_Sale_Turover(logIn.company,t,logIn.fy_Start_Date, logIn.fy_End_Date)
                                 
                                 select new { c.Total_Sales }).ToList();
                    if (ST.Count > 0)
                    {
                        linkLabel2.Text = ST[0].Total_Sales.ToString(); ;
                        
                    }



                    //if (teamTotalScores.Count > 0)
                    //{
                    //    linkLabel2.Text = teamTotalScores;

                    //}



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
                if (OrderManagement.Transactions.ListOfInvoices.SO_No != null)
                {
                    txtInvNo.Text = OrderManagement.Transactions.ListOfInvoices.SO_No;
                }
                else
                {
                    txtInvNo.Text = frmCRMDashBoard.SO_No;
                }
                String myString = "";
                myString = txtInvNo.Text;
                var da = (from obj in db.Invoice_Masters
                          where obj.Inv_No == txtInvNo.Text && obj.Company_ID == logIn.company
                          select obj).ToList();

                if (da.Count > 0)
                {
                    txtInvNo.Text = da[0].Inv_No.ToString();
                    dpInvDate.Text = da[0].InvDate.ToString();
                    cmbInvType.Text = da[0].InvType;

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
                    if (da[0].Sale_Account != null)
                    {
                        cmbSaleAccount.SelectedValue = da[0].Sale_Account;
                    }                   
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
                    txtOthers.Text = da[0].Other_Charges.ToString();
                    txtTCSPer.Text = da[0].TCS_Per.ToString();
                    txtTCSAmt.Text = da[0].TCS_Amnt.ToString();
                    txtCessAmt.Text = da[0].Cess_Amount.ToString();
                    txtRounding.Text = da[0].Rounding.ToString();
                    txtLRNo.Text = da[0].InvValueInWords;
                    cmbTransporter.Text = da[0].Transporter_Name;
                    txtDespthrough.Text = da[0].DespatchThrough;
                    txtVehicleNo.Text = da[0].VehicleNo;
                    txtWayBillNo.Text = da[0].WayBillNo;
                    txtSplInstructions.Text = da[0].Spl_Instructions;                    
                    cmbStatus.SelectedValue = da[0].Status;      
                    lblCreatedBy.Text = da[0].Created_By;
                    lblModified.Text = da[0].Modified_By;
                    txtSoNo.Text = da[0].SO_Ref_No;
                    txtLRNo.Text = da[0].LR_No;
                    dtLRDate.Text = da[0].LR_Date.ToString();
                    textBox1.Text = da[0].Inv_Issue_Time;
                    textBox2.Text = da[0].Inv_Removal_Time;
                    txtPacking.Text= da[0].Packing_Charges.ToString();
                    txtInsurance.Text = da[0].Insurance_Charges.ToString();
                    if (da[0].RCM_Invoice == true)
                    {
                        checkBox1.Checked = true;
                    }
                    if (da[0].useDigitalSign == true)
                    {
                        checkBox2.Checked = true;
                    }
                    txtDCNo.Text = da[0].DC_No;
                    txtDestination.Text = da[0].Destination;
                    txtIRNNo.Text = da[0].EInv_IRN_No;
                    txtACKNo.Text = da[0].Einv_ACK_No;
                    txtACKDate.Text = da[0].Einv_ACK_Date;
                    txtQRCode.Text = da[0].EInv_QR_Code;

                }


                var dm1 = (from s in db.Invoice_Childs
                           join p in db.Products on Convert.ToInt32(s.Prod_Code) equals p.prod_ID
                           join u in db.UoM_Masters on p.Prod_Primary_UOM_Id equals u.UOM_ID
                           where s.Inv_No == myString && s.Company_ID == logIn.company
                           orderby s.Item_No


                           select new

                           {
                               Item_Code = s.Prod_Code,
                               Prod_Code = p.Prod_Code,
                               Item_Description = p.Prod_Name.Trim(),
                               Item_Grade = s.Prod_Grade.Trim(),
                               UOM = u.Uom_Descr.Trim(),
                               s.PO_Qty,
                               s.Stock_qty,
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
                               Remarks = s.Remarks.Trim(),
                               SO_Ref_No = s.SO_Ref_No.Trim(),
                               SO_Item_No = s.Item_No,
                               Stock_Price = s.Sale_Unit_Price


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
                    txtConAddress.Text = da[0].Delivery_Address;
                    //dpSODate.Text = da[0].SODate.ToString();
                    if (da[0].Multi_Loc_Delivery == true)
                    {
                        bindDeliveryAddress();
                    }
                    else
                    {
                        //bindCustomer();
                        txtConAddress.Text = da[0].Delivery_Address;
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
                    cmbTransporter.Text = da[0].Transporter_Name;
                    
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

        public void GetLoadingSlipData()
        {
            try
            {
                //txtSoNo.Text = OrderManagement.Transactions.ListOfOrders.SO_No;
                String myString = "";

                var da = (from obj in db.Loading_Childs
                          join s in db.Sale_Order_Masters on obj.SO_Ref_No equals s.SO_NO
                          join c in db.Supplier_informations on s.BuyerName equals c.ID
                          join L in db.Loading_Masters on obj.Loading_Master_ID equals L.Id
                          where obj.Loading_Slip_No == txtDCNo.Text && obj.Company_ID == logIn.company && L.BU_ID == logIn.BU_ID
                          select new {s.SO_NO,s.CustomerPONo,s.Tax_Class,s.Delivery_Address,s.BuyerName,s.ConsigneeName,c.GSTIN_NO,
                              s.PODate,s.Transporter_Name, c.StateCode}).ToList();

                if (da.Count > 0)
                {
                    SONo = da[0].SO_NO.ToString();
                    txtSoNo.Text = da[0].CustomerPONo;
                    //cmbTaxClass.SelectedValue = da[0].Tax_Class;
                    txtConAddress.Text = da[0].Delivery_Address;                 
                    CmbBuyerName.SelectedValue = da[0].BuyerName;
                    bindConsignee();
                    CmbConsigneeName.SelectedValue = da[0].ConsigneeName;
                    txtCustGSTNo.Text = da[0].GSTIN_NO;
                    txtCustStateCode.Text = da[0].StateCode;
                    dpPODate.Text = da[0].PODate.ToString();
                    cmbTransporter.Text = da[0].Transporter_Name;

                }
                else
                {
                    MessageBox.Show("Invalid Loading Slip Enterered, Cannot Proceed");
                    txtDCNo.Focus();
                    return;

                }

                SqlCommand cmd3 = new SqlCommand("Get_Products_into_Invoice", con);
                cmd3.CommandType = CommandType.StoredProcedure;
                cmd3.Parameters.AddWithValue("@compname", logIn.company);              
                cmd3.Parameters.AddWithValue("@Slip_No", txtDCNo.Text);
                cmd3.Parameters.AddWithValue("@buid", logIn.BU_ID);

                SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                //DataSet ds2 = new DataSet();
                DataTable ds3 = new DataTable();
                // da2.Fill(ds2, "x");
                da3.Fill(ds3);
                if (ds3.Rows.Count > 0)
                {
                    dgProducts.DataSource = ds3;




                }
                else
                {
                    dgProducts.DataSource = null;
                    MessageBox.Show("No Pending Products To Generate Invoice");
                    return;
                }

                //var dm1 = (from s in db.Loading_Childs
                //           join p in db.Products on Convert.ToInt32(s.Prod_Code) equals p.prod_ID
                //           join u in db.UoM_Masters on p.Prod_Primary_UOM_Id equals u.UOM_ID
                //           join so in db.Sale_Order_Childs on new { x = s.SO_Item_No, x1= s.SO_Ref_No } equals new { x=so.enq_item_no, x1=so.SO_NO }
                //           where s.Loading_Slip_No == txtDCNo.Text && s.Company_ID == logIn.company
                //           orderby s.Item_No


                //           select new

                //           {
                //               Item_Code = s.Prod_Code,
                //               Prod_Code = p.Prod_Code,
                //               Item_Description = p.Prod_Name.Trim(),
                //               Item_Grade = so.Prod_Grade.Trim(),
                //               UOM = u.Uom_Descr.Trim(),
                //               s.PO_Qty,
                //               s.Stock_qty,
                //               Inv_Qty = s.Qty_Loaded,
                //               Basic_Price = so.Price,
                //               Amt_Before_Disc = so.Amount,
                //               so.Disc_Per,
                //               Disc_Amt = so.Disc_Amount,
                //               so.Taxable_Value,
                //               so.CGST_Per,
                //               CGST_Amt = so.CGST_Amnt,
                //               so.SGST_Per,
                //               SGST_Amt = so.SGST_Amnt,
                //               so.IGST_Per,
                //               IGST_Amt = so.IGST_Amnt,
                //               Total_Amount = so.Net_Amount,
                //               Remarks = s.Remarks.Trim(),
                //               SO_Ref_No = s.SO_Ref_No.Trim(),
                //               SO_Item_No = s.Item_No,
                //               Stock_Price = 0


                //           });




                //SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                //SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataTable dtr = new DataTable();
                //da2.Fill(dtr);
                //if (dtr.Rows.Count >= 0)
                //    dgProducts.DataSource = dtr;


            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public class genConsigneeAddress
        {
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public int PinCode { get; set; }
            public string State { get; set; }
            public string StateCode { get; set; }
            public string GSTIN { get; set; }

        }
    }
}
