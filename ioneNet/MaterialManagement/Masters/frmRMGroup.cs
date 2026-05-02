using Ione_DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ioneNet.MaterialManagement.Masters
{
    public partial class frmRMGroup : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public frmRMGroup()
        {
            InitializeComponent();
        }

        private void frmRMGroup_Load(object sender, EventArgs e)
        {
            try
            {
                bindGroups();
                using (SqlCommand cmd = new SqlCommand("SELECT distinct Prod_Name FROM [Products] where Prod_Type_Id = @ptypeid", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@ptypeid", "140");
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (System.Data.DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                checkedListBox1.Items.Add(dt.Rows[i]["Prod_Name"].ToString());
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
                var p = (from s in db.RM_Group_Masters
                         where s.Company_ID == logIn.company


                         select new
                         {
                             ID = s.id,
                             Group_Name = s.RM_Group_Name,


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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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

                    var c = db.RM_Group_Masters.Where(w => w.id == Convert.ToInt32(txtGroupID.Text)).FirstOrDefault();
                    {
                        //c.ID = Convert.ToInt32(txtCityId.Text.ToString());
                        c.RM_Group_Name = (txtGroupName.Text == "") ? "" : (txtGroupName.Text);
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

                        c.RM_Section_Name = Prod_Types;

                        c.Created_By = linkCreatedBy.Text;
                        c.Modified_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                        db.SubmitChanges();
                        MessageBox.Show("Record Upadated Successfully");
                        //clear1();

                    }
                    
                }
                else
                {
                    //if (frmGate.Create_menu.Contains(this.Text))
                    //{
                    RM_Group_Master ci = new RM_Group_Master();
                    ci.RM_Group_Name = (txtGroupName.Text == "") ? "" : (txtGroupName.Text);
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

                    ci.RM_Section_Name = Prod_Types;

                    

                    
                    ci.Created_By = linkCreatedBy.Text;
                    ci.Modified_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                    ci.Company_ID = logIn.company;
                    db.RM_Group_Masters.InsertOnSubmit(ci);
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

        private void dgvcity_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtGroupID.Text = dgvcity.Rows[dgvcity.CurrentRow.Index].Cells["ID"].Value.ToString();

                var d = (from po in db.RM_Group_Masters
                             // join s in db.Sales_Men_Informations on po.Salesmen_Code equals s.Salesmen_Code
                         where
                         po.id == Convert.ToInt32(txtGroupID.Text) //&& po.Creation_Company == Creation_Company
                         select new
                         {
                             po.RM_Group_Name,
                             po.RM_Section_Name,                           
                             po.Created_By,
                             po.Modified_By



                         }).ToList();
                if (d.Count > 0)
                {

                    txtGroupName.Text = d[0].RM_Group_Name;
                    //cmbMainGroup.Text = d[0].MainGroup;
                    //txtGroupPrefix.Text = d[0].Group_PreFix;
                    //comboBox1.SelectedValue = Convert.ToInt32(d[0].Prod_Type);
                    linkCreatedBy.Text = d[0].Created_By;
                    linkModifiedBy.Text = d[0].Modified_By;

                    for (int i = 0; i < checkedListBox1.Items.Count; i++)
                    {
                        checkedListBox1.SetItemChecked(i, false);

                    }
                   


                    if (d[0].RM_Section_Name != null)
                    {
                        string MP = d[0].RM_Section_Name.ToString();
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


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
