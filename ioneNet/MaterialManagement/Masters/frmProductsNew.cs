using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.IO;
using System.Data.OleDb;
using System.Configuration;
using Ione_DAL;

namespace ioneNet.MaterialManagement.Masters
{
    public partial class frmProductsNew : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        SqlCommand cmd;
        string columnName;
        Boolean ProductEdit = false;
        public frmProductsNew()
        {
            InitializeComponent();
        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void frmProductsNew_Load(object sender, EventArgs e)
        {
            //this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

            cmbProdType.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbProdType.AutoCompleteSource = AutoCompleteSource.ListItems;

            cmbProdGroup.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbProdGroup.AutoCompleteSource = AutoCompleteSource.ListItems;

            cmbUom.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbUom.AutoCompleteSource = AutoCompleteSource.ListItems;

            cmbAltUom.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbAltUom.AutoCompleteSource = AutoCompleteSource.ListItems;

            cmbStorageLoc.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbStorageLoc.AutoCompleteSource = AutoCompleteSource.ListItems;

            cmbMtrlGrade.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbMtrlGrade.AutoCompleteSource = AutoCompleteSource.ListItems;

            cmbScrapProduct.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbScrapProduct.AutoCompleteSource = AutoCompleteSource.ListItems;
            BindMasters();
            ProductEdit = false;
            txtProd_PartNo.Enabled = true;
            label18.Text = "Customer Code";
            if (logIn.company == 1044)
            {
                txtProd_PartNo.Enabled = false;
                label18.Text = "CAS No";
            }

            if(logIn.company ==25)
            {
                label18.Text = "Product Dia";
                label26.Text = "Cutting Cost";
            }
            if (ProductList.var == "0")
            {

                ProductList.var = null;
                GetProductMasterData(ProductList.productCode);
                ProductEdit = true;
            }
            else

            if (ProductList.var == "2")
            {
                GetProductMasterData(ProductList.productCode);
                ProductEdit = true;
                btnSave.Enabled = false;
                btnClear.Enabled = false;
                
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void GetProductMasterData(int ProductCode)
        {
            try
            {
                var ProductMasterList = (from prdmstr in db.Products where prdmstr.prod_ID == ProductCode select prdmstr).ToList();
                if (ProductMasterList.Count > 0)
                {
                    ProdID.Text = ProductCode.ToString();
                    txtProdID.Text = (ProductMasterList[0].Prod_Code);
                    txtProdCode.Text = ProductMasterList[0].Prod_Alternative_Code;
                    txtProductName.Text = (ProductMasterList[0].Prod_Name);
                    txtProdAliasName.Text = ProductMasterList[0].Prod_Alias_Name;
                    cmbProdType.SelectedValue = (ProductMasterList[0].Prod_Type_Id);
                    cmbProdGroup.SelectedValue = (ProductMasterList[0].Prod_Group_Id);
                   // cmbStorageLoc.SelectedValue = (ProductMasterList[0].Prod_Storage_Location_Id);
                    cmbUom.SelectedValue = (ProductMasterList[0].Prod_Primary_UOM_Id);
                    if (ProductMasterList[0].Prod_Alternative_UOM_Id != null)
                    {
                        cmbAltUom.SelectedValue = (ProductMasterList[0].Prod_Alternative_UOM_Id);
                    }
                    else

                    {
                        cmbAltUom.SelectedValue = (ProductMasterList[0].Prod_Primary_UOM_Id);
                    }
                    txtConvFormula.Text = Convert.ToString(ProductMasterList[0].Conv_Formula);
                    if (ProductMasterList[0].Prod_Storage_Location_Id != null)
                    {
                        cmbStorageLoc.SelectedValue = (ProductMasterList[0].Prod_Storage_Location_Id);
                    }
                    txtProd_Unit_Wt.Text = Convert.ToString(ProductMasterList[0].Prod_Unit_Wt);
                    txtProd_Color_Code.Text = Convert.ToString(ProductMasterList[0].Prod_Color_Code);
                    txtProd_Version.Text = Convert.ToString(ProductMasterList[0].Prod_Version);
                    txtProd_PartNo.Text = Convert.ToString(ProductMasterList[0].Prod_PartNo);
                    txtProd_Mfg_Code.Text = Convert.ToString(ProductMasterList[0].Prod_Mfg_Code);
                    txtProd_Cust_Code.Text = Convert.ToString(ProductMasterList[0].Prod_Customer_Code);
                    txtProd_Description.Text = Convert.ToString(ProductMasterList[0].Prod_Description);

                    txtShelfLifeDays.Text = Convert.ToString(ProductMasterList[0].Prod_Shelf_Life);

                    if (ProductMasterList[0].Prod_isShelfLife != null)
                    {
                        chkShelfLife.Checked = ProductMasterList[0].Prod_isShelfLife.Value;
                    }
                    if (ProductMasterList[0].Prod_IsCritical_Item != null)
                    {
                        chkCriticalItem.Checked = ProductMasterList[0].Prod_IsCritical_Item.Value;
                    }
                    if (ProductMasterList[0].Prod_IsBOM_Item != null)
                    {
                        chkBOM.Checked = ProductMasterList[0].Prod_IsBOM_Item.Value;
                    }

                    //cmbScrapProduct.SelectedValue = (ProductMasterList[0].Prod_Scrap_Product_Id);
                    txtHSNCode.Text = Convert.ToString(ProductMasterList[0].Prod_HSN_Code);
                    if (ProductMasterList[0].Prod_Tax_Class != null)
                    {
                        cmbTaxClass.SelectedValue = (ProductMasterList[0].Prod_Tax_Class);
                    }
                 
                    if (ProductMasterList[0].Purchase_Account != null)
                    {
                        cmbProdCC.SelectedValue = ProductMasterList[0].Purchase_Account;

                    }
                    if (ProductMasterList[0].Prod_Scrap_Product_Id != null)
                    {
                        cmbScrapProduct.SelectedValue = (ProductMasterList[0].Prod_Scrap_Product_Id);
                    }
                    if (ProductMasterList[0].Prod_Field1 != null)
                    {
                        cmbMtrlGrade.SelectedValue = (ProductMasterList[0].Prod_Field1);
                    }
                    cmbStatus.SelectedValue = ProductMasterList[0].Prod_Status_ID;

                    //cmbMtrlGrade.Text = Convert.ToString(ProductMasterList[0].Prod_Field1);
                    txtField2.Text = ProductMasterList[0].Prod_Field2;
                    cmbTaxClass.SelectedValue = ProductMasterList[0].Prod_Tax_Class;
                    cmbProdCC.SelectedValue = ProductMasterList[0].Purchase_Account;
                    txtHSNCode.Text = ProductMasterList[0].Prod_HSN_Code.ToString();
                    if (ProductMasterList[0].Prod_Image != null)
                    {
                        var f = (from s in db.Products where s.Prod_Code == txtProdID.Text select s);
                        SqlCommand cmd = (SqlCommand)db.GetCommand(f);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        DataSet ds = new DataSet("MyImages");
                        byte[] MyData = new byte[0];
                        da.Fill(ds, "MyImages");
                        DataRow myRow;
                        myRow = ds.Tables["MyImages"].Rows[0];
                        MyData = (byte[])myRow["Prod_Image"];
                        MemoryStream stream = new MemoryStream(MyData);
                        picProduct.Image = Image.FromStream(stream);


                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Products", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

        }
        public static void CheckFill(Control control, bool enabled)
        {
            control.Enabled = enabled;
            foreach (Control child in control.Controls)
            {
                if (child is TextBox && string.IsNullOrWhiteSpace(child.Text))
                {
                    if (child.Tag == "r")
                    {
                        MessageBox.Show(string.Format("Field {0} Cannot Be Empty", child.Name.Substring(3)));
                        child.Focus();
                        return;
                    }

                }
                CheckFill(child, enabled);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cmbProdType.Text == string.Empty)
                {
                    MessageBox.Show("Product Type Should Not Be Empty", "Products", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbProdType.Focus();
                    return;
                }
                else if (txtProductName.Text == string.Empty)
                {
                    MessageBox.Show("Product Name  Should Not Be Empty");
                    txtProductName.Focus();
                    return;
                }
                else if (txtProdID.Text == string.Empty)
                {
                    MessageBox.Show("Product Code  Should Not Be Empty");
                    txtProdID.Focus();
                    return;
                }
                else if (cmbProdGroup.Text == string.Empty)
                {
                    MessageBox.Show("Select Category,");
                    cmbProdGroup.Focus();
                    return;
                }
                else if (cmbUom.Text == string.Empty)
                {
                    MessageBox.Show("Please Select UOM");
                    cmbUom.Focus();
                    return;
                }
                else if (cmbAltUom.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Alternate UOM");
                    cmbUom.Focus();
                    return;
                }
                else if (cmbTaxClass.Text == string.Empty)
                {
                    MessageBox.Show("Tax Class Cannot be Empty");
                    cmbTaxClass.Focus();
                    return;
                }
                else if (txtHSNCode.Text == string.Empty)
                {
                    MessageBox.Show("HSN Code Cannot be Empty");
                    txtHSNCode.Focus();
                    return;
                }
                else if (cmbStatus.Text == string.Empty)
                {
                    MessageBox.Show("Select Status ");
                    cmbStatus.Focus();
                    return;
                }
                else
                {
                    save();


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Products", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        public void Clear()
        {

            try
            {
                foreach (Control c in groupBox1.Controls.Cast<Control>().OrderBy(c => c.TabIndex))
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
                foreach (Control c in tabPage1.Controls.Cast<Control>().OrderBy(c => c.TabIndex))
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
                lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

                txtHSNCode.Text = "";
                cmbTaxClass.Text = "";
                cmbProdCC.SelectedValue = logIn.BU_ID;
                ProductEdit = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Products", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void autogen()
        {
            try

            {
                //var result = db.Sp_autoincrement_ProdMaster(Convert.ToInt32(cmbProdType.SelectedValue),logIn.company);
                if (cmbProdType.Text == "Project Items")
                {

                    var result = db.Sp_autoincrement_ProdMaster_ProjectItems(txtProdCode.Text, Convert.ToInt32(cmbProdGroup.SelectedValue), logIn.company);
                    txtProdID.Text = result.FirstOrDefault().Product_Code;
                }
                else
                {
                    var result = db.Sp_autoincrement_ProdMaster_New(Convert.ToInt32(cmbProdType.SelectedValue), Convert.ToInt32(cmbProdGroup.SelectedValue), logIn.company);
                    txtProdID.Text = result.FirstOrDefault().Product_Code;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Products", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void BindMasters()
        {
            try
            {
                //    //Bind product Types
                //Get Allowed Product Types for the user logged in


                //string Ptype = "";
                //var GetTypes = (from m in db.Purchase_Group_Masters.Where(w => w.User_Name.Contains(logIn.username))

                                 
                //                 select new
                //                 {
                //                     m.Product_Types
                                    
                //                 }).ToList();

                //if (GetTypes.Count > 0)
                //{
                //    Ptype = GetTypes[0].Product_Types;
                //}
                    var bindTypes = (from m in db.Attributes_Prod_Types
                                 
                                 where m.Company_ID == logIn.company 
                                 //&& m.Prod_Type.Contains(Ptype)
                                 select new
                                 {
                                     m.Prod_Type,
                                     m.Prod_Type_Id,
                                 }).ToList();

                if (bindTypes.Count > 0)
                {
                    cmbProdType.DataSource = bindTypes;
                    cmbProdType.DisplayMember = "Prod_Type";
                    cmbProdType.ValueMember = "Prod_Type_Id";
                    cmbProdType.SelectedIndex = -1;

                }

                //    //Bind product Groups
                var bindGroups = (from m in db.Product_Groups
                                  where m.Company_ID == logIn.company
                                  select new
                                  {
                                      m.Prod_Group_Name,
                                      m.ID,
                                  }).ToList();

                if (bindGroups.Count > 0)
                {
                    cmbProdGroup.DataSource = bindGroups;
                    cmbProdGroup.DisplayMember = "Prod_Group_Name";
                    cmbProdGroup.ValueMember = "ID";
                    cmbProdGroup.SelectedIndex = -1;
                }


                //    //Bind UOM               

                var bindUOM = (from m in db.UoM_Masters
                                   // where m.Company_ID == logIn.company
                               select new
                               {
                                   m.Uom_Descr,
                                   m.UOM_ID,
                               }).Distinct().ToList();

                if (bindUOM.Count > 0)
                {
                    cmbUom.DataSource = bindUOM;
                    cmbUom.DisplayMember = "Uom_Descr";
                    cmbUom.ValueMember = "UOM_ID";
                    cmbUom.SelectedIndex = -1;
                    //cmbAltUom.DataSource = bindUOM;
                    //cmbAltUom.DisplayMember = "Uom_Descr";
                    //cmbAltUom.ValueMember = "UOM_ID";

                }
                var bindUOM1 = (from m in db.UoM_Masters
                                where m.Company_ID == logIn.company
                                select new
                                {
                                    m.Uom_Descr,
                                    m.UOM_ID,
                                }).Distinct().ToList();

                if (bindUOM1.Count > 0)
                {
                    //cmbUom.DataSource = bindUOM;
                    //cmbUom.DisplayMember = "Uom_Descr";
                    //cmbUom.ValueMember = "UOM_ID";
                    cmbAltUom.DataSource = bindUOM1;
                    cmbAltUom.DisplayMember = "Uom_Descr";
                    cmbAltUom.ValueMember = "UOM_ID";
                    cmbAltUom.SelectedIndex = -1;

                }

                //Bind Tax Calss
                var bindTax = (from m in db.Tax_Class_Masters
                               where m.Company_ID == logIn.company
                               select new
                               {
                                   m.Tax_Class_Name,
                                   m.ID,
                               }).ToList();

                if (bindTax.Count > 0)
                {

                    cmbTaxClass.DataSource = bindTax;
                    cmbTaxClass.DisplayMember = "Tax_Class_Name";
                    cmbTaxClass.ValueMember = "ID";
                    cmbTaxClass.SelectedIndex = -1;

                }

                //Bind Cost Center
                var bindCC = (from m in db.Costing_Units
                              where m.Company == logIn.company
                              select new
                              {
                                  m.BU_Name,
                                  m.id,
                              }).ToList();

                if (bindCC.Count > 0)
                {

                    cmbProdCC.DataSource = bindCC;
                    cmbProdCC.DisplayMember = "BU_Name";
                    cmbProdCC.ValueMember = "id";
                    cmbProdCC.SelectedIndex = -1;

                }
                cmbProdCC.SelectedValue = logIn.BU_ID;
                ////Bind Purchase Accounts
                //var bindPurchaseAct = (from m in db.AccountMasters
                //                    where m.AccGroup == "All Purchase Accounts" && m.Company_ID == logIn.company
                //                       select new
                //               {
                //                   m.AccName,
                //                   m.id,
                //               }).ToList();

                //if (bindPurchaseAct.Count > 0)
                //{
                //    cmbPurchaseAccount.DataSource = bindPurchaseAct;
                //    cmbPurchaseAccount.DisplayMember = "AccName";
                //    cmbPurchaseAccount.ValueMember = "id";
                //    cmbPurchaseAccount.SelectedIndex = -1;

                //}
                ////Bind Sale Accounts
                //var bindSaleAct = (from m in db.AccountMasters
                //                       where m.AccGroup == "All Sales Accounts" && m.Company_ID == logIn.company
                //                   select new
                //                       {
                //                           m.AccName,
                //                           m.id,
                //                       }).ToList();

                //if (bindSaleAct.Count > 0)
                //{
                //    cmbSaleAccount.DataSource = bindSaleAct;
                //    cmbSaleAccount.DisplayMember = "AccName";
                //    cmbSaleAccount.ValueMember = "id";
                //    cmbSaleAccount.SelectedIndex = -1;

                //}

                //Status
                var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Master" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbStatus.DataSource = pStatus;
                    cmbStatus.ValueMember = "ID";
                    cmbStatus.DisplayMember = "Descr";
                }

                //Scrap Product
                var pscrap = (from m in db.RM_Group_Masters where m.Company_ID == logIn.company select new { m.id, m.RM_Group_Name }).Distinct().ToList();
                if (pscrap.Count > 0)
                {
                    cmbScrapProduct.DataSource = pscrap;
                    cmbScrapProduct.ValueMember = "id";
                    cmbScrapProduct.DisplayMember = "RM_Group_Name";
                    cmbScrapProduct.SelectedIndex = -1;
                }

                //Material Grades
                var Mgrade = (from m in db.QA_Mtrl_Grade_Masters where m.Company_ID == logIn.company select new { m.id, m.Material_Grade }).Distinct().ToList();
                if (Mgrade.Count > 0)
                {
                    cmbMtrlGrade.DataSource = Mgrade;
                    cmbMtrlGrade.ValueMember = "id";
                    cmbMtrlGrade.DisplayMember = "Material_Grade";
                    cmbMtrlGrade.SelectedIndex = -1;
                }

                //storage location
                var sl = (from m in db.Storage_Locations where m.Company_ID == logIn.company select new { m.Storage_Loc_Id, m.Storage_Loc_Name }).Distinct().ToList();
                if (sl.Count > 0)
                {
                    cmbStorageLoc.DataSource = sl;
                    cmbStorageLoc.ValueMember = "Storage_Loc_Id";
                    cmbStorageLoc.DisplayMember = "Storage_Loc_Name";
                    cmbStorageLoc.SelectedIndex = -1;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbProdType_Leave(object sender, EventArgs e)
        {
            try
            {
                if (cmbProdType.Text != "")
                {
                    var bindLoc = (from m in db.Attributes_Prod_Types
                                   where m.Prod_Type == cmbProdType.Text && m.Company_ID == logIn.company
                                   select new
                                   {
                                       m.Type_PreFix,
                                   }).ToList();

                    if (bindLoc.Count > 0)
                    {


                        cmd = new SqlCommand("select isnull( MAX(convert(int,substring(Prod_Code,3,len(Prod_Code)))),0)+1 from Products where Prod_Type_Id='" + cmbProdType.SelectedValue.ToString() + "'", con);
                        if (con.State == ConnectionState.Closed)
                            con.Open();
                        int i = Convert.ToInt32(cmd.ExecuteScalar());

                        if (i < 10)
                        {

                            txtProdID.Text = bindLoc[0].Type_PreFix + "000" + i;
                        }
                        else if (i >= 10 && i <= 99)
                        {

                            txtProdID.Text = bindLoc[0].Type_PreFix + "00" + i;
                        }
                        else if (i >= 100 && i <= 999)
                        {

                            txtProdID.Text = bindLoc[0].Type_PreFix + "0" + i;
                        }
                        else if (i >= 1000)
                        {

                            txtProdID.Text = bindLoc[0].Type_PreFix + i.ToString();
                        }

                        con.Close();
                    }
                }

            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void save()
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (ProdID.Text !="")
                {
                    if ((from u in db.Products where u.prod_ID == Convert.ToInt32(ProdID.Text) && u.Company_ID == logIn.company select u).Count() > 0)
                    {
                        var p1 = db.Products.Where(w => w.prod_ID == Convert.ToInt32(ProdID.Text) && w.Company_ID == logIn.company).FirstOrDefault();

                        p1.Prod_Code = txtProdID.Text;
                        p1.Prod_Alternative_Code = (txtProdCode.Text == "") ? "" : txtProdCode.Text;
                        p1.Prod_Group_Id = Convert.ToInt32(cmbProdGroup.SelectedValue.ToString());
                        p1.Prod_Description = (txtProd_Description.Text == "") ? "" : txtProd_Description.Text;
                        p1.Prod_Type_Id = Convert.ToInt32(cmbProdType.SelectedValue.ToString());
                        p1.Prod_Name = txtProductName.Text;
                        p1.Prod_Alias_Name = txtProdAliasName.Text;
                        p1.Prod_Mfg_Code = (txtProd_Mfg_Code.Text == "") ? "" : txtProd_Mfg_Code.Text;
                        p1.Prod_Storage_Location_Id = (cmbStorageLoc.SelectedValue == null) ? 0 : Convert.ToInt32(cmbStorageLoc.SelectedValue.ToString());
                        p1.Prod_Primary_UOM_Id = Convert.ToInt32(cmbUom.SelectedValue.ToString());
                        p1.Prod_Alternative_UOM_Id = Convert.ToInt32(cmbAltUom.SelectedValue.ToString());
                        p1.Prod_Unit_Wt = (txtProd_Unit_Wt.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtProd_Unit_Wt.Text);
                        p1.Conv_Formula = (txtConvFormula.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtConvFormula.Text);
                        //p.Prod_GST_Rate = (cmbTaxClass.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(cmbTaxClass,.Text);
                        p1.Prod_Tax_Class = (cmbTaxClass.SelectedValue == null) ? 0 : Convert.ToInt32(cmbTaxClass.SelectedValue.ToString());
                        //if (cmbProdCC.Text != "")
                        //{
                        //    p1.Purchase_Account = Convert.ToInt32(cmbProdCC.SelectedValue.ToString());
                        //}
                        p1.Purchase_Account = logIn.BU_ID;
                        
                        if (cmbScrapProduct.Text != "")
                        {
                            p1.Prod_Scrap_Product_Id = Convert.ToInt32(cmbScrapProduct.SelectedValue.ToString());
                        }

                        if (picProduct.Image != null)
                        {
                            Image img = picProduct.Image;
                            System.IO.MemoryStream ms = new MemoryStream();
                            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            byte[] bytes = ms.ToArray();
                            p1.Prod_Image = bytes;
                        }
                        else
                            p1.Prod_Image = null;
                        p1.Prod_PartNo = (txtProd_PartNo.Text == "") ? null : txtProd_PartNo.Text;
                        p1.Prod_IsBOM_Item = chkBOM.Checked;
                        p1.Prod_IsCritical_Item = chkCriticalItem.Checked;
                        p1.Prod_isShelfLife = chkShelfLife.Checked;
                        p1.Prod_Status_ID = Convert.ToInt32(cmbStatus.SelectedValue.ToString());
                        p1.Prod_Shelf_Life = (txtShelfLifeDays.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtShelfLifeDays.Text);
                        p1.Prod_Color_Code = (txtProd_Color_Code.Text == "") ? "" : txtProd_Color_Code.Text;
                        //p.Color_Name = (txtColorName.Text == "") ? "" : txtColorName.Text;
                        p1.Prod_Version = (txtProd_Version.Text == "") ? "" : txtProd_Version.Text;
                        p1.Prod_HSN_Code = (txtHSNCode.Text == "") ? "" : txtHSNCode.Text;

                        p1.Prod_Customer_Code = (txtProd_Cust_Code.Text == "") ? "" : txtProd_Cust_Code.Text;
                        p1.Created_By = lblCreatedBy.Text;
                        p1.Modified_BY = logIn.username + "-" + DateTime.Now;
                        p1.Company_ID = logIn.company;
                        if (cmbMtrlGrade.Text != "")
                        {
                            p1.Prod_Field1 = Convert.ToInt32(cmbMtrlGrade.SelectedValue.ToString());
                        }
                        //p1.Prod_Field1 = Convert.ToInt32(cmbMtrlGrade.SelectedValue.ToString());                    
                        p1.Prod_Field2 = (txtField2.Text == "") ? "" : txtField2.Text;
                        db.SubmitChanges();
                        MessageBox.Show("Record Updated Successfully");
                        Clear();
                    }
                    else
                    {
                        if (null != db.Connection)
                        {
                            db.Connection.Close();
                        }
                        System.Data.Common.DbTransaction transaction;
                        db.Connection.Open();
                        transaction = db.Connection.BeginTransaction();
                        db.Transaction = transaction;

                        Product p = new Product();
                        autogen();
                        if (cmbProdCC.Text != "")
                        {
                            p.Purchase_Account = Convert.ToInt32(cmbProdCC.SelectedValue.ToString());
                        }

                        p.Prod_Mfg_Code = (txtProd_Mfg_Code.Text == "") ? "" : txtProd_Mfg_Code.Text;
                        p.Prod_Code = txtProdID.Text;
                        p.Prod_Alternative_Code = (txtProdCode.Text == "") ? "" : txtProdCode.Text;
                        p.Prod_Group_Id = Convert.ToInt32(cmbProdGroup.SelectedValue.ToString());
                        p.Prod_Description = (txtProd_Description.Text == "") ? "" : txtProd_Description.Text;
                        p.Prod_Type_Id = Convert.ToInt32(cmbProdType.SelectedValue.ToString());
                        p.Prod_Name = txtProductName.Text;
                        p.Prod_Alias_Name = txtProdAliasName.Text;
                        p.Prod_Storage_Location_Id = (cmbStorageLoc.SelectedValue == null) ? 0 : Convert.ToInt32(cmbStorageLoc.SelectedValue.ToString());
                        p.Prod_Primary_UOM_Id = Convert.ToInt32(cmbUom.SelectedValue.ToString());
                        p.Prod_Alternative_UOM_Id = Convert.ToInt32(cmbAltUom.SelectedValue.ToString());
                        p.Prod_Unit_Wt = (txtProd_Unit_Wt.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtProd_Unit_Wt.Text);
                        p.Conv_Formula = (txtConvFormula.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtConvFormula.Text);
                        //p.Prod_GST_Rate = (cmbTaxClass.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(cmbTaxClass,.Text);
                        p.Prod_Tax_Class = (cmbTaxClass.SelectedValue == null) ? 0 : Convert.ToInt32(cmbTaxClass.SelectedValue.ToString());
                        //if (cmbPurchaseAccount.Text != "")
                        //{
                        //    p.Purchase_Account = Convert.ToInt32(cmbPurchaseAccount.SelectedValue.ToString());
                        //}
                        p.Purchase_Account = logIn.BU_ID;

                        
                        if (cmbScrapProduct.Text != "")
                        {
                            p.Prod_Scrap_Product_Id = Convert.ToInt32(cmbScrapProduct.SelectedValue.ToString());
                        }

                        if (picProduct.Image != null)
                        {
                            Image img = picProduct.Image;
                            System.IO.MemoryStream ms = new MemoryStream();
                            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            byte[] bytes = ms.ToArray();
                            p.Prod_Image = bytes;
                        }
                        else
                            p.Prod_Image = null;
                        p.Prod_IsCritical_Item = chkCriticalItem.Checked;
                        p.Prod_isShelfLife = chkShelfLife.Checked;
                        p.Prod_IsBOM_Item = chkBOM.Checked;
                        p.Prod_Shelf_Life = (txtShelfLifeDays.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtShelfLifeDays.Text);
                        p.Prod_Color_Code = (txtProd_Color_Code.Text == "") ? "" : txtProd_Color_Code.Text;
                        //p.Color_Name = (txtColorName.Text == "") ? "" : txtColorName.Text;
                        p.Prod_Version = (txtProd_Version.Text == "") ? "" : txtProd_Version.Text;
                        p.Prod_HSN_Code = (txtHSNCode.Text == "") ? "" : txtHSNCode.Text;
                        p.Prod_Status_ID = Convert.ToInt32(cmbStatus.SelectedValue.ToString());
                        p.Prod_Customer_Code = (txtProd_Cust_Code.Text == "") ? "" : txtProd_Cust_Code.Text;
                        p.Created_By = logIn.username + "-" + DateTime.Now;
                        p.Modified_BY = logIn.username + "-" + DateTime.Now;
                        p.Company_ID = logIn.company;
                        if (cmbMtrlGrade.Text != "")
                        {
                            p.Prod_Field1 = Convert.ToInt32(cmbMtrlGrade.SelectedValue.ToString());
                        }
                        //p.Prod_Field1 = Convert.ToInt32(cmbMtrlGrade.SelectedValue.ToString());
                        p.Prod_Field2 = (txtField2.Text == "") ? "" : txtField2.Text;
                        p.Prod_PartNo = (txtProd_PartNo.Text == "") ? null : txtProd_PartNo.Text;
                        db.Products.InsertOnSubmit(p);
                        db.SubmitChanges();
                        db.Transaction = transaction;
                        transaction.Commit();

                        MessageBox.Show("Record Saved Successfully");
                        Clear();
                    }

                }
                else
                {
                    if (null != db.Connection)
                    {
                        db.Connection.Close();
                    }
                    System.Data.Common.DbTransaction transaction;
                    db.Connection.Open();
                    transaction = db.Connection.BeginTransaction();
                    db.Transaction = transaction;

                    Product p = new Product();
                    autogen();
                    if (cmbProdCC.Text != "")
                    {
                        p.Purchase_Account = Convert.ToInt32(cmbProdCC.SelectedValue.ToString());
                    }
                    
                    p.Prod_Mfg_Code = (txtProd_Mfg_Code.Text == "") ? "" : txtProd_Mfg_Code.Text;
                    p.Prod_Code = txtProdID.Text;
                    p.Prod_Alternative_Code = (txtProdCode.Text == "") ? "" : txtProdCode.Text;
                    p.Prod_Group_Id = Convert.ToInt32(cmbProdGroup.SelectedValue.ToString());
                    p.Prod_Description = (txtProd_Description.Text == "") ? "" : txtProd_Description.Text;
                    p.Prod_Type_Id = Convert.ToInt32(cmbProdType.SelectedValue.ToString());
                    p.Prod_Name = txtProductName.Text;
                    p.Prod_Alias_Name = txtProdAliasName.Text;
                    p.Prod_Storage_Location_Id = (cmbStorageLoc.SelectedValue == null) ? 0 : Convert.ToInt32(cmbStorageLoc.SelectedValue.ToString());
                    p.Prod_Primary_UOM_Id = Convert.ToInt32(cmbUom.SelectedValue.ToString());
                    p.Prod_Alternative_UOM_Id = Convert.ToInt32(cmbAltUom.SelectedValue.ToString());
                    p.Prod_Unit_Wt = (txtProd_Unit_Wt.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtProd_Unit_Wt.Text);
                    p.Conv_Formula = (txtConvFormula.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtConvFormula.Text);
                    //p.Prod_GST_Rate = (cmbTaxClass.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(cmbTaxClass,.Text);
                    p.Prod_Tax_Class = (cmbTaxClass.SelectedValue == null) ? 0 : Convert.ToInt32(cmbTaxClass.SelectedValue.ToString());
                    //if (cmbPurchaseAccount.Text != "")
                    //{
                    //    p.Purchase_Account = Convert.ToInt32(cmbPurchaseAccount.SelectedValue.ToString());
                    //}
                    p.Purchase_Account = logIn.BU_ID;

                    
                    if (cmbScrapProduct.Text != "")
                    {
                        p.Prod_Scrap_Product_Id = Convert.ToInt32(cmbScrapProduct.SelectedValue.ToString());
                    }

                    if (picProduct.Image != null)
                    {
                        Image img = picProduct.Image;
                        System.IO.MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] bytes = ms.ToArray();
                        p.Prod_Image = bytes;
                    }
                    else
                        p.Prod_Image = null;
                    p.Prod_IsCritical_Item = chkCriticalItem.Checked;
                    p.Prod_isShelfLife = chkShelfLife.Checked;
                    p.Prod_IsBOM_Item = chkBOM.Checked;
                    p.Prod_Shelf_Life = (txtShelfLifeDays.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtShelfLifeDays.Text);
                    p.Prod_Color_Code = (txtProd_Color_Code.Text == "") ? "" : txtProd_Color_Code.Text;
                    //p.Color_Name = (txtColorName.Text == "") ? "" : txtColorName.Text;
                    p.Prod_Version = (txtProd_Version.Text == "") ? "" : txtProd_Version.Text;
                    p.Prod_HSN_Code = (txtHSNCode.Text == "") ? "" : txtHSNCode.Text;
                    p.Prod_Status_ID = Convert.ToInt32(cmbStatus.SelectedValue.ToString());
                    p.Prod_Customer_Code = (txtProd_Cust_Code.Text == "") ? "" : txtProd_Cust_Code.Text;
                    p.Created_By = logIn.username + "-" + DateTime.Now;
                    p.Modified_BY = logIn.username + "-" + DateTime.Now;
                    p.Company_ID = logIn.company;
                    if (cmbMtrlGrade.Text != "")
                    {
                        p.Prod_Field1 = Convert.ToInt32(cmbMtrlGrade.SelectedValue.ToString());
                    }
                    //p.Prod_Field1 = Convert.ToInt32(cmbMtrlGrade.SelectedValue.ToString());
                    p.Prod_Field2 = (txtField2.Text == "") ? "" : txtField2.Text;
                    p.Prod_PartNo= (txtProd_PartNo.Text == "") ? null : txtProd_PartNo.Text;
                    db.Products.InsertOnSubmit(p);
                    db.SubmitChanges();
                    db.Transaction = transaction;
                    transaction.Commit();

                    MessageBox.Show("Record Saved Successfully");
                    Clear();
                }
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                MessageBox.Show(ex.Message);
            }

            finally
            {
                if (null != db.Connection)
                {
                    db.Connection.Close();
                }
            }
            Cursor.Current = Cursors.Default;
        }

        private void cmbProdGroup_Leave(object sender, EventArgs e)
        {
            try
            {

                if (cmbProdGroup.Text != "")
                {
                    //if (ProductList.var != "0" && ProductList.var != null)
                    if (ProductEdit == false)
                    {

                        autogen();
                    }
                    else
                    {

                    }



                }
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbProdType_Leave_1(object sender, EventArgs e)
        {
            try
            {
                if (cmbProdType.Text == "Project Items")
                {
                    label11.Text = "Enter Project";
                }
                else
                {
                    label11.Text = "Alternative Code";
                }
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                MessageBox.Show(ex.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                OpenFileDialog open = new OpenFileDialog();
                if (open.ShowDialog() == DialogResult.OK)
                {
                    Image img = new Bitmap(open.FileName);
                    picProduct.Image = img;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtProdID_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbProdType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            ioneNet.Masters.ProductGroups frm = new ioneNet.Masters.ProductGroups();
            //frm.MdiParent = this.MdiParent;

            frm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ioneNet.MaterialManagement.UOM frm = new ioneNet.MaterialManagement.UOM();
            //frm.MdiParent = this.MdiParent;

            frm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ioneNet.Masters.ProductTypes frm = new ioneNet.Masters.ProductTypes();
            //frm.MdiParent = this.MdiParent;

            frm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ioneNet.MaterialManagement.StorageLocation frm = new ioneNet.MaterialManagement.StorageLocation();
            //frm.MdiParent = this.MdiParent;

            frm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ioneNet.FinanceManagement.TaxClass frm = new ioneNet.FinanceManagement.TaxClass();
            //frm.MdiParent = this.MdiParent;

            frm.Show();
        }

        private void cmbUom_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbProdGroup_Enter(object sender, EventArgs e)
        {
            try
            {

                //Bind product Groups
                string cValue = cmbProdGroup.Text;
                var bindGroups = (from m in db.Product_Groups
                                  where m.Company_ID == logIn.company
                                  select new
                                  {
                                      m.Prod_Group_Name,
                                      m.ID,
                                  }).ToList();

                if (bindGroups.Count > 0)
                {
                    cmbProdGroup.DataSource = bindGroups;
                    cmbProdGroup.DisplayMember = "Prod_Group_Name";
                    cmbProdGroup.ValueMember = "ID";
                    cmbProdGroup.SelectedIndex = -1;
                }
                cmbProdGroup.Text = cValue;




                //    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                //    AddJC(DataColl);
                //    txtJobCardNo.AutoCompleteCustomSource = DataColl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbProdType_Enter(object sender, EventArgs e)
        {
            try
            {
                string cValue = cmbProdType.Text;
                //Bind product Types
                var bindTypes = (from m in db.Attributes_Prod_Types
                                 where m.Company_ID == logIn.company
                                 select new
                                 {
                                     m.Prod_Type,
                                     m.Prod_Type_Id,
                                 }).ToList();

                if (bindTypes.Count > 0)
                {
                    cmbProdType.DataSource = bindTypes;
                    cmbProdType.DisplayMember = "Prod_Type";
                    cmbProdType.ValueMember = "Prod_Type_Id";
                    cmbProdType.SelectedIndex = -1;

                }
                cmbProdType.Text = cValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbUom_Enter(object sender, EventArgs e)
        {
            try
            {
                //Bind UOM
                string cValue = cmbUom.Text;
                var bindUOM = (from m in db.UoM_Masters
                               //where m.Company_ID == logIn.company
                               select new
                               {
                                   m.Uom_Descr,
                                   m.UOM_ID,
                               }).Distinct().ToList();

                if (bindUOM.Count > 0)
                {
                    cmbUom.DataSource = bindUOM;
                    cmbUom.DisplayMember = "Uom_Descr";
                    cmbUom.ValueMember = "UOM_ID";
                    cmbUom.SelectedIndex = -1;
                    //cmbAltUom.DataSource = bindUOM;
                    //cmbAltUom.DisplayMember = "Uom_Descr";
                    //cmbAltUom.ValueMember = "UOM_ID";

                }
                cmbUom.Text = cValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbAltUom_Enter(object sender, EventArgs e)
        {
            try
            {
                //Bind UOM
                string cValue = cmbAltUom.Text;
                var bindUOM = (from m in db.UoM_Masters
                               //where m.Company_ID == logIn.company
                               select new
                               {
                                   m.Uom_Descr,
                                   m.UOM_ID,
                               }).Distinct().ToList();

                if (bindUOM.Count > 0)
                {
                    cmbAltUom.DataSource = bindUOM;
                    cmbAltUom.DisplayMember = "Uom_Descr";
                    cmbAltUom.ValueMember = "UOM_ID";
                    cmbAltUom.SelectedIndex = -1;
                    //cmbAltUom.DataSource = bindUOM;
                    //cmbAltUom.DisplayMember = "Uom_Descr";
                    //cmbAltUom.ValueMember = "UOM_ID";

                }
                cmbAltUom.Text = cValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbStorageLoc_Enter(object sender, EventArgs e)
        {
            try
            {
                //Bind Storage Location
                string cValue = cmbStorageLoc.Text;
                var bindLoc = (from m in db.Storage_Locations
                               where m.Company_ID == logIn.company
                               select new
                               {
                                   m.Storage_Loc_Name,
                                   m.Storage_Loc_Id,
                               }).ToList();

                if (bindLoc.Count > 0)
                {
                    cmbStorageLoc.DataSource = bindLoc;
                    cmbStorageLoc.DisplayMember = "Storage_Loc_Name";
                    cmbStorageLoc.ValueMember = "Storage_Loc_Id";
                    cmbStorageLoc.SelectedIndex = -1;

                }
                cmbStorageLoc.Text = cValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbTaxClass_Enter(object sender, EventArgs e)
        {
            try
            {

                string cValue = cmbTaxClass.Text;
                //Bind Tax Calss
                var bindTax = (from m in db.Tax_Class_Masters
                               where m.Company_ID == logIn.company
                               select new
                               {
                                   m.Tax_Class_Name,
                                   m.ID,
                               }).ToList();

                if (bindTax.Count > 0)
                {

                    cmbTaxClass.DataSource = bindTax;
                    cmbTaxClass.DisplayMember = "Tax_Class_Name";
                    cmbTaxClass.ValueMember = "ID";
                    cmbTaxClass.SelectedIndex = -1;

                }
                cmbTaxClass.Text = cValue;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtProdCode_Enter(object sender, EventArgs e)
        {
            try
            {
                txtProdCode.AutoCompleteCustomSource = null;
                if (cmbProdType.Text == "Project Items")
                {
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    AddProject(DataColl);
                    txtProdCode.AutoCompleteCustomSource = DataColl;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void AddProject(AutoCompleteStringCollection coll)
        {
            try
            {

                var Prodname = (from d in db.Project_code_Masters


                                select new { d.Project_Code }).ToList();
                DataTable dt = new DataTable();
                dt.Columns.Add("Project_Code");
                foreach (var item in Prodname)
                {
                    dt.Rows.Add(item.Project_Code);
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

        public void GetProdName(AutoCompleteStringCollection coll)
        {
            try
            {

                var Prodname = (from d in db.Products
                                where d.Company_ID == logIn.company

                                select new { d.Prod_Name }).ToList();
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
            catch (Exception ex)
            {
                ex.Message.ToString();
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void txtProdCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtProdCode_Leave(object sender, EventArgs e)
        {

        }

        private void txtProductName_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtProductName.Text != "")
                {
                    if (txtProdID.Text == "")
                    {
                        if (logIn.company == 1044)
                        {
                            if ((from u in db.Products where u.Prod_Name == txtProductName.Text && u.Company_ID == logIn.company && u.Purchase_Account == logIn.BU_ID select u).Count() > 0)
                            {
                                MessageBox.Show("Product Name Cannot Be Duplicate");
                                txtProductName.Focus();
                                return;
                            }
                            else
                            {
                                var getProductName = (from s in db.Products
                                                      join u in db.UoM_Masters on s.Prod_Primary_UOM_Id equals u.UOM_ID
                                                      join g in db.Product_Groups on s.Prod_Group_Id equals g.ID
                                                      where s.Prod_Name == txtProductName.Text && s.Company_ID == logIn.company
                                                      select new { s.prod_ID, s.Prod_Primary_UOM_Id,s.Prod_Alternative_UOM_Id,  s.Prod_Group_Id, s.Prod_HSN_Code, s.Prod_Code, s.Prod_Tax_Class, s.Prod_Alternative_Code }).ToList();


                                if (getProductName.Count > 0)
                                {
                                    txtProd_PartNo.Text = getProductName[0].Prod_Code;
                                    cmbProdGroup.SelectedValue = getProductName[0].Prod_Group_Id;
                                    cmbUom.SelectedValue = getProductName[0].Prod_Primary_UOM_Id;
                                    cmbAltUom.SelectedValue = getProductName[0].Prod_Alternative_UOM_Id;
                                    txtHSNCode.Text = getProductName[0].Prod_HSN_Code;
                                    cmbTaxClass.SelectedValue = getProductName[0].Prod_Tax_Class;

                                }

                            }
                        }
                        else
                        {
                            if ((from u in db.Products where u.Prod_Name == txtProductName.Text && u.Company_ID == logIn.company select u).Count() > 0)
                            {
                                MessageBox.Show("Product Name Cannot Be Duplicate");
                                txtProductName.Focus();
                                return;
                            }
                        }
                    }
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

        private void txtField3_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbUom_Leave(object sender, EventArgs e)
        {
            try
            {
                if (cmbUom.Text != "")
                {
                    
                    if ((from u in db.UoM_Masters where u.UOM_ID == Convert.ToInt32(cmbUom.SelectedValue) select u).Count() > 0)
                    {
                        
                    }
                    else
                    {
                        MessageBox.Show("Invalid UOM Selected");
                        cmbUom.Focus();
                        return;
                    }    
                    
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

        private void cmbAltUom_Leave(object sender, EventArgs e)
        {
            try
            {
                if (cmbAltUom.Text != "")
                {

                    if ((from u in db.UoM_Masters where u.UOM_ID == Convert.ToInt32(cmbAltUom.SelectedValue) select u).Count() > 0)
                    {

                    }
                    else
                    {
                        MessageBox.Show("Invalid UOM Selected");
                        cmbAltUom.Focus();
                        return;
                    }

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

        private void txtProductName_Enter(object sender, EventArgs e)
        {
            try
            {
                txtProductName.AutoCompleteCustomSource = null;
                //if (cmbProdType.Text == "Project Items")
                //{
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    GetProdName(DataColl);
                txtProductName.AutoCompleteCustomSource = DataColl;
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbProdGroup_LocationChanged(object sender, EventArgs e)
        {

        }
    }
}
