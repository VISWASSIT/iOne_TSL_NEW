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
    public partial class frmForge_RFPL_JobCard : Form
    {
        DataClasses1DataContext db = new DataClasses1DataContext();
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["iOneConnection"].ConnectionString);

        public static string ReportNo, Operation;

        public frmForge_RFPL_JobCard()
        {
            InitializeComponent();
        }

        private void frmForge_Production_Forging_Load(object sender, EventArgs e)
        {
            lblCreatedBy.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            //lbldt1.Text = Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");
            lblModified.Text = logIn.username + '-' + Convert.ToDateTime(DateTime.Now).ToString("dd/MM/yyyy HH:mm:ss tt");

            AutoincrementId();
            if (FrmForge_JobCardList.editMode == true)
            {
                bindedit();
                FrmForge_JobCardList.editMode = false;

            }
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

           

        }
        public void AutoincrementId()
        {
            try
            {

                var result = db.Sp_autoincrement_JobCard_RFPL(logIn.company, logIn.fy_Start_Date, logIn.fy_End_Date);
                txtvchno.Text = result.FirstOrDefault().Jc_no;
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
                    if (columnName == "Qty_Produced" || columnName == "Qty_Rejected" || columnName == "Qty_Rework")
                {
                    //int Itemcode = Convert.ToInt32(R1.Cells["Item_Code"].Value.ToString());
                    if (R1.Cells["Job_Card"].Value != null)
                    {
                        decimal Qty_Produced = (R1.Cells["Qty_Produced"].Value == "" || R1.Cells["Qty_Produced"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty_Produced"].Value);
                        decimal Qty_Rejected = (R1.Cells["Qty_Rejected"].Value == "" || R1.Cells["Qty_Rejected"].Value == null || R1.Cells["Qty_Rejected"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty_Rejected"].Value);
                        decimal Qty_Pending = (R1.Cells["Pending_Qty"].Value == "" || R1.Cells["Pending_Qty"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Pending_Qty"].Value);

                        decimal Qty_Rework = (R1.Cells["Qty_Rework"].Value == "" || R1.Cells["Qty_Rework"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(R1.Cells["Qty_Rework"].Value);

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
                                Qty_Accepted = Qty_Produced - Qty_Rejected - Qty_Rework;
                                R1.Cells["Qty_Accepted"].Value = Qty_Accepted.ToString("0.00");
                            }
                        }
                        else
                        {
                            MessageBox.Show("Qty Produced Cannot Be Greater Than Qty Pending");
                            R1.Cells["Qty_Produced"].Value = 0;
                            R1.Cells["Qty_Rejected"].Value = 0;
                            R1.Cells["Qty_Rework"].Value = 0;
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
                        //for (int i = 0; i < dgJobCardData.Rows.Count - 1; i++)
                        //{
                        //    x += (dgJobCardData.Rows[i].Cells["Ideal_Hrs"].Value.ToString() == "" || dgJobCardData.Rows[i].Cells["Ideal_Hrs"].Value == null || dgJobCardData.Rows[i].Cells["Ideal_Hrs"].Value == DBNull.Value) ? Convert.ToDecimal(0) : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Ideal_Hrs"].Value);

                        //}
                        
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
                if ((from a in db.Forging_Job_Card_RFPLs where a.Company_ID == logIn.company && a.Job_Card_No == txtvchno.Text select a).Count() > 0)
                {
                    myString = txtvchno.Text;
                    db.Sp_delete_RFPL_JobCard_Forging(logIn.company, txtvchno.Text);
                }
                else
                {
                    AutoincrementId();
                }
                //Save Child Data (Job Card)
                for (int i = 0; i < dgJobCardData.Rows.Count - 1; i++)
                {
                    Forging_Job_Card_RFPL SC = new Forging_Job_Card_RFPL();
                    SC.Job_Card_No = txtvchno.Text;
                    SC.Job_Card_Date = dpdate.Value;
                    SC.Prod_Shift = cmbShift.Text;
                    SC.PlanningRef = (dgJobCardData.Rows[i].Cells["PlanningRef"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["PlanningRef"].Value).ToString();

                    SC.MO_Sno = (dgJobCardData.Rows[i].Cells["MO_Sno"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgJobCardData.Rows[i].Cells["MO_Sno"].Value);
                    SC.Mo_No = (dgJobCardData.Rows[i].Cells["Mo_No"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Mo_No"].Value).ToString();
                    var d1 = (from a in db.Forge_MFG_Order_Masters where a.MO_No == dgJobCardData.Rows[i].Cells["Mo_No"].Value.ToString() && a.Company_ID == logIn.company select new { a.id }).ToList();
                    SC.MO_Master_ID = d1[0].id;


                    //SC.Mo_Date = (dgProducts.Rows[i].Cells["Mo_No"].Value == null) ? "" : (dgProducts.Rows[i].Cells["Mo_No"].Value).ToString();
                    SC.Prod_Code = (dgJobCardData.Rows[i].Cells["Item_No"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgJobCardData.Rows[i].Cells["Item_No"].Value);

                    //SC.Item_Description = (dgJobCardData.Rows[i].Cells["Item_Description"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Item_Description"].Value).ToString();
                    //SC.MaterialGrade = (dgJobCardData.Rows[i].Cells["Item_Grade"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Item_Grade"].Value).ToString();
                    SC.Job_Card_Qty = (dgJobCardData.Rows[i].Cells["BalQty"].Value == DBNull.Value) ? Convert.ToInt32("00") : Convert.ToInt32(dgJobCardData.Rows[i].Cells["BalQty"].Value);
                    //SC.Forging_Size = (dgJobCardData.Rows[i].Cells["ForgingSize"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["ForgingSize"].Value).ToString();
                    //SC.ForgingWt = (dgJobCardData.Rows[i].Cells["Forging_Wt"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["Forging_Wt"].Value);
                    //SC.RMSec = (dgJobCardData.Rows[i].Cells["RM_Sec"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["RM_Sec"].Value).ToString();

                    //SC.RMAvbl = (dgJobCardData.Rows[i].Cells["RM_WT"].Value == DBNull.Value) ? Convert.ToDecimal("00") : Convert.ToDecimal(dgJobCardData.Rows[i].Cells["RM_WT"].Value);
                    SC.Stage = (dgJobCardData.Rows[i].Cells["stage"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["stage"].Value).ToString();
                    //SC.Remarks = (dgJobCardData.Rows[i].Cells["Remarks"].Value == null) ? "" : (dgJobCardData.Rows[i].Cells["Remarks"].Value).ToString();
                    SC.Company_ID = logIn.company;
                    SC.Created_By = lblCreatedBy.Text;
                    SC.Modified_BY = logIn.username + "-" + DateTime.Now;

                    db.Forging_Job_Card_RFPLs.InsertOnSubmit(SC);
                }
                db.SubmitChanges();
                MessageBox.Show("Record Updated Sucessfully");
                //clear();
                return;               
                   
                
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

        public void bindedit()
        {
            try
            {
                txtvchno.Text = ProductionManagement.Transactions.FrmForge_JobCardList.SO_No;
                String myString = "";
                myString = txtvchno.Text;
                var sa = (from sq in db.Forging_Job_Card_RFPLs
                              where sq.Company_ID == logIn.company && sq.Job_Card_No == txtvchno.Text
                              select new
                              {
                                  sq.id,
                                  sq.Job_Card_Date,
                                  sq.Prod_Shift,
                                  sq.Prod_Machine,                                  
                                  sq.Created_By,
                                  sq.Modified_BY,
                                  

                              }).ToList();
                    if (sa.Count > 0)
                    {
                        //myString = sa[0].id;
                        dpdate.Value = Convert.ToDateTime(sa[0].Job_Card_Date);
                       // cmbMachineID.SelectedValue = sa[0].Machine_Name;
                        cmbShift.Text = sa[0].Prod_Shift;
                       
                        //txtRemarks.Text = sa[0].Remarks.ToString();
                        lblCreatedBy.Text = sa[0].Created_By;
                        lblModified.Text = sa[0].Modified_BY;
                       
                        //txtPlanningRef.Text = sa[0].Plan_Ref_No;
                        //cmbProcess_Leave(sender, e);

                       // cmbSubProcess.SelectedValue = sa[0].Sub_Process_Name;
                      //  txtStartTime.Text = sa[0].StartTime.ToString();
                      //  txtEdnTime.Text = sa[0].EndTime.ToString();
                        



                    }
                    var ca = (from sq in db.Forging_Job_Card_RFPLs
                              join k in db.Forge_Mfg_Order_Childs on new { x1 = sq.MO_Master_ID, x2 = sq.Prod_Code } equals new { x1 = k.MO_Master_ID, x2 = k.Prod_Code }
                              join m in db.Forge_MFG_Order_Masters on sq.Mo_No equals m.MO_No                              
                              join c in db.Supplier_informations on m.Customer_Name equals c.ID
                              join f in db.Forge_ProdPlannings on new { sq.Mo_No, sq.Prod_Code, sq.PlanningRef } equals new { f.Mo_No, f.Prod_Code, f.PlanningRef }
                              

                              where sq.Job_Card_No == myString
                              select new
                              {
                                  MO_No = sq.Mo_No,
                                  Customer_Name = c.Supplier_Name,
                                  MO_Sno = sq.MO_Sno,
                                  Item_No = sq.Prod_Code,
                                  Item_Description = k.Product_Description,
                                  Item_Grade = k.Prod_Grade,
                                  BalQty = sq.Job_Card_Qty,
                                  ForgingSize = f.Forging_Size,
                                  Forging_Wt = f.ForgingWt,
                                  RM_Sec = f.RMSec,
                                  RM_WT = f.RMAvbl,
                                  stage = sq.Stage,
                                  sq.PlanningRef,
                                  Remarks = ""


                              });
                    SqlCommand cmd3 = (SqlCommand)db.GetCommand(ca);
                    SqlDataAdapter da3 = new SqlDataAdapter(cmd3);
                    DataTable dt1 = new DataTable();
                    da3.Fill(dt1);
                    if (dt1.Rows.Count > 0)
                        dgJobCardData.DataSource = dt1;

                
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
            
           // //Get sum of ideal Hrs

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

        private void txtPlanningRef_Leave(object sender, EventArgs e)
        {
            try
            {

                SqlCommand cmd2 = new SqlCommand("sp_ForgingPlan_VsReport", con);
                cmd2.CommandType = CommandType.StoredProcedure;
                cmd2.Parameters.AddWithValue("@compname", logIn.company);
                cmd2.Parameters.AddWithValue("@PlanningRef", txtPlanningRef.Text);
                SqlDataAdapter da2 = new SqlDataAdapter(cmd2);
                //DataSet ds2 = new DataSet();
                DataTable ds2 = new DataTable();
                // da2.Fill(ds2, "x");
                da2.Fill(ds2);
                if (ds2.Rows.Count > 0)
                {
                    dgJobCardData.DataSource = ds2;
                }
                else
                {
                    MessageBox.Show("Either Invalid Ref No Entered  or Already Report Generated");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void btnGetPlanData_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtexisting = new DataTable();               

                ProductionManagement.Transactions.getPlanningData form = new ProductionManagement.Transactions.getPlanningData();
                form.ShowDialog();
                if (dgJobCardData.Rows.Count > 1)
                {
                    //dtexisting.Rows.Clear();
                    //dtexisting.Columns.Clear();
                    dtexisting.Rows.Clear();
                    dtexisting.Columns.Clear();
                    dtexisting.Columns.Add("MO_No", typeof(string));
                    dtexisting.Columns.Add("Customer_Name", typeof(string));
                    dtexisting.Columns.Add("MO_Sno", typeof(string));
                    dtexisting.Columns.Add("Item_No", typeof(string));
                    dtexisting.Columns.Add("Item_Description", typeof(string));
                    dtexisting.Columns.Add("Item_Grade", typeof(string));
                    dtexisting.Columns.Add("BalQty", typeof(string));
                    dtexisting.Columns.Add("ForgingSize", typeof(string));
                    dtexisting.Columns.Add("Forging_Wt", typeof(string));
                    dtexisting.Columns.Add("RM_Sec", typeof(string));
                    dtexisting.Columns.Add("RM_WT", typeof(string));
                    dtexisting.Columns.Add("stage", typeof(string));
                    dtexisting.Columns.Add("PlanningRef", typeof(string));
                    
                    dtexisting.Columns.Add("Remarks", typeof(string));
                    //for (int i = 0; i < dgProducts.ColumnCount ; i++)
                    //{
                    //    int columnIndex = i;
                    //    string columnName = dgProducts.Columns[columnIndex].Name;
                    //    dtexisting.Columns.Add(columnName, typeof(string));
                    //}

                    for (int i = 0; i < dgJobCardData.Rows.Count - 1; i++)
                    {
                        DataRow dr;
                        dr = dtexisting.NewRow();
                        //for (int c = 0; c < dgProducts.ColumnCount; c++)
                        //{
                        //    //MessageBox.Show(dgProducts.Columns[c].Name);
                        //    if (dgProducts.Rows[i].Cells[c].Value != null)
                        //    {
                        //        dr[c] = dgProducts.Rows[i].Cells[c].Value.ToString();
                        //    }
                        //}
                        dr["MO_No"] = dgJobCardData.Rows[i].Cells["MO_No"].Value.ToString();
                        dr["Customer_Name"] = dgJobCardData.Rows[i].Cells["Customer_Name"].Value.ToString();
                        dr["MO_Sno"] = dgJobCardData.Rows[i].Cells["MO_Sno"].Value.ToString();
                        dr["Item_No"] = dgJobCardData.Rows[i].Cells["Item_No"].Value.ToString();
                        dr["Item_Description"] = dgJobCardData.Rows[i].Cells["Item_Description"].Value.ToString();
                        dr["Item_Grade"] = dgJobCardData.Rows[i].Cells["Item_Grade"].Value.ToString();
                        dr["BalQty"] = dgJobCardData.Rows[i].Cells["BalQty"].Value.ToString();
                        dr["ForgingSize"] = dgJobCardData.Rows[i].Cells["ForgingSize"].Value.ToString();
                        dr["Forging_Wt"] = dgJobCardData.Rows[i].Cells["Forging_Wt"].Value.ToString();
                        dr["RM_Sec"] = dgJobCardData.Rows[i].Cells["RM_Sec"].Value.ToString();
                        dr["RM_WT"] = dgJobCardData.Rows[i].Cells["RM_WT"].Value.ToString();
                        dr["stage"] = dgJobCardData.Rows[i].Cells["stage"].Value.ToString();
                        dr["PlanningRef"] = dgJobCardData.Rows[i].Cells["PlanningRef"].Value.ToString();                        
                        if (dgJobCardData.Rows[i].Cells["Remarks"].Value != null)
                        {
                            dr["Remarks"] = (dgJobCardData.Rows[i].Cells["Remarks"].Value.ToString() == "" || dgJobCardData.Rows[i].Cells["Remarks"].Value == null) ? "" : dgJobCardData.Rows[i].Cells["Remarks"].Value.ToString();
                        }


                        dtexisting.Rows.Add(dr);

                    }
                    dtexisting.AcceptChanges();
                }


                if (ioneNet.ProductionManagement.Transactions.getPlanningData.dtgetproducts.Rows.Count > 0)
                {
                    DataTable dt = new DataTable();
                    for (int i = 0; i < dgJobCardData.ColumnCount; i++)
                    {
                        int columnIndex = i;
                        string columnName = dgJobCardData.Columns[columnIndex].Name;
                        dt.Columns.Add(columnName, typeof(string));
                    }
                    //dt.Rows.Add();
                    for (int i = 0; i < ioneNet.ProductionManagement.Transactions.getPlanningData.dtgetproducts.Rows.Count; i++)
                    {
                        string Plan_no = ioneNet.ProductionManagement.Transactions.getPlanningData.dtgetproducts.Rows[i]["PlanningRef"].ToString();
                        string MO_No = ioneNet.ProductionManagement.Transactions.getPlanningData.dtgetproducts.Rows[i]["Mo_No"].ToString();
                        string MO_Sno = ioneNet.ProductionManagement.Transactions.getPlanningData.dtgetproducts.Rows[i]["MO_Sno"].ToString();


                        var getproducts = (from s in db.GetPlanningData_JobCards
                                           where s.MO_Sno == Convert.ToInt32(MO_Sno) && s.Mo_No == MO_No && s.PlanningRef == Plan_no
                                           
                                           select new
                                           { /*s.Prod_HSN_Code, t.Gst_Rate, s.Prod_Name, u.Uom_Descr*/

                                               MO_No = s.Mo_No,
                                               Customer_Name = s.Supplier_Name,
                                               MO_Sno = s.MO_Sno,
                                               Item_No = s.Prod_Code,
                                               Item_Description = s.Product_Description,
                                               Item_Grade = s.MaterialGrade,
                                               BalQty = s.Qty,
                                               ForgingSize = s.Forging_Size,
                                               Forging_Wt = s.ForgingWt,
                                               RM_Sec = s.RMSec,
                                               RM_WT = s.RMAvbl,
                                               stage = s.Stage,
                                               s.PlanningRef,
                                               Remarks = ""
                                           }).ToList();
                        dt.Rows.Add(getproducts[0].MO_No, getproducts[0].Customer_Name, getproducts[0].MO_Sno, getproducts[0].Item_No, getproducts[0].Item_Description, getproducts[0].Item_Grade, getproducts[0].BalQty, getproducts[0].ForgingSize, getproducts[0].Forging_Wt, getproducts[0].RM_Sec, getproducts[0].RM_WT, getproducts[0].stage, getproducts[0].PlanningRef, getproducts[0].Remarks);


                    }

                    dtexisting = dtexisting.AsEnumerable().Union(dt.AsEnumerable()).CopyToDataTable();
                    //dtexisting = dtexisting.AsEnumerable().Union(dtgetfinalprducts.AsEnumerable()).CopyToDataTable();

                    dgJobCardData.DataSource = dtexisting;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {

        }

        private void dgJobCardData_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
           

        }
    }
}
