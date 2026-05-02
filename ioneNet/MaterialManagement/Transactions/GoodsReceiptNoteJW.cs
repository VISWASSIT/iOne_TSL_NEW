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
using System.Runtime.Remoting.Metadata.W3cXsd2001;

namespace ioneNet.MaterialManagement.Transactions
{
    public partial class GoodsReceiptNoteJW : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();
        public static string SONo,ItemCode,RecQty,Suppname;

        public GoodsReceiptNoteJW()
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

            if (GRNJWList.var == "0")
            {
                if (GRNJWList.editMode == true)
                {
                    bindedit();
                }
            }
            else
            if (GRNJWList.var == "1")
            {
                if (GRNJWList.editMode == true)
                {
                    bindedit();
                    btnClear.Enabled = false;
                    btnSave.Enabled = false;
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
                    MessageBox.Show("Customer Name Should Not Be Empty", "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CmbSuplierName.Focus();
                    return;
                }
                else if (txtSupplierInvNo.Text == string.Empty)
                {
                    MessageBox.Show("Supplier Inv no  Should Not Be Empty");
                    txtSupplierInvNo.Focus();
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
                var Buyerblind = (from m in db.Supplier_informations where m.Company_ID==logIn.company select new { m.ID, m.Supplier_Name }).Distinct().ToList();
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
                CmbSuplierName.SelectedIndex = -1;

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
                //var SO = (from m in db.Attributes_Datas where m.Head_Name == "Sales Office" select new { m.ID, m.Descr }).Distinct().ToList();
                //if (SO.Count > 0)
                //{
                //    cmbWareHouse.DataSource = SO;
                //    cmbWareHouse.ValueMember = "ID";
                //    cmbWareHouse.DisplayMember = "Descr";
                //}
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

                var result = db.Sp_autoincrement_GRNJW (logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date);
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
                if (tb3 != null && columnName == "Item Received")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }

                else
                {
                    //tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    //tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    //AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    //addItems(DataColl);
                    //tb3.AutoCompleteCustomSource = DataColl;

                   tb3.AutoCompleteMode = AutoCompleteMode.None;
                    //tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    //AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    //addItems(DataColl);
                    //tb3.AutoCompleteCustomSource = DataColl;
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
                    if (columnName == "Item Received")
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
                    //else
                    //{
                    //    if (columnName == "Make /Model / Grade")
                    //    {
                    //        //var Prodname = (from d in db.GoodsReceiptNote_Childs where d.Company_ID == logIn.company select new { d.Prod_Grade }).Distinct().ToList();
                    //        //DataTable dt = new DataTable();
                    //        //dt.Columns.Add("Prod_Grade");
                    //        //foreach (var item in Prodname)
                    //        //{
                    //        //    dt.Rows.Add(item.Prod_Grade);
                    //        //}
                    //        //for (int i = 0; i < dt.Rows.Count; i++)
                    //        //{
                    //        //    coll.Add(dt.Rows[i][0].ToString());
                    //        //}
                    //    }
                    //}

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                //if (txttcsAmnt.Text != "" && txttcsAmnt.Text != "0.00")
                //{

                //    decimal per = Convert.ToDecimal(txttcsper.Text);
                //    decimal amnt = Convert.ToDecimal(txttcsAmnt.Text);
                //    decimal percentamnt = (amnt * per) / 100;
                //    //decimalnt totalorder = Convert.ToInt32(txtTot_OrderValue.Text);
                //    decimal totaltaxable = Convert.ToDecimal(txtTot_TaxableValue.Text);
                //    decimal cgst = Convert.ToDecimal(txtTot_CGST.Text);
                //    decimal sgst = Convert.ToDecimal(txtTot_SGST.Text);
                //    decimal igst = Convert.ToDecimal(txtTot_IGST.Text);
                //    decimal freight = Convert.ToDecimal(txtfreight.Text);
                //    decimal othercharges = Convert.ToDecimal(txtothercharges.Text);


                //    txtTot_OrderValue.Text = Convert.ToString(amnt + percentamnt + totaltaxable + cgst + sgst + igst + freight + othercharges);
                //}

            }
            catch (Exception ex)
            {


            }
        }

        decimal igstamnt, otherper;
        public void othercalculation()
        {
            decimal Finalother, othercharges, otherchargesvalue, totalotherchargesvalue,   otheramnt=0;
            decimal Freight, pervalue, Freightmnt, Finalfrght=0;
            decimal igstper, igstcharges, TotIGST,  Finaligst, igstfreightamnt, igstotheramnt=0;
            try
            {

                decimal j = 0; decimal CGST_Amt = 0; decimal IGST_Amt = 0; decimal SGST_Amt = 0;
                for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                {
                    // CGST_Per
                    decimal CGST_Per = Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Per"].Value.ToString());
                    decimal IGST_Per = Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Per"].Value.ToString());
                    CGST_Amt += Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Amt"].Value.ToString());
                    SGST_Amt += Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Amt"].Value.ToString());
                    IGST_Amt += Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Amt"].Value.ToString());
                    if (CGST_Per > 0)
                    {
                        if (j < CGST_Per + CGST_Per)
                        {
                            j = Convert.ToInt32(CGST_Per + CGST_Per);

                        }
                    }
                    else
                    {
                        if (j < IGST_Per + IGST_Per)
                        {
                            j = Convert.ToInt32(IGST_Per + IGST_Per);

                        }
                    }
                }
                               


                if (txtTot_IGST.Text == null || txtTot_IGST.Text == "" || txtTot_IGST.Text == "0.00")
                {
                    igstcharges = 0;
                    TotIGST = 0;
                    igstfreightamnt = 0;
                    igstotheramnt = 0;
                }
                else
                {
                    igstcharges = Convert.ToDecimal(txtTot_IGST.Text);
                    igstper = Convert.ToInt32(j);
                    igstfreightamnt = Math.Round((igstcharges * igstper) / 100);
                    igstotheramnt = Math.Round((igstcharges * igstper) / 100);



                }

                decimal FinalCGSTAMount = 0; decimal FinalSGSTAMount = 0; decimal FinalIGSTAMount = 0;

                if (CGST_Amt != 1)
                {
                    txtTot_CGST.Text = Convert.ToString(Finalfrght + CGST_Amt);
                    txtTot_SGST.Text = Convert.ToString(Finalfrght + SGST_Amt);
                    FinalCGSTAMount = Finalfrght + CGST_Amt;
                    FinalSGSTAMount = Finalfrght + SGST_Amt;


                }
                if (txtTot_IGST.Text != "0.00" )
                {
                    TotIGST = Convert.ToDecimal(txtTot_IGST.Text);
                    igstper = Convert.ToInt32(j);
                    igstamnt = 0;
                    
                   
                }


                decimal TotalOrderValue = Convert.ToDecimal(txtTot_TaxableValue.Text)+ FinalCGSTAMount + FinalSGSTAMount + FinalIGSTAMount  + otherper + igstamnt;
                txtTot_OrderValue.Text = Convert.ToString(Math.Round(TotalOrderValue));


            }
            catch (Exception ex)
            {


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
                
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sfDataGrid1_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            //groupBox2.Visible = false;
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

                //Get Gate Pass Data
                var Buyerblind = (from m in db.GP_Status_Views where m.Company_ID == logIn.company && m.BU_ID == logIn.BU_ID && m.Supplier_Name == CmbSuplierName.Text && m.BalQty>0 select new { m.GateVch_No }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    cmbPurchaseBasis.DataSource = Buyerblind;
                    cmbPurchaseBasis.ValueMember = "GateVch_No";
                    cmbPurchaseBasis.DisplayMember = "GateVch_No";
                }
                cmbPurchaseBasis.SelectedIndex = -1;
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

        private void cmbPurchaseBasis_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Get Gate Pass Data
                var gpData = (from m in db.GP_Status_Views where m.GateVch_No == cmbPurchaseBasis.Text && m.Company_ID == logIn.company && m.Supplier_Name == CmbSuplierName.Text && m.BalQty > 0

                              select new {
                                  Item_Code= m.Prod_Code,
                                  Item_Description= m.Product_Description,
                                  GP_Qty =m.BalQty,
                                  Basic_Price = m.Price,
                                  m.UOM
                              });
                SqlCommand cmd = (SqlCommand)db.GetCommand(gpData);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                DataRow dr = dt.NewRow();
                dgProducts.DataSource = dt;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dgProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Suppname = CmbSuplierName.Text;
            DataTable dtexisting = new DataTable();
            MaterialManagement.Transactions.GP_Dialog form = new MaterialManagement.Transactions.GP_Dialog();
            form.ShowDialog();

            if (dgProducts.Rows.Count > 1)
            {
                dtexisting.Rows.Clear();
                dtexisting.Columns.Clear();
                dtexisting.Columns.Add("Item_Code", typeof(string));
                dtexisting.Columns.Add("Item_Description", typeof(string));
                dtexisting.Columns.Add("UOM", typeof(string));
                dtexisting.Columns.Add("GP_Qty", typeof(string));
                dtexisting.Columns.Add("Basic_Price", typeof(string));
                dtexisting.Columns.Add("Item_Spec", typeof(string));
                dtexisting.Columns.Add("Item_Code_Received", typeof(string));
                dtexisting.Columns.Add("Received_UOM", typeof(string));
                dtexisting.Columns.Add("ReceivedQty", typeof(string));
                dtexisting.Columns.Add("RejectedQty", typeof(string));
                dtexisting.Columns.Add("Unit_Wt", typeof(string));
                dtexisting.Columns.Add("Qty_RM_UOM", typeof(string));
                dtexisting.Columns.Add("JW_Unit_Rate", typeof(string));
                dtexisting.Columns.Add("Net_Rate", typeof(string));
                dtexisting.Columns.Add("Taxable_Value", typeof(string));
                dtexisting.Columns.Add("CGST_Per", typeof(decimal));
                dtexisting.Columns.Add("CGST_Amt", typeof(decimal));
                dtexisting.Columns.Add("SGST_Per", typeof(decimal));
                dtexisting.Columns.Add("SGST_Amt", typeof(decimal));
                dtexisting.Columns.Add("IGST_Per", typeof(decimal));
                dtexisting.Columns.Add("IGST_Amt", typeof(decimal));
                dtexisting.Columns.Add("Total_Amount", typeof(decimal));
                dtexisting.Columns.Add("Remarks", typeof(string));
                dtexisting.Columns.Add("GP_No", typeof(string));

                for (int i = 0; i < dgProducts.Rows.Count-1; i++)
                {
                    DataRow dr;
                    dr = dtexisting.NewRow();
                    for (int c = 0; c < dgProducts.ColumnCount; c++)
                    {
                        dr[c] = dgProducts.Rows[i].Cells[c].Value.ToString();
                    }
                   
                    dtexisting.Rows.Add(dr);

                }
                dtexisting.AcceptChanges();
            }


            if (ioneNet.MaterialManagement.Transactions.GP_Dialog.dtgetproducts.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                //for (int i = 0; i < dgProducts.ColumnCount; i++)
                //{
                //    int columnIndex = i;
                //    string columnName = dgProducts.Columns[columnIndex].Name;
                //    dt.Columns.Add(columnName, typeof(string));
                //}

                dt.Columns.Add("Item_Code", typeof(string));
                dt.Columns.Add("Item_Description", typeof(string));
                dt.Columns.Add("UOM", typeof(string));
                dt.Columns.Add("GP_Qty", typeof(string));
                dt.Columns.Add("Basic_Price", typeof(string));
                dt.Columns.Add("Item_Spec", typeof(string));
                dt.Columns.Add("Item_Code_Received", typeof(string));
                dt.Columns.Add("Received_UOM", typeof(string));
                dt.Columns.Add("ReceivedQty", typeof(string));
                dt.Columns.Add("RejectedQty", typeof(string));
                dt.Columns.Add("Unit_Wt", typeof(string));
                dt.Columns.Add("Qty_RM_UOM", typeof(string));
                dt.Columns.Add("JW_Unit_Rate", typeof(string));
                dt.Columns.Add("Net_Rate", typeof(string));
                dt.Columns.Add("Taxable_Value", typeof(string));
                dt.Columns.Add("CGST_Per", typeof(decimal));
                dt.Columns.Add("CGST_Amt", typeof(decimal));
                dt.Columns.Add("SGST_Per", typeof(decimal));
                dt.Columns.Add("SGST_Amt", typeof(decimal));
                dt.Columns.Add("IGST_Per", typeof(decimal));
                dt.Columns.Add("IGST_Amt", typeof(decimal));
                dt.Columns.Add("Total_Amount", typeof(decimal));
                dt.Columns.Add("Remarks", typeof(string));
                dt.Columns.Add("GP_No", typeof(string));
               

                //dt.Rows.Add();
                int j = dgProducts.Rows.Count - 1;
                for (int i = 0; i < ioneNet.MaterialManagement.Transactions.GP_Dialog.dtgetproducts.Rows.Count; i++)
                {
                   // string Item_Code = ioneNet.MaterialManagement.Transactions.frnPRDailog.dtgetproducts.Rows[i]["Prod_Code"].ToString();
                    //string PRNo = ioneNet.MaterialManagement.Transactions.frnPRDailog.dtgetproducts.Rows[i]["GP_No"].ToString();
                    //string PO_Qty = ioneNet.MaterialManagement.Transactions.frnPRDailog.dtgetproducts.Rows[i]["BalQty"].ToString();


                    string Item_Code = ioneNet.MaterialManagement.Transactions.GP_Dialog.dtgetproducts.Rows[i]["Prod_Code"].ToString();
                    string Item_Description = ioneNet.MaterialManagement.Transactions.GP_Dialog.dtgetproducts.Rows[i]["Product_Description"].ToString();                    
                    string UOM = ioneNet.MaterialManagement.Transactions.GP_Dialog.dtgetproducts.Rows[i]["UOM"].ToString();
                    string GP_Qty = ioneNet.MaterialManagement.Transactions.GP_Dialog.dtgetproducts.Rows[i]["BalQty"].ToString();
                    string Basic_Price = ioneNet.MaterialManagement.Transactions.GP_Dialog.dtgetproducts.Rows[i]["Basic_Price"].ToString();                   
                    string Item_Spec = "";
                    string Item_Code_Received = "";
                    string Received_UOM = "";
                    string ReceivedQty = "";
                    string RejectedQty = "";
                    string Unit_Wt = "";
                    string Qty_RM_UOM = "";
                    string JW_Unit_Rate = "";
                    string Net_Rate = "";
                    string Taxable_Value = "";
                    string CGST_Per = "0";
                    string CGST_Amt = "0";
                    string SGST_Per = "0";
                    string SGST_Amt = "0";
                    string IGST_Per = "0";
                    string IGST_Amt = "0";
                    string Total_Amount = "0";                   
                    string Remarks = "";
                    string GP_No = ioneNet.MaterialManagement.Transactions.GP_Dialog.dtgetproducts.Rows[i]["GP_No"].ToString();



                    j = j + 1;
                    dt.Rows.Add(Item_Code,
                                       Item_Description,
                                       UOM,
                                       GP_Qty,
                                       Basic_Price,
                                       Item_Spec ,
                                       Item_Code_Received,
                                       Received_UOM,
                                       ReceivedQty,
                                       RejectedQty,
                                       Unit_Wt,
                                       Qty_RM_UOM,
                                       JW_Unit_Rate,
                                       Net_Rate,
                                       Taxable_Value,
                                       CGST_Per,
                                       CGST_Amt,
                                       SGST_Per,
                                       SGST_Amt,
                                       IGST_Per,
                                       IGST_Amt,
                                       Total_Amount,
                                       Remarks,
                                       GP_No
                                       
                                       );

                }

                dtexisting = dtexisting.AsEnumerable().Union(dt.AsEnumerable()).CopyToDataTable();
                // dtexisting = dtexisting.AsEnumerable().Union(dtgetfinalprducts.AsEnumerable()).CopyToDataTable();
                dgProducts.DataSource = dtexisting;
                //dgProducts.DataSource = ioneNet.MaterialManagement.Transactions.frnPRDailog.dtgetproducts; //dtexisting;

            }


        }

        private void txtothercharges_Leave(object sender, EventArgs e)
        {
            othercalculation();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            try
            {
                //var d = (from data in db.SP_GetOrders_Sel(logIn.company, CmbSuplierName.Text,1,"") select data).ToList();

                //if (d.Count > 0)
                //{
                //    //dgProductsList.DataSource = d;
                //    sfDataGrid1.DataSource = d;
                //}

                //groupBox2.Visible = true;
                //txtSearch.Focus();
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
                //if (cmbPurchaseBasis.Text == "Direct")
                //{
                    if (columnName == "Item_Spec")
                    {
                        if (R1.Cells["Item_Spec"].Value != "")
                        {

                            var getProductName = (from s in db.Products
                                                  join u in db.UoM_Masters on s.Prod_Primary_UOM_Id equals u.UOM_ID
                                                  join g in db.Product_Groups on s.Prod_Group_Id equals g.ID
                                                  where s.Prod_Name == R1.Cells["Item_Spec"].Value.ToString() && s.Company_ID == logIn.company
                                                  select new { s.prod_ID, u.Uom_Descr, g.Prod_Group_Name,s.Prod_HSN_Code,s.Prod_Unit_Wt }).FirstOrDefault();

                            if (getProductName != null)
                            {
                            R1.Cells["Received_UOM"].Value = getProductName.Uom_Descr.ToString();
                            R1.Cells["Item_Code_Received"].Value = getProductName.prod_ID.ToString();
                             R1.Cells["Unit_Wt"].Value = getProductName.Prod_Unit_Wt.ToString();
                            //if (R1.Cells["Product_Descr"].Value == "" || R1.Cells["Product_Descr"].Value == DBNull.Value || R1.Cells["Product_Descr"].Value == null)
                            //{
                            //    R1.Cells["Product_Descr"].Value = getProductName.Product_Descr.ToString();
                            //}
                        }
                    }
                    }

                //}
                decimal UnitWt = 0;
                decimal qtyWt = 0;
                if (columnName == "ReceivedQty" || columnName == "RejectedQty")
                {
                    int Itemcode = Convert.ToInt32(R1.Cells["Item_Code"].Value);
                    if (Itemcode != null)
                    {
                        R1.Cells["JW_Unit_Rate"].Value = "0";
                        decimal b, c, d;
                        //c = dgProducts.Rows[dgProducts.CurrentRow.Index];
                        decimal POQty = (R1.Cells["GP_Qty"].Value == "" || R1.Cells["GP_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["GP_Qty"].Value);
                        decimal ReceivedQty = (R1.Cells["ReceivedQty"].Value == "" || R1.Cells["ReceivedQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["ReceivedQty"].Value);
                        UnitWt = (R1.Cells["Unit_Wt"].Value == "" || R1.Cells["Unit_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Unit_Wt"].Value);
                        qtyWt = UnitWt * ReceivedQty;
                        decimal RejectedQty = (R1.Cells["RejectedQty"].Value == "" || R1.Cells["RejectedQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["RejectedQty"].Value);

                        //if (cmbPurchaseBasis.Text != "Direct")
                        //{
                        if ((qtyWt+ RejectedQty) > POQty)
                            {
                                MessageBox.Show("Received Qty Cannot Be Greater Than GP Qty");
                                R1.Cells["ReceivedQty"].Value = "";
                                return;
                            }
                        //else
                        //{
                        //    R1.Cells["AcceptedQty"].Value = (ReceivedQty - RejectedQty).ToString("0.00");
                        //}
                        //}
                        //else
                        //{
                        //    R1.Cells["AcceptedQty"].Value = (ReceivedQty - RejectedQty).ToString("0.00");
                        //}
                        R1.Cells["Qty_RM_UOM"].Value = qtyWt;
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
                            //if (chkRCM.Checked == false)
                            //{
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
                            //}
                            //else
                            //{
                            //    R1.Cells["CGST_Per"].Value = "0.00";
                            //    R1.Cells["SGST_Per"].Value = "0.00";
                            //    R1.Cells["IGST_Per"].Value = "0.00";
                            //}
                        }
                    }

                }
                else
                { 
                    if (columnName == "JW_Unit_Rate")
                    {

                        //if (ReceivedQty > 0)
                        //{

                        // }
                    }
                }
                decimal Qty = (R1.Cells["ReceivedQty"].Value == "" || R1.Cells["ReceivedQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["ReceivedQty"].Value);

                decimal price = (R1.Cells["Basic_Price"].Value == "" || R1.Cells["Basic_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Basic_Price"].Value);
                decimal JwPrice = (R1.Cells["JW_Unit_Rate"].Value == "" || R1.Cells["JW_Unit_Rate"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["JW_Unit_Rate"].Value);

               // decimal NetRate = (R1.Cells["AcceptedQty"].Value == "" || R1.Cells["AcceptedQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["AcceptedQty"].Value);

                decimal Amt, NetRate, netAmt, gst, igst, totamt;
                UnitWt = (R1.Cells["Unit_Wt"].Value == "" || R1.Cells["Unit_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Unit_Wt"].Value);
                NetRate = Math.Round((price*UnitWt) + JwPrice,2);
                Amt = Qty * JwPrice;
                qtyWt = UnitWt * Qty;
                R1.Cells["Net_Rate"].Value = NetRate.ToString("0.00");
                R1.Cells["Taxable_Value"].Value = Amt.ToString("0.00");
                gst = (Convert.ToDecimal(R1.Cells["Taxable_Value"].Value) * Convert.ToDecimal(R1.Cells["CGST_Per"].Value)) / 100;
                //Qty_RM_UOM
               
                R1.Cells["CGST_Amt"].Value = gst;
                R1.Cells["SGST_Amt"].Value = gst;
                igst = (Convert.ToDecimal(R1.Cells["Taxable_Value"].Value) * Convert.ToDecimal(R1.Cells["IGST_Per"].Value)) / 100;
                R1.Cells["IGST_Amt"].Value = igst;

                totamt = Math.Round(Amt + gst + gst + igst,2);
                R1.Cells["Total_Amount"].Value = totamt;
                decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
                for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                {

                    x += (dgProducts.Rows[i].Cells["ReceivedQty"].Value == "" || dgProducts.Rows[i].Cells["ReceivedQty"].Value == null || dgProducts.Rows[i].Cells["ReceivedQty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["ReceivedQty"].Value);
                 //   y += (dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == "" || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == null || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value);
                   // q += (dgProducts.Rows[i].Cells["Disc_Amt"].Value == "" || dgProducts.Rows[i].Cells["Disc_Amt"].Value == null || dgProducts.Rows[i].Cells["Disc_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Amt"].Value);
                    v += (dgProducts.Rows[i].Cells["Taxable_Value"].Value == "" || dgProducts.Rows[i].Cells["Taxable_Value"].Value == null || dgProducts.Rows[i].Cells["Taxable_Value"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Taxable_Value"].Value);
                    cg += (dgProducts.Rows[i].Cells["CGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["CGST_Amt"].Value == null || dgProducts.Rows[i].Cells["CGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Amt"].Value);
                    sg += (dgProducts.Rows[i].Cells["SGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["SGST_Amt"].Value == null || dgProducts.Rows[i].Cells["SGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Amt"].Value);
                    ig += (dgProducts.Rows[i].Cells["IGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["IGST_Amt"].Value == null || dgProducts.Rows[i].Cells["IGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Amt"].Value);
                    totA += (dgProducts.Rows[i].Cells["Total_Amount"].Value == "" || dgProducts.Rows[i].Cells["Total_Amount"].Value == null || dgProducts.Rows[i].Cells["Total_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Total_Amount"].Value);

                }
                //DataGridViewRow row = (DataGridViewRow)dgProducts.Rows[0].Clone();
                //int aa = R1.Index;
                //if (aa == dgProducts.Rows.Count -2)
                //{
                //    DataGridViewRow row = (DataGridViewRow)dgProducts.Rows[0].Clone();
                //    dgProducts.Rows.Add(row);
                //}
               
                //dgProducts.Rows[dgProducts.Rows.Count - 1].Cells["AcceptedQty"].Value = x.ToString("0.00");
                //dgProducts.Rows[dgProducts.Rows.Count - 1].Cells["Amt_Before_Disc"].Value = y.ToString("0.00");
                //dgProducts.Rows[dgProducts.Rows.Count - 1].Cells["Disc_Amt"].Value = q.ToString("0.00");
                //dgProducts.Rows[dgProducts.Rows.Count - 1].Cells["Taxable_Value"].Value = v.ToString("0.00");

                //dgProducts.Rows[dgProducts.Rows.Count-1].Cells["CGST_Amt"].Value = cg.ToString("0.00");
                //dgProducts.Rows[dgProducts.Rows.Count - 1].Cells["SGST_Amt"].Value = sg.ToString("0.00");
                //dgProducts.Rows[dgProducts.Rows.Count - 1].Cells["IGST_Amt"].Value = ig.ToString("0.00");
                //dgProducts.Rows[dgProducts.Rows.Count - 1].Cells["Total_Amount"].Value = totA.ToString("0.00");

                txtTotalQty.Text = x.ToString("0.00");
                //txtSubTotal.Text = y.ToString("0.00");
                //txtTotDiscount.Text = q.ToString("0.00");
                txtTot_TaxableValue.Text = v.ToString("0.00");
                txtTot_CGST.Text = cg.ToString("0.00");
                txtTot_SGST.Text = sg.ToString("0.00");
                txtTot_IGST.Text = ig.ToString("0.00");
                //decimal fAmt = (txtFrieght.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtFrieght.Text);

                txtTot_OrderValue.Text = (totA).ToString("0.00");

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
                if ((from u in db.GRN_JW_Masters where u.Grn_NO == myString && u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID && u.isDeleted == false select u).Count() > 0)
                {
                    myString = txtSoNo.Text;
                    db.sp_GRNJW_Delete(myString, logIn.company);
                }
                else
                {
                    AutoincrementId();
                    myString = txtSoNo.Text;

                }
                    //if (frmGate.Modify.Contains(this.Text))
                    //{

               // dgProducts.Enabled = false;
                //var S = db.Sale_Order_Masters.Where(w => w.SO_NO == myString && w.Company_ID == logIn.company).FirstOrDefault();
               GRN_JW_Master S = new GRN_JW_Master();
                {
                    S.Grn_NO = myString;
                    S.Grn_Date = dpSODate.Value;                   
                    S.GP_No =(cmbPurchaseBasis.Text);
                    S.SupplierName = Convert.ToInt32(CmbSuplierName.SelectedValue.ToString());
                    if (cmbPurchaseAccount.Text != "" && cmbPurchaseAccount.Text != "NA")
                    {
                        S.Purchase_Account = Convert.ToInt32(cmbPurchaseAccount.SelectedValue.ToString());
                    }
                    else
                    {
                        S.Purchase_Account = 7210;
                    }

                    //S.Purchase_Account = Convert.ToInt32(cmbPurchaseAccount.SelectedValue.ToString());
                    S.TAX_Class = Convert.ToInt32(cmbTaxClass.SelectedValue.ToString());
                    S.Supp_GST_No = txtSupGSTNo.Text;                                     
                    S.Supplier_InvNo = txtSupplierInvNo.Text;
                    S.Supplier_InvDate = dtsupinvdate.Value;
                    


                    S.TotalQty = (txtTotalQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotalQty.Text);
                    S.SubTotal = (txtSubTotal.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtSubTotal.Text);
                    //S.Tot_Discount = (txtTotDiscount.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotDiscount.Text);
                   
                    //S.Freight = (txtfreight.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtfreight.Text);
                    //S.Other_Charges = (txtothercharges.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtothercharges.Text);
                    S.Tot_TaxableValue = (txtTot_TaxableValue.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTot_TaxableValue.Text);
                    // S.CustomerPONo = (txtvehicleno.Text == "") ? "" : txtvehicleno.Text;                            
                    // S. = (txtTot_TaxableValue.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTot_TaxableValue.Text);

                    //   S.Tot_TaxableValue = (txtTot_TaxableValue.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTot_TaxableValue.Text);
                    // S.Cust_GST_No = (txtSupGSTNo.Text == "") ? "" : txtSupGSTNo.Text;
                    S.Tot_CGST_Amnt = (txtTot_CGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_CGST.Text);
                    S.Tot_SGST_Amnt = (txtTot_SGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_SGST.Text);
                    S.Tot_IGST_Amnt = (txtTot_IGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_IGST.Text);
                    S.Total_Amount = (txtTotal_Amt.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTotal_Amt.Text);
                    //S.Tcs_Per = (txttcsper.Text == "" || txttcsper.Text == null) ? Convert.ToDecimal("0.00"): Convert.ToDecimal(txttcsper.Text);
                    //S.Tcs_Amount = (txttcsAmnt.Text == "" || txttcsAmnt.Text == null) ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txttcsAmnt.Text);
                    S.Tot_Ord_Value = (txtTot_OrderValue.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_OrderValue.Text);
                    //if (cmbWareHouse.SelectedValue == null)
                    //{
                    //    S.Warehouse_Code = 0;
                    //}
                    //else
                    //{
                    //    S.Warehouse_Code = Convert.ToInt32(cmbWareHouse.SelectedValue.ToString());

                    //}
                    S.isDeleted = false;
                    S.Remarks = (txttransportname.Text == "") ? "" : txttransportname.Text;
                    S.Vehicle_No = (txtvehicalnr.Text == "") ? "" : txtvehicalnr.Text;
                    //S.LrNo_LrDate = (txtlrnodate.Text == "") ? "" : txtlrnodate.Text;
                    //S.Transporter_Name = (cmbOtherTermsandNotes.Text == "") ? "" : cmbOtherTermsandNotes.Text;
                    //S.Other_Terms = (cmbOtherTermsandNotes.Text==""||cmbOtherTermsandNotes.Text==null)?"": cmbOtherTermsandNotes.Text;
                   // S.ConsigneeName = Convert.ToInt32(1);
                    S.Status = Convert.ToInt32(cmbStatus.SelectedValue.ToString());
                    //S.Sale_office  = Convert.ToInt32(cmbSaleOffice.SelectedValue.ToString());
                    //S.SaleExecutive = Convert.ToInt32(cmbSaleExecutive.SelectedValue.ToString());

                    //S.Insurance_Scope = Convert.ToInt32(cmbInsurance.SelectedValue.ToString());
                    //S.PaymentTerms = Convert.ToInt32(cmbPaymentTerms.SelectedValue.ToString());
                    //S.Trasnport_Scope = Convert.ToInt32(cmbTransport_Scope.SelectedValue.ToString());
                    //S.Transporter_Name = (cmbTransporter.Text == "") ? "" : cmbTransporter.Text;

                    //S.Repeat_Order = (chkRepeatOrder.Checked == true) ? true : false;
                    //S.Old_Ord_Ref = (txt.Text == "") ? "" : txtOtherTerms.Text;
                    //S.SEZ_Order = (chkSEZOrder.Checked == true) ? true : false;
                    //S.Customer_Contact = (txtCustomeContact.Text == "") ? "" : txtCustomeContact.Text;
                    //S.Other_Terms =   (txtOtherTerms.Text == "") ? "" : txtOtherTerms.Text;
                    //S.Spl_Instructions = (txtSplInstructions.Text == "") ? "" : txtSplInstructions.Text;
                    S.BU_ID = logIn.BU_ID;
                    S.Company_ID = logIn.company;
                    S.Created_By = lblCreatedBy.Text;                    
                    S.Modified_By = logIn.username + "-" + DateTime.Now;
                    db.GRN_JW_Masters.InsertOnSubmit(S);
                    db.SubmitChanges();
                }
                        //db.Transaction = transaction;
                for (int i = 0; i < dgProducts.RowCount - 1; i++)
                {
                    GRN_JW_Child SC = new GRN_JW_Child();
                    var d1 = (from a in db.GRN_JW_Masters where a.Grn_NO == myString && a.Company_ID == logIn.company && a.BU_ID == logIn.BU_ID && a.isDeleted == false select new { a.Id }).ToList();
                    SC.GRN_Master_ID = d1[0].Id;
                    SC.Grn_NO = myString;
                    SC.Prod_Code_Sent = (dgProducts.Rows[i].Cells["Item_Code"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value);                    
                    SC.Product_Description_Sent = (dgProducts.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value).ToString();
                    SC.Prod_Code_Received = (dgProducts.Rows[i].Cells["Item_Code_Received"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code_Received"].Value);
                    SC.Product_Description_Received = (dgProducts.Rows[i].Cells["Item_Spec"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Spec"].Value).ToString();
                    SC.UOM_Received = (dgProducts.Rows[i].Cells["Received_UOM"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["Received_UOM"].Value.ToString();
                    SC.Uom = (dgProducts.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["uom"].Value.ToString();
                    SC.GP_Qty = (dgProducts.Rows[i].Cells["GP_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["GP_Qty"].Value);
                    SC.ReceivedQty = (dgProducts.Rows[i].Cells["ReceivedQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["ReceivedQty"].Value);
                    SC.RejectedQty = (dgProducts.Rows[i].Cells["RejectedQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["RejectedQty"].Value);
                   // SC.AcceptedQty = (dgProducts.Rows[i].Cells["AcceptedQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["AcceptedQty"].Value);
                    SC.RM_Price = (dgProducts.Rows[i].Cells["Basic_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Basic_Price"].Value);
                    SC.Unit_Wt = (dgProducts.Rows[i].Cells["Unit_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Unit_Wt"].Value);
                    SC.Qty_Kgs = (dgProducts.Rows[i].Cells["Qty_RM_UOM"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty_RM_UOM"].Value);
                    SC.JW_Rate = (dgProducts.Rows[i].Cells["JW_Unit_Rate"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["JW_Unit_Rate"].Value);
                    SC.Net_Rate = (dgProducts.Rows[i].Cells["Net_Rate"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Net_Rate"].Value);
                    SC.Taxable_Value = (dgProducts.Rows[i].Cells["Taxable_Value"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Taxable_Value"].Value);
                    SC.CGST_Per = (dgProducts.Rows[i].Cells["CGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Per"].Value);
                    SC.SGST_Per = (dgProducts.Rows[i].Cells["SGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Per"].Value);
                    SC.IGST_Per = (dgProducts.Rows[i].Cells["IGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Per"].Value);
                    SC.CGST_Amnt = (dgProducts.Rows[i].Cells["CGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Amt"].Value);
                    SC.SGST_Amnt = (dgProducts.Rows[i].Cells["SGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Amt"].Value);
                    SC.IGST_Amnt = (dgProducts.Rows[i].Cells["IGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Amt"].Value);
                    SC.Net_Amount = (dgProducts.Rows[i].Cells["Total_Amount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Total_Amount"].Value);
                    SC.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value).ToString();
                    SC.GP_No = (dgProducts.Rows[i].Cells["GP_No"].Value == null) ? "" : (dgProducts.Rows[i].Cells["GP_No"].Value).ToString();
                    SC.ProdSno = i + 1;
                    SC.Company_ID = logIn.company;
                    db.GRN_JW_Childs.InsertOnSubmit(SC);
                }
                db.SubmitChanges();
                //transaction.Commit();               
                //Update the status of roll data
                //SqlCommand cmd = new SqlCommand();
                //cmd.CommandText = "Update Bloom_Roll_Wise_Receipts set status = 'Closed' where [Grn_ID]=@param1 and Company_ID =@compName";
                //cmd.Parameters.AddWithValue("@param1", txtSoNo.Text);
                //cmd.Parameters.AddWithValue("@CompName", logIn.company);
                //cmd.Connection = con;
                //con.Open();
                //cmd.ExecuteNonQuery();
                //con.Close();
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
                txtSoNo.Text =GRNJWList.SO_No;
                String myString = "";
                int grnID=0;
                myString = txtSoNo.Text;
                var da = (from obj in db.GRN_JW_Masters
                          where obj.Grn_NO == txtSoNo.Text && obj.Company_ID == logIn.company && obj.isDeleted == false
                          select obj).ToList();

                if (da.Count > 0)
                {
                    grnID = da[0].Id;
                    txtSoNo.Text = da[0].Grn_NO.ToString();
                    dpSODate.Text = da[0].Grn_Date.ToString();
                    //bindCustomer();
                    CmbSuplierName.SelectedValue = da[0].SupplierName;
                    cmbPurchaseBasis.Text = da[0].GP_No.ToString();
                   
                   
                    txtSupplierInvNo.Text = da[0].Supplier_InvNo;
                    dtsupinvdate.Text = da[0].Supplier_InvDate.ToString();
                    
                    txtSupGSTNo.Text = da[0].Supp_GST_No;                  
                   
                    txtTotalQty.Text = da[0].TotalQty.ToString();
                    txtSubTotal.Text = da[0].SubTotal.ToString();
                   // txtTotDiscount.Text = da[0].Tot_Discount.ToString();
                    txtTot_TaxableValue.Text = da[0].Tot_TaxableValue.ToString();
                    txtTot_CGST.Text = da[0].Tot_CGST_Amnt.ToString();
                    txtTot_SGST.Text = da[0].Tot_SGST_Amnt.ToString();
                    txtTot_IGST.Text = da[0].Tot_IGST_Amnt.ToString();
                    txtTot_OrderValue.Text = da[0].Tot_Ord_Value.ToString();
                    //txttcsper.Text = da[0].Tcs_Per.ToString();
                    //txttcsAmnt.Text = da[0].Tcs_Amount.ToString();
                    //txtfreight.Text = Convert.ToString(da[0].Freight);
                    //txtothercharges.Text = Convert.ToString(da[0].Other_Charges);
                    //txttcsAmnt.Text = Convert.ToString(da[0].Tcs_Amount);
                    //txttcsper.Text = Convert.ToString(da[0].Tcs_Per);
                    txtTotal_Amt.Text = da[0].Total_Amount.ToString();
                    //cmbWareHouse.Text = da[0].Warehouse_Code.ToString();
                    txtvehicalnr.Text = da[0].Vehicle_No;
                    //txtlrnodate.Text = da[0].LrNo_LrDate;
                    //cmbOtherTermsandNotes.Text = da[0].Transporter_Name;
                  //  txttransportname.Text = da[0].Other_Terms;
                    cmbTaxClass.SelectedValue = da[0].TAX_Class;
                    if (da[0].Purchase_Account != null)
                    {
                        cmbPurchaseAccount.SelectedValue = da[0].Purchase_Account;
                    }
                    txttransportname.Text = da[0].Remarks;
                    cmbStatus.SelectedValue = da[0].Status;      
                    lblCreatedBy.Text = da[0].Created_By;
                    lblModified.Text = da[0].Modified_By;                    
                }


                var dm1 = (from s in db.GRN_JW_Childs
                           where s.GRN_Master_ID == grnID && s.Company_ID == logIn.company


                           select new

                           {
                               Item_Code = s.Prod_Code_Sent,
                               Item_Description = s.Product_Description_Sent,
                               UOM = s.Uom,
                               s.GP_Qty,
                               Basic_Price = s.RM_Price,
                               Item_Spec = s.Product_Description_Received,
                               Item_Code_Received = s.Prod_Code_Received,
                               Received_UOM = s.UOM_Received,                               
                               s.ReceivedQty,
                               s.RejectedQty,
                               Unit_Wt= s.Unit_Wt,
                               Qty_RM_UOM= s.Qty_Kgs,                               
                               JW_Unit_Rate = s.JW_Rate,
                               s.Net_Rate,                              
                               s.Taxable_Value,
                               s.CGST_Per,
                               CGST_Amt = s.CGST_Amnt,
                               s.SGST_Per,
                               SGST_Amt = s.SGST_Amnt,
                               s.IGST_Per,
                               IGST_Amt = s.IGST_Amnt,
                               Total_Amount = s.Net_Amount,                            
                               s.Remarks,
                               s.GP_No
                               
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
               // txtTotDiscount.Text = q.ToString("0.00");
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
