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
using System.Globalization;
using Ione_DAL;
using Syncfusion.Windows.Forms.Chart.SvgBase;
using Syncfusion.WinForms.DataGrid.Enums;
using Syncfusion.WinForms.DataGrid.Interactivity;
using Syncfusion.XlsIO.Implementation.XmlSerialization;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ioneNet.OrderManagement.Transactions
{
    public partial class frmNewOrder_Forging : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string Enq_NO, Item_Shape, MtrlGrade;      
        public static string SONo,ItemCode,OrdQty,CompStateCode;
        public static DataTable dtgetproducts = new DataTable();
        public static DataTable dtgetfinalprducts = new DataTable();
        decimal cgstPer, sgstPer, igstPer;
        public frmNewOrder_Forging()
        {
            InitializeComponent();
        }

        private void frmNewOrder_Load(object sender, EventArgs e)
        {
           // dpSODate.MinDate = logIn.fy_Start_Date;
           // dpSODate.MaxDate = logIn.fy_End_Date;
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

            bindCustomer();
            bindConsignee();
            //GetPendingQuotes();
            bindDroupDown_Lookup();
            if(logIn.company ==18)
            {
                dgProducts.Columns["Item_Grade"].HeaderText = "Process Involved";
            }
            else
            {
                dgProducts.Columns["Item_Grade"].HeaderText = "Model / Grade/Part No";
            }
            if(logIn.company == 25)

            {
                //tabControl1.TabPages[2].Show = false;

            }
            AutoincrementId();
            if (ListOfOrders.editMode == true)
            {
                bindedit();
            }
        }

       

       
       
        

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {


            try
            {
                if (CmbBuyerName.Text == string.Empty)
                {
                    MessageBox.Show("Customer Name Should Not Be Empty", "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CmbBuyerName.Focus();
                    return;
                }
                else if (CmbConsigneeName.Text == string.Empty)
                {
                    MessageBox.Show("Consignee Name Should Not Be Empty", "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    CmbConsigneeName.Focus();
                    return;
                }
                else if (cmbTaxClass.Text == string.Empty)
                {
                    MessageBox.Show("Tax Class Should Not Be Empty", "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbTaxClass.Focus();
                    return;
                }
                else if (txtCustPoNo.Text == string.Empty)
                {
                    MessageBox.Show("Customer PO No Should Not Be Empty");
                    txtCustPoNo.Focus();
                    return;
                }
                //else if (cmbQuotNo.Text == string.Empty)
                //{
                //    MessageBox.Show("Select Quotation No,");
                //    cmbQuotNo.Focus();
                //    return;
                //}
                else if (cmbStatus.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Status");
                    cmbStatus.Focus();
                    return;
                }

                else if (logIn.company == 18 && txtRMbasicPrice.Text == string.Empty)
                {
                    MessageBox.Show("Please Enter RM Basic Price Considred");
                    txtRMbasicPrice.Focus();
                    return;
                }
                else
                if (chkMultiLocation.Checked)
                {
                    if ((from u in db.Sales_Order_Delivery_Addresses where u.SO_NO == txtSoNo.Text && u.Company_ID == logIn.company select u).Count() ==0)
                    {

                        MessageBox.Show("You Have Selected Multi Loacation Delivery Option, But Not Entered the Location Details", "Order Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;

                    }
                }
                else
                if (dgProducts.Rows[0].Cells["Item_Code"].Value == null)
                {
                    MessageBox.Show("Atleast One Record To Be Entered To Proceed", "Order Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    SaveNew_Sql_proc();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void SaveNew_Sql_proc()
        {
            try
            {
                String myString = "";

                myString = txtSoNo.Text;
                string AmendNo = txtAmendNo.Text;
                if (txtSoNo.Text != "")
                {

                    if (ListOfOrders.var == "1")
                    {
                        var result = db.Sp_autoincrement_PO_AmendNo(logIn.company, logIn.BU_ID, ListOfOrders.SO_No);
                        txtAmendNo.Text = result.FirstOrDefault().PO_Amend_no;

                        //Set Previous Version Quote Status as Amended
                        var ci = db.Sale_Order_Masters.Where(w => w.SO_NO == myString && w.So_Amend_No == AmendNo && w.Company_ID == logIn.company && w.BU_ID == logIn.BU_ID).FirstOrDefault();
                        {

                          //  ci.is_amended = true;
                            ci.Modified_By = logIn.username + "-" + DateTime.Now;
                            db.SubmitChanges();

                        }

                    }

                    myString = txtSoNo.Text;
                    SqlCommand cmd = new SqlCommand("SaveSaleOrder", con);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PO_No", myString);
                    cmd.Parameters.AddWithValue("@PO_Date", dpSODate.Value);
                    cmd.Parameters.AddWithValue("@Po_Amend_No", (txtAmendNo.Text == "") ? "" : txtAmendNo.Text);
                    cmd.Parameters.AddWithValue("@Po_Amend_Date", dtAmendDate.Value);
                    cmd.Parameters.AddWithValue("@Repeat_Order", chkRepeatOrder.Checked);
                    cmd.Parameters.AddWithValue("@QutoNo", (cmbQuotNo.Text == "") ? "" : cmbQuotNo.Text);
                    //cmd.Parameters.AddWithValue("@QutoDate", dpPODate.Value);
                    //cmd.Parameters.AddWithValue("@Old_Ord_Ref", logIn.BU_ID);
                    cmd.Parameters.AddWithValue("@SEZ_Order", chkSEZOrder.Checked);
                    cmd.Parameters.AddWithValue("@Tax_Class", Convert.ToInt32(cmbTaxClass.SelectedValue.ToString()));

                    cmd.Parameters.AddWithValue("@SupplierName", Convert.ToInt32(CmbBuyerName.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@Cust_GST_No", (txtCustGSTNo.Text == "") ? "" : txtCustGSTNo.Text);
                    cmd.Parameters.AddWithValue("@Multi_Delivery", chkMultiLocation.Checked);                    
                    cmd.Parameters.AddWithValue("@Delivery_Date", dtDeliveryDate.Value);
                    cmd.Parameters.AddWithValue("@Customer_Po_No", (txtCustPoNo.Text == "") ? "" : txtCustPoNo.Text);
                    cmd.Parameters.AddWithValue("@Cust_Po_Date", dpPODate.Value);
                    cmd.Parameters.AddWithValue("@Ship_To", Convert.ToInt32(CmbConsigneeName.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@Sale_Office", Convert.ToInt32(cmbSaleOffice.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@Sale_Executive", Convert.ToInt32(cmbSaleExecutive.SelectedValue.ToString()));

                    cmd.Parameters.AddWithValue("@TotalQty", (txtTotalQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotalQty.Text));
                    cmd.Parameters.AddWithValue("@SubTotal", (txtSubTotal.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtSubTotal.Text));
                    cmd.Parameters.AddWithValue("@Tot_Discount", (txtTotDiscount.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotDiscount.Text));
                    cmd.Parameters.AddWithValue("@Tot_TaxableValue", (txtTot_TaxableValue.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTot_TaxableValue.Text));
                    cmd.Parameters.AddWithValue("@Tot_CGST_Amnt", (txtTot_CGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_CGST.Text));
                    cmd.Parameters.AddWithValue("@Tot_SGST_Amnt", (txtTot_SGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_SGST.Text));
                    cmd.Parameters.AddWithValue("@Tot_IGST_Amnt", (txtTot_IGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_IGST.Text));
                    cmd.Parameters.AddWithValue("@Tot_Ord_Value", (txtTot_OrderValue.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_OrderValue.Text));
                    cmd.Parameters.AddWithValue("@Price_Basis", Convert.ToInt32(cmbPriceBasis.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@Insurance_Scope", Convert.ToInt32(cmbInsurance.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@PaymentTerms", Convert.ToInt32(cmbPaymentTerms.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@Trasnport_Scope", Convert.ToInt32(cmbTransport_Scope.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@Transporter_Name", (cmbTransporter.Text == "") ? "" : cmbTransporter.Text);
                    cmd.Parameters.AddWithValue("@Customer_Contact", (txtCustomeContact.Text == null || txtCustomeContact.Text == "") ? "" : txtCustomeContact.Text);
                    cmd.Parameters.AddWithValue("@Other_Terms", (txtOtherTerms.Text == "") ? "" : txtOtherTerms.Text);
                    cmd.Parameters.AddWithValue("@Spl_Instructions", (txtSplInstructions.Text == "") ? "" : txtSplInstructions.Text);
                    cmd.Parameters.AddWithValue("@Status", Convert.ToInt32(cmbStatus.SelectedValue.ToString()));
                    cmd.Parameters.AddWithValue("@BU_ID", logIn.BU_ID);
                    cmd.Parameters.AddWithValue("@Company_ID", logIn.company);
                    cmd.Parameters.AddWithValue("@Created_By", lblCreatedBy.Text);
                    cmd.Parameters.AddWithValue("@Modified_By", logIn.username + "-" + DateTime.Now);
                    cmd.Parameters.AddWithValue("@Contact_EMail", (txtCustEmail.Text == null || txtCustEmail.Text == "") ? "" : txtCustEmail.Text);
                    cmd.Parameters.AddWithValue("@Freight", (txtFreight_Amt.Text == null || txtFreight_Amt.Text == "") ? 0 : Convert.ToDecimal(txtFreight_Amt.Text));
                    cmd.Parameters.AddWithValue("@Other_Charges", (txtOtherCharges.Text == null || txtOtherCharges.Text == "") ? 0 : Convert.ToDecimal(txtOtherCharges.Text));
                    cmd.Parameters.AddWithValue("@frieghtPerTon", (txtFreight_Rate.Text == null || txtFreight_Rate.Text == "") ? 0 : Convert.ToDecimal(txtFreight_Rate.Text));
                    cmd.Parameters.AddWithValue("@jobWorkOrder", chkJWORder.Checked);
                    cmd.Parameters.AddWithValue("@RM_Basic_Price", (txtRMbasicPrice.Text == null || txtRMbasicPrice.Text == "") ? 0 : Convert.ToDecimal(txtRMbasicPrice.Text));
                    cmd.Parameters.AddWithValue("@Packing_Charges", (txtPackingCharges.Text == null || txtPackingCharges.Text == "") ? 0 : Convert.ToDecimal(txtPackingCharges.Text));
                    cmd.Parameters.AddWithValue("@Insurance_Charges", (txtInsCharges.Text == null || txtInsCharges.Text == "") ? 0 : Convert.ToDecimal(txtInsCharges.Text));
                    cmd.Parameters.AddWithValue("@Payment_Terms_Custom", (txtCustomerPaymentTerms.Text == null || txtCustomerPaymentTerms.Text == "") ? "" : txtCustomerPaymentTerms.Text);

                    cmd.Parameters.AddWithValue("@delAddress", (txtConAddress.Text == "") ? "" : txtConAddress.Text);
                    cmd.Parameters.AddWithValue("@delGSTIN", (txtConGSTNo.Text == "") ? "" : txtConGSTNo.Text);
                    string Tests_Required = "";
                    for (int i = 0; i < chkTests.Items.Count; i++)
                    {
                        if (chkTests.GetItemChecked(i))
                        {
                            if (Tests_Required != "")
                            {
                                Tests_Required = Tests_Required + "," + chkTests.Items[i].ToString();
                            }
                            else
                            {
                                Tests_Required = chkTests.Items[i].ToString();
                            }
                        }
                    }
                    cmd.Parameters.AddWithValue("@Tests_Required", Tests_Required);

                    string Standards_To_Refer = "";
                    for (int i = 0; i < chkStdRefer.Items.Count; i++)
                    {
                        if (chkStdRefer.GetItemChecked(i))
                        {
                            if (Standards_To_Refer != "")
                            {
                                Standards_To_Refer = Standards_To_Refer + "," + chkStdRefer.Items[i].ToString();
                            }
                            else
                            {
                                Standards_To_Refer = chkStdRefer.Items[i].ToString();
                            }
                        }
                    }
                   

                    cmd.Parameters.AddWithValue("@Standards_To_Refer", Standards_To_Refer);

                    cmd.Parameters.AddWithValue("@RM_Available", (cmbRMAvailable.Text == "") ? "" : cmbRMAvailable.Text);
                    //cmd.Parameters.AddWithValue("@RM_Risk_Point", txtRMRemarks.Text);
                    cmd.Parameters.AddWithValue("@Mfg_Feasibility", (cmbMfgFeasibility.Text == "") ? "" : cmbMfgFeasibility.Text);
                    cmd.Parameters.AddWithValue("@Spec_Is_Clear", (cmbSpecClear.Text == "") ? "" : cmbSpecClear.Text);
                    //cmd.Parameters.AddWithValue("@Enq_No", (cmbSpecClear.Text == "") ? "" : cmbSpecClear.Text);
                  //  cmd.Parameters.AddWithValue("@Coating", (cmbSpecClear.Text == "") ? "" : cmbSpecClear.Text);
                    //cmd.Parameters.AddWithValue("@Packing", (cmbSpecClear.Text == "") ? "" : cmbSpecClear.Text);
                   // cmd.Parameters.AddWithValue("@Payment_Terms_Custom", (txtcy.Text == "") ? "" : cmbSpecClear.Text);


                    string Prod_Code = "";
                    string Product_Description = "";
                    string Prod_Spec = "";
                    string Prod_Grade = "";
                    string Uom = "";
                    string PR_Qty = "";
                    string Qty = "";
                    string Price = "";
                    string Amount = "";
                    string Disc_Per = "";
                    string Disc_Amount = "";
                    string Taxable_Value = "";
                    string CGST_Per = "";
                    string SGST_Per = "";
                    string IGST_Per = "";
                    string CGST_Amnt = "";
                    string SGST_Amnt = "";
                    string IGST_Amnt = "";
                    string Net_Amount = "";
                    string Quot_Master_ID = "";
                    string Remarks = "";
                    string ProdSno = "";
                    string Company_ID = "";
                    string Tole_Qty = "";
                    string Int_Prod_Code = "";
                    string HSN_Code = "";
                    string Del_Date = "";
                    string enq_Master_ID = "";
                    string enq_item_no = "";
                    

                    int rowcount = 0;

                    for (int i = 0; i < dgProducts.RowCount - 1; i++)
                    {

                        Prod_Code = Prod_Code + Convert.ToString(dgProducts.Rows[i].Cells["Item_Code"].Value).PadRight(14);
                        Product_Description = ""; //   Product_Description + Convert.ToString(dgProducts.Rows[i].Cells["Item_Description"].Value).PadRight(50);
                        //Prod_Spec = Prod_Spec + Convert.ToString(dgProducts.Rows[i].Cells["Item_spec"].Value).PadRight(50);
                        Prod_Grade = ""; // Prod_Grade + Convert.ToString(dgProducts.Rows[i].Cells["Item_Grade"].Value).PadRight(50);
                        Uom = Uom + Convert.ToString(dgProducts.Rows[i].Cells["uom"].Value).PadRight(14);
                        //PR_Qty = PR_Qty + Convert.ToString(dgProducts.Rows[i].Cells["PR_Qty"].Value).PadRight(14);
                        Qty = Qty + Convert.ToString(dgProducts.Rows[i].Cells["Qty"].Value).PadRight(14);
                        Price = Price + Convert.ToString(dgProducts.Rows[i].Cells["Basic_Price"].Value).PadRight(14);
                        Amount = Amount + Convert.ToString(dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value).PadRight(14);
                        Disc_Per = Disc_Per + Convert.ToString(dgProducts.Rows[i].Cells["Disc_Per"].Value).PadRight(14);
                        Disc_Amount = Disc_Amount + Convert.ToString(dgProducts.Rows[i].Cells["Disc_Amt"].Value).PadRight(14);
                        Taxable_Value = Taxable_Value + Convert.ToString(dgProducts.Rows[i].Cells["Taxable_Value"].Value).PadRight(14);
                        CGST_Per = CGST_Per + Convert.ToString(dgProducts.Rows[i].Cells["CGST_Per"].Value).PadRight(14);
                        SGST_Per = SGST_Per + Convert.ToString(dgProducts.Rows[i].Cells["SGST_Per"].Value).PadRight(14);
                        IGST_Per = IGST_Per + Convert.ToString(dgProducts.Rows[i].Cells["IGST_Per"].Value).PadRight(14);
                        CGST_Amnt = CGST_Amnt + Convert.ToString(dgProducts.Rows[i].Cells["CGST_Amt"].Value).PadRight(14);
                        SGST_Amnt = SGST_Amnt + Convert.ToString(dgProducts.Rows[i].Cells["SGST_Amt"].Value).PadRight(14);
                        IGST_Amnt = IGST_Amnt + Convert.ToString(dgProducts.Rows[i].Cells["IGST_Amt"].Value).PadRight(14);
                        Net_Amount = Net_Amount + Convert.ToString(dgProducts.Rows[i].Cells["Total_Amount"].Value).PadRight(14);
                        Quot_Master_ID = Quot_Master_ID + Convert.ToString(dgProducts.Rows[i].Cells["Quot_No"].Value).PadRight(14);
                        Remarks = Remarks + Convert.ToString(dgProducts.Rows[i].Cells["Remarks"].Value).PadRight(14);
                        Tole_Qty = Tole_Qty+ Convert.ToString(dgProducts.Rows[i].Cells["Prod_Tole_Qty"].Value).PadRight(14);
                        Int_Prod_Code = Int_Prod_Code+ Convert.ToString(dgProducts.Rows[i].Cells["Int_Prod_Code"].Value).PadRight(14);
                        HSN_Code = HSN_Code+ Convert.ToString(dgProducts.Rows[i].Cells["HSN_Code"].Value).PadRight(14);
                        Del_Date = Del_Date+ Convert.ToString(dgProducts.Rows[i].Cells["Delivery_Date"].Value).PadRight(14);
                        enq_Master_ID = enq_Master_ID + Convert.ToString(dgProducts.Rows[i].Cells["EnqNo"].Value).PadRight(14);
                        enq_item_no = enq_item_no + Convert.ToString(dgProducts.Rows[i].Cells["Enq_Item_ID"].Value).PadRight(14);


                        //ProdSno = i + 1;
                        //Company_ID = logIn.company;
                        rowcount += 1;
                    }

                    cmd.Parameters.AddWithValue("@txt_Prod_Code", Prod_Code);
                    cmd.Parameters.AddWithValue("@txt_Product_Description", Product_Description);
                    //cmd.Parameters.AddWithValue("@txt_Prod_Spec", Prod_Spec);
                    cmd.Parameters.AddWithValue("@txt_Prod_Grade", Prod_Grade);
                    cmd.Parameters.AddWithValue("@txt_Uom", Uom);
                    //cmd.Parameters.AddWithValue("@txt_PR_Qty", PR_Qty);
                    cmd.Parameters.AddWithValue("@txt_Qty", Qty);
                    cmd.Parameters.AddWithValue("@txt_Price", Price);
                    cmd.Parameters.AddWithValue("@txt_Amount", Amount);
                    cmd.Parameters.AddWithValue("@txt_Disc_Per", Disc_Per);
                    cmd.Parameters.AddWithValue("@txt_Disc_Amount", Disc_Amount);
                    cmd.Parameters.AddWithValue("@txt_Taxable_Value", Taxable_Value);
                    cmd.Parameters.AddWithValue("@txt_CGST_Per", CGST_Per);
                    cmd.Parameters.AddWithValue("@txt_CGST_Amnt", CGST_Amnt);
                    cmd.Parameters.AddWithValue("@txt_SGST_Amnt", SGST_Amnt);
                    cmd.Parameters.AddWithValue("@txt_SGST_Per", SGST_Per);
                    cmd.Parameters.AddWithValue("@txt_IGST_Per", IGST_Per);
                    cmd.Parameters.AddWithValue("@txt_IGST_Amnt", IGST_Amnt);
                    cmd.Parameters.AddWithValue("@txt_Net_Amount", Net_Amount);
                    cmd.Parameters.AddWithValue("@txt_Remarks", Remarks);
                    cmd.Parameters.AddWithValue("@txt_Tole_Qty", Tole_Qty);
                    cmd.Parameters.AddWithValue("@txt_Del_Date", Del_Date);
                    cmd.Parameters.AddWithValue("@txt_HSN_Code", HSN_Code);
                    cmd.Parameters.AddWithValue("@txt_Int_Prod_Code", Int_Prod_Code);
                    cmd.Parameters.AddWithValue("@txt_Quot_NO", Quot_Master_ID);
                    cmd.Parameters.AddWithValue("@txt_Enq_NO", enq_Master_ID);
                    cmd.Parameters.AddWithValue("@txt_Enq_Item_NO", enq_item_no);

                    cmd.Parameters.AddWithValue("@gridcount", rowcount);

                    try
                    {
                        con.Open();
                        int result = cmd.ExecuteNonQuery();

                        if (Convert.ToBoolean(result))
                        {
                            MessageBox.Show("Record has been successfully saved..");
                        }


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        con.Close();
                    }


                }
                else
                {
                    //AutoincrementId();
                    //myString = txtSoNo.Text;

                }
                //if (frmGate.Modify.Contains(this.Text))
                //{

                //dgProducts.Enabled = false;
                //var S = db.Sale_Order_Masters.Where(w => w.SO_NO == myString && w.Company_ID == logIn.company).FirstOrDefault();
                //Purchase_Order_Master S = new Purchase_Order_Master();
                //{
                //    S.PO_NO = myString;
                //    S.PO_Date = dpSODate.Value;
                //    S.Purchase_Bases = cmbBasis.Text;
                //    S.Contact_Person = (txtcontactperson.Text == null || txtcontactperson.Text == "") ? "" : txtcontactperson.Text;
                //    S.Contact_EMail = (txtcontactEmail.Text == null || txtcontactEmail.Text == "") ? "" : txtcontactEmail.Text;
                //    S.SupplierName = Convert.ToInt32(CmbBuyerName.SelectedValue.ToString());
                //    //S. = txtcontactEmail.Text;

                //    S.Freight = (txtFrieght.Text == null || txtFrieght.Text == "") ? 0 : Convert.ToDecimal(txtFrieght.Text);
                //    S.Other_Charges = (txtOtherCharges.Text == null || txtOtherCharges.Text == "") ? 0 : Convert.ToDecimal(txtOtherCharges.Text);
                //    S.Packing_Charges = (txtPackingCharges.Text == null || txtPackingCharges.Text == "") ? 0 : Convert.ToDecimal(txtPackingCharges.Text);

                //    S.ConsigneeName = Convert.ToInt32(1);
                //    S.Status = Convert.ToInt32(cmbStatus.SelectedValue.ToString());
                //    S.TotalQty = (txtTotalQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotalQty.Text);
                //    S.SubTotal = (txtSubTotal.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtSubTotal.Text);
                //    S.Tot_Discount = (txtTotDiscount.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotDiscount.Text);
                //    S.Tot_TaxableValue = (txtTot_TaxableValue.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTot_TaxableValue.Text);
                //    S.Supplier_QutoNo = (txtCustPoNo.Text == "") ? "" : txtCustPoNo.Text;
                //    S.Supplier_QutoDate = dpPODate.Value;
                //    S.Cust_GST_No = (txtCustGSTNo.Text == "") ? "" : txtCustGSTNo.Text;
                //    S.Tot_CGST_Amnt = (txtTot_CGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_CGST.Text);
                //    S.Tot_SGST_Amnt = (txtTot_SGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_SGST.Text);
                //    S.Tot_IGST_Amnt = (txtTot_IGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_IGST.Text);
                //    S.Tot_Ord_Value = (txtTot_OrderValue.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_OrderValue.Text);
                //    S.Delivery_Date = dtDeliveryDate.Value;
                //    S.Qty_Tolerence = (txtqtytolerence.Text == "") ? "" : txtqtytolerence.Text;
                //    S.Po_Amend_No = (txtAmendNo.Text == "") ? "" : txtAmendNo.Text;
                //    S.Po_Amend_Date = dtAmendDate.Value;
                //    if (cmbShipTo.SelectedValue == null)
                //    {
                //        S.Ship_To = 0;
                //    }
                //    else
                //    {
                //        S.Ship_To = Convert.ToInt32(cmbShipTo.SelectedValue.ToString());
                //    }
                //    //S.SaleExecutive = Convert.ToInt32(cmbSaleExecutive.SelectedValue.ToString());
                //    S.Price_Basis = Convert.ToInt32(cmbPriceBasis.SelectedValue.ToString());
                //    S.Insurance_Scope = Convert.ToInt32(cmbInsurance.SelectedValue.ToString());
                //    S.PaymentTerms = Convert.ToInt32(cmbPaymentTerms.SelectedValue.ToString());
                //    S.Trasnport_Scope = Convert.ToInt32(cmbTransport_Scope.SelectedValue.ToString());
                //    S.Transporter_Name = (cmbTransporter.Text == "") ? "" : cmbTransporter.Text;
                //    S.Multi_Loc_Delivery = (chkMultiLocation.Checked == true) ? true : false;
                //    S.Desp_Mode = Convert.ToInt32(cmbModeofDesp.SelectedValue.ToString());
                //    //S.Repeat_Order = (chkRepeatOrder.Checked == true) ? true : false;
                //    //S.Old_Ord_Ref = (txt.Text == "") ? "" : txtOtherTerms.Text;
                //    //S.SEZ_Order = (chkSEZOrder.Checked == true) ? true : false;
                //    S.LD_Date = dtLDClause.Value;
                //    S.LD_Description = (txtLDClause.Text == "") ? "" : txtLDClause.Text;
                //    S.LD_Clause_applicable = chkLDClause.Checked;
                //    S.Import_PO = chkImportPO.Checked;
                //    S.Customer_Contact = (txtCustomeContact.Text == "") ? "" : txtCustomeContact.Text;
                //    S.Other_Terms = (txtOtherTerms.Text == "") ? "" : txtOtherTerms.Text;
                //    S.Spl_Instructions = (txtSplInstructions.Text == "") ? "" : txtSplInstructions.Text;
                //    S.Warrenty = (txtWarrenty.Text == "") ? "" : txtWarrenty.Text;
                //    S.FCurrency = (cmbFCurrency.Text == "") ? "" : cmbFCurrency.Text;
                //    S.ExchangeRate = (txtExchangeRate.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtExchangeRate.Text);
                //    S.is_amended = false;
                //    S.BU_ID = logIn.BU_ID;
                //    S.Company_ID = logIn.company;
                //    S.Created_By = lblCreatedBy.Text;
                //    S.Modified_By = logIn.username + "-" + DateTime.Now;
                //    db.Purchase_Order_Masters.InsertOnSubmit(S);
                //    db.SubmitChanges();
                //}
                ////db.Transaction = transaction;
                //for (int i = 0; i < dgProducts.RowCount - 1; i++)
                //{
                //    Purchase_Order_Child SC = new Purchase_Order_Child();
                //    var d1 = (from a in db.Purchase_Order_Masters where a.PO_NO == myString && a.Po_Amend_No == txtAmendNo.Text && a.Company_ID == logIn.company select new { a.Id }).ToList();
                //    SC.PO_Master_ID = d1[0].Id;

                //    SC.PO_NO = myString;
                //    //SC.Prod_Code = (dgProducts.Rows[i].Cells["Item_Code"].Value == null) ? "" : Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value).ToString());
                //    SC.Prod_Code = (dgProducts.Rows[i].Cells["Item_Code"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value);

                //    SC.Product_Description = (dgProducts.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value).ToString();
                //    SC.Prod_Spec = (dgProducts.Rows[i].Cells["Item_spec"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_spec"].Value).ToString();
                //    SC.Prod_Grade = (dgProducts.Rows[i].Cells["Item_Grade"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value).ToString();
                //    SC.Uom = (dgProducts.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["uom"].Value.ToString();
                //    SC.PR_Qty = (dgProducts.Rows[i].Cells["PR_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["PR_Qty"].Value);
                //    SC.Qty = (dgProducts.Rows[i].Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty"].Value);
                //    SC.Price = (dgProducts.Rows[i].Cells["Basic_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Basic_Price"].Value);

                //    SC.Amount = (dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value);
                //    SC.Disc_Per = (dgProducts.Rows[i].Cells["Disc_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Per"].Value);
                //    SC.Disc_Amount = (dgProducts.Rows[i].Cells["Disc_Amt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Amt"].Value);
                //    SC.Taxable_Value = (dgProducts.Rows[i].Cells["Taxable_Value"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Taxable_Value"].Value);
                //    SC.CGST_Per = (dgProducts.Rows[i].Cells["CGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Per"].Value);
                //    SC.SGST_Per = (dgProducts.Rows[i].Cells["SGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Per"].Value);
                //    SC.IGST_Per = (dgProducts.Rows[i].Cells["IGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Per"].Value);
                //    SC.CGST_Amnt = (dgProducts.Rows[i].Cells["CGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Amt"].Value);
                //    SC.SGST_Amnt = (dgProducts.Rows[i].Cells["SGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Amt"].Value);
                //    SC.IGST_Amnt = (dgProducts.Rows[i].Cells["IGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Amt"].Value);
                //    SC.Net_Amount = (dgProducts.Rows[i].Cells["Total_Amount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Total_Amount"].Value);
                //    SC.PR_NO = (dgProducts.Rows[i].Cells["PR_NO"].Value == null) ? "" : (dgProducts.Rows[i].Cells["PR_NO"].Value).ToString();
                //    SC.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value).ToString();
                //    SC.ProdSno = i + 1;
                //    SC.Company_ID = logIn.company;
                //    db.Purchase_Order_Childs.InsertOnSubmit(SC);
                //}
                //db.SubmitChanges();
                ////transaction.Commit();               
                //MessageBox.Show("Record Saved / Updated Successfully With Transaction Ref No : " + txtSoNo.Text);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }
        public void bindCustomer()
        {
            try
            {
                var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company && m.Status==1 select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    CmbBuyerName.DataSource = Buyerblind;
                    CmbBuyerName.ValueMember = "ID";
                    CmbBuyerName.DisplayMember = "Supplier_Name";
                  
                }
                //if (CmbBuyerName.Items.Count > 0)
                    CmbBuyerName.SelectedIndex = -1;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void bindConsignee()
        {
            try
            {
                var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    //CmbBuyerName.DataSource = Buyerblind;
                    //CmbBuyerName.ValueMember = "ID";
                    //CmbBuyerName.DisplayMember = "Customer_Alias_Name";

                    CmbConsigneeName.DataSource = Buyerblind;
                    CmbConsigneeName.ValueMember = "ID";
                    CmbConsigneeName.DisplayMember = "Supplier_Name";

                }
                //if (CmbBuyerName.Items.Count > 0)
                    CmbConsigneeName.SelectedIndex = -1;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void GetPendingQuotes()
        {
            try
            {

                SqlCommand cmd2 = new SqlCommand("SP_GetQuotesForSaleOrder", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);
                

                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataSet ds2 = new DataSet();
                DataTable ds2 = new DataTable();
                // da2.Fill(ds2, "x");
                da2.Fill(ds2);
                for (int i = 0; i < ds2.Rows.Count; i++)
                {
                    //cmbQuotNo.Items.Add(ds2.Rows[i]["Quot_NO"].ToString());
                }
                //var d = (from data in db.SP_GetQuotesForSaleOrder(logIn.company) select new { data.Quot_NO }).Distinct().ToList();

                //if (d.Count > 0)
                ////    var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company && m.Status == 1 select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                ////if (Buyerblind.Count > 0)
                ////{
                //    cmbQuotNo.DataSource = d;
                //    CmbBuyerName.ValueMember = "ID";
                //    CmbBuyerName.DisplayMember = "Supplier_Name";

                //}
                ////if (CmbBuyerName.Items.Count > 0)
                //cmbQuotNo.SelectedIndex = -1;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void bindDroupDown_Lookup()
        {
            try
            {
                //Price Basis
                var PBasis = (from m in db.Attributes_Datas where m.Head_Name == "Price Basis" select new { m.ID, m.Descr }).Distinct().ToList();
                if (PBasis.Count > 0)
                {
                    cmbPriceBasis.DataSource = PBasis;
                    cmbPriceBasis.ValueMember = "ID";
                    cmbPriceBasis.DisplayMember = "Descr";                    
                }
                //Payment Terms
                var PTerms = (from m in db.Attributes_Datas where m.Head_Name == "Payment Terms" select new { m.ID, m.Descr }).Distinct().ToList();
                if (PTerms.Count > 0)
                {
                    cmbPaymentTerms.DataSource = PTerms;
                    cmbPaymentTerms.ValueMember = "ID";
                    cmbPaymentTerms.DisplayMember = "Descr";
                }

                //Status
                var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Trans" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pStatus.Count > 0)
                {
                    cmbStatus.DataSource = pStatus;
                    cmbStatus.ValueMember = "ID";
                    cmbStatus.DisplayMember = "Descr";
                }
                //Insurance
                var pIns = (from m in db.Attributes_Datas where m.Head_Name == "Insurance" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pIns.Count > 0)
                {
                    cmbInsurance.DataSource = pIns;
                    cmbInsurance.ValueMember = "ID";
                    cmbInsurance.DisplayMember = "Descr";
                }

                //Transportation
                var pTrans = (from m in db.Attributes_Datas where m.Head_Name == "Transportation" select new { m.ID, m.Descr }).Distinct().ToList();
                if (pTrans.Count > 0)
                {
                    cmbTransport_Scope.DataSource = pTrans;
                    cmbTransport_Scope.ValueMember = "ID";
                    cmbTransport_Scope.DisplayMember = "Descr";
                }

                
               
                //Sales Office
                var SO = (from m in db.Attributes_Datas where m.Head_Name == "Sales Office" select new { m.ID, m.Descr }).Distinct().ToList();
                if (SO.Count > 0)
                {
                    cmbSaleOffice.DataSource = SO;
                    cmbSaleOffice.ValueMember = "ID";
                    cmbSaleOffice.DisplayMember = "Descr";
                }

                //Sales Office
                var SM = (from m in db.Sales_Men_Informations where m.Company_ID == logIn.company select new { m.Id, m.Salesmen_Code }).Distinct().ToList();
                if (SM.Count > 0)
                {
                    cmbSaleExecutive.DataSource = SM;
                    cmbSaleExecutive.ValueMember = "Id";
                    cmbSaleExecutive.DisplayMember = "Salesmen_Code";
                }

                //Bind Tax Class
                var bindLoc = (from m in db.Tax_Class_Masters
                               where m.Company_ID == logIn.company
                               select new
                               {
                                   m.Tax_Class_Name,
                                   m.ID,
                               }).ToList();

                if (bindLoc.Count > 0)
                {
                    cmbTaxClass.DisplayMember = "Tax_Class_Name";
                    cmbTaxClass.ValueMember = "ID";
                    cmbTaxClass.DataSource = bindLoc;

                }

                using (SqlCommand cmd = new SqlCommand("SELECT distinct TestName,id FROM [TestMaster] where company_id = @CompName  order by id", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@CompName", logIn.company);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                chkTests.Items.Add(dt.Rows[i]["TestName"].ToString());
                            }

                        }
                    }
                }
                using (SqlCommand cmd = new SqlCommand("SELECT distinct StandardCode,id FROM [Mtrl_Standards] where company_id = @CompName  order by id", con))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@CompName", logIn.company);
                    using (SqlDataAdapter sda = new SqlDataAdapter(cmd))
                    {
                        using (DataTable dt = new DataTable())
                        {
                            sda.Fill(dt);
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                chkStdRefer.Items.Add(dt.Rows[i]["StandardCode"].ToString());
                            }

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void AutoincrementId()
        {
            try
            {

                var result = db.Sp_autoincrement_SaleOrder_forging (logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, logIn.BU_ID);
                txtSoNo.Text = result.FirstOrDefault().So_no;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgProducts_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            { 
                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].HeaderText;
                TextBox tb3 = e.Control as TextBox;
                tb3.AutoCompleteMode = AutoCompleteMode.None;                
                if (tb3 != null && columnName == "UOM")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }
                if (tb3 != null && columnName == "Item Description")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
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
        public void addItems(AutoCompleteStringCollection coll)
        {
            try
            {
                DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];

                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].HeaderText;

                if (columnName == "UOM")
                {
                    var Prodname = (from d in db.UoM_Masters select new { d.Uom_Descr }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Uom_Descr");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Uom_Descr);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
                    }
                }
                else
                {
                    if (columnName == "Item Description")
                    {
                        var Prodname = (from d in db.Get_ProductsList(logIn.company,0,null) select new { d.Prod_Name }).ToList();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Prod_Name");
                        foreach (var item in Prodname)
                        {
                            dt.Rows.Add(item.Prod_Name);
                        }
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            coll.Add(dt.Rows[i][0].ToString());
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CmbBuyerName_Leave(object sender, EventArgs e)
        {
            try
            {
                //int s = CmbBuyerName.SelectedIndex;

                if (CmbBuyerName.Text != "")
                   
                {
                    //var State = (from c in db.Supplier_informations
                    //             join a in db.AccountMasters on c.Supplier_Id equals a.AccCode
                    //             where c.ID == Convert.ToInt32(CmbBuyerName.SelectedValue)
                    //             select new { c.GSTIN_NO, a.id, c.StateCode }).ToList();
                    //if (State.Count > 0)
                    //{
                    //    txtCustGSTNo.Text = State[0].GSTIN_NO;
                    //    txtCustStateCode.Text = State[0].StateCode;

                    //    //Get Order data and pending receivables

                    //    DateTime t = dpSODate.Value;
                    //    string t1 = t.ToString("dd/MMM/yyyy");
                    //    var getBal = (from b in db.GetAccountBalance(logIn.company, t, State[0].id, 1)
                    //                  select new { b.Balance, b.BalType }).FirstOrDefault();
                    //    if (getBal != null)
                    //    {
                    //        linkLabel5.Text = getBal.Balance.ToString() + getBal.BalType.ToString();
                    //    }
                    //    else
                    //    {
                    //        linkLabel5.Text = "0";
                    //    }
                    //    var getOrd = (from b in db.PreviousHistory(logIn.company, Convert.ToInt32(CmbBuyerName.SelectedValue))
                    //                  select new { b.TotalOrders, b.PendingOrderValue }).FirstOrDefault();
                    //    if (getOrd != null)
                    //    {
                    //        linkLabel2.Text = getOrd.TotalOrders.ToString();
                    //        linkLabel4.Text = getOrd.PendingOrderValue.ToString();
                    //    }
                    //    else
                    //    {
                    //        linkLabel2.Text = "0";
                    //        linkLabel4.Text = "0";
                    //    }
                    //}
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dgProducts_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
               
                var d1 = (from a in db.Company_Infos where a.Id == logIn.company select new { a.State_Code }).ToList();
                if (d1.Count > 0)
                {
                    CompStateCode = d1[0].State_Code;
                }
                DataTable dtexisting = new DataTable();
                if (e.KeyCode == Keys.F2)
                {
                    ioneNet.Masters.ProdSearch form = new ioneNet.Masters.ProdSearch();
                    ioneNet.Masters.ProdSearch.frmName = "SOrder";                                   
                    form.ShowDialog();
                    if (dgProducts.Rows.Count > 1)
                    {
                        dtexisting.Rows.Clear();
                        dtexisting.Columns.Clear();
                        dtexisting.Columns.Add("Item_Code", typeof(string));
                        dtexisting.Columns.Add("Int_Prod_Code", typeof(string));
                        dtexisting.Columns.Add("Item_Description", typeof(string));
                        dtexisting.Columns.Add("Item_Grade", typeof(string));
                        dtexisting.Columns.Add("HSN_Code", typeof(string));                        
                        dtexisting.Columns.Add("UOM", typeof(string));
                        dtexisting.Columns.Add("Qty", typeof(decimal));
                        dtexisting.Columns.Add("Basic_Price", typeof(decimal));
                        dtexisting.Columns.Add("Amt_Before_Disc", typeof(decimal));
                        dtexisting.Columns.Add("Disc_Per", typeof(decimal));
                        dtexisting.Columns.Add("Disc_Amt", typeof(decimal));
                        dtexisting.Columns.Add("Taxable_Value", typeof(decimal));
                        dtexisting.Columns.Add("CGST_Per", typeof(decimal));
                        dtexisting.Columns.Add("CGST_Amt", typeof(decimal));
                        dtexisting.Columns.Add("SGST_Per", typeof(decimal));
                        dtexisting.Columns.Add("SGST_Amt", typeof(decimal));
                        dtexisting.Columns.Add("IGST_Per", typeof(decimal));
                        dtexisting.Columns.Add("IGST_Amt", typeof(decimal));
                        dtexisting.Columns.Add("Total_Amount", typeof(decimal));
                        dtexisting.Columns.Add("Prod_Tole_Qty", typeof(string));
                        dtexisting.Columns.Add("Delivery_Date", typeof(string));
                        dtexisting.Columns.Add("Remarks", typeof(string));

                        for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                        {
                            DataRow dr;
                            dr = dtexisting.NewRow();
                            dr["Item_Code"] = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                            dr["Int_Prod_Code"] = dgProducts.Rows[i].Cells["Int_Prod_Code"].Value.ToString();
                            dr["Item_Description"] = dgProducts.Rows[i].Cells["Item_Description"].Value.ToString();
                            dr["Item_Grade"] = dgProducts.Rows[i].Cells["Item_Grade"].Value.ToString();
                            dr["HSN_Code"] = dgProducts.Rows[i].Cells["HSN_Code"].Value.ToString();
                            dr["UOM"] = dgProducts.Rows[i].Cells["UOM"].Value.ToString();
                            dr["Qty"] = dgProducts.Rows[i].Cells["Qty"].Value.ToString();
                            dr["Basic_Price"] = dgProducts.Rows[i].Cells["Basic_Price"].Value.ToString();
                            dr["Amt_Before_Disc"] = dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value.ToString();
                            dr["Disc_Per"] = dgProducts.Rows[i].Cells["Disc_Per"].Value.ToString();
                            dr["Disc_Amt"] = dgProducts.Rows[i].Cells["Disc_Amt"].Value.ToString();
                            dr["Taxable_Value"] = dgProducts.Rows[i].Cells["Taxable_Value"].Value.ToString();
                            dr["CGST_Per"] = dgProducts.Rows[i].Cells["CGST_Per"].Value.ToString();
                            dr["CGST_Amt"] = dgProducts.Rows[i].Cells["CGST_Amt"].Value.ToString();
                            dr["SGST_Per"] = dgProducts.Rows[i].Cells["SGST_Per"].Value.ToString();
                            dr["SGST_Amt"] = dgProducts.Rows[i].Cells["SGST_Amt"].Value.ToString();
                            dr["IGST_Per"] = dgProducts.Rows[i].Cells["IGST_Per"].Value.ToString();
                            dr["IGST_Amt"] = dgProducts.Rows[i].Cells["IGST_Amt"].Value.ToString();
                            dr["Total_Amount"] = dgProducts.Rows[i].Cells["Total_Amount"].Value.ToString();
                            dr["Prod_Tole_Qty"] = dgProducts.Rows[i].Cells["Prod_Tole_Qty"].Value.ToString();  
                            dr["Delivery_Date"] = dgProducts.Rows[i].Cells["Delivery_Date"].Value.ToString();  
                            dr["Remarks"] = dgProducts.Rows[i].Cells["Remarks"].Value.ToString();
                            dtexisting.Rows.Add(dr);

                        }
                        dtexisting.AcceptChanges();
                    }






                    if (ioneNet.Masters.ProdSearch.dtgetproducts.Rows.Count > 0)
                    {
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Item_Code", typeof(string));
                        dt.Columns.Add("Int_Prod_Code", typeof(string));
                        dt.Columns.Add("Item_Description", typeof(string));
                        dt.Columns.Add("Item_Grade", typeof(string));
                        dt.Columns.Add("HSN_Code", typeof(string));
                        dt.Columns.Add("UOM", typeof(string));
                        dt.Columns.Add("Qty", typeof(decimal));
                        dt.Columns.Add("Basic_Price", typeof(decimal));
                        dt.Columns.Add("Amt_Before_Disc", typeof(decimal));
                        dt.Columns.Add("Disc_Per", typeof(decimal));
                        dt.Columns.Add("Disc_Amt", typeof(decimal));
                        dt.Columns.Add("Taxable_Value", typeof(decimal));
                        dt.Columns.Add("CGST_Per", typeof(decimal));
                        dt.Columns.Add("CGST_Amt", typeof(decimal));
                        dt.Columns.Add("SGST_Per", typeof(decimal));
                        dt.Columns.Add("SGST_Amt", typeof(decimal));
                        dt.Columns.Add("IGST_Per", typeof(decimal));
                        dt.Columns.Add("IGST_Amt", typeof(decimal));
                        dt.Columns.Add("Total_Amount", typeof(decimal));
                        dt.Columns.Add("Prod_Tole_Qty", typeof(string));
                        dt.Columns.Add("Delivery_Date", typeof(string));
                        dt.Columns.Add("Remarks", typeof(string));

                        //dt.Rows.Add();
                        for (int i = 0; i < ioneNet.Masters.ProdSearch.dtgetproducts.Rows.Count; i++)
                        {
                            string prodcode = ioneNet.Masters.ProdSearch.dtgetproducts.Rows[i]["prod_id"].ToString();
                            var getproducts = (from obj in db.Products
                                               join uom in db.UoM_Masters on obj.Prod_Primary_UOM_Id equals uom.UOM_ID
                                               join tm in db.Tax_Class_Masters on obj.Prod_Tax_Class equals tm.ID
                                               where obj.prod_ID == Convert.ToInt32(prodcode)
                                               select new
                                               {
                            
                                                   // {
                                                   Item_Code = obj.prod_ID,
                                                   Int_Prod_Code = obj.Prod_Code,
                                                   Item_Description = obj.Prod_Name,
                                                   Item_Grade = "",
                                                  HSN_Code= obj.Prod_HSN_Code,   
                                                   UOM = uom.Uom_Descr,
                                                   Qty = 0,
                                                   Basic_Price = 0,
                                                   Amt_Before_Disc = 0,
                                                   Disc_Per = 0,
                                                   Disc_Amt = 0,
                                                   Taxable_Value = 0,                                                  
                                                    CGST_Per = 0,
                                                    CGST_Amt = 0,
                                                    SGST_Per = 0,
                                                    SGST_Amt = 0,
                                                    IGST_Per = 0,
                                                    IGST_Amt = 0,
                                                   Total_Amount = 0,
                                                   Prod_Tole_Qty =0,
                                                   Delivery_Date= DateTime.Today.ToString("MM/dd/yyyy"),
                                                  
                                                   Remarks = ""
                                               }).ToList();
                            dt.Rows.Add(getproducts[0].Item_Code,getproducts[0].Int_Prod_Code,  getproducts[0].Item_Description, getproducts[0].Item_Grade, getproducts[0].HSN_Code, getproducts[0].UOM, getproducts[0].Qty, getproducts[0].Basic_Price, getproducts[0].Amt_Before_Disc, getproducts[0].Disc_Per, getproducts[0].Disc_Amt, getproducts[0].Taxable_Value, getproducts[0].CGST_Per, getproducts[0].CGST_Amt, getproducts[0].SGST_Per, getproducts[0].SGST_Amt, getproducts[0].IGST_Per, getproducts[0].IGST_Amt, getproducts[0].Total_Amount, getproducts[0].Prod_Tole_Qty,getproducts[0].Delivery_Date, getproducts[0].Remarks);

                        }

                        dtexisting = dtexisting.AsEnumerable().Union(dt.AsEnumerable()).CopyToDataTable();
                        dgProducts.DataSource = dtexisting;
                    }
                }
                if (e.KeyCode == Keys.F3)
                {

                    ioneNet.OrderManagement.Transactions.frmEnqProductSpecs form = new ioneNet.OrderManagement.Transactions.frmEnqProductSpecs();
                    //Enq_NO = txtEnqNo.Text;
                    DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
                    int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                    string EnqId = R1.Cells["EnqNo"].Value.ToString();
                    var getproducts = (from obj in db.Sale_Enquiry_Masters                                      
                                       where obj.Id == Convert.ToInt32(EnqId)
                                       select new
                                       {
                                           obj.Enq_NO
                                       }).ToList();

                    Enq_NO = getproducts[0].Enq_NO;
                    ItemCode = R1.Cells["Enq_Item_ID"].Value.ToString();
                    //Item_Shape = txtShape.Text;
                    MtrlGrade = R1.Cells["Item_Grade"].Value.ToString(); ;
                    frmMain.frmname = "Order";
                    form.ShowDialog();

                  
                }
                if(e.KeyCode==Keys.F7)
                {

                    ioneNet.OrderManagement.Transactions.Forge_StockAllotment form = new ioneNet.OrderManagement.Transactions.Forge_StockAllotment();
                    //ioneNet.Masters.ProdSearch.frmName = "SOrder";                                   
                    form.ShowDialog();
                }
                if (e.KeyCode == Keys.F5)
                {
                    ioneNet.MaterialManagement.Masters.frmProductsNew form = new ioneNet.MaterialManagement.Masters.frmProductsNew();
                    //ioneNet.Masters.ProdSearch.frmName = "SOrder";                                   
                    form.ShowDialog();
                }

                if (e.KeyCode == Keys.F4) //Delivery Information / Locations
                {
                    if (chkMultiLocation.Checked == true)
                    {
                        ioneNet.OrderManagement.Transactions.SO_MultiDeliveryAddress form = new OrderManagement.Transactions.SO_MultiDeliveryAddress();
                        //ioneNet.Masters.ProdSearch.frmName = "SOrder";
                        int i = dgProducts.CurrentCell.RowIndex;
                        SONo = txtSoNo.Text;
                        ItemCode = dgProducts.Rows[i].Cells["Item_Code"].Value.ToString();
                        OrdQty = dgProducts.Rows[i].Cells["Qty"].Value.ToString();
                        form.ShowDialog();
                    }
                    else
                    {
                        MessageBox.Show("Check Multi Location Delivery? To Add The Details");

                    }
                }
                if (e.KeyCode == Keys.F6) //Remove Rows
                {
                    if (dgProducts.Rows.Count > 0)
                    {

                        foreach (DataGridViewCell oneCell in dgProducts.SelectedCells)
                        {
                            if (oneCell.Selected)
                                dgProducts.Rows.RemoveAt(oneCell.RowIndex);
                        }
                        decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0;
                        for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                        {

                            x += (dgProducts.Rows[i].Cells["Qty"].Value == "" || dgProducts.Rows[i].Cells["Qty"].Value == null || dgProducts.Rows[i].Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty"].Value);
                            y += (dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == "" || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == null || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value);
                            q += (dgProducts.Rows[i].Cells["Disc_Amt"].Value == "" || dgProducts.Rows[i].Cells["Disc_Amt"].Value == null || dgProducts.Rows[i].Cells["Disc_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Amt"].Value);
                            v += (dgProducts.Rows[i].Cells["Taxable_Value"].Value == "" || dgProducts.Rows[i].Cells["Taxable_Value"].Value == null || dgProducts.Rows[i].Cells["Taxable_Value"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Taxable_Value"].Value);
                            cg += (dgProducts.Rows[i].Cells["CGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["CGST_Amt"].Value == null || dgProducts.Rows[i].Cells["CGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Amt"].Value);
                            sg += (dgProducts.Rows[i].Cells["SGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["SGST_Amt"].Value == null || dgProducts.Rows[i].Cells["SGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Amt"].Value);
                            ig += (dgProducts.Rows[i].Cells["IGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["IGST_Amt"].Value == null || dgProducts.Rows[i].Cells["IGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Amt"].Value);
                            totA += (dgProducts.Rows[i].Cells["Total_Amount"].Value == "" || dgProducts.Rows[i].Cells["Total_Amount"].Value == null || dgProducts.Rows[i].Cells["Total_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Total_Amount"].Value);
                            cgstPer = Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Per"].Value);
                            sgstPer = Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Per"].Value);
                            igstPer = Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Per"].Value);

                        }

                        txtTotalQty.Text = x.ToString(".00");
                        txtSubTotal.Text = y.ToString("0.00");
                        txtTotDiscount.Text = q.ToString(".00");
                        txtTot_TaxableValue.Text = v.ToString(".00");
                        txtTot_CGST.Text = cg.ToString(".00");
                        txtTot_SGST.Text = sg.ToString(".00");
                        txtTot_IGST.Text = ig.ToString(".00");
                        //decimal fAmt = (txtFrieght.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtFrieght.Text);

                        txtTot_OrderValue.Text = (totA).ToString(".00");

                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void dgProducts_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                string comnpstatecode, suppStateCode;
                decimal taxRate=0;
                DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].Name;
                if(columnName =="Delivery_Date")
                {
                    //string eDate = R1.Cells["Delivery_Date"].Value.ToString();
                    //DateTime t = Convert.ToDateTime(R1.Cells["Delivery_Date"].Value.ToString());

                   // DateTime D = DateTime.ParseExact(eDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
                else
                
                    if (columnName == "Item_Description" && R1.Cells["Item_Description"].Value != null)
                    {

                        var getProductName = (from s in db.Get_ProductsList(logIn.company,1, R1.Cells["Item_Description"].Value.ToString())
                                              select new { s.prod_ID,s.Prod_Code, s.Uom_Descr, s.Prod_Group_Name, s.Prod_HSN_Code,s.Gst_Rate,s.Prod_Customer_Code,s.Prod_Description}).FirstOrDefault();

                        if (getProductName != null)
                        {
                            R1.Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                            R1.Cells["Item_code"].Value = getProductName.prod_ID.ToString();
                            R1.Cells["Int_Prod_Code"].Value = getProductName.Prod_Code.ToString();
                            R1.Cells["HSN_Code"].Value = getProductName.Prod_HSN_Code.ToString();
                        if (getProductName.Prod_Customer_Code != null)
                            {
                                R1.Cells["Remarks"].Value = getProductName.Prod_Customer_Code.ToString();
                            }
                            taxRate = Convert.ToDecimal(getProductName.Gst_Rate);
                            R1.Cells["Prod_Tole_Qty"].Value = "0";
                            R1.Cells["Delivery_Date"].Value = DateTime.Today.ToString("MM/dd/yyyy");
                            R1.Cells["Item_Grade"].Value = getProductName.Prod_Description;
                    }

                        else
                        {
                            R1.Cells["Item_Code"].Value = dgProducts.CurrentCell.RowIndex + 1;
                            R1.Cells["Int_Prod_Code"].Value = "NA";
                            if(logIn.company ==11)
                            {
                            R1.Cells["HSN_Code"].Value = "76072090";
                            }
                            else
                            {
                            R1.Cells["HSN_Code"].Value = "";
                            }
                            R1.Cells["Prod_Tole_Qty"].Value = "0";
                            R1.Cells["Delivery_Date"].Value = DateTime.Today.ToString("MM/dd/yyyy");
                        }
                        decimal  d;                    
                   
                        if(taxRate!=0)
                        {

                        }
                        else
                        {
                        var taxPer = (from s in db.Tax_Class_Masters where s.ID == Convert.ToInt32(cmbTaxClass.SelectedValue)
                                              select new { s.Gst_Rate }).FirstOrDefault();

                        taxRate = Convert.ToDecimal(taxPer.Gst_Rate);
                        }
                        var d1 = (from a in db.Company_Infos where a.Id == logIn.company select new { a.GST_No,a.State_Code }).ToList();
                        if (d1.Count > 0)
                        {
                            comnpstatecode = d1[0].State_Code;
                            suppStateCode = txtCustStateCode.Text;
                            if (chkSEZOrder.Checked==false)
                            {
                                if (suppStateCode == comnpstatecode)
                                {
                                    d = Convert.ToDecimal(taxRate) / 2;
                                    R1.Cells["CGST_Per"].Value = d.ToString("0.00");
                                    R1.Cells["SGST_Per"].Value = d.ToString("0.00");
                                    R1.Cells["IGST_Per"].Value = "0.00";
                                }
                                else
                                {
                                    d = taxRate;
                                    R1.Cells["CGST_Per"].Value = "0.00";
                                    R1.Cells["SGST_Per"].Value = "0.00";
                                    R1.Cells["IGST_Per"].Value = d.ToString("0.00");
                                }
                            }
                            else
                            {
                                R1.Cells["CGST_Per"].Value = "0.00";
                                R1.Cells["SGST_Per"].Value = "0.00";
                                R1.Cells["IGST_Per"].Value = "0.00";
                            }
                        }

                    }
                    if(columnName =="Qty")
                    {
                        decimal d;
                   
                        var getProductName = (from s in db.Get_ProductsList(logIn.company, 1, R1.Cells["Item_Description"].Value.ToString())
                                              select new { s.prod_ID, s.Uom_Descr, s.Prod_Group_Name, s.Prod_HSN_Code, s.Gst_Rate }).FirstOrDefault();


                        if (getProductName != null)
                        {
                            //R1.Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                            //R1.Cells["Item_code"].Value = getProductName.prod_ID.ToString();
                            taxRate = Convert.ToDecimal(getProductName.Gst_Rate);
                        }
                        if (taxRate != 0)
                        {

                        }
                        else
                        {
                        var taxPer = (from s in db.Tax_Class_Masters
                                      where s.ID == Convert.ToInt32(cmbTaxClass.SelectedValue)
                                      select new { s.Gst_Rate }).FirstOrDefault();

                            taxRate = Convert.ToDecimal(taxPer.Gst_Rate);
                        }
                        var d1 = (from a in db.Company_Infos where a.Id == logIn.company select new { a.GST_No,a.State_Code }).ToList();
                        if (d1.Count > 0)
                        {
                            comnpstatecode = d1[0].State_Code;
                            suppStateCode = txtCustStateCode.Text;
                            if (chkSEZOrder.Checked == false)
                            {
                                if (suppStateCode == comnpstatecode)
                                {
                                    d = Convert.ToDecimal(taxRate) / 2;
                                    R1.Cells["CGST_Per"].Value = d.ToString("0.00");
                                    R1.Cells["SGST_Per"].Value = d.ToString("0.00");
                                    R1.Cells["IGST_Per"].Value = "0.00";
                                }
                                else
                                {
                                    d = taxRate;
                                    R1.Cells["CGST_Per"].Value = "0.00";
                                    R1.Cells["SGST_Per"].Value = "0.00";
                                    R1.Cells["IGST_Per"].Value = d.ToString("0.00");
                                }
                            }
                            else
                            {
                                R1.Cells["CGST_Per"].Value = "0.00";
                                R1.Cells["SGST_Per"].Value = "0.00";
                                R1.Cells["IGST_Per"].Value = "0.00";
                            }
                        }

                    }
                if (R1.Cells["Item_Description"].Value != null)
                {
                    decimal ReceivedQty = (R1.Cells["Qty"].Value == "" || R1.Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty"].Value);
                    //decimal RejectedQty = (R1.Cells["Qty_Rejected"].Value == "" || R1.Cells["Qty_Rejected"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty_Rejected"].Value);

                    

                    if (columnName == "Basic_Price" || columnName == "Disc_Per" || columnName == "Qty")
                    {
                        if (ReceivedQty > 0)
                        {
                            decimal AcceptedQty = (R1.Cells["Qty"].Value == "" || R1.Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty"].Value);
                            decimal price = (R1.Cells["Basic_Price"].Value == "" || R1.Cells["Basic_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Basic_Price"].Value);
                            decimal DiscPer = (R1.Cells["Disc_Per"].Value == "" || R1.Cells["Disc_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Disc_Per"].Value);


                            decimal Amt, DiscAmt, netAmt, gst, igst, totamt;

                            Amt = AcceptedQty * price;

                            R1.Cells["Amt_Before_Disc"].Value = Amt.ToString("0.00");
                            DiscAmt = (Amt * DiscPer) / 100;
                            R1.Cells["Disc_Amt"].Value = DiscAmt.ToString("0.00");
                            netAmt = Amt - DiscAmt;
                            R1.Cells["Taxable_Value"].Value = netAmt.ToString("0.00");
                            gst = (Convert.ToDecimal(R1.Cells["Taxable_Value"].Value) * Convert.ToDecimal(R1.Cells["CGST_Per"].Value)) / 100;

                            R1.Cells["CGST_Amt"].Value = gst;
                            R1.Cells["SGST_Amt"].Value = gst;
                            igst = (Convert.ToDecimal(R1.Cells["Taxable_Value"].Value) * Convert.ToDecimal(R1.Cells["IGST_Per"].Value)) / 100;
                            R1.Cells["IGST_Amt"].Value = igst;

                            totamt = Math.Round(netAmt + gst + gst + igst);
                            R1.Cells["Total_Amount"].Value = totamt;
                            decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0;
                            for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                            {

                                x += (dgProducts.Rows[i].Cells["Qty"].Value == "" || dgProducts.Rows[i].Cells["Qty"].Value == null || dgProducts.Rows[i].Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty"].Value);
                                y += (dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == "" || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == null || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value);
                                q += (dgProducts.Rows[i].Cells["Disc_Amt"].Value == "" || dgProducts.Rows[i].Cells["Disc_Amt"].Value == null || dgProducts.Rows[i].Cells["Disc_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Amt"].Value);
                                v += (dgProducts.Rows[i].Cells["Taxable_Value"].Value == "" || dgProducts.Rows[i].Cells["Taxable_Value"].Value == null || dgProducts.Rows[i].Cells["Taxable_Value"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Taxable_Value"].Value);
                                cg += (dgProducts.Rows[i].Cells["CGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["CGST_Amt"].Value == null || dgProducts.Rows[i].Cells["CGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Amt"].Value);
                                sg += (dgProducts.Rows[i].Cells["SGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["SGST_Amt"].Value == null || dgProducts.Rows[i].Cells["SGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Amt"].Value);
                                ig += (dgProducts.Rows[i].Cells["IGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["IGST_Amt"].Value == null || dgProducts.Rows[i].Cells["IGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Amt"].Value);
                                totA += (dgProducts.Rows[i].Cells["Total_Amount"].Value == "" || dgProducts.Rows[i].Cells["Total_Amount"].Value == null || dgProducts.Rows[i].Cells["Total_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Total_Amount"].Value);
                                cgstPer = Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Per"].Value);
                                sgstPer = Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Per"].Value);
                                igstPer = Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Per"].Value);

                            }

                            txtTotalQty.Text = x.ToString(".00");
                            txtSubTotal.Text = y.ToString("0.00");
                            txtTotDiscount.Text = q.ToString(".00");
                            txtTot_TaxableValue.Text = v.ToString(".00");
                            txtTot_CGST.Text = cg.ToString(".00");
                            txtTot_SGST.Text = sg.ToString(".00");
                            txtTot_IGST.Text = ig.ToString(".00");
                            //decimal fAmt = (txtFrieght.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtFrieght.Text);

                            txtTot_OrderValue.Text = (totA).ToString(".00");
                        }

                    }
                    
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

        private void dgProducts_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            try
            {
                if (CmbBuyerName.Text == "")
                {
                    MessageBox.Show("Select Customer Name To Proceed");
                }
                else
                   if (txtCustGSTNo.Text == "")
                {
                    MessageBox.Show("Customer GST No Required To Proceed");
                }
                else
                      if (cmbTaxClass.Text == "")
                {
                    MessageBox.Show("Select Tax Class To Proceed");
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

        private void chkRepeatOrder_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBoxAutoComplete1_Leave(object sender, EventArgs e)
        {
            
        }

        private void btnClear_Click(object sender, EventArgs e)
        {

        }

        private void CmbBuyerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
               

                if (CmbBuyerName.Text != "")

                {
                    //int ss= CmbBuyerName.ValueMember;
                    int s = CmbBuyerName.SelectedIndex;
                    var State = (from c in db.Supplier_informations                                
                                 //where c.ID == Convert.ToInt32(CmbBuyerName.SelectedValue)
                                 where c.Supplier_Name == CmbBuyerName.Text
                                 select new { c.GSTIN_NO, c.StateCode }).ToList();
                    if (State.Count > 0)
                    {
                        txtCustGSTNo.Text = State[0].GSTIN_NO;
                        txtCustStateCode.Text = State[0].StateCode;

                        var getOrd = (from b in db.PreviousHistory(logIn.company, Convert.ToInt32(CmbBuyerName.SelectedValue))
                                      select new { b.TotalOrders, b.PendingOrderValue }).FirstOrDefault();
                        if (getOrd != null)
                        {
                            linkLabel2.Text = getOrd.TotalOrders.ToString();
                            linkLabel4.Text = getOrd.PendingOrderValue.ToString();
                        }
                        else
                        {
                            linkLabel2.Text = "0";
                            linkLabel4.Text = "0";
                        }
                    }
                    //Get Order data and pending receivables
                    var AB = (from c in db.Supplier_informations
                                 join a in db.AccountMasters on c.Supplier_Id equals a.AccCode
                                 //where c.ID == Convert.ToInt32(CmbBuyerName.SelectedValue)
                                 where c.Supplier_Name == CmbBuyerName.Text
                                 select new { c.GSTIN_NO, a.id, c.StateCode }).ToList();
                    if (AB.Count > 0)
                    {
                        DateTime t = dpSODate.Value;
                        string t1 = t.ToString("dd/MMM/yyyy");
                        var getBal = (from b in db.GetAccountBalance(logIn.company, t, AB[0].id, 1,logIn.BU_ID)
                                      select new { b.Balance, b.BalType }).FirstOrDefault();
                        if (getBal != null)
                        {
                            linkLabel5.Text = getBal.Balance.ToString() + getBal.BalType.ToString();
                        }
                        else
                        {
                            linkLabel5.Text = "0";
                        }
                       
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void cmdPrevOrder_Click(object sender, EventArgs e)
        {

            try
            {

                var d = (from data in db.SP_Forge_Get_RepeatOrders (logIn.company, Convert.ToInt32(CmbBuyerName.SelectedValue))
                         select data                         
                         ).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["CustomerPONo"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["CustomerPONo"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["CustomerPONo"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["CustomerPONo"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Item_Description"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Item_Description"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Item_Description"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Item_Description"].FilterRowCondition = FilterRowCondition.Contains;
                    btnOK.Visible = false;
                    btnOKOrder.Visible = true;
                    groupBox2.Visible = true;
                    txtSearch.Focus();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ioneNet.FinanceManagement.TaxClass form = new ioneNet.FinanceManagement.TaxClass();
            //ioneNet.Masters.ProdSearch.frmName = "SOrder";                                   
            form.ShowDialog();
        }

        private void cmbTaxClass_Enter(object sender, EventArgs e)
        {
            //Bind Tax Class
            string cmbText = cmbTaxClass.Text;
            var bindLoc = (from m in db.Tax_Class_Masters
                           where m.Company_ID == logIn.company
                           select new
                           {
                               m.Tax_Class_Name,
                               m.ID,
                           }).ToList();

            if (bindLoc.Count > 0)
            {
                cmbTaxClass.DisplayMember = "Tax_Class_Name";
                cmbTaxClass.ValueMember = "ID";
                cmbTaxClass.DataSource = bindLoc;

            }
              cmbTaxClass.Text = cmbText;
        }

        private void linkLabel3_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ioneNet.MaterialManagement.SupplierMaster form = new ioneNet.MaterialManagement.SupplierMaster();
            //ioneNet.Masters.ProdSearch.frmName = "SOrder";                                   
            form.ShowDialog();
        }

        private void CmbBuyerName_Enter(object sender, EventArgs e)
        {
            try
            {
                String myString = CmbBuyerName.Text;
                var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company && m.Status == 1 select new { m.ID, m.Supplier_Name }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    CmbBuyerName.DataSource = Buyerblind;
                    CmbBuyerName.ValueMember = "ID";
                    CmbBuyerName.DisplayMember = "Supplier_Name";

                }
                //if (CmbBuyerName.Items.Count > 0)
                CmbBuyerName.Text = myString;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbQuotNo_Leave(object sender, EventArgs e)
        {
            try
            {
            //    txtSoNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_No;
            //txtAmendNo.Text = OrderManagement.Transactions.ListOfQuotes.SO_Amend_No;
            if(cmbQuotNo.Text!="NA" && cmbQuotNo.Text!="")
                { 
                String myString = "";
                int QuoteMasterID = 0;
                myString = txtSoNo.Text;

                var da = (from obj in db.Sale_Quotation_Masters
                          where obj.Quot_NO == cmbQuotNo.Text && obj.is_amended == false && obj.Company_ID == logIn.company && obj.bu_id == logIn.BU_ID
                          select obj).ToList();

                    if (da.Count > 0)
                    {

                        //dpQuotDate.Text = da[0].Quot_Date.ToString();
                        //bindCustomer();
                        CmbBuyerName.SelectedValue = da[0].BuyerName;
                        cmbTaxClass.SelectedValue = da[0].Tax_Class;
                        //  CmbConsigneeName.SelectedValue = da[0].ConsigneeName;
                        txtCustGSTNo.Text = da[0].Cust_GST_No;
                        
                        txtEnqNo.Text = da[0].EnqNo;

                        if (da[0].SEZ_Order == true)
                        {
                            chkSEZOrder.Checked = true;
                        }
                        else
                        {
                            chkSEZOrder.Checked = false;
                        }


                        if (da[0].Sale_office == null)
                        {
                            cmbSaleOffice.SelectedValue = 36;
                        }
                        else
                        {
                            cmbSaleOffice.SelectedValue = da[0].Sale_office;
                        }
                        if (da[0].Price_Basis == null)
                        {
                            cmbPriceBasis.SelectedValue = 7;
                        }
                        else
                        {
                            cmbPriceBasis.SelectedValue = da[0].Price_Basis;
                        }
                        if (da[0].Insurance_Scope == null)
                        {
                            cmbInsurance.SelectedValue = 11;
                        }
                        else
                        {
                            cmbInsurance.SelectedValue = da[0].Insurance_Scope;
                        }

                        if (da[0].PaymentTerms == null)
                        {
                            cmbPaymentTerms.SelectedValue = 35;
                        }
                        else
                        {
                            cmbPaymentTerms.SelectedValue = da[0].PaymentTerms;
                        }

                        if (da[0].Trasnport_Scope == null)
                        {
                            cmbTransport_Scope.SelectedValue = 19;
                        }
                        else
                        {
                            cmbTransport_Scope.SelectedValue = da[0].Trasnport_Scope;
                        }



                        txtSplInstructions.Text = da[0].Delivery_Terms;

                        txtOtherTerms.Text = da[0].Other_Terms;
                        txtCustomeContact.Text = da[0].Customer_Contact;
                    }
                    //Bind Tests and Standards

                    var ETest = (from obj in db.Sale_Enquiry_Masters
                              where obj.Enq_NO == txtEnqNo.Text && obj.Company_ID == logIn.company
                              select obj).ToList();

                    if (ETest.Count > 0)
                    {
                        if (ETest[0].Tests_Required != null)
                        {
                            string MP = ETest[0].Tests_Required.ToString();
                            string[] values = MP.Split(',');
                            for (int j = 0; j < values.Length; j++)
                            {
                                values[j] = values[j].Trim();
                                string m = values[j].ToString();
                                for (int i = 0; i < chkTests.Items.Count; i++)
                                {
                                    if (chkTests.Items[i].ToString() == m)
                                    {
                                        chkTests.SetItemChecked(i, true);
                                    }
                                }
                            }
                        }

                        if (ETest[0].Standards_To_Refer != null)
                        {
                            string MP = ETest[0].Standards_To_Refer.ToString();
                            string[] values = MP.Split(',');
                            for (int j = 0; j < values.Length; j++)
                            {
                                values[j] = values[j].Trim();
                                string m = values[j].ToString();
                                for (int i = 0; i < chkStdRefer.Items.Count; i++)
                                {
                                    if (chkStdRefer.Items[i].ToString() == m)
                                    {
                                        chkStdRefer.SetItemChecked(i, true);
                                    }
                                }
                            }
                        }
                    }
                    cmbRMAvailable.Text = ETest[0].RM_Available;
                    cmbSpecClear.Text = ETest[0].Spec_Is_Clear;
                    cmbMfgFeasibility.Text = ETest[0].Mfg_Feasibility;

                    //Get Products
                    SqlCommand cmd2 = new SqlCommand("SP_GetQuotesDetails_ForSaleOrder_Forge", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@compname", logIn.company);                  
                    
                    cmd2.Parameters.AddWithValue("@Quotno", cmbQuotNo.Text);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    //DataSet ds2 = new DataSet();
                    DataTable ds2 = new DataTable();
                    // da2.Fill(ds2, "x");
                    da2.Fill(ds2);
                    dgProducts.DataSource = ds2;

                    //Calculate the Values
                    decimal b, c, d;
                    string comnpstatecode, suppStateCode;
                    decimal taxRate = 0;
                    DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
                    int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                    string columnName = dgProducts.Columns[columnIndex].Name;
                    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                    {
                        var getProductName = (from s in db.Get_ProductsList(logIn.company, 1, dgProducts.Rows[i].Cells["Item_Description"].Value.ToString())
                                              select new { s.prod_ID, s.Uom_Descr, s.Prod_Group_Name, s.Prod_HSN_Code, s.Gst_Rate }).FirstOrDefault();


                        if (getProductName != null)
                        {
                            //R1.Cells["UOM"].Value = getProductName.Uom_Descr.ToString();
                            //R1.Cells["Item_code"].Value = getProductName.prod_ID.ToString();
                            taxRate = Convert.ToDecimal(getProductName.Gst_Rate);
                        }
                        if (taxRate != 0)
                        {

                        }
                        else
                        {
                            taxRate = 18;
                        }
                        var d1 = (from a in db.Company_Infos where a.Id == logIn.company select new { a.GST_No, a.State_Code }).ToList();
                        if (d1.Count > 0)
                        {
                            comnpstatecode = d1[0].State_Code;
                            suppStateCode = txtCustStateCode.Text;
                            if (chkSEZOrder.Checked == false)
                            {
                                if (suppStateCode == comnpstatecode)
                                {
                                    d = Convert.ToDecimal(taxRate) / 2;
                                    dgProducts.Rows[i].Cells["CGST_Per"].Value = d.ToString("0.00");
                                    dgProducts.Rows[i].Cells["SGST_Per"].Value = d.ToString("0.00");
                                    dgProducts.Rows[i].Cells["IGST_Per"].Value = "0.00";
                                }
                                else
                                {
                                    d = taxRate;
                                    dgProducts.Rows[i].Cells["CGST_Per"].Value = "0.00";
                                    dgProducts.Rows[i].Cells["SGST_Per"].Value = "0.00";
                                    dgProducts.Rows[i].Cells["IGST_Per"].Value = d.ToString("0.00");
                                }
                            }
                            else
                            {
                                dgProducts.Rows[i].Cells["CGST_Per"].Value = "0.00";
                                dgProducts.Rows[i].Cells["SGST_Per"].Value = "0.00";
                                dgProducts.Rows[i].Cells["IGST_Per"].Value = "0.00";
                            }

                            decimal AcceptedQty = (dgProducts.Rows[i].Cells["Qty"].Value == "" || dgProducts.Rows[i].Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty"].Value);
                            decimal price = (dgProducts.Rows[i].Cells["Basic_Price"].Value == "" || dgProducts.Rows[i].Cells["Basic_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Basic_Price"].Value);
                            decimal DiscPer = (dgProducts.Rows[i].Cells["Disc_Per"].Value == "" || dgProducts.Rows[i].Cells["Disc_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Per"].Value);

                            decimal Amt, DiscAmt, netAmt, gst, igst, totamt;

                            Amt = AcceptedQty * price;

                            dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value = Amt.ToString("0.00");
                            DiscAmt = (Amt * DiscPer) / 100;
                            dgProducts.Rows[i].Cells["Disc_Amt"].Value = DiscAmt.ToString("0.00");
                            netAmt = Amt - DiscAmt;
                            dgProducts.Rows[i].Cells["Taxable_Value"].Value = netAmt.ToString("0.00");
                            gst = (Convert.ToDecimal(dgProducts.Rows[i].Cells["Taxable_Value"].Value) * Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Per"].Value)) / 100;

                            dgProducts.Rows[i].Cells["CGST_Amt"].Value = gst;
                            dgProducts.Rows[i].Cells["SGST_Amt"].Value = gst;
                            igst = (Convert.ToDecimal(dgProducts.Rows[i].Cells["Taxable_Value"].Value) * Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Per"].Value)) / 100;

                            dgProducts.Rows[i].Cells["IGST_Amt"].Value = igst;

                            totamt = Math.Round(netAmt + gst + gst + igst);
                            dgProducts.Rows[i].Cells["Total_Amount"].Value = totamt;
                        }
                    }
                    decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
                    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                    {

                        x += (dgProducts.Rows[i].Cells["Qty"].Value == "" || dgProducts.Rows[i].Cells["Qty"].Value == null || dgProducts.Rows[i].Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty"].Value);
                        y += (dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == "" || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == null || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value);
                        q += (dgProducts.Rows[i].Cells["Disc_Amt"].Value == "" || dgProducts.Rows[i].Cells["Disc_Amt"].Value == null || dgProducts.Rows[i].Cells["Disc_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Amt"].Value);
                        v += (dgProducts.Rows[i].Cells["Taxable_Value"].Value == "" || dgProducts.Rows[i].Cells["Taxable_Value"].Value == null || dgProducts.Rows[i].Cells["Taxable_Value"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Taxable_Value"].Value);
                        cg += (dgProducts.Rows[i].Cells["CGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["CGST_Amt"].Value == null || dgProducts.Rows[i].Cells["CGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Amt"].Value);
                        sg += (dgProducts.Rows[i].Cells["SGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["SGST_Amt"].Value == null || dgProducts.Rows[i].Cells["SGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Amt"].Value);
                        ig += (dgProducts.Rows[i].Cells["IGST_Amt"].Value == "" || dgProducts.Rows[i].Cells["IGST_Amt"].Value == null || dgProducts.Rows[i].Cells["IGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Amt"].Value);
                        totA += (dgProducts.Rows[i].Cells["Total_Amount"].Value == "" || dgProducts.Rows[i].Cells["Total_Amount"].Value == null || dgProducts.Rows[i].Cells["Total_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Total_Amount"].Value);

                    }

                    txtTotalQty.Text = x.ToString(".00");
                    txtSubTotal.Text = y.ToString("0.00");
                    txtTotDiscount.Text = q.ToString(".00");
                    txtTot_TaxableValue.Text = v.ToString(".00");
                    txtTot_CGST.Text = cg.ToString(".00");
                    txtTot_SGST.Text = sg.ToString(".00");
                    txtTot_IGST.Text = ig.ToString(".00");
                    //decimal fAmt = (txtFrieght.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtFrieght.Text);

                    txtTot_OrderValue.Text = (totA).ToString(".00");


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtPackingCharges_Leave(object sender, EventArgs e)
        {
            GetTot();
        }

        private void txtInsCharges_Leave(object sender, EventArgs e)
        {
            GetTot();
        }

        private void tableLayoutPanel5_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {

                var d = (from data in db.SP_GetQuotesDetails_ForSaleOrder_Forge(logIn.company, Convert.ToInt32(CmbBuyerName.SelectedValue)) 
                         select new


                         {data.Quot_NO,
                             Quote_Date = data.Quot_Amend_Date ,data.Item_Code,data.Item_Description,data.Item_Grade,data.UOM,data.Qty,
                             data.Basic_Price,
                             data.Rec_ID }).ToList();

                if (d.Count > 0)
                {
                    //dgProductsList.DataSource = d;
                    sfDataGrid1.DataSource = d;
                    this.sfDataGrid1.FilterRowPosition = RowPosition.Top;
                    this.sfDataGrid1.Columns["Quot_NO"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Quot_NO"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Quot_NO"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Quot_NO"].FilterRowCondition = FilterRowCondition.Contains;

                    this.sfDataGrid1.Columns["Item_Description"].FilterRowEditorType = "TextBox";
                    this.sfDataGrid1.Columns["Item_Description"].ShowFilterRowOptions = false;
                    this.sfDataGrid1.Columns["Item_Description"].ImmediateUpdateColumnFilter = true;
                    this.sfDataGrid1.Columns["Item_Description"].FilterRowCondition = FilterRowCondition.Contains;
                    btnOK.Visible = true;
                    btnOKOrder.Visible = false;
                    groupBox2.Visible = true;
                    txtSearch.Focus();
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            groupBox2.Visible = false;
        }

        System.Data.DataRow drgetproducts;
        DataTable dtexisting = new DataTable();
        private void btnOK_Click(object sender, EventArgs e)
        {
            try
            {
                int sno = 0;
                DataTable dtgetSelectedprducts = new DataTable();
                //Check Whether Exisitng Products Already Selected in Main Grid
                if (dgProducts.Rows.Count > 1)
                {
                    dtexisting.Rows.Clear();
                    dtexisting.Columns.Clear();
                    dtexisting.Columns.Add("Item_Code", typeof(string));
                    dtexisting.Columns.Add("Int_Prod_Code", typeof(string));                    
                    dtexisting.Columns.Add("Item_Description", typeof(string));                   
                    dtexisting.Columns.Add("Item_Grade", typeof(string));
                    dtexisting.Columns.Add("HSN_Code", typeof(string));
                    dtexisting.Columns.Add("UOM", typeof(string));
                    dtexisting.Columns.Add("Qty", typeof(decimal));                   
                    dtexisting.Columns.Add("Basic_Price", typeof(string));
                    dtexisting.Columns.Add("Amt_Before_Disc", typeof(string));
                    dtexisting.Columns.Add("Disc_Per", typeof(string));
                    dtexisting.Columns.Add("Disc_Amt", typeof(string));
                    dtexisting.Columns.Add("Taxable_Value", typeof(string));
                    dtexisting.Columns.Add("CGST_Per", typeof(string));
                    dtexisting.Columns.Add("CGST_Amt", typeof(string));
                    dtexisting.Columns.Add("SGST_Per", typeof(string));
                    dtexisting.Columns.Add("SGST_Amt", typeof(string));
                    dtexisting.Columns.Add("IGST_Per", typeof(string));
                    dtexisting.Columns.Add("IGST_Amt", typeof(string));
                    dtexisting.Columns.Add("Total_Amount", typeof(string));
                    dtexisting.Columns.Add("Prod_Tole_Qty", typeof(string));
                    dtexisting.Columns.Add("Delivery_Date", typeof(string));
                    dtexisting.Columns.Add("Quot_no", typeof(string));
                    dtexisting.Columns.Add("EnqNo", typeof(string));
                    dtexisting.Columns.Add("Enq_Item_ID", typeof(string));
                    dtexisting.Columns.Add("Remarks", typeof(string));
                    //
                    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                    {
                        DataRow dr;
                        dr = dtexisting.NewRow();
                        dr["Item_Code"] =sno+1;
                        dr["Int_Prod_Code"] = dgProducts.Rows[i].Cells["Int_Prod_Code"].Value.ToString();
                        dr["Item_Description"] = dgProducts.Rows[i].Cells["Item_Description"].Value.ToString();
                        dr["Item_Grade"] = dgProducts.Rows[i].Cells["Item_Grade"].Value.ToString();
                        dr["HSN_Code"] = dgProducts.Rows[i].Cells["HSN_Code"].Value.ToString();
                        dr["UOM"] = dgProducts.Rows[i].Cells["UOM"].Value.ToString();
                        dr["Qty"] = dgProducts.Rows[i].Cells["Qty"].Value.ToString();                       
                        dr["Basic_Price"] = dgProducts.Rows[i].Cells["Basic_Price"].Value.ToString();
                        dr["Amt_Before_Disc"] = dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value.ToString();
                        dr["Disc_Per"] = dgProducts.Rows[i].Cells["Disc_Per"].Value.ToString();
                        dr["Disc_Amt"] = dgProducts.Rows[i].Cells["Disc_Amt"].Value.ToString();
                        dr["Taxable_Value"] = dgProducts.Rows[i].Cells["Taxable_Value"].Value.ToString();
                        dr["CGST_Per"] = dgProducts.Rows[i].Cells["CGST_Per"].Value.ToString();
                        dr["CGST_Amt"] = dgProducts.Rows[i].Cells["CGST_Amt"].Value.ToString();
                        dr["SGST_Per"] = dgProducts.Rows[i].Cells["SGST_Per"].Value.ToString();
                        dr["SGST_Amt"] = dgProducts.Rows[i].Cells["SGST_Amt"].Value.ToString();
                        dr["IGST_Per"] = dgProducts.Rows[i].Cells["IGST_Per"].Value.ToString();
                        dr["IGST_Amt"] = dgProducts.Rows[i].Cells["IGST_Amt"].Value.ToString();
                        dr["Total_Amount"] = dgProducts.Rows[i].Cells["Total_Amount"].Value.ToString();
                        dr["Prod_Tole_Qty"] = dgProducts.Rows[i].Cells["Prod_Tole_Qty"].Value.ToString();
                        dr["Delivery_Date"] = dgProducts.Rows[i].Cells["Delivery_Date"].Value.ToString();                       
                        dr["Quot_no"] = dgProducts.Rows[i].Cells["Quot_no"].Value.ToString();
                        dr["EnqNo"] = dgProducts.Rows[i].Cells["EnqNo"].Value.ToString();
                        dr["Enq_Item_ID"] = dgProducts.Rows[i].Cells["Enq_Item_ID"].Value.ToString();
                        dr["Remarks"] = dgProducts.Rows[i].Cells["Remarks"].Value.ToString();
                        dtexisting.Rows.Add(dr);
                        sno = sno + 1;
                    }
                    dtexisting.AcceptChanges();
                }



                dtgetproducts.Columns.Clear();
                dtgetproducts.Rows.Clear();
                dtgetproducts.Columns.Add("Item_Code", typeof(string));
                dtgetproducts.Columns.Add("Int_Prod_Code", typeof(string));
                dtgetproducts.Columns.Add("Item_Description", typeof(string));                
                dtgetproducts.Columns.Add("Item_Grade", typeof(string));
                dtgetproducts.Columns.Add("HSN_Code", typeof(string));
                dtgetproducts.Columns.Add("UOM", typeof(string));
                dtgetproducts.Columns.Add("Qty", typeof(string));               
                dtgetproducts.Columns.Add("Basic_Price", typeof(string));
                dtgetproducts.Columns.Add("Amt_Before_Disc", typeof(string));
                dtgetproducts.Columns.Add("Disc_Per", typeof(string));
                dtgetproducts.Columns.Add("Disc_Amt", typeof(string));
                dtgetproducts.Columns.Add("Taxable_Value", typeof(string));
                dtgetproducts.Columns.Add("CGST_Per", typeof(string));
                dtgetproducts.Columns.Add("CGST_Amt", typeof(string));
                dtgetproducts.Columns.Add("SGST_Per", typeof(string));
                dtgetproducts.Columns.Add("SGST_Amt", typeof(string));
                dtgetproducts.Columns.Add("IGST_Per", typeof(string));
                dtgetproducts.Columns.Add("IGST_Amt", typeof(string));
                dtgetproducts.Columns.Add("Total_Amount", typeof(string));
                dtgetproducts.Columns.Add("Prod_Tole_Qty", typeof(string));
                dtgetproducts.Columns.Add("Delivery_Date", typeof(string));
                dtgetproducts.Columns.Add("Quot_no", typeof(string));
                dtgetproducts.Columns.Add("EnqNo", typeof(string));
                dtgetproducts.Columns.Add("Enq_Item_ID", typeof(string));
                dtgetproducts.Columns.Add("Remarks", typeof(string));
                dtgetfinalprducts.Rows.Clear();
                //listBox.Items.Clear();
                // Get the selected items of SfDataGrid
                //var reflector = this.sfDataGrid1.View.GetPropertyAccessProvider();
                //var row = this.sfDataGrid1.SelectedItem;

                //string ProdCode;
                //string SoNo;
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
                        var ProdCodeCol = sfDataGrid1.Columns[1].MappingName;
                        var SONoCol = sfDataGrid1.Columns[0].MappingName;
                        //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                        //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                        if (rowData == item)
                        {
                            var Item_Code = (rowData.GetType().GetProperty("Item_Code").GetValue(rowData, null).ToString());
                            var Item_Description = (rowData.GetType().GetProperty("Item_Description").GetValue(rowData, null).ToString());
                            var Item_Grade = (rowData.GetType().GetProperty("Item_Grade").GetValue(rowData, null).ToString());

                            var UOM = (rowData.GetType().GetProperty("UOM").GetValue(rowData, null).ToString());
                            var PO_Qty = (rowData.GetType().GetProperty("Qty").GetValue(rowData, null).ToString());
                            //var Basic_Price = (rowData.GetType().GetProperty("Basic_Price").GetValue(rowData, null).ToString());
                            var SO_Ref_No = (rowData.GetType().GetProperty("Quot_NO").GetValue(rowData, null).ToString());
                            var Rec_ID = (rowData.GetType().GetProperty("Rec_ID").GetValue(rowData, null).ToString());
                            
                            var getHSN = (from s in db.MaterialGrades 
                                          where s.Material_Grade == Item_Grade.ToString()
                                          select new { s.HSNCode }).ToList();
                            string HSNCode="";
                            if(getHSN.Count>0)
                            {
                                HSNCode = getHSN[0].HSNCode;
                            }
                                                        //var getTolQty = (from s in db.Purchase_Order_Masters
                            //                 where s.Id == Convert.ToInt32(Rec_ID)
                            //                 select new { s.Qty_Tolerence }).FirstOrDefault();    
                            decimal DiscPer = 0;
                            var getDiscPer = (from s in db.Sale_Quotation_Childs
                                              join q in db.Sale_Quotation_Masters on s.Quot_Master_ID equals q.Id
                                              join es in db.Sale_Enquiry_Childs on new { x1 = q.EnqNo, x2 = s.Prod_Code } equals new { x1 = es.Enq_NO, x2 = es.Prod_Code }
                                              where s.Quot_Master_ID == Convert.ToInt32(Rec_ID) && s.Prod_Code == Convert.ToInt32(Item_Code)
                                              select new { s.Disc_Per, s.Price, es.Prod_Shape, s.Amount, 
                                                  s.Disc_Amount, s.CGST_Per, s.CGST_Amnt, s.SGST_Per, 
                                                  s.SGST_Amnt, s.IGST_Per, s.IGST_Amnt, s.Net_Amount, 
                                                  s.Taxable_Value,s.Quot_Master_ID,es.Enq_Master_ID,es.Prod_Code }).FirstOrDefault();

                            DiscPer = Convert.ToDecimal(getDiscPer.Disc_Per);
                            drgetproducts = dtgetproducts.NewRow();
                            drgetproducts["Item_Code"] =sno+1;
                            drgetproducts["Int_Prod_Code"] =getDiscPer.Prod_Shape;
                            drgetproducts["Item_Description"] = Item_Description.ToString();                            
                            drgetproducts["Item_Grade"] = Item_Grade.ToString();
                            drgetproducts["HSN_Code"] = HSNCode;                            
                            drgetproducts["UOM"] = UOM.ToString();
                            drgetproducts["Qty"] = PO_Qty.ToString();                           
                            drgetproducts["Basic_Price"] = getDiscPer.Price.ToString();
                            drgetproducts["Amt_Before_Disc"] = getDiscPer.Amount.ToString();
                            drgetproducts["Disc_Per"] = DiscPer;
                            drgetproducts["Disc_Amt"] = getDiscPer.Disc_Amount.ToString(); 
                            drgetproducts["Taxable_Value"] = getDiscPer.Taxable_Value.ToString(); 
                            drgetproducts["CGST_Per"] = getDiscPer.CGST_Per.ToString(); 
                            drgetproducts["CGST_Amt"] = getDiscPer.CGST_Amnt.ToString(); 
                            drgetproducts["SGST_Per"] = getDiscPer.SGST_Per.ToString(); 
                            drgetproducts["SGST_Amt"] = getDiscPer.SGST_Amnt.ToString(); 
                            drgetproducts["IGST_Per"] = getDiscPer.IGST_Per.ToString(); 
                            drgetproducts["IGST_Amt"] = getDiscPer.IGST_Amnt.ToString(); 
                            drgetproducts["Total_Amount"] = getDiscPer.Net_Amount.ToString();
                            drgetproducts["Prod_Tole_Qty"] = "";
                            drgetproducts["Delivery_Date"] = "";
                            drgetproducts["Quot_no"] = Rec_ID.ToString();
                            drgetproducts["EnqNo"] = getDiscPer.Enq_Master_ID.ToString();
                            drgetproducts["Enq_Item_ID"] = getDiscPer.Prod_Code.ToString();
                            drgetproducts["Remarks"] = "";

                            dtgetproducts.Rows.Add(drgetproducts);
                            dtgetfinalprducts = dtgetfinalprducts.AsEnumerable().Union(dtgetproducts.AsEnumerable()).CopyToDataTable();
                            //}
                            //}
                            dtgetproducts.Rows.Clear();
                            sno = sno + 1;
                        }
                    }
                }
                dtexisting = dtexisting.AsEnumerable().Union(dtgetfinalprducts.AsEnumerable()).CopyToDataTable();
                dtgetSelectedprducts = dtgetSelectedprducts.AsEnumerable().Union(dtexisting.AsEnumerable()).CopyToDataTable();

                dgProducts.DataSource = dtgetSelectedprducts;

                //txtCustPoNo.Text = "Multi";
                //txtSoNo.Text = "Multi";


                //dgProducts.DataSource = dtgetfinalprducts;
                groupBox2.Visible = false;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int sno = 0;
                DataTable dtgetSelectedprducts = new DataTable();
                //Check Whether Exisitng Products Already Selected in Main Grid
                if (dgProducts.Rows.Count > 1)
                {
                    dtexisting.Rows.Clear();
                    dtexisting.Columns.Clear();
                    dtexisting.Columns.Add("Item_Code", typeof(string));
                    dtexisting.Columns.Add("Int_Prod_Code", typeof(string));
                    dtexisting.Columns.Add("Item_Description", typeof(string));
                    dtexisting.Columns.Add("Item_Grade", typeof(string));
                    dtexisting.Columns.Add("HSN_Code", typeof(string));
                    dtexisting.Columns.Add("UOM", typeof(string));
                    dtexisting.Columns.Add("Qty", typeof(decimal));
                    dtexisting.Columns.Add("Basic_Price", typeof(string));
                    dtexisting.Columns.Add("Amt_Before_Disc", typeof(string));
                    dtexisting.Columns.Add("Disc_Per", typeof(string));
                    dtexisting.Columns.Add("Disc_Amt", typeof(string));
                    dtexisting.Columns.Add("Taxable_Value", typeof(string));
                    dtexisting.Columns.Add("CGST_Per", typeof(string));
                    dtexisting.Columns.Add("CGST_Amt", typeof(string));
                    dtexisting.Columns.Add("SGST_Per", typeof(string));
                    dtexisting.Columns.Add("SGST_Amt", typeof(string));
                    dtexisting.Columns.Add("IGST_Per", typeof(string));
                    dtexisting.Columns.Add("IGST_Amt", typeof(string));
                    dtexisting.Columns.Add("Total_Amount", typeof(string));
                    dtexisting.Columns.Add("Prod_Tole_Qty", typeof(string));
                    dtexisting.Columns.Add("Delivery_Date", typeof(string));
                    dtexisting.Columns.Add("Quot_no", typeof(string));
                    dtexisting.Columns.Add("EnqNo", typeof(string));
                    dtexisting.Columns.Add("Enq_Item_ID", typeof(string));
                    dtexisting.Columns.Add("Remarks", typeof(string));
                    //
                    for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                    {
                        DataRow dr;
                        dr = dtexisting.NewRow();
                        dr["Item_Code"] = sno+1;
                        dr["Int_Prod_Code"] = dgProducts.Rows[i].Cells["Int_Prod_Code"].Value.ToString();
                        dr["Item_Description"] = dgProducts.Rows[i].Cells["Item_Description"].Value.ToString();
                        dr["Item_Grade"] = dgProducts.Rows[i].Cells["Item_Grade"].Value.ToString();
                        dr["HSN_Code"] = dgProducts.Rows[i].Cells["HSN_Code"].Value.ToString();
                        dr["UOM"] = dgProducts.Rows[i].Cells["UOM"].Value.ToString();
                        dr["Qty"] = dgProducts.Rows[i].Cells["Qty"].Value.ToString();
                        dr["Basic_Price"] = dgProducts.Rows[i].Cells["Basic_Price"].Value.ToString();
                        dr["Amt_Before_Disc"] = dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value.ToString();
                        dr["Disc_Per"] = dgProducts.Rows[i].Cells["Disc_Per"].Value.ToString();
                        dr["Disc_Amt"] = dgProducts.Rows[i].Cells["Disc_Amt"].Value.ToString();
                        dr["Taxable_Value"] = dgProducts.Rows[i].Cells["Taxable_Value"].Value.ToString();
                        dr["CGST_Per"] = dgProducts.Rows[i].Cells["CGST_Per"].Value.ToString();
                        dr["CGST_Amt"] = dgProducts.Rows[i].Cells["CGST_Amt"].Value.ToString();
                        dr["SGST_Per"] = dgProducts.Rows[i].Cells["SGST_Per"].Value.ToString();
                        dr["SGST_Amt"] = dgProducts.Rows[i].Cells["SGST_Amt"].Value.ToString();
                        dr["IGST_Per"] = dgProducts.Rows[i].Cells["IGST_Per"].Value.ToString();
                        dr["IGST_Amt"] = dgProducts.Rows[i].Cells["IGST_Amt"].Value.ToString();
                        dr["Total_Amount"] = dgProducts.Rows[i].Cells["Total_Amount"].Value.ToString();
                        dr["Prod_Tole_Qty"] = dgProducts.Rows[i].Cells["Prod_Tole_Qty"].Value.ToString();
                        dr["Delivery_Date"] = dgProducts.Rows[i].Cells["Delivery_Date"].Value.ToString();
                        dr["Quot_no"] = dgProducts.Rows[i].Cells["Quot_no"].Value.ToString();
                        dr["EnqNo"] = dgProducts.Rows[i].Cells["EnqNo"].Value.ToString();
                        dr["Enq_Item_ID"] = dgProducts.Rows[i].Cells["Enq_Item_ID"].Value.ToString();
                        dr["Remarks"] = dgProducts.Rows[i].Cells["Remarks"].Value.ToString();
                        dtexisting.Rows.Add(dr);
                        sno = sno +1;

                    }
                    dtexisting.AcceptChanges();
                }



                dtgetproducts.Columns.Clear();
                dtgetproducts.Rows.Clear();
                dtgetproducts.Columns.Add("Item_Code", typeof(string));
                dtgetproducts.Columns.Add("Int_Prod_Code", typeof(string));
                dtgetproducts.Columns.Add("Item_Description", typeof(string));
                dtgetproducts.Columns.Add("Item_Grade", typeof(string));
                dtgetproducts.Columns.Add("HSN_Code", typeof(string));
                dtgetproducts.Columns.Add("UOM", typeof(string));
                dtgetproducts.Columns.Add("Qty", typeof(string));
                dtgetproducts.Columns.Add("Basic_Price", typeof(string));
                dtgetproducts.Columns.Add("Amt_Before_Disc", typeof(string));
                dtgetproducts.Columns.Add("Disc_Per", typeof(string));
                dtgetproducts.Columns.Add("Disc_Amt", typeof(string));
                dtgetproducts.Columns.Add("Taxable_Value", typeof(string));
                dtgetproducts.Columns.Add("CGST_Per", typeof(string));
                dtgetproducts.Columns.Add("CGST_Amt", typeof(string));
                dtgetproducts.Columns.Add("SGST_Per", typeof(string));
                dtgetproducts.Columns.Add("SGST_Amt", typeof(string));
                dtgetproducts.Columns.Add("IGST_Per", typeof(string));
                dtgetproducts.Columns.Add("IGST_Amt", typeof(string));
                dtgetproducts.Columns.Add("Total_Amount", typeof(string));
                dtgetproducts.Columns.Add("Prod_Tole_Qty", typeof(string));
                dtgetproducts.Columns.Add("Delivery_Date", typeof(string));
                dtgetproducts.Columns.Add("Quot_no", typeof(string));
                dtgetproducts.Columns.Add("EnqNo", typeof(string));
                dtgetproducts.Columns.Add("Enq_Item_ID", typeof(string));
                dtgetproducts.Columns.Add("Remarks", typeof(string));
                dtgetfinalprducts.Rows.Clear();
                //listBox.Items.Clear();
                // Get the selected items of SfDataGrid
                //var reflector = this.sfDataGrid1.View.GetPropertyAccessProvider();
                //var row = this.sfDataGrid1.SelectedItem;

                //string ProdCode;
                //string SoNo;
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
                        var ProdCodeCol = sfDataGrid1.Columns[1].MappingName;
                        var SONoCol = sfDataGrid1.Columns[0].MappingName;
                        //var record1 = sfDataGrid1.View.Records.GetItemAt(recordIndex);
                        //var cellVaue = (record1.GetType().GetProperty(mappingName).GetValue(record1, null).ToString());
                        if (rowData == item)
                        {
                            var Item_Code = (rowData.GetType().GetProperty("Item_Code").GetValue(rowData, null).ToString());
                            var Item_Description = (rowData.GetType().GetProperty("Item_Description").GetValue(rowData, null).ToString());
                            var Item_Grade = (rowData.GetType().GetProperty("Item_Grade").GetValue(rowData, null).ToString());

                            var UOM = (rowData.GetType().GetProperty("UOM").GetValue(rowData, null).ToString());
                            var PO_Qty = (rowData.GetType().GetProperty("Qty").GetValue(rowData, null).ToString());
                            var Basic_Price = (rowData.GetType().GetProperty("Basic_Price").GetValue(rowData, null).ToString());
                            var SO_Ref_No = (rowData.GetType().GetProperty("CustomerPONo").GetValue(rowData, null).ToString());
                            var Rec_ID = (rowData.GetType().GetProperty("Rec_ID").GetValue(rowData, null).ToString());

                            var getHSN = (from s in db.MaterialGrades
                                          where s.Material_Grade == Item_Grade.ToString()
                                          select new { s.HSNCode }).ToList();
                            string HSNCode = "";
                            if (getHSN.Count > 0)
                            {
                                HSNCode = getHSN[0].HSNCode;
                            }
                            //var getTolQty = (from s in db.Purchase_Order_Masters
                            //                 where s.Id == Convert.ToInt32(Rec_ID)
                            //                 select new { s.Qty_Tolerence }).FirstOrDefault();    
                            decimal DiscPer = 0;
                            var getDiscPer = (from s in db.Sale_Order_Childs
                                              join q in db.Sale_Order_Masters on s.So_Master_ID equals q.Id
                                              where s.So_Master_ID == Convert.ToInt32(Rec_ID) && s.Prod_Code == Item_Code
                                              select new { s.Disc_Per, s.Price, s.Int_Prod_Code, s.Amount, 
                                              s.Disc_Amount, s.CGST_Per, s.CGST_Amnt, s.SGST_Per, s.SGST_Amnt, 
                                                  s.IGST_Per, s.IGST_Amnt, s.Net_Amount,
                                                  s.Taxable_Value,s.enq_item_no,s.enq_Master_ID }).FirstOrDefault();

                            //DiscPer = Convert.ToDecimal(getDiscPer.Disc_Per);
                            drgetproducts = dtgetproducts.NewRow();
                            drgetproducts["Item_Code"] = sno+1; 
                            if (getDiscPer.Int_Prod_Code != null || getDiscPer.Int_Prod_Code.ToString() != null)
                            {
                                drgetproducts["Int_Prod_Code"] = getDiscPer.Int_Prod_Code.ToString();
                            }
                            drgetproducts["Item_Description"] = Item_Description.ToString();
                            drgetproducts["Item_Grade"] = Item_Grade.ToString();
                            drgetproducts["HSN_Code"] = HSNCode;
                            drgetproducts["UOM"] = UOM.ToString();
                            drgetproducts["Qty"] = PO_Qty.ToString();
                            drgetproducts["Basic_Price"] = getDiscPer.Price.ToString();
                            drgetproducts["Amt_Before_Disc"] = getDiscPer.Amount.ToString();
                            drgetproducts["Disc_Per"] = getDiscPer.Disc_Per.ToString();
                            drgetproducts["Disc_Amt"] = getDiscPer.Disc_Amount.ToString();
                            drgetproducts["Taxable_Value"] = getDiscPer.Taxable_Value.ToString();
                            drgetproducts["CGST_Per"] = getDiscPer.CGST_Per.ToString();
                            drgetproducts["CGST_Amt"] = getDiscPer.CGST_Amnt.ToString();
                            drgetproducts["SGST_Per"] = getDiscPer.SGST_Per.ToString();
                            drgetproducts["SGST_Amt"] = getDiscPer.SGST_Amnt.ToString();
                            drgetproducts["IGST_Per"] = getDiscPer.IGST_Per.ToString();
                            drgetproducts["IGST_Amt"] = getDiscPer.IGST_Amnt.ToString();
                            drgetproducts["Total_Amount"] = getDiscPer.Net_Amount.ToString();
                            drgetproducts["Prod_Tole_Qty"] = "0";
                            drgetproducts["Delivery_Date"] = "";
                            drgetproducts["Quot_no"] = Rec_ID.ToString(); ;
                            drgetproducts["EnqNo"] = getDiscPer.enq_Master_ID.ToString();
                            drgetproducts["Enq_Item_ID"] = getDiscPer.enq_item_no.ToString();
                            drgetproducts["Remarks"] = "";

                            dtgetproducts.Rows.Add(drgetproducts);
                            dtgetfinalprducts = dtgetfinalprducts.AsEnumerable().Union(dtgetproducts.AsEnumerable()).CopyToDataTable();
                            //}
                            //}
                            dtgetproducts.Rows.Clear();
                            sno = sno + 1;
                        }
                    }
                }
                dtexisting = dtexisting.AsEnumerable().Union(dtgetfinalprducts.AsEnumerable()).CopyToDataTable();
                dtgetSelectedprducts = dtgetSelectedprducts.AsEnumerable().Union(dtexisting.AsEnumerable()).CopyToDataTable();

                dgProducts.DataSource = dtgetSelectedprducts;

                //txtCustPoNo.Text = "Multi";
                //txtSoNo.Text = "Multi";


                //dgProducts.DataSource = dtgetfinalprducts;
                groupBox2.Visible = false;
            }

            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtOtherCharges_Leave(object sender, EventArgs e)
        {
            GetTot();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            JObject jsoncancel = JObject.Parse(txtConAddress.Text);
            txtAddress1.Text = (string)jsoncancel.SelectToken("Address1"); ;
            txtAddress2.Text = (string)jsoncancel.SelectToken("Address2");
            cmbCity.Text = (string)jsoncancel.SelectToken("City");
            txtstate.Text = (string)jsoncancel.SelectToken("State");
            txtPincode.Text = (string)jsoncancel.SelectToken("PinCode");
            txtStateCode.Text = (string)jsoncancel.SelectToken("StateCode");
            txtConGSTIN.Text = (string)jsoncancel.SelectToken("GSTIN");
            //txtConGSTNo.Text = State[0].GSTIN_NO;

            groupBox3.Visible = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            genConsigneeAddress CAddr = new genConsigneeAddress();
            CAddr.Address1 = txtAddress1.Text;

            CAddr.Address2 = txtAddress2.Text; ;
            CAddr.City = cmbCity.Text;
            CAddr.PinCode = Convert.ToInt32(txtPincode.Text);
            CAddr.State = txtstate.Text;
            CAddr.StateCode = txtStateCode.Text;
            CAddr.GSTIN = txtConGSTIN.Text;
            JavaScriptSerializer serializer = new JavaScriptSerializer();

            string json = JsonConvert.SerializeObject(CAddr, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            txtConAddress.Text = json;
            groupBox3.Visible = false;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            groupBox3.Visible = false;
        }

        private void CmbConsigneeName_Leave(object sender, EventArgs e)
        {
            if (Convert.ToInt32(CmbConsigneeName.SelectedValue) != 0)
            {
                var State = (from c in db.Supplier_informations
                             where c.ID == Convert.ToInt32(CmbConsigneeName.SelectedValue.ToString())
                             select new { c.Address_1, c.Address_2, c.City, c.State, c.GSTIN_NO, c.Pincode, c.StateCode }).ToList();
                if (State.Count > 0)
                {

                    genConsigneeAddress CAddr = new genConsigneeAddress();
                    CAddr.Address1 = State[0].Address_1;

                    CAddr.Address2 = State[0].Address_2; ;
                    CAddr.City = State[0].City;
                    if (State[0].Pincode == "")
                    {
                        MessageBox.Show("Consignee PIN CODE should not be blank and Min 6 Digits, Update the same by click on View/Update Address");
                        CAddr.PinCode = 0;
                    }
                    else
                    {
                        CAddr.PinCode = Convert.ToInt32(State[0].Pincode);
                    }
                    CAddr.State = State[0].State;
                    CAddr.StateCode = State[0].StateCode;
                    CAddr.GSTIN = State[0].GSTIN_NO;

                    JavaScriptSerializer serializer = new JavaScriptSerializer();

                    string json = JsonConvert.SerializeObject(CAddr, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings
                    {
                        NullValueHandling = NullValueHandling.Ignore
                    });





                    txtConAddress.Text = json;
                    //State[0].Address_1 + ", " + State[0].Address_2 + ", " + State[0].City + "," + State[0].Pincode + "," + State[0].State +","+State[0].StateCode;
                }
            }
        }

        private void txtFreight_Amt_Leave(object sender, EventArgs e)
        {
            GetTot();
        }

        private void dgProducts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        public void Save()
        {
            try
            {
                String myString = "";
                myString = txtSoNo.Text;
                if ((from u in db.Sale_Order_Masters where u.SO_NO == myString && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    myString = txtSoNo.Text;
                    db.sp_SO_Delete(myString, logIn.company,logIn.BU_ID);
                }
                else
                {
                    AutoincrementId();
                    myString = txtSoNo.Text;

                }
                    //if (frmGate.Modify.Contains(this.Text))
                    //{

                //dgProducts.Enabled = false;
                //var S = db.Sale_Order_Masters.Where(w => w.SO_NO == myString && w.Company_ID == logIn.company).FirstOrDefault();
                Sale_Order_Master S = new Sale_Order_Master();
                {
                    S.SO_NO = myString;
                    S.SODate = dpSODate.Value;
                    S.QuotNo = cmbQuotNo.Text;
                    //S.QuotDate = dpQuotDate.Value;
                    S.BuyerName = Convert.ToInt32(CmbBuyerName.SelectedValue.ToString());
                    S.ConsigneeName = Convert.ToInt32(CmbConsigneeName.SelectedValue.ToString());
                    S.Tax_Class = Convert.ToInt32(cmbTaxClass.SelectedValue.ToString());
                    S.Status = Convert.ToInt32(cmbStatus.SelectedValue.ToString()); 
                    S.TotalQty = (txtTotalQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotalQty.Text);
                    S.SubTotal = (txtSubTotal.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtSubTotal.Text);
                    S.Tot_Discount = (txtTotDiscount.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotDiscount.Text);
                    S.Tot_TaxableValue = (txtTot_TaxableValue.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTot_TaxableValue.Text);
                    S.CustomerPONo = (txtCustPoNo.Text == "") ? "" : txtCustPoNo.Text;                            
                    S.PODate = dpPODate.Value;
                    S.Cust_GST_No = (txtCustGSTNo.Text == "") ? "" : txtCustGSTNo.Text;
                    S.Tot_CGST_Amnt = (txtTot_CGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_CGST.Text);
                    S.Tot_SGST_Amnt = (txtTot_SGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_SGST.Text);
                    S.Tot_IGST_Amnt = (txtTot_IGST.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_IGST.Text);
                    S.Tot_Ord_Value = (txtTot_OrderValue.Text == "") ? Convert.ToDecimal("0.00") : Convert.ToDecimal(txtTot_OrderValue.Text);
                    S.Delivery_Date = dtDeliveryDate.Value;
                    S.So_Amend_No = (txtAmendNo.Text == "") ? "" : txtAmendNo.Text;
                    S.So_Amend_Date = dtAmendDate.Value;
                    S.Sale_office  = Convert.ToInt32(cmbSaleOffice.SelectedValue.ToString());
                    //S.SaleExecutive = Convert.ToInt32(cmbSaleExecutive.SelectedValue.ToString());
                    S.Price_Basis = Convert.ToInt32(cmbPriceBasis.SelectedValue.ToString());
                    S.Insurance_Scope = Convert.ToInt32(cmbInsurance.SelectedValue.ToString());
                    S.PaymentTerms = Convert.ToInt32(cmbPaymentTerms.SelectedValue.ToString());
                    S.Trasnport_Scope = Convert.ToInt32(cmbTransport_Scope.SelectedValue.ToString());
                    S.Transporter_Name = (cmbTransporter.Text == "") ? "" : cmbTransporter.Text;
                    S.Multi_Loc_Delivery = (chkMultiLocation.Checked == true) ? true : false;                            
                    S.Repeat_Order = (chkRepeatOrder.Checked == true) ? true : false;
                    S.Job_Work_Order = (chkJWORder.Checked == true) ? true : false;
                    //S.Old_Ord_Ref = (txt.Text == "") ? "" : txtOtherTerms.Text;
                    S.SEZ_Order = (chkSEZOrder.Checked == true) ? true : false;
                    S.Customer_Contact = (txtCustomeContact.Text == "") ? "" : txtCustomeContact.Text;
                    S.Other_Terms =   (txtOtherTerms.Text == "") ? "" : txtOtherTerms.Text;
                    S.Spl_Instructions = (txtSplInstructions.Text == "") ? "" : txtSplInstructions.Text;
                    S.Cust_Eail = (txtCustEmail.Text == "") ? "" : txtCustEmail.Text;
                    S.Frieght_Unit = (txtFreight_Rate.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtFreight_Rate.Text);
                    S.Frieght_Amount = (txtFreight_Amt.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtFreight_Amt.Text);
                    S.RM_Basic_Price = (txtRMbasicPrice.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtRMbasicPrice.Text);
                    S.Packing_Charges = (txtPackingCharges.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtPackingCharges.Text);
                    S.Insurance_Charges = (txtInsCharges.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtInsCharges.Text);
                    S.Other_Charges = (txtOtherCharges.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtOtherCharges.Text);
                    S.Delivery_Address = (txtConAddress.Text == "") ? "" : txtConAddress.Text;
                    S.Delivery_GSTIN = (txtConGSTNo.Text == "") ? "" : txtConGSTNo.Text;
                    S.Enq_No = (txtEnqNo.Text == "") ? "" : txtEnqNo.Text;

                    string Tests_Required = "";
                    for (int i = 0; i < chkTests.Items.Count; i++)
                    {
                        if (chkTests.GetItemChecked(i))
                        {
                            if (Tests_Required != "")
                            {
                                Tests_Required = Tests_Required + "," + chkTests.Items[i].ToString();
                            }
                            else
                            {
                                Tests_Required = chkTests.Items[i].ToString();
                            }
                        }
                    }
                    S.Tests_Required = Tests_Required;
                    string Standards_To_Refer = "";
                    for (int i = 0; i < chkStdRefer.Items.Count; i++)
                    {
                        if (chkStdRefer.GetItemChecked(i))
                        {
                            if (Standards_To_Refer != "")
                            {
                                Standards_To_Refer = Standards_To_Refer + "," + chkStdRefer.Items[i].ToString();
                            }
                            else
                            {
                                Standards_To_Refer = chkStdRefer.Items[i].ToString();
                            }
                        }
                    }
                    S.Standards_To_Refer = Standards_To_Refer;                  
                    S.RM_Available = cmbRMAvailable.Text;
                    S.Spec_Is_Clear = cmbSpecClear.Text;
                    S.Mfg_Feasibility = cmbMfgFeasibility.Text;


                    S.Company_ID = logIn.company;
                    S.BU_ID = logIn.BU_ID;
                    S.Created_By = lblCreatedBy.Text;                    
                    S.Modified_By = logIn.username + "-" + DateTime.Now;
                    db.Sale_Order_Masters.InsertOnSubmit(S);
                    db.SubmitChanges();
                }
                        //db.Transaction = transaction;
                for (int i = 0; i < dgProducts.RowCount - 1; i++)
                {
                    Sale_Order_Child SC = new Sale_Order_Child();
                    var d1 = (from a in db.Sale_Order_Masters where a.SO_NO == myString && a.Company_ID == logIn.company select new { a.Id }).ToList();
                    SC.So_Master_ID = d1[0].Id;
                    SC.SO_NO = myString;
                    SC.Prod_Code = (dgProducts.Rows[i].Cells["Item_Code"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Code"].Value).ToString();
                    SC.Product_Description = (dgProducts.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value).ToString();
                    SC.Prod_Grade = (dgProducts.Rows[i].Cells["Item_Grade"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value).ToString();
                    SC.Uom = (dgProducts.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["uom"].Value.ToString();

                    double amt = Convert.ToDouble(dgProducts.Rows[i].Cells["Qty"].Value);
                    //decimal qty = decimal.Round(Convert.ToDecimal(amt),5);
                    SC.Qty = amt;
                    //SC.Qty = (dgProducts.Rows[i].Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty"].Value);
                    SC.Price = (dgProducts.Rows[i].Cells["Basic_Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Basic_Price"].Value);

                    SC.Amount = (dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value);
                    SC.Disc_Per = (dgProducts.Rows[i].Cells["Disc_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Per"].Value);
                    SC.Disc_Amount = (dgProducts.Rows[i].Cells["Disc_Amt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Amt"].Value);
                    SC.Taxable_Value = (dgProducts.Rows[i].Cells["Taxable_Value"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Taxable_Value"].Value);
                    SC.CGST_Per = (dgProducts.Rows[i].Cells["CGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Per"].Value);
                    SC.SGST_Per = (dgProducts.Rows[i].Cells["SGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Per"].Value);
                    SC.IGST_Per = (dgProducts.Rows[i].Cells["IGST_Per"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Per"].Value);
                    SC.CGST_Amnt = (dgProducts.Rows[i].Cells["CGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Amt"].Value);
                    SC.SGST_Amnt = (dgProducts.Rows[i].Cells["SGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Amt"].Value);
                    SC.IGST_Amnt = (dgProducts.Rows[i].Cells["IGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Amt"].Value);
                    SC.Net_Amount = (dgProducts.Rows[i].Cells["Total_Amount"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Total_Amount"].Value);
                    SC.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value).ToString();
                    SC.Tole_Qty = (dgProducts.Rows[i].Cells["Prod_Tole_Qty"].Value == ""||dgProducts.Rows[i].Cells["Prod_Tole_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Prod_Tole_Qty"].Value);
                    SC.Int_Prod_Code = (dgProducts.Rows[i].Cells["Int_Prod_Code"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Int_Prod_Code"].Value).ToString();
                    SC.HSN_Code = (dgProducts.Rows[i].Cells["HSN_Code"].Value == null) ? "" : (dgProducts.Rows[i].Cells["HSN_Code"].Value).ToString();
                    SC.Del_Date = (dgProducts.Rows[i].Cells["Delivery_Date"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Delivery_Date"].Value).ToString();
                    SC.Quot_Master_ID = (dgProducts.Rows[i].Cells["Quot_No"].Value == DBNull.Value) ? Convert.ToInt32("0") : Convert.ToInt32(dgProducts.Rows[i].Cells["Quot_No"].Value);
                    SC.enq_Master_ID = (dgProducts.Rows[i].Cells["EnqNo"].Value == DBNull.Value) ? Convert.ToInt32("0") : Convert.ToInt32(dgProducts.Rows[i].Cells["EnqNo"].Value);
                    SC.enq_item_no = (dgProducts.Rows[i].Cells["Enq_Item_ID"].Value == DBNull.Value) ? Convert.ToInt32("0") : Convert.ToInt32(dgProducts.Rows[i].Cells["Enq_Item_ID"].Value);
                    SC.Company_ID = logIn.company;
                    db.Sale_Order_Childs.InsertOnSubmit(SC);
                }
                db.SubmitChanges();
                //transaction.Commit();               
                MessageBox.Show("Record Saved / Updated Successfully With Transaction Ref No : " + txtSoNo.Text);
                this.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        public void bindedit()
        {
            try
            {
                txtSoNo.Text = OrderManagement.Transactions.ListOfOrders.SO_No;
                String myString = "";
                myString = txtSoNo.Text;
                var da = (from obj in db.Sale_Order_Masters
                          where obj.SO_NO == txtSoNo.Text && obj.Company_ID == logIn.company
                          select obj).ToList();

                if (da.Count > 0)
                {
                    txtSoNo.Text = da[0].SO_NO.ToString();
                    dpSODate.Text = da[0].SODate.ToString();
                    //bindCustomer();
                    CmbBuyerName.SelectedValue = da[0].BuyerName;
                    cmbTaxClass.SelectedValue = da[0].Tax_Class;
                    CmbConsigneeName.SelectedValue = da[0].ConsigneeName;
                    txtCustGSTNo.Text = da[0].Cust_GST_No;
                    txtAmendNo.Text = da[0].So_Amend_No;
                    dtAmendDate.Text = da[0].So_Amend_Date.ToString();
                    if(da[0].Repeat_Order == true)
                    {
                        chkRepeatOrder.Checked = true;                    
                    }
                    else
                    {
                      chkRepeatOrder.Checked = false;                        
                    }
                    if (da[0].SEZ_Order == true)
                    {
                        chkSEZOrder.Checked = true;
                    }
                    else
                    {
                        chkSEZOrder.Checked = false;
                    }
                    if (da[0].Multi_Loc_Delivery == true)
                    {
                        chkMultiLocation.Checked = true;
                    }
                    else
                    {
                        chkMultiLocation.Checked = false;
                    }
                    if (da[0].Job_Work_Order == true)
                    {
                        chkJWORder.Checked = true;
                    }
                    else
                    {
                        chkJWORder.Checked = false;
                    }
                    //cmbCustomer.Enabled = false;
                    cmbQuotNo.Text = da[0].QuotNo;
                    //dpQuotDate.Text = da[0].QuotDate.ToString();

                    txtTotalQty.Text = da[0].TotalQty.ToString();
                    txtSubTotal.Text = da[0].SubTotal.ToString();
                    txtTotDiscount.Text = da[0].Tot_Discount.ToString();
                    txtTot_TaxableValue.Text = da[0].Tot_TaxableValue.ToString();
                    txtTot_CGST.Text = da[0].Tot_CGST_Amnt.ToString();
                    txtTot_SGST.Text = da[0].Tot_SGST_Amnt.ToString();
                    txtTot_IGST.Text = da[0].Tot_IGST_Amnt.ToString();
                    txtTot_OrderValue.Text = da[0].Tot_Ord_Value.ToString();
                    txtCustPoNo.Text = da[0].CustomerPONo;
                    dpPODate.Text = da[0].PODate.ToString();
                    dtDeliveryDate.Text = da[0].Delivery_Date.ToString();
                    txtFreight_Rate.Text = da[0].Frieght_Unit.ToString();
                    txtFreight_Amt.Text = da[0].Frieght_Amount.ToString();
                    txtRMbasicPrice.Text = da[0].RM_Basic_Price.ToString();
                    txtOtherCharges.Text = da[0].Other_Charges.ToString();
                    txtPackingCharges.Text = da[0].Packing_Charges.ToString();
                    txtInsCharges.Text = da[0].Insurance_Charges.ToString();
                    if (da[0].Sale_office == null)
                    {
                        cmbSaleOffice.SelectedValue = 36;
                    }
                    else
                    {
                        cmbSaleOffice.SelectedValue = da[0].Sale_office;
                    }
                    if (da[0].Price_Basis == null)
                    {
                        cmbPriceBasis.SelectedValue = 7;
                    }
                    else
                    {
                        cmbPriceBasis.SelectedValue = da[0].Price_Basis;
                    }
                    if (da[0].Insurance_Scope == null)
                    {
                        cmbInsurance.SelectedValue = 11;
                    }
                    else
                    {
                        cmbInsurance.SelectedValue = da[0].Insurance_Scope; 
                    }

                    if (da[0].PaymentTerms == null)
                    {
                        cmbPaymentTerms.SelectedValue = 35;
                    }
                    else
                    {
                        cmbPaymentTerms.SelectedValue = da[0].PaymentTerms;
                    }

                    if (da[0].Trasnport_Scope == null)
                    {
                        cmbTransport_Scope.SelectedValue = 19;
                    }
                    else
                    {
                        cmbTransport_Scope.SelectedValue = da[0].Trasnport_Scope;
                    }

                    if (da[0].Tests_Required != null)
                    {
                        string MP = da[0].Tests_Required.ToString();
                        string[] values = MP.Split(',');
                        for (int j = 0; j < values.Length; j++)
                        {
                            values[j] = values[j].Trim();
                            string m = values[j].ToString();
                            for (int i = 0; i < chkTests.Items.Count; i++)
                            {
                                if (chkTests.Items[i].ToString() == m)
                                {
                                    chkTests.SetItemChecked(i, true);
                                }
                            }
                        }
                    }

                    if (da[0].Standards_To_Refer != null)
                    {
                        string MP = da[0].Standards_To_Refer.ToString();
                        string[] values = MP.Split(',');
                        for (int j = 0; j < values.Length; j++)
                        {
                            values[j] = values[j].Trim();
                            string m = values[j].ToString();
                            for (int i = 0; i < chkStdRefer.Items.Count; i++)
                            {
                                if (chkStdRefer.Items[i].ToString() == m)
                                {
                                    chkStdRefer.SetItemChecked(i, true);
                                }
                            }
                        }
                    }
                    cmbRMAvailable.Text = da[0].RM_Available;
                    cmbSpecClear.Text = da[0].Spec_Is_Clear;
                    cmbMfgFeasibility.Text = da[0].Mfg_Feasibility;

                    cmbTransporter.Text = da[0].Transporter_Name;
                    txtCustomeContact.Text = da[0].Customer_Contact;
                    txtConAddress.Text = da[0].Delivery_Address;
                    txtConGSTNo.Text = da[0].Delivery_GSTIN;
                    txtOtherTerms.Text = da[0].Other_Terms;
                    txtSplInstructions.Text = da[0].Spl_Instructions;                    
                    cmbStatus.SelectedValue = da[0].Status;      
                    lblCreatedBy.Text = da[0].Created_By;
                    lblModified.Text = da[0].Modified_By;                    
                }


                var dm1 = (from s in db.Sale_Order_Childs
                           where s.SO_NO == myString && s.Company_ID == logIn.company
                           

                           select new

                           {
                               Item_Code = s.Prod_Code,
                               Int_Prod_Code = s.Int_Prod_Code.Trim(),
                               Item_Description = s.Product_Description.Trim(),
                               Item_Grade = s.Prod_Grade.Trim(),
                               HSN_Code= s.HSN_Code.Trim(),
                               UOM = s.Uom.Trim(),
                               s.Qty,
                               Basic_Price = s.Price,
                               Amt_Before_Disc = s.Amount,
                               s.Disc_Per,
                               Disc_Amt = s.Disc_Amount,
                               s.Taxable_Value,
                               s.CGST_Per,
                               CGST_Amt = s.CGST_Amnt,
                               s.SGST_Per,
                               SGST_Amt = s.SGST_Amnt,
                               s.IGST_Per,
                               IGST_Amt = s.IGST_Amnt,
                               Total_Amount = s.Net_Amount,                                                      
                               Prod_Tole_Qty = s.Tole_Qty,
                               Delivery_date = s.Del_Date.Trim(),
                               Quot_No = s.Quot_Master_ID,
                               EnqNo = s.enq_Master_ID,
                               Enq_Item_ID= s.enq_item_no,
                               s.Remarks
                           });




                SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataTable dtr = new DataTable();
                da2.Fill(dtr);
                if (dtr.Rows.Count >= 0)
                    dgProducts.DataSource = dtr;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public class genConsigneeAddress
        {
            public string Address1 { get; set; }
            public string Address2 { get; set; }
            public string City { get; set; }
            public int PinCode { get; set; }
            public string State { get; set; }
            public string StateCode { get; set; }
            public string GSTIN { get; set; }

        }
        public void GetTot()
        {
            try
            {
                double totQty = 0;
                decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
                for (int i = 0; i < dgProducts.Rows.Count - 1; i++)
                {
                    totQty += (dgProducts.Rows[i].Cells["Qty"].Value.ToString() == "" || dgProducts.Rows[i].Cells["Qty"].Value == null || dgProducts.Rows[i].Cells["Qty"].Value == DBNull.Value) ? Convert.ToDouble(0) : Convert.ToDouble(dgProducts.Rows[i].Cells["Qty"].Value);
                    //x += (dgProducts.Rows[i].Cells["Inv_Qty"].Value == "" || dgProducts.Rows[i].Cells["Inv_Qty"].Value == null || dgProducts.Rows[i].Cells["Inv_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Inv_Qty"].Value);
                    y += (dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value.ToString() == "" || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == null || dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Amt_Before_Disc"].Value);
                    q += (dgProducts.Rows[i].Cells["Disc_Amt"].Value.ToString() == "" || dgProducts.Rows[i].Cells["Disc_Amt"].Value == null || dgProducts.Rows[i].Cells["Disc_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Disc_Amt"].Value);
                    v += (dgProducts.Rows[i].Cells["Taxable_Value"].Value.ToString() == "" || dgProducts.Rows[i].Cells["Taxable_Value"].Value == null || dgProducts.Rows[i].Cells["Taxable_Value"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Taxable_Value"].Value);
                    cg += (dgProducts.Rows[i].Cells["CGST_Amt"].Value.ToString() == "" || dgProducts.Rows[i].Cells["CGST_Amt"].Value == null || dgProducts.Rows[i].Cells["CGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Amt"].Value);
                    sg += (dgProducts.Rows[i].Cells["SGST_Amt"].Value.ToString() == "" || dgProducts.Rows[i].Cells["SGST_Amt"].Value == null || dgProducts.Rows[i].Cells["SGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Amt"].Value);
                    ig += (dgProducts.Rows[i].Cells["IGST_Amt"].Value.ToString() == "" || dgProducts.Rows[i].Cells["IGST_Amt"].Value == null || dgProducts.Rows[i].Cells["IGST_Amt"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Amt"].Value);
                    totA += (dgProducts.Rows[i].Cells["Total_Amount"].Value.ToString() == "" || dgProducts.Rows[i].Cells["Total_Amount"].Value == null || dgProducts.Rows[i].Cells["Total_Amount"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProducts.Rows[i].Cells["Total_Amount"].Value);
                    cgstPer = Convert.ToDecimal(dgProducts.Rows[i].Cells["CGST_Per"].Value);
                    sgstPer = Convert.ToDecimal(dgProducts.Rows[i].Cells["SGST_Per"].Value);
                    igstPer = Convert.ToDecimal(dgProducts.Rows[i].Cells["IGST_Per"].Value);
                }

                txtTotalQty.Text = totQty.ToString(".00000");
                txtSubTotal.Text = y.ToString("0.00");
                txtTotDiscount.Text = q.ToString(".00");
                decimal fAmt = (txtFreight_Amt.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtFreight_Amt.Text);
                decimal OthAmt = (txtOtherCharges.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtOtherCharges.Text);
                decimal PFAmt = (txtPackingCharges.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtPackingCharges.Text);

                decimal InsAmt = (txtInsCharges.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtInsCharges.Text);

                txtTot_TaxableValue.Text = (v + fAmt + OthAmt+PFAmt+InsAmt).ToString(".00");
                decimal taxvalue = Convert.ToDecimal(txtTot_TaxableValue.Text);
                decimal cgst = (taxvalue * cgstPer) / 100;
                decimal sgst = (taxvalue * sgstPer) / 100;
                decimal igst = (taxvalue * igstPer) / 100;

                txtTot_CGST.Text = cgst.ToString(".00");
                txtTot_SGST.Text = sgst.ToString(".00");
                txtTot_IGST.Text = igst.ToString(".00");
                decimal AmtForTCs = (v);
                //decimal AmtForTCs = (fAmt + OthAmt + v + cgst + sgst + igst);
                txtTot_OrderValue.Text = (taxvalue + cgst + sgst + igst ).ToString(".00");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }
    }
}
