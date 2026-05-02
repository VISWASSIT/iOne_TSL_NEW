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
using Syncfusion.WinForms.DataGrid.Interactivity;
using Ione_DAL;
namespace ioneNet.MaterialManagement.Transactions
{
    public partial class MaterialReceiptNote : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();
        public static string SONo,ItemCode,RecQty,Suppname;
        decimal cgstPer, sgstPer, igstPer;
        public MaterialReceiptNote()
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
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            
            bindCustomer();
            //bindConsignee();
            bindDroupDown_Lookup();

            if(logIn.company ==13)
            {
                //dgProducts.Columns["Basic_Price"].Visible = false;
                //dgProducts.Columns["Amt_Before_Disc"].Visible = false;
                //dgProducts.Columns["Disc_Per"].Visible = false;
                //dgProducts.Columns["Disc_Amt"].Visible = false;
                //dgProducts.Columns["Taxable_Value"].Visible = false;
                //dgProducts.Columns["CGST_Per"].Visible = false;
                //dgProducts.Columns["CGST_Amt"].Visible = false;
                //dgProducts.Columns["SGST_Per"].Visible = false;
                //dgProducts.Columns["SGST_Amt"].Visible = false;
                //dgProducts.Columns["IGST_Per"].Visible = false;
                //dgProducts.Columns["IGST_Amt"].Visible = false;
                //dgProducts.Columns["Total_Amount"].Visible = false;
                
            }

            if(MaterialReceiptNoteList.var=="0")
            {
                if (MaterialReceiptNoteList.editMode == true)
                {
                    bindedit();
                }
            }
            else
            {
            AutoincrementId();
            }
            
        }

       

       
       
        

        private void btnClose_Click(object sender, EventArgs e)
        {
            //Masters.ProdSearch.dtgetfinalprducts = null;
            
            
           // SqlCommand cmd2 = new SqlCommand("delete  from [Bloom_Roll_Wise_Receipts] where [Grn_ID] =@ProdID and status ='Open' and company_ID = @comp", con);

           //// cmd2.Parameters.AddWithValue("@AccID", AccID);
           // cmd2.Parameters.AddWithValue("@comp", logIn.company);
           // cmd2.Parameters.AddWithValue("@ProdID", txtSoNo.Text);
           // if (con.State != ConnectionState.Open)
           //     con.Open();
           // //con.Open();
           // cmd2.ExecuteNonQuery();
           
           // con.Close();
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {


            try
            {
                if (CmbSuplierName.Text == string.Empty)
                {
                    MessageBox.Show("Supplier Name Should Not Be Empty", "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CmbSuplierName.Focus();
                    return;
                }
                else if (txtSupplierInvNo.Text == string.Empty)
                {
                    MessageBox.Show("Supplier Inv no  Should Not Be Empty");
                    txtlrnodate.Focus();
                    return;
                }
                else if (cmbPurchaseBasis.Text == string.Empty)
                {
                    MessageBox.Show("Select Purchase Basis No,");
                    cmbPurchaseBasis.Focus();
                    return;
                }
                else if (cmbStatus.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Status");
                    cmbStatus.Focus();
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
                var Buyerblind = (from m in db.Supplier_informations where m.Company_ID==logIn.company && m.Supplier_Category !=27 select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    CmbSuplierName.DataSource = Buyerblind;
                    CmbSuplierName.ValueMember = "ID";
                    CmbSuplierName.DisplayMember = "Supplier_Name";

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
                var Buyerblind = (from m in db.Customer_informations select new { m.ID, m.Customer_Alias_Name }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    //CmbBuyerName.DataSource = Buyerblind;
                    //CmbBuyerName.ValueMember = "ID";
                    //CmbBuyerName.DisplayMember = "Customer_Alias_Name";


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
               

                
               
                //Warehouse
                var SO = (from m in db.Attributes_Datas where m.Head_Name == "Sales Office" select new { m.ID, m.Descr }).Distinct().ToList();
                if (SO.Count > 0)
                {
                    cmbWareHouse.DataSource = SO;
                    cmbWareHouse.ValueMember = "ID";
                    cmbWareHouse.DisplayMember = "Descr";
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
                //Purchase Account
                var d = (from po in db.AccountMasters
                         join A in db.AccountGroups on po.AccGroup_ID equals A.ID
                         where po.Company_ID == logIn.company //&& A.GroupType == "Expenses"
                         select new { po.id, po.AccName }).Distinct().ToList();
                if (d.Count > 0)
                {
                    cmbPurchaseAccount.DataSource = d;
                    cmbPurchaseAccount.ValueMember = "id";
                    cmbPurchaseAccount.DisplayMember = "AccName";
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

                var result = db.Sp_autoincrement_MRN(logIn.company,logIn.fy_Start_Date, logIn.fy_End_Date);
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
                if (tb3 != null && columnName == "Item Description")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }

                if (tb3 != null && columnName == "Make /Model / Grade")
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
                        var Prodname = (from d in db.Products where d.Company_ID == logIn.company && d.Prod_Status_ID == 1 select new { d.Prod_Name }).ToList();
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
                    else
                    {
                        if (columnName == "Make /Model / Grade")
                        {
                            var Prodname = (from d in db.GoodsReceiptNote_Childs where d.Company_ID == logIn.company select new { d.Prod_Grade }).Distinct().ToList();
                            DataTable dt = new DataTable();
                            dt.Columns.Add("Prod_Grade");
                            foreach (var item in Prodname)
                            {
                                dt.Rows.Add(item.Prod_Grade);
                            }
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                coll.Add(dt.Rows[i][0].ToString());
                            }
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
            try
            {
                //var gstno= (from c in db.Supplier_informations
                //             where c.Supplier_Name == CmbSuplierName.Text
                //             select new { c.GSTIN_NO,c.StateCode}).ToList();
                //if (gstno.Count > 0)
                //{
                //    txtSupGSTNo.Text = gstno[0].GSTIN_NO;                   
                //    txtStateCode.Text = gstno[0].StateCode;
                //}                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        string prodcode, ponumber, podate;
        private void dgProducts_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //DataTable dtexisting = new DataTable();
                if (e.KeyCode == Keys.F3)
                {
                    int i = dgProducts.CurrentCell.RowIndex;
                    ItemCode = (dgProducts.Rows[i].Cells["Item_Code"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Code"].Value).ToString();
                    RecQty = (dgProducts.Rows[i].Cells["Item_Spec"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Spec"].Value).ToString();

                    //ItemCode = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                    //if(dgProducts.Rows[i].Cells["Item_Spec"].Value.ToString() != null)
                    //{
                    //    RecQty = dgProducts.Rows[i].Cells["Item_Spec"].Value.ToString();

                    //}
                    //else
                    //{
                    //    RecQty = "";
                    //}
                    //RecQty = (dgProducts.Rows[i].Cells["Item_Spec"].Value.ToString() == null) ? "" : dgProducts.Rows[i].Cells["Item_Spec"].Value.ToString();


                    if (ItemCode != "" && RecQty != "")
                    {
                        ioneNet.OrderManagement.Transactions.ProdSpecs form = new ioneNet.OrderManagement.Transactions.ProdSpecs();
                        //ioneNet.Masters.ProdSearch.frmName = "SOrder";       
                       
                        SONo = txtSoNo.Text;


                        form.ShowDialog();
                        dgProducts.Rows[i].Cells["ReceivedQty"].Value = ioneNet.OrderManagement.Transactions.ProdSpecs.TotQty;
                    }
                    else
                    {
                        MessageBox.Show("Cannot Proceed Without Item Code and No of Rolls Received");
                    }
                }
                //    ioneNet.Masters.ProdSearch form = new ioneNet.Masters.ProdSearch();
                //    ioneNet.Masters.ProdSearch.frmName = "GRN";
                //    form.ShowDialog();

                //    if (dgProducts.Rows.Count > 1)
                //    {
                //        dtexisting.Rows.Clear();
                //        dtexisting.Columns.Clear();
                //        dtexisting.Columns.Add("Item_Code", typeof);
                //        dtexisting.Columns.Add("Item_Description", typeof);
                //        dtexisting.Columns.Add("Item_Grade", typeof);
                //        dtexisting.Columns.Add("UOM", typeof);
                //        dtexisting.Columns.Add("Qty", typeof(decimal));
                //        dtexisting.Columns.Add("Basic_Price", typeof(decimal));
                //        dtexisting.Columns.Add("Amt_Before_Disc", typeof(decimal));
                //        dtexisting.Columns.Add("Disc_Per", typeof(decimal));
                //        dtexisting.Columns.Add("Disc_Amt", typeof(decimal));
                //        dtexisting.Columns.Add("Taxable_Value", typeof(decimal));
                //        dtexisting.Columns.Add("CGST_Per", typeof(decimal));
                //        dtexisting.Columns.Add("CGST_Amt", typeof(decimal));
                //        dtexisting.Columns.Add("SGST_Per", typeof(decimal));
                //        dtexisting.Columns.Add("SGST_Amt", typeof(decimal));
                //        dtexisting.Columns.Add("IGST_Per", typeof(decimal));
                //        dtexisting.Columns.Add("IGST_Amt", typeof(decimal));
                //        dtexisting.Columns.Add("Total_Amount", typeof(decimal));
                //        dtexisting.Columns.Add("Remarks", typeof);

                //        for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                //        {
                //            DataRow dr;
                //            dr = dtexisting.NewRow();
                //            dr["Item_Code"] = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                //            dr["Item_Description"] = dgProducts.Rows[i].Cells["Item_Description"].Value.ToString();
                //            dr["Item_Grade"] = dgProducts.Rows[i].Cells["Item_Grade"].Value.ToString();
                //            dr["UOM"] = dgProducts.Rows[i].Cells["UOM"].Value.ToString();
                //            dr["Qty"] = dgProducts.Rows[i].Cells["Qty"].Value.ToString();
                //            dr["Basic_Price"] = dgProducts.Rows[i].Cells["Basic_Price"].Value.ToString();
                //            dr["Amt_Before_Disc"] = dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value.ToString();
                //            dr["Disc_Per"] = dgProducts.Rows[i].Cells["Disc_Per"].Value.ToString();
                //            dr["Disc_Amt"] = dgProducts.Rows[i].Cells["Disc_Amt"].Value.ToString();
                //            dr["Taxable_Value"] = dgProducts.Rows[i].Cells["Taxable_Value"].Value.ToString();
                //            dr["CGST_Per"] = dgProducts.Rows[i].Cells["CGST_Per"].Value.ToString();
                //            dr["CGST_Amt"] = dgProducts.Rows[i].Cells["CGST_Amt"].Value.ToString();
                //            dr["SGST_Per"] = dgProducts.Rows[i].Cells["SGST_Per"].Value.ToString();
                //            dr["SGST_Amt"] = dgProducts.Rows[i].Cells["SGST_Amt"].Value.ToString();
                //            dr["IGST_Per"] = dgProducts.Rows[i].Cells["IGST_Per"].Value.ToString();
                //            dr["IGST_Amt"] = dgProducts.Rows[i].Cells["IGST_Amt"].Value.ToString();
                //            dr["Total_Amount"] = dgProducts.Rows[i].Cells["Total_Amount"].Value.ToString();
                //            dr["Remarks"] = dgProducts.Rows[i].Cells["Remarks"].Value.ToString();
                //            dtexisting.Rows.Add(dr);

                //        }
                //        dtexisting.AcceptChanges();
                //    }


                //    if (ioneNet.Masters.ProdSearch.dtgetfinalprducts.Rows.Count > 0)
                //    {
                //        DataTable dt = new DataTable();
                //        dt.Columns.Add("Item_Code", typeof);
                //        dt.Columns.Add("Item_Description", typeof);
                //        dt.Columns.Add("Item_Grade", typeof);
                //        dt.Columns.Add("UOM", typeof);
                //        dt.Columns.Add("Qty", typeof(decimal));
                //        dt.Columns.Add("Basic_Price", typeof(decimal));
                //        dt.Columns.Add("Amt_Before_Disc", typeof(decimal));
                //        dt.Columns.Add("Disc_Per", typeof(decimal));
                //        dt.Columns.Add("Disc_Amt", typeof(decimal));
                //        dt.Columns.Add("Taxable_Value", typeof(decimal));
                //        dt.Columns.Add("CGST_Per", typeof(decimal));
                //        dt.Columns.Add("CGST_Amt", typeof(decimal));
                //        dt.Columns.Add("SGST_Per", typeof(decimal));
                //        dt.Columns.Add("SGST_Amt", typeof(decimal));
                //        dt.Columns.Add("IGST_Per", typeof(decimal));
                //        dt.Columns.Add("IGST_Amt", typeof(decimal));
                //        dt.Columns.Add("Total_Amount", typeof(decimal));
                //        dt.Columns.Add("Remarks", typeof);

                //        //dt.Rows.Add();
                //        for (int i = 0; i < ioneNet.Masters.ProdSearch.dtgetfinalprducts.Rows.Count; i++)
                //        {
                //            string prodcode = ioneNet.Masters.ProdSearch.dtgetfinalprducts.Rows[i]["Prod_Code"].ToString();
                //            var getproducts = (from obj in db.Products
                //                               join uom in db.UoM_Masters on obj.Prod_Primary_UOM_Id equals uom.UOM_ID
                //                               where obj.Prod_Code == prodcode
                //                               select new
                //                               {
                //                                   Item_Code = obj.Prod_Code,
                //                                   Item_Description = obj.Prod_Name,
                //                                   Item_Grade = "",
                //                                   UOM = uom.Uom_Descr,
                //                                   Qty = 0,
                //                                   Basic_Price = 0,
                //                                   Amt_Before_Disc = 0,
                //                                   Disc_Per = 0,
                //                                   Disc_Amt = 0,
                //                                   Taxable_Value = 0,
                //                                   CGST_Per = 0,
                //                                   CGST_Amt = 0,
                //                                   SGST_Per = 0,
                //                                   SGST_Amt = 0,
                //                                   IGST_Per = 0,
                //                                   IGST_Amt = 0,
                //                                   Total_Amount = 0,
                //                                   Remarks = ""
                //                               }).ToList();
                //            dt.Rows.Add(getproducts[0].Item_Code, getproducts[0].Item_Description, getproducts[0].Item_Grade, getproducts[0].UOM, getproducts[0].Qty, getproducts[0].Basic_Price, getproducts[0].Amt_Before_Disc, getproducts[0].Disc_Per, getproducts[0].Disc_Amt, getproducts[0].Taxable_Value, getproducts[0].CGST_Per, getproducts[0].CGST_Amt, getproducts[0].SGST_Per, getproducts[0].SGST_Amt, getproducts[0].IGST_Per, getproducts[0].IGST_Amt, getproducts[0].Total_Amount, getproducts[0].Remarks);

                //        }

                //        dtexisting = dtexisting.AsEnumerable().Union(dt.AsEnumerable()).CopyToDataTable();
                //        dgProducts.DataSource = dtexisting;
                //    }
                //}
                //if (e.KeyCode == Keys.F4) //Delivery Information / Locations
                //{
                //    if (chkMultiLocation.Checked == true)
                //    {
                //        ioneNet.OrderManagement.Transactions.SO_MultiDeliveryAddress form = new OrderManagement.Transactions.SO_MultiDeliveryAddress();
                //        //ioneNet.Masters.ProdSearch.frmName = "SOrder";
                //        int i = dgProducts.CurrentCell.RowIndex;
                //        SONo = txtSoNo.Text;
                //        ItemCode = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                //        OrdQty = dgProducts.Rows[i].Cells["Qty"].Value.ToString();
                //        form.ShowDialog();
                //    }
                //    else
                //    {
                //        MessageBox.Show("Check Multi Location Delivery? To Add The Details");

                //    }
                //}
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void txttcsAmnt_Leave(object sender, EventArgs e)
        {
            try
            {
                //if (txttcsAmnt.Text != "" && txttcsAmnt.Text != "0.00")
                //{
                //    decimal per = Convert.ToDecimal(txttcsper.Text);
                //    decimal amnt = Convert.ToDecimal(txttcsAmnt.Text);
                //    decimal percentamnt = (amnt * per) / 100;
                //    //decimal totalorder = Convert.ToInt32(txtTot_OrderValue.Text);
                //    decimal totaltaxable = Convert.ToDecimal(txtTot_TaxableValue.Text);
                //    decimal cgst = Convert.ToDecimal(txtTot_CGST.Text);
                //    decimal sgst = Convert.ToDecimal(txtTot_SGST.Text);
                //    decimal igst = Convert.ToDecimal(txtTot_IGST.Text);
                //    decimal freight = Convert.ToDecimal(txtfreight.Text);
                //    decimal othercharges = Convert.ToDecimal(txtothercharges.Text);


                //    txtTot_OrderValue.Text = Convert.ToString(amnt + percentamnt + totaltaxable + cgst + sgst + igst + freight + othercharges);
                othercalculation();

                //}
            }
            catch (Exception ex)
            {


            }
        }

        private void txttcsper_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txttcsAmnt.Text != "" && txttcsAmnt.Text != "0.00")
                {

                    decimal per = Convert.ToDecimal(txttcsper.Text);
                    decimal amnt = Convert.ToDecimal(txttcsAmnt.Text);
                    decimal percentamnt = (amnt * per) / 100;
                    //decimalnt totalorder = Convert.ToInt32(txtTot_OrderValue.Text);
                    decimal totaltaxable = Convert.ToDecimal(txtTot_TaxableValue.Text);
                    decimal cgst = Convert.ToDecimal(txtTot_CGST.Text);
                    decimal sgst = Convert.ToDecimal(txtTot_SGST.Text);
                    decimal igst = Convert.ToDecimal(txtTot_IGST.Text);
                    decimal freight = Convert.ToDecimal(txtfreight.Text);
                    decimal othercharges = Convert.ToDecimal(txtothercharges.Text);


                    txtTot_OrderValue.Text = Convert.ToString(amnt + percentamnt + totaltaxable + cgst + sgst + igst + freight + othercharges);
                }

            }
            catch (Exception ex)
            {


            }
        }

        decimal igstamnt, otherper;
        public void othercalculation()
        {
            //decimal Finalother, othercharges, otherchargesvalue, totalotherchargesvalue,   otheramnt=0;
            //decimal Freight, pervalue, Freightmnt, Finalfrght=0;
            //decimal igstper, igstcharges, TotIGST,  Finaligst, igstfreightamnt, igstotheramnt=0;
            try
            {

                //    decimal j = 0; decimal CGST_Amt = 0; decimal IGST_Amt = 0; decimal SGST_Amt = 0;
                //    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                //    {
                //        // CGST_Per
                //        decimal CGST_Per = Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Per"].Value.ToString());
                //        decimal IGST_Per = Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Per"].Value.ToString());
                //        CGST_Amt += Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Amt"].Value.ToString());
                //        SGST_Amt += Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Amt"].Value.ToString());
                //        IGST_Amt += Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Amt"].Value.ToString());
                //        if (CGST_Per > 0)
                //        {
                //            if (j < CGST_Per + CGST_Per)
                //            {
                //                j = Convert.ToInt32(CGST_Per + CGST_Per);

                //            }
                //        }
                //        else
                //        {
                //            if (j < IGST_Per)
                //            {
                //                j = Convert.ToInt32(IGST_Per);

                //            }
                //        }
                //    }


                //    if (txtfreight.Text == null || txtfreight.Text == "" || txtfreight.Text == "0.00")
                //    {
                //        Freight = 0;
                //        Freightmnt = 0;
                //        pervalue = 0;
                //        Finalfrght = 0;
                //    }
                //    else
                //    {
                //        Freight = Convert.ToDecimal(txtfreight.Text);
                //        pervalue = Convert.ToInt32(j);
                //        Freightmnt = Math.Round((Freight * pervalue) / 100);
                //        Finalfrght = Freightmnt / 2;
                //    }


                //    if (txtothercharges.Text == null || txtothercharges.Text == "" || txtothercharges.Text == "0.00")
                //    {
                //        othercharges = 0;
                //        otherchargesvalue = 0;
                //        totalotherchargesvalue = 0;
                //    }
                //    else
                //    {
                //        othercharges = Convert.ToDecimal(txtothercharges.Text);
                //        otherper = Convert.ToInt32(j);
                //        otheramnt = Math.Round((othercharges * otherper) / 100);

                //        Finalother = otheramnt / 2;
                //        otherchargesvalue = otheramnt;
                //        totalotherchargesvalue = othercharges + otherchargesvalue;
                //    }


                //    if (txtTot_IGST.Text == null || txtTot_IGST.Text == "" || txtTot_IGST.Text == "0.00")
                //    {
                //        igstcharges = 0;
                //        TotIGST = 0;
                //        igstfreightamnt = 0;
                //        igstotheramnt = 0;
                //    }
                //    else
                //    {
                //        igstcharges = Convert.ToDecimal(txtTot_IGST.Text);
                //        igstper = Convert.ToInt32(j);
                //        igstfreightamnt = Math.Round((igstcharges * igstper) / 100);
                //        igstotheramnt = Math.Round((igstcharges * igstper) / 100);



                //    }

                //    decimal FinalCGSTAMount = 0; decimal FinalSGSTAMount = 0; decimal FinalIGSTAMount = 0;

                //    if (CGST_Amt > 0)
                //    {
                //        txtTot_CGST.Text = Convert.ToString(Finalfrght + CGST_Amt);
                //        txtTot_SGST.Text = Convert.ToString(Finalfrght + SGST_Amt);
                //        FinalCGSTAMount = Finalfrght + CGST_Amt;
                //        FinalSGSTAMount = Finalfrght + SGST_Amt;


                //    }
                //    if (txtTot_IGST.Text != "0.00" )
                //    {
                //        txtTot_IGST.Text = Convert.ToString(Finalfrght + IGST_Amt);

                //        FinalIGSTAMount = Finalfrght + IGST_Amt;
                //        //igstper = Convert.ToInt32(j);
                //        //igstamnt = 0;


                //    }


                //    decimal TotalOrderValue = Convert.ToDecimal(txtTot_TaxableValue.Text)+ FinalCGSTAMount + FinalSGSTAMount + FinalIGSTAMount + Freight + othercharges + +pervalue + otherper + igstamnt;
                //    txtTot_OrderValue.Text = Convert.ToString(Math.Round(TotalOrderValue));
                decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
                for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                {

                    x += (dgProducts.Rows[i].Cells["AcceptedQty"].Value == "" || dgProducts.Rows[i].Cells["AcceptedQty"].Value == null || dgProducts.Rows[i].Cells["AcceptedQty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["AcceptedQty"].Value);
                    y += (dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == "" || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == null || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value);
                    q += (dgProducts.Rows[i].Cells["Disc_Amt"].Value == "" || dgProducts.Rows[i].Cells["Disc_Amt"].Value == null || dgProducts.Rows[i].Cells["Disc_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Amt"].Value);
                    v += (dgProducts.Rows[i].Cells["Taxable_Value"].Value == "" || dgProducts.Rows[i].Cells["Taxable_Value"].Value == null || dgProducts.Rows[i].Cells["Taxable_Value"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Taxable_Value"].Value);
                    cg += (dgProducts.Rows[i].Cells["CGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["CGST_Amt"].Value == null || dgProducts.Rows[i].Cells["CGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Amt"].Value);
                    sg += (dgProducts.Rows[i].Cells["SGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["SGST_Amt"].Value == null || dgProducts.Rows[i].Cells["SGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Amt"].Value);
                    ig += (dgProducts.Rows[i].Cells["IGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["IGST_Amt"].Value == null || dgProducts.Rows[i].Cells["IGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Amt"].Value);
                    totA += (dgProducts.Rows[i].Cells["Total_Amount"].Value == "" || dgProducts.Rows[i].Cells["Total_Amount"].Value == null || dgProducts.Rows[i].Cells["Total_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Total_Amount"].Value);

                }

                txtTotalQty.Text = x.ToString("0.00");

                txtSubTotal.Text = y.ToString("0.00");
                txtTotDiscount.Text = q.ToString("0.00");
                decimal fAmt = (txtfreight.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtfreight.Text);
                decimal OthAmt = (txtothercharges.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtothercharges.Text);
               txtTot_TaxableValue.Text = (v + fAmt + OthAmt).ToString(".00");
                decimal taxvalue = Convert.ToDecimal(txtTot_TaxableValue.Text);
                decimal cgst = (taxvalue * cgstPer) / 100;
                decimal sgst = (taxvalue * sgstPer) / 100;
                decimal igst = (taxvalue * igstPer) / 100;

                txtTot_CGST.Text = cgst.ToString(".00");
                txtTot_SGST.Text = sgst.ToString(".00");
                txtTot_IGST.Text = igst.ToString(".00");
                //txtTot_TaxableValue.Text = v.ToString("0.00");
                //txtTot_CGST.Text = cg.ToString("0.00");
                //txtTot_SGST.Text = sg.ToString("0.00");
                //txtTot_IGST.Text = ig.ToString("0.00");
                //decimal fAmt = (txtFrieght.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtFrieght.Text);
                //decimal AmtForTCs = (v);
                decimal AmtForTCs = (taxvalue +cgst + sgst + igst);
                decimal tcsPer = (txttcsper.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txttcsper.Text);

                decimal TcsAmt = AmtForTCs * tcsPer / 100;
                txttcsAmnt.Text = TcsAmt.ToString(".00");
                decimal rndAmt = (textBox1.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(textBox1.Text);
                txtTot_OrderValue.Text = (taxvalue + cgst + sgst + igst + TcsAmt + rndAmt).ToString(".00");


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }     
        
        
               
        private void txtfreight_Leave(object sender, EventArgs e)
        {
            othercalculation();

        }

        System.Data.DataRow drgetproducts;
        DataTable dtexisting = new DataTable();
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                //Check Whether Exisitng Products Already Selected in Main Grid
                if (dgProducts.Rows.Count > 1)
                {
                    dtexisting.Rows.Clear();
                    dtexisting.Columns.Clear();
                    dtexisting.Columns.Add("Item_Code", typeof(string));
                    dtexisting.Columns.Add("Item_Description", typeof(string));
                    dtexisting.Columns.Add("Item_Spec", typeof(string));
                    dtexisting.Columns.Add("Item_Grade", typeof(string));
                    dtexisting.Columns.Add("HSN_Code", typeof(string));
                    dtexisting.Columns.Add("UOM", typeof(string));
                    dtexisting.Columns.Add("PO_Qty", typeof(decimal));
                    dtexisting.Columns.Add("ReceivedQty", typeof(decimal));
                    dtexisting.Columns.Add("RejectedQty", typeof(decimal));
                    dtexisting.Columns.Add("AcceptedQty", typeof(decimal));
                    dtexisting.Columns.Add("Basic_Price", typeof(decimal));
                    dtexisting.Columns.Add("Amt_Before_Disc", typeof(decimal));
                    dtexisting.Columns.Add("Disc_Per", typeof(decimal));
                    dtexisting.Columns.Add("Disc_Amt", typeof(decimal));
                    dtexisting.Columns.Add("Taxable_Value", typeof(decimal));
                    dtexisting.Columns.Add("CGST_Per", typeof(decimal));
                    dtexisting.Columns.Add("CGST_Amt", typeof(decimal));
                    dtexisting.Columns.Add("SGST_Per", typeof(decimal));
                    dtexisting.Columns.Add("SGST_Amt", typeof(decimal));
                    dtexisting.Columns.Add("IGST_Per", typeof(decimal));
                    dtexisting.Columns.Add("IGST_Amt", typeof(decimal));
                    dtexisting.Columns.Add("Total_Amount", typeof(decimal));
                    dtexisting.Columns.Add("PO_No", typeof(string));
                    dtexisting.Columns.Add("PR_No", typeof(string));
                    dtexisting.Columns.Add("Heat_No", typeof(string));
                    dtexisting.Columns.Add("TCNo", typeof(string));
                    dtexisting.Columns.Add("Remarks", typeof(string));
                    //
                    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                    {
                        DataRow dr;
                        dr = dtexisting.NewRow();
                        dr["Item_Code"] = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                        dr["Item_Description"] = dgProducts.Rows[i].Cells["Item_Description"].Value.ToString();
                        dr["Item_Grade"] = dgProducts.Rows[i].Cells["Item_Grade"].Value.ToString();
                        dr["HSN_Code"] = dgProducts.Rows[i].Cells["HSN_Code"].Value.ToString();
                        dr["UOM"] = dgProducts.Rows[i].Cells["UOM"].Value.ToString();
                        dr["PO_Qty"] = dgProducts.Rows[i].Cells["PO_Qty"].Value.ToString();
                        dr["ReceivedQty"] = dgProducts.Rows[i].Cells["ReceivedQty"].Value.ToString();
                        dr["RejectedQty"] = dgProducts.Rows[i].Cells["RejectedQty"].Value.ToString();
                        dr["AcceptedQty"] = dgProducts.Rows[i].Cells["AcceptedQty"].Value.ToString();
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
                        dr["PO_No"] = dgProducts.Rows[i].Cells["PO_No"].Value.ToString();
                        dr["PR_No"] = dgProducts.Rows[i].Cells["PR_No"].Value.ToString();
                        dr["Heat_No"] = dgProducts.Rows[i].Cells["Heat_No"].Value.ToString();
                        dr["TCNo"] = dgProducts.Rows[i].Cells["TCNo"].Value.ToString();
                        dr["Remarks"] = dgProducts.Rows[i].Cells["Remarks"].Value.ToString();                        
                        dtexisting.Rows.Add(dr);

                    }
                    dtexisting.AcceptChanges();
                }



                dtgetproducts.Columns.Clear();
                dtgetproducts.Rows.Clear();
                dtgetproducts.Columns.Add("Item_Code", typeof(string));
                dtgetproducts.Columns.Add("Item_Description", typeof(string));
                dtgetproducts.Columns.Add("Item_Spec", typeof(string));
                dtgetproducts.Columns.Add("Item_Grade", typeof(string));
                dtgetproducts.Columns.Add("HSN_Code", typeof(string));
                dtgetproducts.Columns.Add("UOM", typeof(string));
                dtgetproducts.Columns.Add("PO_Qty", typeof(decimal));
                dtgetproducts.Columns.Add("ReceivedQty", typeof(decimal));
                dtgetproducts.Columns.Add("RejectedQty", typeof(decimal));
                dtgetproducts.Columns.Add("AcceptedQty", typeof(decimal));
                dtgetproducts.Columns.Add("Basic_Price", typeof(decimal));
                dtgetproducts.Columns.Add("Amt_Before_Disc", typeof(decimal));
                dtgetproducts.Columns.Add("Disc_Per", typeof(decimal));
                dtgetproducts.Columns.Add("Disc_Amt", typeof(decimal));
                dtgetproducts.Columns.Add("Taxable_Value", typeof(decimal));
                dtgetproducts.Columns.Add("CGST_Per", typeof(decimal));
                dtgetproducts.Columns.Add("CGST_Amt", typeof(decimal));
                dtgetproducts.Columns.Add("SGST_Per", typeof(decimal));
                dtgetproducts.Columns.Add("SGST_Amt", typeof(decimal));
                dtgetproducts.Columns.Add("IGST_Per", typeof(decimal));
                dtgetproducts.Columns.Add("IGST_Amt", typeof(decimal));
                dtgetproducts.Columns.Add("Total_Amount", typeof(decimal));
                dtgetproducts.Columns.Add("PO_No", typeof(string));
                dtgetproducts.Columns.Add("PR_No", typeof(string));
                dtgetproducts.Columns.Add("Heat_No", typeof(string));
                dtgetproducts.Columns.Add("TCNo", typeof(string));
                dtgetproducts.Columns.Add("Remarks", typeof(string));

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
                            var SO_Ref_No = (rowData.GetType().GetProperty("PO_No").GetValue(rowData, null).ToString());
                            var getHSN = (from s in db.Products     
                                                  where s.Prod_Name == Item_Description.ToString()
                                                  select new { s.Prod_HSN_Code }).FirstOrDefault();

                            //if (getHSN != null)

                            drgetproducts = dtgetproducts.NewRow();
                            drgetproducts["Item_Code"] = Item_Code.ToString();
                            drgetproducts["Item_Description"] = Item_Description.ToString();
                            drgetproducts["Item_Spec"] = "";
                            drgetproducts["Item_Grade"] = "";
                            drgetproducts["HSN_Code"] = getHSN.Prod_HSN_Code;
                            drgetproducts["UOM"] = UOM.ToString();
                            drgetproducts["PO_Qty"] = PO_Qty.ToString();
                            drgetproducts["ReceivedQty"] = 0;
                            drgetproducts["RejectedQty"] = 0;
                            drgetproducts["AcceptedQty"] = 0;
                            drgetproducts["Basic_Price"] = Basic_Price.ToString();
                            drgetproducts["Disc_Per"] = 0;
                            drgetproducts["Disc_Amt"] = 0;
                            drgetproducts["Taxable_Value"] = 0;
                            drgetproducts["CGST_Per"] = 0;
                            drgetproducts["CGST_Amt"] = 0;
                            drgetproducts["SGST_Per"] = 0;
                            drgetproducts["SGST_Amt"] = 0;
                            drgetproducts["IGST_Per"] = 0;
                            drgetproducts["IGST_Amt"] = 0;
                            drgetproducts["Total_Amount"] = 0;
                            drgetproducts["PO_No"] = SO_Ref_No.ToString();
                            drgetproducts["PR_No"] = "";
                            drgetproducts["Heat_No"] = "";
                            drgetproducts["TCNo"] = "";
                            drgetproducts["Remarks"] = "";
                           
                            dtgetproducts.Rows.Add(drgetproducts);
                            dtgetfinalprducts = dtgetfinalprducts.AsEnumerable().Union(dtgetproducts.AsEnumerable()).CopyToDataTable();
                            //}
                            //}
                            dtgetproducts.Rows.Clear();
                        }
                    }
                }
                dtexisting = dtexisting.AsEnumerable().Union(dtgetfinalprducts.AsEnumerable()).CopyToDataTable();
                dgProducts.DataSource = dtexisting;
                //txtCustPoNo.Text = "Multi";
                //txtSoNo.Text = "Multi";

                //dgProducts.DataSource = dtgetfinalprducts;
                groupBox2.Visible = false;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sfDataGrid1_Click(object sender, EventArgs e)
        {

        }

        private void dgProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
        }

        private void CmbSuplierName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var gstno = (from c in db.Supplier_informations
                             where c.Supplier_Name == CmbSuplierName.Text
                             select new { c.GSTIN_NO, c.StateCode }).ToList();
                if (gstno.Count > 0)
                {
                    txtSupGSTNo.Text = gstno[0].GSTIN_NO;
                    txtStateCode.Text = gstno[0].StateCode;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void txtvehicalnr_TextChanged(object sender, EventArgs e)
        {

        }

        private void GoodsReceiptNote_FormClosed(object sender, FormClosedEventArgs e)
        {
            SqlCommand cmd2 = new SqlCommand("delete  from [Bloom_Roll_Wise_Receipts] where [Grn_ID] =@ProdID and status ='Open' and company_ID = @comp", con);

            // cmd2.Parameters.AddWithValue("@AccID", AccID);
            cmd2.Parameters.AddWithValue("@comp", logIn.company);
            cmd2.Parameters.AddWithValue("@ProdID", txtSoNo.Text);
            if (con.State != ConnectionState.Open)
                con.Open();
            //con.Open();
            cmd2.ExecuteNonQuery();

            con.Close();
        }

        private void txtothercharges_Leave(object sender, EventArgs e)
        {
            othercalculation();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            try
            {
                var d = (from data in db.SP_GetOrders_Sel(logIn.company, CmbSuplierName.Text,1,"") select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                }

                groupBox2.Visible = true;
                txtSearch.Focus();
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
                if (cmbPurchaseBasis.Text == "Direct")
                {
                    if (columnName == "Item_Description")
                    {
                        if (R1.Cells["Item_Description"].Value != null)
                        {

                            var getProductName = (from s in db.Products
                                                  join u in db.UoM_Masters on s.Prod_Primary_UOM_Id equals u.UOM_ID
                                                  join g in db.Product_Groups on s.Prod_Group_Id equals g.ID
                                                  where s.Prod_Name == R1.Cells["Item_Description"].Value.ToString() && s.Company_ID == logIn.company
                                                  select new { s.prod_ID, u.Uom_Descr, g.Prod_Group_Name,s.Prod_HSN_Code }).FirstOrDefault();

                            if (getProductName != null)
                            {
                                R1.Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                                R1.Cells["Item_code"].Value = getProductName.prod_ID.ToString();
                                R1.Cells["HSN_Code"].Value = getProductName.Prod_HSN_Code.ToString();
                                //if (R1.Cells["Product_Descr"].Value == "" || R1.Cells["Product_Descr"].Value == DBNull.Value || R1.Cells["Product_Descr"].Value == null)
                                //{
                                //    R1.Cells["Product_Descr"].Value = getProductName.Product_Descr.ToString();
                                //}
                            }
                        }
                    }

                }

                if (columnName == "ReceivedQty")
                {
                    int Itemcode = Convert.ToInt32(R1.Cells["Item_Code"].Value);
                    if (Itemcode != null)
                    {
                        decimal b, c, d;
                        //c = dgProducts.Rows[dgProducts.CurrentRow.Index];
                        decimal POQty = (R1.Cells["PO_Qty"].Value == "" || R1.Cells["PO_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["PO_Qty"].Value);
                        decimal ReceivedQty = (R1.Cells["ReceivedQty"].Value == "" || R1.Cells["ReceivedQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["ReceivedQty"].Value);
                        decimal RejectedQty = (R1.Cells["RejectedQty"].Value == "" || R1.Cells["RejectedQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["RejectedQty"].Value);
                        if (cmbPurchaseBasis.Text != "Direct")
                        {
                            if (ReceivedQty > POQty)
                            {
                                MessageBox.Show("Received Qty Cannot Be Greater Than PO Qty");
                                dgProducts.CurrentCell = dgProducts.Rows[dgProducts.CurrentRow.Index].Cells[dgProducts.CurrentCell.ColumnIndex];
                                R1.Cells["ReceivedQty"].Value = 0;
                                return;
                            }
                            else
                            {
                                R1.Cells["AcceptedQty"].Value = (ReceivedQty - RejectedQty).ToString("0.00");
                            }
                        }
                        else
                        {
                            R1.Cells["AcceptedQty"].Value = (ReceivedQty - RejectedQty).ToString("0.00");
                        }
                        int taxRate = 0;
                    var taxratelist = (from prd in db.Products join tax in db.Tax_Class_Masters on prd.Prod_Tax_Class equals tax.ID where prd.prod_ID == Itemcode select new { tax.Gst_Rate }).ToList();
                        if (taxratelist.Count > 0)
                        {
                            taxRate = Convert.ToInt32(taxratelist[0].Gst_Rate); //Convert.ToInt32(getProduct_Name.GSTRate);
                        }
                        else
                        {
                            MessageBox.Show("Tax Rate Not Defined / Mapped for the Slected Product");
                            return;
                        }
                        var d1 = (from a in db.Company_Infos where a.Id == logIn.company select new { a.GST_No }).ToList();
                        if (d1.Count > 0)
                        {
                            comnpstatecode = Mid(d1[0].GST_No, 1, 2);
                            suppStateCode = txtStateCode.Text;
                            if (chkRCM.Checked == false)
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
                                R1.Cells["CGST_Per"].Value = "0.00";
                                R1.Cells["SGST_Per"].Value = "0.00";
                                R1.Cells["IGST_Per"].Value = "0.00";
                                cgstPer = 0;
                                sgstPer = 0;
                            }
                        }
                    }

                }
                else
                { 
                    if (columnName == "RejectedQty" || columnName == "Basic_Price" || columnName == "Disc_Per")
                    {
                        decimal ReceivedQty = (R1.Cells["ReceivedQty"].Value == "" || R1.Cells["ReceivedQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["ReceivedQty"].Value);

                        if (ReceivedQty > 0)
                        {

                            decimal RejectedQty = (R1.Cells["RejectedQty"].Value == "" || R1.Cells["RejectedQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["RejectedQty"].Value);
                            if (RejectedQty > ReceivedQty)
                            {
                                MessageBox.Show("Rejected Qty Cannot Be Greater Than Received Qty");
                               
                                dgProducts.CurrentCell = dgProducts.Rows[dgProducts.CurrentRow.Index].Cells[dgProducts.CurrentCell.ColumnIndex];
                                R1.Cells["RejectedQty"].Value = 0;
                                return;
                            }
                            else
                            {
                                R1.Cells["AcceptedQty"].Value = (ReceivedQty - RejectedQty).ToString("0.00");
                            }

                            //R1.Cells["AcceptedQty"].Value = (ReceivedQty - RejectedQty).ToString("0.00");
                        }
                    }
                }
                decimal price = (R1.Cells["Basic_Price"].Value == "" || R1.Cells["Basic_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Basic_Price"].Value);
                decimal DiscPer = (R1.Cells["Disc_Per"].Value == "" || R1.Cells["Disc_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Disc_Per"].Value);

                decimal AcceptedQty = (R1.Cells["AcceptedQty"].Value == "" || R1.Cells["AcceptedQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["AcceptedQty"].Value);

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

                    x += (dgProducts.Rows[i].Cells["AcceptedQty"].Value == "" || dgProducts.Rows[i].Cells["AcceptedQty"].Value == null || dgProducts.Rows[i].Cells["AcceptedQty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["AcceptedQty"].Value);
                    y += (dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == "" || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == null || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value);
                    q += (dgProducts.Rows[i].Cells["Disc_Amt"].Value == "" || dgProducts.Rows[i].Cells["Disc_Amt"].Value == null || dgProducts.Rows[i].Cells["Disc_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Amt"].Value);
                    v += (dgProducts.Rows[i].Cells["Taxable_Value"].Value == "" || dgProducts.Rows[i].Cells["Taxable_Value"].Value == null || dgProducts.Rows[i].Cells["Taxable_Value"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Taxable_Value"].Value);
                    cg += (dgProducts.Rows[i].Cells["CGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["CGST_Amt"].Value == null || dgProducts.Rows[i].Cells["CGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Amt"].Value);
                    sg += (dgProducts.Rows[i].Cells["SGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["SGST_Amt"].Value == null || dgProducts.Rows[i].Cells["SGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Amt"].Value);
                    ig += (dgProducts.Rows[i].Cells["IGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["IGST_Amt"].Value == null || dgProducts.Rows[i].Cells["IGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Amt"].Value);
                    totA += (dgProducts.Rows[i].Cells["Total_Amount"].Value == "" || dgProducts.Rows[i].Cells["Total_Amount"].Value == null || dgProducts.Rows[i].Cells["Total_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Total_Amount"].Value);

                }
               
                txtTotalQty.Text = x.ToString("0.00");

                txtSubTotal.Text = y.ToString("0.00");
                txtTotDiscount.Text = q.ToString("0.00");
                decimal fAmt = (txtfreight.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtfreight.Text);
                decimal OthAmt = (txtothercharges.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtothercharges.Text);
                txtTot_TaxableValue.Text = (v + fAmt + OthAmt).ToString(".00");
                decimal taxvalue = Convert.ToDecimal(txtTot_TaxableValue.Text);
                decimal cgst = (taxvalue * cgstPer) / 100;
                decimal sgst = (taxvalue * sgstPer) / 100;
                 igst = (taxvalue * igstPer) / 100;

                txtTot_CGST.Text = cgst.ToString(".00");
                txtTot_SGST.Text = sgst.ToString(".00");
                txtTot_IGST.Text = igst.ToString(".00");
                //txtTot_TaxableValue.Text = v.ToString("0.00");
                //txtTot_CGST.Text = cg.ToString("0.00");
                //txtTot_SGST.Text = sg.ToString("0.00");
                //txtTot_IGST.Text = ig.ToString("0.00");
                //decimal fAmt = (txtFrieght.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtFrieght.Text);
                decimal AmtForTCs = (v);
                //decimal AmtForTCs = (fAmt + OthAmt + v + cgst + sgst + igst);
                decimal tcsPer = (txttcsper.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txttcsper.Text);

                decimal TcsAmt = AmtForTCs * tcsPer / 100;
                txttcsAmnt.Text = TcsAmt.ToString(".00");
                decimal rndAmt = (textBox1.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(textBox1.Text);
                txtTot_OrderValue.Text = (taxvalue + cgst + sgst + igst + TcsAmt + rndAmt).ToString(".00");

                //txtTot_OrderValue.Text = (totA).ToString("0.00");

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
                if ((from u in db.GoodsReceiptNote_Masters where u.Grn_NO == myString && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    myString = txtSoNo.Text;
                    db.sp_GRN_Delete(myString, logIn.company,logIn.BU_ID);
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
               GoodsReceiptNote_Master S = new GoodsReceiptNote_Master();
                {
                    S.GRN_Type = "MRN";
                    S.Grn_NO = myString;
                    S.Grn_Date = dpSODate.Value;
                    S.Inward_No = txtInwardNo.Text;
                    S.Inward_Date = dtInwardDate.Value;
                    S.Purchase_Basis =(cmbPurchaseBasis.Text);
                    S.SupplierName = Convert.ToInt32(CmbSuplierName.SelectedValue.ToString());
                    S.Purchase_Account = Convert.ToInt32(cmbPurchaseAccount.SelectedValue.ToString());
                    S.TAX_Class = Convert.ToInt32(cmbTaxClass.SelectedValue.ToString());
                    S.Supp_GST_No = txtSupGSTNo.Text;
                    S.DC_No = txtDcNo.Text;
                    if (txtDcNo.Text != "")
                    {
                        S.DC_Date = dpDCDate.Value;
                    }
                    else
                    {
                        S.DC_Date = null;
                    }
                    S.Supplier_InvNo = txtSupplierInvNo.Text;
                    S.Supplier_InvDate = dtsupinvdate.Value;
                    S.OriginalInvReceived = chkOriginalInvoice.Checked;


                    S.TotalQty = (txtTotalQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotalQty.Text);
                    S.SubTotal = (txtSubTotal.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtSubTotal.Text);
                    S.Tot_Discount = (txtTotDiscount.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotDiscount.Text);
                   
                    S.Freight = (txtfreight.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtfreight.Text);
                    S.Other_Charges = (txtothercharges.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtothercharges.Text);
                    S.Tot_TaxableValue = (txtTot_TaxableValue.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTot_TaxableValue.Text);
                    // S.CustomerPONo = (txtvehicleno.Text == "") ? "" : txtvehicleno.Text;                            
                    // S. = (txtTot_TaxableValue.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTot_TaxableValue.Text);

                    //   S.Tot_TaxableValue = (txtTot_TaxableValue.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTot_TaxableValue.Text);
                    // S.Cust_GST_No = (txtSupGSTNo.Text == "") ? "" : txtSupGSTNo.Text;
                    S.Tot_CGST_Amnt = (txtTot_CGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_CGST.Text);
                    S.Tot_SGST_Amnt = (txtTot_SGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_SGST.Text);
                    S.Tot_IGST_Amnt = (txtTot_IGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_IGST.Text);
                    S.Total_Amount = (txtTotal_Amt.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTotal_Amt.Text);
                    S.Tcs_Per = (txttcsper.Text == "" || txttcsper.Text == null) ? Convert.ToDecimal("0.00"): Convert.ToDecimal(txttcsper.Text);
                    S.Tcs_Amount = (txttcsAmnt.Text == "" || txttcsAmnt.Text == null) ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txttcsAmnt.Text);
                    S.Tot_Ord_Value = (txtTot_OrderValue.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_OrderValue.Text);
                    S.Net_GRN_Amount = (txtTot_OrderValue.Text == "" || txtTot_OrderValue.Text == null) ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_OrderValue.Text);

                    if (cmbWareHouse.SelectedValue == null)
                    {
                        S.Warehouse_Code = 0;
                    }
                    else
                    {
                        S.Warehouse_Code = Convert.ToInt32(cmbWareHouse.SelectedValue.ToString());

                    }
                    S.isDeleted = false;
                  
                    S.Vehicle_No = (txtvehicalnr.Text == "") ? "" : txtvehicalnr.Text;
                    S.LrNo_LrDate = (txtlrnodate.Text == "") ? "" : txtlrnodate.Text;
                    S.Transporter_Name = (cmbOtherTermsandNotes.Text == "") ? "" : cmbOtherTermsandNotes.Text;
                    S.Other_Terms = (cmbOtherTermsandNotes.Text==""||cmbOtherTermsandNotes.Text==null)?"": cmbOtherTermsandNotes.Text;
                   // S.ConsigneeName = Convert.ToInt32(1);
                    S.Status = Convert.ToInt32(cmbStatus.SelectedValue.ToString());
                    S.RCM = chkRCM.Checked; 
                    S.IneligibleTax = chkTaxInelgible.Checked;                   
                    S.Company_ID = logIn.company;
                    S.BU_ID = logIn.BU_ID;
                    S.Created_By = lblCreatedBy.Text;                    
                    S.Modified_By = logIn.username + "-" + DateTime.Now;
                    db.GoodsReceiptNote_Masters.InsertOnSubmit(S);
                    db.SubmitChanges();
                }
                        //db.Transaction = transaction;
                for (int i = 0; i < dgProducts.RowCount - 1; i++)
                {
                    GoodsReceiptNote_Child SC = new GoodsReceiptNote_Child();
                    var d1 = (from a in db.GoodsReceiptNote_Masters where a.Grn_NO == myString && a.Company_ID == logIn.company select new { a.Id }).ToList();
                    SC.GRN_Master_ID = d1[0].Id;
                    SC.Grn_NO = myString;
                    SC.Prod_Code = (dgProducts.Rows[i].Cells["Item_Code"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value);                    
                    SC.Product_Description = (dgProducts.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value).ToString();
                    SC.Prod_Spec = (dgProducts.Rows[i].Cells["Item_spec"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_spec"].Value).ToString();
                    SC.Prod_Grade = (dgProducts.Rows[i].Cells["Item_Grade"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value).ToString();
                    SC.HSN_Code = (dgProducts.Rows[i].Cells["HSN_Code"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["HSN_Code"].Value.ToString();
                    SC.Uom = (dgProducts.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["uom"].Value.ToString();
                    SC.PO_Qty = (dgProducts.Rows[i].Cells["PO_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["PO_Qty"].Value);
                    SC.ReceivedQty = (dgProducts.Rows[i].Cells["ReceivedQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["ReceivedQty"].Value);
                    SC.RejectedQty = (dgProducts.Rows[i].Cells["RejectedQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["RejectedQty"].Value);
                    SC.AcceptedQty = (dgProducts.Rows[i].Cells["AcceptedQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["AcceptedQty"].Value);
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
                    SC.PO_No = (dgProducts.Rows[i].Cells["PO_No"].Value == null) ? "" : (dgProducts.Rows[i].Cells["PO_No"].Value).ToString();
                    SC.PR_No = (dgProducts.Rows[i].Cells["PR_No"].Value == null) ? "" : (dgProducts.Rows[i].Cells["PR_No"].Value).ToString();
                    //if (dgProducts.Rows[i].Cells["PR_No"].Value == null|| dgProducts.Rows[i].Cells["PO_Date"].Value == "")
                    //{
                    //    SC.PO_Date = null;
                    //}
                    //else
                    //{
                    //    SC.PO_Date =  Convert.ToDateTime(dgProducts.Rows[i].Cells["PO_Date"].Value.ToString());
                    //}
                    SC.Heat_No = (dgProducts.Rows[i].Cells["Heat_No"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Heat_No"].Value).ToString();
                    SC.TCNo = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["TCNo"].Value).ToString();
                    SC.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value).ToString();
                    SC.ProdSno = i + 1;
                    SC.Company_ID = logIn.company;
                    db.GoodsReceiptNote_Childs.InsertOnSubmit(SC);
                }
                db.SubmitChanges();
                //transaction.Commit();               
                //Update the status of roll data
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Update Bloom_Roll_Wise_Receipts set status = 'Closed' where [Grn_ID]=@param1 and Company_ID =@compName";
                cmd.Parameters.AddWithValue("@param1", txtSoNo.Text);
                cmd.Parameters.AddWithValue("@CompName", logIn.company);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
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
                txtSoNo.Text =MaterialReceiptNoteList.SO_No;
                String myString = "";
                myString = txtSoNo.Text;
                var da = (from obj in db.GoodsReceiptNote_Masters
                          where obj.Grn_NO == txtSoNo.Text && obj.Company_ID == logIn.company
                          select obj).ToList();

                if (da.Count > 0)
                {
                    txtSoNo.Text = da[0].Grn_NO.ToString();
                    dpSODate.Text = da[0].Grn_Date.ToString();
                    //bindCustomer();
                    txtInwardNo.Text = da[0].Inward_No;
                    dtInwardDate.Text = da[0].Inward_Date.ToString();
                    cmbPurchaseBasis.Text = da[0].Purchase_Basis;
                    txtDcNo.Text = da[0].DC_No;
                    dpDCDate.Text = da[0].DC_Date.ToString();
                    txtSupplierInvNo.Text = da[0].Supplier_InvNo;
                    dtsupinvdate.Text = da[0].Supplier_InvDate.ToString();
                    CmbSuplierName.SelectedValue = da[0].SupplierName;
                    txtSupGSTNo.Text = da[0].Supp_GST_No;                  
                    chkOriginalInvoice.Checked = da[0].OriginalInvReceived.Value;
                    txtTotalQty.Text = da[0].TotalQty.ToString();
                    txtSubTotal.Text = da[0].SubTotal.ToString();
                    txtTotDiscount.Text = da[0].Tot_Discount.ToString();
                    txtTot_TaxableValue.Text = da[0].Tot_TaxableValue.ToString();
                    txtTot_CGST.Text = da[0].Tot_CGST_Amnt.ToString();
                    txtTot_SGST.Text = da[0].Tot_SGST_Amnt.ToString();
                    txtTot_IGST.Text = da[0].Tot_IGST_Amnt.ToString();
                    txtTot_OrderValue.Text = da[0].Tot_Ord_Value.ToString();
                    txttcsper.Text = da[0].Tcs_Per.ToString();
                    txttcsAmnt.Text = da[0].Tcs_Amount.ToString();
                    txtfreight.Text = Convert.ToString(da[0].Freight);
                    txtothercharges.Text = Convert.ToString(da[0].Other_Charges);
                    txttcsAmnt.Text = Convert.ToString(da[0].Tcs_Amount);
                    txttcsper.Text = Convert.ToString(da[0].Tcs_Per);
                    txtTotal_Amt.Text = da[0].Total_Amount.ToString();
                    cmbWareHouse.Text = da[0].Warehouse_Code.ToString();
                    txtvehicalnr.Text = da[0].Vehicle_No;
                    txtlrnodate.Text = da[0].LrNo_LrDate;
                    cmbOtherTermsandNotes.Text = da[0].Transporter_Name;
                    txttransportname.Text = da[0].Other_Terms;
                    cmbTaxClass.SelectedValue = da[0].TAX_Class;
                    if (da[0].Purchase_Account != null)
                    {
                        cmbPurchaseAccount.SelectedValue = da[0].Purchase_Account;
                    }
                    if (da[0].RCM != null)
                    {
                        chkRCM.Checked = da[0].RCM.Value;
                    }
                    if (da[0].IneligibleTax != null)
                    {
                        chkTaxInelgible.Checked = da[0].IneligibleTax.Value;
                    }
                    cmbStatus.SelectedValue = da[0].Status;      
                    lblCreatedBy.Text = da[0].Created_By;
                    lblModified.Text = da[0].Modified_By;                    
                }


                var dm1 = (from s in db.GoodsReceiptNote_Childs
                           where s.Grn_NO == myString && s.Company_ID == logIn.company


                           select new

                           {
                               Item_Code = s.Prod_Code,
                               Item_Description = s.Product_Description,
                               Item_Spec = s.Prod_Spec,
                               Item_Grade = s.Prod_Grade,
                               HSN_Code = s.HSN_Code,
                               UOM = s.Uom,                              
                               s.PO_Qty,
                               s.ReceivedQty,
                               s.RejectedQty,
                               s.AcceptedQty,
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
                               s.PO_No,
                               s.PR_No,
                               s.Heat_No,
                               s.TCNo,
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

        public void gridcalculations()
        {
            try
            {
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

                txtTotalQty.Text = x.ToString("0.00");
                txtSubTotal.Text = y.ToString("0.00");
                txtTotDiscount.Text = q.ToString("0.00");
                txtTot_TaxableValue.Text = v.ToString("0.00");
                txtTot_CGST.Text = cg.ToString("0.00");
                txtTot_SGST.Text = sg.ToString("0.00");
                txtTot_IGST.Text = ig.ToString("0.00");
                //decimal fAmt = (txtFrieght.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtFrieght.Text);

                txtTot_OrderValue.Text = (totA).ToString(".00");
            }
            catch (Exception ex)
            {

               
            }
        }
    }
}
