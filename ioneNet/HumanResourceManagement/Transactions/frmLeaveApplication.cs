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
using iTextSharp.text;

namespace ioneNet.HumanResourceManagement.Transactions
{
    public partial class frmLeaveApplication : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmLeaveApplication()
        {
            InitializeComponent();
        }

        private void frmLeaveApplication_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

            //Bind Leave Types
            var bindTypes = (from m in db.HR_Leaves_Masters
                             where m.Company_ID == logIn.company
                             select new
                             {
                                 m.Short_Name,
                                 m.id,
                             }).ToList();

            if (bindTypes.Count > 0)
            {
                cmbLeaveTypes.DataSource = bindTypes;
                cmbLeaveTypes.DisplayMember = "Short_Name";
                cmbLeaveTypes.ValueMember = "id";
                cmbLeaveTypes.SelectedIndex = -1;

            }

            //Bind Employees
            var bindEmp = (from m in db.HR_Employee_Master_Datas
                             where m.Company_ID == logIn.company && m.is_Resigned ==false && m.BU_ID  == logIn.BU_ID
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

            //Bind Status
            var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Trans" select new { m.ID, m.Descr }).Distinct().ToList();
            if (pStatus.Count > 0)
            {
                cmbStatus.DataSource = pStatus;
                cmbStatus.ValueMember = "ID";
                cmbStatus.DisplayMember = "Descr";
            }

        }

        private void txtEmpName_Enter(object sender, EventArgs e)
        {
            try
            {

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void AddEmployee(AutoCompleteStringCollection coll)
        {
            try
            {

                var Prodname = (from d in db.HR_Employee_Master_Datas where d.Company_ID == logIn.company && d.BU_ID == logIn.BU_ID


                                select new { d.Emp_Name }).ToList();
                DataTable dt = new DataTable();
                dt.Columns.Add("Emp_Name");
                foreach (var item in Prodname)
                {
                    dt.Rows.Add(item.Emp_Name);
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
               

        private void cmbEmpName_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (cmbEmpName.Text != "")
            //{
            //    txtEmpID.Text = cmbEmpName.SelectedValue.ToString();

            //    var p = (from s in db.HR_Leave_Applications
            //             where s.Emp_Master_ID == Convert.ToInt32(txtEmpID.Text) && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID


            //             select new
            //             {
            //                 s.Leave_Applied_For,
            //                 s.Leave_From,
            //                 s.Leave_To,
            //                 s.Leave_Balance,
            //                 s.Leave_Days
                             
            //                 // Main_Group=s.MainGroup


            //             }
            //            );
            //    SqlCommand cmd1 = (SqlCommand)db.GetCommand(p);
            //    SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            //    DataTable dt1 = new DataTable();
            //    da1.Fill(dt1);

            //    if (dt1.Rows.Count >= 0)
            //    {
            //        grdPrevLeaves.DataSource = dt1;
            //    }
            //    else
            //    {
            //    }



            //}
        }

        private void cmbLeaveTypes_Leave(object sender, EventArgs e)
        {
            try
            {


                if (cmbLeaveTypes.Text != "")
                {
                    DateTime dt = dtAppDate.Value;
                    //string dt1 = dt.ToString("dd/MM/yyyy");
                    string eyear = dt.ToString("yyyy");
                    if (checkBox1.Checked == false)
                    {
                        var leaveData = (from d in db.SP_HR_Get_Employee_Leave_Balance(logIn.company, Convert.ToInt32(cmbEmpName.SelectedValue), cmbLeaveTypes.Text, logIn.BU_ID, eyear) select new { d.Leave_Balance }).ToList();
                        if (leaveData.Count > 0)
                        {
                            if (leaveData != null)
                            {
                                // R1.Cells["Job_card_date"].Value = getDetails.J_Date.ToString();
                                txtLeaveBalance.Text = leaveData[0].Leave_Balance.ToString();
                                //txtLeaveNoofDays.Enabled = false;
                            }
                        }
                        else
                        {
                            txtLeaveBalance.Text = "0";
                            MessageBox.Show("Balance Not Available for Selected Leave");
                            return;
                        }
                    }
                    else
                    {
                        var leaveData = (from d in db.SP_HR_Get_Employee_Leave_Balance_forEncashment(logIn.company, Convert.ToInt32(cmbEmpName.SelectedValue), cmbLeaveTypes.Text, logIn.BU_ID, eyear) select new { d.Leave_Balance }).ToList();
                        if (leaveData.Count > 0)
                        {
                            if (leaveData != null)
                            {
                                // R1.Cells["Job_card_date"].Value = getDetails.J_Date.ToString();
                                txtLeaveBalance.Text = leaveData[0].Leave_Balance.ToString();
                                txtLeaveNoofDays.Enabled = true;
                            }
                        }
                        else
                        {
                            txtLeaveBalance.Text = "0";
                            MessageBox.Show("Balance Not Available for Selected Leave");
                            return;
                        }
                    }

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

        private void dtLeaveTo_Leave(object sender, EventArgs e)
        {
            DateTime EndDate = dtLeaveTo.Value;
            DateTime StartDate = dtLeaveFrom.Value;
            Decimal lbal = Convert.ToDecimal(txtLeaveBalance.Text);
            if(lbal>= Convert.ToDecimal((EndDate - StartDate).TotalDays))
            {
                decimal noofdays = Convert.ToDecimal((EndDate - StartDate).TotalDays.ToString("0.00"));
                txtLeaveNoofDays.Text = (noofdays+1).ToString();
            }
            else
            {
                MessageBox.Show("Sufficiant Leave Balance Not Available,Cannot Proceed");
                dtLeaveTo.Focus();
                return;
            }

            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {


            try
            {
                if (cmbEmpName.Text == string.Empty)
                {
                    MessageBox.Show("Employee Namer Should Not Be Empty", "Leave Application", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbEmpName.Focus();
                    return;
                }
                if (txtLeaveNoofDays.Text == string.Empty)
                {
                    MessageBox.Show("No of Days Leave Applied Cannot be Blank", "Leave Application", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    return;
                }
                else               
                {
                    Save();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Leave Application", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void Save()
        {
            try
            {
                String myString = "";
               
                myString = txtAppNo.Text;
                if(myString!="")
                //if ((from u in db.HR_Leave_Applications where u.id == Convert.ToInt32(myString) && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    var p1 = db.HR_Leave_Applications.Where(w => w.id == Convert.ToInt32(myString) && w.Company_ID == logIn.company).FirstOrDefault();
                    p1.Application_Date = dtAppDate.Value;
                    p1.Employe_Name = cmbEmpName.Text;
                    p1.Emp_Master_ID = Convert.ToInt32(txtEmpID.Text);
                    p1.Leave_Applied_For = cmbLeaveTypes.Text;
                    p1.Leave_Balance = Convert.ToDecimal(txtLeaveBalance.Text);
                    p1.Leave_From = dtLeaveFrom.Value;
                    p1.Leave_To = dtLeaveTo.Value;
                    p1.Leave_Days = Convert.ToDecimal(txtLeaveNoofDays.Text);
                    p1.Reason_for_Leave = (txtReasonforLeave.Text == "") ? "" : txtReasonforLeave.Text;
                    // S.PODate = dpPODate.Value;

                    //S.Frieght_Amount = (txtFreight_Amt.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtFreight_Amt.Text);
                    p1.BU_ID = logIn.BU_ID;
                    p1.Status = Convert.ToInt32(cmbStatus.SelectedValue);
                    p1.Company_ID = logIn.company;                   
                    p1.Modified_BY = logIn.username + "-" + DateTime.Now;
                    p1.Leave_Encashment = checkBox1.Checked;
                    db.SubmitChanges();
                }
                else
                {

                    HR_Leave_Application S = new HR_Leave_Application();
                    {
                        S.Application_Date = dtAppDate.Value;
                        S.Employe_Name = cmbEmpName.Text;
                        S.Emp_Master_ID =Convert.ToInt32(txtEmpID.Text);
                        S.Leave_Applied_For = cmbLeaveTypes.Text;
                        S.Leave_Balance = Convert.ToDecimal(txtLeaveBalance.Text);
                        S.Leave_From = dtLeaveFrom.Value;
                        S.Leave_To = dtLeaveTo.Value;
                        S.Leave_Days = Convert.ToDecimal(txtLeaveNoofDays.Text);                       
                        S.Reason_for_Leave = (txtReasonforLeave.Text == "") ? "" : txtReasonforLeave.Text;
                        // S.PODate = dpPODate.Value;
                        S.Status = Convert.ToInt32(cmbStatus.SelectedValue);
                        //S.Frieght_Amount = (txtFreight_Amt.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtFreight_Amt.Text);
                        S.BU_ID = logIn.BU_ID;                      
                        S.Company_ID = logIn.company;
                        S.Created_By = lblCreatedBy.Text;
                        S.Modified_BY = logIn.username + "-" + DateTime.Now;
                        S.Leave_Encashment = checkBox1.Checked;
                        db.HR_Leave_Applications.InsertOnSubmit(S);
                        db.SubmitChanges();
                    }
                }
               MessageBox.Show("Record Saved / Updated Successfully");
                clear();
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

        private void cmbEmpName_Leave(object sender, EventArgs e)
        {
            if (cmbEmpName.Text != "")
            {

                var d1 = (from a in db.HR_Employee_Master_Datas where a.Emp_Name == cmbEmpName.Text && a.Company_ID == logIn.company select new { a.id }).ToList();
                //    SC.PR_Master_ID = d1[0].ID;
               
                 txtEmpID.Text = d1[0].id.ToString();
           
                


                



                var p = (from s in db.HR_Leave_Applications
                         where s.Emp_Master_ID == Convert.ToInt32(txtEmpID.Text) && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID


                         select new
                         {
                             s.id,
                             s.Leave_Applied_For,
                             s.Leave_From,
                             s.Leave_To,
                             s.Leave_Balance,
                             s.Leave_Days,
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
                    grdPrevLeaves.DataSource = dt1;
                }
                else
                {
                }

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
                                               
                        SqlCommand cmd1 = new SqlCommand("delete  from [HR_Leave_Application] where id = @ProdID and Company_ID = @compid and BU_ID = @buid", con);
                        cmd1.Parameters.AddWithValue("@ProdID", i);
                        cmd1.Parameters.AddWithValue("@compid", logIn.company);
                        cmd1.Parameters.AddWithValue("@buid", logIn.BU_ID);
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        //con.Open();
                        cmd1.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Record Deleted Successfully");
                        clear();
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

        private void grdPrevLeaves_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void grdPrevLeaves_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string ProductCode = grdPrevLeaves.Rows[grdPrevLeaves.CurrentRow.Index].Cells["ID"].Value.ToString();
                var ProductMasterList = (from prdmstr in db.HR_Leave_Applications where prdmstr.id == Convert.ToInt32(ProductCode) select prdmstr).ToList();
                if (ProductMasterList.Count > 0)
                {
                    txtAppNo.Text = ProductCode;
                    txtEmpID.Text = ProductMasterList[0].Emp_Master_ID.ToString();
                    cmbEmpName.Text = (ProductMasterList[0].Employe_Name);
                    cmbLeaveTypes.Text = ProductMasterList[0].Leave_Applied_For;
                    txtLeaveBalance.Text = (ProductMasterList[0].Leave_Balance.ToString());
                    dtAppDate.Text = (ProductMasterList[0].Application_Date.ToString());
                    dtLeaveFrom.Text = (ProductMasterList[0].Leave_From.ToString());
                    dtLeaveTo.Text = (ProductMasterList[0].Leave_To.ToString());
                    txtLeaveNoofDays.Text = (ProductMasterList[0].Leave_Days.ToString());
                    txtReasonforLeave.Text = ProductMasterList[0].Reason_for_Leave;
                    cmbStatus.SelectedValue = ProductMasterList[0].Status;
                    lblCreatedBy.Text = ProductMasterList[0].Created_By;
                    lblModified.Text = ProductMasterList[0].Modified_BY;
                    if (ProductMasterList[0].Leave_Encashment == true)
                    {
                        checkBox1.Checked = true;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
           

        }
        public void clear()
        {
            txtAppNo.Text = "";
            txtEmpID.Text = "";
            cmbEmpName.Text = "";
            cmbLeaveTypes.Text = "";
            txtLeaveBalance.Text = "";
            txtLeaveNoofDays.Text = "";
            txtReasonforLeave.Text = "";
            cmbStatus.SelectedValue = "";
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
        }

        private void txtLeaveNoofDays_Leave(object sender, EventArgs e)
        {
            Decimal lbal = Convert.ToDecimal(txtLeaveBalance.Text);
            if (lbal >= Convert.ToDecimal(txtLeaveNoofDays.Text))
            {
                
               //txtLeaveNoofDays.Text = (noofdays + 1).ToString();
            }
            else
            {
                MessageBox.Show("Sufficiant Leave Balance Not Available To Encash,Cannot Proceed");
                dtLeaveTo.Focus();
                return;
            }
        }
    }
}
