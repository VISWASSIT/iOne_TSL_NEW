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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Net.Mail;
using System.Net;
using System.Security;
using iTextSharp.text.pdf;
using System.Security.Cryptography.X509Certificates;
using iTextSharp.text.pdf.security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Security;
using X509Certificate = Org.BouncyCastle.X509.X509Certificate;
using System.IO;
using Ione_DAL;
using Syncfusion.WinForms.DataGridConverter;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Events;
using Syncfusion.WinForms.DataGrid;
using Syncfusion.Data;

namespace ioneNet.OrderManagement.Transactions
{
    public partial class ListOfQuotes : Form
    {
        public static string InvoiceNoList, Order_NoList, SO_No,SO_Amend_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;
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
                            cmd.CommandText = "Update Sale_Quotation_Master set status = '6' where Quot_NO=@param1 and Company_ID =@compName";
                            cmd.Parameters.AddWithValue("@param1", SO_No);
                            cmd.Parameters.AddWithValue("@CompName", logIn.company);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            cmd.Parameters.Clear();
                            string strT = logIn.username + "-" + DateTime.Now;
                            cmd.CommandText = "Update Sale_Quotation_Master set Modified_By = @strT where Quot_NO=@param1 and Company_ID =@compName";
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
                MessageBox.Show("Selected Quote(s) Are Approved Successfully");
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

                int i = sfDataGrid1.CurrentCell.RowIndex;
                //if (i >= 0)
                //{
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var currentCellValue = (rowData.GetType().GetProperty("Quot_NO").GetValue(rowData, null).ToString());
                var currentAmendValue = (rowData.GetType().GetProperty("Quot_Amend_No").GetValue(rowData, null).ToString());

                var mappingName = sfDataGrid1.Columns["Status"].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());


                if (cellVaue.ToString() != "Order Received")
                {
                    groupBox1.Visible = true;
                    comboBox1.Focus();
                }
                else
                {
                    MessageBox.Show("Order Received Against Selected Quotation, Hence No Buinses Lost Data is Required");
                    return;
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
                    int i = sfDataGrid1.CurrentCell.RowIndex;
                    //if (i >= 0)
                    //{
                    var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                    //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                    var currentCellValue = (rowData.GetType().GetProperty("Quot_NO").GetValue(rowData, null).ToString());
                    var currentAmendValue = (rowData.GetType().GetProperty("Quot_Amend_No").GetValue(rowData, null).ToString());

                    var mappingName = sfDataGrid1.Columns["Status"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());


                    if (cellVaue.ToString() != "Order Received")
                        {
                            SqlCommand cmd = new SqlCommand();

                            SO_No = currentCellValue.ToString();
                            cmd.CommandText = "Update Sale_Quotation_Master set status = '24' where Quot_NO=@param1 and Company_ID =@compName";
                            cmd.Parameters.AddWithValue("@param1", SO_No);
                            cmd.Parameters.AddWithValue("@CompName", logIn.company);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            cmd.Parameters.Clear();
                            string strT = logIn.username + "-" + DateTime.Now;
                            cmd.CommandText = "Update Sale_Quotation_Master set Modified_By = @strT where Quot_NO=@param1 and Company_ID =@compName";
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
                            MessageBox.Show("Selected Quote Cannot Be Deleted As Already Order Received, Pre-Close the order insted");
                        }
                           
                    }
                    MessageBox.Show("Selected Quote(s) Are Deleted Successfully");
                    BindOrderslist();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgvRecordList_DataSourceChanged(object sender, EventArgs e)
        {
           
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


            BindOrderslist();

        }
        void sfDataGrid_QueryCellStyle(object sender, Syncfusion.WinForms.DataGrid.Events.QueryCellStyleEventArgs e)
        {
            var rowColumnIndex = new RowColumnIndex(e.RowIndex, e.ColumnIndex);
            if (colorDict.ContainsKey(rowColumnIndex))
                e.Style.BackColor = colorDict[rowColumnIndex];
        }

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns[1].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                // string path1 = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.pdf");
                SqlCommand cmd1 = con.CreateCommand();
                SqlCommand cmd2 = con.CreateCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                //int i = sfDataGrid1.CurrentRow.Index;
                SO_No = cellVaue.ToString();


                path = Path.Combine(Directory.GetCurrentDirectory(), "Quotation.pdf");
                //string path = @"D:\Invoice.pdf";
                FileInfo fi1 = new FileInfo(path);
                DataTable Dt = new DataTable();

                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                                                    
                   
                rep = new OrderManagement.Transactions.Quotation_STIPL();

                SqlCommand cmd = new SqlCommand("sp_Rpt_Quotation", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Quot_No", SO_No);
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

        private void eMailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SendEmail();
        }

        void SetCellBackgroundColor(RowColumnIndex rowColumnIndex, Color color)
        {
            if (!colorDict.ContainsKey(rowColumnIndex))
                colorDict.Add(rowColumnIndex, color);
            else
                colorDict[rowColumnIndex] = color;
            sfDataGrid1.TableControl.Invalidate(this.sfDataGrid1.TableControl.GetCellRectangle(rowColumnIndex.RowIndex, rowColumnIndex.ColumnIndex, false));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
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
                        var mappingName = sfDataGrid1.Columns[1].MappingName;
                        //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                        //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                        if (rowData == item)
                        {
                            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                           var ci = db.Sale_Quotation_Masters.Where(w => w.Quot_NO == cellVaue && w.Company_ID == logIn.company && w.bu_id == logIn.BU_ID).FirstOrDefault();
                            {
                                ci.Status = 88;
                                if (comboBox1.Text != "Others")
                                {
                                    ci.Buiness_Lost_Reason = comboBox1.Text;
                                 }
                                else
                                {
                                    ci.Buiness_Lost_Reason = textBox1.Text;
                                }
                                ci.Modified_By = logIn.username + "-" + DateTime.Now;
                                db.SubmitChanges();
                            }


                            SqlCommand cmd = new SqlCommand();

                            SO_No = cellVaue.ToString();
                            cmd.CommandText = "Update Sale_Quotation_Master set status = '86' where Quot_NO=@param1 and Company_ID =@compName";
                            cmd.Parameters.AddWithValue("@param1", SO_No);
                            cmd.Parameters.AddWithValue("@CompName", logIn.company);
                            cmd.Connection = con;
                            con.Open();
                            cmd.ExecuteNonQuery();
                            con.Close();
                            cmd.Parameters.Clear();
                            string strT = logIn.username + "-" + DateTime.Now;
                            cmd.CommandText = "Update Sale_Quotation_Master set Modified_By = @strT where Quot_NO=@param1 and Company_ID =@compName";
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
                MessageBox.Show("Selected Quote(s) Status Updated Successfully");
                BindOrderslist();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                //Quote on Selected Format

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


                path = Path.Combine(Directory.GetCurrentDirectory(), "Quotation.pdf");
                //string path = @"D:\Invoice.pdf";
                FileInfo fi1 = new FileInfo(path);
                DataTable Dt = new DataTable();

                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

              
                //if (comboBox2.Text == "Others - I")
                //{
                        
                //    rep = new OrderManagement.Transactions.rptQuotation_RFPL_Oth1();
                //}
                //else
                //    if (comboBox2.Text == "Others -II")
                //{
                //    //RFPL
                //    rep = new OrderManagement.Transactions.rptQuotation_RFPL_Oth2();
                       
                //}

                //else
                //    if (comboBox2.Text == "Job Work")
                //{
                //    rep = new OrderManagement.Transactions.rptQuotation_RFPL_JW();


                //}
                SqlCommand cmd = new SqlCommand("sp_Rpt_Quotation_RFPL", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Quot_No", SO_No);
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

        private void printOnSelectedFormatToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = true;
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
            workBook.Worksheets[0].Range["A2"].Value = "Quoations List";
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\QuotationList.xlsx");
            string doc = Fname + "\\QuotationList.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
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
                var currentCellValue = (rowData.GetType().GetProperty("Quot_NO").GetValue(rowData, null).ToString());
                var currentAmendValue = (rowData.GetType().GetProperty("Quot_Amend_No").GetValue(rowData, null).ToString());
                var mappingName = sfDataGrid1.Columns["Status"].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                if (cellVaue.ToString() != "Closed" || cellVaue.ToString() != "Pre Closed")
                {
                    if (cellVaue.ToString() == "Created" || cellVaue.ToString() == "Approved")
                    {
                        var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Sale Quotation" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                        if (uRole.Count > 0)
                        {
                            if (uRole[0].Modify_Role == true)
                            {
                                SO_No = currentCellValue.ToString();
                                SO_Amend_No = currentAmendValue.ToString();
                                var = "1";
                                editMode = true;
                                sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.None;
                                OrderManagement.Transactions.frmQuotation frm = new frmQuotation();
                                //OrderManagement.Transactions.
                                frm.MdiParent = this.MdiParent;
                                frm.Show();
                                sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;

                            }
                            else
                            {
                                MessageBox.Show("You Have No Permissions to Modify The Quotation");
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
                        MessageBox.Show("The Quote Cannot Be Modified Either Closed or Order Receiced");
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

        public static Boolean editMode;
        public ListOfQuotes()
        {
            InitializeComponent();
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
                var d = (from data in db.ShowQuotesList(logIn.company,logIn.fy_Start_Date,logIn.fy_End_Date,logIn.BU_ID, logIn.userID,p) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                   
                    this.sfDataGrid1.QueryCellStyle += SfDataGrid1_QueryCellStyle;
                    string cellValue;
                for (int i = 2; i < sfDataGrid1.RowCount-1; i++)
                {
                        //        SetCellBackgroundColor(new RowColumnIndex(i, 7), Color.Gray);
                        //        SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.Gray);
                        var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                        var mappingName = sfDataGrid1.Columns["Status"].MappingName;
                        var mappingName1 = sfDataGrid1.Columns["Quote_Validity"].MappingName;
                        var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                        var cellVaue1 = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
                        if (cellVaue.ToString() == "Approved" || cellVaue.ToString() == "Created")
                        {
                            SetCellBackgroundColor(new RowColumnIndex(i, 7), Color.LightSkyBlue);
                            DateTime dtvalid = Convert.ToDateTime(cellVaue1);
                            if (dtvalid <= DateTime.Now)
                            {
                                SetCellBackgroundColor(new RowColumnIndex(i, 8), Color.Red);
                            }
                            else
                            {
                                //SetCellBackgroundColor(new RowColumnIndex(i + 1, 8), Color.Red);
                            }


                        }
                            //    if (cellVaue.ToString() == "Order Received")
                            //    {
                            //        SetCellBackgroundColor(new RowColumnIndex(i, 7), Color.LightGreen);
                            //    }
                            //    if (cellVaue.ToString() == "Closed")
                            //    {
                            //        SetCellBackgroundColor(new RowColumnIndex(i, 7), Color.SaddleBrown);
                            //    }
                            //    if (cellVaue.ToString() == "Lost")
                            //    {
                            //        SetCellBackgroundColor(new RowColumnIndex(i, 7), Color.Red);
                            //    }
                }
                            this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                this.sfDataGrid1.Columns["Quot_NO"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["Quot_NO"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Quot_NO"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["Quot_NO"].FilterRowCondition = FilterRowCondition.Contains;
                this.sfDataGrid1.Columns["Customer_name"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["Customer_name"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Customer_name"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["Customer_name"].FilterRowCondition = FilterRowCondition.Contains;
                this.sfDataGrid1.Columns["Sales_Executive_Name"].FilterRowEditorType = "TextBox";
                this.sfDataGrid1.Columns["Sales_Executive_Name"].ShowFilterRowOptions = false;
                this.sfDataGrid1.Columns["Sales_Executive_Name"].ImmediateUpdateColumnFilter = true;
                this.sfDataGrid1.Columns["Sales_Executive_Name"].FilterRowCondition = FilterRowCondition.Contains;
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
                    summaryColumn1.Name = "No Of Quotes";
                    summaryColumn1.SummaryType = SummaryType.DoubleAggregate;
                    summaryColumn1.Format = "No Of Quotes: {Count}";
                    summaryColumn1.MappingName = "Quot_NO";

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
                else if (e.DisplayText == "Order Received")
                {
                    Status = "Order Received";
                    e.Style.BackColor = Color.LightGreen;
                    e.Style.TextColor = Color.DarkSlateBlue;

                }
                else if (e.DisplayText == "Lost")
                {
                    Status = "Lost";
                    e.Style.BackColor = Color.Red;
                    e.Style.TextColor = Color.Black;

                }
            }

            if (e.Column.MappingName == "Quote_Validity")
            {

                if (Status == "Created")
                {
                    DateTime dtvalid = Convert.ToDateTime(e.DisplayText);
                    if (dtvalid <= DateTime.Now)
                    {
                        e.Style.BackColor = Color.Orange;
                    }
                }
            }
        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.None;

            editMode = false;
            OrderManagement.Transactions.frmQuotation frm = new frmQuotation();
           frm.MdiParent = this.MdiParent;
           frm.Show();
            sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;

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
                    var currentCellValue =  (rowData.GetType().GetProperty("Quot_NO").GetValue(rowData, null).ToString());
                    var currentAmendValue = (rowData.GetType().GetProperty("Quot_Amend_No").GetValue(rowData, null).ToString());

                    var mappingName = sfDataGrid1.Columns["Status"].MappingName;
                    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                    if (cellVaue.ToString() != "Closed" || cellVaue.ToString() != "Pre Closed")
                    {
                        if (cellVaue.ToString() == "Created" ||  cellVaue.ToString() == "Approved")
                        {
                            var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "Quotation"  && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                            if (uRole.Count > 0)
                            {
                                if(uRole[0].Modify_Role==true)
                                {
                                    SO_No = currentCellValue.ToString();
                                    SO_Amend_No = currentAmendValue.ToString();
                                    var = "0";
                                    editMode = true;
                                    sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.None;

                                    OrderManagement.Transactions.frmQuotation frm = new frmQuotation();
                                    //OrderManagement.Transactions.
                                    frm.MdiParent = this.MdiParent;
                                    frm.Show();
                                    sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;


                            }
                            else
                                {
                                    MessageBox.Show("You Have No Permissions to Modify The Approved or Processed Quotation");
                                    return;
                                }
                         }
                        else
                        {
                            SO_No = currentCellValue.ToString();
                            SO_Amend_No = currentAmendValue.ToString();
                            var = "0";
                            editMode = true;
                            sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.None;
                            OrderManagement.Transactions.frmQuotation frm = new frmQuotation();
                            //OrderManagement.Transactions.
                            frm.MdiParent = this.MdiParent;
                            
                            frm.Show();
                            sfDataGrid1.AutoSizeColumnsMode = AutoSizeColumnsMode.Fill;
                            
                            //FrmInv.ShowDialog();
                            //i1 = 0;
                        }
                    }
                    else
                    {
                        MessageBox.Show("The Quoote Cannot Be Modified Either Closed or Order Received");
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
        public void SendEmail()
        {
            try
            {
                string filepath = "";
                DataTable Dt = new DataTable();
                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns[1].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                // string path1 = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.pdf");
                SqlCommand cmd1 = con.CreateCommand();
                SqlCommand cmd2 = con.CreateCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                //int i = sfDataGrid1.CurrentRow.Index;
                SO_No = cellVaue.ToString();


                path = Path.Combine(Directory.GetCurrentDirectory(), "Quotation.pdf");
                //string path = @"D:\Invoice.pdf";
                FileInfo fi1 = new FileInfo(path);


                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                //Get Invoice Format Mapped to the Company
                var gstno = (from c in db.Company_Report_Formats
                             where c.Company_ID == logIn.company
                             select new { c.Quot_Format }).ToList();
                if (gstno.Count > 0)
                {
                    if (gstno[0].Quot_Format == "Q1")
                    {
                        //Takhi Drive
                        //rep = new OrderManagement.Transactions.Quotation();

                        //SqlCommand cmd = new SqlCommand("sp_Rpt_Quotation", con);
                        //cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@Quot_No", SO_No);
                        //cmd.Parameters.AddWithValue("@Creation_Company", logIn.company);
                        //cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                        //SqlDataAdapter da = new SqlDataAdapter(cmd);

                        ////DataTable Dt = new DataTable();

                        //da.SelectCommand = cmd;
                        //da.Fill(Dt);
                    }
                    else
                    if (gstno[0].Quot_Format == "Q3")
                    {
                        //Takhi Drive
                        rep = new OrderManagement.Transactions.Quotation_STIPL();

                        SqlCommand cmd = new SqlCommand("sp_Rpt_Quotation", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@Quot_No", SO_No);
                        cmd.Parameters.AddWithValue("@Creation_Company", logIn.company);
                        cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                        SqlDataAdapter da = new SqlDataAdapter(cmd);

                        //DataTable Dt = new DataTable();

                        da.SelectCommand = cmd;
                        da.Fill(Dt);
                    }
                    else

                     if (gstno[0].Quot_Format == "Q2")
                    {
                        //MPower
                        //rep = new OrderManagement.Transactions.rptQuotation_RFPL();

                        //SqlCommand cmd = new SqlCommand("sp_Rpt_Quotation_RFPL", con);
                        //cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.AddWithValue("@Quot_No", SO_No);
                        //cmd.Parameters.AddWithValue("@Creation_Company", logIn.company);
                        //cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                        //SqlDataAdapter da = new SqlDataAdapter(cmd);

                        //da.SelectCommand = cmd;
                        //da.Fill(Dt);
                    }

                    else
                    {
                       // rep = new OrderManagement.Transactions.Quotation();


                    }

                }

                else
                {
                    //rep = new OrderManagement.Transactions.Quotation();

                }



                //if (fi1.Exists)
                //{
                //    fi1.Delete();
                //}

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
                    filepath = path;


                    var Email = (from c in db.Sale_Quotation_Masters
                                 join inv in db.Supplier_informations
                                 on c.BuyerName equals inv.ID
                                 join em in db.EMailServerSettings on c.Company_ID equals em.company_ID
                                 join se in db.Sales_Men_Informations on inv.Sale_Executive equals se.Id
                                 where c.Company_ID == logIn.company && c.Quot_NO == cellVaue.ToString()
                                 select new { inv.Email_Id, exe_Mail_id = se.Email_Id , em.SmtpServer, em.SmptPort, em.QuotMailID, em.QuotMailPW , em.Default_CC_Mail_id }).ToList();
                    string email = "";
                    if (Email.Count > 0)
                    {
                        email = "v4viswanbath@hotmail.com";
                        email = Email[0].Email_Id;
                    }
                    else
                    {
                        MessageBox.Show("Customer Mail ID Not Available to Send E Mail");
                        return;
                    }
                    // string email = Email[0].Cust_Eail;
                    MailMessage mm = new MailMessage();
                    mm.From = new MailAddress(Email[0].exe_Mail_id);
                    mm.To.Add(email);
                    if (Email[0].Default_CC_Mail_id != null)
                    {
                        mm.CC.Add(Email[0].Default_CC_Mail_id);
                    }
                    mm.Subject = "Quotation No :" + currentCellValue.ToString();
                    mm.Body = "Dear Sir," + "\n" + "Above referred Quotation is attached here with. Hope the Quotation is in line with your requirement and expecting your favourbale reply at the earliest";
                    // mm.Attachments.Add(new Attachment(new MemoryStream(bytes), "Purchase_Vocher" + DateTime.Now.ToString("dd_MM_yyyy_HHmmss") + ".pdf"));
                    mm.Attachments.Add(new Attachment(filepath));
                    string nme;


                    //}
                    mm.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = Email[0].SmtpServer;
                    smtp.EnableSsl = true;
                    NetworkCredential NetworkCred = new NetworkCredential();
                    NetworkCred.UserName = Email[0].QuotMailID;
                    NetworkCred.Password = Email[0].QuotMailPW;
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = NetworkCred;
                    smtp.Port = Convert.ToInt32(Email[0].SmptPort);
                    try
                    {
                        smtp.Send(mm);
                        MessageBox.Show("Quote Sent by Email Successfully");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                        Debug.WriteLine("Exception Message: " + ex.Message);
                        if (ex.InnerException != null)
                            Debug.WriteLine("Exception Inner:   " + ex.InnerException);
                    }
                    
                }
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message);
            }
        }
    }
}
