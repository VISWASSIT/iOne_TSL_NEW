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
using System.Windows.Forms.DataVisualization.Charting;
using System.Web.UI.Design;

namespace ioneNet.OrderManagement.Transactions
{
    public partial class frmNewEnquiry : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static int ReceordMasterID;
        public static string Enq_NO,ItemCode,Item_Shape,MtrlGrade;
        public string ParaValue = "";
        public frmNewEnquiry()
        {
            InitializeComponent();
        }

        private void frmNewOrder_Load(object sender, EventArgs e)
        {
          
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");            
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");            
            bindCustomer();           
            bindDroupDown_Lookup();
           // bindCitys();
            AutoincrementId();
            if (ListOfEnquiries.editMode == true)
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
                Boolean recval = false;
                for (int i = 0; i < dgProducts.RowCount - 1; i++)
                {
                    if (dgProducts.Rows[i].Cells["Item_Code"].Value != null)
                    {
                        if (dgProducts.Rows[i].Cells["Item_Grade"].Value == null)
                        {
                            MessageBox.Show("Material Grade Cannot Be Empty");
                            return;
                        }
                        double qty = Convert.ToDouble(dgProducts.Rows[i].Cells["Qty"].Value);
                        if (qty == 0)
                        {
                            MessageBox.Show("Enquiry Qty Should Be Greater Than Zero");
                            return;
                        }


                        //double amt = Convert.ToDouble(dgProducts.Rows[i].Cells["Finish_Wt"].Value);
                        //if (amt > 0)
                        //{
                        //    recval = true;
                        //}
                        //else
                        //{
                        //    recval = false;
                        //}

                    }
                }
                if (txtEnqNo.Text == string.Empty)
                {
                    MessageBox.Show("Enquiry No Should Not Be Empty", "Enquiry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtEnqNo.Focus();
                    return;
                }
                else

               if (CmbBuyerName.Text == string.Empty)
                {
                    MessageBox.Show("Customer Name Should Not Be Empty", "Enquiry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CmbBuyerName.Focus();
                    return;
                }
                else

               if (cmbPaymentterms.Text == string.Empty)
                {
                    MessageBox.Show("Payment Terms Should Not Be Empty", "Enquiry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbPaymentterms.Focus();
                    return;
                }
                else

               if (cmbPricebasis.Text == string.Empty)
                {
                    MessageBox.Show("Price Basis Should Not Be Empty", "Enquiry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbPricebasis.Focus();
                    return;
                }
                else

               if (txtCity.Text == string.Empty)
                {
                    MessageBox.Show("Select The Delivery Location", "Enquiry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCity.Focus();
                    return;
                }
                else

               if (cmbPacking.Text == string.Empty)
                {
                    MessageBox.Show("Packing  Should Not Be Empty", "Enquiry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbPacking.Focus();
                    return;
                }
                else

               if (cmbCutting.Text == string.Empty)
                {
                    MessageBox.Show("Cutting  Should Not Be Empty", "Enquiry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbCutting.Focus();
                    return;
                }
                else

               if (cmbUT.Text == string.Empty)
                {
                    MessageBox.Show("UT Criteria  Should Not Be Empty", "Enquiry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbUT.Focus();
                    return;
                }
                else

               if (cmbSaleExecutive.Text == string.Empty)
                {
                    MessageBox.Show("Select Sales Executive", "Enquiry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbSaleExecutive.Focus();
                    return;
                }

                else
                if (dgProducts.Rows[0].Cells["Item_Code"].Value == null)
                {
                    MessageBox.Show("Atleast One Record To Be Entered To Proceed", "Enquiry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                //else if (recval == false)
                //{
                //    MessageBox.Show("Finish Wt Must be Entered For all the Items", "Enquiry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    return;
                //}

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

        public void bindGridColumns()
        {
            try
            {
                var ColNames = (from m in db.Form_Grid_Columns where m.Company_ID == logIn.company && m.Form_Name == Convert.ToInt32(frmMain.frmname) 
                                  select new { m.Column_Heading, m.Field_Name, m.Column_Width }).ToList();
                if (ColNames.Count > 0)
                {
                    dgProducts.Columns.Clear();
                    dgProducts.ColumnCount = ColNames.Count;
                    for (int i = 0; i < ColNames.Count - 1; i++)
                    {
                       
                        dgProducts.Columns[i].HeaderText = ColNames[i].Column_Heading;
                        dgProducts.Columns[i].Width = Convert.ToInt32(ColNames[i].Column_Width);
                        dgProducts.Columns[i].Name = ColNames[i].Field_Name;



                    }

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

        public void bindCustomer()
        {
            try
            {
                var Buyerblind = (from m in db.Supplier_informations where  m.Company_ID == logIn.company select new { m.ID, m.Supplier_Name }).Distinct().ToList();
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

        //public void bindCitys()
        //{

        //    var bindNearestCity = (from m in db.City_Masters select new { m.City_Name, m.ID }).Distinct().ToList();
        //    if (bindNearestCity.Count > 0)
        //    {
        //        cmbCity.DataSource = bindNearestCity;
        //        cmbCity.ValueMember = "ID";
        //        cmbCity.DisplayMember = "City_Name";
        //    }
        //    if (cmbCity.Items.Count == 1)
        //        cmbCity.SelectedIndex = 0;
        //    else
        //        cmbCity.SelectedIndex = -1;


        //}
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
                //Insurance
                var pIns = (from m in db.Sales_Men_Informations where m.Company_ID == logIn.company select new { m.Id, m.Salesmen_Code }).Distinct().ToList();
                if (pIns.Count > 0)
                {
                    cmbSaleExecutive.DataSource = pIns;
                    cmbSaleExecutive.ValueMember = "Id";
                    cmbSaleExecutive.DisplayMember = "Salesmen_Code";
                }

               
                //Sales Office
                //var SO = (from m in db.Attributes_Datas where m.Head_Name == "Sales Office" select new { m.ID, m.Descr }).Distinct().ToList();
                //if (SO.Count > 0)
                //{
                //    cmbSaleOffice.DataSource = SO;
                //    cmbSaleOffice.ValueMember = "ID";
                //    cmbSaleOffice.DisplayMember = "Descr";
                //}
                ////Payment Terms
                //var PTerms = (from m in db.Attributes_Datas where m.Head_Name == "Payment Terms" select new { m.ID, m.Descr }).Distinct().ToList();
                //if (PTerms.Count > 0)
                //{
                //    cmbPaymentterms.DataSource = PTerms;
                //    cmbPaymentterms.ValueMember = "ID";
                //    cmbPaymentterms.DisplayMember = "Descr";
                //}

                ////Price Basis
                //var PBasis = (from m in db.Attributes_Datas where m.Head_Name == "Price Basis" select new { m.ID, m.Descr }).Distinct().ToList();
                //if (PBasis.Count > 0)
                //{
                //    cmbPricebasis.DataSource = PBasis;
                //    cmbPricebasis.ValueMember = "ID";
                //    cmbPricebasis.DisplayMember = "Descr";
                //}


                //var cutting = (from m in db.Attributes_Datas where m.Head_Name == "Cutting" select new { m.ID, m.Descr }).Distinct().ToList();
                //if (cutting.Count > 0)
                //{
                //    cmbCutting.DataSource = cutting;
                //    cmbCutting.ValueMember = "ID";
                //    cmbCutting.DisplayMember = "Descr";
                //}


                //var Packing = (from m in db.Attributes_Datas where m.Head_Name == "Packing" select new { m.ID, m.Descr }).Distinct().ToList();
                //if (Packing.Count > 0)
                //{
                //    cmbPacking.DataSource = Packing;
                //    cmbPacking.ValueMember = "ID";
                //    cmbPacking.DisplayMember = "Descr";
                //}


                //var ESource = (from m in db.Attributes_Datas where m.Head_Name == "Enq Source" select new { m.ID, m.Descr }).Distinct().ToList();
                //if (ESource.Count > 0)
                //{
                //    cmbEnqSource.DataSource = ESource;
                //    cmbEnqSource.ValueMember = "ID";
                //    cmbEnqSource.DisplayMember = "Descr";
                //}

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

                var result = db.Sp_autoincrement_SaleEnquiry (logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date,logIn.BU_ID);
                txtEnqNo.Text = result.FirstOrDefault().So_no;
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
                    tb3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }
                if (tb3 != null && columnName == "Shape")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
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

                if (columnName == "Item Description")
                {
                    var Prodname = (from d in db.Products where d.Company_ID == logIn.company && d.Prod_Type_Id == 139 select new { d.Prod_Name }).ToList();
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
                if (CmbBuyerName.Text !="")
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
                    var getD = (from c1 in db.Supplier_informations
                                join c3 in db.Attributes_Datas on c1.Area_Region equals c3.ID
                                where c1.Supplier_Name == CmbBuyerName.Text
                                select new { c1.Sale_Executive, c1.Area_Region, c3.Descr, c1.City }).ToList();
                    if (getD.Count > 0)
                    {
                        cmbSaleOffice.Text = getD[0].Descr;
                        txtSalesregion.Text = getD[0].Area_Region.ToString();
                        cmbSaleExecutive.SelectedValue = getD[0].Sale_Executive;
                        txtCity.Text = getD[0].City.ToString();
                    }
                }
            }
            }
            catch (Exception ex)
            {
            }
        }

        private void dgProducts_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
               
                if (e.KeyCode == Keys.F4) //Delivery Information / Locations
                {
                    if (chkSEZSupply.Checked == true)
                    {
                        //ioneNet.OrderManagement.Transactions.SO_MultiDeliveryAddress form = new OrderManagement.Transactions.SO_MultiDeliveryAddress();
                        ////ioneNet.Masters.ProdSearch.frmName = "SOrder";
                        //int i = dgProducts.CurrentCell.RowIndex;
                        //SONo = txtSoNo.Text;
                        //ItemCode = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                        //OrdQty = dgProducts.Rows[i].Cells["Qty"].Value.ToString();
                        //form.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Check Multi Location Delivery? To Add The Details");

                    }

                   
                }
                if (e.KeyCode == Keys.F5)
                {
                    MaterialManagement.Masters.frmProductsNew form = new MaterialManagement.Masters.frmProductsNew();

                    form.ShowDialog();
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

                if (columnName == "Item_Grade")
                {
                    var MGrade = (from c1 in db.QA_Mtrl_Grade_Masters
                                  where c1.Material_Grade == R1.Cells["Item_Grade"].Value
                                  select c1).ToList();
                    if (MGrade.Count > 0)
                    {

                    }
                    else
                    {
                        MessageBox.Show("Invalid Grade Entered");
                        R1.Cells["Item_Grade"].Value = "";
                        return;
                    }

                    //Get Pending Orders

                    var Order = (from data in db.Get_pending_Oders_ItemWise(logIn.company, Convert.ToInt32(R1.Cells["Item_ID"].Value.ToString()), R1.Cells["Item_Grade"].Value.ToString()) select data).ToList();
                    R1.Cells["Pending_Order_Qty"].Value = "0";
                    if (Order.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        R1.Cells["Pending_Order_Qty"].Value = Order[0].Bal_Qty;


                    }


                    //Get Stock Avaibale
                    DateTime t = dpEnqDate.Value;
                    string dt1 = t.ToString("yyyy/MM/dd");
                    var stock = (from data in db.ShowStockReport_FG_ItemWise(logIn.company, Convert.ToDateTime(dt1), logIn.BU_ID, R1.Cells["Item_Grade"].Value.ToString(), Convert.ToInt32(R1.Cells["Item_ID"].Value.ToString())) select data).ToList();
                    R1.Cells["Stock_Avialble"].Value = "0";
                    if (stock.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        R1.Cells["Stock_Avialble"].Value = stock[0].ClosingQty;


                    }
                }
                if (columnName == "Item_Description")
                {

                    decimal b, c, d;
                    decimal taxRate;
                    //c = dgProducts.Rows[dgProducts.CurrentRow.Index];
                    //R1.Cells["Item_Code"].Value = dgProducts.CurrentCell.RowIndex+1;
                    var State = (from c1 in db.Products
                                 join U in db.UoM_Masters on c1.Prod_Primary_UOM_Id equals U.UOM_ID
                                 where c1.Prod_Name == R1.Cells["Item_Description"].Value
                                 select new { c1.prod_ID,UOM = U.Uom_Descr, c1.Prod_Code }).ToList();
                    if (State.Count > 0)
                    {
                        R1.Cells["Item_ID"].Value = State[0].prod_ID;
                        R1.Cells["Item_Code"].Value = State[0].Prod_Code;
                        R1.Cells["UOM"].Value = State[0].UOM;

                        


                        

                    }
                    else
                    {
                        MessageBox.Show("Invalid Product Name Entered");
                    }


                    //var d1 = (from a in db.Company_Infos where a.Id == logIn.company select new { a.GST_No }).ToList();
                    //if (d1.Count > 0)
                    //{
                    //    comnpstatecode = Mid(d1[0].GST_No, 1, 2);
                    //    suppStateCode = Mid(txtCustGSTNo.Text, 1, 2);
                    //    if (chkSEZOrder.Checked==false)
                    //    {
                    //        if (suppStateCode == comnpstatecode)
                    //        {
                    //            d = Convert.ToDecimal(taxRate) / 2;
                    //            R1.Cells["CGST_Per"].Value = d.ToString("0.00");
                    //            R1.Cells["SGST_Per"].Value = d.ToString("0.00");
                    //            R1.Cells["IGST_Per"].Value = "0.00";
                    //        }
                    //        else
                    //        {
                    //            d = taxRate;
                    //            R1.Cells["CGST_Per"].Value = "0.00";
                    //            R1.Cells["SGST_Per"].Value = "0.00";
                    //            R1.Cells["IGST_Per"].Value = d.ToString("0.00");
                    //        }
                    //    }
                    //    else
                    //    {
                    //        R1.Cells["CGST_Per"].Value = "0.00";
                    //        R1.Cells["SGST_Per"].Value = "0.00";
                    //        R1.Cells["IGST_Per"].Value = "0.00";
                    //    }
                    //}

                }
                if (R1.Cells["Item_Description"].Value != null)
                {
                    decimal ReceivedQty = (R1.Cells["Qty"].Value == "" || R1.Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty"].Value);
                    //decimal RejectedQty = (R1.Cells["Qty_Rejected"].Value == "" || R1.Cells["Qty_Rejected"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty_Rejected"].Value);

                    string Custstatetcode;                   

                    if (columnName == "Basic_Price" || columnName == "Disc_Per")
                    {
                        //if (ReceivedQty > 0)
                        //{
                        //    decimal AcceptedQty = (R1.Cells["Qty"].Value == "" || R1.Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty"].Value);
                        //    decimal price = (R1.Cells["Basic_Price"].Value == "" || R1.Cells["Basic_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Basic_Price"].Value);
                        //    decimal DiscPer = (R1.Cells["Disc_Per"].Value == "" || R1.Cells["Disc_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Disc_Per"].Value);


                        //    decimal Amt, DiscAmt, netAmt, gst, igst, totamt;

                        //    Amt = AcceptedQty * price;

                        //    R1.Cells["Amt_Before_Disc"].Value = Amt.ToString("0.00");
                        //    DiscAmt = (Amt * DiscPer) / 100;
                        //    R1.Cells["Disc_Amt"].Value = DiscAmt.ToString("0.00");
                        //    netAmt = Amt - DiscAmt;
                        //    R1.Cells["Taxable_Value"].Value = netAmt.ToString("0.00");
                        //    gst = (Convert.ToDecimal(R1.Cells["Taxable_Value"].Value) * Convert.ToDecimal(R1.Cells["CGST_Per"].Value)) / 100;

                        //    R1.Cells["CGST_Amt"].Value = gst;
                        //    R1.Cells["SGST_Amt"].Value = gst;
                        //    igst = (Convert.ToDecimal(R1.Cells["Taxable_Value"].Value) * Convert.ToDecimal(R1.Cells["IGST_Per"].Value)) / 100;
                        //    R1.Cells["IGST_Amt"].Value = igst;

                        //    totamt = Math.Round(netAmt + gst + gst + igst);
                        //    R1.Cells["Total_Amount"].Value = totamt;
                        //    decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
                        //    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                        //    {

                        //        x += (dgProducts.Rows[i].Cells["Qty"].Value == "" || dgProducts.Rows[i].Cells["Qty"].Value == null || dgProducts.Rows[i].Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty"].Value);
                        //        y += (dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == "" || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == null || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value);
                        //        q += (dgProducts.Rows[i].Cells["Disc_Amt"].Value == "" || dgProducts.Rows[i].Cells["Disc_Amt"].Value == null || dgProducts.Rows[i].Cells["Disc_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Amt"].Value);
                        //        v += (dgProducts.Rows[i].Cells["Taxable_Value"].Value == "" || dgProducts.Rows[i].Cells["Taxable_Value"].Value == null || dgProducts.Rows[i].Cells["Taxable_Value"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Taxable_Value"].Value);
                        //        cg += (dgProducts.Rows[i].Cells["CGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["CGST_Amt"].Value == null || dgProducts.Rows[i].Cells["CGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Amt"].Value);
                        //        sg += (dgProducts.Rows[i].Cells["SGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["SGST_Amt"].Value == null || dgProducts.Rows[i].Cells["SGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Amt"].Value);
                        //        ig += (dgProducts.Rows[i].Cells["IGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["IGST_Amt"].Value == null || dgProducts.Rows[i].Cells["IGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Amt"].Value);
                        //        totA += (dgProducts.Rows[i].Cells["Total_Amount"].Value == "" || dgProducts.Rows[i].Cells["Total_Amount"].Value == null || dgProducts.Rows[i].Cells["Total_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Total_Amount"].Value);

                        //    }

                        //    txtTotalQty.Text = x.ToString(".00");
                        //    txtSubTotal.Text = y.ToString("0.00");
                        //    txtTotDiscount.Text = q.ToString(".00");
                        //    txtTot_TaxableValue.Text = v.ToString(".00");
                        //    txtTot_CGST.Text = cg.ToString(".00");
                        //    txtTot_SGST.Text = sg.ToString(".00");
                        //    txtTot_IGST.Text = ig.ToString(".00");
                        //    //decimal fAmt = (txtFrieght.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtFrieght.Text);

                        //    txtTot_OrderValue.Text = (totA).ToString(".00");
                        //}

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
                //else
                //   if (txtCustGSTNo.Text == "")
                //{
                //    MessageBox.Show("Customer GST No Required To Proceed");
                //}
                //else
                //      if (cmbTaxClass.Text == "")
                //{
                //    MessageBox.Show("Select Tax Class To Proceed");
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

        private void cmbPriceBasis_Click(object sender, EventArgs e)
        {
           
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtSoNo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtEnqNo.Text != "")
                {

                    if (ListOfEnquiries.editMode == true)
                    {
                    }
                    else
                    { 

                        if ((from u in db.Sale_Enquiry_Masters where u.Enq_NO == txtEnqNo.Text && u.Company_ID == logIn.company select u).Count() > 0)
                        {
                            MessageBox.Show("Enquiry No Cannot Be Duplicate", "Enquiry Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtEnqNo.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Enquiry Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtSoNo_Layout(object sender, LayoutEventArgs e)
        {

        }

        private void label33_Click(object sender, EventArgs e)
        {

        }

        private void frmNewEnquiry_FormClosed(object sender, FormClosedEventArgs e)
        {
            //db.Sp_delete_EnqSpecData_No_Link();
        }

        private void label7_Click(object sender, EventArgs e)
        {
            GlobalVariables.attrdesc = "Payment Terms";
            OrderManagement.Masters.frmattributesdata form = new OrderManagement.Masters.frmattributesdata();

            form.ShowDialog();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GlobalVariables.attrdesc = "Sales Office";
            OrderManagement.Masters.frmattributesdata form = new OrderManagement.Masters.frmattributesdata();

            form.ShowDialog();
        }

        private void label10_Click(object sender, EventArgs e)
        {
            GlobalVariables.attrdesc = "Price Basis";
            OrderManagement.Masters.frmattributesdata form = new OrderManagement.Masters.frmattributesdata();

            form.ShowDialog();
        }

        private void label11_Click(object sender, EventArgs e)
        {
            
        }

        private void label12_Click(object sender, EventArgs e)
        {
            GlobalVariables.attrdesc = "Packing";
            OrderManagement.Masters.frmattributesdata form = new OrderManagement.Masters.frmattributesdata();

            form.ShowDialog();
        }

        private void label14_Click(object sender, EventArgs e)
        {
            GlobalVariables.attrdesc = "Cutting";
            OrderManagement.Masters.frmattributesdata form = new OrderManagement.Masters.frmattributesdata();

            form.ShowDialog();
        }

        //private void comboBox3_Enter(object sender, EventArgs e)
        //{
        //    string cityname = cmbCity.Text;
        //    bindCitys();
        //    cmbCity.Text = cityname;
        //}

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GlobalVariables.attrdesc = "Enq Source";
            OrderManagement.Masters.frmattributesdata form = new OrderManagement.Masters.frmattributesdata();

            form.ShowDialog();
        }

        private void linkLabel2_Click(object sender, EventArgs e)
        {
            //GlobalVariables.attrdesc = "Enq Source";
            //OrderManagement.Masters.frmattributesdata form = new OrderManagement.Masters.frmattributesdata();

            //form.ShowDialog();
        }

        private void btnaddnewcustomer_Click(object sender, EventArgs e)
        {
            MaterialManagement.SupplierMaster form = new MaterialManagement.SupplierMaster();
            
            form.ShowDialog();
        }

        private void linkLabel3_Click(object sender, EventArgs e)
        {
            ioneNet.MaterialManagement.Masters.frmProductsNew form = new ioneNet.MaterialManagement.Masters.frmProductsNew();                                   
            form.ShowDialog();
        }

        private void label26_Click(object sender, EventArgs e)
        {

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

        public void AddPara(AutoCompleteStringCollection coll)
        {
            try
            {

                var Prodname = (from d in db.Attributes_Datas
                                where d.Head_Name == ParaValue  
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

                        txtPackCode.Text = da[0].ID.ToString();

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

        private void cmbSaleOffice_Enter(object sender, EventArgs e)
        {
            try
            {
                ParaValue = "Sales Office";
                AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                AddPara(DataColl);
                cmbSaleOffice.AutoCompleteCustomSource = DataColl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbSaleOffice_Leave(object sender, EventArgs e)
        {
            try
            {
                //    txtSoNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_No;
                //txtAmendNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_Amend_No;
                if (cmbSaleOffice.Text != "")
                {

                    var da = (from obj in db.Attributes_Datas
                              where obj.Head_Name == "Sales Office" && obj.Descr == cmbSaleOffice.Text
                              select obj).ToList();

                    if (da.Count > 0)
                    {

                        txtSalesregion.Text = da[0].ID.ToString();

                    }
                    else
                    {
                        MessageBox.Show("Invalid Entry, Click on Link to Add New Values");
                        cmbSaleOffice.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbPaymentterms_Enter(object sender, EventArgs e)
        {
            try
            {
                ParaValue = "Payment Terms";
                AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                AddPara(DataColl);
                cmbPaymentterms.AutoCompleteCustomSource = DataColl;
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void cmbPaymentterms_Leave(object sender, EventArgs e)
        {
            try
            {
                //    txtSoNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_No;
                //txtAmendNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_Amend_No;
                if (cmbPaymentterms.Text != "")
                {

                    var da = (from obj in db.Attributes_Datas
                              where obj.Head_Name == "Payment Terms" && obj.Descr == cmbPaymentterms.Text
                              select obj).ToList();

                    if (da.Count > 0)
                    {

                        txtPayment.Text = da[0].ID.ToString();

                    }
                    else
                    {
                        MessageBox.Show("Invalid Entry, Click on Link to Add New Values");
                        cmbPaymentterms.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbPricebasis_Enter(object sender, EventArgs e)
        {
            try
            {
                ParaValue = "Price Basis";
                AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                AddPara(DataColl);
                cmbPricebasis.AutoCompleteCustomSource = DataColl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }


        }

        private void cmbPricebasis_Leave(object sender, EventArgs e)
        {
            try
            {
                //    txtSoNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_No;
                //txtAmendNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_Amend_No;
                if (cmbPricebasis.Text != "")
                {

                    var da = (from obj in db.Attributes_Datas
                              where obj.Head_Name == "Price Basis" && obj.Descr == cmbPricebasis.Text
                              select obj).ToList();

                    if (da.Count > 0)
                    {

                        txtPricebasis.Text = da[0].ID.ToString();

                    }
                    else
                    {
                        MessageBox.Show("Invalid Entry, Click on Link to Add New Values");
                        cmbPricebasis.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private void linkLabel3_Click_1(object sender, EventArgs e)
        {
            //GlobalVariables.attrdesc = "Payment Terms";
            //OrderManagement.Masters.frmattributesdata form = new OrderManagement.Masters.frmattributesdata();

            //form.ShowDialog();
        }

        private void linkLabel4_Click(object sender, EventArgs e)
        {
            //GlobalVariables.attrdesc = "Price Basis";
            //OrderManagement.Masters.frmattributesdata form = new OrderManagement.Masters.frmattributesdata();

            //form.ShowDialog();
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GlobalVariables.attrdesc = "Payment Terms";
            OrderManagement.Masters.frmattributesdata form = new OrderManagement.Masters.frmattributesdata();

            form.ShowDialog();
        }

        private void linkLabel4_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GlobalVariables.attrdesc = "Price Basis";
            OrderManagement.Masters.frmattributesdata form = new OrderManagement.Masters.frmattributesdata();

            form.ShowDialog();
        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //GlobalVariables.attrdesc = "Price Basis";
            //OrderManagement.Masters.frmattributesdata form = new OrderManagement.Masters.frmattributesdata();

            //form.ShowDialog();
        }

        private void linkLabel7_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GlobalVariables.attrdesc = "Packing";
            OrderManagement.Masters.frmattributesdata form = new OrderManagement.Masters.frmattributesdata();

            form.ShowDialog();
        }

        private void linkLabel8_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GlobalVariables.attrdesc = "Cutting";
            OrderManagement.Masters.frmattributesdata form = new OrderManagement.Masters.frmattributesdata();

            form.ShowDialog();
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

        private void linkLabel9_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            GlobalVariables.attrdesc = "UT Required";
            OrderManagement.Masters.frmattributesdata form = new OrderManagement.Masters.frmattributesdata();

            form.ShowDialog();
        }

        private void cmbEnqSource_Enter(object sender, EventArgs e)
        {
            try
            {
                ParaValue = "Enq Source";
                AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                AddPara(DataColl);
                cmbEnqSource.AutoCompleteCustomSource = DataColl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void cmbEnqSource_Leave(object sender, EventArgs e)
        {
            try
            {

                if (cmbEnqSource.Text != "")
                {

                    var da = (from obj in db.Attributes_Datas
                              where obj.Head_Name == "Enq Source" && obj.Descr == cmbEnqSource.Text
                              select obj).ToList();

                    if (da.Count > 0)
                    {

                        txtEnqSource.Text = da[0].ID.ToString();

                    }
                    else
                    {
                        MessageBox.Show("Invalid Entry, Click on Link to Add New Values");
                        cmbEnqSource.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OrderManagement.frmAddSalesMen form = new OrderManagement.frmAddSalesMen();

            form.ShowDialog();
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            //string SelText = "";
            //for (int i = 0; i < chkStdRefer.Items.Count - 1; i++)
            //{
                
            //    SelText = SelText+","+chkStdRefer.SelectedValue.ToString();
            //}
            //txtStandardRefer.Text = SelText;
        }

        public void Save()
        {
            try
            {
                String myString = "";
                myString = txtEnqNo.Text;
                 if (txtEnqNo.Text != "")
                {
                    if (ListOfEnquiries.var == "1")
                    {
                                         

                    }
                    else
                    {
                        myString = txtEnqNo.Text;
                        db.sp_Enq_Delete(ReceordMasterID.ToString(), logIn.company);
                    }

                }
                else
                {
                    //AutoincrementId();
                    myString = txtEnqNo.Text;

                }
                
                dgProducts.Enabled = false;
                //var S = db.Sale_Order_Masters.Where(w => w.SO_NO == myString && w.Company_ID == logIn.company).FirstOrDefault();
                Sale_Enquiry_Master S = new Sale_Enquiry_Master();
                {
                    S.Enq_NO = myString;
                    S.Enq_Date = dpEnqDate.Value;
                    S.Cust_RefNo = txtCustRefNo.Text;
                    S.Cust_Ref_Date = dpCustRefDate.Value;
                    S.Enq_Source = Convert.ToInt32(txtEnqSource.Text);
                    S.BuyerName = Convert.ToInt32(CmbBuyerName.SelectedValue.ToString());
                    S.Status = Convert.ToInt32(cmbStatus.SelectedValue.ToString()); 
                     S.Delivery_Date = dtDeliveryDate.Value;
                    S.Quote_Submit_On = dtpQuoteToSubmit.Value;
                    S.SaleExecutive = Convert.ToInt32(cmbSaleExecutive.SelectedValue.ToString());
                    //S.Sale_office = Convert.ToInt32(txtSalesregion.Text); ;
                    S.SEZ_Supply = (chkSEZSupply.Checked == true) ? true : false;                            
                    S.New_Enq = (chkNew.Checked == true) ? true : false;
                    S.Export_Enquiry = (chkExport.Checked == true) ? true : false;
                    S.TPI_Involved = (chkTPI.Checked == true) ? true : false;
                    S.IBR_Cert = (chkIBR.Checked == true) ? true : false;
                    S.UT_Req = Convert.ToInt32(txtUT.Text);
                    S.Macro = (chkMacro.Checked == true) ? true : false;
                    S.Packing = Convert.ToInt32(txtPackCode.Text);
                    S.Cutting = Convert.ToInt32(txtCutting.Text);
                    S.Payment_Terms = Convert.ToInt32(txtPayment.Text);
                    S.Price_Basis = Convert.ToInt32(txtPricebasis.Text);
                    S.Delivery_Location = (txtCity.Text == "") ? "" : txtCity.Text;

                    S.Special_Instructions = (txtOtherRemarks.Text == "") ? "" : txtOtherRemarks.Text;
                    S.Company_ID = logIn.company;
                    S.Created_By = lblCreatedBy.Text;
                    S.bu_id = logIn.BU_ID;
                    S.Modified_By = logIn.username + "-" + DateTime.Now;
                    db.Sale_Enquiry_Masters.InsertOnSubmit(S);
                    db.SubmitChanges();
                }
                        //db.Transaction = transaction;
                for (int i = 0; i < dgProducts.RowCount - 1; i++)
                {
                    Sale_Enquiry_Child SC = new Sale_Enquiry_Child();
                            
                    SC.Enq_NO = myString;
                    var d1 = (from a in db.Sale_Enquiry_Masters where a.Enq_NO == myString && a.Company_ID == logIn.company select new { a.Id }).ToList();
                    SC.Enq_Master_ID = d1[0].Id;

                    SC.Prod_Code = Convert.ToInt32(dgProducts.Rows[i].Cells["Item_ID"].Value);
                    //           SC.Product_Description = (dgProducts.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value).ToString();
                    SC.Prod_Grade = (dgProducts.Rows[i].Cells["Item_Grade"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value).ToString();
                    SC.Length = (dgProducts.Rows[i].Cells["Prod_Length"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Prod_Length"].Value).ToString();
                    SC.Uom = (dgProducts.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["uom"].Value.ToString();
                    SC.Qty = (dgProducts.Rows[i].Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty"].Value);
                    SC.Stock_Avialble = (dgProducts.Rows[i].Cells["Stock_Avialble"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Stock_Avialble"].Value);
                    SC.Pending_Order_Qty = (dgProducts.Rows[i].Cells["Pending_Order_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Pending_Order_Qty"].Value);
                    SC.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value).ToString();
                    SC.Company_ID = logIn.company;
                    db.Sale_Enquiry_Childs.InsertOnSubmit(SC);
                }
                db.SubmitChanges();
                //transaction.Commit();               
                MessageBox.Show("Record Saved / Updated Successfully With Transaction Ref No : " + txtEnqNo.Text);
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
                txtEnqNo.Text = OrderManagement.Transactions.ListOfEnquiries.SO_No;
                 String myString = "";
                ReceordMasterID = 0;
                myString = txtEnqNo.Text;
                var da = (from obj in db.getEnq_Master_data(logIn.company,logIn.BU_ID,txtEnqNo.Text)
                          
                          select new { obj.Id,obj.Enq_NO,obj.Enq_Date,obj.BuyerName,obj.New_Enq,
                              obj.SEZ_Supply,obj.Export_Enquiry,obj.TPI_Involved,obj.IBR_Cert,obj.UT_Req,obj.Special_Instructions,
                              obj.Macro,obj.Cust_RefNo,obj.Cust_Ref_Date,obj.Quote_Submit_On,obj.Delivery_Date,
                              obj.Sale_office,obj.SaleExecutive,obj.Enq_Source,obj.Payment_Terms,obj.Price_Basis,
                              obj.Packing,obj.Cutting,obj.Delivery_Location,obj.Status,obj.Modified_By,obj.Created_By,

                              obj.Sales_Desc ,
                              obj.Enq_Source_Desc,
                              obj.Payment_Desc,
                              obj.UT_Desc ,
                              obj.Executive_Desc,
                              obj.Price_Desc,
                              obj.Packing_Desc ,obj.Cutting_Desc }).ToList();

                if (da.Count > 0)
                {
                    ReceordMasterID = da[0].Id;
                    txtEnqNo.Text = da[0].Enq_NO.ToString();
                    dpEnqDate.Text = da[0].Enq_Date.ToString();
                    //bindCustomer();
                    CmbBuyerName.SelectedValue = da[0].BuyerName;                  
                     if(da[0].New_Enq == true)
                    {
                        chkNew.Checked = true;                    
                    }
                    else
                    {
                      chkNew.Checked = false;                        
                    }
                    if (da[0].SEZ_Supply == true)
                    {
                        chkSEZSupply.Checked = true;
                    }
                    else
                    {
                        chkSEZSupply.Checked = false;
                    }
                    if (da[0].Export_Enquiry == true)
                    {
                        chkExport.Checked = true;
                    }
                    else
                    {
                        chkExport.Checked = false;
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

                    //cmbCustomer.Enabled = false;
                    txtOtherRemarks.Text = da[0].Special_Instructions;
                    txtCustRefNo.Text = da[0].Cust_RefNo;
                    dpCustRefDate.Text = da[0].Cust_Ref_Date.ToString();
                    dtpQuoteToSubmit.Text = da[0].Quote_Submit_On.ToString();
                    dtDeliveryDate.Text = da[0].Delivery_Date.ToString();
                    
                   
                    dtDeliveryDate.Text = da[0].Delivery_Date.ToString();
                    if (da[0].Sales_Desc != null)
                    {
                        txtSalesregion.Text = da[0].Sale_office.ToString();
                        cmbSaleOffice.Text = da[0].Sales_Desc.ToString();
                    }
                    if (da[0].SaleExecutive != null)
                    {
                        cmbSaleExecutive.SelectedValue = da[0].SaleExecutive;
                    }
                    if (da[0].Enq_Source_Desc != null)
                    {
                        txtEnqSource.Text = da[0].Enq_Source.ToString();
                        cmbEnqSource.Text = da[0].Enq_Source_Desc.ToString();
                    }


                    if (da[0].Payment_Desc != null)
                    {
                        txtPayment.Text = da[0].Payment_Terms.ToString();
                        cmbPaymentterms.Text = da[0].Payment_Desc.ToString();
                    }
                    if (da[0].UT_Desc != null)
                    {
                        txtUT.Text = da[0].UT_Req.ToString();
                        cmbUT.Text = da[0].UT_Desc.ToString();
                    }

                    if (da[0].Price_Desc != null)
                    {
                        txtPricebasis.Text = da[0].Price_Basis.ToString();
                        cmbPricebasis.Text = da[0].Price_Desc.ToString();
                    }
                    if (da[0].Packing_Desc != null)
                    {       
                        txtPackCode.Text = da[0].Packing.ToString();
                        cmbPacking.Text = da[0].Packing_Desc.ToString();
                    }
                    if (da[0].Cutting_Desc != null)
                    {
                        txtCutting.Text = da[0].Cutting.ToString();
                        cmbCutting.Text = da[0].Cutting_Desc.ToString();
                    }
                   
                    txtCity.Text = da[0].Delivery_Location.ToString();
                    


                    cmbStatus.SelectedValue = da[0].Status;      
                    lblCreatedBy.Text = da[0].Created_By;
                    lblModified.Text = da[0].Modified_By;                    
                }


                var dm1 = (from s in db.Sale_Enquiry_Childs                         
                           join p in db.Products on s.Prod_Code equals p.prod_ID
                           where s.Enq_Master_ID == ReceordMasterID && s.Company_ID == logIn.company

                           select new

                           {
                               Item_ID = p.prod_ID,
                               Item_Code =p.Prod_Code,
                            
                               Item_Description =p.Prod_Name,
                               Item_Grade =s.Prod_Grade,
                               Prod_Length = s.Length,
                               UOM=s.Uom,
                               s.Qty,                            
                               s.Stock_Avialble,
                               s.Pending_Order_Qty,
                               s.Remarks

                           });

                SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataTable dtr = new DataTable();
                da2.Fill(dtr);
                if (dtr.Rows.Count >= 0)
                    dgProducts.DataSource = dtr;

                //linkLabel3.Visible = false;
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
