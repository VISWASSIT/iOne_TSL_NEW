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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Diagnostics;
using Ione_DAL;
namespace ioneNet.OrderManagement.Transactions
{
    public partial class frmSaleReturns : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();
        public static string SONo,ItemCode,OrdQty, CR_No_for_EInv;
        private Database crDatabase;
        private Tables crTables;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        private string path;
        public frmSaleReturns()
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
            AutoincrementId();
            if (ListOfProformaInvoices.editMode == true)
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
                if (txtVchNo.Text == string.Empty)
                {
                    MessageBox.Show("Invoice No Should Not Be Empty", "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CmbBuyerName.Focus();
                    return;
                }
                if (CmbBuyerName.Text == string.Empty)
                {
                    MessageBox.Show("Customer Name Should Not Be Empty", "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CmbBuyerName.Focus();
                    return;
                }
                else if (txtOurInvNo.Text == string.Empty)
                {
                    MessageBox.Show("Customer Ref No Should Not Be Empty");
                    txtOurInvNo.Focus();
                    return;
                }
                                
                //else if (cmbSaleAccount.Text == string.Empty)
                //{
                //    MessageBox.Show("Sale Account is Not Selected,");
                //    cmbSaleAccount.Focus();
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
                    //cmbStatus.DataSource = pStatus;
                    //cmbStatus.ValueMember = "ID";
                    //cmbStatus.DisplayMember = "Descr";
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
                    //cmbTaxClass.DisplayMember = "Tax_Class_Name";
                    //cmbTaxClass.ValueMember = "ID";
                    //cmbTaxClass.DataSource = bindLoc;

                }

                //Sales Account
                var d = (from po in db.AccountMasters
                         join A in db.AccountGroups on po.AccGroup_ID equals A.ID
                         where po.Company_ID == logIn.company && A.GroupType =="Income" 
                select new { po.id, po.AccName }).Distinct().ToList();
                if (d.Count > 0)
                {
                    cmbSaleAccount.DataSource = d;
                    cmbSaleAccount.ValueMember = "id";
                    cmbSaleAccount.DisplayMember = "AccName";
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

                var result = db.Sp_autoincrement_SaleReturns (logIn.company,logIn.fy_Start_Date,logIn.fy_End_Date,logIn.BU_ID);
                txtVchNo.Text = result.FirstOrDefault().Vch_No;
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
                MessageBox.Show(ex.Message);
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
                
                DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].Name;
               
                if (columnName == "Item_Description")
                {

                    decimal d;                    
                    //c = dgProducts.Rows[dgProducts.CurrentRow.Index];
                    //R1.Cells["Item_Code"].Value = dgProducts.CurrentCell.RowIndex+1;
                    //var State = (from c1 in db.Tax_Class_Masters
                    //             where c1.ID == Convert.ToInt32(cmbTaxClass.SelectedValue)
                    //             select new { c1.Gst_Rate }).ToList();
                    //if (State.Count > 0)
                    //{
                    //    taxRate = Convert.ToDecimal(State[0].Gst_Rate);
                    //}
                    //else
                    //{
                    //    taxRate = 18;
                    //}
                     //Convert.ToInt32(getProduct_Name.GSTRate);
                    
                    //var d1 = (from a in db.Company_Infos where a.Id == logIn.company select new { a.GST_No }).ToList();
                    //if (d1.Count > 0)
                    //{
                    //    comnpstatecode = Mid(d1[0].GST_No, 1, 2);
                    //    suppStateCode = Mid(txtCustGSTNo.Text, 1, 2);
                    //    if (cmbInvType.Text != "SEZ Invoice")
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
                    decimal POQty = (R1.Cells["Inv_Qty"].Value == "" || R1.Cells["Inv_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Inv_Qty"].Value);
                    //decimal RejectedQty = (R1.Cells["Qty_Rejected"].Value == "" || R1.Cells["Qty_Rejected"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty_Rejected"].Value);

                                     

                    if (columnName == "Return_Qty")
                    {
                        
                        decimal AcceptedQty = (R1.Cells["Return_Qty"].Value == "" || R1.Cells["Return_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Return_Qty"].Value);
                        if(AcceptedQty >POQty)
                        {
                            MessageBox.Show("Return Qty Cannot Be Greater Than Invoice Qty");
                        }
                        else
                        { 
                        decimal price = (R1.Cells["Basic_Price"].Value == "" || R1.Cells["Basic_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Basic_Price"].Value);
                        decimal DiscPer = (R1.Cells["Disc_Per"].Value == "" || R1.Cells["Disc_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Disc_Per"].Value);

                        //Get Tax rates
                        decimal b, c, d;                    
                        
                        decimal Amt, DiscAmt, netAmt, gst, igst, totamt;

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
                if(txtOurInvNo.Text!="")
                {
                    GetOrderInfo();
                }
            }
            catch
            {

            }
        }
        public void GetTot()
        {
            decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
            for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
            {

                x += (dgProducts.Rows[i].Cells["Return_Qty"].Value == "" || dgProducts.Rows[i].Cells["Return_Qty"].Value == null || dgProducts.Rows[i].Cells["Return_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Return_Qty"].Value);
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
        public void Save()
        {
            try
            {
                String myString = "";
                myString = txtVchNo.Text;
                if ((from u in db.SaleReturns_Masters where u.Vch_No == myString && u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID select u).Count() > 0)
                {
                    myString = txtVchNo.Text;
                    db.sp_SaleReturn_Delete(myString, logIn.company,logIn.BU_ID);
                }
                else
                {
                    AutoincrementId();
                    myString = txtVchNo.Text;

                }
                    //if (frmGate.Modify.Contains(this.Text))
                    //{

               // dgProducts.Enabled = false;
                //var S = db.Sale_Order_Masters.Where(w => w.SO_NO == myString && w.Company_ID == logIn.company).FirstOrDefault();
                SaleReturns_Master S = new SaleReturns_Master();
                {
                    S.Inv_No = myString;
                    S.Vch_No = myString;
                    S.Vch_Date = dpInvDate.Value;
                    //S.InvType = cmbInvType.Text;
                   // S.Tax_Class = Convert.ToInt32(cmbTaxClass.SelectedValue.ToString());
                    S.BuyerName = Convert.ToInt32(CmbBuyerName.SelectedValue.ToString());
                    S.ConsigneeName = Convert.ToInt32(CmbConsigneeName.SelectedValue.ToString());
                    S.ConsigneeAddress = txtConAddress.Text;
                    S.Con_GST_No = txtConGSTNo.Text;
                    //S.Status = Convert.ToInt32(cmbStatus.SelectedValue.ToString()); 
                    S.TotalQty = (txtTotalQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotalQty.Text);
                    S.SubTotal = (txtSubTotal.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtSubTotal.Text);
                    S.Tot_Discount = (txtTotDiscount.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotDiscount.Text);
                    S.Tot_TaxableValue = (txtTot_TaxableValue.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTot_TaxableValue.Text);
                    S.Inv_No = (txtOurInvNo.Text == "") ? "" : txtOurInvNo.Text;                            
                    S.CustRefDate = dpPODate.Value;
                    S.CustomerRefNo = txtSoNo.Text;
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
                    if (cmbSaleAccount.Text != "")
                    {
                        S.Sale_Account = Convert.ToInt32(cmbSaleAccount.SelectedValue.ToString());
                    }
                    else
                    {
                        S.Sale_Account = 7210;
                    }
                   // S.Transporter_Name = (cmbTransporter.Text == "") ? "" : cmbTransporter.Text;                                        
                   
                   // S.DespatchThrough = (txtDespthrough.Text == "") ? "" : txtDespthrough.Text;
                    S.VehicleNo =   (txtVehicleNo.Text == "") ? "" : txtVehicleNo.Text;
                   // S.WayBillNo = (txtWayBillNo.Text == "") ? "" : txtWayBillNo.Text;
                    S.InvValueInWords = (txtLRNo.Text == "") ? "" : txtLRNo.Text;
                    S.Spl_Instructions = (txtSplInstructions.Text == "") ? "" : txtSplInstructions.Text;
                    
                   // S.LR_Date = dtLRDate.Value;
                    S.Company_ID = logIn.company;
                    S.BU_ID = logIn.BU_ID;
                    S.Created_By = lblCreatedBy.Text;                    
                    S.Modified_By = logIn.username + "-" + DateTime.Now;
                    db.SaleReturns_Masters.InsertOnSubmit(S);
                    db.SubmitChanges();
                }
                        //db.Transaction = transaction;
                for (int i = 0; i < dgProducts.RowCount - 1; i++)
                {
                    SaleReturns_Child SC = new SaleReturns_Child();
                    var d1 = (from a in db.SaleReturns_Masters where a.Vch_No == myString && a.Company_ID == logIn.company && a.BU_ID == logIn.BU_ID select new { a.Id }).ToList();
                    SC.SR_Master_ID = d1[0].Id;
                    SC.Vch_No = myString;
                    SC.Prod_Code = (dgProducts.Rows[i].Cells["Item_Code"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Code"].Value).ToString();
                    SC.Product_Description = (dgProducts.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value).ToString();
                    SC.Prod_Grade = (dgProducts.Rows[i].Cells["Item_Grade"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value).ToString();
                    SC.Uom = (dgProducts.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["uom"].Value.ToString();
                    SC.Inv_Qty = (dgProducts.Rows[i].Cells["Inv_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Inv_Qty"].Value);

                    SC.Return_Qty = (dgProducts.Rows[i].Cells["Return_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Return_Qty"].Value);
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
                    SC.Inv_Ref_No = (dgProducts.Rows[i].Cells["Inv_Ref_No"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Inv_Ref_No"].Value).ToString();
                    SC.Item_No = i+1;

                    SC.Company_ID = logIn.company;
                    db.SaleReturns_Childs.InsertOnSubmit(SC);
                }
                db.SubmitChanges();
                //transaction.Commit();                
                MessageBox.Show("Sale Returns Saved / Updated Successfully With Transaction Ref No : " + txtVchNo.Text);
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
                    if (txtOurInvNo.Text != "Multi")
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

        private void btnCancel_Click(object sender, EventArgs e)
        {
           // groupBox2.Visible = false;
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {

              
                //var p = (from obj in db.Sale_Order_Masters
                //         join C in db.Customer_informations on obj.BuyerName equals C.ID
                //          where obj.Status == 6 && obj.Company_ID == logIn.company && SqlMethods.Like(C.Customer_Name, "%" + txtSearch.Text + "%")
                          
                //             select new
                //         {
                //             C.Customer_Name,
                //             obj.CustomerPONo,
                //             obj.SO_NO


                //         }
                //        );
                //SqlCommand cmd1 = (SqlCommand)db.GetCommand(p);
                //SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                //DataTable dt1 = new DataTable();
                //da1.Fill(dt1);

                //if (dt1.Rows.Count > 0)
                //{
                //    sfDataGrid1.DataSource = dt1;
                //}
                //else
                //{
                //    MessageBox.Show("No Orders Found on Entered Customer");
                //    sfDataGrid1.DataSource = dt1;
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmdPrevOrder_Click(object sender, EventArgs e)
        {
            //var d = (from data in db.ShowSOList_Invoicing(logIn.company, Convert.ToInt32(CmbBuyerName.SelectedValue)) select data).ToList();

            //if (d.Count > 0)
            //{
            //    //dgProductsList.DataSource = d;
            //    sfDataGrid1.DataSource = d;
            //}

            //groupBox2.Visible = true;
            //txtSearch.Focus();
        }
       // System.Data.DataRow drgetproducts;
        //DataTable dtexisting = new DataTable();
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                //Check Whether Exisitng Products Already Selected in Main Grid
                //if (dgProducts.Rows.Count > 1)
                //{
                //    dtexisting.Rows.Clear();
                //    dtexisting.Columns.Clear();
                //    dtexisting.Columns.Add("Item_Code", typeof);
                //    dtexisting.Columns.Add("Item_Description", typeof);
                //    dtexisting.Columns.Add("Item_Grade", typeof);
                //    dtexisting.Columns.Add("UOM", typeof);
                //    dtexisting.Columns.Add("PO_Qty", typeof(decimal));
                //    dtexisting.Columns.Add("Stock_Qty", typeof(decimal));
                //    dtexisting.Columns.Add("Inv_Qty", typeof(decimal));
                //    dtexisting.Columns.Add("Basic_Price", typeof(decimal));
                //    dtexisting.Columns.Add("Amt_Before_Disc", typeof(decimal));
                //    dtexisting.Columns.Add("Disc_Per", typeof(decimal));
                //    dtexisting.Columns.Add("Disc_Amt", typeof(decimal));
                //    dtexisting.Columns.Add("Taxable_Value", typeof(decimal));
                //    dtexisting.Columns.Add("CGST_Per", typeof(decimal));
                //    dtexisting.Columns.Add("CGST_Amt", typeof(decimal));
                //    dtexisting.Columns.Add("SGST_Per", typeof(decimal));
                //    dtexisting.Columns.Add("SGST_Amt", typeof(decimal));
                //    dtexisting.Columns.Add("IGST_Per", typeof(decimal));
                //    dtexisting.Columns.Add("IGST_Amt", typeof(decimal));
                //    dtexisting.Columns.Add("Total_Amount", typeof(decimal));
                //    dtexisting.Columns.Add("Remarks", typeof);
                //    dtexisting.Columns.Add("SO_Ref_No", typeof);
                //    //
                //    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                //    {
                //        DataRow dr;
                //        dr = dtexisting.NewRow();
                //        dr["Item_Code"] = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                //        dr["Item_Description"] = dgProducts.Rows[i].Cells["Item_Description"].Value.ToString();
                //        dr["Item_Grade"] = dgProducts.Rows[i].Cells["Item_Grade"].Value.ToString();
                //        dr["UOM"] = dgProducts.Rows[i].Cells["UOM"].Value.ToString();
                //        dr["PO_Qty"] = dgProducts.Rows[i].Cells["PO_Qty"].Value.ToString();
                //        dr["Stock_Qty"] = dgProducts.Rows[i].Cells["Stock_Qty"].Value.ToString();
                //        dr["Inv_Qty"] = dgProducts.Rows[i].Cells["Inv_Qty"].Value.ToString();
                //        dr["Basic_Price"] = dgProducts.Rows[i].Cells["Basic_Price"].Value.ToString();
                //        dr["Amt_Before_Disc"] = dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value.ToString();
                //        dr["Disc_Per"] = dgProducts.Rows[i].Cells["Disc_Per"].Value.ToString();
                //        dr["Disc_Amt"] = dgProducts.Rows[i].Cells["Disc_Amt"].Value.ToString();
                //        dr["Taxable_Value"] = dgProducts.Rows[i].Cells["Taxable_Value"].Value.ToString();
                //        dr["CGST_Per"] = dgProducts.Rows[i].Cells["CGST_Per"].Value.ToString();
                //        dr["CGST_Amt"] = dgProducts.Rows[i].Cells["CGST_Amt"].Value.ToString();
                //        dr["SGST_Per"] = dgProducts.Rows[i].Cells["SGST_Per"].Value.ToString();
                //        dr["SGST_Amt"] = dgProducts.Rows[i].Cells["SGST_Amt"].Value.ToString();
                //        dr["IGST_Per"] = dgProducts.Rows[i].Cells["IGST_Per"].Value.ToString();
                //        dr["IGST_Amt"] = dgProducts.Rows[i].Cells["IGST_Amt"].Value.ToString();
                //        dr["Total_Amount"] = dgProducts.Rows[i].Cells["Total_Amount"].Value.ToString();
                //        dr["Remarks"] = dgProducts.Rows[i].Cells["Remarks"].Value.ToString();
                //        dr["SO_Ref_No"] = dgProducts.Rows[i].Cells["SO_Ref_No"].Value.ToString();
                //        dtexisting.Rows.Add(dr);

                //    }
                //    dtexisting.AcceptChanges();
                //}



                //dtgetproducts.Columns.Clear();
                //dtgetproducts.Rows.Clear();
                //dtgetproducts.Columns.Add("Item_Code", typeof);
                //dtgetproducts.Columns.Add("Item_Description", typeof);
                //dtgetproducts.Columns.Add("Item_Grade", typeof);
                //dtgetproducts.Columns.Add("UOM", typeof);
                //dtgetproducts.Columns.Add("PO_Qty", typeof(decimal));
                //dtgetproducts.Columns.Add("Stock_Qty", typeof(decimal));
                //dtgetproducts.Columns.Add("Inv_Qty", typeof(decimal));
                //dtgetproducts.Columns.Add("Basic_Price", typeof(decimal));
                //dtgetproducts.Columns.Add("Amt_Before_Disc", typeof(decimal));
                //dtgetproducts.Columns.Add("Disc_Per", typeof(decimal));
                //dtgetproducts.Columns.Add("Disc_Amt", typeof(decimal));
                //dtgetproducts.Columns.Add("Taxable_Value", typeof(decimal));
                //dtgetproducts.Columns.Add("CGST_Per", typeof(decimal));
                //dtgetproducts.Columns.Add("CGST_Amt", typeof(decimal));
                //dtgetproducts.Columns.Add("SGST_Per", typeof(decimal));
                //dtgetproducts.Columns.Add("SGST_Amt", typeof(decimal));
                //dtgetproducts.Columns.Add("IGST_Per", typeof(decimal));
                //dtgetproducts.Columns.Add("IGST_Amt", typeof(decimal));
                //dtgetproducts.Columns.Add("Total_Amount", typeof(decimal));
                //dtgetproducts.Columns.Add("Remarks", typeof);
                //dtgetproducts.Columns.Add("SO_Ref_No", typeof);               
                //dtgetfinalprducts.Rows.Clear();
                ////listBox.Items.Clear();
                //// Get the selected items of SfDataGrid
                ////var reflector = this.sfDataGrid1.View.GetPropertyAccessProvider();
                ////var row = this.sfDataGrid1.SelectedItem;

                ////string ProdCode;
                ////string SoNo;
                //for (int i = 1; i < sfDataGrid1.RowCount; i++)
                //{
                //    foreach (var item in sfDataGrid1.SelectedItems)
                //    {

                //        //foreach (var col in sfDataGrid1.Columns)
                //        //{
                //        //if (col.MappingName == "Alternative_Code")
                //        //{
                //        //var mappingName = sfDataGrid1.Columns[columnIndex].MappingName;
                //        //var recordIndex = sfDataGrid1.TableControl.ResolveToRecordIndex(rowIndex);
                //        var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                //        var ProdCodeCol = sfDataGrid1.Columns[1].MappingName;
                //        var SONoCol = sfDataGrid1.Columns[0].MappingName;
                //        //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                //        //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                //        if (rowData == item)
                //        {
                //            var Item_Code = (rowData.GetType().GetProperty("Prod_Code").GetValue(rowData, null).ToString());
                //            var Item_Description = (rowData.GetType().GetProperty("Product_Description").GetValue(rowData, null).ToString());
                //            var UOM = (rowData.GetType().GetProperty("Uom").GetValue(rowData, null).ToString());
                //            var PO_Qty = (rowData.GetType().GetProperty("BalQty").GetValue(rowData, null).ToString());
                //            var Basic_Price = (rowData.GetType().GetProperty("Price").GetValue(rowData, null).ToString());
                //            var SO_Ref_No = (rowData.GetType().GetProperty("SO_NO").GetValue(rowData, null).ToString());
                //            drgetproducts = dtgetproducts.NewRow();
                //            drgetproducts["Item_Code"] = Item_Code.ToString();
                //            drgetproducts["Item_Description"] = Item_Description.ToString();
                //            drgetproducts["Item_Grade"] = "";
                //            drgetproducts["UOM"] = UOM.ToString();
                //            drgetproducts["PO_Qty"] = PO_Qty.ToString();
                //            DateTime t = dpInvDate.Value;
                //            string f1 = t.ToString("dd/MMM/yyyy");
                //            var stock = (from data in db.ShowItemWiseStockReport(logIn.company, Convert.ToInt32(Item_Code.ToString()), t) select data).ToList();

                //            if (stock.Count > 0)
                //            {
                //                //dgProductsList.DataSource = d;
                //                drgetproducts["Stock_Qty"] = stock[0].ClosingQty;
                //            }
                //            //drgetproducts["Stock_Qty"] =0;
                //            drgetproducts["Inv_Qty"] = 0;
                //            drgetproducts["Basic_Price"] = Basic_Price.ToString();
                //            drgetproducts["Disc_Per"] = 0;
                //            drgetproducts["Disc_Amt"] = 0;
                //            drgetproducts["Taxable_Value"] = 0;
                //            drgetproducts["CGST_Per"] = 0;
                //            drgetproducts["CGST_Amt"] = 0;
                //            drgetproducts["SGST_Per"] = 0;
                //            drgetproducts["SGST_Amt"] = 0;
                //            drgetproducts["IGST_Per"] = 0;
                //            drgetproducts["IGST_Amt"] = 0;
                //            drgetproducts["Total_Amount"] = 0;
                //            drgetproducts["Remarks"] = "";
                //            drgetproducts["SO_Ref_No"] = SO_Ref_No.ToString();
                //            dtgetproducts.Rows.Add(drgetproducts);
                //            dtgetfinalprducts = dtgetfinalprducts.AsEnumerable().Union(dtgetproducts.AsEnumerable()).CopyToDataTable();
                //            //}
                //            //}
                //            dtgetproducts.Rows.Clear();
                //        }
                //    }
                //}
                ////dtexisting = dtexisting.AsEnumerable().Union(dtgetfinalprducts.AsEnumerable()).CopyToDataTable();
                ////dgProducts.DataSource = dtexisting;
                //txtCustPoNo.Text = "Multi";
                //txtSoNo.Text = "Multi";
                
                //dgProducts.DataSource = dtgetfinalprducts;
                //groupBox2.Visible = false;
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
            //txtOurInvNo.Focus();
            //groupBox2.Visible = false;        }
        }
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
                                 select new { c.GSTIN_NO }).ToList();
                    if (State.Count > 0)
                    {
                        txtCustGSTNo.Text = State[0].GSTIN_NO;
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

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                //  bindCashAct();
                OrderManagement.Transactions.frmSaleReturnVouchersList obj = new OrderManagement.Transactions.frmSaleReturnVouchersList();

                if (obj.ShowDialog() == DialogResult.OK)
                {
                    txtVchNo.Text = OrderManagement.Transactions.frmSaleReturnVouchersList.voucherNo;

                    if (!string.IsNullOrEmpty(txtVchNo.Text))
                    {
                        bindedit();


                    }

                    //var f = (from s in db.Account_Opening_Balances where s.Voucher_no == txtVoucherNo.Text && s.Company_ID == logIn.company select s).FirstOrDefault();
                    //if (f != null)
                    //{
                    //    AsAtdate.Text = f.OB_date.ToString();
                    //    //cmbAccountName.Text = f.Account;
                    //    txtRemarks.Text = f.Remarks;
                    //    //ObDate.Text = f.OBDate.ToString();
                    //    //txtCreditAmtTotal.Text = f.TotalCreditAmt.ToString();
                    //    //txtDebitAmtTotal.Text = f.TotalDebitAmt.ToString();

                    //}
                    //double Qty = 0, Amout = 0;

                    //for (int j = 0; j < dgvJournalVouchar.Rows.Count; j++)
                    //{

                    //    if (dgvJournalVouchar.Rows[j].Cells["Debit"].Value != null)
                    //    {

                    //        Qty += (dgvJournalVouchar.Rows[j].Cells["Debit"].Value == DBNull.Value) ? Convert.ToDouble("00") : Convert.ToDouble(dgvJournalVouchar.Rows[j].Cells["Debit"].Value);
                    //        Amout += (dgvJournalVouchar.Rows[j].Cells["Credit"].Value == DBNull.Value) ? Convert.ToDouble("00") : Convert.ToDouble(dgvJournalVouchar.Rows[j].Cells["Credit"].Value);


                    //    }

                    //}
                    //txtDebitAmtTotal.Text = Qty.ToString(".00");
                    //txtCreditAmtTotal.Text = Amout.ToString(".00");

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

                    db.sp_SaleReturn_Delete(txtVchNo.Text, logIn.company,logIn.BU_ID);

                    MessageBox.Show("Selected Voucher Deleted Successfully");

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
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

               
                path = Path.Combine(Directory.GetCurrentDirectory(), "CreditNote.pdf");
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
                    //rep = new OrderManagement.Transactions.Credit_Note_Vpack();


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
               
                    rep.RecordSelectionFormula = "{ SaleReturns_Master.Vch_No} = '" + txtVchNo.Text + "' and { SaleReturns_Master.Company_ID} = " + logIn.company + " and { SaleReturns_Master.BU_ID} = " + logIn.BU_ID + "";


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

        private void button1_Click(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                GlobalVariables.docName = "Debit Note";
            }
            else
            {
                GlobalVariables.docName = "Credit Note";
            }
            CR_No_for_EInv = txtVchNo.Text;
            ioneNet.OrderManagement.Transactions.GenerateEInvoice frm = new GenerateEInvoice();
            frm.MdiParent = this.MdiParent;
            frm.Show();
        }

        public void bindedit()
        {
            try
            {
                //txtVchNo.Text = OrderManagement.Transactions.ListOfInvoices.SO_No;
                String myString = "";
                int SRID = 0;
                myString = txtVchNo.Text;
                var da = (from obj in db.SaleReturns_Masters
                          where obj.Vch_No == txtVchNo.Text && obj.Company_ID == logIn.company && obj.BU_ID == logIn.BU_ID
                          select obj).ToList();

                if (da.Count > 0)
                {
                    SRID = da[0].Id;
                    txtVchNo.Text = da[0].Vch_No.ToString();
                    dpInvDate.Text = da[0].Vch_Date.ToString();
                    //cmbInvType.Text = da[0].InvType;

                    //cmbTaxClass.SelectedValue = da[0].Tax_Class;
                    //bindCustomer();
                    CmbBuyerName.SelectedValue = da[0].BuyerName;
                    bindConsignee();
                    CmbConsigneeName.SelectedValue = da[0].ConsigneeName;
                    txtCustGSTNo.Text = da[0].Cust_GST_No;
                    txtConAddress.Text  = da[0].ConsigneeAddress;
                    txtConGSTNo.Text = da[0].Con_GST_No;
                    txtOurInvNo.Text = da[0].Inv_No;
                    dpPODate.Text = da[0].CustRefDate.ToString();
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
                    txtOtherCharges.Text = da[0].Other_Charges.ToString();
                    txtTCSPer.Text = da[0].TCS_Per.ToString();
                    txtTCSAmt.Text = da[0].TCS_Amnt.ToString();
                    txtRounding.Text = da[0].Rounding.ToString();
                    txtLRNo.Text = da[0].InvValueInWords;
                  //  cmbTransporter.Text = da[0].Transporter_Name;
                   // txtDespthrough.Text = da[0].DespatchThrough;
                    txtVehicleNo.Text = da[0].VehicleNo;
                  //  txtWayBillNo.Text = da[0].WayBillNo;
                    txtSplInstructions.Text = da[0].Spl_Instructions;                    
                   // cmbStatus.SelectedValue = da[0].Status;      
                    lblCreatedBy.Text = da[0].Created_By;
                    lblModified.Text = da[0].Modified_By;
                    txtSoNo.Text = da[0].CustomerRefNo;
                    //txtLRNo.Text = da[0].LR_No;
                   // dtLRDate.Text = da[0].LR_Date.ToString();
                }


                var dm1 = (from s in db.SaleReturns_Childs
                           where s.SR_Master_ID == SRID && s.Company_ID == logIn.company orderby s.Item_No


                           select new

                           {
                               Item_Code =s.Prod_Code,
                               Item_Description =s.Product_Description,
                               Item_Grade =s.Prod_Grade,
                               UOM=s.Uom,
                               s.Inv_Qty,
                               //s.Stock_qty,
                               Return_Qty = s.Return_Qty,
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
                               Inv_Ref_No = s.Inv_Ref_No,
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
               
                String myString = "";
                int InvID = 0;
                myString = txtOurInvNo.Text;
                var da = (from obj in db.Invoice_Masters
                          where obj.Inv_No == txtOurInvNo.Text && obj.Company_ID == logIn.company && obj.BU_ID == logIn.BU_ID
                          select obj).ToList();

                if (da.Count > 0)
                {
                    InvID = da[0].Id;
                    CmbBuyerName.SelectedValue = da[0].BuyerName;
                    bindConsignee();
                    CmbConsigneeName.SelectedValue = da[0].ConsigneeName;
                    txtCustGSTNo.Text = da[0].Cust_GST_No;
                    txtConAddress.Text = da[0].ConsigneeAddress;
                    txtConGSTNo.Text = da[0].Con_GST_No;                
                  
                    if (da[0].Sale_Account != null)
                    {
                        cmbSaleAccount.SelectedValue = da[0].Sale_Account;
                    }
                    //cmbCustomer.Enabled = false;
                    //txtTotalQty.Text = da[0].TotalQty.ToString();
                    //txtSubTotal.Text = da[0].SubTotal.ToString();
                    //txtTotDiscount.Text = da[0].Tot_Discount.ToString();
                    //txtTot_TaxableValue.Text = da[0].Tot_TaxableValue.ToString();
                    //txtTot_CGST.Text = da[0].Tot_CGST_Amnt.ToString();
                    //txtTot_SGST.Text = da[0].Tot_SGST_Amnt.ToString();
                    //txtTot_IGST.Text = da[0].Tot_IGST_Amnt.ToString();
                    //txtTot_InvValue.Text = da[0].Tot_Inv_Value.ToString();
                    //txtFrieght.Text = da[0].Frieght_amnt.ToString();
                    //txtOtherCharges.Text = da[0].Other_Charges.ToString();
                    //txtTCSPer.Text = da[0].TCS_Per.ToString();
                    //txtTCSAmt.Text = da[0].TCS_Amnt.ToString();
                    //txtRounding.Text = da[0].Rounding.ToString();
                   
                    
                }


                var dm1 = (from s in db.Invoice_Childs
                           where s.Inv_Master_ID == InvID && s.Company_ID == logIn.company
                           orderby s.Item_No


                           select new

                           {
                               Item_Code = s.Prod_Code,
                               Item_Description = s.Product_Description,
                               Item_Grade = s.Prod_Grade,
                               UOM = s.Uom,                              
                               Inv_Qty = s.Qty,
                               Basic_Price = s.Price,
                              // Amt_Before_Disc = s.Amount,
                               s.Disc_Per,
                             //  Disc_Amt = s.Disc_Amount,
                             //  s.Taxable_Value,
                               s.CGST_Per,
                              // CGST_Amt = s.CGST_Amnt,
                               s.SGST_Per,
                              // SGST_Amt = s.SGST_Amnt,
                               s.IGST_Per,
                               //  IGST_Amt = s.IGST_Amnt,
                               // Total_Amount = s.Net_Amount,
                               // s.Remarks,
                               Inv_Ref_No = s.Inv_No,
                              // Item_No = s.Item_No



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
