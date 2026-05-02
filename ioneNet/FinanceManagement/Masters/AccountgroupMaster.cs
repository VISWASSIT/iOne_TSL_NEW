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
using System.Configuration;
using Ione_DAL;

namespace ioneNet.FinanaceManagement
{
    public partial class AccountGroups : Form
    {

          DataClasses1DataContext db = new DataClasses1DataContext();
        //string objConfig = ConfigurationManager.ConnectionStrings["LaksanaIndSysConnectionString"].ToString();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public AccountGroups()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAccGroupName.Text == "")
                {
                    MessageBox.Show("Please Enter Account Group Name ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtAccGroupName.Focus();
                    return;
                }
                else if (cmbGroupType.Text == "")
                {
                    MessageBox.Show("Please Select Account GroupType ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbSubGroup.Focus();
                    return;
                }

                else if (cmbSubGroup.Text == "")
                {
                    MessageBox.Show("Please Select Account SubGroup ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbSubGroup.Focus();
                    return;
                }
                save();
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void SubGroupLoad()
        {
            // cmbSubGroup.SelectedIndex = 0;
            // var sub= (from u in db.AccountGroups where u.Company == AppCode.GlobalAccess.companyName select u.GroupName).

            SqlCommand cmd = new SqlCommand("select  Distinct groupname,ID from accountgroups WHERE [Company_ID] = @CompID", con);
            cmd.Parameters.AddWithValue("@CompID", logIn.company);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataSet ds = new DataSet();
            da.Fill(ds, "t");
            DataRow drow = ds.Tables["t"].NewRow();
            drow["groupname"] = "Primary";
            ds.Tables["t"].Rows.InsertAt(drow, 0);
            cmbSubGroup.DataSource = ds.Tables["t"];
            cmbSubGroup.DisplayMember = "groupname";
            cmbSubGroup.ValueMember = "ID";
            cmbSubGroup.SelectedIndex = 0;
            // cmbSubGroup.DataSource = AccGrName;
          
              
        }
        private void PopulateTreeView(int parentId, TreeNode parentNode)
        {
            try
            {
                TreeNode childNode;
                SqlCommand cmd = new SqlCommand("select * from [AccountGroups] WHERE [Company_ID] = @CompID", con);
                cmd.Parameters.AddWithValue("@CompID", logIn.company); 

                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dap.Fill(dt);
                //twAccounts.Nodes.Clear();
                //foreach (DataRow dr in dt.Rows)
                foreach (DataRow dr in dt.Select("[SubGroup_ID]=" + parentId))
                {
                    TreeNode t = new TreeNode();
                    t.Text = dr["GroupName"].ToString();
                    t.Name = dr["id"].ToString();
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
        private void loadGrid()
        {
            try
            {

               var result = (from d in db.AccountGroups select new { d.GroupName, d.GroupType, d.SubGroup }).ToList();
               if (result.Count > 0)
               dgAcGroup.DataSource = result;

            }

            catch ( Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void save()
        {
            try
            {
                if ((from u in db.AccountGroups where u.GroupName == txtAccGroupName.Text && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    //if(AppCode.GlobalAccess.Edit=="Yes")
                    //{ 
                    var d = db.AccountGroups.Where(w => w.GroupName == txtAccGroupName.Text && w.Company_ID == logIn.company).FirstOrDefault();
                    
                        d.GroupType = cmbGroupType.Text;
                        d.SubGroup = cmbSubGroup.Text;
                        if (cmbSubGroup.Text == "Primary")
                        {
                            d.SubGroup_ID = 0;
                        }
                        else
                        {
                            d.SubGroup_ID = Convert.ToInt32(cmbSubGroup.SelectedValue);
                        }
                        d.BalSheetHead = (cmbblgroup.Text == "") ? "" : cmbblgroup.Text;
                        d.PLHead = (cmbplgroup.Text == "") ? "" : cmbplgroup.Text;
                        d.BLGroup = checkBox1.Checked;
                        d.Company_ID = logIn.company;
                    //d.Modified_BY = AppCode.GlobalAccess.UserName;
                    //d.Created_By = Convert.ToDateTime(DateTime.Now.ToString());
                    db.SubmitChanges();
                        MessageBox.Show("Record Updated Successfully");
                        SubGroupLoad();
                        clear();
                    //}
                    //else
                    //  {
                    //    MessageBox.Show("You dont Have Privileges", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    clear();
                    //  }
                }
                else
                {
                    //if (AppCode.GlobalAccess.Add == "Yes")
                    //{
                        // var insert= db.sp_ProductGroupsInsert(txtProdGroupCode.Text, txtProdGroupName.Text, cmbSubGroup.Text, txtSubHeading.Text, txtCompName.Text);

                        AccountGroup p = new AccountGroup();
                        p.GroupName = txtAccGroupName.Text;
                        p.GroupType = cmbGroupType.Text;
                        p.SubGroup = (cmbSubGroup.Text == "") ? "" : cmbSubGroup.Text;
                        if (cmbSubGroup.Text == "Primary")
                        {
                            p.SubGroup_ID =0;
                        }
                        else
                        {
                            p.SubGroup_ID = Convert.ToInt32(cmbSubGroup.SelectedValue);
                        }
                        p.BalSheetHead = (cmbplgroup.Text=="")?"": cmbplgroup.Text;
                        p.PLHead = (cmbplgroup.Text == "") ? "" : cmbplgroup.Text;
                        p.BLGroup = checkBox1.Checked;
                        p.Company_ID = logIn.company;
                    //p.createdBy = AppCode.GlobalAccess.UserName;
                    //p.createdon = Convert.ToDateTime(DateTime.Now.ToString());


                    db.AccountGroups.InsertOnSubmit(p);
                        db.SubmitChanges();
                        MessageBox.Show("Record Saved Successfully");
                        SubGroupLoad();
                        clear();
                    //}
                    //else
                    //{
                    //    MessageBox.Show("You dont Have Privileges", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    clear();
                    //}
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void clear()
        {
        
            txtAccGroupName.Text = "";
            cmbGroupType.Text = "";
            cmbSubGroup.Text = "";
          

            //for (int i = 0; i < dgAcGroup.Rows.Count ; i++)
            //{
            //    dgAcGroup.Rows.RemoveAt(i);
            //    i--;
            //    while (dgAcGroup.Rows.Count == 0)
            //        continue;
            //}
            loadGrid();
            getblgroup();
            getplgroup();
            SubGroupLoad();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                //if (AppCode.GlobalAccess.Edit == "Yes")
                //{                     

                    if (txtAccGroupName.Text != "")
                    {

                        var f = (from u in db.AccountMasters where u.AccGroup == txtAccGroupName.Text && u.Company_ID==logIn.company select u).ToList();
                        if (f.Count > 0)
                        {
                            MessageBox.Show("The Account Group is Already Used In Account Master, Cannot Be Deleted");
                            return;
                        }
                        var result = MessageBox.Show("Do You Want to Delete this Record ", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (result == DialogResult.Yes)
                        {
                            var d = db.AccountGroups.Where(w => w.GroupName == txtAccGroupName.Text && w.Company_ID == logIn.company).FirstOrDefault();
                            {
                                if (d != null)
                                {
                                    db.AccountGroups.DeleteOnSubmit(d);
                                    db.SubmitChanges();
                                    MessageBox.Show("Record Deleted Successfully");
                                    clear();

                                }
                            }
                        }
                        else
                        {
                            return;
                        }

                    }
                    else
                    {
                        MessageBox.Show("Please Select the Record");
                    }
                //}
                //else
                //{
                //    MessageBox.Show("You dont Have Privileges", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    clear();
                //}
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void txtAccGroupName_TextChanged(object sender, EventArgs e)
        {

        }

        public void fill_Tree2()
        {
           
            SqlCommand cmd = new SqlCommand("select distinct(GroupType) from accountgroups", con);
          
          
            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dap.Fill(dt);

       
            twGroups.Nodes.Clear();
            foreach (DataRow dr in dt.Rows)
            {
                TreeNode tnParent = new TreeNode();
                tnParent.Text = dr["GroupType"].ToString();
              
                tnParent.Expand();
              
                twGroups.Nodes.Add(tnParent);
                AddChildNodes(tnParent, tnParent.Text);
            }
            con.Close();
        }
        public void AddChildNodes(TreeNode tr1, string p)
        {
          
            SqlCommand cmd = new SqlCommand("SELECT GroupName From AccountGroups WHERE [GroupType]= '" + p + "'and subgroup='Primary'", con);

           
            SqlDataAdapter dap = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            dap.Fill(dt);

            foreach (DataRow dr in dt.Rows)
            {
                TreeNode child = new TreeNode();
                child.Text = dr["GroupName"].ToString().Trim();

                FillChild_Child(child, child.Text);
                AddChildNodes(child, child.Text);
                tr1.Nodes.Add(child);
            }
            con.Close();

        }
        public void FillChild_Child(TreeNode tr1, string p)

        {
         
                SqlCommand cmd = new SqlCommand("SELECT groupname FROM accountgroups WHERE subgroup= '"+p+"'", con);
             
                //con.Open();
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dap.Fill(dt);
                //parent.ChildNodes.Clear();
                foreach (DataRow dr in dt.Rows)
                {
                    TreeNode child = new TreeNode();
                    child.Text = dr["groupname"].ToString().Trim();
               
                 FillChild_Child(child, child.Text);
                tr1.Nodes.Add(child);
                }
                //con.Close();
            }

        private void AccountgroupMaster_Load(object sender, EventArgs e)
        {
            try
            {
                //fill_Tree2();
                //PictureBox2.Image = AppCode.GlobalAccess.comylogo;
                txtAccGroupName.Focus();

                //var ac = (from u in db.AccountGroups where u.Company == AppCode.GlobalAccess.companyName select u.GroupName).ToList();
                //cmbblgroup.DataSource = ac;
                //if (cmbblgroup.Items.Count > 0)
                //    cmbblgroup.SelectedIndex = -1;

                //getblgroup();

                //getplgroup();
                SubGroupLoad();
                PopulateTreeView(0, null);

                //loadGrid();
                //clear();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
           


        }

        private void getplgroup()
        {
            //var objpl = (from obj in db.AccountGroups
            //             where  obj.PLHead != "null"
            //             select obj.PLHead).Distinct().ToList();

            //cmbplgroup.DataSource = objpl;
            //if (cmbplgroup.Items.Count > 0)
            //    cmbplgroup.SelectedIndex = -1;
        }

        private void getblgroup()
        {
            //var objbl = (from obj in db.AccountGroups
            //             where  obj.BalSheetHead != "null"
            //             select obj.BalSheetHead).Distinct().ToList();

            //cmbblgroup.DataSource = objbl;
            //if (cmbblgroup.Items.Count > 0)
            //    cmbblgroup.SelectedIndex = -1;
        }

        private void TreeView1_DoubleClick(object sender, EventArgs e)
        {
            //txtSubHeading.Text = dt.Rows[0][3].ToString();
        }

        private void txtAccGroupName_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtAccGroupName.Text != "")
                {
                    if ((from u in db.AccountGroups where u.GroupName == txtAccGroupName.Text && u.Company_ID == logIn.company  select u).Count() > 0)
                    {
                        MessageBox.Show("Already  Group Name Exist,Please Try Another One");
                        txtAccGroupName.Text = "";
                        txtAccGroupName.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgAcGroup_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex >= 0)
                {
                    txtAccGroupName.Text = dgAcGroup.Rows[e.RowIndex].Cells["GroupName"].Value.ToString();
                    cmbGroupType.Text = dgAcGroup.Rows[e.RowIndex].Cells["GroupType"].Value.ToString();
                    cmbSubGroup.Text = dgAcGroup.Rows[e.RowIndex].Cells["SubGroup"].Value.ToString();

                    var bl = (from obj in db.AccountGroups
                              where  obj.GroupName == txtAccGroupName.Text select new
                              {
                                  obj.BalSheetHead,
                                  obj.PLHead,
                                  obj.BLGroup
                              }).ToList();
                            cmbblgroup.Text = bl[0].BalSheetHead;                                                          
                    cmbplgroup.Text = bl[0].PLHead;
                    if (bl[0].BLGroup != null)
                    {
                        checkBox1.Checked = bl[0].BLGroup.Value;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtAccGroupName_KeyPress(object sender, KeyPressEventArgs e)
        {

            //if (e.KeyChar != 32)
            //{
            //    e.Handled = char.IsPunctuation(e.KeyChar) || char.IsSeparator(e.KeyChar) || char.IsSymbol(e.KeyChar);

            //}
        }

        private void twGroups_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
               
                    txtAccGroupName.Text = twGroups.SelectedNode.Text;
                //cmbGroupType.Text = dgAcGroup.Rows[e.RowIndex].Cells["GroupType"].Value.ToString();
                //cmbSubGroup.Text = dgAcGroup.Rows[e.RowIndex].Cells["SubGroup"].Value.ToString();

                var bl = (from obj in db.AccountGroups
                          where obj.GroupName == txtAccGroupName.Text && obj.Company_ID == logIn.company
                              select new
                              {
                                  obj.ID,
                                  obj.GroupType,
                                  obj.SubGroup,
                                  obj.SubGroup_ID,
                                  obj.BalSheetHead,
                                  obj.PLHead,
                                  obj.BLGroup
                              }).ToList();
                    
                    cmbblgroup.Text = bl[0].BalSheetHead;
                    cmbplgroup.Text = bl[0].PLHead;
                    txtID.Text = bl[0].ID.ToString();
                    cmbSubGroup.SelectedValue = bl[0].SubGroup_ID;
                    cmbGroupType.Text = bl[0].GroupType;
                    if (bl[0].BLGroup != null)
                    {
                        checkBox1.Checked = bl[0].BLGroup.Value;
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
