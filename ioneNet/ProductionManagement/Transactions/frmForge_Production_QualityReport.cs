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
namespace ioneNet.ProductionManagement.Transactions
{
    public partial class frmForge_Production_QualityReport : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public frmForge_Production_QualityReport()
        {
            InitializeComponent();
        }

        private void frmForge_Production_Forging_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

            AutoincrementId();
           
            //Main Process
            
            var pStatus = (from m in db.Forging_ProcessMasters where m.Company_ID == logIn.company && m.Main_Process_ID == 4 select new { m.ID, m.Process_Name }).Distinct().ToList();
            if (pStatus.Count > 0)
            {
                cmbProcess.DataSource = pStatus;
                cmbProcess.ValueMember = "ID";
                cmbProcess.DisplayMember = "Process_Name";
                ////if (cmbProcess.Items.Count > 0)
                ////{
                cmbProcess.SelectedIndex = -1;
                //    }
                //    else
                //    {
                //        cmbProcess.SelectedIndex = -1;
                //    }
            }

            //var Sup = (from k in db.Forging_Heat_TreatmentReports select new { k.s }).Distinct().ToList();
            //if (Sup.Count > 0)
            //{
            //    cmbSupervisor.DataSource = Sup;
            //    cmbSupervisor.DisplayMember = "Supervisor";
            //    cmbSupervisor.ValueMember = "Supervisor";
            //    if (cmbSupervisor.Items.Count > 0)
            //    {
            //        cmbSupervisor.SelectedIndex = -1;
            //    }
            //    else
            //    {
            //        cmbSupervisor.SelectedIndex = -1;
            //    }
            //}

        }
        public void AutoincrementId()
        {
            try
            {

                var result = db.Sp_autoincrement_Forging_Quality_Report (logIn.company);
                txtvchno.Text = result.FirstOrDefault().Report_No;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgJobCardData_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {

                DataGridViewRow R1 = dgJobCardData.Rows[dgJobCardData.CurrentRow.Index];
                int columnIndex = dgJobCardData.CurrentCell.ColumnIndex;
                string columnName = dgJobCardData.Columns[columnIndex].Name;
                if (columnName == "Machine_Drawing" && R1.Cells["Machine_Drawing"].Value != null)
                {
                    //Get Previous Process ID

                    Boolean GetProcess = false;
                    int PID = Convert.ToInt32(cmbProcess.SelectedValue);
                    int PrevProcessID = 0;
                    while (GetProcess == false)
                    {
                        var ProcID = (from s in db.Forging_ProcessMasters
                                      where s.ID == PID
                                      select new { s.PreDessor_Process }).ToList();

                        var ProcID1 = (from s in db.GetPreviousProcess(R1.Cells["Job_Card"].Value.ToString(), ProcID[0].PreDessor_Process)
                                       select new { s.Process_ID }).ToList();
                        if (ProcID1.Count > 0)
                        {
                            GetProcess = true;
                            PrevProcessID = Convert.ToInt32(ProcID1[0].Process_ID);
                        }
                        else
                        {
                            PID = Convert.ToInt32(ProcID[0].PreDessor_Process);
                        }
                    }

                    var getDetails = (from s in db.Forge_Get_MachinedQty_ForQuality(R1.Cells["Job_Card"].Value.ToString(), Convert.ToInt32(cmbProcess.SelectedValue), PrevProcessID, R1.Cells["Machine_Drawing"].Value.ToString())
                                      select new { s.J_Date, s.FG_Item_Code, s.FG_Item_Name, s.Job_CardQty, s.Heat_Code, s.Qty_Avalable, s.Qty_Pending, s.Qty_Accepted, s.FG__Code }).FirstOrDefault();
                    //var getDetails = (from s in db.Forge_Get_MachinedQty_ForQuality(logIn.company, R1.Cells["Job_Card"].Value.ToString(), R1.Cells["Machine_Drawing"].Value.ToString())
                    //                  select new { s.FG_Item_Code, s.Qty_Available }).FirstOrDefault();

                    if (getDetails != null)
                    {
                      //  R1.*/Cells["Job_card_date"].Value = getDetails.J_Date.ToString();
                        R1.Cells["Item_Code"].Value = getDetails.FG_Item_Code.ToString();                     
                        R1.Cells["Job_Card_Qty"].Value = getDetails.Qty_Avalable.ToString();
                        //R1.Cells["Pending_Qty"].Value = getDetails.Qty_Pending.ToString();
                        
                        //Fill HT Batch Number

                    }
                    else
                    {
                        MessageBox.Show("Invalid Job Card Entered");
                        return;
                    }
                   
                }
                
                //if (columnName == "Accepted_qty" || columnName == "Finishing_Qty")
                //{
                //    //int Itemcode = Convert.ToInt32(R1.Cells["Item_Code"].Value.ToString());
                //    if (R1.Cells["Job_Card"].Value != null)
                //    {
                //        decimal Qty_Rough = (R1.Cells["Rough_Grinding"].Value == "" || R1.Cells["Rough_Grinding"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Rough_Grinding"].Value);
                //        decimal Qty_Finish = (R1.Cells["Finishing_Qty"].Value == "" || R1.Cells["Finishing_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Finishing_Qty"].Value);
                        



                //    }

                //}
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
            }
        }

        private void dgJobCardData_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                int columnIndex = dgJobCardData.CurrentCell.ColumnIndex;
                string columnName = dgJobCardData.Columns[columnIndex].HeaderText;
                TextBox tb3 = e.Control as TextBox;
                if (tb3 != null && columnName == "Job Card No")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }

                if (tb3 != null && columnName == "Maching Draiwng No")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }

                if (tb3 != null && columnName == "Inspected_By")
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
        public void addItems(AutoCompleteStringCollection coll)
        {
            try
            {
                DataGridViewRow R1 = dgJobCardData.Rows[dgJobCardData.CurrentRow.Index];

                int columnIndex = dgJobCardData.CurrentCell.ColumnIndex;
                string columnName = dgJobCardData.Columns[columnIndex].HeaderText;

                if (columnName == "Job Card No")
                {
                    var Prodname = (from d in db.sp_Forging_Get_JobCards_Quality(cmbProcess.Text)
                                    select new { d.Job_CardNo }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Job_CardNo");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Job_CardNo);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
                    }
                }
                else
                {
                    if (columnName == "Inspected_By")
                    {
                        var Prodname = (from d in db.Forging_Employees where d.Department == "Quality" && d.Designation == "Inspector" select new { d.Employee_Name }).ToList().Distinct();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Inspected_By");
                        foreach (var item in Prodname)
                        {
                            dt.Rows.Add(item.Employee_Name);
                        }
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            coll.Add(dt.Rows[i][0].ToString());
                        }
                    }

                    //Maching Draiwng No
                    else
                    {
                        if (columnName == "Maching Draiwng No")
                        {
                            var Prodname = (from d in db.Forging_JobCard_MachiningPlans where d.Job_CardNo == R1.Cells["Job_Card"].Value.ToString() select new { d.Machine_Drawing_No }).ToList().Distinct();
                            DataTable dt = new DataTable();
                            dt.Columns.Add("Machine_Drawing_No");
                            foreach (var item in Prodname)
                            {
                                dt.Rows.Add(item.Machine_Drawing_No);
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

        private void cmbProcess_Leave(object sender, EventArgs e)
        {
            try
            {
               
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
                String myString = "";
                myString = txtvchno.Text;
                if ((from a in db.Forging_Quality_Reports where a.Company_ID == logIn.company && a.Report_No == txtvchno.Text select a).Count() > 0)
                {
                    myString = txtvchno.Text;
                    db.Sp_delete_Production_Quality(logIn.company, txtvchno.Text);    
                    //Save Child Data (Job Card)
                    for (int i = 0; i < dgJobCardData.Rows.Count - 1; i++)
                    {
                        Forging_Quality_Report SC = new Forging_Quality_Report();
                        SC.Job_CardNo = (dgJobCardData.Rows[i].Cells["Job_Card"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Job_Card"].Value).ToString();
                        SC.Report_No = txtvchno.Text;
                        SC.Report_Date = dpdate.Value;
                        //pb.Shift = cmbShift.Text;
                        SC.Process = Convert.ToInt32(cmbProcess.SelectedValue);
                        SC.Company_ID = logIn.company;
                        SC.Modified_By = logIn.username + "-" + DateTime.Now;
                        SC.FG_Item_Code = (dgJobCardData.Rows[i].Cells["Item_Code"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Item_Code"].Value).ToString();
                        SC.Job_CardQty = (dgJobCardData.Rows[i].Cells["Job_Card_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Job_Card_Qty"].Value);
                        SC.Balance_Qty = (dgJobCardData.Rows[i].Cells["Pending_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Pending_Qty"].Value);
                        SC.Accepted_Qty = (dgJobCardData.Rows[i].Cells["Accepted_qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Accepted_qty"].Value);
                        SC.Rework_qty = (dgJobCardData.Rows[i].Cells["ReWork_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["ReWork_Qty"].Value);                                         
                        SC.Rejectec_Qty = (dgJobCardData.Rows[i].Cells["Rejected_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Rejected_Qty"].Value);
                        SC.Inspector = (dgJobCardData.Rows[i].Cells["Inspected_By"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Inspected_By"].Value).ToString();
                        db.Forging_Quality_Reports.InsertOnSubmit(SC);
                    }
                    db.SubmitChanges();
                    MessageBox.Show("Record Updated Sucessfully");
                    clear();
                    return;
                }
                else
                {
                    AutoincrementId();
                    for (int i = 0; i < dgJobCardData.Rows.Count - 1; i++)
                    {
                        Forging_Quality_Report SC = new Forging_Quality_Report();
                        SC.Job_CardNo = (dgJobCardData.Rows[i].Cells["Job_Card"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Job_Card"].Value).ToString();
                        SC.Report_No = txtvchno.Text;
                        SC.Report_Date = dpdate.Value;
                        //pb.Shift = cmbShift.Text;
                        SC.Process = Convert.ToInt32(cmbProcess.SelectedValue);
                        //  pb.Supervisor = (cmbSupervisor.Text == null) ? "" : cmbSupervisor.Text;                       
                        SC.Company_ID = logIn.company;
                        SC.Modified_By = logIn.username + "-" + DateTime.Now;
                        SC.Created_By = logIn.username + "-" + DateTime.Now;
                        SC.FG_Item_Code = (dgJobCardData.Rows[i].Cells["Item_Code"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Item_Code"].Value).ToString();
                        SC.Job_CardQty = (dgJobCardData.Rows[i].Cells["Job_Card_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Job_Card_Qty"].Value);
                        SC.Balance_Qty = (dgJobCardData.Rows[i].Cells["Pending_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Pending_Qty"].Value);
                        SC.Accepted_Qty = (dgJobCardData.Rows[i].Cells["Accepted_qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Accepted_qty"].Value);
                        SC.Rework_qty = (dgJobCardData.Rows[i].Cells["ReWork_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["ReWork_Qty"].Value);
                        SC.Rejectec_Qty = (dgJobCardData.Rows[i].Cells["Rejected_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Rejected_Qty"].Value);
                        SC.Inspector = (dgJobCardData.Rows[i].Cells["Inspected_By"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Inspected_By"].Value).ToString();
                        db.Forging_Quality_Reports.InsertOnSubmit(SC);
                    }
                    db.SubmitChanges();
                    MessageBox.Show("Record Saved Sucessfully");
                    clear();
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void clear()
        {
            txtvchno.Text = "";
            AutoincrementId();
            dpdate.Value = DateTime.Now;
            
            //cmbMachineID.SelectedValue = -1;
            cmbProcess.SelectedValue = -1;
            
            cmbSupervisor.Text = "";
            txtRemarks.Text = "";
           
            if (dgJobCardData.Rows.Count >= 1)
            {
                for (int i = 0; i < dgJobCardData.Rows.Count - 1; i++)
                {
                    dgJobCardData.Rows.RemoveAt(i);
                    i--;
                    while (dgJobCardData.Rows.Count == 0)
                        continue;
                }
            }
        }
        private void cmbProcess_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            try
            {
                ProductionManagement.frmForging_Production_QualityReportList obj = new frmForging_Production_QualityReportList();
                if (obj.ShowDialog() == DialogResult.OK)
                {
                    int Report_ID = 0;
                    txtvchno.Text = ProductionManagement.frmForging_Production_QualityReportList.Voucherno;
                    var sa = (from sq in db.Forging_Quality_Reports
                              where sq.Company_ID == logIn.company && sq.Report_No == txtvchno.Text
                              select new
                              {
                                  sq.id,
                                  sq.Report_Date,    
                                  sq.Created_By,
                                  sq.Modified_By,     
                                  sq.Process,
                                  sq.Job_CardNo,
                                  

                              }).ToList();
                    if (sa.Count > 0)
                    {
                        Report_ID = sa[0].id;
                        dpdate.Value = Convert.ToDateTime(sa[0].Report_Date);                          
                        lblCreatedBy.Text = sa[0].Created_By;
                        lblModified.Text = sa[0].Modified_By;                       
                        cmbProcess.SelectedValue = sa[0].Process;
                        cmbProcess_Leave(sender, e);                     
                     
                    }
                    var ca = (from sq in db.Forging_Quality_Reports
                              where sq.Report_No == txtvchno.Text
                              select new
                              {

                                  Job_Card = sq.Job_CardNo,                                  
                                  Item_Code = sq.FG_Item_Code,                                 
                                  Job_Card_Qty = sq.Job_CardQty,
                                  Pending_Qty = sq.Balance_Qty,
                                  Accepted_qty = sq.Accepted_Qty,
                                  ReWork_Qty = sq.Rework_qty,
                                  Rejected_Qty= sq.Rejectec_Qty,
                                  Inspected_By = sq.Inspector,  
                                  Remarks = sq.Line_Remarks                                 
                              });
                    SqlCommand cmd3 = (SqlCommand)db.GetCommand(ca);
                    SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                    DataTable dt1 = new DataTable();
                    da3.Fill(dt1);
                    if (dt1.Rows.Count > 0)
                        dgJobCardData.DataSource = dt1;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            clear();
        }

        private void txtBreakDown_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
           // groupBox1.Visible = false;
        }

        private void btnParameters_Click(object sender, EventArgs e)
        {


           // groupBox1.Visible = true;
        }

        private void txtStartTime_Leave(object sender, EventArgs e)

        {

        }
        public void ClaculateWHrs()
        {
           // decimal DieSetting = (txtDieSetting.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtDieSetting.Text);
           //// decimal LunchTime = (txtLunchTime.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtLunchTime.Text);
           // decimal PowerCut = (txtPowerCut.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtPowerCut.Text);
           // decimal BreakDown = (txtBreakDown.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtBreakDown.Text);
           // decimal NoPlan = (txtNoPlan.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtNoPlan.Text);
           // decimal ManPower = (txtManPower.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtManPower.Text);
           // decimal Others = (txtOthers.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtOthers.Text);


           //     decimal TotHrs =DieSetting+PowerCut+BreakDown+NoPlan+ManPower+Others ;
           //txtTotalHrs.Text = (TotHrs/60).ToString("0.00");
           // decimal WHrs = (txtWorkingHrs.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtWorkingHrs.Text);

           // decimal TotAvblHrs = WHrs - (TotHrs / 60);
           // txtAllocatedHrs.Text = TotAvblHrs.ToString("0.00");
           // decimal IPHrs = Convert.ToDecimal(txtIdealHrs.Text);
           // decimal NoReasonHrs = (TotAvblHrs - IPHrs);
           // txtNoReasonHrs.Text = NoReasonHrs.ToString("0.00");
           // if (WHrs > 0)
           // {
           //     decimal IPHEff = IPHrs / WHrs * 100;
           //     txtIPHEff.Text = IPHEff.ToString("0.00");
           // }
            
            //Get sum of ideal Hrs

        }

        private void txtDieSetting_Leave(object sender, EventArgs e)
        {
            ClaculateWHrs();
        }

        private void txtLunchTime_Leave(object sender, EventArgs e)
        {
            ClaculateWHrs();
        }

        private void txtPowerCut_Leave(object sender, EventArgs e)
        {
            ClaculateWHrs();
        }

        private void txtBreakDown_Leave(object sender, EventArgs e)
        {
            ClaculateWHrs();
        }

        private void txtNoPlan_Leave(object sender, EventArgs e)
        {
            ClaculateWHrs();
        }

        private void txtManPower_Leave(object sender, EventArgs e)
        {
            ClaculateWHrs();
        }

        private void txtOthers_Leave(object sender, EventArgs e)
        {
            ClaculateWHrs();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are You Sure To Delete The Record? Cannot Undo This Operation!", "Delete Confirmation", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                db.Sp_delete_Production_Quality(logIn.company, txtvchno.Text);
                MessageBox.Show("Record Deleted Successfully");
                clear();
            }
        }

        private void txtNoReasonHrs_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
