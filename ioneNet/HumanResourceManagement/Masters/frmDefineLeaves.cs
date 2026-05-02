using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Linq.SqlClient;
using System.Configuration;
using Syncfusion.Windows.Forms.Tools;
using Ione_DAL;
namespace ioneNet.HumanResourceManagement.Masters
{
    public partial class frmDefineLeaves : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        int Creation_Company = logIn.company;

        public frmDefineLeaves()
        {
            InitializeComponent();
        }

        private void dgcity_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        // // // // City Names Grid Binding
        public void bindGroups()
        {
            try
            {
                var p = (from s in db.HR_Leaves_Masters where s.Status == 1 && s.Company_ID == logIn.company


                         select new
                         {
                             ID=s.id,
                             Leave_Name = s.Leave_Name,
                             s.Short_Name


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

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtGroupName.Text == string.Empty)
                {
                    MessageBox.Show("Leave Name should Not be Empty", "Leave Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtGroupName.Focus();
                    return;
                }
                else if (txtGroupShortName.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter Leave Shortname", "Leave Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtGroupShortName.Focus();
                    return;
                }
              
                else
                {
                    Save();
                    
                    bindGroups();// // // After Saving The City Details Bind City Grid                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Leave Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        public void Save()
        {
            try
            {
               
                    //if ((from u in db.Product_Groups where u.ID == Convert.ToInt32(txtGroupID.Text) && u.Company_ID == logIn.company select u).Count() > 0)
                    if(txtGroupID.Text !="")
                {

                            //if (frmGate.Modify.Contains(this.Text))
                            //{

                            var c = db.HR_Leaves_Masters.Where(w => w.id == Convert.ToInt32(txtGroupID.Text)).FirstOrDefault();
                        {
                            //c.ID = Convert.ToInt32(txtCityId.Text.ToString());
                            c.Leave_Name = (txtGroupName.Text == "") ? "" : (txtGroupName.Text);
                            c.Short_Name = (txtGroupShortName.Text == "") ? "" : (txtGroupShortName.Text);
                            c.Allotment = cmbCalculation.Text;
                            c.No_of_leaves_Per_Year = Convert.ToDecimal(txtBasicPer.Text);
                            c.Encash_Limit = Convert.ToInt32(txtDispOrder.Text);
                            c.Balance_Transfer_Limit = Convert.ToInt32(txtBalTransfer.Text);
                            c.EncashMent_Allowed = chkESI.Checked;
                            c.Balance_Transfer = chkPF.Checked;
                            c.Company_ID = logIn.company;

                            c.Status = 1;
                           
                            //c.cr = linkCreatedBy.Text;
                            //c.Modified_BY = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt"); 
                            db.SubmitChanges();
                            MessageBox.Show("Record Upadated Successfully");
                        clear1();
                        autogen();                        }
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Sorry! You Do not have privileges to Modify City");
                    //}
                }
                else
                {
                    //if (frmGate.Create_menu.Contains(this.Text))
                    //{
                        HR_Leaves_Master ci = new HR_Leaves_Master();
                        ci.Leave_Name = (txtGroupName.Text == "") ? "" : (txtGroupName.Text);
                        ci.Short_Name = (txtGroupShortName.Text == "") ? "" : (txtGroupShortName.Text);
                        ci.Allotment = cmbCalculation.Text;
                        ci.No_of_leaves_Per_Year = Convert.ToDecimal(txtBasicPer.Text);
                        ci.Encash_Limit = Convert.ToInt32(txtDispOrder.Text);
                        ci.Balance_Transfer_Limit = Convert.ToInt32(txtBalTransfer.Text);
                        ci.EncashMent_Allowed = chkESI.Checked;
                        ci.Balance_Transfer = chkPF.Checked;
                    
                        ci.Status = 1;                    
                        ci.Company_ID = logIn.company;
                        db.HR_Leaves_Masters.InsertOnSubmit(ci);
                        db.SubmitChanges();
                        MessageBox.Show("Record Saved Successfully");
                    clear1();
                    bindGroups();
                    //autogen();
                    //}
                    //else
                    //{
                    //    MessageBox.Show("Sorry! You Do not have privileges to Save City");
                    //}

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

        private void frmCityMaster_Load(object sender, EventArgs e)
        {
           
            linkCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            linkModifiedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

                        
            txtGroupName.Focus();
            bindGroups();
            bindmethod();
            //autogen();      
        }
        public void autogen()
        {
            //var result = db.Sp_autoincrement_ProdGroup_Master(logIn.company);
            //txtGroupID.Text = result.FirstOrDefault().Prod_Group_ID;
        }
        public void bindmethod()
        {
            try
            {

                //SqlCommand cmd = new SqlCommand("select  Distinct Prod_Group_Name,Prod_Group_ID from Product_Groups WHERE [Company_ID] = @CompID", con);
                //cmd.Parameters.AddWithValue("@CompID", logIn.company);

                //SqlDataAdapter da = new SqlDataAdapter(cmd);
                //DataSet ds = new DataSet();
                //da.Fill(ds, "t");
                //DataRow drow = ds.Tables["t"].NewRow();
                //drow["Prod_Group_Name"] = "Primary";
                //ds.Tables["t"].Rows.InsertAt(drow, 0);
                //cmbMainGroup.DataSource = ds.Tables["t"];
                //cmbMainGroup.DisplayMember = "Prod_Group_Name";
                //cmbMainGroup.ValueMember = "Prod_Group_ID";
                //cmbMainGroup.SelectedIndex = 0;


                //var bindMainGroups = (from m in db.HR_Employee_Groups
                //                      where m.Company_ID == logIn.company && m.Status_ID == 1
                //                      select new
                //                      {
                //                          m.Prod_Group_Name,
                //                          m.ID,
                //                      }).ToList();

                //if (bindMainGroups.Count > 0)
                //{
                //    cmbMainGroup.DisplayMember = "Prod_Group_Name";
                //    cmbMainGroup.ValueMember = "ID";
                //    cmbMainGroup.DataSource = bindMainGroups;

                //}

                //var bindProdTypes = (from m in db.Attributes_Prod_Types
                //                      where m.Company_ID == logIn.company 
                //                      select new
                //                      {
                //                          m.Prod_Type,
                //                          m.Prod_Type_Id,
                //                      }).ToList();

                //if (bindProdTypes.Count > 0)
                //{
                //    comboBox1.DisplayMember = "Prod_Type";
                //    comboBox1.ValueMember = "Prod_Type_Id";
                //    comboBox1.DataSource = bindProdTypes;

                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvcity_CellClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void clear1()
        {
            foreach (Control x in this.Controls)
            {
                foreach (Control d in panel1.Controls)
                {
                    if (d is TextBox)
                        (d as TextBox).Clear();
                    if (d is ComboBox)
                        (d as ComboBox).SelectedIndex = -1;
                    if (d is CheckBox)
                        (d as CheckBox).Checked = false;
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
           try
           {
                if (txtGroupID.Text != "")
                {

                        DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            int ProdID;
                            ProdID = Convert.ToInt32(txtGroupID.Text);
                            SqlCommand cmd1 = new SqlCommand("delete  from [HR_Salary_HeadsInfo] where id =@ProdID abd Company_ID = @compID", con);
                            cmd1.Parameters.AddWithValue("@ProdID", ProdID);
                            cmd1.Parameters.AddWithValue("@compID", logIn.company);

                        if (con.State != ConnectionState.Open)
                                con.Open();
                            //con.Open();
                            cmd1.ExecuteNonQuery();
                            con.Close();
                            MessageBox.Show("Head Deleted Successfully");
                        }

                }
                else
                {
                    MessageBox.Show("Please Select Head to Delete");
                }
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146232060)
                {
                    MessageBox.Show("The Master Record Already in Use, Cannot Be Deleted");
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }                
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            clear1();
            bindGroups();
        }

        private void dgvcity_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtGroupID.Text = dgvcity.Rows[dgvcity.CurrentRow.Index].Cells["ID"].Value.ToString();                
                
                var d = (from po in db.HR_Leaves_Masters
                             // join s in db.Sales_Men_Informations on po.Salesmen_Code equals s.Salesmen_Code
                         where
                         po.id == Convert.ToInt32(txtGroupID.Text) //&& po.Creation_Company == Creation_Company
                         select new
                         {
                             po.Leave_Name,
                             po.Short_Name,                            
                             po.Allotment,
                             po.No_of_leaves_Per_Year,
                             po.Encash_Limit,
                             po.EncashMent_Allowed,
                             po.Balance_Transfer,
                             po.Balance_Transfer_Limit
                         }).ToList();
                if (d.Count > 0)
                {
                  
                    txtGroupName.Text = d[0].Leave_Name;
                    txtBasicPer.Text = d[0].No_of_leaves_Per_Year.ToString();
                    txtGroupShortName.Text = d[0].Short_Name;
                    cmbCalculation.Text = d[0].Allotment;
                    txtDispOrder.Text = d[0].Encash_Limit.ToString();
                    chkESI.Checked = false;
                    chkPF.Checked = false;
                    txtBalTransfer.Text = d[0].Balance_Transfer_Limit.ToString();
                    if(d[0].EncashMent_Allowed ==true)
                    {
                        chkPF.Checked = true;
                    }

                    if (d[0].Balance_Transfer ==true)
                    {
                        chkESI.Checked = true;
                    }


                }               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtGroupname_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtGroupName.Text != "")
                {
                    if ((from u in db.Product_Groups where u.Prod_Group_Name == txtGroupName.Text && u.Company_ID == logIn.company select u).Count() > 0)
                    {
                        MessageBox.Show("Group Name Cannot Be Duplicate");
                        txtGroupName.Text = "";
                        txtGroupName.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmCityMaster_KeyDown(object sender, KeyEventArgs e)
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

                if (e.Control && e.KeyCode == Keys.D)
                    btnDelete_Click(sender, e);

                if (e.Alt && e.KeyCode == Keys.F4)
                    btnClose_Click(sender, e);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                var p = (from s in db.ProductGroup_Lists
                         where s.Company_ID == logIn.company  && SqlMethods.Like(s.Prod_Group_Name,"%"+txtSearch.Text +"%") 


                         select new
                         {
                             ID = s.ID,
                             Group_Name = s.Prod_Group_Name,
                             Main_Group = s.MainGroup


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

        private void txtSearch_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                btnFind_Click(sender, e);
            }
        }
    }
}
