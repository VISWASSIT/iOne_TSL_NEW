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
    public partial class StorageLocation : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        int Creation_Company = logIn.company;

        public StorageLocation()
        {
            InitializeComponent();
        }

        private void dgcity_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
        }

        // // // // Uom Names Grid Binding
        public void bindUOM()
        {
            try
            {
                var p = (from s in db.Storage_Locations
                         where s.Status_ID == 1 && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID


                         select new
                         {
                             s.Storage_Loc_Id,
                             s.Storage_Loc_Name
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
                if (txtLocationName.Text == string.Empty)
                {
                    MessageBox.Show("Location Name should Not be Empty", "Storage Location Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtLocationName.Focus();
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
                    bindUOM();// // // After Saving The City Details Bind City Grid                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Storage Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        public void Save()
        {
            try
            {
                int uomid = (txtLocID.Text == null || txtLocID.Text == "") ? 0 : Convert.ToInt32(txtLocID.Text);
                if ((from u in db.Storage_Locations where u.Storage_Loc_Id == uomid select u).Count() > 0)
                {
                    //if (frmGate.Modify.Contains(this.Text))
                    //{

                    var c = db.Storage_Locations.Where(w => w.Storage_Loc_Id == (Convert.ToInt32(txtLocID.Text.ToString()))).FirstOrDefault();
                    {
                        //c.ID = Convert.ToInt32(txtCityId.Text.ToString());
                        c.Storage_Loc_Name = (txtLocationName.Text == "") ? "" : (txtLocationName.Text);
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
                    Storage_Location ci = new Storage_Location();
                    ci.Storage_Loc_Name = (txtLocationName.Text == "") ? "" : (txtLocationName.Text);
                    //ci.Prod_Main_Group_Id = Convert.ToInt32(cmbMainGroup.SelectedValue.ToString()); ;
                    //ci.Group_PreFix = (txtGroupPrefix.Text == "") ? "" : (txtGroupPrefix.Text);
                    ci.Created_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                    ci.Modified_BY = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                    ci.Company_ID = logIn.company;
                    ci.BU_ID = logIn.BU_ID;
                    ci.Status_ID = 1;
                    db.Storage_Locations.InsertOnSubmit(ci);
                    db.SubmitChanges();
                    MessageBox.Show("Record Saved Successfully");
                    bindUOM();
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
            txtLocationName.Focus();
            bindUOM();
            //  bindmethod();      
        }

        public void bindmethod()
        {
            try
            {
                //var bindMainGroups = (from m in db.Product_Groups
                //                 where m.Company_ID == logIn.company && m.Status_ID==1
                //                 select new
                //                 {
                //                     m.Prod_Group_Name,
                //                     m.Prod_Group_ID,
                //                 }).ToList();

                //if (bindMainGroups.Count > 0)
                //{
                //    cmbMainGroup.DisplayMember = "Prod_Group_Name";
                //    cmbMainGroup.ValueMember = "Prod_Group_ID";
                //    cmbMainGroup.DataSource = bindMainGroups;

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

                if (txtLocID.Text != "")
                {
                    var result = MessageBox.Show("Are You Sure Want to Delete this Record ", "UOM Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {

                        var deletesUOM = db.Storage_Locations.Single(course => course.Storage_Loc_Id == Convert.ToInt32(txtLocID.Text));
                        deletesUOM.Status_ID = 2;
                        db.SubmitChanges();
                        MessageBox.Show("Record Deleted Successfully");
                        bindUOM();
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
            bindUOM();
        }

        private void dgvcity_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtLocID.Text = dgvcity.Rows[dgvcity.CurrentRow.Index].Cells["Storage_Loc_Id"].Value.ToString();

                var d = (from po in db.Storage_Locations
                             // join s in db.Sales_Men_Informations on po.Salesmen_Code equals s.Salesmen_Code
                         where
                         po.Storage_Loc_Id == Convert.ToInt32(txtLocID.Text) //&& po.Creation_Company == Creation_Company
                         select new
                         {
                             po.Storage_Loc_Name,
                             //po.MainGroup,
                             po.Status_ID,
                             // po.Group_PreFix,
                             po.Created_By,
                             po.Modified_BY,

                         }).ToList();
                if (d.Count > 0)
                {

                    txtLocationName.Text = d[0].Storage_Loc_Name;
                    //cmbMainGroup.Text = d[0].MainGroup;
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
                if (txtLocationName.Text != "")
                {
                    if ((from u in db.Storage_Locations where u.Storage_Loc_Name == txtLocationName.Text && u.Company_ID == logIn.company && u.BU_ID  == logIn.BU_ID select u).Count() > 0)
                    {
                        MessageBox.Show("Storage Location Name Cannot Be Duplicate");
                        txtLocationName.Text = "";
                        txtLocationName.Focus();
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
                string uomname = "%" + txtSearch.Text + "%";
                var p = (from s in db.Storage_Locations
                         where s.Status_ID == 1 && s.Company_ID == logIn.company  && s.BU_ID == logIn.BU_ID && SqlMethods.Like(s.Storage_Loc_Name, uomname)


                         select new
                         {
                              s.Storage_Loc_Id,
                              s.Storage_Loc_Name
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
    }
}
