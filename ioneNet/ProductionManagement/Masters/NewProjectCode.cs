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
namespace ioneNet.ProductionManagement.Masters
{
    public partial class NewProjectCode : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public NewProjectCode()
        {
            InitializeComponent();
        }
        
        private void NewProjectCode_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

            bindCustomer();
            if (ProjectCodesList.editMode == true)
            {
                bindedit();
            }
            else
            {
                clear();
            }
        }

        public void bindCustomer()
        {
            try
            {
                var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    //cmbContractor.DataSource = Buyerblind;
                    //cmbContractor.ValueMember = "ID";
                    //cmbContractor.DisplayMember = "Supplier_Name";

                    //sfComboBox1.DataSource = Buyerblind;
                    //sfComboBox1.ValueMember = "ID";
                    //sfComboBox1.DisplayMember = "Supplier_Name";

                    //comboBoxAutoComplete1.DataSource = Buyerblind;
                    //comboBoxAutoComplete1.ValueMember = "ID";
                    //comboBoxAutoComplete1.DisplayMember = "Supplier_Name";

                }
                //if (CmbBuyerName.Items.Count > 0)
                //    CmbBuyerName.SelectedIndex = -1;

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
                //if (txtCompanyId.Text == string.Empty)
                //{
                //    MessageBox.Show("Company Id should Not be Empty", "Company Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    txtCompanyId.Focus();
                //    return;

                //}
                if (txtProjectCode.Text == string.Empty)
                {
                    MessageBox.Show("Project Code should Not be Empty", "Project Code", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txtProjectCode.Focus();
                    return;

                }

                //else if (cmbContractor.Text == "")
                //{
                //    MessageBox.Show("Select Contractor Name", "Project Code", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    cmbContractor.Focus();
                //    return;

                //}                
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


                if (txtID.Text != "")
                {
                    //if (AppCode.GlobalAccess.Add == "Yes")
                    //{
                    var c = db.Project_code_Masters.Where(w => w.id == Convert.ToInt32(txtID.Text)).FirstOrDefault();
                    {

                        // c.Company_ID = txtCompanyId.Text;
                        c.Project_Code = (txtProjectCode.Text == "") ? "" : (txtProjectCode.Text);
                        c.Project_Description = (txtProjectDescr.Text == "") ? "" : (txtProjectDescr.Text);
                        c.Project_Type = (txtProjectType.Text == "") ? "" : txtProjectType.Text;
                        c.Client_Name = (txtClient.Text == "") ? "" : txtClient.Text;                       
                        c.Contractor_name = Convert.ToInt32(txtClientCode.Text);
                        c.Company_ID = logIn.company;
                        c.Created_By = lblCreatedBy.Text;
                        c.Modified_By = logIn.username + "-" + DateTime.Now;
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

                    Project_code_Master ci = new Project_code_Master();
                    //ci.Company_ID = txtCompanyId.Text;
                    ci.Project_Code = (txtProjectCode.Text == "") ? "" : (txtProjectCode.Text);
                    ci.Project_Description = (txtProjectDescr.Text == "") ? "" : (txtProjectDescr.Text);
                    ci.Project_Type = (txtProjectType.Text == "") ? "" : txtProjectType.Text;
                    ci.Client_Name = (txtClient.Text == "") ? "" : txtClient.Text;
                    ci.Contractor_name = Convert.ToInt32(txtClientCode.Text);
                    ci.Company_ID = logIn.company;
                    ci.Created_By = logIn.username + "-" + DateTime.Now;
                    ci.Modified_By = logIn.username + "-" + DateTime.Now;

                    db.Project_code_Masters.InsertOnSubmit(ci);
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
        public void bindedit()
        {
            try
            {
                txtID.Text = ProjectCodesList.SO_No;
                //int myString;
                //myString = txtID.Text;
                var da = (from obj in db.Project_code_Masters
                          where obj.id == Convert.ToInt32(txtID.Text) && obj.Company_ID == logIn.company
                          select obj).ToList();

                if (da.Count > 0)
                {
                    
                    txtClientCode.Text = da[0].Contractor_name.ToString();                 
                    txtProjectCode.Text = da[0].Project_Code;
                    txtProjectDescr.Text = da[0].Project_Description;
                    txtProjectType.Text = da[0].Project_Type;
                   
                    //cmbCustomer.Enabled = false;
                    txtClient.Text = da[0].Client_Name;                    
                    lblCreatedBy.Text = da[0].Created_By;
                    lblModified.Text = da[0].Modified_By;
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

        private void txtClient_Enter(object sender, EventArgs e)
        {
            try
            {

                AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                AddJC(DataColl);
                txtClient.AutoCompleteCustomSource = DataColl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void AddJC(AutoCompleteStringCollection coll)
        {
            try
            {

                var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company 
                                  select new { m.ID, m.Supplier_Name }).Distinct().ToList();
               
                DataTable dt = new DataTable();
                dt.Columns.Add("Supplier_Name");
                foreach (var item in Buyerblind)
                {
                    dt.Rows.Add(item.Supplier_Name);
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    coll.Add(dt.Rows[i][0].ToString());
                }

            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void txtClient_Leave(object sender, EventArgs e)
        {
            if(txtClient.Text !="")
            {
                var sa = (from s in db.Supplier_informations
                          
                          where s.Supplier_Name == txtClient.Text && s.Company_ID == logIn.company
                          select new { s.ID }).ToList();
                //var sa = (from a in db.Products where a.prod_ID == Convert.ToInt32(cmbpname.SelectedValue) select new { a.Prod_Primary_UOM_Id, a.Prod_Group_Id,a.Prod_Unit_Wt }).ToList();
                if (sa.Count > 0)
                {

                    txtClientCode.Text = sa[0].ID.ToString();                   
                }
                else
                {
                    txtClientCode.Text = "0";
                }
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }
        public void clear()
        {
            txtID.Text = "";
            txtProjectCode.Text = "";
            txtProjectType.Text = "";
            txtProjectDescr.Text = "";
            txtClient.Text = "";
            txtClientCode.Text = "";
        }

        private void txtProjectCode_Leave(object sender, EventArgs e)
        {
            var da = (from obj in db.Project_code_Masters
                      where obj.Company_ID == logIn.company && obj.Project_Code==txtProjectCode.Text
                      select obj).ToList();

            if (da.Count > 0)
            {
                MessageBox.Show("Project Code Already Exist ");
                txtProjectCode.Text = "";
                txtProjectCode.Focus();
            }

        }
    }
}
