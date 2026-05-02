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
using System.Text.RegularExpressions;
using System.IO;
using System.Data.OleDb;
using System.Configuration;
using Ione_DAL;


namespace ioneNet.HumanResourceManagement.Transactions
{
    public partial class frmLoanEntry : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmLoanEntry()
        {
            InitializeComponent();
        }

        private void frmLoanEntry_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

            //Bind Employees
            var bindEmp = (from m in db.HR_Employee_Master_Datas
                           where m.Company_ID == logIn.company && m.BU_ID == logIn.BU_ID
                           select new
                           {
                               m.Emp_Name,
                               m.id,
                           }).ToList();

            if (bindEmp.Count > 0)
            {
                cmbEmpName.DataSource = bindEmp;
                cmbEmpName.DisplayMember = "Emp_Name";
                cmbEmpName.ValueMember = "id";
                cmbEmpName.SelectedIndex = -1;

            }
        }

        private void cmbEmpName_Leave(object sender, EventArgs e)
        {
            if (cmbEmpName.Text != "")
            {
                var emp = (from c in db.HR_Employee_Master_Datas
                           where c.id == Convert.ToInt32(cmbEmpName.SelectedValue)
                           select new { c.AadharNo,c.id }).ToList();
                if (emp.Count > 0)
                {
                    txtEmpID.Text = emp[0].id.ToString();
                    textBox1.Text = emp[0].AadharNo;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {


            try
            {
                if (cmbEmpName.Text == string.Empty)
                {
                    MessageBox.Show("Employee Namer Should Not Be Empty", "Loan Entry", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbEmpName.Focus();
                    return;
                }
                if (txtLeaveNoofDays.Text == string.Empty)
                {
                    MessageBox.Show("Loan Amount Cannot Be Blank", "Loan Entry", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    return;
                }
                else
                {
                    Save();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void Save()
        {
            try
            {
                String myString = "";

                myString = txtAppNo.Text;
                if (myString != "")
                //if ((from u in db.HR_Leave_Applications where u.id == Convert.ToInt32(myString) && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    var p1 = db.HR_Loan_Entries.Where(w => w.id == Convert.ToInt32(myString) && w.Company_ID == logIn.company).FirstOrDefault();
                    p1.Application_Date = dtAppDate.Value;
                    p1.Employe_Name = cmbEmpName.Text;
                    p1.Emp_Master_ID = Convert.ToInt32(txtEmpID.Text);
                    p1.Loan_Type = cmbLoanType.Text;
                    //p1.lo = Convert.ToDecimal(txtLeaveBalance.Text);
                    p1.Loan_Date = dtLeaveFrom.Value;
                    p1.Deduct_From = dtLeaveTo.Value;
                    p1.Loan_Amount = Convert.ToDecimal(txtLeaveNoofDays.Text);
                    p1.Deduct_Per_Month = Convert.ToDecimal(txtDeductPerMonth.Text);
                    p1.Remarks = (txtReasonforLeave.Text == "") ? "" : txtReasonforLeave.Text;
                    // S.PODate = dpPODate.Value;

                    //S.Frieght_Amount = (txtFreight_Amt.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtFreight_Amt.Text);
                    p1.BU_ID = logIn.BU_ID;
                    p1.Company_ID = logIn.company;
                    p1.Modified_BY = logIn.username + "-" + DateTime.Now;
                    db.SubmitChanges();
                }
                else
                {

                    HR_Loan_Entry S = new HR_Loan_Entry();
                    {
                        S.Application_Date = dtAppDate.Value;
                        S.Employe_Name = cmbEmpName.Text;
                        S.Emp_Master_ID = Convert.ToInt32(txtEmpID.Text);
                        S.Loan_Type = cmbLoanType.Text;
                        //p1.lo = Convert.ToDecimal(txtLeaveBalance.Text);
                        S.Loan_Date = dtLeaveFrom.Value;
                        S.Deduct_From = dtLeaveTo.Value;
                        S.Loan_Amount = Convert.ToDecimal(txtLeaveNoofDays.Text);
                        S.Deduct_Per_Month = Convert.ToDecimal(txtDeductPerMonth.Text);
                        S.Remarks = (txtReasonforLeave.Text == "") ? "" : txtReasonforLeave.Text;

                        //S.Frieght_Amount = (txtFreight_Amt.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtFreight_Amt.Text);
                        S.BU_ID = logIn.BU_ID;
                        S.Company_ID = logIn.company;
                        S.Created_By = lblCreatedBy.Text;
                        S.Modified_BY = logIn.username + "-" + DateTime.Now;
                        db.HR_Loan_Entries.InsertOnSubmit(S);
                        db.SubmitChanges();
                    }
                }
                MessageBox.Show("Record Saved / Updated Successfully");
                //this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }
        public void bindList()
        {
            try
            {
                var p = (from s in db.HR_Loan_Entries
                         where s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID


                         select new
                         {
                             ID = s.id,
                             s.Application_Date,
                             s.Employe_Name,
                             s.Loan_Amount
                             // Main_Group=s.MainGroup


                         }
                        );
                SqlCommand cmd1 = (SqlCommand)db.GetCommand(p);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);

                if (dt1.Rows.Count >= 0)
                {
                    dataGridView1.DataSource = dt1;
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

        private void button1_Click(object sender, EventArgs e)
        {
            bindList();
            groupBox1.Visible = true;

        }

        private void button2_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                txtAppNo.Text = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["ID"].Value.ToString();

                var d = (from po in db.HR_Loan_Entries
                             // join s in db.Sales_Men_Informations on po.Salesmen_Code equals s.Salesmen_Code
                         where
                         po.id == Convert.ToInt32(txtAppNo.Text) //&& po.Creation_Company == Creation_Company
                         select new
                         {
                             po.id,
                             po.Application_Date                            
                            ,po.Emp_Master_ID
                            ,po.Employe_Name
                            ,po.Employe_Aadhar
                            ,po.Loan_Date
                            ,po.Loan_Type
                            ,po.Loan_Amount
                            ,po.Leave_Balance
                            ,po.Deduct_Per_Month
                            ,po.Deduct_From
                            ,po.Remarks
                             ,po.Created_By
                             ,po.Modified_BY

                         }).ToList();
                if (d.Count > 0)
                {

                    txtAppNo.Text = d[0].id.ToString();
                    dtAppDate.Text = d[0].Application_Date.ToString();
                    cmbEmpName.SelectedValue = d[0].Emp_Master_ID;
                    txtEmpID.Text = d[0].Emp_Master_ID.ToString();
                    txtLeaveNoofDays.Text = d[0].Loan_Amount.ToString();
                    dtLeaveFrom.Text = d[0].Loan_Date.ToString();
                    txtDeductPerMonth.Text = d[0].Deduct_Per_Month.ToString();
                    textBox1.Text = d[0].Employe_Aadhar;
                    dtLeaveTo.Text = d[0].Deduct_From.ToString();
                    // txts.Text = d[0].MainGroup;

                    // txtGroupPrefix.Text = d[0].Group_PreFix;
                    lblCreatedBy.Text = d[0].Created_By;
                    lblModified.Text = d[0].Modified_BY;
                }
                groupBox1.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int i = Convert.ToInt32(txtAppNo.Text);
                if (i >= 0)
                {
                    DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {

                        SqlCommand cmd1 = new SqlCommand("delete  from [HR_Loan_Entry] where id = @ProdID and Company_ID = @compid and BU_ID = @buid", con);
                        cmd1.Parameters.AddWithValue("@ProdID", i);
                        cmd1.Parameters.AddWithValue("@compid", logIn.company);
                        cmd1.Parameters.AddWithValue("@buid", logIn.BU_ID);
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        //con.Open();
                        cmd1.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Record Deleted Successfully");
                       
                    }

                }
                else
                {
                    MessageBox.Show("Please Select Atleast One Product to Delete");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Leave Application", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }
    }
}
