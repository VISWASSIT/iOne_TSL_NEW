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


namespace ioneNet.FinanceManagement.Masters
{
    public partial class AccountOBBillWise : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public decimal totAmt;
        public string vchno = "";
        public AccountOBBillWise()
        {
            InitializeComponent();
        }

        private void AccountOBBillWise_Load(object sender, EventArgs e)
        {
            try
            {

                string VchType = frmMain.frmname;
                
                if (VchType == "27")
                {
                    txtGRN_No.Text = FinanaceManagement.AccountVoucher.custname;
                    txtItem_Code.Text = FinanaceManagement.AccountVoucher.custid.ToString();
                    vchno = FinanaceManagement.AccountVoucher.vchno;
                }
                else
                {
                    txtGRN_No.Text = FinanaceManagement.Masters.Opening_Balances.LedgerName;
                    txtItem_Code.Text = FinanaceManagement.Masters.Opening_Balances.LedgerCode;
                    vchno = "OB";
                }
                //totAmt = FinanaceManagement.Masters.Opening_Balances.TotAmt;

                var dm1 = (from s in db.Account_Opening_BillWises
                           where s.Acc_ID == Convert.ToInt32(txtItem_Code.Text) && s.Company_ID == logIn.company && s.Vch_Ref_No ==vchno
                           select new
                           {
                               Bill_No = s.Bill_No,
                               Bill_Date = s.Bill_Date,
                               Amount = s.Bill_Amount,
                               Due_Date = s.Bill_Due_Date,

                           });
                SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataTable dtr = new DataTable();
                da2.Fill(dtr);
                if (dtr.Rows.Count >= 0)
                {
                    dgProductsList.DataSource = dtr;
                }
                decimal x = 0;
                for (int i = 0; i < dgProductsList.Rows.Count - 1; i++)
                {

                    x += (dgProductsList.Rows[i].Cells["Amount"].Value == "" || dgProductsList.Rows[i].Cells["Amount"].Value == null || dgProductsList.Rows[i].Cells["Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Amount"].Value);

                }
                textBox2.Text = x.ToString(".00");
                //txtQty.Text = MaterialManagement.Transactions.GoodsReceiptNote.RecQty;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgProductsList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow R1 = dgProductsList.Rows[dgProductsList.CurrentRow.Index];
                int columnIndex = dgProductsList.CurrentCell.ColumnIndex;
                string columnName = dgProductsList.Columns[columnIndex].Name;
                if(columnName == "Bill_Date")
                {
                    string inputString = R1.Cells["Bill_Date"].Value.ToString();
                    DateTime dDate;
                    if (DateTime.TryParse(inputString, out dDate))
                    {
                        //String.Format("{0:d/MM/yyyy}", dDate);

                    }
                    else
                    {
                        MessageBox.Show("Invalid Date Format Entered, Enter in DD/MM/YYYY Format"); // <-- Control flow goes here
                        return;
                    }
                }
                if (columnName == "Due_Date")
                {
                    string inputString = R1.Cells["Due_Date"].Value.ToString();
                    DateTime dDate;
                    if (DateTime.TryParse(inputString, out dDate))
                    {
                        //String.Format("{0:d/MM/yyyy}", dDate);

                    }
                    else
                    {
                        MessageBox.Show("Invalid Date Format Entered, Enter in DD/MM/YYYY Format"); // <-- Control flow goes here
                        return;
                    }
                }
                if (columnName == "Amount")
                {
                    decimal x = 0;
                    decimal r = 0;
                    for (int i = 0; i < dgProductsList.Rows.Count - 1; i++)
                    {

                        x += (dgProductsList.Rows[i].Cells["Amount"].Value == "" || dgProductsList.Rows[i].Cells["Amount"].Value == null || dgProductsList.Rows[i].Cells["Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Amount"].Value);
                        r = r + 1;

                    }

                    textBox2.Text = x.ToString(".00");
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

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                String myString = "";
                int Icode = Convert.ToInt32(txtItem_Code.Text);
               // myString = txtItem_Code.Text;
                //if (Convert.ToDecimal(textBox2.Text) >0 && ioneNet.FinanaceManagement.Masters.Opening_Balances.TotAmt != Convert.ToDecimal(textBox2.Text))
                //{
                //    MessageBox.Show("Bill Wise Amount Should Be Matched With Total Amount");
                //    return;
                //}
                //else
                //{
                    if ((from u in db.Account_Opening_BillWises where u.Acc_ID == Icode && u.Company_ID == logIn.company && u.Vch_Ref_No ==vchno select u).Count() > 0)
                    {
                        myString = txtGRN_No.Text;

                        db.sp_DeleteOpeningBalance_BillWise(Icode, logIn.company,vchno,logIn.BU_ID);
                    }
                    else
                    {

                        // myString = txtSoNo.Text;

                    }
                    //decimal y, x;
                   // y = Convert.ToDecimal(txtTotalQty.Text);



                    dgProductsList.Enabled = true;
                    //db.Transaction = transaction;
                    for (int i = 0; i < dgProductsList.RowCount - 1; i++)
                    {
                        Account_Opening_BillWise SC = new Account_Opening_BillWise();

                        SC.AccName = txtGRN_No.Text;
                        SC.Acc_ID = Convert.ToInt32(txtItem_Code.Text);
                    //    SC.TotalWt = Convert.ToDecimal(txtTotalQty.Text);
                        SC.Bill_No = (dgProductsList.Rows[i].Cells["Bill_No"].Value == null) ? "" : (dgProductsList.Rows[i].Cells["Bill_No"].Value).ToString();
                        SC.Bill_Date = Convert.ToDateTime(dgProductsList.Rows[i].Cells["Bill_Date"].Value);
                        //SC.Con_GSTINNo = (dgProductsList.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProductsList.Rows[i].Cells["Remarks"].Value).ToString();
                        SC.Bill_Amount = (dgProductsList.Rows[i].Cells["Amount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Amount"].Value);
                        SC.Bill_Due_Date = Convert.ToDateTime(dgProductsList.Rows[i].Cells["Due_Date"].Value);
                        //SC.Status = "Open";
                        SC.Vch_Ref_No = vchno;
                        SC.bu_id = logIn.BU_ID;
                        SC.Company_ID = logIn.company;
                        db.Account_Opening_BillWises.InsertOnSubmit(SC);
                        db.SubmitChanges();
                    }
                    db.SubmitChanges();
                    //transaction.Commit();               
                    MessageBox.Show("Bill Details Updated Sucessfully");                    
                    this.Close();
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        private void dgProductsList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F6) //Remove Rows
            {
                if (dgProductsList.Rows.Count > 0)
                {

                    foreach (DataGridViewCell oneCell in dgProductsList.SelectedCells)
                    {
                        if (oneCell.Selected)
                            dgProductsList.Rows.RemoveAt(oneCell.RowIndex);
                    }
                    
                    decimal x = 0;
                    decimal r = 0;
                    for (int i = 0; i < dgProductsList.Rows.Count - 1; i++)
                    {

                        x += (dgProductsList.Rows[i].Cells["Amount"].Value == "" || dgProductsList.Rows[i].Cells["Amount"].Value == null || dgProductsList.Rows[i].Cells["Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Amount"].Value);
                        r = r + 1;

                    }

                    textBox2.Text = x.ToString(".00");

                }
            }
        }
    }
}
