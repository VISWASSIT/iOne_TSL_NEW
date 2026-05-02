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
using System.Diagnostics;
using Ione_DAL;
using OpenCvSharp;
namespace ioneNet.HumanResourceManagement.Transactions
{
    public partial class frmDayAttendanceApprove : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public frmDayAttendanceApprove()
        {
            InitializeComponent();
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            
                        
            DateTime dt = dateTimePicker1.Value;
            string dt1 = dt.ToString("yyyy/MM/dd");

            SqlCommand cmd2 = new SqlCommand("SP_HR_Get_Employee_AttendanceReport", con);
            cmd2.CommandType = CommandType.StoredProcedure;
            cmd2.Parameters.AddWithValue("@compname", logIn.company);
            cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
            cmd2.Parameters.AddWithValue("@edate", dt1);
            // cmd2.Parameters.AddWithValue("@todate", dateTimePicker1.Value);


            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //DataSet ds2 = new DataSet();
            DataTable ds2 = new DataTable();
            // da2.Fill(ds2, "x");
            da2.Fill(ds2);
            dgvAttnData.DataSource = ds2;


            for (int i = 0; i < dgvAttnData.Rows.Count-1; i++)
            {
                DataGridViewRow R1 = dgvAttnData.Rows[dgvAttnData.CurrentRow.Index];
                int j = dgvAttnData.CurrentRow.Index;
                if (dgvAttnData.Rows[i].Cells["Attn_Status"].Value.ToString() == "No Punch")
                {
                    dgvAttnData.Rows[i].Cells["Attn_Status"].Style.ForeColor = Color.Red;
                    dgvAttnData.Rows[i].Cells["Attn_Status"].Style.Font = new Font("Bold", 10);
                    //  dg
                }
                if (dgvAttnData.Rows[i].Cells["Attn_Status"].Value.ToString() == "On_Leave")
                {
                    dgvAttnData.Rows[i].Cells["Attn_Status"].Style.ForeColor = Color.Blue;
                    dgvAttnData.Rows[i].Cells["Attn_Status"].Style.Font = new Font("Bold", 10);
                    //  dg
                }
                if (dgvAttnData.Rows[i].Cells["Attn_Status"].Value.ToString() == "Present")
                {
                    dgvAttnData.Rows[i].Cells["Attn_Status"].Style.ForeColor = Color.DarkGreen;
                    dgvAttnData.Rows[i].Cells["Attn_Status"].Style.Font = new Font("Bold", 10);
                    //  dg
                }
            }
        }

        private void dgvAttnData_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int columnIndex = dgvAttnData.CurrentCell.ColumnIndex;
                string columnName = dgvAttnData.Columns[columnIndex].HeaderText;
                TextBox tb3 = e.Control as TextBox;
                if (tb3 != null && columnName == "Reason for Miss Punch")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }
                if (tb3 != null && columnName == "Final Attn Status")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }

                


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void addItems(AutoCompleteStringCollection coll)
        {
            try
            {
                DataGridViewRow R1 = dgvAttnData.Rows[dgvAttnData.CurrentRow.Index];

                int columnIndex = dgvAttnData.CurrentCell.ColumnIndex;
                string columnName = dgvAttnData.Columns[columnIndex].HeaderText;
               

                if (columnName == "Reason for Miss Punch")
                {
                    var Prodname = (from d in db.Attributes_Datas where d.Head_Name == "Miss Punch Reason" select new { d.Descr }).ToList().Distinct();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Reason");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Descr);
                    }

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
                    }
                }

                if (columnName == "Final Attn Status")
                {
                    //var Prodname = (from d in db.Forging_MachineMasters where d.Machine_Type == "Forging" select new { d.Machine_Name }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Attn_Status");
                    dt.Rows.Add("Present");
                    dt.Rows.Add("LOP");
                    dt.Rows.Add("Leave");

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
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

        private void sfButton1_Click(object sender, EventArgs e)
        {
            try
            {
                 DateTime dt = dateTimePicker1.Value;
            string dt1 = dt.ToString("yyyy/MM/dd");

                if ((from u in db.HR_DayAttendanceApproveds where u.Company_id == logIn.company && u.BU_ID == logIn.BU_ID && u.E_Date == Convert.ToDateTime(dt1) select u).Count() > 0)
                {
                    db.sp_DeleteDay_Attendnace_Report(Convert.ToDateTime(dt1), logIn.company, logIn.BU_ID);

                }
                   

                    for (int i = 0; i < dgvAttnData.Rows.Count - 1; i++)
                    {

                        HR_DayAttendanceApproved p = new HR_DayAttendanceApproved();
                        p.Company_id = logIn.company;
                        p.BU_ID = logIn.BU_ID;

                        p.E_Date = Convert.ToDateTime(dateTimePicker1.Value);
                        p.BioMetric_Id = dgvAttnData.Rows[i].Cells["BioMetric_Id"].Value.ToString();
                        p.Emp_Code = dgvAttnData.Rows[i].Cells["Emp_Code"].Value.ToString();
                        p.Miss_Punch_Reason = dgvAttnData.Rows[i].Cells["Miss_Punch_Reason"].Value.ToString();
                        p.Final_Attn_Status = dgvAttnData.Rows[i].Cells["Final_Attn_Status"].Value.ToString();
                        p.Hrs_Worked = (dgvAttnData.Rows[i].Cells["Hrs_Worked"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvAttnData.Rows[i].Cells["Hrs_Worked"].Value);  
                        p.Approved_By = logIn.username + "-" + DateTime.Now;

                        db.HR_DayAttendanceApproveds.InsertOnSubmit(p);


                    }
                    db.SubmitChanges();
                    MessageBox.Show("Attendance Data Updated Successfully");
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
