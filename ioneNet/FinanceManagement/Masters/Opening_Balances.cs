using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Diagnostics;
using Ione_DAL;

namespace ioneNet.FinanaceManagement.Masters
{

    public partial class Opening_Balances : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string SONo, LedgerName,  LedgerCode;
        public static decimal TotAmt;
        public Opening_Balances()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void clear()
        {
            try
            {

                foreach (Control x in this.Controls)
                {

                    foreach (Control d in groupBox3.Controls)
                    {
                        if (d is TextBox)
                            (d as TextBox).Clear();
                        if (d is ComboBox)
                            (d as ComboBox).SelectedIndex = -1;
                        if (d is CheckBox)
                            (d as CheckBox).Checked = false;
                    }
                }
                if (dgvJournalVouchar.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvJournalVouchar.Rows.Count - 1; i++)
                    {
                        dgvJournalVouchar.Rows.RemoveAt(i);
                        i--;
                        while (dgvJournalVouchar.Rows.Count == 0)
                            continue;
                    }
                }
                //cmbAccountName.Text = "";
                txtCreditAmtTotal.Text = txtDebitAmtTotal.Text = "";
                AutoincrementId();
                // txtTotalAmt.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void bindAct()
        {
            try
            {

                var bindcreditAct = (from m in db.AccountMasters where m.Company_ID == logIn.company select m.AccName).Distinct().ToList();


                //if (bindcreditAct.Count > 0)
                //{
                //    cmbAccountName.DataSource = bindcreditAct;

                //}
                //if (cmbAccountName.Items.Count > 0)
                //    cmbAccountName.SelectedIndex = -1;
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
                var auto = db.Sp_autoincrement_Fin_OpeningBalance(logIn.company);
                txtVoucherNo.Text = auto.FirstOrDefault().Voucher_No;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " While AutoIncrement Getting Error ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public string GetmaterialcodeBymaterialName(string Acc_Name)
        {
            var AccCode = (from s in db.AccountMasters
                           where s.AccName == Acc_Name //&& s.CompName == AppCode.GlobalAccess.companyName
                           select s.AccCode).FirstOrDefault();

            return Convert.ToString(AccCode);
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                //if (cmbAccountName.Text == "")
                //{
                //    MessageBox.Show("Account Name Should not be empty");
                //    return;
                //}
                Save();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void Save()
        {
            try
            {
                if ((from u in db.Account_Opening_Balances where u.Voucher_no == txtVoucherNo.Text && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    db.sp_DeleteOpeningBalance_Finance(txtVoucherNo.Text, logIn.company);

                    //var p1 = db.Account_Opening_Balances.Where(w => w.Voucher_no == txtVoucherNo.Text && w.Company_ID == logIn.company).FirstOrDefault();
                    //for (int i = 0; i < dgvJournalVouchar.Rows.Count - 1; i++)
                    //{
                    //    p1.Voucher_no = txtVoucherNo.Text;                            //p.Account = (cmbAccountName.Text == "") ? "" : cmbAccountName.Text;
                    //    p1.OB_date = AsAtdate.Value;
                    //    //p.OBDate = ObDate.Value;
                    //    p1.Remarks = (txtRemarks.Text == "") ? "" : txtRemarks.Text;
                    //    p1.Company_ID = logIn.company;
                    //    //p.Credit = (dgvJournalVouchar.Rows[i].Cells["Credit"].Value == DBNull.Value && dgvJournalVouchar.Rows[i].Cells["Credit"].Value == null && dgvJournalVouchar.Rows[i].Cells["Credit"].Value == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Credit"].Value);
                    //    p1.Credit_Amount = (dgvJournalVouchar.Rows[i].Cells["Credit"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Credit"].Value);
                    //    p1.Debit_Amount = (dgvJournalVouchar.Rows[i].Cells["Debit"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Debit"].Value);
                    //    //p.Debit = (dgvJournalVouchar.Rows[i].Cells["Debit"].Value == DBNull.Value && dgvJournalVouchar.Rows[i].Cells["Debit"].Value == null && dgvJournalVouchar.Rows[i].Cells["Debit"].Value == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Debit"].Value);
                    //    p1.Acc_ID = Convert.ToInt32(dgvJournalVouchar.Rows[i].Cells["Acc_ID"].Value.ToString());
                    //    p1.AccName = dgvJournalVouchar.Rows[i].Cells["AccName"].Value.ToString();

                    //    p1.Created_By = lblCreatedBy.Text;
                    //    p1.Modified_BY = logIn.username + "-" + DateTime.Now;
                    //    db.SubmitChanges();
                    //}

                    
                    //MessageBox.Show("Record Updated Successfully");
                    //clear();
                    //db.sp_DeleteOpeningBalance_Finance(txtVoucherNo.Text, logIn.company);
                }
                  //else
                  //  {
                       

                        for (int i = 0; i < dgvJournalVouchar.Rows.Count - 1; i++)
                        {
                            Account_Opening_Balance p = new Account_Opening_Balance();
                            p.Voucher_no = txtVoucherNo.Text;                            //p.Account = (cmbAccountName.Text == "") ? "" : cmbAccountName.Text;
                            p.OB_date = AsAtdate.Value;
                            //p.OBDate = ObDate.Value;
                            p.Remarks = (txtRemarks.Text == "") ? "" : txtRemarks.Text;
                            p.Company_ID = logIn.company;
                            //p.Credit = (dgvJournalVouchar.Rows[i].Cells["Credit"].Value == DBNull.Value && dgvJournalVouchar.Rows[i].Cells["Credit"].Value == null && dgvJournalVouchar.Rows[i].Cells["Credit"].Value == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Credit"].Value);
                            p.Credit_Amount = (dgvJournalVouchar.Rows[i].Cells["Credit"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Credit"].Value);
                            p.Debit_Amount = (dgvJournalVouchar.Rows[i].Cells["Debit"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Debit"].Value);
                            //p.Debit = (dgvJournalVouchar.Rows[i].Cells["Debit"].Value == DBNull.Value && dgvJournalVouchar.Rows[i].Cells["Debit"].Value == null && dgvJournalVouchar.Rows[i].Cells["Debit"].Value == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Debit"].Value);
                            p.Acc_ID = Convert.ToInt32(dgvJournalVouchar.Rows[i].Cells["Acc_ID"].Value.ToString());
                            p.AccName = dgvJournalVouchar.Rows[i].Cells["AccName"].Value.ToString();
                            p.BU_ID = logIn.BU_ID;
                            p.Created_By = lblCreatedBy.Text;
                            p.Modified_BY = logIn.username + "-" + DateTime.Now;

                            db.Account_Opening_Balances.InsertOnSubmit(p);

                        }

                        db.SubmitChanges();
                        MessageBox.Show("Record Updated Successfully");
                        clear();
                    //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                //if (AppCode.GlobalAccess.Edit == "Yes")
                //{
                    if (txtVoucherNo.Text != "")
                    {
                        if ((from u in db.Account_Opening_Balances where u.Voucher_no == txtVoucherNo.Text && u.Company_ID == logIn.company select u).Count() > 0)
                        {
                            var result = MessageBox.Show("Are You Sure Want to Delete this Record ", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                            if (result == DialogResult.Yes)
                            {
                                db.sp_DeleteOpeningBalance_Finance(txtVoucherNo.Text, logIn.company);

                                MessageBox.Show("Record Deleted Successfully");
                                clear();
                            }
                            else
                            {
                                return;
                            }

                        }
                        else
                        {
                            MessageBox.Show("This Record Not Exist,Please Try Another Record");
                            clear();

                        }

                    }
                    else
                        MessageBox.Show("Please Select the Record");
                //}
                //else
                //{
                //    MessageBox.Show("You dont Have Privileges", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);

                //    clear();
                //}
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void JournalVoucher_Load(object sender, EventArgs e)
        {
            try
            {
                //PictureBox2.Image = AppCode.GlobalAccess.comylogo;
                AutoincrementId();
                //Status
                var pStatus = (from m in db.AccountGroups where m.Company_ID == logIn.company select new { m.ID, m.GroupName }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbAccountGroup.DataSource = pStatus;
                    cmbAccountGroup.ValueMember = "ID";
                    cmbAccountGroup.DisplayMember = "GroupName";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvJournalVouchar_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                DataGridViewRow R1 = dgvJournalVouchar.Rows[dgvJournalVouchar.CurrentRow.Index];
                int columnIndex = dgvJournalVouchar.CurrentCell.ColumnIndex;
                string columnName = dgvJournalVouchar.Columns[columnIndex].Name;

                if ((R1.Cells["AccName"].Value) != "" && (R1.Cells["AccName"].Value) != null && (R1.Cells["AccName"].Value) != DBNull.Value)
                {
                    if ((from u in db.AccountMasters where u.AccName == R1.Cells["AccName"].Value select u).Count() == 0)
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

                        if (getProductName != null)
                        {

                            R1.Cells["Acc_ID"].Value = getProductName.id.ToString();                            
                        }





                    }
                }
                decimal Credit = 0, Debit = 0;
                for (int i = 0; i < dgvJournalVouchar.Rows.Count - 1; i++)
                {

                    Debit += (dgvJournalVouchar.Rows[i].Cells["Debit"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Debit"].Value);
                    Credit += (dgvJournalVouchar.Rows[i].Cells["Credit"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Credit"].Value);

                }
                txtDebitAmtTotal.Text = Debit.ToString();
                txtCreditAmtTotal.Text = Credit.ToString();


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
                //  bindCashAct();
                FinanaceManagement.Masters.OpeningBalanceSearch obj = new FinanaceManagement.Masters.OpeningBalanceSearch();

                if (obj.ShowDialog() == DialogResult.OK)
                {
                    txtVoucherNo.Text = FinanaceManagement.Masters.OpeningBalanceSearch.voucherNo;

                    if (!string.IsNullOrEmpty(txtVoucherNo.Text))
                    {
                        var dm1 = (from s in db.Account_Opening_Balances
                                       //  join a in db.AccountMasters on s.AccCode equals a.AccCode
                                   where s.Voucher_no == txtVoucherNo.Text && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID
                                   select new
                                   {
                                       s.Acc_ID,
                                       s.AccName,
                                       Debit = s.Debit_Amount,
                                       Credit = s.Credit_Amount
                                   }) ;

                        SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        DataTable dtr = new DataTable();
                        da2.Fill(dtr);
                        if (dtr.Rows.Count >= 0)
                            dgvJournalVouchar.DataSource = dtr;


                    }

                    var f = (from s in db.Account_Opening_Balances where s.Voucher_no == txtVoucherNo.Text && s.Company_ID == logIn.company select s).FirstOrDefault();
                    if (f != null)
                    {
                        AsAtdate.Text = f.OB_date.ToString();
                        //cmbAccountName.Text = f.Account;
                        txtRemarks.Text = f.Remarks;
                        //ObDate.Text = f.OBDate.ToString();
                        //txtCreditAmtTotal.Text = f.TotalCreditAmt.ToString();
                        //txtDebitAmtTotal.Text = f.TotalDebitAmt.ToString();

                    }
                    double Qty = 0, Amout = 0;

                    for (int j = 0; j < dgvJournalVouchar.Rows.Count; j++)
                    {

                        if (dgvJournalVouchar.Rows[j].Cells["Debit"].Value != null)
                        {

                            Qty += (dgvJournalVouchar.Rows[j].Cells["Debit"].Value == DBNull.Value) ? Convert.ToDouble("00") : Convert.ToDouble(dgvJournalVouchar.Rows[j].Cells["Debit"].Value);
                            Amout += (dgvJournalVouchar.Rows[j].Cells["Credit"].Value == DBNull.Value) ? Convert.ToDouble("00") : Convert.ToDouble(dgvJournalVouchar.Rows[j].Cells["Credit"].Value);


                        }

                    }
                    txtDebitAmtTotal.Text = Qty.ToString(".00");
                    txtCreditAmtTotal.Text = Amout.ToString(".00");

                }

            }
            catch (Exception ex)
            {

                  MessageBox.Show(ex.Message);
            }
        }
        private void Quantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)
                && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.'
                && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }
        private void dgvJournalVouchar_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                //if (e.Control is ComboBox)
                //{
                //    ComboBox box = e.Control as ComboBox;
                //    box.DropDownStyle = ComboBoxStyle.DropDown;
                //    box.AutoCompleteSource = AutoCompleteSource.ListItems;
                //    box.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                //}


                int columnIndex = dgvJournalVouchar.CurrentCell.ColumnIndex;
                string columnName = dgvJournalVouchar.Columns[columnIndex].HeaderText;

                e.Control.KeyPress -= new KeyPressEventHandler(Quantity_KeyPress);

                TextBox tb = e.Control as TextBox;
                if (tb != null && columnName == "Debit")
                {
                    tb.KeyPress += new KeyPressEventHandler(Quantity_KeyPress);
                }
                if (tb != null && columnName == "Credit")
                {
                    tb.KeyPress += new KeyPressEventHandler(Quantity_KeyPress);
                }


                TextBox tb3 = e.Control as TextBox;
                if (columnName == "Account Name")
                {
                    if (tb3 != null && columnName == "Account Name")
                    {
                        tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                        tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                        addItems(DataColl);
                        tb3.AutoCompleteCustomSource = DataColl;
                    }


                }
                else
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.None;
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
                DataGridViewRow R1 = dgvJournalVouchar.Rows[dgvJournalVouchar.CurrentRow.Index];

                int columnIndex = dgvJournalVouchar.CurrentCell.ColumnIndex;
                string columnName = dgvJournalVouchar.Columns[columnIndex].HeaderText;

                if (columnName == "Account Name")
                {
                    var Buyerbind = (from m in db.AccountMasters
                                     where m.Company_ID == logIn.company
                                     select new
                                     { m.AccName }).ToList();
                    //var Prodname = (from d in db.ProdMasters where d.Comp_Name == AppCode.GlobalAccess.companyName && d.Product_Name == R1.Cells["ProductName"].Value.ToString() select new { d.Product_Category }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("AccName");
                    foreach (var item in Buyerbind)
                    {
                        dt.Rows.Add(item.AccName);
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

        private void dgvJournalVouchar_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(dgvJournalVouchar.RowHeadersDefaultCellStyle.ForeColor))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(), e.InheritedRowStyle.Font, b, e.RowBounds.Location.X + 10, e.RowBounds.Location.Y + 4);
            }
        }

        private void PictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void dgvJournalVouchar_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // DataGridViewRow R1 = dgProductData.Rows[dgProductData.CurrentRow.Index];

                if (dgvJournalVouchar.Rows[dgvJournalVouchar.CurrentRow.Index].Cells[dgvJournalVouchar.CurrentCell.ColumnIndex].Value == "Remove")
                {
                    if (dgvJournalVouchar.Rows.Count > 0)
                    {
                        foreach (DataGridViewCell oneCell in dgvJournalVouchar.SelectedCells)
                        {
                            if (oneCell.Selected)
                                dgvJournalVouchar.Rows.RemoveAt(oneCell.RowIndex);
                        }

                        double Qty = 0, Amout = 0;

                        for (int j = 0; j < dgvJournalVouchar.Rows.Count; j++)
                        {

                            if (dgvJournalVouchar.Rows[j].Cells["Debit"].Value != null)
                            {

                                Qty += (dgvJournalVouchar.Rows[j].Cells["Debit"].Value == DBNull.Value) ? Convert.ToDouble("00") : Convert.ToDouble(dgvJournalVouchar.Rows[j].Cells["Debit"].Value);
                                Amout += (dgvJournalVouchar.Rows[j].Cells["Credit"].Value == DBNull.Value) ? Convert.ToDouble("00") : Convert.ToDouble(dgvJournalVouchar.Rows[j].Cells["Credit"].Value);


                            }

                        }
                        txtDebitAmtTotal.Text = Qty.ToString(".00");
                        txtCreditAmtTotal.Text = Amout.ToString(".00");

                        

                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dgvJournalVouchar_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnImport_Click(object sender, EventArgs e)
        {

        }

        private void cmdPrint_Click(object sender, EventArgs e)
        {
            try
            {

                if (dgvJournalVouchar.Rows.Count > 0)
                {
                    ExportToExcel(dgvJournalVouchar, "Opening Balances");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGetAccounts_Click(object sender, EventArgs e)
        {
            if (chkAllAccounts.Checked)
            {
                var dm1 = (from s in db.View_Account_OpeningBals
                               //  join a in db.AccountMasters on s.AccCode equals a.AccCode
                           where  s.Company_ID == logIn.company
                           select new
                           {
                               s.Acc_ID,
                               s.AccName,
                               s.Debit,
                               s.Credit
                           });
                SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataTable dtr = new DataTable();
                da2.Fill(dtr);
                if (dtr.Rows.Count >= 0)
                    dgvJournalVouchar.DataSource = dtr;
            }
            else
            {
                var dm1 = (from s in db.View_Account_OpeningBals
                               //  join a in db.AccountMasters on s.AccCode equals a.AccCode
                           where s.GroupName == cmbAccountGroup.Text &&  s.Company_ID == logIn.company
                           select new
                           {
                               s.Acc_ID,
                               s.AccName,
                               s.Debit,
                               s.Credit
                           });
                SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataTable dtr = new DataTable();
                da2.Fill(dtr);
                if (dtr.Rows.Count >= 0)
                    dgvJournalVouchar.DataSource = dtr;
            }
            double Qty = 0, Amout = 0;

            for (int j = 0; j < dgvJournalVouchar.Rows.Count; j++)
            {

                if (dgvJournalVouchar.Rows[j].Cells["Debit"].Value != null)
                {

                    Qty += (dgvJournalVouchar.Rows[j].Cells["Debit"].Value == DBNull.Value) ? Convert.ToDouble("00") : Convert.ToDouble(dgvJournalVouchar.Rows[j].Cells["Debit"].Value);
                    Amout += (dgvJournalVouchar.Rows[j].Cells["Credit"].Value == DBNull.Value) ? Convert.ToDouble("00") : Convert.ToDouble(dgvJournalVouchar.Rows[j].Cells["Credit"].Value);


                }

            }
            txtDebitAmtTotal.Text = Qty.ToString(".00");
            txtCreditAmtTotal.Text = Amout.ToString(".00");

        }

        private void dgvJournalVouchar_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F3)
                {
                    
                    ioneNet.FinanceManagement.Masters.AccountOBBillWise form = new ioneNet.FinanceManagement.Masters.AccountOBBillWise();
                    //ioneNet.Masters.ProdSearch.frmName = "SOrder";       
                    int i = dgvJournalVouchar.CurrentCell.RowIndex;
                   // SONo = "OB";
                    LedgerCode = dgvJournalVouchar.Rows[i].Cells["Acc_ID"].Value.ToString();
                    LedgerName = dgvJournalVouchar.Rows[i].Cells["AccName"].Value.ToString();
                    TotAmt = 0;
                    decimal dbtAmt = (dgvJournalVouchar.Rows[i].Cells["Debit"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Debit"].Value);
                    decimal creditAmt = (dgvJournalVouchar.Rows[i].Cells["Credit"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Credit"].Value);
                    if (dbtAmt > 0)
                    {
                        TotAmt = dbtAmt;
                    }
                    else
                    {
                        if (creditAmt > 0)
                        {
                            TotAmt = creditAmt;

                        }
                    }
                    if (TotAmt > 0)
                    {
                        form.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Enter Opening Balance Amount To Proceed for Bill Wise Entry");
                        return;
                    }
                    //dgvJournalVouchar.Rows[i].Cells["OB_Stock_Wt"].Value = ioneNet.OrderManagement.Transactions.ProdSpecs.TotQty;
                }
                if (e.KeyCode == Keys.F6) //Remove Rows
                {
                    if (dgvJournalVouchar.Rows.Count > 0)
                    {

                        foreach (DataGridViewCell oneCell in dgvJournalVouchar.SelectedCells)
                        {
                            if (oneCell.Selected)
                                dgvJournalVouchar.Rows.RemoveAt(oneCell.RowIndex);
                        }
                        decimal Credit = 0, Debit = 0;
                        for (int i = 0; i < dgvJournalVouchar.Rows.Count - 1; i++)
                        {

                            Debit += (dgvJournalVouchar.Rows[i].Cells["Debit"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Debit"].Value);
                            Credit += (dgvJournalVouchar.Rows[i].Cells["Credit"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Credit"].Value);

                        }
                        txtDebitAmtTotal.Text = Debit.ToString();
                        txtCreditAmtTotal.Text = Credit.ToString();

                    }

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        
        }

        public void ExportToExcel(DataGridView gridviewID, string excelFilename)
        {
            try
            {

                //string path = Path.Combine(Directory.GetCurrentDirectory(), "" + excelFilename + ".xlsx");
                //Excel.Application xlAppToExport = new Excel.Application();
                //xlAppToExport.Workbooks.Add("");

                //// ADD A WORKSHEET.
                //Excel.Worksheet xlWorkSheetToExport = default(Excel.Worksheet);
                //xlWorkSheetToExport = (Excel.Worksheet)xlAppToExport.Sheets["Sheet1"];

                //FileInfo file = new FileInfo(path);
                //if (file.Exists)//check file exsit or not
                //{
                //    file.Delete();
                //}

                //int iRowCnt = 7;
                //var data = (from s in db.Company_Infos
                //            where s.Id == logIn.company
                //            select new
                //            {
                //                s.Company_Name,
                //                Company_address = s.Address + ',' + s.City + ',' + s.State + ',' + s.Phone_No + ',' + s.E_Mail + ',' + s.Website + '.'
                //            }).ToList();


                //xlWorkSheetToExport.Cells[1, 1] = data[0].Company_Name.ToString();
                //Excel.Range range = xlWorkSheetToExport.Cells[1, 1] as Excel.Range;
                //range.EntireRow.Font.Name = "Calibri";
                //range.EntireRow.Font.Bold = true;
                //range.EntireRow.Font.Size = 12;
                ////xlWorkSheetToExport.Range["A1:M1"].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                ////xlWorkSheetToExport.Range["A1:M1"].MergeCells = true;       // MERGE CELLS OF THE HEADER.                
                //xlWorkSheetToExport.Cells[2, 1] = "Opening Balances";


                ////xlWorkSheetToExport.Cells[4, 1] = "Account Name" + cmbAccName.Text;
                ////Excel.Range range1 = xlWorkSheetToExport.Cells[2, 1] as Excel.Range;
                ////range1.EntireRow.Font.Name = "Calibri";
                ////range1.EntireRow.Font.Bold = false;
                ////range1.EntireRow.Font.Size = 12;
                ////range1.RowHeight = 20;
                ////xlWorkSheetToExport.Range["A2:M2"].WrapText = true;
                ////xlWorkSheetToExport.Range["A2:M2"].MergeCells = true;
                //// SHOW THE HEADER File Name
                //string d = "";
                //// SHOW THE HEADER File Name

                //// string d = " From Date :" +dpFromDate.Text +",      TO Date :"+ (dpTodate.Text) +",      Customer Name :"+ txtCust_Prod_code.Text + "  ,  Product Name :"+txtProductName.Text;
                //xlWorkSheetToExport.Cells[4, 1] = d;
                //Excel.Range range5 = xlWorkSheetToExport.Cells[2, 1] as Excel.Range;
                //range5.EntireRow.Font.Name = "Calibri";
                ////  range5.EntireRow.Font.Bold = true;
                //range5.EntireRow.Font.Size = 12;
                ////xlWorkSheetToExport.Range["A5:M5"].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                ////xlWorkSheetToExport.Range["A5:M5"].WrapText = true;
                ////xlWorkSheetToExport.Range["A5:M5"].MergeCells = true;
                //// MERGE CELLS OF THE HEADER.

                //for (int i = 1; i < gridviewID.Columns.Count + 1; i++)
                //{
                //    xlWorkSheetToExport.Cells[6, i] = gridviewID.Columns[i - 1].HeaderText;
                //}

                //for (int i = 0; i < gridviewID.Rows.Count; i++)
                //{
                //    for (int j = 0; j < gridviewID.Columns.Count; j++)
                //    {
                //        if (gridviewID.Rows[i].Cells[j].Value != null)
                //        {
                //            Excel.Range range7 = xlWorkSheetToExport.Cells[i + 7, j + 1] as Excel.Range;

                //            range7.NumberFormat = "@";

                //            xlWorkSheetToExport.Cells[i + 7, j + 1] = gridviewID.Rows[i].Cells[j].Value.ToString();

                //        }
                //    }
                //}

                //xlWorkSheetToExport.Columns.AutoFit();
                //xlAppToExport.DisplayAlerts = false;
                //xlWorkSheetToExport.SaveAs(path);
                //// CLEAR.
                //xlAppToExport.Workbooks.Close();
                //xlAppToExport.Quit();
                //xlAppToExport = null;
                //xlWorkSheetToExport = null;
                //Process.Start(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
