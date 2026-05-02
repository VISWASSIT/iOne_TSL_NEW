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
using System.Diagnostics;

namespace ioneNet.FinanceManagement
{
    public partial class GetBillDetails : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();
        public static decimal totAmt;
        
        public GetBillDetails()
        {
            InitializeComponent();
        }

        private void GetBillDetails_Load(object sender, EventArgs e)
        {

            int cId = ioneNet.FinanaceManagement.AccountVoucher.custid;
            txtCustomer.Text = ioneNet.FinanaceManagement.AccountVoucher.custid.ToString();
            txtVchNo.Text = ioneNet.FinanaceManagement.AccountVoucher.vchno;

            if ((from u in db.Cust_Received_Bills where u.Voucher_No == txtVchNo.Text && u.Customer_ID == cId && u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID select u).Count() > 0)
            {
                // var d = (from u in db.Cust_Received_Bills where u.Voucher_No == txtVchNo.Text && u.Customer_ID == cId && u.Company_ID == logIn.company) select data).ToList();
                var d = (from data in db.GetReceivedBills (logIn.company, Convert.ToInt32(txtCustomer.Text), txtVchNo.Text) select data).ToList();

                if (d.Count >= 0)
                {
                    dgProductsList.DataSource = d;
                }
                else
                {

                }
               
            }
            else
            {

                var d = (from data in db.GetPendingBills(logIn.company, Convert.ToInt32(txtCustomer.Text), txtVchNo.Text,1,logIn.BU_ID) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    dgProductsList.DataSource = d;
                }
                //this.dgProductsList.Rows.Add();//("five", "six", "seven", "eight");
            }
            decimal TotAmt = 0;
            for (int i = 0; i < dgProductsList.Rows.Count; i++)
            {


                TotAmt += //((dgRecipts.Rows[i].Cells["Debit"].Value == DBNull.Value) && (dgSelectedocument.Rows[i].Cells["Debit"].Value == "") && (dgSelectedocument.Rows[i].Cells["Debit"].Value == null)) ? Convert.ToDecimal("0.00") : Convert.ToDecimal(dgSelectedocument.Rows[i].Cells["Debit"].Value);//
                    (dgProductsList.Rows[i].Cells["Amt_Received"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Amt_Received"].Value);



            }
            txtTotAmount.Text = TotAmt.ToString(".00");







            //Check for any already seleted bills
            //if (dtgetfinalprducts.Rows.Count > 0)
            //{
            //    if (txtVchNo.Text == ioneNet.FinanaceManagement.AccountVoucher.vchno && txtCustomer.Text == ioneNet.FinanaceManagement.AccountVoucher.custid.ToString())
            //    {
            //        dgProductsList.DataSource = dtgetfinalprducts;
            //    }
            //    else
            //    {

            //if ((from u in db.Cust_Received_Bills where u.Voucher_No == txtVchNo.Text && u.Customer_ID == cId && u.Company_ID == logIn.company select u).Count() > 0)
            //        {
            //            var dm1 = (from s in db.Cust_Received_Bills
            //                       where s.Customer_ID == cId && s.Voucher_No == txtVchNo.Text && s.Company_ID == logIn.company
            //                       select new
            //                       {
            //                           Inv_No = s.Bill_No,
            //                           InvDate = s.Bill_Date,
            //                           Tot_Inv_Value = s.Bill_Value,
            //                           BalanceAmount = s.Due_Amount,
            //                           Amt_Received =s.AmtReceived

            //                       });
            //            SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
            //            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //            DataTable dtr = new DataTable();
            //            da2.Fill(dtr);
            //            if (dtr.Rows.Count >= 0)
            //            {
            //                dgProductsList.DataSource = dtr;
            //            }
            //        }
            //        else
            //        {
            //            var dm1 = (from s in db.Customer_Bills_Views
            //                       where s.Acc_ID == cId && s.Company_ID == logIn.company && s.AmtBalance>0
            //                       select new
            //                       {
            //                           Inv_No = s.Bill_No,
            //                           InvDate = s.Bill_Date,
            //                           Tot_Inv_Value = s.Bill_Amount,
            //                           BalanceAmount = s.AmtBalance

            //                       });
            //            SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
            //            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //            DataTable dtr = new DataTable();
            //            da2.Fill(dtr);
            //            if (dtr.Rows.Count >= 0)
            //            {
            //                dgProductsList.DataSource = dtr;
            //            }
            //        }
            ////    }
            //}
            //else
            //if ((from u in db.Cust_Received_Bills where u.Voucher_No == txtVchNo.Text && u.Customer_ID == cId && u.Company_ID == logIn.company select u).Count() > 0)
            //{
            //    var dm1 = (from s in db.Cust_Received_Bills
            //               where s.Customer_ID == cId && s.Voucher_No == txtVchNo.Text && s.Company_ID == logIn.company
            //               select new
            //               {
            //                   Inv_No = s.Bill_No,
            //                   InvDate = s.Bill_Date,
            //                   Tot_Inv_Value = s.Bill_Value,
            //                   BalanceAmount = s.Due_Amount,
            //                   Amt_Received = s.AmtReceived

            //               });
            //    SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
            //    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //    DataTable dtr = new DataTable();
            //    da2.Fill(dtr);
            //    if (dtr.Rows.Count >= 0)
            //    {
            //        dgProductsList.DataSource = dtr;
            //    }
            //}
            //else
            //{
            //    txtCustomer.Text = ioneNet.FinanaceManagement.AccountVoucher.custid.ToString();
            //    txtVchNo.Text = ioneNet.FinanaceManagement.AccountVoucher.vchno;
            //    var dm1 = (from s in db.Customer_Bills_Views
            //               where s.Acc_ID == cId && s.Company_ID == logIn.company
            //               select new
            //               {
            //                   Inv_No = s.Bill_No,
            //                   InvDate = s.Bill_Date,
            //                   Tot_Inv_Value = s.Bill_Amount,
            //                   BalanceAmount = s.AmtBalance

            //               });
            //    SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
            //    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //    DataTable dtr = new DataTable();
            //    da2.Fill(dtr);
            //    if (dtr.Rows.Count >= 0)
            //    {
            //        dgProductsList.DataSource = dtr;
            //    }
            //}


        }

        private void dgProductsList_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow R1 = dgProductsList.Rows[dgProductsList.CurrentRow.Index];
                int columnIndex = dgProductsList.CurrentCell.ColumnIndex;
                string columnName = dgProductsList.Columns[columnIndex].Name;
                if (columnName == "Inv_No")
                {

                    //var d = (from data in db.GetPendingBills(logIn.company, Convert.ToInt32(txtCustomer.Text), txtVchNo.Text) select data).ToList();

                    var billDetails = (from s in db.Customer_Bills_Views
                                       where s.Acc_ID == Convert.ToInt32(txtCustomer.Text) && s.Company_ID == logIn.company && s.bu_id == logIn.BU_ID && s.Bill_No == R1.Cells["Inv_No"].Value.ToString()
                                       select new { s.Bill_Date, s.Bill_Amount, s.AmtBalance }).FirstOrDefault();

                    if (billDetails != null)
                    {

                        R1.Cells["InvDate"].Value = billDetails.Bill_Date.ToString();
                        R1.Cells["Tot_Inv_Value"].Value = billDetails.Bill_Amount.ToString();
                        R1.Cells["BalanceAmount"].Value = billDetails.AmtBalance.ToString();
                    }
                }

                if (columnName == "Amt_Received")
                {
                    decimal AmtReceived = (R1.Cells["Amt_Received"].Value == "" || R1.Cells["Amt_Received"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Amt_Received"].Value);
                    decimal DueAmt = (R1.Cells["BalanceAmount"].Value == "" || R1.Cells["BalanceAmount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["BalanceAmount"].Value);

                    if (AmtReceived > DueAmt)
                    {
                        MessageBox.Show("Amount Received Cannot Be Greater Than Due Amount");
                        R1.Cells["Amt_Received"].Value = "";
                        dgProductsList.Focus();
                        return;

                    }
                    else
                    {
                        decimal TotAmt = 0;
                        for (int i = 0; i < dgProductsList.Rows.Count; i++)
                        {


                            TotAmt += //((dgRecipts.Rows[i].Cells["Debit"].Value == DBNull.Value) && (dgSelectedocument.Rows[i].Cells["Debit"].Value == "") && (dgSelectedocument.Rows[i].Cells["Debit"].Value == null)) ? Convert.ToDecimal("0.00") : Convert.ToDecimal(dgSelectedocument.Rows[i].Cells["Debit"].Value);//
                                (dgProductsList.Rows[i].Cells["Amt_Received"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Amt_Received"].Value);



                        }
                        txtTotAmount.Text = TotAmt.ToString(".00");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void btnSelect_Click(object sender, EventArgs e)
        {
            try
            {
                //DataRow drgetproducts;
                String myString = "";
                int Ccode = Convert.ToInt32(txtCustomer.Text);
                myString = txtVchNo.Text;
                if ((from u in db.Cust_Received_Bills  where u.Voucher_No == myString && u.Customer_ID == Ccode && u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID select u).Count() > 0)
                {
                    
                    db.sp_AccountVoucher_Bill_Delete(myString, Ccode, logIn.BU_ID);
                }
                else
                {

                    // myString = txtSoNo.Text;

                }
                for (int i = 0; i < dgProductsList.Rows.Count ; i++)
                {
                                     
                        Cust_Received_Bill CB = new Cust_Received_Bill();
                        {
                            if (Convert.ToDecimal(dgProductsList.Rows[i].Cells["Amt_Received"].Value) > 0)
                            {
                                CB.Voucher_No = txtVchNo.Text;
                                CB.Bill_No = dgProductsList.Rows[i].Cells["Inv_No"].Value.ToString();
                               // MessageBox.Show(CB.Bill_No);
                                string bDate = dgProductsList.Rows[i].Cells["InvDate"].Value.ToString();

                                DateTime bdate =  DateTime.ParseExact(bDate, "dd/MM/yyyy", null);
                                CB.Bill_Date = bdate;
                                CB.Bill_Value = Convert.ToDecimal(dgProductsList.Rows[i].Cells["Tot_Inv_Value"].Value);
                                CB.Due_Amount = Convert.ToDecimal(dgProductsList.Rows[i].Cells["BalanceAmount"].Value);
                                CB.AmtReceived = Convert.ToDecimal(dgProductsList.Rows[i].Cells["Amt_Received"].Value);
                                CB.Company_ID = logIn.company;
                                CB.BU_ID = logIn.BU_ID;
                                CB.Customer_ID = Convert.ToInt32(txtCustomer.Text);
                                CB.RefDocNo = dgProductsList.Rows[i].Cells["Ref_Doc_No"].Value.ToString();
                                CB.Status = "Open";
                                db.Cust_Received_Bills.InsertOnSubmit(CB);
                                db.SubmitChanges();
                            }
                        }
                }
                totAmt = Convert.ToDecimal(txtTotAmount.Text);
                this.Close();                //}
                //else
                //{
                //    MessageBox.Show("No Bills / Invoices Are Selected");

                //}
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

        private void dgProductsList_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int columnIndex = dgProductsList.CurrentCell.ColumnIndex;
                string columnName = dgProductsList.Columns[columnIndex].Name;
                TextBox tb = e.Control as TextBox;
                if (tb != null && columnName == "Inv_No")
                {
                    tb.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb.AutoCompleteCustomSource = DataColl;
                }
                else
                {
                    tb.AutoCompleteMode = AutoCompleteMode.None;
                }

            }
            catch (Exception ex)
            {
            }
        }
        public void addItems(AutoCompleteStringCollection coll)
        {
            try
            {
                DataGridViewRow R1 = dgProductsList.Rows[dgProductsList.CurrentRow.Index];
                //var d = (from data in db.Customer_Bills_Views where  select data).ToList();


                var d = (from c in db.Customer_Bills_Views where c.Company_ID == logIn.company && c.bu_id == logIn.BU_ID && c.Acc_ID == Convert.ToInt32(txtCustomer.Text) && c.AmtBalance>0 select new { c.Bill_No }).ToList();

                DataTable dt = new DataTable();
                dt.Columns.Add("d");
                foreach (var item in d)
                {
                    dt.Rows.Add(item.Bill_No);
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    coll.Add(dt.Rows[i][0].ToString());
                }

            }
            catch (Exception ex)
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

                    decimal TotAmt = 0;
                    for (int i = 0; i < dgProductsList.Rows.Count; i++)
                    {


                        TotAmt += //((dgRecipts.Rows[i].Cells["Debit"].Value == DBNull.Value) && (dgSelectedocument.Rows[i].Cells["Debit"].Value == "") && (dgSelectedocument.Rows[i].Cells["Debit"].Value == null)) ? Convert.ToDecimal("0.00") : Convert.ToDecimal(dgSelectedocument.Rows[i].Cells["Debit"].Value);//
                            (dgProductsList.Rows[i].Cells["Amt_Received"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductsList.Rows[i].Cells["Amt_Received"].Value);



                    }
                    txtTotAmount.Text = TotAmt.ToString(".00");

                }
            }
        }
    }
}
