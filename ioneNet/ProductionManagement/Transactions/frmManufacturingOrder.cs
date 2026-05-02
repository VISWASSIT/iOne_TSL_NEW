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
using System.Diagnostics;
using System.IO;
using Ione_DAL;
namespace ioneNet.ProductionManagement.Transactions
{
    public partial class frmManufacturingOrder : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string Enq_NO, Item_Shape, MtrlGrade, ItemCode;
        public frmManufacturingOrder()
        {
            InitializeComponent();
        }

        private void frmManufacturingOrder_Load(object sender, EventArgs e)
        {
            dpSODate.MinDate = logIn.fy_Start_Date;
            dpSODate.MaxDate = logIn.fy_End_Date;
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            bindCustomer();
            //Status
            var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Status Trans" select new { m.ID, m.Descr }).Distinct().ToList();
            if (pStatus.Count > 0)
            {
                cmbStatus.DataSource = pStatus;
                cmbStatus.ValueMember = "ID";
                cmbStatus.DisplayMember = "Descr";
            }


            var da = (from obj in db.TestMasters
                      where obj.Company_ID == logIn.company
                      select new
                      { Characterstics = obj.TestName });

            SqlCommand cmd2 = (SqlCommand)db.GetCommand(da);
            SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            DataTable dtr = new DataTable();
            da2.Fill(dtr);
            if (dtr.Rows.Count >= 0)
                dataGridView1.DataSource = dtr;

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                row.HeaderCell.Value = row.Index + 1;
            }

            GetPendingSO();

            if (frmForge_MO_List.editMode == true)
            {
                bindedit();
            }
        }
        private void checkedListBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void bindCustomer()
        {
            try
            {
                var Buyerblind = (from m in db.Supplier_informations where m.Company_ID == logIn.company && m.Status == 1 select new { m.ID, m.Supplier_Name }).Distinct().ToList();
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
        public void GetPendingSO()
        {
            try
            {

                SqlCommand cmd2 = new SqlCommand("SP_Orders_ToRaise_MO", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);


                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataSet ds2 = new DataSet();
                DataTable ds2 = new DataTable();
                // da2.Fill(ds2, "x");
                da2.Fill(ds2);
                for (int i = 0; i < ds2.Rows.Count; i++)
                {
                    cmbQuotNo.Items.Add(ds2.Rows[i]["SO_NO"].ToString());
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
                cmbQuotNo.SelectedIndex = -1;

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
                if (cmbQuotNo.Text != "NA" && cmbQuotNo.Text != "")
                {
                    String myString = "";
                    int QuoteMasterID = 0;
                    myString = txtSoNo.Text;

                    var da = (from obj in db.Sale_Order_Masters
                              where obj.SO_NO == cmbQuotNo.Text && obj.Company_ID == logIn.company && obj.BU_ID == logIn.BU_ID
                              select obj).ToList();

                    if (da.Count > 0)
                    {

                        dpQuotDate.Text = da[0].SODate.ToString();
                        //bindCustomer();
                        CmbBuyerName.SelectedValue = da[0].BuyerName; 
                        txtCustPoNo.Text = da[0].CustomerPONo;
                        dpDeliveryDate.Text = da[0].Delivery_Date.ToString();                      
                    }                                     
                  

                    //Get Products
                    SqlCommand cmd2 = new SqlCommand("SP_GetSODetails_ForMO_Forge", con);
                    cmd2.CommandType = CommandType.StoredProcedure;
                    cmd2.Parameters.AddWithValue("@compname", logIn.company);

                    cmd2.Parameters.AddWithValue("@ordNo", cmbQuotNo.Text);
                    SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                    //DataSet ds2 = new DataSet();
                    DataTable ds2 = new DataTable();
                    // da2.Fill(ds2, "x");
                    da2.Fill(ds2);
                    dgProducts.DataSource = ds2;         
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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
                else if (txtSoNo.Text == string.Empty)
                {
                    MessageBox.Show("Enter MO No  To Proceed", "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbQuotNo.Focus();
                    return;
                }
                else if (cmbQuotNo.Text == string.Empty)
                {
                    MessageBox.Show("Select SO No  To Proceed", "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    cmbQuotNo.Focus();
                    return;
                }               
               
                else if (cmbStatus.Text == string.Empty)
                {
                    MessageBox.Show("Please Select Status");
                    cmbStatus.Focus();
                    return;
                }                          
                
                else
                if (dgProducts.Rows[0].Cells["Item_Code"].Value == null)
                {
                    MessageBox.Show("Atleast One Record To Be Entered To Proceed", "Order Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    Save();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Orders", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        public void Save()
        {
            try
            {
                String myString = "";
                myString = txtSoNo.Text;
                if ((from u in db.Forge_MFG_Order_Masters where u.MO_No == myString && u.Company_ID == logIn.company select u).Count() > 0)
                {
                    myString = txtSoNo.Text;
                    var S1 = db.Forge_MFG_Order_Masters.Where(w => w.MO_No == myString && w.Company_ID == logIn.company).FirstOrDefault();
                    S1.MO_No = myString;
                    S1.MO_Date = dpSODate.Value;
                    var d1 = (from a in db.Sale_Order_Masters where a.SO_NO == cmbQuotNo.Text && a.Company_ID == logIn.company select new { a.Id }).ToList();

                    S1.Ord_Master_ID = d1[0].Id;

                    S1.Customer_Name = Convert.ToInt32(CmbBuyerName.SelectedValue.ToString());
                    S1.Status = Convert.ToInt32(cmbStatus.SelectedValue.ToString());
                    S1.Customer_PO = (txtCustPoNo.Text == "") ? "" : txtCustPoNo.Text;
                    S1.Delivery_Date = dpDeliveryDate.Value;


                    string RMInsp = "";
                    for (int i = 0; i < chkRMInsp.Items.Count; i++)
                    {
                        if (chkRMInsp.GetItemChecked(i))
                        {
                            if (RMInsp != "")
                            {
                                RMInsp = RMInsp + "," + chkRMInsp.Items[i].ToString();
                            }
                            else
                            {
                                RMInsp = chkRMInsp.Items[i].ToString();
                            }
                        }
                    }
                    S1.RMInsp = RMInsp;
                    string FGInsp = "";
                    for (int i = 0; i < chkFinalInsp.Items.Count; i++)
                    {
                        if (chkFinalInsp.GetItemChecked(i))
                        {
                            if (FGInsp != "")
                            {
                                FGInsp = FGInsp + "," + chkFinalInsp.Items[i].ToString();
                            }
                            else
                            {
                                FGInsp = chkFinalInsp.Items[i].ToString();
                            }
                        }
                    }
                    S1.FinalInsp = FGInsp;

                    S1.ApprovedQAP = (txtApprovedQAP.Text == "") ? "" : txtApprovedQAP.Text;
                    S1.Last_Forge_Sno = (txtLastForgeSNo.Text == "") ? "" : txtLastForgeSNo.Text;

                    S1.RF_No = (txtRFNo.Text == "") ? "" : txtRFNo.Text;
                    S1.Mill_TC_No = (txtMillTC.Text == "") ? "" : txtMillTC.Text;
                    S1.Coating = (cmbCoating.Text == "") ? "" : cmbCoating.Text;
                    S1.Packing = (cmbPacking.Text == "") ? "" : cmbPacking.Text;
                    S1.Company_ID = logIn.company;

                    S1.Created_By = lblCreatedBy.Text;
                    S1.Modified_BY = logIn.username + "-" + DateTime.Now;
                    db.SubmitChanges();

                    db.sp_MO_Child_Delete(myString, logIn.company);

                    for (int i = 0; i < dgProducts.RowCount - 1; i++)
                    {
                        Forge_Mfg_Order_Child SC = new Forge_Mfg_Order_Child();
                        var d2 = (from a in db.Forge_MFG_Order_Masters where a.MO_No == myString && a.Company_ID == logIn.company select new { a.id }).ToList();
                        SC.MO_Master_ID = d2[0].id;

                        SC.Prod_Code = Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value);
                        SC.Product_Description = (dgProducts.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value).ToString();
                        SC.Prod_Grade = (dgProducts.Rows[i].Cells["Item_Grade"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value).ToString();
                        SC.Drawing_No = (dgProducts.Rows[i].Cells["Drawing_No"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Drawing_No"].Value).ToString();
                        SC.Uom = (dgProducts.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["uom"].Value.ToString();


                        SC.Qty = (dgProducts.Rows[i].Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty"].Value);
                        SC.Forging_Qty = (dgProducts.Rows[i].Cells["Forging_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Forging_Qty"].Value);
                        SC.ForgingSize = (dgProducts.Rows[i].Cells["Forging_Size"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Forging_Size"].Value).ToString();
                        SC.HTCondition = (dgProducts.Rows[i].Cells["HT_Condition"].Value == null) ? "" : (dgProducts.Rows[i].Cells["HT_Condition"].Value).ToString();
                        SC.SupplyCondition = (dgProducts.Rows[i].Cells["Supply_Condition"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Supply_Condition"].Value).ToString();
                        SC.RM_Sec = (dgProducts.Rows[i].Cells["RM_Section"].Value == null) ? "" : (dgProducts.Rows[i].Cells["RM_Section"].Value).ToString();
                        var d4 = (from a in db.Products where a.Prod_Name == dgProducts.Rows[i].Cells["RM_Section"].Value.ToString() && a.Company_ID == logIn.company select new { a.prod_ID }).ToList();
                        SC.RM_Sec_ID = d4[0].prod_ID;

                        SC.RM_WT = (dgProducts.Rows[i].Cells["RM_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["RM_Wt"].Value);
                        SC.Forging_Wt = (dgProducts.Rows[i].Cells["Forging_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Forging_Wt"].Value);
                        SC.TF_Qty = (dgProducts.Rows[i].Cells["TF_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["TF_Qty"].Value);
                        SC.TPL_Size = (dgProducts.Rows[i].Cells["TPL_Size"].Value == null) ? "" : (dgProducts.Rows[i].Cells["TPL_Size"].Value).ToString();

                        SC.TPL_Wt = (dgProducts.Rows[i].Cells["TPL_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["TPL_Wt"].Value);
                        SC.TPL_Qty = (dgProducts.Rows[i].Cells["TPL_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["TPL_Qty"].Value);
                        SC.TP_Size = (dgProducts.Rows[i].Cells["TP_Size"].Value == null) ? "" : (dgProducts.Rows[i].Cells["TP_Size"].Value).ToString();

                        SC.TP_Wt = (dgProducts.Rows[i].Cells["TP_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["TP_Wt"].Value);
                        SC.TP_Qty = (dgProducts.Rows[i].Cells["TP_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["TP_Qty"].Value);

                        SC.DD_Involved = Convert.ToBoolean(dgProducts.Rows[i].Cells["DD"].Value);
                        SC.DD_Size = (dgProducts.Rows[i].Cells["DD_Size"].Value == null) ? "" : (dgProducts.Rows[i].Cells["DD_Size"].Value).ToString();

                        SC.DD_Wt = (dgProducts.Rows[i].Cells["DD_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["DD_Wt"].Value);
                        SC.DD_Qty = (dgProducts.Rows[i].Cells["DD_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["DD_Qty"].Value);
                        SC.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value).ToString();

                        SC.Forging_Size_Prod = (dgProducts.Rows[i].Cells["Forging_Size_Prod"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Forging_Size_Prod"].Value).ToString();
                        SC.Forging_Press = (dgProducts.Rows[i].Cells["Forging_Press"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Forging_Press"].Value).ToString();
                        SC.HT_Furnace = (dgProducts.Rows[i].Cells["HT_Furnace"].Value == null) ? "" : (dgProducts.Rows[i].Cells["HT_Furnace"].Value).ToString();
                        SC.HT_Section = (dgProducts.Rows[i].Cells["HT_Section"].Value == null) ? "" : (dgProducts.Rows[i].Cells["HT_Section"].Value).ToString();
                        SC.No_Of_Heats = (dgProducts.Rows[i].Cells["No_Of_Heats"].Value == null) ? "" : (dgProducts.Rows[i].Cells["No_Of_Heats"].Value).ToString();
                        int PSno = 0;
                        string ProdSno = "";
                        if (dgProducts.Rows[i].Cells["Mo_S_No"].Value == null || dgProducts.Rows[i].Cells["Mo_S_No"].Value.ToString() == "")
                        {


                            //var d1 = (from a in db.Purchase_Order_Childs where a.PO_NO == myString && a.Company_ID == logIn.company select a.ProdSno).ToList().Max(a => a.ProdSno);
                            //  string sno = d1[0].prod;

                            if (PSno == 0)
                            {
                                ProdSno = ProdSno + Convert.ToString(i + 1);
                                PSno = i + 1;
                            }
                            else
                            {
                                ProdSno = ProdSno + Convert.ToString(PSno + 1);
                                PSno = PSno + 1;
                            }
                        }
                        else
                        {
                            ProdSno = ProdSno + Convert.ToString(dgProducts.Rows[i].Cells["Mo_S_No"].Value);
                            PSno = Convert.ToInt32(dgProducts.Rows[i].Cells["Mo_S_No"].Value);
                        }
                        SC.Mo_S_No = PSno;

                        SC.Company_ID = logIn.company;
                        db.Forge_Mfg_Order_Childs.InsertOnSubmit(SC);
                    }
                    db.SubmitChanges();

                    for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                    {
                        Forge_Mfg_Order_Spec SC = new Forge_Mfg_Order_Spec();
                        var d3 = (from a in db.Forge_MFG_Order_Masters where a.MO_No == myString && a.Company_ID == logIn.company select new { a.id }).ToList();
                        SC.MO_Master_ID = d3[0].id;

                        SC.Characterstics = (dataGridView1.Rows[i].Cells["Characterstics"].Value == null) ? "" : (dataGridView1.Rows[i].Cells["Characterstics"].Value).ToString();
                        SC.Specification = (dataGridView1.Rows[i].Cells["Spec"].Value == null) ? "" : (dataGridView1.Rows[i].Cells["Spec"].Value).ToString();

                        SC.Acceptance_Criteria = (dataGridView1.Rows[i].Cells["Acceptance_Criteria"].Value == null) ? "" : (dataGridView1.Rows[i].Cells["Acceptance_Criteria"].Value).ToString();
                        SC.NABL = Convert.ToBoolean(dataGridView1.Rows[i].Cells["NABL"].Value);
                        SC.Int_Insp = Convert.ToBoolean(dataGridView1.Rows[i].Cells["Internal"].Value);
                        SC.SEP = Convert.ToBoolean(dataGridView1.Rows[i].Cells["SEP"].Value);
                        SC.Company_ID = logIn.company;
                        db.Forge_Mfg_Order_Specs.InsertOnSubmit(SC);
                    }
                    //transaction.Commit(); 
                    db.SubmitChanges();



                }
                else
                {
                    //AutoincrementId();
                    myString = txtSoNo.Text;
                    Forge_MFG_Order_Master S = new Forge_MFG_Order_Master();
                    {
                        S.MO_No = myString;
                        S.MO_Date = dpSODate.Value;
                        var d1 = (from a in db.Sale_Order_Masters where a.SO_NO == cmbQuotNo.Text && a.Company_ID == logIn.company select new { a.Id }).ToList();

                        S.Ord_Master_ID = d1[0].Id;

                        S.Customer_Name = Convert.ToInt32(CmbBuyerName.SelectedValue.ToString());
                        S.Status = Convert.ToInt32(cmbStatus.SelectedValue.ToString());
                        S.Customer_PO = (txtCustPoNo.Text == "") ? "" : txtCustPoNo.Text;
                        S.Delivery_Date = dpDeliveryDate.Value;


                        string RMInsp = "";
                        for (int i = 0; i < chkRMInsp.Items.Count; i++)
                        {
                            if (chkRMInsp.GetItemChecked(i))
                            {
                                if (RMInsp != "")
                                {
                                    RMInsp = RMInsp + "," + chkRMInsp.Items[i].ToString();
                                }
                                else
                                {
                                    RMInsp = chkRMInsp.Items[i].ToString();
                                }
                            }
                        }
                        S.RMInsp = RMInsp;
                        string FGInsp = "";
                        for (int i = 0; i < chkFinalInsp.Items.Count; i++)
                        {
                            if (chkFinalInsp.GetItemChecked(i))
                            {
                                if (FGInsp != "")
                                {
                                    FGInsp = FGInsp + "," + chkFinalInsp.Items[i].ToString();
                                }
                                else
                                {
                                    FGInsp = chkFinalInsp.Items[i].ToString();
                                }
                            }
                        }
                        S.FinalInsp = FGInsp;

                        S.ApprovedQAP = (txtApprovedQAP.Text == "") ? "" : txtApprovedQAP.Text;
                        S.Last_Forge_Sno = (txtLastForgeSNo.Text == "") ? "" : txtLastForgeSNo.Text;

                        S.RF_No = (txtRFNo.Text == "") ? "" : txtRFNo.Text;
                        S.Mill_TC_No = (txtMillTC.Text == "") ? "" : txtMillTC.Text;
                        S.Coating = (cmbCoating.Text == "") ? "" : cmbCoating.Text;
                        S.Packing = (cmbPacking.Text == "") ? "" : cmbPacking.Text;
                        S.Company_ID = logIn.company;

                        S.Created_By = lblCreatedBy.Text;
                        S.Modified_BY = logIn.username + "-" + DateTime.Now;
                        db.Forge_MFG_Order_Masters.InsertOnSubmit(S);
                        db.SubmitChanges();

                    }

                    for (int i = 0; i < dgProducts.RowCount - 1; i++)
                    {
                        Forge_Mfg_Order_Child SC = new Forge_Mfg_Order_Child();
                        var d1 = (from a in db.Forge_MFG_Order_Masters where a.MO_No == myString && a.Company_ID == logIn.company select new { a.id }).ToList();
                        SC.MO_Master_ID = d1[0].id;

                        SC.Prod_Code = Convert.ToInt32(dgProducts.Rows[i].Cells["Item_Code"].Value);
                        SC.Product_Description = (dgProducts.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Description"].Value).ToString();
                        SC.Prod_Grade = (dgProducts.Rows[i].Cells["Item_Grade"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Item_Grade"].Value).ToString();
                        SC.Drawing_No = (dgProducts.Rows[i].Cells["Drawing_No"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Drawing_No"].Value).ToString();
                        SC.Uom = (dgProducts.Rows[i].Cells["uom"].Value == DBNull.Value) ? "" : dgProducts.Rows[i].Cells["uom"].Value.ToString();


                        SC.Qty = (dgProducts.Rows[i].Cells["Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Qty"].Value);
                        SC.Forging_Qty = (dgProducts.Rows[i].Cells["Forging_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Forging_Qty"].Value);
                        SC.ForgingSize = (dgProducts.Rows[i].Cells["Forging_Size"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Forging_Size"].Value).ToString();
                        SC.HTCondition = (dgProducts.Rows[i].Cells["HT_Condition"].Value == null) ? "" : (dgProducts.Rows[i].Cells["HT_Condition"].Value).ToString();
                        SC.SupplyCondition = (dgProducts.Rows[i].Cells["Supply_Condition"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Supply_Condition"].Value).ToString();
                        SC.RM_Sec = (dgProducts.Rows[i].Cells["RM_Section"].Value == null) ? "" : (dgProducts.Rows[i].Cells["RM_Section"].Value).ToString();
                        var d2 = (from a in db.Products where a.Prod_Name == dgProducts.Rows[i].Cells["RM_Section"].Value.ToString() && a.Company_ID == logIn.company select new { a.prod_ID }).ToList();
                        SC.RM_Sec_ID = d2[0].prod_ID;

                        SC.RM_WT = (dgProducts.Rows[i].Cells["RM_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["RM_Wt"].Value);
                        SC.Forging_Wt = (dgProducts.Rows[i].Cells["Forging_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["Forging_Wt"].Value);
                        SC.TF_Qty = (dgProducts.Rows[i].Cells["TF_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["TF_Qty"].Value);
                        SC.TPL_Size = (dgProducts.Rows[i].Cells["TPL_Size"].Value == null) ? "" : (dgProducts.Rows[i].Cells["TPL_Size"].Value).ToString();

                        SC.TPL_Wt = (dgProducts.Rows[i].Cells["TPL_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["TPL_Wt"].Value);
                        SC.TPL_Qty = (dgProducts.Rows[i].Cells["TPL_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["TPL_Qty"].Value);
                        SC.TP_Size = (dgProducts.Rows[i].Cells["TP_Size"].Value == null) ? "" : (dgProducts.Rows[i].Cells["TP_Size"].Value).ToString();

                        SC.TP_Wt = (dgProducts.Rows[i].Cells["TP_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["TP_Wt"].Value);
                        SC.TP_Qty = (dgProducts.Rows[i].Cells["TP_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["TP_Qty"].Value);

                        SC.DD_Involved = Convert.ToBoolean(dgProducts.Rows[i].Cells["DD"].Value);
                        SC.DD_Size = (dgProducts.Rows[i].Cells["DD_Size"].Value == null) ? "" : (dgProducts.Rows[i].Cells["DD_Size"].Value).ToString();

                        SC.DD_Wt = (dgProducts.Rows[i].Cells["DD_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["DD_Wt"].Value);
                        SC.DD_Qty = (dgProducts.Rows[i].Cells["DD_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgProducts.Rows[i].Cells["DD_Qty"].Value);
                        SC.Remarks = (dgProducts.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Remarks"].Value).ToString();

                        SC.Forging_Size_Prod = (dgProducts.Rows[i].Cells["Forging_Size_Prod"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Forging_Size_Prod"].Value).ToString();
                        SC.Forging_Press = (dgProducts.Rows[i].Cells["Forging_Press"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Forging_Press"].Value).ToString();
                        SC.HT_Furnace = (dgProducts.Rows[i].Cells["HT_Furnace"].Value == null) ? "" : (dgProducts.Rows[i].Cells["HT_Furnace"].Value).ToString();
                        SC.HT_Section = (dgProducts.Rows[i].Cells["HT_Section"].Value == null) ? "" : (dgProducts.Rows[i].Cells["HT_Section"].Value).ToString();
                        SC.No_Of_Heats = (dgProducts.Rows[i].Cells["No_Of_Heats"].Value == null) ? "" : (dgProducts.Rows[i].Cells["No_Of_Heats"].Value).ToString();

                        int PSno = 0;
                        string ProdSno = "";
                        if (dgProducts.Rows[i].Cells["Mo_S_No"].Value == null || dgProducts.Rows[i].Cells["Mo_S_No"].Value.ToString() == "")
                        {


                            //var d1 = (from a in db.Purchase_Order_Childs where a.PO_NO == myString && a.Company_ID == logIn.company select a.ProdSno).ToList().Max(a => a.ProdSno);
                            //  string sno = d1[0].prod;

                            if (PSno == 0)
                            {
                                ProdSno = ProdSno + Convert.ToString(i + 1);
                                PSno = i + 1;
                            }
                            else
                            {
                                ProdSno = ProdSno + Convert.ToString(PSno + 1);
                                PSno = PSno + 1;
                            }
                        }
                        else
                        {
                            ProdSno = ProdSno + Convert.ToString(dgProducts.Rows[i].Cells["Mo_S_No"].Value);
                            PSno = Convert.ToInt32(dgProducts.Rows[i].Cells["Mo_S_No"].Value);
                        }
                        SC.Mo_S_No = PSno;

                        SC.Company_ID = logIn.company;
                        db.Forge_Mfg_Order_Childs.InsertOnSubmit(SC);
                    }
                    db.SubmitChanges();

                    for (int i = 0; i < dataGridView1.RowCount - 1; i++)
                    {
                        Forge_Mfg_Order_Spec SC = new Forge_Mfg_Order_Spec();
                        var d1 = (from a in db.Forge_MFG_Order_Masters where a.MO_No == myString && a.Company_ID == logIn.company select new { a.id }).ToList();
                        SC.MO_Master_ID = d1[0].id;

                        SC.Characterstics = (dataGridView1.Rows[i].Cells["Characterstics"].Value == null) ? "" : (dataGridView1.Rows[i].Cells["Characterstics"].Value).ToString();
                        SC.Specification = (dataGridView1.Rows[i].Cells["Spec"].Value == null) ? "" : (dataGridView1.Rows[i].Cells["Spec"].Value).ToString();

                        SC.Acceptance_Criteria = (dataGridView1.Rows[i].Cells["Acceptance_Criteria"].Value == null) ? "" : (dataGridView1.Rows[i].Cells["Acceptance_Criteria"].Value).ToString();
                        SC.NABL = Convert.ToBoolean(dataGridView1.Rows[i].Cells["NABL"].Value);
                        SC.Int_Insp = Convert.ToBoolean(dataGridView1.Rows[i].Cells["Internal"].Value);
                        SC.SEP = Convert.ToBoolean(dataGridView1.Rows[i].Cells["SEP"].Value);
                        SC.Company_ID = logIn.company;
                        db.Forge_Mfg_Order_Specs.InsertOnSubmit(SC);
                    }
                    //transaction.Commit(); 
                    db.SubmitChanges();
                }
                
                //db.Transaction = transaction;
                
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
                txtSoNo.Text = ProductionManagement.Transactions.frmForge_MO_List.MO_No;                
                String myString = "";
                int QuoteMasterID = 0;
                myString = txtSoNo.Text;
                var da = (from obj in db.Forge_MFG_Order_Masters
                          where obj.MO_No == txtSoNo.Text && obj.Company_ID == logIn.company
                          select obj).ToList();

                if (da.Count > 0)
                {
                    QuoteMasterID = da[0].id;                  
                    dpSODate.Text = da[0].MO_Date.ToString();
                    //bindCustomer();
                    CmbBuyerName.SelectedValue = da[0].Customer_Name;
                    var d1 = (from a in db.Sale_Order_Masters where a.Id == da[0].Ord_Master_ID && a.Company_ID == logIn.company select new { a.SO_NO, a.SODate }).ToList();


                    cmbQuotNo.Text = d1[0].SO_NO;
                    dpQuotDate.Text = d1[0].SODate.ToString();
                    txtCustPoNo.Text = da[0].Customer_PO;
                    dpDeliveryDate.Text = da[0].Delivery_Date.ToString();              
                 

                    if (da[0].RMInsp != null)
                    {
                        string MP = da[0].RMInsp.ToString();
                        string[] values = MP.Split(',');
                        for (int j = 0; j < values.Length; j++)
                        {
                            values[j] = values[j].Trim();
                            string m = values[j].ToString();
                            for (int i = 0; i < chkRMInsp.Items.Count; i++)
                            {
                                if (chkRMInsp.Items[i].ToString() == m)
                                {
                                    chkRMInsp.SetItemChecked(i, true);
                                }
                            }
                        }
                    }

                    if (da[0].FinalInsp != null)
                    {
                        string MP = da[0].FinalInsp.ToString();
                        string[] values = MP.Split(',');
                        for (int j = 0; j < values.Length; j++)
                        {
                            values[j] = values[j].Trim();
                            string m = values[j].ToString();
                            for (int i = 0; i < chkFinalInsp.Items.Count; i++)
                            {
                                if (chkFinalInsp.Items[i].ToString() == m)
                                {
                                    chkFinalInsp.SetItemChecked(i, true);
                                }
                            }
                        }
                    }
                    txtApprovedQAP.Text = da[0].ApprovedQAP;
                    txtLastForgeSNo.Text = da[0].Last_Forge_Sno;
                    txtRFNo.Text = da[0].RF_No;
                    txtMillTC.Text = da[0].Mill_TC_No;
                    cmbCoating.Text = da[0].Coating;
                    cmbPacking.Text = da[0].Packing;
                    cmbStatus.SelectedValue = da[0].Status;
                    lblCreatedBy.Text = da[0].Created_By;
                    lblModified.Text = da[0].Modified_BY;
                }


                var dm1 = (from s in db.Forge_Mfg_Order_Childs
                           where s.MO_Master_ID == QuoteMasterID && s.Company_ID == logIn.company

                           select new

                           {
                               s.Mo_S_No,                               
                               Drawing_No = s.Drawing_No.Trim(),
                               Item_Description = s.Product_Description.Trim(),                               
                               Item_Grade = s.Prod_Grade.Trim(),
                               UOM = s.Uom.Trim(),
                               s.Qty,
                               s.Forging_Qty,
                               Forging_Size= s.ForgingSize,
                               s.Forging_Size_Prod,                               
                               Supply_Condition=s.SupplyCondition,
                               RM_Section=s.RM_Sec,
                               RM_Wt=s.RM_WT,
                               Forging_Wt= s.Forging_Wt,
                               s.Forging_Press,
                               s.No_Of_Heats,
                               HT_Condition = s.HTCondition,
                               s.HT_Section,
                               s.HT_Furnace,                                                            
                               
                               TF_Qty= s.TF_Qty,
                               s.TPL_Size
                              ,s.TPL_Wt
                              ,s.TPL_Qty
                              ,s.TP_Size
                              ,s.TP_Wt
                              ,s.TP_Qty,
                              DD=s.DD_Involved
                              ,s.DD_Size
                              ,s.DD_Wt
                              ,s.DD_Qty
                              ,s.Remarks
                              ,Item_Code = s.Prod_Code

                           });




                SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                DataTable dtr = new DataTable();
                da2.Fill(dtr);
                if (dtr.Rows.Count >= 0)
                    dgProducts.DataSource = dtr;


                var dm2 = (from s in db.Forge_Mfg_Order_Specs
                           where s.MO_Master_ID == QuoteMasterID && s.Company_ID == logIn.company

                           select new

                           {
                               Characterstics= s.Characterstics,
                               Spec= s.Specification,
                               Acceptance_Criteria= s.Acceptance_Criteria,
                               s.NABL,
                               Internal=s.Int_Insp,
                               s.SEP     
                           });




                SqlCommand cmd3 = (SqlCommand)db.GetCommand(dm2);
                SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                DataTable dtr1 = new DataTable();
                da3.Fill(dtr1);
                if (dtr1.Rows.Count >= 0)
                    dataGridView1.DataSource = dtr1;
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
                if (tb3 != null && columnName == "Forging Press")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }
                else
                {
                    if (tb3 != null && columnName == "HT Furnace")
                    {
                        tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                        tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                        addItems(DataColl);
                        tb3.AutoCompleteCustomSource = DataColl;
                    }
                    else
                     if (tb3 != null && columnName == "RM Sec")
                    {
                        tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                        tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                        AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                        addItems(DataColl);
                        tb3.AutoCompleteCustomSource = DataColl;
                    }

                    else
                    {
                        tb3.AutoCompleteMode = AutoCompleteMode.None;
                        //tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmbQuotNo_LocationChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int columnIndex = dataGridView1.CurrentCell.ColumnIndex;
                string columnName = dataGridView1.Columns[columnIndex].HeaderText;
                TextBox tb3 = e.Control as TextBox;
                tb3.AutoCompleteMode = AutoCompleteMode.None;
                if (tb3 != null && columnName == "Specification")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addQP(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }
               
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void addQP(AutoCompleteStringCollection coll)
        {
            try
            {
                DataGridViewRow R1 = dataGridView1.Rows[dataGridView1.CurrentRow.Index];

                int columnIndex = dataGridView1.CurrentCell.ColumnIndex;
                string columnName = dataGridView1.Columns[columnIndex].HeaderText;

                if (columnName == "Specification")
                {
                    var Prodname = (from d in db.Quaity_Plans select new { d.Plan_Ref_No }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Plan_Ref_No");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Plan_Ref_No);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
                    }
                }
               

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        public void addItems(AutoCompleteStringCollection coll)
        {
            try
            {
                DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];

                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string columnName = dgProducts.Columns[columnIndex].HeaderText;

                if (columnName == "Forging Press")
                {
                    var Prodname = (from d in db.Forging_MachineMasters where d.Machine_Type == "Forging Press" && d.Company_ID == logIn.company select new { d.Machine_ID }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Machine_ID");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Machine_ID);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
                    }
                }
                else
                {
                    if (columnName == "HT Furnace")
                    {
                        var Prodname = (from d in db.Forging_MachineMasters where d.Machine_Type == "HT Furnace" && d.Company_ID == logIn.company select new { d.Machine_ID }).ToList();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Machine_ID");
                        foreach (var item in Prodname)
                        {
                            dt.Rows.Add(item.Machine_ID);
                        }
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            coll.Add(dt.Rows[i][0].ToString());
                        }
                    }
                    else
                    {
                        //
                        if (columnName == "RM Sec")
                        {
                            var Prodname = (from d in db.Products where d.Company_ID == logIn.company select new { d.Prod_Name }).ToList();
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

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtSoNo_Leave(object sender, EventArgs e)
        {
            try
            {
                if (txtSoNo.Text != "")
                {

                    if (frmForge_MO_List.editMode == true)
                    {
                    }
                    else
                    {

                        if ((from u in db.Forge_MFG_Order_Masters where u.MO_No == txtSoNo.Text && u.Company_ID == logIn.company select u).Count() > 0)
                        {
                            MessageBox.Show("MO No Cannot Be Duplicate", "MO Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            txtSoNo.Focus();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "MO Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void dgProducts_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F3)
            {
                ioneNet.OrderManagement.Transactions.frmEnqProductSpecs form = new ioneNet.OrderManagement.Transactions.frmEnqProductSpecs();
                //Enq_NO = txtEnqNo.Text;
                DataGridViewRow R1 = dgProducts.Rows[dgProducts.CurrentRow.Index];
                int columnIndex = dgProducts.CurrentCell.ColumnIndex;
                string ProdCode = R1.Cells["Item_Code"].Value.ToString();
                var getproducts = (from obj in db.Sale_Order_Childs
                                   join enq in db.Sale_Enquiry_Masters on obj.enq_Master_ID equals enq.Id
                                   where obj.SO_NO == cmbQuotNo.Text && obj.Prod_Code == ProdCode
                                   select new
                                   {
                                       obj.enq_Master_ID,
                                       obj.enq_item_no,
                                       enq.Enq_NO
                                   }).ToList();

                Enq_NO = getproducts[0].Enq_NO;
                ItemCode = getproducts[0].enq_item_no.ToString(); 
                //Item_Shape = txtShape.Text;
                MtrlGrade = R1.Cells["Item_Grade"].Value.ToString(); 
                frmMain.frmname = "MO";
                form.ShowDialog();
            }
        }
    }
}
