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
namespace ioneNet.ProductionManagement.Transactions
{
    public partial class frmMO_Create : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmMO_Create()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
             
                if (cmbProject.Text == string.Empty)
                {
                    MessageBox.Show("Project Code should Not be Empty", "MO Creation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbProject.Focus();
                    return;

                }

                else if (cmbCustomer.Text == "")
                {
                    MessageBox.Show("Select Custmer Name", "MO Creation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbCustomer.Focus();
                    return;

                }
                else if (cmbStatus.Text == "")
                {
                    MessageBox.Show("Select Status", "MO Creation", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbCustomer.Focus();
                    return;

                }
                else
                {
                    Save();
                    //clear();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void Save()
        {
            con.Open();
            SqlTransaction tran = con.BeginTransaction();
            try
            {

                if ((from u in db.Engg_Mfg_Orders where u.MO_No == txtMONo.Text && u.Company_ID == logIn.company select u).Count() > 0)
                
                {
                    //if (AppCode.GlobalAccess.Add == "Yes")
                    //{
                    var c = db.Engg_Mfg_Orders.Where(w => w.MO_No == txtMONo.Text).FirstOrDefault();
                    {

                        // c.Company_ID = txtCompanyId.Text;
                        c.MO_Date = dtMODate.Value;
                        c.Project_Code = (cmbProject.Text == "") ? "" : (cmbProject.Text);
                        c.Project_ID = Convert.ToInt32(cmbProject.SelectedValue);                      
                        c.SO_No = (txtSONo.Text == "") ? "" : (txtSONo.Text);
                        c.Customer_Name = Convert.ToInt32(cmbCustomer.SelectedValue);
                        c.MO_Qty = (txtQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtQty.Text);
                        c.BU_ID = logIn.BU_ID;
                        c.Status = Convert.ToInt32(cmbStatus.SelectedValue);
                        c.Company_ID = logIn.company;
                        c.Created_By = lblCreatedBy.Text;
                        c.Modified_BY = logIn.username + "-" + DateTime.Now;
                        db.SubmitChanges();
                        tran.Commit();
                        MessageBox.Show("Record Updated Successfully");
                        con.Close();
                        this.Close();

                    }
                    //}
                    //else
                    //{
                    //    MessageBox.Show("You dont Have Privileges", "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //    clear();
                    //}


                }
                else
                {

                    Engg_Mfg_Order ci = new Engg_Mfg_Order();
                    //ci.Company_ID = txtCompanyId.Text;
                    ci.MO_No = (txtMONo.Text == "") ? "" : (txtMONo.Text);
                    ci.MO_Date = dtMODate.Value;
                    ci.Project_Code = (cmbProject.Text == "") ? "" : (cmbProject.Text);
                    ci.Project_ID = Convert.ToInt32(cmbProject.SelectedValue);
                    ci.SO_No = (txtSONo.Text == "") ? "" : (txtSONo.Text);
                    ci.Customer_Name = Convert.ToInt32(cmbCustomer.SelectedValue);
                    ci.MO_Qty = (txtQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtQty.Text);
                    ci.BU_ID = logIn.BU_ID;
                    ci.Company_ID = logIn.company;
                    ci.Created_By = lblCreatedBy.Text;
                    ci.Modified_BY = lblModified.Text;
                    ci.Status = Convert.ToInt32(cmbStatus.SelectedValue);
                    db.Engg_Mfg_Orders.InsertOnSubmit(ci);
                    db.SubmitChanges();
                    tran.Commit();
                    MessageBox.Show("Record Saved Successfully");
                    con.Close();
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                try
                {
                    tran.Rollback();
                }
                catch (Exception exRollBack)
                {
                    //MessageBox.Show(ex.Message);
                    Console.WriteLine(exRollBack.Message);
                }
                MessageBox.Show(ex.Message);
            }
        }

        private void frmMO_Create_Load(object sender, EventArgs e)
        {
            try
            {

                lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");


                var BuyerBind = (from m in db.Supplier_informations where m.Company_ID == logIn.company select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                if (BuyerBind.Count > 0)
                {
                    cmbCustomer.DataSource = BuyerBind;
                    cmbCustomer.ValueMember = "ID";
                    cmbCustomer.DisplayMember = "Supplier_Name";
                    cmbCustomer.SelectedIndex = -1;
                }

                var ProjectBind = (from m in db.Project_code_Masters where m.Company_ID == logIn.company select new { m.id, m.Project_Code }).Distinct().ToList();
                if (ProjectBind.Count > 0)
                {
                    cmbProject.DataSource = ProjectBind;
                    cmbProject.ValueMember = "id";
                    cmbProject.DisplayMember = "Project_Code";
                    cmbProject.SelectedIndex = -1;
                }

                var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Trans" && (m.Company_ID == logIn.company || m.Company_ID == 0) select new { m.ID, m.Descr }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbStatus.DataSource = pStatus;
                    cmbStatus.ValueMember = "ID";
                    cmbStatus.DisplayMember = "Descr";
                    cmbStatus.SelectedIndex = -1;
                }

                if (ProductionManagement.Transactions.frmMOList.var == "0")
                {
                    if (ProductionManagement.Transactions.frmMOList.editMode == true)
                    {
                        bindedit();
                        ProductionManagement.Transactions.frmMOList.var = null;
                    }
                }
                else
                {
                    AutoincrementId();
                }


               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void AutoincrementId()
        {
            try
            {

                var result = db.Sp_autoincrement_MO(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date);
                txtMONo.Text = result.FirstOrDefault().Mo_no;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void bindedit()
        {
            try
            {
                txtMONo.Text = ProductionManagement.Transactions.frmMOList.MO_No;
                //int myString;
                //myString = txtID.Text;
                var da = (from obj in db.Engg_Mfg_Orders
                          where obj.MO_No == txtMONo.Text && obj.Company_ID == logIn.company
                          select obj).ToList();

                if (da.Count > 0)
                {

                    cmbCustomer.SelectedValue = da[0].Customer_Name;
                    cmbProject.SelectedValue = da[0].Project_ID;
                    txtSONo.Text = da[0].SO_No;
                    dtMODate.Text = da[0].MO_Date.ToString();
                    txtQty.Text = da[0].MO_Qty.ToString();
                    cmbStatus.SelectedValue = da[0].Status;
                    //cmbCustomer.Enabled = false;
                  
                    lblCreatedBy.Text = da[0].Created_By;
                    lblModified.Text = da[0].Modified_BY;
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            txtMONo.Text = "";
            cmbProject.SelectedIndex = -1;
            txtSONo.Text = "";
            cmbCustomer.SelectedIndex = -1;
            txtQty.Text = "";
            cmbStatus.SelectedIndex = -1;
            AutoincrementId();


        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void dtTargetDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cmbProject_Leave(object sender, EventArgs e)
        {
            if (cmbProject.Text != "")
            {
                var da = (from obj in db.Project_code_Masters
                          where obj.Project_Code == cmbProject.Text && obj.Company_ID == logIn.company
                          select obj).ToList();

                if (da.Count > 0)
                {

                    cmbCustomer.Text = da[0].Client_Name;
                }
            }
        }
    }
}
