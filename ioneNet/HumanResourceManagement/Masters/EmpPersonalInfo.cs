using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ioneNet.HumanResourceManagement.Masters
{
    public partial class EmpPersonalInfo : Form
    {
       
        System.Data.Common.DbTransaction transaction;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["CONString"].ConnectionString);
        DataTable dt = new DataTable();
        SqlCommand cmd;
        SqlDataAdapter da;
        Timer MyTimer = new Timer();
        string mode = "";
        public EmpPersonalInfo()
        {
            InitializeComponent();
        }
        public void bindit()
        {
            try
            {
                txtEmpID.Text = HRMS.Employee.Empcode_get;
                txtlastName.Text = HRMS.Employee.Empname_get;
                if (txtEmpID.Text != "" && txtlastName.Text != "")
                {
                    var d = (from em in db.HRMS_EmployeeInfos
                             where em.Emp_ID == txtEmpID.Text && em.Emp_Name == txtlastName.Text
                             select new
                             {
                                 em.Emp_Group,
                                 em.Department,
                                 em.Emp_ID,
                                 em.Emp_FirstName,
                                 em.Emp_MiddleName,
                                 em.Emp_LastName,
                                 em.Emp_Name,
                            //     em.Role,
                                 em.Birth_Date,
                                 em.Age,
                                 em.Gender,
                                 em.B_Group,
                                 em.Parent_Spouse,
                                 em.M_Status,
                                 em.Religion,
                                 em.Address,
                                 em.City,
                                 em.State,
                                 em.Mobile,
                                 em.Photo,
                                 em.Mail,
                                 em.EmergencyNumber,
                                 em.ReferredBy,
                                 em.Qualification,
                                 em.Experience,
                                 em.Exp_Month,
                                 em.AadharNo,
                                 em.Vouter_Id,
                                 em.PAN_No,
                                 em.Passport_No,
                                 em.Passport_ExpiryDate,
                                 em.Status,
                             }).ToList();
                    if (d.Count > 0)
                    {
                        //cmbEmployeeGroup.Text = d[0].Emp_Group;
                        //cmbDepartment.Text = d[0].Department;
                        txtEmpID.Text = d[0].Emp_ID;
                        txtfirstname.Text = d[0].Emp_Name;
                        txtmidnaME.Text = d[0].Emp_MiddleName;
                        txtlastName.Text = d[0].Emp_LastName;
                        // txtlastName.Text = d[0].Emp_Name;
                    //    cmbRole.Text = d[0].Role;
                        dtpDOB.Value = Convert.ToDateTime(d[0].Birth_Date);
                        txtAge.Text = d[0].Age;
                        cmbGender.Text = d[0].Gender;
                        cmbBloodGroup.Text = d[0].B_Group;
                        txtParent.Text = d[0].Parent_Spouse;
                        cmbMarialStatus.Text = d[0].M_Status;
                        cmbReligion.Text = d[0].Religion;
                        txtAddress.Text = d[0].Address;
                        cmbCity.Text = d[0].City;
                        txtState.Text = d[0].State;
                        txtMobile.Text = d[0].Mobile;
                        if (d[0].Photo != null)
                        {
                            var img = (from s in db.HRMS_EmployeeInfos where s.Emp_ID == txtEmpID.Text select s);
                            SqlCommand cmd1 = (SqlCommand)db.GetCommand(img);
                            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                            DataTable dt = new DataTable();
                            DataSet ds = new DataSet("MyImages");
                            byte[] MyData = new byte[0];
                            da1.Fill(ds, "MyImages");
                            DataRow myRow;
                            myRow = ds.Tables["MyImages"].Rows[0];
                            MyData = (byte[])myRow["Photo"];
                            MemoryStream stream = new MemoryStream(MyData);
                            picEmpImage.Image = Image.FromStream(stream);
                            picEmpImage.SizeMode = PictureBoxSizeMode.StretchImage;
                        }
                        txtEMail.Text = d[0].Mail;
                        //txtEmergencyContact.Text = d[0].EmergencyNumber;
                        txtReference.Text = d[0].ReferredBy;
                        Cmbqualification.Text = d[0].Qualification;
                        CmbExperiance.Text = d[0].Experience;
                        //cmbExpMonth.Text = d[0].Exp_Month;
                        txtaadharno.Text = d[0].AadharNo;
                        //txtvouterid.Text = d[0].Vouter_Id;
                        //txtpanno.Text = d[0].PAN_No;
                        //txtpassportno.Text = d[0].Passport_No;
                        //dpexpirydate.Value = Convert.ToDateTime(d[0].Passport_ExpiryDate);
                        cmbStatus.Text = d[0].Status;

                    }

                }
            }
            catch(Exception ex)
            {

            }

        }

    
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnBrowseImage_Click(object sender, EventArgs e)
        {
          

            try
            {
                OpenFileDialog open = new OpenFileDialog();
                if (open.ShowDialog() == DialogResult.OK)
                {
                    Image img = new Bitmap(open.FileName);
                    picEmpImage.Image = img;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #region Methods

        public void save()
        {
            try
            {
                if ((from u in db.HRMS_EmployeeInfos where u.Emp_ID == txtEmpID.Text && u.Creation_Company == AppCode.GlobalAccesscs.companyName  select u).Count() > 0)
                {
                    try
                    {

                        if (AppCode.GlobalAccesscs.Edit == "Yes")
                        {
                            var c = db.HRMS_EmployeeInfos.Where(w => w.Emp_ID == txtEmpID.Text && w.Creation_Company == AppCode.GlobalAccesscs.companyName).FirstOrDefault();
                            db.HRMS_Sp_EmployeeMaste_Delete(txtEmpID.Text, AppCode.GlobalAccesscs.companyName);
                            HRMS_EmployeeInfo cq = new HRMS_EmployeeInfo();
                            cq.Emp_Group = (cmbEmployeeGroup.Text == "") ? "" : cmbEmployeeGroup.Text;
                            cq.Department = (cmbDepartment.Text == "") ? "" : cmbDepartment.Text;
                            cq.Emp_ID = txtEmpID.Text;
                            cq.Emp_FirstName = (txtfirstname.Text == "") ? "" : txtfirstname.Text;
                            cq.Emp_MiddleName = (txtmidnaME.Text == "") ? "" : txtmidnaME.Text;
                            cq.Emp_LastName = (txtlastName.Text == "") ? "" : txtlastName.Text;
                            cq.Emp_Name = txtfirstname.Text + " " + txtmidnaME.Text + " " + txtlastName.Text;
                            //  cq.Role= (cmbRole.Text == "") ? "" : cmbRole.Text;
                            cq.Birth_Date = Convert.ToDateTime(dtpDOB.Value);
                            cq.Age = txtAge.Text;
                            cq.Gender = (cmbGender.Text == "") ? "" : cmbGender.Text;
                            cq.B_Group = cmbBloodGroup.Text;
                            cq.Parent_Spouse = txtParent.Text;
                            cq.M_Status = (cmbMarialStatus.Text == "") ? "" : cmbMarialStatus.Text;
                            cq.Religion = (cmbReligion.Text == "") ? "" : cmbReligion.Text;
                            cq.Address = txtAddress.Text;
                            cq.City = (cmbCity.Text == "") ? "" : cmbCity.Text;
                            cq.State = txtState.Text;
                            cq.Mobile = txtMobile.Text;
                            if (picEmpImage.Image != null)
                            {
                                Image img = picEmpImage.Image;
                                System.IO.MemoryStream ms = new MemoryStream();
                                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                                byte[] bytes = ms.ToArray();
                                cq.Photo = bytes;
                            }
                            else
                            {
                                cq.Photo = null;
                            }
                            cq.Mail = txtEMail.Text;
                            cq.EmergencyNumber = txtEmergencyContact.Text;
                            cq.ReferredBy = txtReference.Text;
                            cq.Qualification = (Cmbqualification.Text == "") ? "" : Cmbqualification.Text;
                            cq.Experience = (CmbExperiance.Text == "") ? "" : CmbExperiance.Text;
                            cq.Exp_Month = (cmbExpMonth.Text == "") ? "" : cmbExpMonth.Text;
                            cq.AadharNo = txtaadharno.Text;
                            cq.Vouter_Id = txtvouterid.Text;
                            cq.PAN_No = txtpanno.Text;
                            cq.Passport_No = txtpassportno.Text;
                            cq.Passport_ExpiryDate = Convert.ToDateTime(dpexpirydate.Value).ToString();
                            cq.Status = (cmbStatus.Text == "") ? "" : cmbStatus.Text;
                            cq.Comments = (txtComment.Text == "") ? "" : txtComment.Text;
                            cq.Date_resign = Convert.ToDateTime(dpDateofResign.Value).ToString();
                            cq.Creation_Company = AppCode.GlobalAccesscs.companyName;
                            cq.Modified_by = AppCode.GlobalAccesscs.UserName;
                            cq.Modified_On = Convert.ToDateTime(DateTime.Now.ToString());
                            db.HRMS_EmployeeInfos.InsertOnSubmit(cq);
                            db.SubmitChanges();
                            MessageBox.Show("Record Updated Successfully");
                            clear();
                            BindQulification();
                        }
                        else
                        {
                            MessageBox.Show("Sorry You Dont Have Permissions to Modify This Records");
                        }
                       
                    }
                    catch(Exception ex)
                    {

                    }
                }

                else
                {
                    if (AppCode.GlobalAccesscs.Add == "Yes")
                    {
                        autoincreament();
                        db.Transaction = null;
                        if (null != db.Connection)
                        {
                            db.Connection.Close();
                        }
                        HRMS_EmployeeInfo EMP = new HRMS_EmployeeInfo();
                        EMP.Emp_Group = (cmbEmployeeGroup.Text == "") ? "" : cmbEmployeeGroup.Text;
                        EMP.Department = (cmbDepartment.Text == "") ? "" : cmbDepartment.Text;
                        EMP.Emp_ID = txtEmpID.Text;
                        EMP.Emp_FirstName= (txtfirstname.Text == "") ? "" : txtfirstname.Text;
                        EMP.Emp_MiddleName = (txtmidnaME.Text == "") ? "" : txtmidnaME.Text;
                        EMP.Emp_LastName = (txtlastName.Text == "") ? "" : txtlastName.Text;
                        EMP.Emp_Name = txtfirstname.Text +" "+ txtmidnaME.Text +" "+ txtlastName.Text;
                     //   EMP.Role = (cmbRole.Text == "") ? "" : cmbRole.Text;
                        EMP.Birth_Date = Convert.ToDateTime(dtpDOB.Value);
                        EMP.Age = txtAge.Text;
                        EMP.Gender = (cmbGender.Text == "") ? "" : cmbGender.Text;
                        EMP.B_Group = cmbBloodGroup.Text;
                        EMP.Parent_Spouse = txtParent.Text;
                        EMP.M_Status = (cmbMarialStatus.Text == "") ? "" : cmbMarialStatus.Text;
                        EMP.Religion = (cmbReligion.Text == "") ? "" : cmbReligion.Text;
                        EMP.Address = txtAddress.Text;
                        EMP.City = (cmbCity.Text == "") ? "" : cmbCity.Text;
                        EMP.State = txtState.Text;
                        EMP.Mobile = txtMobile.Text;
                        if (picEmpImage.Image != null)
                        {
                            Image img = picEmpImage.Image;
                            System.IO.MemoryStream ms = new MemoryStream();
                            img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                            byte[] bytes = ms.ToArray();
                            EMP.Photo = bytes;
                        }
                        else
                            EMP.Photo = null;
                        EMP.Mail = txtEMail.Text;
                        EMP.EmergencyNumber = txtEmergencyContact.Text;
                        EMP.ReferredBy = txtReference.Text;
                        EMP.Qualification = (Cmbqualification.Text == "") ? "" : Cmbqualification.Text;
                        EMP.Experience = (CmbExperiance.Text == "") ? "" : CmbExperiance.Text;
                        EMP.Exp_Month = (cmbExpMonth.Text == "") ? "" : cmbExpMonth.Text;
                        EMP.AadharNo = txtaadharno.Text;
                        EMP.Vouter_Id = txtvouterid.Text;
                        EMP.PAN_No = txtpanno.Text;
                        EMP.Passport_No = txtpassportno.Text;
                        EMP.Passport_ExpiryDate = Convert.ToDateTime(dpexpirydate.Value).ToString();
                        EMP.Status = (cmbStatus.Text == "") ? "" : cmbStatus.Text;
                        EMP.Comments = (txtComment.Text == "") ? "" : txtComment.Text;
                        EMP.Date_resign = Convert.ToDateTime(dpDateofResign.Value).ToString();
                        EMP.Created_by = AppCode.GlobalAccesscs.UserName;
                        EMP.Created_On = Convert.ToDateTime(DateTime.Now.ToString());
                        EMP.Creation_Company =AppCode.GlobalAccesscs.companyName;
                        db.HRMS_EmployeeInfos.InsertOnSubmit(EMP);
                        db.SubmitChanges();
                        MessageBox.Show("Record Saved Successfully");
                        clear();
                        BindQulification();
                    }
                    else
                    {
                        MessageBox.Show("Sorry You Dont Have Permissions to Save This Records");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void clear()
        {
            try
            {
                foreach (Control x in this.Controls)
                {
                    foreach (Control d in GroupBox1.Controls)
                    {
                        if (d is TextBox)
                            (d as TextBox).Clear();
                        if (d is ComboBox)
                            (d as ComboBox).SelectedIndex = -1;
                        if (d is PictureBox)
                            (d as PictureBox).Image = null;
                    }
                }
                CityLoad();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public void autoincreament()
        {
            try
            {
                var auto = db.HRMS_Sp_autoincrement_Employee_Info(AppCode.GlobalAccesscs.companyName);
                txtEmpID.Text = auto.FirstOrDefault().Emp_ID;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       public void delete()
        {
            //try
            //{
            //    if ((from u in db.EmployeeInfos where u.Emp_ID == txtEmpID.Text && u.CompName == AppCode.GlobalAccesscs.companyName select u).Count() > 0)
            //    {
            //        if (AppCode.GlobalAccesscs.Edit == "Yes")
            //        {

            //            DialogResult result = MessageBox.Show("Are You Sure Want to Delete this Record?", "Emp Personal Info", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
            //            if (result == DialogResult.OK)
            //            {
            //                db.sp_EmployeeMaster_Delete(txtEmpID.Text, AppCode.GlobalAccesscs.companyName);
            //                MessageBox.Show("Record Deleted Successfully ", "Emp Personal Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            //                clear();
            //            }
            //            else
            //            {
            //                Cursor.Current = Cursors.Default;
            //                MessageBox.Show("Record Not Deleted While Getting Error", "Emp Personal Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //            }

            //        }
            //        else
            //        {
            //            Cursor.Current = Cursors.Default;
            //            MessageBox.Show("You dont have privileges to Record this Record", "Material Issue", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //        }
            //    }
            //    else
            //    {
            //        Cursor.Current = Cursors.Default;
            //        MessageBox.Show("This Record Not Exising ,Please Select The Existing  Record", "Gate Pass", MessageBoxButtons.OK, MessageBoxIcon.Information);
            //    }


            //}
            //catch (Exception ex)
            //{
            //    Cursor.Current = Cursors.Default;
            //    // transaction.Rollback();
            //    MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

            //}

        }
        private void DepartmentLoad()
        {
            var c = (from u in  db.HRMS_Department_EmpInfos orderby u.Department_Name ascending select  u.Department_Name).ToList();
            cmbDepartment.DataSource = c;
            if (cmbDepartment.Items.Count > 0)
                cmbDepartment.SelectedIndex = -1;
        }
        private void CityLoad()
        {
            var c = (from u in db.HRMS_Cities select u.City).ToList();
            cmbCity.DataSource = c;
            if (cmbCity.Items.Count > 0)
                cmbCity.SelectedIndex = -1;
        }
        #endregion

        private void btnSubmit_Click(object sender, EventArgs e)
        {

            try
            {
                //if (cmbDepartment.SelectedIndex == -1)
                //{
                //    MessageBox.Show("Please Select Department");
                //    cmbDepartment.Focus();
                //    return;
                //}
               else if (txtfirstname.Text == "")
                {
                    MessageBox.Show("Please Enter First Name");
                    txtfirstname.Focus();
                    return;
                }
                //else if (txtlastName.Text == "")
                //{
                //    MessageBox.Show("Please Enter Last Name");
                //    txtlastName.Focus();
                //    return;
                //}
               
                //else if (txtlastName.Text == "")
                //{
                //    MessageBox.Show("Please Enter Employee Name");
                //    txtlastName.Focus();
                //    return;
                //}
                else if(cmbGender.SelectedIndex==-1)
                {
                    MessageBox.Show("Please Select Gender");
                    cmbGender.Focus();
                    return;
                }
                //else if (txtMobile.Text == "")
                //{
                //    MessageBox.Show("Please Enter Moblie Number");
                //    txtMobile.Focus();
                //    return;
                //}
                //else if (txtaadharno.Text == "")
                //{
                //    MessageBox.Show("Please Enter Aadhar Number");
                //    txtaadharno.Focus();
                //    return;
                //}
                else if (cmbStatus.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Select Status");
                    cmbStatus.Focus();
                    return;
                }
                else
                {
                    save();
                    clear();
                    this.Close();
                }
                
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                clear();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

       
        private void btndelete_Click(object sender, EventArgs e)
        {
            try
            {
                delete();
            }
            catch (Exception ex)
            {

            }
        }
        public void BindExperince()
        {
            try
            {
                var v = (from q in db.HRMS_EmployeeInfos where q.Creation_Company == AppCode.GlobalAccesscs.companyName select new { q.Experience }).Distinct().ToList();
                if(v.Count>0)
                {
                    CmbExperiance.DataSource = v;
                    CmbExperiance.DisplayMember = "Experience";
                    CmbExperiance.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {

            }
        }
       public void BindQulification()
        {
            try
            {
                var s = (from a in db.HRMS_EmployeeInfos where a.Creation_Company ==AppCode.GlobalAccesscs.companyName select new { a.Qualification }).Distinct().ToList();
                if (s.Count > 0)
                {
                    Cmbqualification.DataSource = s;
                    Cmbqualification.DisplayMember = "Qualification";
                    Cmbqualification.SelectedIndex = -1;
                }
            }
            catch (Exception ex)
            {

            }
          
        }
        public void BindBloodGroup()
        {
            var q = (from s in db.HRMS_EmployeeInfos
                    where s.Creation_Company == AppCode.GlobalAccesscs.companyName
                    select new
                    {
                        s.B_Group
                    }).Distinct().ToList();
            if(q.Count()>0)
            {
                cmbBloodGroup.DataSource = q;
                cmbBloodGroup.DisplayMember = "B_Group";
                cmbBloodGroup.SelectedIndex = -1;
            }
        }

       
        private void EmpPersonalInfo_Load(object sender, EventArgs e)
        {
            try
            {
               
              
                EmployeeBind();
                clear();
                CityLoad();
                DepartmentLoad();
                BindQulification();
                BindBloodGroup();
                BindExperince();
                string a = HRMS.Employee.var;
                if (a != "1")
                {
                    bindit();

                }
                else if (a == "1")
                {
                    autoincreament();
                }

              
                MyTimer.Interval = (10 * 60 * 1000 ); // 10 mins               
                MyTimer.Start();
                timerper.Start();
                lbltime.Text = DateTime.Now.ToLongTimeString();
                MyTimer.Tick += new EventHandler(MyTimer_Tick);
                

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void MyTimer_Tick(object sender, EventArgs e)
        {
            MessageBox.Show("Personal Information Time is Out..");
            timerper.Stop();
            MyTimer.Stop();
            this.Close();
          
            return;
            
        }

        public void EmployeeBind()
        {
            //var c = (from u in db.HRMS_Define_EmployeeGroups select u.Emp_Group).ToList();
            //cmbEmployeeGroup.DataSource = c;
            //if (cmbEmployeeGroup.Items.Count > 0)
            //    cmbEmployeeGroup.SelectedIndex = -1;
        }
        private void btnNewCity_Click_1(object sender, EventArgs e)
        {
            MASTERS.CityMaster obj = new MASTERS.CityMaster();
            if (obj.ShowDialog() == DialogResult.OK)
            {
                CityLoad();
                //FormNameLoad();
            }
            var d = (from s in db.HRMS_Cities where s.Creation_Company==AppCode.GlobalAccesscs.companyName select new { s.City,s.State}).ToList();
            if(d.Count>0)
            {
                cmbCity.DataSource = d;
                cmbCity.DisplayMember = "City";
                cmbCity.SelectedIndex = -1;
            }
        }
       
        private void dtpDOB_Leave_1(object sender, EventArgs e)
        {
            DateTime birth = Convert.ToDateTime(dtpDOB.Value);
            DateTime today = DateTime.Now;       //we usually don't care about birth time
            TimeSpan age = today - birth;        //.NET FCL should guarantee this as precise
            double ageInDays = age.TotalDays;    //total number of days ... also precise
            double daysInYear = 365.2425;        //statistical value for 400 years
            double ageInYears = ageInDays / daysInYear;  //can be shifted ... not so precise
            txtAge.Text = ageInYears.ToString("00");
        }

        private void cmbCity_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var da = (from obj in db.HRMS_Cities
                          where obj.City == cmbCity.Text
                          select new
                          {
                              obj.State
                          }).Distinct().ToList();
                if (da.Count > 0)
                {
                    if(cmbCity.SelectedIndex==-1)
                    {
                        txtState.Text = string.Empty;

                    }
                    else
                    {
                        txtState.Text = da[0].State;
                    }

                }
                else
                {                   
                    txtState.Text = null;
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void txtEMail_Validating(object sender, CancelEventArgs e)
        {
            System.Text.RegularExpressions.Regex rEmail = new System.Text.RegularExpressions.Regex(@"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$");

            if (txtEMail.Text.Length > 0 && txtEMail.Text.Trim().Length != 0)
            {
                if (!rEmail.IsMatch(txtEMail.Text.Trim()))
                {
                    MessageBox.Show("Invalid Email-ID");
                    txtEMail.SelectAll();
                    e.Cancel = true;
                }
            }
       
    }

        private void txtEmpName_Leave(object sender, EventArgs e)
        {
            try
            {
                //if (txtlastName.Text != "")
                //{
                //    if (txtlastName.Text.Length >= 3)
                //    {
                //        // txtCompanyPrefix.Focus();
                //    }
                //    else
                //    {
                //        MessageBox.Show("Last Name Should Be 3 Characters");
                //        txtlastName.Text = "";
                //        txtlastName.Focus();
                //    }
                //}
                //if (txtlastName.Text != "")
                //{
                //    if ((from u in db.HRMS_EmployeeInfos where u.Emp_LastName == txtlastName.Text select u).Count() > 0)
                //    {
                //        MessageBox.Show("This Employee Name Already Exist..., Please try Another One");
                //        txtlastName.Text = "";
                //        txtlastName.Focus();
                //        return;
                //    }
                //    else
                //    {

                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cmbStatus.SelectedIndex==2)
            {
                //lbComment.Visible = true;
                //lbDateofresign.Visible = true;
                //txtComment.Visible = true;
                //dpDateofResign.Visible = true;
               
            }
            else
            {
                //lbComment.Visible = false;
                //lbDateofresign.Visible = false;
                //txtComment.Visible = false;
                //dpDateofResign.Visible = false;
            }
        }

        private void cmbStatus_Leave(object sender, EventArgs e)
        {

        }

        private void txtComment_Leave(object sender, EventArgs e)
        {
            
        }

        private void dpDateofResign_Leave(object sender, EventArgs e)
        {
            //if (dpDateofResign.Value.ToString() == "")
            //{
            //    MessageBox.Show("Please Select Date!");
            //    dpDateofResign.Focus();
            //    return;
            //}
        }

        private void btnDepartment_Click(object sender, EventArgs e)
        {
            //HRMS.Department obj = new HRMS.Department();
            //if (obj.ShowDialog() == DialogResult.OK)
            //{
            //    DepartmentLoad();
            //    //FormNameLoad();
            //}
            //var d = (from s in db.HRMS_Department_EmpInfos where s.Creation_Company == AppCode.GlobalAccesscs.companyName select new { s.Department_Name }).ToList();
            //if (d.Count > 0)
            //{
            //    cmbDepartment.DataSource = d;
            //    cmbDepartment.DisplayMember = "Department_Name";
            //    cmbDepartment.SelectedIndex = -1;

            //}
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lbltime.Text = DateTime.Now.ToLongTimeString();
            timerper.Start();
          
        }

        private void Label4_Click(object sender, EventArgs e)
        {

        }
    }
}
