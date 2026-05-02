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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using Syncfusion.WinForms.GridCommon.ScrollAxis;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using System.Net.Mail;
using System.Net;
using Syncfusion.WinForms.DataGridConverter;
using Ione_DAL;
using Newtonsoft.Json.Linq;

namespace ioneNet.MaterialManagement
{
    public partial class PurchaseOrdersList : Form
    {
        public static string InvoiceNoList, Order_NoList, SO_No, SO_Amend_No,Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;
        DataClasses1DataContext db = new DataClasses1DataContext();
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();

        private void dgvRecordList_FilterStringChanged(object sender, EventArgs e)
        {
            //this.showPOListBindingSource.Filter = dgvRecordList.FilterString;
        }

        private void dgvRecordList_SortStringChanged(object sender, EventArgs e)
        {
            //this.showPOListBindingSource.Sort = dgvRecordList.SortString;
        }

        private void approveToolStripMenuItem_Click(object sender, EventArgs e)
        {
           try
           {
                for (int i = 2; i < sfDataGrid1.RowCount; i++)
                {

                    //foreach (var item in sfDataGrid1.SelectedItems)
                    //{
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["PO_NO"].MappingName;
                    var mappingName1 = sfDataGrid1.Columns["Status"].MappingName;
                    var mappingName2 = sfDataGrid1.Columns["Sel"].MappingName;
                    //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                    var cellVaue3 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
                    if (cellVaue3 == "True")
                    {
                        var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                        var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());

                        string Status = cellVaue1.ToString();
                        string OrdNo = cellVaue.ToString();
                        int al = 3;
                        int y = 0;
                        switch (Status)
                        {
                            case "Created":
                                y = 1;
                                var rRole = (from m in db.view_Trans_Auth_Levels
                                             where m.Company_ID == logIn.company && m.Menu_Item == "Purchase Order"
                                             && m.Auth_Level == y + 1
                                             select new { m.Auth_Level, m.Auth_Allowed, m.Status_Code }).Distinct().ToList();
                                if (rRole.Count > 0)
                                {
                                    y = 1;
                                    var uRole = (from m in db.view_Trans_Auth_Levels
                                                 where m.Company_ID == logIn.company && m.Menu_Item == "Purchase Order"
                                                 && m.Role_ID == logIn.UserRoleID && m.Auth_Level == y + 1
                                                 select new { m.Auth_Level, m.Auth_Allowed, m.Status_Code }).Distinct().ToList();
                                    if (uRole.Count > 0)
                                    {

                                        MessageBox.Show("Selected PR cannot be Approved as it is not yet Reviewed");
                                        return;

                                    }
                                    else
                                    {
                                        var aRole = (from m in db.view_Trans_Auth_Levels
                                                     where m.Company_ID == logIn.company && m.Menu_Item == "Purchase Order"
                                                     && m.Role_ID == logIn.UserRoleID && m.Auth_Level == al
                                                     select new { m.Auth_Level, m.Auth_Allowed, m.Status_Code }).Distinct().ToList();
                                        if (aRole.Count > 0)
                                        {
                                            if (aRole[0].Auth_Allowed == true)
                                            {
                                                var ci = db.Purchase_Order_Masters.Where(w => w.PO_NO == OrdNo && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                                                {

                                                    ci.Status = 6;
                                                    ci.Modified_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                                                    //ci.Approved_By = logIn.username + "-" + DateTime.Now;
                                                    db.SubmitChanges();

                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show("You Have No Permissions to Approve The Purchase Order");
                                                return;
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    var aRole = (from m in db.view_Trans_Auth_Levels
                                                  where m.Company_ID == logIn.company && m.Menu_Item == "Purchase Order"
                                                  && m.Role_ID == logIn.UserRoleID && m.Auth_Level == al
                                                  select new { m.Auth_Level, m.Auth_Allowed, m.Status_Code }).Distinct().ToList();
                                    if (aRole.Count > 0)
                                    {
                                        if (aRole[0].Auth_Allowed == true)
                                        {
                                            var ci = db.Purchase_Order_Masters.Where(w => w.PO_NO == OrdNo && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                                            {

                                                ci.Status = 6;
                                                ci.Modified_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                                                //ci.Approved_By = logIn.username + "-" + DateTime.Now;
                                                db.SubmitChanges();

                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show("You Have No Permissions to Approve The Purchase Order");
                                            return;
                                        }
                                    }
                                }
                                break;
                            case "Reviewed":
                                y = 2;
                                var uRole1 = (from m in db.view_Trans_Auth_Levels
                                              where m.Company_ID == logIn.company && m.Menu_Item == "Purchase Order"
                                              && m.Role_ID == logIn.UserRoleID && m.Auth_Level == al
                                              select new { m.Auth_Level, m.Auth_Allowed, m.Status_Code }).Distinct().ToList();
                                if (uRole1.Count > 0)
                                {
                                    if (uRole1[0].Auth_Allowed == true)
                                    {
                                        var ci = db.Purchase_Order_Masters.Where(w => w.PO_NO == OrdNo && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                                        {

                                            ci.Status = 6;
                                            ci.Modified_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                                            //ci.Approved_By = logIn.username + "-" + DateTime.Now;
                                            db.SubmitChanges();

                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("You Have No Permissions to Approve The Purchase Order");
                                        return;
                                    }
                                }
                                break;
                            case "Approved":
                                y = 3;
                                MessageBox.Show("PO No " + OrdNo + " Is Already Approved or Further Processed.. No Work Done");
                                break;
                            default:
                                MessageBox.Show("PO No " + OrdNo + " Is Already Approved or Further Processed.. No Work Done");
                                break;

                        }
                    }

                }
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
                       var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                        var mappingName = sfDataGrid1.Columns["Rec_ID"].MappingName;
                        if (rowData == item)
                        {
                            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                            var ci = db.Purchase_Order_Masters.Where(w => w.Id == Convert.ToInt32(cellVaue) && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                            {
                                ci.Status = 26;
                                ci.Modified_By = logIn.username + "-" + DateTime.Now;
                                db.SubmitChanges();
                            }                           
                        }
                    }
                }
                MessageBox.Show("Selected Order(s) Are Pre-Closed Successfully");
                BindOrderslist();
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
                    int i = sfDataGrid1.CurrentCell.RowIndex;
                    var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["Rec_ID"].MappingName;
                    var mappingName1 = sfDataGrid1.Columns["Status"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());

                    if (cellVaue.ToString() != "")
                    {
                        string custId = cellVaue;
                        if (cellVaue1 == "Created")
                        {

                            var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Purchase Order" && m.Role_ID == logIn.UserRoleID select new { m.Delete_Role }).Distinct().ToList();
                            if (uRole.Count > 0)
                            {
                                if (uRole[0].Delete_Role == true)
                                {
                                    var ci = db.Purchase_Order_Masters.Where(w => w.Id == Convert.ToInt32(custId) && w.Company_ID == logIn.company).FirstOrDefault();
                                    {
                                        ci.Status = 24;
                                        ci.Modified_By = logIn.username + "-" + DateTime.Now;
                                        db.SubmitChanges();
                                        MessageBox.Show("Purchase Order Deleted Sucessfully");
                                        BindOrderslist();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("You Have No Permissions to Delete The Purchase Orders");
                                    return;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("This PO is Already Approved or Process Started, Cannot Be Deleted");
                            return;
                        }

                    }
                    else
                    {
                        MessageBox.Show("Please Select Any One Purchase Order");
                    }
                }

                //foreach (DataGridViewRow row in dgvRecordList.Rows)
                //{
                //    bool isSelected = Convert.ToBoolean(row.Cells["SelOrd"].Value);
                //    if (isSelected)
                //    {
                //        SqlCommand cmd = new SqlCommand();
                //        SO_No = row.Cells["sONODataGridViewTextBoxColumn"].Value.ToString();
                //        cmd.CommandText = "Update Sale_Order_Master_New set status = 'Deleted' where So_No=@param1 and Company_ID =@compName";
                //        cmd.Parameters.AddWithValue("@param1", SO_No);
                //        cmd.Parameters.AddWithValue("@CompName", logIn.company);
                //        cmd.Connection = con;
                //        con.Open();
                //        cmd.ExecuteNonQuery();
                //        con.Close();
                //        string strT = logIn.username + "-" + DateTime.Now;
                //        cmd.CommandText = "Update Sale_Order_Master_New set Modified_By = @strT where So_No=@param1 and Company_ID =@compName";
                //        cmd.Parameters.AddWithValue("@strT", strT);
                //        cmd.Parameters.AddWithValue("@param1", SO_No);
                //        cmd.Parameters.AddWithValue("@CompName", logIn.company);
                //        cmd.Connection = con;
                //        con.Open();
                //        cmd.ExecuteNonQuery();
                //        con.Close();
                //    }
                //    else
                //    {
                //        MessageBox.Show("Atlease One So No to be Selected to Approve");
                //    }
                //}
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName2 = sfDataGrid1.Columns["import_po"].MappingName;               
                
                //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                var cellVaue3 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());

                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                //var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["Rec_ID"].MappingName;
                //var mappingName1 = sfDataGrid1.Columns[4].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                //crys viewer1 = new CrstalReportViewer1();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "PurchaseOrder.pdf");
                //string path = Path.Combine(Directory.GetCurrentDirectory(), @"\Purchase_Order.pdf");
                System.IO.FileInfo fi = new System.IO.FileInfo(path);
                Cursor.Current = Cursors.WaitCursor;
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
           //     rep = new MaterialManagement.Transactions.PurchaseOrder();
                var gstno = (from c in db.Purchase_Order_Masters
                             where c.Company_ID == logIn.company && c.Id == Convert.ToInt32(cellVaue)
                             select new { c.Raw_Material_PO }).ToList();
                if (gstno.Count > 0)
                {
                    if (gstno[0].Raw_Material_PO == true)
                    {
                        rep = new MaterialManagement.Transactions.rptPurchaseOrder_Others();
                    }
                    else
                    {
                        rep = new MaterialManagement.Transactions.rptPurchaseOrder_SMPL();
                    }
                }
                
               

                SqlCommand cmd = new SqlCommand("sp_Rpt_PurchaseOrder", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PO_No", cellVaue);
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


                    //rep.SetParameterValue("Invoice_No", SO_No);
                    //rep.SetParameterValue("Creation_Company", logIn.company);
                    ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                    string shippTo = "";
                    string CAddr = "";
                    string CCity = "";
                    string cState = "";
                    String cGSTIN = "";

                    var da1 = (from so in db.Purchase_Order_Masters

                               join c in db.Costing_Units on so.Ship_To equals c.id
                               where so.Id == Convert.ToInt32(cellVaue) && so.BU_ID == logIn.BU_ID && so.Status != 24
                               select new
                               {
                                   c.ToPrintName,
                                   so.Shipping_Address,
                                   c.Address,
                                   c.City,
                                   c.State,
                                   c.State_Code,
                                   c.PinCode,
                                   c.GST_No

                               }).ToList();


                    if (da1.Count > 0)
                    {

                        //                ValidateJSON(ca[0].ConsigneeAddress);
                        if (da1[0].Shipping_Address == null)
                        {
                            shippTo = da1[0].ToPrintName;
                            CAddr = da1[0].Address;
                            CCity = da1[0].City + "-" + da1[0].PinCode;
                            cState = da1[0].State + ", State Code: "+ da1[0].State_Code;
                            cGSTIN = da1[0].GST_No;

                        }
                        else
                        { 
                            JObject jsoncancel = JObject.Parse(da1[0].Shipping_Address);
                            shippTo = (string)jsoncancel.SelectToken("ShipTo");
                            CAddr = (string)jsoncancel.SelectToken("Address") ;
                            CCity = (string)jsoncancel.SelectToken("City") + "," + (string)jsoncancel.SelectToken("PinCode");
                            cState = (string)jsoncancel.SelectToken("State") + ", State Code : " + (string)jsoncancel.SelectToken("StateCode");
                            cGSTIN = "GSTIN : " + (string)jsoncancel.SelectToken("GSTIN");

                        }
                        
                        //JToken.Parse(ca[0].ConsigneeAddress);

                    }


                    //rep.SetParameterValue("Creation_Company", logIn.company);
                    // cmd1.Parameters.AddWithValue("@Con_Address1", "Door No");
                    rep.SetParameterValue("shipto", shippTo);
                    rep.SetParameterValue("ShipAddress", CAddr);
                    rep.SetParameterValue("ShipCity", CCity);
                    rep.SetParameterValue("shipState", cState);
                    rep.SetParameterValue("shipGSTIN", cGSTIN);



                    // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                    viewer.crystalReportViewer1.ReportSource = rep;
                    viewer.crystalReportViewer1.Refresh();
                    rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);
                    
                    cmd.Parameters.Clear();
                    Process.Start(path);
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ListOfOrders_Load(object sender, EventArgs e)
        {
            try
            {
                BindOrderslist();
                var bindLoc = (from m in db.User_Roles
                               where m.Company_ID == logIn.company && m.Role_ID == logIn.UserRoleID && m.Form_Name== "Purchase Order"
                               select new
                               {
                                   m.Review_Role,
                                   m.Approve_Role,
                                   m.View_Role,
                                   m.Modify_Role,
                                   m.Create_Role,
                                   m.Delete_Role
                               }).ToList();
                this.reviewedToolStripMenuItem.Enabled = false;
                if (bindLoc[0].Review_Role == true)
                {
                    this.reviewedToolStripMenuItem.Enabled = true;
                }
                this.modifyToolStripMenuItem.Enabled = false;
                if (bindLoc[0].Modify_Role == true)
                {
                    this.modifyToolStripMenuItem.Enabled = true;
                }
                this.approveToolStripMenuItem.Enabled = false;
                if (bindLoc[0].Approve_Role == true)
                {
                    this.approveToolStripMenuItem.Enabled = true;
                }
                this.amendmentToolStripMenuItem.Enabled = false;
                if (bindLoc[0].Modify_Role == true)
                {
                    this.amendmentToolStripMenuItem.Enabled = true;
                }

                this.deleteToolStripMenuItem.Enabled = false;
                if (bindLoc[0].Delete_Role == true)
                {

                    this.deleteToolStripMenuItem.Enabled = true;
                }
                btnAddNew.Enabled = false;
                if (bindLoc[0].Create_Role == true)
                {
                    btnAddNew.Enabled = true;
                }

                //showPOListTableAdapter.Fill(ioneDataSet.ShowPOList, logIn.company," ");
                //dgvRecordList.DataSource = showPOListBindingSource;
            }
            catch(Exception ex)
            {
                //showPOListTableAdapter.Fill(ioneDataSet.ShowPOList, logIn.company, " ");
                //dgvRecordList.DataSource = showPOListBindingSource;
            }
           
        }

        public static Boolean editMode;

        public PurchaseOrdersList()
        {
            InitializeComponent();
        }

        private void eMailToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int i = sfDataGrid1.CurrentCell.RowIndex;
            var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
            var mappingName = sfDataGrid1.Columns["Rec_ID"].MappingName;
            var mappingName1 = sfDataGrid1.Columns["Status"].MappingName;
            var mappingName2 = sfDataGrid1.Columns["Sel"].MappingName;
            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            var cellvalue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
            var cellVaue3 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
            if (cellVaue.ToString() != "")
            {
                if (cellvalue1.ToString() == "Created")
                {
                    MessageBox.Show("The Selected PO is Not Yet Approved, E Mail Cannot Sent", "E Mail Confirmation");
                   
                }
                else
                {
                    SendEmail();
                }
            }

                  
        }

        private void sfButton4_Click(object sender, EventArgs e)
        {
            
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 5;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname + "-" + logIn.BU_ID;
            workBook.Worksheets[0].Range["A2"].Value = "Purchase Order List";            
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\PO_List.xlsx");
            string doc = Fname + "\\PO_List.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
           
        }

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["PO_NO"].MappingName;
                var currentAmendValue = (rowData.GetType().GetProperty("Po_Amend_No").GetValue(rowData, null).ToString());
                var mappingName1 = sfDataGrid1.Columns["Status"].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                var cellvalue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());

                if (cellVaue.ToString() != "")
                {
                    //if (cellvalue1.ToString() == "Created" || cellvalue1.ToString() == "Reviewed" || cellvalue1.ToString() == "Approved" || cellvalue1.ToString() == "Process Started")
                    //{
                    var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Purchase Order" && m.Role_ID == logIn.UserRoleID select new { m.View_Role }).Distinct().ToList();
                    if (uRole.Count > 0)
                    {
                        if (uRole[0].View_Role == true)
                        {

                            SO_No = cellVaue;

                            SO_Amend_No = currentAmendValue.ToString();
                            var = "2";
                            editMode = true;
                            MaterialManagement.PurchaseOrder frm = new PurchaseOrder();
                            //OrderManagement.Transactions.
                            frm.MdiParent = this.MdiParent;
                            frm.Show();
                            //FrmInv.ShowDialog();
                            //i1 = 0;
                            BindOrderslist();
                        }
                        else
                        {
                            MessageBox.Show("You Have No Permissions to View The Purchase Orders");
                            return;
                        }
                    }
                    //}

                    else
                    {
                        MessageBox.Show("The Selected PO is Closed, Fourther Mordifications Not Allowed");
                        return;
                        //i1 = 0;
                    }
                }
                else
                {
                    MessageBox.Show("Please Select Any One Record");
                    return;
                    //i1 = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void reviewedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 2; i < sfDataGrid1.RowCount; i++)
            {

                //foreach (var item in sfDataGrid1.SelectedItems)
                //{
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["PO_NO"].MappingName;
                var mappingName1 = sfDataGrid1.Columns["Status"].MappingName;
                var mappingName2 = sfDataGrid1.Columns["Sel"].MappingName;
                //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                var cellVaue3 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
                if (cellVaue3 == "True")
                {
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());

                    string Status = cellVaue1.ToString();
                    string OrdNo = cellVaue.ToString();
                    int al = 2;
                    int y = 0;
                    switch (Status)
                    {
                        case "Created":
                            y = 1;
                            var uRole = (from m in db.view_Trans_Auth_Levels
                                         where m.Company_ID == logIn.company && m.Menu_Item == "Purchase Order"
                                         && m.Role_ID == logIn.UserRoleID && m.Auth_Level == al
                                         select new { m.Auth_Level, m.Auth_Allowed, m.Status_Code }).Distinct().ToList();
                            if (uRole.Count > 0)
                            {

                                if (uRole[0].Auth_Allowed == true)
                                {
                                    var ci = db.Purchase_Order_Masters.Where(w => w.PO_NO == OrdNo && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                                    {

                                        ci.Status = 4;
                                        ci.Modified_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                                       // ci.Reviewed_By = logIn.username + "-" + DateTime.Now;
                                        db.SubmitChanges();

                                    }
                                }
                                else
                                {
                                    MessageBox.Show("You Have No Permissions to Approve The Purchase Order");
                                    return;
                                }

                            }
                            break;
                        case "Reviewed":
                            MessageBox.Show("PO No " + OrdNo + " Is Already Reviewed or Further Processed.. No Work Done");
                            break;
                        case "Approved":
                            y = 3;
                            MessageBox.Show("PO No " + OrdNo + " Is Already Approved or Further Processed.. No Work Done");
                            break;
                        default:
                            MessageBox.Show("PO No " + OrdNo + " Is Already Approved or Further Processed.. No Work Done");
                            break;

                    }
                }

            }
            BindOrderslist();
        }

        private void amendmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                //if (i >= 0)
                //{
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var currentCellValue = (rowData.GetType().GetProperty("PO_NO").GetValue(rowData, null).ToString());
                var currentAmendValue = (rowData.GetType().GetProperty("Po_Amend_No").GetValue(rowData, null).ToString());
                var mappingName = sfDataGrid1.Columns["Status"].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                if (cellVaue.ToString() != "Closed" || cellVaue.ToString() != "Pre Closed")
                {
                    if (cellVaue.ToString() == "Created" || cellVaue.ToString() == "Approved")
                    {
                        var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Purchase Order" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                        if (uRole.Count > 0)
                        {
                            if (uRole[0].Modify_Role == true)
                            {
                                SO_No = currentCellValue.ToString();
                                SO_Amend_No = currentAmendValue.ToString();
                                var = "1";
                                editMode = true;
                                MaterialManagement.PurchaseOrder frm = new PurchaseOrder();
                                //OrderManagement.Transactions.
                                frm.MdiParent = this.MdiParent;
                                frm.Show();

                            }
                            else
                            {
                                MessageBox.Show("You Have No Permissions to Modify The PO");
                                return;
                            }
                        }
                        ////else
                        ////{
                        ////    SO_No = currentCellValue.ToString();
                        ////    var = "1";
                        ////    editMode = true;
                        ////    OrderManagement.Transactions.frmQuotation frm = new frmQuotation();
                        ////    //OrderManagement.Transactions.
                        ////    frm.MdiParent = this.MdiParent;
                        ////    frm.Show();
                        ////    //FrmInv.ShowDialog();
                        ////    //i1 = 0;
                        ////}
                    }
                    else
                    {
                        MessageBox.Show("The PO Cannot Be Modified Either Closed or Order Receiced");
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

        public void BindOrderslist()
        {
            try
            {
                var d = (from data in db.ShowPOList (logIn.company, logIn.fy_Start_Date,logIn.fy_End_Date, logIn.BU_ID) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                
                 (sfDataGrid1.Columns["Sel"] as GridCheckBoxColumn).AllowThreeState = false;
                (sfDataGrid1.Columns["Sel"] as GridCheckBoxColumn).AllowCheckBoxOnHeader = true;

                this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                this.sfDataGrid1.Columns["PO_NO"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["PO_NO"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["PO_NO"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["PO_NO"].FilterRowCondition = FilterRowCondition.Contains;

                this.sfDataGrid1.Columns["Supplier_Name"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["Supplier_Name"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Supplier_Name"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["Supplier_Name"].FilterRowCondition = FilterRowCondition.Contains;

                this.sfDataGrid1.Columns["Status"].FilterRowEditorType = "ComboBox";
                this.sfDataGrid1.Columns["Status"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Status"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["Status"].FilterRowCondition = FilterRowCondition.Equals;

                this.sfDataGrid1.QueryCellStyle += sfDataGrid_QueryCellStyle;
                string cellValue;
                for (int i = 2; i < sfDataGrid1.RowCount; i++)
                {
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["Status"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    if (cellVaue.ToString() == "Approved")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i,4), Color.Green);
                    }
                    if (cellVaue.ToString() == "Process Started")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 4), Color.LightSkyBlue);
                    }
                    if (cellVaue.ToString() == "Closed")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 4), Color.SaddleBrown);
                    }
                    if (cellVaue.ToString() == "Pre-Closed")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 4), Color.Red);
                    }
                    if (cellVaue.ToString() == "Created")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 4), Color.White);
                    }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

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
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            editMode = false;
            var = "3";
            MaterialManagement.PurchaseOrder frm = new MaterialManagement.PurchaseOrder();
           frm.MdiParent = this.MdiParent;
           frm.Show();
            BindOrderslist();
            
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //showPOListTableAdapter.Fill(ioneDataSet.ShowPOList, logIn.company, txtSearch.Text);
            //dgvRecordList.DataSource = showPOListBindingSource;
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
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["PO_NO"].MappingName;
                var currentAmendValue = (rowData.GetType().GetProperty("Po_Amend_No").GetValue(rowData, null).ToString());
                var mappingName1 = sfDataGrid1.Columns["Status"].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                var cellvalue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());

                if (cellVaue.ToString() != "")
                {
                    if (cellvalue1.ToString() != "Closed")
                    {
                        //if (cellvalue1.ToString() == "Created" || cellvalue1.ToString() == "Reviewed" || cellvalue1.ToString() == "Approved" || cellvalue1.ToString() == "Process Started")
                        //{
                            var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Purchase Order" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                            if (uRole.Count > 0)
                            {
                                if (uRole[0].Modify_Role == true)
                                {

                                    SO_No = cellVaue;

                                    SO_Amend_No = currentAmendValue.ToString();
                                    var = "0";
                                    editMode = true;
                                    MaterialManagement.PurchaseOrder frm = new PurchaseOrder();
                                    //OrderManagement.Transactions.
                                    frm.MdiParent = this.MdiParent;
                                    frm.Show();
                                //FrmInv.ShowDialog();
                                //i1 = 0;
                                BindOrderslist();
                                }
                                else
                                {
                                    MessageBox.Show("You Have No Permissions to Modify The Purchase Orders");
                                    return;
                                }
                            }
                        //}
                    }
                    else
                    {
                        MessageBox.Show("The Selected PO is Closed, Fourther Mordifications Not Allowed");
                        return;
                        //i1 = 0;
                    }
                }
                else
                {
                    MessageBox.Show("Please Select Any One Record");
                    return;
                    //i1 = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void SendEmail()
        {
            try
            {
                //string shipid = txtShippingID.Text;
                //string ordno = txtCustPONo.Text;
                //string lrno = txtLRNo.Text;
                //string transporter = txtTransporter.Text;
                //string contentship = txtShipingContent.Text;
                //string invno = txtInvoiceNo.Text;
                //string Mobileno = txtMobileNo.Text;
                

                string filepath = "";
                string path = Path.Combine(Directory.GetCurrentDirectory(), "PurchaseOrder.pdf");
                //string path = Path.Combine(Directory.GetCurrentDirectory(), @"\Purchase_Order.pdf");
                System.IO.FileInfo fi = new System.IO.FileInfo(path);
                Cursor.Current = Cursors.WaitCursor;
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                //     rep = new MaterialManagement.Transactions.PurchaseOrder();
                var gstno = (from c in db.Company_Report_Formats
                             where c.Company_ID == logIn.company
                             select new { c.PO_Format }).ToList();
                if (gstno.Count > 0)
                {
                    if (gstno[0].PO_Format == "PO1")
                    {
                        rep = new MaterialManagement.Transactions.PurchaseOrder();
                    }
                    
                }
                else
                {
                    rep = new MaterialManagement.Transactions.PurchaseOrder();

                }

                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["Rec_ID"].MappingName;
                var mappingName1 = sfDataGrid1.Columns[4].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                SqlCommand cmd = new SqlCommand("sp_Rpt_PurchaseOrder", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@PO_No", cellVaue);
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


                    //rep.SetParameterValue("Invoice_No", SO_No);
                    //rep.SetParameterValue("Creation_Company", logIn.company);
                    ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();

                    string shippTo = "";
                    string CAddr = "";
                    string CCity = "";
                    string cState = "";
                    String cGSTIN = "";

                    var da1 = (from so in db.Purchase_Order_Masters

                               join c in db.Costing_Units on so.Ship_To equals c.id
                               where so.Id == Convert.ToInt32(cellVaue) && so.BU_ID == logIn.BU_ID && so.Status != 24
                               select new
                               {
                                   c.ToPrintName,
                                   so.Shipping_Address,
                                   c.Address,
                                   c.City,
                                   c.State,
                                   c.State_Code,
                                   c.PinCode,
                                   c.GST_No

                               }).ToList();


                    if (da1.Count > 0)
                    {

                        //                ValidateJSON(ca[0].ConsigneeAddress);
                        if (da1[0].Shipping_Address == null)
                        {
                            shippTo = da1[0].ToPrintName;
                            CAddr = da1[0].Address;
                            CCity = da1[0].City + "-" + da1[0].PinCode;
                            cState = da1[0].State + ", State Code: " + da1[0].State_Code;
                            cGSTIN = da1[0].GST_No;

                        }
                        else
                        {
                            JObject jsoncancel = JObject.Parse(da1[0].Shipping_Address);
                            shippTo = (string)jsoncancel.SelectToken("ShipTo");
                            CAddr = (string)jsoncancel.SelectToken("Address");
                            CCity = (string)jsoncancel.SelectToken("City") + "," + (string)jsoncancel.SelectToken("PinCode");
                            cState = (string)jsoncancel.SelectToken("State") + ", State Code : " + (string)jsoncancel.SelectToken("StateCode");
                            cGSTIN = "GSTIN : " + (string)jsoncancel.SelectToken("GSTIN");

                        }

                        //JToken.Parse(ca[0].ConsigneeAddress);

                    }


                    //rep.SetParameterValue("Creation_Company", logIn.company);
                    // cmd1.Parameters.AddWithValue("@Con_Address1", "Door No");
                    rep.SetParameterValue("shipto", shippTo);
                    rep.SetParameterValue("ShipAddress", CAddr);
                    rep.SetParameterValue("ShipCity", CCity);
                    rep.SetParameterValue("shipState", cState);
                    rep.SetParameterValue("shipGSTIN", cGSTIN);



                    viewer.crystalReportViewer1.ReportSource = rep;
                    viewer.crystalReportViewer1.Refresh();
                    rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                    cmd.Parameters.Clear();
                    
                }
                con.Close();
                filepath = path;


                var Email = (from c in db.Purchase_Order_Masters
                             join inv in db.Supplier_informations
                             on c.SupplierName equals inv.ID
                             join em in db.EMailServerSettings on c.Company_ID equals em.company_ID
                             where c.Company_ID == logIn.company && c.Id == Convert.ToInt32(cellVaue)
                             select new { inv.Email_Id,em.SmtpServer,em.SmptPort,em.POMailID,em.POMailPW,em.Default_CC_Mail_id , c.PO_NO}).ToList();
                //string email = Email[0].Email_Id + ","+ Email[0].Default_CC_Mail_id;
                string email="";
                //Get User Mail ID
                var Umail = (from c in db.User_Setups                           
                            
                             where c.Company_ID == logIn.company && c.User_ID == logIn.userID
                             select new { c.Email}).ToList();
                if (Umail.Count > 0)
                {
                    if (Umail[0].Email != "")
                    {
                        email = Email[0].Email_Id + "," + Umail[0].Email;
                    }
                    else
                    {
                        email = Email[0].Email_Id;
                    }
                }
                else
                {
                    email = Email[0].Email_Id;
                }
                if(email == "" || email is null)
                {
                    MessageBox.Show("E Mail ID of Supplier Is Not Avaiable, No work done");
                    return;
                }
                // string email = Email[0].Cust_Eail;
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(Email[0].POMailID);
                mm.To.Add(email);
                mm.CC.Add(Email[0].Default_CC_Mail_id); 
                mm.Subject = "Purchase Order No :" + Email[0].PO_NO;
                mm.Body = "Dear Sir," + "\n" + "Above referred purchase order attached here with. Please acknowledge the receipt and arrange supply at the earliest" + "\n" + "The Purchase Order has been sent to email " + "\n" + email;
                // mm.Attachments.Add(new Attachment(new MemoryStream(bytes), "Purchase_Vocher" + DateTime.Now.ToString("dd_MM_yyyy_HHmmss") + ".pdf"));
                mm.Attachments.Add(new Attachment(filepath));
                string nme;
              
               
                //}
                mm.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = Email[0].SmtpServer;
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential();
                NetworkCred.UserName = Email[0].POMailID;
                NetworkCred.Password = Email[0].POMailPW;
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port =Convert.ToInt32(Email[0].SmptPort);
                smtp.Send(mm);
                var ci = db.Purchase_Order_Masters.Where(w => w.Id == Convert.ToInt32(cellVaue) && w.Company_ID == logIn.company).FirstOrDefault();
                {
                    ci.email_Sent = true;                    
                    db.SubmitChanges();                  
                }
                MessageBox.Show("PO Sent by Email Successfully");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
    }
}
