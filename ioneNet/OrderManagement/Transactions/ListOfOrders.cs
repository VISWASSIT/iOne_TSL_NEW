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
using System.Diagnostics;
using System.Data.OleDb;
using Syncfusion.WinForms.GridCommon.ScrollAxis;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Ione_DAL;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGridConverter;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using Newtonsoft.Json.Linq;

namespace ioneNet.OrderManagement.Transactions
{
    public partial class ListOfOrders : Form
    {
        public static string InvoiceNoList, Order_NoList, SO_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;
        DataClasses1DataContext db = new DataClasses1DataContext();
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        private string path;

        private void dgvRecordList_FilterStringChanged(object sender, EventArgs e)
        {
            //this.showSOListBindingSource.Filter = dgvRecordList.FilterString;
        }

        private void dgvRecordList_SortStringChanged(object sender, EventArgs e)
        {
            //this.showSOListBindingSource.Sort = dgvRecordList.SortString;
        }

        private void approveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string cellValue;
                for (int i = 1; i < sfDataGrid1.RowCount; i++)
                {
                    foreach (var item in sfDataGrid1.SelectedItems)
                    {

                        //foreach (var col in sfDataGrid1.Columns)
                        //{
                        //if (col.MappingName == "Alternative_Code")
                        //{
                        //var mappingName = sfDataGrid1.Columns[columnIndex].MappingName;
                        //var recordIndex = sfDataGrid1.TableControl.ResolveToRecordIndex(rowIndex);
                        var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                        var mappingName = sfDataGrid1.Columns[0].MappingName;
                        //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                        //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                        if (rowData == item)
                        {
                            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                            SqlCommand cmd = new SqlCommand();

                            SO_No = cellVaue.ToString();
                            cmd.CommandText = "Update Sale_Order_Master set status = '6' where So_No=@param1 and Company_ID =@compName";
                            cmd.Parameters.AddWithValue("@param1", SO_No);
                            cmd.Parameters.AddWithValue("@CompName", logIn.company);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            cmd.Parameters.Clear();
                            string strT = logIn.username + "-" + DateTime.Now;
                            cmd.CommandText = "Update Sale_Order_Master set Modified_By = @strT where So_No=@param1 and Company_ID =@compName";
                            cmd.Parameters.AddWithValue("@strT", strT);
                            cmd.Parameters.AddWithValue("@param1", SO_No);
                            cmd.Parameters.AddWithValue("@CompName", logIn.company);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                MessageBox.Show("Selected Order(s) Are Approved Successfully");
                BindOrderslist();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void accountPostingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                string cellValue;
                for (int i = 1; i < sfDataGrid1.RowCount; i++)
                {
                    foreach (var item in sfDataGrid1.SelectedItems)
                    {

                        //foreach (var col in sfDataGrid1.Columns)
                        //{
                        //if (col.MappingName == "Alternative_Code")
                        //{
                        //var mappingName = sfDataGrid1.Columns[columnIndex].MappingName;
                        //var recordIndex = sfDataGrid1.TableControl.ResolveToRecordIndex(rowIndex);
                        var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                        var mappingName = sfDataGrid1.Columns[0].MappingName;
                        //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                        //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                        if (rowData == item)
                        {
                            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                            SqlCommand cmd = new SqlCommand();

                            SO_No = cellVaue.ToString();
                            cmd.CommandText = "Update Sale_Order_Master set status = '26' where So_No=@param1 and Company_ID =@compName";
                            cmd.Parameters.AddWithValue("@param1", SO_No);
                            cmd.Parameters.AddWithValue("@CompName", logIn.company);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            cmd.Parameters.Clear();
                            string strT = logIn.username + "-" + DateTime.Now;
                            cmd.CommandText = "Update Sale_Order_Master set Modified_By = @strT where So_No=@param1 and Company_ID =@compName";
                            cmd.Parameters.AddWithValue("@strT", strT);
                            cmd.Parameters.AddWithValue("@param1", SO_No);
                            cmd.Parameters.AddWithValue("@CompName", logIn.company);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                MessageBox.Show("Selected Order(s) Are Pre-Closed Successfully");
                BindOrderslist();





                //foreach (i= 0; i <= dgvRecordList.Rows.Count;if++)
                //for (int i = 0; i < dgvRecordList.Rows.Count - 1; i++)
                //{
                ////DataGridViewRow row = i;

                //bool isSelected = Convert.ToBoolean(dgvRecordList.Rows[i].Cells["SelOrd"].Value);
                //if (isSelected)
                //{

                //    SqlCommand cmd = new SqlCommand();

                //    SO_No = dgvRecordList.Rows[i].Cells["sONODataGridViewTextBoxColumn"].Value.ToString();
                //    cmd.CommandText = "Update Sale_Order_Master set status = '26' where So_No=@param1 and Company_ID =@compName";
                //    cmd.Parameters.AddWithValue("@param1", SO_No);
                //    cmd.Parameters.AddWithValue("@CompName", logIn.company);                       
                //    cmd.Connection = con;
                //    con.Open();
                //    cmd.ExecuteNonQuery();
                //    con.Close();
                //    cmd.Parameters.Clear();
                //    string strT = logIn.username + "-" + DateTime.Now;
                //    cmd.CommandText = "Update Sale_Order_Master set Modified_By = @strT where So_No=@param1 and Company_ID =@compName";
                //    cmd.Parameters.AddWithValue("@strT", strT);
                //    cmd.Parameters.AddWithValue("@param1", SO_No);
                //    cmd.Parameters.AddWithValue("@CompName", logIn.company);
                //    cmd.Connection = con;
                //    con.Open();
                //    cmd.ExecuteNonQuery();
                //    con.Close();
                //}
                ////else
                ////{
                ////    MessageBox.Show("Atlease One So No to be Selected to Approve");
                ////}
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {

                    string cellValue;
                    for (int i = 1; i < sfDataGrid1.RowCount; i++)
                    {
                        foreach (var item in sfDataGrid1.SelectedItems)
                        {

                            //foreach (var col in sfDataGrid1.Columns)
                            //{
                            //if (col.MappingName == "Alternative_Code")
                            //{
                            //var mappingName = sfDataGrid1.Columns[columnIndex].MappingName;
                            //var recordIndex = sfDataGrid1.TableControl.ResolveToRecordIndex(rowIndex);
                            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                            var mappingName = sfDataGrid1.Columns[0].MappingName;
                            var mappingName1 = sfDataGrid1.Columns[9].MappingName;
                            //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                            //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                            if (rowData == item)
                            {
                                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                                var cellStatus = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                                if (cellStatus.ToString() != "Despatches Started")
                                {
                                    SqlCommand cmd = new SqlCommand();

                                    SO_No = cellVaue.ToString();
                                    cmd.CommandText = "Update Sale_Order_Master set status = '24' where So_No=@param1 and Company_ID =@compName";
                                    cmd.Parameters.AddWithValue("@param1", SO_No);
                                    cmd.Parameters.AddWithValue("@CompName", logIn.company);
                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                    cmd.Parameters.Clear();
                                    string strT = logIn.username + "-" + DateTime.Now;
                                    cmd.CommandText = "Update Sale_Order_Master set Modified_By = @strT where So_No=@param1 and Company_ID =@compName";
                                    cmd.Parameters.AddWithValue("@strT", strT);
                                    cmd.Parameters.AddWithValue("@param1", SO_No);
                                    cmd.Parameters.AddWithValue("@CompName", logIn.company);
                                    cmd.Connection = con;
                                    con.Open();
                                    cmd.ExecuteNonQuery();
                                    con.Close();
                                }
                                else
                                {
                                    MessageBox.Show("Selected Order Cannot Be Deleted As Already Despatches Started, Pre-Close the order insted");
                                }
                            }
                        }
                    }
                    MessageBox.Show("Selected Order(s) Are Deleted Successfully");
                    BindOrderslist();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvRecordList_DataSourceChanged(object sender, EventArgs e)
        {
            //for (int i = 0; i < dgvRecordList.Rows.Count; i++)
            //{
                
            //    if (dgvRecordList.Rows[i].Cells["statusDataGridViewTextBoxColumn"].Value.ToString() == "Created")
            //    {
            //        dgvRecordList.Rows[i].Cells["statusDataGridViewTextBoxColumn"].Style.ForeColor = Color.OrangeRed;
            //        dgvRecordList.Rows[i].Cells["statusDataGridViewTextBoxColumn"].Style.Font = new Font("Bold", 10);
            //        // dgorders.Rows[i].DefaultCellStyle.BackColor = Color.Orange;
            //    }
            //    else if (dgvRecordList.Rows[i].Cells["statusDataGridViewTextBoxColumn"].Value.ToString() == "Approved")
            //    {
            //        dgvRecordList.Rows[i].Cells["statusDataGridViewTextBoxColumn"].Style.ForeColor = Color.DarkGreen;
            //        dgvRecordList.Rows[i].Cells["statusDataGridViewTextBoxColumn"].Style.Font = new Font("Bold", 10);
            //        // dgorders.Rows[i].DefaultCellStyle.BackColor = Color.LightPink;
            //    }
               
            //    else if (dgvRecordList.Rows[i].Cells["statusDataGridViewTextBoxColumn"].Value.ToString() == "Pre-Closed")
            //    {
            //        dgvRecordList.Rows[i].Cells["statusDataGridViewTextBoxColumn"].Style.ForeColor = Color.DarkViolet;
            //        dgvRecordList.Rows[i].Cells["statusDataGridViewTextBoxColumn"].Style.Font = new Font("Bold", 10);
            //        //dgorders.Rows[i].DefaultCellStyle.BackColor = Color.Violet;
            //    }
            //    else if (dgvRecordList.Rows[i].Cells["statusDataGridViewTextBoxColumn"].Value.ToString() == "Shipped")
            //    {
            //        dgvRecordList.Rows[i].Cells["statusDataGridViewTextBoxColumn"].Style.ForeColor = Color.SkyBlue;
            //        dgvRecordList.Rows[i].Cells["statusDataGridViewTextBoxColumn"].Style.Font = new Font("Bold", 10);
            //        //dgorders.Rows[i].DefaultCellStyle.BackColor = Color.SkyBlue;
            //    }
               
            //}
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {

        }

        private void sfDataGrid1_Click(object sender, EventArgs e)
        {

        }

        private void ListOfOrders_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'viswaSiOneDataSet.Order_Data_Monthly_for_Chart' table. You can move, or remove it, as needed.
            //showSOListTableAdapter.Fill(ioneDataSet.ShowSOList, logIn.company,null);
            ////dgvRecordList.DataSource = showSOListBindingSource;
            //sfDataGrid1.DataSource = showSOListBindingSource;

            //string pcode = "%" + txtSearch.Text + "%";
            BindOrderslist();



        }
        void sfDataGrid_QueryCellStyle(object sender, Syncfusion.WinForms.DataGrid.Events.QueryCellStyleEventArgs e)
        {
            var rowColumnIndex = new RowColumnIndex(e.RowIndex, e.ColumnIndex);
            if (colorDict.ContainsKey(rowColumnIndex))
                e.Style.BackColor = colorDict[rowColumnIndex];
        }

        private void allotStockToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = sfDataGrid1.CurrentCell.RowIndex;
            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
            //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
            var currentCellValue = (rowData.GetType().GetProperty("SO_NO").GetValue(rowData, null).ToString());

            SO_No = currentCellValue.ToString();
            OrderManagement.Transactions.Forge_StockAllotment frm = new Forge_StockAllotment();
            //frm.MdiParent = this.MdiParent;
            frm.ShowDialog();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {

            var options = new ExcelExportingOptions();
            options.StartRowIndex = 5;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            workBook.Worksheets[0].Range["A2"].Value = "Orders List";
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\OrdersList.xlsx");
            string doc = Fname + "\\OrdersList.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

            rep = new OrderManagement.Transactions.Sale_Order();
            path = Path.Combine(Directory.GetCurrentDirectory(), "Sale_Order.pdf");
            //string path = @"D:\Invoice.pdf";
            FileInfo fi1 = new FileInfo(path);


            int i = sfDataGrid1.CurrentCell.RowIndex;
            var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
            var mappingName = sfDataGrid1.Columns[0].MappingName;
            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            // string path1 = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.pdf");
            SqlCommand cmd1 = con.CreateCommand();
            SqlCommand cmd2 = con.CreateCommand();
            if (con.State != ConnectionState.Open)
                con.Open();
            //int i = sfDataGrid1.CurrentRow.Index;
            SO_No = cellVaue.ToString();
            SqlCommand cmd = new SqlCommand("sp_Rpt_SaleorderReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@SO_No", SO_No);
            cmd.Parameters.AddWithValue("@Creation_Company", logIn.company);
            cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);

            DataTable Dt = new DataTable();

            da.SelectCommand = cmd;
            da.Fill(Dt);
            if (Dt.Rows.Count > 0)
            {


                crConnectionInfo.ServerName = frmMain.ServerIP;
                crConnectionInfo.DatabaseName = frmMain.Database;
                crConnectionInfo.UserID = frmMain.DBUserID;
                crConnectionInfo.Password = frmMain.Password;


                crDatabase = rep.Database;
                crTables = crDatabase.Tables;
                //Loop through all tables in the report and apply the connection information for each table.
                for (int k = 0; k < crTables.Count; k++)
                {
                    //  crTable = crTables[i];
                    crTableLogOnInfo = crTables[k].LogOnInfo;
                    crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                    crTables[k].ApplyLogOnInfo(crTableLogOnInfo);

                }
                rep.SetDataSource(Dt);
                string CAddr = "";
                string CCity = "";
                string cState = "";
                String cGSTIN = "";

                var da1 = (from so in db.Sale_Order_Masters
                          
                        
                           where so.SO_NO == SO_No && so.Company_ID == logIn.company && so.Status != 24
                           select new
                           {
                               so.Delivery_Address,
                               so.Delivery_GSTIN                               
                           }).ToList();


                if (da1.Count > 0)
                {

                    //                ValidateJSON(ca[0].ConsigneeAddress);
                    if (Mid(da1[0].Delivery_Address, 3, 4) == "Addr")
                    {
                        JObject jsoncancel = JObject.Parse(da1[0].Delivery_Address);

                        CAddr = (string)jsoncancel.SelectToken("Address1") + "," + (string)jsoncancel.SelectToken("Address2");
                        CCity = (string)jsoncancel.SelectToken("City") + "," + (string)jsoncancel.SelectToken("PinCode");
                        cState = (string)jsoncancel.SelectToken("State") + ", State Code : " + (string)jsoncancel.SelectToken("StateCode");
                        cGSTIN = "GSTIN : " + (string)jsoncancel.SelectToken("GSTIN");

                    }
                    else
                    {
                        CAddr = da1[0].Delivery_Address;
                        cGSTIN = "GSTIN : " + da1[0].Delivery_GSTIN;
                        CCity = "";
                    }
                    //JToken.Parse(ca[0].ConsigneeAddress);

                }


                //rep.SetParameterValue("Creation_Company", logIn.company);
                ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                // cmd1.Parameters.AddWithValue("@Con_Address1", "Door No");

                rep.SetParameterValue("Con_Address1", CAddr);
                rep.SetParameterValue("Con_City", CCity);
                rep.SetParameterValue("Con_State", cState);
                rep.SetParameterValue("Con_GSTIN", cGSTIN);

                viewer.crystalReportViewer1.ReportSource = rep;
                viewer.crystalReportViewer1.Refresh();
                rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                cmd.Parameters.Clear();
                Process.Start(path);
            }
            con.Close();
        }
        public static string Mid(string s, int a, int b)
        {
            string temp = s.Substring(a - 1, b);
            return temp;
        }

        private void holdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                string cellValue;
                for (int i = 1; i < sfDataGrid1.RowCount; i++)
                {
                    foreach (var item in sfDataGrid1.SelectedItems)
                    {

                        //foreach (var col in sfDataGrid1.Columns)
                        //{
                        //if (col.MappingName == "Alternative_Code")
                        //{
                        //var mappingName = sfDataGrid1.Columns[columnIndex].MappingName;
                        //var recordIndex = sfDataGrid1.TableControl.ResolveToRecordIndex(rowIndex);
                        var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                        var mappingName = sfDataGrid1.Columns[0].MappingName;
                        //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                        //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                        if (rowData == item)
                        {
                            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                            SqlCommand cmd = new SqlCommand();

                            SO_No = cellVaue.ToString();
                            cmd.CommandText = "Update Sale_Order_Master set status = '12276' where So_No=@param1 and Company_ID =@compName";
                            cmd.Parameters.AddWithValue("@param1", SO_No);
                            cmd.Parameters.AddWithValue("@CompName", logIn.company);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            cmd.Parameters.Clear();
                            string strT = logIn.username + "-" + DateTime.Now;
                            cmd.CommandText = "Update Sale_Order_Master set Modified_By = @strT where So_No=@param1 and Company_ID =@compName";
                            cmd.Parameters.AddWithValue("@strT", strT);
                            cmd.Parameters.AddWithValue("@param1", SO_No);
                            cmd.Parameters.AddWithValue("@CompName", logIn.company);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                        }
                    }
                }
                MessageBox.Show("Status of Order Changed to Hold");
                BindOrderslist();





                //foreach (i= 0; i <= dgvRecordList.Rows.Count;if++)
                //for (int i = 0; i < dgvRecordList.Rows.Count - 1; i++)
                //{
                ////DataGridViewRow row = i;

                //bool isSelected = Convert.ToBoolean(dgvRecordList.Rows[i].Cells["SelOrd"].Value);
                //if (isSelected)
                //{

                //    SqlCommand cmd = new SqlCommand();

                //    SO_No = dgvRecordList.Rows[i].Cells["sONODataGridViewTextBoxColumn"].Value.ToString();
                //    cmd.CommandText = "Update Sale_Order_Master set status = '26' where So_No=@param1 and Company_ID =@compName";
                //    cmd.Parameters.AddWithValue("@param1", SO_No);
                //    cmd.Parameters.AddWithValue("@CompName", logIn.company);                       
                //    cmd.Connection = con;
                //    con.Open();
                //    cmd.ExecuteNonQuery();
                //    con.Close();
                //    cmd.Parameters.Clear();
                //    string strT = logIn.username + "-" + DateTime.Now;
                //    cmd.CommandText = "Update Sale_Order_Master set Modified_By = @strT where So_No=@param1 and Company_ID =@compName";
                //    cmd.Parameters.AddWithValue("@strT", strT);
                //    cmd.Parameters.AddWithValue("@param1", SO_No);
                //    cmd.Parameters.AddWithValue("@CompName", logIn.company);
                //    cmd.Connection = con;
                //    con.Open();
                //    cmd.ExecuteNonQuery();
                //    con.Close();
                //}
                ////else
                ////{
                ////    MessageBox.Show("Atlease One So No to be Selected to Approve");
                ////}
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void SetCellBackgroundColor(RowColumnIndex rowColumnIndex, Color color)
        {
            if (!colorDict.ContainsKey(rowColumnIndex))
                colorDict.Add(rowColumnIndex, color);
            else
                colorDict[rowColumnIndex] = color;
            sfDataGrid1.TableControl.Invalidate(this.sfDataGrid1.TableControl.GetCellRectangle(rowColumnIndex.RowIndex, rowColumnIndex.ColumnIndex, false));
        }



        public static Boolean editMode;
        public ListOfOrders()
        {
            InitializeComponent();
        }

        
        public void BindOrderslist()
        {
            try
            {
                var d = (from data in db.ShowSOList(logIn.company,logIn.fy_Start_Date,logIn.fy_End_Date, null,logIn.BU_ID) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["SO_NO"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["SO_NO"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["SO_NO"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["SO_NO"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["Customer_name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Customer_name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Customer_name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Customer_name"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["CustomerPoNo"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["CustomerPoNo"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["CustomerPoNo"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["CustomerPoNo"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["Status"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Status"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Status"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Status"].FilterRowCondition = FilterRowCondition.Contains;

                }
                this.sfDataGrid1.QueryCellStyle += sfDataGrid_QueryCellStyle;
                string cellValue;
                for (int i = 2; i < sfDataGrid1.RowCount; i++)
                {
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns[9].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    if (cellVaue.ToString() == "Approved")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 9), Color.Green);
                    }
                    if (cellVaue.ToString() == "Despatches Started")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 9), Color.LightSkyBlue);
                    }
                    if (cellVaue.ToString() == "Closed")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 9), Color.SaddleBrown);
                    }
                    if (cellVaue.ToString() == "Pre-Closed")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 9), Color.Orange);
                    }
                    if (cellVaue.ToString() == "Hold")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 9), Color.Red);
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
            editMode = false;
            if (logIn.company == 25)
            {
               // OrderManagement.Transactions.frmNewOrder_Forging frm = new frmNewOrder_Forging();
                //frm.MdiParent = this.MdiParent;
                //frm.Show();
            }
            else
            {
                OrderManagement.Transactions.frmNewOrder_TSL frm = new frmNewOrder_TSL();
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
          
            BindOrderslist();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //showSOListTableAdapter.Fill(ioneDataSet.ShowSOList, logIn.company, txtSearch.Text);
            //dgvRecordList.DataSource = showSOListBindingSource;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            BindOrderslist();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                //if (i >= 0)
                //{
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                    var currentCellValue =  (rowData.GetType().GetProperty("SO_NO").GetValue(rowData, null).ToString());
               
                    var mappingName = sfDataGrid1.Columns[9].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    if (cellVaue.ToString() != "Closed" || cellVaue.ToString() != "Pre Closed")
                    {

                        if (cellVaue.ToString() == "Created" ||  cellVaue.ToString() == "Approved" || cellVaue.ToString() == "Despatches Started")
                        {
                            var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Sale Order"  && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                            if (uRole.Count > 0)
                            {
                                if(uRole[0].Modify_Role==true)
                                {
                                    SO_No = currentCellValue.ToString();
                                    var = "0";
                                    editMode = true;
                                if (logIn.company == 25)
                                {
                                    //OrderManagement.Transactions.frmNewOrder_Forging frm = new frmNewOrder_Forging();
                                    //frm.MdiParent = this.MdiParent;
                                    //frm.Show();
                                }
                                else
                                {
                                    OrderManagement.Transactions.frmNewOrder_TSL frm = new frmNewOrder_TSL();
                                    frm.MdiParent = this.MdiParent;
                                    frm.Show();
                                }


                                //OrderManagement.Transactions.frmNewOrder frm = new frmNewOrder();
                                //    //OrderManagement.Transactions.
                                //    frm.MdiParent = this.MdiParent;
                                //    frm.Show();

                                }
                                else
                                {
                                    MessageBox.Show("You Have No Permissions to Modify The Approbed or Processed Order");
                                    return;
                                }
                         }
                        else
                        {
                            SO_No = currentCellValue.ToString();
                            var = "0";
                            editMode = true;
                            if (logIn.company == 25)
                            {
                                //OrderManagement.Transactions.frmNewOrder_Forging frm = new frmNewOrder_Forging();
                                //frm.MdiParent = this.MdiParent;
                                //frm.Show();
                            }
                            else
                            {
                                OrderManagement.Transactions.frmNewOrder_TSL frm = new frmNewOrder_TSL();
                                frm.MdiParent = this.MdiParent;
                                frm.Show();
                            }
                        }
                    }
                    else
                    {
                        if (logIn.company == 23)
                        {
                            SO_No = currentCellValue.ToString();
                            var = "0";
                            editMode = true;
                            OrderManagement.Transactions.frmNewOrder_TSL frm = new frmNewOrder_TSL();
                            frm.MdiParent = this.MdiParent;
                            frm.Show();
                        }
                        else
                        {
                            MessageBox.Show("The Order Cannot Be Modified Either Closed or Despatches Started");
                        }
                        //i1 = 0;
                    }
                }
                //else
                //{
                //    MessageBox.Show("No Order is Selected to Modify");

                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
