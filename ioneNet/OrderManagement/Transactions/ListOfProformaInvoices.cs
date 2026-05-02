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
using System.IO;
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
using Ione_DAL;
using Syncfusion.WinForms.DataGridConverter;
using Syncfusion.WinForms.DataGrid.Enums;

namespace ioneNet.OrderManagement.Transactions
{
    public partial class ListOfProformaInvoices : Form
    {
        public static string InvoiceNoList, Order_NoList, SO_No, Shicomp_name, Consignee, Shivar, Usertype, var, inv_No1, FileToAttach;
        DataClasses1DataContext db = new DataClasses1DataContext();
        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        private string path;
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        private void dgvRecordList_FilterStringChanged(object sender, EventArgs e)
        {
            //this.showInvListBindingSource.Filter = dgvRecordList.FilterString;
        }

        private void dgvRecordList_SortStringChanged(object sender, EventArgs e)
        {
            //this.showInvListBindingSource.Sort = dgvRecordList.SortString;
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

               



                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                //Get Invoice Format Mapped to the Company
                var gstno = (from c in db.Company_Report_Formats
                             where c.Company_ID == logIn.company
                             select new { c.GSTInv_Format }).ToList();
                if (gstno.Count > 0)

                {

                    //InvV4
                    if (gstno[0].GSTInv_Format == "Inv5")
                    {
                        //rep = new OrderManagement.Transactions.ProformaInvoice_VPack();                       
                    }                   
                    else
                    {
                        if (gstno[0].GSTInv_Format == "InvV4")
                        {
                          //  rep = new OrderManagement.Transactions.KDS_Proforma_Invoice();
                        }
                        else
                        {
                            rep = new OrderManagement.Transactions.ProformaInvoice();
                        }                      

                    }
                }
                else
                {
                    rep = new OrderManagement.Transactions.ProformaInvoice();
                }

                path = Path.Combine(Directory.GetCurrentDirectory(), "Proforma_Invoice.pdf");
                //string path = @"D:\Invoice.pdf";
                FileInfo fi1 = new FileInfo(path);
               
                
                //if (fi1.Exists)
                //{
                //    fi1.Delete();
                //}
                SqlCommand cmd = new SqlCommand("sp_Rpt_Proforma_InvoiceReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Invoice_No", SO_No);
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


                // PdfSignature ps = new PdfSignature("serial number");
                // //load the PDF document
                // ps.LoadPdfDocument(path);
                // ps.SignaturePosition = SignaturePosition.BottomRight;
                // ps.SigningReason = "I approve this document";
                //// ps.SignaturePosition = SignaturePosition.TopRight;
                // //Load the signature certificate from Microsoft Certificate Store
                // ps.DigitalSignatureCertificate = DigitalCertificate.LoadCertificate(false, "",
                // "Select the certificate", "");
                // //write the signed file
                // File.WriteAllBytes(Path.Combine(Directory.GetCurrentDirectory(), "Invoice_Signed.pdf"), ps.ApplyDigitalSignature());
                // string path1 = Path.Combine(Directory.GetCurrentDirectory(), "Invoice_Signed.pdf");

                 Process.Start(path);

                //Get Certifictae
               // X509CertificateParser cp = new X509CertificateParser();

               // //Get Sertifiacte
               // X509Certificate2 certClient = null;
               // X509Store st = new X509Store(StoreName.My, StoreLocation.CurrentUser);
               // st.Open(OpenFlags.MaxAllowed);
               // X509Certificate2Collection collection = X509Certificate2UI.SelectFromCollection(st.Certificates,
               //     "Please choose certificate:", "", X509SelectionFlag.SingleSelection);
               // if (collection.Count > 0)
               // {
               //     certClient = collection[0];
               // }
               // st.Close();
               // //Get Cert Chain
               // IList<X509Certificate> chain = new List<X509Certificate>();
               // X509Chain x509Chain = new X509Chain();

               // x509Chain.Build(certClient);

               // foreach (X509ChainElement x509ChainElement in x509Chain.ChainElements)
               // {
               //     chain.Add(Org.BouncyCastle.Security.DotNetUtilities.FromX509Certificate(x509ChainElement.Certificate));
               // }

               // PdfReader inputPdf = new PdfReader(path);
               //  string path1 = Path.Combine(Directory.GetCurrentDirectory(), "Invoice_Signed.pdf");
               // FileStream signedPdf = new FileStream(path1, FileMode.Create);

               // PdfStamper pdfStamper = PdfStamper.CreateSignature(inputPdf, signedPdf, '\0');

               // IExternalSignature externalSignature = new X509Certificate2Signature(certClient, "SHA-1");

               // PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;
               // signatureAppearance.Reason = "I Approved This Document";                
               // signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(550, 50, 400,90), 1, null);
               // //signatureAppearance.SignatureGraphic = Image.GetInstance(pathToSignatureImage);
               //// signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(600,-0, 500,50), 1, null);
               // //signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(600, -0, 400, 150), 2, null);
               // signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;
                
               // MakeSignature.SignDetached(signatureAppearance, externalSignature, chain, null, null, null, 0,
               //     CryptoStandard.CMS);
               // inputPdf.Close();
               // pdfStamper.Close();
                // Process.Start(path1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
            

        private void ListOfOrders_Load(object sender, EventArgs e)
        {
            //showInvListTableAdapter.Fill(ioneDataSet.ShowInvList, logIn.company,null);
            //dgvRecordList.DataSource = showInvListBindingSource;
            BindOrderslist();
        }

        public static Boolean editMode;

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                int i = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
                var mappingName = sfDataGrid1.Columns[1].MappingName;
                var mappingName1 = sfDataGrid1.Columns[4].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());

                if (cellVaue.ToString() != "")
                {
                    var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "GST Invoice" && m.Role_ID == logIn.UserRoleID select new { m.Delete_Role }).Distinct().ToList();
                    if (uRole.Count > 0)
                    {
                        if (uRole[0].Delete_Role == true)
                        {

                            DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                            {
                                SO_No = cellVaue.ToString();

                                db.sp_ProformaInv_Delete(SO_No, logIn.company,logIn.BU_ID);
                                BindOrderslist();
                            }
                        }
                        else
                        {
                            MessageBox.Show("You Are Not Authorized To Do This Action");
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

        private void eMailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                SendEmail();
                //MessageBox.Show("Email Sent Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public ListOfProformaInvoices()
        {
            InitializeComponent();
        }

        private void printInvoiceOnPrePrintedFormToolStripMenuItem_Click(object sender, EventArgs e)
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
                if (con.State != ConnectionState.Open)
                    con.Open();
                //int i = sfDataGrid1.CurrentRow.Index;
                SO_No = cellVaue.ToString();
                cmd1.CommandText = "UPDATE temp_Inv_Copy SET Inv_No = @InvNo where company_ID = @comp";
                cmd1.Parameters.AddWithValue("@InvNo", SO_No);
                cmd1.Parameters.AddWithValue("@comp", logIn.company);
                cmd1.ExecuteNonQuery();
                path = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.pdf");
                //string path = @"D:\Invoice.pdf";
                FileInfo fi1 = new FileInfo(path);


                //if (fi1.Exists)
                //{
                //    fi1.Delete();
                //}
                SqlCommand cmd = new SqlCommand("sp_Rpt_InvoiceReport", con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Invoice_No", SO_No);
                cmd.Parameters.AddWithValue("@Creation_Company", logIn.company);
                cmd.Parameters.AddWithValue("@buid", logIn.BU_ID);
                SqlDataAdapter da = new SqlDataAdapter(cmd);

                DataTable Dt = new DataTable();

                da.SelectCommand = cmd;
                da.Fill(Dt);
                if (Dt.Rows.Count > 0)
                {
                    CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                    //Get Invoice Format Mapped to the Company
                    var gstno = (from c in db.Company_Report_Formats
                                 where c.Company_ID == logIn.company
                                 select new { c.GSTInv_Format }).ToList();
                    if (gstno.Count > 0)
                    {
                        if (gstno[0].GSTInv_Format == "InvV")
                        {
                            rep = new OrderManagement.Transactions.SaleInvoice_GST_Oth();
                        }
                        else
                         if (gstno[0].GSTInv_Format == "InvV1")
                        {
                            //rep = new OrderManagement.Transactions.rptExciseInvoice2();
                        }
                        else
                         if (gstno[0].GSTInv_Format == "InvV2")
                        {
                            //rep = new OrderManagement.Transactions.SaleInvoice_GST_Yen();
                        }
                        else
                        if (gstno[0].GSTInv_Format == "InvV4")
                        {
                           // rep = new OrderManagement.Transactions.GST_Invoice();
                        }
                       
                        else
                        {
                            //rep = new OrderManagement.Transactions.SaleInvoice_GST();

                        }

                    }
                    else
                    {
                        rep = new OrderManagement.Transactions.SaleInvoice_GST_Oth();

                    }

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
                    // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                    viewer.crystalReportViewer1.ReportSource = rep;
                    viewer.crystalReportViewer1.Refresh();
                    rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);
                    Process.Start(path);
                    cmd.Parameters.Clear();
                }
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        

        

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

       

     
        private void textBox1_TextChanged(object sender, EventArgs e)
        {

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
            workBook.Worksheets[0].Range["A2"].Value = "Proforma Invoice List";
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\Proforma_Invoices.xlsx");
            string doc = Fname + "\\Proforma_Invoices.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }

        private void accountPostingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        public void SendEmail()
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





            CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

            //Get Invoice Format Mapped to the Company
            var gstno = (from c in db.Company_Report_Formats
                         where c.Company_ID == logIn.company
                         select new { c.GSTInv_Format }).ToList();
            if (gstno.Count > 0)

            {


                if (gstno[0].GSTInv_Format == "Inv5")
                {
                    //rep = new OrderManagement.Transactions.ProformaInvoice_VPack();
                }
                else
                {
                    rep = new OrderManagement.Transactions.ProformaInvoice();


                }
            }
            else
            {
                rep = new OrderManagement.Transactions.ProformaInvoice();
            }

            path = Path.Combine(Directory.GetCurrentDirectory(), "Proforma_Invoice.pdf");
            //string path = @"D:\Invoice.pdf";
            FileInfo fi1 = new FileInfo(path);


            //if (fi1.Exists)
            //{
            //    fi1.Delete();
            //}
            SqlCommand cmd = new SqlCommand("sp_Rpt_Proforma_InvoiceReport", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Invoice_No", SO_No);
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
            string filepath = path;
            var Email = (from c in db.Proforma_Invoice_Masters
                         join inv in db.Supplier_informations
                         on c.BuyerName equals inv.ID
                         join em in db.EMailServerSettings on c.Company_ID equals em.company_ID
                         where c.Company_ID == logIn.company && c.Inv_No == cellVaue
                         select new { inv.Email_Id, em.SmtpServer, em.SmptPort, em.QuotMailID, em.QuotMailPW, em.Default_CC_Mail_id, c.Inv_No }).ToList();
            //string email = Email[0].Email_Id + ","+ Email[0].Default_CC_Mail_id;
            string email = "";
            //Get User Mail ID
            var Umail = (from c in db.User_Setups

                         where c.Company_ID == logIn.company && c.User_ID == logIn.userID
                         select new { c.Email }).ToList();
            if (Umail.Count > 0)
            {
                if (Umail[0].Email != "")
                {
                    email = Email[0].Email_Id + "," + Umail[0].Email;
                }
                else
                {
                    email = Email[0].Email_Id;
                    //email = "v4viswanath@hotmail.com";
                }
            }
            else
            {
                email = Email[0].Email_Id;
            }
            if (email == "")
            {
                MessageBox.Show("E Mail ID of Supplier Is Not Avaiable, No work done");
                return;
            }
            // string email = Email[0].Cust_Eail;
            MailMessage mm = new MailMessage();
            mm.From = new MailAddress(Email[0].QuotMailID);
            mm.To.Add(email);
            if (Email[0].Default_CC_Mail_id != null)
            { 
                mm.CC.Add(Email[0].Default_CC_Mail_id); 
            }
            mm.Subject = "Proforma Invoice No :" + Email[0].Inv_No;
            mm.Body = "Dear Sir," + "\n" + "Above referred Proforma Invoice attached here with. Please acknowledge the receipt of the same";
            // mm.Attachments.Add(new Attachment(new MemoryStream(bytes), "Purchase_Vocher" + DateTime.Now.ToString("dd_MM_yyyy_HHmmss") + ".pdf"));
            mm.Attachments.Add(new Attachment(filepath));
            string nme;


            //}
            mm.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = Email[0].SmtpServer;
            smtp.EnableSsl = false;
            NetworkCredential NetworkCred = new NetworkCredential();
            NetworkCred.UserName = Email[0].QuotMailID;
            NetworkCred.Password = Email[0].QuotMailPW;
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;
            smtp.Port = Convert.ToInt32(Email[0].SmptPort);
            
            //var ci = db.Purchase_Order_Masters.Where(w => w.Id == Convert.ToInt32(cellVaue) && w.Company_ID == logIn.company).FirstOrDefault();
            //{
            //    ci.email_Sent = true;
            //    db.SubmitChanges();
            //}
            try
            {
                smtp.Send(mm);
                MessageBox.Show("PI Sent by Email Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                Debug.WriteLine("Exception Message: " + ex.Message);
                if (ex.InnerException != null)
                    Debug.WriteLine("Exception Inner:   " + ex.InnerException);
            }
            
        }
        public void BindOrderslist()
        {
            try
            {
                var d = (from data in db.ShowProformaInvList(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, null,logIn.BU_ID) select data).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["Inv_No"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Inv_No"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Inv_No"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Inv_No"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["Customer_Name"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Customer_Name"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Customer_Name"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Customer_Name"].FilterRowCondition = FilterRowCondition.Contains;
                    this.sfDataGrid1.Columns["CustomerPONo"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["CustomerPONo"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["CustomerPONo"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["CustomerPONo"].FilterRowCondition = FilterRowCondition.Contains;
                    //this.sfDataGrid1.Columns["Status"].FilterRowEditorType = "TextBox";
                    //this.sfDataGrid1.Columns["Status"].ShowFilterRowOptions = false;
                    //this.sfDataGrid1.Columns["Status"].ImmediateUpdateColumnFilter = true;
                    //this.sfDataGrid1.Columns["Status"].FilterRowCondition = FilterRowCondition.Contains;
                }
               
                
                

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        void sfDataGrid_QueryCellStyle(object sender, Syncfusion.WinForms.DataGrid.Events.QueryCellStyleEventArgs e)
        {
            //var rowColumnIndex = new RowColumnIndex(e.RowIndex, e.ColumnIndex);
            //if (colorDict.ContainsKey(rowColumnIndex))
            //    e.Style.BackColor = colorDict[rowColumnIndex];
        }


        void SetCellBackgroundColor(RowColumnIndex rowColumnIndex, Color color)
        {
            //if (!colorDict.ContainsKey(rowColumnIndex))
            //    colorDict.Add(rowColumnIndex, color);
            //else
            //    colorDict[rowColumnIndex] = color;
            //sfDataGrid1.TableControl.Invalidate(this.sfDataGrid1.TableControl.GetCellRectangle(rowColumnIndex.RowIndex, rowColumnIndex.ColumnIndex, false));
        }
        private void btnAddNew_Click(object sender, EventArgs e)
        {
            editMode = false;
            ioneNet.OrderManagement.Transactions.frmProformaInvoice frm = new frmProformaInvoice();
           frm.MdiParent = this.MdiParent;
           frm.Show();
            BindOrderslist();
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            //showInvListTableAdapter.Fill(ioneDataSet.ShowInvList, logIn.company, txtSearch.Text);
            //dgvRecordList.DataSource = showInvListBindingSource;
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
                var mappingName = sfDataGrid1.Columns[1].MappingName;
                var mappingName1 = sfDataGrid1.Columns[4].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
               
                if (cellVaue.ToString() != "")
                {
                    var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "GST Invoice" && m.Role_ID == logIn.UserRoleID select new { m.Modify_Role }).Distinct().ToList();
                    if (uRole.Count > 0)
                    {
                        if (uRole[0].Modify_Role == true)
                        {
                            SO_No = cellVaue.ToString();
                            var = "0";
                            editMode = true;
                            OrderManagement.Transactions.frmProformaInvoice frm = new frmProformaInvoice();
                            //OrderManagement.Transactions.
                            frm.MdiParent = this.MdiParent;
                            frm.Show();
                            BindOrderslist();
                        }
                    }
                    //FrmInv.ShowDialog();
                    //i1 = 0;
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
    }
}
