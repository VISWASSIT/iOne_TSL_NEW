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
using Syncfusion.WinForms.DataGrid.Enums;
using Ione_DAL;
using System.Linq.Expressions;
using Syncfusion.Windows.Forms.Tools.Win32API;
using Syncfusion.Windows.Forms.Tools;

namespace ioneNet.MaterialManagement.Transactions
{
    public partial class GoodsReceiptNote : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();
        public static string SONo,ItemCode,RecQty,Suppname, HeatNo,GRN_Basis;
        int taxRate = 0;
        decimal cgstPer, sgstPer, igstPer;
        public static string transname,transno; 
        public GoodsReceiptNote()
        {
            InitializeComponent();
        }

        private void frmNewOrder_Load(object sender, EventArgs e)
        {

            //Reindexing voucher numbers
            DateTime dtt = dpSODate.Value;
            
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

            //if (ioneNet.frmMenuBoard.finModule == true)
            //{
                //cmbPurchaseAccount.Visible = true;
                //label27.Visible = true;

            //}
            //else
            //{
            //    cmbPurchaseAccount.Visible = false;
            //    label27.Visible = false;
            //}
              bindCustomer();
            //bindConsignee();
            bindDroupDown_Lookup();

            


            if (GoodsReceiptNoteList.var == "0")
            {
                if (GoodsReceiptNoteList.editMode == true)
                {
                    bindedit();
                }
            }
            else if (GoodsReceiptNoteList.var == "2")
            {
                if (GoodsReceiptNoteList.editMode == false)
                {
                    bindedit();
                    btnClear.Enabled = false;
                    btnSave.Enabled = false;
                }
            }
            else
            {
            //AutoincrementId();
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
                if (chkConversion.Checked  == true)
                {
                   if(cmbConvPartyName.Text == string.Empty)
                    {
                        MessageBox.Show("Converty Party Name To Whome This Material Received For!!");
                        cmbConvPartyName.Focus();
                        return;
                    }
                }
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
                    MessageBox.Show("Select Purchase Basis");
                    cmbPurchaseBasis.Focus();
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
                    MessageBox.Show("Please Select GRN Type");
                    cmbTaxClass.Focus();
                    return;
                }
                if (dgProducts.Rows[0].Cells["Item_Code"].Value == null)
                {
                    MessageBox.Show("Atleast One Record To Be Entered To Proceed", "GRN", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    //Save();
                    SaveNew_Sql_proc();
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
                if (cmbPurchaseBasis.Text != "For Job Work")
                {
                    var Buyerblind = (from m in db.Supplier_informations
                                      join a in db.Attributes_Datas on m.Supplier_Category equals a.ID
                                      where m.Company_ID == logIn.company && m.Status == 1
                                      //&& a.Descr != "Customer"
                                      select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                    if (Buyerblind.Count > 0)
                    {
                        CmbSuplierName.DataSource = Buyerblind;
                        CmbSuplierName.ValueMember = "ID";
                        CmbSuplierName.DisplayMember = "Supplier_Name";
                    }
                    CmbSuplierName.SelectedIndex = -1;
                }
                else if (cmbPurchaseBasis.Text == "For Job Work")
                {
                    var Buyerblind = (from m in db.Supplier_informations
                                      join a in db.Attributes_Datas on m.Supplier_Category equals a.ID
                                      where m.Company_ID == logIn.company && m.Status == 1
                                      //&& a.Descr != "Customer"
                                      select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                    if (Buyerblind.Count > 0)
                    {
                        CmbSuplierName.DataSource = Buyerblind;
                        CmbSuplierName.ValueMember = "ID";
                        CmbSuplierName.DisplayMember = "Supplier_Name";  
                    }
                    CmbSuplierName.SelectedIndex = -1;
                }


                var Buyerbind = (from m in db.Supplier_informations
                                  join a in db.Attributes_Datas on m.Supplier_Category equals a.ID
                                  where m.Company_ID == logIn.company && m.Status == 1
                                  
                                  select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                if (Buyerbind.Count > 0)
                {
                    cmbPurchaseAccount.DataSource = Buyerbind;
                    cmbPurchaseAccount.ValueMember = "ID";
                    cmbPurchaseAccount.DisplayMember = "Supplier_Name";
                }
                cmbPurchaseAccount.SelectedIndex = -1;


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



                //Purchase Basis
                var PurBasis = (from m in db.Attributes_Datas where m.Head_Name == "Purchase Basis GRN" && (m.Company_ID == logIn.company || m.Company_ID == 0) select new { m.ID, m.Descr }).Distinct().ToList();
                if (PurBasis.Count > 0)
                {
                    cmbPurchaseBasis.DataSource = PurBasis;
                    cmbPurchaseBasis.ValueMember = "ID";
                    cmbPurchaseBasis.DisplayMember = "Descr";
                }

                //Status

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


                
                    var TrasnportBind = (from m in db.Supplier_informations
                                      join a in db.Attributes_Datas on m.Supplier_Category equals a.ID
                                      where m.Company_ID == logIn.company && m.Status == 1
                                      && a.Descr == "Transporter"
                                      select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                    if (TrasnportBind.Count > 0)
                    {
                        cmbOtherTermsandNotes.DataSource = TrasnportBind;
                        cmbOtherTermsandNotes.ValueMember = "ID";
                        cmbOtherTermsandNotes.DisplayMember = "Supplier_Name";
                    }
                        cmbOtherTermsandNotes.SelectedIndex = -1;



               




                //Warehouse
                var SO = (from m in db.Attributes_Datas where m.Head_Name == "Ware House" select new { m.ID, m.Descr }).Distinct().ToList();
                if (SO.Count > 0)
                {
                    cmbWareHouse.DataSource = SO;
                    cmbWareHouse.ValueMember = "ID";
                    cmbWareHouse.DisplayMember = "Descr";
                }

                //GRNN Type Basis
                var grntype = (from m in db.Attributes_Datas 
                               join p in db.Purchase_Groups_Accesses on m.Descr equals p.grn_type
                               where m.Head_Name == "GRN Type" && (m.Company_ID == logIn.company || m.Company_ID == 0) select new { m.ID, m.Descr }).Distinct().ToList();
                if (PurBasis.Count > 0)
                {
                    cmbTaxClass.DataSource = grntype;
                    cmbTaxClass.ValueMember = "ID";
                    cmbTaxClass.DisplayMember = "Descr";
                    cmbTaxClass.SelectedIndex = -1;
                }

                var pconv = (from m in db.Attributes_Datas where m.Head_Name == "Conversion_Party" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pconv.Count > 0)
                {
                    cmbConvPartyName.DataSource = pconv;
                    cmbConvPartyName.ValueMember = "ID";
                    cmbConvPartyName.DisplayMember = "Descr";
                    cmbConvPartyName.SelectedIndex = -1;
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
                string gs = "";
                if(cmbTaxClass.Text == "Raw Material")
                {
                    gs = "RM";
                }
                else
                if(cmbTaxClass.Text == "Finished - Trade")
                {

                    gs = "FG";
                }
                else
                {
                    gs = "";
                }
                var result = db.Sp_autoincrement_GRN_TSL(logIn.company, logIn.BU_ID, logIn.fy_Start_Date, logIn.fy_End_Date,cmbTaxClass.Text,gs);
                txtSoNo.Text = result.FirstOrDefault().GRN_NO;
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
                if(cmbPurchaseBasis.Text == "Direct")
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

                    if (tb3 != null && columnName == "Make /Model / Grade")
                    {
                        tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                        tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                        addItems(DataColl);
                        tb3.AutoCompleteCustomSource = DataColl;
                    }
                }
                //else
                //{
                //    MessageBox.Show("Products Cannot Be Added When Purchase Basis is With PR/PO");
                //    return;
                //}
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
                if (CmbSuplierName.Text != "")
                {
                    int i = CmbSuplierName.FindString(CmbSuplierName.Text);
                    if (i >= 0)
                    {
                        //var scode = (from m in db.Supplier_informations where m.Company_ID == logIn.company && m.ID == i select new { m.ID, m.Supplier_Name, m.GSTIN_NO, m.StateCode }).Distinct().ToList();
                        //if (scode.Count == 0)
                        //{
                        //    MessageBox.Show("Invalid Supplier Name");
                        //    CmbSuplierName.Focus();
                        //}
                        //else
                        //{
                        //    txtSupGSTNo.Text = scode[0].GSTIN_NO;
                        //    txtStateCode.Text = scode[0].StateCode;
                        //}
                    }
                    else
                    {
                        MessageBox.Show("Invalid Supplier Selected");
                        CmbSuplierName.Focus();
                    }
                }
               
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

                if (e.KeyCode == Keys.F6 )
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
                                    SONo = txtSoNo.Text;
                                    ItemCode = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                                    SqlCommand cmd1 = new SqlCommand("delete  from [GoodsReceiptNote_Child] where [Prod_Code] =@ProdID and [GRN_No] = @pono", con);
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
                        Calcaulation();

                    }
                }
                DataTable dtexisting = new DataTable();
                if (e.KeyCode == Keys.F2 )
                {
                    ioneNet.Masters.ProdSearch form = new ioneNet.Masters.ProdSearch();
                    ioneNet.Masters.ProdSearch.frmName = "POrder";
                    form.ShowDialog();

                    if (dgProducts.Rows.Count > 1)
                    {
                        dtexisting.Rows.Clear();
                        dtexisting.Columns.Clear();

                        dtexisting.Columns.Add("S_No", typeof(string));
                        dtexisting.Columns.Add("Item_Code", typeof(string));
                        dtexisting.Columns.Add("Prod_Code", typeof(string));
                        dtexisting.Columns.Add("Item_Description", typeof(string));
                        dtexisting.Columns.Add("Item_Spec", typeof(string));
                        dtexisting.Columns.Add("Item_Grade", typeof(string));
                        dtexisting.Columns.Add("HSN_Code", typeof(string));
                        dtexisting.Columns.Add("UOM", typeof(string));
                        dtexisting.Columns.Add("PO_Qty", typeof(string));
                        dtexisting.Columns.Add("Inv_Qty", typeof(string));
                        dtexisting.Columns.Add("Tole_Qty", typeof(string));
                        dtexisting.Columns.Add("ReceivedQty", typeof(string));
                        dtexisting.Columns.Add("RejectedQty", typeof(string));
                        dtexisting.Columns.Add("AcceptedQty", typeof(string));
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
                        dtexisting.Columns.Add("Int_Batch_No", typeof(string));
                        dtexisting.Columns.Add("Remarks", typeof(string));
                        


                        for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                        {
                            DataRow dr;
                            dr = dtexisting.NewRow();
                            dr["S_No"] = dgProducts.Rows[i].Cells["S_No"].Value.ToString();
                            dr["Item_Code"] = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                            dr["Prod_Code"] = dgProducts.Rows[i].Cells["Prod_Code"].Value.ToString();
                            dr["Item_Description"] = dgProducts.Rows[i].Cells["Item_Description"].Value.ToString();
                            dr["Item_Spec"] = (dgProducts.Rows[i].Cells["Item_Spec"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Item_Spec"].Value);
                            dr["Item_Grade"] = (dgProducts.Rows[i].Cells["Item_Grade"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value);
                            dr["HSN_Code"] = dgProducts.Rows[i].Cells["HSN_Code"].Value.ToString();
                            dr["UOM"] = dgProducts.Rows[i].Cells["UOM"].Value.ToString();
                            dr["PO_Qty"] = (dgProducts.Rows[i].Cells["PO_Qty"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["PO_Qty"].Value);
                            dr["Inv_Qty"] = (dgProducts.Rows[i].Cells["Inv_Qty"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Inv_Qty"].Value);
                            dr["Tole_Qty"] = (dgProducts.Rows[i].Cells["Tole_Qty"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Tole_Qty"].Value);
                            dr["ReceivedQty"] = (dgProducts.Rows[i].Cells["ReceivedQty"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["ReceivedQty"].Value);
                            dr["RejectedQty"] = (dgProducts.Rows[i].Cells["RejectedQty"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["RejectedQty"].Value);
                            dr["AcceptedQty"] = (dgProducts.Rows[i].Cells["AcceptedQty"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["AcceptedQty"].Value);
                            dr["Basic_Price"] = (dgProducts.Rows[i].Cells["Basic_Price"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Basic_Price"].Value);


                            dr["Amt_Before_Disc"] = dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value.ToString();
                            dr["Disc_Per"] = (dgProducts.Rows[i].Cells["Disc_Per"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Per"].Value);
                            dr["Disc_Amt"] = dgProducts.Rows[i].Cells["Disc_Amt"].Value.ToString();
                            dr["Taxable_Value"] = dgProducts.Rows[i].Cells["Taxable_Value"].Value.ToString();
                            dr["CGST_Per"] = dgProducts.Rows[i].Cells["CGST_Per"].Value.ToString();
                            dr["CGST_Amt"] = dgProducts.Rows[i].Cells["CGST_Amt"].Value.ToString();
                            dr["SGST_Per"] = dgProducts.Rows[i].Cells["SGST_Per"].Value.ToString();
                            dr["SGST_Amt"] = dgProducts.Rows[i].Cells["SGST_Amt"].Value.ToString();
                            dr["IGST_Per"] = dgProducts.Rows[i].Cells["IGST_Per"].Value.ToString();
                            dr["IGST_Amt"] = dgProducts.Rows[i].Cells["IGST_Amt"].Value.ToString();
                            dr["Total_Amount"] = dgProducts.Rows[i].Cells["Total_Amount"].Value.ToString();
                            dr["PO_No"] = (dgProducts.Rows[i].Cells["PO_No"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["PO_No"].Value);
                            dr["PR_No"] = (dgProducts.Rows[i].Cells["PR_No"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["PR_No"].Value);
                            dr["Heat_No"] = (dgProducts.Rows[i].Cells["Heat_No"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Heat_No"].Value);
                            dr["TCNo"] = (dgProducts.Rows[i].Cells["TCNo"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["TCNo"].Value);
                            dr["Int_Batch_No"] = (dgProducts.Rows[i].Cells["Int_Batch_No"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Int_Batch_No"].Value);

                            dr["Remarks"] = (dgProducts.Rows[i].Cells["Remarks"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value);

                            dtexisting.Rows.Add(dr);

                        }
                        dtexisting.AcceptChanges();
                    }


                    if (ioneNet.Masters.ProdSearch.dtgetproducts.Rows.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("S_No", typeof(string));
                        dt.Columns.Add("Item_Code", typeof(string));
                        dt.Columns.Add("Prod_Code", typeof(string));
                        dt.Columns.Add("Item_Description", typeof(string));
                        dt.Columns.Add("Item_Spec", typeof(string));
                        dt.Columns.Add("Item_Grade", typeof(string));
                        dt.Columns.Add("HSN_Code", typeof(string));
                        dt.Columns.Add("UOM", typeof(string));   
                        dt.Columns.Add("PO_Qty", typeof(string));

                        dt.Columns.Add("Inv_Qty", typeof(string));
                        dt.Columns.Add("Tole_Qty", typeof(string));
                        dt.Columns.Add("ReceivedQty", typeof(string));
                        dt.Columns.Add("RejectedQty", typeof(string));
                        dt.Columns.Add("AcceptedQty", typeof(string));

                        dt.Columns.Add("Basic_Price", typeof(decimal));
                        dt.Columns.Add("Amt_Before_Disc", typeof(decimal));
                        dt.Columns.Add("Disc_Per", typeof(decimal));
                        dt.Columns.Add("Disc_Amt", typeof(decimal));
                        dt.Columns.Add("Taxable_Value", typeof(decimal));
                        dt.Columns.Add("CGST_Per", typeof(decimal));
                        dt.Columns.Add("CGST_Amt", typeof(decimal));
                        dt.Columns.Add("SGST_Per", typeof(decimal));
                        dt.Columns.Add("SGST_Amt", typeof(decimal));
                        dt.Columns.Add("IGST_Per", typeof(decimal));
                        dt.Columns.Add("IGST_Amt", typeof(decimal));
                        dt.Columns.Add("Total_Amount", typeof(decimal));
                        dt.Columns.Add("PO_No", typeof(string));
                        dt.Columns.Add("PR_No", typeof(string));
                        dt.Columns.Add("Heat_No", typeof(string));
                        dt.Columns.Add("TCNo", typeof(string));
                        dt.Columns.Add("Int_Batch_No", typeof(string));
                        dt.Columns.Add("Remarks", typeof(string));
                       

                        //dt.Rows.Add();
                        int j = dgProducts.Rows.Count - 1;
                        for (int i = 0; i < ioneNet.Masters.ProdSearch.dtgetproducts.Rows.Count; i++)
                        {
                            string prodcode = ioneNet.Masters.ProdSearch.dtgetproducts.Rows[i]["prod_id"].ToString();
                            string prod_code = ioneNet.Masters.ProdSearch.dtgetproducts.Rows[i]["prod_code"].ToString();

                            var getproducts = (from obj in db.Products
                                               join uom in db.UoM_Masters on obj.Prod_Primary_UOM_Id equals uom.UOM_ID
                                               where obj.prod_ID == Convert.ToInt32(prodcode)
                                               select new
                                               {
                                                   Item_Code = obj.prod_ID,
                                                   Prod_Code = prod_code,
                                                   Item_Description = obj.Prod_Name,
                                                   Item_Spec="",
                                                   Item_Grade = obj.Prod_Field2,
                                                   HSN_Code=obj.Prod_HSN_Code,
                                                   UOM = uom.Uom_Descr,
                                                   PO_Qty="",
                                                   Inv_Qty="",
                                                   Tole_Qty="",
                                                   ReceivedQty="",
                                                   RejectedQty="",
                                                   AcceptedQty="",
                                                   Basic_Price = 0,
                                                   Amt_Before_Disc = 0,
                                                   Disc_Per = 0,
                                                   Disc_Amt = 0,
                                                   Taxable_Value = 0,
                                                   CGST_Per = 0,
                                                   CGST_Amt = 0,
                                                   SGST_Per = 0,
                                                   SGST_Amt = 0,
                                                   IGST_Per = 0,
                                                   IGST_Amt = 0,
                                                   Total_Amount = 0,
                                                   PO_No="",
                                                   PR_No="",
                                                   Heat_No="",
                                                   TCNo="",
                                                   Int_Batch_No ="",

                                                   Remarks = "",
                                                   
                                               }).ToList();
                            j = j + 1;
                            dt.Rows.Add(j, getproducts[0].Item_Code,
                                                   getproducts[0].Prod_Code ,
                                                   getproducts[0].Item_Description ,
                                                   getproducts[0].Item_Spec,
                                                   getproducts[0].Item_Grade ,
                                                   getproducts[0].HSN_Code ,
                                                   getproducts[0].UOM,
                                                   getproducts[0].PO_Qty,
                                                   getproducts[0].Inv_Qty ,
                                                   getproducts[0].Tole_Qty ,
                                                   getproducts[0].ReceivedQty ,
                                                   getproducts[0].RejectedQty ,
                                                   getproducts[0].AcceptedQty ,
                                                   getproducts[0].Basic_Price ,
                                                   getproducts[0].Amt_Before_Disc,
                                                   getproducts[0].Disc_Per,
                                                  getproducts[0].Disc_Amt ,
                                                   getproducts[0].Taxable_Value,
                                                   getproducts[0].CGST_Per ,
                                                   getproducts[0].CGST_Amt ,
                                                   getproducts[0].SGST_Per ,
                                                   getproducts[0].SGST_Amt ,
                                                   getproducts[0].IGST_Per ,
                                                   getproducts[0].IGST_Amt ,
                                                   getproducts[0].Total_Amount ,
                                                   getproducts[0].PO_No ,
                                                   getproducts[0].PR_No ,
                                                   getproducts[0].Heat_No,                                                  getproducts[0].TCNo,
                                                   getproducts[0].Int_Batch_No,

                                                   getproducts[0].Remarks );

                        }

                        dtexisting = dtexisting.AsEnumerable().Union(dt.AsEnumerable()).CopyToDataTable();
                        dgProducts.DataSource = dtexisting;
                        
                            int l = 0;
                            for (int m = 0; m <= dgProducts.Rows.Count - 2; m++)
                            {
                                if (dgProducts.Rows[m].Cells["Item_Code"].Value != "")
                                {
                                    l = l + 1;
                                    dgProducts.Rows[m].Cells["S_No"].Value = l.ToString();
                                }
                            }

                    }
                }
                //DataTable dtexisting = new DataTable();
                if (e.KeyCode == Keys.F3)
                {
                    if (logIn.company == 1044)
                    {
                                            }
                    else
                    {
                        int i = dgProducts.CurrentCell.RowIndex;
                        ItemCode = (dgProducts.Rows[i].Cells["Item_Code"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Code"].Value).ToString();
                        RecQty = (dgProducts.Rows[i].Cells["Item_Spec"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Spec"].Value).ToString();
                        HeatNo = (dgProducts.Rows[i].Cells["Int_Batch_No"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Int_Batch_No"].Value).ToString();
                        if (ItemCode != "" && RecQty != "")
                        {
                            GlobalVariables.FormName = "GRN";
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
                Calcaulation();

                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

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
                Calcaulation();

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
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
                decimal rndAmt = (txtRounding.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtRounding.Text);

                txtTot_OrderValue.Text = (taxvalue + cgst + sgst + igst + TcsAmt + rndAmt).ToString(".00");
                decimal TDSAmt = (txtTDSAmount.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtTDSAmount.Text);
                decimal TotVal = Convert.ToDecimal(txtTot_OrderValue.Text);
                txtNetGRNValue.Text = (TotVal - TDSAmt).ToString(".00");

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }     
        
        
               
        private void txtfreight_Leave(object sender, EventArgs e)
        {
            Calcaulation();
            //othercalculation();

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
                    dtexisting.Columns.Add("S_No", typeof(string));
                    dtexisting.Columns.Add("Item_Code", typeof(string));
                    dtexisting.Columns.Add("Prod_Code", typeof(string));
                    dtexisting.Columns.Add("Item_Description", typeof(string));
                    dtexisting.Columns.Add("Item_Spec", typeof(string));
                    dtexisting.Columns.Add("Item_Grade", typeof(string));
                    dtexisting.Columns.Add("HSN_Code", typeof(string));
                    dtexisting.Columns.Add("UOM", typeof(string));
                    dtexisting.Columns.Add("PO_Qty", typeof(decimal));
                    dtexisting.Columns.Add("Inv_Qty", typeof(decimal));
                    dtexisting.Columns.Add("Tole_Qty", typeof(decimal));                    
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
                    dtexisting.Columns.Add("Int_Batch_No", typeof(string));
                    dtexisting.Columns.Add("Remarks", typeof(string));
                    //
                    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                    {
                        DataRow dr;
                        dr = dtexisting.NewRow();
                        dr["S_No"] = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                        dr["Item_Code"] = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                        dr["Prod_Code"] = dgProducts.Rows[i].Cells["Prod_Code"].Value.ToString();
                        dr["Item_Description"] = dgProducts.Rows[i].Cells["Item_Description"].Value.ToString();
                        dr["Item_Grade"] = dgProducts.Rows[i].Cells["Item_Grade"].Value.ToString();
                        dr["HSN_Code"] = dgProducts.Rows[i].Cells["HSN_Code"].Value.ToString();
                        dr["UOM"] = dgProducts.Rows[i].Cells["UOM"].Value.ToString();
                        dr["PO_Qty"] = dgProducts.Rows[i].Cells["PO_Qty"].Value.ToString();
                        dr["Inv_Qty"] = dgProducts.Rows[i].Cells["Inv_Qty"].Value.ToString();
                        dr["Tole_Qty"] = dgProducts.Rows[i].Cells["Tole_Qty"].Value.ToString();
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
                        dr["Int_Batch_No"] = dgProducts.Rows[i].Cells["Int_Batch_No"].Value.ToString();
                        dr["Remarks"] = dgProducts.Rows[i].Cells["Remarks"].Value.ToString();                        
                        dtexisting.Rows.Add(dr);

                    }
                    dtexisting.AcceptChanges();
                }



                dtgetproducts.Columns.Clear();
                dtgetproducts.Rows.Clear();
                dtgetproducts.Columns.Add("S_No", typeof(string));
                dtgetproducts.Columns.Add("Item_Code", typeof(string));
                dtgetproducts.Columns.Add("Prod_Code", typeof(string));
                dtgetproducts.Columns.Add("Item_Description", typeof(string));
                dtgetproducts.Columns.Add("Item_Spec", typeof(string));
                dtgetproducts.Columns.Add("Item_Grade", typeof(string));
                dtgetproducts.Columns.Add("HSN_Code", typeof(string));
                dtgetproducts.Columns.Add("UOM", typeof(string));
                dtgetproducts.Columns.Add("PO_Qty", typeof(decimal));
                dtgetproducts.Columns.Add("Inv_Qty", typeof(decimal));
                dtgetproducts.Columns.Add("Tole_Qty", typeof(decimal));
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
                dtgetproducts.Columns.Add("Int_Batch_No", typeof(string));                
                dtgetproducts.Columns.Add("Remarks", typeof(string));
                dtgetfinalprducts.Rows.Clear();
                //listBox.Items.Clear();
                // Get the selected items of SfDataGrid
                //var reflector = this.sfDataGrid1.View.GetPropertyAccessProvider();
                //var row = this.sfDataGrid1.SelectedItem;

                //string ProdCode;
                //string SoNo;


                //int j = dgProducts.Rows.Count - 1;
                //for (int i = 0; i < sfDataGrid1.SelectedItems.Count; i++)
                //{
                //    string prodcode = ioneNet.MaterialManagement.Transactions.frnPRDailog.dtgetproducts.Rows[i]["Prod_Code"].ToString();
                //    string PRNo = ioneNet.MaterialManagement.Transactions.frnPRDailog.dtgetproducts.Rows[i]["PR_No"].ToString();
                //    string PO_Qty = ioneNet.MaterialManagement.Transactions.frnPRDailog.dtgetproducts.Rows[i]["BalQty"].ToString();
                    

                //    var getproducts = (from s in db.Products
                //                       join t in db.Tax_Class_Masters on s.Prod_Tax_Class equals t.ID
                //                       join u in db.UoM_Masters on s.Prod_Primary_UOM_Id equals u.UOM_ID
                //                       where s.prod_ID == Convert.ToInt32(prodcode)
                //                       select new
                //                       { /*s.Prod_HSN_Code, t.Gst_Rate, s.Prod_Name, u.Uom_Descr*/

                //                           Item_Code = s.prod_ID,
                //                           Prod_Code = s.Prod_Code,
                //                           Item_Description = s.Prod_Name,
                //                           Item_Spec = "",
                //                           Item_Grade = s.Prod_Field2,
                //                           HSN_Code = s.Prod_HSN_Code,
                //                           UOM = u.Uom_Descr,
                //                           PO_Qty = PO_Qty.ToString(),
                //                           Inv_Qty = '0',
                //                           ReceivedQty = '0',
                //                           Tole_Qty = '0',
                //                           RejectedQty = '0',
                //                           AcceptedQty = '0',
                //                           Basic_Price = "0",
                //                           Amt_Before_Disc = "0",
                //                           Disc_Per = "0",
                //                           Disc_Amt = "0",
                //                           Taxable_Value = "0",
                //                           CGST_Per = "0",
                //                           CGST_Amt = "0",
                //                           SGST_Per = "0",
                //                           SGST_Amt = "0",
                //                           IGST_Per = "0",
                //                           IGST_Amt = "0",
                //                           Total_Amount = "0",
                //                           PO_No = "",
                //                           PR_No = PRNo,
                //                           Heat_No = "",
                //                           TCNo = "",
                //                           Int_Batch_No = "",
                //                           Remarks = ""
                //                       }).ToList();
                //    j = j + 1;
                //    dt.Rows.Add(j, getproducts[0].Item_Code,
                //                       getproducts[0].Prod_Code,
                //                       getproducts[0].Item_Description,
                //                       getproducts[0].Item_Spec,
                //                       getproducts[0].Item_Grade,
                //                       getproducts[0].HSN_Code,
                //                       getproducts[0].UOM,
                //                       getproducts[0].PO_Qty,
                //                       getproducts[0].Inv_Qty,
                //                       getproducts[0].Tole_Qty,
                //                       getproducts[0].ReceivedQty,
                //                       getproducts[0].RejectedQty,
                //                       getproducts[0].AcceptedQty,
                //                       getproducts[0].Basic_Price,
                //                       getproducts[0].Amt_Before_Disc,
                //                       getproducts[0].Disc_Per,
                //                      getproducts[0].Disc_Amt,
                //                       getproducts[0].Taxable_Value,
                //                       getproducts[0].CGST_Per,
                //                       getproducts[0].CGST_Amt,
                //                       getproducts[0].SGST_Per,
                //                       getproducts[0].SGST_Amt,
                //                       getproducts[0].IGST_Per,
                //                       getproducts[0].IGST_Amt,
                //                       getproducts[0].Total_Amount,
                //                       getproducts[0].PO_No,
                //                       getproducts[0].PR_No,
                //                       getproducts[0].Heat_No, getproducts[0].TCNo,
                //                       getproducts[0].Int_Batch_No,

                //                       getproducts[0].Remarks);

                //}

                //dtexisting = dtexisting.AsEnumerable().Union(dt.AsEnumerable()).CopyToDataTable();
                ////dtexisting = dtexisting.AsEnumerable().Union(dtgetfinalprducts.AsEnumerable()).CopyToDataTable();

                //dgProducts.DataSource = dtexisting;



                int j = dgProducts.Rows.Count-1;
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
                            j = j + 1;
                            var s_No = j ;
                            var Item_Code = (rowData.GetType().GetProperty("Prod_Code").GetValue(rowData, null).ToString());
                            var Item_Description = (rowData.GetType().GetProperty("Product_Description").GetValue(rowData, null).ToString());
                                              
                            var UOM = (rowData.GetType().GetProperty("Uom").GetValue(rowData, null).ToString());
                            var PO_Qty = (rowData.GetType().GetProperty("BalQty").GetValue(rowData, null).ToString());
                            var Basic_Price = (rowData.GetType().GetProperty("Price").GetValue(rowData, null).ToString());
                            var SO_Ref_No = (rowData.GetType().GetProperty("PO_No").GetValue(rowData, null).ToString());
                            var Rec_ID = (rowData.GetType().GetProperty("Rec_ID").GetValue(rowData, null).ToString());
                            var PR_Ref_No = (rowData.GetType().GetProperty("pr_no").GetValue(rowData, null).ToString());
                            var Original_PO_Qty = (rowData.GetType().GetProperty("PO_Qty").GetValue(rowData, null).ToString());
                           

                            var getTolQty = (from s in db.Purchase_Order_Masters
                                          where s.Id == Convert.ToInt32(Rec_ID)
                                             select new { s.Qty_Tolerence }).FirstOrDefault();

                            //if (getHSN != null)
                            decimal poQty = Convert.ToDecimal(Original_PO_Qty);
                            decimal TolPer = 0;
                            if (getTolQty.Qty_Tolerence != "")
                            {
                                TolPer = Convert.ToDecimal(getTolQty.Qty_Tolerence);
                            }
                            decimal Tolqty = 0;
                            if ( TolPer >0 )
                            {
                                Tolqty = Math.Round(poQty * TolPer / 100,2);
                            }
                            decimal DiscPer = 0;
                            var getDiscPer = (from s in db.Purchase_Order_Childs
                                              join p in db.Products on s.Prod_Code equals p.prod_ID
                                             where s.PO_Master_ID == Convert.ToInt32(Rec_ID) && (p.Prod_Code.Contains(Item_Code) || p.Prod_Alternative_Code.Contains(Item_Code))
                                              select new { s.Disc_Per, p.Prod_Field2,p.Prod_HSN_Code,p.prod_ID }).FirstOrDefault();
                            if (getDiscPer.Disc_Per != null)
                            {
                                DiscPer = Convert.ToDecimal(getDiscPer.Disc_Per);
                            }
                            else
                            {
                                DiscPer = 0;
                            }

                            //var a = (from s in db.Products
                            //         where s.prod_ID == Convert.ToInt32(Item_Code.ToString()) 
                            //         select new { s.Prod_Field2, s.Prod_HSN_Code, s.Prod_Code }).FirstOrDefault();


                            string Prod_Code = getDiscPer.prod_ID.ToString();
                            string Item_Grade =  (getDiscPer.Prod_Field2 == null) ? "" : getDiscPer.Prod_Field2.ToString();
                            //string Item_Grade = a.Prod_Field2.ToString();
                            string HSN_Code = getDiscPer.Prod_HSN_Code.ToString();
                            drgetproducts = dtgetproducts.NewRow();
                            drgetproducts["S_No"] = s_No;
                            drgetproducts["Item_Code"] = Prod_Code.ToString();
                            drgetproducts["Prod_Code"] = Item_Code.ToString();
                            drgetproducts["Item_Description"] = Item_Description.ToString();
                            drgetproducts["Item_Spec"] = "";
                            drgetproducts["Item_Grade"] = Item_Grade.ToString(); 
                            drgetproducts["HSN_Code"] = HSN_Code.ToString(); 
                            drgetproducts["UOM"] = UOM.ToString();
                            drgetproducts["PO_Qty"] = PO_Qty.ToString();
                            drgetproducts["Inv_Qty"] = 0;
                            drgetproducts["Tole_Qty"] = Tolqty.ToString();
                            drgetproducts["ReceivedQty"] = 0;
                            drgetproducts["RejectedQty"] = 0;
                            drgetproducts["AcceptedQty"] = 0;
                            drgetproducts["Basic_Price"] = Basic_Price.ToString();
                            drgetproducts["Amt_Before_Disc"] = 0;
                            drgetproducts["Disc_Per"] = DiscPer;
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
                            drgetproducts["PR_No"] = PR_Ref_No.ToString();
                            drgetproducts["Heat_No"] = "";
                            drgetproducts["TCNo"] = "";
                            drgetproducts["Int_Batch_No"] = "";                           
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
                dtgetSelectedprducts = dtgetSelectedprducts.AsEnumerable().Union(dtexisting.AsEnumerable()).CopyToDataTable();

                dgProducts.DataSource = dtgetSelectedprducts;
            
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
            if(CmbSuplierName.Text=="")
            {
                MessageBox.Show("Enter Supplier Name");
            }
           
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
                             where c.Supplier_Name == CmbSuplierName.Text && c.Company_ID == logIn.company
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

        private void txtRounding_Leave(object sender, EventArgs e)
        {
            Calcaulation();
        }

        private void txtTDSAmount_Leave(object sender, EventArgs e)
        {
            Calcaulation();
        }

        private void cmbPurchaseBasis_SelectedIndexChanged(object sender, EventArgs e)
        {
            //bindCustomer();
            if(cmbPurchaseBasis.Text == "With PO")
            {
                dgProducts.Columns["Basic_Price"].ReadOnly = true;
                dgProducts.Columns["Disc_Per"].ReadOnly = true;
               // dgProducts.Columns["Disc_Per"].ReadOnly = true;
            }
            else
            {
                dgProducts.Columns["Basic_Price"].ReadOnly = false;
                dgProducts.Columns["Disc_Per"].ReadOnly = false;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
           
        }

        private void txtInwardNo_Leave(object sender, EventArgs e)
        {
            //var dm1 = (from s in db.Invoice_Childs
            //           join p in db.Products on Convert.ToInt32(s.Prod_Code) equals p.prod_ID
            //           where s.Inv_No == txtInwardNo.Text && s.Company_ID == logIn.company
            //           orderby s.Item_No


            //           select new

            //           {
            //               Item_Code = s.Prod_Code,
            //               Prod_Code = p.Prod_Code,
            //               Item_Description = s.Product_Description.Trim(),
            //               HSN_Code = p.Prod_HSN_Code,
            //               UOM = s.Uom.Trim(),
            //               s.PO_Qty,
            //               s.Stock_qty,
            //               Inv_Qty = s.Qty,
            //               ReceivedQty = s.Qty,
            //               AcceptedQty  = s.Qty,
            //               Basic_Price = s.Price,
            //               Amt_Before_Disc = s.Amount,
            //               s.Disc_Per,
            //               Disc_Amt = s.Disc_Amount,
            //               s.Taxable_Value,
            //               s.CGST_Per,
            //               CGST_Amt = s.CGST_Amnt,
            //               s.SGST_Per,
            //               SGST_Amt = s.SGST_Amnt,
            //               s.IGST_Per,
            //               IGST_Amt = s.IGST_Amnt,
            //               Total_Amount = s.Net_Amount,
                        


            //           });




            //SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
            //SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //DataTable dtr = new DataTable();
            //da2.Fill(dtr);
            //if (dtr.Rows.Count >= 0)
            //    dgProducts.DataSource = dtr;

        }

        private void txtInwardNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtTDSPer_Leave(object sender, EventArgs e)
        {
            Calcaulation();
        }

        private void txtCessPetMT_Leave(object sender, EventArgs e)
        {
            if (txtCessPetMT.Text != "")
            {
                if (Convert.ToDecimal(txtCessPetMT.Text) > 0)
                {
                    decimal cessper = Convert.ToDecimal(txtCessPetMT.Text);
                    decimal TotQty = Convert.ToDecimal(txtTotalQty.Text);
                    txtCessAmount.Text = (cessper * TotQty).ToString("0.00");
                    Calcaulation();
                }
                else
                {
                    txtCessAmount.Text = "0";
                    Calcaulation();
                }
            }
        }

        private void dgProducts_Leave(object sender, EventArgs e)
        {
            
        }

        private void cmbPurchaseBasis_Leave(object sender, EventArgs e)
        {
            try
            {
                //if (GRN_Basis != "")
                //{
                //    if (cmbPurchaseBasis.Text != GRN_Basis && cmbPurchaseBasis.Text != "")
                //    {
                //        DialogResult result = MessageBox.Show("Are You Sure To Delete The Existing " + GRN_Basis + " Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                //        if (result == DialogResult.Yes)
                //        {
                //            dgProducts.Rows.Clear();
                //            //int count = 
                //            //for (int i = 0; i < dgProducts.Rows.Count; i++)
                //            //{
                //            //    i = 0;
                //            //    dgProducts.Rows.RemoveAt(i);
                //            //}

                //        }
                //        else
                //        {
                //            cmbPurchaseBasis.Text = GRN_Basis;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dgProducts_DragEnter(object sender, DragEventArgs e)
        {

        }

        private void dgProducts_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CmbSuplierName_Enter(object sender, EventArgs e)
        {
            string sname = cmbPurchaseAccount.Text;
            string pname = CmbSuplierName.Text;
            
            bindCustomer();
            CmbSuplierName.Text = pname;
            cmbPurchaseAccount.Text = sname;
        }

        private void cmbTaxClass_Leave(object sender, EventArgs e)
        {
            if (GoodsReceiptNoteList.editMode == true)
            {

            }
            else
            {
                if(cmbTaxClass.Text !="")
                { 
                    AutoincrementId();
                    switch (cmbTaxClass.Text)
                    {

                        case "Stores":
                            cmbWareHouse.Text = "Engg Stores";

                            break;
                        case "Steam Coal":
                            cmbWareHouse.Text = "Coal Yard";

                            break;
                        case "Rolls":
                            cmbWareHouse.Text = "Roll Yard";

                            break;
                        case "Capital":
                            cmbWareHouse.Text = "Engg Stores";

                            break;
                        case "Raw Material":
                            cmbWareHouse.Text = "TSL Yard";

                            break;
                    }
                }
            }
        }

        private void cmbPurchaseBasis_Enter(object sender, EventArgs e)
        {
            GRN_Basis = cmbPurchaseBasis.Text;
        }

        private void cmbConvPartyName_Leave(object sender, EventArgs e)
        {
            if (chkConversion.Checked)
            {
                if (cmbConvPartyName.Text != "")
                {

                    switch (cmbConvPartyName.Text)
                    {
                        case "RINL":
                            cmbWareHouse.Text = "RINL Yard";

                            break;
                        case "TATA":
                            cmbWareHouse.Text = "TATA Yard";

                            break;
                        case "JSW":
                            cmbWareHouse.Text = "JSW Yard";
                            break;
                    }
                }
            }
            else
            {

            }
        }

        private void btnTransLog_Click(object sender, EventArgs e)
        {
            transname = "Goods Receipot Note";
            transno = txtSoNo.Text;
            Transactions.frmTransLog form = new Transactions.frmTransLog();
            form.ShowDialog();
        }

        private void txtPFCharges_Leave(object sender, EventArgs e)
        {
            Calcaulation();
        }

        private void GoodsReceiptNote_FormClosed(object sender, FormClosedEventArgs e)
        {
            //SqlCommand cmd2 = new SqlCommand("delete  from [Bloom_Roll_Wise_Receipts] where [Grn_ID] =@ProdID and status ='Open' and company_ID = @comp", con);

            //// cmd2.Parameters.AddWithValue("@AccID", AccID);
            //cmd2.Parameters.AddWithValue("@comp", logIn.company);
            //cmd2.Parameters.AddWithValue("@ProdID", txtSoNo.Text);
            //if (con.State != ConnectionState.Open)
            //    con.Open();
            ////con.Open();
            //cmd2.ExecuteNonQuery();

            //con.Close();
        }

        private void txtothercharges_Leave(object sender, EventArgs e)
        {
            Calcaulation();
            //othercalculation();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            try
            {
                //int m = dgProducts.Rows.Count;
                //for (int i=1;i<=m-1; i++)
                //{
                //    dgProducts.Rows.RemoveAt(dgProducts.Rows[i].Index);
                //}

                //Get PR Data
                switch (cmbPurchaseBasis.Text)
                {

                    
                    case "With PR":
                        DataTable dtexisting = new DataTable();
                        MaterialManagement.Transactions.frnPRDailog form = new MaterialManagement.Transactions.frnPRDailog();
                        form.ShowDialog();
                        if (dgProducts.Rows.Count > 1)
                        {
                            dtexisting.Rows.Clear();
                            dtexisting.Columns.Clear();
                            for (int i = 0; i < dgProducts.ColumnCount; i++)
                            {
                                int columnIndex = i;
                                string columnName = dgProducts.Columns[columnIndex].Name;
                                dtexisting.Columns.Add(columnName, typeof(string));
                            }
                            //dtexisting.Columns.Add("Item_Code", typeof(string));
                            //dtexisting.Columns.Add("Item_Description", typeof(string));
                            //dtexisting.Columns.Add("Item_Spec", typeof(string));
                            //dtexisting.Columns.Add("Item_Grade", typeof(string));
                            //dtexisting.Columns.Add("UOM", typeof(string));
                            //dtexisting.Columns.Add("Qty", typeof(decimal));
                            //dtexisting.Columns.Add("Basic_Price", typeof(decimal));
                            //dtexisting.Columns.Add("Amt_Before_Disc", typeof(decimal));
                            //dtexisting.Columns.Add("Disc_Per", typeof(decimal));
                            //dtexisting.Columns.Add("Disc_Amt", typeof(decimal));
                            //dtexisting.Columns.Add("Taxable_Value", typeof(decimal));
                            //dtexisting.Columns.Add("CGST_Per", typeof(decimal));
                            //dtexisting.Columns.Add("CGST_Amt", typeof(decimal));
                            //dtexisting.Columns.Add("SGST_Per", typeof(decimal));
                            //dtexisting.Columns.Add("SGST_Amt", typeof(decimal));
                            //dtexisting.Columns.Add("IGST_Per", typeof(decimal));
                            //dtexisting.Columns.Add("IGST_Amt", typeof(decimal));
                            //dtexisting.Columns.Add("Total_Amount", typeof(decimal));
                            //dtexisting.Columns.Add("PR_No", typeof(string));
                            //dtexisting.Columns.Add("Remarks", typeof(string));

                            for (int i = 0; i < dgProducts.Rows.Count-1; i++)
                            {
                                DataRow dr;
                                dr = dtexisting.NewRow();
                                for (int c = 0; c < dgProducts.ColumnCount; c++)
                                {
                                    dr[c] = dgProducts.Rows[i].Cells[c].Value.ToString();
                                }
                                // dr["Item_Code"] = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                                //dr["Item_Description"] = dgProducts.Rows[i].Cells["Item_Description"].Value.ToString();
                                //dr["Item_Spec"] = dgProducts.Rows[i].Cells["Item_Spec"].Value.ToString();
                                //dr["Item_Grade"] = dgProducts.Rows[i].Cells["Item_Grade"].Value.ToString();
                                //dr["UOM"] = dgProducts.Rows[i].Cells["UOM"].Value.ToString();
                                //dr["Qty"] = dgProducts.Rows[i].Cells["Qty"].Value.ToString();
                                //dr["Basic_Price"] = dgProducts.Rows[i].Cells["Basic_Price"].Value.ToString();
                                //dr["Amt_Before_Disc"] = dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value.ToString();
                                //dr["Disc_Per"] = dgProducts.Rows[i].Cells["Disc_Per"].Value.ToString();
                                //dr["Disc_Amt"] = dgProducts.Rows[i].Cells["Disc_Amt"].Value.ToString();
                                //dr["Taxable_Value"] = dgProducts.Rows[i].Cells["Taxable_Value"].Value.ToString();
                                //dr["CGST_Per"] = dgProducts.Rows[i].Cells["CGST_Per"].Value.ToString();
                                //dr["CGST_Amt"] = dgProducts.Rows[i].Cells["CGST_Amt"].Value.ToString();
                                //dr["SGST_Per"] = dgProducts.Rows[i].Cells["SGST_Per"].Value.ToString();
                                //dr["SGST_Amt"] = dgProducts.Rows[i].Cells["SGST_Amt"].Value.ToString();
                                //dr["IGST_Per"] = dgProducts.Rows[i].Cells["IGST_Per"].Value.ToString();
                                //dr["IGST_Amt"] = dgProducts.Rows[i].Cells["IGST_Amt"].Value.ToString();
                                //dr["Total_Amount"] = dgProducts.Rows[i].Cells["Total_Amount"].Value.ToString();
                                //dr["PR_No"] = dgProducts.Rows[i].Cells["PR_No"].Value.ToString();
                                //dr["Remarks"] = dgProducts.Rows[i].Cells["Remarks"].Value.ToString();
                                dtexisting.Rows.Add(dr);

                            }
                            dtexisting.AcceptChanges();
                        }


                        if (ioneNet.MaterialManagement.Transactions.frnPRDailog.dtgetproducts.Rows.Count > 0)
                        {
                            DataTable dt = new DataTable();
                            for (int i = 0; i < dgProducts.ColumnCount ; i++)
                            {
                                int columnIndex = i;
                                string columnName = dgProducts.Columns[columnIndex].Name;
                                dt.Columns.Add(columnName, typeof(string));
                            }


                            //dt.Columns.Add("Item_Code", typeof(string));
                            //dt.Columns.Add("Item_Description", typeof(string));
                            //dt.Columns.Add("Item_Spec", typeof(string));
                            //dt.Columns.Add("Item_Grade", typeof(string));
                            //dt.Columns.Add("UOM", typeof(string));
                            //dt.Columns.Add("Qty", typeof(decimal));
                            //dt.Columns.Add("Basic_Price", typeof(decimal));
                            //dt.Columns.Add("Amt_Before_Disc", typeof(decimal));
                            //dt.Columns.Add("Disc_Per", typeof(decimal));
                            //dt.Columns.Add("Disc_Amt", typeof(decimal));
                            //dt.Columns.Add("Taxable_Value", typeof(decimal));
                            //dt.Columns.Add("CGST_Per", typeof(decimal));
                            //dt.Columns.Add("CGST_Amt", typeof(decimal));
                            //dt.Columns.Add("SGST_Per", typeof(decimal));
                            //dt.Columns.Add("SGST_Amt", typeof(decimal));
                            //dt.Columns.Add("IGST_Per", typeof(decimal));
                            //dt.Columns.Add("IGST_Amt", typeof(decimal));
                            //dt.Columns.Add("Total_Amount", typeof(decimal));
                            //dt.Columns.Add("PR_No", typeof(string));
                            //dt.Columns.Add("Remarks", typeof(string));

                            //dt.Rows.Add();
                            int j = dgProducts.Rows.Count - 1;
                            for (int i = 0; i < ioneNet.MaterialManagement.Transactions.frnPRDailog.dtgetproducts.Rows.Count; i++)
                            {
                                string prodcode = ioneNet.MaterialManagement.Transactions.frnPRDailog.dtgetproducts.Rows[i]["prod_id"].ToString();
                                string PRNo = ioneNet.MaterialManagement.Transactions.frnPRDailog.dtgetproducts.Rows[i]["PR_No"].ToString();
                                string PO_Qty = ioneNet.MaterialManagement.Transactions.frnPRDailog.dtgetproducts.Rows[i]["BalQty"].ToString();
                                //string UOM = ioneNet.Masters.ProdSearch.dtgetproducts.Rows[i]["Uom"].ToString();


                                //var d1 = (from a in db.Company_Infos where a.Id == logIn.company select new { a.GST_No }).ToList();
                                //if (d1.Count > 0)
                                //{
                                //    comnpstatecode = Mid(d1[0].GST_No, 1, 2);
                                //    suppStateCode = txtStateCode.Text;
                                //}
                               
                                var getproducts = (from s in db.Products
                                                   join t in db.Tax_Class_Masters on s.Prod_Tax_Class equals t.ID
                                                   join u in db.UoM_Masters on s.Prod_Primary_UOM_Id equals u.UOM_ID
                                                   where s.prod_ID == Convert.ToInt32(prodcode)
                                                   select new
                                                   { /*s.Prod_HSN_Code, t.Gst_Rate, s.Prod_Name, u.Uom_Descr*/

                                                       Item_Code = s.prod_ID,
                                                       Prod_Code = s.Prod_Code,
                                                       Item_Description = s.Prod_Name,
                                                       Item_Spec = "",
                                                       Item_Grade = s.Prod_Field2,
                                                       HSN_Code = s.Prod_HSN_Code,
                                                       UOM = u.Uom_Descr,
                                                       PO_Qty = PO_Qty.ToString(),
                                                       Inv_Qty = '0',
                                                       ReceivedQty = '0',
                                                       Tole_Qty = '0',
                                                       RejectedQty = '0',
                                                       AcceptedQty = '0',
                                                       Basic_Price = "0",
                                                       Amt_Before_Disc = "0",
                                                       Disc_Per = "0",
                                                       Disc_Amt = "0",
                                                       Taxable_Value = "0",
                                                       CGST_Per = "0",
                                                       CGST_Amt = "0",
                                                       SGST_Per = "0",
                                                       SGST_Amt = "0",
                                                       IGST_Per = "0",
                                                       IGST_Amt = "0",
                                                       Total_Amount = "0",
                                                       PO_No = "",
                                                       PR_No = PRNo,
                                                       Heat_No ="",
                                                       TCNo ="",
                                                       Int_Batch_No="",                                                      
                                                       Remarks = ""
                                                   }).ToList();
                                j = j + 1;
                                dt.Rows.Add(j, getproducts[0].Item_Code,
                                                   getproducts[0].Prod_Code,
                                                   getproducts[0].Item_Description,
                                                   getproducts[0].Item_Spec,
                                                   getproducts[0].Item_Grade,
                                                   getproducts[0].HSN_Code,
                                                   getproducts[0].UOM,
                                                   getproducts[0].PO_Qty,
                                                   getproducts[0].Inv_Qty,
                                                   getproducts[0].Tole_Qty,
                                                   getproducts[0].ReceivedQty,
                                                   getproducts[0].RejectedQty,
                                                   getproducts[0].AcceptedQty,
                                                   getproducts[0].Basic_Price,
                                                   getproducts[0].Amt_Before_Disc,
                                                   getproducts[0].Disc_Per,
                                                   getproducts[0].Disc_Amt,
                                                   getproducts[0].Taxable_Value,
                                                   getproducts[0].CGST_Per,
                                                   getproducts[0].CGST_Amt,
                                                   getproducts[0].SGST_Per,
                                                   getproducts[0].SGST_Amt,
                                                   getproducts[0].IGST_Per,
                                                   getproducts[0].IGST_Amt,
                                                   getproducts[0].Total_Amount,
                                                   getproducts[0].PO_No,
                                                   getproducts[0].PR_No,
                                                   getproducts[0].Heat_No,
                                                   getproducts[0].TCNo,
                                                   getproducts[0].Int_Batch_No,
                                                   getproducts[0].Remarks);

                            }

                            dtexisting = dtexisting.AsEnumerable().Union(dt.AsEnumerable()).CopyToDataTable();
                            //dtexisting = dtexisting.AsEnumerable().Union(dtgetfinalprducts.AsEnumerable()).CopyToDataTable();

                            dgProducts.DataSource = dtexisting;
                            
                        }
                        break;


                    case "With PO":


                        //Get PO Data

                        var d = (from data in db.SP_GetOrders_Sel(logIn.company, CmbSuplierName.Text, logIn.BU_ID, "") select data).ToList();

                        if (d.Count > 0)
                        {
                            //dgProductsList.DataSource = d;
                            sfDataGrid1.DataSource = d;
                            this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                            this.sfDataGrid1.Columns["PO_No"].FilterRowEditorType = "TextBox";
                            this.sfDataGrid1.Columns["PO_No"].ShowFilterRowOptions = false;
                            this.sfDataGrid1.Columns["PO_No"].ImmediateUpdateColumnFilter = true;
                            this.sfDataGrid1.Columns["PO_No"].FilterRowCondition = FilterRowCondition.Contains;

                            this.sfDataGrid1.Columns["Product_Description"].FilterRowEditorType = "TextBox";
                            this.sfDataGrid1.Columns["Product_Description"].ShowFilterRowOptions = false;
                            this.sfDataGrid1.Columns["Product_Description"].ImmediateUpdateColumnFilter = true;
                            this.sfDataGrid1.Columns["Product_Description"].FilterRowCondition = FilterRowCondition.Contains;
                            groupBox2.Visible = true;
                            txtSearch.Focus();
                            dgProducts.Columns["Item_Description"].ReadOnly = true;
                            //dgProducts.AllowUserToAddRows = false;
                        }
                        break;
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
                if (cmbPurchaseBasis.Text == "Direct" || cmbPurchaseBasis.Text == "For Job Work")
                {
                    string pcode = "";
                    if (columnName == "Item_Grade" && cmbTaxClass.Text == "Raw Material")
                    {
                        if (R1.Cells["Item_Grade"].Value != null)
                        {
                            string prodgrade = R1.Cells["Item_Grade"].Value.ToString();
                            var S = (from a in db.QA_Mtrl_Grade_Masters
                                     where a.Company_ID == logIn.company && a.Material_Grade == prodgrade
                                     select a).ToList();
                            if (S.Count > 0)
                            {

                            }
                            else
                            {
                                MessageBox.Show("Material Grade Entered is invalid");
                                R1.Cells["Item_Grade"].Value = "";
                                return;
                            }
                        }
                    }

                    if (columnName == "Prod_Code" && R1.Cells["Prod_Code"].Value != null)
                    {

                        pcode = R1.Cells["Prod_Code"].Value.ToString();
                    }

                    if (columnName == "Item_Description" && R1.Cells["Item_Description"].Value != null)
                    {
                        pcode = R1.Cells["Item_Description"].Value.ToString();
                    }
                    if (pcode != "")
                    {
                        var d = (from data in db.Get_Product_into_Trans(logIn.company, logIn.BU_ID, pcode)
                                 select
                                     new
                                     {
                                         Item_Code = data.prod_id,
                                         data.Prod_Code,
                                         Item_Description = data.Prod_Name,
                                         UOM = data.Uom_Descr,
                                         data.Prod_Field2,
                                         data.Prod_HSN_Code
                                     }).ToList();

                        if (d.Count > 0)
                        {
                            //if (d[0].Prod_Field2 != null)
                            //{
                            //    R1.Cells["Item_Grade"].Value = d[0].Prod_Field2.ToString();
                            //}

                            R1.Cells["Item_Description"].Value = d[0].Item_Description.ToString();

                            R1.Cells["UOM"].Value = d[0].UOM.ToString();
                            R1.Cells["Item_code"].Value = d[0].Item_Code.ToString();
                            R1.Cells["Prod_Code"].Value = d[0].Prod_Code.ToString();
                            R1.Cells["HSN_Code"].Value = d[0].Prod_HSN_Code.ToString();
                            R1.Cells["PO_Qty"].Value = "0";
                            R1.Cells["Tole_Qty"].Value = "0";
                            R1.Cells["RejectedQty"].Value = "0";
                            R1.Cells["Disc_Per"].Value = "0";
                            R1.Cells["Disc_Per"].Value = "0";

                            if (R1.Cells["S_no"].Value == null || R1.Cells["S_no"].Value.ToString() == "")
                            {
                                R1.Cells["S_no"].Value = dgProducts.Rows.Count - 1;
                            }

                        }
                        else
                        {
                            MessageBox.Show("Record Not Found");
                            return;
                        }

                        //Check for Duplicate Item Entry
                        for (int i = 0; i < dgProducts.CurrentRow.Index; i++)
                        {
                            if(dgProducts.Rows[i].Cells["Item_Description"].Value == R1.Cells["Item_Description"].Value)
                            {
                                MessageBox.Show("The Product Already Selected / Enterered Cannot be Repeated");
                                R1.Cells["Item_Description"].Value = "";
                                return;

                            }
                          
                        }


                        //    if (columnName == "Prod_Code")
                        //{
                        //    if (R1.Cells["Prod_Code"].Value != null)
                        //    {

                        //        var getProductName = (from s in db.Products
                        //                              join u in db.UoM_Masters on s.Prod_Primary_UOM_Id equals u.UOM_ID
                        //                              join g in db.Product_Groups on s.Prod_Group_Id equals g.ID
                        //                              where s.Prod_Alternative_Code == R1.Cells["Prod_Code"].Value.ToString() && s.Company_ID == logIn.company && s.Purchase_Account == logIn.BU_ID
                        //                              select new { s.prod_ID, u.Uom_Descr, g.Prod_Group_Name, s.Prod_HSN_Code, s.Prod_Code, s.Prod_Field2, s.Prod_Name }).FirstOrDefault();

                        //        if (getProductName != null)
                        //        {
                        //            if (getProductName.Prod_Field2 != null)
                        //            {
                        //                R1.Cells["Item_Grade"].Value = getProductName.Prod_Field2.ToString();
                        //            }

                        //           R1.Cells["Item_Description"].Value = getProductName.Prod_Name.ToString();

                        //            R1.Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                        //            R1.Cells["Item_code"].Value = getProductName.prod_ID.ToString();
                        //            R1.Cells["HSN_Code"].Value = getProductName.Prod_HSN_Code.ToString();
                        //            if (R1.Cells["S_no"].Value == null || R1.Cells["S_no"].Value.ToString() == "")
                        //            {
                        //                R1.Cells["S_no"].Value = dgProducts.Rows.Count - 1;
                        //            }
                        //            //if (R1.Cells["Product_Descr"].Value == "" || R1.Cells["Product_Descr"].Value == DBNull.Value || R1.Cells["Product_Descr"].Value == null)
                        //            //{
                        //            //    R1.Cells["Product_Descr"].Value = getProductName.Product_Descr.ToString();
                        //            //}
                        //        }
                        //        else
                        //        {
                        //            MessageBox.Show("Invalid Productname Entered");
                        //            R1.Cells["Item_Description"].Value = "";
                        //            return;
                        //        }
                        //    }
                        //}

                        //if (columnName == "Item_Description")
                        //{
                        //    if (R1.Cells["Item_Description"].Value != null)
                        //    {

                        //        var getProductName = (from s in db.Products
                        //                              join u in db.UoM_Masters on s.Prod_Primary_UOM_Id equals u.UOM_ID
                        //                              join g in db.Product_Groups on s.Prod_Group_Id equals g.ID
                        //                              where s.Prod_Name == R1.Cells["Item_Description"].Value.ToString() && s.Company_ID == logIn.company && s.Purchase_Account == logIn.BU_ID
                        //                              select new { s.prod_ID, u.Uom_Descr, g.Prod_Group_Name, s.Prod_HSN_Code, s.Prod_Code, s.Prod_Field2,s.Prod_Alternative_Code }).FirstOrDefault();

                        //        if (getProductName != null)
                        //        {
                        //            if (getProductName.Prod_Field2 != null)
                        //            {
                        //                R1.Cells["Item_Grade"].Value = getProductName.Prod_Field2.ToString();
                        //            }
                        //            if (logIn.company == 1044)
                        //            {
                        //                R1.Cells["Prod_Code"].Value = getProductName.Prod_Alternative_Code.ToString();
                        //            }
                        //            else
                        //            {
                        //                R1.Cells["Prod_Code"].Value = getProductName.Prod_Code.ToString();
                        //            }
                        //            R1.Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                        //            R1.Cells["Item_code"].Value = getProductName.prod_ID.ToString();
                        //            R1.Cells["HSN_Code"].Value = getProductName.Prod_HSN_Code.ToString();
                        //            if (R1.Cells["S_no"].Value == null || R1.Cells["S_no"].Value.ToString() == "")
                        //            {
                        //                R1.Cells["S_no"].Value = dgProducts.Rows.Count - 1;
                        //            }
                        //            //if (R1.Cells["Product_Descr"].Value == "" || R1.Cells["Product_Descr"].Value == DBNull.Value || R1.Cells["Product_Descr"].Value == null)
                        //            //{
                        //            //    R1.Cells["Product_Descr"].Value = getProductName.Product_Descr.ToString();
                        //            //}
                        //        }
                        //        else
                        //        {
                        //            MessageBox.Show("Invalid Productname Entered");
                        //            R1.Cells["Item_Description"].Value = "";
                        //            return;
                        //        }
                        //    }
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
                        decimal TolQty = (R1.Cells["Tole_Qty"].Value == "" || R1.Cells["Tole_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Tole_Qty"].Value);
                        decimal ReceivedQty = (R1.Cells["ReceivedQty"].Value == "" || R1.Cells["ReceivedQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["ReceivedQty"].Value);
                        decimal RejectedQty = (R1.Cells["RejectedQty"].Value == "" || R1.Cells["RejectedQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["RejectedQty"].Value);
                        if (cmbPurchaseBasis.Text == "With PR" || cmbPurchaseBasis.Text == "With PO")
                        {
                            if (ReceivedQty > (POQty+ TolQty))
                            {
                                MessageBox.Show("Received Qty Cannot Be Greater Than PO Qty");
                                dgProducts.CurrentCell = dgProducts.Rows[dgProducts.CurrentRow.Index].Cells[dgProducts.CurrentCell.ColumnIndex];
                                R1.Cells["ReceivedQty"].Value = 0;
                                
                               // R1.Cells["ReceivedQty"].Selected = true;
                                return;
                            }
                            else
                            {
                                R1.Cells["AcceptedQty"].Value = (ReceivedQty - RejectedQty).ToString("0.000");
                            }
                        }
                        else
                        {
                            R1.Cells["AcceptedQty"].Value = (ReceivedQty - RejectedQty).ToString("0.000");
                        }
                        
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
                        var d1 = (from a in db.Costing_Units where a.id == logIn.BU_ID select new { a.GST_No }).ToList();
                        if (d1.Count > 0)
                        {
                            comnpstatecode = Mid(d1[0].GST_No, 1, 2);
                            suppStateCode = txtStateCode.Text;
                            //if (chkRCM.Checked == false)
                            //{
                            //    if (suppStateCode == comnpstatecode)
                            //    {
                            //        d = Convert.ToDecimal(taxRate) / 2;
                            //        R1.Cells["CGST_Per"].Value = d.ToString("0.00");
                            //        R1.Cells["SGST_Per"].Value = d.ToString("0.00");
                            //        R1.Cells["IGST_Per"].Value = "0.00";
                            //        cgstPer = d;
                            //        sgstPer = d;
                            //    }
                            //    else
                            //    {
                            //        d = taxRate;
                            //        R1.Cells["CGST_Per"].Value = "0.00";
                            //        R1.Cells["SGST_Per"].Value = "0.00";
                            //        R1.Cells["IGST_Per"].Value = d.ToString("0.00");
                            //        igstPer = d;                                    
                            //    }
                            //}
                            //else
                            //{
                                R1.Cells["CGST_Per"].Value = "0.00";
                                R1.Cells["SGST_Per"].Value = "0.00";
                                R1.Cells["IGST_Per"].Value = "0.00";
                                cgstPer = 0;
                                sgstPer = 0;
                            //}
                        }
                    }

                }
                else
                {
                    //if (columnName == "Basic_Price" || columnName == "Disc_Per")
                    //{

                    //}
                    //else
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
                                R1.Cells["AcceptedQty"].Value = (ReceivedQty - RejectedQty).ToString("0.000");
                            }

                            //R1.Cells["AcceptedQty"].Value = (ReceivedQty - RejectedQty).ToString("0.00");
                        }
                    }
                }
                decimal price = (R1.Cells["Basic_Price"].Value == "" || R1.Cells["Basic_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Basic_Price"].Value);
                decimal DiscPer = (R1.Cells["Disc_Per"].Value == "" || R1.Cells["Disc_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Disc_Per"].Value);
                //Consider Tole Qty for Billing

                double ToleQty =0;
                //= (R1.Cells["Tole_Qty"].Value == "" || R1.Cells["Tole_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Tole_Qty"].Value);
                decimal InvQty = (R1.Cells["Inv_Qty"].Value == "" || R1.Cells["Inv_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Inv_Qty"].Value);
                decimal RQty = (R1.Cells["ReceivedQty"].Value == "" || R1.Cells["ReceivedQty"].Value == DBNull.Value) ? Convert.ToDecimal("00.00") : Convert.ToDecimal(R1.Cells["ReceivedQty"].Value);
                double TolePer =0.4;
                if (logIn.company == 18)
                {
                    ToleQty = Convert.ToDouble(InvQty) * (TolePer / 100);
                    R1.Cells["Tole_Qty"].Value = ToleQty.ToString("0.00");
                }
                
                decimal AcceptedQty =0;

                if (RQty <InvQty)
                {
                    if (RQty + Convert.ToDecimal(ToleQty) > InvQty)
                    {
                        AcceptedQty = InvQty;
                    }
                    else
                    {
                        AcceptedQty = RQty + Convert.ToDecimal(ToleQty);
                    }
                }
                else
                {
                    AcceptedQty = (R1.Cells["AcceptedQty"].Value == "" || R1.Cells["AcceptedQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["AcceptedQty"].Value);
                }

                Calcaulation();
                

                //decimal Amt, DiscAmt, netAmt, gst, igst, totamt;

                //Amt = AcceptedQty * price;

                //R1.Cells["Amt_Before_Disc"].Value = Amt.ToString("0.00");
                //DiscAmt = (Amt * DiscPer) / 100;
                //R1.Cells["Disc_Amt"].Value = DiscAmt.ToString("0.00");
                //netAmt = Amt - DiscAmt;
                //R1.Cells["Taxable_Value"].Value = netAmt.ToString("0.00");
                //gst = (Convert.ToDecimal(R1.Cells["Taxable_Value"].Value) * Convert.ToDecimal(R1.Cells["CGST_Per"].Value)) / 100;

                //R1.Cells["CGST_Amt"].Value = gst;
                //R1.Cells["SGST_Amt"].Value = gst;
                //igst = (Convert.ToDecimal(R1.Cells["Taxable_Value"].Value) * Convert.ToDecimal(R1.Cells["IGST_Per"].Value)) / 100;
                //R1.Cells["IGST_Amt"].Value = igst;

                //totamt = Math.Round(netAmt + gst + gst + igst);
                //R1.Cells["Total_Amount"].Value = totamt;

                //decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
                //for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                //{

                //    x += (dgProducts.Rows[i].Cells["AcceptedQty"].Value == "" || dgProducts.Rows[i].Cells["AcceptedQty"].Value == null || dgProducts.Rows[i].Cells["AcceptedQty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["AcceptedQty"].Value);
                //    y += (dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == "" || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == null || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value);
                //    q += (dgProducts.Rows[i].Cells["Disc_Amt"].Value == "" || dgProducts.Rows[i].Cells["Disc_Amt"].Value == null || dgProducts.Rows[i].Cells["Disc_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Amt"].Value);
                //    v += (dgProducts.Rows[i].Cells["Taxable_Value"].Value == "" || dgProducts.Rows[i].Cells["Taxable_Value"].Value == null || dgProducts.Rows[i].Cells["Taxable_Value"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Taxable_Value"].Value);
                //    cg += (dgProducts.Rows[i].Cells["CGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["CGST_Amt"].Value == null || dgProducts.Rows[i].Cells["CGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Amt"].Value);
                //    sg += (dgProducts.Rows[i].Cells["SGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["SGST_Amt"].Value == null || dgProducts.Rows[i].Cells["SGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Amt"].Value);
                //    ig += (dgProducts.Rows[i].Cells["IGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["IGST_Amt"].Value == null || dgProducts.Rows[i].Cells["IGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Amt"].Value);
                //    totA += (dgProducts.Rows[i].Cells["Total_Amount"].Value == "" || dgProducts.Rows[i].Cells["Total_Amount"].Value == null || dgProducts.Rows[i].Cells["Total_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Total_Amount"].Value);

                //}

                //txtTotalQty.Text = x.ToString("0.00");

                //txtSubTotal.Text = y.ToString("0.00");
                //txtTotDiscount.Text = q.ToString("0.00");
                //decimal fAmt = (txtfreight.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtfreight.Text);
                //decimal OthAmt = (txtothercharges.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtothercharges.Text);
                //txtTot_TaxableValue.Text = (v + fAmt + OthAmt).ToString(".00");
                //decimal taxvalue = Convert.ToDecimal(txtTot_TaxableValue.Text);
                //decimal cgst = (taxvalue * cgstPer) / 100;
                //decimal sgst = (taxvalue * sgstPer) / 100;
                // igst = (taxvalue * igstPer) / 100;

                //txtTot_CGST.Text = cgst.ToString(".00");
                //txtTot_SGST.Text = sgst.ToString(".00");
                //txtTot_IGST.Text = igst.ToString(".00");                
                //decimal AmtForTCs = (v);                
                //decimal tcsPer = (txttcsper.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txttcsper.Text);

                //decimal TcsAmt = AmtForTCs * tcsPer / 100;
                //txttcsAmnt.Text = TcsAmt.ToString(".00");
                //decimal rndAmt = (txtRounding.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtRounding.Text);
                //txtTot_OrderValue.Text = (taxvalue + cgst + sgst + igst + TcsAmt + rndAmt).ToString(".00");
                //decimal TDSAmt = (txtTDSAmount.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtTDSAmount.Text);
                //decimal TotVal = Convert.ToDecimal(txtTot_OrderValue.Text);
                //txtNetGRNValue.Text = (TotVal - TDSAmt).ToString(".00");


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        public void Calcaulation()
        {
            decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0, GSTPer = 0;
            decimal acceptedQty =0;
            string comnpstatecode = "";
            string suppStateCode = "";
            var d1 = (from a in db.Costing_Units where a.id == logIn.BU_ID select new { a.GST_No }).ToList();
            if (d1.Count > 0)
            {
                comnpstatecode = Mid(d1[0].GST_No, 1, 2);
                suppStateCode = txtStateCode.Text;

            }
            decimal gstRate = 0;
            for (int i = 0; i < dgProducts.Rows.Count; i++)
            {
                DataGridViewRow R1 = dgProducts.Rows[i];
                int Itemcode = Convert.ToInt32(R1.Cells["Item_Code"].Value);
                acceptedQty = (R1.Cells["AcceptedQty"].Value == "" || R1.Cells["AcceptedQty"].Value == DBNull.Value) ? Convert.ToDecimal("00.00") : Convert.ToDecimal(R1.Cells["AcceptedQty"].Value);

                //acceptedQty = Convert.ToDecimal(R1.Cells["AcceptedQty"].Value);
                var taxratelist = (from prd in db.Products join tax in db.Tax_Class_Masters on prd.Prod_Tax_Class equals tax.ID where prd.prod_ID == Itemcode select new { tax.Gst_Rate }).ToList();
                if (taxratelist.Count > 0)
                {
                    taxRate = Convert.ToInt32(taxratelist[0].Gst_Rate); //Convert.ToInt32(getProduct_Name.GSTRate);
                }
                //if (chkRCM.Checked == false)
                //{
                if (suppStateCode == comnpstatecode)
                {
                    GSTPer = Convert.ToDecimal(taxRate) / 2;
                    R1.Cells["CGST_Per"].Value = GSTPer.ToString("0.00");
                    R1.Cells["SGST_Per"].Value = GSTPer.ToString("0.00");
                    R1.Cells["IGST_Per"].Value = "0.00";
                    cgstPer = GSTPer;
                    sgstPer = GSTPer;
                    if (taxRate > gstRate)
                    {
                        gstRate = taxRate;
                    }
                }
                else
                {
                    GSTPer = taxRate;
                    R1.Cells["CGST_Per"].Value = "0.00";
                    R1.Cells["SGST_Per"].Value = "0.00";
                    R1.Cells["IGST_Per"].Value = GSTPer.ToString("0.00");
                    igstPer = GSTPer;
                    if (taxRate > gstRate)
                    {
                        gstRate = taxRate;
                    }
                }
            //}
            ////else
            ////{
            //R1.Cells["CGST_Per"].Value = "0.00";
            //        R1.Cells["SGST_Per"].Value = "0.00";
            //        R1.Cells["IGST_Per"].Value = "0.00";
            //        cgstPer = 0;
            //        sgstPer = 0;
                //}
                decimal price = (R1.Cells["Basic_Price"].Value == "" || R1.Cells["Basic_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Basic_Price"].Value);
                decimal DiscPer = (dgProducts.Rows[i].Cells["Disc_Per"].Value == "" || dgProducts.Rows[i].Cells["Disc_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Per"].Value);
                decimal InvQty = (dgProducts.Rows[i].Cells["AcceptedQty"].Value == "" || dgProducts.Rows[i].Cells["AcceptedQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["AcceptedQty"].Value);
                decimal Amt = 0;
                if (InvQty > 0)
                {
                    Amt = InvQty * price;
                }
                else
                {
                    Amt = acceptedQty * price;
                }

                R1.Cells["Amt_Before_Disc"].Value = Amt.ToString("0.00");
                decimal DiscAmt = (Amt * DiscPer) / 100;
                R1.Cells["Disc_Amt"].Value = DiscAmt.ToString("0.00");
                decimal netAmt = Amt - DiscAmt;
                R1.Cells["Taxable_Value"].Value = netAmt.ToString("0.00");
                decimal gst = (Convert.ToDecimal(R1.Cells["Taxable_Value"].Value) * Convert.ToDecimal(R1.Cells["CGST_Per"].Value)) / 100;

                R1.Cells["CGST_Amt"].Value = gst;
                R1.Cells["SGST_Amt"].Value = gst;
                decimal igstAmt = (Convert.ToDecimal(R1.Cells["Taxable_Value"].Value) * Convert.ToDecimal(R1.Cells["IGST_Per"].Value)) / 100;
                R1.Cells["IGST_Amt"].Value = igstAmt;

                decimal totamt = Math.Round(netAmt + gst + gst + igstAmt);
                R1.Cells["Total_Amount"].Value = totamt;


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
            decimal PFAmt = (txtPFCharges.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtPFCharges.Text);
            txtTot_TaxableValue.Text = (v + fAmt + OthAmt+PFAmt).ToString(".00");
            decimal taxvalue = (fAmt + OthAmt); // Get Amount To calculate GST on Frieght and Others

            
            decimal cgst = 0;
            decimal sgst = 0;
            decimal igst = 0;
            if (cg > 0)
            {
                cgst = (taxvalue * (gstRate/2)) / 100;
                sgst = (taxvalue * (gstRate/2)) / 100;
            }
            else
            { 
            igst = (taxvalue * gstRate) / 100;
            }

            txtTot_CGST.Text = (cg + cgst).ToString(".00");
            txtTot_SGST.Text = (sg+ sgst).ToString(".00");
            txtTot_IGST.Text = (ig+ igst).ToString(".00");
            
            decimal AmtForTCs = (v + fAmt + OthAmt + PFAmt+Convert.ToDecimal(txtTot_CGST.Text) +Convert.ToDecimal(txtTot_SGST.Text) + Convert.ToDecimal(txtTot_IGST.Text));
            txtTotal_Amt.Text = (AmtForTCs).ToString(".00");
            decimal tcsPer = (txttcsper.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txttcsper.Text);

            decimal TcsAmt = AmtForTCs * tcsPer / 100;
            txttcsAmnt.Text = TcsAmt.ToString(".00");
            decimal cessAmt = (txtCessAmount.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtCessAmount.Text);
            decimal rndAmt = (txtRounding.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtRounding.Text);
            txtTot_OrderValue.Text = (v+taxvalue + cg+cgst + sg+sgst + ig+igst + TcsAmt + rndAmt+cessAmt).ToString(".00");
            decimal tdsPer = (txtTDSPer.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtTDSPer.Text);
            decimal AmtforTDS = (txtTot_OrderValue.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtTot_OrderValue.Text);
            decimal TDSAmt = v * tdsPer / 100;
            txtTDSAmount.Text = TDSAmt.ToString(".00");
            //decimal TDSAmt = (txtTDSAmount.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtTDSAmount.Text);
            decimal TotVal = Convert.ToDecimal(txtTot_OrderValue.Text);
            txtNetGRNValue.Text = (TotVal - TDSAmt).ToString(".00");
        }

        public void SaveNew_Sql_proc()
        {
            try
            {
                String myString = "";
                myString = txtSoNo.Text;
                
                if (txtSoNo.Text != "")
                {

                    if (GoodsReceiptNoteList.editMode == true)
                    {
                        

                    }
                    else
                    {
                        AutoincrementId();
                        GoodsReceiptNoteList.editMode = false;
                    }

                    myString = txtSoNo.Text;
                    SqlCommand cmd = new SqlCommand("SaveGRN", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Grn_NO", myString);
                    cmd.Parameters.AddWithValue("@Grn_Date", dpSODate.Value);
                    cmd.Parameters.AddWithValue("@Inward_No", (txtInwardNo.Text == "") ? "" : txtInwardNo.Text);
                    cmd.Parameters.AddWithValue("@Inward_Date", dtInwardDate.Value);
                    cmd.Parameters.AddWithValue("@Purchase_Bases", cmbPurchaseBasis.Text);
                    cmd.Parameters.AddWithValue("@SupplierName", Convert.ToInt32(CmbSuplierName.SelectedValue.ToString()));
                    int Pa = 7210;
                    if (cmbPurchaseAccount.Text != "" && cmbPurchaseAccount.Text != "NA")
                    {
                        Pa = Convert.ToInt32(cmbPurchaseAccount.SelectedValue.ToString());
                    }
                    else
                    {
                        Pa = 7210;
                    }

                    cmd.Parameters.AddWithValue("@Purchase_Account", Pa);
                    cmd.Parameters.AddWithValue("@GRN_Type", cmbTaxClass.Text);
                    cmd.Parameters.AddWithValue("@DC_No", (txtDcNo.Text == "") ? "" : txtDcNo.Text);
                    cmd.Parameters.AddWithValue("@DC_Date", dpDCDate.Value);
                    cmd.Parameters.AddWithValue("@Supplier_InvNo", (txtSupplierInvNo.Text == "") ? "" : txtSupplierInvNo.Text);

                    cmd.Parameters.AddWithValue("@Supplier_InvDate", dtsupinvdate.Value);
                    cmd.Parameters.AddWithValue("@OriginalInvReceived", chkOriginalInvoice.Checked);

                    
                    
                    cmd.Parameters.AddWithValue("@TotalQty", (txtTotalQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotalQty.Text));
                    cmd.Parameters.AddWithValue("@SubTotal", (txtSubTotal.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtSubTotal.Text));
                    cmd.Parameters.AddWithValue("@Tot_Discount", (txtTotDiscount.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotDiscount.Text));
                    cmd.Parameters.AddWithValue("@Freight", (txtfreight.Text == null || txtfreight.Text == "") ? 0 : Convert.ToDecimal(txtfreight.Text));
                    cmd.Parameters.AddWithValue("@Other_Charges", (txtothercharges.Text == null || txtothercharges.Text == "") ? 0 : Convert.ToDecimal(txtothercharges.Text));
                    cmd.Parameters.AddWithValue("@PF_Charges", (txtPFCharges.Text == null || txtPFCharges.Text == "") ? 0 : Convert.ToDecimal(txtPFCharges.Text));

                    cmd.Parameters.AddWithValue("@Tot_TaxableValue", (txtTot_TaxableValue.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTot_TaxableValue.Text));
                    cmd.Parameters.AddWithValue("@Tot_CGST_Amnt", (txtTot_CGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_CGST.Text));
                    cmd.Parameters.AddWithValue("@Tot_SGST_Amnt", (txtTot_SGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_SGST.Text));
                    cmd.Parameters.AddWithValue("@Tot_IGST_Amnt", (txtTot_IGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_IGST.Text));
                    cmd.Parameters.AddWithValue("@Total_Amount", (txtTotal_Amt.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTotal_Amt.Text));
                    cmd.Parameters.AddWithValue("@Tcs_Per", (txttcsper.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txttcsper.Text));
                    cmd.Parameters.AddWithValue("@Tcs_Amount", (txttcsAmnt.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txttcsAmnt.Text));

                    cmd.Parameters.AddWithValue("@Tot_Ord_Value", (txtTot_OrderValue.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_OrderValue.Text));
                    cmd.Parameters.AddWithValue("@TDS_Per", (txtTDSPer.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTDSPer.Text));
                    cmd.Parameters.AddWithValue("@TDS_Amount", (txtTDSAmount.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTDSAmount.Text));
                    cmd.Parameters.AddWithValue("@Net_GRN_Amount", (txtNetGRNValue.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtNetGRNValue.Text));
                    cmd.Parameters.AddWithValue("@Cess_Per", (txtCessPetMT.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtCessPetMT.Text));
                    cmd.Parameters.AddWithValue("@Cess_Amount", (txtCessAmount.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtCessAmount.Text));
                    cmd.Parameters.AddWithValue("@Rounding", (txtRounding.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtRounding.Text));
                    int cparty = 0;
                    if (cmbConvPartyName.SelectedValue == null)
                    {
                        cparty = 0;
                    }
                    else
                    {
                        cparty = Convert.ToInt32(cmbConvPartyName.SelectedValue.ToString());

                    }
                    cmd.Parameters.AddWithValue("@Conv_Party", cparty);
                    int WhCode = 0;
                    if (cmbWareHouse.SelectedValue == null)
                    {
                        WhCode = 0;
                    }
                    else
                    {
                        WhCode = Convert.ToInt32(cmbWareHouse.SelectedValue.ToString());

                    }
                    cmd.Parameters.AddWithValue("@Warehouse_Code", WhCode);

                    cmd.Parameters.AddWithValue("@Transporter_Name", (cmbOtherTermsandNotes.Text == "") ? "" : cmbOtherTermsandNotes.Text);
                    cmd.Parameters.AddWithValue("@Other_Terms", (txttransportname.Text == "") ? "" : txttransportname.Text);


                    cmd.Parameters.AddWithValue("@Vehicle_No", (txtvehicalnr.Text == "") ? "" : txtvehicalnr.Text);
                    cmd.Parameters.AddWithValue("@LrNo_LrDate", (txtlrnodate.Text == "") ? "" : txtlrnodate.Text);


                    cmd.Parameters.AddWithValue("@Status", Convert.ToInt32(cmbStatus.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@Received_For_Conversion", chkConversion.Checked);
                    cmd.Parameters.AddWithValue("@Frieght_Paid", (txtFrieghtPaid.Text == null || txtFrieghtPaid.Text == "") ? 0 : Convert.ToDecimal(txtFrieghtPaid.Text));
                    cmd.Parameters.AddWithValue("@Hamali_Charges", (txtHamali.Text == null || txtHamali.Text == "") ? 0 : Convert.ToDecimal(txtHamali.Text));

                    cmd.Parameters.AddWithValue("@BU_ID", logIn.BU_ID);
                    cmd.Parameters.AddWithValue("@Company_ID", logIn.company);
                    cmd.Parameters.AddWithValue("@Created_By", lblCreatedBy.Text);
                    cmd.Parameters.AddWithValue("@Modified_By", logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt"));

                    cmd.Parameters.AddWithValue("@isDeleted", false);
                    
                    
                    
                    string Prod_Code = "";                   
                    string Prod_Spec = "";
                    string Prod_Grade = "";                   
                    string PO_Qty = "";
                    string Challan_Qty = "";
                    string ReceivedQty = "";
                    string Tole_Qty = "";
                    string RejectedQty = "";
                    string AcceptedQty = "";
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
                    string TCNo = "";
                    string Heat_No = "";
                    string PR_NO = "";
                    string PO_No = "";
                    string Remarks = "";
                    string ProdSno = "";
                    string Int_Batch_No = "";
                    int rowcount = 0;
                    int PSno = 0;
                    decimal PrQty = 0;
                    for (int i = 0; i < dgProducts.RowCount - 1; i++)
                    {

                        Prod_Code = Prod_Code + Convert.ToString(dgProducts.Rows[i].Cells["Item_Code"].Value).PadRight(14);
                        //Product_Description = Product_Description + Convert.ToString(dgProducts.Rows[i].Cells["Item_Description"].Value).PadRight(50); 
                        Prod_Spec = Prod_Spec + Convert.ToString(dgProducts.Rows[i].Cells["Item_spec"].Value).PadRight(50);
                        Prod_Grade = Prod_Grade + Convert.ToString(dgProducts.Rows[i].Cells["Item_Grade"].Value).PadRight(50);
                        
                        if (dgProducts.Rows[i].Cells["PO_Qty"].Value == DBNull.Value || dgProducts.Rows[i].Cells["PO_Qty"].Value == null)
                        {
                            PO_Qty = PO_Qty + "0";
                        }
                        else
                        {
                            PO_Qty = PO_Qty + Convert.ToString(dgProducts.Rows[i].Cells["PO_Qty"].Value).PadRight(14);
                        }
                        Challan_Qty = Challan_Qty + Convert.ToString(dgProducts.Rows[i].Cells["Inv_Qty"].Value).PadRight(14);
                        ReceivedQty = ReceivedQty + Convert.ToString(dgProducts.Rows[i].Cells["ReceivedQty"].Value).PadRight(14);
                        //Tole_Qty = Tole_Qty + Convert.ToString(dgProducts.Rows[i].Cells["Tole_Qty"].Value).PadRight(14);
                        if (dgProducts.Rows[i].Cells["Tole_Qty"].Value == DBNull.Value || dgProducts.Rows[i].Cells["Tole_Qty"].Value == null)
                        {
                            Tole_Qty = Tole_Qty  + "0";
                        }
                        else
                        {
                            Tole_Qty = Tole_Qty + Convert.ToString(dgProducts.Rows[i].Cells["Tole_Qty"].Value).PadRight(14);
                        }
                        RejectedQty = RejectedQty + Convert.ToString(dgProducts.Rows[i].Cells["RejectedQty"].Value).PadRight(14);
                        AcceptedQty = AcceptedQty + Convert.ToString(dgProducts.Rows[i].Cells["AcceptedQty"].Value).PadRight(14);
                        Price = Price + Convert.ToString(dgProducts.Rows[i].Cells["Basic_Price"].Value).PadRight(14);
                        Amount = Amount + Convert.ToString(dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value).PadRight(14);
                        Disc_Per = Disc_Per + Convert.ToString(dgProducts.Rows[i].Cells["Disc_Per"].Value).PadRight(14);
                        Disc_Amount = Disc_Amount + Convert.ToString(dgProducts.Rows[i].Cells["Disc_Amt"].Value).PadRight(14);
                        Taxable_Value = Taxable_Value + Convert.ToString(dgProducts.Rows[i].Cells["Taxable_Value"].Value).PadRight(14);
                        CGST_Per = CGST_Per + Convert.ToString(dgProducts.Rows[i].Cells["CGST_Per"].Value).PadRight(14);
                        SGST_Per = SGST_Per + Convert.ToString(dgProducts.Rows[i].Cells["SGST_Per"].Value).PadRight(14);
                        IGST_Per = IGST_Per + Convert.ToString(dgProducts.Rows[i].Cells["IGST_Per"].Value).PadRight(14);
                        CGST_Amnt = CGST_Amnt + Convert.ToString(dgProducts.Rows[i].Cells["CGST_Amt"].Value).PadRight(14);
                        SGST_Amnt = SGST_Amnt + Convert.ToString(dgProducts.Rows[i].Cells["SGST_Amt"].Value).PadRight(14);
                        IGST_Amnt = IGST_Amnt + Convert.ToString(dgProducts.Rows[i].Cells["IGST_Amt"].Value).PadRight(14);
                        Net_Amount = Net_Amount + Convert.ToString(dgProducts.Rows[i].Cells["Total_Amount"].Value).PadRight(14);
                        PO_No = PO_No + Convert.ToString(dgProducts.Rows[i].Cells["PO_No"].Value).PadRight(20);
                        TCNo = TCNo + Convert.ToString(dgProducts.Rows[i].Cells["TCNo"].Value).PadRight(20);
                        Heat_No = Heat_No + Convert.ToString(dgProducts.Rows[i].Cells["Heat_No"].Value).PadRight(20);
                        PR_NO = PR_NO + Convert.ToString(dgProducts.Rows[i].Cells["PR_NO"].Value).PadRight(20);
                        Int_Batch_No = Int_Batch_No + Convert.ToString(dgProducts.Rows[i].Cells["Int_Batch_No"].Value).PadRight(20);
                        Remarks = Remarks + Convert.ToString(dgProducts.Rows[i].Cells["Remarks"].Value).PadRight(50);
                        //PSno = PSno +Convert.ToInt32( dgProducts.Rows[i].Cells["S_No"].Value);
                        if (dgProducts.Rows[i].Cells["S_no"].Value == null || dgProducts.Rows[i].Cells["S_no"].Value.ToString() == "")
                        {

                            if (PSno == 0)
                            {
                                ProdSno = ProdSno + Convert.ToString(i + 1).PadRight(14);
                                PSno = i + 1;
                            }
                            else
                            {
                                ProdSno = ProdSno + Convert.ToString(PSno + 1).PadRight(14);
                                PSno = PSno + 1;
                            }
                        }
                        else
                        {
                            ProdSno = ProdSno + Convert.ToString(dgProducts.Rows[i].Cells["S_no"].Value).PadRight(14);
                            PSno = Convert.ToInt32(dgProducts.Rows[i].Cells["S_no"].Value);
                        }

                        //Company_ID = logIn.company;
                        rowcount += 1;
                    }

                    cmd.Parameters.AddWithValue("@txt_Prod_Code", Prod_Code);
                    
                    cmd.Parameters.AddWithValue("@txt_Prod_Spec", Prod_Spec);
                    cmd.Parameters.AddWithValue("@txt_Prod_Grade", Prod_Grade);
                    cmd.Parameters.AddWithValue("@txt_PO_Qty", PO_Qty);
                    cmd.Parameters.AddWithValue("@txt_Challan_Qty", Challan_Qty);
                    cmd.Parameters.AddWithValue("@txt_ReceivedQty", ReceivedQty);
                    cmd.Parameters.AddWithValue("@txt_Tole_Qty", Tole_Qty);
                    cmd.Parameters.AddWithValue("@txt_RejectedQty", RejectedQty);
                    cmd.Parameters.AddWithValue("@txt_AcceptedQty", AcceptedQty);
                    cmd.Parameters.AddWithValue("@txt_Price", Price);
                    cmd.Parameters.AddWithValue("@txt_Amount", Amount);
                    cmd.Parameters.AddWithValue("@txt_Disc_Per", Disc_Per);
                    cmd.Parameters.AddWithValue("@txt_Disc_Amount", Disc_Amount);
                    cmd.Parameters.AddWithValue("@txt_Taxable_Value", Taxable_Value);
                    cmd.Parameters.AddWithValue("@txt_CGST_Per", CGST_Per);
                    cmd.Parameters.AddWithValue("@txt_CGST_Amnt", CGST_Amnt);
                    cmd.Parameters.AddWithValue("@txt_SGST_Amnt", SGST_Amnt);
                    cmd.Parameters.AddWithValue("@txt_SGST_Per", SGST_Per);
                    cmd.Parameters.AddWithValue("@txt_IGST_Per", IGST_Per);
                    cmd.Parameters.AddWithValue("@txt_IGST_Amnt", IGST_Amnt);
                    cmd.Parameters.AddWithValue("@txt_Net_Amount", Net_Amount);
                    cmd.Parameters.AddWithValue("@txt_Remarks", Remarks);
                    cmd.Parameters.AddWithValue("@txt_TCNo", TCNo);
                    cmd.Parameters.AddWithValue("@txt_Heat_No", Heat_No);
                    cmd.Parameters.AddWithValue("@txt_PR_NO", PR_NO);
                    cmd.Parameters.AddWithValue("@txt_PO_No", PO_No);
                    cmd.Parameters.AddWithValue("@txt_Int_Batch_No", Int_Batch_No);
                    cmd.Parameters.AddWithValue("@txt_ProdSno", ProdSno);
                    cmd.Parameters.AddWithValue("@gridcount", rowcount);

                    try
                    {
                        con.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (Convert.ToBoolean(result))
                        {
                            MessageBox.Show("Record has been successfully saved..");
                            this.Close();

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
                if(txtSoNo.Text !="")
                //if ((from u in db.GoodsReceiptNote_Masters where u.Grn_NO == myString && u.Company_ID == logIn.company select u).Count() > 0)
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

                //dgProducts.Enabled = true;
                //var S = db.Sale_Order_Masters.Where(w => w.SO_NO == myString && w.Company_ID == logIn.company).FirstOrDefault();
               GoodsReceiptNote_Master S = new GoodsReceiptNote_Master();
                {
                    S.GRN_Type = cmbTaxClass.Text;
                    S.Grn_NO = myString;
                    S.Grn_Date = dpSODate.Value;
                    S.Inward_No = txtInwardNo.Text;
                    S.Inward_Date = dtInwardDate.Value;
                    S.Purchase_Basis =(cmbPurchaseBasis.Text);

                    S.SupplierName = Convert.ToInt32(CmbSuplierName.SelectedValue.ToString());
                    if (cmbPurchaseAccount.Text != "" && cmbPurchaseAccount.Text != "NA")
                    {
                        S.Purchase_Account = Convert.ToInt32(cmbPurchaseAccount.SelectedValue.ToString());
                    }
                    else
                    {
                        S.Purchase_Account = 7210;
                    }
                   
                    S.GRN_Type = cmbTaxClass.Text;
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
                    S.PF_Charges = (txtPFCharges.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtPFCharges.Text);
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
                    S.TDS_Per = (txtTDSPer.Text == "" || txtTDSPer.Text == null) ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTDSPer.Text);
                    S.TDS_Amount = (txtTDSAmount.Text == "" || txtTDSAmount.Text == null) ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTDSAmount.Text);
                    S.Net_GRN_Amount = (txtNetGRNValue.Text == "" || txtNetGRNValue.Text == null) ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtNetGRNValue.Text);
                    if (cmbWareHouse.SelectedValue == null)
                    {
                        S.Warehouse_Code = 0;
                    }
                    else
                    {
                        S.Warehouse_Code = Convert.ToInt32(cmbWareHouse.SelectedValue.ToString());

                    }
                    S.isDeleted = false;
                    S.Cess_Per = (txtCessPetMT.Text == "" || txtCessPetMT.Text == null) ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtCessPetMT.Text);
                    S.Cess_Amount = (txtCessAmount.Text == "" || txtCessAmount.Text == null) ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtCessAmount.Text);
                    S.Vehicle_No = (txtvehicalnr.Text == "") ? "" : txtvehicalnr.Text;
                    S.LrNo_LrDate = (txtlrnodate.Text == "") ? "" : txtlrnodate.Text;
                    S.Transporter_Name = (cmbOtherTermsandNotes.Text == "") ? "" : cmbOtherTermsandNotes.Text;
                    S.Other_Terms = (cmbOtherTermsandNotes.Text==""||cmbOtherTermsandNotes.Text==null)?"": cmbOtherTermsandNotes.Text;
                   // S.ConsigneeName = Convert.ToInt32(1);
                    S.Status = Convert.ToInt32(cmbStatus.SelectedValue.ToString());
                    //S.RCM = chkRCM.Checked; 
                    S.IneligibleTax = chkConversion.Checked;
                    S.Frieght_Paid = (txtFrieghtPaid.Text == "" || txtFrieghtPaid.Text == null) ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtFrieghtPaid.Text);
                    S.Hamali_Charges = (txtHamali.Text == "" || txtHamali.Text == null) ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtHamali.Text);

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
                    var d1 = (from a in db.GoodsReceiptNote_Masters where a.Grn_NO == myString && a.isDeleted ==false && a.Company_ID == logIn.company && a.BU_ID == logIn.BU_ID select new { a.Id }).ToList();
                    SC.GRN_Master_ID = d1[0].Id;
                    SC.Grn_NO = myString;
                    SC.Prod_Code = (dgProducts.Rows[i].Cells["Item_Code"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value);                    
                    //SC.Product_Description = (dgProducts.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value).ToString();
                    SC.Prod_Spec = (dgProducts.Rows[i].Cells["Item_spec"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_spec"].Value).ToString();
                    SC.Prod_Grade = (dgProducts.Rows[i].Cells["Item_Grade"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value).ToString();
                    //SC.HSN_Code = (dgProducts.Rows[i].Cells["HSN_Code"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["HSN_Code"].Value.ToString();
                    //SC.Uom = (dgProducts.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["uom"].Value.ToString();
                    SC.PO_Qty = (dgProducts.Rows[i].Cells["PO_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["PO_Qty"].Value);
                    SC.Challan_Qty = (dgProducts.Rows[i].Cells["Inv_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Inv_Qty"].Value);
                    SC.ReceivedQty = (dgProducts.Rows[i].Cells["ReceivedQty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["ReceivedQty"].Value);
                    SC.Tole_Qty = (dgProducts.Rows[i].Cells["Tole_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Tole_Qty"].Value);

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
                    SC.TCNo = (dgProducts.Rows[i].Cells["TCNo"].Value == null) ? "" : (dgProducts.Rows[i].Cells["TCNo"].Value).ToString();
                    SC.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value).ToString();
                    SC.Int_Batch_No = (dgProducts.Rows[i].Cells["Int_Batch_No"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Int_Batch_No"].Value).ToString();
                    SC.ProdSno = Convert.ToInt32(dgProducts.Rows[i].Cells["S_No"].Value) ;
                    SC.Company_ID = logIn.company;
                    db.GoodsReceiptNote_Childs.InsertOnSubmit(SC);
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
                //MessageBox.Show("Record Saved / Updated Successfully With Transaction Ref No : " + txtSoNo.Text);
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
                txtSoNo.Text =GoodsReceiptNoteList.SO_No;
                String myString = "";
                myString = txtSoNo.Text;
                var da = (from obj in db.GoodsReceiptNote_Masters
                          where obj.Grn_NO == txtSoNo.Text && obj.Company_ID == logIn.company && obj.BU_ID == logIn.BU_ID && obj.isDeleted ==false
                          select obj).ToList();

                if (da.Count > 0)
                {
                    cmbTaxClass.Text = da[0].GRN_Type.ToString();
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
                    txtPFCharges.Text = Convert.ToString(da[0].PF_Charges);
                    txttcsAmnt.Text = Convert.ToString(da[0].Tcs_Amount);
                    txttcsper.Text = Convert.ToString(da[0].Tcs_Per);
                    txtTotal_Amt.Text = da[0].Total_Amount.ToString();
                    txtTDSAmount.Text = da[0].TDS_Amount.ToString();
                    txtNetGRNValue.Text = da[0].Net_GRN_Amount.ToString();
                    cmbWareHouse.Text = da[0].Warehouse_Code.ToString();
                    txtvehicalnr.Text = da[0].Vehicle_No;
                    txtlrnodate.Text = da[0].LrNo_LrDate;
                    cmbOtherTermsandNotes.Text = da[0].Transporter_Name;
                    txttransportname.Text = da[0].Other_Terms;
                    //cmbTaxClass.SelectedValue = da[0].TAX_Class;
                    if (da[0].Purchase_Account != null)
                    {
                        cmbPurchaseAccount.SelectedValue = da[0].Purchase_Account;
                    }
                    if (da[0].Conv_Party != null)
                    {
                        cmbConvPartyName.SelectedValue = da[0].Conv_Party;
                    }
                    //if (da[0].RCM != null)
                    //{
                    //    chkRCM.Checked = da[0].RCM.Value;
                    //}
                    if (da[0].IneligibleTax != null)
                    {
                        chkConversion.Checked = da[0].IneligibleTax.Value;
                    }
                    cmbStatus.SelectedValue = da[0].Status;      
                    lblCreatedBy.Text = da[0].Created_By;
                    lblModified.Text = da[0].Modified_By;
                    txtFrieghtPaid.Text = da[0].Frieght_Paid.ToString();
                    txtHamali.Text = da[0].Hamali_Charges.ToString();
                    txtTDSPer.Text = da[0].TDS_Per.ToString();
                    txtCessPetMT.Text = da[0].Cess_Per.ToString();
                    txtCessAmount.Text = da[0].Cess_Amount.ToString();
                }

                if(logIn.company == 1044)
                {
                    var dm1 = (from s in db.GoodsReceiptNote_Childs
                               join g in db.GoodsReceiptNote_Masters on s.GRN_Master_ID equals g.Id
                               join pr in db.Products on s.Prod_Code equals pr.prod_ID
                               join u in db.UoM_Masters on pr.Prod_Primary_UOM_Id equals u.UOM_ID
                               where s.Grn_NO == myString && s.Company_ID == logIn.company && g.isDeleted == false && g.BU_ID == logIn.BU_ID


                               select new

                               {
                                   S_No = s.ProdSno,
                                   Prod_Code = pr.Prod_Alternative_Code,
                                   Item_Code = s.Prod_Code,
                                   Item_Description = pr.Prod_Name,
                                   Item_Spec = s.Prod_Spec,
                                   Item_Grade = s.Prod_Grade,
                                   HSN_Code = pr.Prod_HSN_Code,
                                   UOM = u.Uom_Descr,
                                   s.PO_Qty,
                                   Inv_Qty = s.Challan_Qty,
                                   s.Tole_Qty,
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
                                   s.Int_Batch_No,
                                   s.Remarks
                               });
                    SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    DataTable dtr = new DataTable();
                    da2.Fill(dtr);
                    if (dtr.Rows.Count >= 0)
                        dgProducts.DataSource = dtr;
                }
                else
                { 
                var dm1 = (from s in db.GoodsReceiptNote_Childs
                           join g in db.GoodsReceiptNote_Masters on s.GRN_Master_ID equals g.Id
                           join pr in db.Products on s.Prod_Code equals pr.prod_ID
                           join u in db.UoM_Masters on pr.Prod_Primary_UOM_Id equals u.UOM_ID
                           where s.Grn_NO == myString && s.Company_ID == logIn.company  && g.isDeleted==false && g.BU_ID == logIn.BU_ID
                           select new
                           {
                               S_No=s.ProdSno,
                               Prod_Code = pr.Prod_Code,
                               Item_Code = s.Prod_Code,
                               Item_Description = pr.Prod_Name,
                               Item_Spec = s.Prod_Spec,
                               Item_Grade = s.Prod_Grade,
                               HSN_Code = pr.Prod_HSN_Code,
                               UOM = u.Uom_Descr,                              
                               s.PO_Qty,                               
                               Inv_Qty= s.Challan_Qty,
                               s.Tole_Qty,
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
                               s.Int_Batch_No,
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
