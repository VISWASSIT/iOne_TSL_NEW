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
using Ione_DAL;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Net.Sockets;

namespace ioneNet.MaterialManagement
{
    public partial class SupplierMaster : Form
    {

        DataClasses1DataContext db = new DataClasses1DataContext();
        int Creation_Company = logIn.company;
       
        int CustID;
        int SuppID;

        public SupplierMaster()
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
                    MessageBox.Show("Supplier Name Should Not Be Empty", "Party Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                else if (cmbCity.Text == string.Empty)
                {
                    MessageBox.Show("Please Select City Name");
                    cmbCity.Focus();
                    return;
                }
                else if (cmbPartyType.Text == string.Empty)
                {
                    MessageBox.Show("Party Category Should Not Be Empty", "Party Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbPartyType.Focus();
                    return;
                }

                else if (cmbCustType.Text == "")
                {
                    MessageBox.Show("Party Type Should Not Be Empty", "Party Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbCustType.Focus();
                    return;
                }
                else if (txtPincode.Text == "")
                {
                    MessageBox.Show("Pincode No Should Not Be Empty ", "Party Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPincode.Focus();
                    return;
                }

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
                    if (ioneNet.frmMenuBoard.finModule == true)
                    {

                        //saveAccountInfo();
                    }
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
            txtCustomerID.Text = result.FirstOrDefault().Supplier_Id;
        }
        //public static string  a;
        private void frmAddNewCustomer_Load(object sender, EventArgs e)
        {
            linkCreatedBy.Text = logIn.username + '-'+ Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            linkModifiedBy.Text = logIn.username + '-'+ Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

            //lnkus2.Text = frmLogin.UserName;
            //lbldt2.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //groupBox3.Visible = false;
            bindCitys();
            BindMasters();
            string a = "";
            if (ListOfSupplier.var == null)
            {
                a = OrderManagement.Masters.frmCustomers.var;
                CustID = OrderManagement.Masters.frmCustomers.custId;
                cmbPartyType.SelectedValue = 27;
                cmbSegment.Visible = true;
                label37.Visible = true;
                label17.Visible = true;
                label31.Visible = true;

                cmbAccountManager.Visible = true;
                cmbArea.Visible = true;
                OrderManagement.Masters.frmCustomers.var = null;
            }
            else
            {
                a = ListOfSupplier.var;
                SuppID = MaterialManagement.ListOfSupplier.SuppId;
                cmbPartyType.SelectedValue = 28;
                cmbSegment.Visible = false;
                cmbAccountManager.Visible = false;
                cmbArea.Visible = false;

                label37.Visible = false;
                label17.Visible = false;
                label31.Visible = false;
                ListOfSupplier.var = null;
            }
            //bindSalesExecutives();
            
            if (a == "1")
            {
               autogen();
            }
            else if (a == "0")
            {
                bindedit();
            }
            else if (a == "2")
            {
                bindedit();
                btnClear.Enabled = false;
                btnSubmit.Enabled = false;
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
               
                //int CustID = Convert.ToInt32(txtId.Text.ToString());
                //if ((from u in db.Supplier_informations where u.ID == CustID && u.Company_ID == Creation_Company select u).Count() > 0)
                //{
                    //if (frmGate.Modify.Contains(this.Text))
                    //{
                    if (txtId.Text != "")
                    {
                        int CustID = Convert.ToInt32(txtId.Text.ToString());
                        var ci = db.Supplier_informations.Where(w => w.ID == CustID && w.Company_ID == Creation_Company).FirstOrDefault();
                            {
                                ci.Supplier_Id = txtCustomerID.Text;
                                ci.Supplier_Name = (txtCustomerName.Text == "") ? "" : (txtCustomerName.Text);
                                ci.Supplier_Alias_Name = (txtCustomerAliasName.Text == "") ? "" : (txtCustomerAliasName.Text);
                                ci.Supplier_Type = (cmbCustType.Text == "") ? "" : (cmbCustType.Text);
                                ci.Account_Group =Convert.ToInt32(cmbAccountGroup.SelectedValue);
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
                                ci.BankName = (txtbankName.Text == "") ? "" : txtbankName.Text;
                                //ci.Customer_Plant_ID = (txtCustPlantID.Text == "") ? "" : txtCustPlantID.Text;

                            ci.BranchName = (txtMSMENo.Text == "") ? "" : txtMSMENo.Text; // Saving MSME Number
                            ci.AccNo = (cmbMSMEType.Text == "") ? "" : cmbMSMEType.Text; //Saving MSME Type
                            //ci.IFSCCode = (txtIFSCcode.Text == "") ? "" : txtIFSCcode.Text;
                            ci.Supplier_Category = Convert.ToInt32(cmbPartyType.SelectedValue);
                            ci.Sale_Executive = Convert.ToInt32(cmbAccountManager.SelectedValue);
                        //ci.Price_List = (txtPriceList.Text == "") ? "" : txtPriceList.Text;
                            ci.Credit_Limit = Convert.ToDecimal(txtCreditLimit.Text);
                            ci.Apply_Credit_Limit = chkCreditApply.Checked;
                            ci.Area_Region = Convert.ToInt32(cmbArea.SelectedValue);
                            ci.Customer_Segment = Convert.ToInt32(cmbSegment.SelectedValue);
                            ci.Product_services = (cmbProducts.Text == "") ? "" : cmbProducts.Text;
                            ci.Status = Convert.ToInt32(cmbStatus.SelectedValue);
                            ci.Company_ID = Creation_Company;
                             ci.Modified_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                           
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
                        var result = db.Sp_autoincrement_Supplier_Master(Creation_Company);
                        txtCustomerID.Text = result.FirstOrDefault().Supplier_Id;
                        ci1.Supplier_Id = txtCustomerID.Text;
                        ci1.Supplier_Name = (txtCustomerName.Text == "") ? "" : (txtCustomerName.Text);
                        ci1.Supplier_Alias_Name = (txtCustomerAliasName.Text == "") ? "" : (txtCustomerAliasName.Text);
                        ci1.Supplier_Type = (cmbCustType.Text == "") ? "" : (cmbCustType.Text);
                        ci1.Account_Group = Convert.ToInt32(cmbAccountGroup.SelectedValue);
                        ci1.Group_Ledger = Convert.ToInt32(cmbGroupLedger.SelectedValue);
                        ci1.Address_1 = (txtAddress1.Text == "") ? "" : txtAddress1.Text;
                        ci1.Address_2 = (txtAddress2.Text == "") ? "" : txtAddress2.Text;
                        ci1.City = (cmbCity.Text == "") ? "" : cmbCity.Text;
                        ci1.State = (txtstate.Text == "") ? "" : txtstate.Text;
                        ci1.StateCode = (txtStateCode.Text == "") ? "" : txtStateCode.Text;
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
                        ci1.Supplier_Category = Convert.ToInt32(cmbPartyType.SelectedValue);
                        ci1.Sale_Executive = Convert.ToInt32(cmbAccountManager.SelectedValue);
                        //ci.Price_List = (txtPriceList.Text == "") ? "" : txtPriceList.Text;
                        ci1.Credit_Limit = Convert.ToDecimal(txtCreditLimit.Text);
                        ci1.Apply_Credit_Limit = chkCreditApply.Checked;
                        ci1.Area_Region = Convert.ToInt32(cmbArea.SelectedValue);
                        ci1.Customer_Segment = Convert.ToInt32(cmbSegment.SelectedValue);
                        ci1.Product_services = (cmbProducts.Text == "") ? "" : cmbProducts.Text;
                         ci1.BankName = (txtbankName.Text == "") ? "" : txtbankName.Text;
                        //ci1.Customer_Plant_ID = (txtCustPlantID.Text == "") ? "" : txtCustPlantID.Text;
                        ci1.BranchName = (txtMSMENo.Text == "") ? "" : txtMSMENo.Text; // Saving MSME Number
                        ci1.AccNo = (cmbMSMEType.Text == "") ? "" : cmbMSMEType.Text; //Saving MSME Type
                    //ci1.IFSCCode = (txtIFSCcode.Text == "") ? "" : txtIFSCcode.Text;
                    ci1.Status = Convert.ToInt32(cmbStatus.SelectedValue);
                        ci1.Company_ID = Creation_Company;
                        ci1.Created_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                    //ci1.Created_Date = Convert.ToDateTime(DateTime.Now.ToString());
                    ci1.Modified_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                    //ci1.Modified_Date = Convert.ToDateTime(DateTime.Now.ToString());
                    db.Supplier_informations.InsertOnSubmit(ci1);
                        db.SubmitChanges();
                        MessageBox.Show("Record Saved Successfully");
                    Clear();
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
            if (txtAccountID.Text != "")
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
                        am.AccCode = txtCustomerID.Text;
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
                        am.Created_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt"); ;
                        am.Modified_BY = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt"); ;
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
                    am.AccCode = txtCustomerID.Text;
                    am.AccGroup = cmbAccountGroup.Text;
                    am.AccName = txtCustomerName.Text;
                    am.AccGroup_ID = Convert.ToInt32(cmbAccountGroup.SelectedValue);
                    am.Group_Head_ID = Convert.ToInt32(cmbGroupLedger.SelectedValue);
                    am.Created_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt"); 
                    am.Modified_BY = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
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
            try
            {
                //int CustID = OrderManagement.Masters.frmCustomers.custId;
                if (CustID != 0)
                {
                    txtId.Text = CustID.ToString();
                }
                else
                {
                    txtId.Text = SuppID.ToString();
                }
                //if (txtCustomerId.Text != "" && Creation_Company = null)
                //{

                var d = (from po in db.Supplier_informations
                             // join s in db.Sales_Men_Informations on po.Salesmen_Code equals s.Salesmen_Code
                         where
                         po.ID == Convert.ToInt32(txtId.Text) && po.Company_ID == logIn.company
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
                             po.BankName,
                             po.BranchName,
                             po.AccNo,
                             po.IFSCCode,
                             //po.Created_Date,
                             po.Created_By,
                             po.Modified_By,
                             //po.Modified_Date,
                             po.StateCode,
                             po.Supplier_Category,
                             po.Sale_Executive,
                             po.Area_Region,
                             po.Credit_Limit,
                             po.Apply_Credit_Limit,
                             po.Product_services,
                             po.Price_List,
                             po.Customer_Segment

                         }).ToList();
                if (d.Count > 0)
                {
                    txtId.Text = d[0].ID.ToString();
                    txtCustomerID.Text = d[0].Supplier_Id;
                    txtCustomerName.Text = d[0].Supplier_Name;
                    txtCustomerAliasName.Text = d[0].Supplier_Alias_Name;
                    cmbCustType.Text = d[0].Supplier_Type;
                    if (d[0].Account_Group.ToString() != "")
                    {
                        cmbAccountGroup.SelectedValue = d[0].Account_Group;
                    }
                    if (d[0].Group_Ledger.ToString() != "")
                    {
                        cmbGroupLedger.SelectedValue = d[0].Group_Ledger;
                    }
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
                    txtGSTNo.Text = d[0].GSTIN_NO;
                    txtStateCode.Text = d[0].StateCode;
                    txtPanno.Text = d[0].PAN_No;
                    txtbankName.Text = d[0].BankName;
                    txtMSMENo.Text = d[0].BranchName;
                    cmbMSMEType.Text = d[0].AccNo;
                    //txtIFSCcode.Text = d[0].IFSCCode;
                    //txtCustPlantID.Text = d[0].Customer_Plant_ID;
                    if (d[0].Supplier_Category.ToString() != "")
                    {
                        cmbPartyType.SelectedValue = d[0].Supplier_Category;
                    }

                    if (d[0].Sale_Executive.ToString() != "")
                    {
                        cmbAccountManager.SelectedValue = d[0].Sale_Executive;
                    }

                    if (d[0].Area_Region.ToString() != "")
                    {
                        cmbArea.SelectedValue = d[0].Area_Region;
                    }

                    if (d[0].Customer_Segment.ToString() != "")
                    {
                        cmbSegment.SelectedValue = d[0].Customer_Segment;
                    }

                    txtCreditLimit.Text = d[0].Credit_Limit.ToString();
                    if (d[0].Apply_Credit_Limit == true)
                    {
                        chkCreditApply.Checked = true;
                    }
                    else
                    {
                        chkCreditApply.Checked = false;
                    }
                    cmbProducts.Text = d[0].Product_services;
                    cmbStatus.SelectedValue = d[0].Status;
                    linkCreatedBy.Text = d[0].Created_By;
                    linkModifiedBy.Text = d[0].Modified_By;
                    

                }
                else
                {
                }
                var d1 = (from po in db.AccountMasters
                              // join s in db.Sales_Men_Informations on po.Salesmen_Code equals s.Salesmen_Code
                          where
                          po.AccCode == txtCustomerID.Text && po.Company_ID == logIn.company 
                          select new
                          {
                              po.id
                          }).ToList();
                if (d1.Count > 0)
                {
                    txtAccountID.Text = d1[0].id.ToString();
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

        private void btnCustNew_Click(object sender, EventArgs e)
        {
        }
        public void BindMasters()
        {
            //Bind Accounts
            //var GrpMast = (from m in db.AccountGroups where m.Company_ID == logIn.company select new { m.ID, m.GroupName }).Distinct().ToList();
            //if (GrpMast.Count > 0)
            //{
            //    cmbAccountGroup.DataSource = GrpMast;
            //    cmbAccountGroup.ValueMember = "ID";
            //    cmbAccountGroup.DisplayMember = "GroupName";
            //    cmbAccountGroup.SelectedIndex = -1;
            //}
            ////Bind Accounts
            //var AccMast = (from m in db.AccountMasters where m.Company_ID == logIn.company select new { m.id, m.AccName }).Distinct().ToList();
            //if (AccMast.Count > 0)
            //{
            //    cmbGroupLedger.DataSource = AccMast;
            //    cmbGroupLedger.ValueMember = "id";
            //    cmbGroupLedger.DisplayMember = "AccName";
            //    cmbGroupLedger.SelectedIndex = -1;
            //}
            //Status
            var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Master" select new { m.ID, m.Descr }).Distinct().ToList();
            if (pStatus.Count > 0)
            {
                cmbStatus.DataSource = pStatus;
                cmbStatus.ValueMember = "ID";
                cmbStatus.DisplayMember = "Descr";
            }
            //Party Types
            var pType = (from m in db.Attributes_Datas where m.Head_Name == "Party Type" select new { m.ID, m.Descr }).Distinct().ToList();
            if (pType.Count > 0)
            {
                cmbPartyType.DataSource = pType;
                cmbPartyType.ValueMember = "ID";
                cmbPartyType.DisplayMember = "Descr";
                cmbPartyType.SelectedIndex = -1;
            }
            //Area / Region
            var sarea = (from m in db.Attributes_Datas where m.Head_Name == "Sales Office" where m.Company_ID == logIn.company select new { m.ID, m.Descr }).Distinct().ToList();
            if (pType.Count > 0)
            {
                cmbArea.DataSource = sarea;
                cmbArea.ValueMember = "ID";
                cmbArea.DisplayMember = "Descr";
                cmbArea.SelectedIndex = -1;
            }
            //Customer Segment
            var ssegment = (from m in db.Attributes_Datas where m.Head_Name == "Customer-Segment" select new { m.ID, m.Descr }).Distinct().ToList();
            if (pType.Count > 0)
            {
                cmbSegment.DataSource = ssegment;
                cmbSegment.ValueMember = "ID";
                cmbSegment.DisplayMember = "Descr";
                cmbSegment.SelectedIndex = -1;
            }

            //Product Group
            var pGroup = (from m in db.Product_Groups where m.Company_ID == logIn.company select new { m.ID, m.Prod_Group_Name }).Distinct().ToList();
            if (pGroup.Count > 0)
            {
                cmbProducts.DataSource = pGroup;
                cmbProducts.ValueMember = "ID";
                cmbProducts.DisplayMember = "Prod_Group_Name";
            }

            //Product Group
            var SMen = (from m in db.Sales_Men_Informations where m.Company_ID == logIn.company select new { m.Id, m.Salesmen_Code }).Distinct().ToList();
            if (SMen.Count > 0)
            {
                cmbAccountManager.DataSource = SMen;
                cmbAccountManager.ValueMember = "Id";
                cmbAccountManager.DisplayMember = "Salesmen_Code";
                cmbAccountManager.SelectedIndex = -1;
            }
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
                if (txtCustomerName.Text !="")
                {
                    if ((from u in db.Supplier_informations where u.Supplier_Name == txtCustomerName.Text && u.ID != Convert.ToInt32(txtId.Text.ToString()) select u).Count() > 0)
                    {
                        MessageBox.Show("Duplicate Supplier Name!, Cannot Proceed");
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
                
                if (cmbCity.Text != "")
                {
                    int i = cmbCity.FindString(cmbCity.Text);
                    if (i >= 0)
                    {
                        var State = (from c in db.City_Masters
                                        where c.City_Name == cmbCity.Text
                                        select new { c.State_Name, c.State_Code }).ToList();
                        if (State.Count > 0)
                        {
                            txtstate.Text = State[0].State_Name;
                            txtStateCode.Text = State[0].State_Code;
                            txtPincode.Focus();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid City Name Selected");
                        cmbCity.Focus();
                    }
                }             
              
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
                Clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Party Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void Clear()
        {

            try
            {
                foreach (Control c in groupBox2.Controls.Cast<Control>().OrderBy(c => c.TabIndex))
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
                foreach (Control c in groupBox3.Controls.Cast<Control>().OrderBy(c => c.TabIndex))
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
                foreach (Control c in groupBox4.Controls.Cast<Control>().OrderBy(c => c.TabIndex))
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
                var result = db.Sp_autoincrement_Supplier_Master(Creation_Company);
                txtCustomerID.Text = result.FirstOrDefault().Supplier_Id;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Products", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void cmbCity_Enter(object sender, EventArgs e)
        {
            string cityname = cmbCity.Text;
            bindCitys();
            cmbCity.Text = cityname;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void label26_Click(object sender, EventArgs e)
        {

        }

        private void txtGSTNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbCustType_Leave(object sender, EventArgs e)
        {
            try
            {
                if (cmbCustType.Text!="")
                {
                    if(cmbCustType.Text == "Un Registered Dealer")
                    {
                        txtGSTNo.Text = "NA";
                    }
                    else
                    {
                        if (txtGSTNo.Text == "NA")
                        {
                            txtGSTNo.Text = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Party Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtCustomerName_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtCustomerName.Text != "")
            {
                txtCustomerAliasName.Text = txtCustomerName.Text;
                if (txtId.Text == "")
                {

                    if ((from u in db.Supplier_informations where u.Supplier_Name == txtCustomerName.Text && u.Company_ID==logIn.company select u).Count() > 0)
                    {
                        MessageBox.Show("Party Name Cannot Be Duplicate", "Party Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtCustomerName.Focus();
                    }
                }
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Party Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtGSTNo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtGSTNo.Text!="")
                {
                    if (cmbCustType.Text == "Registered Dealer")
                    {
                        IsValid(txtGSTNo.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Party Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtGSTNo.Focus();
            }
        }

        #region "Variables/Constants"
        private const string GSTIN_REGEX = "[0-9]{2}[a-zA-Z]{5}[0-9]{4}[a-zA-Z]{1}[1-9A-Za-z]{1}[Z]{1}[0-9a-zA-Z]{1}";
        private const string CHECKSUM_CHARS = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        #endregion

        #region "Subs/Functions"
        /// <summary>
        /// Method to check if a GSTIN Is valid. Checks the GSTIN format And thecheck digit Is valid for the passed input GSTIN
        /// </summary>
        /// <param name="GSTIN">GSTIN to Validate</param>
        /// 
        public static bool IsValid(string GSTIN)
        {
            bool isValidFormat = false;
            GSTIN = GSTIN.Trim();
            if (string.IsNullOrEmpty(GSTIN))
                throw new Exception("GSTIN is empty");
            else if (Regex.IsMatch(GSTIN, GSTIN_REGEX))
                isValidFormat = GSTIN[GSTIN.Length - 1].Equals(GenerateCheckSum(GSTIN.Substring(0, GSTIN.Length - 1)));
            else
                throw new Exception("Invalid GSTIN Number Entered");                
            return isValidFormat;
        }

        /// <summary>Generates and returns checksum digit for given GSTIN (without checksum digit)</summary>
        /// <param name="GSTIN">GSTIN without checksum digit to generate checksum digit</param>
        private static char GenerateCheckSum(string GSTIN)
        {
            int factor = 2;
            int sum = 0;
            int checkCodePoint = 0;
            char[] cpChars;
            char[] inputChars;

            if (string.IsNullOrEmpty(GSTIN))
                throw new Exception("GSTIN supplied for checkdigit calculation is null");
            cpChars = CHECKSUM_CHARS.ToCharArray();
            inputChars = GSTIN.ToUpper().ToCharArray();

            int Mod_ = cpChars.Length;
            for (int i = inputChars.Length - 1; i >= 0; i += -1)
            {
                int codePoint = -1;
                for (int j = 0; j <= cpChars.Length - 1; j++)
                {
                    if (cpChars[j] == inputChars[i])
                    {
                        codePoint = j;
                        break;
                    }
                }
                int digit = factor * codePoint;
                factor = factor == 2 ? 1 : 2;
                digit = (digit / Mod_) + (digit % Mod_);
                sum += digit;
            }
            checkCodePoint = (Mod_ - (sum % Mod_)) % Mod_;
            return cpChars[checkCodePoint];
        }
        #endregion

        private void txtPincode_Leave(object sender, EventArgs e)
        {
            try
            {
                //if (txtPincode.Text != "")
                //{
                //    int p = Convert.ToInt32(txtPincode.Text);

                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Party Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                txtGSTNo.Focus();
            }
        }
    }
}
