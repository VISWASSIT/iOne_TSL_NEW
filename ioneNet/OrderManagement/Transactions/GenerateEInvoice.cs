using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using System.IO;
using System.Net;
using Ione_DAL;
using System.Security.Cryptography;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using TaxProEInvoice.API;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Script.Serialization;
using System.Net.Sockets;
using System.Diagnostics;
using System.Globalization;
using static System.Windows.Forms.AxHost;
using Syncfusion.Windows.Forms.Tools.Win32API;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.InteropServices.WindowsRuntime;
using ZXing.QrCode;
using ZXing;
using Syncfusion.WinForms.DataGrid;

namespace ioneNet.OrderManagement.Transactions
{

    public partial class GenerateEInvoice : Form
    {
        public string strToken, json,ipAddr;
        DataClasses1DataContext db = new DataClasses1DataContext();
        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        private string path;
        public string UserName = "abcd";
        public string PW = "PW";
        public string GSTIN = "36";
        public string TransportNBame,VechNo;
        public string InvDate;
        public string SellerPinCode, BuyerPinCode;
        public string WayBillData;

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public GenerateEInvoice()
        {
            InitializeComponent();
        }


        private void GenerateEInvoice_Load(object sender, EventArgs e)
        {
            if(GlobalVariables.docName == "Credit Note" || GlobalVariables.docName == "Debit Note")
            {
                btnGenerateIRN.Visible = false;
                btnGenerateIRN_CR.Visible = true;
                txtInvoiceNo.Text = frmSaleReturns.CR_No_for_EInv;
            }
            else
            {
                btnGenerateIRN.Visible = true;
                btnGenerateIRN_CR.Visible = false;
                txtInvoiceNo.Text = ListOfInvoices.Inv_NO_for_EInv;
            }
            

            var user = (from c1 in db.E_Invoice_Credentials
                        join c2 in db.Costing_Units on c1.BU_ID equals c2.id
                        where c1.BU_ID == logIn.BU_ID
                        select new { c1.E_Invoice_User_ID, c1.E_Invoice_PW, c2.GST_No,c2.PinCode }).ToList();
            if (user.Count > 0)
            {
                UserName = user[0].E_Invoice_User_ID;
                PW = user[0].E_Invoice_PW;
                GSTIN = user[0].GST_No;
                SellerPinCode = user[0].PinCode;
            }


            var State = (from c1 in db.E_Invoice_Auth_Tokens
                            where c1.company_id == logIn.BU_ID
                         select new { c1.Auth_Key, c1.Auth_Key_Valid_Till }).ToList();
            if (State.Count > 0)
            {
                txtAuthKey.Text = State[0].Auth_Key;
                txtAuthKeyValid.Text = State[0].Auth_Key_Valid_Till.ToString();

                DateTime tokenValid = Convert.ToDateTime(txtAuthKeyValid.Text);
                if (tokenValid > DateTime.Now)
                {

                }
                else
                {
                    getAuthToken();
                }

            }
            else
            {
                getAuthToken();
            }
            if (GlobalVariables.docName == "Credit Note" || GlobalVariables.docName == "Debit Note")

            {

                var irnno = (from c1 in db.SaleReturns_Masters                             
                             where c1.Company_ID == logIn.company && c1.Vch_No == txtInvoiceNo.Text
                             select new
                             {
                                 c1.Vch_Date,
                                 c1.Einv_ACK_No,                                
                                 c1.EInv_IRN_No,
                                 c1.EInv_QR_Code
                                 
                             }).ToList();
                if (irnno.Count > 0)
                {
                    txtIRNNo.Text = irnno[0].EInv_IRN_No;
                    txtAckNo.Text = irnno[0].Einv_ACK_No;                   
                    txtQRCode.Text = irnno[0].EInv_QR_Code;
                    if (irnno[0].Einv_ACK_No != null)
                    {
                        textBox1.Text = "IRN Generated";

                        btnGenerateIRN.Enabled = false;
                    }
                    else
                    {
                        btnGenerateIRN.Enabled = true;
                    }
                }
                else
                {
                    //btnGenerateIRN.Enabled = true;
                }
            }
            else
            {
                var irnno = (from c1 in db.Invoice_Masters
                             join s1 in db.Attributes_Datas on c1.Status equals s1.ID
                             where c1.Company_ID == logIn.company && c1.Inv_No == txtInvoiceNo.Text && s1.Descr == "IRN Generated"
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
                    txtIRNNo.Text = irnno[0].EInv_IRN_No;
                    txtAckNo.Text = irnno[0].Einv_ACK_No;
                    txtAckDate.Text = irnno[0].Einv_ACK_Date;
                    txtQRCode.Text = irnno[0].EInv_QR_Code;
                    txtEWBNo.Text = irnno[0].WayBillNo;
                    TransportNBame = irnno[0].Transporter_Name;
                    VechNo = irnno[0].VehicleNo;
                    textBox1.Text = "IRN Generated";
                    DateTime dt = irnno[0].InvDate.Value;
                    InvDate = dt.ToString("dd/MM/yyyy");
                    textBox3.Text = InvDate;
                    btnGenerateIRN.Enabled = false;
                }
                else
                {
                    var invdate = (from c1 in db.Invoice_Masters
                                 join s1 in db.Attributes_Datas on c1.Status equals s1.ID
                                 where c1.Company_ID == logIn.company && c1.Inv_No == txtInvoiceNo.Text
                                 select new
                                 {
                                     c1.InvDate
                                   
                                 }).ToList();
                    DateTime dt = invdate[0].InvDate.Value;
                    InvDate = dt.ToString("dd/MM/yyyy");
                    textBox3.Text = InvDate;
                    btnGenerateIRN.Enabled = true;
                }
            }


            




            getIpAdress();

        }
        private string getAuthToken()
        {
            string key = "xxx";
            string secKey = "yyy";




            //DateTime.UtcNow nonse = DateTime(1970,1,1,0,0,0).to
            string url11 = "https://api.whitebooks.in/einvoice/authenticate?email=ssits.hyd%40gmail.com";
            string sign = "0";
            //Uri ourURL = Uri(url11);
            WebRequest request = WebRequest.Create(url11);


            WebResponse myResponse;
            request.Method = "GET";
            request.Headers.Add("username", UserName);
            request.Headers.Add("password", PW);
            request.Headers.Add("ip_address", ipAddr);
            request.Headers.Add("client_id", "EINP6e5b709e-51a7-45b6-8b70-299a8a4dc84a");
            request.Headers.Add("client_secret", "EINPbba84c61-0c43-4def-af90-f64ea5185ea3");
            request.Headers.Add("gstin", GSTIN);



            // request.Headers.Add("Content-Type", "application/json; charset=utf-8");
            //request. = DataFormat.Json;


            myResponse = request.GetResponse();

            System.IO.StreamReader myreader = new System.IO.StreamReader(myResponse.GetResponseStream());
            string streamtext = myreader.ReadToEnd();
            //MessageBox.Show(streamtext);
            JObject json = JObject.Parse(streamtext);
            var authToken = (string)json.SelectToken("data.AuthToken");
            var authTokenValid = (string)json.SelectToken("data.TokenExpiry");
            txtAuthKey.Text = authToken;
            txtAuthKeyValid.Text = authTokenValid;
            if (authToken != null)
            {
                if ((from u in db.E_Invoice_Auth_Tokens where u.company_id == logIn.BU_ID select u).Count() > 0)
                {
                    var c = db.E_Invoice_Auth_Tokens.Where(w => w.company_id == logIn.BU_ID).FirstOrDefault();
                    c.Auth_Key = txtAuthKey.Text;
                    c.Auth_Key_Valid_Till = Convert.ToDateTime(txtAuthKeyValid.Text);
                    c.company_id = logIn.company;

                    db.SubmitChanges();
                    //MessageBox.Show("Record Upadated Successfully");

                }
                else
                {
                    E_Invoice_Auth_Token p = new E_Invoice_Auth_Token();
                    p.Auth_Key = txtAuthKey.Text;
                    p.Auth_Key_Valid_Till = Convert.ToDateTime(txtAuthKeyValid.Text);
                    p.company_id = logIn.BU_ID;
                    db.E_Invoice_Auth_Tokens.InsertOnSubmit(p);
                    db.SubmitChanges();
                }

                
            }
            else
            {
                MessageBox.Show("Auth Key Could not generated, check the internet connection");
                //return;

            }
            return authToken;

        }
        private async void getIRNNO(string token)
        {
            using (var httpClient = new HttpClient())
            {
                GenerateJson();
                //   using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.mastergst.com//einvoice//type//GENERATE//version//V1_03"))
                //  {
                // Replace with your authorization code

                httpClient.DefaultRequestHeaders.Add("email", "ssits.hyd@gmail.com");
                httpClient.DefaultRequestHeaders.Add("ip_address", ipAddr);
                httpClient.DefaultRequestHeaders.Add("client_id", "EINP6e5b709e-51a7-45b6-8b70-299a8a4dc84a");
                httpClient.DefaultRequestHeaders.Add("client_secret", "EINPbba84c61-0c43-4def-af90-f64ea5185ea3");
                httpClient.DefaultRequestHeaders.Add("username", UserName);
                //  request.Headers.Add("password", "Malli#123");
                httpClient.DefaultRequestHeaders.Add("auth-token", txtAuthKey.Text);
                httpClient.DefaultRequestHeaders.Add("gstin", GSTIN);
                // request.Headers.TryAddWithoutValidation("Token", "tZEbFSTsPZf039fy1LowtXZyy");

                // Replace with your E-Invoice JSON data
                //System.IO.StreamReader myreader = new System.IO.StreamReader("C:\\Users\\245 G7\\Downloads\\E_IIvoice.json");
                //string streamtext = myreader.ReadToEnd();
                //JObject json = JObject.Parse(streamtext);
                //var json1 = JsonConvert.SerializeObject(json);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                //  request.Content = data;// ("{ \"SellerDtls\": {\"Gstin\": \"27AADCG4992P1ZT\"} }");
                // request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                //string url11 = "https://api.whitebooks.in/einvoice/authenticate?email=ssits.hyd%40gmail.com";
                string url11 = "https://api.whitebooks.in/einvoice/type/GENERATE/version/V1_03?email=ssits.hyd%40gmail.com";
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url11, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var customerJsonString = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(customerJsonString);
                    
                    txtAckNo.Text = (string)json.SelectToken("data.AckNo");
                    if (txtAckNo.Text != "")
                    {
                        txtIRNNo.Text = (string)json.SelectToken("data.Irn");
                        txtEWBNo.Text = (string)json.SelectToken("data.EwbNo");
                        txtQRCode.Text = (string)json.SelectToken("data.SignedQRCode");
                        txtAckDate.Text = (string)json.SelectToken("data.AckDate");
                        var c = db.Invoice_Masters.Where(w => w.Company_ID == logIn.company && w.Inv_No == txtInvoiceNo.Text).FirstOrDefault();
                        c.Einv_ACK_No = txtAckNo.Text;
                        c.EInv_IRN_No = txtIRNNo.Text;
                        c.EInv_QR_Code = txtQRCode.Text;
                        c.WayBillNo = txtEWBNo.Text;
                        c.Einv_ACK_Date = txtAckDate.Text;
                        var irnno = (from c1 in db.Attributes_Datas
                                     where c1.Descr == "IRN Generated"
                                     select new
                                     {
                                         c1.ID
                                     }).ToList();
                       
                            c.Status = irnno[0].ID;
                                     
                        
                        db.SubmitChanges();
                    }
                    else
                    {
                        MessageBox.Show("Could Not Generate IRN due to error " + customerJsonString);
                    }
                    //var cust = JsonConvert.DeserializeObject<Response>(customerJsonString);
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }

                //  System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream());
                // string streamtext = myreader.ReadToEnd();



                //  }
            }
        }

        private async void getIRNNO_CN(string token)
        {
            using (var httpClient = new HttpClient())
            {
                GenerateJson_CN();
                //   using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.mastergst.com//einvoice//type//GENERATE//version//V1_03"))
                //  {
                // Replace with your authorization code

                httpClient.DefaultRequestHeaders.Add("email", "ssits.hyd@gmail.com");
                httpClient.DefaultRequestHeaders.Add("ip_address", ipAddr);
                httpClient.DefaultRequestHeaders.Add("client_id", "EINP6e5b709e-51a7-45b6-8b70-299a8a4dc84a");
                httpClient.DefaultRequestHeaders.Add("client_secret", "EINPbba84c61-0c43-4def-af90-f64ea5185ea3");
                httpClient.DefaultRequestHeaders.Add("username", UserName);
                //  request.Headers.Add("password", "Malli#123");
                httpClient.DefaultRequestHeaders.Add("auth-token", txtAuthKey.Text);
                httpClient.DefaultRequestHeaders.Add("gstin", GSTIN);
                // request.Headers.TryAddWithoutValidation("Token", "tZEbFSTsPZf039fy1LowtXZyy");

                // Replace with your E-Invoice JSON data
                //System.IO.StreamReader myreader = new System.IO.StreamReader("C:\\Users\\245 G7\\Downloads\\E_IIvoice.json");
                //string streamtext = myreader.ReadToEnd();
                //JObject json = JObject.Parse(streamtext);
                //var json1 = JsonConvert.SerializeObject(json);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                //  request.Content = data;// ("{ \"SellerDtls\": {\"Gstin\": \"27AADCG4992P1ZT\"} }");
                // request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                string url11 = "https://api.whitebooks.in/einvoice/authenticate?email=ssits.hyd%40gmail.com"; 
                
                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url11, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var customerJsonString = await response.Content.ReadAsStringAsync();
                    JObject json = JObject.Parse(customerJsonString);

                    txtAckNo.Text = (string)json.SelectToken("data.AckNo");
                    if (txtAckNo.Text != "")
                    {
                        txtIRNNo.Text = (string)json.SelectToken("data.Irn");
                        txtEWBNo.Text = (string)json.SelectToken("data.EwbNo");
                        txtQRCode.Text = (string)json.SelectToken("data.SignedQRCode");
                        txtAckDate.Text = (string)json.SelectToken("data.AckDate");
                        var c = db.SaleReturns_Masters.Where(w => w.Company_ID == logIn.company && w.Vch_No == txtInvoiceNo.Text).FirstOrDefault();
                        c.Einv_ACK_No = txtAckNo.Text;
                        c.EInv_IRN_No = txtIRNNo.Text;
                        c.EInv_QR_Code = txtQRCode.Text;

                        db.SubmitChanges();
                    }
                    else
                    {
                        MessageBox.Show("Could Not Generate IRN due to error " + customerJsonString);
                    }
                    //var cust = JsonConvert.DeserializeObject<Response>(customerJsonString);
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }

                //  System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream());
                // string streamtext = myreader.ReadToEnd();



                //  }
            }
        }

        private async void CancelIRNNO(string token)
        {

            IRNNo_Cancel IRNC = new IRNNo_Cancel();
            IRNC.Irn = txtIRNNo.Text;
            IRNC.CnlRsn = "1";
            IRNC.CnlRem = "Wrong Entry";

            JavaScriptSerializer serializer = new JavaScriptSerializer();

            string json = JsonConvert.SerializeObject(IRNC, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            using (var httpClient = new HttpClient())
            {

                //   using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.mastergst.com//einvoice//type//GENERATE//version//V1_03"))
                //  {
                // Replace with your authorization code

                httpClient.DefaultRequestHeaders.Add("email", "ssits.hyd@gmail.com");
                httpClient.DefaultRequestHeaders.Add("ip_address", ipAddr);
                httpClient.DefaultRequestHeaders.Add("client_id", "EINP6e5b709e-51a7-45b6-8b70-299a8a4dc84a");
                httpClient.DefaultRequestHeaders.Add("client_secret", "EINPbba84c61-0c43-4def-af90-f64ea5185ea3");
                httpClient.DefaultRequestHeaders.Add("username", UserName);
                //  request.Headers.Add("password", "Malli#123");
                httpClient.DefaultRequestHeaders.Add("auth-token", txtAuthKey.Text);
                httpClient.DefaultRequestHeaders.Add("gstin", GSTIN);
                // request.Headers.TryAddWithoutValidation("Token", "tZEbFSTsPZf039fy1LowtXZyy");

                // Replace with your E-Invoice JSON data
                //System.IO.StreamReader myreader = new System.IO.StreamReader("C:\\Users\\245 G7\\Downloads\\E_IIvoice.json");
                //string streamtext = myreader.ReadToEnd();
                //JObject json = JObject.Parse(streamtext);
                //var json1 = JsonConvert.SerializeObject(json);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                //  request.Content = data;// ("{ \"SellerDtls\": {\"Gstin\": \"27AADCG4992P1ZT\"} }");
                // request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                string url11 = "https://api.whitebooks.in/einvoice/authenticate?email=ssits.hyd%40gmail.com";


                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url11, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var customerJsonString = await response.Content.ReadAsStringAsync();
                    JObject jsoncancel = JObject.Parse(customerJsonString);

                    var IRNNo = (string)jsoncancel.SelectToken("data.Irn");
                    var cDate = (string)jsoncancel.SelectToken("data.CancelDate");


                    //var cust = JsonConvert.DeserializeObject<Response>(customerJsonString);
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }

                //  System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream());
                // string streamtext = myreader.ReadToEnd();



                //  }
            }
        }

        private async void GetIRNNOByDocNo()
        {


            //DateTime.UtcNow nonse = DateTime(1970,1,1,0,0,0).to
            string url11 = "https://api.whitebooks.in/einvoice/authenticate?email=ssits.hyd%40gmail.com";

            string sign = "0";
            //Uri ourURL = Uri(url11);
            WebRequest request = WebRequest.Create(url11);


            WebResponse myResponse;
            request.Method = "GET";            
            request.Headers.Add("docnum", txtInvoiceNo.Text);
            request.Headers.Add("docdate", textBox3.Text);            
            request.Headers.Add("ip_address", ipAddr);
            request.Headers.Add("ip_address", ipAddr);
            request.Headers.Add("client_id", "EINP6e5b709e-51a7-45b6-8b70-299a8a4dc84a");
            request.Headers.Add("client_secret", "EINPbba84c61-0c43-4def-af90-f64ea5185ea3");
            request.Headers.Add("username", UserName);
            request.Headers.Add("auth-token", txtAuthKey.Text);
           
            request.Headers.Add("gstin", GSTIN);



            // request.Headers.Add("Content-Type", "application/json; charset=utf-8");
            //request. = DataFormat.Json;


            myResponse = request.GetResponse();

            System.IO.StreamReader myreader = new System.IO.StreamReader(myResponse.GetResponseStream());
            string streamtext = myreader.ReadToEnd();
            //MessageBox.Show(streamtext);
            JObject json = JObject.Parse(streamtext);
            var IRNNo = (string)json.SelectToken("data.Irn");
            var ACKNO = (string)json.SelectToken("data.AckNo");
            var QRCODE = (string)json.SelectToken("data.SignedInvoice");
            var EwbNo = (string)json.SelectToken("data.EwbNo");
            var AckDt = (string)json.SelectToken("data.AckDt");
            //txtAuthKey.Text = authToken;
            //txtAuthKeyValid.Text = authTokenValid;

            txtIRNNo.Text = IRNNo;
            txtAckNo.Text = ACKNO;
            txtEWBNo.Text = EwbNo;
            txtQRCode.Text = QRCODE;
            txtAckDate.Text = AckDt;
            var c = db.Invoice_Masters.Where(w => w.Company_ID == logIn.company && w.Inv_No == txtInvoiceNo.Text).FirstOrDefault();
            c.Einv_ACK_No = txtAckNo.Text;
            c.EInv_IRN_No = txtIRNNo.Text;
            c.EInv_QR_Code = txtQRCode.Text;
            c.WayBillNo = txtEWBNo.Text;
            c.Einv_ACK_Date = txtAckDate.Text;
            var irnno = (from c1 in db.Attributes_Datas
                         where c1.Descr == "IRN Generated"
                         select new
                         {
                             c1.ID
                         }).ToList();

            c.Status = irnno[0].ID;

            db.SubmitChanges();

            //return authToken;
        }

        private async void GenerateEWB(string token)
        {

            genEWB EWB = new genEWB();
            EWB.Irn = txtIRNNo.Text;
            if (BuyerPinCode == SellerPinCode)
            {
                EWB.Distance = 80;
            }
            else
            {
                EWB.Distance = 0;
            }
            
            EWB.TransMode = "1";
            EWB.TransId = null;
            EWB.TransName = TransportNBame;
            EWB.TransDocDt = InvDate;
            EWB.TransDocNo = txtInvoiceNo.Text;
            EWB.VehNo = VechNo;
            EWB.VehType = "R";
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            string json = JsonConvert.SerializeObject(EWB, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            using (var httpClient = new HttpClient())
            {

                //   using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.mastergst.com//einvoice//type//GENERATE//version//V1_03"))
                //  {
                // Replace with your authorization code

                httpClient.DefaultRequestHeaders.Add("email", "ssits.hyd@gmail.com");
                httpClient.DefaultRequestHeaders.Add("ip_address", ipAddr);
                httpClient.DefaultRequestHeaders.Add("client_id", "EINP6e5b709e-51a7-45b6-8b70-299a8a4dc84a");
                httpClient.DefaultRequestHeaders.Add("client_secret", "EINPbba84c61-0c43-4def-af90-f64ea5185ea3");
                httpClient.DefaultRequestHeaders.Add("username", UserName);
                //  request.Headers.Add("password", "Malli#123");
                httpClient.DefaultRequestHeaders.Add("auth-token", txtAuthKey.Text);
                httpClient.DefaultRequestHeaders.Add("gstin", GSTIN);
                // request.Headers.TryAddWithoutValidation("Token", "tZEbFSTsPZf039fy1LowtXZyy");

                // Replace with your E-Invoice JSON data
                //System.IO.StreamReader myreader = new System.IO.StreamReader("C:\\Users\\245 G7\\Downloads\\E_IIvoice.json");
                //string streamtext = myreader.ReadToEnd();
                //JObject json = JObject.Parse(streamtext);
                //var json1 = JsonConvert.SerializeObject(json);
                var data = new StringContent(json, Encoding.UTF8, "application/json");
                //  request.Content = data;// ("{ \"SellerDtls\": {\"Gstin\": \"27AADCG4992P1ZT\"} }");
                // request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/json");
                string url11 = "https://api.whitebooks.in/einvoice/authenticate?email=ssits.hyd%40gmail.com";


                var httpContent = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await httpClient.PostAsync(url11, httpContent);

                if (response.IsSuccessStatusCode)
                {
                    var customerJsonString = await response.Content.ReadAsStringAsync();
                    JObject jsoncancel = JObject.Parse(customerJsonString);

                    var ewb = (string)jsoncancel.SelectToken("data.EwbNo");
                    var cDate = (string)jsoncancel.SelectToken("data.EwbDt");

                    txtEWBNo.Text = ewb;
                    var c = db.Invoice_Masters.Where(w => w.Company_ID == logIn.company && w.Inv_No == txtInvoiceNo.Text).FirstOrDefault();
                    c.WayBillNo = txtEWBNo.Text;              
                    db.SubmitChanges();

                    //var cust = JsonConvert.DeserializeObject<Response>(customerJsonString);
                }
                else
                {
                    Console.WriteLine("{0} ({1})", (int)response.StatusCode, response.ReasonPhrase);
                }

                //  System.IO.StreamReader myreader = new System.IO.StreamReader(response.GetResponseStream());
                // string streamtext = myreader.ReadToEnd();



                //  }
            }
        }

        private void GenerateJson()
        {
            int i = 0;
            //int R = sfDataGrid1.CurrentCell.RowIndex;
            //// int k = sfDataGrid1.CurrentCell.RowIndex;
            //var currentCellValue = sfDataGrid1.CurrentCell.CellRenderer.GetControlValue();
            //var rowData = sfDataGrid1.GetRecordAtRowIndex(R);
            //var mappingName = sfDataGrid1.Columns[1].MappingName;
            //var mappingName1 = sfDataGrid1.Columns[4].MappingName;
            //var cellVaue = (rowData.GetType().GetProperty(mappingName).GetValue(rowData, null).ToString());
            string myString = txtInvoiceNo.Text;

            var comp = (from c in db.Costing_Units where c.id == logIn.BU_ID select c).ToList();

            var da = (from inv in db.Invoice_Masters
                      where inv.Inv_No == myString && inv.Company_ID == logIn.company
                      select inv).ToList();

            var cust = (from d in db.Supplier_informations where d.ID == da[0].BuyerName select d).ToList();

            var con = (from d in db.Supplier_informations where d.ID == da[0].ConsigneeName select d).ToList();

            string LRNO = da[0].LR_No;

            ReqPlGenIRN reqPlGenIRN = new ReqPlGenIRN();
            reqPlGenIRN.Version = "1.1";
            reqPlGenIRN.TranDtls = new ReqPlGenIRN.TranDetails();
            reqPlGenIRN.TranDtls.TaxSch = "GST";
            if (da[0].InvType == "SEZ Invoice")
            {
                reqPlGenIRN.TranDtls.SupTyp = "SEZWOP";
            }
            else
            if (da[0].InvType == "SEZ Service Invoice")
            {
                reqPlGenIRN.TranDtls.SupTyp = "SEZWOP";
            }
            else
            {
                reqPlGenIRN.TranDtls.SupTyp = "B2B";
            }
            reqPlGenIRN.TranDtls.IgstOnIntra = "N";
            reqPlGenIRN.TranDtls.RegRev = "N";
            reqPlGenIRN.TranDtls.EcmGstin = null;


            reqPlGenIRN.DocDtls = new ReqPlGenIRN.DocSetails();
            reqPlGenIRN.DocDtls.Typ = "INV";



            reqPlGenIRN.DocDtls.No = da[0].Inv_No;
            DateTime dt = da[0].InvDate.Value;
            //string dt1 = dt.ToString("dd/MM/yyyy");
            string dt1 = String.Format("{0:dd'/'MM'/'yyyy}", dt);
            //var dt2 = DateTime.TryParse(dt1, out DateTime myDate);
            //string dt3 = myDate.ToString("dd/MM/yyyy");
            //var dt2 = DateTime.ParseExact(dt1, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            //var dt2 =  Convert.ToDateTime(dt, CultureInfo.CurrentCulture).ToString("dd/MM/yyyy");
            reqPlGenIRN.DocDtls.Dt = dt1;
            reqPlGenIRN.SellerDtls = new ReqPlGenIRN.SellerDetails();
            reqPlGenIRN.SellerDtls.Gstin = comp[0].GST_No;
            reqPlGenIRN.SellerDtls.LglNm = comp[0].ToPrintName;
            reqPlGenIRN.SellerDtls.TrdNm = comp[0].ToPrintName;
            string A1 = null;
            string A2 = null;

            //if (comp[0].Address.Length>100)
            //{
            //    int Alen = comp[0].Address.Length;
            //    A1 = Mid(comp[0].Address, 1, 100);
            //    A2 = Mid(comp[0].Address, 101, Alen-1);
            //}
            //else
            //{
            A1 = comp[0].Address;
            //}
            if(A1.Length >100)
            {
                MessageBox.Show("Company Address Cannot be More Than 100 Characters");
                return;
            }
            reqPlGenIRN.SellerDtls.Addr1 = A1;
            reqPlGenIRN.SellerDtls.Addr2 = comp[0].City;
            reqPlGenIRN.SellerDtls.Loc = comp[0].City;
            reqPlGenIRN.SellerDtls.Pin = Convert.ToInt32(comp[0].PinCode);
            reqPlGenIRN.SellerDtls.Stcd = comp[0].State_Code;
            reqPlGenIRN.SellerDtls.Ph = null;
            reqPlGenIRN.SellerDtls.Em = comp[0].E_Mail;



            reqPlGenIRN.BuyerDtls = new ReqPlGenIRN.BuyerDetails();
            reqPlGenIRN.BuyerDtls.Gstin = cust[0].GSTIN_NO;
            reqPlGenIRN.BuyerDtls.LglNm = cust[0].Supplier_Alias_Name;
            reqPlGenIRN.BuyerDtls.TrdNm = null;
            reqPlGenIRN.BuyerDtls.Addr1 = cust[0].Address_1;
            if (cust[0].Address_2 != "")
            {
                reqPlGenIRN.BuyerDtls.Addr2 = cust[0].Address_2;
            }
            else
            {
                reqPlGenIRN.BuyerDtls.Addr2 = null;
            }
            reqPlGenIRN.BuyerDtls.Loc = cust[0].City;
            BuyerPinCode = cust[0].Pincode;
            reqPlGenIRN.BuyerDtls.Pin = Convert.ToInt32(cust[0].Pincode);
            reqPlGenIRN.BuyerDtls.Pos = cust[0].StateCode;
            reqPlGenIRN.BuyerDtls.Stcd = cust[0].StateCode;
            reqPlGenIRN.BuyerDtls.Ph = null;
            reqPlGenIRN.BuyerDtls.Em = null;

            reqPlGenIRN.DispDtls = null;
            //    new ReqPlGenIRN.DispatchedDetails();                
            //reqPlGenIRN.DispDtls.Nm = con[0].Supplier_Alias_Name;
            //reqPlGenIRN.DispDtls.Addr1 = con[0].Address_1;
            //if (con[0].Address_2 != "")
            //{
            //    reqPlGenIRN.DispDtls.Addr2 = con[0].Address_2;
            //}
            //else
            //{
            //    reqPlGenIRN.DispDtls.Addr2 = null;
            //}

            //reqPlGenIRN.DispDtls.Loc = con[0].City;
            //reqPlGenIRN.DispDtls.Pin = Convert.ToInt32(con[0].Pincode);
            //reqPlGenIRN.DispDtls.Stcd = con[0].StateCode;

            var da1 = (from inv in db.Invoice_Childs
                       join so in db.Invoice_Masters 
                       on inv.Inv_Master_ID equals so.Id
                       where inv.Inv_Master_ID == da[0].Id && so.BU_ID == logIn.BU_ID && so.Status  !=24 
                       select new { inv.SO_Ref_No,inv.Prod_Code, inv.Product_Description,inv.Qty,inv.Uom, so.Inv_No, 
                           inv.Price,
                           inv.Disc_Amount,
                           inv.Amount,inv.Taxable_Value,inv.CGST_Per,inv.SGST_Per,inv.IGST_Per,
                           inv.SGST_Amnt,inv.CGST_Amnt,inv.IGST_Amnt,inv.Net_Amount,
                           so.ConsigneeAddress }).ToList();
            JObject jsoncancel = JObject.Parse(da1[0].ConsigneeAddress);
            
            reqPlGenIRN.ShipDtls = new ReqPlGenIRN.ShippedDetails();
            reqPlGenIRN.ShipDtls.Gstin = (string)jsoncancel.SelectToken("GSTIN");
            reqPlGenIRN.ShipDtls.LglNm = con[0].Supplier_Alias_Name;
            reqPlGenIRN.ShipDtls.TrdNm = null;
            reqPlGenIRN.ShipDtls.Addr1 = (string)jsoncancel.SelectToken("Address1");
            if ((string)jsoncancel.SelectToken("Address2") != "")
            {
                reqPlGenIRN.ShipDtls.Addr2 = (string)jsoncancel.SelectToken("Address2");
            }
            else
            {
                reqPlGenIRN.ShipDtls.Addr2 = null;
            }

            reqPlGenIRN.ShipDtls.Loc = (string)jsoncancel.SelectToken("City");
            BuyerPinCode = (string)jsoncancel.SelectToken("PinCode");
            reqPlGenIRN.ShipDtls.Pin = Convert.ToInt32((string)jsoncancel.SelectToken("PinCode"));
            reqPlGenIRN.ShipDtls.Stcd = (string)jsoncancel.SelectToken("StateCode");

            reqPlGenIRN.ItemList = new List<ReqPlGenIRN.ItmList>();

            foreach (var ItemNo in da1)
            {
                var prodhsncode = "";
                var prodtype = "";
                var prod1 = (from p in db.Products
                             join pt in db.Attributes_Prod_Types on p.Prod_Type_Id equals pt.Prod_Type_Id
                             where p.prod_ID == Convert.ToInt32(ItemNo.Prod_Code) && p.Company_ID == logIn.company
                             select new { pt.Prod_Type, p.Prod_HSN_Code }).ToList();
                if (prod1.Count > 0)
                {
                    prodhsncode = prod1[0].Prod_HSN_Code;
                    prodtype = prod1[0].Prod_Type;
                }
                else
                {
                    var prod = (from p in db.Sale_Order_Childs where p.SO_NO == ItemNo.SO_Ref_No && p.Prod_Code == Convert.ToInt32(ItemNo.Prod_Code) select p).ToList();

                    prodhsncode = prod[0].HSN_Code;
                }


                 ReqPlGenIRN.ItmList itm = new ReqPlGenIRN.ItmList();
                itm.SlNo = (i + 1).ToString();
                
                if (prodtype == "Service" || prodtype == "SERVICE" || da[0].InvType == "Service Invoice" || da[0].InvType == "SEZ Service Invoice")
                {
                    itm.IsServc = "Y";
                }
                else
                {
                    itm.IsServc = "N";
                }
                itm.PrdDesc = ItemNo.Product_Description.Trim();
                itm.HsnCd = prodhsncode;
                itm.BchDtls = null;
                itm.Qty = Convert.ToDouble(ItemNo.Qty);
                itm.Unit = da1[0].Uom.Trim();
                itm.UnitPrice = Convert.ToDouble(ItemNo.Price);
                itm.TotAmt = Convert.ToDouble(ItemNo.Amount);
                itm.Discount = Convert.ToDouble(ItemNo.Disc_Amount);
                itm.AssAmt = Convert.ToDouble(ItemNo.Taxable_Value);
                itm.GstRt = Convert.ToDouble(ItemNo.CGST_Per + ItemNo.SGST_Per + ItemNo.IGST_Per);
                itm.SgstAmt = Convert.ToDouble(ItemNo.SGST_Amnt);
                itm.IgstAmt = Convert.ToDouble(ItemNo.IGST_Amnt);
                itm.CgstAmt = Convert.ToDouble(ItemNo.CGST_Amnt);
                itm.CesRt = 0.0;
                itm.CesAmt = 0.0;
                itm.CesNonAdvlAmt = 0.0;
                itm.StateCesRt = 0.0;
                itm.StateCesAmt = 0.0;
                itm.StateCesNonAdvlAmt = 0.0;
                itm.OthChrg = 0.0;
                itm.TotItemVal = Convert.ToDouble(ItemNo.Net_Amount);
                itm.AttribDtls = null;
                reqPlGenIRN.ItemList.Add(itm);

                i = i + 1;
            }

            

            reqPlGenIRN.PayDtls = null;
            reqPlGenIRN.RefDtls = null;
            reqPlGenIRN.AddlDocDtls = null;
            reqPlGenIRN.ExpDtls = null;

            reqPlGenIRN.EwbDtls = new ReqPlGenIRN.EwbDetails();
            reqPlGenIRN.EwbDtls.TransId = null;
            if (da[0].InvType == "Service Invoice")
            {
                reqPlGenIRN.EwbDtls.TransName = null;
            }
            else
            {
                reqPlGenIRN.EwbDtls.TransName = da[0].Transporter_Name;
            }
            reqPlGenIRN.EwbDtls.TransMode = "1";
            if (BuyerPinCode == SellerPinCode)
            {
                reqPlGenIRN.EwbDtls.Distance = 80;
            }
            else
            {
                reqPlGenIRN.EwbDtls.Distance = 0;
            }
            string TDoc;
            if (LRNO.Length > 1)
            {
                TDoc = LRNO;
            }
            else
            {
                if (txtInvoiceNo.Text.Length > 15)
                {
                    TDoc = Mid(txtInvoiceNo.Text, 1, 14);
                }
                else
                {
                    TDoc = txtInvoiceNo.Text;
                }
            }
            reqPlGenIRN.EwbDtls.TransDocNo = TDoc;
            reqPlGenIRN.EwbDtls.TransDocDt = dt1;
            if (da[0].InvType == "Service Invoice")
            {
                reqPlGenIRN.EwbDtls.VehNo = null;
            }
            else
            {
                reqPlGenIRN.EwbDtls.VehNo = da[0].VehicleNo;
            }
            
            reqPlGenIRN.EwbDtls.VehType = "R";

            reqPlGenIRN.ValDtls = new ReqPlGenIRN.ValDetails();
            reqPlGenIRN.ValDtls.AssVal = Convert.ToDouble(da[0].Tot_TaxableValue);
            reqPlGenIRN.ValDtls.CgstVal = Convert.ToDouble(da[0].Tot_CGST_Amnt);
            reqPlGenIRN.ValDtls.SgstVal = Convert.ToDouble(da[0].Tot_SGST_Amnt);
            reqPlGenIRN.ValDtls.IgstVal = Convert.ToDouble(da[0].Tot_IGST_Amnt);
            reqPlGenIRN.ValDtls.CesVal = 0.0;
            reqPlGenIRN.ValDtls.StCesVal = 0.0;
            reqPlGenIRN.ValDtls.OthChrg = Convert.ToDouble(da[0].TCS_Amnt) + Convert.ToDouble(da[0].Packing_Charges);
            reqPlGenIRN.ValDtls.RndOffAmt = Convert.ToDouble(da[0].Rounding);
            reqPlGenIRN.ValDtls.TotInvVal = Convert.ToDouble(da[0].Tot_Inv_Value); ;


            JavaScriptSerializer serializer = new JavaScriptSerializer();

            json = JsonConvert.SerializeObject(reqPlGenIRN, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }


        private void GenerateJson_CN()
        {
            int i = 0;            
            string myString = txtInvoiceNo.Text;

            var comp = (from c in db.Costing_Units where c.id == logIn.BU_ID select c).ToList();

            var da = (from sr in db.SaleReturns_Masters
                     
                      where sr.Vch_No == myString && sr.Company_ID == logIn.company && sr.BU_ID == logIn.BU_ID
                      select sr).ToList();

            var cust = (from d in db.Supplier_informations where d.ID == da[0].BuyerName select d).ToList();

            var con = (from d in db.Supplier_informations where d.ID == da[0].ConsigneeName select d).ToList();

            var invType = (from iv in db.Invoice_Masters where iv.Inv_No == da[0].Inv_No select iv).ToList();

            ReqPlGenIRN reqPlGenIRN = new ReqPlGenIRN();
            reqPlGenIRN.Version = "1.1";
            reqPlGenIRN.TranDtls = new ReqPlGenIRN.TranDetails();
            reqPlGenIRN.TranDtls.TaxSch = "GST";

            if (invType[0].InvType == "SEZ Invoice")
            {
                reqPlGenIRN.TranDtls.SupTyp = "SEZWOP";
            }
            else
            if (invType[0].InvType == "SEZ Service Invoice")
            {
                reqPlGenIRN.TranDtls.SupTyp = "SEZWOP";
            }
            else
            {
                reqPlGenIRN.TranDtls.SupTyp = "B2B";
            }
            //reqPlGenIRN.TranDtls.SupTyp = "B2B";
            
            reqPlGenIRN.TranDtls.IgstOnIntra = "N";
            reqPlGenIRN.TranDtls.RegRev = "N";
            reqPlGenIRN.TranDtls.EcmGstin = null;


            reqPlGenIRN.DocDtls = new ReqPlGenIRN.DocSetails();
            if (GlobalVariables.docName == "Debit Note")
            {
                reqPlGenIRN.DocDtls.Typ = "DBN";
            }
            else
            {
                reqPlGenIRN.DocDtls.Typ = "CRN";
            }
            reqPlGenIRN.DocDtls.No = da[0].Vch_No;
            DateTime dt = da[0].Vch_Date.Value;
            //string dt1 = dt.ToString("dd/MM/yyyy");
            string dt1 = String.Format("{0:dd'/'MM'/'yyyy}", dt);



            //DateTime dt = da[0].Vch_Date.Value;
            //string dt1 = dt.ToString("dd/MM/yyyy");
            reqPlGenIRN.DocDtls.Dt = dt1;
            reqPlGenIRN.SellerDtls = new ReqPlGenIRN.SellerDetails();
            reqPlGenIRN.SellerDtls.Gstin = comp[0].GST_No;
            reqPlGenIRN.SellerDtls.LglNm = logIn.compname;
            reqPlGenIRN.SellerDtls.TrdNm = logIn.compname;
            string A1 = null;
            string A2 = null;

            //if (comp[0].Address.Length>100)
            //{
            //    int Alen = comp[0].Address.Length;
            //    A1 = Mid(comp[0].Address, 1, 100);
            //    A2 = Mid(comp[0].Address, 101, Alen-1);
            //}
            //else
            //{
            A1 = comp[0].Address;
            //}
            reqPlGenIRN.SellerDtls.Addr1 = A1;
            reqPlGenIRN.SellerDtls.Addr2 = comp[0].City;
            reqPlGenIRN.SellerDtls.Loc = comp[0].City;
            reqPlGenIRN.SellerDtls.Pin = Convert.ToInt32(comp[0].PinCode);
            reqPlGenIRN.SellerDtls.Stcd = comp[0].State_Code;
            reqPlGenIRN.SellerDtls.Ph = null;
            reqPlGenIRN.SellerDtls.Em = comp[0].E_Mail;



            reqPlGenIRN.BuyerDtls = new ReqPlGenIRN.BuyerDetails();
            reqPlGenIRN.BuyerDtls.Gstin = cust[0].GSTIN_NO;
            reqPlGenIRN.BuyerDtls.LglNm = cust[0].Supplier_Alias_Name;
            reqPlGenIRN.BuyerDtls.TrdNm = null;
            reqPlGenIRN.BuyerDtls.Addr1 = cust[0].Address_1;
            if (cust[0].Address_2 != "")
            {
                reqPlGenIRN.BuyerDtls.Addr2 = cust[0].Address_2;
            }
            else
            {
                reqPlGenIRN.BuyerDtls.Addr2 = null;
            }
            reqPlGenIRN.BuyerDtls.Loc = cust[0].City;
            BuyerPinCode = cust[0].Pincode;
            reqPlGenIRN.BuyerDtls.Pin = Convert.ToInt32(cust[0].Pincode);
            reqPlGenIRN.BuyerDtls.Pos = cust[0].StateCode;
            reqPlGenIRN.BuyerDtls.Stcd = cust[0].StateCode;
            reqPlGenIRN.BuyerDtls.Ph = null;
            reqPlGenIRN.BuyerDtls.Em = null;

            reqPlGenIRN.DispDtls = null;
            //    new ReqPlGenIRN.DispatchedDetails();                
            //reqPlGenIRN.DispDtls.Nm = con[0].Supplier_Alias_Name;
            //reqPlGenIRN.DispDtls.Addr1 = con[0].Address_1;
            //if (con[0].Address_2 != "")
            //{
            //    reqPlGenIRN.DispDtls.Addr2 = con[0].Address_2;
            //}
            //else
            //{
            //    reqPlGenIRN.DispDtls.Addr2 = null;
            //}

            //reqPlGenIRN.DispDtls.Loc = con[0].City;
            //reqPlGenIRN.DispDtls.Pin = Convert.ToInt32(con[0].Pincode);
            //reqPlGenIRN.DispDtls.Stcd = con[0].StateCode;

            var da1 = (from inv in db.SaleReturns_Childs
                       join so in db.SaleReturns_Masters
                       on inv.SR_Master_ID equals so.Id
                       where inv.SR_Master_ID == da[0].Id && inv.Company_ID == logIn.company 
                       select new
                       {
                           inv.Inv_Ref_No,
                           inv.Prod_Code,
                           inv.Product_Description,
                           inv.Return_Qty,
                           inv.Uom,
                           so.Inv_No,
                           inv.Price,
                           inv.Disc_Amount,
                           inv.Amount,
                           inv.Taxable_Value,
                           inv.CGST_Per,
                           inv.SGST_Per,
                           inv.IGST_Per,
                           inv.SGST_Amnt,
                           inv.CGST_Amnt,
                           inv.IGST_Amnt,
                           inv.Net_Amount,
                           so.ConsigneeAddress
                       }).ToList();
            JObject jsoncancel = JObject.Parse(da1[0].ConsigneeAddress);

            reqPlGenIRN.ShipDtls = new ReqPlGenIRN.ShippedDetails();
            reqPlGenIRN.ShipDtls.Gstin = (string)jsoncancel.SelectToken("GSTIN");
            reqPlGenIRN.ShipDtls.LglNm = con[0].Supplier_Alias_Name;
            reqPlGenIRN.ShipDtls.TrdNm = null;
            reqPlGenIRN.ShipDtls.Addr1 = (string)jsoncancel.SelectToken("Address1");
            if ((string)jsoncancel.SelectToken("Address2") != "")
            {
                reqPlGenIRN.ShipDtls.Addr2 = (string)jsoncancel.SelectToken("Address2");
            }
            else
            {
                reqPlGenIRN.ShipDtls.Addr2 = null;
            }

            reqPlGenIRN.ShipDtls.Loc = (string)jsoncancel.SelectToken("City");
            BuyerPinCode = (string)jsoncancel.SelectToken("PinCode");
            reqPlGenIRN.ShipDtls.Pin = Convert.ToInt32((string)jsoncancel.SelectToken("PinCode"));
            reqPlGenIRN.ShipDtls.Stcd = (string)jsoncancel.SelectToken("StateCode");

            reqPlGenIRN.ItemList = new List<ReqPlGenIRN.ItmList>();

            foreach (var ItemNo in da1)
            {
                var prodhsncode = "";
                var prodtype = "";
                var prod1 = (from p in db.Products
                             join pt in db.Attributes_Prod_Types on p.Prod_Type_Id equals pt.Prod_Type_Id
                             where p.prod_ID == Convert.ToInt32(ItemNo.Prod_Code) && p.Company_ID == logIn.company
                             select new { pt.Prod_Type, p.Prod_HSN_Code }).ToList();
                if (prod1.Count > 0)
                {
                    prodhsncode = prod1[0].Prod_HSN_Code;
                    prodtype = prod1[0].Prod_Type;
                }
                else
                { 
                var prod = (from p in db.Sale_Order_Childs
                            join inv in db.Invoice_Childs on p.SO_NO equals inv.SO_Ref_No

                            where inv.Inv_No == ItemNo.Inv_Ref_No && p.Prod_Code == Convert.ToInt32(ItemNo.Prod_Code)
                            select p).ToList();
                    prodhsncode = prod[0].HSN_Code;                    
                }
                ReqPlGenIRN.ItmList itm = new ReqPlGenIRN.ItmList();
                itm.SlNo = (i + 1).ToString();
                if(prodtype == "SERVICE" || prodtype == "Service")
                {
                    itm.IsServc = "Y";
                }
                else
                {
                    itm.IsServc = "N";
                }
                itm.PrdDesc = ItemNo.Product_Description.Trim();
                itm.HsnCd = prodhsncode.Trim();
                itm.BchDtls = null;
                itm.Qty = Convert.ToDouble(ItemNo.Return_Qty);
                itm.Unit = da1[0].Uom.Trim();
                itm.UnitPrice = Convert.ToDouble(ItemNo.Price);
                itm.TotAmt = Convert.ToDouble(ItemNo.Amount);
                itm.Discount = Convert.ToDouble(ItemNo.Disc_Amount);
                itm.AssAmt = Convert.ToDouble(ItemNo.Taxable_Value);
                itm.GstRt = Convert.ToDouble(ItemNo.CGST_Per + ItemNo.SGST_Per + ItemNo.IGST_Per);
                itm.SgstAmt = Convert.ToDouble(ItemNo.SGST_Amnt);
                itm.IgstAmt = Convert.ToDouble(ItemNo.IGST_Amnt);
                itm.CgstAmt = Convert.ToDouble(ItemNo.CGST_Amnt);
                itm.CesRt = 0.0;
                itm.CesAmt = 0.0;
                itm.CesNonAdvlAmt = 0.0;
                itm.StateCesRt = 0.0;
                itm.StateCesAmt = 0.0;
                itm.StateCesNonAdvlAmt = 0.0;
                itm.OthChrg = 0.0;
                itm.TotItemVal = Convert.ToDouble(ItemNo.Net_Amount);
                itm.AttribDtls = null;
                reqPlGenIRN.ItemList.Add(itm);

                i = i + 1;
            }



            reqPlGenIRN.PayDtls = null;
            reqPlGenIRN.RefDtls = null;
            reqPlGenIRN.AddlDocDtls = null;
            reqPlGenIRN.ExpDtls = null;

            reqPlGenIRN.EwbDtls = new ReqPlGenIRN.EwbDetails();
            reqPlGenIRN.EwbDtls.TransId = null;
            reqPlGenIRN.EwbDtls.TransName = null;
            reqPlGenIRN.EwbDtls.TransMode = "1";
            if (BuyerPinCode == SellerPinCode)
            {
                reqPlGenIRN.EwbDtls.Distance = 80;
            }
            else
            {
                reqPlGenIRN.EwbDtls.Distance = 0;
            }

            reqPlGenIRN.EwbDtls.TransDocNo = txtInvoiceNo.Text;
            reqPlGenIRN.EwbDtls.TransDocDt = dt1;
            reqPlGenIRN.EwbDtls.VehNo = da[0].VehicleNo; // "NA";
            reqPlGenIRN.EwbDtls.VehType = "R";

            reqPlGenIRN.ValDtls = new ReqPlGenIRN.ValDetails();
            reqPlGenIRN.ValDtls.AssVal = Convert.ToDouble(da[0].Tot_TaxableValue);
            reqPlGenIRN.ValDtls.CgstVal = Convert.ToDouble(da[0].Tot_CGST_Amnt);
            reqPlGenIRN.ValDtls.SgstVal = Convert.ToDouble(da[0].Tot_SGST_Amnt);
            reqPlGenIRN.ValDtls.IgstVal = Convert.ToDouble(da[0].Tot_IGST_Amnt);
            reqPlGenIRN.ValDtls.CesVal = 0.0;
            reqPlGenIRN.ValDtls.StCesVal = 0.0;
            reqPlGenIRN.ValDtls.OthChrg = Convert.ToDouble(da[0].TCS_Amnt);
            reqPlGenIRN.ValDtls.RndOffAmt = Convert.ToDouble(da[0].Rounding);
            reqPlGenIRN.ValDtls.TotInvVal = Convert.ToDouble(da[0].Tot_Inv_Value); ;


            JavaScriptSerializer serializer = new JavaScriptSerializer();

            json = JsonConvert.SerializeObject(reqPlGenIRN, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });
        }


        private void button8_Click(object sender, EventArgs e)
        {
            //  getAuthToken();
            strToken = getAuthToken();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string date = DateTime.Today.ToString("dd/MM/yyyy");
            Console.WriteLine(date);

            if (txtAuthKeyValid.Text != "")
            {
                DateTime tokenValid = Convert.ToDateTime(txtAuthKeyValid.Text);
                if (tokenValid > DateTime.Now)
                {

                }
                else
                {
                    getAuthToken();
                }
            }
            else
            {
                getAuthToken();
            }
            getIRNNO(txtAuthKey.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {

            var uRole = (from m in db.User_Roles where m.Company_ID == logIn.company && m.Form_Name == "GST Invoice" && m.Role_ID == logIn.UserRoleID select new { m.Delete_Role }).Distinct().ToList();
            if (uRole.Count > 0)
            {
                if (uRole[0].Delete_Role == true)
                {

                    DialogResult result = MessageBox.Show("Are You Sure To Cancel The Invoice? Cannot Undo This Operation!", "Cancel Confirmation", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {

                        CancelIRNNO(txtIRNNo.Text);
                        string SO_No = txtInvoiceNo.Text;
                        if (btnGenerateIRN.Visible == true)
                        {
                            var ci = db.Invoice_Masters.Where(w => w.Inv_No == SO_No && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                            {
                                ci.Status = 25;
                                ci.Modified_By = logIn.username + "-" + DateTime.Now;
                                db.SubmitChanges();

                            }
                        }
                        else
                        {
                            var ci = db.SaleReturns_Masters.Where(w => w.Vch_No == SO_No && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                            {
                                ci.Spl_Instructions = "Cancelled";
                                ci.Modified_By = logIn.username + "-" + DateTime.Now;
                                db.SubmitChanges();

                            }
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

        public class IRNNo_Cancel
        {
            public string Irn { get; set; }
            public string CnlRsn { get; set; }
            public string CnlRem { get; set; }

        }

        public class genEWB
        {
            public string Irn { get; set; }
            public int Distance { get; set; }
            public string TransMode { get; set; }
            public string TransId { get; set; }
            public string TransName { get; set; }
            public string TransDocDt { get; set; }
            public string TransDocNo { get; set; }
            public string VehNo { get; set; }
            public string VehType { get; set; }





        }

        private void btnPrintInvoice_Click(object sender, EventArgs e)
        {
            try
            {
                IBarcodeWriter writer = new BarcodeWriter { Format = BarcodeFormat.QR_CODE };

                string QrCode = "";
                if(txtQRCode.Text.Length > 955)
                {
                    QrCode = Mid(txtQRCode.Text,1,954);
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
                if (con.State != ConnectionState.Open)
                    con.Open();
                String SO_No = txtInvoiceNo.Text;
                SqlCommand cmd5 = new SqlCommand("delete  from [temp_inv_QRCOde] where Company_ID =@comp", con);
                cmd5.Parameters.AddWithValue("@comp", logIn.company);
                cmd5.ExecuteNonQuery();
                SqlCommand cmd1 = con.CreateCommand();

                cmd1.CommandText = "INSERT INTO temp_inv_QRCOde  (Inv_No,Company_ID,QR_COde) VALUES  (@invNo1," + logIn.company + ",@Qrcode)";
                //cmd1.CommandText =  "INSERT INTO dbo.temp_Inv_Copy ([Inv_No],[Copy_Name],[Copy_No],[Company_ID]) VALUES (@invNo,@Copy_Name,@Copy_No, @comp)";

                //// SqlCommand command = new SqlCommand(query, db.Connection);
                cmd1.Parameters.AddWithValue("@invNo1", SO_No);
                cmd1.Parameters.AddWithValue("@Qrcode", bytes);
                cmd1.ExecuteNonQuery();


                if (GlobalVariables.docName == "Credit Note" || GlobalVariables.docName == "Debit Note")
                {
                    GetCreditnotePrint();   
                }
                else
                {
                    
                    SqlCommand cmd2 = con.CreateCommand();
                    
                    //int i = sfDataGrid1.CurrentRow.Index;
                    

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

                        var da1 = (from inv in db.Invoice_Childs
                                   join so in db.Sale_Order_Masters

                                   on new { A = inv.SO_Ref_No.Trim(), B = inv.Company_ID } equals new { A = so.SO_NO, B = so.Company_ID }
                                   join c in db.Supplier_informations on so.ConsigneeName equals c.ID
                                   where inv.Inv_No == SO_No && inv.Company_ID == logIn.company && so.Status!=24
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
                        rep.SetParameterValue("Con_Address1", CAddr);
                        rep.SetParameterValue("Con_City", CCity);
                        rep.SetParameterValue("Con_State", cState);
                        rep.SetParameterValue("Con_GSTIN", cGSTIN);
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
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void GetCreditnotePrint()
        {
            try
            {
                
                
                
                
                SqlCommand cmd1 = con.CreateCommand();
                SqlCommand cmd2 = con.CreateCommand();
                if (con.State != ConnectionState.Open)
                    con.Open();
                
                path = Path.Combine(Directory.GetCurrentDirectory(), "CreditNote.pdf");
                //string path = @"D:\Invoice.pdf";
                FileInfo fi1 = new FileInfo(path);
                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();
                if (logIn.company == 20)
                {
                    //rep = new OrderManagement.Transactions.Credit_Note_Vpack();
                }
                else
                {
                    rep = new OrderManagement.Transactions.Credit_Note();
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

                rep.RecordSelectionFormula = "{ SaleReturns_Master.Vch_No} = '" + txtInvoiceNo.Text + "' and { SaleReturns_Master.Company_ID} = " + logIn.company + " and { SaleReturns_Master.BU_ID} = " + logIn.BU_ID + "";
                ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();
                viewer.crystalReportViewer1.ReportSource = rep;
                viewer.crystalReportViewer1.Refresh();

                rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);



                con.Close();
                Process.Start(path);


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public static string Mid(string s, int a, int b)
        {
            string temp = s.Substring(a - 1, b);
            return temp;
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            try
            {
                GetEWBByIRNNo();
                JObject json = JObject.Parse(WayBillData);
                var EwbNo = (string)json.SelectToken("data.EwbNo");
                var GenGstin = (string)json.SelectToken("data.GenGstin");
                var EwbValidTill = (string)json.SelectToken("data.EwbValidTill");
                var EwbDt = (string)json.SelectToken("data.EwbDt");
                var Status = (string)json.SelectToken("data.Status");
                DateTime wbdate = Convert.ToDateTime(EwbDt);
                DateTime wbvaliddate = Convert.ToDateTime(EwbValidTill);
                IBarcodeWriter writer = new BarcodeWriter { Format = BarcodeFormat.QR_CODE };

                string QrCode = "";

                QrCode = "EWB No.:" + EwbNo + "/ GSTIN:" + GenGstin + " / Date : " + wbdate;

                var result = writer.Write(QrCode);
                var barcodeBitmap = new Bitmap(result);
                pictureBox1.Image = barcodeBitmap;
                Image img = pictureBox1.Image;
                MemoryStream ms = new MemoryStream();
                img.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                byte[] bytes = ms.ToArray();
                if (con.State != ConnectionState.Open)
                    con.Open();
                String SO_No = txtInvoiceNo.Text;
                SqlCommand cmd5 = new SqlCommand("delete  from [temp_inv_QRCOde] where Company_ID =@comp", con);
                cmd5.Parameters.AddWithValue("@comp", logIn.company);
                cmd5.ExecuteNonQuery();
                SqlCommand cmd1 = con.CreateCommand();

                cmd1.CommandText = "INSERT INTO temp_inv_QRCOde  (Inv_No,Company_ID,QR_COde) VALUES  (@invNo1," + logIn.company + ",@Qrcode)";
                //cmd1.CommandText =  "INSERT INTO dbo.temp_Inv_Copy ([Inv_No],[Copy_Name],[Copy_No],[Company_ID]) VALUES (@invNo,@Copy_Name,@Copy_No, @comp)";

                //// SqlCommand command = new SqlCommand(query, db.Connection);
                cmd1.Parameters.AddWithValue("@invNo1", SO_No);
                cmd1.Parameters.AddWithValue("@Qrcode", bytes);
                cmd1.ExecuteNonQuery();

                SqlCommand cmd4 = new SqlCommand("delete  from [temp_Inv_Copy] where Company_ID =@comp", con);
                cmd4.Parameters.AddWithValue("@comp", logIn.company);
                cmd4.ExecuteNonQuery();



                CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();


                rep = new OrderManagement.Transactions.rptEWaybill();
                cmd1.CommandText = "INSERT INTO temp_Inv_Copy  (Inv_No, Copy_Name, Copy_No,Company_ID) VALUES  (@invNo, 'Original for Receipent', '1'," + logIn.company + ")";
                //cmd1.CommandText =  "INSERT INTO dbo.temp_Inv_Copy ([Inv_No],[Copy_Name],[Copy_No],[Company_ID]) VALUES (@invNo,@Copy_Name,@Copy_No, @comp)";

                //// SqlCommand command = new SqlCommand(query, db.Connection);
                cmd1.Parameters.AddWithValue("@invNo", SO_No);

                cmd1.ExecuteNonQuery();



                path = Path.Combine(Directory.GetCurrentDirectory(), "WayBill.pdf");
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
                               where inv.Inv_No == SO_No && inv.Company_ID == logIn.company && so.Status != 24
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
                    rep.SetParameterValue("Con_Address1", CAddr);
                    rep.SetParameterValue("Con_City", CCity);
                    rep.SetParameterValue("Con_State", cState);
                    rep.SetParameterValue("ewbGenDate", wbdate.ToString());
                    rep.SetParameterValue("ewbValidTill", wbvaliddate.ToString());
                    rep.SetParameterValue("ApproxDistance", "");
                    string SuppType;
                    var da2 = (from inv in db.Invoice_Masters
                               where inv.Inv_No == txtInvoiceNo.Text && inv.Company_ID == logIn.company
                               select inv).ToList();
                    if (da2[0].InvType == "SEZ Invoice")
                    {
                        SuppType = "SEZWOP";
                    }
                    else
                    if (da2[0].InvType == "SEZ Service Invoice")
                    {
                        SuppType = "SEZWOP";
                    }
                    else
                    if (da2[0].InvType == "Export Invoice")
                    {
                        SuppType = "EXPWOP";
                    }
                    else
                    {
                        SuppType = "B2B";
                    }
                    rep.SetParameterValue("SuppType", "Outward-Supply");
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

        private void btnGenerateIRN_CR_Click(object sender, EventArgs e)
        {
            if (txtAuthKeyValid.Text != "")
            {
                DateTime tokenValid = Convert.ToDateTime(txtAuthKeyValid.Text);
                if (tokenValid > DateTime.Now)
                {

                }
                else
                {
                    getAuthToken();
                }
            }
            else
            {
                getAuthToken();
            }
            getIRNNO_CN(txtAuthKey.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            GetIRNNOByDocNo();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            GenerateEWB(txtIRNNo.Text);
        }
        private async void GetEWBByIRNNo()
        {

            var comp = (from c1 in db.Costing_Units where c1.id == logIn.BU_ID select c1).ToList();
            string GSTIN = comp[0].GST_No;
            //DateTime.UtcNow nonse = DateTime(1970,1,1,0,0,0).to
            string url11 = "https://api.mastergst.com/einvoice/type/GETEWAYBILLIRN/version/V1_03?param1=" + txtIRNNo.Text + "&supplier_gstn=" + GSTIN + "&email=ssits.hyd%40gmail.com";

            //string url11 = "https://api.mastergst.com/einvoice/type/GETIRNBYDOCDETAILS/version/V1_03?param1=INV&email=ssits.hyd%40gmail.com";
            string sign = "0";
            //Uri ourURL = Uri(url11);
            WebRequest request = WebRequest.Create(url11);


            WebResponse myResponse;
            request.Method = "GET";

            request.Headers.Add("email", "ssits.hyd@gmail.com");
            request.Headers.Add("ip_address", ipAddr);
            request.Headers.Add("client_id", "f4a225ea-bf52-4d4d-a9d6-48e1e453b0d5");
            request.Headers.Add("client_secret", "425fceba-6a5b-4dc7-a83f-76d08b0ce72d");
            request.Headers.Add("username", UserName);
            request.Headers.Add("auth-token", txtAuthKey.Text);

            request.Headers.Add("gstin", GSTIN);



            // request.Headers.Add("Content-Type", "application/json; charset=utf-8");
            //request. = DataFormat.Json;


            myResponse = request.GetResponse();

            System.IO.StreamReader myreader = new System.IO.StreamReader(myResponse.GetResponseStream());
            WayBillData = myreader.ReadToEnd();
            //MessageBox.Show(streamtext);
            //JObject json = JObject.Parse(streamtext);
            //var EwbNo = (string)json.SelectToken("data.EwbNo");
            //var GenGstin = (string)json.SelectToken("data.GenGstin");
            //var EwbValidTill = (string)json.SelectToken("data.EwbValidTill");
            //var EwbDt = (string)json.SelectToken("data.EwbDt");
            //var Status = (string)json.SelectToken("data.Status");
            //txtAuthKey.Text = authToken;
            //txtAuthKeyValid.Text = authTokenValid;



            //return authToken;
        }

        private void button6_Click(object sender, EventArgs e)
        {

        }

        private void getIpAdress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                   // return ip.ToString();
                    ipAddr = ip.ToString();
                }
                //else
                //{
                  //  throw new Exception("No network adapters with an IPv4 address in the system!");
                //}
            }
            
        }
    
    }
}
