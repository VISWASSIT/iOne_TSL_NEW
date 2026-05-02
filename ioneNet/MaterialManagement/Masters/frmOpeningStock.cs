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
using System.Configuration;
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Diagnostics;
using System.Data.OleDb;
using Ione_DAL;

namespace ioneNet.MaterialManagement.Masters
{
    public partial class frmOpeningStock : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string SONo, ItemCode, RecQty, Suppname;
        public frmOpeningStock()
        {
            InitializeComponent();
        }

        private void frmOpeningStock_Load(object sender, EventArgs e)
        {
            try
            {
                //PictureBox2.Image = AppCode.GlobalAccess.comylogo;
                AutoincrementId();
                lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

                //Product Groups
                var pStatus = (from m in db.ProductGroup_Lists where m.Company_ID == logIn.company select new { m.ID, m.Prod_Group_Name }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbAccountGroup.DataSource = pStatus;
                    cmbAccountGroup.ValueMember = "ID";
                    cmbAccountGroup.DisplayMember = "Prod_Group_Name";
                }
                //Storage Locations
                var plocation = (from m in db.Storage_Locations where m.Company_ID == logIn.company && m.Status_ID==1 select new { m.Storage_Loc_Id, m.Storage_Loc_Name }).Distinct().ToList();
                if (plocation.Count > 0)
                {
                    cmbStorageLocation.DataSource = plocation;
                    cmbStorageLocation.ValueMember = "Storage_Loc_Id";
                    cmbStorageLocation.DisplayMember = "Storage_Loc_Name";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnGetAccounts_Click(object sender, EventArgs e)
        {
            try
            {
                if (logIn.company == 1044)
                {
                    if (chkAllAccounts.Checked)
                    {
                        var dm1 = (from s in db.View_Product_OpeningBals
                                       //  join a in db.AccountMasters on s.AccCode equals a.AccCode
                                   where s.Company_ID == logIn.company && s.purchase_account == logIn.BU_ID
                                   select new
                                   {
                                       s.prod_ID,
                                       Prod_code = s.Prod_Alternative_Code,
                                       s.Prod_Name,
                                       s.Drawing_No,
                                       s.Uom_Descr,
                                       s.Prod_Group_Name,
                                       s.OB_Stock_Wt,
                                       s.OB_Price,
                                       s.OB_Value

                                   });
                        SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        DataTable dtr = new DataTable();
                        da2.Fill(dtr);
                        if (dtr.Rows.Count >= 0)
                            dgvJournalVouchar.DataSource = dtr;
                    }
                    else
                    {
                        var dm1 = (from s in db.View_Product_OpeningBals
                                       //  join a in db.AccountMasters on s.AccCode equals a.AccCode
                                   where s.Prod_Group_Name == cmbAccountGroup.Text && s.Company_ID == logIn.company && s.purchase_account == logIn.BU_ID
                                   select new
                                   {
                                      
                                       Prod_Code = s.prod_ID,
                                       Item_Code = (logIn.company == 1044 ? s.Prod_Alternative_Code : s.Prod_Grade)  ,    
                                       s.Prod_Name,
                                       s.Drawing_No,
                                       s.Uom_Descr,
                                       s.Prod_Group_Name,
                                       s.OB_Stock_Wt,
                                       s.OB_Price,
                                       s.OB_Value

                                   });
                        SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        DataTable dtr = new DataTable();
                        da2.Fill(dtr);
                        if (dtr.Rows.Count >= 0)
                            dgvJournalVouchar.DataSource = dtr;
                    }
                }
                else
                {

                    if (chkAllAccounts.Checked)
                    {
                        var dm1 = (from s in db.View_Product_OpeningBals
                                       //  join a in db.AccountMasters on s.AccCode equals a.AccCode
                                   where s.Company_ID == logIn.company
                                   select new
                                   {
                                       Prod_Code = s.prod_ID,
                                       Item_Code = (logIn.company == 1044 ? s.Prod_Alternative_Code : s.Prod_Grade),
                                       s.Prod_Name,
                                       s.Drawing_No,
                                       s.Uom_Descr,
                                       s.Prod_Group_Name,
                                       s.OB_Stock_Wt,
                                       s.OB_Price,
                                       s.OB_Value

                                   });
                        SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        DataTable dtr = new DataTable();
                        da2.Fill(dtr);
                        if (dtr.Rows.Count >= 0)
                            dgvJournalVouchar.DataSource = dtr;
                    }
                    else
                    {
                        var dm1 = (from s in db.View_Product_OpeningBals
                                       //  join a in db.AccountMasters on s.AccCode equals a.AccCode
                                   where s.Prod_Group_Name == cmbAccountGroup.Text && s.Company_ID == logIn.company
                                   select new
                                   {
                                       Prod_Code = s.prod_ID,
                                       Item_Code = (logIn.company == 1044 ? s.Prod_Alternative_Code : s.Prod_Grade),
                                       s.Prod_Name,
                                       s.Drawing_No,
                                       s.Uom_Descr,
                                       s.Prod_Group_Name,
                                       s.OB_Stock_Wt,
                                       s.OB_Price,
                                       s.OB_Value

                                   });
                        SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        DataTable dtr = new DataTable();
                        da2.Fill(dtr);
                        if (dtr.Rows.Count >= 0)
                            dgvJournalVouchar.DataSource = dtr;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
}

        private void btnImport_Click(object sender, EventArgs e)
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

            string SheetName = "Sheet4";
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
            dt.Columns.Add(new DataColumn("Prod_ID", typeof(string)));            
            dt.Columns.Add(new DataColumn("OB_Stock_Qty", typeof(string)));
            dt.Columns.Add(new DataColumn("OB_Price", typeof(string)));           
            dt.Columns.Add(new DataColumn("OB_Value", typeof(string)));           

            for (int i = 0; i < count; i++)
            {
                if(DtSet.Rows[i]["Prod_ID"].ToString()== "#N/A")
                {
                    Product p = new Product();
                    p.Prod_Code = DtSet.Rows[i]["Prod_Code"].ToString();
                    //p.Prod_Group_Id = Convert.ToInt32(dt.Rows[i]["Prod_Group_Id"].ToString());
                    //p.Prod_Type_Id = Convert.ToInt32(dt.Rows[i]["Prod_Type_Id"].ToString());
                    p.Prod_Name = DtSet.Rows[i]["Prod_Name"].ToString();
                    //p.Prod_Storage_Location_Id = (dt.Rows[i]["Prod_Unit_Wt"].ToString() == "") ? Convert.ToInt32("0") : Convert.ToInt32(dt.Rows[i]["Prod_Storage_Location_Id"].ToString());
                    //p.Prod_Primary_UOM_Id = Convert.ToInt32(dt.Rows[i]["Prod_Primary_UOM_Id"].ToString());
                    //p.Prod_Unit_Wt = (dt.Rows[i]["Prod_Unit_Wt"].ToString() == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(dt.Rows[i]["Prod_Unit_Wt"].ToString());
                    //p.Prod_Tax_Class = Convert.ToInt32(dt.Rows[i]["Prod_Tax_Class"].ToString());
                    //p.Prod_Description = (dt.Rows[i]["Prod_Description"].ToString() == "") ? "" : dt.Rows[i]["Prod_Description"].ToString();
                    //p.Prod_HSN_Code = (dt.Rows[i]["Prod_HSN_Code"].ToString() == "") ? "" : dt.Rows[i]["Prod_HSN_Code"].ToString();
                    //p.Prod_Status_ID = Convert.ToInt32(dt.Rows[i]["Prod_Status_ID"].ToString());
                    //p.Prod_Mfg_Code = (dt.Rows[i]["Prod_Mfg_Code"].ToString() == "") ? "" : dt.Rows[i]["Prod_Mfg_Code"].ToString();
                    p.Created_By = logIn.username + "-" + DateTime.Now;
                    p.Modified_BY = logIn.username + "-" + DateTime.Now;
                    p.Company_ID = logIn.company;
                    db.Products.InsertOnSubmit(p);
                    db.SubmitChanges();
                }
                dr = dt.NewRow();
                var dm2 = (from s in db.Products
                               //  join a in db.Storage_Locations on s.Storage_Location equals a.Storage_Loc_Id
                           where s.prod_ID == Convert.ToInt32(DtSet.Rows[i]["Prod_ID"].ToString())
                           select new
                           {
                               s.prod_ID
                           }).ToList();
                if (dm2.Count > 0)
                { 
                dr["Prod_ID"] = DtSet.Rows[i]["Prod_ID"].ToString();
                //dr["Prod_Name"] = DtSet.Rows[i]["Prod_Name"].ToString();
              //  dr["UOM"] = DtSet.Rows[i]["UOM"].ToString();
                dr["OB_Stock_Qty"] = DtSet.Rows[i]["OB_Stock_Qty"].ToString();
                dr["OB_Price"] = DtSet.Rows[i]["OB_Price"].ToString(); ;
              //  dr["Prod_Storage_Location_Id"] = DtSet.Rows[i]["Prod_Storage_Location_Id"].ToString();
                dr["OB_Value"] = DtSet.Rows[i]["OB_Value"].ToString();

                dt.Rows.Add(dr);
                }
            }
            //dgProductData.DataSource = dt;

            MyConnection.Close();
            for (int i = 0; i < dt.Rows.Count ; i++)
            {
                Product_OpeningStock p1 = new Product_OpeningStock();
                p1.Voucher_no = txtVoucherNo.Text;
                p1.OB_date = AsAtdate.Value;
                p1.Company_ID = logIn.company;
                //p1.Storage_Location =  Convert.ToInt32(dt.Rows[i]["Prod_Storage_Location_Id"]); ;
                p1.Prod_ID = Convert.ToInt32 (dt.Rows[i]["Prod_ID"]); ;
               // p1.Prod_Name = dt.Rows[i]["Prod_Name"].ToString();
                //p1.UOM = dt.Rows[i]["UOM"].ToString();
                p1.OB_Stock_Wt = (dt.Rows[i]["OB_Stock_Qty"] == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dt.Rows[i]["OB_Stock_Qty"]);
                p1.OB_Price = (dt.Rows[i]["OB_Price"] == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dt.Rows[i]["OB_Price"]);
                p1.OB_Value = (dt.Rows[i]["OB_Value"] == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dt.Rows[i]["OB_Value"]);
                p1.OB_Stock_qty = Convert.ToDecimal("00");
                p1.Unit_wt = Convert.ToDecimal("00");
                //p1.Prod_Grade = dt.Rows[i]["Drawing_No"].ToString();
                p1.Prod_length = Convert.ToDecimal("00");

                p1.Sub_Contractor = checkBox1.Checked;
                p1.BU_ID = logIn.BU_ID;
                p1.Created_By = lblCreatedBy.Text;
                p1.Modified_BY = logIn.username + "-" + DateTime.Now;
                db.SubmitChanges();
                db.Product_OpeningStocks.InsertOnSubmit(p1);
                db.SubmitChanges();



            }

            Cursor.Current = Cursors.Default;
            MessageBox.Show("Imported Successfully");
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (cmbStorageLocation.Text == "")
                {
                    MessageBox.Show("Ware House / Storage Location Should not be empty");
                    cmbStorageLocation.Focus();
                    return;
                }
                Save();
            }
            catch (Exception ex)
            {


            }
        }
        private void Save()
        {
            try
            {
                if ((from u in db.Product_OpeningStocks where u.Voucher_no == txtVoucherNo.Text && u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID select u).Count() > 0)
                {
                    db.sp_DeleteOpeningStocks(txtVoucherNo.Text, logIn.company, logIn.BU_ID);
                }        

                for (int i = 0; i < dgvJournalVouchar.Rows.Count - 1; i++)
                {
                    decimal stkqty= (dgvJournalVouchar.Rows[i].Cells["OB_Stock_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["OB_Stock_Wt"].Value);
                    if (stkqty > 0)
                    {
                        Product_OpeningStock p = new Product_OpeningStock();
                        p.Voucher_no = txtVoucherNo.Text;
                        //p.Account = (cmbAccountName.Text == "") ? "" : cmbAccountName.Text;
                        p.OB_date = AsAtdate.Value;
                        //p.OBDate = ObDate.Value;
                        p.Prod_ID = Convert.ToInt32(dgvJournalVouchar.Rows[i].Cells["Prod_Code"].Value.ToString());
                        //p.Prod_Name = dgvJournalVouchar.Rows[i].Cells["Prod_Name"].Value.ToString();
                        //p.UOM = dgvJournalVouchar.Rows[i].Cells["Uom_Descr"].Value.ToString();
                       // p.Unit_wt = (dgvJournalVouchar.Rows[i].Cells["Prod_Unit_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Prod_Unit_Wt"].Value);
                        p.Prod_Grade = cmbAccountGroup.Text; 
                        //p.Prod_length = (dgvJournalVouchar.Rows[i].Cells["Prod_length"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Prod_length"].Value);
                        //p.OB_Stock_qty = (dgvJournalVouchar.Rows[i].Cells["OB_Stock_qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["OB_Stock_qty"].Value);
                        p.OB_Stock_Wt = (dgvJournalVouchar.Rows[i].Cells["OB_Stock_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["OB_Stock_Wt"].Value);
                        p.OB_Price = (dgvJournalVouchar.Rows[i].Cells["OB_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["OB_Price"].Value);
                        p.OB_Value = (dgvJournalVouchar.Rows[i].Cells["OB_Value"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["OB_Value"].Value);
                        p.Company_ID = logIn.company;
                        p.BU_ID = logIn.BU_ID;
                        p.Created_By = lblCreatedBy.Text;
                        p.Modified_BY = logIn.username + "-" + DateTime.Now;
                        p.Storage_Location = Convert.ToInt32(cmbStorageLocation.SelectedValue);                       
                        p.Sub_Contractor = checkBox1.Checked;
                        db.Product_OpeningStocks.InsertOnSubmit(p);

                    }
                 }
                db.SubmitChanges();
                MessageBox.Show("Record Saved Successfully");
                clear();
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

                foreach (Control x in this.Controls)
                {

                    foreach (Control d in groupBox3.Controls)
                    {
                        if (d is TextBox)
                            (d as TextBox).Clear();
                        if (d is ComboBox)
                            (d as ComboBox).SelectedIndex = -1;
                        if (d is CheckBox)
                            (d as CheckBox).Checked = false;
                    }
                }
                if (dgvJournalVouchar.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvJournalVouchar.Rows.Count - 1; i++)
                    {
                        dgvJournalVouchar.Rows.RemoveAt(i);
                        i--;
                        while (dgvJournalVouchar.Rows.Count == 0)
                            continue;
                    }
                }
                //cmbAccountName.Text = "";
                //txtCreditAmtTotal.Text = txtDebitAmtTotal.Text = "";
                AutoincrementId();
                // txtTotalAmt.Text = "";
            }
            catch (Exception ex)
            {

            }
        }
        public void AutoincrementId()
        {
            try
            {
                var auto = db.Sp_autoincrement_Prod_OpeningStock(logIn.company,logIn.BU_ID);
                txtVoucherNo.Text = auto.FirstOrDefault().Voucher_No;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " While AutoIncrement Getting Error ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            AutoincrementId();
            AsAtdate.Value = DateTime.Now;
            dgvJournalVouchar.Rows.Clear();
            dgvJournalVouchar.Refresh();
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Opening Stock Info" && m.Role_ID == logIn.UserRoleID select new { m.Delete_Role }).Distinct().ToList();
            if (uRole.Count > 0)
            {
                if (uRole[0].Delete_Role == true)
                {
                    DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        db.sp_DeleteOpeningStocks(txtVoucherNo.Text, logIn.company, logIn.BU_ID);
                        MessageBox.Show("Record Deleted Successfully");
                        clear();
                    }

                }
                else
                {
                    MessageBox.Show("You Are Not Authorized to Delete The Opening Stock Data");
                    return;
                }
            }
            
        }

        private void cmdPrint_Click(object sender, EventArgs e)
        {
            if (dgvJournalVouchar.Rows.Count > 0)
            {
                ExportToExcel(dgvJournalVouchar, "Opening Stock");
            }
        }

        private void dgvJournalVouchar_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // DataGridViewRow dgProductData.Rows[i] = dgProductData.Rows[dgProductData.CurrentRow.Index];
                if (dgvJournalVouchar.Rows.Count > 1)
                {
                    if (dgvJournalVouchar.Rows[dgvJournalVouchar.CurrentRow.Index].Cells[dgvJournalVouchar.CurrentCell.ColumnIndex].Value == "Remove")
                    {
                        if (dgvJournalVouchar.Rows.Count > 0)
                        {
                            foreach (DataGridViewCell oneCell in dgvJournalVouchar.SelectedCells)
                            {
                                if (oneCell.Selected)
                                    dgvJournalVouchar.Rows.RemoveAt(oneCell.RowIndex);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if(checkBox1.Checked)
            {
                //Storage Locations
                var plocation = (from m in db.Supplier_informations where m.Company_ID == logIn.company && m.Supplier_Category==32 select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                if (plocation.Count > 0)
                {
                    cmbStorageLocation.DataSource = plocation;
                    cmbStorageLocation.ValueMember = "ID";
                    cmbStorageLocation.DisplayMember = "Supplier_Name";
                }
            }
            else
            {
                //Storage Locations
                var plocation = (from m in db.Storage_Locations where m.Company_ID == logIn.company  select new { m.Storage_Loc_Id, m.Storage_Loc_Name }).Distinct().ToList();
                if (plocation.Count > 0)
                {
                    cmbStorageLocation.DataSource = plocation;
                    cmbStorageLocation.ValueMember = "Storage_Loc_Id";
                    cmbStorageLocation.DisplayMember = "Storage_Loc_Name";
                }
            }
        }

        private void dgvJournalVouchar_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                DataGridViewRow R1 = dgvJournalVouchar.Rows[dgvJournalVouchar.CurrentRow.Index];
                int columnIndex = dgvJournalVouchar.CurrentCell.ColumnIndex;
                string columnName = dgvJournalVouchar.Columns[columnIndex].Name;
                string pcode = "";
                if (columnName == "Prod_Code" && R1.Cells["Prod_Code"].Value != null)
                {

                    pcode = R1.Cells["Prod_Code"].Value.ToString();
                }

                if (columnName == "Prod_Name" && R1.Cells["Prod_Name"].Value != null)
                {
                    pcode = R1.Cells["Prod_Name"].Value.ToString();
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
                    if (d != null)
                    {
                        R1.Cells["Uom_Descr"].Value = d[0].UOM.ToString();
                        R1.Cells["Prod_Code"].Value = d[0].Item_Code.ToString();
                        R1.Cells["Item_Code"].Value = d[0].Prod_Code.ToString();

                    }

                    else
                    {
                        //   R1.Cells["Prod_ID"].Value = dgProducts.CurrentCell.RowIndex + 1;
                        // R1.Cells["Int_Prod_Code"].Value = "NA";
                    }
                }

                int i = dgvJournalVouchar.CurrentCell.RowIndex;
                decimal Qty = (R1.Cells["OB_Stock_Wt"].Value == "" || R1.Cells["OB_Stock_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["OB_Stock_Wt"].Value);

                decimal Price = (R1.Cells["OB_Stock_Wt"].Value == "" || R1.Cells["OB_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["OB_Price"].Value);

                
                dgvJournalVouchar.Rows[i].Cells["OB_Value"].Value = Qty * Price;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }

        }

        private void dgvJournalVouchar_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int columnIndex = dgvJournalVouchar.CurrentCell.ColumnIndex;
                string columnName = dgvJournalVouchar.Columns[columnIndex].HeaderText;
                TextBox tb3 = e.Control as TextBox;

                if (tb3 != null && columnName == "Prod_Name")
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
                DataGridViewRow R1 = dgvJournalVouchar.Rows[dgvJournalVouchar.CurrentRow.Index];

                int columnIndex = dgvJournalVouchar.CurrentCell.ColumnIndex;
                string columnName = dgvJournalVouchar.Columns[columnIndex].HeaderText;


                if (columnName == "Prod_Name")
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


            }
            catch (Exception ex)
            {
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void brnSearch_Click(object sender, EventArgs e)
        {

            try
            {
                //  bindCashAct();

                //Opening Stock Info

                var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Opening Stock Info" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                if (uRole.Count > 0)
                {
                    if (uRole[0].Modify_Role == true)
                    {

                    }
                    else
                    {
                        MessageBox.Show("You Are Not Authorized to Modify The Opening Stock Data");
                        return;
                    }
                }
                
                MaterialManagement.Masters.OBStockVouchers obj = new MaterialManagement.Masters.OBStockVouchers();

                if (obj.ShowDialog() == DialogResult.OK)
                {
                    txtVoucherNo.Text = MaterialManagement.Masters.OBStockVouchers.voucherNo;

                    if (!string.IsNullOrEmpty(txtVoucherNo.Text))
                    {
                        var dm1 = (from s in db.Product_OpeningStocks
                                   join a in db.Products on s.Prod_ID equals a.prod_ID
                                   join u in db.UoM_Masters on a.Prod_Primary_UOM_Id equals u.UOM_ID
                                   where s.Voucher_no == txtVoucherNo.Text && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID
                                   select new
                                   {
                                       Prod_Code = s.Prod_ID,
                                       Item_Code =  (logIn.company == 1044 ? a.Prod_Alternative_Code : a.Prod_Code),
                                       a.Prod_Name,
                                       Uom_Descr = u.Uom_Descr,
                                       Prod_Unit_Wt = s.Unit_wt,
                                       Drawing_No = a.Prod_Field2,
                                       s.OB_Stock_Wt,
                                       s.OB_Price,
                                       s.OB_Value
                                   });

                        SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        DataTable dtr = new DataTable();
                        da2.Fill(dtr);
                        if (dtr.Rows.Count >= 0)
                            dgvJournalVouchar.DataSource = dtr;



                        var dm2 = (from s in db.Product_OpeningStocks
                                       //  join a in db.Storage_Locations on s.Storage_Location equals a.Storage_Loc_Id
                                   where s.Voucher_no == txtVoucherNo.Text && s.Company_ID == logIn.company && s.BU_ID==logIn.BU_ID
                                   select new
                                   {
                                       s.Sub_Contractor,
                                       s.Storage_Location,
                                       s.OB_date,
                                       s.Prod_Grade,
                                       s.Modified_BY,
                                       s.Created_By
                                       
                                   }).ToList();
                        if (dm2[0].Storage_Location != null)
                        {
                            cmbStorageLocation.SelectedValue = dm2[0].Storage_Location;
                        }
                        if (dm2[0].Sub_Contractor == true)
                        {
                            checkBox1.Checked =true;
                        }
                        else
                        {
                            checkBox1.Checked = false;
                        }
                        AsAtdate.Text = dm2[0].OB_date.ToString();
                        lblModified.Text = dm2[0].Modified_BY.ToString();
                        lblCreatedBy.Text = dm2[0].Created_By.ToString();

                        // cmbAccountGroup.Text = dm2[0].Prod_Grade;

                    }


                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dgvJournalVouchar_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                if (logIn.company == 1044)
                {
                }
                else
                {
                    GlobalVariables.FormName = "OpeningStock";
                    ioneNet.OrderManagement.Transactions.ProdSpecs form = new ioneNet.OrderManagement.Transactions.ProdSpecs();
                    //ioneNet.Masters.ProdSearch.frmName = "SOrder";       
                    int i = dgvJournalVouchar.CurrentCell.RowIndex;
                    SONo = "OB";
                    ItemCode = dgvJournalVouchar.Rows[i].Cells["Prod_Code"].Value.ToString();
                    RecQty = dgvJournalVouchar.Rows[i].Cells["OB_Stock_Wt"].Value.ToString();
                    form.ShowDialog();
                    dgvJournalVouchar.Rows[i].Cells["OB_Stock_Wt"].Value = ioneNet.OrderManagement.Transactions.ProdSpecs.TotQty;
                }
            }
            if (e.KeyCode == Keys.F6)
            {
                if (dgvJournalVouchar.Rows.Count > 0)
                {

                    foreach (DataGridViewCell oneCell in dgvJournalVouchar.SelectedCells)
                    {
                        if (oneCell.Selected)
                            dgvJournalVouchar.Rows.RemoveAt(oneCell.RowIndex);
                    }
                }
            }
        }
        public void ExportToExcel(DataGridView gridviewID, string excelFilename)
        {
            try
            {

                string path = Path.Combine(Directory.GetCurrentDirectory(), "" + excelFilename + ".xlsx");
                Excel.Application xlAppToExport = new Excel.Application();
                xlAppToExport.Workbooks.Add("");

                // ADD A WORKSHEET.
                Excel.Worksheet xlWorkSheetToExport = default(Excel.Worksheet);
                xlWorkSheetToExport = (Excel.Worksheet)xlAppToExport.Sheets["Sheet1"];

                FileInfo file = new FileInfo(path);
                if (file.Exists)//check file exsit or not
                {
                    file.Delete();
                }

                int iRowCnt = 7;
                var data = (from s in db.Company_Infos
                            where s.Id == logIn.company
                            select new
                            {
                                s.Company_Name,
                                Company_address = s.Address + ',' + s.City + ',' + s.State + ',' + s.Phone_No + ',' + s.E_Mail + ',' + s.Website + '.'
                            }).ToList();


                xlWorkSheetToExport.Cells[1, 1] = data[0].Company_Name.ToString();
                Excel.Range range = xlWorkSheetToExport.Cells[1, 1] as Excel.Range;
                range.EntireRow.Font.Name = "Calibri";
                range.EntireRow.Font.Bold = true;
                range.EntireRow.Font.Size = 12;
                //xlWorkSheetToExport.Range["A1:M1"].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                //xlWorkSheetToExport.Range["A1:M1"].MergeCells = true;       // MERGE CELLS OF THE HEADER.                
                xlWorkSheetToExport.Cells[2, 1] ="Opening Stock";


                //xlWorkSheetToExport.Cells[4, 1] = "Account Name" + cmbAccName.Text;
                //Excel.Range range1 = xlWorkSheetToExport.Cells[2, 1] as Excel.Range;
                //range1.EntireRow.Font.Name = "Calibri";
                //range1.EntireRow.Font.Bold = false;
                //range1.EntireRow.Font.Size = 12;
                //range1.RowHeight = 20;
                //xlWorkSheetToExport.Range["A2:M2"].WrapText = true;
                //xlWorkSheetToExport.Range["A2:M2"].MergeCells = true;
                // SHOW THE HEADER File Name
                string d = "";
                // SHOW THE HEADER File Name
              
                // string d = " From Date :" +dpFromDate.Text +",      TO Date :"+ (dpTodate.Text) +",      Customer Name :"+ txtCust_Prod_code.Text + "  ,  Product Name :"+txtProductName.Text;
                xlWorkSheetToExport.Cells[4, 1] = d;
                Excel.Range range5 = xlWorkSheetToExport.Cells[2, 1] as Excel.Range;
                range5.EntireRow.Font.Name = "Calibri";
                //  range5.EntireRow.Font.Bold = true;
                range5.EntireRow.Font.Size = 12;
                //xlWorkSheetToExport.Range["A5:M5"].HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
                //xlWorkSheetToExport.Range["A5:M5"].WrapText = true;
                //xlWorkSheetToExport.Range["A5:M5"].MergeCells = true;
                // MERGE CELLS OF THE HEADER.

                for (int i = 1; i < gridviewID.Columns.Count + 1; i++)
                {
                    xlWorkSheetToExport.Cells[6, i] = gridviewID.Columns[i - 1].HeaderText;
                }

                for (int i = 0; i < gridviewID.Rows.Count; i++)
                {
                    for (int j = 0; j < gridviewID.Columns.Count; j++)
                    {
                        if (gridviewID.Rows[i].Cells[j].Value != null)
                        {
                            Excel.Range range7 = xlWorkSheetToExport.Cells[i + 7, j + 1] as Excel.Range;

                            range7.NumberFormat = "@";

                            xlWorkSheetToExport.Cells[i + 7, j + 1] = gridviewID.Rows[i].Cells[j].Value.ToString();

                        }
                    }
                }

                xlWorkSheetToExport.Columns.AutoFit();
                xlAppToExport.DisplayAlerts = false;
                xlWorkSheetToExport.SaveAs(path);
                // CLEAR.
                xlAppToExport.Workbooks.Close();
                xlAppToExport.Quit();
                xlAppToExport = null;
                xlWorkSheetToExport = null;
                Process.Start(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
