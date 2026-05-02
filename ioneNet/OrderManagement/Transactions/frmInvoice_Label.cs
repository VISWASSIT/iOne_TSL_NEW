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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Ione_DAL;
using Newtonsoft.Json.Linq;
using Syncfusion.Windows.Forms.Tools.Win32API;

namespace ioneNet.OrderManagement.Transactions
{
    public partial class frmInvoice_Label : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
      SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        public frmInvoice_Label()
        {
            InitializeComponent();
        }

        private void frmInvoice_Label_Load(object sender, EventArgs e)
        {
            try
            {
                DateTime podate;
                txtInvNo.Text = OrderManagement.Transactions.ListOfInvoices.SO_No;
                //txtCustomerName.Text = OrderManagement.Transactions.ListOfInvoices.Consignee;
                var Buyerblind = (from m in db.Invoice_Childs
                                  join s in db.Sale_Order_Masters on m.SO_Ref_No equals s.SO_NO 
                                  where m.Inv_No == txtInvNo.Text && m.Company_ID == logIn.company && s.Company_ID == logIn.company
                                  select new { m.Prod_Code,s.CustomerPONo,s.PODate,m.Prod_Grade,m.Product_Description }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    cmbItemCode.DataSource = Buyerblind;
                    cmbItemCode.ValueMember = "Prod_Code";
                    cmbItemCode.DisplayMember = "Prod_Code";
                    txtMtrlCode.Text = Buyerblind[0].Prod_Grade;
                    podate = Convert.ToDateTime(Buyerblind[0].PODate.ToString());
                    txtCustPONo.Text = Buyerblind[0].CustomerPONo + "  DT:" +  podate.ToString("dd/MM/yyy");
                    txtProductName.Text = Buyerblind[0].Product_Description;
                    //CmbConsigneeName.DataSource = Buyerblind;
                    //CmbConsigneeName.ValueMember = "ID";
                    //CmbConsigneeName.DisplayMember = "Customer_Alias_Name";

                }
                var da = (from obj in db.Invoice_Masters
                          join   C in db.Supplier_informations on obj.ConsigneeName equals C.ID
                          where obj.Inv_No == txtInvNo.Text && obj.Company_ID == logIn.company
                          select new { obj.InvDate, obj.TotalQty, C.Supplier_Alias_Name }).ToList();

                if (da.Count > 0)
                {

                    dateTimePicker1.Text = da[0].InvDate.ToString();
                    
                    txtQty.Text = da[0].TotalQty.ToString();
                    txtCustomerName.Text = da[0].Supplier_Alias_Name;

                }
                var da1 = (from obj in db.Invoice_labels
                          where obj.Inv_No == txtInvNo.Text && obj.Company_ID == logIn.company && obj.Item_Code ==Convert.ToInt32(cmbItemCode.Text)
                          select obj).ToList();

                if (da1.Count > 0)
                {
                    txtBatchNo.Text = da1[0].Batch_No;
                    txtProductName.Text = da1[0].Product_Description;
                    txtProdSize.Text = da1[0].Prod_Size;
                    txtMfgDate.Text = da1[0].Mfg_Date;
                    txtMtrlCode.Text = da1[0].Material_Code;
                    txtQty.Text = da1[0].Qty;
                }               
            }
            catch
            {

            }
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                Save();
                DialogResult result = MessageBox.Show("Click Yes To Print Labels","Lables", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    SqlCommand cmd1 = con.CreateCommand();



                    string path = Path.Combine(Directory.GetCurrentDirectory(), "InvoiceLabel.pdf");
                    //string path = @"D:\Invoice.pdf";
                    FileInfo fi1 = new FileInfo(path);


                    if (fi1.Exists)
                    {
                        fi1.Delete();
                    }




                    CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                    rep = new OrderManagement.Transactions.LabelPrint();
                    crConnectionInfo.ServerName = frmMain.ServerIP;
                    crConnectionInfo.DatabaseName = frmMain.Database;
                    crConnectionInfo.UserID = frmMain.DBUserID;
                    crConnectionInfo.Password = frmMain.Password;
                    //crConnectionInfo.ServerName = "43.255.152.26";
                    //crConnectionInfo.DatabaseName = "viswaSiOne";
                    //crConnectionInfo.UserID = "viswam";
                    //crConnectionInfo.Password = "Viswam@1972";
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

                    //rep.SetParameterValue("Creation_Company", frmLogin.Creation_Company);                 
                    rep.RecordSelectionFormula = "{ Invoice_labels.Inv_No} = '" + txtInvNo.Text + "' and { Invoice_labels.Item_Code} = " + cmbItemCode.Text + " and { Invoice_labels.Company_ID} = " + logIn.company + "";

                    string CAddr = "";
                    string CCity = "";
                    string cState = "";
                    String cGSTIN = "";
                    var da1 = (from so in db.Invoice_Masters
                    join c in db.Supplier_informations on so.ConsigneeName equals c.ID
                               where so.Inv_No == txtInvNo.Text && so.BU_ID == logIn.BU_ID && so.Status != 24
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
                            cState = (string)jsoncancel.SelectToken("State");
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
                    rep.SetParameterValue("Con_Address1", CAddr);
                    rep.SetParameterValue("Con_City", CCity);
                    rep.SetParameterValue("Con_State", cState);
                    rep.SetParameterValue("Con_GSTIN", cGSTIN);
                    ioneNet.Reports.RptViewer viewer = new ioneNet.Reports.RptViewer();

                    // rep.SetParameterValue("CopyName", "Original for Buyer/Duplicate for Transporter/Triplicate for Assessee/CTD Copy");
                    viewer.crystalReportViewer1.ReportSource = rep;
                    viewer.crystalReportViewer1.Refresh();
                    rep.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, path);
                    Process.Start(path);
                }
                    
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
        public void Save()
        {
            try
            {
                String myString = "";
                myString = txtInvNo.Text;
                if ((from u in db.Invoice_labels where u.Inv_No == myString && u.Company_ID == logIn.company && u.Item_Code == Convert.ToInt32(cmbItemCode.Text) select u).Count() > 0)
                {
                 
                    SqlCommand cmd1 = con.CreateCommand();
                    if (con.State != ConnectionState.Open)
                        con.Open();
                   
                   
                    cmd1.CommandText = "Delete from Invoice_labels where Inv_No = @InvNo and Item_Code = @item and  Company_ID =@company";
                    cmd1.Parameters.AddWithValue("@InvNo", txtInvNo.Text);
                    cmd1.Parameters.AddWithValue("@item", Convert.ToInt32(cmbItemCode.Text));
                    cmd1.Parameters.AddWithValue("@company", logIn.company);
                    cmd1.ExecuteNonQuery();
                    con.Close();
                }
                else
                {
                    
                    myString = txtInvNo.Text;

                }
                //if (frmGate.Modify.Contains(this.Text))
                //{

                // dgProducts.Enabled = false;
                //var S = db.Sale_Order_Masters.Where(w => w.SO_NO == myString && w.Company_ID == logIn.company).FirstOrDefault();
                Invoice_label S = new Invoice_label();
                {
                    S.Inv_No = txtInvNo.Text;
                    S.inv_date = dateTimePicker1.Value;
                    S.Customer_Name =txtCustomerName.Text;
                    S.Customer_PO = txtCustPONo.Text;
                    S.Item_Code = Convert.ToInt32(cmbItemCode.Text);
                    
                    S.Batch_No = txtBatchNo.Text;
                    S.Product_Description = txtProductName.Text;
                    S.Material_Code = txtMtrlCode.Text;
                    S.Mfg_Date = txtMfgDate.Text;
                    S.Prod_Size = txtProdSize.Text;
                    S.Qty = txtQty.Text;
                    
                    S.Company_ID = logIn.company;
                   
                    db.Invoice_labels.InsertOnSubmit(S);
                    db.SubmitChanges();
                }             
               
              
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbItemCode_Leave(object sender, EventArgs e)
        {
            try
            {
                if (cmbItemCode.Text !="")
                {
                    var da1 = (from obj in db.Invoice_labels
                               where obj.Inv_No == txtInvNo.Text && obj.Company_ID == logIn.company && obj.Item_Code == Convert.ToInt32(cmbItemCode.Text)
                               select obj).ToList();

                    if (da1.Count > 0)
                    {
                        txtBatchNo.Text = da1[0].Batch_No;
                        txtProductName.Text = da1[0].Product_Description;
                        txtProdSize.Text = da1[0].Prod_Size;
                        txtMfgDate.Text = da1[0].Mfg_Date;
                        txtMtrlCode.Text = da1[0].Material_Code;
                        txtQty.Text = da1[0].Qty;
                    }
                    else
                    {
                        var da2 = (from obj in db.Invoice_Childs
                                   where obj.Inv_No == txtInvNo.Text && obj.Company_ID == logIn.company && obj.Prod_Code == cmbItemCode.Text
                                   select obj).ToList();

                        if (da2.Count > 0)
                        {
                           // txtBatchNo.Text = da1[0].Batch_No;
                            txtProductName.Text = da2[0].Product_Description;
                            txtProdSize.Text = "";
                            txtMfgDate.Text = "";
                            txtMtrlCode.Text = da2[0].Prod_Grade;
                            txtQty.Text = da2[0].Qty.ToString();
                        }
                    }
                }
            }
            catch
            {

            }
        }
    }
}
