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
    public partial class FinancialYear : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        int Creation_Company = logIn.company;
        public FinancialYear()
        {
            InitializeComponent();
        }
        private void FinancialYear_Load(object sender, EventArgs e)
        {
            try
            {
                linkCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                linkModifiedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                //Status
                var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Master" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbStatus.DataSource = pStatus;
                    cmbStatus.ValueMember = "ID";
                    cmbStatus.DisplayMember = "Descr";
                }
                BindFYear();
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
                if (txtcityname.Text == string.Empty)
                {
                    MessageBox.Show("City Name should Not be Empty", "City Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtcityname.Focus();
                    return;
                }
                else if (cmbStatus.Text == string.Empty)
                {
                    MessageBox.Show("Please Select State..", "City Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbStatus.Focus();
                    return;
                }             
                else
                {
                    Save();
                    clear1();
                    BindFYear();// // // After Saving The City Details Bind City Grid
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "City Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            clear1();
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
        public void Save()
        {
            try
            {
                //if ((from u in db.Financial_Year_Masters where u.id == (Convert.ToInt32(txtCityId.Text)) select u).Count() > 0)
                //{
                    if (txtCityId.Text!="")
                    {

                        var c = db.Financial_Year_Masters.Where(w => w.id == (Convert.ToInt32(txtCityId.Text))).FirstOrDefault();
                        {
                        //c.ID = Convert.ToInt32(txtCityId.Text.ToString());
                        c.F_Year = (txtcityname.Text == "") ? "" : (txtcityname.Text);
                        c.Start_Date = dateTimePicker1.Value;
                        c.End_Date = dateTimePicker2.Value;
                        c.FY_ShortCode = (txtYearCode.Text == "") ? "" : (txtYearCode.Text);
                        c.Uses_AsSufix = checkBox1.Checked;
                        c.Created_By = linkCreatedBy.Text;
                        c.Modified_BY = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                        c.Status = Convert.ToInt32(cmbStatus.SelectedValue);
                        c.Company_ID = logIn.company;
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
                    Financial_Year_Master ci = new Financial_Year_Master();
                    //string company = Creation_Company;
                    //var result = db.Sp_autoincrement_CityMaster(company);
                    //ci.City_Id = result.FirstOrDefault().City_Id;
                    ci.F_Year = (txtcityname.Text == "") ? "" : (txtcityname.Text);
                    ci.Start_Date = dateTimePicker1.Value;
                    ci.End_Date = dateTimePicker2.Value;
                    ci.FY_ShortCode = (txtYearCode.Text == "") ? "" : (txtYearCode.Text);
                    ci.Uses_AsSufix = checkBox1.Checked;
                    ci.Created_By = linkCreatedBy.Text;
                    ci.Modified_BY = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                    ci.Status = Convert.ToInt32(cmbStatus.SelectedValue);
                    ci.Company_ID = logIn.company;
                    db.Financial_Year_Masters.InsertOnSubmit(ci);
                    db.SubmitChanges();
                    MessageBox.Show("Record Saved Successfully");
                    BindFYear();
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

        
        public void BindFYear()
        {
            try
            {
                var p = (from s in db.Financial_Year_Masters
                         where s.Company_ID == logIn.company


                         select new
                         {
                             s.id,
                             s.F_Year,
                             s.Start_Date,
                             s.End_Date,
                             s.Status
                          
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

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

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
        public void bindmethod(int Id)
        {
            try
            {
                var p = (from s in db.Financial_Year_Masters
                         where s.id == Id
                         select new
                         {
                             s.id,
                             s.F_Year,
                             s.Start_Date,
                             s.End_Date,
                             s.FY_ShortCode,
                             s.Uses_AsSufix,
                             s.Status

                         }
                        ).ToList();
                if (p.Count > 0)
                {

                    //txtCityId.Text = p[0].id;
                    txtcityname.Text = p[0].F_Year;
                    cmbStatus.SelectedValue = p[0].Status;
                    txtYearCode.Text = p[0].FY_ShortCode;
                    dateTimePicker1.Text = p[0].Start_Date.ToString();
                    dateTimePicker2.Text = p[0].End_Date.ToString();
                    if (p[0].Uses_AsSufix != null)
                    {
                        checkBox1.Checked = p[0].Uses_AsSufix.Value;
                    }
                }
                else
                {

                }

            }
            catch (Exception ex)
            {

            }
        }
    }
}
