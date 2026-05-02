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
    public partial class frmStockJournal : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string DocNo, ItemCode, RecQty, Suppname;
        public frmStockJournal()
        {
            InitializeComponent();
        }

        private void frmMaterialIssues_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //txtIssuedBy.Text = logIn.username;
            dpSODate.MinDate = logIn.fy_Start_Date;
            dpSODate.MaxDate = logIn.fy_End_Date;
            bindDept();
            AutoincrementId();
        }

        public void AutoincrementId()
        {
            try
            {

                var result = db.Sp_autoincrement_StockJournal(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date,logIn.BU_ID);
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
                    //var Dept = (from m in db.Costing_Units where m.Company == logIn.company && m.id != logIn.BU_ID select new { m.id, m.BU_Name }).Distinct().ToList();
                    //if (Dept.Count > 0)
                    //{
                    //    cmbDepartment.DataSource = Dept;
                    //    cmbDepartment.ValueMember = "id";
                    //    cmbDepartment.DisplayMember = "BU_Name";
                    //}
                           
                   
                
                //cmbDepartment.Text = depttext;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void txtIndentNo_Leave(object sender, EventArgs e)
        {
            try
            {
               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
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
                                          select new { s.prod_ID, u.Uom_Descr, g.Prod_Group_Name, s.Prod_Code }).FirstOrDefault();

                    if (getProductName != null)
                    {
                        R1.Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                        R1.Cells["Item_code"].Value = getProductName.prod_ID.ToString();
                        R1.Cells["Prod_Code"].Value = getProductName.Prod_Code.ToString();

                    }

                    else
                    {
                        R1.Cells["Item_Code"].Value = dgProducts.CurrentCell.RowIndex + 1;
                    }
                    DateTime t = dpSODate.Value;
                    R1.Cells["Stock_Qty"].Value = 0;
                    R1.Cells["Price"].Value = 0;
                    string dt1 = t.ToString("yyyy/MM/dd");
                    var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(R1.Cells["Item_Code"].Value), Convert.ToDateTime(dt1), logIn.BU_ID) select data).ToList();

                    if (stock.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        R1.Cells["Stock_Qty"].Value = stock[0].ClosingQty;
                        R1.Cells["Price"].Value = stock[0].CBPrice;
                        //R1.Cells["Price"].Value = stock[0].CBPrice;
                    }
                }

                if (columnName == "Stock_In_Item" && R1.Cells["Stock_In_Item"].Value != null)
                {

                    var getProductName = (from s in db.Products
                                          join u in db.UoM_Masters on s.Prod_Primary_UOM_Id equals u.UOM_ID
                                          join g in db.Product_Groups on s.Prod_Group_Id equals g.ID
                                          where s.Prod_Name == R1.Cells["Stock_In_Item"].Value.ToString() && s.Company_ID == logIn.company
                                          select new { s.prod_ID, u.Uom_Descr, g.Prod_Group_Name, s.Prod_Code }).FirstOrDefault();

                    if (getProductName != null)
                    {
                       // R1.Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                        R1.Cells["Stock_In_Item_ID"].Value = getProductName.prod_ID.ToString();
                        R1.Cells["Stock_in_Item_Code"].Value = getProductName.Prod_Code.ToString();

                    }

                    else
                    {
                        //R1.Cells["Item_Code"].Value = dgProducts.CurrentCell.RowIndex + 1;
                    }
                    
                }

                if (columnName == "Stock_Out")
                {
                    int Itemcode = Convert.ToInt32(R1.Cells["Item_Code"].Value.ToString());
                    if (R1.Cells["Item_Description"].Value != null)
                    {
                        decimal IssuedQty = (R1.Cells["Stock_Out"].Value == "" || R1.Cells["Stock_Out"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Stock_Out"].Value);
                        decimal StockQty = (R1.Cells["Stock_Qty"].Value == "" || R1.Cells["Stock_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Stock_Qty"].Value);
                        decimal price = (R1.Cells["Price"].Value == "" || R1.Cells["Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Price"].Value);
                       
                        if (IssuedQty <= StockQty)
                        {
                            decimal Amt, DiscAmt, netAmt, gst, igst, totamt;

                            Amt = IssuedQty * price;
                            R1.Cells["Amount"].Value = Amt.ToString("0.00");
                        }
                        else
                        {
                            MessageBox.Show("Stock Out Qty Cannot Be Greater Than Stock Qty");
                            R1.Cells["Stock_Out"].Value = 0;
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
                if (tb3 != null && columnName == "Stock Out Item")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }

                if (tb3 != null && columnName == "Stock_In_Item")
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
                    if (columnName == "Stock Out Item")
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
                        if (columnName == "Stock_In_Item")
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
                            if (columnName == "Cost Center")
                            {
                                //var Prodname = (from d in db.CostCenter_Masters where d.Company_ID == logIn.company select new { d.CostCenter_Name }).Distinct().ToList();
                                //DataTable dt = new DataTable();
                                //dt.Columns.Add("CostCenter_Name");
                                //foreach (var item in Prodname)
                                //{
                                //    dt.Rows.Add(item.CostCenter_Name);
                                //}
                                //for (int i = 0; i < dt.Rows.Count; i++)
                                //{
                                //    coll.Add(dt.Rows[i][0].ToString());
                                //}
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
                
                
                
                    Save();


                
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
                if ((from u in db.Stock_Journals where u.Slip_NO == myString && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    myString = txtSlipNo.Text;
                    db.sp_Delete_Stockjournal(myString, logIn.company,logIn.BU_ID);
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
                               //db.Transaction = transaction;
                for (int i = 0; i < dgProducts.RowCount - 1; i++)
                {
                    Stock_Journal SC = new Stock_Journal();
                   
                    SC.Slip_NO = myString;
                    SC.Stock_Out_Prod_Code = (dgProducts.Rows[i].Cells["Item_Code"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value);
                    SC.Stock_Int_Prod_Code = (dgProducts.Rows[i].Cells["Stock_In_Item_ID"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["Stock_In_Item_ID"].Value);
                    SC.Uom = (dgProducts.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["uom"].Value.ToString();
                    SC.Stock_Qty = (dgProducts.Rows[i].Cells["Stock_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Stock_Qty"].Value);
                    SC.Stock_Transfer_Qty = (dgProducts.Rows[i].Cells["Stock_Out"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Stock_Out"].Value);
                    SC.Issue_Price = (dgProducts.Rows[i].Cells["Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Price"].Value);
                    SC.Issue_Value = (dgProducts.Rows[i].Cells["Amount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Amount"].Value);
                    //SC.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value).ToString();
                    SC.ProdSno = i + 1;
                    SC.BU_ID = logIn.BU_ID;
                    SC.Slip_Date = dpSODate.Value;
                    SC.Company_ID = logIn.company;
                    SC.Created_By = lblCreatedBy.Text;
                    SC.Modified_By = logIn.username + "-" + DateTime.Now;
                    db.Stock_Journals.InsertOnSubmit(SC);
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
                    if ((from a in db.Stock_Journals where a.Company_ID == logIn.company && a.Slip_NO == txtSlipNo.Text select a).Count() > 0)
                    {
                        db.sp_Delete_Stockjournal(txtSlipNo.Text,logIn.company,logIn.BU_ID);
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

        private void dgProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
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
                MaterialManagement.Transactions.frmStockJournalList obj = new MaterialManagement.Transactions.frmStockJournalList();

                if (obj.ShowDialog() == DialogResult.OK)
                {
                    txtSlipNo.Text = MaterialManagement.Transactions.frmStockJournalList.voucherNo;

                    if (!string.IsNullOrEmpty(txtSlipNo.Text))
                    {
                        var dm1 = (from s in db.Stock_Journals
                                         join a in db.Products on s.Stock_Out_Prod_Code equals a.prod_ID
                                         join p in db.Products on s.Stock_Int_Prod_Code equals p.prod_ID
                                   where s.Slip_NO == txtSlipNo.Text && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID
                                   select new
                                   {
                                       Item_Code= s.Stock_Out_Prod_Code,
                                       Prod_Code = a.Prod_Code,
                                       Item_Description= a.Prod_Name,                                      
                                       UOM = s.Uom,
                                       s.Stock_Qty,                                       
                                       Stock_Out = s.Stock_Transfer_Qty,
                                       Price =s.Issue_Price,
                                       Amount=s.Issue_Value,
                                       Stock_In_Item = p.Prod_Name,
                                       Stock_In_Item_Code = p.Prod_Code,
                                       Stock_In_Item_ID = s.Stock_Int_Prod_Code
                                      
                                       
                                   });


                        SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        DataTable dtr = new DataTable();
                        da2.Fill(dtr);
                        if (dtr.Rows.Count >= 0)
                            dgProducts.DataSource = dtr;


                    }

                    var f = (from s in db.Stock_Journals where s.Slip_NO == txtSlipNo.Text && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID select s).FirstOrDefault();
                    if (f != null)
                    {
                        dpSODate.Text = f.Slip_Date.ToString();
                        
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
