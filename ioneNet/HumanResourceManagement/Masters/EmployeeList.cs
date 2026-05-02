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
using System.Data.Linq.SqlClient;
using Syncfusion.WinForms.GridCommon.ScrollAxis;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.Data;
using System.Data.OleDb;
using Ione_DAL;
using Syncfusion.WinForms.DataGridConverter;
using System.Diagnostics;

namespace ioneNet.HumanResourceManagement.Masters
{
    public partial class EmployeeList : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public static int productCode;
        public static string var;
        public EmployeeList()
        {
            InitializeComponent();
        }

       

        private void ProductList_Load(object sender, EventArgs e)
        {
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

            BindProductslist();

        }
        void sfDataGrid_QueryCellStyle(object sender, Syncfusion.WinForms.DataGrid.Events.QueryCellStyleEventArgs e)
        {
            var rowColumnIndex = new RowColumnIndex(e.RowIndex, e.ColumnIndex);
            if (colorDict.ContainsKey(rowColumnIndex))
                e.Style.BackColor = colorDict[rowColumnIndex];
        }


        void SetCellBackgroundColor(RowColumnIndex rowColumnIndex, Color color)
        {
            //if (!colorDict.ContainsKey(rowColumnIndex))
            //    colorDict.Add(rowColumnIndex, color);
            //else
            //    colorDict[rowColumnIndex] = color;
            //sfDataGrid1.TableControl.Invalidate(this.sfDataGrid1.TableControl.GetCellRectangle(rowColumnIndex.RowIndex, rowColumnIndex.ColumnIndex, false));
        }

        public void BindProductslist()
        {
            try
            {
                //sfDataGrid1.DataSource=Refresh();
               
                
                var d = (from data in db.ShowEmployeesList(logIn.company,logIn.BU_ID) select data).ToList();
                if (d.Count > 0)
                {
                    
                    sfDataGrid1.DataSource = d;


                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["EMP_ID"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["EMP_ID"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["EMP_ID"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["EMP_ID"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["EMP_Code"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["EMP_Code"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["EMP_Code"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["EMP_Code"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["EMP_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["EMP_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["EMP_Name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["EMP_Name"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.TableSummaryRows.Clear();
                    GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                    tableSummaryRow1.Name = "TableSummary";
                    tableSummaryRow1.ShowSummaryInRow = false;
                    tableSummaryRow1.Position = VerticalPosition.Bottom;

                    GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                    summaryColumn1.Name = "Total Employees";
                    summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn1.Format = "Total Employees: {Count}";
                    summaryColumn1.MappingName = "EMP_ID";

                    tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

                    this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);
                }
                DateTime dt = DateTime.Now;
                string dt1 = dt.ToString("yyyy/MM/dd");

                int cr, a, p, d1, cl,c2;
                var cnt = (from s in db.employeeCount(dt,  logIn.BU_ID, logIn.company) select s).ToList();
                //var d = (from data in db.ShowSOList(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, null) select data).ToList();
                if (cnt.Count > 0)
                {
                    //int C = cnt[0].Created.Value;
                    if (cnt[0].Emp_Joined_Month == null)
                    {
                        cr = 0;
                    }
                    else
                    {
                        cr = cnt[0].Emp_Joined_Month;
                    }
                    if (cnt[0].Emp_Joined_Year == null)
                    {
                        a = 0;
                    }
                    else
                    {
                        a = cnt[0].Emp_Joined_Year;
                    }
                    if (cnt[0].Emp_Resigned_Month == null)
                    {
                        d1 = 0;
                    }
                    else
                    {
                        d1 = cnt[0].Emp_Resigned_Month;
                    }
                    if (cnt[0].Emp_Resigned_Year == null)
                    {
                        cl = 0;
                    }
                    else
                    {
                        cl = cnt[0].Emp_Resigned_Year;
                    }
                    if (cnt[0].AgeAbove58 == null)
                    {
                        c2 = 0;
                    }
                    else
                    {
                        c2 = cnt[0].AgeAbove58;
                    }
                   
                    button1.Text = "Employee >58 Yrs " + "\n" + (c2);
                    button2.Text = "New Joinees  " + "\n" + (cr + "/"+ a);
                    button3.Text = "Employees Resigned " + "\n" + (d1 + "/" + cl);
                    //    + Convert.ToUInt32(cnt[0].Despatches_Started.ToString()) + Convert.ToUInt32(cnt[0].Closed.ToString());
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            Masters.frmEmployeesNew frm = new Masters.frmEmployeesNew();
            //frm.MdiParent = this.MdiParent;
            var = "1";
            frm.ShowDialog();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void chart2_Click(object sender, EventArgs e)
        {

        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                int i = sfDataGrid1.CurrentCell.RowIndex;
                if (i >= 0)
                {

                    var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["EMP_ID"].MappingName;
                    var mappingName1 = sfDataGrid1.Columns["Status"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                    //var currentCellV00alue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                    if (cellVaue1 != "Resigned")
                    {
                        productCode = Convert.ToInt32(cellVaue.ToString());
                        var = "0";
                        ioneNet.HumanResourceManagement.Masters.frmEmployeesNew frm = new ioneNet.HumanResourceManagement.Masters.frmEmployeesNew();
                        frm.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Employee Status is Resigned, Data Cannot be Modified");
                    }

                }
                else
                {
                    MessageBox.Show("Please Select Any One Product");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
           
        }

        private void deactivateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
               
                //Transferred
                var pscrap = (from m in db.Costing_Units where m.Company == logIn.company select new { m.id, m.BU_Name }).Distinct().ToList();
                if (pscrap.Count > 0)
                {
                    cmbTransferredTo.DataSource = pscrap;
                    cmbTransferredTo.ValueMember = "id";
                    cmbTransferredTo.DisplayMember = "BU_Name";
                    cmbTransferredTo.SelectedIndex = -1;
                }
                var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Emp_Status" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbEmpStatus.DataSource = pStatus;
                    cmbEmpStatus.ValueMember = "ID";
                    cmbEmpStatus.DisplayMember = "Descr";
                }
                groupBox1.Visible = true;
                cmbEmpStatus.Focus();

                int i = sfDataGrid1.CurrentCell.RowIndex;
                if (i >= 0)
                {
                    //var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    //var mappingName = sfDataGrid1.Columns["Alternative_Code"].MappingName;
                    //var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    ////var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                    //productCode = Convert.ToInt32(cellVaue.ToString());
                    //var deleteproduct = db.Products.Single(course => course.prod_ID == productCode);
                    
                    //db.SubmitChanges();
                    //MessageBox.Show("Product De-Activated Successfully");
                    //BindProductslist();
                    // Bindprod

                }
                else
                {
                    MessageBox.Show("Please Select Any One Product");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void agvProdList_FilterStringChanged(object sender, EventArgs e)
        {
           
        }

        private void agvProdList_SortStringChanged(object sender, EventArgs e)
        {
           
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            BindProductslist();
            

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                if (i >= 0)
                {
                    DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!","Delete Confirmation",MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        int ProdID;
                        var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                        var mappingName = sfDataGrid1.Columns["EMP_ID"].MappingName;
                        var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                       
                        ProdID = Convert.ToInt32(cellVaue.ToString());
                        db.sp_Delete_Employees (ProdID, logIn.company);
                        MessageBox.Show("Employee Deleted Successfully");
                        BindProductslist();
                    }

                }
                else
                {
                    MessageBox.Show("Please Select Atleast One Product to Delete");
                }
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146232060)
                {
                    
                    MessageBox.Show("The Master Record Already in Use, Cannot Be Deleted");
                }
                else
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void createBOMToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void sfDataGrid1_AutoGeneratingColumn(object sender, Syncfusion.WinForms.DataGrid.Events.AutoGeneratingColumnArgs e)
        {
            if (e.Column.MappingName == "Alternative_Code")
            {
                e.Column.Visible = false;
                //e.Column.AllowSorting = true;
                //e.Column.AllowGrouping = false;
                //e.Column.HeaderStyle.BackColor = Color.LightSkyBlue;
                //e.Column.CellStyle.BackColor = Color.MediumBlue;
            }
        }

        private void chkShowDeactivateProducts_CheckedChanged(object sender, EventArgs e)
        {
            BindProductslist();
        }

        private void sfDataGrid1_Click(object sender, EventArgs e)
        {

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

            string SheetName = "MTPL (2)";
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
            dt.Columns.Add(new DataColumn("Emp_Code", typeof(string)));            
            dt.Columns.Add(new DataColumn("Bank_Ac_No", typeof(string)));           
            dt.Columns.Add(new DataColumn("Bank_Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Bank_IFSC_Code", typeof(string)));
            dt.Columns.Add(new DataColumn("AadharNo", typeof(string)));     
            //dt.Columns.Add(new DataColumn("Prod_Storage_Location_Id", typeof(string)));
            //dt.Columns.Add(new DataColumn("Product_Bin_Id", typeof(string)));
            //dt.Columns.Add(new DataColumn("Prod_Unit_Wt", typeof(string)));
            //dt.Columns.Add(new DataColumn("Prod_PartNo", typeof(string)));
            //dt.Columns.Add(new DataColumn("Prod_Mfg_Code", typeof(string)));
            //dt.Columns.Add(new DataColumn("Prod_Description", typeof(string)));
            //dt.Columns.Add(new DataColumn("Prod_HSN_Code", typeof(string)));
            //dt.Columns.Add(new DataColumn("Prod_Tax_Class", typeof(string)));
            //dt.Columns.Add(new DataColumn("Prod_Status_ID", typeof(string)));
            //dt.Columns.Add(new DataColumn("Company_ID", typeof(string)));
            //dt.Columns.Add(new DataColumn("Created_By", typeof(string)));
            //dt.Columns.Add(new DataColumn("Modified_BY", typeof(string)));
            for (int i = 0; i < count; i++)
            {
                dr = dt.NewRow();               
                dr["Emp_Code"] = DtSet.Rows[i]["Emp_Code"].ToString(); ;
                dr["Bank_Ac_No"] = DtSet.Rows[i]["Bank_Ac_No"].ToString();
                dr["Bank_Name"] = DtSet.Rows[i]["Bank_Name"].ToString();
                dr["Bank_IFSC_Code"] = DtSet.Rows[i]["Bank_IFSC_Code"].ToString();
                dr["AadharNo"] = DtSet.Rows[i]["AadharNo"].ToString(); ;
                //dr["Prod_Storage_Location_Id"] = DtSet.Rows[i]["Prod_Storage_Location_Id"].ToString();
                //dr["Product_Bin_Id"] = DtSet.Rows[i]["Product_Bin_Id"].ToString();
                //dr["Prod_Unit_Wt"] = DtSet.Rows[i]["Prod_Unit_Wt"].ToString();
                //dr["Prod_PartNo"] = DtSet.Rows[i]["Prod_PartNo"].ToString();
                //dr["Prod_Mfg_Code"] = DtSet.Rows[i]["Prod_Mfg_Code"].ToString(); ;
                //dr["Prod_Description"] = DtSet.Rows[i]["Prod_Description"].ToString();
                //dr["Prod_HSN_Code"] = DtSet.Rows[i]["Prod_HSN_Code"].ToString();
                //dr["Prod_Tax_Class"] = DtSet.Rows[i]["Prod_Tax_Class"].ToString(); ;
                //dr["Prod_Status_ID"] = DtSet.Rows[i]["Prod_Status_ID"].ToString(); ;
                //dr["Company_ID"] = logIn.company;
                //dr["Created_By"] = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt"); 
                //dr["Modified_BY"] = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

                dt.Rows.Add(dr);
            }
            //dgProductData.DataSource = dt;
         
            MyConnection.Close();
            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                var p = db.HR_Employee_Master_Datas.Where(w => w.Emp_Code == dt.Rows[i]["Emp_Code"].ToString()).FirstOrDefault();

                //Product p = new Product();               
               // p.Emp_Code = dt.Rows[i]["Emp_Code"].ToString();
                p.Bank_Ac_No = dt.Rows[i]["Bank_Ac_No"].ToString();
                p.Bank_Name = dt.Rows[i]["Bank_Name"].ToString();
                p.Bank_IFSC_Code = dt.Rows[i]["Bank_IFSC_Code"].ToString();
                p.AadharNo = dt.Rows[i]["AadharNo"].ToString();
                //p.Bank_Ac_No = Convert.ToInt32(dt.Rows[i]["Prod_Group_Id"].ToString());              
                //p.Bank_Name = Convert.ToInt32(dt.Rows[i]["Prod_Type_Id"].ToString());
                //p.Bank_IFSC_Code = dt.Rows[i]["Prod_Name"].ToString();
                //p.AadharNo = (dt.Rows[i]["AadharNo"].ToString() == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(dt.Rows[i]["Prod_Unit_Wt"].ToString());
                //p.Prod_Tax_Class = Convert.ToInt32(dt.Rows[i]["Prod_Tax_Class"].ToString());
                //p.Prod_Description = (dt.Rows[i]["Prod_Description"].ToString() == "") ? "" : dt.Rows[i]["Prod_Description"].ToString();
                //p.Prod_HSN_Code = (dt.Rows[i]["Prod_HSN_Code"].ToString() == "") ? "" : dt.Rows[i]["Prod_HSN_Code"].ToString();
                //p.Prod_Status_ID = Convert.ToInt32(dt.Rows[i]["Prod_Status_ID"].ToString());
                //p.Prod_Mfg_Code = (dt.Rows[i]["Prod_Mfg_Code"].ToString() == "") ? "" : dt.Rows[i]["Prod_Mfg_Code"].ToString();
                //p.Created_By = logIn.username + "-" + DateTime.Now;
                //p.Modified_BY = logIn.username + "-" + DateTime.Now;
                //p.Company_ID = logIn.company;              
                //db.Products.InsertOnSubmit(p);
                db.SubmitChanges();
              

               
            }

                Cursor.Current = Cursors.Default;
        }

        private void updateSalaryDataToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                int i = sfDataGrid1.CurrentCell.RowIndex;
                if (i >= 0)
                {

                    var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["EMP_ID"].MappingName;
                    var mappingName1 = sfDataGrid1.Columns["Status"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                    if (cellVaue1 != "Resigned")
                    {
                        productCode = Convert.ToInt32(cellVaue.ToString());
                        var = "0";
                        ioneNet.HumanResourceManagement.Masters.frmSalaryStructure frm = new ioneNet.HumanResourceManagement.Masters.frmSalaryStructure();
                        frm.ShowDialog();

                    }
                    else
                    {
                        MessageBox.Show("Employee Status is Resigned, Data Cannot be Modified");
                    }                   
                 
                }
                else
                {
                    MessageBox.Show("Please Select Any One Product");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
        }

        private void cmbEmpStatus_Leave(object sender, EventArgs e)
        {
            if(cmbEmpStatus.Text == "Transferred")
            {
                cmbTransferredTo.Visible = true;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            int i = sfDataGrid1.CurrentCell.RowIndex;
            var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
            var mappingName = sfDataGrid1.Columns["EMP_ID"].MappingName;
            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            //var currentCellV00alue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
            productCode = Convert.ToInt32(cellVaue.ToString());
            var p1 = db.HR_Employee_Master_Datas.Where(w => w.id == productCode && w.Company_ID == logIn.company).FirstOrDefault();

            p1.Status = Convert.ToInt32(cmbEmpStatus.SelectedValue);
            p1.Resigned_On = dtDate.Value;
            p1.Reason_For_Leave = txtRemarks.Text;
            if (cmbEmpStatus.Text == "Resigned")
            {
                p1.is_Resigned = true;
            }
            else
            {
                p1.is_Resigned = false;
            }           
            p1.Modified_BY = logIn.username + "-" + DateTime.Now;          

            db.SubmitChanges();
            MessageBox.Show("Employee Status Updated Successfully");
           

        }

        private void generateIndentToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void advancedDataGridView1_FilterStringChanged(object sender, EventArgs e)
        {

        }

        private void sfButton1_Click(object sender, EventArgs e)
        {

            var options = new ExcelExportingOptions();
            options.StartRowIndex = 5;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            workBook.Worksheets[0].Range["A2"].Value = "Employees List";
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\EmployeesList.xlsx");
            string doc = Fname + "\\EmployeesList.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }
    }
}
