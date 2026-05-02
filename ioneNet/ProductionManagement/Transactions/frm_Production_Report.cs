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
using Ione_DAL;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Diagnostics.Eventing.Reader;

namespace ioneNet.ProductionManagement.Transactions
{
    public partial class frm_Production_Report : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string ReportNo, ItemCode, RecQty, Suppname, var, RCode, ItemName,MtrlGrade,proddate;
        public static int iTem_Code, p_id,conv_party;
        public static float res = 0;
        public static DateTime pdate;
        public static Boolean convprod = false;


        public frm_Production_Report()
        {
            InitializeComponent();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtBurnloss_Leave(object sender, EventArgs e)
        {
            try
            {

                decimal a = (TxtrolledQty.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(TxtrolledQty.Text);
                decimal b = (txtQCrej.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtQCrej.Text);
                decimal c = (txtMissroll.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtMissroll.Text);
                decimal d = (txtBurnloss.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtBurnloss.Text);
                decimal f = (txtEndCuts.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtEndCuts.Text);
                decimal m = a - (b + c + d + f);
                txtFinishedQty.Text = m.ToString();


            }
            catch (Exception ex)
            {

            }
        }

        private void txttotal_wrk_hrs_Leave(object sender, EventArgs e)
        {
            decimal a = (TxtrolledQty.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(TxtrolledQty.Text);
            decimal c = (txttotal_wrk_hrs.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txttotal_wrk_hrs.Text);
            decimal d = (txt_breakdwnhrs.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txt_breakdwnhrs.Text);


            txt_eff_work.Text = (c - d).ToString();
            if (a > 0 && Convert.ToDecimal(txt_eff_work.Text)>0)
            {
                txt_prod_rate.Text = (a / decimal.Parse(txt_eff_work.Text)).ToString();
            }
            else
            {
                txt_prod_rate.Text = "0";
            }

        }

        private void label11_Click(object sender, EventArgs e)
        {

        }

        private void txt_eff_work_Leave(object sender, EventArgs e)
        {

        }

        private void txt_breakdwnhrs_Leave(object sender, EventArgs e)
        {
            try
            {

                decimal a = (TxtrolledQty.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(TxtrolledQty.Text);
                decimal c = (txttotal_wrk_hrs.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txttotal_wrk_hrs.Text);
                decimal d = (txt_breakdwnhrs.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txt_breakdwnhrs.Text);
                

                txt_eff_work.Text = ( c- d).ToString();
                txt_prod_rate.Text = (a / decimal.Parse(txttotal_wrk_hrs.Text)).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void textBox5_Leave(object sender, EventArgs e)
        {
            try
            {
                if (textBox5.Text != "")
                {
                    txtcoalbymt.Text = ((float.Parse(textBox5.Text) / float.Parse(txtFinishedQty.Text)) * 1000).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void textBox7_Leave(object sender, EventArgs e)
        {
            try
            {
                // txtUnitbymt.Text = ( float.Parse(textBox7.Text)/ res ).ToString();
                if (textBox7.Text != "")
                {
                    txtUnitbymt.Text = ((float.Parse(textBox7.Text) / float.Parse(txtFinishedQty.Text))).ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {

                if (chkNoProduction.Checked == true)
                {
                    if (cmbReason.SelectedIndex == -1)
                    {
                        MessageBox.Show("Reason for No Production Should be Selected");
                        cmbReason.Focus();
                        return;
                    }
                }

                if (chkConversion.Checked == true)
                {
                    if (cmbConvPartyName.SelectedIndex == -1)
                    {
                        MessageBox.Show("Conversion Party Name Should be Selected");
                        cmbConvPartyName.Focus();
                        return;
                    }
                }


                if ((from a in db.Production_report_rollings where a.Company_Id == logIn.company && a.PR_No == txtrepNoa.Text select a).Count() > 0)
                {
                    var pb1 = db.Production_report_rollings.Where(w => w.PR_No == txtrepNoa.Text && w.Company_Id == logIn.company).FirstOrDefault();

                    //db.Sp_delete_Production_rollings( txtrepNoa.Text, logIn.company, logIn.BU_ID);
                    if (chkNoProduction.Checked == true)
                    {
                        //Production_report_rolling pb1 = new Production_report_rolling();
                        pb1.No_production = chkNoProduction.Checked;
                        pb1.PR_date = Convert.ToDateTime(dpPDate.Value.ToString("yyyy-MM-dd"));
                        //pb1.PR_date = Date.Value;
                        pb1.PR_No = txtrepNoa.Text;
                        if (cmbReason.Text != "")
                        {
                            pb1.Reason_No_Production = Convert.ToInt32(cmbReason.SelectedValue);
                        }
                        else
                        {
                            pb1.Reason_No_Production = 0;
                        }
                        pb1.Power_Used = decimal.Parse(textBox7.Text);
                        pb1.Unit_by_mt = 0;
                        pb1.Company_Id = logIn.company;
                        pb1.BU_ID = logIn.BU_ID;
                        pb1.Modified_BY = logIn.username + "-" + DateTime.Now;
                        //db.Production_report_rollings.InsertOnSubmit(pb1);
                        db.SubmitChanges();
                    }
                    else
                    {
                        pb1.PR_date = Convert.ToDateTime(dpPDate.Value.ToString("yyyy-MM-dd"));
                        pb1.PR_No = txtrepNoa.Text;
                        //pb.Section_rolled = CmbsectionrollName.Text;
                        //pb.Prod_ID = Convert.ToInt32(CmbsectionrollName.SelectedValue);
                        pb1.Rolled_Qty = (TxtrolledQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(TxtrolledQty.Text);
                        pb1.Local_Qty = (txtLocalQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtLocalQty.Text);
                        pb1.QC_Rej_Qty = (txtQCrej.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtQCrej.Text);
                        pb1.Miss_Roll = (txtMissroll.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtMissroll.Text);
                        pb1.Crop_Ends = (txtEndCuts.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtEndCuts.Text);
                        pb1.Burn_Loss = (txtBurnloss.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtBurnloss.Text);
                        pb1.Finished_Qty = (txtFinishedQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtFinishedQty.Text);
                        pb1.Coal_Used = (textBox5.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(textBox5.Text);
                        pb1.Coal_by_mt = (txtcoalbymt.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtcoalbymt.Text);
                        pb1.Power_Used = (textBox7.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(textBox7.Text);
                        pb1.Unit_by_mt = (txtUnitbymt.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtUnitbymt.Text);
                        pb1.Tot_Work_hrs = (txttotal_wrk_hrs.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txttotal_wrk_hrs.Text);
                        pb1.Breakdown_hrs = (txt_breakdwnhrs.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txt_breakdwnhrs.Text);
                        pb1.Effictive_work = (txt_eff_work.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txt_eff_work.Text);
                        pb1.Production_rate_by_br = (txt_prod_rate.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txt_prod_rate.Text);
                        pb1.Conversion_Production = chkConversion.Checked;
                        if (cmbConvPartyName.Text != "")
                        {
                            pb1.Party_Name = Convert.ToInt32(cmbConvPartyName.SelectedValue);
                        }
                        else
                        {
                            pb1.Party_Name = 0;
                        }
                        pb1.No_production = chkNoProduction.Checked;
                        if (cmbReason.Text != "")
                        {
                            pb1.Reason_No_Production = Convert.ToInt32(cmbReason.SelectedValue);
                        }
                        else
                        {
                            pb1.Reason_No_Production = 0;
                        }

                        pb1.Company_Id = logIn.company;
                        pb1.BU_ID = logIn.BU_ID;
                        pb1.Created_By = lblCreatedBy.Text;
                        pb1.Modified_BY = logIn.username + "-" + DateTime.Now;
                        
                        db.SubmitChanges();
                    }
                }
                else
                {

                    AutoincrementId();
                    Production_report_rolling pb1 = new Production_report_rolling();
                    if (chkNoProduction.Checked == true)
                    {
                        
                        pb1.No_production = chkNoProduction.Checked;
                        pb1.PR_date = Convert.ToDateTime(dpPDate.Value.ToString("yyyy-MM-dd"));
                        pb1.PR_No = txtrepNoa.Text;
                        if (cmbReason.Text != "")
                        {
                            pb1.Reason_No_Production = Convert.ToInt32(cmbReason.SelectedValue);
                        }
                        else
                        {
                            pb1.Reason_No_Production = 0;
                        }
                        pb1.Power_Used = decimal.Parse(textBox7.Text);
                        pb1.Unit_by_mt = 0;
                        pb1.Company_Id = logIn.company;
                        pb1.BU_ID = logIn.BU_ID;
                        pb1.Modified_BY = logIn.username + "-" + DateTime.Now;
                        
                    }
                    else
                    {

                        pb1.PR_date = Convert.ToDateTime(dpPDate.Value.ToString("yyyy-MM-dd"));
                        pb1.PR_No = txtrepNoa.Text;
                        //pb.Section_rolled = CmbsectionrollName.Text;
                        //pb.Prod_ID = Convert.ToInt32(CmbsectionrollName.SelectedValue);
                        pb1.Rolled_Qty = (TxtrolledQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(TxtrolledQty.Text);
                        pb1.Local_Qty = (txtLocalQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtLocalQty.Text);
                        pb1.QC_Rej_Qty = (txtQCrej.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtQCrej.Text);
                        pb1.Miss_Roll = (txtMissroll.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtMissroll.Text);
                        pb1.Crop_Ends = (txtEndCuts.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtEndCuts.Text);
                        pb1.Burn_Loss = (txtBurnloss.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtBurnloss.Text);
                        pb1.Finished_Qty = (txtFinishedQty.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtFinishedQty.Text);
                        pb1.Coal_Used = (textBox5.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(textBox5.Text);
                        pb1.Coal_by_mt = (txtcoalbymt.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtcoalbymt.Text);
                        pb1.Power_Used = (textBox7.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(textBox7.Text);
                        pb1.Unit_by_mt = (txtUnitbymt.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txtUnitbymt.Text);
                        pb1.Tot_Work_hrs = (txttotal_wrk_hrs.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txttotal_wrk_hrs.Text);
                        pb1.Breakdown_hrs = (txt_breakdwnhrs.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txt_breakdwnhrs.Text);
                        pb1.Effictive_work = (txt_eff_work.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txt_eff_work.Text);
                        pb1.Production_rate_by_br = (txt_prod_rate.Text == "") ? Convert.ToDecimal("0") : Convert.ToDecimal(txt_prod_rate.Text);
                        pb1.Conversion_Production = chkConversion.Checked;
                        if (cmbConvPartyName.Text != "")
                        {
                            pb1.Party_Name = Convert.ToInt32(cmbConvPartyName.SelectedValue);
                        }
                        else
                        {
                            pb1.Party_Name = 0;
                        }
                        pb1.No_production = chkNoProduction.Checked;
                        if (cmbReason.Text != "")
                        {
                            pb1.Reason_No_Production = Convert.ToInt32(cmbReason.SelectedValue);
                        }
                        else
                        {
                            pb1.Reason_No_Production = 0;
                        }

                        pb1.Company_Id = logIn.company;
                        pb1.BU_ID = logIn.BU_ID;
                        pb1.Modified_BY = logIn.username + "-" + DateTime.Now;
                            
                    }
                    db.Production_report_rollings.InsertOnSubmit(pb1);
                    db.SubmitChanges();

                }

                db.Sp_delete_Production_rollings(txtrepNoa.Text, logIn.company, logIn.BU_ID);
                for (int i = 0; i < dgSectionProduced.RowCount - 1; i++)
                {
                    Production_Report_Rolling_Section SC = new Production_Report_Rolling_Section();
                    var d1 = (from a in db.Production_report_rollings where a.PR_No == txtrepNoa.Text && a.Company_Id == logIn.company && a.BU_ID == logIn.BU_ID select new { a.ID }).ToList();
                    SC.PR_Master_ID = d1[0].ID;
                    //SC.PR_No = txtrepNoa.Text;
                    SC.Prod_Id = Convert.ToInt32(dgSectionProduced.Rows[i].Cells["Prod_ID"].Value); ;
                    var S = (from a in db.QA_Mtrl_Grade_Masters
                                where a.Company_ID == logIn.company && a.Material_Grade == (dgSectionProduced.Rows[i].Cells["Mtrl_Grade"].Value).ToString()
                                select new { a.id }).ToList();


                    SC.Prod_Grade = S[0].id;
                    //SC.Prod_Length = dgSectionProduced.Rows[i].Cells["Prod_Length"].Value.ToString();
                    SC.Rolled_Qty = (dgSectionProduced.Rows[i].Cells["Rolled_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgSectionProduced.Rows[i].Cells["Rolled_Qty"].Value);

                    SC.Qty_Finished = (dgSectionProduced.Rows[i].Cells["Qty_Finished"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgSectionProduced.Rows[i].Cells["Qty_Finished"].Value);
                    SC.Qty_Rejected = (dgSectionProduced.Rows[i].Cells["Quality_Rjectection"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgSectionProduced.Rows[i].Cells["Quality_Rjectection"].Value);
                    SC.Qty_Local = (dgSectionProduced.Rows[i].Cells["Local_Sale"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgSectionProduced.Rows[i].Cells["Local_Sale"].Value);
                    SC.Miss_Roll_Qty = (dgSectionProduced.Rows[i].Cells["Miss_Roll_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgSectionProduced.Rows[i].Cells["Miss_Roll_Qty"].Value);
                    SC.End_Cuts_Qty = (dgSectionProduced.Rows[i].Cells["End_Cuts_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgSectionProduced.Rows[i].Cells["End_Cuts_Qty"].Value);
                    SC.Burning_Loss_Qty = (dgSectionProduced.Rows[i].Cells["Burning_Loss_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgSectionProduced.Rows[i].Cells["Burning_Loss_Qty"].Value);

                    SC.Remarks = dgSectionProduced.Rows[i].Cells["Remarks"].Value.ToString();
                    SC.FG_Lot_No = (dgSectionProduced.Rows[i].Cells["FG_Lot_No"].Value == null) ? "" : dgSectionProduced.Rows[i].Cells["FG_Lot_No"].Value.ToString();

                    SC.Company_Id = logIn.company;
                    db.Production_Report_Rolling_Sections.InsertOnSubmit(SC);
                }
                db.SubmitChanges();
                MessageBox.Show("Record Saved / Updated Sucessfully");
                clear();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void clear()
        {
            dpPDate.Value = DateTime.Now;
            txtrepNoa.Text = "";
            //CmbsectionrollName.Text = "";
           
            TxtrolledQty.Text = "";
            txtQCrej.Text = "";
            txtMissroll.Text = "";
            txtEndCuts.Text = "";
            txtBurnloss.Text = "";
            txtFinishedQty.Text = "";
            textBox5.Text = "";
            txtcoalbymt.Text = "";
            textBox7.Text = "";
            txtUnitbymt.Text = "";
            txttotal_wrk_hrs.Text = "";
            txt_breakdwnhrs.Text = "";
            txt_eff_work.Text = "";
            txt_prod_rate.Text = "";
            cmbReason.Text = "";
            chkNoProduction.Checked = false;
            //txtLOTNo.Text = "";

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
            AutoincrementId();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label14_Click(object sender, EventArgs e)
        {

        }

        private void dgrmconsumed_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
            DataGridViewRow R1 = dgrmconsumed.Rows[dgrmconsumed.CurrentRow.Index];
            int columnIndex = dgrmconsumed.CurrentCell.ColumnIndex;
            string columnName = dgrmconsumed.Columns[columnIndex].HeaderText;
            if(columnName == "Raw Material Size")
            {
                var S = (from a in db.Products
                         where a.Company_ID == logIn.company && a.Prod_Name == (R1.Cells["RM_Size"].Value).ToString()
                         select new { a.prod_ID }).ToList();

                R1.Cells["RM_Prod_ID"].Value = S[0].prod_ID.ToString();
            }
            if (columnName == "RM Issued Qty")
            {
                for (int i = 0; i < dgrmconsumed.Rows.Count - 1; i++)
                {
                    y += (dgrmconsumed.Rows[i].Cells["RM_iss_Qty"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_iss_Qty"].Value);
                    q += (dgrmconsumed.Rows[i].Cells["Rm_Cut_Qty"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["Rm_Cut_Qty"].Value);
                    v += (dgrmconsumed.Rows[i].Cells["GC_Loss"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["GC_Loss"].Value);


                }
                TxtrolledQty.Text = (y - q - v).ToString();
            }
            if(columnName == "RM_Grade")
            {
                
                var S = (from a in db.QA_Mtrl_Grade_Masters
                         where a.Company_ID == logIn.company && a.Material_Grade == R1.Cells["RM_Grade"].Value.ToString()
                         select a).ToList();
                if (S.Count > 0)
                {

                }
                else
                {
                    MessageBox.Show("Material Grade Entered is invalid");
                    R1.Cells["RM_Grade"].Value = "";
                    return;
                }
            }
        }

        private void dgrmconsumed_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int columnIndex = dgrmconsumed.CurrentCell.ColumnIndex;
                string columnName = dgrmconsumed.Columns[columnIndex].HeaderText;
                TextBox tb3 = e.Control as TextBox;
                tb3.AutoCompleteCustomSource = null;
                if (tb3 != null && columnName == "Raw Material Size")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;

                }
                if (tb3 != null && columnName == "RM_Grade")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;

                }
                if (tb3 != null && columnName == "RM_Lot_No")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;

                }

                //decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
                //for (int i = 0; i < dgrmconsumed.Rows.Count; i++)
                //{
                //    y += (dgrmconsumed.Rows[i].Cells["RM_iss_Qty"].Value.ToString() == "" || dgrmconsumed.Rows[i].Cells["RM_iss_Qty"].Value == null || dgrmconsumed.Rows[i].Cells["RM_iss_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_iss_Qty"].Value);
                //    q += (dgrmconsumed.Rows[i].Cells["Rm_Cut_Qty"].Value.ToString() == "" || dgrmconsumed.Rows[i].Cells["Rm_Cut_Qty"].Value == null || dgrmconsumed.Rows[i].Cells["Rm_Cut_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["Rm_Cut_Qty"].Value);
                //    v += (dgrmconsumed.Rows[i].Cells["GC_Loss"].Value.ToString() == "" || dgrmconsumed.Rows[i].Cells["GC_Loss"].Value == null || dgrmconsumed.Rows[i].Cells["GC_Loss"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["GC_Loss"].Value);


                //}
                //TxtrolledQty.Text = (y - q - v).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtcoalbymt_TextChanged(object sender, EventArgs e)
        {

        }

        private void CmbsectionrollName_Leave(object sender, EventArgs e)
        {
           


        }

        private void dgrmconsumed_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F6) //Remove Rows
            {
                if (dgrmconsumed.Rows.Count > 0)
                {

                    foreach (DataGridViewCell oneCell in dgrmconsumed.SelectedCells)
                    {
                        if (oneCell.Selected)
                            dgrmconsumed.Rows.RemoveAt(oneCell.RowIndex);
                    }
                    decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
                    for (int i = 0; i < dgrmconsumed.Rows.Count - 1; i++)
                    {
                        y += (dgrmconsumed.Rows[i].Cells["RM_iss_Qty"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_iss_Qty"].Value);
                        q += (dgrmconsumed.Rows[i].Cells["Rm_Cut_Qty"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["Rm_Cut_Qty"].Value);
                        v += (dgrmconsumed.Rows[i].Cells["GC_Loss"].Value == null) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["GC_Loss"].Value);


                    }
                    TxtrolledQty.Text = (y - q - v).ToString();

                }
            }
        }

        private void TxtrolledQty_Leave(object sender, EventArgs e)
        {
            decimal a = (TxtrolledQty.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(TxtrolledQty.Text);
            decimal b = (txtQCrej.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtQCrej.Text);
            decimal c = (txtMissroll.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtMissroll.Text);
            decimal d = (txtBurnloss.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtBurnloss.Text);
            decimal f = (txtEndCuts.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtEndCuts.Text);
            decimal m = a - (b + c + d + f);
            txtFinishedQty.Text = m.ToString();
        }

        private void txtMissroll_Leave(object sender, EventArgs e)
        {
            decimal a = (TxtrolledQty.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(TxtrolledQty.Text);
            decimal b = (txtQCrej.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtQCrej.Text);
            decimal c = (txtMissroll.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtMissroll.Text);
            decimal d = (txtBurnloss.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtBurnloss.Text);
            decimal f = (txtEndCuts.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtEndCuts.Text);
            decimal m = a - (b + c + d + f);
            txtFinishedQty.Text = m.ToString();
        }

        private void txtQCrej_Leave(object sender, EventArgs e)
        {
            decimal a = (TxtrolledQty.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(TxtrolledQty.Text);
            decimal b = (txtQCrej.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtQCrej.Text);
            decimal c = (txtMissroll.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtMissroll.Text);
            decimal d = (txtBurnloss.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtBurnloss.Text);
            decimal f = (txtEndCuts.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtEndCuts.Text);
            decimal m = a - (b + c + d + f);
            txtFinishedQty.Text = m.ToString();
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            decimal a = (TxtrolledQty.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(TxtrolledQty.Text);
            decimal b = (txtQCrej.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtQCrej.Text);
            decimal c = (txtMissroll.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtMissroll.Text);
            decimal d = (txtBurnloss.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtBurnloss.Text);
            decimal f = (txtEndCuts.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtEndCuts.Text);
            decimal m = a - (b + c + d + f);
            txtFinishedQty.Text = m.ToString();
        }

        private void dataGridView1_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int columnIndex = dgSectionProduced.CurrentCell.ColumnIndex;
                string columnName = dgSectionProduced.Columns[columnIndex].HeaderText;
                TextBox tb3 = e.Control as TextBox;
                tb3.AutoCompleteCustomSource = null;
                if (tb3 != null && columnName == "Section Produced")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addSections(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;

                }
                if (tb3 != null && columnName == "Mtrl_Grade")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addSections(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }

                //decimal x = 0, y = 0, q = 0, v = 0, cg = 0, sg = 0, ig = 0, totA = 0, sgp = 0, igp = 0;
                //for (int i = 0; i < dgrmconsumed.Rows.Count; i++)
                //{
                //    y += (dgrmconsumed.Rows[i].Cells["RM_iss_Qty"].Value.ToString() == "" || dgrmconsumed.Rows[i].Cells["RM_iss_Qty"].Value == null || dgrmconsumed.Rows[i].Cells["RM_iss_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["RM_iss_Qty"].Value);
                //    q += (dgrmconsumed.Rows[i].Cells["Rm_Cut_Qty"].Value.ToString() == "" || dgrmconsumed.Rows[i].Cells["Rm_Cut_Qty"].Value == null || dgrmconsumed.Rows[i].Cells["Rm_Cut_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["Rm_Cut_Qty"].Value);
                //    v += (dgrmconsumed.Rows[i].Cells["GC_Loss"].Value.ToString() == "" || dgrmconsumed.Rows[i].Cells["GC_Loss"].Value == null || dgrmconsumed.Rows[i].Cells["GC_Loss"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgrmconsumed.Rows[i].Cells["GC_Loss"].Value);


                //}
                //TxtrolledQty.Text = (y - q - v).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgrmconsumed_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgSectionProduced_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                decimal Rolled_Qty = 0, Finsihed_Qty = 0, QC_Rej_Qty = 0, EC_Qty = 0, MR_Qty = 0, BL_Qty = 0, Local_Qty = 0, totA = 0, sgp = 0, igp = 0;

            DataGridViewRow R1 = dgSectionProduced.Rows[dgSectionProduced.CurrentRow.Index];
            //DataGridViewRow R2 = dgSectionProduced.Rows[dgSectionProduced.CurrentRow.Index-1];
            int columnIndex = dgSectionProduced.CurrentCell.ColumnIndex;
            string columnName = dgSectionProduced.Columns[columnIndex].Name;
            string prodgrade = "";
            string prodname = "";
            string Prevprodgrade = "";
            string Prevprodname = "";
                if (columnName == "Section_Produced")
            {

                
               prodname = R1.Cells["Section_Produced"].Value.ToString();
               //Prevprodname = R2.Cells["Section_Produced"].Value.ToString();


                var getProductName = (from s in db.Get_ProductsList_TSL(logIn.company, 1, prodname, prodgrade)
                                        select new { s.prod_ID, s.Prod_Code, s.Uom_Descr, s.Prod_Group_Name, s.Prod_HSN_Code, s.Gst_Rate, s.Prod_Customer_Code, s.Price }).FirstOrDefault();



                if (getProductName != null)
                {

                    R1.Cells["Prod_ID"].Value = getProductName.prod_ID.ToString();

                }
            }
            if (columnName == "Mtrl_Grade")
            {
                
                if (R1.Cells["Mtrl_Grade"].Value != null || R1.Cells["Mtrl_Grade"].Value.ToString() != "")
                {
                    prodgrade = R1.Cells["Mtrl_Grade"].Value.ToString();
                    //Prevprodgrade = R2.Cells["Mtrl_Grade"].Value.ToString();
                    
                    if (prodgrade != "")
                    {
                        var S = (from a in db.QA_Mtrl_Grade_Masters
                                 where a.Company_ID == logIn.company && a.Material_Grade == prodgrade
                                 select a).ToList();
                        if (S.Count > 0)
                        {

                        }
                        else
                        {
                            MessageBox.Show("Material Grade Entered is invalid");
                            R1.Cells["Mtrl_Grade"].Value = "";
                            return;
                        }
                    }

                }
                
            }
                for (int i = 0; i < dgSectionProduced.Rows.Count - 1; i++)
                {
                    Prevprodname = dgSectionProduced.Rows[i].Cells["Section_Produced"].Value.ToString();
                    Prevprodgrade = dgSectionProduced.Rows[i].Cells["Mtrl_Grade"].Value.ToString();
                    if (prodname == Prevprodname)
                    {
                        if (prodgrade == Prevprodgrade)
                        {
                            MessageBox.Show("The product with same grade is already entered");
                            return;
                        }
                    }
                }
                DateTime t = dpPDate.Value;
               
                string f1 = t.ToString("yyyyMMdd");
                int m = dgSectionProduced.CurrentRow.Index;
                //if ( m == 0)
                //{
                //    R1.Cells["FG_Lot_No"].Value = f1;
                //}
                //else
                //{
                //    R1.Cells["FG_Lot_No"].Value = f1 + "/"+ m;
                //}
                    // Finsihed_Qty += (dgSectionProduced.Rows[i].Cells["Qty_Finished"].Value == null || dgSectionProduced.Rows[i].Cells["Qty_Finished"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgSectionProduced.Rows[i].Cells["Qty_Finished"].Value);
                QC_Rej_Qty = (R1.Cells["Quality_Rjectection"].Value == null || R1.Cells["Quality_Rjectection"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(R1.Cells["Quality_Rjectection"].Value);
                Local_Qty = (R1.Cells["Local_Sale"].Value == null || R1.Cells["Local_Sale"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(R1.Cells["Local_Sale"].Value);
                EC_Qty = (R1.Cells["End_Cuts_Qty"].Value == null || R1.Cells["End_Cuts_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(R1.Cells["End_Cuts_Qty"].Value);
                MR_Qty = (R1.Cells["Miss_Roll_Qty"].Value == null || R1.Cells["Miss_Roll_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(R1.Cells["Miss_Roll_Qty"].Value);
                BL_Qty = (R1.Cells["Burning_Loss_Qty"].Value == null || R1.Cells["Burning_Loss_Qty"].Value == DBNull.Value)  ? Convert.ToDecimal(0) : Convert.ToDecimal(R1.Cells["Burning_Loss_Qty"].Value);
                Rolled_Qty = (R1.Cells["Rolled_Qty"].Value == null || R1.Cells["Rolled_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(R1.Cells["Rolled_Qty"].Value);
                R1.Cells["Qty_Finished"].Value = Rolled_Qty - QC_Rej_Qty -  EC_Qty - MR_Qty - BL_Qty;
                QC_Rej_Qty = 0;
                Local_Qty = 0;
                EC_Qty = 0;
                MR_Qty = 0;
                BL_Qty = 0;
                Rolled_Qty = 0;


            for (int i = 0; i < dgSectionProduced.Rows.Count - 1; i++)
            {

                Finsihed_Qty += (dgSectionProduced.Rows[i].Cells["Qty_Finished"].Value == null || dgSectionProduced.Rows[i].Cells["Qty_Finished"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgSectionProduced.Rows[i].Cells["Qty_Finished"].Value);
                QC_Rej_Qty += (dgSectionProduced.Rows[i].Cells["Quality_Rjectection"].Value == null || dgSectionProduced.Rows[i].Cells["Quality_Rjectection"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgSectionProduced.Rows[i].Cells["Quality_Rjectection"].Value);
                Local_Qty += (dgSectionProduced.Rows[i].Cells["Local_Sale"].Value == null || dgSectionProduced.Rows[i].Cells["Local_Sale"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgSectionProduced.Rows[i].Cells["Local_Sale"].Value);
                EC_Qty += (dgSectionProduced.Rows[i].Cells["End_Cuts_Qty"].Value == null || dgSectionProduced.Rows[i].Cells["End_Cuts_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgSectionProduced.Rows[i].Cells["End_Cuts_Qty"].Value);
                MR_Qty += (dgSectionProduced.Rows[i].Cells["Miss_Roll_Qty"].Value == null || dgSectionProduced.Rows[i].Cells["Miss_Roll_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgSectionProduced.Rows[i].Cells["Miss_Roll_Qty"].Value);
                BL_Qty += (dgSectionProduced.Rows[i].Cells["Burning_Loss_Qty"].Value == null || dgSectionProduced.Rows[i].Cells["Burning_Loss_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgSectionProduced.Rows[i].Cells["Burning_Loss_Qty"].Value);
                Rolled_Qty += (dgSectionProduced.Rows[i].Cells["Rolled_Qty"].Value == null || dgSectionProduced.Rows[i].Cells["Rolled_Qty"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgSectionProduced.Rows[i].Cells["Rolled_Qty"].Value);

            }
            txtFinishedQty.Text = Finsihed_Qty.ToString();
            txtQCrej.Text = QC_Rej_Qty.ToString();
            txtLocalQty.Text = Local_Qty.ToString();
            TxtrolledQty.Text = Rolled_Qty.ToString();
            txtMissroll.Text = MR_Qty.ToString();
            txtEndCuts.Text = EC_Qty.ToString();
            txtBurnloss.Text = BL_Qty.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dgSectionProduced_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void tableLayoutPanel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            string pd = dpPDate.Value.ToString("yyyy-MM-dd");
            


            SqlCommand cmd3 = new SqlCommand("SP_Get_Planning_Data_To_Report", con);
            cmd3.CommandType = CommandType.StoredProcedure;
            cmd3.Parameters.AddWithValue("@compname", logIn.company);
            cmd3.Parameters.AddWithValue("@prod_Date", Convert.ToDateTime(pd));
            cmd3.Parameters.AddWithValue("@convprod", chkConversion.Checked);
            cmd3.Parameters.AddWithValue("@ProdFor",Convert.ToInt32(cmbConvPartyName.SelectedValue));

            SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
            //DataSet ds2 = new DataSet();
            DataTable ds3 = new DataTable();
            // da2.Fill(ds2, "x");
            da3.Fill(ds3);
            if (ds3.Rows.Count > 0)
            {
                dgSectionProduced.DataSource = ds3;
            }
            else
            {
                dgSectionProduced.DataSource = null;
            }

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReportNo = txtrepNoa.Text;
            ProductionManagement.Transactions.frmBreakDownData frm = new frmBreakDownData();
            frm.ShowDialog();
        }

        private void dgSectionProduced_KeyDown(object sender, KeyEventArgs e)
        {
            try 
            { 
                if (e.KeyCode == Keys.F3)
                {
                   
                    int i = dgSectionProduced.CurrentCell.RowIndex;
                    ItemCode = (dgSectionProduced.Rows[i].Cells["Prod_ID"].Value == null) ? "" : (dgSectionProduced.Rows[i].Cells["Prod_ID"].Value).ToString();
                    RecQty = (dgSectionProduced.Rows[i].Cells["Qty_Finished"].Value == null) ? "" : (dgSectionProduced.Rows[i].Cells["Qty_Finished"].Value).ToString();
                    ItemName = (dgSectionProduced.Rows[i].Cells["Section_Produced"].Value == null) ? "" : (dgSectionProduced.Rows[i].Cells["Section_Produced"].Value).ToString();
                    MtrlGrade = (dgSectionProduced.Rows[i].Cells["Mtrl_Grade"].Value == null) ? "" : (dgSectionProduced.Rows[i].Cells["Mtrl_Grade"].Value).ToString();
                    proddate = dpPDate.Value.ToString("yyyy-MM-dd");


                    if (ItemCode != "" && RecQty != "")
                        if (ItemCode != "" && RecQty != "")

                        {
                        // GlobalVariables.FormName = "Production_Repot";
                        ioneNet.ProductionManagement.Transactions.frmProdSectionWiseLengths form = new ioneNet.ProductionManagement.Transactions.frmProdSectionWiseLengths();
                        //ioneNet.Masters.ProdSearch.frmName = "SOrder";       

                        ReportNo = txtrepNoa.Text;


                        form.ShowDialog();

                        //dgSectionProduced.Rows[i].Cells["ReceivedQty"].Value = ioneNet.ProductionManagement.Transactions.frmProdSectionWiseLengths.TotQty;
                    }
                    else
                    {
                        MessageBox.Show("Cannot Proceed Without Item Code and Total Qty Prodcued");
                    }
                   
                }
                if (e.KeyCode == Keys.F4)
                {

                    int i = dgSectionProduced.CurrentCell.RowIndex;
                    ItemCode = (dgSectionProduced.Rows[i].Cells["Prod_ID"].Value == null) ? "" : (dgSectionProduced.Rows[i].Cells["Prod_ID"].Value).ToString();
                    RecQty = (dgSectionProduced.Rows[i].Cells["Rolled_Qty"].Value == null) ? "" : (dgSectionProduced.Rows[i].Cells["Rolled_Qty"].Value).ToString();
                    ItemName = (dgSectionProduced.Rows[i].Cells["Section_Produced"].Value == null) ? "" : (dgSectionProduced.Rows[i].Cells["Section_Produced"].Value).ToString();
                    MtrlGrade = (dgSectionProduced.Rows[i].Cells["Mtrl_Grade"].Value == null) ? "" : (dgSectionProduced.Rows[i].Cells["Mtrl_Grade"].Value).ToString();
                    pdate = dpPDate.Value;
                    conv_party = Convert.ToInt32(cmbConvPartyName.SelectedValue);
                    if (chkConversion.Checked == true)
                    {
                        convprod = true;
                    }
                    else
                    {
                        convprod = false;
                    }
                    if (ItemCode != "" && RecQty != "")
                        if (ItemCode != "" && RecQty != "")

                        {
                            // GlobalVariables.FormName = "Production_Repot";
                            ioneNet.ProductionManagement.Transactions.frmRMIssue_LotWise form = new ioneNet.ProductionManagement.Transactions.frmRMIssue_LotWise();
                            //ioneNet.Masters.ProdSearch.frmName = "SOrder";       

                            ReportNo = txtrepNoa.Text;


                            form.ShowDialog();

                            //dgSectionProduced.Rows[i].Cells["ReceivedQty"].Value = ioneNet.ProductionManagement.Transactions.frmProdSectionWiseLengths.TotQty;
                        }
                        else
                        {
                            MessageBox.Show("Cannot Proceed Without Item Code and Total Qty Rolled");
                        }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtCustPoNo_TextChanged(object sender, EventArgs e)
        {


        }

        private void frm_Production_Report_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'ioneDataSet.Products' table. You can move, or remove it, as needed.
            //this.productsTableAdapter.Fill(this.ioneDataSet.Products);
            
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

            var pStatus = (from m in db.Attributes_Datas where m.Head_Name == "Conversion_Party" select new { m.ID, m.Descr }).Distinct().ToList();
            if (pStatus.Count > 0)
            {
                cmbConvPartyName.DataSource = pStatus;
                cmbConvPartyName.ValueMember = "ID";
                cmbConvPartyName.DisplayMember = "Descr";
                cmbConvPartyName.SelectedIndex = -1;
            }

            var pReason = (from m in db.Attributes_Datas where m.Head_Name == "Reason for No Production" select new { m.ID, m.Descr }).Distinct().ToList();
            if (pReason.Count > 0)
            {
                cmbReason.DataSource = pReason;
                cmbReason.ValueMember = "ID";
                cmbReason.DisplayMember = "Descr";
                cmbReason.SelectedIndex = -1;
            }



            //}
            if (Lst_Productionreport_rolling.editMode == true)
            {
                bindedit();
            }
            else
            {

                AutoincrementId();
            }
            //var l = (from a in db.Products
            //         join u in db.UoM_Masters on a.Prod_Primary_UOM_Id equals u.UOM_ID
            //         join g in db.Product_Groups on a.Prod_Group_Id equals g.ID
            //         where a.Company_ID == logIn.company
            //         select new { a.Prod_Name, a.prod_ID }).ToList();
            //if (l.Count > 0)
            //{
            //    cmbrawmat.DataSource = l;
            //    cmbrawmat.ValueMember = "prod_ID";
            //    cmbrawmat.DisplayMember = "Prod_Name";
            //    if (cmbrawmat.Items.Count > 0)
            //    {
            //        cmbrawmat.SelectedIndex = -1;
            //    }
            //    else
            //    {
            //        cmbrawmat.SelectedIndex = -1;
            //    }
            //}




        }

        public void bindedit()
        {
            try
            {
                if (ProductionManagement.Transactions.Lst_Productionreport_rolling.PR_No != null)
                {
                    txtrepNoa.Text = ProductionManagement.Transactions.Lst_Productionreport_rolling.PR_No;
                }

                String myString = "";
                int pr_id = 0;
                myString = txtrepNoa.Text;
                var da = (from obj in db.Production_report_rollings
                          where obj.PR_No == txtrepNoa.Text && obj.Company_Id == logIn.company
                          select obj).ToList();

                if (da.Count > 0)
                {
                    pr_id = da[0].ID;
                    txtrepNoa.Text = da[0].PR_No.ToString();
                    dpPDate.Text = da[0].PR_date.ToString();
                   
                    //CmbsectionrollName.SelectedValue = da[0].Prod_ID;

                    TxtrolledQty.Text = (da[0].Rolled_Qty).ToString();
                    txtQCrej.Text = (da[0].QC_Rej_Qty).ToString();
                    txtMissroll.Text = (da[0].Miss_Roll).ToString();
                    txtEndCuts.Text = (da[0].Crop_Ends).ToString();
                    txtBurnloss.Text = (da[0].Burn_Loss).ToString();
                    txtFinishedQty.Text = da[0].Finished_Qty.ToString();
                    txtLocalQty.Text = da[0].Local_Qty.ToString();
                    textBox5.Text = da[0].Coal_Used.ToString();
                    textBox7.Text = da[0].Power_Used.ToString();
                    txtcoalbymt.Text = da[0].Coal_by_mt.ToString();
                    txtUnitbymt.Text = da[0].Unit_by_mt.ToString();
                    txttotal_wrk_hrs.Text = da[0].Tot_Work_hrs.ToString();
                    txt_breakdwnhrs.Text = da[0].Breakdown_hrs.ToString();
                    txt_eff_work.Text = da[0].Effictive_work.ToString();
                    txt_prod_rate.Text = da[0].Production_rate_by_br.ToString();
                    chkConversion.Checked = false;
                    cmbConvPartyName.SelectedValue = da[0].Party_Name;
                    if (da[0].Conversion_Production ==true)
                    {
                        chkConversion.Checked = true;
                        //cmbConvPartyName.SelectedValue = da[0].Party_Name;
                        
                    }
                    
                    chkNoProduction.Checked = false;
                    if (da[0].No_production == true)
                    {
                        chkNoProduction.Checked = true;
                        cmbReason.SelectedValue = da[0].Reason_No_Production;
                    }
                    
                    
                    
                    


                }


                //var dm1 = (from s in db.Production_report_rolling_Childs
                //           join p in db.Products on s.Prod_ID equals p.prod_ID
                //           join g in db.QA_Mtrl_Grade_Masters on s.RM_Grade equals g.id
                //           where s.PR_Master_ID == pr_id && s.Company_Id == logIn.company
                //           orderby s.ID


                //           select new

                //           {
                //               RM_Prod_ID = p.prod_ID,
                //               RM_Size = p.Prod_Name,
                //               RM_Grade = g.Material_Grade,
                //               s.RM_Lot_No,
                //               s.RM_Lot_Qty,                              
                //               RM_iss_Qty = s.RM_iss_Qty,
                //               Rm_Cut_Qty = s.Rm_Cut_Qty,
                //               GC_Loss = s.GC_Loss,
                //           });




                //SqlCommand cmd2 = (SqlCommand)db.GetCommand(dm1);
                //SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataTable dtr = new DataTable();
                //da2.Fill(dtr);
                //if (dtr.Rows.Count >= 0)
                //    dgrmconsumed.DataSource = dtr;


                var dm2 = (from s in db.Production_Report_Rolling_Sections
                           join p in db.Products on s.Prod_Id   equals p.prod_ID
                           join r in db.Production_report_rollings on s.PR_Master_ID equals r.ID
                           join g in db.QA_Mtrl_Grade_Masters on s.Prod_Grade equals g.id
                           where r.PR_No == myString && r.Company_Id == logIn.company
                           orderby s.id


                           select new

                           {
                               p.prod_ID,
                               Section_Produced = p.Prod_Name,
                               //s.Prod_Length,
                               Mtrl_Grade = g.Material_Grade,
                               s.Rolled_Qty,
                               Quality_Rjectection = s.Qty_Rejected,
                               s.Miss_Roll_Qty,
                               s.End_Cuts_Qty,
                               s.Burning_Loss_Qty,
                               Local_Sale = s.Qty_Local,
                               s.Qty_Finished,
                               s.FG_Lot_No,
                               s.Remarks
                              
                               
                              
                           });




                SqlCommand cmd3 = (SqlCommand)db.GetCommand(dm2);
                SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                DataTable dtr1 = new DataTable();
                da3.Fill(dtr1);
                if (dtr1.Rows.Count >= 0)
                    dgSectionProduced.DataSource = dtr1;


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
                var getSufix = (from m in db.Financial_Year_Masters where m.Company_ID == logIn.company && m.Start_Date == logIn.fy_Start_Date select new { m.Uses_AsSufix }).Distinct().ToList();
                if (getSufix.Count > 0)
                {
                //    if (getSufix[0].Uses_AsSufix == true)
                //    {

                        var result = db.Sp_autoincrement_ProductionReport_Rolling(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date, logIn.BU_ID);
                        txtrepNoa.Text = result.FirstOrDefault().PR_No;
                  //  }
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
                DataGridViewRow R1 = dgrmconsumed.Rows[dgrmconsumed.CurrentRow.Index];

                int columnIndex = dgrmconsumed.CurrentCell.ColumnIndex;
                string columnName = dgrmconsumed.Columns[columnIndex].HeaderText;
                if (columnName == "Raw Material Size")
                {
                    var Prodname = (from a in db.Products
                                    join u in db.UoM_Masters on a.Prod_Primary_UOM_Id equals u.UOM_ID
                                    join g in db.Product_Groups on a.Prod_Group_Id equals g.ID
                                    where a.Company_ID == logIn.company && a.Prod_Type_Id == 140 && a.Prod_Status_ID ==1
                                    select new { a.Prod_Name, a.prod_ID }).ToList();
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
                if (columnName == "RM_Grade")
                {
                    var Prodname = (from d in db.QA_Mtrl_Grade_Masters select new { d.Material_Grade }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("RM_Grade");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Material_Grade);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
                    }
                }
                if (columnName == "RM_Lot_No")
                {
                    var Prodname = (from d in db.Incoming_Chemical_Reports where d.Accepted_Grade == R1.Cells["RM_Grade"].Value.ToString() select new { d.New_Batch_No }).Distinct().ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("RM_Lot_No");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.New_Batch_No);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
                    }
                }
            }
            catch
            { 
            }

        }
        public void addSections(AutoCompleteStringCollection coll)
        {
            try
            {
                DataGridViewRow R1 = dgSectionProduced.Rows[dgSectionProduced.CurrentRow.Index];

                int columnIndex = dgSectionProduced.CurrentCell.ColumnIndex;
                string columnName = dgSectionProduced.Columns[columnIndex].HeaderText;
                if (columnName == "Section Produced")
                {
                    var Prodname = (from a in db.Products
                                    join u in db.UoM_Masters on a.Prod_Primary_UOM_Id equals u.UOM_ID
                                    join g in db.Product_Groups on a.Prod_Group_Id equals g.ID
                                    where a.Company_ID == logIn.company && a.Prod_Type_Id == 139
                                    select new { a.Prod_Name, a.prod_ID }).ToList();
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
                if (columnName == "Mtrl_Grade")
                {
                    var Prodname = (from d in db.QA_Mtrl_Grade_Masters select new { d.Material_Grade }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Mtrl_Grade");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Material_Grade);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
                    }
                }
            }
            catch
            {
            }

        }
    }
} 
