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

namespace ioneNet.FinanceManagement
{
    public partial class TaxClass : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        int Creation_Company = logIn.company;

        public TaxClass()
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
        public void bindGroups()
        {
            try
            {
                var p = (from s in db.Tax_Class_Masters where s.Status == 1 && s.Company_ID == logIn.company


                         select new
                         {
                             ID=s.ID,
                             Class_Name=s.Tax_Class_Name,
                             GSTRate = s.Gst_Rate
                             

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
                if (txtName.Text == string.Empty)
                {
                    MessageBox.Show("Class Name should Not be Empty", "Tax Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtName.Focus();
                    return;
                }
                else if (txtGSTRate.Text == string.Empty)
                {
                    MessageBox.Show("GST Rate Cannot be Empty", "Tax Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbCGSTLedger.Focus();
                    return;
                }
                else if (cmbStatus.Text == string.Empty)
                {
                    MessageBox.Show("Select Status", "City Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbStatus.Focus();
                    return;
                }
                else
                {
                    Save();
                    //clear1();
                    //bindGroups();// // // After Saving The City Details Bind City Grid                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Tax Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        public void Save()
        {
            try
            {
                if (txtID.Text != "")
                {
                    if ((from u in db.Tax_Class_Masters where u.ID == Convert.ToInt32(txtID.Text.ToString()) && u.Company_ID == logIn.company select u).Count() > 0)
                    {
                        //if (frmGate.Modify.Contains(this.Text))
                        //{

                        var c = db.Tax_Class_Masters.Where(w => w.ID == (Convert.ToInt32(txtID.Text.ToString())) && w.Company_ID == logIn.company).FirstOrDefault();
                        {
                            //c.ID = Convert.ToInt32(txtCityId.Text.ToString());
                            c.Tax_Class_Name = (txtName.Text == "") ? "" : (txtName.Text);
                            c.CGST_Ledger = (cmbCGSTLedger.Text == "") ? Convert.ToInt32("0") : Convert.ToInt32(cmbCGSTLedger.SelectedValue.ToString());
                            c.Gst_Rate = (txtGSTRate.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtGSTRate.Text);
                            c.SGST_Ledger = (cmbSGSTLedger.Text == "") ? Convert.ToInt32("0") : Convert.ToInt32(cmbSGSTLedger.SelectedValue.ToString());
                            c.IGST_Ledger = (cmbIGSTLedger.Text == "") ? Convert.ToInt32("0") : Convert.ToInt32(cmbIGSTLedger.SelectedValue.ToString());
                            c.Status = (cmbStatus.Text == "") ? Convert.ToInt32("0") : Convert.ToInt32(cmbStatus.SelectedValue.ToString());
                            c.Created_By = linkCreatedBy.Text;
                            c.Modified_BY = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                            db.SubmitChanges();
                            MessageBox.Show("Record Upadated Successfully");
                            clear1();
                            bindGroups();
                        }
                        //}
                        //else
                        //{
                        //    MessageBox.Show("Sorry! You Do not have privileges to Modify City");
                        //}
                    }
                }
                else
                {
                    //if (frmGate.Create_menu.Contains(this.Text))
                    //{
                    Tax_Class_Master ci = new Tax_Class_Master();
                    ci.Tax_Class_Name = (txtName.Text == "") ? "" : (txtName.Text);
                    ci.CGST_Ledger = (cmbCGSTLedger.Text == "") ? Convert.ToInt32("0") :  Convert.ToInt32(cmbCGSTLedger.SelectedValue.ToString());
                    ci.Gst_Rate = (txtGSTRate.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtGSTRate.Text);
                    ci.SGST_Ledger = (cmbSGSTLedger.Text == "") ? Convert.ToInt32("0") :  Convert.ToInt32(cmbSGSTLedger.SelectedValue.ToString());
                    ci.IGST_Ledger = (cmbIGSTLedger.Text == "") ? Convert.ToInt32("0") :  Convert.ToInt32(cmbIGSTLedger.SelectedValue.ToString());
                    ci.Created_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                    ci.Modified_BY = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                    ci.Company_ID = logIn.company;
                    ci.Status = (cmbStatus.Text == "") ? Convert.ToInt32("0") : Convert.ToInt32(cmbStatus.SelectedValue.ToString());
                    db.Tax_Class_Masters.InsertOnSubmit(ci);
                    db.SubmitChanges();
                    MessageBox.Show("Record Saved Successfully");
                    clear1();
                    bindGroups();                   
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

        private void TaxClass_Load(object sender, EventArgs e)
        {
           
            linkCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            linkModifiedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            txtName.Focus();
            bindGroups();
            bindmethod();      
        }

        public void bindmethod()
        {
            try
            {
                //var bindMainGroups = (from m in db.Product_Groups
                //                      where m.Company_ID == logIn.company && m.Status_ID == 1
                //                      select new
                //                      {
                //                          m.Prod_Group_Name,
                //                          m.Prod_Group_ID,
                //                      }).ToList();

                //if (bindMainGroups.Count > 0)
                //{
                //    cmbCGSTLedger.DisplayMember = "Prod_Group_Name";
                //    cmbCGSTLedger.ValueMember = "Prod_Group_ID";
                //    cmbCGSTLedger.DataSource = bindMainGroups;

                //}


                //Status
                var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Master" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbStatus.DataSource = pStatus;
                    cmbStatus.ValueMember = "ID";
                    cmbStatus.DisplayMember = "Descr";
                }

                //A/c Ledgers
                var CGST = (from m in db.AccountMasters where m.Company_ID == logIn.company select new { m.id, m.AccName }).Distinct().ToList();
                if (CGST.Count > 0)
                {
                    cmbCGSTLedger.DataSource = CGST;
                    cmbCGSTLedger.ValueMember = "id";
                    cmbCGSTLedger.DisplayMember = "AccName";
                }
                var SGST = (from m in db.AccountMasters where m.Company_ID == logIn.company select new { m.id, m.AccName }).Distinct().ToList();
                if (SGST.Count > 0)
                {
                    cmbSGSTLedger.DataSource = SGST;
                    cmbSGSTLedger.ValueMember = "id";
                    cmbSGSTLedger.DisplayMember = "AccName";
                }
                var IGST = (from m in db.AccountMasters where m.Company_ID == logIn.company select new { m.id, m.AccName }).Distinct().ToList();
                if (IGST.Count > 0)
                {
                    cmbIGSTLedger.DataSource = IGST;
                    cmbIGSTLedger.ValueMember = "id";
                    cmbIGSTLedger.DisplayMember = "AccName";
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

                if (txtID.Text != "")
                {
                    var result = MessageBox.Show("Are You Sure Want to Delete this Record ", "Group Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {

                        //db.sp_Citymaster_Delete(txtCityId.Text);
                        MessageBox.Show("Record Deleted Successfully");
                        bindGroups();
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
            bindGroups();
        }

        private void dgvcity_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtID.Text = dgvcity.Rows[dgvcity.CurrentRow.Index].Cells["ID"].Value.ToString();

                var d = (from po in db.Tax_Class_Masters
                             // join s in db.Sales_Men_Informations on po.Salesmen_Code equals s.Salesmen_Code
                         where
                         po.ID == Convert.ToInt32(txtID.Text) && po.Company_ID == logIn.company && po.Status ==1
                         select new
                         {
                             po.Tax_Class_Name,
                             po.Gst_Rate,
                             po.CGST_Ledger,
                             po.SGST_Ledger,
                             po.IGST_Ledger,                                                  
                             po.Created_By,
                             po.Modified_BY,     
                             po.Status,                   

                         }).ToList();
                if (d.Count > 0)
                {
                  
                    txtName.Text = d[0].Tax_Class_Name;
                    cmbCGSTLedger.SelectedValue = d[0].CGST_Ledger;
                    cmbSGSTLedger.SelectedValue = d[0].SGST_Ledger;
                    cmbIGSTLedger.SelectedValue = d[0].IGST_Ledger;
                    txtGSTRate.Text = d[0].Gst_Rate.ToString();
                    cmbStatus.SelectedValue = d[0].Status;
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
                if (txtName.Text != "")
                {
                    if ((from u in db.Product_Groups where u.Prod_Group_Name == txtName.Text && u.Company_ID == logIn.company select u).Count() > 0)
                    {
                        MessageBox.Show("Group Name Cannot Be Duplicate");
                        txtName.Text = "";
                        txtName.Focus();
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
                         where s.status == "Active" && s.Company_ID == logIn.company  && SqlMethods.Like(s.Prod_Group_Name,"%"+txtSearch) 


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
