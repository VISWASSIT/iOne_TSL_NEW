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
   
    public partial class frmBreakDownData : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmBreakDownData()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmBreakDownData_Load(object sender, EventArgs e)
        {
           // if (frmMain.frmname == "131")
            //{
                txtReportNo.Text = frm_Production_Report.ReportNo;
                txtOperation.Text = "Rolling";
            //}
           

            var ca = (from sq in db.Production_BreakdownDatas
                      where sq.Report_ID == txtReportNo.Text
                      select new
                      {

                          sq.Time_From,
                          sq.Time_To,
                          BD_Nature = sq.BreakDown_Nature                         
                      });
            SqlCommand cmd3 = (SqlCommand)db.GetCommand(ca);
            SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
            DataTable dt1 = new DataTable();
            da3.Fill(dt1);
            if (dt1.Rows.Count > 0)
                dgBDData.DataSource = dt1;

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {

            SqlCommand cmd1 = new SqlCommand("delete  from [Production_BreakdownData] where Report_ID =@RepID", con);
            cmd1.Parameters.AddWithValue("@RepID", txtReportNo.Text);
           // cmd1.Parameters.AddWithValue("@oprnName", txtOperation.Text);

            if (con.State != ConnectionState.Open)
                con.Open();
            //con.Open();
            cmd1.ExecuteNonQuery();
            con.Close();
            for (int i = 0; i < dgBDData.Rows.Count - 1; i++)
            {
                Production_BreakdownData SC = new Production_BreakdownData();
               // SC.Job_CardNo = (dgJobCardData.Rows[i].Cells["Job_Card"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Job_Card"].Value).ToString();
                SC.Report_ID = txtReportNo.Text;
                //SC.Machine_ID = (txtOperation.Text == null) ? "" : txtOperation.Text;
                //SC.Company_ID = logIn.company;
                //SC.Created_By = logIn.username + "-" + DateTime.Now;
                //SC.Modified_BY = logIn.username + "-" + DateTime.Now;
                SC.Time_From = (dgBDData.Rows[i].Cells["Time_From"].Value == null) ? "" : (dgBDData.Rows[i].Cells["Time_From"].Value).ToString();
                SC.Time_To = (dgBDData.Rows[i].Cells["Time_To"].Value == null) ? "" : (dgBDData.Rows[i].Cells["Time_To"].Value).ToString();
                SC.BreakDown_Nature = (dgBDData.Rows[i].Cells["BD_Nature"].Value == null) ? "" : (dgBDData.Rows[i].Cells["BD_Nature"].Value).ToString();


                db.Production_BreakdownDatas.InsertOnSubmit(SC);
            }
            db.SubmitChanges();
            MessageBox.Show("Breakdown Data Saved Successfully");
            this.Close();
        }
    }
}
