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
using Ione_DAL;
using System.Data.OleDb;

namespace ioneNet.MaterialManagement.Masters
{
    public partial class frmOpeningStock_FG : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string SONo, ItemCode, RecQty, Suppname;
        public frmOpeningStock_FG()
        {
            InitializeComponent();
        }

        private void frmOpeningStock_Load(object sender, EventArgs e)
        {
            try
            {
                //PictureBox2.Image = AppCode.GlobalAccess.comylogo;
                AutoincrementId();
                //Product Groups
                var pStatus = (from m in db.ProductGroup_Lists where m.Company_ID == logIn.company && m.Prod_Type == 139 select new { m.ID, m.Prod_Group_Name }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbProdGroup.DataSource = pStatus;
                    cmbProdGroup.ValueMember = "ID";
                    cmbProdGroup.DisplayMember = "Prod_Group_Name";
                }
                //Storage Locations
                var plocation = (from m in db.Supplier_informations where m.Company_ID == logIn.company select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                if (plocation.Count > 0)
                {
                    cmbCustomer.DataSource = plocation;
                    cmbCustomer.ValueMember = "ID";
                    cmbCustomer.DisplayMember = "Supplier_Name";
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
                if(cmbProdGroup.Text !="")
                {
                    var dm1 = (from s in db.View_Product_OpeningBal_FGs
                                   //  join a in db.AccountMasters on s.AccCode equals a.AccCode
                               where s.Company_ID == logIn.company && s.Prod_Group_Name == cmbProdGroup.Text
                               select new
                               {
                                   s.prod_ID,
                                   s.Prod_Code,
                                   s.Prod_Name,
                                   s.Uom_Descr,
                                   s.Prod_Unit_Wt,
                                   //s.Prod_Group_Name,
                                   s.Prod_length,
                                   s.OB_Stock_qty,
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
                    var dm1 = (from s in db.View_Product_OpeningBal_FGs
                                   //  join a in db.AccountMasters on s.AccCode equals a.AccCode
                               where s.Company_ID == logIn.company
                               select new
                               {
                                   s.prod_ID,
                                   s.Prod_Code,
                                   s.Prod_Name,
                                   s.Uom_Descr,
                                   s.Prod_Unit_Wt,
                                   s.Prod_Group_Name,
                                   s.Prod_length,
                                   s.OB_Stock_qty,
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
            System.Data.DataRow dr = null;
            dt.Columns.Add(new DataColumn("Prod_ID", typeof(string)));
            dt.Columns.Add(new DataColumn("Prod_Code", typeof(string)));
            dt.Columns.Add(new DataColumn("Prod_Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Uom_Descr", typeof(string)));
            dt.Columns.Add(new DataColumn("OB_Stock_Wt", typeof(string)));
            dt.Columns.Add(new DataColumn("OB_Price", typeof(string)));            
            dt.Columns.Add(new DataColumn("OB_Value", typeof(string)));

            for (int i = 0; i < count; i++)
            {
                dr = dt.NewRow();

                //var dm2 = (from s in db.Products
                //               //  join a in db.Storage_Locations on s.Storage_Location equals a.Storage_Loc_Id
                //           where s.Prod_Code == DtSet.Rows[i]["Prod_Code"].ToString()
                //           select new
                //           {
                //               s.prod_ID
                //           }).ToList();
                //if (dm2.Count > 0)
                //{
                    dr["Prod_ID"] =   DtSet.Rows[i]["Prod_ID"].ToString();
                    dr["Prod_Code"] = DtSet.Rows[i]["Prod_Code"].ToString();
                    dr["Prod_Name"] = DtSet.Rows[i]["Prod_Name"].ToString();
                    dr["Uom_Descr"] = DtSet.Rows[i]["Uom_Descr"].ToString();
                    dr["OB_Stock_Wt"] = DtSet.Rows[i]["OB_Stock_Wt"].ToString();
                    dr["OB_Price"] = DtSet.Rows[i]["OB_Price"].ToString(); ;
                    //dr["Prod_Storage_Location_Id"] = DtSet.Rows[i]["Prod_Storage_Location_Id"].ToString();
                    dr["OB_Value"] = DtSet.Rows[i]["OB_Value"].ToString();

                    dt.Rows.Add(dr);
                //}
            }
            dgvJournalVouchar.DataSource = dt;

            MyConnection.Close();
            //for (int i = 0; i < dt.Rows.Count - 1; i++)
            //{
            //    Product_OpeningStock p1 = new Product_OpeningStock();
            //    p1.Voucher_no = txtVoucherNo.Text;
            //    p1.OB_date = AsAtdate.Value;
            //    p1.Company_ID = logIn.company;
            //    //p1.Storage_Location =  Convert.ToInt32(dt.Rows[i]["Prod_Storage_Location_Id"]); ;
            //    p1.Prod_ID = Convert.ToInt32(dt.Rows[i]["Prod_ID"]); ;
            //    p1.Prod_Name = dt.Rows[i]["Prod_Name"].ToString();
            //    p1.UOM = dt.Rows[i]["Uom_Descr"].ToString();
            //    p1.OB_Stock_Wt = (dt.Rows[i]["OB_Stock_Wt"] == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dt.Rows[i]["OB_Stock_Wt"]);
            //    p1.OB_Price = (dt.Rows[i]["OB_Price"] == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dt.Rows[i]["OB_Price"]);
            //    p1.OB_Value = (dt.Rows[i]["OB_Value"] == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dt.Rows[i]["OB_Value"]);
            //    p1.OB_Stock_qty = Convert.ToDecimal("00");
            //    p1.Unit_wt = Convert.ToDecimal("00");
            //    p1.Prod_Grade = "";
            //    p1.Prod_length = Convert.ToDecimal("00");

            //    p1.Sub_Contractor = checkBox1.Checked;
            //    p1.BU_ID = logIn.BU_ID;
            //    p1.Created_By = lblCreatedBy.Text;
            //    p1.Modified_BY = logIn.username + "-" + DateTime.Now;
            //    db.SubmitChanges();
            //    db.Product_OpeningStocks.InsertOnSubmit(p1);
            //    db.SubmitChanges();



            //}

            Cursor.Current = Cursors.Default;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                //if (cmbAccountName.Text == "")
                //{
                //    MessageBox.Show("Account Name Should not be empty");
                //    return;
                //}
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

                if ((from u in db.Forging_FG_OpeningStocks where u.Voucher_no == txtVoucherNo.Text && u.Company_ID == logIn.company select u).Count() > 0)
                {

                    //if (AppCode.GlobalAccess.Edit == "Yes")
                    //{
                    db.sp_DeleteFG_OpeningStocks(txtVoucherNo.Text, logIn.company);

                    for (int i = 0; i < dgvJournalVouchar.Rows.Count - 1; i++)
                    {
                        Forging_FG_OpeningStock p = new Forging_FG_OpeningStock();
                        p.Voucher_no = txtVoucherNo.Text;                            //p.Account = (cmbAccountName.Text == "") ? "" : cmbAccountName.Text;
                        p.OB_date = AsAtdate.Value;                        //p.OBDate = ObDate.Value;
                       
                        p.Company_ID = logIn.company;
                       // p.Storage_Location = Convert.ToInt32(cmbStorageLocation.SelectedValue);
                        //p.Credit = (dgvJournalVouchar.Rows[i].Cells["Credit"].Value == DBNull.Value && dgvJournalVouchar.Rows[i].Cells["Credit"].Value == null && dgvJournalVouchar.Rows[i].Cells["Credit"].Value == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Credit"].Value);
                        //p.Credit_Amount = (dgvJournalVouchar.Rows[i].Cells["Credit"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Credit"].Value);
                        //p.Debit_Amount = (dgvJournalVouchar.Rows[i].Cells["Debit"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Debit"].Value);
                        //p.Debit = (dgvJournalVouchar.Rows[i].Cells["Debit"].Value == DBNull.Value && dgvJournalVouchar.Rows[i].Cells["Debit"].Value == null && dgvJournalVouchar.Rows[i].Cells["Debit"].Value == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Debit"].Value);
                        p.Prod_ID = Convert.ToInt32(dgvJournalVouchar.Rows[i].Cells["Prod_ID"].Value.ToString());
                        //p.Prod_Name = dgvJournalVouchar.Rows[i].Cells["Prod_Name"].Value.ToString();
                        //p.UOM = dgvJournalVouchar.Rows[i].Cells["Uom_Descr"].Value.ToString();                       
                //        p.Unit_wt = (dgvJournalVouchar.Rows[i].Cells["Prod_Unit_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Prod_Unit_Wt"].Value);
                        p.Prod_Grade =  dgvJournalVouchar.Rows[i].Cells["Prod_Grade"].Value.ToString().Trim();                        
                        p.Prod_length = (dgvJournalVouchar.Rows[i].Cells["Prod_length"].Value.ToString());
                        p.OB_Stock_qty = (dgvJournalVouchar.Rows[i].Cells["OB_Stock_qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["OB_Stock_qty"].Value);
                        p.OB_Stock_Wt = (dgvJournalVouchar.Rows[i].Cells["OB_Stock_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["OB_Stock_Wt"].Value);
                        p.OB_Price = (dgvJournalVouchar.Rows[i].Cells["OB_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["OB_Price"].Value);
                        p.OB_Value = (dgvJournalVouchar.Rows[i].Cells["OB_Value"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["OB_Value"].Value);
                        p.Stock_Stage = cmbStorageLocation.Text;
                        p.Conversion_Material = checkBox1.Checked;
                        if (cmbCustomer.Text != "")
                        {
                            p.Customer_Name = Convert.ToInt32(cmbCustomer.SelectedValue);
                        }
                        else
                        {
                            p.Customer_Name = 0;
                        }
                        p.Created_By = lblCreatedBy.Text;
                        p.Modified_BY = logIn.username + "-" + DateTime.Now;

                        db.Forging_FG_OpeningStocks.InsertOnSubmit(p);

                    }

                    db.SubmitChanges();
                    MessageBox.Show("Record Updated Successfully");
                    clear();
                    ////}
                    ////else
                    ////{
                    ////    MessageBox.Show("You dont Have Privileges", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ////    clear();
                    ////}


                }
                else
                {
                    try
                    {

                        //if (AppCode.GlobalAccess.Add == "Yes")
                        //{
                        for (int i = 0; i < dgvJournalVouchar.Rows.Count - 1; i++)
                        {
                            decimal stkqty= (dgvJournalVouchar.Rows[i].Cells["OB_Stock_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["OB_Stock_Wt"].Value);
                            if (stkqty > 0)
                            {
                                Forging_FG_OpeningStock p = new Forging_FG_OpeningStock();
                                p.Voucher_no = txtVoucherNo.Text;
                                //p.Account = (cmbAccountName.Text == "") ? "" : cmbAccountName.Text;
                                p.OB_date = AsAtdate.Value;
                                //p.OBDate = ObDate.Value;
                                p.Prod_ID = Convert.ToInt32(dgvJournalVouchar.Rows[i].Cells["Prod_ID"].Value.ToString());
                  //              p.Prod_Name = dgvJournalVouchar.Rows[i].Cells["Prod_Name"].Value.ToString();
                    //            p.UOM = dgvJournalVouchar.Rows[i].Cells["Uom_Descr"].Value.ToString();
                      //          p.Unit_wt = (dgvJournalVouchar.Rows[i].Cells["Prod_Unit_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Prod_Unit_Wt"].Value);
                                p.Prod_Grade = dgvJournalVouchar.Rows[i].Cells["Prod_Grade"].Value.ToString().Trim();
                                p.Prod_length = (dgvJournalVouchar.Rows[i].Cells["Prod_length"].Value.ToString());
                                p.OB_Stock_qty = (dgvJournalVouchar.Rows[i].Cells["OB_Stock_qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["OB_Stock_qty"].Value);
                                p.OB_Stock_Wt = (dgvJournalVouchar.Rows[i].Cells["OB_Stock_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["OB_Stock_Wt"].Value);
                                p.OB_Price = (dgvJournalVouchar.Rows[i].Cells["OB_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["OB_Price"].Value);
                                p.OB_Value = (dgvJournalVouchar.Rows[i].Cells["OB_Value"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["OB_Value"].Value);
                                p.Company_ID = logIn.company;
                                //p.Storage_Location = Convert.ToInt32(cmbStorageLocation.SelectedValue);
                                p.Stock_Stage = cmbStorageLocation.Text;
                                p.Conversion_Material = checkBox1.Checked;
                                if (cmbCustomer.Text != "")
                                {
                                    p.Customer_Name = Convert.ToInt32(cmbCustomer.SelectedValue);
                                }
                                else
                                {
                                    p.Customer_Name = 0;
                                }
                                p.Created_By = logIn.username + "-" + DateTime.Now;
                                p.Modified_BY = logIn.username + "-" + DateTime.Now;

                                db.Forging_FG_OpeningStocks.InsertOnSubmit(p);

                            }
                        }
                        db.SubmitChanges();
                        MessageBox.Show("Record Saved Successfully");
                        clear();
                        //}
                        //else
                        //{
                        //    MessageBox.Show("You dont Have Privileges", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        //    clear();
                        //}
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show(ex.Message);
                    }

                    finally
                    {
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
                var auto = db.Sp_Forging_autoincrement_FG_OpeningStock(logIn.company);
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
            //dgvJournalVouchar.Rows.Clear();
            //dgvJournalVouchar.Refresh();
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                db.sp_DeleteFG_OpeningStocks (txtVoucherNo.Text, logIn.company);
                MessageBox.Show("Record Deleted Successfully");
                clear();
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
            //if(checkBox1.Checked)
            //{
            //    //Storage Locations
            //    var plocation = (from m in db.Supplier_informations where m.Company_ID == logIn.company && m.Supplier_Category==32 select new { m.ID, m.Supplier_Name }).Distinct().ToList();
            //    if (plocation.Count > 0)
            //    {
            //        cmbStorageLocation.DataSource = plocation;
            //        cmbStorageLocation.ValueMember = "ID";
            //        cmbStorageLocation.DisplayMember = "Supplier_Name";
            //    }
            //}
            //else
            //{
            //    //Storage Locations
            //    var plocation = (from m in db.Storage_Locations where m.Company_ID == logIn.company  select new { m.Storage_Loc_Id, m.Storage_Loc_Name }).Distinct().ToList();
            //    if (plocation.Count > 0)
            //    {
            //        cmbStorageLocation.DataSource = plocation;
            //        cmbStorageLocation.ValueMember = "Storage_Loc_Id";
            //        cmbStorageLocation.DisplayMember = "Storage_Loc_Name";
            //    }
            //}
        }

        private void dgvJournalVouchar_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewRow R1 = dgvJournalVouchar.Rows[dgvJournalVouchar.CurrentRow.Index];
            int columnIndex = dgvJournalVouchar.CurrentCell.ColumnIndex;
            string columnName = dgvJournalVouchar.Columns[columnIndex].Name;

            if (columnName == "Prod_Name")
            {
                string prodgrade = "";
                string prodname = R1.Cells["Prod_Name"].Value.ToString();

                var getProductName = (from s in db.Get_ProductsList_TSL(logIn.company, 1, prodname, prodgrade)
                                      select new { s.prod_ID, s.Prod_Code, s.Uom_Descr, s.Prod_Group_Name, s.Prod_Unit_Wt, s.Price }).FirstOrDefault();


                if (getProductName != null)
                {
                    R1.Cells["Uom_Descr"].Value = getProductName.Uom_Descr.ToString();
                    R1.Cells["Prod_ID"].Value = getProductName.prod_ID.ToString();
                    R1.Cells["Prod_Code"].Value = getProductName.Prod_Code.ToString();
                    R1.Cells["OB_Price"].Value = getProductName.Price.ToString();
                    if (getProductName.Prod_Unit_Wt == null)
                    {
                        R1.Cells["Prod_Unit_Wt"].Value = 0;
                    }
                    else
                    {
                        R1.Cells["Prod_Unit_Wt"].Value = getProductName.Prod_Unit_Wt.ToString();
                    }
                }
            }
            else
            {
                if (columnName == "OB_Stock_Wt" || columnName == "OB_Price" || columnName == "OB_Stock_qty")
                {
                    int i = dgvJournalVouchar.CurrentCell.RowIndex;
                    decimal UnitWt = (dgvJournalVouchar.Rows[i].Cells["Prod_Unit_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["OB_Stock_Wt"].Value);

                    decimal Qty = (dgvJournalVouchar.Rows[i].Cells["OB_Stock_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["OB_Stock_Wt"].Value);

                    decimal Price = (dgvJournalVouchar.Rows[i].Cells["OB_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["OB_Price"].Value);

                    dgvJournalVouchar.Rows[i].Cells["OB_Value"].Value = Qty * Price;
                }
            }
        }

        private void dgvJournalVouchar_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int columnIndex = dgvJournalVouchar.CurrentCell.ColumnIndex;
                string columnName = dgvJournalVouchar.Columns[columnIndex].HeaderText;
                TextBox tb3 = e.Control as TextBox;
                tb3.AutoCompleteMode = AutoCompleteMode.None;
                
                if (tb3 != null && columnName == "Item GRADE")
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

                if (columnName == "Item Description")
                {
                    var Prodname = (from d in db.Products where d.Company_ID == logIn.company && d.Prod_Type_Id == 139 select new { d.Prod_Name }).ToList();
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

                if (columnName == "Item GRADE")
                {
                    var Prodname = (from d in db.QA_Mtrl_Grade_Masters select new { d.Material_Grade }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Item_Grade");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Material_Grade);
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
        private void brnSearch_Click(object sender, EventArgs e)
        {

            try
            {
                //  bindCashAct();
                MaterialManagement.Masters.FG_OBStockVouchers obj = new MaterialManagement.Masters.FG_OBStockVouchers();

                if (obj.ShowDialog() == DialogResult.OK)
                {
                    txtVoucherNo.Text = MaterialManagement.Masters.FG_OBStockVouchers.voucherNo;

                    if (!string.IsNullOrEmpty(txtVoucherNo.Text))
                    {
                        var dm1 = (from s in db.Forging_FG_OpeningStocks
                                   
                                         join a in db.Products on s.Prod_ID equals a.prod_ID
                                         join u in db.UoM_Masters on a.Prod_Primary_UOM_Id equals u.UOM_ID
                                   where s.Voucher_no == txtVoucherNo.Text && s.Company_ID == logIn.company
                                   select new
                                   {
                                       s.Prod_ID,
                                       Prod_Code = a.Prod_Code,
                                       a.Prod_Name,
                                       Uom_Descr = u.Uom_Descr,
                                       Prod_Unit_Wt = a.Prod_Unit_Wt,
                                       s.Prod_Grade,
                                       s.Prod_length,
                                       s.OB_Stock_qty,
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



                        var dm2 = (from s in db.Forging_FG_OpeningStocks
                                       //  join a in db.Storage_Locations on s.Storage_Location equals a.Storage_Loc_Id
                                   where s.Voucher_no == txtVoucherNo.Text && s.Company_ID == logIn.company
                                   select new
                                   {
                                       s.Stock_Stage,
                                       s.Conversion_Material,
                                       s.Customer_Name,
                                       s.OB_date
                                   }).ToList();
                        cmbStorageLocation.Text = dm2[0].Stock_Stage;

                        if (dm2[0].Conversion_Material == true)
                        {
                            checkBox1.Checked = true;
                            cmbCustomer.SelectedValue = dm2[0].Customer_Name;
                        }
                        else
                        {
                            checkBox1.Checked = false;
                            cmbCustomer.Text = "";
                        }
                        AsAtdate.Text = dm2[0].OB_date.ToString();

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
                GlobalVariables.FormName = "OpeningStock";
                ioneNet.OrderManagement.Transactions.ProdSpecs form = new ioneNet.OrderManagement.Transactions.ProdSpecs();
                //ioneNet.Masters.ProdSearch.frmName = "SOrder";       
                int i = dgvJournalVouchar.CurrentCell.RowIndex;
                SONo = "OB";
                ItemCode = dgvJournalVouchar.Rows[i].Cells["Prod_ID"].Value.ToString();
                RecQty = dgvJournalVouchar.Rows[i].Cells["OB_Stock_qty"].Value.ToString();
                form.ShowDialog();
                dgvJournalVouchar.Rows[i].Cells["OB_Stock_Wt"].Value = ioneNet.OrderManagement.Transactions.ProdSpecs.TotQty;
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
