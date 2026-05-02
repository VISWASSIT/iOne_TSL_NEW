using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
using System.Data.Linq.SqlClient;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.Data;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Ione_DAL;
using Syncfusion.WinForms.DataGridConverter;
using System.Diagnostics;
using System.Data.OleDb;
using System.Security.Cryptography;
using Syncfusion.WinForms.DataGrid.Events;

namespace ioneNet.OrderManagement.Masters
{
    public partial class frmCustomers : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        int Creation_Company = logIn.company;
        public static string Customer_Id1, Customer_Name2, var, Customer_Name3,  Salesmen;
        public static int custId;
        public frmCustomers()
        {
            InitializeComponent();
        }

        #region Method
        public void bindCustomers()
        {
            try
            {
                var p = (from s in db.Customer_informations where s.Company_ID == logIn.company && s.Status  ==1

                         select new
                         {
                             s.ID,
                             s.Customer_Id,
                             Customer_Name= s.Customer_Name,
                             s.Customer_Type,
                             s.Contact_Person,
                             s.Contact_Mobile,
                             s.GSTIN_NO,                            
                             s.City,                             
                         });
                SqlCommand cmd1 = (SqlCommand)db.GetCommand(p);
                SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
                DataTable dt1 = new DataTable();
                da1.Fill(dt1);

                if (dt1.Rows.Count >= 0)
                {
                    dgvCustomers.DataSource = dt1;
                }
                else
                {
                }
            }
            catch (Exception ex)
            {

            }
        }
        #endregion

        private void frmCustomers_Load(object sender, EventArgs e)
        {
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            BindSupplierslist();
            //this.customer_informationTableAdapter.Fill(this.ioneDataSet.Customer_information,logIn.company);
            //if (!frmGate.Create_menu.Contains(this.Text))
            //{
            //    btnAdd.Enabled = false;
            //}
            //if (!frmGate.Modify.Contains(this.Text))
            //{
            //    btnModify.Enabled = false;
            //    dgvCustomers.Enabled = false;
            //    btnDelete.Enabled = false;
            //}
            //if (!frmGate.Authorization_Menus.Contains(this.Text))
            //{
            //    btnDelete.Enabled = false;
            //}

            //bindCustomers();
        }

        // // // Edit Button Event
        private void button4_Click(object sender, EventArgs e)
        {
            MaterialManagement.SupplierMaster form = new MaterialManagement.SupplierMaster();
            var = "1";
            form.ShowDialog();

        }

        // // // Reset Button Event
        


        private void btnDelete_Click(object sender, EventArgs e)
        {
            
        }

        private void dgvCustomers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try

            {
                //Cursor.Current = Cursors.WaitCursor;
                //int i = dgvCustomers.CurrentRow.Index;
                //int columnIndex = dgvCustomers.CurrentCell.ColumnIndex;
                //string columnName = dgvCustomers.Columns[columnIndex].HeaderText;
                //if (columnName == "Customer ID")
                //{
                //    Customer_Id1 = dgvCustomers.Rows[dgvCustomers.CurrentRow.Index].Cells["Customer_Id"].Value.ToString();
                //    Customer_Name2 = dgvCustomers.Rows[dgvCustomers.CurrentRow.Index].Cells["Customer_Name"].Value.ToString();
                // //   Region = (dgvCustomers.Rows[dgvCustomers.CurrentRow.Index].Cells["City"].Value=null):""?dgvCustomers.Rows[dgvCustomers.CurrentRow.Index].Cells["City"].Value.ToString();
                //    //Salesmen = dgvCustomers.Rows[dgvCustomers.CurrentRow.Index].Cells["Salesmen_Code"].Value.ToString();
                //    var = "0";
                //    OrderManagement.Masters.frmCustomers form = new OrderManagement.Masters.frmCustomers();
                //    //  frmcust.MdiParent = this.ParentForm;
                //    //frmcust.ShowDialog();
                //}
                //else
                //{
                //    Customer_Id1 = dgvCustomers.Rows[dgvCustomers.CurrentRow.Index].Cells["Customer_Id"].Value.ToString();
                //    Customer_Name3 = dgvCustomers.Rows[dgvCustomers.CurrentRow.Index].Cells["Customer_Name"].Value.ToString();

                //    string s = Customer_Name3;
                //    string[] values = s.Split('-');
                //    for (int j = 0; j < values.Length; j++)
                //    {
                //        values[j] = values[j].Trim();
                //    }

                //    Region = values[1].ToString(); //dgvCustomers.Rows[dgvCustomers.CurrentRow.Index].Cells["City"].Value.ToString();
                //    //Salesmen = dgvCustomers.Rows[dgvCustomers.CurrentRow.Index].Cells["Salesmen_Code"].Value.ToString();
                //    var = "CustomerName";
                //    //Reports.Cusomer_Ledger FmrOrd = new Reports.Cusomer_Ledger();
                //    //FmrOrd.MdiParent = this.ParentForm;
                //    //FmrOrd.Show();
                //}
                //Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {
                ex.Message.ToString();
            }
        }

        private void frmCustomers_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F2)
                    button4_Click(sender, e);


                if (e.Control && e.KeyCode == Keys.R)
                    //button5_Click(sender, e);


                if (e.Alt && e.KeyCode == Keys.F4)
                    btnClose_Click(sender, e);
                if (e.KeyCode == Keys.F3)
                    //btnModify_Click(sender, e);


                if (e.KeyCode == Keys.F4)
                    btnDelete_Click(sender, e);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = dgvCustomers.CurrentCell.RowIndex; ;
                if (i >= 0)
                {
                    var rowData = dgvCustomers.GetRecordAtRowIndex(i);
                    var mappingName = dgvCustomers.Columns["ID"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                    custId = Convert.ToInt32(cellVaue.ToString());
                    var = "0";
                    //Customer_Name2 = dgvCustomers.Rows[dgvCustomers.CurrentRow.Index].Cells["customerNameDataGridViewTextBoxColumn"].Value.ToString();
                    //CRM.frmAddNewCustomer frmcust = new frmAddNewCustomer();
                    //frmcust.MdiParent = this.ParentForm;
                    //frmcust.Show();
                    MaterialManagement.SupplierMaster form = new MaterialManagement.SupplierMaster();

                    //OrderManagement.Masters.frmAddNewCustomer form = new OrderManagement.Masters.frmAddNewCustomer();
                    //var = "1";
                    form.ShowDialog();

                }
                else
                {
                    MessageBox.Show("Please Select Any One Customer");
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
                int i = dgvCustomers.SelectedIndex;
                if (i >= 0)
                {
                    var rowData = dgvCustomers.GetRecordAtRowIndex(i);
                    var mappingName = dgvCustomers.Columns["ID"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                    custId = Convert.ToInt32(cellVaue.ToString());
                    var ci = db.Supplier_informations.Where(w => w.ID == custId && w.Company_ID == Creation_Company).FirstOrDefault();
                    {
                        ci.Status = 2;
                        ci.Modified_By = logIn.username;
                        //ci.Modified_Date = Convert.ToDateTime(DateTime.Now.ToString());
                        db.SubmitChanges();
                        MessageBox.Show("Customer De-Activated Sucessfully");
                        BindSupplierslist();
                    }

                }
                else
                {
                    MessageBox.Show("Please Select Any One Customer");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            BindSupplierslist();
            // bindCustomers();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            
        }

        private void dgvCustomers_SortStringChanged(object sender, EventArgs e)
        {
            
        }

        private void dgvCustomers_FilterStringChanged(object sender, EventArgs e)
        {
           
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = dgvCustomers.CurrentCell.RowIndex; 
                if (i >= 0)
                {
                    DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        int ProdID, compID;
                        string AccID;

                        var rowData = dgvCustomers.GetRecordAtRowIndex(i);
                        var mappingName = dgvCustomers.Columns["ID"].MappingName;
                        var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                        var mappingName1 = dgvCustomers.Columns["Customer_ID"].MappingName;
                        var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                        ProdID = Convert.ToInt32(cellVaue.ToString());
                        AccID = cellVaue1.ToString();
                        SqlCommand cmd2 = new SqlCommand("delete  from [Supplier_information] where id =@ProdID", con);
                        SqlCommand cmd1 = new SqlCommand("delete  from [AccountMaster] where AccCode =@AccID and Company_ID=@compID", con);

                        cmd1.Parameters.AddWithValue("@AccID", AccID);
                        cmd1.Parameters.AddWithValue("@compID", logIn.company);
                        cmd2.Parameters.AddWithValue("@ProdID", ProdID);
                        if (con.State != ConnectionState.Open)
                            con.Open();
                        //con.Open();
                        cmd2.ExecuteNonQuery();
                        cmd1.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Party Info Deleted Successfully");
                        BindSupplierslist();
                    }
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

        int Parameter;

        private void btnImport_Click(object sender, EventArgs e)
        {
           
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 5;
            var excelEngine = dgvCustomers.ExportToExcel(dgvCustomers.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            workBook.Worksheets[0].Range["A2"].Value = "Customers List";         
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\CustomersList.xlsx");
            string doc = Fname + "\\CustomersList.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void btnImportData_Click(object sender, EventArgs e)
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
            dt.Columns.Add(new DataColumn("Customer_Code", typeof(string)));
            dt.Columns.Add(new DataColumn("Customer_Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Supplier_Alias_Name", typeof(string)));            
            dt.Columns.Add(new DataColumn("Address_1", typeof(string)));
            dt.Columns.Add(new DataColumn("Address_2", typeof(string)));
            dt.Columns.Add(new DataColumn("City", typeof(string)));
            dt.Columns.Add(new DataColumn("Pincode", typeof(string)));
            dt.Columns.Add(new DataColumn("StateCode", typeof(string)));
            dt.Columns.Add(new DataColumn("GSTIN_NO", typeof(string)));
            dt.Columns.Add(new DataColumn("Contact_Mobile", typeof(string)));
            dt.Columns.Add(new DataColumn("Contact_Email", typeof(string)));
            dt.Columns.Add(new DataColumn("Company_ID", typeof(string)));
            dt.Columns.Add(new DataColumn("Created_By", typeof(string)));
            dt.Columns.Add(new DataColumn("Modified_BY", typeof(string)));
            for (int i = 0; i < count; i++)
            {
                dr = dt.NewRow();
                dr["Customer_Code"] = DtSet.Rows[i]["Customer_Code"].ToString(); ;
                dr["Customer_Name"] = DtSet.Rows[i]["Customer_Name"].ToString();
                dr["Supplier_Alias_Name"] = DtSet.Rows[i]["Customer_Name"].ToString();                
                dr["Address_1"] = DtSet.Rows[i]["Address_1"].ToString(); ;
                dr["Address_2"] = DtSet.Rows[i]["Address_2"].ToString();
                dr["City"] = DtSet.Rows[i]["City"].ToString();
                dr["Pincode"] = DtSet.Rows[i]["Pincode"].ToString();
                dr["StateCode"] = DtSet.Rows[i]["StateCode"].ToString();
                dr["GSTIN_NO"] = DtSet.Rows[i]["GSTIN_NO"].ToString();
                dr["Contact_Mobile"] = DtSet.Rows[i]["Contact_Mobile"].ToString();
                dr["Company_ID"] = logIn.company;
                dr["Created_By"] = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                dr["Modified_BY"] = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

                dt.Rows.Add(dr);
            }
            //dgProductData.DataSource = dt;

            MyConnection.Close();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                if ((from u in db.Supplier_informations where u.Supplier_Id == dt.Rows[i]["Customer_Code"].ToString() && u.Company_ID == Creation_Company select u).Count() > 0)
                {
                    var ci = db.Supplier_informations.Where(w => w.Supplier_Id == dt.Rows[i]["Customer_Code"].ToString() && w.Company_ID == Creation_Company).FirstOrDefault();

                    ci.Supplier_Name = dt.Rows[i]["Customer_Name"].ToString();
                    ci.Supplier_Alias_Name = dt.Rows[i]["Customer_Name"].ToString();
                    ci.Supplier_Category = 27;// Convert.ToInt32(dt.Rows[i]["Supplier_Category"].ToString());
                    ci.Address_1 = dt.Rows[i]["Address_1"].ToString();
                    ci.Address_2 = dt.Rows[i]["Address_2"].ToString();
                    ci.City = dt.Rows[i]["City"].ToString();
                    ci.Pincode = dt.Rows[i]["Pincode"].ToString();
                    ci.StateCode = dt.Rows[i]["StateCode"].ToString();
                    ci.GSTIN_NO = (dt.Rows[i]["GSTIN_NO"].ToString() == "") ? "" : dt.Rows[i]["GSTIN_NO"].ToString();
                    ci.Contact_Mobile = DtSet.Rows[i]["Contact_Mobile"].ToString();
                    ci.Modified_By = logIn.username + "-" + DateTime.Now;
                    db.SubmitChanges();
                }
                else
                {
                    Supplier_information p = new Supplier_information();
                    p.Supplier_Id = dt.Rows[i]["Customer_Code"].ToString();
                    p.Supplier_Name = dt.Rows[i]["Customer_Name"].ToString();
                    p.Supplier_Alias_Name = dt.Rows[i]["Customer_Name"].ToString();
                    p.Supplier_Category = 27; //Convert.ToInt32(dt.Rows[i]["Supplier_Category"].ToString());
                    p.Address_1 = dt.Rows[i]["Address_1"].ToString();
                    p.Address_2 = dt.Rows[i]["Address_2"].ToString();
                    p.City = dt.Rows[i]["City"].ToString();
                    p.Pincode = dt.Rows[i]["Pincode"].ToString();
                    p.StateCode = dt.Rows[i]["StateCode"].ToString();
                    p.GSTIN_NO = (dt.Rows[i]["GSTIN_NO"].ToString() == "") ? "" : dt.Rows[i]["GSTIN_NO"].ToString();
                    p.Contact_Mobile = DtSet.Rows[i]["Contact_Mobile"].ToString();
                    p.Status = 1;
                    p.Created_By = logIn.username + "-" + DateTime.Now;
                    p.Modified_By = logIn.username + "-" + DateTime.Now;
                    p.Company_ID = logIn.company;
                    db.Supplier_informations.InsertOnSubmit(p);
                    db.SubmitChanges();
                }

            }

            Cursor.Current = Cursors.Default;
        }

        public int re, re1, re2;
       

        public void MyMenu_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
           
        }

      
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void BindSupplierslist()
        {
            try
            {
               
                var d = (from data in db.ShowCustomers_TSL(logIn.company,logIn.userID) select data).ToList();
                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    dgvCustomers.DataSource = d;
                    this.dgvCustomers.QueryCellStyle += dgvCustomers_QueryCellStyle;

                    this.dgvCustomers.FilterRowPosition = RowPosition.Top;
                this.dgvCustomers.Columns["Sales_Exe"].FilterRowEditorType = "TextBox";
                this.dgvCustomers.Columns["Sales_Exe"].ShowFilterRowOptions = false;
                this.dgvCustomers.Columns["Sales_Exe"].ImmediateUpdateColumnFilter = true;
                this.dgvCustomers.Columns["Sales_Exe"].FilterRowCondition = FilterRowCondition.Contains;

                this.dgvCustomers.Columns["Customer_Name"].FilterRowEditorType = "TextBox";
                this.dgvCustomers.Columns["Customer_Name"].ShowFilterRowOptions = false;
                this.dgvCustomers.Columns["Customer_Name"].ImmediateUpdateColumnFilter = true;
                this.dgvCustomers.Columns["Customer_Name"].FilterRowCondition = FilterRowCondition.Contains;

                this.dgvCustomers.Columns["Category"].FilterRowEditorType = "TextBox";
                this.dgvCustomers.Columns["Category"].ShowFilterRowOptions = false;
                this.dgvCustomers.Columns["Category"].ImmediateUpdateColumnFilter = true;
                this.dgvCustomers.Columns["Category"].FilterRowCondition = FilterRowCondition.Contains;

                this.dgvCustomers.Columns["GSTIN_NO"].FilterRowEditorType = "TextBox";
                this.dgvCustomers.Columns["GSTIN_NO"].ShowFilterRowOptions = false;
                this.dgvCustomers.Columns["GSTIN_NO"].ImmediateUpdateColumnFilter = true;

                this.dgvCustomers.Columns["city"].FilterRowEditorType = "TextBox";
                this.dgvCustomers.Columns["city"].ShowFilterRowOptions = false;
                this.dgvCustomers.Columns["city"].ImmediateUpdateColumnFilter = true;
                this.dgvCustomers.Columns["city"].FilterRowCondition = FilterRowCondition.Contains;


                this.dgvCustomers.TableSummaryRows.Clear();
                GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                tableSummaryRow1.Name = "TableSummary";
                tableSummaryRow1.ShowSummaryInRow = false;
                tableSummaryRow1.Position = VerticalPosition.Bottom;

                GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                summaryColumn1.Name = "No Of Customers";
                summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                summaryColumn1.Format = "No Of Customers: {Count}";
                summaryColumn1.MappingName = "ID";

                tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

                this.dgvCustomers.TableSummaryRows.Add(tableSummaryRow1);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void dgvCustomers_QueryCellStyle(object sender, QueryCellStyleEventArgs e)
        {
            if (e.Column.MappingName == "Region")
            {
                if (e.DisplayText == "Southern Region")
                {
                    e.Style.BackColor = Color.LightSkyBlue;
                    e.Style.TextColor = Color.Black;
                }
                else if (e.DisplayText == "Western Region")
                {
                    e.Style.BackColor = Color.LightGreen;
                    e.Style.TextColor = Color.DarkSlateBlue;
                }
                else if (e.DisplayText == "Northern Region")
                {
                    e.Style.BackColor = Color.LightCoral;
                    e.Style.TextColor = Color.Black;
                }
                else if (e.DisplayText == "Eastern Region")
                {
                    e.Style.BackColor = Color.LightPink;
                    e.Style.TextColor = Color.Black;
                }
            }

            if (e.Column.MappingName == "Quote_Validity")
            {
                //var dataRowView = e.RowData as DataRowView;
                //var dataRow = dataRowView.Row;
                //var cellValue = dataRow["Status"].ToString();

                //DateTime dtvalid = Convert.ToDateTime(e.DisplayText);
                //if (dtvalid <= DateTime.Now)
                //{
                //    e.Style.BackColor = Color.Red;
                //}
            }
        }
    }
}

