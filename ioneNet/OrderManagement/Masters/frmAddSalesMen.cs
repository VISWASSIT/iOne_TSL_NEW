using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Drawing.Imaging;
using System.IO;
using System.Configuration;
using Ione_DAL;
namespace ioneNet.OrderManagement
{
    public partial class frmAddSalesMen : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmAddSalesMen()
        {
            InitializeComponent();
        }


        public void BindTeamMem()
        {
            try
            {
                
                    var da = (from comp in db.Sales_Men_Informations
                              where  comp.Company_ID == logIn.company && comp.Status == 1
                              select new
                              {
                                  comp.Sales_Executive_Name,
                                  comp.Id,
                                  comp.Team_Member,
                              }).ToList();

                    if (da.Count > 0)
                    {
                        cmbRept.DataSource = da;
                        cmbRept.DisplayMember = "Sales_Executive_Name";
                        cmbRept.ValueMember = "Id";
                        // cmbRept.Text = cmbRept.SelectedItem.ToString();
                    }
                    else
                    {

                    }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void BindArea()
        {
            try
            {
                // CheckedListBox.CheckedItemCollection checkedItemCollection = chkItemsGroupList.CheckedItems;
                //for (int i = chkArea.Items.Count; i > 0; i--)
                //{
                //    chkArea.Items.Remove(chkArea.Items[i - 1]);
                //}
                cmbArearesp.Items.Clear();
                var da = (from comp in db.Attributes_Datas
                          where comp.Head_Name == "Sales Office"
                          select new
                          {
                              comp.Descr,
                              comp.ID
                             
                          }).ToList();

                if (da.Count > 0)
                {
                    cmbArearesp.DataSource = da;
                    cmbArearesp.DisplayMember = "Descr";
                    cmbArearesp.ValueMember = "ID";
                    // cmbRept.Text = cmbRept.SelectedItem.ToString();
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void bindedit()
        {


            //txtSalesCode.Text = CRM.frmSalesForce.comp_id;
            //txtExecutiveName.Text = CRM.frmSalesForce.comp_name;
            
                var d = (from po in db.Sales_Men_Informations
                         where
                         po.Id == Convert.ToInt32(txtID.Text)
                         select new
                         {
                             po.Sales_Exec_Image_Path,
                             po.Salesmen_Code,
                             po.Sales_Executive_Name,
                             po.Designation,
                             po.Mobile_No_Primary,
                             po.Team_Level,
                             po.Team_Member,                            
                             po.Status,
                             po.Email_Id,
                             po.Created_By,                            
                             po.Area_Responsible,
                             po.Modified_By,
                             po.ERP_User_ID
                             

                         }).ToList();
                if (d.Count > 0)
                {
                    txtSalesCode.Text = d[0].Salesmen_Code;
                    txtExecutiveName.Text = d[0].Sales_Executive_Name;
                    cmbDesignation.Text = d[0].Designation;

                    lnkus1.Text = d[0].Created_By;
                    lnkus2.Text = d[0].Modified_By;                   
                    cmbArearesp.Text = d[0].Area_Responsible;
                    
                    

                    
                    // if (d[0].Fixed_Qty == "1")
                    //{
                    //    chkFixedQty.Checked = true;
                    //}
                    bindDroupDown_Lookup();
                    cmbERPUser.SelectedValue = d[0].ERP_User_ID;
                    cmbRept.SelectedValue = d[0].Team_Member;
                    lnkus1.Text = d[0].Created_By;
                   
                    lnkus2.Text = d[0].Modified_By;
                    
                    txtMobile.Text = d[0].Mobile_No_Primary;
                   
                    cmbStatus.SelectedValue = d[0].Status;
                    txtEMail.Text = d[0].Email_Id;
                    if (d[0].Sales_Exec_Image_Path != null)
                    {
                        var img = (from s in db.Sales_Men_Informations where s.Salesmen_Code == txtSalesCode.Text 
                                  // && s.Creation_Company == frmLogin.D_CreationComp
                                   select s);
                        SqlCommand cmd1 = (SqlCommand)db.GetCommand(img);
                        SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                        DataTable dt = new DataTable();
                        DataSet ds = new DataSet("MyImages");
                        byte[] MyData = new byte[0];
                        da1.Fill(ds, "MyImages");
                        DataRow myRow;
                        myRow = ds.Tables["MyImages"].Rows[0];
                        MyData = (byte[])myRow["Sales_Exec_Image_Path"];
                        MemoryStream stream = new MemoryStream(MyData);
                        picEmpImage.Image = Image.FromStream(stream);
                        picEmpImage.SizeMode = PictureBoxSizeMode.StretchImage;
                    }
                    
                    //w.Document = new System.Data.Linq.Binary(bytes);

                }
                else
                {
                }



            
        }

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
            }
            // chkTeamMember.Items.Clear();
            picEmpImage.Image = null;
            
            txtExecutiveName.Focus();
            // autogen();
            // fill_Tree2();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtSalesCode.Text == string.Empty)
                {
                    MessageBox.Show("SalecExecutive Code should Not be Empty", "SalesMen Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtSalesCode.Focus();
                    return;

                }
                else if (txtExecutiveName.Text == string.Empty)
                {
                    MessageBox.Show("SalecExecutive Name should Not be Empty", "SalesMen Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtExecutiveName.Focus();
                    return;

                }
                else if (cmbDesignation.Text == string.Empty)
                {
                    MessageBox.Show("Designation should not empty", "SalesMen Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbDesignation.Focus();
                    return;

                }
                else if (txtMobile.Text == "")
                {
                    MessageBox.Show("Primary Mobile No should Not be Empty ", "SalesMen Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtMobile.Focus();
                    return;

                }
                //if (chkauth.Checked != true && cmbRept.Text == "")
                //{
                //    MessageBox.Show("Reporting To should Not be Empty", "SalesMen Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    cmbRept.Focus();
                //    return;
                //}

                else if (cmbStatus.Text == "")
                {
                    MessageBox.Show("Status should Not be Empty ", "SalesMen Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbStatus.Focus();
                    return;

                }
                else
                {
                    Save();
                    clear1();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SalesMen Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        string StrRoute;
        public void Save()
        {
            try
            {
                if (txtID.Text != "")
                {
                    
                        var c = db.Sales_Men_Informations.Where(w => w.Id == Convert.ToInt32(txtID.Text) ).FirstOrDefault();
                        {
                            c.Salesmen_Code = txtSalesCode.Text;
                            c.Sales_Executive_Name = (txtExecutiveName.Text == "") ? "" : (txtExecutiveName.Text);
                            c.Designation = cmbDesignation.Text;
                            c.Mobile_No_Primary = (txtMobile.Text == "") ? "" : txtMobile.Text;
                            c.Email_Id = (txtEMail.Text == "") ? "" : (txtEMail.Text);
                            c.Status = Convert.ToInt32(cmbStatus.SelectedValue);
                            c.Area_Responsible = cmbArearesp.Text;
                            c.Team_Member = Convert.ToInt32(cmbRept.SelectedValue);
                        c.ERP_User_ID = Convert.ToInt32(cmbERPUser.SelectedValue);
                        string str1 = "";
                            if (lblFilepath.Text != "")
                            {
                                string filePath = lblFilepath.Text;
                                string filename = Path.GetFileName(filePath);
                                FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                                BinaryReader br = new BinaryReader(fs);
                                Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                                br.Close();
                                fs.Close();

                                str1 = str1 + filePath;
                                //c.Attach_File_Name = lnklblFileName.Text;
                                //c.Attache_Doc_Path = lblFilepath.Text;
                                //c.Attach_Document = bytes;
                            }
                            c.Modified_By = logIn.username;
                           
                            if (picEmpImage.Image != null)
                            {
                                Image img = picEmpImage.Image;
                                MemoryStream ms = new MemoryStream();
                                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                byte[] bytes = ms.ToArray();
                                c.Sales_Exec_Image_Path = bytes;
                            }
                            else
                            {
                                c.Sales_Exec_Image_Path = null;
                            }

                            db.SubmitChanges();
                            MessageBox.Show("Record Upadated Successfully");

                        }
                    }
                    
                
                else
                {

                  
                    Sales_Men_Information ci = new Sales_Men_Information();

                   

                    ci.Salesmen_Code = txtSalesCode.Text;
                    ci.Sales_Executive_Name = (txtExecutiveName.Text == "") ? "" : (txtExecutiveName.Text);
                    ci.Designation = cmbDesignation.Text;
                    ci.Mobile_No_Primary = (txtMobile.Text == "") ? "" : txtMobile.Text;
                    ci.Email_Id = (txtEMail.Text == "") ? "" : (txtEMail.Text);
                    ci.Status = Convert.ToInt32(cmbStatus.SelectedValue);
                    ci.Area_Responsible = cmbArearesp.Text;
                    ci.Team_Member = Convert.ToInt32(cmbRept.SelectedValue);
                    ci.ERP_User_ID = Convert.ToInt32(cmbERPUser.SelectedValue);
                    ci.Company_ID = logIn.company;
                    ci.Created_By = logIn.username;
                    ci.Modified_By = logIn.username;

                    if (picEmpImage.Image != null)
                    {
                        Image img = picEmpImage.Image;
                        MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] bytes = ms.ToArray();
                        ci.Sales_Exec_Image_Path = bytes;
                    }
                    else
                    {
                        ci.Sales_Exec_Image_Path = null;
                    }
                    string str1 = "";
                    //if (lblFilepath.Text != "")
                    //{
                    //    string filePath = lblFilepath.Text;
                    //    string filename = Path.GetFileName(filePath);
                    //    FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                    //    BinaryReader br = new BinaryReader(fs);
                    //    Byte[] bytes = br.ReadBytes((Int32)fs.Length);
                    //    br.Close();
                    //    fs.Close();

                    //    str1 = str1 + filePath;
                    //    ci.Attach_File_Name = lnklblFileName.Text;
                    //    ci.Attache_Doc_Path = lblFilepath.Text;
                    //    ci.Attach_Document = bytes;
                    //}
                    db.Sales_Men_Informations.InsertOnSubmit(ci);
                    db.SubmitChanges();
                    MessageBox.Show("Record Saved Successfully");
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
        string a;
        private void frmAddSalesMen_Load(object sender, EventArgs e)
        {
            binddesignation();
            //a = CRM.frmSalesForce.var;
            BindArea();
            bindDroupDown_Lookup();
            bindSaleMenList();

            var da = (from comp in db.Attributes_Datas
                      where comp.Head_Name == "Status Master"
                      select new
                      {
                          comp.Descr,
                          comp.ID

                      }).ToList();

            if (da.Count > 0)
            {
                cmbStatus.DataSource = da;
                cmbStatus.DisplayMember = "Descr";
                cmbStatus.ValueMember = "ID";
                // cmbRept.Text = cmbRept.SelectedItem.ToString();
            }

            lnkus1.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                lnkus2.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                 //autogen();
            
                //bindedit();
           
        }
        public void binddesignation()
        {
            try
            {

                var da = (from comp in db.HR_Emp_DesigInfos
                          where comp.Company_ID == logIn.company
                          select new
                          {
                              comp.Designation,
                              comp.ID
                          }
                          ).Distinct().ToList();
                if (da.Count > 0)
                {

                    cmbDesignation.DataSource = da;
                    cmbDesignation.DisplayMember = "Designation";
                    cmbDesignation.ValueMember = "ID";
                    cmbDesignation.Text = cmbDesignation.SelectedValue.ToString();
                    cmbDesignation.SelectedIndex = -1;
                }
                if (cmbDesignation.Items.Count > 0)
                    cmbDesignation.SelectedIndex = -1;
                if (cmbDesignation.Items.Count == 1)
                    cmbDesignation.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void bindSaleMenList()
        {
            try
            {
                var p = (from s in db.Sales_Men_Informations
                         where s.Company_ID == logIn.company 


                         select new
                         {
                             ID = s.Id,
                             s.Salesmen_Code,
                             s.Sales_Executive_Name


                         }
                        );
                SqlCommand cmd1 = (SqlCommand)db.GetCommand(p);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);

                if (dt1.Rows.Count >= 0)
                {
                    dgvcity.DataSource = dt1;
                }
                else
                {
                }
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
                var SM = (from m in db.Sales_Men_Informations where m.Company_ID == logIn.company select new { m.Id, m.Sales_Executive_Name }).Distinct().ToList();
                if (SM.Count > 0)
                {
                    cmbRept.DataSource = SM;
                    cmbRept.ValueMember = "Id";
                    cmbRept.DisplayMember = "Sales_Executive_Name";
                    cmbRept.SelectedIndex = -1;
                }

                var US = (from m in db.User_Setups where m.Company_ID == logIn.company select new { m.User_ID, m.User_Name }).Distinct().ToList();
                if (US.Count > 0)
                {
                    cmbERPUser.DataSource = US;
                    cmbERPUser.ValueMember = "User_ID";
                    cmbERPUser.DisplayMember = "User_Name";
                    cmbERPUser.SelectedIndex = -1;
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

                private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            //frmSalesForce Fm1 = new frmSalesForce();

            //frmSalesForce fc = (frmSalesForce)Application.OpenForms["frmSalesForce"];
            //if (fc != null)
            //    fc.Close();

            //Fm1.Show();
            //Fm1.BringToFront();
            //Fm1.Activate();
        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
            try
            {

                OpenFileDialog open = new OpenFileDialog();
                if (open.ShowDialog() == DialogResult.OK)
                {
                    Image img = new Bitmap(open.FileName);
                    picEmpImage.Image = img;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            clear1();
        }

        private void txtMobile_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (txtMobile.Text != "" )
            {
                double parsedValue;
                if (!double.TryParse(txtMobile.Text, out parsedValue))
                {
                    txtMobile.Text = "";
                    txtMobile.Focus();
                    MessageBox.Show("Please Enter Numbers Only");
                }
            }

        }

        private void btnAddArea_Click(object sender, EventArgs e)
        {
            //if (frmGate.Create_menu.Contains("Manage Districts"))
            //{
            //    BindArea();
            //    Administrator.frmAreaMaster FmrOrd = new Administrator.frmAreaMaster();
            //    FmrOrd.MdiParent = this.ParentForm;
            //    FmrOrd.Show();
            //}
            //else
            //{
            //    MessageBox.Show("Sorry! You Do not have privileges to Create Districts");
            //}
        }

        private void cmbTeamLevel_Leave(object sender, EventArgs e)
        {
            //string t = cmbTeamLevel.Text;

            //if (cmbTeamLevel.Text == "")
            //{
            //    MessageBox.Show("Please Select Valid Lavel");
            //    cmbTeamLevel.Text = "";
            //    cmbTeamLevel.Focus();
            //}
            //else
            //{
            //    //MessageBox.Show("Please Select Valid Lavel"");
            //    //cmbTeamLevel.Text = "";
            //    //cmbTeamLevel.Focus();
            //}

            //if (cmbTeamLevel.SelectedItem == null && t != "")
            //{
            //    MessageBox.Show("Please Select Valid Lavel");

            //    cmbTeamLevel.Text = "";
            //    cmbTeamLevel.Focus();
            //}
            //else
            //{

            //    if (cmbTeamLevel.Text != "")
            //    {
            //        //Bi/*n*/dTeamMem();
            //    }
            //}
        }

        private void btnAtchDoc_Click(object sender, EventArgs e)
        {
            OpenFileDialog fDialog = new OpenFileDialog();
            fDialog.Title = "Select file to be upload";
            //fDialog.Filter = "PDF Files|*.pdf|All Files|*.*";
            fDialog.Filter = "PDF Files|*.pdf|All Files|*.*";
            if (fDialog.ShowDialog() == DialogResult.OK)
            {
                lblFilepath.Text = fDialog.FileName.ToString();
                lnklblFileName.Text = Path.GetFileName(fDialog.FileName);

            }
        }

        private void lnklblFileName_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void txtEMail_Validating(object sender, CancelEventArgs e)
        {
            System.Text.RegularExpressions.Regex rEmail = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");

            if (txtEMail.Text.Length > 0 && txtEMail.Text.Trim().Length != 0)
            {
                if ((!rEmail.IsMatch(txtEMail.Text.Trim())))
                {
                    MessageBox.Show("Please Enter Valid Email Id");
                    txtEMail.SelectAll();
                    e.Cancel = true;
                }

            }
        }

        private void cmbStatus_Leave(object sender, EventArgs e)
        {

            //string t = cmbStatus.Text;

            //if (cmbStatus.SelectedItem == null && t != "")
            //{
            //    MessageBox.Show("Please Select Valid Status");

            //    cmbStatus.Text = "";
            //    cmbStatus.Focus();
            //}
            //else
            //{

            //    if (cmbStatus.Text == "De_Active")
            //    {
            //        try
            //        {

            //            if ((from obj in db.Sales_Men_Informations
            //                 join pm in db.Customer_Information_Masters on new { a = obj.Area_Responsible, b = obj.Creation_Company } equals new { a = pm.Area_Name, b = pm.Creation_Company }
            //                // where obj.Creation_Company == frmLogin.D_CreationComp
            //                 select new
            //                 {
            //                     obj.Creation_Company
            //                 }).Count() > 0)
            //            {
            //                MessageBox.Show("This Salesmen Assigned to Some Customers So it Doesn't De_Active");
            //                cmbStatus.Text = "Active";
            //            }

            //            else
            //            {

            //            }


            //        }
            //        catch (Exception ex)
            //        {

            //        }
            //    }
            //}
        }
        
        private void txtExecutiveName_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtExecutiveName.Text != "")
                {
                    
                    if ((from u in db.Sales_Men_Informations where u.Sales_Executive_Name == txtExecutiveName.Text && u.Salesmen_Code != txtSalesCode.Text 
                        // && u.Creation_Company==frmLogin.Creation_Company
                         select u).Count() > 0)
                    {
                        MessageBox.Show("ExecutiveName Already Exists..,Please Try Another One");
                        txtExecutiveName.Text = "";
                        txtExecutiveName.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmAddSalesMen_Activated(object sender, EventArgs e)
        {
            if (a == "1" && cmbArearesp.Text == "")
                BindArea();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void cmbArearesp_Leave(object sender, EventArgs e)
        {
            //var da = (from obj in db.Attributes_Datas
            //          where obj.Area_Name == cmbArearesp.Text
            //          //&& obj.Creation_Company == frmLogin.D_CreationComp
            //          select obj).ToList();
            //if (da.Count > 0)
            //{

            //}
            //else
            //{
            //    if (cmbArearesp.Text == "")
            //    {

            //    }
            //    else
            //    {
            //        MessageBox.Show("Please select Valid Area Name...");
            //        cmbArearesp.Text = "";
            //        cmbArearesp.Focus();
            //    }
            //}

        }

        private void frmAddSalesMen_KeyDown(object sender, KeyEventArgs e)
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

                if (e.Control && e.KeyCode == Keys.Z)
                    btnClose_Click(sender, e);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void cmbDesignation_Leave(object sender, EventArgs e)
        {
            //var da = (from obj in db.User_Role_Details
            //          where obj.Role_Name == cmbDesignation.Text 
            //          //&& obj.Creation_Company == frmLogin.D_CreationComp
            //          select obj).ToList();
            //if (da.Count > 0)
            //{

            //}
            //else
            //{
            //    if (cmbDesignation.Text == "")
            //    {

            //    }
            //    else
            //    {
            //        MessageBox.Show("Please select Valid Designation...");
            //        cmbDesignation.Text = "";
            //        cmbDesignation.Focus();
            //    }
            //}

        }

        private void chkauth_CheckedChanged(object sender, EventArgs e)
        {

        }
        string str, str1;

        private void chkArea_Validating(object sender, CancelEventArgs e)
        {
            //if (chkArea.CheckedItems.Count > 0)
            //{
            //}
            //else
            //{
            //    MessageBox.Show("Please Select Atleast One Area Name--");
            //    chkArea.Focus();
            //}
        }

        private void dgvcity_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtID.Text = dgvcity.Rows[dgvcity.CurrentRow.Index].Cells["ID"].Value.ToString();
                bindedit();              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvcity_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void chkArea_SelectedIndexChanged(object sender, EventArgs e)
        {
                   }
    }
}




