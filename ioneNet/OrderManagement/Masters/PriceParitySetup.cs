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
namespace ioneNet.OrderManagement.Masters
{
    public partial class PriceParitySetup : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public PriceParitySetup()
        {
            InitializeComponent();
        }

        private void btnGetAccounts_Click(object sender, EventArgs e)
        {
            try
            {

                SqlCommand cmd2 = new SqlCommand("Get_Product_PriceList", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);
                
                cmd2.Parameters.AddWithValue("@ProdGroup", Convert.ToInt32(cmbProdGroup.SelectedValue.ToString()));
                cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataSet ds2 = new DataSet();
                DataTable ds2 = new DataTable();
                // da2.Fill(ds2, "x");
                da2.Fill(ds2);
                dgvJournalVouchar.DataSource = ds2;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void brnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                //  bindCashAct();
                OrderManagement.Masters.PriceListVouchers obj = new OrderManagement.Masters.PriceListVouchers();

                if (obj.ShowDialog() == DialogResult.OK)
                {
                    txtVoucherNo.Text = OrderManagement.Masters.PriceListVouchers.voucherNo;

                    if (!string.IsNullOrEmpty(txtVoucherNo.Text))
                    {
                        var dm1 = (from s in db.Prod_PriceLists
                                       //  join a in db.Storage_Locations on s.Storage_Location equals a.Storage_Loc_Id
                                   where s.Voucher_no == txtVoucherNo.Text && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID
                                   select new
                                   {
                                       s.Prod_ID,
                                       s.Prod_Code,
                                       s.Prod_Name,
                                       Uom_Descr = s.UOM,
                                       MRP = s.MRP,                                      
                                       s.Remarks



                                   });

                        SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                        SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                        DataTable dtr = new DataTable();
                        da2.Fill(dtr);
                        if (dtr.Rows.Count >= 0)
                            dgvJournalVouchar.DataSource = dtr;



                        var dm2 = (from s in db.Prod_PriceLists
                                       //  join a in db.Storage_Locations on s.Storage_Location equals a.Storage_Loc_Id
                                   where s.Voucher_no == txtVoucherNo.Text && s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID
                                   select new
                                   {
                                       s.Price_Allow_Per,
                                       s.Effective_date,
                                       s.Exchange_Rate,
                                       s.Link_USD_Rate
                                   }).ToList();

                      
                        AsAtdate.Text = dm2[0].Effective_date.ToString();
                     
                        
                    }


                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                //if (cmbStorageLocation.Text == "")
                //{
                //    MessageBox.Show("Ware House / Storage Location Should not be empty");
                //    cmbStorageLocation.Focus();
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
                if ((from u in db.Prod_PriceLists where u.Voucher_no == txtVoucherNo.Text && u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID select u).Count() > 0)
                {

                    for (int i = 0; i < dgvJournalVouchar.Rows.Count - 1; i++)
                    {
                        int PId = Convert.ToInt32(dgvJournalVouchar.Rows[i].Cells["Prod_ID"].Value.ToString());
                        if ((from u in db.Prod_PriceLists where u.Prod_ID == PId && u.Voucher_no == txtVoucherNo.Text && u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID select u).Count() > 0)
                        {
                            var p1 = db.Prod_PriceLists.Where(w => w.Voucher_no == txtVoucherNo.Text && w.Prod_ID == PId && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();

                            p1.Voucher_no = txtVoucherNo.Text;
                            p1.Effective_date = AsAtdate.Value;
                            p1.Company_ID = logIn.company;
                            p1.Prod_ID = Convert.ToInt32(dgvJournalVouchar.Rows[i].Cells["Prod_ID"].Value.ToString());
                            p1.Prod_Code = dgvJournalVouchar.Rows[i].Cells["Prod_Code"].Value.ToString();
                            p1.Prod_Name = dgvJournalVouchar.Rows[i].Cells["Prod_Name"].Value.ToString();
                            p1.UOM = dgvJournalVouchar.Rows[i].Cells["Uom_Descr"].Value.ToString();
                            p1.MRP = (dgvJournalVouchar.Rows[i].Cells["MRP"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["MRP"].Value);
                            //p1.MRP_USD = (dgvJournalVouchar.Rows[i].Cells["MRP_USD"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["MRP_USD"].Value);
                            
                            p1.Remarks =  (dgvJournalVouchar.Rows[i].Cells["Remarks"].Value == null)?"":dgvJournalVouchar.Rows[i].Cells["Remarks"].Value.ToString();
                            p1.BU_ID = logIn.BU_ID;
                            p1.Created_By = lblCreatedBy.Text;
                            p1.Modified_BY = logIn.username + "-" + DateTime.Now;
                            db.SubmitChanges();
                        }
                        else
                        {
                            Prod_PriceList p = new Prod_PriceList();
                            p.Voucher_no = txtVoucherNo.Text;
                            p.Effective_date = AsAtdate.Value;
                            p.Company_ID = logIn.company;
                            p.Prod_ID = Convert.ToInt32(dgvJournalVouchar.Rows[i].Cells["Prod_ID"].Value.ToString());
                            p.Prod_Code = dgvJournalVouchar.Rows[i].Cells["Prod_Code"].Value.ToString();
                            p.Prod_Name = dgvJournalVouchar.Rows[i].Cells["Prod_Name"].Value.ToString();
                            p.UOM = dgvJournalVouchar.Rows[i].Cells["Uom_Descr"].Value.ToString();
                            p.MRP = (dgvJournalVouchar.Rows[i].Cells["MRP"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["MRP"].Value);
                            //p.MRP_USD = (dgvJournalVouchar.Rows[i].Cells["MRP_USD"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["MRP_USD"].Value);

                            p.Remarks = (dgvJournalVouchar.Rows[i].Cells["Remarks"].Value == null)?"": dgvJournalVouchar.Rows[i].Cells["Remarks"].Value.ToString();
                            p.BU_ID = logIn.BU_ID;
                            p.Created_By = logIn.username + "-" + DateTime.Now;
                            p.Modified_BY = logIn.username + "-" + DateTime.Now;
                            db.Prod_PriceLists.InsertOnSubmit(p);

                        }
                        db.SubmitChanges();


                    }
                    MessageBox.Show("Record Updated Successfully");
                    clear();
                }
                else
                {
                    try
                    {

                        //if (AppCode.GlobalAccess.Add == "Yes")
                        //{
                        for (int i = 0; i < dgvJournalVouchar.Rows.Count - 1; i++)
                        {
                            decimal stkqty = (dgvJournalVouchar.Rows[i].Cells["MRP"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["MRP"].Value);
                            if (stkqty >= 0)
                            {
                                Prod_PriceList p = new Prod_PriceList();
                                p.Voucher_no = txtVoucherNo.Text;
                                p.Effective_date = AsAtdate.Value;
                                p.Company_ID = logIn.company;
                                p.Prod_ID = Convert.ToInt32(dgvJournalVouchar.Rows[i].Cells["Prod_ID"].Value.ToString());
                                p.Prod_Code = dgvJournalVouchar.Rows[i].Cells["Prod_Code"].Value.ToString();
                                p.Prod_Name = dgvJournalVouchar.Rows[i].Cells["Prod_Name"].Value.ToString();
                                p.UOM = dgvJournalVouchar.Rows[i].Cells["Uom_Descr"].Value.ToString();
                                p.MRP = (dgvJournalVouchar.Rows[i].Cells["MRP"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["MRP"].Value);
                                //p.MRP_USD = (dgvJournalVouchar.Rows[i].Cells["MRP_USD"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["MRP_USD"].Value);

                                p.Remarks = (dgvJournalVouchar.Rows[i].Cells["Remarks"].Value == null) ? "" : dgvJournalVouchar.Rows[i].Cells["Remarks"].Value.ToString();
                                p.BU_ID = logIn.BU_ID;
                                p.Created_By = logIn.username + "-" + DateTime.Now;
                                p.Modified_BY = logIn.username + "-" + DateTime.Now;
                                db.Prod_PriceLists.InsertOnSubmit(p);

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

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                db.sp_DeletePriceList(txtVoucherNo.Text, logIn.company, logIn.BU_ID, 0, 0);

                MessageBox.Show("Record Deleted Successfully");
                clear();
            }
        }
        public void AutoincrementId()
        {
            try
            {
                var auto = db.Sp_autoincrement_Prod_PriceList(logIn.company, logIn.BU_ID);
                txtVoucherNo.Text = auto.FirstOrDefault().Voucher_No;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " While AutoIncrement Getting Error ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
            AutoincrementId();
            AsAtdate.Value = DateTime.Now;

            //dgvJournalVouchar.Rows.Clear();
            //dgvJournalVouchar.Refresh();
        }

        private void cmdPrint_Click(object sender, EventArgs e)
        {
            if (dgvJournalVouchar.Rows.Count > 0)
            {
                ExportToExcel(dgvJournalVouchar, "Price List");
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

                int iRowCnt = 4;
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
                xlWorkSheetToExport.Range["A1:F1"].MergeCells = true;       // MERGE CELLS OF THE HEADER.                
                xlWorkSheetToExport.Cells[2, 1] = "PRICE LIST - " + txtVoucherNo.Text;




                // SHOW THE HEADER File Name

                string d = " Effective From :" + AsAtdate.Text;
                xlWorkSheetToExport.Cells[2, 3] = d;
                Excel.Range range5 = xlWorkSheetToExport.Cells[2, 1] as Excel.Range;
                range5.EntireRow.Font.Name = "Calibri";
                //  range5.EntireRow.Font.Bold = true;
                range5.EntireRow.Font.Size = 12;

                for (int i = 1; i < gridviewID.Columns.Count + 1; i++)
                {
                    xlWorkSheetToExport.Cells[4, i] = gridviewID.Columns[i - 1].HeaderText;
                }

                for (int i = 0; i < gridviewID.Rows.Count; i++)
                {
                    for (int j = 0; j < gridviewID.Columns.Count; j++)
                    {
                        if (gridviewID.Rows[i].Cells[j].Value != null)
                        {
                            Excel.Range range7 = xlWorkSheetToExport.Cells[i + 5, j + 1] as Excel.Range;

                            range7.NumberFormat = "@";

                            xlWorkSheetToExport.Cells[i + 5, j + 1] = gridviewID.Rows[i].Cells[j].Value.ToString();

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

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PriceList_Load(object sender, EventArgs e)
        {
            AutoincrementId();
            var Buyerblind = (from m in db.Product_Groups where m.Company_ID == logIn.company select new { m.ID, m.Prod_Group_Name }).Distinct().ToList();
            if (Buyerblind.Count > 0)
            {
                cmbProdGroup.DataSource = Buyerblind;
                cmbProdGroup.ValueMember = "ID";
                cmbProdGroup.DisplayMember = "Prod_Group_Name";

                //CmbConsigneeName.DataSource = Buyerblind;
                //CmbConsigneeName.ValueMember = "ID";
                //CmbConsigneeName.DisplayMember = "Customer_Alias_Name";

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
            dt.Columns.Add(new DataColumn("MRP", typeof(string)));
            dt.Columns.Add(new DataColumn("Remarks", typeof(string)));
            for (int i = 0; i < count; i++)
            {
                dr = dt.NewRow();
                dr["Prod_ID"] = DtSet.Rows[i]["Item ID"].ToString(); ;
                dr["Prod_Code"] = DtSet.Rows[i]["Product Code"].ToString();
                dr["Prod_Name"] = DtSet.Rows[i]["Product Name"].ToString();
                dr["Uom_Descr"] = DtSet.Rows[i]["UOM"].ToString();
                dr["MRP"] = DtSet.Rows[i]["MRP"].ToString(); ;
                dr["Remarks"] = DtSet.Rows[i]["Remarks"].ToString();
                dt.Rows.Add(dr);
            }
            //dgProductData.DataSource = dt;
            dgvJournalVouchar.DataSource = dt;
            MyConnection.Close();
            //for (int i = 0; i < dt.Rows.Count - 1; i++)
            //{
            //    Prod_PriceList p = new Prod_PriceList();
            //    p.Prod_Code = dt.Rows[i]["Prod_Code"].ToString();
            //    p.Prod_Group_Id = Convert.ToInt32(dt.Rows[i]["Prod_Group_Id"].ToString());
            //    p.Prod_Type_Id = Convert.ToInt32(dt.Rows[i]["Prod_Type_Id"].ToString());
            //    p.Prod_Name = dt.Rows[i]["Prod_Name"].ToString();
            //    p.Prod_Storage_Location_Id = (dt.Rows[i]["Prod_Unit_Wt"].ToString() == "") ? Convert.ToInt32("0") : Convert.ToInt32(dt.Rows[i]["Prod_Storage_Location_Id"].ToString());
            //    p.Prod_Primary_UOM_Id = Convert.ToInt32(dt.Rows[i]["Prod_Primary_UOM_Id"].ToString());
            //    p.Prod_Unit_Wt = (dt.Rows[i]["Prod_Unit_Wt"].ToString() == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(dt.Rows[i]["Prod_Unit_Wt"].ToString());
            //    p.Prod_Tax_Class = Convert.ToInt32(dt.Rows[i]["Prod_Tax_Class"].ToString());
            //    p.Prod_Description = (dt.Rows[i]["Prod_Description"].ToString() == "") ? "" : dt.Rows[i]["Prod_Description"].ToString();
            //    p.Prod_HSN_Code = (dt.Rows[i]["Prod_HSN_Code"].ToString() == "") ? "" : dt.Rows[i]["Prod_HSN_Code"].ToString();
            //    p.Prod_Status_ID = Convert.ToInt32(dt.Rows[i]["Prod_Status_ID"].ToString());
            //    p.Prod_Mfg_Code = (dt.Rows[i]["Prod_Mfg_Code"].ToString() == "") ? "" : dt.Rows[i]["Prod_Mfg_Code"].ToString();
            //    p.Created_By = logIn.username + "-" + DateTime.Now;
            //    p.Modified_BY = logIn.username + "-" + DateTime.Now;
            //    p.Company_ID = logIn.company;
            //    db.Products.InsertOnSubmit(p);
            //    db.SubmitChanges();



            //}
        }

        private void dgvJournalVouchar_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {


                int columnIndex = dgvJournalVouchar.CurrentCell.ColumnIndex;
                string columnName = dgvJournalVouchar.Columns[columnIndex].HeaderText;

                TextBox tb3 = e.Control as TextBox;
                if (columnName == "Product Name")
                {
                    if (tb3 != null && columnName == "Product Name")
                    {
                        tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                        tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                        addItems(DataColl);
                        tb3.AutoCompleteCustomSource = DataColl;
                    }


                }
                else
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.None;
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

                if (columnName == "Product Name")
                {
                    var Buyerbind = (from m in db.Products
                                     where m.Company_ID == logIn.company
                                     select new
                                     { m.Prod_Name }).ToList();
                    //var Prodname = (from d in db.ProdMasters where d.Comp_Name == AppCode.GlobalAccess.companyName && d.Product_Name == R1.Cells["ProductName"].Value.ToString() select new { d.Product_Category }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Prod_Name");
                    foreach (var item in Buyerbind)
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
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvJournalVouchar_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                DataGridViewRow R1 = dgvJournalVouchar.Rows[dgvJournalVouchar.CurrentRow.Index];
                int columnIndex = dgvJournalVouchar.CurrentCell.ColumnIndex;
                string columnName = dgvJournalVouchar.Columns[columnIndex].Name;

                if (columnName == "Prod_Name" && (R1.Cells["Prod_Name"].Value) != "" && (R1.Cells["Prod_Name"].Value) != null && (R1.Cells["Prod_Name"].Value) != DBNull.Value)
                {
                    if ((from u in db.Products where u.Prod_Name == R1.Cells["Prod_Name"].Value select u).Count() == 0)
                    {
                        MessageBox.Show("Please  select the valid the Product Name");
                        R1.Cells["Prod_Name"].Value = "";
                        return;
                    }
                    else
                    {
                        var getProductName = (from s in db.Products
                                              join u in db.UoM_Masters on s.Prod_Primary_UOM_Id equals u.UOM_ID
                                              where s.Prod_Name == R1.Cells["Prod_Name"].Value.ToString() && s.Company_ID == logIn.company
                                              select new { s.prod_ID, s.Prod_Code, u.Uom_Descr }).FirstOrDefault();

                        if (getProductName != null)
                        {

                            R1.Cells["Prod_ID"].Value = getProductName.prod_ID.ToString();
                            R1.Cells["Prod_Code"].Value = getProductName.Prod_Code.ToString();
                            R1.Cells["Uom_Descr"].Value = getProductName.Uom_Descr.ToString();
                        }

                    }
                }
                if (columnName == "MRP")
                {
                    
                    //decimal MRP_INR = (R1.Cells["MRP"].Value == "" || R1.Cells["MRP"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["MRP"].Value);
                    //R1.Cells["MRP_USD"].Value = "0";
                    //if (checkBox1.Checked)
                    //{
                    //    decimal eRate = Convert.ToDecimal(txtExchangeRate.Text);
                    //    R1.Cells["MRP_USD"].Value = (MRP_INR / eRate).ToString("0.00");
                    //}
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dgvJournalVouchar_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F6)
                {
                    if (dgvJournalVouchar.Rows.Count > 0)
                    {

                        foreach (DataGridViewCell oneCell in dgvJournalVouchar.SelectedCells)
                        {
                            if (oneCell.Selected)
                                db.sp_DeletePriceList(txtVoucherNo.Text,logIn.company,logIn.BU_ID,Convert.ToInt32(dgvJournalVouchar.Rows[oneCell.RowIndex].Cells["Prod_ID"].Value), 1);
                            dgvJournalVouchar.Rows.RemoveAt(oneCell.RowIndex);
                        }
                        
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void lblModified_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }
    }
}
