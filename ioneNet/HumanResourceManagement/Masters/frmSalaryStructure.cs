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
namespace ioneNet.HumanResourceManagement.Masters
{
    public partial class frmSalaryStructure : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmSalaryStructure()
        {
            InitializeComponent();
        }
        private void frmSalaryStructure_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            txtVersionNo.Text = "0";
            bindHeads();
            if(EmployeeList.productCode.ToString() != "")
            {
                txtEmpID.Text = EmployeeList.productCode.ToString();
                var Prodname = (from d in db.SP_HR_GetSalaryInfo(logIn.company, Convert.ToInt32(txtEmpID.Text),logIn.BU_ID)
                                                               
                                select new { d.Emp_Name, d.emp_code, d.Group_Name, d.GrossSal,d.BasicSal,d.Basic_Per,d.Sal_Rev_No,d.Sal_Rev_Date }).ToList();

                if (Prodname.Count > 0)
                {
                    txtEmpName.Text = Prodname[0].Emp_Name;
                    txtEmpCode.Text = Prodname[0].emp_code.ToString();
                    txtEmpGroup.Text = Prodname[0].Group_Name.ToString();
                    txtGrossSalary.Text = Prodname[0].GrossSal.ToString();
                    txtBasicPer.Text = Prodname[0].Basic_Per.ToString();
                    txtBasicAmount.Text = Prodname[0].BasicSal.ToString();
                    txtVersionNo.Text = (Prodname[0].Sal_Rev_No.ToString() == "") ? "0" : (Prodname[0].Sal_Rev_No.ToString());
                    dateTimePicker1.Text = Prodname[0].Sal_Rev_Date.ToString();
                    txtTotalSalary.Text = Prodname[0].GrossSal.ToString();
                }

                //Get Salary Heads wise info

                //var getHeads = (from d in db.SP_HR_GetSalaryInfo_HeadWise(logIn.company, Convert.ToInt32(txtEmpID.Text), logIn.BU_ID,txtEmpGroup.Text, Convert.ToInt32(txtVersionNo.Text))

                //                select new { d.id, Head_Name=d.Head, d.Amount }).ToList();

                //if (getHeads.Count > 0)
                //{
                //    grdHeads.DataSource = getHeads;
                //}
            }
        }
        private void txtEmpName_Enter(object sender, EventArgs e)
        {
            try
            {
                txtEmpName.AutoCompleteCustomSource = null;
               
                AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                AddEmployee(DataColl);
                txtEmpName.AutoCompleteCustomSource = DataColl;
                

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

                var Prodname = (from d in db.HR_Employee_Master_Datas
                                where d.Company_ID == logIn.company

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

        private void txtEmpName_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtEmpName.Text != "")
                {
                    var Prodname = (from d in db.HR_Employee_Master_Datas

                                    where d.Emp_Name == txtEmpName.Text && d.Company_ID == logIn.company
                                    select new { d.Emp_Code, d.id, d.Emp_Group, d.GrossSal }).ToList();

                    if (Prodname.Count > 0)
                    {
                        txtEmpCode.Text = Prodname[0].Emp_Code;
                        txtEmpID.Text = Prodname[0].id.ToString();
                        //var salData = (from d in db.SP_HR_GetSalaryInfo(logIn.company, Convert.ToInt32(txtEmpID.Text), logIn.BU_ID)

                        //               select new { d.Emp_Name, d.emp_code, d.Group_Name, d.GrossSal, d.BasicSal, d.Basic_Per,d.Sal_Rev_Date,d.Sal_Rev_No }).ToList();

                        //if (salData.Count > 0)
                        //{
                        //    txtEmpName.Text = salData[0].Emp_Name;
                        //    txtEmpCode.Text = salData[0].emp_code.ToString();
                        //    txtEmpGroup.Text = salData[0].Group_Name.ToString();
                        //    txtGrossSalary.Text = salData[0].GrossSal.ToString();
                        //    txtBasicPer.Text = salData[0].Basic_Per.ToString();
                        //    txtBasicAmount.Text = salData[0].BasicSal.ToString();
                        //    txtVersionNo.Text = salData[0].Sal_Rev_No.ToString();
                        //    dateTimePicker1.Text = salData[0].Sal_Rev_Date.ToString();
                        //    txtTotalSalary.Text = salData[0].GrossSal.ToString();
                        //}

                        //Get Salary Heads wise info

                        //var getHeads = (from d in db.SP_HR_GetSalaryInfo_HeadWise(logIn.company, Convert.ToInt32(txtEmpID.Text), logIn.BU_ID, txtEmpGroup.Text)

                        //                select new { d.id, d.Head, d.Amount }).ToList();

                        //if (getHeads.Count > 0)
                        //{
                        //    grdHeads.DataSource = getHeads;
                        //}
                    }
                    else
                    {
                        MessageBox.Show("Invalid Employee Name Entered");
                        txtEmpName.Focus();
                        return;
                    }


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
        public void bindHeads()
        {
            try
            {
                var p = (from s in db.HR_Salary_HeadsInfos
                         where s.Status == 1 && s.Company_ID == logIn.company
                         orderby s.DisplayOrder

                         select new
                         {
                             ID = s.id,
                             Head_Name = s.Short_Name,
                             


                         }
                        );
                SqlCommand cmd1 = (SqlCommand)db.GetCommand(p);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);

                if (dt1.Rows.Count >= 0)
                {
                    grdHeads.DataSource = dt1;
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

        private void txtEmpCode_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtEmpCode.Text != "")
                {
                    var Prodname = (from d in db.HR_Employee_Master_Datas

                                    where d.Emp_Code == txtEmpCode.Text && d.BU_ID == logIn.BU_ID
                                    select new { d.Emp_Name, d.id, d.Emp_Group, d.GrossSal }).ToList();

                    if (Prodname.Count > 0)
                    {
                        txtEmpName.Text = Prodname[0].Emp_Name;
                        txtEmpID.Text = Prodname[0].id.ToString();
                        //   var salData = (from d in db.SP_HR_GetSalaryInfo(logIn.company, Convert.ToInt32(txtEmpID.Text), logIn.BU_ID)

                        //                            select new { d.Emp_Name, d.emp_code, d.Group_Name, d.GrossSal, d.BasicSal, d.Basic_Per,d.Sal_Rev_No,d.Sal_Rev_Date }).ToList();

                        //            if (salData.Count > 0)
                        //            {
                        //                txtEmpName.Text = salData[0].Emp_Name;
                        //                txtEmpCode.Text = salData[0].emp_code.ToString();
                        //                txtEmpGroup.Text = salData[0].Group_Name.ToString();
                        //                txtGrossSalary.Text = salData[0].GrossSal.ToString();
                        //                txtBasicPer.Text = salData[0].Basic_Per.ToString();
                        //                txtBasicAmount.Text = salData[0].BasicSal.ToString();
                        //                txtVersionNo.Text = salData[0].Sal_Rev_No.ToString();
                        //                dateTimePicker1.Text = salData[0].Sal_Rev_Date.ToString();
                        //                txtTotalSalary.Text = salData[0].GrossSal.ToString();
                        //            }

                        //            //Get Salary Heads wise info

                        //            if (txtVersionNo.Text != "")
                        //            {
                        //                var getSaData = (from d in db.HR_EmpSalInfos
                        //                                 where d.Company_ID == logIn.company
                        //                         && d.Emp_Master_ID == Convert.ToInt32(txtEmpID.Text)
                        //                         && d.Sal_Rev_No == Convert.ToInt32(txtVersionNo.Text)

                        //                                 select new { id = d.Head_ID, Head_Name = d.Head, d.Amount });

                        //                SqlCommand cmd2 = (SqlCommand)db.GetCommand(getSaData);
                        //                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        //                DataTable dtr = new DataTable();
                        //                da2.Fill(dtr);
                        //                if (dtr.Rows.Count >= 0)
                        //                    grdHeads.DataSource = dtr;



                        //                //if (getSaData.Count > 0)
                        //                //{
                        //                //    grdHeads.DataSource = getSaData;
                        //                //}
                        //                else
                        //                {
                        //                    var getHeads = (from d in db.SP_HR_GetSalaryInfo_HeadWise(logIn.company, Convert.ToInt32(txtEmpID.Text), logIn.BU_ID, txtEmpGroup.Text, Convert.ToInt32(txtVersionNo.Text))

                        //                                    select new { d.id, d.Head, d.Amount }).ToList();

                        //                    if (getHeads.Count > 0)
                        //                    {
                        //                        grdHeads.DataSource = getHeads;
                        //                    }
                        //                }
                        //            }
                        //            else
                        //            {
                        //                txtVersionNo.Text = "1";
                        //                var getHeads = (from d in db.SP_HR_GetSalaryInfo_HeadWise(logIn.company, Convert.ToInt32(txtEmpID.Text), logIn.BU_ID, txtEmpGroup.Text, Convert.ToInt32(txtVersionNo.Text))

                        //                                select new { d.id, d.Head, d.Amount }).ToList();

                        //                if (getHeads.Count > 0)
                        //                {
                        //                    grdHeads.DataSource = getHeads;
                        //                }
                        //            }
                        //        }                                     
                        //        else
                        //        {
                        //            MessageBox.Show("Invalid Employee Code Entered");
                        //            txtEmpCode.Focus();
                        //        }
                    }

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
        DataTable dtexisting = new DataTable();
        private void txtBasicPer_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtBasicPer.Text != "")
                {
                    decimal GSal = Convert.ToDecimal(txtGrossSalary.Text);
                    decimal BPer = Convert.ToDecimal(txtBasicPer.Text);
                    decimal Bsal = Math.Round(GSal * BPer/100);
                    txtBasicAmount.Text = Bsal.ToString("0.00");
                    var Prodname = (from d in db.HR_Salary_HeadsInfos

                                    where  d.Company_ID == logIn.company orderby d.DisplayOrder
                                    select new { d.Short_Name, d.id, d.Calculation, d.basicper }).ToList();
                    dtexisting.Rows.Clear();
                    dtexisting.Columns.Clear();
                    dtexisting.Columns.Add("ID", typeof(string));
                    dtexisting.Columns.Add("Head_Name", typeof(string));
                    dtexisting.Columns.Add("Amount", typeof(string));
                    decimal BPer1 = 0;
                    for (int i = 0; i < Prodname.Count; i++)
                    {
                        
                        switch (Prodname[i].Calculation)
                        {
                            case "% of Gross":
                                DataRow dr;
                                dr = dtexisting.NewRow();
                                dr["ID"] = Prodname[i].id;
                                dr["Head_Name"] = Prodname[i].Short_Name;
                                BPer1 = Convert.ToDecimal(Prodname[i].basicper.ToString());
                                dr["Amount"] = Math.Round(((GSal * BPer1) / 100)).ToString("0.00");
                                dtexisting.Rows.Add(dr);
                                break;
                            case "% of Basic":
                                //DataRow dr;
                                dr = dtexisting.NewRow();
                                dr["ID"] = Prodname[i].id;
                                dr["Head_Name"] = Prodname[i].Short_Name;
                                BPer1 = Convert.ToDecimal(Prodname[i].basicper.ToString());
                                dr["Amount"] = Math.Round((Bsal * BPer1) / 100).ToString("0.00");
                                dtexisting.Rows.Add(dr);
                                break;
                            case "Fixed":
                                //DataRow dr;
                                dr = dtexisting.NewRow();
                                dr["ID"] = Prodname[i].id;
                                dr["Head_Name"] = Prodname[i].Short_Name;
                                //BPer1 = Convert.ToDecimal(Prodname[i].basicper.ToString());
                                dr["Amount"] = "0";
                                dtexisting.Rows.Add(dr);
                                break;
                            case "Balance of Gross":
                                //DataRow dr;
                                dr = dtexisting.NewRow();
                                dr["ID"] = Prodname[i].id;
                                dr["Head_Name"] = Prodname[i].Short_Name;
                                decimal TotSal =0;
                                for (int k = 0; k < dtexisting.Rows.Count - 1; k++)
                                {
                                    string sal1 = dtexisting.Rows[k].Field<string>("Amount").ToString();
                                    TotSal += Convert.ToDecimal(sal1);
                                 
                                }

                                //BPer1 = Convert.ToDecimal(Prodname[i].basicper.ToString());
                                dr["Amount"] = Math.Round(GSal - (Bsal+TotSal));
                                dtexisting.Rows.Add(dr);
                                break;

                        }
                       

                    }
                    dtexisting.AcceptChanges();
                    grdHeads.DataSource = dtexisting;

                    //Get Total
                    double totQty = 0;
                    decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
                    for (int i = 0; i < grdHeads.Rows.Count - 1; i++)
                    {
                        y += (grdHeads.Rows[i].Cells["Amount"].Value.ToString() == "" || grdHeads.Rows[i].Cells["Amount"].Value == null || grdHeads.Rows[i].Cells["Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(grdHeads.Rows[i].Cells["Amount"].Value);
                      
                    }

                    txtTotalSalary.Text = (Bsal+ y).ToString(".00");
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                

                String myString = "";
                int Icode = Convert.ToInt32(txtEmpID.Text);
               // myString = txtGRN_No.Text;
                if (Convert.ToDecimal(txtTotalSalary.Text) != Convert.ToDecimal(txtGrossSalary.Text))
                {
                    MessageBox.Show("Total Salary & Gross Salary Is Not Matching, Cannot Be Saved");
                    return;
                }
                else
                {
                    if ((from u in db.HR_EmpSalInfos where u.Emp_Master_ID == Icode && u.Company_ID == logIn.company && u.Sal_Rev_No == Convert.ToInt32(txtVersionNo.Text) select u).Count() > 0)
                    {
                        //Check wether user has right to modify
                        var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Employee Salary Info" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                        if (uRole.Count > 0)
                        {
                            if (uRole[0].Modify_Role == true)
                            {
                                db.sp_Delete_Salary_Structure(Icode, logIn.company, Convert.ToInt32(txtVersionNo.Text));

                            }
                            else
                            {
                                MessageBox.Show("You Have No Permissions to Modify The Salary Structure");
                                return;
                            }
                        }
                    }
                    else
                    {

                        // myString = txtSoNo.Text;

                    }
                    decimal y, x;
                    //y = Convert.ToDecimal(txtTotalQty.Text);
                    //db.Transaction = transaction;
                    for (int i = 0; i < grdHeads.RowCount-1 ; i++)
                    {
                        HR_EmpSalInfo SC = new HR_EmpSalInfo();

                        SC.Emp_Master_ID = Icode;
                        SC.Emp_Code = txtEmpCode.Text;
                        SC.Head_ID = (grdHeads.Rows[i].Cells["ID"].Value == DBNull.Value) ? Convert.ToInt32("0") : Convert.ToInt32(grdHeads.Rows[i].Cells["ID"].Value);
                        SC.Head = (grdHeads.Rows[i].Cells["Head_Name"].Value == null) ? "" : (grdHeads.Rows[i].Cells["Head_Name"].Value).ToString();
                        SC.Sal_Rev_No = Convert.ToInt32(txtVersionNo.Text);
                        SC.Sal_Rev_Date = dateTimePicker1.Value;
                        SC.Amount = (grdHeads.Rows[i].Cells["Amount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(grdHeads.Rows[i].Cells["Amount"].Value);
                        SC.GrossSal = (txtGrossSalary.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtGrossSalary.Text);
                        SC.BasicSal = (txtBasicAmount.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtBasicAmount.Text);
                        SC.Basic_Per = (txtBasicPer.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtBasicPer.Text);

                        SC.Status = 1;
                        SC.Created_By = lblCreatedBy.Text;
                        SC.Modified_BY = logIn.username + "-" + DateTime.Now;
                        SC.Company_ID = logIn.company;
                        db.HR_EmpSalInfos.InsertOnSubmit(SC);
                    }
                    db.SubmitChanges();
                    //transaction.Commit();               
                    MessageBox.Show("Details Updated Sucessfully");                    
                   
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }
        public void Clear()
        {

            try
            {
                foreach (Control c in groupBox1.Controls.Cast<Control>().OrderBy(c => c.TabIndex))
                {
                    if (c is TextBox)
                    {
                        c.Text = "";
                    }
                    else
                    if (c is ComboBox)
                    {
                        c.Text = "";
                    }


                }
                grdHeads.DataSource = null;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Products", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void txtBasicPer_TextChanged(object sender, EventArgs e)
        {

        }

        private void grdHeads_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (txtBasicPer.Text != "")
                {
                    decimal GSal = Convert.ToDecimal(txtGrossSalary.Text);
                    decimal BPer = Convert.ToDecimal(txtBasicPer.Text);
                    decimal Bsal = Math.Round(GSal * BPer / 100);
                    txtBasicAmount.Text = Bsal.ToString("0.00");
                                
                    decimal BPer1 = 0;
                    for (int i = 0; i < grdHeads.Rows.Count - 1; i++)
                    {
                        var Prodname = (from d in db.HR_Salary_HeadsInfos

                                        where d.Company_ID == logIn.company && d.Short_Name == grdHeads.Rows[i].Cells["Head_Name"].Value.ToString()
                                        select new { d.Calculation, d.basicper }).ToList();
                        switch (Prodname[0].Calculation)                        {
                                                      
                           
                            case "Balance of Gross":
                                //DataRow dr;
                               
                                decimal TotSal = 0;
                                for (int k = 0; k < i; k++)
                                {
                                    string sal1 = grdHeads.Rows[k].Cells["Amount"].Value.ToString();
                                    TotSal += Convert.ToDecimal(sal1);

                                }

                                //BPer1 = Convert.ToDecimal(Prodname[i].basicper.ToString());
                                grdHeads.Rows[i].Cells["Amount"].Value = (GSal - (Bsal + TotSal)).ToString();                                
                                break;

                        }


                    }
                   
                    //Get Total
                    double totQty = 0;
                    decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
                    for (int i = 0; i < grdHeads.Rows.Count - 1; i++)
                    {
                        y += (grdHeads.Rows[i].Cells["Amount"].Value.ToString() == "" || grdHeads.Rows[i].Cells["Amount"].Value == null || grdHeads.Rows[i].Cells["Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(grdHeads.Rows[i].Cells["Amount"].Value);

                    }

                    txtTotalSalary.Text = (Bsal + y).ToString(".00");
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

        private void txtVersionNo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtVersionNo.Text != "")
                {
                    var salData = (from d in db.SP_HR_GetSalaryInfo_RevWise(logIn.company, Convert.ToInt32(txtEmpID.Text), logIn.BU_ID, Convert.ToInt32(txtVersionNo.Text))

                                   select new { d.Emp_Name, d.emp_code, d.Group_Name, d.GrossSal, d.BasicSal, d.Basic_Per, d.Sal_Rev_No,
                                       d.Sal_Rev_Date }).ToList();

                    if (salData.Count > 0)
                    {
                        //txtEmpName.Text = salData[0].Emp_Name;
                        txtEmpCode.Text = salData[0].emp_code.ToString();
                        txtEmpGroup.Text = salData[0].Group_Name.ToString();
                        txtGrossSalary.Text = salData[0].GrossSal.ToString();
                        txtBasicPer.Text = salData[0].Basic_Per.ToString();
                        txtBasicAmount.Text = salData[0].BasicSal.ToString();
                       // txtVersionNo.Text = salData[0].Sal_Rev_No.ToString();
                        dateTimePicker1.Text = salData[0].Sal_Rev_Date.ToString();
                        txtTotalSalary.Text = salData[0].GrossSal.ToString();
                        
                    }

                    //Get Salary Heads wise info

                    var getHeads = (from d in db.SP_HR_GetSalaryInfo_HeadWise(logIn.company, Convert.ToInt32(txtEmpID.Text), logIn.BU_ID, txtEmpGroup.Text,Convert.ToInt32(txtVersionNo.Text))

                                    select new { d.id, d.Head, d.Amount }).ToList();

                    if (getHeads.Count > 0)
                    {
                        grdHeads.DataSource = getHeads;
                    }


         //           var getSaData = (from d in db.HR_EmpSalInfos                                     
         //                            where d.Company_ID == logIn.company
         //&& d.Emp_Master_ID == Convert.ToInt32(txtEmpID.Text)
         //&& d.Sal_Rev_No == Convert.ToInt32(txtVersionNo.Text)

         //                            select new { id = d.Head_ID, Head_Name = d.Head, d.Amount });

         //           SqlCommand cmd2 = (SqlCommand)db.GetCommand(getSaData);
         //           SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
         //           DataTable dtr = new DataTable();
         //           da2.Fill(dtr);
         //           if (dtr.Rows.Count >= 0)
         //               grdHeads.DataSource = dtr;
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
    }
}
