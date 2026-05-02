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
    public partial class frmUserCreation : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmUserCreation()
        {
            InitializeComponent();
        }
        private void frmUserCreation_Load(object sender, EventArgs e)
        {
            try
            {
                lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");


                using (SqlCommand cmd = new SqlCommand("SELECT distinct ([BU_Name]) FROM [Costing_Units] where company =@Company_ID", con))
                {
                    cmd.Parameters.AddWithValue("@Company_ID", logIn.company);
                    cmd.CommandType = CommandType.Text;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                lstBusinessUnits.Items.Add(dt.Rows[i]["BU_Name"].ToString());
                            }

                        }
                    }
                }

                using (SqlCommand cmd1 = new SqlCommand("SELECT distinct ([F_Year]) FROM [Financial_Year_Master] where company_id =@Company_ID", con))
                {
                    cmd1.Parameters.AddWithValue("@Company_ID", logIn.company);
                    cmd1.CommandType = CommandType.Text;

                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd1))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                lstFinYear.Items.Add(dt.Rows[i]["F_Year"].ToString());
                            }

                        }
                    }
                }

                //Status
                var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Master" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbStatus.DataSource = pStatus;
                    cmbStatus.ValueMember = "ID";
                    cmbStatus.DisplayMember = "Descr";
                }

                //User Roles
                var uRoles = (from m in db.User_Roles where m.Company_ID == logIn.company select new { m.Role_ID, m.Role_Name }).Distinct().ToList();
                if (uRoles.Count > 0)
                {
                    cmbUserRole.DataSource = uRoles;
                    cmbUserRole.ValueMember = "Role_ID";
                    cmbUserRole.DisplayMember = "Role_Name";
                }

                //Cost Center -MM
                var cc = (from m in db.Department_Masters where m.Company_ID == logIn.company select new { m.Id, m.Dept_Name }).Distinct().ToList();
                if (cc.Count > 0)
                {
                    cmbDepartment.DataSource = cc;
                    cmbDepartment.ValueMember = "Id";
                    cmbDepartment.DisplayMember = "Dept_Name";
                }

                if (ioneNet.Masters.frmUsersList.UserID > 0)

                {
                    Edit_UserInfo();
                    ioneNet.Masters.frmUsersList.UserID = 0;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }


        }
        private void cmbProdGroup_SelectedIndexChanged(object sender, EventArgs e)
        {

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
                if (txtUserName.Text == string.Empty)
                {
                    MessageBox.Show("User Name should Not be Empty", "User Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtUserName.Focus();
                    return;

                }

                else if (txtPassword.Text == "")
                {
                    MessageBox.Show("Password should Not be Empty ", "User Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtPassword.Focus();
                    return;

                }
                else if (cmbUserRole.Text == "")
                {
                    MessageBox.Show("User Role should Not be Empty ", "User Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbUserRole.Focus();
                    return;

                }                

                else if (cmbStatus.Text == "")
                {
                    MessageBox.Show("Status should Not be Empty ", "User Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbStatus.Focus();
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
        public void Save()
        {
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {

                string BUNames = "";
                string Fyear = "";

                if (txtID.Text != "")
                {
                    //if (AppCode.GlobalAccess.Add == "Yes")
                    //{
                    var c = db.User_Setups.Where(w => w.User_ID == Convert.ToInt32(txtID.Text)).FirstOrDefault();
                    {

                        // c.Company_ID = txtCompanyId.Text;
                        c.User_Name = (txtUserName.Text == "") ? "" : (txtUserName.Text);
                        c.Password = (txtPassword.Text == "") ? "" : (txtPassword.Text);
                        c.User_Role_ID = Convert.ToInt32(cmbUserRole.SelectedValue);
                        c.Employee_Name = cmbEmployeeName.Text;
                        c.Email = (txtEmail.Text == "") ? "" : txtEmail.Text;
                        c.MobileNo = (txtMobileNo.Text == "") ? "" : txtMobileNo.Text;
                        c.Department = Convert.ToInt32(cmbDepartment.SelectedValue);                       
                        c.Status = Convert.ToInt32(cmbStatus.SelectedValue);
                        c.Company_ID = logIn.company;
                        c.Created_By = lblCreatedBy.Text;
                        c.Modified_BY = logIn.username + "-" + DateTime.Now;

                        if (picUser.Image != null)
                        {
                            Image img = picUser.Image;
                            MemoryStream ms = new MemoryStream();
                            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            byte[] bytes = ms.ToArray();
                            c.User_Image = bytes;
                        }
                        else
                        c.User_Image = null;
                        for (int i = 0; i < lstBusinessUnits.Items.Count; i++)
                        {
                            if (lstBusinessUnits.GetItemCheckState(i) == CheckState.Checked)
                            {
                                
                                if (BUNames != "")
                                {

                                    BUNames = BUNames + "," + lstBusinessUnits.Items[i].ToString();

                                }
                                else
                                {
                                    BUNames = lstBusinessUnits.Items[i].ToString();
                                }

                            }
                        }
                        c.BU_Allowed = BUNames;

                        for (int i = 0; i < lstFinYear.Items.Count; i++)
                        {
                            if (lstFinYear.GetItemCheckState(i) == CheckState.Checked)
                            {

                                if (Fyear != "")
                                {

                                    Fyear = Fyear + "," + lstFinYear.Items[i].ToString();

                                }
                                else
                                {
                                    Fyear = lstFinYear.Items[i].ToString();
                                }

                            }
                        }
                        c.FY_Allowed = Fyear;


                        db.SubmitChanges();
                        tran.Commit();
                        MessageBox.Show("Record Updated Successfully");
                        con.Close();
                        txtUserName.Focus();
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

                    User_Setup ci = new User_Setup();
                    //ci.Company_ID = txtCompanyId.Text;
                    ci.User_Name = (txtUserName.Text == "") ? "" : (txtUserName.Text);
                    ci.Password = (txtPassword.Text == "") ? "" : (txtPassword.Text);
                    ci.User_Role_ID = Convert.ToInt32(cmbUserRole.SelectedValue);
                    ci.Employee_Name = cmbEmployeeName.Text;
                    ci.Email = (txtEmail.Text == "") ? "" : txtEmail.Text;
                    ci.MobileNo = (txtMobileNo.Text == "") ? "" : txtMobileNo.Text;
                    ci.Department = Convert.ToInt32(cmbDepartment.SelectedValue);
                    ci.Status = Convert.ToInt32(cmbStatus.SelectedValue);
                    ci.Company_ID = logIn.company;
                    ci.Created_By = lblCreatedBy.Text;
                    ci.Modified_BY = logIn.username + "-" + DateTime.Now;

                    if (picUser.Image != null)
                    {
                        Image img = picUser.Image;
                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] bytes = ms.ToArray();
                        ci.User_Image = bytes;
                    }
                    else
                        ci.User_Image = null;
                    for (int i = 0; i < lstBusinessUnits.Items.Count; i++)
                    {
                        if (lstBusinessUnits.GetItemCheckState(i) == CheckState.Checked)
                        {

                            if (BUNames != "")
                            {

                                BUNames = BUNames + "," + lstBusinessUnits.Items[i].ToString();

                            }
                            else
                            {
                                BUNames = lstBusinessUnits.Items[i].ToString();
                            }

                        }
                    }
                    ci.BU_Allowed = BUNames;
                    db.User_Setups.InsertOnSubmit(ci);
                    db.SubmitChanges();
                    tran.Commit();
                    MessageBox.Show("Record Saved Successfully");
                    con.Close();
                    txtUserName.Focus();
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        

        private void txtUserName_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtUserName.Text != "")
                {
                    if (txtID.Text == "")
                    {

                        if ((from u in db.User_Setups where u.User_Name == txtUserName.Text && u.Company_ID == logIn.company select u).Count() > 0)
                        {
                            MessageBox.Show("User Name Cannot Be Duplicate", "Party Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtUserName.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "User Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void Edit_UserInfo()
        {
            try
            {
                //CompanyList cs = new CompanyList();
                //if (cs.ShowDialog() == DialogResult.OK)
                //{
                txtID.Text = frmUsersList.UserID.ToString();
              
                var d = (from s in db.User_Setups where s.User_ID == Convert.ToInt32(txtID.Text) && s.Company_ID ==logIn.company select s).SingleOrDefault();
                if (d != null)
                {
                    
                    txtUserName.Text = d.User_Name;
                    txtPassword.Text = d.Password;
                    cmbUserRole.SelectedValue = d.User_Role_ID;
                    cmbEmployeeName.Text = d.Employee_Name;
                    txtMobileNo.Text = d.MobileNo;
                    cmbDepartment.SelectedValue = d.Department;                   
                    txtEmail.Text = d.Email;                   
                    cmbStatus.SelectedValue = d.Status;
                   
                    lblCreatedBy.Text = d.Created_By;
                    lblModified.Text = d.Modified_BY;                   
                    if (d.User_Image != null)
                    {
                        var f = (from s in db.User_Setups where s.User_ID == Convert.ToInt32(txtID.Text) && s.Company_ID == logIn.company select s);
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
                        picUser.Image = Image.FromStream(stream);


                    }

                    string s1 = d.BU_Allowed;
                    string[] values = s1.Split(',');
                    for (int j = 0; j < values.Length; j++)
                    {
                        values[j] = values[j].Trim();
                        string m = values[j].ToString();
                        for (int i = 0; i < lstBusinessUnits.Items.Count; i++)
                        {
                            if (lstBusinessUnits.Items[i].ToString() == m)
                            {
                                lstBusinessUnits.SetItemChecked(i, true);
                            }
                     
                        }

                    }
                    string s2 = d.FY_Allowed;
                    string[] values1 = s2.Split(',');
                    for (int j = 0; j < values1.Length; j++)
                    {
                        values1[j] = values1[j].Trim();
                        string m = values1[j].ToString();
                        for (int i = 0; i < lstFinYear.Items.Count; i++)
                        {
                            if (lstFinYear.Items[i].ToString() == m)
                            {
                                lstFinYear.SetItemChecked(i, true);
                            }
                            //else
                            //{
                            //    lstModules.SetItemChecked(i, false);
                            //}

                        }
                    }
                }


                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                    picUser.Image = img;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
