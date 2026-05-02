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
using Syncfusion.WinForms.DataGrid.Events;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGridConverter;
using Ione_DAL;
namespace ioneNet.MaterialManagement.Transactions
{
    public partial class PurchaseRequistionsList : Form
    {
        public static string InvoiceNoList, Order_NoList, SO_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;
        DataClasses1DataContext db = new DataClasses1DataContext();
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();
        public static Boolean editMode;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
              

        private TableLogOnInfo crTableLogOnInfo;

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns[1].MappingName;
                var mappingName1 = sfDataGrid1.Columns["Status"].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                var cellvalue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                if (cellVaue.ToString() != "")
                {

                    var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Purchase Requisition" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                    if (uRole.Count > 0)
                    {
                        if (uRole[0].Modify_Role == true)
                        {
                            switch (cellvalue1)
                            {
                                case "Created":
                                    SO_No = cellVaue;
                                    var = "0";
                                    editMode = true;
                                    MaterialManagement.Transactions.PurchaseRequisition frm = new PurchaseRequisition();
                                    //OrderManagement.Transactions.
                                    frm.MdiParent = this.MdiParent;
                                    frm.Show();
                                    break;
                                case "Reviewed":
                                    int al = 2;
                                    var uRole1 = (from m in db.view_Trans_Auth_Levels
                                                  where m.Company_ID == logIn.company && m.Menu_Item == "Purchase Requisition"
                                                  && m.Role_ID == logIn.UserRoleID && m.Auth_Level == al
                                                  select new { m.Auth_Level, m.Auth_Allowed, m.Status_Code }).Distinct().ToList();
                                    if (uRole1.Count > 0)
                                    {
                                        SO_No = cellVaue;
                                        var = "0";
                                        editMode = true;
                                        MaterialManagement.Transactions.PurchaseRequisition frm1 = new PurchaseRequisition();
                                        //OrderManagement.Transactions.
                                        frm1.MdiParent = this.MdiParent;
                                        frm1.Show();
                                    }
                                    else
                                    {
                                        MessageBox.Show("You Cannot Modify The PR as the PR is already Reviewed");
                                    }
                                    break;
                                case "Approved":
                                    int al1 = 3;
                                    var uRole2 = (from m in db.view_Trans_Auth_Levels
                                                  where m.Company_ID == logIn.company && m.Menu_Item == "Purchase Requisition"
                                                  && m.Role_ID == logIn.UserRoleID && m.Auth_Level == al1
                                                  select new { m.Auth_Level, m.Auth_Allowed, m.Status_Code }).Distinct().ToList();
                                    if (uRole2.Count > 0)
                                    {
                                        SO_No = cellVaue;
                                        var = "0";
                                        editMode = true;
                                        MaterialManagement.Transactions.PurchaseRequisition frm2 = new PurchaseRequisition();
                                        //OrderManagement.Transactions.
                                        frm2.MdiParent = this.MdiParent;
                                        frm2.Show();
                                    }
                                    else
                                    {
                                        MessageBox.Show("You Cannot Modify The PR as the PR is already Approved");
                                    }
                                    break;
                                case "Pre-Closed":
                                    int al2 = 3;
                                    var uRole3 = (from m in db.view_Trans_Auth_Levels
                                                  where m.Company_ID == logIn.company && m.Menu_Item == "Purchase Requisition"
                                                  && m.Role_ID == logIn.UserRoleID && m.Auth_Level == al2
                                                  select new { m.Auth_Level, m.Auth_Allowed, m.Status_Code }).Distinct().ToList();
                                    if (uRole3.Count > 0)
                                    {
                                        SO_No = cellVaue;
                                        var = "0";
                                        editMode = true;
                                        MaterialManagement.Transactions.PurchaseRequisition frm2 = new PurchaseRequisition();
                                        //OrderManagement.Transactions.
                                        frm2.MdiParent = this.MdiParent;
                                        frm2.Show();
                                    }
                                    else
                                    {
                                        MessageBox.Show("You Cannot Modify The PR as the PR is already Approved");
                                    }
                                    break;
                                default:
                                    MessageBox.Show("You Cannot Modify The PR as the PR is closed or Process Started");
                                    
                                 break;
                            }
                        }
                        else
                        {
                            MessageBox.Show("You Have No Permissions to Modify The Purchase Requisitions");
                            return;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sfButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void sfButton3_Click(object sender, EventArgs e)
        {
            BindOrderslist();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //crys viewer1 = new CrstalReportViewer1();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "PurchaseReq.pdf");
                //string path = Path.Combine(Directory.GetCurrentDirectory(), @"\Purchase_Order.pdf");
                System.IO.FileInfo fi = new System.IO.FileInfo(path);
                Cursor.Current = Cursors.WaitCursor;
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                rep = new MaterialManagement.Transactions.rptPurchaseRequisition();


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
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["PR_No"].MappingName;
                var mappingName1 = sfDataGrid1.Columns[4].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                //int i = dgvRecordList.CurrentRow.Index;
                //string a = dgvRecordList.Rows[i].Cells["PoNoDateDataGridViewTextBoxColumn"].Value.ToString();
                //rep.RecordSelectionFormula = "{ Purchase_Req_Master.PR_NO} = " + cellVaue + " and { Purchase_Req_Master.Company_ID} =" + logIn.company;

                //string b = AppCode.GlobalAccess.companyName;
                // rep.RecordSelectionFormula = "{Purchase_Order_Master.PO_NO} = " + a + "  and {Purchase_Order_Master.Company_ID} = " + logIn.company;
                rep.SetParameterValue("prno", cellVaue);
                rep.SetParameterValue("compname", logIn.BU_ID);

                ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                viewer.crystalReportViewer1.ReportSource = rep;
                viewer.crystalReportViewer1.Refresh();
                rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);
                Process.Start(path);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private ConnectionInfo crConnectionInfo = new ConnectionInfo();

        private void approveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 2; i < sfDataGrid1.RowCount; i++)
            {

                //foreach (var item in sfDataGrid1.SelectedItems)
                //{
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["PR_No"].MappingName;
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
                                         where m.Company_ID == logIn.company && m.Menu_Item == "Purchase Requisition"
                                         && m.Auth_Level == y + 1
                                         select new { m.Auth_Level, m.Auth_Allowed, m.Status_Code }).Distinct().ToList();
                            if (rRole.Count > 0)
                            {
                                y = 1;
                                var uRole = (from m in db.view_Trans_Auth_Levels
                                             where m.Company_ID == logIn.company && m.Menu_Item == "Purchase Requisition"
                                             && m.Role_ID == logIn.UserRoleID && m.Auth_Level == y + 1
                                             select new { m.Auth_Level, m.Auth_Allowed, m.Status_Code }).Distinct().ToList();
                                if (uRole.Count > 0)
                                {

                                    MessageBox.Show("Selected PR cannot be Approved as it is not yet Reviewed");
                                    return;

                                }
                            }
                            else
                            {
                                var aRole1 = (from m in db.view_Trans_Auth_Levels
                                              where m.Company_ID == logIn.company && m.Menu_Item == "Purchase Requisition"
                                              && m.Role_ID == logIn.UserRoleID && m.Auth_Level == al
                                              select new { m.Auth_Level, m.Auth_Allowed, m.Status_Code }).Distinct().ToList();
                                if (aRole1.Count > 0)
                                {
                                    if (aRole1[0].Auth_Allowed == true)
                                    {
                                        var ci = db.Purchase_Req_Masters.Where(w => w.PR_NO == OrdNo && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                                        {

                                            ci.Status = 6;
                                            //ci.Modified_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                                            ci.Approved_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                                            db.SubmitChanges();

                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("You Have No Permissions to Approve The Purchase Requisitions");
                                        return;
                                    }
                                }
                            }
                            break;
                        case "Reviewed":
                            y = 2;
                            var uRole1 = (from m in db.view_Trans_Auth_Levels
                                          where m.Company_ID == logIn.company && m.Menu_Item == "Purchase Requisition"
                                          && m.Role_ID == logIn.UserRoleID && m.Auth_Level == al
                                          select new { m.Auth_Level, m.Auth_Allowed, m.Status_Code }).Distinct().ToList();
                            if (uRole1.Count > 0)
                            {
                                if (uRole1[0].Auth_Allowed == true)
                                {
                                    var ci = db.Purchase_Req_Masters.Where(w => w.PR_NO == OrdNo && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                                    {

                                        ci.Status = 6;
                                        //ci.Modified_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                                        ci.Approved_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                                        db.SubmitChanges();

                                    }
                                }
                                else
                                {
                                    MessageBox.Show("You Have No Permissions to Approve The Purchase Requisitions");
                                    return;
                                }
                            }
                            break;
                        case "Approved":
                            y = 3;
                            MessageBox.Show("PR No " + OrdNo + " Is Already Approved or Further Processed.. No Work Done");
                            break;
                        default:
                            MessageBox.Show("PR No " + OrdNo + " Is Already Approved or Further Processed.. No Work Done");
                            break;

                    }
                }

            }
            BindOrderslist();
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
            workBook.Worksheets[0].Range["A2"].Value = "Purchase Req List";
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\PR_List.xlsx");
            string doc = Fname + "\\PR_List.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //  db.sp_PurchaseReq_Delete(myString, logIn.company, logIn.BU_ID);
            try
            {
                DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    int i = sfDataGrid1.CurrentCell.RowIndex;
                    var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["PR_No"].MappingName;
                    var mappingName1 = sfDataGrid1.Columns["Status"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());

                    if (cellVaue.ToString() != "")
                    {
                        string custId = cellVaue;
                        if (cellVaue1 == "Created")
                        {

                            var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Purchase Requisition" && m.Role_ID == logIn.UserRoleID select new { m.Delete_Role }).Distinct().ToList();
                            if (uRole.Count > 0)
                            {
                                if (uRole[0].Delete_Role == true)
                                {
                                    //var ci = db.Purchase_Order_Masters.Where(w => w.PO_NO == custId && w.Company_ID == logIn.company).FirstOrDefault();
                                    //{
                                    //    ci.Status = 24;
                                    //    ci.Modified_By = logIn.username + "-" + DateTime.Now;
                                    //    db.SubmitChanges();
                                        db.sp_PurchaseReq_Delete(custId, logIn.company, logIn.BU_ID);
                                        MessageBox.Show("Purchase Request Deleted Sucessfully");
                                        BindOrderslist();
                                    //}
                                }
                                else
                                {
                                    MessageBox.Show("You Have No Permissions to Delete The Purchase Requisitions");
                                    return;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("This PR is Already Approved or Process Started, Cannot Be Deleted");
                            return;
                        }

                    }
                    else
                    {
                        MessageBox.Show("Please Select Any One Purchase Req");
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
            Transactions.PurchaseRequisition frm = new Transactions.PurchaseRequisition();
            
            var = "1";
            frm.ShowDialog();
           
        }

        private void eMailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //crys viewer1 = new CrstalReportViewer1();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "PurchaseReq.pdf");
                //string path = Path.Combine(Directory.GetCurrentDirectory(), @"\Purchase_Order.pdf");
                System.IO.FileInfo fi = new System.IO.FileInfo(path);
                Cursor.Current = Cursors.WaitCursor;
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                rep = new MaterialManagement.Transactions.rptPurchaseRequisition();


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
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["PR_No"].MappingName;
                var mappingName1 = sfDataGrid1.Columns[4].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                //int i = dgvRecordList.CurrentRow.Index;
                //string a = dgvRecordList.Rows[i].Cells["PoNoDateDataGridViewTextBoxColumn"].Value.ToString();
                //rep.RecordSelectionFormula = "{ Purchase_Req_Master.PR_NO} = " + cellVaue + " and { Purchase_Req_Master.Company_ID} =" + logIn.company;

                //string b = AppCode.GlobalAccess.companyName;
                // rep.RecordSelectionFormula = "{Purchase_Order_Master.PO_NO} = " + a + "  and {Purchase_Order_Master.Company_ID} = " + logIn.company;
                rep.SetParameterValue("prno", cellVaue);
                rep.SetParameterValue("compname", logIn.BU_ID);

                ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                viewer.crystalReportViewer1.ReportSource = rep;
                viewer.crystalReportViewer1.Refresh();
                rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);
                Process.Start(path);
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
                        var mappingName = sfDataGrid1.Columns["PR_No"].MappingName;
                        if (rowData == item)
                        {
                            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                            var ci = db.Purchase_Req_Masters.Where(w => w.PR_NO == cellVaue && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                            {
                                ci.Status = 26;
                                ci.Modified_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                                db.SubmitChanges();
                            }
                        }
                    }
                }
                MessageBox.Show("Selected Requisitions(s) Are Pre-Closed Successfully");
                BindOrderslist();
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
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns[1].MappingName;
                var mappingName1 = sfDataGrid1.Columns["Status"].MappingName; ;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                var cellvalue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                if (cellVaue.ToString() != "")
                {

                    var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Purchase Requisition" && m.Role_ID == logIn.UserRoleID select new { m.View_Role }).Distinct().ToList();
                    if (uRole.Count > 0)
                    {
                        if (uRole[0].View_Role == true)
                        {
                            SO_No = cellVaue;
                            var = "1";
                            editMode = true;
                            MaterialManagement.Transactions.PurchaseRequisition frm = new PurchaseRequisition();
                            //OrderManagement.Transactions.
                            frm.MdiParent = this.MdiParent;
                            frm.Show();
                        }
                        else
                        {
                            MessageBox.Show("You Have No Permissions to View The Purchase Requisitions");
                            return;
                        }
                    }


                }
                else
                {
                    MessageBox.Show("Please Select Any One Record");
                    //i1 = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void contextMenuStrip1_Click(object sender, EventArgs e)
        {
           
           
        }

        private void sfDataGrid1_MouseUp(object sender, MouseEventArgs e)
        {
           
        }

        private void reviewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 2; i < sfDataGrid1.RowCount; i++)
            {

                //foreach (var item in sfDataGrid1.SelectedItems)
                //{
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["PR_No"].MappingName;
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
                                         where m.Company_ID == logIn.company && m.Menu_Item == "Purchase Requisition"
                                         && m.Role_ID == logIn.UserRoleID && m.Auth_Level == al
                                         select new { m.Auth_Level, m.Auth_Allowed, m.Status_Code }).Distinct().ToList();
                            if (uRole.Count > 0)
                            {

                                if (uRole[0].Auth_Allowed == true)
                                {
                                    var ci = db.Purchase_Req_Masters.Where(w => w.PR_NO == OrdNo && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                                    {

                                        ci.Status = 4;
                                        //ci.Modified_By = logIn.username + "-" + DateTime.Now;
                                        ci.Reviewed_By = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
                                        db.SubmitChanges();

                                    }
                                }
                                else
                                {
                                    MessageBox.Show("You Have No Permissions to Approve The Purchase Requisitions");
                                    return;
                                }

                            }
                            break;
                        case "Reviewed":
                            MessageBox.Show("PR No " + OrdNo + " Is Already Reviewed or Further Processed.. No Work Done");
                            break;
                        case "Approved":
                            y = 3;
                            MessageBox.Show("PR No " + OrdNo + " Is Already Approved or Further Processed.. No Work Done");
                            break;
                        default:
                            MessageBox.Show("PR No " + OrdNo + " Is Already Approved or Further Processed.. No Work Done");
                            break;

                    }
                }

            }
            BindOrderslist();
        }

        public PurchaseRequistionsList()
        {
            InitializeComponent();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void ListofPurchaseRequistions_Load(object sender, EventArgs e)
        {
            try
            {
                //this.Tag = null;

                BindOrderslist();
                var bindLoc = (from m in db.User_Roles
                               where m.Company_ID == logIn.company && m.Role_ID == logIn.UserRoleID && m.Form_Name == "Purchase Requisition"
                               select new
                               {
                                   m.Review_Role,
                                   m.Approve_Role,
                                   m.View_Role,
                                   m.Modify_Role,
                                   m.Create_Role,
                                   m.Delete_Role
                               }).ToList();
                this.reviewToolStripMenuItem.Enabled = false;
                if (bindLoc[0].Review_Role == true)
                {
                    this.reviewToolStripMenuItem.Enabled = true;
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
                this.viewToolStripMenuItem.Enabled = false;
                if (bindLoc[0].View_Role == true)
                {
                    this.viewToolStripMenuItem.Enabled = true;
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
            catch (Exception ex)
            {
                //showPOListTableAdapter.Fill(ioneDataSet.ShowPOList, logIn.company, " ");
                //dgvRecordList.DataSource = showPOListBindingSource;
            }
        }
        private void sfButton1_Click(object sender, EventArgs e)
        {
            editMode = false;
            var = "2";
            MaterialManagement.Transactions.PurchaseRequisition frm = new PurchaseRequisition();
            frm.MdiParent = this.MdiParent;
            frm.Show();
            BindOrderslist();
        }
        public void BindOrderslist()
        {
            try
            {
                if (logIn.company == 1043)
                {
                    var d = (from data in db.ShowPRList_NHVS(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, logIn.BU_ID) select data).ToList();

                    if (d.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        sfDataGrid1.DataSource = d;
                        (sfDataGrid1.Columns["Sel"] as GridCheckBoxColumn).AllowThreeState = false;
                        (sfDataGrid1.Columns["Sel"] as GridCheckBoxColumn).AllowCheckBoxOnHeader = true;

                        this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                        this.sfDataGrid1.Columns["PR_No"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["PR_No"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["PR_No"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["PR_No"].FilterRowCondition = FilterRowCondition.Contains;

                        this.sfDataGrid1.Columns["Doc_Ref"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["Doc_Ref"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["Doc_Ref"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["Doc_Ref"].FilterRowCondition = FilterRowCondition.Contains;

                        this.sfDataGrid1.Columns["Status"].FilterRowEditorType = "ComboBox";
                        this.sfDataGrid1.Columns["Status"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["Status"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["Status"].FilterRowCondition = FilterRowCondition.Equals;

                        this.sfDataGrid1.Columns["TC_No"].FilterRowEditorType = "ComboBox";
                        this.sfDataGrid1.Columns["TC_No"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["TC_No"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["TC_No"].FilterRowCondition = FilterRowCondition.Contains;

                        this.sfDataGrid1.Columns["PR_Type"].FilterRowEditorType = "ComboBox";
                        this.sfDataGrid1.Columns["PR_Type"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["PR_Type"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["PR_Type"].FilterRowCondition = FilterRowCondition.Contains;
                        //  this.sfDataGrid1.FilterRowPosition = RowPosition.Top;

                        this.sfDataGrid1.QueryCellStyle += sfDataGrid_QueryCellStyle;
                        string cellValue;
                        for (int i = 2; i < sfDataGrid1.RowCount; i++)
                        {
                            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                            var mappingName = sfDataGrid1.Columns["Status"].MappingName;
                            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                            if (cellVaue.ToString() == "Approved")
                            {
                                SetCellBackgroundColor(new RowColumnIndex(i, 7), Color.Green);
                            }
                            else
                            if (cellVaue.ToString() == "Process Started")
                            {
                                SetCellBackgroundColor(new RowColumnIndex(i, 7), Color.LightSkyBlue);
                            }
                            else
                            if (cellVaue.ToString() == "Closed")
                            {
                                SetCellBackgroundColor(new RowColumnIndex(i, 7), Color.SaddleBrown);
                            }
                            else
                            if (cellVaue.ToString() == "Pre-Closed")
                            {
                                SetCellBackgroundColor(new RowColumnIndex(i, 7), Color.Red);
                            }
                            else
                            if (cellVaue.ToString() == "Created")
                            {
                                SetCellBackgroundColor(new RowColumnIndex(i, 7), Color.White);
                            }
                        }
                    }
                }
                else
                {
                    var d = (from data in db.ShowPRList(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, logIn.BU_ID) select data).ToList();

                    if (d.Count > 0)
                    {
                        //dgProductsList.DataSource = d;
                        sfDataGrid1.DataSource = d;
                        (sfDataGrid1.Columns["Sel"] as GridCheckBoxColumn).AllowThreeState = false;
                        (sfDataGrid1.Columns["Sel"] as GridCheckBoxColumn).AllowCheckBoxOnHeader = true;

                        this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                        this.sfDataGrid1.Columns["PR_No"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["PR_No"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["PR_No"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["PR_No"].FilterRowCondition = FilterRowCondition.Contains;

                        this.sfDataGrid1.Columns["Doc_Ref"].FilterRowEditorType = "TextBox";
                        this.sfDataGrid1.Columns["Doc_Ref"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["Doc_Ref"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["Doc_Ref"].FilterRowCondition = FilterRowCondition.Contains;

                        this.sfDataGrid1.Columns["Status"].FilterRowEditorType = "ComboBox";
                        this.sfDataGrid1.Columns["Status"].ShowFilterRowOptions = false;
                        this.sfDataGrid1.Columns["Status"].ImmediateUpdateColumnFilter = true;
                        this.sfDataGrid1.Columns["Status"].FilterRowCondition = FilterRowCondition.Equals;
                        //  this.sfDataGrid1.FilterRowPosition = RowPosition.Top;

                        this.sfDataGrid1.QueryCellStyle += sfDataGrid_QueryCellStyle;
                        string cellValue;
                        for (int i = 2; i < sfDataGrid1.RowCount; i++)
                        {
                            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                            var mappingName = sfDataGrid1.Columns["Status"].MappingName;
                            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                            if (cellVaue.ToString() == "Approved")
                            {
                                SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.Green);
                            }
                            else
                            if (cellVaue.ToString() == "Process Started")
                            {
                                SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.LightSkyBlue);
                            }
                            else
                            if (cellVaue.ToString() == "Closed")
                            {
                                SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.SaddleBrown);
                            }
                            else
                            if (cellVaue.ToString() == "Pre-Closed")
                            {
                                SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.Red);
                            }
                            else
                            if (cellVaue.ToString() == "Created")
                            {
                                SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.White);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void sfDataGrid1_CellCheckBoxClick(object sender, CellCheckBoxClickEventArgs e)
        {

            if (e.Column.MappingName == "Sel")
            {
                //e.Cancel =true;
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
    }
}
