using Ione_DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Linq.SqlClient;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ioneNet.MaterialManagement.Masters
{
    public partial class Scope_of_work : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        int Creation_Company = logIn.company;
        public Scope_of_work()
        {
            InitializeComponent();
        }

        private void Scope_of_work_Load(object sender, EventArgs e)
        {
            linkCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            linkModifiedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            txtScopeName.Focus();
            bindsname();
        }
        public void bindsname()
        {
            try
            {
                var p = (from s in db.Scopeofwork_Masters
                         where s.Status_ID == 1 && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID


                         select new
                         {
                             ID = s.S_ID,
                             Scope_of_Work_Name = s.Scope_Name
                             // Main_Group=s.MainGroup


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
                if (txtScopeName.Text == string.Empty)
                {
                    MessageBox.Show("Scope of Work Name should Not be Empty","", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtScopeName.Focus();
                    return;
                }
                //else if (cmbMainGroup.Text == string.Empty)
                //{
                //    MessageBox.Show("Please Select Main Group..", "Group Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    cmbMainGroup.Focus();
                //    return;
                //}
                //else if (txtStateCode.Text == string.Empty)
                //{
                //    MessageBox.Show("Enter State Code", "City Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    txtStateCode.Focus();
                //    return;
                //}
                else
                {
                    Save();
                    clear1();
                    bindsname();// // // After Saving The City Details Bind City Grid                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Scope of work Name", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
        public void Save()
        {
            try
            {
                int scopeid = (txtScopeID.Text == null || txtScopeID.Text == "") ? 0 : Convert.ToInt32(txtScopeID.Text);
                if ((from u in db.Scopeofwork_Masters where u.S_ID == scopeid && u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID select u).Count() > 0)
                {
                    //if (frmGate.Modify.Contains(this.Text))
                    //{

                    var c = db.Scopeofwork_Masters.Where(w => w.S_ID == scopeid).FirstOrDefault();
                    {
                        //c.ID = Convert.ToInt32(txtCityId.Text.ToString());
                        c.Scope_Name = (txtScopeName.Text == "") ? "" : (txtScopeName.Text);
                        //c.Prod_Main_Group_Id = Convert.ToInt32(cmbMainGroup.SelectedValue.ToString()); ;
                        //c.Group_PreFix = (txtGroupPrefix.Text == "") ? "" : (txtGroupPrefix.Text);
                        c.BU_ID = logIn.BU_ID;
                        c.Created_By = linkCreatedBy.Text;
                        c.Modified_BY = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                        db.SubmitChanges();
                        MessageBox.Show("Record Updated Successfully");
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
                    Scopeofwork_Master ci = new Scopeofwork_Master();
                    ci.Scope_Name = (txtScopeName.Text == "") ? "" : (txtScopeName.Text);
                    //ci.Prod_Main_Group_Id = Convert.ToInt32(cmbMainGroup.SelectedValue.ToString()); ;
                    //ci.Group_PreFix = (txtGroupPrefix.Text == "") ? "" : (txtGroupPrefix.Text);
                    ci.Created_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                    ci.Modified_BY = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                    ci.Company_ID = logIn.company;
                    ci.BU_ID = logIn.BU_ID;
                    ci.Status_ID = 1;
                    db.Scopeofwork_Masters.InsertOnSubmit(ci);
                    db.SubmitChanges();
                    MessageBox.Show("Record Saved Successfully");
                    bindsname();
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

                if (txtScopeID.Text != "")
                {
                    var result = MessageBox.Show("Are You Sure Want to Delete this Record ", "Scope of work Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        var deletescopename = db.Scopeofwork_Masters.Single(course => course.S_ID == Convert.ToInt32(txtScopeID.Text));
                        deletescopename.Status_ID = 2;
                        db.SubmitChanges();
                        MessageBox.Show("Record Deleted Successfully");
                        bindsname();
                        clear1();
                    }
                }
                else
                {
                    MessageBox.Show("Please Select the Record", "Group Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            bindsname();
        }

        private void dgvcity_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                txtScopeID.Text = dgvcity.Rows[dgvcity.CurrentRow.Index].Cells["ID"].Value.ToString();

                var d = (from po in db.Scopeofwork_Masters
                             // join s in db.Sales_Men_Informations on po.Salesmen_Code equals s.Salesmen_Code
                         where
                         po.S_ID == Convert.ToInt32(txtScopeID.Text) //&& po.Creation_Company == Creation_Company
                         select new
                         {
                             po.S_ID,
                             po.Scope_Name,
                             po.Status_ID,
                             // po.Group_PreFix,
                             po.Created_By,
                             po.Modified_BY,

                         }).ToList();
                if (d.Count > 0)
                {

                    txtScopeName.Text = d[0].Scope_Name;
                    // txts.Text = d[0].MainGroup;
                    // txtGroupPrefix.Text = d[0].Group_PreFix;
                    linkCreatedBy.Text = d[0].Created_By;
                    linkModifiedBy.Text = d[0].Modified_BY;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtStorageLocationName_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtScopeName.Text != "" && txtScopeID.Text=="")
                {
                    if ((from u in db.Scopeofwork_Masters where u.Scope_Name == txtScopeName.Text && u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID select u).Count() > 0)
                    {
                        MessageBox.Show("Scope of work Name Cannot Be Duplicate");
                        txtScopeName.Text = "";
                        txtScopeName.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                string sname = "%" + txtSearch.Text + "%";
                var p = (from s in db.Scopeofwork_Masters
                         where s.Status_ID == 1 && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID && SqlMethods.Like(s.Scope_Name, sname)


                         select new
                         {
                             ID = s.S_ID,
                             Dept_Name = s.Scope_Name,
                             // Main_Group = s.MainGroup


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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtScopeID_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
