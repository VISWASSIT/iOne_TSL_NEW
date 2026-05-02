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
    public partial class frmMaterialIssues : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string DocNo, ItemCode, RecQty, Suppname;
        public static DataTable dtgetfinalprducts = new DataTable();
        public frmMaterialIssues()
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
            AutoincrementId();
            

            }

            public void AutoincrementId()
        {
            try
            {

                var result = db.Sp_autoincrement_MaterialIssues(logIn.company, logIn.BU_ID, logIn.fy_Start_Date, logIn.fy_End_Date);
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
                string depttext = cmbDepartment.Text;
                if (cmbIssuedTo.Text != "")
                {
                    switch (cmbIssuedTo.Text)
                    {

                        case "Internal Department":
                            var Dept = (from m in db.Department_Masters where m.Company_ID == logIn.company select new { m.Id, m.Dept_Name }).Distinct().ToList();
                            if (Dept.Count > 0)
                            {
                                cmbDepartment.DataSource = Dept;
                                cmbDepartment.ValueMember = "Id";
                                cmbDepartment.DisplayMember = "Dept_Name";
                            }
                            break;

                        case "Sub Contractor":
                            var pStatus = (from m in db.Supplier_informations where m.Company_ID == logIn.company && m.Supplier_Category == 32 select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                            if (pStatus.Count > 0)
                            {
                                cmbDepartment.DataSource = pStatus;
                                cmbDepartment.ValueMember = "ID";
                                cmbDepartment.DisplayMember = "Supplier_Name";
                            }
                            break;
                    }
                }
                cmbDepartment.Text = depttext;
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
                if (cmbRefDocType.Text != "")
                {

                    if (cmbRefDocType.Text == "Cutting Plan")
                    {
                        cmbIssuedTo.Text = "Sub Contractor";
                        bindDept();
                        var gstno = (from c in db.CuttingPlan_Releases
                                     where c.Ref_No == txtIndentNo.Text && c.Company_ID == logIn.company
                                     select new { c.Contractor_ID }).ToList();
                        if (gstno.Count > 0)
                        {

                            cmbDepartment.SelectedValue = gstno[0].Contractor_ID;

                        }

                        DataTable dt = new DataTable();
                        //String conStr = "Data Source=172.16.6.173;Initial Catalog=servion_hari;User  ID=sa;Password=Servion@123";
                        //SqlConnection con = new SqlConnection(con);
                        //DateTime t = dpdate.Value;
                        //string t1 = t.ToString("dd/MMM/yyyy");
                        SqlCommand com = new SqlCommand("CP_To_Issue", con);
                        com.Parameters.AddWithValue("@compname", logIn.company);
                        com.Parameters.AddWithValue("@RefNo", txtIndentNo.Text);
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
                        int j = 0;
                        for (int i = 0; i <= dgProducts.Rows.Count - 1; i++)
                        {
                            j = j + 1;
                            dgProducts.Rows[i].Cells["S_No"].Value = j.ToString();
                        }
                        for (int i = 0; i < dgProducts.Rows.Count; i++)
                        {

                            //Get Stock Report
                            DateTime t = dpSODate.Value;
                            string dt1 = t.ToString("yyyy/MM/dd");
                            var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(Item_Code.ToString()), Convert.ToDateTime(dt1), logIn.BU_ID) select data).ToList();

                            //string f1 = t.ToString("dd/MMM/yyyy");
                            //var stock = (from data in db.ShowStockLedger_New(logIn.company, Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value), t, t,logIn.BU_ID) select data).ToList();

                            if (stock.Count > 0)
                            {
                                //dgProductsList.DataSource = d;
                                dgProducts.Rows[i].Cells["Stock_Qty"].Value = stock[0].ClosingQty;
                                //decimal cValue = Convert.ToDecimal(stock[0].ClosingValue);
                                //decimal cStock = Convert.ToDecimal(stock[0].ClosingQty);
                                //R1.Cells["Price"].Value = (cValue / cStock);
                                dgProducts.Rows[i].Cells["Price"].Value = stock[0].CBPrice;
                            }

                        }
                    }

                    if (cmbRefDocType.Text == "MRP")
                    {
                        DataTable dt = new DataTable();
                        //String conStr = "Data Source=172.16.6.173;Initial Catalog=servion_hari;User  ID=sa;Password=Servion@123";
                        //SqlConnection con = new SqlConnection(con);
                        DateTime t = dpSODate.Value;
                        string t1 = t.ToString("dd/MMM/yyyy");
                        SqlCommand com = new SqlCommand("ProdutsToIssue", con);
                        com.Parameters.AddWithValue("@compname", logIn.company);
                        com.Parameters.AddWithValue("@docno", txtIndentNo.Text);
                        com.Parameters.AddWithValue("@mrpdate", t1);
                        com.Parameters.AddWithValue("@buid", logIn.BU_ID);
                        com.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(com);

                        con.Open();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            dgProducts.DataSource = dt;
                            int j = 0;
                            for (int i = 0; i <= dgProducts.Rows.Count - 1; i++)
                            {
                                j = j + 1;
                                dgProducts.Rows[i].Cells["S_No"].Value = j.ToString();
                            }
                        }

                        else
                        {
                            MessageBox.Show("Either Invalid MRP No Entered and No Pending Items Available to Issue");
                        }
                        con.Close();
                    }

                    if (cmbRefDocType.Text == "Indent")
                    {
                        DataTable dt = new DataTable();
                        //String conStr = "Data Source=172.16.6.173;Initial Catalog=servion_hari;User  ID=sa;Password=Servion@123";
                        //SqlConnection con = new SqlConnection(con);
                        DateTime t = dpSODate.Value;
                        string t1 = t.ToString("dd/MMM/yyyy");
                        SqlCommand com = new SqlCommand("Produts_Indents_ToIssue", con);
                        com.Parameters.AddWithValue("@compname", logIn.company);
                        com.Parameters.AddWithValue("@docno", txtIndentNo.Text);
                        com.Parameters.AddWithValue("@mrpdate", t1);
                        com.Parameters.AddWithValue("@buid", logIn.BU_ID);
                        com.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter da = new SqlDataAdapter(com);

                        con.Open();
                        da.Fill(dt);
                        if (dt.Rows.Count > 0)
                        {
                            dgProducts.DataSource = dt;
                            int j = 0;
                            for (int i = 0; i <= dgProducts.Rows.Count - 1; i++)
                            {
                                j = j + 1;
                                dgProducts.Rows[i].Cells["S_No"].Value = j.ToString();
                            }
                        }

                        else
                        {
                            MessageBox.Show("Either Invalid Indent No Entered Or No Pending Items Available to Issue");
                        }
                        con.Close();
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
                if (columnName == "Item_Description" || columnName== "Int_Prod_Code")
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
                            if (R1.Cells["S_no"].Value == null || R1.Cells["S_no"].Value.ToString() == "")
                            {
                                R1.Cells["S_no"].Value = dgProducts.Rows.Count - 1;
                            }

                            R1.Cells["Indent_Qty"].Value = 0;
                            }

                        }
                        else
                        {
                            MessageBox.Show("Record Not Found");
                            return;
                        }


                    //    var getProductName = (from s in db.Products
                    //                      join u in db.UoM_Masters on s.Prod_Primary_UOM_Id equals u.UOM_ID
                    //                      join g in db.Product_Groups on s.Prod_Group_Id equals g.ID
                    //                      where s.Prod_Name == R1.Cells["Item_Description"].Value.ToString() && s.Company_ID == logIn.company
                    //                      select new { s.prod_ID, s.Prod_Code, u.Uom_Descr, g.Prod_Group_Name, s.Prod_HSN_Code,s.Prod_Field2, s.Prod_Alternative_Code }).FirstOrDefault();

                    //if (getProductName != null)
                    //{
                    //    R1.Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                    //    R1.Cells["Item_code"].Value = getProductName.prod_ID.ToString();
                    //    R1.Cells["Int_Prod_Code"].Value = (logIn.company == 1044 ? getProductName.Prod_Alternative_Code.ToString() : getProductName.Prod_Code.ToString());
                    //    if (getProductName.Prod_Field2 != null)
                    //    { 
                    //    R1.Cells["Item_Grade"].Value = getProductName.Prod_Field2.ToString();
                    //    }
                    //    if (R1.Cells["S_no"].Value == null || R1.Cells["S_no"].Value.ToString() == "")
                    //    {
                    //        R1.Cells["S_no"].Value = dgProducts.Rows.Count - 1;
                    //    }
                        
                    //    R1.Cells["Indent_Qty"].Value = 0;

                    //}

                    //else
                    //{

                    //    MessageBox.Show("Invalid Product Name");
                    //    R1.Cells["Item_Description"].Value = "";
                    //    return;
                       
                    //}
                    DateTime t = dpSODate.Value;                    
                    string dt1 = t.ToString("yyyy/MM/dd");
                    decimal stqty = 0;
                    decimal CBprice = 0;
                    if (logIn.company == 1043)
                    {
                        var stock = (from data in db.ShowItemWiseStockReport_NHVS(logIn.company, Convert.ToInt32(R1.Cells["Item_Code"].Value.ToString()), Convert.ToDateTime(dt1), logIn.BU_ID, cmbFromBin.Text) select data).ToList();
                        R1.Cells["Stock_Qty"].Value = "0";
                        if (stock.Count > 0)
                        {
                            stqty = Convert.ToDecimal(stock[0].ClosingQty);
                            CBprice = Convert.ToDecimal(stock[0].CBPrice);
                        }
                    }
                    else
                    {
                        var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(R1.Cells["Item_Code"].Value.ToString()), Convert.ToDateTime(dt1), logIn.BU_ID) select data).ToList();
                        R1.Cells["Stock_Qty"].Value = "0";
                        if (stock.Count > 0)
                        {
                            stqty = Convert.ToDecimal(stock[0].ClosingQty);
                            CBprice = Convert.ToDecimal(stock[0].CBPrice);
                        }
                    }

                        if (stqty > 0)
                        {
                            //dgProductsList.DataSource = d;
                            R1.Cells["Stock_Qty"].Value = stqty;
                            //decimal cValue = Convert.ToDecimal(stock[0].CBPrice);
                            //decimal cStock = Convert.ToDecimal(stock[0].ClosingQty); 
                            R1.Cells["Price"].Value = CBprice;
                        }
                    
                }

                if (columnName == "Item_Spec" && R1.Cells["Item_Spec"].Value != null)
                {

                    //var getProductName = (from s in db.Bloom_Roll_Wise_Stocks
                                          
                    //                      where s.Prod_ID == Convert.ToInt32(R1.Cells["Item_code"].Value.ToString()) && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID
                    //                      select new { s.QtyStock}).FirstOrDefault();

                    //if (getProductName != null)
                    //{
                        
                    //    if (getProductName.QtyStock != null)
                    //    {
                    //        R1.Cells["Stock_Qty"].Value = getProductName.QtyStock.ToString();
                    //    }                                            

                    //}

                    //else
                    //{

                    //    MessageBox.Show("Invalid ARN No");
                    //    R1.Cells["Item_Spec"].Value = "";
                    //    return;

                    //}                    
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
                        if (cmbRefDocType.Text != "NA")
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

                if (tb3 != null && columnName == "Length/Spec")
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
                        if (columnName == "Length/Spec")
                        {

                           
                            //var Prodname = (from d in db.Bloom_Roll_Wise_Stocks where d.Company_ID == logIn.company && d.BU_ID == logIn.BU_ID && d.Prod_ID == Convert.ToInt32(R1.Cells["Item_Code"].Value)  select new { d.RollNo }).Distinct().ToList();
                            //DataTable dt = new DataTable();
                            //dt.Columns.Add("ARN_No");
                            //foreach (var item in Prodname)
                            //{
                            //    dt.Rows.Add(item.RollNo);
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
                if (cmbIssuedTo.Text == string.Empty)
                {
                    MessageBox.Show("Issued To Should Not Be Empty", "Issues", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbIssuedTo.Focus();
                    return;
                }

                else
                if (cmbDepartment.Text == string.Empty)
                {
                    MessageBox.Show("Select Dpeatment / Sub Contractor Name", "Issues", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbDepartment.Focus();
                    return;
                }
                else
                if (cmbRefDocType.Text == string.Empty)
                {
                    MessageBox.Show("Select Ref Doc Type", "Issues", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbRefDocType.Focus();
                    return;
                }
                if (dgProducts.Rows[0].Cells["Item_Code"].Value == null)
                {
                    MessageBox.Show("Atleast One Record To Be Entered To Proceed", "Material Issues", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Material Issues" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role,m.Create_Role }).Distinct().ToList();
                    if (uRole.Count > 0)
                    {
                        if (uRole[0].Modify_Role == true || uRole[0].Create_Role == true )
                        {
                            Save();
                        }
                        else
                        {
                            MessageBox.Show("You Have No Permissions to Save Data");
                            return;
                        }

                    }
                    
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
                if ((from u in db.Material_Issue_Masters where u.Slip_NO == myString && u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID select u).Count() > 0)
                {
                    myString = txtSlipNo.Text;
                    db.sp_Issues_Delete(myString, logIn.company, logIn.BU_ID);
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
                Material_Issue_Master S = new Material_Issue_Master();
                {
                    S.Slip_NO = myString;
                    S.Slip_Date = dpSODate.Value;
                    S.Indent_No = txtIndentNo.Text;
                    S.Dept_Name = Convert.ToInt32(cmbDepartment.SelectedValue.ToString());
                    S.Ref_Doc_Type = (cmbRefDocType.Text);
                    S.Issued_To = (cmbIssuedTo.Text);
                    S.Issued_By = txtIssuedBy.Text;
                    S.Issued_Person_To = txtIssuedTo.Text;
                    S.Remarks = txtRemarks.Text;
                    S.isDeleted = false;
                    S.Returnable = checkBox1.Checked;
                    S.BU_ID = logIn.BU_ID;
                    S.Company_ID = logIn.company;
                    S.Created_By = lblCreatedBy.Text;
                    S.FromBin = cmbFromBin.Text;
                    S.ToBin = cmbToBin.Text;

                    S.Modified_By = logIn.username + "-" + DateTime.Now;
                    db.Material_Issue_Masters.InsertOnSubmit(S);
                    db.SubmitChanges();
                }
                //db.Transaction = transaction;
                for (int i = 0; i < dgProducts.RowCount - 1; i++)
                {
                    Material_Issue_Child SC = new Material_Issue_Child();
                    var d1 = (from a in db.Material_Issue_Masters where a.Slip_NO == myString && a.Company_ID == logIn.company && a.BU_ID == logIn.BU_ID select new { a.Id }).ToList();
                    SC.Slip_Master_ID = d1[0].Id;
                    SC.Slip_NO = myString;
                    SC.Prod_Code = (dgProducts.Rows[i].Cells["Item_Code"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value);
                    //SC.Int_Prod_Code = (dgProducts.Rows[i].Cells["Int_Prod_Code"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Int_Prod_Code"].Value).ToString();
                    //SC.Product_Description = (dgProducts.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value).ToString();
                    //SC.Prod_Spec = (dgProducts.Rows[i].Cells["Item_spec"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_spec"].Value).ToString();
                    //SC.Prod_Grade = (dgProducts.Rows[i].Cells["Item_Grade"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value).ToString();
                    //SC.Uom = (dgProducts.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["uom"].Value.ToString();
                    SC.Indent_Qty = (dgProducts.Rows[i].Cells["Indent_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Indent_Qty"].Value);
                    SC.Stock_Qty = (dgProducts.Rows[i].Cells["Stock_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Stock_Qty"].Value);
                    SC.Issued_Qty = (dgProducts.Rows[i].Cells["Issue_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Issue_Qty"].Value);
                    SC.Issue_Price = (dgProducts.Rows[i].Cells["Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Price"].Value);
                    SC.Issue_Value = (dgProducts.Rows[i].Cells["Amount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Amount"].Value);
                    SC.Cost_Center = (dgProducts.Rows[i].Cells["Cost_Center"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Cost_Center"].Value).ToString();
                    SC.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value).ToString();
                    SC.ProdSno = Convert.ToInt32(dgProducts.Rows[i].Cells["S_No"].Value);
                    SC.Company_ID = logIn.company;
                    db.Material_Issue_Childs.InsertOnSubmit(SC);
                }
                db.SubmitChanges();
                //SqlCommand cmd = new SqlCommand();
                //cmd.CommandText = "Update Bloom_Roll_Wise_Issues set status = 'Closed' where [Doc_Ref]=@param1 and Company_ID =@compName";
                //cmd.Parameters.AddWithValue("@param1", txtSlipNo.Text);
                //cmd.Parameters.AddWithValue("@CompName", logIn.company);
                //cmd.Connection = con;
                //con.Open();
                //cmd.ExecuteNonQuery();
                //con.Close();
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
            if (e.KeyCode == Keys.F6 )
            {
                if (dgProducts.Rows.Count > 0)
                {
                    //First Delete respective entry in bloom wise entry
                    //SqlCommand cmd = new SqlCommand();
                    //cmd.CommandText = "delete Bloom_Roll_Wise_Issues where Doc_Ref =@param1 and prod_id = @param2 and  Company_ID =@compName";
                    //cmd.Parameters.AddWithValue("@param1", txtSlipNo.Text);
                    //cmd.Parameters.AddWithValue("@param2", dgProducts.Rows[dgProducts.CurrentRow.Index].Cells["Item_Code"].Value);
                    //cmd.Parameters.AddWithValue("@CompName", logIn.company);
                    //cmd.Connection = con;
                    //con.Open();
                    //cmd.ExecuteNonQuery();
                    //con.Close();



                    //dgProducts.Rows.RemoveAt(dgProducts.CurrentRow.Index);
                    //int l = 0;
                    //for (int m = 0; m <= dgProducts.Rows.Count - 2; m++)
                    //{
                    //    if (dgProducts.Rows[m].Cells["Item_Code"].Value != "")
                    //    {
                    //        l = l + 1;
                    //        dgProducts.Rows[m].Cells["S_No"].Value = l.ToString();
                    //    }
                    //}
                    


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
                    dtexisting.Columns.Add("Item_Code", typeof(string));
                    dtexisting.Columns.Add("Int_Prod_Code", typeof(string));
                    dtexisting.Columns.Add("Item_Description", typeof(string));
                    dtexisting.Columns.Add("Item_Spec", typeof(string));
                    dtexisting.Columns.Add("Item_Grade", typeof(string));
                    dtexisting.Columns.Add("UOM", typeof(string));
                    dtexisting.Columns.Add("Indent_Qty", typeof(string));
                    dtexisting.Columns.Add("Stock_Qty", typeof(string));
                    dtexisting.Columns.Add("Issue_Qty", typeof(string));
                    dtexisting.Columns.Add("Price", typeof(string));
                    dtexisting.Columns.Add("Amount", typeof(string));
                    dtexisting.Columns.Add("Cost_Center", typeof(string));
                    dtexisting.Columns.Add("Remarks", typeof(string));

                    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                    {
                        DataRow dr;
                        dr = dtexisting.NewRow();
                        dr["S_No"] = dgProducts.Rows[i].Cells["S_No"].Value.ToString();
                        dr["Item_Code"] = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                        dr["Int_Prod_Code"] = dgProducts.Rows[i].Cells["Int_Prod_Code"].Value.ToString(); 
                        dr["Item_Description"] = dgProducts.Rows[i].Cells["Item_Description"].Value.ToString();
                        dr["Item_Spec"] = (dgProducts.Rows[i].Cells["Item_Spec"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Item_Spec"].Value);
                        dr["Item_Grade"] = (dgProducts.Rows[i].Cells["Item_Grade"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value);
                        dr["UOM"] = dgProducts.Rows[i].Cells["UOM"].Value.ToString();
                        dr["Indent_Qty"] = (dgProducts.Rows[i].Cells["Indent_Qty"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Item_Spec"].Value);
                        dr["Stock_Qty"] = dgProducts.Rows[i].Cells["Stock_Qty"].Value.ToString();
                        dr["Issue_Qty"] = (dgProducts.Rows[i].Cells["Issue_Qty"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Issue_Qty"].Value);
                        dr["Price"] = (dgProducts.Rows[i].Cells["Price"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Price"].Value);
                        dr["Amount"] = (dgProducts.Rows[i].Cells["Amount"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Amount"].Value);
                        dr["Cost_Center"] = (dgProducts.Rows[i].Cells["Cost_Center"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Cost_Center"].Value);
                        dr["Remarks"] = (dgProducts.Rows[i].Cells["Remarks"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value);
                        dtexisting.Rows.Add(dr);

                    }
                    dtexisting.AcceptChanges();
                }






                if (ioneNet.Masters.ProdSearch.dtgetproducts.Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    dt.Columns.Add("S_No", typeof(string));   
                    dt.Columns.Add("Item_Code", typeof(string));
                    dt.Columns.Add("Int_Prod_Code", typeof(string));
                    dt.Columns.Add("Item_Description", typeof(string));
                    dt.Columns.Add("Item_Spec", typeof(string));
                    dt.Columns.Add("Item_Grade", typeof(string));
                    dt.Columns.Add("UOM", typeof(string));
                    dt.Columns.Add("Indent_Qty", typeof(string));
                    dt.Columns.Add("Stock_Qty", typeof(string));
                    dt.Columns.Add("Issue_Qty", typeof(string));
                    dt.Columns.Add("Price", typeof(string));
                    dt.Columns.Add("Amount", typeof(string));
                    dt.Columns.Add("Cost_Center", typeof(string));
                    dt.Columns.Add("Remarks", typeof(string));
                    int j = dgProducts.Rows.Count - 1;
                    //dt.Rows.Add();
                    for (int i = 0; i < ioneNet.Masters.ProdSearch.dtgetproducts.Rows.Count; i++)
                    {
                        DateTime t = dpSODate.Value;
                        string dt1 = t.ToString("yyyy/MM/dd");
                        string prodcode = ioneNet.Masters.ProdSearch.dtgetproducts.Rows[i]["prod_id"].ToString();
                        string prod_code = ioneNet.Masters.ProdSearch.dtgetproducts.Rows[i]["prod_code"].ToString();
                        decimal cstock = 0;
                        decimal cprice = 0;
                        var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(prodcode), Convert.ToDateTime(dt1), logIn.BU_ID) select data).ToList();
                        if(stock.Count>0)
                        {
                            cstock = Convert.ToDecimal(stock[0].ClosingQty);
                            cprice = Convert.ToDecimal(stock[0].CBPrice);
                        }

                        var getproducts = (from obj in db.Products
                                           join uom in db.UoM_Masters on obj.Prod_Primary_UOM_Id equals uom.UOM_ID
                                           where obj.prod_ID == Convert.ToInt32(prodcode)
                                           select new
                                           {

                                               // {
                                               ProdSno = obj.prod_ID,
                                               Item_Code = prod_code,
                                               Item_Description = obj.Prod_Name,
                                               Item_Spec = "",
                                               Item_Grade = "",
                                               UOM = uom.Uom_Descr,
                                               Indent_Qty = 0,
                                               Stock_Qty = 0,
                                               Issue_Qty = "",
                                               price = 0,
                                               Amount = "",
                                               Cost_Center="",
                                               Remarks = "" 
                                           }).ToList();
                        j = j + 1;
                        dt.Rows.Add(j, getproducts[0].ProdSno, getproducts[0].Item_Code, getproducts[0].Item_Description, getproducts[0].Item_Spec, getproducts[0].Item_Grade, getproducts[0].UOM, getproducts[0].Indent_Qty, cstock, getproducts[0].Issue_Qty, cprice, getproducts[0].Amount, getproducts[0].Cost_Center,getproducts[0].Remarks);

                    }

                    dtexisting = dtexisting.AsEnumerable().Union(dt.AsEnumerable()).CopyToDataTable();
                    dgProducts.DataSource = dtexisting;
                   
                        int l = 0;
                        for (int m = 0; m <= dgProducts.Rows.Count - 2; m++)
                        {
                            if (dgProducts.Rows[m].Cells["Item_Code"].Value != "")
                            {
                                l = l + 1;
                                dgProducts.Rows[m].Cells["S_No"].Value = l.ToString();
                            }
                        }

                    
                }
            }
            if (e.KeyCode == Keys.F3)
            {

                if (logIn.company == 1044)
                {
                    
                }
                else
                {
                    
                }
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
                    if ((from a in db.Material_Issue_Masters where a.Company_ID == logIn.company && a.Slip_NO == txtSlipNo.Text && a.BU_ID == logIn.BU_ID select a).Count() > 0)
                    {
                        db.sp_Issues_Delete(txtSlipNo.Text, logIn.company, logIn.BU_ID);
                        //SqlCommand cmd = new SqlCommand();
                        //cmd.CommandText = "delete Bloom_Roll_Wise_Issues where Doc_Ref =@param1 and Company_ID =@compName";                       
                        //cmd.Parameters.AddWithValue("@param1", txtSlipNo.Text);
                        //cmd.Parameters.AddWithValue("@CompName", logIn.company);
                        //cmd.Connection = con;
                        //con.Open();
                        //cmd.ExecuteNonQuery();
                        //con.Close();


                        MessageBox.Show("Recored Deleted Successfully");
                        clear();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtIndentNo_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            DataTable dtexisting = new DataTable();
            ioneNet.MaterialManagement.Transactions.GetProductsByMO form = new ioneNet.MaterialManagement.Transactions.GetProductsByMO();
            DocNo = txtIndentNo.Text;
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
                dtexisting.Columns.Add("Indent_Qty", typeof(decimal));
                dtexisting.Columns.Add("Stock_Qty", typeof(decimal));                
                dtexisting.Columns.Add("Issue_Qty", typeof(decimal));               
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
                    dr["Indent_Qty"] = dgProducts.Rows[i].Cells["Indent_Qty"].Value.ToString();
                    dr["Stock_Qty"] = dgProducts.Rows[i].Cells["Stock_Qty"].Value.ToString();
                    dr["Issue_Qty"] = dgProducts.Rows[i].Cells["Issue_Qty"].Value.ToString();
                    dr["Price"] = dgProducts.Rows[i].Cells["Price"].Value.ToString();
                    dr["Amount"] = dgProducts.Rows[i].Cells["Amount"].Value.ToString();
                    dr["Cost_Center"] = dgProducts.Rows[i].Cells["Cost_Center"].Value.ToString();
                    dr["Remarks"] = dgProducts.Rows[i].Cells["Remarks"].Value.ToString();
                    dtexisting.Rows.Add(dr);

                }
                dtexisting.AcceptChanges();
            }






            if (ioneNet.MaterialManagement.Transactions.GetProductsByMO.dtgetproducts.Rows.Count > 0)
            {
                DataTable dt = new DataTable();                
                dt.Columns.Add("Item_Code", typeof(string));
                dt.Columns.Add("Int_Prod_Code", typeof(string));
                dt.Columns.Add("Item_Description", typeof(string));
                dt.Columns.Add("Item_Spec", typeof(string));
                dt.Columns.Add("Item_Grade", typeof(string));
                dt.Columns.Add("UOM", typeof(string));
                dt.Columns.Add("Indent_Qty", typeof(decimal));
                dt.Columns.Add("Stock_Qty", typeof(decimal));
                dt.Columns.Add("Issue_Qty", typeof(decimal));
                dt.Columns.Add("Price", typeof(decimal));
                dt.Columns.Add("Amount", typeof(decimal));
                dt.Columns.Add("Cost_Center", typeof(string));
                dt.Columns.Add("Remarks", typeof(string));

                //dt.Rows.Add();
                for (int i = 0; i < ioneNet.MaterialManagement.Transactions.GetProductsByMO.dtgetproducts.Rows.Count; i++)
                {
                    string prodcode = ioneNet.MaterialManagement.Transactions.GetProductsByMO.dtgetproducts.Rows[i]["prod_id"].ToString();
                    string mrpno = ioneNet.MaterialManagement.Transactions.GetProductsByMO.dtgetproducts.Rows[i]["MRPNo"].ToString();
                    string indqty = ioneNet.MaterialManagement.Transactions.GetProductsByMO.dtgetproducts.Rows[i]["Indent_Qty"].ToString();
                    string stkqty = ioneNet.MaterialManagement.Transactions.GetProductsByMO.dtgetproducts.Rows[i]["Stock_Qty"].ToString();
                    string price = ioneNet.MaterialManagement.Transactions.GetProductsByMO.dtgetproducts.Rows[i]["Price"].ToString();

                    var getproducts = (from obj in db.Products
                                       join uom in db.UoM_Masters on obj.Prod_Primary_UOM_Id equals uom.UOM_ID
                                       where obj.prod_ID == Convert.ToInt32(prodcode)
                                       select new
                                       {
                                                                              
                                           Item_Code = obj.prod_ID,
                                           Int_Prod_Code = obj.Prod_Code,
                                           Item_Description = obj.Prod_Name,
                                           Item_Spec = "",
                                           Item_Grade = "",
                                           UOM = uom.Uom_Descr,
                                           Indent_Qty = indqty,
                                           Stock_Qty = stkqty,
                                           Issue_Qty = "0.00",
                                           Price = price,
                                           Amount = 0,
                                           Cost_Center = mrpno,
                                           Remarks = ""
                                       }).ToList();
                    dt.Rows.Add( getproducts[0].Item_Code, getproducts[0].Int_Prod_Code, getproducts[0].Item_Description, getproducts[0].Item_Spec, getproducts[0].Item_Grade, getproducts[0].UOM, getproducts[0].Indent_Qty, getproducts[0].Stock_Qty, getproducts[0].Issue_Qty, getproducts[0].Price, getproducts[0].Amount, getproducts[0].Cost_Center, getproducts[0].Remarks);

                }

                dtexisting = dtexisting.AsEnumerable().Union(dt.AsEnumerable()).CopyToDataTable();
                dgProducts.DataSource = dtexisting;
                int l = 0;
                for (int m = 0; m <= dgProducts.Rows.Count - 2; m++)
                {
                    if (dgProducts.Rows[m].Cells["Item_Code"].Value != "")
                    {
                        l = l + 1;
                        dgProducts.Rows[m].Cells["S_No"].Value = l.ToString();
                    }
                }

            }

        }

        private void cmbRefDocType_Leave(object sender, EventArgs e)
        {
            try
            {
                if (cmbRefDocType.Text == "MO No")
                {
                    button1.Enabled = true;
                }
                else
                {
                    button1.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void txtIndentNo_Enter(object sender, EventArgs e)
        {
            try
            {
                if (cmbRefDocType.Text == "Indent")
                {
                    txtIndentNo.AutoCompleteCustomSource = null;

                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    Addindent(DataColl);
                    txtIndentNo.AutoCompleteCustomSource = DataColl;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void Addindent(AutoCompleteStringCollection coll)
        {

            try
            {
                
                var Indname = (from p in db.Pen_Indents(logIn.company, dpSODate.Value, logIn.BU_ID) select new { p.Indent_NO}).ToList();
                //Indname.FirstOrDefault().Indent_NO;
                DataTable dt = new DataTable();
                dt.Columns.Add("Indent_NO");
                foreach (var item in Indname)
                {
                    dt.Rows.Add(item.Indent_NO);
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    coll.Add(dt.Rows[i][0].ToString());
                }


            }
            catch (Exception ex)
            {
                ex.Message.ToString();
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void cmbDepartment_Leave(object sender, EventArgs e)
        {
            int i = (cmbDepartment.FindString(cmbDepartment.Text));
            if (i < 0)
            {
                MessageBox.Show("Invalid Department Name");
                cmbDepartment.Focus();
            }
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
                MaterialManagement.Transactions.frmMaterialIssueVouchers obj = new MaterialManagement.Transactions.frmMaterialIssueVouchers();

                if (obj.ShowDialog() == DialogResult.OK)
                {
                    txtSlipNo.Text = MaterialManagement.Transactions.frmMaterialIssueVouchers.voucherNo;

                    if (!string.IsNullOrEmpty(txtSlipNo.Text))
                    {
                        var dm1 = (from s in db.Material_Issue_Childs
                                   join a in db.Material_Issue_Masters on s.Slip_Master_ID equals a.Id
                                   join pr in db.Products on s.Prod_Code equals pr.prod_ID
                                   join u in db.UoM_Masters on pr.Prod_Primary_UOM_Id equals u.UOM_ID
                                   where s.Slip_NO == txtSlipNo.Text && s.Company_ID == logIn.company  && a.BU_ID==logIn.BU_ID
                                   select new
                                   {
                                       S_No=s.ProdSno,
                                       Item_Code= s.Prod_Code,
                                       Int_Prod_Code = (logIn.company == 1044 ? pr.Prod_Alternative_Code : pr.Prod_Code),                                       
                                       Item_Description = pr.Prod_Name,
                                       Item_Spec= s.Prod_Spec,
                                       Item_Grade = s.Prod_Grade,
                                       UOM = u.Uom_Descr,
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

                    var f = (from s in db.Material_Issue_Masters where s.Slip_NO == txtSlipNo.Text && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID  select s).FirstOrDefault();
                    if (f != null)
                    {
                        dpSODate.Text = f.Slip_Date.ToString();
                        cmbRefDocType.Text = f.Ref_Doc_Type;
                        txtIndentNo.Text = f.Indent_No;
                        cmbIssuedTo.Text = f.Issued_To;
                        bindDept();
                        cmbDepartment.SelectedValue = f.Dept_Name;
                        if (f.Returnable == true)
                        {
                            checkBox1.Checked = true;
                        }
                        else
                        {
                            checkBox1.Checked = false;
                        }
                        txtRemarks.Text = f.Remarks;
                        //ObDate.Text = f.OBDate.ToString();
                        txtIssuedBy.Text = f.Issued_By.ToString();
                        if (f.Issued_Person_To != null)
                        {
                            txtIssuedTo.Text = f.Issued_Person_To.ToString();
                        }
                        lblCreatedBy.Text = f.Created_By;
                        lblModified.Text = f.Modified_By;
                        cmbFromBin.Text = f.FromBin;
                        cmbToBin.Text = f.ToBin;

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
                txtIndentNo.Text = "";
                cmbIssuedTo.Text = "";
                cmbDepartment.Text = "";
                txtIssuedBy.Text = logIn.username;
                txtIssuedTo.Text = "";
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
