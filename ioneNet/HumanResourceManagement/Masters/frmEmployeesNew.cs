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
//using iTextSharp.text;


namespace ioneNet.HumanResourceManagement.Masters
{
    public partial class frmEmployeesNew : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        SqlCommand cmd;
        string columnName;
        public frmEmployeesNew()
        {
            InitializeComponent();
        }

        private void label30_Click(object sender, EventArgs e)
        {

        }

        private void frmProductsNew_Load(object sender, EventArgs e)
        {
            //this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

            cmbEmpGroup.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbEmpGroup.AutoCompleteSource = AutoCompleteSource.ListItems;

            cmbGender.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbGender.AutoCompleteSource = AutoCompleteSource.ListItems;

            cmbMarried.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbMarried.AutoCompleteSource = AutoCompleteSource.ListItems;

            cmbNationality.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbNationality.AutoCompleteSource = AutoCompleteSource.ListItems;

            cmbReligion.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbReligion.AutoCompleteSource = AutoCompleteSource.ListItems;


            cmbDepartment.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbDepartment.AutoCompleteSource = AutoCompleteSource.ListItems;

            cmbDesignation.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbDesignation.AutoCompleteSource = AutoCompleteSource.ListItems;

            cmbLocation.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbLocation.AutoCompleteSource = AutoCompleteSource.ListItems;

            cmbBankName.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbBankName.AutoCompleteSource = AutoCompleteSource.ListItems;

            BindMasters();
            
            if(EmployeeList.var=="0")
            {
                txtEmpID.Text = EmployeeList.productCode.ToString();
                GetProductMasterData(EmployeeList.productCode);
                EmployeeList.var = null;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void GetProductMasterData(int ProductCode)
        {
            try
            {
                var ProductMasterList = (from prdmstr in db.HR_Employee_Master_Datas where prdmstr.id == ProductCode select prdmstr).ToList();
                if (ProductMasterList.Count > 0)
                {
                    txtEmp_Code.Text = (ProductMasterList[0].Emp_Code);
                    txtEmpName.Text = (ProductMasterList[0].Emp_Name);
                    txtFatherName.Text = ProductMasterList[0].Father_Name;
                    cmbEmpGroup.SelectedValue = (ProductMasterList[0].Emp_Group);
                    cmbGender.Text = (ProductMasterList[0].Gender);
                    cmbMarried.Text = (ProductMasterList[0].M_Status);
                    cmbNationality.Text = (ProductMasterList[0].Nationality);     
                    cmbReligion.Text = (ProductMasterList[0].Religion);
                    txtPresentAddress.Text = ProductMasterList[0].Address;
                    txtPresentCity.Text = ProductMasterList[0].City;
                    txtPresentState.Text = ProductMasterList[0].State;
                    txtPermanentAddr.Text = ProductMasterList[0].Permanent_Address;
                    txtPermanentCity.Text = ProductMasterList[0].Permanent_City;
                    txtPresentContactNo.Text = ProductMasterList[0].Phone;                    
                    txtPermanentState.Text = ProductMasterList[0].Permanent_State;
                    txtPermanentContactNo.Text = ProductMasterList[0].Phone_Permanent;

                    if (ProductMasterList[0].PermenanceSameAsPresent != null)
                    {
                        chkisSamePresentAddr.Checked = ProductMasterList[0].PermenanceSameAsPresent.Value;
                    }
                  
                    if(ProductMasterList[0].Department != null)
                    {
                        cmbDepartment.SelectedValue = ProductMasterList[0].Department;

                    }
                   if(ProductMasterList[0].Desig!=null)
                    {
                    cmbDesignation.SelectedValue = (ProductMasterList[0].Desig);
                    }
                    dtDOB.Text = ProductMasterList[0].Birth_Date.ToString();
                    DateTime dob = dtDOB.Value;
                    DateTime dtToday = DateTime.Now;
                    TimeSpan diffResult = dtToday - dob;
                    txtAgeinYears.Text = (diffResult.Days / 365).ToString();

                    dtpDOJ.Text = ProductMasterList[0].Date_Join.ToString();

                    DateTime doj = dtpDOJ.Value;                    
                    TimeSpan diffResult1 = dtToday - doj;
                    txtSerinYears.Text = (diffResult1.Days / 365).ToString();

                    if (ProductMasterList[0].Emp_Division != null)
                    {
                        cmbLocation.SelectedValue = (ProductMasterList[0].Emp_Division);
                    }

                    
                    txtBloodGroup.Text = ProductMasterList[0].B_Group;

                    if (ProductMasterList[0].isBloodDonor != null)
                    {
                        chkBloodDonor.Checked = ProductMasterList[0].isBloodDonor.Value;
                    }
                    cmbStatus.SelectedValue = ProductMasterList[0].Status;

                    //cmbMtrlGrade.Text = Convert.ToString(ProductMasterList[0].Prod_Field1);
                    txtReference1.Text = Convert.ToString(ProductMasterList[0].Reference1);
                    txtReference2.Text = Convert.ToString(ProductMasterList[0].Reference2);
                    txtGrossSalary.Text = Convert.ToString(ProductMasterList[0].GrossSal);

                    txtMobileNo.Text = Convert.ToString(ProductMasterList[0].Mobile);
                    txtEmail.Text = Convert.ToString(ProductMasterList[0].Mail);
                    txtEmegencyContact.Text = Convert.ToString(ProductMasterList[0].EmergencyNumber);
                    txtBiometricID.Text = Convert.ToString(ProductMasterList[0].BiometricID);
                    txtESINo.Text = Convert.ToString(ProductMasterList[0].ESI_Code);
                    txtPFNo.Text = Convert.ToString(ProductMasterList[0].PFUAN);
                    txtMedicalPolicy.Text = Convert.ToString(ProductMasterList[0].Medical_Policy_No);
                    chkMedicalCovered.Checked = ProductMasterList[0].MedicalCovered.Value;
                    txtPANNo.Text = Convert.ToString(ProductMasterList[0].PANNo);
                    txtAadhano.Text = Convert.ToString(ProductMasterList[0].AadharNo);
                    chkEligibleOT.Checked = ProductMasterList[0].OTEligible.Value;
                    cmbSalaryMode.Text = Convert.ToString(ProductMasterList[0].Salary_Mode);
                    txtbankAccountNo.Text = Convert.ToString(ProductMasterList[0].Bank_Ac_No);
                    cmbBankName.Text = ProductMasterList[0].Bank_Name.ToString();
                    txtBankIFSCCode.Text = ProductMasterList[0].Bank_IFSC_Code.ToString();
                    cmbEduQualification.Text =  ProductMasterList[0].EduQualificiation.ToString();
                    cmbPrevExp.Text = ProductMasterList[0].ProfExp.ToString();
                    txtPassPortNo.Text = ProductMasterList[0].Passport_No.ToString();
                    txtDLNo.Text = ProductMasterList[0].Driving_Licence_NO.ToString();
                    dtPassportValidity.Text = ProductMasterList[0].Passport_Validity.ToString();
                    dtDLValidity.Text = ProductMasterList[0].DL_Validity.ToString();
                    if (ProductMasterList[0].Pf_limit != null)
                    {
                        chkpflimit.Checked = ProductMasterList[0].Pf_limit.Value;
                    }
                    if (ProductMasterList[0].Emp_Image != null)
                    {
                        var f = (from s in db.HR_Employee_Master_Datas where s.id == Convert.ToInt32(txtEmpID.Text) select s);
                        SqlCommand cmd = (SqlCommand)db.GetCommand(f);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        DataTable dt = new DataTable();
                        DataSet ds = new DataSet("MyImages");
                        byte[] MyData = new byte[0];
                        da.Fill(ds, "MyImages");
                        DataRow myRow;
                        myRow = ds.Tables["MyImages"].Rows[0];
                        MyData = (byte[])myRow["Emp_Image"];
                        MemoryStream stream = new MemoryStream(MyData);
                        picEmployee.Image = Image.FromStream(stream);


                    }
                    if (ProductMasterList[0].Langauges_Known != null)
                    {
                        string MP = ProductMasterList[0].Langauges_Known.ToString();
                        string[] values = MP.Split(',');
                        for (int j = 0; j < values.Length; j++)
                        {
                            values[j] = values[j].Trim();
                            string m = values[j].ToString();
                            for (int i = 0; i < ckhLanguages.Items.Count; i++)
                            {
                                if (ckhLanguages.Items[i].ToString() == m)
                                {
                                    ckhLanguages.SetItemChecked(i, true);
                                }
                            }
                        }
                    }
                    linkLabel1.Text = ProductMasterList[0].Aadhar_File_Path;
                    linkLabel2.Text = ProductMasterList[0].Pan_File_Path;
                    linkLabel3.Text = ProductMasterList[0].Edu_Certifcate_Path;
                    linkLabel4.Text = ProductMasterList[0].Passport_File_Path;
                    linkLabel5.Text = ProductMasterList[0].Driving_Licence_Path;
                    linkLabel6.Text = ProductMasterList[0].Resume_File_Path;

                    //Get Family Data
                    var dm1 = (from s in db.HR_Emp_FamilyDatas
                               where s.Emp_Master_ID == Convert.ToInt32(txtEmpID.Text) 


                               select new

                               {
                                   Family_Member_Name = s.Emp_Relation_Name,
                                   Familiy_Relation = s.Emp_Relation,
                                   Mobile_Number = s.Emp_Releation_Mobile,
                                   Dependent = s.isDependent,
                                   Nominee = s.isNominee                                 

                               });
                    SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    DataTable dtr = new DataTable();
                    da2.Fill(dtr);
                    if (dtr.Rows.Count >= 0)
                        dgFamilyData.DataSource = dtr;

                    //Get Prev Exp
                    var dm2 = (from s in db.HR_Emp_Prv_Exp_Datas
                               where s.Emp_Master_ID == Convert.ToInt32(txtEmpID.Text)


                               select new

                               {
                                   Organization_Name = s.Emp_Prev_Organization,
                                   Designation = s.Emp_Prev_Designation,
                                   Period_From = s.Emp_Prev_Perido_From,
                                   Period_To = s.Emp_Prev_Perido_To,
                                   No_Of_Yrs = s.Emp_Prev_exp_Yrs,
                                   CTC = s.Emp_Prev_CTC

                               });
                    SqlCommand cmd3 = (SqlCommand)db.GetCommand(dm2);
                    SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                    DataTable dtr1 = new DataTable();
                    da3.Fill(dtr1);
                    if (dtr1.Rows.Count >= 0)
                        dgPrevExp.DataSource = dtr1;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Employees", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

        }
        public static void CheckFill(Control control, bool enabled)
        {
            control.Enabled = enabled;
            foreach (Control child in control.Controls)
            {
                if (child is TextBox && string.IsNullOrWhiteSpace(child.Text))
                {
                    if (child.Tag == "r")
                    {
                        MessageBox.Show(string.Format("Field {0} Cannot Be Empty", child.Name.Substring(3)));
                        child.Focus();
                        return;
                    }

                }
                CheckFill(child, enabled);
            }
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime dob = dtDOB.Value;
                DateTime dtToday = DateTime.Now;
                TimeSpan diffResult = dtToday - dob;
                txtAgeinYears.Text = (diffResult.Days / 365).ToString();

                if (cmbEmpGroup.Text == string.Empty)
                {
                    MessageBox.Show("Employee Group Should Not Be Empty", "Employees", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbEmpGroup.Focus();
                    return;
                }
                else if (txtEmpName.Text == string.Empty)
                {
                    MessageBox.Show("Employee Name  Should Not Be Empty");
                    txtEmpName.Focus();
                    return;
                }
                else if (cmbGender.Text == string.Empty)
                {
                    MessageBox.Show("Select Gender,");
                    cmbGender.Focus();
                    return;
                }
                else if (cmbDepartment.Text == string.Empty)
                {
                    MessageBox.Show("Select Department Name,");
                    cmbDepartment.Focus();
                    return;
                }

                else if (cmbDesignation.Text == string.Empty)
                {
                    MessageBox.Show("Select Designation");
                    cmbDesignation.Focus();
                    return;
                }
                else if (cmbLocation.Text == string.Empty)
                {
                    MessageBox.Show("Select Division /Location ");
                    cmbLocation.Focus();
                    return;
                }

                else if (Convert.ToInt32(txtAgeinYears.Text) < 18)
                {
                    MessageBox.Show("Employee Age Must Be Above 18 Years");
                    dtDOB.Focus();
                    return;
                }
                
                else if (cmbStatus.Text == string.Empty)
                {
                    MessageBox.Show("Select Status ");
                    cmbStatus.Focus();
                    return;
                }
                else
                {
                    save();


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Products", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Products", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void autogen()
        {
            try

            {
                //var result = db.Sp_autoincrement_ProdMaster(Convert.ToInt32(cmbProdType.SelectedValue),logIn.company);
                if (cmbEmpGroup.Text == "Project Items")
                {
                    var result = db.Sp_autoincrement_ProdMaster_ProjectItems(txtEmpID.Text, Convert.ToInt32(cmbGender.SelectedValue), logIn.company);
                    txtEmp_Code.Text = result.FirstOrDefault().Product_Code;
                }
                else
                {
                    var result = db.Sp_autoincrement_ProdMaster_New(Convert.ToInt32(cmbEmpGroup.SelectedValue), Convert.ToInt32(cmbGender.SelectedValue), logIn.company);
                    txtEmp_Code.Text = result.FirstOrDefault().Product_Code;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Products", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void BindMasters()
        {
            try
            {
                //Bind EMP Groups
                var bindTypes = (from m in db.HR_Employee_Groups
                                 where m.Company_ID == logIn.company
                                 select new
                                 {
                                     m.Group_Name,
                                     m.ID,
                                 }).ToList();

                if (bindTypes.Count > 0)
                {
                    cmbEmpGroup.DataSource = bindTypes;
                    cmbEmpGroup.DisplayMember = "Group_Name";
                    cmbEmpGroup.ValueMember = "ID";
                    cmbEmpGroup.SelectedIndex = -1;

                }

                //Bind Department
                var bindGroups = (from m in db.Department_Masters
                                  where m.Company_ID == logIn.company
                                  select new
                                  {
                                      m.Dept_Name,
                                      m.Id,
                                  }).ToList();

                if (bindGroups.Count > 0)
                {
                    cmbDepartment.DataSource = bindGroups;
                    cmbDepartment.DisplayMember = "Dept_Name";
                    cmbDepartment.ValueMember = "Id";
                    cmbDepartment.SelectedIndex = -1;
                }


                //Bind Desig               
               
                var bindUOM = (from m in db.HR_Emp_DesigInfos
                               where m.Company_ID == logIn.company
                               select new
                               {
                                   m.Designation,
                                   m.ID,
                               }).ToList();

                if (bindUOM.Count > 0)
                {
                    cmbDesignation.DataSource = bindUOM;
                    cmbDesignation.DisplayMember = "Designation";
                    cmbDesignation.ValueMember = "ID";
                    cmbDesignation.SelectedIndex = -1;
                    //cmbAltUom.DataSource = bindUOM;
                    //cmbAltUom.DisplayMember = "Uom_Descr";
                    //cmbAltUom.ValueMember = "UOM_ID";

                }
                

                //Bind Bank Namess
                var bindTax = (from m in db.HR_Employee_Master_Datas
                               where m.Company_ID == logIn.company
                               select new
                               {
                                   m.Bank_Name
                                   
                               }).Distinct().ToList();

                if (bindTax.Count > 0)
                {

                    cmbBankName.DataSource = bindTax;
                    cmbBankName.DisplayMember = "Bank_Name";
                    cmbBankName.ValueMember = "Bank_Name";
                    cmbBankName.SelectedIndex = -1;

                }
               
                //Status
                var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Emp_Status" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbStatus.DataSource = pStatus;
                    cmbStatus.ValueMember = "ID";
                    cmbStatus.DisplayMember = "Descr";
                }

                //Scrap Product
                var pscrap = (from m in db.HR_DivisionMasters where m.Company_ID == logIn.company select new { m.ID, m.DivisionName }).Distinct().ToList();
                if (pscrap.Count > 0)
                {
                    cmbLocation.DataSource = pscrap;
                    cmbLocation.ValueMember = "ID";
                    cmbLocation.DisplayMember = "DivisionName";
                    cmbLocation.SelectedIndex = -1;
                }

              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbProdType_Leave(object sender, EventArgs e)
        {
            try
            {
                
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void save()
        {
            Cursor.Current = Cursors.WaitCursor;           
                try
                {
                //if ((from u in db.HR_Employee_Master_Datas where u.id == Convert.ToInt32(txtEmpID.Text) && u.Company_ID == logIn.company select u).Count() > 0)
                    if(txtEmpID.Text!="")
                {
                    var p1 = db.HR_Employee_Master_Datas.Where(w => w.id == Convert.ToInt32(txtEmpID.Text) && w.Company_ID == logIn.company).FirstOrDefault();

                    p1.Emp_Code = txtEmp_Code.Text;                   
                    p1.Gender = cmbGender.Text;                   
                    p1.Emp_Group = Convert.ToInt32(cmbEmpGroup.SelectedValue.ToString());
                    p1.Emp_Name = txtEmpName.Text;
                    p1.Father_Name = txtFatherName.Text;
                    p1.Religion = cmbReligion.Text;
                    p1.M_Status = cmbMarried.Text;
                    p1.Nationality = cmbNationality.Text;
                    p1.Address = (txtPresentAddress.Text == "")? "" :txtPresentAddress.Text;
                    p1.City = (txtPresentCity.Text == "") ? "" : txtPresentCity.Text;
                    p1.State = (txtPresentState.Text == "") ? "" : txtPresentState.Text;
                    p1.Phone = (txtPresentContactNo.Text == "") ? "" : txtPresentContactNo.Text;

                    p1.PermenanceSameAsPresent = chkisSamePresentAddr.Checked;
                    p1.Permanent_Address = (txtPermanentAddr.Text == "") ? "" : txtPermanentAddr.Text;
                    p1.Permanent_City = (txtPermanentCity.Text == "") ? "" : txtPermanentCity.Text;
                    p1.Permanent_State = (txtPermanentState.Text == "") ? "" : txtPermanentState.Text;
                    p1.Phone_Permanent = (txtPermanentContactNo.Text == "") ? "" : txtPermanentContactNo.Text;
                    p1.Birth_Date = dtDOB.Value;
                    p1.Age = txtAgeinYears.Text;
                    p1.B_Group = txtBloodGroup.Text;
                    p1.isBloodDonor = chkBloodDonor.Checked;
                    p1.Reference1 = txtReference1.Text;
                    p1.Reference2 = txtReference2.Text;
                    p1.Mobile = (txtMobileNo.Text == "") ? "" : txtMobileNo.Text;
                    p1.Mail = (txtEmail.Text == "") ? "" : txtEmail.Text;
                    p1.EmergencyNumber = (txtEmegencyContact.Text == "") ? "" : txtEmegencyContact.Text;
                    //p.Prod_GST_Rate = (cmbTaxClass.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(cmbTaxClass,.Text);
                    if (cmbDepartment.Text != "")
                    {
                        p1.Department = Convert.ToInt32(cmbDepartment.SelectedValue.ToString());
                    }
                    if (cmbDesignation.Text != "")
                    {
                        p1.Desig = Convert.ToInt32(cmbDesignation.SelectedValue.ToString());
                    }
                    if (cmbLocation.Text != "")
                    {
                        p1.Emp_Division = Convert.ToInt32(cmbLocation.SelectedValue.ToString());
                    }                   
                    p1.BU_ID =logIn.BU_ID;

                    if (picEmployee.Image != null)
                    {
                        Image img = picEmployee.Image;
                        System.IO.MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] bytes = ms.ToArray();
                        p1.Emp_Image = bytes;
                    }
                    else
                    {
                        p1.Emp_Image = null;
                    }


                    p1.Date_Join = dtpDOJ.Value;
                    p1.BiometricID = txtBiometricID.Text;
                    p1.GrossSal = (txtGrossSalary.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtGrossSalary.Text);
                    p1.OTEligible = chkEligibleOT.Checked;
                    p1.MedicalCovered = chkMedicalCovered.Checked ;
                    
                    p1.Status = Convert.ToInt32(cmbStatus.SelectedValue.ToString());
                    p1.PFUAN = (txtPFNo.Text == "") ? "" : txtPFNo.Text;
                    p1.ESI_Code = (txtESINo.Text == "") ? "" : txtESINo.Text;
                    p1.Medical_Policy_No = (txtMedicalPolicy.Text == "") ? "" : txtMedicalPolicy.Text;
                    p1.PANNo = (txtPANNo.Text == "") ? "" : txtPANNo.Text;
                    p1.is_Resigned = false;
                    p1.AadharNo = (txtAadhano.Text == "") ? "" : txtAadhano.Text;
                    p1.Salary_Mode = (cmbSalaryMode.Text == "") ? "" : cmbSalaryMode.Text;
                    p1.Bank_Ac_No = (txtbankAccountNo.Text == "") ? "" : txtbankAccountNo.Text;
                    p1.Bank_Name = (cmbBankName.Text == "") ? "" : cmbBankName.Text;
                    p1.Bank_IFSC_Code = (txtBankIFSCCode.Text == "") ? "" : txtBankIFSCCode.Text;
                    p1.EduQualificiation = (cmbEduQualification.Text == "") ? "" : cmbEduQualification.Text;
                    p1.ProfExp = (cmbPrevExp.Text == "") ? "" : cmbPrevExp.Text;
                    p1.Passport_No = (txtPassPortNo.Text == "") ? "" : txtPassPortNo.Text;
                    p1.Driving_Licence_NO = (txtDLNo.Text == "") ? "" : txtDLNo.Text;
                    p1.Passport_Validity = dtPassportValidity.Value;
                    p1.DL_Validity = dtDLValidity.Value;
                    p1.Created_By = lblCreatedBy.Text;
                    p1.Pf_limit = chkpflimit.Checked;
                    p1.Modified_BY = logIn.username + "-" + DateTime.Now;
                    p1.Company_ID = logIn.company;

                    string Languages_Known = "";
                    for (int i = 0; i < ckhLanguages.Items.Count; i++)
                    {
                        if (ckhLanguages.GetItemChecked(i))
                        {
                            if (Languages_Known != "")
                            {
                                Languages_Known = Languages_Known + "," + ckhLanguages.Items[i].ToString();
                            }
                            else
                            {
                                Languages_Known = ckhLanguages.Items[i].ToString();
                            }
                        }
                    }

                    p1.Langauges_Known = Languages_Known;


                    //Save Document files
                    if (linkLabel1.Text != "" && linkLabel1.Text!= "Open File")  //Aadhar Card
                    {
                        string varFilePath = linkLabel1.Text;
                        byte[] file;
                        using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = new BinaryReader(stream))
                            {
                                file = reader.ReadBytes((int)stream.Length);
                            }
                        }
                        p1.Aadhar_File_Path = linkLabel1.Text;
                        p1.Aadhar_File = file;
                    }
                    if (linkLabel2.Text != "" && linkLabel2.Text != "Open File")  //PAN Card
                    {
                        string varFilePath = linkLabel2.Text;
                        byte[] file;
                        using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = new BinaryReader(stream))
                            {
                                file = reader.ReadBytes((int)stream.Length);
                            }
                        }
                        p1.Pan_File_Path = linkLabel2.Text;
                        p1.Pan_File = file;
                    }
                    if (linkLabel3.Text != "" && linkLabel3.Text != "Open File")  //Edu Certificates
                    {
                        string varFilePath = linkLabel3.Text;
                        byte[] file;
                        using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = new BinaryReader(stream))
                            {
                                file = reader.ReadBytes((int)stream.Length);
                            }
                        }
                        p1.Edu_Certifcate_Path = linkLabel3.Text;
                        p1.Edu_Certifcate_File = file;
                    }
                    if (linkLabel4.Text != "" && linkLabel4.Text != "Open File")  //Passport
                    {
                        string varFilePath = linkLabel4.Text;
                        byte[] file;
                        using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = new BinaryReader(stream))
                            {
                                file = reader.ReadBytes((int)stream.Length);
                            }
                        }
                        p1.Passport_File_Path = linkLabel4.Text;
                        p1.Passport_File = file;
                    }
                    if (linkLabel5.Text != "" && linkLabel5.Text != "Open File")  //Driving Licence
                    {
                        string varFilePath = linkLabel5.Text;
                        byte[] file;
                        using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = new BinaryReader(stream))
                            {
                                file = reader.ReadBytes((int)stream.Length);
                            }
                        }
                        p1.Driving_Licence_Path = linkLabel5.Text;
                        p1.Driving_Licence_File = file;
                    }
                    if (linkLabel6.Text != "" && linkLabel6.Text != "Open File")  //Resume
                    {
                        string varFilePath = linkLabel6.Text;
                        byte[] file;
                        using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = new BinaryReader(stream))
                            {
                                file = reader.ReadBytes((int)stream.Length);
                            }
                        }
                        p1.Resume_File_Path = linkLabel6.Text;
                        p1.Resume_File = file;
                    }
                    db.SubmitChanges();

                    //Save Family Data
                    SqlCommand cmd1 = new SqlCommand("delete  from [HR_Emp_FamilyData] where Emp_Master_ID =@ProdID", con);
                    cmd1.Parameters.AddWithValue("@ProdID", Convert.ToInt32(txtEmpID.Text));

                    if (con.State != ConnectionState.Open)
                        con.Open();
                    //con.Open();
                    cmd1.ExecuteNonQuery();
                    con.Close();
                    for (int i = 0; i < dgFamilyData.RowCount - 1; i++)
                    {
                        HR_Emp_FamilyData SC = new HR_Emp_FamilyData();
                        var d1 = (from a in db.HR_Employee_Master_Datas where a.Emp_Code == txtEmp_Code.Text && a.Company_ID == logIn.company select new { a.id }).ToList();
                        SC.Emp_Master_ID = d1[0].id;                      
                        SC.Emp_Relation_Name = (dgFamilyData.Rows[i].Cells["Family_Member_Name"].Value == null) ? "" : (dgFamilyData.Rows[i].Cells["Family_Member_Name"].Value).ToString();
                        SC.Emp_Relation = (dgFamilyData.Rows[i].Cells["Familiy_Relation"].Value == null) ? "" : (dgFamilyData.Rows[i].Cells["Familiy_Relation"].Value).ToString();
                        SC.Emp_Releation_Mobile = (dgFamilyData.Rows[i].Cells["Mobile_Number"].Value == null) ? "" : (dgFamilyData.Rows[i].Cells["Mobile_Number"].Value).ToString();
                        SC.isDependent = Convert.ToBoolean(dgFamilyData.Rows[i].Cells["Dependent"].Value);
                        //SC.isNominee = Convert.ToBoolean(dgFamilyData.Rows[i].Cells["Nominee"].Value);
                        SC.isNominee =  (dgFamilyData.Rows[i].Cells["Nominee"].Value == DBNull.Value) ? Convert.ToBoolean("0") : Convert.ToBoolean(dgFamilyData.Rows[i].Cells["Nominee"].Value);
                        db.HR_Emp_FamilyDatas.InsertOnSubmit(SC);
                    }
                    db.SubmitChanges();

                    //Save Prev Exp Data
                    SqlCommand cmd2 = new SqlCommand("delete  from [HR_Emp_Prv_Exp_Data] where Emp_Master_ID =@ProdID", con);
                    cmd2.Parameters.AddWithValue("@ProdID", Convert.ToInt32(txtEmpID.Text));

                    if (con.State != ConnectionState.Open)
                        con.Open();
                    //con.Open();
                    cmd2.ExecuteNonQuery();
                    con.Close();
                    for (int i = 0; i < dgPrevExp.RowCount - 1; i++)
                    {
                        HR_Emp_Prv_Exp_Data SC = new HR_Emp_Prv_Exp_Data();
                        var d1 = (from a in db.HR_Employee_Master_Datas where a.Emp_Code == txtEmp_Code.Text && a.Company_ID == logIn.company select new { a.id }).ToList();
                        SC.Emp_Master_ID = d1[0].id;
                        SC.Emp_Prev_Organization = (dgPrevExp.Rows[i].Cells["Organization_Name"].Value == null) ? "" : (dgPrevExp.Rows[i].Cells["Organization_Name"].Value).ToString();
                        SC.Emp_Prev_Designation = (dgPrevExp.Rows[i].Cells["Designation"].Value == null) ? "" : (dgPrevExp.Rows[i].Cells["Designation"].Value).ToString();
                        SC.Emp_Prev_Perido_From = (dgPrevExp.Rows[i].Cells["Period_From"].Value == null) ? "" : (dgPrevExp.Rows[i].Cells["Period_From"].Value).ToString();
                        SC.Emp_Prev_Perido_To = (dgPrevExp.Rows[i].Cells["Period_To"].Value == null) ? "" : (dgPrevExp.Rows[i].Cells["Period_To"].Value).ToString();
                        SC.Emp_Prev_exp_Yrs = (dgPrevExp.Rows[i].Cells["No_Of_Yrs"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgPrevExp.Rows[i].Cells["No_Of_Yrs"].Value);
                        SC.Emp_Prev_CTC = (dgPrevExp.Rows[i].Cells["CTC"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgPrevExp.Rows[i].Cells["CTC"].Value);

                        db.HR_Emp_Prv_Exp_Datas.InsertOnSubmit(SC);
                    }
                    db.SubmitChanges();


                    MessageBox.Show("Record Updated Successfully");
                    this.Close();


                }
                else
                {
                    if (null != db.Connection)
                    {
                        db.Connection.Close();
                    }
                    System.Data.Common.DbTransaction transaction;
                    db.Connection.Open();
                    transaction = db.Connection.BeginTransaction();
                    db.Transaction = transaction;

                    HR_Employee_Master_Data p = new HR_Employee_Master_Data();
                    p.Emp_Code = txtEmp_Code.Text;
                    p.Gender = cmbGender.Text;
                    p.Emp_Group = Convert.ToInt32(cmbEmpGroup.SelectedValue.ToString());
                    p.Emp_Name = txtEmpName.Text;
                    p.Father_Name = txtFatherName.Text;
                    p.Religion = cmbReligion.Text;
                    p.M_Status = cmbMarried.Text;
                    p.Nationality = cmbNationality.Text;
                    p.Address = (txtPresentAddress.Text == "") ? "" : txtPresentAddress.Text;
                    p.City = (txtPresentCity.Text == "") ? "" : txtPresentCity.Text;
                    p.State = (txtPresentState.Text == "") ? "" : txtPresentState.Text;
                    p.Phone = (txtPresentContactNo.Text == "") ? "" : txtPresentContactNo.Text;
                    p.PermenanceSameAsPresent = chkisSamePresentAddr.Checked;
                    p.Permanent_Address = (txtPermanentAddr.Text == "") ? "" : txtPermanentAddr.Text;
                    p.Permanent_City = (txtPermanentCity.Text == "") ? "" : txtPermanentCity.Text;
                    p.Permanent_State = (txtPermanentState.Text == "") ? "" : txtPermanentState.Text;
                    p.Phone_Permanent = (txtPermanentContactNo.Text == "") ? "" : txtPermanentContactNo.Text;
                    p.Birth_Date = dtDOB.Value;
                    p.Age = txtAgeinYears.Text;
                    p.B_Group = txtBloodGroup.Text;
                    p.isBloodDonor = chkBloodDonor.Checked;
                    p.Reference1 = txtReference1.Text;
                    p.Reference2 = txtReference2.Text;
                    p.Mobile = (txtMobileNo.Text == "") ? "" : txtMobileNo.Text;
                    p.Mail = (txtEmail.Text == "") ? "" : txtEmail.Text;
                    p.EmergencyNumber = (txtEmegencyContact.Text == "") ? "" : txtEmegencyContact.Text;
                    //p.Prod_GST_Rate = (cmbTaxClass.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(cmbTaxClass,.Text);
                    if (cmbDepartment.Text != "")
                    {
                        p.Department = Convert.ToInt32(cmbDepartment.SelectedValue.ToString());
                    }
                    if (cmbDesignation.Text != "")
                    {
                        p.Desig = Convert.ToInt32(cmbDesignation.SelectedValue.ToString());
                    }
                    
                    p.BU_ID = logIn.BU_ID;
                   
                    if (cmbLocation.Text != "")
                    {
                        p.Emp_Division = Convert.ToInt32(cmbLocation.SelectedValue.ToString());
                    }
                    if (picEmployee.Image != null)
                    {
                        Image img = picEmployee.Image;
                        System.IO.MemoryStream ms = new MemoryStream();
                        img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        byte[] bytes = ms.ToArray();
                        p.Emp_Image = bytes;
                    }
                    else
                        p.Emp_Image = null;
                    p.Date_Join = dtpDOJ.Value;
                    p.BiometricID = txtBiometricID.Text;
                    p.GrossSal = (txtGrossSalary.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtGrossSalary.Text);
                    p.OTEligible = chkEligibleOT.Checked;
                    p.MedicalCovered = chkMedicalCovered.Checked;
                    p.is_Resigned = false;
                    p.Status = Convert.ToInt32(cmbStatus.SelectedValue.ToString());
                    p.PFUAN = (txtPFNo.Text == "") ? "" : txtPFNo.Text;
                    p.ESI_Code = (txtESINo.Text == "") ? "" : txtESINo.Text;
                    p.Medical_Policy_No = (txtMedicalPolicy.Text == "") ? "" : txtMedicalPolicy.Text;
                    p.PANNo = (txtPANNo.Text == "") ? "" : txtPANNo.Text;

                    p.AadharNo = (txtAadhano.Text == "") ? "" : txtAadhano.Text;
                    p.Salary_Mode = (cmbSalaryMode.Text == "") ? "" : cmbSalaryMode.Text;
                    p.Bank_Ac_No = (txtbankAccountNo.Text == "") ? "" : txtbankAccountNo.Text;
                    p.Bank_Name = (cmbBankName.Text == "") ? "" : cmbBankName.Text;
                    p.Bank_IFSC_Code = (txtBankIFSCCode.Text == "") ? "" : txtBankIFSCCode.Text;
                    p.EduQualificiation = (cmbEduQualification.Text == "") ? "" : cmbEduQualification.Text;
                    p.ProfExp = (cmbPrevExp.Text == "") ? "" : cmbPrevExp.Text;
                    p.Passport_No = (txtPassPortNo.Text == "") ? "" : txtPassPortNo.Text;
                    p.Driving_Licence_NO = (txtDLNo.Text == "") ? "" : txtDLNo.Text;
                    p.Passport_Validity = dtPassportValidity.Value;
                    p.DL_Validity = dtDLValidity.Value;
                    p.Created_By = lblCreatedBy.Text;
                    p.Modified_BY = logIn.username + "-" + DateTime.Now;
                    p.Company_ID = logIn.company;
                    p.Pf_limit = chkpflimit.Checked;
                    string Languages_Known = "";
                    for (int i = 0; i < ckhLanguages.Items.Count; i++)
                    {
                        if (ckhLanguages.GetItemChecked(i))
                        {
                            if (Languages_Known != "")
                            {
                                Languages_Known = Languages_Known + "," + ckhLanguages.Items[i].ToString();
                            }
                            else
                            {
                                Languages_Known = ckhLanguages.Items[i].ToString();
                            }
                        }
                    }

                    p.Langauges_Known = Languages_Known;


                    //Save Document files
                    if (linkLabel1.Text != "" && linkLabel1.Text != "Open File")  //Aadhar Card
                    {
                        string varFilePath = linkLabel1.Text;
                        byte[] file;
                        using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = new BinaryReader(stream))
                            {
                                file = reader.ReadBytes((int)stream.Length);
                            }
                        }
                        p.Aadhar_File_Path = linkLabel1.Text;
                        p.Aadhar_File = file;
                    }
                    if (linkLabel2.Text != "" && linkLabel2.Text != "Open File")  //PAN Card
                    {
                        string varFilePath = linkLabel2.Text;
                        byte[] file;
                        using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = new BinaryReader(stream))
                            {
                                file = reader.ReadBytes((int)stream.Length);
                            }
                        }
                        p.Pan_File_Path = linkLabel2.Text;
                        p.Pan_File = file;
                    }
                    if (linkLabel3.Text != "" && linkLabel3.Text != "Open File")  //Edu Certificates
                    {
                        string varFilePath = linkLabel3.Text;
                        byte[] file;
                        using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = new BinaryReader(stream))
                            {
                                file = reader.ReadBytes((int)stream.Length);
                            }
                        }
                        p.Edu_Certifcate_Path = linkLabel3.Text;
                        p.Edu_Certifcate_File = file;
                    }
                    if (linkLabel4.Text != "" && linkLabel4.Text != "Open File")   //Passport
                    {
                        string varFilePath = linkLabel4.Text;
                        byte[] file;
                        using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = new BinaryReader(stream))
                            {
                                file = reader.ReadBytes((int)stream.Length);
                            }
                        }
                        p.Passport_File_Path = linkLabel4.Text;
                        p.Passport_File = file;
                    }
                    if (linkLabel5.Text != "" && linkLabel5.Text != "Open File")  //Driving Licence
                    {
                        string varFilePath = linkLabel5.Text;
                        byte[] file;
                        using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = new BinaryReader(stream))
                            {
                                file = reader.ReadBytes((int)stream.Length);
                            }
                        }
                        p.Driving_Licence_Path = linkLabel5.Text;
                        p.Driving_Licence_File = file;
                    }
                    if (linkLabel6.Text != "" && linkLabel6.Text != "Open File")  //Resume
                    {
                        string varFilePath = linkLabel6.Text;
                        byte[] file;
                        using (var stream = new FileStream(varFilePath, FileMode.Open, FileAccess.Read))
                        {
                            using (var reader = new BinaryReader(stream))
                            {
                                file = reader.ReadBytes((int)stream.Length);
                            }
                        }
                        p.Resume_File_Path = linkLabel6.Text;
                        p.Resume_File = file;
                    }

                    db.HR_Employee_Master_Datas.InsertOnSubmit(p);
                    db.SubmitChanges();
                    db.Transaction = transaction;
                    transaction.Commit();

                    for (int i = 0; i < dgFamilyData.RowCount - 1; i++)
                    {
                        HR_Emp_FamilyData SC = new HR_Emp_FamilyData();
                        var d1 = (from a in db.HR_Employee_Master_Datas where a.Emp_Code == txtEmp_Code.Text && a.Company_ID == logIn.company select new { a.id }).ToList();
                        SC.Emp_Master_ID = d1[0].id;
                        SC.Emp_Relation_Name = (dgFamilyData.Rows[i].Cells["Family_Member_Name"].Value == null) ? "" : (dgFamilyData.Rows[i].Cells["Family_Member_Name"].Value).ToString();
                        SC.Emp_Relation = (dgFamilyData.Rows[i].Cells["Familiy_Relation"].Value == null) ? "" : (dgFamilyData.Rows[i].Cells["Familiy_Relation"].Value).ToString();
                        SC.Emp_Releation_Mobile = (dgFamilyData.Rows[i].Cells["Mobile_Number"].Value == null) ? "" : (dgFamilyData.Rows[i].Cells["Mobile_Number"].Value).ToString();
                        SC.isDependent = Convert.ToBoolean(dgFamilyData.Rows[i].Cells["Dependent"].Value);
                        //SC.isNominee = Convert.ToBoolean(dgFamilyData.Rows[i].Cells["Nominee"].Value);
                        SC.isNominee = (dgFamilyData.Rows[i].Cells["Nominee"].Value == DBNull.Value) ? Convert.ToBoolean("0") : Convert.ToBoolean(dgFamilyData.Rows[i].Cells["Nominee"].Value);
                        db.HR_Emp_FamilyDatas.InsertOnSubmit(SC);
                    }
                    db.SubmitChanges();

                    //Save Prev Exp Data                   
                  
                    for (int i = 0; i < dgPrevExp.RowCount - 1; i++)
                    {
                        HR_Emp_Prv_Exp_Data SC = new HR_Emp_Prv_Exp_Data();
                        var d1 = (from a in db.HR_Employee_Master_Datas where a.Emp_Code == txtEmp_Code.Text && a.Company_ID == logIn.company select new { a.id }).ToList();
                        SC.Emp_Master_ID = d1[0].id;
                        SC.Emp_Prev_Organization = (dgPrevExp.Rows[i].Cells["Organization_Name"].Value == null) ? "" : (dgPrevExp.Rows[i].Cells["Organization_Name"].Value).ToString();
                        SC.Emp_Prev_Designation = (dgPrevExp.Rows[i].Cells["Designation"].Value == null) ? "" : (dgPrevExp.Rows[i].Cells["Designation"].Value).ToString();
                        SC.Emp_Prev_Perido_From = (dgPrevExp.Rows[i].Cells["Period_From"].Value == null) ? "" : (dgPrevExp.Rows[i].Cells["Period_From"].Value).ToString();
                        SC.Emp_Prev_Perido_To = (dgPrevExp.Rows[i].Cells["Period_To"].Value == null) ? "" : (dgPrevExp.Rows[i].Cells["Period_To"].Value).ToString();
                        SC.Emp_Prev_exp_Yrs = (dgPrevExp.Rows[i].Cells["No_Of_Yrs"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgPrevExp.Rows[i].Cells["No_Of_Yrs"].Value);
                        SC.Emp_Prev_CTC = (dgPrevExp.Rows[i].Cells["CTC"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgPrevExp.Rows[i].Cells["CTC"].Value);

                        db.HR_Emp_Prv_Exp_Datas.InsertOnSubmit(SC);
                    }
                    db.SubmitChanges();


                    MessageBox.Show("Record Saved Successfully");
                    this.Close();
                }
                }
                catch (Exception ex)
                {
                    //transaction.Rollback();
                    MessageBox.Show(ex.Message);
                }            
                
                finally
                {
                    if (null != db.Connection)
                    {
                        db.Connection.Close();
                    }
                }            
            Cursor.Current = Cursors.Default;
        }

        private void cmbProdGroup_Leave(object sender, EventArgs e)
        {
            try
            {
               
                if (cmbGender.Text != "")
                {
                    if (EmployeeList.var != "0" && EmployeeList.var != null)
                    {
                        
                        autogen();
                    }



                }
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbProdType_Leave_1(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                MessageBox.Show(ex.Message);
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {



                OpenFileDialog open = new OpenFileDialog();
                if (open.ShowDialog() == DialogResult.OK)
                {
                    Image img = new Bitmap(open.FileName);
                    picEmployee.Image = img;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtProdID_TextChanged(object sender, EventArgs e)
        {

        }

        private void cmbProdType_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            ioneNet.MaterialManagement.Departments frm = new ioneNet.MaterialManagement.Departments();
            //frm.MdiParent = this.MdiParent;

            frm.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //ioneNet.MaterialManagement.UOM frm = new ioneNet.MaterialManagement.UOM();
            ////frm.MdiParent = this.MdiParent;

            //frm.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ioneNet.HumanResourceManagement.Masters.EmployeeGroups frm = new ioneNet.HumanResourceManagement.Masters.EmployeeGroups();
            //frm.MdiParent = this.MdiParent;

            frm.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ioneNet.HumanResourceManagement.frmDesignations frm = new ioneNet.HumanResourceManagement.frmDesignations();
            //frm.MdiParent = this.MdiParent;

            frm.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            //ioneNet.FinanceManagement.TaxClass frm = new ioneNet.FinanceManagement.TaxClass();
            ////frm.MdiParent = this.MdiParent;

            //frm.Show();
        }

        private void txtEmpName_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            ioneNet.HumanResourceManagement.Masters.frmSalaryStructure frm = new ioneNet.HumanResourceManagement.Masters.frmSalaryStructure();
            //frm.MdiParent = this.MdiParent;

            frm.Show();
        }

        private void txtAadhano_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtAadhano.Text != "")
                {
                    
                    
                    if ((from u in db.HR_Employee_Master_Datas where u.AadharNo == txtAadhano.Text && u.Company_ID == logIn.company select u).Count() > 0)
                    {
                        MessageBox.Show("The Employee is With Same AADHAR Number is already Available", "Employee Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txtAadhano.Focus();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Employee Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            try
            {

                string filename = "";
                OpenFileDialog fdlg = new OpenFileDialog();
                fdlg.Title = "Select file";
                fdlg.InitialDirectory = @"c:\";
                //  fdlg.FileName = txtChooseFile.Text;
                //fdlg.Filter = "Excel Sheet(*.xls)|*.xls|All Files(*.*)|*.*";
                fdlg.FilterIndex = 1;
                fdlg.RestoreDirectory = true;
                if (fdlg.ShowDialog() == DialogResult.OK)
                {
                    filename = fdlg.FileName;
                    linkLabel1.Text = filename;
                    Application.DoEvents();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {

                string filename = "";
                OpenFileDialog fdlg = new OpenFileDialog();
                fdlg.Title = "Select file";
                fdlg.InitialDirectory = @"c:\";
                //  fdlg.FileName = txtChooseFile.Text;
                //fdlg.Filter = "Excel Sheet(*.xls)|*.xls|All Files(*.*)|*.*";
                fdlg.FilterIndex = 1;
                fdlg.RestoreDirectory = true;
                if (fdlg.ShowDialog() == DialogResult.OK)
                {
                    filename = fdlg.FileName;
                    linkLabel2.Text = filename;
                    Application.DoEvents();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            try
            {

                string filename = "";
                OpenFileDialog fdlg = new OpenFileDialog();
                fdlg.Title = "Select file";
                fdlg.InitialDirectory = @"c:\";
                //  fdlg.FileName = txtChooseFile.Text;
                //fdlg.Filter = "Excel Sheet(*.xls)|*.xls|All Files(*.*)|*.*";
                fdlg.FilterIndex = 1;
                fdlg.RestoreDirectory = true;
                if (fdlg.ShowDialog() == DialogResult.OK)
                {
                    filename = fdlg.FileName;
                    linkLabel3.Text = filename;
                    Application.DoEvents();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button8_Click(object sender, EventArgs e)
        {
            try
            {

                string filename = "";
                OpenFileDialog fdlg = new OpenFileDialog();
                fdlg.Title = "Select file";
                fdlg.InitialDirectory = @"c:\";
                //  fdlg.FileName = txtChooseFile.Text;
                //fdlg.Filter = "Excel Sheet(*.xls)|*.xls|All Files(*.*)|*.*";
                fdlg.FilterIndex = 1;
                fdlg.RestoreDirectory = true;
                if (fdlg.ShowDialog() == DialogResult.OK)
                {
                    filename = fdlg.FileName;
                    linkLabel4.Text = filename;
                    Application.DoEvents();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            try
            {

                string filename = "";
                OpenFileDialog fdlg = new OpenFileDialog();
                fdlg.Title = "Select file";
                fdlg.InitialDirectory = @"c:\";
                //  fdlg.FileName = txtChooseFile.Text;
                //fdlg.Filter = "Excel Sheet(*.xls)|*.xls|All Files(*.*)|*.*";
                fdlg.FilterIndex = 1;
                fdlg.RestoreDirectory = true;
                if (fdlg.ShowDialog() == DialogResult.OK)
                {
                    filename = fdlg.FileName;
                    linkLabel5.Text = filename;
                    Application.DoEvents();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCapture_Click(object sender, EventArgs e)
        {

        }

        private void button12_Click(object sender, EventArgs e)
        {
            try
            {

                string filename = "";
                OpenFileDialog fdlg = new OpenFileDialog();
                fdlg.Title = "Select file";
                fdlg.InitialDirectory = @"c:\";
                //  fdlg.FileName = txtChooseFile.Text;
                //fdlg.Filter = "Excel Sheet(*.xls)|*.xls|All Files(*.*)|*.*";
                fdlg.FilterIndex = 1;
                fdlg.RestoreDirectory = true;
                if (fdlg.ShowDialog() == DialogResult.OK)
                {
                    filename = fdlg.FileName;
                    linkLabel6.Text = filename;
                    Application.DoEvents();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dtDOB_Leave(object sender, EventArgs e)
        {
            try
            {

                DateTime dob = dtDOB.Value;
                DateTime dtToday = DateTime.Now;
                TimeSpan diffResult = dtToday - dob;
                txtAgeinYears.Text = (diffResult.Days/365).ToString();
                if(Convert.ToInt32(txtAgeinYears.Text)<18)
                {
                    MessageBox.Show("Employee Age Must Be Above 18 Years");
                    dtDOB.Focus();
                    return;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        
        }

        private void dtpDOJ_Leave(object sender, EventArgs e)
        {
            try
            {

                DateTime dob = dtpDOJ.Value;
                DateTime dtToday = DateTime.Now;
                TimeSpan diffResult = dtToday - dob;
                txtSerinYears.Text = (diffResult.Days / 365).ToString();
               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void linkLabel6_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void linkLabel5_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }
}
