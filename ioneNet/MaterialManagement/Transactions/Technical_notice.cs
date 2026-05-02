using Ione_DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using OpenCvSharp;


namespace ioneNet.MaterialManagement.Transactions
{
    public partial class Technical_notice : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        System.Data.Common.DbTransaction transaction;
        int r=0,c=0;
        public static string DocNo, ItemCode, RecQty, Suppname, SONo;
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();
        public Technical_notice()
        {
            InitializeComponent();
        }
       
        DataTable dtexisting = new DataTable();
        public void sow()
        {

            try
            {
                
                var p = (from s in db.Scopeofwork_Masters
                         where s.Status_ID==1 && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID
                         select new
                         {
                             scopename=s.Scope_Name,
                         }
                        );
                if (p.Count() > 0)
                {
                    int i = 0;
                    foreach (var item in p)
                    {
                        dgscope.Rows.Add();
                        dgscope[0,i].Value = item.scopename;
                        i=i + 1;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void bindcust()
        {
            var Buyerblind = (from m in db.Supplier_informations
                              join a in db.Attributes_Datas on m.Supplier_Category equals a.ID
                              where m.Company_ID == logIn.company && m.Status == 1
                              //&& a.Descr != "Customer"
                              select new { m.ID, m.Supplier_Name }).Distinct().ToList();
            if (Buyerblind.Count > 0)
            {
                cmbsupname.DataSource = Buyerblind;
                cmbsupname.ValueMember = "ID";
                cmbsupname.DisplayMember = "Supplier_Name";
            }
            cmbsupname.SelectedIndex = -1;
        }
        public void recdepbind()
        {
            var dep = (from m in db.Department_Masters where (m.Company_ID == logIn.company || m.Company_ID == 0) select new { m.Id, m.Dept_Name }).Distinct().ToList();
            if (dep.Count > 0)
            {
                cmb_rec_dep.DataSource = dep;
                cmb_rec_dep.ValueMember = "Id";
                cmb_rec_dep.DisplayMember = "Dept_Name";
            }
            cmb_rec_dep.SelectedIndex = -1;
        }
        public void reldepbind()
        {
            var dep1 = (from m in db.Department_Masters where (m.Company_ID == logIn.company || m.Company_ID == 0) select new { m.Id, m.Dept_Name }).Distinct().ToList();
            if (dep1.Count > 0)
            {
                cmb_rel_dep.DataSource = dep1;
                cmb_rel_dep.ValueMember = "Id";
                cmb_rel_dep.DisplayMember = "Dept_Name";
            }
            cmb_rel_dep.SelectedIndex = -1;
        }
        private void bind()
        {
            sow();
            var pStatus = (from m in db.Attributes_Datas
                           join r in db.view_Trans_Auth_Levels
                           on m.ID equals r.Status_Code
                           where r.Menu_Item == this.Text && (r.Company_ID == logIn.company) && r.Role_ID == logIn.UserRoleID
                           select new { m.ID, m.Descr }).Distinct().ToList();
            if (pStatus.Count > 0)
            {
                cmbStatus.DataSource = pStatus;
                cmbStatus.ValueMember = "ID";
                cmbStatus.DisplayMember = "Descr";
            }



            //var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Trans" && (m.Company_ID == logIn.company || m.Company_ID == 0) select new { m.ID, m.Descr }).Distinct().ToList();
            //if (pStatus.Count > 0)
            //{
            //    cmbStatus.DataSource = pStatus;
            //    cmbStatus.ValueMember = "ID";
            //    cmbStatus.DisplayMember = "Descr";
            //}
            //cmbStatus.SelectedIndex = -1;

           

            

           

           
        }

        public void AutoincrementId()
        {
            try
            {

                var result = db.Sp_autoincrement_technical_notice(logIn.company, logIn.BU_ID, logIn.fy_Start_Date, logIn.fy_End_Date);
                txt_vchno.Text = result.FirstOrDefault().TC_No;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                AutoincrementId();
                foreach (Control c in tableLayoutPanel1.Controls.Cast<Control>().OrderBy(c => c.TabIndex))
                {
                    if (c is TextBox)
                    {
                        c.Text = "";
                    }
                    else
                    if (c is ComboBox)
                    {
                        c.Text = "";
                    }
                }
                cmbStatus.SelectedIndex = -1;
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Products", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int count = 0;
                for(int i=0;i<dgProducts.Rows.Count-1;i++)
                {
                    if (dgProducts.Rows[i].Cells["Item_ID"].Value.ToString() == "")
                    {
                        count = count + 1;
                        if(count>1)
                        {
                            dgProducts.Rows.RemoveAt(i);
                            count = count - 1;
                        }
                    }
                    
                }
                
                if (txt_vchno.Text != "")
                {
                    if (cmbStatus.Text == string.Empty)
                    {
                        MessageBox.Show("Select Status", "Technical Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cmbStatus.Focus();
                        return;
                    }
                    else
                    //Check Wether 1st Row is filled or not


                    if (dgProducts.Rows[0].Cells["ItemName"].Value == null)
                    {
                        MessageBox.Show("Atleast One Record To Be Entered To Proceed", "Technical Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    if (cmbAsset.Text == string.Empty)
                    {
                        MessageBox.Show("Select Project Code", "Technical Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cmbAsset.Focus();
                        return;
                    }
                    else if (cmb_rel_dep.Text == string.Empty)
                    {
                        MessageBox.Show("Select Releasing Departement", "Technical Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cmb_rel_dep.Focus();
                        return;
                    }
                    else if (cmb_rec_dep.Text == string.Empty)
                    {
                        MessageBox.Show("Select Receiving Departement", "Technical Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        cmb_rec_dep.Focus();
                        return;
                    }
                    else if (txt_proj_type.Text == "")
                    {
                        MessageBox.Show("Enter Product Type", "Technical Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txt_proj_type.Focus();
                        return;
                    }
                    else if (txt_version.Text == "")
                    {
                        MessageBox.Show("Enter Version No", "Technical Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        txt_version.Focus();
                        return;
                    }
                    //if (cmbStatus.Text == "Approved")
                    //{

                    //    var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Purchase Requisition" && m.Role_ID == logIn.UserRoleID select new { m.Approve_Role }).Distinct().ToList();
                    //    if (uRole.Count > 0)
                    //    {
                    //        if (uRole[0].Approve_Role == false)
                    //        {
                    //            MessageBox.Show("You Are Not Authorized To Appove ");
                    //            return;
                    //        }
                    //        else
                    //        {
                    //            SaveTC();
                    //        }
                    //    }
                    //}

                    else
                    {
                        SaveTC();
                    }
                }
                else
                {
                    MessageBox.Show("Enter TC NO");
                    txt_vchno.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, " Technical Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
         
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgProducts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].Name;
                if ( R1.Cells["ItemName"].Value != null)
                {

                    var getProductName = (from s in db.Products
                                          join u in db.UoM_Masters on s.Prod_Primary_UOM_Id equals u.UOM_ID
                                          join g in db.Product_Groups on s.Prod_Group_Id equals g.ID
                                          where s.Prod_Name == R1.Cells["ItemName"].Value.ToString() && s.Company_ID == logIn.company
                                          select new { s.prod_ID,s.Prod_Code, u.Uom_Descr,s.Prod_Description, g.Prod_Group_Name, s.Prod_HSN_Code,s.Prod_Field2 }).FirstOrDefault();

                    if (getProductName != null)
                    {
                        R1.Cells["Item_ID"].Value = getProductName.prod_ID.ToString();
                        R1.Cells["Item_Code"].Value = getProductName.Prod_Code.ToString();
                        R1.Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                        R1.Cells["DrawingNo"].Value = getProductName.Prod_Field2.ToString();
                        R1.Cells["Item_Description"].Value = getProductName.Prod_Description.ToString();
                        
                        if (R1.Cells["S_No"].Value ==null || R1.Cells["S_No"].Value.ToString()=="")
                        {
                            R1.Cells["S_No"].Value = dgProducts.Rows.Count - 1;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Product Name Entered");
                        R1.Cells["ItemName"].Value = "";
                        return;
                    }

                 
                }

                else if ( R1.Cells["DrawingNo"].Value != null)
                {

                    var getProductName = (from s in db.Products
                                          join u in db.UoM_Masters on s.Prod_Primary_UOM_Id equals u.UOM_ID
                                          join g in db.Product_Groups on s.Prod_Group_Id equals g.ID
                                          where s.Prod_Field2 == R1.Cells["DrawingNo"].Value.ToString() && s.Company_ID == logIn.company
                                          select new { s.prod_ID,s.Prod_Code, u.Uom_Descr, g.Prod_Group_Name, s.Prod_HSN_Code, s.Prod_Name }).FirstOrDefault();

                    if (getProductName != null)
                    {
                        R1.Cells["Item_ID"].Value = getProductName.prod_ID.ToString();
                        R1.Cells["Item_Code"].Value = getProductName.Prod_Code.ToString();
                        R1.Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                        R1.Cells["ItemName"].Value = getProductName.Prod_Name.ToString();
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
                    if (columnName == "Item Name/Make")
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
                        if (columnName == "Drawing No/Model No")
                        {
                            var Prodname = (from d in db.Products where d.Company_ID == logIn.company select new { d.Prod_Field2 }).Distinct().ToList();
                            DataTable dt = new DataTable();
                            dt.Columns.Add("Prod_Field2");
                            foreach (var item in Prodname)
                            {
                                dt.Rows.Add(item.Prod_Field2);
                            }
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                coll.Add(dt.Rows[i][0].ToString());
                            }
                        }
                        //else
                        //{
                        //    if (columnName == "Scope_of_work")
                        //    {
                        //        var Prodname = (from d in db.Scopeofwork_Masters where d.Company_ID == logIn.company select new { d.Scope_Name }).Distinct().ToList();
                        //        DataTable dt = new DataTable();
                        //        dt.Columns.Add("Scope_Name");
                        //        foreach (var item in Prodname)
                        //        {
                        //            dt.Rows.Add(item.Scope_Name);
                        //        }
                        //        for (int i = 0; i < dt.Rows.Count; i++)
                        //        {
                        //            coll.Add(dt.Rows[i][0].ToString());
                        //        }
                        //    }

                        //}

                    }
                     
                    

                }
            }
            catch (Exception ex)
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
                if (tb3 != null && columnName == "Item Name/Make")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }

                if (tb3 != null && columnName == "Drawing No/Model No")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }

                //if ( columnName == "Scope_of_work")
                //{
                //    if (tb3 != null)
                //    {
                //        tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                //        tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                //        AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                //        addItems(DataColl);
                //        tb3.AutoCompleteCustomSource = DataColl;
                //    }
                //}




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgProducts_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F6 )
                {
                    if (dgProducts.Rows.Count > 0)
                    {


                        //foreach (DataGridViewCell oneCell in dgProducts.SelectedCells)
                        //{
                            //ask for permission
                            DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                        if (result == DialogResult.Yes)
                        {
                            int i = dgProducts.CurrentCell.RowIndex;
                            SONo = txt_vchno.Text;
                            if (dgProducts.Rows[i].Cells["Item_ID"].Value == null)
                            {
                                dgProducts.Rows.RemoveAt(dgProducts.CurrentRow.Index);
                            }
                            else
                            {
                                ItemCode = dgProducts.Rows[i].Cells["Item_ID"].Value.ToString();
                                SqlCommand cmd1 = new SqlCommand("delete  from [technical_notice_child_1] where [Voch_no] =@pono and [Item_ID] = @ProdID", con);
                                cmd1.Parameters.AddWithValue("@ProdID", ItemCode);
                                cmd1.Parameters.AddWithValue("@pono", SONo);
                                if (con.State != ConnectionState.Open)
                                    con.Open();

                                cmd1.ExecuteNonQuery();
                                con.Close();


                                dgProducts.Rows.RemoveAt(dgProducts.CurrentRow.Index);

                            }
                            int l = 0;
                            for (int m = 0; m <= dgProducts.Rows.Count - 2; m++)
                            {
                                if (dgProducts.Rows[m].Cells["Item_ID"].Value != "")
                                {
                                    l = l + 1;
                                    dgProducts.Rows[m].Cells["S_No"].Value = l.ToString();
                                }
                            }
                        }

                       // }



                    }

                }
                if (e.KeyCode == Keys.F5)
                {
                    if (dgProducts.Rows.Count > 0)
                    {

                        try
                        {

                                Scopeofworkgb.Visible = true;
                                r = dgProducts.CurrentRow.Index;

                            
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }

                }
                DataTable dtexisting = new DataTable();
                if (e.KeyCode == Keys.F2)
                {
                    ioneNet.Masters.ProdSearch form = new ioneNet.Masters.ProdSearch();
                    ioneNet.Masters.ProdSearch.frmName = "TC";
                    form.ShowDialog();
                    if (dgProducts.Rows.Count > 1)
                    {
                        dtexisting.Rows.Clear();
                        dtexisting.Columns.Clear();

                        dtexisting.Columns.Add("S_No", typeof(string));  
                        dtexisting.Columns.Add("Item_ID", typeof(string));
                        dtexisting.Columns.Add("Item_Code", typeof(string));
                        dtexisting.Columns.Add("DrawingNo", typeof(string));
                        dtexisting.Columns.Add("ItemName", typeof(string));
                        dtexisting.Columns.Add("Item_Description", typeof(string));
                        dtexisting.Columns.Add("Prod_Length", typeof(string));
                        dtexisting.Columns.Add("RM", typeof(string));
                        dtexisting.Columns.Add("UOM", typeof(string));
                        dtexisting.Columns.Add("Indent_Qty", typeof(string));
                        dtexisting.Columns.Add("Scope_of_work", typeof(string));
                       

                        for (int i = 0; i < dgProducts.Rows.Count-1 ; i++)
                        {
                            DataRow dr;
                            dr = dtexisting.NewRow();
                            dr["S_No"] = dgProducts.Rows[i].Cells["S_No"].Value.ToString();
                            dr["Item_ID"] = dgProducts.Rows[i].Cells["Item_ID"].Value.ToString(); 
                            dr["Item_Code"] = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                            dr["DrawingNo"] = dgProducts.Rows[i].Cells["DrawingNo"].Value.ToString();
                            dr["ItemName"] = dgProducts.Rows[i].Cells["ItemName"].Value.ToString();
                            dr["Item_Description"] = dgProducts.Rows[i].Cells["Item_Description"].Value.ToString();
                            dr["Prod_Length"] = (dgProducts.Rows[i].Cells["Prod_Length"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Prod_Length"].Value);
                            dr["RM"] = (dgProducts.Rows[i].Cells["RM"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["RM"].Value);
                            dr["UOM"] = dgProducts.Rows[i].Cells["UOM"].Value.ToString();
                            dr["Indent_Qty"] = (dgProducts.Rows[i].Cells["Indent_Qty"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Indent_Qty"].Value);
                            dr["Scope_of_work"] = (dgProducts.Rows[i].Cells["Scope_of_work"].Value == DBNull.Value) ? "" : (dgProducts.Rows[i].Cells["Scope_of_work"].Value);

                            dtexisting.Rows.Add(dr);

                        }
                        dtexisting.AcceptChanges();
                    }






                    if (ioneNet.Masters.ProdSearch.dtgetproducts.Rows.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("S_No", typeof(string));
                        dt.Columns.Add("Item_ID", typeof(string));
                        dt.Columns.Add("Item_Code", typeof(string));
                        dt.Columns.Add("DrawingNo", typeof(string));
                        dt.Columns.Add("ItemName", typeof(string));
                        dt.Columns.Add("Item_Description", typeof(string));
                        dt.Columns.Add("Prod_Length", typeof(string));
                        dt.Columns.Add("RM", typeof(string));
                        dt.Columns.Add("UOM", typeof(string));
                        dt.Columns.Add("Indent_Qty", typeof(string));
                        dt.Columns.Add("Scope_of_work", typeof(string));


                        //dt.Rows.Add();
                        int j = dgProducts.Rows.Count - 1;
                        for (int i = 0; i < ioneNet.Masters.ProdSearch.dtgetproducts.Rows.Count; i++)
                        {
                            string prodcode = ioneNet.Masters.ProdSearch.dtgetproducts.Rows[i]["prod_id"].ToString();
                            var getproducts = (from obj in db.Products
                                               join uom in db.UoM_Masters on obj.Prod_Primary_UOM_Id equals uom.UOM_ID
                                               where obj.prod_ID == Convert.ToInt32(prodcode)
                                               select new
                                               {

                                                   // {

                                                   Item_ID = obj.prod_ID,
                                                   Item_Code = obj.Prod_Code,
                                                   ItemName = obj.Prod_Name,
                                                   Item_Description = obj.Prod_Description,
                                                   Item_Grade = obj.Prod_Field2,
                                                   Length = "",
                                                   RM = "",
                                                   UOM = uom.Uom_Descr,
                                                   Qty_Required = "",
                                                   Scope_of_work = ""
                                               }).ToList();
                            j = j + 1;
                            dt.Rows.Add(j, getproducts[0].Item_ID, getproducts[0].Item_Code, getproducts[0].Item_Grade, getproducts[0].ItemName, getproducts[0].Item_Description, getproducts[0].Length, getproducts[0].RM, getproducts[0].UOM, getproducts[0].Qty_Required, getproducts[0].Scope_of_work);

                        }

                        dtexisting = dtexisting.AsEnumerable().Union(dt.AsEnumerable()).CopyToDataTable();
                        dgProducts.DataSource = dtexisting;

                        int l = 0;
                        for (int m = 0; m <= dgProducts.Rows.Count - 2; m++)
                        {
                            if (dgProducts.Rows[m].Cells["Item_ID"].Value != "")
                            {
                                l = l + 1;
                                dgProducts.Rows[m].Cells["S_No"].Value = l.ToString();
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

        private void label20_Click(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
           ioneNet.MaterialManagement.Masters.Scope_of_work s = new ioneNet.MaterialManagement.Masters.Scope_of_work();
           s.Show();
           
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                string s = "";
                foreach (DataGridViewRow row in dgscope.Rows)
                {

                    if (Convert.ToBoolean(row.Cells[1].Value) == true)
                    {
                        if (s != "")
                        {
                            s = s + "," + row.Cells[0].Value.ToString();
                        }
                        else
                        {
                            s = row.Cells[0].Value.ToString();
                        }
                    }
                }
                dgProducts.Rows[r].Cells["Scope_of_work"].Value = s;
                Scopeofworkgb.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Scopeofworkgb.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            sow();
        }

        private void label33_Click(object sender, EventArgs e)
        {

        }

        private void cmbAsset_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbAsset_Leave(object sender, EventArgs e)
        {
            try
            {

                var Prodname = (from d in db.Project_code_Masters where d.Company_ID == logIn.company && d.Project_Code == cmbAsset.Text 
                                select new { d.Project_Description,d.Project_Type }).Distinct().ToList();
                if (Prodname.Count > 0)
                {
                    txt_projname.Text = Prodname[0].Project_Description;
                    txt_proj_type.Text = Prodname[0].Project_Type;

                }
                else
                {
                    MessageBox.Show("Invalid Project Code");
                    cmbAsset.Focus();
                    return;
                }

                var pcode = (from m in db.Engg_Mfg_Orders where m.Company_ID == logIn.company && m.Project_Code== cmbAsset.Text select new { m.id, m.MO_No }).Distinct().ToList();
                if (pcode.Count > 0)
                {
                    cmb_MOno.DataSource = pcode;
                    cmb_MOno.ValueMember = "id";
                    cmb_MOno.DisplayMember = "MO_No";
                }
                cmb_MOno.SelectedIndex = -1;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void txt_vchno_Leave(object sender, EventArgs e)
        {
            //var da = (from obj in db.technical_notice_masters
            //          where obj.Vch_no == txt_vchno.Text && obj.Company_ID == logIn.company
            //          select obj).ToList();

            //if (da.Count > 0)
            //{
            //    MessageBox.Show("Record already exist with TC Number" + txt_vchno.Text + ".");
            //    txt_vchno.Focus();
            //}
        }

        private void cmbAsset_Enter(object sender, EventArgs e)
        {
            bindproj();
        }

        private void cmbsupname_Leave(object sender, EventArgs e)
        {

            


            if (cmbsupname.Text != "")
            {
                int i = (cmbsupname.FindString(cmbsupname.Text));

               // var n = cmbsupname.Items[cmbsupname.SelectedIndex];
                //var m = cmbsupname.SelectedItem;
                
                if (i < 0)
                {
                    MessageBox.Show("Invalid Supplier Name");
                    cmbsupname.Focus();
                }
            }
        }





        private void cmbsupname_Enter(object sender, EventArgs e)
        {
            bindcust();
        }

        private void cmb_rel_dep_Enter(object sender, EventArgs e)
        {
            reldepbind();
        }

        private void cmb_rel_dep_Leave(object sender, EventArgs e)
        {
            if (cmb_rel_dep.Text != "")
            {
                int i = (cmb_rel_dep.FindString(cmb_rel_dep.Text));
                if (i < 0)
                {
                    MessageBox.Show("Invalid Department Name");
                    cmb_rel_dep.Focus();
                }
            }


        }

        private void cmb_rec_dep_Leave(object sender, EventArgs e)
        {
            if (cmb_rec_dep.Text != "")
            {
                int i = (cmb_rec_dep.FindString(cmb_rec_dep.Text));
                if (i < 0)
                {
                    MessageBox.Show("Invalid Department Name");
                    cmb_rec_dep.Focus();
                }
            }
        }

        private void cmb_rec_dep_Enter(object sender, EventArgs e)
        {
            recdepbind();
        }

        private void button5_Click(object sender, EventArgs e)
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
                dt.Columns.Add(new DataColumn("S_No", typeof(string)));
                dt.Columns.Add(new DataColumn("Item_Code", typeof(string)));
                dt.Columns.Add(new DataColumn("DrawingNo", typeof(string)));
                dt.Columns.Add(new DataColumn("ItemName", typeof(string)));
                dt.Columns.Add(new DataColumn("Item_Description", typeof(string)));
                dt.Columns.Add(new DataColumn("Prod_Length", typeof(string)));
                dt.Columns.Add(new DataColumn("RM", typeof(string)));
                dt.Columns.Add(new DataColumn("UOM", typeof(string)));
                dt.Columns.Add(new DataColumn("Indent_Qty", typeof(string)));
                dt.Columns.Add(new DataColumn("Scope_of_work", typeof(string)));
                for (int i = 0; i < count; i++)
                {
                    dr = dt.NewRow();
                    //txtProdID.Text = result.FirstOrDefault().Product_Code;
                    //dr["Prod_Code"] = result.FirstOrDefault().Product_Code; ;
                    dr["S_No"] = DtSet.Rows[i]["S_No"].ToString();
                    dr["Item_Code"] = DtSet.Rows[i]["Item_Code"].ToString();
                    dr["DrawingNo"] = DtSet.Rows[i]["DrawingNo"].ToString();
                    dr["ItemName"] = DtSet.Rows[i]["ItemName"].ToString(); ;
                    dr["Item_Description"] = DtSet.Rows[i]["Item_Description"].ToString();
                    dr["Prod_Length"] = DtSet.Rows[i]["Prod_Length"].ToString();
                    dr["RM"] = DtSet.Rows[i]["RM"].ToString();
                    dr["UOM"] = DtSet.Rows[i]["UOM"].ToString();
                    dr["Indent_Qty"] = DtSet.Rows[i]["Qty_Required"].ToString(); ;
                    dr["Scope_of_work"] = DtSet.Rows[i]["Part_Work_Scope"].ToString();


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
                        dgProducts.Rows[i].Cells["Item_ID"].Value = getProductName.prod_ID.ToString();
                        dgProducts.Rows[i].Cells["ItemName"].Value = getProductName.Prod_Name.ToString();
                        dgProducts.Rows[i].Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                        dgProducts.Rows[i].Cells["DrawingNo"].Value = getProductName.Prod_Field2.ToString();
                        dgProducts.Rows[i].Cells["Item_Description"].Value = getProductName.Prod_Description.ToString();

                        if (dgProducts.Rows[i].Cells["S_No"].Value == null || dgProducts.Rows[i].Cells["S_No"].Value.ToString() == "")
                        {
                            dgProducts.Rows[i].Cells["S_No"].Value = i+1;
                        }

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

        public void bindproj()
        {

            try
            {
                var pcode = (from m in db.Project_code_Masters where m.Company_ID == logIn.company select new { m.id, m.Project_Code }).Distinct().ToList();
                if (pcode.Count > 0)
                {
                    cmbAsset.DataSource = pcode;
                    cmbAsset.ValueMember = "id";
                    cmbAsset.DisplayMember = "Project_Code";
                }
                cmbAsset.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void dgProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                var s = (DataGridView)sender;
                if (s.Columns[e.ColumnIndex] is DataGridViewButtonColumn && e.RowIndex >= 0)
                {
                    Scopeofworkgb.Visible = true;
                    r = e.RowIndex;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            

        }

        public void SaveTC()
        {
            try
            {
                string AmendNo = txtAmendNo.Text;
                if (Technical_Notice_List.var == "3")
                {

                    var result = db.Sp_autoincrement_TC_AmendNo(logIn.company, logIn.BU_ID, txt_vchno.Text);
                    txtAmendNo.Text = result.FirstOrDefault().TC_Amend_no;

                    //Set Previous Version Quote Status as Amended
                    var ci = db.technical_notice_masters.Where(w => w.Vch_no == txt_vchno.Text && w.Amend_No == Convert.ToInt32(AmendNo) && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                    {

                        ci.is_amended = true;
                        ci.Modified_by = logIn.username + "-" + DateTime.Now;
                        db.SubmitChanges();

                    }


                }


                if (txt_vchno.Text != "")
                {
                    //if (cmbShipTo.SelectedValue == null)
                    //{
                    //    Ship_To = 0;
                    //}
                    //else
                    //{
                    //    Ship_To = Convert.ToInt32(cmbShipTo.SelectedValue.ToString());
                    //}
                    SqlCommand cmd = new SqlCommand("Save_technical_notice", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Vch_no", txt_vchno.Text);
                    cmd.Parameters.AddWithValue("@V_date", vchDate.Value);
                    cmd.Parameters.AddWithValue("@Job_no", "");
                   // cmd.Parameters.AddWithValue("@Imp_date",impdate.Value);
                    cmd.Parameters.AddWithValue("@Rec_department", (cmb_rec_dep.Text));
                    cmd.Parameters.AddWithValue("@Dep_no", (cmb_rel_dep.Text));
                    cmd.Parameters.AddWithValue("@Serial_no",(cmb_MOno.Text=="")? 0: (Convert.ToInt32(cmb_MOno.SelectedValue.ToString())));
                    cmd.Parameters.AddWithValue("@Version", (txt_version.Text == "") ? "" : txt_version.Text);
                    cmd.Parameters.AddWithValue("@Project_Name", (Convert.ToInt32( cmbAsset.SelectedValue.ToString())));
                    cmd.Parameters.AddWithValue("@Project_type", (txt_proj_type.Text == "") ? "" : txt_proj_type.Text);
                    cmd.Parameters.AddWithValue("@Supplier_Name", (cmbsupname.Text == "") ? 0 : (Convert.ToInt32(cmbsupname.SelectedValue.ToString())));
                    cmd.Parameters.AddWithValue("@Raw_Material", (txtRM.Text == "") ? "" : txtRM.Text);
                    cmd.Parameters.AddWithValue("@Process", (txtprocess.Text == "") ? "" : txtprocess.Text);
                    cmd.Parameters.AddWithValue("@Painting", (txtpainting.Text == "") ? "" : txtpainting.Text);
                    cmd.Parameters.AddWithValue("@Sliver_Plating", (txtsliver.Text == "") ? "" : txtsliver.Text);
                    cmd.Parameters.AddWithValue("@Other", (txtother.Text == "") ? "" : txtother.Text);
                    cmd.Parameters.AddWithValue("@Status", Convert.ToInt32(cmbStatus.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@Company_ID", logIn.company);
                    cmd.Parameters.AddWithValue("@BU_ID", logIn.BU_ID);
                    cmd.Parameters.AddWithValue("@Created_By", lblCreatedBy.Text);
                     cmd.Parameters.AddWithValue("@isDeleted", false);
                    
                    
                    cmd.Parameters.AddWithValue("@Amend_No", (txtAmendNo.Text == "") ? 0 : Convert.ToInt32(txtAmendNo.Text));
                    cmd.Parameters.AddWithValue("@TC_Mtrl_Group", txtTCMaterial.Text);
                    cmd.Parameters.AddWithValue("@is_amended", false);
                    cmd.Parameters.AddWithValue("@Modified_By", logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt"));
                   
                    string Item_ID = "";
                    string Item_Code = "";
                    string Item_Name = "";
                    string Drawing_No = "";
                    string RM = "";
                    string length = "";
                    string UOM = "";
                    string Qty = "";
                    string Scope_of_work = "";
                    string Company_ID = "";
                    string BUID = "";
                    int rowcount = 0 ;
                    string S_No ="";
                    for (int i = 0; i < dgProducts.RowCount - 1; i++)
                    {
                        Item_ID = Item_ID + Convert.ToString(dgProducts.Rows[i].Cells["Item_ID"].Value).Trim().PadRight(14);
                        //Item_Code = Item_Code+ Convert.ToString(dgProducts.Rows[i].Cells["Item_Code"].Value).Trim().PadRight(25);
                        //Item_Name = Item_Name + Convert.ToString(dgProducts.Rows[i].Cells["ItemName"].Value).Trim().PadRight(25);
                        length = length + Convert.ToString(dgProducts.Rows[i].Cells["Prod_Length"].Value).PadRight(14);
                        //Drawing_No = Drawing_No + Convert.ToString(dgProducts.Rows[i].Cells["DrawingNo"].Value).Trim().PadRight(14);
                        RM = RM + Convert.ToString(dgProducts.Rows[i].Cells["RM"].Value).PadRight(250);
                        //UOM = UOM + Convert.ToString(dgProducts.Rows[i].Cells["UOM"].Value).PadRight(14);
                        Qty = Qty + Convert.ToString(dgProducts.Rows[i].Cells["Indent_Qty"].Value).PadRight(14);
                        Scope_of_work = Scope_of_work + Convert.ToString(dgProducts.Rows[i].Cells["Scope_of_work"].Value).PadRight(250);
                        Company_ID = logIn.company.ToString();
                        BUID = logIn.BU_ID.ToString();
                        S_No = S_No + Convert.ToString(dgProducts.Rows[i].Cells["S_No"].Value).PadRight(14);
                        rowcount = rowcount + 1;

                    }
                    cmd.Parameters.AddWithValue("@txt_item_ID", Item_ID);
                    cmd.Parameters.AddWithValue("@txt_Item_Code", Item_Code);
                    cmd.Parameters.AddWithValue("@txt_Item_Name", Item_Name);
                    cmd.Parameters.AddWithValue("@txt_prd_Length", length);
                    cmd.Parameters.AddWithValue("@txt_Drawing_No", Drawing_No);
                    cmd.Parameters.AddWithValue("@txt_RM", RM);
                    cmd.Parameters.AddWithValue("@txt_UOM", UOM);
                    cmd.Parameters.AddWithValue("@txt_Qty", Qty);
                    cmd.Parameters.AddWithValue("@txt_Scope_of_work", Scope_of_work);
                    //cmd.Parameters.AddWithValue("@txt_CompanyID", Company_ID);
                    //cmd.Parameters.AddWithValue("@txt_BUID", BUID);
                    cmd.Parameters.AddWithValue("@txt_S_No", S_No);
                    cmd.Parameters.AddWithValue("@gridcount1", rowcount);
                    try
                    {
                        con.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (Convert.ToBoolean(result))
                        {


                            //if (cmbStatus.Text == "Approved")
                            //{
                            //    var ci = db.technical_notice_masters.Where(w => w.Vch_no == txt_vchno.Text && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                            //    {
                            //        ci.Status = 6;
                            //        //ci.Modified_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                            //        ci.Approved_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                            //        db.SubmitChanges();
                            //    }

                            //}
                            //else
                            //if (cmbStatus.Text == "Reviewed")
                            //{
                            //    var ci = db.technical_notice_masters.Where(w => w.Vch_no == txt_vchno.Text && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                            //    {
                            //        ci.Status = 4;
                            //        //ci.Modified_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                            //        ci.Reviewed_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                            //        db.SubmitChanges();
                            //    }

                            //}
                            MessageBox.Show("Record has been successfully Saved/Updated with Voucher No :" + txt_vchno.Text);
                           

                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        con.Close();
                    }





                    //technical_notice_master tc =new technical_notice_master();
                    //{
                    //    tc.isDeleted = false;
                    //}
                    //try
                    //{
                    //    con.Close();
                    //    con.Open();
                    //    int result = cmd.ExecuteNonQuery();

                    //    if (Convert.ToBoolean(result))
                    //    {
                    
                    //    //AutoincrementId();



                    //}
                    //catch (Exception ex)
                    //{
                    //    MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    //}
                    //finally
                    //{
                    //    con.Close();
                    //}


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

        private void Form1_Load(object sender, EventArgs e)
        {
            Scopeofworkgb.Visible = false;
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            bindproj();
            bind();
            bindcust();
            reldepbind();
            recdepbind();
            if (MaterialManagement.Transactions.Technical_Notice_List.var == "0" || MaterialManagement.Transactions.Technical_Notice_List.var == "3")
            {
                if (Technical_Notice_List.editMode == true)
                {
                    txt_vchno.Text = MaterialManagement.Transactions.Technical_Notice_List.SO_No;
                    bindedit();

                }
            }
            else

           
           
            if (MaterialManagement.Transactions.Technical_Notice_List.var == "1")
            {
                if (Technical_Notice_List.editMode == true)
                {
                    txt_vchno.Text = MaterialManagement.Transactions.Technical_Notice_List.SO_No;
                    bindedit();
                    btnClear.Enabled = false;
                    btnSave.Enabled = false;
                    cmbStatus.Enabled = false;

                }
            }

            else
            {
               AutoincrementId();
            }


        }

        public void bindedit()
        {
            try
            {
                int TC_id = 0;
                string tc_no = txt_vchno.Text;
                var da = (from obj in db.technical_notice_masters
                          where obj.Vch_no == tc_no && obj.Company_ID == logIn.company && obj.is_amended ==false
                          select obj).ToList();
                
                if (da.Count > 0)
                {
                    TC_id = da[0].Id;
                    txt_vchno.Text = da[0].Vch_no.ToString();
                    vchDate.Value = da[0].V_date.Value;
                    //bindCustomer();
                    cmb_rel_dep.Text = da[0].Dep_no;

                    //impdate.Value = da[0].Imp_date.Value;
                    cmb_rec_dep.Text = da[0].Rec_department;
                   
                    cmb_MOno.SelectedValue = Convert.ToInt32(da[0].Serial_no);
                    txt_version.Text = da[0].Version;
                    var da1 = (from obj in db.Project_code_Masters
                              where obj.id == Convert.ToInt32(da[0].Project_Name) && obj.Company_ID == logIn.company
                              select obj).ToList();
                    txt_projname.Text = da1[0].Project_Description;
                    if (da.Count > 0)
                    {
                        cmbAsset.Text =da1[0].Project_Code;
                    }
                    
                    txt_proj_type.Text = da[0].Project_type;
                    cmbStatus.SelectedValue = da[0].Status;
                    cmbsupname.SelectedValue = da[0].Supplier_Name;
                    txtRM.Text = da[0].Raw_Material;
                    txtpainting.Text = da[0].Painting;
                    txtprocess.Text = da[0].Process;
                    txtsliver.Text = da[0].Sliver_Plating;
                    txtother.Text = da[0].Other;
                  
                    lblCreatedBy.Text = da[0].Created_by;
                    lblModified.Text = da[0].Modified_by;
                    txtTCMaterial.Text = da[0].TC_Mtrl_Group;
                    txtAmendNo.Text = da[0].Amend_No.ToString();
                }


                var dm1 = (from s in db.technical_notice_child_1s
                           join i in db.Products on s.Item_ID equals i.prod_ID
                           join u in db.UoM_Masters on i.Prod_Alternative_UOM_Id equals u.UOM_ID
                           join p in db.technical_notice_masters on s.M_ID equals p.Id
                           where s.M_ID== TC_id


                           select new

                           {
                               S_No=s.S_No,
                               s.Item_ID,
                               Item_Code=i.Prod_Code,
                               ItemName =i.Prod_Name,
                               DrawingNo=i.Prod_Field2,
                               Item_Description = i.Prod_Description,
                               Prod_Length = s.Length.Trim(),
                               RM=s.RM.Trim(),
                               UOM=u.Uom_Descr,
                               Indent_Qty=s.Qty,
                               Scope_of_work=s.Scope_of_work.Trim(),

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

