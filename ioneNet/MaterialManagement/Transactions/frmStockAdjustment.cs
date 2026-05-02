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
    public partial class frmStockAdjustment : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string DocNo, ItemCode, RecQty, Suppname;
        public frmStockAdjustment()
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
            //bindDept();
            AutoincrementId();
        }

        public void AutoincrementId()
        {
            try
            {

                var result = db.Sp_autoincrement_StockAdjusment(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, logIn.BU_ID);
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
                //bindDept();
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
                        R1.Cells["Prod_Code"].Value ="NA";
                    }
                    DateTime t = dpSODate.Value;                   
                    R1.Cells["Stock_Qty"].Value = 0;
                    R1.Cells["Price"].Value = 0;
                    string dt1 = t.ToString("yyyy/MM/dd");
                    var stock = (from data in db.ShowItemWiseStockReport_New(logIn.company, Convert.ToInt32(R1.Cells["Item_code"].Value.ToString()), Convert.ToDateTime(dt1), logIn.BU_ID) select data).ToList();

                    if (stock.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        R1.Cells["Stock_Qty"].Value = stock[0].ClosingQty;                      
                        R1.Cells["Price"].Value = stock[0].CBPrice;
                        //R1.Cells["Price"].Value = stock[0].CBPrice;
                    }
                }

                if (columnName == "Stock_Out" || columnName == "Stock_In")
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
                if (columnName == "Price")
                {
                    int Itemcode = Convert.ToInt32(R1.Cells["Item_Code"].Value.ToString());
                    if (R1.Cells["Item_Description"].Value != null)
                    {
                        decimal IssuedQty = (R1.Cells["Stock_Out"].Value == "" || R1.Cells["Stock_Out"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Stock_Out"].Value);
                        decimal StockQty = (R1.Cells["Stock_In"].Value == "" || R1.Cells["Stock_In"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Stock_In"].Value);
                        decimal price = (R1.Cells["Price"].Value == "" || R1.Cells["Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Price"].Value);

                        //if (IssuedQty <= StockQty)
                        //{
                            decimal Amt, DiscAmt, netAmt, gst, igst, totamt;

                            Amt = StockQty * price;
                            R1.Cells["Amount"].Value = Amt.ToString("0.00");
                        //}
                        //else
                        //{
                        //    MessageBox.Show("Stock Out Qty Cannot Be Greater Than Stock Qty");
                        //    R1.Cells["Stock_Out"].Value = 0;
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
                if ((from u in db.StockAdjustments where u.Slip_NO == myString && u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID select u).Count() > 0)
                {
                    myString = txtSlipNo.Text;
                    db.sp_Delete_StockAdjustment(myString, logIn.company,logIn.BU_ID);
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

                myString = txtSlipNo.Text;
                SqlCommand cmd = new SqlCommand("SaveStockAdjustment", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Slip_NO", myString);
                cmd.Parameters.AddWithValue("@Slip_Date", dpSODate.Value);
                cmd.Parameters.AddWithValue("@BU_ID", logIn.BU_ID);
                cmd.Parameters.AddWithValue("@Company_ID", logIn.company);
                cmd.Parameters.AddWithValue("@Created_By", lblCreatedBy.Text);
                cmd.Parameters.AddWithValue("@Modified_By", logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt"));

                string Prod_Code = "";              
                string Uom = "";
                string Stock_In = "";
                string Stock_Qty = "";
                string Stock_Out = "";
                string Issue_Price = "";
                string Issue_Value = "";               
                string Remarks = "";
                string ProdSno = "";
                int rowcount = 0;
                int PSno = 0;

                for (int i = 0; i < dgProducts.RowCount - 1; i++)
                {
                    Prod_Code = Prod_Code + Convert.ToString(dgProducts.Rows[i].Cells["Item_Code"].Value).PadRight(14);
                    //Product_Description = Product_Description + Convert.ToString(dgProducts.Rows[i].Cells["Item_Description"].Value).PadRight(50); 
                    Uom = Uom + Convert.ToString(dgProducts.Rows[i].Cells["uom"].Value).PadRight(14);

                    Stock_In = Stock_In + Convert.ToString((dgProducts.Rows[i].Cells["Stock_In"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Stock_In"].Value)).PadRight(14);
                    Stock_Qty = Stock_Qty + Convert.ToString(dgProducts.Rows[i].Cells["Stock_Qty"].Value).PadRight(14);
                    Stock_Out = Stock_Out + Convert.ToString((dgProducts.Rows[i].Cells["Stock_Out"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Stock_Out"].Value)).PadRight(14);
                    Issue_Price = Issue_Price + Convert.ToString(dgProducts.Rows[i].Cells["Price"].Value).PadRight(14);
                    Issue_Value = Issue_Value + Convert.ToString(dgProducts.Rows[i].Cells["Amount"].Value).PadRight(14);
                     Remarks = Remarks + Convert.ToString(dgProducts.Rows[i].Cells["Remarks"].Value).PadRight(50);
                    //PSno = PSno +Convert.ToInt32( dgProducts.Rows[i].Cells["S_No"].Value);
                    if (PSno == 0)
                    {
                        ProdSno = ProdSno + Convert.ToString(i + 1).PadRight(14);
                        PSno = i + 1;
                    }
                    else
                    {
                        ProdSno = ProdSno + Convert.ToString(PSno + 1).PadRight(14);
                        PSno = PSno + 1;
                    }
                   


                   
                    rowcount += 1;
                }
                cmd.Parameters.AddWithValue("@txt_Prod_Code", Prod_Code);               ;
                cmd.Parameters.AddWithValue("@txt_Uom", Uom);
                cmd.Parameters.AddWithValue("@txt_Stock_In", Stock_In);
                cmd.Parameters.AddWithValue("@txt_Stock_Qty", Stock_Qty);
                cmd.Parameters.AddWithValue("@txt_Stock_Out", Stock_Out);
                cmd.Parameters.AddWithValue("@txt_Issue_Price", Issue_Price);
                cmd.Parameters.AddWithValue("@txt_Issue_Value", Issue_Value);              
                cmd.Parameters.AddWithValue("@txt_Remarks", Remarks);               
                cmd.Parameters.AddWithValue("@txt_Prod_SNO", ProdSno);
                cmd.Parameters.AddWithValue("@gridcount", rowcount);

                try
                {
                    con.Open();
                    int result = cmd.ExecuteNonQuery();

                    if (Convert.ToBoolean(result))
                    {
                        MessageBox.Show("Record has been successfully saved..");
                        clear();
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

                //MessageBox.Show("Record Saved / Updated Successfully With Transaction Ref No : " + txtSlipNo.Text);
               
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
                //ioneNet.MaterialManagement.Transactions.frmSelectRolls form = new ioneNet.MaterialManagement.Transactions.frmSelectRolls();
                ////ioneNet.Masters.ProdSearch.frmName = "SOrder";       
                //int i = dgProducts.CurrentCell.RowIndex;
                //DocNo = txtSlipNo.Text;
                //ItemCode = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                ////RecQty = dgProducts.Rows[i].Cells["ReceivedQty"].Value.ToString();
                //form.ShowDialog();
                //if (ioneNet.MaterialManagement.Transactions.frmSelectRolls.TotQty > 0)
                //{
                //    dgProducts.Rows[i].Cells["Issue_Qty"].Value = ioneNet.MaterialManagement.Transactions.frmSelectRolls.TotQty;
                //}
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
                    if ((from a in db.StockAdjustments where a.Company_ID == logIn.company && a.Slip_NO == txtSlipNo.Text select a).Count() > 0)
                    {
                        db.sp_Delete_StockAdjustment(txtSlipNo.Text,logIn.company,logIn.BU_ID);
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

        private void button1_Click(object sender, EventArgs e)
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

            string SheetName = "Sheet5";
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
            System.Data.DataRow dr = null;
            dt.Columns.Add(new DataColumn("Prod_Code", typeof(string)));
            dt.Columns.Add(new DataColumn("Stock_In", typeof(string)));
            dt.Columns.Add(new DataColumn("Stock_Out", typeof(string)));
            dt.Columns.Add(new DataColumn("Issue_Price", typeof(string)));

            for (int i = 0; i < count; i++)
            {
                dr = dt.NewRow();

                    dr["Prod_Code"] = DtSet.Rows[i]["Prod_Code"].ToString();
                    //dr["Prod_Name"] = DtSet.Rows[i]["Prod_Name"].ToString();
                    //  dr["UOM"] = DtSet.Rows[i]["UOM"].ToString();
                    dr["Stock_In"] = DtSet.Rows[i]["Stock_In"].ToString();
                    dr["Stock_Out"] = DtSet.Rows[i]["Stock_Out"].ToString(); ;
                    //  dr["Prod_Storage_Location_Id"] = DtSet.Rows[i]["Prod_Storage_Location_Id"].ToString();
                    dr["Issue_Price"] = DtSet.Rows[i]["Issue_Price"].ToString();

                    dt.Rows.Add(dr);
                
            }
            //dgProductData.DataSource = dt;

            MyConnection.Close();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                StockAdjustment SC = new StockAdjustment();
                SC.Slip_NO = txtSlipNo.Text;
                SC.Slip_Date = dpSODate.Value;
                SC.Prod_Code = Convert.ToInt32(dt.Rows[i]["Prod_Code"]); 
                SC.Stock_In = (dt.Rows[i]["Stock_In"] == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dt.Rows[i]["Stock_In"]);
                SC.Stock_Qty = 0;
                SC.Stock_Out = (dt.Rows[i]["Stock_Out"] == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dt.Rows[i]["Stock_Out"]);
                SC.Issue_Price = (dt.Rows[i]["Issue_Price"] == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dt.Rows[i]["Issue_Price"]);
                SC.Issue_Value = 0;
                SC.Remarks = "Stock Adjusted based on Report received by Ramesh as on 30/11/2022";
                SC.Created_By = lblCreatedBy.Text;
                SC.Modified_By = logIn.username + "-" + DateTime.Now;
                SC.ProdSno = i + 1;
                SC.Company_ID = logIn.company;
                SC.BU_ID = logIn.BU_ID;
                db.StockAdjustments.InsertOnSubmit(SC);
                db.SubmitChanges();



            }

            Cursor.Current = Cursors.Default;
            MessageBox.Show("Imported Successfully");
        }

        private void btnFind_Click(object sender, EventArgs e)
        {

           
        }

        private void brnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //  bindCashAct();
                MaterialManagement.Transactions.frmStockAdjustmentList obj = new MaterialManagement.Transactions.frmStockAdjustmentList();

                if (obj.ShowDialog() == DialogResult.OK)
                {
                    txtSlipNo.Text = MaterialManagement.Transactions.frmStockAdjustmentList.voucherNo;
                    dpSODate.Text = MaterialManagement.Transactions.frmStockAdjustmentList.vdate.ToString();
                    lblCreatedBy.Text = MaterialManagement.Transactions.frmStockAdjustmentList.Createdby.ToString();
                    lblModified.Text = MaterialManagement.Transactions.frmStockAdjustmentList.Modifiedby.ToString();


                    if (!string.IsNullOrEmpty(txtSlipNo.Text))
                    {
                        var dm1 = (from s in db.StockAdjustments         
                                   join p in db.Products on s.Prod_Code equals p.prod_ID
                                   where s.Slip_NO == txtSlipNo.Text && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID
                                   select new
                                   {
                                       Item_Code= s.Prod_Code,
                                       Prod_Code = p.Prod_Code,
                                       Item_Description = p.Prod_Name,                                   
                                       UOM = s.Uom,
                                       s.Stock_Qty,
                                       s.Stock_In,
                                       s.Stock_Out,                                       
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
