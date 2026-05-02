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
    public partial class frmMaterialReturs : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string DocNo, ItemCode, RecQty, Suppname;
        public static DataTable dtgetfinalprducts = new DataTable();
        public frmMaterialReturs()
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

                var result = db.Sp_autoincrement_MaterialReturnJW(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date);
                txtSlipNo.Text = result.FirstOrDefault().Slip_No;
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

                string cValue = cmbDepartment.Text;
                var Dept = (from m in db.Department_Masters where m.Company_ID == logIn.company select new { m.Id, m.Dept_Name }).Distinct().ToList();
                if (Dept.Count > 0)
                {
                    cmbDepartment.DataSource = Dept;
                    cmbDepartment.ValueMember = "Id";
                    cmbDepartment.DisplayMember = "Dept_Name";
                }
                cmbDepartment.Text = cValue;
                if (logIn.company == 20)
                {
                    var pCodes = (from m in db.Engg_Mfg_Orders where m.Company_ID == logIn.company select new { m.id, m.MO_No }).Distinct().ToList();
                    if (pCodes.Count > 0)
                    {
                        cmbRefDocType.DataSource = pCodes;
                        cmbRefDocType.ValueMember = "id";
                        cmbRefDocType.DisplayMember = "MO_No";
                    }
                    cmbRefDocType.SelectedIndex = -1;
                }
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
               
                if (cmbRefDocType.Text == "Cutting Plan")
                {
                    //cmbIssuedTo.Text = "Sub Contractor";
                    //bindDept();
                    //var gstno = (from c in db.CuttingPlan_Releases
                    //             where c.Ref_No == txtIndentNo.Text && c.Company_ID == logIn.company
                    //             select new { c.Contractor_ID }).ToList();
                    //if (gstno.Count > 0)
                    //{
                        
                    //    cmbDepartment.SelectedValue = gstno[0].Contractor_ID;

                    //}
                }



                DataTable dt = new DataTable();
                //String conStr = "Data Source=172.16.6.173;Initial Catalog=servion_hari;User  ID=sa;Password=Servion@123";
                //SqlConnection con = new SqlConnection(con);
                //DateTime t = dpdate.Value;
                //string t1 = t.ToString("dd/MMM/yyyy");
                SqlCommand com = new SqlCommand("CP_To_Issue", con);
                com.Parameters.AddWithValue("@compname", logIn.company);
                //com.Parameters.AddWithValue("@RefNo", txtIndentNo.Text);               
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
                    dgProducts.DataSource = dt;

                }
                
                for (int i = 0; i < dgProducts.Rows.Count; i++)
                {

                    //Get Stock Report
                    DateTime t = dpSODate.Value;
                    //string f1 = t.ToString("dd/MMM/yyyy");
                    //var stock = (from data in db.ShowStockLedger_New(logIn.company, Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value), t,t,logIn.BU_ID) select data).ToList();
                    string dt1 = t.ToString("yyyy/MM/dd");
                    var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(Item_Code.ToString()), Convert.ToDateTime(dt1), logIn.BU_ID) select data).ToList();

                    if (stock.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        dgProducts.Rows[i].Cells["Stock_Qty"].Value = stock[0].ClosingQty;
                        //dgProducts.Rows[i].Cells["Price"].Value = stock[0].CBPrice;                        
                        dgProducts.Rows[i].Cells["Price"].Value = stock[0].CBPrice;
                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

                    string pcode = "";
                    if (columnName == "Int_Prod_Code" && R1.Cells["int_Prod_Code"].Value != null)
                    {

                        pcode = R1.Cells["int_Prod_Code"].Value.ToString();
                    }

                    if (columnName == "Item_Description" && R1.Cells["Item_Description"].Value != null)
                    {
                        pcode = R1.Cells["Item_Description"].Value.ToString();
                    }
                    if (pcode != "")
                    {
                        var d = (from data in db.Get_Product_into_Trans(logIn.company, logIn.BU_ID, pcode)
                                 select
                                     new
                                     {
                                         Item_Code = data.prod_id,
                                         data.Prod_Code,
                                         Item_Description = data.Prod_Name,
                                         UOM = data.Uom_Descr,
                                         data.Prod_Field2,
                                         data.Prod_HSN_Code
                                     }).ToList();

                        if (d.Count > 0)
                        {
                            R1.Cells["UOM"].Value = d[0].UOM.ToString();
                            R1.Cells["Item_Description"].Value = d[0].Item_Description;
                            R1.Cells["Item_code"].Value = d[0].Item_Code.ToString();
                            R1.Cells["Int_Prod_Code"].Value = d[0].Prod_Code;
                            if (d[0].Prod_Field2 != null)
                            {
                                R1.Cells["Item_Grade"].Value = d[0].Prod_Field2.ToString();
                            }
                            //if (R1.Cells["S_no"].Value == null || R1.Cells["S_no"].Value.ToString() == "")
                            //{
                            //    R1.Cells["S_no"].Value = dgProducts.Rows.Count - 1;
                            //}

                            R1.Cells["Issued_Qty"].Value = 0;
                        }

                    }
                    else
                    {
                        MessageBox.Show("Record Not Found");
                        return;
                    }

                    DateTime t = dpSODate.Value;
                    string dt1 = t.ToString("yyyy/MM/dd");
                    var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(R1.Cells["Item_Code"].Value.ToString()), Convert.ToDateTime(dt1), logIn.BU_ID) select data).ToList();
                    //R1.Cells["Stock_Qty"].Value = "0";
                    if (stock.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        //R1.Cells["Stock_Qty"].Value = stock[0].ClosingQty;
                        //decimal cValue = Convert.ToDecimal(stock[0].CBPrice);
                        //decimal cStock = Convert.ToDecimal(stock[0].ClosingQty); 
                        R1.Cells["Price"].Value = stock[0].CBPrice;
                    }
                }
                if (columnName == "Item_Spec" && R1.Cells["Item_Spec"].Value != null)
                {

                    var getProductName = (from s in db.Bloom_Roll_Wise_Issues

                                          where s.Prod_ID == Convert.ToInt32(R1.Cells["Item_code"].Value.ToString()) 
                                          && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID && s.RollNo == R1.Cells["Item_Spec"].Value
                                          select  s.RollWt_Issued).Sum();

                    if (getProductName != null)
                    {

                        if (getProductName != null)
                        {
                            R1.Cells["Issued_Qty"].Value = getProductName;
                        }

                    }

                    else
                    {

                        MessageBox.Show("Invalid ARN No");
                        R1.Cells["Item_Spec"].Value = "";
                        return;

                    }
                }

                if (columnName == "Return_Qty" || columnName == "Price")
                {
                    int Itemcode = Convert.ToInt32(R1.Cells["Item_Code"].Value.ToString());                                  
                    if (R1.Cells["Item_Description"].Value != null)
                    {
                        decimal IssuedQty = (R1.Cells["Issued_Qty"].Value == "" || R1.Cells["Issued_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Issued_Qty"].Value);
                        decimal Return_Qty = (R1.Cells["Return_Qty"].Value == "" || R1.Cells["Return_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Return_Qty"].Value);
                       // decimal StockQty = (R1.Cells["Stock_Qty"].Value == "" || R1.Cells["Stock_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Stock_Qty"].Value);
                        decimal price = (R1.Cells["Price"].Value == "" || R1.Cells["Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Price"].Value);
                        if (cmbRefDocType.Text != "NA")
                        {
                            if (Return_Qty > IssuedQty)
                            {
                                MessageBox.Show("Returned Qty Cannot Be Greater Than Issued Qty");
                                R1.Cells["Return_Qty"].Value = 0;
                                return;
                            }
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
                //if (cmbIssuedTo.Text == string.Empty)
                //{
                //    MessageBox.Show("Issued To Should Not Be Empty", "Issues", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //    cmbIssuedTo.Focus();
                //    return;
                //}               
                
                
                if (cmbDepartment.Text == string.Empty)
                {
                    MessageBox.Show("Select Received From", "Issue Retunrs", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbDepartment.Focus();
                    return;
                }
                else
                if (cmbRefDocType.Text == string.Empty)
                {
                    MessageBox.Show("Select Ref Doc Type", "Issue Returns", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbRefDocType.Focus();
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
                if ((from u in db.Material_Returns_JWs where u.Slip_NO == myString && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    myString = txtSlipNo.Text;
                    db.sp_IssueReturns_Delete(myString, logIn.company,logIn.BU_ID);
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
                Material_Returns_JW S = new Material_Returns_JW();
                {
                    S.Slip_NO = myString;
                    S.Slip_Date = dpSODate.Value;
                    //S.Indent_No = txtIndentNo.Text;                  
                    S.Receive_From = Convert.ToInt32(cmbDepartment.SelectedValue.ToString());
                    S.Ref_Doc_No = (cmbRefDocType.Text);
                    //S.Issued_To = (cmbIssuedTo.Text);
                    S.Receive_By = txtIssuedBy.Text;
                   // S.Issued_Person_To = txtIssuedTo.Text;                   
                    S.Remarks = txtRemarks.Text;    
                    S.isDeleted = false;
                    //S.Returnable = checkBox1.Checked;                    
                    S.Company_ID = logIn.company;
                    S.BU_ID = logIn.BU_ID;
                    S.Created_By = lblCreatedBy.Text;
                    S.Modified_By = logIn.username + "-" + DateTime.Now;
                    db.Material_Returns_JWs.InsertOnSubmit(S);
                    db.SubmitChanges();
                }
                //db.Transaction = transaction;
                for (int i = 0; i < dgProducts.RowCount - 1; i++)
                {
                    Material_Returns_JW_Child SC = new Material_Returns_JW_Child();
                    var d1 = (from a in db.Material_Returns_JWs where a.Slip_NO == myString && a.Company_ID == logIn.company select new { a.Id }).ToList();
                    SC.Slip_Master_ID = d1[0].Id;
                    SC.Slip_NO = myString;
                    SC.Prod_Code = (dgProducts.Rows[i].Cells["Item_Code"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value);
                    //SC.Product_Description = (dgProducts.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value).ToString();
                     SC.Prod_Spec = (dgProducts.Rows[i].Cells["Item_spec"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_spec"].Value).ToString();
                    //SC.Prod_Grade = (dgProducts.Rows[i].Cells["Item_Grade"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value).ToString();
                    //SC.Uom = (dgProducts.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["uom"].Value.ToString();
                    SC.Issued_Qty = (dgProducts.Rows[i].Cells["Issued_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Issued_Qty"].Value);
                    SC.Received_Qty = (dgProducts.Rows[i].Cells["Return_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Return_Qty"].Value);
                    SC.Issue_Price = (dgProducts.Rows[i].Cells["Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Price"].Value);
                    SC.Issue_Value = (dgProducts.Rows[i].Cells["Amount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Amount"].Value);
                    SC.Cost_Center = (dgProducts.Rows[i].Cells["Cost_Center"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Cost_Center"].Value).ToString();
                    SC.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value).ToString();
                    SC.ProdSno = i + 1;
                    SC.Company_ID = logIn.company;
                    db.Material_Returns_JW_Childs.InsertOnSubmit(SC);
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
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = "Delete from Bloom_Roll_Wise_Issues where [Doc_Ref]=@param1 and Company_ID =@compName and status ='Open'";
            cmd.Parameters.AddWithValue("@param1", txtSlipNo.Text);
            cmd.Parameters.AddWithValue("@CompName", logIn.company);
            cmd.Connection = con;
            con.Open();
            cmd.ExecuteNonQuery();
            con.Close();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    if ((from a in db.Material_Issue_Masters where a.Company_ID == logIn.company && a.Slip_NO == txtSlipNo.Text select a).Count() > 0)
                    {
                        db.sp_Issues_Delete(txtSlipNo.Text,logIn.company,logIn.BU_ID);
                        MessageBox.Show("Recored Deleted Successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                ////        case "Sub Contractor":
                //var pStatus = (from m in db.Department_Masters where m.Company_ID == logIn.company select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                //if (pStatus.Count > 0)
                //{
                //    cmbDepartment.DataSource = pStatus;
                //    cmbDepartment.ValueMember = "ID";
                //    cmbDepartment.DisplayMember = "Supplier_Name";
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DataTable dtexisting = new DataTable();
            ioneNet.MaterialManagement.Transactions.GetProductsforReturn form = new ioneNet.MaterialManagement.Transactions.GetProductsforReturn();
            DocNo = cmbRefDocType.Text;
            form.ShowDialog();
            if (dgProducts.Rows.Count > 1)
            {
                dtexisting.Rows.Clear();
                dtexisting.Columns.Clear();
                dtexisting.Columns.Add("Item_Code", typeof(string));
                dtexisting.Columns.Add("Int_Prod_Code", typeof(string));
                dtexisting.Columns.Add("Item_Description", typeof(string));
                dtexisting.Columns.Add("Item_Spec", typeof(string));
                dtexisting.Columns.Add("Item_Grade", typeof(string));
                dtexisting.Columns.Add("uom", typeof(string));
                dtexisting.Columns.Add("Issued_Qty", typeof(decimal));
                dtexisting.Columns.Add("Return_Qty", typeof(decimal));              
                dtexisting.Columns.Add("Price", typeof(decimal));
                dtexisting.Columns.Add("Amount", typeof(decimal));
                dtexisting.Columns.Add("Cost_Center", typeof(string));
                dtexisting.Columns.Add("Remarks", typeof(string));


                for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                {
                    DataRow dr;
                    dr = dtexisting.NewRow();
                    dr["Item_Code"] = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                    dr["Int_Prod_Code"] = dgProducts.Rows[i].Cells["Int_Prod_Code"].Value.ToString();
                    dr["Item_Description"] = dgProducts.Rows[i].Cells["Item_Description"].Value.ToString();
                    dr["Item_Spec"] = dgProducts.Rows[i].Cells["Item_Spec"].Value.ToString();
                    dr["Item_Grade"] = dgProducts.Rows[i].Cells["Item_Grade"].Value.ToString();
                    dr["UOM"] = dgProducts.Rows[i].Cells["UOM"].Value.ToString();
                    dr["Issued_Qty"] = dgProducts.Rows[i].Cells["Issued_Qty"].Value.ToString();
                    dr["Return_Qty"] = dgProducts.Rows[i].Cells["Return_Qty"].Value.ToString();                   
                    dr["Price"] = dgProducts.Rows[i].Cells["Price"].Value.ToString();
                    dr["Amount"] = dgProducts.Rows[i].Cells["Amount"].Value.ToString();
                    dr["Cost_Center"] = dgProducts.Rows[i].Cells["Cost_Center"].Value.ToString();
                    dr["Remarks"] = dgProducts.Rows[i].Cells["Remarks"].Value.ToString();
                    dtexisting.Rows.Add(dr);

                }
                dtexisting.AcceptChanges();
            }






            if (ioneNet.MaterialManagement.Transactions.GetProductsforReturn.dtgetproducts.Rows.Count > 0)
            {
                DataTable dt = new DataTable();
                dt.Columns.Add("Item_Code", typeof(string));
                dt.Columns.Add("Int_Prod_Code", typeof(string));
                dt.Columns.Add("Item_Description", typeof(string));
                dt.Columns.Add("Item_Spec", typeof(string));
                dt.Columns.Add("Item_Grade", typeof(string));
                dt.Columns.Add("UOM", typeof(string));
                dt.Columns.Add("Issued_Qty", typeof(decimal));
                dt.Columns.Add("Return_Qty", typeof(decimal));              
                dt.Columns.Add("Price", typeof(decimal));
                dt.Columns.Add("Amount", typeof(decimal));
                dt.Columns.Add("Cost_Center", typeof(string));
                dt.Columns.Add("Remarks", typeof(string));

                //dt.Rows.Add();
                for (int i = 0; i < ioneNet.MaterialManagement.Transactions.GetProductsforReturn.dtgetproducts.Rows.Count; i++)
                {
                    string prodcode = ioneNet.MaterialManagement.Transactions.GetProductsforReturn.dtgetproducts.Rows[i]["prod_id"].ToString();
                    string mrpno = ioneNet.MaterialManagement.Transactions.GetProductsforReturn.dtgetproducts.Rows[i]["MRPNo"].ToString();
                    string issqty = ioneNet.MaterialManagement.Transactions.GetProductsforReturn.dtgetproducts.Rows[i]["Issued_Qty"].ToString();                    
                    string price = ioneNet.MaterialManagement.Transactions.GetProductsforReturn.dtgetproducts.Rows[i]["Price"].ToString();

                    var getproducts = (from obj in db.Products
                                       join uom in db.UoM_Masters on obj.Prod_Primary_UOM_Id equals uom.UOM_ID
                                       where obj.Prod_Code == prodcode
                                       select new
                                       {


                                           Item_Code = obj.prod_ID,
                                           Int_Prod_Code = obj.Prod_Code,
                                           Item_Description = obj.Prod_Name,
                                           Item_Spec = "",
                                           Item_Grade = "",
                                           UOM = uom.Uom_Descr,
                                           Issued_Qty = issqty,
                                           Return_Qty = "0.00",
                                           Price = price,
                                           Amount = 0,
                                           Cost_Center = mrpno,
                                           Remarks = ""
                                       }).ToList();
                    dt.Rows.Add(getproducts[0].Item_Code, getproducts[0].Int_Prod_Code, getproducts[0].Item_Description, getproducts[0].Item_Spec, getproducts[0].Item_Grade, getproducts[0].UOM, getproducts[0].Issued_Qty, getproducts[0].Return_Qty,  getproducts[0].Price, getproducts[0].Amount, getproducts[0].Cost_Center, getproducts[0].Remarks);

                }

                dtexisting = dtexisting.AsEnumerable().Union(dt.AsEnumerable()).CopyToDataTable();
                dgProducts.DataSource = dtexisting;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {

           
        }

        private void brnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //  bindCashAct();
                MaterialManagement.Transactions.frmMaterialReturnVouchers obj = new MaterialManagement.Transactions.frmMaterialReturnVouchers();

                if (obj.ShowDialog() == DialogResult.OK)
                {
                    txtSlipNo.Text = MaterialManagement.Transactions.frmMaterialReturnVouchers.voucherNo;

                    if (!string.IsNullOrEmpty(txtSlipNo.Text))
                    {
                        var dm1 = (from s in db.Material_Returns_JW_Childs
                                         join a in db.Material_Returns_JWs on s.Slip_Master_ID equals a.Id
                                         join p in db.Products on s.Prod_Code equals p.prod_ID
                                   where s.Slip_NO == txtSlipNo.Text && s.Company_ID == logIn.company && a.BU_ID == logIn.BU_ID
                                   select new
                                   {
                                       Item_Code= s.Prod_Code,
                                       Int_Prod_Code = (logIn.company == 1044 ? p.Prod_Alternative_Code : p.Prod_Code),
                                       Item_Description = p.Prod_Name,
                                       Item_Spec= s.Prod_Spec,
                                       Item_Grade = s.Prod_Grade,
                                       UOM = s.Uom,
                                       s.Issued_Qty,
                                       Return_Qty = s.Received_Qty,                                       
                                       Price=s.Issue_Price,
                                       Amount=s.Issue_Value,
                                       s.Cost_Center,
                                       s.Remarks
                                   });


                        SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        DataTable dtr = new DataTable();
                        da2.Fill(dtr);
                        if (dtr.Rows.Count >= 0)
                            dgProducts.DataSource = dtr;


                    }

                    var f = (from s in db.Material_Returns_JWs where s.Slip_NO == txtSlipNo.Text && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID select s).FirstOrDefault();
                    if (f != null)
                    {
                        dpSODate.Text = f.Slip_Date.ToString();
                        cmbRefDocType.Text = f.Ref_Doc_No;
                    //    txtIndentNo.Text = f.Indent_No;
                      //  cmbIssuedTo.Text = f.Issued_To;
                        bindDept();
                        cmbDepartment.SelectedValue = f.Receive_From;
                       
                        txtRemarks.Text = f.Remarks;
                        //ObDate.Text = f.OBDate.ToString();
                        txtIssuedBy.Text = f.Receive_By.ToString();
                       // txtIssuedTo.Text = f.Issued_Person_To.ToString();
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
                cmbRefDocType.Text = "";
               // txtIndentNo.Text = "";
                //cmbIssuedTo.Text = "";
                cmbDepartment.Text = "";
                txtIssuedBy.Text = logIn.username;
                //txtIssuedTo.Text = "";
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
