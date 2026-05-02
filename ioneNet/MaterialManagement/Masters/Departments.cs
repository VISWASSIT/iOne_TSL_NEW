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
using Ione_DAL;
namespace ioneNet.MaterialManagement
{
    public partial class Departments : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        int Creation_Company = logIn.company;

        public Departments()
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
        public void bindstoragelocations()
        {
            try
            {
                var p = (from s in db.Department_Masters
                         where s.Status_ID == 1 && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID


                         select new
                         {
                             ID = s.Id,
                             Dept_Name = s.Dept_Name,
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
                if (txtStorageLocationName.Text == string.Empty)
                {
                    MessageBox.Show("Department Name should Not be Empty", "Department Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtStorageLocationName.Focus();
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
                    bindstoragelocations();// // // After Saving The City Details Bind City Grid                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "StorageLocation Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        public void Save()
        {
            try
            {
                int storageid = (txtStorageLocationID.Text == null || txtStorageLocationID.Text == "") ? 0 : Convert.ToInt32(txtStorageLocationID.Text);
                if ((from u in db.Department_Masters where u.Id == storageid && u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID select u).Count() > 0)
                {
                    //if (frmGate.Modify.Contains(this.Text))
                    //{

                    var c = db.Department_Masters.Where(w => w.Id == (Convert.ToInt32(txtStorageLocationID.Text.ToString()))).FirstOrDefault();
                    {
                        //c.ID = Convert.ToInt32(txtCityId.Text.ToString());
                        c.Dept_Name = (txtStorageLocationName.Text == "") ? "" : (txtStorageLocationName.Text);
                        //c.Prod_Main_Group_Id = Convert.ToInt32(cmbMainGroup.SelectedValue.ToString()); ;
                        //c.Group_PreFix = (txtGroupPrefix.Text == "") ? "" : (txtGroupPrefix.Text);
                        c.BU_ID = logIn.BU_ID;
                        c.Created_By = linkCreatedBy.Text;
                        c.Modified_BY = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                        db.SubmitChanges();
                        MessageBox.Show("Record Upadated Successfully");
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
                    Department_Master ci = new Department_Master();
                    ci.Dept_Name = (txtStorageLocationName.Text == "") ? "" : (txtStorageLocationName.Text);
                    //ci.Prod_Main_Group_Id = Convert.ToInt32(cmbMainGroup.SelectedValue.ToString()); ;
                    //ci.Group_PreFix = (txtGroupPrefix.Text == "") ? "" : (txtGroupPrefix.Text);
                    ci.Created_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                    ci.Modified_BY = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                    ci.Company_ID = logIn.company;
                    ci.BU_ID = logIn.BU_ID;
                    ci.Status_ID = 1;
                    db.Department_Masters.InsertOnSubmit(ci);
                    db.SubmitChanges();
                    MessageBox.Show("Record Saved Successfully");
                    bindstoragelocations();
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
            txtStorageLocationName.Focus();
            bindstoragelocations();
            //  bindmethod();      
        }

        //public void bindmethod()
        //{
        //    try
        //    {
        //        var bindMainGroups = (from m in db.Product_Groups
        //                         where m.Company_ID == logIn.company && m.Status_ID==1
        //                         select new
        //                         {
        //                             m.Prod_Group_Name,
        //                             m.Prod_Group_ID,
        //                         }).ToList();

        //        if (bindMainGroups.Count > 0)
        //        {
        //            cmbMainGroup.DisplayMember = "Prod_Group_Name";
        //            cmbMainGroup.ValueMember = "Prod_Group_ID";
        //            cmbMainGroup.DataSource = bindMainGroups;

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
        //}

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

                if (txtStorageLocationID.Text != "")
                {
                    var result = MessageBox.Show("Are You Sure Want to Delete this Record ", "StorageLocation Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        var deletestoragelocation = db.Department_Masters.Single(course => course.Id == Convert.ToInt32(txtStorageLocationID.Text));
                        deletestoragelocation.Status_ID = 2;
                        db.SubmitChanges();
                        MessageBox.Show("Record Deleted Successfully");
                        bindstoragelocations();
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
            bindstoragelocations();
        }

        private void dgvcity_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtStorageLocationID.Text = dgvcity.Rows[dgvcity.CurrentRow.Index].Cells["ID"].Value.ToString();

                var d = (from po in db.Department_Masters
                             // join s in db.Sales_Men_Informations on po.Salesmen_Code equals s.Salesmen_Code
                         where
                         po.Id == Convert.ToInt32(txtStorageLocationID.Text) //&& po.Creation_Company == Creation_Company
                         select new
                         {
                             po.Id,
                             po.Dept_Name,
                             po.Status_ID,
                             // po.Group_PreFix,
                             po.Created_By,
                             po.Modified_BY,

                         }).ToList();
                if (d.Count > 0)
                {

                    txtStorageLocationName.Text = d[0].Dept_Name;
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

        private void txtGroupname_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtStorageLocationName.Text != "")
                {
                    if ((from u in db.Department_Masters where u.Dept_Name == txtStorageLocationName.Text && u.Company_ID == logIn.company && u.BU_ID ==logIn.BU_ID select u).Count() > 0)
                    {
                        MessageBox.Show("Department Name Cannot Be Duplicate");
                        txtStorageLocationName.Text = "";
                        txtStorageLocationName.Focus();
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
                string storagename = "%" + txtSearch.Text + "%";
                var p = (from s in db.Department_Masters
                         where s.Status_ID == 1 && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID  && SqlMethods.Like(s.Dept_Name, storagename)


                         select new
                         {
                             ID = s.Id,
                             Dept_Name = s.Dept_Name,
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

        private void txtStorageLocationID_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
