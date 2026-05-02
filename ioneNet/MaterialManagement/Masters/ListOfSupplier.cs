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
using Syncfusion.WinForms.GridCommon.ScrollAxis;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.Data;
using System.Data.OleDb;
using Ione_DAL;
using Syncfusion.WinForms.DataGridConverter;
using System.Diagnostics;

namespace ioneNet.MaterialManagement
{
    public partial class ListOfSupplier : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        int Creation_Company = logIn.company;
        public static string Customer_Id1, Customer_Name2, var, Customer_Name3, Salesmen;
        public static int SuppId;
        public ListOfSupplier()
        {
            InitializeComponent();
        }

        #region Method
        public void bindCustomers()
        {
            try
            {
                var p = (from s in db.Supplier_informations
                         where s.Company_ID==Creation_Company && s.Status==1
                         select new
                         {
                             s.ID,
                             s.Supplier_Id,
                             Supplier_Name = s.Supplier_Name,
                             s.Supplier_Type,
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
            var bindLoc = (from m in db.User_Roles
                           where m.Company_ID == logIn.company && m.Role_ID == logIn.UserRoleID && m.Form_ID == Convert.ToInt32(frmMain.frmname)
                           select new
                           {

                               m.View_Role,
                               m.Modify_Role,
                               m.Create_Role,
                               m.Delete_Role
                           }).ToList();
            this.modifyToolStripMenuItem.Enabled = false;
            if (bindLoc[0].Modify_Role == true)
            {
                this.modifyToolStripMenuItem.Enabled = true;
            }
            this.viewToolStripMenuItem.Enabled = false;
            if (bindLoc[0].View_Role == true)
            {
                this.viewToolStripMenuItem.Enabled = true;
            }
            this.deactivateToolStripMenuItem.Enabled = false;
            this.deleteToolStripMenuItem.Enabled = false;
            if (bindLoc[0].Delete_Role == true)
            {
                this.deactivateToolStripMenuItem.Enabled = true;
                this.deleteToolStripMenuItem.Enabled = true;
            }
            btnAddNew.Enabled = false;
            if (bindLoc[0].Create_Role == true)
            {
                btnAddNew.Enabled = true;
            }
            BindSupplierslist();

            //var result = db.Sp_autoincrement_Supplier_Master(Creation_Company);

        }

        // // // Edit Button Event
        private void button4_Click(object sender, EventArgs e)
        {
            MaterialManagement.SupplierMaster form = new MaterialManagement.SupplierMaster();
            var = "1";
            form.ShowDialog();

        }

           

        

        private void frmCustomers_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                //if (e.KeyCode == Keys.F2)
                //    button4_Click(sender, e);


                //if (e.Control && e.KeyCode == Keys.R)
                //    //button5_Click(sender, e);


                //if (e.Alt && e.KeyCode == Keys.F4)
                //    btnClose_Click(sender, e);
               
                   


               
                   
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
                int i = dgvCustomers.CurrentCell.RowIndex;
                if (i >= 0)
                {
                    var rowData = dgvCustomers.GetRecordAtRowIndex(i);
                    var mappingName = dgvCustomers.Columns["ID"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    SuppId = Convert.ToInt32(cellVaue.ToString());
                    var = "0";
                    MaterialManagement.SupplierMaster form = new MaterialManagement.SupplierMaster();
                    //var = "1";
                    form.ShowDialog();
                }

                //    if (dgvCustomers.Rows[dgvCustomers.CurrentRow.Index].Cells["idDataGridViewTextBoxColumn"].Value.ToString() != "")
                //{
                //    SuppId = Convert.ToInt32(dgvCustomers.Rows[dgvCustomers.CurrentRow.Index].Cells["idDataGridViewTextBoxColumn"].Value.ToString());
                   
                //    Customer_Name2 = dgvCustomers.Rows[dgvCustomers.CurrentRow.Index].Cells["partynameDataGridViewTextBoxColumn"].Value.ToString();
                //    //CRM.frmAddNewCustomer frmcust = new frmAddNewCustomer();
                //    //frmcust.MdiParent = this.ParentForm;
                //    //frmcust.Show();
                   

                //}
                else
                {
                    MessageBox.Show("Please Select Any One Party");
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
                    SuppId = Convert.ToInt32(cellVaue.ToString());
                    var ci = db.Supplier_informations.Where(w => w.ID == SuppId && w.Company_ID == Creation_Company).FirstOrDefault();
                    {
                        ci.Status = 2;
                        ci.Modified_By = logIn.username;
                        //ci.Modified_Date = Convert.ToDateTime(DateTime.Now.ToString());
                        db.SubmitChanges();
                        MessageBox.Show("Party De-Activated Sucessfully");
                    }

                }
                   
                else
                {
                    MessageBox.Show("Please Select Any One Supplier");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

            

        private void button1_Click(object sender, EventArgs e)
        {
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;            
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                //int i = dgvCustomers.SelectedIndex;
                int i = dgvCustomers.CurrentCell.RowIndex;
                //var rowData1 = dgvCustomers.GetRecordAtRowIndex(i);
                //var mappingName2 = dgvCustomers.Columns["ID"].MappingName;
                //var cellVaue2 = (rowData1.GetType().GetProperty(mappingName2).GetValue(rowData1, null).ToString());


                
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
                        var mappingName1 = dgvCustomers.Columns["Supplier_Id"].MappingName;
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
                        //BindSupplierslist();
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

        private void sfButton1_Click(object sender, EventArgs e)
        {
            BindSupplierslist();
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

            string SheetName = "Suppliers";
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
            dt.Columns.Add(new DataColumn("Supplier_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("Supplier_Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Supplier_Category", typeof(string)));
            dt.Columns.Add(new DataColumn("Supplier_Alias_Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Supplier_Type", typeof(string)));
            dt.Columns.Add(new DataColumn("Address_1", typeof(string)));
            dt.Columns.Add(new DataColumn("Address_2", typeof(string)));
            dt.Columns.Add(new DataColumn("City", typeof(string)));
            dt.Columns.Add(new DataColumn("State", typeof(string)));
            dt.Columns.Add(new DataColumn("StateCode", typeof(string)));
            dt.Columns.Add(new DataColumn("Pincode", typeof(string)));
            dt.Columns.Add(new DataColumn("Phone_No", typeof(string)));
            dt.Columns.Add(new DataColumn("Country", typeof(string)));
            dt.Columns.Add(new DataColumn("GSTIN_NO", typeof(string)));
            dt.Columns.Add(new DataColumn("Status", typeof(string)));
            dt.Columns.Add(new DataColumn("Company_ID", typeof(string)));
            dt.Columns.Add(new DataColumn("Created_By", typeof(string)));
            dt.Columns.Add(new DataColumn("Modified_BY", typeof(string)));
            for (int i = 0; i < count; i++)
            {
                dr = dt.NewRow();
                dr["Supplier_Id"] = DtSet.Rows[i]["Supplier_Id"].ToString(); ;
                dr["Supplier_Name"] = DtSet.Rows[i]["Supplier_Name"].ToString();
                dr["Supplier_Category"] = DtSet.Rows[i]["Supplier_Category"].ToString();
                dr["Supplier_Alias_Name"] = DtSet.Rows[i]["Supplier_Alias_Name"].ToString();
                dr["Supplier_Type"] = DtSet.Rows[i]["Supplier_Type"].ToString(); ;
                dr["Address_1"] = DtSet.Rows[i]["Address_1"].ToString();
                dr["Address_2"] = DtSet.Rows[i]["Address_2"].ToString();
                dr["City"] = DtSet.Rows[i]["City"].ToString();
                dr["State"] = DtSet.Rows[i]["State"].ToString();
                dr["StateCode"] = DtSet.Rows[i]["StateCode"].ToString(); ;
                dr["Pincode"] = DtSet.Rows[i]["Pincode"].ToString();
                dr["Country"] = DtSet.Rows[i]["Country"].ToString();
                dr["GSTIN_NO"] = DtSet.Rows[i]["GSTIN_NO"].ToString(); ;
                dr["Status"] = DtSet.Rows[i]["Status"].ToString(); ;
                dr["Company_ID"] = logIn.company;
                dr["Created_By"] = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                dr["Modified_BY"] = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

                dt.Rows.Add(dr);
            }
            //dgProductData.DataSource = dt;

            MyConnection.Close();
            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                Supplier_information p = new Supplier_information();
                p.Supplier_Id = dt.Rows[i]["Supplier_Id"].ToString();
                p.Supplier_Name = dt.Rows[i]["Supplier_Name"].ToString();
                p.Supplier_Category = Convert.ToInt32(dt.Rows[i]["Supplier_Category"].ToString());
                p.Supplier_Alias_Name = dt.Rows[i]["Supplier_Alias_Name"].ToString();
                p.Supplier_Type = dt.Rows[i]["Supplier_Type"].ToString();
                p.Address_1 = dt.Rows[i]["Address_1"].ToString();
                p.Address_2 = dt.Rows[i]["Address_2"].ToString();
                p.City = dt.Rows[i]["City"].ToString();
                p.State = dt.Rows[i]["State"].ToString();
                p.StateCode = dt.Rows[i]["StateCode"].ToString();
                p.Pincode = dt.Rows[i]["Pincode"].ToString();
                p.Country = dt.Rows[i]["Country"].ToString();
                p.GSTIN_NO = dt.Rows[i]["GSTIN_NO"].ToString();
                p.Status = Convert.ToInt32(dt.Rows[i]["Status"].ToString());
                p.Created_By = logIn.username + "-" + DateTime.Now;
                p.Modified_By = logIn.username + "-" + DateTime.Now;
                p.Company_ID = logIn.company;
                db.Supplier_informations.InsertOnSubmit(p);
                db.SubmitChanges();



            }

            Cursor.Current = Cursors.Default;
        }

        int Parameter;

        private void btnExport_Click(object sender, EventArgs e)
        {
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 5;
            var excelEngine = dgvCustomers.ExportToExcel(dgvCustomers.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            workBook.Worksheets[0].Range["A2"].Value = "SUPPLIERS LIST";
            //workBook.Worksheets[0].Range["D2"].Value = "As on :" + dt1;
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\Suppliers_List.xlsx");
            string doc = Fname + "\\Suppliers_List.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void sfButton2_Click(object sender, EventArgs e)
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
            dt.Columns.Add(new DataColumn("Supplier_Id", typeof(string)));
            dt.Columns.Add(new DataColumn("Supplier_Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Supplier_Alias_Name", typeof(string)));
            dt.Columns.Add(new DataColumn("Supplier_Category", typeof(string)));
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
                dr["Supplier_Id"] = DtSet.Rows[i]["Supplier_Id"].ToString(); ;
                dr["Supplier_Name"] = DtSet.Rows[i]["Supplier_Name"].ToString();
                dr["Supplier_Alias_Name"] = DtSet.Rows[i]["Supplier_Name"].ToString();
                dr["Supplier_Category"] = DtSet.Rows[i]["Supplier_Category"].ToString();
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
            for (int i = 0; i < dt.Rows.Count - 1; i++)
            {
                if ((from u in db.Supplier_informations where u.Supplier_Id == dt.Rows[i]["Supplier_Id"].ToString() && u.Company_ID == Creation_Company select u).Count() > 0)
                {
                    var ci = db.Supplier_informations.Where(w => w.Supplier_Id == dt.Rows[i]["Supplier_Id"].ToString() && w.Company_ID == Creation_Company).FirstOrDefault();
                  
                    ci.Supplier_Name = dt.Rows[i]["Supplier_Name"].ToString();
                    ci.Supplier_Alias_Name = dt.Rows[i]["Supplier_Name"].ToString();
                    ci.Supplier_Category = 28;// Convert.ToInt32(dt.Rows[i]["Supplier_Category"].ToString());
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
                    p.Supplier_Id = dt.Rows[i]["Supplier_Id"].ToString();
                    p.Supplier_Name = dt.Rows[i]["Supplier_Name"].ToString();
                    p.Supplier_Alias_Name = dt.Rows[i]["Supplier_Name"].ToString();
                    p.Supplier_Category = 28; //Convert.ToInt32(dt.Rows[i]["Supplier_Category"].ToString());
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

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = dgvCustomers.CurrentCell.RowIndex;
                if (i >= 0)
                {
                    var rowData = dgvCustomers.GetRecordAtRowIndex(i);
                    var mappingName = dgvCustomers.Columns["ID"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    SuppId = Convert.ToInt32(cellVaue.ToString());
                    var = "2";
                    MaterialManagement.SupplierMaster form = new MaterialManagement.SupplierMaster();
                    //var = "1";
                    form.ShowDialog();
                }

                //    if (dgvCustomers.Rows[dgvCustomers.CurrentRow.Index].Cells["idDataGridViewTextBoxColumn"].Value.ToString() != "")
                //{
                //    SuppId = Convert.ToInt32(dgvCustomers.Rows[dgvCustomers.CurrentRow.Index].Cells["idDataGridViewTextBoxColumn"].Value.ToString());

                //    Customer_Name2 = dgvCustomers.Rows[dgvCustomers.CurrentRow.Index].Cells["partynameDataGridViewTextBoxColumn"].Value.ToString();
                //    //CRM.frmAddNewCustomer frmcust = new frmAddNewCustomer();
                //    //frmcust.MdiParent = this.ParentForm;
                //    //frmcust.Show();


                //}
                else
                {
                    MessageBox.Show("Please Select Any One Party");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
                //dgvCustomers.DataSource = null;
                //dgvCustomers.View.Refresh();
                dgvCustomers.ClearFilters();
                
                var d = (from data in db.ShowSuppliersList(logIn.company) select data).ToList();
                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    dgvCustomers.DataSource = d;


                    this.dgvCustomers.FilterRowPosition = RowPosition.Top;
                    this.dgvCustomers.Columns["Supplier_Name"].FilterRowEditorType = "TextBox";
                    this.dgvCustomers.Columns["Supplier_Name"].ShowFilterRowOptions = false;
                    this.dgvCustomers.Columns["Supplier_Name"].ImmediateUpdateColumnFilter = true;
                    this.dgvCustomers.Columns["Supplier_Name"].FilterRowCondition = FilterRowCondition.Contains;

                    this.dgvCustomers.Columns["Supplier_Category"].FilterRowEditorType = "TextBox";
                    this.dgvCustomers.Columns["Supplier_Category"].ShowFilterRowOptions = false;
                    this.dgvCustomers.Columns["Supplier_Category"].ImmediateUpdateColumnFilter = true;
                    this.dgvCustomers.Columns["Supplier_Category"].FilterRowCondition = FilterRowCondition.Contains;

                    this.dgvCustomers.Columns["GSTIN_NO"].FilterRowEditorType = "TextBox";
                    this.dgvCustomers.Columns["GSTIN_NO"].ShowFilterRowOptions = false;
                    this.dgvCustomers.Columns["GSTIN_NO"].ImmediateUpdateColumnFilter = true;
                    this.dgvCustomers.Columns["GSTIN_NO"].FilterRowCondition = FilterRowCondition.Contains;

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
                    summaryColumn1.Name = "No Of Suppliers";
                    summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn1.Format = "No Of Suppliers: {Count}";
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
    }
}

