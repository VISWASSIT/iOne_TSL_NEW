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
using Newtonsoft.Json;
using System.Web.Script.Serialization;
using Syncfusion.WinForms.DataGridConverter;
using Syncfusion.WinForms.DataGrid.Enums;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;


using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using System.Net.Http;
//using TaxProEInvoice.API;
using System.Net.Http.Headers;
using ZXing;

namespace ioneNet.OrderManagement.Transactions
{
    public partial class ListOfInvoices : Form
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
        public string json;
        public string authToken;
        public static string Inv_NO_for_EInv;
        //public datetime authToken_valid;

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

                var irnno = (from c1 in db.Invoice_Masters
                             join s1 in db.Attributes_Datas on c1.Status equals s1.ID
                             where c1.Company_ID == logIn.company && c1.Inv_No == SO_No && s1.Descr == "IRN Generated"
                             select new
                             {
                                 c1.InvDate,
                                 c1.Einv_ACK_No,
                                 c1.Einv_ACK_Date,
                                 c1.EInv_IRN_No,
                                 c1.EInv_QR_Code,
                                 c1.WayBillNo,
                                 c1.Status,
                                 c1.Transporter_Name,
                                 c1.VehicleNo
                             }).ToList();
                if (irnno.Count > 0)
                {

                    txtQRCode.Text = irnno[0].EInv_QR_Code;

                }

                string QrCode = "";
                IBarcodeWriter writer = new BarcodeWriter { Format = BarcodeFormat.QR_CODE };

                if (txtQRCode.Text.Length > 955)
                {
                    QrCode = Mid(txtQRCode.Text, 1, 954);
                }
                else
                {
                    QrCode = txtQRCode.Text;
                }
                if (QrCode.Length > 0)
                {
                    var result = writer.Write(QrCode);


                    var barcodeBitmap = new Bitmap(result);
                    pictureBox1.Image = barcodeBitmap;
                    Image img = pictureBox1.Image;
                    MemoryStream ms = new MemoryStream();
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] bytes = ms.ToArray();

                    SqlCommand cmd5 = new SqlCommand("delete  from [temp_inv_QRCOde] where Company_ID =@comp", con);
                    cmd5.Parameters.AddWithValue("@comp", logIn.company);
                    cmd5.ExecuteNonQuery();

                    cmd1.CommandText = "INSERT INTO temp_inv_QRCOde  (Inv_No,Company_ID,QR_COde) VALUES  (@invNo1," + logIn.company + ",@Qrcode)";
                    //cmd1.CommandText =  "INSERT INTO dbo.temp_Inv_Copy ([Inv_No],[Copy_Name],[Copy_No],[Company_ID]) VALUES (@invNo,@Copy_Name,@Copy_No, @comp)";

                    //// SqlCommand command = new SqlCommand(query, db.Connection);
                    cmd1.Parameters.AddWithValue("@invNo1", SO_No);
                    cmd1.Parameters.AddWithValue("@Qrcode", bytes);
                    cmd1.ExecuteNonQuery();


                }



                SqlCommand cmd4 = new SqlCommand("delete  from [temp_Inv_Copy] where Company_ID =@comp", con);
                cmd4.Parameters.AddWithValue("@comp", logIn.company);
                cmd4.ExecuteNonQuery();

                

                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                 //Get Invoice Format Mapped to the Company
                var gstno = (from c in db.Company_Report_Formats
                             where c.Company_ID == logIn.company
                             select new { c.GSTInv_Format }).ToList();
                if (gstno.Count > 0)
                {
                    if (gstno[0].GSTInv_Format == "InvV")
                    {
                        //Takhi Drive
                        rep = new OrderManagement.Transactions.SaleInvoice_GST_Oth_WithLogo();
                        cmd1.CommandText = "INSERT INTO temp_Inv_Copy  (Inv_No, Copy_Name, Copy_No,Company_ID) VALUES  (@invNo, 'Original for Receipent', '1'," + logIn.company + "),   (@invNo, 'Duplicate for Transporter / Supplier', '2'," + logIn.company + "),   (@invNo, 'Triplicate for Supplier', '3'," + logIn.company + ")";
                        //cmd1.CommandText =  "INSERT INTO dbo.temp_Inv_Copy ([Inv_No],[Copy_Name],[Copy_No],[Company_ID]) VALUES (@invNo,@Copy_Name,@Copy_No, @comp)";

                        //// SqlCommand command = new SqlCommand(query, db.Connection);
                        cmd1.Parameters.AddWithValue("@invNo", SO_No);

                        cmd1.ExecuteNonQuery();
                    }
                    
                    
                    else
                    if (gstno[0].GSTInv_Format == "InvV3")
                    {
                        //Vikas Castings
                        rep = new OrderManagement.Transactions.SaleInvoice_GST_WithLogBig();
                        if(logIn.company ==1047 || logIn.company == 1046)
                        {
                            cmd1.CommandText = "INSERT INTO temp_Inv_Copy  (Inv_No, Copy_Name, Copy_No,Company_ID) VALUES  (@invNo, 'Original for Receipent', '1'," + logIn.company + "),   (@invNo, 'Duplicate for Transporter / Supplier', '2'," + logIn.company + ")";

                        }
                        else
                        {
                            cmd1.CommandText = "INSERT INTO temp_Inv_Copy  (Inv_No, Copy_Name, Copy_No,Company_ID) VALUES  (@invNo, 'Original for Receipent', '1'," + logIn.company + "),   (@invNo, 'Duplicate for Transporter / Supplier', '2'," + logIn.company + "),   (@invNo, 'Triplicate for Supplier', '3'," + logIn.company + ")";

                        }
                        //cmd1.CommandText =  "INSERT INTO dbo.temp_Inv_Copy ([Inv_No],[Copy_Name],[Copy_No],[Company_ID]) VALUES (@invNo,@Copy_Name,@Copy_No, @comp)";

                        //// SqlCommand command = new SqlCommand(query, db.Connection);
                        cmd1.Parameters.AddWithValue("@invNo", SO_No);
            

                        cmd1.ExecuteNonQuery();
                    }
                    
                    else
                    {
                        rep = new OrderManagement.Transactions.SaleInvoice_GST_Oth();
                        cmd1.CommandText = "INSERT INTO temp_Inv_Copy  (Inv_No, Copy_Name, Copy_No,Company_ID) VALUES  (@invNo, 'Original for Receipent', '1'," + logIn.company + "),   (@invNo, 'Duplicate for Transporter / Supplier', '2'," + logIn.company + "),   (@invNo, 'Triplicate for Supplier', '3'," + logIn.company + ")";
                        //cmd1.CommandText =  "INSERT INTO dbo.temp_Inv_Copy ([Inv_No],[Copy_Name],[Copy_No],[Company_ID]) VALUES (@invNo,@Copy_Name,@Copy_No, @comp)";

                        //// SqlCommand command = new SqlCommand(query, db.Connection);
                        cmd1.Parameters.AddWithValue("@invNo", SO_No);

                        cmd1.ExecuteNonQuery();

                    }

                }
                     
                else
                {
                    rep = new OrderManagement.Transactions.SaleInvoice_GST_Oth();
                    cmd1.CommandText = "INSERT INTO temp_Inv_Copy  (Inv_No, Copy_Name, Copy_No,Company_ID) VALUES  (@invNo, 'Original for Receipent', '1'," + logIn.company + "),   (@invNo, 'Duplicate for Transporter / Supplier', '2'," + logIn.company + "),   (@invNo, 'Triplicate for Supplier', '3'," + logIn.company + ")";
                    //cmd1.CommandText =  "INSERT INTO dbo.temp_Inv_Copy ([Inv_No],[Copy_Name],[Copy_No],[Company_ID]) VALUES (@invNo,@Copy_Name,@Copy_No, @comp)";

                    //// SqlCommand command = new SqlCommand(query, db.Connection);
                    cmd1.Parameters.AddWithValue("@invNo", SO_No);

                    cmd1.ExecuteNonQuery();

                }

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

                    var d = (from po in db.Invoice_Childs
                             join im in db.Invoice_Masters on po.Inv_Master_ID equals im.Id 
                             where im.Inv_No == SO_No && im.BU_ID == logIn.BU_ID 
                             select po).FirstOrDefault();
                    




                    var da1 = (from so in db.Invoice_Masters
                           
                               join c in db.Supplier_informations on so.ConsigneeName equals c.ID
                               where so.Inv_No == SO_No && so.BU_ID == logIn.BU_ID && so.Status!=24 
                               select new
                               {
                                   so.ConsigneeAddress,
                                   so.Con_GST_No,
                                   c.City
                               }).ToList();

                                        
                    if (da1.Count > 0)
                    {

                        //                ValidateJSON(ca[0].ConsigneeAddress);
                        if(Mid(da1[0].ConsigneeAddress, 3,4) == "Addr")
                        {
                            JObject jsoncancel = JObject.Parse(da1[0].ConsigneeAddress);

                            CAddr = (string)jsoncancel.SelectToken("Address1") + "," + (string)jsoncancel.SelectToken("Address2");
                            CCity = (string)jsoncancel.SelectToken("City") + "," + (string)jsoncancel.SelectToken("PinCode");
                            cState = (string)jsoncancel.SelectToken("State") + ", State Code : " + (string)jsoncancel.SelectToken("StateCode");
                            cGSTIN =  "GSTIN : " + (string)jsoncancel.SelectToken("GSTIN");

                        }
                        else
                        {
                            CAddr = da1[0].ConsigneeAddress;
                            cGSTIN = "GSTIN : " +  da1[0].Con_GST_No;
                            CCity = da1[0].City;
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
       

        private void SignWithThisCert(X509Certificate2 cert, string locationName)
        {

            string SourcePdfFileName = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.pdf");

            
            string DestPdfFileName = Path.Combine(Directory.GetCurrentDirectory(), "Invoice_signed.pdf");
            Org.BouncyCastle.X509.X509CertificateParser cp = new Org.BouncyCastle.X509.X509CertificateParser();
            Org.BouncyCastle.X509.X509Certificate[] chain = new Org.BouncyCastle.X509.X509Certificate[] { cp.ReadCertificate(cert.RawData) };
            IExternalSignature externalSignature = new X509Certificate2Signature(cert, "SHA-1");
            PdfReader pdfReader = new PdfReader(SourcePdfFileName);
            FileStream signedPdf = new FileStream(DestPdfFileName, FileMode.Create);  //the output pdf file
            PdfStamper pdfStamper = PdfStamper.CreateSignature(pdfReader, signedPdf, '\0');
            PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;
            //here set signatureAppearance at your will
            signatureAppearance.Reason = "I Approved This Document";
            //signatureAppearance.Location = lbSdmTehsilName.Text;  //To Entered it Later
            signatureAppearance.Location = locationName;
            signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;
            //For OBC
            signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(900, 80, 350, 220), 1, null);
            MakeSignature.SignDetached(signatureAppearance, externalSignature, chain, null, null, null, 0, CryptoStandard.CMS);
            //MakeSignature.SignDetached(signatureAppearance, externalSignature, chain, null, null, null, 0, CryptoStandard.CADES);


        ////////////////////////////
    }

private void printChallanToolStripMenuItem_Click(object sender, EventArgs e)
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
                var mappingName = sfDataGrid1.Columns[1].MappingName;
                var mappingName1= sfDataGrid1.Columns[7].MappingName;
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

                rep.SetParameterValue("INVNO", SO_No);
                rep.SetParameterValue("Slip_No", cellVaue1);
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

        private void printLabelsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OrderManagement.Transactions.frmInvoice_Label frm = new frmInvoice_Label();
            //frm.MdiParent = this.MdiParent;
            int i = sfDataGrid1.CurrentCell.RowIndex;
            var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
            var rowData = sfDataGrid1.GetRecordAtRowIndex(i);
            var mappingName = sfDataGrid1.Columns[1].MappingName;
            var mappingName1 = sfDataGrid1.Columns[3].MappingName;
            var mappingName2 = sfDataGrid1.Columns[3].MappingName;
            var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            var Cons = (rowData.GetType().GetProperty(mappingName1).GetValue(rowData, null).ToString());
            var cellVaue1 = (rowData.GetType().GetProperty(mappingName2).GetValue(rowData, null).ToString());
            SO_No = cellVaue.ToString();

            Consignee = Cons.ToString();
            frm.ShowDialog();
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

                                db.sp_GSTInv_Delete(SO_No, logIn.company,logIn.BU_ID);
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

        private async void eMailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {

                //call_server_api();

                //ioneNet.OrderManagement.Transactions.GenerateEInvoice frm = new GenerateEInvoice();
                //frm.MdiParent = this.MdiParent;
                //frm.Show();


                


                SendEmail();
                //MessageBox.Show("Email Sent Successfully");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
                

        public ListOfInvoices()
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
                    string CAddr = "";
                    string CCity = "";
                    string cState = "";
                    String cGSTIN = "";

                    var da1 = (from inv in db.Invoice_Childs
                               join so in db.Sale_Order_Masters

                               on new { A = inv.SO_Ref_No.Trim(), B = inv.Company_ID } equals new { A = so.SO_NO, B = so.Company_ID }
                               join c in db.Supplier_informations on so.ConsigneeName equals c.ID
                               where inv.Inv_No == SO_No && inv.Company_ID == logIn.company
                               select new
                               {
                                   so.Delivery_Address,
                                   so.Delivery_GSTIN,
                                   c.City
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
                            CCity = da1[0].City;
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

        private void printInvoiceWithDigitalSignToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                groupBox1.Visible = true;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
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

                var irnno = (from c1 in db.Invoice_Masters
                             join s1 in db.Attributes_Datas on c1.Status equals s1.ID
                             where c1.Company_ID == logIn.company && c1.Inv_No == SO_No && s1.Descr == "IRN Generated"
                             select new
                             {
                                 c1.InvDate,
                                 c1.Einv_ACK_No,
                                 c1.Einv_ACK_Date,
                                 c1.EInv_IRN_No,
                                 c1.EInv_QR_Code,
                                 c1.WayBillNo,
                                 c1.Status,
                                 c1.Transporter_Name,
                                 c1.VehicleNo
                             }).ToList();
                if (irnno.Count > 0)
                {
                    
                    txtQRCode.Text = irnno[0].EInv_QR_Code;
                  
                }

                string QrCode = "";
                IBarcodeWriter writer = new BarcodeWriter { Format = BarcodeFormat.QR_CODE };

                if (txtQRCode.Text.Length > 955)
                {
                    QrCode = Mid(txtQRCode.Text, 1, 954);
                }
                else
                {
                    QrCode = txtQRCode.Text;
                }
                var result = writer.Write(QrCode);


                var barcodeBitmap = new Bitmap(result);
                pictureBox1.Image = barcodeBitmap;
                Image img = pictureBox1.Image;
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] bytes = ms.ToArray();




                SqlCommand cmd4 = new SqlCommand("delete  from [temp_Inv_Copy] where Company_ID =@comp", con);
                cmd4.Parameters.AddWithValue("@comp", logIn.company);
                cmd4.ExecuteNonQuery();

                SqlCommand cmd5 = new SqlCommand("delete  from [temp_inv_QRCOde] where Company_ID =@comp", con);
                cmd5.Parameters.AddWithValue("@comp", logIn.company);
                cmd5.ExecuteNonQuery();

                cmd1.CommandText = "INSERT INTO temp_inv_QRCOde  (Inv_No,Company_ID,QR_COde) VALUES  (@invNo1," + logIn.company + ",@Qrcode)";
                //cmd1.CommandText =  "INSERT INTO dbo.temp_Inv_Copy ([Inv_No],[Copy_Name],[Copy_No],[Company_ID]) VALUES (@invNo,@Copy_Name,@Copy_No, @comp)";

                //// SqlCommand command = new SqlCommand(query, db.Connection);
                cmd1.Parameters.AddWithValue("@invNo1", SO_No);
                cmd1.Parameters.AddWithValue("@Qrcode", bytes);
                cmd1.ExecuteNonQuery();


                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                //Get Invoice Format Mapped to the Company
                var gstno = (from c in db.Company_Report_Formats
                             where c.Company_ID == logIn.company
                             select new { c.GSTInv_Format }).ToList();
                if (gstno.Count > 0)
                {
                    if (gstno[0].GSTInv_Format == "InvV")
                    {
                        //Takhi Drive
                        rep = new OrderManagement.Transactions.SaleInvoice_GST_Oth_WithLogo();
                        cmd1.CommandText = "INSERT INTO temp_Inv_Copy  (Inv_No, Copy_Name, Copy_No,Company_ID) VALUES  (@invNo, 'Original for Receipent', '1'," + logIn.company + "),   (@invNo, 'Duplicate for Transporter / Supplier', '2'," + logIn.company + "),   (@invNo, 'Triplicate for Supplier', '3'," + logIn.company + ")";
                        //cmd1.CommandText =  "INSERT INTO dbo.temp_Inv_Copy ([Inv_No],[Copy_Name],[Copy_No],[Company_ID]) VALUES (@invNo,@Copy_Name,@Copy_No, @comp)";

                        //// SqlCommand command = new SqlCommand(query, db.Connection);
                        cmd1.Parameters.AddWithValue("@invNo", SO_No);

                        cmd1.ExecuteNonQuery();
                    }
                    
                    else
                    if (gstno[0].GSTInv_Format == "InvV3")
                    {
                        //Vikas Castings
                        rep = new OrderManagement.Transactions.SaleInvoice_GST_WithLogBig();
                        cmd1.CommandText = "INSERT INTO temp_Inv_Copy  (Inv_No, Copy_Name, Copy_No,Company_ID) VALUES  (@invNo, 'Original for Receipent', '1'," + logIn.company + "),   (@invNo, 'Duplicate for Transporter / Supplier', '2'," + logIn.company + "),   (@invNo, 'Triplicate for Supplier', '3'," + logIn.company + ")";
                        //cmd1.CommandText =  "INSERT INTO dbo.temp_Inv_Copy ([Inv_No],[Copy_Name],[Copy_No],[Company_ID]) VALUES (@invNo,@Copy_Name,@Copy_No, @comp)";

                        //// SqlCommand command = new SqlCommand(query, db.Connection);
                        cmd1.Parameters.AddWithValue("@invNo", SO_No);

                        cmd1.ExecuteNonQuery();
                    }
                    
                    else
                    {
                        rep = new OrderManagement.Transactions.SaleInvoice_GST_Oth();
                        cmd1.CommandText = "INSERT INTO temp_Inv_Copy  (Inv_No, Copy_Name, Copy_No,Company_ID) VALUES  (@invNo, 'Original for Receipent', '1'," + logIn.company + "),   (@invNo, 'Duplicate for Transporter / Supplier', '2'," + logIn.company + "),   (@invNo, 'Triplicate for Supplier', '3'," + logIn.company + ")";
                        //cmd1.CommandText =  "INSERT INTO dbo.temp_Inv_Copy ([Inv_No],[Copy_Name],[Copy_No],[Company_ID]) VALUES (@invNo,@Copy_Name,@Copy_No, @comp)";

                        //// SqlCommand command = new SqlCommand(query, db.Connection);
                        cmd1.Parameters.AddWithValue("@invNo", SO_No);

                        cmd1.ExecuteNonQuery();

                    }

                }

                else
                {
                    rep = new OrderManagement.Transactions.SaleInvoice_GST_Oth();
                    cmd1.CommandText = "INSERT INTO temp_Inv_Copy  (Inv_No, Copy_Name, Copy_No,Company_ID) VALUES  (@invNo, 'Original for Receipent', '1'," + logIn.company + "),   (@invNo, 'Duplicate for Transporter / Supplier', '2'," + logIn.company + "),   (@invNo, 'Triplicate for Supplier', '3'," + logIn.company + ")";
                    //cmd1.CommandText =  "INSERT INTO dbo.temp_Inv_Copy ([Inv_No],[Copy_Name],[Copy_No],[Company_ID]) VALUES (@invNo,@Copy_Name,@Copy_No, @comp)";

                    //// SqlCommand command = new SqlCommand(query, db.Connection);
                    cmd1.Parameters.AddWithValue("@invNo", SO_No);

                    cmd1.ExecuteNonQuery();

                }




                path = Path.Combine(Directory.GetCurrentDirectory(), "Invoice.pdf");
                //string path = @"D:\Invoice.pdf";
                FileInfo fi1 = new FileInfo(path);
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
                    //CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                    ////Get Invoice Format Mapped to the Company
                    //var gstno = (from c in db.Company_Report_Formats
                    //             where c.Company_ID == logIn.company
                    //             select new { c.GSTInv_Format }).ToList();
                    //if (gstno.Count > 0)
                    //{
                    //    if (gstno[0].GSTInv_Format == "InvV")
                    //    {
                    //        //Takhi Drive
                    //        rep = new OrderManagement.Transactions.SaleInvoice_GST_Oth_WithLogo();
                           
                    //    }
                    //    else
                    //     if (gstno[0].GSTInv_Format == "InvV1")
                    //    {
                    //        //MPower
                    //        rep = new OrderManagement.Transactions.rptExciseInvoice_Header();
                           
                    //    }
                    //    else
                    //     if (gstno[0].GSTInv_Format == "InvV2")
                    //    {
                    //        //Yen Flexi
                    //        rep = new OrderManagement.Transactions.SaleInvoice_GST_Yen_e();
                            
                    //    }
                    //    else
                    //    if (gstno[0].GSTInv_Format == "InvV3")
                    //    {
                    //        //Vikas Castings
                    //        rep = new OrderManagement.Transactions.SaleInvoice_GST_WithLogBig();
                            
                    //    }
                    //    else
                    //     if (gstno[0].GSTInv_Format == "InvV4")
                    //    {
                    //        //KDS Entp
                    //        rep = new OrderManagement.Transactions.GST_Invoice();
                           


                    //    }
                    //    else
                    //     if (gstno[0].GSTInv_Format == "InvH")
                    //    {
                    //        //Nano
                    //        rep = new OrderManagement.Transactions.SaleInvoice_GST();
                            

                    //    }
                    //    else
                    //     if (gstno[0].GSTInv_Format == "Inv5")
                    //    {
                    //        //V Pack
                    //        rep = new OrderManagement.Transactions.rptExciseInvoice_vpack();
                            

                    //    }

                    //    else
                    //    {
                    //        rep = new OrderManagement.Transactions.SaleInvoice_GST_Oth();
                           

                    //    }

                    //}

                    //else
                    //{
                    //    rep = new OrderManagement.Transactions.SaleInvoice_GST_Oth();
                        

                    //}

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

                    var da1 = (from so in db.Invoice_Masters

                               join c in db.Supplier_informations on so.ConsigneeName equals c.ID
                               where so.Inv_No == SO_No && so.BU_ID == logIn.BU_ID && so.Status != 24
                               select new
                               {
                                   so.ConsigneeAddress,
                                   so.Con_GST_No,
                                   c.City
                               }).ToList();


                    if (da1.Count > 0)
                    {
                        //                ValidateJSON(ca[0].ConsigneeAddress);
                        if (Mid(da1[0].ConsigneeAddress, 3, 4) == "Addr")
                        {
                            JObject jsoncancel = JObject.Parse(da1[0].ConsigneeAddress);

                            CAddr = (string)jsoncancel.SelectToken("Address1") + "," + (string)jsoncancel.SelectToken("Address2");
                            CCity = (string)jsoncancel.SelectToken("City") + "," + (string)jsoncancel.SelectToken("PinCode");
                            cState = (string)jsoncancel.SelectToken("State") + ", State Code : " + (string)jsoncancel.SelectToken("StateCode");
                            cGSTIN = "GSTIN : " + (string)jsoncancel.SelectToken("GSTIN");

                        }
                        else
                        {
                            CAddr = da1[0].ConsigneeAddress;
                            cGSTIN = "GSTIN : " + da1[0].Con_GST_No;
                            CCity = da1[0].City;
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
                    // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                    viewer.crystalReportViewer1.ReportSource = rep;
                    viewer.crystalReportViewer1.Refresh();
                    rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                    cmd.Parameters.Clear();
                }
                con.Close();


                //Get Certifictae
                X509CertificateParser cp = new X509CertificateParser();

                //Get Sertifiacte
                X509Certificate2 certClient = null;
                X509Store st = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                st.Open(OpenFlags.MaxAllowed);
                X509Certificate2Collection collection = X509Certificate2UI.SelectFromCollection(st.Certificates,
                    "Please choose certificate:", "", X509SelectionFlag.SingleSelection);
                if (collection.Count > 0)
                {
                    certClient = collection[0];
                }
                st.Close();
                //Get Cert Chain
                IList<X509Certificate> chain = new List<X509Certificate>();
                X509Chain x509Chain = new X509Chain();

                x509Chain.Build(certClient);

                foreach (X509ChainElement x509ChainElement in x509Chain.ChainElements)
                {
                    chain.Add(Org.BouncyCastle.Security.DotNetUtilities.FromX509Certificate(x509ChainElement.Certificate));
                }

                PdfReader inputPdf = new PdfReader(path);
                string path1 = Path.Combine(Directory.GetCurrentDirectory(), "Invoice_Signed.pdf");
                FileStream signedPdf = new FileStream(path1, FileMode.Create);

                PdfStamper pdfStamper = PdfStamper.CreateSignature(inputPdf, signedPdf, '\0');

                IExternalSignature externalSignature = new X509Certificate2Signature(certClient, "SHA-1");

                PdfSignatureAppearance signatureAppearance = pdfStamper.SignatureAppearance;
                signatureAppearance.Reason = "I Approved This Document";
                if(logIn.company == 11 )
                {
                    signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(635, 60, 401, 102), 1, null);

                }
                else
                {
                    signatureAppearance.SetVisibleSignature(new iTextSharp.text.Rectangle(635, 63, 385, 102), 1, null);

                }
                signatureAppearance.SignatureRenderingMode = PdfSignatureAppearance.RenderingMode.DESCRIPTION;

                MakeSignature.SignDetached(signatureAppearance, externalSignature, chain, null, null, null, 0,
                    CryptoStandard.CMS);
                inputPdf.Close();
                pdfStamper.Close();
                Process.Start(path1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            if(comboBox1.Text == "Original for Receipent")
            {
                textBox1.Text = "1";
            }
            if (comboBox1.Text == "Duplicate for Transporter / Supplier")
            {
                textBox1.Text = "2";
            }
            if (comboBox1.Text == "Triplicate for Supplier")
            {
                textBox1.Text = "3";
            }

            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            groupBox1.Visible = false;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void accountPostingToolStripMenuItem_Click(object sender, EventArgs e)
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

                            DialogResult result = MessageBox.Show("Are You Sure To Cancel The Record? Cannot Undo This Operation!", "Cancel Confirmation", MessageBoxButtons.YesNo);
                            if (result == DialogResult.Yes)
                            {
                                SO_No = cellVaue.ToString();

                                var ci = db.Invoice_Masters.Where(w => w.Inv_No == SO_No && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                                {
                                    ci.Status = 25;
                                    ci.Modified_By = logIn.username + "-" + DateTime.Now;
                                    db.SubmitChanges();
                                    BindOrderslist();
                                }
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

        private void generateEInvoiceJSONToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {



                int i = 0;
                int R = sfDataGrid1.CurrentCell.RowIndex;
                // int k = sfDataGrid1.CurrentCell.RowIndex;
                var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
                var rowData = sfDataGrid1.GetRecordAtRowIndex(R);
                var mappingName = sfDataGrid1.Columns[1].MappingName;
                var mappingName1 = sfDataGrid1.Columns[4].MappingName;
                var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
                string myString = cellVaue.ToString();
                Inv_NO_for_EInv = myString;
                ioneNet.OrderManagement.Transactions.GenerateEInvoice frm = new GenerateEInvoice();
                frm.MdiParent = this.MdiParent;
                frm.Show();

                //var comp = (from c in db.Company_Infos where c.Id == logIn.company select c).ToList();

                //var da = (from inv in db.Invoice_Masters
                //          where inv.Inv_No == myString && inv.Company_ID == logIn.company
                //          select inv).ToList();

                //var cust = (from d in db.Supplier_informations where d.ID == da[0].BuyerName select d).ToList();

                //var con = (from d in db.Supplier_informations where d.ID == da[0].ConsigneeName select d).ToList();

                //var da1 = (from inv in db.Invoice_Childs
                //           where inv.Inv_Master_ID == da[0].Id && inv.Company_ID == logIn.company
                //           select inv).ToList();

                //ReqPlGenIRN reqPlGenIRN = new ReqPlGenIRN();
                //reqPlGenIRN.Version = "1.1";
                //reqPlGenIRN.TranDtls = new ReqPlGenIRN.TranDetails();
                //reqPlGenIRN.TranDtls.TaxSch = "GST";
                //reqPlGenIRN.TranDtls.SupTyp = "B2B";
                //reqPlGenIRN.TranDtls.IgstOnIntra = "N";
                //reqPlGenIRN.TranDtls.RegRev = "N";
                //reqPlGenIRN.TranDtls.EcmGstin = null;


                //reqPlGenIRN.DocDtls = new ReqPlGenIRN.DocSetails();
                //reqPlGenIRN.DocDtls.Typ = "INV";
                //reqPlGenIRN.DocDtls.No = da[0].Inv_No;
                //DateTime dt = da[0].InvDate.Value;
                //string dt1 = dt.ToString("dd/MM/yyyy");
                //reqPlGenIRN.DocDtls.Dt = dt1;
                //reqPlGenIRN.SellerDtls = new ReqPlGenIRN.SellerDetails();
                //reqPlGenIRN.SellerDtls.Gstin = "29AABCT1332L000"; //comp[0].GST_No;
                //reqPlGenIRN.SellerDtls.LglNm = logIn.compname;
                //reqPlGenIRN.SellerDtls.TrdNm = logIn.compname;
                //string A1 = null;
                //string A2 = null;

                ////if (comp[0].Address.Length>100)
                ////{
                ////    int Alen = comp[0].Address.Length;
                ////    A1 = Mid(comp[0].Address, 1, 100);
                ////    A2 = Mid(comp[0].Address, 101, Alen-1);
                ////}
                ////else
                ////{
                //A1 = comp[0].Address;
                ////}
                //reqPlGenIRN.SellerDtls.Addr1 = A1;
                //reqPlGenIRN.SellerDtls.Addr2 = A2;
                //reqPlGenIRN.SellerDtls.Loc = comp[0].City;
                //reqPlGenIRN.SellerDtls.Pin = 560001;
                //reqPlGenIRN.SellerDtls.Stcd = "29"; // comp[0].State_Code;
                //reqPlGenIRN.SellerDtls.Ph = comp[0].Phone_No;
                //reqPlGenIRN.SellerDtls.Em = comp[0].E_Mail;



                //reqPlGenIRN.BuyerDtls = new ReqPlGenIRN.BuyerDetails();
                //reqPlGenIRN.BuyerDtls.Gstin = cust[0].GSTIN_NO;
                //reqPlGenIRN.BuyerDtls.LglNm = cust[0].Supplier_Alias_Name;
                //reqPlGenIRN.BuyerDtls.TrdNm = null;
                //reqPlGenIRN.BuyerDtls.Addr1 = cust[0].Address_1;
                //if (cust[0].Address_2 != "")
                //{
                //    reqPlGenIRN.BuyerDtls.Addr2 = cust[0].Address_2;
                //}
                //else
                //{
                //    reqPlGenIRN.BuyerDtls.Addr2 = null;
                //}
                //reqPlGenIRN.BuyerDtls.Loc = cust[0].City;

                //reqPlGenIRN.BuyerDtls.Pin = Convert.ToInt32(cust[0].Pincode);
                //reqPlGenIRN.BuyerDtls.Pos = cust[0].StateCode;
                //reqPlGenIRN.BuyerDtls.Stcd = cust[0].StateCode;
                //reqPlGenIRN.BuyerDtls.Ph = null;
                //reqPlGenIRN.BuyerDtls.Em = null;

                //reqPlGenIRN.DispDtls = null;
                ////    new ReqPlGenIRN.DispatchedDetails();                
                ////reqPlGenIRN.DispDtls.Nm = con[0].Supplier_Alias_Name;
                ////reqPlGenIRN.DispDtls.Addr1 = con[0].Address_1;
                ////if (con[0].Address_2 != "")
                ////{
                ////    reqPlGenIRN.DispDtls.Addr2 = con[0].Address_2;
                ////}
                ////else
                ////{
                ////    reqPlGenIRN.DispDtls.Addr2 = null;
                ////}

                ////reqPlGenIRN.DispDtls.Loc = con[0].City;
                ////reqPlGenIRN.DispDtls.Pin = Convert.ToInt32(con[0].Pincode);
                ////reqPlGenIRN.DispDtls.Stcd = con[0].StateCode;

                //reqPlGenIRN.ShipDtls = new ReqPlGenIRN.ShippedDetails();
                //reqPlGenIRN.ShipDtls.Gstin = con[0].GSTIN_NO;
                //reqPlGenIRN.ShipDtls.LglNm = con[0].Supplier_Alias_Name;
                //reqPlGenIRN.ShipDtls.TrdNm = null;
                //reqPlGenIRN.ShipDtls.Addr1 = con[0].Address_1;
                //if (con[0].Address_2 != "")
                //{
                //    reqPlGenIRN.ShipDtls.Addr2 = con[0].Address_2;
                //}
                //else
                //{
                //    reqPlGenIRN.ShipDtls.Addr2 = null;
                //}

                //reqPlGenIRN.ShipDtls.Loc = con[0].City;
                //reqPlGenIRN.ShipDtls.Pin = Convert.ToInt32(con[0].Pincode);
                //reqPlGenIRN.ShipDtls.Stcd = con[0].StateCode;

                //reqPlGenIRN.ItemList = new List<ReqPlGenIRN.ItmList>();

                //foreach (var ItemNo in da1)
                //{
                //    var prod = (from p in db.Sale_Order_Childs where p.SO_NO == ItemNo.SO_Ref_No && p.Prod_Code == ItemNo.Prod_Code select p).ToList();
                //    ReqPlGenIRN.ItmList itm = new ReqPlGenIRN.ItmList();
                //    itm.SlNo = (i + 1).ToString();
                //    itm.IsServc = "N";
                //    itm.PrdDesc = ItemNo.Product_Description.Trim();
                //    itm.HsnCd = prod[0].HSN_Code;
                //    itm.BchDtls = null;
                //    itm.Qty = Convert.ToDouble(ItemNo.Qty);
                //    itm.Unit = da1[0].Uom.Trim();
                //    itm.UnitPrice = Convert.ToDouble(ItemNo.Price);
                //    itm.TotAmt = Convert.ToDouble(ItemNo.Amount);
                //    itm.Discount = Convert.ToDouble(ItemNo.Disc_Amount);
                //    itm.AssAmt = Convert.ToDouble(ItemNo.Taxable_Value);
                //    itm.GstRt = Convert.ToDouble(ItemNo.CGST_Per + ItemNo.SGST_Per + ItemNo.IGST_Per);
                //    itm.SgstAmt = Convert.ToDouble(ItemNo.SGST_Amnt);
                //    itm.IgstAmt = Convert.ToDouble(ItemNo.IGST_Amnt);
                //    itm.CgstAmt = Convert.ToDouble(ItemNo.CGST_Amnt);
                //    itm.CesRt = 0.0;
                //    itm.CesAmt = 0.0;
                //    itm.CesNonAdvlAmt = 0.0;
                //    itm.StateCesRt = 0.0;
                //    itm.StateCesAmt = 0.0;
                //    itm.StateCesNonAdvlAmt = 0.0;
                //    itm.OthChrg = 0.0;
                //    itm.TotItemVal = Convert.ToDouble(ItemNo.Net_Amount);
                //    itm.AttribDtls = null;
                //    reqPlGenIRN.ItemList.Add(itm);

                //    i = i + 1;
                //}

                //reqPlGenIRN.PayDtls = null;
                //reqPlGenIRN.RefDtls = null;
                //reqPlGenIRN.AddlDocDtls = null;
                //reqPlGenIRN.ExpDtls = null;

                //reqPlGenIRN.EwbDtls = new ReqPlGenIRN.EwbDetails();
                //reqPlGenIRN.EwbDtls.TransId = null;
                //reqPlGenIRN.EwbDtls.TransName = da[0].Transporter_Name;
                //reqPlGenIRN.EwbDtls.TransMode = "1";
                //reqPlGenIRN.EwbDtls.Distance = 80;
                //reqPlGenIRN.EwbDtls.TransDocNo = null;
                //reqPlGenIRN.EwbDtls.TransDocDt = dt1;
                //reqPlGenIRN.EwbDtls.VehNo = da[0].VehicleNo;
                //reqPlGenIRN.EwbDtls.VehType = "R";

                //reqPlGenIRN.ValDtls = new ReqPlGenIRN.ValDetails();
                //reqPlGenIRN.ValDtls.AssVal = Convert.ToDouble(da[0].Tot_TaxableValue);
                //reqPlGenIRN.ValDtls.CgstVal = Convert.ToDouble(da[0].Tot_CGST_Amnt);
                //reqPlGenIRN.ValDtls.SgstVal = Convert.ToDouble(da[0].Tot_SGST_Amnt);
                //reqPlGenIRN.ValDtls.IgstVal = Convert.ToDouble(da[0].Tot_IGST_Amnt);
                //reqPlGenIRN.ValDtls.CesVal = 0.0;
                //reqPlGenIRN.ValDtls.StCesVal = 0.0;
                //reqPlGenIRN.ValDtls.OthChrg = Convert.ToDouble(da[0].TCS_Amnt);
                //reqPlGenIRN.ValDtls.RndOffAmt = Convert.ToDouble(da[0].Rounding);
                //reqPlGenIRN.ValDtls.TotInvVal = Convert.ToDouble(da[0].Tot_Inv_Value); ;


                //JavaScriptSerializer serializer = new JavaScriptSerializer();

                //json = JsonConvert.SerializeObject(reqPlGenIRN, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
                //{
                //    NullValueHandling = NullValueHandling.Ignore
                //});

                ////serializer.MaxJsonLength = 1024 * 1024 * 100;// Needed to increases if JSOn is longer than MaxJsonLength
                ////string json = serializer.Serialize(obj);

                //Console.WriteLine(json);
                //Console.ReadLine();
                //string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
                //System.IO.File.WriteAllText(Fname + "\\E_IIvoice" + ".json", json);

                //getAuthToken();

                //getIRNNO();



                //MessageBox.Show("Json File Created and Saved in My Documents Folder");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public async void getIRNNO()
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.mastergst.com/einvoice/type/GENERATE/version/V1_03?email=ssits_hyd%40gmail.com"))
                {
                    // Replace with your authorization code
                    request.Headers.Add("ip_address", "106.51.52.121");
                    request.Headers.Add("client_id", "b9713055-df1f-458f-932e-39828a8a1bac");
                    request.Headers.Add("client_secret", "97d80158-0b01-4b10-a33c-2e67c4b15f54");
                    request.Headers.Add("username", "mastergst");
                    request.Headers.Add("auth-token", authToken);
                    request.Headers.Add("gstin", "29AABCT1332L000");
                    // request.Headers.TryAddWithoutValidation("Token", "tZEbFSTsPZf039fy1LowtXZyy");

                    // Replace with your E-Invoice JSON data

                    var json1 = JsonConvert.SerializeObject(json);
                    var data = new StringContent(json1, Encoding.UTF8, "application/json");
                    request.Content = data;// ("{ \"SellerDtls\": {\"Gstin\": \"27AADCG4992P1ZT\"} }");
                    request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                    string url11 = "https://api.mastergst.com/einvoice/type/GENERATE/version/V1_03?email=ssits_hyd%40gmail.com";
                    var response = await httpClient.PostAsync(url11, data);

                        if (response.IsSuccessStatusCode)
                        {
                            var customerJsonString = await response.Content.ReadAsStringAsync();                        

                            //var cust = JsonConvert.DeserializeObject<Response>(customerJsonString);
                    }
                    else
                        {
                            Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                        }
                    
                    //  System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream());
                    // string streamtext = myreader.ReadToEnd();



                }
            }

        }
        public void getAuthToken()
        {
            string key = "xxx";
            string secKey = "yyy";
            //DateTime.UtcNow nonse = DateTime(1970,1,1,0,0,0).to
            string url11 = "https://api.mastergst.com/einvoice/authenticate?email=ssits_hyd%40gmail.com";
            string sign = "0";
            //Uri ourURL = Uri(url11);
            WebRequest request = WebRequest.Create(url11);


            WebResponse myResponse;
            request.Method = "GET";
            request.Headers.Add("username", "mastergst");
            request.Headers.Add("password", "Malli#123");
            request.Headers.Add("ip_address", "106.51.52.121");
            request.Headers.Add("client_id", "b9713055-df1f-458f-932e-39828a8a1bac");
            request.Headers.Add("client_secret", "97d80158-0b01-4b10-a33c-2e67c4b15f54");
            request.Headers.Add("gstin", "29AABCT1332L000");

            
           
            // request.Headers.Add("Content-Type", "application/json; charset=utf-8");
            //request. = DataFormat.Json;


            myResponse = request.GetResponse();

            System.IO.StreamReader myreader = new System.IO.StreamReader(myResponse.GetResponseStream());
            string streamtext = myreader.ReadToEnd();
            //MessageBox.Show(streamtext);
            JObject json = JObject.Parse(streamtext);
            authToken = (string)json.SelectToken("data.AuthToken");
            var authToken_valid = (string)json.SelectToken("data.TokenExpiry");
        }
        public static string Mid(string s, int a, int b)
        {
            string temp = s.Substring(a - 1, b);
            return temp;
        }
        public void SendEmail()
        {

            try
            {

                string filepath = "";
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

                var irnno = (from c1 in db.Invoice_Masters
                             join s1 in db.Attributes_Datas on c1.Status equals s1.ID
                             where c1.Company_ID == logIn.company && c1.Inv_No == SO_No && s1.Descr == "IRN Generated"
                             select new
                             {
                                 c1.InvDate,
                                 c1.Einv_ACK_No,
                                 c1.Einv_ACK_Date,
                                 c1.EInv_IRN_No,
                                 c1.EInv_QR_Code,
                                 c1.WayBillNo,
                                 c1.Status,
                                 c1.Transporter_Name,
                                 c1.VehicleNo
                             }).ToList();
                if (irnno.Count > 0)
                {

                    txtQRCode.Text = irnno[0].EInv_QR_Code;

                }

                string QrCode = "";
                IBarcodeWriter writer = new BarcodeWriter { Format = BarcodeFormat.QR_CODE };

                if (txtQRCode.Text.Length > 955)
                {
                    QrCode = Mid(txtQRCode.Text, 1, 954);
                }
                else
                {
                    QrCode = txtQRCode.Text;
                }
                if (QrCode.Length > 0)
                {
                    var result = writer.Write(QrCode);


                    var barcodeBitmap = new Bitmap(result);
                    pictureBox1.Image = barcodeBitmap;
                    Image img = pictureBox1.Image;
                    MemoryStream ms = new MemoryStream();
                    img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                    byte[] bytes = ms.ToArray();

                    SqlCommand cmd5 = new SqlCommand("delete  from [temp_inv_QRCOde] where Company_ID =@comp", con);
                    cmd5.Parameters.AddWithValue("@comp", logIn.company);
                    cmd5.ExecuteNonQuery();

                    cmd1.CommandText = "INSERT INTO temp_inv_QRCOde  (Inv_No,Company_ID,QR_COde) VALUES  (@invNo1," + logIn.company + ",@Qrcode)";
                    //cmd1.CommandText =  "INSERT INTO dbo.temp_Inv_Copy ([Inv_No],[Copy_Name],[Copy_No],[Company_ID]) VALUES (@invNo,@Copy_Name,@Copy_No, @comp)";

                    //// SqlCommand command = new SqlCommand(query, db.Connection);
                    cmd1.Parameters.AddWithValue("@invNo1", SO_No);
                    cmd1.Parameters.AddWithValue("@Qrcode", bytes);
                    cmd1.ExecuteNonQuery();


                }



                SqlCommand cmd4 = new SqlCommand("delete  from [temp_Inv_Copy] where Company_ID =@comp", con);
                cmd4.Parameters.AddWithValue("@comp", logIn.company);
                cmd4.ExecuteNonQuery();



                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                //Get Invoice Format Mapped to the Company
                var gstno = (from c in db.Company_Report_Formats
                             where c.Company_ID == logIn.company
                             select new { c.GSTInv_Format }).ToList();
                if (gstno.Count > 0)
                {
                    if (gstno[0].GSTInv_Format == "InvV")
                    {
                        //Takhi Drive
                        rep = new OrderManagement.Transactions.SaleInvoice_GST_Oth_WithLogo();
                        cmd1.CommandText = "INSERT INTO temp_Inv_Copy  (Inv_No, Copy_Name, Copy_No,Company_ID) VALUES  (@invNo, 'Original for Receipent', '1'," + logIn.company + "),   (@invNo, 'Duplicate for Transporter / Supplier', '2'," + logIn.company + "),   (@invNo, 'Triplicate for Supplier', '3'," + logIn.company + ")";
                        //cmd1.CommandText =  "INSERT INTO dbo.temp_Inv_Copy ([Inv_No],[Copy_Name],[Copy_No],[Company_ID]) VALUES (@invNo,@Copy_Name,@Copy_No, @comp)";

                        //// SqlCommand command = new SqlCommand(query, db.Connection);
                        cmd1.Parameters.AddWithValue("@invNo", SO_No);

                        cmd1.ExecuteNonQuery();
                    }


                    else
                    if (gstno[0].GSTInv_Format == "InvV3")
                    {
                        //Vikas Castings
                        rep = new OrderManagement.Transactions.SaleInvoice_GST_WithLogBig();
                        if (logIn.company == 1047 || logIn.company == 1046)
                        {
                            cmd1.CommandText = "INSERT INTO temp_Inv_Copy  (Inv_No, Copy_Name, Copy_No,Company_ID) VALUES  (@invNo, 'Original for Receipent', '1'," + logIn.company + "),   (@invNo, 'Duplicate for Transporter / Supplier', '2'," + logIn.company + ")";

                        }
                        else
                        {
                            cmd1.CommandText = "INSERT INTO temp_Inv_Copy  (Inv_No, Copy_Name, Copy_No,Company_ID) VALUES  (@invNo, 'Original for Receipent', '1'," + logIn.company + "),   (@invNo, 'Duplicate for Transporter / Supplier', '2'," + logIn.company + "),   (@invNo, 'Triplicate for Supplier', '3'," + logIn.company + ")";

                        }
                        //cmd1.CommandText =  "INSERT INTO dbo.temp_Inv_Copy ([Inv_No],[Copy_Name],[Copy_No],[Company_ID]) VALUES (@invNo,@Copy_Name,@Copy_No, @comp)";

                        //// SqlCommand command = new SqlCommand(query, db.Connection);
                        cmd1.Parameters.AddWithValue("@invNo", SO_No);


                        cmd1.ExecuteNonQuery();
                    }

                    else
                    {
                        rep = new OrderManagement.Transactions.SaleInvoice_GST_Oth();
                        cmd1.CommandText = "INSERT INTO temp_Inv_Copy  (Inv_No, Copy_Name, Copy_No,Company_ID) VALUES  (@invNo, 'Original for Receipent', '1'," + logIn.company + "),   (@invNo, 'Duplicate for Transporter / Supplier', '2'," + logIn.company + "),   (@invNo, 'Triplicate for Supplier', '3'," + logIn.company + ")";
                        //cmd1.CommandText =  "INSERT INTO dbo.temp_Inv_Copy ([Inv_No],[Copy_Name],[Copy_No],[Company_ID]) VALUES (@invNo,@Copy_Name,@Copy_No, @comp)";

                        //// SqlCommand command = new SqlCommand(query, db.Connection);
                        cmd1.Parameters.AddWithValue("@invNo", SO_No);

                        cmd1.ExecuteNonQuery();

                    }

                }

                else
                {
                    rep = new OrderManagement.Transactions.SaleInvoice_GST_Oth();
                    cmd1.CommandText = "INSERT INTO temp_Inv_Copy  (Inv_No, Copy_Name, Copy_No,Company_ID) VALUES  (@invNo, 'Original for Receipent', '1'," + logIn.company + "),   (@invNo, 'Duplicate for Transporter / Supplier', '2'," + logIn.company + "),   (@invNo, 'Triplicate for Supplier', '3'," + logIn.company + ")";
                    //cmd1.CommandText =  "INSERT INTO dbo.temp_Inv_Copy ([Inv_No],[Copy_Name],[Copy_No],[Company_ID]) VALUES (@invNo,@Copy_Name,@Copy_No, @comp)";

                    //// SqlCommand command = new SqlCommand(query, db.Connection);
                    cmd1.Parameters.AddWithValue("@invNo", SO_No);

                    cmd1.ExecuteNonQuery();

                }

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

                    var d = (from po in db.Invoice_Childs
                             join im in db.Invoice_Masters on po.Inv_Master_ID equals im.Id
                             where im.Inv_No == SO_No && im.BU_ID == logIn.BU_ID
                             select po).FirstOrDefault();





                    var da1 = (from so in db.Invoice_Masters

                               join c in db.Supplier_informations on so.ConsigneeName equals c.ID
                               where so.Inv_No == SO_No && so.BU_ID == logIn.BU_ID && so.Status != 24
                               select new
                               {
                                   so.ConsigneeAddress,
                                   so.Con_GST_No,
                                   c.City
                               }).ToList();


                    if (da1.Count > 0)
                    {

                        //                ValidateJSON(ca[0].ConsigneeAddress);
                        if (Mid(da1[0].ConsigneeAddress, 3, 4) == "Addr")
                        {
                            JObject jsoncancel = JObject.Parse(da1[0].ConsigneeAddress);

                            CAddr = (string)jsoncancel.SelectToken("Address1") + "," + (string)jsoncancel.SelectToken("Address2");
                            CCity = (string)jsoncancel.SelectToken("City") + "," + (string)jsoncancel.SelectToken("PinCode");
                            cState = (string)jsoncancel.SelectToken("State") + ", State Code : " + (string)jsoncancel.SelectToken("StateCode");
                            cGSTIN = "GSTIN : " + (string)jsoncancel.SelectToken("GSTIN");

                        }
                        else
                        {
                            CAddr = da1[0].ConsigneeAddress;
                            cGSTIN = "GSTIN : " + da1[0].Con_GST_No;
                            CCity = da1[0].City;
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


                    //rep.SetParameterValue("Invoice_No", SO_No);
                    //rep.SetParameterValue("Creation_Company", logIn.company);
                  
                    // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                    viewer.crystalReportViewer1.ReportSource = rep;
                    viewer.crystalReportViewer1.Refresh();
                    rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);

                    cmd.Parameters.Clear();
                    //Process.Start(path);
                }
                con.Close();

                filepath = path;


                var Email = (from c in db.Invoice_Masters
                             join inv in db.Supplier_informations
                             on c.BuyerName equals inv.ID
                             join em in db.EMailServerSettings on c.Company_ID equals em.company_ID
                             where c.Company_ID == logIn.company && c.Inv_No == SO_No
                             select new { inv.Email_Id, em.SmtpServer, em.SmptPort, em.POMailID, em.POMailPW,em.Default_CC_Mail_id }).ToList();
                string email = "viswanathk@sujana.com"; // Email[0].Email_Id;
                
                if (email == "" || email == null)
                {
                    MessageBox.Show("Customer Email Not Found In the Records");
                    return;
                }
                // string email = Email[0].Cust_Eail;
                MailMessage mm = new MailMessage();
                mm.From = new MailAddress(Email[0].POMailID);

                mm.To.Add(email);
                if (Email[0].Default_CC_Mail_id != null)
                {
                    mm.CC.Add(Email[0].Default_CC_Mail_id);
                }
                mm.Subject = "Invoice No :" + cellVaue;
                mm.Body = "Dear Sir," + "\n" + "Please Find Attached Invoice for Your information";
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
                //var ci = db.Purchase_Order_Masters.Where(w => w.PO_NO == cellVaue && w.Company_ID == logIn.company).FirstOrDefault();
                //{
                //    ci.email_Sent = true;
                //    db.SubmitChanges();
                //}
                MessageBox.Show("Invoice Sent by Email Successfully");
            }
            catch (Exception Ex)
            {
                MessageBox.Show(Ex.Message); 
            }
            //MessageBox.Show("Mail Send Success");
        }
        public void BindOrderslist()
        {
            try
            {
                var d = (from data in db.ShowInvList(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, null,logIn.BU_ID) select data).ToList();

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
            ioneNet.OrderManagement.Transactions.frmNewInvoice frm = new frmNewInvoice();
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
               // int k = sfDataGrid1.CurrentCell.RowIndex;
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
                            OrderManagement.Transactions.frmNewInvoice frm = new frmNewInvoice();
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

       
        private void btnImport_Click(object sender, EventArgs e)
        {
            var options = new ExcelExportingOptions();
            options.StartRowIndex = 5;
            var excelEngine = sfDataGrid1.ExportToExcel(sfDataGrid1.View, options);
            var workBook = excelEngine.Excel.Workbooks[0];
            var ws = excelEngine.Excel.Worksheets[1];
            workBook.Worksheets[0].Range["A3:R100"].AutofitColumns();
            workBook.Worksheets[0].Range["A1"].Value = logIn.compname;
            workBook.Worksheets[0].Range["A2"].Value = "Invoice List";
            workBook.Worksheets[0].PageSetup.Orientation = Syncfusion.XlsIO.ExcelPageOrientation.Landscape;
            workBook.Worksheets[0].PageSetup.TopMargin = 0.5;
            workBook.Worksheets[0].PageSetup.BottomMargin = 0.5;
            workBook.Worksheets[0].PageSetup.RightMargin = 0.5;
            workBook.Worksheets[0].PageSetup.LeftMargin = 0.5;
            workBook.Worksheets[0].PageSetup.Zoom = 85;
            workBook.Worksheets[0].PageSetup.PrintGridlines = true;
            string Fname = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            workBook.SaveAs(Fname + "\\Invoices_List.xlsx");
            string doc = Fname + "\\Invoices_List.xlsx";
            Process prc = new Process();
            prc.StartInfo.FileName = doc;
            prc.Start();
        }
                
    }
}
