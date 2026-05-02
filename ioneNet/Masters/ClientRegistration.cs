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
    public partial class ClientRegistration : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public ClientRegistration()
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
            txtCompanyName.Focus();
        }

        

        public void Save()
        {
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            { 

                if(txtCompanyId.Text!="")
                //if ((from u in db.Client_Infos where u.Id == txtCompanyId.Text select u).Count() > 0)
                {
                    //if (AppCode.GlobalAccess.Add == "Yes")
                    //{
                        var c = db.Client_Infos.Where(w => w.Id == Convert.ToInt32(txtCompanyId.Text)).FirstOrDefault();
                        {

                            // c.Company_ID = txtCompanyId.Text;
                            c.Client_Name = (txtCompanyName.Text == "") ? "" : (txtCompanyName.Text);
                            c.Address = (txtAddress.Text == "") ? "" : (txtAddress.Text);
                            c.City = (txtCity.Text == "") ? "" : txtCity.Text;
                            c.State = (txtState.Text == "") ? "" : txtState.Text;
                            c.State_Code= (txtstatecode.Text == "") ? "" : txtstatecode.Text;
                            c.Phone_No = (txtPhoneNo.Text == "") ? "" : txtPhoneNo.Text;
                            //c.Fax_No = (txtFax.Text == "") ? "" : txtFax.Text;
                            c.E_Mail = (txtEmail.Text == "") ? "" : txtEmail.Text;
                            c.ContactPerson = (txtContactPerson.Text == "") ? "" : txtContactPerson.Text;                        
                            c.MobileNo = (txtMobile.Text == "") ? "" : txtMobile.Text;
                            c.NoofUsers = Convert.ToInt32(txtNoOfUsers.Text);
                            c.AdminUserName = (txtAdminUserName.Text == "") ? "" : txtAdminUserName.Text;                            
                            c.AdminPassword = (txtPassWord.Text == "") ? "" : txtPassWord.Text;
                            c.Date_Of_Live = dtLiveDate.Value;
                            c.Amc_Start_Date = dtAMCDate.Value;
                            c.Alias_Name = (txtAliasName.Text == "") ? "" : (txtAliasName.Text);
                            c.Status = Convert.ToInt32(cmbStatus.SelectedValue);

                            //c.Comp_ShortName = (txtCompShortName.Text == "") ? "" : (txtCompShortName.Text);

                            //c.Company = logIn.company;
                            c.Modified_By = logIn.username;
                            c.Modified_On = Convert.ToDateTime(DateTime.Now.ToString());
                            //string modules ="";
                            //for (int k = 0; k < checkedListBox1.Items.Count; k++) //Getting Grouops
                            //{
                            //    if (checkedListBox1.GetItemChecked(k))
                            //    {
                            //        if (modules != "")
                            //        {
                            //            modules = modules +"," + checkedListBox1.Items[k].ToString();
                            //        }
                            //        else
                            //        {
                            //            modules = checkedListBox1.Items[k].ToString();
                            //        }
                            //    }
                            //}

                            c.Modules_allowed= textBox1.Text;                        
                            db.SubmitChanges();
                            tran.Commit();
                            con.Close();

                        
                        //if (CompID.Count > 0)
                        //{
                        //txtCompanyId.Text = CompID[0].Id.ToString();
                        //}

                        String query = "INSERT INTO dbo.User_Setup (User_Name,Password,Company_ID,bu_allowed,user_role_id) VALUES (@username,@password, @Comp,@buname,'1')";

                        SqlCommand command = new SqlCommand(query, con);
                        //command.Parameters.Add("@id", "abc");
                        command.Parameters.Add("@username", txtAdminUserName.Text);
                        command.Parameters.Add("@password", txtPassWord.Text);

                        // command.Parameters.Add("@Role", txtPassWord);
                        var CompID = (from m in db.Company_Infos where m.Client_ID == Convert.ToInt32(txtCompanyId.Text) select new { m.Id }).Distinct().ToList();
                        if (CompID.Count > 0)
                        {
                            command.Parameters.Add("@Comp", CompID[0].Id);
                        }
                        else
                        {
                            command.Parameters.Add("@Comp",0);
                        }

                        command.Parameters.Add("@buname", txtCompanyName.Text);
                        con.Open();
                        command.ExecuteNonQuery();
                        con.Close();

                        //User Role
                        String role = "INSERT INTO dbo.User_Roles (Role_ID,Role_name,Modules_Allowed,Form_ID,Form_name,Create_Role,Role_Status_ID,Company_ID,roll_type) VALUES ('1','resadmin','Administrator','7','User Roles','1','1',@comp,'Super User')";
                        con.Open();
                        SqlCommand command1 = new SqlCommand(role, con);

                        command1.Parameters.Add("@comp", CompID[0].Id);


                        command1.ExecuteNonQuery();
                        con.Close();
                      
                        SaveCompInfo();
                        SaveBUInfo();

                       

                       


                        MessageBox.Show("Record Updated Successfully");
                           
                            txtCompanyName.Focus();
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
                
                    Client_Info ci = new Client_Info();
                    ci.Client_Name = (txtCompanyName.Text == "") ? "" : (txtCompanyName.Text);
                    ci.Address = (txtAddress.Text == "") ? "" : (txtAddress.Text);
                    ci.City = (txtCity.Text == "") ? "" : txtCity.Text;
                    ci.State = (txtState.Text == "") ? "" : txtState.Text;
                    ci.State_Code = (txtstatecode.Text == "") ? "" : txtstatecode.Text;
                    ci.Phone_No = (txtPhoneNo.Text == "") ? "" : txtPhoneNo.Text;
                    //c.Fax_No = (txtFax.Text == "") ? "" : txtFax.Text;
                    ci.E_Mail = (txtEmail.Text == "") ? "" : txtEmail.Text;
                    ci.ContactPerson = (txtContactPerson.Text == "") ? "" : txtContactPerson.Text;
                    ci.MobileNo = (txtMobile.Text == "") ? "" : txtMobile.Text;
                    ci.NoofUsers = Convert.ToInt32(txtNoOfUsers.Text);
                    ci.AdminUserName = (txtAdminUserName.Text == "") ? "" : txtAdminUserName.Text;
                    ci.AdminPassword = (txtPassWord.Text == "") ? "" : txtPassWord.Text;
                    ci.Date_Of_Live = dtLiveDate.Value;
                    ci.Amc_Start_Date = dtAMCDate.Value;
                    ci.Alias_Name = (txtAliasName.Text == "") ? "" : (txtAliasName.Text);

                    //c.Comp_ShortName = (txtCompShortName.Text == "") ? "" : (txtCompShortName.Text);

                    //c.Company = logIn.company;
                    ci.Modified_By = logIn.username;
                    ci.Modified_On = Convert.ToDateTime(DateTime.Now.ToString());
                    string modules = "";
                    for (int k = 0; k < checkedListBox1.Items.Count; k++) //Getting Grouops
                    {
                        if (checkedListBox1.GetItemChecked(k))
                        {
                            if (modules != "")
                            {
                                modules = modules + "," + checkedListBox1.Items[k].ToString();
                            }
                            else
                            {
                                modules = checkedListBox1.Items[k].ToString();
                            }
                        }
                    }

                    ci.Modules_allowed = modules;
                    ci.Status = Convert.ToInt32(cmbStatus.SelectedValue);
                    db.Client_Infos.InsertOnSubmit(ci);
                    db.SubmitChanges();
                    tran.Commit();
                    con.Close();
                    var ClientID = (from m in db.Client_Infos where m.Client_Name == txtCompanyName.Text select new { m.Id }).Distinct().ToList();
                    if (ClientID.Count > 0)
                    {
                        txtCompanyId.Text = ClientID[0].Id.ToString();
                    }
                    SaveCompInfo();
                    SaveBUInfo();                   


                    String query = "INSERT INTO dbo.User_Setup (User_Name,Password,Company_ID,bu_allowed,user_role_id) VALUES (@username,@password, @Comp,@buname,'1')";

                    SqlCommand command = new SqlCommand(query, con);
                    //command.Parameters.Add("@id", "abc");
                    command.Parameters.Add("@username", txtAdminUserName.Text);
                    command.Parameters.Add("@password", txtPassWord.Text);
                   
                    // command.Parameters.Add("@Role", txtPassWord);
                    var CompID = (from m in db.Company_Infos where m.Client_ID == Convert.ToInt32(txtCompanyId.Text)  select new { m.Id }).Distinct().ToList();
                    if (CompID.Count > 0)
                    {
                        command.Parameters.Add("@comp", CompID[0].Id);
                    }
                    command.Parameters.Add("@buname", txtCompanyName.Text);
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                    //User Role
                    String role = "INSERT INTO dbo.User_Roles (Role_ID,Role_name,Modules_Allowed,Form_ID,Form_name,Create_Role,Role_Status_ID,Company_ID,roll_type) VALUES ('1','resadmin','Administrator','7','User Roles','1','1',@comp,'Super User')";
                    con.Open();
                    SqlCommand command1 = new SqlCommand(role, con);                   
                    command1.Parameters.Add("@comp", CompID[0].Id);
                    
                    
                    command1.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Record Saved Successfully");
                    
                    txtCompanyName.Focus();
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

                //OpenFileDialog open = new OpenFileDialog();
                //if (open.ShowDialog() == DialogResult.OK)
                //{
                //    Image img = new Bitmap(open.FileName);
                //    pictureBox1.Image = img;

                //}

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
                    if (txtCompanyId.Text != "")
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
                if (txtCompanyName.Text == string.Empty)
                {
                    MessageBox.Show("Client Name should Not be Empty", "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCompanyName.Focus();
                    return;

                }

                else if (txtAddress.Text == "")
                {
                    MessageBox.Show("Address should Not be Empty ", "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtAddress.Focus();
                    return;

                }
                else if (txtCity.Text == "")
                {
                    MessageBox.Show("City should Not be Empty ", "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtCity.Focus();
                    return;

                }
                else if (txtState.Text == "")
                {
                    MessageBox.Show("State should Not be Empty ", "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtState.Focus();
                    return;

                }
                else if (txtstatecode.Text == "")
                {
                    MessageBox.Show("State Code should Not be Empty ", "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtstatecode.Focus();
                    return;

                }
                else if (txtPhoneNo.Text == "")
                {
                    MessageBox.Show("Phone No should Not be Empty ", "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPhoneNo.Focus();
                    return;

                }

                else if (txtNoOfUsers.Text == "")
                {
                    MessageBox.Show("Number of Users Should Not be Empty, Enter 0 for Unlimited Users ", "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtNoOfUsers.Focus();
                    return;

                }
                else if (txtAdminUserName.Text == "")
                {
                    MessageBox.Show("User Name should Not be Empty ", "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtAdminUserName.Focus();
                    return;

                }
                else if (txtPassWord.Text == "")
                {
                    MessageBox.Show("password should Not be Empty ", "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPassWord.Focus();
                return;

                }
                else if (textBox1.Text == "")
                {
                    MessageBox.Show("Have to Select Atleast 1 Module to proceed", "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    checkedListBox1.Focus();
                    return;

                }
                else
                {
                   

                    Save();                   
                   // clear();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }       

        public void Edit_CompInfo()
        {
            try
            {
                //CompanyList cs = new CompanyList();
                //if (cs.ShowDialog() == DialogResult.OK)
                //{
                    txtCompanyId.Text = ClientList.comp_id;
                    txtCompanyName.Text = ClientList.comp_name;
                    var d = (from s in db.Client_Infos where s.Id == Convert.ToInt32(txtCompanyId.Text) select s).SingleOrDefault();
                    if (d != null)
                    {
                        txtCompanyId.Text = d.Id.ToString();
                        txtCompanyName.Text = d.Client_Name;
                        txtAliasName.Text = d.Alias_Name;
                        txtAddress.Text = d.Address;
                        txtCity.Text = d.City;
                        txtState.Text = d.State;
                        txtstatecode.Text = d.State_Code;
                        txtPhoneNo.Text = d.Phone_No;
                        txtContactPerson.Text = d.ContactPerson;
                        
                        txtEmail.Text = d.E_Mail;
                        txtMobile.Text = d.MobileNo;                        
                        txtNoOfUsers.Text = d.NoofUsers.ToString();
                        txtAdminUserName.Text = d.AdminUserName;                      
                        txtPassWord.Text = d.AdminPassword;
                        dtLiveDate.Text = d.Date_Of_Live.ToString();
                        dtAMCDate.Text = d.Amc_Start_Date.ToString();
                        cmbStatus.SelectedValue = d.Status;
                        textBox1.Text = d.Modules_allowed;
                        string items = d.Modules_allowed;
                        if (items != null)
                        {
                            string[] p = new string[] { };
                            p = items.ToString().Split(',');
                            int lenght = p.Length;
                            for (int i = 0; i < lenght; i++)
                            {
                                string fet = p[i];
                                for (int j = 0; j <= checkedListBox1.Items.Count - 1; j++)
                                {
                                    if (checkedListBox1.Items[j].ToString() == fet)
                                    {
                                    checkedListBox1.SetItemChecked(j, true);
                                    }
                                }
                            }
                        }

                    //txtESICode.Text = d.ESiCode;
                    //txtJurisdition.Text = d.Jurisdiction;                        
                    lblCreated.Text = d.Created_By + "_" + d.Created_On;
                        lblModified.Text = d.Modified_By + "_" + d.Modified_On;
                        
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
                //pictureBox1.Image = null;
                //autogen();
                txtCompanyName.Focus();

                using (SqlCommand cmd = new SqlCommand("SELECT distinct([Module_name]) FROM [Modules_list]", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                checkedListBox1.Items.Add(dt.Rows[i]["Module_name"].ToString());
                            }

                        }
                    }
                }
                var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Master" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbStatus.DataSource = pStatus;
                    cmbStatus.ValueMember = "ID";
                    cmbStatus.DisplayMember = "Descr";
                }

                if (ioneNet.Masters.ClientList.comp_id != ""&& ioneNet.Masters.ClientList.comp_id !=null)
                    
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

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtCompanyName_Leave(object sender, EventArgs e)
        {
            if(txtCompanyName.Text!="")
            {
                if (txtCompanyId.Text == "")
                {

                    if ((from u in db.Client_Infos where u.Client_Name == txtCompanyName.Text select u).Count() > 0)
                    {
                        MessageBox.Show("Client Name Cannot Be Duplicate", "Client Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtCompanyName.Focus();
                    }
                }
            }
        }

        private void txtAdminUserName_Leave(object sender, EventArgs e)
        {
            if (txtAdminUserName.Text != "")
            {
                if (txtCompanyId.Text == "")
                {

                    if ((from u in db.Client_Infos where u.AdminUserName == txtAdminUserName.Text select u).Count() > 0)
                    {
                        MessageBox.Show("User Name Not Avaialble", "Client Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtAdminUserName.Focus();
                    }
                }
                else
                {

                    if ((from u in db.Client_Infos where u.AdminUserName == txtAdminUserName.Text && u.Id == Convert.ToInt32(txtCompanyId.Text) select u).Count() > 0)
                    {
                        //MessageBox.Show("User Name Not Avaialble", "Client Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //txtAdminUserName.Focus();
                    }
                    else
                    {
                        if ((from u in db.Client_Infos where u.AdminUserName == txtAdminUserName.Text select u).Count() > 0)
                        {
                            MessageBox.Show("User Name Not Avaialble", "Client Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtAdminUserName.Focus();
                        }
                    }
                }
            }
        }

        private void checkedListBox1_ItemCheck(object sender, ItemCheckEventArgs e)
        {
           
        }

        private void checkedListBox1_MouseUp(object sender, MouseEventArgs e)
        {
            string modules = "";
            for (int k = 0; k < checkedListBox1.Items.Count; k++) //Getting Grouops
            {
                if (checkedListBox1.GetItemChecked(k))
                {
                    if (modules != "")
                    {
                        modules = modules + "," + checkedListBox1.Items[k].ToString();
                    }
                    else
                    {
                        modules = checkedListBox1.Items[k].ToString();
                    }
                }
            }

            textBox1.Text = modules;
        }
        public void SaveCompInfo()
        {
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {


                if ((from u in db.Company_Infos where u.Client_ID == (Convert.ToInt32(txtCompanyId.Text)) select u).Count() > 0)
                {
                    //if (AppCode.GlobalAccess.Add == "Yes")
                    //{
                    var c = db.Company_Infos.Where(w => w.Client_ID == Convert.ToInt32(txtCompanyId.Text)).FirstOrDefault();
                    {

                        // c.Company_ID = txtCompanyId.Text;
                        c.Company_Name = (txtCompanyName.Text == "") ? "" : (txtCompanyName.Text);
                        c.Address = (txtAddress.Text == "") ? "" : (txtAddress.Text);
                        c.City = (txtCity.Text == "") ? "" : txtCity.Text;
                        c.State = (txtState.Text == "") ? "" : txtState.Text;
                        c.State_Code = (txtstatecode.Text == "") ? "" : txtstatecode.Text;
                        c.Phone_No = (txtPhoneNo.Text == "") ? "" : txtPhoneNo.Text;                        
                        c.Alias_Name = (txtAliasName.Text == "") ? "" : (txtAliasName.Text);
                        c.Client_ID = Convert.ToInt32(txtCompanyId.Text);
                        c.Modified_By = logIn.username;
                        c.Modified_On = Convert.ToDateTime(DateTime.Now.ToString());
                        db.SubmitChanges();
                        tran.Commit();
                        //MessageBox.Show("Record Updated Successfully");
                        con.Close();
                        txtCompanyName.Focus();
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

                    Company_Info ci = new Company_Info();
                    //ci.Company_ID = txtCompanyId.Text;
                    ci.Company_Name = (txtCompanyName.Text == "") ? "" : (txtCompanyName.Text);
                    ci.Address = (txtAddress.Text == "") ? "" : (txtAddress.Text);
                    ci.City = (txtCity.Text == "") ? "" : txtCity.Text;
                    ci.State = (txtState.Text == "") ? "" : txtState.Text;
                    ci.State_Code = (txtstatecode.Text == "") ? "" : txtstatecode.Text;
                    ci.Phone_No = (txtPhoneNo.Text == "") ? "" : txtPhoneNo.Text;                   
                    ci.Company = logIn.company;
                    ci.Created_By = logIn.username;
                    ci.Alias_Name = (txtAliasName.Text == "") ? "" : (txtAliasName.Text);
                    ci.Client_ID = Convert.ToInt32(txtCompanyId.Text);
                    ci.Created_On = Convert.ToDateTime(DateTime.Now.ToString());                  

                    db.Company_Infos.InsertOnSubmit(ci);
                    db.SubmitChanges();
                    tran.Commit();
                    //MessageBox.Show("Record Saved Successfully");
                    con.Close();
                    txtCompanyName.Focus();
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

        public void SaveBUInfo()
        {
            con.Open();
            int CID =0;
            SqlTransaction tran = con.BeginTransaction();
            try
            {
                var ClientID = (from m in db.Company_Infos where m.Client_ID == (Convert.ToInt32(txtCompanyId.Text)) select new { m.Id }).Distinct().ToList();
                if (ClientID.Count > 0)
                {
                    CID = ClientID[0].Id;
                    if ((from u in db.Costing_Units where u.Company == (Convert.ToInt32(CID)) select u).Count() > 0)
                    {

                        var c = db.Costing_Units.Where(w => w.Company == Convert.ToInt32(CID)).FirstOrDefault();
                        {
                            c.BU_Name = (txtCompanyName.Text == "") ? "" : (txtCompanyName.Text);
                            c.Address = (txtAddress.Text == "") ? "" : (txtAddress.Text);
                            c.City = (txtCity.Text == "") ? "" : txtCity.Text;
                            c.State = (txtState.Text == "") ? "" : txtState.Text;
                            c.State_Code = (txtstatecode.Text == "") ? "" : txtstatecode.Text;
                            c.Phone_No = (txtPhoneNo.Text == "") ? "" : txtPhoneNo.Text;
                            //ci.Company =CID;
                            c.Created_By = logIn.username;
                            c.ToPrintName = (txtAliasName.Text == "") ? "" : (txtAliasName.Text);
                            c.Company = CID;
                            c.Status = 1;
                            c.Created_On = Convert.ToDateTime(DateTime.Now.ToString());
                            db.SubmitChanges();
                        }
                    }
                    else
                    {


                        Costing_Unit ci = new Costing_Unit();
                        //ci.Company_ID = txtCompanyId.Text;
                        ci.BU_Name = (txtCompanyName.Text == "") ? "" : (txtCompanyName.Text);
                        ci.Address = (txtAddress.Text == "") ? "" : (txtAddress.Text);
                        ci.City = (txtCity.Text == "") ? "" : txtCity.Text;
                        ci.State = (txtState.Text == "") ? "" : txtState.Text;
                        ci.State_Code = (txtstatecode.Text == "") ? "" : txtstatecode.Text;
                        ci.Phone_No = (txtPhoneNo.Text == "") ? "" : txtPhoneNo.Text;
                        //ci.Company =CID;
                        ci.Created_By = logIn.username;
                        ci.ToPrintName = (txtAliasName.Text == "") ? "" : (txtAliasName.Text);
                        ci.Company = CID;
                        ci.Status = 1;
                        ci.Created_On = Convert.ToDateTime(DateTime.Now.ToString());
                        db.Costing_Units.InsertOnSubmit(ci);
                        db.SubmitChanges();
                        tran.Commit();
                        //MessageBox.Show("Record Saved Successfully");
                        con.Close();
                        txtCompanyName.Focus();
                    }
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

        public void SaveFinYear()
        {
            //try
            //{
            //    //if ((from u in db.Financial_Year_Masters where u.id == (Convert.ToInt32(txtCityId.Text)) select u).Count() > 0)
            //    //{
            //    if (txtCityId.Text != "")
            //    {

            //        var c = db.Financial_Year_Masters.Where(w => w.id == (Convert.ToInt32(txtCityId.Text))).FirstOrDefault();
            //        {
            //            //c.ID = Convert.ToInt32(txtCityId.Text.ToString());
            //            c.F_Year = (txtcityname.Text == "") ? "" : (txtcityname.Text);
            //            c.Start_Date = dateTimePicker1.Value;
            //            c.End_Date = dateTimePicker2.Value;
            //            c.FY_ShortCode = (txtYearCode.Text == "") ? "" : (txtYearCode.Text);
            //            c.Uses_AsSufix = checkBox1.Checked;
            //            c.Created_By = linkCreatedBy.Text;
            //            c.Modified_BY = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //            c.Status = Convert.ToInt32(cmbStatus.SelectedValue);
            //            c.Company_ID = logIn.company;
            //            db.SubmitChanges();
            //            MessageBox.Show("Record Upadated Successfully");
            //        }
            //        //}
            //        //else
            //        //{
            //        //    MessageBox.Show("Sorry! You Do not have privileges to Modify City");
            //        //}
            //    }
            //    else
            //    {
            //        //if (frmGate.Create_menu.Contains(this.Text))
            //        //{
            //        Financial_Year_Master ci = new Financial_Year_Master();
            //        //string company = Creation_Company;
            //        //var result = db.Sp_autoincrement_CityMaster(company);
            //        //ci.City_Id = result.FirstOrDefault().City_Id;
            //        ci.F_Year = (txtcityname.Text == "") ? "" : (txtcityname.Text);
            //        ci.Start_Date = dateTimePicker1.Value;
            //        ci.End_Date = dateTimePicker2.Value;
            //        ci.FY_ShortCode = (txtYearCode.Text == "") ? "" : (txtYearCode.Text);
            //        ci.Uses_AsSufix = checkBox1.Checked;
            //        ci.Created_By = linkCreatedBy.Text;
            //        ci.Modified_BY = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //        ci.Status = Convert.ToInt32(cmbStatus.SelectedValue);
            //        ci.Company_ID = logIn.company;
            //        db.Financial_Year_Masters.InsertOnSubmit(ci);
            //        db.SubmitChanges();
            //        MessageBox.Show("Record Saved Successfully");
            //        BindFYear();
            //        //}
            //        //else
            //        //{
            //        //    MessageBox.Show("Sorry! You Do not have privileges to Save City");
            //        //}

            //    }
            //}

            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //finally
            //{
            //}
        }
    }
}
