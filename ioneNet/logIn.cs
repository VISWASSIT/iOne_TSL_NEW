using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Ione_DAL;
namespace ioneNet
{
    public partial class logIn : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        public static int userID;
        public static int UserRoleID;
        public static int company;
        public static int BU_ID;
        public static string items,username,compname,BU_Name;
        public static DateTime fy_Start_Date, fy_End_Date;
        public static string BU_To_Show, F_Year;
        public logIn()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (txtUserName.Text.Trim() != "" && txtPassword.Text.Trim() != "")
                {
                    var d = (from c in db.User_Setups where c.User_Name == txtUserName.Text.Trim() && c.Password == txtPassword.Text.Trim() select c).Count();
                    if (d <= 0)
                    {
                        MessageBox.Show("Invalid Credentials");
                        txtPassword.Focus();

                    }
                    else
                    {
                        if (cmbFYear.Text == "")
                        {
                            MessageBox.Show("Select Financial Year  To Login");
                            cmbFYear.Focus();
                        }
                        else
                        if (cmbCompName.Text == "")
                        {
                            MessageBox.Show("Select Company Name  To Login");
                            cmbCompName.Focus();
                        }
                        else
                        {
                            company = Convert.ToInt32(cmbCompName.SelectedValue.ToString());
                            compname = cmbCompName.Text;
                            //Get Financial Year and Dates
                            var bindComp = (from m in db.Financial_Year_Masters
                                            where m.F_Year == cmbFYear.Text && m.Company_ID == company
                                            select new
                                            {
                                                m.Start_Date,
                                                m.End_Date,
                                                m.F_Year,
                                            }).ToList();

                            fy_Start_Date = Convert.ToDateTime(bindComp[0].Start_Date);
                            fy_End_Date = Convert.ToDateTime(bindComp[0].End_Date);
                            F_Year = bindComp[0].F_Year;
                            var bindBU = (from m in db.Costing_Units
                                            where m.BU_Name == cmbBU_Name.Text && m.Company == company
                                            select new
                                            {
                                                m.id,
                                                m.ToPrintName
                                               
                                            }).ToList();
                            BU_ID = bindBU[0].id;
                            BU_Name = cmbBU_Name.Text;
                            BU_To_Show = bindBU[0].ToPrintName;                            
                            frmMenuBoard frmMain = new frmMenuBoard();
                            this.Hide();
                            frmMain.ShowDialog();
                            this.Close();
                        }
                    }
                }

                else
                {
                    MessageBox.Show("Enter Credentials To Login");
                    txtUserName.Focus();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void txtPassword_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtUserName.Text.Trim() != "" && txtPassword.Text.Trim() != "")
                {
                    var d = (from c in db.User_Setups where c.User_Name == txtUserName.Text.Trim() && c.Password == txtPassword.Text.Trim() select c).Count();
                    if (d <= 0)
                    {
                        MessageBox.Show("please Enter Correct password");
                        txtPassword.Focus();

                    }
                    else
                    {
                        var bindComp = (from m in db.Users_Companies
                                        where m.User_ID == userID
                                        select new
                                        {
                                            m.Company_Name,
                                            m.Company_ID,
                                        }).ToList();

                        if (bindComp.Count > 0)
                        {
                            cmbCompName.DisplayMember = "Company_Name";
                            cmbCompName.ValueMember = "Company_ID";
                            cmbCompName.DataSource = bindComp;

                        }
                        if (cmbCompName.Items.Count == 1)
                            cmbCompName.SelectedIndex = 0;
                        else
                            cmbCompName.SelectedIndex = -1;


                        //Get Business Unit name
                        var bindBU = (from m in db.User_Setups
                                      where m.User_ID == userID
                                      select new
                                      {
                                          m.BU_Allowed,
                                          m.FY_Allowed
                                      }).ToList();

                        if (bindBU.Count > 0)
                        {
                            cmbBU_Name.Items.Clear();
                            string s = bindBU[0].BU_Allowed;
                            string[] values = s.Split(',');
                            for (int j = 0; j < values.Length; j++)
                            {
                                values[j] = values[j].Trim();
                                string m = values[j].ToString();
                                cmbBU_Name.Items.Add(m.ToString());
                            }
                            if (cmbBU_Name.Items.Count == 1)
                                cmbBU_Name.SelectedIndex = 0;
                            else
                                cmbBU_Name.SelectedIndex = -1;


                            cmbFYear.Items.Clear();
                            string s1 = bindBU[0].FY_Allowed;
                            string[] values1 = s1.Split(',');
                            for (int j = 0; j < values1.Length; j++)
                            {
                                values1[j] = values1[j].Trim();
                                string m = values1[j].ToString();
                                cmbFYear.Items.Add(m.ToString());
                            }
                            if (cmbFYear.Items.Count == 1)
                                cmbFYear.SelectedIndex = 0;
                            else
                                cmbFYear.SelectedIndex = -1;

                        }
                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbCompName_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void cmbBU_Name_Leave(object sender, EventArgs e)
        {
            //Financial Year
            if (cmbCompName.Text != "")
            {
                //var pStatus = (from m in db.Financial_Year_Masters where m.Company_ID == Convert.ToInt32(cmbCompName.SelectedValue.ToString()) select new { m.id, m.F_Year }).Distinct().ToList();
                //if (pStatus.Count > 0)
                //{
                //    cmbFYear.DataSource = pStatus;
                //    cmbFYear.ValueMember = "id";
                //    cmbFYear.DisplayMember = "F_Year";
                //}
            }
        }

        private void cmbCompName_Leave(object sender, EventArgs e)
        {
            //Status
            if (cmbCompName.Text != "")
            { 
                //var pStatus = (from m in db.Financial_Year_Masters where m.Company_ID == Convert.ToInt32(cmbCompName.SelectedValue.ToString()) select new { m.id, m.F_Year }).Distinct().ToList();
                //if (pStatus.Count > 0) 
                //{
                //    cmbFYear.DataSource = pStatus;
                //    cmbFYear.ValueMember = "id";
                //    cmbFYear.DisplayMember = "F_Year";
                //}
            }
        }

        private void txtUserName_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtUserName.Text.Trim() != "" )
                {
                    var d = (from c in db.User_Setups where c.User_Name == txtUserName.Text.Trim() select c).Count();
                    if (d <= 0)
                    {
                        MessageBox.Show("Invalid User Name");
                        txtUserName.Focus();

                    }
                    else
                    {
                        var user = (from m in db.User_Setups
                                    where m.User_Name == txtUserName.Text.Trim()
                                    select new
                                    {
                                        m.User_ID,
                                        m.User_Name,
                                        m.User_Role_ID,
                                        m.Company_ID,
                                    }).ToList();
                        userID = user[0].User_ID;
                        UserRoleID = Convert.ToInt32(user[0].User_Role_ID);
                        username = user[0].User_Name;
                        //frmMenuBoard frmMain = new frmMenuBoard();
                        //this.Hide();
                        //frmMain.ShowDialog();
                        //this.Close();
                    }
                }

                else
                {
                    MessageBox.Show("Enter Credentials To Login");
                    txtUserName.Focus();
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void logIn_Load(object sender, EventArgs e)
        {
            
        }
    }
}
