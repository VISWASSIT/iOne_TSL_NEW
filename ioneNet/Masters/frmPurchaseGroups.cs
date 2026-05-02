using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Linq.SqlClient;
using System.Configuration;
using Ione_DAL;
using Syncfusion.Pdf.Graphics;

namespace ioneNet.Masters
{
    public partial class frmPurchaseGroups : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmPurchaseGroups()
        {
            InitializeComponent();
        }

        private void frmPurchaseGroups_Load(object sender, EventArgs e)
        {
            bindMaster();
            bindGroups();
        }

        public void bindMaster()
        {
            try
            {
                using (SqlCommand cmd = new SqlCommand("SELECT distinct Prod_Type,Prod_Type_Id FROM [Attributes_Prod_Types] where company_id = @CompName  order by Prod_Type_Id", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@CompName", logIn.company);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                checkedListBox1.Items.Add(dt.Rows[i]["Prod_Type"].ToString());
                            }

                        }
                    }
                }

                using (SqlCommand cmd = new SqlCommand("SELECT distinct Descr,ID FROM [Attributes_Data] where head_name = 'GRN Type' and company_id = @CompName", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@CompName", logIn.company);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                checkedListBox3.Items.Add(dt.Rows[i]["Descr"].ToString());
                            }

                        }
                    }
                }

                using (SqlCommand cmd = new SqlCommand("SELECT distinct User_Name,User_ID FROM [User_Setup] where company_id = @CompName  order by User_ID", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@CompName", logIn.company);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                checkedListBox2.Items.Add(dt.Rows[i]["User_Name"].ToString());
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void bindGroups()
        {
            try
            {
                var p = (from s in db.Purchase_Group_Masters
                         where  s.Company_id == logIn.company


                         select new
                         {
                             ID = s.id,
                             Group_Name = s.Purchase_Group_Name,                            


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

        private void dgvcity_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtGroupID.Text = dgvcity.Rows[dgvcity.CurrentRow.Index].Cells["ID"].Value.ToString();

                var d = (from po in db.Purchase_Group_Masters
                             // join s in db.Sales_Men_Informations on po.Salesmen_Code equals s.Salesmen_Code
                         where
                         po.id == Convert.ToInt32(txtGroupID.Text) //&& po.Creation_Company == Creation_Company
                         select new
                         {
                             po.Purchase_Group_Name,
                             po.Product_Types,
                             po.GRN_Type,
                             po.User_Name,                           
                             po.Created_By,
                             po.Modified_BY
                             


                         }).ToList();
                if (d.Count > 0)
                {

                    txtGroupName.Text = d[0].Purchase_Group_Name;
                    //cmbMainGroup.Text = d[0].MainGroup;
                    //txtGroupPrefix.Text = d[0].Group_PreFix;
                    //comboBox1.SelectedValue = Convert.ToInt32(d[0].Prod_Type);
                    linkCreatedBy.Text = d[0].Created_By;
                    linkModifiedBy.Text = d[0].Modified_BY;

                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        checkedListBox1.SetItemChecked(i, false);
                       
                    }
                    for (int i = 0; i < checkedListBox2.Items.Count; i++)
                    {
                        checkedListBox2.SetItemChecked(i, false);

                    }
                    for (int i = 0; i < checkedListBox3.Items.Count; i++)
                    {
                        checkedListBox3.SetItemChecked(i, false);

                    }


                    if (d[0].Product_Types != null)
                    {
                        string MP = d[0].Product_Types.ToString();
                        string[] values = MP.Split(',');
                        for (int j = 0; j < values.Length; j++)
                        {
                            values[j] = values[j].Trim();
                            string m = values[j].ToString();
                            for (int i = 0; i < checkedListBox1.Items.Count; i++)
                            {
                                if (checkedListBox1.Items[i].ToString() == m)
                                {
                                    checkedListBox1.SetItemChecked(i, true);
                                }
                                else
                                {
                                    //checkedListBox1.SetItemChecked(i, false);
                                }
                            }
                        }
                    }


                    if (d[0].User_Name != null)
                    {
                        string MP = d[0].User_Name.ToString();
                        string[] values = MP.Split(',');
                        for (int j = 0; j < values.Length; j++)
                        {
                            values[j] = values[j].Trim();
                            string m = values[j].ToString();
                            for (int i = 0; i < checkedListBox2.Items.Count; i++)
                            {
                                if (checkedListBox2.Items[i].ToString() == m)
                                {
                                    checkedListBox2.SetItemChecked(i, true);
                                }
                                else
                                {
                                    //checkedListBox2.SetItemChecked(i, false);
                                }
                            }
                        }
                    }

                    if (d[0].GRN_Type != null)
                    {
                        string MP = d[0].GRN_Type.ToString();
                        string[] values = MP.Split(',');
                        for (int j = 0; j < values.Length; j++)
                        {
                            values[j] = values[j].Trim();
                            string m = values[j].ToString();
                            for (int i = 0; i < checkedListBox3.Items.Count; i++)
                            {
                                if (checkedListBox3.Items[i].ToString() == m)
                                {
                                    checkedListBox3.SetItemChecked(i, true);
                                }
                                else
                                {
                                    //checkedListBox2.SetItemChecked(i, false);
                                }
                            }
                        }
                    }

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
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtGroupName.Text != "")
            {
                Save();
            }
            else
            {
                MessageBox.Show("Enter Purchase Group Name");
                txtGroupName.Focus();
                return;
            }
        }

        public void Save()
        {
            try
            {

                //if ((from u in db.Product_Groups where u.ID == Convert.ToInt32(txtGroupID.Text) && u.Company_ID == logIn.company select u).Count() > 0)
                if (txtGroupID.Text != "")
                {

                    //if (frmGate.Modify.Contains(this.Text))
                    //{

                    var c = db.Purchase_Group_Masters.Where(w => w.id == Convert.ToInt32(txtGroupID.Text)).FirstOrDefault();
                    {
                        //c.ID = Convert.ToInt32(txtCityId.Text.ToString());
                        c.Purchase_Group_Name = (txtGroupName.Text == "") ? "" : (txtGroupName.Text);
                        string Prod_Types = "";
                        for (int i = 0; i < checkedListBox1.Items.Count; i++)
                        {
                            if (checkedListBox1.GetItemChecked(i))
                            {
                                if (Prod_Types != "")
                                {
                                    Prod_Types = Prod_Types + "," + checkedListBox1.Items[i].ToString();
                                }
                                else
                                {
                                    Prod_Types = checkedListBox1.Items[i].ToString();
                                }
                            }
                        }

                        c.Product_Types = Prod_Types;

                        string User_Names = "";
                        for (int i = 0; i < checkedListBox2.Items.Count; i++)
                        {
                            if (checkedListBox2.GetItemChecked(i))
                            {
                                if (User_Names != "")
                                {
                                    User_Names = User_Names + "," + checkedListBox2.Items[i].ToString();
                                }
                                else
                                {
                                    User_Names = checkedListBox2.Items[i].ToString();
                                }
                            }
                        }

                        c.User_Name = User_Names;

                        string Grn_Type = "";
                        for (int i = 0; i < checkedListBox3.Items.Count; i++)
                        {
                            if (checkedListBox3.GetItemChecked(i))
                            {
                                if (Grn_Type != "")
                                {
                                    Grn_Type = Grn_Type + "," + checkedListBox3.Items[i].ToString();
                                }
                                else
                                {
                                    Grn_Type = checkedListBox3.Items[i].ToString();
                                }
                            }
                        }

                        c.GRN_Type = Grn_Type;

                        c.Created_By = linkCreatedBy.Text;
                        c.Modified_BY = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                        db.SubmitChanges();
                        MessageBox.Show("Record Upadated Successfully");
                        //clear1();
                        
                    }
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
                    Purchase_Group_Master ci = new Purchase_Group_Master();
                    ci.Purchase_Group_Name = (txtGroupName.Text == "") ? "" : (txtGroupName.Text);
                    string Prod_Types = "";
                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        if (checkedListBox1.GetItemChecked(i))
                        {
                            if (Prod_Types != "")
                            {
                                Prod_Types = Prod_Types + "," + checkedListBox1.Items[i].ToString();
                            }
                            else
                            {
                                Prod_Types = checkedListBox1.Items[i].ToString();
                            }
                        }
                    }

                    ci.Product_Types = Prod_Types;

                    string User_Names = "";
                    for (int i = 0; i < checkedListBox2.Items.Count; i++)
                    {
                        if (checkedListBox2.GetItemChecked(i))
                        {
                            if (User_Names != "")
                            {
                                User_Names = User_Names + "," + checkedListBox2.Items[i].ToString();
                            }
                            else
                            {
                                User_Names = checkedListBox2.Items[i].ToString();
                            }
                        }
                    }

                    ci.User_Name = User_Names;

                    string Grn_Type = "";
                    for (int i = 0; i < checkedListBox3.Items.Count; i++)
                    {
                        if (checkedListBox3.GetItemChecked(i))
                        {
                            if (Grn_Type != "")
                            {
                                Grn_Type = Grn_Type + "," + checkedListBox3.Items[i].ToString();
                            }
                            else
                            {
                                Grn_Type = checkedListBox3.Items[i].ToString();
                            }
                        }
                    }

                    ci.GRN_Type = Grn_Type;
                    ci.Created_By = linkCreatedBy.Text;
                    ci.Modified_BY = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                    ci.Company_id = logIn.company;
                    db.Purchase_Group_Masters.InsertOnSubmit(ci);
                    db.SubmitChanges();
                    MessageBox.Show("Record Saved Successfully");
                    
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

        private void cmdDelete_Click(object sender, EventArgs e)
        {

        }
    }
}
