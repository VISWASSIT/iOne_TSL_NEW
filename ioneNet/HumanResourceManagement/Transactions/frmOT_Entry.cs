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
using Excel = Microsoft.Office.Interop.Excel;
using System.IO;
using System.Diagnostics;
using System.Data.OleDb;
using Syncfusion.CompoundFile.XlsIO.Native;
namespace ioneNet.HumanResourceManagement.Transactions
{
    public partial class frmOT_Entry : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string SONo, ItemCode, RecQty, Suppname;
        public frmOT_Entry()
        {
            InitializeComponent();
        }

        private void frmOpeningStock_Load(object sender, EventArgs e)
        {
            try
            {
                AutoincrementId();
                //Bind EMP Groups
                var bindTypes = (from m in db.Department_Masters
                                 where m.Company_ID == logIn.company
                                 select new
                                 {
                                     m.Dept_Name,
                                     m.Id,
                                 }).ToList();

                if (bindTypes.Count > 0)
                {
                    cmbEmpGroup.DataSource = bindTypes;
                    cmbEmpGroup.DisplayMember = "Dept_Name";
                    cmbEmpGroup.ValueMember = "id";
                    cmbEmpGroup.SelectedIndex = -1;

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
                if(checkBox2.Checked)
                {
                    var dm1 = (from d in db.HR_Employee_Master_Datas
                               join p in db.Department_Masters on d.Department equals p.Id
                               where  d.Company_ID == logIn.company && d.OTEligible == true
                               orderby p.Dept_Name
                               select new
                               {
                                   ID = d.id,
                                   d.Emp_Code,
                                   Emp_Name = d.Emp_Name,
                                   Department = p.Dept_Name  ,
                                   No_of_Hrs = 0


                               });
                    SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    DataTable dtr = new DataTable();
                    da2.Fill(dtr);
                    if (dtr.Rows.Count >= 0)
                    {
                        dgvEmployees.DataSource = dtr;
                    }
                    else
                    {
                        //dgItemDetails.DataSource = null;
                    }
                }
                else
                { 
                    if (cmbEmpGroup.Text == "")
                    {
                        MessageBox.Show("Select Employee Department To Proceed");
                        cmbEmpGroup.Focus();
                        return;

                    }
                    var dm1 = (from d in db.HR_Employee_Master_Datas
                               join p in db.Department_Masters on d.Department equals p.Id
                           

                               where d.Department == Convert.ToInt32(cmbEmpGroup.SelectedValue) && d.Company_ID == logIn.company && d.OTEligible == true
                               orderby d.id
                               select new
                               {
                                   ID = d.id,
                                   d.Emp_Code,
                                   Emp_Name = d.Emp_Name,
                                   Department = p.Dept_Name,
                                   No_of_Hrs = 0                              


                               });
                    SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    DataTable dtr = new DataTable();
                    da2.Fill(dtr);
                    if (dtr.Rows.Count >= 0)
                    {
                        dgvEmployees.DataSource = dtr;
                    }
                    else
                    {
                        //dgItemDetails.DataSource = null;
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
            try
            {
                
                HumanResourceManagement.Transactions.OT_Entry_Search obj = new HumanResourceManagement.Transactions.OT_Entry_Search();
                if (obj.ShowDialog() == DialogResult.OK)
                {
                    // cmbMONo.Text = ProductionManagement.Masters.Bom_Project_Search.MoNO;
                    txtVchNo.Text = HumanResourceManagement.Transactions.OT_Entry_Search.Proj_Code;
                    var dm = (from s in db.HR_OT_Entries
                               where s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID && s.Vch_no == txtVchNo.Text


                               select new

                               {
                                   s.e_Date,
                                   s.Emp_Dept,                                   
                                   s.ISCoff,
                                  s.Remarks

                               }).ToList();


                    dateTimePicker1.Text = dm[0].e_Date.ToString();
                    cmbEmpGroup.Text = dm[0].Emp_Dept;
                    textBox1.Text = dm[0].Remarks;
                    checkBox1.Checked = false;
                    if(dm[0].ISCoff == true)
                    {
                        checkBox1.Checked = true;
                    }


                    var dm1 = (from s in db.HR_OT_Entries
                               join emp in db.HR_Employee_Master_Datas on s.Emp_Master_ID equals emp.id
                               join d in db.Department_Masters on emp.Department equals d.Id
                               where s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID && s.Vch_no == txtVchNo.Text


                               select new

                               {
                                   ID = s.Emp_Master_ID,
                                   Emp_Code = emp.Emp_Code,
                                   Emp_Name = emp.Emp_Name,
                                   Department = d.Dept_Name,
                                   No_of_Hrs = s.OT_Hrs,
                                   s.Remarks




                               });




                    SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    DataTable dtr = new DataTable();
                    da2.Fill(dtr);
                    if (dtr.Rows.Count >= 0)
                    {
                        dgvEmployees.DataSource = dtr;
                    }
                    else
                    {
                        //dgvJournalVouchar.DataSource = null;
                    }




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
                if ((from u in db.HR_OT_Entries where u.Vch_no == txtVchNo.Text && u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID select u).Count() > 0)
                {

                    //if (AppCode.GlobalAccess.Edit == "Yes")
                    //{
                    db.sp_DeleteOT_Entry(dateTimePicker1.Value, logIn.company, logIn.BU_ID, txtVchNo.Text);

                }
                //else
                //{
                    try
                    {
                    //string D = "01";
                    //int m = cmbMonth.SelectedIndex + 1;
                    //string Y = cmbYear.Text;
                    
                    //string ed = Y + "-" + m + "-" + D;

                        //if (AppCode.GlobalAccess.Add == "Yes")
                        //{
                        for (int i = 0; i < dgvEmployees.Rows.Count - 1; i++)
                        {

                            HR_OT_Entry p = new HR_OT_Entry();
                            p.Company_ID = logIn.company;
                            p.BU_ID = logIn.BU_ID;
                            p.e_Date = dateTimePicker1.Value;
                            p.Vch_no = txtVchNo.Text;
                            p.ISCoff = checkBox1.Checked;
                            p.Emp_Dept = cmbEmpGroup.Text;
                            p.Remarks = (dgvEmployees.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgvEmployees.Rows[i].Cells["Remarks"].Value).ToString();
                            p.Emp_Master_ID = Convert.ToInt32(dgvEmployees.Rows[i].Cells["ID"].Value.ToString());
                           
                            p.OT_Hrs = (dgvEmployees.Rows[i].Cells["No_of_Hrs"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgvEmployees.Rows[i].Cells["No_of_Hrs"].Value); ;
                          
                            
                            p.Created_By = lblCreatedBy.Text;
                            p.Modified_BY = logIn.username + "-" + DateTime.Now;

                            db.HR_OT_Entries.InsertOnSubmit(p);

                       
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
        public void AutoincrementId()
        {
            try
            {

                var result = db.Sp_autoincrement_HR_OTEntry(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date);
                txtVchNo.Text = result.FirstOrDefault().Report_No;
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
                if (dgvEmployees.Rows.Count > 0)
                {
                    for (int i = 0; i < dgvEmployees.Rows.Count - 1; i++)
                    {
                        dgvEmployees.Rows.RemoveAt(i);
                        i--;
                        while (dgvEmployees.Rows.Count == 0)
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
                //if ((from u in db.HR_OT_Entries where u.Company_ID == logIn.company && u.BU_ID == logIn.BU_ID && u.Vch_no == txtVchNo.Text select u).Count() > 0)
                //{
                //    var dm1 = (from s in db.HR_OT_Entries
                //               join emp in db.HR_Employee_Master_Datas on s.Emp_Master_ID equals emp.id
                //               where s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID  && s.Vch_no == txtVchNo.Text


                //               select new

                //               {
                //                   ID = s.Emp_Master_ID,
                //                   Emp_Code = emp.Emp_Code,
                //                   Emp_Name = emp.Emp_Name,
                //                   No_of_Hrs = s.OT_Hrs,
                                   



                //               });




                //    SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                //    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //    DataTable dtr = new DataTable();
                //    da2.Fill(dtr);
                //    if (dtr.Rows.Count >= 0)
                //        {
                //            dgvJournalVouchar.DataSource = dtr;
                //        }
                //    else
                //    {
                //        //dgvJournalVouchar.DataSource = null;
                //    }
                //}
                //else
                //{
                //    if (dgvJournalVouchar.Rows.Count > 0)
                //    {
                //        for (int i = 0; i < dgvJournalVouchar.Rows.Count - 1; i++)
                //        {
                //            dgvJournalVouchar.Rows.RemoveAt(i);
                //            i--;
                //            while (dgvJournalVouchar.Rows.Count == 0)
                //                continue;
                //        }
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
                DataGridViewRow R1 = dgvEmployees.Rows[dgvEmployees.CurrentRow.Index];
                int columnIndex = dgvEmployees.CurrentCell.ColumnIndex;
                string columnName = dgvEmployees.Columns[columnIndex].Name;

                if (columnName == "Emp_Code")
                {
                    var Prodname = (from d in db.HR_Employee_Master_Datas
                                    where d.Company_ID == logIn.company && d.Emp_Code == R1.Cells["Emp_Code"].Value.ToString() 


                                    select new { d.Emp_Name,d.id }).ToList();
                    if (Prodname.Count > 0)
                    {
                        R1.Cells["Emp_Name"].Value = Prodname[0].Emp_Name;
                        R1.Cells["ID"].Value = Prodname[0].id;
                    }
                    else
                    {
                        MessageBox.Show("Invalid Employee Code Entered");
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

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                db.sp_DeleteOT_Entry (dateTimePicker1.Value, logIn.company, logIn.BU_ID, txtVchNo.Text);
                MessageBox.Show("Record Deleted");
                clear();
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void cmdPrint_Click(object sender, EventArgs e)
        {
            if (dgvEmployees.Rows.Count > 0)
            {
                ExportToExcel(dgvEmployees, "Opening Stock");
            }
        }

        private void dgvJournalVouchar_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // DataGridViewRow dgProductData.Rows[i] = dgProductData.Rows[dgProductData.CurrentRow.Index];
                if (dgvEmployees.Rows.Count > 1)
                {
                    if (dgvEmployees.Rows[dgvEmployees.CurrentRow.Index].Cells[dgvEmployees.CurrentCell.ColumnIndex].Value == "Remove")
                    {
                        if (dgvEmployees.Rows.Count > 0)
                        {
                            foreach (DataGridViewCell oneCell in dgvEmployees.SelectedCells)
                            {
                                if (oneCell.Selected)
                                    dgvEmployees.Rows.RemoveAt(oneCell.RowIndex);
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
                            dgvEmployees.DataSource = dtr;


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
                int i = dgvEmployees.CurrentCell.RowIndex;
                SONo = "OB";
                ItemCode = dgvEmployees.Rows[i].Cells["Prod_ID"].Value.ToString();
                //RecQty = dgProducts.Rows[i].Cells["ReceivedQty"].Value.ToString();
                form.ShowDialog();
                dgvEmployees.Rows[i].Cells["OB_Stock_Wt"].Value = ioneNet.OrderManagement.Transactions.ProdSpecs.TotQty;
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
                xlWorkSheetToExport.Cells[2, 1] = "Leave Allotment Data";


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

    }
}
