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
using iTextSharp.text.pdf;
using System.Security.Cryptography.X509Certificates;
using iTextSharp.text.pdf.security;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.Security;
using X509Certificate = Org.BouncyCastle.X509.X509Certificate;
using System.Net.Mail;
using System.Net;
using Ione_DAL;
using Syncfusion.Windows.Forms.CellGrid.ScrollAxis;
using Syncfusion.Windows.Forms.Chart;

namespace ioneNet.OrderManagement
{
  
    public partial class frmCRMDashBoard : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        Dictionary<RowColumnIndex, Color> colorDict = new Dictionary<RowColumnIndex, Color>();

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string InvoiceNoList, Order_NoList, SO_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;
        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        private string path;

        private void printInvoiceOnPrePrintedFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //int i = sfDataGrid2.SelectedIndex;
                //var currentCellValue = sfDataGrid2.CurrentCell.CellRenderer.GetControlValue();
                //var rowData = sfDataGrid2.GetRecordAtRowIndex(i);
                //var mappingName = sfDataGrid2.Columns[0].MappingName;
                //var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                //// string path1 = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.pdf");
                //SqlCommand cmd1 = con.CreateCommand();
                //if (con.State != ConnectionState.Open)
                //    con.Open();
                ////int i = sfDataGrid1.CurrentRow.Index;
                //SO_No = cellVaue.ToString();
                //cmd1.CommandText = "UPDATE temp_Inv_Copy SET Inv_No = @InvNo where company_ID = @comp";
                //cmd1.Parameters.AddWithValue("@InvNo", SO_No);
                //cmd1.Parameters.AddWithValue("@comp", logIn.company);
                //cmd1.ExecuteNonQuery();
                //path = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.pdf");
                ////string path = @"D:\Invoice.pdf";
                //FileInfo fi1 = new FileInfo(path);


                ////if (fi1.Exists)
                ////{
                ////    fi1.Delete();
                ////}
                //SqlCommand cmd = new SqlCommand("sp_Rpt_InvoiceReport", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Invoice_No", SO_No);
                //cmd.Parameters.AddWithValue("@Creation_Company", logIn.company);
                //cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                //SqlDataAdapter da = new SqlDataAdapter(cmd);

                //DataTable Dt = new DataTable();

                //da.SelectCommand = cmd;
                //da.Fill(Dt);
                //if (Dt.Rows.Count > 0)
                //{
                //    CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                //    //Get Invoice Format Mapped to the Company
                //    var gstno = (from c in db.Company_Report_Formats
                //                 where c.Company_ID == logIn.company
                //                 select new { c.GSTInv_Format }).ToList();
                //    if (gstno.Count > 0)
                //    {
                //        if (gstno[0].GSTInv_Format == "InvV")
                //        {
                //            rep = new OrderManagement.Transactions.SaleInvoice_GST_Oth();
                //        }
                        

                //    }
                //    else
                //    {
                //        rep = new OrderManagement.Transactions.SaleInvoice_GST_Oth();

                //    }

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
                //    rep.SetDataSource(Dt);
                //    ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                //    // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                //    viewer.crystalReportViewer1.ReportSource = rep;
                //    viewer.crystalReportViewer1.Refresh();
                //    rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);
                //    Process.Start(path);
                //    cmd.Parameters.Clear();
                //}
                //con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void printInvoiceWithDigitalSignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //groupBox1.Visible = true;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            //if (comboBox1.Text == "Original for Receipent")
            //{
            //    textBox1.Text = "1";
            //}
            //if (comboBox1.Text == "Duplicate for Transporter / Supplier")
            //{
            //    textBox1.Text = "2";
            //}
            //if (comboBox1.Text == "Triplicate for Supplier")
            //{
            //    textBox1.Text = "3";
            //}
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {

                //int i = sfDataGrid2.SelectedIndex;
                //var currentCellValue = sfDataGrid2.CurrentCell.CellRenderer.GetControlValue();
                //var rowData = sfDataGrid2.GetRecordAtRowIndex(i);
                //var mappingName = sfDataGrid2.Columns[0].MappingName;
                //var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                //// string path1 = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.pdf");
                //SqlCommand cmd1 = con.CreateCommand();
                //SqlCommand cmd2 = con.CreateCommand();
                //if (con.State != ConnectionState.Open)
                //    con.Open();
                ////int i = sfDataGrid1.CurrentRow.Index;
                //SO_No = cellVaue.ToString();

                //SqlCommand cmd4 = new SqlCommand("delete  from [temp_Inv_Copy] where Company_ID =@comp", con);
                //cmd4.Parameters.AddWithValue("@comp", logIn.company);
                //cmd4.ExecuteNonQuery();

                //cmd2.CommandText = "INSERT INTO dbo.temp_Inv_Copy ([Inv_No],[Copy_Name],[Copy_No],[Company_ID]) VALUES (@invNo,@Copy_Name,@Copy_No, @comp)";

                //// SqlCommand command = new SqlCommand(query, db.Connection);
                //cmd2.Parameters.AddWithValue("@invNo", SO_No);
                //cmd2.Parameters.AddWithValue("@Copy_Name", comboBox1.Text);
                //cmd2.Parameters.AddWithValue("@Copy_No", textBox1.Text);
                //cmd2.Parameters.AddWithValue("@comp", logIn.company);


                //cmd2.ExecuteNonQuery();
                //path = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.pdf");
                ////string path = @"D:\Invoice.pdf";
                //FileInfo fi1 = new FileInfo(path);
                //SqlCommand cmd = new SqlCommand("sp_Rpt_InvoiceReport", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Invoice_No", SO_No);
                //cmd.Parameters.AddWithValue("@Creation_Company", logIn.company);
                //cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);

                //SqlDataAdapter da = new SqlDataAdapter(cmd);

                //DataTable Dt = new DataTable();

                //da.SelectCommand = cmd;
                //da.Fill(Dt);
                //if (Dt.Rows.Count > 0)
                //{
                //    CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                //        rep = new OrderManagement.Transactions.SaleInvoice_GST_Oth();



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
                //    rep.SetDataSource(Dt);
                //    ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                //    // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                //    viewer.crystalReportViewer1.ReportSource = rep;
                //    viewer.crystalReportViewer1.Refresh();
                //    rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                //    cmd.Parameters.Clear();
                //}
                //con.Close();


                ////Get Certifictae
                //X509CertificateParser cp = new X509CertificateParser();

                ////Get Sertifiacte
                //X509Certificate2 certClient = null;
                //X509Store st = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                //st.Open(OpenFlags.MaxAllowed);
                //X509Certificate2Collection collection = X509Certificate2UI.SelectFromCollection(st.Certificates,
                //    "Please choose certificate:", "", X509SelectionFlag.SingleSelection);
                //if (collection.Count > 0)
                //{
                //    certClient = collection[0];
                //}
                //st.Close();
                ////Get Cert Chain
                //IList<X509Certificate> chain = new List<X509Certificate>();
                //X509Chain x509Chain = new X509Chain();

                //x509Chain.Build(certClient);

                //foreach (X509ChainElement x509ChainElement in x509Chain.ChainElements)
                //{
                //    chain.Add(Org.BouncyCastle.Security.DotNetUtilities.FromX509Certificate(x509ChainElement.Certificate));
                //}

                //PdfReader inputPdf = new PdfReader(path);
                //string path1 = Path.Combine(Directory.GetCurrentDirectory(), "Invoice_Signed.pdf");
                //FileStream signedPdf = new FileStream(path1, FileMode.Create);

                //PdfStamper pdfStamper = PdfStamper.CreateSignature(inputPdf, signedPdf, '\0');

                //IExternalSignature externalSignature = new X509Certificate2Signature(certClient, "SHA-1");

                //PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;
                //signatureAppearance.Reason = "I Approved This Document";
                //signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(635, 63, 385, 102), 1, null);
                //signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;

                //MakeSignature.SignDetached(signatureAppearance, externalSignature, chain, null, null, null, 0,
                //    CryptoStandard.CMS);
                //inputPdf.Close();
                //pdfStamper.Close();
                //Process.Start(path1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void printChallanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //// string path1 = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.pdf");
                //SqlCommand cmd1 = con.CreateCommand();
                //if (con.State != ConnectionState.Open)
                //    con.Open();
                //int i = sfDataGrid2.SelectedIndex;
                //var currentCellValue = sfDataGrid2.CurrentCell.CellRenderer.GetControlValue();
                //var rowData = sfDataGrid2.GetRecordAtRowIndex(i);
                //var mappingName = sfDataGrid2.Columns[0].MappingName;
                //var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                //SO_No = cellVaue.ToString();
                //cmd1.CommandText = "UPDATE temp_Inv_Copy SET Inv_No = @InvNo where Company_ID = @compID";
                //cmd1.Parameters.AddWithValue("@InvNo", SO_No);
                //cmd1.Parameters.AddWithValue("@compID", logIn.company);
                //cmd1.ExecuteNonQuery();
                //string path = Path.Combine(Directory.GetCurrentDirectory(), "DeliverChallan.pdf");
                ////string path = @"D:\Invoice.pdf";
                //FileInfo fi1 = new FileInfo(path);


                //if (fi1.Exists)
                //{
                //    fi1.Delete();
                //}
                //SqlCommand cmd = new SqlCommand("sp_Rpt_InvoiceReport", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Invoice_No", SO_No);
                //cmd.Parameters.AddWithValue("@Creation_Company", logIn.company);
                //cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                //SqlDataAdapter da = new SqlDataAdapter(cmd);

                //DataTable Dt = new DataTable();

                //da.SelectCommand = cmd;
                //da.Fill(Dt);
                //if (Dt.Rows.Count > 0)
                //{
                //    CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                //    rep = new OrderManagement.Transactions.DeliveryChallan();


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

                //    rep.SetDataSource(Dt);
                //    ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                //    // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                //    viewer.crystalReportViewer1.ReportSource = rep;
                //    viewer.crystalReportViewer1.Refresh();
                //    rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);
                //    Process.Start(path);
                //    cmd.Parameters.Clear();
                //}
                //con.Close();
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

        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                //    int i = sfDataGrid2.SelectedIndex;
                //    var currentCellValue = sfDataGrid2.CurrentCell.CellRenderer.GetControlValue();
                //    var rowData = sfDataGrid2.GetRecordAtRowIndex(i);
                //    var mappingName = sfDataGrid2.Columns[0].MappingName;
                //    var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                //    // string path1 = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.pdf");
                //    SqlCommand cmd1 = con.CreateCommand();
                //    SqlCommand cmd2 = con.CreateCommand();
                //    if (con.State != ConnectionState.Open)
                //        con.Open();
                //    //int i = sfDataGrid1.CurrentRow.Index;
                //    SO_No = cellVaue.ToString();

                //    SqlCommand cmd4 = new SqlCommand("delete  from [temp_Inv_Copy] where Company_ID =@comp", con);
                //    cmd4.Parameters.AddWithValue("@comp", logIn.company);
                //    cmd4.ExecuteNonQuery();

                //    CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                //        rep = new OrderManagement.Transactions.SaleInvoice_GST_Oth();
                //        cmd1.CommandText = "INSERT INTO temp_Inv_Copy  (Inv_No, Copy_Name, Copy_No,Company_ID) VALUES  (@invNo, 'Original for Receipent', '1'," + logIn.company + "),   (@invNo, 'Duplicate for Transporter / Supplier', '2'," + logIn.company + "),   (@invNo, 'Triplicate for Supplier', '3'," + logIn.company + ")";
                //        //cmd1.CommandText =  "INSERT INTO dbo.temp_Inv_Copy ([Inv_No],[Copy_Name],[Copy_No],[Company_ID]) VALUES (@invNo,@Copy_Name,@Copy_No, @comp)";

                //        //// SqlCommand command = new SqlCommand(query, db.Connection);
                //        cmd1.Parameters.AddWithValue("@invNo", SO_No);

                //        cmd1.ExecuteNonQuery();



                //    path = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.pdf");
                //    //string path = @"D:\Invoice.pdf";
                //    FileInfo fi1 = new FileInfo(path);


                //    //if (fi1.Exists)
                //    //{
                //    //    fi1.Delete();
                //    //}
                //    SqlCommand cmd = new SqlCommand("sp_Rpt_InvoiceReport", con);
                //    cmd.CommandType = CommandType.StoredProcedure;
                //    cmd.Parameters.AddWithValue("@Invoice_No", SO_No);
                //    cmd.Parameters.AddWithValue("@Creation_Company", logIn.company);
                //    cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                //    SqlDataAdapter da = new SqlDataAdapter(cmd);

                //    DataTable Dt = new DataTable();

                //    da.SelectCommand = cmd;
                //    da.Fill(Dt);
                //    if (Dt.Rows.Count > 0)
                //    {








                //        crConnectionInfo.ServerName = frmMain.ServerIP;
                //        crConnectionInfo.DatabaseName = frmMain.Database;
                //        crConnectionInfo.UserID = frmMain.DBUserID;
                //        crConnectionInfo.Password = frmMain.Password;


                //        crDatabase = rep.Database;
                //        crTables = crDatabase.Tables;
                //        //Loop through all tables in the report and apply the connection information for each table.
                //        for (int k = 0; k < crTables.Count; k++)
                //        {
                //            //  crTable = crTables[i];
                //            crTableLogOnInfo = crTables[k].LogOnInfo;
                //            crTableLogOnInfo.ConnectionInfo = crConnectionInfo;
                //            crTables[k].ApplyLogOnInfo(crTableLogOnInfo);

                //        }
                //        rep.SetDataSource(Dt);
                //        ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                //        // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                //        viewer.crystalReportViewer1.ReportSource = rep;
                //        viewer.crystalReportViewer1.Refresh();
                //        rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                //        cmd.Parameters.Clear();
                //    }
                //    con.Close();


                //    Process.Start(path);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void toolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

        public static Boolean editMode;

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

        }

        private void chart1_Click(object sender, EventArgs e)
        {

        }

        public frmCRMDashBoard()
        {
            InitializeComponent();
        }

        private void sfButton1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmCRMDashBoard_Load(object sender, EventArgs e)
        {

            cmbYear.Text = "Current Month";
            

            bindDashBoard();
            //this.sfDataGrid1.Style.HeaderStyle.BackColor = Color.LightSkyBlue;
            //this.sfDataGrid1.Style.HeaderStyle.TextColor = Color.Black;
            //this.sfDataGrid1.Style.HeaderStyle.Font.Bold = true;

            //this.sfDataGrid2.Style.HeaderStyle.BackColor = Color.Lavender;
            //this.sfDataGrid2.Style.HeaderStyle.TextColor = Color.Black;
            //this.sfDataGrid2.Style.HeaderStyle.Font.Bold = true;

        }

        private void sfButton11_Click(object sender, EventArgs e)
        {
            bindDashBoard();
        }
        public void bindDashBoard()
        {

            int year, month, days,startmonth,startyear,days2, startmonth1;
            string endDate = "";
            string startdate = "";
            string endDate1 = "";
            string startdate1 = "";
            year = Convert.ToInt32(DateTime.Now.Year.ToString());
            month = Convert.ToInt32(DateTime.Now.Month.ToString());
          
            if (cmbYear.Text == "Current Month")
            {
                days = DateTime.DaysInMonth(year, month);
                startdate = year + "-" + month + "-" + 1;
                endDate = year + "-" + month + "-" + days;

            }
            if (cmbYear.Text == "Last Month")
            {

                startmonth = month - 1;
                days = DateTime.DaysInMonth(year, startmonth);
                startdate = year + "-" + startmonth + "-" + 1;
                endDate = year + "-" + startmonth + "-" + days;

            }
            if (cmbYear.Text == "Last 3 Months")
            {

                startmonth = month - 3;
                days = DateTime.DaysInMonth(year, startmonth);
                days2 = DateTime.DaysInMonth(year, month);
                startdate = year + "-" + startmonth + "-" + 1;
                endDate = year + "-" + month + "-" + days2;

            }
            if (cmbYear.Text == "Last 6 Months")
            {

                startmonth = month - 6;
                days = DateTime.DaysInMonth(year, startmonth);
                days2 = DateTime.DaysInMonth(year, month);
                startdate = year + "-" + startmonth + "-" + 1;
                endDate = year + "-" + month + "-" + days2;

            }
            if (cmbYear.Text == "Last 12 Months")
            {

                startmonth = month - 12;
                days = DateTime.DaysInMonth(year, startmonth);
                days2 = DateTime.DaysInMonth(year, month);
                startdate = year + "-" + startmonth + "-" + 1;
                endDate = year + "-" + month + "-" + days2;

            }

            DateTime SDate = Convert.ToDateTime(startdate);

            DateTime EDate = Convert.ToDateTime(endDate);
            //Bind Sale Data Graph

            DateTime dt =logIn.fy_Start_Date;
            string dt1 = dt.ToString("yyyy/MM/dd");

            DateTime dtt = logIn.fy_End_Date;
            string dt2 = dtt.ToString("yyyy/MM/dd");

            
            //Bind Target Vs Actual Bar Chart
            SqlCommand cmd = new SqlCommand("SP_Bind_Sale_TargetVsActual", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FromDate", SDate);
            cmd.Parameters.AddWithValue("@ToDate", EDate);
            cmd.Parameters.AddWithValue("@compname", logIn.company);
            cmd.Parameters.AddWithValue("@SaleExe", 12);
            cmd.Parameters.AddWithValue("@para", 2);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable ds = new DataTable();
            da.Fill(ds);          

            chart1.DataSource = ds;
            chart1.Series["Tot_Target"].XValueMember = "Salesmen_Code";
            chart1.Series["Tot_Target"].YValueMembers = "Tot_Target";
            chart1.Series["Qty_Achieved"].YValueMembers = "Qty_Achieved";
            chart1.DataBind();


            //Bind Order Booking Trend (Line Chart)

            //startmonth1 = month - 12;
            //int days1 = DateTime.DaysInMonth(year, startmonth1);
            //int days3 = DateTime.DaysInMonth(year, month);
            //startdate1 = year + "-" + startmonth1 + "-" + 1;
            //endDate1 = year + "-" + month + "-" + days3;


            SqlCommand cmdOB = new SqlCommand("SP_Bind_Sale_Trend_Graph", con);
            cmdOB.CommandType = CommandType.StoredProcedure;
            cmdOB.Parameters.AddWithValue("@FromDate", logIn.fy_Start_Date);
            cmdOB.Parameters.AddWithValue("@ToDate", EDate);
            cmdOB.Parameters.AddWithValue("@compname", logIn.company);
            cmdOB.Parameters.AddWithValue("@buid", logIn.BU_ID);
           
            SqlDataAdapter daOB = new SqlDataAdapter(cmdOB);
            DataTable dsOB = new DataTable();
            daOB.Fill(dsOB);

            chart6.DataSource = dsOB;
            chart6.Series["Order_Book_Qty"].XValueMember = "eMonth";
            chart6.Series["Order_Book_Qty"].YValueMembers = "Ord_Qty";
            
            chart6.DataBind();



            //Bind Sales Funnel Chart Data
            SqlCommand cmd5 = new SqlCommand("SP_Bind_Sales_Data_Graph", con);
            cmd5.CommandType = CommandType.StoredProcedure;
            cmd5.Parameters.AddWithValue("@FromDate", SDate);
            cmd5.Parameters.AddWithValue("@ToDate", EDate);
            cmd5.Parameters.AddWithValue("@compname", logIn.company);
            cmd5.Parameters.AddWithValue("@para", 1);
            cmd5.Parameters.AddWithValue("@saleExe", 12);

            SqlDataAdapter da5 = new SqlDataAdapter(cmd5);
            DataTable ds5 = new DataTable();
            da5.Fill(ds5);

            chart3.DataSource = ds5;
            //chart3.Series["Series1"].XValueMember = "Parameter";
            chart3.Series["Series1"].YValueMembers = "Qty";
            chart3.DataBind();



            //Bind Product Wise Orders (PIE CHART)
            SqlCommand cmd6 = new SqlCommand("SP_Bind_Sale_ProductWise_Graph", con);
            cmd6.CommandType = CommandType.StoredProcedure;
            cmd6.Parameters.AddWithValue("@FromDate", SDate);
            cmd6.Parameters.AddWithValue("@ToDate", EDate);
            cmd6.Parameters.AddWithValue("@compname", logIn.company);
            cmd6.Parameters.AddWithValue("@buid", logIn.BU_ID);


            SqlDataAdapter da6 = new SqlDataAdapter(cmd6);
            DataTable ds6 = new DataTable();
            da6.Fill(ds6);

            chart4.DataSource = ds6;
            chart4.Series["Series1"].XValueMember = "PGroup";
            chart4.Series["Series1"].YValueMembers = "Ord_Qty";
            chart4.DataBind();


            //Bind BusinessLost Analysis (PIE CHART)
            SqlCommand cmd7 = new SqlCommand("SP_BusinessLost_Analysis", con);
            cmd7.CommandType = CommandType.StoredProcedure;
            cmd7.Parameters.AddWithValue("@FromDate", SDate);
            cmd7.Parameters.AddWithValue("@ToDate", EDate);
            cmd7.Parameters.AddWithValue("@compname", logIn.company);
            cmd7.Parameters.AddWithValue("@buid", logIn.BU_ID);


            SqlDataAdapter da7 = new SqlDataAdapter(cmd7);
            DataTable ds7 = new DataTable();
            da7.Fill(ds7);

            chart5.DataSource = ds7;
            chart5.Series["Series1"].XValueMember = "Buiness_Lost_Reason";
            chart5.Series["Series1"].YValueMembers = "Lost_Qty";
            chart5.DataBind();





            //Order Count
            int cr,a,p,d,cl,v;
             var cnt = (from s in db.OrderCount(logIn.company, SDate,EDate ,logIn.BU_ID) select s).ToList();
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
                if (cnt[0].PreClosed == null)
                {
                    d = 0;
                }
                else
                {
                    d = cnt[0].PreClosed;
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
                linkLabel4.Text =   Tot.ToString();
                linkLabel7.Text =  (Tot -cl-d).ToString();
                //    + Convert.ToUInt32(cnt[0].Despatches_Started.ToString()) + Convert.ToUInt32(cnt[0].Closed.ToString());
            }

            //Quote Count
            
            var qcnt = (from s in db.QuotesCount(logIn.company, SDate, EDate) select s).ToList();
            //var d = (from data in db.ShowSOList(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, null) select data).ToList();
            if (qcnt.Count > 0)
            {
                //int C = cnt[0].Created.Value;
                if (qcnt[0].Created == null)
                {
                    cr = 0;
                }
                else
                {
                    cr = qcnt[0].Created.Value;
                }               
                if (qcnt[0].Lost == null)
                {
                    d = 0;
                }
                else
                {
                    d = qcnt[0].Lost.Value;
                }
                if (qcnt[0].Order_Received == null)
                {
                    a = 0;
                }
                else
                {
                    a = qcnt[0].Order_Received.Value;
                }
                if (qcnt[0].Validity_Expired == null)
                {
                    v = 0;
                }
                else
                {
                    v = qcnt[0].Validity_Expired.Value;
                }
                decimal totq = a + d + cr + v;

                linkLabel1.Text = (totq).ToString();
                linkLabel3.Text = (d).ToString();
                linkLabel2.Text = (v).ToString();
                
                decimal or = a;
                if (totq > 0 && or > 0)
                {
                    decimal HitRate = (or / totq) * 100;
                    linkLabel5.Text = HitRate.ToString("0.00");
                }
               

                //    + Convert.ToUInt32(cnt[0].Despatches_Started.ToString()) + Convert.ToUInt32(cnt[0].Closed.ToString());
            }


            //Order Count
            double pval;
            //var pv = (from s in db.Order_Value_Pending(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date,logIn.BU_ID) select s).ToList();
            ////var d = (from data in db.ShowSOList(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, null) select data).ToList();
            //if (cnt.Count > 0)
            //{
            //    //int C = cnt[0].Created.Value;
            //    if (pv[0].Pending_Order_Value == null)
            //    {
            //        pval = 0;
            //    }
            //    else
            //    {
            //        pval = pv[0].Pending_Order_Value.Value;
            //    }
            //   // decimal pendVal = Convert.ToDecimal(pval) / 100000;
            //    sfButton4.Text = "Pend Ord Val  " + "\n" + (pval.ToString("00.00"));
               
                //    + Convert.ToUInt32(cnt[0].Despatches_Started.ToString()) + Convert.ToUInt32(cnt[0].Closed.ToString());
            //}

            //Sale Value
            double sval;

//            var employeeCount = (from s in db.Invoice_Masters
//                                 where s.Company_ID == logIn.company && s.BU_ID == logIn.BU_ID && s.Status !=25
//&& s.InvDate >= logIn.fy_Start_Date
//&& s.InvDate <= logIn.fy_End_Date

//                                 select s.Tot_Inv_Value).Sum();


//            decimal SalVal = Convert.ToDecimal(employeeCount)/100000;

//            sfButton6.Text = "Sales (Gross in Lacs)  " + "\n" + (SalVal.ToString("00.00"));


            //Top 5 Customers
            //Bind Sale Data Graph
                   
          
            SqlCommand cmd1 = new SqlCommand("Top5Customers", con);
            cmd1.CommandType = CommandType.StoredProcedure;
            cmd1.Parameters.AddWithValue("@fY_SDate", SDate);
            cmd1.Parameters.AddWithValue("@fY_EDate", EDate);
            cmd1.Parameters.AddWithValue("@compname", logIn.company);
            SqlDataAdapter da1 = new SqlDataAdapter(cmd1);
            DataTable ds1 = new DataTable();
            da1.Fill(ds1);

            chart2.DataSource = ds1;
            chart2.Series["Sale_Value"].XValueMember = "Customer";
            chart2.Series["Sale_Value"].YValueMembers = "SaleValue";
            chart2.DataBind();

        }
        void SetCellBackgroundColor(RowColumnIndex rowColumnIndex, Color color)
        {
            //if (!colorDict.ContainsKey(rowColumnIndex))
            //    colorDict.Add(rowColumnIndex, color);
            //else
            //    colorDict[rowColumnIndex] = color;
            //sfDataGrid1.TableControl.Invalidate(this.sfDataGrid1.TableControl.GetCellRectangle(rowColumnIndex.RowIndex, rowColumnIndex.ColumnIndex, false));
        }
        private void autoLabel1_Click(object sender, EventArgs e)
        {

        }

        private void modifyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                //int i = sfDataGrid2.SelectedIndex;
                //var currentCellValue = sfDataGrid2.CurrentCell.CellRenderer.GetControlValue();
                //var rowData = sfDataGrid2.GetRecordAtRowIndex(i);
                //var mappingName = sfDataGrid2.Columns[0].MappingName;
                //var mappingName1 = sfDataGrid2.Columns[4].MappingName;
                //var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                //if (cellVaue.ToString() != "")
                //{
                //    SO_No = cellVaue.ToString();
                //    var = "0";
                //    editMode = true;
                //    OrderManagement.Transactions.frmNewInvoice frm = new OrderManagement.Transactions.frmNewInvoice();
                //    //OrderManagement.Transactions.
                //    frm.MdiParent = this.MdiParent;
                //    frm.Show();                  
                //    //FrmInv.ShowDialog();
                //    //i1 = 0;
                //}
                //else
                //{
                //    MessageBox.Show("Please Select Any One Record");
                //    //i1 = 0;
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
               
                //string filepath = "";
                //int i = sfDataGrid2.SelectedIndex;
                //var currentCellValue = sfDataGrid2.CurrentCell.CellRenderer.GetControlValue();
                //var rowData = sfDataGrid2.GetRecordAtRowIndex(i);
                //var mappingName = sfDataGrid2.Columns[0].MappingName;
                //var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                //// string path1 = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.pdf");
                //SqlCommand cmd1 = con.CreateCommand();
                //SqlCommand cmd2 = con.CreateCommand();
                //if (con.State != ConnectionState.Open)
                //    con.Open();
                ////int i = sfDataGrid1.CurrentRow.Index;
                //SO_No = cellVaue.ToString();

                //SqlCommand cmd4 = new SqlCommand("delete  from [temp_Inv_Copy] where Company_ID =@comp", con);
                //cmd4.Parameters.AddWithValue("@comp", logIn.company);
                //cmd4.ExecuteNonQuery();
                //cmd1.CommandText = "INSERT INTO temp_Inv_Copy  (Inv_No, Copy_Name, Copy_No,Company_ID) VALUES  (@invNo, 'Original for Receipent', '1'," + logIn.company + "),   (@invNo, 'Duplicate for Transporter / Supplier', '2'," + logIn.company + "),   (@invNo, 'Triplicate for Supplier', '3'," + logIn.company + ")";
                ////cmd1.CommandText =  "INSERT INTO dbo.temp_Inv_Copy ([Inv_No],[Copy_Name],[Copy_No],[Company_ID]) VALUES (@invNo,@Copy_Name,@Copy_No, @comp)";

                ////// SqlCommand command = new SqlCommand(query, db.Connection);
                //cmd1.Parameters.AddWithValue("@invNo", SO_No);

                //cmd1.ExecuteNonQuery();

                //path = Path.Combine(Directory.GetCurrentDirectory(), "Invoice_" + SO_No + ".pdf");
                ////string path = @"D:\Invoice.pdf";
                //FileInfo fi1 = new FileInfo(path);


                ////if (fi1.Exists)
                ////{
                ////    fi1.Delete();
                ////}
                //SqlCommand cmd = new SqlCommand("sp_Rpt_InvoiceReport", con);
                //cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@Invoice_No", SO_No);
                //cmd.Parameters.AddWithValue("@Creation_Company", logIn.company);
                //cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                //SqlDataAdapter da = new SqlDataAdapter(cmd);

                //DataTable Dt = new DataTable();

                //da.SelectCommand = cmd;
                //da.Fill(Dt);
                //if (Dt.Rows.Count > 0)
                //{
                //    CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                //    //Get Invoice Format Mapped to the Company
                //    var gstno = (from c in db.Company_Report_Formats
                //                 where c.Company_ID == logIn.company
                //                 select new { c.GSTInv_Format }).ToList();
                //    if (gstno.Count > 0)
                //    {
                //        if (gstno[0].GSTInv_Format == "InvV")
                //        {
                //            rep = new OrderManagement.Transactions.SaleInvoice_GST_Oth();
                //        }
                        

                //    }
                //    else
                //    {
                //        rep = new OrderManagement.Transactions.SaleInvoice_GST_Oth();

                //    }

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
                //    rep.SetDataSource(Dt);
                //    ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                //    // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                //    viewer.crystalReportViewer1.ReportSource = rep;
                //    viewer.crystalReportViewer1.Refresh();
                //    rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                //    cmd.Parameters.Clear();
                //}
                //con.Close();
                          
                //filepath = path;


                //var Email = (from c in db.Invoice_Masters
                //             join inv in db.Supplier_informations
                //             on c.BuyerName equals inv.ID
                //             join em in db.EMailServerSettings on c.Company_ID equals em.company_ID
                //             where c.Company_ID == logIn.company && c.Inv_No == cellVaue
                //             select new { inv.Contact_Email, em.SmtpServer, em.SmptPort, em.POMailID, em.POMailPW }).ToList();
                //string email = Email[0].Contact_Email;
                //if(email =="")
                //{
                //    MessageBox.Show("Customer Email Not Found In the Records");
                //    return;
                //}
                //// string email = Email[0].Cust_Eail;
                //MailMessage mm = new MailMessage();
                //mm.From = new MailAddress(Email[0].POMailID);

                //mm.To.Add(email + ",viswanath@laksanait.com");
                //mm.Subject = "Invoice No :" + cellVaue;
                //mm.Body = "Dear Sir," + "\n" + "Please Find Attached Invoice for Your information";
                //// mm.Attachments.Add(new Attachment(new MemoryStream(bytes), "Purchase_Vocher" + DateTime.Now.ToString("dd_MM_yyyy_HHmmss") + ".pdf"));
                //mm.Attachments.Add(new Attachment(filepath));
                //string nme;


                ////}
                //mm.IsBodyHtml = true;
                //SmtpClient smtp = new SmtpClient();
                //smtp.Host = Email[0].SmtpServer;
                //smtp.EnableSsl = true;
                //NetworkCredential NetworkCred = new NetworkCredential();
                //NetworkCred.UserName = Email[0].POMailID;
                //NetworkCred.Password = Email[0].POMailPW;
                //smtp.UseDefaultCredentials = true;
                //smtp.Credentials = NetworkCred;
                //smtp.Port = Convert.ToInt32(Email[0].SmptPort);
                //smtp.Send(mm);
                ////var ci = db.Purchase_Order_Masters.Where(w => w.PO_NO == cellVaue && w.Company_ID == logIn.company).FirstOrDefault();
                ////{
                ////    ci.email_Sent = true;
                ////    db.SubmitChanges();
                ////}
                //MessageBox.Show("Invoice Sent by Email Successfully");
            }
            catch (Exception Ex)
            {
                Ex.Message.ToString();
            }
        }
    }
}
