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
namespace ioneNet.Masters
{
    public partial class frmCreateUserRoles : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();
        public frmCreateUserRoles()
        {
            InitializeComponent();
        }

        private void frmCreateUserRoles_Load(object sender, EventArgs e)
        {
            try
            {
                lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

                BindMasters();
                autogen();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void BindMasters()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT distinct(Module_name) FROM [Modules_List]", con))
                {
                    cmd.CommandType = CommandType.Text;
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                lstModules.Items.Add(dt.Rows[i]["Module_name"].ToString());
                            }

                        }
                    }
                }



                ////Bind product Types
                //var bindTypes = (from m in db.Modules_Lists
                //                     //where m.Company_ID == logIn.company
                //                 select new
                //                 {

                //                     m.Module_name
                //                 }).ToList();

                //if (bindTypes.Count > 0)
                //{
                //    lstModules.DataSource = bindTypes;
                //    //lstModules.DisplayMember = "Module_name";

                //    //lstModules.SelectedIndex = -1;

                //}



                //Status
                var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Master" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbStatus.DataSource = pStatus;
                    cmbStatus.ValueMember = "ID";
                    cmbStatus.DisplayMember = "Descr";
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        System.Data.DataRow drgetproducts;
        DataTable dtexisting = new DataTable();
        private void btnFormLevelPermissions_Click(object sender, EventArgs e)
        {
            try
            {
                string modules = "";
                //if (dgMenuItems.Rows.Count > 1)
                //{
                    dtexisting.Rows.Clear();
                    dtexisting.Columns.Clear();
                    dtexisting.Columns.Add("Form_ID", typeof(string));
                    dtexisting.Columns.Add("Form_Name", typeof(string));
                    dtexisting.Columns.Add("Add", typeof(bool));
                    dtexisting.Columns.Add("Modify", typeof(bool));
                    dtexisting.Columns.Add("View", typeof(bool));
                    dtexisting.Columns.Add("Delete", typeof(bool));
                    dtexisting.Columns.Add("Review", typeof(bool));
                    dtexisting.Columns.Add("Approve", typeof(bool));                   

                    //for (int i = 0; i < dgMenuItems.Rows.Count - 1; i++)
                    //{
                    //    DataRow dr;
                    //    dr = dtexisting.NewRow();
                    //    dr["Form_ID"] = dgMenuItems.Rows[i].Cells["Form_ID"].Value.ToString();
                    //    dr["Form_Name"] = dgMenuItems.Rows[i].Cells["Form_Name"].Value.ToString();
                    //    dr["Add"] = dgMenuItems.Rows[i].Cells["Add"].Value;
                    //    dr["Modify"] = dgMenuItems.Rows[i].Cells["Modify"].Value;
                    //    dr["View"] = dgMenuItems.Rows[i].Cells["View"].Value;
                    //    dr["Delete"] = dgMenuItems.Rows[i].Cells["Delete"].Value;
                    //    dr["Review"] = dgMenuItems.Rows[i].Cells["Review"].Value;
                    //    dr["Approve"] = dgMenuItems.Rows[i].Cells["Approve"].Value;
                    //       dtexisting.Rows.Add(dr);

                    //}
                    //dtexisting.AcceptChanges();
                //}




            dtgetproducts.Columns.Clear();
            dtgetproducts.Rows.Clear();
            dtgetproducts.Columns.Add("Form_ID", typeof(string));
            dtgetproducts.Columns.Add("Form_Name", typeof(string));
            dtgetproducts.Columns.Add("Add", typeof(bool));
            dtgetproducts.Columns.Add("Modify", typeof(bool));
            dtgetproducts.Columns.Add("View", typeof(bool));
            dtgetproducts.Columns.Add("Delete", typeof(bool));
            dtgetproducts.Columns.Add("Review", typeof(bool));
            dtgetproducts.Columns.Add("Approve", typeof(bool));
            dtgetfinalprducts.Rows.Clear();
            for (int i = 0; i < lstModules.Items.Count; i++)
            {
                if (lstModules.GetItemCheckState(i) == CheckState.Checked)
                {
                    cmbModule.Items.Add(lstModules.Items[i].ToString());
                    if (modules != "")
                    {

                        modules = modules + "," + lstModules.Items[i].ToString();

                    }
                    else
                    {
                        modules = lstModules.Items[i].ToString();
                    }

                }
            }
            txtModules.Text = modules;

            ////Fomrs / Menu Items
            string s = txtModules.Text;
            string[] values = s.Split(',');
            for (int j = 0; j < values.Length; j++)
            {
                values[j] = values[j].Trim();
                string m = values[j].ToString();
                drgetproducts = dtgetproducts.NewRow();

                    SqlCommand cmd2 = new SqlCommand("GetUserRoleForms ", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@compname", logIn.company);
                    cmd2.Parameters.AddWithValue("@rollID", Convert.ToInt32(txtID.Text));                  
                    cmd2.Parameters.AddWithValue("@menumodule", m);
                    //SqlDataAdapter dachildmnu = new SqlDataAdapter(cmd2);

                    //DataTable dtchild = new DataTable();

                    //dachildmnu.Fill(dtchild);
                    //var p = (from S in db.Menu_Items
                    //         orderby S.Menu_DispOrder descending
                    //         where S.Menu_Module == m
                    //         select new
                    //         {
                    //         Form_ID = S.Menu_ID,
                    //         Form_Name = S.Menu_Item1


                    //     }
                    //);
                    //SqlCommand cmd1 = (SqlCommand)db.GetCommand(p);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd2);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);
                foreach (DataRow dr in dt1.Rows)
                {
                    drgetproducts["Form_ID"] = dr["Menu_ID"].ToString();
                    drgetproducts["Form_Name"] = dr["Menu_Item"].ToString();
                    drgetproducts["Add"] = dr["Create_Role"];
                    drgetproducts["Modify"] = dr["Modify_Role"];
                    drgetproducts["View"] = dr["View_Role"];
                    drgetproducts["Delete"] = dr["Delete_Role"];
                    drgetproducts["Review"] = dr["Review_Role"];
                    drgetproducts["Approve"] = dr["Approve_Role"];
                    dtgetproducts.Rows.Add(drgetproducts);
                    dtgetfinalprducts = dtgetfinalprducts.AsEnumerable().Union(dtgetproducts.AsEnumerable()).CopyToDataTable();
                    dtgetproducts.Rows.Clear();
                }       
                }
                dtexisting = dtexisting.AsEnumerable().Union(dtgetfinalprducts.AsEnumerable()).CopyToDataTable();
                //dgProducts.DataSource = dtexisting;
                dgMenuItems.DataSource = dtexisting;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        
        private void lstModules_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            
        }

        private void checkBox1_CheckStateChanged(object sender, EventArgs e)
        {
            try
            {
                if (checkBox1.Checked)
            {
                for (int i = 0; i < lstModules.Items.Count; i++)
                {
                    lstModules.SetItemChecked(i,true);
                    
                }
            }
            else
            {
                for (int i = 0; i < lstModules.Items.Count; i++)
                {
                    lstModules.SetItemChecked(i, false);

                }
            }
        }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
}

        private void cmbModule_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var p = (from S in db.Menu_Items
                         orderby S.Menu_DispOrder descending
                         where S.Menu_Module == cmbModule.Text
                         select new
                         {
                             Form_ID = S.Menu_ID,
                             Form_Name = S.Menu_Item1


                         });
                SqlCommand cmd1 = (SqlCommand)db.GetCommand(p);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);

                if (dt1.Rows.Count >= 0)
                {
                    dgMenuItems.DataSource = dt1;
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

        private void checkBox2_CheckStateChanged(object sender, EventArgs e)
        {
            //Add Permission
            foreach (DataGridViewRow row in dgMenuItems.Rows)
            {
                if (checkBox2.Checked)
                {
                    row.Cells["Add"].Value = 1;
                }
                else
                {
                    row.Cells["Add"].Value = 0;
                }
                //DataGridViewCheckBoxCell chk =  dgMenuItems.CurrentCell as DataGridViewCheckBoxCell;
                //chk.Value = !(chk.Value == null ? false : (bool)chk.Value); //because chk.Value is initialy null
            }
        }

        private void checkBox3_CheckStateChanged(object sender, EventArgs e)
        {
            //Modify Permission
            foreach (DataGridViewRow row in dgMenuItems.Rows)
            {
                if (checkBox3.Checked)
                {
                    row.Cells["Modify"].Value = 1;
                }
                else
                {
                    row.Cells["Modify"].Value = 0;
                }
            }
        }

        private void checkBox4_CheckStateChanged(object sender, EventArgs e)
        {
            //View Permission
            foreach (DataGridViewRow row in dgMenuItems.Rows)
            {

                if (checkBox4.Checked)
                {
                    row.Cells["View"].Value = 1;
                }
                else
                {
                    row.Cells["View"].Value = 0;
                }
            }
        }

        private void checkBox5_CheckStateChanged(object sender, EventArgs e)
        {
            //Delete Permission
            foreach (DataGridViewRow row in dgMenuItems.Rows)
            {
                if (checkBox5.Checked)
                {
                    row.Cells["Delete"].Value = 1;
                }
                else
                {
                    row.Cells["Delete"].Value = 0;
                }
            }
        }

        private void checkBox6_CheckStateChanged(object sender, EventArgs e)
        {
            //Review Permission
            foreach (DataGridViewRow row in dgMenuItems.Rows)
            {
                if (checkBox6.Checked)
                {
                    row.Cells["Review"].Value = 1;
                }
                else
                {
                    row.Cells["Review"].Value = 0;
                }
            }
        }

        private void checkBox7_CheckStateChanged(object sender, EventArgs e)
        {
            //Approval Permission
            foreach (DataGridViewRow row in dgMenuItems.Rows)
            {
                if (checkBox7.Checked)
                {
                    row.Cells["Approve"].Value = 1;
                }
                else
                {
                    row.Cells["Approve"].Value = 0;
                }
            }
        }
        public void autogen()
        {
            var result = db.Sp_autoincrement_UserRoleID(logIn.company);
            txtID.Text = result.FirstOrDefault().Role_ID;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtUserName.Text == "")
                {
                    MessageBox.Show("Role Name Cannot Be Blank");
                    return;
                }
                else if (cmbUserRole.Text == "")
                {
                    MessageBox.Show("Select User Role");
                    return;
                }
                else
                {
                    int inserted = 0;
                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.CommandText = "delete from User_roles where Company_ID = @Company_ID and Role_ID = @Role_Name";

                    cmd1.Parameters.AddWithValue("@Company_ID", logIn.company);
                    cmd1.Parameters.AddWithValue("@Role_Name", Convert.ToInt32(txtID.Text));
                    cmd1.Connection = con;
                    con.Open();
                    cmd1.ExecuteNonQuery();
                    con.Close();

                    foreach (DataGridViewRow row in dgMenuItems.Rows)
                    {
                        //bool isSelected = Convert.ToBoolean(row.Cells["checkBoxColumn"].Value);
                        if (row.Cells["Form_Name"].Value != null)
                        {

                            using (SqlCommand cmd = new SqlCommand("INSERT INTO User_roles VALUES(@Role_ID,@Role_Name, @Roll_Type, @Modules_Allowed,@Form_ID, @Form_Name,@Create_Role,@Modify_Role,@View_Role,@Delete_Role,@Review_Role,@Approve_Role,@Role_Status_ID,@Company_ID,@Created_By,@Modified_BY)", con))
                            {
                                cmd.Parameters.AddWithValue("@Role_ID", Convert.ToInt32(txtID.Text));
                                cmd.Parameters.AddWithValue("@Role_Name", txtUserName.Text);
                                cmd.Parameters.AddWithValue("@Roll_Type", cmbUserRole.Text);
                                cmd.Parameters.AddWithValue("@Modules_Allowed", txtModules.Text);
                                cmd.Parameters.AddWithValue("@Form_ID", Convert.ToInt32(row.Cells["Form_ID"].Value));
                                cmd.Parameters.AddWithValue("@Form_Name", row.Cells["Form_Name"].Value);
                                cmd.Parameters.AddWithValue("@Create_Role", row.Cells["Add"].Value);
                                cmd.Parameters.AddWithValue("@Modify_Role", row.Cells["Modify"].Value);
                                cmd.Parameters.AddWithValue("@View_Role", row.Cells["View"].Value);
                                cmd.Parameters.AddWithValue("@Delete_Role", row.Cells["Delete"].Value);
                                cmd.Parameters.AddWithValue("@Review_Role", row.Cells["Review"].Value);
                                cmd.Parameters.AddWithValue("@Approve_Role", row.Cells["Approve"].Value);
                                cmd.Parameters.AddWithValue("@Role_Status_ID", Convert.ToInt32(cmbStatus.SelectedValue.ToString()));
                                cmd.Parameters.AddWithValue("@Company_ID", logIn.company);
                                cmd.Parameters.AddWithValue("@Created_By", lblCreatedBy.Text);
                                cmd.Parameters.AddWithValue("@Modified_BY", lblModified.Text);

                                con.Open();
                                cmd.ExecuteNonQuery();
                                con.Close();
                            }

                            inserted++;
                        }
                    }


                    if (inserted > 0)
                    {
                        MessageBox.Show(string.Format("{0} records inserted.", inserted), "Message");
                    }
                }
                clear();
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                var ca = (from sq in db.User_Roles                          
                          where sq.Company_ID == logIn.company
                          select new
                          {
                              sq.Role_ID,
                              sq.Role_Name,
                              sq.Roll_Type                              
                          }).Distinct();
                SqlCommand cmd3 = (SqlCommand)db.GetCommand(ca);
                SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                DataTable dt1 = new DataTable();
                da3.Fill(dt1);
                if (dt1.Rows.Count > 0)
                    dgUserRoles.DataSource = dt1;
                groupBox4.Visible = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void dgUserRoles_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int i = dgUserRoles.CurrentRow.Index;
                if (dgUserRoles.Rows[i].Cells["Role_ID"].Value.ToString() != "")
                {
                    txtID.Text = dgUserRoles.Rows[i].Cells["Role_ID"].Value.ToString();
                    //var = "0";
                    //txtRefNo.Text = CuttingPlansList.SO_No;
                    var sa = (from a in db.User_Roles
                              where a.Company_ID == logIn.company && a.Role_ID == Convert.ToInt32( txtID.Text)
                              select new
                              {
                                  a.Role_Name,
                                  a.Roll_Type,
                                  a.Role_Status_ID,
                                  a.Modules_Allowed,
                                  a.Created_By,
                                  a.Modified_BY,
                              }).ToList();
                    if (sa.Count > 0)
                    {
                        //cmbpname.SelectedValue = sa[0].Bom_Item_ID;
                        txtUserName.Text = sa[0].Role_Name.ToString();
                        cmbUserRole.Text = sa[0].Roll_Type.ToString();
                        cmbStatus.SelectedValue = sa[0].Role_Status_ID;
                        txtModules.Text = sa[0].Modules_Allowed;
                        //cmbStatus.SelectedValue = sa[0].Status;
                        lblCreatedBy.Text = sa[0].Created_By;
                        lblModified.Text = sa[0].Modified_BY;
                    }
                    string s = txtModules.Text;
                    string[] values = s.Split(',');
                    for (int j = 0; j < values.Length; j++)
                    {
                        values[j] = values[j].Trim();
                        string m = values[j].ToString();
                        for(i=0; i< lstModules.Items.Count; i++)
                        {
                            if(lstModules.Items[i].ToString()==m)
                            {
                                lstModules.SetItemChecked(i, true);
                            }
                            //else
                            //{
                            //    lstModules.SetItemChecked(i, false);
                            //}

                        }
                      
                        
                       


                    }
                    var ca = (from sq in db.User_Roles
                              where sq.Company_ID == logIn.company && sq.Role_ID == Convert.ToInt32(txtID.Text)
                              select new
                              {
                                 sq.Form_ID,
                                 sq.Form_Name,
                                  Add =sq.Create_Role,
                                  Modify=sq.Modify_Role,
                                  View= sq.View_Role,
                                 Delete=  sq.Delete_Role,
                                 Review=  sq.Review_Role,
                                  Approve = sq.Approve_Role                                 
                                 
                              });
                    SqlCommand cmd3 = (SqlCommand)db.GetCommand(ca);
                    SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                    DataTable dt1 = new DataTable();
                    da3.Fill(dt1);
                    if (dt1.Rows.Count > 0)
                        dgMenuItems.DataSource = dt1;
                    
                    decimal x = 0;
                    //for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                    //{

                    //    x += (dgProducts.Rows[i].Cells["Qty_wt"].Value == "" || dgProducts.Rows[i].Cells["Qty_wt"].Value == null || dgProducts.Rows[i].Cells["Qty_wt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty_wt"].Value);

                }
                groupBox4.Visible = false;
                //txtTotalQty.Text = x.ToString(".00");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void txtUserName_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtUserName.Text != "")
                {
                    if (txtID.Text == "")
                    {

                        if ((from u in db.User_Roles where u.Role_Name == txtUserName.Text && u.Company_ID == logIn.company select u).Count() > 0)
                        {
                            MessageBox.Show("Role Name Cannot Be Duplicate", "User Roles", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtUserName.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "User Roles", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void lstModules_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dgUserRoles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if ((from u in db.User_Setups where u.User_Role_ID == Convert.ToInt32(txtID.Text) && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    MessageBox.Show("The Role Already in Use, Cannot Be Deleted");
                }
                else
                {

                    SqlCommand cmd1 = new SqlCommand();
                    cmd1.CommandText = "delete from User_roles where Company_ID = @Company_ID and Role_ID = @Role_Name";

                    cmd1.Parameters.AddWithValue("@Company_ID", logIn.company);
                    cmd1.Parameters.AddWithValue("@Role_Name", Convert.ToInt32(txtID.Text));
                    cmd1.Connection = con;
                    con.Open();
                    cmd1.ExecuteNonQuery();
                    con.Close();
                    MessageBox.Show("Role Deleted Successfully");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "User Roles", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            groupBox4.Visible = false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }
        public void clear()
        {
            try
            {
                checkBox1.Checked = false;
                checkBox2.Checked = false;
                checkBox3.Checked = false;
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
                for (int i = 0; i < lstModules.Items.Count; i++)
                {
                    lstModules.SetItemChecked(i, false);
                }

                foreach (DataGridViewRow row in dgMenuItems.Rows)
                {

                    row.Cells["Add"].Value = 0;
                    row.Cells["Modify"].Value = 0;
                    row.Cells["View"].Value = 0;
                    row.Cells["Delete"].Value = 0;
                    row.Cells["Review"].Value = 0;
                    row.Cells["Approve"].Value = 0;


                }
                txtUserName.Text = "";
                txtModules.Text = "";
                cmbModule.Text = "";
                cmbStatus.Text = "";
                BindMasters();
                autogen();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
