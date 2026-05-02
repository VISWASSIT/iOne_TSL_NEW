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
using System.Globalization;
using System.IO;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Ione_DAL;
namespace ioneNet.ProductionManagement.Transactions
{
    public partial class frmForge_JobCard : Form
    {
        
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static int CustName;
        public frmForge_JobCard()
        {
            InitializeComponent();
        }

        private void checkedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cmbBasis_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (cmbBasis.Text == "Sale Order")
                //{
                //    var Buyerblind = (from m in db.Sale_Order_Masters      
                //                      join sc in db.Sale_Order_Childs on m.Id  equals sc.So_Master_ID                                
                //                      where m.Company_ID == logIn.company && m.Status == 6 && m.Job_Work_Order == false select new { m.SO_NO }).Distinct().ToList();
                //    if (Buyerblind.Count > 0)
                //    {
                //        cmbRefDocNo.DataSource = Buyerblind;
                //        cmbRefDocNo.ValueMember = "SO_No";
                //        cmbRefDocNo.DisplayMember = "SO_No";

                //    }
                //    //if (CmbBuyerName.Items.Count > 0)
                //    cmbRefDocNo.SelectedIndex = -1;
                //}
                //else
                //if (cmbBasis.Text == "Job Work")
                //{

                //    var Buyerblind = (from m in db.Sale_Order_Masters
                //                      join sc in db.Sale_Order_Childs on m.Id equals sc.So_Master_ID
                //                      where m.Company_ID == logIn.company && m.Status == 6 && m.Job_Work_Order==true
                //                      select new { m.SO_NO }).Distinct().ToList();
                //    if (Buyerblind.Count > 0)
                //    {
                //        cmbRefDocNo.DataSource = Buyerblind;
                //        cmbRefDocNo.ValueMember = "SO_No";
                //        cmbRefDocNo.DisplayMember = "SO_No";

                //    }
                //    cmbRefDocNo.SelectedIndex = -1;
                //}
                //else
                    if (cmbBasis.Text == "Stock")
                {

                    //if (CmbBuyerName.Items.Count > 0)
                    cmbRefDocNo.DataSource = null;
                    cmbRefDocNo.SelectedIndex = -1;
                    cmbRefDocNo.Text = "NA";
                    txtPending_Qty.Text = "0";
                    txtBalaQty.Text = "0";

                    txt_fg_Item_Code.Enabled = true;
                    dtDelDate.Enabled = true;
                    //var Buyerblind = (from m in db.Forging_Finished_Goods
                    //                   where m.Company_ID == logIn.company
                    //                  select new { m.Prod_Forging_Code, m.prod_ID }).ToList();
                    //if (Buyerblind.Count > 0)
                    //{
                    //    cmb_fg_Item_Code.DataSource = Buyerblind;
                    //    cmb_fg_Item_Code.ValueMember = "prod_ID";
                    //    cmb_fg_Item_Code.DisplayMember = "Prod_Forging_Code";

                    //}
                    //cmb_fg_Item_Code.SelectedIndex = -1;


                    txt_fg_Item_Code.Focus();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbRefDocNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbBasis.Text == "Sale Order" || cmbBasis.Text == "Job Work")
                {
                  // cmb_fg_Item_Code.DataSource = null;
                   var Buyerblind = (from m in db.Get_Items_ToRaise_JobCard(logIn.company,0,cmbRefDocNo.Text, null)                                     
                                      select new { m.SO_NO,m.BuyerName, m.Cust_Item_Code, m.Product_Description,m.RM_Basic_Price, m.Del_Date,m.Qty,m.BalQty }).Distinct().ToList();
                    if (Buyerblind.Count > 0)
                    {
                        //cmb_fg_Item_Code.DataSource = Buyerblind;
                        //cmb_fg_Item_Code.ValueMember = "Cust_Item_Code";
                        //cmb_fg_Item_Code.DisplayMember = "Cust_Item_Code";
                        sfDataGrid1.DataSource = Buyerblind;
                        groupBox2.Visible = true;

                    }
                    else
                    {
                        //cmb_fg_Item_Code.Enabled = true;
                        //dtDelDate.Enabled = true;
                        
                        //cmb_fg_Item_Code.Focus();
                    }
                    //if (CmbBuyerName.Items.Count > 0)
                    //cmb_fg_Item_Code.SelectedIndex = -1;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmb_fg_Item_Code_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbBasis.Text == "Sale Order" || cmbBasis.Text =="Job Work")
                {

                    var Buyerblind = (from m in db.Get_Items_ToRaise_JobCard(logIn.company,1, cmbRefDocNo.Text, txt_fg_Item_Code.Text)
                                      select new { m.Cust_Item_Code,m.Product_Description,m.Del_Date,m.BalQty,m.BuyerName }).Distinct().ToList();
                    if (Buyerblind.Count > 0)
                    {
                        txt_fg_Item_Name.Text = Buyerblind[0].Product_Description;
                        txtPending_Qty.Text = Buyerblind[0].BalQty.ToString();
                        dtDelDate.Text = Buyerblind[0].Del_Date.ToString();
                        CustName = Convert.ToInt32(Buyerblind[0].BuyerName);
                    }                  
                    
                }
                else
                {
                    //var Buyerblind = (from m in db.Forging_Finished_Goods where m.prod_ID == Convert.ToInt32(cmb_fg_Item_Code.SelectedValue)
                    //                  select new { m.Prod_Name,m.prod_ID}).Distinct().ToList();
                    //if (Buyerblind.Count > 0)
                    //{
                    //    txt_fg_Item_Name.Text = Buyerblind[0].Prod_Name;
                    //    txtItemCode.Text = Buyerblind[0].prod_ID.ToString();
                        
                    //}
                }

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

        }

        private void dgRM_Items_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            int columnIndex = dgRM_Items.CurrentCell.ColumnIndex;
            string columnName = dgRM_Items.Columns[columnIndex].HeaderText;
            TextBox tb3 = e.Control as TextBox;
            tb3.AutoCompleteMode = AutoCompleteMode.None;
            tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
            tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;

            AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
            addItems(DataColl);
            tb3.AutoCompleteCustomSource = DataColl;
        }
        
        public void addItems(AutoCompleteStringCollection coll)
        {
            try
            {
                DataGridViewRow R1 = dgRM_Items.Rows[dgRM_Items.CurrentRow.Index];

                int columnIndex = dgRM_Items.CurrentCell.ColumnIndex;
                string columnName = dgRM_Items.Columns[columnIndex].HeaderText;

                if (columnName == "Heat Code")
                {
                    //if (cmbBasis.Text == "Sale Order")
                    //{
                    //    var Prodname = (from d in db.Forge_Get_RM_HeatNo(logIn.company, R1.Cells["Item_code"].Value.ToString(), "Sale Order") select new { d.Heat_No, d.Price }).ToList();
                    //    DataTable dt = new DataTable();
                    //    dt.Columns.Add("Heat_No");
                    //    dt.Columns.Add("Price");
                    //    foreach (var item in Prodname)
                    //    {
                    //        dt.Rows.Add(item.Heat_No);
                    //        dt.Rows.Add(item.Price);
                    //    }
                    //    for (int i = 0; i < dt.Rows.Count; i++)
                    //    {
                    //        coll.Add(dt.Rows[i][0].ToString());
                    //        coll.Add(dt.Rows[i][1].ToString());
                    //    }
                    //}
                    //else if (cmbBasis.Text == "Job Work")
                    //{
                    //    var Prodname = (from d in db.GoodsReceiptNote_Childs
                    //                    join GM in db.GoodsReceiptNote_Masters on d.GRN_Master_ID equals GM.Id 
                    //                    where GM.Purchase_Basis == "For Job Work" && GM.SupplierName == CustName &&  d.Product_Description == R1.Cells["Item_code"].Value.ToString()
                    //                    select new { d.Int_Batch_No, d.Price }).ToList();
                    //    DataTable dt = new DataTable();
                    //    dt.Columns.Add("Heat_No");
                    //    foreach (var item in Prodname)
                    //    {
                    //        dt.Rows.Add(item.Int_Batch_No);
                    //    }
                    //    for (int i = 0; i < dt.Rows.Count; i++)
                    //    {
                    //        coll.Add(dt.Rows[i][0].ToString());
                    //    }
                    //}
                }
                else
                {
                    if (columnName == "Item Description")
                    {
                        var Prodname = (from d in db.Forging_Finished_Goods                                       
                                        where d.Prod_Forging_Code == txt_fg_Item_Code.Text
                                        select new { d.Raw_Material }).ToList();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Raw_Material");
                        foreach (var item in Prodname)
                        {

                            string MP = item.Raw_Material;
                            string[] values = MP.Split(',');
                            for (int j = 0; j < values.Length; j++)
                            {
                                values[j] = values[j].Trim();
                                string m = values[j].ToString();
                                dt.Rows.Add(m);
                            }
                        }
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            coll.Add(dt.Rows[i][0].ToString());
                        }
                        
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgRM_Items_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                
                DataGridViewRow R1 = dgRM_Items.Rows[dgRM_Items.CurrentRow.Index];
                int columnIndex = dgRM_Items.CurrentCell.ColumnIndex;
                string columnName = dgRM_Items.Columns[columnIndex].Name;
               

               if (columnName == "Item_Code" && R1.Cells["Item_Code"].Value != null)
                {

                    var getProductName = (from s in db.Get_ProductsList(logIn.company, 1, R1.Cells["Item_code"].Value.ToString())
                                          select new { s.prod_ID, s.Uom_Descr, s.Prod_Group_Name, s.Prod_HSN_Code, s.Gst_Rate, s.Prod_Customer_Code }).FirstOrDefault();

                    if (getProductName != null)
                    {
                        if (getProductName.Uom_Descr != null)
                        {
                            R1.Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                        }
                        R1.Cells["Item_ID"].Value = getProductName.prod_ID.ToString();
                        //Get Input Wt
                        var fgWt = (from s in db.Forging_Finished_Goods
                                    where s.Prod_Forging_Code == txt_fg_Item_Code.Text
                                    select new { s.Input_Weight }).FirstOrDefault();

                        if (fgWt != null)
                        {
                            if (txtJobCardQty.Text != "")
                            {
                                decimal jQty = Convert.ToDecimal(txtJobCardQty.Text);

                                decimal InputWt = Convert.ToDecimal(fgWt.Input_Weight.ToString());
                                R1.Cells["Qty_Req"].Value = jQty * InputWt;
                            }
                            else
                            {
                                MessageBox.Show("Enter Job Card Qty");
                                txtJobCardQty.Focus();
                            }

                        }

                        var stock = (from data in db.Forge_StockReport_JobCard(logIn.company, Convert.ToInt32(R1.Cells["Item_ID"].Value)) select data).ToList();

                        if (stock.Count > 0)
                        {
                            dataGridView1.DataSource = stock;
                            groupBox1.Visible = true;
                            //R1.Cells["Remarks"].Value = getProductName.Prod_Customer_Code.ToString();
                            //taxRate = Convert.ToDecimal(getProductName.Gst_Rate);
                        }
                    }

                    else
                    {
                        //R1.Cells["Item_Code"].Value = dgProducts.CurrentCell.RowIndex + 1;
                    }
                    
                    
                }
                if (columnName == "Heat_Code")
                {

                    //Get Stock Report
                    //DateTime t = dtJDate.Value;
                    //string f1 = t.ToString("dd/MMM/yyyy");
                    ////DataGridViewRow R2 = dgRM_Items.Rows[dgRM_Items.CurrentRow.Index];
                    //if (R1.Cells["Heat_Code"].Value.ToString() != "")
                    //{
                    //    string heatno = Mid(R1.Cells["Heat_Code"].Value.ToString(), 1, 4);
                    //    //var stock = (from data in db.Forge_StockReport_JobCard(logIn.company, Convert.ToInt32(R1.Cells["Item_ID"].Value), R1.Cells["Heat_Code"].Value.ToString(), t) select data).ToList();

                    //    //if (stock.Count > 0)
                    //    //{
                    //    //    //dgProductsList.DataSource = d;
                    //    //    R1.Cells["Qty_Stock"].Value = stock[0].StkQty;

                    //    //    //                        dgRM_Items.Rows[i].Cells["QtyinStock"].Value = stock[0].ClosingQty;
                    //    //}

                    //}
                }

                if (columnName == "Qty_Allocated")
                {
                    if (Convert.ToDecimal(R1.Cells["Qty_Allocated"].Value) > 0)
                    {
                        //Get Stock Report
                        decimal stkqty = Convert.ToDecimal(R1.Cells["Qty_Stock"].Value);
                        decimal AltQty = Convert.ToDecimal(R1.Cells["Qty_Allocated"].Value);
                        decimal ReqQty = Convert.ToDecimal(R1.Cells["Qty_Req"].Value);
                        if (AltQty > ReqQty)
                        {
                            MessageBox.Show("Alloted Qty Cannot Be More than Required Qty");
                            R1.Cells["Qty_Allocated"].Value = "";
                        }
                        else
                        {
                            if (AltQty > stkqty)
                            {
                                //dgProductsList.DataSource = d;
                                MessageBox.Show("Alloted Qty Cannot Be More than Stock Qty");
                                R1.Cells["Qty_Allocated"].Value = "";
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
        public static string Mid(string s, int a, int b)
        {
            string temp = s.Substring(a - 1, b);
            return temp;
        }
        private void frmForge_JobCard_Load(object sender, EventArgs e)
        {
            try
            {
                txt_fg_Item_Code.AutoCompleteMode = AutoCompleteMode.Suggest;
                txt_fg_Item_Code.AutoCompleteSource = AutoCompleteSource.CustomSource;
                AutoincrementJobCard();
            PopulateTreeViewGroups(0, null);
            //using (SqlCommand cmd = new SqlCommand("SELECT distinct(Process_Name) FROM [Forging_ProcessMaster] where [Main_Process_Id]=4", con))
            //{
            //    cmd.CommandType = CommandType.Text;
            //    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
            //    {
            //        using (DataTable dt = new DataTable())
            //        {
            //            sda.Fill(dt);
            //            for (int i = 0; i < dt.Rows.Count; i++)
            //            {
            //                //lstMainProcess.Items.Add(dt.Rows[i]["Process_Name"].ToString());
            //            }

            //        }
            //    }
            //}

            using (SqlCommand cmd = new SqlCommand("SELECT distinct Process_Name,seq_id FROM [Forging_ProcessMaster] where [Main_Process_Id] ='11' order by seq_id", con))
            {
                cmd.CommandType = CommandType.Text;
                using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                {
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            lstSubProcess.Items.Add(dt.Rows[i]["Process_Name"].ToString());
                        }

                    }
                }
            }
            if (FrmForge_JobCardList.editMode == true)
            {
                bindedit();
                FrmForge_JobCardList.editMode = false;
                
            }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void AutoincrementJobCard()
        {
            try
            {

                var result = db.Sp_autoincrement_JobCard(logIn.company,logIn.fy_Start_Date,logIn.fy_End_Date);
                txtJobCard.Text = result.FirstOrDefault().Jc_no;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }


        private void PopulateTreeViewGroups(int parentId, TreeNode parentNode)
        {
            try
            {
                treeView1.BeginUpdate();
                TreeNode childNode;
                SqlCommand cmd = new SqlCommand("select DISTINCT [Process_Name],id from [Forging_ProcessMaster] WHERE [Company_ID] = @CompID and [Main_Process_ID] =4 and id <>11", con);
                cmd.Parameters.AddWithValue("@CompID", logIn.company);

                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dap.Fill(dt);
                //twAccounts.Nodes.Clear();
                //foreach (DataRow dr in dt.Rows)
                foreach (DataRow dr in dt.Rows)
                {
                    TreeNode t = new TreeNode();
                    t.Text = dr["Process_Name"].ToString();
                    t.Name = dr["ID"].ToString();
                    t.Tag = dt.Rows.IndexOf(dr);
                    if (parentNode == null)
                    {
                        treeView1.Nodes.Add(t);
                        childNode = t;
                    }
                    else
                    {
                        //parentNode.Nodes.Add(t);
                        treeView1.SelectedNode.Nodes.Add((TreeNode)t.Clone());
                        childNode = t;
                    }

                    PopulateTreeView(Convert.ToInt32(dr["id"].ToString()), childNode);
                }
                treeView1.EndUpdate();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void PopulateTreeView(int parentId, TreeNode parentNode)
        //private void PopulateTreeView()
        {
            try
            {
                //Cursor.Current = new Cursor("MyWait.cur");
                //twAccounts.BeginUpdate();
                TreeNode childNode;
                SqlCommand cmd = new SqlCommand("select * from [Forging_ProcessMaster] WHERE [Company_ID] = @CompID and Main_Process_ID =@gid", con);
                cmd.Parameters.AddWithValue("@CompID", logIn.company);
                cmd.Parameters.AddWithValue("@gid", parentId);
                SqlDataAdapter dap = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                dap.Fill(dt);
                //SqlDataAdapter dap = new SqlDataAdapter(cmd);
                //DataTable dt = new DataTable();
                //dap.Fill(dt);
              //  treeView1.SelectedNode.Nodes.Clear();
                //foreach (DataRow dr in dt.Rows)
                foreach (DataRow dr in dt.Rows)
                {
                    TreeNode t = new TreeNode();
                    t.Text = dr["Process_Name"].ToString();
                    t.Name = dr["id"].ToString();
                    t.Tag = dt.Rows.IndexOf(dr);
                    if (parentNode == null)
                    {
                        treeView1.Nodes.Add(t);
                        childNode = t;
                    }
                    else
                    {
                        parentNode.Nodes.Add(t);
                        //treeView1.SelectedNode.Nodes.Add((TreeNode)t.Clone());
                        childNode = t;
                    }

                    //PopulateTreeView(Convert.ToInt32(dr["id"].ToString()), childNode);
                }
                //twAccounts.EndUpdate();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        private void txtJobCardQty_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtJobCardQty.Text != "")
                {
                    if (cmbBasis.Text != "Stock")
                    {
                        decimal JQty = Convert.ToDecimal(txtJobCardQty.Text);
                        decimal tQty = 0;
                        var fgWt = (from s in db.Sale_Order_Childs
                                    where s.SO_NO == cmbRefDocNo.Text && s.Prod_Code == txtItemCode.Text
                                    select new { s.Tole_Qty }).FirstOrDefault();

                        if (fgWt != null)
                        {

                            tQty = Convert.ToDecimal(fgWt.Tole_Qty.ToString());



                        }
                        decimal PQty = Convert.ToDecimal(txtPending_Qty.Text);
                        decimal APQty = PQty + tQty;
                        decimal BQty = PQty - JQty;
                        if (JQty > APQty)
                        {
                            MessageBox.Show("Job Card Qty Should Not Be Greater Than Pending Qty");
                            txtJobCardQty.Focus();
                        }
                        txtBalaQty.Text = BQty.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static string ProcessSelected = "";
        private void btnSave_Click(object sender, EventArgs e)
        {


            try
            {
                
                for (int i = 0; i < dgRM_Items.RowCount - 1; i++)
                {
                    if (dgRM_Items.Rows[i].Cells["Item_Code"].Value != null)
                    {
                        if (Convert.ToDecimal(dgRM_Items.Rows[i].Cells["Qty_Allocated"].Value) <= 0)
                        {
                            MessageBox.Show("Job Card Cannot Be Saved Without Rawmaterial Allotment");
                            return;
                        }

                    }
                }


                if (cmbRefDocNo.Text == string.Empty)
                {
                    MessageBox.Show("Select Ref Doc No to Proceed", "Job Card", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbRefDocNo.Focus();
                    return;
                }
                else if (txt_fg_Item_Code.Text == string.Empty)
                {
                    MessageBox.Show("Select Item To Generate Job Card", "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    txt_fg_Item_Code.Focus();
                    return;
                }
                else if (txtJobCardQty.Text == string.Empty)
                {
                    MessageBox.Show("Job Card Qty Cannot be Null or Zero");
                    txtJobCardQty.Focus();
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

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                OpenFileDialog open = new OpenFileDialog();
                if (open.ShowDialog() == DialogResult.OK)
                {
                    Image img = new Bitmap(open.FileName);
                    //pictureBox1.Image = img;

                }

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
                myString = txtJobCard.Text;
                if ((from u in db.Forging_JobCards where u.Job_CardNo == myString && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    myString = txtJobCard.Text;
                    db.sp_JobCard_Delete(myString, logIn.company);
                }
                else
                {
                    AutoincrementJobCard();
                    myString = txtJobCard.Text;

                }
                //if (frmGate.Modify.Contains(this.Text))
                //{

                //dgProducts.Enabled = false;
                //var S = db.Sale_Order_Masters.Where(w => w.SO_NO == myString && w.Company_ID == logIn.company).FirstOrDefault();
                Forging_JobCard S = new Forging_JobCard();
                {
                    S.Job_CardNo = myString;
                    S.J_Date = dtJDate.Value;
                    S.Basis = cmbBasis.Text;
                    S.Ref_Doc_No = cmbRefDocNo.Text;                  
                    S.FG_Item_Code = txt_fg_Item_Code.Text;                    
                    S.FG_Item_Name = txt_fg_Item_Name.Text;                  
                    S.Pending_qty = (txtPending_Qty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtPending_Qty.Text);
                    S.Job_CardQty = (txtJobCardQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtJobCardQty.Text);
                    S.Balance_Qty = (txtBalaQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtBalaQty.Text);
                    S.Del_Date = dtDelDate.Value;                  
                    // Get the checked nodes.
                    List<TreeNode> checked_nodes = CheckedNodes(treeView1);
                    
                    S.Sub_Process_Involved = ProcessSelected;
                    ProcessSelected = "";

                    string MachineProcess = "";
                    for (int i = 0; i < lstSubProcess.Items.Count; i++)
                    {
                        if (lstSubProcess.GetItemChecked(i))

                        {
                        if (MachineProcess != "")
                        {

                                MachineProcess = MachineProcess + "," + lstSubProcess.Items[i].ToString();
                                
                        }
                        else
                        {

                                MachineProcess = lstSubProcess.Items[i].ToString();
                        }
                        }
                    }
                    S.Main_Process_Involved = MachineProcess;
                    S.RM_Basic_Price = (txtRMPrice.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtRMPrice.Text);
                    S.FG__Code = Convert.ToInt32(txtItemCode.Text);
                    S.Company_ID = logIn.company;
                    S.Created_By = lblCreatedBy.Text;
                    S.Modified_By = logIn.username + "-" + DateTime.Now;
                    db.Forging_JobCards.InsertOnSubmit(S);
                    db.SubmitChanges();
                }
                //db.Transaction = transaction;
                for (int i = 0; i < dgRM_Items.RowCount - 1; i++)
                {
                    Forging_JobCardRM SC = new Forging_JobCardRM();
                    var d1 = (from a in db.Forging_JobCards where a.Job_CardNo == myString && a.Company_ID == logIn.company select new { a.id }).ToList();
                    SC.JobCard_ID = d1[0].id;
                    SC.Job_CardNo = myString;
                    SC.RM_Code = Convert.ToInt32(dgRM_Items.Rows[i].Cells["Item_ID"].Value);
                    SC.RM_Name = (dgRM_Items.Rows[i].Cells["Item_Code"].Value == null) ? "" : (dgRM_Items.Rows[i].Cells["Item_Code"].Value).ToString();
                    SC.RM_Spec = (dgRM_Items.Rows[i].Cells["Item_Spec"].Value == null) ? "" : (dgRM_Items.Rows[i].Cells["Item_Spec"].Value).ToString();
                    SC.RM_Uom = (dgRM_Items.Rows[i].Cells["UOM"].Value == null) ? "" : dgRM_Items.Rows[i].Cells["UOM"].Value.ToString();
                    SC.Heat_Code = (dgRM_Items.Rows[i].Cells["Heat_Code"].Value == null) ? "" : dgRM_Items.Rows[i].Cells["Heat_Code"].Value.ToString();
                    //SC.Qty = (dgProducts.Rows[i].Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty"].Value);
                    SC.Qty_Req_MT = (dgRM_Items.Rows[i].Cells["Qty_Req"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgRM_Items.Rows[i].Cells["Qty_Req"].Value);

                    SC.Qty_Stock = (dgRM_Items.Rows[i].Cells["Qty_Stock"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgRM_Items.Rows[i].Cells["Qty_Stock"].Value);
                    SC.Qty_Alloted = (dgRM_Items.Rows[i].Cells["Qty_Allocated"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgRM_Items.Rows[i].Cells["Qty_Allocated"].Value);
                    SC.RM_Price = (dgRM_Items.Rows[i].Cells["RM_Basic_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgRM_Items.Rows[i].Cells["RM_Basic_Price"].Value);
                    db.Forging_JobCardRMs.InsertOnSubmit(SC);
                }
                db.SubmitChanges();
                //transaction.Commit();  

                //Saving Machine Planning;
                for (int i = 0; i < dgMachinePlan.RowCount; i++)
                {
                    Forging_JobCard_MachiningPlan SC = new Forging_JobCard_MachiningPlan();
                    var d1 = (from a in db.Forging_JobCards where a.Job_CardNo == myString && a.Company_ID == logIn.company select new { a.id }).ToList();
                    SC.JobCard_ID = d1[0].id;
                    SC.Job_CardNo = myString;
                    SC.Machine_Code = (dgMachinePlan.Rows[i].Cells["Prod_Machine_Code"].Value == null) ? "" : (dgMachinePlan.Rows[i].Cells["Prod_Machine_Code"].Value).ToString();
                    SC.Machine_Drawing_No = (dgMachinePlan.Rows[i].Cells["Machining_Drawing_No"].Value == null) ? "" : (dgMachinePlan.Rows[i].Cells["Machining_Drawing_No"].Value).ToString();
                    SC.Machine_Qty = (dgMachinePlan.Rows[i].Cells["Machining_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgMachinePlan.Rows[i].Cells["Machining_Qty"].Value);
                    db.Forging_JobCard_MachiningPlans.InsertOnSubmit(SC);
                }
                db.SubmitChanges();
                MessageBox.Show("Record Saved / Updated Successfully With Transaction Ref No : " + txtJobCard.Text);
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }
        // Return a list of the TreeNodes that are checked.
        private void FindCheckedNodes(
            List<TreeNode> checked_nodes, TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                // Add this node.
                if (node.Checked)
                {
                    checked_nodes.Add(node);
                    if (ProcessSelected == "")
                    {
                        ProcessSelected = node.Text;
                    }
                    else
                    {
                        ProcessSelected = ProcessSelected + "," + node.Text;
                    }
                }

                // Check the node's descendants.
                FindCheckedNodes(checked_nodes, node.Nodes);
            }
        }

        // Return a list of the checked TreeView nodes.
        private List<TreeNode> CheckedNodes(TreeView trv)
        {
            List<TreeNode> checked_nodes = new List<TreeNode>();
            FindCheckedNodes(checked_nodes, treeView1.Nodes);
            return checked_nodes;
        }
        public void bindedit()
        {
            try
            {
               // DateTime dtDel;
                txtJobCard.Text = ProductionManagement.Transactions.FrmForge_JobCardList.SO_No;
                String myString = "";
                myString = txtJobCard.Text;
                var da = (from obj in db.Forging_JobCards
                          where obj.Job_CardNo == txtJobCard.Text && obj.Company_ID == logIn.company
                          select obj).ToList();

                if (da.Count > 0)
                {
                    txtJobCard.Text = da[0].Job_CardNo.ToString();
                    dtJDate.Text = da[0].J_Date.ToString();
                    //bindCustomer();
                    cmbBasis.Text = da[0].Basis;
                    
                    cmbRefDocNo.Text = da[0].Ref_Doc_No;
                    txt_fg_Item_Code.Text = da[0].FG_Item_Code;
                    txt_fg_Item_Name.Text = da[0].FG_Item_Name;
                    txtPending_Qty.Text = da[0].Pending_qty.ToString();
                    txtJobCardQty.Text = da[0].Job_CardQty.ToString();
                    txtBalaQty.Text = da[0].Balance_Qty.ToString();
                    DateTime dtDel = Convert.ToDateTime(da[0].Del_Date);
                    dtDelDate.Value = dtDel; // da[0].Del_Date.ToString();
                    txtRMPrice.Text = da[0].RM_Basic_Price.ToString();
                    if (da[0].Remarks != null)
                    {
                        txtRemarks.Text = da[0].Remarks.ToString();
                    }
                    //if (da[0].Job_Image != null)
                    //{
                    //    var f = (from s in db.Forging_JobCards where s.Job_CardNo == txtJobCard.Text select s);
                    //    SqlCommand cmd = (SqlCommand)db.GetCommand(f);
                    //    SqlDataAdapter da1 = new SqlDataAdapter(cmd);
                    //    DataTable dt = new DataTable();
                    //    DataSet ds = new DataSet("MyImages");
                    //    byte[] MyData = new byte[0];
                    //    da1.Fill(ds, "MyImages");
                    //    DataRow myRow;
                    //    myRow = ds.Tables["MyImages"].Rows[0];
                    //    MyData = (byte[])myRow["Job_Image"];
                    //    MemoryStream stream = new MemoryStream(MyData);
                    //    //pictureBox1.Image = Image.FromStream(stream);


                    //}
                    if (da[0].Main_Process_Involved != null)
                    {
                        string MP = da[0].Main_Process_Involved.ToString();
                        string[] values = MP.Split(',');
                        for (int j = 0; j < values.Length; j++)
                        {
                            values[j] = values[j].Trim();
                            string m = values[j].ToString();
                            for (int i = 0; i < lstSubProcess.Items.Count; i++)
                            {
                                if (lstSubProcess.Items[i].ToString() == m)
                                {
                                    lstSubProcess.SetItemChecked(i, true);
                                }
                            }
                        }
                    }
                    if (da[0].Sub_Process_Involved != null)
                    {
                        string s1 = da[0].Sub_Process_Involved.ToString();
                        string[] values1 = s1.Split(',');
                        for (int j = 0; j < values1.Length; j++)
                        {
                            values1[j] = values1[j].Trim();
                            string m = values1[j].ToString();

                            for (int i = 0; i < treeView1.Nodes.Count; i++)
                            {

                                if (treeView1.Nodes[i].Text == m)
                                {
                                    treeView1.Nodes[i].Checked = true;
                                }
                                else
                                {
                                    foreach (TreeNode tn in treeView1.Nodes[i].Nodes)
                                    {
                                        if (tn.Text == m)
                                        {
                                            tn.Checked = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    lblCreatedBy.Text = da[0].Created_By;
                    lblModified.Text = da[0].Modified_By;
                    txtItemCode.Text = da[0].FG__Code.ToString();
                }


                var dm1 = (from s in db.Forging_JobCardRMs
                           where s.Job_CardNo == myString 


                           select new

                           {
                               Item_ID = s.RM_Code,
                               Item_Code= s.RM_Name,
                               Item_Spec = s.RM_Spec,
                               UOM = s.RM_Uom,
                               Heat_Code=s.Heat_Code,
                               RM_Basic_Price = s.RM_Price,
                               Qty_Req = s.Qty_Req_MT,
                               Qty_Stock = s.Qty_Stock,

                               Qty_Allocated = s.Qty_Alloted,                             


                           });




                SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataTable dtr = new DataTable();
                da2.Fill(dtr);
                if (dtr.Rows.Count >= 0)
                    dgRM_Items.DataSource = dtr;


                var dm2 = (from s in db.Forging_JobCard_MachiningPlans
                           where s.Job_CardNo == myString


                           select new

                           {
                               Machining_Drawing_No= s.Machine_Drawing_No,
                               Prod_Machine_Code=  s.Machine_Code,
                               Machining_Qty= s.Machine_Qty
                               


                           });




                SqlCommand cmd3 = (SqlCommand)db.GetCommand(dm2);
                SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                DataTable dtr1 = new DataTable();
                da3.Fill(dtr1);
                if (dtr1.Rows.Count >= 0)
                    dgMachinePlan.DataSource = dtr1;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dtDelDate_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                if (cmbBasis.Text == "Sale Order" || cmbBasis.Text == "Job Work")
                {

                    var Buyerblind = (from m in db.Get_Items_ToRaise_JobCard_Based_On_Del_Date(logIn.company, cmbRefDocNo.Text, txt_fg_Item_Code.Text,dtDelDate.Value)
                                      select new { m.Cust_Item_Code, m.Product_Description, m.Del_Date, m.BalQty, m.BuyerName }).Distinct().ToList();
                    if (Buyerblind.Count > 0)
                    {
                        //txt_fg_Item_Name.Text = Buyerblind[0].Product_Description;
                        txtPending_Qty.Text = Buyerblind[0].BalQty.ToString();
                        //dtDelDate.Text = Buyerblind[0].Del_Date.ToString();
                        //CustName = Convert.ToInt32(Buyerblind[0].BuyerName);
                    }

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancelItemSelection_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                if (e.RowIndex >= 0)
                {
                    //cmb_fg_Item_Code.Text = sfDataGrid1.Rows[e.RowIndex].Cells["Cust_Item_Code"].Value.ToString();
                    //txt_fg_Item_Name.Text = sfDataGrid1.Rows[e.RowIndex].Cells["Product_Description"].Value.ToString();
                    //txtPending_Qty.Text = sfDataGrid1.Rows[e.RowIndex].Cells["BalQty"].Value.ToString();
                    //dtDelDate.Text = sfDataGrid1.Rows[e.RowIndex].Cells["Del_Date"].Value.ToString();
                    var Buyerblind = (from m in db.Get_Items_ToRaise_JobCard(logIn.company, 1, cmbRefDocNo.Text, txt_fg_Item_Code.Text)
                                      select new { m.Cust_Item_Code, m.Product_Description, m.Del_Date, m.BalQty, m.BuyerName }).Distinct().ToList();
                    if (Buyerblind.Count > 0)
                    {
                        //txt_fg_Item_Name.Text = Buyerblind[0].Product_Description;
                        //txtPending_Qty.Text = Buyerblind[0].BalQty.ToString();
                        //dtDelDate.Text = Buyerblind[0].Del_Date.ToString();
                        CustName = Convert.ToInt32(Buyerblind[0].BuyerName);
                    }

                    groupBox2.Visible = false;

                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

            // cmb_fg_Item_Code.DataSource = null;
            if (cmbBasis.Text == "Sale Order")
            {
                var Buyerblind = (from m in db.Get_Orders_ToRaise_JobCard(logIn.company, cmbBasis.Text)
                                  select m).ToList();
                if (Buyerblind.Count > 0)
                {
                    //cmb_fg_Item_Code.DataSource = Buyerblind;
                    //cmb_fg_Item_Code.ValueMember = "Cust_Item_Code";
                    //cmb_fg_Item_Code.DisplayMember = "Cust_Item_Code";
                    sfDataGrid1.DataSource = Buyerblind;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["Cust_Item_Code"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Cust_Item_Code"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Cust_Item_Code"].ImmediateUpdateColumnFilter = true;

                    this.sfDataGrid1.Columns["Customer_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Customer_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Customer_Name"].ImmediateUpdateColumnFilter = true;

                    this.sfDataGrid1.Columns["SO_NO"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["SO_NO"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["SO_NO"].ImmediateUpdateColumnFilter = true;

                    this.sfDataGrid1.Columns["RM_Basic_Price"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["RM_Basic_Price"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["RM_Basic_Price"].ImmediateUpdateColumnFilter = true;

                    groupBox2.Visible = true;

                }
                else
                {
                    MessageBox.Show("No Records Found");
                }
            }
            else if (cmbBasis.Text =="Planning")
            {
                var Buyerblind = (from m in db.Get_Orders_Planning_ToRaise_JobCard(logIn.company)
                                  select m).ToList();
                if (Buyerblind.Count > 0)
                {
                    //cmb_fg_Item_Code.DataSource = Buyerblind;
                    //cmb_fg_Item_Code.ValueMember = "Cust_Item_Code";
                    //cmb_fg_Item_Code.DisplayMember = "Cust_Item_Code";
                    sfDataGrid1.DataSource = Buyerblind;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["Cust_Item_Code"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Cust_Item_Code"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Cust_Item_Code"].ImmediateUpdateColumnFilter = true;

                    this.sfDataGrid1.Columns["Customer_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Customer_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Customer_Name"].ImmediateUpdateColumnFilter = true;

                    this.sfDataGrid1.Columns["SO_NO"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["SO_NO"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["SO_NO"].ImmediateUpdateColumnFilter = true;

                    this.sfDataGrid1.Columns["RM_Basic_Price"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["RM_Basic_Price"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["RM_Basic_Price"].ImmediateUpdateColumnFilter = true;

                    groupBox2.Visible = true;

                }
                else
                {
                    MessageBox.Show("No Records Found");
                }
            }
        }

        private void sfDataGrid1_CellDoubleClick(object sender, Syncfusion.WinForms.DataGrid.Events.CellClickEventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["SO_NO"].MappingName;
                var mappingName2 = sfDataGrid1.Columns["Cust_Item_Code"].MappingName;
                var mappingName3 = sfDataGrid1.Columns["Customer_Name"].MappingName;
                var mappingName4 = sfDataGrid1.Columns["RM_Basic_Price"].MappingName;
                var mappingName5 = sfDataGrid1.Columns["BalQty"].MappingName;
                var mappingName6 = sfDataGrid1.Columns["Product_Description"].MappingName;
                var mappingName7 = sfDataGrid1.Columns["del_date"].MappingName;
                var mappingName8 = sfDataGrid1.Columns["prod_ID"].MappingName;

                var SO_NO = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                var Cust_Item_Code = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
                var BuyerName = (rowData.GetType().GetProperty(mappingName3).GetValue(rowData, null).ToString());
                var RM_Basic_Price = (rowData.GetType().GetProperty(mappingName4).GetValue(rowData, null).ToString());
                var BalQty = (rowData.GetType().GetProperty(mappingName5).GetValue(rowData, null).ToString());
                var Product_Description = (rowData.GetType().GetProperty(mappingName6).GetValue(rowData, null).ToString());
                var Del_Date = (rowData.GetType().GetProperty(mappingName7).GetValue(rowData, null).ToString());
                var prod_ID = (rowData.GetType().GetProperty(mappingName8).GetValue(rowData, null).ToString());
                txt_fg_Item_Code.Text = Cust_Item_Code.ToString();
                txt_fg_Item_Name.Text = Product_Description.ToString();
                txtPending_Qty.Text = BalQty.ToString();
                dtDelDate.Text = Del_Date.ToString();
                cmbRefDocNo.Text = SO_NO.ToString();
                txtRMPrice.Text = RM_Basic_Price.ToString();
                txtItemCode.Text = prod_ID.ToString();
                //Get Machine Plan
                var Buyerblind = (from m in db.Forge_Get_MachinePlan(logIn.company, txt_fg_Item_Code.Text, txtJobCard.Text)
                                  select m).ToList();
                if (Buyerblind.Count > 0)
                {
                  
                    dgMachinePlan.DataSource = Buyerblind;                   

                }
                groupBox2.Visible = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow R1 = dgRM_Items.Rows[dgRM_Items.CurrentRow.Index];
            int columnIndex = dgRM_Items.CurrentCell.ColumnIndex;
            string columnName = dgRM_Items.Columns[columnIndex].Name;
            R1.Cells["Heat_Code"].Value = dataGridView1.Rows[e.RowIndex].Cells["Heat_No"].Value.ToString();
            R1.Cells["RM_Basic_Price"].Value = dataGridView1.Rows[e.RowIndex].Cells["Basic_Price"].Value.ToString();
            R1.Cells["Qty_Stock"].Value = dataGridView1.Rows[e.RowIndex].Cells["StkQty"].Value.ToString();

            decimal QtyReq = Convert.ToDecimal(R1.Cells["Qty_Req"].Value);
            decimal QtyStock = Convert.ToDecimal(R1.Cells["Qty_Stock"].Value);
            if (QtyReq >= QtyStock)
            {
                MessageBox.Show("Required Qty Not Available In The Stock");
                R1.Cells["Qty_Allocated"].Value = "0";
            }
            else if (QtyStock >QtyReq)
            {
                R1.Cells["Qty_Allocated"].Value = QtyReq;
            }
            groupBox1.Visible = false;
        }

        private void dgMachinePlan_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow R1 = dgMachinePlan.Rows[dgMachinePlan.CurrentRow.Index];
            int columnIndex = dgMachinePlan.CurrentCell.ColumnIndex;
            string columnName = dgMachinePlan.Columns[columnIndex].Name;


            if (columnName == "Machining_Drawing_No")
            {

                var Prodname = (from m in db.Forging_Finished_Goods
                                where m.Company_ID == logIn.company && m.Machining_Drawing_No == R1.Cells["Machining_Drawing_No"].Value.ToString()
                                select new { m.Prod_Machine_Code, m.prod_ID }).FirstOrDefault();
                if (Prodname != null)
                {
                    R1.Cells["Prod_Machine_Code"].Value = Prodname.Prod_Machine_Code.ToString();
                }

            }
            else if (columnName == "Prod_Machine_Code")
            {

                var Prodname = (from m in db.Forging_Finished_Goods
                                where m.Company_ID == logIn.company && m.Prod_Machine_Code == R1.Cells["Prod_Machine_Code"].Value.ToString()
                                select new { m.Machining_Drawing_No, m.prod_ID }).FirstOrDefault();
                if (Prodname != null)
                {
                    R1.Cells["Machining_Drawing_No"].Value = Prodname.Machining_Drawing_No.ToString();
                }

            }
            else
            {
                decimal jQty = Convert.ToDecimal(txtJobCardQty.Text);
                decimal mQty = 0;
                for (int i = 0; i < dgMachinePlan.Rows.Count; i++)
                {

                    mQty += (dgMachinePlan.Rows[i].Cells["Machining_Qty"].Value == "" || dgMachinePlan.Rows[i].Cells["Machining_Qty"].Value == null || dgMachinePlan.Rows[i].Cells["Machining_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgMachinePlan.Rows[i].Cells["Machining_Qty"].Value);

                }
                if (mQty > jQty)
                {
                    MessageBox.Show("Total Machining Qty Cannot Exceed Job Card Qty");
                    R1.Cells["Machining_Qty"].Value = "0";
                    return;
                }
            }
        }

        private void txtJobCardQty_TextChanged(object sender, EventArgs e)
        {

        }

        private void txt_fg_Item_Code_Enter(object sender, EventArgs e)
        {
            try
            {

                AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                AddItems(DataColl);
                txt_fg_Item_Code.AutoCompleteCustomSource = DataColl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void AddItems(AutoCompleteStringCollection coll)
        {
            try
            {

                var Prodname = (from m in db.Forging_Finished_Goods
                                  where m.Company_ID == logIn.company
                                  select new { m.Prod_Forging_Code, m.prod_ID }).ToList();

                DataTable dt = new DataTable();
                dt.Columns.Add("Prod_Forging_Code");
                foreach (var item in Prodname)
                {
                    dt.Rows.Add(item.Prod_Forging_Code);
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

        private void txt_fg_Item_Code_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txt_fg_Item_Code.Text != "")
                {
                    var Buyerblind = (from m in db.Forging_Finished_Goods
                                      where m.Prod_Forging_Code == txt_fg_Item_Code.Text
                                      select new { m.Prod_Name, m.prod_ID }).Distinct().ToList();
                    if (Buyerblind.Count > 0)
                    {
                        txt_fg_Item_Name.Text = Buyerblind[0].Prod_Name;
                        txtItemCode.Text = Buyerblind[0].prod_ID.ToString();

                    }

                    //Get Machine Plan
                    var MachineBind = (from m in db.Forge_Get_MachinePlan(logIn.company, txt_fg_Item_Code.Text, txtJobCard.Text)
                                      select m).ToList();
                    if (MachineBind.Count > 0)
                    {

                        dgMachinePlan.DataSource = MachineBind;

                    }
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

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgMachinePlan_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            int columnIndex = dgMachinePlan.CurrentCell.ColumnIndex;
            string columnName = dgMachinePlan.Columns[columnIndex].HeaderText;
            if (columnName == "Machine Drawing No")
            {
                TextBox tb3 = e.Control as TextBox;
                tb3.AutoCompleteMode = AutoCompleteMode.None;
                tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;

                AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                addDrawings(DataColl);
                tb3.AutoCompleteCustomSource = DataColl;
            }
            if (columnName == "Machine Code")
            {
                TextBox tb3 = e.Control as TextBox;
                tb3.AutoCompleteMode = AutoCompleteMode.None;
                tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;

                AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                addDrawings(DataColl);
                tb3.AutoCompleteCustomSource = DataColl;
            }
        }

        public void addDrawings(AutoCompleteStringCollection coll)
        {
            try
            {
                int columnIndex = dgMachinePlan.CurrentCell.ColumnIndex;
                string columnName = dgMachinePlan.Columns[columnIndex].HeaderText;

                if (columnName == "Machine Drawing No")
                {
                  
                    var Prodname = (from m in db.Forging_Finished_Goods
                                    where m.Company_ID == logIn.company && m.prod_ID == Convert.ToInt32(txtItemCode.Text)
                                    select new { m.Machining_Drawing_No, m.prod_ID }).ToList();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Machining_Drawing_No");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Machining_Drawing_No);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
                    }
                }
                if (columnName == "Machine Code")
                {
                    var Prodname = (from m in db.Forging_Finished_Goods
                                    where m.Company_ID == logIn.company && m.prod_ID == Convert.ToInt32(txtItemCode.Text)
                                    select new { m.Prod_Machine_Code, m.prod_ID }).ToList();

                    DataTable dt = new DataTable();
                    dt.Columns.Add("Prod_Machine_Code");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Prod_Machine_Code);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
                    }
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
    }
}
