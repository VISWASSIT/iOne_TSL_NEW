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
using stdole;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Script.Serialization;

namespace ioneNet.OrderManagement.Transactions
{
    public partial class frmNewOrder_TSL : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string Enq_NO, Item_Shape, MtrlGrade;
        public static string SONo,ItemCode,OrdQty,CompStateCode;
        int ReceordMasterID;
        public string ParaValue = "";
        public frmNewOrder_TSL()
        {
            InitializeComponent();
        }

        private void frmNewOrder_Load(object sender, EventArgs e)
        {
            dpSODate.MinDate = logIn.fy_Start_Date;
            dpSODate.MaxDate = logIn.fy_End_Date;
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //GetPendingEnq();
           
            bindCustomer();
            bindConsignee();
            GetPendingQuotes();
            bindDroupDown_Lookup();
            if (logIn.company == 1042)
            {
                txtSoNo.Enabled = false;
                AutoincrementId();
            }
            else
            {
                txtSoNo.Enabled = true;
            }
           
            if (ListOfOrders.editMode == true)
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


            try
            {
                //Validate Consignee Address
                JObject jsoncancel = JObject.Parse(txtConAddress.Text);
                txtAddress1.Text = (string)jsoncancel.SelectToken("Address1"); ;
                txtAddress2.Text = (string)jsoncancel.SelectToken("Address2");
                cmbCity.Text = (string)jsoncancel.SelectToken("City");
                txtstate.Text = (string)jsoncancel.SelectToken("State");
                txtPincode.Text = (string)jsoncancel.SelectToken("PinCode");
                txtStateCode.Text = (string)jsoncancel.SelectToken("StateCode");
                txtConGSTIN.Text = (string)jsoncancel.SelectToken("GSTIN");
                if (txtAddress1.Text.Length > 100)
                {
                    MessageBox.Show("Address1 cannot be more than 100 characters, update the same by click on View/Edit Address Button", "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                if (txtSoNo.Text == string.Empty)
                {
                    MessageBox.Show("SO Number Should Not Be Empty", "Quotation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtSoNo.Focus();
                    return;
                }
                if (CmbBuyerName.Text == string.Empty)
                {
                    MessageBox.Show("Customer Name Should Not Be Empty", "Sale Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CmbBuyerName.Focus();
                    return;
                }
                if (CmbConsigneeName.Text == string.Empty)
                {
                    MessageBox.Show("Consignee Name Should Not Be Empty", "Sale Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CmbConsigneeName.Focus();
                    return;
                }
                else if (cmbQuotNo.Text == string.Empty)
                {
                    MessageBox.Show("Select Quote No,");
                    cmbQuotNo.Focus();
                    return;
                }
                else if (cmbStatus.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Status");
                    cmbStatus.Focus();
                    return;
                }
                if (dgProducts.Rows[0].Cells["Item_Code"].Value == null)
                {
                    MessageBox.Show("Atleast One Record To Be Entered To Proceed", "Sale Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                if (txtConAddress.Text == string.Empty)
                {
                    MessageBox.Show("Delivery Address Cannnot Be Blank");
                    CmbConsigneeName.Focus();
                    return;
                }
                if (cmbPaymentTerms.Text == string.Empty)
                {
                    MessageBox.Show("Payment Terms Should Not Be Empty", "Sale Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbPaymentTerms.Focus();
                    return;
                }
                else
                if (cmbOrderExecuteFrom.Text == string.Empty)
                {
                    MessageBox.Show("Order Execute from Cannot be Empty", "Sale Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbPaymentTerms.Focus();
                    return;
                }
                else

              if (cmbPriceBasis.Text == string.Empty)
                {
                    MessageBox.Show("Price Basis Should Not Be Empty", "Sale Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbPriceBasis.Focus();
                    return;
                }
                else

              if (txtCity.Text == string.Empty)
                {
                    MessageBox.Show("Select The Delivery Location", "Sale Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCity.Focus();
                    return;
                }
                else

              if (cmbPacking.Text == string.Empty)
                {
                    MessageBox.Show("Packing  Should Not Be Empty", "Sale Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbPacking.Focus();
                    return;
                }
                else

              if (cmbCutting.Text == string.Empty)
                {
                    MessageBox.Show("Cutting  Should Not Be Empty", "Sale Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbCutting.Focus();
                    return;
                }
                else

              if (cmbUT.Text == string.Empty)
                {
                    MessageBox.Show("UT Criteria  Should Not Be Empty", "Sale Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbUT.Focus();
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

        public void bindCustomer()
        {
            try
            {
                var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company && m.Status==1 select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    CmbBuyerName.DataSource = Buyerblind;
                    CmbBuyerName.ValueMember = "ID";
                    CmbBuyerName.DisplayMember = "Supplier_Name";
                  
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
                var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company select new { m.ID, m.Supplier_Name }).Distinct().ToList();
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

        public void GetPendingQuotes()
        {
            try
            {

                SqlCommand cmd2 = new SqlCommand("SP_GetQuotesForSaleOrder", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);


                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataSet ds2 = new DataSet();
                DataTable ds2 = new DataTable();
                // da2.Fill(ds2, "x");
                da2.Fill(ds2);
                for (int i = 0; i < ds2.Rows.Count; i++)
                {
                    cmbQuotNo.Items.Add(ds2.Rows[i]["Quot_NO"].ToString());
                }
                //var d = (from data in db.SP_GetQuotesForSaleOrder(logIn.company) select new { data.Quot_NO }).Distinct().ToList();

                //if (d.Count > 0)
                ////    var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company && m.Status == 1 select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                ////if (Buyerblind.Count > 0)
                ////{
                //    cmbQuotNo.DataSource = d;
                //    CmbBuyerName.ValueMember = "ID";
                //    CmbBuyerName.DisplayMember = "Supplier_Name";

                //}
                ////if (CmbBuyerName.Items.Count > 0)
                cmbQuotNo.SelectedIndex = -1;

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
                //var PBasis = (from m in db.Attributes_Datas where m.Head_Name == "Price Basis" select new { m.ID, m.Descr }).Distinct().ToList();
                //if (PBasis.Count > 0)
                //{
                //    cmbPriceBasis.DataSource = PBasis;
                //    cmbPriceBasis.ValueMember = "ID";
                //    cmbPriceBasis.DisplayMember = "Descr";
                //    cmbPriceBasis.SelectedIndex = -1;
                //}
                //Ut Required
                //var UT = (from m in db.Attributes_Datas where m.Head_Name == "UT Required" select new { m.ID, m.Descr }).Distinct().ToList();
                //if (UT.Count > 0)
                //{
                //    cmbUT.DataSource = UT;
                //    cmbUT.ValueMember = "ID";
                //    cmbUT.DisplayMember = "Descr";
                //    cmbUT.SelectedIndex = -1;
                //}
                //cutting
                //var cut = (from m in db.Attributes_Datas where m.Head_Name == "Cutting" select new { m.ID, m.Descr }).Distinct().ToList();
                //if (cut.Count > 0)
                //{
                //    cmbCutting.DataSource = cut;
                //    cmbCutting.ValueMember = "ID";
                //    cmbCutting.DisplayMember = "Descr";
                //    cmbCutting.SelectedIndex = -1;
                //}

                //Packing
                var Pack = (from m in db.Costing_Units where m.Company == logIn.company select new { m.BU_ShortName, m.id }).Distinct().ToList();
                if (Pack.Count > 0)
                {
                    cmbOrderExecuteFrom.DataSource = Pack;
                    cmbOrderExecuteFrom.ValueMember = "id";
                    cmbOrderExecuteFrom.DisplayMember = "BU_ShortName";
                    cmbOrderExecuteFrom.SelectedIndex = -1;
                }

                //Payment Terms
                var PTerms = (from m in db.Attributes_Datas where m.Head_Name == "Payment Terms" select new { m.ID, m.Descr }).Distinct().ToList();
                if (PTerms.Count > 0)
                {
                    cmbPaymentTerms.DataSource = PTerms;
                    cmbPaymentTerms.ValueMember = "ID";
                    cmbPaymentTerms.DisplayMember = "Descr";
                    cmbPaymentTerms.SelectedIndex = -1;
                }

                //Currency
                var Cur = (from m in db.Currency_Masters select new { m.id, m.Currency_Code }).Distinct().ToList();
                if (Cur.Count > 0)
                {
                    cmbcurrency.DataSource = Cur;
                    cmbcurrency.ValueMember = "id";
                    cmbcurrency.DisplayMember = "Currency_Code";
                }


                //Status
                var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Trans" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbStatus.DataSource = pStatus;
                    cmbStatus.ValueMember = "ID";
                    cmbStatus.DisplayMember = "Descr";
                }
               
                //Transportation
                //var pTrans = (from m in db.Attributes_Datas where m.Head_Name == "Transportation" select new { m.ID, m.Descr }).Distinct().ToList();
                //if (pTrans.Count > 0)
                //{
                //    cmbTransport_Scope.DataSource = pTrans;
                //    cmbTransport_Scope.ValueMember = "ID";
                //    cmbTransport_Scope.DisplayMember = "Descr";
                //}

                
               
                


                

                //Bind Sale Executive
                var bindSE = (from m in db.Sales_Men_Informations
                               where m.Company_ID == logIn.company
                               select new
                               {
                                   m.Salesmen_Code,
                                   m.Id,
                               }).ToList();

                if (bindSE.Count > 0)
                {
                    cmbSaleExecutive.DisplayMember = "Salesmen_Code";
                    cmbSaleExecutive.ValueMember = "Id";
                    cmbSaleExecutive.DataSource = bindSE;

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

                var result = db.Sp_autoincrement_SaleOrder (logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date,logIn.BU_ID);
                txtSoNo.Text = result.FirstOrDefault().So_no;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private Boolean numkey = false;

        private void dgProducts_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                numkey = false;
                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].HeaderText;
                TextBox tb3 = e.Control as TextBox;
                tb3.AutoCompleteMode = AutoCompleteMode.None;                
              if (tb3 != null && columnName == "UOM")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }
                if (tb3 != null && columnName == "Item Grade")
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
                if (columnName == "Length")
                {
                    numkey = true;
                    e.Control.KeyPress += new KeyPressEventHandler(CheckKey);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CheckKey(object sender, KeyPressEventArgs e)
        {
            if (numkey == true)
            {
                if (!char.IsControl(e.KeyChar)
                    && !char.IsDigit(e.KeyChar)
                    && e.KeyChar != '-'
                    && e.KeyChar != '.')
                {
                    e.Handled = true;
                }
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
                
                
                    if (columnName == "Item Description")
                    {
                        var Prodname = (from d in db.Products where d.Company_ID == logIn.company && d.Prod_Type_Id == 139 || d.Prod_Type_Id == 141 || d.Prod_Type_Id == 157 select new { d.Prod_Name }).ToList();
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

                if (columnName == "Item Grade")
                {
                    var Prodname = (from d in db.QA_Mtrl_Grade_Masters select new { d.Material_Grade }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Item_Grade");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Material_Grade);
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
                //int s = CmbBuyerName.SelectedIndex;

                if (CmbBuyerName.Text != "")                   
                {
                    int i = (CmbBuyerName.FindString(CmbBuyerName.Text));
                    if (i < 0)
                    {
                        MessageBox.Show("Invalid Customer Name Selected");
                        CmbBuyerName.Focus();
                        return;
                    }
                    else
                    {
                        var State = (from c1 in db.Supplier_informations
                                     join c3 in db.Attributes_Datas on c1.Area_Region equals c3.ID
                                     where c1.ID == Convert.ToInt32(CmbBuyerName.SelectedValue) && c1.Supplier_Name == CmbBuyerName.Text
                                     select new { c1.StateCode, c1.Contact_Person, c1.Contact_Mobile, c1.Sale_Executive, c1.Area_Region, c3.Descr, c1.City }).ToList();
                        if (State.Count > 0)
                        {
                            //txtCustGSTNo.Text = State[0].GSTIN_NO;
                            txtCustomeContact.Text = State[0].Contact_Person + "-" + State[0].Contact_Mobile;
                            //cmbSaleOffice.Text = State[0].Descr;
                            cmbSaleExecutive.SelectedValue = State[0].Sale_Executive;
                            txtCity.Text = State[0].City.ToString();

                        }
                    }
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
               
                var d1 = (from a in db.Company_Infos where a.Id == logIn.company select new { a.State_Code }).ToList();
                if (d1.Count > 0)
                {
                    CompStateCode = d1[0].State_Code;
                }
                    DataTable dtexisting = new DataTable();
                if (e.KeyCode == Keys.F2)
                {
                    ioneNet.Masters.ProdSearch form = new ioneNet.Masters.ProdSearch();
                    ioneNet.Masters.ProdSearch.frmName = "Quote";                                   
                    form.ShowDialog();
                    if (dgProducts.Rows.Count > 1)
                    {
                        dtexisting.Rows.Clear();
                        dtexisting.Columns.Clear();
                        dtexisting.Columns.Add("Item_Code", typeof(string));
                        dtexisting.Columns.Add("Prod_Code", typeof(string));
                        dtexisting.Columns.Add("Item_Description", typeof(string));
                        dtexisting.Columns.Add("Item_Grade", typeof(string));
                        dtexisting.Columns.Add("UOM", typeof(string));
                        dtexisting.Columns.Add("Qty", typeof(decimal));
                        dtexisting.Columns.Add("Basic_Price", typeof(decimal));
                        dtexisting.Columns.Add("Amt_Before_Disc", typeof(decimal));
                        dtexisting.Columns.Add("Disc_Per", typeof(decimal));
                        dtexisting.Columns.Add("Disc_Amt", typeof(decimal));
                        dtexisting.Columns.Add("Taxable_Value", typeof(decimal));
                       
                        dtexisting.Columns.Add("Remarks", typeof(string));

                        for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                        {
                            DataRow dr;
                            dr = dtexisting.NewRow();
                            dr["Item_Code"] = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                            dr["Prod_Code"] = dgProducts.Rows[i].Cells["Prod_Code"].Value.ToString();
                            dr["Item_Description"] = dgProducts.Rows[i].Cells["Item_Description"].Value.ToString();
                            if (dgProducts.Rows[i].Cells["Item_Grade"].Value != null)
                            {
                                dr["Item_Grade"] = dgProducts.Rows[i].Cells["Item_Grade"].Value.ToString();
                            }
                            dr["UOM"] = dgProducts.Rows[i].Cells["UOM"].Value.ToString();
                            dr["Qty"] = dgProducts.Rows[i].Cells["Qty"].Value.ToString();
                            dr["Basic_Price"] = dgProducts.Rows[i].Cells["Basic_Price"].Value.ToString();
                            dr["Amt_Before_Disc"] = dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value.ToString();
                            dr["Disc_Per"] = dgProducts.Rows[i].Cells["Disc_Per"].Value.ToString();
                            dr["Disc_Amt"] = dgProducts.Rows[i].Cells["Disc_Amt"].Value.ToString();
                            dr["Taxable_Value"] = dgProducts.Rows[i].Cells["Taxable_Value"].Value.ToString();
                           
                            dr["Remarks"] = dgProducts.Rows[i].Cells["Remarks"].Value.ToString();
                            dtexisting.Rows.Add(dr);

                        }
                        dtexisting.AcceptChanges();
                    }






                    if (ioneNet.Masters.ProdSearch.dtgetproducts.Rows.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Item_Code", typeof(string));
                        dt.Columns.Add("Prod_Code", typeof(string));
                        dt.Columns.Add("Item_Description", typeof(string));
                        dt.Columns.Add("Item_Grade", typeof(string));
                        dt.Columns.Add("UOM", typeof(string));
                        dt.Columns.Add("Qty", typeof(decimal));
                        dt.Columns.Add("Basic_Price", typeof(decimal));
                        dt.Columns.Add("Amt_Before_Disc", typeof(decimal));
                        dt.Columns.Add("Disc_Per", typeof(decimal));
                        dt.Columns.Add("Disc_Amt", typeof(decimal));
                        dt.Columns.Add("Taxable_Value", typeof(decimal));
                      
                        dt.Columns.Add("Remarks", typeof(string));

                        //dt.Rows.Add();
                        for (int i = 0; i < ioneNet.Masters.ProdSearch.dtgetproducts.Rows.Count; i++)
                        {
                            string prodcode = ioneNet.Masters.ProdSearch.dtgetproducts.Rows[i]["prod_id"].ToString();
                            string MRP = "0";
                            var getprice = (from p in db.Prod_PriceLists
                                            where p.Prod_ID == Convert.ToInt32(prodcode)
                                            select new { p.MRP }
                                            ).ToList();
                            if(getprice.Count>0)
                            {
                                MRP = getprice[0].MRP.ToString();
                            }
                            var getproducts = (from obj in db.Products
                                               join uom in db.UoM_Masters on obj.Prod_Primary_UOM_Id equals uom.UOM_ID
                                               join tm in db.Tax_Class_Masters on obj.Prod_Tax_Class equals tm.ID
                                               where obj.prod_ID == Convert.ToInt32(prodcode)
                                               select new
                                               {
                            
                                                   // {
                                                   Item_Code = obj.prod_ID,
                                                   Prod_Code = obj.Prod_Code,
                                                   Item_Description = obj.Prod_Name,
                                                   Item_Grade = "",
                                                   UOM = uom.Uom_Descr,
                                                   Qty = 0,
                                                   Basic_Price = MRP,
                                                   Amt_Before_Disc = 0,
                                                   Disc_Per = 0,
                                                   Disc_Amt = 0,
                                                   Taxable_Value = 0,                                                  
                                                 
                                                   Remarks = ""
                                               }).ToList();
                            dt.Rows.Add(getproducts[0].Item_Code, getproducts[0].Prod_Code,getproducts[0].Item_Description, getproducts[0].Item_Grade, getproducts[0].UOM, getproducts[0].Qty, getproducts[0].Basic_Price, getproducts[0].Amt_Before_Disc, getproducts[0].Disc_Per, getproducts[0].Disc_Amt, getproducts[0].Taxable_Value, getproducts[0].Remarks);

                        }

                        dtexisting = dtexisting.AsEnumerable().Union(dt.AsEnumerable()).CopyToDataTable();
                        dgProducts.DataSource = dtexisting;
                    }
                }
                if (e.KeyCode == Keys.F3)
                {
                    //ioneNet.OrderManagement.Transactions.frmEnqProductSpecs form = new ioneNet.OrderManagement.Transactions.frmEnqProductSpecs();
                    //Enq_NO = txtEnqNo.Text;
                    //DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
                    //int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                    //ItemCode = R1.Cells["Item_code"].Value.ToString();
                    ////Item_Shape = txtShape.Text;
                    //MtrlGrade = R1.Cells["Item_Grade"].Value.ToString(); ;
                    //frmMain.frmname = "Quotation";
                    //form.ShowDialog();
                   
                }

                if (e.KeyCode == Keys.F5)
                {
                    ioneNet.MaterialManagement.Masters.frmProductsNew form = new ioneNet.MaterialManagement.Masters.frmProductsNew();
                    //ioneNet.Masters.ProdSearch.frmName = "SOrder";                                   
                    form.ShowDialog();
                }
                if (e.KeyCode == Keys.F4) //Delivery Information / Locations
                {
                    //if (chkMultiLocation.Checked == true)
                    //{
                    //    ioneNet.OrderManagement.Transactions.SO_MultiDeliveryAddress form = new OrderManagement.Transactions.SO_MultiDeliveryAddress();
                    //    //ioneNet.Masters.ProdSearch.frmName = "SOrder";
                    //    int i = dgProducts.CurrentCell.RowIndex;
                    //    SONo = txtSoNo.Text;
                    //    ItemCode = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                    //    OrdQty = dgProducts.Rows[i].Cells["Qty"].Value.ToString();
                    //    form.ShowDialog();
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Check Multi Location Delivery? To Add The Details");

                    //}
                }
                if (e.KeyCode == Keys.F6) //Remove Rows
                {
                    if (dgProducts.Rows.Count > 0)
                    {

                        foreach (DataGridViewCell oneCell in dgProducts.SelectedCells)
                        {
                            if (oneCell.Selected)
                                dgProducts.Rows.RemoveAt(oneCell.RowIndex);
                        }
                        decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
                        for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                        {

                            x += (dgProducts.Rows[i].Cells["Qty"].Value == "" || dgProducts.Rows[i].Cells["Qty"].Value == null || dgProducts.Rows[i].Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty"].Value);
                            y += (dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == "" || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == null || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value);
                            q += (dgProducts.Rows[i].Cells["Disc_Amt"].Value == "" || dgProducts.Rows[i].Cells["Disc_Amt"].Value == null || dgProducts.Rows[i].Cells["Disc_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Amt"].Value);
                            v += (dgProducts.Rows[i].Cells["Taxable_Value"].Value == "" || dgProducts.Rows[i].Cells["Taxable_Value"].Value == null || dgProducts.Rows[i].Cells["Taxable_Value"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Taxable_Value"].Value);
                            //cg += (dgProducts.Rows[i].Cells["CGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["CGST_Amt"].Value == null || dgProducts.Rows[i].Cells["CGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Amt"].Value);
                            //sg += (dgProducts.Rows[i].Cells["SGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["SGST_Amt"].Value == null || dgProducts.Rows[i].Cells["SGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Amt"].Value);
                            //ig += (dgProducts.Rows[i].Cells["IGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["IGST_Amt"].Value == null || dgProducts.Rows[i].Cells["IGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Amt"].Value);
                            //totA += (dgProducts.Rows[i].Cells["Total_Amount"].Value == "" || dgProducts.Rows[i].Cells["Total_Amount"].Value == null || dgProducts.Rows[i].Cells["Total_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Total_Amount"].Value);

                        }

                        txtTotalQty.Text = x.ToString(".00");
                        txtSubTotal.Text = y.ToString("0.00");
                        
                        txtTot_TaxableValue.Text = v.ToString(".00");
                        //txtTot_CGST.Text = cg.ToString(".00");
                        //txtTot_SGST.Text = sg.ToString(".00");
                        //txtTot_IGST.Text = ig.ToString(".00");
                        ////decimal fAmt = (txtFrieght.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtFrieght.Text);

                        //txtTot_OrderValue.Text = (totA).ToString(".00");

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
                decimal taxRate=0;
                DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].Name;
                if (columnName == "Delivery_Date")
                {
                    string eDate = R1.Cells["Delivery_Date"].Value.ToString();
                    DateTime t = Convert.ToDateTime(R1.Cells["Delivery_Date"].Value.ToString());

                    // DateTime D = DateTime.ParseExact(eDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else

                    if (columnName == "Item_Description" || columnName == "Item_Grade")
                {
                    string prodgrade = "";
                    string prodname =  (R1.Cells["Item_Description"].Value == null) ? "" : (R1.Cells["Item_Description"].Value.ToString());
                    //string prodname = R1.Cells["Item_Description"].Value.ToString();
                    if (prodname != "")
                    {
                        if (R1.Cells["Item_Grade"].Value != null)
                        {
                            prodgrade = R1.Cells["Item_Grade"].Value.ToString();
                        }




                        var getProductName = (from s in db.Get_ProductsList_TSL(logIn.company, 1, prodname, prodgrade)
                                              select new { s.prod_ID, s.Prod_Code, s.Uom_Descr, s.Prod_Group_Name, s.Prod_HSN_Code, s.Gst_Rate, s.Prod_Customer_Code, s.Price }).FirstOrDefault();



                        if (getProductName != null)
                        {
                            R1.Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                            R1.Cells["Item_code"].Value = getProductName.prod_ID.ToString();
                            R1.Cells["Int_Prod_Code"].Value = getProductName.Prod_Code.ToString();
                            R1.Cells["HSN_Code"].Value = getProductName.Prod_HSN_Code.ToString();
                            R1.Cells["Basic_Price"].Value = "0.00";
                            if (getProductName.Price != null)
                            {
                                R1.Cells["Basic_Price"].Value = getProductName.Price.ToString();
                            }

                            if (getProductName.Prod_Customer_Code != null)
                            {
                                R1.Cells["Remarks"].Value = getProductName.Prod_Customer_Code.ToString();
                            }
                            taxRate = Convert.ToDecimal(getProductName.Gst_Rate);
                            //R1.Cells["Prod_Tole_Qty"].Value = "0";
                            // R1.Cells["Delivery_Date"].Value = DateTime.Today.ToString("MM/dd/yyyy"); 
                        }

                        else
                        {
                            R1.Cells["Item_Code"].Value = dgProducts.CurrentCell.RowIndex + 1;
                            //R1.Cells["Prod_Tole_Qty"].Value = "0";
                            //R1.Cells["Delivery_Date"].Value = DateTime.Today.ToString("MM/dd/yyyy");
                        }
                        decimal b, c, d;

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
                            suppStateCode = "0";
                            if (chkSEZOrder.Checked == false)
                            {

                            }
                        }

                    }
                }
                    if(columnName =="Qty")
                    {
                        decimal b, c, d;
                   
                        var getProductName = (from s in db.Get_ProductsList(logIn.company, 1, R1.Cells["Item_Description"].Value.ToString())
                                              select new { s.prod_ID, s.Uom_Descr, s.Prod_Group_Name, s.Prod_HSN_Code, s.Gst_Rate }).FirstOrDefault();


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
                            taxRate = 18;
                        }
                        var d1 = (from a in db.Company_Infos where a.Id == logIn.company select new { a.GST_No,a.State_Code }).ToList();
                        if (d1.Count > 0)
                        {
                            comnpstatecode = d1[0].State_Code;
                            suppStateCode = "0";
                            if (chkSEZOrder.Checked == false)
                            {
                            //    if (suppStateCode == comnpstatecode)
                            //    {
                            //        d = Convert.ToDecimal(taxRate) / 2;
                            //        R1.Cells["CGST_Per"].Value = d.ToString("0.00");
                            //        R1.Cells["SGST_Per"].Value = d.ToString("0.00");
                            //        R1.Cells["IGST_Per"].Value = "0.00";
                            //    }
                            //    else
                            //    {
                            //        d = taxRate;
                            //        R1.Cells["CGST_Per"].Value = "0.00";
                            //        R1.Cells["SGST_Per"].Value = "0.00";
                            //        R1.Cells["IGST_Per"].Value = d.ToString("0.00");
                            //    }
                            //}
                            //else
                            //{
                            //    R1.Cells["CGST_Per"].Value = "0.00";
                            //    R1.Cells["SGST_Per"].Value = "0.00";
                            //    R1.Cells["IGST_Per"].Value = "0.00";
                            }
                        }

                    }
                if (R1.Cells["Item_Description"].Value != null)
                {
                    decimal ReceivedQty = (R1.Cells["Qty"].Value == "" || R1.Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty"].Value);
                    //decimal RejectedQty = (R1.Cells["Qty_Rejected"].Value == "" || R1.Cells["Qty_Rejected"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty_Rejected"].Value);

                    string Custstatetcode;

                    if (columnName == "Basic_Price" || columnName == "Disc_Per" || columnName == "Qty")
                    {
                        if (ReceivedQty > 0)
                        {
                            decimal AcceptedQty = (R1.Cells["Qty"].Value == "" || R1.Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty"].Value);
                            decimal price = (R1.Cells["Basic_Price"].Value == "" || R1.Cells["Basic_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Basic_Price"].Value);
                            decimal DiscPer = (R1.Cells["Disc_Per"].Value == "" || R1.Cells["Disc_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Disc_Per"].Value);


                            decimal Amt, DiscAmt, netAmt, gst, igst, totamt;

                            DiscAmt = price- DiscPer;
                            if (DiscAmt > 0)
                            {
                                R1.Cells["Disc_Amt"].Value = DiscAmt.ToString("0.00");
                            }
                            else
                            {
                                R1.Cells["Disc_Amt"].Value = price.ToString("0.00");
                            }
                            decimal Actprice = (R1.Cells["Disc_Amt"].Value == "" || R1.Cells["Disc_Amt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Disc_Amt"].Value);
                            Amt = AcceptedQty * Actprice;

                            R1.Cells["Amt_Before_Disc"].Value = Amt.ToString("0.00");
                            
                           
                            netAmt = Amt - DiscAmt;
                            //R1.Cells["Taxable_Value"].Value = netAmt.ToString("0.00");
                            //gst = (Convert.ToDecimal(R1.Cells["Taxable_Value"].Value) * Convert.ToDecimal(R1.Cells["CGST_Per"].Value)) / 100;

                            //R1.Cells["CGST_Amt"].Value = gst;
                            //R1.Cells["SGST_Amt"].Value = gst;
                            ////igst = (Convert.ToDecimal(R1.Cells["Taxable_Value"].Value) * Convert.ToDecimal(R1.Cells["IGST_Per"].Value)) / 100;
                            //R1.Cells["IGST_Amt"].Value = igst;

                            //totamt = Math.Round(netAmt + gst + gst + igst);
                            //R1.Cells["Total_Amount"].Value = totamt;
                            decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
                            for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                            {

                                x += (dgProducts.Rows[i].Cells["Qty"].Value == "" || dgProducts.Rows[i].Cells["Qty"].Value == null || dgProducts.Rows[i].Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty"].Value);
                                y += (dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == "" || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == null || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value);
                                //q += (dgProducts.Rows[i].Cells["Disc_Amt"].Value == "" || dgProducts.Rows[i].Cells["Disc_Amt"].Value == null || dgProducts.Rows[i].Cells["Disc_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Amt"].Value);
                                //v += (dgProducts.Rows[i].Cells["Taxable_Value"].Value == "" || dgProducts.Rows[i].Cells["Taxable_Value"].Value == null || dgProducts.Rows[i].Cells["Taxable_Value"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Taxable_Value"].Value);
                               // cg += (dgProducts.Rows[i].Cells["CGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["CGST_Amt"].Value == null || dgProducts.Rows[i].Cells["CGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Amt"].Value);
                                //sg += (dgProducts.Rows[i].Cells["SGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["SGST_Amt"].Value == null || dgProducts.Rows[i].Cells["SGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Amt"].Value);
                                //ig += (dgProducts.Rows[i].Cells["IGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["IGST_Amt"].Value == null || dgProducts.Rows[i].Cells["IGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Amt"].Value);
                               // totA += (dgProducts.Rows[i].Cells["Total_Amount"].Value == "" || dgProducts.Rows[i].Cells["Total_Amount"].Value == null || dgProducts.Rows[i].Cells["Total_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Total_Amount"].Value);

                            }

                            txtTotalQty.Text = x.ToString(".00");
                            txtSubTotal.Text = y.ToString("0.00");
                            //txtTotDiscount.Text = q.ToString(".00");
                            //txtTot_TaxableValue.Text = v.ToString(".00");
                            //txtTot_CGST.Text = cg.ToString(".00");
                            //txtTot_SGST.Text = sg.ToString(".00");
                            //txtTot_IGST.Text = ig.ToString(".00");
                            ////decimal fAmt = (txtFrieght.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtFrieght.Text);

                            //txtTot_OrderValue.Text = (totA).ToString(".00");
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

        private void dgProducts_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                if (CmbBuyerName.Text == "")
                {
                    MessageBox.Show("Select Customer Name To Proceed");
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

        private void chkRepeatOrder_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxAutoComplete1_Leave(object sender, EventArgs e)
        {
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }

        private void CmbBuyerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
               

                if (CmbBuyerName.Text != "")

                {
                    //int ss= CmbBuyerName.ValueMember;
                    int s = CmbBuyerName.SelectedIndex;
                    var State = (from c in db.Supplier_informations
                                 //oin a in db.AccountMasters on c.Supplier_Id equals a.AccCode
                                 //where c.ID == Convert.ToInt32(CmbBuyerName.SelectedValue)
                                 where c.Supplier_Name == CmbBuyerName.Text
                                 select new { c.GSTIN_NO, c.StateCode }).ToList();
                    if (State.Count > 0)
                    {
                        //txtCustGSTNo.Text = State[0].GSTIN_NO;
                        

                        //Get Order data and pending receivables

                        DateTime t = dpSODate.Value;
                        string t1 = t.ToString("dd/MMM/yyyy");
                        //var getBal = (from b in db.GetAccountBalance(logIn.company, t, State[0].id, 1,logIn.BU_ID)
                        //              select new { b.Balance, b.BalType }).FirstOrDefault();
                        //if (getBal != null)
                        //{
                        //    //linkLabel5.Text = getBal.Balance.ToString() + getBal.BalType.ToString();
                        //}
                        //else
                        //{
                        //    //linkLabel5.Text = "0";
                        //}
                        //var getOrd = (from b in db.PreviousHistory(logIn.company, Convert.ToInt32(CmbBuyerName.SelectedValue))
                        //              select new { b.TotalOrders, b.PendingOrderValue }).FirstOrDefault();
                        //if (getOrd != null)
                        //{
                        //   // linkLabel2.Text = getOrd.TotalOrders.ToString();
                        //    //linkLabel4.Text = getOrd.PendingOrderValue.ToString();
                        //}
                        //else
                        //{
                        //    //linkLabel2.Text = "0";
                        //    //linkLabel4.Text = "0";
                        //}
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void cmbPriceBasis_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbPaymentTerms_Leave(object sender, EventArgs e)
        {
            if(cmbPaymentTerms.Text =="Custom")
            {
                txtPaymentTermsCustom.Enabled = true;
            }
            else
            {
                txtPaymentTermsCustom.Enabled = false;
            }
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
           
        }

        private void txtSoNo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtSoNo.Text != "")
                {

                    if (ListOfQuotes.editMode == true)
                    {
                    }
                    else
                    {

                        if ((from u in db.Sale_Quotation_Masters where u.Quot_NO == txtSoNo.Text && u.Company_ID == logIn.company select u).Count() > 0)
                        {
                            MessageBox.Show("Quotation No Cannot Be Duplicate", "Quotation Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtSoNo.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Quotation Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cmdPrevOrder_Click(object sender, EventArgs e)
        {

        }

        private void txtEnqNo_Enter(object sender, EventArgs e)
        {
            try
            {

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtEnqNo_Leave(object sender, EventArgs e)
        {
         
        }

        private void label19_Click(object sender, EventArgs e)
        {

        }

        private void label24_Click(object sender, EventArgs e)
        {

        }

        private void label22_Click(object sender, EventArgs e)
        {
            GlobalVariables.attrdesc = "Price Basis";
            OrderManagement.Masters.frmattributesdata form = new OrderManagement.Masters.frmattributesdata();

            form.ShowDialog();
        }

        private void label25_Click(object sender, EventArgs e)
        {
            GlobalVariables.attrdesc = "Transportation";
            OrderManagement.Masters.frmattributesdata form = new OrderManagement.Masters.frmattributesdata();

            form.ShowDialog();
        }

        private void label29_Click(object sender, EventArgs e)
        {
            GlobalVariables.attrdesc = "PnF Charges";
            OrderManagement.Masters.frmattributesdata form = new OrderManagement.Masters.frmattributesdata();

            form.ShowDialog();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GlobalVariables.attrdesc = "Payment Terms";
            OrderManagement.Masters.frmattributesdata form = new OrderManagement.Masters.frmattributesdata();

            form.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GlobalVariables.attrdesc = "Price Basis";
            OrderManagement.Masters.frmattributesdata form = new OrderManagement.Masters.frmattributesdata();

            form.ShowDialog();
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GlobalVariables.attrdesc = "Transportation";
            OrderManagement.Masters.frmattributesdata form = new OrderManagement.Masters.frmattributesdata();

            form.ShowDialog();
        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        public void AddPara(AutoCompleteStringCollection coll)
        {
            try
            {

                var Prodname = (from d in db.Attributes_Datas
                                where d.Company_ID == logIn.company && d.Head_Name == ParaValue


                                select new { d.Descr }).ToList();
                DataTable dt = new DataTable();
                dt.Columns.Add("Head_Name");
                foreach (var item in Prodname)
                {
                    dt.Rows.Add(item.Descr);
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    coll.Add(dt.Rows[i][0].ToString());
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void cmbCutting_Enter(object sender, EventArgs e)
        {
            try
            {
                ParaValue = "Cutting";
                AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                AddPara(DataColl);
                cmbCutting.AutoCompleteCustomSource = DataColl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbCutting_Leave(object sender, EventArgs e)
        {
            try
            {
                //    txtSoNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_No;
                //txtAmendNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_Amend_No;
                if (cmbCutting.Text != "")
                {

                    var da = (from obj in db.Attributes_Datas
                              where obj.Head_Name == "Cutting" && obj.Descr == cmbCutting.Text
                              select obj).ToList();

                    if (da.Count > 0)
                    {

                        txtCutting.Text = da[0].ID.ToString();

                    }
                    else
                    {
                        MessageBox.Show("Invalid Entry, Click on Link to Add New Values");
                        cmbCutting.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbPacking_Enter(object sender, EventArgs e)
        {
            try
            {
                ParaValue = "Packing";
                AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                AddPara(DataColl);
                cmbPacking.AutoCompleteCustomSource = DataColl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbPacking_Leave(object sender, EventArgs e)
        {
            try
            {
                //    txtSoNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_No;
                //txtAmendNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_Amend_No;
                if (cmbPacking.Text != "")
                {

                    var da = (from obj in db.Attributes_Datas
                              where obj.Head_Name == "Packing" && obj.Descr == cmbPacking.Text
                              select obj).ToList();

                    if (da.Count > 0)
                    {

                        txtPacking.Text = da[0].ID.ToString();

                    }
                    else
                    {
                        MessageBox.Show("Invalid Entry, Click on Link to Add New Values");
                        cmbPacking.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbUT_Enter(object sender, EventArgs e)
        {
            try
            {
                ParaValue = "UT Required";
                AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                AddPara(DataColl);
                cmbUT.AutoCompleteCustomSource = DataColl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbUT_Leave(object sender, EventArgs e)
        {
            try
            {
                //    txtSoNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_No;
                //txtAmendNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_Amend_No;
                if (cmbUT.Text != "")
                {

                    var da = (from obj in db.Attributes_Datas
                              where obj.Head_Name == "UT Required" && obj.Descr == cmbUT.Text
                              select obj).ToList();

                    if (da.Count > 0)
                    {

                        txtUT.Text = da[0].ID.ToString();

                    }
                    else
                    {
                        MessageBox.Show("Invalid Entry, Click on Link to Add New Values");
                        cmbUT.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void linkLabel2_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GlobalVariables.attrdesc = "Cutting";
            OrderManagement.Masters.frmattributesdata form = new OrderManagement.Masters.frmattributesdata();

            form.ShowDialog();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GlobalVariables.attrdesc = "UT Required";
            OrderManagement.Masters.frmattributesdata form = new OrderManagement.Masters.frmattributesdata();

            form.ShowDialog();
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GlobalVariables.attrdesc = "Packing";
            OrderManagement.Masters.frmattributesdata form = new OrderManagement.Masters.frmattributesdata();

            form.ShowDialog();
        }

        private void label9_Click(object sender, EventArgs e)
        {
            GlobalVariables.attrdesc = "Sales Office";
            OrderManagement.Masters.frmattributesdata form = new OrderManagement.Masters.frmattributesdata();

            form.ShowDialog();
        }

        private void cmbPriceBasis_Enter(object sender, EventArgs e)
        {
            try
            {
                ParaValue = "Price Basis";
                AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                AddPara(DataColl);
                cmbPriceBasis.AutoCompleteCustomSource = DataColl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbPriceBasis_Leave(object sender, EventArgs e)
        {
            try
            {
                //    txtSoNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_No;
                //txtAmendNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_Amend_No;
                if (cmbPriceBasis.Text != "")
                {

                    var da = (from obj in db.Attributes_Datas
                              where obj.Head_Name == "Price Basis" && obj.Descr == cmbPriceBasis.Text
                              select obj).ToList();

                    if (da.Count > 0)
                    {

                        txtPriceBasis.Text = da[0].ID.ToString();

                    }
                    else
                    {
                        MessageBox.Show("Invalid Entry, Click on Link to Add New Values");
                        cmbPriceBasis.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbCity_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtCustomeContact_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbQuotNo_Leave(object sender, EventArgs e)
        {
            try
            {
                //    txtSoNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_No;
                //txtAmendNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_Amend_No;
                if (cmbQuotNo.Text != "NA" && cmbQuotNo.Text != "")
                {
                    String myString = "";
                    int QuoteMasterID = 0;

                    myString = txtSoNo.Text;

                    var da = (from obj in db.getQuote_Master_data(logIn.company, logIn.BU_ID, cmbQuotNo.Text)
                              select new
                              {
                                  obj.Id,
                                  obj.Quot_NO,
                                  obj.Quot_Date,
                                  obj.BuyerName,
                                  obj.Delivery_Terms,
                                  obj.Other_Terms,
                                  obj.SEZ_Order,
                                  obj.TPI_Involved,
                                  obj.IBR_Cert,
                                  obj.UT_Required,
                                  obj.EnqNo,
                                  obj.EnqDate,
                                  obj.Tot_Discount,
                                  obj.Tot_TaxableValue,
                                  obj.TotalQty,
                                  obj.SubTotal,
                                  obj.CustomerRefDoc,
                                  obj.Quot_Amend_No,
                                  obj.Payment_Terms_Custom,
                                  obj.Technical_Terms,
                                  obj.Spl_Instructions,
                                  obj.Quote_Validity,
                                  obj.Quote_Subject,
                                  obj.Quot_Amend_Date,
                                  obj.Macro,
                                  obj.Customer_Contact,
                                  obj.Sale_office,
                                  obj.SaleExecutive,
                                  obj.PaymentTerms,
                                  obj.Price_Basis,
                                  obj.Packing,
                                  obj.Cutting,
                                  obj.Status,
                                  obj.Modified_By,
                                  obj.Created_By,

                                  obj.Sales_Desc,

                                  obj.Payment_Desc,
                                  obj.UT_Desc,
                                  obj.Executive_Desc,
                                  obj.Price_Desc,
                                  obj.Packing_Desc,
                                  obj.Cutting_Desc,

                              }).ToList();

                    if (da.Count > 0)
                    {

                        dpQuotDate.Text = da[0].Quot_Date.ToString();
                        //bindCustomer();
                        CmbBuyerName.SelectedValue = da[0].BuyerName;


                        if (da[0].SEZ_Order == true)
                        {
                            chkSEZOrder.Checked = true;
                        }
                        else
                        {
                            chkSEZOrder.Checked = false;
                        }


                        if (da[0].Packing_Desc != null)
                        {
                            txtPacking.Text = da[0].Packing.ToString();
                            cmbPacking.Text = da[0].Packing_Desc.ToString();
                        }
                        if (da[0].Cutting_Desc != null)
                        {
                            txtCutting.Text = da[0].Cutting.ToString();
                            cmbCutting.Text = da[0].Cutting_Desc.ToString();
                        }

                        if (da[0].UT_Desc != null)
                        {
                            txtUT.Text = da[0].UT_Required.ToString();
                            cmbUT.Text = da[0].UT_Desc.ToString();
                        }




                        cmbSaleExecutive.SelectedValue = da[0].SaleExecutive;
                        if (da[0].Price_Desc != null)
                        {
                            txtPriceBasis.Text = da[0].Price_Basis.ToString();
                            cmbPriceBasis.Text = da[0].Price_Desc.ToString();
                        }
                        //if (da[0].Insurance_Scope == null)
                        //{
                        //    cmbInsurance.SelectedValue = 11;
                        //}
                        //else
                        //{
                        //    cmbInsurance.SelectedValue = da[0].Insurance_Scope; 
                        //}

                        if (da[0].PaymentTerms == null)
                        {
                            cmbPaymentTerms.SelectedValue = 35;
                        }
                        else
                        {
                            //cmbPaymentTerms.SelectedValue = da[0].PaymentTerms;
                            if (da[0].PaymentTerms != null)
                            {
                                cmbPaymentTerms.SelectedValue = da[0].PaymentTerms;
                                if (cmbPaymentTerms.Text == "Custom")
                                {
                                    txtPaymentTermsCustom.Text = da[0].Payment_Terms_Custom;
                                }
                                else
                                {
                                    txtPaymentTermsCustom.Text = "";
                                }
                            }
                            else
                            {
                                cmbPaymentTerms.SelectedValue = 87;
                            }

                        }
                        txtSplInstructions.Text = da[0].Spl_Instructions;
                        txtCustPoNo.Text = da[0].CustomerRefDoc;
                        txtDeliveryTerms.Text = da[0].Delivery_Terms;
                        txtCustomeContact.Text = da[0].Customer_Contact;
                    }
                    //Get Products
                    SqlCommand cmd2 = new SqlCommand("SP_GetQuotesDetails_ForSaleOrder", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@compname", logIn.company);

                    cmd2.Parameters.AddWithValue("@Quotno", cmbQuotNo.Text);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    //DataSet ds2 = new DataSet();
                    DataTable ds2 = new DataTable();
                    // da2.Fill(ds2, "x");
                    da2.Fill(ds2);
                    dgProducts.DataSource = ds2;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CmbConsigneeName_Leave(object sender, EventArgs e)
        {
            if (CmbConsigneeName.Text != "")
            //if (Convert.ToInt32(CmbConsigneeName.SelectedValue) != 0)
            {
                var State = (from c in db.Supplier_informations
                             where c.Supplier_Name == CmbConsigneeName.Text && c.Company_ID == logIn.company
                             select new { c.Address_1, c.Address_2, c.City, c.State, c.GSTIN_NO, c.Pincode, c.StateCode }).ToList();
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
                    CAddr.City = State[0].City;
                    CAddr.State = State[0].State;
                    CAddr.StateCode = State[0].StateCode;
                    CAddr.GSTIN = State[0].GSTIN_NO;

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    string json = JsonConvert.SerializeObject(CAddr, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });





                    txtConAddress.Text = json;
                    //State[0].Address_1 + ", " + State[0].Address_2 + ", " + State[0].City + "," + State[0].Pincode + "," + State[0].State +","+State[0].StateCode;
                }
                else
                {
                    MessageBox.Show("Invalid Consignee Name Selected");
                    CmbConsigneeName.Focus();

                }
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if(txtStateCode.Text == string.Empty)
            {
                MessageBox.Show("State / State Code Cannot Be Blank");
                    return;
            }
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (txtConAddress.Text != "")
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

                JObject jsoncancel = JObject.Parse(txtConAddress.Text);
                txtAddress1.Text = (string)jsoncancel.SelectToken("Address1"); ;
                txtAddress2.Text = (string)jsoncancel.SelectToken("Address2");
                cmbCity.Text = (string)jsoncancel.SelectToken("City");
                txtstate.Text = (string)jsoncancel.SelectToken("State");
                txtPincode.Text = (string)jsoncancel.SelectToken("PinCode");
                txtStateCode.Text = (string)jsoncancel.SelectToken("StateCode");
                txtConGSTIN.Text = (string)jsoncancel.SelectToken("GSTIN");
                //txtConGSTNo.Text = State[0].GSTIN_NO;

                groupBox3.Visible = true;
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = false;
        }

        private void dgProducts_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

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
               else
                {
                    MessageBox.Show("Invalid City Selected");
                    return;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dgProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        public void AddEnqNos(AutoCompleteStringCollection coll)
        {
            try
            {

                var Prodname = (from d in db.SP_GetEnqForQuote(logIn.company)


                                select new { d.Enq_NO }).ToList();
                DataTable dt = new DataTable();
                dt.Columns.Add("Enq_NO");
                foreach (var item in Prodname)
                {
                    dt.Rows.Add(item.Enq_NO);
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    coll.Add(dt.Rows[i][0].ToString());
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        public void Save()
        {
            try
            {
                String myString = "";
                myString = txtSoNo.Text;
                if ((from u in db.Sale_Order_Masters where u.SO_NO == myString && u.Company_ID == logIn.company  select u).Count() > 0)
                {
                    myString = txtSoNo.Text;
                    db.sp_SO_Delete(myString, logIn.company, logIn.BU_ID);
                }
                else
                {
                    AutoincrementId();
                    myString = txtSoNo.Text;

                }
                //var S = db.Sale_Order_Masters.Where(w => w.SO_NO == myString && w.Company_ID == logIn.company).FirstOrDefault();
                Sale_Order_Master S = new Sale_Order_Master();
                {
                    S.SO_NO = myString;
                    S.SODate = dpSODate.Value;
                    S.QuotNo = cmbQuotNo.Text;
                   // S.EnqDate = dpQuotDate.Value;
                    S.BuyerName = Convert.ToInt32(CmbBuyerName.SelectedValue.ToString());
                    S.ConsigneeName = Convert.ToInt32(CmbConsigneeName.SelectedValue.ToString());
                    S.Status = Convert.ToInt32(cmbStatus.SelectedValue.ToString()); 
                    S.TotalQty = (txtTotalQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotalQty.Text);
                    S.SubTotal = (txtSubTotal.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtSubTotal.Text);
                    S.Tot_TaxableValue = (txtTot_TaxableValue.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTot_TaxableValue.Text);
                    S.CustomerPONo = (txtCustPoNo.Text == "") ? "" : txtCustPoNo.Text;
                    S.PODate = dpPODate.Value;
                    S.Export_Order = (chkExport.Checked == true) ? true : false;
                    S.Foreign_Currency = cmbcurrency.Text;
                    S.Exchange_Rate = (txtExchangeRate.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtExchangeRate.Text);
                    S.Delivery_Address = txtConAddress.Text;
                    S.Delivery_Location = (txtCity.Text == "") ? "" : txtCity.Text;

                    S.So_Amend_No = (txtAmendNo.Text == "") ? "" : txtAmendNo.Text;
                    S.So_Amend_Date = dtAmendDate.Value;
                    S.UT_Required = Convert.ToInt32(txtUT.Text);

                    S.LC_No = (txtLCNo.Text == "") ? "" : txtLCNo.Text;
                    S.LC_Value = (txtLCValue.Text == "") ? "" : txtLCValue.Text;
                    S.LC_Date = dpLCExpdate.Value;
                    S.LC_Last_Date = dpLCLastdate.Value;
                    S.Job_Work_Order = (chkJWORder.Checked == true) ? true : false;
                    //S.Old_Ord_Ref = (txt.Text == "") ? "" : txtOtherTerms.Text;
                    S.SEZ_Order = (chkSEZOrder.Checked == true) ? true : false;
                    // S.Pre_Ship = (chkPreShipment.Checked == true) ? true : false;
                    S.Frieght_Unit = (txtFreight_Rate.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtFreight_Rate.Text);
                    S.RM_Basic_Price = (txtRMBasic.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtRMBasic.Text);
                    S.Other_Charges = (textBox1.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(textBox1.Text);

                   
                    S.Price_Basis = Convert.ToInt32(txtPriceBasis.Text);
                    S.PaymentTerms = Convert.ToInt32(cmbPaymentTerms.SelectedValue.ToString());
                    if (cmbPaymentTerms.Text == "Custom")
                    {
                        S.Payment_Terms_Custom = txtPaymentTermsCustom.Text;
                    }
                    else
                    {
                        S.Payment_Terms_Custom = cmbPaymentTerms.Text;
                    }
                    S.Cutting = Convert.ToInt32(txtCutting.Text);
                    S.Packing = Convert.ToInt32(txtPacking.Text);
                    //S.Trasnport_Scope = Convert.ToInt32(cmbTransport_Scope.SelectedValue.ToString());
                    S.Delivery_Terms = (txtDeliveryTerms.Text == "") ? "" : txtDeliveryTerms.Text;
                   // S.Multi_Loc_Delivery = (chkMultiLocation.Checked == true) ? true : false;                            
                   // S.Repeat_Order = (chkRepeatOrder.Checked == true) ? true : false;
                    //S.Job_Work_Order = (chkJWORder.Checked == true) ? true : false;
                    S.Technical_Terms = (txtTechTerms.Text == "") ? "" : txtTechTerms.Text;
                    S.SEZ_Order = (chkSEZOrder.Checked == true) ? true : false;
                    S.TPI_Involved = (chkTPI.Checked == true) ? true : false;
                    S.IBR_Cert = (chkIBR.Checked == true) ? true : false;
                    S.Macro = (chkMacro.Checked == true) ? true : false;
                    S.Customer_Contact = (txtCustomeContact.Text == "") ? "" : txtCustomeContact.Text;
                    S.Spl_Instructions = (txtSplInstructions.Text == "") ? "" : txtSplInstructions.Text;
                    //S.Spl_Notes = (txtSplNotes.Text == "") ? "" : txtSplNotes.Text;
                    S.BU_ID = Convert.ToInt32(cmbOrderExecuteFrom.SelectedValue);

                    //S.is_amended = false;
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
                    var d1 = (from a in db.Sale_Order_Masters where a.SO_NO == myString && a.Company_ID == logIn.company  select new { a.Id }).ToList();
                    SC.So_Master_ID = d1[0].Id;
                    SC.SO_NO = myString;
                    SC.Prod_Code = Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value);
                    //SC.Product_Description = (dgProducts.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value).ToString();
                    
                     string Prod_Grade = (dgProducts.Rows[i].Cells["Item_Grade"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value).ToString();
                    SC.Prod_Grade = Prod_Grade;
                    if (dgProducts.Rows[i].Cells["Item_Grade"].Value != null )
                    {
                        var S1 = (from a in db.QA_Mtrl_Grade_Masters
                                  where a.Company_ID == logIn.company && a.Material_Grade == Prod_Grade
                                  select new { a.id }).ToList();

                        if (S1.Count > 0)
                        {
                            SC.Prod_Grade_Id = S1[0].id;
                        }
                        else
                        {
                            SC.Prod_Grade_Id = 3034;
                        }
                    }
                    else
                    {
                        SC.Prod_Grade_Id = 3034;
                    }
                    SC.Prod_Length = (dgProducts.Rows[i].Cells["Prod_Length"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Prod_Length"].Value).ToString();
                    SC.ReqLengthMinMtr = (dgProducts.Rows[i].Cells["ReqLengthMinMtr"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["ReqLengthMinMtr"].Value);
                    SC.ReqLengthMaxMtr = (dgProducts.Rows[i].Cells["ReqLengthMaxMtr"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["ReqLengthMaxMtr"].Value);
                    SC.ReqLengthMtr = (dgProducts.Rows[i].Cells["ReqLengthMaxMtr"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["ReqLengthMaxMtr"].Value);

                    SC.Uom = (dgProducts.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["uom"].Value.ToString();

                    double amt = Convert.ToDouble(dgProducts.Rows[i].Cells["Qty"].Value);
                    //decimal qty = decimal.Round(Convert.ToDecimal(amt),5);
                    SC.Qty = amt;
                    SC.No_Of_Pieces = (dgProducts.Rows[i].Cells["No_Of_Pieces"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["No_Of_Pieces"].Value);
                    SC.Price = (dgProducts.Rows[i].Cells["Basic_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Basic_Price"].Value);

                    SC.Amount = (dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value);
                    SC.Disc_Per = (dgProducts.Rows[i].Cells["Disc_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Per"].Value);
                    SC.Disc_Amount = (dgProducts.Rows[i].Cells["Disc_Amt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Amt"].Value);
                    SC.Taxable_Value = (dgProducts.Rows[i].Cells["Taxable_Value"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Taxable_Value"].Value);
                    SC.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value).ToString();
                    SC.Tole_Qty = (dgProducts.Rows[i].Cells["Prod_Tole_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Prod_Tole_Qty"].Value);
                    SC.Int_Prod_Code = (dgProducts.Rows[i].Cells["Int_Prod_Code"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Int_Prod_Code"].Value).ToString();
                    SC.HSN_Code = (dgProducts.Rows[i].Cells["HSN_Code"].Value == null) ? "" : (dgProducts.Rows[i].Cells["HSN_Code"].Value).ToString();
                    SC.Del_Date = (dgProducts.Rows[i].Cells["Delivery_Date"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Delivery_Date"].Value).ToString();

                    if (dgProducts.Rows[i].Cells["SO_Line_Item_No"].Value == DBNull.Value || dgProducts.Rows[i].Cells["SO_Line_Item_No"].Value == null || dgProducts.Rows[i].Cells["SO_Line_Item_No"].Value == "")
                    {
                        SC.enq_item_no = i + 1;
                    }
                    else
                    {
                        SC.enq_item_no = Convert.ToInt32(dgProducts.Rows[i].Cells["SO_Line_Item_No"].Value);
                    }

                    

                    if (cmbQuotNo.Text == "NA")
                    {
                        SC.Quot_Master_ID = 0;
                        SC.enq_Master_ID = 0;
                    //    SC.enq_item_no = 0;
                    }
                    else
                    {
                        var d2 = (from a in db.Sale_Quotation_Masters where a.Quot_NO == cmbQuotNo.Text && a.Company_ID == logIn.company && a.bu_id == logIn.BU_ID select new { a.Id }).ToList();
                        if (d2.Count > 0)
                        {
                            SC.Quot_Master_ID = d2[0].Id;
                            SC.enq_Master_ID = (dgProducts.Rows[i].Cells["Quote_Item_No"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["Quote_Item_No"].Value);

                            //SC.enq_Master_ID = Convert.ToInt32(dgProducts.Rows[i].Cells["Quote_Item_No"].Value);
                        }
                        else
                        {
                            SC.Quot_Master_ID = 0;
                            SC.enq_Master_ID = 0;
                        }
                    }

                    SC.Company_ID = logIn.company;
                    db.Sale_Order_Childs.InsertOnSubmit(SC);
                }
                db.SubmitChanges();
                //transaction.Commit();               
                MessageBox.Show("Record Saved / Updated Successfully With Transaction Ref No : " + txtSoNo.Text);
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

        public void bindedit()
        {
            try
            {
                txtSoNo.Text = OrderManagement.Transactions.ListOfOrders.SO_No;
                String myString = "";
                int OrdId = 0;
                myString = txtSoNo.Text;
                var da = (from obj in db.getOrder_Master_data_New(logIn.company, logIn.BU_ID, txtSoNo.Text)

                          select new
                          {
                              obj.Id,
                              obj.QuotNo,
                              obj.QuotDate,
                              obj.BuyerName,
                              obj.SO_NO,
                              obj.SODate,
                              obj.So_Amend_No,
                              obj.So_Amend_Date,
                              obj.Job_Work_Order,
                              obj.PODate,
                              obj.Frieght_Unit,
                              obj.SEZ_Order,
                              obj.TPI_Involved,
                              obj.IBR_Cert,
                              obj.UT_Required,
                             
                              obj.Tot_Discount,
                              obj.Tot_TaxableValue,
                              obj.TotalQty,
                              obj.SubTotal,
                              obj.CustomerPONo,                              
                              obj.Macro,
                              obj.Customer_Contact,
                              obj.Delivery_Terms,
                              obj.Other_Terms,
                              obj.Payment_Terms_Custom,
                              obj.Technical_Terms,
                              obj.Spl_Instructions,                            
                              obj.Sale_office,
                              obj.SaleExecutive,
                              obj.PaymentTerms,
                              obj.Price_Basis,
                              obj.ConsigneeName,                            
                              obj.Delivery_Date,
                              obj.Delivery_Address,
                              obj.Packing,
                              obj.Cutting,
                              obj.Status,
                              obj.Modified_By,
                              obj.Created_By,
                              obj.LC_Date,
                              obj.LC_Last_Date,
                              obj.LC_No,
                              obj.LC_Value,
                              obj.Pre_Ship,
                              obj.Sales_Desc,
                              obj.Payment_Desc,
                              obj.UT_Desc,
                              obj.Executive_Desc,
                              obj.Price_Desc,
                              obj.Packing_Desc,
                              obj.Cutting_Desc,
                              obj.Delivery_Location,
                              obj.Export_Order,
                              obj.Foreign_Currency,
                              obj.Exchange_Rate,
                              obj.RM_Basic_Price,
                              obj.bu_id
                             
                              
                          }).ToList();
                if (da.Count > 0)
                {
                    OrdId = da[0].Id;
                    txtSoNo.Text = da[0].SO_NO.ToString();
                    dpSODate.Text = da[0].SODate.Value.ToString();
                    //bindCustomer();
                    CmbBuyerName.SelectedValue = da[0].BuyerName;




                    CmbConsigneeName.SelectedValue = da[0].ConsigneeName;
                    txtConAddress.Text = da[0].Delivery_Address;
                    txtCity.Text = da[0].Delivery_Location;
                    txtDeliveryTerms.Text = da[0].Delivery_Terms;

                    txtAmendNo.Text = da[0].So_Amend_No;
                    dtAmendDate.Text = da[0].So_Amend_Date.ToString();
                    dpLCExpdate.Text = da[0].LC_Date.ToString();
                    dpLCLastdate.Text = da[0].LC_Last_Date.ToString();
                    txtLCNo.Text = da[0].LC_No;
                    txtLCValue.Text = da[0].LC_Value;

                    if (da[0].Export_Order == true)
                    {
                        chkExport.Checked = true;
                        cmbcurrency.Text = da[0].Foreign_Currency;
                        txtExchangeRate.Text = da[0].Exchange_Rate.ToString();

                    }
                    else
                    {
                        chkExport.Checked = false;
                        cmbcurrency.Text = "INR";
                        txtExchangeRate.Text = "1";

                    }


                    if (da[0].SEZ_Order == true)
                    {
                        chkSEZOrder.Checked = true;
                    }
                    else
                    {
                        chkSEZOrder.Checked = false;
                    }



                    if (da[0].Job_Work_Order == true)
                    {
                        chkJWORder.Checked = true;
                    }
                    else
                    {
                        chkJWORder.Checked = false;
                    }
                    if (da[0].TPI_Involved == true)
                    {
                        chkTPI.Checked = true;
                    }
                    else
                    {
                        chkTPI.Checked = false;
                    }
                    if (da[0].IBR_Cert == true)
                    {
                        chkIBR.Checked = true;
                    }
                    else
                    {
                        chkIBR.Checked = false;
                    }

                    if (da[0].Macro == true)
                    {
                        chkMacro.Checked = true;
                    }
                    else
                    {
                        chkMacro.Checked = false;
                    }


                    if(da[0].Packing_Desc != null)
                    {
                        txtPacking.Text = da[0].Packing.ToString();
                        cmbPacking.Text = da[0].Packing_Desc.ToString();
                    }
                    if (da[0].Cutting_Desc != null)
                    {
                        txtCutting.Text = da[0].Cutting.ToString();
                        cmbCutting.Text = da[0].Cutting_Desc.ToString();
                    }

                    if (da[0].UT_Desc != null)
                    {
                        txtUT.Text = da[0].UT_Required.ToString();
                        cmbUT.Text = da[0].UT_Desc.ToString();
                    }



                    if (da[0].SaleExecutive != null)
                    {
                        cmbSaleExecutive.SelectedValue = da[0].SaleExecutive;
                    }
                    if (da[0].Price_Desc != null)
                    {
                        txtPriceBasis.Text = da[0].Price_Basis.ToString();
                        cmbPriceBasis.Text = da[0].Price_Desc.ToString();
                    }
                    //if (da[0].Insurance_Scope == null)
                    //{
                    //    cmbInsurance.SelectedValue = 11;
                    //}
                    //else
                    //{
                    //    cmbInsurance.SelectedValue = da[0].Insurance_Scope; 
                    //}

                    if (da[0].PaymentTerms == null)
                    {
                        cmbPaymentTerms.SelectedValue = 35;
                    }
                    else
                    {
                        //cmbPaymentTerms.SelectedValue = da[0].PaymentTerms;
                        if (da[0].PaymentTerms != null)
                        {
                            cmbPaymentTerms.SelectedValue = da[0].PaymentTerms;
                            if (cmbPaymentTerms.Text == "Custom")
                            {
                                txtPaymentTermsCustom.Text = da[0].Payment_Terms_Custom;
                            }
                            else
                            {
                                txtPaymentTermsCustom.Text = "";
                            }
                        }
                        else
                        {
                            cmbPaymentTerms.SelectedValue = 87;
                        }

                    }





                    //cmbCustomer.Enabled = false;
                    cmbQuotNo.Text = da[0].QuotNo;
                    dpQuotDate.Text = da[0].QuotDate.ToString();

                    txtTotalQty.Text = da[0].TotalQty.ToString();
                    txtSubTotal.Text = da[0].SubTotal.ToString();
                    txtCustPoNo.Text = da[0].CustomerPONo;
                    dpPODate.Text = da[0].PODate.ToString();
                    //.Text = da[0].Delivery_Date.ToString();
                    txtFreight_Rate.Text = da[0].Frieght_Unit.ToString();
                    txtRMBasic.Text = da[0].RM_Basic_Price.ToString();
                    //  if (da[0].Price_Basis == null)
                    //{
                    //    cmbPriceBasis.SelectedValue = 7;
                    //}
                    //else
                    //{
                    //    cmbPriceBasis.SelectedValue = da[0].Price_Basis;
                    //}

                    //if (da[0].PaymentTerms == null)
                    //{
                    //    cmbPaymentTerms.SelectedValue = 35;
                    //}
                    //else
                    //{
                    //    cmbPaymentTerms.SelectedValue = da[0].PaymentTerms;
                    //    //if (Convert.ToInt32(cmbPaymentTerms.SelectedValue.ToString()) == 87)
                    //    //{

                    //    //
                    //    //else
                    //    //{
                    //    //  txtCustomerPaymentTerms.Text = "";
                    //    //}
                    //}
                    txtTechTerms.Text = da[0].Technical_Terms;
                    txtCustomeContact.Text = da[0].Customer_Contact;
                    txtConAddress.Text = da[0].Delivery_Address;
                    txtDeliveryTerms.Text = da[0].Delivery_Terms;
                    //txtOtherCharges.Text = da[0].Other_Terms;
                    txtSplInstructions.Text = da[0].Spl_Instructions;
                    cmbStatus.SelectedValue = da[0].Status;
                    lblCreatedBy.Text = da[0].Created_By;
                    lblModified.Text = da[0].Modified_By;
                    cmbOrderExecuteFrom.SelectedValue = da[0].bu_id;
                }

                var isNumeric = 0;
                int n = 0;
                var dm1 = (from s in db.Sale_Order_Childs
                           join p in db.Products on Convert.ToInt32(s.Prod_Code) equals p.prod_ID
                           join u in db.UoM_Masters on p.Prod_Primary_UOM_Id equals u.UOM_ID
                           join m in db.QA_Mtrl_Grade_Masters on s.Prod_Grade_Id equals m.id into ps
                           from m in ps.DefaultIfEmpty()
                           where s.So_Master_ID == OrdId && s.Company_ID == logIn.company
                           select new
                           {
                               Item_Code = s.Prod_Code,
                               SO_Line_Item_No = s.enq_item_no,
                               Int_Prod_Code = p.Prod_Code,
                               Item_Description = p.Prod_Name,
                               Item_Grade = m.Material_Grade,
                               Prod_Length = s.Prod_Length,
                               isNumeric = int.TryParse(s.Prod_Length, out n),
                               s.ReqLengthMinMtr,
                               s.ReqLengthMaxMtr,
                               HSN_Code = p.Prod_HSN_Code,
                               UOM = u.Uom_Descr,
                               s.Qty,
                               s.No_Of_Pieces,
                               Basic_Price = s.Price,
                               Amt_Before_Disc = s.Amount,
                               s.Taxable_Value,
                               Prod_Tole_Qty = s.Tole_Qty,
                               Delivery_date = s.Del_Date,
                               Quot_No = s.Quot_Master_ID,
                               Quote_Item_No = s.enq_Master_ID,
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
