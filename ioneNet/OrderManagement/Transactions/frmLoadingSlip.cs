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
using Syncfusion.Windows.Forms.Chart.SvgBase;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using Syncfusion.Windows.Forms.Tools.Win32API;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;

namespace ioneNet.OrderManagement.Transactions
{
    public partial class frmLoadingSlip : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();
        public static string SONo,ItemCode,OrdQty;
        decimal taxRate = 0;
        decimal cgstPer,sgstPer,igstPer;
        public static string DocNo, ItemCode_Issue, RecQty, Suppname;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        private Database crDatabase;
        private string path;
        private string pattern = "^[0-9]{0,5}-";
        public frmLoadingSlip()
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
            //textBox1.Text = DateTime.Now.ToString("hh:mm");
            //textBox2.Text = DateTime.Now.ToString("hh:mm");
            txtInvNo.Enabled = false;

            txtBUID.Text = logIn.BU_ID.ToString(); 
            AutoincrementId();
            if (ListofLoadingSlips.editMode == true)
            {
                bindedit();
            }
            else
            {
                

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

                //if (cmbInvType.Text == string.Empty)
                //{
                //    MessageBox.Show("Inv Type Should Not Be Empty", "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    cmbInvType.Focus();
                //    return;
                //}
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
                //else if (cmbInvType.Text == string.Empty)
                //{
                //    MessageBox.Show("Select Invoice Type,");
                //    cmbInvType.Focus();
                //    return;
                //}
                else if (cmbStatus.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Status");
                    cmbStatus.Focus();
                    return;
                }
                //else if (cmbTCS.Text == string.Empty)
                //{
                //    decimal TotTO = Convert.ToDecimal(linkLabel2.Text);
                //    if (TotTO > 5000000)
                //    {
                //        MessageBox.Show("Sale Turnover to this Customer is >50.0 Lacs, Need To Deduct TCS, Select NA to Proceed without Deduction");
                        
                //    }
                //    else                     
                        
                //    {
                //        MessageBox.Show("Please Select Wether TCS Applicable or Not");
                //        //cmbTCS.Focus();
                //        return;
                //    }
                    
                    

                //}
                //else if (cmbTaxClass.Text == string.Empty)
                //{
                //    MessageBox.Show("Tax Class Cannot be Empty,");
                //    cmbTaxClass.Focus();
                //    return;
                //}
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
                var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company  select new { m.ID, m.Supplier_Name }).Distinct().ToList();
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

                    //CmbConsigneeName.DataSource = Buyerblind;
                    //CmbConsigneeName.ValueMember = "ID";
                    //CmbConsigneeName.DisplayMember = "Supplier_Name";

                }
                //if (CmbBuyerName.Items.Count > 0)
                //CmbConsigneeName.SelectedIndex = -1;

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

                    //CmbConsigneeName.DataSource = Buyerblind;
                    //CmbConsigneeName.ValueMember = "ConsigneeCode";
                    //CmbConsigneeName.DisplayMember = "ConsigneeName";

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

                //Bind Transporter
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
                var result = db.Sp_autoincrement_Loading(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, logIn.BU_ID);
                txtInvNo.Text = result.FirstOrDefault().Loading_Slip_No;

                
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
                if (tb3 != null && columnName == "Length_Loaded")
                {

                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;

                    e.Control.KeyPress += new KeyPressEventHandler(CheckKey);
                    //dgProducts.EditingControl.KeyPress -= EditingControl_KeyPress;
                    //dgProducts.EditingControl.KeyPress += EditingControl_KeyPress;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CheckKey(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar)
                && e.KeyChar != '-'
                && e.KeyChar != '.')
            {
                e.Handled = true;
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
                //Length_Loaded
                if (columnName == "Length_Loaded")
                {

                    //DateTime dt = 
                    string dt1 = "2024-03-01";

                    DateTime dtt = dpInvDate.Value;
                    string dt2 = dtt.ToString("yyyy/MM/dd");

                    var Prodname = (from d in db.ShowStockReport_FG(logIn.company,Convert.ToDateTime(dt1),Convert.ToDateTime(dt2),logIn.BU_ID) where d.Item_Name == R1.Cells["Item_Description"].Value.ToString()
                                    select new { d.Prod_length }).Distinct().ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Prod_length");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Prod_length);
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
                                    ItemCode = dgProducts.Rows[i].Cells["Item_No"].Value.ToString();
                                    SqlCommand cmd1 = new SqlCommand("delete  from [LOADING_CHILD] where [ITEM_NO] =@ProdID and [Loading_Slip_No] = @pono", con);
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
                        decimal x = 0, y = 0;

                        for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                        {

                            x += (dgProducts.Rows[i].Cells["Inv_Qty"].Value == "" || dgProducts.Rows[i].Cells["Inv_Qty"].Value == null || dgProducts.Rows[i].Cells["Inv_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Inv_Qty"].Value);

                            y += (dgProducts.Rows[i].Cells["Qty_Loaded"].Value == "" || dgProducts.Rows[i].Cells["Qty_Loaded"].Value == null || dgProducts.Rows[i].Cells["Qty_Loaded"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty_Loaded"].Value);

                        }

                        txtTotalQty.Text = x.ToString(".00");
                        txtQtyLoaded.Text = y.ToString(".00");


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
                string pcode = "";
                decimal toleQty = 0;
                if (columnName == "Item_Description")
                {
                    R1.Cells["Item_Code"].Value = dgProducts.CurrentCell.RowIndex + 1;

                    decimal b, c, d;
                    
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
                        // R1.Cells["Disc_Per"].Value = "0";

                        DateTime t = dpInvDate.Value;
                        string dt1 = t.ToString("yyyy/MM/dd");
                        var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(R1.Cells["Item_Code"].Value.ToString()), Convert.ToDateTime(dt1), logIn.BU_ID) select data).ToList();
                        R1.Cells["Stock_Qty"].Value = "0";
                        if (stock.Count > 0)
                        {
                            //dgProductsList.DataSource = d;
                            R1.Cells["Stock_Qty"].Value = stock[0].ClosingQty;

                        }

                    }

                }
                if (columnName == "Inv_Qty" || columnName == "Qty_Loaded")
                {

                    if (R1.Cells["Item_Description"].Value != null)
                    {
                        decimal POQty = (R1.Cells["PO_Qty"].Value == "" || R1.Cells["PO_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["PO_Qty"].Value);
                        decimal Stock_Qty = (R1.Cells["Stock_Qty"].Value == "" || R1.Cells["Stock_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Stock_Qty"].Value);
                        decimal Bal_Qty = (R1.Cells["Bal_Qty"].Value == "" || R1.Cells["Bal_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Bal_Qty"].Value);
                        decimal Amt, DiscAmt, netAmt, gst, igst, totamt;
                        string Custstatetcode;
                        toleQty = 0;
                        if (txtCustPoNo.Text != "NA")
                        {

                            string SO_Ref_No = R1.Cells["SO_Ref_No"].Value.ToString();
                            var PGrade = (from data in db.Sale_Order_Childs where data.SO_NO == SO_Ref_No && data.enq_item_no == Convert.ToInt32(R1.Cells["SO_Item_No"].Value.ToString()) && data.Company_ID == logIn.company select data).ToList();

                            if (PGrade.Count > 0)
                            {
                                //dgProductsList.DataSource = d;
                                toleQty = Convert.ToDecimal(PGrade[0].Tole_Qty);
                                if (toleQty == 0)
                                {
                                    toleQty = (POQty * 5) / 100;
                                }
                                //if (PGrade[0].Prod_Grade != null)
                                //{
                                //    drgetproducts["Item_Grade"] = PGrade[0].Prod_Grade.Trim();
                                //}


                            }
                            else
                            {
                                toleQty = (POQty * 5) / 100;
                            }

                            decimal AcceptedQty = (R1.Cells["Inv_Qty"].Value == "" || R1.Cells["Inv_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Inv_Qty"].Value);
                            if (AcceptedQty > Bal_Qty)
                            {


                                if (AcceptedQty > (Bal_Qty + toleQty))
                                {
                                    MessageBox.Show("Invoice Qty Cannot Be Greater Than Order Qty");
                                    R1.Cells["Inv_Qty"].Value = "0";
                                    return;
                                }


                            }

                            else
                            {

                                int j1 = dgProducts.CurrentRow.Index;
                                //int j1 =  Convert.ToInt32(R1.ToString()) + 1;
                                if (R1.Cells["Item_Code"].Value != "")
                                {

                                }
                                else
                                {
                                    R1.Cells["Item_Code"].Value = (j1 + 1).ToString();
                                }

                            }
                        }
                        decimal x = 0, y=0 ;

                        for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                        {

                            x += (dgProducts.Rows[i].Cells["Inv_Qty"].Value == "" || dgProducts.Rows[i].Cells["Inv_Qty"].Value == null || dgProducts.Rows[i].Cells["Inv_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Inv_Qty"].Value);

                            y += (dgProducts.Rows[i].Cells["Qty_Loaded"].Value == "" || dgProducts.Rows[i].Cells["Qty_Loaded"].Value == null || dgProducts.Rows[i].Cells["Qty_Loaded"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty_Loaded"].Value);

                        }

                        txtTotalQty.Text = x.ToString(".00");
                        txtQtyLoaded.Text = y.ToString(".00");

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
                //txtSubTotal.Text = y.ToString("0.00");
                //txtTotDiscount.Text = q.ToString(".00");
                //decimal frieghtPerMT = (txtFrieghtPerTon.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtFrieghtPerTon.Text);
                //if (frieghtPerMT > 0)
                //{
                //    txtFrieght.Text = (frieghtPerMT * totQty).ToString(); ;
                // }
                //else
                //{

                //}
                //decimal fAmt = (txtFrieght.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtFrieght.Text);
                //decimal OthAmt = (txtOthers.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtOthers.Text);
                //decimal pAmt = (txtPacking.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtPacking.Text);
                //decimal InsAmt = (txtInsurance.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtInsurance.Text);
                //decimal cessAmt = (txtCessAmt.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtCessAmt.Text);

                //txtTot_TaxableValue.Text = (v + fAmt + OthAmt+pAmt+InsAmt).ToString(".00");
                //decimal taxvalue = Convert.ToDecimal(txtTot_TaxableValue.Text);
                //decimal cgst = (taxvalue * cgstPer) / 100;
                //decimal sgst = (taxvalue * sgstPer) / 100;
                //decimal igst = (taxvalue * igstPer) / 100;

                //txtTot_CGST.Text = cgst.ToString(".00");
                //txtTot_SGST.Text = sgst.ToString(".00");
                //txtTot_IGST.Text = igst.ToString(".00");
                //decimal AmtForTCs = 0;
                


                //if (cmbTCS.Text == "TCS on Basic")
                //{
                //    AmtForTCs = (v);
                //}
                //else if (cmbTCS.Text == "TCS on Gross")

                //{
                //    AmtForTCs = (fAmt + OthAmt + v + cgst + sgst + igst+ cessAmt);
                //    //AmtForTCs = (v);
                //}
                //else
                //{
                //    AmtForTCs = 0;
                //}
                
                //decimal tcsPer = (txtTCSPer.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtTCSPer.Text);

                //decimal TcsAmt = AmtForTCs * tcsPer / 100;
                //txtTCSAmt.Text = TcsAmt.ToString(".00");
                //decimal rndAmt = (txtRounding.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtRounding.Text);
                //txtTot_InvValue.Text = (taxvalue + cgst + sgst + igst + TcsAmt + rndAmt+cessAmt).ToString(".00");
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
                    if ((from u in db.Loading_Masters where u.Loading_Slip_No == myString && u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID select u).Count() > 0)
                    {
                        myString = txtInvNo.Text;
                    }
                    
                    else
                    {
                        AutoincrementId();
                    }                    
                }
                myString = txtInvNo.Text;
                if (txtInvNo.Text != "") 
                {

                    SqlCommand cmd = new SqlCommand("SaveLoadingSlip", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Loading_Slip_No", myString);
                    cmd.Parameters.AddWithValue("@Slip_Date", dpInvDate.Value);
                    cmd.Parameters.AddWithValue("@BuyerName", Convert.ToInt32(CmbBuyerName.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@CustomerPONo", (txtSoNo.Text == "") ? "" : txtSoNo.Text);
                    cmd.Parameters.AddWithValue("@SO_Ref_No", txtCustPoNo.Text);
                    cmd.Parameters.AddWithValue("@TotalQty", (txtTotalQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotalQty.Text));
                    cmd.Parameters.AddWithValue("@Transporter_Name", (cmbTransporter.Text == "") ? "" : cmbTransporter.Text);
                    cmd.Parameters.AddWithValue("@VehicleNo", (txtVehicleNo.Text == "") ? "" : txtVehicleNo.Text);
                    cmd.Parameters.AddWithValue("@Spl_Instructions", (txtSplInstructions.Text == "") ? "" : txtSplInstructions.Text);
                    cmd.Parameters.AddWithValue("@Status", Convert.ToInt32(cmbStatus.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@BU_ID", Convert.ToInt32(txtBUID.Text));
                    cmd.Parameters.AddWithValue("@Company_ID", logIn.company);
                    cmd.Parameters.AddWithValue("@Created_By", lblCreatedBy.Text);
                    cmd.Parameters.AddWithValue("@Modified_By", logIn.username + "-" + DateTime.Now);
                    cmd.Parameters.AddWithValue("@Destination", (txtDestination.Text == "") ? "" : txtDestination.Text);
                    cmd.Parameters.AddWithValue("@Issue_Time", dtIssueTime.Value);
                    cmd.Parameters.AddWithValue("@Loading_Completion_Time", dtCompletionTime.Value);
                    cmd.Parameters.AddWithValue("@Assigned_To", (cmbAssignedTo.Text == "") ? "" : cmbAssignedTo.Text);

                    string Prod_Code = "";                  
                    string PO_Qty = "";
                    string Stock_Qty = "";
                    string Qty_To_Load = "";
                    string No_Of_Pcs = "";
                    string Length_Loaded = "";
                    string Qty_Loaded = "";
                    string No_pcs_Loaded = "";
                    string SO_NO = "";
                    string Remarks = "";
                    string Item_No = "";
                    string Company_ID = "";
                    string SO_Item_No = "";
                    int rowcount = 0;
                    decimal BalQty = 0;
                    int k = 0;
                    for (int i = 0; i < dgProducts.RowCount - 1; i++)
                    {

                        Prod_Code = Prod_Code + Convert.ToString(dgProducts.Rows[i].Cells["Item_Code"].Value).PadRight(14);
                        BalQty = (dgProducts.Rows[i].Cells["Bal_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Bal_Qty"].Value);

                        PO_Qty = PO_Qty + Convert.ToString(BalQty).PadRight(14);
                        Stock_Qty = Stock_Qty + Convert.ToString(dgProducts.Rows[i].Cells["Stock_Qty"].Value).PadRight(14);
                        Qty_To_Load = Qty_To_Load + Convert.ToString(dgProducts.Rows[i].Cells["Inv_Qty"].Value).PadRight(14);
                        //No_Of_Pcs = No_Of_Pcs + Convert.ToString(dgProducts.Rows[i].Cells["No_Of_Pcs"].Value).PadRight(14);
                        No_Of_Pcs = No_Of_Pcs + Convert.ToString((dgProducts.Rows[i].Cells["No_Of_Pcs"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["No_Of_Pcs"].Value)).PadRight(14);

                        Length_Loaded = Length_Loaded + Convert.ToString(dgProducts.Rows[i].Cells["Length_Loaded"].Value).PadRight(14);
                        //Qty_Loaded = Qty_Loaded + Convert.ToString(dgProducts.Rows[i].Cells["Qty_Loaded"].Value).PadRight(14);
                        Qty_Loaded = Qty_Loaded + Convert.ToString((dgProducts.Rows[i].Cells["Qty_Loaded"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty_Loaded"].Value)).PadRight(14);

                        //No_pcs_Loaded = No_pcs_Loaded + Convert.ToString(dgProducts.Rows[i].Cells["No_pcs_Loaded"].Value).PadRight(14);
                        No_pcs_Loaded = No_pcs_Loaded + Convert.ToString((dgProducts.Rows[i].Cells["No_pcs_Loaded"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["No_pcs_Loaded"].Value)).PadRight(14);

                        Remarks = Remarks + Convert.ToString(dgProducts.Rows[i].Cells["Remarks"].Value).PadRight(200);

                        SO_NO = SO_NO + Convert.ToString(dgProducts.Rows[i].Cells["SO_Ref_No"].Value).Trim().PadRight(14);
                        SO_Item_No = SO_Item_No + Convert.ToString(dgProducts.Rows[i].Cells["SO_Item_No"].Value).PadRight(14);

                        
                        if (dgProducts.Rows[i].Cells["Item_No"].Value == null || dgProducts.Rows[i].Cells["Item_No"].Value == "")
                        {
                            
                            Item_No = Item_No + Convert.ToString(k + 1).PadRight(14);
                            k = k + 1;
                        }
                        else
                        {
                            k = Convert.ToInt32(dgProducts.Rows[i].Cells["Item_No"].Value);
                            Item_No = Item_No + Convert.ToString(dgProducts.Rows[i].Cells["Item_No"].Value).PadRight(14);
                        }

                        rowcount += 1;
                    }

                    cmd.Parameters.AddWithValue("@txt_Prod_Code", Prod_Code);
                     cmd.Parameters.AddWithValue("@txt_PO_Qty", PO_Qty);
                    cmd.Parameters.AddWithValue("@txt_Stock_Qty", Stock_Qty);
                    cmd.Parameters.AddWithValue("@txt_Qty", Qty_To_Load);
                    cmd.Parameters.AddWithValue("@txt_No_Of_Pcs", No_Of_Pcs);
                    cmd.Parameters.AddWithValue("@txt_Qty_Loaded", Qty_Loaded);
                    cmd.Parameters.AddWithValue("@txt_Length_Loaded", Length_Loaded);
                    cmd.Parameters.AddWithValue("@txt_No_pcs_Loaded", No_pcs_Loaded);
                    cmd.Parameters.AddWithValue("@txt_Remarks", Remarks);
                    cmd.Parameters.AddWithValue("@txt_SO_NO", SO_NO);
                    cmd.Parameters.AddWithValue("@txt_Item_No", Item_No);
                    cmd.Parameters.AddWithValue("@txt_SO_Item_No", SO_Item_No);
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

                DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].Name;
               
                //if(txtCustPoNo.Text =="NA")
                //{
                //    R1.Cells["Basic_Price"].ReadOnly = false;
                //}
                //else
                //{
                //    R1.Cells["Basic_Price"].ReadOnly = true;
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
            sfDataGrid1.DataSource = null;
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
                string so_Nos = "";
                
                DataTable dtgetSelectedprducts = new DataTable();
                //Check Whether Exisitng Products Already Selected in Main Grid
                if (dgProducts.Rows.Count > 1)
                {
                    dtexisting.Rows.Clear();
                    dtexisting.Columns.Clear();
                    dtexisting.Columns.Add("Item_No", typeof(string));
                    dtexisting.Columns.Add("Item_Code", typeof(string));
                    dtexisting.Columns.Add("Prod_Code", typeof(string));
                    dtexisting.Columns.Add("Item_Description", typeof(string));                   
                    dtexisting.Columns.Add("Item_Grade", typeof(string));
                    dtexisting.Columns.Add("Prod_Length", typeof(string));
                    dtexisting.Columns.Add("No_Of_Pcs", typeof(string));
                    dtexisting.Columns.Add("UOM", typeof(string));
                    dtexisting.Columns.Add("PO_Qty", typeof(string));
                    dtexisting.Columns.Add("Bal_Qty", typeof(string));
                    dtexisting.Columns.Add("Stock_Qty", typeof(string));
                    dtexisting.Columns.Add("Inv_Qty", typeof(string));
                    dtexisting.Columns.Add("Length_Loaded", typeof(string));
                    dtexisting.Columns.Add("Qty_Loaded", typeof(string));
                    dtexisting.Columns.Add("No_pcs_Loaded", typeof(string));
                    dtexisting.Columns.Add("SO_Ref_No", typeof(string));
                    dtexisting.Columns.Add("SO_Item_No", typeof(string));
                    dtexisting.Columns.Add("Remarks", typeof(string));




                    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                    {
                        DataRow dr;
                        dr = dtexisting.NewRow();
                        dr["Item_No"] = dgProducts.Rows[i].Cells["Item_No"].Value.ToString();
                        dr["Item_Code"] = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                        dr["Prod_Code"] = dgProducts.Rows[i].Cells["Prod_Code"].Value.ToString();
                        dr["Item_Description"] = dgProducts.Rows[i].Cells["Item_Description"].Value.ToString();
                        dr["Item_Grade"] = dgProducts.Rows[i].Cells["Item_Grade"].Value.ToString();
                        dr["Prod_Length"] = dgProducts.Rows[i].Cells["Prod_Length"].Value.ToString();
                        dr["No_Of_Pcs"] = dgProducts.Rows[i].Cells["No_Of_Pcs"].Value.ToString();
                        dr["UOM"] = dgProducts.Rows[i].Cells["UOM"].Value.ToString();
                        dr["PO_Qty"] = dgProducts.Rows[i].Cells["PO_Qty"].Value.ToString();
                        dr["Bal_Qty"] = dgProducts.Rows[i].Cells["Bal_Qty"].Value.ToString();
                        dr["Stock_Qty"] = dgProducts.Rows[i].Cells["Stock_Qty"].Value.ToString();
                        dr["Inv_Qty"] = dgProducts.Rows[i].Cells["Inv_Qty"].Value.ToString();
                        dr["Length_Loaded"] = dgProducts.Rows[i].Cells["Length_Loaded"].Value.ToString();
                        dr["Qty_Loaded"] = dgProducts.Rows[i].Cells["Qty_Loaded"].Value.ToString();
                        dr["No_pcs_Loaded"] = dgProducts.Rows[i].Cells["No_pcs_Loaded"].Value.ToString();
                        dr["SO_Ref_No"] = dgProducts.Rows[i].Cells["SO_Ref_No"].Value.ToString();
                        dr["SO_Item_No"] = dgProducts.Rows[i].Cells["SO_Item_No"].Value.ToString();
                        dr["Remarks"] = dgProducts.Rows[i].Cells["Remarks"].Value.ToString();

                        dtexisting.Rows.Add(dr);

                    }
                    dtexisting.AcceptChanges();               
                }

                dtgetproducts.Columns.Clear();
                dtgetproducts.Rows.Clear();
                dtgetproducts.Columns.Add("Item_No", typeof(string));
                dtgetproducts.Columns.Add("Item_Code", typeof(string));
                dtgetproducts.Columns.Add("Prod_Code", typeof(string));
                dtgetproducts.Columns.Add("Item_Description", typeof(string));
                dtgetproducts.Columns.Add("Item_Grade", typeof(string));
                dtgetproducts.Columns.Add("Prod_Length", typeof(string));
                dtgetproducts.Columns.Add("No_Of_Pcs", typeof(string));
                dtgetproducts.Columns.Add("UOM", typeof(string));
                dtgetproducts.Columns.Add("PO_Qty", typeof(string));
                dtgetproducts.Columns.Add("Bal_Qty", typeof(string));
                dtgetproducts.Columns.Add("Stock_Qty", typeof(string));
                dtgetproducts.Columns.Add("Inv_Qty", typeof(string));
                dtgetproducts.Columns.Add("Length_Loaded", typeof(string));
                dtgetproducts.Columns.Add("Qty_Loaded", typeof(string));
                dtgetproducts.Columns.Add("No_pcs_Loaded", typeof(string));
                dtgetproducts.Columns.Add("SO_Ref_No", typeof(string));
                dtgetproducts.Columns.Add("SO_Item_No", typeof(string));
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
                            var PO_Qty = (rowData.GetType().GetProperty("OrdQty").GetValue(rowData, null).ToString());
                            var Bal_Qty = (rowData.GetType().GetProperty("BalQty").GetValue(rowData, null).ToString());
                            var PO_No_Pcs = (rowData.GetType().GetProperty("No_Of_Pieces").GetValue(rowData, null).ToString());
                            var Basic_Price = (rowData.GetType().GetProperty("Price").GetValue(rowData, null).ToString());
                            var SO_Ref_No = (rowData.GetType().GetProperty("SO_NO").GetValue(rowData, null).ToString());
                            var So_line_Item_No = (rowData.GetType().GetProperty("So_line_Item_No").GetValue(rowData, null).ToString());

                            drgetproducts = dtgetproducts.NewRow();
                            drgetproducts["Item_Code"] = Item_Code.ToString();
                            drgetproducts["Item_Description"] = Item_Description.ToString();

                            int ordId = 0;
                            //Get Consignee Details
                            var State = (from c in db.Sale_Order_Masters
                                         join s in db.Supplier_informations on c.ConsigneeName equals s.ID
                                         where c.Status !=24 && c.SO_NO == SO_Ref_No && c.Company_ID == logIn.company //Convert.ToInt32(CmbBuyerName.SelectedValue.ToString())
                                         select new { c.Delivery_Address, c.ConsigneeName,c.Delivery_GSTIN,c.Frieght_Unit, c.Id,s.City,c.BU_ID }).ToList();
                            if (State.Count > 0)
                            {
                                    ordId = State[0].Id;
                                txtBUID.Text = State[0].BU_ID.ToString();

                            }
                            //drgetproducts["Disc_Per"] = 0;
                            var PGrade = (from data in db.Sale_Order_Childs
                                          //join q in db.Sale_Quotation_Childs on new { X1 = data.enq_Master_ID, X2 = data.Quot_Master_ID } equals new { X1 = q.Quote_Item_No, X2 = q.Quot_Master_ID }
                                          where data.So_Master_ID == ordId 
                                          && data.enq_item_no == Convert.ToInt32(So_line_Item_No)  && data.Company_ID ==logIn.company  
                                          select new { data.Int_Prod_Code, data.enq_item_no, data.Prod_Length, data.Prod_Grade, data.No_Of_Pieces }).ToList();

                            if (PGrade.Count > 0)
                            {
                                
                                //dgProductsList.DataSource = d;
                                if (PGrade[0].Prod_Grade != null)
                                {
                                    drgetproducts["Item_Grade"] = PGrade[0].Prod_Grade.Trim();
                                }
                                drgetproducts["Prod_Code"] = PGrade[0].Int_Prod_Code;
                                drgetproducts["SO_Item_No"] = PGrade[0].enq_item_no;
                                drgetproducts["Prod_Length"] = PGrade[0].Prod_Length;
                                drgetproducts["No_Of_Pcs"] = PO_No_Pcs.ToString();

                            }
                            drgetproducts["UOM"] = UOM.ToString();
                            drgetproducts["PO_Qty"] = PO_Qty.ToString();
                            drgetproducts["Bal_Qty"] = Bal_Qty.ToString();
                            DateTime t = dpInvDate.Value;
                            string dt1 = t.ToString("yyyy/MM/dd");
                            var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(Item_Code.ToString()), Convert.ToDateTime(dt1), logIn.BU_ID) select data).ToList();
                            drgetproducts["Stock_Qty"] = "0";
                            if (stock.Count > 0)
                            {
                                //dgProductsList.DataSource = d;
                                drgetproducts["Stock_Qty"] = stock[0].ClosingQty;
                                //drgetproducts["Stock_Price"] = stock[0].CBPrice;
                            }
                            //drgetproducts["Stock_Qty"] =0;
                            drgetproducts["Inv_Qty"] = 0;
                            drgetproducts["Length_Loaded"] = 0;
                            drgetproducts["Qty_Loaded"] = 0;
                            drgetproducts["No_pcs_Loaded"] = 0;

                            //drgetproducts["SO_Item_No"] = "";
                            drgetproducts["SO_Ref_No"] = SO_Ref_No.ToString();
                            if (so_Nos != "")
                            {
                                so_Nos = so_Nos + "," + SO_Ref_No;
                            }
                            else
                            {
                                so_Nos = SO_Ref_No;
                            }
                            drgetproducts["Item_No"] = "";
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
                //dtgetSelectedprducts = dtgetSelectedprducts.AsEnumerable().Union(dtexisting.AsEnumerable()).CopyToDataTable();
                dtgetSelectedprducts = dtgetSelectedprducts.AsEnumerable().Union(dtexisting.AsEnumerable()).CopyToDataTable();

                dgProducts.DataSource = dtgetSelectedprducts;

                //txtCustPoNo.Text = "Multi";
                txtSoNo.Text = "Multi";

                txtCustPoNo.Text = string.Join(",", so_Nos.Split(',').Distinct());
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
                            //y += (dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == "" || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == null || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value);
                            //q += (dgProducts.Rows[i].Cells["Disc_Amt"].Value == "" || dgProducts.Rows[i].Cells["Disc_Amt"].Value == null || dgProducts.Rows[i].Cells["Disc_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Amt"].Value);
                            //v += (dgProducts.Rows[i].Cells["Taxable_Value"].Value == "" || dgProducts.Rows[i].Cells["Taxable_Value"].Value == null || dgProducts.Rows[i].Cells["Taxable_Value"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Taxable_Value"].Value);
                            //cg += (dgProducts.Rows[i].Cells["CGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["CGST_Amt"].Value == null || dgProducts.Rows[i].Cells["CGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Amt"].Value);
                            //sg += (dgProducts.Rows[i].Cells["SGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["SGST_Amt"].Value == null || dgProducts.Rows[i].Cells["SGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Amt"].Value);
                            //ig += (dgProducts.Rows[i].Cells["IGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["IGST_Amt"].Value == null || dgProducts.Rows[i].Cells["IGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Amt"].Value);
                            //totA += (dgProducts.Rows[i].Cells["Total_Amount"].Value == "" || dgProducts.Rows[i].Cells["Total_Amount"].Value == null || dgProducts.Rows[i].Cells["Total_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Total_Amount"].Value);

                        }

                        txtTotalQty.Text = x.ToString(".00");
                        //txtSubTotal.Text = y.ToString("0.00");
                        //txtTotDiscount.Text = q.ToString(".00");
                        //decimal fAmt = (txtFrieght.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtFrieght.Text);
                        //decimal OthAmt = (txtPacking.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtPacking.Text);

                        //txtTot_TaxableValue.Text = (v).ToString(".00");

                        //txtTot_CGST.Text = cg.ToString(".00");
                        //txtTot_SGST.Text = sg.ToString(".00");
                        //txtTot_IGST.Text = ig.ToString(".00");
                        //decimal AmtForTCs = (fAmt + OthAmt + v + cg + sg + ig);
                        //decimal tcsPer = (txtTCSPer.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtTCSPer.Text);

                        //decimal TcsAmt = AmtForTCs * tcsPer / 100;
                        //txtTCSAmt.Text = TcsAmt.ToString(".00");
                        //decimal rndAmt = (txtRounding.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtRounding.Text);
                        //txtTot_InvValue.Text = (totA + TcsAmt + fAmt + OthAmt + rndAmt).ToString(".00");


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

            //if (txtInvNo.Text != "")
            //{
            //}
            ////else
            ////{
            //    //if (cmbInvType.Text != "" && cmbInvType.Text == "Bill of Supply")
            //    //{
            //    //    txtInvNo.Enabled = true;
            //    //}
            //    else
            //    {
            //        txtInvNo.Enabled = false;
            //        AutoincrementId();
            //    }
            ////}

           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //var bindNearestCity = (from m in db.City_Masters select new { m.City_Name, m.ID }).Distinct().ToList();
            //if (bindNearestCity.Count > 0)
            //{
            //    cmbCity.DataSource = bindNearestCity;
            //    cmbCity.ValueMember = "ID";
            //    cmbCity.DisplayMember = "City_Name";
            //}
            //if (cmbCity.Items.Count == 1)
            //    cmbCity.SelectedIndex = 0;
            //else
            //    cmbCity.SelectedIndex = -1;
            //if (Mid(txtConAddress.Text, 3, 4) == "Addr")
            //{
            //    JObject jsoncancel = JObject.Parse(txtConAddress.Text);
            //    txtAddress1.Text = (string)jsoncancel.SelectToken("Address1"); ;
            //    txtAddress2.Text = (string)jsoncancel.SelectToken("Address2");
            //    cmbCity.Text = (string)jsoncancel.SelectToken("City");
            //    txtstate.Text = (string)jsoncancel.SelectToken("State");
            //    txtPincode.Text = (string)jsoncancel.SelectToken("PinCode");
            //    txtStateCode.Text = (string)jsoncancel.SelectToken("StateCode");
            //    txtConGSTIN.Text = (string)jsoncancel.SelectToken("GSTIN");
            //    //txtConGSTNo.Text = State[0].GSTIN_NO;
            //}
            //else
            //{
            //    txtAddress1.Text = txtConAddress.Text;
            //}
            //groupBox3.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //genConsigneeAddress CAddr = new genConsigneeAddress();
            //CAddr.Address1 = txtAddress1.Text;

            //CAddr.Address2 = txtAddress2.Text; ;
            //CAddr.City = cmbCity.Text;
            //CAddr.PinCode = Convert.ToInt32(txtPincode.Text);
            //CAddr.State = txtstate.Text;
            //CAddr.StateCode = txtStateCode.Text;
            //CAddr.GSTIN = txtConGSTIN.Text;
            //JavaScriptSerializer serializer = new JavaScriptSerializer();

            //string json = JsonConvert.SerializeObject(CAddr, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
            //{
            //    NullValueHandling = NullValueHandling.Ignore
            //});

            //txtConAddress.Text = json;
            //groupBox3.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //groupBox3.Visible = false;
        }

        private void dpInvDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            try
            {
              
                string SO_No = txtInvNo.Text;


                path = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "LoadingSlip.pdf");
                //string path = @"D:\Invoice.pdf";
                FileInfo fi1 = new FileInfo(path);
                DataTable Dt = new DataTable();

                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();


                rep = new OrderManagement.Transactions.LoadingSlip();

                SqlCommand cmd = new SqlCommand("sp_Rpt_LoadingSlip", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Slip_No", SO_No);
                cmd.Parameters.AddWithValue("@Creation_Company", logIn.company);
                cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                //DataTable Dt = new DataTable();

                da.SelectCommand = cmd;
                da.Fill(Dt);



                if (Dt.Rows.Count > 0)
                {


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
                    rep.SetDataSource(Dt);


                    //rep.SetParameterValue("Invoice_No", SO_No);
                    //rep.SetParameterValue("Creation_Company", logIn.company);
                    ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                    // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                    viewer.crystalReportViewer1.ReportSource = rep;
                    viewer.crystalReportViewer1.Refresh();
                    rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                    //cmd.Parameters.Clear();
                }
                con.Close();


                Process.Start(path);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
       
        private void dgProducts_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            
        }

        private void cmbCity_Leave(object sender, EventArgs e)
        {
            try
            {
                //var State = (from c in db.City_Masters
                //             where c.City_Name == cmbCity.Text
                //             select new { c.State_Name, c.State_Code }).ToList();
                //if (State.Count > 0)
                //{
                //    txtstate.Text = State[0].State_Name;
                //    txtStateCode.Text = State[0].State_Code;
                //    txtPincode.Focus();
                //}
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
                //if (txtCustPoNo.Text == "NA")
                //{
                //    R1.Cells["Basic_Price"].ReadOnly = false;
                //}
                //else
                //{
                //    R1.Cells["Basic_Price"].ReadOnly = true;
                //}
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
                        //txtCustGSTNo.Text = State[0].GSTIN_NO;
                        //txtCustStateCode.Text = State[0].StateCode;
                    }

                    var ST = (from c in db.Get_Sale_Turover(logIn.company,t,logIn.fy_Start_Date, logIn.fy_End_Date)
                                 
                                 select new { c.Total_Sales }).ToList();
                    if (ST.Count > 0)
                    {
                        //linkLabel2.Text = ST[0].Total_Sales.ToString(); ;
                        
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
                if (OrderManagement.Transactions.ListofLoadingSlips.SO_No != null)
                {
                    txtInvNo.Text = OrderManagement.Transactions.ListofLoadingSlips.SO_No;
                }
                else
                {
                    //txtInvNo.Text = frmCRMDashBoard.SO_No;
                }
                String myString = "";
                myString = txtInvNo.Text;
                var da = (from obj in db.Loading_Masters
                          where obj.Loading_Slip_No == txtInvNo.Text && obj.Company_ID == logIn.company
                          select obj).ToList();

                if (da.Count > 0)
                {
                    txtInvNo.Text = da[0].Loading_Slip_No.ToString();
                    dpInvDate.Text = da[0].Slip_Date.ToString();
                  
                    CmbBuyerName.SelectedValue = da[0].BuyerName;
                    
                   
                    txtSoNo.Text = da[0].SO_Ref_No;
                    txtCustPoNo.Text = da[0].CustomerPONo;
                               
                   
                    txtTotalQty.Text = da[0].TotalQty.ToString();                  
                    cmbTransporter.Text = da[0].Transporter_Name;                   
                    txtVehicleNo.Text = da[0].VehicleNo;
                    //txtWayBillNo.Text = da[0].WayBillNo;
                    txtSplInstructions.Text = da[0].Spl_Instructions;                    
                    cmbStatus.SelectedValue = da[0].Status;      
                    lblCreatedBy.Text = da[0].Created_By;
                    lblModified.Text = da[0].Modified_By;
                    txtSoNo.Text = da[0].SO_Ref_No;
                    cmbAssignedTo.Text = da[0].Assigned_To;
                    dtIssueTime.Text = da[0].Issue_Time.ToString();
                    dtCompletionTime.Text = da[0].Loading_Completion_Time.ToString();

                    txtDestination.Text = da[0].Destination;
                    txtBUID.Text = da[0].BU_ID.ToString();
                   

                }


                var dm1 = (from s in db.Loading_Childs
                           join q in db.Sale_Order_Childs on new { X1 = s.SO_Ref_No, X2 = s.SO_Item_No } equals new { X1 = q.SO_NO, X2 = q.enq_item_no }
                            join qm in db.Sale_Order_Masters on q.So_Master_ID equals qm.Id
                            join p in db.Products on s.Prod_Code equals p.prod_ID 
                            join u in db.UoM_Masters on p.Prod_Primary_UOM_Id equals u.UOM_ID
                           where s.Loading_Slip_No == myString && s.Company_ID == logIn.company orderby s.Item_No

                           select new

                           {
                               Item_No = s.Item_No,
                               Item_Code =s.Prod_Code,
                               Prod_Code = p.Prod_Code,
                               Item_Description =p.Prod_Name.Trim(),
                               Item_Grade =q.Prod_Grade.Trim(),
                               s.No_Of_Pcs,
                               Prod_Length = q.Prod_Length.Trim(),
                               UOM= u.Uom_Descr.Trim(),
                               PO_Qty = q.Qty,
                               Bal_Qty = s.PO_Qty,
                               Stock_Qty = s.Stock_qty,                               
                               Inv_Qty = s.Qty_To_Load,
                               Length_Loaded = s.Length_Loaded.Trim(),
                               s.Qty_Loaded,                               
                               s.No_pcs_Loaded,                      
                               SO_Ref_No= s.SO_Ref_No.Trim(),                              
                               s.SO_Item_No,
                               Remarks = s.Remarks.Trim()


                           });


                 

                SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataTable dtr = new DataTable();
                da2.Fill(dtr);
                if (dtr.Rows.Count >= 0)
                    dgProducts.DataSource = dtr;

                decimal x = 0, y = 0;

                for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                {

                    x += (dgProducts.Rows[i].Cells["Inv_Qty"].Value == "" || dgProducts.Rows[i].Cells["Inv_Qty"].Value == null || dgProducts.Rows[i].Cells["Inv_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Inv_Qty"].Value);

                    y += (dgProducts.Rows[i].Cells["Qty_Loaded"].Value == "" || dgProducts.Rows[i].Cells["Qty_Loaded"].Value == null || dgProducts.Rows[i].Cells["Qty_Loaded"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty_Loaded"].Value);

                }

                txtTotalQty.Text = x.ToString(".00");
                txtQtyLoaded.Text = y.ToString(".00");
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
                    //cmbTaxClass.SelectedValue = da[0].Tax_Class;
                    //txtConAddress.Text = da[0].Delivery_Address;
                    //dpSODate.Text = da[0].SODate.ToString();
                    if (da[0].Multi_Loc_Delivery == true)
                    {
                        bindDeliveryAddress();
                    }
                    else
                    {
                        //bindCustomer();
                        //txtConAddress.Text = da[0].Delivery_Address;
                        CmbBuyerName.SelectedValue = da[0].BuyerName;
                        bindConsignee();
                        //CmbConsigneeName.SelectedValue = da[0].ConsigneeName;
                        //txtCustGSTNo.Text = da[0].Cust_GST_No;
                        
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
                    //dpPODate.Text = da[0].PODate.ToString();
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
