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
using System.Data.OleDb;

using System.Configuration;
using System.Drawing.Printing;

namespace ioneNet.OrderManagement
{
    public partial class SaleOrderr_GST : Form
    {
        #region DeclarationVariable
        iOne_LinqSqlDataContext db = new iOne_LinqSqlDataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        DataRow dr;

        System.Data.Common.DbTransaction transaction;
        public static int global = 0;
        //AppCode.Logic obj = new AppCode.Logic();
        public static string saleOrder;
        private int rowindex;
        DataTable dt = new DataTable();
        DataTable dt1; string name;
        public static string saleOrderReport;
        public static string FName;
        private DateTimePicker cellDateTimePicker;
        private List<int> dateColumnsIndexes;


        public SaleOrderr_GST()
        {
            InitializeComponent();
            dgProductData.DataSourceChanged += new EventHandler(dgProductData_DataSourceChanged);
        }
        #endregion

        #region Method

        public void Search()
        {
            try
            {
                if ((from u in db.SaleOrders where u.SO_Code == SaleOrderReport.SONO && u.Company == AppCode.GlobalAccess.companyName select u).Count() > 0)
                {
                    string tb = SaleOrderReport.SONO;
                    // string 
                    var d = (from s in db.SaleOrders where s.SO_Code == tb && s.Company == AppCode.GlobalAccess.companyName select s).ToList();
                    TxtSONO.Text = SaleOrderReport.SONO;
                    txtCustomerPONO.Text = d[0].CustomerPONo;
                    dpPODate.Value = Convert.ToDateTime(d[0].PODate);
                    txtSaleExecutive.Text = d[0].SaleExecutive;
                    txtCGSTAmount.Text = d[0].CGST_Amnt.ToString();
                    txtTotalQty.Text = d[0].TotalQty.ToString();
                    txtMAV.Text = d[0].TotalMAV.ToString();
                    txtTotalAmount.Text = d[0].TotalAmount.ToString();
                    txtTotalInvValue.Text = d[0].TotalInvValue.ToString();
                    txtAccValue.Text = d[0].TotalASSValue.ToString();
                    txtSubtotal.Text = d[0].SubTotal.ToString();
                    txtSGSTAmt.Text = d[0].SGST_Amnt.ToString();
                    txtIGSTAmount.Text = d[0].IGST_Amnt.ToString();
                    txtSaleRegion.Text = d[0].SaleRegion;
                    CmbTaxclass.Text = d[0].TaxClass;
                    CmbStatus.Text = d[0].Status;
                    CmbQuotNO.Text = d[0].QuotNo;
                    dpQuotDate.Value = Convert.ToDateTime(d[0].QuotDate);

                    // blindbuyer();
                    var Buyerblind = (from m in db.AccountMasters where  m.AccCode == d[0].BuyerName select new { m.AccCode, m.AccName }).Distinct().ToList();
                    if (Buyerblind.Count > 0)
                    {
                        CmbBuyerName.Text = Buyerblind[0].AccName;
                        CmbBuyerName.ValueMember = "AccCode";
                        CmbBuyerName.DisplayMember = "AccName";
                    }
                    var ConSigneeblind = (from m in db.AccountMasters where m.AccCode == d[0].ConsigneeName select new { m.AccCode, m.AccName }).Distinct().ToList();
                    if (ConSigneeblind.Count > 0)
                    {
                        CmbConsigneeName.Text = ConSigneeblind[0].AccName;
                        CmbConsigneeName.ValueMember = "AccCode";
                        CmbConsigneeName.DisplayMember = "AccName";
                    }

                    var f = (from s in db.SaleOrder_ProductDatas
                             join p in db.ProdMasters on s.ProductCode equals p.Product_code
                             // join EI in db.ExciseInvoice_ProductDetails on s.SO_Code equals EI.SONo
                             where s.SO_Code == TxtSONO.Text && s.Company == AppCode.GlobalAccess.companyName
                             && s.Company == p.Comp_Name && p.Comp_Name == AppCode.GlobalAccess.companyName
                             select new
                             {
                                 s.ProductCode,
                                 s.Product_Category,
                                 p.Product_Name,
                                 s.Grade,
                                 p.Unit_Sale,
                                 s.Qty,
                                 s.Price,
                                 s.Amount,
                                 s.MAC_P,
                                 s.MAV,
                                 s.AssValue,
                                 s.CGST_Per,
                                 s.CGST_Amnt,
                                 s.SGST_Per,
                                 s.SGST_Amnt,
                                 s.IGST_Per,
                                 s.IGST_Amnt,
                                 s.Net_Amount
                             });


                    SqlCommand cmd2 = (SqlCommand)db.GetCommand(f);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    DataTable dt = new DataTable();
                    da2.Fill(dt);
                    if (dt.Rows.Count > 0)
                        dgProductData.DataSource = dt;


                    var dm2 = (from s in db.SaleOrder_OtherTerms
                               where s.SO_Code == TxtSONO.Text && s.Company == AppCode.GlobalAccess.companyName
                               select new
                               { s.Term, s.Condition });

                    SqlCommand cmd3 = (SqlCommand)db.GetCommand(dm2);
                    SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                    DataTable dt1 = new DataTable();
                    da3.Fill(dt1);
                    if (dt1.Rows.Count > 0)
                        dgTermsandConditions.DataSource = dt1;

                    var t = (from s in db.SaleOrder_OtherTerms where s.SO_Code == TxtSONO.Text && s.Company == AppCode.GlobalAccess.companyName select s).FirstOrDefault();
                    if (t != null)
                    {
                        cmbPackingForwaring.Text = t.PackingForwarding;
                        CmbPaymentTerms.Text = t.PaymentTerms;
                    }


                    var dm = (from s in db.SaleOrder_DeliverySchedules
                              where s.SO_Code == TxtSONO.Text && s.Company == AppCode.GlobalAccess.companyName
                              select new
                              { s.Product_Name, s.QtyToDeliver, s.Delivery_Before });

                    SqlCommand cmd = (SqlCommand)db.GetCommand(dm);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt2 = new DataTable();
                    da.Fill(dt2);
                    if (dt2.Rows.Count >= 0)
                        dgDeliverySchedule.DataSource = dt2;
                    global -= 1;
                    CmbTaxclass.Text = d[0].TaxClass;
                    // blindqty();
                    bindDeliverProductName();
                }
                else
                {
                    MessageBox.Show("Record Not Existing,please Try Existing Record");
                }
            }
            catch (Exception ex)
            {
            }

        }

        public void BindQuoto()
        {
            try
            {
                var data = (from c in db.sp_BlindQuotNo(AppCode.GlobalAccess.companyName) select c).ToList();

                if (data.Count() > 0)
                {
                    CmbQuotNO.DataSource = data;
                    CmbQuotNO.DisplayMember = "QuotNo";

                    if (CmbQuotNO.Items.Count > 0)
                    {
                        CmbQuotNO.SelectedIndex = -1;
                    }
                }

            }
            catch (Exception ex)
            {
            }

        }

        public void bindPayment()
        {
            try
            {
                var PaymentTermsblind = (from m in db.Payment_Terms_Masters  select m.Payment_Terms).Distinct().ToList();
                if (PaymentTermsblind.Count > 0)
                {
                    CmbPaymentTerms.DataSource = PaymentTermsblind;
                }
                if (CmbPaymentTerms.Items.Count > 0)
                    CmbPaymentTerms.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
            }

        }

        public void bindPricebasis()
        {
            try
            {
                var Price_Basisbind = (from m in db.Price_Basis_Masters select m.Price_Basis).Distinct().ToList();
                if (Price_Basisbind.Count > 0)
                {
                    cmbPriceBasis.DataSource = Price_Basisbind;
                }
                if (cmbPriceBasis.Items.Count > 0)
                    cmbPriceBasis.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
            }

        }

        public void bindbuyer()
        {
            try
            {
                var Buyerblind = (from m in db.AccountMasters where  m.CustAccount == "true" select new { m.AccCode, m.AccName }).Distinct().ToList();
                if (Buyerblind.Count > 0)
                {
                    CmbBuyerName.DataSource = Buyerblind;
                    CmbBuyerName.ValueMember = "AccCode";
                    CmbBuyerName.DisplayMember = "AccName";
                    CmbConsigneeName.DataSource = Buyerblind;
                    CmbConsigneeName.ValueMember = "AccCode";
                    CmbConsigneeName.DisplayMember = "AccName";

                }
                if (CmbBuyerName.Items.Count > 0)
                    CmbBuyerName.SelectedIndex = -1;
                if (CmbConsigneeName.Items.Count > 0)
                    CmbConsigneeName.SelectedIndex = -1;
                txtSaleExecutive.Text = "";
                txtSaleRegion.Text = "";
            }
            catch (Exception ex)
            {
            }
        }

        public void BindTaxClass()
        {
            try
            {
                var TaxClass = (from m in db.Prod_TaxRates
                                    // where m.Company == AppCode.GlobalAccess.companyName
                                where m.TaxRate.Contains("Sales")
                                select m.TaxRate).Distinct().ToList();
                if (TaxClass.Count() > 0)
                {
                    CmbTaxclass.DataSource = TaxClass;
                }
                if (CmbTaxclass.Items.Count > 0)
                    CmbTaxclass.SelectedIndex = -1;
                if (CmbTaxclass.Items.Count == 1)
                {
                    CmbTaxclass.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {


            }
        }
        public void bindAct()
        {
            try
            {

                var bindcreditAct = (from m in db.AccountMasters  select m.AccName).Distinct().ToList();


                if (bindcreditAct.Count > 0)
                {
                    cmbAccountGroup.DataSource = bindcreditAct;

                }
                if (cmbAccountGroup.Items.Count > 0)
                    cmbAccountGroup.SelectedIndex = -1;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void clear()
        {
            try
            {
                BindQuoto();
                txtSaleExecutive.Text = "";
                txtSaleRegion.Text = "";
                foreach (Control x in this.Controls)
                {
                    foreach (Control d in groupBox1.Controls)
                    {
                        if (d is TextBox)
                            (d as TextBox).Clear();
                        if (d is ComboBox)
                            (d as ComboBox).SelectedIndex = -1;
                    }
                    foreach (Control d in groupBox4.Controls)
                    {
                        if (d is TextBox)
                            (d as TextBox).Clear();
                    }
                }
                if (dgDeliverySchedule.Rows.Count > 0)
                {
                    for (int i = 0; i < dgDeliverySchedule.Rows.Count - 1; i++)
                    {
                        dgDeliverySchedule.Rows.RemoveAt(i);
                        i--;
                        while (dgDeliverySchedule.Rows.Count == 0)
                            continue;
                    }
                }
                if (dgTermsandConditions.Rows.Count > 0)
                {
                    for (int i = 0; i < dgTermsandConditions.Rows.Count - 1; i++)
                    {
                        dgTermsandConditions.Rows.RemoveAt(i);
                        i--;
                        while (dgTermsandConditions.Rows.Count == 0)
                            continue;
                    }
                }
                if (dgProductData.Rows.Count > 0)
                {
                    for (int i = 0; i < dgProductData.Rows.Count - 1; i++)
                    {
                        dgProductData.Rows.RemoveAt(i);
                        i--;
                        while (dgProductData.Rows.Count == 0)
                            continue;
                    }
                }

                AutoincrementId();
                bindAct();
                if (CmbPaymentTerms.Items.Count > 0)
                {
                    CmbPaymentTerms.SelectedIndex = -1;
                }
                cmbPackingForwaring.SelectedIndex = -1;
                TxtSONO.Focus();
                name = "";
            }
            catch (Exception ex)
            {
            }
        }

        public void bindDeliverProductName()
        {
            try
            {
                DataGridViewComboBoxColumn combo = (DataGridViewComboBoxColumn)dgDeliverySchedule.Columns["ProductionName"];
                dt1 = new DataTable();

                dt1.Columns.Add("ProductionName", typeof(string));

                for (int i = 0; i < dgProductData.Rows.Count - 1; i++)
                {
                    if (dgProductData.Rows[i].Cells["ProductName"].Value != null && dgProductData.Rows[i].Cells["ProductName"].Value != DBNull.Value)
                    {

                        string ProductionName = dgProductData.Rows[i].Cells["ProductName"].Value.ToString();

                        dt1.Rows.Add(ProductionName);

                    }
                }
                if (dt1.Rows.Count > 0)
                {
                    combo.DataSource = dt1;
                    combo.DisplayMember = "ProductionName";
                }

            }
            catch (Exception)
            {
            }
        }

        public void AutoincrementId()
        {
            try
            {
                var auto = db.Sp_autoincrement_SaleOrder(AppCode.GlobalAccess.companyName);
                TxtSONO.Text = auto.FirstOrDefault().SO;

            }
            catch (Exception ex)
            {
                // MessageBox.Show(ex.Message + " While AutoIncrement Getting Error ", "Sale Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void gridvaluetax()
        {
            try
            {
                double SUMQty = 0;
                double SUMMAV = 0;
                double SUMAmount = 0;
                double TotalAssValue = 0;
                double sumCGSTP = 0;
                double sumCGSTAmount = 0;
                double sumSub_Total = 0;
                double sumSGSTP = 0;
                double sumSGSTA = 0;
                double sumIGSTP = 0;
                double sumIGSTA = 0;
                double sumNet_Amount = 0;

                Cursor.Current = Cursors.WaitCursor;

                for (int i = 0; i < dgProductData.Rows.Count - 1; i++)
                {
                    if (dgProductData.Rows.Count > 0)
                    {
                        if (dgProductData.Rows[i].Cells["ProductName"].Value != null && dgProductData.Rows[i].Cells["ProductName"].Value != DBNull.Value)
                        {
                            var getProductName = (from s in db.ProdMasters where  s.Product_Name == dgProductData.Rows[i].Cells["ProductName"].Value.ToString().Trim() select s).FirstOrDefault();

                            if (getProductName != null)
                            {
                                dgProductData.Rows[i].Cells["ProductCode"].Value = getProductName.Product_code.ToString();
                                dgProductData.Rows[i].Cells["UOM"].Value = getProductName.Unit_Sale.ToString();
                                dgProductData.Rows[i].Cells["MAC"].Value = getProductName.MoldAmortisationCost.ToString();
                            }
                        }


                        decimal Qty, MAC, price, MAV, Amount, AssValue, CGSTP, CGSTAmount, SGSTP, SGSTA, IGSTP, IGSTA, InvValue = 0;


                        Qty = (dgProductData.Rows[i].Cells["Qty"].Value == "" || dgProductData.Rows[i].Cells["Qty"].Value == DBNull.Value || dgProductData.Rows[i].Cells["Qty"].Value == null) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductData.Rows[i].Cells["Qty"].Value);
                        MAC = (dgProductData.Rows[i].Cells["MAC"].Value == "" || dgProductData.Rows[i].Cells["MAC"].Value == DBNull.Value || dgProductData.Rows[i].Cells["MAC"].Value == null) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductData.Rows[i].Cells["MAC"].Value);
                        price = (dgProductData.Rows[i].Cells["Price"].Value == "" || dgProductData.Rows[i].Cells["Price"].Value == DBNull.Value || dgProductData.Rows[i].Cells["Price"].Value == null) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductData.Rows[i].Cells["Price"].Value);
                        MAV = (dgProductData.Rows[i].Cells["MAV"].Value == "" || dgProductData.Rows[i].Cells["MAV"].Value == DBNull.Value || dgProductData.Rows[i].Cells["MAV"].Value == null) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductData.Rows[i].Cells["MAV"].Value);

                        Amount = (dgProductData.Rows[i].Cells["Amount"].Value == "" || dgProductData.Rows[i].Cells["Amount"].Value == DBNull.Value || dgProductData.Rows[i].Cells["Amount"].Value == null) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductData.Rows[i].Cells["Amount"].Value);
                        AssValue = (dgProductData.Rows[i].Cells["AssValue"].Value == "" || dgProductData.Rows[i].Cells["AssValue"].Value == DBNull.Value || dgProductData.Rows[i].Cells["AssValue"].Value == null) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductData.Rows[i].Cells["AssValue"].Value);

                        CGSTP = (dgProductData.Rows[i].Cells["CGST_Per"].Value == "" || dgProductData.Rows[i].Cells["CGST_Per"].Value == DBNull.Value || dgProductData.Rows[i].Cells["CGST_Per"].Value == null) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductData.Rows[i].Cells["CGST_Per"].Value);
                        CGSTAmount = (dgProductData.Rows[i].Cells["CGST_Amnt"].Value == "" || dgProductData.Rows[i].Cells["CGST_Amnt"].Value == DBNull.Value || dgProductData.Rows[i].Cells["CGST_Amnt"].Value == null) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductData.Rows[i].Cells["CGST_Amnt"].Value);

                        SGSTP = (dgProductData.Rows[i].Cells["SGST_Per"].Value == "" || dgProductData.Rows[i].Cells["SGST_Per"].Value == DBNull.Value || dgProductData.Rows[i].Cells["SGST_Per"].Value == null) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductData.Rows[i].Cells["SGST_Per"].Value);
                        SGSTA = (dgProductData.Rows[i].Cells["SGST_Amnt"].Value == "" || dgProductData.Rows[i].Cells["SGST_Amnt"].Value == DBNull.Value || dgProductData.Rows[i].Cells["SGST_Amnt"].Value == null) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductData.Rows[i].Cells["SGST_Amnt"].Value);

                        IGSTP = (dgProductData.Rows[i].Cells["IGST_Per"].Value == "" || dgProductData.Rows[i].Cells["IGST_Per"].Value == DBNull.Value || dgProductData.Rows[i].Cells["IGST_Per"].Value == null) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductData.Rows[i].Cells["IGST_Per"].Value);
                        IGSTA = (dgProductData.Rows[i].Cells["IGST_Amnt"].Value == "" || dgProductData.Rows[i].Cells["IGST_Amnt"].Value == DBNull.Value || dgProductData.Rows[i].Cells["IGST_Amnt"].Value == null) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductData.Rows[i].Cells["IGST_Amnt"].Value);
                        InvValue = (dgProductData.Rows[i].Cells["Net_Amount"].Value == "" || dgProductData.Rows[i].Cells["Net_Amount"].Value == DBNull.Value || dgProductData.Rows[i].Cells["Net_Amount"].Value == null) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProductData.Rows[i].Cells["Net_Amount"].Value);


                        Amount = (Qty * price);
                        MAV = (Qty * MAC);
                        AssValue = Amount - MAV;
                        dgProductData.Rows[i].Cells["Qty"].Value = Qty.ToString("0.00");
                        dgProductData.Rows[i].Cells["Price"].Value = price.ToString("0.00");
                        dgProductData.Rows[i].Cells["MAV"].Value = MAV.ToString("0.00");
                        dgProductData.Rows[i].Cells["Amount"].Value = Amount.ToString("0.00");
                        dgProductData.Rows[i].Cells["AssValue"].Value = AssValue.ToString("0.00");

                        if (CmbTaxclass.Text != "")
                        {
                            string s1 = dgProductData.Rows[i].Cells["ProductName"].Value.ToString().Trim();
                            var tax = (from s in db.sp_get_PurchaseVocher(s1, CmbTaxclass.Text.Trim()) select s).ToList();
                            if (tax.Count > 0)
                            {
                                CGSTP = Convert.ToDecimal(tax[0].CGST_Per);
                                SGSTP = Convert.ToDecimal(tax[0].SGST_Per);
                                IGSTP = Convert.ToDecimal(tax[0].IGST_Per);
                            }
                        }

                        CGSTAmount = Convert.ToDecimal((AssValue) * (CGSTP) / 100);
                        SGSTA = Convert.ToDecimal((AssValue) * (SGSTP) / 100);
                        IGSTA = Convert.ToDecimal((AssValue) * (IGSTP) / 100);

                        InvValue = AssValue + CGSTAmount + SGSTA + IGSTA;

                        dgProductData.Rows[i].Cells["CGST_Per"].Value = CGSTP.ToString("0.00");
                        dgProductData.Rows[i].Cells["CGST_Amnt"].Value = CGSTAmount.ToString("0.00");

                        dgProductData.Rows[i].Cells["SGST_Per"].Value = SGSTP.ToString("0.00");
                        dgProductData.Rows[i].Cells["SGST_Amnt"].Value = SGSTA.ToString("0.00");

                        dgProductData.Rows[i].Cells["IGST_Per"].Value = IGSTP.ToString("0.00");
                        dgProductData.Rows[i].Cells["IGST_Amnt"].Value = IGSTA.ToString("0.00");

                        dgProductData.Rows[i].Cells["Net_Amount"].Value = InvValue.ToString("0.00");

                        SUMQty += Convert.ToDouble(Qty);//((dgProductData.Rows[i].Cells["Qty"].Value == DBNull.Value) || (dgProductData.Rows[i].Cells["Qty"].Value == null)) ? Convert.ToDouble("00") : Convert.ToDouble(dgProductData.Rows[i].Cells["Qty"].Value);
                        SUMMAV += Convert.ToDouble(MAV);//((dgProductData.Rows[i].Cells["MAV"].Value == DBNull.Value) || (dgProductData.Rows[i].Cells["MAV"].Value == null)) ? Convert.ToDouble("00") : Convert.ToDouble(dgProductData.Rows[i].Cells["MAV"].Value);
                        SUMAmount += Convert.ToDouble(Amount); //((dgProductData.Rows[i].Cells["Amount"].Value == DBNull.Value) || (dgProductData.Rows[i].Cells["Amount"].Value == null)) ? Convert.ToDouble("00") : Convert.ToDouble(dgProductData.Rows[i].Cells["Amount"].Value);
                        TotalAssValue += Convert.ToDouble(AssValue);//((dgProductData.Rows[i].Cells["AssValue"].Value == DBNull.Value) || (dgProductData.Rows[i].Cells["AssValue"].Value == null)) ? Convert.ToDouble("00") : Convert.ToDouble(dgProductData.Rows[i].Cells["AssValue"].Value);
                        sumCGSTP += Convert.ToDouble(CGSTP); //((dgProductData.Rows[i].Cells["EDP"].Value == DBNull.Value) || (dgProductData.Rows[i].Cells["EDP"].Value == null)) ? Convert.ToDouble("00") : Convert.ToDouble(dgProductData.Rows[i].Cells["EDP"].Value);
                        sumCGSTAmount += Convert.ToDouble(CGSTAmount); //((dgProductData.Rows[i].Cells["EDAmount"].Value == DBNull.Value) || (dgProductData.Rows[i].Cells["EDAmount"].Value == null)) ? Convert.ToDouble("00") : Convert.ToDouble(dgProductData.Rows[i].Cells["EDAmount"].Value);

                        sumSGSTP += Convert.ToDouble(SGSTP); //((dgProductData.Rows[i].Cells["VATP"].Value == DBNull.Value) || (dgProductData.Rows[i].Cells["VATP"].Value == null)) ? Convert.ToDouble("00") : Convert.ToDouble(dgProductData.Rows[i].Cells["VATP"].Value);
                        sumSGSTA += Convert.ToDouble(SGSTA); //((dgProductData.Rows[i].Cells["VATA"].Value == DBNull.Value) || (dgProductData.Rows[i].Cells["VATA"].Value == null)) ? Convert.ToDouble("00") : Convert.ToDouble(dgProductData.Rows[i].Cells["VATA"].Value);

                        sumIGSTP += Convert.ToDouble(IGSTP);//((dgProductData.Rows[i].Cells["CSTP"].Value == DBNull.Value) || (dgProductData.Rows[i].Cells["CSTP"].Value == null)) ? Convert.ToDouble("00") : Convert.ToDouble(dgProductData.Rows[i].Cells["CSTP"].Value);
                        sumIGSTA += Convert.ToDouble(IGSTA); //((dgProductData.Rows[i].Cells["CSTA"].Value == DBNull.Value) || (dgProductData.Rows[i].Cells["CSTA"].Value == null)) ? Convert.ToDouble("00") : Convert.ToDouble(dgProductData.Rows[i].Cells["CSTA"].Value);
                        sumNet_Amount += Convert.ToDouble(InvValue); //((dgProductData.Rows[i].Cells["Net_Amount"].Value == DBNull.Value) || (dgProductData.Rows[i].Cells["Net_Amount"].Value == null)) ? Convert.ToDouble("00") : Convert.ToDouble(dgProductData.Rows[i].Cells["Net_Amount"].Value);

                    }

                    txtTotalQty.Text = SUMQty.ToString(".00");
                    txtMAV.Text = SUMMAV.ToString(".00");
                    txtTotalAmount.Text = SUMAmount.ToString(".00");
                    txtAccValue.Text = TotalAssValue.ToString(".00");

                    txtCGST_Per.Text = sumCGSTP.ToString(".00");
                    txtCGSTAmount.Text = sumCGSTAmount.ToString(".00");

                    txtSGSTp.Text = sumSGSTP.ToString(".00");
                    txtSGSTAmt.Text = sumSGSTA.ToString(".00");

                    txtIGStPer.Text = sumIGSTP.ToString("0.00");
                    txtIGSTAmount.Text = sumIGSTA.ToString("0.00");

                    txtTotalInvValue.Text = sumNet_Amount.ToString("0.00");

                    Cursor.Current = Cursors.Default;
                }
            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
            }
        }

        #endregion

        #region buttonclick

        private void SaleOrderr_Load(object sender, EventArgs e)
        {
            try
            {
                PictureBox2.Image = AppCode.GlobalAccess.comylogo;
                tabControl1.TabPages.Remove(tabPage2);
                tabControl1.TabPages.Remove(tabPage3);
                bindSaleAc();
                BindQuoto();
                bindbuyer();
                BindTaxClass();
                bindPayment();
                bindPricebasis();
                //var tc = (from c in db.ProdMasters  where c.Comp_Name == AppCode.GlobalAccess.companyName select c).Distinct().ToList();
                //DataGridViewComboBoxColumn combo = (DataGridViewComboBoxColumn)dgProductData.Columns["Product_Category"];
                //if (tc.Count > 0)
                //{
                //    combo.DataSource = tc;
                //    combo.DisplayMember = "Product_Category";
                //    combo.ValueMember = "Product_Category";
                //}
                if (SaleOrderReport.SaleOrder == "SaleOrderReport")
                {
                    Search();
                }
                else
                {
                    AutoincrementId();
                }

            }
            catch (Exception ex)
            {
            }
        }
        public void bindSaleAc()
        {
            var BindSale = (from m in db.AccountMasters  select new { m.AccCode, m.AccName }).Distinct().ToList();
            if (BindSale.Count > 0)
            {
                cmbAccountGroup.DataSource = BindSale;
                cmbAccountGroup.ValueMember = "AccCode";
                cmbAccountGroup.DisplayMember = "AccName";
                cmbAccountGroup.SelectedIndex = -1;

            }

        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtCustomerPONO.Text == "")
                {
                    MessageBox.Show("Please Enter the Customer Po No, it Should Not Be Empty");
                    txtCustomerPONO.Focus();
                    return;
                }
                else if (CmbBuyerName.SelectedIndex == -1)
                {
                    MessageBox.Show("Please Select the Customer Name, it Should Not Be Empty");
                    CmbBuyerName.Focus();
                    return;
                }
                else if (CmbStatus.Text == "")
                {
                    MessageBox.Show("Please Select the status, it Should Not Be Empty");
                    CmbStatus.Focus();
                    return;
                }
                else if (CmbTaxclass.Text == "")
                {
                    MessageBox.Show("Please Select the Tax Class, it Should Not Be Empty");
                    CmbTaxclass.Focus();
                    return;
                }
                else if (dgProductData.Rows.Count == 1)
                {
                    MessageBox.Show("Please Enter atleast one Product details");
                    dgProductData.Focus();
                    return;
                }

                if (dgProductData.Rows.Count > 1)
                {
                    for (int i = 0; i < dgProductData.Rows.Count - 1; i++)
                    {
                        if (dgProductData.Rows[i].Cells["Qty"].Value == null || dgProductData.Rows[i].Cells["Qty"].Value == DBNull.Value)
                        {
                            MessageBox.Show("Please Enter the qty or price,it should not be Empty ");
                            return;
                        }
                        if (dgProductData.Rows[i].Cells["Price"].Value == null || dgProductData.Rows[i].Cells["Price"].Value == DBNull.Value)
                        {
                            MessageBox.Show("Please Enter the qty or price,it should not be Empty ");
                            return;
                        }
                    }

                }

                if ((from u in db.SaleOrders where u.SO_Code == TxtSONO.Text && u.Company == AppCode.GlobalAccess.companyName select u).Count() > 0)
                {
                    try
                    {
                        if (AppCode.GlobalAccess.Edit == "Yes")
                        {
                            if (null != db.Connection)
                            {
                                db.Connection.Close();
                            }
                            System.Data.Common.DbTransaction transaction;
                            db.Connection.Open();
                            transaction = db.Connection.BeginTransaction();
                            db.Transaction = transaction;
                            db.sp_select_Delete_SaleOrder(TxtSONO.Text, AppCode.GlobalAccess.companyName, 2);
                            db.Transaction = transaction;
                            SaleOrder so = new SaleOrder();
                            so.SO_Code = TxtSONO.Text;
                            so.SODate = dpSODate.Value;
                            so.QuotNo = CmbQuotNO.Text;
                            so.QuotDate = dpQuotDate.Value;
                            so.BuyerName = CmbBuyerName.SelectedValue.ToString();
                            so.ConsigneeName = (CmbConsigneeName.Text == "") ? "" : (CmbConsigneeName.SelectedValue.ToString());
                            so.CustomerPONo = txtCustomerPONO.Text;
                            so.PODate = dpPODate.Value;
                            so.TaxClass = CmbTaxclass.Text;
                            so.SaleRegion = txtSaleRegion.Text;
                            so.SaleExecutive = txtSaleExecutive.Text;
                            so.Status = CmbStatus.Text;
                            so.PackingForwarding = (cmbPackingForwaring.Text == "") ? "" : cmbPackingForwaring.Text.ToString();
                            so.PaymentTerms = CmbPaymentTerms.Text.ToString();
                            so.Company = AppCode.GlobalAccess.companyName;


                            so.Modified_By = AppCode.GlobalAccess.UserName;
                            so.Modified_On = DateTime.Now.ToString();
                            so.TotalQty = Convert.ToDecimal(txtTotalQty.Text);
                            so.TotalAmount = Convert.ToDecimal(txtTotalAmount.Text);
                            so.TotalMAV = Convert.ToDecimal(txtMAV.Text);
                            so.TotalASSValue = Convert.ToDecimal(txtAccValue.Text);

                            so.CGST_Per = (txtCGST_Per.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtCGST_Per.Text);
                            so.CGST_Amnt = (txtCGSTAmount.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtCGSTAmount.Text);
                            so.SGST_Per = (txtSGSTp.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtSGSTp.Text);
                            so.SGST_Amnt = (txtSGSTAmt.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtSGSTAmt.Text);
                            so.IGST_Per = (txtIGStPer.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtIGStPer.Text);
                            so.IGST_Amnt = (txtIGSTAmount.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtIGSTAmount.Text);


                            so.SubTotal = (txtSubtotal.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtSubtotal.Text);
                            so.TotalInvValue = (txtTotalInvValue.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtTotalInvValue.Text);
                            so.PuchaseAccount = (cmbAccountGroup.Text == "") ? "" : cmbAccountGroup.Text;
                            db.SaleOrders.InsertOnSubmit(so);
                            db.SubmitChanges();
                            db.Transaction = transaction;
                            //OtherTerms
                            for (int i = 0; i < dgProductData.Rows.Count - 1; i++)
                            {
                                SaleOrder_ProductData sp = new SaleOrder_ProductData();

                                sp.SO_Code = TxtSONO.Text;
                                sp.ProductCode = (dgProductData.Rows[i].Cells["ProductCode"].Value == null) ? "" : dgProductData.Rows[i].Cells["ProductCode"].Value.ToString();
                                sp.Product_Category = (dgProductData.Rows[i].Cells["Product_Category"].Value == null) ? "" : dgProductData.Rows[i].Cells["Product_Category"].Value.ToString();
                                sp.Grade = (dgProductData.Rows[i].Cells["Grade"].Value == null) ? "" : dgProductData.Rows[i].Cells["Grade"].Value.ToString();
                                sp.Qty = (dgProductData.Rows[i].Cells["Qty"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["Qty"].Value.ToString());
                                sp.MAC_P = (dgProductData.Rows[i].Cells["MAC"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["MAC"].Value.ToString());
                                sp.Price = (dgProductData.Rows[i].Cells["Price"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["Price"].Value.ToString());
                                sp.MAV = (dgProductData.Rows[i].Cells["MAv"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["MAV"].Value.ToString());
                                sp.Amount = (dgProductData.Rows[i].Cells["Amount"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["Amount"].Value.ToString());
                                sp.AssValue = (dgProductData.Rows[i].Cells["AssValue"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["AssValue"].Value.ToString());

                                sp.CGST_Per = (dgProductData.Rows[i].Cells["CGST_Per"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["CGST_Per"].Value.ToString());
                                sp.CGST_Amnt = (dgProductData.Rows[i].Cells["CGST_Amnt"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["CGST_Amnt"].Value.ToString());

                                sp.SGST_Per = (dgProductData.Rows[i].Cells["SGST_Per"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["SGST_Per"].Value.ToString());
                                sp.SGST_Amnt = (dgProductData.Rows[i].Cells["SGST_Amnt"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["SGST_Amnt"].Value.ToString());

                                sp.IGST_Per = (dgProductData.Rows[i].Cells["IGST_Per"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["IGST_Per"].Value.ToString());
                                sp.IGST_Amnt = (dgProductData.Rows[i].Cells["IGST_Amnt"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["IGST_Amnt"].Value.ToString());

                                sp.Net_Amount = (dgProductData.Rows[i].Cells["Net_Amount"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["Net_Amount"].Value.ToString());
                                sp.Company = AppCode.GlobalAccess.companyName;
                                sp.Modified_By = AppCode.GlobalAccess.UserName;
                                sp.Modified_On = DateTime.Now.ToString();



                                db.SaleOrder_ProductDatas.InsertOnSubmit(sp);
                            }
                            db.SubmitChanges();
                            db.Transaction = transaction;
                            //OtherTerms
                            for (int i = 0; i < dgTermsandConditions.Rows.Count - 1; i++)
                            {

                                SaleOrder_OtherTerm sT = new SaleOrder_OtherTerm();

                                sT.Term = (dgTermsandConditions.Rows[i].Cells["Term"].Value == null) ? "" : dgTermsandConditions.Rows[i].Cells["Term"].Value.ToString();
                                sT.Condition = (dgTermsandConditions.Rows[i].Cells["Condition"].Value == null) ? "" : dgTermsandConditions.Rows[i].Cells["Condition"].Value.ToString();
                                sT.Company = AppCode.GlobalAccess.companyName;
                                sT.Created_By = AppCode.GlobalAccess.UserName;
                                sT.Created_On = DateTime.Now.ToString();
                                sT.Modified_By = AppCode.GlobalAccess.UserName;
                                sT.Modified_On = DateTime.Now.ToString();
                                db.SaleOrder_OtherTerms.InsertOnSubmit(sT);
                            }
                            db.SubmitChanges();
                            db.Transaction = transaction;
                            //delivery
                            for (int i = 0; i < dgDeliverySchedule.Rows.Count - 1; i++)
                            {
                                SaleOrder_DeliverySchedule sd = new SaleOrder_DeliverySchedule();
                                sd.Product_Name = (dgDeliverySchedule.Rows[i].Cells["ProductionName"].Value == null) ? "" : dgDeliverySchedule.Rows[i].Cells["ProductionName"].Value.ToString();
                                sd.QtyToDeliver = (dgDeliverySchedule.Rows[i].Cells["QtyToDeliver"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgDeliverySchedule.Rows[i].Cells["QtyToDeliver"].Value.ToString());
                                sd.SO_Code = TxtSONO.Text;
                                sd.Modified_By = AppCode.GlobalAccess.UserName;
                                sd.Modified_On = DateTime.Now.ToString();
                                sd.Created_By = AppCode.GlobalAccess.UserName;
                                sd.Created_On = DateTime.Now.ToString();
                                db.SaleOrder_DeliverySchedules.InsertOnSubmit(sd);
                            }
                            db.SubmitChanges();
                            transaction.Commit();
                            MessageBox.Show("Record Updated Successfully");
                            clear();
                        }
                        else
                        {
                            MessageBox.Show("You dont Have Privileges", "Sales Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show(ex.Message);
                    }
                    finally
                    {
                        if (null != db.Connection)
                        {
                            db.Connection.Close();
                        }
                    }
                }
                else
                {
                    try
                    {
                        db.Transaction = null;
                        if (AppCode.GlobalAccess.Add == "Yes")
                        {
                            if (null != db.Connection)
                            {
                                db.Connection.Close();
                            }
                            System.Data.Common.DbTransaction transaction;
                            db.Connection.Open();
                            transaction = db.Connection.BeginTransaction();
                            db.Transaction = transaction;
                            SaleOrder so = new SaleOrder();
                            so.SO_Code = TxtSONO.Text;
                            so.SODate = dpSODate.Value;
                            so.QuotNo = CmbQuotNO.Text;
                            so.QuotDate = dpQuotDate.Value;
                            so.BuyerName = CmbBuyerName.SelectedValue.ToString();
                            so.ConsigneeName = (CmbConsigneeName.Text == "") ? "" : (CmbConsigneeName.SelectedValue.ToString());
                            so.CustomerPONo = txtCustomerPONO.Text;
                            so.PODate = dpPODate.Value;
                            so.TaxClass = CmbTaxclass.Text;
                            so.SaleRegion = txtSaleRegion.Text;
                            so.SaleExecutive = txtSaleExecutive.Text;
                            so.Status = CmbStatus.Text;
                            so.PackingForwarding = (cmbPackingForwaring.Text == "") ? "" : cmbPackingForwaring.Text.ToString();
                            so.PaymentTerms = CmbPaymentTerms.Text.ToString();
                            so.Company = AppCode.GlobalAccess.companyName;
                            so.Created_By = AppCode.GlobalAccess.UserName;
                            so.Created_On = DateTime.Now.ToString();

                            so.TotalQty = Convert.ToDecimal(txtTotalQty.Text);
                            so.TotalAmount = Convert.ToDecimal(txtTotalAmount.Text);
                            so.TotalMAV = Convert.ToDecimal(txtMAV.Text);
                            so.TotalASSValue = Convert.ToDecimal(txtAccValue.Text);

                            so.CGST_Per = (txtCGST_Per.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtCGST_Per.Text);
                            so.CGST_Amnt = (txtCGSTAmount.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtCGSTAmount.Text);
                            so.SGST_Per = (txtSGSTp.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtSGSTp.Text);
                            so.SGST_Amnt = (txtSGSTAmt.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtSGSTAmt.Text);
                            so.IGST_Per = (txtIGStPer.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtIGStPer.Text);
                            so.IGST_Amnt = (txtIGSTAmount.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtIGSTAmount.Text);

                            so.SubTotal = (txtSubtotal.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtSubtotal.Text);
                            so.TotalInvValue = (txtTotalInvValue.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtTotalInvValue.Text);
                            so.PuchaseAccount = (cmbAccountGroup.Text == "") ? "" : cmbAccountGroup.Text;
                            db.SaleOrders.InsertOnSubmit(so);
                            db.SubmitChanges();
                            db.Transaction = transaction;
                            //OtherTerms
                            for (int i = 0; i < dgProductData.Rows.Count - 1; i++)
                            {
                                SaleOrder_ProductData sp = new SaleOrder_ProductData();
                                sp.SO_Code = TxtSONO.Text;
                                sp.ProductCode = (dgProductData.Rows[i].Cells["ProductCode"].Value == null) ? "" : dgProductData.Rows[i].Cells["ProductCode"].Value.ToString();
                                sp.Product_Category = (dgProductData.Rows[i].Cells["Product_Category"].Value == null) ? "" : dgProductData.Rows[i].Cells["Product_Category"].Value.ToString();
                                sp.Grade = (dgProductData.Rows[i].Cells["Grade"].Value == null) ? "" : dgProductData.Rows[i].Cells["Grade"].Value.ToString();
                                sp.Qty = (dgProductData.Rows[i].Cells["Qty"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["Qty"].Value.ToString());
                                sp.MAC_P = (dgProductData.Rows[i].Cells["MAC"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["MAC"].Value.ToString());
                                sp.Price = (dgProductData.Rows[i].Cells["Price"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["Price"].Value.ToString());
                                sp.MAV = (dgProductData.Rows[i].Cells["MAv"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["MAV"].Value.ToString());
                                sp.Amount = (dgProductData.Rows[i].Cells["Amount"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["Amount"].Value.ToString());
                                sp.AssValue = (dgProductData.Rows[i].Cells["AssValue"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["AssValue"].Value.ToString());

                                sp.CGST_Per = (dgProductData.Rows[i].Cells["CGST_Per"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["CGST_Per"].Value.ToString());
                                sp.CGST_Amnt = (dgProductData.Rows[i].Cells["CGST_Amnt"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["CGST_Amnt"].Value.ToString());

                                sp.SGST_Per = (dgProductData.Rows[i].Cells["SGST_Per"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["SGST_Per"].Value.ToString());
                                sp.SGST_Amnt = (dgProductData.Rows[i].Cells["SGST_Amnt"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["SGST_Amnt"].Value.ToString());

                                sp.IGST_Per = (dgProductData.Rows[i].Cells["IGST_Per"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["IGST_Per"].Value.ToString());
                                sp.IGST_Amnt = (dgProductData.Rows[i].Cells["IGST_Amnt"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["IGST_Amnt"].Value.ToString());

                                sp.Net_Amount = (dgProductData.Rows[i].Cells["Net_Amount"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgProductData.Rows[i].Cells["Net_Amount"].Value.ToString());
                                sp.Company = AppCode.GlobalAccess.companyName;
                                sp.Created_By = AppCode.GlobalAccess.UserName;
                                sp.Created_On = DateTime.Now.ToString();

                                db.SaleOrder_ProductDatas.InsertOnSubmit(sp);
                            }
                            db.SubmitChanges();
                            db.Transaction = transaction;
                            //OtherTerms
                            for (int i = 0; i < dgTermsandConditions.Rows.Count - 1; i++)
                            {

                                SaleOrder_OtherTerm sT = new SaleOrder_OtherTerm();
                                sT.Term = (dgTermsandConditions.Rows[i].Cells["Term"].Value == null) ? "" : dgTermsandConditions.Rows[i].Cells["Term"].Value.ToString();
                                sT.Condition = (dgTermsandConditions.Rows[i].Cells["Condition"].Value == null) ? "" : dgTermsandConditions.Rows[i].Cells["Condition"].Value.ToString();
                                sT.Company = AppCode.GlobalAccess.companyName;
                                sT.Created_By = AppCode.GlobalAccess.UserName;
                                sT.Created_On = DateTime.Now.ToString();
                                sT.Modified_By = AppCode.GlobalAccess.UserName;
                                sT.Modified_On = DateTime.Now.ToString();
                                db.SaleOrder_OtherTerms.InsertOnSubmit(sT);
                            }
                            db.SubmitChanges();
                            db.Transaction = transaction;
                            //delivery
                            for (int i = 0; i < dgDeliverySchedule.Rows.Count - 1; i++)
                            {
                                SaleOrder_DeliverySchedule sd = new SaleOrder_DeliverySchedule();
                                sd.Product_Name = (dgDeliverySchedule.Rows[i].Cells["ProductionName"].Value == null) ? "" : dgDeliverySchedule.Rows[i].Cells["ProductionName"].Value.ToString();
                                sd.QtyToDeliver = (dgDeliverySchedule.Rows[i].Cells["QtyToDeliver"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgDeliverySchedule.Rows[i].Cells["QtyToDeliver"].Value.ToString());
                                sd.SO_Code = TxtSONO.Text;
                                sd.Modified_By = AppCode.GlobalAccess.UserName;
                                sd.Modified_On = DateTime.Now.ToString();
                                sd.Created_By = AppCode.GlobalAccess.UserName;
                                sd.Created_On = DateTime.Now.ToString();
                                db.SaleOrder_DeliverySchedules.InsertOnSubmit(sd);
                            }
                            db.SubmitChanges();
                            transaction.Commit();
                            MessageBox.Show("Record Saved Successfully");
                            clear();
                        }
                        else
                        {
                            MessageBox.Show("You dont Have Privileges", "businessLost", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            clear();
                        }
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        MessageBox.Show(ex.Message);
                    }

                    finally
                    {
                        if (null != db.Connection)
                        {
                            db.Connection.Close();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
                FName = "Saleorder";
                saleOrderReport = TxtSONO.Text;
                PrintFormate pf = new PrintFormate();
                pf.MdiParent = this.ParentForm;
                pf.Show();
                clear();

            }
            catch (Exception ex)
            {
            }
        }

        private void btnClose_Click_1(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnDelete_Click_1(object sender, EventArgs e)
        {
            try
            {
                if ((from u in db.SaleOrders where u.SO_Code == TxtSONO.Text && u.Company == AppCode.GlobalAccess.companyName select u).Count() > 0)
                {
                    if (AppCode.GlobalAccess.Edit == "Yes")
                    {
                        DialogResult result = MessageBox.Show("Are You Sure Want to Delete this Record?", "Sale Order", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                        if (result == DialogResult.OK)
                        {
                            db.sp_select_Delete_SaleOrder(TxtSONO.Text, AppCode.GlobalAccess.companyName, 2);
                            MessageBox.Show("Record Deleted Successfully ", "Sale Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            clear();
                        }
                        else
                        {
                            Cursor.Current = Cursors.Default;
                            MessageBox.Show("Record Not Deleted While Getting Error", "Sale Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                    }
                    else
                    {
                        Cursor.Current = Cursors.Default;
                        MessageBox.Show("You dont have privileges to Record this Record", "Sale Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
                else
                {
                    Cursor.Current = Cursors.Default;
                    MessageBox.Show("This Record Not Exising ,Please Select The Existing  Record", "Sale Order", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }


            }
            catch (Exception ex)
            {
                Cursor.Current = Cursors.Default;
                // transaction.Rollback();
                MessageBox.Show(ex.Message, "Sale Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                clear();
            }
            catch (Exception)
            {
            }
        }

        private void btnAddPayments_Click(object sender, EventArgs e)
        {
            try
            {
                PaymentTermsMaster obj = new PaymentTermsMaster();
                if (obj.ShowDialog() == DialogResult.Cancel)
                {
                    bindPayment();
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                name = "Search";
                OrderManagement.SaleOrderSearch obj = new SaleOrderSearch();

                if (obj.ShowDialog() == DialogResult.OK)
                {
                    TxtSONO.Text = OrderManagement.SaleOrderSearch.So_NO_get;
                    if (OrderManagement.SaleOrderSearch.So_NO_get != "")
                    {

                        if ((from u in db.SaleOrders where u.SO_Code == TxtSONO.Text && u.Company == AppCode.GlobalAccess.companyName select u).Count() > 0)
                        {
                            //var data = (from c in db.sp_BlindQuotNoforSearch(AppCode.GlobalAccess.companyName) select c).ToList();

                            //if (data.Count() > 0)
                            //{
                            //    CmbQuotNO.DataSource = data;
                            //    CmbQuotNO.DisplayMember = "QuotNo";
                            //}

                            string tb = "";
                            var d = (from s in db.SaleOrders where s.SO_Code == TxtSONO.Text && s.Company == AppCode.GlobalAccess.companyName select s).Distinct().ToList();


                            dpSODate.Text = d[0].SODate.ToString();
                            txtCustomerPONO.Text = d[0].CustomerPONo;
                            dpPODate.Text = d[0].PODate.ToString();
                            txtSaleExecutive.Text = d[0].SaleExecutive;
                            txtSaleRegion.Text = d[0].SaleRegion;
                            CmbTaxclass.Text = d[0].TaxClass;
                            CmbStatus.Text = d[0].Status;
                            CmbQuotNO.Text = d[0].QuotNo;
                            dpQuotDate.Text = d[0].QuotDate.ToString();
                            txtTotalQty.Text = d[0].TotalQty.ToString();
                            txtTotalAmount.Text = d[0].TotalAmount.ToString();
                            txtMAV.Text = d[0].TotalMAV.ToString();
                            txtAccValue.Text = d[0].TotalASSValue.ToString();

                            txtCGST_Per.Text = d[0].CGST_Per.ToString();
                            txtCGSTAmount.Text = d[0].CGST_Amnt.ToString();

                            txtSGSTp.Text = d[0].SGST_Per.ToString();
                            txtSGSTAmt.Text = d[0].SGST_Amnt.ToString();

                            txtIGStPer.Text = d[0].IGST_Per.ToString();
                            txtIGSTAmount.Text = d[0].IGST_Amnt.ToString();

                            txtSubtotal.Text = d[0].SubTotal.ToString();
                            txtTotalInvValue.Text = d[0].TotalInvValue.ToString();
                            if (d[0].PuchaseAccount != null)
                                cmbAccountGroup.Text = d[0].PuchaseAccount;
                            txtSGSTp.Text = d[0].SGST_Per.ToString();
                            txtSGSTAmt.Text = d[0].SGST_Amnt.ToString();
                            CmbBuyerName.SelectedValue = d[0].BuyerName;
                            if (d[0].ConsigneeName != null)
                                CmbConsigneeName.SelectedValue = d[0].ConsigneeName;

                            //var f = (from s in db.Sp_SaleorederDetailSearch(AppCode.GlobalAccess.companyName, TxtSONO.Text) select s).ToString();

                            var f = (from s in db.SaleOrder_ProductDatas
                                     join p in db.ProdMasters on s.ProductCode equals p.Product_code
                                     // join EI in db.ExciseInvoice_ProductDetails on s.SO_Code equals EI.SONo
                                     where s.SO_Code == TxtSONO.Text && s.Company == AppCode.GlobalAccess.companyName
                                    //// && s.Company == p.Comp_Name && p.Comp_Name == AppCode.GlobalAccess.companyName
                                     // && s.Company == EI.CompanyName

                                     select new
                                     {
                                         s.ProductCode,
                                         p.Product_Category,
                                         p.Product_Name,
                                         s.Grade,
                                         s.Qty,

                                         s.Price,
                                         s.Amount,
                                         s.MAC_P,
                                         s.MAV,

                                         s.AssValue,

                                         s.CGST_Per,
                                         s.CGST_Amnt,


                                         s.SGST_Per,
                                         s.SGST_Amnt,

                                         s.IGST_Per,
                                         s.IGST_Amnt,

                                         s.Net_Amount,
                                         // EI.qty
                                     });
                            if (f.Count() > 0)
                            {
                                SqlCommand cmd2 = (SqlCommand)db.GetCommand(f);
                                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                                DataTable dt = new DataTable();
                                da2.Fill(dt);
                                object Qty, Price, Amount, MAC_P, MAV, AssValue,CGST_Per, CGST_Amnt,SGST_Per, SGST_Amnt,IGST_Per,IGST_Amnt,Net_Amount;
                                Qty = dt.Compute("Sum(Qty)", "True");
                                Price = dt.Compute("Sum(Price)", "True");
                                Amount = dt.Compute("Sum(Amount)", "True");
                                MAC_P = dt.Compute("Sum(MAC_P)", "True");
                                 MAV = dt.Compute("Sum(MAV)", "True");
                                AssValue = dt.Compute("Sum(AssValue)", "True");
                                CGST_Per = dt.Compute("Sum(CGST_Per)", "True");
                                CGST_Amnt = dt.Compute("Sum(CGST_Amnt)", "True");
                                SGST_Per = dt.Compute("Sum(SGST_Per)", "True");
                                SGST_Amnt = dt.Compute("Sum(SGST_Amnt)", "True");
                                IGST_Per = dt.Compute("Sum(IGST_Per)", "True");
                                IGST_Amnt = dt.Compute("Sum(IGST_Amnt)", "True");
                                Net_Amount = dt.Compute("Sum(Net_Amount)", "True");

                                dt.Rows.Add("Grand Totals", "", "", "", Qty, Price, Amount, MAC_P, MAV, AssValue, CGST_Per, CGST_Amnt, SGST_Per, SGST_Amnt, IGST_Per, IGST_Amnt, Net_Amount);
                                if (dt.Rows.Count > 0)

                                    dgProductData.DataSource = dt;
                            }
                           

                            var dm2 = (from s in db.SaleOrder_OtherTerms
                                       where s.SO_Code == TxtSONO.Text && s.Company == AppCode.GlobalAccess.companyName
                                       select new
                                       { s.Term, s.Condition });

                            SqlCommand cmd3 = (SqlCommand)db.GetCommand(dm2);
                            SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                            DataTable dt1 = new DataTable();
                            da3.Fill(dt1);
                            if (dt1.Rows.Count > 0)
                                dgTermsandConditions.DataSource = dt1;

                            var t = (from s in db.SaleOrder_OtherTerms where s.SO_Code == TxtSONO.Text && s.Company == AppCode.GlobalAccess.companyName select s).FirstOrDefault();
                            if (t != null)
                            {
                                cmbPackingForwaring.Text = t.PackingForwarding;
                                CmbPaymentTerms.Text = t.PaymentTerms;
                            }

                            var dm = (from s in db.SaleOrder_DeliverySchedules
                                      where s.SO_Code == TxtSONO.Text && s.Company == AppCode.GlobalAccess.companyName
                                      select new
                                      { s.Product_Name, s.QtyToDeliver, s.Delivery_Before });

                            SqlCommand cmd = (SqlCommand)db.GetCommand(dm);
                            SqlDataAdapter da = new SqlDataAdapter(cmd);
                            DataTable dt2 = new DataTable();
                            da.Fill(dt2);
                            if (dt2.Rows.Count >= 0)
                                dgDeliverySchedule.DataSource = dt2;
                            global -= 1;
                            CmbTaxclass.Text = d[0].TaxClass;

                            bindDeliverProductName();
                        }
                        else
                        {
                            MessageBox.Show("Record Not Existing,please Try Existing Record");
                        }
                    }

                }


            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void CmbBuyerName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (CmbBuyerName.Text != "")
                {
                    //Saleregion
                    var saleregionblind = (from m in db.AccountMasters where m.AccName == CmbBuyerName.Text select m).ToList();
                    if (saleregionblind.Count > 0)
                    {
                        txtSaleExecutive.Text = saleregionblind[0].MktgExe;
                        txtSaleRegion.Text = saleregionblind[0].SalesRegion;

                    }
                    else
                    {
                        txtSaleExecutive.Text = "";
                        txtSaleRegion.Text = "";
                    }

                }
            }
            catch (Exception ex)
            {
            }

        }

        private void dgProductData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // DataGridViewRow dgProductData.Rows[i] = dgProductData.Rows[dgProductData.CurrentRow.Index];
                if (dgProductData.Rows.Count > 1)
                {
                    if (dgProductData.Rows[dgProductData.CurrentRow.Index].Cells[dgProductData.CurrentCell.ColumnIndex].Value == "Remove")
                    {
                        if (dgProductData.Rows.Count > 0)
                        {
                            foreach (DataGridViewCell oneCell in dgProductData.SelectedCells)
                            {
                                if (oneCell.Selected)
                                    dgProductData.Rows.RemoveAt(oneCell.RowIndex);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void dgProductData_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                gridvaluetax();
                if (name == "Search")
                {
                    if (dgProductData.Rows.Count > 0)
                    {
                        DataGridViewRow R1 = dgProductData.Rows[dgProductData.CurrentRow.Index];
                        decimal Qty, invQty, pend = 0;
                        if (R1.Cells["Qty"].Value != null && R1.Cells["Qty"].Value != DBNull.Value)
                        {

                            Qty = Convert.ToDecimal(R1.Cells["Qty"].Value);
                            //invQty = Convert.ToDecimal(R1.Cells["qty2"].Value);
                            //pend = (Qty - invQty);
                            if (Convert.ToDouble(R1.Cells["Qty"].Value) < Convert.ToDouble(R1.Cells["qty2"].Value))
                            {
                                MessageBox.Show("Quantity Should not be less than Excuted Quantity");
                                R1.Cells["Qty"].Value = 0;
                                return;
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {
            }
        }

        private void dgTermsandConditions_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // DataGridViewRow R1 = dgDeliverySchedule.Rows[dgDeliverySchedule.CurrentRow.Index];
                if (dgTermsandConditions.Rows.Count > 1)
                {
                    if (dgTermsandConditions.Rows[dgTermsandConditions.CurrentRow.Index].Cells[dgTermsandConditions.CurrentCell.ColumnIndex].Value == "Remove")
                    {
                        if (dgTermsandConditions.Rows.Count > 0)
                        {
                            foreach (DataGridViewCell oneCell in dgTermsandConditions.SelectedCells)
                            {
                                if (oneCell.Selected)
                                    dgTermsandConditions.Rows.RemoveAt(oneCell.RowIndex);
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void dgTermsandConditions_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                //DataGridViewRow R1 = dgTermsandConditions.Rows[dgTermsandConditions.CurrentRow.Index];

                if (dgTermsandConditions.Rows[dgTermsandConditions.CurrentRow.Index].Cells[dgTermsandConditions.CurrentCell.ColumnIndex].Value == "Remove")
                {
                    if (dgTermsandConditions.Rows.Count > 0)
                    {
                        foreach (DataGridViewCell oneCell in dgTermsandConditions.SelectedCells)
                        {
                            if (oneCell.Selected)
                                dgTermsandConditions.Rows.RemoveAt(oneCell.RowIndex);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                //  MessageBox.Show(ex.Message);
            }
        }

        private void dgDeliverySchedule_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                // DataGridViewRow R1 = dgDeliverySchedule.Rows[dgDeliverySchedule.CurrentRow.Index];

                if (dgDeliverySchedule.Rows[dgDeliverySchedule.CurrentRow.Index].Cells[dgDeliverySchedule.CurrentCell.ColumnIndex].Value == "Remove")
                {
                    if (dgDeliverySchedule.Rows.Count > 0)
                    {
                        foreach (DataGridViewCell oneCell in dgDeliverySchedule.SelectedCells)
                        {
                            if (oneCell.Selected)
                                dgDeliverySchedule.Rows.RemoveAt(oneCell.RowIndex);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void dgDeliverySchedule_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                for (int i = 0; i < dgProductData.Rows.Count - 1; i++)
                {
                    double qty = 0;
                    double sumQty = 0;
                    string productname = "";

                    if (dgProductData.Rows[i].Cells["ProductName"].Value != null && dgProductData.Rows[i].Cells["ProductName"].Value != DBNull.Value)
                    {
                        productname = dgProductData.Rows[i].Cells["ProductName"].Value.ToString();

                        for (int j = 0; j < dgDeliverySchedule.Rows.Count - 1; j++)
                        {
                            if (dgDeliverySchedule.Rows[j].Cells["ProductionName"].Value != null && dgDeliverySchedule.Rows[j].Cells["ProductionName"].Value != DBNull.Value && dgDeliverySchedule.Rows[j].Cells["ProductionName"].Value != "")
                            {
                                if (productname == dgDeliverySchedule.Rows[j].Cells["ProductionName"].Value)
                                {
                                    if (dgDeliverySchedule.Rows[j].Cells["QtyToDeliver"].Value != null && dgDeliverySchedule.Rows[j].Cells["QtyToDeliver"].Value != DBNull.Value)
                                    {

                                        sumQty += Convert.ToDouble(dgDeliverySchedule.Rows[j].Cells["QtyToDeliver"].Value);

                                    }
                                }
                            }


                            if (dgProductData.Rows[i].Cells["ProductName"].Value != null && dgDeliverySchedule.Rows[j].Cells["ProductionName"].Value != null)
                            {
                                if (dgProductData.Rows[i].Cells["ProductName"].Value.ToString() == dgDeliverySchedule.Rows[j].Cells["ProductionName"].Value.ToString())
                                {
                                    qty = Convert.ToDouble(dgProductData.Rows[i].Cells["Qty"].Value);
                                    if (qty < sumQty)
                                    {
                                        MessageBox.Show("Qty to Deliver Should not more than SO Ord Qty");
                                        dgDeliverySchedule.Rows[j].Cells["QtyToDeliver"].Value = "";
                                        return;
                                    }
                                }
                            }
                        }
                    }

                }

            }
            catch (Exception ex)
            {

            }

        }

        private void dpSODate_Validated(object sender, EventArgs e)
        {
            try
            {
                if (global == 0)
                {
                    if (dpSODate.Value > DateTime.Now)
                    {
                        MessageBox.Show("Sale Order Date Should be less than or Equal to today Date");
                        dpSODate.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void dgProductData_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int columnIndex = dgProductData.CurrentCell.ColumnIndex;
                string columnName = dgProductData.Columns[columnIndex].HeaderText;

                e.Control.KeyPress -= new KeyPressEventHandler(Quantity_KeyPress);

                TextBox tb = e.Control as TextBox;
                if (tb != null && columnName == "Qty")
                {
                    tb.KeyPress += new KeyPressEventHandler(Quantity_KeyPress);
                }

                e.Control.KeyPress -= new KeyPressEventHandler(Quantity_KeyPress);

                TextBox tb1 = e.Control as TextBox;
                if (tb1 != null && columnName == "Price")
                {
                    tb1.KeyPress += new KeyPressEventHandler(Quantity_KeyPress);
                }
                TextBox tb3 = e.Control as TextBox;
                if (columnName == "Product Group" || columnName == "Grade" || columnName == "Product Name")
                {
                    if (tb3 != null && columnName == "Product Group")
                    {
                        tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                        tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                        addItems(DataColl);
                        tb3.AutoCompleteCustomSource = DataColl;
                    }

                    //Grade

                    if (tb3 != null && columnName == "Grade")
                    {
                        tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                        tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                        addItems(DataColl);
                        tb3.AutoCompleteCustomSource = DataColl;
                    }

                    if (tb3 != null && columnName == "Product Name")
                    {
                        tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                        tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                        addItems(DataColl);
                        tb3.AutoCompleteCustomSource = DataColl;
                    }
                }
                else
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.None;
                }
            }
            catch (Exception ex)
            {
            }

        }

        public void addItems(AutoCompleteStringCollection coll)
        {
            try
            {
                DataGridViewRow R1 = dgProductData.Rows[dgProductData.CurrentRow.Index];

                int columnIndex = dgProductData.CurrentCell.ColumnIndex;
                string columnName = dgProductData.Columns[columnIndex].HeaderText;

                if (columnName == "Product Group")
                {
                    var Prodname = (from d in db.ProdMasters  select new { d.Product_Category }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Product_Category");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Product_Category);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
                    }
                }

                if (columnName == "Product Name")
                {
                    var Pname = (from d in db.ProdMasters where  d.Product_Category == R1.Cells["Product_Category"].Value.ToString() select new { d.Product_Name }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("ProductName");
                    foreach (var item in Pname)
                    {
                        dt.Rows.Add(item.Product_Name);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
                    }
                }

                if (columnName == "Grade")
                {
                    if (R1.Cells["Product_Name"].Value != null)
                    {

                        var grade = (from d in db.Grade_Masters where  d.Product_Name == R1.Cells["Product_Name"].Value select new { d.Grade }).ToList();
                        //var grade = (from d in db.Grade_Masters where d.Company == AppCode.GlobalAccess.companyName select new { d.Grade }).ToList();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Grade");
                        foreach (var item in grade)
                        {
                            dt.Rows.Add(item.Grade);
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
            }
        }
        private void Quantity_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar)
                && e.KeyChar != '.')
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if (e.KeyChar == '.'
                && (sender as TextBox).Text.IndexOf('.') > -1)
            {
                e.Handled = true;
            }
        }
        private void Column4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != 32 && e.KeyChar != 46)
            {

                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            }
        }

        private void dpQuotDate_Validated(object sender, EventArgs e)
        {
            try
            {
                if (dpQuotDate.Value > DateTime.Now)
                {
                    MessageBox.Show("Quotation Date should be not more than Current date ");
                    dpQuotDate.Focus();
                    return;
                }

            }
            catch (Exception ex)
            {
            }
        }

        private void dpPODate_Validated(object sender, EventArgs e)
        {
            try
            {
                if (dpPODate.Value > dpSODate.Value)
                {
                    MessageBox.Show("PO Date should be not more than SO Date");
                    dpSODate.Focus();
                    return;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void btnAddTaxClass_Click(object sender, EventArgs e)
        {
            try
            {
                Administrator.TaxMaster ts = new Administrator.TaxMaster();
                if (ts.ShowDialog() == DialogResult.Cancel)
                {
                    BindTaxClass();
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void CmbQuotNO_Leave(object sender, EventArgs e)
        {
            try
            {
                if (CmbQuotNO.Text != "" && CmbQuotNO.Text != "Verbal")
                {
                    var sd = (from data in db.Quots where data.CompName == AppCode.GlobalAccess.companyName && data.QuotNo == CmbQuotNO.Text select data).ToList();
                    if (sd.Count > 0)
                    {
                        dpQuotDate.Text = sd[0].QuotDate.ToString();
                        CmbBuyerName.Text = sd[0].BuyerName;
                        CmbTaxclass.Text = sd[0].TaxClass;
                        txtSaleExecutive.Text = sd[0].SaleExe.ToString();
                        txtSaleRegion.Text = sd[0].SalesRegion.ToString();
                        cmbPriceBasis.Text = sd[0].PriceBasis.ToString();
                        CmbPaymentTerms.Text = sd[0].PaymentTerms.ToString();

                        // blindbuyer();
                        var Buyerblind = (from m in db.AccountMasters where m.CompName == AppCode.GlobalAccess.companyName && m.AccCode == sd[0].BuyerName select new { m.AccCode, m.AccName }).Distinct().ToList();
                        if (Buyerblind.Count > 0)
                        {
                            CmbBuyerName.Text = Buyerblind[0].AccName;
                            CmbBuyerName.ValueMember = "AccCode";
                            CmbBuyerName.DisplayMember = "AccName";
                        }
                        var sd1 = (from data in db.Quots
                                   join prod in db.ProdMasters on data.Product_Name equals prod.Product_Name
                                   where data.CompName == AppCode.GlobalAccess.companyName && data.QuotNo == CmbQuotNO.Text
                                   select
                                 new
                                 {
                                     data.ProductCode,
                                     prod.Product_Category,
                                     data.Product_Name,
                                     data.Unit_Sale,
                                     data.Qty,
                                     data.MAC_P,
                                     data.Price,
                                     data.MAV,
                                     data.Amount
                                 });
                        if (sd1.Count() > 0)
                        {
                            SqlCommand cmd2 = (SqlCommand)db.GetCommand(sd1);
                            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                            DataTable dt = new DataTable();
                            da2.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                dgProductData.DataSource = dt;
                            }

                        }
                        var sd2 = (from data in db.Quotation_OtherTerms
                                   where data.Company == AppCode.GlobalAccess.companyName && data.QuotNo == CmbQuotNO.Text
                                   select
                                 new { data.Term, data.Condition });
                        if (sd2.Count() > 0)
                        {
                            SqlCommand cmd2 = (SqlCommand)db.GetCommand(sd2);
                            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                            DataTable dt = new DataTable();
                            da2.Fill(dt);
                            if (dt.Rows.Count > 0)
                            {
                                dgTermsandConditions.DataSource = dt;
                            }

                        }
                        gridvaluetax();
                        bindDeliverProductName();
                        // blindbuyer();
                        var Buyerblind1 = (from m in db.AccountMasters where m.CompName == AppCode.GlobalAccess.companyName && m.AccCode == sd[0].BuyerName select new { m.AccCode, m.AccName }).Distinct().ToList();
                        if (Buyerblind.Count > 0)
                        {
                            CmbBuyerName.Text = Buyerblind1[0].AccName;
                            CmbBuyerName.ValueMember = "AccCode";
                            CmbBuyerName.DisplayMember = "AccName";

                        }

                    }
                    txtCustomerPONO.Focus();
                }

            }
            catch (Exception)
            {
            }
        }
        #endregion

        private void btnPriceBasis_Click(object sender, EventArgs e)
        {
            try
            {
                //PriceBasisMaster obj = new PriceBasisMaster();
                //if (obj.ShowDialog() == DialogResult.Cancel)
                //{
                //    bindPayment();
                //}
            }
            catch (Exception ex)
            {
            }

        }

        private void dgDeliverySchedule_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int columnIndex = dgDeliverySchedule.CurrentCell.ColumnIndex;
                string columnName = dgDeliverySchedule.Columns[columnIndex].HeaderText;
                if (columnName == "Delivery On or Before")
                {
                    lblDisplay.Visible = true;
                }
                else
                {
                    lblDisplay.Visible = false;
                }
            }
            catch (Exception)
            {
            }
        }

        private void CmbTaxclass_Leave(object sender, EventArgs e)
        {
            try
            {
                //if (!string.IsNullOrEmpty(CmbTaxclass.Text))
                //{
                //    var TaxClass = (from m in db.Prod_TaxRates
                //                    where m.TaxRate.Contains("Sales") && m.TaxRate == CmbTaxclass.Text
                //                    select m.TaxRate).Distinct().ToList();
                //    if (TaxClass.Count == 0)
                //    {

                //        MessageBox.Show("Please Select  Valid TaxClass");
                //        CmbTaxclass.Focus();
                //        return;
                //    }
                //}
                //gridvaluetax();
            }
            catch (Exception ex)
            {

            }
        }

        private void CmbBuyerName_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(CmbBuyerName.Text))
                {
                    var Acc = (from data in db.AccountMasters
                               where  data.AccName == CmbBuyerName.Text
                               select data.AccName).ToList();
                    if (Acc.Count == 0)
                    {
                        MessageBox.Show("Please Enter Valid Party Name");
                        CmbBuyerName.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {


            }
        }

        private void cmbAccountGroup_Leave(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(cmbAccountGroup.Text))
                {
                    var BindSale = (from m in db.AccountMasters
                                    where  m.AccName == cmbAccountGroup.Text
                                    select m.AccName).Distinct().ToList();
                    if (BindSale.Count == 0)
                    {
                        MessageBox.Show("Please Enter Valid Account Name");
                        cmbAccountGroup.Focus();
                        return;
                    }
                }
            }
            catch (Exception ex)
            {


            }

        }

        private void dgProductData_DataSourceChanged(object sender, EventArgs e)
        {
            //int sum = 0;
            ////t sum2 = 0;
            //for (int i = 0; i < dgProductData.Rows.Count - 1; i++)
            //{
            //    if (dgProductData["Qty", i].Value != DBNull.Value)
            //    {
            //        sum += (int)dgProductData["Qty", i].Value;
            //    }
            //    //if (dataGridView1["Credit", i].Value != DBNull.Value)
            //    //{
            //    //    sum2 += (int)dataGridView1["Credit", i].Value;
            //    //}
            //}
            //dgProductData["Qty", dgProductData.Rows.Count - 1].Value = sum;
            ////ataGridView1["Credit", dataGridView1.Rows.Count - 1].Value = sum2;
            ////    int sum = 0;

            //    for (int i = 0; i < dgProductData.Rows.Count - 1; i++)
            //    {

            //        if (dgProductData[1, i].Value != DBNull.Value)
            //            sum += (int)dgProductData[1, i].Value;
            //    }
            //    dgProductData[1, dgProductData.Rows.Count - 1].Value = sum;
            //}

        }
    }
}

    



     

