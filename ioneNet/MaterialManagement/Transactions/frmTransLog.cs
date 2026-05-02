using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using Ione_DAL;

namespace ioneNet.MaterialManagement.Transactions
{
    public partial class frmTransLog : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmTransLog()
        {
            InitializeComponent();
        }

        private void frmTransLog_Load(object sender, EventArgs e)
        {
            try
            {

                
                //var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Trans" && (m.Company_ID == logIn.company || m.Company_ID == 0) select new { m.ID, m.Descr }).Distinct().ToList();
                //if (pStatus.Count > 0)
                //{
                //    cmbStatus.DataSource = pStatus;
                //    cmbStatus.ValueMember = "ID";
                //    cmbStatus.DisplayMember = "Descr";
                //}
                txtusername.Text = logIn.username;

                if (MaterialManagement.Transactions.PurchaseRequisition.transno != null)
                {
                    txtTransName.Text = MaterialManagement.Transactions.PurchaseRequisition.transname;
                    txtRefDocNo.Text = MaterialManagement.Transactions.PurchaseRequisition.transno;

                    var dm1 = (from s in db.Transaction_Logs                               
                               where s.Trans_Name == txtTransName.Text && s.Trans_Doc_No == txtRefDocNo.Text &&  s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID

                               select new

                               {
                                   s.e_Date,
                                   s.e_User,
                                   s.Trans_Remarks,
                                   s.Trans_Status
                               });
                    SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    DataTable dtr = new DataTable();
                    da2.Fill(dtr);
                    if (dtr.Rows.Count >= 0)
                        dgProductsList.DataSource = dtr;
                }
                else
                if (MaterialManagement.PurchaseOrder.transno != null)
                {
                    
                    txtTransName.Text = MaterialManagement.PurchaseOrder.transname;
                    txtRefDocNo.Text = MaterialManagement.PurchaseOrder.transno;

                    var dm1 = (from s in db.Transaction_Logs
                               where s.Trans_Name == txtTransName.Text && s.Trans_Doc_No == txtRefDocNo.Text && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID

                               select new

                               {
                                   s.e_Date,
                                   s.e_User,
                                   s.Trans_Remarks,
                                   s.Trans_Status
                               });
                    SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    DataTable dtr = new DataTable();
                    da2.Fill(dtr);
                    if (dtr.Rows.Count >= 0)
                        dgProductsList.DataSource = dtr;
                }
                else
                if (MaterialManagement.Transactions.GoodsReceiptNote.transno != null)
                {
                    txtTransName.Text = MaterialManagement.Transactions.GoodsReceiptNote.transname;
                    txtRefDocNo.Text = MaterialManagement.Transactions.GoodsReceiptNote.transno;

                    var dm1 = (from s in db.Transaction_Logs
                               where s.Trans_Name == txtTransName.Text && s.Trans_Doc_No == txtRefDocNo.Text && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID

                               select new

                               {
                                   s.e_Date,
                                   s.e_User,
                                   s.Trans_Remarks,
                                   s.Trans_Status
                               });
                    SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    DataTable dtr = new DataTable();
                    da2.Fill(dtr);
                    if (dtr.Rows.Count >= 0)
                        dgProductsList.DataSource = dtr;
                }
                //Status
                var pStatus = (from m in db.Attributes_Datas
                               join r in db.view_Trans_Auth_Levels
                               on m.ID equals r.Status_Code
                               where r.Menu_Item == txtTransName.Text && (r.Company_ID == logIn.company) && r.Role_ID == logIn.UserRoleID
                               select new { m.ID, m.Descr }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbStatus.DataSource = pStatus;
                    cmbStatus.ValueMember = "ID";
                    cmbStatus.DisplayMember = "Descr";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dtexisting = new DataTable();
            if (dgProductsList.Rows.Count > 1)
            {
                dtexisting.Rows.Clear();
                dtexisting.Columns.Clear();
                dtexisting.Columns.Add("e_Date", typeof(string));
                dtexisting.Columns.Add("e_User", typeof(string));
                dtexisting.Columns.Add("Trans_Remarks", typeof(string));
                dtexisting.Columns.Add("Trans_Status", typeof(string));

                for (int i = 0; i < dgProductsList.Rows.Count - 1; i++)
                {
                    DataRow dr;
                    dr = dtexisting.NewRow();
                    dr["e_Date"] = dgProductsList.Rows[i].Cells["e_Date"].Value.ToString();
                    dr["e_User"] = dgProductsList.Rows[i].Cells["e_User"].Value.ToString();
                    dr["Trans_Remarks"] = dgProductsList.Rows[i].Cells["Trans_Remarks"].Value.ToString();
                    dr["Trans_Status"] = dgProductsList.Rows[i].Cells["Trans_Status"].Value.ToString();

                    dtexisting.Rows.Add(dr);

                }
                dtexisting.AcceptChanges();
            }

            if (cmbStatus.Text != "")
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("e_Date", typeof(string));
                dt.Columns.Add("e_User", typeof(string));
                dt.Columns.Add("Trans_Remarks", typeof(string));
                dt.Columns.Add("Trans_Status", typeof(string));                

                dt.Rows.Add(DateTime.Now, txtusername.Text, txtTransRemarks.Text, cmbStatus.Text);

            

            dtexisting = dtexisting.AsEnumerable().Union(dt.AsEnumerable()).CopyToDataTable();
            }
            dgProductsList.DataSource = dtexisting;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {

            
            SqlCommand cmd1 = new SqlCommand("delete  from [Transaction_Log] where Trans_Name =@Trans_Name and Trans_Doc_No = @Trans_Doc_No", con);
            cmd1.Parameters.AddWithValue("@Trans_Name", txtTransName.Text);
            cmd1.Parameters.AddWithValue("@Trans_Doc_No", txtRefDocNo.Text);
            if (con.State != ConnectionState.Open)
                con.Open();
            //con.Open();
            cmd1.ExecuteNonQuery();

            for (int i = 0; i < dgProductsList.RowCount - 1; i++)
            {
                Transaction_Log SC = new Transaction_Log();
               
              
                SC.Trans_Name = txtTransName.Text;
                SC.Trans_Doc_No = txtRefDocNo.Text;
                SC.e_Date = (dgProductsList.Rows[i].Cells["e_Date"].Value == null) ? DateTime.Now : Convert.ToDateTime((dgProductsList.Rows[i].Cells["e_Date"].Value).ToString());
                SC.e_User = (dgProductsList.Rows[i].Cells["e_User"].Value == null) ? "" : (dgProductsList.Rows[i].Cells["e_User"].Value).ToString();
                SC.Trans_Remarks = (dgProductsList.Rows[i].Cells["Trans_Remarks"].Value == null) ? "" : (dgProductsList.Rows[i].Cells["Trans_Remarks"].Value).ToString();
                SC.Trans_Status = (dgProductsList.Rows[i].Cells["Trans_Status"].Value == null) ? "" : (dgProductsList.Rows[i].Cells["Trans_Status"].Value).ToString();

                SC.Company_ID = logIn.company;
                SC.BU_ID = logIn.BU_ID;
                db.Transaction_Logs.InsertOnSubmit(SC);
            }
            db.SubmitChanges();
        }
    }
    
}
