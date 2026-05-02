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
using System.Globalization;
using Ione_DAL;
namespace ioneNet.ProductionManagement.Transactions
{
    public partial class frmForge_Production_Forging : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public static string ReportNo, Operation;

        public frmForge_Production_Forging()
        {
            InitializeComponent();
        }

        private void frmForge_Production_Forging_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

            AutoincrementId();
            //var Sup = (from k in db.Forging_ForgingReport_Masters select new { k.Supervisor }).Distinct().ToList();
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

            //Main Process

            var pStatus = (from m in db.Forging_ProcessMasters where m.Company_ID == logIn.company && m.Main_Process_ID == 6 || m.Process_Name == "Forging" select new { m.ID, m.Process_Name }).Distinct().ToList();
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

            var EmpName = (from m in db.Forging_Employees where m.Company_ID == logIn.company && m.Designation == "Supervisor" && m.Department == "Forging" select new { m.id, m.Employee_Name }).Distinct().ToList();
            if (EmpName.Count > 0)
            {
                cmbSupervisor.DataSource = EmpName;
                cmbSupervisor.ValueMember = "id";
                cmbSupervisor.DisplayMember = "Employee_Name";
                ////if (cmbProcess.Items.Count > 0)
                ////{
                cmbSupervisor.SelectedIndex = -1;
                //    }
                //    else
                //    {
                //        cmbProcess.SelectedIndex = -1;
                //    }
            }

        }
        public void AutoincrementId()
        {
            try
            {

                var result = db.Sp_autoincrement_Forging_ForgingReport(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date);
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
                int PrevProcessID = 0;
                if (columnName == "Job_Card" && R1.Cells["Job_Card"].Value != null)
                {

                    //Check for basic inputs
                    if (cmbProcess.Text != "")
                    {
                        if (txtWorkingHrs.Text != "")
                        {

                        }
                        else
                        {
                            MessageBox.Show("Enter Working Hrs To Proceed");
                            txtWorkingHrs.Focus();
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Select Process To Proceed");
                        cmbProcess.Focus();
                        return;
                    }

                    //Get Previous Process ID

                    Boolean GetProcess = false;
                    int PID = Convert.ToInt32(cmbProcess.SelectedValue);
                    //int PrevProcessID = 0;
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

                    var getDetails = (from s in db.sp_Forging_JopbCard_Process_Stage(R1.Cells["Job_Card"].Value.ToString(), Convert.ToInt32(cmbProcess.SelectedValue), PrevProcessID)
                                      select new { s.J_Date, s.FG_Item_Code, s.FG_Item_Name, s.Job_CardQty, s.Heat_Code, s.Qty_Avalable, s.Qty_Pending, s.Qty_Accepted, s.FG__Code}).FirstOrDefault();

                    if (getDetails != null)
                    {
                        R1.Cells["Job_card_date"].Value = getDetails.J_Date.ToString();
                        R1.Cells["Item_Code"].Value = getDetails.FG_Item_Code.ToString();
                        R1.Cells["Item_Name"].Value = getDetails.FG_Item_Name.ToString();
                        R1.Cells["Heat_Code"].Value = getDetails.Heat_Code.ToString();
                        R1.Cells["Job_Card_Qty"].Value = getDetails.Job_CardQty.ToString();
                        R1.Cells["Pending_Qty"].Value = getDetails.Qty_Pending.ToString();
                        //R1.Cells["RM_Sec"].Value = getDetails.RM_Name.ToString();

                    }
                    else
                    {
                        MessageBox.Show("Invalid Job Card Entered");
                        return;
                    }
                    var getPR = (from s in db.Forge_Get_FG_PR(logIn.company, Convert.ToInt32(cmbProcess.SelectedValue), getDetails.FG__Code.ToString())                                 
                                 select new { s.PR }).FirstOrDefault();

                    if (getPR != null && Convert.ToDecimal(getPR.PR) != 0)
                    {

                        R1.Cells["PR"].Value = getPR.PR.ToString();
                    }
                    else
                    {
                       // MessageBox.Show("PR Not Defined For the Selected Item");
                        
                        R1.Cells["PR"].Value = "0";
                    }

                }
                if (columnName == "PR")
                {
                    DialogResult result = MessageBox.Show("It Seems You Have Changed The PR, Do You Want To Update the Master Data?", "Update Confirmation", MessageBoxButtons.YesNo);
                    if (result == DialogResult.Yes)
                    {
                        var getDetails = (from s in db.sp_Forging_JopbCard_Process_Stage(R1.Cells["Job_Card"].Value.ToString(), Convert.ToInt32(cmbProcess.SelectedValue), PrevProcessID)
                                          select new { s.J_Date, s.FG_Item_Code, s.FG_Item_Name, s.Job_CardQty, s.Heat_Code, s.Qty_Avalable, s.Qty_Pending, s.Qty_Accepted, s.FG__Code }).FirstOrDefault();

                        if (getDetails != null)
                        {
                            int productCode = Convert.ToInt32(getDetails.FG__Code.ToString());
                            var deleteproduct = db.Forging_FinishedGoods_PRs.Single(course => course.FG_Code == getDetails.FG__Code.ToString() && course.Process_ID == Convert.ToInt32(cmbProcess.SelectedValue));
                            deleteproduct.PR = Convert.ToDecimal(R1.Cells["PR"].Value);
                        }                    
                        db.SubmitChanges();                        
                    }
                }
                    if (columnName == "Qty_Produced" || columnName == "Qty_Rejected")
                {
                    //int Itemcode = Convert.ToInt32(R1.Cells["Item_Code"].Value.ToString());
                    if (R1.Cells["Job_Card"].Value != null)
                    {
                        decimal Qty_Produced = (R1.Cells["Qty_Produced"].Value == "" || R1.Cells["Qty_Produced"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty_Produced"].Value);
                        decimal Qty_Rejected = (R1.Cells["Qty_Rejected"].Value == "" || R1.Cells["Qty_Rejected"].Value == null || R1.Cells["Qty_Rejected"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty_Rejected"].Value);
                        decimal Qty_Pending = (R1.Cells["Pending_Qty"].Value == "" || R1.Cells["Pending_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Pending_Qty"].Value);


                        decimal Qty_Accepted = 0;
                        if (Qty_Produced <= Qty_Pending)
                        {
                            if (Qty_Rejected > Qty_Produced)
                            {
                                MessageBox.Show("Qty Rejected Cannot Be Greater Than Qty Produced");
                                R1.Cells["Qty_Rejected"].Value = 0;
                            }
                            else
                            {
                                Qty_Accepted = Qty_Produced - Qty_Rejected;
                                R1.Cells["Qty_Accepted"].Value = Qty_Accepted.ToString("0.00");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Qty Produced Cannot Be Greater Than Qty Pending");
                            R1.Cells["Qty_Produced"].Value = 0;
                            R1.Cells["Qty_Rejected"].Value = 0;
                        }


                        //decimal Qty_Accepted = Qty_Produced - Qty_Rejected;
                        //decimal StockQty = (R1.Cells["Stock_Qty"].Value == "" || R1.Cells["Stock_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Stock_Qty"].Value);
                        //decimal price = (R1.Cells["Price"].Value == "" || R1.Cells["Price"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Price"].Value);

                        //R1.Cells["Qty_Accepted"].Value = Qty_Accepted.ToString("0.00");
                        if (Convert.ToDecimal(R1.Cells["PR"].Value) > 0)
                        {
                            decimal Ideal_Hrs = Convert.ToDecimal(R1.Cells["Qty_Produced"].Value) / Convert.ToDecimal(R1.Cells["PR"].Value);
                            //PR

                            R1.Cells["Ideal_Hrs"].Value = Ideal_Hrs.ToString("0.00");
                        }

                        decimal x = 0;
                        for (int i = 0; i < dgJobCardData.Rows.Count - 1; i++)
                        {
                            x += (dgJobCardData.Rows[i].Cells["Ideal_Hrs"].Value.ToString() == "" || dgJobCardData.Rows[i].Cells["Ideal_Hrs"].Value == null || dgJobCardData.Rows[i].Cells["Ideal_Hrs"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Ideal_Hrs"].Value);

                        }
                        txtIdealHrs.Text = x.ToString();
                    }

                }
                if (columnName == "End_Time")
                {
                    //double x = 0, y = 0;
                    //decimal startTime = Convert.ToDecimal(R1.Cells["Start_Time"].Value);
                    //decimal EndTime = Convert.ToDecimal(R1.Cells["End_Time"].Value);
                    //x = EndTime - startTime;
                    if (R1.Cells["End_Time"].Value.ToString() != "")
                    {

                        
                        DateTime date1 = dpdate.Value;
                        DateTime date2 = date1.AddDays(1);
                        String t3 = date1.Date.ToString("dd/MM/yyyy");
                        string eDate = "";
                        string SDate = string.Concat(t3, " "+R1.Cells["Start_Time"].Value.ToString());
                        if(cmbShift.Text !="C")
                        {
                            eDate = string.Concat(t3, " " + R1.Cells["End_Time"].Value.ToString());
                        }
                        else
                        {
                            if (Convert.ToInt32(R1.Cells["End_Time"].Value.ToString()) < 22)
                            {
                                String t4 = date2.Date.ToString("dd/MM/yyyy");
                                eDate = string.Concat(t4, " " + R1.Cells["End_Time"].Value.ToString());
                            }
                            else
                            {
                                eDate = string.Concat(t3, " " + R1.Cells["End_Time"].Value.ToString());
                            }
                        }
                        //string eDate = date2.ToString();

                       
                        DateTime sTime = Convert.ToDateTime(SDate.ToString());
                        DateTime eTime = Convert.ToDateTime(eDate.ToString());
                        //DateTime dt1 = DateTime.ParseExact(sTime, "HH:mm", new DateTimeFormatInfo());
                        //DateTime dt2 = DateTime.ParseExact(eTime, "HH:mm", new DateTimeFormatInfo());
                        TimeSpan ts = eTime.Subtract(sTime);
                        R1.Cells["Actual_Hrs"].Value = ts;
                        //Console.WriteLine(eTime.Subtract(sTime).TotalMinutes);
                        //x = eTime.Subtract(sTime).TotalMinutes/60;
                    }
                    //DateTime a = new DateTime(2010, 05, 12, 13, 15, 00);
                    //DateTime b = new DateTime(2010, 05, 12, 13, 45, 00);
                    

                    



                }

                if (columnName == "Power_Closing_Reading")
                {
                    decimal x = 0;
                    decimal startTime = Convert.ToDecimal(R1.Cells["Power_Open_Reading"].Value);
                    decimal EndTime = Convert.ToDecimal(R1.Cells["Power_Closing_Reading"].Value);
                    x = EndTime - startTime;
                    R1.Cells["Total_Units"].Value = x.ToString("0.00");



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
                if (tb3 != null && columnName == "Heat Code")
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

                if (tb3 != null && columnName == "Rejection Reasons")
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
                if (columnName == "Heat Code")
                {
                    var Prodname = (from d in db.Forging_JobCardRMs where d.Job_CardNo == R1.Cells["Job_Card"].Value.ToString() select new { d.Heat_Code }).ToList();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Heat_Code");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Heat_Code);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
                    }
                }
                
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
                            
                if (columnName == "Operator_Name")
                {
                    var Prodname = (from d in db.Forging_Employees where d.Department =="Forging" && d.Designation == "Operator" select new { d.Employee_Name }).ToList().Distinct();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Operator_Name");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Employee_Name);
                    }
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        coll.Add(dt.Rows[i][0].ToString());
                    }
                }
                            
                if (columnName == "Rejection Reasons")
                {
                    var Prodname = (from d in db.Attributes_Datas where d.Head_Name == "Rejection Reasons" select new { d.Descr }).ToList().Distinct();
                    DataTable dt = new DataTable();
                    dt.Columns.Add("Rejection_Reason");
                    foreach (var item in Prodname)
                    {
                        dt.Rows.Add(item.Descr);
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

        private void cmbProcess_Leave(object sender, EventArgs e)
        {
            try
            {
                //Sub Process
                //if (cmbProcess.Text != "")
                //{
                //    int PId = Convert.ToInt32(cmbProcess.SelectedValue);
                //    var pStatus = (from m in db.Forging_ProcessMasters where m.Company_ID == logIn.company && m.Main_Process_ID == PId select new { m.ID, m.Process_Name }).Distinct().ToList();
                //    if (pStatus.Count > 0)
                //    {
                //        cmbSubProcess.DataSource = pStatus;
                //        cmbSubProcess.ValueMember = "ID";
                //        cmbSubProcess.DisplayMember = "Process_Name";
                //    }
                //    else
                //    {
                //        cmbSubProcess.DataSource = null;
                //    }

                //}
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
                if ((from a in db.Forging_ForgingReport_Masters where a.Company_ID == logIn.company && a.Report_No == txtvchno.Text select a).Count() > 0)
                {
                    myString = txtvchno.Text;
                    db.Sp_delete_Production_Forging(logIn.company, txtvchno.Text);

                    Forging_ForgingReport_Master pb = new Forging_ForgingReport_Master();
                    pb.Report_No = txtvchno.Text;
                    pb.Report_Date = dpdate.Value;
                    pb.Shift = cmbShift.Text;
                    pb.Working_Hrs = (txtWorkingHrs.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtWorkingHrs.Text);

                    pb.Process_Name = Convert.ToInt32(cmbProcess.SelectedValue);
                    pb.Sub_Process_Name = Convert.ToInt32(cmbProcess.SelectedValue);
                    //pb.Machi//ne_Name = Convert.ToInt32(cmbMachineID.SelectedValue);
                   // pb.StartTime = (txtStartTime.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtStartTime.Text);
                   // pb.EndTime = (txtEdnTime.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtEdnTime.Text);
                    pb.Allocated_Hrs = (txtAllocatedHrs.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtAllocatedHrs.Text);
                    pb.Ideal_Hrs = (txtIdealHrs.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtIdealHrs.Text);
                    pb.Die_Setting_Time = (txtDieSetting.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtDieSetting.Text);
                    pb.Power_Cut = (txtPowerCut.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtPowerCut.Text);
                    pb.BreakDown_Hrs = (txtBreakDown.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtBreakDown.Text);
                    pb.No_Plan = (txtNoPlan.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtNoPlan.Text);
                    pb.Man_Power = (txtManPower.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtManPower.Text);
                    pb.Other_Hrs = (txtOthers.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtOthers.Text);
                    pb.No_Reason_Hrs = (txtNoReasonHrs.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtNoReasonHrs.Text);
                    pb.IPH_Eff_Per = (txtIPHEff.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtIPHEff.Text);

                    pb.Others_Reason = (txtOthersReason.Text == null) ? "" : txtOthersReason.Text;
                    pb.Supervisor = (cmbSupervisor.Text == null) ? "" : cmbSupervisor.Text;
                    pb.Remarks = (txtRemarks.Text == null) ? "" : txtRemarks.Text;
                    pb.Company_ID = logIn.company;
                    pb.Modified_By = logIn.username + "-" + DateTime.Now;
                    db.Forging_ForgingReport_Masters.InsertOnSubmit(pb);
                    db.SubmitChanges();

                    //}

                    //db.SubmitChanges();

                    //Save Child Data (Job Card)
                    for (int i = 0; i < dgJobCardData.Rows.Count - 1; i++)
                    {
                        Forging_Forging_Child SC = new Forging_Forging_Child();
                        var d1 = (from a in db.Forging_ForgingReport_Masters where a.Report_No == myString && a.Company_ID == logIn.company select new { a.id }).ToList();
                        SC.Report_Master_ID = d1[0].id;
                        SC.Job_CardNo = (dgJobCardData.Rows[i].Cells["Job_Card"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Job_Card"].Value).ToString();

                        if (dgJobCardData.Rows[i].Cells["Job_card_date"].Value.ToString() != "")
                        {
                            DateTime t = Convert.ToDateTime(dgJobCardData.Rows[i].Cells["Job_card_date"].Value.ToString());
                            string t1 = t.ToString("dd/MM/yyyy");
                            SC.J_Date = t;// DateTime.ParseExact(eDate, "MM/dd/yyyy", null); ;
                        }
                        SC.FG_Item_Code = Convert.ToInt32(dgJobCardData.Rows[i].Cells["Item_Code"].Value);
                        SC.FG_Item_Name = (dgJobCardData.Rows[i].Cells["Item_Name"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Item_Name"].Value).ToString();
                        SC.Job_CardQty = (dgJobCardData.Rows[i].Cells["Job_Card_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Job_Card_Qty"].Value);
                        SC.Balance_Qty = (dgJobCardData.Rows[i].Cells["Pending_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Pending_Qty"].Value);
                        SC.Heat_Code = (dgJobCardData.Rows[i].Cells["Heat_Code"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Heat_Code"].Value).ToString();
                        SC.Qty_Produced = (dgJobCardData.Rows[i].Cells["Qty_Produced"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Qty_Produced"].Value);
                        SC.Qty_Rejected = (dgJobCardData.Rows[i].Cells["Qty_Rejected"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Qty_Rejected"].Value);
                        SC.Qty_Accepted = (dgJobCardData.Rows[i].Cells["Qty_Accepted"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Qty_Accepted"].Value);
                        SC.Remarks = (dgJobCardData.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Remarks"].Value).ToString();
                        SC.Prod_Rate = (dgJobCardData.Rows[i].Cells["PR"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["PR"].Value);
                        SC.Qty_Rework = (dgJobCardData.Rows[i].Cells["Qty_Rework"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Qty_Rework"].Value);
                        SC.Machine_name = (dgJobCardData.Rows[i].Cells["Machine_name"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Machine_name"].Value).ToString();

                        SC.Start_Time = (dgJobCardData.Rows[i].Cells["Start_Time"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Start_Time"].Value);
                        SC.End_Time = (dgJobCardData.Rows[i].Cells["End_Time"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["End_Time"].Value);
                        SC.End_Time = (dgJobCardData.Rows[i].Cells["End_Time"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["End_Time"].Value);
                        SC.Actual_Hrs = (dgJobCardData.Rows[i].Cells["Actual_Hrs"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Actual_Hrs"].Value);

                        


                        SC.Ideal_Hrs = (dgJobCardData.Rows[i].Cells["Ideal_Hrs"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Ideal_Hrs"].Value);
                        SC.Power_Open_Reading = (dgJobCardData.Rows[i].Cells["Power_Open_Reading"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Power_Open_Reading"].Value);
                        SC.Power_Closing_Reading = (dgJobCardData.Rows[i].Cells["Power_Closing_Reading"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Power_Closing_Reading"].Value);
                        SC.Total_Units = (dgJobCardData.Rows[i].Cells["Total_Units"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Total_Units"].Value);

                        SC.Operator_Name = (dgJobCardData.Rows[i].Cells["Operator_Name"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Operator_Name"].Value).ToString();

                        
                        db.Forging_Forging_Childs.InsertOnSubmit(SC);
                    }
                    db.SubmitChanges();
                    MessageBox.Show("Record Updated Sucessfully");
                    clear();
                    return;
                }
                else
                {
                    AutoincrementId();
                    Forging_ForgingReport_Master pb = new Forging_ForgingReport_Master();
                    pb.Report_No = txtvchno.Text;
                    pb.Report_Date = dpdate.Value;
                    pb.Shift = cmbShift.Text;
                    pb.Working_Hrs = (txtWorkingHrs.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtWorkingHrs.Text);

                    pb.Process_Name = Convert.ToInt32(cmbProcess.SelectedValue);
                    pb.Sub_Process_Name = Convert.ToInt32(cmbProcess.SelectedValue);
                    //pb.Machine_Name = Convert.ToInt32(cmbMachineID.SelectedValue);
                   // pb.StartTime = (txtStartTime.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtStartTime.Text);
                    //pb.EndTime = (txtEdnTime.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtEdnTime.Text);
                    pb.Allocated_Hrs = (txtAllocatedHrs.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtAllocatedHrs.Text);
                    pb.Ideal_Hrs = (txtIdealHrs.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtIdealHrs.Text);
                    pb.Die_Setting_Time = (txtDieSetting.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtDieSetting.Text);
                     pb.Power_Cut = (txtPowerCut.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtPowerCut.Text);
                    pb.BreakDown_Hrs = (txtBreakDown.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtBreakDown.Text);
                    pb.No_Plan = (txtNoPlan.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtNoPlan.Text);
                    pb.Man_Power = (txtManPower.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtManPower.Text);
                    pb.Other_Hrs = (txtOthers.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtOthers.Text);
                    pb.No_Reason_Hrs = (txtNoReasonHrs.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtNoReasonHrs.Text);
                    pb.IPH_Eff_Per = (txtIPHEff.Text == "") ? Convert.ToDecimal("00") : Convert.ToDecimal(txtIPHEff.Text);

                    pb.Others_Reason = (txtOthersReason.Text == null) ? "" : txtOthersReason.Text;
                    pb.Supervisor = (cmbSupervisor.Text == null) ? "" : cmbSupervisor.Text;
                    pb.Remarks = (txtRemarks.Text == null) ? "" : txtRemarks.Text;
                    pb.Company_ID = logIn.company;
                    pb.Modified_By = logIn.username + "-" + DateTime.Now;
                    db.Forging_ForgingReport_Masters.InsertOnSubmit(pb);
                    db.SubmitChanges();


                    //Save Child Data (Job Card)
                    for (int i = 0; i < dgJobCardData.Rows.Count - 1; i++)
                    {
                        Forging_Forging_Child SC = new Forging_Forging_Child();
                        var d1 = (from a in db.Forging_ForgingReport_Masters where a.Report_No == myString && a.Company_ID == logIn.company select new { a.id }).ToList();
                        SC.Report_Master_ID = d1[0].id;
                        SC.Job_CardNo = (dgJobCardData.Rows[i].Cells["Job_Card"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Job_Card"].Value).ToString();

                        if (dgJobCardData.Rows[i].Cells["Job_card_date"].Value.ToString() != "")
                        {
                            DateTime t = Convert.ToDateTime(dgJobCardData.Rows[i].Cells["Job_card_date"].Value.ToString());
                            string t1 = t.ToString("dd/MM/yyyy");
                            SC.J_Date = t;// DateTime.ParseExact(eDate, "MM/dd/yyyy", null); ;
                        }
                        SC.FG_Item_Code = Convert.ToInt32(dgJobCardData.Rows[i].Cells["Item_Code"].Value);
                        SC.FG_Item_Name = (dgJobCardData.Rows[i].Cells["Item_Name"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Item_Name"].Value).ToString();
                        SC.Job_CardQty = (dgJobCardData.Rows[i].Cells["Job_Card_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Job_Card_Qty"].Value);
                        SC.Balance_Qty = (dgJobCardData.Rows[i].Cells["Pending_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Pending_Qty"].Value);
                        SC.Heat_Code = (dgJobCardData.Rows[i].Cells["Heat_Code"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Heat_Code"].Value).ToString();
                        SC.Qty_Produced = (dgJobCardData.Rows[i].Cells["Qty_Produced"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Qty_Produced"].Value);
                        SC.Qty_Rejected = (dgJobCardData.Rows[i].Cells["Qty_Rejected"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Qty_Rejected"].Value);
                        SC.Qty_Accepted = (dgJobCardData.Rows[i].Cells["Qty_Accepted"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Qty_Accepted"].Value);
                        SC.Remarks = (dgJobCardData.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Remarks"].Value).ToString();
                        SC.Prod_Rate = (dgJobCardData.Rows[i].Cells["PR"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["PR"].Value);
                        SC.Qty_Rework = (dgJobCardData.Rows[i].Cells["Qty_Rework"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Qty_Rework"].Value);
                        SC.Machine_name = (dgJobCardData.Rows[i].Cells["Machine_name"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Machine_name"].Value).ToString();

                        SC.Start_Time = (dgJobCardData.Rows[i].Cells["Start_Time"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Start_Time"].Value);
                        SC.End_Time = (dgJobCardData.Rows[i].Cells["End_Time"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["End_Time"].Value);
                        SC.End_Time = (dgJobCardData.Rows[i].Cells["End_Time"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["End_Time"].Value);

                        SC.Actual_Hrs = (dgJobCardData.Rows[i].Cells["Actual_Hrs"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Actual_Hrs"].Value);


                        SC.Ideal_Hrs = (dgJobCardData.Rows[i].Cells["Ideal_Hrs"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Ideal_Hrs"].Value);
                        SC.Power_Open_Reading = (dgJobCardData.Rows[i].Cells["Power_Open_Reading"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Power_Open_Reading"].Value);
                        SC.Power_Closing_Reading = (dgJobCardData.Rows[i].Cells["Power_Closing_Reading"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Power_Closing_Reading"].Value);
                        SC.Total_Units = (dgJobCardData.Rows[i].Cells["Total_Units"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Total_Units"].Value);

                        SC.Operator_Name = (dgJobCardData.Rows[i].Cells["Operator_Name"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Operator_Name"].Value).ToString();

                        db.Forging_Forging_Childs.InsertOnSubmit(SC);
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
            //cmbMachineID.SelectedValue = -1;
            cmbProcess.SelectedValue = -1;
           // cmbSubProcess.SelectedValue = -1;
            txtOthersReason.Text = "";
            cmbSupervisor.Text = "";
            txtRemarks.Text = "";
            txtWorkingHrs.Text = "";
           // txtStartTime.Text = "";
           // txtEdnTime.Text = "";
            txtAllocatedHrs.Text = "";
            txtIdealHrs.Text = "";
            txtDieSetting.Text = "";            
            txtPowerCut.Text = "";
            txtBreakDown.Text = "";
            txtNoPlan.Text = "";
            txtManPower.Text = "";
            txtOthers.Text = "";
            txtManPower.Text = "";
            txtIPHEff.Text = "";
            txtNoReasonHrs.Text = "";
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
                ProductionManagement.frmForging_Production_ForgingList obj = new frmForging_Production_ForgingList();
                if (obj.ShowDialog() == DialogResult.OK)
                {
                    int Report_ID = 0;
                    txtvchno.Text = ProductionManagement.frmForging_Production_ForgingList.Voucherno;
                    var sa = (from sq in db.Forging_ForgingReport_Masters
                              where sq.Company_ID == logIn.company && sq.Report_No == txtvchno.Text
                              select new
                              {
                                  sq.id,
                                  sq.Report_Date,
                                  sq.Shift,
                                  sq.Machine_Name,
                                  sq.Others_Reason,
                                  sq.Supervisor,
                                  sq.Remarks,
                                  sq.Created_By,
                                  sq.Modified_By,
                                  sq.Working_Hrs,                                  
                                  sq.Allocated_Hrs,
                                  sq.Ideal_Hrs,                                  
                                  sq.No_Plan,
                                  sq.Power_Cut,
                                  sq.Sub_Process_Name,
                                  sq.Process_Name,
                                  sq.Man_Power,
                                  sq.Other_Hrs,
                                  sq.Die_Setting_Time,
                                  sq.BreakDown_Hrs,
                                  sq.No_Reason_Hrs,
                                  sq.IPH_Eff_Per

                              }).ToList();
                    if (sa.Count > 0)
                    {
                        Report_ID = sa[0].id;
                        dpdate.Value = Convert.ToDateTime(sa[0].Report_Date);
                       // cmbMachineID.SelectedValue = sa[0].Machine_Name;
                        cmbShift.Text = sa[0].Shift;
                        txtOthersReason.Text = sa[0].Others_Reason;
                        cmbSupervisor.Text = sa[0].Supervisor;
                        txtRemarks.Text = sa[0].Remarks.ToString();
                        lblCreatedBy.Text = sa[0].Created_By;
                        lblModified.Text = sa[0].Modified_By;
                        txtWorkingHrs.Text = sa[0].Working_Hrs.ToString();
                        cmbProcess.SelectedValue = sa[0].Process_Name;
                        cmbProcess_Leave(sender, e);

                       // cmbSubProcess.SelectedValue = sa[0].Sub_Process_Name;
                      //  txtStartTime.Text = sa[0].StartTime.ToString();
                      //  txtEdnTime.Text = sa[0].EndTime.ToString();
                        txtAllocatedHrs.Text = sa[0].Allocated_Hrs.ToString();
                        txtIdealHrs.Text = sa[0].Ideal_Hrs.ToString();
                        txtDieSetting.Text = sa[0].Die_Setting_Time.ToString();                        
                        txtPowerCut.Text = sa[0].Power_Cut.ToString();
                        txtBreakDown.Text = sa[0].BreakDown_Hrs.ToString();
                        txtNoPlan.Text = sa[0].No_Plan.ToString();
                        txtManPower.Text = sa[0].Man_Power.ToString();
                        txtOthers.Text = sa[0].Other_Hrs.ToString();
                        txtManPower.Text = sa[0].Man_Power.ToString();
                        txtNoReasonHrs.Text = sa[0].No_Reason_Hrs.ToString();
                        txtIPHEff.Text = sa[0].IPH_Eff_Per.ToString();



                    }
                    var ca = (from sq in db.Forging_Forging_Childs
                              where sq.Report_Master_ID == Report_ID
                              select new
                              {

                                  Job_Card = sq.Job_CardNo,
                                  Job_card_date = sq.J_Date,
                                  Item_Code = sq.FG_Item_Code,
                                  Item_Name = sq.FG_Item_Name,
                                  Job_Card_Qty = sq.Job_CardQty,
                                  Pending_Qty = sq.Balance_Qty,
                                  Heat_Code = sq.Heat_Code,
                                  PR = sq.Prod_Rate,
                                  Qty_Produced = sq.Qty_Produced,
                                  Qty_Rejected = sq.Qty_Rejected,
                                  Qty_Accepted = sq.Qty_Accepted,
                                  Qty_Rework = sq.Qty_Rework,
                                  sq.Machine_name,
                                  Ideal_Hrs = sq.Ideal_Hrs,
                                  sq.Start_Time,
                                  sq.End_Time,                                  
                                  sq.Actual_Hrs,
                                  sq.Power_Open_Reading,
                                  sq.Power_Closing_Reading,
                                  sq.Total_Units,
                                  sq.Operator_Name,
                                  //RM_End_Piece = sq.RM_End_Piece,                                 
                                  sq.Remarks
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
            ReportNo = txtvchno.Text;
            Operation = "Forging";

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
            decimal NoPlan = (txtNoPlan.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtNoPlan.Text);
            decimal ManPower = (txtManPower.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtManPower.Text);
            decimal Others = (txtOthers.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtOthers.Text);


                decimal TotHrs =DieSetting+PowerCut+BreakDown+NoPlan+ManPower+Others ;
           txtTotalHrs.Text = (TotHrs/60).ToString("0.00");
            decimal WHrs = (txtWorkingHrs.Text == "") ? Convert.ToDecimal(0) : Convert.ToDecimal(txtWorkingHrs.Text);

            decimal TotAvblHrs = WHrs - (TotHrs / 60);
            txtAllocatedHrs.Text = TotAvblHrs.ToString("0.00");
            decimal IPHrs = Convert.ToDecimal(txtIdealHrs.Text);
            decimal NoReasonHrs = (TotAvblHrs - IPHrs);
            txtNoReasonHrs.Text = NoReasonHrs.ToString("0.00");
            if (WHrs > 0)
            {
                decimal IPHEff = IPHrs / WHrs * 100;
                txtIPHEff.Text = IPHEff.ToString("0.00");
            }
            
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
                db.Sp_delete_Production_Forging(logIn.company, txtvchno.Text);

                SqlCommand cmd1 = new SqlCommand("delete  from [Forging_BreakDownData] where Report_ID =@RepID and Operation_Name = @oprnName", con);
                cmd1.Parameters.AddWithValue("@RepID", txtvchno.Text);
                cmd1.Parameters.AddWithValue("@oprnName", "Forging");

                if (con.State != ConnectionState.Open)
                    con.Open();
                //con.Open();
                cmd1.ExecuteNonQuery();
                con.Close();



                MessageBox.Show("Record Deleted Successfully");
                clear();
            }
        }

        private void txtNoReasonHrs_TextChanged(object sender, EventArgs e)
        {

        }

        private void dgJobCardData_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
           

        }
    }
}
