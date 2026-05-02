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
using System.Configuration;
using Ione_DAL;
namespace ioneNet.Masters
{
    public partial class frmForge_EmployeeMasters : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        int Creation_Company = logIn.company;

        public frmForge_EmployeeMasters()
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
                var p = (from s in db.Forging_Employees where s.Company_ID == logIn.company


                         select new
                         {
                             ID=s.id,
                             s.Employee_Name,
                             s.Designation,
                             s.Department


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
                if (txtEmpName.Text == string.Empty)
                {
                    MessageBox.Show("Employee Name should Not be Empty", "Employee Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtEmpName.Focus();
                    return;
                }
                else if (cmbDesignation.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Designation..", "Employee Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbDesignation.Focus();
                    return;
                }
                else if (cmbDepartment.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Department..", "Employee Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbDepartment.Focus();
                    return;
                }
                else
                {
                    Save();
                    
                    bindGroups();// // // After Saving The City Details Bind City Grid                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Machine Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        public void Save()
        {
            try
            {
               
                    //if ((from u in db.Product_Groups where u.ID == Convert.ToInt32(txtGroupID.Text) && u.Company_ID == logIn.company select u).Count() > 0)
                    if(txtGroupID.Text !="")
                {

                            //if (frmGate.Modify.Contains(this.Text))
                            //{

                            var c = db.Forging_Employees.Where(w => w.id == Convert.ToInt32(txtGroupID.Text)).FirstOrDefault();
                        {
                            //c.ID = Convert.ToInt32(txtCityId.Text.ToString());
                            c.Employee_Name = (txtEmpName.Text == "") ? "" : (txtEmpName.Text);                            
                            c.Designation = cmbDesignation.Text ;
                            c.Department = cmbDepartment.Text;
                        //c.Group_PreFix = (txtGroupPrefix.Text == "") ? "" : (txtGroupPrefix.Text);

                            c.Created_By = linkCreatedBy.Text;
                            c.Modified_BY = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt"); 
                            db.SubmitChanges();
                            MessageBox.Show("Record Upadated Successfully");
                        clear1();
                        //autogen();                        }
                    }
                    //else
                    //{
                    //    MessageBox.Show("Sorry! You Do not have privileges to Modify City");
                    //}
                }
                else
                {
                    //if (frmGate.Create_menu.Contains(this.Text))
                    //{
                        Forging_Employee ci = new Forging_Employee();
                   // ci.Machine_ID = (txtGroupID.Text == "") ? "" : (txtGroupID.Text);
                    ci.Employee_Name = (txtEmpName.Text == "") ? "" : (txtEmpName.Text);
                                       {
                        ci.Designation = cmbDesignation.Text ;
                        ci.Department = cmbDepartment.Text;
                    }
                   // ci.Group_PreFix = (txtGroupPrefix.Text == "") ? "" : (txtGroupPrefix.Text);
                    ci.Created_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                    ci.Modified_BY = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                    ci.Company_ID = logIn.company;                   
                    db.Forging_Employees.InsertOnSubmit(ci);
                        db.SubmitChanges();
                        MessageBox.Show("Record Saved Successfully");
                    clear1();
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

        private void frmCityMaster_Load(object sender, EventArgs e)
        {
           
            linkCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            linkModifiedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            txtEmpName.Focus();
            bindGroups();
            //bindmethod();
            //autogen();      
        }
        public void autogen()
        {
            //var result = db.Sp_autoincrement_Machinery_Master(logIn.company);
            //txtGroupID.Text = result.FirstOrDefault().Machine_ID;
        }
        public void bindmethod()
        {
            try
            {

               


                var bindMainGroups = (from m in db.Forging_ProcessMasters
                                      where m.Company_ID == logIn.company && m.Status_ID == 1
                                      select new
                                      {
                                          m.Process_Name,
                                          m.ID,
                                      }).ToList();

                if (bindMainGroups.Count > 0)
                {
                    cmbDesignation.DisplayMember = "Process_Name";
                    cmbDesignation.ValueMember = "ID";
                    cmbDesignation.DataSource = bindMainGroups;

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
                if (txtGroupID.Text != "")
                {

                        DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            int ProdID;
                            ProdID = Convert.ToInt32(txtGroupID.Text);
                            SqlCommand cmd1 = new SqlCommand("delete  from [Forging_MachineMaster] where id =@ProdID abd Company_ID = @compID", con);
                            cmd1.Parameters.AddWithValue("@ProdID", ProdID);
                            cmd1.Parameters.AddWithValue("@compID", logIn.company);

                        if (con.State != ConnectionState.Open)
                                con.Open();
                            //con.Open();
                            cmd1.ExecuteNonQuery();
                            con.Close();
                            MessageBox.Show("Process Deleted Successfully");
                        }

                }
                else
                {
                    MessageBox.Show("Please Select Process to Delete");
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

        private void btnReset_Click(object sender, EventArgs e)
        {
            clear1();
            bindGroups();
        }

        private void dgvcity_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtGroupID.Text = dgvcity.Rows[dgvcity.CurrentRow.Index].Cells["ID"].Value.ToString();                
                
                var d = (from po in db.Forging_Employees
                             // join s in db.Sales_Men_Informations on po.Salesmen_Code equals s.Salesmen_Code
                         where
                         po.id == Convert.ToInt32(txtGroupID.Text) //&& po.Creation_Company == Creation_Company
                         select new
                         {
                             po.Employee_Name,
                             po.Designation,
                             po.Department,
                             // po.Group_PreFix,
                             po.Created_By,
                             po.Modified_BY,                        

                         }).ToList();
                if (d.Count > 0)
                {
                  
                    txtEmpName.Text = d[0].Employee_Name;
                    cmbDesignation.Text = d[0].Designation;
                    cmbDepartment.Text = d[0].Department;
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
                if (txtEmpName.Text != "")
                {
                    if ((from u in db.Forging_Employees where u.Employee_Name == txtEmpName.Text && u.Company_ID == logIn.company select u).Count() > 0)
                    {
                        MessageBox.Show("Employee Name Cannot Be Duplicate");
                        txtEmpName.Text = "";
                        txtEmpName.Focus();
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
                var p = (from s in db.Forging_Employees
                         where s.Company_ID == logIn.company  && SqlMethods.Like(s.Employee_Name,"%"+txtSearch.Text +"%") 


                         select new
                         {
                             ID = s.id,
                             s.Employee_Name,
                             s.Designation,
                             s.Department


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
