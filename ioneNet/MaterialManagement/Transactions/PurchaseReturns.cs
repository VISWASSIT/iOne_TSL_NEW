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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Diagnostics;
namespace ioneNet.MaterialManagement.Transactions
{
    public partial class PurchaseReturns : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();
        public static string SONo,ItemCode,RecQty,Suppname;
        private Database crDatabase;
        private Tables crTables;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        private string path;


        public PurchaseReturns()
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
                    txtSupplierInvNo.Focus();
                    return;
                }
                else if (txtInwardNo.Text == string.Empty)
                {
                    MessageBox.Show("Enter GRN No,");
                    txtInwardNo.Focus();
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

                var result = db.Sp_autoincrement_PurchaseReturns (logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date,logIn.BU_ID);
                txtSoNo.Text = result.FirstOrDefault().Vch_No;
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
                        decimal fAmt = (txtfreight.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtfreight.Text);
                       decimal OthAmt = (txtothercharges.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtothercharges.Text);

                        txtTot_TaxableValue.Text = (v).ToString(".00");

                        txtTot_CGST.Text = cg.ToString(".00");
                        txtTot_SGST.Text = sg.ToString(".00");
                        txtTot_IGST.Text = ig.ToString(".00");
                        decimal AmtForTCs = (fAmt + OthAmt + v + cg + sg + ig);
                        decimal tcsPer = (txttcsper.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txttcsper.Text);

                        decimal TcsAmt = AmtForTCs * tcsPer / 100;
                        txttcsAmnt.Text = TcsAmt.ToString(".00");
                        decimal rndAmt = (textBox1.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(textBox1.Text);
                        txtTot_OrderValue.Text = (totA + TcsAmt + fAmt + OthAmt + rndAmt).ToString(".00");


                    }
                }
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
                if (txttcsAmnt.Text != "" && txttcsAmnt.Text != "0.00")
                {
                    decimal per = Convert.ToDecimal(txttcsper.Text);
                    decimal amnt = Convert.ToDecimal(txttcsAmnt.Text);
                    decimal percentamnt = (amnt * per) / 100;
                    //decimal totalorder = Convert.ToInt32(txtTot_OrderValue.Text);
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


                if (txtfreight.Text == null || txtfreight.Text == "" || txtfreight.Text == "0.00")
                {
                    Freight = 0;
                    Freightmnt = 0;
                    pervalue = 0;
                    Finalfrght = 0;
                }
                else
                {
                    Freight = Convert.ToDecimal(txtfreight.Text);
                    pervalue = Convert.ToInt32(j);
                    Freightmnt = Math.Round((Freight * pervalue) / 100);
                    Finalfrght = Freightmnt / 2;
                }


                if (txtothercharges.Text == null || txtothercharges.Text == "" || txtothercharges.Text == "0.00")
                {
                    othercharges = 0;
                    otherchargesvalue = 0;
                    totalotherchargesvalue = 0;
                }
                else
                {
                    othercharges = Convert.ToDecimal(txtothercharges.Text);
                    otherper = Convert.ToInt32(j);
                    otheramnt = Math.Round((othercharges * otherper) / 100);

                    Finalother = otheramnt / 2;
                    otherchargesvalue = otheramnt;
                    totalotherchargesvalue = othercharges + otherchargesvalue;
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


                decimal TotalOrderValue = Convert.ToDecimal(txtTot_TaxableValue.Text)+ FinalCGSTAMount + FinalSGSTAMount + FinalIGSTAMount + Freight + othercharges + +pervalue + otherper + igstamnt;
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

        private void txtInwardNo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtInwardNo.Text!="")
                {

                    int grnid = 0;      
                    String myString = "";
                    myString = txtInwardNo.Text;
                    var da = (from obj in db.GoodsReceiptNote_Masters
                              where obj.Grn_NO == txtInwardNo.Text && obj.Company_ID == logIn.company && obj.BU_ID == logIn.BU_ID
                              select obj).ToList();

                    if (da.Count > 0)
                    {


                        //bindCustomer();


                        grnid = da[0].Id;
                        txtSupplierInvNo.Text = da[0].Supplier_InvNo;
                        dtsupinvdate.Text = da[0].Supplier_InvDate.ToString();
                        CmbSuplierName.SelectedValue = da[0].SupplierName;
                        txtSupGSTNo.Text = da[0].Supp_GST_No;                       
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
                       
                        if (da[0].Purchase_Account != null)
                        {
                            cmbPurchaseAccount.SelectedValue = da[0].Purchase_Account;
                        }
                      
                        lblCreatedBy.Text = da[0].Created_By;
                        lblModified.Text = da[0].Modified_By;
                    }


                    var dm1 = (from s in db.GoodsReceiptNote_Childs
                               where s.GRN_Master_ID == grnid && s.Company_ID == logIn.company


                               select new

                               {
                                   Item_Code = s.Prod_Code,
                                   Item_Description = s.Product_Description,
                                   Item_Spec = s.Prod_Spec,
                                   Item_Grade = s.Prod_Grade,
                                   HSN_Code = s.HSN_Code,
                                   UOM = s.Uom,
                                   GRN_Qty = s.ReceivedQty,
                                   Returned_Qty = s.RejectedQty,                                 
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                   
                    db.sp_PurchaseReturn_Delete(txtSoNo.Text, logIn.company,logIn.BU_ID);

                    MessageBox.Show("Selected Voucher Deleted Successfully");
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                //int i = sfDataGrid1.CurrentCell.RowIndex;
                //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                //var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                //var mappingName = sfDataGrid1.Columns[1].MappingName;
                //var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                // string path1 = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.pdf");
                SqlCommand cmd1 = con.CreateCommand();
                SqlCommand cmd2 = con.CreateCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                //int i = sfDataGrid1.CurrentRow.Index;
                //SO_No = txtVchNo.Text;


                path = Path.Combine(Directory.GetCurrentDirectory(), "DebitNote.pdf");
                //string path = @"D:\Invoice.pdf";
                FileInfo fi1 = new FileInfo(path);


                //if (fi1.Exists)
                //{
                //    fi1.Delete();
                //}
                //SqlCommand cmd = new SqlCommand("sp_Rpt_CreditNote", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Invoice_No", txtVchNo.Text);
                //cmd.Parameters.AddWithValue("@Creation_Company", logIn.company);
                //SqlDataAdapter da = new SqlDataAdapter(cmd);

                //DataTable Dt = new DataTable();

                //da.SelectCommand = cmd;
                //da.Fill(Dt);
                //if (Dt.Rows.Count > 0)
                //{
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                rep = new ioneNet.OrderManagement.Transactions.Debite_Note_Vpack();

                


                crConnectionInfo.ServerName = frmMain.ServerIP;
                crConnectionInfo.DatabaseName = frmMain.Database;
                crConnectionInfo.UserID = frmMain.DBUserID;
                crConnectionInfo.Password = frmMain.Password;


                crDatabase = rep.Database;
                crTables = crDatabase.Tables;
                //Loop through all tables in the report and apply the connection information for each table.
                for (int k = 0; k < crTables.Count; k++)
                {
                    //  crTable = crTables[i];
                    crTableLogOnInfo = crTables[k].LogOnInfo;
                    crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                    crTables[k].ApplyLogOnInfo(crTableLogOnInfo);

                }

                rep.RecordSelectionFormula = "{ Purchase_Return_Master.Vch_No} = '" + txtSoNo.Text + "' and { Purchase_Return_Master.Company_ID} = " + logIn.company + " and { Purchase_Return_Master.BU_ID} = " + logIn.BU_ID + "";


                //rep.SetParameterValue(0, txtVchNo.Text);
                //    rep.SetParameterValue(1, logIn.company);
                //    rep.SetDataSource(Dt);
                ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                //   // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");


                viewer.crystalReportViewer1.ReportSource = rep;
                ////   viewer.Show();

                viewer.crystalReportViewer1.Refresh();

                rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);



                con.Close();
                Process.Start(path);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                MaterialManagement.Transactions.PurchaseReturnsList obj = new MaterialManagement.Transactions.PurchaseReturnsList();

                if (obj.ShowDialog() == DialogResult.OK)
                {
                    txtSoNo.Text = MaterialManagement.Transactions.PurchaseReturnsList.voucherNo;

                    if (!string.IsNullOrEmpty(txtSoNo.Text))
                    {
                        bindedit();
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

                if (columnName == "Returned_Qty")
                {
                    int Itemcode = Convert.ToInt32(R1.Cells["Item_Code"].Value);
                    if (Itemcode != null)
                    {
                        decimal b, c, d;
                        //c = dgProducts.Rows[dgProducts.CurrentRow.Index];
                        decimal POQty = (R1.Cells["GRN_Qty"].Value == "" || R1.Cells["GRN_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["GRN_Qty"].Value);
                        decimal ReceivedQty = (R1.Cells["Returned_Qty"].Value == "" || R1.Cells["Returned_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Returned_Qty"].Value);
                        if (ReceivedQty > POQty)
                        {
                            MessageBox.Show("Returned Qty Cannot Be Greater Than GRN Qty");
                            return;
                        }                      
                       
                        int taxRate = 0;                    
                        
                    }

                }                
                decimal price = (R1.Cells["Basic_Price"].Value == "" || R1.Cells["Basic_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Basic_Price"].Value);
                decimal DiscPer = (R1.Cells["Disc_Per"].Value == "" || R1.Cells["Disc_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Disc_Per"].Value);

                decimal AcceptedQty = (R1.Cells["Returned_Qty"].Value == "" || R1.Cells["Returned_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Returned_Qty"].Value);

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

                    x += (dgProducts.Rows[i].Cells["Returned_Qty"].Value == "" || dgProducts.Rows[i].Cells["Returned_Qty"].Value == null || dgProducts.Rows[i].Cells["Returned_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Returned_Qty"].Value);
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
                if ((from u in db.Purchase_Return_Masters where u.Vch_NO == myString && u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID  select u).Count() > 0)
                {
                    myString = txtSoNo.Text;
                    db.sp_PurchaseReturn_Delete(myString, logIn.company,logIn.BU_ID);
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
               Purchase_Return_Master S = new Purchase_Return_Master();
                {
                    S.Vch_NO = myString;
                    S.Vch_Date = dpSODate.Value;
                    S.Grn_NO = txtInwardNo.Text;                   
                    S.SupplierName = Convert.ToInt32(CmbSuplierName.SelectedValue.ToString());
                    S.Purchase_Account = Convert.ToInt32(cmbPurchaseAccount.SelectedValue.ToString());                    
                    S.Supp_GST_No = txtSupGSTNo.Text;
                   
                    S.Supplier_InvNo = txtSupplierInvNo.Text;
                    S.Supplier_InvDate = dtsupinvdate.Value;                   


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
                   
                    S.isDeleted = false;
                  
                    S.Vehicle_No = (txtvehicalnr.Text == "") ? "" : txtvehicalnr.Text;
                    S.Other_Terms = (txtRemarks.Text==""||txtRemarks.Text==null)?"": txtRemarks.Text;
                   // S.ConsigneeName = Convert.ToInt32(1);
                  
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
                    S.Company_ID = logIn.company;
                    S.Created_By = lblCreatedBy.Text;                    
                    S.Modified_By = logIn.username + "-" + DateTime.Now;
                    S.BU_ID = logIn.BU_ID;
                    db.Purchase_Return_Masters.InsertOnSubmit(S);
                    db.SubmitChanges();
                }
                        //db.Transaction = transaction;
                for (int i = 0; i < dgProducts.RowCount - 1; i++)
                {
                    Purchase_Returns_Child SC = new Purchase_Returns_Child();
                    var d1 = (from a in db.Purchase_Return_Masters where a.Vch_NO == myString && a.Company_ID == logIn.company && a.BU_ID ==logIn.BU_ID select new { a.Id }).ToList();
                    SC.PRM_Master_ID = d1[0].Id;
                    SC.Vch_NO = myString;
                    SC.Prod_Code = (dgProducts.Rows[i].Cells["Item_Code"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value);                    
                    SC.Product_Description = (dgProducts.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value).ToString();
                    SC.Prod_Spec = (dgProducts.Rows[i].Cells["Item_spec"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_spec"].Value).ToString();
                    SC.Prod_Grade = (dgProducts.Rows[i].Cells["Item_Grade"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value).ToString();
                    SC.HSN_Code = (dgProducts.Rows[i].Cells["HSN_Code"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["HSN_Code"].Value.ToString();
                    SC.Uom = (dgProducts.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["uom"].Value.ToString();
                    SC.GRN_Qty = (dgProducts.Rows[i].Cells["GRN_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["GRN_Qty"].Value);
                    SC.Returned_Qty = (dgProducts.Rows[i].Cells["Returned_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Returned_Qty"].Value);
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
                    SC.ProdSno = i + 1;
                    SC.Company_ID = logIn.company;
                    db.Purchase_Returns_Childs.InsertOnSubmit(SC);
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
                
                String myString = "";
                int pID = 0;
                myString = txtSoNo.Text;
                var da = (from obj in db.Purchase_Return_Masters
                          where obj.Vch_NO == txtSoNo.Text && obj.Company_ID == logIn.company && obj.BU_ID == logIn.BU_ID
                          select obj).ToList();

                if (da.Count > 0)
                {
                    pID = da[0].Id;
                    txtSoNo.Text = da[0].Vch_NO.ToString();
                    dpSODate.Text = da[0].Vch_Date.ToString();
                    //bindCustomer();
                    txtInwardNo.Text = da[0].Grn_NO;                   
                    txtSupplierInvNo.Text = da[0].Supplier_InvNo;
                    dtsupinvdate.Text = da[0].Supplier_InvDate.ToString();
                    CmbSuplierName.SelectedValue = da[0].SupplierName;
                    txtSupGSTNo.Text = da[0].Supp_GST_No;                
                   
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
                 
                    txtvehicalnr.Text = da[0].Vehicle_No;
                   
                    txtRemarks.Text = da[0].Other_Terms;
                  
                  
                    if (da[0].Purchase_Account != null)
                    {
                        cmbPurchaseAccount.SelectedValue = da[0].Purchase_Account;
                    }
                    
                    lblCreatedBy.Text = da[0].Created_By;
                    lblModified.Text = da[0].Modified_By;                    
                }


                var dm1 = (from s in db.Purchase_Returns_Childs
                           where s.PRM_Master_ID == pID && s.Company_ID == logIn.company


                           select new

                           {
                               Item_Code = s.Prod_Code,
                               Item_Description = s.Product_Description,
                               Item_Spec = s.Prod_Spec,
                               Item_Grade = s.Prod_Grade,
                               HSN_Code = s.HSN_Code,
                               UOM = s.Uom,                              
                               s.GRN_Qty,
                               s.Returned_Qty,                              
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
