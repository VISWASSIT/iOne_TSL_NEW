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
using System.IO;

namespace ioneNet.MaterialManagement.Masters
{
    public partial class ProductList : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public static int productCode;
        public static string var;
        public ProductList()
        {
            InitializeComponent();
        }



        private void ProductList_Load(object sender, EventArgs e)
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
            if (!colorDict.ContainsKey(rowColumnIndex))
                colorDict.Add(rowColumnIndex, color);
            else
                colorDict[rowColumnIndex] = color;
            sfDataGrid1.TableControl.Invalidate(this.sfDataGrid1.TableControl.GetCellRectangle(rowColumnIndex.RowIndex, rowColumnIndex.ColumnIndex, false));
        }

        public void BindProductslist()
        {
            try
            {
                //sfDataGrid1.DataSource = null;
                if (chkShowDeactivateProducts.Checked == true)
                {
                    this.sfDataGrid1.ClearGrouping();
                    this.sfDataGrid1.ClearSorting();
                    this.sfDataGrid1.ClearFilters();
                    this.sfDataGrid1.Refresh();
                    var d = (from data in db.TSL_ShowProductsList(logIn.company, 2, logIn.username) select data).ToList();
                    if (d.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        sfDataGrid1.DataSource = d;
                    }
                }
                else
                {
                    //sfDataGrid1.DataSource = null;
                    //this.sfDataGrid1.ClearGrouping();
                    //this.sfDataGrid1.ClearSorting();
                    //this.sfDataGrid1.ClearFilters();
                    this.sfDataGrid1.Refresh();
                    var d = (from data in db.TSL_ShowProductsList(logIn.company, 0, logIn.username) select data).ToList();
                    if (d.Count > 0)
                    {
                        //dgProductsList.DataSource = d;


                        sfDataGrid1.DataSource = null;
                        sfDataGrid1.DataSource = d;
                        this.sfDataGrid1.ClearGrouping();
                        this.sfDataGrid1.ClearSorting();
                        this.sfDataGrid1.ClearFilters();
                        this.sfDataGrid1.TableSummaryRows.Clear();
                        this.sfDataGrid1.GroupSummaryRows.Clear();
                        this.sfDataGrid1.Refresh();
                        this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                        this.sfDataGrid1.Columns["Prod_Code"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["Prod_Code"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["Prod_Code"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["Prod_Code"].FilterRowCondition = FilterRowCondition.Contains;

                        this.sfDataGrid1.Columns["Prod_Name"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["Prod_Name"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["Prod_Name"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["Prod_Name"].FilterRowCondition = FilterRowCondition.Contains;

                        this.sfDataGrid1.Columns["Prod_Group_Name"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["Prod_Group_Name"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["Prod_Group_Name"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["Prod_Group_Name"].FilterRowCondition = FilterRowCondition.Contains;

                        //this.sfDataGrid1.TableSummaryRows.Clear();
                        //GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                        //tableSummaryRow1.Name = "TableSummary";
                        //tableSummaryRow1.ShowSummaryInRow = false;
                        //tableSummaryRow1.Position = VerticalPosition.Bottom;

                        //GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                        //summaryColumn1.Name = "Total Products";
                        //summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                        //summaryColumn1.Format = "Total Products: {Count}";
                        //summaryColumn1.MappingName = "Prod_Code";

                        //tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

                        //this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void BindVasistaProductslist()
        {
            try
            {
                //sfDataGrid1.DataSource = null;
                if (chkShowDeactivateProducts.Checked == true)
                {
                    this.sfDataGrid1.ClearGrouping();
                    this.sfDataGrid1.ClearSorting();
                    this.sfDataGrid1.ClearFilters();
                    this.sfDataGrid1.Refresh();
                    var d = (from data in db.ShowProductsList_Vasista(logIn.company, 2, null,logIn.BU_ID) select data).ToList();
                    if (d.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        sfDataGrid1.DataSource = d;
                    }
                }
                else
                {
                    //sfDataGrid1.DataSource = null;
                    //this.sfDataGrid1.ClearGrouping();
                    //this.sfDataGrid1.ClearSorting();
                    //this.sfDataGrid1.ClearFilters();
                    this.sfDataGrid1.Refresh();
                    var d = (from data in db.ShowProductsList_Vasista(logIn.company, 0, null,logIn.BU_ID) select data).ToList();
                    if (d.Count > 0)
                    {
                        //dgProductsList.DataSource = d;


                        sfDataGrid1.DataSource = null;
                        sfDataGrid1.DataSource = d;
                        this.sfDataGrid1.Columns["prod_id"].Width = 0;
                        this.sfDataGrid1.ClearGrouping();
                        this.sfDataGrid1.ClearSorting();
                        this.sfDataGrid1.ClearFilters();
                        this.sfDataGrid1.TableSummaryRows.Clear();
                        this.sfDataGrid1.GroupSummaryRows.Clear();
                        this.sfDataGrid1.Refresh();
                        this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                        this.sfDataGrid1.Columns["Prod_Code"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["Prod_Code"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["Prod_Code"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["Prod_Code"].FilterRowCondition = FilterRowCondition.Contains;

                        this.sfDataGrid1.Columns["Prod_Name"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["Prod_Name"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["Prod_Name"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["Prod_Name"].FilterRowCondition = FilterRowCondition.Contains;

                        this.sfDataGrid1.Columns["Prod_Group_Name"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["Prod_Group_Name"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["Prod_Group_Name"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["Prod_Group_Name"].FilterRowCondition = FilterRowCondition.Contains;

                        //this.sfDataGrid1.TableSummaryRows.Clear();
                        //GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                        //tableSummaryRow1.Name = "TableSummary";
                        //tableSummaryRow1.ShowSummaryInRow = false;
                        //tableSummaryRow1.Position = VerticalPosition.Bottom;

                        //GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                        //summaryColumn1.Name = "Total Products";
                        //summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                        //summaryColumn1.Format = "Total Products: {Count}";
                        //summaryColumn1.MappingName = "Prod_Code";

                        //tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

                        //this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            Masters.frmProductsNew frm = new Masters.frmProductsNew();
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
                    var mappingName = sfDataGrid1.Columns["prod_id"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    //var currentCellV00alue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                    productCode = Convert.ToInt32(cellVaue.ToString());
                    var = "0";
                    ioneNet.MaterialManagement.Masters.frmProductsNew frm = new ioneNet.MaterialManagement.Masters.frmProductsNew();
                    frm.ShowDialog();

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
                int i = sfDataGrid1.CurrentCell.RowIndex;
                if (i >= 0)
                {
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["prod_id"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                    productCode = Convert.ToInt32(cellVaue.ToString());
                    var deleteproduct = db.Products.Single(course => course.prod_ID == productCode);
                    if (chkShowDeactivateProducts.Checked == true)
                    {
                        deleteproduct.Prod_Status_ID = 1;
                    }
                    else
                    {
                        deleteproduct.Prod_Status_ID = 2;
                    }
                    db.SubmitChanges();
                    MessageBox.Show("Product De-Activated Successfully");
                    if (logIn.company == 1044)
                    {
                        BindVasistaProductslist();
                    }
                    else
                    {
                        BindProductslist();
                    }
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
            if (logIn.company == 1044)
            {
                BindVasistaProductslist();
            }
            else
            {
                BindProductslist();
            }

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                if (i >= 0)
                {
                    DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        int ProdID;
                        var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                        var mappingName = sfDataGrid1.Columns["prod_id"].MappingName;
                        var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                        ProdID = Convert.ToInt32(cellVaue.ToString());
                        SqlCommand cmd1 = new SqlCommand("delete  from [Products] where prod_id =@ProdID", con);
                        cmd1.Parameters.AddWithValue("@ProdID", ProdID);

                        if (con.State != ConnectionState.Open)
                            con.Open();
                        //con.Open();
                        cmd1.ExecuteNonQuery();
                        con.Close();
                        MessageBox.Show("Product Deleted Successfully");
                        if (logIn.company == 1044)
                        {
                            BindVasistaProductslist();
                        }
                        else
                        {
                            BindProductslist();
                        }
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
            if (e.Column.MappingName == "prod_id")
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
            if (logIn.company == 1044)
            {
                BindVasistaProductslist();
            }
            else
            {
                BindProductslist();
            }
        }

        private void sfDataGrid1_Click(object sender, EventArgs e)
        {

        }
        
        private void btnImport_Click(object sender, EventArgs e)
        {
            try
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
                string SheetName = "New product Template";
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
                System.Data.DataRow dr = null; //dataGrdView.Visible = true;
                                               //dataGrdView.DataSource = dtExcel;
                dt.Columns.Add(new DataColumn("Prod_Code", typeof(string)));
                dt.Columns.Add(new DataColumn("Prod_Name", typeof(string)));
                dt.Columns.Add(new DataColumn("Prod_Type_Id", typeof(string)));
                dt.Columns.Add(new DataColumn("Prod_Group_Id", typeof(string)));
                dt.Columns.Add(new DataColumn("Prod_Primary_UOM_Id", typeof(string)));
                dt.Columns.Add(new DataColumn("Prod_Storage_Location_Id", typeof(string)));
                dt.Columns.Add(new DataColumn("Product_Bin_Id", typeof(string)));
                dt.Columns.Add(new DataColumn("Prod_Unit_Wt", typeof(string)));
                dt.Columns.Add(new DataColumn("Prod_PartNo", typeof(string)));
                dt.Columns.Add(new DataColumn("Prod_Mfg_Code", typeof(string)));
                dt.Columns.Add(new DataColumn("Prod_Description", typeof(string)));
                dt.Columns.Add(new DataColumn("Prod_HSN_Code", typeof(string)));
                dt.Columns.Add(new DataColumn("Prod_Tax_Class", typeof(string)));
                dt.Columns.Add(new DataColumn("Prod_Field2", typeof(string)));
                dt.Columns.Add(new DataColumn("Company_ID", typeof(string)));
                dt.Columns.Add(new DataColumn("Created_By", typeof(string)));
                dt.Columns.Add(new DataColumn("Modified_BY", typeof(string)));
                dt.Columns.Add(new DataColumn("Prod_Alternative_Code", typeof(string)));
                for (int i = 0; i < count; i++)
                {
                    dr = dt.NewRow();
                    //txtProdID.Text = result.FirstOrDefault().Product_Code;
                    //dr["Prod_Code"] = result.FirstOrDefault().Product_Code; ;
                    dr["Prod_Name"] = DtSet.Rows[i]["Prod_Name"].ToString();
                    dr["Prod_Type_Id"] = DtSet.Rows[i]["Prod_Type_Id"].ToString();
                    dr["Prod_Group_Id"] = DtSet.Rows[i]["Prod_Group_Id"].ToString();
                    dr["Prod_Primary_UOM_Id"] = DtSet.Rows[i]["Prod_Primary_UOM_Id"].ToString(); ;
                    dr["Prod_Storage_Location_Id"] = DtSet.Rows[i]["Prod_Storage_Location_Id"].ToString();
                    dr["Product_Bin_Id"] = DtSet.Rows[i]["Product_Bin_Id"].ToString();
                    dr["Prod_Unit_Wt"] = DtSet.Rows[i]["Prod_Unit_Wt"].ToString();
                    dr["Prod_PartNo"] = DtSet.Rows[i]["Plant1_Code"].ToString();
                    dr["Prod_Mfg_Code"] = DtSet.Rows[i]["Plant2_Code"].ToString(); ;
                    dr["Prod_Description"] = DtSet.Rows[i]["Prod_Description"].ToString();
                    dr["Prod_HSN_Code"] = DtSet.Rows[i]["Prod_HSN_Code"].ToString();
                    dr["Prod_Tax_Class"] = DtSet.Rows[i]["Prod_Tax_Class"].ToString(); ;
                    dr["Prod_Field2"] = DtSet.Rows[i]["Drawing_No"].ToString(); ;
                    dr["Prod_Alternative_Code"] = DtSet.Rows[i]["Prod_Alternative_Code"].ToString(); ;
                    dr["Company_ID"] = logIn.company;
                    dr["Created_By"] = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                    dr["Modified_BY"] = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

                    dt.Rows.Add(dr);
                }

                //dgProductData.DataSource = dt;
                MyConnection.Close();
                for (int i = 0; i <= dt.Rows.Count - 1; i++)
                {
                    if ((from u in db.Products where u.Prod_Name == dt.Rows[i]["Prod_Name"].ToString() && u.Company_ID == logIn.company select u).Count() > 0)
                    {
                    }
                    else
                    {
                        Product p = new Product();
                        var result = db.Sp_autoincrement_ProdMaster_New(Convert.ToInt32(dt.Rows[i]["Prod_Type_Id"].ToString()), Convert.ToInt32(dt.Rows[i]["Prod_Group_Id"].ToString()), logIn.company);

                        p.Prod_Code = result.FirstOrDefault().Product_Code;
                        p.Prod_Group_Id = Convert.ToInt32(dt.Rows[i]["Prod_Group_Id"].ToString());
                        p.Prod_Type_Id = Convert.ToInt32(dt.Rows[i]["Prod_Type_Id"].ToString());
                        p.Prod_Name = dt.Rows[i]["Prod_Name"].ToString();
                        p.Prod_Storage_Location_Id = (dt.Rows[i]["Prod_Unit_Wt"].ToString() == "") ? Convert.ToInt32("0") : Convert.ToInt32(dt.Rows[i]["Prod_Storage_Location_Id"].ToString());
                        p.Prod_Primary_UOM_Id = Convert.ToInt32(dt.Rows[i]["Prod_Primary_UOM_Id"].ToString());
                        p.Prod_Alternative_UOM_Id = Convert.ToInt32(dt.Rows[i]["Prod_Primary_UOM_Id"].ToString());
                        p.Prod_Unit_Wt = (dt.Rows[i]["Prod_Unit_Wt"].ToString() == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(dt.Rows[i]["Prod_Unit_Wt"].ToString());
                        p.Prod_Tax_Class = Convert.ToInt32(dt.Rows[i]["Prod_Tax_Class"].ToString());
                        p.Prod_Description = (dt.Rows[i]["Prod_Description"].ToString() == "") ? "" : dt.Rows[i]["Prod_Description"].ToString();
                        p.Prod_HSN_Code = (dt.Rows[i]["Prod_HSN_Code"].ToString() == "") ? "" : dt.Rows[i]["Prod_HSN_Code"].ToString();
                        //                p.Prod_Status_ID = Convert.ToInt32(dt.Rows[i]["Prod_Status_ID"].ToString());
                        p.Prod_Mfg_Code = (dt.Rows[i]["Prod_Mfg_Code"].ToString() == "") ? "" : dt.Rows[i]["Prod_Mfg_Code"].ToString();
                        p.Prod_PartNo = (dt.Rows[i]["Prod_PartNo"].ToString() == "") ? "" : dt.Rows[i]["Prod_PartNo"].ToString();
                        p.Prod_Field2 = (dt.Rows[i]["Prod_Field2"].ToString() == "") ? "" : dt.Rows[i]["Prod_Field2"].ToString();
                        p.Prod_Alternative_Code = (dt.Rows[i]["Prod_Alternative_Code"].ToString() == "") ? "" : dt.Rows[i]["Prod_Alternative_Code"].ToString();
                        p.Prod_Status_ID = 1;
                        p.Created_By = logIn.username + "-" + DateTime.Now;
                        p.Modified_BY = logIn.username + "-" + DateTime.Now;
                        p.Company_ID = logIn.company;
                        db.Products.InsertOnSubmit(p);
                        db.SubmitChanges();
                    }
                    

                }

                MessageBox.Show("Data Uploaded Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString());
            }

        }
        //string filename = "";
        //OpenFileDialog fdlg = new OpenFileDialog();
        //fdlg.Title = "Select file";
        //fdlg.InitialDirectory = @"c:\";
        ////  fdlg.FileName = txtChooseFile.Text;
        //fdlg.Filter = "Excel Sheet(*.xls)|*.xls|All Files(*.*)|*.*";
        //fdlg.FilterIndex = 1;
        //fdlg.RestoreDirectory = true;
        //if (fdlg.ShowDialog() == DialogResult.OK)
        //{
        //    filename = fdlg.FileName;
        //    Application.DoEvents();
        //}


       // Cursor.Current = Cursors.WaitCursor;
                System.Data.OleDb.OleDbConnection MyConnection;
        System.Data.DataTable DtSet;
        System.Data.OleDb.OleDbDataAdapter MyCommand;

        //string SheetName = "New product Template";
        //// string ExcellSheet = ;

        //string str = "Provider = Microsoft.ACE.OLEDB.12.0; Data source=" + filename + ";Extended Properties=\"Excel 8.0;HDR= yes;\";";
        //MyConnection = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filename + ";Extended Properties='Excel 8.0;HDR=Yes'");

        //MyCommand = new System.Data.OleDb.OleDbDataAdapter("Select * From [" + SheetName + "$] ", MyConnection);
        ////MyCommand = new System.Data.OleDb.OleDbDataAdapter("Select * From  [" + txtExcellSheet.Text + "$] ", MyConnection);
        //MyCommand.TableMappings.Add("Table", filename);
        //DtSet = new System.Data.DataTable();
        //MyCommand.Fill(DtSet);
        //int count = dtExcel.Rows.Count;
        //DataTable dt = new DataTable();
        //System.Data.DataRow dr = null;
        //dt.Columns.Add(new DataColumn("Prod_Code", typeof(string)));
        //dt.Columns.Add(new DataColumn("Prod_Name", typeof(string)));
        //dt.Columns.Add(new DataColumn("Prod_Type_Id", typeof(string)));
        //dt.Columns.Add(new DataColumn("Prod_Group_Id", typeof(string)));
        //dt.Columns.Add(new DataColumn("Prod_Primary_UOM_Id", typeof(string)));
        //dt.Columns.Add(new DataColumn("Prod_Storage_Location_Id", typeof(string)));
        //dt.Columns.Add(new DataColumn("Product_Bin_Id", typeof(string)));
        //dt.Columns.Add(new DataColumn("Prod_Unit_Wt", typeof(string)));
        //dt.Columns.Add(new DataColumn("Prod_PartNo", typeof(string)));
        //dt.Columns.Add(new DataColumn("Prod_Mfg_Code", typeof(string)));
        //dt.Columns.Add(new DataColumn("Prod_Description", typeof(string)));
        //dt.Columns.Add(new DataColumn("Prod_HSN_Code", typeof(string)));
        //dt.Columns.Add(new DataColumn("Prod_Tax_Class", typeof(string)));
        //dt.Columns.Add(new DataColumn("Prod_Field2", typeof(string)));
        //dt.Columns.Add(new DataColumn("Company_ID", typeof(string)));
        //dt.Columns.Add(new DataColumn("Created_By", typeof(string)));
        //dt.Columns.Add(new DataColumn("Modified_BY", typeof(string)));
        //for (int i = 0; i < count; i++)
        //{
        //    dr = dt.NewRow();
        //    //txtProdID.Text = result.FirstOrDefault().Product_Code;
        //    //dr["Prod_Code"] = result.FirstOrDefault().Product_Code; ;
        //    dr["Prod_Name"] = DtSet.Rows[i]["Prod_Name"].ToString();
        //    dr["Prod_Type_Id"] = DtSet.Rows[i]["Prod_Type_Id"].ToString();
        //    dr["Prod_Group_Id"] = DtSet.Rows[i]["Prod_Group_Id"].ToString();
        //    dr["Prod_Primary_UOM_Id"] = DtSet.Rows[i]["Prod_Primary_UOM_Id"].ToString(); ;
        //    dr["Prod_Storage_Location_Id"] = DtSet.Rows[i]["Prod_Storage_Location_Id"].ToString();
        //    dr["Product_Bin_Id"] = DtSet.Rows[i]["Product_Bin_Id"].ToString();
        //    dr["Prod_Unit_Wt"] = DtSet.Rows[i]["Prod_Unit_Wt"].ToString();
        //    dr["Prod_PartNo"] = DtSet.Rows[i]["Prod_PartNo"].ToString();
        //    dr["Prod_Mfg_Code"] = DtSet.Rows[i]["Prod_Mfg_Code"].ToString(); ;
        //    dr["Prod_Description"] = DtSet.Rows[i]["Prod_Description"].ToString();
        //    dr["Prod_HSN_Code"] = DtSet.Rows[i]["Prod_HSN_Code"].ToString();
        //    dr["Prod_Tax_Class"] = DtSet.Rows[i]["Prod_Tax_Class"].ToString(); ;
        //    dr["Prod_Field2"] = DtSet.Rows[i]["Prod_Field2"].ToString(); ;
        //    dr["Company_ID"] = logIn.company;
        //    dr["Created_By"] = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
        //    dr["Modified_BY"] = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

        //    dt.Rows.Add(dr);
        //}
        //dgProductData.DataSource = dt;

        // MyConnection.Close();
        //for (int i = 0; i <= dt.Rows.Count - 1; i++)
        //{
        //    Product p = new Product();
        //    var result = db.Sp_autoincrement_ProdMaster_New(Convert.ToInt32(dt.Rows[i]["Prod_Type_Id"].ToString()), Convert.ToInt32(dt.Rows[i]["Prod_Group_Id"].ToString()), logIn.company);

        //    p.Prod_Code = result.FirstOrDefault().Product_Code;
        //    p.Prod_Group_Id = Convert.ToInt32(dt.Rows[i]["Prod_Group_Id"].ToString());
        //    p.Prod_Type_Id = Convert.ToInt32(dt.Rows[i]["Prod_Type_Id"].ToString());
        //    p.Prod_Name = dt.Rows[i]["Prod_Name"].ToString();
        //    p.Prod_Storage_Location_Id = (dt.Rows[i]["Prod_Unit_Wt"].ToString() == "") ? Convert.ToInt32("0") : Convert.ToInt32(dt.Rows[i]["Prod_Storage_Location_Id"].ToString());
        //    p.Prod_Primary_UOM_Id = Convert.ToInt32(dt.Rows[i]["Prod_Primary_UOM_Id"].ToString());
        //    p.Prod_Alternative_UOM_Id = Convert.ToInt32(dt.Rows[i]["Prod_Primary_UOM_Id"].ToString());
        //    p.Prod_Unit_Wt = (dt.Rows[i]["Prod_Unit_Wt"].ToString() == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(dt.Rows[i]["Prod_Unit_Wt"].ToString());
        //    p.Prod_Tax_Class = Convert.ToInt32(dt.Rows[i]["Prod_Tax_Class"].ToString());
        //    p.Prod_Description = (dt.Rows[i]["Prod_Description"].ToString() == "") ? "" : dt.Rows[i]["Prod_Description"].ToString();
        //    p.Prod_HSN_Code = (dt.Rows[i]["Prod_HSN_Code"].ToString() == "") ? "" : dt.Rows[i]["Prod_HSN_Code"].ToString();
        //    //                p.Prod_Status_ID = Convert.ToInt32(dt.Rows[i]["Prod_Status_ID"].ToString());
        //    p.Prod_Mfg_Code = (dt.Rows[i]["Prod_Mfg_Code"].ToString() == "") ? "" : dt.Rows[i]["Prod_Mfg_Code"].ToString();
        //    p.Prod_Field2 = (dt.Rows[i]["Prod_Field2"].ToString() == "") ? "" : dt.Rows[i]["Prod_Field2"].ToString();

        //    p.Prod_Status_ID = 1;
        //    p.Created_By = logIn.username + "-" + DateTime.Now;
        //    p.Modified_BY = logIn.username + "-" + DateTime.Now;
        //    p.Company_ID = logIn.company;
        //    db.Products.InsertOnSubmit(p);
        //    db.SubmitChanges();



        //}
    
          
        private void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                var options = new ExcelExportingOptions();
                options.StartRowIndex = 5;
                var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
                var workBook = excelEngine.Excel.Workbooks[0];
                var ws = excelEngine.Excel.Worksheets[1];
                workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
                workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
                workBook.Worksheets[0].Range["A2"].Value = "PRODUCTS LIST";
                //workBook.Worksheets[0].Range["D2"].Value = "As on :" + dt1;
                workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
                workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
                workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
                workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
                workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
                workBook.Worksheets[0].PageSetup.Zoom = 85;
                workBook.Worksheets[0].PageSetup.PrintGridlines = true;
                string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                workBook.SaveAs(Fname + "\\Products_List.xlsx");
                string doc = Fname + "\\Products_List.xlsx";
                Process prc = new Process();
                prc.StartInfo.FileName = doc;
                prc.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                int i = sfDataGrid1.CurrentCell.RowIndex;
                if (i >= 0)
                {

                    var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["prod_id"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    //var currentCellV00alue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                    productCode = Convert.ToInt32(cellVaue.ToString());
                    var = "2";
                    ioneNet.MaterialManagement.Masters.frmProductsNew frm = new ioneNet.MaterialManagement.Masters.frmProductsNew();
                    frm.ShowDialog();

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

        private void transferDataItemToItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Transactions.frmshiftItemStock frm = new Transactions.frmshiftItemStock();
            //frm.MdiParent = this.MdiParent;
            var = "1";
            frm.ShowDialog();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }
    }
}
