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
using Ione_DAL;
namespace ioneNet.Masters
{
    public partial class CostCenters : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        int Creation_Company = logIn.company;

        public CostCenters()
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
        private void PopulateTreeView(int parentId, TreeNode parentNode)
        {
            try
            {
                TreeNode childNode;
                SqlCommand cmd = new SqlCommand("select * from [CostCenter_Master] WHERE [Company_ID] = @CompID and bu_id =@buid", con);
                cmd.Parameters.AddWithValue("@CompID", logIn.company);
                cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);

                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dap.Fill(dt);
                //twAccounts.Nodes.Clear();
                //foreach (DataRow dr in dt.Rows)
                foreach (DataRow dr in dt.Select("[SubCostCenter_ID]=" + parentId))
                {
                    TreeNode t = new TreeNode();
                    t.Text = dr["CostCenter_Name"].ToString();
                    t.Name = dr["ID"].ToString();
                    t.Tag = dt.Rows.IndexOf(dr);
                    if (parentNode == null)
                    {
                        twGroups.Nodes.Add(t);
                        childNode = t;
                    }
                    else
                    {
                        parentNode.Nodes.Add(t);
                        childNode = t;
                    }

                    PopulateTreeView(Convert.ToInt32(dr["id"].ToString()), childNode);
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
                    MessageBox.Show("Cost Center Name should Not be Empty", "CC Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtGroupName.Focus();
                    return;
                }
                else
                if (checkBox1.Checked == false)
                {
                    if (cmbMainGroup.Text == string.Empty)
                    {
                        MessageBox.Show("Please Select Main Group..", "CC Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cmbMainGroup.Focus();
                        return;
                    }
                    else
                    {
                        Save();

                        //bindGroups();// // // After Saving The City Details Bind City Grid                   
                    }
                }
                else
                {
                    Save();
                }
                //else if (txtStateCode.Text == string.Empty)
                //{
                //    MessageBox.Show("Enter State Code", "City Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    txtStateCode.Focus();
                //    return;
                //}
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "City Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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

                            var c = db.CostCenter_Masters.Where(w => w.ID == Convert.ToInt32(txtGroupID.Text)).FirstOrDefault();
                        {
                            //c.ID = Convert.ToInt32(txtCityId.Text.ToString());
                            c.CostCenter_Name = (txtGroupName.Text == "") ? "" : (txtGroupName.Text);
                            if (checkBox1.Checked == false)
                            {
                                c.SubCostCenter_ID = Convert.ToInt32(cmbMainGroup.SelectedValue.ToString());
                            }
                            else
                            {
                                c.SubCostCenter_ID = 0;
                            }
                            c.SubCostCenter = cmbMainGroup.Text;
                            c.IsMain = checkBox1.Checked;
                            c.Dept_ID = Convert.ToInt32(cmbDepartment.SelectedValue.ToString());
                             c.BU_ID = logIn.BU_ID;
                            c.Created_By = linkCreatedBy.Text;
                            c.Modified_BY = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt"); 
                            db.SubmitChanges();
                            MessageBox.Show("Record Upadated Successfully");
                        clear1();                                              }
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
                        CostCenter_Master ci = new CostCenter_Master();
                        ci.CostCenter_Name = (txtGroupName.Text == "") ? "" : (txtGroupName.Text);
                        ci.IsMain = checkBox1.Checked;
                    if (checkBox1.Checked == false)
                    {
                        ci.SubCostCenter_ID = Convert.ToInt32(cmbMainGroup.SelectedValue.ToString());
                    }
                    else
                    {
                        ci.SubCostCenter_ID = 0;
                    }
                    ci.SubCostCenter = (cmbMainGroup.Text == "") ? "" : (cmbMainGroup.Text); 
                        
                        ci.Dept_ID = Convert.ToInt32(cmbDepartment.SelectedValue.ToString());
                        ci.Created_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                        ci.Modified_BY = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                        ci.Company_ID = logIn.company;
                        ci.BU_ID = logIn.BU_ID;
                        db.CostCenter_Masters.InsertOnSubmit(ci);
                        db.SubmitChanges();
                        MessageBox.Show("Record Saved Successfully");
                    clear1();
                    PopulateTreeView(0, null);
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
            PopulateTreeView(0, null);
            bindmethod();
            bindDepts();
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
                  var bindMainGroups = (from m in db.CostCenter_Masters
                                      where m.Company_ID == logIn.company && m.BU_ID == logIn.BU_ID
                                      select new
                                      {
                                          m.CostCenter_Name,
                                          m.ID,
                                      }).ToList();

                if (bindMainGroups.Count > 0)
                {
                    cmbMainGroup.DisplayMember = "CostCenter_Name";
                    cmbMainGroup.ValueMember = "ID";
                    cmbMainGroup.DataSource = bindMainGroups;

                }                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void bindDepts()
        {
            try
            {                
                var Dept = (from m in db.Department_Masters
                            where m.Company_ID == logIn.company && m.BU_ID == logIn.BU_ID
                            select new
                            {
                                m.Dept_Name,
                                m.Id,
                            }).ToList();

                if (Dept.Count > 0)
                {
                    cmbDepartment.DisplayMember = "Dept_Name";
                    cmbDepartment.ValueMember = "Id";
                    cmbDepartment.DataSource = Dept;

                }

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
                            SqlCommand cmd1 = new SqlCommand("delete  from [CostCenter_Masters] where id =@ProdID abd Company_ID = @compID", con);
                            cmd1.Parameters.AddWithValue("@ProdID", ProdID);
                            cmd1.Parameters.AddWithValue("@compID", logIn.company);

                        if (con.State != ConnectionState.Open)
                                con.Open();
                            //con.Open();
                            cmd1.ExecuteNonQuery();
                            con.Close();
                            MessageBox.Show("Cost Center Deleted Successfully");
                        }

                }
                else
                {
                    MessageBox.Show("Please Select Cost Center to Delete");
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
            PopulateTreeView(0, null);
        }

        private void dgvcity_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //txtGroupID.Text = dgvcity.Rows[dgvcity.CurrentRow.Index].Cells["ID"].Value.ToString();                
                
                var d = (from po in db.ProductGroup_Lists
                             // join s in db.Sales_Men_Informations on po.Salesmen_Code equals s.Salesmen_Code
                         where
                         po.ID == Convert.ToInt32(txtGroupID.Text) //&& po.Creation_Company == Creation_Company
                         select new
                         {
                             po.Prod_Group_Name,
                             po.MainGroup,
                             po.status,
                             po.Group_PreFix,
                             po.Created_By,
                             po.Modified_BY,                        

                         }).ToList();
                if (d.Count > 0)
                {
                  
                    txtGroupName.Text = d[0].Prod_Group_Name;
                    cmbMainGroup.Text = d[0].MainGroup;
                    //txtGroupPrefix.Text = d[0].Group_PreFix;
                    linkCreatedBy.Text = d[0].Created_By;
                    linkModifiedBy.Text = d[0].Modified_BY;                   
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
                    if ((from u in db.CostCenter_Masters where u.CostCenter_Name == txtGroupName.Text && u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID select u).Count() > 0)
                    {
                        MessageBox.Show("Cost Center Cannot Be Duplicate");
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
                             ID = s.Prod_Group_ID,
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
                    //dgvcity.DataSource = dt1;
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

        private void button1_Click(object sender, EventArgs e)
        {
            ioneNet.MaterialManagement.Departments compFrm = new ioneNet.MaterialManagement.Departments();
            compFrm.ShowDialog();
        }

        private void cmbDepartment_Enter(object sender, EventArgs e)
        {
            bindDepts();
        }
    }
}
