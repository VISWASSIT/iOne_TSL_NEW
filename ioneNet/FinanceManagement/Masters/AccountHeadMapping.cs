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
namespace ioneNet.OrderManagement.Transactions
{
    public partial class AccountHeadMapping : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public AccountHeadMapping()
        {
            InitializeComponent();
        }

        private void dgDetails_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int columnIndex = dgDetails.CurrentCell.ColumnIndex;
                string columnName = dgDetails.Columns[columnIndex].HeaderText;
                TextBox tb3 = e.Control as TextBox;
                if (tb3 != null && columnName == "Account Ledger")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }
                if (tb3 != null && columnName == "Voucher Head")
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
                DataGridViewRow R1 = dgDetails.Rows[dgDetails.CurrentRow.Index];

                int columnIndex = dgDetails.CurrentCell.ColumnIndex;
                string columnName = dgDetails.Columns[columnIndex].HeaderText;

                if (columnName == "Account Ledger")
                {
                    var Prodname = (from d in db.AccountMasters where d.Company_ID == logIn.company select new { d.AccName }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("AccName");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.AccName);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
                    }
                }
                if (columnName == "Voucher Head")
                {
                    var Prodname = (from d in db.Attributes_Datas where d.Head_Name == "Vch_Head" select new { d.Descr }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Descr");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Descr);
                    }
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

        private void dgDetails_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow R1 = dgDetails.Rows[dgDetails.CurrentRow.Index];
            int columnIndex = dgDetails.CurrentCell.ColumnIndex;
            int RowIndex = dgDetails.CurrentCell.RowIndex;
            string columnName = dgDetails.Columns[columnIndex].Name;
            if (columnName == "AccName")
            {
                var d1 = (from a in db.AccountMasters where a.AccName == R1.Cells["AccName"].Value && a.Company_ID == logIn.company select new { a.id }).ToList();
                if (d1.Count > 0)
                {
                   
                    R1.Cells["AccCode"].Value = d1[0].id;
                }
            }            

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SO_MultiDeliveryAddress_Load(object sender, EventArgs e)
        {
            try
            {
                
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

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                db.Transaction = null;
                String myString = "";

                myString = cmbVoucher.Text;
                if ((from u in db.Voucher_LedgerMappings where u.VoucherType == myString && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    myString = cmbVoucher.Text;
                    db.Connection.Open();
                    System.Data.Common.DbTransaction transaction;
                    transaction = db.Connection.BeginTransaction();
                    db.Transaction = transaction;
                    var ci = db.Voucher_LedgerMappings.Where(w => w.VoucherType == cmbVoucher.Text && w.Company_ID == logIn.company).FirstOrDefault();

                    for (int i = 0; i < dgDetails.RowCount - 1; i++)
                    {

                        ci.AccName = (dgDetails.Rows[i].Cells["AccName"].Value == null) ? "" : (dgDetails.Rows[i].Cells["AccName"].Value).ToString();
                        //.Tot_Debit = (txtTotDebit.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotDebit.Text);
                        //S.Debit_Amount = (dgRecipts.Rows[i].Cells["Debit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Debit_Amount"].Value);


                        ci.AccCode = (dgDetails.Rows[i].Cells["Acccode"].Value == DBNull.Value) ? Convert.ToInt32("0") : Convert.ToInt32(dgDetails.Rows[i].Cells["Acccode"].Value);

                    }
                    db.Transaction = transaction;
                    transaction.Commit();
                    db.SubmitChanges();
                    //transaction.Commit();               
                    MessageBox.Show("Record Updated Sucessfully");
                    db.Connection.Close();
                }
                else
                {
                  
                    myString = cmbVoucher.Text;

                }
                //if (frmGate.Modify.Contains(this.Text))
                //{

                // dgProducts.Enabled = false;
                //var S = db.Sale_Order_Masters.Where(w => w.SO_NO == myString && w.Company_ID == logIn.company).FirstOrDefault();
                for (int i = 0; i < dgDetails.RowCount - 1; i++)
                {
                    Voucher_LedgerMapping S = new Voucher_LedgerMapping();
                    {
                        S.VoucherType = cmbVoucher.Text;
                        S.AccName = (dgDetails.Rows[i].Cells["AccName"].Value == null) ? "" : (dgDetails.Rows[i].Cells["AccName"].Value).ToString();
                        //.Tot_Debit = (txtTotDebit.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotDebit.Text);
                        //S.Debit_Amount = (dgRecipts.Rows[i].Cells["Debit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Debit_Amount"].Value);
                        S.Head  = (dgDetails.Rows[i].Cells["Head"].Value == null) ? "" : (dgDetails.Rows[i].Cells["Head"].Value).ToString();

                        S.AccCode = (dgDetails.Rows[i].Cells["Acccode"].Value == DBNull.Value) ? Convert.ToInt32("0") : Convert.ToInt32(dgDetails.Rows[i].Cells["Acccode"].Value);
                        S.Company_ID = logIn.company;
                        db.Voucher_LedgerMappings.InsertOnSubmit(S);
                        db.SubmitChanges();

                    }
                }
                                  
                    
                //transaction.Commit();               
                MessageBox.Show("Record Updated Sucessfully");
                db.Connection.Close();
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

        private void cmbVoucher_Leave(object sender, EventArgs e)
        {
            try
            {
                var dm1 = (from s in db.Voucher_LedgerMappings
                           where s.VoucherType == cmbVoucher.Text && s.Company_ID == logIn.company


                           select new

                           {
                               s.Head,
                               s.AccName,
                               s.AccCode,                             
                              
                           });




                SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataTable dtr = new DataTable();
                da2.Fill(dtr);
                if (dtr.Rows.Count >= 0)
                    dgDetails.DataSource = dtr;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgDetails_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cmbVoucher_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}
