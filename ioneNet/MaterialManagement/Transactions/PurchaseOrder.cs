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
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using OpenCvSharp;

namespace ioneNet.MaterialManagement
{
    public partial class PurchaseOrder : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();
        public static string SONo,ItemCode,OrdQty, var ,transname, transno, PO_Basis;
        decimal cgstPer, sgstPer, igstPer;
        public string comnpstatecode, suppStateCode, reviewed = "", approved = "";
        public PurchaseOrder()
        {
            InitializeComponent();
        }

        private void frmNewOrder_Load(object sender, EventArgs e)
        {
           // dpSODate.MinDate = logIn.fy_Start_Date;
            //dpSODate.MaxDate = logIn.fy_End_Date;
           
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

            bindCustomer();
            //bindConsignee();
            bindDroupDown_Lookup();
            BindShippingAddress();
            cmbasset.Visible = false;
            label71.Visible = false;
            if (logIn.company == 1043)
            {
                bindproj();
                cmbasset.Visible = true;
                label71.Visible = true;
            }

            if (MaterialManagement.PurchaseOrdersList.var == "0" || MaterialManagement.PurchaseOrdersList.var == "1")
            {
                if (PurchaseOrdersList.editMode == true)
                {
                    txtSoNo.Text = PurchaseOrdersList.SO_No;
                    bindedit();
                }
                else
                {
                    AutoincrementId();
                }
            }
            else
            if (MaterialManagement.frmMMDashBoard.var == "0")
            {
                if (MaterialManagement.frmMMDashBoard.editMode == true)
                {
                    txtSoNo.Text = MaterialManagement.frmMMDashBoard.SO_No;
                    bindedit();
                }
            }
            else
            if (MaterialManagement.PurchaseOrdersList.var == "2")
            {
                if (MaterialManagement.PurchaseOrdersList.editMode == true)
                {
                    txtSoNo.Text = PurchaseOrdersList.SO_No;
                    bindedit();
                    btnSave.Enabled = false;
                    btnClear.Enabled = false;

                }
            }
            else
            if (MaterialManagement.frmMMDashBoard.var == "2")
            {
                if (MaterialManagement.frmMMDashBoard.editMode == true)
                {
                    txtSoNo.Text = MaterialManagement.frmMMDashBoard.SO_No;
                    bindedit();
                    btnSave.Enabled = false;
                    btnClear.Enabled = false;

                }
            }
            else
            {
                AutoincrementId();
            }
            
        }
        public void bindproj()
        {
            var pcode = (from m in db.Project_code_Masters where m.Company_ID == logIn.company select new { m.id, m.Project_Code }).Distinct().ToList();
            if (pcode.Count > 0)
            {
                cmbasset.DataSource = pcode;
                cmbasset.ValueMember = "id";
                cmbasset.DisplayMember = "Project_Code";
            }
            cmbasset.SelectedIndex = -1;
        }
        private void BindShippingAddress()
        {
            
            
            var State = (from c in db.Costing_Units
                         where c.id == logIn.BU_ID
                         select new {c.ToPrintName,  c.GST_No, c.Address, c.City, c.State, c.PinCode,c.State_Code }).ToList();
            if (State.Count > 0)
            {
                genConsigneeAddress CAddr = new genConsigneeAddress();
                CAddr.ShipTo = State[0].ToPrintName;

                CAddr.Address = State[0].Address; ;
                CAddr.City = State[0].City;
                CAddr.PinCode = Convert.ToInt32(State[0].PinCode);
                CAddr.State = State[0].State;
                CAddr.StateCode = State[0].State_Code;
                CAddr.GSTIN = State[0].GST_No;
                JavaScriptSerializer serializer = new JavaScriptSerializer();

                string json = JsonConvert.SerializeObject(CAddr, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                });

                txtShippingAddress.Text = json;
               // txtShippingAddress.Text = State[0].ToPrintName +","+  State[0].Address + "," + State[0].City + "," + State[0].PinCode + "," + State[0].State +","+State[0].State_Code + ", GSTIN :" + State[0].GST_No;

                

            }
        }

       
       
        

        private void btnClose_Click(object sender, EventArgs e)
        {
            if (txtSoNo.Text != "")
            {
                var ci = db.Purchase_Order_Masters.Where(w => w.PO_NO == txtSoNo.Text && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).ToList();
                if (ci.Count > 0)
                {

                    if (dgProducts.Rows[0].Cells["Item_Code"].Value == null)
                    {
                        MessageBox.Show("Atleast One Record To Be Entered To Proceed", "Purchase Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        this.Close();
                    }
                }
                else
                {
                    //Masters.ProdSearch.dtgetfinalprducts = null;
                    this.Close();
                }
            }
            else
            {
                this.Close();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {


            try
            {
                if (txtSoNo.Text != "")
                {
                    if (dgProducts.Rows[0].Cells["Item_Code"].Value == null)
                    {
                        MessageBox.Show("Atleast One Record To Be Entered To Proceed", "Purchase Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        if (checkBox1.Checked)
                        {
                            for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                            {

                                if(dgProducts.Rows[i].Cells["Item_Grade"].Value == "" || dgProducts.Rows[i].Cells["Item_Grade"].Value == null || dgProducts.Rows[i].Cells["Item_Grade"].Value == DBNull.Value)
                                {
                                    MessageBox.Show("Item Grade Cannot Be Blank, Please select from the List");
                                    return;
                                }

                            }
                        }
                    }

                    if (CmbBuyerName.Text == string.Empty)
                    {
                        MessageBox.Show("Supplier Name Should Not Be Empty", "Purchase Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        CmbBuyerName.Focus();
                        return;
                    }
                    else if (txtCustPoNo.Text == string.Empty)
                    {
                        MessageBox.Show("Supplier Quote No Should Not Be Empty");
                        txtCustPoNo.Focus();
                        return;
                    }
                    else if (cmbBasis.Text == string.Empty)
                    {
                        //MessageBox.Show("Select Quotation No,");
                        //cmbQuotNo.Focus();
                        //return;
                    }
                    else if (cmbStatus.Text == string.Empty)
                    {
                        MessageBox.Show("Please Select Status");
                        cmbStatus.Focus();
                        return;
                    }
                    else if (cmbModeofDesp.Text == string.Empty)
                    {
                        MessageBox.Show("Please Select Mode of Despatch");
                        cmbModeofDesp.Focus();
                        return;
                    }
                    else if (cmbPaymentTerms.Text == string.Empty)
                    {
                        MessageBox.Show("Please Select Payment Terms");
                        cmbPaymentTerms.Focus();
                        return;
                    }
                    else if (cmbPriceBasis.Text == string.Empty)
                    {
                        MessageBox.Show("Please Select Price Basis");
                        cmbPriceBasis.Focus();
                        return;
                    }
                    else                    
                    if (cmbStatus.Text == "Approved")
                    {

                        var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Purchase Order" && m.Role_ID == logIn.UserRoleID select new { m.Approve_Role }).Distinct().ToList();
                        if (uRole.Count > 0)
                        {
                            if (uRole[0].Approve_Role == true)
                            {
                                SaveNew_Sql_proc();
                            }
                            else
                            {
                                MessageBox.Show("You Have No Permissions to Approve The Purchase Orders");
                                return;
                            }
                        }

                    }
                    else
                    {
                        SaveNew_Sql_proc();
                        //Save();
                    }
                }
                else
                {
                    MessageBox.Show("Enter PO NO");
                    txtSoNo.Focus();
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
                var Buyerblind = (from m in db.Supplier_informations
                                  join a in db.Attributes_Datas on m.Supplier_Category equals a.ID
                                  where m.Company_ID == logIn.company    &&  m.Status == 1
                                  && a.Descr != "Customer" select new { m.ID, m.Supplier_Name }).Distinct().ToList();
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
                var Buyerblind = (from m in db.Customer_informations select new { m.ID, m.Customer_Alias_Name }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    //CmbBuyerName.DataSource = Buyerblind;
                    //CmbBuyerName.ValueMember = "ID";
                    //CmbBuyerName.DisplayMember = "Customer_Alias_Name";

                    CmbConsigneeName.DataSource = Buyerblind;
                    CmbConsigneeName.ValueMember = "ID";
                    CmbConsigneeName.DisplayMember = "Customer_Alias_Name";

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
                var PurBasis = (from m in db.Attributes_Datas where m.Head_Name == "Purchase Basis PO" && (m.Company_ID == logIn.company || m.Company_ID == 0) select new { m.ID, m.Descr }).Distinct().ToList();
                if (PurBasis.Count > 0)
                {
                    cmbBasis.DataSource = PurBasis;
                    cmbBasis.ValueMember = "ID";
                    cmbBasis.DisplayMember = "Descr";
                }

                //Price Basis
                var PBasis = (from m in db.Attributes_Datas where m.Head_Name == "Price Basis"  && (m.Company_ID == logIn.company || m.Company_ID == 0) select new { m.ID, m.Descr }).Distinct().ToList();
                if (PBasis.Count > 0)
                {
                    cmbPriceBasis.DataSource = PBasis;
                    cmbPriceBasis.ValueMember = "ID";
                    cmbPriceBasis.DisplayMember = "Descr";                    
                }
                //Payment Terms
                var PTerms = (from m in db.Attributes_Datas where m.Head_Name == "Payment Terms" && (m.Company_ID == logIn.company || m.Company_ID == 0) select new { m.ID, m.Descr }).Distinct().ToList();
                if (PTerms.Count > 0)
                {
                    cmbPaymentTerms.DataSource = PTerms;
                    cmbPaymentTerms.ValueMember = "ID";
                    cmbPaymentTerms.DisplayMember = "Descr";
                }

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


                //var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Trans" && (m.Company_ID == logIn.company || m.Company_ID == 0)  select new { m.ID, m.Descr }).Distinct().ToList();
                //if (pStatus.Count > 0)
                //{
                //    cmbStatus.DataSource = pStatus;
                //    cmbStatus.ValueMember = "ID";
                //    cmbStatus.DisplayMember = "Descr";
                //}
                //Insurance
                var pIns = (from m in db.Attributes_Datas where m.Head_Name == "Insurance" && (m.Company_ID == logIn.company || m.Company_ID == 0)  select new { m.ID, m.Descr }).Distinct().ToList();
                if (pIns.Count > 0)
                {
                    cmbInsurance.DataSource = pIns;
                    cmbInsurance.ValueMember = "ID";
                    cmbInsurance.DisplayMember = "Descr";
                }

                //Transportation
                var pTrans = (from m in db.Attributes_Datas where m.Head_Name == "Transportation" && (m.Company_ID == logIn.company || m.Company_ID == 0)  select new { m.ID, m.Descr }).Distinct().ToList();
                if (pTrans.Count > 0)
                {
                    cmbTransport_Scope.DataSource = pTrans;
                    cmbTransport_Scope.ValueMember = "ID";
                    cmbTransport_Scope.DisplayMember = "Descr";
                }
                //Mode of Desp
                var pDesp = (from m in db.Attributes_Datas where m.Head_Name == "Mode Of Despatcg" && (m.Company_ID == logIn.company || m.Company_ID == 0) select new { m.ID, m.Descr }).Distinct().ToList();
                if (pDesp.Count > 0)
                {
                    cmbModeofDesp.DataSource = pDesp;
                    cmbModeofDesp.ValueMember = "ID";
                    cmbModeofDesp.DisplayMember = "Descr";
                }


                //shipTo
                var SO = (from m in db.Costing_Units where m.Company == logIn.company select new { m.id, m.BU_Name }).Distinct().ToList();
                if (SO.Count > 0)
                {
                    cmbShipTo.DataSource = SO;
                    cmbShipTo.ValueMember = "id";
                    cmbShipTo.DisplayMember = "BU_Name";
                }

                //Currency Master
                var Cur = (from m in db.Currency_Masters select new { m.id, m.Currency_Code }).Distinct().ToList();
                if (Cur.Count > 0)
                {
                    cmbFCurrency.DataSource = Cur;
                    cmbFCurrency.ValueMember = "id";
                    cmbFCurrency.DisplayMember = "Currency_Code";
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

                if (checkBox1.Checked == true)
                {
                    var result = db.Sp_autoincrement_PurchaseOrder_RM(logIn.company, logIn.BU_ID, logIn.fy_Start_Date, logIn.fy_End_Date);
                    txtSoNo.Text = result.FirstOrDefault().So_no;
                }
                else
                {
                    var result = db.Sp_autoincrement_PurchaseOrder(logIn.company, logIn.BU_ID, logIn.fy_Start_Date, logIn.fy_End_Date);
                    txtSoNo.Text = result.FirstOrDefault().So_no;
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
                else
                {
                    if (tb3 != null && columnName == "Item Description")
                    {
                        tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                        tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                        addItems(DataColl);
                        tb3.AutoCompleteCustomSource = DataColl;
                    }
                    else
                    {
                        if (tb3 != null && columnName == "Make /Model /Grade")
                        {
                            if (checkBox1.Checked)
                            {
                                tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                                tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                                AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                                addItems(DataColl);
                                tb3.AutoCompleteCustomSource = DataColl;
                            }
                        }

                        else
                        {
                            tb3.AutoCompleteMode = AutoCompleteMode.None;
                            //tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        }
                    }
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
                        var Prodname = (from d in db.Products where d.Company_ID == logIn.company && d.Prod_Status_ID == 1  select new { d.Prod_Name }).ToList();
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
                        if (columnName == "Make /Model /Grade")
                        {
                            if (checkBox1.Checked)
                            {
                                var Prodname = (from d in db.QA_Mtrl_Grade_Masters  select new { d.Material_Grade }).ToList();
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
                if (CmbBuyerName.Text != "")
                {
                   
                    int i = (CmbBuyerName.FindString(CmbBuyerName.Text));
                    if (i < 0)
                    {
                        MessageBox.Show("Invalid Supplier Name Selected");
                        CmbBuyerName.Focus();

                    }
                    else
                    {
                        //Get Supplier PO data as on date

                        var getPOs = (from b in db.SupplierWise_Data_In_PO(logIn.company, Convert.ToInt32(CmbBuyerName.SelectedValue))
                                      select new { b.Total_PO, b.Pending_Po, b.Pending_Val }).FirstOrDefault();
                        if (getPOs != null)
                        {
                            linkLabel6.Text = getPOs.Total_PO.ToString();
                            linkLabel5.Text = getPOs.Pending_Po.ToString();
                            linkLabel1.Text = getPOs.Pending_Val.ToString();
                            //R1.Cells["Balance_Type"].Value = getBal.BalType.ToString();
                        }
                        else
                        {
                            linkLabel6.Text = "0";
                            linkLabel5.Text = "0";
                            linkLabel1.Text = "0";
                        }

                        ////Get Accopunt Balance
                        //var getProductName = (from s in db.AccountMasters
                        //                      where s.AccName == CmbBuyerName.Text && s.Company_ID == logIn.company
                        //                      select new { s.id }).FirstOrDefault();

                        //if (getProductName != null)
                        //{

                        //    //R1.Cells["Acc_ID"].Value = getProductName.id.ToString();
                        //    DateTime t = dpPODate.Value;
                        //    string t1 = t.ToString("dd/MMM/yyyy");
                        //    var getBal = (from b in db.GetAccountBalance(logIn.company, t, Convert.ToInt32(getProductName.id.ToString()), 1, logIn.BU_ID)
                        //                  select new { b.Balance, b.BalType }).FirstOrDefault();
                        //    if (getBal != null)
                        //    {
                        //        decimal bal = Convert.ToDecimal(getBal.Balance.ToString());
                        //        linkLabel7.Text = (bal / 100000).ToString();
                        //        //R1.Cells["Balance_Type"].Value = getBal.BalType.ToString();
                        //    }
                        //    else
                        //    {
                        //        linkLabel7.Text = "0";
                        //    }
                        //}
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
                DataTable dtexisting = new DataTable();
                if (e.KeyCode == Keys.F2)
                {
                    //try
                    //{
                    //    if (CmbBuyerName.Text == "")
                    //    {
                    //        MessageBox.Show("Select Supplier Name");
                    //        CmbBuyerName.Focus();
                    //    }
                    //}
                    //catch (Exception ex)
                    //{
                    //    MessageBox.Show(ex.Message);
                    //}
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

                        dtexisting.Columns.Add("UOM", typeof(string));
                        dtexisting.Columns.Add("PR_Qty", typeof(string));
                        dtexisting.Columns.Add("Qty", typeof(decimal));
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
                        dtexisting.Columns.Add("PR_No", typeof(string));
                        dtexisting.Columns.Add("Remarks", typeof(string));



                        for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                        {
                            if (dgProducts.Rows[i].Cells["Item_Description"].Value != null || dgProducts.Rows[i].Cells["Item_Description"].Value != "")
                            {
                                DataRow dr;
                                dr = dtexisting.NewRow();
                                dr["S_No"] = dgProducts.Rows[i].Cells["S_No"].Value.ToString();
                                dr["Item_Code"] = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                                dr["Prod_Code"] = dgProducts.Rows[i].Cells["Prod_Code"].Value.ToString();
                                dr["Item_Description"] = dgProducts.Rows[i].Cells["Item_Description"].Value.ToString();
                                dr["Item_Spec"] = (dgProducts.Rows[i].Cells["Item_Spec"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Item_Spec"].Value);
                                dr["Item_Grade"] = (dgProducts.Rows[i].Cells["Item_Grade"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value);
                                dr["UOM"] = dgProducts.Rows[i].Cells["UOM"].Value.ToString();
                                dr["PR_Qty"] = (dgProducts.Rows[i].Cells["PR_Qty"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["PR_Qty"].Value);
                                dr["Qty"] = dgProducts.Rows[i].Cells["Qty"].Value.ToString();
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
                                dr["PR_No"] = (dgProducts.Rows[i].Cells["PR_No"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["PR_No"].Value);
                                dr["Remarks"] = (dgProducts.Rows[i].Cells["Remarks"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value);

                                dtexisting.Rows.Add(dr);
                            }

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
                        dt.Columns.Add("UOM", typeof(string));
                        dt.Columns.Add("PR_Qty", typeof(string));
                        dt.Columns.Add("Qty", typeof(decimal));
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
                        dt.Columns.Add("PR_No", typeof(string));
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
                                                   Item_Spec = "",
                                                   Item_Grade = obj.Prod_Field2,
                                                   UOM = uom.Uom_Descr,
                                                   PR_Qty = "",
                                                   Qty = 0,
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
                                                   PR_No = "",
                                                   Remarks = "",

                                               }).ToList();
                            j = j + 1;
                            dt.Rows.Add(j, getproducts[0].Item_Code,
                                                   getproducts[0].Prod_Code,
                                                   getproducts[0].Item_Description,
                                                   getproducts[0].Item_Spec,
                                                   getproducts[0].Item_Grade, getproducts[0].UOM, getproducts[0].PR_Qty, getproducts[0].Qty, getproducts[0].Basic_Price, getproducts[0].Amt_Before_Disc, getproducts[0].Disc_Per, getproducts[0].Disc_Amt, getproducts[0].Taxable_Value, getproducts[0].CGST_Per, getproducts[0].CGST_Amt, getproducts[0].SGST_Per, getproducts[0].SGST_Amt, getproducts[0].IGST_Per, getproducts[0].IGST_Amt, getproducts[0].Total_Amount, getproducts[0].PR_No, getproducts[0].Remarks);

                        }

                        dtexisting = dtexisting.AsEnumerable().Union(dt.AsEnumerable()).CopyToDataTable();
                        dgProducts.DataSource = dtexisting;

                        

                    }
                }
                
                if (e.KeyCode == Keys.F4) //Delivery Information / Locations
                {
                    if (chkMultiLocation.Checked == true)
                    {
                        ioneNet.OrderManagement.Transactions.SO_MultiDeliveryAddress form = new OrderManagement.Transactions.SO_MultiDeliveryAddress();
                        //ioneNet.Masters.ProdSearch.frmName = "SOrder";
                        int i = dgProducts.CurrentCell.RowIndex;
                        SONo = txtSoNo.Text;
                        ItemCode = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                        OrdQty = dgProducts.Rows[i].Cells["Qty"].Value.ToString();
                        form.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Check Multi Location Delivery? To Add The Details");

                    }
                }
                if (e.KeyCode == Keys.F6 ) //Remove Rows
                {
                    if (dgProducts.Rows.Count > 0)
                    {

                        foreach (DataGridViewCell oneCell in dgProducts.SelectedCells)
                        {
                            //ask for permission
                            DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                            {
                                int i = dgProducts.CurrentCell.RowIndex;
                                SONo = txtSoNo.Text;
                                if (dgProducts.Rows[i].Cells["S_No"].Value == null)
                                {
                                    dgProducts.Rows.RemoveAt(dgProducts.CurrentRow.Index);
                                }
                                else
                                {
                                    ItemCode = dgProducts.Rows[i].Cells["S_No"].Value.ToString();
                                    SqlCommand cmd1 = new SqlCommand("delete  from [Purchase_Order_Child] where [ProdSno] =@ProdID and [PO_NO] = @pono", con);
                                    cmd1.Parameters.AddWithValue("@ProdID", ItemCode);
                                    cmd1.Parameters.AddWithValue("@pono", SONo);
                                    if (con.State != ConnectionState.Open)
                                        con.Open();
                                    //con.Open();
                                    cmd1.ExecuteNonQuery();
                                    con.Close();
                                    if (oneCell.Selected)
                                        dgProducts.Rows.RemoveAt(oneCell.RowIndex);

                                    GetTot();
                                }
                                
                            }
                            int j = 0;
                            //for (int k = 0; k < dgProducts.Rows.Count - 1; k++)
                            //{
                            //    j = j + 1;
                            //    dgProducts.Rows[k].Cells["S_No"].Value = j.ToString();
                            //}
                        }
                        GetTot();
                        //decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
                        //for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                        //{

                        //    x += (dgProducts.Rows[i].Cells["Qty"].Value == "" || dgProducts.Rows[i].Cells["Qty"].Value == null || dgProducts.Rows[i].Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty"].Value);
                        //    y += (dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == "" || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == null || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value);
                        //    q += (dgProducts.Rows[i].Cells["Disc_Amt"].Value == "" || dgProducts.Rows[i].Cells["Disc_Amt"].Value == null || dgProducts.Rows[i].Cells["Disc_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Amt"].Value);
                        //    v += (dgProducts.Rows[i].Cells["Taxable_Value"].Value == "" || dgProducts.Rows[i].Cells["Taxable_Value"].Value == null || dgProducts.Rows[i].Cells["Taxable_Value"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Taxable_Value"].Value);
                        //    cg += (dgProducts.Rows[i].Cells["CGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["CGST_Amt"].Value == null || dgProducts.Rows[i].Cells["CGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Amt"].Value);
                        //    sg += (dgProducts.Rows[i].Cells["SGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["SGST_Amt"].Value == null || dgProducts.Rows[i].Cells["SGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Amt"].Value);
                        //    ig += (dgProducts.Rows[i].Cells["IGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["IGST_Amt"].Value == null || dgProducts.Rows[i].Cells["IGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Amt"].Value);
                        //    totA += (dgProducts.Rows[i].Cells["Total_Amount"].Value == "" || dgProducts.Rows[i].Cells["Total_Amount"].Value == null || dgProducts.Rows[i].Cells["Total_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Total_Amount"].Value);

                        //}

                        //txtTotalQty.Text = x.ToString(".00");
                        //txtSubTotal.Text = y.ToString("0.00");
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
                string pcode = "";

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
                                     data.Prod_Field2
                                 }).ToList();

                    if (d.Count > 0)
                    {
                        R1.Cells["UOM"].Value = d[0].UOM.ToString();
                        R1.Cells["Item_code"].Value = d[0].Item_Code.ToString();

                        R1.Cells["Prod_Code"].Value = d[0].Prod_Code.ToString();

                        if (d[0].Prod_Field2 != null)
                        {
                            R1.Cells["Item_Grade"].Value = d[0].Prod_Field2.ToString();
                        }
                        if (R1.Cells["S_no"].Value == null || R1.Cells["S_no"].Value.ToString() == "")
                        {
                            R1.Cells["S_no"].Value = dgProducts.Rows.Count - 1;
                        }
                        R1.Cells["Item_Description"].Value = d[0].Item_Description.ToString();
                        R1.Cells["Qty"].Value = 0;
                        R1.Cells["Basic_Price"].Value = 0;
                        R1.Cells["Amt_Before_Disc"].Value = 0;
                        R1.Cells["Disc_Per"].Value = 0;
                        R1.Cells["Disc_Amt"].Value = 0;
                        R1.Cells["Taxable_Value"].Value = 0;

                        R1.Cells["CGST_Per"].Value = 0;
                        R1.Cells["CGST_Amt"].Value = 0;
                        R1.Cells["SGST_Per"].Value = 0;
                        R1.Cells["SGST_Amt"].Value = 0;
                        R1.Cells["IGST_Per"].Value = 0;
                        R1.Cells["IGST_Amt"].Value = 0;
                        R1.Cells["Total_Amount"].Value = 0;

                    }
                    else
                    {
                        MessageBox.Show("Record Not Found");
                        return;
                    }
                


                //var getProductName = (from s in db.Products
                //                          join u in db.UoM_Masters on s.Prod_Primary_UOM_Id equals u.UOM_ID
                //                          join g in db.Product_Groups on s.Prod_Group_Id equals g.ID
                //                          where s.Prod_Alternative_Code == R1.Cells["Prod_Code"].Value.ToString() && s.Company_ID == logIn.company && s.Purchase_Account == logIn.BU_ID
                //                          select new { s.prod_ID, u.Uom_Descr, g.Prod_Group_Name, s.Prod_HSN_Code, s.Prod_Code, s.Prod_Field2, s.Prod_Name }).ToList();
                //    if (getProductName.Count > 0)
                //    {
                //        if (getProductName != null)
                //        {
                //            R1.Cells["UOM"].Value = getProductName[0].Uom_Descr.ToString();
                //            R1.Cells["Item_code"].Value = getProductName[0].prod_ID.ToString();
                //            R1.Cells["Item_Description"].Value = getProductName[0].Prod_Name.ToString();
                //            if (getProductName[0].Prod_Field2 != null)
                //            {
                //                R1.Cells["Item_Grade"].Value = getProductName[0].Prod_Field2.ToString();
                //            }
                //            if (R1.Cells["S_no"].Value == null || R1.Cells["S_no"].Value.ToString() == "")
                //            {
                //                R1.Cells["S_no"].Value = dgProducts.Rows.Count - 1;
                //            }
                //            R1.Cells["Qty"].Value = 0;
                //            R1.Cells["Basic_Price"].Value = 0;
                //            R1.Cells["Amt_Before_Disc"].Value = 0;
                //            R1.Cells["Disc_Per"].Value = 0;
                //            R1.Cells["Disc_Amt"].Value = 0;
                //            R1.Cells["Taxable_Value"].Value = 0;

                //            R1.Cells["CGST_Per"].Value = 0;
                //            R1.Cells["CGST_Amt"].Value = 0;
                //            R1.Cells["SGST_Per"].Value = 0;
                //            R1.Cells["SGST_Amt"].Value = 0;
                //            R1.Cells["IGST_Per"].Value = 0;
                //            R1.Cells["IGST_Amt"].Value = 0;
                //            R1.Cells["Total_Amount"].Value = 0;
                //        }
                //    }
                //    else
                //    {
                //        //R1.Cells["Item_Code"].Value = dgProducts.CurrentCell.RowIndex + 1;
                //        MessageBox.Show("Invalid Product Name");

                //        R1.Cells["Item_Description"].Value = "";

                //        return;
                //    }

                    //Get product Last Purchase Data

                    var getPOs = (from b in db.ProductWise_Data_In_PO(logIn.company, Convert.ToInt32(d[0].Item_Code.ToString()))
                                  select new { b.last_Purchase_Date, b.Price }).ToList();
                    if (getPOs.Count > 0)
                    {
                        if (getPOs[0].last_Purchase_Date != null)
                        {
                            linkLabel2.Text = getPOs[0].last_Purchase_Date.ToString();
                            linkLabel3.Text = getPOs[0].Price.ToString();
                        }
                        //R1.Cells["Balance_Type"].Value = getBal.BalType.ToString();
                    }
                    else
                    {
                        linkLabel2.Text = "";
                        linkLabel3.Text = "0";
                    }


                    //Get Item Current Stock
                    DateTime t = dpPODate.Value;
                    string dt1 = t.ToString("yyyy/MM/dd");
                    var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(R1.Cells["Item_code"].Value.ToString()), Convert.ToDateTime(dt1), logIn.BU_ID) select data).ToList();
                    linkLabel4.Text = "0";
                    if (stock.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        linkLabel4.Text = stock[0].ClosingQty.ToString();
                    }
                //}


                //if (columnName == "Item_Description" && R1.Cells["Item_Description"].Value != null)
                //{
                //    var getProductName = (from s in db.Products
                //                          join u in db.UoM_Masters on s.Prod_Primary_UOM_Id equals u.UOM_ID
                //                          join g in db.Product_Groups on s.Prod_Group_Id equals g.ID
                //                          where s.Prod_Name == R1.Cells["Item_Description"].Value.ToString() && s.Company_ID == logIn.company && s.Purchase_Account == logIn.BU_ID
                //                          select new { s.prod_ID, u.Uom_Descr, g.Prod_Group_Name, s.Prod_HSN_Code,s.Prod_Code,s.Prod_Field2 , s.Prod_Alternative_Code}).ToList();
                //    if (getProductName.Count > 0)
                //    {
                //        if (getProductName != null)
                //        {
                //            R1.Cells["UOM"].Value = getProductName[0].Uom_Descr.ToString();
                //            R1.Cells["Item_code"].Value = getProductName[0].prod_ID.ToString();
                //            if(logIn.company ==1044)
                //            {
                //                R1.Cells["Prod_Code"].Value = getProductName[0].Prod_Alternative_Code.ToString();
                //            }
                //            {
                //                R1.Cells["Prod_Code"].Value = getProductName[0].Prod_Code.ToString();
                //            }
                //            if (getProductName[0].Prod_Field2 != null)
                //            {
                //                R1.Cells["Item_Grade"].Value = getProductName[0].Prod_Field2.ToString();
                //            }
                //            if (R1.Cells["S_no"].Value == null || R1.Cells["S_no"].Value.ToString() == "")
                //            {
                //                R1.Cells["S_no"].Value = dgProducts.Rows.Count - 1;
                //            }
                //            R1.Cells["Qty"].Value = 0;
                //            R1.Cells["Basic_Price"].Value = 0;
                //            R1.Cells["Amt_Before_Disc"].Value = 0;
                //            R1.Cells["Disc_Per"].Value = 0;
                //            R1.Cells["Disc_Amt"].Value = 0;
                //            R1.Cells["Taxable_Value"].Value = 0;
                //            R1.Cells["CGST_Per"].Value = 0;
                //            R1.Cells["CGST_Amt"].Value = 0;
                //            R1.Cells["SGST_Per"].Value = 0;
                //            R1.Cells["SGST_Amt"].Value = 0;
                //            R1.Cells["IGST_Per"].Value = 0;
                //            R1.Cells["IGST_Amt"].Value = 0;
                //            R1.Cells["Total_Amount"].Value = 0;
                //        }
                //    }
                //    else
                //    {
                //        //R1.Cells["Item_Code"].Value = dgProducts.CurrentCell.RowIndex + 1;
                //        MessageBox.Show("Invalid Product Name");
                        
                //        R1.Cells["Item_Description"].Value = "";
                       
                //        return;
                //    }

                //    //Get product Last Purchase Data
                    
                //        var getPOs = (from b in db.ProductWise_Data_In_PO(logIn.company, Convert.ToInt32(getProductName[0].prod_ID.ToString()))
                //                      select new { b.last_Purchase_Date, b.Price }).ToList();
                //        if (getPOs.Count > 0)
                //        {
                //            if (getPOs[0].last_Purchase_Date != null)
                //            {
                //                linkLabel2.Text = getPOs[0].last_Purchase_Date.ToString();
                //                linkLabel3.Text = getPOs[0].Price.ToString();
                //            }
                //            //R1.Cells["Balance_Type"].Value = getBal.BalType.ToString();
                //        }
                //        else
                //        {
                //            linkLabel2.Text = "";
                //            linkLabel3.Text = "0";
                //        }

                    
                //    //Get Item Current Stock
                //    DateTime t = dpPODate.Value;
                //    string dt1 = t.ToString("yyyy/MM/dd");
                //    var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(R1.Cells["Item_code"].Value.ToString()), Convert.ToDateTime(dt1), logIn.BU_ID) select data).ToList();
                //    linkLabel4.Text = "0";
                //    if (stock.Count > 0)
                //    {
                //        //dgProductsList.DataSource = d;
                //        linkLabel4.Text =  stock[0].ClosingQty.ToString();                       
                //    }
                }

                if (columnName == "Qty" || columnName == "Disc_Per" || columnName == "Basic_Price")
                {
                   int Itemcode= Convert.ToInt32(R1.Cells["Item_Code"].Value.ToString());
                    decimal taxRate;
                    if (Itemcode!=null)
                    {
                        decimal b, c,tr;
                        decimal AcceptedQty = (R1.Cells["Qty"].Value == "" || R1.Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty"].Value);
                        decimal price = (R1.Cells["Basic_Price"].Value == "" || R1.Cells["Basic_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Basic_Price"].Value);
                        decimal DiscPer = (R1.Cells["Disc_Per"].Value == "" || R1.Cells["Disc_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Disc_Per"].Value);

                        decimal POQty = (R1.Cells["Qty"].Value == "" || R1.Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty"].Value);
                        decimal PRQty = (R1.Cells["PR_Qty"].Value == "" || R1.Cells["PR_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["PR_Qty"].Value);
                        if (columnName == "Qty")
                        {
                            if (cmbBasis.Text == "With PR")
                            {
                                if (logIn.company != 1044 && logIn.company !=1043 )
                                {
                                    if (POQty > (PRQty))
                                    {
                                        MessageBox.Show("PO Qty Cannot Be Greater Than PR Qty");
                                        dgProducts.CurrentCell = dgProducts.Rows[dgProducts.CurrentRow.Index].Cells[dgProducts.CurrentCell.ColumnIndex];
                                        R1.Cells["Qty"].Value = 0;

                                        // R1.Cells["ReceivedQty"].Selected = true;
                                        return;
                                    }
                                    else
                                    {
                                        //R1.Cells["Qty"].Value = (ReceivedQty - RejectedQty).ToString("0.00");
                                    }
                                }
                            }
                            else
                            {
                                R1.Cells["PR_Qty"].Value = "0";
                                R1.Cells["Disc_Per"].Value = "0";
                            }
                        }


                        //c = dgProducts.Rows[dgProducts.CurrentRow.Index];
                        var taxratelist = (from prd in db.Products join tax in db.Tax_Class_Masters on prd.Prod_Tax_Class equals tax.ID where prd.prod_ID == Itemcode select new { tax.Gst_Rate }).ToList();
                        if (taxratelist.Count > 0)
                        {
                            taxRate = Convert.ToDecimal(taxratelist[0].Gst_Rate); //Convert.ToInt32(getProduct_Name.GSTRate);
                        }
                        else
                        {
                            MessageBox.Show("Tax Clas Not Defined For the Selected Product");
                            return;
                        }
                        if (chkImportPO.Checked == false)
                        {
                            var d1 = (from a in db.Costing_Units where a.id == logIn.BU_ID select new { a.GST_No }).ToList();
                            if (d1.Count > 0)
                            {
                                comnpstatecode = Mid(d1[0].GST_No, 1, 2);
                                suppStateCode = txtStateCode.Text;
                                if(suppStateCode == "NA")
                                {
                                R1.Cells["CGST_Per"].Value = "0.00";
                                R1.Cells["SGST_Per"].Value = "0.00";
                                R1.Cells["IGST_Per"].Value = "0.00";
                                }
                                else
                                if (suppStateCode == comnpstatecode)
                                {
                                    tr = Convert.ToDecimal(taxRate) / 2;
                                    R1.Cells["CGST_Per"].Value = tr.ToString("0.00");
                                    R1.Cells["SGST_Per"].Value = tr.ToString("0.00");
                                    R1.Cells["IGST_Per"].Value = "0.00";
                                }
                                else
                                {
                                    tr = taxRate;
                                    R1.Cells["CGST_Per"].Value = "0.00";
                                    R1.Cells["SGST_Per"].Value = "0.00";
                                    R1.Cells["IGST_Per"].Value = tr.ToString("0.00");
                                }

                            }
                        }
                        else
                        {
                            R1.Cells["CGST_Per"].Value = "0.00";
                            R1.Cells["SGST_Per"].Value = "0.00";
                            R1.Cells["IGST_Per"].Value = "0.00";
                        }

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
                        GetTot();

                    }
                    else
                    {

                    }
                   

                }
                if(columnName == "Item_Grade")
                {
                    if(checkBox1.Checked)
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
                }
                if (columnName == "Item_Description" && R1.Cells["Item_Description"].Value != null)
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

        private void txtFrieght_Leave(object sender, EventArgs e)
        {
            try
            {
                GetTot();
                //if (txtFrieght.Text == null || txtFrieght.Text == "")
                //{

                //}
                //else
                //{
                //    decimal j = 0; decimal CGST_Amt = 0; decimal IGST_Amt = 0;
                //    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                //    {
                //        // CGST_Per
                //        decimal CGST_Per = Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Per"].Value.ToString());
                //        decimal IGST_Per = Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Per"].Value.ToString());
                //        CGST_Amt += Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Amt"].Value.ToString());
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
                //            if (j < IGST_Per + IGST_Per)
                //            {
                //                j = Convert.ToInt32(IGST_Per + IGST_Per);

                //            }
                //        }
                //    }
                //    decimal Freight = Convert.ToDecimal(txtFrieght.Text);
                //    int pervalue = Convert.ToInt32(j);
                //    decimal Freightmnt = (Freight * pervalue) / 100;
                //    double Fvalue = (Convert.ToInt32(Freightmnt));
                //    double FinalFreightamt = Math.Round(Fvalue);
                //    double Finalfrght = FinalFreightamt / 2;
                //    int CGST_Amount = Convert.ToInt32(CGST_Amt);
                //    int IGST_Amount = Convert.ToInt32(IGST_Amt);

                //    double cgstvalue = Convert.ToDouble(CGST_Amt);
                //    double igstvalue = Convert.ToDouble(IGST_Amt);
                //    double othercharges = 0;
                //    double otherchargesvalue = 0;
                //    double totalotherchargesvalue = 0;
                //    if (txtOtherCharges.Text == null || txtOtherCharges.Text == "")
                //    {
                //        othercharges = 0;
                //        otherchargesvalue = 0;
                //        totalotherchargesvalue = 0;
                //    }
                //    else
                //    {
                //        othercharges = Convert.ToDouble(txtOtherCharges.Text);
                //        otherchargesvalue = (txtOtherchargerAmount.Text == null || txtOtherchargerAmount.Text == "") ? 0 : Convert.ToDouble(txtOtherchargerAmount.Text);
                //        totalotherchargesvalue = othercharges + otherchargesvalue;
                //    }
                //    int FinalCGSTAMount = 0; int FinalSGSTAMount = 0; int FinalIGSTAMount = 0;
                //    // double otherchargesvalue = Convert.ToDouble(textBox1.Text==null|| textBox1.Text == "")?0.00:Convert.ToDouble(textBox1.Text);
                //    if (CGST_Amount != 1)
                //    {
                //        txtTot_CGST.Text = Convert.ToString(Finalfrght + cgstvalue);
                //        FinalCGSTAMount = Convert.ToInt32(Finalfrght + cgstvalue);
                //        txtTot_SGST.Text = Convert.ToString(Finalfrght + cgstvalue);
                //        FinalSGSTAMount = Convert.ToInt32(Finalfrght + cgstvalue);
                //    }
                //    else
                //    {
                //        txtTot_IGST.Text = Convert.ToString(Finalfrght + igstvalue);
                //        FinalIGSTAMount = Convert.ToInt32(Finalfrght + igstvalue);
                //    }

                //    txtFreightAmount.Text = Convert.ToString(FinalFreightamt);
                //    decimal TotalTaxbleValue = Convert.ToDecimal(txtTot_TaxableValue.Text);
                //    double TotalOrderValue = Convert.ToDouble(TotalTaxbleValue + Freight + FinalCGSTAMount + FinalSGSTAMount + FinalIGSTAMount + Convert.ToInt32(othercharges + otherchargesvalue));
                //    txtTot_OrderValue.Text = Convert.ToString(Math.Round(TotalOrderValue));

                //}
            }
            catch (Exception ex)
            {

                
            }

           
        }

        private void dgProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void CmbBuyerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (CmbBuyerName.Text != "")
            {
                var gstno = (from c in db.Supplier_informations
                             where c.Supplier_Name == CmbBuyerName.Text && c.Company_ID == logIn.company
                             select new { c.GSTIN_NO, c.Contact_Person, c.Contact_Mobile, c.Contact_Email, c.StateCode }).ToList();
                if (gstno.Count > 0)
                {
                    txtCustGSTNo.Text = gstno[0].GSTIN_NO;
                    txtcontactperson.Text = gstno[0].Contact_Person;
                    txtcontactEmail.Text = gstno[0].Contact_Email;
                    txtContactMobile.Text = gstno[0].Contact_Mobile;
                    txtStateCode.Text = gstno[0].StateCode;

                }
                //else
                //{
                //    MessageBox.Show("Invalid Supplier Selected");
                //    CmbBuyerName.Focus();
                //}

                
            }

        }

        private void CmbBuyerName_Enter(object sender, EventArgs e)
        {
            bindCustomer();
        }
        System.Data.DataRow drgetproducts;
        //DataTable dtexisting = new DataTable();
        private void cmdPrevOrder_Click(object sender, EventArgs e)
        {

            try
            {
                //try
                //{
                //    if (CmbBuyerName.Text == "")
                //    {
                //        MessageBox.Show("Select Supplier Name");
                //        CmbBuyerName.Focus();
                //    }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //}
                switch (cmbBasis.Text)
                {
                    

                    case "With PR":
                        DataTable dtexisting = new DataTable();
                        MaterialManagement.Transactions.frnPRDailog form = new MaterialManagement.Transactions.frnPRDailog();                       
                        form.ShowDialog();
                        if (dgProducts.Rows.Count > 1)
                        {
                            dtexisting.Rows.Clear();
                            dtexisting.Columns.Clear();
                            dtexisting.Columns.Add("S_No", typeof(string));
                            //dtexisting.Columns.Add("ProdSno", typeof(string));
                            dtexisting.Columns.Add("Item_Code", typeof(string));
                            dtexisting.Columns.Add("Prod_Code", typeof(string));
                            dtexisting.Columns.Add("Item_Description", typeof(string));
                            dtexisting.Columns.Add("Item_Spec", typeof(string));
                            dtexisting.Columns.Add("Item_Grade", typeof(string));
                            dtexisting.Columns.Add("UOM", typeof(string));
                            dtexisting.Columns.Add("PR_Qty", typeof(decimal));
                            dtexisting.Columns.Add("Qty", typeof(decimal));
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
                            dtexisting.Columns.Add("PR_No", typeof(string));
                            dtexisting.Columns.Add("Remarks", typeof(string));
                            
                            for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                            {
                                DataRow dr;
                                dr = dtexisting.NewRow();
                                dr["S_No"] = dgProducts.Rows[i].Cells["S_No"].Value.ToString();
                                //dr["ProdSno"] = dgProducts.Rows[i].Cells["ProdSno"].Value.ToString();
                                dr["Item_Code"] = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                                dr["Prod_Code"] = dgProducts.Rows[i].Cells["Prod_Code"].Value.ToString();
                                dr["Item_Description"] = dgProducts.Rows[i].Cells["Item_Description"].Value.ToString();
                                dr["Item_Spec"] = dgProducts.Rows[i].Cells["Item_Spec"].Value.ToString();
                                dr["Item_Grade"] = dgProducts.Rows[i].Cells["Item_Grade"].Value.ToString();
                                dr["UOM"] = dgProducts.Rows[i].Cells["UOM"].Value.ToString();
                                dr["PR_Qty"] = dgProducts.Rows[i].Cells["PR_Qty"].Value.ToString();
                                dr["Qty"] = dgProducts.Rows[i].Cells["Qty"].Value.ToString();
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
                                dr["PR_No"] = dgProducts.Rows[i].Cells["PR_No"].Value.ToString();
                                dr["Remarks"] = dgProducts.Rows[i].Cells["Remarks"].Value.ToString();
                               
                                
                                dtexisting.Rows.Add(dr);

                            }
                            dtexisting.AcceptChanges();
                        }


                        if (ioneNet.MaterialManagement.Transactions.frnPRDailog.dtgetproducts.Rows.Count > 0)
                        {
                            DataTable dt = new DataTable();
                            dt.Columns.Add("S_No", typeof(string));
                            //dt.Columns.Add("ProdSno", typeof(string));
                            dt.Columns.Add("Item_Code", typeof(string));
                            dt.Columns.Add("Prod_Code", typeof(string));
                            dt.Columns.Add("Item_Description", typeof(string));
                            dt.Columns.Add("Item_Spec", typeof(string));
                            dt.Columns.Add("Item_Grade", typeof(string));
                            dt.Columns.Add("UOM", typeof(string));
                            dt.Columns.Add("PR_Qty", typeof(decimal));
                            dt.Columns.Add("Qty", typeof(decimal));
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
                            dt.Columns.Add("PR_No", typeof(string));
                            dt.Columns.Add("Remarks", typeof(string));

                            int j = dgProducts.Rows.Count-1;
                            //dt.Rows.Add();
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
                                                  Prod_Code = (logIn.company == 1044 ? s.Prod_Alternative_Code : s.Prod_Code),                                                 
                                                Item_Description = s.Prod_Name,
                                                Item_Spec = "",
                                                  Item_Grade = s.Prod_Field2,
                                                  UOM = u.Uom_Descr,
                                                  PR_Qty = PO_Qty.ToString(),
                                                  Qty = "0",
                                                  Basic_Price="0",
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
                                                  PR_No = PRNo,
                                                  Remarks = ""
                                                  
                                              }).ToList();
                                j = j + 1;
                                dt.Rows.Add(j,getproducts[0].Item_Code, getproducts[0].Prod_Code, getproducts[0].Item_Description, getproducts[0].Item_Spec, getproducts[0].Item_Grade, getproducts[0].UOM,getproducts[0].PR_Qty,  getproducts[0].Qty, getproducts[0].Basic_Price, getproducts[0].Amt_Before_Disc, getproducts[0].Disc_Per, getproducts[0].Disc_Amt, getproducts[0].Taxable_Value, getproducts[0].CGST_Per, getproducts[0].CGST_Amt, getproducts[0].SGST_Per, getproducts[0].SGST_Amt, getproducts[0].IGST_Per, getproducts[0].IGST_Amt, getproducts[0].Total_Amount, getproducts[0].PR_No, getproducts[0].Remarks);




                                //if (comnpstatecode == suppStateCode)
                                //{
                                //    drgetproducts["CGST_Per"] = getHSN[0].Gst_Rate / 2;
                                //    drgetproducts["SGST_Per"] = getHSN[0].Gst_Rate / 2;
                                //}
                                //else
                                //{
                                //    drgetproducts["IGST_Per"] = getHSN[0].Gst_Rate;
                                //}

                                //dtgetfinalprducts = dtgetfinalprducts.AsEnumerable().Union(dtgetproducts.AsEnumerable()).CopyToDataTable();
                                ////}
                                ////}
                                //dtgetproducts.Rows.Clear();
                            }
                        
                        dtexisting = dtexisting.AsEnumerable().Union(dt.AsEnumerable()).CopyToDataTable();
                        //dtexisting = dtexisting.AsEnumerable().Union(dtgetfinalprducts.AsEnumerable()).CopyToDataTable();

                        dgProducts.DataSource = dtexisting;
                        }
                        break;
                }                                
                
               
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        //System.Data.DataRow drgetproducts;
        //DataTable dtexisting = new DataTable();
        private void btnOK_Click(object sender, EventArgs e)
        {
            //DataTable dtgetSelectedprducts = new DataTable();
            //try
            //{
            //    //Check Whether Exisitng Products Already Selected in Main Grid
            //    if (dgProducts.Rows.Count > 1)
            //    {
            //        dtexisting.Rows.Clear();
            //        dtexisting.Columns.Clear();
            //        dtexisting.Columns.Add("Item_Code", typeof(string));
            //        dtexisting.Columns.Add("Item_Description", typeof(string));
            //        dtexisting.Columns.Add("Item_Spec", typeof(string));
            //        dtexisting.Columns.Add("Item_Grade", typeof(string));
            //        dtexisting.Columns.Add("UOM", typeof(string));
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
            //        dtexisting.Columns.Add("PR_No", typeof(string));
            //        dtexisting.Columns.Add("Remarks", typeof(string));
            //        //
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
            //            dr["PR_No"] = dgProducts.Rows[i].Cells["PR_No"].Value.ToString();
            //            dr["Remarks"] = dgProducts.Rows[i].Cells["Remarks"].Value.ToString();
            //            dtexisting.Rows.Add(dr);

            //        }
            //        dtexisting.AcceptChanges();
            //    }



            //    dtgetproducts.Columns.Clear();
            //    dtgetproducts.Rows.Clear();
            //    dtgetproducts.Columns.Add("Item_Code", typeof(string));
            //    dtgetproducts.Columns.Add("Item_Description", typeof(string));
            //    dtgetproducts.Columns.Add("Item_Spec", typeof(string));
            //    dtgetproducts.Columns.Add("Item_Grade", typeof(string));
            //    dtgetproducts.Columns.Add("UOM", typeof(string));
            //    dtgetproducts.Columns.Add("Qty", typeof(decimal));
            //    dtgetproducts.Columns.Add("Basic_Price", typeof(decimal));
            //    dtgetproducts.Columns.Add("Amt_Before_Disc", typeof(decimal));
            //    dtgetproducts.Columns.Add("Disc_Per", typeof(decimal));
            //    dtgetproducts.Columns.Add("Disc_Amt", typeof(decimal));
            //    dtgetproducts.Columns.Add("Taxable_Value", typeof(decimal));
            //    dtgetproducts.Columns.Add("CGST_Per", typeof(decimal));
            //    dtgetproducts.Columns.Add("CGST_Amt", typeof(decimal));
            //    dtgetproducts.Columns.Add("SGST_Per", typeof(decimal));
            //    dtgetproducts.Columns.Add("SGST_Amt", typeof(decimal));
            //    dtgetproducts.Columns.Add("IGST_Per", typeof(decimal));
            //    dtgetproducts.Columns.Add("IGST_Amt", typeof(decimal));
            //    dtgetproducts.Columns.Add("Total_Amount", typeof(decimal));
            //    dtgetproducts.Columns.Add("PR_No", typeof(string));
            //    dtgetproducts.Columns.Add("Remarks", typeof(string));

            //    dtgetfinalprducts.Rows.Clear();
            //    //listBox.Items.Clear();
            //    // Get the selected items of SfDataGrid
            //    //var reflector = this.sfDataGrid1.View.GetPropertyAccessProvider();
            //    //var row = this.sfDataGrid1.SelectedItem;

            //    //string ProdCode;
            //    //string SoNo;
            //    for (int i = 2; i < sfDataGrid1.RowCount; i++)
            //    {
            //        foreach (var item in sfDataGrid1.SelectedItems)
            //        {

            //            //foreach (var col in sfDataGrid1.Columns)
            //            //{
            //            //if (col.MappingName == "Alternative_Code")
            //            //{
            //            //var mappingName = sfDataGrid1.Columns[columnIndex].MappingName;
            //            //var recordIndex = sfDataGrid1.TableControl.ResolveToRecordIndex(rowIndex);
            //            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
            //            var ProdCodeCol = sfDataGrid1.Columns[1].MappingName;
            //            var SONoCol = sfDataGrid1.Columns[0].MappingName;
            //            //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
            //            //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
            //            if (rowData == item)
            //            {
            //                var Item_Code = (rowData.GetType().GetProperty("Prod_Code").GetValue(rowData, null).ToString());
            //                var Item_Description = (rowData.GetType().GetProperty("Product_Description").GetValue(rowData, null).ToString());

            //                var UOM = (rowData.GetType().GetProperty("Uom").GetValue(rowData, null).ToString());
            //                var PO_Qty = (rowData.GetType().GetProperty("BalQty").GetValue(rowData, null).ToString());
            //                // var Basic_Price = (rowData.GetType().GetProperty("Price").GetValue(rowData, null).ToString());
            //                var SO_Ref_No = (rowData.GetType().GetProperty("PR_No").GetValue(rowData, null).ToString());
            //                var getHSN = (from s in db.Products
            //                              join t in db.Tax_Class_Masters on s.Prod_Tax_Class equals t.ID
            //                              where s.Prod_Name == Item_Description.ToString()
            //                              select new { s.Prod_HSN_Code, t.Gst_Rate }).ToList();
            //                var d1 = (from a in db.Company_Infos where a.Id == logIn.company select new { a.GST_No }).ToList();
            //                if (d1.Count > 0)
            //                {
            //                    comnpstatecode = Mid(d1[0].GST_No, 1, 2);
            //                    suppStateCode = txtStateCode.Text;
            //                }

            //                drgetproducts = dtgetproducts.NewRow();
            //                drgetproducts["Item_Code"] = Item_Code.ToString();
            //                drgetproducts["Item_Description"] = Item_Description.ToString();
            //                drgetproducts["Item_Spec"] = "";
            //                drgetproducts["Item_Grade"] = "";
            //                drgetproducts["UOM"] = UOM.ToString();
            //                drgetproducts["Qty"] = PO_Qty.ToString();
            //                drgetproducts["Basic_Price"] = 0;
            //                drgetproducts["Amt_Before_Disc"] = 0;
            //                drgetproducts["Disc_Per"] = 0;
            //                drgetproducts["Disc_Amt"] = 0;
            //                drgetproducts["Taxable_Value"] = 0;
            //                drgetproducts["CGST_Per"] = 0;
            //                drgetproducts["SGST_Per"] = 0;
            //                drgetproducts["IGST_Per"] = 0;

            //                if (comnpstatecode == suppStateCode)
            //                {
            //                    drgetproducts["CGST_Per"] = getHSN[0].Gst_Rate / 2;
            //                    drgetproducts["SGST_Per"] = getHSN[0].Gst_Rate / 2;
            //                }
            //                else
            //                {
            //                    drgetproducts["IGST_Per"] = getHSN[0].Gst_Rate;
            //                }
            //                drgetproducts["CGST_Amt"] = 0;

            //                drgetproducts["SGST_Amt"] = 0;

            //                drgetproducts["IGST_Amt"] = 0;
            //                drgetproducts["Total_Amount"] = 0;
            //                drgetproducts["PR_No"] = SO_Ref_No.ToString();
            //                drgetproducts["Remarks"] = "";
            //                dtgetproducts.Rows.Add(drgetproducts);
            //                dtgetfinalprducts = dtgetfinalprducts.AsEnumerable().Union(dtgetproducts.AsEnumerable()).CopyToDataTable();
            //                //}
            //                //}
            //                dtgetproducts.Rows.Clear();
            //            }
            //        }
            //    }
            //    dtexisting = dtexisting.AsEnumerable().Union(dtgetfinalprducts.AsEnumerable()).CopyToDataTable();
            //    dtgetSelectedprducts = dtgetSelectedprducts.AsEnumerable().Union(dtexisting.AsEnumerable()).CopyToDataTable();

            //    dgProducts.DataSource = dtgetSelectedprducts;
            //    //txtCustPoNo.Text = "Multi";
            //    //txtSoNo.Text = "Multi";

            //    //dgProducts.DataSource = dtgetfinalprducts;
            //    groupBox3.Visible = false;
            //}

            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = false;
        }

        private void dtLDClause_ValueChanged(object sender, EventArgs e)
        {

        }

        private void txtPackingCharges_Leave(object sender, EventArgs e)
        {
            GetTot();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {

        }

        

        private void txtOtherCharges_Leave(object sender, EventArgs e)
        {
            GetTot();
        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void cmbCDuty_Leave(object sender, EventArgs e)
        {
            GetTot();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            GetTot();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtRounding_Leave(object sender, EventArgs e)
        {
            GetTot();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void txtSoNo_Leave(object sender, EventArgs e)
        {
            var da = (from obj in db.Purchase_Order_Masters
                      where obj.PO_NO == txtSoNo.Text && obj.Company_ID == logIn.company
                      select obj).ToList();

            if (da.Count > 0)
            {
                MessageBox.Show("Record already exist with PO Number" + txtSoNo.Text+".");
                txtSoNo.Focus();
            }

        }

        private void cmbPriceBasis_Leave(object sender, EventArgs e)
        {
            int i = (cmbPriceBasis.FindString(cmbPriceBasis.Text));
            if (i < 0)
            {
                MessageBox.Show("Invalid Selection");
                cmbPriceBasis.Focus();

            }
        }

        private void cmbInsurance_Leave(object sender, EventArgs e)
        {
            int i = (cmbInsurance.FindString(cmbInsurance.Text));
            if (i < 0)
            {
                MessageBox.Show("Invalid Selection");
                cmbInsurance.Focus();

            }
        }

        private void cmbPaymentTerms_Leave(object sender, EventArgs e)
        {
            int i = (cmbPaymentTerms.FindString(cmbPaymentTerms.Text));
            if (i < 0)
            {
                MessageBox.Show("Invalid Selection");
                cmbPaymentTerms.Focus();

            }
        }

        private void cmbModeofDesp_Leave(object sender, EventArgs e)
        {
            int i = (cmbModeofDesp.FindString(cmbModeofDesp.Text));
            if (i < 0)
            {
                MessageBox.Show("Invalid Selection");
                cmbModeofDesp.Focus();

            }
        }

        private void cmbTransport_Scope_Leave(object sender, EventArgs e)
        {
            int i = (cmbTransport_Scope.FindString(cmbTransport_Scope.Text));
            if (i < 0)
            {
                MessageBox.Show("Invalid Selection");
                cmbTransport_Scope.Focus();

            }
        }

        private void cmbShipTo_Leave(object sender, EventArgs e)
        {
            int i = (cmbShipTo.FindString(cmbShipTo.Text));
            if (i < 0)
            {
                MessageBox.Show("Invalid Selection");
                cmbShipTo.Focus();

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

            JObject jsoncancel = JObject.Parse(txtShippingAddress.Text);
            txtShipTo.Text = (string)jsoncancel.SelectToken("ShipTo"); ;
            txtAddress2.Text = (string)jsoncancel.SelectToken("Address");
            cmbCity.Text = (string)jsoncancel.SelectToken("City");
            txtstate.Text = (string)jsoncancel.SelectToken("State");
            txtPincode.Text = (string)jsoncancel.SelectToken("PinCode");
            txtStateCode.Text = (string)jsoncancel.SelectToken("StateCode");
            txtConGSTIN.Text = (string)jsoncancel.SelectToken("GSTIN");
            //txtConGSTNo.Text = State[0].GSTIN_NO;

            groupBox4.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            genConsigneeAddress CAddr = new genConsigneeAddress();
            CAddr.ShipTo = txtShipTo.Text;

            CAddr.Address = txtAddress2.Text; ;
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

            txtShippingAddress.Text = json;
            groupBox4.Visible = false;
        }
        public class genConsigneeAddress
        {
            public string ShipTo { get; set; }
            public string Address { get; set; }
            public string City { get; set; }
            public int PinCode { get; set; }
            public string State { get; set; }
            public string StateCode { get; set; }
            public string GSTIN { get; set; }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            groupBox4.Visible = false;
        }

        private void PurchaseOrder_FormClosed(object sender, FormClosedEventArgs e)
        {
            //var ci = db.Purchase_Order_Masters.Where(w => w.PO_NO == txtSoNo.Text && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).ToList();
            //if (ci.Count > 0)
            //{

            //    if (dgProducts.Rows[0].Cells["Item_Code"].Value == null)
            //    {
            //        MessageBox.Show("Atleast One Record To Be Entered To Proceed", "Purchase Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        return;
            //    }
            //}
            //else
            //{
            //    ////Masters.ProdSearch.dtgetfinalprducts = null;
            //    this.Close();
            //}
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (PurchaseOrdersList.editMode == true)
            {

            }
            else
            {
                AutoincrementId();
            }
        }

        private void dgProducts_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void cmbBasis_Enter(object sender, EventArgs e)
        {
            PO_Basis = cmbBasis.Text;
        }

        private void cmbBasis_Leave(object sender, EventArgs e)
        {
            try
            {
                //if (cmbBasis.Text != PO_Basis && cmbBasis.Text=="")
                //{
                //    DialogResult result = MessageBox.Show("Are You Sure To Delete The Existing "+ PO_Basis + " Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                //    if (result == DialogResult.Yes)
                //    {
                //        //dgProducts.Rows.Clear();
                //        //int count = 
                //        for (int i = 0; i < dgProducts.Rows.Count; i++)
                //        {
                //            i = 0;
                //            dgProducts.Rows.RemoveAt(i);
                //        }

                //    }
                //    else
                //    {
                //        cmbBasis.Text = PO_Basis;
                //    }
                //}
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        public void clear()
        {
            foreach (Control c in tabControl1.Controls.Cast<Control>().OrderBy(c => c.TabIndex))
            {
                if (c is TextBox)
                {
                    c.Text = "";
                }
                else
                if (c is ComboBox)
                {
                    c.Text = "";
                }
             

            }
            foreach (Control c in tableLayoutPanel1.Controls.Cast<Control>().OrderBy(c => c.TabIndex))
            {
                if (c is TextBox)
                {
                    c.Text = "";
                }
                else
                if (c is ComboBox)
                {
                    c.Text = "";
                }
                

            }
            dtAmendDate.Value = DateTime.Now;
            dpSODate.Value= DateTime.Now;
            dtLDClause.Value= DateTime.Now;
            dtDeliveryDate.Value = DateTime.Now;
            dpPODate.Value= DateTime.Now;
            chkImportPO.Checked = false;
            chkMultiLocation.Checked = false;
            chkLDClause.Checked = false;
            dtgetfinalprducts.Rows.Clear();
            dtgetfinalprducts.Columns.Clear();
            dgProducts.DataSource = dtgetfinalprducts;
            CmbConsigneeName.Text = "";
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

            cmbStatus.Text = "";
        }

        private void btnTransLog_Click(object sender, EventArgs e)
        {
            transname = "Purchase Order";
            transno = txtSoNo.Text;
            Transactions.frmTransLog form = new Transactions.frmTransLog();
            form.ShowDialog();
        }

        private void txtCustomeContact_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnAddNewSupplier_Click(object sender, EventArgs e)
        {
            MaterialManagement.SupplierMaster form = new MaterialManagement.SupplierMaster();
            var = "1";
            form.ShowDialog();
        }

        public void Save()
        {
            try
            {
                String myString = "";

                myString = txtSoNo.Text;
                string AmendNo = txtAmendNo.Text;
                if (txtSoNo.Text!="")
                {

                    if (PurchaseOrdersList.var == "1")
                    {
                        var result = db.Sp_autoincrement_PO_AmendNo(logIn.company, logIn.BU_ID, PurchaseOrdersList.SO_No);
                        txtAmendNo.Text = result.FirstOrDefault().PO_Amend_no;

                        //Set Previous Version Quote Status as Amended
                        var ci = db.Purchase_Order_Masters.Where(w => w.PO_NO == myString && w.Po_Amend_No == AmendNo && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                        {

                            ci.is_amended = true;
                            ci.Modified_By = logIn.username + "-" + DateTime.Now;
                            db.SubmitChanges();

                        }                        

                    }
                    else
                    {
                        myString = txtSoNo.Text;
                        db.sp_PO_Delete(myString, logIn.company, logIn.BU_ID);
                    }

                }
                else
                {
                    //AutoincrementId();
                    myString = txtSoNo.Text;

                }
                    //if (frmGate.Modify.Contains(this.Text))
                    //{

                //dgProducts.Enabled = false;
                //var S = db.Sale_Order_Masters.Where(w => w.SO_NO == myString && w.Company_ID == logIn.company).FirstOrDefault();
                Purchase_Order_Master S = new Purchase_Order_Master();
                {
                    S.PO_NO = myString;
                    S.PO_Date = dpSODate.Value;
                    S.Purchase_Bases = cmbBasis.Text;
                    S.Contact_Person = (txtcontactperson.Text == null || txtcontactperson.Text == "") ? "" : txtcontactperson.Text ;
                    S.Contact_EMail = (txtcontactEmail.Text==null||txtcontactEmail.Text=="")?"":txtcontactEmail.Text;
                    S.SupplierName = Convert.ToInt32(CmbBuyerName.SelectedValue.ToString());
                    //S. = txtcontactEmail.Text;
                    
                    S.Freight = (txtFrieght.Text==null||txtFrieght.Text=="")?0:Convert.ToDecimal(txtFrieght.Text);
                    S.Other_Charges = (txtOtherCharges.Text == null || txtOtherCharges.Text == "") ? 0 : Convert.ToDecimal(txtOtherCharges.Text);
                    S.Packing_Charges = (txtPackingCharges.Text == null || txtPackingCharges.Text == "") ? 0 : Convert.ToDecimal(txtPackingCharges.Text);

                    S.ConsigneeName = Convert.ToInt32(1);
                    S.Status = Convert.ToInt32(cmbStatus.SelectedValue.ToString()); 
                    S.TotalQty = (txtTotalQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotalQty.Text);
                    S.SubTotal = (txtSubTotal.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtSubTotal.Text);
                    S.Tot_Discount = (txtTotDiscount.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotDiscount.Text);
                    S.Tot_TaxableValue = (txtTot_TaxableValue.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTot_TaxableValue.Text);
                    S.Supplier_QutoNo = (txtCustPoNo.Text == "") ? "" : txtCustPoNo.Text;                            
                    S.Supplier_QutoDate = dpPODate.Value;
                    S.Cust_GST_No = (txtCustGSTNo.Text == "") ? "" : txtCustGSTNo.Text;
                    S.Tot_CGST_Amnt = (txtTot_CGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_CGST.Text);
                    S.Tot_SGST_Amnt = (txtTot_SGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_SGST.Text);
                    S.Tot_IGST_Amnt = (txtTot_IGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_IGST.Text);
                    S.Tot_Ord_Value = (txtTot_OrderValue.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_OrderValue.Text);
                    S.Delivery_Date = dtDeliveryDate.Value;
                    S.Qty_Tolerence = (txtqtytolerence.Text == "") ? "" : txtqtytolerence.Text;
                    S.Po_Amend_No = (txtAmendNo.Text == "") ? "" : txtAmendNo.Text;
                    S.Po_Amend_Date = dtAmendDate.Value;
                    if (cmbShipTo.SelectedValue == null)
                    {
                        S.Ship_To = 0;
                    }
                    else
                    {
                    S.Ship_To  = Convert.ToInt32(cmbShipTo.SelectedValue.ToString());
                    }
                    //S.SaleExecutive = Convert.ToInt32(cmbSaleExecutive.SelectedValue.ToString());
                    S.Price_Basis = Convert.ToInt32(cmbPriceBasis.SelectedValue.ToString());
                    S.Insurance_Scope = Convert.ToInt32(cmbInsurance.SelectedValue.ToString());
                    S.PaymentTerms = Convert.ToInt32(cmbPaymentTerms.SelectedValue.ToString());
                    S.Trasnport_Scope = Convert.ToInt32(cmbTransport_Scope.SelectedValue.ToString());
                    S.Transporter_Name = (cmbTransporter.Text == "") ? "" : cmbTransporter.Text;
                    S.Multi_Loc_Delivery = (chkMultiLocation.Checked == true) ? true : false;  
                    S.Desp_Mode = Convert.ToInt32(cmbModeofDesp.SelectedValue.ToString());
                    //S.Repeat_Order = (chkRepeatOrder.Checked == true) ? true : false;
                    //S.Old_Ord_Ref = (txt.Text == "") ? "" : txtOtherTerms.Text;
                    //S.SEZ_Order = (chkSEZOrder.Checked == true) ? true : false;
                    S.LD_Date = dtLDClause.Value;
                    S.LD_Description = (txtLDClause.Text == "") ? "" : txtLDClause.Text;
                    S.LD_Clause_applicable = chkLDClause.Checked;
                    S.Import_PO = chkImportPO.Checked;
                    S.Customer_Contact = (txtCustomeContact.Text == "") ? "" : txtCustomeContact.Text;
                    S.Other_Terms =   (txtOtherTerms.Text == "") ? "" : txtOtherTerms.Text;
                    S.Spl_Instructions = (txtSplInstructions.Text == "") ? "" : txtSplInstructions.Text;
                    S.Warrenty = (txtWarrenty.Text == "") ? "" : txtWarrenty.Text;
                    S.FCurrency = (cmbFCurrency.Text == "") ? "" : cmbFCurrency.Text;
                    S.ExchangeRate = (txtExchangeRate.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtExchangeRate.Text);
                    S.is_amended = false;
                    S.BU_ID = logIn.BU_ID;
                    S.Company_ID = logIn.company;
                    S.Created_By = lblCreatedBy.Text;                    
                    S.Modified_By = logIn.username + "-" + DateTime.Now;
                    db.Purchase_Order_Masters.InsertOnSubmit(S);
                    db.SubmitChanges();
                }
                        //db.Transaction = transaction;
                for (int i = 0; i < dgProducts.RowCount - 1; i++)
                {
                    Purchase_Order_Child SC = new Purchase_Order_Child();
                    var d1 = (from a in db.Purchase_Order_Masters where a.PO_NO == myString && a.Po_Amend_No == txtAmendNo.Text && a.Company_ID == logIn.company select new { a.Id }).ToList();
                    SC.PO_Master_ID = d1[0].Id;

                    SC.PO_NO = myString;
                    //SC.Prod_Code = (dgProducts.Rows[i].Cells["Item_Code"].Value == null) ? "" : Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value).ToString());
                    SC.Prod_Code = (dgProducts.Rows[i].Cells["Item_Code"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value);

                    SC.Product_Description = (dgProducts.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value).ToString();
                    SC.Prod_Spec = (dgProducts.Rows[i].Cells["Item_spec"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_spec"].Value).ToString();
                    SC.Prod_Grade = (dgProducts.Rows[i].Cells["Item_Grade"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value).ToString();
                    SC.Uom = (dgProducts.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["uom"].Value.ToString();
                    SC.PR_Qty = (dgProducts.Rows[i].Cells["PR_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["PR_Qty"].Value);
                    SC.Qty = (dgProducts.Rows[i].Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty"].Value);
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
                    SC.PR_NO = (dgProducts.Rows[i].Cells["PR_NO"].Value == null) ? "" : (dgProducts.Rows[i].Cells["PR_NO"].Value).ToString();
                    SC.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value).ToString();
                    SC.ProdSno = i + 1;
                    SC.Company_ID = logIn.company;
                    db.Purchase_Order_Childs.InsertOnSubmit(SC);
                }
                db.SubmitChanges();
                //transaction.Commit();               
                MessageBox.Show("Record Saved / Updated Successfully With Transaction Ref No : " + txtSoNo.Text);                   

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }
        public void SaveNew_Sql_proc()
        {
            try
            {
                String myString = "";

                myString = txtSoNo.Text;
                string AmendNo = txtAmendNo.Text;
                if (txtSoNo.Text != "")
                {

                    if (PurchaseOrdersList.var == "1")
                    {
                        var result = db.Sp_autoincrement_PO_AmendNo(logIn.company, logIn.BU_ID, PurchaseOrdersList.SO_No);
                        txtAmendNo.Text = result.FirstOrDefault().PO_Amend_no;

                        //Set Previous Version Quote Status as Amended
                        var ci = db.Purchase_Order_Masters.Where(w => w.PO_NO == myString && w.Po_Amend_No == AmendNo && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                        {

                            ci.is_amended = true;
                            ci.Modified_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                            db.SubmitChanges();

                        }

                    }
                    else
                    {
                        if (PurchaseOrdersList.var == "0")
                        {
                        }        
                        else
                        {
                            AutoincrementId();
                            PurchaseOrdersList.var = "0";
                        }
                    }    
                   
                    myString = txtSoNo.Text;
                    SqlCommand cmd = new SqlCommand("SavePurchaseOrder", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PO_No", myString);
                    cmd.Parameters.AddWithValue("@PO_Date", dpSODate.Value);
                    cmd.Parameters.AddWithValue("@Po_Amend_No", (txtAmendNo.Text == "") ? "" : txtAmendNo.Text);
                    cmd.Parameters.AddWithValue("@Po_Amend_Date", dtAmendDate.Value);
                    if (cmbasset.Text != "")
                    {
                        cmd.Parameters.AddWithValue("@Project_Code", Convert.ToInt32(cmbasset.SelectedValue));
                    }
                    else
                    {
                        cmd.Parameters.AddWithValue("@Project_Code",0);
                    }
                    //cmd.Parameters.AddWithValue("@Old_Ord_Ref", logIn.BU_ID);
                    cmd.Parameters.AddWithValue("@Purchase_Bases", cmbBasis.Text);
                    cmd.Parameters.AddWithValue("@Contact_Person", (txtcontactperson.Text == null || txtcontactperson.Text == "") ? "" : txtcontactperson.Text);
                    //cmd.Parameters.AddWithValue("@SEZ_Order", logIn.BU_ID);
                    cmd.Parameters.AddWithValue("@SupplierName", Convert.ToInt32(CmbBuyerName.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@Cust_GST_No", (txtCustGSTNo.Text == "") ? "" : txtCustGSTNo.Text);
                    //  cmd.Parameters.AddWithValue("@ConsigneeName", Convert.ToInt32(1));
                    // cmd.Parameters.AddWithValue("@Multi_Loc_Delivery", logIn.BU_ID);
                    cmd.Parameters.AddWithValue("@Supplier_QutoNo", (txtCustPoNo.Text == "") ? "" : txtCustPoNo.Text);
                    cmd.Parameters.AddWithValue("@Supplier_QutoDate", dpPODate.Value);
                    cmd.Parameters.AddWithValue("@Delivery_Date", dtDeliveryDate.Value);
                    cmd.Parameters.AddWithValue("@Ship_To", Convert.ToInt32(cmbShipTo.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@TotalQty", (txtTotalQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotalQty.Text));
                    cmd.Parameters.AddWithValue("@SubTotal", (txtSubTotal.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtSubTotal.Text));
                    cmd.Parameters.AddWithValue("@Tot_Discount", (txtTotDiscount.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotDiscount.Text));
                    cmd.Parameters.AddWithValue("@Tot_TaxableValue", (txtTot_TaxableValue.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTot_TaxableValue.Text));
                    cmd.Parameters.AddWithValue("@Tot_CGST_Amnt", (txtTot_CGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_CGST.Text));
                    cmd.Parameters.AddWithValue("@Tot_SGST_Amnt", (txtTot_SGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_SGST.Text));
                    cmd.Parameters.AddWithValue("@Tot_IGST_Amnt", (txtTot_IGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_IGST.Text));
                    cmd.Parameters.AddWithValue("@Tot_Ord_Value", (txtTot_OrderValue.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_OrderValue.Text));
                    cmd.Parameters.AddWithValue("@Price_Basis", Convert.ToInt32(cmbPriceBasis.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@Insurance_Scope", Convert.ToInt32(cmbInsurance.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@PaymentTerms", Convert.ToInt32(cmbPaymentTerms.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@Trasnport_Scope", Convert.ToInt32(cmbTransport_Scope.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@Transporter_Name", (cmbTransporter.Text == "") ? "" : cmbTransporter.Text);
                    cmd.Parameters.AddWithValue("@Customer_Contact", (txtCustomeContact.Text == null || txtCustomeContact.Text == "") ? "" : txtCustomeContact.Text);
                    cmd.Parameters.AddWithValue("@Other_Terms", (txtOtherTerms.Text == "") ? "" : txtOtherTerms.Text);
                    cmd.Parameters.AddWithValue("@Spl_Instructions", (txtSplInstructions.Text == "") ? "" : txtSplInstructions.Text);
                    cmd.Parameters.AddWithValue("@Status", Convert.ToInt32(cmbStatus.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@BU_ID", logIn.BU_ID);
                    cmd.Parameters.AddWithValue("@Company_ID", logIn.company);
                    cmd.Parameters.AddWithValue("@Created_By", lblCreatedBy.Text);             
                    cmd.Parameters.AddWithValue("@Modified_By", logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt"));   
                    
                    //if (cmbStatus.Text == "Approved")
                    //{
                    //    cmd.Parameters.AddWithValue("@Approved_By", logIn.username + "-" + DateTime.Now);
                    //    cmd.Parameters.AddWithValue("@Reviewed_By", logIn.username + "-" + DateTime.Now);

                    //}
                    //else
                    //if (cmbStatus.Text == "Reviewed")
                    //{
                    //    cmd.Parameters.AddWithValue("@Reviewed_By", logIn.username + "-" + DateTime.Now);
                    //    cmd.Parameters.AddWithValue("@Approved_By", "");
                    //} 
                    cmd.Parameters.AddWithValue("@Contact_EMail", (txtcontactEmail.Text == null || txtcontactEmail.Text == "") ? "" : txtcontactEmail.Text);
                    cmd.Parameters.AddWithValue("@Freight", (txtFrieght.Text == null || txtFrieght.Text == "") ? 0 : Convert.ToDecimal(txtFrieght.Text));
                    cmd.Parameters.AddWithValue("@Other_Charges", (txtOtherCharges.Text == null || txtOtherCharges.Text == "") ? 0 : Convert.ToDecimal(txtOtherCharges.Text));
                    cmd.Parameters.AddWithValue("@Qty_Tolerence", (txtqtytolerence.Text == "") ? "" : txtqtytolerence.Text);
                    
                    cmd.Parameters.AddWithValue("@Desp_Mode", Convert.ToInt32(cmbModeofDesp.SelectedValue.ToString()));
                    
                    cmd.Parameters.AddWithValue("@Warrenty", (txtWarrenty.Text == "") ? "" : txtWarrenty.Text);
                    cmd.Parameters.AddWithValue("@FCurrency", (cmbFCurrency.Text == "") ? "" : cmbFCurrency.Text);
                    cmd.Parameters.AddWithValue("@ExchangeRate", (txtExchangeRate.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtExchangeRate.Text));
                    cmd.Parameters.AddWithValue("@Import_PO", chkImportPO.Checked);
                    cmd.Parameters.AddWithValue("@LD_Clause_applicable", chkLDClause.Checked);
                    cmd.Parameters.AddWithValue("@LD_Date", dtLDClause.Value);
                    cmd.Parameters.AddWithValue("@LD_Description", (txtLDClause.Text == "") ? "" : txtLDClause.Text);
                    cmd.Parameters.AddWithValue("@email_Sent", false);
                    cmd.Parameters.AddWithValue("@Packing_Charges", (txtPackingCharges.Text == null || txtPackingCharges.Text == "") ? 0 : Convert.ToDecimal(txtPackingCharges.Text));
                    cmd.Parameters.AddWithValue("@is_amended", false);
                    cmd.Parameters.AddWithValue("@Custom_Payment_Terms", (txtCustomPaymentTerms.Text == "") ? "" : txtCustomPaymentTerms.Text);

                    cmd.Parameters.AddWithValue("@Custom_Duty_Per", (cmbCDuty.Text == null || cmbCDuty.Text == "") ? 0 : Convert.ToDecimal(cmbCDuty.Text));
                    cmd.Parameters.AddWithValue("@Custom_Duty_Amt", (txtCdutyAmt.Text == null || txtCdutyAmt.Text == "") ? 0 : Convert.ToDecimal(txtCdutyAmt.Text));
                    cmd.Parameters.AddWithValue("@Cess_CD_Per", (txtCessPer.Text == null || txtCessPer.Text == "") ? 0 : Convert.ToDecimal(txtCessPer.Text));
                    cmd.Parameters.AddWithValue("@Cess_CD_Amt", (txtCessAmt.Text == null || txtCessAmt.Text == "") ? 0 : Convert.ToDecimal(txtCessAmt.Text));
                    cmd.Parameters.AddWithValue("@Container_Detention", (txtContainerDetention.Text == "") ? "" : txtContainerDetention.Text);
                    cmd.Parameters.AddWithValue("@Custom_Duty_Remarks", (txtCustomDutyRemarks.Text == "") ? "" : txtCustomDutyRemarks.Text);
                    cmd.Parameters.AddWithValue("@Test_Certificates", (txtTestCertificates.Text == "") ? "" : txtTestCertificates.Text);
                    cmd.Parameters.AddWithValue("@Identifcation_Text", (txtIdentification.Text == "") ? "" : txtIdentification.Text);
                    cmd.Parameters.AddWithValue("@Despatch_Documents", (txtDespatchDocuments.Text == "") ? "" : txtDespatchDocuments.Text);
                    cmd.Parameters.AddWithValue("@Rounding_Amt", (txtRounding.Text == null || txtRounding.Text == "") ? 0 : Convert.ToDecimal(txtRounding.Text));
                    cmd.Parameters.AddWithValue("@GST_Per_On_Frieght", (txtGSTonFrieght.Text == null || txtGSTonFrieght.Text == "") ? 0 : Convert.ToDecimal(txtGSTonFrieght.Text));
                    cmd.Parameters.AddWithValue("@shippingaddress", (txtShippingAddress.Text == "") ? "" : txtShippingAddress.Text);
                    cmd.Parameters.AddWithValue("@Raw_Material_PO",checkBox1.Checked);

                    string Prod_Code = "";
                    string Product_Description = "";
                    string Prod_Spec = "";
                    string Prod_Grade = "";
                    string Uom = "";
                    string PR_Qty = "";
                    string Qty = "";
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
                    string PR_NO = "";
                    string Remarks = "";
                    string ProdSno = "";
                    int rowcount = 0;
                    int PSno = 0;
                    decimal PrQty = 0;
                    for (int i = 0; i < dgProducts.RowCount - 1; i++)
                    {

                        Prod_Code = Prod_Code+ Convert.ToString(dgProducts.Rows[i].Cells["Item_Code"].Value).PadRight(14);
                        //Product_Description = Product_Description + Convert.ToString(dgProducts.Rows[i].Cells["Item_Description"].Value).PadRight(50); 
                        Prod_Spec = Prod_Spec+ Convert.ToString(dgProducts.Rows[i].Cells["Item_spec"].Value).PadRight(50);
                        Prod_Grade = Prod_Grade+ Convert.ToString(dgProducts.Rows[i].Cells["Item_Grade"].Value).PadRight(50);
                        Uom = Uom+Convert.ToString(dgProducts.Rows[i].Cells["uom"].Value).PadRight(14);
                        if (dgProducts.Rows[i].Cells["PR_Qty"].Value == DBNull.Value)
                        {
                            PR_Qty = PR_Qty + "0";
                        }
                        else
                        {
                            PR_Qty = PR_Qty + Convert.ToString(dgProducts.Rows[i].Cells["PR_Qty"].Value).PadRight(14);
                        }
                        Qty = Qty+Convert.ToString(dgProducts.Rows[i].Cells["Qty"].Value).PadRight(14);
                        Price = Price+ Convert.ToString(dgProducts.Rows[i].Cells["Basic_Price"].Value).PadRight(14);
                        Amount = Amount+ Convert.ToString(dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value).PadRight(14);
                        Disc_Per = Disc_Per+  Convert.ToString(dgProducts.Rows[i].Cells["Disc_Per"].Value).PadRight(14);
                        Disc_Amount = Disc_Amount+ Convert.ToString(dgProducts.Rows[i].Cells["Disc_Amt"].Value).PadRight(14);
                        Taxable_Value = Taxable_Value+Convert.ToString(dgProducts.Rows[i].Cells["Taxable_Value"].Value).PadRight(14);
                        CGST_Per = CGST_Per+ Convert.ToString(dgProducts.Rows[i].Cells["CGST_Per"].Value).PadRight(14);
                        SGST_Per = SGST_Per+ Convert.ToString(dgProducts.Rows[i].Cells["SGST_Per"].Value).PadRight(14);
                        IGST_Per = IGST_Per + Convert.ToString(dgProducts.Rows[i].Cells["IGST_Per"].Value).PadRight(14);
                        CGST_Amnt = CGST_Amnt + Convert.ToString(dgProducts.Rows[i].Cells["CGST_Amt"].Value).PadRight(14);
                        SGST_Amnt = SGST_Amnt + Convert.ToString(dgProducts.Rows[i].Cells["SGST_Amt"].Value).PadRight(14);
                        IGST_Amnt = IGST_Amnt + Convert.ToString(dgProducts.Rows[i].Cells["IGST_Amt"].Value).PadRight(14);
                        Net_Amount = Net_Amount + Convert.ToString(dgProducts.Rows[i].Cells["Total_Amount"].Value).PadRight(14);
                        PR_NO = PR_NO + Convert.ToString(dgProducts.Rows[i].Cells["PR_NO"].Value).PadRight(20);
                        Remarks = Remarks + Convert.ToString(dgProducts.Rows[i].Cells["Remarks"].Value).PadRight(50);
                        //PSno = PSno +Convert.ToInt32( dgProducts.Rows[i].Cells["S_No"].Value);
                        if (dgProducts.Rows[i].Cells["S_no"].Value == null || dgProducts.Rows[i].Cells["S_no"].Value.ToString() == "")
                        {


                            //var d1 = (from a in db.Purchase_Order_Childs where a.PO_NO == myString && a.Company_ID == logIn.company select a.ProdSno).ToList().Max(a => a.ProdSno);
                            //  string sno = d1[0].prod;

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


                        //{


                        //    //var d1 = (from a in db.Purchase_Order_Childs where a.PO_NO == myString && a.Company_ID == logIn.company select a.ProdSno).ToList().Max(a => a.ProdSno);
                        //    //  string sno = d1[0].prod;

                        //    if (PSno == 0)
                        //    {
                        //        ProdSno = ProdSno + Convert.ToString(i + 1).PadRight(14);
                        //        PSno = i + 1;
                        //    }
                        //    else
                        //    {
                        //        ProdSno = ProdSno + Convert.ToString(PSno+1).PadRight(14);
                        //        PSno = PSno + 1;
                        //    }
                        //}
                        //else
                        //{
                        //    ProdSno = ProdSno + Convert.ToString(dgProducts.Rows[i].Cells["ProdSno"].Value).PadRight(14);
                        //    PSno = Convert.ToInt32(dgProducts.Rows[i].Cells["ProdSno"].Value);
                        //}    

                        //Company_ID = logIn.company;
                        rowcount += 1;
                    }

                    cmd.Parameters.AddWithValue("@txt_Prod_Code", Prod_Code);
                    cmd.Parameters.AddWithValue("@txt_Product_Description", "");
                    cmd.Parameters.AddWithValue("@txt_Prod_Spec", Prod_Spec);
                    cmd.Parameters.AddWithValue("@txt_Prod_Grade", Prod_Grade);
                    cmd.Parameters.AddWithValue("@txt_Uom", Uom);
                    cmd.Parameters.AddWithValue("@txt_PR_Qty", PR_Qty);
                    cmd.Parameters.AddWithValue("@txt_Qty", Qty);
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
                    cmd.Parameters.AddWithValue("@txt_PR_NO", PR_NO);
                    cmd.Parameters.AddWithValue("@txt_Prod_SNO", ProdSno);
                    cmd.Parameters.AddWithValue("@gridcount", rowcount);

                    try
                    {
                        con.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (Convert.ToBoolean(result))
                        {
                            MessageBox.Show("Record has been successfully saved..");
                            
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
                //if (frmGate.Modify.Contains(this.Text))
                //{

                //dgProducts.Enabled = false;
                //var S = db.Sale_Order_Masters.Where(w => w.SO_NO == myString && w.Company_ID == logIn.company).FirstOrDefault();
                //Purchase_Order_Master S = new Purchase_Order_Master();
                //{
                //    S.PO_NO = myString;
                //    S.PO_Date = dpSODate.Value;
                //    S.Purchase_Bases = cmbBasis.Text;
                //    S.Contact_Person = (txtcontactperson.Text == null || txtcontactperson.Text == "") ? "" : txtcontactperson.Text;
                //    S.Contact_EMail = (txtcontactEmail.Text == null || txtcontactEmail.Text == "") ? "" : txtcontactEmail.Text;
                //    S.SupplierName = Convert.ToInt32(CmbBuyerName.SelectedValue.ToString());
                //    //S. = txtcontactEmail.Text;

                //    S.Freight = (txtFrieght.Text == null || txtFrieght.Text == "") ? 0 : Convert.ToDecimal(txtFrieght.Text);
                //    S.Other_Charges = (txtOtherCharges.Text == null || txtOtherCharges.Text == "") ? 0 : Convert.ToDecimal(txtOtherCharges.Text);
                //    S.Packing_Charges = (txtPackingCharges.Text == null || txtPackingCharges.Text == "") ? 0 : Convert.ToDecimal(txtPackingCharges.Text);

                //    S.ConsigneeName = Convert.ToInt32(1);
                //    S.Status = Convert.ToInt32(cmbStatus.SelectedValue.ToString());
                //    S.TotalQty = (txtTotalQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotalQty.Text);
                //    S.SubTotal = (txtSubTotal.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtSubTotal.Text);
                //    S.Tot_Discount = (txtTotDiscount.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotDiscount.Text);
                //    S.Tot_TaxableValue = (txtTot_TaxableValue.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTot_TaxableValue.Text);
                //    S.Supplier_QutoNo = (txtCustPoNo.Text == "") ? "" : txtCustPoNo.Text;
                //    S.Supplier_QutoDate = dpPODate.Value;
                //    S.Cust_GST_No = (txtCustGSTNo.Text == "") ? "" : txtCustGSTNo.Text;
                //    S.Tot_CGST_Amnt = (txtTot_CGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_CGST.Text);
                //    S.Tot_SGST_Amnt = (txtTot_SGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_SGST.Text);
                //    S.Tot_IGST_Amnt = (txtTot_IGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_IGST.Text);
                //    S.Tot_Ord_Value = (txtTot_OrderValue.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_OrderValue.Text);
                //    S.Delivery_Date = dtDeliveryDate.Value;
                //    S.Qty_Tolerence = (txtqtytolerence.Text == "") ? "" : txtqtytolerence.Text;
                //    S.Po_Amend_No = (txtAmendNo.Text == "") ? "" : txtAmendNo.Text;
                //    S.Po_Amend_Date = dtAmendDate.Value;
                //    if (cmbShipTo.SelectedValue == null)
                //    {
                //        S.Ship_To = 0;
                //    }
                //    else
                //    {
                //        S.Ship_To = Convert.ToInt32(cmbShipTo.SelectedValue.ToString());
                //    }
                //    //S.SaleExecutive = Convert.ToInt32(cmbSaleExecutive.SelectedValue.ToString());
                //    S.Price_Basis = Convert.ToInt32(cmbPriceBasis.SelectedValue.ToString());
                //    S.Insurance_Scope = Convert.ToInt32(cmbInsurance.SelectedValue.ToString());
                //    S.PaymentTerms = Convert.ToInt32(cmbPaymentTerms.SelectedValue.ToString());
                //    S.Trasnport_Scope = Convert.ToInt32(cmbTransport_Scope.SelectedValue.ToString());
                //    S.Transporter_Name = (cmbTransporter.Text == "") ? "" : cmbTransporter.Text;
                //    S.Multi_Loc_Delivery = (chkMultiLocation.Checked == true) ? true : false;
                //    S.Desp_Mode = Convert.ToInt32(cmbModeofDesp.SelectedValue.ToString());
                //    //S.Repeat_Order = (chkRepeatOrder.Checked == true) ? true : false;
                //    //S.Old_Ord_Ref = (txt.Text == "") ? "" : txtOtherTerms.Text;
                //    //S.SEZ_Order = (chkSEZOrder.Checked == true) ? true : false;
                //    S.LD_Date = dtLDClause.Value;
                //    S.LD_Description = (txtLDClause.Text == "") ? "" : txtLDClause.Text;
                //    S.LD_Clause_applicable = chkLDClause.Checked;
                //    S.Import_PO = chkImportPO.Checked;
                //    S.Customer_Contact = (txtCustomeContact.Text == "") ? "" : txtCustomeContact.Text;
                //    S.Other_Terms = (txtOtherTerms.Text == "") ? "" : txtOtherTerms.Text;
                //    S.Spl_Instructions = (txtSplInstructions.Text == "") ? "" : txtSplInstructions.Text;
                //    S.Warrenty = (txtWarrenty.Text == "") ? "" : txtWarrenty.Text;
                //    S.FCurrency = (cmbFCurrency.Text == "") ? "" : cmbFCurrency.Text;
                //    S.ExchangeRate = (txtExchangeRate.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtExchangeRate.Text);
                //    S.is_amended = false;
                //    S.BU_ID = logIn.BU_ID;
                //    S.Company_ID = logIn.company;
                //    S.Created_By = lblCreatedBy.Text;
                //    S.Modified_By = logIn.username + "-" + DateTime.Now;
                //    db.Purchase_Order_Masters.InsertOnSubmit(S);
                //    db.SubmitChanges();
                //}
                ////db.Transaction = transaction;
                //for (int i = 0; i < dgProducts.RowCount - 1; i++)
                //{
                //    Purchase_Order_Child SC = new Purchase_Order_Child();
                //    var d1 = (from a in db.Purchase_Order_Masters where a.PO_NO == myString && a.Po_Amend_No == txtAmendNo.Text && a.Company_ID == logIn.company select new { a.Id }).ToList();
                //    SC.PO_Master_ID = d1[0].Id;

                //    SC.PO_NO = myString;
                //    //SC.Prod_Code = (dgProducts.Rows[i].Cells["Item_Code"].Value == null) ? "" : Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value).ToString());
                //    SC.Prod_Code = (dgProducts.Rows[i].Cells["Item_Code"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value);

                //    SC.Product_Description = (dgProducts.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value).ToString();
                //    SC.Prod_Spec = (dgProducts.Rows[i].Cells["Item_spec"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_spec"].Value).ToString();
                //    SC.Prod_Grade = (dgProducts.Rows[i].Cells["Item_Grade"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value).ToString();
                //    SC.Uom = (dgProducts.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["uom"].Value.ToString();
                //    SC.PR_Qty = (dgProducts.Rows[i].Cells["PR_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["PR_Qty"].Value);
                //    SC.Qty = (dgProducts.Rows[i].Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty"].Value);
                //    SC.Price = (dgProducts.Rows[i].Cells["Basic_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Basic_Price"].Value);

                //    SC.Amount = (dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value);
                //    SC.Disc_Per = (dgProducts.Rows[i].Cells["Disc_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Per"].Value);
                //    SC.Disc_Amount = (dgProducts.Rows[i].Cells["Disc_Amt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Amt"].Value);
                //    SC.Taxable_Value = (dgProducts.Rows[i].Cells["Taxable_Value"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Taxable_Value"].Value);
                //    SC.CGST_Per = (dgProducts.Rows[i].Cells["CGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Per"].Value);
                //    SC.SGST_Per = (dgProducts.Rows[i].Cells["SGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Per"].Value);
                //    SC.IGST_Per = (dgProducts.Rows[i].Cells["IGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Per"].Value);
                //    SC.CGST_Amnt = (dgProducts.Rows[i].Cells["CGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Amt"].Value);
                //    SC.SGST_Amnt = (dgProducts.Rows[i].Cells["SGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Amt"].Value);
                //    SC.IGST_Amnt = (dgProducts.Rows[i].Cells["IGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Amt"].Value);
                //    SC.Net_Amount = (dgProducts.Rows[i].Cells["Total_Amount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Total_Amount"].Value);
                //    SC.PR_NO = (dgProducts.Rows[i].Cells["PR_NO"].Value == null) ? "" : (dgProducts.Rows[i].Cells["PR_NO"].Value).ToString();
                //    SC.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value).ToString();
                //    SC.ProdSno = i + 1;
                //    SC.Company_ID = logIn.company;
                //    db.Purchase_Order_Childs.InsertOnSubmit(SC);
                //}
                //db.SubmitChanges();
                ////transaction.Commit();               
                //MessageBox.Show("Record Saved / Updated Successfully With Transaction Ref No : " + txtSoNo.Text);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        public void GetTot()
        {
            try
            {
                double totQty = 0;
                decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
                for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                {
                    totQty += (dgProducts.Rows[i].Cells["Qty"].Value.ToString() == "" || dgProducts.Rows[i].Cells["Qty"].Value == null || dgProducts.Rows[i].Cells["Qty"].Value == DBNull.Value) ? Convert.ToDouble(0) : Convert.ToDouble(dgProducts.Rows[i].Cells["Qty"].Value);
                    //x += (dgProducts.Rows[i].Cells["Inv_Qty"].Value == "" || dgProducts.Rows[i].Cells["Inv_Qty"].Value == null || dgProducts.Rows[i].Cells["Inv_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Inv_Qty"].Value);
                    y += (dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value.ToString() == "" || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == null || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value);
                    q += (dgProducts.Rows[i].Cells["Disc_Amt"].Value.ToString() == "" || dgProducts.Rows[i].Cells["Disc_Amt"].Value == null || dgProducts.Rows[i].Cells["Disc_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Amt"].Value);
                    v += (dgProducts.Rows[i].Cells["Taxable_Value"].Value.ToString() == "" || dgProducts.Rows[i].Cells["Taxable_Value"].Value == null || dgProducts.Rows[i].Cells["Taxable_Value"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Taxable_Value"].Value);
                    cg += (dgProducts.Rows[i].Cells["CGST_Amt"].Value.ToString() == "" || dgProducts.Rows[i].Cells["CGST_Amt"].Value == null || dgProducts.Rows[i].Cells["CGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Amt"].Value);
                    sg += (dgProducts.Rows[i].Cells["SGST_Amt"].Value.ToString() == "" || dgProducts.Rows[i].Cells["SGST_Amt"].Value == null || dgProducts.Rows[i].Cells["SGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Amt"].Value);
                    ig += (dgProducts.Rows[i].Cells["IGST_Amt"].Value.ToString() == "" || dgProducts.Rows[i].Cells["IGST_Amt"].Value == null || dgProducts.Rows[i].Cells["IGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Amt"].Value);
                    totA += (dgProducts.Rows[i].Cells["Total_Amount"].Value.ToString() == "" || dgProducts.Rows[i].Cells["Total_Amount"].Value == null || dgProducts.Rows[i].Cells["Total_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Total_Amount"].Value);
                    cgstPer = Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Per"].Value);
                    sgstPer = Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Per"].Value);
                    igstPer = Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Per"].Value);
                }

                txtTotalQty.Text = totQty.ToString(".00000");
                txtSubTotal.Text = y.ToString("0.00");
                txtTotDiscount.Text = q.ToString(".00");
                decimal fAmt = (txtFrieght.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtFrieght.Text);
                decimal OthAmt = (txtOtherCharges.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtOtherCharges.Text);
                decimal pAmt = (txtPackingCharges.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtPackingCharges.Text);
                
                txtTot_TaxableValue.Text = (v ).ToString(".00");
                decimal gstonFrieght = 0;
                decimal gstAmtonfrieght = 0;
                decimal gstAmtonOthAmt = 0;
                decimal gstAmtonpAmt = 0;
                gstonFrieght = (txtGSTonFrieght.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtGSTonFrieght.Text);
                if (igstPer > 0)
                {
                    
                    gstAmtonfrieght = (fAmt * gstonFrieght) / 100;
                    gstAmtonOthAmt = (OthAmt * gstonFrieght) / 100;
                    gstAmtonpAmt = (pAmt * gstonFrieght) / 100;
                    decimal igstamt = ig + gstAmtonpAmt + gstAmtonOthAmt + gstAmtonfrieght;
                    txtTot_IGST.Text = (igstamt).ToString(".00");
                    txtTot_SGST.Text = "00";
                    txtTot_CGST.Text = "00";
                }
                else
                {
                    gstAmtonfrieght = ((fAmt * gstonFrieght) / 100)/2;
                    gstAmtonOthAmt = ((OthAmt * gstonFrieght) / 100)/2;
                    gstAmtonpAmt = ((pAmt * gstonFrieght) / 100)/2;
                    decimal sgstamt = sg + gstAmtonpAmt + gstAmtonOthAmt + gstAmtonfrieght;
                    decimal cgstamt = cg + gstAmtonpAmt + gstAmtonOthAmt + gstAmtonfrieght;
                    txtTot_IGST.Text = "0";
                    txtTot_SGST.Text = (sgstamt).ToString(".00");
                    txtTot_CGST.Text = (cgstamt).ToString(".00");
                }


                decimal taxvalue = Convert.ToDecimal(txtTot_TaxableValue.Text);
                decimal customduty = (cmbCDuty.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(cmbCDuty.Text);
                decimal cAmt = 0;
                decimal cessPer = (txtCessPer.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtCessPer.Text);
                decimal cessAmt = 0;
                if (customduty>0)
                {
                    cAmt = (taxvalue * customduty) / 100;
                    cessAmt = (cAmt * cessPer) / 100;
                }
                
                txtCdutyAmt.Text = cAmt.ToString(".00");
                txtCessAmt.Text = cessAmt.ToString(".00");

                taxvalue = taxvalue + cAmt+cessAmt;
                //decimal cgst = (taxvalue * cgstPer) / 100;
                //decimal sgst = (taxvalue * sgstPer) / 100;
                //decimal igst = (taxvalue * igstPer) / 100;

                decimal cgst =Convert.ToDecimal(txtTot_CGST.Text);
                decimal sgst = Convert.ToDecimal(txtTot_SGST.Text);
                decimal igst = Convert.ToDecimal(txtTot_IGST.Text);


                decimal AmtForTCs = (v);
                //decimal AmtForTCs = (fAmt + OthAmt + v + cgst + sgst + igst);
                decimal rndAmt = (txtRounding.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtRounding.Text);
                txtTot_OrderValue.Text = (taxvalue + fAmt+OthAmt+pAmt+ cgst + sgst + igst + rndAmt).ToString(".00");
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
                bindCustomer();
                //txtSoNo.Text =MaterialManagement.ListOfPurchaseOrders.SO_No;
                txtAmendNo.Text = MaterialManagement.PurchaseOrdersList.SO_Amend_No;
                String myString = "";
                int POMasterID = 0;
                myString = txtSoNo.Text;
                var da = (from obj in db.Purchase_Order_Masters
                          where obj.PO_NO == txtSoNo.Text &&  obj.Po_Amend_No == txtAmendNo.Text && obj.Company_ID == logIn.company && obj.Status !=24
                          select obj).ToList();

                if (da.Count > 0)
                {
                    POMasterID = da[0].Id;
                    txtSoNo.Text = da[0].PO_NO.ToString();
                    dpSODate.Text = da[0].PO_Date.ToString();
                    //bindCustomer();
                    CmbBuyerName.SelectedValue = da[0].SupplierName;
                    CmbConsigneeName.SelectedValue = da[0].ConsigneeName;
                    txtCustGSTNo.Text = da[0].Cust_GST_No;
                    txtAmendNo.Text = da[0].Po_Amend_No;
                    dtAmendDate.Text = da[0].Po_Amend_Date.ToString();                    
                    if (da[0].Multi_Loc_Delivery == true)
                    {
                        chkMultiLocation.Checked = true;
                    }
                    else
                    {
                        chkMultiLocation.Checked = false;
                    }

                    //cmbCustomer.Enabled = false;
                    cmbBasis.Text = da[0].Purchase_Bases;
                    txtcontactperson.Text = da[0].Contact_Person.ToString();

                    txtTotalQty.Text = da[0].TotalQty.ToString();
                    txtSubTotal.Text = da[0].SubTotal.ToString();
                    txtTotDiscount.Text = da[0].Tot_Discount.ToString();
                    txtTot_TaxableValue.Text = da[0].Tot_TaxableValue.ToString();
                    txtTot_CGST.Text = da[0].Tot_CGST_Amnt.ToString();
                    txtTot_SGST.Text = da[0].Tot_SGST_Amnt.ToString();
                    txtTot_IGST.Text = da[0].Tot_IGST_Amnt.ToString();
                    txtFrieght.Text = da[0].Freight.ToString();
                    txtOtherCharges.Text = da[0].Other_Charges.ToString();
                    txtPackingCharges.Text = da[0].Packing_Charges.ToString();
                    txtcontactEmail.Text = da[0].Contact_EMail;
                    txtTot_OrderValue.Text = da[0].Tot_Ord_Value.ToString();
                    txtCustPoNo.Text = da[0].Supplier_QutoNo;
                    dpPODate.Text = da[0].Supplier_QutoDate.ToString();
                    dtDeliveryDate.Text = da[0].Delivery_Date.ToString();
                    cmbShipTo.SelectedValue = (da[0].Ship_To==null)?0:da[0].Ship_To;
                    cmbPriceBasis.SelectedValue = da[0].Price_Basis;
                    cmbInsurance.SelectedValue = da[0].Insurance_Scope;
                    cmbPaymentTerms.SelectedValue = da[0].PaymentTerms;
                    cmbTransport_Scope.SelectedValue = da[0].Trasnport_Scope;
                    cmbModeofDesp.SelectedValue = da[0].Desp_Mode;
                    txtqtytolerence.Text = da[0].Qty_Tolerence;
                    cmbTransporter.Text = da[0].Transporter_Name;
                    txtCustomeContact.Text = da[0].Customer_Contact;
                    txtOtherTerms.Text = da[0].Other_Terms;
                    txtLDClause.Text = da[0].LD_Description;
                    dtLDClause.Text = da[0].LD_Date.ToString();
                    if (da[0].LD_Clause_applicable == true)
                    {
                        chkLDClause.Checked = true;
                    }
                    else
                    {
                        chkLDClause.Checked = false;
                    }
                    if (da[0].Import_PO == true)
                    {
                        chkImportPO.Checked = true;
                    }
                    else
                    {
                        chkImportPO.Checked = false;
                    }
                    if (da[0].Raw_Material_PO == true)
                    {
                        checkBox1.Checked = true;
                    }
                    else
                    {
                        checkBox1.Checked = false;
                    }
                    txtWarrenty.Text = da[0].Warrenty;
                    cmbFCurrency.Text = da[0].FCurrency;
                    txtExchangeRate.Text = da[0].ExchangeRate.ToString();
                    txtSplInstructions.Text = da[0].Spl_Instructions;                    
                    cmbStatus.SelectedValue = da[0].Status;
                    txtCustomPaymentTerms.Text = da[0].Custom_Payment_Terms;
                    cmbCDuty.Text = da[0].Custom_Duty_Per.ToString();
                    txtCdutyAmt.Text = da[0].Custom_Duty_Amt.ToString();
                    txtCessPer.Text = da[0].Cess_CD_Per.ToString();
                    txtCessAmt.Text = da[0].Cess_CD_Amt.ToString();
                    txtCustomDutyRemarks.Text = da[0].Custom_Duty_Remarks;
                    txtTestCertificates.Text = da[0].Test_Certificates;
                    txtIdentification.Text = da[0].Identifcation_Text;
                    txtDespatchDocuments.Text = da[0].Despatch_Documents;
                    txtContainerDetention.Text = da[0].Container_Detention;
                    if (da[0].Shipping_Address != null)
                    {
                        txtShippingAddress.Text = da[0].Shipping_Address;
                    }
                    lblCreatedBy.Text = da[0].Created_By;
                    lblModified.Text = da[0].Modified_By;                    
                    txtRounding.Text = da[0].Rounding_Amt.ToString();
                }

                if (logIn.company == 1044)
                {
                    var dm1 = (from s in db.Purchase_Order_Childs
                               join P in db.Products on s.Prod_Code equals P.prod_ID
                               join u in db.UoM_Masters on P.Prod_Primary_UOM_Id equals u.UOM_ID
                               where s.PO_Master_ID == POMasterID
                               select new

                               {
                                   S_No = s.ProdSno,
                                   Prod_Code = P.Prod_Alternative_Code,
                                   Item_Code = s.Prod_Code,
                                   Item_Description = P.Prod_Name,
                                   Item_spec = s.Prod_Spec.Trim(),
                                   Item_Grade = s.Prod_Grade.Trim(),
                                   UOM = u.Uom_Descr,
                                   s.PR_Qty,
                                   s.Qty,
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
                                   PR_No = s.PR_NO.Trim(),
                                   s.Remarks,

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
                    var dm1 = (from s in db.Purchase_Order_Childs
                               join P in db.Products on s.Prod_Code equals P.prod_ID
                               join u in db.UoM_Masters on P.Prod_Primary_UOM_Id equals u.UOM_ID
                               where s.PO_Master_ID == POMasterID
                               select new

                               {
                                   S_No = s.ProdSno,
                                   Prod_Code = P.Prod_Code,
                                   Item_Code = s.Prod_Code,
                                   Item_Description = P.Prod_Name,
                                   Item_spec = s.Prod_Spec.Trim(),
                                   Item_Grade = s.Prod_Grade.Trim(),
                                   UOM = u.Uom_Descr,
                                   s.PR_Qty,
                                   s.Qty,
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
                                   PR_No = s.PR_NO.Trim(),
                                   s.Remarks,

                               });
                    SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    DataTable dtr = new DataTable();
                    da2.Fill(dtr);
                    if (dtr.Rows.Count >= 0)
                        dgProducts.DataSource = dtr;
                }
                //int j = 0;
                //for (int i = 0; i <= dgProducts.Rows.Count - 1; i++)
                //{
                //    j = j + 1;
                //    dgProducts.Rows[i].Cells["S_No"].Value = j.ToString();
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
