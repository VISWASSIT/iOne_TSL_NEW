using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ioneNet.FinanceManagement.Transactions
{
    using System.Configuration;
    using System.Data.SqlClient;
    using Excel = Microsoft.Office.Interop.Excel;
    using System.Diagnostics;
    using System.IO;
    using Syncfusion.Windows.Forms.Grid;
    using Ione_DAL;

    public partial class BankReconsilationStatement : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public BankReconsilationStatement()
        {
            InitializeComponent();
        }

        private void BankReconsilationStatement_Load(object sender, EventArgs e)
        {
            dpFromDate.MinDate = logIn.fy_Start_Date;
            dpTodate.MaxDate = logIn.fy_End_Date;
            var d = (from po in db.AccountMasters
                     join A in db.AccountGroups on po.AccGroup_ID equals A.ID
                     where po.Company_ID == logIn.company && A.GroupType == "Asset" || A.GroupType =="Liability"
                     select new { po.id, po.AccName }).Distinct().ToList();
            if (d.Count > 0)
            {
                cmbAccName.DataSource = d;
                cmbAccName.ValueMember = "id";
                cmbAccName.DisplayMember = "AccName";
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                 binddata();
                DateTime t = dpTodate.Value;
                string t1 = t.ToString("dd/MMM/yyyy");
                var getBal = (from b in db.GetAccountBalance(logIn.company, t, Convert.ToInt32(cmbAccName.SelectedValue.ToString()), 1,logIn.BU_ID)
                              select new { b.Balance, b.BalType }).FirstOrDefault();
                if (getBal != null)
                {
                   txtBookBalance.Text = getBal.Balance.ToString();
                   txtBookBalType.Text = getBal.BalType.ToString();
                }
                else
                {
                    txtBookBalance.Text = "0";
                }
                var getBankBal = (from b in db.GetAccountBalance(logIn.company, t, Convert.ToInt32(cmbAccName.SelectedValue.ToString()), 2,logIn.BU_ID)
                              select new { b.Balance, b.BalType }).FirstOrDefault();
                if (getBankBal != null)
                {
                    txtBankBal.Text = getBankBal.Balance.ToString();
                    txtBankBalType.Text = getBankBal.BalType.ToString();
                }
                else
                {
                    txtBankBal.Text = "0";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void binddata()
        {
            try
            {
                DateTime dt = dpFromDate.Value;
                string dt1 = dt.ToString("yyyy/MM/dd");

                DateTime dtt = dpTodate.Value;
                string dt2 = dtt.ToString("yyyy/MM/dd");

                //var data = db.Sp_ProductionTunnageReport(AppCode.GlobalAccess.companyName, Convert.ToDateTime(dt1), Convert.ToDateTime(dt2)).ToString();


                SqlCommand cmd2 = new SqlCommand("sp_BankReconsilation", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);
                cmd2.Parameters.AddWithValue("@accName", Convert.ToInt32(cmbAccName.SelectedValue.ToString()));
                cmd2.Parameters.AddWithValue("@Fromdate", Convert.ToDateTime(dt1));
                cmd2.Parameters.AddWithValue("@Todate", Convert.ToDateTime(dt2));
                if (checkBox1.Checked == true)
                {
                    cmd2.Parameters.AddWithValue("@para", 2);
                }
                else
                {
                    cmd2.Parameters.AddWithValue("@para", 1);
                }
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataSet ds2 = new DataSet();
                DataTable ds2 = new DataTable();
                // da2.Fill(ds2, "x");
                da2.Fill(ds2);
                dataGridView1.DataSource = ds2;
                double OPQty = 0, RecQty = 0, IssQty = 0;
                decimal rec, iss, op, closing = 0;
               
               

                

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

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                {
                    //BankReconsilationReport S = new BankReconsilationReport();
                    //{
                        if (dataGridView1.Rows[i].Cells["Bank_Date"].Value.ToString() != "")

                        {
                        SqlCommand cmd = new SqlCommand();
                        cmd.CommandText = "Update Account_Voucher set BankReconsiled = '1' where [Voucher_No]=@param1 and Company_ID =@compName";
                        cmd.Parameters.AddWithValue("@param1", dataGridView1.Rows[i].Cells["Voucher_No"].Value);
                        cmd.Parameters.AddWithValue("@CompName", logIn.company);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        cmd.Parameters.Clear();
                        DateTime strT = Convert.ToDateTime(dataGridView1.Rows[i].Cells["Bank_Date"].Value);
                        cmd.CommandText = "Update Account_Voucher set Bank_Date = @strT where [Voucher_No]=@param1 and Company_ID =@compName";
                        cmd.Parameters.AddWithValue("@strT", strT);
                        cmd.Parameters.AddWithValue("@param1", dataGridView1.Rows[i].Cells["Voucher_No"].Value);
                        cmd.Parameters.AddWithValue("@CompName", logIn.company);
                        cmd.Connection = con;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        con.Close();
                        //S.Voucher_No = (dataGridView1.Rows[i].Cells["Voucher_No"].Value == null) ? "" : (dataGridView1.Rows[i].Cells["Voucher_No"].Value).ToString();
                        //S.AccName = (dataGridView1.Rows[i].Cells["Account_Name"].Value == null) ? "" : (dataGridView1.Rows[i].Cells["Account_Name"].Value).ToString();
                        //S.Transaction_Ref_No = (dataGridView1.Rows[i].Cells["RefNo"].Value == null) ? "" : (dataGridView1.Rows[i].Cells["RefNo"].Value).ToString();
                        //S.Transaction_Ref_Date = Convert.ToDateTime(dataGridView1.Rows[i].Cells["Inst_Date"].Value);
                        //S.Acc_ID = Convert.ToInt32(dataGridView1.Rows[i].Cells["Acc_ID"].Value);
                        //S.Bank_Date = Convert.ToDateTime(dataGridView1.Rows[i].Cells["Bank_Date"].Value);
                        //S.Company_ID = logIn.company;
                        //S.Created_By = logIn.username + "-" + DateTime.Now;
                        //S.Modified_BY = logIn.username + "-" + DateTime.Now;
                        //db.BankReconsilationReports.InsertOnSubmit(S);
                        //db.SubmitChanges();
                    }
                    //}
                }
                db.SubmitChanges();
                MessageBox.Show("Data Updated Successfully");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
