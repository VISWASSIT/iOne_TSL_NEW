using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
//using Aga.Controls.Tree;
using Ione_DAL;

namespace ioneNet.FinanaceManagement
{
    public partial class ChartOfAccounts: Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        //string objConfig = ConfigurationManager.ConnectionStrings["LaksanaIndSysConnectionString"].ToString();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        System.Data.Common.DbTransaction transaction;
      
        string mode = "";
        private List<TreeNode> CurrentNodeMatches = new List<TreeNode>();
        private int LastNodeIndex = 0;
        private string LastSearchText;

        public ChartOfAccounts()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void brnAddNewGroup_Click(object sender, EventArgs e)
        {
            ioneNet.FinanaceManagement.AccountGroups obj = new ioneNet.FinanaceManagement.AccountGroups();
            if (obj.ShowDialog() == DialogResult.OK)
            {
                ACCGroupLoad();
            }
        }

        private void Clear()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                foreach (Control x in this.Controls)
                {
                    foreach (Control d in txtCustomerAccount.Controls)
                    {
                        if (d is TextBox)
                            (d as TextBox).Clear();
                        if (d is ComboBox)
                            (d as ComboBox).Text = "";
                        if (d is ComboBox)
                            (d as ComboBox).SelectedIndex = -1;
                        if (d is CheckBox)
                            (d as CheckBox).Checked = false;
                    }

                }              
                
                //ACCGroupLoad();               
                //cmbAccountGroup.Text = "";
                //cmbAccountGroup.SelectedIndex = -1;            
                //fill_Tree2();
                txtAccountName.Focus();
                mode = "";
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);

            }
        }


        // // department load method to bind all the department names from department table to the cost centre form
        //private void ACCGroupLoad()
        //{
        //    try
        //    {

        //        SqlCommand cmd = new SqlCommand("select  GroupName  from AccountGroups where Company='" + AppCode.GlobalAccess.companyName + "'", con);
        //        SqlDataAdapter da = new SqlDataAdapter(cmd);
        //        DataSet ds = new DataSet();
        //        da.Fill(ds, "p");
        //        DataRow drow = ds.Tables["p"].NewRow();
        //        drow["GroupName"] = "Reverse";
        //        ds.Tables["p"].Rows.InsertAt(drow, 0);
        //        cmbAccountGroup.DataSource = ds.Tables["p"];
        //        cmbAccountGroup.DisplayMember = "GroupName";
        //        cmbAccountGroup.SelectedIndex = 0;

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //    finally
        //    {

        //    }

        //}

        private void ACCGroupLoad()
        {
            try
            {
                var Buyerblind = (from m in db.AccountGroups where  m.Company_ID == logIn.company  
                                  select new { m.GroupName, m.ID }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    cmbAccountGroup.DataSource = Buyerblind;
                    cmbAccountGroup.DisplayMember = "GroupName";
                    cmbAccountGroup.ValueMember = "ID";
                }
                if (cmbAccountGroup.Items.Count > 0)
                {

                    cmbAccountGroup.SelectedIndex = -1;
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        

      

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {                
                if (txtAccountName.Text == "")
                {
                    MessageBox.Show("Please Enter Account Name", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtAccountName.Focus();
                    return;
                }
                if (cmbAccountGroup.Text == "")
                {
                    MessageBox.Show("Please Select Account Group Name", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbAccountGroup.Focus();
                    return;
                }
               
                if (CmbHead.Text == "")
                {
                    MessageBox.Show("Please Select Account Head / Groupe", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CmbHead.Focus();
                    return;
                }
                else
                {
                    if (CmbHead.Text != txtAccountName.Text)
                    {
                        var dialog =  MessageBox.Show("Account Head is Diffrent from account Name, Are You Sure To Proceed", "Info", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        if (dialog == DialogResult.No)
                        {
                            CmbHead.Focus();
                            return;
                        }
                    }
                }
                save();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
               
        public void save()
        {
           if (txtAccCode.Text!="")
            {
                db.Transaction = null;
                try
                {

                    //if (AppCode.GlobalAccess.Edit == "Yes")
                    //{
                    var am = db.AccountMasters.Where(w => w.id == Convert.ToInt32(txtAccCode.Text) && w.Company_ID == logIn.company).FirstOrDefault();
                    {


                        // var created = false;
                        if (null != db.Connection)
                        {
                            db.Connection.Close();
                        }
                        System.Data.Common.DbTransaction transaction;
                        db.Connection.Open();
                        transaction = db.Connection.BeginTransaction();
                        db.Transaction = transaction;                      
                        am.AccGroup = cmbAccountGroup.Text;
                        am.AccName = txtAccountName.Text;
                        am.AccGroup_ID = Convert.ToInt32(cmbAccountGroup.SelectedValue);                      
                        am.Group_Head_ID = Convert.ToInt32(CmbHead.SelectedValue);
                        am.AccHead = CmbHead.Text;
                        am.Status = 1;
                        am.Created_By =linkCreatedBy.Text;
                        am.Modified_BY = logIn.username + "-" + DateTime.Now;
                        am.Company_ID = logIn.company;                       
                        db.SubmitChanges();
                        db.Transaction = transaction;                        
                        transaction.Commit();
                        MessageBox.Show("Record Updated Successfully");
                        Clear();

                    }
                    //else
                    //{
                    //    MessageBox.Show("You dont Have Privileges", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    Clear();
                    //}
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
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
                        transaction = db.Connection.BeginTransaction();
                        db.Transaction = transaction;
                        AccountMaster am = new AccountMaster();
                        //am.AccCode = db.Sp_autoincrement_AcountMaster(AppCode.GlobalAccess.companyName).FirstOrDefault().AccCode; ;
                        am.AccGroup = cmbAccountGroup.Text;
                        am.AccName = txtAccountName.Text;
                        am.AccGroup_ID = Convert.ToInt32(cmbAccountGroup.SelectedValue);
                        am.Group_Head_ID = Convert.ToInt32(CmbHead.SelectedValue);
                        am.Created_By = logIn.username;
                        am.Modified_BY = logIn.username;
                        am.Company_ID = logIn.company;                       

                        if (!string.IsNullOrEmpty(CmbHead.Text))
                        {
                            am.AccHead = (CmbHead.Text == "") ? "" : (CmbHead.Text);
                        }
                        else
                        {
                            am.AccHead = (txtAccountName.Text == "") ? "" : (txtAccountName.Text);
                        }
                        am.Status = 1;
                        
                        am.Created_By = logIn.username + "-" + DateTime.Now;
                        am.Modified_BY = logIn.username + "-" + DateTime.Now;
                        db.AccountMasters.InsertOnSubmit(am);
                        db.SubmitChanges();
                        db.Transaction = transaction;                        
                        transaction.Commit();
                        MessageBox.Show("Record Saved Successfully,This Record code is '" + am.AccCode + "'");
                        Clear();
                    //}
                    //else
                    //{
                    //    MessageBox.Show("You dont Have Privileges", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    Clear();
                    //}
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
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

        private void Accounts_Load(object sender, EventArgs e)
        {
            try
            {
                //PictureBox2.Image = AppCode.GlobalAccess.comylogo;
                ACCGroupLoad();
                cmbAccountGroup.Text = "";
                cmbAccountGroup.SelectedIndex = -1;
                txtAccountName.Focus();
                PopulateTreeViewGroups(0,null);
                //FillTreeView();
                bindAct();
                cmbAccountGroup.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                //txtSearch.AutoCompleteMode = AutoCompleteMode.None;
                //PopulateTreeView();

                cmbAccountGroup.AutoCompleteSource = AutoCompleteSource.ListItems;
                
                

                //NodeTextBox ntb = new NodeTextBox();
                //ntb.DataPropertyName = "Text";
                //this.treeViewAdv1.NodeControls.Add(ntb);

                //treeViewAdv1.EndUpdate();


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void PopulateTreeView_SubGroups(int parentId, TreeNode parentNode)
        //private void PopulateTreeView()
        {
            try
            {
               //Cursor.Current = new Cursor("MyWait.cur");
                //twAccounts.BeginUpdate();
                TreeNode childNode;
                SqlCommand cmd = new SqlCommand("select * from [AccountGroups] WHERE [Company_ID] = @CompID and SubGroup_ID =@gid order by groupname", con);
                cmd.Parameters.AddWithValue("@CompID", logIn.company);
                cmd.Parameters.AddWithValue("@gid", parentId);
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dap.Fill(dt);
                //SqlDataAdapter dap = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //dap.Fill(dt);
                twAccounts.SelectedNode.Nodes.Clear();
                //foreach (DataRow dr in dt.Rows)
                foreach (DataRow dr in dt.Rows)
                {
                    TreeNode t = new TreeNode();
                    t.Text = dr["GroupName"].ToString();
                    t.Name = dr["id"].ToString();
                    t.Tag = "AccGroup";
                    if (parentNode == null)
                    {
                        twAccounts.Nodes.Add(t);
                        childNode = t;
                    }
                    else
                    {
                        //parentNode.Nodes.Add(t);
                        twAccounts.SelectedNode.Nodes.Add((TreeNode)t.Clone());
                        childNode = t;
                    }

                    //PopulateTreeView(Convert.ToInt32(dr["id"].ToString()), childNode);
                }
                //twAccounts.EndUpdate();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void PopulateTreeView_Accounts(int parentId, TreeNode parentNode)
        //private void PopulateTreeView()
        {
            try
            {
                //Cursor.Current = new Cursor("MyWait.cur");
                //twAccounts.BeginUpdate();
                TreeNode childNode;
                SqlCommand cmd = new SqlCommand("select * from [AccountsList] WHERE [Company_ID] = @CompID and subgroup_id =@gid order by groupname", con);
                cmd.Parameters.AddWithValue("@CompID", logIn.company);
                cmd.Parameters.AddWithValue("@gid", parentId);
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dap.Fill(dt);
                //SqlDataAdapter dap = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //dap.Fill(dt);
                twAccounts.SelectedNode.Nodes.Clear();
                //foreach (DataRow dr in dt.Rows)
                foreach (DataRow dr in dt.Rows)
                {
                    TreeNode t = new TreeNode();
                    t.Text = dr["groupname"].ToString();
                    t.Name = dr["id"].ToString();
                    t.Tag = "AccName";
                    if (parentNode == null)
                    {
                        twAccounts.Nodes.Add(t);
                        childNode = t;
                    }
                    else
                    {
                        //parentNode.Nodes.Add(t);
                        twAccounts.SelectedNode.Nodes.Add((TreeNode)t.Clone());
                        childNode = t;
                    }

                    //PopulateTreeView(Convert.ToInt32(dr["id"].ToString()), childNode);
                }
                //twAccounts.EndUpdate();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void PopulateTreeViewGroups(int parentId, TreeNode parentNode)
        {
            try
            {
                twAccounts.BeginUpdate();
                TreeNode childNode;
                SqlCommand cmd = new SqlCommand("select DISTINCT [GroupName],id from [AccountGroups] WHERE [Company_ID] = @CompID and SubGroup_ID = @subid", con);
                cmd.Parameters.AddWithValue("@CompID", logIn.company);
                cmd.Parameters.AddWithValue("@subid", parentId);

                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dap.Fill(dt);
                //twAccounts.Nodes.Clear();
                //foreach (DataRow dr in dt.Rows)
                foreach (DataRow dr in dt.Rows)
                {
                    TreeNode t = new TreeNode();
                    t.Text = dr["GroupName"].ToString();
                    t.Name = dr["ID"].ToString();
                    t.Tag = "AccType"; //dt.Rows.IndexOf(dr);
                    if (parentNode == null)
                    {
                        twAccounts.Nodes.Add(t);
                        childNode = t;
                    }
                    else
                    {
                        //parentNode.Nodes.Add(t);
                        twAccounts.SelectedNode.Nodes.Add((TreeNode)t.Clone());
                        childNode = t;
                    }

                    //PopulateTreeView(Convert.ToInt32(dr["id"].ToString()), childNode);
                }
                twAccounts.EndUpdate();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        //private void PopulateTreeViewGroups(int parentId, TreeNode parentNode)
        //{
        //    try
        //    {
        //        twAccounts.BeginUpdate();
        //        TreeNode childNode;
        //        SqlCommand cmd = new SqlCommand("select DISTINCT [GroupType],SubGroup_ID,GroupName,id from [AccountGroups] WHERE [Company_ID] = @CompID", con);
        //        cmd.Parameters.AddWithValue("@CompID", logIn.company);

        //        SqlDataAdapter dap = new SqlDataAdapter(cmd);
        //        DataTable dt = new DataTable();
        //        dap.Fill(dt);
        //        //twAccounts.Nodes.Clear();
        //        //foreach (DataRow dr in dt.Rows)
        //        foreach (DataRow dr in dt.Select("[SubGroup_ID]=" + parentId))
        //        {
        //            TreeNode t = new TreeNode();
        //            t.Text = dr["GroupName"].ToString();
        //            t.Name = dr["id"].ToString();
        //            t.Tag = dt.Rows.IndexOf(dr);
        //            if (parentNode == null)
        //            {
        //                twAccounts.Nodes.Add(t);
        //                childNode = t;
        //            }
        //            else
        //            {
        //                parentNode.Nodes.Add(t);
        //                childNode = t;
        //            }

        //            PopulateTreeViewGroups(Convert.ToInt32(dr["id"].ToString()), childNode);
        //        }
        //        twAccounts.EndUpdate();
        //    }
        //    catch (Exception ex)
        //    {

        //        MessageBox.Show(ex.Message);
        //    }
        //}
        private void twAccounts_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            //TreeNode childNode;
            //TreeNode t = new TreeNode();
            //t.Text = e.Node.Text; // twAccounts.SelectedNode.Text;
            //childNode = t;
            //TreeNode parentNode = t;
            //PopulateTreeView(Convert.ToInt32(e.Node.Name), childNode);
            //SqlCommand cmd = new SqlCommand("select * from [AccountsList] WHERE [Company_ID] = @CompID and SubGroup_ID =@gid", con);
            //cmd.Parameters.AddWithValue("@CompID", logIn.company);
            //cmd.Parameters.AddWithValue("@gid", Convert.ToInt32(e.Node.Name));
            //SqlDataAdapter dap = new SqlDataAdapter(cmd);
            //DataTable dt = new DataTable();
            //dap.Fill(dt);
            //foreach (DataRow dr in dt.Rows)
            //{
            //    //if(dr["group_yes"] =="No")
            //    //{
            //        t.Text = dr["GroupName"].ToString();
            //        t.Name = dr["id"].ToString();
            //        t.Tag = dt.Rows.IndexOf(dr);
            //        parentNode.Nodes.Add(t);
            //        childNode = t;
            //    //}
            //    //TreeNode t = new TreeNode();
              
                
            //    //if (parentNode == null)
            //    //{
            //    //    twAccounts.Nodes.Add(t);
            //    //    childNode = t;
            //    //}
            //    //else
            //    {
                    
            //    }
            //    //if (dr["Group_Yes"] != "No")
                //{
                //
                //}
            //}
            ////var d = (from s in db.accou where s.id == Convert.ToInt32(twAccounts.SelectedNode.Name) select s).SingleOrDefault();
            //PopulateTreeView(Convert.ToInt32(twAccounts.SelectedNode.Name), childNode);
        }
        public void bindAct()
        {
            try
            {


                var Buyerblind = (from m in db.AccountMasters where  m.Company_ID == logIn.company 
                                  select new { m.id, m.AccName }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    CmbHead.DataSource = Buyerblind;
                    CmbHead.DisplayMember = "AccName";
                    CmbHead.ValueMember = "id";
                }
                if (CmbHead.Items.Count > 0)
                {

                    CmbHead.SelectedIndex = -1;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ColorNodes(TreeNode t)
        {
            foreach (TreeNode tn in t.Nodes)
            {
                tn.ForeColor = Color.Blue;
                ColorNodes(tn);
            }
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
                MessageBox.Show(ex.Message);
            }
        }

        private void txtAccountName_Leave(object sender, EventArgs e)
        {
            try
            {
                if (mode != "Search")
                {
                    if (txtAccountName.Text != "")
                    {
                        if ((from u in db.AccountMasters where u.AccName == txtAccountName.Text && u.Company_ID == logIn.company select u).Count() > 0)
                        {
                            MessageBox.Show("Already Account Name Exist,Please Try Another One");
                            //txtAccountName.Text = "";
                            txtAccountName.Focus();
                            mode = "";
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void txtAccountName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 32)
            {
                e.Handled = char.IsPunctuation(e.KeyChar) || char.IsSeparator(e.KeyChar) || char.IsSymbol(e.KeyChar);

            }
        }

        
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtAccCode.Text != "")
                {

                    DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        int ProdID;
                        ProdID = Convert.ToInt32(txtAccCode.Text);
                        SqlCommand cmd1 = new SqlCommand("delete  from [AccountMaster] where id =@ProdID", con);
                        cmd1.Parameters.AddWithValue("@ProdID", ProdID);

                        if (con.State != ConnectionState.Open)
                            con.Open();
                        //con.Open();
                        cmd1.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Account Deleted Successfully");
                    }

                }
                else
                {
                    MessageBox.Show("Please Select Atleast One Account Name to Delete");
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

       

        public void fill_Tree2()
        {
            try
            {
                //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LaksanaIndSysConnectionString"].ToString());
                //con.Open();
                ////      SqlCommand cmd = new SqlCommand("select distinct(GroupType) from AccountGroups where subgroup = 'Primary'  and Company='" + AppCode.GlobalAccess.companyName + "'", con);
                //SqlCommand cmd = new SqlCommand("select distinct(GroupType) from AccountGroups where subgroup = 'Primary'", con);
                ////SqlCommand cmd = new SqlCommand("sp_getTreeGroups", con);
                ////cmd.CommandType = CommandType.StoredProcedure;
                ////cmd.Parameters.AddWithValue("@company",AppCode.GlobalAccess.companyName);
                //SqlDataAdapter dap = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //dap.Fill(dt);
                //twAccounts.Nodes.Clear();
                //foreach (DataRow dr in dt.Rows)
                //{
                //    TreeNode tnParent = new TreeNode();
                //    tnParent.Text = dr["GroupType"].ToString();
                //    tnParent.Expand();
                //    twAccounts.Nodes.Add(tnParent);
                //    AddChildNodes(tnParent, tnParent.Text);
                //}
                ////con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void AddChildNodes(TreeNode tr1, string p)
        {
            try
            {
               // string Sub_Group_Of = p;
               // SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LaksanaIndSysConnectionString"].ToString());
                
               // con.Open();
               //// SqlCommand cmd = new SqlCommand("SELECT GroupName,GroupType FROM AccountGroups WHERE GroupType= '" + p + "'  and Company='" + AppCode.GlobalAccess.companyName + "'  and  GroupName NOT IN(SELECT GroupName FROM AccountGroups WHERE SubGroup != 'Primary' AND  Company ='" + AppCode.GlobalAccess.companyName + "')", con);
               // SqlCommand cmd = new SqlCommand("SELECT GroupName,GroupType FROM AccountGroups WHERE GroupType= '" + p + "'  and  GroupName NOT IN(SELECT GroupName FROM AccountGroups WHERE SubGroup != 'Primary')", con);
               // //SqlCommand cmd = new SqlCommand("sp_getTreeSubGroups", con);
               // //cmd.CommandType = CommandType.StoredProcedure;
               // //cmd.Parameters.AddWithValue("@Grouptype", p);
               // //cmd.Parameters.AddWithValue("@company", AppCode.GlobalAccess.companyName);

               // SqlDataAdapter dap = new SqlDataAdapter(cmd);
               // DataTable dt = new DataTable();
               // dap.Fill(dt);

               // foreach (DataRow dr in dt.Rows)
               // {
               //     TreeNode child = new TreeNode();
               //     child.Text = dr["GroupName"].ToString().Trim();

               //     AddChild_ChildNodes(child, child.Text);

               //     tr1.Nodes.Add(child);
               // }
               //// con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public void AddChild_ChildNodes(TreeNode tr1, string p)
        {
            try
            {
            //    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["LaksanaIndSysConnectionString"].ToString());
                
            //    con.Open();
            ////    SqlCommand cmd = new SqlCommand("SELECT GroupName,SubGroup  FROM AccountGroups WHERE  Company='" + AppCode.GlobalAccess.companyName + "' and SubGroup!='Primary' and SubGroup='" + p + "'  UNION SELECT  AccName AS 'GroupName',AccGroup AS 'SubGroup'  FROM  AccountMaster WHERE AccGroup='" + p + "' AND CompName='" + AppCode.GlobalAccess.companyName + "'", con);
            //    SqlCommand cmd = new SqlCommand("SELECT GroupName,SubGroup  FROM AccountGroups WHERE  SubGroup!='Primary' and SubGroup='" + p + "'  UNION SELECT  AccName AS 'GroupName',AccGroup AS 'SubGroup'  FROM  AccountMaster WHERE AccGroup='" + p + "'", con);
            //    //SqlCommand cmd = new SqlCommand("sp_getTreeSubChildGroups", con);
            //    //cmd.CommandType = CommandType.StoredProcedure;
            //    //cmd.Parameters.AddWithValue("@Subgroup", p);
            //    //cmd.Parameters.AddWithValue("@company", AppCode.GlobalAccess.companyName);


            //    SqlDataAdapter dap = new SqlDataAdapter(cmd);
            //    DataTable dt = new DataTable();
            //    dap.Fill(dt);

            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        TreeNode child = new TreeNode();
            //        child.Text = dr["GroupName"].ToString().Trim();

            //        AddChild_ChildNodes(child, child.Text);

            //        tr1.Nodes.Add(child);
            //    }
            //    con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void twAccounts_DoubleClick(object sender, EventArgs e)
        {
            try
            {

                mode = "Search";
                string id = twAccounts.SelectedNode.Name;
                var d = (from s in db.AccountMasters where s.id == Convert.ToInt32(id) && s.Company_ID == logIn.company  select s).SingleOrDefault();
                if (d != null)
                {
                    txtAccCode.Text = d.id.ToString();
                    txtAccountName.Text = d.AccName;
                    cmbAccountGroup.SelectedValue = d.AccGroup_ID;
                    CmbHead.SelectedValue = d.Group_Head_ID;                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       

        private void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {

                string searchText = this.txtSearchtreeView.Text;
                if (String.IsNullOrEmpty(searchText))
                {
                    return;
                };


                if (LastSearchText != searchText)
                {
                    //It's a new Search
                    CurrentNodeMatches.Clear();
                    LastSearchText = searchText;
                    LastNodeIndex = 0;
                    SearchNodes(searchText, twAccounts.Nodes[0]);
                }

                if (LastNodeIndex >= 0 && CurrentNodeMatches.Count > 0 && LastNodeIndex < CurrentNodeMatches.Count)
                {
                    TreeNode selectedNode = CurrentNodeMatches[LastNodeIndex];
                    // LastNodeIndex++;
                    this.twAccounts.SelectedNode = selectedNode;
                    this.twAccounts.SelectedNode.Expand();
                    this.twAccounts.Select();

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void SearchNodes(string SearchText, TreeNode StartNode)
        {
            try
            {
                //TreeNode node = null;
                while (StartNode != null)
                {
                    if (StartNode.Text.ToLower().Contains(SearchText.ToLower()))
                    {
                        CurrentNodeMatches.Add(StartNode);
                    };
                    if (StartNode.Nodes.Count != 0)
                    {
                        SearchNodes(SearchText, StartNode.Nodes[0]);//Recursive Search 
                    };
                    StartNode = StartNode.NextNode;
                };
            }
            catch (Exception)
            {
            }
        }

        private void FindByText()
        {
            try
            {
                TreeNodeCollection nodes = twAccounts.Nodes;
                foreach (TreeNode n in nodes)
                {
                    FindRecursive(n);
                }

            }
            catch (Exception)
            {
            }

        }

        private void FindRecursive(TreeNode treeNode)
        {
            try
            {
                foreach (TreeNode tn in treeNode.Nodes)
                {
                    // if the text properties match, color the item
                    if (tn.Text == this.txtSearchtreeView.Text)
                        tn.BackColor = Color.LightBlue;

                    FindRecursive(tn);
                }
            }
            catch (Exception)
            {
            }
        }

        private void ClearBackColor()
        {
            try
            {
                TreeNodeCollection nodes = twAccounts.Nodes;
                foreach (TreeNode n in nodes)
                {
                    ClearRecursive(n);
                }
            }
            catch (Exception)
            {
            }
        }

        // called by ClearBackColor function
        private void ClearRecursive(TreeNode treeNode)
        {
            try
            {
                foreach (TreeNode tn in treeNode.Nodes)
                {
                    tn.BackColor = Color.White;
                    ClearRecursive(tn);
                }
            }
            catch (Exception)
            {
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                //OpenFileDialog fdlg = new OpenFileDialog();
                //string ChooseFilepath = "";
                //fdlg.Title = "Select file";
                //fdlg.InitialDirectory = @"c:\";
                //fdlg.FileName = ChooseFilepath;
                //fdlg.Filter = "Excel Sheet(*.xls)|*.xls|All Files(*.*)|*.*";
                //fdlg.FilterIndex = 1;
                //fdlg.RestoreDirectory = true;
                //if (fdlg.ShowDialog() == DialogResult.OK)
                //{
                //    ChooseFilepath = fdlg.FileName;
                //    System.Data.OleDb.OleDbConnection MyConnection;
                //    System.Data.DataTable DtSet;
                //    System.Data.OleDb.OleDbDataAdapter MyCommand;
                //    string str = "Provider = Microsoft.jet.OLEDB.4.0; Data source=" + ChooseFilepath + ";Extended Properties=\"Excel 8.0;HDR= yes;\";";
                //    MyConnection = new System.Data.OleDb.OleDbConnection("Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + ChooseFilepath + ";Extended Properties='Excel 8.0;HDR=Yes'");
                //    MyCommand = new System.Data.OleDb.OleDbDataAdapter("Select * From [ProductMaster$]", MyConnection);                    //Product Name Name
                //    MyCommand.TableMappings.Add("Table", ChooseFilepath);
                //    DtSet = new System.Data.DataTable();
                //    MyCommand.Fill(DtSet);
                //    int count = DtSet.Rows.Count;
                //    for (int i = 0; i < count; i++)
                //    {
                //        if ((from u in db.ProdMasters where u.Product_Name == DtSet.Rows[i]["Product Name"].ToString() && u.Comp_Name == AppCode.GlobalAccess.companyName select u).Count() == 0)
                //        {
                //            ProdMaster p = new ProdMaster();
                //            p.Product_type = DtSet.Rows[i]["Product Type"].ToString();

                //            var d1 = (from s in db.ProductGroups where s.Comp_Name == AppCode.GlobalAccess.companyName && s.Product_Category == DtSet.Rows[i]["Product Group"].ToString().Trim() select s).ToList();
                //            if (d1.Count > 0)
                //            {
                //                p.Product_Category = DtSet.Rows[i]["Product Group"].ToString();
                //            }
                //            else
                //            {
                //                MessageBox.Show(" this '" + DtSet.Rows[i]["Product Group"].ToString() + "' product group  Not Exist Please Try another One");
                //                return;
                //            }

                //            p.unit_purchase = DtSet.Rows[i]["Purchase UOM"].ToString();
                //            p.Unit_Sale = DtSet.Rows[i]["Sale UOM"].ToString();
                //            p.ScrapItems = DtSet.Rows[i]["Scrap Items"].ToString();
                //            p.Salable_Item = DtSet.Rows[i]["Salable"].ToString();
                //            var d2 = (from s in db.ColourCode_Masters where s.Company == AppCode.GlobalAccess.companyName && s.ColourName == DtSet.Rows[i]["Color_Name"].ToString().Trim() select s).ToList();
                //            if (d2.Count > 0)
                //            {
                //                p.Color_Name = DtSet.Rows[i]["Color_Name"].ToString();
                //            }
                //            else
                //            {
                //                MessageBox.Show(" this '" + DtSet.Rows[i]["Color_Name"].ToString() + "' colour Name  Not Exist Please Try another One");
                //                return;
                //            }
                //            p.Critical_Item = DtSet.Rows[i]["Critical Item"].ToString();
                //            p.Mfg_Item = DtSet.Rows[i]["Manufacturing Item"].ToString();
                //            p.Cenvat_Item = DtSet.Rows[i]["CENVAT Item"].ToString();
                //            p.VAT_Item = DtSet.Rows[i]["VAT Item"].ToString();
                //            p.QAP_Item = DtSet.Rows[i]["QAP Item"].ToString();
                //            p.Min_Stock = Convert.ToDecimal(DtSet.Rows[i]["Minimum Stock"].ToString());
                //            p.Max_Stock = Convert.ToDecimal(DtSet.Rows[i]["Maximum Stock"].ToString());
                //            p.ReOrd_Level = Convert.ToDecimal(DtSet.Rows[i]["Re-Order Level"].ToString());
                //            p.ReOrd_Qty = Convert.ToDecimal(DtSet.Rows[i]["Re-Order Qty"].ToString());
                //            p.Product_Version = DtSet.Rows[i]["Version"].ToString();
                //            p.Sub_Heading = DtSet.Rows[i]["Sub Heading No"].ToString();
                //            p.MoldAmortisationCost = Convert.ToDecimal(DtSet.Rows[i]["MAC"].ToString());
                //            p.Comp_Name = AppCode.GlobalAccess.companyName;
                //            p.createdby = AppCode.GlobalAccess.UserName;
                //            p.createdon = DateTime.Now;
                //            db.ProdMasters.InsertOnSubmit(p);
                //        }
                //    }
                //    db.SubmitChanges();
                //    MessageBox.Show("Import Data Successfully");
                //    MyConnection.Close();

                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void cmbAccountGroup_Leave(object sender, EventArgs e)

        {

        }

        private void btnshowdata_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                foreach (TreeNode tn in twAccounts.Nodes)
                {
                    tn.ForeColor = Color.Blue;
                    ColorNodes(tn);
                }
                fill_Tree2();
                Cursor.Current = Cursors.Default;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void cmbCreditTerms_Leave(object sender, EventArgs e)
        {
            try
            {
                //if(cmbCreditTerms.Text== "Credit")
                //{
                //    label45.Visible = true;
                //    txtcreditdays.Visible = true;
                //}
                //else
                //{
                //    label45.Visible = false;
                //    txtcreditdays.Visible = false;
                //}
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void cmbSupplierCreditTerms_Leave(object sender, EventArgs e)
        {
            //if(cmbSupplierCreditTerms.Text== "Credit")
            //{
            //    label46.Visible = true;
            //    txtsupdays.Visible = true;
            //}
            //else
            //{
            //    label46.Visible = false;
            //    txtsupdays.Visible = false;
            //}
        }

        private void txtsupdays_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtcreditdays_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void txtAccGroup_Enter(object sender, EventArgs e)
        {
            //AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
            //addItems(DataColl);
            //txtAccGroup.AutoCompleteCustomSource = DataColl;
        }
        public void addItems(AutoCompleteStringCollection coll)
        {
            try
            {


                //string columnName = cmbSearch.Text;
                //string Cust = txtAccGroup.Text;

                ////if (columnName == "Customer Name")
                ////{
                //    var Pname = (from d in db.AccountGroups select new { d.GroupName }).ToList();
                //    DataTable dt = new DataTable();
                //    dt.Columns.Add("GroupName");
                //    foreach (var item in Pname)
                //    {
                //        dt.Rows.Add(item.GroupName);
                //    }
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        coll.Add(dt.Rows[i][0].ToString());
                //    }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void twAccounts_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            { 
                TreeNode childNode;
                TreeNode t = new TreeNode();
                t.Text = e.Node.Text; // twAccounts.SelectedNode.Text;
                childNode = t;
                TreeNode parentNode = t;
                if (e.Node.Tag == "AccType")
                {
                    PopulateTreeView_SubGroups(Convert.ToInt32(e.Node.Name), childNode);
                }
                else
                {
                    PopulateTreeView_Accounts(Convert.ToInt32(e.Node.Name), childNode);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
}

      
      
    }
}
