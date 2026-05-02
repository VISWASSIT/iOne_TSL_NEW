using Ione_DAL;
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

namespace ioneNet.OrderManagement.Masters
{
    public partial class frmattributesdata : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        int Creation_Company = logIn.company;
        public frmattributesdata()
        {
            InitializeComponent();
        }

        public void bindattributesdata()
        {
            try
            {
                var p = (from s in db.Attributes_Datas
                         where s.Company_ID == logIn.company && s.Head_Name == txtattrheadname.Text


                         select new
                         {
                             ID = s.ID,
                             Descr = s.Descr,
                             Head_Name = s.Head_Name, 



                         }
                        );
                SqlCommand cmd1 = (SqlCommand)db.GetCommand(p);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);

                if (dt1.Rows.Count >= 0)
                {
                    dgattrdata.DataSource = dt1;
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

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void frmattributesdata_Load(object sender, EventArgs e)
        {
            txtattrdesc.Focus();
            txtattrheadname.Text = GlobalVariables.attrdesc;

            bindattributesdata();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtattrdesc.Text == string.Empty)
                {
                    MessageBox.Show("Description Name should Not be Empty", "Department Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtattrdesc.Focus();
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
                    bindattributesdata();// // // After Saving The City Details Bind City Grid                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "StorageLocation Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            

            


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {

                if (txtID.Text != "")
                {
                    var result = MessageBox.Show("Are You Sure Want to Delete this Record ", "Attributes Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {
                        var deleteattributesdata = db.Attributes_Datas.Single(course => course.ID == Convert.ToInt32(txtID.Text));
                        
                        db.SubmitChanges();
                        MessageBox.Show("Record Deleted Successfully");
                        bindattributesdata();
                        //clear1();
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
        public void Save()
        {
            try
            {
                int storageid = (txtID.Text == null || txtID.Text == "") ? 0 : Convert.ToInt32(txtID.Text);
                if ((from u in db.Attributes_Datas where u.ID == storageid && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    //if (frmGate.Modify.Contains(this.Text))
                    //{

                    var c = db.Attributes_Datas.Where(w => w.ID == (Convert.ToInt32(txtID.Text.ToString()))).FirstOrDefault();
                    {
                        //c.ID = Convert.ToInt32(txtCityId.Text.ToString());
                        c.Descr = (txtattrdesc.Text == "") ? "" : (txtattrdesc.Text);
                        c.Head_Name = (txtattrheadname.Text == "") ? "" : (txtattrheadname.Text);
                        //c.Prod_Main_Group_Id = Convert.ToInt32(cmbMainGroup.SelectedValue.ToString()); ;
                        //c.Group_PreFix = (txtGroupPrefix.Text == "") ? "" : (txtGroupPrefix.Text);

                        //c.Created_By = linkCreatedBy.Text;
                        //c.Modified_BY = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
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
                    Attributes_Data ci = new Attributes_Data();
                    ci.Descr = (txtattrdesc.Text == "") ? "" : (txtattrdesc.Text);
                    ci.Head_Name = (txtattrheadname.Text == "") ? "" : (txtattrheadname.Text);
                    //ci.Prod_Main_Group_Id = Convert.ToInt32(cmbMainGroup.SelectedValue.ToString()); ;
                    //ci.Group_PreFix = (txtGroupPrefix.Text == "") ? "" : (txtGroupPrefix.Text);
                    //ci.Created_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                    //ci.Modified_BY = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                    ci.Company_ID = logIn.company;
                    //ci.BU_ID = logIn.BU_ID;
                    //ci.Status_ID = 1;
                    db.Attributes_Datas.InsertOnSubmit(ci);
                    db.SubmitChanges();
                    MessageBox.Show("Record Saved Successfully");
                    bindattributesdata();
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


        private void dgattrdata_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtID.Text = dgattrdata.Rows[dgattrdata.CurrentRow.Index].Cells["ID"].Value.ToString();

                var d = (from po in db.Attributes_Datas
                             // join s in db.Sales_Men_Informations on po.Salesmen_Code equals s.Salesmen_Code
                         where
                         po.ID == Convert.ToInt32(txtID.Text) //&& po.Creation_Company == Creation_Company
                         select new
                         {
                             po.ID,
                             po.Descr,
                             po.Head_Name,
                             //po.Status_ID,
                             // po.Group_PreFix,
                             //po.Created_By,
                             //po.Modified_BY,

                         }).ToList();
                if (d.Count > 0)
                {

                    txtattrdesc.Text = d[0].Descr;
                    txtattrheadname.Text = d[0].Head_Name;
                    // txts.Text = d[0].MainGroup;
                    // txtGroupPrefix.Text = d[0].Group_PreFix;
                    //linkCreatedBy.Text = d[0].Created_By;
                    //linkModifiedBy.Text = d[0].Modified_BY;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtattrdesc_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtattrdesc.Text != "")
                {
                    if ((from u in db.Attributes_Datas where u.Descr == txtattrdesc.Text && u.Head_Name == txtattrheadname.Text && u.Company_ID == logIn.company  select u).Count() > 0)
                    {
                        MessageBox.Show("Descriptiont Name Cannot Be Duplicate");
                        txtattrdesc.Text = "";
                        txtattrdesc.Focus();
                        txtattrheadname.Text = "";
                        txtattrheadname.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void frmattributesdata_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.Enter)
                {
                    SendKeys.Send("{TAB}");
                }

                //if (e.Control && e.KeyCode == Keys.R)
                //    btnReset_Click(sender, e);


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
    }
}
