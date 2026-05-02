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
using Ione_DAL;
using System.Diagnostics;
using System.IO;
using Excel = Microsoft.Office.Interop.Excel;
namespace ioneNet.HumanResourceManagement.Transactions
{
    public partial class frmVariable_Allowences_Entry : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string SONo, ItemCode, RecQty, Suppname;
        public frmVariable_Allowences_Entry()
        {
            InitializeComponent();
        }

        private void frmOpeningStock_Load(object sender, EventArgs e)
        {
            try
            {
                //Bind EMP Groups
                var bindTypes = (from m in db.HR_Employee_Groups
                                 where m.Company_ID == logIn.company
                                 select new
                                 {
                                     m.Group_Name,
                                     m.ID,
                                 }).ToList();

                if (bindTypes.Count > 0)
                {
                    cmbEmpGroup.DataSource = bindTypes;
                    cmbEmpGroup.DisplayMember = "Group_Name";
                    cmbEmpGroup.ValueMember = "ID";
                    cmbEmpGroup.SelectedIndex = -1;

                }

                //Bind Heads
                var bindHeads = (from m in db.HR_Salary_HeadsInfos
                                 where m.Company_ID == logIn.company && m.variable_head == true
                                 select new
                                 {
                                     m.Short_Name,
                                     m.id,
                                 }).ToList();

                if (bindHeads.Count > 0)
                {
                    cmbHeadName.DataSource = bindHeads;
                    cmbHeadName.DisplayMember = "Short_Name";
                    cmbHeadName.ValueMember = "id";
                    cmbHeadName.SelectedIndex = -1;

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
                if(cmbEmpGroup.Text=="")
                {
                    MessageBox.Show("Select Employee Group Proceed");
                    cmbEmpGroup.Focus();
                    return;

                }
                    SqlCommand cmd2 = new SqlCommand("SP_HR_Get_Employee_OtherEarnings", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@compname", logIn.company);
                    if (chkAllAccounts.Checked)
                    {
                        cmd2.Parameters.AddWithValue("@empgroup", Convert.ToInt32(cmbEmpGroup.SelectedValue.ToString()));
                    }
                    else
                    {
                        cmd2.Parameters.AddWithValue("@empgroup", 0);
                    }
                    cmd2.Parameters.AddWithValue("@buid", logIn.BU_ID);
                    cmd2.Parameters.AddWithValue("@month", cmbMonth.SelectedIndex + 1);
                    cmd2.Parameters.AddWithValue("@year", cmbYear.Text);
                    cmd2.Parameters.AddWithValue("@headname", cmbHeadName.Text);

                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    //DataSet ds2 = new DataSet();
                    DataTable ds2 = new DataTable();
                    // da2.Fill(ds2, "x");
                    da2.Fill(ds2);
                    dgvJournalVouchar.DataSource = ds2;
                //}
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {

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
                if ((from u in db.HR_Month_Other_Earnings where u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID && u.Month == cmbMonth.Text && u.Year == cmbYear.Text && u.Emp_Group == Convert.ToInt32(cmbEmpGroup.SelectedValue) && u.Head == cmbHeadName.Text select u).Count() > 0)
                {

                    //if (AppCode.GlobalAccess.Edit == "Yes")
                    //{
                    db.sp_DeleteAllowences_Entry(cmbMonth.Text,cmbYear.Text,  logIn.company,logIn.BU_ID, Convert.ToInt32(cmbEmpGroup.SelectedValue), cmbHeadName.Text);
                    

                }
                //else
                //{
                    try
                    {
                    string D = "01";
                    int m = cmbMonth.SelectedIndex + 1;
                    string Y = cmbYear.Text;
                    
                    string ed = Y + "-" + m + "-" + D;

                        //if (AppCode.GlobalAccess.Add == "Yes")
                        //{
                        for (int i = 0; i < dgvJournalVouchar.Rows.Count - 1; i++)
                        {

                            HR_Month_Other_Earning p = new HR_Month_Other_Earning();
                            p.Company_ID = logIn.company;
                            p.BU_ID = logIn.BU_ID;
                            p.Month = cmbMonth.Text;
                            p.Year = cmbYear.Text;                            
                            p.Emp_Group = Convert.ToInt32(cmbEmpGroup.SelectedValue);
                            p.Emp_Master_ID = Convert.ToInt32(dgvJournalVouchar.Rows[i].Cells["ID"].Value.ToString());
                            p.Head = cmbHeadName.Text;
                            p.Amount = (dgvJournalVouchar.Rows[i].Cells["Amount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvJournalVouchar.Rows[i].Cells["Amount"].Value); ;
                          
                            
                            p.Created_By = lblCreatedBy.Text;
                            p.Modified_BY = logIn.username + "-" + DateTime.Now;

                            db.HR_Month_Other_Earnings.InsertOnSubmit(p);

                       
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

                //}
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
                //var auto = db.Sp_autoincrement_Prod_OpeningStock(logIn.company);
                //txtVoucherNo.Text = auto.FirstOrDefault().Voucher_No;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " While AutoIncrement Getting Error ", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label33_Click(object sender, EventArgs e)
        {

        }

        private void cmbEmpGroup_Leave(object sender, EventArgs e)
        {
            try
            {
                //if(cmbEmpGroup.Text !="")
                //{
                //    //Bind EMP Groups
                //    var bindTypes = (from m in db.HR_Employee_Master_Datas
                //                     where m.Company_ID == logIn.company && m.Emp_Group == Convert.ToInt32(cmbEmpGroup.SelectedValue)
                //                     select new
                //                     {
                //                         m.Emp_Name,
                //                         m.id,
                //                     }).ToList();

                //    if (bindTypes.Count > 0)
                //    {
                //        cmbHeadName.DataSource = bindTypes;
                //        cmbHeadName.DisplayMember = "Emp_Name";
                //        cmbHeadName.ValueMember = "id";
                //        cmbHeadName.SelectedIndex = -1;

                //    }
                //}
            }
            catch (Exception ex)
            {
            }
        }

        private void dgvJournalVouchar_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string comnpstatecode, suppStateCode;
                DataGridViewRow R1 = dgvJournalVouchar.Rows[dgvJournalVouchar.CurrentRow.Index];
                int columnIndex = dgvJournalVouchar.CurrentCell.ColumnIndex;
                string columnName = dgvJournalVouchar.Columns[columnIndex].Name;

                if (columnName == "LOP" || columnName == "Leaves")
                {

                    decimal TotDays, PDays, Sdays, HDays, LDays, LOP, EarnDays;
                    TotDays = Convert.ToDecimal(R1.Cells["Month_Days"].Value);
                    LOP = Convert.ToDecimal(R1.Cells["LOP"].Value);
                    if (LOP == TotDays)
                    {

                        R1.Cells["Sundays"].Value = 0;
                        R1.Cells["Holidays"].Value = 0;
                    }
                    Sdays = Convert.ToDecimal(R1.Cells["Sundays"].Value);
                    HDays = Convert.ToDecimal(R1.Cells["Holidays"].Value);
                    LDays = Convert.ToDecimal(R1.Cells["LeavesUsed"].Value);
                    
                    
                    R1.Cells["P_Days"].Value = TotDays - Sdays- HDays- LDays - LOP;
                    PDays = Convert.ToDecimal(R1.Cells["P_Days"].Value);
                    EarnDays = PDays + Sdays + HDays + LDays;
                    R1.Cells["Total_Days"].Value = EarnDays;
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

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void cmdPrint_Click(object sender, EventArgs e)
        {
            if (dgvJournalVouchar.Rows.Count > 0)
            {
                ExportToExcel(dgvJournalVouchar, "Variable Allowences");
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
                xlWorkSheetToExport.Cells[2, 1] = "Variable Allowences";


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

        private void brnSearch_Click(object sender, EventArgs e)
        {

            try
            {
                //  bindCashAct();
                MaterialManagement.Masters.FG_OBStockVouchers obj = new MaterialManagement.Masters.FG_OBStockVouchers();

                if (obj.ShowDialog() == DialogResult.OK)
                {
                    //txtVoucherNo.Text = MaterialManagement.Masters.OBStockVouchers.voucherNo;

                    //if (!string.IsNullOrEmpty(txtVoucherNo.Text))
                    //{
                        var dm1 = (from s in db.Product_OpeningStocks
                                       //  join a in db.Storage_Locations on s.Storage_Location equals a.Storage_Loc_Id
                                   where s.Company_ID == logIn.company 
                                   select new
                                   {
                                       s.Prod_ID,
                                       s.Prod_Name,
                                       Uom_Descr=s.UOM,
                                       Prod_Unit_Wt=s.Unit_wt,
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


                    }

                    //var f = (from s in db.Account_Opening_Balances where s.Voucher_no == txtVoucherNo.Text && s.Company_ID == logIn.company select s).FirstOrDefault();
                    //if (f != null)
                    //{
                    //    AsAtdate.Text = f.OB_date.ToString();
                    //    //cmbAccountName.Text = f.Account;
                    //    txtRemarks.Text = f.Remarks;
                    //    //ObDate.Text = f.OBDate.ToString();
                    //    //txtCreditAmtTotal.Text = f.TotalCreditAmt.ToString();
                    //    //txtDebitAmtTotal.Text = f.TotalDebitAmt.ToString();

                    //}
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

                //}

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
                ioneNet.OrderManagement.Transactions.ProdSpecs form = new ioneNet.OrderManagement.Transactions.ProdSpecs();
                //ioneNet.Masters.ProdSearch.frmName = "SOrder";       
                int i = dgvJournalVouchar.CurrentCell.RowIndex;
                SONo = "OB";
                ItemCode = dgvJournalVouchar.Rows[i].Cells["Prod_ID"].Value.ToString();
                //RecQty = dgProducts.Rows[i].Cells["ReceivedQty"].Value.ToString();
                form.ShowDialog();
                dgvJournalVouchar.Rows[i].Cells["OB_Stock_Wt"].Value = ioneNet.OrderManagement.Transactions.ProdSpecs.TotQty;
            }
        }
    }
}
