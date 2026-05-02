using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Data.SqlClient;
using System.Configuration;
using Ione_DAL;
namespace ioneNet.Masters
{
    public partial class BusinessUnitInformation : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public BusinessUnitInformation()
        {
            InitializeComponent();
        }

        #region methods

        public void clear()
        {
            foreach (Control x in this.Controls)
            {
                foreach (Control d in groupBox3.Controls)
                {
                    if (d is TextBox)
                        (d as TextBox).Clear();
                }
                //foreach (Control s in groupBox2.Controls)
                //{
                //    if (s is TextBox)
                //        (s as TextBox).Clear();
                //}

            }
            //pictureBox1.Image = null;
            //autogen();
            txtBUName.Focus();
        }

        
        public void Save()
        {
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            { 

                
                if(txtBUID.Text !="")
                {
                    //if (AppCode.GlobalAccess.Add == "Yes")
                    //{
                        var c = db.Costing_Units.Where(w => w.id == Convert.ToInt32(txtBUID.Text)).FirstOrDefault();
                        {

                            // c.Company_ID = txtCompanyId.Text;
                            c.BU_Name = (txtBUName.Text == "") ? "" : (txtBUName.Text);
                            c.Address = (txtAddress.Text == "") ? "" : (txtAddress.Text);
                            c.City = (txtCity.Text == "") ? "" : txtCity.Text;
                            c.State = (txtState.Text == "") ? "" : txtState.Text;
                            c.State_Code= (txtstatecode.Text == "") ? "" : txtstatecode.Text;
                            c.Phone_No = (txtPhoneNo.Text == "") ? "" : txtPhoneNo.Text;                           
                            c.E_Mail = (txtEmail.Text == "") ? "" : txtEmail.Text;                                            
                            c.GST_No= (txtgstno.Text == "") ? "" : txtgstno.Text;
                            c.PinCode = txtPinCode.Text;
                            c.PFCode = (txtPFCode.Text == "") ? "" : txtPFCode.Text;
                            c.ESiCode = (txtESICode.Text == "") ? "" : txtESICode.Text;
                            c.PANNo = (txtpanNO.Text == "") ? "" : txtpanNO.Text;
                            c.ToPrintName = (txtBUNameToPrint.Text == "") ? "" : txtBUNameToPrint.Text;
                            c.BU_ShortName = (txtCompShortName.Text == "") ? "" : (txtCompShortName.Text);
                            c.BankDetails = (txtBankDetails.Text == "") ? "" : (txtBankDetails.Text);
                            c.VoucherSeries = (cmbVchSeries.Text == "") ? "" : (cmbVchSeries.Text);
                            c.Status = Convert.ToInt32(cmbStatus.SelectedValue);                                                       
                            c.Company = logIn.company;
                            c.Modified_By = logIn.username;
                            c.Modified_On = Convert.ToDateTime(DateTime.Now.ToString());
                            c.Jurisdiction= (txtJurisdition.Text == "") ? "" : txtJurisdition.Text;
                            c.PT_Reg_No = (txtPTRegNo.Text == "") ? "" : txtPTRegNo.Text;

                        db.SubmitChanges();
                        tran.Commit();
                            MessageBox.Show("Record Updated Successfully");
                        con.Close();
                        txtBUName.Focus();
                        }
                    //}
                    //else
                    //{
                    //    MessageBox.Show("You dont Have Privileges", "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    clear();
                    //}


                }
                else
                {
                
                    Costing_Unit ci = new Costing_Unit();
                    //ci.Company_ID = txtCompanyId.Text;
                    ci.BU_Name = (txtBUName.Text == "") ? "" : (txtBUName.Text);
                    ci.Address = (txtAddress.Text == "") ? "" : (txtAddress.Text);
                    ci.City = (txtCity.Text == "") ? "" : txtCity.Text;
                    ci.State = (txtState.Text == "") ? "" : txtState.Text;
                    ci.State_Code = (txtstatecode.Text == "") ? "" : txtstatecode.Text;
                    ci.Phone_No = (txtPhoneNo.Text == "") ? "" : txtPhoneNo.Text;
                  
                    ci.E_Mail = (txtEmail.Text == "") ? "" : txtEmail.Text;
                   
                    ci.GST_No = (txtgstno.Text == "") ? "" : txtgstno.Text;
                   
                    ci.PFCode = (txtPFCode.Text == "") ? "" : txtPFCode.Text;
                    ci.ESiCode = (txtESICode.Text == "") ? "" : txtESICode.Text;
                    ci.PANNo = (txtpanNO.Text == "") ? "" : txtpanNO.Text;
                    ci.ToPrintName = (txtBUNameToPrint.Text == "") ? "" : txtBUNameToPrint.Text;                   
                    ci.BankDetails = (txtBankDetails.Text == "") ? "" : (txtBankDetails.Text);
                    ci.VoucherSeries = (cmbVchSeries.Text == "") ? "" : (cmbVchSeries.Text);                    
                    ci.Company = logIn.company;
                    ci.Created_By = logIn.username;
                  
                    ci.BU_ShortName = (txtCompShortName.Text == "") ? "" : (txtCompShortName.Text);
                    ci.Status = Convert.ToInt32(cmbStatus.SelectedValue);
                    ci.Created_On = Convert.ToDateTime(DateTime.Now.ToString());
                    ci.Jurisdiction = (txtJurisdition.Text == "") ? "" : txtJurisdition.Text;
                    ci.PT_Reg_No = (txtPTRegNo.Text == "") ? "" : txtPTRegNo.Text;

                    db.Costing_Units.InsertOnSubmit(ci);
                    db.SubmitChanges();
                    tran.Commit();
                    MessageBox.Show("Record Saved Successfully");
                    con.Close();
                    txtBUName.Focus();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    tran.Rollback();
                }
                catch (Exception exRollBack)
                {
                    //MessageBox.Show(ex.Message);
                    Console.WriteLine(exRollBack.Message);
                }
               MessageBox.Show(ex.Message);
            }
        }

        #endregion

        #region buttonclickevent

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                clear();

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
                //if (AppCode.GlobalAccess.Edit == "Yes")
                //{
                    if (txtBUID.Text != "")
                    {
                        var result = MessageBox.Show("Are You Sure Want to Delete this Record ", "Company Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (result == DialogResult.Yes)
                        {
                            // db.sp_compInfo_Delete(txtCompanyId.Text);
                            //db.sp_compInfo_Delete(txtCompanyId.Text);
                            MessageBox.Show("Record Deleted Successfully");

                            clear();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Please Select the Record", "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                //}
                //else
                //{
                //    MessageBox.Show("You donot Have Privileges", "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    clear();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                //if (txtCompanyId.Text == string.Empty)
                //{
                //    MessageBox.Show("Company Id should Not be Empty", "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    txtCompanyId.Focus();
                //    return;

                //}
                if (txtBUName.Text == string.Empty)
                {
                    MessageBox.Show("Company Name should Not be Empty", "BU Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtBUName.Focus();
                    return;

                }

                else if (txtAddress.Text == "")
                {
                    MessageBox.Show("Address should Not be Empty ", "BU Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtAddress.Focus();
                    return;

                }
                else if (txtCity.Text == "")
                {
                    MessageBox.Show("City should Not be Empty ", "BU Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCity.Focus();
                    return;

                }
                else if (txtPinCode.Text == "")
                {
                    MessageBox.Show("PIN Code should Not be Empty ", "BU Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPinCode.Focus();
                    return;

                }
                else if (txtState.Text == "")
                {
                    MessageBox.Show("State should Not be Empty ", "BU Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtState.Focus();
                    return;

                }
                else if (txtstatecode.Text == "")
                {
                    MessageBox.Show("State Code should Not be Empty ", "BU Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtstatecode.Focus();
                    return;

                }
                else if (txtPhoneNo.Text == "")
                {
                    MessageBox.Show("Phone No should Not be Empty ", "BU Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPhoneNo.Focus();
                    return;

                }               

                else if (txtgstno.Text == "")
                {
                    MessageBox.Show("GST NO should Not be Empty ", "BU Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtgstno.Focus();
                    return;

                }
                else if (cmbVchSeries.Text == "")
                {
                    MessageBox.Show("You Have To Select Voucher Series Option", "BU Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbVchSeries.Focus();
                    return;

                }
               
                else
                {
                    Save();
                    clear();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "BU Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }       

        public void Edit_CompInfo()
        {
            try
            {
                //CompanyList cs = new CompanyList();
                //if (cs.ShowDialog() == DialogResult.OK)
                //{
                txtBUID.Text = BusinessUnitsList.comp_id;
                txtBUName.Text = BusinessUnitsList.comp_name;
                    var d = (from s in db.Costing_Units where s.id == Convert.ToInt32(txtBUID.Text) select s).SingleOrDefault();
                    if (d != null)
                    {
                        txtBUID.Text = d.id.ToString();
                        txtBUName.Text = d.BU_Name;                      
                        txtAddress.Text = d.Address;
                        txtCity.Text = d.City;
                        txtState.Text = d.State;
                        txtstatecode.Text = d.State_Code;
                        txtPhoneNo.Text = d.Phone_No;                      
                        txtEmail.Text = d.E_Mail;
                        txtPinCode.Text = d.PinCode;   
                        txtgstno.Text = d.GST_No;
                                      
                        txtPFCode.Text = d.PFCode;
                        txtpanNO.Text = d.PANNo;
                       
                        txtESICode.Text = d.ESiCode;
                        cmbStatus.SelectedValue = d.Status;
                        txtJurisdition.Text = d.Jurisdiction;                        
                        lblCreated.Text = d.Created_By + "_" + d.Created_On;
                        lblModified.Text = d.Modified_By + "_" + d.Modified_On;
                        txtCompShortName.Text = d.BU_ShortName;
                        txtCompName.Text = logIn.compname;
                        txtCompID.Text = logIn.company.ToString();
                        txtBankDetails.Text = d.BankDetails;
                        cmbVchSeries.Text = d.VoucherSeries;
                        txtBUNameToPrint.Text = d.ToPrintName;
                        txtPTRegNo.Text = d.PT_Reg_No;
                  
                }
                
            
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        #endregion

        #region event

        private void CompanyInformation_Load(object sender, EventArgs e)
        {
            try
            {
                ///*  pictureBox2.Image = AppCode.Global*/Access.comylogo;
               
                //autogen();
                var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Master" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbStatus.DataSource = pStatus;
                    cmbStatus.ValueMember = "ID";
                    cmbStatus.DisplayMember = "Descr";
                }
                txtBUName.Focus();
                txtCompID.Text = logIn.company.ToString();
                txtCompName.Text = logIn.compname;
                if (ioneNet.Masters.BusinessUnitsList.comp_id != ""&& ioneNet.Masters.BusinessUnitsList.comp_id !=null)
                    
                {
                    Edit_CompInfo();
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtPhoneNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }





        #endregion

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label23_Click(object sender, EventArgs e)
        {

        }

        private void txtWebsite_TextChanged(object sender, EventArgs e)
        {

        }

        private void label17_Click(object sender, EventArgs e)
        {

        }

        private void txtgstno_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void txtPFCode_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtESICode_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtJurisdition_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtpanNO_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtEmail_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPhoneNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtState_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCity_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtAddress_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCompShortName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCompanyName_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCompanyId_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
