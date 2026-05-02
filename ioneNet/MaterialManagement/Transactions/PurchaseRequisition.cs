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
using Syncfusion.WinForms.DataGrid;
using System.Windows.Controls;

namespace ioneNet.MaterialManagement.Transactions
{
    public partial class PurchaseRequisition : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        System.Data.Common.DbTransaction transaction;
        public static int status=0;
        public static Boolean modify;
        public static string DocNo, ItemCode, RecQty, Suppname,transname,transno,refdoctype,PR_Basis,PR_Ref_Basis;
        public PurchaseRequisition()
        {
            InitializeComponent();
        }

        private void frmMaterialIssues_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            if (logIn.company != 20)
            {
                txtIndentedBy.Text = logIn.username;
                txtIndentedBy.Enabled = false;
            }
            else
            {
                txtIndentedBy.Enabled = true;
            }
            dpSODate.MinDate = logIn.fy_Start_Date;
            dpSODate.MaxDate = logIn.fy_End_Date;
            bindDept();
            bindproj();
            bindstatus();

            //MessageBox.Show(this.Text);
            if (MaterialManagement.Transactions.PurchaseRequistionsList.var == "0")
            {
                if (PurchaseRequistionsList.editMode == true)
                {
                    txtSlipNo.Text = MaterialManagement.Transactions.PurchaseRequistionsList.SO_No;
                    bindedit();
                    modify = true;
                }
            }
            else
            
            if (MaterialManagement.frmMMDashBoard.var == "0")
            {
                if (MaterialManagement.frmMMDashBoard.editMode == true)
                {
                    txtSlipNo.Text = MaterialManagement.frmMMDashBoard.SO_No;
                    modify = true;
                    bindedit();
                    
                }
            }
            else
             if (MaterialManagement.Transactions.PurchaseRequistionsList.var == "1")
            {
                if (PurchaseRequistionsList.editMode == true)
                {
                    txtSlipNo.Text = MaterialManagement.Transactions.PurchaseRequistionsList.SO_No;
                    bindedit();
                    btnClear.Enabled = false;
                    btnSave.Enabled = false;
                    cmbStatus.Enabled = false;

                }
            }

            else
            {
                if (MaterialManagement.Transactions.PurchaseRequistionsList.var == "2")
                {
                    AutoincrementId();
                }
            }

           
        }

        public void AutoincrementId()
        {
            try
            {

                var result = db.Sp_autoincrement_PurchaseReq(logIn.company,logIn.BU_ID, logIn.fy_Start_Date, logIn.fy_End_Date);
                txtSlipNo.Text = result.FirstOrDefault().Pr_No;
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
            var Dept = (from m in db.Department_Masters where m.Company_ID == logIn.company select new { m.Id, m.Dept_Name }).Distinct().ToList();
            if (Dept.Count > 0)
            {
                cmbDepartment.DataSource = Dept;
                cmbDepartment.ValueMember = "Id";
                cmbDepartment.DisplayMember = "Dept_Name";
            }
            cmbDepartment.Text = cValue;


           
            //Status
            
           

        }
        public void bindstatus()
        {
            var pStatus = (from m in db.Attributes_Datas 
                           join r in db.view_Trans_Auth_Levels
                           on m.ID equals r.Status_Code
                           where r.Menu_Item == this.Text && (r.Company_ID == logIn.company) && r.Role_ID == logIn.UserRoleID select new { m.ID, m.Descr }).Distinct().ToList();
            if (pStatus.Count > 0)
            {
                cmbStatus.DataSource = pStatus;
                cmbStatus.ValueMember = "ID";
                cmbStatus.DisplayMember = "Descr";
            }
        }
        public void bindproj()
        {
            //var pcode = (from m in db.Project_code_Masters where m.Company_ID == logIn.company select new { m.id, m.Project_Code }).Distinct().ToList();
            //if (pcode.Count > 0)
            //{
            //    cmbasset.DataSource = pcode;
            //    cmbasset.ValueMember = "id";
            //    cmbasset.DisplayMember = "Project_Code";
            //}
            //cmbasset.SelectedIndex = -1;
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
                string pcode = "";
                if (columnName == "Item_Spec" && R1.Cells["Item_Spec"].Value != null)
                {
                    pcode = R1.Cells["Item_Spec"].Value.ToString();
                }
                if (columnName == "Item_Description" && R1.Cells["Item_Description"].Value != null)
                {
                    pcode = R1.Cells["Item_Description"].Value.ToString();
                }
                if(pcode !="")
                { 
                var d = (from data in db.Get_Product_into_Trans(logIn.company, logIn.BU_ID, pcode) select 
                             
                             
                             new
                             {
                                 Item_Code = data.prod_id,
                                 data.Prod_Code,
                                 Item_Description = data.Prod_Name,                                 
                                 UOM = data.Uom_Descr,
                                 data.Prod_Field2
                             }).ToList();

                    if (d.Count > 0)
                    {
                        R1.Cells["UOM"].Value = d[0].UOM.ToString();
                        R1.Cells["Item_Code"].Value = d[0].Item_Code.ToString();
                        R1.Cells["Item_Spec"].Value = d[0].Prod_Code.ToString();

                        R1.Cells["Item_Description"].Value = d[0].Item_Description.ToString();

                    if (d[0].Prod_Field2 != null)
                    {
                        R1.Cells["Item_Grade"].Value = d[0].Prod_Field2.ToString();
                    }

                    if (R1.Cells["ProdSno"].Value == null || R1.Cells["ProdSno"].Value.ToString() == "")
                        {
                            R1.Cells["ProdSno"].Value = dgProducts.Rows.Count - 1;
                        }
                        R1.Cells["Indent_Qty"].Value = "0";

                    }
                    else
                    {
                        MessageBox.Show("Record Not Found");
                        return;
                    }

                  

                    //var getProductName = (from s in db.Products
                    //                      join u in db.UoM_Masters on s.Prod_Primary_UOM_Id equals u.UOM_ID
                    //                      join g in db.Product_Groups on s.Prod_Group_Id equals g.ID
                    //                      where s.Prod_Alternative_Code == R1.Cells["Item_Spec"].Value.ToString() && s.Company_ID == logIn.company
                    //                      && s.Purchase_Account == logIn.BU_ID
                    //                      select new { s.prod_ID, u.Uom_Descr, g.Prod_Group_Name, s.Prod_HSN_Code, s.Prod_Name, s.Prod_Field2, s.Prod_Alternative_Code }).ToList();


                    //if (getProductName.Count > 0)
                    //{
                    //    R1.Cells["UOM"].Value = getProductName[0].Uom_Descr.ToString();
                    //    R1.Cells["Item_Code"].Value = getProductName[0].prod_ID.ToString();

                    //    R1.Cells["Item_Description"].Value = getProductName[0].Prod_Name.ToString();

                    //    if (getProductName[0].Prod_Field2 != null)
                    //    {
                    //        R1.Cells["Item_Grade"].Value = getProductName[0].Prod_Field2.ToString();
                    //    }

                    //    if (R1.Cells["ProdSno"].Value == null || R1.Cells["ProdSno"].Value.ToString() == "")
                    //    {
                    //        R1.Cells["ProdSno"].Value = dgProducts.Rows.Count - 1;
                    //    }
                    //    R1.Cells["Indent_Qty"].Value = "0";

                    //}

                    //else
                    //{
                    //    //R1.Cells["Item_Code"].Value = dgProducts.CurrentCell.RowIndex + 1;
                    //    MessageBox.Show("Invalid Product Name");
                    //    R1.Cells["Item_Description"].Value = "";
                    //    return;

                    //}
                    DateTime t = dpSODate.Value;
                    string dt1 = t.ToString("yyyy/MM/dd");
                    var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(R1.Cells["Item_Code"].Value.ToString()), Convert.ToDateTime(dt1), logIn.BU_ID) select data).ToList();
                    R1.Cells["Stock_Qty"].Value = "0";
                    if (stock.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        R1.Cells["Stock_Qty"].Value = stock[0].ClosingQty;

                        //decimal cValue = Convert.ToDecimal(stock[0].ClosingValue);
                        //decimal cStock = Convert.ToDecimal(stock[0].ClosingQty);
                        //R1.Cells["Price"].Value = stock[0].CBPrice;
                    }
                


                //if (columnName == "Item_Description" && R1.Cells["Item_Description"].Value != null)
                //{
                    

                //    var getProductName = (from s in db.Products
                //                          join u in db.UoM_Masters on s.Prod_Primary_UOM_Id equals u.UOM_ID
                //                          join g in db.Product_Groups on s.Prod_Group_Id equals g.ID
                //                          where s.Prod_Name == R1.Cells["Item_Description"].Value.ToString() && s.Company_ID == logIn.company 
                //                          && s.Purchase_Account == logIn.BU_ID
                //                          select new { s.prod_ID, u.Uom_Descr, g.Prod_Group_Name, s.Prod_HSN_Code, s.Prod_Code, s.Prod_Field2,s.Prod_Alternative_Code }).ToList();

                    
                //    if (getProductName.Count > 0)
                //    {
                //        R1.Cells["UOM"].Value = getProductName[0].Uom_Descr.ToString();
                //        R1.Cells["Item_Code"].Value = getProductName[0].prod_ID.ToString();
                //        if (logIn.company == 1044) 
                //        {
                //            R1.Cells["Item_Spec"].Value = getProductName[0].Prod_Alternative_Code.ToString(); 
                //        } 
                //        else 
                //        {
                //            R1.Cells["Item_Spec"].Value = getProductName[0].Prod_Code.ToString(); 
                //        }
                //        if (getProductName[0].Prod_Field2 != null)
                //        {
                //            R1.Cells["Item_Grade"].Value = getProductName[0].Prod_Field2.ToString();
                //        }

                //        if (R1.Cells["ProdSno"].Value == null || R1.Cells["ProdSno"].Value.ToString() == "")
                //        {
                //            R1.Cells["ProdSno"].Value = dgProducts.Rows.Count - 1;
                //        }
                //        R1.Cells["Indent_Qty"].Value = "0";

                //    }

                //    else
                //    {
                //        //R1.Cells["Item_Code"].Value = dgProducts.CurrentCell.RowIndex + 1;
                //        MessageBox.Show("Invalid Product Name");
                //        R1.Cells["Item_Description"].Value = "";
                //        return;

                //    }
                //    DateTime t = dpSODate.Value;
                //    string dt1 = t.ToString("yyyy/MM/dd");
                //    var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(R1.Cells["Item_Code"].Value.ToString()), Convert.ToDateTime(dt1), logIn.BU_ID) select data).ToList();
                //    R1.Cells["Stock_Qty"].Value = "0";
                //    if (stock.Count > 0)
                //    {
                //        //dgProductsList.DataSource = d;
                //        R1.Cells["Stock_Qty"].Value = stock[0].ClosingQty;
                      
                //        //decimal cValue = Convert.ToDecimal(stock[0].ClosingValue);
                //        //decimal cStock = Convert.ToDecimal(stock[0].ClosingQty);
                //        //R1.Cells["Price"].Value = stock[0].CBPrice;
                //    }
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
               System.Windows.Forms.TextBox tb3 = e.Control as System.Windows.Forms.TextBox;
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
        //public void tc()
        //{
        //    txtRefDoc.AutoCompleteMode = AutoCompleteMode.Suggest;
        //    txtRefDoc.AutoCompleteSource = AutoCompleteSource.CustomSource;
        //    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
        //    addItems(DataColl);
        //    txtRefDoc.AutoCompleteCustomSource = DataColl;

        //}
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
                
                if (cmbreftype.Text == "")
                {
                    MessageBox.Show("Select Reference Document Type");
                    cmbreftype.Focus();
                    return;
                }
                
                Boolean Rec = false;
                if (cmbDepartment.Text == string.Empty)
                {
                    MessageBox.Show("Department Name Should Not Be Empty", "Purchase Req", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbDepartment.Focus();
                    return;
                }

                else
                if (cmbStatus.Text == string.Empty)
                {
                    MessageBox.Show("Select Status", "Purchase Req", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbStatus.Focus();
                    return;
                }
                else
                //Check Wether 1st Row is filled or not
                   
                    
                if (dgProducts.Rows[0].Cells["Item_Code"].Value == null) 
                {
                    MessageBox.Show("Atleast One Record To Be Entered To Proceed", "Purchase Req", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    SavePR();
                }
                    
                    
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Purchase Requistion", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void CheckAuthorizations()
        {
            string Status = cmbStatus.Text;
            string aStatus = "";
            int y = 0;
            //Getting wether transaction is already Saved and if true getting the current status of transaction
            if ((from u in db.Purchase_Req_Masters where u.PR_NO == txtSlipNo.Text && u.Company_ID == logIn.company select u).Count() > 0)
            {
                var getStatus = (from m in db.Purchase_Req_Masters
                             join s in db.Attributes_Datas on m.Status equals s.ID
                             where m.Company_ID == logIn.company && m.PR_NO == txtSlipNo.Text
                             
                             select new { m.Status, s.Descr}).Distinct().ToList();
                aStatus = getStatus[0].Descr;
                

             }
            else
            {
                //if the trasaction is new initiation
                aStatus = "Created";
            }

            switch (aStatus)
            {
                case "Created":

                break;
                case "Reviewed":

                break;

                case "Approved":

                break;
            }
            switch (Status)
            {

                case "Reviewed":
                    y = 1;
                    var uRole = (from m in db.view_Trans_Auth_Levels
                                 where m.Company_ID == logIn.company && m.Menu_Item == "Purchase Order"
                                 && m.Role_ID == logIn.UserRoleID && m.Auth_Level == y + 1
                                 select new { m.Auth_Level, m.Auth_Allowed, m.Status_Code }).Distinct().ToList();
                    if (uRole.Count > 0)
                    {

                        MessageBox.Show("Selected PR cannot be Approved as it is not yet Reviewed");
                        return;

                    }
                    break;
                case "Approved":
                    y = 2;
                    var uRole1 = (from m in db.view_Trans_Auth_Levels
                                  where m.Company_ID == logIn.company && m.Menu_Item == "Purchase Order"
                                  && m.Role_ID == logIn.UserRoleID && m.Auth_Level == 2
                                  select new { m.Auth_Level, m.Auth_Allowed, m.Status_Code }).Distinct().ToList();
                    if (uRole1.Count > 0)
                    {
                        if (uRole1[0].Auth_Allowed == true)
                        {
                           
                        }
                        else
                        {
                            MessageBox.Show("You Have No Permissions to Approve The Purchase Order");
                            return;
                        }
                    }
                    break;               
                  

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
                    db.sp_PurchaseReq_Delete(myString, logIn.company, logIn.BU_ID);

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
             
                Purchase_Req_Master S = new Purchase_Req_Master();
                {
                    S.PR_NO = myString;
                    S.Req_Date = dpSODate.Value;
                    S.Dept_Name = Convert.ToInt32(cmbDepartment.SelectedValue.ToString());
                    S.Ref_Doc = cmbrefd.Text;                  
                    S.Asset_Code = cmbasset.Text;
                    S.Ref_Doc_Type = cmbreftype.Text;
                    S.Remarks = txtRemarks.Text;
                    S.isDeleted = false;
                    S.Indented_By = txtIndentedBy.Text;
                    S.Indentor_Mobile = txtIndentMobile.Text;
                    S.BU_ID = logIn.BU_ID;
                    //S.Returnable = checkBox1.Checked;  
                    S.Status  = Convert.ToInt32(cmbStatus.SelectedValue.ToString());
                    S.Company_ID = logIn.company;
                    S.Created_By = lblCreatedBy.Text;
                    S.Modified_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");   
                    if (cmbStatus.Text == "Approved")
                    {
                        S.Approved_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                    }
                    else
                    if (cmbStatus.Text == "Reviewed")
                    {
                        S.Reviewed_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                    }
                    S.PR_Type = txtPRType.Text;
                    db.Purchase_Req_Masters.InsertOnSubmit(S);
                    db.SubmitChanges();
                }
                    //db.Transaction = transaction;
                for (int i = 0; i < dgProducts.RowCount - 1; i++)
                {
                    Purchase_Req_Child SC = new Purchase_Req_Child();
                    var d1 = (from a in db.Purchase_Req_Masters where a.PR_NO == myString && a.Company_ID == logIn.company && a.BU_ID == logIn.BU_ID && a.isDeleted ==false select new { a.Id }).ToList();
                    SC.PR_Master_ID = d1[0].Id;
                    SC.PR_NO = myString;
                    SC.Prod_Code = (dgProducts.Rows[i].Cells["Item_Code"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value);
                   // SC.Product_Description = (dgProducts.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value).ToString();
                    //SC.Prod_Spec = (dgProducts.Rows[i].Cells["Item_spec"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_spec"].Value).ToString();
                    //SC.Prod_Grade = (dgProducts.Rows[i].Cells["Item_Grade"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value).ToString();
                    SC.Uom = (dgProducts.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["uom"].Value.ToString();
                    SC.Indent_Qty = (dgProducts.Rows[i].Cells["Indent_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Indent_Qty"].Value);
                    SC.Stock_Qty = (dgProducts.Rows[i].Cells["Stock_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Stock_Qty"].Value);
                    SC.PR_Qty = (dgProducts.Rows[i].Cells["PR_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["PR_Qty"].Value);
                    SC.Required_On = (dgProducts.Rows[i].Cells["Required_On"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Required_On"].Value).ToString();
                    SC.Last_Purchase_On = (dgProducts.Rows[i].Cells["Last_Purchase_On"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Last_Purchase_On"].Value).ToString();
                    
                    SC.Consumption_3Months = (dgProducts.Rows[i].Cells["Consumption_3Months"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Consumption_3Months"].Value);
                    
                    // SC.Last_Purchase_On = (dgProducts.Rows[i].Cells["Required_On"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Required_On"].Value).ToString();
                    //SC.Priority = (dgProducts.Rows[i].Cells["Priority"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Priority"].Value).ToString();

                    SC.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value).ToString();
                    SC.ProdSno = i+1;
                    SC.Company_ID = logIn.company;
                    db.Purchase_Req_Childs.InsertOnSubmit(SC);
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


        public void Save()
        {
            try
            {
                String myString = "";
                myString = txtSlipNo.Text;
                if ((from u in db.Purchase_Req_Masters where u.PR_NO == myString && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    myString = txtSlipNo.Text;
                    var p1 = db.Purchase_Req_Masters.Where(w => w.PR_NO == myString && w.Company_ID == logIn.company).FirstOrDefault();
                    p1.PR_NO = myString;
                    p1.Req_Date = dpSODate.Value;
                    p1.Dept_Name = Convert.ToInt32(cmbDepartment.SelectedValue.ToString());
                    p1.Ref_Doc = cmbrefd.Text;
                    p1.Asset_Code = cmbasset.Text;
                    p1.Remarks = txtRemarks.Text;
                    p1.Indented_By = txtIndentedBy.Text;
                    p1.Indentor_Mobile = txtIndentMobile.Text;
                    p1.isDeleted = false;
                    p1.BU_ID = logIn.BU_ID;
                    //S.Returnable = checkBox1.Checked;                    
                    p1.Company_ID = logIn.company;
                    p1.Created_By = lblCreatedBy.Text;
                    p1.Modified_By = logIn.username + "-" + DateTime.Now;
                    //db.Material_Issue_Masters.InsertOnSubmit(S);
                    db.SubmitChanges();

                    for (int i = 0; i < dgProducts.RowCount - 1; i++)
                    {
                        var SC = db.Purchase_Req_Childs.Where(w => w.PR_NO == txtSlipNo.Text && w.Prod_Code == Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value.ToString())).FirstOrDefault();
                        {
                            var d1 = (from a in db.Purchase_Req_Masters where a.PR_NO == myString && a.Company_ID == logIn.company select new { a.Id }).ToList();
                            SC.PR_Master_ID = d1[0].Id;
                            SC.PR_NO = myString;
                            SC.Prod_Code = (dgProducts.Rows[i].Cells["Item_Code"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value);
                            SC.Product_Description = (dgProducts.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value).ToString();
                            SC.Prod_Spec = (dgProducts.Rows[i].Cells["Item_spec"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_spec"].Value).ToString();
                            SC.Prod_Grade = (dgProducts.Rows[i].Cells["Item_Grade"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value).ToString();
                            SC.Uom = (dgProducts.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["uom"].Value.ToString();
                            SC.Indent_Qty = (dgProducts.Rows[i].Cells["Indent_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Indent_Qty"].Value);
                            SC.Stock_Qty = (dgProducts.Rows[i].Cells["Stock_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Stock_Qty"].Value);
                            SC.PR_Qty = (dgProducts.Rows[i].Cells["PR_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["PR_Qty"].Value);
                            SC.Required_On = (dgProducts.Rows[i].Cells["Required_On"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Required_On"].Value).ToString();
                            SC.Last_Purchase_On = (dgProducts.Rows[i].Cells["Last_Purchase_On"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Last_Purchase_On"].Value).ToString();

                           
                            SC.Consumption_3Months = (dgProducts.Rows[i].Cells["Consumption_3Months"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Consumption_3Months"].Value);
                           
                            // SC.Last_Purchase_On = (dgProducts.Rows[i].Cells["Required_On"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Required_On"].Value).ToString();
                            //SC.Priority = (dgProducts.Rows[i].Cells["Priority"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Priority"].Value).ToString();

                            SC.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value).ToString();
                            SC.ProdSno = i + 1;
                            SC.Company_ID = logIn.company;
                            //db.Purchase_Req_Childs.InsertOnSubmit(SC);
                        }
                        db.SubmitChanges();
                    }

                }
                else
                {
                    AutoincrementId();
                    myString = txtSlipNo.Text;
                    Purchase_Req_Master S = new Purchase_Req_Master();
                    {
                        S.PR_NO = myString;
                        S.Req_Date = dpSODate.Value;
                        S.Dept_Name = Convert.ToInt32(cmbDepartment.SelectedValue.ToString());
                        S.Ref_Doc = cmbrefd.Text;
                        S.Asset_Code = cmbasset.Text;
                        S.Remarks = txtRemarks.Text;
                        S.isDeleted = false;
                        S.Indented_By = txtIndentedBy.Text;
                        S.Indentor_Mobile = txtIndentMobile.Text;
                        S.BU_ID = logIn.BU_ID;
                        //S.Returnable = checkBox1.Checked;                    
                        S.Company_ID = logIn.company;
                        S.Created_By = lblCreatedBy.Text;
                        S.Modified_By = logIn.username + "-" + DateTime.Now;
                        db.Purchase_Req_Masters.InsertOnSubmit(S);
                        db.SubmitChanges();
                    }
                    //db.Transaction = transaction;
                    for (int i = 0; i < dgProducts.RowCount - 1; i++)
                    {
                        Purchase_Req_Child SC = new Purchase_Req_Child();
                        var d1 = (from a in db.Purchase_Req_Masters where a.PR_NO == myString && a.Company_ID == logIn.company select new { a.Id }).ToList();
                        SC.PR_Master_ID = d1[0].Id;
                        SC.PR_NO = myString;
                        SC.Prod_Code = (dgProducts.Rows[i].Cells["Item_Code"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value);
                       // SC.Product_Description = (dgProducts.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value).ToString();
                        SC.Prod_Spec = (dgProducts.Rows[i].Cells["Item_spec"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_spec"].Value).ToString();
                        SC.Prod_Grade = (dgProducts.Rows[i].Cells["Item_Grade"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value).ToString();
                        SC.Uom = (dgProducts.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["uom"].Value.ToString();
                        SC.Indent_Qty = (dgProducts.Rows[i].Cells["Indent_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Indent_Qty"].Value);
                        SC.Stock_Qty = (dgProducts.Rows[i].Cells["Stock_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Stock_Qty"].Value);
                        SC.PR_Qty = (dgProducts.Rows[i].Cells["PR_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["PR_Qty"].Value);
                        SC.Required_On = (dgProducts.Rows[i].Cells["Required_On"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Required_On"].Value).ToString();

                        SC.Last_Purchase_On = (dgProducts.Rows[i].Cells["Last_Purchase_On"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Last_Purchase_On"].Value).ToString();


                        SC.Consumption_3Months = (dgProducts.Rows[i].Cells["Consumption_3Months"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Consumption_3Months"].Value);
                        
                        // SC.Last_Purchase_On = (dgProducts.Rows[i].Cells["Required_On"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Required_On"].Value).ToString();
                        //SC.Priority = (dgProducts.Rows[i].Cells["Priority"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Priority"].Value).ToString();

                        SC.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value).ToString();
                        SC.ProdSno = i + 1;
                        SC.Company_ID = logIn.company;
                        db.Purchase_Req_Childs.InsertOnSubmit(SC);
                    }
                    db.SubmitChanges();
                    MessageBox.Show("Record Saved / Updated Successfully With Transaction Ref No : " + txtSlipNo.Text);
                    clear();
                }
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
            if (e.KeyCode == Keys.F6 )
            {
                if (dgProducts.Rows.Count > 0)
                {
                    
                    dgProducts.Rows.RemoveAt(dgProducts.CurrentRow.Index);

                    int j = 0;
                    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                    {
                        j = j + 1;
                        dgProducts.Rows[i].Cells["ProdSno"].Value = j.ToString();
                    }
                }

            }
            DataTable dtexisting = new DataTable();
            if (e.KeyCode == Keys.F2)
            {
                ioneNet.Masters.ProdSearch form = new ioneNet.Masters.ProdSearch();
                ioneNet.Masters.ProdSearch.frmName = "PReq";
                form.ShowDialog();
                if (dgProducts.Rows.Count > 1)
                {
                    dtexisting.Rows.Clear();
                    dtexisting.Columns.Clear();
                    dtexisting.Columns.Add("ProdSno", typeof(string));
                    dtexisting.Columns.Add("Item_Code", typeof(string));
                    dtexisting.Columns.Add("Item_Description", typeof(string));
                    dtexisting.Columns.Add("Item_Spec", typeof(string));
                    dtexisting.Columns.Add("Item_Grade", typeof(string));
                    dtexisting.Columns.Add("UOM", typeof(string));
                    dtexisting.Columns.Add("Indent_Qty", typeof(string));
                    dtexisting.Columns.Add("Stock_Qty", typeof(string));
                    dtexisting.Columns.Add("PR_Qty", typeof(string));
                    dtexisting.Columns.Add("Required_On", typeof(string));
                    dtexisting.Columns.Add("Last_Purchase_On", typeof(string));
                    dtexisting.Columns.Add("Consumption_3Months", typeof(string));
                    dtexisting.Columns.Add("Remarks", typeof(string));

                    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                    {
                        System.Data.DataRow dr;
                        //DataRow dr;
                        dr = dtexisting.NewRow();
                        dr["ProdSno"] = (dgProducts.Rows[i].Cells["ProdSno"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["ProdSno"].Value);
                        dr["Item_Code"] = (dgProducts.Rows[i].Cells["Item_Code"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Item_Code"].Value);
                        dr["Item_Description"] = (dgProducts.Rows[i].Cells["Item_Description"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value);
                        dr["Item_Spec"] = (dgProducts.Rows[i].Cells["Item_Spec"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Item_Spec"].Value);
                        dr["Item_Grade"] = (dgProducts.Rows[i].Cells["Item_Grade"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value);
                        dr["UOM"] = (dgProducts.Rows[i].Cells["UOM"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["UOM"].Value);
                        dr["Indent_Qty"] = (dgProducts.Rows[i].Cells["Indent_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Indent_Qty"].Value);
                        dr["Stock_Qty"] = (dgProducts.Rows[i].Cells["Stock_Qty"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Stock_Qty"].Value);
                        dr["PR_Qty"] = (dgProducts.Rows[i].Cells["PR_Qty"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["PR_Qty"].Value);
                        dr["Required_On"] = (dgProducts.Rows[i].Cells["Required_On"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Required_On"].Value);
                        dr["Last_Purchase_On"] = (dgProducts.Rows[i].Cells["Last_Purchase_On"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Last_Purchase_On"].Value);
                        dr["Consumption_3Months"] = (dgProducts.Rows[i].Cells["Consumption_3Months"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Consumption_3Months"].Value);
                        dr["Remarks"] = (dgProducts.Rows[i].Cells["Remarks"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value);
                        dtexisting.Rows.Add(dr);

                    }
                    dtexisting.AcceptChanges();
                }






                if (ioneNet.Masters.ProdSearch.dtgetproducts.Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ProdSno", typeof(string));
                    dt.Columns.Add("Item_Code", typeof(string));
                    dt.Columns.Add("Item_Description", typeof(string));
                    dt.Columns.Add("Item_Spec", typeof(string));
                    dt.Columns.Add("Item_Grade", typeof(string));
                    dt.Columns.Add("UOM", typeof(string));
                    dt.Columns.Add("Indent_Qty", typeof(decimal));
                    dt.Columns.Add("Stock_Qty", typeof(decimal));
                    dt.Columns.Add("PR_Qty", typeof(string));
                    dt.Columns.Add("Required_On", typeof(string));
                    dt.Columns.Add("Last_Purchase_On", typeof(string));
                    dt.Columns.Add("Consumption_3Months", typeof(string));
                    dt.Columns.Add("Remarks", typeof(string));

                    //dt.Rows.Add();
                    int j = dgProducts.Rows.Count - 1;
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
                                               ProdSno=j+1,
                                               Item_Code = obj.prod_ID,
                                               Item_Description = obj.Prod_Name,
                                               Item_Spec = prod_code,
                                               obj.Prod_Alternative_Code,
                                               Item_Grade = obj.Prod_Field2,
                                               UOM = uom.Uom_Descr,
                                               Indent_Qty = 0,
                                               Stock_Qty = "0.00",
                                               PR_Qty = "0.00",
                                               Required_On = "",
                                               Last_Purchase_On = "",
                                               Consumption_3Months = 0,
                                               Remarks = ""
                                           }).ToList();

                        dt.Rows.Add(getproducts[0].ProdSno, getproducts[0].Item_Code, getproducts[0].Item_Description, getproducts[0].Item_Spec, getproducts[0].Item_Grade, getproducts[0].UOM, getproducts[0].Indent_Qty, getproducts[0].Stock_Qty, getproducts[0].PR_Qty, getproducts[0].Required_On, getproducts[0].Last_Purchase_On, getproducts[0].Consumption_3Months, getproducts[0].Remarks);

                    }

                    dtexisting = dtexisting.AsEnumerable().Union(dt.AsEnumerable()).CopyToDataTable();
                    dgProducts.DataSource = dtexisting;



                    int l = 0;
                    for (int m = 0; m <= dgProducts.Rows.Count - 2; m++)
                    {
                        if (dgProducts.Rows[m].Cells["Item_Code"].Value != "")
                        {
                            l = l + 1;
                            dgProducts.Rows[m].Cells["ProdSno"].Value = l.ToString();
                        }
                    }
                }

                for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                {
                    DateTime t = dpSODate.Value;
                    string dt1 = t.ToString("yyyy/MM/dd");
                    var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value.ToString()), Convert.ToDateTime(dt1), logIn.BU_ID) select data).ToList();
                    dgProducts.Rows[i].Cells["Stock_Qty"].Value = "0";
                    if (stock.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        dgProducts.Rows[i].Cells["Stock_Qty"].Value = stock[0].ClosingQty;
                        // R1.Cells["Price"].Value = stock[0].CBPrice;
                    }

                    var LP = (from data in db.ProductWise_Data_In_PO(logIn.company, Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value.ToString())) select data).ToList();
                    if (LP.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        dgProducts.Rows[i].Cells["Last_Purchase_On"].Value = LP[0].last_Purchase_Date;
                        // R1.Cells["Price"].Value = stock[0].CBPrice;
                    }

                    var l3C = (from data in db.ProductWise_Last3MonthCon(logIn.company, Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value.ToString()), logIn.BU_ID, t) select data).ToList();
                    dgProducts.Rows[i].Cells["Consumption_3Months"].Value = "0";
                    if (l3C.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        dgProducts.Rows[i].Cells["Consumption_3Months"].Value = l3C[0].Con_Qty;
                        // R1.Cells["Price"].Value = stock[0].CBPrice;
                    }
                }



            }

        }

       
        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
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
                bindDept();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void txtRefDoc_Enter(object sender, EventArgs e)
        {
            
        }
        

        private void cmbtc_Leave(object sender, EventArgs e)
        {
            try
            {


                if (cmbreftype.Text == "TC Notice")
                {
                    var Prodname = (from d in db.SP_GetTC_Sel(logIn.company, logIn.BU_ID)


                                    select new { d.TC_No }).ToList();

                    if (Prodname.Count > 0)
                    {
                        cmbrefd.DataSource = Prodname;
                        cmbrefd.ValueMember = "TC_No";
                        cmbrefd.DisplayMember = "TC_No";
                    }

                    cmbrefd.SelectedIndex = -1;

                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtRefDoc_Leave(object sender, EventArgs e)
        {

        }

        

        private void cmbasset_Leave(object sender, EventArgs e)
        {
            try
            {
                if (cmbasset.Text != "")
                {
                    int i = cmbasset.FindString(cmbasset.Text);
                    if (i < 0)
                    {
                        MessageBox.Show("Invalid Project Code");
                        cmbasset.Focus();
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbrefd_Leave(object sender, EventArgs e)
        {
            try
            {
                
                if (cmbreftype.Text =="TC Notice" && cmbrefd.SelectedValue != null)
                {
                    //var tc = (from m in db.technical_notice_masters
                    //          join p in db.Project_code_Masters on m.Project_Name equals p.id
                    //          where m.Company_ID == logIn.company && m.Vch_no == cmbrefd.Text

                    //          select new { m.Project_Name, p.Project_Code,p.id,m.Process,m.Painting, m.Sliver_Plating,m.Other }).ToList();

                    //if(tc.Count>0)
                    //{
                    //    cmbasset.SelectedValue = tc[0].id;
                    //    txtRemarks.Text = "Painting :" + tc[0].Painting + "; Sliver Painting : " +  tc[0].Sliver_Plating + "; Others: " + tc[0].Other;
                        
                    //}


                    DataTable dt = new DataTable();
                    //String conStr = "Data Source=172.16.6.173;Initial Catalog=servion_hari;User  ID=sa;Password=Servion@123";
                    //SqlConnection con = new SqlConnection(con);
                    DateTime t = dpSODate.Value;
                    string t1 = t.ToString("dd/MMM/yyyy");
                    SqlCommand com = new SqlCommand("SP_GetTC_Products_PR", con);
                    com.Parameters.AddWithValue("@compname", logIn.company);
                    com.Parameters.AddWithValue("@buid", logIn.BU_ID);
                    com.Parameters.AddWithValue("@TCNO", cmbrefd.Text);
                   
                    com.CommandType = CommandType.StoredProcedure;
                    SqlDataAdapter da = new SqlDataAdapter(com);

                    con.Open();
                    da.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        dgProducts.DataSource = dt;

                    }
                    else
                    {
                        MessageBox.Show("Either Invalid TC No Entered Or No Pending Items Available to Generate PR");
                    }
                    con.Close();
                    

                    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                    {
                        DateTime t2 = dpSODate.Value;
                        string dt1 = t2.ToString("yyyy/MM/dd");
                        var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value.ToString()), Convert.ToDateTime(dt1), logIn.BU_ID) select data).ToList();
                        dgProducts.Rows[i].Cells["Stock_Qty"].Value = "0";
                        if (stock.Count > 0)
                        {
                            //dgProductsList.DataSource = d;
                            dgProducts.Rows[i].Cells["Stock_Qty"].Value = stock[0].ClosingQty;
                            // R1.Cells["Price"].Value = stock[0].CBPrice;
                        }

                        var LP = (from data in db.ProductWise_Data_In_PO(logIn.company, Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value.ToString())) select data).ToList();
                        if (LP.Count > 0)
                        {
                            //dgProductsList.DataSource = d;
                            dgProducts.Rows[i].Cells["Last_Purchase_On"].Value = LP[0].last_Purchase_Date;
                            // R1.Cells["Price"].Value = stock[0].CBPrice;
                        }

                        var l3C = (from data in db.ProductWise_Last3MonthCon(logIn.company, Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value.ToString()), logIn.BU_ID, t) select data).ToList();
                        dgProducts.Rows[i].Cells["Consumption_3Months"].Value = "0";
                        if (l3C.Count > 0)
                        {
                            //dgProductsList.DataSource = d;
                            dgProducts.Rows[i].Cells["Consumption_3Months"].Value = l3C[0].Con_Qty;
                            // R1.Cells["Price"].Value = stock[0].CBPrice;
                        }
                    }


                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgProducts_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cmbrefd_Enter(object sender, EventArgs e)
        {
            PR_Ref_Basis = cmbrefd.Text;
        }

        private void cmbasset_Enter(object sender, EventArgs e)
        {
            if (Convert.ToInt32(cmbasset.SelectedIndex)<0)
            {
                bindproj();
               
            }
            
        }

        private void cmbDepartment_Leave(object sender, EventArgs e)
        {
            if (cmbDepartment.Text != "")
            {
                int i = cmbDepartment.FindString(cmbDepartment.Text);
                if (i < 0)
                {
                    MessageBox.Show("Invalid Department Name");
                    cmbDepartment.Focus();
                }
            }
           
        }

        private void cmbreftype_Enter(object sender, EventArgs e)
        {
            PR_Basis = cmbreftype.Text;
        }

        

       

        private void dgProducts_Leave(object sender, EventArgs e)
        {

        }

        private void btnTransLog_Click(object sender, EventArgs e)
        {
            transname = this.Text;
            transno = txtSlipNo.Text;
            frmTransLog form = new frmTransLog();            
            form.ShowDialog();
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
                cmbDepartment.Text = "";
                cmbrefd.Text = "";
              //  txtIssuedBy.Text = logIn.username;
               // txtIssuedTo.Text = "";
                txtRemarks.Text = "";
                lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                cmbasset.Text = "";
                cmbreftype.Text = "";

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
                var da = (from obj in db.Purchase_Req_Masters
                          
                          where obj.PR_NO == txtSlipNo.Text && obj.Company_ID == logIn.company && obj.BU_ID == logIn.BU_ID
                          select obj).ToList();
                var pm= (from obj in db.Project_code_Masters

                        where  obj.Company_ID == logIn.company 
                        select obj).ToList();
                if (da.Count > 0)
                {
                    txtSlipNo.Text = da[0].PR_NO.ToString();
                    dpSODate.Text = da[0].Req_Date.ToString();
                    //bindCustomer();
                    cmbDepartment.SelectedValue = da[0].Dept_Name;
                    cmbStatus.SelectedValue = da[0].Status;

                    cmbasset.Text = da[0].Asset_Code;
                    cmbreftype.Text = da[0].Ref_Doc_Type;
                    txtIndentedBy.Text = da[0].Indented_By;
                    txtIndentMobile.Text = da[0].Indentor_Mobile;
                    cmbrefd.Text = da[0].Ref_Doc;
                    txtRemarks.Text = da[0].Remarks;
                    lblCreatedBy.Text = da[0].Created_By;
                    lblModified.Text = da[0].Modified_By;
                    txtPRType.Text = da[0].PR_Type;
                }


                var dm1 = (from s in db.Purchase_Req_Childs
                           join p in db.Purchase_Req_Masters on s.PR_Master_ID equals p.Id
                           join pr in db.Products on s.Prod_Code equals pr.prod_ID
                           join u in db.UoM_Masters on pr.Prod_Primary_UOM_Id equals u.UOM_ID
                           where s.PR_NO == txtSlipNo.Text && s.Company_ID == logIn.company && p.BU_ID == logIn.BU_ID


                           select new

                           {
                               ProdSno = s.ProdSno,
                               Item_Code = s.Prod_Code,
                               Item_Description = pr.Prod_Name,
                               Item_spec = pr.Prod_Code,
                               Item_Grade = pr.Prod_Description + " " + pr.Prod_Field2,
                               UOM = u.Uom_Descr,
                               s.Indent_Qty,
                               s.Stock_Qty,
                               s.PR_Qty,
                               s.Required_On,
                               s.Last_Purchase_On,
                               s.Consumption_3Months,
                               s.Remarks
                           });
                SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataTable dtr = new DataTable();
                da2.Fill(dtr);
                if (dtr.Rows.Count >= 0)
                {
                    dgProducts.DataSource = dtr;
                    
                }
                //int j = 0;
                //for(int i=0;i<=dgProducts.Rows.Count-1;i++)
                //{
                //    j = j + 1;
                //    dgProducts.Rows[i].Cells["ProdSno"].Value=j.ToString();
                //}


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
