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
    public partial class frmStockTransfers : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string DocNo, ItemCode, RecQty, Suppname;
        public frmStockTransfers()
        {
            InitializeComponent();
        }

        private void frmMaterialIssues_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            
            dpSODate.MinDate = logIn.fy_Start_Date;
            dpSODate.MaxDate = logIn.fy_End_Date;
            bindDept();
            AutoincrementId();
        }

        public void AutoincrementId()
        {
            try
            {

                var result = db.Sp_autoincrement_StockTransfers(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, logIn.BU_ID);
                txtSlipNo.Text = result.FirstOrDefault().Slip_NO;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbIssuedTo_Leave(object sender, EventArgs e)
        {
            try
            {
                bindDept();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void bindDept()
        {
            try
            {
                //string depttext = cmbDepartment.Text;
                //if (cmbIssuedTo.Text != "")
                //{
                //    switch (cmbIssuedTo.Text)
                //    {

                //case "Internal Department":
                var SO = (from m in db.Attributes_Datas where m.Head_Name == "Ware House" select new { m.ID, m.Descr }).Distinct().ToList();
                if (SO.Count > 0)
                {
                    cmbTransferFrom.DataSource = SO;
                    cmbTransferFrom.ValueMember = "ID";
                    cmbTransferFrom.DisplayMember = "Descr";
                }

                var SO1 = (from m in db.Attributes_Datas where m.Head_Name == "Ware House" select new { m.ID, m.Descr }).Distinct().ToList();
                if (SO1.Count > 0)
                {
                    cmbTransferTo.DataSource = SO1;
                    cmbTransferTo.ValueMember = "ID";
                    cmbTransferTo.DisplayMember = "Descr";
                }



                //cmbDepartment.Text = depttext;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void txtIndentNo_Leave(object sender, EventArgs e)
        {
            
        }

        private void dgProducts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
               
                DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].Name;
                if (columnName == "Item_Description" && R1.Cells["Item_Description"].Value != null)
                {

                    var getProductName = (from s in db.Products
                                          join u in db.UoM_Masters on s.Prod_Primary_UOM_Id equals u.UOM_ID
                                          join g in db.Product_Groups on s.Prod_Group_Id equals g.ID
                                          where s.Prod_Name == R1.Cells["Item_Description"].Value.ToString() && s.Company_ID == logIn.company
                                          select new { s.prod_ID, u.Uom_Descr, g.Prod_Group_Name, s.Prod_HSN_Code }).FirstOrDefault();

                    if (getProductName != null)
                    {
                        R1.Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                        R1.Cells["Item_code"].Value = getProductName.prod_ID.ToString();

                    }

                    else
                    {
                        R1.Cells["Item_Code"].Value = dgProducts.CurrentCell.RowIndex + 1;
                    }
                    DateTime t = dpSODate.Value;                   
                    R1.Cells["Stock_Qty"].Value = 0;
                    R1.Cells["Price"].Value = 0;
                    string dt1 = t.ToString("yyyy/MM/dd");
                    var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(R1.Cells["Item_code"].Value), Convert.ToDateTime(dt1), logIn.BU_ID) select data).ToList();

                    if (stock.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        R1.Cells["Stock_Qty"].Value = stock[0].ClosingQty;                      
                        R1.Cells["Price"].Value = stock[0].CBPrice;
                        //R1.Cells["Price"].Value = stock[0].CBPrice;
                    }
                }

                if (columnName == "Issue_Qty")
                {
                    int Itemcode = Convert.ToInt32(R1.Cells["Item_Code"].Value.ToString());                                  
                    if (R1.Cells["Item_Description"].Value != null)
                    {
                        decimal IssuedQty = (R1.Cells["Issue_Qty"].Value == "" || R1.Cells["Issue_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Issue_Qty"].Value);
                        decimal IndentQty = (R1.Cells["Indent_Qty"].Value == "" || R1.Cells["Indent_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Indent_Qty"].Value);
                        decimal StockQty = (R1.Cells["Stock_Qty"].Value == "" || R1.Cells["Stock_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Stock_Qty"].Value);
                        decimal price = (R1.Cells["Price"].Value == "" || R1.Cells["Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Price"].Value);
                        if (cmbTransferFrom.Text != "NA")
                        {
                            if (IssuedQty > IndentQty)
                            {
                                MessageBox.Show("Issued Qty Cannot Be Greater Than Indent Qty");
                                R1.Cells["Issue_Qty"].Value = 0;
                                return;
                            }
                        }
                        if (IssuedQty <= StockQty)
                        {
                            decimal Amt, DiscAmt, netAmt, gst, igst, totamt;

                            Amt = IssuedQty * price;
                            R1.Cells["Amount"].Value = Amt.ToString("0.00");
                        }
                        else
                        {
                            MessageBox.Show("Issued Qty Cannot Be Greater Than Stock Qty");
                            R1.Cells["Issue_Qty"].Value = 0;
                            return;
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

        private void dgProducts_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].HeaderText;
                TextBox tb3 = e.Control as TextBox;
                if (tb3 != null && columnName == "UOM")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }
                if (tb3 != null && columnName == "Item Description")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }

                if (tb3 != null && columnName == "Grade / Make / Model")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }
                if (tb3 != null && columnName == "Cost Center")
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
                DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];

                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].HeaderText;

                if (columnName == "UOM")
                {
                    var Prodname = (from d in db.UoM_Masters select new { d.Uom_Descr }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Uom_Descr");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Uom_Descr);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
                    }
                }
                else
                {
                    if (columnName == "Item Description")
                    {
                        var Prodname = (from d in db.Products where d.Company_ID == logIn.company && d.Prod_Status_ID == 1 select new { d.Prod_Name }).ToList();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Prod_Name");
                        foreach (var item in Prodname)
                        {
                            dt.Rows.Add(item.Prod_Name);
                        }
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            coll.Add(dt.Rows[i][0].ToString());
                        }
                    }
                    else
                    {
                        if (columnName == "Grade / Make / Model")
                        {
                            var Prodname = (from d in db.GoodsReceiptNote_Childs where d.Company_ID == logIn.company select new { d.Prod_Grade }).Distinct().ToList();
                            DataTable dt = new DataTable();
                            dt.Columns.Add("Prod_Grade");
                            foreach (var item in Prodname)
                            {
                                dt.Rows.Add(item.Prod_Grade);
                            }
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                coll.Add(dt.Rows[i][0].ToString());
                            }
                        }
                        else
                        {
                            if (columnName == "Cost Center")
                            {
                                var Prodname = (from d in db.CostCenter_Masters where d.Company_ID == logIn.company select new { d.CostCenter_Name }).Distinct().ToList();
                                DataTable dt = new DataTable();
                                dt.Columns.Add("CostCenter_Name");
                                foreach (var item in Prodname)
                                {
                                    dt.Rows.Add(item.CostCenter_Name);
                                }
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    coll.Add(dt.Rows[i][0].ToString());
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                
                if (cmbTransferTo.Text == string.Empty)
                {
                    MessageBox.Show("Select Cost Unit Name Where the Goods Being Trasferred", "Stock Transfer", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbTransferTo.Focus();
                    return;
                }
                else
                if (cmbTransferFrom.Text == string.Empty)
                {
                    MessageBox.Show("Select Ref Doc Type", "Issues", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbTransferFrom.Focus();
                    return;
                }
                else
                {
                    Save();


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void Save()
        {
            try
            {
                String myString = "";
                myString = txtSlipNo.Text;
                if ((from u in db.Stock_Transfer_Masters where u.Slip_NO == myString && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    myString = txtSlipNo.Text;
                    db.sp_Delete_StockTransfer(myString, logIn.company,logIn.BU_ID);
                }
                else
                {
                    AutoincrementId();
                    myString = txtSlipNo.Text;

                }
                //if (frmGate.Modify.Contains(this.Text))
                //{

                //dgProducts.Enabled = false;
                //var S = db.Sale_Order_Masters.Where(w => w.SO_NO == myString && w.Company_ID == logIn.company).FirstOrDefault();
                Stock_Transfer_Master S = new Stock_Transfer_Master();
                {
                    S.Slip_NO = myString;
                    S.Slip_Date = dpSODate.Value;                    
                    S.BU_Name_To = Convert.ToInt32(cmbTransferTo.SelectedValue.ToString());
                    S.Ref_Doc_Type = (cmbTransferFrom.Text);                    
                          
                    S.Remarks = txtRemarks.Text;    
                    S.isDeleted = false;                   
                    S.BU_ID = logIn.BU_ID;                  
                    S.Company_ID = logIn.company;
                    S.Created_By = lblCreatedBy.Text;
                    S.Modified_By = logIn.username + "-" + DateTime.Now;
                    db.Stock_Transfer_Masters.InsertOnSubmit(S);
                    db.SubmitChanges();
                }
                //db.Transaction = transaction;
                for (int i = 0; i < dgProducts.RowCount - 1; i++)
                {
                    Stock_Transfer_Child SC = new Stock_Transfer_Child();
                    var d1 = (from a in db.Stock_Transfer_Masters where a.Slip_NO == myString && a.Company_ID == logIn.company select new { a.Id }).ToList();
                    SC.Slip_Master_ID = d1[0].Id;
                    SC.Slip_NO = myString;
                    SC.Prod_Code = (dgProducts.Rows[i].Cells["Item_Code"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value);
                    //SC.Product_Description = (dgProducts.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value).ToString();
                    //SC.Prod_Spec = (dgProducts.Rows[i].Cells["Item_spec"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_spec"].Value).ToString();
                    //SC.Prod_Grade = (dgProducts.Rows[i].Cells["Item_Grade"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value).ToString();
                    //SC.Uom = (dgProducts.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["uom"].Value.ToString();
                    SC.Indent_Qty = (dgProducts.Rows[i].Cells["Indent_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Indent_Qty"].Value);
                    SC.Stock_Qty = (dgProducts.Rows[i].Cells["Stock_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Stock_Qty"].Value);
                    SC.Issued_Qty = (dgProducts.Rows[i].Cells["Issue_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Issue_Qty"].Value);
                    SC.Issue_Price = (dgProducts.Rows[i].Cells["Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Price"].Value);
                    SC.Issue_Value = (dgProducts.Rows[i].Cells["Amount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Amount"].Value);
                    SC.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value).ToString();
                    SC.ProdSno = i + 1;
                    SC.Company_ID = logIn.company;
                    db.Stock_Transfer_Childs.InsertOnSubmit(SC);
                }
                db.SubmitChanges();               
                
                MessageBox.Show("Record Saved / Updated Successfully With Transaction Ref No : " + txtSlipNo.Text);
                clear();
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

        private void btnClose_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        private void dgProducts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F6)
            {
                if (dgProducts.Rows.Count > 0)
                {
                    //DataGridViewRow i = dgProducts.Rows[dgProducts.CurrentRow.Index];
                    //foreach (DataGridViewCell oneCell in dgProducts.SelectedCells)
                    //{
                    //if (oneCell.Selected)
                    dgProducts.Rows.RemoveAt(dgProducts.CurrentRow.Index);
                    //}
                }
            }
            if (e.KeyCode == Keys.F3)
            {
                
            }
        }

        private void frmMaterialIssues_FormClosed(object sender, FormClosedEventArgs e)
        {
            //SqlCommand cmd = new SqlCommand();
            //cmd.CommandText = "Delete from Bloom_Roll_Wise_Issues where [Doc_Ref]=@param1 and Company_ID =@compName and status ='Open'";
            //cmd.Parameters.AddWithValue("@param1", txtSlipNo.Text);
            //cmd.Parameters.AddWithValue("@CompName", logIn.company);
            //cmd.Connection = con;
            //con.Open();
            //cmd.ExecuteNonQuery();
            //con.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    if ((from a in db.Stock_Transfer_Masters where a.Company_ID == logIn.company && a.Slip_NO == txtSlipNo.Text select a).Count() > 0)
                    {
                        db.sp_Delete_StockTransfer(txtSlipNo.Text,logIn.company,logIn.BU_ID);
                        MessageBox.Show("Recored Deleted Successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void label33_Click(object sender, EventArgs e)
        {

        }

        private void btnFind_Click(object sender, EventArgs e)
        {

           
        }

        private void brnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //  bindCashAct();
                MaterialManagement.Transactions.frmStockTransferList obj = new MaterialManagement.Transactions.frmStockTransferList();

                if (obj.ShowDialog() == DialogResult.OK)
                {
                    txtSlipNo.Text = MaterialManagement.Transactions.frmStockTransferList.voucherNo;

                    if (!string.IsNullOrEmpty(txtSlipNo.Text))
                    {
                        var dm1 = (from s in db.Stock_Transfer_Childs
                                         join a in db.Stock_Transfer_Masters on s.Slip_Master_ID equals a.Id
                                   where s.Slip_NO == txtSlipNo.Text && s.Company_ID == logIn.company && a.BU_ID == logIn.BU_ID
                                   select new
                                   {
                                       Item_Code= s.Prod_Code,
                                       Item_Description= s.Product_Description,
                                       Item_Spec= s.Prod_Spec,
                                       Item_Grade = s.Prod_Grade,
                                       UOM = s.Uom,
                                       s.Indent_Qty,
                                       s.Stock_Qty,
                                       Issue_Qty=s.Issued_Qty,
                                       Price=s.Issue_Price,
                                       Amount=s.Issue_Value,                                     
                                       s.Remarks
                                   });


                        SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        DataTable dtr = new DataTable();
                        da2.Fill(dtr);
                        if (dtr.Rows.Count >= 0)
                            dgProducts.DataSource = dtr;


                    }

                    var f = (from s in db.Stock_Transfer_Masters where s.Slip_NO == txtSlipNo.Text && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID select s).FirstOrDefault();
                    if (f != null)
                    {
                        dpSODate.Text = f.Slip_Date.ToString();
                        cmbTransferFrom.Text = f.Ref_Doc_Type;
                     
                        
                        bindDept();
                        cmbTransferTo.SelectedValue = f.BU_Name_To;
                        
                        txtRemarks.Text = f.Remarks;
                        //ObDate.Text = f.OBDate.ToString();
                        
                        lblCreatedBy.Text = f.Created_By;
                        lblModified.Text = f.Modified_By;
                    }
                    //double Qty = 0, Amout = 0;

                    //for (int j = 0; j < dgvJournalVouchar.Rows.Count; j++)
                    //{

                    //    if (dgvJournalVouchar.Rows[j].Cells["Debit"].Value != null)
                    //    {

                    //        Qty += (dgvJournalVouchar.Rows[j].Cells["Debit"].Value == DBNull.Value) ? Convert.ToDouble("00") : Convert.ToDouble(dgvJournalVouchar.Rows[j].Cells["Debit"].Value);
                    //        Amout += (dgvJournalVouchar.Rows[j].Cells["Credit"].Value == DBNull.Value) ? Convert.ToDouble("00") : Convert.ToDouble(dgvJournalVouchar.Rows[j].Cells["Credit"].Value);


                    //    }

                    //}
                    //txtDebitAmtTotal.Text = Qty.ToString(".00");
                    //txtCreditAmtTotal.Text = Amout.ToString(".00");

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void clear()
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                AutoincrementId();
                cmbTransferFrom.Text = "";                              
                cmbTransferTo.Text = "";
                
                txtRemarks.Text = "";
                lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");


                if (dgProducts.Rows.Count > 0)
                {
                    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                    {
                        dgProducts.Rows.RemoveAt(i);
                        i--;
                        while (dgProducts.Rows.Count == 0)
                            continue;
                    }
                }
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
    }
}
