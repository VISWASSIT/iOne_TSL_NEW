using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using Ione_DAL;
namespace ioneNet.Masters
{
    public partial class frmCityMaster : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        int Creation_Company = logIn.company;

        public frmCityMaster()
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
        public void bindcity()
        {
            try
            {
                var p = (from s in db.City_Masters

                         select new
                         {
                             s.ID,
                             s.City_Name, 
                             s.State_Name,
                             s.State_Code

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
            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtcityname.Text == string.Empty)
                {
                    MessageBox.Show("City Name should Not be Empty", "City Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtcityname.Focus();
                    return;
                }
                else if (cmbState.Text == string.Empty)
                {
                    MessageBox.Show("Please Select State..", "City Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbState.Focus();
                    return;
                }
                else if (txtStateCode.Text == string.Empty)
                {
                    MessageBox.Show("Enter State Code", "City Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtStateCode.Focus();
                    return;
                }
                else
                {
                    Save();
                    clear1();
                    bindcity();// // // After Saving The City Details Bind City Grid
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "City Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        public void Save()
        {
            try
            {
                if ((from u in db.City_Masters where u.City_Name == (txtcityname.Text) select u).Count() > 0)
                {
                    //if (frmGate.Modify.Contains(this.Text))
                    //{

                        var c = db.City_Masters.Where(w => w.City_Name == (txtcityname.Text)).FirstOrDefault();
                        {
                            //c.ID = Convert.ToInt32(txtCityId.Text.ToString());
                            c.City_Name = (txtcityname.Text == "") ? "" : (txtcityname.Text);
                            c.State_Name = (cmbState.Text == "") ? "" : (cmbState.Text);
                            c.State_Code = (txtStateCode.Text == "") ? "" : (txtStateCode.Text);
                                                  

                            // c.Created_By = frmLogin.UserName.ToString();
                            //c.Modified_By = logIn.username.ToString();
                            //// c.Created_Date = Convert.ToDateTime(DateTime.Now.ToString());
                            //c.Modified_Date = Convert.ToDateTime(DateTime.Now.ToString());
                            //c.Creation_Company = Creation_Company;
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
                        City_Master ci = new City_Master();
                        //string company = Creation_Company;
                        //var result = db.Sp_autoincrement_CityMaster(company);
                        //ci.City_Id = result.FirstOrDefault().City_Id;
                        ci.City_Name = (txtcityname.Text == "") ? "" : (txtcityname.Text);
                        ci.State_Name = (cmbState.Text == "") ? "" : (cmbState.Text);
                        ci.State_Code = (txtStateCode.Text == "") ? "" : (txtStateCode.Text);                       
                        db.City_Masters.InsertOnSubmit(ci);
                        db.SubmitChanges();
                        MessageBox.Show("Record Saved Successfully");
                        bindcity();
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
            //if (!frmGate.Create_menu.Contains(this.Text))
            //{
            //    btnSubmit.Enabled = false;
            //}
            //if (!frmGate.Modify.Contains(this.Text))
            //{
            //    dgvcity.Enabled = false;
            //    btnDelete.Enabled = false;
            //}
            //if (!frmGate.Authorization_Menus.Contains(this.Text))
            //{
            //    btnDelete.Enabled = false;
            //}
            txtcityname.Focus();
            bindcity();
            //lnkus1.Text = frmLogin.UserName;
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lnkus2.Text = frmLogin.UserName;
            //lbldt2.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
        }

        public void bindmethod(int Id)
        {
            try
            {
                var p = (from s in db.City_Masters
                         where s.ID == Id 
                         select new
                         {
                             s.ID,
                             s.City_Name,
                             s.State_Name,
                             s.State_Code,
                            

                         }
                        ).ToList();
                if (p.Count > 0)
                {
                  
                    //txtCityId.Text = p[0].id;
                    txtcityname.Text = p[0].City_Name;
                    cmbState.Text = p[0].State_Name;
                    txtStateCode.Text = p[0].State_Code;                    
                }
                else
                {

                }

            }
            catch (Exception ex)
            {

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

                if (txtCityId.Text != "")
                {
                    var result = MessageBox.Show("Are You Sure Want to Delete this Record ", "City Information", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                    if (result == DialogResult.Yes)
                    {

                        SqlCommand cmd = new SqlCommand();

                        //SO_No = cellVaue.ToString();
                        cmd.CommandText = "Delete from City_Masters  where ID =@param1";
                        cmd.Parameters.AddWithValue("@param1", txtCityId.Text);
                        //cmd.Parameters.AddWithValue("@CompName", logIn.company);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Record Deleted Successfully");
                        bindcity();
                        clear1();
                    }
                }
                else
                {
                    MessageBox.Show("Please Select the Record", "City Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            bindcity();
        }

        private void dgvcity_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtCityId.Text = dgvcity.Rows[dgvcity.CurrentRow.Index].Cells["ID"].Value.ToString();
                bindmethod(Convert.ToInt32(txtCityId.Text.ToString()));
            }
            catch (Exception ex)
            {
            }
        }

        private void txtcityname_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtcityname.Text != "")
                {
                    if ((from u in db.City_Masters where u.City_Name == txtcityname.Text select u).Count() > 0)
                    {
                        MessageBox.Show("City Name Already Exists..,Please Try Another One");
                        txtcityname.Text = "";
                        txtcityname.Focus();
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
    }
}
