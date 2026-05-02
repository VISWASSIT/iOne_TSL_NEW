using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Ione_DAL;
namespace ioneNet.FinanaceManagement
{
    public partial class AccountVoucher : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        System.Data.Common.DbTransaction transaction;
        SqlConnection objSqlConnection;
        SqlCommand objSqlCommand;
        public static string vochertype = "",vchno,custname,refdocno;
        public static int custid;
        public static Boolean editmode;
        public static DataTable dt;
        DataSet ds = new DataSet();
        DataSet ds1 = new DataSet();

        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();

        public AccountVoucher()
        {
            InitializeComponent();
        }

        public void clear()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;

                //foreach (Control d in groupBox1.Controls)
                //{
                //    if (d is TextBox)
                //        (d as TextBox).Clear();
                //    if (d is ComboBox)
                //        (d as ComboBox).SelectedIndex = -1;

                //}
                //foreach (Control d in groupBox2.Controls)
                //{
                //    if (d is TextBox)
                //        (d as TextBox).Clear();
                //    if (d is ComboBox)
                //        (d as ComboBox).SelectedIndex = -1;

                //}

                if (dgRecipts.Rows.Count > 0)
                {
                    for (int i = 0; i < dgRecipts.Rows.Count - 1; i++)
                    {
                        dgRecipts.Rows.RemoveAt(i);
                        i--;
                        while (dgRecipts.Rows.Count == 0)
                            continue;
                    }
                }
                editmode = false;
                ConfigVchrs();
                AutoincrementId();
                txtDocRefNo.Text = "";
                txtTotCredit.Text = "";
                txtTotDebit.Text = "";
                txtInstrumentNo.Text = "";
                txtNarration.Text = "";
                txtNetDue.Text = "";
                txtAdvanceAmt.Text = "";
                txtAdvVchNo.Text = "";
                lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

                //if (dgSelectedocument.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dgSelectedocument.Rows.Count - 1; i++)
                //    {
                //        dgSelectedocument.Rows.RemoveAt(i);
                //        i--;
                //        while (dgSelectedocument.Rows.Count == 0)
                //            continue;
                //    }
                //}
                //txtTotalAmt.Text = "";
                //txtTotalReceivedAmount.Text = "";

                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                MessageBox.Show(ex.Message + " While  clear Getting Error ", "PaymentVoucher", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }

        public void addItems(AutoCompleteStringCollection coll)
        {
            try
            {
                DataGridViewRow R1 = dgRecipts.Rows[dgRecipts.CurrentRow.Index];

                var d = (from c in db.AccountMasters where c.Company_ID == logIn.company  select new { c.AccName }).ToList();

                DataTable dt = new DataTable();
                dt.Columns.Add("d");
                foreach (var item in d)
                {
                    dt.Rows.Add(item.AccName);
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    coll.Add(dt.Rows[i][0].ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       

        private void Receipt_Load(object sender, EventArgs e)
        {
            try
            {
                dpdate.MinDate = logIn.fy_Start_Date;
                dpdate.MaxDate = logIn.fy_End_Date;
                //Status
                var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Trans" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbStatus.DataSource = pStatus;
                    cmbStatus.ValueMember = "ID";
                    cmbStatus.DisplayMember = "Descr";
                }

                ////Sales Account
                //var d = (from po in db.AccountMasters
                //         join A in db.AccountGroups on po.AccGroup_ID equals A.ID
                //         where po.Company_ID == logIn.company && A.GroupType == "Income"
                //         select new { po.id, po.AccName }).Distinct().ToList();
                //if (d.Count > 0)
                //{
                //    cmbSaleAccount.DataSource = d;
                //    cmbSaleAccount.ValueMember = "id";
                //    cmbSaleAccount.DisplayMember = "AccName";
                //}

                editmode = false;
                ConfigVchrs();
                AutoincrementId();
                lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                if(ioneNet.FinanaceManagement.Reports.frmAccountLedger.vchno != null)
                {
                    editmode = true;
                    txtVchNo.Text = ioneNet.FinanaceManagement.Reports.frmAccountLedger.vchno;
                    cmbVocherType.Text = ioneNet.FinanaceManagement.Reports.frmAccountLedger.vchtype;

                    bindedit();
                    decimal debit = 0, credit = 0;
                    for (int i = 0; i < dgRecipts.Rows.Count - 1; i++)
                    {

                        debit += (dgRecipts.Rows[i].Cells["Debit_Amount"].Value == "" || dgRecipts.Rows[i].Cells["Debit_Amount"].Value == null || dgRecipts.Rows[i].Cells["Debit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Debit_Amount"].Value);
                        credit += (dgRecipts.Rows[i].Cells["Credit_Amount"].Value == "" || dgRecipts.Rows[i].Cells["Credit_Amount"].Value == null || dgRecipts.Rows[i].Cells["Credit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Credit_Amount"].Value);

                    }
                    txtTotDebit.Text = debit.ToString(".00");
                    txtTotCredit.Text = credit.ToString(".00");
                    ioneNet.FinanaceManagement.Reports.frmAccountLedger.vchedit = false;
                    ioneNet.FinanaceManagement.Reports.frmAccountLedger.vchno = null;


                }

                if (ioneNet.FinanceManagement.Transactions.PaymentRequestList.SO_No != null)
                {
                    editmode = false;
                    txtDocRefNo.Text = ioneNet.FinanceManagement.Transactions.PaymentRequestList.SO_No;
                    label4.Text = "PAYMENT VOUCHER";
                    cmbVocherType.Text = "Payment Voucher";
                    txtDocRefNo.Visible = true;
                    label5.Visible = true;
                    label5.Text = "Pymy Req No";
                   // btnGetDoc.Visible = false;
                    label12.Text = "Payment/Receipt Ref No";
                    label7.Text = "Date";
                    label3.Visible = true;
                    linkLabel1.Visible = false;
                    txtAdvVchNo.Visible = false;
                    txtAdvanceAmt.Visible = false;
                    txtNetDue.Visible = false;
                    label10.Visible = true;
                    label11.Visible = true;
                    label9.Visible = true;
                    AutoincrementId();
                    //bindedit();
                    DataTable dt = new DataTable();
                    //String conStr = "Data Source=172.16.6.173;Initial Catalog=servion_hari;User  ID=sa;Password=Servion@123";
                    //SqlConnection con = new SqlConnection(con);
                    DateTime t = dpdate.Value;
                    string t1 = t.ToString("dd/MMM/yyyy");
                    SqlCommand com = new SqlCommand("[BindAccountToPaymentRequest]", con);
                    com.Parameters.AddWithValue("@compname", logIn.company);
                    com.Parameters.AddWithValue("@ReqNo", txtDocRefNo.Text);
                    com.Parameters.AddWithValue("@edate", t1);
                    com.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(com);
                    try
                    {
                        con.Open();
                        da.Fill(dt);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        if (con.State == ConnectionState.Open)
                            con.Close();
                    }
                    if (dt.Rows.Count > 0)
                    {
                        dgRecipts.DataSource = dt;

                    }
                    decimal debit = 0, credit = 0;
                    for (int i = 0; i < dgRecipts.Rows.Count - 1; i++)
                    {

                        debit += (dgRecipts.Rows[i].Cells["Debit_Amount"].Value == "" || dgRecipts.Rows[i].Cells["Debit_Amount"].Value == null || dgRecipts.Rows[i].Cells["Debit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Debit_Amount"].Value);
                        credit += (dgRecipts.Rows[i].Cells["Credit_Amount"].Value == "" || dgRecipts.Rows[i].Cells["Credit_Amount"].Value == null || dgRecipts.Rows[i].Cells["Credit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Credit_Amount"].Value);

                    }
                    txtTotDebit.Text = debit.ToString(".00");
                    txtTotCredit.Text = credit.ToString(".00");
                    ioneNet.FinanceManagement.Transactions.PaymentRequestList.SO_No = "";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }  

        private void btnClear_Click(object sender, EventArgs e)
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                decimal ActAmt = Convert.ToDecimal(txtTotCredit.Text);
                decimal advAmt = (txtAdvanceAmt.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtAdvanceAmt.Text);

                if (cmbVocherType.Text == "")
                {
                    MessageBox.Show("please Select the Voucher Type ,should not be Empty");
                    cmbVocherType.Focus();
                    return;
                }

                else if (Convert.ToDecimal(txtTotCredit.Text) != Convert.ToDecimal(txtTotDebit.Text))
                {
                    MessageBox.Show("Debbit and Credit Amount not equal ,Please Try again");
                    dgRecipts.Focus();
                    return;
                }

                else
                {
                    if (advAmt <= ActAmt)
                    {

                        decimal NetDue = ActAmt - advAmt;
                        txtNetDue.Text = NetDue.ToString(".00");
                    }
                    else
                    {
                        MessageBox.Show("Advance Adjustment Cannot be Greater Than Actual Bill Amount");
                        return;
                    }
                }
                //else
                //{

                    Save();
                    
                    
                //}
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void AutoincrementId()
        {
            try
            {

                var result =  db.Sp_autoincrement_AccountVoucher(logIn.company,cmbVocherType.Text,logIn.fy_Start_Date,logIn.fy_End_Date,logIn.BU_ID);
                txtVchNo.Text = result.FirstOrDefault().Voucher_No;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Save()
        {
            try
            {
                String myString = "";
                myString = txtVchNo.Text;
                if ((from u in db.Account_Vouchers where u.Voucher_No == myString && u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID select u).Count() > 0)
                {
                    myString = txtVchNo.Text;
                    db.sp_AccountVoucher_Delete(myString, logIn.company,cmbVocherType.Text,logIn.BU_ID);

                }
                else
                {
                    AutoincrementId();
                    myString = txtVchNo.Text;

                }
                //if (frmGate.Modify.Contains(this.Text))
                //{

                // dgProducts.Enabled = false;
                //var S = db.Sale_Order_Masters.Where(w => w.SO_NO == myString && w.Company_ID == logIn.company).FirstOrDefault();
                for (int i = 0; i < dgRecipts.RowCount - 1; i++)
                {
                    Account_Voucher S = new Account_Voucher();
                    {
                        S.Voucher_No = myString;
                        S.Voucher_Date   = dpdate.Value;
                        S.Voucher_Type = cmbVocherType.Text;                   
                        S.Doc_Ref_No = txtDocRefNo.Text;
                        S.Status = Convert.ToInt32(cmbStatus.SelectedValue.ToString());
                        S.Tot_Debit = (txtTotDebit.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotDebit.Text);
                        S.Tot_Credit = (txtTotCredit.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotCredit.Text);
                        S.Transaction_Ref_No = (txtInstrumentNo.Text == "") ? "" : txtInstrumentNo.Text;
                        S.Transaction_Ref_Date = dpinsmentsdate.Value;
                        S.Narration = (txtNarration.Text == "") ? "" : txtNarration.Text;                  

                        //Save Child Data
                        S.Acc_ID= Convert.ToInt32((dgRecipts.Rows[i].Cells["Acc_ID"].Value).ToString());
                        S.AccName = (dgRecipts.Rows[i].Cells["AccName"].Value == null) ? "" : (dgRecipts.Rows[i].Cells["AccName"].Value).ToString();
                        S.Debit_Amount = (dgRecipts.Rows[i].Cells["Debit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Debit_Amount"].Value);
                        S.Credit_Amount = (dgRecipts.Rows[i].Cells["Credit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Credit_Amount"].Value);
                        S.Balance_amount = (dgRecipts.Rows[i].Cells["Balance_amount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Balance_amount"].Value);
                        S.Balance_Type = (dgRecipts.Rows[i].Cells["Balance_Type"].Value == null) ? "" : (dgRecipts.Rows[i].Cells["Balance_Type"].Value).ToString();
                        S.Advance_Vch_No = (txtAdvVchNo.Text == "") ? "" : txtAdvVchNo.Text;
                        S.Advance_Amount = (txtAdvanceAmt.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtAdvanceAmt.Text);
                        S.Net_Due_Amount = (txtNetDue.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtNetDue.Text);
                        S.BU_ID = logIn.BU_ID;
                        S.Company_ID = logIn.company;
                        S.Created_By = lblCreatedBy.Text;
                        S.Modified_BY = logIn.username + "-" + DateTime.Now;
                        db.Account_Vouchers.InsertOnSubmit(S);
                        db.SubmitChanges();
                    }    
                }
                db.SubmitChanges();
                //Save Bill Details
                //if (ioneNet.FinanceManagement.GetBillDetails.dtgetfinalprducts.Rows.Count > 0)
                //{
                //    for (int i = 0; i < ioneNet.FinanceManagement.GetBillDetails.dtgetfinalprducts.Rows.Count; i++)
                //    {
                //        Cust_Received_Bill CB = new Cust_Received_Bill();
                //        {
                //            CB.Voucher_No = txtVchNo.Text;
                //            CB.Bill_No = ioneNet.FinanceManagement.GetBillDetails.dtgetfinalprducts.Rows[i]["Inv_No"].ToString();
                //            CB.Bill_Date = Convert.ToDateTime(ioneNet.FinanceManagement.GetBillDetails.dtgetfinalprducts.Rows[i]["InvDate"].ToString());
                //            CB.Bill_Value = Convert.ToDecimal(ioneNet.FinanceManagement.GetBillDetails.dtgetfinalprducts.Rows[i]["Tot_Inv_Value"].ToString());
                //            CB.Due_Amount = Convert.ToDecimal(ioneNet.FinanceManagement.GetBillDetails.dtgetfinalprducts.Rows[i]["BalanceAmount"].ToString());
                //            CB.AmtReceived = Convert.ToDecimal(ioneNet.FinanceManagement.GetBillDetails.dtgetfinalprducts.Rows[i]["Amt_Received"].ToString());
                //            CB.Company_ID = logIn.company;
                //            CB.Customer_ID = custid;
                //            db.Cust_Received_Bills.InsertOnSubmit(CB);
                //            db.SubmitChanges();
                //        }


                //    }
                //}
                SqlCommand cmd = new SqlCommand();
                cmd.CommandText = "Update Cust_Received_Bills set status = 'Closed' where [Voucher_No]=@param1 and Company_ID =@compName";
                if (txtDocRefNo.Visible == true)
                {
                    cmd.Parameters.AddWithValue("@param1", txtDocRefNo.Text);
                }
                else
                {
                    cmd.Parameters.AddWithValue("@param1", txtVchNo.Text);
                }
                cmd.Parameters.AddWithValue("@CompName", logIn.company);
                cmd.Connection = con;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
                //transaction.Commit();               
                MessageBox.Show("Voucher Saved / Updated Successfully With Transaction Ref No : " + txtVchNo.Text);
                ioneNet.FinanceManagement.GetBillDetails.dtgetfinalprducts.Rows.Clear();
                string vchtype = cmbVocherType.Text;
                clear();
                cmbVocherType.Text = vchtype;
                AutoincrementId();
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

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {

                //if (AppCode.GlobalAccess.Edit == "Yes")
                //{
                //    if (txtVchNo.Text != "")
                //    {
                //        if ((from u in db.FIN_Receipt_Payment_Vouchers where u.VoucherNo == txtVchNo.Text && u.Creation_Company == AppCode.GlobalAccess.companyName && u.Type == "Payment" select u).Count() > 0)
                //        {
                DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    //myString = txtVchNo.Text;
                    db.sp_AccountVoucher_Delete(txtVchNo.Text, logIn.company, cmbVocherType.Text,logIn.BU_ID);
                    db.sp_AccountVoucher_Bill_Delete(txtVchNo.Text, null,logIn.BU_ID);
                    var getProductName = (from s in db.Account_Opening_BillWises
                                          where s.Vch_Ref_No == txtVchNo.Text
                                          select new { s.Acc_ID }).FirstOrDefault();

                    if (getProductName != null)
                    {

                        custid = Convert.ToInt32(getProductName.Acc_ID);
                    }
                    //db.sp_DeleteOpeningBalance_BillWise(custid, logIn.company, txtVchNo.Text);
                    
                    MessageBox.Show("Record Deleted Successfully");
                    clear();
                }
                //            db.Transaction = null;
                //            System.Data.Common.DbTransaction transaction;
                //            var result = MessageBox.Show("Are You Sure Want to Delete this Record ", "PAYMENT VOUCHER", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                //            if (result == DialogResult.Yes)
                //            {
                //                if (null != db.Connection)
                //                {
                //                    db.Connection.Close();
                //                }
                //                db.Connection.Open();
                //                transaction = db.Connection.BeginTransaction();
                //                db.Transaction = transaction;
                //                db.sp_FIN_Delete_Voucher(AppCode.GlobalAccess.companyName, txtVchNo.Text, 2);
                //                MessageBox.Show("Record Deleted Successfully", "PAYMENT VOUCHER", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //                transaction.Commit();
                //                clear();
                //            }
                //            else
                //            {
                //                return;
                //            }

                //        }
                //        else
                //        {
                //            MessageBox.Show("Please Select the Another Record,this Record Not Existing--", "PAYMENT VOUCHER", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //            return;


                //        }
                //    }
                //    else
                //    {
                //        MessageBox.Show("Please Select the Record for Delete", "PAYMENT VOUCHER", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //        return;
                //    }
                //}
                //else
                //{
                //    MessageBox.Show("You dont Have Privileges", "PAYMENT VOUCHER", MessageBoxButtons.OK, MessageBoxIcon.Information);

                //    clear();
                //    return;
                //}
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                transaction.Rollback();
                MessageBox.Show(ex.Message + "Record Not Delete", "PAYMENT VOUCHER", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            finally
            {
                if (null != db.Connection)
                {
                    db.Connection.Close();
                }
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();

        }
        

        private void dgRecipts_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int columnIndex = dgRecipts.CurrentCell.ColumnIndex;
                string columnName = dgRecipts.Columns[columnIndex].Name;
                TextBox tb = e.Control as TextBox;
                if (tb != null && columnName == "AccName")
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
                MessageBox.Show(ex.Message);
            }
        }

        private void dgRecipts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow R1 = dgRecipts.Rows[dgRecipts.CurrentRow.Index];

                if ((R1.Cells["AccName"].Value) != "" && (R1.Cells["AccName"].Value) != null && (R1.Cells["AccName"].Value) != DBNull.Value)
                {
                    if ((from u in db.AccountMasters where u.AccName == R1.Cells["AccName"].Value  && u.Company_ID == logIn.company select u).Count() == 0)
                    {
                        MessageBox.Show("Please  select the valid the Account Name");
                        R1.Cells["AccName"].Value = "";
                        return;
                    }
                    else
                    {
                        var getProductName = (from s in db.AccountMasters                                                                         
                                            where s.AccName == R1.Cells["AccName"].Value.ToString() && s.Company_ID ==logIn.company
                                              select new { s.id }).FirstOrDefault();

                        if (getProductName != null)                       {
                          
                            R1.Cells["Acc_ID"].Value = getProductName.id.ToString();
                            DateTime t = dpdate.Value;
                            string t1 = t.ToString("dd/MMM/yyyy");
                            var getBal = (from b in db.GetAccountBalance(logIn.company,t, Convert.ToInt32(getProductName.id.ToString()),1,logIn.BU_ID)
                                select new { b.Balance,b.BalType }).FirstOrDefault();
                            if (getBal != null)
                            {
                                R1.Cells["Balance_amount"].Value = getBal.Balance.ToString();
                                R1.Cells["Balance_Type"].Value = getBal.BalType.ToString();
                            }
                            else
                            {
                                R1.Cells["Balance_amount"].Value = "0";
                            }
                        }

                       



                    }
                }

                decimal Credit = 0, debit = 0;
                for (int i = 0; i < dgRecipts.Rows.Count - 1; i++)
                {


                    debit += //((dgRecipts.Rows[i].Cells["Debit"].Value == DBNull.Value) && (dgSelectedocument.Rows[i].Cells["Debit"].Value == "") && (dgSelectedocument.Rows[i].Cells["Debit"].Value == null)) ? Convert.ToDecimal("0.00") : Convert.ToDecimal(dgSelectedocument.Rows[i].Cells["Debit"].Value);//
                        (dgRecipts.Rows[i].Cells["Debit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Debit_Amount"].Value);
                    Credit += //((dgRecipts.Rows[i].Cells["Debit"].Value == DBNull.Value) && (dgSelectedocument.Rows[i].Cells["Debit"].Value == "") && (dgSelectedocument.Rows[i].Cells["Debit"].Value == null)) ? Convert.ToDecimal("0.00") : Convert.ToDecimal(dgSelectedocument.Rows[i].Cells["Debit"].Value);//
                    (dgRecipts.Rows[i].Cells["Credit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Credit_Amount"].Value);



                }
                if (debit==0)
                {

                }
                else
                {
                  //  (R1.Cells["Debit"].Value) = debit;
                    txtTotDebit.Text = debit.ToString(".00");
                    txtTotCredit.Text = Credit.ToString(".00");
                }
              


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       
       
       

       

        
        

        

        

        
        
        public string sss;
        public string name1;
        private void dgRecipts_KeyDown(object sender, KeyEventArgs e)
        {
            try

            {
                decimal Credit = 0, debit = 0;
                if (e.KeyCode == Keys.F3) //Select Bills
                {
                    
                    DataGridViewRow R1 = dgRecipts.Rows[dgRecipts.CurrentRow.Index];
                    custid = Convert.ToInt32(R1.Cells["Acc_ID"].Value);
                    custname = R1.Cells["AccName"].Value.ToString();
                    refdocno = txtDocRefNo.Text;
                    if (txtDocRefNo.Visible=true && txtDocRefNo.Text!="")
                    {
                        vchno = txtDocRefNo.Text;
                    }
                    else
                    {
                        vchno = txtVchNo.Text;
                    }
                    ioneNet.FinanceManagement.GetBillDetails form = new ioneNet.FinanceManagement.GetBillDetails();
                    //ioneNet.Masters.ProdSearch.frmName = "SOrder";
                    form.ShowDialog();
                    if (ioneNet.FinanceManagement.GetBillDetails.totAmt > 0)
                    {
                        if (cmbVocherType.Text == "Receipt Voucher")
                        {
                            R1.Cells["Credit_Amount"].Value = ioneNet.FinanceManagement.GetBillDetails.totAmt;
                        }
                        else
                        {
                            R1.Cells["Debit_Amount"].Value = ioneNet.FinanceManagement.GetBillDetails.totAmt;
                        }
                        for (int i = 0; i < dgRecipts.Rows.Count - 1; i++)
                        {


                            debit += //((dgRecipts.Rows[i].Cells["Debit"].Value == DBNull.Value) && (dgSelectedocument.Rows[i].Cells["Debit"].Value == "") && (dgSelectedocument.Rows[i].Cells["Debit"].Value == null)) ? Convert.ToDecimal("0.00") : Convert.ToDecimal(dgSelectedocument.Rows[i].Cells["Debit"].Value);//
                                (dgRecipts.Rows[i].Cells["Debit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Debit_Amount"].Value);
                            Credit += //((dgRecipts.Rows[i].Cells["Debit"].Value == DBNull.Value) && (dgSelectedocument.Rows[i].Cells["Debit"].Value == "") && (dgSelectedocument.Rows[i].Cells["Debit"].Value == null)) ? Convert.ToDecimal("0.00") : Convert.ToDecimal(dgSelectedocument.Rows[i].Cells["Debit"].Value);//
                            (dgRecipts.Rows[i].Cells["Credit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Credit_Amount"].Value);



                        }
                        if (debit == 0)
                        {

                        }
                        else
                        {
                            //  (R1.Cells["Debit"].Value) = debit;
                            txtTotDebit.Text = debit.ToString(".00");
                            txtTotCredit.Text = Credit.ToString(".00");
                        }

                    }
                    ioneNet.FinanceManagement.GetBillDetails.totAmt = 0;
                }

                if (e.KeyCode == Keys.F4) //Enter  Bills Details for JVs
                {

                    DataGridViewRow R1 = dgRecipts.Rows[dgRecipts.CurrentRow.Index];
                    custid = Convert.ToInt32(R1.Cells["Acc_ID"].Value);
                    custname = R1.Cells["AccName"].Value.ToString();
                    refdocno = txtDocRefNo.Text;
                    vchno = txtVchNo.Text;
                    ioneNet.FinanceManagement.Masters.AccountOBBillWise form = new ioneNet.FinanceManagement.Masters.AccountOBBillWise();
                    //ioneNet.Masters.ProdSearch.frmName = "SOrder";
                    form.ShowDialog();
                    
                }




                if (e.KeyCode == Keys.F6) //Remove Rows
                {
                    if (dgRecipts.Rows.Count > 0)
                    {

                        foreach (DataGridViewCell oneCell in dgRecipts.SelectedCells)
                        {
                            if (oneCell.Selected)
                                dgRecipts.Rows.RemoveAt(oneCell.RowIndex);
                        }
                       
                        for (int i = 0; i < dgRecipts.Rows.Count - 1; i++)
                        {


                            debit += //((dgRecipts.Rows[i].Cells["Debit"].Value == DBNull.Value) && (dgSelectedocument.Rows[i].Cells["Debit"].Value == "") && (dgSelectedocument.Rows[i].Cells["Debit"].Value == null)) ? Convert.ToDecimal("0.00") : Convert.ToDecimal(dgSelectedocument.Rows[i].Cells["Debit"].Value);//
                                (dgRecipts.Rows[i].Cells["Debit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Debit_Amount"].Value);
                            Credit += //((dgRecipts.Rows[i].Cells["Debit"].Value == DBNull.Value) && (dgSelectedocument.Rows[i].Cells["Debit"].Value == "") && (dgSelectedocument.Rows[i].Cells["Debit"].Value == null)) ? Convert.ToDecimal("0.00") : Convert.ToDecimal(dgSelectedocument.Rows[i].Cells["Debit"].Value);//
                            (dgRecipts.Rows[i].Cells["Credit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Credit_Amount"].Value);



                        }
                        if (debit == 0)
                        {

                        }
                        else
                        {
                            //  (R1.Cells["Debit"].Value) = debit;
                            txtTotDebit.Text = debit.ToString(".00");
                            txtTotCredit.Text = Credit.ToString(".00");
                        }

                    }
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            
        }

        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            //if (e.KeyChar != (Char)Keys.Back) //allow backspace (to delete)
            //{
            //    e.Handled = !char.IsNumber(e.KeyChar);
            //}

            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == (char)Keys.Back || e.KeyChar == '.'))
            { e.Handled = true; }
            TextBox txtDecimal = sender as TextBox;
            if (e.KeyChar == '.' && txtDecimal.Text.Contains("."))
            {
                e.Handled = true;
            }
            
        }

        
        private void DocRefNo_Leave(object sender, EventArgs e)
        {
            try
            {
                string VchType = cmbVocherType.Text;
                int buid = logIn.company;
                switch (VchType)
                {
                    case "Sale Voucher":
                        if (txtDocRefNo.Text != "")
                        {
                            if ((from u in db.Invoice_Masters where u.Inv_No == txtDocRefNo.Text && u.Company_ID == logIn.company  select u).Count() > 0)
                            {
                                if (editmode == false)
                                {
                                    if ((from u in db.Account_Vouchers where u.Doc_Ref_No == txtDocRefNo.Text && u.Company_ID == logIn.company && u.Voucher_Type == "Sale Voucher" select u).Count() > 0)
                                    {
                                        MessageBox.Show("Sale Voucher is Already Posted for Selected Invoice No");
                                        txtDocRefNo.Focus();
                                        return;

                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid Invoice No Entered");
                                txtDocRefNo.Focus();
                                return;

                            }

                            //Bind Vouhcers
                            //Get Acc ID 
                            var d4 = (from a in db.Costing_Units where a.id == logIn.BU_ID select new { a.VoucherSeries }).ToList();
                            if (d4.Count > 0)
                            {
                                if (d4[0].VoucherSeries == "Company Wise")
                                {
                                    var d2 = (from a in db.Invoice_Masters where a.Inv_No == txtDocRefNo.Text && a.Company_ID == logIn.company select new { a.BU_ID }).ToList();
                                    buid = Convert.ToInt32(d2[0].BU_ID);
                                }
                                else
                                if (d4[0].VoucherSeries == "Separate Series for this BU")
                                {
                                    buid = logIn.BU_ID;
                                }

                            }
                            DataTable dt = new DataTable();
                            //String conStr = "Data Source=172.16.6.173;Initial Catalog=servion_hari;User  ID=sa;Password=Servion@123";
                            //SqlConnection con = new SqlConnection(con);
                            DateTime t = dpdate.Value;                                                     
                            string t1 = t.ToString("dd/MMM/yyyy");
                            SqlCommand com = new SqlCommand("[BindAccountToSaleVoucher]", con);
                            com.Parameters.AddWithValue("@compname", logIn.company);
                            com.Parameters.AddWithValue("@InvNo", txtDocRefNo.Text);
                            com.Parameters.AddWithValue("@edate", t1);
                            com.Parameters.AddWithValue("@buid", buid);
                            com.CommandType = CommandType.StoredProcedure;
                            SqlDataAdapter da = new SqlDataAdapter(com);
                            try
                            {
                                con.Open();
                                da.Fill(dt);
                            }
                            catch (Exception ex)
                            {
                               MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                if (con.State == ConnectionState.Open)
                                    con.Close();
                            }
                            if (dt.Rows.Count > 0)
                            {
                                dgRecipts.DataSource = dt;

                            }
                            decimal debit = 0, credit = 0;
                            for (int i = 0; i < dgRecipts.Rows.Count - 1; i++)
                            {

                                debit += (dgRecipts.Rows[i].Cells["Debit_Amount"].Value == "" || dgRecipts.Rows[i].Cells["Debit_Amount"].Value == null || dgRecipts.Rows[i].Cells["Debit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Debit_Amount"].Value);
                                credit += (dgRecipts.Rows[i].Cells["Credit_Amount"].Value == "" || dgRecipts.Rows[i].Cells["Credit_Amount"].Value == null || dgRecipts.Rows[i].Cells["Credit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Credit_Amount"].Value);

                            }
                            txtTotDebit.Text = debit.ToString(".00");
                            txtTotCredit.Text = credit.ToString(".00");
                        }
                            break;
                    case "Purchase Voucher":

                        if (txtDocRefNo.Text != "")
                        {
                            if (checkBox1.Checked == true)
                            {
                                if ((from u in db.GoodsReceiptNote_Import_Masters where u.Grn_NO == txtDocRefNo.Text && u.Company_ID == logIn.company  && u.isDeleted == false select u).Count() > 0)
                                {
                                    if (editmode == false)
                                    {
                                        if ((from u in db.Account_Vouchers where u.Doc_Ref_No == txtDocRefNo.Text && u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID &&  u.Voucher_Type == "Purchase Voucher" select u).Count() > 0)
                                        {
                                            MessageBox.Show("Purchase Voucher is Already Posted for Selected GRN No");
                                            txtDocRefNo.Focus();
                                            return;

                                        }
                                    }
                                }

                                else
                                {
                                    MessageBox.Show("Invalid GRN No Entered");
                                    txtDocRefNo.Focus();
                                    return;

                                }
                            }
                            else
                            {
                                if ((from u in db.GoodsReceiptNote_Masters where u.Grn_NO == txtDocRefNo.Text && u.Company_ID == logIn.company && u.isDeleted == false select u).Count() > 0)
                                {
                                    if (editmode == false)
                                    {
                                        if ((from u in db.Account_Vouchers where u.Doc_Ref_No == txtDocRefNo.Text && u.Company_ID == logIn.company && u.Voucher_Type == "Purchase Voucher" select u).Count() > 0)
                                        {
                                            MessageBox.Show("Purchase Voucher is Already Posted for Selected GRN No");
                                            txtDocRefNo.Focus();
                                            return;

                                        }
                                    }
                                }

                                else
                                {
                                    MessageBox.Show("Invalid GRN No Entered");
                                    txtDocRefNo.Focus();
                                    return;

                                }
                            }
                            if (checkBox1.Checked == true)
                            {

                                var d3 = (from a in db.Costing_Units where a.id == logIn.BU_ID select new { a.VoucherSeries }).ToList();
                                if (d3.Count > 0)
                                {
                                    if (d3[0].VoucherSeries == "Company Wise")
                                    {
                                        var d2 = (from a in db.GoodsReceiptNote_Import_Masters where a.Grn_NO == txtDocRefNo.Text && a.Company_ID == logIn.company select new { a.BU_ID }).ToList();
                                        buid = Convert.ToInt32(d2[0].BU_ID);
                                    }
                                    else
                                    if (d3[0].VoucherSeries == "Separate Series for this BU")
                                    {
                                        buid = logIn.BU_ID;
                                    }

                                }
                                DataTable dt1 = new DataTable();
                                //String conStr = "Data Source=172.16.6.173;Initial Catalog=servion_hari;User  ID=sa;Password=Servion@123";
                                //SqlConnection con = new SqlConnection(con);
                                DateTime t2 = dpdate.Value;
                                string t3 = t2.ToString("dd/MMM/yyyy");
                                SqlCommand com1 = new SqlCommand("[BindAccountToPurchaseVoucher_Import]", con);
                                com1.Parameters.AddWithValue("@compname", logIn.company);
                                com1.Parameters.AddWithValue("@InvNo", txtDocRefNo.Text);
                                com1.Parameters.AddWithValue("@edate", t2);
                                com1.Parameters.AddWithValue("@buid", buid);
                                com1.CommandType = CommandType.StoredProcedure;
                                SqlDataAdapter da1 = new SqlDataAdapter(com1);
                                try
                                {
                                    con.Open();
                                    da1.Fill(dt1);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                                finally
                                {
                                    if (con.State == ConnectionState.Open)
                                        con.Close();
                                }
                                if (dt1.Rows.Count > 0)
                                {
                                    dgRecipts.DataSource = dt1;

                                }
                                decimal debit1 = 0, credit1 = 0;
                                for (int i = 0; i < dgRecipts.Rows.Count - 1; i++)
                                {

                                    debit1 += (dgRecipts.Rows[i].Cells["Debit_Amount"].Value == "" || dgRecipts.Rows[i].Cells["Debit_Amount"].Value == null || dgRecipts.Rows[i].Cells["Debit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Debit_Amount"].Value);
                                    credit1 += (dgRecipts.Rows[i].Cells["Credit_Amount"].Value == "" || dgRecipts.Rows[i].Cells["Credit_Amount"].Value == null || dgRecipts.Rows[i].Cells["Credit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Credit_Amount"].Value);

                                }
                                txtTotDebit.Text = debit1.ToString(".00");
                                txtTotCredit.Text = credit1.ToString(".00");
                            }
                            else
                            {
                                //Bind Account                      

                                DataTable dt = new DataTable();
                                //String conStr = "Data Source=172.16.6.173;Initial Catalog=servion_hari;User  ID=sa;Password=Servion@123";
                                //SqlConnection con = new SqlConnection(con);
                                //Get BUID from GRN

                                var d1 = (from a in db.Costing_Units where a.id == logIn.BU_ID select new { a.VoucherSeries }).ToList();
                                if (d1.Count > 0)
                                {
                                    if (d1[0].VoucherSeries == "Company Wise")
                                    {
                                        var d2 = (from a in db.GoodsReceiptNote_Masters where a.Grn_NO == txtDocRefNo.Text && a.Company_ID == logIn.company && a.isDeleted == false select new { a.BU_ID }).ToList();
                                        buid = Convert.ToInt32(d2[0].BU_ID);
                                    }
                                    else
                                    if (d1[0].VoucherSeries == "Separate Series for this BU")
                                    {
                                        buid = logIn.BU_ID;
                                    }

                                }
                                DateTime t = dpdate.Value;
                                string t1 = t.ToString("dd/MMM/yyyy");
                                SqlCommand com = new SqlCommand("[BindAccountToPurchaseVoucher]", con);
                                com.Parameters.AddWithValue("@compname", logIn.company);
                                com.Parameters.AddWithValue("@InvNo", txtDocRefNo.Text);
                                com.Parameters.AddWithValue("@edate", t1);
                                com.Parameters.AddWithValue("@buid", buid);
                                com.CommandType = CommandType.StoredProcedure;
                                SqlDataAdapter da = new SqlDataAdapter(com);
                                try
                                {
                                    con.Open();
                                    da.Fill(dt);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                                finally
                                {
                                    if (con.State == ConnectionState.Open)
                                        con.Close();
                                }
                                if (dt.Rows.Count > 0)
                                {
                                    dgRecipts.DataSource = dt;

                                }
                                decimal debit = 0, credit = 0;
                                for (int i = 0; i < dgRecipts.Rows.Count - 1; i++)
                                {

                                    debit += (dgRecipts.Rows[i].Cells["Debit_Amount"].Value == "" || dgRecipts.Rows[i].Cells["Debit_Amount"].Value == null || dgRecipts.Rows[i].Cells["Debit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Debit_Amount"].Value);
                                    credit += (dgRecipts.Rows[i].Cells["Credit_Amount"].Value == "" || dgRecipts.Rows[i].Cells["Credit_Amount"].Value == null || dgRecipts.Rows[i].Cells["Credit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Credit_Amount"].Value);

                                }
                                txtTotDebit.Text = debit.ToString(".00");
                                txtTotCredit.Text = credit.ToString(".00");
                            }
                        }
                        break;
                    case "Credit Note":

                        if (txtDocRefNo.Text != "")
                        {
                            if ((from u in db.SaleReturns_Masters where u.Vch_No == txtDocRefNo.Text && u.Company_ID == logIn.company select u).Count() > 0)
                            {
                                if (editmode == false)
                                {
                                    if ((from u in db.Account_Vouchers where u.Doc_Ref_No == txtDocRefNo.Text && u.Company_ID == logIn.company && u.Voucher_Type == "Credit Note" select u).Count() > 0)
                                    {
                                        MessageBox.Show("Credit Note is Already Posted for Selected Vch No");
                                        txtDocRefNo.Focus();
                                        return;

                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid Vch No No Entered");
                                txtDocRefNo.Focus();
                                return;

                            }

                            //Bind Account                      
                            DataTable dt = new DataTable();
                            //String conStr = "Data Source=172.16.6.173;Initial Catalog=servion_hari;User  ID=sa;Password=Servion@123";
                            //SqlConnection con = new SqlConnection(con);
                            DateTime t = dpdate.Value;
                            string t1 = t.ToString("dd/MMM/yyyy");
                            SqlCommand com = new SqlCommand("[BindAccountToSaleReturnVoucher]", con);
                            com.Parameters.AddWithValue("@compname", logIn.company);
                            com.Parameters.AddWithValue("@InvNo", txtDocRefNo.Text);
                            com.Parameters.AddWithValue("@edate", t1);
                            com.Parameters.AddWithValue("@buid", logIn.BU_ID);
                            com.CommandType = CommandType.StoredProcedure;
                            SqlDataAdapter da = new SqlDataAdapter(com);
                            try
                            {
                                con.Open();
                                da.Fill(dt);
                            }
                            catch (Exception ex)
                            {
                               MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                if (con.State == ConnectionState.Open)
                                    con.Close();
                            }
                            if (dt.Rows.Count > 0)
                            {
                                dgRecipts.DataSource = dt;

                            }
                            decimal debit = 0, credit = 0;
                            for (int i = 0; i < dgRecipts.Rows.Count - 1; i++)
                            {

                                debit += (dgRecipts.Rows[i].Cells["Debit_Amount"].Value == "" || dgRecipts.Rows[i].Cells["Debit_Amount"].Value == null || dgRecipts.Rows[i].Cells["Debit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Debit_Amount"].Value);
                                credit += (dgRecipts.Rows[i].Cells["Credit_Amount"].Value == "" || dgRecipts.Rows[i].Cells["Credit_Amount"].Value == null || dgRecipts.Rows[i].Cells["Credit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Credit_Amount"].Value);

                            }
                            txtTotDebit.Text = debit.ToString(".00");
                            txtTotCredit.Text = credit.ToString(".00");
                        }
                        break;
                    case "Debit Note":

                        if (txtDocRefNo.Text != "")
                        {
                            if ((from u in db.Purchase_Return_Masters where u.Vch_NO == txtDocRefNo.Text && u.Company_ID == logIn.company select u).Count() > 0)
                            {
                                if (editmode == false)
                                {
                                    if ((from u in db.Account_Vouchers where u.Doc_Ref_No == txtDocRefNo.Text && u.Company_ID == logIn.company && u.Voucher_Type == "Credit Note" select u).Count() > 0)
                                    {
                                        MessageBox.Show("Debit Note is Already Posted for Selected Vch No");
                                        txtDocRefNo.Focus();
                                        return;

                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show("Invalid Vch No No Entered");
                                txtDocRefNo.Focus();
                                return;

                            }

                            //Bind Account                      
                            DataTable dt = new DataTable();
                            //String conStr = "Data Source=172.16.6.173;Initial Catalog=servion_hari;User  ID=sa;Password=Servion@123";
                            //SqlConnection con = new SqlConnection(con);
                            DateTime t = dpdate.Value;
                            string t1 = t.ToString("dd/MMM/yyyy");
                            SqlCommand com = new SqlCommand("[BindAccountToPurchaseReturnVoucher]", con);
                            com.Parameters.AddWithValue("@compname", logIn.company);
                            com.Parameters.AddWithValue("@InvNo", txtDocRefNo.Text);
                            com.Parameters.AddWithValue("@edate", t1);
                            com.Parameters.AddWithValue("@buid", logIn.BU_ID);
                            com.CommandType = CommandType.StoredProcedure;
                            SqlDataAdapter da = new SqlDataAdapter(com);
                            try
                            {
                                con.Open();
                                da.Fill(dt);
                            }
                            catch (Exception ex)
                            {
                               MessageBox.Show(ex.Message);
                            }
                            finally
                            {
                                if (con.State == ConnectionState.Open)
                                    con.Close();
                            }
                            if (dt.Rows.Count > 0)
                            {
                                dgRecipts.DataSource = dt;

                            }
                            decimal debit = 0, credit = 0;
                            for (int i = 0; i < dgRecipts.Rows.Count - 1; i++)
                            {

                                debit += (dgRecipts.Rows[i].Cells["Debit_Amount"].Value == "" || dgRecipts.Rows[i].Cells["Debit_Amount"].Value == null || dgRecipts.Rows[i].Cells["Debit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Debit_Amount"].Value);
                                credit += (dgRecipts.Rows[i].Cells["Credit_Amount"].Value == "" || dgRecipts.Rows[i].Cells["Credit_Amount"].Value == null || dgRecipts.Rows[i].Cells["Credit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Credit_Amount"].Value);

                            }
                            txtTotDebit.Text = debit.ToString(".00");
                            txtTotCredit.Text = credit.ToString(".00");
                        }
                        break;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void brnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                vochertype = cmbVocherType.Text;
            ioneNet.FinanceManagement.Transactions.frmVouchersList form = new ioneNet.FinanceManagement.Transactions.frmVouchersList();
            //ioneNet.Masters.ProdSearch.frmName = "SOrder";
            //form.ShowDialog();
            if (form.ShowDialog() == DialogResult.OK)
            {
                txtVchNo.Text = ioneNet.FinanceManagement.Transactions.frmVouchersList.vchno;
                editmode = true;
                bindedit();
                    decimal debit = 0, credit = 0;
                    for (int i = 0; i < dgRecipts.Rows.Count - 1; i++)
                    {

                        debit += (dgRecipts.Rows[i].Cells["Debit_Amount"].Value == "" || dgRecipts.Rows[i].Cells["Debit_Amount"].Value == null || dgRecipts.Rows[i].Cells["Debit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Debit_Amount"].Value);
                        credit += (dgRecipts.Rows[i].Cells["Credit_Amount"].Value == "" || dgRecipts.Rows[i].Cells["Credit_Amount"].Value == null || dgRecipts.Rows[i].Cells["Credit_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Credit_Amount"].Value);

                    }
                    txtTotDebit.Text = debit.ToString(".00");
                    txtTotCredit.Text = credit.ToString(".00");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }
        

        private void dgList_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            //txtVchNo.Text = dgList.Rows[dgList.CurrentRow.Index].Cells["Voucher_No"].Value.ToString();
            //groupBox4.Visible = false;
            //bindedit();
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //groupBox4.Visible = false;
        }

        private void dgList_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgRecipts_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void txtDocRefNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnGetDoc_Click(object sender, EventArgs e)
        {

        }

        private void AccountVoucher_FormClosed(object sender, FormClosedEventArgs e)
        {
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Delete from Cust_Received_Bills where [Voucher_No]=@param1 and Company_ID =@compName and status ='Open'";
            cmd.Parameters.AddWithValue("@param1", txtVchNo.Text);
            cmd.Parameters.AddWithValue("@CompName", logIn.company);
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                string path = Path.Combine(Directory.GetCurrentDirectory(), "AccountVoucher.pdf");
            //string path = @"D:\Invoice.pdf";
            FileInfo fi1 = new FileInfo(path);


            if (fi1.Exists)
            {
                fi1.Delete();
            }




            CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                
            rep = new ioneNet.FinanceManagement.Transactions.AccountVoucher();
                crConnectionInfo.ServerName = frmMain.ServerIP;
                crConnectionInfo.DatabaseName = frmMain.Database;
                crConnectionInfo.UserID = frmMain.DBUserID;
                crConnectionInfo.Password = frmMain.Password;
                crDatabase = rep.Database;
            crTables = crDatabase.Tables;
            //Loop through all tables in the report and apply the connection information for each table.
            for (int i = 0; i < crTables.Count; i++)
            {
                //  crTable = crTables[i];
                crTableLogOnInfo = crTables[i].LogOnInfo;
                crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                crTables[i].ApplyLogOnInfo(crTableLogOnInfo);
                //If your DatabaseName is changing at runtime, specify the table location. For example, when you are reporting off of a Northwind database on SQL server you should have the following line of code:

            }

            //rep.SetParameterValue("Creation_Company", frmLogin.Creation_Company);                 
            rep.RecordSelectionFormula = "{ Account_Voucher.Voucher_No} = '" + txtVchNo.Text + "' and { Account_Voucher.BU_ID} = " + logIn.BU_ID + "";
            ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
            // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
            viewer.crystalReportViewer1.ReportSource = rep;
            viewer.crystalReportViewer1.Refresh();
            rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);
            Process.Start(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //Get Account ID  
            int AccId=0;
            if (cmbVocherType.Text == "Sale Voucher")
            {
                for (int i = 0; i < dgRecipts.RowCount - 1; i++)
                {
                    if(Convert.ToDecimal(dgRecipts.Rows[i].Cells["Debit_Amount"].Value)>0)
                    {
                        AccId = Convert.ToInt32((dgRecipts.Rows[i].Cells["Acc_ID"].Value).ToString());
                        break;
                    }
                }
            }
            else
            {
                for (int i = 0; i < dgRecipts.RowCount - 1; i++)
                {
                    if (Convert.ToDecimal(dgRecipts.Rows[i].Cells["Credit_Amount"].Value) > 0)
                    {
                        AccId = Convert.ToInt32((dgRecipts.Rows[i].Cells["Acc_ID"].Value).ToString());
                        break;
                    }
                }

            }


                DataTable dt = new DataTable();
            //String conStr = "Data Source=172.16.6.173;Initial Catalog=servion_hari;User  ID=sa;Password=Servion@123";
            //SqlConnection con = new SqlConnection(con);
            DateTime t = dpdate.Value;
            string t1 = t.ToString("dd/MMM/yyyy");
            SqlCommand com = new SqlCommand("[GetAdvanceVouchers]", con);
            com.Parameters.AddWithValue("@compname", logIn.company);
            com.Parameters.AddWithValue("@accid", AccId);
            com.Parameters.AddWithValue("@vchdate", t1);
            com.Parameters.AddWithValue("@buid", logIn.BU_ID);
            com.CommandType = CommandType.StoredProcedure;
            SqlDataAdapter da = new SqlDataAdapter(com);
            try
            {
                con.Open();
                da.Fill(dt);
            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                    con.Close();
            }
            if (dt.Rows.Count > 0)
            {
                dgAdvVouhcers.DataSource = dt;

            }
            groupBox4.Visible = true;
        }

        private void Close_Click(object sender, EventArgs e)
        {
            groupBox4.Visible = false;
        }

        private void dgAdvVouhcers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtAdvVchNo.Text =  dgAdvVouhcers.Rows[dgAdvVouhcers.CurrentRow.Index].Cells["Voucher_No"].Value.ToString();
            txtAdvanceAmt.Text = dgAdvVouhcers.Rows[dgAdvVouhcers.CurrentRow.Index].Cells["Amount"].Value.ToString();
            if (cmbVocherType.Text == "Sale Voucher")
            {
                decimal ActAmt = Convert.ToDecimal(txtTotDebit.Text);
                //decimal advAmt = Convert.ToDecimal(txtAdvanceAmt.Text);
                decimal advAmt = (txtAdvanceAmt.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtAdvanceAmt.Text);

                decimal NetDue = ActAmt - advAmt;
                txtNetDue.Text = NetDue.ToString(".00");
            }
            else
            {
                decimal ActAmt = Convert.ToDecimal(txtTotCredit.Text);
               // decimal advAmt = Convert.ToDecimal(txtAdvanceAmt.Text);
                decimal advAmt = (txtAdvanceAmt.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtAdvanceAmt.Text);

                decimal NetDue = ActAmt - advAmt;
                txtNetDue.Text = NetDue.ToString(".00");
            }
                groupBox4.Visible = false;
        }

        private void txtAdvanceAmt_Leave(object sender, EventArgs e)
        {
            try
            {
                decimal ActAmt = Convert.ToDecimal(txtTotCredit.Text);
            
              //  decimal advAmt = Convert.ToDecimal(txtAdvanceAmt.Text);
                decimal advAmt = (txtAdvanceAmt.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtAdvanceAmt.Text);

                if (advAmt <= ActAmt)
                {
                    decimal NetDue = ActAmt - advAmt;
                    txtNetDue.Text = NetDue.ToString(".00");
                }
                else
                {
                    MessageBox.Show("Advance Adjustment Cannot be Greater Than Actual Bill Amount");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgRecipts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgAdvVouhcers_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void bindedit()
        {
            try
            {
                
               
                //cmbVocherType.Text = dgList.Rows[dgList.CurrentRow.Index].Cells["Voucher_No"].Value.ToString();
                String myString = "";
                myString = txtVchNo.Text;
                var da = (from obj in db.Account_Vouchers
                          where obj.Voucher_No == txtVchNo.Text  && obj.Voucher_Type == cmbVocherType.Text && obj.Company_ID == logIn.company && obj.BU_ID == logIn.BU_ID
                          select obj).ToList();

                if (da.Count > 0)
                {
                   
                    dpdate.Text = da[0].Voucher_Date.ToString();
                    txtDocRefNo.Text = da[0].Doc_Ref_No;                   
                    txtNarration.Text = da[0].Narration;
                    txtInstrumentNo.Text = da[0].Transaction_Ref_No;
                    dpinsmentsdate.Text = da[0].Transaction_Ref_Date.ToString();
                    txtTotDebit.Text = da[0].Tot_Debit.ToString();
                    txtTotCredit.Text = da[0].Tot_Credit.ToString();
                    //cmbStatus.SelectedValue = da[0].Status;
                    lblCreatedBy.Text = da[0].Created_By;
                    lblModified.Text = da[0].Modified_BY;
                    txtAdvVchNo.Text = da[0].Advance_Vch_No;
                    txtAdvanceAmt.Text = da[0].Advance_Amount.ToString() ;
                    txtNetDue.Text = da[0].Net_Due_Amount.ToString();


                }


                var dm1 = (from s in db.Account_Vouchers
                           where s.Voucher_No == txtVchNo.Text && s.Voucher_Type == cmbVocherType.Text && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID


                           select new

                           {
                               Acc_ID = s.Acc_ID,
                               AccName = s.AccName,
                               Debit_Amount = s.Debit_Amount,
                               Credit_Amount = s.Credit_Amount,
                               Balance_amount = s.Balance_amount,
                               s.Balance_Type     

                           });




                SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataTable dtr = new DataTable();
                da2.Fill(dtr);
                if (dtr.Rows.Count >= 0)
                    dgRecipts.DataSource = dtr;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ConfigVchrs()
        {
            try
            {
                string VchType = frmMain.menuName;
                switch (VchType)
                {
                    case "Sale Voucher":
                        label4.Text = "SALE VOUCHER";
                        cmbVocherType.Text = "Sale Voucher";
                        txtDocRefNo.Visible = true;
                        label5.Visible = true;
                       // btnGetDoc.Visible = true;
                        label12.Text = "Payment Terms";
                        label7.Text = "Payment Due Date";
                        label3.Visible = false;
                        linkLabel1.Visible = true;
                        txtAdvVchNo.Visible = true;
                        txtAdvanceAmt.Visible = true;
                        txtNetDue.Visible = true;
                        label10.Visible = false;
                        break;
                    case "Purchase Voucher":
                        label4.Text = "PURCHASE VOUCHER";
                        cmbVocherType.Text = "Purchase Voucher";
                        txtDocRefNo.Visible = true;
                        label5.Visible = true;
                       // btnGetDoc.Visible = true;
                        label12.Text = "Payment Terms";
                        label7.Text = "Payment Due Date";
                        label3.Visible = false;
                        linkLabel1.Visible = true;
                        txtAdvVchNo.Visible = true;
                        txtAdvanceAmt.Visible = true;
                        txtNetDue.Visible = true;
                        label10.Visible = false;
                        break;
                    case "Payment Voucher":
                        label4.Text = "PAYMENT VOUCHER";
                        cmbVocherType.Text = "Payment Voucher";
                        txtDocRefNo.Visible = false;
                        label5.Visible = false;
                       // btnGetDoc.Visible = false;
                        label12.Text = "Payment/Receipt Ref No";
                        label7.Text = "Date";
                        label3.Visible = true;
                        linkLabel1.Visible = false;
                        txtAdvVchNo.Visible = false;
                        txtAdvanceAmt.Visible = false;
                        txtNetDue.Visible = false;
                        label10.Visible = true;
                        label10.Text = "Press F3 to Select Bill Details";
                        break;

                    case "Receipt Voucher":
                        label4.Text = "RECEIPT VOUCHER";
                        cmbVocherType.Text = "Receipt Voucher";
                        txtDocRefNo.Visible = false;
                        label5.Visible = false;
                     //   btnGetDoc.Visible = false;
                        label12.Text = "Payment/Receipt Ref No";
                        label7.Text = "Date";
                        label3.Visible = true;
                        linkLabel1.Visible = false;
                        txtAdvVchNo.Visible = false;
                        txtAdvanceAmt.Visible = false;
                        txtNetDue.Visible = false;
                        label10.Visible = true;
                        label10.Text = "Press F3 to Select Bill Details";
                        break;

                    case "Journal Voucher":
                        label4.Text = "JOURNAL VOUCHER";
                        cmbVocherType.Text = "Journal Voucher";
                        txtDocRefNo.Visible = false;
                        label5.Visible = false;
                       // btnGetDoc.Visible = false;
                        label12.Visible = false;
                        label3.Visible = false;
                        label7.Visible = false;
                        txtInstrumentNo.Visible = false;
                        dpinsmentsdate.Visible = false;
                        linkLabel1.Visible = false;
                        txtAdvVchNo.Visible = false;
                        txtAdvanceAmt.Visible = false;
                        txtNetDue.Visible = false;
                        label10.Visible = true;
                        label10.Text = "Press F4 to Enter Bill Details";
                        break;
                    case "Credit Note":
                        label4.Text = "CREDIT NOTE";
                        cmbVocherType.Text = "Credit Note";
                        txtDocRefNo.Visible = true;
                        label5.Visible = true;
                      //  btnGetDoc.Visible = false;
                        label12.Visible = false;
                        label3.Visible = false;
                        label7.Visible = false;
                        txtInstrumentNo.Visible = false;
                        dpinsmentsdate.Visible = false;
                        linkLabel1.Visible = false;
                        txtAdvVchNo.Visible = false;
                        txtAdvanceAmt.Visible = false;
                        txtNetDue.Visible = false;
                        label10.Visible = false;
                        break;
                    case "Debit Note":
                        label4.Text = "DEBIT NOTE";
                        cmbVocherType.Text = "Debit Note";
                        txtDocRefNo.Visible = true;
                        label5.Visible = true;
                       // btnGetDoc.Visible = false;
                        label12.Visible = false;
                        label3.Visible = false;
                        label7.Visible = false;
                        txtInstrumentNo.Visible = false;
                        dpinsmentsdate.Visible = false;
                        linkLabel1.Visible = false;
                        txtAdvVchNo.Visible = false;
                        txtAdvanceAmt.Visible = false;
                        txtNetDue.Visible = false;
                        label10.Visible = false;
                        break;
                    case "Petty Cash Voucher":
                        label4.Text = "PETTY CASH VOUCHER";
                        cmbVocherType.Text = "Petty Cash Voucher";
                        txtDocRefNo.Visible = false;
                        label5.Visible = false;
                       // btnGetDoc.Visible = false;
                        label12.Text = "Payment/Receipt Ref No";
                        label7.Text = "Date";
                        label3.Visible = true;
                        linkLabel1.Visible = false;
                        txtAdvVchNo.Visible = false;
                        txtAdvanceAmt.Visible = false;
                        txtNetDue.Visible = false;
                        label10.Visible = true;
                        label10.Text = "Press F3 to Select Bill Details";
                        break;

                    case "Receipt Voucher - Cash":
                        label4.Text = "CASH RECEIPT VOUCHER";
                        cmbVocherType.Text = "Cash Receipt Voucher";
                        txtDocRefNo.Visible = false;
                        label5.Visible = false;
                       // btnGetDoc.Visible = false;
                        label12.Text = "Payment/Receipt Ref No";
                        label7.Text = "Date";
                        label3.Visible = true;
                        linkLabel1.Visible = false;
                        txtAdvVchNo.Visible = false;
                        txtAdvanceAmt.Visible = false;
                        txtNetDue.Visible = false;
                        label10.Visible = true;
                        label10.Text = "Press F3 to Select Bill Details";
                        break;
                        //case "Debit Note":
                        //     txtDocRefNo.Visible = false;
                        //     label5.Visible = false;
                        //     break;

                        // case "Credit Note":
                        //     txtDocRefNo.Visible = false;
                        //     label5.Visible = false;
                        //     break;


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
       
    }
}
