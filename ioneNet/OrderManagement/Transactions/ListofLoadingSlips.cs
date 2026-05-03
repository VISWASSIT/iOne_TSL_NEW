using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Syncfusion.WinForms.DataGrid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CrystalDecisions.Shared;
using Ione_DAL;
using System.Configuration;
using System.Data.SqlClient;
using Syncfusion.Windows.Forms.CellGrid.ScrollAxis;
using CrystalDecisions.CrystalReports.Engine;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using Syncfusion.WinForms.DataGrid.Events;
using Syncfusion.Data;
using Syncfusion.WinForms.DataGridConverter;
using System.Diagnostics;
using System.IO;

namespace ioneNet.OrderManagement.Transactions
{
       public partial class ListofLoadingSlips : Form
    {
        public static string InvoiceNoList, Order_NoList, SO_No, SO_Amend_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;
        DataClasses1DataContext db = new DataClasses1DataContext();
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        private Database crDatabase;
        public static Boolean editMode;

        private void btnAddNew_Click(object sender, EventArgs e)
        {
            sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.None;

            editMode = false;
            OrderManagement.Transactions.frmLoadingSlip frm = new frmLoadingSlip();
            frm.MdiParent = this.MdiParent;
            frm.Show();
            sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;

            BindOrderslist();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            BindOrderslist();
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
            workBook.Worksheets[0].Range["A2"].Value = "Loading Slips List";
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\LoadingSLipList.xlsx");
            string doc = Fname + "\\LoadingSLipList.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
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
                var currentCellValue = (rowData.GetType().GetProperty("Loading_Slip_No").GetValue(rowData, null).ToString());
               // var currentAmendValue = (rowData.GetType().GetProperty("Quot_Amend_No").GetValue(rowData, null).ToString());

                var mappingName = sfDataGrid1.Columns["Status"].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                
                    //if (cellVaue.ToString() == "Created")
                    //{
                        var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Quotation" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                        if (uRole.Count > 0)
                        {
                            if (uRole[0].Modify_Role == true)
                            {
                                SO_No = currentCellValue.ToString();
                                //SO_Amend_No = currentAmendValue.ToString();
                                var = "0";
                                editMode = true;
                                sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.None;

                                OrderManagement.Transactions.frmLoadingSlip frm = new frmLoadingSlip();
                                //OrderManagement.Transactions.
                                frm.MdiParent = this.MdiParent;
                                frm.Show();
                                sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
                            }
                            else
                            {
                                MessageBox.Show("You Have No Permissions to Modify The Loading Slips");
                                return;
                            }
                        }
                        else
                        {
                            SO_No = currentCellValue.ToString();
                            //SO_Amend_No = currentAmendValue.ToString();
                            var = "0";
                            editMode = true;
                            sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.None;
                            OrderManagement.Transactions.frmLoadingSlip frm = new frmLoadingSlip();
                            //OrderManagement.Transactions.
                            frm.MdiParent = this.MdiParent;

                            frm.Show();
                            sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;

                            //FrmInv.ShowDialog();
                            //i1 = 0;
                        }
                    //}
                    //else
                    //{
                    //    MessageBox.Show("The Loading Slip Cannot Be Modified as Loading Completed");
                    //    //i1 = 0;
                    //}
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void printSlipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
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


                path = Path.Combine(Directory.GetCurrentDirectory(), "LoadingSlip.pdf");
                //string path = @"D:\Invoice.pdf";
                FileInfo fi1 = new FileInfo(path);
                DataTable Dt = new DataTable();

                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();


                rep = new OrderManagement.Transactions.LoadingSlip();

                SqlCommand cmd = new SqlCommand("sp_Rpt_LoadingSlip", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Slip_No", SO_No);
                cmd.Parameters.AddWithValue("@Creation_Company", logIn.company);
                cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                //DataTable Dt = new DataTable();

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
                    // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                    viewer.crystalReportViewer1.ReportSource = rep;
                    viewer.crystalReportViewer1.Refresh();
                    rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                    //cmd.Parameters.Clear();
                }
                con.Close();


                Process.Start(path);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void updateLoadingDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void cancelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                if (i >= 0)                {
                    
                    var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    var mappingName = sfDataGrid1.Columns[0].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    var deleteproduct = db.Loading_Masters.Single(course => course.Loading_Slip_No == cellVaue);                    
                    deleteproduct.Status = 25;                   
                    db.SubmitChanges();
                    MessageBox.Show("Loading Slip Cancelled Successfully");
                    BindOrderslist();

                }
                else
                {
                    MessageBox.Show("Have to Select Loading Slip");
                }
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
                    int i = sfDataGrid1.CurrentCell.RowIndex;
                    //if (i >= 0)
                    //{
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                    var currentCellValue = (rowData.GetType().GetProperty("Loading_Slip_No").GetValue(rowData, null).ToString());                    

                    var mappingName = sfDataGrid1.Columns["Status"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    
                    if (cellVaue.ToString() != "Closed")
                    {
                        SqlCommand cmd = new SqlCommand();

                        SO_No = currentCellValue.ToString();

                        db.Sp_delete_LoadingSLip(SO_No, logIn.company, logIn.BU_ID);
                        MessageBox.Show("Loading Slip Deleted Successfully");
                        BindOrderslist();
                    }
                    else
                    {
                        MessageBox.Show("Selected Slips Cannot Be Deleted As Already Closed");
                    }                        
                   
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void printGatepassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // string path1 = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.pdf");
                SqlCommand cmd1 = con.CreateCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                int r = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(r);
                var mappingName = sfDataGrid1.Columns[0].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                SO_No = cellVaue.ToString();
                var mappingName1 = sfDataGrid1.Columns["Loading_Slip_No"].MappingName;

                var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                // boolen GPType = cellVaue1;
                //cmd1.CommandText = "UPDATE temp_Inv_Copy SET Inv_No = @InvNo where Company_ID = @compID";
                //cmd1.Parameters.AddWithValue("@InvNo", SO_No);
                //cmd1.Parameters.AddWithValue("@compID", logIn.company);
                //cmd1.ExecuteNonQuery();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "GatePass.pdf");
                //string path = @"D:\Invoice.pdf";
                FileInfo fi1 = new FileInfo(path);
                DataTable Dt = new DataTable();

                if (fi1.Exists)
                {
                    fi1.Delete();
                }
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                rep = new MaterialManagement.Transactions.GatePass_NonStock();

                SqlCommand cmd = new SqlCommand("sp_Rpt_FG_Gatepass", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Slip_No", SO_No);
                cmd.Parameters.AddWithValue("@Creation_Company", logIn.company);
                cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                //DataTable Dt = new DataTable();

                da.SelectCommand = cmd;
                da.Fill(Dt);

                crConnectionInfo.ServerName = frmMain.ServerIP;
                crConnectionInfo.DatabaseName = frmMain.Database;
                crConnectionInfo.UserID = frmMain.DBUserID;
                crConnectionInfo.Password = frmMain.Password;
                crDatabase = rep.Database;
                crTables = crDatabase.Tables;
                //Loop through all tables in the report and apply the connection information for each table.
                for (int i = 0; i < crTables.Count; i++)
                {
                    //  crTable = crTables[i];
                    crTableLogOnInfo = crTables[i].LogOnInfo;
                    crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                    crTables[i].ApplyLogOnInfo(crTableLogOnInfo);
                    //If your DatabaseName is changing at runtime, specify the table location. For example, when you are reporting off of a Northwind database on SQL server you should have the following line of code:

                }
                //string SlipNo = cellVaue1;
                rep.SetParameterValue("@Slip_No", SO_No);
                rep.SetParameterValue("@Creation_Company", logIn.company);
                rep.SetParameterValue("@buid", logIn.BU_ID);
                //rep.RecordSelectionFormula = "{Loading_Master.Loading_Slip_No} = '" + SlipNo + "'";



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

        private void printPackingListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // string path1 = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.pdf");
                SqlCommand cmd1 = con.CreateCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns[0].MappingName;
                var mappingName1 = sfDataGrid1.Columns[7].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                SO_No = cellVaue.ToString();
                string path = Path.Combine(Directory.GetCurrentDirectory(), "PackingList.pdf");
                //string path = @"D:\Invoice.pdf";
                FileInfo fi1 = new FileInfo(path);

                if (fi1.Exists)
                {
                    fi1.Delete();
                }


                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                rep = new OrderManagement.Transactions.DeliveryChallan();

                crConnectionInfo.ServerName = frmMain.ServerIP;
                crConnectionInfo.DatabaseName = frmMain.Database;
                crConnectionInfo.UserID = frmMain.DBUserID;
                crConnectionInfo.Password = frmMain.Password;
                crDatabase = rep.Database;
                crTables = crDatabase.Tables;
                //Loop through all tables in the report and apply the connection information for each table.
                for (int j = 0; j < crTables.Count; j++)
                {
                    //  crTable = crTables[i];
                    crTableLogOnInfo = crTables[j].LogOnInfo;
                    crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                    crTables[j].ApplyLogOnInfo(crTableLogOnInfo);
                    //If your DatabaseName is changing at runtime, specify the table location. For example, when you are reporting off of a Northwind database on SQL server you should have the following line of code:

                }

                //rep.SetParameterValue("Creation_Company", logIn.company);
                ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                // cmd1.Parameters.AddWithValue("@Con_Address1", "Door No");

                rep.SetParameterValue("INVNO", "");
                rep.SetParameterValue("Slip_No", SO_No);
                viewer.crystalReportViewer1.ReportSource = rep;
                viewer.crystalReportViewer1.Refresh();
                rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);
                Process.Start(path);

                con.Close();
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
        private string path;
        public ListofLoadingSlips()
        {
            InitializeComponent();
        }

        private void ListofLoadingSlips_Load(object sender, EventArgs e)
        {
            BindOrderslist();
        }

        public void BindOrderslist()
        {
            try
            {
                string p = "";

                var da = (from obj in db.User_Roles
                          where obj.Role_ID == logIn.UserRoleID && obj.Company_ID == logIn.company
                          select obj).ToList();

                if (da[0].Roll_Type == "User")
                {
                    p = "user";
                }
                else
                {
                    p = "admin";
                }
                var d = (from data in db.ShowLoadingSlips(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, logIn.BU_ID) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;

                    this.sfDataGrid1.QueryCellStyle += SfDataGrid1_QueryCellStyle;
                    string cellValue;
                    //for (int i = 2; i < sfDataGrid1.RowCount; i++)
                    //{
                    //    //        SetCellBackgroundColor(new RowColumnIndex(i, 7), Color.Gray);
                    //    //        SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.Gray);
                    //    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    //    var mappingName = sfDataGrid1.Columns["Status"].MappingName;
                    //    //var mappingName1 = sfDataGrid1.Columns["Quote_Validity"].MappingName;
                    //    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    //    //var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                        
                    //}
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["Loading_Slip_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Loading_Slip_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Loading_Slip_No"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Loading_Slip_No"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["Customer_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Customer_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Customer_Name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Customer_Name"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["VehicleNo"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["VehicleNo"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["VehicleNo"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["VehicleNo"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["Assigned_To"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Assigned_To"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Assigned_To"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Assigned_To"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Status"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Status"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Status"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Status"].FilterRowCondition = FilterRowCondition.Contains;



                    this.sfDataGrid1.TableSummaryRows.Clear();
                    GridTableSummaryRow tableSummaryRow1 = new GridTableSummaryRow();
                    tableSummaryRow1.Name = "TableSummary";
                    tableSummaryRow1.ShowSummaryInRow = false;
                    tableSummaryRow1.Position = VerticalPosition.Bottom;

                    GridSummaryColumn summaryColumn1 = new GridSummaryColumn();
                    summaryColumn1.Name = "No Of Slips";
                    summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn1.Format = "No Of Slips: {Count}";
                    summaryColumn1.MappingName = "Loading_Slip_No";

                    tableSummaryRow1.SummaryColumns.Add(summaryColumn1);

                    this.sfDataGrid1.TableSummaryRows.Add(tableSummaryRow1);
                }
                //else
                //{
                //    MessageBox.Show("Record Not Found");
                //    //txtSearch.Text = "";
                //}

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public string Status = "";
        private void SfDataGrid1_QueryCellStyle(object sender, QueryCellStyleEventArgs e)
        {


            if (e.Column.MappingName == "Status")
            {
                if (e.DisplayText == "Created")
                {
                    Status = "Created";
                    e.Style.BackColor = Color.LightSkyBlue;
                    e.Style.TextColor = Color.Black;
                }
                else if (e.DisplayText == "Closed")
                {
                    Status = "Closed";
                    e.Style.BackColor = Color.LightGreen;
                    e.Style.TextColor = Color.DarkSlateBlue;

                }
                else if (e.DisplayText == "Cancelled")
                {
                    Status = "Cancelled";
                    e.Style.BackColor = Color.Red;
                    e.Style.TextColor = Color.Black;

                }
            }

           
        }
    }
}
