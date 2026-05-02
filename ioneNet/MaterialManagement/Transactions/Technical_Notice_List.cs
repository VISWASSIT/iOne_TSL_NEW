using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Ione_DAL;
using Syncfusion.Windows.Forms.CellGrid.ScrollAxis;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Events;
using Syncfusion.WinForms.DataGrid.Interactivity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ioneNet.MaterialManagement.Transactions
{
    public partial class Technical_Notice_List : Form
    {
        public static string InvoiceNoList, Order_NoList, SO_No, SO_Amend_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;
        DataClasses1DataContext db = new DataClasses1DataContext();
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();
        public static Boolean editMode;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        private Database crDatabase;

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
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

                    var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Technical notice" && m.Role_ID == logIn.UserRoleID select new { m.View_Role }).Distinct().ToList();
                    if (uRole.Count > 0)
                    {
                        if (uRole[0].View_Role == true)
                        {
                            SO_No = cellVaue;
                            var = "1";
                            editMode = true;
                            MaterialManagement.Transactions.Technical_notice frm = new Technical_notice();
                            //OrderManagement.Transactions.
                            frm.MdiParent = this.MdiParent;
                            frm.Show();
                        }
                        else
                        {
                            MessageBox.Show("You Have No Permissions to View The Technical notice");
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
                    
                        //if (cellvalue1.ToString() == "Created" || cellvalue1.ToString() == "Approved" )
                        //{
                            var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Technical notice" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                            if (uRole.Count > 0)
                            {
                                if (uRole[0].Modify_Role == true)
                                {
                                    SO_No = cellVaue;
                                    var = "0";
                                    editMode = true;
                                    MaterialManagement.Transactions.Technical_notice frm = new Technical_notice();
                                    //OrderManagement.Transactions.
                                    frm.MdiParent = this.MdiParent;
                                    frm.Show();
                                }
                                else
                                {
                                    MessageBox.Show("You Have No Permissions to Modify The Technical notice");
                                    return;
                                }
                            }

                        //}
                    

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

        private void approveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 2; i < sfDataGrid1.RowCount; i++)
            {

                //foreach (var item in sfDataGrid1.SelectedItems)
                //{
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["TC_No"].MappingName;
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
                    if (Status == "Created")
                    {

                        var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Technical notice" && m.Role_ID == logIn.UserRoleID select new { m.Approve_Role }).Distinct().ToList();
                        if (uRole.Count > 0)
                        {
                            if (uRole[0].Approve_Role == true)
                            {


                                var ci = db.technical_notice_masters.Where(w => w.Vch_no == OrdNo && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID && w.is_amended == false).FirstOrDefault();
                                {

                                    ci.Status = 6;
                                    db.SubmitChanges();

                                }
                                var ca = db.technical_notice_masters.Where(w => w.Vch_no == OrdNo && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID && w.is_amended == false).FirstOrDefault();
                                {

                                    ci.Modified_by = logIn.username + "-" + DateTime.Now;
                                    db.SubmitChanges();

                                }
                            }
                            else
                            {
                                MessageBox.Show("You Have No Permissions to Approve The Technical notice");
                                return;
                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("TC No " + OrdNo + " Is Already Approved or Further Processed.. No Work Done");

                    }
                }
                

                //}
            }
            BindOrderslist();

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
                    var mappingName = sfDataGrid1.Columns["TC_No"].MappingName;
                    var mappingName1 = sfDataGrid1.Columns["Status"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());

                    if (cellVaue.ToString() != "")
                    {
                        string custId = cellVaue;
                        if (cellVaue1 == "Created")
                        {

                            var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Technical notice" && m.Role_ID == logIn.UserRoleID  select new { m.Delete_Role }).Distinct().ToList();
                            if (uRole.Count > 0)
                            {
                                if (uRole[0].Delete_Role == true)
                                {
                                    //var ci = db.Purchase_Order_Masters.Where(w => w.PO_NO == custId && w.Company_ID == logIn.company).FirstOrDefault();
                                    //{
                                    //    ci.Status = 24;
                                    //    ci.Modified_By = logIn.username + "-" + DateTime.Now;
                                    //    db.SubmitChanges();
                                    db.sp_TC_Delete(custId, logIn.company, logIn.BU_ID);
                                    
                                    MessageBox.Show("Technical notice Request Deleted Sucessfully");
                                    BindOrderslist();
                                    //}
                                }
                                else
                                {
                                    MessageBox.Show("You Have No Permissions to Delete The Technical notice");
                                    return;
                                }
                            }
                        }
                        else
                        {
                            MessageBox.Show("This Technical notice is Already Approved or Process Started, Cannot Be Deleted");
                            return;
                        }

                    }
                    else
                    {
                        MessageBox.Show("Please Select Any One Technical notice");
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            BindOrderslist();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                //var mappingName2 = sfDataGrid1.Columns["import_po"].MappingName;

                ////var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                //var cellVaue3 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());

                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                //var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["TC_No"].MappingName;
                //var mappingName1 = sfDataGrid1.Columns[4].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                //crys viewer1 = new CrstalReportViewer1();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Techinical notice"+ cellVaue+".pdf");
                //string path = Path.Combine(Directory.GetCurrentDirectory(), @"\Purchase_Order.pdf");
                System.IO.FileInfo fi = new System.IO.FileInfo(path);
                Cursor.Current = Cursors.WaitCursor;
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                rep = new MaterialManagement.Transactions.rptTechnical_Notice();
               
                

                SqlCommand cmd = new SqlCommand("sp_Rpt_TechinicalnoticeReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@vch_No", cellVaue);
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


               
                    ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                   
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
            //try
            //{
            //    //crys viewer1 = new CrstalReportViewer1();
            //    string path = Path.Combine(Directory.GetCurrentDirectory(), "TechnicalNotice.pdf");
            //    //string path = Path.Combine(Directory.GetCurrentDirectory(), @"\Purchase_Order.pdf");
            //    System.IO.FileInfo fi = new System.IO.FileInfo(path);
            //    Cursor.Current = Cursors.WaitCursor;
            //    CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
            //    rep = new MaterialManagement.Transactions.rptTechnical_Notice();


            //    crConnectionInfo.ServerName = frmMain.ServerIP;
            //    crConnectionInfo.DatabaseName = frmMain.Database;
            //    crConnectionInfo.UserID = frmMain.DBUserID;
            //    crConnectionInfo.Password = frmMain.Password;
            //    crDatabase = rep.Database;
            //    crTables = crDatabase.Tables;
            //    //Loop through all tables in the report and apply the connection information for each table.
            //    for (int k = 0; k < crTables.Count; k++)
            //    {
            //        //  crTable = crTables[i];
            //        crTableLogOnInfo = crTables[k].LogOnInfo;
            //        crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
            //        crTables[k].ApplyLogOnInfo(crTableLogOnInfo);

            //    }
            //    int i = sfDataGrid1.CurrentCell.RowIndex;
            //    var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
            //    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
            //    var mappingName = sfDataGrid1.Columns["TC_No"].MappingName;
            //    var mappingName1 = sfDataGrid1.Columns[4].MappingName;
            //    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

            //    //int i = dgvRecordList.CurrentRow.Index;
            //    //string a = dgvRecordList.Rows[i].Cells["PoNoDateDataGridViewTextBoxColumn"].Value.ToString();
            //    //rep.RecordSelectionFormula = "{ Purchase_Req_Master.PR_NO} = " + cellVaue + " and { Purchase_Req_Master.Company_ID} =" + logIn.company;

            //    //string b = AppCode.GlobalAccess.companyName;
            //    // rep.RecordSelectionFormula = "{Purchase_Order_Master.PO_NO} = " + a + "  and {Purchase_Order_Master.Company_ID} = " + logIn.company;
            //    rep.SetParameterValue("vch_No", cellVaue.ToString());
            //    rep.SetParameterValue("Creation_Company", logIn.company);
            //    rep.SetParameterValue("buid", logIn.BU_ID);

            //    ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
            //    // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
            //    viewer.crystalReportViewer1.ReportSource = rep;
            //    viewer.crystalReportViewer1.Refresh();
            //    rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);
            //    Process.Start(path);
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
        }

        private void preCloseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 2; i < sfDataGrid1.RowCount; i++)
                {

                    //foreach (var item in sfDataGrid1.SelectedItems)
                    //{
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns["TC_No"].MappingName;
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


                        var ci = db.technical_notice_masters.Where(w => w.Vch_no == OrdNo && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID && w.is_amended == false).FirstOrDefault();
                        {

                            ci.Status = 26;
                            ci.Modified_by = logIn.username + "-" + DateTime.Now;
                            db.SubmitChanges();

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

        private void amendmentToolStripMenuItem_Click(object sender, EventArgs e)
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

                    //if (cellvalue1.ToString() == "Created" || cellvalue1.ToString() == "Approved" )
                    //{
                    var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Technical notice" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                    if (uRole.Count > 0)
                    {
                        if (uRole[0].Modify_Role == true)
                        {
                            SO_No = cellVaue;
                            var = "3";
                            editMode = true;
                            MaterialManagement.Transactions.Technical_notice frm = new Technical_notice();
                            //OrderManagement.Transactions.
                            frm.MdiParent = this.MdiParent;
                            frm.Show();
                        }
                        else
                        {
                            MessageBox.Show("You Have No Permissions to Amend/Modify The Technical notice");
                            return;
                        }
                    }

                    //}


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

        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();

        public Technical_Notice_List()
        {
            InitializeComponent();
        }

        private void Technical_Notice_List_Load(object sender, EventArgs e)
        {
            try
            {
                BindOrderslist();
                var bindLoc = (from m in db.User_Roles
                               where m.Company_ID == logIn.company && m.Role_ID == logIn.UserRoleID && m.Form_Name == "Technical notice"
                               select new
                               {
                                   m.Approve_Role,
                                   m.View_Role,
                                   m.Modify_Role,
                                   m.Create_Role,
                                   m.Delete_Role
                               }).ToList();
                if (bindLoc.Count > 0)
                {
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

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            editMode = false;
            var = "2";
            MaterialManagement.Transactions.Technical_notice frm = new Technical_notice();
            frm.MdiParent = this.MdiParent;
            frm.Show();
            BindOrderslist();
        }
        public void BindOrderslist()
        {
            try
            {
                var d = (from data in db.ShowTCList(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, logIn.BU_ID) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;

                    (sfDataGrid1.Columns["Sel"] as GridCheckBoxColumn).AllowThreeState = false;
                    (sfDataGrid1.Columns["Sel"] as GridCheckBoxColumn).AllowCheckBoxOnHeader = true;

                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["TC_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["TC_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["TC_No"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["TC_No"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Job_no"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Job_no"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Job_no"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Job_no"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Required_Department"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Required_Department"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Required_Department"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Required_Department"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Project_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Project_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Project_Name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Project_Name"].FilterRowCondition = FilterRowCondition.Contains;

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
                            SetCellBackgroundColor(new RowColumnIndex(i, 12), Color.Green);
                        }
                        if (cellVaue.ToString() == "Process Started")
                        {
                            SetCellBackgroundColor(new RowColumnIndex(i, 12), Color.LightSkyBlue);
                        }
                        if (cellVaue.ToString() == "Closed")
                        {
                            SetCellBackgroundColor(new RowColumnIndex(i, 12), Color.SaddleBrown);
                        }
                        if (cellVaue.ToString() == "Pre-Closed")
                        {
                            SetCellBackgroundColor(new RowColumnIndex(i, 12), Color.Red);
                        }
                        if (cellVaue.ToString() == "Created")
                        {
                            SetCellBackgroundColor(new RowColumnIndex(i, 12), Color.White);
                        }
                    }
                }
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
        void sfDataGrid_QueryCellStyle(object sender, Syncfusion.WinForms.DataGrid.Events.QueryCellStyleEventArgs e)
        {
            var rowColumnIndex = new RowColumnIndex(e.RowIndex, e.ColumnIndex);
            if (colorDict.ContainsKey(rowColumnIndex))
                e.Style.BackColor = colorDict[rowColumnIndex];
        }

    }
}
