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
//using DAL;
//using BAL;
using System.Diagnostics;

namespace ioneNet.MaterialManagement
{
    public partial class frmPurchaseInvoice : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmPurchaseInvoice()
        {
            InitializeComponent();
        }

        private void frmPurchaseInvoice_Load(object sender, EventArgs e)
        {
            cmbSupplier.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cmbSupplier.AutoCompleteSource = AutoCompleteSource.ListItems;
           
            bindSupplier();
            AutoincrementId();
            //if (PurchaseVouchersList.editMode == true)
            //{
            //    bindedit();
            //}
        }
        public void bindSupplier()
        {
            try
            {
                var Buyerblind = (from m in db.Supplier_Informations  select new { m.Supplier_Id, m.Supplier_Name }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    cmbSupplier.DataSource = Buyerblind;
                    cmbSupplier.ValueMember = "Supplier_Id";
                    cmbSupplier.DisplayMember = "Supplier_Name";                   

                }
                if (cmbSupplier.Items.Count > 0)
                    cmbSupplier.SelectedIndex = -1;
               
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
                
                //var result = db.Sp_autoincrement_PurchaseInvoice(Creation_Company);
                //txtInvoiceno.Text = result.FirstOrDefault().Voucher_No;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbSupplier_Leave(object sender, EventArgs e)
        {
            try
            {
                var t = cmbSupplier.Text;
                var GSTNo = (from m in db.Supplier_Informations
                             where m.Supplier_Name == t
                             select new
                             {
                                 m.TIN_NO,
                             }).ToList();
                if (GSTNo.Count > 0)
                {
                    //for (int i = 0; i < GSTNo.Count - 1; i++)
                    //{

                    txtGSTINNO.Text = GSTNo[0].TIN_NO;                   



                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        private void dgvInvoice_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {

                int columnIndex = dgvInvoice.CurrentCell.ColumnIndex;
                int rowindex = dgvInvoice.CurrentCell.RowIndex;
                string columnName = dgvInvoice.Columns[columnIndex].HeaderText;

                TextBox tb3 = e.Control as TextBox;
                if (columnName == "Item Description")
                {
                    if (tb3 != null && columnName == "Item Description")
                    {
                        tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                        tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                        addItems(DataColl);
                        tb3.AutoCompleteCustomSource = DataColl;
                    }
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
                DataGridViewRow R1 = dgvInvoice.Rows[dgvInvoice.CurrentRow.Index];

                int columnIndex = dgvInvoice.CurrentCell.ColumnIndex;

                string columnName = dgvInvoice.Columns[columnIndex].HeaderText;

                
                    var Pname = (from d in db.Product_Masters select new { d.Item_Name }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Item_Name");
                    foreach (var item in Pname)
                    {
                        dt.Rows.Add(item.Item_Name);
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
        public static string Mid(string s, int a, int b)
        {
            string temp = s.Substring(a - 1, b);
            return temp;
        }
        private void dgvInvoice_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string comnpstatecode, suppStateCode;
                DataGridViewRow R1 = dgvInvoice.Rows[dgvInvoice.CurrentRow.Index];
                int columnIndex = dgvInvoice.CurrentCell.ColumnIndex;
                string columnName = dgvInvoice.Columns[columnIndex].Name;
                if (columnName == "Item_Name" && R1.Cells["Item_Name"].Value != null)
                {
                    var getProduct_Name = (from s in db.Product_Masters where s.Item_Name == R1.Cells["Item_Name"].Value.ToString() select s).FirstOrDefault();

                    if (getProduct_Name != null)
                    {
                        R1.Cells["Item_Code"].Value = getProduct_Name.Item_Code.ToString();
                        var getuom = (from s in db.UOM_Masters where s.UOM_ID == getProduct_Name.UOM_ID select s).FirstOrDefault();
                        if (getuom != null)
                        {
                            R1.Cells["uom"].Value = getuom.UOM;
                        }
                        int taxRate = Convert.ToInt32(getProduct_Name.GSTRate);
                        decimal b, c, d;
                        var d1 = (from a in db.Company_Informations where a.Company_ID == Creation_Company select new { a.GST_No }).ToList();
                        if (d1.Count > 0)
                        {
                            comnpstatecode = Mid(d1[0].GST_No, 1, 2);
                            suppStateCode = Mid(txtGSTINNO.Text, 1, 2);
                            if (suppStateCode == comnpstatecode)
                            {
                                d = Convert.ToDecimal(taxRate) / 2;
                                R1.Cells["CGST_Per"].Value = d.ToString("0.00");
                                R1.Cells["SGST_Per"].Value = d.ToString("0.00");
                                R1.Cells["IGST_Per"].Value = "0.00";

                            }
                            else
                            {
                                d = taxRate;
                                R1.Cells["CGST_Per"].Value = "0.00";
                                R1.Cells["SGST_Per"].Value = "0.00";
                                R1.Cells["IGST_Per"].Value = d.ToString("0.00");
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Product Name");
                        R1.Cells["Item_Name"].Value = "";
                        dgvInvoice.CurrentCell = dgvInvoice.Rows[dgvInvoice.CurrentRow.Index].Cells["Item_Name"];
                        dgvInvoice.CurrentCell.Selected = true;

                    }
                }
                else if (columnName == "Item_Code" && R1.Cells["Item_Code"].Value != null)
                {
                    var getProduct_Name = (from s in db.Product_Masters where s.Item_Code == R1.Cells["Item_Code"].Value.ToString() select s).FirstOrDefault();

                    if (getProduct_Name != null)
                    {
                        R1.Cells["Item_Name"].Value = getProduct_Name.Item_Name.ToString();
                        var getuom = (from s in db.UOM_Masters where s.UOM_ID == getProduct_Name.UOM_ID select s).FirstOrDefault();
                        if (getuom != null)
                        {
                            R1.Cells["uom"].Value = getuom.UOM;
                           
                        }
                        
                        
                    }
                }

                if (R1.Cells["Item_Code"].Value != null && R1.Cells["Item_Name"].Value != null)
                {
                    decimal ReceivedQty = (R1.Cells["Qty_Received"].Value == "" || R1.Cells["Qty_Received"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty_Received"].Value);
                    decimal RejectedQty = (R1.Cells["Qty_Rejected"].Value == "" || R1.Cells["Qty_Rejected"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty_Rejected"].Value);

                    string Custstatetcode;
                    if (columnName == "Qty_Received" || columnName == "Qty_Rejected")
                    {
                        if (ReceivedQty > 0)
                        {
                            if (ReceivedQty >= RejectedQty)
                            {
                                R1.Cells["Qty_Accepted"].Value = ReceivedQty - RejectedQty;
                            }
                            else
                            {
                                MessageBox.Show("Rejecred Qty Cannot be Greater Than Received Qty");
                                R1.Cells["Qty_Rejected"].Value ="0.00" ;
                            }
                            //decimal R = Convert.ToDecimal(R1.Cells["Rate"].Value) / Convert.ToDecimal(getProduct_Name.Uom_Per_EachUnit);
                            //R1.Cells["price"].Value = R.ToString("0.00");                            

                        }

                    }

                    if (columnName == "price" || columnName=="Discper")
                    {
                        if (ReceivedQty > 0)
                        {
                            decimal AcceptedQty = (R1.Cells["Qty_Accepted"].Value == "" || R1.Cells["Qty_Accepted"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty_Accepted"].Value);
                            decimal price = (R1.Cells["price"].Value == "" || R1.Cells["price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["price"].Value);
                            decimal DiscPer = (R1.Cells["Discper"].Value == "" || R1.Cells["Discper"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Discper"].Value);

                               
                                decimal Amt, DiscAmt, netAmt, gst,igst,totamt;

                                Amt = AcceptedQty * price;
                               
                                R1.Cells["AmtBeforeDisc"].Value = Amt.ToString("0.00");
                                DiscAmt = (Amt * DiscPer) / 100;
                                R1.Cells["DiscAmount"].Value = DiscAmt.ToString("0.00");
                                netAmt = Amt - DiscAmt;
                                R1.Cells["Amount"].Value = netAmt.ToString("0.00");
                                gst = (Convert.ToDecimal(R1.Cells["Amount"].Value)* Convert.ToDecimal(R1.Cells["CGST_Per"].Value))/100;

                                R1.Cells["CGST_Amnt"].Value = gst;
                                R1.Cells["SGST_Amnt"].Value = gst;
                                igst = (Convert.ToDecimal(R1.Cells["Amount"].Value) * Convert.ToDecimal(R1.Cells["IGST_Per"].Value)) / 100;
                                R1.Cells["IGST_Amnt"].Value = igst;

                                totamt = Math.Round(netAmt + gst + gst+igst);
                                R1.Cells["TotalAmount"].Value = totamt;
                            decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
                            for (int i = 0; i < dgvInvoice.Rows.Count - 1; i++)
                            {

                                x += (dgvInvoice.Rows[i].Cells["Qty_Accepted"].Value == "" || dgvInvoice.Rows[i].Cells["Qty_Accepted"].Value == null || dgvInvoice.Rows[i].Cells["Qty_Accepted"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["Qty_Accepted"].Value);
                                y += (dgvInvoice.Rows[i].Cells["AmtBeforeDisc"].Value == "" || dgvInvoice.Rows[i].Cells["AmtBeforeDisc"].Value == null || dgvInvoice.Rows[i].Cells["AmtBeforeDisc"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["AmtBeforeDisc"].Value);
                                q += (dgvInvoice.Rows[i].Cells["DiscAmount"].Value == "" || dgvInvoice.Rows[i].Cells["DiscAmount"].Value == null || dgvInvoice.Rows[i].Cells["DiscAmount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["DiscAmount"].Value);
                                v += (dgvInvoice.Rows[i].Cells["Amount"].Value == "" || dgvInvoice.Rows[i].Cells["Amount"].Value == null || dgvInvoice.Rows[i].Cells["Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["Amount"].Value);
                                cg += (dgvInvoice.Rows[i].Cells["CGST_Amnt"].Value == "" || dgvInvoice.Rows[i].Cells["CGST_Amnt"].Value == null || dgvInvoice.Rows[i].Cells["CGST_Amnt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["CGST_Amnt"].Value);
                                sg += (dgvInvoice.Rows[i].Cells["SGST_Amnt"].Value == "" || dgvInvoice.Rows[i].Cells["SGST_Amnt"].Value == null || dgvInvoice.Rows[i].Cells["SGST_Amnt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["SGST_Amnt"].Value);
                                ig += (dgvInvoice.Rows[i].Cells["IGST_Amnt"].Value == "" || dgvInvoice.Rows[i].Cells["IGST_Amnt"].Value == null || dgvInvoice.Rows[i].Cells["IGST_Amnt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["IGST_Amnt"].Value);
                                totA += (dgvInvoice.Rows[i].Cells["TotalAmount"].Value == "" || dgvInvoice.Rows[i].Cells["TotalAmount"].Value == null || dgvInvoice.Rows[i].Cells["TotalAmount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["TotalAmount"].Value);

                            }

                            txtTotalQty.Text = x.ToString(".00");
                            txtSubTotal.Text = y.ToString("0.00");
                            txtDiscAmt.Text = q.ToString(".00");
                            txtAmtAftrDis.Text = v.ToString(".00");
                            txtcgst.Text = cg.ToString(".00");
                            txtsgst.Text = sg.ToString(".00");
                            txtigst.Text = ig.ToString(".00");
                            decimal fAmt = (txtFrieght.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtFrieght.Text);

                            txtInvAmunt.Text = (fAmt+totA).ToString(".00");
                        }

                        }

                    }
                   
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        private void txtFrieght_Leave(object sender, EventArgs e)
        {
            decimal Amt = (txtAmtAftrDis.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtAmtAftrDis.Text);
            decimal cgst = (txtcgst.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtcgst.Text);
            decimal sgst = (txtsgst.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtsgst.Text);
            decimal igst = (txtigst.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtigst.Text);
            decimal fAmt = (txtFrieght.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtFrieght.Text);
            txtInvAmunt.Text = (fAmt + Amt+ cgst+ sgst+ igst).ToString(".00");

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            clear1();
        }
        public void clear1()
        {
            foreach (Control x in this.Controls)
            {
                foreach (Control d in groupBox1.Controls)
                {
                    if (d is TextBox)
                        (d as TextBox).Clear();
                    if (d is ComboBox)
                        (d as ComboBox).SelectedIndex = -1;
                    if (d is CheckBox)
                        (d as CheckBox).Checked = false;
                }
                //lblTotalDiscount.Text = "";
                
            }
            if (dgvInvoice.Rows.Count > 0)
            {
                for (int i = 0; i < dgvInvoice.Rows.Count - 1; i++)
                {
                    dgvInvoice.Rows.RemoveAt(i);
                    i--;
                    while (dgvInvoice.Rows.Count == 0)
                        continue;
                }
            }

        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Save();
        }
        public void Save()
        {
            try
            {
                String myString = "";
                myString = txtInvoiceno.Text; ;
                if ((from u in db.Purchase_Invoice_Masters where u.Voucher_No == myString && u.Creation_Company == frmLogin.Creation_Company select u).Count() > 0)
                {
                    if (frmGate.Modify.Contains(this.Text))
                    {
                        dgvInvoice.Enabled = false;
                        var S = db.Purchase_Invoice_Masters.Where(w => w.Voucher_No == myString && w.Creation_Company == Creation_Company).FirstOrDefault();
                        {
                            S.Voucher_No = myString;
                            S.Voucher_Date = dtpInvDt.Value;
                            S.Supplier_Name = cmbSupplier.SelectedValue.ToString();
                            S.Status = cmbStatus.Text;
                            S.Total_Qty = (txtTotalQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotalQty.Text);
                            S.Sub_Total = (txtSubTotal.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtSubTotal.Text);
                            S.Tot_Discount = (txtDiscAmt.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtDiscAmt.Text);
                            S.Amt_After_Discount = (txtAmtAftrDis.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtAmtAftrDis.Text);
                            S.Remarks = (txtRemarks.Text == "") ? "" : txtRemarks.Text;
                            S.Po_RefNo = (txtId.Text == "") ? "" : txtId.Text;
                            S.Doc_Ref_No = (textBox1.Text == "") ? "" : textBox1.Text;
                            S.Doc_Date = dateTimePicker1.Value;
                            S.Supplier_GSTNo = (txtGSTINNO.Text == "") ? "" : txtGSTINNO.Text;
                            S.T_CGST_Amount = (txtcgst.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtcgst.Text);
                            S.T_SGST_Amount = (txtsgst.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtsgst.Text);
                            S.T_IGST_Amount = (txtigst.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtigst.Text);
                            S.Frieght_Charges = (txtFrieght.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtFrieght.Text);
                            S.Total_Amount = (txtInvAmunt.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtInvAmunt.Text);
                            S.Received_Through = (textBox6.Text == "") ? "" : textBox6.Text;
                            S.Vehicle_No = (textBox2.Text == "") ? "" : textBox2.Text;
                            S.LR_No = (txtLrNo.Text == "") ? "" : txtLrNo.Text;
                            S.Received_By = (textBox4.Text == "") ? "" : textBox4.Text;
                            S.Creation_Company = frmLogin.Creation_Company;
                            //S.Created_By = frmLogin.Usertype;
                            //S.Created_Date = DateTime.Now;
                            S.Modified_By = frmLogin.UserName;
                            S.Modified_Date = DateTime.Now;
                            db.SubmitChanges();
                        }
                        //db.Transaction = transaction;
                        for (int i = 0; i < dgvInvoice.RowCount - 1; i++)
                        {
                            if ((from u in db.Purchase_Invoice_Childs where u.Voucher_No == myString && u.Item_Code == dgvInvoice.Rows[i].Cells["Item_Code"].Value && u.Creation_Company == Creation_Company select u).Count() > 0)
                            {
                                var SC = db.Purchase_Invoice_Childs.Where(w => w.Voucher_No == myString && w.Item_Code == dgvInvoice.Rows[i].Cells["Item_Code"].Value && w.Creation_Company == Creation_Company).FirstOrDefault();
                                {
                                    SC.Voucher_No = myString;
                                    SC.Item_Code = (dgvInvoice.Rows[i].Cells["Item_Code"].Value == null) ? "" : (dgvInvoice.Rows[i].Cells["Item_Code"].Value).ToString();
                                    SC.Item_Name = (dgvInvoice.Rows[i].Cells["Item_Name"].Value == null) ? "" : (dgvInvoice.Rows[i].Cells["Item_Name"].Value).ToString();
                                    SC.Qty_Received = (dgvInvoice.Rows[i].Cells["Qty_Received"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["Qty_Received"].Value);
                                    SC.Qty_Rejected = (dgvInvoice.Rows[i].Cells["Qty_Rejected"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["Qty_Rejected"].Value);
                                    SC.Qty_Accepted = (dgvInvoice.Rows[i].Cells["Qty_Accepted"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["Qty_Accepted"].Value);
                                    SC.Unit = (dgvInvoice.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgvInvoice.Rows[i].Cells["uom"].Value.ToString();
                                    SC.Amount = (dgvInvoice.Rows[i].Cells["AmtBeforeDisc"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["AmtBeforeDisc"].Value);
                                    //SC.Discount = (dgvInvoice.Rows[i].Cells["Billd_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["Billd_Qty"].Value);
                                    SC.Rate = (dgvInvoice.Rows[i].Cells["price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["price"].Value);
                                    SC.Taxable_Value = (dgvInvoice.Rows[i].Cells["Amount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["Amount"].Value);
                                    SC.Discount_Amount = (dgvInvoice.Rows[i].Cells["DiscAmount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["DiscAmount"].Value);

                                    SC.Discount = (dgvInvoice.Rows[i].Cells["DiscPer"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["DiscPer"].Value);
                                    SC.CGST_Per = (dgvInvoice.Rows[i].Cells["CGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["CGST_Per"].Value);
                                    SC.SGST_Per = (dgvInvoice.Rows[i].Cells["SGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["SGST_Per"].Value);
                                    SC.IGST_Per = (dgvInvoice.Rows[i].Cells["IGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["IGST_Per"].Value);
                                    SC.CGST_Amount = (dgvInvoice.Rows[i].Cells["CGST_Amnt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["CGST_Amnt"].Value);
                                    SC.SGST_Amount = (dgvInvoice.Rows[i].Cells["SGST_Amnt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["SGST_Amnt"].Value);
                                    SC.IGST_Amount = (dgvInvoice.Rows[i].Cells["IGST_Amnt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["IGST_Amnt"].Value);
                                    SC.Total_Amount = (dgvInvoice.Rows[i].Cells["TotalAmount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["TotalAmount"].Value);


                                    SC.Creation_Company = frmLogin.Creation_Company;

                                    SC.Modified_By = frmLogin.UserName;
                                    SC.Modified_Date = DateTime.Now;
                                }
                            }
                            else
                            {
                                Purchase_Invoice_Child SC = new Purchase_Invoice_Child();
                                SC.Voucher_No = myString;
                                SC.Item_Code = (dgvInvoice.Rows[i].Cells["Item_Code"].Value == null) ? "" : (dgvInvoice.Rows[i].Cells["Item_Code"].Value).ToString();
                                SC.Item_Name = (dgvInvoice.Rows[i].Cells["Item_Name"].Value == null) ? "" : (dgvInvoice.Rows[i].Cells["Item_Name"].Value).ToString();
                                SC.Qty_Received = (dgvInvoice.Rows[i].Cells["Qty_Received"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["Qty_Received"].Value);
                                SC.Qty_Rejected = (dgvInvoice.Rows[i].Cells["Qty_Rejected"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["Qty_Rejected"].Value);
                                SC.Qty_Accepted = (dgvInvoice.Rows[i].Cells["Qty_Accepted"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["Qty_Accepted"].Value);
                                SC.Unit = (dgvInvoice.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgvInvoice.Rows[i].Cells["uom"].Value.ToString();
                                SC.Amount = (dgvInvoice.Rows[i].Cells["AmtBeforeDisc"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["AmtBeforeDisc"].Value);
                                //SC.Discount = (dgvInvoice.Rows[i].Cells["Billd_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["Billd_Qty"].Value);
                                SC.Rate = (dgvInvoice.Rows[i].Cells["price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["price"].Value);
                                SC.Taxable_Value = (dgvInvoice.Rows[i].Cells["Amount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["Amount"].Value);
                                SC.Discount_Amount = (dgvInvoice.Rows[i].Cells["DiscAmount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["DiscAmount"].Value);

                                SC.Discount = (dgvInvoice.Rows[i].Cells["DiscPer"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["DiscPer"].Value);
                                SC.CGST_Per = (dgvInvoice.Rows[i].Cells["CGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["CGST_Per"].Value);
                                SC.SGST_Per = (dgvInvoice.Rows[i].Cells["SGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["SGST_Per"].Value);
                                SC.IGST_Per = (dgvInvoice.Rows[i].Cells["IGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["IGST_Per"].Value);
                                SC.CGST_Amount = (dgvInvoice.Rows[i].Cells["CGST_Amnt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["CGST_Amnt"].Value);
                                SC.SGST_Amount = (dgvInvoice.Rows[i].Cells["SGST_Amnt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["SGST_Amnt"].Value);
                                SC.IGST_Amount = (dgvInvoice.Rows[i].Cells["IGST_Amnt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["IGST_Amnt"].Value);
                                SC.Total_Amount = (dgvInvoice.Rows[i].Cells["TotalAmount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["TotalAmount"].Value);


                                SC.Creation_Company = frmLogin.Creation_Company;
                                SC.Created_By = frmLogin.UserName;
                                SC.Modified_Date = DateTime.Now;
                                SC.Modified_By = frmLogin.UserName;
                                SC.Modified_Date = DateTime.Now;
                                db.Purchase_Invoice_Childs.InsertOnSubmit(SC);
                            }
                        }
                        db.SubmitChanges();
                        //transaction.Commit();
                        lnkus2.Text = frmLogin.UserName;
                        MessageBox.Show("Record Updated Successfully");
                    }
                    else
                    {
                        MessageBox.Show("Sorry! You Do not have privileges to Modify Order");
                    }

                }
                else
                {
                    if (frmGate.Create_menu.Contains(this.Text))
                    {
                        db.Sp_Delete_PurchaseInvoice(myString, frmLogin.Creation_Company);
                        // // // // Auto Increment For Sale Order

                        AutoincrementId();

                        Purchase_Invoice_Master S = new Purchase_Invoice_Master();
                        S.Voucher_No = myString;
                        S.Voucher_Date = dtpInvDt.Value;
                        S.Supplier_Name = cmbSupplier.SelectedValue.ToString();
                        S.Status = cmbStatus.Text;
                        S.Total_Qty = (txtTotalQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotalQty.Text);
                        S.Sub_Total = (txtSubTotal.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtSubTotal.Text);
                        S.Tot_Discount = (txtDiscAmt.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtDiscAmt.Text);
                        S.Amt_After_Discount = (txtAmtAftrDis.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtAmtAftrDis.Text);
                        S.Remarks = (txtRemarks.Text == "") ? "" : txtRemarks.Text;
                        S.Po_RefNo = (txtId.Text == "") ? "" : txtId.Text;
                        S.Doc_Ref_No = (textBox1.Text == "") ? "" : textBox1.Text;
                        S.Doc_Date = dateTimePicker1.Value;
                        S.Supplier_GSTNo = (txtGSTINNO.Text == "") ? "" : txtGSTINNO.Text;
                        S.T_CGST_Amount = (txtcgst.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtcgst.Text);
                        S.T_SGST_Amount = (txtsgst.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtsgst.Text);
                        S.T_CGST_Amount = (txtigst.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtigst.Text);
                        S.Frieght_Charges = (txtFrieght.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtFrieght.Text);
                        S.Total_Amount = (txtInvAmunt.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtInvAmunt.Text);
                        S.Received_Through = (textBox6.Text == "") ? "" : textBox6.Text;
                        S.Vehicle_No = (textBox2.Text == "") ? "" : textBox2.Text;
                        S.LR_No = (txtLrNo.Text == "") ? "" : txtLrNo.Text;
                        S.Received_By = (textBox4.Text == "") ? "" : textBox4.Text;
                        S.Creation_Company = frmLogin.Creation_Company;
                        S.Created_By = frmLogin.Usertype;
                        S.Created_Date = DateTime.Now;
                        S.Modified_By = frmLogin.UserName;
                        S.Modified_Date = DateTime.Now;
                        db.SubmitChanges();

                        db.Purchase_Invoice_Masters.InsertOnSubmit(S);
                        db.SubmitChanges();
                        //db.Transaction = transaction;
                        for (int i = 0; i < dgvInvoice.RowCount - 1; i++)
                        {
                            Purchase_Invoice_Child SC = new Purchase_Invoice_Child();
                            SC.Voucher_No = myString;
                            SC.Item_Code = (dgvInvoice.Rows[i].Cells["Item_Code"].Value == null) ? "" : (dgvInvoice.Rows[i].Cells["Item_Code"].Value).ToString();
                            SC.Item_Name = (dgvInvoice.Rows[i].Cells["Item_Name"].Value == null) ? "" : (dgvInvoice.Rows[i].Cells["Item_Name"].Value).ToString();
                            SC.Qty_Received = (dgvInvoice.Rows[i].Cells["Qty_Received"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["Qty_Received"].Value);
                            SC.Qty_Rejected = (dgvInvoice.Rows[i].Cells["Qty_Rejected"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["Qty_Rejected"].Value);
                            SC.Qty_Accepted = (dgvInvoice.Rows[i].Cells["Qty_Accepted"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["Qty_Accepted"].Value);
                            SC.Unit = (dgvInvoice.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgvInvoice.Rows[i].Cells["uom"].Value.ToString();
                            SC.Amount = (dgvInvoice.Rows[i].Cells["AmtBeforeDisc"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["AmtBeforeDisc"].Value);
                            //SC.Discount = (dgvInvoice.Rows[i].Cells["Billd_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["Billd_Qty"].Value);
                            SC.Rate = (dgvInvoice.Rows[i].Cells["price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["price"].Value);
                            SC.Taxable_Value = (dgvInvoice.Rows[i].Cells["Amount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["Amount"].Value);
                            SC.Discount_Amount = (dgvInvoice.Rows[i].Cells["DiscAmount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["DiscAmount"].Value);

                            SC.Discount = (dgvInvoice.Rows[i].Cells["DiscPer"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["DiscPer"].Value);
                            SC.CGST_Per = (dgvInvoice.Rows[i].Cells["CGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["CGST_Per"].Value);
                            SC.SGST_Per = (dgvInvoice.Rows[i].Cells["SGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["SGST_Per"].Value);
                            SC.IGST_Per = (dgvInvoice.Rows[i].Cells["IGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["IGST_Per"].Value);
                            SC.CGST_Amount = (dgvInvoice.Rows[i].Cells["CGST_Amnt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["CGST_Amnt"].Value);
                            SC.SGST_Amount = (dgvInvoice.Rows[i].Cells["SGST_Amnt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["SGST_Amnt"].Value);
                            SC.IGST_Amount = (dgvInvoice.Rows[i].Cells["IGST_Amnt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["IGST_Amnt"].Value);
                            SC.Total_Amount = (dgvInvoice.Rows[i].Cells["TotalAmount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvInvoice.Rows[i].Cells["TotalAmount"].Value);


                            SC.Creation_Company = frmLogin.Creation_Company;
                            SC.Created_By = frmLogin.UserName;
                            SC.Modified_Date = DateTime.Now;
                            SC.Modified_By = frmLogin.UserName;
                            SC.Modified_Date = DateTime.Now;
                            db.Purchase_Invoice_Childs.InsertOnSubmit(SC); ;
                        }
                        db.SubmitChanges();
                        MessageBox.Show("Record Saved Successfully");
                        //transaction.Commit();
                    }
                }
 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        public void bindedit()
        {
            try
            {
                txtInvoiceno.Text = Inventory_Management.PurchaseVouchersList.ShiInv_No;
                String myString = "";
                myString = txtInvoiceno.Text;
                var da = (from obj in db.Purchase_Invoice_Masters
                          where obj.Voucher_No == txtInvoiceno.Text && obj.Creation_Company == frmLogin.Creation_Company
                          select obj).ToList();

                if (da.Count > 0)
                {
                    dtpInvDt.Text = da[0].Voucher_Date.ToString();
                   
                    bindSupplier();
                    cmbSupplier.SelectedValue = da[0].Supplier_Name;
                    txtGSTINNO.Text = da[0].Supplier_GSTNo;
                    //cmbCustomer.Enabled = false;
                    txtId.Text = da[0].Po_RefNo;
                    textBox1.Text = da[0].Doc_Ref_No;
                    dateTimePicker1.Text = da[0].Doc_Date.ToString();        
                     
                    textBox6.Text = (da[0].Received_Through.ToString() == "" || da[0].Received_Through.ToString() == null) ? "" : da[0].Received_Through.ToString();
                    textBox2.Text = (da[0].Vehicle_No.ToString() == "" || da[0].Vehicle_No.ToString() == null) ? "" : da[0].Vehicle_No.ToString();
                    txtLrNo.Text = da[0].LR_No.ToString();
                    txtRemarks.Text = da[0].Remarks;
                    cmbStatus.Text = da[0].Status;
                    textBox4.Text = da[0].Received_By;
                    txtTotalQty.Text = da[0].Total_Qty.ToString();
                    txtSubTotal.Text = da[0].Sub_Total.ToString();
                    txtDiscAmt.Text = da[0].Tot_Discount.ToString();
                    
                   
                    txtcgst.Text = da[0].T_CGST_Amount.ToString();
                    txtsgst.Text = da[0].T_SGST_Amount.ToString();
                    txtigst.Text = da[0].T_IGST_Amount.ToString();
                    txtFrieght.Text = da[0].Frieght_Charges.ToString();                    

                    txtInvAmunt.Text = da[0].Total_Amount.ToString();
                        lnkus1.Text = da[0].Created_By;
                        lbldt1.Text = string.Format("{0:dd/MM/yyyy HH:mm tt}", da[0].Created_Date);

                    
                }


                var dm1 = (from s in db.Purchase_Invoice_Childs
                           where s.Voucher_No == myString && s.Creation_Company == Creation_Company


                           select new

                           {
                               s.Item_Code,
                               s.Item_Name,
                               s.Qty_Received,
                               s.Qty_Rejected,
                               s.Qty_Accepted,                               
                               price = s.Rate,
                               uom = s.Unit,
                               AmtBeforeDisc = s.Amount,
                               Discper = s.Discount,
                               DiscAmount = s.Discount_Amount,
                               Amount = s.Taxable_Value,
                               s.CGST_Per,
                               CGST_Amnt =s.CGST_Amount,
                               s.SGST_Per,
                               SGST_Amnt=s.SGST_Amount,
                               s.IGST_Per,
                               IGST_Amnt=s.IGST_Amount,
                               TotalAmount = s.Total_Amount,                              
                              
                               
                               
                              
                           });




                SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataTable dtr = new DataTable();
                da2.Fill(dtr);
                if (dtr.Rows.Count >= 0)
                    dgvInvoice.DataSource = dtr;

                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
