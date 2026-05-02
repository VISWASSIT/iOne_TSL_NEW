using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Interactivity;
using System.Diagnostics;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Net.Mail;
using System.Net;
using Ione_DAL;
using System.Runtime.InteropServices.ComTypes;
namespace ioneNet.MaterialManagement
{
  
    public partial class frmMMDashBoard : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string InvoiceNoList, Order_NoList, SO_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;
        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        public frmMMDashBoard()
        {
            InitializeComponent();
        }

        private void sfButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCRMDashBoard_Load(object sender, EventArgs e)
        {
            try
            {
                bindDashBoard();
                this.sfDataGrid1.Style.HeaderStyle.BackColor = Color.LightSkyBlue;
                this.sfDataGrid1.Style.HeaderStyle.TextColor = Color.Black;
                this.sfDataGrid1.Style.HeaderStyle.Font.Bold = true;

                this.sfDataGrid2.Style.HeaderStyle.BackColor = Color.Lavender;
                this.sfDataGrid2.Style.HeaderStyle.TextColor = Color.Black;
                this.sfDataGrid2.Style.HeaderStyle.Font.Bold = true;

                var bindLoc = (from m in db.User_Roles
                               where m.Company_ID == logIn.company && m.Role_ID == logIn.UserRoleID && m.Form_Name == "Purchase Order"
                               select new
                               {
                                   m.Approve_Role,
                                   m.View_Role,
                                   m.Modify_Role,
                                   m.Create_Role,
                                   m.Delete_Role
                               }).ToList();
                if (bindLoc.Count>0) {
                    this.modifyToolStripMenuItem.Enabled = false;
                    if (bindLoc[0].Modify_Role == true)
                    {
                        this.modifyToolStripMenuItem.Enabled = true;
                    }
                    this.approveToolStripMenuItem1.Enabled = false;
                    if (bindLoc[0].Approve_Role == true)
                    {
                        this.approveToolStripMenuItem1.Enabled = true;
                    }


                    this.deleteToolStripMenuItem1.Enabled = false;
                    if (bindLoc[0].Delete_Role == true)
                    {

                        this.deleteToolStripMenuItem1.Enabled = true;
                    }
                    this.viewToolStripMenuItem1.Enabled = true;
                    if (bindLoc[0].View_Role == true)
                    {

                        this.viewToolStripMenuItem1.Enabled = true;
                    }

                }
                var bindLoc1 = (from m in db.User_Roles
                               where m.Company_ID == logIn.company && m.Role_ID == logIn.UserRoleID && m.Form_Name == "Purchase Requisition"
                                select new
                               {
                                   m.Approve_Role,
                                   m.View_Role,
                                   m.Modify_Role,
                                   m.Create_Role,
                                   m.Delete_Role
                               }).ToList();
                if (bindLoc1.Count > 0)
                {
                    this.toolStripMenuItem2.Enabled = false;
                    if (bindLoc1[0].Modify_Role == true)
                    {
                        this.toolStripMenuItem2.Enabled = true;
                    }
                    this.approveToolStripMenuItem.Enabled = false;
                    if (bindLoc1[0].Approve_Role == true)
                    {
                        this.approveToolStripMenuItem.Enabled = true;
                    }


                    this.deleteToolStripMenuItem.Enabled = false;
                    if (bindLoc1[0].Delete_Role == true)
                    {

                        this.deleteToolStripMenuItem.Enabled = true;
                    }

                    this.viewToolStripMenuItem.Enabled = false;

                    if (bindLoc1[0].View_Role == true)
                    {
                        this.viewToolStripMenuItem.Enabled = true;

                    }
                    
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void sfButton11_Click(object sender, EventArgs e)
        {
            bindDashBoard();
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //crys viewer1 = new CrstalReportViewer1();
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
                var mappingName = sfDataGrid2.Columns["PO_No"].MappingName;
                //var mappingName1 = sfDataGrid2.Columns[4].MappingName;
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

        public void bindDashBoard()
        {
            //Top 10 Orders

            var PR = (from data in db.ShowPRList_forApproval(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, logIn.BU_ID) select data).ToList();

            if (PR.Count > 0)
            {                
            
                //dgProductsList.DataSource = d;
                sfDataGrid1.DataSource = PR;
                (sfDataGrid1.Columns["Sel"] as GridCheckBoxColumn).AllowThreeState = false;
                (sfDataGrid1.Columns["Sel"] as GridCheckBoxColumn).AllowCheckBoxOnHeader = true;
            }
            //(this.sfDataGrid1.Columns["Order_No"] as GridHyperlinkColumn).HyperlinkOpenArea = HyperlinkOpenArea.Cell;
            //(this.sfDataGrid1.Columns["Order_No"] as GridHyperlinkColumn).HyperlinkOpenBehavior = HyperlinkOpenBehavior.SingleClick;

            //Top 10 Invoices
            var PO = (from data in db.ShowPOList_ForApproval(logIn.company,logIn.BU_ID) select data).ToList();

            if (PO.Count > 0)
            {
                //dgProductsList.DataSource = d;
                sfDataGrid2.DataSource = PO;

                (sfDataGrid2.Columns["Sel"] as GridCheckBoxColumn).AllowThreeState = false;
                (sfDataGrid2.Columns["Sel"] as GridCheckBoxColumn).AllowCheckBoxOnHeader = true;
            }
                
            //var dm2 = (from s in db.Invoice_Masters
            //           join c in db.Supplier_informations on s.BuyerName  equals c.ID
            //           where s.Company_ID == logIn.company && s.InvDate >= logIn.fy_Start_Date && s.InvDate<= logIn.fy_End_Date
            //           orderby s.Id descending
            //           select new
            //           {
            //               s.Inv_No,
            //               s.InvDate,
            //               c.Supplier_Name,
            //               s.CustomerPONo,
            //               s.Tot_Inv_Value

            //           }).Take(10);
            //SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm2);
            //SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //DataTable dtr = new DataTable();
            //da2.Fill(dtr);
            //if (dtr.Rows.Count >= 0)
            //{
            //    sfDataGrid2.DataSource = dtr;
            //}

            //(this.sfDataGrid2.Columns["Inv_No"] as GridHyperlinkColumn).HyperlinkOpenArea = HyperlinkOpenArea.Cell;
            //(this.sfDataGrid2.Columns["Inv_No"] as GridHyperlinkColumn).HyperlinkOpenBehavior = HyperlinkOpenBehavior.SingleClick;


            //Bind Sale Data Graph
            string EndDate = "";
            DateTime dt =logIn.fy_Start_Date;
            string dt1 = dt.ToString("yyyy/MM/dd");

            DateTime dtt = logIn.fy_End_Date;
            string dt2 = dtt.ToString("yyyy/MM/dd");

            int Year = Convert.ToInt32(DateTime.Now.Year.ToString());
            int Month = Convert.ToInt32(DateTime.Now.Month.ToString());
            int Days = DateTime.DaysInMonth(Year, Month);
            EndDate = Year + "-" + Month + "-" + Days;           
            SqlCommand cmd = new SqlCommand("SP_Bind_Purchase_Report_Graph", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FromDate", dt);
            cmd.Parameters.AddWithValue("@ToDate", dtt);
            cmd.Parameters.AddWithValue("@compname", logIn.company);
            cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);          

            chart1.DataSource = ds;
            //chart1.Series["Tot_Target"].XValueMember = "Salesmen_Code";
            //chart1.Series["Tot_Target"].YValueMembers = "Tot_Target";
            //chart1.Series["Qty_Achieved"].YValueMembers = "Qty_Achieved";
            

            chart1.Series["Spares"].XValueMember = "eMonth";           
            chart1.Series["Spares"].YValueMembers = "Spares";
            chart1.Series["Steam_Coal"].XValueMember = "eMonth";
            chart1.Series["Steam_Coal"].YValueMembers = "Steam_Coal";
            chart1.Series["Capital"].XValueMember = "eMonth";
            chart1.Series["Capital"].YValueMembers = "Capital";
            chart1.Series["Rolls"].XValueMember = "eMonth";
            chart1.Series["Rolls"].YValueMembers = "Rolls";
            //chart1.Series["Purchase_Value"].YValueMembers = "Spares";
            //chart1.Series["Purchase_Value"].YValueMembers = "Spares";
            chart1.DataBind();

            SqlCommand cmdIssue = new SqlCommand("SP_Bind_Consumption_Report_Graph", con);
            cmdIssue.CommandType = CommandType.StoredProcedure;
            cmdIssue.Parameters.AddWithValue("@FromDate", dt);
            cmdIssue.Parameters.AddWithValue("@ToDate", dtt);
            cmdIssue.Parameters.AddWithValue("@compname", logIn.company);
            cmdIssue.Parameters.AddWithValue("@buid", logIn.BU_ID);
            SqlDataAdapter daIssue = new SqlDataAdapter(cmdIssue);
            DataTable dsIssue = new DataTable();
            daIssue.Fill(dsIssue);

            chart3.DataSource = dsIssue;
            chart3.Series["Issue_Value"].XValueMember = "eMonth";
            chart3.Series["Issue_Value"].YValueMembers = "Amount";
            chart3.DataBind();


            string FDate = Year + "-" + Month + "-01";
            string EDate = Year + "-" + Month + "-" + Days;

            SqlCommand cmdDept = new SqlCommand("MaterialIssueSummary_DeptWise", con);
            cmdDept.CommandType = CommandType.StoredProcedure;
            cmdDept.Parameters.AddWithValue("@fDate", FDate);
            cmdDept.Parameters.AddWithValue("@tDate", EDate);
            cmdDept.Parameters.AddWithValue("@compname", logIn.company);
            cmdDept.Parameters.AddWithValue("@buid", logIn.BU_ID);
            SqlDataAdapter dadept = new SqlDataAdapter(cmdDept);
            DataTable dsdept = new DataTable();
            dadept.Fill(dsdept);

            dataGridView3.DataSource = dsdept;

            //Order Count
            int cr,a,p,d,cl;
            var cnt = (from s in db.PurchaseOrderCount(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date,logIn.BU_ID) select s).ToList();
            //var d = (from data in db.ShowSOList(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, null) select data).ToList();
            if (cnt.Count > 0)
            {
                //int C = cnt[0].Created.Value;
                if (cnt[0].Created == null)
                {
                    cr = 0;
                        }
                else
                {
                    cr = cnt[0].Created.Value;
                }
                if (cnt[0].Approved == null)
                {
                    a = 0;
                }
                else
                {
                    a = cnt[0].Approved.Value;
                }
                if (cnt[0].Process_Started == null)
                {
                    d = 0;
                }
                else
                {
                    d = cnt[0].Process_Started.Value;
                }
                if (cnt[0].Closed == null)
                {
                    cl = 0;
                }
                else
                {
                    cl = cnt[0].Closed.Value;
                }
                int Tot = cr + a + d + cl;
                sfButton10.Text = "Total PO(s)  " + "\n" + (Tot);
                sfButton8.Text = "Pending PO(s) " + "\n" + (Tot -cl);
                //    + Convert.ToUInt32(cnt[0].Despatches_Started.ToString()) + Convert.ToUInt32(cnt[0].Closed.ToString());
            }
            //Order Count
            decimal pval;
            var pv = (from s in db.PO_Value_Pending(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date) select s).ToList();
            //var d = (from data in db.ShowSOList(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, null) select data).ToList();
            if (pv.Count > 0)
            {
                //int C = cnt[0].Created.Value;
                if (pv[0].Pending_Order_Value == null)
                {
                    pval = 0;
                }
                else
                {
                    pval = pv[0].Pending_Order_Value.Value;
                }               
                
                sfButton2.Text = "Pend PO Val  " + "\n" + (pval);
               
                //    + Convert.ToUInt32(cnt[0].Despatches_Started.ToString()) + Convert.ToUInt32(cnt[0].Closed.ToString());
            }


            // Stock Value
            //decimal stockval;
            //DateTime dtToDate = DateTime.Now.Date;
            //var sv = (from s in db.getStockValue(logIn.company, logIn.BU_ID, dtToDate) select s).ToList();
            ////var d = (from data in db.ShowSOList(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, null) select data).ToList();
            //if (sv.Count > 0)
            //{
            //    //int C = cnt[0].Created.Value;
            //    if (sv[0].StockVal == null)
            //    {
            //        stockval = 0;
            //    }
            //    else
            //    {
            //        stockval = sv[0].StockVal.Value;
            //    }

            //    sfButton3.Text = "Stock Val  " + "\n" + (stockval);

            //    //    + Convert.ToUInt32(cnt[0].Despatches_Started.ToString()) + Convert.ToUInt32(cnt[0].Closed.ToString());
            //}


            //PURCHASE Value
            double sval;

            var employeeCount = (from s in db.GoodsReceiptNote_Masters
                                 where s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID
&& s.Grn_Date >= logIn.fy_Start_Date
&& s.Grn_Date <= logIn.fy_End_Date

                                 select s.Tot_Ord_Value).Sum();


            decimal SalVal = Convert.ToDecimal(employeeCount)/100000;

            sfButton6.Text = "Total Purchase  (Gross)  " + "\n" + (SalVal.ToString("00.00"));


            //Top 5 Customers
            //Bind Sale Data Graph
                   
          
            //SqlCommand cmd1 = new SqlCommand("Top5Customers", con);
            //cmd1.CommandType = CommandType.StoredProcedure;
            //cmd1.Parameters.AddWithValue("@fY_SDate", dt);
            //cmd1.Parameters.AddWithValue("@fY_EDate", dtt);
            //cmd1.Parameters.AddWithValue("@compname", logIn.company);
            //SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            //DataTable ds1 = new DataTable();
            //da1.Fill(ds1);

            //chart2.DataSource = ds1;
            //chart2.Series["Sale_Value"].XValueMember = "Customer";
            //chart2.Series["Sale_Value"].YValueMembers = "SaleValue";
            //chart2.DataBind();

        }

        private void eMailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendEmail();
        }

        private void deleteToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void accountPostingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void printToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        private void viewToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid2.SelectedIndex;
                var currentCellValue = sfDataGrid2.CurrentCell.CellRenderer.GetControlValue();
                //var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                //var mappingName = sfDataGrid1.Columns["PR_No"].MappingName;
                // var mappingName1 = sfDataGrid1.Columns[7].MappingName;
                // var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                // var cellvalue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                if (currentCellValue.ToString() != "")
                {
                    //if (cellvalue1.ToString() != "Closed" || cellvalue1.ToString() != "Pre Closed")
                    //{
                    //    if (cellvalue1.ToString() == "Created" || cellvalue1.ToString() == "Approved" || cellvalue1.ToString() == "Process Started")
                    //    {
                    var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Purchase Order" && m.Role_ID == logIn.UserRoleID select new { m.View_Role }).Distinct().ToList();
                    if (uRole.Count > 0)
                    {
                        if (uRole[0].View_Role == true)
                        {
                            SO_No = currentCellValue.ToString();
                            var = "2";
                            editMode = true;
                            MaterialManagement.PurchaseOrder frm = new PurchaseOrder();
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

                    //    }
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

        private void viewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["PR_No"].MappingName;
                // var mappingName1 = sfDataGrid1.Columns[7].MappingName;
                // var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                // var cellvalue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                if (currentCellValue.ToString() != "")
                {
                    //if (cellvalue1.ToString() != "Closed" || cellvalue1.ToString() != "Pre Closed")
                    //{
                    //    if (cellvalue1.ToString() == "Created" || cellvalue1.ToString() == "Approved" || cellvalue1.ToString() == "Process Started")
                    //    {
                    var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Purchase Requisition" && m.Role_ID == logIn.UserRoleID select new { m.View_Role }).Distinct().ToList();
                    if (uRole.Count > 0)
                    {
                        if (uRole[0].View_Role == true)
                        {
                            SO_No = currentCellValue.ToString();
                            var = "1";
                            editMode = true;
                            MaterialManagement.Transactions.MaterialIndent_Others frm = new MaterialManagement.Transactions.MaterialIndent_Others();
                            //OrderManagement.Transactions.
                            frm.MdiParent = this.MdiParent;
                            frm.Show();
                        }
                        else
                        {
                            MessageBox.Show("You Have No Permissions to Modify The Purchase Requisitions");
                            return;
                        }
                    }

                    //    }
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

        private void printToolStripMenuItem1_Click(object sender, EventArgs e)
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

        private void autoLabel1_Click(object sender, EventArgs e)
        {

        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //Modify PO Data
            try
            {
                int i = sfDataGrid2.SelectedIndex;
                var currentCellValue = sfDataGrid2.CurrentCell.CellRenderer.GetControlValue();
                //var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                //var mappingName = sfDataGrid1.Columns["PR_No"].MappingName;
                // var mappingName1 = sfDataGrid1.Columns[7].MappingName;
                // var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                // var cellvalue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                if (currentCellValue.ToString() != "")
                {
                    //if (cellvalue1.ToString() != "Closed" || cellvalue1.ToString() != "Pre Closed")
                    //{
                    //    if (cellvalue1.ToString() == "Created" || cellvalue1.ToString() == "Approved" || cellvalue1.ToString() == "Process Started")
                    //    {
                    var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Purchase Order" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                    if (uRole.Count > 0)
                    {
                        if (uRole[0].Modify_Role == true)
                        {
                            SO_No = currentCellValue.ToString();
                            var = "0";
                            editMode = true;
                            MaterialManagement.PurchaseOrder frm = new PurchaseOrder();
                            //OrderManagement.Transactions.
                            frm.MdiParent = this.MdiParent;
                            frm.Show();
                        }
                        else
                        {
                            MessageBox.Show("You Have No Permissions to Modify The Purchase Requisitions");
                            return;
                        }
                    }

                    //    }
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

        private void sfButton3_Click(object sender, EventArgs e)
        {

        }

        private void sfButton4_Click(object sender, EventArgs e)
        {

        }

        private void approveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < sfDataGrid1.RowCount; i++)
            {

                //foreach (var item in sfDataGrid1.SelectedItems)
                //{
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["PR_No"].MappingName;
                //var mappingName1 = sfDataGrid1.Columns["Status"].MappingName;
                var mappingName2 = sfDataGrid1.Columns["Sel"].MappingName;
                //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                var cellVaue3 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
                if (cellVaue3 == "True")
                {
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                   // var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());

                    //string Status = cellVaue1.ToString();
                    string OrdNo = cellVaue.ToString();
                    //if (Status == "Created")
                    //{

                        var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Purchase Requisition" && m.Role_ID == logIn.UserRoleID select new { m.Approve_Role }).Distinct().ToList();
                        if (uRole.Count > 0)
                        {
                            if (uRole[0].Approve_Role == true)
                            {


                                var ci = db.Purchase_Req_Masters.Where(w => w.PR_NO == OrdNo && w.Company_ID == logIn.company).FirstOrDefault();
                                {

                                    ci.Status = 6;
                                    db.SubmitChanges();

                                }
                                var ca = db.Purchase_Req_Masters.Where(w => w.PR_NO == OrdNo && w.Company_ID == logIn.company).FirstOrDefault();
                                {

                                    ci.Modified_By = logIn.username + "-" + DateTime.Now;
                                    db.SubmitChanges();

                                }
                            }
                            else
                            {
                                MessageBox.Show("You Have No Permissions to Approve The Purchase Requisitions");
                                return;
                            }
                        }

                    //}
                    //else
                    //{
                    //    MessageBox.Show("PR No " + OrdNo + " Is Already Approved or Further Processed.. No Work Done");

                    //}
                }

                //}
            }
            var PR = (from data in db.ShowPRList_forApproval(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, logIn.BU_ID) select data).ToList();

            if (PR.Count > 0)
            {

                //dgProductsList.DataSource = d;
                sfDataGrid1.DataSource = PR;
                (sfDataGrid1.Columns["Sel"] as GridCheckBoxColumn).AllowThreeState = false;
                (sfDataGrid1.Columns["Sel"] as GridCheckBoxColumn).AllowCheckBoxOnHeader = true;
            }
            else
            {
                sfDataGrid1.DataSource = null;
            }
        }
        
        private void approveToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < sfDataGrid2.RowCount; i++)
            {

                //foreach (var item in sfDataGrid1.SelectedItems)
                //{
                var rowData = sfDataGrid2.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid2.Columns["PO_No"].MappingName;
                //var mappingName1 = sfDataGrid1.Columns["Status"].MappingName;
                var mappingName2 = sfDataGrid2.Columns["Sel"].MappingName;
                //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                var cellVaue3 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
                if (cellVaue3 == "True")
                {
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    // var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());

                    //string Status = cellVaue1.ToString();
                    string OrdNo = cellVaue.ToString();
                    //if (Status == "Created")
                    //{

                    var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Purchase Order" && m.Role_ID == logIn.UserRoleID select new { m.Approve_Role }).Distinct().ToList();
                    if (uRole.Count > 0)
                    {
                        if (uRole[0].Approve_Role == true)
                        {


                            var ci = db.Purchase_Order_Masters.Where(w => w.PO_NO == OrdNo && w.Company_ID == logIn.company).FirstOrDefault();
                            {

                                ci.Status = 6;
                                db.SubmitChanges();

                            }
                            var ca = db.Purchase_Order_Masters.Where(w => w.PO_NO == OrdNo && w.Company_ID == logIn.company).FirstOrDefault();
                            {

                                ci.Modified_By = logIn.username + "-" + DateTime.Now;
                                db.SubmitChanges();

                            }
                        }
                        else
                        {
                            MessageBox.Show("You Have No Permissions to Approve The Purchase Order");
                            return;
                        }
                    }

                    //}
                    //else
                    //{
                    //    MessageBox.Show("PR No " + OrdNo + " Is Already Approved or Further Processed.. No Work Done");

                    //}
                }

                //}
            }
            var PO = (from data in db.ShowPOList_ForApproval(logIn.company,logIn.BU_ID) select data).ToList();

            if (PO.Count > 0)
            {
                //dgProductsList.DataSource = d;
                sfDataGrid2.DataSource = PO;

                (sfDataGrid2.Columns["Sel"] as GridCheckBoxColumn).AllowThreeState = false;
                (sfDataGrid2.Columns["Sel"] as GridCheckBoxColumn).AllowCheckBoxOnHeader = true;
            }
            else
            {
                sfDataGrid2.DataSource = null;
            }
        }
        public static Boolean editMode;

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns["PR_No"].MappingName;
               // var mappingName1 = sfDataGrid1.Columns[7].MappingName;
               // var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
               // var cellvalue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                if (currentCellValue.ToString() != "")
                {
                    //if (cellvalue1.ToString() != "Closed" || cellvalue1.ToString() != "Pre Closed")
                    //{
                    //    if (cellvalue1.ToString() == "Created" || cellvalue1.ToString() == "Approved" || cellvalue1.ToString() == "Process Started")
                    //    {
                            var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Purchase Requisition" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                            if (uRole.Count > 0)
                            {
                                if (uRole[0].Modify_Role == true)
                                {
                                    SO_No = currentCellValue.ToString();
                                    var = "0";
                                    editMode = true;
                                    MaterialManagement.Transactions.MaterialIndent_Others frm = new MaterialManagement.Transactions.MaterialIndent_Others();
                                    //OrderManagement.Transactions.
                                    frm.MdiParent = this.MdiParent;
                                    frm.Show();
                                }
                                else
                                {
                                    MessageBox.Show("You Have No Permissions to Modify The Purchase Requisitions");
                                    return;
                                }
                            }

                    //    }
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

        public void SendEmail()
        {
            try
            {
                
                //crys viewer1 = new CrstalReportViewer1();
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
                var mappingName = sfDataGrid1.Columns["PO_No"].MappingName;
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
                    // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                    viewer.crystalReportViewer1.ReportSource = rep;
                    viewer.crystalReportViewer1.Refresh();
                    rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                    cmd.Parameters.Clear();
                        
                }
                con.Close();
                string filepath = "";
                filepath = path;


                var Email = (from c in db.Purchase_Order_Masters
                             join inv in db.Supplier_informations
                             on c.SupplierName equals inv.ID
                             join em in db.EMailServerSettings on c.Company_ID equals em.company_ID
                             where c.Company_ID == logIn.company && c.PO_NO == currentCellValue.ToString()
                             select new { c.Contact_EMail, em.SmtpServer, em.SmptPort, em.POMailID, em.POMailPW }).ToList();
                string email = Email[0].Contact_EMail;
                // string email = Email[0].Cust_Eail;
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(Email[0].POMailID);
                mm.To.Add(email + ",viswanath@laksanait.com");
                mm.Subject = "Purchase Order No :" + currentCellValue.ToString();
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
                smtp.Port = Convert.ToInt32(Email[0].SmptPort);
                smtp.Send(mm);
                var ci = db.Purchase_Order_Masters.Where(w => w.PO_NO == currentCellValue.ToString() && w.Company_ID == logIn.company).FirstOrDefault();
                {
                    ci.email_Sent = true;
                    db.SubmitChanges();
                }
                MessageBox.Show("PO Sent by Email Successfully");
            }
            catch (Exception Ex)
            {
                Ex.Message.ToString();
            }
        }
    }
}
