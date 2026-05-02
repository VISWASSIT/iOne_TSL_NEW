using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Data.SqlClient;


namespace ioneNet.OrderManagement.Masters
{
    public partial class frmAddNewCustomer : Form
    {

        DataClasses1DataContext db = new DataClasses1DataContext();
        int Creation_Company = logIn.company;
        int CustID = frmCustomers.custId;
    
        public frmAddNewCustomer()
        {
            InitializeComponent();
        }

        #region Events
        private void btnReset_Click(object sender, EventArgs e)
        {
            clear1();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCustomerName.Text == string.Empty)
                {
                    MessageBox.Show("Customer Name Should Not Be Empty", "Customer Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCustomerName.Focus();
                    return;
                }
                else if (txtContMobile.Text == string.Empty)
                {
                    MessageBox.Show("Mobile No Should Not Be Empty");
                    txtContMobile.Focus();
                    return;
                }
                else if (txtGSTNo.Text == string.Empty)
                {
                    MessageBox.Show("GST No Should Not Be Empty");
                    txtGSTNo.Focus();
                    return;
                }
                else if (cmbStatus.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Status");
                    cmbStatus.Focus();
                    return;
                }
                //else if (txtAddress1.Text == string.Empty)
                //{
                //    MessageBox.Show("Address 1 Should Not Be Empty", "Customer Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    txtAddress1.Focus();
                //    return;
                //}

                //else if (txtstate.Text == "")
                //{
                //    MessageBox.Show("State Should Not Be Empty", "Customer Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    txtstate.Focus();
                //    return;
                //}
                //else if (txtPincode.Text == "")
                //{
                //    MessageBox.Show("Pincode No Should Not Be Empty ", "Customer Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    txtPincode.Focus();
                //    return;
                //}

                //else if (txtEmail.Text == "")
                //{
                //    MessageBox.Show("Email Id Should Not Be Empty ", "Customer Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    txtEmail.Focus();
                //    return;
                //}
                //else if (txtContactPerson.Text == "")
                //{
                //    MessageBox.Show("Contact Person Should Not Be Empty ", "Customer Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    txtContactPerson.Focus();
                //    return;
                //}
                else
                {
                    saveAccountInfo();
                    Save();                    
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Customer Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

        private void txtPincode_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtPincode.Text != "")
            {
                double parsedValue;
                if (!double.TryParse(txtPincode.Text, out parsedValue))
                {
                    txtPincode.Text = "";
                    txtPincode.Focus();
                    MessageBox.Show("Please Enter Numbers Only");
                }
            }
        }

        private void txtPhoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtPhoneNo.Text != "")
            {
                double parsedValue;
                if (!double.TryParse(txtPhoneNo.Text, out parsedValue))
                {
                    txtPhoneNo.Text = "";
                    txtPhoneNo.Focus();
                    MessageBox.Show("Please Enter Numbers Only");
                }
            }
        }
        
        public void autogen()
        {
            var result = db.Sp_autoincrement_Supplier_Master(Creation_Company);
            txtCustomerId.Text = result.FirstOrDefault().Supplier_Id;
        }
        public static string  a;
        private void frmAddNewCustomer_Load(object sender, EventArgs e)
        {
            linkCreatedBy.Text = logIn.username + '-'+ Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            linkModifiedBy.Text = logIn.username + '-'+ Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

            //lnkus2.Text = frmLogin.UserName;
            //lbldt2.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //groupBox3.Visible = false;
            string a = frmCustomers.var;
            //bindSalesExecutives();
            bindCitys();

            if (a == "1")
            {
                autogen();
            }
            else if (a != "1")
            {
                bindedit();
            }

            // txtCustomerName.Focus();
        }
        #endregion

        #region Methods


        // // Clear All Fields
        public void clear1()
        {
            foreach (Control x in this.Controls)
            {
                foreach (Control d in groupBox1.Controls)
                {
                    if (d is TextBox)
                        (d as TextBox).Clear();
                    if (d is ComboBox)
                        (d as ComboBox).SelectedIndex = -1;
                    if (d is CheckBox)
                        (d as CheckBox).Checked = false;
                }
                //lblTotalDiscount.Text = "";
                cmbCity.Text = "";
            }
           
        }


        // // // Saving Method
        public void Save()
        {
            try
            {
                
                if (txtId.Text!="")
                {
                    //if (frmGate.Modify.Contains(this.Text))
                    //{
                        int CustID = Convert.ToInt32(txtId.Text.ToString());
                        var ci = db.Supplier_informations.Where(w => w.ID == CustID && w.Company_ID == Creation_Company).FirstOrDefault();
                        {
                            ci.Supplier_Id = txtCustomerId.Text;
                            ci.Supplier_Name = (txtCustomerName.Text == "") ? "" : (txtCustomerName.Text);
                            ci.Supplier_Alias_Name = (txtCustomerAliasName.Text == "") ? "" : (txtCustomerAliasName.Text);
                            ci.Supplier_Name = (cmbCustType.Text == "") ? "" : (cmbCustType.Text);
                            ci.Account_Group = Convert.ToInt32(cmbAccountGroup.SelectedValue);
                            ci.Group_Ledger = Convert.ToInt32(cmbGroupLedger.SelectedValue);
                            ci.Address_1 = (txtAddress1.Text == "") ? "" : txtAddress1.Text;
                            ci.Address_2 = (txtAddress2.Text == "") ? "" : txtAddress2.Text;
                            ci.City = (cmbCity.Text == "") ? "" : cmbCity.Text;
                            ci.State = (txtstate.Text == "") ? "" : txtstate.Text;
                            ci.StateCode = (txtStateCode.Text == "") ? "" : txtStateCode.Text;
                            ci.Pincode = (txtPincode.Text == "") ? "" : txtPincode.Text;
                            ci.Phone_No = (txtPhoneNo.Text == "") ? "" : txtPhoneNo.Text;
                            ci.Fax_No = (txtFaxNo.Text == "") ? "" : txtFaxNo.Text;
                            ci.Email_Id = (txtEmail.Text == "") ? "" : txtEmail.Text;
                            ci.Web_URL = (txtWeburl.Text == "") ? "" : txtWeburl.Text;
                            ci.Contact_Person = (txtContactPerson.Text == "") ? "" : txtContactPerson.Text;
                            ci.Designation = (txtDesignation.Text == "") ? "" : txtDesignation.Text;
                            ci.Contact_Mobile = (txtContMobile.Text == "") ? "" : txtContMobile.Text;
                            ci.GSTIN_NO = (txtGSTNo.Text == "") ? "" : txtGSTNo.Text;
                            ci.PAN_No = (txtPanno.Text == "") ? "" : txtPanno.Text;
                            ci.Status = Convert.ToInt32(cmbStatus.SelectedValue);
                            ci.Company_ID = Creation_Company;
                            ci.Modified_By = logIn.username;
                            //ci.Modified_Date = Convert.ToDateTime(DateTime.Now.ToString());
                            db.SubmitChanges();
                            MessageBox.Show("Record Upadated Successfully");
                            clear1();
                    }
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Sorry! You Do not have privileges to Modify Suppliers");
                    //}
                }
                else
                {
                    //if (frmGate.Create_menu.Contains(this.Text))
                    //{
                        Supplier_information ci1 = new Supplier_information();
                        ci1.Company_ID = Creation_Company;
                        var result = db.Sp_autoincrement_Customer_Master(Creation_Company);
                        txtCustomerId.Text = result.FirstOrDefault().Customer_Id;
                        ci1.Supplier_Id = txtCustomerId.Text;
                        ci1.Supplier_Name = (txtCustomerName.Text == "") ? "" : (txtCustomerName.Text);
                        ci1.Supplier_Alias_Name = (txtCustomerAliasName.Text == "") ? "" : (txtCustomerAliasName.Text);
                        ci1.Supplier_Type = (cmbCustType.Text == "") ? "" : (cmbCustType.Text);
                        ci1.Account_Group = Convert.ToInt32(cmbAccountGroup.SelectedValue);
                        ci1.Group_Ledger = Convert.ToInt32(cmbGroupLedger.SelectedValue);

                        ci1.Address_1 = (txtAddress1.Text == "") ? "" : txtAddress1.Text;
                        ci1.Address_2 = (txtAddress2.Text == "") ? "" : txtAddress2.Text;
                        ci1.City = (cmbCity.Text == "") ? "" : cmbCity.Text;
                        ci1.State = (txtstate.Text == "") ? "" : txtstate.Text;
                        ci1.Pincode = (txtPincode.Text == "") ? "" : txtPincode.Text;
                        ci1.Phone_No = (txtPhoneNo.Text == "") ? "" : txtPhoneNo.Text;
                        ci1.Fax_No = (txtFaxNo.Text == "") ? "" : txtFaxNo.Text;
                        ci1.Email_Id = (txtEmail.Text == "") ? "" : txtEmail.Text;
                        ci1.Web_URL = (txtWeburl.Text == "") ? "" : txtWeburl.Text;
                        ci1.Contact_Person = (txtContactPerson.Text == "") ? "" : txtContactPerson.Text;
                        ci1.Designation = (txtDesignation.Text == "") ? "" : txtDesignation.Text;
                        ci1.Contact_Mobile = (txtContMobile.Text == "") ? "" : txtContMobile.Text;
                        ci1.GSTIN_NO = (txtGSTNo.Text == "") ? "" : txtGSTNo.Text;
                        //ci1.CST_No = (txtCstNo.Text == "") ? "" : txtCstNo.Text;
                        ci1.PAN_No = (txtPanno.Text == "") ? "" : txtPanno.Text;
                    //ci1.Payment_Terms = (cmbPaymenyTerms.Text == "") ? "" : cmbPaymenyTerms.Text;
                    //ci1.Credit_Limit = (txtCreditLimt.Text == "") ? Convert.ToDecimal(00) : Convert.ToDecimal(txtCreditLimt.Text);
                    //ci1.gs = (txtGSTNo.Text == "") ? "" : txtGSTNo.Text;
                    //ci1.Salesmen_Code = (cmbSalwExecutive.Text == "") ? "" : cmbSalwExecutive.SelectedValue.ToString();
                    //ci1.Discount_1 = (txtDiscuont1.Text == "") ? 0 : Convert.ToDecimal(txtDiscuont1.Text);
                    //ci1.Plus_Discount = (txtPlusDiscount.Text == "") ? 0 : Convert.ToDecimal(txtPlusDiscount.Text);
                    //ci1.Total_Discount = (lblTotalDiscount.Text == "") ? 0 : Convert.ToDecimal(lblTotalDiscount.Text);
                        ci1.Status = Convert.ToInt32(cmbStatus.SelectedValue);
                        ci1.Company_ID = Creation_Company;
                        ci1.Created_By = logIn.username;
                        //ci1.Created_Date = Convert.ToDateTime(DateTime.Now.ToString());
                        ci1.Modified_By = logIn.username;
                        //ci1.Modified_Date = Convert.ToDateTime(DateTime.Now.ToString());
                        db.Supplier_informations.InsertOnSubmit(ci1);
                        db.SubmitChanges();
                        MessageBox.Show("Record Saved Successfully");
                         clear1();
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Sorry! You Do not have privileges to Save Customers");
                    //} 
                }
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            {

        }
    }
            finally
            {
            }
        }

        public void saveAccountInfo()
        {
            if(txtAccountID.Text!="")
            { 
            
                db.Transaction = null;
                try
                {

                    //if (AppCode.GlobalAccess.Edit == "Yes")
                    //{
                    // var created = false;
                    var am = db.AccountMasters.Where(w => w.id == Convert.ToInt32(txtAccountID.Text) && w.Company_ID == logIn.company).FirstOrDefault();
                    {

                        if (null != db.Connection)
                        {
                            db.Connection.Close();
                        }
                        System.Data.Common.DbTransaction transaction;
                        db.Connection.Open();
                        transaction = db.Connection.BeginTransaction();
                        db.Transaction = transaction;

                        //AccountMaster am = new AccountMaster();
                        am.AccCode = txtCustomerId.Text;
                        am.AccGroup = cmbAccountGroup.Text;
                        am.AccName = txtCustomerName.Text;
                        am.AccGroup_ID = Convert.ToInt32(cmbAccountGroup.SelectedValue);
                        am.Group_Head_ID = Convert.ToInt32(cmbGroupLedger.SelectedValue);
                        if (!string.IsNullOrEmpty(cmbGroupLedger.Text))
                        {
                            am.AccHead = (cmbGroupLedger.Text == "") ? "" : (cmbGroupLedger.Text);
                        }
                        else
                        {
                            am.AccHead = (txtCustomerName.Text == "") ? "" : (txtCustomerName.Text);
                        }
                        am.Created_By = logIn.username;
                        am.Modified_BY = logIn.username;
                        am.Company_ID = logIn.company;
                        //db.AccountMasters.InsertOnSubmit(am);
                        db.SubmitChanges();
                        db.Transaction = transaction;
                        transaction.Commit();
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

            }
            else
            {
                try
                {
                    db.Transaction = null;
                    //if (AppCode.GlobalAccess.Add == "Yes")
                    //{
                    if (null != db.Connection)
                    {
                        db.Connection.Close();
                    }
                    db.Connection.Open();
                    //transaction = db.Connection.BeginTransaction();
                    //db.Transaction = transaction;
                    AccountMaster am = new AccountMaster();
                    am.AccCode = txtCustomerId.Text;
                    am.AccGroup = cmbAccountGroup.Text;
                    am.AccName = txtCustomerName.Text;
                    am.AccGroup_ID = Convert.ToInt32(cmbAccountGroup.SelectedValue);
                    am.Group_Head_ID = Convert.ToInt32(cmbGroupLedger.SelectedValue);
                    am.Created_By = logIn.username;
                    am.Modified_BY = logIn.username;
                    am.Company_ID = logIn.company;

                    if (!string.IsNullOrEmpty(cmbGroupLedger.Text))
                    {
                        am.AccHead = (cmbGroupLedger.Text == "") ? "" : (cmbGroupLedger.Text);
                    }
                    else
                    {
                        am.AccHead = (txtCustomerName.Text == "") ? "" : (txtCustomerName.Text);
                    }

                    db.AccountMasters.InsertOnSubmit(am);
                    db.SubmitChanges();
                    //db.Transaction = transaction;
                    //transaction.Commit();
                    //MessageBox.Show("Record Saved Successfully,This Record code is '" + am.AccCode + "'");
                    //Clear();
                    //}
                    //else
                    //{
                    //    MessageBox.Show("You dont Have Privileges", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    Clear();
                    //}
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

            }
        }

        #endregion

        public void bindedit()
        {
            //int CustID = OrderManagement.Masters.frmCustomers.custId;
            txtId.Text = CustID.ToString();
            //if (txtCustomerId.Text != "" && Creation_Company = null)
            //{

            var d = (from po in db.Supplier_informations
                         // join s in db.Sales_Men_Informations on po.Salesmen_Code equals s.Salesmen_Code
                     where
                     po.ID == CustID && po.Company_ID == logIn.company
                     select new
                     {
                         po.ID,
                         po.Supplier_Id,
                         po.Supplier_Name,
                         po.Supplier_Alias_Name,
                         po.Supplier_Type,
                         po.Account_Group,
                         po.Group_Ledger,
                         po.Address_1,
                         po.Address_2,
                         po.City,
                         po.State,
                         po.Pincode,
                         po.Fax_No,
                         po.Email_Id,
                         po.Web_URL,
                         po.Contact_Person,
                         po.Designation,
                         po.Contact_Mobile,
                         po.Phone_No,
                         po.GSTIN_NO,
                         //po.CST_No,
                         po.PAN_No,
                         //po.Payment_Terms,
                         //po.Credit_Limit,
                         //po.Tax_Class,
                         //po.Salesmen_Code,
                         // po.Sales_Executive_Name,
                         po.Status,
                         //po.Field_1,
                         //po.Discount_1,
                         //po.Plus_Discount,
                         //po.Total_Discount,
                         //po.Created_Date,
                         po.Created_By,
                         po.Modified_By,
                         //po.Modified_Date,
                         po.StateCode,

                     }).ToList();
            if (d.Count > 0)
            {
                txtId.Text = d[0].ID.ToString();
                txtCustomerId.Text = d[0].Supplier_Id.ToString();
                txtCustomerName.Text = d[0].Supplier_Name;
                txtCustomerAliasName.Text = d[0].Supplier_Alias_Name;
                cmbCustType.Text = d[0].Supplier_Type;
                cmbAccountGroup.SelectedValue = d[0].Account_Group;
                cmbGroupLedger.SelectedValue = d[0].Group_Ledger;
                txtAddress1.Text = d[0].Address_1;
                txtAddress2.Text = d[0].Address_2;
                cmbCity.Text = d[0].City;
                txtstate.Text = d[0].State;
                txtPincode.Text = d[0].Pincode;
                txtPhoneNo.Text = d[0].Phone_No;
                txtFaxNo.Text = d[0].Fax_No;
                txtEmail.Text = d[0].Email_Id;
                txtWeburl.Text = d[0].Web_URL;
                txtContactPerson.Text = d[0].Contact_Person;
                txtDesignation.Text = d[0].Designation;
                txtContMobile.Text = d[0].Contact_Mobile;
                cmbStatus.SelectedValue = d[0].Status;
                txtGSTNo.Text = d[0].GSTIN_NO;
                txtStateCode.Text = d[0].StateCode;
                txtPanno.Text = d[0].PAN_No;


                linkCreatedBy.Text = d[0].Created_By;
                linkCreatedBy.Text = d[0].Created_By + string.Format("{0:dd/MM/yyyy HH:mm tt}");
                //lnkus2.Text = d[0].Modified_By;
                linkCreatedBy.Text = d[0].Modified_By + String.Format("{0:dd/MM/yyyy HH:mm tt}");

            }
            else
            {
            }
            var d1 = (from po in db.AccountMasters
                          // join s in db.Sales_Men_Informations on po.Salesmen_Code equals s.Salesmen_Code
                      where
                      po.AccCode == txtCustomerId.Text && po.Company_ID == logIn.company && po.AccGroup_ID == Convert.ToInt32(cmbAccountGroup.SelectedValue)
                      select new
                      {
                          po.id
                      }).ToList();
            if (d1.Count > 0)
            {
                txtAccountID.Text = d1[0].id.ToString();
            }
        }

        private void btnCustNew_Click(object sender, EventArgs e)
        {
        }

        public void bindCitys()
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

            //Bind Accounts
            var GrpMast = (from m in db.AccountGroups where m.Company_ID == logIn.company select new { m.ID, m.GroupName }).Distinct().ToList();
            if (GrpMast.Count > 0)
            {
                cmbAccountGroup.DataSource = GrpMast;
                cmbAccountGroup.ValueMember = "ID";
                cmbAccountGroup.DisplayMember = "GroupName";
            }
            //Bind Accounts
            var AccMast = (from m in db.AccountMasters where m.Company_ID == logIn.company select new { m.id, m.AccName }).Distinct().ToList();
            if (AccMast.Count > 0)
            {
                cmbGroupLedger.DataSource = AccMast;
                cmbGroupLedger.ValueMember = "id";
                cmbGroupLedger.DisplayMember = "AccName";
            }
            //Status
            var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Master" select new { m.ID, m.Descr }).Distinct().ToList();
            if (pStatus.Count > 0)
            {
                cmbStatus.DataSource = pStatus;
                cmbStatus.ValueMember = "ID";
                cmbStatus.DisplayMember = "Descr";
            }
        }

        private void btnNewCity_Click(object sender, EventArgs e)
        {
            //Masters. compFrm = new Administrator.frmCityMaster();
            ioneNet.Masters.frmCityMaster compFrm = new ioneNet.Masters.frmCityMaster();
            compFrm.ShowDialog();
            //var sa = (from a in db.City_Masters select new { a.City_Name }).ToList();
            //if (sa.Count > 0)
            //{
            //    cmbCity.DataSource = sa;
            //    cmbCity.DisplayMember = "City_Name";
            //    cmbCity.ValueMember = "City_Name";
            //    if (cmbCity.Items.Count > 0)
            //    {
            //        cmbCity.SelectedIndex = -1;
            //    }
            //    else
            //    {
            //        cmbCity.SelectedIndex = -1;
            //    }
            //}
            //cmbCity.Focus();
        }

        private void txtCustomerAliasName_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtCustomerAliasName.Text !="")
                {
                    if ((from u in db.Supplier_informations where u.Supplier_Name == txtCustomerName.Text && u.ID != Convert.ToInt32(txtId.Text.ToString()) select u).Count() > 0)
                    {
                        MessageBox.Show("Duplicate Customer Name!, Cannot Proceed");
                        txtCustomerName.Text = "";
                        txtCustomerName.Focus();
                        return;
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

        public static decimal Discount;
        // // // // Additional Percentage Adding And Calculation Related Details
       

        private void cmbCity_Leave(object sender, EventArgs e)
        {
            try
            { 
                var State = (from c in db.City_Masters
                             where c.City_Name == cmbCity.Text
                             select  new { c.State_Name, c.State_Code }).ToList();
                if (State.Count > 0)
                {
                    txtstate.Text = State[0].State_Name;
                    txtStateCode.Text = State[0].State_Code;
                    txtPincode.Focus();
                }
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

       

  
        private void frmAddNewCustomer_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SendKeys.Send("{TAB}");
                }

                if (e.Control && e.KeyCode == Keys.R)
                    btnReset_Click(sender, e);


                if (e.Control && e.KeyCode == Keys.S)
                    btnSubmit_Click(sender, e);


                if (e.Alt && e.KeyCode == Keys.F4)
                    btnClose_Click(sender, e);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }


        }

        private void txtContMobile_Leave(object sender, EventArgs e)
        {
         //int len=   txtContMobile.TextLength;
         //   if(len>9)
         //   {

         //   }
         //   else
         //   {
         //       MessageBox.Show("Please Check Your Mobile number it is bellow 10 Digits...");
         //       txtContMobile.Focus();
         //       return;
         //   }
        }

        private void txtPhoneNo_Leave(object sender, EventArgs e)
        {
            //int len = txtPhoneNo.TextLength;
            //if (len > 9)
            //{

            //}
            //else
            //{
            //    MessageBox.Show("Please Check Your Phone number. It is bellow 10 Digits...");
            //    txtPhoneNo.Focus();
            //    return;
            //}
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Customer Data", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void cmbCity_Enter(object sender, EventArgs e)
        {
            bindCitys();
        }

        private void txtStateCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCustomerName_Leave(object sender, EventArgs e)
        {
            if (txtCustomerName.Text != "")
            {
                if (txtId.Text == "")
                {

                    if ((from u in db.Supplier_informations where u.Supplier_Name == txtCustomerName.Text select u).Count() > 0)
                    {
                        MessageBox.Show("Customer Name Cannot Be Duplicate", "Customer Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtCustomerName.Focus();
                    }
                }
            }
        }
    }
}
