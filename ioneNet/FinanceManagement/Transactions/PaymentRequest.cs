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
    public partial class PaymentRequest : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        System.Data.Common.DbTransaction transaction;
        SqlConnection objSqlConnection;
        SqlCommand objSqlCommand;
        public static string vochertype = "",vchno,custname;
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

        public PaymentRequest()
        {
            InitializeComponent();
        }

        public void clear()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                cmbPartyName.SelectedIndex = -1;
                cmbPaymentType.Text = "";
                cmbDocRefType.Text = "";
                txtTotDebit.Text = "";
                txtNarration.Text = "";
                txtBankDetails.Text = "";
                cmbPaymentMode.Text = "";
                cmbStatus.Text = "";             
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
                AutoincrementId();
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

                var d = (from c in db.Purchase_Order_Masters                         
                         where c.SupplierName == Convert.ToInt32(txtSuppID.Text) && c.Company_ID == logIn.company  select new { c.PO_NO }).ToList();

                DataTable dt = new DataTable();
                dt.Columns.Add("d");
                foreach (var item in d)
                {
                    dt.Rows.Add(item.PO_NO);
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
                var d = (from po in db.AccountMasters                         
                         where po.Company_ID == logIn.company
                         select new { po.id, po.AccName }).Distinct().ToList();
                if (d.Count > 0)
                {
                    cmbPartyName.DataSource = d;
                    cmbPartyName.ValueMember = "id";
                    cmbPartyName.DisplayMember = "AccName";
                }
                lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

                editmode = false;

                if (FinanceManagement.Transactions.PaymentRequestList .var == "0" || FinanceManagement.Transactions.PaymentRequestList.var == "1")
                {
                    if (FinanceManagement.Transactions.PaymentRequestList.editMode == true)
                    {
                        txtVchNo.Text = FinanceManagement.Transactions.PaymentRequestList.SO_No;
                        editmode = true;

                        bindedit();
                    }
                    else
                    {
                        AutoincrementId();
                    }
                }
                else          
                {
                    AutoincrementId();
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

                var result = db.Sp_autoincrement_PaymentRequest(logIn.company, logIn.BU_ID, logIn.fy_Start_Date, logIn.fy_End_Date);
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
                if ((from u in db.Payment_Request_Masters where u.Voucher_No == myString && u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID select u).Count() > 0)
                {
                    myString = txtVchNo.Text;
                    db.sp_PaymentRequest_Delete(myString, logIn.company,logIn.BU_ID);

                }
                else
                {
                    AutoincrementId();
                    myString = txtVchNo.Text;

                }

                Payment_Request_Master S = new Payment_Request_Master();
                {
                    S.Voucher_No = myString;
                    S.Voucher_Date = dpdate.Value;
                    S.Payment_Type = cmbPaymentType.Text;
                    S.Doc_Type = cmbDocRefType.Text;
                    S.Status = Convert.ToInt32(cmbStatus.SelectedValue.ToString());
                    S.Total_Amunt = (txtTotDebit.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotDebit.Text);

                    S.Narration = (txtNarration.Text == "") ? "" : txtNarration.Text;
                    S.Supplier_ID = Convert.ToInt32(txtSuppID.Text);
                    S.Comments = (txtComments.Text == "") ? "" : txtComments.Text;
                    //Save Child Data
                    S.Party_Name = Convert.ToInt32(cmbPartyName.SelectedValue);
                    S.Payment_Mode = cmbPaymentMode.Text;
                    S.Party_Bank_Details = txtBankDetails.Text;
                    S.Status = Convert.ToInt32(cmbStatus.SelectedValue);
                    S.Company_ID = logIn.company;
                    S.BU_ID = logIn.BU_ID;
                    S.Created_By = lblCreatedBy.Text;
                    S.Modified_BY = logIn.username + "-" + DateTime.Now;
                    db.Payment_Request_Masters.InsertOnSubmit(S);
                    db.SubmitChanges();
                }
                //if (frmGate.Modify.Contains(this.Text))
                //{

                // dgProducts.Enabled = false;
                //var S = db.Sale_Order_Masters.Where(w => w.SO_NO == myString && w.Company_ID == logIn.company).FirstOrDefault();
                for (int i = 0; i < dgRecipts.RowCount - 1; i++)
                {
                    Payment_Request_Child c = new Payment_Request_Child();
                    {
                        if (Convert.ToDecimal(dgRecipts.Rows[i].Cells["Amount_Release"].Value) > 0)
                        {
                            var d1 = (from a in db.Payment_Request_Masters where a.Voucher_No == myString && a.Company_ID == logIn.company && a.BU_ID == logIn.BU_ID select new { a.id }).ToList();


                            c.Pymt_Req_Master_ID = d1[0].id;
                            c.Bill_No = (dgRecipts.Rows[i].Cells["Bill_No"].Value == null) ? "" : (dgRecipts.Rows[i].Cells["Bill_No"].Value).ToString();
                            c.Bill_Date = (dgRecipts.Rows[i].Cells["Bill_Date"].Value == null) ? "" : (dgRecipts.Rows[i].Cells["Bill_Date"].Value).ToString();
                            c.Bill_Amount = (dgRecipts.Rows[i].Cells["Bill_Amount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Bill_Amount"].Value);
                            c.Due_Amount = (dgRecipts.Rows[i].Cells["Due_Amount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Due_Amount"].Value);

                            c.Due_Date = (dgRecipts.Rows[i].Cells["Due_Date"].Value == null) ? "" : (dgRecipts.Rows[i].Cells["Due_Date"].Value).ToString();
                            c.Amt_To_Release = (dgRecipts.Rows[i].Cells["Amount_Release"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Amount_Release"].Value);
                            c.Remarks = (dgRecipts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgRecipts.Rows[i].Cells["Remarks"].Value).ToString();

                            c.Company_ID = logIn.company;

                            db.Payment_Request_Childs.InsertOnSubmit(c);
                            db.SubmitChanges();
                        }
                    }

                    //Saving into Customer Received Bill With Status as Requested
                    if (cmbDocRefType.Text == "Supplier Invoice")
                    {
                        Cust_Received_Bill CB = new Cust_Received_Bill();
                        {
                            if (Convert.ToDecimal(dgRecipts.Rows[i].Cells["Amount_Release"].Value) > 0)
                            {
                                CB.Voucher_No = txtVchNo.Text;
                                CB.Bill_No = dgRecipts.Rows[i].Cells["Bill_No"].Value.ToString();
                                // MessageBox.Show(CB.Bill_No);
                                string bDate = dgRecipts.Rows[i].Cells["Bill_Date"].Value.ToString();
                                DateTime bdate = DateTime.ParseExact(bDate, "dd/MM/yyyy", null);
                                CB.Bill_Date = bdate;
                                CB.Bill_Value = Convert.ToDecimal(dgRecipts.Rows[i].Cells["Bill_Amount"].Value);
                                CB.Due_Amount = Convert.ToDecimal(dgRecipts.Rows[i].Cells["Due_Amount"].Value);
                                CB.AmtReceived = Convert.ToDecimal(dgRecipts.Rows[i].Cells["Amount_Release"].Value);
                                CB.Company_ID = logIn.company;
                                CB.Customer_ID = Convert.ToInt32(cmbPartyName.SelectedValue);
                                CB.Status = "Request Raised";
                                db.Cust_Received_Bills.InsertOnSubmit(CB);
                                db.SubmitChanges();
                            }
                        }
                    }
                }

                

                db.SubmitChanges();              
                
                clear();                
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
                    
                    db.sp_PaymentRequest_Delete(txtVchNo.Text, logIn.company, logIn.BU_ID);

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
                MessageBox.Show(ex.Message + "Record Not Delete", "PAYMENT REQUEST", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                if (tb != null && columnName == "Bill_No")
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
                int columnIndex = dgRecipts.CurrentCell.ColumnIndex;
                string columnName = dgRecipts.Columns[columnIndex].Name;
                if (columnName == "Bill_No")
                {
                    if (R1.Cells["Bill_No"].Value != null)
                    {

                        var getProductName = (from s in db.Purchase_Order_Masters                                              
                                              where s.PO_NO == R1.Cells["Bill_No"].Value.ToString() && s.SupplierName == Convert.ToInt32(txtSuppID.Text) && s.Company_ID == logIn.company
                                              select new { s.PO_Date, s.Tot_Ord_Value }).FirstOrDefault();

                        if (getProductName != null)
                        {
                            R1.Cells["Bill_Date"].Value = getProductName.PO_Date.ToString();                            
                            R1.Cells["Bill_Amount"].Value = getProductName.Tot_Ord_Value.ToString();
                            R1.Cells["Due_Amount"].Value = getProductName.Tot_Ord_Value.ToString();
                            R1.Cells["Due_Date"].Value = getProductName.PO_Date.ToString();
                            //if (R1.Cells["Product_Descr"].Value == "" || R1.Cells["Product_Descr"].Value == DBNull.Value || R1.Cells["Product_Descr"].Value == null)
                            //{
                            //    R1.Cells["Product_Descr"].Value = getProductName.Product_Descr.ToString();
                            //}
                        }
                        else
                        {
                            //MessageBox.Show("Invalid PO No Entered");
                            //return;
                        }
                    }
                }
                else
                {


                    //DataGridViewRow R1 = dgRecipts.Rows[dgRecipts.CurrentRow.Index];
                    decimal Amt1 = 0, Amt2 = 0;
                    Amt1 = (R1.Cells["Amount_Release"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(R1.Cells["Amount_Release"].Value);
                    Amt2 = (R1.Cells["Due_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(R1.Cells["Due_Amount"].Value);

                    if (Amt1 > Amt2)
                    {
                        MessageBox.Show("Amount To Release Cannot Be Greater Than Amount Due");
                        R1.Cells["Amount_Release"].Value = "0";
                        return;

                    }

                    decimal Credit = 0, debit = 0;
                    for (int i = 0; i < dgRecipts.Rows.Count - 1; i++)
                    {


                        debit += //((dgRecipts.Rows[i].Cells["Debit"].Value == DBNull.Value) && (dgSelectedocument.Rows[i].Cells["Debit"].Value == "") && (dgSelectedocument.Rows[i].Cells["Debit"].Value == null)) ? Convert.ToDecimal("0.00") : Convert.ToDecimal(dgSelectedocument.Rows[i].Cells["Debit"].Value);//
                            (dgRecipts.Rows[i].Cells["Amount_Release"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Amount_Release"].Value);
                    }
                    if (debit == 0)
                    {

                    }
                    else
                    {
                        //  (R1.Cells["Debit"].Value) = debit;
                        txtTotDebit.Text = debit.ToString(".00");

                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void bindedit()
        {
            try
            {
               
                String myString = "";
                int POMasterID = 0;               
                var da = (from obj in db.Payment_Request_Masters
                          where obj.Voucher_No == txtVchNo.Text && obj.Company_ID == logIn.company && obj.BU_ID == logIn.BU_ID
                          select obj).ToList();

                if (da.Count > 0)
                {
                    POMasterID = da[0].id;                  
                    dpdate.Text = da[0].Voucher_Date.ToString();
                    //bindCustomer();
                    cmbPartyName.SelectedValue = da[0].Party_Name;
                    cmbPaymentType.Text = da[0].Payment_Type;
                    cmbDocRefType.Text = da[0].Doc_Type;
                    txtTotDebit.Text = da[0].Total_Amunt.ToString();                    

                    //cmbCustomer.Enabled = false;
                    txtNarration.Text = da[0].Narration;
                    txtComments.Text = da[0].Comments;
                    txtSuppID.Text = da[0].Supplier_ID.ToString();
                    txtBankDetails.Text = da[0].Party_Bank_Details.ToString();
                    cmbPaymentMode.Text = da[0].Payment_Mode.ToString();                    
                    cmbStatus.SelectedValue = da[0].Status;
                    lblCreatedBy.Text = da[0].Created_By;
                    lblModified.Text = da[0].Modified_BY;
                }


                var dm1 = (from s in db.Payment_Request_Childs

                           where s.Pymt_Req_Master_ID == POMasterID
                           select new

                           {
                               s.Bill_No,
                               s.Bill_Date,
                               s.Bill_Amount,
                               s.Due_Amount,
                                s.Due_Date,
                               Amount_Release = s.Amt_To_Release,
                               s.Remarks,
                               
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
















        public string sss;
        public string name1;
        private void dgRecipts_KeyDown(object sender, KeyEventArgs e)
        {
            try

            {
                decimal Credit = 0, debit = 0;
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
                                (dgRecipts.Rows[i].Cells["Amount_Release"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgRecipts.Rows[i].Cells["Amount_Release"].Value);
                           


                        }
                        if (debit == 0)
                        {

                        }
                        else
                        {
                            //  (R1.Cells["Debit"].Value) = debit;
                            txtTotDebit.Text = debit.ToString(".00");
                            
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
                //vochertype = cmbVocherType.Text;
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
            try
            {

                if (cmbPartyName.Text != "")
                {
                    if (cmbDocRefType.Text == "Supplier Invoice")
                    {
                        DataTable dt = new DataTable();
                        //String conStr = "Data Source=172.16.6.173;Initial Catalog=servion_hari;User  ID=sa;Password=Servion@123";
                        //SqlConnection con = new SqlConnection(con);
                        DateTime t = dpdate.Value;
                        string t1 = t.ToString("dd/MMM/yyyy");
                        SqlCommand com = new SqlCommand("[Payable_Bills_To_Request]", con);
                        com.Parameters.AddWithValue("@compname", logIn.company);
                        com.Parameters.AddWithValue("@partyname", Convert.ToInt32(cmbPartyName.SelectedValue));
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
                    }

                    if (cmbDocRefType.Text == "Purchase Order")
                    {
                        
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
            rep.RecordSelectionFormula = "{ Account_Voucher.Voucher_No} = '" + txtVchNo.Text + "' and { Account_Voucher.Company_ID} = " + logIn.company + "";
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
                    }

        private void Close_Click(object sender, EventArgs e)
        {
           
        }

        private void dgAdvVouhcers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void txtAdvanceAmt_Leave(object sender, EventArgs e)
        {
           
        }

        private void cmbPartyName_Leave(object sender, EventArgs e)
        {
            if (cmbPartyName.Text != "")
            {
                var da = (from obj in db.Supplier_informations                        

                          join es in db.AccountMasters on new { x1 = obj.Supplier_Id, x2 = obj.Company_ID } equals new { x1 = es.AccCode, x2 = es.Company_ID }

                          where es.id == Convert.ToInt32(cmbPartyName.SelectedValue) && obj.Company_ID == logIn.company
                          select new {obj.BankName,SuppID = obj.ID }).ToList();

                if (da.Count > 0)
                {
                    txtBankDetails.Text = da[0].BankName;
                    txtSuppID.Text = da[0].SuppID.ToString();
                }
            }
        }

        private void cmbDocRefType_Leave(object sender, EventArgs e)
        {
            btnGetDoc.Enabled = false;
            if (cmbDocRefType.Text == "Supplier Invoice")
            {
                btnGetDoc.Enabled = true;   
            }
        }

       
    }
}
