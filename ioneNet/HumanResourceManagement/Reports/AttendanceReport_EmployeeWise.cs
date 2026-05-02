using Ione_DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ioneNet.HumanResourceManagement.Reports
{
    public partial class AttendanceReport_EmployeeWise : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public AttendanceReport_EmployeeWise()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {

            DateTime dt = dateTimePicker1.Value;
            string dt1 = dt.ToString("yyyy/MM/dd");

            DateTime dt2 = dateTimePicker2.Value;
            string dt3 = dt2.ToString("yyyy/MM/dd");

            SqlCommand cmd2 = new SqlCommand("SP_HR_Get_AttendanceReport_EmployeeWise", con);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@compname", logIn.company);
            cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
            cmd2.Parameters.AddWithValue("@frmDate", dt1);
            cmd2.Parameters.AddWithValue("@toDate", dt3);
            cmd2.Parameters.AddWithValue("@empid", txtEmpCode.Text);


            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //DataSet ds2 = new DataSet();
            DataTable ds2 = new DataTable();
            // da2.Fill(ds2, "x");
            da2.Fill(ds2);
            dgvAttnData.DataSource = ds2;


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtEmpCode_Leave(object sender, EventArgs e)
        {
            var p1 = (from so in db.HR_Employee_Master_Datas
                      where so.Emp_Code == txtEmpCode.Text
                      select new
                      {
                          so.Emp_Name

                      }).ToList();

            textBox2.Text = p1[0].Emp_Name;
        }
    }
}
