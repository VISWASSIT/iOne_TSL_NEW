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
    public partial class MaterialIndentList : Form
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
                var mappingName = sfDataGrid1.Columns[0].MappingName;
                var mappingName1 = sfDataGrid1.Columns[6].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                var cellvalue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                if (cellVaue.ToString() != "")
                {
                    if (cellvalue1.ToString() != "Closed" || cellvalue1.ToString() != "Pre Closed")
                    {
                        if (cellvalue1.ToString() == "Created" || cellvalue1.ToString() == "Approved" || cellvalue1.ToString() == "Process Started")
                        {
                            var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Material Indent" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                            if (uRole.Count > 0)
                            {
                                if (uRole[0].Modify_Role == true)
                                {
                                    SO_No = cellVaue;
                                    var = "0";
                                    editMode = true;
                                    if (logIn.company == 20 || logIn.company == 1043)
                                    {
                                        MaterialManagement.Transactions.MaterialIndent frm = new MaterialIndent();
                                        frm.MdiParent = this.MdiParent;
                                        frm.Show();
                                    }
                                    else
                                    {
                                        MaterialManagement.Transactions.MaterialIndent_Others frm = new MaterialIndent_Others();
                                        frm.MdiParent = this.MdiParent;
                                        frm.Show();
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
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Indent.pdf");
                //string path = Path.Combine(Directory.GetCurrentDirectory(), @"\Purchase_Order.pdf");
                System.IO.FileInfo fi = new System.IO.FileInfo(path);
                Cursor.Current = Cursors.WaitCursor;
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                rep = new MaterialManagement.Transactions.rptMaterialIndent();


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
                var mappingName = sfDataGrid1.Columns["Indent_no"].MappingName;
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
            //for (int i = 2; i < sfDataGrid1.RowCount; i++)
            //{

            //foreach (var item in sfDataGrid1.SelectedItems)
            //{
            int i = sfDataGrid1.CurrentCell.RowIndex;
            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
            var mappingName = sfDataGrid1.Columns["Indent_no"].MappingName;
            var mappingName1 = sfDataGrid1.Columns["status"].MappingName;
            //var mappingName2 = sfDataGrid1.Columns["Sel"].MappingName;
            //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
            //var cellVaue3 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
            //if (cellVaue3 == "True")
            //{
            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());

            string Status = cellVaue1.ToString();
            string OrdNo = cellVaue.ToString();
            if (Status == "Created")
            {

                var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Material Indent" && m.Role_ID == logIn.UserRoleID select new { m.Approve_Role }).Distinct().ToList();
                if (uRole.Count > 0)
                {
                    if (uRole[0].Approve_Role == true)
                    {


                        var ci = db.Material_Indent_Masters.Where(w => w.Indent_NO == OrdNo && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                        {

                            ci.Status = 6;
                            db.SubmitChanges();

                        }
                        var ca = db.Material_Indent_Masters.Where(w => w.Indent_NO == OrdNo && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                        {

                            ci.Modified_By = logIn.username + "-" + DateTime.Now;
                            db.SubmitChanges();

                        }
                    }
                    else
                    {
                        MessageBox.Show("You Have No Permissions to Approve The Indents");
                        return;
                    }
                }

            }
            else
            {
                MessageBox.Show("Indent No " + OrdNo + " Is Already Approved or Further Processed.. No Work Done");

            }
            //}
            // }
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
            workBook.Worksheets[0].Range["A2"].Value = "Material Indent List";
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\Indent_List.xlsx");
            string doc = Fname + "\\Indent_List.xlsx";
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
                    var mappingName = sfDataGrid1.Columns["Indent_no"].MappingName;
                    var mappingName1 = sfDataGrid1.Columns["status"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());

                    if (cellVaue.ToString() != "")
                    {
                        string custId = cellVaue;
                        if (cellVaue1 == "Created")
                        {

                            var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Material Indent" && m.Role_ID == logIn.UserRoleID select new { m.Delete_Role }).Distinct().ToList();
                            if (uRole.Count > 0)
                            {
                                if (uRole[0].Delete_Role == true)
                                {
                                    //var ci = db.Purchase_Order_Masters.Where(w => w.PO_NO == custId && w.Company_ID == logIn.company).FirstOrDefault();
                                    //{
                                    //    ci.Status = 24;
                                    //    ci.Modified_By = logIn.username + "-" + DateTime.Now;
                                    //    db.SubmitChanges();
                                        db.sp_Indent_Delete (custId, logIn.company, logIn.BU_ID);
                                        MessageBox.Show("Indent Request Deleted Sucessfully");
                                        BindOrderslist();
                                    //}
                                }
                                else
                                {
                                    MessageBox.Show("You Have No Permissions to Delete The Indents");
                                    return;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("This Indent is Already Approved or Process Started, Cannot Be Deleted");
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

        private void accountPostingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void viewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns[0].MappingName;
                var mappingName1 = sfDataGrid1.Columns[6].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                var cellvalue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                if (cellVaue.ToString() != "")
                {
                    //if (cellvalue1.ToString() != "Closed" || cellvalue1.ToString() != "Pre Closed")
                    //{
                    //    if (cellvalue1.ToString() == "Created" || cellvalue1.ToString() == "Approved" || cellvalue1.ToString() == "Process Started")
                    //    {
                            var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Material Indent" && m.Role_ID == logIn.UserRoleID select new { m.View_Role }).Distinct().ToList();
                            if (uRole.Count > 0)
                            {
                                if (uRole[0].View_Role == true)
                                {
                                    SO_No = cellVaue;
                                    var = "1";
                                    editMode = true;
                                    if (logIn.company == 20 || logIn.company == 1043)
                            {
                                        MaterialManagement.Transactions.MaterialIndent frm = new MaterialIndent();
                                        frm.MdiParent = this.MdiParent;
                                        frm.Show();
                                    }
                                    else
                                    {
                                        MaterialManagement.Transactions.MaterialIndent_Others frm = new MaterialIndent_Others();
                                        frm.MdiParent = this.MdiParent;
                                        frm.Show();
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("You Have No Permissions to Modify The Purchase Requisitions");
                                    return;
                                }
                        //    }

                        //}
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

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        public MaterialIndentList()
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
                var bindLoc = (from m in db.User_Roles
                               where m.Company_ID == logIn.company && m.Role_ID == logIn.UserRoleID && m.Form_Name== "Material Indent"
                               select new
                               {
                                   m.Approve_Role,
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
                this.approveToolStripMenuItem.Enabled = false;
                if (bindLoc[0].Approve_Role == true)
                {
                    this.approveToolStripMenuItem.Enabled = true;
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
                BindOrderslist();
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
            if (logIn.company == 20 || logIn.company == 1043)
            {
                var = "3";
                MaterialManagement.Transactions.MaterialIndent frm = new MaterialIndent();
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            else
            {
                MaterialManagement.Transactions.MaterialIndent_Others frm = new MaterialIndent_Others();
                frm.MdiParent = this.MdiParent;
                frm.Show();
            }
            BindOrderslist();
        }
        public void BindOrderslist()
        {
            try
            {
                var d = (from data in db.ShowIndentList(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, logIn.BU_ID) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                    //(sfDataGrid1.Columns["Sel"] as GridCheckBoxColumn).AllowThreeState = false;
                    //(sfDataGrid1.Columns["Sel"] as GridCheckBoxColumn).AllowCheckBoxOnHeader = true;
               
                this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                this.sfDataGrid1.Columns["Indent_no"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["Indent_no"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Indent_no"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["Indent_no"].FilterRowCondition = FilterRowCondition.Contains;

                this.sfDataGrid1.Columns["Doc_Ref"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["Doc_Ref"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Doc_Ref"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["Doc_Ref"].FilterRowCondition = FilterRowCondition.Contains;

                this.sfDataGrid1.Columns["status"].FilterRowEditorType = "ComboBox";
                this.sfDataGrid1.Columns["status"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["status"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["status"].FilterRowCondition = FilterRowCondition.Equals;
                //  this.sfDataGrid1.FilterRowPosition = RowPosition.Top;

                this.sfDataGrid1.QueryCellStyle += sfDataGrid_QueryCellStyle;
                string cellValue;
                for (int i = 2; i < sfDataGrid1.RowCount; i++)
                {
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["status"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    if (cellVaue.ToString() == "Approved")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i,7), Color.Green);
                    }
                    if (cellVaue.ToString() == "Process Started")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 7), Color.LightSkyBlue);
                    }
                    if (cellVaue.ToString() == "Closed")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 7), Color.SaddleBrown);
                    }
                    if (cellVaue.ToString() == "Pre-Closed")
                    {
                        SetCellBackgroundColor(new RowColumnIndex(i, 7), Color.Red);
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
