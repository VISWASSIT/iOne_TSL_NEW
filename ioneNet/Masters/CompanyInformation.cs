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
    public partial class CompanyInformation : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public CompanyInformation()
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
                foreach (Control s in groupBox2.Controls)
                {
                    if (s is TextBox)
                        (s as TextBox).Clear();
                }

            }
            pictureBox1.Image = null;
            //autogen();
            txtCompanyName.Focus();
        }

        
        public void Save()
        {
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            { 

                
                if(txtCompanyId.Text !="")
                {
                    //if (AppCode.GlobalAccess.Add == "Yes")
                    //{
                        var c = db.Company_Infos.Where(w => w.Id == Convert.ToInt32(txtCompanyId.Text)).FirstOrDefault();
                        {

                            // c.Company_ID = txtCompanyId.Text;
                            c.Company_Name = (txtCompanyName.Text == "") ? "" : (txtCompanyName.Text);
                            c.Address = (txtAddress.Text == "") ? "" : (txtAddress.Text);
                            c.City = (txtCity.Text == "") ? "" : txtCity.Text;
                            c.State = (txtState.Text == "") ? "" : txtState.Text;
                            c.State_Code= (txtstatecode.Text == "") ? "" : txtstatecode.Text;
                            c.Phone_No = (txtPhoneNo.Text == "") ? "" : txtPhoneNo.Text;
                            c.Fax_No = (txtFax.Text == "") ? "" : txtFax.Text;
                            c.E_Mail = (txtEmail.Text == "") ? "" : txtEmail.Text;
                            c.Website = (txtWebsite.Text == "") ? "" : txtWebsite.Text;                        
                            c.GST_No= (txtgstno.Text == "") ? "" : txtgstno.Text;
                            c.ARN_No = (txtARNno.Text == "") ? "" : txtARNno.Text;
                            c.PFCode = (txtPFCode.Text == "") ? "" : txtPFCode.Text;
                            c.ESiCode = (txtESICode.Text == "") ? "" : txtESICode.Text;
                            c.PANNo = (txtpanNO.Text == "") ? "" : txtpanNO.Text;
                            c.CINNo = (txtCINNo.Text == "") ? "" : txtCINNo.Text;
                            c.Alias_Name = (txtAliasName.Text == "") ? "" : (txtAliasName.Text);
                            c.Comp_ShortName = (txtCompShortName.Text == "") ? "" : (txtCompShortName.Text);
                            c.Bank_Name = (txtBankDetails.Text == "") ? "" : (txtBankDetails.Text);
                            c.Status = Convert.ToInt32(cmbStatus.SelectedValue);                                                       
                            c.Company = logIn.company;
                            c.Modified_By = logIn.username;
                            c.Modified_On = Convert.ToDateTime(DateTime.Now.ToString());
                            c.Jurisdiction= (txtJurisdition.Text == "") ? "" : txtJurisdition.Text;
                            if (pictureBox1.Image != null)
                            {
                                Image img = pictureBox1.Image;
                                MemoryStream ms = new MemoryStream();
                                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                byte[] bytes = ms.ToArray();
                                c.Company_Logo = bytes;
                            }
                            else
                                c.Company_Logo = null;


                        if (pictureBox3.Image != null)
                        {
                            Image img = pictureBox3.Image;
                            MemoryStream ms = new MemoryStream();
                            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            byte[] bytes = ms.ToArray();
                            c.Company_ISO_Logo = bytes;
                        }
                        else
                            c.Company_ISO_Logo = null;
                        db.SubmitChanges();
                        tran.Commit();
                            MessageBox.Show("Record Updated Successfully");
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
                    ci.Fax_No = (txtFax.Text == "") ? "" : txtFax.Text;
                    ci.E_Mail = (txtEmail.Text == "") ? "" : txtEmail.Text;
                    ci.Website = (txtWebsite.Text == "") ? "" : txtWebsite.Text;
                    ci.GST_No = (txtgstno.Text == "") ? "" : txtgstno.Text;
                    ci.ARN_No = (txtARNno.Text == "") ? "" : txtARNno.Text;
                    ci.PFCode = (txtPFCode.Text == "") ? "" : txtPFCode.Text;
                    ci.ESiCode = (txtESICode.Text == "") ? "" : txtESICode.Text;
                    ci.PANNo = (txtpanNO.Text == "") ? "" : txtpanNO.Text;
                    ci.CINNo = (txtCINNo.Text == "") ? "" : txtCINNo.Text;
                    ci.Company = logIn.company;
                    ci.Created_By = logIn.username;
                    ci.Alias_Name = (txtAliasName.Text == "") ? "" : (txtAliasName.Text);
                    ci.Comp_ShortName = (txtCompShortName.Text == "") ? "" : (txtCompShortName.Text);
                    ci.Status = Convert.ToInt32(cmbStatus.SelectedValue);
                    ci.Created_On = Convert.ToDateTime(DateTime.Now.ToString());
                    if (pictureBox1.Image != null)
                    {
                        Image img = pictureBox1.Image;
                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] bytes = ms.ToArray();
                        ci.Company_Logo = bytes;
                    }
                    else
                        ci.Company_Logo = null;

                    db.Company_Infos.InsertOnSubmit(ci);
                    db.SubmitChanges();
                    tran.Commit();
                    MessageBox.Show("Record Saved Successfully");
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

                OpenFileDialog open = new OpenFileDialog();
                if (open.ShowDialog() == DialogResult.OK)
                {
                    Image img = new Bitmap(open.FileName);
                    pictureBox1.Image = img;

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
                    MessageBox.Show("Company Name should Not be Empty", "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                else if (txtgstno.Text == "")
                {
                    MessageBox.Show("GST NO should Not be Empty ", "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtgstno.Focus();
                    return;

                }
                //else if (txtARNno.Text == "")
                //{
                //    MessageBox.Show("ARN No should Not be Empty ", "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    txtARNno.Focus();
                //    return;

                //}                
                //else if (txtpanNO.Text == "")
                //{
                //    MessageBox.Show("PAN NO should Not be Empty ", "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    txtpanNO.Focus();
                //    return;

                //}
                //else if (txtCINNo.Text == "")
                //{
                //    MessageBox.Show("CIN NO should Not be Empty ", "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    txtCINNo.Focus();
                //    return;

                //}
                else
                {
                    Save();
                    clear();
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
                txtCompanyId.Text = CompanyList.comp_id;
                txtCompanyName.Text = CompanyList.comp_name;
                    var d = (from s in db.Company_Infos where s.Id == Convert.ToInt32(txtCompanyId.Text) select s).SingleOrDefault();
                    if (d != null)
                    {
                        txtCompanyId.Text = d.Id.ToString();
                        txtCompanyName.Text = d.Company_Name;
                        txtAliasName.Text = d.Alias_Name;
                        txtAddress.Text = d.Address;
                        txtCity.Text = d.City;
                        txtState.Text = d.State;
                        txtstatecode.Text = d.State_Code;
                        txtPhoneNo.Text = d.Phone_No;
                        txtFax.Text = d.Fax_No;
                        txtEmail.Text = d.E_Mail;
                        txtWebsite.Text = d.Website;                        
                        txtgstno.Text = d.GST_No;
                        txtARNno.Text = d.ARN_No;                      
                        txtPFCode.Text = d.PFCode;
                        txtpanNO.Text = d.PANNo;
                        txtCINNo.Text = d.CINNo;
                        txtESICode.Text = d.ESiCode;
                        cmbStatus.SelectedValue = d.Status;
                        txtJurisdition.Text = d.Jurisdiction;                        
                        lblCreated.Text = d.Created_By + "_" + d.Created_On;
                        lblModified.Text = d.Modified_By + "_" + d.Modified_On;
                        txtCompShortName.Text = d.Comp_ShortName;
                    txtBankDetails.Text = d.Bank_Name;
                    if (d.Company_Logo != null)
                        {
                            var f = (from s in db.Company_Infos where s.Id == Convert.ToInt32(txtCompanyId.Text) select s);
                            SqlCommand cmd = (SqlCommand)db.GetCommand(f);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt = new DataTable();
                            DataSet ds = new DataSet("MyImages");
                            byte[] MyData = new byte[0];
                            da.Fill(ds, "MyImages");
                            DataRow myRow;
                            myRow = ds.Tables["MyImages"].Rows[0];
                            MyData = (byte[])myRow["Company_Logo"];
                            MemoryStream stream = new MemoryStream(MyData);
                            pictureBox1.Image = Image.FromStream(stream);


                        }
                    if (d.Company_ISO_Logo != null)
                    {
                        var f = (from s in db.Company_Infos where s.Id == Convert.ToInt32(txtCompanyId.Text) select s);
                        SqlCommand cmd = (SqlCommand)db.GetCommand(f);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        DataSet ds = new DataSet("MyImages");
                        byte[] MyData = new byte[0];
                        da.Fill(ds, "MyImages");
                        DataRow myRow;
                        myRow = ds.Tables["MyImages"].Rows[0];
                        MyData = (byte[])myRow["Company_ISO_Logo"];
                        MemoryStream stream = new MemoryStream(MyData);
                        pictureBox3.Image = Image.FromStream(stream);


                    }
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
                pictureBox1.Image = null;
                //autogen();
                var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Master" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbStatus.DataSource = pStatus;
                    cmbStatus.ValueMember = "ID";
                    cmbStatus.DisplayMember = "Descr";
                }
                txtCompanyName.Focus();
                if (ioneNet.Masters.CompanyList.comp_id != ""&& ioneNet.Masters.CompanyList.comp_id !=null)
                    
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

                OpenFileDialog open = new OpenFileDialog();
                if (open.ShowDialog() == DialogResult.OK)
                {
                    Image img = new Bitmap(open.FileName);
                    pictureBox3.Image = img;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
