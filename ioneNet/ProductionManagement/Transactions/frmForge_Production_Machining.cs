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
    public partial class frmForge_Production_Machining : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);
        public static string ReportNo, Operation;
        public static int PrevProcessID = 0;
        public frmForge_Production_Machining()
        {
            InitializeComponent();
        }

        private void frmForge_Production_Forging_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            txtJobCardNo.AutoCompleteMode = AutoCompleteMode.Suggest;
            txtJobCardNo.AutoCompleteSource = AutoCompleteSource.CustomSource;
            AutoincrementId();
            var machine = (from k in db.Forging_MachineMasters where k.Machine_Type == "Machining" select new { k.Machine_Name, k.ID }).ToList();
            if (machine.Count > 0)
            {


                cmbMachineID.DataSource = machine;
                cmbMachineID.DisplayMember = "Machine_Name";
                cmbMachineID.ValueMember = "ID";
                if (cmbMachineID.Items.Count > 0)
                {
                    cmbMachineID.SelectedIndex = -1;
                }
                else
                {
                    cmbMachineID.SelectedIndex = -1;
                }
            }

            //Main Process
            
            var pStatus = (from m in db.Forging_ProcessMasters where m.Company_ID == logIn.company && m.Main_Process_ID == 11
                           orderby m.seq_id
                           select new { m.ID, m.Process_Name }).Distinct().ToList();
            if (pStatus.Count > 0)
            {
                cmbProcess.DataSource = pStatus;
                cmbProcess.ValueMember = "ID";
                cmbProcess.DisplayMember = "Process_Name";               
                cmbProcess.SelectedIndex = -1;
               
            }

            var Oprn = (from k in db.Forging_Employees where k.Department == "Machining" && k.Designation == "Operator" select new { k.Employee_Name }).Distinct().ToList();
            if (Oprn.Count > 0)
            {
                cmbOperator.DataSource = Oprn;
                cmbOperator.DisplayMember = "Employee_Name";
                cmbOperator.ValueMember = "Employee_Name";
                if (cmbOperator.Items.Count > 0)
                {
                    cmbOperator.SelectedIndex = -1;
                }
                else
                {
                    cmbOperator.SelectedIndex = -1;
                }
            }

            var Sup = (from k in db.Forging_Employees where k.Department == "Machining" && k.Designation == "Supervisor" select new { k.Employee_Name }).Distinct().ToList();
            if (Sup.Count > 0)
            {
                cmbSupervisor.DataSource = Sup;
                cmbSupervisor.DisplayMember = "Employee_Name";
                cmbSupervisor.ValueMember = "Employee_Name";
                if (cmbSupervisor.Items.Count > 0)
                {
                    cmbSupervisor.SelectedIndex = -1;
                }
                else
                {
                    cmbSupervisor.SelectedIndex = -1;
                }
            }

        }
        public void AutoincrementId()
        {
            try
            {

                var result = db.Sp_autoincrement_Forging_MachiningReport(logIn.company);
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
                if (columnName == "HT_BatchNo")
                {

                    Boolean GetProcess = false;
                    int PID = Convert.ToInt32(cmbProcess.SelectedValue);
                    int PrevProcessID = 0;
                    while (GetProcess == false)
                    {
                        var ProcID = (from s in db.Forging_ProcessMasters
                                      where s.ID == PID
                                      select new { s.PreDessor_Process }).ToList();

                        var ProcID1 = (from s in db.GetPreviousProcess(txtJobCardNo.Text, ProcID[0].PreDessor_Process)
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
                    if (R1.Cells["HT_BatchNo"].Value.ToString() == "NA")
                    {
                        var getDetails = (from s in db.sp_Forging_JopbCard_Process_Stage(txtJobCardNo.Text, Convert.ToInt32(cmbProcess.SelectedValue), PrevProcessID)
                                          select new { s.J_Date, s.FG_Item_Code, s.FG_Item_Name, s.Job_CardQty, s.Heat_Code, s.Qty_Avalable, s.Qty_Pending, s.Qty_Accepted, s.FG__Code }).FirstOrDefault();

                        if (getDetails != null)
                        {
                           // R1.Cells["Job_card_date"].Value = getDetails.J_Date.ToString();
                            //R1.Cells["Item_Code"].Value = getDetails.FG_Item_Code.ToString();
                            //R1.Cells["Item_Name"].Value = getDetails.FG_Item_Name.ToString();
                            R1.Cells["Heat_Code"].Value = getDetails.Heat_Code.ToString();
                            
                            R1.Cells["Pending_Qty"].Value = getDetails.Qty_Pending.ToString();
                            //R1.Cells["RM_Sec"].Value = getDetails.RM_Name.ToString();
                            R1.Cells["Batch_Qty"].Value = getDetails.Job_CardQty.ToString(); 

                        }
                    }
                    else
                    {
                        var Prodname = (from d in db.Forge_Get_HT_BatchNo(logIn.company, txtJobCardNo.Text, PrevProcessID, PID, R1.Cells["HT_BatchNo"].Value.ToString(), 2) select new { d.bqty, d.Heat_Code }).ToList();
                        if (Prodname.Count > 0)
                        {
                            R1.Cells["Batch_Qty"].Value = Prodname[0].bqty.ToString();
                            decimal QtyAvbl = Convert.ToDecimal(Prodname[0].bqty.ToString());
                            decimal QtyProd = 0;
                            var HBQty = (from d in db.Forge_Get_HT_BatchQty_ForMachining(logIn.company, txtJobCardNo.Text, Convert.ToInt32(cmbProcess.SelectedValue), R1.Cells["HT_BatchNo"].Value.ToString()) select new { d.Qty_Produced }).ToList();
                            if (HBQty.Count > 0)
                            {
                                QtyProd = Convert.ToDecimal(HBQty[0].Qty_Produced.ToString());
                            }
                            R1.Cells["Pending_Qty"].Value = QtyAvbl - QtyProd;
                            R1.Cells["Heat_Code"].Value = Prodname[0].Heat_Code.ToString();
                        }
                        else
                        {
                            MessageBox.Show("Invalid Heat Number");
                            R1.Cells["HT_BatchNo"].Value = "";
                            return;
                        }
                    }
                }

                if (columnName == "Qty_Produced")
                {
                    //int Itemcode = Convert.ToInt32(R1.Cells["Item_Code"].Value.ToString());
                    if (R1.Cells["HT_BatchNo"].Value != null)
                    {
                        decimal Qty_Planned = Convert.ToDecimal(txtPlannedQty.Text);
                        decimal Qty_Pending = (R1.Cells["Pending_Qty"].Value == "" || R1.Cells["Pending_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Pending_Qty"].Value);
                        decimal Qty_Accepted = (R1.Cells["Qty_Produced"].Value == "" || R1.Cells["Qty_Produced"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty_Produced"].Value);

                        if (Qty_Planned == 0)
                        {
                            MessageBox.Show("Planning Qty is Zero, Cannot Proceed With Machining");
                            R1.Cells["Qty_Produced"].Value = "";
                            return;
                        }
                        else
                        {
                            if (Qty_Accepted <= Qty_Planned)
                            {
                                if (Qty_Accepted <= Qty_Pending)
                                {
                                    
                                   
                                        //Qty_Accepted = Qty_Produced - Qty_Rejected;
                                        R1.Cells["Qty_Produced"].Value = Qty_Accepted.ToString("0.00");
                                    
                                }
                                else
                                {
                                    MessageBox.Show("Qty Produced Cannot Be Greater Than Qty Pending");
                                    R1.Cells["Qty_Produced"].Value = 0;                                    
                                }
                            }
                            else
                            {
                                MessageBox.Show("Qty Accepted Cannot Be Greater Than Qty Planned");
                                R1.Cells["Qty_Produced"].Value = 0;
                            }
                        }

                        //decimal Qty_Accepted = Qty_Produced - Qty_Rejected;
                        //decimal StockQty = (R1.Cells["Stock_Qty"].Value == "" || R1.Cells["Stock_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Stock_Qty"].Value);
                        //decimal price = (R1.Cells["Price"].Value == "" || R1.Cells["Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Price"].Value);

                        //R1.Cells["Qty_Accepted"].Value = Qty_Accepted.ToString("0.00");
                        if (Convert.ToDecimal(txtPR.Text) > 0)
                        {
                            decimal Ideal_Hrs = Convert.ToDecimal(R1.Cells["Qty_Produced"].Value) / Convert.ToDecimal(txtPR.Text);
                            //PR

                            R1.Cells["Ideal_Hrs"].Value = Ideal_Hrs.ToString("0.00");
                        }

                       
                    }

                }
                if (columnName == "Serial_End")
                {
                    decimal x = 0;
                    decimal startTime = Convert.ToDecimal(R1.Cells["Serial_Start"].Value);
                    decimal EndTime = Convert.ToDecimal(R1.Cells["Serial_End"].Value);
                    x = EndTime - startTime;
                    R1.Cells["Qty_Received"].Value = x.ToString("0.00");



                }

                if (columnName == "Working_Hrs")
                {
                  
                    decimal WHrs = (R1.Cells["Working_Hrs"].Value == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(R1.Cells["Working_Hrs"].Value);

                   // decimal TotAvblHrs = WHrs - (TotHrs / 60);
                    //txtAllocatedHrs.Text = TotAvblHrs.ToString("0.00");
                    decimal IPHrs = Convert.ToDecimal(R1.Cells["Ideal_Hrs"].Value);
                    //decimal NoReasonHrs = (TotAvblHrs - IPHrs);
                    //txtInsertChange.Text = NoReasonHrs.ToString("0.00");
                    if (WHrs > 0)
                    {
                        decimal IPHEff = IPHrs / WHrs * 100;
                        R1.Cells["IPH_Eff"].Value = IPHEff.ToString("0.00");
                    }
                    
                }
                decimal iHrs = 0, wHrs=0,LMin=0;
                for (int i = 0; i < dgJobCardData.Rows.Count - 1; i++)
                {
                    iHrs += (dgJobCardData.Rows[i].Cells["Ideal_Hrs"].Value == "" || dgJobCardData.Rows[i].Cells["Ideal_Hrs"].Value == null || dgJobCardData.Rows[i].Cells["Ideal_Hrs"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Ideal_Hrs"].Value);
                    wHrs += (dgJobCardData.Rows[i].Cells["Working_Hrs"].Value == "" || dgJobCardData.Rows[i].Cells["Working_Hrs"].Value == null || dgJobCardData.Rows[i].Cells["Working_Hrs"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Working_Hrs"].Value);
                    LMin += (dgJobCardData.Rows[i].Cells["Loss_Mins"].Value == "" || dgJobCardData.Rows[i].Cells["Loss_Mins"].Value == null || dgJobCardData.Rows[i].Cells["Loss_Mins"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Loss_Mins"].Value);

                }
                txtIdealHrs.Text = iHrs.ToString();
                txtAllocatedHrs.Text = wHrs.ToString();
                txtTotalLossHrs.Text = (LMin / 60).ToString("0.00");
                if (wHrs > 0)
                {
                    decimal iPE = iHrs / wHrs * 100;
                    txtIPHEff.Text = iPE.ToString("0.00");
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
                if (tb3 != null && columnName == "HT Batch No")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }

                if (tb3 != null && columnName == "Disposal")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }

                if (tb3 != null && columnName == "Disposal")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }

                if (tb3 != null && columnName == "Machine / Press")
                {
                    tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                    tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                    AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                    addItems(DataColl);
                    tb3.AutoCompleteCustomSource = DataColl;
                }

                if (tb3 != null && columnName == "Operator_Name")
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
                    var Prodname = (from d in db.sp_Forging_GetProcessInvoiced(cmbProcess.Text)


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
                if (columnName == "HT Batch No")
                {
                        Boolean GetProcess = false;
                        int PID = Convert.ToInt32(cmbProcess.SelectedValue);
                        int PrevProcessID = 0;
                        while (GetProcess == false)
                        {
                            var ProcID = (from s in db.Forging_ProcessMasters
                                          where s.ID == PID
                                          select new { s.PreDessor_Process }).ToList();

                            var ProcID1 = (from s in db.GetPreviousProcess(txtJobCardNo.Text, ProcID[0].PreDessor_Process)
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

                        var Prodname = (from d in db.Forge_Get_HT_BatchNo(logIn.company, txtJobCardNo.Text, PrevProcessID, PID, null, 1) select new { d.HT_BatchNo }).ToList();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("HT_BatchNo");
                        foreach (var item in Prodname)
                        {
                            dt.Rows.Add(item.HT_BatchNo);
                        }
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            coll.Add(dt.Rows[i][0].ToString());
                        }
                    }
                else
                {
                    if (columnName == "Disposal")
                    {
                        //  var Prodname = (from d in db.Forging_JobCardRMs where d.Job_CardNo == R1.Cells["Job_Card"].Value.ToString() select new { d.Heat_Code }).ToList();
                        DataTable dt = new DataTable();
                        dt.Columns.Add("Disposal");
                        dt.Rows.Add("Stock");
                        dt.Rows.Add("Scrap");

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            coll.Add(dt.Rows[i][0].ToString());
                        }
                    }
                    else
                    {
                            if (columnName == "Machine / Press")
                            {
                                var Prodname = (from d in db.Forging_MachineMasters where d.Machine_Type == "Forging" select new { d.Machine_Name }).ToList();
                                DataTable dt = new DataTable();
                                dt.Columns.Add("Machine_name");
                                foreach (var item in Prodname)
                                {
                                    dt.Rows.Add(item.Machine_Name);
                                }
                                for (int i = 0; i < dt.Rows.Count; i++)
                                {
                                    coll.Add(dt.Rows[i][0].ToString());
                                }
                            }
                            else
                            {
                                if (columnName == "Operator_Name")
                                {
                                    var Prodname = (from d in db.Forging_Forging_Childs select new { d.Operator_Name }).ToList().Distinct();
                                    DataTable dt = new DataTable();
                                    dt.Columns.Add("Operator_Name");
                                    foreach (var item in Prodname)
                                    {
                                        dt.Rows.Add(item.Operator_Name);
                                    }
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        coll.Add(dt.Rows[i][0].ToString());
                                    }
                                }
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
                //Sub Process
                if (cmbProcess.Text != "")
                {
                    //int PId = Convert.ToInt32(cmbProcess.SelectedValue);
                    //var pStatus = (from m in db.Forging_ProcessMasters where m.Company_ID == logIn.company && m.Main_Process_ID == PId select new { m.ID, m.Process_Name }).Distinct().ToList();
                    //if (pStatus.Count > 0)
                    //{
                    //    cmbSubProcess.DataSource = pStatus;
                    //    cmbSubProcess.ValueMember = "ID";
                    //    cmbSubProcess.DisplayMember = "Process_Name";
                    //}
                    //else
                    //{
                    //    cmbSubProcess.DataSource = null;
                    //}

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
                //Check for mandatory fields
                if(cmbProcess.Text =="")
                {
                    MessageBox.Show("Select Process From the List");
                    cmbProcess.Focus();
                    return;
                }               

                if (txtJobCardNo.Text == "")
                {
                    MessageBox.Show("Job Card No Cannot Be Blank");
                    txtJobCardNo.Focus();
                    return;
                }

                if (cmbDrawingNo.Text == "")
                {
                    MessageBox.Show("Select Drawing From the List");
                    cmbDrawingNo.Focus();
                    return;
                }
                if (cmbMachineID.Text == "")
                {
                    MessageBox.Show("Select Machine From the List");
                    cmbMachineID.Focus();
                    return;
                }
                if (cmbOperator.Text == "")
                {
                    MessageBox.Show("Operator Name Cannot Be Blank");
                    cmbOperator.Focus();
                    return;
                }
                if (cmbSupervisor.Text == "")
                {
                    MessageBox.Show("Supervisor Name Cannot Be Blank");
                    cmbSupervisor.Focus();
                    return;
                }


                String myString = "";
                myString = txtvchno.Text;
                if ((from a in db.Forging_Machining_Masters where a.Company_ID == logIn.company && a.Report_No == txtvchno.Text select a).Count() > 0)
                {
                    myString = txtvchno.Text;
                    db.Sp_delete_Production_Machining(logIn.company, txtvchno.Text);

                    Forging_Machining_Master pb = new Forging_Machining_Master();
                    pb.Report_No = txtvchno.Text;
                    pb.Report_Date = dpdate.Value;
                    pb.Shift = cmbShift.Text;
                   // pb.Working_Hrs = (txtWorkingHrs.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtWorkingHrs.Text);

                    pb.Process_Name = Convert.ToInt32(cmbProcess.SelectedValue);
                   // pb.Sub_Process_Name = Convert.ToInt32(cmbProcess.SelectedValue);
                    pb.Machine_name = cmbMachineID.Text;
                    pb.Job_CardNo = txtJobCardNo.Text;
                    pb.FG_ID = Convert.ToInt32(txtFGCode.Text);
                    pb.FG_Item_Name = txtItemDescr.Text;
                    pb.Machine_Drawing_NO = cmbDrawingNo.Text;
                    pb.R_Material = txtRawMaterial.Text;
                    pb.RM_Wt = Convert.ToDecimal(txtRMWeight.Text);
                    pb.Oprn_Wt = Convert.ToDecimal(txtOprnWt.Text);
                    pb.Prod_Cycle = txtMachineCycle.Text;
                    pb.Prod_Rate = Convert.ToDecimal(txtPR.Text);
                    pb.Planning_Qty = Convert.ToDecimal(txtPlannedQty.Text);
                    pb.JC_Qty = Convert.ToDecimal(txtJobCardQty.Text);
                    pb.Available_Qty = Convert.ToDecimal(txtAvailableQty.Text);
                    // pb.StartTime = (txtStartTime.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtStartTime.Text);
                    // pb.EndTime = (txtEdnTime.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtEdnTime.Text);
                    pb.Working_Hrs = (txtAllocatedHrs.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtAllocatedHrs.Text);
                    //pb = (txtAllocatedHrs.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtAllocatedHrs.Text);
                    pb.Ideal_Hrs = (txtIdealHrs.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtIdealHrs.Text);
                    pb.Die_Setting_Time = (txtDieSetting.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtDieSetting.Text);
                    pb.Power_Cut = (txtPowerCut.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtPowerCut.Text);
                    pb.BreakDown_Hrs = (txtBreakDown.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtBreakDown.Text);
                    pb.Scrap_Cleaning = (txtScrapCleaning.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtScrapCleaning.Text);
                    pb.Man_Power = (txtManPower.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtManPower.Text);
                    pb.Other_Hrs = (txtOthers.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtOthers.Text);
                    pb.Insert_Change = (txtInsertChange.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtInsertChange.Text);
                    pb.IPH_Eff_Per = (txtIPHEff.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtIPHEff.Text);

                    pb.Others_Reason = (txtOthersReason.Text == null) ? "" : txtOthersReason.Text;
                    pb.Operator = (cmbOperator.Text == null) ? "" : cmbOperator.Text;
                    pb.Supervisor = (cmbSupervisor.Text == null) ? "" : cmbSupervisor.Text;
                    pb.Remarks = (txtRemarks.Text == null) ? "" : txtRemarks.Text;
                    pb.Company_ID = logIn.company;
                    pb.Modified_By = logIn.username + "-" + DateTime.Now;
                    pb.Tot_Loss_Hrs_Line = (txtTotalLossHrs.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtTotalLossHrs.Text);
                    pb.Tot_Loss_Hrs = (txtTotalHrs.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtTotalHrs.Text);
                    db.Forging_Machining_Masters.InsertOnSubmit(pb);
                    db.SubmitChanges();

                    //Save Child Data (Job Card)
                    for (int i = 0; i < dgJobCardData.Rows.Count - 1; i++)
                    {
                        Forging_Machining_Child SC = new Forging_Machining_Child();
                        var d1 = (from a in db.Forging_Machining_Masters where a.Report_No == myString && a.Company_ID == logIn.company select new { a.id }).ToList();
                        SC.Report_Master_ID = d1[0].id;
                        SC.HT_BatchNo = (dgJobCardData.Rows[i].Cells["HT_BatchNo"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["HT_BatchNo"].Value).ToString();

                       
                        SC.Batch_Qty = (dgJobCardData.Rows[i].Cells["Batch_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Batch_Qty"].Value);
                        SC.Serial_Start = (dgJobCardData.Rows[i].Cells["Serial_Start"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Serial_Start"].Value);
                        SC.Serial_End = (dgJobCardData.Rows[i].Cells["Serial_End"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Serial_End"].Value);
                        SC.Qty_Received = (dgJobCardData.Rows[i].Cells["Qty_Received"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Qty_Received"].Value);
                        SC.Heat_Code = (dgJobCardData.Rows[i].Cells["Heat_Code"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Heat_Code"].Value).ToString();
                        SC.Qty_Produced = (dgJobCardData.Rows[i].Cells["Qty_Produced"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Qty_Produced"].Value);
                        SC.Rejected_Qty = (dgJobCardData.Rows[i].Cells["Rejected_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Rejected_Qty"].Value);
                        SC.Rework_Qty = (dgJobCardData.Rows[i].Cells["Rework_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Rework_Qty"].Value);
                        SC.Working_Hrs = (dgJobCardData.Rows[i].Cells["Working_Hrs"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Working_Hrs"].Value);
                        SC.Loss_Mins = (dgJobCardData.Rows[i].Cells["Loss_Mins"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Loss_Mins"].Value);
                        SC.Ideal_Hrs = (dgJobCardData.Rows[i].Cells["Ideal_Hrs"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Ideal_Hrs"].Value);
                        SC.IPH_Eff = (dgJobCardData.Rows[i].Cells["IPH_Eff"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["IPH_Eff"].Value);

                        SC.Line_Remarks = (dgJobCardData.Rows[i].Cells["Line_Remarks"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Line_Remarks"].Value).ToString();


                        db.Forging_Machining_Childs.InsertOnSubmit(SC);
                    }
                    db.SubmitChanges();

                    //Save Insert Data
                    for (int i = 0; i < grdInsertData.Rows.Count - 1; i++)
                    {
                        Forging_Machining_InsertData SC = new Forging_Machining_InsertData();
                        var d1 = (from a in db.Forging_Machining_Masters where a.Report_No == myString && a.Company_ID == logIn.company select new { a.id }).ToList();
                        SC.Report_Master_ID = d1[0].id;
                        SC.E_Time = (grdInsertData.Rows[i].Cells["E_Time"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(grdInsertData.Rows[i].Cells["E_Time"].Value);


                        SC.Insert_Type = (grdInsertData.Rows[i].Cells["Insert_Type"].Value == null) ? "" : (grdInsertData.Rows[i].Cells["Insert_Type"].Value).ToString();
                        SC.Insert_No = (grdInsertData.Rows[i].Cells["Insert_No"].Value == null) ? "" : (grdInsertData.Rows[i].Cells["Insert_No"].Value).ToString();
                        SC.Tool_Station = (grdInsertData.Rows[i].Cells["Tool_Station"].Value == null) ? "" : (grdInsertData.Rows[i].Cells["Tool_Station"].Value).ToString();
                        SC.Insert_Corner = (grdInsertData.Rows[i].Cells["Insert_Corner"].Value == null) ? "" : (grdInsertData.Rows[i].Cells["Insert_Corner"].Value).ToString();
                        //SC.Insert_Lify_Qty = (grdInsertData.Rows[i].Cells["Insert_Lify_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(grdInsertData.Rows[i].Cells["Insert_Lify_Qty"].Value);


                        db.Forging_Machining_InsertDatas.InsertOnSubmit(SC);
                    }
                    db.SubmitChanges();


                    MessageBox.Show("Record Updated Sucessfully");
                    clear();
                    return;
                }
                else
                {
                    AutoincrementId();
                    Forging_Machining_Master pb = new Forging_Machining_Master();
                    pb.Report_No = txtvchno.Text;
                    pb.Report_Date = dpdate.Value;
                    pb.Shift = cmbShift.Text;
                    // pb.Working_Hrs = (txtWorkingHrs.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtWorkingHrs.Text);

                    pb.Process_Name = Convert.ToInt32(cmbProcess.SelectedValue);
                    // pb.Sub_Process_Name = Convert.ToInt32(cmbProcess.SelectedValue);
                    pb.Machine_name = cmbMachineID.Text;
                    pb.Job_CardNo = txtJobCardNo.Text;
                    pb.FG_ID = Convert.ToInt32(txtFGCode.Text);
                    pb.FG_Item_Name = txtItemDescr.Text;
                    pb.Machine_Drawing_NO = cmbDrawingNo.Text;
                    pb.R_Material = txtRawMaterial.Text;
                    pb.RM_Wt = Convert.ToDecimal(txtRMWeight.Text);
                    pb.Oprn_Wt = Convert.ToDecimal(txtOprnWt.Text);
                    pb.Prod_Cycle = txtMachineCycle.Text;
                    pb.Prod_Rate = Convert.ToDecimal(txtPR.Text);
                    pb.Planning_Qty = Convert.ToDecimal(txtPlannedQty.Text);
                    pb.JC_Qty = Convert.ToDecimal(txtJobCardQty.Text);
                    pb.Available_Qty = Convert.ToDecimal(txtAvailableQty.Text);
                    // pb.StartTime = (txtStartTime.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtStartTime.Text);
                    // pb.EndTime = (txtEdnTime.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtEdnTime.Text);
                    pb.Working_Hrs = (txtAllocatedHrs.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtAllocatedHrs.Text);
                    //pb = (txtAllocatedHrs.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtAllocatedHrs.Text);
                    pb.Ideal_Hrs = (txtIdealHrs.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtIdealHrs.Text);
                    pb.Die_Setting_Time = (txtDieSetting.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtDieSetting.Text);
                    pb.Power_Cut = (txtPowerCut.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtPowerCut.Text);
                    pb.BreakDown_Hrs = (txtBreakDown.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtBreakDown.Text);
                    pb.Scrap_Cleaning = (txtScrapCleaning.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtScrapCleaning.Text);
                    pb.Man_Power = (txtManPower.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtManPower.Text);
                    pb.Other_Hrs = (txtOthers.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtOthers.Text);
                    pb.Insert_Change = (txtInsertChange.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtInsertChange.Text);
                    pb.IPH_Eff_Per = (txtIPHEff.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtIPHEff.Text);

                    pb.Others_Reason = (txtOthersReason.Text == null) ? "" : txtOthersReason.Text;
                    pb.Operator = (cmbOperator.Text == null) ? "" : cmbOperator.Text;
                    pb.Supervisor = (cmbSupervisor.Text == null) ? "" : cmbSupervisor.Text;
                    pb.Remarks = (txtRemarks.Text == null) ? "" : txtRemarks.Text;
                    pb.Company_ID = logIn.company;
                    pb.Modified_By = logIn.username + "-" + DateTime.Now;
                    db.Forging_Machining_Masters.InsertOnSubmit(pb);
                    db.SubmitChanges();

                    //Save Child Data (Job Card)
                    for (int i = 0; i < dgJobCardData.Rows.Count - 1; i++)
                    {
                        Forging_Machining_Child SC = new Forging_Machining_Child();
                        var d1 = (from a in db.Forging_Machining_Masters where a.Report_No == myString && a.Company_ID == logIn.company select new { a.id }).ToList();
                        SC.Report_Master_ID = d1[0].id;
                        SC.HT_BatchNo = (dgJobCardData.Rows[i].Cells["HT_BatchNo"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["HT_BatchNo"].Value).ToString();


                        SC.Batch_Qty = (dgJobCardData.Rows[i].Cells["Batch_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Batch_Qty"].Value);
                        SC.Serial_Start = (dgJobCardData.Rows[i].Cells["Serial_Start"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Serial_Start"].Value);
                        SC.Serial_End = (dgJobCardData.Rows[i].Cells["Serial_End"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Serial_End"].Value);
                        SC.Qty_Received = (dgJobCardData.Rows[i].Cells["Qty_Received"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Qty_Received"].Value);
                        SC.Heat_Code = (dgJobCardData.Rows[i].Cells["Heat_Code"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Heat_Code"].Value).ToString();
                        SC.Qty_Produced = (dgJobCardData.Rows[i].Cells["Qty_Produced"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Qty_Produced"].Value);
                        SC.Rejected_Qty = (dgJobCardData.Rows[i].Cells["Rejected_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Rejected_Qty"].Value);
                        SC.Rework_Qty = (dgJobCardData.Rows[i].Cells["Rework_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Rework_Qty"].Value);
                        SC.Working_Hrs = (dgJobCardData.Rows[i].Cells["Working_Hrs"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Working_Hrs"].Value);
                        SC.Loss_Mins = (dgJobCardData.Rows[i].Cells["Loss_Mins"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Loss_Mins"].Value);
                        SC.Ideal_Hrs = (dgJobCardData.Rows[i].Cells["Ideal_Hrs"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Ideal_Hrs"].Value);
                        SC.IPH_Eff = (dgJobCardData.Rows[i].Cells["IPH_Eff"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["IPH_Eff"].Value);

                        SC.Line_Remarks = (dgJobCardData.Rows[i].Cells["Line_Remarks"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Line_Remarks"].Value).ToString();


                        db.Forging_Machining_Childs.InsertOnSubmit(SC);
                    }
                    db.SubmitChanges();

                    //Save Insert Data
                    for (int i = 0; i < grdInsertData.Rows.Count - 1; i++)
                    {
                        Forging_Machining_InsertData SC = new Forging_Machining_InsertData();
                        var d1 = (from a in db.Forging_Machining_Masters where a.Report_No == myString && a.Company_ID == logIn.company select new { a.id }).ToList();
                        SC.Report_Master_ID = d1[0].id;
                        SC.E_Time = (grdInsertData.Rows[i].Cells["E_Time"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(grdInsertData.Rows[i].Cells["E_Time"].Value);
                        SC.Insert_Type = (grdInsertData.Rows[i].Cells["Insert_Type"].Value == null) ? "" : (grdInsertData.Rows[i].Cells["Insert_Type"].Value).ToString();
                        SC.Insert_No = (grdInsertData.Rows[i].Cells["Insert_No"].Value == null) ? "" : (grdInsertData.Rows[i].Cells["Insert_No"].Value).ToString();
                        SC.Tool_Station = (grdInsertData.Rows[i].Cells["Tool_Station"].Value == null) ? "" : (grdInsertData.Rows[i].Cells["Tool_Station"].Value).ToString();
                        SC.Insert_Corner = (grdInsertData.Rows[i].Cells["Insert_Corner"].Value == null) ? "" : (grdInsertData.Rows[i].Cells["Insert_Corner"].Value).ToString();
                        //SC.Insert_Lify_Qty = (grdInsertData.Rows[i].Cells["Insert_Lify_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(grdInsertData.Rows[i].Cells["Insert_Lify_Qty"].Value);


                        db.Forging_Machining_InsertDatas.InsertOnSubmit(SC);
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
            cmbShift.Text = "";
            cmbMachineID.SelectedValue = -1;
            cmbProcess.SelectedValue = -1;
            txtJobCardNo.Text = "";
            txtItemDescr.Text = "";
            cmbDrawingNo.Text = "";
            txtOthersReason.Text = "";
            cmbSupervisor.Text = "";
            txtRemarks.Text = "";           
            txtRawMaterial.Text = "";
            txtRMWeight.Text = "";
            txtOprnWt.Text = "";
            txtFGCode.Text = "";
            txtMachineCycle.Text = "";
            txtPR.Text = "";
            txtJobCardQty.Text = "";
            txtAvailableQty.Text = "";
            txtPlannedQty.Text = "";
            txtAllocatedHrs.Text = "";
            txtIdealHrs.Text = "";
            txtDieSetting.Text = "";            
            txtPowerCut.Text = "";
            txtBreakDown.Text = "";
            txtScrapCleaning.Text = "";
            txtManPower.Text = "";
            txtOthers.Text = "";
            txtManPower.Text = "";
            txtIPHEff.Text = "";
            txtInsertChange.Text = "";
            txtTotalLossHrs.Text = "";
            txtTotalHrs.Text = "";
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

            if (grdInsertData.Rows.Count >= 1)
            {
                for (int i = 0; i < grdInsertData.Rows.Count - 1; i++)
                {
                    grdInsertData.Rows.RemoveAt(i);
                    i--;
                    while (grdInsertData.Rows.Count == 0)
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
                ProductionManagement.frmForging_Production_MachiningList obj = new frmForging_Production_MachiningList();
                if (obj.ShowDialog() == DialogResult.OK)
                {
                    int Report_ID = 0;
                    txtvchno.Text = ProductionManagement.frmForging_Production_MachiningList.Voucherno;
                    var sa = (from sq in db.Forging_Machining_Masters
                              where sq.Company_ID == logIn.company && sq.Report_No == txtvchno.Text
                              select new
                              {
                                  sq.id,
                                  sq.Report_Date,
                                  sq.Shift,
                                  sq.Machine_name,
                                  sq.Job_CardNo,
                                  sq.FG_Item_Name,
                                  sq.FG_ID,
                                  sq.Machine_Drawing_NO,
                                  sq.R_Material,
                                  sq.RM_Wt,
                                  sq.Oprn_Wt,
                                  sq.Prod_Cycle,
                                  sq.Prod_Rate,
                                  sq.JC_Qty,
                                  sq.Available_Qty,
                                  sq.Planning_Qty,                                  
                                  sq.Others_Reason,
                                  sq.Supervisor,
                                  sq.Operator,
                                  sq.Remarks,
                                  sq.Created_By,
                                  sq.Modified_By,
                                  sq.Working_Hrs,                                  
                                  sq.Allocated_Hrs,
                                  sq.Ideal_Hrs,                                  
                                  sq.Scrap_Cleaning,
                                  sq.Power_Cut,
                                  sq.Insert_Change,
                                  sq.Process_Name,
                                  sq.Man_Power,
                                  sq.Other_Hrs,
                                  sq.Die_Setting_Time,
                                  sq.BreakDown_Hrs,
                                  sq.No_Reason_Hrs,
                                  sq.IPH_Eff_Per,
                                  sq.Tot_Loss_Hrs,
                                  sq.Tot_Loss_Hrs_Line
                                 
                              }).ToList();
                    if (sa.Count > 0)
                    {
                        Report_ID = sa[0].id;
                        dpdate.Value = Convert.ToDateTime(sa[0].Report_Date);
                        cmbMachineID.Text = sa[0].Machine_name;
                        txtJobCardNo.Text = sa[0].Job_CardNo;
                        txtItemDescr.Text = sa[0].FG_Item_Name;
                        txtFGCode.Text = sa[0].FG_ID.ToString();
                        cmbDrawingNo.Text = sa[0].Machine_Drawing_NO;
                        txtRawMaterial.Text = sa[0].R_Material;
                        txtRMWeight.Text = sa[0].RM_Wt.ToString();
                        txtOprnWt.Text = sa[0].Oprn_Wt.ToString();
                        txtJobCardQty.Text = sa[0].JC_Qty.ToString();
                        txtAvailableQty.Text = sa[0].Available_Qty.ToString();
                        txtPlannedQty.Text = sa[0].Planning_Qty.ToString();
                        txtPR.Text = sa[0].Prod_Rate.ToString();
                        txtMachineCycle.Text = sa[0].Prod_Cycle.ToString();
                        cmbShift.Text = sa[0].Shift;
                        txtOthersReason.Text = sa[0].Others_Reason; 
                        cmbSupervisor.Text = sa[0].Supervisor;
                        txtRemarks.Text = sa[0].Remarks.ToString();
                        lblCreatedBy.Text = sa[0].Created_By;
                        lblModified.Text = sa[0].Modified_By;
                        txtAllocatedHrs.Text = sa[0].Working_Hrs.ToString();
                        cmbProcess.SelectedValue = sa[0].Process_Name;
                        cmbProcess_Leave(sender, e);                      
                        txtIdealHrs.Text = sa[0].Ideal_Hrs.ToString();
                        txtDieSetting.Text = sa[0].Die_Setting_Time.ToString();                        
                        txtPowerCut.Text = sa[0].Power_Cut.ToString();
                        txtBreakDown.Text = sa[0].BreakDown_Hrs.ToString();
                        txtScrapCleaning.Text = sa[0].Scrap_Cleaning.ToString();
                        txtManPower.Text = sa[0].Man_Power.ToString();
                        txtOthers.Text = sa[0].Other_Hrs.ToString();
                        txtManPower.Text = sa[0].Man_Power.ToString();
                        txtInsertChange.Text = sa[0].Insert_Change.ToString();
                        txtIPHEff.Text = sa[0].IPH_Eff_Per.ToString();
                        cmbOperator.Text = sa[0].Operator;
                        txtTotalLossHrs.Text = sa[0].Tot_Loss_Hrs_Line.ToString();
                        txtTotalHrs.Text = sa[0].Tot_Loss_Hrs.ToString();
                        lblCreatedBy.Text = sa[0].Created_By;
                        lblModified.Text = sa[0].Modified_By;




                    }
                    var ca = (from sq in db.Forging_Machining_Childs
                              where sq.Report_Master_ID == Report_ID
                              select new
                              {

                                 sq.HT_BatchNo,
                                  sq.Batch_Qty,
                                  Pending_qty = sq.Batch_Qty,
                                  sq.Serial_Start,
                                  sq.Serial_End,
                                  sq.Qty_Received,
                                  sq.Qty_Produced,
                                  sq.Heat_Code,
                                  sq.Rejected_Qty,
                                  sq.Rework_Qty,
                                  sq.Working_Hrs,
                                  sq.Loss_Mins,
                                  sq.Ideal_Hrs,
                                  sq.IPH_Eff,                                                                  
                                  sq.Line_Remarks
                              });
                    SqlCommand cmd3 = (SqlCommand)db.GetCommand(ca);
                    SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                    DataTable dt1 = new DataTable();
                    da3.Fill(dt1);
                    if (dt1.Rows.Count > 0)
                        dgJobCardData.DataSource = dt1;


                    var ca1 = (from sq in db.Forging_Machining_InsertDatas
                              where sq.Report_Master_ID == Report_ID
                              select new
                              {

                                  sq.E_Time,
                                  sq.Insert_Type,                                 
                                  sq.Insert_No,
                                  sq.Tool_Station,
                                  sq.Insert_Corner
                              });
                    SqlCommand cmd4 = (SqlCommand)db.GetCommand(ca1);
                    SqlDataAdapter da4 = new SqlDataAdapter(cmd4);
                    DataTable dt2 = new DataTable();
                    da4.Fill(dt2);
                    if (dt2.Rows.Count > 0)
                        grdInsertData.DataSource = dt2;

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
            ReportNo = txtvchno.Text;
            Operation = "Machining";

            ProductionManagement.Transactions.frmBreakDownData obj = new ProductionManagement.Transactions.frmBreakDownData();
            obj.ShowDialog();

            // groupBox1.Visible = true;
        }

        private void txtStartTime_Leave(object sender, EventArgs e)
        {

        }
        public void ClaculateWHrs()
        {
            decimal DieSetting = (txtDieSetting.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtDieSetting.Text);
           // decimal LunchTime = (txtLunchTime.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtLunchTime.Text);
            decimal PowerCut = (txtPowerCut.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtPowerCut.Text);
            decimal BreakDown = (txtBreakDown.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtBreakDown.Text);
           // decimal NoPlan = (txtScrapCleaning.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtScrapCleaning.Text);
            decimal ManPower = (txtManPower.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtManPower.Text);
            decimal Others = (txtOthers.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtOthers.Text);
            decimal ScrapClean = (txtScrapCleaning.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtScrapCleaning.Text);
            decimal InsertChange = (txtInsertChange.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtInsertChange.Text);


            decimal TotHrs =DieSetting+PowerCut+BreakDown+ManPower+Others+ ScrapClean+ InsertChange;
           txtTotalHrs.Text = (TotHrs/60).ToString("0.00");
            //decimal WHrs = (txtAllocatedHrs.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtAllocatedHrs.Text);

           // decimal TotAvblHrs = WHrs - (TotHrs / 60);
            //txtAllocatedHrs.Text = TotAvblHrs.ToString("0.00");
          //  decimal IPHrs = Convert.ToDecimal(txtIdealHrs.Text);
            //decimal NoReasonHrs = (TotAvblHrs - IPHrs);
           // txtInsertChange.Text = NoReasonHrs.ToString("0.00");
            //if (WHrs > 0)
            //{
            //    decimal IPHEff = IPHrs / WHrs * 100;
            //    txtIPHEff.Text = IPHEff.ToString("0.00");
            //}
            
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
                db.Sp_delete_Production_Machining(logIn.company, txtvchno.Text);
                MessageBox.Show("Record Deleted Successfully");
                clear();
            }
        }

        private void txtNoReasonHrs_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtIPHEff_TextChanged(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtJobCardNo_Leave(object sender, EventArgs e)
        {
            try
            {
                
                Boolean GetProcess = false;
                if (txtJobCardNo.Text != "")
                {
                    int PID = Convert.ToInt32(cmbProcess.SelectedValue);
                    //int PrevProcessID = 0;
                    while (GetProcess == false)
                    {
                        var ProcID = (from s in db.Forging_ProcessMasters
                                      where s.ID == PID
                                      select new { s.PreDessor_Process }).ToList();

                        var ProcID1 = (from s in db.GetPreviousProcess(txtJobCardNo.Text, ProcID[0].PreDessor_Process)
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

                    var getDetails = (from s in db.sp_Forging_JopbCard_Process_Stage(txtJobCardNo.Text, Convert.ToInt32(cmbProcess.SelectedValue), PrevProcessID)
                                      select new { s.J_Date, s.FG_Item_Code, s.FG_Item_Name, s.Job_CardQty, s.Heat_Code, s.Qty_Avalable, s.Qty_Pending, s.Qty_Accepted,s.FG__Code }).FirstOrDefault();

                    if (getDetails != null)
                    {
                       // R1.Cells["Job_card_date"].Value = getDetails.J_Date.ToString();
                        txtItemDescr.Text = getDetails.FG_Item_Code.ToString();
                        txtFGCode.Text = getDetails.FG__Code.ToString();
                        //R1.Cells["Item_Name"].Value = getDetails.FG_Item_Name.ToString();
                        //R1.Cells["Heat_Code"].Value = getDetails.Heat_Code.ToString();
                        txtJobCardQty.Text = getDetails.Job_CardQty.ToString();
                        txtAvailableQty.Text = getDetails.Qty_Pending.ToString();
                        //R1.Cells["RM_Sec"].Value = getDetails.RM_Name.ToString();

                        var getRMData = (from s in db.Forging_Finished_Goods where s.prod_ID == Convert.ToInt32(txtFGCode.Text)
                                          select new { s.Raw_Material, s.Input_Weight, s.Machining_Weight }).FirstOrDefault();

                        if (getRMData != null)
                        {
                            txtRawMaterial.Text = getRMData.Raw_Material.ToString();
                            txtRMWeight.Text = getRMData.Input_Weight.ToString();
                            txtOprnWt.Text = getRMData.Machining_Weight.ToString();
                        }

                        var pStatus = (from m in db.Forging_JobCard_MachiningPlans where m.Job_CardNo == txtJobCardNo.Text select new { m.Machine_Drawing_No }).Distinct().ToList();
                        if (pStatus.Count > 0)
                        {
                            cmbDrawingNo.DataSource = pStatus;
                            cmbDrawingNo.ValueMember = "Machine_Drawing_No";
                            cmbDrawingNo.DisplayMember = "Machine_Drawing_No";
                            cmbDrawingNo.SelectedIndex = -1;

                        }
                    }
                    else
                    {
                        MessageBox.Show("Invalid Job Card Entered");
                        return;
                    }
                    var getPR = (from s in db.Forging_FinishedGoods_PRs
                                 where s.Process_ID == Convert.ToInt32(cmbProcess.SelectedValue) && s.FG_Code == txtFGCode.Text
                                 select new { s.PR }).FirstOrDefault();

                    if (getPR != null)
                    {
                       txtPR.Text = getPR.PR.ToString();
                    }
                    else
                    {
                        txtPR.Text = "0";
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void comboBox1_Leave(object sender, EventArgs e)
        {
            if (cmbDrawingNo.Text != "")
            {
                //decimal getRMData = (from s in db.Forging_JobCard_MachiningPlans
                //                 where s.Job_CardNo == txtJobCardNo.Text && s.Machine_Drawing_No == comboBox1.Text

                //                 select s.Machine_Qty).Sum();

                var TotQty = (from s in db.Forging_JobCard_MachiningPlans
                              where s.Job_CardNo == txtJobCardNo.Text && s.Machine_Drawing_No == cmbDrawingNo.Text
                              select s).Sum(x => x.Machine_Qty);

                //if (getRMData != null)
                //{
                    txtPlannedQty.Text = TotQty.ToString();                    
                //}
            }
        }

        private void grdInsertData_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            int columnIndex = grdInsertData.CurrentCell.ColumnIndex;
            string columnName = grdInsertData.Columns[columnIndex].HeaderText;
            TextBox tb3 = e.Control as TextBox;
            if (tb3 != null && columnName == "Intert Type")
            {
                tb3.AutoCompleteMode = AutoCompleteMode.Suggest;
                tb3.AutoCompleteSource = AutoCompleteSource.CustomSource;
                AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                addItems1(DataColl);
                tb3.AutoCompleteCustomSource = DataColl;
            }
        }
        public void addItems1(AutoCompleteStringCollection coll)
        {
            try
            {
                DataGridViewRow R1 = grdInsertData.Rows[grdInsertData.CurrentRow.Index];

                int columnIndex = grdInsertData.CurrentCell.ColumnIndex;
                string columnName = grdInsertData.Columns[columnIndex].HeaderText;

                
                if (columnName == "Intert Type")
                {
                    //  var Prodname = (from d in db.Forging_JobCardRMs where d.Job_CardNo == R1.Cells["Job_Card"].Value.ToString() select new { d.Heat_Code }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Intert Type");
                    dt.Rows.Add("CNMG");
                    dt.Rows.Add("DNMG");
                    dt.Rows.Add("CCMT");

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

        private void txtJobCardNo_Enter(object sender, EventArgs e)
        {
            try
            {

                AutoCompleteStringCollection DataColl = new AutoCompleteStringCollection();
                AddJC(DataColl);
                txtJobCardNo.AutoCompleteCustomSource = DataColl;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void txtPR_Leave(object sender, EventArgs e)
        {
            try
            {
                DialogResult result = MessageBox.Show("It Seems You Have Changed The PR, Do You Want To Update the Master Data?", "Update Confirmation", MessageBoxButtons.YesNo);
                if (result == DialogResult.Yes)
                {
                    var getDetails = (from s in db.sp_Forging_JopbCard_Process_Stage(txtJobCardNo.Text, Convert.ToInt32(cmbProcess.SelectedValue), PrevProcessID)
                                        select new { s.J_Date, s.FG_Item_Code, s.FG_Item_Name, s.Job_CardQty, s.Heat_Code, s.Qty_Avalable, s.Qty_Pending, s.Qty_Accepted, s.FG__Code }).FirstOrDefault();

                    if (getDetails != null)
                    {
                        int productCode = Convert.ToInt32(getDetails.FG__Code.ToString());
                        var deleteproduct = db.Forging_FinishedGoods_PRs.Single(course => course.FG_Code == getDetails.FG__Code.ToString() && course.Process_ID == Convert.ToInt32(cmbProcess.SelectedValue));
                        deleteproduct.PR = Convert.ToDecimal(txtPR.Text);
                    }
                    db.SubmitChanges();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void txtOprnWt_Leave(object sender, EventArgs e)
        {
          

        }

        public void AddJC(AutoCompleteStringCollection coll)
        {
            try
            {

                var Prodname = (from d in db.sp_Forging_Get_JObcard_machining(cmbProcess.Text)
                                

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
            catch (Exception ex)
            {
                ex.Message.ToString();
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
