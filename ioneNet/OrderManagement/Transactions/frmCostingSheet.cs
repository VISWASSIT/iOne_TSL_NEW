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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Ione_DAL;
namespace ioneNet.OrderManagement.Transactions
{
    public partial class frmCostingSheet : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        private static string AmendNo = "";
        public static string Enq_NO, ItemCode, Item_Shape, MtrlGrade;
        private Database crDatabase;
        private Tables crTables;
        private Table crTable;
        private TableLogOnInfo crTableLogOnInfo;
        private ConnectionInfo crConnectionInfo = new ConnectionInfo();
        public frmCostingSheet()
        {
            InitializeComponent();
        }

        private void frmCostingSheet_Load(object sender, EventArgs e)
        {
            string EnqNo = OrderManagement.Transactions.ListOfEnquiries.SO_No;
            AmendNo = OrderManagement.Transactions.ListOfEnquiries.SO_Amend_No;
            txtEnqNo.Text = OrderManagement.Transactions.ListOfEnquiries.SO_No;
            txtCustomerName.Text = OrderManagement.Transactions.ListOfEnquiries.Consignee;
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");


            var da = (from obj in db.ShowEnq_Products_Costing(logIn.company, EnqNo)

                      select obj).ToList();



            //SqlCommand cmd2 = (SqlCommand)db.GetCommand(da);
            //SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
            //DataTable dtr = new DataTable();
            //da2.Fill(dtr);
            if (da.Count > 0)
            {
                dataGridView1.DataSource = da;
                foreach (DataGridViewRow Myrow in dataGridView1.Rows)
                {            //Here 2 cell is target value and 1 cell is Volume
                    if (Myrow.Cells[2].Value.ToString() == "Yes")// Or your condition 
                    {
                        Myrow.DefaultCellStyle.BackColor = Color.LightGreen;
                    }
                    else
                    {
                        Myrow.DefaultCellStyle.BackColor = Color.Orange;
                    }
                }

            }

            OrderManagement.Transactions.ListOfEnquiries.SO_No = "";
            OrderManagement.Transactions.ListOfEnquiries.SO_Amend_No = "";
        }

        private void sfDataGrid1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                Clear();
                int itemCode = Convert.ToInt32(dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells["Prod_Code"].Value.ToString());
                var da = (from obj in db.Sale_Enquiry_Childs
                          join c in db.Sale_Enquiry_Masters on obj.Enq_Master_ID equals c.Id
                          join s in db.Supplier_informations on c.BuyerName equals s.ID
                          join es in db.Sale_Enquiry_Prod_Specs on new { x1=obj.Enq_NO , x2=obj.Prod_Code} equals  new {x1=es.Enq_No,x2=es.Prod_Code }
                      where c.Enq_NO == txtEnqNo.Text && c.Enq_Amend_No == AmendNo && obj.Company_ID == logIn.company && obj.Prod_Code == itemCode
                      select new
                      { obj.Prod_Code, obj.Drawing_No,obj.Prod_Grade,obj.Prod_Shape,obj.Qty,es.Forging_Wt,es.Finish_Wt,es.ProofMachining_Wt,s.Supplier_Name }).ToList();

                if(da.Count>0)
                {
                    txtItemCode.Text = da[0].Prod_Code.ToString();
                    txtDrawing.Text = da[0].Drawing_No.ToString();
                    txtShape.Text = da[0].Prod_Shape.ToString();
                    txtMtrlGrade.Text = da[0].Prod_Grade.ToString();
                    txtQty.Text = da[0].Qty.ToString();
                    txtForgingWt.Text =  da[0].Forging_Wt.ToString();
                    txtwtperPc.Text = da[0].ProofMachining_Wt.ToString();
                    txtCustomerName.Text = da[0].Supplier_Name.ToString();
                }
                else
                {
                    MessageBox.Show("Invalid Item Selected Or Specifications Not Entered for Selected Item");
                    txtItemCode.Text = "";
                    txtDrawing.Text = "";
                    txtShape.Text = "";
                    txtMtrlGrade.Text = "";
                    txtQty.Text = "";
                    txtForgingWt.Text = "";
                    txtwtperPc.Text = "";
                    //txtCustomerName.Text = "";
                }

                //Add Products based on Material Grade
                //Material Grades
                var Mgrade = (from m in db.GetProdListForCosting(logIn.company,txtMtrlGrade.Text) select new { m.prod_id, m.prod_name }).Distinct().ToList();
                if (Mgrade.Count > 0)
                {
                    cmbRMSec.DataSource = Mgrade;
                    cmbRMSec.ValueMember = "prod_id";
                    cmbRMSec.DisplayMember = "prod_name";
                    cmbRMSec.SelectedIndex = -1;
                }
                else
                {
                    cmbRMSec.Text = "";
                    cmbRMSec.DataSource = null;
                }

                //Get Costing Data if already done
                
                var CostData = (from C in db.CostingDatas where C.Enq_No == txtEnqNo.Text && C.Prod_Code == itemCode select C).ToList();
                if (CostData.Count > 0)
                {
                    GetCostingData();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void CalculateWt()
        {

            try
            {

               
                decimal ForgeWt =  (txtForgingWt.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtForgingWt.Text);
                decimal SEWeight = (txtSEWeight.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtSEWeight.Text);
                decimal SECost = (txtSECost.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtSECost.Text);
                decimal ScaleLossPer = (txtScaleLossPer.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtScaleLossPer.Text);
                
                
                txtScaleLossCost.Text = (((ForgeWt + SEWeight) * ScaleLossPer) / 100).ToString("0.00");
                decimal ScaleLossCost = (txtScaleLossCost.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtScaleLossCost.Text);
                
                txtInputCost.Text = (ForgeWt + SECost + ScaleLossCost + SEWeight).ToString("0.00");
                decimal InputCost = (txtInputCost.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtInputCost.Text);
                decimal RMCostKg = (txtRMCostperKG.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtRMCostperKG.Text);
                
                txtRMCost.Text = (InputCost * RMCostKg).ToString();

                decimal ConvCostKg = (txtConvCostPerKG.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtConvCostPerKG.Text);
                decimal HTCostKg = (txtHTCostperKG.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtHTCostperKG.Text);
               
                if (chkJW.Checked) 
                {
                   txtConvCost.Text = Math.Round((InputCost * ConvCostKg),2).ToString("0.00");                    
                }
                else
                {
                    txtConvCost.Text = Math.Round((ForgeWt * ConvCostKg),2).ToString("0.00");                    
                }
                txtHTCost.Text = Math.Round((ForgeWt * HTCostKg),2).ToString("0.00");

                decimal RMCost = (txtRMCost.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtRMCost.Text);
                decimal ConvCost = (txtConvCost.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtConvCost.Text);
                decimal HTCost = (txtHTCost.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtHTCost.Text);
                decimal CuttingCost = (txtCuttingCost.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtCuttingCost.Text);
                decimal TestingCost = (txtTestingCost.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtTestingCost.Text);
                decimal MachineCost = (txtMachineCost.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtMachineCost.Text);
                decimal DieCost = (txtDieCost.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtDieCost.Text);
                decimal PnFCost = (txtPFCharges.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtPFCharges.Text);
                txtSubtotal1.Text = Math.Round((RMCost + ConvCost + HTCost + CuttingCost + TestingCost + MachineCost + DieCost + PnFCost)).ToString("0.00");

                decimal AddlTestCost = (txtAddTestCharges.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtAddTestCharges.Text);
                decimal OtherCost = (txtOtherCharges.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtOtherCharges.Text);
                decimal ProfitPer1 = (txtP1Per.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtP1Per.Text);

                txtProfit1.Text = ((Convert.ToDecimal(txtSubtotal1.Text) + AddlTestCost + OtherCost)* ProfitPer1/100).ToString("0.00");
                decimal ProfitAmt1 = (txtProfit1.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtProfit1.Text);
                txtSubtotal2.Text = (Convert.ToDecimal(txtSubtotal1.Text) + AddlTestCost + OtherCost + ProfitAmt1).ToString("0.00");

                decimal ProfitPer2 = (txtP2Per.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtP2Per.Text);
                txtProfit2.Text = Math.Round(((Convert.ToDecimal(txtSubtotal2.Text)) * ProfitPer2 / 100)).ToString("0.00");
                decimal ProfitAmt2 = (txtProfit2.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtProfit2.Text);
                decimal Rounding = (txtRounding.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtRounding.Text);
                txtTotalCost.Text = (Convert.ToDecimal(txtSubtotal2.Text) + ProfitAmt2+ Rounding).ToString("0.00");
                decimal wtperpc = (txtwtperPc.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtwtperPc.Text);
                if (Convert.ToDecimal(txtSubtotal2.Text) > 0 && wtperpc > 0)
                {
                    txtProofMachCostPerPc.Text =Math.Round((Convert.ToDecimal(txtSubtotal2.Text) / wtperpc),2).ToString("0.00");
                }
                if (Convert.ToDecimal(txtSubtotal2.Text) > 0 && ForgeWt > 0)
                {
                    txtForgingCostPerPC.Text = Math.Round((Convert.ToDecimal(txtSubtotal2.Text) / ForgeWt),2).ToString("0.00");
                }
                int NoofPcs = (txtNoOfMultiples.Text == "") ? Convert.ToInt32("0") : Convert.ToInt32(txtNoOfMultiples.Text);
                if (NoofPcs > 0)
                {
                    txtCostPerPC.Text = (Convert.ToDecimal(txtTotalCost.Text) / NoofPcs).ToString("0.00");
                }
                else
                {
                    txtCostPerPC.Text = Convert.ToDecimal(txtTotalCost.Text).ToString("0.00");
                }



            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtCuttingCost_Leave(object sender, EventArgs e)
        {
            CalculateWt();
        }

        private void txtScaleLossPer_Leave(object sender, EventArgs e)
        {
            CalculateWt();
        }

        private void txtTestingCost_Leave(object sender, EventArgs e)
        {
            CalculateWt();
        }

        private void txtMachineCost_Leave(object sender, EventArgs e)
        {
            CalculateWt();
        }

        private void txtDieCost_Leave(object sender, EventArgs e)
        {
            CalculateWt();
        }

        private void txtPFCharges_Leave(object sender, EventArgs e)
        {
            CalculateWt();
        }

        private void txtAddTestCharges_Leave(object sender, EventArgs e)
        {
            CalculateWt();
        }

        private void txtOtherCharges_Leave(object sender, EventArgs e)
        {
            CalculateWt();
        }

        private void txtP1Per_Leave(object sender, EventArgs e)
        {
            CalculateWt();
        }

        private void txtP2Per_Leave(object sender, EventArgs e)
        {
            CalculateWt();
        }

        private void txtRounding_Leave(object sender, EventArgs e)
        {
            CalculateWt();
        }

        private void txtNoOfMultiples_Leave(object sender, EventArgs e)
        {
            CalculateWt();
        }

        private void txtRMCostperKG_Leave(object sender, EventArgs e)
        {
            CalculateWt();
        }

        private void txtConvCostPerKG_Leave(object sender, EventArgs e)
        {
            CalculateWt();
        }

        private void txtHTCostperKG_Leave(object sender, EventArgs e)
        {
            CalculateWt();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if(cmbRMSec.Text =="")
                {
                    MessageBox.Show("Selecr RM Section");
                    cmbRMSec.Focus();
                    return;
                }
                else if(Convert.ToDecimal(txtTotalCost.Text)>0 )
                {
                    if ((from u in db.CostingDatas where u.Enq_No == txtEnqNo.Text && u.Prod_Code == Convert.ToInt32(txtItemCode.Text) && u.Company_ID == logIn.company select u).Count() > 0)
                    {
                        var S = db.CostingDatas.Where(w => w.Enq_No == txtEnqNo.Text && w.Prod_Code == Convert.ToInt32(txtItemCode.Text) && w.Company_ID == logIn.company).FirstOrDefault();
                        S.Enq_No = txtEnqNo.Text;
                        S.VersionDate = dpCostingDate.Value;
                        S.VersionNo = (txtVersionNo.Text == "") ? Convert.ToInt32("1") : Convert.ToInt32(txtScaleLossPer.Text);
                        S.Prod_Code = Convert.ToInt32(txtItemCode.Text);
                        S.ScaleLossPer = (txtScaleLossPer.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtScaleLossPer.Text);
                        S.RMCostPerKg = (txtRMCostperKG.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtRMCostperKG.Text);
                        S.ConvCostPerKG = (txtConvCostPerKG.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtConvCostPerKG.Text);
                        S.RMSec = Convert.ToInt32(cmbRMSec.SelectedValue.ToString());
                        S.HTCostPerKg = (txtHTCostperKG.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtHTCostperKG.Text);
                        S.SEShape = cmbSEShape.Text;
                        S.SESize = txtSESize.Text;
                        S.SEWeight = (txtSEWeight.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtSEWeight.Text);
                        S.SELoss = (txtSECost.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtSECost.Text);
                        S.ScaleLossQty = (txtScaleLossCost.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtScaleLossCost.Text);
                        S.TotalRMQty = (txtInputCost.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtInputCost.Text);
                        S.RMCost = (txtRMCost.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtRMCost.Text);

                        S.ConvCost = (txtConvCost.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtConvCost.Text);
                        S.CuttingCost = (txtCuttingCost.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtCuttingCost.Text);
                        S.TestingCost = (txtTestingCost.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTestingCost.Text);
                        S.HTCost = (txtHTCost.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtHTCost.Text);
                        S.MachineCost = (txtMachineCost.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtMachineCost.Text);
                        S.DieCost = (txtDieCost.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtDieCost.Text);
                        S.PackingCost = (txtPFCharges.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtPFCharges.Text);
                        S.SubTotal = (txtSubtotal1.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtSubtotal1.Text);
                        S.AddlTestCharges = (txtAddTestCharges.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtAddTestCharges.Text);
                        S.AddTestRemarks = txtAddtestRemarks.Text;
                        S.ProfitPer = (txtP1Per.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtP1Per.Text);
                        S.Profit = (txtProfit1.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtProfit1.Text);
                        S.OtherCharges = (txtOtherCharges.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtOtherCharges.Text);
                        S.TotalCost = (txtSubtotal2.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtSubtotal2.Text);
                        S.ProfitPer1 = (txtP2Per.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtP2Per.Text);
                        S.Profit1 = (txtProfit2.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtProfit2.Text);
                        S.TotalCost1 = (txtTotalCost.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotalCost.Text);
                        S.FinishCostPerKg = (txtProofMachCostPerPc.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtProofMachCostPerPc.Text);
                        S.FForgingCostPerKg = (txtForgingCostPerPC.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtForgingCostPerPC.Text);
                        S.NoOfMultiples = (txtNoOfMultiples.Text == "") ? Convert.ToInt32("0") : Convert.ToInt32(txtNoOfMultiples.Text);
                        S.CostperPc = (txtCostPerPC.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtCostPerPC.Text);
                        S.DieSeleceted = cmbSelectDie.Text;
                        S.Rounding = (txtRounding.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtRounding.Text);
                        S.HTSection = txtHTSection.Text;
                        S.NoOfHeats = (txtNoOfHeats.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtNoOfHeats.Text);
                        S.QuotRegreted = chkJW.Checked;
                        S.Company_ID = logIn.company;
                        S.Created_By = lblCreatedBy.Text;
                        S.Modified_BY = logIn.username + "-" + DateTime.Now;                        
                        db.SubmitChanges();
                        MessageBox.Show("Costing Data Successfull Updated");

                    }
                    else
                    {
                        CostingData S = new CostingData();
                        {
                            S.Enq_No = txtEnqNo.Text;
                            S.VersionDate = dpCostingDate.Value;
                            S.VersionNo = (txtVersionNo.Text == "") ? Convert.ToInt32("1") : Convert.ToInt32(txtScaleLossPer.Text);
                            S.Prod_Code = Convert.ToInt32(txtItemCode.Text);
                            S.ScaleLossPer = (txtScaleLossPer.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtScaleLossPer.Text);
                            S.RMCostPerKg = (txtRMCostperKG.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtRMCostperKG.Text);
                            S.ConvCostPerKG = (txtConvCostPerKG.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtConvCostPerKG.Text);
                            S.RMSec = Convert.ToInt32(cmbRMSec.SelectedValue.ToString());
                            S.HTCostPerKg = (txtHTCost.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtHTCost.Text);
                            S.SEShape = cmbSEShape.Text;
                            S.SESize = txtSESize.Text;
                            S.SEWeight = (txtSEWeight.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtSEWeight.Text);
                            S.SELoss = (txtSECost.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtSECost.Text);
                            S.ScaleLossQty = (txtScaleLossCost.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtScaleLossCost.Text);
                            S.TotalRMQty = (txtInputCost.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtInputCost.Text);
                            S.RMCost = (txtRMCost.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtRMCost.Text);

                            S.ConvCost = (txtConvCost.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtConvCost.Text);
                            S.CuttingCost = (txtCuttingCost.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtCuttingCost.Text);
                            S.TestingCost = (txtTestingCost.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTestingCost.Text);
                            S.HTCost = (txtHTCost.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtHTCost.Text);
                            S.MachineCost = (txtMachineCost.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtMachineCost.Text);
                            S.DieCost = (txtDieCost.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtDieCost.Text);
                            S.PackingCost = (txtPFCharges.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtPFCharges.Text);
                            S.SubTotal = (txtSubtotal1.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtSubtotal1.Text);
                            S.AddlTestCharges = (txtAddTestCharges.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtAddTestCharges.Text);
                            S.ProfitPer = (txtP1Per.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtP1Per.Text);
                            S.Profit = (txtProfit1.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtProfit1.Text);
                            S.OtherCharges = (txtOtherCharges.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtOtherCharges.Text);
                            S.TotalCost = (txtSubtotal2.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtSubtotal2.Text);
                            S.ProfitPer1 = (txtP2Per.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtP2Per.Text);
                            S.Profit1 = (txtProfit2.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtProfit2.Text);
                            S.TotalCost1 = (txtTotalCost.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtTotalCost.Text);
                            S.FinishCostPerKg = (txtProofMachCostPerPc.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtProofMachCostPerPc.Text);
                            S.FForgingCostPerKg = (txtForgingCostPerPC.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtForgingCostPerPC.Text);
                            S.NoOfMultiples = (txtNoOfMultiples.Text == "") ? Convert.ToInt32("0") : Convert.ToInt32(txtNoOfMultiples.Text);
                            S.CostperPc = (txtCostPerPC.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtCostPerPC.Text);
                            S.DieSeleceted = cmbSelectDie.Text;
                            S.Rounding = (txtRounding.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtRounding.Text);
                            S.HTSection = txtHTSection.Text;
                            S.NoOfHeats = (txtNoOfHeats.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtNoOfHeats.Text);
                            S.QuotRegreted = chkJW.Checked;
                            S.Company_ID = logIn.company;
                            S.Created_By = lblCreatedBy.Text;
                            S.Modified_BY = logIn.username + "-" + DateTime.Now;
                            db.CostingDatas.InsertOnSubmit(S);
                            db.SubmitChanges();
                            MessageBox.Show("Costing Data Successfull Saved");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Costing Data Cannot Be Saved, Mandatory Fileds Not Filled");
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

        private void btnClear_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void txtSECost_Leave(object sender, EventArgs e)
        {
            CalculateWt();
        }

        private void txtHTCostperKG_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnPrint_Click(object sender, EventArgs e)
        {
            try
            {
              
                   SqlCommand cmd1 = con.CreateCommand();
                    string path = Path.Combine(Directory.GetCurrentDirectory(), "CostingSheet.pdf");
                    //string path = @"D:\Invoice.pdf";
                    FileInfo fi1 = new FileInfo(path);                
                    if (fi1.Exists)
                    {
                        fi1.Delete();
                    }
                    
                    CrystalDecisions.CrystalReports.Engine.ReportDocument rep = new CrystalDecisions.CrystalReports.Engine.ReportDocument();

                    rep = new OrderManagement.Transactions.rptCosting();
                    crConnectionInfo.ServerName = frmMain.ServerIP;
                    crConnectionInfo.DatabaseName = frmMain.Database;
                    crConnectionInfo.UserID = frmMain.DBUserID;
                    crConnectionInfo.Password = frmMain.Password;
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
                    rep.RecordSelectionFormula = "{ CostingData.Enq_No} = '" + txtEnqNo.Text + "' and { CostingData.Prod_Code} = " + txtItemCode.Text + " and { CostingData.Company_ID} = " + logIn.company + "";



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

        private void cmbRMSec_Leave(object sender, EventArgs e)
        {
            try
            {
                var da = (from obj in db.Products
                              where obj.prod_ID ==Convert.ToInt32(cmbRMSec.SelectedValue) && obj.Company_ID == logIn.company
                          select new
                          { obj.Prod_Field2 }).ToList();

                if (da.Count > 0)
                {
                    txtCuttingCost.Text = da[0].Prod_Field2;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtEnqNo_Leave(object sender, EventArgs e)
        {
            try
            {

                var ANo = (from row in db.Sale_Enquiry_Masters where row.Enq_NO == txtEnqNo.Text
                               group row by true into r
                               select new
                               {
                                   ANomax = r.Max(z => z.Enq_Amend_No)
                               }).ToList();
                if (ANo.Count > 0)
                {
                    AmendNo = ANo[0].ANomax;
                }
                else
                {
                    MessageBox.Show("Invalid Enq Number Entered");
                    return;
                }


                var da = (from obj in db.ShowEnq_Products_Costing(logIn.company, txtEnqNo.Text)

                          select obj).ToList();

                //var da = (from obj in db.Sale_Enquiry_Childs
                          
                //      join c in db.Sale_Enquiry_Masters on obj.Enq_Master_ID equals c.Id
                      
                //      where c.Enq_NO == txtEnqNo.Text && c.Enq_Amend_No == AmendNo && obj.Company_ID == logIn.company
                //      select new
                //      { obj.Prod_Code, obj.Drawing_No});

                //SqlCommand cmd2 = (SqlCommand)db.GetCommand(da);
                //SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataTable dtr = new DataTable();
                //da2.Fill(dtr);
                if (da.Count >= 0)
                {
                    Clear();
                    dataGridView1.DataSource = da;
                    foreach (DataGridViewRow Myrow in dataGridView1.Rows)
                    {            //Here 2 cell is target value and 1 cell is Volume
                        if (Myrow.Cells[2].Value.ToString() == "Yes")// Or your condition 
                        {
                            Myrow.DefaultCellStyle.BackColor = Color.LightGreen;
                        }
                        else
                        {
                            Myrow.DefaultCellStyle.BackColor = Color.Orange;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Invalid Enq Number Entered");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtSESize_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.KeyCode == Keys.F3)
                {

                    try
                    {
                       
                        txtDia.Text = "";
                        txtWidth.Text = "";
                        txtLength.Text = "";
                        //Get Density
                        var MDensity = (from m in db.MaterialGrades
                                        where m.Material_Grade == txtMtrlGrade.Text
                                        select new { m.Density, m.App_Specs }).ToList();
                        if (MDensity.Count > 0)
                        {

                            txtDensity.Text = MDensity[0].Density.ToString();                           
                            
                        }
                        switch (cmbSEShape.Text)
                        {
                            
                            case "Blank":
                            case "Round":
                                label7.Text = "Dia";
                                label9.Text = "Length / Thick";
                                txtWidth.Enabled = false;
                                if (txtSESize.Text != "")
                                {
                                    //Bind HT Condition
                                    string s = txtSESize.Text;
                                    string[] values = s.Split('X');
                                    for (int j = 0; j < values.Length; j++)
                                    {
                                        values[j] = values[j].Trim();
                                        string m = values[j].ToString();
                                        string a = m;
                                        string b = string.Empty;
                                        int val = 0;

                                        for (int i = 0; i < a.Length; i++)
                                        {
                                            if (Char.IsDigit(a[i]))
                                                b += a[i];
                                        }

                                        if (b.Length > 0)
                                            val = int.Parse(b);
                                        if (txtDia.Text != "")
                                        {
                                            txtLength.Text = val.ToString();
                                        }
                                        else
                                        {
                                            txtDia.Text = val.ToString();
                                        }

                                    }

                                }
                                groupBox6.Visible = true;
                                txtDia.Focus();
                                break;
                            case "Ring/Tube":
                                label7.Text = "OD";
                                label8.Text = "ID";
                                label9.Text = "Length / Thick";
                                txtWidth.Enabled = true;
                                if (txtSESize.Text != "")
                                {
                                    //Bind HT Condition
                                    string s = txtSESize.Text;
                                    // string S1 = CharacterCasing.Upper('X');
                                    string[] values = s.Split('X');
                                    for (int j = 0; j < values.Length; j++)
                                    {
                                        values[j] = values[j].Trim();
                                        string m = values[j].ToString();
                                        string a = m;
                                        string b = string.Empty;
                                        int val = 0;

                                        for (int i = 0; i < a.Length; i++)
                                        {
                                            if (Char.IsDigit(a[i]))
                                                b += a[i];
                                        }

                                        if (b.Length > 0)
                                            val = int.Parse(b);
                                        if (txtDia.Text != "")
                                        {
                                            if (txtWidth.Text != "")
                                            {

                                                txtLength.Text = val.ToString();
                                            }
                                            else
                                            {
                                                txtWidth.Text = val.ToString();
                                            }
                                        }
                                        else
                                        {
                                            txtDia.Text = val.ToString();
                                        }

                                    }

                                }
                                groupBox6.Visible = true;
                                txtDia.Focus();
                                break;
                            case "Flat":
                                label7.Text = "Width";
                                label8.Text = "Thick";
                                label9.Text = "Length";

                                if (txtSESize.Text != "")
                                {
                                    //Bind HT Condition
                                    string s = txtSESize.Text;
                                    string[] values = s.Split('X');
                                    for (int j = 0; j < values.Length; j++)
                                    {
                                        values[j] = values[j].Trim();
                                        string m = values[j].ToString();
                                        if (txtDia.Text != "")
                                        {
                                            if (txtWidth.Text != "")
                                            {
                                                txtLength.Text = m;
                                            }
                                            else
                                            {
                                                txtWidth.Text = m;
                                            }
                                        }
                                        else
                                        {
                                            txtDia.Text = m;
                                        }
                                    }

                                }

                                txtWidth.Enabled = true;
                                groupBox6.Visible = true;
                                txtDia.Focus();
                                break;


                        }

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    groupBox6.Visible = true;

                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }

        }

        private void sfButton1_Click(object sender, EventArgs e)
        {
            try
            {
                decimal Width = (txtWidth.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtWidth.Text);
                decimal Length = (txtLength.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtLength.Text);
                decimal dia = (txtDia.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtDia.Text);
                decimal Density = (txtDensity.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtDensity.Text);
                decimal pievalue = Convert.ToDecimal("0.000785714");
                decimal Wt = 0;
                string Size = "";
                switch (cmbSEShape.Text)
                {
                    case "Blank":
                    case "Round":

                        Wt = (dia * dia * Density * Length * pievalue) / 1000;
                        Size = "" + (char)216 + txtDia.Text + 'X' + txtLength.Text;
                        break;
                    case "Ring/Tube":
                        decimal ODWt = (dia * dia * Density * Length * pievalue) / 1000;
                        decimal IDWt = (Width * Width * Density * Length * pievalue) / 1000;

                        Wt = ODWt - IDWt;
                        Size = "" + (char)216 + txtDia.Text + 'X' + "" + (char)216 + txtWidth.Text + 'X' + txtLength.Text;
                        break;
                    case "Flat":
                    case "Square":

                        Wt = (dia * Width * Density * Length) / 1000000;
                        Size = txtDia.Text + 'X' + txtWidth.Text + 'X' + txtLength.Text;
                        break;
                }
               
               
                txtSEWeight.Text = Wt.ToString("0.000");
                txtSESize.Text = Size;
                
                //string size222 = ""+ (char)216 ; 
                txtDia.Text = "";
                txtWidth.Text = "";
                txtLength.Text = "";
                groupBox6.Visible = false;
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
                DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {

                   
                    db.Sp_delete_Costing(logIn.company,Convert.ToInt32(txtItemCode.Text),txtEnqNo.Text);                    
                    MessageBox.Show("Selected Costing Data is Deleted Successfully");
                    Clear();

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void cmdViewSpec_Click(object sender, EventArgs e)
        {
            try
            {
                    ioneNet.OrderManagement.Transactions.frmEnqProductSpecs form = new ioneNet.OrderManagement.Transactions.frmEnqProductSpecs();
                    Enq_NO = txtEnqNo.Text;                  
                    ItemCode = txtItemCode.Text;
                    Item_Shape = txtShape.Text;
                    MtrlGrade = txtMtrlGrade.Text;
                    frmMain.frmname = "Costing";
                
                    form.ShowDialog();
                    txtwtperPc.Text = frmEnqProductSpecs.finishWt;
                    txtForgingWt.Text = frmEnqProductSpecs.ForgeWt;
                    CalculateWt();
                

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }
        public void Clear()
        {

            try
            {
                foreach (Control c in groupBox3.Controls.Cast<Control>().OrderBy(c => c.TabIndex))
                {
                    if (c is TextBox)
                    {
                        c.Text = "";
                    }
                    else
                    if (c is ComboBox)
                    {
                        c.Text = "";
                    }


                }
                foreach (Control c in groupBox5.Controls.Cast<Control>().OrderBy(c => c.TabIndex))
                {
                    if (c is TextBox)
                    {
                        c.Text = "";
                    }
                    else
                    if (c is ComboBox)
                    {
                        c.Text = "";
                    }


                }
                foreach (Control c in groupBox4.Controls.Cast<Control>().OrderBy(c => c.TabIndex))
                {
                    if (c is TextBox)
                    {
                        c.Text = "";
                    }
                    else
                    if (c is ComboBox)
                    {
                        c.Text = "";
                    }


                }
                foreach (Control c in groupBox2.Controls.Cast<Control>().OrderBy(c => c.TabIndex))
                {
                    if (c is TextBox)
                    {
                        c.Text = "";
                    }
                    else
                    if (c is ComboBox)
                    {
                        c.Text = "";
                    }


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Products", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void GetCostingData()
        {
            try
            {
                var CostData = (from C in db.CostingDatas where C.Enq_No == txtEnqNo.Text && C.Prod_Code== Convert.ToInt32(txtItemCode.Text) select C).ToList();
                if (CostData.Count > 0)
                {
                    txtScaleLossPer.Text = Convert.ToString(CostData[0].ScaleLossPer);
                    txtRMCostperKG.Text = Convert.ToString(CostData[0].RMCostPerKg);
                    txtConvCostPerKG.Text = Convert.ToString(CostData[0].ConvCostPerKG);
                    if (CostData[0].RMSec != null)
                    {
                        cmbRMSec.SelectedValue = (CostData[0].RMSec);
                    }
                    txtHTCostperKG.Text = Convert.ToString(CostData[0].HTCostPerKg);

                    cmbSEShape.Text = Convert.ToString(CostData[0].SEShape);
                    txtSESize.Text = Convert.ToString(CostData[0].SESize);
                    txtSEWeight.Text = Convert.ToString(CostData[0].SEWeight);
                    txtSECost.Text = Convert.ToString(CostData[0].SELoss);
                    txtScaleLossCost.Text = Convert.ToString(CostData[0].ScaleLossQty);
                    txtInputCost.Text = Convert.ToString(CostData[0].TotalRMQty);
                    txtRMCost.Text = Convert.ToString(CostData[0].RMCost);

                    txtConvCost.Text = Convert.ToString(CostData[0].ConvCost);

                   
                    if (CostData[0].QuotRegreted != null)
                    {
                        chkJW.Checked = CostData[0].QuotRegreted.Value;
                    }

                    //cmbScrapProduct.SelectedValue = (ProductMasterList[0].Prod_Scrap_Product_Id);
                    txtCuttingCost.Text = Convert.ToString(CostData[0].CuttingCost);

                    //cmbMtrlGrade.Text = Convert.ToString(ProductMasterList[0].Prod_Field1);
                    txtTestingCost.Text = Convert.ToString(CostData[0].TestingCost);
                    txtHTCost.Text = CostData[0].HTCost.ToString();                   
                    txtMachineCost.Text = CostData[0].MachineCost.ToString();
                    txtDieCost.Text = CostData[0].DieCost.ToString();
                    txtPFCharges.Text = CostData[0].PackingCost.ToString();
                    txtSubtotal1.Text = CostData[0].SubTotal.ToString();
                    txtAddTestCharges.Text = CostData[0].AddlTestCharges.ToString();
                    if (CostData[0].AddTestRemarks != null)
                    {
                        txtAddtestRemarks.Text = CostData[0].AddTestRemarks.ToString();
                    }
                    txtP1Per.Text = CostData[0].ProfitPer.ToString();
                    txtProfit1.Text = CostData[0].Profit.ToString();
                    txtOtherCharges.Text = CostData[0].OtherCharges.ToString();
                    txtSubtotal2.Text = CostData[0].TotalCost.ToString();
                    txtP2Per.Text = CostData[0].ProfitPer1.ToString();
                    txtProfit2.Text = CostData[0].Profit1.ToString();
                    txtTotalCost.Text = CostData[0].TotalCost1.ToString();
                    txtProofMachCostPerPc.Text = CostData[0].FinishCostPerKg.ToString();
                    txtForgingCostPerPC.Text = CostData[0].FForgingCostPerKg.ToString();
                    txtNoOfMultiples.Text = CostData[0].NoOfMultiples.ToString();
                    cmbSelectDie.Text = CostData[0].DieSeleceted.ToString();
                    txtRounding.Text = CostData[0].Rounding.ToString();
                    txtHTSection.Text = CostData[0].HTSection.ToString();
                    txtNoOfHeats.Text = CostData[0].NoOfHeats.ToString();  
                    txtCostPerPC.Text   = CostData[0].CostperPc.ToString();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Products", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

        }
    }
}
