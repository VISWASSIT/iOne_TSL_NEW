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
using System.Data.OleDb;

namespace ioneNet.MaterialManagement.Transactions
{
    public partial class MaterialIndent : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        System.Data.Common.DbTransaction transaction;
        public static string DocNo, ItemCode, RecQty, Suppname,frmname;
        public MaterialIndent()
        {
            InitializeComponent();
        }

        private void frmMaterialIssues_Load(object sender, EventArgs e)
        {
            frmname = "MaterialIndent";
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            dpSODate.MinDate = logIn.fy_Start_Date;
            dpSODate.MaxDate = logIn.fy_End_Date;
            bindDept();
            depbind();
            pbind();

            if (MaterialManagement.Transactions.MaterialIndentList.var == "0")
            {
                if (MaterialIndentList.editMode == true)
                {
                    txtSlipNo.Text = MaterialManagement.Transactions.MaterialIndentList.SO_No;
                    bindedit();
                    
                }
            }
            else
            if (MaterialManagement.Transactions.MaterialIndentList.var == "1")
            {
                if (MaterialIndentList.editMode == true)
                {
                    txtSlipNo.Text = MaterialManagement.Transactions.MaterialIndentList.SO_No;
                    bindedit();
                    btnSave.Enabled = false;
                    btnClear.Enabled = false;


                }
            }
            else
            
            if (MaterialManagement.frmMMDashBoard.var == "0")
            {
                if (MaterialManagement.frmMMDashBoard.editMode == true)
                {
                    txtSlipNo.Text = MaterialManagement.frmMMDashBoard.SO_No;
                    bindedit();
                    
                }
            }
            
            else
            {
                //AutoincrementId();
            }

           
        }

        public void AutoincrementId()
        {
            try
            {

                var result = db.Sp_autoincrement_MaterialIndent(logIn.company,logIn.BU_ID, logIn.fy_Start_Date, logIn.fy_End_Date);
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

            //Bind Department
            string cValue = cmbDepartment.Text;
            

           
            //Status
            var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Trans" && (m.Company_ID == logIn.company || m.Company_ID == 0) select new { m.ID, m.Descr }).Distinct().ToList();
            if (pStatus.Count > 0)
            {
                cmbStatus.DataSource = pStatus;
                cmbStatus.ValueMember = "ID";
                cmbStatus.DisplayMember = "Descr";
            }

            //Status
            //var pCodes = (from m in db.Engg_Mfg_Orders where m.Company_ID == logIn.company select new { m.id, m.MO_No }).Distinct().ToList();
            //if (pCodes.Count > 0)
            //{
            //    cmbAssetCode.DataSource = pCodes;
            //    cmbAssetCode.ValueMember = "id";
            //    cmbAssetCode.DisplayMember = "MO_No";
            //}
            //cmbAssetCode.SelectedIndex = -1;
        }

        public void depbind()
        {
            var Dept = (from m in db.Department_Masters where m.Company_ID == logIn.company select new { m.Id, m.Dept_Name }).Distinct().ToList();
            if (Dept.Count > 0)
            {
                cmbDepartment.DataSource = Dept;
                cmbDepartment.ValueMember = "Id";
                cmbDepartment.DisplayMember = "Dept_Name";
            }
            cmbDepartment.SelectedIndex =-1 ;
        }
        public void pbind()
        {
            var pj = (from a in db.Project_code_Masters
                      where a.Company_ID == logIn.company
                      select new
                      {
                          a.id,

                          a.Project_Code,


                      }).ToList();
            if (pj.Count > 0)
            {
                cmbAssetCode.DataSource = pj;
                cmbAssetCode.ValueMember = "id";
                cmbAssetCode.DisplayMember = "Project_Code";

            }
            cmbAssetCode.SelectedIndex = -1;
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

                if (columnName == "Item_Code" && R1.Cells["Item_Code"].Value != null)
                {
                    
                }
                

                if (columnName == "Item_Description" && R1.Cells["Item_Description"].Value != null)
                {

                var getProductName = (from s in db.Products
                                        join u in db.UoM_Masters on s.Prod_Primary_UOM_Id equals u.UOM_ID
                                        join g in db.Product_Groups on s.Prod_Group_Id equals g.ID
                                        where s.Prod_Name == R1.Cells["Item_Description"].Value.ToString() && s.Company_ID == logIn.company
                                        select new { s.prod_ID, s.Prod_Code, u.Uom_Descr, g.Prod_Group_Name, s.Prod_HSN_Code,s.Prod_Field2,s.Prod_Alternative_Code }).FirstOrDefault();

                if (getProductName != null)
                {
                    R1.Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                    R1.Cells["Prod_Code"].Value = getProductName.prod_ID.ToString();
                    R1.Cells["Item_code"].Value = (logIn.company == 1044 ? getProductName.Prod_Alternative_Code.ToString() : getProductName.Prod_Code.ToString());

                    if (getProductName.Prod_Field2 != null)
                    {
                        R1.Cells["Item_Grade"].Value = getProductName.Prod_Field2.ToString();
                    }
                    if (R1.Cells["S_no"].Value == null || R1.Cells["S_no"].Value.ToString() == "")
                    {
                        R1.Cells["S_no"].Value = dgProducts.Rows.Count - 1;
                    }

                }

                else
                {
                    MessageBox.Show("Invalid Product Name Entered");
                    R1.Cells["Item_Description"].Value = "";
                    return;
                }
                DateTime t = dpSODate.Value;
                string dt1 = t.ToString("yyyy/MM/dd");
                string Item_Code = R1.Cells["Prod_Code"].Value.ToString();
                var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(Item_Code), Convert.ToDateTime(dt1), logIn.BU_ID) select data).ToList();
                R1.Cells["Stock_Qty"].Value = "0";
                if (stock.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    R1.Cells["Stock_Qty"].Value = stock[0].ClosingQty;
                    //R1.Cells["Price"].Value = stock[0].CBPrice;
                }
                }

                if (columnName == "Issue_Qty")
                {
                    int Itemcode = Convert.ToInt32(R1.Cells["Item_Code"].Value.ToString());                                  
                    if (R1.Cells["Item_Description"].Value != null)
                    {
                        //decimal IssuedQty = (R1.Cells["Issue_Qty"].Value == "" || R1.Cells["Issue_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Issue_Qty"].Value);
                        //decimal IndentQty = (R1.Cells["Indent_Qty"].Value == "" || R1.Cells["Indent_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Indent_Qty"].Value);
                        //decimal StockQty = (R1.Cells["Stock_Qty"].Value == "" || R1.Cells["Stock_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Stock_Qty"].Value);
                        //decimal price = (R1.Cells["Price"].Value == "" || R1.Cells["Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Price"].Value);
                        //if (cmbRefDocType.Text != "NA")
                        //{
                        //    if (IssuedQty > IndentQty)
                        //    {
                        //        MessageBox.Show("Issued Qty Cannot Be Greater Than Indent Qty");
                        //        R1.Cells["Issue_Qty"].Value = 0;
                        //        return;
                        //    }
                        //}
                        //if (IssuedQty <= StockQty)
                        //{
                        //    decimal Amt, DiscAmt, netAmt, gst, igst, totamt;

                        //    Amt = IssuedQty * price;
                        //    R1.Cells["Amount"].Value = Amt.ToString("0.00");
                        //}
                        //else
                        //{
                        //    MessageBox.Show("Issued Qty Cannot Be Greater Than Stock Qty");
                        //    R1.Cells["Issue_Qty"].Value = 0;
                        //    return;
                        //}                        
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
                tb3.AutoCompleteCustomSource = null;
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
                            //var Prodname = (from d in db.GoodsReceiptNote_Childs where d.Company_ID == logIn.company select new { d.Prod_Grade }).Distinct().ToList();
                            //DataTable dt = new DataTable();
                            //dt.Columns.Add("Prod_Grade");
                            //foreach (var item in Prodname)
                            //{
                            //    dt.Rows.Add(item.Prod_Grade);
                            //}
                            //for (int i = 0; i < dt.Rows.Count; i++)
                            //{
                            //    coll.Add(dt.Rows[i][0].ToString());
                            //}
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
                Boolean Rec = false;
                if (cmbDepartment.Text == string.Empty)
                {
                    MessageBox.Show("Department Name Should Not Be Empty", "Material Indent", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbDepartment.Focus();
                    return;
                }

                else
                if (cmbStatus.Text == string.Empty)
                {
                    MessageBox.Show("Select Status", "Material Indent", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbStatus.Focus();
                    return;
                }
                else
                //Check Wether 1st Row is filled or not
                   
                    
                if (dgProducts.Rows[0].Cells["Item_Code"].Value == null) 
                {
                    MessageBox.Show("Atleast One Record To Be Entered To Proceed", "Material Indent", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    SavePR();
                }
                    
                    
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Material Indent", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void SavePR()
        {
            try
            {
                //db.Connection.Open();
                //transaction = db.Connection.BeginTransaction();
                //db.Transaction = transaction;

                String myString = "";
                myString = txtSlipNo.Text;
                if(txtSlipNo.Text !="")
              //  if ((from u in db.Purchase_Req_Masters where u.PR_NO == myString && u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID select u).Count() > 0)
                {
                    //Set PR Temporarly Deleted Mode
                    myString = txtSlipNo.Text;
                    //var ci = db.Purchase_Req_Masters.Where(w => w.PR_NO == myString && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                    //{
                    //    ci.isDeleted = true;                                              
                    //    db.SubmitChanges();

                    //}
                    db.sp_Indent_Delete(myString, logIn.company, logIn.BU_ID);

                    //SqlCommand cmd1 = new SqlCommand("update Purchase_Req_Child set [ProdSno] ='1' where PR_NO =@ProdID and Company_ID =@compname", con);
                    //cmd1.Parameters.AddWithValue("@ProdID", myString);
                    //cmd1.Parameters.AddWithValue("@compname", logIn.company);

                    //if (con.State != ConnectionState.Open)
                    //    con.Open();
                    ////con.Open();
                    //cmd1.ExecuteNonQuery();
                    //con.Close();                    
                }
                else
                {
                    AutoincrementId();
                    myString = txtSlipNo.Text;

                }
             
                Material_Indent_Master S = new Material_Indent_Master();
                {
                    S.Indent_NO = myString;
                    S.Indent_Date = dpSODate.Value;
                    S.Dept_Name = Convert.ToInt32(cmbDepartment.SelectedValue.ToString());
                     S.Ref_Doc = txtRefDoc.Text;
                    S.MO_No = Convert.ToInt32(cmbmono.SelectedValue);
                    S.Asset_Code = Convert.ToInt32(cmbAssetCode.SelectedValue);

                    S.Remarks = txtRemarks.Text;
                    S.isDeleted = false;
                    S.Indented_By = txtIndentedBy.Text;
                    S.Indentor_Mobile = txtIndentMobile.Text;
                    S.BU_ID = logIn.BU_ID;
                    //S.Returnable = checkBox1.Checked;  
                    S.Status  = Convert.ToInt32(cmbStatus.SelectedValue.ToString());
                    S.Company_ID = logIn.company;
                    S.Created_By = lblCreatedBy.Text;
                    S.Modified_By = logIn.username + "-" + DateTime.Now;
                    db.Material_Indent_Masters.InsertOnSubmit(S);
                    db.SubmitChanges();
                }
                    //db.Transaction = transaction;
                for (int i = 0; i < dgProducts.RowCount - 1; i++)
                {
                    Material_Indent_Child SC = new Material_Indent_Child();
                    var d1 = (from a in db.Material_Indent_Masters where a.Indent_NO == myString && a.Company_ID == logIn.company && a.BU_ID == logIn.BU_ID && a.isDeleted ==false select new { a.Id }).ToList();
                    SC.Indent_Master_ID = d1[0].Id;
                    SC.Indent_NO = myString;
                    SC.ProdSno = (dgProducts.Rows[i].Cells["Prod_Code"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["Prod_Code"].Value);

                    SC.Prod_Code = (dgProducts.Rows[i].Cells["Item_Code"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Code"].Value).ToString();
                    SC.Product_Description = (dgProducts.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value).ToString();
                    SC.Prod_Spec = (dgProducts.Rows[i].Cells["Item_spec"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_spec"].Value).ToString();
                    SC.Prod_Grade = (dgProducts.Rows[i].Cells["Item_Grade"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value).ToString();
                    SC.Uom = (dgProducts.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["uom"].Value.ToString();
                    SC.Indent_Qty = (dgProducts.Rows[i].Cells["Indent_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Indent_Qty"].Value);
                    SC.Stock_Qty = (dgProducts.Rows[i].Cells["Stock_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Stock_Qty"].Value);
                    SC.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value).ToString();
                    SC.S_No = Convert.ToInt32(dgProducts.Rows[i].Cells["S_No"].Value);
                    SC.Company_ID = logIn.company;
                    db.Material_Indent_Childs.InsertOnSubmit(SC);
                }
                db.SubmitChanges();
                MessageBox.Show("Record Saved / Updated Successfully With Transaction Ref No : " + txtSlipNo.Text);
                //Deleting Old Data which set in delete mode
                
                clear();               
                //this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                //transaction.Rollback();
            }           
        }

                
        private void btnClose_Click(object sender, EventArgs e)
        {
            
            this.Close();
        }

        private void dgProducts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F6 )
            {
                if (dgProducts.Rows.Count > 0)
                {
                    //DataGridViewRow i = dgProducts.Rows[dgProducts.CurrentRow.Index];
                    //foreach (DataGridViewCell oneCell in dgProducts.SelectedCells)
                    //{
                    //if (oneCell.Selected)
                    dgProducts.Rows.RemoveAt(dgProducts.CurrentRow.Index);

                    int l = 0;
                    for (int m = 0; m <= dgProducts.Rows.Count - 2; m++)
                    {
                        if (dgProducts.Rows[m].Cells["Prod_Code"].Value != "")
                        {
                            l = l + 1;
                            dgProducts.Rows[m].Cells["S_no"].Value = l.ToString();
                        }
                    }
                    //}
                }

            }
            DataTable dtexisting = new DataTable();
            if (e.KeyCode == Keys.F2)
            {
                ioneNet.Masters.ProdSearch form = new ioneNet.Masters.ProdSearch();
                ioneNet.Masters.ProdSearch.frmName = "Indent";
                form.ShowDialog();
                if (dgProducts.Rows.Count > 1)
                {
                    dtexisting.Rows.Clear();
                    dtexisting.Columns.Clear();
                    dtexisting.Columns.Add("S_No", typeof(string));
                    dtexisting.Columns.Add("Prod_Code", typeof(string));
                    dtexisting.Columns.Add("Item_Code", typeof(string));
                    dtexisting.Columns.Add("Item_Description", typeof(string));
                    dtexisting.Columns.Add("Item_Spec", typeof(string));
                    dtexisting.Columns.Add("Item_Grade", typeof(string));
                    dtexisting.Columns.Add("UOM", typeof(string));
                    dtexisting.Columns.Add("Indent_Qty", typeof(string));
                    dtexisting.Columns.Add("Stock_Qty", typeof(string));    
                    dtexisting.Columns.Add("Remarks", typeof(string));

                    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                    {
                        DataRow dr;
                        dr = dtexisting.NewRow();
                        dr["S_No"] = dgProducts.Rows[i].Cells["S_No"].Value.ToString();
                        dr["Prod_Code"] = dgProducts.Rows[i].Cells["Prod_Code"].Value.ToString();
                        dr["Item_Code"] = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                        dr["Item_Description"] = dgProducts.Rows[i].Cells["Item_Description"].Value.ToString();
                        dr["Item_Spec"] = (dgProducts.Rows[i].Cells["Item_Spec"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Item_Spec"].Value);
                        dr["Item_Grade"] = (dgProducts.Rows[i].Cells["Item_Grade"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value);
                        dr["UOM"] = dgProducts.Rows[i].Cells["UOM"].Value.ToString();
                        dr["Indent_Qty"] = (dgProducts.Rows[i].Cells["Indent_Qty"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Indent_Qty"].Value);
                        dr["Stock_Qty"] = dgProducts.Rows[i].Cells["Stock_Qty"].Value.ToString();                       
                        dr["Remarks"] = (dgProducts.Rows[i].Cells["Remarks"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value);
                        dtexisting.Rows.Add(dr);

                    }
                    dtexisting.AcceptChanges();
                }






                if (ioneNet.Masters.ProdSearch.dtgetproducts.Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("S_No", typeof(string));
                    dt.Columns.Add("Prod_Code", typeof(string));
                    dt.Columns.Add("Item_Code", typeof(string));
                    dt.Columns.Add("Item_Description", typeof(string));
                    dt.Columns.Add("Item_Spec", typeof(string));
                    dt.Columns.Add("Item_Grade", typeof(string));
                    dt.Columns.Add("UOM", typeof(string));
                    dt.Columns.Add("Indent_Qty", typeof(string));
                    dt.Columns.Add("Stock_Qty", typeof(string));
                    dt.Columns.Add("Remarks", typeof(string));
                    int j = dgProducts.Rows.Count - 1;
                    //dt.Rows.Add();
                    for (int i = 0; i < ioneNet.Masters.ProdSearch.dtgetproducts.Rows.Count; i++)
                    {
                        string prodcode = ioneNet.Masters.ProdSearch.dtgetproducts.Rows[i]["prod_id"].ToString();
                        string prod_code = ioneNet.Masters.ProdSearch.dtgetproducts.Rows[i]["prod_code"].ToString();
                        var getproducts = (from obj in db.Products
                                           join uom in db.UoM_Masters on obj.Prod_Primary_UOM_Id equals uom.UOM_ID
                                           where obj.prod_ID == Convert.ToInt32(prodcode)
                                           select new
                                           {

                                               // {
                                               Prod_Code = obj.prod_ID,
                                               Item_Code = obj.Prod_Code,
                                               Item_Description = obj.Prod_Name,
                                               Item_Spec = "",
                                               Item_Grade = obj.Prod_Field2,
                                               UOM = uom.Uom_Descr,
                                               Indent_Qty = 0,
                                               Stock_Qty = 0,
                                               Remarks = ""
                                           }).ToList();
                        j = j + 1;
                        dt.Rows.Add(j, getproducts[0].Prod_Code, getproducts[0].Item_Code, getproducts[0].Item_Description, getproducts[0].Item_Spec, getproducts[0].Item_Grade, getproducts[0].UOM, getproducts[0].Indent_Qty, getproducts[0].Stock_Qty, getproducts[0].Remarks);

                    }

                    dtexisting = dtexisting.AsEnumerable().Union(dt.AsEnumerable()).CopyToDataTable();
                    dgProducts.DataSource = dtexisting;
                    int l = 0;
                    for (int m = 0; m <= dgProducts.Rows.Count - 2; m++)
                    {
                        if (dgProducts.Rows[m].Cells["Prod_Code"].Value != "")
                        {
                            l = l + 1;
                            dgProducts.Rows[m].Cells["S_No"].Value = l.ToString();
                        }
                    }
                }

                for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                {
                    string Item_Code = dgProducts.Rows[i].Cells["Prod_Code"].Value.ToString();
                    DateTime t = dpSODate.Value;

                    string dt1 = t.ToString("yyyy/MM/dd");
                    var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(Item_Code.ToString()), Convert.ToDateTime(dt1), logIn.BU_ID) select data).ToList();
                    dgProducts.Rows[i].Cells["Stock_Qty"].Value = "0";
                    if (stock.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        dgProducts.Rows[i].Cells["Stock_Qty"].Value = stock[0].ClosingQty;
                        // R1.Cells["Price"].Value = stock[0].CBPrice;
                    }
                }
                


            }

            }

        private void frmMaterialIssues_FormClosed(object sender, FormClosedEventArgs e)
        {
            
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    if ((from a in db.Material_Indent_Masters where a.Company_ID == logIn.company && a.Indent_NO == txtSlipNo.Text select a).Count() > 0)
                    {
                        db.sp_Indent_Delete(txtSlipNo.Text,logIn.company,logIn.BU_ID);
                        MessageBox.Show("Recored Deleted Successfully");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnFind_Click(object sender, EventArgs e)
        {

           
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ioneNet.MaterialManagement.Departments frm = new ioneNet.MaterialManagement.Departments();
            //frm.MdiParent = this.MdiParent;

            frm.Show();
        }

        private void cmbDepartment_Enter(object sender, EventArgs e)
        {
            try
            {
                //Bind UOM
                depbind();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void cmbAssetCode_Leave(object sender, EventArgs e)
        {
            int i=cmbAssetCode.FindString(cmbAssetCode.Text);
            if (i < 0)
            {
                MessageBox.Show("Invalid Project Code");
                cmbAssetCode.Focus();
            }
            else
            {
                var dm1 = (from s in db.Engg_Mfg_Orders
                               //  join a in db.Storage_Locations on s.Storage_Location equals a.Storage_Loc_Id
                           where s.Project_ID == Convert.ToInt32(cmbAssetCode.SelectedValue) && s.Company_ID == logIn.company
                           select new
                           { s.MO_No, s.id }).ToList();
                if (dm1.Count > 0)
                {
                    cmbmono.DataSource = dm1;
                    cmbmono.ValueMember = "id";
                    cmbmono.DisplayMember = "MO_No";
                }
                cmbmono.SelectedIndex = -1;
            }
        }

        private void dgProducts_Leave(object sender, EventArgs e)
        {

        }

        private void cmbDepartment_Leave(object sender, EventArgs e)
        {
            int i = cmbDepartment.FindString(cmbDepartment.Text);
            if (i < 0)
            {
                MessageBox.Show("Invalid Department Name");
                cmbDepartment.Focus();
            }
        }

        private void cmbAssetCode_Enter(object sender, EventArgs e)
        {
            string cmbval = cmbAssetCode.Text;
            pbind();
            cmbAssetCode.Text = cmbval;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string filename = "";
                OpenFileDialog fdlg = new OpenFileDialog();
                fdlg.Title = "Select file";
                fdlg.InitialDirectory = @"c:\";
                //  fdlg.FileName = txtChooseFile.Text;
                fdlg.Filter = "Excel Sheet(*.xls)|*.xls|All Files(*.*)|*.*";
                fdlg.FilterIndex = 1;
                fdlg.RestoreDirectory = true;
                if (fdlg.ShowDialog() == DialogResult.OK)
                {
                    filename = fdlg.FileName;
                    Application.DoEvents();
                }


                Cursor.Current = Cursors.WaitCursor;
                System.Data.OleDb.OleDbConnection MyConnection;
                System.Data.DataTable DtSet;
                System.Data.OleDb.OleDbDataAdapter MyCommand;
                string SheetName = "Sheet1";
                // string ExcellSheet = ;

                string str = "Provider = Microsoft.ACE.OLEDB.12.0; Data source=" + filename + ";Extended Properties=\"Excel 8.0;HDR= yes;\";";
                MyConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended Properties='Excel 8.0;HDR=Yes'");

                MyCommand = new System.Data.OleDb.OleDbDataAdapter("Select * From [" + SheetName + "$] ", MyConnection);
                //MyCommand = new System.Data.OleDb.OleDbDataAdapter("Select * From  [" + txtExcellSheet.Text + "$] ", MyConnection);
                MyCommand.TableMappings.Add("Table", filename);
                DtSet = new System.Data.DataTable();
                MyCommand.Fill(DtSet);
                int count = DtSet.Rows.Count;
                DataTable dt = new DataTable();
                System.Data.DataRow dr = null; //dataGrdView.Visible = true;
                                               //dataGrdView.DataSource = dtExcel;
               
                dt.Columns.Add(new DataColumn("Item_Code", typeof(string))); 
                dt.Columns.Add(new DataColumn("Indent_Qty", typeof(string)));               
                for (int i = 0; i < count; i++)
                {
                    dr = dt.NewRow();
                    //txtProdID.Text = result.FirstOrDefault().Product_Code;
                    //dr["Prod_Code"] = result.FirstOrDefault().Product_Code; ;
                    
                    dr["Item_Code"] = DtSet.Rows[i]["Item_Code"].ToString();                    
                    dr["Indent_Qty"] = DtSet.Rows[i]["Indent_Qty"].ToString();                     


                    dt.Rows.Add(dr);
                }

                dgProducts.DataSource = dt;
                MyConnection.Close();
                for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                {
                    var getProductName = (from s in db.Products
                                          join u in db.UoM_Masters on s.Prod_Primary_UOM_Id equals u.UOM_ID
                                          join g in db.Product_Groups on s.Prod_Group_Id equals g.ID
                                          where s.Prod_Code == dgProducts.Rows[i].Cells["Item_Code"].Value.ToString() && s.Company_ID == logIn.company
                                          select new { s.prod_ID, s.Prod_Name, u.Uom_Descr, s.Prod_Description, g.Prod_Group_Name, s.Prod_HSN_Code, s.Prod_Field2 }).FirstOrDefault();

                    if (getProductName != null)
                    {
                        dgProducts.Rows[i].Cells["Prod_Code"].Value = getProductName.prod_ID.ToString();
                        dgProducts.Rows[i].Cells["Item_Description"].Value = getProductName.Prod_Name.ToString();
                        dgProducts.Rows[i].Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                        dgProducts.Rows[i].Cells["Item_Spec"].Value = getProductName.Prod_Field2.ToString();
                        

                        if (dgProducts.Rows[i].Cells["S_No"].Value == null || dgProducts.Rows[i].Cells["S_No"].Value.ToString() == "")
                        {
                            dgProducts.Rows[i].Cells["S_No"].Value = i + 1;
                        }

                        //DateTime t = dpSODate.Value;
                        //string dt1 = t.ToString("yyyy/MM/dd");
                        //string Item_Code = getProductName.prod_ID.ToString();
                        //var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(Item_Code), Convert.ToDateTime(dt1), logIn.BU_ID) select data).ToList();
                        //dgProducts.Rows[i].Cells["Stock_Qty"].Value = "0";
                        //if (stock.Count > 0)
                        //{
                        //    //dgProductsList.DataSource = d;
                        //    dgProducts.Rows[i].Cells["Stock_Qty"].Value = stock[0].ClosingQty;
                        //    //R1.Cells["Price"].Value = stock[0].CBPrice;
                        //}

                    }
                    else
                    {
                        dgProducts.Rows[i].Cells["Item_Code"].Style.ForeColor = Color.Red;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }


        }

        private void btnsrch_Click(object sender, EventArgs e)
        {
            Search_MO_No s = new Search_MO_No();
            s.ShowDialog();
        }

        private void brnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //  bindCashAct();
                MaterialManagement.Transactions.frmMaterialIssueVouchers obj = new MaterialManagement.Transactions.frmMaterialIssueVouchers();

                if (obj.ShowDialog() == DialogResult.OK)
                {
                    txtSlipNo.Text = MaterialManagement.Transactions.frmMaterialIssueVouchers.voucherNo;

                    if (!string.IsNullOrEmpty(txtSlipNo.Text))
                    {
                        var dm1 = (from s in db.Material_Issue_Childs
                                       //  join a in db.Storage_Locations on s.Storage_Location equals a.Storage_Loc_Id
                                   where s.Slip_NO == txtSlipNo.Text && s.Company_ID == logIn.company
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

                    var f = (from s in db.Material_Issue_Masters where s.Slip_NO == txtSlipNo.Text && s.Company_ID == logIn.company select s).FirstOrDefault();
                    if (f != null)
                    {
                        dpSODate.Text = f.Slip_Date.ToString();
                        //cmbRefDocType.Text = f.Ref_Doc_Type;                   
                        cmbDepartment.Text = f.Issued_To;
                        bindDept();
                        cmbDepartment.SelectedValue = f.Dept_Name;
                        //if (f.Returnable == true)
                        //{
                        //    checkBox1.Checked = true;
                        //}
                        //else
                        //{
                        //    checkBox1.Checked = false;
                        //}
                        txtRemarks.Text = f.Remarks;
                        //ObDate.Text = f.OBDate.ToString();
                        //txtIssuedBy.Text = f.Issued_By.ToString();
                      //  txtIssuedTo.Text = f.Issued_Person_To.ToString();
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
                //cmbRefDocType.Text = "";
              
                cmbDepartment.Text = "";
                cmbDepartment.Text = "";
                cmbStatus.Text = "";
                txtIndentedBy.Text = "";
                txtIndentMobile.Text = "";
                txtRefDoc.Text = "";
              //  txtIssuedBy.Text = logIn.username;
               // txtIssuedTo.Text = "";
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
                MessageBox.Show(ex.Message + " While  clear Getting Error ", "Purchase Requisition", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
        }
        public void bindedit()
        {
            try
            {
               
               
                String myString = "";
                int Master_Id =0;        
                var da = (from obj in db.Material_Indent_Masters
                          where obj.Indent_NO == txtSlipNo.Text && obj.Company_ID == logIn.company && obj.BU_ID == logIn.BU_ID
                          select obj).ToList();

                if (da.Count > 0)
                {
                    txtSlipNo.Text = da[0].Indent_NO.ToString();
                    dpSODate.Text = da[0].Indent_Date.ToString();
                   // bindedit();
                    cmbDepartment.SelectedValue = da[0].Dept_Name;
                    cmbAssetCode.SelectedValue = Convert.ToInt32(da[0].Asset_Code);
                    var dm2 = (from s in db.Engg_Mfg_Orders
                                   //  join a in db.Storage_Locations on s.Storage_Location equals a.Storage_Loc_Id
                               where s.Project_ID == Convert.ToInt32(cmbAssetCode.SelectedValue) && s.Company_ID == logIn.company
                               select new
                               { s.MO_No, s.id }).ToList();
                    if (dm2.Count > 0)
                    {
                        cmbmono.DataSource = dm2;
                        cmbmono.ValueMember = "id";
                        cmbmono.DisplayMember = "MO_No";
                    }
                    cmbmono.SelectedIndex = -1;

                    cmbmono.SelectedValue = Convert.ToInt32(da[0].MO_No);
                    cmbStatus.SelectedValue = da[0].Status;
                    txtIndentedBy.Text = da[0].Indented_By;
                    txtIndentMobile.Text = da[0].Indentor_Mobile;
                    txtRefDoc.Text = da[0].Ref_Doc;
                    txtRemarks.Text = da[0].Remarks;                  
                    lblCreatedBy.Text = da[0].Created_By;
                    lblModified.Text = da[0].Modified_By;
                    Master_Id = da[0].Id;
                }


                var dm1 = (from s in db.Material_Indent_Childs
                           where s.Indent_Master_ID == Master_Id && s.Company_ID == logIn.company


                           select new

                           {
                               S_No=s.S_No,
                               Prod_Code = s.ProdSno,
                               Item_Code = s.Prod_Code,
                               Item_Description = s.Product_Description,
                               Item_spec = s.Prod_Spec,
                               Item_Grade = s.Prod_Grade,
                               UOM = s.Uom,
                               s.Indent_Qty,
                               s.Stock_Qty,                                                         
                               s.Remarks                               
                           });
                SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataTable dtr = new DataTable();
                da2.Fill(dtr);
                if (dtr.Rows.Count >= 0)
                    dgProducts.DataSource = dtr;
                //int j = 0;
                //for (int i = 0; i <= dgProducts.Rows.Count - 1; i++)
                //{
                //    j = j + 1;
                //    dgProducts.Rows[i].Cells["S_No"].Value = j.ToString();
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
